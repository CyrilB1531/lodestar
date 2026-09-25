namespace Lodestar.Conformal.Internal;

/// <summary>MAPIE's <c>_compute_regression_quantile</c>: the ceiling rank, read by numpy's <c>lower</c> method.</summary>
/// <remarks>
/// long-comment: why the arithmetic follows MAPIE's own order.
/// The level is <c>⌈α_ref(n + 1)⌉ / n</c>, clipped at 1, and numpy's <c>lower</c> reads index <c>⌊(n − 1)·level⌋</c>.
/// The lower side's <c>α_ref</c> is MAPIE's <c>(1 − 2a) + a</c>, which is not always <c>1 − a</c> in floating point:
/// <c>0.9 + 0.05</c> is <c>0.9500000000000001</c>, one rank further at <c>n = 59</c>. It is evaluated as written, so the
/// rank moves where MAPIE's moves. A rank past <c>n</c> is an infinite bound, where MAPIE clips its level to 1
/// (decision 0007).
/// </remarks>
internal static class RegressionQuantile
{
    /// <summary>The <paramref name="alphaReference"/> quantile of <paramref name="values"/>, from above.</summary>
    /// <param name="values">The values; reordered in place.</param>
    /// <param name="alphaReference">The level before MAPIE's correction.</param>
    public static double Upper(double[] values, double alphaReference)
    {
        int index = Index(values.Length, alphaReference);
        return index < 0 ? double.PositiveInfinity : Select(values, index);
    }

    /// <summary>The lower bound's quantile at miscoverage <paramref name="alpha"/>: MAPIE's <c>reverse=True</c>.</summary>
    /// <param name="values">The values; reordered in place.</param>
    /// <param name="alpha">The miscoverage this side reads.</param>
    public static double Lower(double[] values, double alpha)
    {
        double alphaReference = (1.0 - (2.0 * alpha)) + alpha;
        int index = Index(values.Length, alphaReference);

        // The reference reads the same rank of the negated values; ascending, that is the rank from the top.
        return index < 0 ? double.NegativeInfinity : Select(values, values.Length - 1 - index);
    }

    /// <summary>The value a full sort would put at <paramref name="k"/>, by Hoare's selection with a median-of-three pivot.</summary>
    /// <remarks>
    /// Linear on average where a sort is <c>n log n</c>: a test point's interval reads one rank of <c>n</c> values per
    /// side, 373 ms by sorting and a fraction of it by selecting at 10,000 samples and 500 points (#1159's benchmark).
    /// The inputs hold no NaN, which every caller refuses first, so the comparisons are a total order.
    /// </remarks>
    private static double Select(double[] values, int k)
    {
        int low = 0;
        int high = values.Length - 1;
        while (low < high)
        {
            (int i, int j) = Partition(values, low, high);
            if (k <= j)
            {
                high = j;
            }
            else if (k >= i)
            {
                low = i;
            }
            else
            {
                return values[k];
            }
        }

        return values[k];
    }

    /// <summary>Hoare's partition of <c>[low, high]</c>: everything left of <c>i</c> is at most the pivot, right of <c>j</c> at least.</summary>
    private static (int I, int J) Partition(double[] values, int low, int high)
    {
        double pivot = MedianOfThree(values[low], values[low + ((high - low) / 2)], values[high]);
        int i = low;
        int j = high;
        while (i <= j)
        {
            while (values[i] < pivot)
            {
                i++;
            }

            while (values[j] > pivot)
            {
                j--;
            }

            if (i <= j)
            {
                (values[i], values[j]) = (values[j], values[i]);
                i++;
                j--;
            }
        }

        return (i, j);
    }

    private static double MedianOfThree(double a, double b, double c) =>
        Math.Max(Math.Min(a, b), Math.Min(Math.Max(a, b), c));

    /// <summary>The 0-based index numpy's <c>lower</c> reads, or −1 when the rank passes <paramref name="n"/>.</summary>
    private static int Index(int n, double alphaReference)
    {
        double rank = Math.Ceiling(alphaReference * (n + 1));
        if (rank > n)
        {
            return -1;
        }

        double level = Math.Min(1.0, rank / n);
        return (int)Math.Floor((n - 1) * level);
    }
}
