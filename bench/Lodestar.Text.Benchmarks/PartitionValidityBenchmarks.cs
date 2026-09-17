using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible block; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The two linear validity scores on many samples of few features, where reading the labels is a visible share.</summary>
[MemoryDiagnoser]
public class PartitionValidityBenchmarks
{
    private const int Features = 2;
    private const int Clusters = 10;
    private int[] _labels = [];
    private double[] _features = [];

    /// <summary>How many samples.</summary>
    [Params(1_000_000)]
    public int Samples { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(17);
        _labels = [.. Enumerable.Range(0, Samples).Select(_ => random.Next(Clusters))];
        _features = [.. Enumerable.Range(0, Samples * Features).Select(i => random.NextDouble() + _labels[i / Features])];
    }

    [Benchmark]
    public double DaviesBouldinScore() => DaviesBouldin.Score(_labels, _features, Features);

    [Benchmark]
    public double CalinskiHarabaszScore() => CalinskiHarabasz.Score(_labels, _features, Features);
}
