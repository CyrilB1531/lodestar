using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What the log-rank family, the restricted mean, the fixed-point test and the concordance index refuse, and the identities they keep.</summary>
public sealed class LogRankFamilyEdgeTests
{
    private static readonly double[] Durations = [1.0, 2.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0];
    private static readonly bool[] Events = [true, true, false, true, true, false, true, true];
    private static readonly int[] Groups = [0, 1, 0, 1, 0, 1, 0, 1];

    [Theory]
    [InlineData(-1.0, 0.0)]
    [InlineData(0.0, double.NaN)]
    [InlineData(double.PositiveInfinity, 0.0)]
    public void An_exponent_that_is_not_a_non_negative_number_is_refused(double p, double q)
    {
        var options = new LogRankOptions { Weighting = LogRankWeighting.FlemingHarrington, P = p, Q = q };

        Assert.Throws<ArgumentOutOfRangeException>(() => LogRank.MultiGroup(Durations, Groups, Events, options));
    }

    [Theory]
    [InlineData(-0.5)]
    [InlineData(double.NaN)]
    public void A_truncation_that_is_not_a_time_is_refused(double truncation)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => LogRank.MultiGroup(Durations, Groups, Events, new LogRankOptions { Truncation = truncation }));
    }

    [Fact]
    public void A_weighting_outside_the_family_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => LogRank.MultiGroup(Durations, Groups, Events, new LogRankOptions { Weighting = (LogRankWeighting)7 }));
    }

    /// <summary>A curve built by hand with no step has nothing to read, and is refused rather than indexed.</summary>
    [Fact]
    public void A_curve_without_steps_is_refused()
    {
        var empty = new KaplanMeierCurve([], [], [], [], 0.95);
        KaplanMeierCurve curve = KaplanMeier.Estimate(Durations, Events);

        Assert.Throws<ArgumentException>(() => KaplanMeier.CompareAt(1.0, empty, curve));
        Assert.Throws<ArgumentException>(() => KaplanMeier.RestrictedMean(empty, 1.0));
    }

    [Fact]
    public void Groups_that_do_not_match_the_subjects_or_are_one_are_refused()
    {
        Assert.Equal("groups", Assert.Throws<ArgumentException>(
            () => LogRank.MultiGroup(Durations, [0, 1], Events)).ParamName);
        Assert.Equal("groups", Assert.Throws<ArgumentException>(
            () => LogRank.MultiGroup(Durations, [4, 4, 4, 4, 4, 4, 4, 4], Events)).ParamName);
        Assert.Throws<ArgumentException>(() => LogRank.Pairwise(Durations, [4, 4, 4, 4, 4, 4, 4, 4], Events));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_weight_that_is_not_positive_and_finite_is_refused(double bad)
    {
        double[] weights = [1.0, 1.0, bad, 1.0, 1.0, 1.0, 1.0, 1.0];

        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => LogRank.MultiGroup(Durations, Groups, Events, weights)).ParamName);
        Assert.Equal("weightsA", Assert.Throws<ArgumentException>(
            () => LogRank.Test([1.0, 2.0], [true, true], [bad, 1.0], [3.0], [true], [], new LogRankOptions())).ParamName);
    }

    /// <summary>Two groups through the multi-group entry point are the two-sample test.</summary>
    [Fact]
    public void Two_groups_through_every_entry_point_agree()
    {
        var options = new LogRankOptions { Weighting = LogRankWeighting.Peto };
        LogRankResult multi = LogRank.MultiGroup(Durations, Groups, Events, options);
        LogRankResult two = LogRank.Test(
            [1.0, 2.0, 4.0, 6.0], [true, false, true, true], [2.0, 3.0, 5.0, 7.0], [true, true, false, true], options);
        PairwiseLogRankResult pair = Assert.Single(LogRank.Pairwise(Durations, Groups, Events, options));

        Assert.Equal(two.Statistic, multi.Statistic, 12);
        Assert.Equal(two.Statistic, pair.Result.Statistic, 12);
        Assert.Equal((0, 1), (pair.GroupA, pair.GroupB));
    }

    /// <summary>A weight of two is the subject written twice, which is what the weights mean.</summary>
    [Fact]
    public void A_weight_of_two_is_a_duplicated_subject()
    {
        LogRankResult weighted = LogRank.MultiGroup(Durations, Groups, Events, [2.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0]);
        LogRankResult repeated = LogRank.MultiGroup(
            [1.0, .. Durations], [0, .. Groups], [true, .. Events]);

        Assert.Equal(repeated.Statistic, weighted.Statistic, 12);
    }

    /// <summary>Weights of one, and a truncation past every duration, change nothing.</summary>
    [Fact]
    public void Unit_weights_and_a_late_truncation_are_no_change()
    {
        LogRankResult plain = LogRank.MultiGroup(Durations, Groups, Events);

        Assert.Equal(plain, LogRank.MultiGroup(Durations, Groups, Events, [1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0]));
        Assert.Equal(plain, LogRank.MultiGroup(Durations, Groups, Events, new LogRankOptions { Truncation = 100.0 }));
    }

    [Fact]
    public void The_restricted_mean_refuses_a_bad_horizon_and_a_missing_curve()
    {
        KaplanMeierCurve curve = KaplanMeier.Estimate(Durations, Events);

        Assert.Throws<ArgumentOutOfRangeException>(() => KaplanMeier.RestrictedMean(curve, -1.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => KaplanMeier.RestrictedMean(curve, double.NaN));
        Assert.Throws<ArgumentNullException>(() => KaplanMeier.RestrictedMean(null!));
    }

    /// <summary>A curve still above zero at its last step has no finite area to an infinite horizon.</summary>
    [Fact]
    public void An_open_curve_has_an_infinite_unrestricted_mean()
    {
        RestrictedMeanResult result = KaplanMeier.RestrictedMean(KaplanMeier.Estimate([1.0, 2.0], [true, false]));

        Assert.Equal(double.PositiveInfinity, result.Mean);
        Assert.Equal(double.PositiveInfinity, result.Variance);
    }

    /// <summary>Up to zero there is no area at all.</summary>
    [Fact]
    public void A_zero_horizon_has_no_area()
    {
        Assert.Equal(new RestrictedMeanResult(0.0, 0.0), KaplanMeier.RestrictedMean(KaplanMeier.Estimate(Durations, Events), 0.0));
    }

    [Fact]
    public void The_fixed_point_test_refuses_a_bad_time_and_a_missing_curve()
    {
        KaplanMeierCurve curve = KaplanMeier.Estimate(Durations, Events);

        Assert.Throws<ArgumentOutOfRangeException>(() => KaplanMeier.CompareAt(-1.0, curve, curve));
        Assert.Throws<ArgumentNullException>(() => KaplanMeier.CompareAt(1.0, curve, null!));
    }

    [Fact]
    public void The_concordance_index_refuses_mismatched_spans_a_nan_and_no_comparable_pair()
    {
        Assert.Throws<ArgumentException>(() => Concordance.Index([1.0, 2.0], [1.0]));
        Assert.Throws<ArgumentException>(() => Concordance.Index([1.0, 2.0], [1.0, double.NaN]));
        Assert.Throws<ArgumentException>(() => Concordance.Index([1.0, 2.0], [1.0, 2.0], [false, false]));
    }

    /// <summary>Scores ordered as the durations are concordant, reversed discordant: the scale's two ends.</summary>
    [Fact]
    public void Scores_in_the_order_of_the_durations_are_perfectly_concordant()
    {
        Assert.Equal(1.0, Concordance.Index([1.0, 2.0, 3.0], [10.0, 20.0, 30.0]));
        Assert.Equal(0.0, Concordance.Index([1.0, 2.0, 3.0], [30.0, 20.0, 10.0]));
        Assert.Equal(0.5, Concordance.Index([1.0, 2.0, 3.0], [5.0, 5.0, 5.0]));
    }
}
