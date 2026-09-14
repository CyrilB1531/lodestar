using System.Buffers;

namespace Lodestar.Stats.Internal;

/// <summary>The sums a Wilcoxon signed-rank test reads from the ranks of its magnitudes, without the ranks.</summary>
/// <remarks>
/// The positive and negative differences are sorted apart by magnitude and merged, a group of zeros
/// ranking first where one is ranked at all (#719). The rank sums are half-integers and the squares
/// quarter-integers, so while every partial sum stays below 2^51 each is exact in any order and equals
/// adding <see cref="Ranks.Average"/>'s ranks in input order; <see cref="MaxRanked"/> keeps it there.
/// </remarks>
internal static class SignedRanks
{
    /// <summary>The most values ranked for which the squared ranks sum below 2^51: n³/3 reaches it near 189,000.</summary>
    internal const int MaxRanked = 180_000;

    /// <summary>What the asymptotic signed-rank test needs.</summary>
    /// <param name="Positive">The rank sum of the positive differences.</param>
    /// <param name="Negative">The rank sum of the negative differences.</param>
    /// <param name="ZeroRankSum">The rank sum of the zero group, 0 where zeros are not ranked.</param>
    /// <param name="Squares">The sum of every ranked value's squared rank, zero group included.</param>
    /// <param name="NonZeroSquares">The same sum over the non-zero differences only.</param>
    internal readonly record struct Sums(
        double Positive, double Negative, double ZeroRankSum, double Squares, double NonZeroSquares);

    /// <summary>Ranks the magnitudes of <paramref name="values"/>, which hold no <c>NaN</c>.</summary>
    /// <param name="values">The differences, zeros included.</param>
    /// <param name="rankZeros">Whether the zeros take ranks, which every zero method but Wilcox's does.</param>
    internal static Sums Compute(ReadOnlySpan<double> values, bool rankZeros)
    {
        int positives = 0;
        int negatives = 0;
        foreach (double value in values)
        {
            positives += value > 0.0 ? 1 : 0;
            negatives += value < 0.0 ? 1 : 0;
        }

        int zeros = values.Length - positives - negatives;
        ulong[] up = ArrayPool<ulong>.Shared.Rent(Math.Max(1, positives));
        ulong[] down = ArrayPool<ulong>.Shared.Rent(Math.Max(1, negatives));
        ulong[] scratch = ArrayPool<ulong>.Shared.Rent(Math.Max(1, Math.Max(positives, negatives)));
        try
        {
            int p = 0;
            int q = 0;
            foreach (double value in values)
            {
                if (value > 0.0)
                {
                    up[p++] = RankKeys.OrderKey(value);
                }
                else if (value < 0.0)
                {
                    down[q++] = RankKeys.OrderKey(-value);
                }
            }

            RankKeys.Sort(up, 0, positives, scratch);
            RankKeys.Sort(down, 0, negatives, scratch);
            return Merge(up.AsSpan(0, positives), down.AsSpan(0, negatives), rankZeros ? zeros : 0);
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(scratch);
            ArrayPool<ulong>.Shared.Return(down);
            ArrayPool<ulong>.Shared.Return(up);
        }
    }

    private static Sums Merge(ReadOnlySpan<ulong> up, ReadOnlySpan<ulong> down, int zeros)
    {
        // The zeros share ranks 1 .. zeros, the smallest magnitude there is.
        double zeroRank = (zeros + 1.0) / 2.0;
        double zeroRankSum = zeros * zeroRank;
        double squares = zeros * zeroRank * zeroRank;

        double positive = 0.0;
        double negative = 0.0;
        double nonZeroSquares = 0.0;
        double below = zeros;
        int i = 0;
        int j = 0;
        while (i < up.Length || j < down.Length)
        {
            ulong value = j >= down.Length || (i < up.Length && up[i] <= down[j]) ? up[i] : down[j];
            int fromUp = RankKeys.CountRun(up, ref i, value);
            int fromDown = RankKeys.CountRun(down, ref j, value);

            double t = fromUp + fromDown;
            double midrank = below + ((t + 1.0) / 2.0);
            positive += fromUp * midrank;
            negative += fromDown * midrank;
            nonZeroSquares += t * midrank * midrank;
            below += t;
        }

        return new Sums(positive, negative, zeroRankSum, squares + nonZeroSquares, nonZeroSquares);
    }
}
