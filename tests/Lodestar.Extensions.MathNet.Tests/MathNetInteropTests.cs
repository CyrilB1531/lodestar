using Lodestar.Abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Xunit;

namespace Lodestar.Extensions.MathNet.Tests;

/// <summary>
/// The bridge, judged on the two things a conversion owes: the same numbers in the
/// same places, and no array shared between the two sides afterwards.
/// </summary>
/// <remarks>
/// No oracle corpus: there is no Python call this maps to, and the arithmetic is a
/// copy. What is pinned instead is the invariant gap decision 0087 records —
/// <c>CsrMatrix</c> promises no column order, Math.NET searches the row, so the
/// unsorted and duplicate-column cases are facts here rather than assumptions.
/// </remarks>
public sealed class MathNetInteropTests
{
    /// <summary>A 3 x 4 matrix with an empty row, built the way a vectorizer builds one.</summary>
    private static CsrMatrix Sorted() => new(
        rowCount: 3,
        columnCount: 4,
        values: [1.5, 2.5, 3.5, 4.5],
        columnIndices: [0, 2, 1, 3],
        rowPointers: [0, 2, 2, 4]);

    /// <summary>
    /// One cell of a CSR matrix, read by scanning its row. Deliberately not
    /// <c>CsrMatrix.ToDense</c>: that returns a rectangular array, and CA1814 refuses one
    /// here for the reason it exists — a jagged array is the .NET default and the
    /// suppression belongs where the shape is forced, not in a test that has a choice.
    /// </summary>
    private static double CellOf(CsrMatrix matrix, int row, int column)
    {
        for (int k = matrix.RowPointers[row]; k < matrix.RowPointers[row + 1]; k++)
        {
            if (matrix.ColumnIndices[k] == column)
            {
                return matrix.Values[k];
            }
        }

        return 0.0;
    }

    /// <summary>Every stored value lands where the source has it, and nowhere else.</summary>
    [Fact]
    public void A_converted_matrix_reads_the_same_cell_by_cell()
    {
        CsrMatrix source = Sorted();

        SparseMatrix converted = MathNetInterop.ToSparseMatrix(source);

        Assert.Equal(source.RowCount, converted.RowCount);
        Assert.Equal(source.ColumnCount, converted.ColumnCount);
        for (int row = 0; row < source.RowCount; row++)
        {
            for (int column = 0; column < source.ColumnCount; column++)
            {
                Assert.Equal(CellOf(source, row, column), converted.At(row, column));
            }
        }
    }

    /// <summary>There and back, unchanged — shape, stored count and every value.</summary>
    [Fact]
    public void The_round_trip_changes_nothing()
    {
        CsrMatrix source = Sorted();

        CsrMatrix back = MathNetInterop.ToCsrMatrix(MathNetInterop.ToSparseMatrix(source));

        Assert.Equal(source.RowCount, back.RowCount);
        Assert.Equal(source.ColumnCount, back.ColumnCount);
        Assert.Equal(source.NonZeroCount, back.NonZeroCount);
        Assert.Equal(source.Values, back.Values);
        Assert.Equal(source.ColumnIndices, back.ColumnIndices);
        Assert.Equal(source.RowPointers, back.RowPointers);
    }

    /// <summary>
    /// The fact decision 0087 turns on. <c>CsrMatrix</c> validates four things and the
    /// order of column indices is not among them, so a hand-built matrix may hand over a
    /// row in any order — and Math.NET reaches a cell by searching that row. Without the
    /// sort this returns zeros for cells that hold values, silently.
    /// </summary>
    [Fact]
    public void A_row_whose_columns_are_out_of_order_converts_correctly()
    {
        var unsorted = new CsrMatrix(
            rowCount: 1,
            columnCount: 5,
            values: [9.0, 7.0, 8.0],
            columnIndices: [4, 0, 2],
            rowPointers: [0, 3]);

        SparseMatrix converted = MathNetInterop.ToSparseMatrix(unsorted);

        Assert.Equal(7.0, converted.At(0, 0));
        Assert.Equal(8.0, converted.At(0, 2));
        Assert.Equal(9.0, converted.At(0, 4));
        Assert.Equal(0.0, converted.At(0, 1));
        Assert.Equal(0.0, converted.At(0, 3));
    }

    /// <summary>
    /// A column stored twice in one row is added together, which is what a CSR consumer
    /// means by it and what Math.NET's own <c>NormalizeDuplicates</c> does. Left alone,
    /// only one of the two would ever be found.
    /// </summary>
    [Fact]
    public void Duplicate_columns_in_a_row_are_added_together()
    {
        var duplicated = new CsrMatrix(
            rowCount: 1,
            columnCount: 3,
            values: [2.0, 5.0, 1.0],
            columnIndices: [1, 1, 0],
            rowPointers: [0, 3]);

        SparseMatrix converted = MathNetInterop.ToSparseMatrix(duplicated);

        Assert.Equal(1.0, converted.At(0, 0));
        Assert.Equal(7.0, converted.At(0, 1));
        Assert.Equal(0.0, converted.At(0, 2));
    }

    /// <summary>
    /// A dense Math.NET matrix has none of the arrays the fast path copies, so it is
    /// walked instead — and its zeros must not become stored values.
    /// </summary>
    [Fact]
    public void A_dense_matrix_converts_and_keeps_only_its_non_zero_entries()
    {
        Matrix<double> dense = DenseMatrix.OfRowArrays(
            [0.0, 3.0, 0.0],
            [0.0, 0.0, 0.0],
            [4.0, 0.0, 5.0]);

        CsrMatrix converted = MathNetInterop.ToCsrMatrix(dense);

        Assert.Equal(3, converted.RowCount);
        Assert.Equal(3, converted.ColumnCount);
        Assert.Equal(3, converted.NonZeroCount);
        Assert.Equal([0, 1, 1, 3], converted.RowPointers);
        Assert.Equal([1, 0, 2], converted.ColumnIndices);
        Assert.Equal([3.0, 4.0, 5.0], converted.Values);
    }

    /// <summary>
    /// Neither side may hold the other's array: Math.NET's storage is mutable through
    /// <c>At</c>, and a shared buffer would let a write on one matrix move the other.
    /// </summary>
    [Fact]
    public void Neither_direction_leaves_the_two_matrices_sharing_an_array()
    {
        CsrMatrix source = Sorted();
        SparseMatrix converted = MathNetInterop.ToSparseMatrix(source);

        converted.At(0, 0, 99.0);
        Assert.Equal(1.5, source.Values[0]);

        CsrMatrix back = MathNetInterop.ToCsrMatrix(converted);
        back.Values[0] = -1.0;
        Assert.Equal(99.0, converted.At(0, 0));
    }

    /// <summary>A matrix with nothing stored is a shape, and the shape survives.</summary>
    [Fact]
    public void An_empty_matrix_survives_the_round_trip()
    {
        var empty = new CsrMatrix(2, 3, [], [], [0, 0, 0]);

        CsrMatrix back = MathNetInterop.ToCsrMatrix(MathNetInterop.ToSparseMatrix(empty));

        Assert.Equal(2, back.RowCount);
        Assert.Equal(3, back.ColumnCount);
        Assert.Equal(0, back.NonZeroCount);
    }

    /// <summary>Both entry points read their argument immediately, so neither takes null.</summary>
    [Fact]
    public void Both_directions_refuse_a_null_matrix()
    {
        Assert.Throws<ArgumentNullException>(() => MathNetInterop.ToSparseMatrix(null!));
        Assert.Throws<ArgumentNullException>(() => MathNetInterop.ToCsrMatrix(null!));
    }
}
