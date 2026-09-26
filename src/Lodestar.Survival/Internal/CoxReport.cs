using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>From a fit on the standardised scale to the table lifelines prints, shared by both Cox fitters.</summary>
internal static class CoxReport
{
    /// <summary>Raises the exception a fit that did not converge deserves, naming the design where the data is at fault.</summary>
    internal static void ThrowUnlessConverged(CoxFit fitted, int maximumIterations, string designName)
    {
        switch (fitted.Outcome)
        {
            case CoxOutcome.Collinear:
                throw new ArgumentException(
                    "The observed information is singular: a covariate is collinear with the others, "
                    + "so its coefficient is not identified. Drop or combine the redundant columns.",
                    designName);
            case CoxOutcome.Separated:
                throw new ArgumentException(
                    "A covariate separates the events: the likelihood keeps rising as a coefficient "
                    + "grows, so it has no maximum and the coefficient is infinite.",
                    designName);
            case CoxOutcome.Exhausted:
                throw new InvalidOperationException(
                    $"The Cox fit did not converge within {maximumIterations} Newton-Raphson iterations; a table built "
                    + "from where it stopped would carry standard errors of no meaning.");
            case CoxOutcome.GaveUp:
                throw new InvalidOperationException(
                    "lifelines' own loop, which an L1 penalty runs, gave up within its step limit or at its smallest "
                    + "step, as lifelines' fit would; its answer there is not one lifelines reports as converged.");
        }
    }

    /// <summary>Breslow's baseline hazard per stratum at every distinct duration, lifelines' <c>_compute_baseline_hazards</c>.</summary>
    /// <remarks>
    /// At each distinct duration of a stratum, its weighted events over the weighted partial hazards of everyone still
    /// at risk in it, <c>exp((x − mean) · β)</c>; a stratum without that duration has zero there, as lifelines fills it.
    /// </remarks>
    internal static CoxBaseline[] Breslow(CoxData data, double[] beta)
    {
        double[] times = [.. data.Durations.Distinct().OrderBy(t => t)];
        var baselines = new CoxBaseline[data.Strata.Length];
        for (int s = 0; s < data.Strata.Length; s++)
        {
            (int label, int start, int end) = data.Strata[s];
            var hazard = new double[times.Length];
            double risk = 0.0;
            int position = end;
            for (int t = times.Length - 1; t >= 0; t--)
            {
                double events = 0.0;
                bool present = false;
                // S1244: a tie is the same recorded duration.
#pragma warning disable S1244
                while (position > start && data.Durations[position - 1] == times[t])
#pragma warning restore S1244
                {
                    position--;
                    present = true;
                    double w = data.Weights[position];
                    risk += w * Math.Exp(Eta(data, position, beta));
                    events += data.Events[position] ? w : 0.0;
                }

                hazard[t] = present ? events / risk : 0.0;
            }

            baselines[s] = Accumulated(label, times, hazard);
        }

        return baselines;
    }

    /// <summary>A baseline from its hazard: the running sum and its survival.</summary>
    internal static CoxBaseline Accumulated(int label, double[] times, double[] hazard)
    {
        var cumulative = new double[hazard.Length];
        double sum = 0.0;
        for (int t = 0; t < hazard.Length; t++)
        {
            sum += hazard[t];
            cumulative[t] = sum;
        }

        return new CoxBaseline(label, times, hazard, cumulative, [.. cumulative.Select(h => Math.Exp(-h))]);
    }

    /// <summary>The summary: coefficients and covariance rescaled to the covariates' units, then the table.</summary>
    /// <param name="data">The sorted, standardised sample.</param>
    /// <param name="fitted">The converged fit on the standardised scale.</param>
    /// <param name="level">The interval level.</param>
    /// <param name="robust">Whether the standard errors are the sandwich.</param>
    /// <param name="concordance">Harrell's index, or NaN where the fitter has none.</param>
    /// <param name="baselines">The baselines to report.</param>
    /// <param name="timeVarying">Whether the fit is the time-varying one.</param>
    internal static CoxSummary Summarize(
        CoxData data, CoxFit fitted, double level, bool robust, double concordance, CoxBaseline[] baselines, bool timeVarying = false)
    {
        int p = data.FeatureCount;
        if (!Cholesky.TryFactor(fitted.Information, p, out double[] lower))
        {
            ThrowUnlessConverged(fitted with { Outcome = CoxOutcome.Collinear }, 0, "design");
        }

        double[] inverse = Cholesky.Inverse(lower, p);
        double[] model = Rescaled(inverse, data.Deviations);
        double[] covariance = robust ? CoxResiduals.Sandwich(data, fitted.Coefficients, inverse) : model;
        var coefficients = new double[p];
        for (int j = 0; j < p; j++)
        {
            coefficients[j] = fitted.Coefficients[j] / data.Deviations[j];
        }

        return Table(coefficients, (covariance, model), fitted, (level, robust, concordance, timeVarying), data.Means, baselines);
    }

    /// <summary>The linear predictor of one sorted row on the standardised scale.</summary>
    internal static double Eta(CoxData data, int position, double[] beta)
    {
        ReadOnlySpan<double> row = data.Row(position);
        double sum = 0.0;
        for (int a = 0; a < beta.Length; a++)
        {
            sum += row[a] * beta[a];
        }

        return sum;
    }

    /// <summary>lifelines' <c>−inv(H) / outer(σ, σ)</c>.</summary>
    private static double[] Rescaled(double[] inverse, double[] deviations)
    {
        int p = deviations.Length;
        var covariance = new double[p * p];
        for (int a = 0; a < p; a++)
        {
            for (int b = 0; b < p; b++)
            {
                covariance[(a * p) + b] = inverse[(a * p) + b] / (deviations[a] * deviations[b]);
            }
        }

        return covariance;
    }

    private static CoxSummary Table(
        double[] coefficients,
        (double[] Reported, double[] Model) covariances,
        CoxFit fitted,
        (double Level, bool Robust, double Concordance, bool TimeVarying) settings,
        double[] means,
        CoxBaseline[] baselines)
    {
        double level = settings.Level;
        double[] covariance = covariances.Reported;
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

        double statistic = 2.0 * (fitted.LogLikelihood - fitted.NullLogLikelihood);
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
            LogLikelihood = fitted.LogLikelihood,
            NullLogLikelihood = fitted.NullLogLikelihood,
            LikelihoodRatioStatistic = statistic,
            LikelihoodRatioPValue = Distributions.ChiSquaredSf(statistic, p),
            LikelihoodRatioDegreesOfFreedom = p,
            ConcordanceIndex = settings.Concordance,
            ConfidenceLevel = level,
            Robust = settings.Robust,
            TimeVarying = settings.TimeVarying,
            ModelCovariance = covariances.Model,
            CovariateMeans = means,
            Baselines = baselines,
        };
    }
}
