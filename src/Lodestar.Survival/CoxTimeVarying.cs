using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>The Cox model with covariates that change over time, one row per interval, fitted on Efron's partial likelihood.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.CoxTimeVaryingFitter</c> 0.30.3. Each row is an interval <c>(start, stop]</c> over
/// which a subject's covariates held; its event flag says whether the subject's event ends that interval. Thread-safe.
/// </remarks>
public static class CoxTimeVarying
{
    /// <summary>Fits the model on start-stop intervals.</summary>
    /// <param name="design">The covariates, row-major: <paramref name="featureCount"/> values per interval.</param>
    /// <param name="starts">Each interval's start, non-negative.</param>
    /// <param name="stops">Each interval's stop, after its start.</param>
    /// <param name="eventObserved">Whether each interval ends in the subject's event.</param>
    /// <param name="featureCount">How many covariates each interval carries.</param>
    /// <param name="options">The interval level, the iteration budget and the penalty, or <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the likelihood-ratio test and the baseline; there is no concordance, as lifelines computes none here.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">The spans disagree in length, an interval is not a finite, non-negative span with its stop after its start, a covariate is not finite or does not vary, no event is observed, a covariate is collinear with the others or separates the events, or <see cref="CoxOptions.Robust"/> is asked for, which lifelines does not implement here.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    public static CoxSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> starts,
        ReadOnlySpan<double> stops,
        ReadOnlySpan<bool> eventObserved,
        int featureCount,
        CoxOptions? options = null) =>
        Fit(design, starts, stops, eventObserved, [], [], featureCount, options);

    /// <summary>Fits the model on weighted, stratified start-stop intervals, lifelines' <c>weights_col</c> and <c>strata</c>.</summary>
    /// <param name="design">The covariates, row-major: <paramref name="featureCount"/> values per interval.</param>
    /// <param name="starts">Each interval's start, non-negative.</param>
    /// <param name="stops">Each interval's stop, after its start.</param>
    /// <param name="eventObserved">Whether each interval ends in the subject's event.</param>
    /// <param name="weights">One positive, finite weight per interval, or empty for ones.</param>
    /// <param name="strata">One stratum label per interval, or empty for none.</param>
    /// <param name="featureCount">How many covariates each interval carries.</param>
    /// <param name="options">The interval level, the iteration budget and the penalty, or <see langword="null"/> for the defaults.</param>
    /// <returns>The coefficient table, the likelihood-ratio test and the baseline.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight or stratum span is neither empty nor one value per interval.</exception>
    /// <exception cref="InvalidOperationException">The fit did not converge.</exception>
    /// <remarks>
    /// The baseline is lifelines': Breslow's cumulative hazard at every distinct event time, pooled over the strata,
    /// with the weighted events over the unweighted partial hazards of the intervals at risk, as lifelines divides.
    /// </remarks>
    // S107: the spans are lifelines' columns, one value per interval each; see CoxProportionalHazards.Fit.
#pragma warning disable S107
    public static CoxSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> starts,
        ReadOnlySpan<double> stops,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        ReadOnlySpan<int> strata,
        int featureCount,
        CoxOptions? options = null)
