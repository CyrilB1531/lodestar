using Lodestar.Abstractions;

namespace Lodestar.Text.Search;

/// <summary>BM25 (Okapi) scored over a document-term matrix of raw counts.</summary>
/// <remarks>
/// Reference behavior: <c>rank_bm25</c> 0.2.2's <c>BM25Okapi</c>, written from Robertson and
/// Zaragoza's published description; where the two part — the floor on a negative IDF —
/// <see cref="Bm25Idf"/> says which is which and the corpus pins it. Build it over the matrix
/// <see cref="Vectorization.CountVectorizer"/> produces, never a
/// <see cref="Vectorization.TfidfVectorizer"/> one: BM25's saturation and length normalization
/// replace TF-IDF's, so an already-weighted matrix is weighted twice. Immutable, and thread-safe.
/// </remarks>
public sealed class Bm25Index
{
    private readonly CsrMatrix _counts;
    private readonly double[] _idf;
    private readonly double[] _documentLength;
    private readonly Bm25Options _options;
    private readonly int[] _postingStart;
    private readonly int[] _postingDocument;
    private readonly double[] _postingFrequency;

    /// <summary>The average document length, which BM25 normalizes against.</summary>
    public double AverageDocumentLength { get; }

    /// <summary>How many documents were indexed.</summary>
    public int DocumentCount => _counts.RowCount;

    /// <summary>How many terms the vocabulary holds.</summary>
    public int TermCount => _counts.ColumnCount;

    /// <summary>Builds an index over a matrix of raw term counts.</summary>
    /// <param name="counts">
    /// One row per document, one column per vocabulary term, values the raw counts —
    /// what <see cref="Vectorization.CountVectorizer.FitTransform"/> returns.
    /// </param>
    /// <param name="options">The saturation, length normalization and IDF; the defaults are <c>rank_bm25</c>'s.</param>
    /// <exception cref="ArgumentNullException"><paramref name="counts"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside the range it is defined on.</exception>
    /// <remarks>
    /// Building is three passes over the matrix — lengths, document frequency, then the
    /// per-term postings — and holds one extra copy of the non-zeros, which is what makes a
    /// query cost the documents that match rather than the whole corpus.
    /// </remarks>
    public Bm25Index(CsrMatrix counts, Bm25Options? options = null)
    {
        Guard.NotNull(counts);
        _counts = counts;
        _options = Validate(options ?? new Bm25Options());

        _documentLength = new double[counts.RowCount];
        double total = 0.0;
        for (int row = 0; row < counts.RowCount; row++)
        {
            // The L1 norm of a raw-count row is the document's length in tokens, which
            // is BM25's |D| -- already public on CsrMatrix, so nothing is recomputed here.
            _documentLength[row] = counts.RowL1Norm(row);
            total += _documentLength[row];
        }

        AverageDocumentLength = counts.RowCount == 0 ? 0.0 : total / counts.RowCount;
        int[] documentFrequency = BuildDocumentFrequency(counts);
        _idf = BuildIdf(documentFrequency, counts.RowCount, _options);
        (_postingStart, _postingDocument, _postingFrequency) = BuildPostings(counts, documentFrequency);
    }

    private static Bm25Options Validate(Bm25Options options)
    {
        RequireFinite(options.K1, nameof(options.K1));
        RequireFinite(options.B, nameof(options.B));
        if (options.K1 < 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.K1, "K1 saturates a term frequency and cannot be negative.");
        }

