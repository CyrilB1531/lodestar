using Lodestar.Abstractions;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Xunit;

namespace Lodestar.Extensions.MathNet.Tests;

/// <summary>
/// The Review B finding of <c>Lodestar.Extensions.MathNet</c> after #1607 (#1608): each way a caller can leave a
/// compressed-row storage inconsistent, refused under <c>matrix</c>, and the two unusual storages still accepted.
/// </summary>
public sealed class ReviewBAfter1607Tests
{
    /// <summary>
    /// A 2 × 2 sparse matrix storing 1 at (0, 0) and 2 at (1, 1), and its compressed-row storage to damage, its two
    /// arrays trimmed to the values it stores where Math.NET leaves spare capacity.
    /// </summary>
    private static (SparseMatrix Matrix, SparseCompressedRowMatrixStorage<double> Storage) TwoStored()
    {
        var matrix = new SparseMatrix(2, 2);
        matrix[0, 0] = 1.0;
        matrix[1, 1] = 2.0;
        var storage = (SparseCompressedRowMatrixStorage<double>)matrix.Storage;
        storage.Values = [1.0, 2.0];
        storage.ColumnIndices = [0, 1];
        return (matrix, storage);
    }

    [Theory]
    [InlineData(0, 1, "The row pointers must start at 0, but start at 1.")]
    [InlineData(1, 3, "The row pointers decrease at pointer 2, 2 after 3.")]
    [InlineData(2, -1, "The row pointers decrease at pointer 2, -1 after 1.")]
    [InlineData(2, 3, "The row pointers end at 3, past the storage's values, of length 2.")]
    public void A_compressed_row_storage_with_changed_pointers_is_refused_under_matrix(int index, int value, string message)
    {
        // A last pointer of -1 threw OverflowException, the others ArgumentException under an internal name (#1608).
        // Ending at 3 overruns both arrays, of length 2: the values are named, checked first.
        (SparseMatrix matrix, SparseCompressedRowMatrixStorage<double> storage) = TwoStored();
        storage.RowPointers[index] = value;

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToCsrMatrix(matrix));
        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void A_compressed_row_storage_short_of_column_indices_alone_is_refused_under_matrix()
    {
        (SparseMatrix matrix, SparseCompressedRowMatrixStorage<double> storage) = TwoStored();
        storage.ColumnIndices = [0];

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToCsrMatrix(matrix));
        Assert.StartsWith(
            "The row pointers end at 2, past the storage's column indices, of length 1.", error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }

    [Theory]
    [InlineData(true, "values")]
    [InlineData(false, "column indices")]
    public void A_compressed_row_storage_with_a_null_array_under_stored_pointers_is_refused_under_matrix(bool nullValues, string array)
    {
        (SparseMatrix matrix, SparseCompressedRowMatrixStorage<double> storage) = TwoStored();
        if (nullValues)
        {
            storage.Values = null!;
        }
        else
        {
            storage.ColumnIndices = null!;
        }

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToCsrMatrix(matrix));
        Assert.StartsWith($"The row pointers end at 2, but the storage's {array} are null.", error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void A_compressed_row_storage_with_a_column_outside_the_matrix_is_refused_under_matrix()
    {
        (SparseMatrix matrix, SparseCompressedRowMatrixStorage<double> storage) = TwoStored();
        storage.ColumnIndices[1] = 5;

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToCsrMatrix(matrix));
        Assert.StartsWith("Stored value 1 sits in column 5, outside [0, 2).", error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void A_compressed_row_storage_whose_arrays_are_null_converts_when_it_stores_nothing()
    {
        var matrix = new SparseMatrix(2, 3);
        var storage = (SparseCompressedRowMatrixStorage<double>)matrix.Storage;
        storage.Values = null!;
        storage.ColumnIndices = null!;

        CsrMatrix converted = MathNetInterop.ToCsrMatrix(matrix);

        Assert.Equal((2, 3, 0), (converted.RowCount, converted.ColumnCount, converted.NonZeroCount));
        Assert.Equal([0, 0, 0], converted.RowPointers);
    }

    [Fact]
    public void A_compressed_row_storage_with_spare_capacity_copies_only_what_it_stores()
    {
        (SparseMatrix matrix, SparseCompressedRowMatrixStorage<double> storage) = TwoStored();
        storage.Values = [1.0, 2.0, 9.0, 9.0];
        storage.ColumnIndices = [0, 1, 7, 7];

        CsrMatrix converted = MathNetInterop.ToCsrMatrix(matrix);

        Assert.Equal([1.0, 2.0], converted.Values);
        Assert.Equal([0, 1], converted.ColumnIndices);
        Assert.Equal([0, 1, 2], converted.RowPointers);
    }
}
