using System.Buffers;
using System.Runtime.CompilerServices;

namespace Lodestar.Text.Distances;

/// <summary>Bit-parallel LCS length: the fast path under <see cref="Lcs"/>, <see cref="Indel"/> and <c>fuzz.ratio</c>.</summary>
/// <remarks>
/// Myers' machinery — a dense Latin-1 equality table, blocked into 64-bit words — over a
/// different recurrence, Myers carrying substitution and LCS not. <c>V</c> holds the LCS
/// row, a set bit being a position that did not increment, advanced per text character by
/// <c>V = (V + (V &amp; P)) | (V - (V &amp; P))</c>; the answer counts the cleared bits.
/// Derived from the published recurrence (Hyyrö), not transcribed — decision 0002.
/// </remarks>
internal static class BitParallelLcs
{
    /// <summary>The subsequence length, over the dense equality table or the side table beside it.</summary>
    /// <remarks>
    /// It no longer returns <c>false</c>: a pattern above Latin-1 refused here until #302 gave
    /// the kernel a side table, so every pattern has a route. The signature is kept because
    /// <see cref="Lcs"/>'s gate reads it, and because Myers' twin still refuses an empty pattern.
    /// </remarks>
    public static bool TrySubsequenceLength(
        ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        if (pattern.Length <= 64)
        {
            return TrySingleWord(pattern, text, out length);
        }

        // Width is tested before the two-word method is entered, not inside it: its stackalloc
        // zeroes on entry, and a CJK pattern paid that 4 KB for nothing, +10% at 128.
        return pattern.Length <= 2 * 64 && IsLatin1(pattern)
            ? TryTwoWords(pattern, text, out length)
            : TryBlocked(pattern, text, out length);
    }

    internal static bool IsLatin1(ReadOnlySpan<char> pattern)
    {
#if NET
        return pattern.IndexOfAnyExceptInRange('\0', '\u00FF') < 0;
#else
        foreach (char c in pattern)
        {
            if (c > 0xFF)
            {
                return false;
            }
        }
        return true;
#endif
    }

    /// <summary>Words advanced together per text pass on the Latin-1 blocked route.</summary>
    /// <remarks>
    /// Swept over the scattered pair at 512 on a Ryzen 7 8700G: one word per pass read 0.83 of
    /// the row-major kernel, two 0.71, four 0.59 and eight 0.64. Past four the JIT runs out of
    /// registers for the words and spills them to the stack.
    /// </remarks>
    private const int GroupWords = 4;

    /// <summary>One entry per Latin-1 code unit: a text character above it reads no match.</summary>
    private const int Entries = 256;

    /// <summary>Below this, a pattern above Latin-1 takes the DP rather than the side table.</summary>
    /// <remarks>
    /// The side table's probe raises the kernel's floor while leaving the dynamic program's
    /// cost untouched, so a wide pattern crosses four bands later than a Latin-1 one — 6
    /// against 2, measured in #409. Tested where the width is established rather than at the
    /// dispatch, which does not know it: that keeps the Latin-1 path free of the question.
    /// </remarks>
    internal const int WideMinPatternLength = 6;

    /// <summary>Longest pattern for which restoring a held table beats letting <c>stackalloc</c> zero one.</summary>
    /// <remarks>
    /// Swept over the pair corpus at 0, 16, 32 and 64 (#301). Its length-32 bucket reads 152.2
    /// ns/pair held nowhere, 138.5 at 16, 132.1 at 32 and 134.2 at 64, so the curve is flat
    /// either side of 32 and 32 is taken. Myers got the same sweep and every value was a
    /// regression there — the table is held in this kernel and not in that one, for the reason
    /// <see cref="Held"/> gives.
    /// </remarks>
    private const int MaxHeldPattern = 32;

    // One table per thread, all-zero between calls because every exit restores it -- including
    // the refusal, which has already written entries by the time it discovers it must give up.
    [ThreadStatic]
    private static ulong[]? held;

