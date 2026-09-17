using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible label matrix; no security use.
#pragma warning disable S2245, CA5394

/// <summary>One two-by-two matrix per label, per sample, and per class of a label vector.</summary>
[MemoryDiagnoser]
public class MultilabelConfusionMatrixBenchmarks
{
    private const int Labels = 20;
    private bool[] _true = [];
    private bool[] _predicted = [];
    private int[] _classTrue = [];
    private int[] _classPredicted = [];
    private double[] _weights = [];

    /// <summary>How many rows, and how many samples of the label vector.</summary>
    [Params(100_000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(11);
        _true = [.. Enumerable.Range(0, Rows * Labels).Select(_ => random.Next(4) == 0)];
        _predicted = [.. _true.Select(value => random.Next(5) == 0 ? !value : value)];
        _classTrue = [.. Enumerable.Range(0, Rows).Select(_ => random.Next(Labels))];
        _classPredicted = [.. _classTrue.Select(value => random.Next(3) == 0 ? random.Next(Labels) : value)];
        _weights = [.. Enumerable.Range(0, Rows).Select(_ => random.NextDouble() + 0.5)];
    }

    [Benchmark]
    public ConfusionMatrix[] PerLabel() => MultilabelConfusionMatrix.Compute(_true, _predicted, Labels);

    [Benchmark]
    public ConfusionMatrix[] PerLabelWeighted() =>
        MultilabelConfusionMatrix.Compute(_true, _predicted, Labels, sampleWeight: _weights);

    [Benchmark]
    public ConfusionMatrix[] PerSample() =>
        MultilabelConfusionMatrix.Compute(_true, _predicted, Labels, samplewise: true);

    [Benchmark]
    public ConfusionMatrix[] PerClass() => MultilabelConfusionMatrix.Compute(_classTrue, _classPredicted);
}
