using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>One exhaustive query against the index, at the sizes where selection outweighs scoring.</summary>
[MemoryDiagnoser]
public class EmbeddingSearchBenchmarks
{
    private const int Dimension = 384;
    private EmbeddingIndex _index = null!;
    private float[] _query = [];

    /// <summary>How many vectors are stored.</summary>
    [Params(10_000, 100_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(754);
        _index = new EmbeddingIndex(Dimension);
        var row = new float[Dimension];
        for (int item = 0; item < Count; item++)
        {
            for (int d = 0; d < Dimension; d++)
            {
                row[d] = (float)((random.NextDouble() * 2.0) - 1.0);
            }

            _index.Add(row);
        }

        _query = [.. row];
    }

    [Benchmark]
    public IReadOnlyList<SearchResult> SearchTop10() => _index.Search(_query, 10);
}
