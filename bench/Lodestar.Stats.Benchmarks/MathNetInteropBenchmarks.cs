using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary><c>MathNetInterop</c> both ways: from a diagonal and a mostly-zero dense matrix, and to Math.NET from rows sorted or not.</summary>
/// <remarks>
/// No incumbent: Math.NET has no CSR type of Lodestar's to convert to, so the package against itself, A/B (#1220, #1402).
/// The diagonal is the case the per-cell walk made quadratic; the reversed rows are the ones Math.NET must sort.
/// </remarks>
[MemoryDiagnoser]
public class MathNetInteropBenchmarks
{
    private Matrix<double> _diagonal = null!;
    private Matrix<double> _dense = null!;
    private CsrMatrix _sorted = null!;
    private CsrMatrix _reversed = null!;

    /// <summary>The side of both matrices.</summary>
    [Params(1_000, 4_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(20260929);
        _diagonal = DiagonalMatrix.Create(Size, Size, i => i + 1.0);
        _dense = DenseMatrix.Create(Size, Size, (_, _) => rng.NextDouble() < 0.01 ? rng.NextDouble() : 0.0);
        _sorted = MathNetInterop.ToCsrMatrix(_dense);
        _reversed = Reversed(_sorted);
    }

    [Benchmark]
    public int Diagonal() => MathNetInterop.ToCsrMatrix(_diagonal).NonZeroCount;

    [Benchmark]
    public int SparseDense() => MathNetInterop.ToCsrMatrix(_dense).NonZeroCount;

    [Benchmark]
    public int ToSparseSorted() => MathNetInterop.ToSparseMatrix(_sorted).NonZerosCount;

    [Benchmark]
    public int ToSparseReversed() => MathNetInterop.ToSparseMatrix(_reversed).NonZerosCount;

    /// <summary>The same matrix with every row's entries in descending column order.</summary>
    private static CsrMatrix Reversed(CsrMatrix matrix)
    {
        var values = (double[])matrix.Values.Clone();
        var columns = (int[])matrix.ColumnIndices.Clone();
        for (int row = 0; row < matrix.RowCount; row++)
        {
            int start = matrix.RowPointers[row];
            int length = matrix.RowPointers[row + 1] - start;
            Array.Reverse(values, start, length);
            Array.Reverse(columns, start, length);
        }

        return new CsrMatrix(matrix.RowCount, matrix.ColumnCount, values, columns, (int[])matrix.RowPointers.Clone());
    }
}
