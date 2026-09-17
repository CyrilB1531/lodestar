using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>
/// Pre-tokens of <c>pre_tokenizers.Whitespace()</c>, measured with <c>tokenizers</c> 0.23.2 (issue #887).
/// The sweep behind them covered every Unicode scalar value; these are the classes .NET's <c>\w</c> got wrong.
/// </summary>
public sealed class WhitespaceScannerTests
{
    public static TheoryData<string, string[]> Measured => new()
    {
        { "a\u093Fa", ["a\u093Fa"] },                       // Mc, a Devanagari vowel sign
        { "a\u20DDa", ["a\u20DDa"] },                       // Me
        { "a\u216Ba", ["a\u216Ba"] },                       // Nl
        { "a\u200Da\u200Ca", ["a\u200Da\u200Ca"] },         // ZWJ and ZWNJ
        { "a\u24B6a", ["a\u24B6a"] },                       // So, but Other_Alphabetic
        { "a\U0001F130a", ["a\U0001F130a"] },               // the same, astral
        { "a\U00020000a", ["a\U00020000a"] },               // CJK Extension B
        { "a\U0001D7CEa", ["a\U0001D7CEa"] },               // an astral Nd
        { "a\U0001F600!a", ["a", "\U0001F600!", "a"] },     // an astral symbol joins punctuation
        { "a\u00B2a", ["a", "\u00B2", "a"] },               // No splits
        { "a\u2460a", ["a", "\u2460", "a"] },               // a circled digit is not alphabetic
        { "a\u0085a\u3000a\u180Ea", ["a", "a", "a", "\u180E", "a"] },
        { "a\u200Ba", ["a", "\u200B", "a"] },               // ZWSP is Cf, not whitespace
        { "\t hi,  there!!\r\n", ["hi", ",", "there", "!!"] },
        { "a\uD800a", ["a", "\uD800", "a"] },               // unreachable from Python, kept as the regex had it
        { " \n ", [] },
        { "", [] },
    };

    [Theory]
    [MemberData(nameof(Measured))]
    public void The_bpe_split_matches_tokenizers(string text, string[] expected)
    {
        var pieces = new List<string>();
        new BpePreTokenizer(null, BpePatterns.Whitespace, false, false).Split(text, pieces);
        Assert.Equal(expected, pieces);
    }

    [Theory]
    [MemberData(nameof(Measured))]
    public void The_scanner_matches_tokenizers(string text, string[] expected) =>
        Assert.Equal(expected, Scan(text, 0, text.Length));

    [Fact]
    public void A_pair_cut_by_the_segment_end_is_classified_as_its_half()
    {
        string text = "a\U00020000";
        Assert.Equal(["a", "\uD840"], Scan(text, 0, 2));
    }

    [Fact]
    public void A_split_step_declaring_the_whitespace_pattern_gets_the_same_split()
    {
        var pieces = new List<string>();
        var step = new BpeSplitStep(BpePatterns.Whitespace, SplitBehavior.Isolated, Invert: false);
        new BpePreTokenizer(step, null, false, false).Split("a\U00020000a b", pieces);
        Assert.Equal(["a\U00020000a", " ", "b"], pieces);
    }

    private static List<string> Scan(string text, int from, int end)
    {
        var words = new List<string>();
        int position = from;
        while (WhitespaceScanner.TryNext(text, end, ref position, out int start))
        {
            words.Add(text.Substring(start, position - start));
        }
        return words;
    }
}
