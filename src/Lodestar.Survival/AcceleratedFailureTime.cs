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
    /// <param name="design">The covariates, row-major, one row per subject; no intercept column, which <see cref="AftOptions.FitIntercept"/> adds.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="featureCount">The number of covariates, the design's row length.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The level, the intercept, the ancillary model, the penalty and the robust errors; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">The spans do not match the subjects, a value is not finite, a duration is not positive, or the primary parameter has no column at all, or the design, the parameters or their square is more than one array holds.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    public static AftSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        AftModel model,
        AftOptions? options = null) =>
        Fit(design, durations, eventObserved, [], [], featureCount, model, options);

    /// <summary>Fits a model to weighted, possibly late-entering, right-censored durations: lifelines' <c>weights_col</c> and <c>entry_col</c>.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    // S107: lifelines' columns — covariates, times, events, weights and entries — with the design's width and the settings.
#pragma warning disable S107
    public static AftSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftModel model,
        AftOptions? options = null) =>
        Run(model, design, Samples.Build(Censoring.Right, durations, durations, eventObserved, weights, entries, nameof(durations)), featureCount, options);
#pragma warning restore S107

    /// <summary>Fits a model to left-censored durations, lifelines' <c>fit_left_censoring</c>: a censored subject had the event by its time.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As <see cref="Fit(ReadOnlySpan{double}, ReadOnlySpan{double}, ReadOnlySpan{bool}, int, AftModel, AftOptions)"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    public static AftSummary FitLeftCensored(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        AftModel model,
        AftOptions? options = null) =>
        FitLeftCensored(design, durations, eventObserved, [], [], featureCount, model, options);

    /// <summary>Fits a model to weighted, possibly late-entering, left-censored durations.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One positive, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration is the event's own time rather than an upper bound on it.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the weighted right-censored overload.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    // S107: as the weighted right-censored overload.
#pragma warning disable S107
    public static AftSummary FitLeftCensored(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftModel model,
        AftOptions? options = null) =>
        Run(model, design, Samples.Build(Censoring.Left, durations, durations, eventObserved, weights, entries, nameof(durations)), featureCount, options);
#pragma warning restore S107

    /// <summary>Fits a model to interval-censored times, lifelines' <c>fit_interval_censoring</c>: each event somewhere in <c>[lower, upper]</c>.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time. Equal bounds are an observed event.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions; no concordance, which lifelines does not compute here.</returns>
    /// <exception cref="ArgumentException">The spans do not match the subjects, a covariate is not finite, a bound is NaN or negative, an upper bound is below its lower one, or the primary parameter has no column at all, or the design, the parameters or their square is more than one array holds.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    public static AftSummary FitIntervalCensored(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        int featureCount,
        AftModel model,
        AftOptions? options = null) =>
        FitIntervalCensored(design, lower, upper, [], [], featureCount, model, options);

    /// <summary>Fits a model to weighted, possibly late-entering, interval-censored times.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="lower">Each interval's lower bound, non-negative.</param>
    /// <param name="upper">Each interval's upper bound, at least its lower bound; infinite for a right-censored time.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its upper bound, or empty for none.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="model">The model.</param>
    /// <param name="options">The fit's settings; <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the model's fit statistics, and its predictions.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or entry span is neither empty nor one valid value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="model"/> names no model, or <paramref name="featureCount"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge, its parameters are not identified, or every observation is a probability and the likelihood reaches one only in the limit of a point mass, so it has no maximum.</exception>
    // S107: as the weighted right-censored overload.
