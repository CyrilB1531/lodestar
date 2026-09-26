using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>lifelines' parametric univariate fitters, right-, left- and interval-censored, fitted to their maximum.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines</c> 0.30.3's <c>ExponentialFitter</c>, <c>WeibullFitter</c>, <c>LogNormalFitter</c>,
/// <c>LogLogisticFitter</c>, <c>PiecewiseExponentialFitter</c> and <c>GeneralizedGammaFitter</c>: the same parameters and
/// likelihoods, with weights and delayed entry. lifelines stops its optimiser short of the maximum; this reaches it,
/// which is what the corpus holds. Thread-safe.
/// </remarks>
public static class ParametricSurvival
{
    private static readonly ParametricOptions Defaults = new();

    /// <summary>Fits a model to right-censored durations, lifelines' <c>fit</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="durations">One positive duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">The spans differ in length or are empty, a duration is not positive and finite, or the piecewise model has no breakpoint.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit Fit(
        ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ParametricOptions? options = null) =>
        Fit(model, durations, eventObserved, [], [], options);

    /// <summary>Fits a model to weighted, possibly late-entering, right-censored durations: lifelines' <c>weights</c> and <c>entry</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="durations">One positive duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration, or empty for none.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit Fit(
        ParametricModel model,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        ParametricOptions? options = null) =>
        Run(model, Samples.Build(Censoring.Right, durations, durations, eventObserved, weights, entries, nameof(durations)), options);

    /// <summary>Fits a model to left-censored durations, lifelines' <c>fit_left_censoring</c>: a censored subject had the event by its time.</summary>
    /// <param name="model">The model.</param>
    /// <param name="durations">One positive duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">As <see cref="Fit(ParametricModel, ReadOnlySpan{double}, ReadOnlySpan{bool}, ParametricOptions)"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit FitLeftCensored(
        ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ParametricOptions? options = null) =>
        FitLeftCensored(model, durations, eventObserved, [], [], options);

    /// <summary>Fits a model to weighted, possibly late-entering, left-censored durations.</summary>
    /// <param name="model">The model.</param>
    /// <param name="durations">One positive duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative, or empty for none.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">As the weighted right-censored overload.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit FitLeftCensored(
        ParametricModel model,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        ParametricOptions? options = null) =>
        Run(model, Samples.Build(Censoring.Left, durations, durations, eventObserved, weights, entries, nameof(durations)), options);

    /// <summary>Fits a model to interval-censored times, lifelines' <c>fit_interval_censoring</c>: each event somewhere in <c>[lower, upper]</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time. Equal bounds are an observed event.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">The spans differ in length or are empty, a bound is NaN or negative, an upper bound is below its lower one, or the piecewise model has no breakpoint.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit FitIntervalCensored(
        ParametricModel model, ReadOnlySpan<double> lower, ReadOnlySpan<double> upper, ParametricOptions? options = null) =>
        FitIntervalCensored(model, lower, upper, [], [], options);

    /// <summary>Fits a model to weighted, possibly late-entering, interval-censored times.</summary>
    /// <param name="model">The model.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative, or empty for none.</param>
    /// <param name="options">The level, the iteration budget and the piecewise breakpoints; <see langword="null"/> for the defaults.</param>
    /// <returns>The fitted parameters with their inference, and the fitted curves.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static ParametricFit FitIntervalCensored(
        ParametricModel model,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        ParametricOptions? options = null) =>
        Run(model, Samples.Build(Censoring.Interval, lower, upper, [], weights, entries, nameof(upper)), options);

    /// <summary>Tests whether two fitted survival functions differ at one time, lifelines' <c>survival_difference_at_fixed_point_in_time_test</c> on two parametric fitters.</summary>
    /// <param name="time">The time both fits are read at; positive and finite.</param>
    /// <param name="fitA">The first fit.</param>
    /// <param name="fitB">The second fit.</param>
    /// <returns>The chi-squared statistic on one degree of freedom and its upper-tail p-value.</returns>
    /// <exception cref="ArgumentNullException">A fit is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="time"/> is not positive and finite.</exception>
    /// <remarks>
    /// long-comment: lifelines' variance, which is not the log scale's.
    /// The statistic is <c>(log(−log S_A) − log(−log S_B))²</c> over <c>σ²_A/(log S_A)² + σ²_B/(log S_B)²</c>, as
    /// <see cref="KaplanMeier.CompareAt"/> computes it. For a parametric fit lifelines takes <c>σ²</c> to be the
    /// delta-method variance of <c>S</c> itself, where a Kaplan-Meier curve gives Greenwood's variance of <c>log S</c>;
    /// this keeps lifelines' reading, so its statistic is the one lifelines reports.
    /// </remarks>
    public static TestResult CompareAt(double time, ParametricFit fitA, ParametricFit fitB)
    {
        Guard.NotNull(fitA);
        Guard.NotNull(fitB);
        if (!(time > 0.0) || double.IsInfinity(time))
        {
            throw new ArgumentOutOfRangeException(nameof(time), time, "A time is positive and finite.");
        }

        (double survivalA, double varianceA) = fitA.SurvivalWithVariance(time);
        (double survivalB, double varianceB) = fitB.SurvivalWithVariance(time);
        double logA = Math.Log(survivalA);
        double logB = Math.Log(survivalB);
        double gap = Math.Log(-logA) - Math.Log(-logB);
        double statistic = gap * gap / ((varianceA / (logA * logA)) + (varianceB / (logB * logB)));
        return new TestResult(statistic, Distributions.ChiSquaredSf(statistic, 1.0));
    }

    internal static ParametricFit Run(ParametricModel model, CensoredSample sample, ParametricOptions? options)
    {
        if (model < ParametricModel.Exponential || model > ParametricModel.GeneralizedGamma)
        {
            throw new ArgumentOutOfRangeException(nameof(model), model, "The model names no parametric model.");
        }

        ParametricOptions settings = options ?? Defaults;
        if (model == ParametricModel.PiecewiseExponential && settings.Breakpoints.Length == 0)
        {
            throw new ArgumentException("The piecewise exponential needs its breakpoints in ParametricOptions.", nameof(options));
        }

        UnivariateModel shape = UnivariateModel.For(model, [.. settings.Breakpoints]);
        (double[] parameters, double logLikelihood, double[] information) = ParametricNewton.Maximize(
            theta => ParametricLikelihood.LogLikelihood(sample, new Univariate(shape, theta), theta.Length),
            shape.Start(sample.StartTimes),
            shape.LowerBounds,
            settings.MaximumIterations);
        return new ParametricFit(model, shape, parameters, logLikelihood, information, settings.ConfidenceLevel);
    }

    /// <summary>Every observation shares the model's parameters, as jets over them.</summary>
    private sealed class Univariate(UnivariateModel shape, double[] theta) : IObservationModel
    {
        private readonly Jet[] _variables = UnivariateModel.Variables(theta);

        public Jet CumulativeHazard(int observation, double t) => shape.CumulativeHazard(_variables, t);

        public Jet LogHazard(int observation, double t) => shape.LogHazard(_variables, t);

        public Jet LogOneMinusSurvival(int observation, double t) => shape.LogOneMinusSurvival(_variables, t);
    }
}
