using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the two corpora do not reach for #765: the refusals each half makes, and the two properties
/// a caller relies on that no frozen number states.
/// </summary>
public sealed class PartialFitAndSparseEdgeTests
{
    private static readonly double[] FourByTwo = [1.0, 10.0, 2.0, 20.0, 3.0, 30.0, 4.0, 40.0];

    /// <summary>A batch of a different width is not the next batch of the same matrix.</summary>
    [Fact]
    public void A_batch_of_another_shape_is_refused()
    {
        StandardScaler standard = StandardScaler.Fit(FourByTwo, 2);

        Assert.Throws<ArgumentException>(() => standard.PartialFit([1.0, 2.0, 3.0]));
        Assert.Throws<ArgumentException>(() => standard.PartialFit([]));
        Assert.Throws<ArgumentException>(() => MinMaxScaler.Fit(FourByTwo, 2).PartialFit([1.0]));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(FourByTwo, 2).PartialFit([1.0]));
    }

    /// <summary>
    /// The scaler a batch was folded into is unchanged: this package's fitted objects are immutable,
    /// which is the one place the spelling differs from the reference's <c>partial_fit</c>.
    /// </summary>
    [Fact]
    public void Partial_fit_leaves_the_scaler_it_was_called_on_alone()
    {
        StandardScaler first = StandardScaler.Fit(FourByTwo, 2);
        double firstMean = first.Mean![0];
        int firstCount = first.SampleCount;

        StandardScaler second = first.PartialFit([100.0, 200.0]);

        Assert.Equal(firstMean, first.Mean[0]);
        Assert.Equal(firstCount, first.SampleCount);
        Assert.Equal(firstCount + 1, second.SampleCount);
        Assert.NotEqual(firstMean, second.Mean![0]);
    }

    /// <summary>A batch inside the range a min-max scaler already covers moves nothing at all.</summary>
    [Fact]
    public void A_batch_inside_the_range_changes_no_statistic()
    {
        MinMaxScaler scaler = MinMaxScaler.Fit(FourByTwo, 2);
        MinMaxScaler folded = scaler.PartialFit([2.5, 25.0]);

        Assert.Equal(scaler.DataMinimum, folded.DataMinimum);
        Assert.Equal(scaler.DataMaximum, folded.DataMaximum);
        Assert.Equal(scaler.Scale, folded.Scale);
        Assert.Equal(scaler.SampleCount + 1, folded.SampleCount);
    }

    /// <summary>
    /// Centring a sparse matrix is refused, which is the reference's rule and not a limitation of
    /// this implementation: subtracting a mean makes every absent zero a stored value.
    /// </summary>
    [Fact]
    public void Centring_a_sparse_matrix_is_refused()
    {
        CsrMatrix matrix = Small();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => StandardScaler.Fit(matrix, new StandardScalerOptions { WithMean = true }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RobustScaler.Fit(matrix, new RobustScalerOptions { WithCentring = true }));

        // And the default for a sparse fit is the one the reference accepts.
        Assert.NotNull(StandardScaler.Fit(matrix).Scale);
        Assert.NotNull(RobustScaler.Fit(matrix).Scale);
        Assert.Null(RobustScaler.Fit(matrix).Centre);
    }

    /// <summary>A non-finite stored value is refused, as the dense overloads refuse one.</summary>
    [Fact]
    public void A_non_finite_stored_value_is_refused()
    {
        var matrix = new CsrMatrix(2, 2, [1.0, double.NaN], [0, 1], [0, 1, 2]);

        Assert.Throws<ArgumentException>(() => StandardScaler.Fit(matrix));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(matrix));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(matrix));
    }

    /// <summary>
    /// A column with no stored value is all zeros, so its scale is floored to 1 rather than to a
    /// division by nothing — the same rule the dense path applies, reached a different way.
    /// </summary>
    [Fact]
    public void A_column_with_no_stored_value_scales_by_one()
    {
        CsrMatrix matrix = Small();

        Assert.Equal(1.0, MaxAbsScaler.Fit(matrix).Scale[1]);
        Assert.Equal(0.0, MaxAbsScaler.Fit(matrix).MaximumAbsolute[1]);
        Assert.Equal(1.0, StandardScaler.Fit(matrix).Scale![1]);
    }

    /// <summary>A scaler that centres cannot transform a sparse matrix either way, however it was fitted (#895).</summary>
    [Fact]
    public void A_centring_scaler_refuses_a_sparse_transform()
    {
        CsrMatrix matrix = Small();
        StandardScaler standard = StandardScaler.Fit([1.0, 0.0, 0.0, 0.0, 3.0, 0.0], 2);
        RobustScaler robust = RobustScaler.Fit([1.0, 0.0, 0.0, 0.0, 3.0, 0.0], 2);

        Assert.Throws<InvalidOperationException>(() => standard.Transform(matrix));
        Assert.Throws<InvalidOperationException>(() => standard.InverseTransform(matrix));
        Assert.Throws<InvalidOperationException>(() => robust.Transform(matrix));
        Assert.Throws<InvalidOperationException>(() => robust.InverseTransform(matrix));
    }

    [Fact]
    public void A_sparse_matrix_of_another_width_is_refused()
    {
        var wide = new CsrMatrix(1, 3, [1.0], [2], [0, 1]);

        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(Small()).Transform(wide));
        Assert.Throws<ArgumentException>(() => StandardScaler.Fit(Small()).InverseTransform(wide));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(Small()).Transform(wide));
    }

    /// <summary>A matrix with no row is refused, as the dense overloads and scikit-learn refuse one (#989).</summary>
    [Fact]
    public void A_sparse_matrix_with_no_row_is_refused()
    {
        var empty = new CsrMatrix(0, 2, [], [], [0]);

        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(Small()).Transform(empty));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(Small()).InverseTransform(empty));
        Assert.Throws<ArgumentException>(() => StandardScaler.Fit(Small()).Transform(empty));
        Assert.Throws<ArgumentException>(() => StandardScaler.Fit(Small()).InverseTransform(empty));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(Small()).Transform(empty));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(Small()).InverseTransform(empty));
    }

    /// <summary>A transform's refusal cites the transform, not the fit's percentile (#989).</summary>
    [Fact]
    public void A_non_finite_stored_value_is_refused_by_a_transform_in_its_own_words()
    {
        var matrix = new CsrMatrix(1, 2, [double.NaN, 1.0], [0, 1], [0, 2]);

        ArgumentException fit = Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(matrix));
        ArgumentException transform = Assert.Throws<ArgumentException>(
            () => MaxAbsScaler.Fit(Small()).Transform(matrix));

        Assert.Contains("percentile", fit.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("percentile", transform.Message, StringComparison.Ordinal);
        Assert.Contains("equivalence", transform.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_clipping_max_abs_scaler_clips_the_stored_values()
    {
        MaxAbsScaler scaler = MaxAbsScaler.Fit(Small(), new MaxAbsScalerOptions { Clip = true });

        CsrMatrix clipped = scaler.Transform(new CsrMatrix(1, 2, [-6.0], [0], [0, 1]));

        Assert.Equal(-1.0, clipped.Values[0]);
    }

    [Fact]
    public void A_sparse_transform_leaves_the_input_alone()
    {
        CsrMatrix matrix = Small();

        CsrMatrix transformed = MaxAbsScaler.Fit(matrix).Transform(matrix);
        transformed.Values[0] = 42.0;

        Assert.Equal(1.0, matrix.Values[0]);
        Assert.NotSame(matrix.ColumnIndices, transformed.ColumnIndices);
    }

    /// <summary>
    /// A column stored twice in one row is the sum of its entries, as scipy's reductions read it
    /// after <c>sum_duplicates</c>: scikit-learn answers <c>scale_ = [8]</c> on this matrix, where
    /// this answered <c>[5]</c> (#1044). <c>CsrMatrix.ToDense</c> agrees from 0.1.2 on (#878), which
    /// is above the floor this package builds against, so only the scalers are asserted here.
    /// </summary>
    [Fact]
    public void A_column_stored_twice_in_a_row_counts_as_the_sum_of_its_entries()
    {
        CsrMatrix duplicated = Duplicated();

        Assert.Equal(8.0, MaxAbsScaler.Fit(duplicated).MaximumAbsolute[0]);
        Assert.Equal(8.0, MaxAbsScaler.Fit(duplicated).Scale[0]);
        Assert.Equal(5.0, StandardScaler.Fit(duplicated).Mean![0]);
    }

    /// <summary>
    /// The buffer a percentile is read over is one slot per row, so a column carrying more values
    /// than there are rows had no percentile to read: it raised <c>destinationArray</c>, an internal
    /// buffer name reaching the caller (#1045). Summing the duplicates first is what makes it a column.
    /// </summary>
    [Fact]
    public void A_column_stored_twice_in_a_row_no_longer_overruns_the_percentile_buffer()
    {
        RobustScaler scaler = RobustScaler.Fit(Duplicated());

        // The column is 8 and 2, whose quartiles by linear interpolation are 3.5 and 6.5.
        Assert.Equal(3.0, scaler.Scale![0], 12);
    }

    /// <summary>A matrix storing each column once per row is the one the statistics already read.</summary>
    [Fact]
    public void A_matrix_with_no_duplicate_is_read_exactly_as_before()
    {
        var plain = new CsrMatrix(2, 2, [1.0, 2.0, 3.0, 4.0], [0, 1, 0, 1], [0, 2, 4]);

        Assert.Equal(3.0, MaxAbsScaler.Fit(plain).Scale[0]);
        Assert.Equal(4.0, MaxAbsScaler.Fit(plain).Scale[1]);
        Assert.Equal(2.0, StandardScaler.Fit(plain).Mean![0]);
        Assert.Equal(1.0, RobustScaler.Fit(plain).Scale![0], 12);
    }

    /// <summary>Two rows, one column, whose only column is stored twice in the first row.</summary>
    private static CsrMatrix Duplicated() => new(2, 1, [3.0, 5.0, 2.0], [0, 0, 0], [0, 2, 3]);

    /// <summary>Three rows, two columns, the second column never stored.</summary>
    private static CsrMatrix Small() => new(3, 2, [1.0, 3.0], [0, 0], [0, 1, 1, 2]);
}
