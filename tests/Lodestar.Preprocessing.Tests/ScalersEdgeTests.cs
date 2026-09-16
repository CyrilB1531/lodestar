using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the corpus does not reach for the three scalers of #763: every refusal, the two
/// asymmetries between a transform and its inverse, and the near-constant floor read from
/// both sides.
/// </summary>
public sealed class ScalersEdgeTests
{
    private static readonly double[] TwoByTwo = [1.0, 10.0, 3.0, 10.0];

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MinMaxScaler.Fit(TwoByTwo, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => MaxAbsScaler.Fit(TwoByTwo, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => RobustScaler.Fit(TwoByTwo, 0));
    }

    [Fact]
    public void A_span_that_is_not_a_whole_number_of_rows_is_refused()
    {
        Assert.Throws<ArgumentException>(() => MinMaxScaler.Fit([1.0, 2.0, 3.0], 2));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit([], 2));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit([1.0, 2.0, 3.0], 2));
    }

    /// <summary>
    /// The reference skips a <c>NaN</c>; a percentile over a sorted column cannot, so all three
    /// refuse it rather than answer from wherever the sort put it.
    /// </summary>
    [Fact]
    public void A_non_finite_value_is_refused_on_the_way_in_and_on_the_way_out()
    {
        double[] withNan = [1.0, double.NaN, 3.0, 4.0];

        Assert.Throws<ArgumentException>(() => MinMaxScaler.Fit(withNan, 2));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(withNan, 2));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(withNan, 2));
        Assert.Throws<ArgumentException>(() => MinMaxScaler.Fit(TwoByTwo, 2).Transform(withNan));
        Assert.Throws<ArgumentException>(
            () => MaxAbsScaler.Fit(TwoByTwo, 2).InverseTransform([1.0, double.PositiveInfinity, 0.0, 0.0]));
    }

    [Fact]
    public void A_feature_range_that_is_not_two_ordered_finite_bounds_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => MinMaxScaler.Fit(TwoByTwo, 2, new MinMaxScalerOptions { Low = 1.0, High = 1.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => MinMaxScaler.Fit(TwoByTwo, 2, new MinMaxScalerOptions { Low = 2.0, High = 1.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => MinMaxScaler.Fit(TwoByTwo, 2, new MinMaxScalerOptions { High = double.PositiveInfinity }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => MinMaxScaler.Fit(TwoByTwo, 2, new MinMaxScalerOptions { Low = double.NaN }));
    }

    [Fact]
    public void A_percentile_range_outside_zero_to_a_hundred_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RobustScaler.Fit(TwoByTwo, 2, new RobustScalerOptions { LowerPercentile = -1.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RobustScaler.Fit(TwoByTwo, 2, new RobustScalerOptions { UpperPercentile = 101.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RobustScaler.Fit(
                TwoByTwo, 2, new RobustScalerOptions { LowerPercentile = 75.0, UpperPercentile = 25.0 }));
    }

    /// <summary>
    /// Clipping applies to the transform and not to its inverse, which is the reference's
    /// asymmetry: a value clipped on the way in is not recoverable, and clipping again on the way
    /// out would hide that rather than undo it.
    /// </summary>
    [Fact]
    public void Clipping_bounds_the_transform_and_never_the_inverse()
    {
        double[] rows = [0.0, 10.0];
        MinMaxScaler clipped = MinMaxScaler.Fit(rows, 1, new MinMaxScalerOptions { Clip = true });
        MinMaxScaler plain = MinMaxScaler.Fit(rows, 1);

        Assert.Equal(1.0, clipped.Transform([20.0])[0]);
        Assert.Equal(2.0, plain.Transform([20.0])[0]);
        Assert.Equal(0.0, clipped.Transform([-5.0])[0]);

        // The inverse of a value outside the range walks straight back out of it.
        Assert.Equal(20.0, clipped.InverseTransform([2.0])[0], 1e-12);

        MaxAbsScaler clippedAbs = MaxAbsScaler.Fit(rows, 1, new MaxAbsScalerOptions { Clip = true });
        Assert.Equal(1.0, clippedAbs.Transform([50.0])[0]);
        Assert.Equal(-1.0, clippedAbs.Transform([-50.0])[0]);
        Assert.Equal(50.0, clippedAbs.InverseTransform([5.0])[0], 1e-12);
    }

    /// <summary>
    /// The near-constant floor is <c>range &lt; 10·eps</c> and not <c>range == 0</c>, read from
    /// both sides of the threshold — the pair the corpus freezes, asserted here as the rule rather
    /// than as two numbers.
    /// </summary>
    [Fact]
    public void A_range_below_ten_epsilons_scales_by_one_and_one_just_above_does_not()
    {
        double[] below = [1.0, 1.0 + 1e-15, 1.0];
        double[] above = [1.0, 1.0 + 4e-15, 1.0 + 2e-15];

        MinMaxScaler floored = MinMaxScaler.Fit(below, 1);
        MinMaxScaler scaled = MinMaxScaler.Fit(above, 1);

        Assert.True(floored.DataRange[0] > 0.0, "the range is small, not zero");
        Assert.Equal(1.0, floored.Scale[0]);
        Assert.True(scaled.Scale[0] > 1e14, $"a range of {scaled.DataRange[0]} should scale by about 2.5e14");

        // A feature the floor caught lands on the bottom of the range rather than anywhere in it.
        Assert.Equal(0.0, floored.Transform(below)[0], 1e-12);
    }

    /// <summary>A feature that is all zeros divides by 1, which is the whole point of the floor.</summary>
    [Fact]
    public void An_all_zero_feature_divides_by_one()
    {
        MaxAbsScaler scaler = MaxAbsScaler.Fit([0.0, 0.0, 0.0], 1);

        Assert.Equal(0.0, scaler.MaximumAbsolute[0]);
        Assert.Equal(1.0, scaler.Scale[0]);
        Assert.Equal([0.0, 0.0, 0.0], scaler.Transform([0.0, 0.0, 0.0]));
    }

    /// <summary>
    /// Each switch decides exactly one statistic, which is a simpler mapping than
    /// <see cref="StandardScaler"/>'s and is worth pinning because of that.
    /// </summary>
    [Fact]
    public void Turning_a_robust_step_off_leaves_exactly_its_own_statistic_null()
    {
        double[] rows = [1.0, 2.0, 3.0, 4.0, 5.0];

        RobustScaler both = RobustScaler.Fit(rows, 1);
        RobustScaler noCentre = RobustScaler.Fit(rows, 1, new RobustScalerOptions { WithCentring = false });
        RobustScaler noScale = RobustScaler.Fit(rows, 1, new RobustScalerOptions { WithScaling = false });
        RobustScaler neither = RobustScaler.Fit(
            rows, 1, new RobustScalerOptions { WithCentring = false, WithScaling = false });

        Assert.NotNull(both.Centre);
        Assert.NotNull(both.Scale);
        Assert.Null(noCentre.Centre);
        Assert.NotNull(noCentre.Scale);
        Assert.NotNull(noScale.Centre);
        Assert.Null(noScale.Scale);
        Assert.Null(neither.Centre);
        Assert.Null(neither.Scale);

        // With both off the transform is the identity, and the inverse with it.
        Assert.Equal(rows, neither.Transform(rows));
    }

    /// <summary>
    /// The percentile interpolates linearly, which is <c>numpy.percentile</c>'s default and neither
    /// of the two quantile conventions already in this repository. Five values put the quartiles
    /// on an index; six put them between two.
    /// </summary>
    [Fact]
    public void The_percentile_interpolates_between_two_values_when_it_falls_between_them()
    {
        // 0..4: h = (5-1) * 0.25 = 1 exactly, so the quartiles are the values themselves, 1 and 3.
        RobustScaler onIndex = RobustScaler.Fit([0.0, 1.0, 2.0, 3.0, 4.0], 1);
        Assert.Equal(2.0, onIndex.Centre![0]);
        Assert.Equal(2.0, onIndex.Scale![0]);

        // 0..5: h = (6-1) * 0.25 = 1.25, so the lower quartile is 1.25 and the upper 3.75.
        RobustScaler between = RobustScaler.Fit([0.0, 1.0, 2.0, 3.0, 4.0, 5.0], 1);
        Assert.Equal(2.5, between.Centre![0]);
        Assert.Equal(2.5, between.Scale![0], 1e-12);
    }

    /// <summary>
    /// Unit variance divides by the normal quantiles of the percentile pair, and 0 and 100 have
    /// none — refused here, where the reference divides by an infinity and reports a scale of zero.
    /// </summary>
    [Fact]
    public void Unit_variance_with_a_percentile_of_zero_or_a_hundred_is_refused()
    {
        double[] rows = [1.0, 2.0, 3.0, 4.0];

        Assert.Throws<ArgumentOutOfRangeException>(() => RobustScaler.Fit(
            rows, 1, new RobustScalerOptions { UnitVariance = true, LowerPercentile = 0.0 }));
        Assert.Throws<ArgumentOutOfRangeException>(() => RobustScaler.Fit(
            rows, 1, new RobustScalerOptions { UnitVariance = true, UpperPercentile = 100.0 }));

        // The same range is accepted with unit variance off, which is what makes this a refusal
        // about the divisor rather than about the range.
        Assert.NotNull(RobustScaler.Fit(
            rows, 1, new RobustScalerOptions { LowerPercentile = 0.0, UpperPercentile = 100.0 }).Scale);
    }

    /// <summary>
    /// The two rules in the order the reference applies them: the range is floored to 1 first and
    /// divided by <c>Φ⁻¹(0.75) − Φ⁻¹(0.25)</c> second, so a constant feature lands on
    /// <c>1/1.3489795</c> rather than on 1. Applying them the other way round gives 1, and the
    /// corpus separates the two.
    /// </summary>
    [Fact]
    public void Unit_variance_divides_the_floored_range_rather_than_the_raw_one()
    {
        RobustScaler scaler = RobustScaler.Fit(
            [5.0, 5.0, 5.0], 1, new RobustScalerOptions { UnitVariance = true });

        Assert.Equal(1.0 / 1.3489795003921634, scaler.Scale![0], 1e-12);
    }

    /// <summary>A transform is refused on a shape the scaler was not fitted for, as the scaling pair is.</summary>
    [Fact]
    public void Transforming_a_shape_the_scaler_was_not_fitted_for_is_refused()
    {
        Assert.Throws<ArgumentException>(() => MinMaxScaler.Fit(TwoByTwo, 2).Transform([1.0, 2.0, 3.0]));
        Assert.Throws<ArgumentException>(() => MaxAbsScaler.Fit(TwoByTwo, 2).InverseTransform([]));
        Assert.Throws<ArgumentException>(() => RobustScaler.Fit(TwoByTwo, 2).Transform([1.0]));
    }
}
