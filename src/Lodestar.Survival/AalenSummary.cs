using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>What an Aalen additive fit reports, at <c>lifelines</c> parity, and the predictions it makes.</summary>
/// <remarks>
/// The per-time lists are row-major, one row per event time and one column per coefficient: the covariates in the
/// design's order, then the intercept. <see cref="CovariateIndices"/> says which is which.
/// </remarks>
public sealed class AalenSummary
{
    private double[] _cumulative = [];
    private int _width;

    private AalenSummary()
    {
    }

    /// <summary>The event times the coefficients step at, ascending; lifelines' index, cut where the fit stopped.</summary>
    public IReadOnlyList<double> EventTimes { get; private init; } = [];

    /// <summary>The design column each coefficient multiplies, <c>-1</c> for the intercept.</summary>
    public IReadOnlyList<int> CovariateIndices { get; private init; } = [];

    /// <summary>Each coefficient's increment at each event time, lifelines' <c>hazards_</c>; row-major, time by coefficient.</summary>
    public IReadOnlyList<double> Hazards { get; private init; } = [];

    /// <summary>The increments summed, lifelines' <c>cumulative_hazards_</c>; row-major, time by coefficient.</summary>
    public IReadOnlyList<double> CumulativeHazards { get; private init; } = [];

    /// <summary>Their variance, lifelines' <c>cumulative_variance_</c>, rescaled by the covariate's deviation as lifelines rescales it.</summary>
    public IReadOnlyList<double> CumulativeVariance { get; private init; } = [];

    /// <summary>The lower end of each cumulative coefficient's interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; private init; } = [];

    /// <summary>The upper end.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; private init; } = [];

    /// <summary>Each cumulative coefficient's slope through the origin, weighted by the number at risk, lifelines' <c>summary["slope(coef)"]</c>.</summary>
    public IReadOnlyList<double> Slopes { get; private init; } = [];

    /// <summary>The slopes' standard errors, lifelines' <c>summary["se(slope(coef))"]</c>.</summary>
    public IReadOnlyList<double> SlopeStandardErrors { get; private init; } = [];

    /// <summary>Harrell's concordance index, lifelines' <c>concordance_index_</c>, which pairs the sorted durations with the predictions in the design's order.</summary>
    public double ConcordanceIndex { get; private init; }

    /// <summary>The level the intervals were built at.</summary>
    public double ConfidenceLevel { get; private init; }

    /// <summary>The number of covariates a design row holds.</summary>
    public int FeatureCount { get; private init; }

    /// <summary>Whether the fit carries an intercept, the last coefficient.</summary>
    public bool FitIntercept { get; private init; }

    internal static AalenSummary Build(
        Fitted fitted, int[] atRisk, (double[] Design, double[] SortedDurations, bool[] SortedEvents) training)
    {
        int m = fitted.Times.Length;
        int d = fitted.FeatureCount + (fitted.Settings.FitIntercept ? 1 : 0);
        double z = AalenAdditive.Critical(fitted.Settings.ConfidenceLevel);
        var lower = new double[m * d];
        var upper = new double[m * d];
        for (int k = 0; k < m * d; k++)
        {
            double error = Math.Sqrt(fitted.Variance[k]);
            lower[k] = fitted.Cumulative[k] - (z * error);
            upper[k] = fitted.Cumulative[k] + (z * error);
        }

        (double[] slopes, double[] errors) = SlopeTable(fitted, atRisk, d);

        // lifelines predicts the training subjects in the order they were given and scores them against the durations
        // it sorted, so the pairs are the sorted durations' with the unsorted predictions.
        int n = training.SortedDurations.Length;
        var risk = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < d; j++)
            {
                double x = j < fitted.FeatureCount ? training.Design[(i * fitted.FeatureCount) + j] : 1.0;
                sum += fitted.Cumulative[((m - 1) * d) + j] * x;
            }

