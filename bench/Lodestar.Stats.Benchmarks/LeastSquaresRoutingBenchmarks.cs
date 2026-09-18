using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;
using MathNet.Numerics.LinearRegression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// The estimate alone, at the two widths the rank refusal and the conditioning gate route
/// differently.
/// </summary>
/// <remarks>
/// Four regressors is a summary table's width, and shows the rank refusal's bracket costs a narrow
/// fit nothing. 250 is past the point where the Frobenius bound alone sent every design to the
/// reflections however well conditioned (#985). Math.NET's QR is the incumbent, coefficients only.
/// </remarks>
[MemoryDiagnoser]
public class LeastSquaresRoutingBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];
    private double[][] _jagged = [];

    /// <summary>Regressors. Four is a table's width, 250 is past the bound's own floor of p.</summary>
    [Params(4, 250)]
    public int Regressors { get; set; }

    private const int SampleSize = 4_000;

    [GlobalSetup]
    public void Setup()
    {
        (_design, _response, _jagged) = RegressionCorpus.Build(
            978, SampleSize, Regressors, value => (value * 2.0) - 1.0, (_, value) => value);
    }

    // Estimate always takes the reflections, so it prices the rank refusal alone (#978); Fit is
    // the entry point that routes, and the only one the conditioning gate reaches (#985).
    [Benchmark(Baseline = true)]
    public double Lodestar_Estimate() =>
        OrdinaryLeastSquares.Estimate(_design, _response, Regressors, withIntercept: true).Coefficients[1];

    [Benchmark]
    public double Lodestar_Fit() =>
        OrdinaryLeastSquares.Fit(_design, _response, Regressors).Coefficients[1];

    [Benchmark]
    public double MathNet_Qr() =>
        MultipleRegression.QR(_jagged, _response, intercept: true)[1];
}
