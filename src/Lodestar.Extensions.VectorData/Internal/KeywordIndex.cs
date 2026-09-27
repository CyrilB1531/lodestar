using System.Buffers;
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// long-comment: what is kept per record, what stays whole-corpus, and why ranking takes a lock.
/// <summary>
/// The keyword half, kept per term so a write costs its own record: each text is tokenized once,
/// its postings added and removed in place, and the corpus statistics BM25 reads kept as counts.
/// </summary>
/// <remarks>
/// <para>
/// What a <c>CountVectorizer</c> fit and a <c>Bm25Index</c> over the held texts would rank, bit for
/// bit (#1214). Tokenizing is the vectorizer's own, with <c>MinDf</c> and <c>MaxDf</c> set aside, since
/// they only prune the vocabulary; the vocabulary is then every term whose document frequency they
/// admit at the current record count. The scoring expressions are <c>Bm25Index</c>'s, in its order.
/// <c>CollectionDifferentialTests</c> replays random writes against both.
/// </para>
/// <para>
/// A written text is only staged; the next ranking tokenizes every staged text in one fit, so a bulk
/// load pays one pass and a vector search none. What stays whole-corpus is what BM25 defines that way:
/// the floor on a negative IDF is a share of the mean IDF over the vocabulary, and pruned bounds make
/// the average length a sum over it, both <c>O(V)</c> in the terms and cached until the next write.
/// </para>
/// <para>
/// Ranking stages and caches, so it runs under a lock: two searches may rank at once, as the rebuild
/// this replaced allowed. A write must still not run beside any search.
/// </para>
/// </remarks>
internal sealed class KeywordIndex
{
    // A 0 x 0 matrix, which Bm25Index accepts, so the options are refused by the type that defines them.
    private static readonly CsrMatrix NoCounts = new(0, 0, [], [], [0]);

    private readonly object _gate = new();
    private readonly CountVectorizerOptions _vectorizer;
    private readonly Bm25Options _bm25;
    private readonly Dictionary<string, Term> _terms = new(StringComparer.Ordinal);
    private readonly SortedSet<Term> _ordered = new(Term.Ordinal);
    private readonly List<int> _staged = [];
    private Document?[] _documents = [];
    private string?[] _pending = [];
    private CountVectorizer? _tokenizer;
    private int _documentCount;
    private long _totalLength;
    private Statistics? _statistics;

    /// <summary>Creates an empty keyword half configured as a collection's options say.</summary>
    public KeywordIndex(LodestarVectorStoreOptions options)
    {
        _vectorizer = options.Vectorizer ?? new CountVectorizerOptions();
        _bm25 = options.Bm25 ?? new Bm25Options();
    }

    /// <summary>How many texts have been tokenized, which the suite asserts a write does not multiply.</summary>
    internal int TokenizedTexts { get; private set; }

    /// <summary>Stages <paramref name="text"/> as <paramref name="slot"/>'s, dropping whatever that slot held.</summary>
    public void Stage(int slot, string text)
    {
        Remove(slot);
        if (slot >= _pending.Length)
        {
            int capacity = Math.Max(Math.Max(4, _pending.Length * 2), slot + 1);
            Array.Resize(ref _pending, capacity);
            Array.Resize(ref _documents, capacity);
        }

        _pending[slot] = text;
        _staged.Add(slot);
    }

    /// <summary>Drops whatever <paramref name="slot"/> held, tokenized or staged.</summary>
    public void Remove(int slot)
    {
        if (slot >= _pending.Length)
        {
            return;
        }

        _pending[slot] = null;
        if (_documents[slot] is { } document)
        {
            Withdraw(slot, document);
        }
    }

    /// <summary>Drops every text.</summary>
    public void Clear()
    {
        _terms.Clear();
        _ordered.Clear();
        _staged.Clear();
        _documents = [];
        _pending = [];
        _documentCount = 0;
        _totalLength = 0;
        _statistics = null;
    }

