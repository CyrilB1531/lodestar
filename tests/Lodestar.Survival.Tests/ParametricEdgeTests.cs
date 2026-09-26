using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What the parametric fits, the AFT regressions and the two curves refuse, and the identities between them.</summary>
public sealed class ParametricEdgeTests
{
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 1.0, 6.0, 2.5, 9.0, 4.0, 1.5, 7.0];
    private static readonly bool[] Events = [true, true, false, true, true, true, false, true, true, true];
    private static readonly double[] Design = [0.5, -1.2, 0.3, 1.8, -0.4, 0.9, -1.5, 0.2, 1.1, -0.7];

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ParametricOptions { ConfidenceLevel = level });
        Assert.Throws<ArgumentOutOfRangeException>(() => new AftOptions { ConfidenceLevel = level });
        Assert.Throws<ArgumentOutOfRangeException>(() => BreslowFlemingHarrington.Estimate(Durations, Events, level));
        Assert.Throws<ArgumentOutOfRangeException>(() => KaplanMeier.EstimateLeftCensored(Durations, Events, level));
    }

    [Fact]
    public void An_iteration_budget_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ParametricOptions { MaximumIterations = 0 });
        Assert.Throws<ArgumentOutOfRangeException>(() => new AftOptions { MaximumIterations = 0 });
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_penalizer_that_is_not_a_non_negative_number_is_refused(double penalizer)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AftOptions { Penalizer = penalizer });
    }

    [Fact]
    public void Breakpoints_must_be_positive_and_ascending()
    {
        Assert.Throws<ArgumentException>(() => new ParametricOptions { Breakpoints = [2.0, 1.0] });
        Assert.Throws<ArgumentException>(() => new ParametricOptions { Breakpoints = [0.0] });
        Assert.Throws<ArgumentException>(() => new ParametricOptions { Breakpoints = [double.PositiveInfinity] });
        Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(ParametricModel.PiecewiseExponential, Durations, Events));
    }

    [Fact]
    public void Breakpoints_are_copied_when_set()
    {
        double[] breakpoints = [2.0, 5.0];
        var options = new ParametricOptions { Breakpoints = breakpoints };
        breakpoints[0] = 9.0;

        Assert.Equal([2.0, 5.0], options.Breakpoints);
    }

    [Fact]
    public void An_undefined_model_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ParametricSurvival.Fit((ParametricModel)42, Durations, Events));
        Assert.Throws<ArgumentOutOfRangeException>(() => AcceleratedFailureTime.Fit((AftModel)42, Design, Durations, Events, 1));
    }

    [Fact]
    public void The_columns_must_be_one_valid_value_per_subject()
    {
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit(ParametricModel.Weibull, Durations, [true]));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit(ParametricModel.Weibull, [], []));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit(ParametricModel.Weibull, [1.0, 0.0], [true, true]));
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events, [1.0], [])).ParamName);
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events, [.. Durations.Select(_ => -1.0)], [])).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events, [], [1.0])).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events, [], [.. Durations.Select(d => d + 1.0)])).ParamName);
        Assert.Throws<ArgumentException>(() => ParametricSurvival.FitIntervalCensored(ParametricModel.Weibull, [2.0], [1.0]));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.FitIntervalCensored(ParametricModel.Weibull, [-1.0], [1.0]));
        Assert.Throws<ArgumentException>(
            () => ParametricSurvival.FitIntervalCensored(ParametricModel.Weibull, [double.PositiveInfinity], [double.PositiveInfinity]));
    }

    [Fact]
    public void A_fit_that_runs_out_of_steps_says_so()
    {
        var options = new ParametricOptions { MaximumIterations = 1 };
        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.Fit(ParametricModel.LogLogistic, Durations, Events, options));
    }

    [Fact]
    public void A_percentile_outside_the_open_unit_interval_is_refused()
    {
        ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events);
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.Percentile(0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.Percentile(1.0));
        Assert.Equal(fit.Percentile(0.5), fit.MedianSurvivalTime);
    }

    [Fact]
    public void The_design_must_fill_its_rows_with_finite_covariates()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => AcceleratedFailureTime.Fit(AftModel.Weibull, Design, Durations, Events, -1));
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(AftModel.Weibull, Design.AsSpan(0, 5), Durations, Events, 1)).ParamName);
        double[] invalid = [.. Design];
        invalid[3] = double.NaN;
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(AftModel.Weibull, invalid, Durations, Events, 1)).ParamName);
        Assert.Equal("featureCount", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(AftModel.Weibull, [], Durations, Events, 0, new AftOptions { FitIntercept = false })).ParamName);
    }

    [Fact]
    public void A_duplicated_covariate_is_not_identified()
    {
        double[] twice = [.. Design.SelectMany(x => new[] { x, x })];
        Assert.Throws<InvalidOperationException>(() => AcceleratedFailureTime.Fit(AftModel.Weibull, twice, Durations, Events, 2));
    }

    /// <summary>With no covariate the regression is its own null model: the univariate fit, logged.</summary>
    [Theory]
    [InlineData(AftModel.Weibull, ParametricModel.Weibull)]
    [InlineData(AftModel.LogLogistic, ParametricModel.LogLogistic)]
    public void An_intercept_only_regression_is_the_univariate_fit(AftModel model, ParametricModel univariate)
    {
        AftSummary summary = AcceleratedFailureTime.Fit(model, [], Durations, Events, 0);
        ParametricFit fit = ParametricSurvival.Fit(univariate, Durations, Events);

        Assert.Equal(fit.LogLikelihood, summary.LogLikelihood, 9);
        Assert.Equal(summary.NullLogLikelihood, summary.LogLikelihood, 9);
        Assert.Equal(0, summary.LikelihoodRatioDegreesOfFreedom);
        Assert.True(double.IsNaN(summary.LikelihoodRatioPValue));
        Assert.Equal(Math.Log(fit.Parameters[0]), summary.Coefficients[0], 7);
        Assert.Equal(fit.Percentile(0.5), Assert.Single(summary.PredictMedian([])), 7);
        Assert.Throws<ArgumentException>(() => summary.PredictMedian([1.0]));
    }

    [Fact]
    public void An_interval_censored_regression_has_no_concordance()
    {
        double[] lower = [.. Durations.Select((d, i) => Events[i] ? d : d - 0.5)];
        double[] upper = [.. Durations.Select((d, i) => Events[i] ? d : d + 0.5)];
        AftSummary summary = AcceleratedFailureTime.FitIntervalCensored(AftModel.LogNormal, Design, lower, upper, 1);

        Assert.True(double.IsNaN(summary.ConcordanceIndex));
        Assert.Equal(["mu_", "mu_", "sigma_"], summary.ParameterNames);
        Assert.Equal([0, -1, -1], summary.CovariateIndices);
    }

    [Fact]
    public void A_prediction_refuses_what_a_fit_would()
    {
        AftSummary summary = AcceleratedFailureTime.FitLeftCensored(AftModel.LogLogistic, Design, Durations, Events, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => summary.PredictPercentile([0.0], 0.0));
        Assert.Throws<ArgumentException>(() => summary.PredictMedian([]));
        Assert.Throws<ArgumentException>(() => summary.PredictMedian([double.NaN]));
        Assert.Throws<ArgumentException>(() => summary.PredictSurvivalFunction([0.0], [0.0]));
        Assert.Throws<ArgumentException>(() => summary.PredictCumulativeHazard([0.0], [double.PositiveInfinity]));
        double[] survival = summary.PredictSurvivalFunction([0.0, 1.0], [1.0, 4.0]);
        Assert.Equal(4, survival.Length);
        Assert.True(survival[0] > survival[1]);
    }

    [Fact]
    public void A_log_logistic_mean_diverges_at_a_shape_of_one_or_less()
    {
        // Durations spread over orders of magnitude fit a shape below one, where the mean is infinite.
        double[] spread = [0.01, 0.1, 1.0, 10.0, 100.0, 0.05, 5.0, 50.0, 0.5, 500.0];
        AftSummary summary = AcceleratedFailureTime.Fit(AftModel.LogLogistic, [], spread, Events, 0);

        Assert.True(Math.Exp(summary.Coefficients[1]) < 1.0);
        Assert.True(double.IsNaN(Assert.Single(summary.PredictExpectation([]))));
    }

    [Fact]
    public void Entries_must_be_one_time_per_subject_before_its_duration()
    {
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => BreslowFlemingHarrington.Estimate(Durations, Events, [1.0])).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => BreslowFlemingHarrington.Estimate(Durations, Events, [.. Durations.Select(d => d + 1.0)])).ParamName);
    }

    /// <summary>Entries at zero are no entries, and the curve's bounds hold the estimate between them.</summary>
    [Fact]
    public void Entries_at_zero_leave_the_curve_unchanged()
    {
        SurvivalCurve plain = BreslowFlemingHarrington.Estimate(Durations, Events);
        SurvivalCurve entered = BreslowFlemingHarrington.Estimate(Durations, Events, new double[Durations.Length]);

        Assert.Equal(plain, entered);
        Assert.All(plain.Survival.Select((s, i) => (s, i)), p => Assert.InRange(p.s, plain.Lower[p.i], plain.Upper[p.i]));
    }
}
