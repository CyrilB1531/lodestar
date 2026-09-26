using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>lifelines' parametric accelerated failure time regressions, right-, left- and interval-censored, fitted to their maximum.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines</c> 0.30.3's <c>WeibullAFTFitter</c>, <c>LogNormalAFTFitter</c> and
/// <c>LogLogisticAFTFitter</c>: the primary parameter's log is linear in the covariates, and the ancillary one's log is
/// an intercept, or linear in the same covariates when <see cref="AftOptions.Ancillary"/> says so. The design is
/// row-major, one row per subject, its columns in the order lifelines sorts them into. Thread-safe.
/// </remarks>
public static class AcceleratedFailureTime
{
    private static readonly AftOptions Defaults = new();

    /// <summary>Fits a model to right-censored durations, lifelines' <c>fit</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject; no intercept column, which <see cref="AftOptions.FitIntercept"/> adds.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="featureCount">The number of covariates, the design's row length.</param>
    /// <param name="options">The level, the intercept, the ancillary model, the penalty and the robust errors; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">The spans do not match the subjects, a value is not finite, a duration is not positive, or the primary parameter has no column at all.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    public static AftSummary Fit(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        AftOptions? options = null) =>
        Fit(model, design, durations, eventObserved, [], [], featureCount, options);

    /// <summary>Fits a model to weighted, possibly late-entering, right-censored durations: lifelines' <c>weights_col</c> and <c>entry_col</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    // S107: lifelines' columns — covariates, times, events, weights and entries — with the design's width and the settings.
#pragma warning disable S107
    public static AftSummary Fit(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftOptions? options = null) =>
        Run(model, design, Samples.Build(Censoring.Right, durations, durations, eventObserved, weights, entries, nameof(durations)), featureCount, options);
#pragma warning restore S107

    /// <summary>Fits a model to left-censored durations, lifelines' <c>fit_left_censoring</c>: a censored subject had the event by its time.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As <see cref="Fit(AftModel, ReadOnlySpan{double}, ReadOnlySpan{double}, ReadOnlySpan{bool}, int, AftOptions)"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    public static AftSummary FitLeftCensored(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        AftOptions? options = null) =>
        FitLeftCensored(model, design, durations, eventObserved, [], [], featureCount, options);

    /// <summary>Fits a model to weighted, possibly late-entering, left-censored durations.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the weighted right-censored overload.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    // S107: as the weighted right-censored overload.
#pragma warning disable S107
    public static AftSummary FitLeftCensored(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftOptions? options = null) =>
        Run(model, design, Samples.Build(Censoring.Left, durations, durations, eventObserved, weights, entries, nameof(durations)), featureCount, options);
#pragma warning restore S107

    /// <summary>Fits a model to interval-censored times, lifelines' <c>fit_interval_censoring</c>: each event somewhere in <c>[lower, upper]</c>.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time. Equal bounds are an observed event.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions; no concordance, which lifelines does not compute here.</returns>
    /// <exception cref="ArgumentException">The spans do not match the subjects, a covariate is not finite, a bound is NaN or negative, an upper bound is below its lower one, or the primary parameter has no column at all.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    public static AftSummary FitIntervalCensored(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        int featureCount,
        AftOptions? options = null) =>
        FitIntervalCensored(model, design, lower, upper, [], [], featureCount, options);

    /// <summary>Fits a model to weighted, possibly late-entering, interval-censored times.</summary>
    /// <param name="model">The model.</param>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its upper bound, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, or its parameters are not identified.</exception>
    // S107: as the weighted right-censored overload.
#pragma warning disable S107
    public static AftSummary FitIntervalCensored(
        AftModel model,
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftOptions? options = null) =>
        Run(model, design, Samples.Build(Censoring.Interval, lower, upper, [], weights, entries, nameof(upper)), featureCount, options);
