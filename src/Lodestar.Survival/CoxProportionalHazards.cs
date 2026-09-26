using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>The Cox proportional hazards model, fitted by Newton-Raphson on Efron's partial likelihood.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.CoxPHFitter</c> 0.30.3, right-censored, with its strata, subject weights,
/// elastic-net penalty, robust and clustered variances, baseline and predictions. Efron is the only handling of ties
/// the reference offers, so it is the only one here. Thread-safe.
/// </remarks>
public static class CoxProportionalHazards
{
    /// <summary>Fits the model and reports its inference table.</summary>
    /// <param name="design">The covariates, row-major: <paramref name="featureCount"/> values per subject.</param>
    /// <param name="durations">Each subject's duration, non-negative.</param>
    /// <param name="eventObserved">Whether each subject's event was observed rather than censored.</param>
    /// <param name="featureCount">How many covariates each subject carries.</param>
    /// <param name="options">The interval level, the iteration budget, the penalty and the variance, or <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficients with their standard errors, tests and intervals, the likelihood-ratio test, the concordance and the baseline.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">The spans disagree in length, a duration is negative or NaN, a covariate is not finite or does not vary, no event is observed, or a covariate separates the events or is collinear with the others.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge within <see cref="CoxOptions.MaximumIterations"/>.</exception>
    public static CoxSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        CoxOptions? options = null) =>
        Fit(design, durations, eventObserved, [], [], [], featureCount, options);

    /// <summary>Fits the model with subject weights, strata and clusters, lifelines' <c>weights_col</c>, <c>strata</c> and <c>cluster_col</c>.</summary>
    /// <param name="design">The covariates, row-major: <paramref name="featureCount"/> values per subject.</param>
    /// <param name="durations">Each subject's duration, non-negative.</param>
    /// <param name="eventObserved">Whether each subject's event was observed rather than censored.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="strata">One stratum label per subject, or empty for none: each stratum has its own baseline hazard.</param>
    /// <param name="clusters">One cluster label per subject, or empty for none: the sandwich variance sums each cluster's residuals.</param>
    /// <param name="featureCount">How many covariates each subject carries.</param>
    /// <param name="options">The interval level, the iteration budget, the penalty and the variance, or <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficients with their standard errors, tests and intervals, the likelihood-ratio test, the concordance and the baselines.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight, stratum or cluster span is neither empty nor one value per subject, or a weight is not positive and finite.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge within <see cref="CoxOptions.MaximumIterations"/>, or lifelines' own loop, which an L1 penalty runs, gave up.</exception>
    // S107: the six spans are lifelines' columns, one value per subject each; a record could not hold spans, and
    // arrays in one would copy what the caller already holds.
#pragma warning disable S107
    public static CoxSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<int> strata,
        ReadOnlySpan<int> clusters,
        int featureCount,
        CoxOptions? options = null)
