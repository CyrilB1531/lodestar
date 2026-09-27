using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>What the two table tests refuse, and where Yates does and does not apply.</summary>
public sealed class TableTestEdgeTests
{
    [Fact]
    public void GoodnessOfFit_refuses_expectations_that_do_not_sum_to_the_observations()
    {
        Assert.Throws<ArgumentException>(
            () => ChiSquared.GoodnessOfFit([10.0, 10.0], [5.0, 6.0]));
    }

    // scipy's chisquare allows sqrt(eps) = 1.49e-8 relative to the smaller total (#1248).
    [Fact]
    public void GoodnessOfFit_allows_the_totals_scipy_allows()
    {
        TestResult inside = ChiSquared.GoodnessOfFit([100.0, 100.0], [100.0, 100.0 + 2.8e-6]);

        Assert.True(inside.PValue > 0.99);
        Assert.Throws<ArgumentException>(
            () => ChiSquared.GoodnessOfFit([100.0, 100.0], [100.0, 100.0 + 3.2e-6]));
    }

    [Fact]
    public void GoodnessOfFit_refuses_a_zero_expectation()
    {
        Assert.Throws<ArgumentException>(
            () => ChiSquared.GoodnessOfFit([10.0, 10.0], [20.0, 0.0]));
    }

    [Fact]
    public void GoodnessOfFit_refuses_mismatched_lengths_and_answers_one_category_as_scipy()
    {
        Assert.Throws<ArgumentException>(
            () => ChiSquared.GoodnessOfFit([10.0, 10.0], [20.0]));
        Assert.Throws<ArgumentException>(() => ChiSquared.GoodnessOfFit([]));

        // scipy's chisquare([5]) is (0, nan): one category leaves no degree of freedom (#1217).
        TestResult one = ChiSquared.GoodnessOfFit([10.0]);
        Assert.Equal(0.0, one.Statistic);
        Assert.True(double.IsNaN(one.PValue));
    }

    /// <summary>scipy's binomtest(0, 0) answers NaN throughout, and so does this (#1217).</summary>
    [Fact]
    public void No_trial_answers_NaN()
    {
        BinomialResult result = Binomial.Test(0, 0);
        Assert.True(double.IsNaN(result.Statistic) && double.IsNaN(result.PValue));
        (double low, double high) = result.ProportionConfidenceInterval();
        Assert.True(double.IsNaN(low) && double.IsNaN(high));
        Assert.Throws<ArgumentOutOfRangeException>(() => Binomial.Test(0, -1));
    }

    [Fact]
    public void GoodnessOfFit_a_NaN_or_an_infinity_propagates_rather_than_throwing()
    {
        // Measured against scipy 1.18.0: both cases are (nan, nan); unguarded, the
        // NaN statistic reached Gamma.RegularizedQ's Validate, which threw on it.
        TestResult nanResult = ChiSquared.GoodnessOfFit([1.0, double.NaN, 3.0]);
        Assert.True(double.IsNaN(nanResult.Statistic));
        Assert.True(double.IsNaN(nanResult.PValue));

        TestResult infResult = ChiSquared.GoodnessOfFit([1.0, double.PositiveInfinity, 3.0]);
        Assert.True(double.IsNaN(infResult.Statistic));
        Assert.True(double.IsNaN(infResult.PValue));
    }

    [Fact]
    public void Yates_applies_to_a_two_by_two_and_to_nothing_else()
    {
        double[][] twoByTwo = [[10.0, 20.0], [30.0, 40.0]];
        double[][] threeByTwo = [[10.0, 20.0], [30.0, 40.0], [15.0, 5.0]];

        Assert.NotEqual(
            ChiSquared.Contingency(twoByTwo, Continuity.Applied).Statistic,
            ChiSquared.Contingency(twoByTwo, Continuity.None).Statistic);

        // Above 2x2 the correction is not defined, so asking for it changes nothing.
        Assert.Equal(
            ChiSquared.Contingency(threeByTwo, Continuity.Applied).Statistic,
            ChiSquared.Contingency(threeByTwo, Continuity.None).Statistic,
            1e-15);
    }

