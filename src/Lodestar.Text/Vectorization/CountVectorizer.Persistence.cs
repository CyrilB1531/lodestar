using System.Buffers;
using System.Text.Json;
using Lodestar.Internal.Persistence;
using Lodestar.Text.Persistence;

namespace Lodestar.Text.Vectorization;

public sealed partial class CountVectorizer
{
    private const string ArtifactName = "count-vectorizer";
    private const int ArtifactVersion = 1;

    /// <summary>
    /// Writes the fitted vectorizer — its options and its sorted vocabulary — to
    /// <paramref name="destination"/> as UTF-8 JSON.
    /// </summary>
    /// <remarks>
    /// The Lodestar equivalent of <c>pickle.dump</c> / <c>joblib.dump</c> on a fitted
    /// <c>sklearn.feature_extraction.text.CountVectorizer</c>, as versioned JSON rather than an executable
    /// pickle — see <see cref="ArtifactLoadOptions"/> and the "Saving a fitted model" section of
    /// <c>docs/guides/vectorization.md</c>.
    /// </remarks>
    /// <param name="destination">The stream to write to. It is flushed but never disposed — the caller owns it.</param>
    /// <exception cref="InvalidOperationException">The vectorizer has not been fitted; refused before anything is written.</exception>
    /// <exception cref="ArgumentException"><paramref name="destination"/> cannot be written to, refused once the vectorizer is known fitted, before its strings are checked (#1641); or a vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write, refused before anything is written with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    public void Save(Stream destination)
    {
        Guard.NotNull(destination);
        // Before the header: past it, a refusal leaves partial JSON in the caller's stream. The strings once the writer
        // has accepted the stream, which refused a read-only one first on main (#1618).
        EnsureFitted();
        ArtifactIo.Save(destination, ArtifactName, ArtifactVersion, WriteArtifactBody, EnsureWritable);
    }

    /// <summary>Writes the fitted vectorizer to <paramref name="path"/>, replacing any existing file.</summary>
    /// <remarks>Equivalent to <c>joblib.dump(vectorizer, path)</c>; the file is UTF-8 without a byte-order mark.</remarks>
    /// <exception cref="InvalidOperationException">The vectorizer has not been fitted, refused before the path is opened.</exception>
    /// <exception cref="ArgumentException">A vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write; refused once the file is open, before its first byte, with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    public void Save(string path)
    {
        // Fitted before opening, as main checked it; the strings once the file is open and before its first byte, where
        // main's write failed on them, so a path opening refuses keeps its place (#1618).
        EnsureFitted();
        using FileStream file = JsonArtifact.OpenWrite(path);
        ArtifactIo.Save(file, ArtifactName, ArtifactVersion, WriteArtifactBody, EnsureWritable);
    }

