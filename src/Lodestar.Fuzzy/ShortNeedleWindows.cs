using System.Buffers;
using System.Runtime.CompilerServices;

namespace Lodestar.Fuzzy;

/// <summary>
/// <see cref="Fuzz.PartialRatio"/>'s best window for a needle that fits one machine word, with the
/// needle's LCS equality table built once per call rather than once per window.
/// </summary>
/// <remarks>
/// Hyyrö's bit-parallel LCS recurrence, as <c>BitParallelLcs</c> in Lodestar.Text states it, over the
/// windows rapidfuzz's <c>partial_ratio_short_needle</c> scores (MIT, read for the algorithm). The
/// edge windows each extend the previous one by a character, so all of them cost one scan per side.
/// A skipped window never raises the maximum: <c>PartialRatioWindowTests</c> replays every window.
/// </remarks>
internal static class ShortNeedleWindows
{
    /// <summary>The longest needle whose equality masks fit one <see cref="ulong"/>.</summary>
    public const int MaxNeedle = 64;

    /// <summary>Twice the most distinct characters above Latin-1 a needle can hold, so a probe always ends.</summary>
    private const int WideCapacity = 128;

    /// <summary>Longest text translated on the stack; past it the slot buffer is rented.</summary>
    private const int MaxStackText = 256;

