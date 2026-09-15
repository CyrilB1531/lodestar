using BenchmarkDotNet.Attributes;
using Cortex.TimeSeries.Decomposition;
using Cortex.TimeSeries.Diagnostics;
using Lodestar.Stats.TimeSeries;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark series; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary>
/// The stationarity tests and the seasonal decomposition against <c>Cortex.TimeSeries</c> 1.1.0
/// (issue #671), each pair on the configuration where both sides return the same statistic.
/// </summary>
/// <remarks>
/// ADF at a fixed lag, since Cortex treats a zero maximum as a request for its own rule; KPSS at the
/// lag Cortex chooses, fixed here; the decomposition at period 12. Cortex's ADF p-value is clamped at
/// 0.01 rather than read from MacKinnon's surface, so only the statistics agree — bench/README.md
/// section 35 has the check.
/// </remarks>
[MemoryDiagnoser]
public class StationarityBenchmarks
{
    private const int AdfLag = 4;
    private const int Period = 12;

    private double[] _series = [];
    private double[] _seasonal = [];
    private DickeyFullerOptions _adf = null!;
    private KpssOptions _kpss = null!;

    [Params(200, 2_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(671);
        double value = 0.0;
        var series = new double[SampleSize];
        var seasonal = new double[SampleSize];
        for (int i = 0; i < SampleSize; i++)
        {
            value = (0.5 * value) + random.NextDouble() - 0.5;
            series[i] = value;
            seasonal[i] = 20.0 + (0.01 * i) + (4.0 * Math.Sin(2.0 * Math.PI * i / Period)) + value;
        }

        _series = series;
        _seasonal = seasonal;
        _adf = new DickeyFullerOptions { LagSelection = LagSelection.Fixed, MaxLag = AdfLag };

        // Cortex picks its own KPSS window; fixing ours to it is what makes the statistics equal.
        int cortexLags = StationarityTests.KPSS(_series, "c").UsedLags;
        _kpss = new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = cortexLags };

        Agree(LodestarAugmentedDickeyFuller(), CortexAugmentedDickeyFuller(), "ADF statistic");
        Agree(LodestarKpss(), CortexKpss(), "KPSS statistic");
        Agree(LodestarDecompose(), CortexDecompose(), "seasonal component");
    }

    private static void Agree(double ours, double theirs, string what)
    {
        // Negated, so a NaN on either side refuses too: every ordered comparison with NaN is false.
        if (!(Math.Abs(ours - theirs) <= 1e-9 * Math.Max(1.0, Math.Abs(ours))))
        {
            throw new InvalidOperationException($"{what}: Lodestar {ours:R} against Cortex {theirs:R}.");
        }
    }

    [Benchmark(Baseline = true)]
    public double LodestarAugmentedDickeyFuller() =>
        Stationarity.AugmentedDickeyFuller(_series, _adf).Statistic;

    [Benchmark]
    public double CortexAugmentedDickeyFuller() =>
        StationarityTests.AugmentedDickeyFuller(_series, AdfLag).TestStatistic;

    [Benchmark]
    public double LodestarKpss() => Stationarity.Kpss(_series, _kpss).Statistic;

    [Benchmark]
    public double CortexKpss() => StationarityTests.KPSS(_series, "c").TestStatistic;

    [Benchmark]
    public double LodestarDecompose() =>
        SeasonalDecomposition.Decompose(_seasonal, Period).Seasonal[0];

    [Benchmark]
    public double CortexDecompose() =>
        SeasonalDecompose.Decompose(_seasonal, Period, DecomposeType.Additive).Seasonal[0];
}
