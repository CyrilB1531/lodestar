using System.Buffers;
using System.Runtime.InteropServices;
using Lodestar.Text.Search;

namespace Lodestar.Extensions.VectorData;

/// long-comment: why reading both rankings to a depth is enough, and why the scores carry RankFusion.Rrf's bits.
/// <summary>
/// The first records of <see cref="RankFusion.Rrf"/>'s fusion of the whole vector ranking with the
/// keyword ranking, filtered, without ranking either in full (#1214).
/// </summary>
/// <remarks>
/// <para>
/// Fagin's threshold algorithm over reciprocal ranks. Both rankings are read to a depth <c>D</c>; a record
/// neither reaches ranks past <c>D</c> in both, so it fuses to at most <c>2 / (k + D + 1)</c>, or
/// <c>1 / (k + D + 1)</c> once the keyword ranking is read to its end. When the <c>wanted</c>-th best
/// admitted record read fuses strictly above that bound, no unread record can displace it; otherwise the
/// depth doubles, up to every record. A read record's rank in the other ranking is counted exactly.
/// </para>
/// <para>
/// Scores are summed as <see cref="RankFusion.Rrf"/> sums them, vector term first, and a tie keeps vector
/// order, the order it first saw the records in. A round is one pass over the records and one over the
/// matches, each with a binary search into the records read, where sorting the whole vector ranking was
/// <c>O(n log n)</c> and fusing it allocated a dictionary entry per record.
/// </para>
/// </remarks>
internal static class FusedTop
{
    private const ulong SignBit = 0x8000_0000_0000_0000UL;

    /// <summary>The best <paramref name="wanted"/> admitted slots, best first, with their fused scores.</summary>
    /// <param name="held">The records and their vectors.</param>
    /// <param name="query">The query vector, not yet normalized.</param>
    /// <param name="matched">The slots the keywords matched, in any order, and their BM25 scores.</param>
    /// <param name="k">The rank offset.</param>
    /// <param name="wanted">How many to return, at least 1 and at most the records held.</param>
    /// <param name="admits">The filter, or <see langword="null"/>.</param>
    public static List<(int Slot, double Score)> Select<TKey, TRecord>(
        HeldRecords<TKey, TRecord> held,
        ReadOnlySpan<float> query,
        KeywordMatches matched,
        int k,
        int wanted,
        Func<TRecord, bool>? admits)
        where TKey : notnull
        where TRecord : class
    {
        int slots = held.SlotCount;
        float[] scores = ArrayPool<float>.Shared.Rent(slots);
        ulong[] vectorKeys = ArrayPool<ulong>.Shared.Rent(held.Count);
        int[] keywordIndex = ArrayPool<int>.Shared.Rent(slots);
        try
        {
            held.Score(query, scores);
            ReadOnlySpan<int> bits = MemoryMarshal.Cast<float, int>(scores.AsSpan(0, slots));
            int live = 0;
            for (int slot = 0; slot < slots; slot++)
            {
                if (held.At(slot) is not null)
                {
                    vectorKeys[live++] = VectorKey(bits[slot], slot);
                }
            }

            // Index + 1 into the matches, so the cleared array reads as "not matched".
            Array.Clear(keywordIndex, 0, slots);
            var keywordKeys = new KeywordKey[matched.Slots.Length];
            for (int i = 0; i < keywordKeys.Length; i++)
            {
                keywordKeys[i] = new KeywordKey(ScoreKey(matched.Scores[i]), matched.Slots[i]);
                keywordIndex[matched.Slots[i]] = i + 1;
            }

            Func<int, bool>? admitsSlot = admits is null ? null : slot => admits(held.At(slot)!);
            var rankings = new Rankings(scores, vectorKeys, live, keywordKeys, keywordIndex, k, admitsSlot);

            // Past 2k the bound falls below any record ranked near the top of both lists.
            long depth = Math.Max(wanted, 2L * k);
            while (true)
            {
                int reached = (int)Math.Min(depth, int.MaxValue);
                if (rankings.Read(reached, wanted) is { } found)
                {
                    return found;
                }

                depth *= 2;
            }
        }
        finally
        {
            ArrayPool<int>.Shared.Return(keywordIndex);
            ArrayPool<ulong>.Shared.Return(vectorKeys);
            ArrayPool<float>.Shared.Return(scores);
        }
    }

