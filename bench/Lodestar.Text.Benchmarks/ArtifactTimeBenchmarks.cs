using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// What a save and a load of undeclared length take in time, in a process that has fitted large vectorizers, and a fit
/// over long documents: the costs the Review B after #1639 and its review found against the latest release (#1642, #1643).
/// </summary>
/// <remarks>
/// The fits are the setup, and the heap they leave is part of what is measured: a scratch list a fit kept slowed every
/// later operation (#1239), and a reader over mebibyte segments was slower there than one array. Saves go to
/// <see cref="Stream.Null"/>, so the time is the library's own. <see cref="ArtifactLoadOnlyBenchmarks"/> loads the same
/// artifacts in a process that fits nothing.
/// </remarks>
[MemoryDiagnoser]
public class ArtifactTimeBenchmarks
{
    private CountVectorizer _count240K = null!;
    private TfidfVectorizer _tfidf45K = null!;
    private EmbeddingIndex _index942K = null!;
    private byte[] _count2M = [];
    private byte[] _count4M = [];
    private string[] _longDocuments = [];

    [GlobalSetup]
    public void Setup()
    {
        _count240K = ArtifactCorpus.Count(20_000);
        _tfidf45K = new TfidfVectorizer().Fit([ArtifactCorpus.Terms(2_000), "t00000000"]);
        _index942K = ArtifactCorpus.Index(10_000);
        _count2M = ArtifactCorpus.Bytes(ArtifactCorpus.Count(175_000));
        _count4M = ArtifactCorpus.Bytes(ArtifactCorpus.Count(350_000));
        _longDocuments = [.. Enumerable.Range(0, 200).Select(ArtifactCorpus.LongDocument)];
    }

    /// <summary>
    /// 200 documents of 20,000 tokens fitted: the scratch list is let go once the fit ends, as letting it go after each
    /// document regrew it 200 times (#1643).
    /// </summary>
    [Benchmark]
    public CountVectorizer CountFitLongDocuments() => new CountVectorizer().Fit(_longDocuments);

    /// <summary>A 240 KB count vectorizer saved synchronously.</summary>
    [Benchmark]
    public void CountSave240K() => _count240K.Save(Stream.Null);

    /// <summary>The same, asynchronously.</summary>
    [Benchmark]
    public Task CountSaveAsync240K() => _count240K.SaveAsync(Stream.Null);

    /// <summary>A 45 KB TF-IDF vectorizer saved synchronously.</summary>
    [Benchmark]
    public void TfidfSave45K() => _tfidf45K.Save(Stream.Null);

    /// <summary>The same, asynchronously.</summary>
    [Benchmark]
    public Task TfidfSaveAsync45K() => _tfidf45K.SaveAsync(Stream.Null);

    /// <summary>A 942 KB index of 10,000 ids saved synchronously.</summary>
    [Benchmark]
    public void IndexSave942K() => _index942K.Save(Stream.Null);

    /// <summary>The same, asynchronously.</summary>
    [Benchmark]
    public Task IndexSaveAsync942K() => _index942K.SaveAsync(Stream.Null);

    /// <summary>A 2.1 MB count vectorizer loaded asynchronously from a stream of undeclared length.</summary>
    [Benchmark]
    public Task<CountVectorizer> CountLoadAsync2M() => ArtifactCorpus.LoadAsync(_count2M);

    /// <summary>A 4.2 MB count vectorizer loaded asynchronously from a stream of undeclared length.</summary>
    [Benchmark]
    public Task<CountVectorizer> CountLoadAsync4M() => ArtifactCorpus.LoadAsync(_count4M);

    /// <summary>The same, synchronously.</summary>
    [Benchmark]
    public CountVectorizer CountLoad4M() => ArtifactCorpus.Load(_count4M);
}