    /// <summary>The thread's equality table, held rather than zeroed on entry.</summary>
    /// <remarks>
    /// <c>Peq[c]</c> has bit <c>i</c> set where <c>pattern[i] == c</c>. Zeroing those 2 KB is a
    /// fixed cost on work that is <c>O(n)</c>, and <c>stackalloc</c> pays it on every call since
    /// nothing here disables <c>localsinit</c>; restoring costs the pattern instead, <c>O(m)</c>.
    /// Myers measured the other way on the same corpus and keeps its <c>stackalloc</c>: this
    /// recurrence is four operations per text character against that one's dozen, so the same
    /// fixed cost is a far larger share of what a call does.
    /// </remarks>
    private static ulong[] Held => held ??= new ulong[Entries];

    private static bool TrySingleWord(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        length = 0;
        if (pattern.Length > MaxHeldPattern)
        {
            return TrySingleWordOverStack(pattern, text, out length);
        }

        ulong[] peq = Held;
        if (!TryFill(pattern, peq))
        {
            // Refused before the wide method's stackalloc, which 0043 records as zeroing on
            // entry whether or not the branch needing it is taken. TryFill has restored Held.
            return pattern.Length >= WideMinPatternLength
                && TrySingleWordWide(pattern, text, out length);
        }

        length = Scan(peq, default, default, pattern.Length, text);
        Restore(pattern, peq);
        return true;
    }

    /// <summary>The same kernel over a table zeroed by <c>localsinit</c>, past the length that pays for.</summary>
    /// <remarks>
    /// A <c>stackalloc</c> anywhere in a method zeroes on entry to it whether or not its branch
    /// is taken, so the held path only avoids the memset by living in a method that has none.
    /// </remarks>
    private static bool TrySingleWordOverStack(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        length = 0;
        Span<ulong> peq = stackalloc ulong[Entries];
        if (!TryFill(pattern, peq))
        {
            // Unreachable below the gate — this path is past MaxHeldPattern — but the two
            // reroutes state the same rule, so neither drifts from the other.
            return pattern.Length >= WideMinPatternLength
                && TrySingleWordWide(pattern, text, out length);
        }

        length = Scan(peq, default, default, pattern.Length, text);
        return true;
    }

    /// <summary>The kernel for a pattern that leaves Latin-1, which carries a side table beside the dense one.</summary>
    /// <remarks>
    /// Its own method for the reason <see cref="TrySingleWordOverStack"/> gives, and because the
    /// side table is 1.25 KB that a Latin-1 pattern must not be charged for.
    /// </remarks>
    private static bool TrySingleWordWide(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        Span<ulong> peq = stackalloc ulong[Entries];
        Span<char> keys = stackalloc char[WideAlphabet.Capacity];
        Span<ulong> masks = stackalloc ulong[WideAlphabet.Capacity];

        WideAlphabet.Fill(pattern, peq, keys, masks);
        length = Scan(peq, keys, masks, pattern.Length, text);
        return true;
    }