    /// <summary>Asynchronous counterpart of <see cref="Save(Stream)"/>.</summary>
    /// <param name="destination">The stream to write to; never disposed by this method.</param>
    /// <exception cref="ArgumentNullException">the stream is null.</exception>
    /// <exception cref="InvalidOperationException">nothing has been fitted yet; refused before anything is written.</exception>
    /// <exception cref="ArgumentException">a vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write; refused before anything is written, with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="NotSupportedException"><paramref name="destination"/> cannot be written to: the stream's own exception, raised by the write once the artifact is composed, as 0.7.0 raised it.</exception>
    /// <exception cref="OperationCanceledException">the token is cancelled.</exception>
    /// <param name="cancellationToken">Cancels the write.</param>
    public async Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
    {
        // The stream first, as Save(Stream) checks it; inside the task, where every refusal here has surfaced (#1618).
        Guard.NotNull(destination);
        EnsureSavable();
        await ArtifactIo.SaveAsync(destination, ArtifactName, ArtifactVersion, WriteArtifactBody, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>Throws unless the fitted model can be written whole, every string checked before any byte (#1618).</summary>
    private void EnsureSavable()
    {
        EnsureFitted();
        EnsureWritable();
    }

    /// <summary>Throws unless every string the artifact holds can be written.</summary>
    private void EnsureWritable()
    {
        VectorizerOptionsJson.EnsureWritable(_options, _analyzer.StopWords);
        EnsureWritableVocabulary();
    }

    /// <summary>Refuses a term the writer cannot write, measuring none when the longest is short enough (#1643).</summary>
    internal void EnsureWritableVocabulary()
    {
        if (_longestTerm > JsonArtifact.AlwaysWritableCharacters)
        {
            FeatureVocabularyJson.EnsureWritableVocabulary(_featureNames);
        }
    }

    /// <summary>
    /// Reads a vectorizer previously written by <see cref="Save(Stream)"/>, ready
    /// to <see cref="Transform"/> without being fitted again.
    /// </summary>
    /// <remarks>
    /// The Lodestar equivalent of <c>pickle.load(f)</c> / <c>joblib.load</c> for a
    /// fitted <c>sklearn.feature_extraction.text.CountVectorizer</c> — with the
    /// difference that this reads data, never code, and enforces the bounds in
    /// <paramref name="options"/>.
    /// </remarks>
    /// <param name="source">The stream to read from; never disposed by this method.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static CountVectorizer Load(Stream source, ArtifactLoadOptions? options = null) =>
        Load(source, ArtifactLoadOptions.LimitsOf(options));

    /// <summary>The read, on limits already resolved: the seam a test reaches the segmented read from (#1618).</summary>
    internal static CountVectorizer Load(Stream source, in ArtifactLimits limits) =>
        FromPayload(JsonArtifact.ReadWhole(source, limits), limits);

    /// <summary>Reads a vectorizer from <paramref name="path"/>.</summary>
    /// <param name="path">The artifact file, as written by <see cref="Save(string)"/>.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static CountVectorizer Load(string path, ArtifactLoadOptions? options = null)
    {
        using FileStream file = JsonArtifact.OpenRead(path);
        return Load(file, options);
    }

    /// <summary>Asynchronous counterpart of <see cref="Load(Stream, ArtifactLoadOptions?)"/>.</summary>
    /// <param name="source">The stream to read from; never disposed by this method.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="ArgumentNullException">the stream is null.</exception>
    /// <exception cref="InvalidDataException">the content is not a saved vectorizer, or exceeds a bound.</exception>
    /// <exception cref="OperationCanceledException">the token is cancelled.</exception>
    /// <param name="cancellationToken">Cancels the read.</param>
    public static async Task<CountVectorizer> LoadAsync(
        Stream source,
        ArtifactLoadOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await LoadAsync(source, ArtifactLoadOptions.LimitsOf(options), cancellationToken).ConfigureAwait(false);

    /// <summary>The asynchronous read, on limits already resolved: the seam a test drives (#1618).</summary>
    internal static async Task<CountVectorizer> LoadAsync(Stream source, ArtifactLimits limits, CancellationToken cancellationToken)
    {
        ReadOnlySequence<byte> payload = await JsonArtifact.ReadWholeAsync(source, limits, cancellationToken).ConfigureAwait(false);
        return FromPayload(payload, limits);
    }

    /// <summary>The options this vectorizer was built with — needed by <see cref="TfidfVectorizer"/>'s artifact.</summary>
    internal CountVectorizerOptions Options => _options;

    /// <summary>The fitted feature names, or an empty array if never fitted.</summary>
    internal string[] FittedFeatureNames => _featureNames;

    /// <summary>The stop words the analyzer filters with, which a save writes (#1648).</summary>
    internal IReadOnlyCollection<string>? AnalyzerStopWords => _analyzer.StopWords;

    /// <summary>Whether <see cref="Fit"/> has run.</summary>
    internal bool IsFitted => _vocabulary is not null;

    /// <summary>Whether a fitted term may hold a lone surrogate, which the vocabulary's save then searches each term for.</summary>
    internal bool VocabularyMayHoldSurrogate => _vocabularyMayHoldSurrogate;

    /// <summary>Rebuilds the fitted state from an artifact's already-validated vocabulary.</summary>
    internal void RestoreVocabulary(string[] sortedFeatureNames, bool mayHoldSurrogate)
    {
        _vocabularyMayHoldSurrogate = mayHoldSurrogate;
        int longest = 0;
        var vocabulary = new Dictionary<string, int>(sortedFeatureNames.Length, StringComparer.Ordinal);
        for (int i = 0; i < sortedFeatureNames.Length; i++)
        {
            longest = Math.Max(longest, sortedFeatureNames[i].Length);
            vocabulary[sortedFeatureNames[i]] = i;
        }
        _longestTerm = longest;
        _featureNames = sortedFeatureNames;
        _vocabulary = vocabulary;
    }

    /// <summary>Writes the fitted body of a <c>datanet/count-vectorizer</c> artifact.</summary>
    internal void WriteArtifactBody(Utf8JsonWriter writer)
    {
        EnsureFitted();
        VectorizerOptionsJson.Write(writer, "options", _options, _analyzer.StopWords);
        writer.WriteNumber(FeatureVocabularyJson.FeatureCountProperty, _featureNames.Length);
        FeatureVocabularyJson.WriteVocabulary(writer, _featureNames, _vocabularyMayHoldSurrogate);
    }

    private static CountVectorizer FromPayload(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
    {
        try
        {
            return Parse(payload, limits);
        }
        catch (JsonException e)
        {
            throw ArtifactIo.Malformed(ArtifactName, e);
        }
    }

    private static CountVectorizer Parse(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
    {
        Utf8JsonReader reader = ArtifactIo.CreateReader(payload, ArtifactName, limits);
        var header = new ArtifactHeader(ArtifactName, ArtifactVersion);

        CountVectorizerOptions? options = null;
        string[]? vocabulary = null;
        bool mayHoldSurrogate = false;
        int featureCount = -1;

        while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
        {
            string name = reader.GetString()!;
            if (header.TryConsume(ref reader, name))
            {
                continue;
            }
            switch (name)
            {
                case "options":
                    options = VectorizerOptionsJson.ReadCount(ref reader, ArtifactName, limits);
                    break;
                case FeatureVocabularyJson.FeatureCountProperty:
                    featureCount = FeatureVocabularyJson.ReadFeatureCount(ref reader, ArtifactName, limits);
                    break;
                case FeatureVocabularyJson.VocabularyProperty:
                    vocabulary = FeatureVocabularyJson.ReadVocabulary(ref reader, ArtifactName, limits, featureCount, out mayHoldSurrogate);
                    break;
                default:
                    throw JsonArtifact.UnknownProperty(ArtifactName, name);
            }
        }

        ArtifactIo.EnsureEndOfDocument(ref reader, ArtifactName);
        header.EnsureComplete();
        if (options is null)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, "options");
        }
        if (vocabulary is null)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, FeatureVocabularyJson.VocabularyProperty);
        }
        if (featureCount < 0)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, FeatureVocabularyJson.FeatureCountProperty);
        }
        FeatureVocabularyJson.EnsureDeclaredCount(ArtifactName, featureCount, vocabulary.Length, FeatureVocabularyJson.VocabularyProperty);

        CountVectorizer vectorizer = VectorizerOptionsJson.Build(ArtifactName, () => new CountVectorizer(options));
        vectorizer.RestoreVocabulary(vocabulary, mayHoldSurrogate);
        return vectorizer;
    }
}