        if (options.B < 0.0 || options.B > 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.B, "B interpolates length normalization and lies in [0, 1].");
        }

        return options;
    }

    private static void RequireFinite(double value, string name)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentOutOfRangeException(nameof(name), value, $"{name} must be a finite number.");
        }
    }

    /// <summary>How many documents hold each term, in column order.</summary>
    private static int[] BuildDocumentFrequency(CsrMatrix counts)
    {
        int[] documentFrequency = new int[counts.ColumnCount];
        for (int row = 0; row < counts.RowCount; row++)
        {
            for (int i = counts.RowPointers[row]; i < counts.RowPointers[row + 1]; i++)
            {
                // A stored zero is not an occurrence: CSR may carry one, and counting it
                // would inflate the document frequency of a term nothing actually holds.
                // S1244: a raw count is an integer held in a double, so exact is the
                // right test -- a tolerance would discard a genuine count of one.
#pragma warning disable S1244
                if (counts.Values[i] != 0.0)
#pragma warning restore S1244
                {
                    documentFrequency[counts.ColumnIndices[i]]++;
                }
            }
        }

        return documentFrequency;
    }

    /// <summary>The postings list per term: which documents hold it, and how often.</summary>
    /// <remarks>
    /// CSR is row-major, so reaching the documents holding one term means reading every row.
    /// Counting sort by column turns that into one pass here and a slice per query term after,
    /// which is what keeps scoring proportional to the matches rather than to the corpus.
    /// </remarks>
    private static (int[] Start, int[] Document, double[] Frequency) BuildPostings(
        CsrMatrix counts, int[] documentFrequency)
    {
        int[] start = new int[counts.ColumnCount + 1];
        for (int term = 0; term < counts.ColumnCount; term++)
        {
            start[term + 1] = start[term] + documentFrequency[term];
        }

        int[] cursor = new int[counts.ColumnCount];
        Array.Copy(start, cursor, counts.ColumnCount);
        int[] document = new int[start[counts.ColumnCount]];
        double[] frequency = new double[document.Length];
        for (int row = 0; row < counts.RowCount; row++)
        {
            for (int i = counts.RowPointers[row]; i < counts.RowPointers[row + 1]; i++)
            {
                // S1244: as above -- a stored zero is not a posting.
#pragma warning disable S1244
                if (counts.Values[i] == 0.0)
#pragma warning restore S1244
                {
                    continue;
                }

                int slot = cursor[counts.ColumnIndices[i]]++;
                document[slot] = row;
                frequency[slot] = counts.Values[i];
            }
        }

        return (start, document, frequency);
    }

    /// <summary>The per-term IDF, in column order.</summary>
    private static double[] BuildIdf(int[] documentFrequency, int documents, Bm25Options options)
    {
        double[] idf = new double[documentFrequency.Length];
        for (int term = 0; term < idf.Length; term++)
        {
            double n = documentFrequency[term];
            double ratio = (documents - n + 0.5) / (n + 0.5);
            idf[term] = options.Idf == Bm25Idf.Lucene ? Math.Log(1.0 + ratio) : Math.Log(ratio);
        }

        return options.Idf == Bm25Idf.Lucene ? idf : FloorNegatives(idf, options.Epsilon);
    }

    /// <summary>Replaces every negative IDF with a small positive share of the mean.</summary>
    /// <remarks>
    /// The mean is taken over the <em>raw</em> values, negatives included, which is what
    /// the reference does — flooring first and averaging after would give a larger floor.
    /// </remarks>
    private static double[] FloorNegatives(double[] idf, double epsilon)
    {
        if (idf.Length == 0)
        {
            return idf;
        }

        double sum = 0.0;
        foreach (double value in idf)
        {
            sum += value;
        }

        double floor = epsilon * (sum / idf.Length);
        for (int term = 0; term < idf.Length; term++)
        {
            if (idf[term] < 0.0)
            {
                idf[term] = floor;
            }
        }

        return idf;
    }

    /// <summary>Scores every document against a query, in document order.</summary>
    /// <param name="queryTerms">
    /// Column indices, one per query <em>occurrence</em>: a term given twice counts twice,
    /// as the reference counts it. An index outside the vocabulary is ignored, which is how
    /// a term the corpus never saw behaves.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="queryTerms"/> is null.</exception>
    /// <remarks>
    /// One entry per document, so a caller wanting the whole distribution gets it; use
    /// <see cref="Top"/> when only the best few matter.
    /// </remarks>
    public double[] Score(IEnumerable<int> queryTerms)
    {
        Guard.NotNull(queryTerms);

        double[] scores = new double[_counts.RowCount];
        foreach (int term in queryTerms)
        {
            if (term < 0 || term >= _counts.ColumnCount)
            {
                continue;
            }

            AccumulateTerm(term, scores);
        }

        return scores;
    }

    /// <summary>Adds one query term's contribution to the documents that hold it.</summary>
    private void AccumulateTerm(int term, double[] scores)
    {
        double idf = _idf[term];
        int end = _postingStart[term + 1];
        for (int i = _postingStart[term]; i < end; i++)
        {
            int row = _postingDocument[i];
            double frequency = _postingFrequency[i];
            double normalized = _options.K1 * (1.0 - _options.B
                + (_options.B * _documentLength[row] / AverageDocumentLength));
            scores[row] += idf * frequency * (_options.K1 + 1.0) / (frequency + normalized);
        }
    }

    /// <summary>The best <paramref name="count"/> documents for a query, best first.</summary>
    /// <param name="queryTerms">Column indices, as <see cref="Score"/> takes them.</param>
    /// <param name="count">How many to return; fewer come back when the corpus is smaller.</param>
    /// <exception cref="ArgumentNullException"><paramref name="queryTerms"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
    /// <remarks>
    /// <strong>Ties break by document index, ascending</strong>, so a query that separates
    /// nothing returns the corpus in its own order rather than in an order the sort happened
    /// to produce. Documents scoring zero are returned like any other; a caller wanting only
    /// matches filters on the score.
    /// </remarks>
    public IReadOnlyList<SearchHit> Top(IEnumerable<int> queryTerms, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "A count of documents is not negative.");
        }

        double[] scores = Score(queryTerms);
        int[] order = new int[scores.Length];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }

        Array.Sort(order, (left, right) =>
        {
            int byScore = scores[right].CompareTo(scores[left]);
            return byScore != 0 ? byScore : left.CompareTo(right);
        });

        int taken = Math.Min(count, order.Length);
        SearchHit[] hits = new SearchHit[taken];
        for (int i = 0; i < taken; i++)
        {
            hits[i] = new SearchHit(order[i], scores[order[i]]);
        }

        return hits;
    }
}
