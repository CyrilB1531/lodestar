using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Lodestar.Abstractions;
using Lodestar.Cluster;
using Lodestar.Conformal;
using Lodestar.Decomposition;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Extensions.AI;
using Lodestar.Extensions.MathNet;
using Lodestar.Fuzzy;
using Lodestar.Gpu.Compute;
using Lodestar.Metrics;
using Lodestar.Preprocessing;
using Lodestar.Onnx;
using Lodestar.Stats;
using Lodestar.Stats.Regression;
using Lodestar.Survival;
using Lodestar.Text.Distances;

namespace Lodestar.Sample;

/// <summary>
/// Fails the build when a public <em>member</em> of the packages is not
/// reachable from this sample (ADR 0009, amended by #265).
/// </summary>
/// <remarks>
/// The surface comes from the assemblies NuGet resolved here, matched against this
/// assembly's <see cref="MemberReference"/> entries — <c>typeof(T)</c> emits only a
/// <see cref="TypeReference"/>. An enum is the documented exception. ADR 0009's
/// <em>Member granularity</em> section records what leaving type granularity cost.
/// </remarks>
internal static class PackagingGate
{
    /// <summary>The charsmap a normalizer needs is a model artifact, and none is committed.</summary>
    private const string NoCharsMap =
        "a precompiled charsmap is a binary trie inside a spiece.model, and model artifacts are "
        + "never committed (CONTRIBUTING.md). FromCharsMap refuses anything else — measured, an "
        + "empty blob and a four-zero-byte header are refused with different sentences — so there "
        + "is no input a sample could pass, and Normalize needs the instance it cannot build";

    /// <summary>A record's equality, synthesised or written: compared with, never called.</summary>
    private const string RecordPlumbing =
        "a record's value equality — the member a consumer compares WITH rather than calls, since "
        + "== reaches it through op_Equality, so no sample line produces a member reference to it";

    /// <summary>The awaitable twin of a loader the sample reads synchronously.</summary>
    private const string AsyncCounterpart =
        "the asynchronous counterpart of a loader the sample calls synchronously. A console sample "
        + "reading a committed fixture has no honest reason to await, and calling both would "
        + "demonstrate the API twice rather than the package once";

    /// <summary>A result record the library builds and the sample only reads.</summary>
    private const string ResultRecordCtor =
        "a result record the library CONSTRUCTS and the sample reads. Its properties are exercised; "
        + "constructing one by hand is what a consumer never does";

