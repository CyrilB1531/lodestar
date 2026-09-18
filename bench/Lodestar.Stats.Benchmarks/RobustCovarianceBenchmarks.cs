using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>What a heteroskedasticity-consistent covariance costs, against the ordinary one.</summary>
/// <remarks>
/// Not a parameter on <see cref="OlsBenchmarks"/>: Accord.Statistics has no robust mode, so
/// five values there would multiply a row that cannot move. Here only the estimator varies,
/// which is what makes the ratio column read as the cost of choosing one. HC2 and HC3 also
/// walk Q for the leverages, so a flat result and a stepped one say different things about
/// where the time goes (#705).
/// </remarks>
[MemoryDiagnoser]
public class RobustCovarianceBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];
    private OlsOptions _options = new();

    /// <summary>Rows. Four regressors throughout, matching OlsBenchmarks beside it.</summary>
    [Params(100, 10_000)]
    public int SampleSize { get; set; }

    /// <summary>The estimator under test; Nonrobust is the baseline the ratio is read against.</summary>
    [Params(
        CovarianceType.Nonrobust,
        CovarianceType.Hc0,
        CovarianceType.Hc1,
        CovarianceType.Hc2,
        CovarianceType.Hc3)]
    public CovarianceType Covariance { get; set; }

    private const int Regressors = 4;

    [GlobalSetup]
    public void Setup()
    {
        // The same seed and the same shape as OlsBenchmarks, so the two classes price the
        // same fit and their numbers can be read side by side.
        (_design, _response, _) = RegressionCorpus.Build(
            566, SampleSize, Regressors, value => value, (column, value) => (column + 1) * value);

        _options = new OlsOptions { CovarianceType = Covariance };
    }

    [Benchmark]
    public double Fit() =>
        OrdinaryLeastSquares.Fit(_design, _response, Regressors, _options).PValues[1];
}
