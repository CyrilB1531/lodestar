using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Lodestar.Text.Internal;

// SonarLint S127: an escape or a surrogate pair advances the loop variable past what it consumed.
#pragma warning disable S127

/// <summary>Compiles a token pattern written for Python's <c>re</c> so .NET reads its classes as Python does (#1239).</summary>
/// <remarks>
/// Measured over every scalar against Python 3.12: <c>\w</c> is exactly <c>L*</c>, <c>N*</c> and
/// <c>_</c>, <c>\d</c> is <c>Nd</c>, <c>\s</c> is .NET's plus U+001C to U+001F. A supplementary
/// character is one, spelled as the surrogate pairs the runtime's tables list; a leading
/// <c>(?u)</c>, Python's default and scikit-learn's spelling, is dropped since .NET refuses it.
/// </remarks>
internal static class PythonPattern
{
    private const string AnyPair = @"[\uD800-\uDBFF][\uDC00-\uDFFF]";

    // Not between the two halves of a pair, where Python has no position: a lone low surrogate
    // must not start a match once the pair it belongs to was passed over.
    private const string NotMidPair = @"(?!(?<=[\uD800-\uDBFF])[\uDC00-\uDFFF])";
    private const string WordBmp = @"\p{L}\p{N}_";
    private const string DigitBmp = @"\p{Nd}";
    private const string SpaceBmp = @"\s\x1C-\x1F";

    private static readonly Lazy<string> WordPairs = new(() => Pairs(IsWordCategory));
    private static readonly Lazy<string> DigitPairs = new(() => Pairs(c => c == UnicodeCategory.DecimalDigitNumber));

    /// <summary>A regular expression that matches as Python's <c>re.compile(pattern)</c> would.</summary>
    public static Regex Compile(string pattern, RegexOptions options, TimeSpan timeout) =>
        new(Translate(pattern), options, timeout);

