using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// The precision family over many labels, where the dense <c>m × m</c> matrix it used to build costs
/// <c>8·m²</c> bytes a call: 200 MB at 5,000 labels (#1200).
/// </summary>
[MemoryDiagnoser]
public class ManyLabelsMetricsBenchmarks
{
    private int[] _yTrue = [];
    private int[] _yPred = [];

    [Params(1_000, 5_000)]
    public int Labels { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(1200);
        _yTrue = new int[100_000];
        _yPred = new int[_yTrue.Length];
        for (int i = 0; i < _yTrue.Length; i++)
        {
            _yTrue[i] = rng.Next(Labels);
            _yPred[i] = rng.NextDouble() < 0.7 ? _yTrue[i] : rng.Next(Labels);
        }
    }

    [Benchmark]
    public double F1Macro() => F1.Score(_yTrue, _yPred, Averaging.Macro);

    [Benchmark]
    public double PrecisionWeighted() => Precision.Score(_yTrue, _yPred, Averaging.Weighted);

    [Benchmark]
    public int ReportRows() => ClassificationReport.Compute(_yTrue, _yPred).Classes.Count;
}