    /// <summary>The slots whose text holds a query term, in no particular order, with their BM25 scores.</summary>
    /// <param name="keywords">The query, taken as one document.</param>
    /// <exception cref="ArgumentOutOfRangeException">The BM25 or vectorizer options are outside their range.</exception>
    /// <exception cref="InvalidOperationException"><c>MaxDf</c> corresponds to fewer records than <c>MinDf</c>.</exception>
    /// <remarks>
    /// Only matched slots are answered, since fusion reads rank and not score; matching is read from the
    /// postings, not the score, whose sign Robertson's IDF leaves open (#884).
    /// </remarks>
    public KeywordMatches Matched(ICollection<string> keywords)
    {
        lock (_gate)
        {
            TokenizeStaged();
            if (_documentCount == 0)
            {
                return new KeywordMatches([], []);
            }

            Statistics statistics = _statistics ??= Measure();
            List<Term> terms = QueryTerms(keywords, statistics);
            return terms.Count == 0 ? new KeywordMatches([], []) : Score(terms, statistics);
        }
    }

    /// <summary>The corpus figures a query reads, as a fit over the texts held now would give them.</summary>
    private Statistics Measure()
    {
        int documents = _documentCount;
        double low = _vectorizer.MinDf is > 0 and < 1 ? _vectorizer.MinDf * documents : _vectorizer.MinDf;
        double high = _vectorizer.MaxDf <= 1.0 ? _vectorizer.MaxDf * documents : _vectorizer.MaxDf;
        if (high < low)
        {
            throw new InvalidOperationException(
                $"MaxDf ({_vectorizer.MaxDf}) corresponds to fewer documents than MinDf ({_vectorizer.MinDf}) over {documents} documents.");
        }

        // Every held term has 1 <= df <= documents, so bounds that wide prune nothing.
        bool unpruned = low <= 1 && high >= documents;
        var statistics = new Statistics(documents, low, high, unpruned);
        long total = unpruned ? _totalLength : PrunedLength(statistics);
        statistics.AverageLength = (double)total / documents;
        return statistics;
    }

    /// <summary>The summed length of every text over the kept terms alone, which is what a pruned fit counts.</summary>
    private long PrunedLength(Statistics statistics) =>
        _terms.Values.Where(statistics.Keeps).Sum(term => term.Occurrences);

    /// <summary>The query's distinct terms the vocabulary keeps, in its column order: ordinal.</summary>
    private List<Term> QueryTerms(ICollection<string> keywords, Statistics statistics)
    {
        var terms = new List<Term>();
        (CsrMatrix _, IReadOnlyList<string> names) = Tokenize([string.Join(" ", keywords)]);
        foreach (string name in names)
        {
            if (_terms.TryGetValue(name, out Term? term) && statistics.Keeps(term))
            {
                terms.Add(term);
            }
        }

        return terms;
    }

