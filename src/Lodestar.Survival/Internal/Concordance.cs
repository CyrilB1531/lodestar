namespace Lodestar.Survival.Internal;

/// <summary>Harrell's concordance index, with <c>lifelines.utils.concordance_index</c>'s rules for ties.</summary>
/// <remarks>
/// A pair is comparable when the earlier duration is an observed event: against any later duration,
/// and against a censoring at the same duration. Two events at one duration are not comparable. The
/// pair is concordant when the earlier subject carries the higher linear predictor, and a tie in the
/// predictor counts one half. Subjects are walked in time order against a Fenwick tree of the events
/// already passed, indexed by predictor rank, so the cost is <c>n log n</c> rather than events × subjects:
/// a quadratic walk took 270 ms at 10,000 subjects, whatever the covariate count.
/// </remarks>
internal static class Concordance
{
    internal static double Harrell(
        ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> coefficients)
    {
        int count = durations.Length;
        double[] eta = LinearPredictor(design, coefficients, count);
        double[] levels = EventLevels(eta, eventObserved);

        int[] order = new int[count];
        double[] keys = durations.ToArray();
        for (int i = 0; i < count; i++)
        {
            order[i] = i;
        }

        Array.Sort(keys, order);
        var pool = new FenwickTree(levels.Length);
        var tally = new Tally();
        int start = 0;
        while (start < count)
        {
            int end = TimeGroupEnd(keys, start);
            Walk(order.AsSpan(start, end - start), eventObserved, eta, levels, pool, tally);
            start = end;
        }

        return tally.Credit / tally.Pairs;
    }

    /// <summary>One time's subjects: its events meet the pool of earlier events and then join it; its
    /// censorings meet that enlarged pool and never join it.</summary>
    private static void Walk(
        ReadOnlySpan<int> group, ReadOnlySpan<bool> eventObserved, double[] eta, double[] levels,
        FenwickTree pool, Tally tally)
    {
        foreach (int subject in group)
        {
            if (eventObserved[subject])
            {
                tally.Compare(eta[subject], levels, pool);
            }
        }

        foreach (int subject in group)
        {
            if (eventObserved[subject])
            {
                pool.Add(LevelsBelow(levels, eta[subject]) + 1);
            }
        }

        foreach (int subject in group)
        {
            if (!eventObserved[subject])
            {
                tally.Compare(eta[subject], levels, pool);
            }
        }
    }

    private static int TimeGroupEnd(double[] keys, int start)
    {
        int end = start + 1;
        // S1244: a tie is the same recorded duration, exactly; a tolerance would merge two times.
#pragma warning disable S1244
        while (end < keys.Length && keys[end] == keys[start])
#pragma warning restore S1244
        {
            end++;
        }

        return end;
    }

    /// <summary>The distinct event predictors, ascending: the ranks the pool is indexed by.</summary>
    private static double[] EventLevels(double[] eta, ReadOnlySpan<bool> eventObserved)
    {
        var values = new List<double>();
        for (int i = 0; i < eta.Length; i++)
        {
            if (eventObserved[i])
            {
                values.Add(eta[i]);
            }
        }

        values.Sort();
        return [.. values.Distinct()];
    }

    /// <summary>How many distinct event predictors lie strictly below <paramref name="value"/>.</summary>
    private static int LevelsBelow(double[] levels, double value)
    {
        int low = 0;
        int high = levels.Length;
        while (low < high)
        {
            int middle = low + ((high - low) / 2);
            if (levels[middle] < value)
            {
                low = middle + 1;
            }
            else
            {
                high = middle;
            }
        }

        return low;
    }

    private static double[] LinearPredictor(ReadOnlySpan<double> design, ReadOnlySpan<double> coefficients, int count)
    {
        int p = coefficients.Length;
        double[] eta = new double[count];
        for (int i = 0; i < count; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < p; j++)
            {
                sum += coefficients[j] * design[(i * p) + j];
            }

            eta[i] = sum;
        }

        return eta;
    }

    /// <summary>The comparable pairs and the credit they earn.</summary>
    private sealed class Tally
    {
        internal double Pairs { get; private set; }

        internal double Credit { get; private set; }

        /// <summary>Compares a later subject against every earlier event in the pool.</summary>
        /// <remarks>An earlier event with a higher predictor is concordant, and an equal one is half.</remarks>
        internal void Compare(double predictor, double[] levels, FenwickTree pool)
        {
            int below = LevelsBelow(levels, predictor);
            // S1244: a tie in the predictor is exact equality; it is what Harrell counts as a half.
#pragma warning disable S1244
            bool matches = below < levels.Length && levels[below] == predictor;
#pragma warning restore S1244
            int strictlyLower = pool.CountUpTo(below);
            int equal = matches ? pool.CountUpTo(below + 1) - strictlyLower : 0;
            Pairs += pool.Total;
            Credit += (pool.Total - strictlyLower - equal) + (0.5 * equal);
        }
    }

    /// <summary>Counts of event predictors by 1-based rank, with prefix sums in logarithmic time.</summary>
    private sealed class FenwickTree(int size)
    {
        private readonly int[] _counts = new int[size + 1];

        internal int Total { get; private set; }

        internal void Add(int rank)
        {
            Total++;
            for (int i = rank; i < _counts.Length; i += i & -i)
            {
                _counts[i]++;
            }
        }

        internal int CountUpTo(int rank)
        {
            int sum = 0;
            for (int i = rank; i > 0; i -= i & -i)
            {
                sum += _counts[i];
            }

            return sum;
        }
    }
}
