using Lodestar.Abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using Xunit;
using DoubleMatrix = MathNet.Numerics.LinearAlgebra.Double.Matrix;

namespace Lodestar.Extensions.MathNet.Tests;

/// <summary>The Review B findings of <c>Lodestar.Extensions.MathNet</c> after #1531, one fact or theory each.</summary>
public sealed class ReviewBAfter1531Tests
{
    /// <summary>
    /// A caller's own storage: it enumerates <see cref="Entries"/> as given, in that order, and answers
    /// <c>At</c> from them. <see cref="SecondWalk"/>, when set, is what it enumerates from the second walk on.
    /// </summary>
    private sealed class ListStorage((int Row, int Column, double Value)[] entries, int rows, int columns)
        : MatrixStorage<double>(rows, columns)
    {
        private int _walks;

        public (int Row, int Column, double Value)[] Entries { get; } = entries;

        public (int Row, int Column, double Value)[]? SecondWalk { get; init; }

        public override bool IsDense => false;

        public override bool IsFullyMutable => false;

        public override bool IsMutableAt(int row, int column) => false;

        public override double At(int row, int column)
        {
            double sum = 0.0;
            foreach ((int r, int c, double v) in Entries)
            {
                if (r == row && c == column)
                {
                    sum += v;
                }
            }

            return sum;
        }

        public override void At(int row, int column, double value) => throw new NotSupportedException();

        public override IEnumerable<(int, int, double)> EnumerateNonZeroIndexed()
        {
            (int Row, int Column, double Value)[] walk = _walks++ > 0 && SecondWalk is not null ? SecondWalk : Entries;
            foreach ((int row, int column, double value) in walk)
            {
                yield return (row, column, value);
            }
        }
    }

    /// <summary>A caller's own matrix type over <see cref="ListStorage"/>: <c>Matrix.Build.OfStorage</c> refuses a storage it does not know.</summary>
    private sealed class ListMatrix(ListStorage storage) : DoubleMatrix(storage);

    private static double CellOf(CsrMatrix matrix, int row, int column)
    {
        double sum = 0.0;
        for (int k = matrix.RowPointers[row]; k < matrix.RowPointers[row + 1]; k++)
        {
            if (matrix.ColumnIndices[k] == column)
            {
                sum += matrix.Values[k];
            }
        }

        return sum;
    }

    [Fact]
    public void A_custom_storage_enumerating_out_of_row_order_converts()
    {
        // #1530 promised it and nothing tested it (#1551).
        var storage = new ListStorage([(2, 1, 5.0), (0, 2, 3.0), (2, 0, 4.0), (0, 0, 1.0), (1, 1, 0.0)], 3, 3);

        CsrMatrix csr = MathNetInterop.ToCsrMatrix(new ListMatrix(storage));

        Assert.Equal([0, 2, 2, 4], csr.RowPointers);
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                Assert.Equal(storage.At(row, column), CellOf(csr, row, column));
            }
        }
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(-1, 0)]
    [InlineData(0, 3)]
    [InlineData(0, -1)]
    public void A_custom_storage_reporting_an_entry_outside_its_shape_is_refused(int row, int column)
    {
        // An IndexOutOfRangeException escaped, or a column past the matrix was accepted (#1547).
        var storage = new ListStorage([(0, 0, 1.0), (row, column, 2.0)], 3, 3);

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MathNetInterop.ToCsrMatrix(new ListMatrix(storage)));
        Assert.Equal("matrix", error.ParamName);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void A_custom_storage_whose_two_walks_differ_is_refused(bool moreOnTheSecond)
    {
        (int, int, double)[] first = [(0, 0, 1.0), (1, 1, 2.0)];
        (int, int, double)[] second = moreOnTheSecond ? [(0, 0, 1.0), (1, 1, 2.0), (1, 0, 3.0)] : [(0, 0, 1.0)];
        var storage = new ListStorage(first, 2, 2) { SecondWalk = second };

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MathNetInterop.ToCsrMatrix(new ListMatrix(storage)));
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void A_long_row_sums_a_repeated_column_in_stored_order()
    {
        // Math.NET's sort is unstable past 16 entries: on main this row read 1 in column 0, where ToDense reads 0 (#1548).
        int[] columns = [9, 15, 19, 14, 0, 6, 11, 1, 9, 8, 4, 22, 20, 13, 17, 16, 0, 18, 5, 5, 3, 23, 0, 13, 6];
        double[] values = [.. Enumerable.Repeat(0.5, columns.Length)];
        values[4] = 1e16;
        values[16] = 1.0;
        values[22] = -1e16;
        var source = new CsrMatrix(1, columns.Length, values, columns, [0, columns.Length]);

        SparseMatrix sparse = MathNetInterop.ToSparseMatrix(source);

        double[,] dense = source.ToDense();
        for (int column = 0; column < columns.Length; column++)
        {
            Assert.Equal(dense[0, column], sparse[0, column]);
        }

        Assert.Equal(0.0, sparse[0, 0]);
    }

    [Fact]
    public void A_lone_negative_zero_in_an_unordered_row_keeps_its_sign()
    {
        SparseMatrix sparse = MathNetInterop.ToSparseMatrix(new CsrMatrix(1, 2, [1.0, -0.0], [1, 0], [0, 2]));

        Assert.Equal(double.NegativeInfinity, 1.0 / sparse[0, 0]);
        Assert.Equal(1.0, sparse[0, 1]);
    }

    public static TheoryData<int[], int[]> InvalidStructures => new()
    {
        { [0, 0], [0] },
        { [1, 1], [0] },
        { [0, 1], [2] },
        { [0, 1], [-1] },
    };

    [Theory]
    [MemberData(nameof(InvalidStructures))]
    public void An_invalid_unchecked_matrix_is_refused_before_Math_NET_sees_it(int[] pointers, int[] columns)
    {
        // Math.NET threw a plain Exception, or kept a column outside the matrix (#1549).
        CsrMatrix matrix = CsrMatrix.CreateUnchecked(1, 2, new double[columns.Length], columns, pointers);

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToSparseMatrix(matrix));
        Assert.Equal("matrix", error.ParamName);
    }

    [Fact]
    public void Decreasing_row_pointers_are_refused()
    {
        CsrMatrix matrix = CsrMatrix.CreateUnchecked(2, 2, [1.0], [0], [0, 1, 1]);
        matrix.RowPointers[1] = 2;

        ArgumentException error = Assert.Throws<ArgumentException>(() => MathNetInterop.ToSparseMatrix(matrix));
        Assert.Equal("matrix", error.ParamName);
    }
}
