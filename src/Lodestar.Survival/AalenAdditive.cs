using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>Aalen's additive hazards model: each covariate adds to the hazard, by an amount that moves with time.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines</c> 0.30.3's <c>AalenAdditiveFitter</c>: at each event time, the ridge-penalised
/// least squares of that time's deaths on the scaled, weighted covariates of the subjects at risk. Its readings are
/// kept: <b>a subject leaves the risk set only at an event time</b>, so one censored between two stays to the end, and
/// <b>the cumulative variance is rescaled by the deviation</b>, not its square. The fit stops once no more than three
/// subjects per covariate remain, as lifelines' does; a singular system gives zero increments. Thread-safe.
/// </remarks>
public static class AalenAdditive
{
    private static readonly AalenOptions Defaults = new();

    // Rounding in a singular system's pivot grows with the sums behind it: 8 ulps of the diagonal per subject at risk
    // covers the 76 ulps measured at 20,000 subjects, while a solvable system's pivot is orders above it.
    private const double SingularPivotPerSubject = 8 * 2.220446049250313e-16;

    /// <summary>Fits the model to right-censored durations, lifelines' <c>fit</c>.</summary>
    /// <param name="design">The covariates, row-major, one row per subject; no intercept column, which <see cref="AalenOptions.FitIntercept"/> adds last.</param>
    /// <param name="durations">One non-negative, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="featureCount">The number of covariates, the design's row length.</param>
    /// <param name="options">The level, the intercept and the two penalties; <see langword="null"/> for the defaults.</param>
    /// <returns>The cumulative coefficients with their variance, the slopes table, the concordance, and the predictions.</returns>
    /// <exception cref="ArgumentException">The spans do not match the subjects, a value is not finite, a duration is negative, no subject has the event, fewer than two subjects are given, a covariate does not vary beside the intercept, or there is no column at all, or the subjects, the event times or the columns themselves times the columns are more cells than one array holds.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is negative.</exception>
    public static AalenSummary Fit(
        ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount, AalenOptions? options = null) =>
        Fit(design, durations, eventObserved, [], featureCount, options);

