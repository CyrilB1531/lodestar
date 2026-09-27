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
/// pattern goes through <see cref="PythonPattern"/>.
/// </remarks>
internal sealed class PythonTokenPattern
{
    private readonly int _minimumRun;
    private readonly Regex? _regex;

    public PythonTokenPattern(string pattern)
    {
        string body = pattern.StartsWith("(?u)", StringComparison.Ordinal) ? pattern.Substring(4) : pattern;
        _minimumRun = body switch
        {
            @"\b\w\w+\b" or @"\w\w+" => 2,
            @"\b\w+\b" or @"\w+" => 1,
            _ => 0,
        };
        if (_minimumRun == 0)
        {
            _regex = PythonPattern.Compile(
                pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
        }
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

        foreach (Match m in _regex.Matches(s))
        {
            matches.Add((m.Index, m.Length));
        }
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
