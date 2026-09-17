using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds reproducible scores; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The binary curves, which keep every point where the area scores only integrate them.</summary>
[MemoryDiagnoser]
public class ClassifierCurveBenchmarks
{
    private int[] _true = [];
    private double[] _scores = [];

    /// <summary>How many samples.</summary>
    [Params(1_000_000)]
    public int Samples { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(13);
        _true = [.. Enumerable.Range(0, Samples).Select(_ => random.Next(2))];
        _scores = [.. _true.Select(label => (random.NextDouble() + (0.3 * label)) / 1.3)];
    }

    [Benchmark]
    public RocCurve Roc() => RocCurve.Compute(_true, _scores);

    [Benchmark]
    public PrecisionRecallCurve PrecisionRecall() => PrecisionRecallCurve.Compute(_true, _scores);
}
