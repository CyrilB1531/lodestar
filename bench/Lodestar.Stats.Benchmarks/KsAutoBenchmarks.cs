using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>What <see cref="ExactMethod.Auto"/> costs for two equal-size samples, before and after #802.</summary>
/// <remarks>
/// Before #802, <c>Auto</c> took the asymptotic branch past a product of 10,000; it now takes the exact
/// closed form up to 10,000 values each, as scipy does. <c>Asymptotic</c> is therefore the old default
/// and <c>Auto</c> the new one, timed in one window.
/// </remarks>
[MemoryDiagnoser]
public class KsAutoBenchmarks
{
    private double[] _a = [];
    private double[] _b = [];

    /// <summary>How many values each sample carries: sizes where the old default was asymptotic.</summary>
    [Params(1_000, 10_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(802);
        _a = [.. Enumerable.Range(0, SampleSize).Select(_ => random.NextDouble())];
        _b = [.. Enumerable.Range(0, SampleSize).Select(_ => random.NextDouble() + 0.01)];
    }

    [Benchmark(Baseline = true)]
    public double Before_Asymptotic() =>
        KolmogorovSmirnov.TwoSample(_a, _b, method: ExactMethod.Asymptotic).PValue;

    [Benchmark]
    public double After_Auto() => KolmogorovSmirnov.TwoSample(_a, _b).PValue;
}
