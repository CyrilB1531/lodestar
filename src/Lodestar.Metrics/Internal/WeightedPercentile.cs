namespace Lodestar.Metrics.Internal;

/// <summary>
/// The averaged weighted percentile at 50 %, which is what scikit-learn's
/// <c>median_absolute_error</c> takes when it is given sample weights.
/// </summary>
/// <remarks>
/// Not "the value at the halfway point": <see cref="Average"/> always averages
/// two order statistics, coinciding on one when they land on the same index,
/// chosen within scikit-learn's own epsilon tolerance rather than exactly at
/// half.
/// </remarks>
internal static class WeightedPercentile
{
    /// <summary>
    /// numpy's machine epsilon, <c>np.finfo(np.float64).eps</c> — the tolerance
    /// scikit-learn allows the cumulative weight to overshoot the halfway point
    /// by before it stops averaging.
    /// </summary>
    /// <remarks>
    /// Not <see cref="double.Epsilon"/>, the smallest positive subnormal and 292
    /// orders of magnitude smaller.NET has no built-in constant for this.
    /// </remarks>
    private const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary>The median of <paramref name="values"/> under <paramref name="weights"/>.</summary>
    /// <param name="values">The values. Sorted in place; pass a copy if the caller still needs the order.</param>
    /// <param name="weights">One weight per value, or empty for weight 1 each.</param>
    public static double Median(double[] values, double[] weights)
    {
        if (weights.Length == 0)
        {
            return MedianUnweighted(values);
        }

        // Array.Sort(keys, items) sorts by the first array, so values (the sort
        // key) must lead weights — the reverse call would sort by weight instead.
        Array.Sort(values, weights);
        return Average(values, weights, 0.5);
    }

    /// <summary>
    /// The weighted quantile at <paramref name="fraction"/>, which
    /// <see cref="Median"/> is the half of.
    /// </summary>
    /// <remarks>
    /// Only <c>d2_pinball_score</c>'s denominator needs one away from the middle. It is the same
    /// <c>_weighted_percentile(…, average=True)</c> the median takes, so a negative weight moves it
    /// exactly as it moves the median: scikit-learn's value, not a reading that differed (#1546).
    /// </remarks>
    /// <param name="values">The values. Sorted in place.</param>
    /// <param name="weights">One weight per value, or empty for weight 1 each.</param>
    /// <param name="fraction">Where in the weight to read, in <c>[0, 1]</c>.</param>
    public static double Quantile(double[] values, double[] weights, double fraction)
    {
        if (weights.Length == 0)
        {
            Array.Sort(values);
            return Average(values, null, fraction);
        }

        Array.Sort(values, weights);
        return Average(values, weights, fraction);
    }

    /// <summary>
    /// The unweighted median without a full sort. <see cref="Average"/> stays the
    /// single place that decides which order statistic(s) the median needs — this
    /// only selects those positions with quickselect and then defers to it, the
    /// same way the weighted path defers to it after <see cref="Array.Sort(Array)"/>.
    /// </summary>
    private static double MedianUnweighted(double[] values)
    {
        int n = values.Length;
        MedianIndices(n, out int lower, out int upper);

        QuickSelect(values, 0, n - 1, lower);

        if (upper != lower)
        {
            // The partition invariant already leaves values[lower..] at or above
            // the order statistic, so the other middle value is just its minimum.
            int minIndex = lower + 1;
            for (int i = lower + 2; i < n; i++)
            {
                if (values[i] < values[minIndex])
                {
                    minIndex = i;
                }
            }

            if (minIndex != upper)
            {
                Swap(values, upper, minIndex);
            }
        }

        return Average(values, null, 0.5);
    }

    /// <summary>
    /// Partially orders <c>values[from..to]</c> so that <c>values[k]</c> holds the
    /// value that would sit at index <paramref name="k"/> if the range were fully
    /// sorted, with everything to its left <c>&lt;=</c> it and everything to its
    /// right <c>&gt;=</c> it. Everything else in the range is left unordered.
    /// </summary>
    private static void QuickSelect(double[] values, int from, int to, int k)
    {
        // Below this width, a full sort is cheap and sidesteps the edge cases a
        // three-point pivot has on ranges that barely hold three positions.
        const int InsertionCutoff = 12;

        int width = to - from + 1;
        if (width <= InsertionCutoff)
        {
            Array.Sort(values, from, width);
            return;
        }

        // Median-of-three still degrades to O(n^2) on adversarial input (organ
        // pipe, many repeats); this budget bounds it.
        int budget = (2 * FloorLog2(width)) + 4;

        while (true)
        {
            width = to - from + 1;
            if (width <= InsertionCutoff)
            {
                Array.Sort(values, from, width);
                return;
            }

            if (budget-- <= 0)
            {
                Array.Sort(values, from, width);
                return;
            }

            int pivotIndex = Partition(values, from, to);
            if (k == pivotIndex)
            {
                return;
            }

            if (k < pivotIndex)
            {
                to = pivotIndex - 1;
            }
            else
            {
                from = pivotIndex + 1;
            }
        }
    }

