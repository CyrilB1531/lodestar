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
        // Review A: counts and group numbers with leading zeros, a bound past int.MaxValue, a back-reference to a surrogate
        ("\\w{000000000002}", "e\u0301x", []),
        ("a{,3000000000}b", "aaab", [("aaab", 1)]),
        ("(a)?(?(0000000001)x|y)", "axy", [("", 1), ("a", 1)]),
        ("(\\ud83d)\\1", "\uD83D\uD83D\uDE00 \uD83D\uD83D", [("\uD83D", 1)]),
        // #1668: IGNORECASE folds as _sre folds, long s and Kelvin sign included, a supplementary letter too
        ("(?i)s", "\u017F S s x", [("S", 1), ("s", 1), ("\u017F", 1)]),
        ("(?i)[a-z]+", "\u212A\u017F ab", [("\u212A\u017F", 1), ("ab", 1)]),
        ("(?i)\uD801\uDC00", "\uD801\uDC28 \uD801\uDC00", [("\uD801\uDC00", 1), ("\uD801\uDC28", 1)]),
        ("(?i:\u03C3)+", "\u03A3\u03C3\u03C2\u03A3", [("\u03A3\u03C3\u03C2\u03A3", 1)]),
        ("a(?i:b)c", "abc aBc ABC abC", [("aBc", 1), ("abc", 1)]),
        ("(?i)(?-i:a)b", "aB Ab", [("aB", 1)]),
        ("(?i)[^a-z]+", "\u212A\u017FAb 12", [(" 12", 1)]),
        ("(?i)(a)\\1", "aA Aa ab", [("A", 1), ("a", 1)]),
        ("(?i)\u01C5", "\u01C4 \u01C5 \u01C6", [("\u01C4", 1), ("\u01C5", 1), ("\u01C6", 1)]),
        ("(?i)[\u01C6]", "\u01C4\u01C5\u01C6", [("\u01C4", 1), ("\u01C5", 1), ("\u01C6", 1)]),
        ("(?i)\u00DF", "\u00DF \u1E9E ss", [("\u00DF", 1), ("\u1E9E", 1)]),
        // A case Unicode added after 15.0, which Python 3.12 does not fold: a Garay capital and its small letter
        ("(?i)\uD803\uDD50", "\uD803\uDD70 \uD803\uDD50", [("\uD803\uDD50", 1)]),
        // #1665, #1666: a loop of units ends between two code points, and no match starts inside a pair
        (".+?", "\uD83D\uDE00a", [("a", 1), ("\uD83D\uDE00", 1)]),
        ("[^a]+", "\uD83D\uDE00\uD83D\uDE00a\uD83D\uDE00", [("\uD83D\uDE00", 1), ("\uD83D\uDE00\uD83D\uDE00", 1)]),
        (".{2}", "\uD83D\uDE00\uD83D\uDE00a", [("\uD83D\uDE00\uD83D\uDE00", 1)]),
        ("\\S*?\\d", "\uD83D\uDE00\uD83D\uDE001 \uD83D\uDE002", [("\uD83D\uDE002", 1), ("\uD83D\uDE00\uD83D\uDE001", 1)]),
        ("\\ude00", "\uD83D\uDE00 \uDE00", [("\uDE00", 1)]),
        (".+\\ude00", "\uD83D\uDE00 x\uDE00", [("\uD83D\uDE00 x\uDE00", 1)]),
        ("(?s).*?x", "\uD83D\uDE00\nx", [("\uD83D\uDE00\nx", 1)]),
        ("[^\\s]{1,}", "a\uD83D\uDE00 b", [("a\uD83D\uDE00", 1), ("b", 1)]),
        // Review A: inside a look-behind, read right to left, no item starts inside a pair
        ("(?<=\\W)a", "\uD835\uDC00a", []),
        ("(?<=[\\udc00-\\udfff])a", "\uD83D\uDE00a", []),
        ("(?<=\\udc00)a", "\uD800\uDC00a", []),
        ("(?<=.)a", "\uD83D\uDE00a", [("a", 1)]),
        // Review A: branches of one literal, class or category each are one charset, as Python's parser makes them
        ("(?:a|b|\\d)+", "ab1c 2a", [("2a", 1), ("ab1", 1)]),
        ("(?:\\w|\\S)+!", new string('a', 30), []),
        ("(?:[a-z]|\\d|\\w)+!", new string('a', 31) + "\uD83D\uDE00", []),
        ("(?i)(?:K|s)+", "kS\u017FK", [("kS\u017FK", 1)]),
        ("(?:[a-]|z)", "-am z", [("-", 1), ("a", 1), ("z", 1)]),
        ("(?:[\\w-]|x)", "\uD801\uDC00 x -", [("-", 1), ("x", 1), ("\uD801\uDC00", 1)]),
        ("(?:x|[-a])", "-ax b", [("-", 1), ("a", 1), ("x", 1)]),
        // Review A: after an empty match Python tries a non-empty one at the same place
        ("|a", "a", [("", 2), ("a", 1)]),
        ("x*|b", "ab", [("", 3), ("b", 1)]),
        ("a*?", "aa", [("", 3), ("a", 2)]),
        ("|a", "a\uD83D\uDE00", [("", 3), ("a", 1)]),
        ("\\w*", "ab cd", [("", 2), ("ab", 1), ("cd", 1)]),
        ("(a|)", "aa", [("", 1), ("a", 2)]),
        ("(?:)|b", "ab b", [("", 5), ("b", 2)]),
        // A repeat of an item that can match empty, read compiled: .NET's interpreter matched it at 3, Python at 0
        ("(?:a|)+?.+|x", "\uD801\uDC00\uD801\uDC00\uD83D\uDE00", [("\uD801\uDC00\uD801\uDC00\uD83D\uDE00", 1)]),
        // #1669: \B never matches an empty document
        ("\\B", "", []),
        // '.' takes a pair as one character
        (".", "\uD83D\uDE00x\uDE00", [("\uD83D\uDE00", 1), ("x", 1), ("\uDE00", 1)]),
        ("(?s).", "😀\n", [("\n", 1), ("😀", 1)]),
        // #1672: a class or branches joining sets that hold every pair between them, or one set twice
        ("[\\w\\S]+\\.i", T, [("1.i", 1)]),
        ("(?i)(\\w|\\w|\\S)+\\S\\.i", T, []),
        ("[\\W\\w]", T, [(" ", 2), (".", 1), ("1", 1), ("a", 1), ("i", 1), ("x", 1), ("²", 1), ("𐐀", 1), ("𝟎", 1), ("😀", 1)]),
        ("[\\d\\D]", T, [(" ", 2), (".", 1), ("1", 1), ("a", 1), ("i", 1), ("x", 1), ("²", 1), ("𐐀", 1), ("𝟎", 1), ("😀", 1)]),
        ("[\\w\\D]", T, [(" ", 2), (".", 1), ("1", 1), ("a", 1), ("i", 1), ("x", 1), ("²", 1), ("𐐀", 1), ("𝟎", 1), ("😀", 1)]),
        ("[\\W\\d]+", T, [(" ", 1), ("𝟎", 1), ("😀 1.", 1)]),
        ("[^\\w\\S]", T, [(" ", 2)]),
        ("[^\\W\\d]+", T, [("a", 1), ("i", 1), ("x²", 1), ("𐐀", 1)]),
        ("[\\d\\w]+", T, [("1", 1), ("a𝟎𐐀", 1), ("i", 1), ("x²", 1)]),
        ("(\\d|\\w|\\W)", T, [(" ", 2), (".", 1), ("1", 1), ("a", 1), ("i", 1), ("x", 1), ("²", 1), ("𐐀", 1), ("𝟎", 1), ("😀", 1)]),
        ("(\\w|\\W)+", T, [("²", 1)]),
        ("[^\\d\\D]", "a😀", []),
    ];

    // Letters, digits and symbols, each in the BMP and as a pair.
    private const string T = "a\uD835\uDFCE\uD801\uDC00\uD83D\uDE00 1.i x\u00B2";

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
        // Python 3.12 refuses past 495 nested groups or 990 conditionals, fewer under a deeper caller; past 480 and 960,
        // a margin below both, the token pattern reads the pattern as .NET writes it. Any thread translates alike (#1667).
        static string Nested(int depth) => new string('(', depth) + "a" + new string(')', depth);
        static string Conditionals(int depth) => "(a)" + string.Concat(Enumerable.Repeat("(?(1)", depth)) + "a" + new string(')', depth);
        Assert.Equal(Nested(480), PythonPattern.Translate(Nested(480), surrogateFree: true));
        Assert.Throws<ArgumentException>(() => PythonPattern.Translate(Nested(481), surrogateFree: true));
        Assert.Throws<ArgumentException>(() => PythonPattern.Translate(Conditionals(961), surrogateFree: true));
        string? onShortStack = null;
        var thread = new Thread(() => onShortStack = PythonPattern.Translate(Conditionals(960)), 256 * 1024);
        thread.Start();
        thread.Join();
        Assert.Equal(PythonPattern.Translate(Conditionals(960)), onShortStack);
        Assert.Equal([("a", 1)], Tokens(Nested(20_000), "a"));
    }

    [Theory]
    [InlineData(".+x")]
    [InlineData(@"\S+\d")]
    [InlineData(@"[^ ]+\d")]
    [InlineData(@"\w+\d")]
    public void A_greedy_loop_over_a_long_text_holding_surrogates_completes(string pattern)
    {
        // 20,000 units with no 'x' nor digit the loop could end on: 0.7.0 and Python complete, where the pair-aware
        // spelling ran past the 1 s timeout (#1666).
        string text = string.Concat(Enumerable.Repeat("\u4E2D\uD835\uDC00a\uD83D\uDE00", 4000)) + "!";
        Assert.Empty(Tokens(pattern, text));
    }

    [Fact]
    public void A_short_text_the_interpreter_does_not_finish_in_a_second_is_read_again_compiled()
    {
        // Three loops in a row over 128 emoji: interpreted, past the second; compiled, unbounded, it completes (#1666).
        Assert.Empty(Tokens(@"\W+\W+\W+\d", string.Concat(Enumerable.Repeat("\uD83D\uDE00", 128))));
    }

    [Fact]
    public void A_short_text_read_again_compiled_is_read_compiled_from_then_on()
    {
        // The first text pays the interpreter's second, the next one like it no longer does (#1673). A pattern no other
        // test reads, as the compiled spelling is shared by every reader of the pattern in the process.
        var pattern = new PythonTokenPattern(@"(?:\W+\W+\W+\d)");
        string text = string.Concat(Enumerable.Repeat("\uD83D\uDE00", 128));
        Assert.Empty(pattern.Matches(text));
        Assert.True(pattern.LastReadInterpreted);
        Assert.Empty(pattern.Matches(text));
        Assert.False(pattern.LastReadInterpreted);
    }

    [Fact]
    public void A_lazy_repeat_with_a_bound_over_a_group_is_read_as_the_compiled_engine_reads_it()
    {
        // Python gives four ''; .NET's compiled engine keeps the capture of an iteration it gave up, as 0.7.0's did, and
        // docs/equivalence.md says so (#1676).
        Assert.Equal([("", 3), ("a", 1)], Tokens(@".([a-z]){,2}?I|", "1ai"));
    }

    [Fact]
    public void A_pattern_the_engines_read_apart_takes_no_other_readers_compiled_spelling()
    {
        // Another reader of the pattern compiles it over a long text; this one still reads its short text interpreted,
        // as Python does, rather than as the compiled engine, which keeps a capture it gave up (#1676, #1677).
        const string Pattern = @"(?:.([a-z]){,2}?I|)";
        Assert.NotEmpty(new PythonTokenPattern(Pattern).Matches(string.Concat(Enumerable.Repeat("1 \uD83D\uDE00", 200))));
        var reader = new PythonTokenPattern(Pattern);
        Assert.Equal([0, 0, 0, 0, 0], reader.Matches("1ai\uD83D\uDE00").Select(m => m.Length));
        Assert.True(reader.LastReadInterpreted);
        // Nor its own, once it read a long text compiled: a short one is still interpreted, as on main.
        Assert.NotEmpty(reader.Matches(string.Concat(Enumerable.Repeat("1 \uD83D\uDE00", 200))));
        Assert.False(reader.LastReadInterpreted);
        Assert.Equal([0, 0, 0, 0, 0], reader.Matches("1ai\uD83D\uDE00").Select(m => m.Length));
        Assert.True(reader.LastReadInterpreted);
    }

    [Theory]
    [InlineData(@"[\w\S]")]
    [InlineData(@"(?i)(\w|\w|\S)")]
    [InlineData(@"[\w\d\w]")]
    [InlineData(@"\w|\w")]
    [InlineData(@"(?s)\w|\w|\S")]
    public void A_class_spells_each_supplementary_set_once(string pattern)
    {
        // The word pairs, 5,000 units, written twice or beside any pair, ran a hundred times Python's time (#1672).
        static int Pairs(string p) => PythonPattern.Translate(p).Length - PythonPattern.Translate(p, surrogateFree: true).Length;
        Assert.True(Pairs(pattern) < Pairs(@"\w") + 200, $"{Pairs(pattern)} against {Pairs(@"\w")}");
    }

    [Fact]
    public void A_repeat_of_an_empty_capable_item_is_read_as_the_compiled_engine_reads_it()
    {
        // Python gives 'Ax -²' alone; .NET's interpreter, which loops on (?:a|)+?.+|x, would too: neither engine reads
        // every such pattern as Python does, and docs/equivalence.md says so (#1675).
        Assert.Equal(
            [("", 1), (" \u00B2\uD83D\uDE00", 1), ("Ax -\u00B2", 1)],
            Tokens(@"(?:^[\w-]{1,3})*?(?>\D.|)+?\w[^\s]?|(?<=1)", "Ax -\u00B2 \u00B2\uD83D\uDE00."));
    }

    [Fact]
    public void A_scan_ends_where_the_engine_hands_back_a_match_before_the_search_start()
    {
        // Over "111" .NET's compiled engine returned (0, 1) from start 1, behind it, which 0.7.0 scanned for good.
        var pattern = new PythonTokenPattern(@"(?:(.){0,2}?b+\S*(?=a){0,2}?)*|(a|)");
        Assert.NotEmpty(pattern.Matches("111"));
        Assert.NotEmpty(pattern.MatchesWithSpans("111"));
    }

    [Fact]
    public void Spans_take_the_non_empty_match_python_finds_where_an_empty_one_ended()
    {
        // re.finditer("|a", "a") gives (0, 0), (0, 1), (1, 1): Rake and TextRank read the spans, the vectorizers the tokens.
        var pattern = new PythonTokenPattern("|a");
        Assert.Equal([(0, 0, 0, 0), (0, 1, 0, 1), (1, 0, 1, 1)], pattern.MatchesWithSpans("a"));
        Assert.Equal([(0, 0, 0, 0), (0, 1, 0, 1), (1, 0, 1, 1), (3, 0, 3, 3)], pattern.MatchesWithSpans("a\uD83D\uDE00"));
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
