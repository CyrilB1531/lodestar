using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>
/// The token-set ratios read two of their three scores from lengths and write the joined strings
/// into one buffer; these pin both against the strings built and scored the long way.
/// </summary>
public sealed class TokenSetLengthTests
{
    public static TheoryData<string, string> Pairs => new()
    {
        { "gamma alpha beta", "beta alpha" },
        { "delta delta epsilon", "zeta eta" },
        { Words(0, 150), Words(40, 190) },
        { Words(0, 200), Words(0, 200) },
        { Words(0, 180), Words(500, 700) },
    };

    [Theory]
    [MemberData(nameof(Pairs))]
    public void TokenSetRatio_is_the_best_of_the_three_joined_comparisons(string a, string b)
    {
        (string sect, string combinedA, string combinedB) = Joined(a, b);
        double expected = Math.Max(
            Fuzz.Ratio(sect, combinedA), Math.Max(Fuzz.Ratio(sect, combinedB), Fuzz.Ratio(combinedA, combinedB)));

        Assert.Equal(expected, Fuzz.TokenSetRatio(a, b));
    }

    [Theory]
    [MemberData(nameof(Pairs))]
    public void PartialTokenSetRatio_is_the_best_of_the_three_joined_comparisons(string a, string b)
    {
        (string sect, string combinedA, string combinedB) = Joined(a, b);
        double expected = Math.Max(
            Fuzz.PartialRatio(sect, combinedA),
            Math.Max(Fuzz.PartialRatio(sect, combinedB), Fuzz.PartialRatio(combinedA, combinedB)));

        Assert.Equal(expected, Fuzz.PartialTokenSetRatio(a, b));
    }

    // The joined comparisons would score a wordless side 100; rapidfuzz returns 0 before reaching them (#860).
    [Theory]
    [InlineData("", "alpha beta")]
    [InlineData("alpha beta", "")]
    [InlineData(" ", "a")]
    [InlineData(" \t ", " ")]
    public void A_side_with_no_words_scores_zero(string a, string b)
    {
        Assert.Equal(0.0, Fuzz.TokenSetRatio(a, b));
        Assert.Equal(0.0, Fuzz.PartialTokenSetRatio(a, b));
    }

    private static (string Sect, string CombinedA, string CombinedB) Joined(string a, string b)
    {
        string[] setA = Distinct(a);
        string[] setB = Distinct(b);
        string[] shared = [.. setA.Where(token => Array.BinarySearch(setB, token, StringComparer.Ordinal) >= 0)];
        string[] onlyA = [.. setA.Where(token => Array.BinarySearch(setB, token, StringComparer.Ordinal) < 0)];
        string[] onlyB = [.. setB.Where(token => Array.BinarySearch(setA, token, StringComparer.Ordinal) < 0)];
        return (string.Join(" ", shared), string.Join(" ", shared.Concat(onlyA)), string.Join(" ", shared.Concat(onlyB)));
    }

    private static string[] Distinct(string s) =>
        [.. s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(token => token, StringComparer.Ordinal)];

    private static string Words(int from, int to) =>
        string.Join(" ", Enumerable.Range(from, to - from).Select(i => "w" + i.ToString(System.Globalization.CultureInfo.InvariantCulture)));
}
