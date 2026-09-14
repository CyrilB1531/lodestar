namespace Lodestar.Embeddings.Tokenization;

/// <summary>The merge table as a map from a pair of symbol ids to its rank, by open addressing.</summary>
/// <remarks>
/// A <c>Dictionary&lt;long, int&gt;</c> answered this before, and the lookup it makes for every
/// adjacent pair of every piece was the largest remaining cost of a byte-level encode (#673).
/// Keys are two non-negative ids packed into one <see cref="long"/>, so -1 marks an empty slot,
/// and a Fibonacci hash over a power-of-two table spreads the packed ids with one multiply.
/// Setting an existing pair replaces its rank: a pair listed twice keeps its last occurrence.
/// </remarks>
internal sealed class PairRanks
{
    private const long Empty = -1;
    private const ulong Fibonacci = 0x9E3779B97F4A7C15;

    private readonly long[] _keys;
    private readonly int[] _ranks;
    private readonly int _shift;
    private readonly int _mask;

    /// <summary>Sizes the table at no more than half full for <paramref name="pairs"/> entries.</summary>
    internal PairRanks(int pairs)
    {
        int bits = 4;
        while ((1 << bits) < 2 * Math.Max(pairs, 1))
        {
            bits++;
        }

        _keys = new long[1 << bits];
        _ranks = new int[1 << bits];
        _shift = 64 - bits;
        _mask = (1 << bits) - 1;
#if NET8_0_OR_GREATER
        Array.Fill(_keys, Empty);
#else
        for (int i = 0; i < _keys.Length; i++)
        {
            _keys[i] = Empty;
        }
#endif
    }

    internal static long Key(int left, int right) => ((long)left << 32) | (uint)right;

    /// <summary>Records <paramref name="rank"/> for the pair, replacing any rank it already had.</summary>
    internal void Set(int left, int right, int rank)
    {
        long key = Key(left, right);
        int slot = Slot(key);
        while (_keys[slot] != Empty && _keys[slot] != key)
        {
            slot = (slot + 1) & _mask;
        }

        _keys[slot] = key;
        _ranks[slot] = rank;
    }

    /// <summary>The pair's rank, if the merge table lists it.</summary>
    internal bool TryGetRank(int left, int right, out int rank)
    {
        long key = Key(left, right);
        int slot = Slot(key);
        while (true)
        {
            long found = _keys[slot];
            if (found == key)
            {
                rank = _ranks[slot];
                return true;
            }

            if (found == Empty)
            {
                rank = 0;
                return false;
            }

            slot = (slot + 1) & _mask;
        }
    }

    private int Slot(long key) => (int)(((ulong)key * Fibonacci) >> _shift);
}
