using Lodestar.Fuzzy;
using Lodestar.Text;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>
/// A null string scores 0 in every scorer, on either side or both, as rapidfuzz 3.14.6 scores <c>None</c>:
/// <c>fuzz.ratio(None, "a") == 0</c> and likewise for the six others (#1611). The token scorers once reached
/// <c>string.Split</c> with it (#891), and every scorer then threw <see cref="ArgumentNullException"/>.
/// </summary>
public sealed class FuzzNullScoreTests
{
    /// <summary>Each scorer's two overloads, by name.</summary>
    private static readonly Dictionary<string, (Func<string?, string?, double> Utf16, Func<string?, string?, TextElement, double> ByElement)> Overloads = new()
    {
        [nameof(Fuzz.Ratio)] = (Fuzz.Ratio, Fuzz.Ratio),
        [nameof(Fuzz.PartialRatio)] = (Fuzz.PartialRatio, Fuzz.PartialRatio),
        [nameof(Fuzz.TokenSortRatio)] = (Fuzz.TokenSortRatio, Fuzz.TokenSortRatio),
        [nameof(Fuzz.TokenSetRatio)] = (Fuzz.TokenSetRatio, Fuzz.TokenSetRatio),
        [nameof(Fuzz.PartialTokenSortRatio)] = (Fuzz.PartialTokenSortRatio, Fuzz.PartialTokenSortRatio),
        [nameof(Fuzz.PartialTokenSetRatio)] = (Fuzz.PartialTokenSetRatio, Fuzz.PartialTokenSetRatio),
        [nameof(Fuzz.WRatio)] = (Fuzz.WRatio, Fuzz.WRatio),
    };

    public static TheoryData<string> Scorers => new(Overloads.Keys);

    private static Func<string?, string?, double> Utf16(string name) => Overloads[name].Utf16;

    private static Func<string?, string?, TextElement, double> ByElement(string name) => Overloads[name].ByElement;

    [Theory]
    [MemberData(nameof(Scorers))]
    public void A_null_string_scores_zero_on_either_side(string name)
    {
        foreach ((string? a, string? b) in new (string?, string?)[] { (null, "a"), ("a", null), (null, null), (null, ""), ("", null) })
        {
            Assert.Equal(0.0, Utf16(name)(a, b));
            Assert.Equal(0.0, ByElement(name)(a, b, TextElement.Utf16Unit));
            Assert.Equal(0.0, ByElement(name)(a, b, TextElement.CodePoint));
        }
    }

    [Theory]
    [MemberData(nameof(Scorers))]
    public void An_undeclared_unit_is_refused_before_a_null_is_scored(string name)
    {
        foreach ((string? a, string? b) in new (string?, string?)[] { (null, "a"), ("a", null), (null, null) })
        {
            ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(() => ByElement(name)(a, b, (TextElement)2));
            Assert.Equal("element", error.ParamName);
        }
    }
}