    /// <summary>
    /// Types that cannot be exercised here, each with a reason a reviewer can
    /// disagree with — the standard CONTRIBUTING.md sets for analyzer
    /// suppressions. A key naming a type that no longer exists fails the gate,
    /// so this list cannot rot into a silent omission.
    /// </summary>
    private static readonly Dictionary<string, string> Excluded = new(StringComparer.Ordinal)
    {
        ["Lodestar.Onnx.OnnxTextEmbedder"] =
            "constructing it loads an ONNX model, and model weights are never committed "
            + "(CONTRIBUTING.md); ADR 0009 already records that the sample stops at the tokenizer",
        ["Lodestar.Extensions.AI.OnnxEmbeddingGenerator"] =
            "it wraps an OnnxTextEmbedder, so building one loads the same ONNX model this list "
            + "already excludes that type for. The exclusion is inherited rather than new: there "
            + "is no constructor of it a sample could reach without the weights",
        ["Lodestar.Metrics.UndefinedMetricException"] =
            "Lot5Metrics does catch it, under ZeroDivision.Throw — but its entire public surface "
            + "is constructors, and a consumer catches rather than constructs. A catch clause emits "
            + "a type reference and no member reference, and reading ex.Message re-parents to "
            + "System.Exception, which declares it. Same shape as the enum carve-out above: the "
            + "only use a consumer has leaves nothing for the member criterion to find",

        ["Lodestar.Embeddings.Tokenization.PrecompiledNormalizer.FromCharsMap"] = NoCharsMap,
        ["Lodestar.Embeddings.Tokenization.PrecompiledNormalizer.Normalize"] = NoCharsMap,

        ["Lodestar.Cluster.KMeansOptions.Equals"] = RecordPlumbing,
        ["Lodestar.Cluster.KMeansOptions.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.AddedToken.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.AddedToken.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.BpeVocabulary.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.BpeVocabulary.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.PrecompiledNormalizer.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.PrecompiledNormalizer.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.SentencePieceVocabulary.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.SentencePieceVocabulary.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.SpecialTokenTemplate.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.SpecialTokenTemplate.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.TokenizationResult.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.TokenizationResult.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.WordPieceVocabulary.Equals"] = RecordPlumbing,
        ["Lodestar.Embeddings.Tokenization.WordPieceVocabulary.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Metrics.ClassificationReport.ToString"] = RecordPlumbing,
        ["Lodestar.Stats.Chi2ContingencyResult.Equals"] = RecordPlumbing,
        ["Lodestar.Stats.Chi2ContingencyResult.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Survival.KaplanMeierCurve.Equals"] = RecordPlumbing,
        ["Lodestar.Survival.KaplanMeierCurve.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Survival.NelsonAalenCurve.Equals"] = RecordPlumbing,
        ["Lodestar.Survival.NelsonAalenCurve.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Text.Keywords.RakeOptions.Equals"] = RecordPlumbing,
        ["Lodestar.Text.Keywords.RakeOptions.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Text.Keywords.TextRankOptions.Equals"] = RecordPlumbing,
        ["Lodestar.Text.Keywords.TextRankOptions.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Text.Vectorization.CountVectorizerOptions.Equals"] = RecordPlumbing,
        ["Lodestar.Text.Vectorization.CountVectorizerOptions.GetHashCode"] = RecordPlumbing,
        ["Lodestar.Embeddings.Persistence.BpeFilesLoader.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Persistence.SentencePieceModelLoader.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Persistence.TokenizerJsonLoader.LoadBpeAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Persistence.TokenizerJsonLoader.LoadUnigramAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Persistence.TokenizerJsonLoader.LoadWordPieceAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Persistence.VocabTxtLoader.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Search.EmbeddingIndex.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Search.EmbeddingIndex.SaveAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.CountVectorizer.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.CountVectorizer.SaveAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.HashingVectorizer.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.HashingVectorizer.SaveAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.TfidfVectorizer.LoadAsync"] = AsyncCounterpart,
        ["Lodestar.Text.Vectorization.TfidfVectorizer.SaveAsync"] = AsyncCounterpart,
        ["Lodestar.Embeddings.Search.SearchResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Embeddings.Tokenization.AddedToken..ctor"] = ResultRecordCtor,
        ["Lodestar.Embeddings.Tokenization.SpecialTokenTemplate..ctor"] = ResultRecordCtor,
        ["Lodestar.Embeddings.Tokenization.TokenizationResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Embeddings.Tokenization.WordPieceVocabulary..ctor"] = ResultRecordCtor,
        ["Lodestar.Fuzzy.ExtractResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Metrics.AverageRow..ctor"] = ResultRecordCtor,
        ["Lodestar.Metrics.ClassRow..ctor"] = ResultRecordCtor,
        ["Lodestar.Metrics.PairConfusionMatrix..ctor"] = ResultRecordCtor,
        ["Lodestar.Stats.Chi2ContingencyResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Stats.KsResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Stats.TTestResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Stats.TestResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Survival.KaplanMeierCurve..ctor"] = ResultRecordCtor,
        ["Lodestar.Survival.LogRankResult..ctor"] = ResultRecordCtor,
        ["Lodestar.Survival.NelsonAalenCurve..ctor"] = ResultRecordCtor,
        ["Lodestar.Survival.SurvivalStep..ctor"] = ResultRecordCtor,
        ["Lodestar.Text.Keywords.KeywordMatch..ctor"] = ResultRecordCtor,
    };

    /// <summary>What one pass over the exported surface found.</summary>
    private readonly record struct Surface(HashSet<string> Exported, int Covered, List<string> Uncovered)
    {
        /// <summary>How many public members were judged, exclusions aside.</summary>
        public int Members => Covered + Uncovered.Count;
    }

