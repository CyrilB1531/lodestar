using System.Buffers;
using System.Runtime.CompilerServices;

namespace Lodestar.Fuzzy;

/// <summary>
/// <see cref="Fuzz.PartialRatio(string, string)"/>'s best window for a needle past one machine word: the passes
/// <see cref="ShortNeedleWindows"/> makes, over an equality table of one row per word.
/// </summary>
/// <remarks>
/// The same edge scans, skip rules and slide bound (#714), with the addition's carry threaded from
/// word to word as <c>BitParallelLcs</c> threads it. Replaced one <c>Indel</c> call per window, each
/// rebuilding the needle's table (#720); <c>PartialRatioWindowTests</c> replays every window.
/// </remarks>
internal static class LongNeedleWindows
{
    /// <summary>The best window's ratio, for <c>needle.Length &gt; 64</c> and <c>needle.Length ≤ text.Length</c>.</summary>
    public static double SlideMax(string needle, string text)
    {
        int m = needle.Length;
        int n = text.Length;
        int words = (m + 63) / 64;
        var slots = new NeedleSlots(needle);
        try
        {
            ulong[] forwardRented = ArrayPool<ulong>.Shared.Rent((slots.Count + 1) * words);
            ulong[] backwardRented = ArrayPool<ulong>.Shared.Rent((slots.Count + 1) * words);
            int[] textRented = ArrayPool<int>.Shared.Rent(n);
            ulong[] rowRented = ArrayPool<ulong>.Shared.Rent(words);
            try
            {
                Span<ulong> forward = forwardRented.AsSpan(0, (slots.Count + 1) * words);
                Span<ulong> backward = backwardRented.AsSpan(0, (slots.Count + 1) * words);
                forward.Clear();
                backward.Clear();
                for (int i = 0; i < m; i++)
                {
                    int row = slots.Of(needle[i]) * words;
                    forward[row + (i >> 6)] |= 1UL << (i & 63);
                    int reversed = m - 1 - i;
                    backward[row + (reversed >> 6)] |= 1UL << (reversed & 63);
                }

                Span<int> textSlots = textRented.AsSpan(0, n);
                for (int j = 0; j < n; j++)
                {
                    textSlots[j] = slots.Of(text[j]);
                }

                var scan = new Scan(forward, backward, textSlots, rowRented.AsSpan(0, words), m);
                return scan.Best();
            }
            finally
            {
                ArrayPool<ulong>.Shared.Return(forwardRented);
                ArrayPool<ulong>.Shared.Return(backwardRented);
                ArrayPool<int>.Shared.Return(textRented);
                ArrayPool<ulong>.Shared.Return(rowRented);
            }
        }
        finally
        {
            slots.Dispose();
        }
    }

