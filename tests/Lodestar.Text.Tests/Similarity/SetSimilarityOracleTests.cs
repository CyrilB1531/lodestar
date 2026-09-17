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
}