            risk[i] = sum;
        }

        return new AalenSummary
        {
            _cumulative = fitted.Cumulative,
            _width = d,
            EventTimes = fitted.Times,
            CovariateIndices = [.. Enumerable.Range(0, fitted.FeatureCount), .. fitted.Settings.FitIntercept ? (int[])[-1] : []],
            Hazards = fitted.Hazards,
            CumulativeHazards = fitted.Cumulative,
            CumulativeVariance = fitted.Variance,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            Slopes = slopes,
            SlopeStandardErrors = errors,
            ConcordanceIndex = HarrellConcordance.Index(training.SortedDurations, risk, training.SortedEvents),
            ConfidenceLevel = fitted.Settings.ConfidenceLevel,
            FeatureCount = fitted.FeatureCount,
            FitIntercept = fitted.Settings.FitIntercept,
        };
    }

    /// <summary>Each subject's cumulative hazard at every event time, lifelines' <c>predict_cumulative_hazard</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major, <see cref="FeatureCount"/> per row; empty for the one subject of a fit with no covariate.</param>
    /// <returns>Row-major, one row per subject and one column per <see cref="EventTimes"/> entry.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictCumulativeHazard(ReadOnlySpan<double> design)
    {
        double[] x = Rows(design, out int rows);
        int m = EventTimes.Count;
        var result = new double[rows * m];
        for (int i = 0; i < rows; i++)
        {
            for (int t = 0; t < m; t++)
            {
                double sum = 0.0;
                for (int j = 0; j < _width; j++)
                {
                    sum += _cumulative[(t * _width) + j] * x[(i * _width) + j];
                }

                result[(i * m) + t] = sum;
            }
        }

        return result;
    }

    /// <summary>Each subject's survival at every event time, lifelines' <c>predict_survival_function</c>: <c>exp(−H)</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>Row-major, one row per subject and one column per event time.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictSurvivalFunction(ReadOnlySpan<double> design) => [.. PredictCumulativeHazard(design).Select(h => Math.Exp(-h))];

    /// <summary>The event time at which each subject's survival reaches <paramref name="probability"/>, lifelines' <c>predict_percentile</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="probability">The survival level, in [0, 1]; a half for the median.</param>
    /// <returns>One time per subject, infinity where the last survival is above the level.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="probability"/> lies outside [0, 1].</exception>
    /// <remarks>
    /// The curve need not fall monotonically, an additive hazard's increments having either sign; the time is where
    /// numpy's search on it lands, as lifelines' <c>qth_survival_time</c> reads it, not necessarily the first crossing.
    /// </remarks>
    public double[] PredictPercentile(ReadOnlySpan<double> design, double probability)
    {
        if (!(probability >= 0.0 && probability <= 1.0))
        {
            throw new ArgumentOutOfRangeException(nameof(probability), probability, "A survival level lies in [0, 1].");
        }

        double[] survival = PredictSurvivalFunction(design);
        int m = EventTimes.Count;
        var result = new double[survival.Length / m];
        for (int i = 0; i < result.Length; i++)
        {
            ReadOnlySpan<double> curve = survival.AsSpan(i * m, m);
            result[i] = curve[m - 1] > probability ? double.PositiveInfinity : EventTimes[Math.Min(LowerBound(curve, probability), m - 1)];
        }

        return result;
    }

    /// <summary>Each subject's median, lifelines' <c>predict_median</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>One time per subject.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictMedian(ReadOnlySpan<double> design) => PredictPercentile(design, 0.5);

    /// <summary>Each subject's expected lifetime, lifelines' <c>predict_expectation</c>: the trapezoid under its survival over the event times.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>One value per subject, from the first event time, not from zero, as lifelines integrates it.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictExpectation(ReadOnlySpan<double> design)
    {
        double[] survival = PredictSurvivalFunction(design);
        int m = EventTimes.Count;
        var result = new double[survival.Length / m];
        for (int i = 0; i < result.Length; i++)
        {
            double area = 0.0;
            for (int t = 0; t + 1 < m; t++)
            {
                area += (EventTimes[t + 1] - EventTimes[t]) * (survival[(i * m) + t + 1] + survival[(i * m) + t]) / 2.0;
            }

            result[i] = area;
        }

        return result;
    }

    /// <summary>The increments smoothed by an Epanechnikov kernel, lifelines' <c>smoothed_hazards_</c>.</summary>
    /// <param name="bandwidth">The kernel's half-width, in time; positive.</param>
    /// <returns>Row-major, time by coefficient, as <see cref="Hazards"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bandwidth"/> is not positive and finite.</exception>
    public double[] SmoothedHazards(double bandwidth = 1.0)
    {
        if (!(bandwidth > 0.0) || double.IsInfinity(bandwidth))
        {
            throw new ArgumentOutOfRangeException(nameof(bandwidth), bandwidth, "A bandwidth is positive and finite.");
        }

        int m = EventTimes.Count;
        var result = new double[m * _width];
        for (int a = 0; a < m; a++)
        {
            for (int b = 0; b < m; b++)
            {
                double gap = EventTimes[a] - EventTimes[b];
                if (Math.Abs(gap) >= bandwidth)
                {
                    continue;
                }

                double scaled = gap / bandwidth;
                double weight = 0.75 * (1.0 - (scaled * scaled));
                for (int j = 0; j < _width; j++)
                {
                    result[(a * _width) + j] += weight * Hazards[(b * _width) + j];
                }
            }
        }

        return result;
    }

    /// <summary>
    /// numpy's <c>searchsorted</c> of <c>−probability</c> in <c>−survival</c>, whose search halves a window from its base
    /// rather than bisecting a range; on a curve that is not monotone the two land apart, and this is numpy's, measured
    /// identical on 20,000 random arrays.
    /// </summary>
    private static int LowerBound(ReadOnlySpan<double> survival, double probability)
    {
        int start = 0;
        int length = survival.Length;
        while (length > 1)
        {
            int half = length / 2;
            if (-survival[start + half] < -probability)
            {
                start += half;
            }

            length -= half;
        }

        return -survival[start] < -probability ? start + 1 : start;
    }

    /// <summary>The design's rows with the intercept appended, validated.</summary>
    private double[] Rows(ReadOnlySpan<double> design, out int rows)
    {
        rows = FeatureCount == 0 ? 1 : design.Length / FeatureCount;
        if ((FeatureCount == 0 && !design.IsEmpty) || (FeatureCount > 0 && (design.IsEmpty || design.Length % FeatureCount != 0)))
        {
            throw new ArgumentException($"A design row holds {FeatureCount} covariates; the design holds {design.Length} values.", nameof(design));
        }

        double[] raw = AftDesign.Validate(design, rows, FeatureCount, fitIntercept: true);
        var x = new double[rows * _width];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < FeatureCount; j++)
            {
                x[(i * _width) + j] = raw[(i * FeatureCount) + j];
            }

            if (FitIntercept)
            {
                x[(i * _width) + FeatureCount] = 1.0;
            }
        }

        return x;
    }

    /// <summary>A fit's per-time arrays and settings, as <see cref="AalenAdditive"/> computes them.</summary>
    internal sealed record Fitted(double[] Times, double[] Hazards, double[] Cumulative, double[] Variance, int FeatureCount, AalenOptions Settings);

    /// <summary>lifelines' <c>_compute_slopes</c>: each cumulative coefficient regressed through the origin on time, weighted by the number at risk.</summary>
    private static (double[] Slopes, double[] Errors) SlopeTable(Fitted fitted, int[] atRisk, int d)
    {
        int m = fitted.Times.Length;
        var slopes = new double[d];
        var errors = new double[d];
        double denominator = 0.0;
        for (int t = 0; t < m; t++)
        {
            denominator += fitted.Times[t] * (atRisk[t] * fitted.Times[t]);
        }

        for (int j = 0; j < d; j++)
        {
            var y = new double[m];
            double sum = 0.0;
            double numerator = 0.0;
            for (int t = 0; t < m; t++)
            {
                sum += atRisk[t] * fitted.Hazards[(t * d) + j];
                y[t] = sum;
                numerator += fitted.Times[t] * sum;
            }

            double beta = numerator / denominator;
            double squares = 0.0;
            for (int t = 0; t < m; t++)
            {
                double error = y[t] - (fitted.Times[t] * beta);
                squares += error * error;
            }

            slopes[j] = beta;
            errors[j] = Math.Sqrt(squares / (m - 2) / denominator);
        }

        return (slopes, errors);
    }
}
