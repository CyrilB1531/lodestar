namespace Lodestar.Stats.Internal;

/// <summary>The exact null distribution of Kendall's tau, for a sample with no tie.</summary>
/// <remarks>
/// Kendall, <em>Rank Correlation Methods</em>, 4th edition, 1970: the number of permutations of
/// <c>n</c> items with a given concordant count follows a recurrence in <c>n</c>, and the
/// p-value is that count's tail over <c>n!</c>. The table is walked in place, one row per
/// item, so the cost is <c>O(n · c)</c> cells — which is why <see cref="MaxCells"/> exists.
/// Past 170 items the factorial overflows a double, and the recurrence is normalised as it
/// goes instead, exactly as scipy switches at the same place.
/// </remarks>
internal static class KendallExact
{
    /// <summary>Past 170 items <c>n!</c> is not a double, and the normalised recurrence takes over.</summary>
    private const int FactorialLimit = 171;

    // long-comment: the bound below is a measured ceiling, not a round number, and a reviewer
    // should be able to see the measurement without leaving the source.
    // The table is (c + 1) cells walked once per item, so the cost is n * (c + 1) updates, and
    // c reaches n(n-1)/4 at the centre of the distribution. Measured on a Ryzen 7 8700G
    // (2026-09-23): 20 million cells is about 40 ms, and the worst case at n = 400 --
    // c = 39,900, 16 million cells -- lands at 32 ms. ExactMethod.Auto never asks for a table
    // this large on its own, since it only chooses exact at n <= 33 or at a degenerate tail,
    // so this bounds an explicit ExactMethod.Exact alone.
    internal const long MaxCells = 20_000_000L;

    /// <summary>How many cells <see cref="PValue"/> would walk for this sample; the cost <see cref="MaxCells"/> bounds.</summary>
    internal static long Cells(int n, long concordant)
    {
        long total = (long)n * (n - 1) / 2;
        return (long)n * (Math.Min(concordant, total - concordant) + 1);
    }

    /// <summary>The exact p-value for <paramref name="concordant"/> concordant pairs out of <c>n(n-1)/2</c>.</summary>
    /// <param name="n">The number of pairs of observations; at least one.</param>
    /// <param name="concordant">The concordant count, <c>tot - discordant</c>.</param>
    /// <param name="alternative">Which tail the p-value covers.</param>
    internal static double PValue(int n, long concordant, Alternative alternative)
    {
        long total = (long)n * (n - 1) / 2;

        // The null distribution is symmetric, so only the lower tail is ever summed and the
        // upper one is read as its reflection.
        bool inRightTail = concordant >= total - concordant;
        long c = Math.Min(concordant, total - concordant);

        (double probability, double massAtC) = TwoSidedProbability(n, c, total, alternative);

        if (alternative != Alternative.TwoSided)
        {
            bool wantsRightTail = alternative == Alternative.Greater;

            // Predicted side: half the two-sided p-value. Other side: its complement, plus
            // the mass exactly at the observation, since a p-value includes its own outcome.
            probability = inRightTail == wantsRightTail
                ? probability / 2.0
                : 1.0 - (probability / 2.0) + massAtC;
        }

        return Math.Min(1.0, Math.Max(0.0, probability));
    }

    private static (double Probability, double MassAtC) TwoSidedProbability(
        int n, long c, long total, Alternative alternative)
    {
        if (n == 1)
        {
            return (1.0, 1.0);
        }
        if (n == 2)
        {
            return (1.0, 0.5);
        }
        if (c == 0)
        {
            double probability = n < FactorialLimit ? 2.0 / Factorial(n) : 0.0;
            return (probability, probability / 2.0);
        }
        if (c == 1)
        {
            double probability = n < FactorialLimit + 1 ? 2.0 / Factorial(n - 1) : 0.0;
            return (probability, (n - 1) / Factorial(n));
        }

        // At the exact centre the two tails cover everything. scipy takes the same shortcut
        // and leaves the mass unused, which a two-sided alternative never reads.
        if (4 * c == total * 2 && alternative == Alternative.TwoSided)
        {
            return (1.0, double.NaN);
        }

        return n < FactorialLimit ? Counted(n, c) : Normalised(n, c);
    }

    /// <summary>The recurrence over exact permutation counts, divided by <c>n!</c> at the end.</summary>
    private static (double Probability, double MassAtC) Counted(int n, long c)
    {
        double[] row = Recurrence(n, c, normalise: false);

        double sum = 0.0;
        for (int i = 0; i < row.Length; i++)
        {
            sum += row[i];
        }

        double factorial = Factorial(n);
        return (2.0 * sum / factorial, row[row.Length - 1] / factorial);
    }

    /// <summary>The same recurrence with each row divided as it is built, for <c>n</c> past 170.</summary>
    private static (double Probability, double MassAtC) Normalised(int n, long c)
    {
        double[] row = Recurrence(n, c, normalise: true);

        double sum = 0.0;
        for (int i = 0; i < row.Length; i++)
        {
            sum += row[i];
        }

        return (sum, row[row.Length - 1] / 2.0);
    }

    private static double[] Recurrence(int n, long c, bool normalise)
    {
        double[] row = new double[c + 1];
        row[0] = 1.0;
        row[1] = 1.0;

        for (int j = 3; j <= n; j++)
        {
            double running = 0.0;
            for (int i = 0; i < row.Length; i++)
            {
                running += row[i];
                row[i] = normalise ? running / j : running;
            }

            if (j <= c)
            {
                // Walked downwards so each cell reads the row as it stood before this
                // subtraction: ascending, a cell at or past 2j would already have moved.
                for (long i = c; i >= j; i--)
                {
                    row[i] -= row[i - j];
                }
            }
        }

        return row;
    }

    private static double Factorial(int n)
    {
        double product = 1.0;
        for (int i = 2; i <= n; i++)
        {
            product *= i;
        }

        return product;
    }
}
