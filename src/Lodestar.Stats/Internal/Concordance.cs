using System.Buffers;

namespace Lodestar.Stats.Internal;

/// <summary>Everything Kendall's tau reads off a pair of samples, in one pass of sorts.</summary>
/// <param name="Discordant">Pairs ordered one way in the first sample and the other way in the second.</param>
/// <param name="XTies">Tied pairs within the first sample, <c>sum c(c-1)/2</c> over its tie groups.</param>
/// <param name="YTies">The same within the second sample.</param>
/// <param name="JointTies">Pairs tied in both samples at once.</param>
/// <param name="X0"><c>sum c(c-1)(c-2)</c> over the first sample's tie groups; a variance term.</param>
/// <param name="X1"><c>sum c(c-1)(2c+5)</c> over the same groups; the other variance term.</param>
/// <param name="Y0">As <paramref name="X0"/>, for the second sample.</param>
/// <param name="Y1">As <paramref name="X1"/>, for the second sample.</param>
/// <param name="DistinctX">How many different values the first sample holds; tau-c's scale.</param>
/// <param name="DistinctY">The same for the second sample.</param>
internal readonly record struct ConcordanceCounts(
    long Discordant,
    long XTies,
    long YTies,
    long JointTies,
    double X0,
    double X1,
    double Y0,
    double Y1,
    int DistinctX,
    int DistinctY);

/// <summary>Counts the concordant, discordant and tied pairs of two aligned samples.</summary>
/// <remarks>
/// The quadratic definition is a double loop over the pairs. This orders the pairs by the
/// first sample and counts the inversions of the second, which is the same number in
/// <c>O(n log n)</c> — see <see cref="Inversions"/>. The samples are reduced to dense ranks
/// first, so a pair fits in one <c>ulong</c> and the ordering is a single sort of those keys
/// rather than a sort carrying a comparison delegate.
/// </remarks>
internal static class Concordance
{
    internal static ConcordanceCounts Compute(ReadOnlySpan<double> x, ReadOnlySpan<double> y)
    {
        int n = x.Length;

        // Rented rather than allocated, as KSampleRanks already rents its keys: at ten
        // thousand pairs these five buffers are 440 KB per call, all of it dead on return.
        int[] rankX = ArrayPool<int>.Shared.Rent(n);
        int[] rankY = ArrayPool<int>.Shared.Rent(n);
        ulong[] pairs = ArrayPool<ulong>.Shared.Rent(n);
        int[] ordered = ArrayPool<int>.Shared.Rent(n);
        try
        {
            return Counts(x, y, rankX, rankY, pairs, ordered);
        }
        finally
        {
            ArrayPool<int>.Shared.Return(rankX);
            ArrayPool<int>.Shared.Return(rankY);
            ArrayPool<ulong>.Shared.Return(pairs);
            ArrayPool<int>.Shared.Return(ordered);
        }
    }

    private static ConcordanceCounts Counts(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        int[] rankX,
        int[] rankY,
        ulong[] pairs,
        int[] ordered)
    {
        int n = x.Length;
        (long xTies, double x0, double x1, int distinctX) = DenseRanks(x, rankX);
        (long yTies, double y0, double y1, int distinctY) = DenseRanks(y, rankY);

        // One key per pair: sorting orders by x, then by y inside a tie of x -- so an x-tie
        // never reads as an inversion, which is right, being neither concordant nor discordant.
        for (int i = 0; i < n; i++)
        {
            pairs[i] = ((ulong)(uint)rankX[i] << 32) | (uint)rankY[i];
        }

        // A rented buffer is longer than asked for, so the sort and every walk below are
        // bounded by n rather than by the array's own length.
        Array.Sort(pairs, 0, n);

        long jointTies = 0L;
        int start = 0;
        for (int i = 0; i < n; i++)
        {
            ordered[i] = (int)(uint)pairs[i];
            if (i + 1 == n || pairs[i + 1] != pairs[i])
            {
                long size = i - start + 1;
                jointTies += size * (size - 1) / 2;
                start = i + 1;
            }
        }

        // rankX has done its work above and is the right length, so it serves as the merge's
        // scratch buffer rather than a second allocation of the same size.
        long discordant = Inversions.Count(ordered, n, rankX);

        return new ConcordanceCounts(
            discordant, xTies, yTies, jointTies, x0, x1, y0, y1, distinctX, distinctY);
    }

    /// <summary>Numbers the distinct values of <paramref name="values"/> from one, and totals its tie groups.</summary>
    private static (long Ties, double Moment0, double Moment1, int Distinct) DenseRanks(
        ReadOnlySpan<double> values, int[] ranks)
    {
        int n = values.Length;
        ulong[] keys = ArrayPool<ulong>.Shared.Rent(n);
        int[] order = ArrayPool<int>.Shared.Rent(n);
        try
        {
            return Group(values, ranks, keys, order);
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(keys);
            ArrayPool<int>.Shared.Return(order);
        }
    }

    private static (long Ties, double Moment0, double Moment1, int Distinct) Group(
        ReadOnlySpan<double> values, int[] ranks, ulong[] keys, int[] order)
    {
        int n = values.Length;
        for (int i = 0; i < n; i++)
        {
            keys[i] = RankKeys.OrderKey(values[i]);
            order[i] = i;
        }

        Array.Sort(keys, order, 0, n);

        long ties = 0L;
        double moment0 = 0.0;
        double moment1 = 0.0;
        int distinct = 0;
        int start = 0;
        while (start < n)
        {
            int end = start;
            while (end + 1 < n && keys[end + 1] == keys[start])
            {
                end++;
            }

            distinct++;
            for (int i = start; i <= end; i++)
            {
                ranks[order[i]] = distinct;
            }

            double size = end - start + 1;
            ties += (long)size * ((long)size - 1) / 2;
            moment0 += size * (size - 1.0) * (size - 2.0);
            moment1 += size * (size - 1.0) * ((2.0 * size) + 5.0);

            start = end + 1;
        }

        return (ties, moment0, moment1, distinct);
    }
}
