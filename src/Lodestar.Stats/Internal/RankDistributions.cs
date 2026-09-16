namespace Lodestar.Stats.Internal;

/// <summary>The exact null distributions of the two rank statistics.</summary>
/// <remarks>
/// Both are counted by dynamic programming rather than by enumerating the
/// arrangements: the signed-rank distribution for n = 25 has 2^25 assignments
/// against 325 table entries.
/// The counts are doubles because they exceed a long: the signed-rank total for
/// n = 60 is 2^60. Only ratios against the total are ever taken, so a 53-bit
/// mantissa loses nothing a p-value compared at 1e-9 relative can see.
/// </remarks>
internal static class RankDistributions
{
    /// <summary>
    /// How many arrangements of two samples of size <paramref name="n"/> and
    /// <paramref name="m"/> give each value of the Mann-Whitney U statistic.
    /// </summary>
    internal static double[] MannWhitneyCounts(int n, int m)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), n, "A sample size cannot be negative.");
        }
        if (m < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(m), m, "A sample size cannot be negative.");
        }

        // U's distribution for (n, m) is the one for (m, n), so rows follow the smaller sample: sized by
        // m alone, 8 against 2,500 built a 400 MB table nine times (#814).
        int rows = Math.Min(n, m);
        int iterations = Math.Max(n, m);
        int max = n * m;
        int width = max + 1;

        // f(i, j, u): arrangements of i of the larger sample and j of the smaller with statistic u,
        // rolling the i dimension through two flat buffers rather than allocating one per step.
        double[] previous = new double[(rows + 1) * width];
        double[] current = new double[(rows + 1) * width];
        for (int j = 0; j <= rows; j++)
        {
            previous[j * width] = 1.0;
        }

        for (int i = 1; i <= iterations; i++)
        {
            Array.Clear(current, 0, current.Length);
            current[0] = 1.0;

            for (int j = 1; j <= rows; j++)
            {
                int here = j * width;
                int below = (j - 1) * width;
                for (int u = 0; u <= max; u++)
                {
                    // Either the next largest value comes from the larger sample, which adds j to the
                    // statistic, or from the smaller, which adds nothing.
                    double fromFirst = u >= j ? previous[here + u - j] : 0.0;
                    current[here + u] = fromFirst + current[below + u];
                }
            }

            (previous, current) = (current, previous);
        }

        double[] counts = new double[width];
        Array.Copy(previous, rows * width, counts, 0, width);
        return counts;
    }

    /// <summary>
    /// How many sign assignments of the ranks <c>1..n</c> give each value of the
    /// positive-rank sum W.
    /// </summary>
    internal static double[] SignedRankCounts(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), n, "A sample size cannot be negative.");
        }

        int max = n * (n + 1) / 2;
        double[] counts = new double[max + 1];
        counts[0] = 1.0;

        // Multiplying by (1 + x^rank) one rank at a time, in place: descending
        // so a rank is never counted twice within its own pass.
        int reach = 0;
        for (int rank = 1; rank <= n; rank++)
        {
            reach += rank;
            for (int w = reach; w >= rank; w--)
            {
                counts[w] += counts[w - rank];
            }
        }

        return counts;
    }
}
