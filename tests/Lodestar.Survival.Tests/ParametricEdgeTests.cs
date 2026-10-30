using Lodestar.Stats;
using Lodestar.Survival.Internal;
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
            () => ParametricSurvival.Fit(Durations, Events, ParametricModel.PiecewiseExponential));
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
        Assert.Throws<ArgumentOutOfRangeException>(() => ParametricSurvival.Fit(Durations, Events, (ParametricModel)42));
        Assert.Throws<ArgumentOutOfRangeException>(() => AcceleratedFailureTime.Fit(Design, Durations, Events, 1, (AftModel)42));
    }

    [Fact]
    public void The_columns_must_be_one_valid_value_per_subject()
    {
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit(Durations, [true], ParametricModel.Weibull));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit([], [], ParametricModel.Weibull));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.Fit([1.0, 0.0], [true, true], ParametricModel.Weibull));
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(Durations, Events, [1.0], [], ParametricModel.Weibull)).ParamName);
        Assert.Equal("weights", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(Durations, Events, [.. Durations.Select(_ => -1.0)], [], ParametricModel.Weibull)).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(Durations, Events, [], [1.0], ParametricModel.Weibull)).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => ParametricSurvival.Fit(Durations, Events, [], [.. Durations.Select(d => d + 1.0)], ParametricModel.Weibull)).ParamName);
        Assert.Throws<ArgumentException>(() => ParametricSurvival.FitIntervalCensored([2.0], [1.0], ParametricModel.Weibull));
        Assert.Throws<ArgumentException>(() => ParametricSurvival.FitIntervalCensored([-1.0], [1.0], ParametricModel.Weibull));
        Assert.Throws<ArgumentException>(
            () => ParametricSurvival.FitIntervalCensored([double.PositiveInfinity], [double.PositiveInfinity], ParametricModel.Weibull));
    }

    [Fact]
    public void A_fit_that_runs_out_of_steps_says_so()
    {
        var options = new ParametricOptions { MaximumIterations = 1 };
        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.Fit(Durations, Events, ParametricModel.LogLogistic, options));
    }

    [Fact]
    public void A_percentile_outside_the_open_unit_interval_is_refused()
    {
        ParametricSummary fit = ParametricSurvival.Fit(Durations, Events, ParametricModel.Weibull);
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.Percentile(0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => fit.Percentile(1.0));
        Assert.Equal(fit.Percentile(0.5), fit.MedianSurvivalTime);
    }

    [Fact]
    public void The_design_must_fill_its_rows_with_finite_covariates()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => AcceleratedFailureTime.Fit(Design, Durations, Events, -1, AftModel.Weibull));
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(Design.AsSpan(0, 5), Durations, Events, 1, AftModel.Weibull)).ParamName);
        // Ten subjects of 429,496,730 covariates wrap to 4 in int, which a four-value design matched (#1614).
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(Design.AsSpan(0, 4), Durations, Events, 429_496_730, AftModel.Weibull)).ParamName);
        double[] invalid = [.. Design];
        invalid[3] = double.NaN;
        Assert.Equal("design", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit(invalid, Durations, Events, 1, AftModel.Weibull)).ParamName);
        Assert.Equal("featureCount", Assert.Throws<ArgumentException>(
            () => AcceleratedFailureTime.Fit([], Durations, Events, 0, AftModel.Weibull, new AftOptions { FitIntercept = false })).ParamName);
    }

    [Fact]
    public void A_duplicated_covariate_is_not_identified()
    {
        double[] twice = [.. Design.SelectMany(x => new[] { x, x })];
        Assert.Throws<InvalidOperationException>(() => AcceleratedFailureTime.Fit(twice, Durations, Events, 2, AftModel.Weibull));
    }

    /// <summary>With no covariate the regression is its own null model: the univariate fit, logged.</summary>
    [Theory]
    [InlineData(AftModel.Weibull, ParametricModel.Weibull)]
    [InlineData(AftModel.LogLogistic, ParametricModel.LogLogistic)]
    public void An_intercept_only_regression_is_the_univariate_fit(AftModel model, ParametricModel univariate)
    {
        AftSummary summary = AcceleratedFailureTime.Fit([], Durations, Events, 0, model);
        ParametricSummary fit = ParametricSurvival.Fit(Durations, Events, univariate);

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
        AftSummary summary = AcceleratedFailureTime.FitIntervalCensored(Design, lower, upper, 1, AftModel.LogNormal);

        Assert.True(double.IsNaN(summary.ConcordanceIndex));
        Assert.Equal(["mu_", "mu_", "sigma_"], summary.ParameterNames);
        Assert.Equal([0, -1, -1], summary.CovariateIndices);
    }

    [Fact]
    public void A_prediction_refuses_what_a_fit_would()
    {
        AftSummary summary = AcceleratedFailureTime.FitLeftCensored(Design, Durations, Events, 1, AftModel.LogLogistic);

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
        AftSummary summary = AcceleratedFailureTime.Fit([], spread, Events, 0, AftModel.LogLogistic);

        Assert.True(Math.Exp(summary.Coefficients[1]) < 1.0);
        Assert.True(double.IsNaN(Assert.Single(summary.PredictExpectation([]))));
    }

    [Fact]
    public void A_generalized_gamma_near_the_log_normal_clips_a_far_survival_as_lifelines_does()
    {
        // A survival of 1e-230, 40 standard units out, is clipped to 1e-35 as lifelines clips it: unclipped, the likelihood
        // fell 446 across the expansion's edge (#1222). lifelines stops on another maximum, λ = −1.0756, at −96.3106.
        double[] events = [.. Enumerable.Range(0, 200).Select(i => Math.Exp(0.3 * Distributions.NormalIsf((i + 0.5) / 200)))];
        double[] durations = [.. events, Math.Exp(12.0)];
        bool[] observed = [.. durations.Select((_, i) => i < events.Length)];

        ParametricSummary fit = ParametricSurvival.Fit(durations, observed, ParametricModel.GeneralizedGamma);
        ParametricSummary logNormal = ParametricSurvival.Fit(events, [.. events.Select(_ => true)], ParametricModel.LogNormal);

        Assert.Equal(0.0, fit.Parameters[2], 1e-9);
        Assert.Equal(logNormal.LogLikelihood + Math.Log(1e-35), fit.LogLikelihood, 1e-9);
    }

    [Theory]
    [InlineData(0.3499)]
    [InlineData(0.3501)]
    [InlineData(-0.3499)]
    [InlineData(-0.3501)]
    public void A_generalized_gamma_survival_within_an_ulp_of_one_clips_on_both_sides_of_the_expansion(double lambda)
    {
        // At z = −30 the survival is one in doubles, as lifelines clips it: kept within the expansion, H read 1.5e-20
        // at its edge, then at 0.05, and 0 past it, and log(1 − S) jumped from −45.6 to −∞ (#1756).
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);

        Assert.Equal(0.0, model.CumulativeHazard([0.0, 0.0, lambda], Math.Exp(-30.0)));
    }

    [Fact]
    public void A_generalized_gamma_at_a_lambda_of_zero_answers_at_a_time_of_zero_and_of_infinity()
    {
        // λz read 0·∞ there and fell to the gamma at an infinite shape, whose chi-squared df threw (#1756). At λ = 0
        // lifelines takes the log-normal's form, unclipped.
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);

        Assert.Equal(0.0, model.CumulativeHazard([0.0, 0.0, 0.0], 0.0));
        Assert.Equal(double.PositiveInfinity, model.CumulativeHazard([0.0, 0.0, 0.0], double.PositiveInfinity));
        Assert.Equal(-Internal.Special.NormalLogSf(15.0), model.CumulativeHazard([0.0, 0.0, 0.0], Math.Exp(15.0)), 1e-12);
    }

    [Fact]
    public void A_generalized_gamma_survival_near_one_keeps_its_cumulative_hazard_past_the_expansion()
    {
        // At λ = 0.3 past the expansion's reach the log of a tail near one kept H = 2.5746e-13 for scipy's 2.5748e-13,
        // and log(1 − S) curved by +4856 for −231.6 (#1756).
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);
        var at = Internal.UnivariateModel.Variables([0.0, 0.0, 0.3]);

        Assert.Equal(2.5747870e-13, model.CumulativeHazard(at, 1.2532244875570633e-05).Value, 1e-19);
        Assert.True(model.LogOneMinusSurvival(at, 1.2532244875570633e-05).Hessian[8] < 0.0);
    }

    [Fact]
    public void A_weibull_interval_fit_steps_past_a_point_whose_derivatives_overflowed()
    {
        // A step took ρ from 944 to 1280, where (U/λ)^ρ overflowed: the likelihood stayed finite, its Hessian read NaN, and
        // every direction after it, where the fit main and lifelines solve has its maximum at ρ = 958.6 (#1756).
        double[] lower = [0.9997, 0.771, 0.84, 0.9984, 0.9999, 0.9999, 0.548, 0.596, 1.0005, 0.716, 1.002, 1.0014, 0.9999, 1.0003, 1.0003, 0.999, 0.9998, 0.9983, 1.0018];
        double[] upper = [0.9997, 1.838, 1.39, 0.9984, 0.9999, 0.9999, 1.951, 1.828, 1.0005, 1.718, 1.002, 1.0014, 0.9999, 1.0003, 1.0003, 0.999, 0.9998, 0.9983, 1.0018];

        ParametricSummary fit = ParametricSurvival.FitIntervalCensored(lower, upper, ParametricModel.Weibull);

        Assert.Equal(1.0006296, fit.Parameters[0], 1e-6);
        Assert.Equal(958.63, fit.Parameters[1], 1e-2);
        Assert.Equal(74.81099, fit.LogLikelihood, 1e-5);
    }

    [Theory]
    [InlineData(1e-9)]
    [InlineData(1e-12)]
    public void An_exponential_scale_far_below_one_reaches_its_closed_form(double unit)
    {
        // Steps measured against one stopped a scale of 1e-9 at 5e-9 of its maximum, and at 1e-12, where the Hessian stays
        // positive along the approach, the fit threw; the maximum is the durations' sum over the events (#1756).
        double[] durations = [.. Enumerable.Range(1, 200).Select(i => unit * Math.Pow((i * 0.618034) % 1.0 * 3.0, 1.0 / 1.7))];
        bool[] events = [.. durations.Select((_, i) => i % 5 != 0)];
        double expected = durations.Sum() / events.Count(e => e);

        ParametricSummary fit = ParametricSurvival.Fit(durations, events, ParametricModel.Exponential);

        Assert.Equal(expected, fit.Parameters[0], 1e-12 * expected);
    }

    [Theory]
    [InlineData(ParametricModel.Weibull)]
    [InlineData(ParametricModel.GeneralizedGamma)]
    public void Intervals_sharing_a_span_have_no_maximum(ParametricModel model)
    {
        // Every interval holds [1.19, 1.5]: the likelihood reaches one only for a point mass there, and a Weibull ρ of 783
        // at a standard error of 6e7 read as its maximum (#1756).
        double[] lower = [.. Enumerable.Range(0, 20).Select(i => 1.0 + (0.01 * i))];
        double[] upper = [.. Enumerable.Range(0, 20).Select(i => 1.5 + (0.02 * i))];

        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.FitIntervalCensored(lower, upper, model));
    }

    [Fact]
    public void Intervals_of_a_small_total_weight_keep_their_maximum()
    {
        // Weights of 1e-11 scale the log-likelihood to within 1e-9 of zero, and a fixed bound read a proper maximum as
        // the supremum a point mass reaches (#1756).
        double[] lower = [0.5, 1.0, 2.0, 0.2, 1.5, 3.0, 0.8, 2.5, 1.2, 0.3, 4.0, 0.7];
        double[] upper = [1.5, 2.0, 3.5, 1.0, 2.5, 5.0, 1.8, 4.0, 2.2, 1.1, 6.0, 1.6];

        ParametricSummary unit = ParametricSurvival.FitIntervalCensored(lower, upper, ParametricModel.Weibull);
        ParametricSummary small = ParametricSurvival.FitIntervalCensored(
            lower, upper, [.. lower.Select(_ => 1e-11)], [], ParametricModel.Weibull);

        Assert.Equal(unit.Parameters[0], small.Parameters[0], 1e-9 * unit.Parameters[0]);
        Assert.Equal(unit.Parameters[1], small.Parameters[1], 1e-9 * unit.Parameters[1]);
        Assert.Equal(1e-11 * unit.LogLikelihood, small.LogLikelihood, 1e-20);
    }

    [Fact]
    public void Narrow_intervals_far_below_the_scale_reach_the_exact_maximum()
    {
        // The maximum of Σ log(S(L) − S(U)) in 60 digits: two survivals near one, differenced, left their rounding in
        // the likelihood, and the fit stopped at 5369.10 (Review A, pass 35).
        double[] lower = [.. Enumerable.Range(0, 91).Select(i => Math.Pow(10, -4 + (9.0 * i / 90.0)))];
        double[] upper = [.. lower.Select(l => l * 1.01)];

        ParametricSummary fit = ParametricSurvival.FitIntervalCensored(lower, upper, ParametricModel.Exponential);

        Assert.Equal(5369.23764602342, fit.Parameters[0], 1e-9 * 5369.0);
    }

    [Fact]
    public void A_generalized_gamma_on_log_normal_quantiles_reaches_the_log_normal_fit()
    {
        // The maximum sits at λ = 0, the log-normal: through the gamma at shape 1/λ² the log-likelihood near it read to
        // 1e-5 on 10,000 times and its gradient to noise, and the fit threw (#1222).
        double[] durations = [.. Enumerable.Range(0, 40).Select(i => Math.Exp(0.5 + (0.8 * Distributions.NormalIsf((i + 0.5) / 40))))];
        bool[] events = [.. durations.Select(_ => true)];

        ParametricSummary fit = ParametricSurvival.Fit(durations, events, ParametricModel.GeneralizedGamma);
        ParametricSummary logNormal = ParametricSurvival.Fit(durations, events, ParametricModel.LogNormal);

        Assert.Equal(logNormal.Parameters[0], fit.Parameters[0], 1e-9);
        Assert.Equal(Math.Log(logNormal.Parameters[1]), fit.Parameters[1], 1e-9);
        Assert.Equal(0.0, fit.Parameters[2], 1e-9);
        Assert.Equal(logNormal.LogLikelihood, fit.LogLikelihood, 1e-9);
    }

    [Fact]
    public void The_generalized_gamma_hazards_keep_a_finite_hessian_at_a_tiny_time()
    {
        // In x, log Q's second derivative of order x^(a−2) overflowed at x = 1e-169 before the chain rule took it back,
        // and the Newton direction fell to the gradient's (#1222).
        Jet[] parameters = UnivariateModel.Variables([7.921295474, -0.7365014317, 10.2463962]);
        var model = UnivariateModel.For(ParametricModel.GeneralizedGamma, []);

        Assert.All(model.CumulativeHazard(parameters, 4.105726723650614e-05).Hessian, h => Assert.True(double.IsFinite(h)));
        Assert.All(model.LogHazard(parameters, 4.105726723650614e-05).Hessian, h => Assert.True(double.IsFinite(h)));
    }

    [Fact]
    public void Entries_must_be_one_time_per_subject_before_its_duration()
    {
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => BreslowFlemingHarrington.Estimate(Durations, Events, [1.0])).ParamName);
        Assert.Equal("entries", Assert.Throws<ArgumentException>(
            () => BreslowFlemingHarrington.Estimate(Durations, Events, [.. Durations.Select(d => d + 1.0)])).ParamName);
    }

    [Fact]
    public void A_time_only_entrants_reach_adds_no_hazard()
    {
        // lifelines' increment reads cum_[−1] at an empty risk set and gives 0.0588 at time 1; no event, no hazard (#1224).
        SurvivalCurve curve = BreslowFlemingHarrington.Estimate([0.0, 3.0, 2.0, 2.0], [true, false, true, true], [0.0, 1.0, 1.0, 1.0]);

        Assert.Equal(Math.Exp(-1.0), curve.Survival[Array.FindIndex(curve.Steps, s => s.Time > 0.5 && s.Time < 1.5)], 1e-15);
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
        // w against a 330-digit inversion of the gamma tail at shape 1/λ², to 4 ulps; Newton on the tail itself stopped at
        // 39.8 for 26.5, and on Φ̄'s slope alone at λ = −0.049 never settled (#1729's Review A).
        var model = Internal.UnivariateModel.For(ParametricModel.GeneralizedGamma, []);
        double expected = Math.Exp(1.3 + (0.7 * w));

        Assert.Equal(w, Internal.GeneralizedGammaQuantile.Standardized(lambda, probability), 4.0 * (Math.BitIncrement(w) - w));
        Assert.Equal(expected, model.Percentile([1.3, Math.Log(0.7), lambda], probability), 4e-15 * w * expected);
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
