using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
#if NET
using System.Runtime.Loader;
#endif

namespace Lodestar.Extensions.VectorData;

/// <summary>
/// How a refusal names a type it is given, the caller's or the one it asks for: out of <see cref="Names(Type[])"/>, or,
/// when a member of one of those types is printed beside them, out of <see cref="Names(Type[], int, string)"/>. A
/// refused selector prints as its expression prints itself, a type a refusal names as fixed text, such as
/// <c>EmbeddingIndex</c>, is written so, and an exception the framework words names a type its own way.
/// </summary>
internal static class TypeNames
{
    // How far a component's name reaches; each level tells apart every pair the one before does.
    private const int Short = 0;
    private const int Full = 1;
    private const int InAssembly = 2;
    private const int InLoadContext = 3;
    private const int Ranked = 4;

    private const string NoLoadContext = "no load context";

#if !NET
    // Looked up once, and only by a name that reaches the load context: netstandard2.0 declares none, though the runtime
    // it runs on may have them, and a lookup that fails then costs that one tail rather than the type's initializer.
    private static readonly Lazy<MethodInfo?> GetLoadContext = new(FindGetLoadContext);
#endif

    /// <summary>
    /// The types one refusal prints, in order: each by its CLR name, its own type arguments in angle brackets in place of
    /// the arity suffix and an array's, by-ref's or pointer's suffix after its element, as <c>List&lt;Int32&gt;[]</c>. A
    /// component, a type without its own arguments, that would read like another distinct one takes the first longer form
    /// telling them apart: namespace and declaring types (<c>+</c> before a nested name) with the arguments they pass, then
    /// its assembly, then its load context, then its rank among those still alike. A function pointer, nameless from
    /// .NET 8, is named by <see cref="Type.ToString"/>; older runtimes reflect it as IntPtr.
    /// </summary>
    /// <remarks>Own arguments: the last listed, as many as the arity digits count but no more; digits counting none stay.</remarks>
    internal static RefusalName[] Names(params Type[] types) => Names(types, mention: null);

    /// <summary>
    /// <see cref="Names(Type[])"/>, for a refusal that also prints <paramref name="member"/> of the type at
    /// <paramref name="owner"/>: what <see cref="RefusalName.Member"/> prints for it is read apart from every type too.
    /// </summary>
    internal static RefusalName[] Names(Type[] types, int owner, string member) => Names(types, new Mention(owner, member));

    private static RefusalName[] Names(Type[] types, Mention? mention)
    {
        var components = new Dictionary<Type, int>(SameComponent.Instance);
        foreach (Type type in types)
        {
            Collect(type, components);
        }

        Dictionary<Type, RefusalName> heads = Heads(types, components, mention);
        return [.. types.Select(type => Name(type, heads))];
    }

    /// <summary>
    /// The type a refusal names a caller's value by: the type the caller declared, as a runtime type may be an iterator or
    /// a library's internal one they never wrote, and the runtime type when they declared object, which names nothing.
    /// </summary>
    internal static Type Declared<T>(T value)
        where T : notnull => typeof(T) == typeof(object) ? value.GetType() : typeof(T);

    // What a name is built of, each numbered in the order met: a type without its own arguments, which are components
    // of their own. A generic parameter is one, so an open type reads apart from one closed over a type of its name.
    private static void Collect(Type type, Dictionary<Type, int> components)
    {
        type = Component(type);
        if (!components.ContainsKey(type))
        {
            components[type] = components.Count;
        }

        foreach (Type argument in type.GetGenericArguments())
        {
            Collect(argument, components);
        }
    }

