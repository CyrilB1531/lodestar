using Lodestar.Abstractions;
using Lodestar.Internal.Persistence;

namespace Lodestar.Text.Vectorization;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
#pragma warning disable S3776

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

    // Whether a term may hold a lone surrogate, and the longest term's length: found once by the fit or the load, so a save
    // searches and measures no term (#1643).
    private bool _vocabularyMayHoldSurrogate;
    private int _longestTerm;

    /// <summary>Creates a vectorizer with the given options (defaults if omitted).</summary>
    /// <exception cref="ArgumentOutOfRangeException"><c>MinDf</c> or <c>MaxDf</c> is negative, not finite, or a fraction above 1.</exception>
    /// <exception cref="ArgumentException"><c>NgramRange</c> descends, <c>Analyzer</c> is not an <see cref="AnalyzerKind"/>, or <c>TokenPattern</c> is null, or has more than one capturing group under the word analyzer.</exception>
    public CountVectorizer(CountVectorizerOptions? options = null)
    {
        _options = options ?? new CountVectorizerOptions();
        RequireDocumentFrequency(_options.MinDf, nameof(CountVectorizerOptions.MinDf), nameof(options));
        RequireDocumentFrequency(_options.MaxDf, nameof(CountVectorizerOptions.MaxDf), nameof(options));
        TextAnalyzer.RequireNgramRange(_options.NgramRange, nameof(options));
        TextAnalyzer.RequireAnalyzer(_options.Analyzer, nameof(options));
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

        // The array itself, as 0.7.0 handed it out, which a cast can edit: from here a save searches and measures every
        // term again, as it did before the fit's findings were kept (#1643).
        _vocabularyMayHoldSurrogate = true;
        _longestTerm = int.MaxValue;
        return _featureNames;
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException">The corpus yields no term, <c>MinDf</c> and <c>MaxDf</c> leave none, or <c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c>, as scikit-learn refuses each.</exception>
    /// <summary>Learns the vocabulary from <paramref name="documents"/>.</summary>
    public CountVectorizer Fit(IEnumerable<string> documents)
    {
        FitTransform(documents);
        return this;
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException">The corpus yields no term, <c>MinDf</c> and <c>MaxDf</c> leave none, or <c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c>, as scikit-learn refuses each.</exception>
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
        try
        {
            for (int row = 0; row < nDocs; row++)
            {
                _analyzer.Analyze(TextAnalyzer.Document(docs, row, nameof(documents)), ref provisional);
                provisional.Tally.Drain(perDoc);
                docStart[row + 1] = perDoc.Count;
            }
        }
        finally
        {
            TextAnalyzer.ReleaseScratch();
        }

        // scikit-learn's _count_vocab refuses a corpus that yields no term, before any bound is read.
        if (provisional.Terms.Count == 0)
        {
            throw new InvalidOperationException("empty vocabulary; perhaps the documents only contain stop words.");
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
        if (high < low)
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
        if (kept.Count == 0)
        {
            throw new InvalidOperationException("After pruning, no terms remain. Try a lower MinDf or a higher MaxDf.");
        }
        // scikit-learn sorts the vocabulary as Python sorts a str, by code point (#1264).
        kept.Sort(CodePointOrder.Instance);

        _featureNames = kept.ToArray();
        _vocabularyMayHoldSurrogate = false;
        _longestTerm = 0;
        _vocabulary = new Dictionary<string, int>(kept.Count, StringComparer.Ordinal);
        for (int i = 0; i < kept.Count; i++)
        {
            _vocabularyMayHoldSurrogate |= JsonArtifact.MayHoldSurrogate(kept[i]);
            _longestTerm = Math.Max(_longestTerm, kept[i].Length);
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
        try
        {
            for (int row = 0; row < docs.Count; row++)
            {
                _analyzer.Analyze(TextAnalyzer.Document(docs, row, nameof(documents)), ref counts);
                counts.Tally.DrainSorted(columns, values, _options.Binary);
                rowPointers[row + 1] = values.Count;
            }
        }
        finally
        {
            TextAnalyzer.ReleaseScratch();
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
