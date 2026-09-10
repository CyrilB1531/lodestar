using Lodestar.Embeddings.Search;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random builds a reproducible corpus so a
// failing case replays; there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>A seeded corpus and the CPU answer it is compared against.</summary>
internal static class CosineCorpus
{
    /// <summary>Row-major vectors from a fixed seed, so a failure replays.</summary>
    public static float[] Rows(int count, int dimension, int seed)
    {
        var random = new Random(seed);
        var values = new float[count * dimension];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (float)((random.NextDouble() * 2.0) - 1.0);
        }

        return values;
    }

    /// <summary>The SIMD path's answer: what the kernel has to reproduce.</summary>
    public static IReadOnlyList<SearchResult> Baseline(
        float[] rows, int count, int dimension, ReadOnlySpan<float> query, int k)
    {
        var index = new EmbeddingIndex(dimension);
        for (int row = 0; row < count; row++)
        {
            index.Add(rows.AsSpan(row * dimension, dimension));
        }

        return index.Search(query, k);
    }
}