#pragma warning restore S107
    {
        Guard.NotLessThan(featureCount, 1);
        CoxOptions settings = options ?? new CoxOptions();
        if (settings.Robust)
        {
            throw new ArgumentException(
                "lifelines' time-varying fitter implements no robust variance, so there is none to match.", nameof(options));
        }

        Intervals intervals = Intervals.Build(design, starts, stops, eventObserved, weights, strata, featureCount);
        var likelihood = new TimeVaryingLikelihood(
            intervals.Design, intervals.Starts, intervals.Stops, intervals.Events, intervals.Weights, intervals.Strata, featureCount);
        var penalty = new ElasticNet(intervals.Count, settings.Penalizer, settings.L1Ratio);
        CoxFit fitted = settings.L1Ratio > 0.0 && !penalty.IsZero
            ? CoxNewton.Lifelines(likelihood, featureCount, penalty, LoopVariant.TimeVarying)
            : CoxNewton.Optimum(likelihood, featureCount, penalty, settings.MaximumIterations);
        CoxReport.ThrowUnlessConverged(fitted, settings.MaximumIterations, nameof(design));
        return CoxReport.Summarize(
            intervals.AsCoxData(), fitted, settings.ConfidenceLevel, false, double.NaN, [intervals.Baseline(fitted.Coefficients)],
            timeVarying: true);
    }

    /// <summary>The intervals, validated and standardised over every row.</summary>
    private sealed class Intervals
    {
        private CoxData _shape = null!;

        public int Count { get; private init; }

        public double[] Design { get; private init; } = [];

        public double[] Starts { get; private init; } = [];

        public double[] Stops { get; private init; } = [];

        public bool[] Events { get; private init; } = [];

        public double[] Weights { get; private init; } = [];

        public int[][] Strata { get; private init; } = [];

        // S107: the builder takes the same columns Fit does.
#pragma warning disable S107
        public static Intervals Build(
            ReadOnlySpan<double> design,
            ReadOnlySpan<double> starts,
            ReadOnlySpan<double> stops,
            ReadOnlySpan<bool> eventObserved,
            ReadOnlySpan<double> weights,
            ReadOnlySpan<int> strata,
            int featureCount)
#pragma warning restore S107
        {
            int n = stops.Length;
            Validate(design, starts, stops, eventObserved, featureCount);

            // The sample's standardisation and checks, borrowed: the stop stands in for a duration, and the order unused.
            CoxData shape = CoxData.Build(design, stops, eventObserved, weights, strata, [], featureCount);
            var standardised = new double[design.Length];
            for (int i = 0; i < n; i++)
            {
                for (int a = 0; a < featureCount; a++)
                {
                    standardised[(i * featureCount) + a] = (design[(i * featureCount) + a] - shape.Means[a]) / shape.Deviations[a];
                }
            }

            int[] labels = strata.IsEmpty ? new int[n] : strata.ToArray();
            return new Intervals
            {
                _shape = shape,
                Count = n,
                Design = standardised,
                Starts = starts.ToArray(),
                Stops = stops.ToArray(),
                Events = eventObserved.ToArray(),
                Weights = weights.IsEmpty ? [.. Enumerable.Repeat(1.0, n)] : weights.ToArray(),
                Strata = [.. Enumerable.Range(0, n).GroupBy(i => labels[i]).OrderBy(g => g.Key).Select(g => g.ToArray())],
            };
        }

        private static void Validate(
            ReadOnlySpan<double> design,
            ReadOnlySpan<double> starts,
            ReadOnlySpan<double> stops,
            ReadOnlySpan<bool> eventObserved,
            int featureCount)
        {
            int n = stops.Length;
            if (starts.Length != n || eventObserved.Length != n || design.Length != (long)n * featureCount || n == 0)
            {
                throw new ArgumentException(
                    $"One start, stop, event flag and {featureCount} covariates are needed per interval; got {starts.Length} "
                    + $"starts, {n} stops, {eventObserved.Length} flags and {design.Length} covariates.",
                    nameof(design));
            }

            for (int i = 0; i < n; i++)
            {
                if (!(starts[i] >= 0.0) || !(stops[i] > starts[i]) || double.IsInfinity(stops[i]))
                {
                    throw new ArgumentException(
                        $"An interval is a finite, non-negative span with its stop after its start; interval {i} is ({starts[i]}, {stops[i]}].",
                        nameof(stops));
                }
            }

            if (eventObserved.IndexOf(true) < 0)
            {
                throw new ArgumentException("A Cox model is fitted on the observed events, and no interval ends in one.", nameof(eventObserved));
            }

            for (int i = 0; i < design.Length; i++)
            {
                if (double.IsNaN(design[i]) || double.IsInfinity(design[i]))
                {
                    throw new ArgumentException($"A covariate is a finite number; design[{i}] is {design[i]}.", nameof(design));
                }
            }
        }

        /// <summary>The standardisation the summary rescales by; its rows are not read.</summary>
        public CoxData AsCoxData() => _shape;

        /// <summary>lifelines' <c>_compute_cumulative_baseline_hazard</c>, at every distinct event time.</summary>
        public CoxBaseline Baseline(double[] beta)
        {
            int p = beta.Length;
            var hazards = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                double eta = 0.0;
                for (int a = 0; a < p; a++)
                {
                    eta += Design[(i * p) + a] * beta[a];
                }

                hazards[i] = Math.Exp(eta);
            }

            double[] times = [.. Enumerable.Range(0, Count).Where(i => Events[i]).Select(i => Stops[i]).Distinct().OrderBy(t => t)];
            var hazard = new double[times.Length];
            for (int t = 0; t < times.Length; t++)
            {
                double deaths = 0.0;
                double risk = 0.0;
                for (int i = 0; i < Count; i++)
                {
                    if (!(Starts[i] < times[t] && times[t] <= Stops[i]))
                    {
                        continue;
                    }

                    risk += hazards[i];
                    // S1244: an event belongs to the time its interval stops at, exactly.
#pragma warning disable S1244
                    deaths += Events[i] && Stops[i] == times[t] ? Weights[i] : 0.0;
#pragma warning restore S1244
                }

                hazard[t] = deaths / risk;
            }

            return CoxReport.Accumulated(0, times, hazard);
        }
    }
}
