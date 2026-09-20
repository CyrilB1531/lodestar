using Lodestar.Text.Stemming;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

public sealed class FrenchSnowballStemmerOracleTests
{
    private static readonly OracleFile<PorterCase> Corpus = OracleCorpus.Load<PorterCase>("snowball_fr.json");

    /// <summary>Frozen from snowballstemmer rather than nltk: decision 0006.</summary>
    [Fact]
    public void Metadata_is_snowballstemmer()
    {
        Assert.Equal("snowballstemmer", Corpus.Metadata.Library);
        Assert.NotEmpty(Corpus.Cases);
    }

    [Fact]
    public void Stem_matches_snowballstemmer()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Stem,
            c => FrenchSnowballStemmer.Stem(c.Word),
            c => $"[#{c.Id}] \"{c.Word}\"");
    }

    [Theory]
    [InlineData("continuellement", "continuel")]
    [InlineData("amoureusement", "amour")]
    [InlineData("chevaux", "cheval")]
    [InlineData("finissait", "fin")]
    [InlineData("gentiment", "gent")]
    [InlineData("prière", "prier")]
    // One word per cause of #973: step 2b's longest suffix, step 1's -if, the prelude read in order.
    [InlineData("abaissassiez", "abaiss")]
    [InlineData("abusif", "abus")]
    [InlineData("abdiquiez", "abdiqu")]
    [InlineData("aboyiez", "aboi")]
    [InlineData("acière", "acier")]
    // Where nltk's FrenchStemmer gives another stem: ind, ès, l'avion, bijoux, canoë, albanais.
    [InlineData("indicatrice", "indiqu")]
    [InlineData("ès", "es")]
    [InlineData("l'avion", "avion")]
    [InlineData("bijoux", "bijou")]
    [InlineData("canoë", "cano")]
    [InlineData("albanaise", "alban")]
    public void Stem_known_values(string word, string expected)
    {
        Assert.Equal(expected, FrenchSnowballStemmer.Stem(word));
    }

    [Fact]
    public void Stem_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => FrenchSnowballStemmer.Stem(null!));
    }
}
