using System.Text.Json;
using Lodestar.Internal.Persistence;

namespace Lodestar.Text.Persistence;

/// <summary>
/// Reads and writes the two arrays a fitted vectorizer carries: the sorted
/// feature names and, for TF-IDF, the idf weights.
/// </summary>
/// <remarks>
/// <c>featureCount</c> is written before either array so a reader can size its
/// buffers from a value it has already checked against
/// <c>ArtifactLoadOptions.MaxVocabularySize</c>, rather than growing a list at the
/// mercy of the file.
/// </remarks>
internal static class FeatureVocabularyJson
{
    public const string FeatureCountProperty = "featureCount";
    public const string VocabularyProperty = "vocabulary";
    public const string IdfProperty = "idf";

    /// <summary>Writes the vocabulary, a term at a time, searching each for a surrogate only where one may be.</summary>
    /// <param name="writer">The writer.</param>
    /// <param name="featureNames">The sorted terms.</param>
    /// <param name="mayHoldSurrogate">Whether any term may hold a surrogate, as the fit or the load that made them found.</param>
    public static void WriteVocabulary(Utf8JsonWriter writer, IReadOnlyList<string> featureNames, bool mayHoldSurrogate)
    {
        writer.WriteStartArray(VocabularyProperty);
        for (int i = 0; i < featureNames.Count; i++)
        {
            JsonArtifact.WriteText(writer, featureNames[i], mayHoldSurrogate);
            JsonArtifact.FlushIfPending(writer);
        }
        writer.WriteEndArray();
    }

    /// <summary>Refuses, before a save's first byte, a term the writer cannot write (#1618).</summary>
    /// <exception cref="InvalidOperationException">A term is beyond what the JSON writer can write.</exception>
    public static void EnsureWritableVocabulary(IReadOnlyList<string> featureNames)
    {
        for (int i = 0; i < featureNames.Count; i++)
        {
            JsonArtifact.EnsureWritableText(featureNames[i], "A vocabulary term");
        }
    }

    /// <summary>Writes the idf vector as base64 of its little-endian bits, once <see cref="EnsureWritableIdf"/> passed.</summary>
    /// <remarks>
    /// The vocabulary stays plain text, because that is the half of an artifact a
    /// human reads; the idf vector does not, because nobody reads thirty thousand
    /// floats by eye. See <c>docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md</c>, "The
    /// idf vector is base64, and the vocabulary is not", for the measurements and
    /// the exactness argument for raw bits over a decimal formatter.
    /// </remarks>
    public static void WriteIdf(Utf8JsonWriter writer, IReadOnlyList<double> idf)
    {
        // Checked again as written: weights changed since the save's own check still never reach the file.
        RequireFinite(idf);
        // Flushed first, so the block has the writer's buffer to itself, as WritableAsProperty counts it (#1618).
        writer.Flush();
        Base64Numbers.WriteDoubles(writer, IdfProperty, idf);
    }

    /// <summary>Refuses idf weights no artifact can hold; every save runs it before its first byte.</summary>
    /// <exception cref="InvalidDataException">A weight is not finite.</exception>
    /// <exception cref="InvalidOperationException">The weights' base64 block comes within two mebibytes of the most the JSON writer holds in one buffer.</exception>
    public static void EnsureWritableIdf(IReadOnlyList<double> idf)
    {
        RequireFinite(idf);

        // The block after the weights, so a model both would refuse is refused for its weights. A longer block failed
        // partway through the artifact (#1617).
        if (!Base64Numbers.WritableAsProperty(idf.Count, sizeof(double)))
        {
            throw new InvalidOperationException(
                $"{idf.Count} idf weights make a base64 block of {Base64Numbers.EncodedLength(idf.Count, sizeof(double))} "
                + "characters, within two mebibytes of the most the JSON writer holds in one buffer.");
        }
    }

