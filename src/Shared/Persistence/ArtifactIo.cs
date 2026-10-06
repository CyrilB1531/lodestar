using System.Buffers;
using System.Text.Json;

namespace Lodestar.Internal.Persistence;

/// <summary>
/// The save/load skeleton every Lodestar artifact shares: open the object, write
/// the header, let the artifact write its body, and on the way back read the whole
/// (byte-capped) payload, in one buffer or past it in segments, for a single reader pass.
/// </summary>
/// <remarks>
/// Streams passed in by the caller are never disposed here — the caller owns
/// them. The <c>string path</c> overloads on each artifact own the
/// <see cref="FileStream"/> they open, and dispose it.
/// </remarks>
internal static class ArtifactIo
{
    /// <summary>What a synchronous save writes, and the refusals it makes once the writer has accepted the stream.</summary>
    /// <remarks>
    /// A struct the save is generic over rather than two delegates: a delegate stays off the heap only while the save is
    /// inlined into its caller, which its try block prevents, and two cost a large save 64 to 190 bytes over the
    /// releases' (#1659).
    /// </remarks>
    internal interface ISavedArtifact
    {
        /// <summary>Refuses, before the first byte, what the write would fail on.</summary>
        void Check();

        /// <summary>Writes every property after the header, or before the block.</summary>
        void Write(Utf8JsonWriter writer);
    }

    /// <summary>The brace that closes an artifact, written by hand when the writer cannot.</summary>
    private const byte CloseBrace = (byte)'}';

    /// <param name="destination">The stream to write to; flushed but never disposed.</param>
    /// <param name="artifact">The artifact kind, for the header.</param>
    /// <param name="version">The artifact version, for the header.</param>
    /// <param name="body">
    /// Checked once the writer has accepted the stream and before its first byte, as main's write met them (#1618), then
    /// written after the header.
    /// </param>
    public static void Save<TArtifact>(Stream destination, string artifact, int version, TArtifact body)
        where TArtifact : struct, ISavedArtifact
    {
        Guard.NotNull(destination);
        using var writer = new Utf8JsonWriter(destination, JsonArtifact.WriterOptions);
        try
        {
            body.Check();
            writer.WriteStartObject();
            ArtifactHeader.Write(writer, artifact, version);
            body.Write(writer);
            writer.WriteEndObject();
            writer.Flush();
        }
        catch
        {
            // As SaveWithBlock: disposing flushes nothing pending into the caller's stream, whose own failure would
            // otherwise mask the refusal SaveAsync raises (#1641).
            writer.Reset(Stream.Null);
            throw;
        }
    }

