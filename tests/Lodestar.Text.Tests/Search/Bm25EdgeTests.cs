using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Search;

/// <summary>What the index refuses, and the shapes it promises.</summary>
public sealed class Bm25EdgeTests
{
    private static CsrMatrix Counts() => new CountVectorizer().FitTransform(
        ["the cat sat", "the dog sat sat", "a bird flew far away today"]);

    [Fact]
    public void A_null_matrix_is_refused() =>
        Assert.Throws<ArgumentNullException>(() => new Bm25Index(null!));

    [Fact]
    public void A_null_query_is_refused() =>
        Assert.Throws<ArgumentNullException>(() => new Bm25Index(Counts()).Score(null!));

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    public void A_negative_or_undefined_k1_is_refused(double k1) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Bm25Index(Counts(), new Bm25Options(K1: k1)));

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    public void A_b_outside_the_unit_interval_is_refused(double b) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Bm25Index(Counts(), new Bm25Options(B: b)));

    [Fact]
    public void A_negative_count_is_refused_by_Top() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bm25Index(Counts()).Top([0], -1));

    /// <summary>A term the corpus never saw scores nothing rather than throwing.</summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(9999)]
    public void A_query_term_outside_the_vocabulary_is_ignored(int term)
    {
        double[] scores = new Bm25Index(Counts()).Score([term]);

        Assert.All(scores, score => Assert.Equal(0.0, score));
    }

    /// <summary>Top-k returns what there is when the corpus is smaller than k.</summary>
    [Fact]
    public void Asking_for_more_documents_than_exist_returns_them_all()
    {
        IReadOnlyList<SearchHit> hits = new Bm25Index(Counts()).Top([0], 100);

        Assert.Equal(3, hits.Count);
    }

    /// <summary>A query that separates nothing returns the corpus in its own order.</summary>
    [Fact]
    public void Ties_break_by_document_index_ascending()
    {
        IReadOnlyList<SearchHit> hits = new Bm25Index(Counts()).Top([], 3);

        Assert.Equal([0, 1, 2], hits.Select(hit => hit.Document));
        Assert.All(hits, hit => Assert.Equal(0.0, hit.Score));
    }

    /// <summary>
    /// The two IDF variants are different numbers, which is why the option exists rather
    /// than a default nobody states.
    /// </summary>
    [Fact]
    public void The_lucene_variant_scores_differently_from_the_floored_robertson_one()
    {
        CsrMatrix counts = Counts();
        // "sat" is in two of three documents, so Robertson's IDF for it is negative and
        // floored, where Lucene's is positive by construction.
        int sat = Array.IndexOf([.. new CountVectorizer().Fit(
            ["the cat sat", "the dog sat sat", "a bird flew far away today"]).GetFeatureNames()], "sat");

        double floored = new Bm25Index(counts).Score([sat])[1];
        double lucene = new Bm25Index(counts, new Bm25Options(Idf: Bm25Idf.Lucene)).Score([sat])[1];

        Assert.True(floored > 0.0);
        Assert.True(lucene > floored);
    }

    /// <summary>Length normalization is what b buys, so switching it off must be visible.</summary>
    /// <remarks>
    /// Five documents, the query term in two of them: enough that Robertson's IDF stays
    /// positive. In a corpus where it goes negative the comparison inverts — a longer
    /// document then scores <em>higher</em> — which is a property of the floored IDF and
    /// not of the normalization, and is why this fixture is sized the way it is.
    /// </remarks>
    [Fact]
    public void Setting_b_to_zero_stops_length_from_mattering()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(
        [
            "term",
            "term filler filler filler filler filler",
            "other words entirely",
            "more unrelated content",
            "nothing relevant here",
        ]);
        int term = Array.IndexOf(
            [.. new CountVectorizer().Fit(
            [
                "term",
                "term filler filler filler filler filler",
                "other words entirely",
                "more unrelated content",
                "nothing relevant here",
            ]).GetFeatureNames()],
            "term");

        double[] normalized = new Bm25Index(counts).Score([term]);
        double[] flat = new Bm25Index(counts, new Bm25Options(B: 0.0)).Score([term]);

        Assert.True(normalized[0] > 0.0, "the IDF must be positive for this comparison to mean anything");
        Assert.True(normalized[0] > normalized[1]);
        Assert.Equal(flat[0], flat[1], 1e-12);
    }
}
