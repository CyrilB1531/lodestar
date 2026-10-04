using BenchmarkDotNet.Attributes;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>The loads of <see cref="ArtifactTimeBenchmarks"/>, in a process that has fitted nothing large.</summary>
[MemoryDiagnoser]
public class ArtifactLoadOnlyBenchmarks
{
    private byte[] _count2M = [];
    private byte[] _count4M = [];

    [GlobalSetup]
    public void Setup()
    {
        // Spliced rather than fitted, so this process's heap holds the artifacts and nothing a large fit left.
        _count2M = ArtifactCorpus.SplicedCount(175_000);
        _count4M = ArtifactCorpus.SplicedCount(350_000);
    }

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
