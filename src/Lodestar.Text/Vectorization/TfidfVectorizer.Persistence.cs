using System.Buffers;
using System.Text.Json;
using Lodestar.Internal.Persistence;
using Lodestar.Text.Persistence;

namespace Lodestar.Text.Vectorization;

public sealed partial class TfidfVectorizer
{
    private const string ArtifactName = "tfidf-vectorizer";
    private const int ArtifactVersion = 1;

    /// <summary>
    /// Writes the fitted vectorizer — options, sorted vocabulary and idf vector —
    /// to <paramref name="destination"/> as UTF-8 JSON.
    /// </summary>
    /// <remarks>
    /// The Lodestar equivalent of <c>pickle.dump</c> / <c>joblib.dump</c> on a fitted
    /// <c>sklearn.feature_extraction.text.TfidfVectorizer</c> (format: see <see cref="CountVectorizer.Save(Stream)"/>).
    /// The idf vector round-trips bit-exact — raw IEEE-754, not a decimal <see cref="double"/> — and is
    /// always written, even when <c>UseIdf</c> is off, so the artifact stays lossless.
    /// </remarks>
    /// <param name="destination">The stream to write to. It is flushed but never disposed — the caller owns it.</param>
    /// <exception cref="InvalidOperationException">The vectorizer has not been fitted, or its idf weights make a base64 block within two mebibytes of the most the JSON writer holds.</exception>
    /// <exception cref="InvalidDataException">An idf weight is not finite; refused before anything is written.</exception>
    /// <exception cref="ArgumentException"><paramref name="destination"/> cannot be written to, refused once the vectorizer is known fitted, before its strings are checked (#1641); or a vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write, refused before anything is written with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    public void Save(Stream destination)
    {
        Guard.NotNull(destination);
        // Before the header: past it, a refusal leaves partial JSON in the caller's stream. The strings and weights once
        // the writer has accepted the stream, which refused a read-only one first on main (#1618).
        double[] idf = EnsureFitted();
        ArtifactIo.Save(destination, ArtifactName, ArtifactVersion, new Saved(this, idf));
    }

    /// <summary>Writes the fitted vectorizer to <paramref name="path"/>, replacing any existing file.</summary>
    /// <remarks>Equivalent to <c>joblib.dump(vectorizer, path)</c>; the file is UTF-8 without a byte-order mark.</remarks>
    /// <exception cref="InvalidOperationException">The vectorizer has not been fitted, refused before the path is opened; or its idf weights make a base64 block within two mebibytes of the most the JSON writer holds, refused once the file is open, before its first byte.</exception>
    /// <exception cref="ArgumentException">A vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write; refused once the file is open, before its first byte, with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="InvalidDataException">An idf weight is not finite; refused once the file is open, before its first byte.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    public void Save(string path)
    {
        // Fitted before opening, as main checked it; the strings and weights once the file is open and before its first
        // byte, where main's write met them, so a path opening refuses keeps its place (#1617, #1618).
        double[] idf = EnsureFitted();
        using FileStream file = JsonArtifact.OpenWrite(path);
        ArtifactIo.Save(file, ArtifactName, ArtifactVersion, new Saved(this, idf));
    }

