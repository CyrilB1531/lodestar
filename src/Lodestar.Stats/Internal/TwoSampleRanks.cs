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
    private const ulong SignBit = 0x8000_0000_0000_0000UL;
    private const int DigitBits = 8;
    private const int Buckets = 1 << DigitBits;
    private const int Digits = 64 / DigitBits;

    // Per pair of equal samples on a Ryzen 7 8700G: the radix loses at 2,048 (51 us against
    // Array.Sort's 40) and wins at 4,096 (100 against 181); docs/guides/performance.md.
    private const int RadixThreshold = 3_072;

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
            SortKeys(left, n, scratch);
            SortKeys(right, m, scratch);
            return Merge(left, n, right, m);
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(scratch);
            ArrayPool<ulong>.Shared.Return(right);
            ArrayPool<ulong>.Shared.Return(left);
        }
    }

    /// <summary>A key whose unsigned order is the numeric order of a non-<c>NaN</c> double.</summary>
    /// <remarks>
    /// Setting a positive's sign bit lifts it above every negative, and complementing a
    /// negative reverses its magnitude order. <c>-0.0</c> is folded onto <c>+0.0</c> first,
    /// since <c>==</c> calls them one value and a tie group must too.
    /// </remarks>
    internal static ulong OrderKey(double value)
    {
        // S1244: exact comparison is the point -- only the two zeros may share a key.
#pragma warning disable S1244
        double folded = value == 0.0 ? 0.0 : value;
#pragma warning restore S1244
        ulong bits = (ulong)BitConverter.DoubleToInt64Bits(folded);
        return (bits & SignBit) != 0 ? ~bits : bits | SignBit;
    }

    private static void FillKeys(ReadOnlySpan<double> values, ulong[] keys)
    {
        for (int i = 0; i < values.Length; i++)
        {
            keys[i] = OrderKey(values[i]);
        }
    }

    private static void SortKeys(ulong[] keys, int length, ulong[] scratch)
    {
        if (length < RadixThreshold)
        {
            Array.Sort(keys, 0, length);
            return;
        }

        // LSD radix sort, a byte per pass (Knuth, TAOCP vol. 3, 5.2.5); histograms in one sweep,
        // and a byte every key shares (a sample's exponent byte, usually) is skipped.
        Span<int> histograms = stackalloc int[Digits * Buckets];
        histograms.Clear();
        for (int i = 0; i < length; i++)
        {
            ulong key = keys[i];
            for (int digit = 0; digit < Digits; digit++)
            {
                histograms[(digit * Buckets) + (int)((key >> (digit * DigitBits)) & (Buckets - 1))]++;
            }
        }

        ulong[] source = keys;
        ulong[] target = scratch;
        for (int digit = 0; digit < Digits; digit++)
        {
            int shift = digit * DigitBits;
            Span<int> counts = histograms.Slice(digit * Buckets, Buckets);
            if (counts[(int)((source[0] >> shift) & (Buckets - 1))] == length)
            {
                continue;
            }

            ScatterByDigit(source, target, length, shift, counts);
            (source, target) = (target, source);
        }

        if (!ReferenceEquals(source, keys))
        {
            Array.Copy(source, keys, length);
        }
    }

    private static void ScatterByDigit(
        ulong[] source,
        ulong[] target,
        int length,
        int shift,
        Span<int> counts)
    {
        int offset = 0;
        for (int bucket = 0; bucket < Buckets; bucket++)
        {
            int count = counts[bucket];
            counts[bucket] = offset;
            offset += count;
        }

        for (int i = 0; i < length; i++)
        {
            ulong key = source[i];
            target[counts[(int)((key >> shift) & (Buckets - 1))]++] = key;
        }
    }

    private static (double RankSum, double TieCorrection, bool HasTies) Merge(
        ulong[] left,
        int n,
        ulong[] right,
        int m)
    {
        double rankSum = 0.0;
        double correction = 0.0;
        bool ties = false;
        double below = 0.0;
        int i = 0;
        int j = 0;
        while (i < n || j < m)
        {
            ulong value = j >= m || (i < n && left[i] <= right[j]) ? left[i] : right[j];
            int fromLeft = CountRun(left, n, ref i, value);
            int fromRight = CountRun(right, m, ref j, value);

            // The group holds 1-based ranks below+1 .. below+t; each member takes their mean.
            double t = fromLeft + fromRight;
            rankSum += fromLeft * (below + ((t + 1.0) / 2.0));
            correction += (t * t * t) - t;
            ties |= t > 1.0;
            below += t;
        }

        return (rankSum, correction, ties);
    }

    private static int CountRun(ulong[] keys, int length, ref int position, ulong value)
    {
        int start = position;
        while (position < length && keys[position] == value)
        {
            position++;
        }

        return position - start;
    }
}
