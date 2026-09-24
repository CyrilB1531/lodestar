using Lodestar.Abstractions;
namespace Lodestar.Text.Vectorization;

/// <summary>
/// Converts documents directly into a TF-IDF matrix, reproducing
/// <c>sklearn.feature_extraction.text.TfidfVectorizer</c> (a <see cref="CountVectorizer"/>
/// followed by a <see cref="TfidfTransformer"/>).
/// </summary>
public sealed partial class TfidfVectorizer
{
    private readonly CountVectorizer _counts;
    private readonly TfidfTransformer _tfidf;

    /// <summary>Creates a vectorizer with the given options (defaults if omitted).</summary>
    /// <exception cref="ArgumentOutOfRangeException"><c>Count.MinDf</c> or <c>Count.MaxDf</c> is negative, not finite, or a fraction above 1.</exception>
    /// <exception cref="ArgumentException"><c>Count.NgramRange</c> is not an ascending range starting at 1 or more.</exception>
    public TfidfVectorizer(TfidfVectorizerOptions? options = null)
    {
        options ??= new TfidfVectorizerOptions();
        _counts = new CountVectorizer(options.Count);
        _tfidf = new TfidfTransformer(options.Tfidf);
    }

    /// <exception cref="InvalidOperationException">nothing has been fitted yet.</exception>
    /// <summary>The learned vocabulary, sorted; valid after fitting.</summary>
    public IReadOnlyList<string> GetFeatureNames() => _counts.GetFeatureNames();

    /// <summary>The learned inverse-document-frequency vector (one per feature).</summary>
    public IReadOnlyList<double> Idf => _tfidf.Idf;

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException"><c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c> over this corpus.</exception>
    /// <summary>Learns the vocabulary and idf weights.</summary>
    public TfidfVectorizer Fit(IEnumerable<string> documents)
    {
        FitTransform(documents);
        return this;
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException"><c>MaxDf</c> corresponds to fewer documents than <c>MinDf</c> over this corpus.</exception>
    /// <summary>Learns and returns the TF-IDF matrix in one pass.</summary>
    public CsrMatrix FitTransform(IEnumerable<string> documents)
    {
        CsrMatrix counts = _counts.FitTransform(documents);
        return _tfidf.FitTransform(counts);
    }

    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="documents"/> holds a null document.</exception>
    /// <exception cref="InvalidOperationException">nothing has been fitted yet.</exception>
    /// <summary>Transforms documents using the already-learned vocabulary and idf weights.</summary>
    public CsrMatrix Transform(IEnumerable<string> documents)
    {
        CsrMatrix counts = _counts.Transform(documents);
        return _tfidf.Transform(counts);
    }
}
