using Accord.Statistics.Testing;
using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// The four families with a .NET incumbent, against Accord.Statistics.Testing 3.8.0 (#1121).
/// </summary>
/// <remarks>
/// Names resolved out of the restored asset by reflection, the protocol bench/README.md §18 set.
/// Friedman has no row and no .NET incumbent at all, so its comparison is the cross-language one.
/// Accord's Anderson-Darling takes the hypothesised distribution rather than fitting one; handing
/// it a normal fitted to the same sample makes the statistics comparable, which
/// <see cref="MetaNumericsAgreement"/> checks before anything is timed.
/// </remarks>
[MemoryDiagnoser]
public class VarianceAndFitBenchmarks
{
    private double[][] _groups = [];
    private double[] _sample = [];
    private int _successes;

    /// <summary>How many values each group carries.</summary>
    [Params(100, 10_000)]
    public int GroupSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(1121);
        _groups =
        [
            [.. Enumerable.Range(0, GroupSize).Select(_ => random.NextDouble())],
            [.. Enumerable.Range(0, GroupSize).Select(_ => 2.0 * random.NextDouble())],
            [.. Enumerable.Range(0, GroupSize).Select(_ => 3.0 * random.NextDouble())],
        ];
        _sample = _groups[0];
        _successes = GroupSize / 3;

        CheckAgreement();
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Levene() => Levene.Test(_groups).PValue;

    [Benchmark]
    public double Accord_Levene() => new LeveneTest(_groups, median: true).PValue;

    [Benchmark]
    public double Lodestar_Bartlett() => Bartlett.Test(_groups).PValue;

    [Benchmark]
    public double Accord_Bartlett() => new BartlettTest(_groups).PValue;

    [Benchmark]
    public double Lodestar_Binomial() => Binomial.Test(_successes, GroupSize, 0.5).PValue;

    [Benchmark]
    public double Accord_Binomial() =>
        new BinomialTest(_successes, GroupSize, 0.5, OneSampleHypothesis.ValueIsDifferentFromHypothesis)
            .PValue;

    [Benchmark]
    public double Lodestar_AndersonDarling() => AndersonDarling.Test(_sample).Statistic;

    [Benchmark]
    public double Accord_AndersonDarling() =>
        new AndersonDarlingTest(_sample, FittedNormal(_sample)).Statistic;

    /// <summary>The Clopper-Pearson interval alone, which is where the beta inversion is.</summary>
    /// <remarks>
    /// Its own row because Accord exports no interval for a proportion: the p-value rows above
    /// compare two libraries, and this one measures the inversion against nothing but itself.
    /// </remarks>
    [Benchmark]
    public double Lodestar_ClopperPearson() =>
        Binomial.Test(_successes, GroupSize, 0.5).ProportionConfidenceInterval().Low;

    /// <summary>Accord's A², or <c>NaN</c> where Accord refuses to produce one.</summary>
    /// <remarks>
    /// Its constructor converts the statistic to a p-value eagerly, and at ten thousand values
    /// that conversion throws <c>InvalidOperationException: CCDF computation generated NaN
    /// values</c> from inside its own distribution. Catching it here keeps the other three pairs
    /// measurable at that size; the refusal is recorded rather than swallowed, and the
    /// <c>Accord_AndersonDarling</c> row reports NA for it.
    /// </remarks>
    private double AccordAndersonStatistic()
    {
        try
        {
            return new AndersonDarlingTest(_sample, FittedNormal(_sample)).Statistic;
        }
        catch (InvalidOperationException)
        {
            return double.NaN;
        }
    }

    private static Accord.Statistics.Distributions.Univariate.NormalDistribution FittedNormal(
        double[] sample)
    {
        double mean = sample.Average();
        double squares = sample.Sum(v => (v - mean) * (v - mean));
        return new Accord.Statistics.Distributions.Univariate.NormalDistribution(
            mean, Math.Sqrt(squares / (sample.Length - 1.0)));
    }

    private void CheckAgreement()
    {
        TestResult ourLevene = Levene.Test(_groups);
        var theirLevene = new LeveneTest(_groups, median: true);
        MetaNumericsAgreement.Record("levene statistic", ourLevene.Statistic, theirLevene.Statistic);
        MetaNumericsAgreement.Record("levene p", ourLevene.PValue, theirLevene.PValue);

        TestResult ourBartlett = Bartlett.Test(_groups);
        var theirBartlett = new BartlettTest(_groups);
        MetaNumericsAgreement.Record("bartlett statistic", ourBartlett.Statistic, theirBartlett.Statistic);
        MetaNumericsAgreement.Record("bartlett p", ourBartlett.PValue, theirBartlett.PValue);

        BinomialResult ourBinomial = Binomial.Test(_successes, GroupSize, 0.5);
        var theirBinomial = new BinomialTest(
            _successes, GroupSize, 0.5, OneSampleHypothesis.ValueIsDifferentFromHypothesis);
        MetaNumericsAgreement.Record("binomial p", ourBinomial.PValue, theirBinomial.PValue);

        AndersonResult ourAnderson = AndersonDarling.Test(_sample);
        MetaNumericsAgreement.Record(
            "anderson-darling statistic", ourAnderson.Statistic, AccordAndersonStatistic());

        MetaNumericsAgreement.Report($"Accord agreement at n={GroupSize}");
    }
}
