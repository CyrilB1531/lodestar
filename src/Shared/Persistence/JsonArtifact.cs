using System.Buffers;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Lodestar.Internal.Persistence;

/// <summary>
/// The JSON reading and writing primitives shared by every Lodestar artifact:
/// byte-capped stream reads, exact <see cref="double"/> formatting, and the
/// exception shapes the public API documents.
/// </summary>
/// <remarks>
/// Artifacts are read in one pass over one buffer, or segments past one array, not a <c>JsonDocument</c>
/// tree: the bytes are what <c>MaxTotalBytes</c> bounds, and the reader allocates nothing per token.
/// </remarks>
internal static class JsonArtifact
{
    /// <summary>UTF-8 without a byte-order mark — what every artifact is written in.</summary>
    /// <remarks>
    /// <see cref="Utf8JsonWriter"/> emits no preamble of its own, so writing is
    /// BOM-free by construction; this instance exists for the paths that need an
    /// explicit encoder, and throws on invalid surrogates rather than substituting.
    /// </remarks>
    public static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>Writer options for artifacts: compact, and validated as it is written.</summary>
    /// <remarks>
    /// The relaxed encoder is deliberate: the default escapes every non-ASCII character as
    /// <c>\uXXXX</c>, and this library ships accented Snowball stop-word lists. "Unsafe" names an
    /// HTML-injection concern that does not apply — an artifact is read back by this library's own
    /// parser, never dropped into a <c>&lt;script&gt;</c> block. See ADR 0001, "Escaping".
    /// </remarks>
    public static JsonWriterOptions WriterOptions => new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Indented = false,
        // Kept on: the structural check is cheap for a writer to perform, and catches
        // a malformed artifact at the point a writer bug would produce one.
        SkipValidation = false,
    };

    /// <summary>Reader options honouring the caller's depth limit; comments and trailing commas are rejected.</summary>
    public static JsonReaderOptions ReaderOptions(in ArtifactLimits limits) => new()
    {
        MaxDepth = limits.MaxJsonDepth,
        CommentHandling = JsonCommentHandling.Disallow,
        AllowTrailingCommas = false,
    };

    /// <summary>
    /// Writes <paramref name="value"/> as a JSON number that reads back as the same
    /// <see cref="double"/>, bit for bit.
    /// </summary>
    /// <remarks>
    /// From <c>net8.0</c>, <see cref="Utf8JsonWriter.WriteNumberValue(double)"/> emits the shortest
    /// round-tripping form, exact since .NET Core 3.0. A <c>netstandard2.0</c> build may run on .NET
    /// Framework, where that is not guaranteed, so it keeps invariant <c>"G17"</c> instead — exact
    /// everywhere, at the cost of longer numbers. See ADR 0001, "Doubles": each build is byte-reproducible against itself, not the two against each other.
    /// </remarks>
    public static void WriteExactDouble(Utf8JsonWriter writer, double value)
    {
        RequirePersistable(value);
#if NETSTANDARD2_0
        writer.WriteRawValue(value.ToString("G17", CultureInfo.InvariantCulture), skipInputValidation: true);
#else
        writer.WriteNumberValue(value);
#endif
    }

    /// <summary>Writes a named property whose value is an exactly round-tripping double.</summary>
    public static void WriteExactDouble(Utf8JsonWriter writer, string propertyName, double value)
    {
        writer.WritePropertyName(propertyName);
        WriteExactDouble(writer, value);
    }

    /// <summary>Refuses a value <see cref="WriteExactDouble(Utf8JsonWriter, double)"/> cannot write, in its words.</summary>
    /// <exception cref="InvalidDataException"><paramref name="value"/> is not finite.</exception>
    public static void RequirePersistable(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new InvalidDataException(
                $"Cannot persist the non-finite value {value.ToString(CultureInfo.InvariantCulture)}: JSON has no representation for it.");
        }
    }

    /// <summary>Reads <paramref name="stream"/> to its end, failing past <c>MaxTotalBytes</c>.</summary>
    /// <remarks>
    /// Never disposed — ownership stays with the caller. On the growable path the returned buffer
    /// is longer than the payload, so read it as the <see cref="ReadOnlyMemory{T}"/> it comes back
    /// as: reaching for the array behind it, or for <c>.ToArray()</c>, both reintroduce the copy this
    /// return type exists to remove and expose a tail of zeroes past the payload's end.
    /// </remarks>
    public static ReadOnlyMemory<byte> ReadAllBytes(Stream stream, in ArtifactLimits limits)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        if (TryReadDeclaredLength(stream, limits.MaxSingleBuffer, pooled: false, out byte[] exact, out int filled))
        {
            return new ReadOnlyMemory<byte>(exact, 0, filled);
        }

        return ReadGrowable(stream, limits);
    }

    /// <summary>As <see cref="ReadAllBytes"/>, but the buffer is rented and the caller returns it.</summary>
    /// <remarks>
    /// For a call site that parses the payload and keeps nothing of it, which is the load
    /// paths: the bytes become strings and arrays of their own, so the buffer is dead the
    /// moment parsing ends. <b>Never below a method that also serves a caller's own memory</b>
    /// — <c>EmbeddingIndex.FromPayload</c> is reached from a public overload taking
    /// <see cref="ReadOnlyMemory{T}"/>, and pooling there would return a caller's buffer.
    /// </remarks>
    public static Buffers.RentedPayload ReadAllBytesPooled(Stream stream, in ArtifactLimits limits)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        if (TryReadDeclaredLength(stream, limits.MaxSingleBuffer, pooled: true, out byte[] rented, out int filled))
        {
            return Buffers.RentedPayload.Rented(rented, filled);
        }

        return ReadGrowablePooled(stream, limits);
    }

    /// <summary>How much of an undeclared-length stream one rented segment holds: 1 MiB.</summary>
    /// <remarks>
    /// Small enough that the pool's power-of-two rounding wastes nothing, large enough that a
    /// 20 MB index is a score of reads rather than thousands.
    /// </remarks>
    private const int GrowableSegmentBytes = 1 << 20;

    /// <summary>Reads a stream with no declared length into rented segments, then one rented buffer.</summary>
    /// <remarks>
    /// The growable <see cref="MemoryStream"/> this replaces doubled into fresh zeroed arrays, so a
    /// 20 MB payload paid ~40 MB of allocation, page commits and copies of everything read so far —
    /// ~13 ms over the inflate on a gzip-wrapped index. Segments are never copied until the length is
    /// known, and then once.
    /// </remarks>
    private static Buffers.RentedPayload ReadGrowablePooled(Stream stream, in ArtifactLimits limits)
    {
        // Never past the ceiling itself, so a test reaches the chained payload at a few bytes.
        int segmentBytes = (int)Math.Min(limits.MaxSingleBuffer, GrowableSegmentBytes);
        var segments = new List<byte[]>();
        byte[]? current = ArrayPool<byte>.Shared.Rent(segmentBytes);
        byte[]? payload = null;
        try
        {
            int inCurrent = 0;
            long total = 0;
            int read;
            while ((read = stream.Read(current, inCurrent, current.Length - inCurrent)) > 0)
            {
                total += read;
                limits.CheckTotalBytes(total);
                inCurrent += read;
                if (inCurrent == current.Length)
                {
                    segments.Add(current);
                    current = ArrayPool<byte>.Shared.Rent(segmentBytes);
                    inCurrent = 0;
                }
            }

            if (segments.Count == 0)
            {
                // One segment held it all: that segment is the payload, and is not copied.
                Buffers.RentedPayload whole = Buffers.RentedPayload.Rented(current, inCurrent);
                current = null;
                return whole;
            }

            // Past one array the rented segments are the payload, unjoined, as ReadWhole hands them over (#1618).
            if (total > limits.MaxSingleBuffer)
            {
                segments.Add(current);
                Buffers.RentedPayload chained = Buffers.RentedPayload.RentedSegments(segments, inCurrent);
                segments = [];
                current = null;
                return chained;
            }

            payload = ArrayPool<byte>.Shared.Rent((int)total);
            int offset = 0;
            foreach (byte[] segment in segments)
            {
                segment.AsSpan().CopyTo(payload.AsSpan(offset));
                offset += segment.Length;
            }
            current.AsSpan(0, inCurrent).CopyTo(payload.AsSpan(offset));

            Buffers.RentedPayload assembled = Buffers.RentedPayload.Rented(payload, (int)total);
            payload = null;
            return assembled;
        }
        finally
        {
            foreach (byte[] segment in segments)
            {
                ArrayPool<byte>.Shared.Return(segment);
            }
            if (current is not null)
            {
                ArrayPool<byte>.Shared.Return(current);
            }
            if (payload is not null)
            {
                ArrayPool<byte>.Shared.Return(payload);
            }
        }
    }

    /// <summary>Reads to the end of a stream whose length is not known up front.</summary>
    private static ReadOnlyMemory<byte> ReadGrowable(Stream stream, in ArtifactLimits limits)
    {
        var buffer = new byte[CopyBufferSize];
        using var accumulated = new MemoryStream();
        int read;
        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            limits.CheckTotalBytes(accumulated.Length + read);
            accumulated.Write(buffer, 0, read);
        }
        return new ReadOnlyMemory<byte>(accumulated.GetBuffer(), 0, (int)accumulated.Length);
    }

    /// <summary>Reads <paramref name="stream"/> into segments none of which exceeds one array.</summary>
    /// <remarks>
    /// An artifact past <see cref="ArtifactLimits.MaxSingleBuffer"/> fits in no single
    /// <see cref="byte"/> array, so it is read into several and handed to the parser as one
    /// sequence — <c>Utf8JsonReader</c> reads those natively. The ceiling then belongs to the
    /// decoded data rather than to its text encoding (#377).
    /// </remarks>
    /// <param name="stream">The stream to read; never disposed here.</param>
    /// <param name="limits">Bounds applied while reading.</param>
    public static ReadOnlySequence<byte> ReadAllSegments(Stream stream, in ArtifactLimits limits)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        return ReadChain(stream, limits, SegmentChain.Large(limits)).Build();
    }

    /// <summary>The whole artifact: one segment where it fits one array, several past it, whatever the stream (#1618).</summary>
    /// <remarks>
    /// <see cref="ReadAllBytes"/> stops at one array, which a save writing a list a mebibyte at a time can pass: a seekable
    /// stream past it is read in large segments, and one of undeclared length in small ones, joined while they fit one.
    /// </remarks>
    /// <param name="stream">The stream to read; never disposed here.</param>
    /// <param name="limits">Bounds applied while reading.</param>
    public static ReadOnlySequence<byte> ReadWhole(Stream stream, in ArtifactLimits limits)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        if (stream.CanSeek && stream.Length - stream.Position > limits.MaxSingleBuffer)
        {
            return ReadChain(stream, limits, SegmentChain.Large(limits)).Build();
        }

        if (TryReadDeclaredLength(stream, limits.MaxSingleBuffer, pooled: false, out byte[] exact, out int filled))
        {
            return new ReadOnlySequence<byte>(exact, 0, filled);
        }

        return ReadGrowing(stream, limits).BuildJoined(limits.MaxSingleBuffer);
    }

    /// <summary>Asynchronous counterpart of <see cref="ReadWhole"/>.</summary>
    /// <param name="stream">The stream to read; never disposed here.</param>
    /// <param name="limits">Bounds applied while reading.</param>
    /// <param name="cancellationToken">Cancels between reads.</param>
    public static async Task<ReadOnlySequence<byte>> ReadWholeAsync(
        Stream stream,
        ArtifactLimits limits,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        if (stream.CanSeek && stream.Length - stream.Position > limits.MaxSingleBuffer)
        {
            return (await ReadChainAsync(stream, limits, SegmentChain.Large(limits), cancellationToken).ConfigureAwait(false))
                .Build();
        }

        ReadOnlyMemory<byte>? exact = await TryReadDeclaredLengthAsync(stream, limits.MaxSingleBuffer, cancellationToken).ConfigureAwait(false);
        if (exact is ReadOnlyMemory<byte> payload)
        {
            return new ReadOnlySequence<byte>(payload);
        }

        return (await ReadGrowingAsync(stream, limits, cancellationToken).ConfigureAwait(false))
            .BuildJoined(limits.MaxSingleBuffer);
    }

    /// <summary>Reads <paramref name="stream"/> to its end into <paramref name="chain"/>.</summary>
    private static SegmentChain ReadChain(Stream stream, in ArtifactLimits limits, SegmentChain chain)
    {
        while (true)
        {
            byte[] block = chain.NextBlock();
            int filled = 0;
            int read;
            while (filled < block.Length && (read = stream.Read(block, filled, block.Length - filled)) > 0)
            {
                filled += read;
            }

            if (!chain.Add(block, filled, limits))
            {
                return chain;
            }
        }
    }

    /// <summary>
    /// Reads a stream of undeclared length to its end a copy buffer at a time, as the read before it did, each read
    /// counted against <c>MaxTotalBytes</c> as it was, so a refusal names the byte count it named (#1618).
    /// </summary>
    private static SegmentChain ReadGrowing(Stream stream, in ArtifactLimits limits)
    {
        SegmentChain chain = SegmentChain.Growing(limits);
        var buffer = new byte[CopyBufferSize];
        int read;
        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            chain.Write(buffer.AsSpan(0, read), limits);
        }

        return chain;
    }

    /// <summary>Asynchronous counterpart of <see cref="ReadGrowing"/>.</summary>
    private static async Task<SegmentChain> ReadGrowingAsync(Stream stream, ArtifactLimits limits, CancellationToken cancellationToken)
    {
        SegmentChain chain = SegmentChain.Growing(limits);
        var buffer = new byte[CopyBufferSize];
        int read;
        while ((read = await ReadChunkAsync(stream, buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
        {
            chain.Write(buffer.AsSpan(0, read), limits);
        }

        return chain;
    }

    /// <summary>Asynchronous counterpart of <see cref="ReadChain"/>.</summary>
    private static async Task<SegmentChain> ReadChainAsync(
        Stream stream,
        ArtifactLimits limits,
        SegmentChain chain,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            byte[] block = chain.NextBlock();
            int filled = 0;
            int read;
            while (filled < block.Length
                && (read = await ReadChunkAsync(stream, block, filled, block.Length - filled, cancellationToken).ConfigureAwait(false)) > 0)
            {
                filled += read;
            }

            if (!chain.Add(block, filled, limits))
            {
                return chain;
            }
        }
    }

    /// <summary>The chain every segmented read builds, and the bounds each applies to it.</summary>
    /// <remarks>
    /// Shared so the reads differ only in their read call and their segment sizes. Duplicating
    /// the accumulate-and-check would be how the synchronous and asynchronous paths drift apart
    /// again, which is the defect #396 exists to close rather than to repeat.
    /// </remarks>
    private sealed class SegmentChain
    {
        private readonly int largest;
        private int next;
        private long written;
        private byte[]? open;
        private int inOpen;
        private Segment? head;
        private Segment? tail;
        private long total;

        /// <summary>Segments of <paramref name="first"/> bytes, doubling up to <paramref name="largest"/>.</summary>
        /// <param name="first">The first segment's length.</param>
        /// <param name="largest">The length segments double up to.</param>
        private SegmentChain(int first, int largest)
        {
            next = first;
            this.largest = largest;
        }

        /// <summary>For a stream known to pass one array: well under the ceiling, and a handful of them rather than thousands.</summary>
        public static SegmentChain Large(in ArtifactLimits limits)
        {
            int size = (int)Math.Min(limits.MaxSingleBuffer, 64L * 1024 * 1024);
            return new SegmentChain(size, size);
        }

        /// <summary>
        /// For a stream of undeclared length, most of which are small: 256 bytes, where the stream the read took
        /// before started, doubling to a mebibyte, so a tiny artifact costs no more than it did (#1618, #1624).
        /// </summary>
        public static SegmentChain Growing(in ArtifactLimits limits) => new(
            (int)Math.Min(limits.MaxSingleBuffer, FirstGrowingSegment),
            (int)Math.Min(limits.MaxSingleBuffer, GrowableSegmentBytes));

        /// <summary>The first segment a growing read takes, as <see cref="MemoryStream"/>'s first buffer was.</summary>
        private const int FirstGrowingSegment = 256;

        /// <summary>Copies one read into the chain, counting it against <c>MaxTotalBytes</c> first.</summary>
        public void Write(ReadOnlySpan<byte> data, in ArtifactLimits limits)
        {
            written += data.Length;
            limits.CheckTotalBytes(written);
            while (!data.IsEmpty)
            {
                if (open is null || inOpen == open.Length)
                {
                    Seal();
                    open = NextBlock();
                }

                int taken = Math.Min(data.Length, open.Length - inOpen);
                data.Slice(0, taken).CopyTo(open.AsSpan(inOpen));
                inOpen += taken;
                data = data.Slice(taken);
            }
        }

        public byte[] NextBlock()
        {
            var block = new byte[next];
            next = Math.Min(next * 2, largest);
            return block;
        }

        /// <summary>Links the block <see cref="Write"/> fills, if it holds anything.</summary>
        private void Seal()
        {
            if (open is not null && inOpen > 0)
            {
                Link(open, inOpen);
            }

            open = null;
            inOpen = 0;
        }

        private void Link(byte[] block, int filled)
        {
            total += filled;
            if (tail is null)
            {
                head = new Segment(block, filled, 0);
                tail = head;
            }
            else
            {
                tail = tail.Append(block, filled);
            }
        }

        /// <summary>Appends what was read, and answers whether the stream may hold more.</summary>
        public bool Add(byte[] block, int filled, in ArtifactLimits limits)
        {
            if (filled == 0)
            {
                return false;
            }

            Link(block, filled);
            limits.CheckTotalBytes(total);
            return filled == block.Length;
        }

        public ReadOnlySequence<byte> Build()
        {
            Seal();
            return head is null
                ? ReadOnlySequence<byte>.Empty
                : new ReadOnlySequence<byte>(head, 0, tail!, tail!.Memory.Length);
        }

        /// <summary>The chain copied into one array where it fits one, which the reader parses fastest; else <see cref="Build"/>.</summary>
        public ReadOnlySequence<byte> BuildJoined(long maxSingleBuffer)
        {
            Seal();
            if (head is null || ReferenceEquals(head, tail) || total > maxSingleBuffer)
            {
                return Build();
            }

            byte[] joined = Buffers.AllocateUninitialized<byte>((int)total);
            int offset = 0;
            for (Segment? segment = head; segment is not null; segment = (Segment?)segment.Next)
            {
                segment.Memory.Span.CopyTo(joined.AsSpan(offset));
                offset += segment.Memory.Length;
            }

            return new ReadOnlySequence<byte>(joined);
        }
    }

    /// <summary>One link of the read's chain, which is all <see cref="ReadOnlySequence{T}"/> asks for.</summary>
    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public Segment(byte[] block, int filled, long runningIndex)
        {
            Memory = new ReadOnlyMemory<byte>(block, 0, filled);
            RunningIndex = runningIndex;
        }

        public Segment Append(byte[] block, int filled)
        {
            var next = new Segment(block, filled, RunningIndex + Memory.Length);
            Next = next;
            return next;
        }
    }

    /// <summary>Asynchronous counterpart of <see cref="ReadAllBytes"/>.</summary>
    public static async Task<ReadOnlyMemory<byte>> ReadAllBytesAsync(
        Stream stream,
        ArtifactLimits limits,
        CancellationToken cancellationToken)
    {
        Guard.NotNull(stream);
        CheckDeclaredLength(stream, limits);

        ReadOnlyMemory<byte>? exact = await TryReadDeclaredLengthAsync(stream, limits.MaxSingleBuffer, cancellationToken).ConfigureAwait(false);
        if (exact is ReadOnlyMemory<byte> payload)
        {
            return payload;
        }

        var buffer = new byte[CopyBufferSize];
        using var accumulated = new MemoryStream();
        int read;
        while ((read = await ReadChunkAsync(stream, buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
        {
            limits.CheckTotalBytes(accumulated.Length + read);

            // CA1849 / SonarLint S6966: the async call in this loop is the ReadAsync above, on
            // the caller's stream, which may be a file or a socket. The destination is
            // a MemoryStream: its WriteAsync performs no I/O, copies into the same
            // buffer and returns an already-completed task, so awaiting it would add a
            // state machine and allocations without ever yielding.
#pragma warning disable S6966, CA1849
            accumulated.Write(buffer, 0, read);
#pragma warning restore S6966, CA1849
        }
        return new ReadOnlyMemory<byte>(accumulated.GetBuffer(), 0, (int)accumulated.Length);
    }

    /// <summary>
    /// Fills one exactly-sized buffer from a stream that knows its own length —
    /// every <c>Load</c> that starts from a path, and every test that starts from a
    /// <see cref="MemoryStream"/>.
    /// </summary>
    /// <remarks>
    /// Returns <c>false</c> when there is no length to size from, that length exceeds what a single array
    /// can hold, or the stream holds more than declared — the position is then put back so the caller's
    /// growable path reads the whole thing rather than the prefix this would have truncated it to.
    /// </remarks>
    private static bool TryReadDeclaredLength(
        Stream stream, long maxSingleBuffer, bool pooled, out byte[] buffer, out int filled)
    {
        buffer = [];
        filled = 0;
        if (!stream.CanSeek)
        {
            return false;
        }

        long origin = stream.Position;
        long declared = stream.Length - origin;
        if (declared < 0 || declared > maxSingleBuffer)
        {
            return false;
        }

        // Every bound below is `length`, never buffer.Length: a rented array is at least as
        // long as asked, so the two are the same number only on the allocating path.
        int length = (int)declared;

        // Uninitialized or rented: the loop fills it, and the caller is handed only the
        // prefix `filled` covers, so nothing past what the stream gave is ever read.
        buffer = pooled
            ? ArrayPool<byte>.Shared.Rent(length)
            : Buffers.AllocateUninitialized<byte>(length);
        int read;
        while (filled < length && (read = stream.Read(buffer, filled, length - filled)) > 0)
        {
            filled += read;
        }

        if (filled == length && stream.ReadByte() >= 0)
        {
            stream.Position = origin;
            if (pooled)
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            buffer = [];
            filled = 0;
            return false;
        }
        return true;
    }

    /// <summary>Asynchronous counterpart of <see cref="TryReadDeclaredLength"/>; <c>null</c> where that one returns <c>false</c>.</summary>
    private static async Task<ReadOnlyMemory<byte>?> TryReadDeclaredLengthAsync(
        Stream stream,
        long maxSingleBuffer,
        CancellationToken cancellationToken)
    {
        if (!stream.CanSeek)
        {
            return null;
        }

        long origin = stream.Position;
        long declared = stream.Length - origin;
        if (declared < 0 || declared > maxSingleBuffer)
        {
            return null;
        }

        var buffer = new byte[declared];
        int filled = 0;
        int read;
        while (filled < buffer.Length
            && (read = await ReadChunkAsync(stream, buffer, filled, buffer.Length - filled, cancellationToken).ConfigureAwait(false)) > 0)
        {
            filled += read;
        }

        if (filled == buffer.Length)
        {
            var probe = new byte[1];
            if (await ReadChunkAsync(stream, probe, 0, 1, cancellationToken).ConfigureAwait(false) > 0)
            {
                stream.Position = origin;
                return null;
            }
        }
        return new ReadOnlyMemory<byte>(buffer, 0, filled);
    }

    /// <summary>Reads one chunk, using the allocation-free overload where it exists.</summary>
    /// <remarks>
    /// <c>Stream.ReadAsync(Memory&lt;byte&gt;, CancellationToken)</c> arrived with
    /// netstandard2.1, so the older target keeps the array overload. Wrapping the
    /// difference here keeps the read loops themselves free of conditional
    /// compilation.
    /// </remarks>
    private static ValueTask<int> ReadChunkAsync(
        Stream stream,
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken) =>
#if NETSTANDARD2_0
        new(stream.ReadAsync(buffer, offset, count, cancellationToken));
#else
        stream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
#endif

    /// <summary>Opens <paramref name="path"/> for writing an artifact; the caller owns the returned stream.</summary>
    public static FileStream OpenWrite(string path)
    {
        Guard.NotNull(path);
        return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, CopyBufferSize, useAsync: false);
    }

    /// <summary>Opens <paramref name="path"/> for reading an artifact; the caller owns the returned stream.</summary>
    public static FileStream OpenRead(string path, bool useAsync = false)
    {
        Guard.NotNull(path);
        return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, CopyBufferSize, useAsync);
    }

    /// <summary>Advances onto the value of the current property and returns it as a <see cref="bool"/>.</summary>
    public static bool ReadBoolean(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || (reader.TokenType != JsonTokenType.True && reader.TokenType != JsonTokenType.False))
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
        return reader.GetBoolean();
    }

    /// <summary>Advances onto the value of the current property and returns it as an <see cref="int"/>.</summary>
    public static int ReadInt32(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int value))
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
        return value;
    }

    /// <summary>Advances onto the value of the current property and returns it as a <see cref="double"/>.</summary>
    public static double ReadDouble(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number || !reader.TryGetDouble(out double value))
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
        return value;
    }

    /// <summary>Advances onto the value of the current property and returns it as a non-null string.</summary>
    public static string ReadString(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
        return GetText(ref reader);
    }

    /// <summary>Writes <paramref name="value"/> as a JSON string that reads back unchanged, a lone surrogate included.</summary>
    /// <remarks>
    /// <see cref="Utf8JsonWriter"/> writes an unpaired surrogate as U+FFFD, so a vocabulary holding
    /// one saved two equal keys, or a term its counts no longer reached. Here it becomes an escape,
    /// which Python's <c>json</c> reads back too, and <see cref="GetText"/> reads it; the text around
    /// it goes through the writer's own encoder, so it is escaped as it always was. A string with no
    /// lone surrogate is written exactly as before.
    /// </remarks>
    public static void WriteText(Utf8JsonWriter writer, string value)
    {
        // A long string goes to a flushed writer, as EnsureWritableText's trial wrote it (#1618).
        if (value.Length > AlwaysWritableCharacters)
        {
            writer.Flush();
        }

        if (!HasLoneSurrogate(value))
        {
            writer.WriteStringValue(value);
            return;
        }

        // Built from the encoder's output and four-digit escapes, so already valid JSON.
        writer.WriteRawValue(EscapeWithLoneSurrogates(value), skipInputValidation: true);
    }

    /// <summary>Writes the property <paramref name="propertyName"/> with <paramref name="value"/>, as <see cref="WriteText(Utf8JsonWriter, string)"/> writes it.</summary>
    public static void WriteText(Utf8JsonWriter writer, string propertyName, string value)
    {
        writer.WritePropertyName(propertyName);
        WriteText(writer, value);
    }

    /// <summary>
    /// The longest string written without a trial: at most six ASCII bytes a unit once escaped, 60 MB, far inside
    /// every ceiling the writer has. System.Text.Json 10 refuses past 166,666,666 characters, a raw value past
    /// 715,827,882, and failed on 120,000,000 characters each escaped, so a longer string is tried first (#1618).
    /// </summary>
    public const int AlwaysWritableCharacters = 10_000_000;

    /// <summary>
    /// Pending bytes past which <see cref="FlushIfPending"/> hands them to the stream. <see cref="Utf8JsonWriter"/> holds
    /// everything until told to flush, and refuses to hold much past two gibibytes (#1618).
    /// </summary>
    private const int FlushThreshold = 1 << 20;

    /// <summary>Flushes the writer once a mebibyte is pending, so a list of any length is held in memory a mebibyte at a time.</summary>
    public static void FlushIfPending(Utf8JsonWriter writer)
    {
        if (writer.BytesPending >= FlushThreshold)
        {
            writer.Flush();
        }
    }

    /// <summary>Refuses, before a save's first byte, a string <see cref="WriteText(Utf8JsonWriter, string)"/> cannot write.</summary>
    /// <param name="value">The string a save will write.</param>
    /// <param name="what">What it is, for the message: <c>"A vocabulary term"</c>, <c>"An id"</c>.</param>
    /// <exception cref="InvalidOperationException">The writer cannot write the value, whatever it raises but cancellation.</exception>
    public static void EnsureWritableText(string? value, string what)
    {
        // A null is left to the write, which main reached it at; a short string cannot fail.
        if (value is null || value.Length <= AlwaysWritableCharacters)
        {
            return;
        }

        // Written for real into nothing: what the writer refuses depends on the escaped UTF-8, not on the characters.
        try
        {
            using var trial = new Utf8JsonWriter(Stream.Null, WriterOptions);
            // In an array, as a list writes it; WriteText flushes first, as it does in the save.
            trial.WriteStartArray();
            WriteText(trial, value);
            trial.Flush();
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            // Whatever the writer throws, the save would throw it too, so nothing it raises is left undocumented:
            // System.Text.Json 10 raised three types here, and its other builds are not measured (#1625).
            throw new InvalidOperationException(
                $"{what} of {value.Length} characters could not be written by the JSON writer ({e.GetType().Name}); nothing was written.", e);
        }
    }

    /// <summary><paramref name="value"/> as a quoted JSON string, its lone surrogates as four-digit escapes.</summary>
    private static string EscapeWithLoneSurrogates(string value)
    {
        var sb = new StringBuilder(value.Length + 16).Append('"');
        int start = 0;
        int i = 0;
        while (i < value.Length)
        {
            if (char.IsSurrogatePair(value, i))
            {
                i += 2;
                continue;
            }
            if (char.IsSurrogate(value[i]))
            {
                sb.Append(RelaxedEncoder.Encode(value.Substring(start, i - start)))
                  .Append('\\').Append('u').Append(((int)value[i]).ToString("X4", CultureInfo.InvariantCulture));
                start = i + 1;
            }
            i++;
        }
        return sb.Append(RelaxedEncoder.Encode(value.Substring(start))).Append('"').ToString();
    }

    /// <summary>The current string token's text, an escaped lone surrogate included.</summary>
    /// <remarks>
    /// <see cref="Utf8JsonReader.GetString"/> refuses an unpaired surrogate escape, which
    /// <see cref="WriteText(Utf8JsonWriter, string)"/> and Python's <c>json</c> both write. Only such a
    /// token is unescaped here; every other one, an escaped pair included, still goes through the
    /// reader, and invalid UTF-8 raises the reader's own <see cref="InvalidOperationException"/>.
    /// </remarks>
    public static string GetText(ref Utf8JsonReader reader)
    {
        if (!reader.ValueIsEscaped)
        {
            return reader.GetString()!;
        }

        if (reader.HasValueSequence)
        {
            byte[] joined = reader.ValueSequence.ToArray();
            return HoldsLoneSurrogateEscape(joined) ? Unescape(joined) : reader.GetString()!;
        }
        return HoldsLoneSurrogateEscape(reader.ValueSpan) ? Unescape(reader.ValueSpan.ToArray()) : reader.GetString()!;
    }

    /// <summary>The encoder <see cref="WriterOptions"/> names, for text written around a lone surrogate.</summary>
    private static JavaScriptEncoder RelaxedEncoder => JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

    private static bool HasLoneSurrogate(string s)
    {
        // Every term is checked on Save and almost none holds a surrogate, so the common case is one
        // unsigned comparison per unit; a vectorised search costs more than that on a short term.
        int i = 0;
        while (i < s.Length && (uint)(s[i] - 0xD800) > 0x7FF)
        {
            i++;
        }
        while (i < s.Length)
        {
            if (char.IsSurrogatePair(s, i))
            {
                i += 2;
                continue;
            }
            if (char.IsSurrogate(s[i]))
            {
                return true;
            }
            i++;
        }
        return false;
    }

    // Whether the raw token holds a surrogate escape that is not half of an escaped pair, the one
    // escape GetString refuses. A surrogate cannot be raw UTF-8, so both halves of a pair are escapes.
    private static bool HoldsLoneSurrogateEscape(ReadOnlySpan<byte> raw)
    {
        int i = 0;
        while (i < raw.Length)
        {
            if (raw[i] != '\\')
            {
                i++;
                continue;
            }
            if (raw[i + 1] != 'u')
            {
                i += 2;
                continue;
            }

            int unit = EscapedUnit(raw, i);
            if (unit is >= 0xD800 and <= 0xDBFF && i + 11 < raw.Length && raw[i + 6] == '\\' && raw[i + 7] == 'u'
                && EscapedUnit(raw, i + 6) is >= 0xDC00 and <= 0xDFFF)
            {
                i += 12;
                continue;
            }
            if (unit is >= 0xD800 and <= 0xDFFF)
            {
                return true;
            }
            i += 6;
        }
        return false;
    }

    // JSON's escapes, over a token the reader has already validated: each \uXXXX becomes one UTF-16
    // unit, so a pair written as two escapes is joined again and a lone one stays alone.
    private static string Unescape(byte[] raw)
    {
        var sb = new StringBuilder(raw.Length);
        int i = 0;
        while (i < raw.Length)
        {
            int next = Array.IndexOf(raw, (byte)'\\', i);
            int end = next < 0 ? raw.Length : next;
            sb.Append(DecodeUtf8(raw, i, end - i));
            if (next < 0)
            {
                break;
            }

            byte kind = raw[next + 1];
            if (kind == 'u')
            {
                sb.Append((char)EscapedUnit(raw, next));
                i = next + 6;
                continue;
            }
            sb.Append(kind switch
            {
                (byte)'b' => '\b',
                (byte)'f' => '\f',
                (byte)'n' => '\n',
                (byte)'r' => '\r',
                (byte)'t' => '\t',
                _ => (char)kind,
            });
            i = next + 2;
        }
        return sb.ToString();
    }

    // The reader does not check UTF-8 inside a string until GetString, which throws this type.
    private static string DecodeUtf8(byte[] raw, int index, int count)
    {
        try
        {
            return Utf8NoBom.GetString(raw, index, count);
        }
        catch (DecoderFallbackException e)
        {
            throw new InvalidOperationException("Cannot read invalid UTF-8 JSON text as string.", e);
        }
    }

    // The unit a \uXXXX escape starting at at spells.
    private static int EscapedUnit(ReadOnlySpan<byte> raw, int at) =>
        (HexValue(raw[at + 2]) << 12) | (HexValue(raw[at + 3]) << 8) | (HexValue(raw[at + 4]) << 4) | HexValue(raw[at + 5]);

    private static int HexValue(byte b) => b switch
    {
        >= (byte)'0' and <= (byte)'9' => b - '0',
        >= (byte)'a' and <= (byte)'f' => b - 'a' + 10,
        >= (byte)'A' and <= (byte)'F' => b - 'A' + 10,
        _ => -1,
    };

    /// <summary>Advances onto the value of the current property, allowing an explicit JSON <c>null</c>.</summary>
    public static string? ReadNullableString(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read())
        {
            throw Truncated(artifact);
        }
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => GetText(ref reader),
            _ => throw UnexpectedToken(artifact, propertyName, reader.TokenType),
        };
    }

    /// <summary>Positions the reader on the opening brace of the current property's object value.</summary>
    public static void ReadStartObject(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
    }

    /// <summary>Positions the reader on the opening bracket of the current property's array value.</summary>
    public static void ReadStartArray(ref Utf8JsonReader reader, string artifact, string propertyName)
    {
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
        {
            throw UnexpectedToken(artifact, propertyName, reader.TokenType);
        }
    }

    /// <summary>The exception raised when an artifact carries a property the schema does not define.</summary>
    public static InvalidDataException UnknownProperty(string artifact, string propertyName) =>
        new($"Unknown property '{propertyName}' in a '{artifact}' artifact. Unknown properties are rejected so a newer file is not silently misread.");

    /// <summary>The exception raised when a required property is absent.</summary>
    public static InvalidDataException MissingProperty(string artifact, string propertyName) =>
        new($"A '{artifact}' artifact is missing the required property '{propertyName}'.");

    /// <summary>The exception raised when a property holds the wrong JSON type.</summary>
    public static InvalidDataException UnexpectedToken(string artifact, string propertyName, JsonTokenType actual) =>
        new($"Property '{propertyName}' of a '{artifact}' artifact has unexpected JSON token type {actual}.");

    /// <summary>The exception raised when the bytes end mid-artifact.</summary>
    public static InvalidDataException Truncated(string artifact) =>
        new($"A '{artifact}' artifact ended unexpectedly: the input is truncated or malformed.");

    /// <summary>The exception raised when two persisted collections disagree on length.</summary>
    public static InvalidDataException Inconsistent(string artifact, string detail) =>
        new($"A '{artifact}' artifact is internally inconsistent: {detail}");

    private const int CopyBufferSize = 81920;

    private static void CheckDeclaredLength(Stream stream, in ArtifactLimits limits)
    {
        // Fail before allocating anything when the stream already knows it is too big.
        if (stream.CanSeek)
        {
            limits.CheckTotalBytes(stream.Length - stream.Position);
        }
    }
}
