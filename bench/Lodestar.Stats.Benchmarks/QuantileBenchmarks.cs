using BenchmarkDotNet.Attributes;

namespace Lodestar.Stats.Benchmarks;

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>The two published quantiles, one call each, at the arguments a fit and a band use.</summary>
/// <remarks>
/// The serial-correlation band and every regression table pay one of these per call, so its
/// cost is read here alone rather than subtracted out of a larger benchmark. The far-tail and
/// Cauchy rows are where an iterative inverse spends the most steps.
/// </remarks>
[MemoryDiagnoser]
public class QuantileBenchmarks
{
    [Benchmark(Baseline = true)]
    public double NormalQuantile() => Distributions.NormalQuantile(0.975);

    [Benchmark]
    public double NormalQuantileFarTail() => Distributions.NormalQuantile(1e-300);

    [Benchmark]
    public double StudentQuantile() => Distributions.StudentQuantile(0.975, 95.0);

    [Benchmark]
    public double StudentQuantileCauchyFarTail() => Distributions.StudentQuantile(1e-12, 1.0);
}
