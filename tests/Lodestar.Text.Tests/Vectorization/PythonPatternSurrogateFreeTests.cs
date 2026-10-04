using System.Text;
using System.Text.RegularExpressions;
using Lodestar.Text.Internal;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus for this
// differential test; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394

/// <summary>
/// On a text without surrogates the plain spelling of a pattern finds what the pair-aware one finds, which is what lets
/// a token pattern take it there (#1645).
/// </summary>
public sealed partial class PythonPatternSurrogateFreeTests
{
    // Letters, digits, marks, Python's extra spaces, punctuation and CJK: every class the translation widens.
    private const string Pool = "ab Z_9\u0301\u0300\u00E9\u0663\u00B2\u2167 \t\n\u001C\u001F.,;-@'\"\u4E2D\u3042\u00A0\u2028x";

    public static TheoryData<string> Patterns => new()
    {
        @"[^ ]+", @"\S+", @"\W+", @"\D+", @"\s+", @"[^\w]+", @"[\w-]+", @"[^\d\s]+", @"(?u)\b[a-z]+\b",
        @"[A-Za-z]\w+", @"(\w+)@", @"\B\w", @"\b\w\w+\b", @"\w", @"[^a-z]", @"\d+(?:\.\d+)?", @"(?<=\s)\S+",
    };

    [Theory]
    [MemberData(nameof(Patterns))]
    public void Both_spellings_find_the_same_matches_in_a_text_without_surrogates(string pattern)
    {
        var plain = PythonPattern.CompileSurrogateFree(pattern, RegexOptions.CultureInvariant, Regex.InfiniteMatchTimeout);
        var pairAware = PythonPattern.Compile(pattern, RegexOptions.CultureInvariant, Regex.InfiniteMatchTimeout);
        var random = new Random(pattern.Length * 7919);
        for (int trial = 0; trial < 300; trial++)
        {
            var text = new StringBuilder();
            int length = random.Next(0, 40);
            for (int i = 0; i < length; i++)
            {
                text.Append(Pool[random.Next(Pool.Length)]);
            }
            string s = text.ToString();
            Assert.Equal(Spans(pairAware, s), Spans(plain, s));
        }
    }

    [Fact]
    public void Whitespace_is_dotnets_and_the_four_separators_on_every_bmp_character()
    {
        // The ranges spell .NET's \s plus U+001C to U+001F, which Python's \s is over every scalar (#1239, #1645).
        var spelled = PythonPattern.CompileSurrogateFree(@"\s", RegexOptions.CultureInvariant, Regex.InfiniteMatchTimeout);
        Regex reference = DotnetSpaceAndSeparators();
        for (int c = 0; c < 0x10000; c++)
        {
            if (c is >= 0xD800 and <= 0xDFFF)
            {
                continue;
            }
            string s = ((char)c).ToString();
            Assert.True(reference.IsMatch(s) == spelled.IsMatch(s), $"U+{c:X4}");
        }
    }

    /// <summary>
    /// Characters on which Python's classes and .NET's agree, with no mark, no number but ASCII digits and nothing from
    /// U+001C to U+001F. The test adds an astral character, so the pair-aware spelling or the raw one reads it.
    /// </summary>
    private const string AgreedPool = "ab Z_9-.`^[]x\u00E9\u4E2D";

    [Theory]
    [InlineData(@"[\s-a]+")]
    [InlineData(@"[\s-~]+")]
    [InlineData(@"[\w-.]+")]
    [InlineData(@"[\w-a]+")]
    [InlineData(@"[^\w-a]+")]
    [InlineData(@"[\w-[0-9]]+")]

