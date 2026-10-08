using Lodestar.Internal.Persistence;
using Lodestar.Text.Persistence;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Persistence;

/// <summary>A save writes an artifact a mebibyte at a time, and a load reads it back past one array (#1618).</summary>
public sealed class WholeArtifactTests
{
    /// <summary>The limits a default load applies, its one-array ceiling brought down to 64 bytes.</summary>
    private static readonly ArtifactLimits SixtyFourByteArray = new(
        ArtifactLimits.DefaultMaxVocabularySize,
        ArtifactLimits.DefaultMaxTokenLength,
        ArtifactLimits.DefaultMaxJsonDepth,
        ArtifactLimits.DefaultMaxTotalBytes,
        ArtifactLimits.DefaultMaxArrayLength,
        maxSingleBuffer: 64);

    /// <summary>What the JSON writer, and so 0.7.0, raises for a string of 166,666,667 characters (#1646).</summary>
    private const string LongValue = "The JSON value of length 166666667 is too large and not supported.";

    private static readonly string[] Corpus = ["apple banana", "banana cherry", "cherry date", "the end"];

    [Fact]
    public async Task Every_vectorizer_loads_past_one_array_from_any_stream()
    {
        var tfidf = new TfidfVectorizer();
        tfidf.Fit(Corpus);
        byte[] tfidfBytes = Bytes(tfidf.Save);
        var count = new CountVectorizer();
        count.Fit(Corpus);
        byte[] countBytes = Bytes(count.Save);
        byte[] hashingBytes = Bytes(new HashingVectorizer(new HashingVectorizerOptions { Count = new CountVectorizerOptions { StopWords = ["the", "a"] } }).Save);

        foreach (Func<byte[], Stream> open in new Func<byte[], Stream>[] { b => new MemoryStream(b), b => new Unseekable(b) })
        {
            Assert.Equal(tfidfBytes, Bytes(TfidfVectorizer.Load(open(tfidfBytes), SixtyFourByteArray).Save));
            Assert.Equal(tfidfBytes, Bytes((await TfidfVectorizer.LoadAsync(open(tfidfBytes), SixtyFourByteArray, default)).Save));
            Assert.Equal(countBytes, Bytes(CountVectorizer.Load(open(countBytes), SixtyFourByteArray).Save));
            Assert.Equal(countBytes, Bytes((await CountVectorizer.LoadAsync(open(countBytes), SixtyFourByteArray, default)).Save));
            Assert.Equal(hashingBytes, Bytes(HashingVectorizer.Load(open(hashingBytes), SixtyFourByteArray).Save));
            Assert.Equal(hashingBytes, Bytes((await HashingVectorizer.LoadAsync(open(hashingBytes), SixtyFourByteArray, default)).Save));
        }
    }

