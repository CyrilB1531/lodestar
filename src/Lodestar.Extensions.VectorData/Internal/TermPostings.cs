using Lodestar.Abstractions;

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
    /// Read from the queried terms' postings alone. The hybrid search then ranks those records only,
    /// where scoring and sorting the whole corpus allocated megabytes per query (#993).
    /// </remarks>
    public int[] Matching(IReadOnlyList<int> terms)
    {
        var matched = new SortedSet<int>();
        for (int i = 0; i < terms.Count; i++)
        {
            int term = terms[i];
            for (int k = _start[term]; k < _start[term + 1]; k++)
            {
                matched.Add(_document[k]);
            }
        }

        return [.. matched];
    }
}