    public void A_class_python_refuses_is_read_as_0_7_0_read_it(string pattern)
    {
        // A range touching a class escape, which Python refuses: read as 0.7.0 read each, by .NET as written, on texts
        // with and without a surrogate pair (#1645).
        var token = new PythonTokenPattern(pattern);
        var raw = new Regex(pattern, RegexOptions.CultureInvariant);
        var random = new Random(pattern.Length * 104729);
        for (int trial = 0; trial < 300; trial++)
        {
            var text = new StringBuilder();
            int length = random.Next(0, 30);
            for (int i = 0; i < length; i++)
            {
                text.Append(random.Next(8) == 0 ? "\U0001F600" : AgreedPool[random.Next(AgreedPool.Length)].ToString());
            }
            string s = text.ToString();
            Assert.Equal(raw.Matches(s).Select(m => (m.Index, m.Length)), token.Matches(s));
        }
    }

    [Theory]
    [InlineData(@"[a-z-\w]+", @"[-a-z\w]+")]
    [InlineData(@"[0-9-\s]+", @"[-0-9\s]+")]
    [InlineData(@"[a-c-\d]+", @"[-a-c\d]+")]
    [InlineData(@"[^a-z-[aeiou]]+", @"[^\-a-z\[aeiou]\]+")]
    [InlineData(@"[a\-\w]+", @"[-a\w]+")]
    [InlineData(@"[+--\w]+", @"[+,\-\w]+")]
    [InlineData(@"[\--/]+", @"[\-./]+")]
    [InlineData(@"[\x2D-/]+", @"[\-./]+")]
    [InlineData(@"[\W]+", @"\W+")]
    [InlineData(@"[^\S]+", @"\s+")]
    [InlineData(@"[\D]+", @"\D+")]
    [InlineData(@"[^\W\d]+", @"(?:(?!\d)\w)+")]
    public void A_class_is_read_as_python_reads_it(string pattern, string equivalent)
    {
        // Read as Python reads the class: a '-' after a range a literal, an escaped '-' a range's start, '[' no subtraction,
        // and \w, \W, \d, \D, \s, \S Python's sets, marks and U+203F outside \w and U+001C inside \s (#1645).
        var token = new PythonTokenPattern(pattern);
        var reference = new PythonTokenPattern(equivalent);
        var random = new Random(pattern.Length * 15485863);
        for (int trial = 0; trial < 300; trial++)
        {
            var text = new StringBuilder();
            int length = random.Next(0, 30);
            for (int i = 0; i < length; i++)
            {
                text.Append(random.Next(8) switch
                {
                    0 => "\U0001F600",
                    1 => "\U0001D400",
                    _ => (Pool[random.Next(Pool.Length)] + "-\u203F")[random.Next(3)].ToString(),
                });
            }
            string s = text.ToString();
            Assert.Equal(reference.Matches(s), token.Matches(s));
        }
    }

    [Theory]
    [InlineData(@"\w(")]
    [InlineData(@"[a-\w]")]
    [InlineData(@"[0-\w]+")]
    [InlineData(@"[.-\w]")]
    [InlineData(@"[_-\w]")]
    [InlineData(@"[0-\d]")]
    [InlineData(@"[\p{L}--\w]+")]
    [InlineData(@"[\---\w]+")]
    [InlineData(@"[\_]+")]
    [InlineData(@"[\U0001F600]")]
    [InlineData(@"[\x00-\s]")]
    public void A_pattern_dotnet_refuses_is_refused_with_0_7_0s_exception(string pattern)
    {
        // Refused when built, with the exception and message .NET gave the pattern as written in 0.7.0, Python reading
        // some of them, [\---\w] and [\_] among them (#1645).
        RegexParseException expected = Assert.Throws<RegexParseException>(() => new Regex(pattern, RegexOptions.CultureInvariant));
        RegexParseException actual = Assert.Throws<RegexParseException>(() => new CountVectorizer(new CountVectorizerOptions { TokenPattern = pattern }));
        Assert.Equal(expected.Message, actual.Message);
    }

    [GeneratedRegex(@"[\s\x1C-\x1F]", RegexOptions.CultureInvariant)]
    private static partial Regex DotnetSpaceAndSeparators();

    private static List<(int, int, string)> Spans(Regex regex, string s) =>
        [.. regex.Matches(s).Select(m => (m.Index, m.Length, string.Join("|", m.Groups.Values.Select(g => g.Success + ":" + g.Index + ":" + g.Length))))];
}
