using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>
/// The segmented decomposition behind the accent strip, which #1087 made reachable from any text
/// holding a code point <c>CharUnicodeInfo</c> calls unassigned — where it used to be reached only
/// by a refusal, U+FFFE alone on .NET 10 and every unassigned one under NLS. The mirror runs the
/// <c>netstandard2.0</c> assembly on the .NET 10 runtime, so it proves that assembly answers, not
/// that a second Unicode backend was met: no job meets NLS (#1089). <c>vocab_txt.json</c> replays
/// these texts against <c>tokenizers</c>, which keeps them as it keeps the rest.
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
            ["͸᫏᫝"] = 5,
            ["##͸᫏᫝"] = 6,
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

    [Fact]
    public void Three_unassigned_code_points_cut_the_walk_three_times_and_keep_their_order()
    {
        // U+1ACF and U+1ADD are unassigned to CharUnicodeInfo and combining marks to ICU, so a
        // whole-string decomposition swaps them where tokenizers keeps them in place (#1087).
        TokenizationResult result = Uncased().Encode("Á͸᫏᫝á");

        Assert.Equal(["a", "##͸᫏᫝", "##a"], result.Tokens);
    }
}
