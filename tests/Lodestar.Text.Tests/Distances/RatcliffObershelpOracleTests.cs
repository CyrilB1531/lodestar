using Lodestar.Text.Distances;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

public sealed class RatcliffObershelpOracleTests
{
    private static readonly OracleFile<SimilarityCase> Corpus =
        OracleCorpus.Load<SimilarityCase>("ratcliff.json");

    [Fact]
    public void Metadata_is_difflib()
    {
        Assert.Equal("difflib", Corpus.Metadata.Library);
        Assert.NotEmpty(Corpus.Cases);
    }

    [Fact]
    public void Similarity_matches_difflib()
    {
        OracleAsserts.Approx(Corpus.Cases,
            c => c.Similarity,
            c => RatcliffObershelp.Similarity(c.A, c.B, TextElement.CodePoint),
            c => $"[#{c.Id} {c.Category}] {OracleAsserts.Escape(c.A)}/{OracleAsserts.Escape(c.B)}");
    }

    [Theory]
    [InlineData("", "", 1.0)]
    [InlineData("abc", "abc", 1.0)]
    [InlineData("kitten", "sitting", 0.6153846153846154)]
    [InlineData("Dupont", "Dupond", 0.8333333333333334)]
    public void Known_values(string a, string b, double expected)
    {
        Assert.Equal(expected, RatcliffObershelp.Similarity(a, b), 12);
    }

    [Fact]
    public void A_chain_of_single_element_blocks_does_not_exhaust_a_small_stack()
    {
        // 500 chained blocks overflowed a 64 KiB thread while the pairing recursed (#877).
        const int n = 500;
        string a = new('a', n);
        string b = string.Concat(Enumerable.Repeat("ab", n));
        double utf16 = double.NaN;
        double codePoints = double.NaN;
        var thread = new Thread(
            () =>
            {
                utf16 = RatcliffObershelp.Similarity(a, b);
                codePoints = RatcliffObershelp.Similarity(a, b, TextElement.CodePoint);
            },
            64 * 1024);
        thread.Start();
        thread.Join();

        // difflib.SequenceMatcher(None, "a" * 500, "ab" * 500, autojunk=False).ratio()
        Assert.Equal(2.0 / 3.0, utf16, 12);
        Assert.Equal(2.0 / 3.0, codePoints, 12);
    }
}