    // Heads settle at the levels they hold before any level rises. Only what the message prints is compared, and a head
    // whose declaring types pass arguments still alike waits for those to part, since they may part it too.
    private static Dictionary<Type, RefusalName> Heads(Type[] types, Dictionary<Type, int> index, Mention? mention)
    {
        var problem = new Problem(types, index, mention);
        Type[] components = problem.Components;
        int count = components.Length;
        int[] levels = new int[count];
        int[] ranks = new int[count];
        int[][] passed = [.. components.Select(component => Passed(component, index))];
        var heads = new Dictionary<Type, RefusalName>(SameComponent.Instance);
        Settle(problem, levels, ranks, heads);

        bool raised = true;
        while (raised)
        {
            raised = false;
            bool[][] pairs = Pairs(problem, Printed(problem, levels, heads), heads);
            bool[] alike = [.. pairs.Select(Alike)];
            for (int i = 0; i < count; i++)
            {
                // Ranked is the last level: ranks given in one round differ, and a level past it would drop them.
                if (!alike[i] || levels[i] == Ranked || (levels[i] >= Full && Array.Exists(passed[i], j => alike[j])))
                {
                    continue;
                }

                levels[i]++;
                ranks[i] = levels[i] == Ranked ? FreeRank(i, problem, ranks, pairs, heads) : 0;
                raised = true;
            }

            Settle(problem, levels, ranks, heads);
        }

        Part(problem, levels, ranks, heads);
        return heads;
    }

    // What the rounds leave alike takes a rank above every other, so two tails end in different numbers; each pass
    // ranks one component or more for good, so as many passes as components end it.
    private static void Part(Problem problem, int[] levels, int[] ranks, Dictionary<Type, RefusalName> heads)
    {
        Type[] components = problem.Components;
        bool[] parted = new bool[components.Length];
        for (int pass = 0; pass < components.Length; pass++)
        {
            bool[][] pairs = Pairs(problem, Printed(problem, levels, heads), heads);
            int[] alike = [.. Enumerable.Range(0, components.Length).Where(i => !parted[i] && Alike(pairs[i]))];
            if (alike.Length == 0)
            {
                return;
            }

            foreach (int i in alike)
            {
                levels[i] = Ranked;
                ranks[i] = ranks.Max() + 1;
                parted[i] = true;
            }

            Settle(problem, levels, ranks, heads);
        }
    }

    // Which pairs read alike: both printed, with one head and as many arguments of their own, or one printed as the other.
    private static bool[][] Pairs(Problem problem, HashSet<string>?[] printed, Dictionary<Type, RefusalName> heads)
    {
        (Type[] components, int[] own) = (problem.Components, problem.Own);
        int count = components.Length;
        bool[][] pairs = new bool[count][];
        for (int i = 0; i < count; i++)
        {
            pairs[i] = new bool[count];
            for (int j = 0; j < count; j++)
            {
                pairs[i][j] = i != j && printed[i] is { } mine && printed[j] is { } theirs
                    && ((heads[components[i]] == heads[components[j]] && own[i] == own[j])
                        || mine.Overlaps(theirs));
            }
        }

        return pairs;
    }

    private static bool Alike(bool[] row) => Array.Exists(row, pair => pair);

    // The smallest rank held neither by a component alike with this one, alike by printing not being transitive, nor by
    // one, ranked in any round, whose name reads the same rank aside: only the rank would tell those two apart.
    private static int FreeRank(int component, Problem problem, int[] ranks, bool[][] pairs, Dictionary<Type, RefusalName> heads)
    {
        Type[] components = problem.Components;
        RefusalName aside = Leaf(components[component], InLoadContext, 0, heads);
        int rank = 1;
        while (Enumerable.Range(0, ranks.Length).Any(j => ranks[j] == rank
            && (pairs[component][j] || Leaf(components[j], InLoadContext, 0, heads) == aside)))
        {
            rank++;
        }

        return rank;
    }

    // The component a type is named after: the type, its array, by-ref and pointer elements stripped.
    private static Type Component(Type type)
    {
        while (type.HasElementType)
        {
            type = type.GetElementType()!;
        }

        return type;
    }

