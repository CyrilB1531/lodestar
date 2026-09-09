using Lodestar.Text.Stemming;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

public sealed class ArabicSnowballStemmerOracleTests
{
    private static readonly OracleFile<PorterCase> Corpus = OracleCorpus.Load<PorterCase>("snowball_ar.json");

    /// <summary>Frozen from snowballstemmer rather than nltk — decision 0094.</summary>
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
            c => ArabicSnowballStemmer.Stem(c.Word),
            c => $"[#{c.Id}] \"{c.Word}\"");
    }

    [Fact]
    public void Stem_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ArabicSnowballStemmer.Stem(null!));
    }
}
