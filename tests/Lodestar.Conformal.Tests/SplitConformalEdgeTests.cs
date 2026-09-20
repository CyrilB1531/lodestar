using Xunit;

namespace Lodestar.Conformal.Tests;

/// <summary>
/// The edges no oracle can carry: MAPIE raises where these return, so what they assert is
/// decision 0007 rather than a frozen value.
/// </summary>
public sealed class SplitConformalEdgeTests
{
    // k = ceil(10 * 0.95) = 10 > 9: the level asks for a score the set does not hold.
    private static readonly double[] NineScores = [0.2, 0.1, 0.4, 0.3, 0.5, 0.1, 0.4, 0.3, 0.1];

    [Fact]
    public void A_calibration_set_too_small_for_the_level_yields_an_infinite_quantile() =>
        Assert.Equal(double.PositiveInfinity, SplitConformal.Quantile(NineScores, 0.05));

    [Fact]
    public void An_infinite_quantile_yields_the_whole_line()
    {
        (double Lower, double Upper) interval =
            SplitConformal.Interval(4.0, double.PositiveInfinity);

        Assert.Equal(double.NegativeInfinity, interval.Lower);
        Assert.Equal(double.PositiveInfinity, interval.Upper);
    }

    [Fact]
    public void An_infinite_quantile_yields_the_full_label_set() =>
        Assert.Equal([true, true, true],
                     SplitConformal.PredictionSet([0.4, 0.35, 0.25], double.PositiveInfinity));

    [Fact]
    public void The_ceiling_does_not_round_an_exact_integer_up()
    {
        // (n + 1)(1 - alpha) = 20 * 0.9 = 18 exactly, so k is 18: the 18th smallest, not the
        // 19th, which is what numpy's method="higher" reads at this level (decision 0007).
        double[] scores = [.. Enumerable.Range(1, 19).Select(value => (double)value)];

        Assert.Equal(18.0, SplitConformal.Quantile(scores, 0.1));
    }

    [Fact]
    public void MAPIE_s_classification_rule_reads_the_19th_where_the_ceiling_reads_the_18th()
    {
        // level = 20 * 0.9 / 19, and ceil(18 * level) = 18 is the 0-based index numpy's higher reads (#866).
        double[] scores = [.. Enumerable.Range(1, 19).Select(value => (double)value)];

        Assert.Equal(19.0, SplitConformal.Quantile(scores, 0.1, ConformalQuantileRule.MapieClassification));
        Assert.Equal(18.0, SplitConformal.Quantile(scores, 0.1, ConformalQuantileRule.Ceiling));
    }

    [Fact]
    public void MAPIE_s_classification_rule_is_infinite_where_its_level_passes_one() =>
        Assert.Equal(
            double.PositiveInfinity,
            SplitConformal.Quantile(NineScores, 0.05, ConformalQuantileRule.MapieClassification));