    // What each component prints as, suffixes included, or null where the message never shows it: the arguments a
    // declaring type passes are printed only from Full on, and only those its qualifier shows.
    private static HashSet<string>?[] Printed(Problem problem, int[] levels, Dictionary<Type, RefusalName> heads)
    {
        (Type[] types, Dictionary<Type, int> index, Mention? mention) = (problem.Types, problem.Index, problem.Mention);
        var printed = new HashSet<string>?[index.Count];
        foreach (Type type in types)
        {
            Visit(type);
        }

        // A member prints as its owner's text with the member before the tail, so it counts among the owner's texts.
        if (mention is { } named)
        {
            printed[index[Component(types[named.Owner])]]!.Add(Name(types[named.Owner], heads).Member(named.Member));
        }

        return printed;

        void Visit(Type whole)
        {
            Type type = Component(whole);
            int component = index[type];
            HashSet<string> seen = printed[component] ??= new HashSet<string>(StringComparer.Ordinal);
            seen.Add(Name(type, heads).ToString());
            seen.Add(Name(whole, heads).ToString());
            foreach (Type argument in Split(type).Own)
            {
                Visit(argument);
            }

            if (levels[component] >= Full)
            {
                foreach (Type argument in Qualifying(type).SelectMany(outer => outer.Arguments))
                {
                    Visit(argument);
                }
            }
        }
    }

    // Recomputed until no head moves: a head at Full or past names the heads of the arguments its declaring types pass,
    // which nest no deeper than there are components, so that many rounds and one more reach the fixed point.
    private static void Settle(Problem problem, int[] levels, int[] ranks, Dictionary<Type, RefusalName> heads)
    {
        Type[] components = problem.Components;
        for (int round = 0; round <= components.Length; round++)
        {
            bool moved = false;
            for (int i = 0; i < components.Length; i++)
            {
                RefusalName head = Leaf(components[i], levels[i], ranks[i], heads);
                moved |= !heads.TryGetValue(components[i], out RefusalName held) || held != head;
                heads[components[i]] = head;
            }

            if (!moved)
            {
                return;
            }
        }

        // Unreachable while the bound above holds; a head naming itself through its arguments would end here.
        Debug.Fail("The heads did not settle within as many rounds as there are components.");
    }

    // The components a head names through the arguments its declaring types pass, at any depth.
    private static int[] Passed(Type component, Dictionary<Type, int> index)
    {
        var found = new Dictionary<Type, int>(SameComponent.Instance);
        foreach (Type argument in Qualifying(component).SelectMany(outer => outer.Arguments))
        {
            Collect(argument, found);
        }

        return [.. found.Keys.Select(type => index[type])];
    }

    private static RefusalName Name(Type type, Dictionary<Type, RefusalName> heads)
    {
        if (type.HasElementType)
        {
            RefusalName element = Name(type.GetElementType()!, heads);
            return element with { Body = element.Body + Suffix(type) };
        }

        // Every component a name is built of was collected and given a head, so a missing one is a fault, not a default.
        Type[] own = Split(type).Own;
        RefusalName head = heads[type];
        return own.Length == 0
            ? head
            : head with { Body = $"{head.Body}<{string.Join(", ", own.Select(argument => Name(argument, heads)))}>" };
    }

    // A component's own name at a level, without its own arguments.
    private static RefusalName Leaf(Type type, int level, int rank, Dictionary<Type, RefusalName> heads)
    {
        string name = Split(type).Name;
        if (level == Short)
        {
            return new RefusalName(name, string.Empty);
        }

        // A generic parameter is named after what declares it, so it does not read like a type nested in that generic.
        string full = type.IsGenericParameter ? $"{name} of {Declarer(type)}" : $"{Qualifier(type, heads)}{name}";
        return level switch
        {
            Full => new RefusalName(full, string.Empty),
            InAssembly => new RefusalName(full, $" [{type.Assembly.FullName}]"),
            InLoadContext => new RefusalName(full, $" [{type.Assembly.FullName}, {LoadContext(type.Assembly)}]"),
            _ => new RefusalName(full, $" [{type.Assembly.FullName}, {LoadContext(type.Assembly)}] #{rank}"),
        };
    }

