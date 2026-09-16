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

    /// <summary>Three rows, two columns, the second column never stored.</summary>
    private static CsrMatrix Small() => new(3, 2, [1.0, 3.0], [0, 0], [0, 1, 1, 2]);
}