    [Fact]
    public void An_undeclared_quantile_rule_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.Quantile(NineScores, 0.1, (ConformalQuantileRule)2));

    [Fact]
    public void A_class_within_MAPIE_s_tolerance_of_the_threshold_is_included() =>
        // (1 - p) - q = 5e-9, inside MAPIE's 1e-8, though p is below 1 - q (#889).
        Assert.True(SplitConformal.PredictionSet([0.699999995, 0.300000005], 0.3)[0]);

    [Fact]
    public void A_class_past_MAPIE_s_tolerance_is_excluded() =>
        Assert.False(SplitConformal.PredictionSet([0.69999998, 0.30000002], 0.3)[0]);

    [Fact]
    public void A_NaN_calibration_score_is_refused()
    {
        // Array.Sort puts NaN first, which moved every order statistic down one (#889).
        double[] scores = [0.1, double.NaN, 0.3, 0.2];

        Assert.Throws<ArgumentException>(() => SplitConformal.Quantile(scores, 0.5));
        Assert.Throws<ArgumentException>(
            () => SplitConformal.Quantile(scores, 0.5, ConformalQuantileRule.MapieClassification));
    }

    [Fact]
    public void One_calibration_score_is_enough_at_a_level_it_can_answer() =>
        Assert.Equal(7.0, SplitConformal.Quantile([7.0], 0.5));

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(-0.1)]
    [InlineData(1.5)]
    [InlineData(double.NaN)]
    public void A_level_outside_the_open_unit_interval_is_refused(double alpha) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.Quantile(NineScores, alpha));

    [Fact]
    public void An_empty_calibration_set_is_refused() =>
        Assert.Throws<ArgumentException>(() => SplitConformal.Quantile([], 0.1));

    [Fact]
    public void Residuals_refuse_spans_of_different_lengths() =>
        Assert.Throws<ArgumentException>(
            () => SplitConformal.AbsoluteResiduals([1.0, 2.0], [1.0]));

    [Fact]
    public void Residuals_are_the_absolute_difference() =>
        Assert.Equal([1.0, 0.5, 0.0],
                     SplitConformal.AbsoluteResiduals([1.0, 2.0, 3.0], [2.0, 1.5, 3.0]));

    [Theory]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    public void A_quantile_that_is_not_a_score_is_refused(double quantile)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SplitConformal.Interval(1.0, quantile));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.PredictionSet([0.5, 0.5], quantile));
    }

    [Fact]
    public void A_class_count_that_is_not_positive_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.LeastAmbiguousScores([0.5, 0.5], [0], 0));

    [Fact]
    public void A_probability_block_that_does_not_fit_the_labels_is_refused() =>
        Assert.Throws<ArgumentException>(
            () => SplitConformal.LeastAmbiguousScores([0.5, 0.5, 0.5], [0, 1], 2));

    [Fact]
    public void A_label_outside_the_class_range_is_refused() =>
        Assert.Throws<ArgumentException>(
            () => SplitConformal.LeastAmbiguousScores([0.5, 0.5], [2], 2));

    [Fact]
    public void A_class_exactly_on_the_threshold_is_included() =>
        // LAC includes the boundary: p >= 1 - q, not p > 1 - q.
        Assert.Equal([true, true], SplitConformal.PredictionSet([0.25, 0.75], 0.75));

    [Fact]
    public void A_zero_quantile_keeps_only_a_certain_class() =>
        Assert.Equal([false, true], SplitConformal.PredictionSet([0.0, 1.0], 0.0));

    [Fact]
    public void Normalised_scores_refuse_spans_of_different_lengths() =>
        Assert.Throws<ArgumentException>(
            () => SplitConformal.NormalisedResiduals([1.0, 2.0], [1.0, 2.0], [1.0]));

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    public void A_residual_estimate_that_is_not_positive_is_refused(double estimate) =>
        // MAPIE floors it at 1e-8 instead. Here the estimate is the caller's own argument,
        // so flooring would turn their bug into an interval that reads as certainty (0118).
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.NormalisedResiduals([1.0], [0.0], [estimate]));

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    public void A_normalised_interval_refuses_the_same_estimate(double estimate) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SplitConformal.NormalisedInterval(1.0, estimate, 0.5));

    [Fact]
    public void An_infinite_quantile_makes_a_normalised_interval_the_whole_line()
    {
        // Carried through the multiplication rather than becoming NaN, which is what
        // an infinite quantile times a finite estimate has to do for decision 0007 to hold.
        (double Lower, double Upper) interval =
            SplitConformal.NormalisedInterval(3.0, 2.0, double.PositiveInfinity);

        Assert.Equal(double.NegativeInfinity, interval.Lower);
        Assert.Equal(double.PositiveInfinity, interval.Upper);
    }

    [Fact]
    public void A_constant_estimate_reduces_the_normalised_score_to_the_absolute_one()
    {
        // The identity that says the two scores are the same operation up to a scale,
        // and the reason a corpus with a constant estimate would test nothing.
        double[] absolute = SplitConformal.AbsoluteResiduals([1.0, 4.0, 9.0], [0.0, 0.0, 0.0]);
        double[] normalised =
            SplitConformal.NormalisedResiduals([1.0, 4.0, 9.0], [0.0, 0.0, 0.0], [2.0, 2.0, 2.0]);

        Assert.Equal([.. absolute.Select(v => v / 2.0)], normalised);
    }
}
