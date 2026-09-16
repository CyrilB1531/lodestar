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

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no
    // security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Setup()
    {
        string[] parts = Shape.Split('x');
        _rowCount = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        _columnCount = int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

        // Each column scaled by its index, so the spectrum is spread rather than flat.
        var random = new Random(701);
        _matrix = new double[_rowCount * _columnCount];
        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = random.NextDouble() * (1 + (i % _columnCount));
        }

        _rows = new Vec<double>[_rowCount];
        for (int row = 0; row < _rowCount; row++)
        {
            _rows[row] = new Vec<double>(_matrix.AsSpan(row * _columnCount, _columnCount).ToArray());
        }
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public double Lodestar_ExplainedVariance() =>
        PrincipalComponentVariance.Compute(_matrix, _rowCount, _columnCount).ExplainedVariance[0];

    [Benchmark]
    public double NumFlat_Pca() => new PrincipalComponentAnalysis(_rows).EigenValues[0];
}
