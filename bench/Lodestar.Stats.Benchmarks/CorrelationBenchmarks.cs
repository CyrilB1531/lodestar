using BenchmarkDotNet.Attributes;
using Lodestar.Stats;
using MetaStatistics = Meta.Numerics.Statistics;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// The three correlation tests against Meta.Numerics 4.2.0's <c>Bivariate</c> (#1120).
/// </summary>
/// <remarks>
/// Names resolved out of the restored <c>netstandard2.0</c> asset by reflection rather than
/// guessed, the protocol bench/README.md §18 set. The corpus is drawn from a continuous
/// generator, so nothing is tied — the only ground the two libraries share, since Meta.Numerics
/// averages no tied rank and computes tau-a. bench/README.md has the measurement.
/// <see cref="MetaNumericsAgreement"/> asserts the three statistics before anything is timed.
/// </remarks>
[MemoryDiagnoser]
public class CorrelationBenchmarks
{
    private double[] _predictor = [];
    private double[] _response = [];

    /// <summary>How many pairs each correlation reads.</summary>
    [Params(100, 10_000)]
    public int PairCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(1120);
        _predictor = new double[PairCount];
        _response = new double[PairCount];
        for (int i = 0; i < PairCount; i++)
        {
            double value = random.NextDouble();
            _predictor[i] = value;

            // Correlated but not collinear: a perfect relationship would send every p-value to
            // zero and make the agreement check pass on a degenerate case.
            _response[i] = (2.0 * value) + (0.3 * random.NextDouble());
        }

        CheckAgreement();
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Pearson() => Pearson.Test(_predictor, _response).PValue;

    [Benchmark]
    public double MetaNumerics_Pearson() =>
        MetaStatistics.Bivariate.PearsonRTest(_predictor, _response).Probability;

    [Benchmark]
    public double Lodestar_Spearman() => Spearman.Test(_predictor, _response).PValue;

    [Benchmark]
    public double MetaNumerics_Spearman() =>
        MetaStatistics.Bivariate.SpearmanRhoTest(_predictor, _response).Probability;

    [Benchmark]
    public double Lodestar_KendallTau() => KendallTau.Test(_predictor, _response).PValue;

    [Benchmark]
    public double MetaNumerics_KendallTau() =>
        MetaStatistics.Bivariate.KendallTauTest(_predictor, _response).Probability;

    /// <summary>The Kendall count alone, which is where the two orders of growth part.</summary>
    /// <remarks>
    /// <see cref="ExactMethod.Asymptotic"/> so the row times the concordance count rather than the
    /// exact table, which <see cref="ExactMethod.Auto"/> would build at a hundred pairs and not at
    /// ten thousand — two different amounts of work under one name.
    /// </remarks>
    [Benchmark]
    public double Lodestar_KendallTau_Asymptotic() =>
        KendallTau.Test(_predictor, _response, method: ExactMethod.Asymptotic).PValue;

    private void CheckAgreement()
    {
        PearsonResult ourR = Pearson.Test(_predictor, _response);
        MetaStatistics.TestResult theirR =
            MetaStatistics.Bivariate.PearsonRTest(_predictor, _response);
        MetaNumericsAgreement.Require(
            "pearson", ourR.Statistic, theirR.Statistic.Value, ourR.PValue, theirR.Probability);

        TestResult ourRho = Spearman.Test(_predictor, _response);
        MetaStatistics.TestResult theirRho =
            MetaStatistics.Bivariate.SpearmanRhoTest(_predictor, _response);
        MetaNumericsAgreement.Require(
            "spearman", ourRho.Statistic, theirRho.Statistic.Value, ourRho.PValue, theirRho.Probability);

        TestResult ourTau = KendallTau.Test(_predictor, _response);
        MetaStatistics.TestResult theirTau =
            MetaStatistics.Bivariate.KendallTauTest(_predictor, _response);
        MetaNumericsAgreement.Require(
            "kendall tau", ourTau.Statistic, theirTau.Statistic.Value, ourTau.PValue, theirTau.Probability);

        MetaNumericsAgreement.Report($"Meta.Numerics correlation agreement at n={PairCount}");
    }
}
