using BenchmarkDotNet.Attributes;
using Lodestar.Decomposition;
using NumFlat;
using NumFlat.MultivariateAnalyses;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// <see cref="PrincipalComponentVariance"/> against NumFlat's <c>PrincipalComponentAnalysis</c>,
/// the one .NET library that reports the eigenvalues (#701).
/// </summary>
/// <remarks>
/// Not like-for-like, and the asymmetry favours NumFlat's row: its constructor also forms the
/// eigenvectors and the mean, where this package computes the eigenvalues alone. Both answer the
/// same spectrum, checked to agree before either was timed; bench/README.md has the reading.
/// </remarks>
[MemoryDiagnoser]
public class PrincipalComponentVarianceBenchmarks
{
    private double[] _matrix = [];
    private Vec<double>[] _rows = [];
    private int _rowCount;
    private int _columnCount;

    /// <summary>Rows by columns: three tall blocks and one wide one.</summary>
    [Params("200x10", "2000x10", "2000x50", "100x200")]
    public string Shape { get; set; } = "200x10";

    [GlobalSetup]
    public void Setup()
    {
        (_rowCount, _columnCount) = PcaBlock.Shape(Shape);
        _matrix = PcaBlock.Matrix(_rowCount, _columnCount);

        _rows = new Vec<double>[_rowCount];
        for (int row = 0; row < _rowCount; row++)
        {
            _rows[row] = new Vec<double>(_matrix.AsSpan(row * _columnCount, _columnCount).ToArray());
        }
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_ExplainedVariance() =>
        PrincipalComponentVariance.Compute(_matrix, _rowCount, _columnCount).ExplainedVariance[0];

    [Benchmark]
    public double NumFlat_Pca() => new PrincipalComponentAnalysis(_rows).EigenValues[0];
}
