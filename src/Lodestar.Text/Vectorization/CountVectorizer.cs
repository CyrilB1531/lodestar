using Lodestar.Abstractions;
namespace Lodestar.Text.Vectorization;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
#pragma warning disable S3776

/// <summary>Configuration for <see cref="CountVectorizer"/> (and, via composition, TF-IDF).</summary>
/// <remarks>Defaults mirror <c>sklearn.feature_extraction.text.CountVectorizer</c>.</remarks>
public sealed record CountVectorizerOptions
{
    /// <summary>Lowercase documents before tokenizing. Default <c>true</c>.</summary>
    public bool Lowercase { get; init; } = true;

    /// <summary>Strip accents via NFKD decomposition (like sklearn <c>strip_accents="unicode"</c>). Default <c>false</c>.</summary>
    public bool StripAccents { get; init; }

    /// <summary>Which analyzer to use. Default <see cref="AnalyzerKind.Word"/>.</summary>
    public AnalyzerKind Analyzer { get; init; } = AnalyzerKind.Word;

    /// <summary>Inclusive (min, max) n-gram sizes. Default <c>(1, 1)</c>.</summary>
    public (int Min, int Max) NgramRange { get; init; } = (1, 1);

    /// <summary>
    /// Minimum document frequency. A value in <c>(0, 1)</c> is a proportion of documents;
    /// a value <c>&gt;= 1</c> is an absolute count. Default <c>1</c>.
    /// </summary>
    public double MinDf { get; init; } = 1.0;

    /// <summary>
    /// Maximum document frequency. A value in <c>[0, 1]</c> is a proportion; a value
    /// <c>&gt; 1</c> is an absolute count. Default <c>1.0</c> (all documents).
    /// </summary>
    public double MaxDf { get; init; } = 1.0;

    /// <summary>If true, all non-zero counts are set to 1. Default <c>false</c>.</summary>
    public bool Binary { get; init; }

    /// <summary>Stop words to remove after tokenizing (word analyzer only). Default none.</summary>
    public IReadOnlyCollection<string>? StopWords { get; init; }

    /// <summary>Regex selecting word tokens. Default matches runs of two or more word characters.</summary>
    public string TokenPattern { get; init; } = @"\b\w\w+\b";

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated equality
    /// would otherwise treat two lists of the same words as unequal. <see cref="TfidfVectorizerOptions"/>
    /// embeds this record, so it inherits the fix.
    /// </remarks>
    public bool Equals(CountVectorizerOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Lowercase != other.Lowercase
            || StripAccents != other.StripAccents
            || Analyzer != other.Analyzer
            || NgramRange != other.NgramRange
            // SonarLint S1244 warns against comparing floating point for exact
            // equality, which is right for arithmetic and wrong here: this is value
            // equality between two configurations, where "the same threshold" means
            // the same bits. double.Equals also treats NaN as equal to NaN, which is
            // what a record's equality needs and what == would get wrong.
#pragma warning disable S1244
            || !MinDf.Equals(other.MinDf)
            || !MaxDf.Equals(other.MaxDf)
#pragma warning restore S1244
            || Binary != other.Binary
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present. Its
    /// <em>count</em> cannot be used: <see cref="Equals(CountVectorizerOptions)"/>
    /// compares as a set, so <c>["the", "the"]</c> equals <c>["the"]</c> while the
    /// counts differ — and equal objects are required to hash alike. Hashing the
    /// words themselves would mean hashing all of them, order-independently, which
    /// is the O(n) this exists to avoid. Unequal options are allowed to collide.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + (Lowercase ? 1 : 0);
            hash = (hash * 31) + (StripAccents ? 1 : 0);
            hash = (hash * 31) + (int)Analyzer;
            hash = (hash * 31) + NgramRange.GetHashCode();
            hash = (hash * 31) + MinDf.GetHashCode();
            hash = (hash * 31) + MaxDf.GetHashCode();
            hash = (hash * 31) + (Binary ? 1 : 0);
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
}

/// <summary>
/// Converts a collection of documents into a sparse matrix of token counts,
/// reproducing <c>sklearn.feature_extraction.text.CountVectorizer</c>.
/// </summary>
/// <remarks>Not thread-safe during <see cref="Fit"/>; read-only afterwards.</remarks>
public sealed partial class CountVectorizer
{
    private readonly CountVectorizerOptions _options;
    private readonly TextAnalyzer _analyzer;
    private Dictionary<string, int>? _vocabulary;
    private string[] _featureNames = [];

