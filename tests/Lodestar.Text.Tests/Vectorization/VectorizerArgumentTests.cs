using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>The vectorizer refusals #901 grouped, and the transformer members no other test reaches directly.</summary>
public sealed class VectorizerArgumentTests
{
    private static readonly string[] Corpus = ["the cat eats", "the dog eats", "the cat and the dog"];

    [Fact]
    public void A_null_document_is_refused_naming_documents()
    {
        string[] withNull = ["the cat", null!];
        CountVectorizer fitted = new CountVectorizer().Fit(Corpus);

        var fit = Assert.Throws<ArgumentException>(() => new CountVectorizer().Fit(withNull));
        var transform = Assert.Throws<ArgumentException>(() => fitted.Transform(withNull));
        var hashing = Assert.Throws<ArgumentException>(() => new HashingVectorizer().Transform(withNull));
        var tfidf = Assert.Throws<ArgumentException>(() => new TfidfVectorizer().Fit(withNull));
        var raw = Assert.Throws<ArgumentException>(
            () => new CountVectorizer(new CountVectorizerOptions { Lowercase = false }).Fit(withNull));

        Assert.All([fit, transform, hashing, tfidf, raw], e => Assert.Equal("documents", e.ParamName));
    }

    [Fact]
    public void A_null_corpus_is_refused_naming_documents()
    {
        var error = Assert.Throws<ArgumentNullException>(() => new CountVectorizer().Fit(null!));

        Assert.Equal("documents", error.ParamName);
    }

    [Theory]
    [InlineData(-1.0, 1.0)]
    [InlineData(double.NaN, 1.0)]
    [InlineData(1.5, 1.0)]
    [InlineData(double.PositiveInfinity, 1.0)]
    [InlineData(1.0, -0.5)]
    [InlineData(1.0, double.NaN)]
    [InlineData(1.0, 2.5)]
    public void A_document_frequency_scikit_learn_refuses_is_refused(double minDf, double maxDf)
    {
        var options = new CountVectorizerOptions { MinDf = minDf, MaxDf = maxDf };

        var error = Assert.Throws<ArgumentOutOfRangeException>(() => new CountVectorizer(options));

        Assert.Equal("options", error.ParamName);
    }

    [Theory]
    [InlineData(0.6, 0.4)]
    [InlineData(3.0, 2.0)]
    public void A_max_df_below_min_df_is_refused_at_fit(double minDf, double maxDf)
    {
        // scikit-learn: ValueError("max_df corresponds to < documents than min_df").
        var cv = new CountVectorizer(new CountVectorizerOptions { MinDf = minDf, MaxDf = maxDf });

        Assert.Throws<InvalidOperationException>(() => cv.Fit(Corpus));
    }

    [Fact]
    public void An_empty_corpus_still_fits_to_no_columns()
    {
        Assert.Equal(0, new CountVectorizer().FitTransform([]).ColumnCount);
    }

    [Fact]
    public void Construction_names_options_for_a_bad_width_or_ngram_range()
    {
        var width = Assert.Throws<ArgumentOutOfRangeException>(
            () => new HashingVectorizer(new HashingVectorizerOptions { NumFeatures = 0 }));
        var range = Assert.Throws<ArgumentException>(
            () => new CountVectorizer(new CountVectorizerOptions { NgramRange = (2, 1) }));

        Assert.Equal("options", width.ParamName);
        Assert.Equal("options", range.ParamName);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    public void A_first_ngram_length_below_one_is_analysed_rather_than_refused(int min, int max)
    {
        // scikit-learn validates only that the range ascends (#1065), so all three fit there.
        // The oracle corpus holds the terms each produces; this holds that none is refused.
        var options = new CountVectorizerOptions { NgramRange = (min, max) };

        CsrMatrix counts = new CountVectorizer(options).FitTransform(Corpus);
        CsrMatrix weights = new TfidfVectorizer(new TfidfVectorizerOptions { Count = options }).FitTransform(Corpus);
        CsrMatrix hashed = new HashingVectorizer(new HashingVectorizerOptions { Count = options }).Transform(Corpus);

        Assert.All([counts, weights, hashed], m => Assert.Equal(Corpus.Length, m.RowCount));
    }

    [Fact]
    public void A_zero_first_ngram_length_adds_the_empty_term_at_every_position_plus_one()
    {
        // "the cat eats" keeps three tokens and the zero-length slice is taken at each of the four
        // positions; Max = 1 skips the slicing for words, so (0, 1) is (1, 1). Measured on 1.9.1.
        var zeroToTwo = new CountVectorizer(new CountVectorizerOptions { NgramRange = (0, 2) });

        double[,] counts = zeroToTwo.FitTransform(["the cat eats"]).ToDense();

        // The vocabulary is sorted, so the empty term is the first column.
        Assert.Equal(string.Empty, zeroToTwo.GetFeatureNames()[0]);
        Assert.Equal(4.0, counts[0, 0]);
        Assert.Equal(
            new CountVectorizer(new CountVectorizerOptions { NgramRange = (1, 1) }).Fit(Corpus).GetFeatureNames(),
            new CountVectorizer(new CountVectorizerOptions { NgramRange = (0, 1) }).Fit(Corpus).GetFeatureNames());
    }

    [Fact]
    public void Saving_an_unfitted_vectorizer_writes_nothing_to_the_stream()
    {
        using var count = new MemoryStream();
        using var tfidf = new MemoryStream();

        Assert.Throws<InvalidOperationException>(() => new CountVectorizer().Save(count));
        Assert.Throws<InvalidOperationException>(() => new TfidfVectorizer().Save(tfidf));

        Assert.Equal(0, count.Length);
        Assert.Equal(0, tfidf.Length);
    }

    [Fact]
    public void Transformer_idf_is_computed_whatever_use_idf_says_and_refused_before_fit()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(Corpus);
        var unfitted = new TfidfTransformer();
        var withoutIdf = new TfidfTransformer(new TfidfOptions { UseIdf = false }).Fit(counts);

        Assert.Throws<InvalidOperationException>(() => unfitted.Idf);
        Assert.Equal(counts.ColumnCount, withoutIdf.Idf.Count);
    }

    [Fact]
    public void Transformer_refuses_to_weight_before_fit_only_when_it_needs_idf()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(Corpus);

        Assert.Throws<InvalidOperationException>(() => new TfidfTransformer().Transform(counts));
        CsrMatrix weighted = new TfidfTransformer(new TfidfOptions { UseIdf = false, Norm = null }).Transform(counts);

        Assert.Equal(counts.Values, weighted.Values);
    }

    [Fact]
    public void Transformer_refuses_a_matrix_of_another_width()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(Corpus);
        CsrMatrix narrower = new CountVectorizer().FitTransform(["the cat"]);
        TfidfTransformer transformer = new TfidfTransformer().Fit(counts);

        var error = Assert.Throws<ArgumentException>(() => transformer.Transform(narrower));

        Assert.Equal("counts", error.ParamName);
    }
}
