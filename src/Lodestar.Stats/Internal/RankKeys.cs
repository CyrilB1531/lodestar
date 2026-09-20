namespace Lodestar.Stats.Internal;

/// <summary>Sortable keys for a rank test's values, and the sort the merged rankings run on them.</summary>
/// <remarks>
/// Taken out of <see cref="TwoSampleRanks"/> (#711) so <see cref="KSampleRanks"/> and
/// <see cref="SignedRanks"/> sort the same keys the same way (#719). A sorted run of keys is a
/// sorted run of values, a tie group included, so every caller merges instead of materialising ranks.
/// </remarks>
internal static class RankKeys
{
    private const ulong SignBit = 0x8000_0000_0000_0000UL;
    private const int DigitBits = 8;
    private const int Buckets = 1 << DigitBits;
    private const int Digits = 64 / DigitBits;

    // Per pair of equal samples on a Ryzen 7 8700G: the radix loses at 2,048 (51 us against
    // Array.Sort's 40) and wins at 4,096 (100 against 181).
    private const int RadixThreshold = 3_072;

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

    /// <summary>Sorts <c>array[offset..offset+length]</c> ascending, <paramref name="scratch"/> being the radix's second buffer.</summary>
    /// <remarks>An array segment rather than a span: <c>netstandard2.0</c> sorts arrays only.</remarks>
    internal static void Sort(ulong[] array, int offset, int length, ulong[] scratch)
    {
        if (length < RadixThreshold)
        {
            Array.Sort(array, offset, length);
            return;
        }

        Span<ulong> keys = array.AsSpan(offset, length);

        // LSD radix sort, a byte per pass (Knuth, TAOCP vol. 3, 5.2.5); histograms in one sweep,
        // and a byte every key shares (a sample's exponent byte, usually) is skipped.
        Span<int> histograms = stackalloc int[Digits * Buckets];
        histograms.Clear();
        foreach (ulong key in keys)
        {
            for (int digit = 0; digit < Digits; digit++)
            {
                histograms[(digit * Buckets) + (int)((key >> (digit * DigitBits)) & (Buckets - 1))]++;
            }
        }

        Span<ulong> source = keys;
        Span<ulong> target = scratch.AsSpan(0, length);
        bool inScratch = false;
        for (int digit = 0; digit < Digits; digit++)
        {
            int shift = digit * DigitBits;
            Span<int> counts = histograms.Slice(digit * Buckets, Buckets);
            if (counts[(int)((source[0] >> shift) & (Buckets - 1))] == length)
            {
                continue;
            }

            ScatterByDigit(source, target, shift, counts);
            Span<ulong> swap = source;
            source = target;
            target = swap;
            inScratch = !inScratch;
        }

        if (inScratch)
        {
            source.CopyTo(keys);
        }
    }

    private static void ScatterByDigit(ReadOnlySpan<ulong> source, Span<ulong> target, int shift, Span<int> counts)
    {
        int offset = 0;
        for (int bucket = 0; bucket < Buckets; bucket++)
        {
            int count = counts[bucket];
            counts[bucket] = offset;
            offset += count;
        }

        foreach (ulong key in source)
        {
            target[counts[(int)((key >> shift) & (Buckets - 1))]++] = key;
        }
    }

    /// <summary>How many keys from <paramref name="position"/> on equal <paramref name="value"/>, advancing past them.</summary>
    internal static int CountRun(ReadOnlySpan<ulong> keys, ref int position, ulong value)
    {
        int start = position;
        while (position < keys.Length && keys[position] == value)
        {
            position++;
        }

        return position - start;
    }
}
