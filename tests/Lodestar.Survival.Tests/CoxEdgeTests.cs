using Lodestar.Stats;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What <see cref="CoxProportionalHazards.Fit"/> refuses, and the settings it honours.</summary>
/// <remarks>
/// lifelines returns numbers behind a warning for a separated or collinear design; this refuses
/// both, because the information is singular there and no standard error is worth reporting.
/// </remarks>
public sealed class CoxEdgeTests
{
    /// <summary>Six subjects on one covariate, with ties and a censoring: a well-posed fit.</summary>
    private static readonly double[] Design = [0.2, 1.1, -0.4, 0.9, -1.3, 0.5];
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 3.0, 9.0, 6.0];
    private static readonly bool[] Events = [true, true, false, true, true, true];

    [Fact]
    public void A_well_posed_fit_reports_one_row_per_covariate()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 1);

        double coefficient = Assert.Single(summary.Coefficients);
        Assert.Equal(0.95, summary.ConfidenceLevel);
        Assert.Equal(1, summary.LikelihoodRatioDegreesOfFreedom);
        Assert.Equal(Math.Exp(coefficient), Assert.Single(summary.HazardRatios), 1e-15);
    }

    [Fact]
    public void The_confidence_level_sets_the_interval_multiplier()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(
            Design, Durations, Events, featureCount: 1, new CoxOptions { ConfidenceLevel = 0.9 });

        double multiplier = Distributions.NormalQuantile(0.95);
        Assert.Equal(0.9, summary.ConfidenceLevel);
        Assert.Equal(
            summary.Coefficients[0] - (multiplier * summary.StandardErrors[0]),
            summary.ConfidenceLower[0], 1e-12);
        Assert.Equal(
            summary.Coefficients[0] + (multiplier * summary.StandardErrors[0]),
            summary.ConfidenceUpper[0], 1e-12);
    }

    [Fact]
    public void A_covariate_that_separates_the_events_is_refused()
    {
        // Every event has a higher covariate than every subject still at risk after it, so the
        // likelihood keeps rising as the coefficient grows and has no maximum.
        double[] design = [4.0, 3.0, 2.0, 1.0, 0.0];
        double[] durations = [1.0, 2.0, 3.0, 4.0, 5.0];
        bool[] events = [true, true, true, true, true];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, durations, events, featureCount: 1));

        Assert.Contains("separat", error.Message, StringComparison.Ordinal);
        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_duplicated_covariate_is_refused_as_collinear()
    {
        double[] design = new double[Design.Length * 2];
        for (int i = 0; i < Design.Length; i++)
        {
            design[2 * i] = Design[i];
            design[(2 * i) + 1] = Design[i];
        }

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, Durations, Events, featureCount: 2));

        Assert.Contains("collinear", error.Message, StringComparison.Ordinal);
        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_sample_with_no_event_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, new bool[Design.Length], featureCount: 1));

        Assert.Equal("eventObserved", error.ParamName);
    }

    [Fact]
    public void A_design_whose_length_disagrees_with_the_sample_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 2));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_non_finite_covariate_is_refused()
    {
        double[] design = [.. Design];
        design[3] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, Durations, Events, featureCount: 1));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void Spans_of_different_lengths_are_refused()
    {
        Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events.AsSpan(0, 5), featureCount: 1));
    }

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 0));
    }

    [Fact]
    public void Running_out_of_iterations_throws_rather_than_reporting_a_table()
    {
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => CoxProportionalHazards.Fit(
                Design, Durations, Events, featureCount: 1, new CoxOptions { MaximumIterations = 1 }));

        Assert.Contains("1", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void An_iteration_budget_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { MaximumIterations = 0 });
    }
}
