using Lodestar.Text;
using Lodestar.Text.Similarity;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.SetSim;

public sealed class SetSimilarityOracleTests
{
    private static readonly OracleFile<SetSimilarityCase> Corpus =
        OracleCorpus.Load<SetSimilarityCase>("set_similarity.json");

    [Fact]
    public void Metadata_is_textdistance()
    {
        Assert.Equal("textdistance", Corpus.Metadata.Library);
        Assert.NotEmpty(Corpus.Cases);
    }

    [Fact]
    public void Jaccard_matches_textdistance()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Jaccard,
            c => Jaccard.Similarity(c.A, c.B, qval: 1, TextElement.CodePoint),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void Dice_matches_textdistance()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Dice,
            c => SorensenDice.Similarity(c.A, c.B, qval: 1, TextElement.CodePoint),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void Overlap_matches_textdistance()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Overlap,
            c => Overlap.Similarity(c.A, c.B, qval: 1, TextElement.CodePoint),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void Tversky_matches_textdistance()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Tversky,
            c => Tversky.Similarity(c.A, c.B, element: TextElement.CodePoint),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Fact]
    public void Cosine_matches_textdistance()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Cosine,
            c => Cosine.Similarity(c.A, c.B, qval: 1, TextElement.CodePoint),
            c => $"[#{c.Id}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Theory]
    [InlineData("", "", 1.0)]      // both empty -> identical
    [InlineData("abc", "", 0.0)]   // one empty -> disjoint
    [InlineData("cat", "cot", 0.5)]
    public void Jaccard_edge_and_known(string a, string b, double expected)
    {
        Assert.Equal(expected, Jaccard.Similarity(a, b), 12);
    }

    [Theory]
    [InlineData(1, 2.0 / 3, 0.8, 1.0, 0.816496580927726, 2.0 / 3)]
    [InlineData(2, 0.0, 0.0, 0.0, 0.0, 0.0)]
    public void A_lone_surrogate_is_one_code_point_rather_than_an_error(
        int qval, double jaccard, double dice, double overlap, double cosine, double tversky)
    {
        // textdistance 4.6.3 over "a\ud800b" and "ab"; the lone surrogate used to throw in
        // char.ConvertFromUtf32 (#1261).
        const string a = "a\uD800b";
        const string b = "ab";
        const TextElement cp = TextElement.CodePoint;

        Assert.Equal(jaccard, Jaccard.Similarity(a, b, qval, cp), 12);
        Assert.Equal(dice, SorensenDice.Similarity(a, b, qval, cp), 12);
        Assert.Equal(overlap, Overlap.Similarity(a, b, qval, cp), 12);
        Assert.Equal(cosine, Cosine.Similarity(a, b, qval, cp), 12);
        Assert.Equal(tversky, Tversky.Similarity(a, b, qval: qval, element: cp), 12);
    }

    [Fact]
    public void Cosine_bigrams_dupont_dupond()
    {
        // Character bigrams: "Dupont"/"Dupond" share Du,up,po,on = 4 of 5 -> 0.8.
        Assert.Equal(0.8, Cosine.Similarity("Dupont", "Dupond", qval: 2), 12);
    }

    [Theory]
    [InlineData("a", "b", 2, 0.0)]
    [InlineData("ab", "cd", 3, 0.0)]
    [InlineData("a", "a", 2, 1.0)]
    [InlineData("", "a", 2, 0.0)]
    [InlineData("", "", 2, 1.0)]
    public void Inputs_shorter_than_qval_score_on_equality(string a, string b, int qval, double expected)
    {
        // Both bags are empty, so only the inputs can tell "a" from "b" (#882).
        foreach (TextElement element in new[] { TextElement.Utf16Unit, TextElement.CodePoint })
        {
            Assert.Equal(expected, Jaccard.Similarity(a, b, qval, element));
            Assert.Equal(expected, SorensenDice.Similarity(a, b, qval, element));
            Assert.Equal(expected, Overlap.Similarity(a, b, qval, element));
            Assert.Equal(expected, Tversky.Similarity(a, b, qval: qval, element: element));
            Assert.Equal(expected, Tversky.Similarity(a, b, alpha: 0, beta: 0, qval: qval, element: element));
            Assert.Equal(expected, Cosine.Similarity(a, b, qval, element));
        }
    }

    [Theory]
    [InlineData("", "abc", 1.0, 0.0, 0.0)]
    [InlineData("abc", "", 0.0, 1.0, 0.0)]
    [InlineData("ab", "cd", 0.0, 0.0, 0.0)]
    [InlineData("ab", "ab", 0.0, 0.0, 1.0)]
    [InlineData("ab", "abc", 0.0, 0.0, 1.0)]
    [InlineData("apple", "pineapple", 1.0, 0.0, 1.0)]
    public void Tversky_answers_textdistance_where_a_weight_is_zero(string a, string b, double alpha, double beta, double expected)
    {
        // textdistance gives 0 for an empty side before weighing anything, and divides by zero for
        // "ab"/"cd" under zero weights; both used to score 1 here (#1198).
        Assert.Equal(expected, Tversky.Similarity(a, b, alpha, beta), 12);
    }
}
