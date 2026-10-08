using Lodestar.Abstractions;
using Lodestar.Text.Internal;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

/// <summary>
/// Token patterns read as Python 3.12's <c>re.findall</c> reads them, counts included, where the translation read them
/// otherwise; where Python refuses the pattern, as 0.7.0 read it, by .NET as written (#1650, #1662, #1663).
/// </summary>
public sealed class PythonPatternGrammarTests
{
    // Pattern, text, and each token with its count, from re.findall on Python 3.12; a list, not InlineData, which would
    // turn a lone surrogate into U+FFFD on the way to the test.
    private static readonly (string Pattern, string Text, (string Token, int Count)[] Tokens)[] Cases =
    [
        // #1650 1
        ("[\\x80-\uFFFF]+", "a\uD83D\uDE00\u00E9", [("\u00E9", 1)]),
        // #1650 1
        ("[\\ud83d]", "\uD83D\uDE00\uD83Dx", [("\uD83D", 1)]),
        // #1650 1
        ("\\w+[\\x80-\uFFFF]", "A\uD83D\uDE00 b\u00E9", [("b\u00E9", 1)]),
        // #1650 2
        ("[a-\uD83D\uDE00]+", "b\uD83D\uDE00 \uD83D\uDE01", [("b\uD83D\uDE00", 1)]),
        // #1650 4
        ("(?x) [a-z]+ # [c-\\w]\n", "ab c", [("ab", 1), ("c", 1)]),
        // #1650 5
        ("[z]*", "\uD83D\uDE00", [("", 2)]),
        // #1650 5
        ("\\B", "\uD801\uDC00", []),
        // #1650 6
        ("\uD83D\uDE00+", "\uD83D\uDE00\uD83D\uDE00\uD83D\uDE00 x", [("\uD83D\uDE00\uD83D\uDE00\uD83D\uDE00", 1)]),
        // #1650 6
        ("\uD83D\uDE00?a", "a \uD83D\uDE00a", [("a", 1), ("\uD83D\uDE00a", 1)]),
        // #1650 7
        ("\\w+\\Z", "abcd aax\n", []),
        // #1650 7
        ("[a-z]{,2}x", "aax", [("aax", 1)]),
        // #1662
        ("((?#[^\\W](())", "a\uD83D\uDE00 b", [("", 5)]),
        // #1663
        ("(?x) [a-z]+   # letters, e.g. [abc\n     (\\d+)?   # optional digits ]", "abc12 de", [("", 1), ("12", 1)]),
        // #1663
        ("(?x)a#[\n(b)]", "ab a", []),
        // #1650 3, refused by Python and read as 0.7.0 read it, by .NET as written: \w holds the mark
        ("(?<n>\\w+)", "ab\u0301c", [("ab\u0301c", 1)]),
        ("\\w+\\p{L}", "e\u0301x", [("e\u0301x", 1)]),
        ("(?i-i:\\w+)", "e\u0301x", [("e\u0301x", 1)]),
        // Review A: a possessive repeat after the braces .NET reads as text, a scoped (?-x:...), escapes in comments
        ("a{,2}+b", "aaab ab", [("aab", 1), ("ab", 1)]),
        ("x{,}+y", "xxxy xy", [("xxxy", 1), ("xy", 1)]),
        ("(?x)a(?-x: b(?#c) d)", "a b d a bd", [("a b d", 1)]),
        ("(?#\\)(a)", "a", [("", 2)]),
        ("(?x)a#\\\n(b)", "ab", [("a", 1)]),
        // Review A: '.' takes a pair as one character
        // Review A: counts and group numbers with leading zeros, a bound past int.MaxValue, a back-reference to a surrogate
        ("\\w{000000000002}", "e\u0301x", []),
        ("a{,3000000000}b", "aaab", [("aaab", 1)]),
        ("(a)?(?(0000000001)x|y)", "axy", [("", 1), ("a", 1)]),
        ("(\\ud83d)\\1", "\uD83D\uD83D\uDE00 \uD83D\uD83D", [("\uD83D", 1)]),
        (".", "\uD83D\uDE00x\uDE00", [("\uD83D\uDE00", 1), ("x", 1), ("\uDE00", 1)]),
        ("(?s).", "\uD83D\uDE00\n", [("\n", 1), ("\uD83D\uDE00", 1)]),
    ];

    [Fact]
    public void Each_pattern_yields_the_tokens_and_counts_python_finds()
    {
        foreach ((string pattern, string text, (string Token, int Count)[] expected) in Cases)
        {
            Assert.Equal(expected.OrderBy(t => t.Token, StringComparer.Ordinal), Tokens(pattern, text));
        }
    }

    [Theory]
    [InlineData(@"(?<=(?:(?:x{2000000000}){2000000000}){4})a")]
    [InlineData(@"(?<=(?:a{2147483647}){2}aa)b")]
    [InlineData(@"(?i-i:a)")]
    [InlineData(@"(?#\)")]
    [InlineData("(?x)a#\\")]
    public void A_pattern_python_refuses_is_refused_by_the_translation(string pattern) =>
        Assert.Throws<ArgumentException>(() => PythonPattern.Translate(pattern));

    [Fact]
    public void A_look_behind_as_wide_as_python_allows_is_translated() =>
        Assert.NotNull(PythonPattern.Translate(@"(?<=(?:a{2147483647}){2}a)b", surrogateFree: true));

    [Fact]
    public void Nesting_python_runs_out_of_recursion_on_is_read_as_written_rather_than_overflowing_the_stack()
    {
        // Python 3.12 refuses past about 495 nested groups or 989 conditionals; the token pattern then reads the pattern
        // as .NET writes it, and so it does on a thread too short of stack for the translation.
        static string Nested(int depth) => new string('(', depth) + "a" + new string(')', depth);
        static string Conditionals(int depth) => "(a)" + string.Concat(Enumerable.Repeat("(?(1)", depth)) + "a" + new string(')', depth);
        Assert.Equal(Nested(480), PythonPattern.Translate(Nested(480), surrogateFree: true));
        Assert.Throws<ArgumentException>(() => PythonPattern.Translate(Nested(481), surrogateFree: true));
        Assert.Throws<ArgumentException>(() => PythonPattern.Translate(Conditionals(961), surrogateFree: true));
        Exception? refused = null;
        var thread = new Thread(() => refused = Record.Exception(() => PythonPattern.Translate(Conditionals(960))), 256 * 1024);
        thread.Start();
        thread.Join();
        Assert.IsType<ArgumentException>(refused);
        Assert.Equal([("a", 1)], Tokens(Nested(20_000), "a"));
    }

    private static List<(string, int)> Tokens(string pattern, string text)
    {
        var vectorizer = new CountVectorizer(new CountVectorizerOptions { Lowercase = false, TokenPattern = pattern });
        CsrMatrix counts;
        try
        {
            counts = vectorizer.FitTransform([text]);
        }
        catch (InvalidOperationException)
        {
            return [];
        }
        IReadOnlyList<string> names = vectorizer.GetFeatureNames();
        return Enumerable.Range(counts.RowPointers[0], counts.RowPointers[1] - counts.RowPointers[0])
            .Select(k => (names[counts.ColumnIndices[k]], (int)counts.Values[k]))
            .OrderBy(t => t.Item1, StringComparer.Ordinal)
            .ToList();
    }
}
