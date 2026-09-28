using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>The surface as it moved: same behaviour, new namespace.</summary>
public sealed class CsrMatrixTests
{
    // [[1, 0, 2], [0, 3, 0]] — one row with a gap, one with a single entry.
    private static CsrMatrix Sample() =>
        new(2, 3, [1.0, 2.0, 3.0], [0, 2, 1], [0, 2, 3]);

    [Fact]
    public void The_dense_form_puts_every_value_back_where_it_came_from()
    {
        double[,] dense = Sample().ToDense();

        Assert.Equal(1.0, dense[0, 0]);
        Assert.Equal(0.0, dense[0, 1]);
        Assert.Equal(2.0, dense[0, 2]);
        Assert.Equal(3.0, dense[1, 1]);
    }

    /// <summary>
    /// The three arrays are public and have been since 0.1.0, so they are part of the
    /// contract that moved rather than an implementation detail behind it.
    /// </summary>
    [Fact]
    public void The_raw_arrays_are_the_ones_they_were_built_from()
    {
        CsrMatrix matrix = Sample();

        Assert.Equal([1.0, 2.0, 3.0], matrix.Values);
        Assert.Equal([0, 2, 1], matrix.ColumnIndices);
        Assert.Equal([0, 2, 3], matrix.RowPointers);
        Assert.Equal(3, matrix.NonZeroCount);
        Assert.Equal(2, matrix.RowCount);
        Assert.Equal(3, matrix.ColumnCount);
    }

    [Fact]
    public void Row_norms_read_only_the_row_they_name()
    {
        CsrMatrix matrix = Sample();

        Assert.Equal(3.0, matrix.RowL1Norm(0));
        Assert.Equal(Math.Sqrt(5.0), matrix.RowL2Norm(0), 1e-12);
        Assert.Equal(3.0, matrix.RowL2Norm(1));
    }

    [Fact]
    public void Normalizing_by_L2_leaves_every_row_of_unit_length()
    {
        CsrMatrix matrix = Sample();

        matrix.NormalizeRows(SparseNorm.L2);

        Assert.Equal(1.0, matrix.RowL2Norm(0), 1e-12);
        Assert.Equal(1.0, matrix.RowL2Norm(1), 1e-12);
    }

    [Fact]
    public void The_vector_product_skips_the_zeros() =>
        Assert.Equal([7.0, 6.0], Sample().Multiply([1.0, 2.0, 3.0]));

    [Fact]
    public void A_vector_of_the_wrong_length_is_refused() =>
        Assert.Throws<ArgumentException>(() => Sample().Multiply([1.0, 2.0]));

    /// <summary>
    /// The unchecked factory, public since 0.2.0, takes the arrays as given; the published
    /// <c>Lodestar.Text</c> that calls it is <c>PublishedTextCompatibilityTests</c>' concern (#1290).
    /// </summary>
    [Fact]
    public void The_unchecked_factory_builds_the_matrix_it_is_given() =>
        Assert.Equal(3, CsrMatrix.CreateUnchecked(2, 3, [1.0, 2.0, 3.0], [0, 2, 1], [0, 2, 3])
                                 .NonZeroCount);

    [Fact]
    public void An_undefined_norm_is_refused_rather_than_read_as_l2()
    {
        // sklearn.preprocessing.normalize raises on an unknown norm; this read it as L2 (#1286).
        CsrMatrix matrix = new(1, 2, [3.0, 4.0], [0, 1], [0, 2]);

        Assert.Throws<ArgumentOutOfRangeException>(() => matrix.NormalizeRows((SparseNorm)2));
        Assert.Equal([3.0, 4.0], matrix.Values);
    }

    [Fact]
    public void A_matrix_too_large_to_densify_is_refused_before_allocating() =>
        // A few bytes stored, 16 GB densified (#1287).
        Assert.Throws<InvalidOperationException>(
            () => new CsrMatrix(1, int.MaxValue, [], [], [0, 0]).ToDense());

    [Fact]
    public void A_product_past_the_largest_array_is_refused_rather_than_allocated()
    {
        // No row, so the operand is empty: 0x7FFFFFC8 result cells fit an int and no array (#1287).
        CsrMatrix matrix = new(0, 1, [], [], [0]);

        Assert.Throws<ArgumentOutOfRangeException>(() => matrix.TransposeMultiply([], 0x7FFFFFC8));
    }

    [Fact]
    public void A_column_stored_twice_in_a_row_densifies_to_the_sum_the_product_reads()
    {
        // scipy's toarray() gives [[3, 0]] for this row; ToDense used to keep the last entry, 2 (#878).
        var matrix = new CsrMatrix(1, 2, [1.0, 2.0], [0, 0], [0, 2]);

        Assert.Equal(3.0, matrix.ToDense()[0, 0]);
        Assert.Equal(0.0, matrix.ToDense()[0, 1]);
        Assert.Equal(matrix.Multiply([1.0, 0.0])[0], matrix.ToDense()[0, 0]);
    }

    [Fact]
    public void Columns_out_of_order_in_a_row_densify_to_their_own_cells()
    {
        var matrix = new CsrMatrix(1, 3, [1.0, 2.0], [2, 0], [0, 2]);

        Assert.Equal([2.0, 0.0, 1.0], [matrix.ToDense()[0, 0], matrix.ToDense()[0, 1], matrix.ToDense()[0, 2]]);
    }
}
