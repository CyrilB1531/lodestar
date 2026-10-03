using System.Globalization;
using BenchmarkDotNet.Attributes;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// The costs the Reviews B after #1619 and #1628 found in the #1618 persistence rework: the writes an asynchronous
/// save makes (#1623), and what a load from a stream of undeclared length allocates, tiny to past a mebibyte (#1624, #1629).
/// </summary>
/// <remarks>
/// The saves go to a sink whose <c>WriteAsync</c> yields once, standing for a network or pipe round trip, so a save
/// that makes ten writes where it made one pays ten round trips here as it would there. The loads read through a
/// stream that declares no length, which is the path a <c>GZipStream</c> or a socket takes. The corpus is generated:
/// what is measured depends on how many bytes the artifact holds, not on which.
/// </remarks>
[MemoryDiagnoser]
public class AsyncArtifactBenchmarks
{
    private TfidfVectorizer _small = null!;
    private CountVectorizer _wide = null!;
    private byte[] _smallArtifact = [];
    private byte[] _hashingArtifact = [];
    private byte[] _countArtifact24K = [];
    private byte[] _countArtifact78K = [];
    private byte[] _countArtifact1M = [];

    [GlobalSetup]
    public void Setup()
    {
        string[] documents = [.. Enumerable.Range(0, 50).Select(d => string.Join(
            " ", Enumerable.Range(0, 20).Select(w => "w" + ((d * 7 + w * 13) % 400).ToString(CultureInfo.InvariantCulture))))];
        _small = new TfidfVectorizer().Fit(documents);

        // 20,000 terms of nine characters: an artifact of about 240 KB, under a mebibyte.
        _wide = new CountVectorizer().Fit(
            [string.Join(" ", Enumerable.Range(0, 20_000).Select(i => "t" + i.ToString("D8", CultureInfo.InvariantCulture)))]);

        using (var stream = new MemoryStream())
        {
            _small.Save(stream);
            _smallArtifact = stream.ToArray();
        }

        using (var stream = new MemoryStream())
        {
            new HashingVectorizer().Save(stream);
            _hashingArtifact = stream.ToArray();
        }

        // About 24 KB, 78 KB and 1.08 MB: where a growing read allocated more than the MemoryStream before (#1629).
        _countArtifact24K = CountArtifact(2_000);
        _countArtifact78K = CountArtifact(6_500);
        _countArtifact1M = CountArtifact(90_000);
    }

    /// <summary>A small TF-IDF vectorizer saved asynchronously over a round trip a write.</summary>
    [Benchmark]
    public async Task<long> TfidfSaveAsyncSmall()
    {
        using var sink = new YieldingSink();
        await _small.SaveAsync(sink).ConfigureAwait(false);
        return sink.Writes;
    }

    /// <summary>A 240 KB count vectorizer saved asynchronously over a round trip a write.</summary>
    [Benchmark]
    public async Task<long> CountSaveAsyncWide()
    {
        using var sink = new YieldingSink();
        await _wide.SaveAsync(sink).ConfigureAwait(false);
        return sink.Writes;
    }

    /// <summary>A hashing vectorizer, a few hundred bytes, loaded from a stream of undeclared length.</summary>
    [Benchmark]
    public HashingVectorizer HashingLoadUndeclared()
    {
        using var pipe = new Undeclared(_hashingArtifact);
        return HashingVectorizer.Load(pipe);
    }

    /// <summary>The small TF-IDF vectorizer loaded from a stream of undeclared length.</summary>
    [Benchmark]
    public TfidfVectorizer TfidfLoadUndeclared()
    {
        using var pipe = new Undeclared(_smallArtifact);
        return TfidfVectorizer.Load(pipe);
    }

    /// <summary>A count vectorizer of about 24 KB loaded from a stream of undeclared length.</summary>
    [Benchmark]
    public CountVectorizer CountLoadUndeclared24K()
    {
        using var pipe = new Undeclared(_countArtifact24K);
        return CountVectorizer.Load(pipe);
    }

    /// <summary>A count vectorizer of about 78 KB loaded from a stream of undeclared length.</summary>
    [Benchmark]
    public CountVectorizer CountLoadUndeclared78K()
    {
        using var pipe = new Undeclared(_countArtifact78K);
        return CountVectorizer.Load(pipe);
    }

    /// <summary>A count vectorizer of about 1.08 MB, just past a mebibyte, loaded from a stream of undeclared length.</summary>
    [Benchmark]
    public CountVectorizer CountLoadUndeclared1M()
    {
        using var pipe = new Undeclared(_countArtifact1M);
        return CountVectorizer.Load(pipe);
    }

    private static byte[] CountArtifact(int terms)
    {
        var vectorizer = new CountVectorizer().Fit(
            [string.Join(" ", Enumerable.Range(0, terms).Select(i => "t" + i.ToString("D8", CultureInfo.InvariantCulture)))]);
        using var stream = new MemoryStream();
        vectorizer.Save(stream);
        return stream.ToArray();
    }

    /// <summary>A write-only stream that discards its bytes, yielding once a write as a round trip would.</summary>
    private sealed class YieldingSink : Stream
    {
        public long Writes { get; private set; }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) => Writes++;

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            WriteAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            Writes++;
        }

        public override void Flush()
        {
            // Nothing is held.
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();
    }

    /// <summary>A read-only stream over bytes that declares no length and cannot seek, as a pipe does.</summary>
    private sealed class Undeclared(byte[] bytes) : Stream
    {
        private int _position;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int taken = Math.Min(count, bytes.Length - _position);
            Array.Copy(bytes, _position, buffer, offset, taken);
            _position += taken;
            return taken;
        }

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush()
        {
            // Nothing is written.
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();
    }
}
