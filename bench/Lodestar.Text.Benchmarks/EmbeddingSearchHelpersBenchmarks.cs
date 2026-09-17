using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Search;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// The two <c>Lodestar.Embeddings.Search</c> passes around a search rather than the search itself:
/// normalizing a bulk block into an index, and MMR's greedy reselection over its candidates.
/// </summary>
[MemoryDiagnoser]
public class EmbeddingSearchHelpersBenchmarks
{
    private const int Dimension = 384;
    private float[] _block = [];
    private float[] _query = [];
    private List<float[]> _candidates = [];

    /// <summary>How many rows the block holds, and how many candidates MMR reselects over.</summary>
    [Params(1_000, 100_000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // S2245 / CA5394: a seeded Random makes the block reproducible; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(474);
        _block = new float[Rows * Dimension];
        for (int i = 0; i < _block.Length; i++)
        {
            _block[i] = (float)((random.NextDouble() * 2) - 1);
        }
        _query = new float[Dimension];
        for (int j = 0; j < Dimension; j++)
        {
            _query[j] = (float)((random.NextDouble() * 2) - 1);
        }
#pragma warning restore S2245, CA5394
        _candidates = [.. Enumerable.Range(0, Math.Min(Rows, 1_000)).Select(row => _block.AsSpan(row * Dimension, Dimension).ToArray())];
    }

    [Benchmark]
    public int FromBlockNormalized() => EmbeddingIndex.FromBlock(_block, Dimension, BlockNormalization.Normalize).Count;

    /// <summary>MMR over at most 1,000 candidates, selecting 100.</summary>
    [Benchmark]
    public int MmrSelect100() => Mmr.Select(_query, _candidates, 100).Length;
}
