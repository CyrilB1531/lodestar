using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lodestar.Text.Internal;

// SonarLint S127: a surrogate pair advances the loop variable past its low half.
#pragma warning disable S127

/// <summary>A token pattern read as Python's <c>re</c> reads it, scanned to (start, length) matches.</summary>
/// <remarks>
/// The default patterns, <c>\b\w\w+\b</c> and <c>\b\w+\b</c> with or without <c>\b</c> and
/// <c>(?u)</c>, mean every maximal run of word characters of at least two or one code points, and
/// are scanned by hand: the translated regex costs ten times the .NET one (#1239). Any other
/// pattern goes through <see cref="PythonPattern"/>, and yields what <c>re.findall</c> yields: the
/// whole match, or the one capturing group's text when the pattern has one (#1262).
/// </remarks>
internal sealed class PythonTokenPattern
{
    private readonly int _minimumRun;
    private readonly int _group;

    // The pattern spelled for a text without surrogates, and as Python reads any text, compiled when a text first holds
    // one: the second's pair-aware classes scan five to eight times slower than plain ones (#1645).
    private readonly Regex? _regex;
    private readonly Lazy<Regex>? _pairAware;

    public PythonTokenPattern(string pattern)
    {
        // First, as 0.7.0's Regex refused a null or a pattern it cannot read before anything else here (#1645).
        string body = RefuseAsWritten(pattern);
        _minimumRun = body switch
        {
            @"\b\w\w+\b" or @"\w\w+" => 2,
            @"\b\w+\b" or @"\w+" => 1,
            _ => 0,
        };
        if (_minimumRun == 0)
        {
            (_regex, _pairAware) = Compile(pattern, body);
            // Group 0 is the match itself. scikit-learn's build_tokenizer refuses a second group,
            // since findall would return tuples of them.
            int[] groups = _regex.GetGroupNumbers();
            if (groups.Length > 2)
            {
                throw new ArgumentException(
                    $"The token pattern '{pattern}' has {groups.Length - 1} capturing groups; at most one may capture the token.");
            }
            _group = groups.Length == 2 ? groups[1] : 0;
        }
    }