    /// <summary>The best window's ratio, for <c>1 ≤ needle.Length ≤ 64</c> and <c>needle.Length ≤ text.Length</c>.</summary>
    public static double SlideMax(string needle, string text)
    {
        int m = needle.Length;
        int n = text.Length;

        // Slot 0 is every character the needle lacks, so its masks stay zero.
        Span<byte> latin = stackalloc byte[256];
        Span<char> wideKeys = stackalloc char[WideCapacity];
        Span<byte> wideSlots = stackalloc byte[WideCapacity];
        Span<ulong> forward = stackalloc ulong[MaxNeedle + 1];
        Span<ulong> backward = stackalloc ulong[MaxNeedle + 1];

        int distinct = 0;
        for (int i = 0; i < m; i++)
        {
            int slot = Assign(needle[i], latin, wideKeys, wideSlots, ref distinct);
            forward[slot] |= 1UL << i;
            backward[slot] |= 1UL << (m - 1 - i);
        }

        byte[]? rented = n > MaxStackText ? ArrayPool<byte>.Shared.Rent(n) : null;
        Span<byte> slots = rented is null ? stackalloc byte[MaxStackText] : rented.AsSpan();
        try
        {
            for (int j = 0; j < n; j++)
            {
                slots[j] = SlotOf(text[j], latin, wideKeys, wideSlots);
            }

            return Best(forward, backward, slots.Slice(0, n), m);
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    private static double Best(ReadOnlySpan<ulong> forward, ReadOnlySpan<ulong> backward, ReadOnlySpan<byte> slots, int m)
    {
        var best = default(BestWindow);
        int prefixLcs = OfferPrefixes(forward, slots, m, ref best);
        int rightmostLcs = OfferSuffixes(backward, slots, m, ref best);
        OfferFullWindows(forward, slots, m, prefixLcs, rightmostLcs, ref best);
        return best.Ratio();
    }

    /// <summary>Offers every prefix <c>text[..w]</c>, <c>w &lt; m</c>, from one scan.</summary>
    /// <returns>The LCS against <c>text[..(m - 1)]</c>, which bounds the first full window.</returns>
    /// <remarks>
    /// A prefix ending on a character the needle lacks keeps the LCS of the one before it over a
    /// longer length, so it cannot score higher and is not offered.
    /// </remarks>
    private static int OfferPrefixes(ReadOnlySpan<ulong> forward, ReadOnlySpan<byte> slots, int m, ref BestWindow best)
    {
        ulong v = ulong.MaxValue;
        for (int j = 0; j < m - 1; j++)
        {
            v = Step(v, forward[slots[j]]);
            if (slots[j] != 0)
            {
                best.Offer(Lcs(v, m), m + j + 1);
            }
        }
        return Lcs(v, m);
    }

    /// <summary>Offers every suffix <c>text[i..]</c>, <c>i ≥ n - m</c>, from one scan read backward.</summary>
    /// <returns>The LCS of the rightmost full window, which the scan ends on.</returns>
    /// <remarks>
    /// Read against the reversed needle, since <c>LCS(p, t) = LCS(reverse p, reverse t)</c>. A suffix
    /// starting on a character the needle lacks is bounded by the shorter one after it.
    /// </remarks>
    private static int OfferSuffixes(ReadOnlySpan<ulong> backward, ReadOnlySpan<byte> slots, int m, ref BestWindow best)
    {
        int n = slots.Length;
        ulong v = ulong.MaxValue;
        for (int j = n - 1; j >= n - m; j--)
        {
            v = Step(v, backward[slots[j]]);
            if (slots[j] != 0)
            {
                best.Offer(Lcs(v, m), m + n - j);
            }
        }
        return Lcs(v, m);
    }

    /// <summary>Offers the full windows <c>text[i..i+m]</c>, <c>i &lt; n - m</c>, that could still win.</summary>
    /// <remarks>
    /// One starting on a character the needle lacks is bounded by window <c>i + 1</c>. Sliding one
    /// place moves the LCS by at most one, so the last window scored and the rightmost one bound
    /// the rest, and a window whose bound cannot beat the best so far is not scanned.
    /// </remarks>
    private static void OfferFullWindows(
        ReadOnlySpan<ulong> forward, ReadOnlySpan<byte> slots, int m, int prefixLcs, int rightmostLcs, ref BestWindow best)
    {
        int last = slots.Length - m;
        int lastLcs = prefixLcs;
        int lastStart = -1;
        for (int i = 0; i < last && !best.IsExact; i++)
        {
            int ceiling = Math.Min(m, Math.Min(lastLcs + (i - lastStart), rightmostLcs + (last - i)));
            if (slots[i] == 0 || !best.CouldImprove(ceiling, 2 * m))
            {
                continue;
            }

            ulong v = ulong.MaxValue;
            foreach (byte slot in slots.Slice(i, m))
            {
                v = Step(v, forward[slot]);
            }

            lastLcs = Lcs(v, m);
            lastStart = i;
            best.Offer(lastLcs, 2 * m);
        }
    }

    /// <summary>The LCS length a row holds: the needle positions that were matched.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Lcs(ulong v, int m)
    {
        ulong mask = m == 64 ? ulong.MaxValue : (1UL << m) - 1;
        return m - PopCount(v & mask);
    }

    /// <summary>The LCS row advanced by one text character, a set bit being a position not yet matched.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Step(ulong v, ulong matches)
    {
        ulong u = v & matches;
        return (v + u) | (v - u);
    }

    private static int Assign(char c, Span<byte> latin, Span<char> wideKeys, Span<byte> wideSlots, ref int distinct)
    {
        if (c <= 0xFF)
        {
            if (latin[c] == 0)
            {
                latin[c] = (byte)++distinct;
            }
            return latin[c];
        }

        int k = Probe(wideKeys, c);
        if (wideKeys[k] == '\0')
        {
            wideKeys[k] = c;
            wideSlots[k] = (byte)++distinct;
        }
        return wideSlots[k];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte SlotOf(char c, ReadOnlySpan<byte> latin, ReadOnlySpan<char> wideKeys, ReadOnlySpan<byte> wideSlots)
    {
        if (c <= 0xFF)
        {
            return latin[c];
        }

        int k = Probe(wideKeys, c);
        return wideKeys[k] == '\0' ? (byte)0 : wideSlots[k];
    }

    /// <summary>The slot holding <paramref name="c"/>, or the empty one where it would go.</summary>
    /// <remarks><c>'\0'</c> marks an empty slot, which no key above Latin-1 can be.</remarks>
    private static int Probe(ReadOnlySpan<char> keys, char c)
    {
        int k = c & (WideCapacity - 1);
        while (keys[k] != '\0' && keys[k] != c)
        {
            k = (k + 1) & (WideCapacity - 1);
        }
        return k;
    }

#if NET
    private static int PopCount(ulong value) => System.Numerics.BitOperations.PopCount(value);
#else
    /// <summary>The SWAR population count, netstandard2.0 having no BitOperations.</summary>
    private static int PopCount(ulong value)
    {
        value -= (value >> 1) & 0x5555555555555555UL;
        value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
        value = (value + (value >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
        return (int)((value * 0x0101010101010101UL) >> 56);
    }
#endif
}
