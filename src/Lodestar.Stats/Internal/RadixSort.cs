namespace Lodestar.Stats.Internal;

/// <summary>A least-significant-digit radix sort of doubles, with or without the permutation, for large samples.</summary>
/// <remarks>
/// Four passes of sixteen bits over each double's order-preserving bits: linear where <c>Array.Sort(keys, items)</c>
/// is <c>n log n</c>, which is where a rank test on a large sample spends its time (#1162). Equal values stay adjacent,
/// so tie groups are found as after <c>Array.Sort</c>, <c>-0.0</c> beside <c>+0.0</c>; a <c>NaN</c> has no place in
/// the order and goes to <c>Array.Sort</c>, as <see cref="Worthwhile"/> decides.
/// </remarks>
internal static class RadixSort
{
    private const int Bits = 16;
    private const int Buckets = 1 << Bits;
    private const int Passes = 64 / Bits;

    /// <summary>Below this many values the 65,536-bucket histograms cost more than they save: measured, the radix sort
    /// took 204 µs to <c>Array.Sort</c>'s 90 µs at 1,000 values, and 282 µs to 413 µs at 10,000.</summary>
    private const int Threshold = 8192;

    /// <summary>Whether <paramref name="values"/> is long enough to gain, and holds no <c>NaN</c>.</summary>
    public static bool Worthwhile(ReadOnlySpan<double> values)
    {
        if (values.Length < Threshold)
        {
            return false;
        }

        foreach (double value in values)
        {
            if (double.IsNaN(value))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Sorts <paramref name="keys"/> ascending in place, carrying <paramref name="items"/> along.</summary>
    public static void Sort(double[] keys, int[] items)
    {
        int n = keys.Length;
        ulong[] bits = Encode(keys);
        var spareBits = new ulong[n];
        var spareItems = new int[n];
        int[] currentItems = items;
        var counts = new int[Buckets];
        for (int pass = 0; pass < Passes; pass++)
        {
            int shift = pass * Bits;
            if (!Count(bits, shift, counts))
            {
                continue;
            }

            for (int i = 0; i < n; i++)
            {
                int bucket = (int)((bits[i] >> shift) & (Buckets - 1));
                int at = counts[bucket]++;
                spareBits[at] = bits[i];
                spareItems[at] = currentItems[i];
            }

            (bits, spareBits) = (spareBits, bits);
            (currentItems, spareItems) = (spareItems, currentItems);
        }

        Decode(bits, keys);
        if (!ReferenceEquals(currentItems, items))
        {
            Array.Copy(currentItems, items, n);
        }
    }

    /// <summary>Sorts <paramref name="keys"/> ascending in place.</summary>
    public static void Sort(double[] keys)
    {
        int n = keys.Length;
        ulong[] bits = Encode(keys);
        var spare = new ulong[n];
        var counts = new int[Buckets];
        for (int pass = 0; pass < Passes; pass++)
        {
            int shift = pass * Bits;
            if (!Count(bits, shift, counts))
            {
                continue;
            }

            for (int i = 0; i < n; i++)
            {
                spare[counts[(int)((bits[i] >> shift) & (Buckets - 1))]++] = bits[i];
            }

            (bits, spare) = (spare, bits);
        }

        Decode(bits, keys);
    }

    /// <summary>Each bucket's starting offset for one digit; false when every value shares it, so the pass is skipped.</summary>
    private static bool Count(ulong[] bits, int shift, int[] counts)
    {
        Array.Clear(counts, 0, counts.Length);
        foreach (ulong value in bits)
        {
            counts[(int)((value >> shift) & (Buckets - 1))]++;
        }

        int total = 0;
        for (int b = 0; b < Buckets; b++)
        {
            if (counts[b] == bits.Length)
            {
                return false;
            }

            int size = counts[b];
            counts[b] = total;
            total += size;
        }

        return true;
    }

    /// <summary>The bit patterns that sort as the doubles do: negatives inverted, positives with the sign bit set.</summary>
    private static ulong[] Encode(double[] keys)
    {
        var bits = new ulong[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            ulong raw = (ulong)BitConverter.DoubleToInt64Bits(keys[i]);
            bits[i] = (raw & 0x8000_0000_0000_0000UL) != 0 ? ~raw : raw | 0x8000_0000_0000_0000UL;
        }

        return bits;
    }

    private static void Decode(ulong[] bits, double[] keys)
    {
        for (int i = 0; i < bits.Length; i++)
        {
            ulong value = bits[i];
            ulong raw = (value & 0x8000_0000_0000_0000UL) != 0 ? value & 0x7FFF_FFFF_FFFF_FFFFUL : ~value;
            keys[i] = BitConverter.Int64BitsToDouble((long)raw);
        }
    }
}