#pragma warning disable S107
    public static AftSummary FitIntervalCensored(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> lower,
        ReadOnlySpan<double> upper,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<double> entries,
        int featureCount,
        AftModel model,
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
        AftDesign blocks = AftDesign.For(featureCount, settings, nameof(featureCount));
        double[] columnMeans = blocks.ColumnMeans(x, sample.Count);
        (double[] deviations, bool[] unpenalised) = blocks.Scales(x, sample.Count, columnMeans);
        var penalty = new Ridge(settings.Penalizer, deviations, unpenalised, sample.TotalWeight);

        // lifelines starts from the univariate fit, each parameter the intercept of its block; that fit is also the null model.
        (double[] startParameters, double nullLogLikelihood) = NullModel(shape, sample);
        double[] start = new double[blocks.Size];
        int primaryIntercept = Array.IndexOf(blocks.PrimaryColumns, AftDesign.Intercept);
        int ancillaryIntercept = Array.IndexOf(blocks.AncillaryColumns, AftDesign.Intercept);
        if (primaryIntercept >= 0)
        {
            start[primaryIntercept] = AftShape.Transformed(startParameters[0]);
        }

        if (ancillaryIntercept >= 0)
        {
            start[blocks.PrimaryColumns.Length + ancillaryIntercept] = AftShape.Transformed(startParameters[1]);
        }

        // On the design centred and scaled, θ = Aφ and its covariance A V Aᵀ (Centring): on raw columns a covariate at 1e5 ±
        // 0.04 beside the intercept cancelled terms in x², and fits lifelines answers did not converge (#1756).
        int k = blocks.Size;
        (double[] means, int[] intercepts) = blocks.Centres(columnMeans);
        double[] toCoefficients = Centring(deviations, means, intercepts, inverse: false);
        var centred = new SearchSpace(
            toCoefficients, Transposed(toCoefficients, k), Standardised(x, sample.Count, featureCount, blocks, deviations, means),
            Apply(Centring(deviations, means, intercepts, inverse: true), start, k), OwnScale: true);
        (SearchSpace space, (double[] searched, double logLikelihood, double[] searchedInformation)) = ParametricNewton.HighestMaximum(
            centred, new SearchSpace(Identity(k), Identity(k), x, start, OwnScale: false),
            sample.TotalWeight / sample.Count,
            (within, onShift) => Search(within, sample, shape, blocks, penalty, settings.MaximumIterations, onShift));
        (toCoefficients, double[] fromCoefficients, double[] scaledDesign) = (space.ToCoefficients, space.FromCoefficients, space.Design);
        ParametricLikelihood.RequireMaximum(sample, logLikelihood);
        double[] theta = Apply(toCoefficients, searched, k);
        double[] searchedCovariance = Cholesky.TryFactor(searchedInformation, k, out double[] factor)
            ? Cholesky.Inverse(factor, k)
            : throw new InvalidOperationException("The observed information at the fit is not positive definite; the coefficients are not identified.");
        if (settings.Robust)
        {
            double[] penaltyGradient = Apply(fromCoefficients, penalty.UnscaledGradient(theta), k);
            searchedCovariance = Sandwich(sample, shape, blocks, scaledDesign, searched, penaltyGradient, searchedCovariance);
        }

        double[] covariance = Multiply(Multiply(toCoefficients, searchedCovariance, k), fromCoefficients, k);

        double concordance = sample.Censoring == Censoring.Interval ? double.NaN : Concordance(sample, shape, blocks, x, theta);
        return AftSummary.Build(model, shape, blocks, (theta, covariance), (logLikelihood, nullLogLikelihood, concordance), settings);
    }

    /// <summary>The parameters a search runs over: θ = Aφ, the transpose of A, the design as φ reads it, and φ's start.</summary>
    private sealed record SearchSpace(double[] ToCoefficients, double[] FromCoefficients, double[] Design, double[] Start, bool OwnScale);

    private static (double[], double, double[]) Search(
        SearchSpace space, CensoredSample sample, AftShape shape, AftDesign blocks, Ridge penalty, int maximumIterations, Action? onShift)
    {
        // The ridge is quadratic in θ = Aφ, so its Hessian over φ, AᵀDA, is one matrix: formed per evaluation, two dense
        // k×k products took a penalised fit over 300 covariates twice lifelines' time (Review A, pass 45).
        double[]? curvature = penalty.Active ? penalty.Curvature(space.ToCoefficients) : null;
        return ParametricNewton.Maximize(
            phi => curvature is null
                ? LogLikelihood(sample, shape, blocks, space.Design, phi)
                : LogLikelihood(sample, shape, blocks, space.Design, phi) - Ridge.Quadratic(curvature, phi),
            space.Start,
            [.. Enumerable.Repeat(double.NegativeInfinity, blocks.Size)],
            maximumIterations,
            new NewtonSettings(double.PositiveInfinity, space.OwnScale, sample.TotalWeight / sample.Count, OnShift: onShift));
    }

    private static double[] Identity(int k)
    {
        var identity = new double[k * k];
        for (int j = 0; j < k; j++)
        {
            identity[(j * k) + j] = 1.0;
        }

        return identity;
    }

    /// <summary>The univariate fit's parameters and log-likelihood, or where it has no maximum its own start and NaN: a
    /// penalty or a covariate can give the model a maximum the null model lacks, and that fit only seeds this one (#1756).</summary>
    private static (double[] Parameters, double LogLikelihood) NullModel(AftShape shape, CensoredSample sample)
    {
        try
        {
            ParametricSummary univariate = ParametricSurvival.Run(shape.Univariate, sample, null);
            return ([.. univariate.Parameters], univariate.LogLikelihood);
        }
        catch (InvalidOperationException)
        {
            return (UnivariateModel.For(shape.Univariate, []).Start(sample.StartTimes), double.NaN);
        }
    }

    /// <summary>The design with each covariate less its mean, where its block has an intercept, over its deviation.</summary>
    private static double[] Standardised(
        double[] x, int rows, int featureCount, AftDesign blocks, double[] deviations, double[] means)
    {
        var centre = new double[featureCount];
        var spread = Enumerable.Repeat(1.0, featureCount).ToArray();
        for (int j = 0; j < blocks.PrimaryColumns.Length; j++)
        {
            int column = blocks.PrimaryColumns[j];
            if (column != AftDesign.Intercept)
            {
                (centre[column], spread[column]) = (means[j], deviations[j]);
            }
        }

        var scaled = new double[x.Length];
        for (int i = 0; i < rows; i++)
        {
            for (int c = 0; c < featureCount; c++)
            {
                scaled[(i * featureCount) + c] = (x[(i * featureCount) + c] - centre[c]) / spread[c];
            }
        }

        return scaled;
    }

    /// <summary>θ = Aφ, or its inverse B: a covariate's φ is its coefficient times its deviation, and its block's intercept
    /// φ carries the coefficients times the means, so A takes θ_j = φ_j/d_j and θ_0 = φ_0 − Σ m_j φ_j/d_j, B the reverse.</summary>
    private static double[] Centring(double[] deviations, double[] means, int[] intercepts, bool inverse)
    {
        int k = deviations.Length;
        var matrix = new double[k * k];
        for (int j = 0; j < k; j++)
        {
            matrix[(j * k) + j] = inverse ? deviations[j] : 1.0 / deviations[j];
            if (intercepts[j] >= 0)
            {
                matrix[(intercepts[j] * k) + j] = inverse ? means[j] : -means[j] / deviations[j];
            }
        }

        return matrix;
    }

    private static double[] Apply(double[] matrix, double[] vector, int k)
    {
        var result = new double[k];
        for (int a = 0; a < k; a++)
        {
            for (int b = 0; b < k; b++)
            {
                result[a] += matrix[(a * k) + b] * vector[b];
            }
        }

        return result;
    }

    private static double[] Transposed(double[] matrix, int k)
    {
        var result = new double[k * k];
        for (int a = 0; a < k; a++)
        {
            for (int b = 0; b < k; b++)
            {
                result[(b * k) + a] = matrix[(a * k) + b];
            }
        }

        return result;
    }

    /// <summary>
    /// lifelines' Huber sandwich <c>V J V</c>: <c>J</c> sums the outer products of each subject's own score — the gradient
    /// of its unweighted negative log-likelihood term plus the penalty's, both over the coefficients and design given.
    /// </summary>
    private static double[] Sandwich(
        CensoredSample sample, AftShape shape, AftDesign blocks, double[] x, double[] theta, double[] penaltyGradient,
        double[] covariance)
    {
        int k = theta.Length;
        var observations = new Observations(shape, blocks, x, theta);
        var meat = new double[TableLength.Of(k, k, ResultTable.FeatureCount)];
        var score = new double[k];
        for (int i = 0; i < sample.Count; i++)
        {
            var gradient = new double[k];
            blocks.Spread(x, i, ParametricLikelihood.Term(sample, observations, i), 1.0, gradient, null);
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
        var hessian = new double[TableLength.Of(k, k, ResultTable.FeatureCount)];
        double value = 0.0;
        for (int i = 0; i < sample.Count; i++)
        {
            Jet term = ParametricLikelihood.Term(sample, observations, i);
            value += sample.Weights[i] * term.Value;
            blocks.Spread(x, i, term, sample.Weights[i], gradient, hessian);
        }

        AftDesign.Mirror(hessian, k);
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
        var product = new double[TableLength.Of(k, k, ResultTable.FeatureCount)];
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
        /// <summary>Whether the penalty is other than zero, which the search then maps through the centring.</summary>
        public bool Active => penalizer > 0.0;

        /// <summary>The penalty's Hessian over φ, θ = Aφ: AᵀDA, D the diagonal <c>W λ σ²</c> over the penalised coefficients.
        /// A holds a diagonal and each block's intercept row, so each row's few entries are read.</summary>
        public double[] Curvature(double[] toCoefficients)
        {
            int k = deviations.Length;
            var curvature = new double[k * k];
            var columns = new List<int>(k);
            for (int j = 0; j < k; j++)
            {
                if (unpenalised[j])
                {
                    continue;
                }

                double weight = penalizer * totalWeight * deviations[j] * deviations[j];
                columns.Clear();
                for (int a = 0; a < k; a++)
                {
                    if (toCoefficients[(j * k) + a] != 0.0)
                    {
                        columns.Add(a);
                    }
                }

                foreach (int a in columns)
                {
                    foreach (int b in columns)
                    {
                        curvature[(a * k) + b] += toCoefficients[(j * k) + a] * weight * toCoefficients[(j * k) + b];
                    }
                }
            }

            return curvature;
        }

        /// <summary>The penalty over φ, <c>φᵀMφ / 2</c>, as a jet: its gradient Mφ, its Hessian M.</summary>
        public static Jet Quadratic(double[] curvature, double[] phi)
        {
            int k = phi.Length;
            var gradient = new double[k];
            double value = 0.0;
            for (int a = 0; a < k; a++)
            {
                double sum = 0.0;
                for (int b = 0; b < k; b++)
                {
                    sum += curvature[(a * k) + b] * phi[b];
                }
                gradient[a] = sum;
                value += 0.5 * phi[a] * sum;
            }

            return Jet.FromParts(value, gradient, [.. curvature]);
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

        public Jet LogDensity(int observation, double t)
        {
            (Jet primary, Jet ancillary) = Predictors(observation);
            return shape.LogDensity(primary, ancillary, t);
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