    /// <summary>Asynchronous counterpart of <see cref="Save(Stream)"/>.</summary>
    /// <param name="destination">The stream to write to; never disposed by this method.</param>
    /// <exception cref="ArgumentNullException">the stream is null.</exception>
    /// <exception cref="InvalidOperationException">nothing has been fitted yet, or the idf weights make a base64 block within two mebibytes of the most the JSON writer holds.</exception>
    /// <exception cref="ArgumentException">a vocabulary term, the token pattern or a stop word is beyond what the JSON writer can write; refused before anything is written, with the writer's own exception, as 0.7.0 raised it (#1646).</exception>
    /// <exception cref="IndexOutOfRangeException">A string whose escaped form passes the JSON writer's buffer: the writer's own exception, as 0.7.0 raised it, before anything is written (#1646).</exception>
    /// <exception cref="InvalidDataException">an idf weight is not finite; refused before anything is written.</exception>
    /// <exception cref="NotSupportedException"><paramref name="destination"/> cannot be written to: the stream's own exception, raised by the write once the artifact is composed, as 0.7.0 raised it.</exception>
    /// <exception cref="OperationCanceledException">the token is cancelled.</exception>
    /// <param name="cancellationToken">Cancels the write.</param>
    public async Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
    {
        // The stream first, then the vectorizer before the header, as Save(Stream) checks them, so a refusal leaves
        // the stream untouched; inside the task, where every refusal of this method has always surfaced (#1617).
        Guard.NotNull(destination);
        EnsureSavable();
        await ArtifactIo.SaveAsync(destination, ArtifactName, ArtifactVersion, WriteArtifactBody, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a vectorizer previously written by <see cref="Save(Stream)"/>, ready
    /// to <see cref="Transform"/> without being fitted again.
    /// </summary>
    /// <remarks>
    /// The Lodestar equivalent of <c>pickle.load(f)</c> / <c>joblib.load</c> for a
    /// fitted <c>sklearn.feature_extraction.text.TfidfVectorizer</c> — reading
    /// data rather than code, under the bounds in <paramref name="options"/>.
    /// </remarks>
    /// <param name="source">The stream to read from; never disposed by this method.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static TfidfVectorizer Load(Stream source, ArtifactLoadOptions? options = null) =>
        Load(source, ArtifactLoadOptions.LimitsOf(options));

    /// <summary>The read, on limits already resolved: the seam a test reaches the segmented read from (#1618).</summary>
    internal static TfidfVectorizer Load(Stream source, in ArtifactLimits limits) =>
        FromPayload(JsonArtifact.ReadWhole(source, limits), limits);

    /// <summary>Reads a vectorizer from <paramref name="path"/>.</summary>
    /// <param name="path">The artifact file, as written by <see cref="Save(string)"/>.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static TfidfVectorizer Load(string path, ArtifactLoadOptions? options = null)
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
    public static async Task<TfidfVectorizer> LoadAsync(
        Stream source,
        ArtifactLoadOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await LoadAsync(source, ArtifactLoadOptions.LimitsOf(options), cancellationToken).ConfigureAwait(false);

    /// <summary>The asynchronous read, on limits already resolved: the seam a test drives (#1618).</summary>
    internal static async Task<TfidfVectorizer> LoadAsync(Stream source, ArtifactLimits limits, CancellationToken cancellationToken)
    {
        ReadOnlySequence<byte> payload = await JsonArtifact.ReadWholeAsync(source, limits, cancellationToken).ConfigureAwait(false);
        return FromPayload(payload, limits);
    }

    /// <summary>Throws unless there is a fitted model to write.</summary>
    /// <remarks>Called before any destination is opened.</remarks>
    private double[] EnsureFitted() =>
        _counts.IsFitted && _tfidf.FittedIdf is { } idf
            ? idf
            : throw new InvalidOperationException("The vectorizer has not been fitted. Call Fit or FitTransform first.");

    /// <summary>Throws unless the fitted model can be written, its idf weights and strings checked before any byte (#1617, #1618).</summary>
    private void EnsureSavable() => EnsureWritable(EnsureFitted());

    private readonly struct Saved : ArtifactIo.ISavedArtifact
    {
        private readonly TfidfVectorizer _owner;
        private readonly double[] _idf;

        public Saved(TfidfVectorizer owner, double[] idf)
        {
            _owner = owner;
            _idf = idf;
        }

        public void Check() => _owner.EnsureWritable(_idf);

        public void Write(Utf8JsonWriter writer) => _owner.WriteArtifactBody(writer);
    }

    /// <summary>Throws unless the fitted <paramref name="idf"/>, vocabulary and options can be written.</summary>
    private void EnsureWritable(double[] idf)
    {
        // In the order the artifact writes them, so each refusal stands where main's write failed.
        VectorizerOptionsJson.EnsureWritable(_counts.Options, _counts.AnalyzerStopWords);
        _counts.EnsureWritableVocabulary();
        FeatureVocabularyJson.EnsureWritableIdf(idf);
    }

    private void WriteArtifactBody(Utf8JsonWriter writer)
    {
        // Unreachable once a save has checked the model; it gives the compiler the non-null weights.
        if (_tfidf.FittedIdf is not { } idf)
        {
            throw new InvalidOperationException("The vectorizer has not been fitted. Call Fit or FitTransform first.");
        }

        string[] featureNames = _counts.FittedFeatureNames;
        VectorizerOptionsJson.Write(writer, "options", _counts.Options, _counts.AnalyzerStopWords);
        VectorizerOptionsJson.Write(writer, "tfidf", _tfidf.Options);
        writer.WriteNumber(FeatureVocabularyJson.FeatureCountProperty, featureNames.Length);
        FeatureVocabularyJson.WriteVocabulary(writer, featureNames, _counts.VocabularyMayHoldSurrogate);
        FeatureVocabularyJson.WriteIdf(writer, idf);
    }

    private static TfidfVectorizer FromPayload(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
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

    private static TfidfVectorizer Parse(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
    {
        Utf8JsonReader reader = ArtifactIo.CreateReader(payload, ArtifactName, limits);
        var header = new ArtifactHeader(ArtifactName, ArtifactVersion);

        CountVectorizerOptions? countOptions = null;
        TfidfOptions? tfidfOptions = null;
        string[]? vocabulary = null;
        bool mayHoldSurrogate = false;
        double[]? idf = null;
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
                    countOptions = VectorizerOptionsJson.ReadCount(ref reader, ArtifactName, limits);
                    break;
                case "tfidf":
                    tfidfOptions = VectorizerOptionsJson.ReadTfidf(ref reader, ArtifactName);
                    break;
                case FeatureVocabularyJson.FeatureCountProperty:
                    featureCount = FeatureVocabularyJson.ReadFeatureCount(ref reader, ArtifactName, limits);
                    break;
                case FeatureVocabularyJson.VocabularyProperty:
                    vocabulary = FeatureVocabularyJson.ReadVocabulary(ref reader, ArtifactName, limits, featureCount, out mayHoldSurrogate);
                    break;
                case FeatureVocabularyJson.IdfProperty:
                    idf = FeatureVocabularyJson.ReadIdf(ref reader, ArtifactName, limits);
                    break;
                default:
                    throw JsonArtifact.UnknownProperty(ArtifactName, name);
            }
        }

        ArtifactIo.EnsureEndOfDocument(ref reader, ArtifactName);
        header.EnsureComplete();
        RequirePresent(countOptions, "options");
        RequirePresent(tfidfOptions, "tfidf");
        RequirePresent(vocabulary, FeatureVocabularyJson.VocabularyProperty);
        RequirePresent(idf, FeatureVocabularyJson.IdfProperty);
        if (featureCount < 0)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, FeatureVocabularyJson.FeatureCountProperty);
        }
        FeatureVocabularyJson.EnsureDeclaredCount(ArtifactName, featureCount, vocabulary!.Length, FeatureVocabularyJson.VocabularyProperty);
        FeatureVocabularyJson.EnsureDeclaredCount(ArtifactName, featureCount, idf!.Length, FeatureVocabularyJson.IdfProperty);

        TfidfVectorizer vectorizer = VectorizerOptionsJson.Build(
            ArtifactName,
            () => new TfidfVectorizer(new TfidfVectorizerOptions { Count = countOptions!, Tfidf = tfidfOptions! }));
        vectorizer._counts.RestoreVocabulary(vocabulary, mayHoldSurrogate);
        vectorizer._tfidf.RestoreIdf(idf);
        return vectorizer;
    }

    private static void RequirePresent(object? value, string propertyName)
    {
        if (value is null)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, propertyName);
        }
    }
}