    /// <summary>Fits the model with subject weights, lifelines' <c>weights_col</c>.</summary>
    /// <param name="design">The covariates, row-major, one row per subject.</param>
    /// <param name="durations">One non-negative, finite duration per subject.</param>
    /// <param name="eventObserved">Whether each duration ends in the event.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="featureCount">The number of covariates.</param>
    /// <param name="options">The level, the intercept and the two penalties; <see langword="null"/> for the defaults.</param>
    /// <returns>The cumulative coefficients with their variance, the slopes table, the concordance, and the predictions.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or the weights are neither empty nor one positive, finite value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is negative.</exception>
    public static AalenSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        int featureCount,
        AalenOptions? options = null)
    {
        AalenOptions settings = options ?? Defaults;
        int n = durations.Length;
        Validate(durations, eventObserved, weights);
        double[] raw = AftDesign.Validate(design, n, featureCount, settings.FitIntercept);
        int d = featureCount + (settings.FitIntercept ? 1 : 0);

        // lifelines sorts the subjects by duration before anything else; the fit reads them in that order.
        double[] given = durations.ToArray();
        bool[] observed = eventObserved.ToArray();
        double[] weighed = weights.IsEmpty ? [.. Enumerable.Repeat(1.0, n)] : weights.ToArray();
        int[] order = [.. Enumerable.Range(0, n).OrderBy(i => given[i])];
        double[] times = [.. order.Select(i => given[i])];
        bool[] events = [.. order.Select(i => observed[i])];
        double[] roots = [.. order.Select(i => Math.Sqrt(weighed[i]))];
        double[] x = Rows(raw, order, featureCount, settings.FitIntercept);
        (double[] deviations, int constant) = Deviations(x, n, d, featureCount, settings.FitIntercept);
        if (constant >= 0)
        {
            throw new ArgumentException($"Covariate {constant} does not vary, or its deviation is not representable, so beside the intercept it has no coefficient.", nameof(design));
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < d; j++)
            {
                x[(i * d) + j] = x[(i * d) + j] / deviations[j] * roots[i];
            }
        }

        (double[] eventTimes, double[] increments, double[] variances) = Increments(x, times, events, roots, d, settings);
        int m = eventTimes.Length;
        var hazards = new double[TableLength.Of(m, d, ResultTable.FeatureCount)];
        var cumulative = new double[TableLength.Of(m, d, ResultTable.FeatureCount)];
        var variance = new double[TableLength.Of(m, d, ResultTable.FeatureCount)];
        for (int j = 0; j < d; j++)
        {
            double sum = 0.0;
            double spread = 0.0;
            for (int t = 0; t < m; t++)
            {
                sum += increments[(t * d) + j];
                spread += variances[(t * d) + j];
                hazards[(t * d) + j] = increments[(t * d) + j] / deviations[j];
                cumulative[(t * d) + j] = sum / deviations[j];
                variance[(t * d) + j] = spread / deviations[j];
            }
        }

        return AalenSummary.Build(
            new AalenSummary.Fitted(eventTimes, hazards, cumulative, variance, featureCount, settings),
            AtRisk(durations, eventTimes),
            (raw, times, events));
    }

    private static void Validate(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights)
    {
        int n = durations.Length;
        if (eventObserved.Length != n || (!weights.IsEmpty && weights.Length != n))
        {
            throw new ArgumentException(
                $"One duration, one flag and one weight are needed per subject; got {n}, {eventObserved.Length} and {weights.Length}.",
                nameof(eventObserved));
        }

        if (n < 2)
        {
            throw new ArgumentException("The covariates are scaled by their sample deviation, which two subjects at least define.", nameof(durations));
        }

        foreach (double duration in durations)
        {
            if (!(duration >= 0.0) || double.IsInfinity(duration))
            {
                throw new ArgumentException($"A duration is non-negative and finite; durations holds {duration}.", nameof(durations));
            }
        }

        foreach (double weight in weights)
        {
            if (!(weight > 0.0) || double.IsInfinity(weight))
            {
                throw new ArgumentException($"A weight is positive and finite; weights holds {weight}.", nameof(weights));
            }
        }

        if (eventObserved.IndexOf(true) < 0)
        {
            throw new ArgumentException("The increments are estimated at the event times, and every subject here is censored.", nameof(eventObserved));
        }
    }

    /// <summary>The sorted subjects' covariates, the intercept last where there is one.</summary>
    private static double[] Rows(double[] raw, int[] order, int featureCount, bool intercept)
    {
        int d = featureCount + (intercept ? 1 : 0);
        var x = new double[TableLength.Of(order.Length, d, "design")];
        for (int i = 0; i < order.Length; i++)
        {
            for (int j = 0; j < featureCount; j++)
            {
                x[(i * d) + j] = raw[(order[i] * featureCount) + j];
            }

            if (intercept)
            {
                x[(i * d) + featureCount] = 1.0;
            }
        }

        return x;
    }

    /// <summary>pandas' sample deviation of each column: one for the intercept, and for a constant column without one.</summary>
    private static (double[] Deviations, int Constant) Deviations(double[] x, int n, int d, int featureCount, bool intercept)
    {
        var deviations = new double[d];
        int constant = -1;
        for (int j = 0; j < d; j++)
        {
            if (j == featureCount)
            {
                deviations[j] = 1.0;
                continue;
            }

            double mean = 0.0;
            for (int i = 0; i < n; i++)
            {
                mean += x[(i * d) + j];
            }

            mean /= n;
            double squares = 0.0;
            bool varies = false;
            for (int i = 0; i < n; i++)
            {
                double gap = x[(i * d) + j] - mean;
                squares += gap * gap;
                // S1244: constant means every value the same number; the deviation of one keeps rounding.
#pragma warning disable S1244
                varies |= x[(i * d) + j] != x[j];
#pragma warning restore S1244
            }

            double deviation = Math.Sqrt(squares / (n - 1));
            // A deviation that squares round to zero or overflow is as unusable as none.
            bool usable = varies && deviation > 0.0 && !double.IsInfinity(deviation);
            if (intercept && !usable && constant < 0)
            {
                constant = j;
            }

            // Without an intercept lifelines leaves a constant column unscaled; with one it would divide by its deviation.
            deviations[j] = !intercept && deviation < 1e-8 ? 1.0 : deviation;
        }

        return (deviations, constant);
    }

    /// <summary>Each event time's increments and their variances, on the scaled columns, until too few subjects remain.</summary>
    private static (double[] Times, double[] Increments, double[] Variances) Increments(
        double[] x, double[] times, bool[] events, double[] roots, int d, AalenOptions settings)
    {
        int n = times.Length;
        double[] eventTimes = [.. times.Where((_, i) => events[i]).Distinct().OrderBy(t => t)];
        int[][] leaving = Leaving(times, eventTimes);
        var grams = new RiskGrams(x, leaving, n, d);
        var increments = new double[TableLength.Of(eventTimes.Length, d, ResultTable.FeatureCount)];
        var variances = new double[TableLength.Of(eventTimes.Length, d, ResultTable.FeatureCount)];
        double ridge = settings.CoefficientPenalizer + settings.SmoothingPenalizer;
        var previous = new double[d];
        int exited = 0;
        int stop = 0;
        for (int e = 0; e < eventTimes.Length; e++)
        {
            int[] deaths = [.. leaving[e].Where(i => events[i])];
            (double[] v, double[] spread) = Solve(x, grams.At(e), n - exited, deaths, roots, d, ridge, settings.SmoothingPenalizer, previous);
            Array.Copy(v, 0, increments, e * d, d);
            Array.Copy(spread, 0, variances, e * d, d);
            previous = v;
            stop = e + 1;
            if (3 * (d - 1) >= n - exited)
            {
                break;
            }

            exited += leaving[e].Length;
        }

        return (eventTimes.AsSpan(0, stop).ToArray(), increments.AsSpan(0, stop * d).ToArray(), variances.AsSpan(0, stop * d).ToArray());
    }

    /// <summary>The subjects whose duration is exactly each event time: lifelines removes them, and only them, at that time.</summary>
    private static int[][] Leaving(double[] times, double[] eventTimes)
    {
        var leaving = new int[eventTimes.Length][];
        int i = 0;
        for (int e = 0; e < eventTimes.Length; e++)
        {
            // Both ascending: the subjects at this event time are the run of equal times the merge reaches.
            while (i < times.Length && times[i] < eventTimes[e])
            {
                i++;
            }

            int first = i;
            while (i < times.Length && times[i] <= eventTimes[e])
            {
                i++;
            }

            leaving[e] = [.. Enumerable.Range(first, i - first)];
        }

        return leaving;
    }

    /// <summary><c>XᵀX</c> over each event time's risk set, accumulated from the last time back.</summary>
    /// <remarks>
    /// A risk set is the one after it with its own leavers added, so each row enters a sum once and none is taken out:
    /// rebuilt per event time the sums cost events × subjects × d², 139 ms at 10,000 subjects (#1224). A matrix is kept
    /// every √E event times, sparser where they would pass CheckpointCells, and each time's is rebuilt from the next.
    /// </remarks>
    private sealed class RiskGrams
    {
        // Past this many cells of kept matrices, the checkpoints thin out: a wide design held a gigabyte at √E of them.
        private const long CheckpointCells = 1L << 22;

        private readonly double[] _x;
        private readonly int[][] _leaving;
        private readonly int _d;
        private readonly int _spacing;
        private readonly double[][] _checkpoints;

        // The current block's matrices, where they fit within CheckpointCells: At(e) in order re-summed a block per call.
        private readonly double[][]? _block;
        private int _cachedBlock = -1;

        public RiskGrams(double[] x, int[][] leaving, int n, int d)
        {
            (_x, _leaving, _d) = (x, leaving, d);
            int cells = TableLength.Of(d, d, ResultTable.FeatureCount);
            int count = leaving.Length;
            long thinnest = Math.Max(1L, ((long)count * cells) / CheckpointCells);
            _spacing = (int)Math.Min(Math.Max(Math.Max(1L, (long)Math.Sqrt(count)), thinnest), Math.Max(1, count));
            _checkpoints = new double[(count / _spacing) + 2][];
            var staying = new bool[n];
            staying.AsSpan().Fill(true);
            foreach (int i in leaving.SelectMany(subjects => subjects))
            {
                staying[i] = false;
            }

            var gram = new double[cells];
            for (int i = 0; i < n; i++)
            {
                if (staying[i])
                {
                    AddRow(gram, x, i, d);
                }
            }

            if ((long)_spacing * cells <= CheckpointCells)
            {
                _block = new double[_spacing][];
            }

            // The checkpoint past the last one holds the subjects no event time removes.
            _checkpoints[(count + _spacing - 1) / _spacing] = [.. gram];
            for (int e = count - 1; e >= 0; e--)
            {
                foreach (int i in leaving[e])
                {
                    AddRow(gram, x, i, d);
                }

                // Block 0 is summed from checkpoint 1, so none is kept at 0.
                if (e > 0 && e % _spacing == 0)
                {
                    _checkpoints[e / _spacing] = [.. gram];
                }
            }
        }

        /// <summary>Event time <paramref name="e"/>'s matrix, the caller's own to shift in place, in the order the checkpoints
        /// summed it; each event time is read once, in order.</summary>
        public double[] At(int e)
        {
            int block = e / _spacing;
            if (_block is null)
            {
                return Summed(block, e, null);
            }

            if (block != _cachedBlock)
            {
                Summed(block, block * _spacing, _block);
                _cachedBlock = block;
            }

            // Each event time is read once, so the kept matrix is handed over rather than copied again.
            double[] gram = _block[e - (block * _spacing)];
            _block[e - (block * _spacing)] = [];
            return gram;
        }

        /// <summary>The matrix at <paramref name="e"/>, summed from the next checkpoint down, keeping each one on the way
        /// in <paramref name="kept"/> where given.</summary>
        private double[] Summed(int block, int e, double[][]? kept)
        {
            int last = Math.Min((block + 1) * _spacing, _leaving.Length) - 1;
            double[] gram = [.. _checkpoints[block + 1]];
            for (int t = last; t >= e; t--)
            {
                foreach (int i in _leaving[t])
                {
                    AddRow(gram, _x, i, _d);
                }

                if (kept is not null)
                {
                    // The last matrix kept is the running one itself, which nothing sums into after it.
                    kept[t - (block * _spacing)] = t == e ? gram : [.. gram];
                }
            }

            return gram;
        }

        private static void AddRow(double[] gram, double[] x, int i, int d)
        {
            for (int p = 0; p < d; p++)
            {
                double xp = x[(i * d) + p];
                for (int q = 0; q < d; q++)
                {
                    gram[(p * d) + q] += xp * x[(i * d) + q];
                }
            }
        }
    }

    /// <summary>
    /// <c>(XᵀX + (c₁ + c₂)I) v = Xᵀy + c₂ v_prev</c> over the subjects at risk, and the solution's columns for each death,
    /// whose squares summed are the increments' variances; zero where the system is not positive definite, as lifelines
    /// answers its <c>LinAlgError</c>.
    /// </summary>
    // S107: the scaled rows, the risk set's Gram matrix and size, the deaths and their weights, the width, both penalties and the previous step.