    /// <summary>Refuses a non-finite weight: JSON has no NaN or infinity, and a model carrying one is broken already.</summary>
    /// <exception cref="InvalidDataException">A weight is not finite.</exception>
    private static void RequireFinite(IReadOnlyList<double> idf)
    {
        for (int i = 0; i < idf.Count; i++)
        {
            double value = idf[i];
            // Decision 0001's "non-finite values": refused on write, and again on read.
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new InvalidDataException(
                    $"Cannot persist a non-finite idf weight at index {i}: the model is broken before it reaches the file.");
            }
        }
    }

    /// <summary>Reads and bounds-checks the declared feature count.</summary>
    public static int ReadFeatureCount(ref Utf8JsonReader reader, string artifact, in ArtifactLimits limits)
    {
        int featureCount = JsonArtifact.ReadInt32(ref reader, artifact, FeatureCountProperty);
        if (featureCount < 0)
        {
            throw JsonArtifact.Inconsistent(artifact, $"{FeatureCountProperty} is negative ({featureCount}).");
        }
        limits.CheckVocabularySize(featureCount);
        return featureCount;
    }

    public static string[] ReadVocabulary(
        ref Utf8JsonReader reader, string artifact, in ArtifactLimits limits, int declaredCount, out bool mayHoldSurrogate)
    {
        // Only an escaped term can carry a lone surrogate, UTF-8 having no other way to; noted as read, for nothing (#1643).
        mayHoldSurrogate = false;
        JsonArtifact.ReadStartArray(ref reader, artifact, VocabularyProperty);

        string[] names = new string[InitialCapacity(declaredCount)];
        int count = 0;
        string? previous = null;
        while (reader.Read() && reader.TokenType == JsonTokenType.String)
        {
            mayHoldSurrogate |= reader.ValueIsEscaped;
            string name = JsonArtifact.GetText(ref reader);
            limits.CheckTokenLength(name.Length);
            // Checked inline, not in a second pass: the predecessor is already in
            // cache, and 30k strings do not need walking twice.
            if (previous is not null && CodePointOrder.Compare(previous.AsSpan(), name.AsSpan()) >= 0)
            {
                throw OutOfOrder(artifact, previous, name);
            }
            if (count == names.Length)
            {
                // SonarLint S2583: the zero-length case is reachable and covered by
                // A_vocabulary_written_before_the_feature_count_still_loads. The reader
                // accepts reordered properties, so 'vocabulary' can arrive before the
                // 'featureCount' that would have sized this buffer, leaving it empty.
                // The analyser cannot see that declaredCount may be -1 there.
#pragma warning disable S2583
                Array.Resize(ref names, names.Length == 0 ? 4 : names.Length * 2);
#pragma warning restore S2583
            }
            names[count++] = name;
            previous = name;
            limits.CheckVocabularySize(count);
        }
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw JsonArtifact.UnexpectedToken(artifact, VocabularyProperty, reader.TokenType);
        }

        if (count != names.Length)
        {
            Array.Resize(ref names, count);
        }
        return names;
    }

    /// <summary>Reads the base64 idf vector written by <see cref="WriteIdf"/>.</summary>
    public static double[] ReadIdf(ref Utf8JsonReader reader, string artifact, in ArtifactLimits limits)
    {
        double[] values = Base64Numbers.ReadDoubles(ref reader, artifact, IdfProperty, limits);
        for (int i = 0; i < values.Length; i++)
        {
            // Checked on read too, matching the write-side refusal (see EnsureWritableIdf,
            // and decision 0001's "non-finite values" row, for why).
            if (double.IsNaN(values[i]) || double.IsInfinity(values[i]))
            {
                throw JsonArtifact.Inconsistent(
                    artifact,
                    $"'{IdfProperty}' holds a value that is not finite, at index {i}.");
            }
        }
        return values;
    }

    /// <summary>Checks the declared feature count against what the arrays actually held.</summary>
    public static void EnsureDeclaredCount(string artifact, int declaredCount, int actualCount, string arrayName)
    {
        if (declaredCount != actualCount)
        {
            throw JsonArtifact.Inconsistent(
                artifact,
                $"{FeatureCountProperty} is {declaredCount} but '{arrayName}' holds {actualCount} entries.");
        }
    }

    /// <summary>
    /// How large to make the vocabulary buffer before reading it.
    /// </summary>
    /// <remarks>
    /// Sized from the declared count, so the common case allocates once at the
    /// right length instead of growing and copying as items arrive. The 64k
    /// ceiling then keeps a declared count from sizing the allocation on its own;
    /// <c>CheckVocabularySize</c> still bounds the total actually accepted.
    /// </remarks>
    private static int InitialCapacity(int declaredCount) =>
        declaredCount > 0 ? Math.Min(declaredCount, MaxPreallocatedEntries) : 0;

    private const int MaxPreallocatedEntries = 65_536;

    /// <summary>
    /// Names the way two consecutive vocabulary entries break the ordering contract.
    /// </summary>
    /// <remarks>
    /// The vectorizers index features by position in this array, and every lookup
    /// assumes it is the ordinal-sorted, duplicate-free list Fit produced. A file
    /// that breaks that would transform documents into the wrong columns — silently.
    /// </remarks>
    private static InvalidDataException OutOfOrder(string artifact, string previous, string current) =>
        string.Equals(previous, current, StringComparison.Ordinal)
            ? JsonArtifact.Inconsistent(artifact, $"'{VocabularyProperty}' contains the duplicate entry '{current}'.")
            : JsonArtifact.Inconsistent(
                artifact,
                $"'{VocabularyProperty}' must be sorted by code point, but '{previous}' precedes '{current}'.");
}
