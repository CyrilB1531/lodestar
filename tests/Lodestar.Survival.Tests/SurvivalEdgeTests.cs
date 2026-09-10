using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What the three entry points refuse, and the shapes they promise.</summary>
public sealed class SurvivalEdgeTests
{
    private static readonly double[] Durations = [1.0, 2.0, 3.0];
    private static readonly bool[] Events = [true, false, true];

    [Fact]
    public void Spans_of_different_length_are_refused()
    {
        Assert.Throws<ArgumentException>(() =>
            KaplanMeier.Estimate([1.0, 2.0], [true]));
        Assert.Throws<ArgumentException>(() =>
            NelsonAalen.Estimate([1.0, 2.0], [true]));
    }

    [Fact]
    public void An_empty_sample_is_refused()
    {
        Assert.Throws<ArgumentException>(() => KaplanMeier.Estimate([], []));
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    public void A_duration_that_is_not_a_time_is_refused(double bad)
    {
        Assert.Throws<ArgumentException>(() =>
            KaplanMeier.Estimate([1.0, bad], [true, true]));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            KaplanMeier.Estimate(Durations, Events, level));
    }

    /// <summary>Time zero is a step, and nothing has happened at it.</summary>
    [Fact]
    public void The_curve_opens_at_time_zero_with_everyone_at_risk()
    {
        KaplanMeierCurve curve = KaplanMeier.Estimate(Durations, Events);

        Assert.Equal(0.0, curve.Steps[0].Time);
        Assert.Equal(3, curve.Steps[0].AtRisk);
        Assert.Equal(0, curve.Steps[0].Events);
        Assert.Equal(1.0, curve.Survival[0]);
    }

    /// <summary>Survival never rises, and the hazard never falls.</summary>
    [Fact]
    public void The_two_curves_are_monotone_in_opposite_directions()
    {
        KaplanMeierCurve km = KaplanMeier.Estimate(Durations, Events);
        NelsonAalenCurve na = NelsonAalen.Estimate(Durations, Events);

        for (int i = 1; i < km.Survival.Length; i++)
        {
            Assert.True(km.Survival[i] <= km.Survival[i - 1]);
            Assert.True(na.CumulativeHazard[i] >= na.CumulativeHazard[i - 1]);
        }
    }

    /// <summary>
    /// A censoring leaves the risk set without moving the estimate, which is the step
    /// Kaplan-Meier is most often got wrong on.
    /// </summary>
    [Fact]
    public void A_time_carrying_only_censorings_does_not_move_the_estimate()
    {
        KaplanMeierCurve curve = KaplanMeier.Estimate([1.0, 2.0, 3.0], [true, false, true]);

        int censoringStep = Array.FindIndex(curve.Steps, s => s.Censored > 0 && s.Events == 0);
        Assert.True(censoringStep > 0);
        Assert.Equal(curve.Survival[censoringStep - 1], curve.Survival[censoringStep]);
    }

    /// <summary>Two identical arms cannot separate, so the statistic is zero.</summary>
    [Fact]
    public void Identical_arms_give_a_zero_statistic_and_a_p_value_of_one()
    {
        LogRankResult result = LogRank.Test(
            [1.0, 2.0, 3.0], [true, true, true],
            [1.0, 2.0, 3.0], [true, true, true]);

        Assert.Equal(0.0, result.Statistic, 1e-12);
        Assert.Equal(1.0, result.PValue, 1e-12);
        Assert.Equal(1, result.DegreesOfFreedom);
    }

    /// <summary>The log-rank p-value is the published chi-squared tail, not a second one.</summary>
    [Fact]
    public void The_p_value_is_the_published_chi_squared_tail()
    {
        LogRankResult result = LogRank.Test(
            [6.0, 7.0, 10.0], [true, true, true],
            [1.0, 2.0, 3.0], [true, true, true]);

        Assert.Equal(
            Lodestar.Stats.Distributions.ChiSquaredSf(result.Statistic, 1.0),
            result.PValue,
            1e-15);
    }
}
