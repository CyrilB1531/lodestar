using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible labelling; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The report from an already-counted matrix, so the class count rather than the samples sets its cost.</summary>
[MemoryDiagnoser]
public class ClassificationReportBenchmarks
{
    private ConfusionMatrix? _matrix;

    /// <summary>How many classes.</summary>
    [Params(10, 1_000)]
    public int Classes { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(19);
        int[] yTrue = [.. Enumerable.Range(0, 100_000).Select(_ => random.Next(Classes))];
        int[] yPred = [.. yTrue.Select(label => random.Next(3) == 0 ? random.Next(Classes) : label)];
        _matrix = ConfusionMatrix.Compute(yTrue, yPred);
    }

    [Benchmark]
    public ClassificationReport Report() => ClassificationReport.Compute(_matrix!);
}