    /// <summary>The plain spelling, compiled, and the pair-aware one, compiled when a text first holds a surrogate.</summary>
    /// <remarks>
    /// The pattern as written is checked first, as 0.7.0 compiled it, a leading <c>(?u)</c> aside: one .NET refuses goes
    /// out with 0.7.0's exception and message, whether or not Python reads it. Both spellings are then parsed here, so
    /// none fails at the first text holding an emoji; one whose class Python refuses, <c>[\w-.]</c>, is read as 0.7.0
    /// read it, by .NET as written (#1645).
    /// </remarks>
    private static (Regex Plain, Lazy<Regex> PairAware) Compile(string pattern, string written)
    {
        const RegexOptions Options = RegexOptions.Compiled | RegexOptions.CultureInvariant;
        try
        {
            Regex plain = PythonPattern.CompileSurrogateFree(pattern, Options, RegexDefaults.MatchTimeout);
            string pairAware = PythonPattern.Translate(pattern);
            _ = new Regex(pairAware, RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
            return (plain, new Lazy<Regex>(() => new Regex(pairAware, Options, RegexDefaults.MatchTimeout)));
        }
        catch (ArgumentException)
        {
            var raw = new Regex(written, Options, RegexDefaults.MatchTimeout);
            return (raw, new Lazy<Regex>(() => raw));
        }
    }

    /// <summary>
    /// Refuses what 0.7.0 refused when it compiled the pattern as written: a null, and a pattern .NET refuses, with .NET's
    /// own exceptions. A leading <c>(?u)</c>, Python's default, is accepted and dropped (#1239, #1645).
    /// </summary>
    /// <returns>The pattern without its <c>(?u)</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null, its parameter named <c>pattern</c>, as Regex names it.</exception>
    /// <exception cref="ArgumentException">.NET refuses the pattern as written: its <c>RegexParseException</c> where it has one.</exception>
    public static string RefuseAsWritten(string pattern)
    {
        string written = pattern?.StartsWith("(?u)", StringComparison.Ordinal) == true ? pattern.Substring(4) : pattern!;
        _ = new Regex(written, RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
        return written;
    }

    /// <summary>The matches in <paramref name="s"/>, in order, as UTF-16 start and length.</summary>
    public List<(int Start, int Length)> Matches(string s)
    {
        var matches = new List<(int Start, int Length)>();
        Matches(s, matches);
        return matches;
    }

    /// <summary>Replaces the content of <paramref name="matches"/> with the matches in <paramref name="s"/>.</summary>
    public void Matches(string s, List<(int Start, int Length)> matches)
    {
        matches.Clear();
        if (_regex is null)
        {
            ScanRuns(s, _minimumRun, matches);
            return;
        }

        Regex regex = RegexFor(s);
#if NET7_0_OR_GREATER
        if (_group == 0)
        {
            // No group to read, so no Match per token: 0.7.0's scan, which a short-word text ran six times faster (#1645).
            foreach (ValueMatch m in regex.EnumerateMatches(s))
            {
                matches.Add((m.Index, m.Length));
            }
            return;
        }
#endif
        foreach (Match m in regex.Matches(s))
        {
            matches.Add(Token(m));
        }
    }

    /// <summary>Each match's token, as <see cref="Matches(string)"/> gives it, with the span the whole match covers.</summary>
    /// <remarks>
    /// The two differ only under a capturing group; the characters the rest of the match consumed
    /// belong to the token's match and are not a gap between two tokens.
    /// </remarks>
    public List<(int Start, int Length, int MatchStart, int MatchEnd)> MatchesWithSpans(string s)
    {
        var spans = new List<(int Start, int Length, int MatchStart, int MatchEnd)>();
        if (_regex is null)
        {
            foreach ((int start, int length) in Matches(s))
            {
                spans.Add((start, length, start, start + length));
            }
            return spans;
        }

        foreach (Match m in RegexFor(s).Matches(s))
        {
            (int start, int length) = Token(m);
            spans.Add((start, length, m.Index, m.Index + m.Length));
        }
        return spans;
    }

#if NET7_0_OR_GREATER
    /// <summary>The regex to scan <paramref name="s"/> with, when the pattern has one and no group to read a token from.</summary>
    public bool TryGetGrouplessRegex(string s, [NotNullWhen(true)] out Regex? regex)
    {
        regex = _regex is not null && _group == 0 ? RegexFor(s) : null;
        return regex is not null;
    }
#endif

    /// <summary>The spelling that reads <paramref name="s"/> as Python does: the plain one unless it holds a surrogate.</summary>
    private Regex RegexFor(string s) => HoldsSurrogate(s) ? _pairAware!.Value : _regex!;

    private static bool HoldsSurrogate(string s)
    {
#if NET8_0_OR_GREATER
        return s.AsSpan().ContainsAnyInRange('\uD800', '\uDFFF');
#else
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsSurrogate(s[i]))
            {
                return true;
            }
        }
        return false;
#endif
    }

    // re.findall's item: the match without a group, the group's last capture with one, and the
    // empty string where the group took no part in the match.
    private (int Start, int Length) Token(Match m)
    {
        if (_group == 0)
        {
            return (m.Index, m.Length);
        }
        Group g = m.Groups[_group];
        return g.Success ? (g.Index, g.Length) : (m.Index, 0);
    }

    /// <summary>Every maximal run of word characters at least <paramref name="minimum"/> code points long.</summary>
    private static void ScanRuns(string s, int minimum, List<(int Start, int Length)> matches)
    {
        int i = 0;
        while (i < s.Length)
        {
            int width = WordWidth(s, i);
            if (width == 0)
            {
                i += char.IsSurrogatePair(s, i) ? 2 : 1;
                continue;
            }

            int start = i;
            int run = 0;
            while (width > 0)
            {
                i += width;
                run++;
                width = i < s.Length ? WordWidth(s, i) : 0;
            }
            if (run >= minimum)
            {
                matches.Add((start, i - start));
            }
        }
    }

    /// <summary>The UTF-16 width of the word character at <paramref name="i"/>, or 0 when it is none.</summary>
    private static int WordWidth(string s, int i)
    {
        char c = s[i];
        if (c < 0x80)
        {
            return c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9') or '_' ? 1 : 0;
        }
        if (char.IsSurrogatePair(s, i))
        {
            return IsWord(CharUnicodeInfo.GetUnicodeCategory(s, i)) ? 2 : 0;
        }
        return IsWord(CharUnicodeInfo.GetUnicodeCategory(c)) ? 1 : 0;
    }

    // Python's \w over every scalar: a letter or a number of any kind (underscore is ASCII).
    private static bool IsWord(UnicodeCategory c) => c is UnicodeCategory.UppercaseLetter
        or UnicodeCategory.LowercaseLetter or UnicodeCategory.TitlecaseLetter or UnicodeCategory.ModifierLetter
        or UnicodeCategory.OtherLetter or UnicodeCategory.DecimalDigitNumber or UnicodeCategory.LetterNumber
        or UnicodeCategory.OtherNumber;
}
