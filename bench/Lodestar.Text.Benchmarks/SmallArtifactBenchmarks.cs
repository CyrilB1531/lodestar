using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// Asynchronous saves of a few hundred bytes, and a 1.4 MB load from a seekable stream: the costs the Review B after
/// #1644 found above the release (#1649).
/// </summary>
/// <remarks>
/// A save this small is the fixed cost of the path, buffer and writer and state machines, which an artifact of a
/// mebibyte hides; the saves go to <see cref="Stream.Null"/>, so the time is the library's own.
/// </remarks>
[MemoryDiagnoser]
public class SmallArtifactBenchmarks
{
    private CountVectorizer _count = null!;
    private TfidfVectorizer _tfidf = null!;
    private HashingVectorizer _hashing = null!;
    private EmbeddingIndex _index = null!;
    private byte[] _count1400K = [];

    [GlobalSetup]
    public void Setup()
    {
        _count = ArtifactCorpus.Count(1);
        _tfidf = new TfidfVectorizer().Fit([ArtifactCorpus.Terms(1), "t00000000"]);
        _hashing = new HashingVectorizer();
        _index = new EmbeddingIndex(2, normalize: false);
        _index.Add([1f, 2f], "a");
        _count1400K = ArtifactCorpus.Bytes(ArtifactCorpus.Count(116_660));
    }

    /// <summary>A count vectorizer of one term, 271 bytes, saved asynchronously.</summary>
    [Benchmark]
    public Task CountSaveAsync() => _count.SaveAsync(Stream.Null);

    /// <summary>A TF-IDF vectorizer of one term, 365 bytes, saved asynchronously.</summary>
    [Benchmark]
    public Task TfidfSaveAsync() => _tfidf.SaveAsync(Stream.Null);

    /// <summary>A hashing vectorizer, 284 bytes, saved asynchronously.</summary>
    [Benchmark]
    public Task HashingSaveAsync() => _hashing.SaveAsync(Stream.Null);

    /// <summary>An index of one id, 207 bytes, saved asynchronously.</summary>
    [Benchmark]
    public Task IndexSaveAsync() => _index.SaveAsync(Stream.Null);

    /// <summary>A 1.4 MB count vectorizer loaded from a <see cref="MemoryStream"/>.</summary>
    [Benchmark]
    public CountVectorizer CountLoadSeekable1400K()
    {
        using var stream = new MemoryStream(_count1400K);
        return CountVectorizer.Load(stream);
    }
}