    /// <summary>Sets each pattern position's bit, or reports a pattern that leaves Latin-1.</summary>
    /// <returns><c>false</c> when a character exceeds U+00FF, the table restored as it was found.</returns>
    private static bool TryFill(ReadOnlySpan<char> pattern, Span<ulong> table)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            char c = pattern[i];
            if (c > 0xFF)
            {
                // The exit that is easy to miss: i entries are already written, and the next
                // pattern would read them as its own.
                Restore(pattern.Slice(0, i), table);
                return false;
            }
            table[c] |= 1UL << i;
        }
        return true;
    }

    /// <summary>Clears what <see cref="TryFill"/> wrote, restoring the all-zero invariant.</summary>
    private static void Restore(ReadOnlySpan<char> pattern, Span<ulong> table)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            table[pattern[i]] = 0UL;
        }
    }

    /// <summary>One machine word of the LCS recurrence, over the pattern's equality table.</summary>
    /// <remarks>Inlined rather than called: this is the hot loop, and a call here costs measurably.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Scan(
        ReadOnlySpan<ulong> peq, ReadOnlySpan<char> keys, ReadOnlySpan<ulong> masks,
        int m, ReadOnlySpan<char> text)
    {
        // All ones: no position has incremented yet. Carries run upward, so the bits
        // above m are masked at the end rather than kept clean throughout.
        ulong v = ulong.MaxValue;
        for (int j = 0; j < text.Length; j++)
        {
            char tc = text[j];
            ulong p = tc <= 0xFF ? peq[tc] : WideAlphabet.Lookup(keys, masks, tc);
            ulong u = v & p;
            v = (v + u) | (v - u);
        }

        ulong mask = m == 64 ? ulong.MaxValue : (1UL << m) - 1;
        return m - PopCount(v & mask);
    }

    /// <summary>A Latin-1 pattern of 65 to 128 characters, both words held in registers.</summary>
    /// <remarks>
    /// The blocked loop kept its words in an array, so every text character loaded and stored
    /// each one and the next character waited on that store. Two words need no array: 0.43 of
    /// the blocked kernel's time at 128 on a Ryzen 7 8700G. The caller has established the
    /// pattern is Latin-1; a wide one takes the blocked route and its side table.
    /// </remarks>
    private static bool TryTwoWords(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        // Interleaved, peq[2c] then peq[2c + 1], so one slice and one bounds check serve both.
        Span<ulong> peq = stackalloc ulong[2 * Entries];
        for (int i = 0; i < pattern.Length; i++)
        {
            peq[(pattern[i] << 1) + (i >> 6)] |= 1UL << (i & 63);
        }

        ulong v0 = ulong.MaxValue;
        ulong v1 = ulong.MaxValue;
        foreach (char tc in text)
        {
            // A character the table cannot hold matches nothing: no bit moves, no carry forms.
            if (tc > 0xFF)
            {
                continue;
            }

            ReadOnlySpan<ulong> p = peq.Slice(tc << 1, 2);
            ulong u0 = v0 & p[0];
            ulong t0 = v0 + u0;
            ulong carry = CarryOut(v0, u0, t0);
            v0 = t0 | (v0 & ~u0);

            ulong u1 = v1 & p[1];
            v1 = (v1 + u1 + carry) | (v1 & ~u1);
        }

        length = pattern.Length - PopCount(v0) - PopCount(v1 & TailMask(pattern.Length - 64));
        return true;
    }

    /// <summary>The carry out of <c>v + u + carryIn</c>, given that <c>u</c> is a bit-subset of <c>v</c>.</summary>
    /// <remarks>
    /// A full adder carries out of bit 63 of <c>(v &amp; u) | ((v | u) &amp; ~sum)</c>, which the
    /// subset reduces to <c>u | (v &amp; ~sum)</c>. The two comparisons it replaces compiled to
    /// <c>cmp</c>/<c>setb</c> twice, never to <c>adc</c>; this compiles to <c>andn</c>, <c>or</c>
    /// and <c>shr</c>, and read 0.78 of the blocked kernel's time at 512 on its own.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong CarryOut(ulong v, ulong u, ulong sum) => (u | (v & ~sum)) >> 63;

    /// <summary>The bits of a word that hold pattern positions, given how many remain from it.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong TailMask(int remaining)
    {
        if (remaining <= 0)
        {
            return 0UL;
        }

        return remaining >= 64 ? ulong.MaxValue : (1UL << remaining) - 1;
    }

    private static bool TryBlocked(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        // Sized from the pattern's characters above Latin-1, not from its length: a Latin-1
        // pattern gets no side rows at all, which is the table this route had before #302.
        int slots = WideAlphabet.CapacityFor(WideAlphabet.CountWide(pattern));
        return slots == 0
            ? TryGrouped(pattern, text, out length)
            : TryBlockedWide(pattern, text, slots, out length);
    }

    /// <summary>The row-major kernel over the dense table and its side rows, for a pattern that leaves Latin-1.</summary>
    /// <remarks>
    /// Its own method so it gets its own profile. Inlined in <see cref="TryBlocked"/>, a Latin
    /// run tiered that method up with this half cold, and the CJK bucket of 128 then read 1 072
    /// to 1 191 ns against the parent's 964 to 973; split out, 784 to 892 (committed corpus).
    /// </remarks>
    private static bool TryBlockedWide(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, int slots, out int length)
    {
        length = 0;
        int m = pattern.Length;
        int blocks = (m + 63) / 64;

        long rows = (long)(Entries + slots) * blocks;
        if (rows > WideAlphabet.MaxTableLength)
        {
            return false;
        }

        int peqLength = (int)rows;
        ulong[] peqRented = ArrayPool<ulong>.Shared.Rent(peqLength);
        char[] keysRented = ArrayPool<char>.Shared.Rent(slots);
        ulong[] vRented = ArrayPool<ulong>.Shared.Rent(blocks);
        try
        {
            Span<ulong> peq = peqRented.AsSpan(0, peqLength);
            Span<ulong> v = vRented.AsSpan(0, blocks);
            Span<char> keys = keysRented.AsSpan(0, slots);
            peq.Clear();
            keys.Clear();

            for (int i = 0; i < m; i++)
            {
                char c = pattern[i];
                int row;
                if (c <= 0xFF)
                {
                    row = c;
                }
                else
                {
                    int k = WideAlphabet.Probe(keys, c);
                    keys[k] = c;
                    row = Entries + k;
                }
                peq[(row * blocks) + (i >> 6)] |= 1UL << (i & 63);
            }

            for (int b = 0; b < blocks; b++)
            {
                v[b] = ulong.MaxValue;
            }

            for (int j = 0; j < text.Length; j++)
            {
                char tc = text[j];
                int row = tc <= 0xFF
                    ? tc * blocks
                    : WideAlphabet.BlockBase(keys, tc, blocks, Entries);
                if (row >= 0)
                {
                    Advance(v, peq.Slice(row, blocks), blocks);
                }

                // A character neither table holds matches nothing, so every u is zero
                // and Advance would hand v back exactly as it took it.
            }

            length = m - Count(v, blocks, m);
            return true;
        }
        finally
        {
            ArrayPool<char>.Shared.Return(keysRented);
            ArrayPool<ulong>.Shared.Return(peqRented);
            ArrayPool<ulong>.Shared.Return(vRented);
        }
    }

    /// <summary>A Latin-1 pattern past two words, advanced <see cref="GroupWords"/> words per text pass.</summary>
    /// <remarks>
    /// The recurrence at word <c>b</c> and text position <c>j</c> needs only word <c>b</c> at
    /// <c>j - 1</c> and the carry out of word <c>b - 1</c> at <c>j</c>, so the loops may be
    /// swapped. Each group then runs the whole text with its words in registers, and the carry
    /// leaving it waits in <c>carries[j]</c> for the next group: one store per character per
    /// group, against one per word per character. 0.58 of the row-major kernel at 512.
    /// </remarks>
    private static bool TryGrouped(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text, out int length)
    {
        length = 0;
        int m = pattern.Length;
        int groups = (((m + 63) / 64) + GroupWords - 1) / GroupWords;

        // Padded to whole groups: a word past the pattern has an all-zero column, and its
        // bits are masked from the count, so the kernel needs no case for a short group.
        long cells = (long)groups * GroupWords * Entries;
        if (cells > WideAlphabet.MaxTableLength)
        {
            return false;
        }

        int peqLength = (int)cells;
        ulong[] peqRented = ArrayPool<ulong>.Shared.Rent(peqLength);
        ulong[] carriesRented = ArrayPool<ulong>.Shared.Rent(text.Length);
        try
        {
            // Column-major, peq[word * 256 + c]: interleaving each group's four words per
            // character measured slower here (0.71 against 0.58), unlike the two-word kernel.
            Span<ulong> peq = peqRented.AsSpan(0, peqLength);
            Span<ulong> carries = carriesRented.AsSpan(0, text.Length);
            peq.Clear();
            carries.Clear();
            for (int i = 0; i < m; i++)
            {
                peq[((i >> 6) * Entries) + pattern[i]] |= 1UL << (i & 63);
            }

            int set = 0;
            for (int g = 0; g < groups; g++)
            {
                int word = g * GroupWords;
                set += SweepGroup(peq.Slice(word * Entries, GroupWords * Entries), text, carries, m - (word * 64));
            }

            length = m - set;
            return true;
        }
        finally
        {
            ArrayPool<ulong>.Shared.Return(peqRented);
            ArrayPool<ulong>.Shared.Return(carriesRented);
        }
    }

    /// <summary>One group of four words over the whole text; returns the set bits that are pattern positions.</summary>
    /// <remarks>Written out word by word: a loop over the group would put the words back in memory.</remarks>
    private static int SweepGroup(ReadOnlySpan<ulong> columns, ReadOnlySpan<char> text, Span<ulong> carries, int remaining)
    {
        ReadOnlySpan<ulong> p0 = columns.Slice(0, Entries);
        ReadOnlySpan<ulong> p1 = columns.Slice(Entries, Entries);
        ReadOnlySpan<ulong> p2 = columns.Slice(2 * Entries, Entries);
        ReadOnlySpan<ulong> p3 = columns.Slice(3 * Entries, Entries);
        ulong v0 = ulong.MaxValue;
        ulong v1 = ulong.MaxValue;
        ulong v2 = ulong.MaxValue;
        ulong v3 = ulong.MaxValue;

        for (int j = 0; j < carries.Length; j++)
        {
            char tc = text[j];
            if (tc > 0xFF)
            {
                continue;
            }

            ulong u = v0 & p0[tc];
            ulong t = v0 + u + carries[j];
            ulong carry = CarryOut(v0, u, t);
            v0 = t | (v0 & ~u);

            u = v1 & p1[tc];
            t = v1 + u + carry;
            carry = CarryOut(v1, u, t);
            v1 = t | (v1 & ~u);

            u = v2 & p2[tc];
            t = v2 + u + carry;
            carry = CarryOut(v2, u, t);
            v2 = t | (v2 & ~u);

            u = v3 & p3[tc];
            t = v3 + u + carry;
            carries[j] = CarryOut(v3, u, t);
            v3 = t | (v3 & ~u);
        }

        return PopCount(v0 & TailMask(remaining))
            + PopCount(v1 & TailMask(remaining - 64))
            + PopCount(v2 & TailMask(remaining - 128))
            + PopCount(v3 & TailMask(remaining - 192));
    }

    /// <summary>One text character, with only the add's carry crossing words.</summary>
    /// <remarks>
    /// <c>u</c> is <c>v &amp; peq</c>, a bit-subset of <c>v</c>, and subtracting a subset
    /// cannot borrow: <c>v - u</c> is <c>v &amp; ~u</c>, so the borrow this threaded between
    /// words was provably zero (#357). The addition still carries — an asymmetry the LCS
    /// recurrence owns, <c>Myers</c> carrying substitution and rightly keeping both chains.
    /// Inlined because it runs once per text character (#320). Only a wide pattern reaches it.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Advance(Span<ulong> v, ReadOnlySpan<ulong> peqRow, int blocks)
    {
        ulong carry = 0;
        for (int b = 0; b < blocks; b++)
        {
            ulong value = v[b];
            ulong u = value & peqRow[b];
            ulong sum = value + u + carry;
            carry = CarryOut(value, u, sum);
            v[b] = sum | (value & ~u);
        }
    }

    /// <summary>Set bits below <paramref name="m"/>: the positions that never incremented.</summary>
    private static int Count(ReadOnlySpan<ulong> v, int blocks, int m)
    {
        int set = 0;
        for (int b = 0; b < blocks - 1; b++)
        {
            set += PopCount(v[b]);
        }

        int tail = m - ((blocks - 1) * 64);
        ulong mask = tail == 64 ? ulong.MaxValue : (1UL << tail) - 1;
        return set + PopCount(v[blocks - 1] & mask);
    }

#if NET
    internal static int PopCount(ulong value) => System.Numerics.BitOperations.PopCount(value);
#else
    /// <summary>The SWAR population count, netstandard2.0 having no BitOperations.</summary>
    internal static int PopCount(ulong value)
    {
        value -= (value >> 1) & 0x5555555555555555UL;
        value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
        value = (value + (value >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
        return (int)((value * 0x0101010101010101UL) >> 56);
    }
#endif
}