    public static async Task SaveAsync(
        Stream destination,
        string artifact,
        int version,
        Action<Utf8JsonWriter> writeBody,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(destination);

        // The body writes through a synchronous Utf8JsonWriter; composed in memory, it never
        // touches the destination, and the copy to it is what the await covers.
        using var buffer = new SpillBuffer();
        // Inline, as 0.7.0 wrote it: a lambda here was a closure and a delegate a save (#1649).
        using (var writer = new Utf8JsonWriter(buffer, JsonArtifact.WriterOptions))
        {
            WriteDocument(writer, artifact, version, writeBody);
            // CA1849 / S6966: the destination is the in-memory buffer, whose writes do no I/O; the copy out is awaited.
#pragma warning disable S6966, CA1849
            writer.Flush();
#pragma warning restore S6966, CA1849
        }
        await buffer.CopyOutAsync(destination, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Saves an artifact whose last property is a float block too large to buffer,
    /// writing that block to <paramref name="destination"/> a slice at a time.
    /// </summary>
    /// <remarks>
    /// <paramref name="head"/> writes every property before the block. <b>Nothing goes through
    /// the <c>Utf8JsonWriter</c> after it</b> — the writer is flushed and disposed on the property
    /// name, the value goes to the stream, and the closing brace is written by hand, because a
    /// writer left on one refuses to close its object. Owning it here, no artifact can get it wrong.
    /// </remarks>
    /// <param name="destination">The stream to write to; flushed but never disposed.</param>
    /// <param name="artifact">The artifact kind, for the header.</param>
    /// <param name="version">The artifact version, for the header.</param>
    /// <param name="head">Checks before the first byte, once the writer has the stream (#1618); writes what precedes the block.</param>
    /// <param name="blockProperty">The name of the block's property.</param>
    /// <param name="block">The float block, written as base64 raw little-endian bits.</param>
    public static void SaveWithBlock<TArtifact>(
        Stream destination,
        string artifact,
        int version,
        TArtifact head,
        string blockProperty,
        ReadOnlySpan<float> block)
        where TArtifact : struct, ISavedArtifact
    {
        Guard.NotNull(destination);

        using (var writer = new Utf8JsonWriter(destination, JsonArtifact.WriterOptions))
        {
            try
            {
                head.Check();
                writer.WriteStartObject();
                ArtifactHeader.Write(writer, artifact, version);
                head.Write(writer);
                writer.WritePropertyName(blockProperty);
                writer.Flush();
            }
            catch
            {
                // Pointed away, as the asynchronous save points it, so disposing it flushes nothing a refusal left
                // pending into the caller's stream, and the stream's own failure cannot mask the refusal (#1641).
                writer.Reset(Stream.Null);
                throw;
            }
        }

        Base64Numbers.WriteSinglesChunked(destination, block);
        destination.WriteByte(CloseBrace);
        destination.Flush();
    }

    /// <summary>The asynchronous counterpart of <see cref="SaveWithBlock"/>.</summary>
    /// <remarks>
    /// The head goes through a writer on the stream, as it did before #1618, flushed asynchronously at each point
    /// <paramref name="writeHead"/> yields, a mebibyte or so: a head of any length costs that much buffer and no copy of
    /// itself (#1635). A refusal before the first flush leaves nothing written. The block, every byte that makes this
    /// artifact large, is written through <c>WriteAsync</c> a slice at a time.
    /// </remarks>
    /// <param name="destination">The stream to write to; flushed but never disposed.</param>
    /// <param name="artifact">The artifact kind, for the header.</param>
    /// <param name="version">The artifact version, for the header.</param>
    /// <param name="writeHead">Writes every property before the block, yielding where its writer flushed.</param>
    /// <param name="blockProperty">The name of the block's property.</param>
    /// <param name="block">The float block, written as base64 raw little-endian bits.</param>
    /// <param name="cancellationToken">Cancels the writes.</param>
    public static async Task SaveWithBlockAsync(
        Stream destination,
        string artifact,
        int version,
        Func<Utf8JsonWriter, IEnumerable<bool>> writeHead,
        string blockProperty,
        ReadOnlyMemory<float> block,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(destination);

        using (var writer = new Utf8JsonWriter(destination, JsonArtifact.WriterOptions))
        {
            try
            {
                writer.WriteStartObject();
                ArtifactHeader.Write(writer, artifact, version);
                foreach (bool _ in writeHead(writer))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
                }

                writer.WritePropertyName(blockProperty);
                await writer.FlushAsync(cancellationToken).ConfigureAwait(false);

                // Nothing is pending, and disposing would still flush the caller's stream synchronously (#1633).
                writer.Reset(Stream.Null);
            }
            catch
            {
                // Pointed away, so disposing it never flushes what a refusal left pending into the caller's stream.
                writer.Reset(Stream.Null);
                throw;
            }
        }

        await Base64Numbers.WriteSinglesChunkedAsync(destination, block, cancellationToken).ConfigureAwait(false);

        byte[] closing = [CloseBrace];
#if NETSTANDARD2_0
        await destination.WriteAsync(closing, 0, 1, cancellationToken).ConfigureAwait(false);
#else
        await destination.WriteAsync(closing.AsMemory(0, 1), cancellationToken).ConfigureAwait(false);
#endif
        await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Creates a reader over <paramref name="payload"/> positioned on the artifact's opening brace.</summary>
    public static Utf8JsonReader CreateReader(ReadOnlySpan<byte> payload, string artifact, in ArtifactLimits limits)
    {
        var reader = new Utf8JsonReader(payload, JsonArtifact.ReaderOptions(limits));
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
        {
            throw new InvalidDataException($"A '{ArtifactHeader.SchemaFor(artifact)}' artifact must be a JSON object.");
        }
        return reader;
    }

    /// <summary>The same reader over an artifact too large for one array (#377).</summary>
    /// <param name="payload">The artifact, in segments none of which exceeds one array.</param>
    /// <param name="artifact">The artifact kind, for the message.</param>
    /// <param name="limits">Bounds applied while reading.</param>
    public static Utf8JsonReader CreateReader(ReadOnlySequence<byte> payload, string artifact, in ArtifactLimits limits)
    {
        var reader = new Utf8JsonReader(payload, JsonArtifact.ReaderOptions(limits));
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
        {
            throw new InvalidDataException($"A '{ArtifactHeader.SchemaFor(artifact)}' artifact must be a JSON object.");
        }
        return reader;
    }

    /// <summary>Checks that the artifact's object closed cleanly and that nothing follows it.</summary>
    /// <remarks>
    /// The final <c>Read</c> is what forces the point: the reader sees the whole
    /// payload as a final block, so a trailing token makes it raise a
    /// <see cref="JsonException"/> that <see cref="Malformed"/> restates. Without
    /// that call, an artifact with junk appended would load as if it were clean.
    /// The explicit throw after it is a belt-and-braces guard for a reader that
    /// might one day report the condition instead of raising.
    /// </remarks>
    public static void EnsureEndOfDocument(ref Utf8JsonReader reader, string artifact)
    {
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw JsonArtifact.Truncated(artifact);
        }
        if (reader.Read())
        {
            throw new InvalidDataException($"Trailing content after the end of a '{ArtifactHeader.SchemaFor(artifact)}' artifact.");
        }
    }

    /// <summary>
    /// Restates a JSON syntax error as the <see cref="InvalidDataException"/> the
    /// public API documents, keeping the parser's message as the inner exception.
    /// </summary>
    /// <remarks>
    /// Callers should not have to catch two exception types depending on whether a
    /// bad file broke the grammar or broke the schema.
    /// </remarks>
    public static InvalidDataException Malformed(string artifact, JsonException inner) =>
        new($"A '{ArtifactHeader.SchemaFor(artifact)}' artifact is not well-formed JSON: {inner.Message}", inner);

    private static void WriteDocument(Utf8JsonWriter writer, string artifact, int version, Action<Utf8JsonWriter> writeBody)
    {
        writer.WriteStartObject();
        ArtifactHeader.Write(writer, artifact, version);
        writeBody(writer);
        writer.WriteEndObject();
    }
}
