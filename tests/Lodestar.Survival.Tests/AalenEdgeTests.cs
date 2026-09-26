using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What the Aalen additive fit refuses, and the identities it keeps.</summary>
public sealed class AalenEdgeTests
{
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 1.0, 6.0, 2.5, 9.0, 4.0, 1.5, 7.0];
    private static readonly bool[] Events = [true, true, false, true, true, true, false, true, true, true];
    private static readonly double[] Design = [0.5, -1.2, 0.3, 1.8, -0.4, 0.9, -1.5, 0.2, 1.1, -0.7];

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_penalizer_that_is_not_a_non_negative_number_is_refused(double penalizer)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AalenOptions { CoefficientPenalizer = penalizer });
        Assert.Throws<ArgumentOutOfRangeException>(() => new AalenOptions { SmoothingPenalizer = penalizer });
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AalenOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void The_columns_must_be_one_valid_value_per_subject()
    {
        Assert.Throws<ArgumentException>(() => AalenAdditive.Fit(Design, Durations, [true], 1));
        Assert.Throws<ArgumentException>(() => AalenAdditive.Fit([1.0], [1.0], [true], 1));
        Assert.Equal("durations", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit(Design, [.. Durations.Select(d => -d)], Events, 1)).ParamName);
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit(Design, Durations, Events, [.. Durations.Select(_ => 0.0)], 1)).ParamName);
        Assert.Equal("eventObserved", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit(Design, Durations, [.. Events.Select(_ => false)], 1)).ParamName);
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit(Design.AsSpan(0, 4), Durations, Events, 1)).ParamName);
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit([.. Durations.Select(_ => 2.0)], Durations, Events, 1)).ParamName);
        // Many copies of 0.1 leave a deviation of rounding, hundreds of ulps, not zero; the column is as constant.
        double[] many = [.. Enumerable.Range(0, 10_000).Select(i => 1.0 + (i % 97))];
        bool[] all = [.. Enumerable.Repeat(true, 10_000)];
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit([.. many.Select(_ => 0.1)], many, all, 1)).ParamName);

        // Values so small their squared gaps round to zero leave no deviation to scale by.
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit([.. Durations.Select((_, i) => i % 2 == 0 ? 0.0 : 1e-170)], Durations, Events, 1)).ParamName);

        // A large offset is not constant: timestamps varying by seconds still fit.
        AalenSummary offset = AalenAdditive.Fit([.. Design.Select((_, i) => 1.7e9 + i)], Durations, Events, 1);
        Assert.Equal(2, offset.CovariateIndices.Count);
        Assert.Equal("featureCount", Assert.Throws<ArgumentException>(
            () => AalenAdditive.Fit([], Durations, Events, 0, new AalenOptions { FitIntercept = false })).ParamName);
    }

    /// <summary>With no covariate and every censoring at an event time, the intercept is Nelson-Aalen's hazard.</summary>
    [Fact]
    public void An_intercept_alone_is_the_nelson_aalen_hazard_where_censorings_fall_on_event_times()
    {
        double[] durations = [1, 2, 2, 3, 4, 4, 5, 6, 7, 8];
        bool[] events = [true, true, false, true, true, false, true, true, true, true];
        AalenSummary fit = AalenAdditive.Fit([], durations, events, 0);

        Assert.Equal([1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0], fit.EventTimes);
        Assert.Equal(1.0 / 10, fit.Hazards[0], 12);
        Assert.Equal(1.0 / 9, fit.Hazards[1], 12);
        Assert.Equal(1.0 / 7, fit.Hazards[2], 12);
        Assert.Equal(-1, Assert.Single(fit.CovariateIndices));
        Assert.Single(fit.PredictMedian([]));
    }

    [Fact]
    public void A_column_of_zeros_without_an_intercept_leaves_every_increment_at_zero()
    {
        AalenSummary fit = AalenAdditive.Fit(new double[Durations.Length], Durations, Events, 1, new AalenOptions { FitIntercept = false });

        Assert.All(fit.Hazards, h => Assert.Equal(0.0, h));
        Assert.All(fit.CumulativeVariance, v => Assert.Equal(0.0, v));
    }

    /// <summary>Once one arm has all died, the covariate is constant among those left: every later increment is zero.</summary>
    [Fact]
    public void A_large_risk_set_made_singular_gives_zero_increments_rather_than_huge_ones()
    {
        const int half = 3000;
        double[] arm = [.. Enumerable.Range(0, 2 * half).Select(i => i < half ? 0.0 : 1.0)];
        double[] durations = [.. Enumerable.Range(0, 2 * half).Select(i => i < half ? 1.0 + (i * 0.001) : 10.0 + (i * 0.001))];
        bool[] events = [.. Enumerable.Repeat(true, 2 * half)];
        AalenSummary fit = AalenAdditive.Fit(arm, durations, events, 1);

        Assert.All(fit.CumulativeHazards, h => Assert.True(Math.Abs(h) < 100.0, $"{h}"));

        // A penalty, however small, makes the system solvable, as LAPACK solves it.
        AalenSummary penalised = AalenAdditive.Fit(arm, durations, events, 1, new AalenOptions { CoefficientPenalizer = 1e-8 });
        Assert.NotEqual(0.0, penalised.Hazards[half * penalised.CovariateIndices.Count]);
        int k = fit.CovariateIndices.Count;
        for (int t = half; t < fit.EventTimes.Count; t++)
        {
            Assert.Equal(0.0, fit.Hazards[t * k]);
        }
    }

    [Fact]
    public void A_prediction_refuses_what_a_fit_would()
    {
        AalenSummary fit = AalenAdditive.Fit(Design, Durations, Events, 1);

        Assert.Throws<ArgumentException>(() => fit.PredictCumulativeHazard([]));
        Assert.Throws<ArgumentException>(() => fit.PredictSurvivalFunction([double.NaN]));
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.PredictPercentile([0.0], 1.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.SmoothedHazards(0.0));
        Assert.Equal(fit.EventTimes.Count * 2, fit.PredictSurvivalFunction([0.0, 1.0]).Length);
        Assert.Equal(2, fit.PredictExpectation([0.0, 1.0]).Length);
        Assert.Equal(1, fit.FeatureCount);
        Assert.True(fit.FitIntercept);
    }
}