    /// <summary>The matched slots, each once, scored as <c>Bm25Index.Score</c> scores them.</summary>
    private KeywordMatches Score(List<Term> terms, Statistics statistics)
    {
        double[] scores = ArrayPool<double>.Shared.Rent(_documents.Length);
        bool[] seen = ArrayPool<bool>.Shared.Rent(_documents.Length);
        double[] lengths = ArrayPool<double>.Shared.Rent(_documents.Length);
        var matched = new List<int>(terms.Sum(term => term.Count));
        try
        {
            // A rented array holds whatever its last renter left, anywhere in the process.
            Array.Clear(seen, 0, _documents.Length);
            foreach (Term term in terms)
            {
                for (int i = 0; i < term.Count; i++)
                {
                    int slot = term.PostingAt(i).Slot;
                    if (!seen[slot])
                    {
                        seen[slot] = true;
                        scores[slot] = 0.0;
                        // Once per matched text: a pruned length walks the whole text (#1214 review).
                        lengths[slot] = NormalizedLength(_documents[slot]!, statistics);
                        matched.Add(slot);
                    }
                }
            }

            // Term by term, in column order, as Bm25Index accumulates: the sums carry the same bits.
            double saturation = _bm25.K1 + 1.0;
            foreach (Term term in terms)
            {
                double idf = Idf(term, statistics);
                for (int i = 0; i < term.Count; i++)
                {
                    Posting posting = term.PostingAt(i);
                    double frequency = posting.Count;
                    double normalizedLength = lengths[posting.Slot];
                    scores[posting.Slot] += idf * frequency * saturation / (frequency + normalizedLength);
                }
            }

            var slots = new int[matched.Count];
            var scored = new double[matched.Count];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = matched[i];
                scored[i] = scores[matched[i]];
            }

            return new KeywordMatches(slots, scored);
        }
        finally
        {
            ArrayPool<bool>.Shared.Return(seen);
            ArrayPool<double>.Shared.Return(lengths);
            ArrayPool<double>.Shared.Return(scores);
        }
    }

    /// <summary><c>Bm25Index</c>'s IDF for <paramref name="term"/>, the floor included.</summary>
    private double Idf(Term term, Statistics statistics)
    {
        double idf = RawIdf(term.Count, statistics.Documents);
        if (_bm25.Idf == Bm25Idf.Lucene || idf >= 0.0)
        {
            return idf;
        }

        statistics.Floor ??= _bm25.Epsilon * MeanIdf(statistics);
        return statistics.Floor.Value;
    }

    private double RawIdf(int documentFrequency, int documents)
    {
        double n = documentFrequency;
        double ratio = (documents - n + 0.5) / (n + 0.5);
        return _bm25.Idf == Bm25Idf.Lucene ? Math.Log(1.0 + ratio) : Math.Log(ratio);
    }

    /// <summary>The mean raw IDF over the vocabulary, summed in its column order as <c>Bm25Index</c> sums it.</summary>
    private double MeanIdf(Statistics statistics)
    {
        double sum = 0.0;
        int kept = 0;
        foreach (Term term in _ordered.Where(statistics.Keeps))
        {
            sum += RawIdf(term.Count, statistics.Documents);
            kept++;
        }

        return sum / kept;
    }

    /// <summary><c>K1 * (1 - B + B * |D| / avgdl)</c>, written as <c>Bm25Index</c> writes it.</summary>
    private double NormalizedLength(Document document, Statistics statistics)
    {
        double length = statistics.Unpruned ? document.Length : document.KeptLength(statistics);
        double k1 = _bm25.K1;
        double b = _bm25.B;
        return k1 * (1.0 - b + (b * length / statistics.AverageLength));
    }

    /// <summary>Tokenizes every staged text in one fit and adds its postings.</summary>
    private void TokenizeStaged()
    {
        if (_staged.Count == 0)
        {
            return;
        }

        // Before anything is consumed, so a refused option leaves every staged text staged.
        if (_tokenizer is null)
        {
            // The caller's options are checked by the types that define them, where a fit checked them.
            _ = new CountVectorizer(_vectorizer);
            _ = new Bm25Index(NoCounts, _bm25);
            _tokenizer = new CountVectorizer(_vectorizer with { MinDf = 1.0, MaxDf = 1.0 });
        }

        var slots = new List<int>(_staged.Count);
        var texts = new List<string>(_staged.Count);
        foreach (int slot in _staged)
        {
            // A slot staged twice, or staged then removed, holds its text once or not at all.
            if (_pending[slot] is { } text)
            {
                slots.Add(slot);
                texts.Add(text);
                _pending[slot] = null;
            }
        }

        _staged.Clear();
        if (texts.Count == 0)
        {
            return;
        }

        (CsrMatrix counts, IReadOnlyList<string> names) = Tokenize(texts);
        var byColumn = new Term?[names.Count];
        for (int row = 0; row < slots.Count; row++)
        {
            Admit(slots[row], counts, row, names, byColumn);
        }

        TokenizedTexts += texts.Count;
    }

    /// <summary>Row <paramref name="row"/> of <paramref name="counts"/> as <paramref name="slot"/>'s document.</summary>
    private void Admit(int slot, CsrMatrix counts, int row, IReadOnlyList<string> names, Term?[] byColumn)
    {
        int from = counts.RowPointers[row];
        int width = counts.RowPointers[row + 1] - from;
        var document = new Document(width);
        for (int k = 0; k < width; k++)
        {
            int column = counts.ColumnIndices[from + k];
            Term term = byColumn[column] ??= TermOf(names[column]);
            int count = (int)counts.Values[from + k];
            document.Set(k, term, count, term.Add(new Posting(slot, count, k)));
            term.Occurrences += count;
            document.Length += count;
        }

        _documents[slot] = document;
        _documentCount++;
        _totalLength += document.Length;
        _statistics = null;
    }

    private Term TermOf(string text)
    {
        if (!_terms.TryGetValue(text, out Term? term))
        {
            term = new Term(text);
            _terms.Add(text, term);
            _ordered.Add(term);
        }

        return term;
    }

    /// <summary>Takes <paramref name="slot"/>'s postings out of every term it held.</summary>
    private void Withdraw(int slot, Document document)
    {
        for (int k = 0; k < document.Width; k++)
        {
            Term term = document.TermAt(k);
            // The posting moved into the gap belongs to another slot, whose back-reference follows it.
            if (term.RemoveAt(document.PostingIndexAt(k)) is { } moved)
            {
                _documents[moved.Slot]!.Repoint(moved.Back, document.PostingIndexAt(k));
            }

            term.Occurrences -= document.CountAt(k);
            if (term.Count == 0)
            {
                _terms.Remove(term.Text);
                _ordered.Remove(term);
            }
        }

        _documents[slot] = null;
        _documentCount--;
        _totalLength -= document.Length;
        _statistics = null;
    }

    /// <summary>The terms and counts of <paramref name="texts"/>, with no document-frequency pruning.</summary>
    private (CsrMatrix Counts, IReadOnlyList<string> Names) Tokenize(List<string> texts)
    {
        // Created by the first tokenizing of staged texts, which any held text precedes.
        CountVectorizer tokenizer = _tokenizer!;
        try
        {
            CsrMatrix counts = tokenizer.FitTransform(texts);
            return (counts, tokenizer.GetFeatureNames());
        }
        catch (InvalidOperationException)
        {
            // Unpruned, the only refusal left is texts that yield no term (#1239): none, then.
            return (new CsrMatrix(texts.Count, 0, [], [], new int[texts.Count + 1]), []);
        }
    }

    /// <summary>A slot's text: the terms it holds, how often, and where in each term its posting sits.</summary>
    private sealed class Document(int width)
    {
        private readonly Term[] _terms = new Term[width];
        private readonly int[] _counts = new int[width];
        private readonly int[] _postingIndex = new int[width];

        public int Width => _terms.Length;

        /// <summary>The text's length over every term, which is BM25's <c>|D|</c> when nothing is pruned.</summary>
        public long Length { get; set; }

        public Term TermAt(int k) => _terms[k];

        public int CountAt(int k) => _counts[k];

        public int PostingIndexAt(int k) => _postingIndex[k];

        public void Set(int k, Term term, int count, int postingIndex)
        {
            _terms[k] = term;
            _counts[k] = count;
            _postingIndex[k] = postingIndex;
        }

        public void Repoint(int k, int postingIndex) => _postingIndex[k] = postingIndex;

        /// <summary>The text's length over the terms the vocabulary keeps, which is what a pruned fit counts.</summary>
        public long KeptLength(Statistics statistics)
        {
            long length = 0;
            for (int k = 0; k < _terms.Length; k++)
            {
                if (statistics.Keeps(_terms[k]))
                {
                    length += _counts[k];
                }
            }

            return length;
        }
    }

    /// <summary>A term's postings, unordered, and how often it occurs over every text.</summary>
    private sealed class Term(string text)
    {
        public static readonly IComparer<Term> Ordinal =
            Comparer<Term>.Create((x, y) => string.CompareOrdinal(x.Text, y.Text));

        private Posting[] _postings = new Posting[1];

        public string Text { get; } = text;

        /// <summary>How many texts hold the term: its document frequency.</summary>
        public int Count { get; private set; }

        public long Occurrences { get; set; }

        public Posting PostingAt(int i) => _postings[i];

        /// <summary>Appends <paramref name="posting"/>, answering where it sits.</summary>
        public int Add(Posting posting)
        {
            if (Count == _postings.Length)
            {
                Array.Resize(ref _postings, Count * 2);
            }

            _postings[Count] = posting;
            return Count++;
        }

        /// <summary>Removes the posting at <paramref name="i"/> by moving the last into its place, answering the one moved.</summary>
        public Posting? RemoveAt(int i)
        {
            int last = --Count;
            if (i == last)
            {
                return null;
            }

            _postings[i] = _postings[last];
            return _postings[i];
        }
    }

    /// <summary>One text holding a term: its slot, the count, and the term's index in that text's own list.</summary>
    private readonly struct Posting(int slot, int count, int back)
    {
        public int Slot { get; } = slot;

        public int Count { get; } = count;

        public int Back { get; } = back;
    }

    /// <summary>The vocabulary's bounds and the figures read from it, fixed until the next write.</summary>
    private sealed class Statistics(int documents, double low, double high, bool unpruned)
    {
        public int Documents { get; } = documents;

        /// <summary>Whether the bounds keep every held term, so no figure needs the vocabulary walked.</summary>
        public bool Unpruned { get; } = unpruned;

        public double AverageLength { get; set; }

        /// <summary>The floor on a negative IDF, computed the first time a query needs it.</summary>
        public double? Floor { get; set; }

        public bool Keeps(Term term) => Unpruned || (term.Count >= low && term.Count <= high);
    }
}
