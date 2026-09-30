using Lodestar.Abstractions;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Xunit;
using DoubleMatrix = MathNet.Numerics.LinearAlgebra.Double.Matrix;

namespace Lodestar.Extensions.MathNet.Tests;

/// <summary>The Review B findings of <c>Lodestar.Extensions.MathNet</c> after #1562, one fact or theory each.</summary>
public sealed class ReviewBAfter1562Tests
{
    /// <summary><c>Array.MaxLength</c>, which netstandard2.0 does not declare: the most values one array holds.</summary>
    private const int ArrayMaxLength = 0x7FFFFFC7;

    /// <summary>A caller's storage reporting <paramref name="count"/> copies of one entry, yielded lazily so none is allocated.</summary>
    private sealed class RepeatingStorage(int count) : MatrixStorage<double>(1, 1)
    {
        public override bool IsDense => false;

        public override bool IsFullyMutable => false;

        public override bool IsMutableAt(int row, int column) => false;

        public override double At(int row, int column) => 1.0;

        public override void At(int row, int column, double value) => throw new NotSupportedException();

        public override IEnumerable<(int, int, double)> EnumerateNonZeroIndexed()
        {
            for (int i = 0; i < count; i++)
            {
                yield return (0, 0, 1.0);
            }
        }
    }

    private sealed class RepeatingMatrix(RepeatingStorage storage) : DoubleMatrix(storage);

    [Theory]
    [InlineData(new[] { 0, 3, 0 }, "must be non-decreasing", "The row pointers decrease at pointer 2, 0 after 3.")]
    [InlineData(new[] { 1, 0, 1 }, "must be 0", "The row pointers must start at 0, but start at 1.")]
    [InlineData(new[] { 0, 0, 0 }, "must end at", "The row pointers must end at the number of stored values, 1, but end at 0.")]
    public void Row_pointers_are_refused_in_the_constructors_order(int[] pointers, string constructor, string message)
    {
        // [0, 3, 0] read "but run from 0 to 0" where the constructor reads "must be non-decreasing" (#1571).
        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => new CsrMatrix(2, 2, [1.0], [0], (int[])pointers.Clone()));
        Assert.Contains(constructor, refused.Message, StringComparison.Ordinal);
        CsrMatrix matrix = CsrMatrix.CreateUnchecked(2, 2, [1.0], [0], pointers);

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToSparseMatrix(matrix));
        Assert.StartsWith(message, error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void A_tall_diagonal_matrix_holding_NaN_and_negative_zero_reads_as_its_dense_copy()
    {
        // Rows past the diagonal's length took a branch nothing reached (#1573); NaN is stored, -0.0 is not.
        var diagonal = new DiagonalMatrix(5, 3, [2.0, double.NaN, -0.0]);

        CsrMatrix fromDiagonal = MathNetInterop.ToCsrMatrix(diagonal);
        CsrMatrix fromDense = MathNetInterop.ToCsrMatrix(DenseMatrix.OfMatrix(diagonal));

        Assert.Equal((5, 3), (fromDiagonal.RowCount, fromDiagonal.ColumnCount));
        Assert.Equal([0, 1, 2, 2, 2, 2], fromDiagonal.RowPointers);
        Assert.Equal([0, 1], fromDiagonal.ColumnIndices);
        Assert.Equal(2.0, fromDiagonal.Values[0]);
        Assert.True(double.IsNaN(fromDiagonal.Values[1]));
        Assert.Equal(fromDense.RowPointers, fromDiagonal.RowPointers);
        Assert.Equal(fromDense.ColumnIndices, fromDiagonal.ColumnIndices);
        Assert.Equal(fromDense.Values, fromDiagonal.Values); // double.Equals holds NaN equal to NaN.
    }

    [Fact]
    public void A_custom_storage_reporting_more_values_than_one_array_holds_is_refused()
    {
        // Untested, and it refused Array.MaxLength values, which fit (#1574). The walk allocates nothing but yields
        // 0x7FFFFFC8 entries, about eight seconds per framework: the bound has no smaller stand-in to test against.
        var matrix = new RepeatingMatrix(new RepeatingStorage(ArrayMaxLength + 1));

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToCsrMatrix(matrix));
        Assert.StartsWith(
            "The matrix stores more non-zero values than one array holds (2147483591).", error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
    }
}