    // The generic definition that declares a parameter, without the arguments it passes, the parameter among them.
    private static string Declarer(Type parameter)
    {
        string owner = parameter.DeclaringType is { } declarer ? Plain(declarer) : string.Empty;
        if (parameter.DeclaringMethod is not { } method)
        {
            return owner;
        }

        // A method of a module, which IL allows, has no declaring type to stand before it.
        string name = $"{method.Name}<{new string(',', method.GetGenericArguments().Length - 1)}>";
        return owner.Length == 0 ? name : $"{owner}.{name}";
    }

    // A definition's namespace, declaring types and own name, each with its arity as empty brackets and no argument.
    private static string Plain(Type type)
    {
        (string bare, Type[] own) = Split(type);
        string name = own.Length == 0 ? bare : $"{bare}<{new string(',', own.Length - 1)}>";
        if (type.DeclaringType is { } outer)
        {
            return $"{Plain(outer)}+{name}";
        }

        return type.Namespace is { Length: > 0 } space ? $"{space}.{name}" : name;
    }

    // The namespace and the declaring types before a type's own name, each with the arguments it passes down. A
    // declaring type ends in '+', as in Type.FullName, so a nested type does not read like one of a namespace.
    private static string Qualifier(Type type, Dictionary<Type, RefusalName> heads)
    {
        (Type Outer, Type[] Arguments)[] shown = Qualifying(type);
        string space = (shown.Length > 0 ? shown[0].Outer : type).Namespace ?? string.Empty;
        var qualifier = new StringBuilder(space.Length == 0 ? space : space + ".");
        foreach ((Type outer, Type[] arguments) in shown)
        {
            string outerName = Split(outer).Name;
            qualifier.Append(arguments.Length == 0
                ? $"{outerName}+"
                : $"{outerName}<{string.Join(", ", arguments.Select(argument => Name(argument, heads)))}>+");
        }

        return qualifier.ToString();
    }

    // The declaring types, outermost first, each with the inherited arguments its own arity suffix counts: the ones
    // a qualifier prints, and so the only ones compared.
    private static (Type Outer, Type[] Arguments)[] Qualifying(Type type)
    {
        var chain = new List<Type>();
        for (Type? outer = type.DeclaringType; outer is not null; outer = outer.DeclaringType)
        {
            chain.Insert(0, outer);
        }

        Type[] passed = Inherited(type);
        int taken = 0;
        var shown = new (Type Outer, Type[] Arguments)[chain.Count];
        for (int i = 0; i < chain.Count; i++)
        {
            int count = Math.Min(Split(chain[i]).Own.Length, passed.Length - taken);
            shown[i] = (chain[i], [.. passed.Skip(taken).Take(count)]);
            taken += count;
        }

        return shown;
    }

    // Its ToString carries the context's name and an ordinal, so two contexts of one name still read apart.
    private static string LoadContext(Assembly assembly)
    {
#if NET
        return AssemblyLoadContext.GetLoadContext(assembly)?.ToString() ?? NoLoadContext;
#else
        // CA1031: a refusal is being built, and whatever stops the optional context lookup must leave that refusal standing.
#pragma warning disable CA1031
        try
        {
            return GetLoadContext.Value?.Invoke(null, [assembly])?.ToString() ?? NoLoadContext;
        }
        catch (Exception)
        {
            return NoLoadContext;
        }
#pragma warning restore CA1031
#endif
    }

#if !NET
    // CA1031: as above; a lookup that throws is a runtime with no load context to name, and Lazy must not cache a throw.
#pragma warning disable CA1031
    private static MethodInfo? FindGetLoadContext()
    {
        try
        {
            return Type.GetType("System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader")
                ?.GetMethod("GetLoadContext", [typeof(Assembly)]);
        }
        catch (Exception)
        {
            return null;
        }
    }
#pragma warning restore CA1031
#endif

