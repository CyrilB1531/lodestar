namespace Lodestar.Stats.Internal;

/// <summary>Mid-ranks, and the tie-correction term the rank tests share.</summary>
/// <remarks>
/// The ranks come back indexed by the input's own positions rather than sorted,
/// because every caller here sums the ranks belonging to one of two interleaved
/// samples and would otherwise have to invert the ordering itself.
/// </remarks>
internal static class Ranks
{
    internal static double[] Average(ReadOnlySpan<double> values) => AverageWithTies(values, out _, out _);

    /// <summary>The mid-ranks, with the tie-correction term <c>Σ (t³ − t)</c> and whether any tie exists, read off the same tie groups.</summary>
    /// <remarks>The groups are walked in the order a second sort would walk them, so both terms are bit for bit its own.</remarks>
    internal static double[] AverageWithTies(ReadOnlySpan<double> values, out double tieCorrection, out bool hasTies)
    {
        // An empty sample is refused by the overload below, which needs one value as its scratch does.
        int n = values.Length;
        double[] ranks = new double[n];
        AverageWithTies(values, new int[n], new double[n], ranks, out tieCorrection, out hasTies);
        return ranks;
    }

    /// <summary>The same into <paramref name="ranks"/>, with <paramref name="order"/> and <paramref name="sorted"/> as scratch, each as long as the values.</summary>
    /// <remarks>A caller ranking many short samples reuses the three: Friedman's three arrays a block took 136 MB at 3 × 1e6 (#1718).</remarks>
    internal static void AverageWithTies(
        ReadOnlySpan<double> values, int[] order, double[] sorted, double[] ranks, out double tieCorrection, out bool hasTies)
    {
        int n = values.Length;
        // A longer reused buffer would sort its stale tail into the ranks, and an empty sample has none.
        if (n == 0 || order.Length != n || sorted.Length != n || ranks.Length != n)
        {
            throw new ArgumentException("Cannot rank an empty sample, or into scratch arrays of another length.", nameof(values));
        }
        for (int i = 0; i < n; i++)
        {
            order[i] = i;
            sorted[i] = values[i];
        }

        if (RadixSort.Worthwhile(values))
        {
            RadixSort.Sort(sorted, order);
        }
        else
        {
            Array.Sort(sorted, order);
        }

        tieCorrection = 0.0;
        hasTies = false;
        int start = 0;
        while (start < n)
        {
            int end = start;

            // S1244: a tie group is defined by literally equal input values, not
            // nearby ones -- a tolerance would merge distinct values into a false
            // tie or split a true one.
#pragma warning disable S1244
            while (end + 1 < n && sorted[end + 1] == sorted[start])
#pragma warning restore S1244
            {
                end++;
            }

            // Ranks are 1-based, so the group spanning positions [start, end]
            // occupies ranks start+1 .. end+1 and every member takes their mean.
            double shared = (start + end + 2) / 2.0;
            for (int i = start; i <= end; i++)
            {
                ranks[order[i]] = shared;
            }

            double t = end - start + 1;
            tieCorrection += (t * t * t) - t;
            hasTies |= end > start;

            start = end + 1;
        }
    }

    // Checked ahead of Average, never inside it: NaN sorts to the front under
    // Array.Sort and would otherwise take a finite rank rather than none.
    internal static bool HasNaN(ReadOnlySpan<double> values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (double.IsNaN(values[i]))
            {
                return true;
            }
        }

        return false;
    }
}
