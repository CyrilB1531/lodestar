using Lodestar.Abstractions;
using Lodestar.Text.Search;

namespace Lodestar.Extensions.VectorData;

/// <summary>The rows of a term-count matrix that hold each term, which is the matrix transposed (#993).</summary>
/// <remarks>
/// One pass over the stored counts at build time, against one per query before: a hybrid search needs
/// what a score cannot say, whether a record matched a query term at all, and only the queried terms'
/// rows answer that. <c>Bm25Index</c> keeps the same shape for scoring and does not expose it.
/// </remarks>
internal sealed class TermPostings
{
    private readonly int[] _start;
    private readonly int[] _document;

    private TermPostings(int[] start, int[] document)
    {
        _start = start;
        _document = document;
    }

    /// <summary>Transposes <paramref name="counts"/> into the rows each column holds, each group ascending.</summary>
    public static TermPostings Of(CsrMatrix counts)
    {
        var start = new int[counts.ColumnCount + 1];
        foreach (int column in counts.ColumnIndices)
        {
            start[column + 1]++;
        }

        for (int term = 0; term < counts.ColumnCount; term++)
        {
            start[term + 1] += start[term];
        }

        var document = new int[counts.ColumnIndices.Length];
        var next = (int[])start.Clone();
        for (int row = 0; row < counts.RowCount; row++)
        {
            for (int k = counts.RowPointers[row]; k < counts.RowPointers[row + 1]; k++)
            {
                document[next[counts.ColumnIndices[k]]++] = row;
            }
        }

        return new TermPostings(start, document);
    }

    /// <summary>The records holding at least one of <paramref name="terms"/>, ascending, each once.</summary>
    /// <remarks>
    /// Read from the queried terms' postings alone, copied into one array sized from them, then sorted
    /// and compacted in place. A set node per matched row cost 880 KB and twice the time of the old
    /// whole-corpus pass when every record held the term (#1036).
    /// </remarks>
    public int[] Matching(IReadOnlyList<int> terms)
    {
        int total = 0;
        for (int i = 0; i < terms.Count; i++)
        {
            total += _start[terms[i] + 1] - _start[terms[i]];
        }

        var matched = new int[total];
        int filled = 0;
        for (int i = 0; i < terms.Count; i++)
        {
            int from = _start[terms[i]];
            int length = _start[terms[i] + 1] - from;
            Array.Copy(_document, from, matched, filled, length);
            filled += length;
        }

        // One term's run is ascending already; it can still repeat a record whose row stores the
        // term twice, which the compaction below folds as it does two terms' overlap.
        if (terms.Count > 1)
        {
            Array.Sort(matched);
        }

        int distinct = 0;
        for (int k = 0; k < matched.Length; k++)
        {
            if (distinct == 0 || matched[k] != matched[distinct - 1])
            {
                matched[distinct++] = matched[k];
            }
        }

        if (distinct != matched.Length)
        {
            Array.Resize(ref matched, distinct);
        }

        return matched;
    }

    /// <summary>The records <see cref="Matching"/> finds, by <paramref name="scorer"/>'s score descending, then by index.</summary>
    /// <remarks>
    /// <c>Top</c>'s order over the matched subset. The sort keys are the matched records' own scores,
    /// negated so ascending is descending, rather than a comparison reaching into the whole score array
    /// (#1036); <see cref="Array.Sort{TKey, TValue}(TKey[], TValue[])"/> is not stable, so each run of
    /// equal scores is put back in index order afterwards.
    /// </remarks>
    public int[] Ranked(IReadOnlyList<int> terms, Bm25Index scorer)
    {
        int[] matched = Matching(terms);
        if (matched.Length == 0)
        {
            return matched;
        }

        double[] scores = scorer.Score(terms);
        var descending = new double[matched.Length];
        for (int i = 0; i < matched.Length; i++)
        {
            descending[i] = -scores[matched[i]];
        }

        Array.Sort(descending, matched);
        int run = 0;
        while (run < matched.Length)
        {
            int end = run + 1;
            while (end < matched.Length && descending[end].CompareTo(descending[run]) == 0)
            {
                end++;
            }

            if (end - run > 1)
            {
                Array.Sort(matched, run, end - run);
            }

            run = end;
        }

        return matched;
    }
}
