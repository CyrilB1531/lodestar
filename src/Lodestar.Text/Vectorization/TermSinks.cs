using System.Text;

namespace Lodestar.Text.Vectorization;

/// <summary>One document's counts per column, dense, and reset by clearing only the columns it touched.</summary>
/// <remarks>
/// Replaces a dictionary per document: the column ids are dense, so an array indexed by them
/// is the whole map, and the touched list keeps the reset proportional to the document.
/// </remarks>
internal sealed class ColumnTally
{
    private int[] _counts = new int[64];
    private int[] _touched = new int[64];
    private int _touchedCount;

    /// <summary>Counts one more occurrence of <paramref name="column"/>.</summary>
    public void Increment(int column)
    {
        if (column >= _counts.Length)
        {
            Array.Resize(ref _counts, Math.Max(column + 1, _counts.Length * 2));
        }

        if (_counts[column]++ == 0)
        {
            if (_touchedCount == _touched.Length)
            {
                Array.Resize(ref _touched, _touched.Length * 2);
            }
            _touched[_touchedCount++] = column;
        }
    }

    /// <summary>Moves the document's (column, count) pairs to <paramref name="into"/>, first-seen order, and resets.</summary>
    public void Drain(List<(int Column, int Count)> into)
    {
        for (int i = 0; i < _touchedCount; i++)
        {
            int column = _touched[i];
            into.Add((column, _counts[column]));
            _counts[column] = 0;
        }
        _touchedCount = 0;
    }

    /// <summary>Appends the document's row in column order, then resets.</summary>
    public void DrainSorted(List<int> columns, List<double> values, bool binary)
    {
        Array.Sort(_touched, 0, _touchedCount);
        for (int i = 0; i < _touchedCount; i++)
        {
            int column = _touched[i];
            columns.Add(column);
            values.Add(binary ? 1.0 : _counts[column]);
            _counts[column] = 0;
        }
        _touchedCount = 0;
    }
}

/// <summary>Assigns each new term the next provisional column, and counts it.</summary>
internal readonly struct ProvisionalCounts : ITermSink
{
#if NET9_0_OR_GREATER
    private readonly Dictionary<string, int>.AlternateLookup<ReadOnlySpan<char>> _lookup;
#endif

    public ProvisionalCounts(Dictionary<string, int> terms)
    {
        Terms = terms;
        Tally = new ColumnTally();
#if NET9_0_OR_GREATER
        _lookup = terms.GetAlternateLookup<ReadOnlySpan<char>>();
#endif
    }

    /// <summary>Every term seen so far, with its provisional column; insertion order is first-seen order.</summary>
    public Dictionary<string, int> Terms { get; }

    /// <summary>The current document's counts.</summary>
    public ColumnTally Tally { get; }

    public void Add(ReadOnlySpan<char> term)
    {
#if NET9_0_OR_GREATER
        // A term already in the vocabulary is found from the span; only a new one becomes a string.
        if (!_lookup.TryGetValue(term, out int column))
        {
            column = Terms.Count;
            _lookup.TryAdd(term, column);
        }
        Tally.Increment(column);
#else
        Add(term.ToString());
#endif
    }

    public void Add(string term)
    {
        if (!Terms.TryGetValue(term, out int column))
        {
            column = Terms.Count;
            Terms[term] = column;
        }
        Tally.Increment(column);
    }
}

/// <summary>Counts the terms a fitted vocabulary holds, and ignores the rest.</summary>
internal readonly struct VocabularyCounts : ITermSink
{
    private readonly Dictionary<string, int> _vocabulary;
#if NET9_0_OR_GREATER
    private readonly Dictionary<string, int>.AlternateLookup<ReadOnlySpan<char>> _lookup;
#endif

    public VocabularyCounts(Dictionary<string, int> vocabulary)
    {
        _vocabulary = vocabulary;
        Tally = new ColumnTally();
#if NET9_0_OR_GREATER
        _lookup = vocabulary.GetAlternateLookup<ReadOnlySpan<char>>();
#endif
    }

    /// <summary>The current document's counts.</summary>
    public ColumnTally Tally { get; }

    public void Add(ReadOnlySpan<char> term)
    {
#if NET9_0_OR_GREATER
        if (_lookup.TryGetValue(term, out int column))
        {
            Tally.Increment(column);
        }
#else
        Add(term.ToString());
#endif
    }

    public void Add(string term)
    {
        if (_vocabulary.TryGetValue(term, out int column))
        {
            Tally.Increment(column);
        }
    }
}

