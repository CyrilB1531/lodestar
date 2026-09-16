using BenchmarkDotNet.Attributes;
using Lodestar.Stats;
using MetaStatistics = Meta.Numerics.Statistics;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// Eight families against Meta.Numerics 4.2.0, which is MS-PL, maintained, and ships
/// <c>netstandard2.0</c> — where the only other incumbent here is archived (#756).
/// </summary>
/// <remarks>
/// <c>Univariate.StudentTTest</c> is the <strong>pooled</strong> two-sample t, so this races
/// <c>Variance.Equal</c> rather than the Welch default: the same care <see cref="StatsBenchmarks"/>
/// takes with Accord's Yates correction. Every pair's statistic is checked before anything is timed
/// and the p-values that differ are printed — bench/README.md §46 has what they were.
/// </remarks>
[MemoryDiagnoser]
public class MetaNumericsStatsBenchmarks
{
    private double[] _a = [];
    private double[] _b = [];
    private double[] _paired = [];
    private double[][] _groups = [];
    private double[][] _table = [];
    private int[][] _counts = [];
    private int[][] _grid = [];

    /// <summary>How many values each sample carries.</summary>
    [Params(100, 10_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(756);
        _a = [.. Enumerable.Range(0, SampleSize).Select(_ => random.NextDouble())];
        _b = [.. Enumerable.Range(0, SampleSize).Select(_ => random.NextDouble() + 0.1)];

        // Drawn from a continuous generator, so there are no ties: the rank tests' tie
        // corrections never fire and "agrees" is not a claim about tied data.
        _paired = [.. _a.Select((value, index) => value + (0.05 * ((index % 7) - 3)))];
        _groups =
        [
            _a,
            _b,
            [.. Enumerable.Range(0, SampleSize).Select(_ => random.NextDouble() + 0.2)],
        ];

        _table = [[30.0, 20.0], [15.0, 35.0]];
        _counts = [[30, 20], [15, 35]];
        _grid = [[30, 20], [15, 35]];

        CheckAgreement();
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_StudentT() =>
        TTest.Independent(_a, _b, variance: Variance.Equal).PValue;

    [Benchmark]
    public double MetaNumerics_StudentT() => MetaStatistics.Univariate.StudentTTest(_a, _b).Probability;

    [Benchmark]
    public double Lodestar_MannWhitney() => MannWhitney.Test(_a, _b).PValue;

    [Benchmark]
    public double MetaNumerics_MannWhitney() => MetaStatistics.Univariate.MannWhitneyTest(_a, _b).Probability;

    [Benchmark]
    public double Lodestar_KruskalWallis() => KruskalWallis.Test(_groups).PValue;

    [Benchmark]
    public double MetaNumerics_KruskalWallis() =>
        MetaStatistics.Univariate.KruskalWallisTest(_groups).Probability;

    [Benchmark]
    public double Lodestar_KolmogorovSmirnov() => KolmogorovSmirnov.TwoSample(_a, _b).PValue;

    [Benchmark]
    public double MetaNumerics_KolmogorovSmirnov() =>
        MetaStatistics.Univariate.KolmogorovSmirnovTest(_a, _b).Probability;

    [Benchmark]
    public double Lodestar_OneWayAnova() => OneWayAnova.Test(_groups).PValue;

    [Benchmark]
    public double MetaNumerics_OneWayAnova() =>
        MetaStatistics.Univariate.OneWayAnovaTest(_groups).Result.Probability;

    [Benchmark]
    public double Lodestar_Wilcoxon() => Wilcoxon.Paired(_a, _paired).PValue;

    [Benchmark]
    public double MetaNumerics_Wilcoxon() =>
        MetaStatistics.Bivariate.WilcoxonSignedRankTest(_a, _paired).Probability;

    /// <summary>Fixed at a 2×2 table: neither side's cost depends on <see cref="SampleSize"/> here.</summary>
    [Benchmark]
    public double Lodestar_FisherExact() => FisherExact.Test(_counts).PValue;

    [Benchmark]
    public double MetaNumerics_FisherExact() =>
        Table(_grid).Binary.FisherExactTest().Probability;

    [Benchmark]
    public double Lodestar_ChiSquare() => ChiSquare.Contingency(_table).PValue;

    [Benchmark]
    public double MetaNumerics_ChiSquare() =>
        Table(_grid).PearsonChiSquaredTest().Probability;

    /// <summary>The counts as Meta.Numerics takes them: a rectangular array, which CA1814 forbids as a field.</summary>
    /// <remarks>Built per call, and that build is inside the timed region on purpose — it is what a caller pays.</remarks>
    private static MetaStatistics.ContingencyTable Table(int[][] counts)
    {
        // CA1814: the jagged array it asks for is what this converts *from*. Meta.Numerics'
        // constructor takes int[,], so the rectangular array is the library's shape, not a choice.
#pragma warning disable CA1814
        var grid = new int[counts.Length, counts[0].Length];
#pragma warning restore CA1814
        for (int row = 0; row < counts.Length; row++)
        {
            for (int column = 0; column < counts[row].Length; column++)
            {
                grid[row, column] = counts[row][column];
            }
        }

        return new MetaStatistics.ContingencyTable(grid);
    }

    /// <summary>Every pair run once, outside the timed region, before any of them is timed.</summary>
    private void CheckAgreement()
    {
        TTestResult ourT = TTest.Independent(_a, _b, variance: Variance.Equal);
        MetaStatistics.TestResult theirT = MetaStatistics.Univariate.StudentTTest(_a, _b);
        MetaNumericsAgreement.Require("student t", ourT.Statistic, theirT.Statistic.Value, ourT.PValue, theirT.Probability);

        TestResult ourU = MannWhitney.Test(_a, _b);
        MetaStatistics.TestResult theirU = MetaStatistics.Univariate.MannWhitneyTest(_a, _b);
        MetaNumericsAgreement.Record("mann-whitney statistic", ourU.Statistic, theirU.Statistic.Value);
        MetaNumericsAgreement.Record("mann-whitney p", ourU.PValue, theirU.Probability);

        TestResult ourH = KruskalWallis.Test(_groups);
        MetaStatistics.TestResult theirH = MetaStatistics.Univariate.KruskalWallisTest(_groups);
        MetaNumericsAgreement.Require("kruskal-wallis", ourH.Statistic, theirH.Statistic.Value, ourH.PValue, theirH.Probability);

        KsResult ourKs = KolmogorovSmirnov.TwoSample(_a, _b);
        MetaStatistics.TestResult theirKs = MetaStatistics.Univariate.KolmogorovSmirnovTest(_a, _b);
        MetaNumericsAgreement.Require("kolmogorov-smirnov", ourKs.Statistic, theirKs.Statistic.Value, ourKs.PValue, theirKs.Probability);

        TestResult ourF = OneWayAnova.Test(_groups);
        MetaStatistics.TestResult theirF = MetaStatistics.Univariate.OneWayAnovaTest(_groups).Result;
        MetaNumericsAgreement.Require("one-way anova", ourF.Statistic, theirF.Statistic.Value, ourF.PValue, theirF.Probability);

        TestResult ourW = Wilcoxon.Paired(_a, _paired);
        MetaStatistics.TestResult theirW = MetaStatistics.Bivariate.WilcoxonSignedRankTest(_a, _paired);
        MetaNumericsAgreement.Record("wilcoxon statistic", ourW.Statistic, theirW.Statistic.Value);
        MetaNumericsAgreement.Record("wilcoxon p", ourW.PValue, theirW.Probability);

        TestResult ourFisher = FisherExact.Test(_counts);
        MetaStatistics.TestResult theirFisher = Table(_grid).Binary.FisherExactTest();
        MetaNumericsAgreement.Record("fisher exact p", ourFisher.PValue, theirFisher.Probability);

        Chi2ContingencyResult ourChi = ChiSquare.Contingency(_table);
        MetaStatistics.TestResult theirChi = Table(_grid).PearsonChiSquaredTest();
        MetaNumericsAgreement.Record("chi-square statistic", ourChi.Statistic, theirChi.Statistic.Value);
        MetaNumericsAgreement.Record("chi-square p", ourChi.PValue, theirChi.Probability);

        MetaNumericsAgreement.Report($"Meta.Numerics agreement at n={SampleSize}");
    }
}