    /// <summary>Lomuto partition of <c>values[from..to]</c> around a median-of-three pivot.</summary>
    private static int Partition(double[] values, int from, int to)
    {
        int mid = from + ((to - from) / 2);

        // The middle of the two endpoints and the midpoint becomes the pivot,
        // defeating the sorted/reverse-sorted input a first-or-last pivot fails on.
        if (values[mid] < values[from])
        {
            Swap(values, from, mid);
        }

        if (values[to] < values[from])
        {
            Swap(values, from, to);
        }

        // This single, oppositely-phrased check reaches the same ordering as
        // pairing a check with an unconditional swap, without the wasted pair.
        if (values[mid] < values[to])
        {
            Swap(values, mid, to);
        }

        double pivot = values[to];
        int storeIndex = from;
        for (int i = from; i < to; i++)
        {
            // Unconditional swap, then advance by the comparison, not a branch.
            // `value` must be read before the swap.
            double value = values[i];
            values[i] = values[storeIndex];
            values[storeIndex] = value;
            storeIndex += value < pivot ? 1 : 0;
        }

        Swap(values, storeIndex, to);
        return storeIndex;
    }

    /// <summary>Floor of log2, for a strictly positive <paramref name="value"/>.</summary>
    private static int FloorLog2(int value)
    {
        int bits = 0;
        while (value > 1)
        {
            value >>= 1;
            bits++;
        }

        return bits;
    }

    private static void Swap(double[] values, int i, int j)
    {
        (values[i], values[j]) = (values[j], values[i]);
    }

    /// <summary>
    /// The pair of order-statistic indices the median needs when every weight is
    /// 1: the cumulative count crosses half the total at <c>(n - 1) / 2</c>, and
    /// the last index still at or under half is <c>n / 2</c>. The single place
    /// that derives that pair: <see cref="Average"/>'s closed form for the unweighted
    /// median uses it, and <see cref="MedianUnweighted"/> selects for the same pair.
    /// </summary>
    private static void MedianIndices(int n, out int lower, out int upper)
    {
        lower = (n - 1) / 2;
        upper = n / 2;
    }

    private static double Average(double[] values, double[]? weights, double fraction)
    {
        // S1244: whether the caller asked for the middle, not whether two computed
        // quantities are close -- the median keeps its own closed-form index pair.
#pragma warning disable S1244
        if (weights is null && fraction == 0.5)
#pragma warning restore S1244
        {
            MedianIndices(values.Length, out int lower, out int upper);
            return (values[lower] + values[upper]) / 2.0;
        }

        return weights is null ? Averaged(values, UnitWeights(values.Length), fraction) : Averaged(values, weights, fraction);
    }

    private static double[] UnitWeights(int count)
    {
        var ones = new double[count];
        ones.AsSpan().Fill(1.0);
        return ones;
    }

    /// <summary>
    /// <c>_weighted_percentile(…, average=True)</c> step for step over sorted values: the cumulative weight,
    /// numpy's <c>searchsorted</c> on it, and the averaging rule. A negative weight makes that cumulative weight
    /// non-monotone, where the binary search's own answer is the reference's (#1546).
    /// </summary>
    private static double Averaged(double[] values, double[] weights, double fraction)
    {
        int n = values.Length;
        var cdf = new double[n];
        double running = 0.0;
        bool allZero = true;
        for (int i = 0; i < n; i++)
        {
            running += weights[i];
            cdf[i] = running;

            // S1244: scikit-learn tests `sample_weight == 0` exactly, and so must this.
#pragma warning disable S1244
            allZero &= weights[i] == 0.0;
#pragma warning restore S1244
        }

        if (allZero)
        {
            return double.NaN;
        }

        // percentile_rank / 100 * total, the rank passed in percent as scikit-learn passes it.
        double rank = fraction * 100.0;
        double adjusted = rank / 100.0 * cdf[n - 1];

        // S1244: an exact zero rank is the one numpy nudges with nextafter.
#pragma warning disable S1244
        if (adjusted == 0.0)
#pragma warning restore S1244
        {
            // Leading zero weights are skipped at rank 0 by stepping just above it (#20528 in scikit-learn).
            adjusted = double.Epsilon;
        }

        int last = n - 1;
        int index = Math.Min(SearchSorted(cdf, adjusted, right: false), last);
        bool fractionAbove = cdf[index] - adjusted > MachineEpsilon;
        if (fractionAbove)
        {
            return values[index];
        }

        int next = Math.Min(index + 1, last);

        // S1244: a zero weight skipped exactly, as scikit-learn skips it.
#pragma warning disable S1244
        if (weights[next] == 0.0)
#pragma warning restore S1244
        {
            next = SearchSorted(cdf, cdf[index], right: true);
            if (next > last)
            {
                next = index;
            }
        }

        return (values[index] + values[next]) / 2.0;
    }

    /// <summary>
    /// numpy 2.5's branchless <c>searchsorted</c>: halve by probing <c>base + half</c>, then compare once more.
    /// On a sorted array any binary search agrees; on a non-monotone cumulative weight only this one gives numpy's
    /// index — it matched <c>np.searchsorted</c> on 40,000 random cases where the textbook search matched 97.7 %.
    /// </summary>
    private static int SearchSorted(double[] sorted, double key, bool right)
    {
        int start = 0;
        int length = sorted.Length;
        while (length > 1)
        {
            int half = length >> 1;
            if (Before(sorted[start + half], key, right))
            {
                start += half;
            }

            length -= half;
        }

        if (length == 0)
        {
            return 0;
        }

        return Before(sorted[start], key, right) ? start + 1 : start;
    }

    /// <summary>Whether a cumulative weight falls before <paramref name="key"/>: strictly for a left search, or equal too for a right one.</summary>
    private static bool Before(double value, double key, bool right) => right ? value <= key : value < key;
}