/// <summary>Hashes each term to a signed bucket, and sums a document's buckets.</summary>
/// <remarks>
/// Every contribution is <c>+1</c> or <c>-1</c>, so a bucket's sum is an exact integer whatever
/// order it is added in, which is what lets the contributions be sorted rather than kept in a map.
/// </remarks>
internal sealed class HashTally
{
    private readonly int _features;
    private readonly bool _alternateSign;
    private byte[] _bytes = new byte[256];
#if !NET9_0_OR_GREATER
    private char[] _chars = new char[64];
#endif
    private int[] _buckets = new int[64];
    private double[] _signs = new double[64];
    private int _count;

    public HashTally(int features, bool alternateSign)
    {
        _features = features;
        _alternateSign = alternateSign;
    }

    public void Add(ReadOnlySpan<char> term)
    {
        EnsureBytes(term.Length);
#if NET9_0_OR_GREATER
        int length = Encoding.UTF8.GetBytes(term, _bytes);
#else
        if (_chars.Length < term.Length)
        {
            _chars = new char[Math.Max(term.Length, _chars.Length * 2)];
        }
        term.CopyTo(_chars);
        int length = Encoding.UTF8.GetBytes(_chars, 0, term.Length, _bytes, 0);
#endif
        Push(MurmurHash3.Hash32(_bytes.AsSpan(0, length)));
    }

    public void Add(string term)
    {
        EnsureBytes(term.Length);
        int length = Encoding.UTF8.GetBytes(term, 0, term.Length, _bytes, 0);
        Push(MurmurHash3.Hash32(_bytes.AsSpan(0, length)));
    }

    /// <summary>Appends the document's non-zero buckets in column order, then resets.</summary>
    public void DrainNonZero(List<int> columns, List<double> values)
    {
        Array.Sort(_buckets, _signs, 0, _count);
        int i = 0;
        while (i < _count)
        {
            int column = _buckets[i];
            double value = 0.0;
            while (i < _count && _buckets[i] == column)
            {
                value += _signs[i];
                i++;
            }

            // SonarLint S1244: this decides what the sparse matrix stores, and
            // "stored" means "not exactly zero". Every accumulated value is a sum
            // of ±1, so exact cancellation is the ordinary outcome when
            // AlternateSign sends two terms to the same column — and it is
            // representable. A tolerance would drop real entries.
#pragma warning disable S1244
            if (value != 0.0)
#pragma warning restore S1244
            {
                columns.Add(column);
                values.Add(value);
            }
        }
        _count = 0;
    }

    private void EnsureBytes(int chars)
    {
        int needed = Encoding.UTF8.GetMaxByteCount(chars);
        if (_bytes.Length < needed)
        {
            _bytes = new byte[Math.Max(needed, _bytes.Length * 2)];
        }
    }

    private void Push(int h)
    {
        if (_count == _buckets.Length)
        {
            Array.Resize(ref _buckets, _count * 2);
            Array.Resize(ref _signs, _count * 2);
        }
        _buckets[_count] = (int)(Math.Abs((long)h) % _features);
        _signs[_count] = _alternateSign && h < 0 ? -1.0 : 1.0;
        _count++;
    }
}

/// <summary>Passes each term to a <see cref="HashTally"/>; the struct the analyzer specialises on.</summary>
internal readonly struct HashedTerms : ITermSink
{
    public HashedTerms(HashTally tally) => Tally = tally;

    /// <summary>The current document's buckets.</summary>
    public HashTally Tally { get; }

    public void Add(ReadOnlySpan<char> term) => Tally.Add(term);

    public void Add(string term) => Tally.Add(term);
}