    /// <summary>Runs the check.</summary>
    /// <returns><c>true</c> when every exported public type is accounted for.</returns>
    public static bool Verify()
    {
        Assembly[] packaged =
        [
            typeof(Levenshtein).Assembly,
            typeof(WordPieceTokenizer).Assembly,
            typeof(Fuzz).Assembly,
            typeof(ConfusionMatrix).Assembly,
            typeof(KMeans).Assembly,
            typeof(SplitConformal).Assembly,
            typeof(CsrMatrix).Assembly,
            typeof(TruncatedSvd).Assembly,
            typeof(OnnxTextEmbedder).Assembly,
            typeof(OnnxEmbeddingGenerator).Assembly,
            typeof(MathNetInterop).Assembly,
            typeof(StandardScaler).Assembly,
            typeof(TTest).Assembly,
            typeof(OrdinaryLeastSquares).Assembly,
            typeof(KaplanMeier).Assembly,
            typeof(GpuContext).Assembly,
        ];

        References(out HashSet<string> typeRefs, out HashSet<string> memberRefs);

        Surface surface = Inspect(packaged, typeRefs, memberRefs);
        string[] stale = [.. Excluded.Keys.Where(k => !surface.Exported.Contains(k)).Order(StringComparer.Ordinal)];

        Report(surface, stale);
        return surface.Uncovered.Count == 0 && stale.Length == 0;
    }

    /// <summary>Matches every exported type against what this assembly references.</summary>
    private static Surface Inspect(
        Assembly[] packaged,
        HashSet<string> typeRefs,
        HashSet<string> memberRefs)
    {
        var exported = new HashSet<string>(StringComparer.Ordinal);
        var uncovered = new List<string>();
        int covered = 0;

        foreach (Type type in packaged.SelectMany(a => a.GetExportedTypes()))
        {
            if (type.IsEnum)
            {
                InspectEnum(type, typeRefs, exported, uncovered, ref covered);
                continue;
            }

            InspectMembers(type, typeRefs, memberRefs, exported, uncovered, ref covered);
        }

        return new Surface(exported, covered, uncovered);
    }

    /// <summary>An enum is judged whole: naming the type is all a consumer can do.</summary>
    private static void InspectEnum(
        Type type,
        HashSet<string> typeRefs,
        HashSet<string> exported,
        List<string> uncovered,
        ref int covered)
    {
        string name = type.FullName!;
        Judge(name, "(enum) is never named", typeRefs.Contains(name), exported, uncovered, ref covered);
    }

    /// <summary>Every public member of one type, against what the sample referenced.</summary>
    private static void InspectMembers(
        Type type,
        HashSet<string> typeRefs,
        HashSet<string> memberRefs,
        HashSet<string> exported,
        List<string> uncovered,
        ref int covered)
    {
        string typeName = type.FullName!;
        exported.Add(typeName);
        bool wholeTypeExcluded = Excluded.ContainsKey(typeName);
        bool named = typeRefs.Contains(typeName);
        ILookup<string, string> declarations = CalledThrough(type);

        foreach (string member in PublicMembers(type))
        {
            string name = $"{typeName}.{member}";
            if (wholeTypeExcluded)
            {
                exported.Add(name);
                continue;
            }

            // A property is reached by either accessor: read in a Console line, or
            // written in an object initializer, which is how the options records are used.
            bool reached = Referenced(member)
                || (member.StartsWith("get_", StringComparison.Ordinal) && Referenced("set_" + member[4..]));

            Judge(name, "is never referenced", reached, exported, uncovered, ref covered);
        }

        // An override is only visible as its base declaration, so the sample must also name
        // the type — otherwise a call through any other subclass would count for this one.
        bool Referenced(string member) =>
            memberRefs.Contains($"{typeName}.{member}")
            || (named && declarations[member].Any(memberRefs.Contains));
    }

