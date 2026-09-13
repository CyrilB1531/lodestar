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
    private readonly PropertyInfo _vector;
    private readonly PropertyInfo? _fullText;

    private RecordSchema(PropertyInfo key, PropertyInfo vector, PropertyInfo? fullText, int dimension)
    {
        _key = key;
        _vector = vector;
        _fullText = fullText;
        Dimension = dimension;
    }

    /// <summary>The vector length every record in the collection must carry.</summary>
    public int Dimension { get; }

    /// <summary>Whether a property was marked for full-text search, which is what BM25 needs.</summary>
    public bool HasFullText => _fullText is not null;

    /// <summary>Reads the schema from <paramref name="definition"/> when one is given, else from the attributes.</summary>
    /// <param name="definition">An explicit description, or <see langword="null"/> to read the attributes.</param>
    /// <exception cref="ArgumentException">No key property, no vector property, a vector of a type other than <c>ReadOnlyMemory&lt;float&gt;</c>, or a definition naming a property the type lacks.</exception>
    /// <exception cref="NotSupportedException">The vector declares a distance function other than cosine similarity.</exception>
    public static RecordSchema<TKey, TRecord> Create(VectorStoreCollectionDefinition? definition)
    {
        PropertyInfo[] properties = typeof(TRecord).GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        return definition is null ? FromAttributes(properties) : FromDefinition(properties, definition);
    }

    /// <summary>The record's key, as the dictionary keys it.</summary>
    /// <exception cref="ArgumentException">The record's key is null, which no dictionary can hold.</exception>
    public TKey KeyOf(TRecord record) =>
        _key.GetValue(record) is { } key
            ? (TKey)key
            : throw new ArgumentException(
                $"A {typeof(TRecord).Name} carries a null {_key.Name}, and a record is addressed by its key.",
                nameof(record));

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

        TKey key = KeyOf(record);
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
        (ReadOnlyMemory<float>)_vector.GetValue(record)!;

    /// <summary>The text BM25 indexes, or the empty string when the record leaves it null.</summary>
    /// <remarks>Never called when <see cref="HasFullText"/> is false.</remarks>
    public string FullTextOf(TRecord record) => (string?)_fullText!.GetValue(record) ?? string.Empty;

    private static RecordSchema<TKey, TRecord> FromAttributes(PropertyInfo[] properties)
    {
        PropertyInfo key = Single(properties, p => p.GetCustomAttribute<VectorStoreKeyAttribute>() is not null, KeyAttribute);
        PropertyInfo? text = Array.Find(properties, p =>
            p.GetCustomAttribute<VectorStoreDataAttribute>() is { IsFullTextIndexed: true });

        (PropertyInfo Property, VectorStoreVectorAttribute Attribute) vector = VectorProperty(properties);
        return Build(
            key, vector.Property, text, vector.Attribute.Dimensions, vector.Attribute.DistanceFunction);
    }

    // Reads the property and its attribute in one pass: re-reading the attribute a second
    // time would need a null-forgiving operator that only one target framework allows.
    private static (PropertyInfo Property, VectorStoreVectorAttribute Attribute) VectorProperty(PropertyInfo[] properties)
    {
        foreach (PropertyInfo property in properties)
        {
            VectorStoreVectorAttribute? attribute = property.GetCustomAttribute<VectorStoreVectorAttribute>();
            if (attribute is not null)
            {
                return (property, attribute);
            }
        }

        throw new ArgumentException(
            $"{typeof(TRecord).Name} carries no [VectorStoreVector] property.", nameof(properties));
    }

    private static RecordSchema<TKey, TRecord> FromDefinition(
        PropertyInfo[] properties, VectorStoreCollectionDefinition definition)
    {
        VectorStoreKeyProperty key = definition.Properties.OfType<VectorStoreKeyProperty>().FirstOrDefault()
            ?? throw new ArgumentException(
                $"The definition names no key property; one {KeyAttribute} property or one "
                + "VectorStoreKeyProperty is what a record is addressed by.", nameof(definition));
        VectorStoreVectorProperty vector = definition.Properties.OfType<VectorStoreVectorProperty>().FirstOrDefault()
            ?? throw new ArgumentException(
                "The definition names no vector property, and a vector store searches by vector.",
                nameof(definition));
        VectorStoreDataProperty? text = definition.Properties.OfType<VectorStoreDataProperty>()
            .FirstOrDefault(p => p.IsFullTextIndexed);

        return Build(
            Named(properties, key.Name),
            Named(properties, vector.Name),
            text is null ? null : Named(properties, text.Name),
            vector.Dimensions,
            vector.DistanceFunction);
    }

    private static RecordSchema<TKey, TRecord> Build(
        PropertyInfo key, PropertyInfo vector, PropertyInfo? text, int dimension, string? distanceFunction)
    {
        if (vector.PropertyType != typeof(ReadOnlyMemory<float>))
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{vector.Name} is {vector.PropertyType.Name}; this store "
                + "holds ReadOnlyMemory<float>, which is what EmbeddingIndex takes.", nameof(vector));
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

        return new RecordSchema<TKey, TRecord>(key, vector, text, dimension);
    }

    private static PropertyInfo Named(PropertyInfo[] properties, string name) =>
        Array.Find(properties, p => p.Name == name)
        ?? throw new ArgumentException(
            $"The definition names {name}, which {typeof(TRecord).Name} does not declare.", nameof(name));

    private static PropertyInfo Single(PropertyInfo[] properties, Func<PropertyInfo, bool> match, string attribute) =>
        Array.Find(properties, new Predicate<PropertyInfo>(match))
        ?? throw new ArgumentException(
            $"{typeof(TRecord).Name} carries no [{attribute}] property.", nameof(properties));
}
