using System.Buffers;

namespace Lodestar.Stats.Internal;

/// <summary>Every group's rank sum in the pooled ranking of k groups, and the tie term.</summary>
/// <remarks>
/// <see cref="TwoSampleRanks"/>' merge over k sorted groups instead of two (#719): each tie group's
/// midrank times its members from a group is all that group's rank sum needs. The sums equal adding
/// <see cref="Ranks.Average"/>'s ranks, every partial sum being a half-integer below 2^53, and the tie
/// term visits the groups <see cref="Ranks.TieCorrection"/> does in the same order (KSampleRanksTests).
/// </remarks>
internal static class KSampleRanks
{
    /// <summary>Past this many groups the head scan costs more than a pooled sort, and callers keep it.</summary>
    internal const int MaxGroups = 16;

    /// <summary>Ranks <paramref name="groups"/> together, writing each group's rank sum to <paramref name="rankSums"/>.</summary>
    /// <param name="groups">At most <see cref="MaxGroups"/> non-empty groups holding no <c>NaN</c>.</param>
    /// <param name="total">The groups' combined length.</param>
    /// <param name="rankSums">One entry per group.</param>
    /// <returns>The sum of t^3 - t over the tie groups.</returns>
    internal static double Compute(double[][] groups, int total, Span<double> rankSums)
    {
        int k = groups.Length;
        ulong[] keys = ArrayPool<ulong>.Shared.Rent(total);
        ulong[] scratch = ArrayPool<ulong>.Shared.Rent(total);
        try
        {
            Span<int> starts = stackalloc int[k + 1];
            int offset = 0;
            for (int g = 0; g < k; g++)
            {
                starts[g] = offset;
                double[] group = groups[g];
                for (int i = 0; i < group.Length; i++)
                {
                    keys[offset + i] = RankKeys.OrderKey(group[i]);
                }

                RankKeys.Sort(keys, offset, group.Length, scratch);
                offset += group.Length;
            }

            starts[k] = offset;
            return Merge(keys, starts, rankSums);
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(scratch);
            ArrayPool<ulong>.Shared.Return(keys);
        }
    }

    private static double Merge(ulong[] keys, ReadOnlySpan<int> starts, Span<double> rankSums)
    {
        int k = rankSums.Length;
        Span<int> heads = stackalloc int[k];
        Span<int> taken = stackalloc int[k];
        starts.Slice(0, k).CopyTo(heads);
        rankSums.Clear();

        double correction = 0.0;
        double below = 0.0;
        while (true)
        {
            ulong value = ulong.MaxValue;
            bool any = false;
            for (int g = 0; g < k; g++)
            {
                if (heads[g] < starts[g + 1] && (!any || keys[heads[g]] < value))
                {
                    value = keys[heads[g]];
                    any = true;
                }
            }

            if (!any)
            {
                return correction;
            }

            int t = 0;
            for (int g = 0; g < k; g++)
            {
                int position = heads[g];
                taken[g] = RankKeys.CountRun(keys.AsSpan(0, starts[g + 1]), ref position, value);
                heads[g] = position;
                t += taken[g];
            }

            // The group holds 1-based ranks below+1 .. below+t; each member takes their mean.
            double midrank = below + ((t + 1.0) / 2.0);
            for (int g = 0; g < k; g++)
            {
                rankSums[g] += taken[g] * midrank;
            }

            double tied = t;
            correction += (tied * tied * tied) - tied;
            below += t;
        }
    }
}
