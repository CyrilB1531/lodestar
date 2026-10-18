using Lodestar.Stats;
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
        // Ten subjects of 429,496,730 covariates wrap to 4 in int, which a four-value design matched (#1614).
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(AftModel.Weibull, Design.AsSpan(0, 4), Durations, Events, 429_496_730)).ParamName);
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

    [Theory]
    [InlineData(0.0)]
    [InlineData(1e-200)]
    [InlineData(-1e-160)]
    [InlineData(1e-50)]
    [InlineData(-1e-12)]
    public void The_generalized_gamma_percentile_at_a_vanishing_lambda_is_the_log_normal_one(double lambda)
    {
        // log(λ² G)/λ took G's ε over a spread of 1/|λ|: 7.5e-7 off at 1e-10, +∞ at 1e-50, and 2/λ² overflowed past
        // 1e-154 (#1729's Review A).
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);
        double[] p = [1.3, Math.Log(0.7), lambda];
        double limit = Math.Exp(1.3 + (0.7 * Distributions.NormalIsf(0.1)));

        Assert.Equal(Math.Exp(1.3), model.Percentile(p, 0.5), 1e-11 * Math.Exp(1.3));
        Assert.Equal(limit, model.Percentile(p, 0.1), 1e-11 * limit);
    }

    [Theory]
    [InlineData(0.03, 1e-12, 6.7863563286593804)]
    [InlineData(-0.03, 1e-12, 7.3017846095528736)]
    [InlineData(0.0499999999, 0.1, 1.2517765997064494)]
    [InlineData(0.049, 1e-250, 26.538411405477092)]
    [InlineData(-0.049, 1e-300, 52.50993266313732)]
    public void The_generalized_gamma_percentile_at_a_small_lambda_keeps_its_digits(double lambda, double probability, double w)
    {
        // w against a 330-digit inversion of the gamma tail at shape 1/λ²; Newton on the tail itself stopped at 39.8 for
        // 26.5, and on Φ̄'s slope alone at λ = −0.049 never settled (#1729's Review A).
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);
        double expected = Math.Exp(1.3 + (0.7 * w));

        Assert.Equal(expected, model.Percentile([1.3, Math.Log(0.7), lambda], probability), 2e-15 * expected);
    }

    [Theory]
    [InlineData(0.049, 5e-324, 29.352557274594482)]
    [InlineData(-0.049, 5e-324, 55.340669887362565)]
    [InlineData(-0.01, 1e-320, 40.877719264460886)]
    [InlineData(0.05, 5e-324, 29.21426673848583)]
    [InlineData(-0.5, 5e-324, 373.4035977677564)]
    public void The_generalized_gamma_percentile_at_a_subnormal_probability_keeps_its_digits(double lambda, double probability, double w)
    {
        // A subnormal tail kept a few bits: w at λ = 1e-100, p = 5e-324 read 38.46163 for 38.46741 (#1729's Review A).
        Assert.Equal(w, Internal.GeneralizedGammaQuantile.Standardized(lambda, probability), 1e-15 * w);
    }

    [Theory]
    [InlineData(5e-324)]
    [InlineData(1e-320)]
    [InlineData(2.2250738585072014e-308)]
    [InlineData(0.1)]
    [InlineData(0.999999)]
    public void The_generalized_gamma_percentile_at_a_vanishing_lambda_and_a_tiny_probability_is_the_normal_quantile(double probability)
    {
        // At 2^-1022 a step under half an ulp read as leaving the bracket, and w settled 3 ulps off; at 0.1 Newton on the
        // normal tail's rounding moved it 3 ulps.
        double z = Distributions.NormalIsf(probability);
        Assert.Equal(z, Internal.GeneralizedGammaQuantile.Standardized(1e-100, probability));
        Assert.Equal(z, Internal.GeneralizedGammaQuantile.Standardized(-1e-300, probability));
    }

    [Fact]
    public void The_log_logistic_percentile_keeps_the_digits_of_its_odds()
    {
        // 1/(1 − p) − 1 at p = 1e-13, and 1/p − 1 at p = 1 − 1e-13, kept the ε of a quotient near one: 1.4e-3 and 1.1e-3
        // of the odds.
        double small = 1e-13;
        var model = Internal.UnivariateModel.For(ParametricModel.LogLogistic, []);
        double expected = 3.0 * Math.Pow(small / (1.0 - small), -1.0 / 2.5);
        Assert.Equal(expected, model.Percentile([3.0, 2.5], small), 1e-15 * expected);

        double large = 1.0 - 1e-13;
        var regression = Internal.AftShape.For(AftModel.LogLogistic);
        double aft = Math.Exp(1.3) * Math.Pow((1.0 - large) / large, 1.0 / 2.5);
        Assert.Equal(aft, regression.Percentile(1.3, Math.Log(2.5), large), 1e-15 * aft);
    }

    [Fact]
    public void The_generalized_gamma_percentile_refuses_a_NaN_lambda_as_main_did_and_the_log_normals_keep_a_tiny_probability()
    {
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);
        Assert.Throws<ArgumentOutOfRangeException>(() => model.Percentile([1.3, Math.Log(0.7), double.NaN], 0.1));

        // 1 − 2^-53 is one, whose normal quantile is +∞: the upper quantile of 1e-20 itself is 9.26.
        var logNormal = Internal.UnivariateModel.For(ParametricModel.LogNormal, []);
        Assert.Equal(Math.Exp(1.3 + (0.7 * 9.262340089798408)), logNormal.Percentile([1.3, 0.7], 1e-20), 1e-14 * 2401.0);
        var regression = Internal.AftShape.For(AftModel.LogNormal);
        Assert.Equal(Math.Exp(1.3 + (0.7 * 9.262340089798408)), regression.Percentile(1.3, Math.Log(0.7), 1e-20), 1e-14 * 2401.0);
    }

    [Fact]
    public void A_critical_value_near_a_level_of_one_keeps_its_digits()
    {
        // NormalQuantile(1 − (1 − level)/2) kept about four digits at 1 − 1e-12, and read +∞ at 1 − 2^-53; against the
        // 50-digit upper quantile of the double (1 − level)/2, 4.999889391399392e-13.
        double level = 1.0 - 1e-12;
        Assert.Equal(7.1305098928792724, Lodestar.Internal.CriticalValue.TwoSided(level), 1e-15 * 7.13);
        Assert.True(double.IsFinite(Lodestar.Internal.CriticalValue.TwoSided(1.0 - 1e-15)));
    }
}