    /// <summary>A record's place in the vector order as one number, smaller ahead.</summary>
    /// <remarks>
    /// Score descending as <see cref="float.CompareTo(float)"/> orders it, then slot: negative zero reads as
    /// zero, which it ties with, and NaN goes last. The total order <see cref="TopHits"/> ranks by.
    /// </remarks>
    private static ulong VectorKey(int bits, int slot)
    {
        uint u = (uint)bits;
        uint descending;
        if ((u & 0x7FFF_FFFFu) > 0x7F80_0000u)
        {
            descending = uint.MaxValue;
        }
        else
        {
            u = u == 0x8000_0000u ? 0u : u;
            descending = (u & 0x8000_0000u) != 0 ? u : ~(u | 0x8000_0000u);
        }

        return ((ulong)descending << 32) | (uint)slot;
    }

    /// <summary>A BM25 score as a number that orders descending as <see cref="double.CompareTo(double)"/> does, smaller ahead.</summary>
    private static ulong ScoreKey(double score)
    {
        if (double.IsNaN(score))
        {
            return ulong.MaxValue;
        }

        ulong u = (ulong)BitConverter.DoubleToInt64Bits(score);
        u = u == SignBit ? 0UL : u;
        return (u & SignBit) != 0 ? u : ~(u | SignBit);
    }

    /// <summary>A match's place in the keyword order: score descending, then slot, as <c>Bm25Index.Top</c> orders.</summary>
    private readonly struct KeywordKey(ulong score, int slot) : IComparable<KeywordKey>
    {
        public ulong Score { get; } = score;

        public int Slot { get; } = slot;

        public int CompareTo(KeywordKey other)
        {
            int byScore = Score.CompareTo(other.Score);
            return byScore != 0 ? byScore : Slot.CompareTo(other.Slot);
        }
    }

