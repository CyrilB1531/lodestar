using Lodestar.Text.Phonetics;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Phonetics;

public sealed class MetaphoneOracleTests
{
    // Real words and names chosen to cover the rules; decision 0009 puts every other input
    // inside the claim too, and phonetics_contract.json replays those.
    private static readonly OracleFile<PhoneticCase> Corpus =
        OracleCorpus.Load<PhoneticCase>("metaphone.json");

    [Fact]
    public void Metaphone_matches_jellyfish()
    {
        OracleAsserts.ExactString(Corpus.Cases,
            c => c.Metaphone,
            c => Metaphone.Encode(c.Word),
            c => $"[#{c.Id}] \"{c.Word}\"");
    }

    [Theory]
    [InlineData("Knuth", "N0")]
    [InlineData("Catherine", "K0RN")]
    [InlineData("Jackson", "JKSN")]
    [InlineData("MacDonald", "MKTNLT")]
    [InlineData("Knighted", "NTT")]
    [InlineData("Thomas", "0MS")]
    [InlineData("Fletcher", "FLXR")]
    [InlineData("Highness", "HNS")]
    [InlineData("Keats's", "KTSS")]
    [InlineData("", "")]
    public void Metaphone_known_values(string word, string expected)
    {
        Assert.Equal(expected, Metaphone.Encode(word));
    }

    [Fact]
    public void Encode_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Metaphone.Encode(null!));
    }
}
