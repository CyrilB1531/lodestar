using System.Text;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Search;
using Lodestar.Internal.Persistence;
using Xunit;

namespace Lodestar.Embeddings.Tests.Persistence;

/// <summary>
/// The stream shapes the read path has to survive beyond an ordinary file: a
/// source that will not say how long it is, one that says it wrong, and one that
/// hands over a few bytes at a time. Each is a branch of
/// <c>JsonArtifact.ReadAllBytes</c> that no other suite reaches.
/// </summary>
public sealed class EmbeddingIndexReadPathTests
{
    [Fact]
    public void A_non_seekable_source_loads_the_same_index()
    {
        byte[] artifact = Artifact();

        using var pipe = new UnseekableStream(artifact);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(pipe));
    }

    [Fact]
    public void A_non_seekable_source_spanning_several_read_segments_loads_the_same_index()
    {
        // Past 1 MiB the undeclared-length read rents more than one segment and assembles
        // them; the tiny artifact above never leaves the first.
        byte[] artifact = Artifact(count: 2_000, dimension: 384);
        Assert.True(artifact.Length > 3 << 20, $"The artifact is {artifact.Length} bytes, not past three segments.");

        using var pipe = new UnseekableStream(artifact, chunk: 65_521);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(pipe));
    }

    [Fact]
    public void A_non_seekable_source_ending_on_a_segment_boundary_loads_the_same_index()
    {
        // Padded with trailing whitespace to exactly two segments, so the read rotates to a
        // fresh segment that then stays empty.
        byte[] artifact = Artifact(count: 800, dimension: 384);
        byte[] padded = new byte[2 << 20];
        artifact.CopyTo(padded, 0);
        padded.AsSpan(artifact.Length).Fill((byte)' ');

        using var pipe = new UnseekableStream(padded);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(pipe));
    }

    [Fact]
    public void A_non_seekable_source_past_the_byte_limit_is_refused_mid_read()
    {
        byte[] artifact = Artifact(count: 2_000, dimension: 384);

        using var pipe = new UnseekableStream(artifact);
        InvalidDataException error = Assert.Throws<InvalidDataException>(
            () => EmbeddingIndex.Load(pipe, new ArtifactLoadOptions { MaxTotalBytes = 2 << 20 }));

        Assert.Contains("MaxTotalBytes", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_short_non_seekable_load_after_a_long_one_carries_no_trailing_bytes()
    {
        // The segments and the assembled buffer are rented, so the long load leaves its bytes in
        // arrays the short one is then handed; only the byte count may bound what is parsed.
        byte[] longer = Artifact(count: 2_000, dimension: 384);
        byte[] shorter = Artifact();

        using (var first = new UnseekableStream(longer))
        {
            GC.KeepAlive(EmbeddingIndex.Load(first));
        }

        using var second = new UnseekableStream(shorter);
        AssertSameIndex(Reference(shorter), EmbeddingIndex.Load(second));
    }

    [Fact]
    public async Task A_non_seekable_source_loads_the_same_index_asynchronously()
    {
        byte[] artifact = Artifact();

        using var pipe = new UnseekableStream(artifact);
        AssertSameIndex(Reference(artifact), await EmbeddingIndex.LoadAsync(pipe, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public void A_non_seekable_source_past_one_array_loads_from_its_segments()
    {
        // The pooled read handed past one array refused with an IOException; its rented segments are now the payload,
        // the last of them empty when the stream ends on a boundary (#1618).
        byte[] artifact = Artifact();
        byte[] padded = new byte[(artifact.Length + 63) / 64 * 64];
        artifact.CopyTo(padded, 0);
        padded.AsSpan(artifact.Length).Fill((byte)' ');

        using var pipe = new UnseekableStream(artifact, chunk: 7);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(pipe, SixtyFourByteArray));
        using var boundary = new UnseekableStream(padded);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(boundary, SixtyFourByteArray));
    }

    [Fact]
    public async Task A_non_seekable_source_past_one_array_loads_asynchronously()
    {
        // The asynchronous read gathered it in a MemoryStream, which refuses past one array (#1618).
        byte[] artifact = Artifact();

        using var pipe = new UnseekableStream(artifact, chunk: 7);
        AssertSameIndex(
            Reference(artifact),
            await EmbeddingIndex.LoadAsync(pipe, SixtyFourByteArray, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task A_read_only_stream_is_refused_by_the_writer_first_asynchronously()
    {
        // The head goes through a writer on the stream, which refuses a read-only one ahead of a non-finite vector, as
        // 0.8.0 did, synchronously and through the task (#1618, #1633).
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([float.NaN], "a");
        await using var readOnly = new MemoryStream(new byte[16], writable: false);
        Assert.Equal(typeof(ArgumentException), SyncRefusal(index, readOnly).GetType());
        Assert.Equal(
            typeof(ArgumentException),
            (await Assert.ThrowsAnyAsync<ArgumentException>(() => index.SaveAsync(readOnly, TestContext.Current.CancellationToken))).GetType());
    }

    [Fact]
    public async Task A_stream_whose_flush_fails_faults_the_task_rather_than_the_call()
    {
        // The head's writer flushes asynchronously and is pointed away before it is disposed, so the failing flush is the
        // task's, never a synchronous one in the call.
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([1f], "a");
        var failing = new FailingFlushStream();
        try
        {
            Task save = index.SaveAsync(failing, TestContext.Current.CancellationToken);
            await Assert.ThrowsAsync<IOException>(() => save);
        }
        finally
        {
            await failing.DisposeAsync();
        }
    }

    /// <summary>A writable stream whose flush always fails.</summary>
    private sealed class FailingFlushStream : MemoryStream
    {
        public override void Flush() => throw new IOException("flush failed");

        public override Task FlushAsync(CancellationToken cancellationToken) => Task.FromException(new IOException("flush failed"));
    }

    [Fact]
    public async Task A_non_finite_index_is_refused_alike_whatever_the_stream_does_with_writes()
    {
        // The refusal comes before the first byte, and the writer is pointed away, so a stream whose writes or flushes fail
        // sees nothing, synchronously or not (#1641).
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([float.NaN], "a");
        var failing = new FailingStream();
        try
        {
            Assert.IsType<InvalidDataException>(SyncRefusal(index, failing));
            Assert.IsType<InvalidDataException>(
                await Assert.ThrowsAnyAsync<Exception>(() => index.SaveAsync(failing, TestContext.Current.CancellationToken)));
            Assert.Equal(0, failing.Calls);
        }
        finally
        {
            await failing.DisposeAsync();
        }
    }

    [Fact]
    public void A_loaded_id_holding_a_lone_surrogate_is_saved_again_as_it_was_read()
    {
        // The load notes the escaped id as it reads it, so the next save searches it rather than writing it straight (#1643).
        string lone = "a" + (char)0xD800 + "b";
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([1f], lone);
        byte[] saved = SaveBytes(index);

        using var source = new MemoryStream(saved);
        EmbeddingIndex loaded = EmbeddingIndex.Load(source);
        Assert.Equal(saved, SaveBytes(loaded));
    }

    [Fact]
    public void A_block_id_holding_a_lone_surrogate_is_saved_and_read_back()
    {
        // The factories search the ids they copy, so the save escapes the lone surrogate rather than replacing it (#1643).
        string lone = "a" + (char)0xD800 + "b";
        EmbeddingIndex index = EmbeddingIndex.FromBlock([1f], dimension: 1, BlockNormalization.Off, [lone]);

        using var source = new MemoryStream(SaveBytes(index));
        Assert.Equal(lone, EmbeddingIndex.Load(source).GetId(0));
    }

    [Fact]
    public async Task A_long_id_is_written_without_a_synchronous_call_on_the_stream()
    {
        // An id past ten million characters is tried and written from a flushed writer, which an asynchronous save now
        // flushes by awaiting, so a stream refusing synchronous calls takes it (#1640).
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([1f], new string('a', 10_000_001));
        var asyncOnly = new AsyncOnlyStream();
        try
        {
            await index.SaveAsync(asyncOnly, TestContext.Current.CancellationToken);
            Assert.True(asyncOnly.Length > 10_000_000);
        }
        finally
        {
            await asyncOnly.DisposeAsync();
        }
    }

    /// <summary>A stream whose every write and flush fails, counting how often it was asked.</summary>
    private sealed class FailingStream : MemoryStream
    {
        public int Calls { get; private set; }

        public override void Write(byte[] buffer, int offset, int count) => throw Failed();

        public override void Write(ReadOnlySpan<byte> buffer) => throw Failed();

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => Task.FromException(Failed());

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => ValueTask.FromException(Failed());

        public override void Flush() => throw Failed();

        public override Task FlushAsync(CancellationToken cancellationToken) => Task.FromException(Failed());

        private IOException Failed()
        {
            Calls++;
            return new IOException("disk full");
        }
    }

    /// <summary>A stream that refuses synchronous writes and flushes, as an ASP.NET Core response body does by default.</summary>
    private sealed class AsyncOnlyStream : Stream
    {
        private readonly MemoryStream _inner = new();

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => _inner.Length;

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) => throw Synchronous();

        public override void Write(ReadOnlySpan<byte> buffer) => throw Synchronous();

        public override void Flush() => throw Synchronous();

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _inner.Write(buffer, offset, count);
            return Task.CompletedTask;
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            _inner.Write(buffer.Span);
            return default;
        }

        public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }

        private static InvalidOperationException Synchronous() => new("Synchronous operations are disallowed.");
    }

    [Fact]
    public async Task A_long_id_is_refused_as_the_writer_refuses_it_before_a_byte_is_written()
    {
        // System.Text.Json 10 writes 166,666,666 characters and refuses one more, with the exception 0.8.0 raised; sync
        // and through the task (#1626, #1646).
        var index = new EmbeddingIndex(dimension: 1);
        index.Add([1f], new string('a', 166_666_667));
        var stream = new MemoryStream();
        try
        {
            Assert.Equal("The JSON value of length 166666667 is too large and not supported.", Assert.Throws<ArgumentException>(() => index.Save(stream)).Message);
            Task save = index.SaveAsync(stream, TestContext.Current.CancellationToken);
            Assert.Equal("The JSON value of length 166666667 is too large and not supported.", (await Assert.ThrowsAsync<ArgumentException>(() => save)).Message);
            Assert.Equal(0, stream.Length);
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }

    [Fact]
    public async Task An_asynchronous_save_writes_a_long_head_as_it_goes()
    {
        // 160,000 ids, a head of about 2.6 MB, flushed through the writer on the stream a mebibyte at a time; composed
        // whole in memory before being copied out, it cost 5,318,944 bytes; the bytes are the synchronous save's (#1635).
        var index = new EmbeddingIndex(dimension: 1);
        for (int item = 0; item < 160_000; item++)
        {
            index.Add([1f], $"item-{item:D8}");
        }

        byte[] expected = SaveBytes(index);
        var buffered = new MemoryStream();
        try
        {
            await index.SaveAsync(buffered, TestContext.Current.CancellationToken);
            Assert.Equal(expected, buffered.ToArray());
        }
        finally
        {
            await buffered.DisposeAsync();
        }

        // Into a stream that keeps nothing, so what is counted is the save's own.
        await index.SaveAsync(Stream.Null, TestContext.Current.CancellationToken);
        long before = GC.GetAllocatedBytesForCurrentThread();
        await index.SaveAsync(Stream.Null, TestContext.Current.CancellationToken);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(allocated < HeadBound, $"{allocated} bytes allocated for {expected.Length}.");
    }

    /// <summary>What the long-head save may allocate: 2,172,784 measured, where composing the head whole took 5,318,944 (#1635).</summary>
    private const long HeadBound = 3_000_000;

    private static Exception SyncRefusal(EmbeddingIndex index, Stream stream) =>
        Assert.ThrowsAny<Exception>(() => index.Save(stream));

    private static byte[] SaveBytes(EmbeddingIndex index)
    {
        using var stream = new MemoryStream();
        index.Save(stream);
        return stream.ToArray();
    }

    [Fact]
    public void Ids_reach_the_stream_a_mebibyte_at_a_time()
    {
        // 160,000 ids, about 2.6 MB, were held whole until the block; flushed, no write passes a mebibyte and an id (#1618).
        var index = new EmbeddingIndex(dimension: 1);
        for (int item = 0; item < 160_000; item++)
        {
            index.Add([1f], $"item-{item:D8}");
        }

        using var recording = new RecordingStream();
        index.Save(recording);
        Assert.True(recording.Length > 5 << 19, $"The artifact is {recording.Length} bytes.");
        Assert.True(recording.Largest < (1 << 20) + 64, $"One write carried {recording.Largest} bytes.");
    }

    [Fact]
    public void A_source_that_under_declares_its_length_loads_in_full()
    {
        // The fast path sizes its buffer from Length. A stream that reports less
        // than it holds must not be silently truncated to what it declared.
        byte[] artifact = Artifact();

        using var liar = new ShortLengthStream(artifact, declared: artifact.Length - 16);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(liar));
    }

    [Fact]
    public async Task A_source_that_under_declares_its_length_loads_in_full_asynchronously()
    {
        byte[] artifact = Artifact();

        using var liar = new ShortLengthStream(artifact, declared: artifact.Length - 16);
        AssertSameIndex(Reference(artifact), await EmbeddingIndex.LoadAsync(liar, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public void A_source_that_answers_in_small_reads_still_fills_the_buffer()
    {
        // Read is allowed to return fewer bytes than asked for; a MemoryStream
        // never does, so nothing else in the suite exercises the fill loop.
        byte[] artifact = Artifact();

        using var trickle = new TrickleStream(artifact, chunk: 7);
        AssertSameIndex(Reference(artifact), EmbeddingIndex.Load(trickle));
    }

    [Fact]
    public void An_escaped_base64_token_loads_the_same_vectors()
    {
        // One base64 character written as its \u00XX escape makes ValueSpan differ from the decoded
        // value -- the only reachable trigger for the fallback decode, since a JSON string can't hold a raw newline.
        string json = Encoding.UTF8.GetString(Artifact());
        int start = json.IndexOf("\"vectors\":\"", StringComparison.Ordinal) + "\"vectors\":\"".Length;
        string escaped = string.Concat(
            json.AsSpan(0, start),
            $"\\u{(int)json[start]:x4}".AsSpan(),
            json.AsSpan(start + 1));

        Assert.NotEqual(json, escaped);
        AssertSameIndex(
            Reference(Encoding.UTF8.GetBytes(json)),
            EmbeddingIndex.Load(new MemoryStream(Encoding.UTF8.GetBytes(escaped))));
    }

    /// <summary>The limits a default load applies, its one-array ceiling brought down to 64 bytes.</summary>
    private static readonly ArtifactLimits SixtyFourByteArray = new(
        ArtifactLimits.DefaultMaxVocabularySize,
        ArtifactLimits.DefaultMaxTokenLength,
        ArtifactLimits.DefaultMaxJsonDepth,
        ArtifactLimits.DefaultMaxTotalBytes,
        ArtifactLimits.DefaultMaxArrayLength,
        maxSingleBuffer: 64);

    /// <summary>A memory stream that keeps the largest single write it was handed.</summary>
    private sealed class RecordingStream : MemoryStream
    {
        public int Largest { get; private set; }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Largest = Math.Max(Largest, count);
            base.Write(buffer, offset, count);
        }

#if !NETFRAMEWORK
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            Largest = Math.Max(Largest, buffer.Length);
            base.Write(buffer);
        }
#endif
    }

    /// <summary>
    /// The index every case is compared against: the same bytes, read through an
    /// ordinary seekable stream. Kept out of the async tests as its own method
    /// because a synchronous <c>Load</c> inside an <c>async</c> body is a rule
    /// violation there, and awaiting a second path would compare two unknowns.
    /// </summary>
    private static EmbeddingIndex Reference(byte[] artifact) => EmbeddingIndex.Load(new MemoryStream(artifact));

    /// <summary>Two vectors of three dimensions, with ids — the ordinary artifact.</summary>
    private static byte[] Artifact()
    {
        var index = new EmbeddingIndex(dimension: 3);
        index.Add([1f, 0f, 0f], "a");
        index.Add([0.6f, 0.8f, 0f], "b");
        using var stream = new MemoryStream();
        index.Save(stream);
        return stream.ToArray();
    }

    /// <summary>A deterministic index large enough to need several read segments.</summary>
    private static byte[] Artifact(int count, int dimension)
    {
        var index = new EmbeddingIndex(dimension);
        var vector = new float[dimension];
        for (int item = 0; item < count; item++)
        {
            for (int i = 0; i < dimension; i++)
            {
                // Deterministic and never all-zero; the values matter only in being reproducible.
                vector[i] = ((item * 31 + i * 17) % 97) - 48.5f;
            }
            index.Add(vector, $"item-{item}");
        }
        using var stream = new MemoryStream();
        index.Save(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// Equality by re-serialization: the artifact is the whole observable state,
    /// so two indexes that save to the same bytes are the same index.
    /// </summary>
    private static void AssertSameIndex(EmbeddingIndex expected, EmbeddingIndex actual)
    {
        using var left = new MemoryStream();
        using var right = new MemoryStream();
        expected.Save(left);
        actual.Save(right);
        Assert.Equal(left.ToArray(), right.ToArray());
    }

    /// <summary>A read-only stream with no length and no seek — a pipe, in effect.</summary>
    private sealed class UnseekableStream : Stream
    {
        private readonly MemoryStream _inner;
        private readonly int _chunk;

        public UnseekableStream(byte[] bytes, int chunk = int.MaxValue)
        {
            _inner = new MemoryStream(bytes);
            _chunk = chunk;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            _inner.Read(buffer, offset, Math.Min(count, _chunk));

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>A seekable stream that under-reports its length and holds more.</summary>
    private sealed class ShortLengthStream : Stream
    {
        private readonly MemoryStream _inner;
        private readonly long _declared;

        public ShortLengthStream(byte[] bytes, long declared)
        {
            _inner = new MemoryStream(bytes);
            _declared = declared;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _declared;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>A seekable, honest stream that never returns more than <c>chunk</c> bytes at a time.</summary>
    private sealed class TrickleStream : Stream
    {
        private readonly MemoryStream _inner;
        private readonly int _chunk;

        public TrickleStream(byte[] bytes, int chunk)
        {
            _inner = new MemoryStream(bytes);
            _chunk = chunk;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _inner.Length;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            _inner.Read(buffer, offset, Math.Min(count, _chunk));

        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