#pragma warning restore S107

    private static AftSummary Run(AftModel model, ReadOnlySpan<double> design, CensoredSample sample, int featureCount, AftOptions? options)
    {
        if (model < AftModel.Weibull || model > AftModel.LogLogistic)
        {
            throw new ArgumentOutOfRangeException(nameof(model), model, "The model names no accelerated failure time model.");
        }

        AftOptions settings = options ?? Defaults;
        double[] x = AftDesign.Validate(design, sample.Count, featureCount, settings.FitIntercept);
        AftShape shape = AftShape.For(model);
        AftDesign blocks = AftDesign.For(featureCount, settings);
        (double[] deviations, bool[] unpenalised) = blocks.Scales(x, sample.Count);
        var penalty = new Ridge(settings.Penalizer, deviations, unpenalised, sample.TotalWeight);

        // lifelines starts from the univariate fit, each parameter the intercept of its block; that fit is also the null model.
        ParametricFit univariate = ParametricSurvival.Run(shape.Univariate, sample, null);
        double[] start = new double[blocks.Size];
        int primaryIntercept = Array.IndexOf(blocks.PrimaryColumns, AftDesign.Intercept);
        int ancillaryIntercept = Array.IndexOf(blocks.AncillaryColumns, AftDesign.Intercept);
        if (primaryIntercept >= 0)
        {
            start[primaryIntercept] = AftShape.Transformed(univariate.Parameters[0]);
        }

        if (ancillaryIntercept >= 0)
        {
            start[blocks.PrimaryColumns.Length + ancillaryIntercept] = AftShape.Transformed(univariate.Parameters[1]);
        }

        (double[] theta, double logLikelihood, double[] information) = ParametricNewton.Maximize(
            parameters => LogLikelihood(sample, shape, blocks, x, parameters) - penalty.Value(parameters),
            start,
            [.. Enumerable.Repeat(double.NegativeInfinity, blocks.Size)],
            settings.MaximumIterations);
        int k = theta.Length;
        double[] covariance = Cholesky.TryFactor(information, k, out double[] factor)
            ? Cholesky.Inverse(factor, k)
            : throw new InvalidOperationException("The observed information at the fit is not positive definite; the coefficients are not identified.");
        if (settings.Robust)
        {
            covariance = Sandwich(sample, shape, blocks, x, theta, penalty, covariance);
        }

        double concordance = sample.Censoring == Censoring.Interval ? double.NaN : Concordance(sample, shape, blocks, x, theta);
        return AftSummary.Build(model, shape, blocks, (theta, covariance), (logLikelihood, univariate.LogLikelihood, concordance), settings);
    }

    /// <summary>
    /// lifelines' Huber sandwich <c>V J V</c>: <c>J</c> sums the outer products of each subject's own score — the gradient
    /// of its unweighted negative log-likelihood term plus the penalty's, at the unscaled coefficients.
    /// </summary>
    private static double[] Sandwich(
        CensoredSample sample, AftShape shape, AftDesign blocks, double[] x, double[] theta, Ridge penalty, double[] covariance)
    {
        int k = theta.Length;
        var observations = new Observations(shape, blocks, x, theta);
        double[] penaltyGradient = penalty.UnscaledGradient(theta);
        var meat = new double[k * k];
        var score = new double[k];
        var scratch = new double[k * k];
        for (int i = 0; i < sample.Count; i++)
        {
            var gradient = new double[k];
            blocks.Spread(x, i, ParametricLikelihood.Term(sample, observations, i), 1.0, gradient, scratch);
            for (int a = 0; a < k; a++)
            {
                score[a] = penaltyGradient[a] - gradient[a];
            }

            for (int a = 0; a < k; a++)
            {
                for (int b = 0; b < k; b++)
                {
                    meat[(a * k) + b] += score[a] * score[b];
                }
            }
        }

        return Multiply(Multiply(covariance, meat, k), covariance, k);
    }

    /// <summary>The summed log-likelihood over every coefficient, each subject differentiated in its two predictors alone.</summary>
    private static Jet LogLikelihood(CensoredSample sample, AftShape shape, AftDesign blocks, double[] x, double[] theta)
    {
        int k = theta.Length;
        var observations = new Observations(shape, blocks, x, theta);
        var gradient = new double[k];
        var hessian = new double[k * k];
        double value = 0.0;
        for (int i = 0; i < sample.Count; i++)
        {
            Jet term = ParametricLikelihood.Term(sample, observations, i);
            value += sample.Weights[i] * term.Value;
            blocks.Spread(x, i, term, sample.Weights[i], gradient, hessian);
        }

        return Jet.FromParts(value, gradient, hessian);
    }

    /// <summary>lifelines' <c>concordance_index_</c>: the durations against the predicted medians, a longer median a longer survival.</summary>
    private static double Concordance(CensoredSample sample, AftShape shape, AftDesign blocks, double[] x, double[] theta)
    {
        var risk = new double[sample.Count];
        for (int i = 0; i < sample.Count; i++)
        {
            (double primary, double ancillary) = blocks.Scores(x, i, theta);
            risk[i] = -shape.Percentile(primary, ancillary, 0.5);
        }

        return HarrellConcordance.Index(sample.Upper, risk, sample.Events);
    }

    private static double[] Multiply(double[] left, double[] right, int k)
    {
        var product = new double[k * k];
        for (int a = 0; a < k; a++)
        {
            for (int c = 0; c < k; c++)
            {
                double l = left[(a * k) + c];
                for (int b = 0; b < k; b++)
                {
                    product[(a * k) + b] += l * right[(c * k) + b];
                }
            }
        }

        return product;
    }

    /// <summary>
    /// lifelines' ridge penalty, <c>λ Σ β² / 2</c> on the coefficients of the covariates scaled to unit deviation,
    /// added to the mean negative log-likelihood: on the summed scale and the unscaled coefficients,
    /// <c>W λ Σ (β σ)² / 2</c>.
    /// </summary>
    private sealed class Ridge(double penalizer, double[] deviations, bool[] unpenalised, double totalWeight)
    {
        public Jet Value(double[] theta)
        {
            int k = theta.Length;
            Jet total = Jet.Constant(0.0, k);
            if (penalizer <= 0.0)
            {
                return total;
            }

            for (int j = 0; j < k; j++)
            {
                if (!unpenalised[j])
                {
                    var gradient = new double[k];
                    gradient[j] = deviations[j];
                    Jet scaled = Jet.Linear(theta[j] * deviations[j], gradient);
                    total += scaled * scaled;
                }
            }

            return total * (0.5 * penalizer * totalWeight);
        }

        /// <summary>The gradient lifelines' sandwich adds: its penalty differentiated at the unscaled coefficients, <c>λ β</c>.</summary>
        public double[] UnscaledGradient(double[] theta)
        {
            var gradient = new double[theta.Length];
            for (int j = 0; j < theta.Length; j++)
            {
                gradient[j] = unpenalised[j] ? 0.0 : penalizer * theta[j];
            }

            return gradient;
        }
    }

    /// <summary>Each subject's hazards at its own linear predictors, as jets over every coefficient.</summary>
    private sealed class Observations(AftShape shape, AftDesign blocks, double[] x, double[] theta) : IObservationModel
    {
        private int _cached = -1;
        private Jet? _primary;
        private Jet? _ancillary;

        public Jet CumulativeHazard(int observation, double t)
        {
            (Jet primary, Jet ancillary) = Predictors(observation);
            return shape.CumulativeHazard(primary, ancillary, t);
        }

        public Jet LogHazard(int observation, double t)
        {
            (Jet primary, Jet ancillary) = Predictors(observation);
            return shape.LogHazard(primary, ancillary, t);
        }

        public Jet LogOneMinusSurvival(int observation, double t)
        {
            (Jet primary, Jet ancillary) = Predictors(observation);
            return shape.LogOneMinusSurvival(primary, ancillary, t);
        }

        private (Jet Primary, Jet Ancillary) Predictors(int observation)
        {
            if (observation != _cached)
            {
                (_primary, _ancillary) = blocks.Predictors(x, observation, theta);
                _cached = observation;
            }

            return (_primary!, _ancillary!);
        }
    }
}
