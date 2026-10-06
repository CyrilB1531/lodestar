using Lodestar.Abstractions;
using System.Text.Json;
using Lodestar.Internal.Persistence;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Persistence;

/// <summary>
/// Reads and writes the option records of the vectorizers.
/// </summary>
/// <remarks>
/// Enumerations persist under their scikit-learn spelling — <c>word</c>/
/// <c>char</c>/<c>char_wb</c>, <c>l1</c>/<c>l2</c> — so an artifact reads like
/// the constructor call it mirrors. Stop words are written in ordinal order:
/// the option is a set, so only sorting makes the artifact byte-reproducible.
/// </remarks>
internal static class VectorizerOptionsJson
{
    /// <summary>Writes <paramref name="options"/>, with the stop words the fitted analyzer filters with.</summary>
    /// <param name="writer">The writer.</param>
    /// <param name="propertyName">The property the options object is written under.</param>
    /// <param name="options">The vectorizer's options.</param>
    /// <param name="stopWords">
    /// The analyzer's own copy of <c>options.StopWords</c>, taken at construction: the caller's collection may have changed
    /// since, and a reloaded vectorizer filtering with it dropped terms its vocabulary holds (#1648).
    /// </param>
    public static void Write(
        Utf8JsonWriter writer, string propertyName, CountVectorizerOptions options, IReadOnlyCollection<string>? stopWords)
    {
        writer.WriteStartObject(propertyName);
        writer.WriteBoolean("lowercase", options.Lowercase);
        writer.WriteBoolean("stripAccents", options.StripAccents);
        writer.WriteString("analyzer", AnalyzerName(options.Analyzer));
        writer.WriteNumber("ngramMin", options.NgramRange.Min);
        writer.WriteNumber("ngramMax", options.NgramRange.Max);
        JsonArtifact.WriteExactDouble(writer, "minDf", options.MinDf);
        JsonArtifact.WriteExactDouble(writer, "maxDf", options.MaxDf);
        writer.WriteBoolean("binary", options.Binary);
        JsonArtifact.WriteText(writer, "tokenPattern", options.TokenPattern);
        WriteStopWords(writer, stopWords);
        writer.WriteEndObject();
    }

    public static void Write(Utf8JsonWriter writer, string propertyName, TfidfOptions options)
    {
        writer.WriteStartObject(propertyName);
        writer.WriteBoolean("useIdf", options.UseIdf);
        writer.WriteBoolean("smoothIdf", options.SmoothIdf);
        writer.WriteBoolean("sublinearTf", options.SublinearTf);
        WriteNorm(writer, "norm", options.Norm);
        writer.WriteEndObject();
    }