    /// <summary>What a call to each member of <paramref name="type"/> is emitted against instead.</summary>
    /// <remarks>
    /// Roslyn emits a virtual call against the least-derived declaration, so an override of
    /// <c>VectorStore.CollectionExistsAsync</c> is referenced as that, and a <c>using</c> as
    /// <c>IDisposable.Dispose</c>: neither appears as a member reference of its own. Measured
    /// on #731 with <c>ilspycmd</c>.
    /// </remarks>
    private static ILookup<string, string> CalledThrough(Type type)
    {
        const BindingFlags Declared =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var pairs = new List<(string Member, string Declaration)>();
        foreach (MethodInfo method in type.GetMethods(Declared))
        {
            MethodInfo definition = method.GetBaseDefinition();
            if (definition.DeclaringType != type)
            {
                pairs.Add((method.Name, MemberName(definition)));
            }
        }

        foreach (Type contract in type.IsInterface ? [] : type.GetInterfaces())
        {
            InterfaceMapping map = type.GetInterfaceMap(contract);
            for (int i = 0; i < map.TargetMethods.Length; i++)
            {
                if (map.TargetMethods[i] is { IsPublic: true } target && target.DeclaringType == type)
                {
                    pairs.Add((target.Name, MemberName(map.InterfaceMethods[i])));
                }
            }
        }

        return pairs.ToLookup(p => p.Member, p => p.Declaration, StringComparer.Ordinal);
    }

    /// <summary>A method named the way <see cref="References"/> names a member reference.</summary>
    private static string MemberName(MethodInfo method)
    {
        Type declaring = method.DeclaringType!;
        Type definition = declaring.IsGenericType ? declaring.GetGenericTypeDefinition() : declaring;
        return $"{definition.FullName}.{method.Name}";
    }

    /// <summary>
    /// The one tally <see cref="InspectEnum"/> and <see cref="InspectMembers"/> both
    /// perform, once per candidate name: record it, skip a documented exclusion, else
    /// count it covered or report it missing.
    /// </summary>
    private static void Judge(
        string name,
        string missingSuffix,
        bool reached,
        HashSet<string> exported,
        List<string> uncovered,
        ref int covered)
    {
        exported.Add(name);
        if (Excluded.ContainsKey(name))
        {
            return;
        }

        if (reached)
        {
            covered++;
        }
        else
        {
            uncovered.Add($"{name} {missingSuffix}");
        }
    }

