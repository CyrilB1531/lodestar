using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible block; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The per-sample silhouette from the samples themselves.</summary>
[MemoryDiagnoser]
public class SilhouetteBenchmarks
{
    private const int Features = 16;
    private const int Clusters = 8;
    private int[] _labels = [];
    private double[] _features = [];

    /// <summary>How many samples.</summary>
    [Params(2_000, 5_000)]
    public int Samples { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(1234);
        _labels = [.. Enumerable.Range(0, Samples).Select(i => i % Clusters)];
        _features = [.. Enumerable.Range(0, Samples * Features).Select(i => random.NextDouble() + ((i / Features) % Clusters))];
    }

    [Benchmark]
    public double[] PerSample() => Silhouette.PerSample(_labels, _features, Features);
}
