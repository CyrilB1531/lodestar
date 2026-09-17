using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>The two-sided asymptotic Kolmogorov-Smirnov p-value where it raises Durbin's matrix to a power.</summary>
/// <remarks>
/// Two evenly spaced samples, the second shifted, so the statistic is the shift and the route is fixed: an effective size
/// of 50 or 140 at a shift of 0.1 (matrices of order 9 and 27), and 10,000 at 0.002, the order 39 the
/// <c>n·d^1.5 ≤ 1.4</c> bound still sends there.
/// </remarks>
[MemoryDiagnoser]
public class KolmogorovDurbinBenchmarks
{
    private double[] _a = [];
    private double[] _b = [];

    /// <summary>How many values each sample carries; the effective size is half of it.</summary>
    [Params(100, 280, 20_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        double shift = SampleSize >= 10_000 ? 0.002 : 0.1;
        _a = [.. Enumerable.Range(0, SampleSize).Select(i => (double)i / SampleSize)];
        _b = [.. _a.Select(value => value + shift)];
    }

    [Benchmark]
    public double Asymptotic() =>
        KolmogorovSmirnov.TwoSample(_a, _b, method: ExactMethod.Asymptotic).PValue;
}
