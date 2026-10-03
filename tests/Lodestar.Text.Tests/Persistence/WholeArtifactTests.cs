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

        // Past the segments' doubling too: 3,000,000 bytes against 2,250,000 stop at 28 reads, 2,293,760.
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
    public void A_string_longer_than_the_writer_takes_is_refused_by_name()
    {
        // System.Text.Json 10 writes 166,666,666 characters and refuses one more.
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => FeatureVocabularyJson.EnsureWritableVocabulary(["short", new string('a', 166_666_667)]));
        Assert.StartsWith("A vocabulary term of 166666667 characters", error.Message, StringComparison.Ordinal);

        // The writer's own exception, named and kept, whatever type it is (#1625, #1631).
        Assert.IsType<ArgumentException>(error.InnerException);
        Assert.Contains("(ArgumentException)", error.Message, StringComparison.Ordinal);

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
    public void A_long_token_pattern_is_refused_by_name()
    {
        // System.Text.Json 10 writes 166,666,666 characters and refuses one more (#1626).
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => VectorizerOptionsJson.EnsureWritable(new CountVectorizerOptions { TokenPattern = new string('a', 166_666_667) }));
        Assert.StartsWith("The token pattern of 166666667 characters", error.Message, StringComparison.Ordinal);
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
        Assert.StartsWith("A stop word of 166666667 characters", Assert.Throws<InvalidOperationException>(() => hashing.Save(stream)).Message, StringComparison.Ordinal);
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public void A_long_vocabulary_term_is_refused_before_a_non_finite_weight()
    {
        // In the order the artifact writes them: the vocabulary, then the idf (#1626).
        var vectorizer = new TfidfVectorizer();
        vectorizer.Fit([new string('a', 166_666_667), "b c"]);
        Assert.IsType<double[]>(vectorizer.Idf)[0] = double.NaN;
        using var stream = new MemoryStream();
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => vectorizer.Save(stream));
        Assert.StartsWith("A vocabulary term of 166666667 characters", error.Message, StringComparison.Ordinal);
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public async Task An_asynchronous_save_under_a_mebibyte_makes_one_write()
    {
        // Its first chunk grows as the MemoryStream it replaced did, so it goes out in one write, as that did (#1623).
        var vectorizer = new CountVectorizer();
        vectorizer.Fit([string.Join(" ", Enumerable.Range(0, 20_000).Select(i => "t" + i.ToString("D8", System.Globalization.CultureInfo.InvariantCulture)))]);
        var recording = new Recording();
        try
        {
            await vectorizer.SaveAsync(recording, TestContext.Current.CancellationToken);
            Assert.True(recording.Length > 200_000, $"The artifact is {recording.Length} bytes.");
            Assert.Equal(1, recording.Writes);
        }
        finally
        {
            await recording.DisposeAsync();
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

    [Theory]
    [InlineData(2_000, 81_920 + 16_384)]
    [InlineData(6_500, 81_920 + 16_384)]
    [InlineData(90_000, 1_600_000)]
    public void A_stream_of_undeclared_length_costs_what_the_MemoryStream_before_did(int terms, int bound)
    {
        // About 24 KB, 78 KB and 1.08 MB, over what a seekable load allocates: one array grown as 0.7.0's MemoryStream
        // grew it, 81,984 and 1,541,336 bytes there; segments joined cost 115,032, 213,480 and 2,179,848 (#1629).
        var vectorizer = new CountVectorizer();
        vectorizer.Fit([string.Join(" ", Enumerable.Range(0, terms).Select(i => "t" + i.ToString("D8", System.Globalization.CultureInfo.InvariantCulture)))]);
        byte[] artifact = Bytes(vectorizer.Save);
        using var warmUp = new Unseekable(artifact, 81_920);
        using var undeclared = new Unseekable(artifact, 81_920);
        using var seekable = new MemoryStream(artifact);
        CountVectorizer.Load(warmUp);

        long before = GC.GetAllocatedBytesForCurrentThread();
        CountVectorizer.Load(seekable);
        long seekableBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        CountVectorizer.Load(undeclared);
        long undeclaredBytes = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.True(undeclaredBytes - seekableBytes < bound, $"{undeclaredBytes - seekableBytes} bytes more for {artifact.Length}.");
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
}
