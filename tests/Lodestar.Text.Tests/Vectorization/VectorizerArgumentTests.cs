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
    public void A_corpus_leaving_no_term_is_refused()
    {
        // scikit-learn raises ValueError on each (#1239): no term at all, and none left by the bounds.
        Assert.Throws<InvalidOperationException>(() => new CountVectorizer().FitTransform([]));
        Assert.Throws<InvalidOperationException>(() => new CountVectorizer().Fit(["a b", ""]));
        Assert.Throws<InvalidOperationException>(() => new TfidfVectorizer().Fit(["a b"]));
        Assert.Throws<InvalidOperationException>(
            () => new CountVectorizer(new CountVectorizerOptions { MinDf = 2 }).Fit(["ab cd", "ef gh"]));
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

    [Fact]
    public void An_undefined_analyzer_is_refused_at_construction()
    {
        // It used to run the word analyzer and then fail at Save, 98 bytes into the stream (#1198).
        var count = new CountVectorizerOptions { Analyzer = (AnalyzerKind)7 };

        Assert.Equal("options", Assert.Throws<ArgumentException>(() => new CountVectorizer(count)).ParamName);
        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => new TfidfVectorizer(new TfidfVectorizerOptions { Count = count })).ParamName);
        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => new HashingVectorizer(new HashingVectorizerOptions { Count = count })).ParamName);
    }

    [Fact]
    public void A_token_pattern_with_two_groups_is_refused_for_the_word_analyzer()
    {
        // scikit-learn's build_tokenizer: "More than 1 capturing group in token pattern" (#1262).
        var options = new CountVectorizerOptions { TokenPattern = @"(\w)(\w)" };

        var error = Assert.Throws<ArgumentException>(() => new CountVectorizer(options));
        Assert.Contains("2 capturing groups", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_null_token_pattern_is_refused_whatever_the_analyzer()
    {
        // Save writes the pattern and Load requires a string, so the character analyzers,
        // which never read it, still cannot take a null one.
        var options = new CountVectorizerOptions { Analyzer = AnalyzerKind.Char, TokenPattern = null! };

        Assert.Throws<ArgumentException>(() => new CountVectorizer(options));
    }

    [Fact]
    public void A_token_pattern_with_two_groups_is_ignored_by_the_character_analyzers()
    {
        // scikit-learn builds no tokenizer for char and char_wb, so it never reads the pattern.
        var options = new CountVectorizerOptions { Analyzer = AnalyzerKind.Char, TokenPattern = @"(\w)(\w)" };

        Assert.Equal(["a", "b"], new CountVectorizer(options).Fit(["ab"]).GetFeatureNames());
    }

    [Fact]
    public void Binary_hashing_stores_one_for_every_bucket_a_term_reached()
    {
        // scikit-learn fills every stored entry with 1, a bucket whose signs cancelled included (#1196).
        var options = new HashingVectorizerOptions
        {
            Count = new CountVectorizerOptions { Binary = true },
            NumFeatures = 2,
            Norm = null,
        };

        CsrMatrix hashed = new HashingVectorizer(options).Transform(Corpus);

        Assert.All(hashed.Values, value => Assert.Equal(1.0, value));
        Assert.Equal(new HashingVectorizer(options with { Count = new CountVectorizerOptions() }).Transform(Corpus).ColumnIndices, hashed.ColumnIndices);
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