    // The arguments a type's declaring types pass it: all it lists but its own.
    private static Type[] Inherited(Type type)
    {
        Type[] arguments = type.GetGenericArguments();
        return [.. arguments.Take(arguments.Length - Split(type).Own.Length)];
    }

    // The name before a numeric arity suffix that counts arguments, and those arguments: the last ones the type lists.
    private static (string Name, Type[] Own) Split(Type type)
    {
        // A function pointer has an empty name; its full one is the only one it has, and it takes no arguments.
        if (type.Name.Length == 0)
        {
            return (type.ToString(), []);
        }

        int tick = type.Name.AsSpan().LastIndexOf('`');
#if NET
        ReadOnlySpan<char> suffix = tick < 0 ? [] : type.Name.AsSpan(tick + 1);
#else
        string suffix = tick < 0 ? string.Empty : type.Name.Substring(tick + 1);
#endif
        Type[] arguments = type.GetGenericArguments();
        int own = int.TryParse(suffix, NumberStyles.None, CultureInfo.InvariantCulture, out int arity)
            ? Math.Min(arity, arguments.Length)
            : 0;
        return own == 0 ? (type.Name, []) : (type.Name.Substring(0, tick), [.. arguments.Skip(arguments.Length - own)]);
    }

    // Read from the type's shape rather than sliced off its name, which no Type implementation promises to compose.
    private static string Suffix(Type type)
    {
        if (type.IsByRef)
        {
            return "&";
        }

        if (type.IsPointer)
        {
            return "*";
        }

        int rank = type.GetArrayRank();
        if (rank > 1)
        {
            return $"[{new string(',', rank - 1)}]";
        }

        // MakeArrayType(1) builds no vector, whatever its lower bound. Without IsSZArray, netstandard2.0 builds the vector
        // type and compares: the runtime types reflection over a record or search value yields are one instance each.
#if NET
        return type.IsSZArray ? "[]" : "[*]";
#else
        return type == type.GetElementType()!.MakeArrayType() ? "[]" : "[*]";
#endif
    }

    /// <summary>One component: one generic definition, closed over the same arguments its declaring types pass it.</summary>
    private sealed class SameComponent : IEqualityComparer<Type>
    {
        internal static readonly SameComponent Instance = new();

        public bool Equals(Type? x, Type? y) =>
            ReferenceEquals(x, y) || (x is not null && y is not null && Definition(x) == Definition(y) && Inherited(x).SequenceEqual(Inherited(y)));

        public int GetHashCode(Type obj) => Definition(obj).GetHashCode();

        private static Type Definition(Type type) => type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
    }

    /// <summary>A member a refusal prints beside the types it names: which type owns it, and its name.</summary>
    private readonly record struct Mention(int Owner, string Member);

    /// <summary>What one <see cref="Names(Type[])"/> call names, fixed across its rounds.</summary>
    /// <param name="Types">The types the refusal prints.</param>
    /// <param name="Index">Each component's number.</param>
    /// <param name="Mention">The member printed beside them, if any.</param>
    private sealed record Problem(Type[] Types, Dictionary<Type, int> Index, Mention? Mention)
    {
        /// <summary>The components, in number order.</summary>
        public Type[] Components { get; } = [.. Index.OrderBy(entry => entry.Value).Select(entry => entry.Key)];

        /// <summary>How many arguments of its own each component takes, in the same order.</summary>
        public int[] Own => [.. Components.Select(component => Split(component).Own.Length)];
    }
}

/// <summary>A type as a refusal prints it: what names it, then what tells it from a type of the same name.</summary>
/// <param name="Body">The name, its arguments and its element suffixes.</param>
/// <param name="Tail">The assembly, load context and rank a name needed to read apart, or nothing.</param>
internal readonly record struct RefusalName(string Body, string Tail)
{
    /// <summary>A member of the type, named before what tells the type apart: <c>Doc.Id [A, …]</c>.</summary>
    internal string Member(string member) => $"{Body}.{member}{Tail}";

    /// <inheritdoc />
    public override string ToString() => Body + Tail;
}