    /// <summary>The public members of a type that a consumer could reference.</summary>
    /// <remarks>
    /// A property is named by its accessors in metadata, so it is offered under both;
    /// <see cref="Inspect"/> accepts either. Compiler-generated members of a record —
    /// <c>&lt;Clone&gt;$</c> and friends — are excluded, as they are from the reference
    /// gate, because a name C# cannot spell is not one a sample can call.
    /// </remarks>
    private static IEnumerable<string> PublicMembers(Type type)
    {
        const BindingFlags Declared =
            BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        foreach (MemberInfo member in type.GetMembers(Declared))
        {
            if (member.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
            {
                continue;
            }

            switch (member)
            {
                case ConstructorInfo:
                    yield return ".ctor";
                    break;
                case MethodInfo { IsSpecialName: false } method:
                    yield return method.Name;
                    break;
                case PropertyInfo property:
                    yield return "get_" + property.Name;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>Prints the tally, then one <c>::error::</c> line per problem.</summary>
    private static void Report(Surface surface, string[] stale)
    {
        Console.WriteLine("packaging gate");
        Console.WriteLine($"  exported public members: {surface.Members}");
        Console.WriteLine($"  referenced by sample   : {surface.Covered}");
        Console.WriteLine($"  documented exclusions  : {Excluded.Count}");

        foreach (string name in surface.Uncovered.Order(StringComparer.Ordinal))
        {
            Console.Error.WriteLine(
                $"::error::{name}. The sample is the only thing that proves this type is reachable "
                + "from outside its assembly once packaged. Reference a member of it in one of the "
                + "Lot*.cs files, or add it to PackagingGate.Excluded with a reason.");
        }
        foreach (string name in stale)
        {
            Console.Error.WriteLine(
                $"::error::PackagingGate.Excluded names '{name}', which no longer exists in the "
                + "packages. Remove the entry.");
        }

        if (surface.Uncovered.Count == 0 && stale.Length == 0)
        {
            Console.WriteLine("  every public member is reachable.");
            Console.WriteLine();
        }
    }

    /// <summary>Reads this assembly's own metadata for every type and member it references.</summary>
    /// <remarks>
    /// Compiled metadata rather than a source scan: a name in a comment, a string or a
    /// <c>using</c> is not a reference, and only the emitted tables tell the difference.
    /// References outside the packages are kept, because an override is called through a
    /// declaration that usually lives in the framework (<see cref="CalledThrough"/>).
    /// </remarks>
    private static void References(out HashSet<string> typeRefs, out HashSet<string> memberRefs)
    {
        typeRefs = new HashSet<string>(StringComparer.Ordinal);
        memberRefs = new HashSet<string>(StringComparer.Ordinal);

        using var file = File.OpenRead(Assembly.GetExecutingAssembly().Location);
        using var pe = new PEReader(file);
        MetadataReader metadata = pe.GetMetadataReader();

        foreach (TypeReferenceHandle handle in metadata.TypeReferences)
        {
            if (FullNameOf(metadata, handle) is { } name)
            {
                typeRefs.Add(name);
            }
        }
        foreach (MemberReferenceHandle handle in metadata.MemberReferences)
        {
            MemberReference member = metadata.GetMemberReference(handle);
            if (ParentName(metadata, member.Parent) is { } name)
            {
                memberRefs.Add($"{name}.{metadata.GetString(member.Name)}");
            }
        }
    }

    /// <summary>
    /// The full name of the type a member reference hangs off, or <c>null</c> when
    /// it is not a type defined in another assembly.
    /// </summary>
    /// <remarks>
    /// A call through <c>Box&lt;int&gt;</c> hangs off a <see cref="TypeSpecification"/>
    /// encoding the instantiation, not a <see cref="TypeReference"/>; it is named after
    /// the open definition underneath, the way <see cref="Type.FullName"/> names the
    /// exported <c>Box`1</c>.
    /// </remarks>
    private static string? ParentName(MetadataReader metadata, EntityHandle parent)
    {
        if (parent.Kind == HandleKind.TypeReference)
        {
            return FullNameOf(metadata, (TypeReferenceHandle)parent);
        }
        if (parent.Kind != HandleKind.TypeSpecification)
        {
            return null;
        }

        BlobReader signature = metadata.GetBlobReader(
            metadata.GetTypeSpecification((TypeSpecificationHandle)parent).Signature);
        if (signature.ReadSignatureTypeCode() != SignatureTypeCode.GenericTypeInstance)
        {
            return null;
        }

        _ = signature.ReadSignatureTypeCode(); // CLASS or VALUETYPE, which the name does not carry
        EntityHandle definition = signature.ReadTypeHandle();
        return definition.Kind == HandleKind.TypeReference
            ? FullNameOf(metadata, (TypeReferenceHandle)definition)
            : null;
    }

    /// <summary>
    /// The full name of a type reference, or <c>null</c> when it does not resolve
    /// to another assembly.
    /// </summary>
    private static string? FullNameOf(MetadataReader metadata, TypeReferenceHandle handle)
    {
        TypeReference reference = metadata.GetTypeReference(handle);
        string name = metadata.GetString(reference.Name);

        switch (reference.ResolutionScope.Kind)
        {
            case HandleKind.TypeReference:
                // A nested type: qualify it with its declaring type, the way
                // Type.FullName does.
                string? declaring = FullNameOf(metadata, (TypeReferenceHandle)reference.ResolutionScope);
                return declaring is null ? null : declaring + "+" + name;

            case HandleKind.AssemblyReference:
                string @namespace = metadata.GetString(reference.Namespace);
                return string.IsNullOrEmpty(@namespace) ? name : @namespace + "." + name;

            default:
                return null;
        }
    }
}
