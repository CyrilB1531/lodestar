using Lodestar.Embeddings.Search;

namespace Lodestar.Extensions.VectorData;

/// <summary>A collection's records by slot, each with its vector normalized once, at its write.</summary>
/// <remarks>
/// Slots are handed out as <see cref="Dictionary{TKey, TValue}"/> hands out its entries: a new key
/// takes the slot freed last, else the next unused one, and a replaced key keeps its own. Slot order
/// is therefore the order the dictionary this replaced enumerated, which is the order ties break in
/// (#1214); <c>CollectionDifferentialTests</c> holds the two side by side. A write costs one row.
/// </remarks>
internal sealed class HeldRecords<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private readonly Dictionary<TKey, int> _slots = [];
    private readonly Stack<int> _free = new();
    private readonly int _dimension;
    private TRecord?[] _records = [];
    private float[] _rows = [];
    private int _used;

    /// <summary>Creates an empty set of records whose vectors are <paramref name="dimension"/> wide.</summary>
    public HeldRecords(int dimension) => _dimension = dimension;

    /// <summary>How many records are held.</summary>
    public int Count => _slots.Count;

    /// <summary>One past the highest slot in use since the last <see cref="Clear"/>: every held record's slot is below it.</summary>
    public int SlotCount => _used;

    /// <summary>The record in <paramref name="slot"/>, or <see langword="null"/> when the slot is free.</summary>
    public TRecord? At(int slot) => _records[slot];

    /// <summary>The normalized vector in <paramref name="slot"/>.</summary>
    public ReadOnlySpan<float> Row(int slot) => _rows.AsSpan(slot * _dimension, _dimension);

    /// <summary>The record held under <paramref name="key"/>, if any.</summary>
    public bool TryGet(TKey key, out TRecord? record)
    {
        if (_slots.TryGetValue(key, out int slot))
        {
            record = _records[slot];
            return true;
        }

        record = null;
        return false;
    }

    /// <summary>The records held, in slot order.</summary>
    public IEnumerable<TRecord> Records()
    {
        for (int slot = 0; slot < _used; slot++)
        {
            if (_records[slot] is { } record)
            {
                yield return record;
            }
        }
    }

    /// <summary>Holds <paramref name="record"/> under <paramref name="key"/>, replacing any record there, and answers its slot.</summary>
    /// <param name="key">The record's key.</param>
    /// <param name="record">The record, already admitted.</param>
    /// <param name="vector">Its vector, <see cref="HeldRecords{TKey, TRecord}"/>'s width, copied and normalized here.</param>
    public int Put(TKey key, TRecord record, ReadOnlySpan<float> vector)
    {
        if (!_slots.TryGetValue(key, out int slot))
        {
            slot = _free.Count > 0 ? _free.Pop() : Grow();
            _slots.Add(key, slot);
        }

        _records[slot] = record;
        Span<float> row = _rows.AsSpan(slot * _dimension, _dimension);
        vector.CopyTo(row);
        Normalize(row);
        return slot;
    }

    /// <summary>Removes the record under <paramref name="key"/>, answering the slot it freed, or -1 when none was held.</summary>
    public int Remove(TKey key)
    {
        if (!_slots.TryGetValue(key, out int slot))
        {
            return -1;
        }

        _slots.Remove(key);
        _records[slot] = null;
        _free.Push(slot);
        return slot;
    }

    /// <summary>Drops every record; the next write takes slot zero again, as a cleared dictionary's does.</summary>
    public void Clear()
    {
        _slots.Clear();
        _free.Clear();
        Array.Clear(_records, 0, _used);
        _used = 0;
    }

    /// <summary>The best <paramref name="wanted"/> records <paramref name="admits"/> lets through, best first.</summary>
    /// <remarks>
    /// Scores are <see cref="EmbeddingIndex.Search"/>'s: the query and the rows normalized as it
    /// normalizes a stored row, which is how it normalizes a query too since #1214, dotted by
    /// <see cref="VectorMath.Dot"/>. Score descending, then slot; the filter runs before any scoring.
    /// </remarks>
    public SearchResult[] Nearest(ReadOnlySpan<float> query, int wanted, Func<TRecord, bool>? admits)
    {
        float[] normalized = Normalized(query);
        var best = new TopHits(wanted);
        for (int slot = 0; slot < _used; slot++)
        {
            TRecord? record = _records[slot];
            if (record is not null && (admits is null || admits(record)))
            {
                best.Offer(new SearchResult(slot, VectorMath.Dot(normalized, Row(slot))));
            }
        }

        return best.Ranked();
    }

    /// <summary>Every held record's score against <paramref name="query"/>, by slot; a free slot's is left unwritten.</summary>
    /// <param name="query">The query, not yet normalized.</param>
    /// <param name="scores">At least <see cref="SlotCount"/> long.</param>
    public void Score(ReadOnlySpan<float> query, Span<float> scores)
    {
        float[] normalized = Normalized(query);
        for (int slot = 0; slot < _used; slot++)
        {
            if (_records[slot] is not null)
            {
                scores[slot] = VectorMath.Dot(normalized, Row(slot));
            }
        }
    }

    /// <summary>The query normalized as a stored row is, in double, so no component overflows a float norm (#1214).</summary>
    private static float[] Normalized(ReadOnlySpan<float> query)
    {
        float[] normalized = query.ToArray();
        Normalize(normalized);
        return normalized;
    }

    /// <summary>Divides a row by its norm exactly as <c>EmbeddingIndex</c> normalizes a stored row, bit for bit.</summary>
    /// <remarks>A scalar sum in order, in double, then <c>(float)(v / norm)</c>; a zero row stays zero.</remarks>
    private static void Normalize(Span<float> row)
    {
        double sum = 0;
        foreach (float v in row)
        {
            sum += (double)v * v;
        }

        double norm = Math.Sqrt(sum);
        // S1244: zero is the one norm the division cannot take, and EmbeddingIndex tests it exactly too.
#pragma warning disable S1244
        if (norm == 0)
#pragma warning restore S1244
        {
            return;
        }

        for (int i = 0; i < row.Length; i++)
        {
            row[i] = (float)(row[i] / norm);
        }
    }

    /// <summary>Refuses, before anything is written, keys that would take the records past the largest array (#1338).</summary>
    /// <remarks>Counts the keys not yet held, each once; freed slots are not counted, so it may refuse early, never late.</remarks>
    public void EnsureRoomFor(ICollection<TKey> keys)
    {
        // Every key new, and still room: the case of every write short of the limit, without a set.
        if ((long)_used + keys.Count <= MaxSlots)
        {
            return;
        }

        var incoming = new HashSet<TKey>(keys.Where(key => !_slots.ContainsKey(key)));

        if ((long)_used + incoming.Count > MaxSlots)
        {
            throw new InvalidOperationException(
                $"The collection holds {Count} records of {_dimension} floats, and {incoming.Count} more are past the largest array.");
        }
    }

    private long MaxSlots => TableLength.MaxLength / Math.Max(_dimension, 1);

    /// <summary>A fresh slot at the end, doubling the storage when it is full.</summary>
    private int Grow()
    {
        if (_used == _records.Length)
        {
            // Slots times the width stays within one array; in int the product wrapped negative (#1338).
            long maxSlots = MaxSlots;
            if (_used >= maxSlots)
            {
                throw new InvalidOperationException(
                    $"The collection holds {_used} records of {_dimension} floats, and one more is past the largest array.");
            }

            int capacity = (int)Math.Min(Math.Max(4L, (long)_records.Length * 2), maxSlots);
            Array.Resize(ref _records, capacity);
            Array.Resize(ref _rows, capacity * _dimension);
        }

        return _used++;
    }
}
