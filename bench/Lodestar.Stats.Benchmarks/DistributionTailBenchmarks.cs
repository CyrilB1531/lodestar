using BenchmarkDotNet.Attributes;

namespace Lodestar.Stats.Benchmarks;

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// The published chi-squared tail and normal quantile, one call per row, at the shapes the tests
/// hand them.
/// </summary>
/// <remarks>
/// No incumbent row: these time the incomplete gamma underneath <c>ChiSquare.Contingency</c>,
/// <c>KruskalWallis</c>, the Ljung-Box statistic and the normal tail, which StatsBenchmarks can
/// only see as part of a whole test. The arguments are the corpus's own
/// (tests/oracles/stats_distributions.json), and 4.41 where the corpus has no case.
/// </remarks>
[MemoryDiagnoser]
public class DistributionTailBenchmarks
{
    // Instance fields, not literals: with constant arguments the JIT folds a closed-form tail
    // into its answer (a 0.02 ns row, measured), and the row times nothing.
    private readonly double _statistic = 4.41;
    private readonly double _oneDf = 1.0;
    private readonly double _fourDfStatistic = 9.488;
    private readonly double _fourDf = 4.0;
    private readonly double _farTailStatistic = 120.0;
    private readonly double _threeDf = 3.0;
    private readonly double _hundred = 100.0;
    private readonly double _fractionalDf = 2.5;
    private readonly double _probability = 0.975;

    /// <summary>One degree of freedom: a 2x2 table, a log-rank test, a Wald z squared.</summary>
    [Benchmark(Baseline = true)]
    public double ChiSquaredSfOneDf() => Distributions.ChiSquaredSf(_statistic, _oneDf);

    /// <summary>An even degree of freedom, as a 3x3 table or a five-group Kruskal-Wallis has.</summary>
    [Benchmark]
    public double ChiSquaredSfFourDf() => Distributions.ChiSquaredSf(_fourDfStatistic, _fourDf);

    /// <summary>An odd degree of freedom in the far tail.</summary>
    [Benchmark]
    public double ChiSquaredSfThreeDfFarTail() => Distributions.ChiSquaredSf(_farTailStatistic, _threeDf);

    /// <summary>A hundred degrees of freedom near the mean, a long Ljung-Box lag window.</summary>
    [Benchmark]
    public double ChiSquaredSfHundredDf() => Distributions.ChiSquaredSf(_hundred, _hundred);

    /// <summary>A non-integer degree of freedom, which no closed form covers.</summary>
    [Benchmark]
    public double ChiSquaredSfFractionalDf() => Distributions.ChiSquaredSf(_statistic, _fractionalDf);

    /// <summary>The normal quantile, which inverts the normal tail.</summary>
    [Benchmark]
    public double NormalQuantile() => Distributions.NormalQuantile(_probability);
}