    /// <summary>The windows of one call, scored over a row of words held in one span.</summary>
    private readonly ref struct Scan
    {
        private readonly ReadOnlySpan<ulong> _forward;
        private readonly ReadOnlySpan<ulong> _backward;
        private readonly ReadOnlySpan<int> _slots;
        private readonly Span<ulong> _row;
        private readonly int _m;
        private readonly ulong _lastMask;

        public Scan(ReadOnlySpan<ulong> forward, ReadOnlySpan<ulong> backward, ReadOnlySpan<int> slots, Span<ulong> row, int m)
        {
            _forward = forward;
            _backward = backward;
            _slots = slots;
            _row = row;
            _m = m;
            int tail = m - ((row.Length - 1) * 64);
            _lastMask = tail == 64 ? ulong.MaxValue : (1UL << tail) - 1;
        }

        public double Best()
        {
            var best = default(BestWindow);
            int prefixLcs = OfferPrefixes(ref best);
            int rightmostLcs = OfferSuffixes(ref best);
            OfferFullWindows(prefixLcs, rightmostLcs, ref best);
            return best.Ratio();
        }

        /// <summary>Offers every prefix <c>text[..w]</c>, <c>w &lt; m</c>; see <see cref="ShortNeedleWindows"/> for the skip.</summary>
        private int OfferPrefixes(ref BestWindow best)
        {
            _row.Fill(ulong.MaxValue);
            for (int j = 0; j < _m - 1; j++)
            {
                Step(_forward, _slots[j]);
                if (_slots[j] != 0)
                {
                    best.Offer(Lcs(), _m + j + 1);
                }
            }
            return Lcs();
        }

        /// <summary>Offers every suffix <c>text[i..]</c>, <c>i ≥ n - m</c>, read backward against the reversed needle.</summary>
        private int OfferSuffixes(ref BestWindow best)
        {
            int n = _slots.Length;
            _row.Fill(ulong.MaxValue);
            for (int j = n - 1; j >= n - _m; j--)
            {
                Step(_backward, _slots[j]);
                if (_slots[j] != 0)
                {
                    best.Offer(Lcs(), _m + n - j);
                }
            }
            return Lcs();
        }

        /// <summary>Offers the full windows that could still win, bounded as <see cref="ShortNeedleWindows"/> bounds them.</summary>
        private void OfferFullWindows(int prefixLcs, int rightmostLcs, ref BestWindow best)
        {
            int last = _slots.Length - _m;
            int lastLcs = prefixLcs;
            int lastStart = -1;
            for (int i = 0; i < last && !best.IsExact; i++)
            {
                int ceiling = Math.Min(_m, Math.Min(lastLcs + (i - lastStart), rightmostLcs + (last - i)));
                if (_slots[i] == 0 || !best.CouldImprove(ceiling, 2 * _m))
                {
                    continue;
                }

                _row.Fill(ulong.MaxValue);
                for (int j = i; j < i + _m; j++)
                {
                    Step(_forward, _slots[j]);
                }

                lastLcs = Lcs();
                lastStart = i;
                best.Offer(lastLcs, 2 * _m);
            }
        }

        /// <summary>The row advanced by one text character, the addition carrying from each word into the next.</summary>
        /// <remarks>
        /// <c>u</c> is a bit-subset of <c>v</c>, so <c>v - u</c> is <c>v &amp; ~u</c> with no borrow, and the carry
        /// out of <c>v + u + carry</c> is <c>(u | (v &amp; ~sum)) &gt;&gt; 63</c>, as <c>BitParallelLcs.CarryOut</c> has it.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Step(ReadOnlySpan<ulong> table, int slot)
        {
            ReadOnlySpan<ulong> matches = table.Slice(slot * _row.Length, _row.Length);
            ulong carry = 0;
            for (int w = 0; w < _row.Length; w++)
            {
                ulong v = _row[w];
                ulong u = v & matches[w];
                ulong sum = v + u + carry;
                carry = (u | (v & ~sum)) >> 63;
                _row[w] = sum | (v & ~u);
            }
        }

        /// <summary>The LCS length the row holds: the needle positions that were matched.</summary>
        private int Lcs()
        {
            int unmatched = 0;
            int last = _row.Length - 1;
            for (int w = 0; w < last; w++)
            {
                unmatched += PopCount(_row[w]);
            }
            return _m - unmatched - PopCount(_row[last] & _lastMask);
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

    /// <summary>A slot per distinct needle character, 0 for every character the needle lacks.</summary>
    /// <remarks>
    /// Latin-1 is indexed directly. What sits above it goes in a probe table sized to twice the needle's
    /// characters above Latin-1, so a probe always ends; <c>'\0'</c> marks an empty slot, which no such key can be.
    /// </remarks>
    private ref struct NeedleSlots
    {
        private readonly int[] _latin;
        private readonly char[] _wideKeys;
        private readonly int[] _wideSlots;
        private readonly int _wideMask;

        public NeedleSlots(string needle)
        {
            int wide = 0;
            for (int i = 0; i < needle.Length; i++)
            {
                wide += needle[i] > 0xFF ? 1 : 0;
            }

            int capacity = 16;
            while (capacity < 2 * wide)
            {
                capacity *= 2;
            }

            _latin = ArrayPool<int>.Shared.Rent(256);
            _wideKeys = ArrayPool<char>.Shared.Rent(capacity);
            _wideSlots = ArrayPool<int>.Shared.Rent(capacity);
            _wideMask = capacity - 1;
            Array.Clear(_latin, 0, 256);
            Array.Clear(_wideKeys, 0, capacity);
            Count = 0;

            foreach (char c in needle)
            {
                if (c <= 0xFF)
                {
                    if (_latin[c] == 0)
                    {
                        _latin[c] = ++Count;
                    }
                    continue;
                }

                int k = Probe(c);
                if (_wideKeys[k] == '\0')
                {
                    _wideKeys[k] = c;
                    _wideSlots[k] = ++Count;
                }
            }
        }

        /// <summary>How many distinct characters the needle holds.</summary>
        public int Count { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int Of(char c)
        {
            if (c <= 0xFF)
            {
                return _latin[c];
            }

            int k = Probe(c);
            return _wideKeys[k] == '\0' ? 0 : _wideSlots[k];
        }

        private readonly int Probe(char c)
        {
            int k = c & _wideMask;
            while (_wideKeys[k] != '\0' && _wideKeys[k] != c)
            {
                k = (k + 1) & _wideMask;
            }
            return k;
        }

        public readonly void Dispose()
        {
            ArrayPool<int>.Shared.Return(_latin);
            ArrayPool<char>.Shared.Return(_wideKeys);
            ArrayPool<int>.Shared.Return(_wideSlots);
        }
    }
}