    /// <summary>The .NET spelling of a Python pattern; see the type's remarks for what changes.</summary>
    public static string Translate(string pattern)
    {
        if (pattern.StartsWith("(?u)", StringComparison.Ordinal))
        {
            pattern = pattern.Substring(4);
        }

        var sb = new StringBuilder(pattern.Length);
        for (int i = 0; i < pattern.Length; i++)
        {
            char c = pattern[i];
            if (c == '\\' && i + 1 < pattern.Length)
            {
                sb.Append(Escape(pattern[i + 1]) ?? pattern.Substring(i, 2));
                i++;
            }
            else if (c == '[')
            {
                int end = ClassEnd(pattern, i);
                if (end < 0)
                {
                    sb.Append(pattern, i, pattern.Length - i);
                    break;
                }
                sb.Append(Class(pattern.Substring(i + 1, end - i - 1)));
                i = end;
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>A class escape outside brackets, or null when .NET already reads it as Python does.</summary>
    private static string? Escape(char escape) => escape switch
    {
        'w' => Positive(WordBmp, WordPairs.Value),
        'W' => Negative(WordBmp, WordPairs.Value),
        'd' => Positive(DigitBmp, DigitPairs.Value),
        'D' => Negative(DigitBmp, DigitPairs.Value),
        's' => $"[{SpaceBmp}]",
        'S' => $"(?:{AnyPair}|{NotMidPair}[^{SpaceBmp}])",
        'b' => $"(?:(?<={Behind()})(?!{Ahead()})|(?<!{Behind()})(?={Ahead()}))",
        'B' => $"(?:(?<={Behind()})(?={Ahead()})|(?<!{Behind()})(?!{Ahead()}))",
        _ => null,
    };

    private static string Ahead() => Positive(WordBmp, WordPairs.Value);

    // Inside a lookbehind .NET matches right to left, so the pairs are listed without the leading
    // lookahead that only speeds the forward match.
    private static string Behind() => $"[{WordBmp}]|{WordPairs.Value}";

    private static string Positive(string bmp, string pairs) => $"(?:[{bmp}]|(?=[\\uD800-\\uDBFF])(?:{pairs}))";

    // A pair that is not in the set is one character, as Python counts it; a lone surrogate is too.
    private static string Negative(string bmp, string pairs) =>
        $"(?:(?!{pairs}){AnyPair}|(?!{AnyPair}){NotMidPair}[^{bmp}])";

    /// <summary>A bracketed class: <c>\w</c>, <c>\d</c> and <c>\s</c> inside it are widened to Python's sets.</summary>
    private static string Class(string body)
    {
        bool negated = body.Length > 0 && body[0] == '^';
        string items = negated ? body.Substring(1) : body;
        var bmp = new StringBuilder(items.Length);
        var pairs = new List<string>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == '\\' && i + 1 < items.Length)
            {
                switch (items[i + 1])
                {
                    case 'w':
                        bmp.Append(WordBmp);
                        pairs.Add(WordPairs.Value);
                        break;
                    case 'd':
                        bmp.Append(DigitBmp);
                        pairs.Add(DigitPairs.Value);
                        break;
                    case 's':
                        bmp.Append(SpaceBmp);
                        break;
                    default:
                        bmp.Append(items, i, 2);
                        break;
                }
                i++;
            }
            else
            {
                bmp.Append(items[i]);
            }
        }

        if (pairs.Count == 0)
        {
            return negated ? $"(?:{AnyPair}|{NotMidPair}[^{bmp}])" : $"[{bmp}]";
        }
        string astral = string.Join("|", pairs);
        return negated
            ? $"(?:(?!{astral}){AnyPair}|(?!{AnyPair}){NotMidPair}[^{bmp}])"
            : $"(?:[{bmp}]|(?=[\\uD800-\\uDBFF])(?:{astral}))";
    }

    /// <summary>The index of the <c>]</c> closing the class opened at <paramref name="open"/>, or -1.</summary>
    private static int ClassEnd(string pattern, int open)
    {
        int i = open + 1;
        if (i < pattern.Length && pattern[i] == '^')
        {
            i++;
        }
        // A ']' first in a class is a literal, in both dialects.
        if (i < pattern.Length && pattern[i] == ']')
        {
            i++;
        }
        for (; i < pattern.Length; i++)
        {
            if (pattern[i] == '\\')
            {
                i++;
            }
            else if (pattern[i] == ']')
            {
                return i;
            }
        }
        return -1;
    }

    private static bool IsWordCategory(UnicodeCategory c) => c is UnicodeCategory.UppercaseLetter
        or UnicodeCategory.LowercaseLetter or UnicodeCategory.TitlecaseLetter or UnicodeCategory.ModifierLetter
        or UnicodeCategory.OtherLetter or UnicodeCategory.DecimalDigitNumber or UnicodeCategory.LetterNumber
        or UnicodeCategory.OtherNumber;

    /// <summary>
    /// Every supplementary scalar of the categories <paramref name="member"/> accepts, as an alternation
    /// of high surrogates, each with the class of low surrogates that complete it — consecutive high
    /// surrogates sharing one class merged into a range.
    /// </summary>
    private static string Pairs(Func<UnicodeCategory, bool> member)
    {
        var groups = new List<(char First, char Last, string Lows)>();
        for (char high = '\uD800'; high <= '\uDBFF'; high++)
        {
            string lows = Lows(high, member);
            if (lows.Length == 0)
            {
                continue;
            }
            if (groups.Count > 0 && groups[groups.Count - 1].Last == high - 1 && groups[groups.Count - 1].Lows == lows)
            {
                groups[groups.Count - 1] = (groups[groups.Count - 1].First, high, lows);
            }
            else
            {
                groups.Add((high, high, lows));
            }
        }

        var sb = new StringBuilder();
        foreach ((char first, char last, string lows) in groups)
        {
            if (sb.Length > 0)
            {
                sb.Append('|');
            }
            sb.Append(first == last ? Unit(first) : $"[{Unit(first)}-{Unit(last)}]").Append('[').Append(lows).Append(']');
        }
        return sb.Length == 0 ? "(?!)" : sb.ToString();
    }

    private static string Lows(char high, Func<UnicodeCategory, bool> member)
    {
        var sb = new StringBuilder();
        int start = -1;
        for (int low = 0xDC00; low <= 0xE000; low++)
        {
            bool inSet = low <= 0xDFFF && member(Category(high, (char)low));
            if (inSet && start < 0)
            {
                start = low;
            }
            else if (!inSet && start >= 0)
            {
                sb.Append(Unit((char)start));
                if (low - 1 > start)
                {
                    sb.Append('-').Append(Unit((char)(low - 1)));
                }
                start = -1;
            }
        }
        return sb.ToString();
    }

    private static UnicodeCategory Category(char high, char low) =>
#if NET5_0_OR_GREATER
        CharUnicodeInfo.GetUnicodeCategory(char.ConvertToUtf32(high, low));
#else
        CharUnicodeInfo.GetUnicodeCategory(new string([high, low]), 0);
#endif

    private static string Unit(char c) => "\\u" + ((int)c).ToString("X4", CultureInfo.InvariantCulture);
}
