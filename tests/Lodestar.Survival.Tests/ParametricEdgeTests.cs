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
    public void A_generalized_gamma_ridge_that_stalls_is_no_maximum()
    {
        // The likelihood rises towards a power law bounded at the largest time as λ grows: given 20,000 steps the search
        // stalled at λ = 78,991, its Newton step still moving λ by 1.6%, and read it as a maximum (Review A, pass 40).
        double[] durations =
        [
            357.1342123205106, 310.29693857355835, 357.1342123205106, 333.71557544703444, 99.52920671227344, 216.62239107965397,
            169.78511733270173, 333.71557544703444, 380.5528491939867, 345.4248938837725, 333.71557544703444, 286.87830170008226,
        ];
        bool[] events = [.. durations.Select(_ => true)];

        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.Fit(
            durations, events, ParametricModel.GeneralizedGamma, new ParametricOptions { MaximumIterations = 20_000 }));
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

    [Fact]
    public void A_log_normal_on_a_ridge_past_lifelines_maximum_still_reaches_it()
    {
        // Levenberg's shift differs between the centred and the given design: the centred path climbed the ridge and threw,
        // and lifelines answers LL −8.37732 at these coefficients (#1756).
        double[] design =
        [
            -0.2380470205015063, 1.60939659048389, 1.0188683154501093, 0.462284982842748, -1.3503809778145424,
            1.0355135608882315, 0.7597354022032026, -0.7474139734679248, 0.7418103476925384, 0.4348892987391184,
            0.4311052995239606, 0.9533886848223703, -1.3136655247791413, -0.35574339181091824, 0.7041242474624168,
            1.1927004638788823, 1.1898954509481627, -0.23582068841339346, -0.968577181257308, 0.35745971349869154,
            -0.13390798797289893, -0.5083226850568426, 0.10068779164360936, 0.5543150841954406, 1.070121567791913,
            -0.4791094908146961, -0.35664334143649806, 0.4493197061924704, -1.5945661968566824, 0.8690640279802874,
        ];
        double[] durations =
        [
            0.8187578540411377, 2.041901578247466, 1.433417322100781, 1.1342425443202773, 0.5006779485583229,
            2.109858217934063, 1.2097356273401008, 0.6865134460771798, 1.2263599552289748, 1.1715625009489028,
            1.4062002542776226, 1.8198785116817036, 0.48285919075560846, 0.7928935075088189, 1.4873540545354043,
            1.4436080002648521, 1.9806274139802003, 0.7636600157638063, 0.629446089481188, 1.118642171601336,
            0.6933321793031862, 0.5466175816968096, 0.9628063065957387, 1.1319350223053228, 1.3468258325551041,
            0.6591608051518619, 0.7682191678453633, 1.1314536420579913, 0.4737959916601253, 1.2398904497908376,
        ];
        bool[] events = [false, false, false, true, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true, false, false, false, false, true, false];

        AftSummary fit = AcceleratedFailureTime.FitLeftCensored(
            design, durations, events, 1, AftModel.LogNormal, new AftOptions { Ancillary = true });

        Assert.Equal(-8.37732357, fit.LogLikelihood, 1e-7);
        double[] lifelines = [0.55865, -0.39876, -0.12332, -1.33082];
        foreach ((double expected, double actual) in lifelines.Zip(fit.Coefficients))
        {
            Assert.Equal(expected, actual, 1e-4);
        }
    }

    [Fact]
    public void A_weibull_whose_small_parameter_would_creep_reaches_lifelines_maximum()
    {
        // A coefficient of 5e-4 shifted in its own scale crept up a ridge to −23.40; lifelines answers −23.6263 (#1756).
        double[] design = [-275.78121429717436, -1079.7572753820175, 2087.72812886874, 1650.8612851966636, 1037.6937834841153, -210.03168371942206, -708.4228445540385, -326.7053911262493];
        double[] durations = [135289.41273164985, 173649.010424603, 81065.36369545985, 194990.05228414203, 156177.17903008114, 151497.46681698115, 112740.02092910626, 123235.5924092323];
        bool[] events = [false, true, false, false, false, false, false, true];

        AftSummary fit = AcceleratedFailureTime.FitLeftCensored(
            design, durations, events, 1, AftModel.Weibull, new AftOptions { Ancillary = true });

        Assert.Equal(-23.6263, fit.LogLikelihood, 1e-4);
    }

    [Fact]
    public void A_penalised_fit_answers_where_only_its_null_model_has_no_maximum()
    {
        // The univariate fit only seeds this one: its refusal threw for a penalised maximum lifelines answers (#1756).
        double[] design = [0.787169346729168, 0.9153382943149029, 0.2464380445098409, -0.6365964997733428, 0.87756504743247, -0.15412383996746148, -0.5928260697928847, 0.053338385535824644];
        double[] lower = [0.5009282117106063, 0.0, 0.7047609995727324, 0.5660877954859419, 0.597456740569629, 0.42830699079269746, 0.25934998277601734, 0.6915918183216493];
        double[] upper = [2.4044305787187588, 0.9898525257891915, 1.3329906202548336, 1.277708569991074, 1.1130668513472484, 2.0805506748300227, 1.2898503946630473, 1.4044580020652802];

        AftSummary fit = AcceleratedFailureTime.FitIntervalCensored(design, lower, upper, 1, AftModel.Weibull, new AftOptions { Penalizer = 0.1 });

        Assert.Equal(-1.95846489, fit.LogLikelihood, 1e-7);
        Assert.Equal(-0.122203, fit.Coefficients[0], 1e-5);
        Assert.True(double.IsNaN(fit.NullLogLikelihood));
    }

    [Fact]
    public void An_exponential_on_very_wide_intervals_keeps_its_maximum()
    {
        // 60 digits. Every interval holds [2, 1e12], so a Weibull can concentrate and has no maximum; an exponential cannot, and
        // one with a likelihood of 1 − 1e-10 was refused as a point mass (#1756).
        double[] lower = [.. Enumerable.Range(0, 20).Select(i => 1.0 + (i / 20.0))];
        double[] upper = [.. lower.Select((l, i) => l * 1e12 * (1.0 + (i / 20.0)))];

        ParametricSummary fit = ParametricSurvival.FitIntervalCensored(lower, upper, ParametricModel.Exponential);

        Assert.Equal(41083200911.64067, fit.Parameters[0], 1e-8 * 4.1e10);
        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.FitIntervalCensored(lower, upper, ParametricModel.Weibull));
    }

    [Fact]
    public void An_exponential_bounded_by_intervals_open_on_either_side_keeps_its_maximum()
    {
        // e^(−1/λ)/(1 − e^(−1/λ)) = 1e-11 at the maximum, λ = 1/log(1 + 1e11), at a likelihood within 1e-9 of one; no
        // interval was bounded on both sides, and the fit was refused as a point mass (Review A, pass 41).
        double infinity = double.PositiveInfinity;

        ParametricSummary fit = ParametricSurvival.FitIntervalCensored([0.0, 0.0, 1e-11, 1e-11], [1.0, 1.0, infinity, infinity], ParametricModel.Exponential);

        Assert.Equal(1.0 / Math.Log(1.0 + 1e11), fit.Parameters[0], 1e-9);
        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.FitIntervalCensored([0.0, 0.0], [1.0, 2.0], ParametricModel.Exponential));
        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.FitIntervalCensored([1.0, 2.0], [infinity, infinity], ParametricModel.Exponential));
    }

    [Fact]
    public void An_exponential_whose_open_intervals_enter_at_their_lower_ends_has_no_maximum()
    {
        // Entered at L, [L, ∞) is S(L)/S(L) = 1 at any rate, and the rate runs off with the two [0, 1]: read as a maximum at
        // λ = 0.0217, a standard error of 3.3e6 and a likelihood of 1 − 1e-18 (Review A, pass 44).
        double infinity = double.PositiveInfinity;

        Assert.Throws<InvalidOperationException>(() => ParametricSurvival.FitIntervalCensored(
            [0.0, 0.0, 1.0, 1.0], [1.0, 1.0, infinity, infinity], [], [0.0, 0.0, 1.0, 1.0], ParametricModel.Exponential));
    }

    [Theory]
    [InlineData(1e-11)]
    [InlineData(1e-30)]
    public void A_generalized_gamma_does_not_depend_on_the_scale_of_its_weights(double weight)
    {
        // A shift and a stationarity bound floored at one treated a weight-scaled likelihood as flat: 90 steps of 1e-3 (#1756).
        double[] durations = [.. Enumerable.Range(0, 60).Select(i => 0.01 - (Math.Log(((i * 37 % 61) + 0.5) / 61.0) * Math.Exp(0.6 * Math.Sin(i))))];
        bool[] events = [.. durations.Select((_, i) => i % 5 != 0)];

        ParametricSummary unit = ParametricSurvival.Fit(durations, events, ParametricModel.GeneralizedGamma);
        ParametricSummary scaled = ParametricSurvival.Fit(
            durations, events, [.. durations.Select(_ => weight)], [], ParametricModel.GeneralizedGamma);

        for (int j = 0; j < unit.Parameters.Count; j++)
        {
            Assert.Equal(unit.Parameters[j], scaled.Parameters[j], 1e-7);
        }
    }

    [Fact]
    public void A_piecewise_exponential_does_not_depend_on_the_unit_of_its_times()
    {
        // From lifelines' start of one the rates crept down 11% a step, and times of order 1e-3 spent the budget (Review A, pass 38).
        double[] durations = [0.00084137, 0.00104349, 0.00114354, 0.00092381, 0.00096535, 0.0010278, 0.00095297, 0.00090764, 0.00081868, 0.0010062, 0.00102728, 0.00100317];
        bool[] events = [false, true, true, true, false, false, true, true, true, true, false, true];

        ParametricSummary fit = ParametricSurvival.Fit(
            durations, events, ParametricModel.PiecewiseExponential, new ParametricOptions { Breakpoints = [0.00100317] });

        Assert.Equal(0.002842901728, fit.Parameters[0], 1e-11);
        Assert.Equal(5.840897127e-05, fit.Parameters[1], 1e-13);
        Assert.Equal(53.77107446, fit.LogLikelihood, 1e-7);
    }

    [Fact]
    public void A_start_on_the_clip_restarts_on_the_data()
    {
        // lifelines' start left every interval on the 1e-25 clip, an all-zero gradient read as stationary; lifelines' -52.611025658 (Review A, pass 38).
        double[] times = [.. Enumerable.Range(0, 30).Select(i => 0.01 * Math.Exp(0.2 * Math.Sin((i * 1.7) + 0.3)))];

        ParametricSummary fit = ParametricSurvival.FitIntervalCensored(
            [.. times.Select(t => t * 0.95)], [.. times.Select(t => t * 1.05)], ParametricModel.GeneralizedGamma);

        Assert.Equal(-52.611025658, fit.LogLikelihood, 1e-7);
        Assert.Equal(-4.61328, fit.Parameters[0], 1e-4);
    }

    [Fact]
    public void An_exponential_does_not_spend_its_budget_on_the_unit_of_its_times()
    {
        // From lifelines' start of one, times of order 1e-12 took some 60 steps (Review A, pass 38).
        double[] times = [.. Enumerable.Range(1, 12).Select(i => (double)i)];
        var options = new ParametricOptions { MaximumIterations = 10 };

        ParametricSummary unit = ParametricSurvival.FitIntervalCensored(
            [.. times.Select(t => t * 0.9)], [.. times.Select(t => t * 1.1)], ParametricModel.Exponential, options);
        ParametricSummary tiny = ParametricSurvival.FitIntervalCensored(
            [.. times.Select(t => t * 0.9e-12)], [.. times.Select(t => t * 1.1e-12)], ParametricModel.Exponential, options);

        Assert.Equal(unit.Parameters[0], tiny.Parameters[0] * 1e12, 1e-9);
    }

    [Fact]
    public void An_accelerated_failure_time_fit_keeps_the_higher_of_its_two_searches()
    {
        // The centred search alone stopped at -53.3147 where lifelines reaches -53.1097507940 (Review A, pass 38).
        double[] design =
        [
            1.5539501066091188, 4.255996426383764, 2.0192957361716646, 2.8787292466376613, 3.768072686859832, 4.7487115869850935,
            4.366155386150279, 3.5890187119109824, 4.448028783769326, 1.9501200194970667, 3.6621428008889687, 1.3759880288310127,
            2.6410531285822576, 2.945482514864312, 3.2663703092743166, 3.2293099596219577, 2.6881579905850095, 2.648966364471117,
            2.1615317061040837, 3.889337452688312, 1.9454090133513997, 3.6748006829047584, 3.361563939892321, 3.1547694867421487,
            2.0919942513035186, 4.90616512849624, 2.317115158319499, 2.3489124764101352, 2.7753826590399684, 3.5784924548868653,
            3.4817840444072563, 4.179408546571189, 3.0942718836489074, 3.3030823390404045, 3.537009976157208, 2.4850468161442887,
            1.6578136525111644, 1.3696875235185817, 2.341497997738938, 3.9720769497936717, 4.160751687682559, 2.555164941069346,
            4.897504336224651, 2.8827747057335786, 0.5190402854941971,
        ];
        double[] lower = [2.1305252104245866, 8.246859242813306, 10.161106463410613, 8.145664924890959, 7.155309013263404, 77.98020207319476, 5.074214745189831, 12.739398208275107, 11.002271978428995, 13.149190097857076, 20.68453123758736, 27.40521887897399, 11.00255797134275, 39.328566445098616, 102.59997763203567];
        double[] upper = [3.160128092276404, 8.246859242813306, 10.161106463410613, 8.145664924890959, 7.155309013263404, 77.98020207319476, 5.074214745189831, 12.739398208275107, 15.621649960705469, 13.149190097857076, 20.68453123758736, 27.40521887897399, 11.00255797134275, 39.328566445098616, 102.59997763203567];

        AftSummary fit = AcceleratedFailureTime.FitIntervalCensored(
            design, lower, upper, featureCount: 3, AftModel.Weibull, new AftOptions { Ancillary = true, Penalizer = 0.1 });

        Assert.Equal(-53.1097507940, fit.LogLikelihood, 1e-7);
    }

    [Theory]
    [InlineData(1e-9)]
    [InlineData(1e-20)]
    public void An_accelerated_failure_time_fit_keeps_its_higher_maximum_under_tiny_weights(double scale)
    {
        // One absolute floor on the two searches' margin read -15.1584 for -14.6348 under weights of 1e-9 (Review A, pass 39).
#pragma warning disable CA5394 // A seeded stream makes the sample the review found; nothing here is security-sensitive.
        var random = new Random(777001);
        double Gaussian() => Math.Sqrt(-2.0 * Math.Log(1.0 - random.NextDouble())) * Math.Cos(2.0 * Math.PI * random.NextDouble());
        const int Count = 25;
        const int Features = 2;
        double[] design = new double[Count * Features];
        for (int i = 0; i < design.Length; i++)
        {
            design[i] = Gaussian();
        }

        double[] times = new double[Count];
        bool[] events = new bool[Count];
        for (int i = 0; i < Count; i++)
        {
            times[i] = Math.Exp((0.5 * (design[i * Features] + design[(i * Features) + 1])) + (0.7 * Gaussian()));
            events[i] = random.NextDouble() < 0.75;
        }

        double[] weights = times.Select(_ => 0.5 + random.NextDouble()).ToArray();
#pragma warning restore CA5394
        var options = new AftOptions { Ancillary = true };

        AftSummary whole = AcceleratedFailureTime.Fit(design, times, events, weights, [], Features, AftModel.LogLogistic, options);
        AftSummary tiny = AcceleratedFailureTime.Fit(
            design, times, events, weights.Select(w => w * scale).ToArray(), [], Features, AftModel.LogLogistic, options);

        Assert.Equal(whole.LogLikelihood, tiny.LogLikelihood / scale, 1e-7);
    }
}
