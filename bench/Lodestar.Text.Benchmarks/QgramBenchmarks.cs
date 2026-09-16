using BenchmarkDotNet.Attributes;
using Lodestar.Text.Similarity;

namespace Lodestar.Text.Benchmarks;

/// <summary>A q-gram set similarity over a pair of sentences, at unigrams and trigrams.</summary>
[MemoryDiagnoser]
public class QgramBenchmarks
{
    private const string A = "the quick brown fox jumps over the lazy dog while the cat sleeps on a warm mat";
    private const string B = "a quick brown dog jumps over the lazy fox while the cat naps on the warm rug";

    /// <summary>Gram length.</summary>
    [Params(1, 3)]
    public int Q { get; set; }

    [Benchmark]
    public double JaccardSimilarity() => Jaccard.Similarity(A, B, Q);
}
