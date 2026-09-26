using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>The Cox proportional hazards model, fitted by Newton-Raphson on Efron's partial likelihood.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.CoxPHFitter</c> 0.30.3, right-censored, unpenalised and unstratified.
/// Efron is the only handling of ties the reference offers, so it is the only one here. Thread-safe.
/// </remarks>
public static class CoxProportionalHazards
{
    // The largest step component below which the fit has converged. Four or five iterations reach it on
    // every fixture of the corpus, from coefficients at zero.
    private const double StepTolerance = 1e-10;

    // Past this many units of log hazard ratio in one step, a flat likelihood is read as a
    // coefficient running away rather than as slow convergence.
    private const double RunawayStep = 0.5;

    // A separated fit "converges" once the score underflows to zero: on five subjects the
    // information fell to 3e-16 of its value at zero, where a strong finite effect kept 0.1.
    private const double CollapsedInformation = 1e-10;

    /// <summary>Fits the model and reports its inference table.</summary>
    /// <param name="design">The covariates, row-major: <paramref name="featureCount"/> values per subject.</param>
    /// <param name="durations">Each subject's duration, non-negative.</param>
    /// <param name="eventObserved">Whether each subject's event was observed rather than censored.</param>
    /// <param name="featureCount">How many covariates each subject carries.</param>
    /// <param name="options">The interval level and the iteration budget, or <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficients with their standard errors, tests and intervals, the likelihood-ratio test and the concordance.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">The spans disagree in length, a duration is negative or NaN, a covariate is not finite, no event is observed, or a covariate separates the events or is collinear with the others.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge within <see cref="CoxOptions.MaximumIterations"/>.</exception>
    public static CoxSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        CoxOptions? options = null)
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
        var likelihood = new EfronPartialLikelihood(design, durations, eventObserved, featureCount);
        Fitted fitted = Maximize(likelihood, featureCount, settings.MaximumIterations);
        switch (fitted.Outcome)
        {
            case Outcome.Collinear:
                throw new ArgumentException(
                    "The observed information is singular: a covariate is collinear with the others, "
                    + "so its coefficient is not identified. Drop or combine the redundant columns.",
                    nameof(design));
            case Outcome.Separated:
                throw new ArgumentException(
                    "A covariate separates the events: the likelihood keeps rising as a coefficient "
                    + "grows, so it has no maximum and the coefficient is infinite.",
                    nameof(design));
            case Outcome.Exhausted:
                throw new InvalidOperationException(
                    $"The Cox fit did not converge within {settings.MaximumIterations} Newton-Raphson "
                    + "iterations; a table built from where it stopped would carry standard errors of no meaning.");
        }

        return Summarize(
            fitted.Coefficients, fitted.Covariance, fitted.LogLikelihood, fitted.NullLogLikelihood,
            settings.ConfidenceLevel, HarrellConcordance.Harrell(design, durations, eventObserved, fitted.Coefficients));
    }

    private enum Outcome
    {
        Converged,
        Collinear,
        Separated,
        Exhausted,
    }

    private sealed record Fitted(
        Outcome Outcome, double[] Coefficients, double LogLikelihood, double NullLogLikelihood, double[] Covariance);

    /// <summary>Newton-Raphson from zero, reporting how it ended rather than throwing, so <see cref="Fit"/> names its own parameters.</summary>
    /// <remarks>
    /// A singular information while the likelihood is still rising is a coefficient running to
    /// infinity; a singular one that is not is a covariate the others already determine.
    /// </remarks>
    private static Fitted Maximize(EfronPartialLikelihood likelihood, int p, int maximumIterations)
    {
        double[] coefficients = new double[p];
        double[] score = new double[p];
        double[] information = new double[p * p];
        double nullLogLikelihood = likelihood.Evaluate(coefficients, score, information);
        double[] initial = Diagonal(information, p);
        double logLikelihood = nullLogLikelihood;
        for (int iteration = 1; iteration <= maximumIterations; iteration++)
        {
            if (!Cholesky.TryFactor(information, p, out double[] lower))
            {
                return Ended(iteration > 1 && logLikelihood > nullLogLikelihood ? Outcome.Separated : Outcome.Collinear);
            }

            double largest = Step(coefficients, Cholesky.Solve(lower, p, score));
            double next = likelihood.Evaluate(coefficients, score, information);
            if (largest < StepTolerance)
            {
                return Finish(coefficients, next, nullLogLikelihood, information, initial);
            }

            if (iteration == maximumIterations && largest > RunawayStep && next - logLikelihood < StepTolerance)
            {
                return Ended(Outcome.Separated);
            }

            logLikelihood = next;
        }

        return Ended(Outcome.Exhausted);
    }

    /// <summary>The table's inputs at a converged step, unless the information collapsed getting there.</summary>
    private static Fitted Finish(
        double[] coefficients, double logLikelihood, double nullLogLikelihood, double[] information, double[] initial)
    {
        int p = coefficients.Length;
        if (Collapsed(information, initial, p))
        {
            return Ended(Outcome.Separated);
        }

        return Cholesky.TryFactor(information, p, out double[] lower)
            ? new Fitted(Outcome.Converged, coefficients, logLikelihood, nullLogLikelihood, Cholesky.Inverse(lower, p))
            : Ended(Outcome.Collinear);
    }

    private static Fitted Ended(Outcome outcome) => new(outcome, [], double.NaN, double.NaN, []);

    private static double[] Diagonal(double[] matrix, int p)
    {
        double[] diagonal = new double[p];
        for (int j = 0; j < p; j++)
        {
            diagonal[j] = matrix[(j * p) + j];
        }

        return diagonal;
    }

    /// <summary>Whether any coefficient's information fell to a vanishing fraction of its value at zero.</summary>
    private static bool Collapsed(double[] information, double[] initial, int p)
    {
        for (int j = 0; j < p; j++)
        {
            if (information[(j * p) + j] < CollapsedInformation * initial[j])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Applies a Newton step and returns its largest component.</summary>
    private static double Step(double[] coefficients, double[] step)
    {
        double largest = 0.0;
        for (int j = 0; j < coefficients.Length; j++)
        {
            coefficients[j] += step[j];
            largest = Math.Max(largest, Math.Abs(step[j]));
        }

        return largest;
    }

    private static CoxSummary Summarize(
        double[] coefficients, double[] covariance, double logLikelihood, double nullLogLikelihood,
        double level, double concordance)
    {
        int p = coefficients.Length;
        double multiplier = Distributions.NormalQuantile(1.0 - ((1.0 - level) / 2.0));
        double[] errors = new double[p];
        double[] z = new double[p];
        double[] pValues = new double[p];
        double[] lower = new double[p];
        double[] upper = new double[p];
        for (int j = 0; j < p; j++)
        {
            errors[j] = Math.Sqrt(covariance[(j * p) + j]);
            z[j] = coefficients[j] / errors[j];
            pValues[j] = Distributions.ChiSquaredSf(z[j] * z[j], 1.0);
            lower[j] = coefficients[j] - (multiplier * errors[j]);
            upper[j] = coefficients[j] + (multiplier * errors[j]);
        }

        double statistic = 2.0 * (logLikelihood - nullLogLikelihood);
        return new CoxSummary
        {
            Coefficients = coefficients,
            StandardErrors = errors,
            ZStatistics = z,
            PValues = pValues,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            HazardRatios = [.. coefficients.Select(Math.Exp)],
            HazardRatioLower = [.. lower.Select(Math.Exp)],
            HazardRatioUpper = [.. upper.Select(Math.Exp)],
            LogLikelihood = logLikelihood,
            NullLogLikelihood = nullLogLikelihood,
            LikelihoodRatioStatistic = statistic,
            LikelihoodRatioPValue = Distributions.ChiSquaredSf(statistic, p),
            LikelihoodRatioDegreesOfFreedom = p,
            ConcordanceIndex = concordance,
            ConfidenceLevel = level,
        };
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
