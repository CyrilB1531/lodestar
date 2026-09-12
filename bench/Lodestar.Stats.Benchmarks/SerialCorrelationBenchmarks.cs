using BenchmarkDotNet.Attributes;
using Cortex.TimeSeries.Diagnostics;
using Lodestar.Stats.TimeSeries;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark series; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// The three serial-correlation diagnostics against <c>Cortex.TimeSeries</c> 1.1.0, the one
/// .NET library carrying the same three functions (issue #617). Each pair is timed on the
/// capability both sides share -- see bench/README.md for what that excludes.
/// </summary>
[MemoryDiagnoser]
public class SerialCorrelationBenchmarks
{
    private const int LagCount = 20;

    private double[] _series = [];

    [Params(200, 2_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(617);
        double value = 0.0;
        var series = new double[SampleSize];
        for (int i = 0; i < SampleSize; i++)
        {
            value = (0.6 * value) + random.NextDouble();
            series[i] = value;
        }

        _series = series;
    }

    [Benchmark(Baseline = true)]
    public double LodestarAutocorrelation() =>
        SerialCorrelation.Autocorrelation(_series, LagCount).Values[^1];

    [Benchmark]
    public double CortexAutocorrelation() =>
        AutocorrelationTests.ACF(_series, LagCount)[^1];

    [Benchmark]
    public double LodestarPartialAutocorrelation() =>
        SerialCorrelation.PartialAutocorrelation(_series, LagCount).Values[^1];

    [Benchmark]
    public double CortexPartialAutocorrelation() =>
        AutocorrelationTests.PACF(_series, LagCount)[^1];

    [Benchmark]
    public double LodestarLjungBox() =>
        SerialCorrelation.LjungBox(_series, LagCount).Statistics[^1];

    [Benchmark]
    public double CortexLjungBox() =>
        AutocorrelationTests.LjungBox(_series, LagCount).TestStatistic;
}