    [Fact]
    public void Yates_clamps_at_zero_rather_than_overshooting_negative()
    {
        // long-comment: pins two scipy-measured numbers rather than one, and the
        // corpus gap that makes doing so necessary is part of what a reviewer needs.
        // No stats_chisquare.json fixture reaches this: every 2x2 case there deviates
        // from independence by more than half a count. Here all four cells sit under
        // 0.5 away, so an unclamped deviation - 0.5 would go negative and, squared,
        // add a spurious positive term -- measured against scipy 1.18.0,
        // correction=True gives exactly 0.0, not the 0.023242630385487566
        // correction=False gives.
        double[][] table = [[10.0, 10.0], [10.0, 11.0]];

        ChiSquaredContingencyResult corrected = ChiSquared.Contingency(table, Continuity.Applied);
        ChiSquaredContingencyResult uncorrected = ChiSquared.Contingency(table, Continuity.None);

        Assert.Equal(0.0, corrected.Statistic, 1e-15);
        StatsOracleAsserts.Statistic(0.023242630385487566, uncorrected.Statistic, "no clamp");
    }

    [Fact]
    public void Contingency_refuses_a_ragged_table_and_a_zero_marginal()
    {
        double[][] ragged = [[1.0, 2.0], [3.0]];
        double[][] emptyRow = [[0.0, 0.0], [3.0, 4.0]];

        Assert.Throws<ArgumentException>(() => ChiSquared.Contingency(ragged));
        Assert.Throws<ArgumentException>(() => ChiSquared.Contingency(emptyRow));
    }

    [Fact]
    public void Contingency_refuses_a_NaN_cell()
    {
        // Contingency is the one family that refuses rather than propagates a NaN --
        // docs/equivalence.md's nan_policy row names this as the deliberate exception.
        double[][] table = [[1.0, double.NaN], [3.0, 4.0]];

        Assert.Throws<ArgumentException>(() => ChiSquared.Contingency(table));
    }

    [Fact]
    public void Contingency_refuses_an_infinite_cell()
    {
        // +Infinity slipped past the value < 0.0 / IsNaN guard and leaked
        // Gamma.Validate's own ParamName ("x") instead of naming the cell.
        double[][] table = [[1.0, double.PositiveInfinity], [3.0, 4.0]];

        ArgumentException exception =
            Assert.Throws<ArgumentException>(() => ChiSquared.Contingency(table));
        Assert.Equal("table", exception.ParamName);
    }

    [Fact]
    public void FisherExact_refuses_a_table_that_is_not_two_by_two()
    {
        Assert.Throws<ArgumentException>(() => FisherExact.Test([[1, 2, 3], [4, 5, 6]]));
        Assert.Throws<ArgumentException>(() => FisherExact.Test([[1, 2]]));
    }

    [Fact]
    public void FisherExact_refuses_a_negative_count()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FisherExact.Test([[1, -2], [3, 4]]));
    }

    [Fact]
    public void FisherExact_odds_ratio_is_infinite_when_a_diagonal_is_zero()
    {
        TestResult result = FisherExact.Test([[5, 0], [0, 5]]);

        Assert.True(double.IsPositiveInfinity(result.Statistic));
    }

    [Fact]
    public void FisherExact_matches_scipy_when_the_observed_probability_underflows()
    {
        // long-comment: pins scipy's own answer rather than a derived "small
        // positive" one, because scipy is degenerate here too (commit 0b2f0732).
        // Reachable inside the 1,000,000 guard (rowOne = columnOne = 500,000): the
        // observed table's own log-probability is about -693,140, a hard double
        // underflow. scipy 1.18.0 gives exactly 0.0 for both alternatives below,
        // not a small positive number -- the true probability is roughly
        // 10^-301,000, far below any double's representable range.
        int[][] table = [[0, 500_000], [500_000, 0]];

        TestResult twoSided = FisherExact.Test(table, Alternative.TwoSided);
        TestResult less = FisherExact.Test(table, Alternative.Less);

        StatsOracleAsserts.PValue(0.0, twoSided.PValue, "underflow two-sided");
        StatsOracleAsserts.PValue(0.0, less.PValue, "underflow less");
    }

    [Fact]
    public void FisherExact_refuses_a_table_too_large_to_enumerate()
    {
        // The k-loop is O(total); a table this large would enumerate for however
        // long that takes rather than fail fast, well below where int addition overflows.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => FisherExact.Test([[600_000_000, 1], [1, 1]]));
    }
}
