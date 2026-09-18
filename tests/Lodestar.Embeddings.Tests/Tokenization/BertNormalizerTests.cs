using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>
/// U+FFFE is the one code point .NET 10's <see cref="string.Normalize(System.Text.NormalizationForm)"/>
/// refuses of the 819,533 its tables call unassigned, so on this runtime it is the only input that
/// reaches the segmented decomposition behind the accent strip; under NLS, which the
/// <c>netstandard2.0</c> assembly meets on .NET Framework, every unassigned code point reaches it.
/// Both mirrors run this, which is what covers the two paths (#1050). <c>vocab_txt.json</c> replays
/// the same two texts against <c>tokenizers</c>, which keeps the noncharacter as it keeps the rest.
/// </summary>
public sealed class BertNormalizerTests
{
    private static WordPieceTokenizer Uncased()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["[UNK]"] = 0,
            ["a"] = 1,
            ["##a"] = 2,
            ["￾"] = 3,
            ["##￾"] = 4,
        };
        return new WordPieceTokenizer(
            new WordPieceVocabulary(vocab, "[UNK]", "##", Lowercase: true) { BasicTokenization = true });
    }

    [Fact]
    public void A_noncharacter_survives_the_normalizer_and_its_accented_neighbours_are_stripped()
    {
        TokenizationResult result = Uncased().Encode("Á￾á");

        Assert.Equal(["a", "##￾", "##a"], result.Tokens);
    }

    [Fact]
    public void The_noncharacter_is_a_word_character_so_it_does_not_split_the_word()
    {
        TokenizationResult result = Uncased().Encode("a￾a");

        Assert.Equal(["a", "##￾", "##a"], result.Tokens);
    }
}
