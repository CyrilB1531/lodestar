using Lodestar.Embeddings.Search;

namespace Lodestar.Gpu.Benchmarks;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The seeded vectors and the CPU index the cosine benchmarks share.</summary>
internal static class SeededVectors
{
    /// <summary>The generator a corpus is drawn from, fixed so that a figure replays.</summary>
    public static Random Seeded(int seed) => new(seed);

    /// <summary><paramref name="length"/> values uniform on [-1, 1), drawn in order from <paramref name="random"/>.</summary>
    public static float[] Uniform(Random random, int length)
    {
        var values = new float[length];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (float)((random.NextDouble() * 2.0) - 1.0);
        }

        return values;
    }

    /// <summary>The SIMD path's index over the same rows, which is the gate's baseline.</summary>
    public static EmbeddingIndex Index(float[] rows, int count, int dimension)
    {
        var index = new EmbeddingIndex(dimension);
        for (int row = 0; row < count; row++)
        {
            index.Add(rows.AsSpan(row * dimension, dimension));
        }

        return index;
    }

    /// <summary>The name of the device <see cref="Lodestar.Gpu.Compute.GpuContext.Create"/> opens, flagged when it is not a GPU.</summary>
    public static string Device()
    {
        using var probe = Lodestar.Gpu.Compute.GpuContext.Create();
        return probe.IsHardwareGpu ? probe.DeviceName : $"{probe.DeviceName} (NOT a GPU)";
    }
}
