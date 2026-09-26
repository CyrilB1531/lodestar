using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What the extended Cox fits refuse, and the identities between their entry points.</summary>
public sealed class CoxExtendedEdgeTests
{
    private static readonly double[] Design = [0.5, -1.2, 0.3, 1.8, -0.4, 0.9, -1.5, 0.2, 1.1, -0.7];
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 1.0, 6.0, 2.5, 9.0, 4.0, 1.5, 7.0];
    private static readonly bool[] Events = [true, true, false, true, true, true, false, true, true, true];

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_penalizer_that_is_not_a_non_negative_number_is_refused(double penalizer)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { Penalizer = penalizer });
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    public void An_l1_ratio_outside_the_unit_interval_is_refused(double ratio)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { L1Ratio = ratio });
    }

    [Fact]
    public void Weights_strata_and_clusters_must_be_one_per_subject()
    {
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, [1.0], [], [], 1)).ParamName);
        Assert.Equal("strata", Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, [], [0, 1], [], 1)).ParamName);
        Assert.Equal("clusters", Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, [], [], [3], 1)).ParamName);
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, [1, 1, 1, 0, 1, 1, 1, 1, 1, 1], [], [], 1)).ParamName);
    }

    [Fact]
    public void A_covariate_that_does_not_vary_is_refused()
    {
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit([.. Durations.Select(_ => 2.0)], Durations, Events, 1)).ParamName);
    }

    /// <summary>Weights of one are no weighting, and a cluster per subject is the plain sandwich.</summary>
    [Fact]
    public void Unit_weights_and_singleton_clusters_change_nothing()
    {
        CoxSummary plain = CoxProportionalHazards.Fit(Design, Durations, Events, 1, new CoxOptions { Robust = true });
        CoxSummary weighted = CoxProportionalHazards.Fit(
            Design, Durations, Events, [.. Durations.Select(_ => 1.0)], [], [], 1, new CoxOptions { Robust = true });
        CoxSummary clustered = CoxProportionalHazards.Fit(
            Design, Durations, Events, [], [], [.. Enumerable.Range(0, Durations.Length)], 1);

        Assert.Equal(plain.Coefficients[0], weighted.Coefficients[0], 12);
        Assert.Equal(plain.StandardErrors[0], weighted.StandardErrors[0], 12);
        Assert.Equal(plain.StandardErrors[0], clustered.StandardErrors[0], 12);
        Assert.True(clustered.Robust);
    }

    /// <summary>With no tie, a weight of two is the subject written twice, as lifelines documents for Efron without ties.</summary>
    [Fact]
    public void Without_ties_a_weight_of_two_is_a_duplicated_subject()
    {
        CoxSummary weighted = CoxProportionalHazards.Fit(
            Design, Durations, Events, [2, 1, 1, 1, 1, 1, 1, 1, 1, 1], [], [], 1);

        // The duplicate ties with its original, which Efron then treats as a tie: so compare on a row that is censored.
        double[] design = [.. Design, Design[2]];
        double[] durations = [.. Durations, Durations[2]];
        bool[] events = [.. Events, Events[2]];
        CoxSummary duplicatedCensored = CoxProportionalHazards.Fit(design, durations, events, 1);
        CoxSummary weightedCensored = CoxProportionalHazards.Fit(
            Design, Durations, Events, [1, 1, 2, 1, 1, 1, 1, 1, 1, 1], [], [], 1);

        Assert.Equal(duplicatedCensored.Coefficients[0], weightedCensored.Coefficients[0], 10);
        Assert.NotEqual(weighted.Coefficients[0], weightedCensored.Coefficients[0]);
    }

    [Fact]
    public void A_stratified_fit_needs_each_subjects_stratum_to_predict()
    {
        CoxSummary fit = CoxProportionalHazards.Fit(
            Design, Durations, Events, [], [0, 1, 0, 1, 0, 1, 0, 1, 0, 1], [], 1);

        Assert.Equal(2, fit.Baselines.Count);
        Assert.Throws<ArgumentException>(() => fit.PredictSurvivalFunction([0.1], [], []));
        Assert.Throws<ArgumentException>(() => fit.PredictSurvivalFunction([0.1], [7], []));
        Assert.Single(fit.PredictMedian([0.1], [1]));
    }

    [Fact]
    public void A_percentile_outside_the_unit_interval_is_refused()
    {
        CoxSummary fit = CoxProportionalHazards.Fit(Design, Durations, Events, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => fit.PredictPercentile([0.1], [], 1.5));
    }

    [Fact]
    public void The_proportional_hazards_test_needs_a_fit_of_the_same_width()
    {
        CoxSummary fit = CoxProportionalHazards.Fit(Design, Durations, Events, 1);

        Assert.Throws<ArgumentNullException>(() => CoxProportionalHazards.TestProportionalHazards(Design, Durations, Events, null!));
        Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.TestProportionalHazards([.. Design, .. Design], Durations, Events, fit));
        Assert.Single(CoxProportionalHazards.TestProportionalHazards(Design, Durations, Events, fit));
        Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.TestProportionalHazards(Design, Durations, [.. Events.Select(_ => false)], fit));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CoxProportionalHazards.TestProportionalHazards(Design, Durations, Events, fit, (CoxTimeTransform)9));

        CoxSummary stratified = CoxProportionalHazards.Fit(Design, Durations, Events, [], [0, 1, 0, 1, 0, 1, 0, 1, 0, 1], [], 1);
        Assert.Throws<ArgumentException>(() => CoxProportionalHazards.TestProportionalHazards(Design, Durations, Events, stratified));
        CoxSummary varying = CoxTimeVarying.Fit(Design, [.. Durations.Select(_ => 0.0)], Durations, Events, 1);
        Assert.Throws<ArgumentException>(() => CoxProportionalHazards.TestProportionalHazards(Design, Durations, Events, varying));
        Assert.Throws<ArgumentException>(() => fit.PredictSurvivalFunction([0.1], [], [double.NaN]));
    }

    [Fact]
    public void The_time_varying_fit_refuses_what_lifelines_does_not_implement_or_accept()
    {
        double[] starts = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0];

        Assert.Throws<ArgumentException>(
            () => CoxTimeVarying.Fit(Design, starts, Durations, Events, 1, new CoxOptions { Robust = true }));
        Assert.Throws<ArgumentException>(
            () => CoxTimeVarying.Fit(Design, Durations, Durations, Events, 1));
        Assert.Throws<ArgumentException>(
            () => CoxTimeVarying.Fit(Design, starts, Durations, [.. Events.Select(_ => false)], 1));
    }

    /// <summary>One interval per subject from zero is the ordinary fit, which is what the time-varying form generalises.</summary>
    [Fact]
    public void Single_intervals_from_zero_are_the_ordinary_fit()
    {
        double[] starts = [.. Durations.Select(_ => 0.0)];

        CoxSummary ordinary = CoxProportionalHazards.Fit(Design, Durations, Events, 1);
        CoxSummary varying = CoxTimeVarying.Fit(Design, starts, Durations, Events, 1);

        Assert.Equal(ordinary.Coefficients[0], varying.Coefficients[0], 10);
        Assert.Equal(ordinary.StandardErrors[0], varying.StandardErrors[0], 10);
        Assert.True(double.IsNaN(varying.ConcordanceIndex));
    }
}
