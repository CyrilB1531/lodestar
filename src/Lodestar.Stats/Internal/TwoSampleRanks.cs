using System.Buffers;

namespace Lodestar.Stats.Internal;

/// <summary>The first sample's rank sum in the pooled ranking of two, and the tie term.</summary>
/// <remarks>
/// Neither the pooled sample nor its ranks are materialised: each sample is sorted on its
/// own and the two are merged, since a tie group's midrank times its members from the first
/// sample is all the rank sum needs. The totals equal <see cref="Ranks.Average"/> and
/// <see cref="Ranks.TieCorrection"/> bit for bit: every partial rank sum is a half-integer
/// below 2^53, and the tie term visits the same groups in the same order
/// (TwoSampleRanksTests replays one against the other).
/// </remarks>
internal static class TwoSampleRanks
{
    /// <summary>Ranks <paramref name="first"/> and <paramref name="second"/> together.</summary>
    /// <param name="first">The sample whose rank sum is returned; must hold no <c>NaN</c>.</param>
    /// <param name="second">The other sample; must hold no <c>NaN</c>.</param>
    /// <returns>
    /// The first sample's mid-rank sum, the sum of t^3 - t over the tie groups, and whether
    /// any group holds more than one value.
    /// </returns>
    internal static (double RankSum, double TieCorrection, bool HasTies) Compute(
        ReadOnlySpan<double> first,
        ReadOnlySpan<double> second)
    {
        int n = first.Length;
        int m = second.Length;
        ulong[] left = ArrayPool<ulong>.Shared.Rent(n);
        ulong[] right = ArrayPool<ulong>.Shared.Rent(m);
        ulong[] scratch = ArrayPool<ulong>.Shared.Rent(Math.Max(n, m));
        try
        {
            FillKeys(first, left);
            FillKeys(second, right);
            RankKeys.Sort(left, 0, n, scratch);
            RankKeys.Sort(right, 0, m, scratch);
            return Merge(left.AsSpan(0, n), right.AsSpan(0, m));
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(scratch);
            ArrayPool<ulong>.Shared.Return(right);
            ArrayPool<ulong>.Shared.Return(left);
        }
    }

    private static void FillKeys(ReadOnlySpan<double> values, ulong[] keys)
    {
        for (int i = 0; i < values.Length; i++)
        {
            keys[i] = RankKeys.OrderKey(values[i]);
        }
    }

    private static (double RankSum, double TieCorrection, bool HasTies) Merge(
        ReadOnlySpan<ulong> left,
        ReadOnlySpan<ulong> right)
    {
        int n = left.Length;
        int m = right.Length;
        double rankSum = 0.0;
        double correction = 0.0;
        bool ties = false;
        double below = 0.0;
        int i = 0;
        int j = 0;
        while (i < n || j < m)
        {
            ulong value = j >= m || (i < n && left[i] <= right[j]) ? left[i] : right[j];
            int fromLeft = RankKeys.CountRun(left, ref i, value);
            int fromRight = RankKeys.CountRun(right, ref j, value);

            // The group holds 1-based ranks below+1 .. below+t; each member takes their mean.
            double t = fromLeft + fromRight;
            rankSum += fromLeft * (below + ((t + 1.0) / 2.0));
            correction += (t * t * t) - t;
            ties |= t > 1.0;
            below += t;
        }

        return (rankSum, correction, ties);
    }
}
