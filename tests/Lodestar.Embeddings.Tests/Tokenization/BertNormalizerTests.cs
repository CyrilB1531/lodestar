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

    /// <summary>The runtime's NFD, refusing U+1F600 as an NLS older than the astral emoji would (#1094).</summary>
    private static string RefusesTheEmoji(string text) =>
        text.Contains("\U0001F600", StringComparison.Ordinal)
            ? throw new ArgumentException("Invalid Unicode code point found.", nameof(text))
            : text.Normalize(System.Text.NormalizationForm.FormD);

    [Fact]
    public void A_refused_code_point_beside_an_unassigned_one_passes_through_and_its_neighbours_are_decomposed()
    {
        string decomposed = BertBasicTokenization.Decompose("\U0001F600Á\u0378é", unassigned: true, RefusesTheEmoji);

        Assert.Equal("\U0001F600A\u0301\u0378e\u0301", decomposed);
    }

    [Fact]
    public void A_refused_code_point_no_table_calls_unassigned_passes_through_where_it_used_to_throw()
    {
        string decomposed = BertBasicTokenization.Decompose("Á\U0001F600é", unassigned: false, RefusesTheEmoji);

        Assert.Equal("A\u0301\U0001F600e\u0301", decomposed);
    }

    [Fact]
    public void A_refusal_no_single_code_point_explains_leaves_its_stretch_as_it_is()
    {
        static string RefusesThePair(string text) =>
            text.Contains("ab", StringComparison.Ordinal)
                ? throw new ArgumentException("Invalid Unicode code point found.", nameof(text))
                : text.Normalize(System.Text.NormalizationForm.FormD);

        string decomposed = BertBasicTokenization.Decompose("Áab", unassigned: false, RefusesThePair);

        Assert.Equal("Áab", decomposed);
    }
}
