using Lodestar.Text.Stemming;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Stemming;

/// <summary>
/// An unpaired surrogate used to make <c>string.Normalize</c> throw inside the stemmers and the
/// accent stripping (#880). Expected values are nltk 3.10.3 and scikit-learn 1.9.0 on the same
/// strings, whose <c>unicodedata.normalize</c> keeps a lone surrogate in place.
/// </summary>
public sealed class LoneSurrogateTests
{
    private static readonly string[] Words =
        ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"];

    public static TheoryData<string, string[]> Stems => new()
    {
        { "arabic", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "danish", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "dutch", ["\uD800", "ab\uDC00", "regions\uD800", "\uD800etes", "\uD800nationalites"] },
        { "english", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9"] },
        { "finnish", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "french", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800et", "\uD800national"] },
        { "german", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "hungarian", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "italian", ["\uD800", "ab\uDC00", "r\u00E8gions\uD800", "\uD800\u00E8t\u00E8s", "\uD800nationalit\u00E8s"] },
        { "norwegian", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "portuguese", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "romanian", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
        { "spanish", ["\uD800", "ab\uDC00", "regions\uD800", "\uD800etes", "\uD800nationalites"] },
        { "swedish", ["\uD800", "ab\uDC00", "r\u00E9gions\uD800", "\uD800\u00E9t\u00E9s", "\uD800nationalit\u00E9s"] },
    };

    [Theory]
    [MemberData(nameof(Stems))]
    public void Snowball_stemmers_keep_a_lone_surrogate_as_nltk_does(string language, string[] expected)
    {
        Func<string, string> stem = Stemmer(language);

        Assert.Equal(expected, Words.Select(stem).ToArray());
    }

    [Fact]
    public void Russian_keeps_a_lone_surrogate_as_nltk_does()
    {
        string[] words = ["\uD800", "\u0451\u0436\uD800", "\u0433\u043E\u0440\u043E\u0434\u0430\uD800\u0439", "\uD800\u0433\u043E\u0440\u043E\u0434\u0430"];

        Assert.Equal(
            ["\uD800", "\u0435\u0436\uD800", "\u0433\u043E\u0440\u043E\u0434\u0430\uD800", "\uD800\u0433\u043E\u0440\u043E\u0434"],
            words.Select(RussianSnowballStemmer.Stem).ToArray());
    }

    [Fact]
    public void Composition_still_applies_on_either_side_of_a_lone_surrogate()
    {
        // The stemmers compose where nltk does not; nltk stems the composed word to itself.
        Assert.Equal("\u00E9\uD800\u00E9", DanishSnowballStemmer.Stem("e\u0301\uD800e\u0301"));
    }

    [Fact]
    public void Accent_stripping_keeps_a_lone_surrogate_as_scikit_learn_does()
    {
        var words = new CountVectorizer(new CountVectorizerOptions { StripAccents = true });
        words.Fit(["ab\uD800cd e\u0301t\u00E9"]);
        var chars = new CountVectorizer(new CountVectorizerOptions { StripAccents = true, Analyzer = AnalyzerKind.Char });
        chars.Fit(["e\u0301\uD800\u00E9"]);

        Assert.Equal(["ab", "cd", "ete"], words.GetFeatureNames());
        Assert.Equal(["e", "\uD800"], chars.GetFeatureNames());
    }

    private static Func<string, string> Stemmer(string language) => language switch
    {
        "arabic" => ArabicSnowballStemmer.Stem,
        "danish" => DanishSnowballStemmer.Stem,
        "dutch" => DutchSnowballStemmer.Stem,
        "english" => EnglishSnowballStemmer.Stem,
        "finnish" => FinnishSnowballStemmer.Stem,
        "french" => FrenchSnowballStemmer.Stem,
        "german" => GermanSnowballStemmer.Stem,
        "hungarian" => HungarianSnowballStemmer.Stem,
        "italian" => ItalianSnowballStemmer.Stem,
        "norwegian" => NorwegianSnowballStemmer.Stem,
        "portuguese" => PortugueseSnowballStemmer.Stem,
        "romanian" => RomanianSnowballStemmer.Stem,
        "spanish" => SpanishSnowballStemmer.Stem,
        "swedish" => SwedishSnowballStemmer.Stem,
        _ => throw new ArgumentOutOfRangeException(nameof(language), language, "No such stemmer."),
    };
}
