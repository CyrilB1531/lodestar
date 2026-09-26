namespace Lodestar.Survival;

/// <summary>What a Cox proportional hazards fit reports, at <c>lifelines</c> parity.</summary>
/// <remarks>
/// The per-coefficient lists are parallel and in the design's column order. There is no iteration
/// count: the reference reaches its answer under a different stopping rule, so no oracle could
/// check one, and a fit that did not converge throws instead of returning.
/// </remarks>
public sealed class CoxSummary
{
    /// <summary>Built by <c>CoxProportionalHazards.Fit</c> and <c>CoxTimeVarying.Fit</c> alone; there is no other way to hold one.</summary>
    internal CoxSummary()
    {
    }

    /// <summary>The log hazard ratios, one per column of the design.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>The square roots of the inverse observed information's diagonal.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>Each coefficient over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; init; } = [];

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The lower end of each coefficient's interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of each coefficient's interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];

    /// <summary>The exponential of each coefficient: how much a unit of the covariate multiplies the hazard.</summary>
    public IReadOnlyList<double> HazardRatios { get; init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceLower"/>.</summary>
    public IReadOnlyList<double> HazardRatioLower { get; init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceUpper"/>.</summary>
    public IReadOnlyList<double> HazardRatioUpper { get; init; } = [];

    /// <summary>The log partial likelihood at the fitted coefficients, with Efron's handling of ties.</summary>
    public double LogLikelihood { get; init; }

    /// <summary>The same at every coefficient zero: the model with no covariate.</summary>
    public double NullLogLikelihood { get; init; }

    /// <summary><c>2 · (LogLikelihood − NullLogLikelihood)</c>.</summary>
    public double LikelihoodRatioStatistic { get; init; }

    /// <summary>The chi-squared upper tail of the likelihood-ratio statistic.</summary>
    public double LikelihoodRatioPValue { get; init; }

    /// <summary>The number of covariates.</summary>
    public int LikelihoodRatioDegreesOfFreedom { get; init; }

    /// <summary>Harrell's concordance index between the durations and the negated linear predictor.</summary>
    public double ConcordanceIndex { get; init; }

    /// <summary>The level the intervals were built at.</summary>
    public double ConfidenceLevel { get; init; }

    /// <summary>The model-based covariance of the coefficients, row-major, whatever <see cref="Robust"/> says: what the proportional hazards test scales by.</summary>
    internal double[] ModelCovariance { get; init; } = [];

    /// <summary>Whether the fit is a <c>CoxTimeVarying</c> one, which the proportional hazards test does not take, as lifelines' does not.</summary>
    internal bool TimeVarying { get; init; }

    /// <summary>Whether <see cref="StandardErrors"/> and everything built on them are the Huber sandwich.</summary>
    public bool Robust { get; init; }

    /// <summary>Each covariate's mean in the fitted sample: the point the baseline and the partial hazards are centred on.</summary>
    public IReadOnlyList<double> CovariateMeans { get; init; } = [];

    /// <summary>The baseline hazard of each stratum, labels ascending; one, labelled zero, when the fit is unstratified.</summary>
    /// <remarks>A <c>CoxTimeVarying</c> fit carries one baseline, labelled zero, pooled over its strata, as lifelines' time-varying fitter computes it.</remarks>
    public IReadOnlyList<CoxBaseline> Baselines { get; init; } = [];

    /// <summary>Each subject's log partial hazard, lifelines' <c>predict_log_partial_hazard</c>: <c>(x − mean) · β</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major, one value per coefficient per subject.</param>
    /// <returns>One value per subject.</returns>
    /// <exception cref="ArgumentException"><paramref name="design"/> is not a whole number of rows of finite values.</exception>
    public double[] PredictLogPartialHazard(ReadOnlySpan<double> design)
    {
        int p = Coefficients.Count;
        if (design.Length == 0 || design.Length % p != 0)
        {
            throw new ArgumentException($"design holds {design.Length} values, not whole rows of {p}.", nameof(design));
        }

        var result = new double[design.Length / p];
        for (int i = 0; i < result.Length; i++)
        {
            double sum = 0.0;
            for (int a = 0; a < p; a++)
            {
                double value = design[(i * p) + a];
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException($"A covariate is a finite number; design[{(i * p) + a}] is {value}.", nameof(design));
                }

                sum += (value - CovariateMeans[a]) * Coefficients[a];
            }

            result[i] = sum;
        }

        return result;
    }

    /// <summary>Each subject's partial hazard, lifelines' <c>predict_partial_hazard</c>: the exponential of the log one.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>One value per subject.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictLogPartialHazard"/>.</exception>
    public double[] PredictPartialHazard(ReadOnlySpan<double> design) => [.. PredictLogPartialHazard(design).Select(Math.Exp)];

    /// <summary>Each subject's cumulative hazard at <paramref name="times"/>, lifelines' <c>predict_cumulative_hazard</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="strata">Each subject's stratum when the fit was stratified; empty otherwise.</param>
    /// <param name="times">The times to read at; empty for the baseline's own times.</param>
    /// <returns>Row-major, one row per subject and one column per time.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictLogPartialHazard"/>, or the strata do not match the subjects or name a stratum the fit did not see, or a time is NaN.</exception>
    /// <remarks>The baseline's cumulative hazard is interpolated linearly between its times and held flat past its ends, lifelines' <c>numpy.interp</c>.</remarks>
    public double[] PredictCumulativeHazard(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, ReadOnlySpan<double> times)
    {
        double[] hazards = PredictPartialHazard(design);
        CoxBaseline[] baselines = BaselinesFor(strata, hazards.Length);
        foreach (double time in times)
        {
            if (double.IsNaN(time))
            {
                throw new ArgumentException("A time to read at is a number; times holds NaN.", nameof(times));
            }
        }

        int columns = times.IsEmpty ? Baselines[0].Times.Length : times.Length;
        var result = new double[hazards.Length * columns];
        for (int i = 0; i < hazards.Length; i++)
        {
            CoxBaseline baseline = baselines[i];
            for (int j = 0; j < columns; j++)
            {
                double at = times.IsEmpty ? baseline.Times[j] : times[j];
                result[(i * columns) + j] = Interpolate(baseline.Times, baseline.CumulativeHazard, at) * hazards[i];
            }
        }

        return result;
    }

    /// <summary>Each subject's survival at <paramref name="times"/>, lifelines' <c>predict_survival_function</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="strata">Each subject's stratum when the fit was stratified; empty otherwise.</param>
    /// <param name="times">The times to read at; empty for the baseline's own times.</param>
    /// <returns>Row-major, one row per subject and one column per time: <c>exp(−H)</c> of <see cref="PredictCumulativeHazard"/>.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictCumulativeHazard"/>.</exception>
    public double[] PredictSurvivalFunction(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, ReadOnlySpan<double> times) =>
        [.. PredictCumulativeHazard(design, strata, times).Select(h => Math.Exp(-h))];

    /// <summary>The time each subject's survival first falls to <paramref name="probability"/>, lifelines' <c>predict_percentile</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="strata">Each subject's stratum when the fit was stratified; empty otherwise.</param>
    /// <param name="probability">The survival level, in [0, 1]; a half for the median.</param>
    /// <returns>One time per subject, infinity where the curve stays above the level.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictCumulativeHazard"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="probability"/> lies outside [0, 1].</exception>
    public double[] PredictPercentile(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, double probability)
    {
        if (!(probability >= 0.0 && probability <= 1.0))
        {
            throw new ArgumentOutOfRangeException(nameof(probability), probability, "A survival level lies in [0, 1].");
        }

        double[] survival = PredictSurvivalFunction(design, strata, []);
        double[] times = Baselines[0].Times;
        int columns = times.Length;
        var result = new double[survival.Length / columns];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = double.PositiveInfinity;
            if (survival[(i * columns) + columns - 1] > probability)
            {
                continue;
            }

            for (int j = 0; j < columns; j++)
            {
                if (survival[(i * columns) + j] <= probability)
                {
                    result[i] = times[j];
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>Each subject's median survival time, lifelines' <c>predict_median</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="strata">Each subject's stratum when the fit was stratified; empty otherwise.</param>
    /// <returns>One time per subject, infinity where the curve stays above one half.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictCumulativeHazard"/>.</exception>
    public double[] PredictMedian(ReadOnlySpan<double> design, ReadOnlySpan<int> strata) => PredictPercentile(design, strata, 0.5);

    /// <summary>The area under each subject's survival curve over the baseline's times, lifelines' <c>predict_expectation</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="strata">Each subject's stratum when the fit was stratified; empty otherwise.</param>
    /// <returns>One value per subject, the trapezoid rule over the baseline's times, from the first of them.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictCumulativeHazard"/>.</exception>
    /// <remarks>
    /// lifelines integrates from the first observed duration rather than from zero, and stops at the last, so this is
    /// the expectation restricted to the observed span, not the unrestricted mean.
    /// </remarks>
    public double[] PredictExpectation(ReadOnlySpan<double> design, ReadOnlySpan<int> strata)
    {
        double[] survival = PredictSurvivalFunction(design, strata, []);
        double[] times = Baselines[0].Times;
        int columns = times.Length;
        var result = new double[survival.Length / columns];
        for (int i = 0; i < result.Length; i++)
        {
            double area = 0.0;
            for (int j = 1; j < columns; j++)
            {
                area += (times[j] - times[j - 1]) * (survival[(i * columns) + j] + survival[(i * columns) + j - 1]) / 2.0;
            }

            result[i] = area;
        }

        return result;
    }

    private CoxBaseline[] BaselinesFor(ReadOnlySpan<int> strata, int subjects)
    {
        var result = new CoxBaseline[subjects];
        if (strata.IsEmpty)
        {
            if (Baselines.Count > 1)
            {
                throw new ArgumentException("The fit was stratified, so each subject needs its stratum.", nameof(strata));
            }

            for (int i = 0; i < subjects; i++)
            {
                result[i] = Baselines[0];
            }

            return result;
        }

        if (strata.Length != subjects)
        {
            throw new ArgumentException($"strata holds {strata.Length} labels for {subjects} subjects.", nameof(strata));
        }

        for (int i = 0; i < subjects; i++)
        {
            int label = strata[i];
            result[i] = Baselines.FirstOrDefault(b => b.Stratum == label)
                ?? throw new ArgumentException($"The fit saw no stratum {label}.", nameof(strata));
        }

        return result;
    }

    /// <summary><c>numpy.interp</c>: linear between the points, flat past either end.</summary>
    private static double Interpolate(double[] x, double[] y, double at)
    {
        if (at <= x[0])
        {
            return y[0];
        }

        int last = x.Length - 1;
        if (at >= x[last])
        {
            return y[last];
        }

        int j = Array.BinarySearch(x, at);
        if (j >= 0)
        {
            return y[j];
        }

        int upper = ~j;
        double slope = (y[upper] - y[upper - 1]) / (x[upper] - x[upper - 1]);
        return (slope * (at - x[upper - 1])) + y[upper - 1];
    }
}