    public static CountVectorizerOptions ReadCount(ref Utf8JsonReader reader, string artifact, in ArtifactLimits limits)
    {
        JsonArtifact.ReadStartObject(ref reader, artifact, "options");

        var result = new CountVectorizerOptions();
        int ngramMin = result.NgramRange.Min;
        int ngramMax = result.NgramRange.Max;

        while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
        {
            string name = reader.GetString()!;
            switch (name)
            {
                case "lowercase":
                    result = result with { Lowercase = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "stripAccents":
                    result = result with { StripAccents = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "analyzer":
                    result = result with { Analyzer = ParseAnalyzer(JsonArtifact.ReadString(ref reader, artifact, name)) };
                    break;
                case "ngramMin":
                    ngramMin = JsonArtifact.ReadInt32(ref reader, artifact, name);
                    break;
                case "ngramMax":
                    ngramMax = JsonArtifact.ReadInt32(ref reader, artifact, name);
                    break;
                case "minDf":
                    result = result with { MinDf = JsonArtifact.ReadDouble(ref reader, artifact, name) };
                    break;
                case "maxDf":
                    result = result with { MaxDf = JsonArtifact.ReadDouble(ref reader, artifact, name) };
                    break;
                case "binary":
                    result = result with { Binary = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "tokenPattern":
                    result = result with { TokenPattern = JsonArtifact.ReadString(ref reader, artifact, name) };
                    break;
                case "stopWords":
                    result = result with { StopWords = ReadStopWords(ref reader, artifact, limits) };
                    break;
                default:
                    throw JsonArtifact.UnknownProperty(artifact, "options." + name);
            }
        }

        EnsureEndOfObject(ref reader, artifact);
        // The constructors' rule and scikit-learn's: only a descending range is refused (#1197).
        if (ngramMax < ngramMin)
        {
            throw JsonArtifact.Inconsistent(artifact, $"n-gram range ({ngramMin}, {ngramMax}) has a lower boundary larger than its upper boundary.");
        }
        return result with { NgramRange = (ngramMin, ngramMax) };
    }

    public static TfidfOptions ReadTfidf(ref Utf8JsonReader reader, string artifact)
    {
        JsonArtifact.ReadStartObject(ref reader, artifact, "tfidf");

        var result = new TfidfOptions();
        while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
        {
            string name = reader.GetString()!;
            switch (name)
            {
                case "useIdf":
                    result = result with { UseIdf = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "smoothIdf":
                    result = result with { SmoothIdf = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "sublinearTf":
                    result = result with { SublinearTf = JsonArtifact.ReadBoolean(ref reader, artifact, name) };
                    break;
                case "norm":
                    result = result with { Norm = ParseNorm(JsonArtifact.ReadNullableString(ref reader, artifact, name), artifact) };
                    break;
                default:
                    throw JsonArtifact.UnknownProperty(artifact, "tfidf." + name);
            }
        }

        EnsureEndOfObject(ref reader, artifact);
        return result;
    }

    public static void WriteNorm(Utf8JsonWriter writer, string propertyName, SparseNorm? norm)
    {
        if (norm is null)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            writer.WriteString(propertyName, norm == SparseNorm.L1 ? "l1" : "l2");
        }
    }

    public static SparseNorm? ParseNorm(string? value, string artifact) => value switch
    {
        null => null,
        "l1" => SparseNorm.L1,
        "l2" => SparseNorm.L2,
        _ => throw new InvalidDataException($"Unknown norm '{value}' in a '{artifact}' artifact; expected \"l1\", \"l2\" or null."),
    };

    /// <summary>
    /// Builds the vectorizer an artifact describes, restating an option its constructor refuses as
    /// the <see cref="InvalidDataException"/> every <c>Load</c> documents.
    /// </summary>
    /// <remarks>
    /// The reader checks the shape of each option, not whether the constructor accepts it: a
    /// <c>tokenPattern</c> of <c>"("</c> is a well-formed string that no regex parses (#881).
    /// </remarks>
    internal static T Build<T>(string artifact, Func<T> build)
    {
        try
        {
            return build();
        }
        catch (ArgumentException e)
        {
            throw new InvalidDataException(
                $"A '{ArtifactHeader.SchemaFor(artifact)}' artifact holds options the vectorizer refuses: {e.Message}",
                e);
        }
    }

    internal static void EnsureEndOfObject(ref Utf8JsonReader reader, string artifact)
    {
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw JsonArtifact.Truncated(artifact);
        }
    }

    private static void WriteStopWords(Utf8JsonWriter writer, IReadOnlyCollection<string>? stopWords)
    {
        if (stopWords is null)
        {
            writer.WriteNull("stopWords");
            return;
        }

        var sorted = stopWords.ToArray();
        Array.Sort(sorted, StringComparer.Ordinal);
        writer.WriteStartArray("stopWords");
        foreach (string word in sorted)
        {
            JsonArtifact.WriteText(writer, word);
            JsonArtifact.FlushIfPending(writer);
        }
        writer.WriteEndArray();
    }

    /// <summary>Refuses, before a save's first byte, a pattern or stop word the writer cannot write (#1618).</summary>
    /// <remarks>
    /// The two frequencies first, as <see cref="Write(Utf8JsonWriter, string, CountVectorizerOptions, IReadOnlyCollection{string})"/> writes them ahead
    /// of the strings: a <c>HashingVectorizer</c> never validates them, and its write refused them first. The analyzer
    /// ahead of them is a guard: every vectorizer's constructor refuses an unknown one already (#1622).
    /// </remarks>
    /// <exception cref="InvalidDataException">The analyzer is unknown, or a frequency is not finite.</exception>
    /// <exception cref="ArgumentException">A string is beyond what the JSON writer can write: the writer's own exception.</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the writer's buffer: the writer's own exception.</exception>
    /// <param name="options">The vectorizer's options.</param>
    /// <param name="stopWords">The stop words <see cref="Write(Utf8JsonWriter, string, CountVectorizerOptions, IReadOnlyCollection{string})"/> writes.</param>
    public static void EnsureWritable(CountVectorizerOptions options, IReadOnlyCollection<string>? stopWords)
    {
        _ = AnalyzerName(options.Analyzer);
        JsonArtifact.RequirePersistable(options.MinDf);
        JsonArtifact.RequirePersistable(options.MaxDf);
        JsonArtifact.EnsureWritableText(options.TokenPattern);

        // Sorted, as WriteStopWords writes them, and only when one is long enough to need it.
        if (stopWords is not null && stopWords.Any(word => word is not null && word.Length > JsonArtifact.AlwaysWritableCharacters))
        {
            string[] sorted = [.. stopWords];
            Array.Sort(sorted, StringComparer.Ordinal);
            foreach (string word in sorted)
            {
                JsonArtifact.EnsureWritableText(word);
            }
        }
    }

    private static List<string>? ReadStopWords(ref Utf8JsonReader reader, string artifact, in ArtifactLimits limits)
    {
        if (!reader.Read())
        {
            throw JsonArtifact.Truncated(artifact);
        }
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw JsonArtifact.UnexpectedToken(artifact, "options.stopWords", reader.TokenType);
        }

        var words = new List<string>();
        while (reader.Read() && reader.TokenType == JsonTokenType.String)
        {
            string word = JsonArtifact.GetText(ref reader);
            limits.CheckTokenLength(word.Length);
            words.Add(word);
            limits.CheckArrayLength(words.Count, "options.stopWords");
        }
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw JsonArtifact.UnexpectedToken(artifact, "options.stopWords", reader.TokenType);
        }
        return words;
    }

    private static string AnalyzerName(AnalyzerKind kind) => kind switch
    {
        AnalyzerKind.Word => "word",
        AnalyzerKind.Char => "char",
        AnalyzerKind.CharWordBoundary => "char_wb",
        _ => throw new InvalidDataException($"Cannot persist the unknown analyzer kind {kind}."),
    };

    private static AnalyzerKind ParseAnalyzer(string value) => value switch
    {
        "word" => AnalyzerKind.Word,
        "char" => AnalyzerKind.Char,
        "char_wb" => AnalyzerKind.CharWordBoundary,
        _ => throw new InvalidDataException($"Unknown analyzer '{value}'; expected \"word\", \"char\" or \"char_wb\"."),
    };
}
