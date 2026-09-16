using BenchmarkDotNet.Attributes;
using Lodestar.Decomposition;
using MetaStatistics = Meta.Numerics.Statistics;

namespace Lodestar.Text.Benchmarks;

// CA1822: BenchmarkDotNet rejects static benchmarks — see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// <see cref="PrincipalComponentVariance"/> against Meta.Numerics 4.2.0, the incumbent that exists
/// below <c>net8.0</c> (#756).
/// </summary>
/// <remarks>
/// Its own class for two reasons that are both findings. <strong>Meta.Numerics refuses a wide
/// matrix</strong> — 100 rows by 200 features raises <c>InsufficientDataException</c> where this
/// package and NumFlat answer — so the shapes are the three it accepts; and it takes the columns
/// where they take the rows, a transposition <c>GlobalSetup</c> does outside the timed region.
/// </remarks>
[MemoryDiagnoser]
public class MetaNumericsPcaBenchmarks
{
    private double[] _matrix = [];
    private double[][] _columns = [];
    private int _rowCount;
    private int _columnCount;

    /// <summary>Rows by columns — the three of <see cref="PrincipalComponentVarianceBenchmarks"/>' shapes Meta.Numerics accepts.</summary>
    [Params("200x10", "2000x10", "2000x50")]
    public string Shape { get; set; } = "200x10";

    [GlobalSetup]
    public void Setup()
    {
        (_rowCount, _columnCount) = PcaBlock.Shape(Shape);
        _matrix = PcaBlock.Matrix(_rowCount, _columnCount);
        _columns = PcaBlock.Columns(_matrix, _rowCount, _columnCount);

        RequireSameSpectrum();
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_ExplainedVarianceRatio() =>
        PrincipalComponentVariance.Compute(_matrix, _rowCount, _columnCount).ExplainedVarianceRatio[0];

    [Benchmark]
    public double MetaNumerics_VarianceFraction() =>
        MetaStatistics.Multivariate.PrincipalComponentAnalysis(_columns).Components[0].VarianceFraction;

    /// <summary>
    /// The two ratios, compared before either is timed: an eigenvalue is scaled differently by each
    /// library, and the fraction of variance a component explains is not.
    /// </summary>
    private void RequireSameSpectrum()
    {
        double ours = PrincipalComponentVariance.Compute(_matrix, _rowCount, _columnCount).ExplainedVarianceRatio[0];
        double theirs = MetaStatistics.Multivariate.PrincipalComponentAnalysis(_columns).Components[0].VarianceFraction;

        if (Math.Abs(ours - theirs) > 1e-9 * Math.Max(1.0, Math.Abs(ours)))
        {
            throw new InvalidOperationException(
                $"{Shape}: Meta.Numerics explains {theirs} of the variance where this package explains {ours}. "
                + "Timing two different spectra under one name would say nothing.");
        }
    }
}