#pragma warning disable S107
    private static (double[] Increments, double[] Variances) Solve(
        double[] x, double[] a, int atRisk, int[] deaths, double[] roots, int d, double ridge, double smoothing, double[] previous)
#pragma warning restore S107
    {
        var b = new double[d];
        foreach (int i in deaths)
        {
            for (int p = 0; p < d; p++)
            {
                b[p] += x[(i * d) + p] * roots[i];
            }
        }

        for (int p = 0; p < d; p++)
        {
            a[(p * d) + p] += ridge;
            b[p] += smoothing * previous[p];
        }

        // A penalty makes the system positive definite, and LAPACK then fails only on a pivot at or below zero.
        double relative = ridge > 0.0 ? 0.0 : SingularPivotPerSubject * Math.Max(1, atRisk);
        double[]? lower = Factor(a, d, relative);
        var spread = new double[d];
        if (lower is null)
        {
            return (new double[d], spread);
        }

        double[] v = Substitute(lower, d, b);
        foreach (int i in deaths)
        {
            double[] column = Substitute(lower, d, x.AsSpan(i * d, d));
            for (int p = 0; p < d; p++)
            {
                spread[p] += column[p] * column[p];
            }
        }

        return (v, spread);
    }

    /// <summary>
    /// The Cholesky factor, or <see langword="null"/> where the system is singular: a pivot within
    /// <paramref name="relative"/> of its diagonal. LAPACK's <c>potrf</c> fails on a pivot at or below zero, and a system
    /// singular in exact arithmetic, as a covariate constant among those left at risk makes it, lands either side of
    /// zero by the summation order; the threshold answers such a system as lifelines' <c>LinAlgError</c> does, with zeros.
    /// </summary>
    private static double[]? Factor(double[] a, int d, double relative)
    {
        var l = new double[TableLength.Of(d, d, ResultTable.FeatureCount)];
        for (int j = 0; j < d; j++)
        {
            double pivot = a[(j * d) + j];
            for (int k = 0; k < j; k++)
            {
                pivot -= l[(j * d) + k] * l[(j * d) + k];
            }

            if (!(pivot > relative * a[(j * d) + j]))
            {
                return null;
            }

            double root = Math.Sqrt(pivot);
            l[(j * d) + j] = root;
            for (int i = j + 1; i < d; i++)
            {
                double sum = a[(i * d) + j];
                for (int k = 0; k < j; k++)
                {
                    sum -= l[(i * d) + k] * l[(j * d) + k];
                }

                l[(i * d) + j] = sum / root;
            }
        }

        return l;
    }

    /// <summary><c>(L Lᵀ)⁻¹ r</c> by the two triangular solves.</summary>
    private static double[] Substitute(double[] l, int d, ReadOnlySpan<double> r)
    {
        var y = new double[d];
        for (int i = 0; i < d; i++)
        {
            double sum = r[i];
            for (int k = 0; k < i; k++)
            {
                sum -= l[(i * d) + k] * y[k];
            }

            y[i] = sum / l[(i * d) + i];
        }

        for (int i = d - 1; i >= 0; i--)
        {
            double sum = y[i];
            for (int k = i + 1; k < d; k++)
            {
                sum -= l[(k * d) + i] * y[k];
            }

            y[i] = sum / l[(i * d) + i];
        }

        return y;
    }

    /// <summary>How many subjects are at risk at each event time, lifelines' event table: everyone whose duration is not earlier.</summary>
    private static int[] AtRisk(ReadOnlySpan<double> durations, double[] eventTimes)
    {
        var counts = new int[eventTimes.Length];
        foreach (double duration in durations)
        {
            for (int t = 0; t < eventTimes.Length && eventTimes[t] <= duration; t++)
            {
                counts[t]++;
            }
        }

        return counts;
    }
}