    /// <summary>One query's two rankings, as keys, read to a depth each round.</summary>
    private sealed class Rankings(
        float[] scores,
        ulong[] vectorKeys,
        int live,
        KeywordKey[] keywordKeys,
        int[] keywordIndex,
        int k,
        Func<int, bool>? admits)
    {
        private readonly Dictionary<int, bool> _admitted = [];

        /// <summary>The answer if both rankings read to <paramref name="depth"/> settle it, else <see langword="null"/>.</summary>
        public List<(int Slot, double Score)>? Read(int depth, int wanted)
        {
            ulong[] topVector = Smallest(vectorKeys.AsSpan(0, live), depth);
            KeywordKey[] topKeyword = Smallest<KeywordKey>(keywordKeys, depth);

            // Each record read, with its vector rank and its keyword rank; zero is not yet known, or not matched.
            var read = new Dictionary<int, (int Vector, int Keyword)>(topVector.Length + topKeyword.Length);
            for (int i = 0; i < topVector.Length; i++)
            {
                read[(int)(uint)topVector[i]] = (i + 1, 0);
            }

            for (int i = 0; i < topKeyword.Length; i++)
            {
                int slot = topKeyword[i].Slot;
                read[slot] = (read.TryGetValue(slot, out (int Vector, int Keyword) ranks) ? ranks.Vector : 0, i + 1);
            }

            CountVectorRanks(read);
            if (topKeyword.Length < keywordKeys.Length)
            {
                CountKeywordRanks(read);
            }

            List<(int Slot, double Score)> fused = Fuse(read);
            if (topVector.Length < live)
            {
                // A record read in neither ranking ranks past depth in both, or in the vector one alone.
                double past = 1.0 / ((double)k + depth + 1);
                double bound = topKeyword.Length < keywordKeys.Length ? past + past : past;
                if (fused.Count < wanted || fused[wanted - 1].Score <= bound)
                {
                    return null;
                }
            }

            return fused.Count > wanted ? fused.GetRange(0, wanted) : fused;
        }

        /// <summary>The admitted records read, by fused score descending, ties in vector order.</summary>
        private List<(int Slot, double Score)> Fuse(Dictionary<int, (int Vector, int Keyword)> read)
        {
            var fused = new List<(int Slot, double Score, ulong Order)>(read.Count);
            foreach (KeyValuePair<int, (int Vector, int Keyword)> entry in read)
            {
                if (!Admits(entry.Key))
                {
                    continue;
                }

                // RankFusion.Rrf's own terms, in double and in its order: vector ranking first.
                double score = 1.0 / ((double)k + entry.Value.Vector);
                if (entry.Value.Keyword != 0)
                {
                    score += 1.0 / ((double)k + entry.Value.Keyword);
                }

                fused.Add((entry.Key, score, KeyOf(entry.Key)));
            }

            fused.Sort((x, y) =>
            {
                int byScore = y.Score.CompareTo(x.Score);
                return byScore != 0 ? byScore : x.Order.CompareTo(y.Order);
            });
            return [.. fused.Select(hit => (hit.Slot, hit.Score))];
        }

        /// <summary>Fills in the vector rank of every record read that the vector depth did not reach.</summary>
        private void CountVectorRanks(Dictionary<int, (int Vector, int Keyword)> read)
        {
            ulong[] unknown = [.. read.Where(entry => entry.Value.Vector == 0).Select(entry => KeyOf(entry.Key))];
            int[] ahead = Ahead(vectorKeys.AsSpan(0, live), unknown);
            for (int t = 0; t < unknown.Length; t++)
            {
                int slot = (int)(uint)unknown[t];
                read[slot] = (ahead[t] + 1, read[slot].Keyword);
            }
        }

        /// <summary>Fills in the keyword rank of every matched record read that the keyword depth did not reach.</summary>
        private void CountKeywordRanks(Dictionary<int, (int Vector, int Keyword)> read)
        {
            KeywordKey[] unknown = [.. read
                .Where(entry => entry.Value.Keyword == 0 && keywordIndex[entry.Key] != 0)
                .Select(entry => keywordKeys[keywordIndex[entry.Key] - 1])];
            int[] ahead = Ahead<KeywordKey>(keywordKeys, unknown);
            for (int t = 0; t < unknown.Length; t++)
            {
                int slot = unknown[t].Slot;
                read[slot] = (read[slot].Vector, ahead[t] + 1);
            }
        }

        private ulong KeyOf(int slot) => VectorKey(MemoryMarshal.Cast<float, int>(scores.AsSpan(slot, 1))[0], slot);

        private bool Admits(int slot)
        {
            if (admits is null)
            {
                return true;
            }

            if (!_admitted.TryGetValue(slot, out bool admitted))
            {
                admitted = admits(slot);
                _admitted[slot] = admitted;
            }

            return admitted;
        }

        /// <summary>The <paramref name="count"/> smallest of <paramref name="keys"/>, ascending.</summary>
        /// <remarks>A max-heap whose root is the largest kept, so a key that cannot enter costs one comparison.</remarks>
        private static T[] Smallest<T>(ReadOnlySpan<T> keys, int count)
            where T : IComparable<T>
        {
            if (count >= keys.Length)
            {
                T[] all = keys.ToArray();
                Array.Sort(all);
                return all;
            }

            var heap = new T[count];
            int size = 0;
            foreach (T key in keys)
            {
                if (size < count)
                {
                    heap[size] = key;
                    MaxHeap.SiftUp(heap, size++);
                }
                else if (key.CompareTo(heap[0]) < 0)
                {
                    heap[0] = key;
                    MaxHeap.SiftDown(heap, size);
                }
            }

            Array.Sort(heap);
            return heap;
        }

        /// <summary>For each of <paramref name="targets"/>, sorted here, how many of <paramref name="keys"/> are smaller.</summary>
        /// <remarks>
        /// A key smaller than target <c>t</c> is smaller than every later one, so one count at the first it
        /// beats, prefix-summed, answers them all.
        /// </remarks>
        private static int[] Ahead<T>(ReadOnlySpan<T> keys, T[] targets)
            where T : IComparable<T>
        {
            Array.Sort(targets);
            var ahead = new int[targets.Length];
            if (targets.Length == 0)
            {
                return ahead;
            }

            T last = targets[^1];
            foreach (T key in keys)
            {
                if (key.CompareTo(last) < 0)
                {
                    ahead[FirstAbove(targets, key)]++;
                }
            }

            for (int t = 1; t < ahead.Length; t++)
            {
                ahead[t] += ahead[t - 1];
            }

            return ahead;
        }

        /// <summary>The first of the sorted <paramref name="targets"/> that <paramref name="key"/> is smaller than; one is.</summary>
        private static int FirstAbove<T>(T[] targets, T key)
            where T : IComparable<T>
        {
            int low = 0;
            int high = targets.Length - 1;
            while (low < high)
            {
                int middle = low + ((high - low) / 2);
                if (key.CompareTo(targets[middle]) < 0)
                {
                    high = middle;
                }
                else
                {
                    low = middle + 1;
                }
            }

            return low;
        }
    }
}
