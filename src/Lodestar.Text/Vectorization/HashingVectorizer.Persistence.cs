using System.Buffers;
using System.Text.Json;
using Lodestar.Internal.Persistence;
using Lodestar.Text.Persistence;

namespace Lodestar.Text.Vectorization;

public sealed partial class HashingVectorizer
{
    private const string ArtifactName = "hashing-vectorizer";
    private const int ArtifactVersion = 1;

    /// <summary>Writes the vectorizer's configuration to <paramref name="destination"/> as UTF-8 JSON.</summary>
    /// <remarks>
    /// The Lodestar equivalent of <c>joblib.dump</c> on a <c>sklearn.feature_extraction.text.HashingVectorizer</c>
    /// (format: see <see cref="CountVectorizer.Save(Stream)"/>). Hashing has no vocabulary to learn, but the
    /// configuration still matters: a pipeline reloaded with a different <c>NumFeatures</c>,
    /// <c>AlternateSign</c> or analyzer silently produces different columns for the same document.
    /// </remarks>
    /// <exception cref="InvalidOperationException">The token pattern or a stop word is longer than the JSON writer accepts; refused before anything is written.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    /// <param name="destination">The stream to write to. It is flushed but never disposed — the caller owns it.</param>
    public void Save(Stream destination)
    {
        // The strings once the writer has accepted the stream, which refused a read-only one first on main (#1618).
        ArtifactIo.Save(
            destination, ArtifactName, ArtifactVersion, WriteArtifactBody, () => VectorizerOptionsJson.EnsureWritable(_options.Count));
    }

    /// <summary>Writes the vectorizer's configuration to <paramref name="path"/>, replacing any existing file.</summary>
    /// <exception cref="InvalidOperationException">The token pattern or a stop word is longer than the JSON writer accepts; refused once the file is open, before its first byte.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    /// <exception cref="IOException">the stream or file system refuses the write.</exception>
    /// <remarks>Equivalent to <c>joblib.dump(vectorizer, path)</c>; the file is UTF-8 without a byte-order mark.</remarks>
    public void Save(string path)
    {
        // The strings once the file is open and before its first byte, where main's write failed on them (#1618).
        using FileStream file = JsonArtifact.OpenWrite(path);
        ArtifactIo.Save(file, ArtifactName, ArtifactVersion, WriteArtifactBody, () => VectorizerOptionsJson.EnsureWritable(_options.Count));
    }

    /// <summary>Asynchronous counterpart of <see cref="Save(Stream)"/>.</summary>
    /// <param name="destination">The stream to write to; never disposed by this method.</param>
    /// <exception cref="ArgumentNullException">the stream is null.</exception>
    /// <exception cref="InvalidOperationException">The token pattern or a stop word is longer than the JSON writer accepts; refused before anything is written.</exception>
    /// <exception cref="OperationCanceledException">the token is cancelled.</exception>
    /// <param name="cancellationToken">Cancels the write.</param>
    public async Task SaveAsync(Stream destination, CancellationToken cancellationToken = default)
    {
        // Inside the task, the stream first, as every refusal of this method has surfaced (#1618).
        Guard.NotNull(destination);
        VectorizerOptionsJson.EnsureWritable(_options.Count);
        await ArtifactIo.SaveAsync(destination, ArtifactName, ArtifactVersion, WriteArtifactBody, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>Reads a vectorizer configuration previously written by <see cref="Save(Stream)"/>.</summary>
    /// <remarks>The Lodestar equivalent of <c>joblib.load</c> for a <c>sklearn.feature_extraction.text.HashingVectorizer</c>.</remarks>
    /// <param name="source">The stream to read from; never disposed by this method.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static HashingVectorizer Load(Stream source, ArtifactLoadOptions? options = null) =>
        Load(source, ArtifactLoadOptions.LimitsOf(options));

    /// <summary>The read, on limits already resolved: the seam a test reaches the segmented read from (#1618).</summary>
    internal static HashingVectorizer Load(Stream source, in ArtifactLimits limits) =>
        FromPayload(JsonArtifact.ReadWhole(source, limits), limits);

    /// <summary>Reads a vectorizer configuration from <paramref name="path"/>.</summary>
    /// <param name="path">The artifact file, as written by <see cref="Save(string)"/>.</param>
    /// <param name="options">Bounds applied while reading, or <c>null</c> for the defaults.</param>
    /// <exception cref="InvalidDataException">The artifact is malformed, of the wrong kind, of an unsupported version, or exceeds a limit.</exception>
    /// <exception cref="ArgumentNullException">the stream or path is null.</exception>
    public static HashingVectorizer Load(string path, ArtifactLoadOptions? options = null)
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
    public static async Task<HashingVectorizer> LoadAsync(
        Stream source,
        ArtifactLoadOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await LoadAsync(source, ArtifactLoadOptions.LimitsOf(options), cancellationToken).ConfigureAwait(false);

    /// <summary>The asynchronous read, on limits already resolved: the seam a test drives (#1618).</summary>
    internal static async Task<HashingVectorizer> LoadAsync(Stream source, ArtifactLimits limits, CancellationToken cancellationToken)
    {
        ReadOnlySequence<byte> payload = await JsonArtifact.ReadWholeAsync(source, limits, cancellationToken).ConfigureAwait(false);
        return FromPayload(payload, limits);
    }

    private void WriteArtifactBody(Utf8JsonWriter writer)
    {
        VectorizerOptionsJson.Write(writer, "options", _options.Count);
        writer.WriteNumber("numFeatures", _options.NumFeatures);
        writer.WriteBoolean("alternateSign", _options.AlternateSign);
        VectorizerOptionsJson.WriteNorm(writer, "norm", _options.Norm);
    }

    private static HashingVectorizer FromPayload(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
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

    private static HashingVectorizer Parse(ReadOnlySequence<byte> payload, in ArtifactLimits limits)
    {
        Utf8JsonReader reader = ArtifactIo.CreateReader(payload, ArtifactName, limits);
        var header = new ArtifactHeader(ArtifactName, ArtifactVersion);

        var result = new HashingVectorizerOptions();
        CountVectorizerOptions? countOptions = null;

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
                case "numFeatures":
                    result = result with { NumFeatures = JsonArtifact.ReadInt32(ref reader, ArtifactName, name) };
                    break;
                case "alternateSign":
                    result = result with { AlternateSign = JsonArtifact.ReadBoolean(ref reader, ArtifactName, name) };
                    break;
                case "norm":
                    result = result with
                    {
                        Norm = VectorizerOptionsJson.ParseNorm(JsonArtifact.ReadNullableString(ref reader, ArtifactName, name), ArtifactName),
                    };
                    break;
                default:
                    throw JsonArtifact.UnknownProperty(ArtifactName, name);
            }
        }

        ArtifactIo.EnsureEndOfDocument(ref reader, ArtifactName);
        header.EnsureComplete();
        if (countOptions is null)
        {
            throw JsonArtifact.MissingProperty(ArtifactName, "options");
        }
        if (result.NumFeatures < 1)
        {
            throw JsonArtifact.Inconsistent(ArtifactName, $"numFeatures must be at least 1 but is {result.NumFeatures}.");
        }
        return VectorizerOptionsJson.Build(ArtifactName, () => new HashingVectorizer(result with { Count = countOptions }));
    }
}