    [Fact]
    public void A_stream_of_undeclared_length_is_refused_at_the_byte_count_main_named()
    {
        // Main read and counted 81,920 bytes at a time: 1,000,000 bytes against a 300,000-byte limit stop at 327,680.
        InvalidDataException error = Assert.Throws<InvalidDataException>(
            () => CountVectorizer.Load(new Unseekable(new byte[1_000_000], 81_920), new ArtifactLoadOptions { MaxTotalBytes = 300_000 }));
        Assert.Contains("327680", error.Message, StringComparison.Ordinal);

        // Past a mebibyte too: 3,000,000 bytes against 2,250,000 stop at 28 reads, 2,293,760.
        error = Assert.Throws<InvalidDataException>(
            () => CountVectorizer.Load(new Unseekable(new byte[3_000_000], 81_920), new ArtifactLoadOptions { MaxTotalBytes = 2_250_000 }));
        Assert.Contains("2293760", error.Message, StringComparison.Ordinal);

        // A stream answering 65,536 bytes a read is counted read by read: 100,000 is passed at 131,072.
        error = Assert.Throws<InvalidDataException>(
            () => CountVectorizer.Load(new Unseekable(new byte[1_000_000], 65_536), new ArtifactLoadOptions { MaxTotalBytes = 100_000 }));
        Assert.Contains("131072", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_vocabulary_reaches_the_stream_a_mebibyte_at_a_time()
    {
        // 220,000 terms of nine characters, about 2.6 MB, reached the stream whole; flushed, no write passes a mebibyte and a term.
        using var recording = new Recording();
        LargeVocabulary().Save(recording);
        Assert.True(recording.Length > 5 << 19, $"The artifact is {recording.Length} bytes.");
        Assert.True(recording.Largest < (1 << 20) + 64, $"One write carried {recording.Largest} bytes.");
    }

    [Fact]
    public async Task An_asynchronous_save_past_a_chunk_writes_the_same_bytes()
    {
        CountVectorizer vectorizer = LargeVocabulary();
        byte[] expected = Bytes(vectorizer.Save);
        using var buffered = new MemoryStream();
        await vectorizer.SaveAsync(buffered);
        Assert.Equal(expected, buffered.ToArray());
    }

    [Fact]
    public void A_string_longer_than_the_writer_takes_is_refused_as_the_writer_refuses_it()
    {
        // System.Text.Json 10 writes 166,666,666 characters and refuses one more, with the exception 0.7.0 raised (#1646).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => FeatureVocabularyJson.EnsureWritableVocabulary(["short", new string('a', 166_666_667)]));
        Assert.Equal(LongValue, error.Message);

        // Past ten million characters a string is tried on the writer itself; one it takes passes.
        FeatureVocabularyJson.EnsureWritableVocabulary([new string('a', 10_000_001), null!]);
    }

    [Fact]
    public async Task A_cancelled_asynchronous_save_writes_nothing()
    {
        // The token reaches the stream's first write, which refuses it as main's one write did, writing nothing.
        using var cancelled = new CancellationTokenSource();
        await cancelled.CancelAsync();
        using var stream = new MemoryStream();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => LargeVocabulary().SaveAsync(stream, cancelled.Token));
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public void A_frequency_the_write_refuses_comes_before_a_long_stop_word()
    {
        // HashingVectorizer never validates MinDf; main's write refused it before the stop words it writes after.
        var hashing = new HashingVectorizer(new HashingVectorizerOptions
        {
            Count = new CountVectorizerOptions { MinDf = double.NaN, StopWords = [new string('a', 166_666_667)] },
        });
        using var stream = new MemoryStream();
        Assert.StartsWith("Cannot persist the non-finite value NaN", Assert.Throws<InvalidDataException>(() => hashing.Save(stream)).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_long_token_pattern_is_refused_as_the_writer_refuses_it()
    {
        // System.Text.Json 10 writes 166,666,666 characters and refuses one more (#1626, #1646).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => VectorizerOptionsJson.EnsureWritable(new CountVectorizerOptions { TokenPattern = new string('a', 166_666_667) }, null));
        Assert.Equal(LongValue, error.Message);
    }

    [Fact]
    public void A_public_save_refuses_a_long_stop_word_through_the_same_check()
    {
        // The token pattern's check runs in it too; a stop word reaches it without compiling a 166-million-character pattern.
        var hashing = new HashingVectorizer(new HashingVectorizerOptions
        {
            Count = new CountVectorizerOptions { StopWords = [new string('a', 166_666_667)] },
        });
        using var stream = new MemoryStream();
        Assert.Equal(LongValue, Assert.Throws<ArgumentException>(() => hashing.Save(stream)).Message);
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public void A_long_vocabulary_term_is_refused_before_a_non_finite_weight()
    {
        // In the order the artifact writes them: the vocabulary, then the idf (#1626).
        var vectorizer = new TfidfVectorizer();
        vectorizer.Fit([new string('a', 166_666_667), "b c"]);
        vectorizer.FittedIdf![0] = double.NaN;
        using var stream = new MemoryStream();
        Assert.Equal(LongValue, Assert.Throws<ArgumentException>(() => vectorizer.Save(stream)).Message);
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public async Task A_refused_save_is_refused_alike_whatever_the_stream_does_with_writes()
    {
        // The writer is pointed away on a refusal, so a stream whose flush fails cannot mask it, sync as async (#1641).
        var vectorizer = new TfidfVectorizer();
        vectorizer.Fit(Corpus);
        vectorizer.FittedIdf![0] = double.NaN;
        var failing = new FailingStream();
        try
        {
            Assert.IsType<InvalidDataException>(Assert.ThrowsAny<Exception>(() => vectorizer.Save(failing)));
            Assert.IsType<InvalidDataException>(
                await Assert.ThrowsAnyAsync<Exception>(() => vectorizer.SaveAsync(failing, TestContext.Current.CancellationToken)));
            Assert.Equal(0, failing.Calls);
        }
        finally
        {
            await failing.DisposeAsync();
        }
    }

    [Theory]
    [InlineData(20_000, 1, 773_608)]
    [InlineData(90_000, 2, 3_300_000)]
    public async Task An_asynchronous_save_costs_no_more_than_the_MemoryStream_before(int terms, int writes, int bound)
    {
        // 0.7.0 saved 240 KB and 1.08 MB in 757,224 and 3,236,504 bytes, one write each; 1.08 MB is two writes here, the
        // writer flushing at a mebibyte, and before #1633 cost 4,253,536, a mebibyte chunk opened for its last 32 KB.
        var vectorizer = new CountVectorizer();
        vectorizer.Fit([string.Join(" ", Enumerable.Range(0, terms).Select(i => "t" + i.ToString("D8", System.Globalization.CultureInfo.InvariantCulture)))]);
        var warm = new Sink();
        var sink = new Sink();
        try
        {
            await vectorizer.SaveAsync(warm, TestContext.Current.CancellationToken);
            long before = GC.GetAllocatedBytesForCurrentThread();
            await vectorizer.SaveAsync(sink, TestContext.Current.CancellationToken);
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            Assert.Equal(writes, sink.Writes);
            Assert.True(allocated < bound, $"{allocated} bytes for {sink.Total}.");
        }
        finally
        {
            await warm.DisposeAsync();
            await sink.DisposeAsync();
        }
    }

    [Fact]
    public void A_tiny_artifact_from_a_stream_of_undeclared_length_allocates_what_it_did()
    {
        // The read's own 81,920-byte buffer, as before, and one array sized to the read, not another 81,920 (#1624, #1629).
        byte[] artifact = Bytes(new HashingVectorizer().Save);
        using var warmUp = new Unseekable(artifact, 81_920);
        using var measured = new Unseekable(artifact, 81_920);
        HashingVectorizer.Load(warmUp);
        long before = GC.GetAllocatedBytesForCurrentThread();
        HashingVectorizer.Load(measured);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(allocated < 81_920 + 40_000, $"{allocated} bytes allocated.");
    }

    /// <summary>
    /// Artifacts of about 24 KB, 78 KB, 1.08 MB and 2.1 MB, a read size, and what an undeclared load may allocate over a
    /// seekable one: 0.7.0's MemoryStream cost and 16 KB, the first array growing as that buffer did up to 64 MiB (#1642).
    /// Read 65,536 bytes at a time, that is 198,608 at 78 KB (#1629) and about 3.13 and 6.31 MB at 1.08 and 2.1 MB.
    /// </summary>
    public static TheoryData<int, int, int> Bands => new()
    {
        { 2_000, 81_920, 98_304 },
        { 2_000, 65_536, 98_304 },
        { 6_500, 81_920, 98_304 },
        { 6_500, 65_536, 214_992 },
        { 90_000, 81_920, 1_600_000 },
        { 90_000, 65_536, 3_150_000 },
        { 175_000, 65_536, 6_325_000 },
    };

    [Theory]
    [MemberData(nameof(Bands))]
    public void A_stream_of_undeclared_length_costs_no_more_than_the_MemoryStream_before(int terms, int chunk, int bound)
    {
        byte[] artifact = CountArtifact(terms);
        long extra = ExtraAllocated(artifact, chunk);
        Assert.True(extra < bound, $"{extra} bytes more for {artifact.Length}.");
    }

    [Theory]
    [MemberData(nameof(Bands))]
    public async Task An_asynchronous_load_of_undeclared_length_costs_no_more_either(int terms, int chunk, int bound)
    {
        // ReadWholeAsync's growing read, which LoadAsync and EmbeddingIndex.LoadAsync take (#1638, #1662).
        byte[] artifact = CountArtifact(terms);
        long extra = await ExtraAllocatedAsync(artifact, chunk);
        Assert.True(extra < bound, $"{extra} bytes more for {artifact.Length}.");
    }

    private static byte[] CountArtifact(int terms)
    {
        var vectorizer = new CountVectorizer();
        vectorizer.Fit([string.Join(" ", Enumerable.Range(0, terms).Select(i => "t" + i.ToString("D8", System.Globalization.CultureInfo.InvariantCulture)))]);
        return Bytes(vectorizer.Save);
    }

    /// <summary>What a load of <paramref name="artifact"/> read <paramref name="chunk"/> bytes at a time allocates over a seekable one, both warmed.</summary>
    private static long ExtraAllocated(byte[] artifact, int chunk)
    {
        long Measure(Func<Stream> open)
        {
            using (Stream warm = open())
            {
                CountVectorizer.Load(warm);
            }

            using Stream measured = open();
            long before = GC.GetAllocatedBytesForCurrentThread();
            CountVectorizer.Load(measured);
            return GC.GetAllocatedBytesForCurrentThread() - before;
        }

        return Measure(() => new Unseekable(artifact, chunk)) - Measure(() => new MemoryStream(artifact));
    }

    /// <summary><see cref="ExtraAllocated"/> through <c>LoadAsync</c>, which every read here completes synchronously.</summary>
    private static async Task<long> ExtraAllocatedAsync(byte[] artifact, int chunk)
    {
        async Task<long> Measure(Func<Stream> open)
        {
            await using (Stream warm = open())
            {
                await CountVectorizer.LoadAsync(warm);
            }

            await using Stream measured = open();
            long before = GC.GetAllocatedBytesForCurrentThread();
            await CountVectorizer.LoadAsync(measured);
            return GC.GetAllocatedBytesForCurrentThread() - before;
        }

        return await Measure(() => new Unseekable(artifact, chunk)) - await Measure(() => new MemoryStream(artifact));
    }

    [Fact]
    public void A_null_stop_word_is_left_to_the_write_after_the_path()
    {
        // Main opened the path before it wrote a stop word, so the path's refusal keeps its place (#1618).
        var nullStopWord = new CountVectorizer(new CountVectorizerOptions { StopWords = ["a", null!] });
        nullStopWord.Fit(Corpus);
        Assert.Throws<DirectoryNotFoundException>(
            () => nullStopWord.Save(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "x.json")));
    }

    private static CountVectorizer LargeVocabulary()
    {
        var vectorizer = new CountVectorizer();
        vectorizer.Fit([string.Join(" ", Enumerable.Range(0, 220_000).Select(i => "t" + i.ToString("D8", System.Globalization.CultureInfo.InvariantCulture)))]);
        return vectorizer;
    }

    private static byte[] Bytes(Action<Stream> save)
    {
        using var stream = new MemoryStream();
        save(stream);
        return stream.ToArray();
    }

    /// <summary>A pipe, in effect: no length, no seek, and a few bytes a read.</summary>
    private sealed class Unseekable(byte[] bytes, int chunk = 7) : Stream
    {
        private readonly MemoryStream _inner = new(bytes);

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, Math.Min(count, chunk));

        // Answered at once, as a buffered pipe would: Stream's own ReadAsync hops threads, which a measure on one cannot follow.
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            Task.FromResult(Read(buffer, offset, count));

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
            new(_inner.Read(buffer.Span[..Math.Min(buffer.Length, chunk)]));

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

    /// <summary>A write-only stream that keeps nothing but how many writes and bytes it was handed.</summary>
    private sealed class Sink : Stream
    {
        public int Writes { get; private set; }

        public long Total { get; private set; }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) => Note(count);

        public override void Write(ReadOnlySpan<byte> buffer) => Note(buffer.Length);

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            Note(count);
            return Task.CompletedTask;
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            Note(buffer.Length);
            return default;
        }

        public override void Flush()
        {
            // Nothing is held.
        }

        public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        private void Note(int count)
        {
            Writes++;
            Total += count;
        }
    }

    /// <summary>A memory stream that counts its writes and the largest of them.</summary>
    private sealed class Recording : MemoryStream
    {
        public int Writes { get; private set; }

        public int Largest { get; private set; }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Note(count);
            base.Write(buffer, offset, count);
        }

#if !NETFRAMEWORK
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            Note(buffer.Length);
            base.Write(buffer);
        }
#endif

        private void Note(int count)
        {
            Writes++;
            Largest = Math.Max(Largest, count);
        }
    }

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
            return new IOException("flush failed");
        }
    }
}