#pragma warning restore S107
    {
        Guard.NotLessThan(featureCount, 1);
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        ValidateDesign(design, durations.Length, featureCount);
        if (eventObserved.IndexOf(true) < 0)
        {
            throw new ArgumentException(
                "A Cox model is fitted on the observed events, and every subject here is censored.",
                nameof(eventObserved));
        }

        CoxOptions settings = options ?? new CoxOptions();
        CoxData data = CoxData.Build(design, durations, eventObserved, weights, strata, clusters, featureCount);
        var likelihood = new EfronPartialLikelihood(data);
        var penalty = new ElasticNet(data.Count, settings.Penalizer, settings.L1Ratio);
        CoxFit fitted = settings.L1Ratio > 0.0 && !penalty.IsZero
            ? CoxNewton.Lifelines(likelihood, featureCount, penalty, LoopVariant.ProportionalHazards)
            : CoxNewton.Optimum(likelihood, featureCount, penalty, settings.MaximumIterations);
        CoxReport.ThrowUnlessConverged(fitted, settings.MaximumIterations, nameof(design));
        bool robust = settings.Robust || !clusters.IsEmpty;
        return CoxReport.Summarize(
            data, fitted, settings.ConfidenceLevel, robust, Concordance(data, fitted.Coefficients), CoxReport.Breslow(data, fitted.Coefficients));
    }

    /// <summary>Tests each covariate for a hazard ratio that drifts with time, lifelines' <c>proportional_hazard_test</c>.</summary>
    /// <param name="design">The covariates the model was fitted on, row-major.</param>
    /// <param name="durations">Each subject's duration, as fitted.</param>
    /// <param name="eventObserved">Each subject's event flag, as fitted.</param>
    /// <param name="fit">The model fitted on these subjects.</param>
    /// <param name="transform">The time scale the residuals are correlated with; the rank of the event by default, as lifelines'.</param>
    /// <returns>One chi-squared test on one degree of freedom per covariate, in the design's column order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fit"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The spans disagree with each other or with the fit's covariate count, a value is not finite, or no event is observed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="transform"/> names no time scale.</exception>
    public static IReadOnlyList<TestResult> TestProportionalHazards(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        CoxSummary fit,
        CoxTimeTransform transform = CoxTimeTransform.Rank) =>
        TestProportionalHazards(design, durations, eventObserved, [], [], fit, transform);

    /// <summary>Tests each covariate of a weighted or stratified fit for a hazard ratio that drifts with time.</summary>
    /// <param name="design">The covariates the model was fitted on, row-major.</param>
    /// <param name="durations">Each subject's duration, as fitted.</param>
    /// <param name="eventObserved">Each subject's event flag, as fitted.</param>
    /// <param name="weights">The weights the model was fitted with, or empty.</param>
    /// <param name="strata">The strata the model was fitted with, or empty.</param>
    /// <param name="fit">The model fitted on these subjects.</param>
    /// <param name="transform">The time scale the residuals are correlated with.</param>
    /// <returns>One chi-squared test on one degree of freedom per covariate, in the design's column order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fit"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or stratum span is neither empty nor one value per subject, or the strata are not the fit's, or the fit is a time-varying one.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="transform"/> names no time scale.</exception>
    /// <remarks>
    /// The Schoenfeld residuals are lifelines' approximation, which R's <c>cox.zph</c> left in 2019, so they are
    /// comparable with lifelines and not with a current R. A weighted fit's rank still counts events, unweighted.
    /// </remarks>
    public static IReadOnlyList<TestResult> TestProportionalHazards(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<int> strata,
        CoxSummary fit,
        CoxTimeTransform transform)
    {
        Guard.NotNull(fit);
        if (transform < CoxTimeTransform.Rank || transform > CoxTimeTransform.Log)
        {
            throw new ArgumentOutOfRangeException(nameof(transform), transform, "The transform names no time scale.");
        }

        int featureCount = fit.Coefficients.Count;
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        ValidateDesign(design, durations.Length, featureCount);
        if (eventObserved.IndexOf(true) < 0)
        {
            throw new ArgumentException("The test reads the events' residuals, and every subject here is censored.", nameof(eventObserved));
        }

        CheckFitMatches(fit, strata);
        CoxData data = CoxData.Build(design, durations, eventObserved, weights, strata, [], featureCount);
        data.UseDesignAsGiven(design);
        return [.. Schoenfeld.Statistics(data, fit, fit.ModelCovariance, transform)
            .Select(statistic => new TestResult(statistic, Distributions.ChiSquaredSf(statistic, 1.0)))];
    }

    /// <summary>Refuses a time-varying fit, and strata that are not the ones the fit was stratified by.</summary>
    private static void CheckFitMatches(CoxSummary fit, ReadOnlySpan<int> strata)
    {
        if (fit.TimeVarying)
        {
            throw new ArgumentException(
                "The test reads a proportional hazards fit; lifelines' test takes no time-varying one either.", nameof(fit));
        }

        int[] labels = [.. strata.ToArray().Distinct().OrderBy(label => label)];
        int[] fitted = [.. fit.Baselines.Select(baseline => baseline.Stratum)];
        bool unstratified = fitted.Length == 1 && fitted[0] == 0 && strata.IsEmpty;
        if (!unstratified && !labels.SequenceEqual(fitted))
        {
            throw new ArgumentException(
                "The strata are not the ones the fit was stratified by; pass the same labels, or none for an unstratified fit.",
                nameof(strata));
        }
    }

    /// <summary>Harrell's index summed over the strata, the pairs of each counted within it, as lifelines counts them.</summary>
    private static double Concordance(CoxData data, double[] beta)
    {
        double credit = 0.0;
        double pairs = 0.0;
        foreach ((_, int start, int end) in data.Strata)
        {
            int length = end - start;
            var eta = new double[length];
            for (int i = 0; i < length; i++)
            {
                ReadOnlySpan<double> row = data.Row(start + i);
                for (int a = 0; a < beta.Length; a++)
                {
                    eta[i] += row[a] * beta[a];
                }
            }

            (double c, double n) = HarrellConcordance.Counts(
                data.Durations.AsSpan(start, length), eta, data.Events.AsSpan(start, length));
            credit += c;
            pairs += n;
        }

        return credit / pairs;
    }

    private static void ValidateDesign(ReadOnlySpan<double> design, int subjects, int featureCount)
    {
        if (design.Length != (long)subjects * featureCount)
        {
            throw new ArgumentException(
                $"design holds {design.Length} values, not the {subjects} x {featureCount} the sample and "
                + "featureCount declare.",
                nameof(design));
        }

        for (int i = 0; i < design.Length; i++)
        {
            if (double.IsNaN(design[i]) || double.IsInfinity(design[i]))
            {
                throw new ArgumentException(
                    $"A covariate is a finite number; design[{i}] is {design[i]}.", nameof(design));
            }
        }
    }
}
