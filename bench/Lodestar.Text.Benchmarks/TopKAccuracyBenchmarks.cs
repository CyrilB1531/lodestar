using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds reproducible scores; no security use.
#pragma warning disable S2245, CA5394

/// <summary>Top-k accuracy over many samples, where the per-row ranking is the cost.</summary>
[MemoryDiagnoser]
public class TopKAccuracyBenchmarks
{
    private const int Samples = 200_000;
    private int[] _true = [];
    private double[] _scores = [];

    /// <summary>How many classes each row scores.</summary>
    [Params(10, 100)]
    public int Classes { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(2026);
        _true = [.. Enumerable.Range(0, Samples).Select(_ => random.Next(Classes))];
        _scores = [.. Enumerable.Range(0, Samples * Classes).Select(_ => random.NextDouble())];
    }

    [Benchmark]
    public double TopTwo() => TopKAccuracy.Score(_true, _scores, Classes, k: 2);
}
