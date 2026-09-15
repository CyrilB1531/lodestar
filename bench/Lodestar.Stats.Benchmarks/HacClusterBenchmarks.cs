using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>What HAC and one-way cluster covariances cost, against the ordinary fit and HC0 (#775).</summary>
/// <remarks>
/// A class of its own rather than two more values on <see cref="RobustCovarianceBenchmarks"/>, so that class's source
/// stays the same on <c>main</c> and on the branch and an A/B/A of it reads the shared pipeline alone. No free .NET
/// library computes either covariance, so the incumbent is <c>statsmodels</c> through <c>compare-ols</c>; this class
/// prices the allocations that harness does not see. Same seed and shape as <see cref="OlsBenchmarks"/>.
/// </remarks>
[MemoryDiagnoser]
public class HacClusterBenchmarks
{
    private const int Regressors = 4;

    /// <summary>Rows per cluster: consecutive blocks, as the <c>compare-ols</c> rows use.</summary>
    private const int ClusterSize = 20;

    private double[] _design = [];
    private double[] _response = [];
    private int[] _clusters = [];

    /// <summary>Rows.</summary>
    [Params(100, 10_000)]
    public int SampleSize { get; set; }

    /// <summary>Lags read by the HAC row; the cluster and baseline rows ignore it.</summary>
    [Params(4)]
    public int Lags { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(566);
        _design = new double[SampleSize * Regressors];
        _response = new double[SampleSize];
        _clusters = new int[SampleSize];

        for (int row = 0; row < SampleSize; row++)
        {
            double signal = 0.0;
            for (int column = 0; column < Regressors; column++)
            {
                double value = random.NextDouble();
                _design[(row * Regressors) + column] = value;
                signal += (column + 1) * value;
            }

            _response[row] = signal + (random.NextDouble() - 0.5);
            _clusters[row] = row / ClusterSize;
        }

        // Zero lags is HC0 by construction; a gap here would time a HAC that no longer is one.
        double white = Hc0();
        double lagless = OrdinaryLeastSquares.Fit(
            _design, _response, Regressors, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 0 }).StandardErrors[1];
        if (!(Math.Abs(white - lagless) <= 1e-12 * Math.Abs(white)))
        {
            throw new InvalidOperationException($"HAC with no lags gives {lagless:R}, HC0 gives {white:R}.");
        }
    }

    [Benchmark(Baseline = true)]
    public double Nonrobust() =>
        OrdinaryLeastSquares.Fit(_design, _response, Regressors).StandardErrors[1];

    [Benchmark]
    public double Hc0() =>
        OrdinaryLeastSquares.Fit(
            _design, _response, Regressors, new OlsOptions { CovarianceType = CovarianceType.Hc0 }).StandardErrors[1];

    [Benchmark]
    public double Hac() =>
        OrdinaryLeastSquares.Fit(
            _design, _response, Regressors, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = Lags }).StandardErrors[1];

    [Benchmark]
    public double Cluster() =>
        OrdinaryLeastSquares.Fit(
            _design, _response, _clusters, Regressors, new OlsOptions { CovarianceType = CovarianceType.Cluster }).StandardErrors[1];
}
