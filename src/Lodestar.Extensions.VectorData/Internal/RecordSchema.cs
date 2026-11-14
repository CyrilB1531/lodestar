using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>Where a record keeps its key, its vector and the text the keyword half indexes.</summary>
/// <remarks>
/// Read once per collection and held, because reflection per record would dominate every
/// rebuild. A definition, when one is given, is the whole answer: it replaces the attributes
/// rather than adding to them, so a caller who describes a schema at runtime gets exactly
/// what they described.
/// </remarks>
internal sealed class RecordSchema<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private const string KeyAttribute = "VectorStoreKey";

    private readonly PropertyInfo _key;

    private readonly Searched _vectorSearched;
    private readonly Searched? _fullTextSearched;

    // What a hybrid search's refusal names, formatted only when one is refused.
    private readonly string[] _textNames;
    private readonly bool _fromDefinition;

    private RecordSchema(
        PropertyInfo key, PropertyInfo vector, PropertyInfo? fullText, string[] textNames, bool fromDefinition, int dimension)
    {
        _key = key;
        _textNames = textNames;
        _fromDefinition = fromDefinition;
        Dimension = dimension;
        _vectorSearched = new Searched(vector, "vector");
        _fullTextSearched = fullText is null ? null : new Searched(fullText, "full-text indexed");
    }

    /// <summary>The vector length every record in the collection must carry.</summary>
    public int Dimension { get; }

    /// <summary>Whether exactly one property was marked for full-text search, which is what BM25 needs.</summary>
    /// <remarks>Read from the same field <see cref="KeywordRefusal"/> is, so the two cannot disagree.</remarks>
    public bool HasFullText => _fullTextSearched is not null;

    /// <summary>Why a hybrid search is refused, or <see langword="null"/> when the record has one text to index.</summary>
    /// <remarks>
    /// Several full-text properties are refused here and not at construction (#1212): a vector search
    /// never reads them, and indexing the first found would leave the others out of every keyword match.
    /// </remarks>
    public string? KeywordRefusal => _fullTextSearched is null ? NoKeywordHalf(_textNames, _fromDefinition) : null;

    /// <summary>Reads the schema from <paramref name="definition"/> when one is given, else from the attributes.</summary>
    /// <param name="definition">An explicit description, or <see langword="null"/> to read the attributes.</param>
    /// <exception cref="ArgumentException">No key property or more than one, a key property of a type other than <typeparamref name="TKey"/>, no vector property, a vector of a type other than <c>ReadOnlyMemory&lt;float&gt;</c>, a sole full-text property that is not a <see cref="string"/>, one of them without a getter, a property marked for two of those roles, a definition naming a property the type lacks, or one listing a property twice; a refusal of a given definition names <paramref name="definition"/>.</exception>
    /// <exception cref="NotSupportedException">More than one vector property, or a vector declaring a distance function other than cosine similarity.</exception>
    /// <remarks>An interface record type is read for the members it declares, as reflection lists them, not its bases'.</remarks>
    public static RecordSchema<TKey, TRecord> Create(VectorStoreCollectionDefinition? definition)
    {
        PropertyInfo[] properties = VisibleProperties();

        // No argument describes the attributes, so their refusals name none; a definition's name it.
        return definition is null
            ? FromAttributes(properties)
            : FromDefinition(properties, definition, nameof(definition));
    }

    /// <summary>The record's key, once its vector has been checked against <see cref="Dimension"/>.</summary>
    /// <remarks>What a write calls before it changes anything, so a refused record leaves nothing behind.</remarks>
    /// <param name="record">The record about to be written.</param>
    /// <param name="paramName">The caller's parameter, which the exception names.</param>
    /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
    /// <exception cref="ArgumentException">The key is null, or the vector is not <see cref="Dimension"/> long.</exception>
    public TKey Admit(TRecord record, string paramName)
    {
        if (record is null)
        {
            throw new ArgumentNullException(paramName, "A null record cannot be written.");
        }

        TKey key = _key.GetValue(record) is { } held
            ? (TKey)held
            : throw new ArgumentException(
                $"A {typeof(TRecord).Name} carries a null {_key.Name}, and a record is addressed by its key.",
                paramName);
        int width = VectorOf(record).Length;
        if (width != Dimension)
        {
            throw new ArgumentException(
                $"The record keyed {key} carries a vector of {width} where this collection is "
                + $"{Dimension} wide.", paramName);
        }

        return key;
    }

    /// <summary>The record's vector, which the index holds a copy of.</summary>
    public ReadOnlyMemory<float> VectorOf(TRecord record) =>
        (ReadOnlyMemory<float>)_vectorSearched.Property.GetValue(record)!;

    /// <summary>The text BM25 indexes, or the empty string when the record leaves it null.</summary>
    /// <remarks>Never called when <see cref="HasFullText"/> is false.</remarks>
    public string FullTextOf(TRecord record) => (string?)_fullTextSearched!.Property.GetValue(record) ?? string.Empty;

    /// <summary>Refuses a search's <c>VectorProperty</c> unless it selects the one vector this schema holds (#1212).</summary>
    /// <param name="selector">The options' selector, or <see langword="null"/> when none was set.</param>
    /// <param name="paramName">The caller's options parameter, which the exception names.</param>
    /// <exception cref="ArgumentException"><paramref name="selector"/> selects anything but the vector property.</exception>
    public void CheckVectorSelector(LambdaExpression? selector, string paramName) =>
        _vectorSearched.Check(selector, paramName);

    /// <summary>Refuses a hybrid search's <c>AdditionalProperty</c> unless it selects the full-text property.</summary>
    /// <remarks>Never called when <see cref="HasFullText"/> is false.</remarks>
    /// <param name="selector">The options' selector, or <see langword="null"/> when none was set.</param>
    /// <param name="paramName">The caller's options parameter, which the exception names.</param>
    /// <exception cref="ArgumentException"><paramref name="selector"/> selects anything but the full-text property.</exception>
    public void CheckFullTextSelector(LambdaExpression? selector, string paramName) =>
        _fullTextSearched!.Check(selector, paramName);

    private static RecordSchema<TKey, TRecord> FromAttributes(PropertyInfo[] properties)
    {
        // Each property read once, for the three marks alone: another attribute on it is never built. The
        // vector's attribute is kept, as re-reading it would need a null-forgiving operator one framework refuses.
        var keys = new List<PropertyInfo>();
        var vectors = new List<VectorMember>();
        var texts = new List<PropertyInfo>();
        foreach (PropertyInfo property in properties)
        {
            Classify(property, keys, vectors, texts);
        }

        PropertyInfo key = AtMostOne(keys, p => p.Name, names => TwoKeys(names, paramName: null))
            ?? throw new ArgumentException($"{typeof(TRecord).Name} carries no [{KeyAttribute}] property{OwnMembersOnly}.");
        VectorMember vector = AtMostOne(vectors, p => p.Property.Name, TwoVectors)
            ?? throw new ArgumentException($"{typeof(TRecord).Name} carries no [VectorStoreVector] property{OwnMembersOnly}.");

        return Build(
            key, vector.Property, texts, vector.Attribute.Dimensions, vector.Attribute.DistanceFunction, fromDefinition: false, paramName: null);
    }

    // One property is one role, as a definition lists each property once.
    private static void Classify(
        PropertyInfo property, List<PropertyInfo> keys, List<VectorMember> vectors, List<PropertyInfo> texts)
    {
        bool isKey = property.GetCustomAttribute<VectorStoreKeyAttribute>() is not null;
        VectorStoreVectorAttribute? vectorMark = property.GetCustomAttribute<VectorStoreVectorAttribute>();
        VectorStoreDataAttribute? dataMark = property.GetCustomAttribute<VectorStoreDataAttribute>();
        if ((isKey ? 1 : 0) + (vectorMark is null ? 0 : 1) + (dataMark is null ? 0 : 1) > 1)
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{property.Name} is marked for more than one of key, vector and data; "
                + "each property plays one role.");
        }

        if (isKey)
        {
            keys.Add(property);
        }
        else if (vectorMark is not null)
        {
            vectors.Add(new VectorMember(property, vectorMark));
        }
        else if (dataMark is { IsFullTextIndexed: true })
        {
            texts.Add(property);
        }
    }

    private static RecordSchema<TKey, TRecord> FromDefinition(
        PropertyInfo[] properties, VectorStoreCollectionDefinition definition, string paramName)
    {
        // Every entry names a property of the type, read or not, as the reference pages promise, and names
        // it once: a name listed twice is a malformed definition, not the two properties the counts refuse.
        var named = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);
        foreach (string name in definition.Properties.Where(p => p is not null).Select(p => p.Name))
        {
            if (named.ContainsKey(name))
            {
                throw new ArgumentException(
                    $"The definition lists {name} more than once; each entry describes one property.", paramName);
            }

            named.Add(name, Named(properties, name, paramName));
        }

        VectorStoreKeyProperty key = AtMostOne(
            definition.Properties.OfType<VectorStoreKeyProperty>(), p => p.Name, names => TwoKeys(names, paramName))
            ?? throw new ArgumentException(
                $"The definition names no key property; one {KeyAttribute} property or one "
                + "VectorStoreKeyProperty is what a record is addressed by.", paramName);
        VectorStoreVectorProperty vector = AtMostOne(
            definition.Properties.OfType<VectorStoreVectorProperty>(), p => p.Name, TwoVectors)
            ?? throw new ArgumentException(
                "The definition names no vector property, and a vector store searches by vector.", paramName);
        List<PropertyInfo> texts = [.. definition.Properties
            .OfType<VectorStoreDataProperty>()
            .Where(p => p.IsFullTextIndexed)
            .Select(p => named[p.Name])];

        return Build(
            named[key.Name],
            named[vector.Name],
            texts,
            vector.Dimensions,
            vector.DistanceFunction,
            fromDefinition: true,
            paramName);
    }

    // fromDefinition says where the schema came from, for the messages; paramName is the argument a refusal
    // names, null for the attributes, which no argument describes.
    private static RecordSchema<TKey, TRecord> Build(
        PropertyInfo key,
        PropertyInfo vector,
        List<PropertyInfo> texts,
        int dimension,
        string? distanceFunction,
        bool fromDefinition,
        string? paramName)
    {
        // Checked here rather than left to the casts in Admit and FullTextOf, which would throw
        // InvalidCastException on the first upsert or, for the text, only inside a later search.
        Type keyType = Nullable.GetUnderlyingType(key.PropertyType) ?? key.PropertyType;
        if (!typeof(TKey).IsAssignableFrom(keyType))
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{key.Name} is {key.PropertyType.Name}, and this collection is "
                + $"keyed by {typeof(TKey).Name}; the key property has to be the collection's key type.", paramName);
        }

        // Only a text the keyword half will read is checked: several refuse the hybrid search whatever their types.
        PropertyInfo? text = texts.Count == 1 ? texts[0] : null;
        if (text is not null && text.PropertyType != typeof(string))
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{text.Name} is {text.PropertyType.Name}; a full-text indexed "
                + "property is a string, which is what the keyword half tokenizes.", paramName);
        }

        // Every property the schema holds is read off each record, through a getter it may inherit.
        key = Readable(key, paramName);
        vector = Readable(vector, paramName);
        text = text is null ? null : Readable(text, paramName);

        if (vector.PropertyType != typeof(ReadOnlyMemory<float>))
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{vector.Name} is {vector.PropertyType.Name}; this store "
                + "holds ReadOnlyMemory<float>, which is what EmbeddingIndex takes.", paramName);
        }

        // EmbeddingIndex scores cosine over normalised vectors, and nothing else. Answering a
        // declared distance with a similarity would also turn ScoreThreshold's direction around.
        if (distanceFunction is not null && distanceFunction != DistanceFunction.CosineSimilarity)
        {
            throw new NotSupportedException(
                $"{typeof(TRecord).Name}.{vector.Name} declares {distanceFunction}; this store scores "
                + "cosine similarity over normalised vectors only, and a distance function would also "
                + "invert what ScoreThreshold means. Declare DistanceFunction.CosineSimilarity or none.");
        }

        return new RecordSchema<TKey, TRecord>(
            key, vector, text, [.. texts.Select(p => p.Name)], fromDefinition, dimension);
    }

    /// <summary>Why a record with no full-text property, or several, has no keyword half to fuse.</summary>
    private static string NoKeywordHalf(string[] textNames, bool fromDefinition)
    {
        string subject = (textNames.Length, fromDefinition) switch
        {
            (0, true) => $"The definition of {typeof(TRecord).Name} names no VectorStoreDataProperty with IsFullTextIndexed",
            (0, false) => $"{typeof(TRecord).Name} marks no property [VectorStoreData(IsFullTextIndexed = true)]"
                + $"{OwnMembersOnly}{HiddenText}",
            (_, true) => $"The definition of {typeof(TRecord).Name} names several full-text indexed properties",
            _ => $"{typeof(TRecord).Name} marks several properties full-text indexed",
        };

        return textNames.Length == 0
            ? subject + ", so this collection has no keyword half to fuse with."
            : subject + $" ({Names(textNames)}); this store's keyword half indexes one text per record, "
                + "and indexing the first found would leave the others out of every match.";
    }

    /// <summary>The properties the schema reads: of a base property and the public <c>new</c> member hiding it, the member.</summary>
    /// <remarks>Marked or not, for the attributes and a definition alike; a non-public member is not listed, so the base is read.</remarks>
    private static PropertyInfo[] VisibleProperties() =>
        [.. typeof(TRecord)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetIndexParameters().Length == 0)
            .GroupBy(p => p.Name, StringComparer.Ordinal)
            .Select(named => named.OrderByDescending(p => Depth(p.DeclaringType)).First())];

    // What the refusal adds when a marked base text is there, hidden by a `new` member the schema reads instead.
    // Each base type is read on its own, since a member of the same type hides the base from the full listing.
    private static string HiddenText
    {
        get
        {
            Dictionary<string, PropertyInfo> read = VisibleProperties().ToDictionary(p => p.Name, StringComparer.Ordinal);
            for (Type? type = typeof(TRecord).BaseType; type is not null; type = type.BaseType)
            {
                PropertyInfo? hidden = Array.Find(
                    type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    p => p.GetIndexParameters().Length == 0
                        && p.GetCustomAttribute<VectorStoreDataAttribute>(inherit: false) is { IsFullTextIndexed: true }
                        && read.TryGetValue(p.Name, out PropertyInfo? member)
                        && member.DeclaringType != type
                        && member.GetCustomAttribute<VectorStoreDataAttribute>() is not { IsFullTextIndexed: true });
                if (hidden is not null)
                {
                    return $" of its own; {type.Name}.{hidden.Name} is marked, and a `new` member hides it";
                }
            }

            return string.Empty;
        }
    }

    /// <summary>The property itself, or the base one whose getter it inherits when it overrides only the setter.</summary>
    /// <remarks>Reflection lists such an override with no getter, though the record reads the base's, dispatched virtually.</remarks>
    /// <exception cref="ArgumentException">Neither the property nor anything it overrides has a getter.</exception>
    private static PropertyInfo Readable(PropertyInfo property, string? paramName)
    {
        if (property.GetGetMethod(nonPublic: true) is not null)
        {
            return property;
        }

        // Only a declaration on the setter's own override chain, whose root it shares: a base property
        // of that name a `new` slot hides is another property, whose getter the record's code never runs.
        MethodInfo? root = property.GetSetMethod(nonPublic: true)?.GetBaseDefinition();
        for (Type? type = property.DeclaringType?.BaseType; root is not null && type is not null; type = type.BaseType)
        {
            PropertyInfo? declared = type.GetProperty(
                property.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            MethodInfo? declaredRoot = declared?.GetSetMethod(nonPublic: true)?.GetBaseDefinition();
            if (declaredRoot is not null
                && SameDeclaration(declaredRoot, root)
                && declared!.GetGetMethod(nonPublic: true) is not null)
            {
                return declared;
            }
        }

        throw new ArgumentException(
            $"{typeof(TRecord).Name}.{property.Name} has no getter, and the store reads it from every record.", paramName);
    }

    private static PropertyInfo Named(PropertyInfo[] properties, string name, string paramName) =>
        Array.Find(properties, p => p.Name == name)
        ?? throw new ArgumentException(
            $"The definition names {name}, which {typeof(TRecord).Name} does not declare{OwnMembersOnly}.", paramName);

    private static int Depth(Type? type)
    {
        int depth = 0;
        for (; type is not null; type = type.BaseType)
        {
            depth++;
        }

        return depth;
    }

    // With several, the first found decided, in an order reflection does not promise (#1212).
    private static T? AtMostOne<T>(IEnumerable<T> candidates, Func<T, string> name, Func<string, Exception> several)
        where T : class
    {
        List<T> found = [.. candidates];
        return found.Count <= 1 ? found.FirstOrDefault() : throw several(Names(found.Select(name)));
    }

    // What a refusal adds for an interface record type, whose base interfaces' members are not read.
    private static string OwnMembersOnly =>
        typeof(TRecord).IsInterface ? " of its own; the members of the interfaces it extends are not read" : string.Empty;

    // Sorted, so a message reads the same whatever order reflection listed the properties in.
    private static string Names(IEnumerable<string> names) =>
        string.Join(", ", names.OrderBy(n => n, StringComparer.Ordinal));

    // One key addresses a record, so two is a malformed schema rather than an unsupported one.
    private static ArgumentException TwoKeys(string names, string? paramName) => new(
        $"The schema of {typeof(TRecord).Name} declares several key properties ({names}); "
        + "a record is addressed by one key.", paramName);

    // Several vectors are a shape the abstraction allows and this store does not search.
    private static NotSupportedException TwoVectors(string names) => new(
        $"The schema of {typeof(TRecord).Name} declares several vector properties ({names}); this store "
        + "holds one vector per record, and searching the first found would search one without saying which.");

    // The token is shared by every closed instantiation of one generic type, so the type is compared too.
    private static bool SameDeclaration(MemberInfo a, MemberInfo b) =>
        a.MetadataToken == b.MetadataToken && a.DeclaringType == b.DeclaringType;

    /// <summary>A property a search reads, with what a selector is compared to, reflected over once.</summary>
    private sealed class Searched
    {
        private readonly string _kind;
        private readonly MethodInfo _held;

        // Mapped by the first selector naming an interface member, since most never do.
        private readonly Lazy<MethodInfo[]> _aliases;

        public Searched(PropertyInfo property, string kind)
        {
            Property = property;
            _kind = kind;
            // Build refuses a property without a getter, so every property searched has one.
            _held = property.GetGetMethod(nonPublic: true)!.GetBaseDefinition();
            // The failures a mapping is known to raise leave an interface unaliased; PublicationOnly retries any other.
            _aliases = new Lazy<MethodInfo[]>(() => InterfaceGetters(_held), LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>The property itself.</summary>
        public PropertyInfo Property { get; }

        /// <summary>Refuses <paramref name="selector"/> unless it reads this property (#1212).</summary>
        /// <exception cref="ArgumentException"><paramref name="selector"/> reads anything else.</exception>
        public void Check(LambdaExpression? selector, string paramName)
        {
            if (selector is not null && !Selects(selector))
            {
                // A selector naming another property used to be ignored, and the search answered over this one.
                throw new ArgumentException(
                    $"The options select {Unconverted(selector.Body)}, and the {_kind} property of "
                    + $"{typeof(TRecord).Name} is {Property.Name}, the only one this collection searches.", paramName);
            }
        }

        /// <summary>Whether <paramref name="selector"/> reads this property off its parameter.</summary>
        /// <remarks>
        /// Both sides may carry conversions: a selector returning <see cref="object"/> boxes the value,
        /// and generic code constrained to an interface or a base casts the parameter to it first, with
        /// a cast or with <c>as</c>. Getters compare by the declaration they override, so an override
        /// answers to the base member an expression names, while a base property a <c>new</c> member
        /// hides does not; an interface member answers through the record's own implementation of it, and
        /// an explicit implementation, a member of its own, is another property.
        /// </remarks>
        private bool Selects(LambdaExpression selector) =>
            Unconverted(selector.Body) is MemberExpression { Member: PropertyInfo selected } access
            && Unconverted(access.Expression) == selector.Parameters[0]
            && selected.GetGetMethod(nonPublic: true) is { } chosen
            && (SameDeclaration(chosen.GetBaseDefinition(), _held)
                || (chosen.DeclaringType is { IsInterface: true }
                    && Array.Exists(_aliases.Value, alias => SameDeclaration(alias, chosen))));

        // A conversion through a user-defined operator computes a value, so it is not unwrapped.
        private static Expression? Unconverted(Expression? expression)
        {
            while (expression is UnaryExpression
                {
                    NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked or ExpressionType.TypeAs,
                    Method: null,
                } convert)
            {
                expression = convert.Operand;
            }

            return expression;
        }

        /// <summary>The interface getters <paramref name="held"/> implements on <typeparamref name="TRecord"/>.</summary>
        /// <remarks>
        /// Only interfaces the record implements itself map: one reached through variance alone has no
        /// mapping to read, and an interface record type implements none. A static member a type leaves to
        /// the interface's default body maps to no target.
        /// </remarks>
        private static MethodInfo[] InterfaceGetters(MethodInfo held)
        {
            if (typeof(TRecord).IsInterface)
            {
                return [];
            }

            var aliases = new List<MethodInfo>();
            foreach (Type contract in typeof(TRecord).GetInterfaces())
            {
                // An interface the runtime cannot map (trimmed, ahead-of-time compiled, unloadable) stays unaliased.
                InterfaceMapping map;
                try
                {
                    map = typeof(TRecord).GetInterfaceMap(contract);
                }
                catch (Exception refused) when (refused is NotSupportedException or ArgumentException or TypeLoadException)
                {
                    continue;
                }

                for (int i = 0; i < map.TargetMethods.Length; i++)
                {
                    if (map.TargetMethods[i] is { } target && SameDeclaration(target.GetBaseDefinition(), held))
                    {
                        aliases.Add(map.InterfaceMethods[i]);
                    }
                }
            }

            return [.. aliases];
        }
    }

    /// <summary>A vector property and the attribute that marks it, read together.</summary>
    private sealed record VectorMember(PropertyInfo Property, VectorStoreVectorAttribute Attribute);
}