    /// <summary>Creates a vectorizer with the given options (defaults if omitted).</summary>
    /// <exception cref="ArgumentOutOfRangeException"><c>MinDf</c> or <c>MaxDf</c> is negative, not finite, or a fraction above 1.</exception>
    /// <exception cref="ArgumentException"><c>NgramRange</c> is not an ascending range starting at 1 or more.</exception>
    public CountVectorizer(CountVectorizerOptions? options = null)
    {
        _options = options ?? new CountVectorizerOptions();
        RequireDocumentFrequency(_options.MinDf, nameof(CountVectorizerOptions.MinDf), nameof(options));
        RequireDocumentFrequency(_options.MaxDf, nameof(CountVectorizerOptions.MaxDf), nameof(options));
        TextAnalyzer.RequireNgramRange(_options.NgramRange, nameof(options));
        _analyzer = new TextAnalyzer(
            _options.Lowercase,
            _options.StripAccents,
            _options.Analyzer,
            _options.NgramRange,
            _options.TokenPattern,
            _options.StopWords);
    }

    /// <exception cref="InvalidOperationException">nothing has been fitted yet.</exception>
    /// <summary>The learned vocabulary, sorted; valid after <see cref="Fit"/>/<see cref="FitTransform"/>.</summary>
    public IReadOnlyList<string> GetFeatureNames()
    {
        EnsureFitted();
        return _featureNames;
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException"><c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c> over this corpus.</exception>
    /// <summary>Learns the vocabulary from <paramref name="documents"/>.</summary>
    public CountVectorizer Fit(IEnumerable<string> documents)
    {
        FitTransform(documents);
        return this;
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException"><c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c> over this corpus.</exception>
    /// <summary>Learns the vocabulary and returns the count matrix in one pass.</summary>
    public CsrMatrix FitTransform(IEnumerable<string> documents)
    {
        Guard.NotNull(documents);
        var docs = documents as IReadOnlyList<string> ?? documents.ToList();
        int nDocs = docs.Count;

        // First pass: provisional vocabulary (first-seen order) and per-document counts,
        // stored flat as (provisional column, count) pairs in first-seen order per document.
        var provisional = new ProvisionalCounts(new Dictionary<string, int>(StringComparer.Ordinal));
        var perDoc = new List<(int Column, int Count)>();
        var docStart = new int[nDocs + 1];
        for (int row = 0; row < nDocs; row++)
        {
            _analyzer.Analyze(TextAnalyzer.Document(docs, row, nameof(documents)), ref provisional);
            provisional.Tally.Drain(perDoc);
            docStart[row + 1] = perDoc.Count;
        }

        // Document frequencies over the provisional columns.
        var df = new int[provisional.Terms.Count];
        foreach ((int col, _) in perDoc)
        {
            df[col]++;
        }

        // Document-frequency limits (sklearn _limit_features semantics).
        double low = _options.MinDf is > 0 and < 1 ? _options.MinDf * nDocs : _options.MinDf;
        double high = _options.MaxDf <= 1.0 ? _options.MaxDf * nDocs : _options.MaxDf;
        // An empty corpus has no terms to keep either way, and refusing it would break a documented no-throw.
        if (nDocs > 0 && high < low)
        {
            throw new InvalidOperationException(
                $"MaxDf ({_options.MaxDf}) corresponds to fewer documents than MinDf ({_options.MinDf}) over {nDocs} documents.");
        }

        // Kept terms, sorted -> final column index.
        var kept = new List<string>();
        foreach (KeyValuePair<string, int> entry in provisional.Terms)
        {
            string term = entry.Key;
            int col = entry.Value;
            if (df[col] >= low && df[col] <= high)
            {
                kept.Add(term);
            }
        }
        kept.Sort(StringComparer.Ordinal);

        _featureNames = kept.ToArray();
        _vocabulary = new Dictionary<string, int>(kept.Count, StringComparer.Ordinal);
        for (int i = 0; i < kept.Count; i++)
        {
            _vocabulary[kept[i]] = i;
        }

        // Map provisional columns to final columns (or -1 if dropped).
        var remap = new int[provisional.Terms.Count];
        foreach (KeyValuePair<string, int> entry in provisional.Terms)
        {
            string term = entry.Key;
            int col = entry.Value;
            remap[col] = _vocabulary.TryGetValue(term, out int finalCol) ? finalCol : -1;
        }

        return BuildMatrix(perDoc, docStart, remap, _featureNames.Length);
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException">nothing has been fitted yet.</exception>
    /// <summary>Transforms <paramref name="documents"/> using the already-learned vocabulary.</summary>
    public CsrMatrix Transform(IEnumerable<string> documents)
    {
        Guard.NotNull(documents);
        EnsureFitted();
        var docs = documents as IReadOnlyList<string> ?? documents.ToList();

        var rowPointers = new int[docs.Count + 1];
        var values = new List<double>();
        var columns = new List<int>();

        var counts = new VocabularyCounts(_vocabulary!);
        for (int row = 0; row < docs.Count; row++)
        {
            _analyzer.Analyze(TextAnalyzer.Document(docs, row, nameof(documents)), ref counts);
            counts.Tally.DrainSorted(columns, values, _options.Binary);
            rowPointers[row + 1] = values.Count;
        }

        return CsrMatrix.CreateUnchecked(docs.Count, _featureNames.Length, values.ToArray(), columns.ToArray(), rowPointers);
    }

    private CsrMatrix BuildMatrix(List<(int Column, int Count)> perDoc, int[] docStart, int[] remap, int columnCount)
    {
        int nDocs = docStart.Length - 1;
        var rowPointers = new int[nDocs + 1];
        var values = new List<double>();
        var columns = new List<int>();
        int[] finalColumns = [];
        int[] finalCounts = [];

        for (int row = 0; row < nDocs; row++)
        {
            int size = docStart[row + 1] - docStart[row];
            if (finalColumns.Length < size)
            {
                finalColumns = new int[Math.Max(size, finalColumns.Length * 2)];
                finalCounts = new int[finalColumns.Length];
            }

            int mapped = 0;
            for (int i = docStart[row]; i < docStart[row + 1]; i++)
            {
                int finalCol = remap[perDoc[i].Column];
                if (finalCol >= 0)
                {
                    finalColumns[mapped] = finalCol;
                    finalCounts[mapped] = perDoc[i].Count;
                    mapped++;
                }
            }

            // Final columns are distinct within a row, so the unstable sort has no tie to break.
            Array.Sort(finalColumns, finalCounts, 0, mapped);
            for (int i = 0; i < mapped; i++)
            {
                columns.Add(finalColumns[i]);
                values.Add(_options.Binary ? 1.0 : finalCounts[i]);
            }
            rowPointers[row + 1] = values.Count;
        }

        return CsrMatrix.CreateUnchecked(nDocs, columnCount, values.ToArray(), columns.ToArray(), rowPointers);
    }

    /// <summary>scikit-learn's <c>min_df</c>/<c>max_df</c> constraint: a proportion in <c>[0, 1]</c> or a whole count.</summary>
    private static void RequireDocumentFrequency(double value, string name, string paramName)
    {
        if (!(value >= 0) || double.IsPositiveInfinity(value) || (value > 1 && Math.Floor(value) < value))
        {
            throw new ArgumentOutOfRangeException(
                paramName, value, $"{name} must be a proportion in [0, 1] or a whole number of documents.");
        }
    }

    private void EnsureFitted()
    {
        if (_vocabulary is null)
        {
            throw new InvalidOperationException("The vectorizer has not been fitted. Call Fit or FitTransform first.");
        }
    }
}
