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

    // As ranges for \s and \S outside brackets, a class .NET searches vectorised: [^\s\x1C-\x1F] made \S+ half again
    // slower than 0.7.0's (#1645).
    private const string SpaceRanges = @"\t-\r\x1C-\x20\x85\xA0\u1680\u2000-\u200A\u2028\u2029\u202F\u205F\u3000";

    private static readonly Lazy<string> WordPairs = new(() => Pairs(IsWordCategory));
    private static readonly Lazy<string> DigitPairs = new(() => Pairs(c => c == UnicodeCategory.DecimalDigitNumber));

    // The complements a class holds for \W, \D and \S, BMP units outside the set, surrogates apart: .NET's own \W, \D and
    // \S read marks, connector punctuation and U+001C to U+001F otherwise than Python (#1645).
    private static readonly Lazy<string> NonWordBmp = new(() => BmpRanges(c => c != '_' && !IsWordCategory(CharUnicodeInfo.GetUnicodeCategory(c))));
    private static readonly Lazy<string> NonDigitBmp = new(() => BmpRanges(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.DecimalDigitNumber));
    private static readonly Lazy<string> NonSpaceBmp = new(() => BmpRanges(c => !IsPythonSpace(c)));
    private static readonly Lazy<string> NonWordPairs = new(() => $"(?!{WordPairs.Value}){AnyPair}");
    private static readonly Lazy<string> NonDigitPairs = new(() => $"(?!{DigitPairs.Value}){AnyPair}");
    private static readonly Lazy<string> EveryPair = new(() => AnyPair);

    /// <summary>A regular expression that matches as Python's <c>re.compile(pattern)</c> would.</summary>
    public static Regex Compile(string pattern, RegexOptions options, TimeSpan timeout) =>
        new(Translate(pattern), options, timeout);

    /// <summary>
    /// <see cref="Compile"/> for a text that holds no surrogate, where a code point and a UTF-16 unit are one: each
    /// class stays a plain class .NET scans vectorised, five to eight times faster than the pair-aware one (#1645).
    /// </summary>
    public static Regex CompileSurrogateFree(string pattern, RegexOptions options, TimeSpan timeout) =>
        new(Translate(pattern, surrogateFree: true), options, timeout);

    /// <summary>The .NET spelling of a Python pattern; see the type's remarks for what changes.</summary>
    /// <param name="pattern">The Python pattern.</param>
    /// <param name="surrogateFree">
    /// Spelled for a text without a surrogate: every branch that matches a pair or guards against splitting one is
    /// dropped, which matches nothing such a text holds, so the two spellings agree on it.
    /// </param>
    public static string Translate(string pattern, bool surrogateFree = false)
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
                sb.Append(Escape(pattern[i + 1], surrogateFree) ?? pattern.Substring(i, 2));
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
                sb.Append(Class(pattern.Substring(i + 1, end - i - 1), surrogateFree));
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
    private static string? Escape(char escape, bool surrogateFree) => escape switch
    {
        'w' => Positive(WordBmp, WordPairs, surrogateFree),
        'W' => Negative(WordBmp, WordPairs, surrogateFree),
        'd' => Positive(DigitBmp, DigitPairs, surrogateFree),
        'D' => Negative(DigitBmp, DigitPairs, surrogateFree),
        's' => $"[{SpaceRanges}]",
        'S' => surrogateFree ? $"[^{SpaceRanges}]" : $"(?:{AnyPair}|{NotMidPair}[^{SpaceRanges}])",
        'b' => $"(?:(?<={Behind(surrogateFree)})(?!{Ahead(surrogateFree)})|(?<!{Behind(surrogateFree)})(?={Ahead(surrogateFree)}))",
        'B' => $"(?:(?<={Behind(surrogateFree)})(?={Ahead(surrogateFree)})|(?<!{Behind(surrogateFree)})(?!{Ahead(surrogateFree)}))",
        _ => null,
    };

    private static string Ahead(bool surrogateFree) => Positive(WordBmp, WordPairs, surrogateFree);

    // Inside a lookbehind .NET matches right to left, so the pairs are listed without the leading
    // lookahead that only speeds the forward match.
    private static string Behind(bool surrogateFree) => surrogateFree ? $"[{WordBmp}]" : $"[{WordBmp}]|{WordPairs.Value}";

    private static string Positive(string bmp, Lazy<string> pairs, bool surrogateFree) =>
        surrogateFree ? $"[{bmp}]" : $"(?:[{bmp}]|(?=[\\uD800-\\uDBFF])(?:{pairs.Value}))";

    // A pair that is not in the set is one character, as Python counts it; a lone surrogate is too.
    private static string Negative(string bmp, Lazy<string> pairs, bool surrogateFree) =>
        surrogateFree ? $"[^{bmp}]" : $"(?:(?!{pairs.Value}){AnyPair}|(?!{AnyPair}){NotMidPair}[^{bmp}])";

    /// <summary>A bracketed class, read item by item as Python's <c>re</c> reads one, and spelled so .NET reads it alike.</summary>
    /// <remarks>
    /// Every literal is written as a <c>\uXXXX</c> escape and every range as one, so no '-' or '[' is left for .NET to read
    /// as a range or a subtraction Python never made: <c>[a-z-[aeiou]]</c> is a class and a literal <c>]</c>, as in
    /// Python. <c>\w</c>, <c>\d</c> and <c>\s</c> are widened to Python's sets. A class Python refuses — a range touching a
    /// class escape, a reversed range, an unknown escape — is refused here, and the token pattern reads it as 0.7.0 did,
    /// by .NET as written (#1645).
    /// </remarks>
    /// <exception cref="ArgumentException">Python's <c>re</c> refuses the class.</exception>
    private static string Class(string body, bool surrogateFree)
    {
        bool negated = body.Length > 0 && body[0] == '^';
        string items = negated ? body.Substring(1) : body;
        var bmp = new StringBuilder(items.Length * 6);
        var pairs = new List<Lazy<string>>();
        bool loneSurrogates = false;
        int i = 0;
        while (i < items.Length)
        {
            ClassItem first = ReadClassItem(items, ref i);
            // A '-' last in the class is a literal, as is one after a range: the loop reads it as an item of its own.
            if (i + 1 < items.Length && items[i] == '-')
            {
                i++;
                AppendRange(bmp, first, ReadClassItem(items, ref i));
            }
            else
            {
                AppendItem(bmp, pairs, ref loneSurrogates, first);
            }
        }

        return Spell(negated, bmp.ToString(), pairs, loneSurrogates, surrogateFree);
    }

    /// <summary>One item of a class: a class escape (<see cref="ClassItem.ClassEscape"/> set), or one code point.</summary>
    private readonly struct ClassItem
    {
        public ClassItem(char escape, int codePoint)
        {
            ClassEscape = escape;
            CodePoint = codePoint;
        }

        public char ClassEscape { get; }

        public int CodePoint { get; }
    }

    /// <summary>Reads the item at <paramref name="i"/> as Python's <c>_class_escape</c> does, and moves past it.</summary>
    private static ClassItem ReadClassItem(string items, ref int i)
    {
        char c = items[i];
        if (c != '\\')
        {
            return new ClassItem('\0', ReadCodePoint(items, ref i));
        }
        if (i + 1 >= items.Length)
        {
            throw NotPython();
        }

        char e = items[i + 1];
        i += 2;
        switch (e)
        {
            case 'w' or 'W' or 'd' or 'D' or 's' or 'S':
                return new ClassItem(e, 0);
            case 'a':
                return new ClassItem('\0', 7);
            case 'b':
                return new ClassItem('\0', 8);
            case 'f':
                return new ClassItem('\0', 12);
            case 'n':
                return new ClassItem('\0', 10);
            case 'r':
                return new ClassItem('\0', 13);
            case 't':
                return new ClassItem('\0', 9);
            case 'v':
                return new ClassItem('\0', 11);
            case 'x':
                return new ClassItem('\0', Hex(items, ref i, 2));
            case 'u':
                return new ClassItem('\0', Hex(items, ref i, 4));
            case 'U':
                return new ClassItem('\0', Hex(items, ref i, 8));
            case >= '0' and <= '7':
                return new ClassItem('\0', Octal(items, ref i, e));
            case >= '0' and <= '9' or >= 'a' and <= 'z' or >= 'A' and <= 'Z':
                // Python's "bad escape", and \N{...}, which Python reads and .NET refuses as 0.7.0 did: read raw.
                throw NotPython();
            default:
                i--;
                return new ClassItem('\0', ReadCodePoint(items, ref i));
        }
    }

    /// <summary>The code point at <paramref name="i"/>, a surrogate pair being one as Python counts it, and moves past it.</summary>
    private static int ReadCodePoint(string s, ref int i)
    {
        if (i + 1 < s.Length && char.IsHighSurrogate(s[i]) && char.IsLowSurrogate(s[i + 1]))
        {
            i += 2;
            return char.ConvertToUtf32(s[i - 2], s[i - 1]);
        }
        return s[i++];
    }

    /// <summary>Exactly <paramref name="digits"/> hexadecimal digits, as Python requires them, a scalar at most.</summary>
    private static int Hex(string items, ref int i, int digits)
    {
        if (i + digits > items.Length)
        {
            throw NotPython();
        }

        long value = 0;
        for (int k = 0; k < digits; k++)
        {
            int digit = HexDigit(items[i + k]);
            if (digit < 0)
            {
                throw NotPython();
            }
            value = (value * 16) + digit;
        }
        i += digits;
        return value <= 0x10FFFF ? (int)value : throw NotPython();
    }

    private static int HexDigit(char c) => c switch
    {
        >= '0' and <= '9' => c - '0',
        >= 'a' and <= 'f' => c - 'a' + 10,
        >= 'A' and <= 'F' => c - 'A' + 10,
        _ => -1,
    };

    /// <summary>Up to three octal digits, the first already read, at most 0o377, as Python reads them in a class.</summary>
    private static int Octal(string items, ref int i, char first)
    {
        int value = first - '0';
        for (int k = 0; k < 2 && i < items.Length && items[i] is >= '0' and <= '7'; k++)
        {
            value = (value * 8) + (items[i] - '0');
            i++;
        }
        return value <= 255 ? value : throw NotPython();
    }

    /// <summary>A range of two code points, as Python accepts one: neither a class escape, nor reversed.</summary>
    private static void AppendRange(StringBuilder bmp, ClassItem first, ClassItem last)
    {
        if (first.ClassEscape != '\0' || last.ClassEscape != '\0' || first.CodePoint > last.CodePoint)
        {
            throw NotPython();
        }
        if (last.CodePoint > 0xFFFF)
        {
            // A range into the supplementary planes has no class of units to stand for it; read as main read it, raw.
            throw NotPython();
        }
        bmp.Append(Unit(first.CodePoint)).Append('-').Append(Unit(last.CodePoint));
    }

    private static void AppendItem(StringBuilder bmp, List<Lazy<string>> pairs, ref bool loneSurrogates, ClassItem item)
    {
        switch (item.ClassEscape)
        {
            case 'w':
                bmp.Append(WordBmp);
                pairs.Add(WordPairs);
                break;
            case 'd':
                bmp.Append(DigitBmp);
                pairs.Add(DigitPairs);
                break;
            case 's':
                bmp.Append(SpaceBmp);
                break;
            case 'W':
                bmp.Append(NonWordBmp.Value);
                pairs.Add(NonWordPairs);
                loneSurrogates = true;
                break;
            case 'D':
                bmp.Append(NonDigitBmp.Value);
                pairs.Add(NonDigitPairs);
                loneSurrogates = true;
                break;
            case 'S':
                bmp.Append(NonSpaceBmp.Value);
                pairs.Add(EveryPair);
                loneSurrogates = true;
                break;
            default:
                if (item.CodePoint <= 0xFFFF)
                {
                    bmp.Append(Unit(item.CodePoint));
                }
                else
                {
                    string pair = char.ConvertFromUtf32(item.CodePoint);
                    string spelled = Unit(pair[0]) + Unit(pair[1]);
                    pairs.Add(new Lazy<string>(() => spelled));
                }
                break;
        }
    }

    private static ArgumentException NotPython() => new("The token pattern holds a class Python's re refuses.");

    /// <summary>A class of <paramref name="bmp"/> and the supplementary sets <paramref name="pairs"/> spell, as one code point.</summary>
    /// <param name="negated">Whether the class is negated.</param>
    /// <param name="bmp">The BMP units the class holds, surrogates apart.</param>
    /// <param name="pairs">Alternatives matching the supplementary characters it holds.</param>
    /// <param name="loneSurrogates">Whether it holds every lone surrogate too, as a complement does: each is a code point.</param>
    /// <param name="surrogateFree">Spelled for a text without a surrogate.</param>
    private static string Spell(bool negated, string bmp, List<Lazy<string>> pairs, bool loneSurrogates, bool surrogateFree)
    {
        // A class of supplementary characters alone has no BMP part: nothing for it to match there, everything for its
        // complement.
        string? positive = bmp.Length == 0 ? null : $"[{bmp}]";
        string excluded = loneSurrogates ? bmp + @"\uD800-\uDFFF" : bmp;
        string negative = excluded.Length == 0 ? @"[\u0000-\uFFFF]" : $"[^{excluded}]";
        if (surrogateFree)
        {
            return negated ? negative : positive ?? "(?!)";
        }

        string lone = loneSurrogates ? $"|(?!{AnyPair}){NotMidPair}[\\uD800-\\uDFFF]" : "";
        if (pairs.Count == 0)
        {
            return negated ? $"(?:{AnyPair}|{NotMidPair}{negative})" : positive ?? "(?!)";
        }
        string astral = string.Join("|", pairs.Select(p => p.Value));
        if (negated)
        {
            return $"(?:(?!{astral}){AnyPair}|(?!{AnyPair}){NotMidPair}{negative})";
        }
        string supplementary = $"(?=[\\uD800-\\uDBFF])(?:{astral})";
        return positive is null ? $"(?:{supplementary}{lone})" : $"(?:{positive}|{supplementary}{lone})";
    }

    /// <summary>The BMP units <paramref name="member"/> accepts, surrogates apart, as a class body of ranges.</summary>
    private static string BmpRanges(Func<char, bool> member)
    {
        var sb = new StringBuilder();
        int start = -1;
        for (int c = 0; c <= 0x10000; c++)
        {
            bool inSet = c <= 0xFFFF && c is < 0xD800 or > 0xDFFF && member((char)c);
            if (inSet && start < 0)
            {
                start = c;
            }
            else if (!inSet && start >= 0)
            {
                sb.Append(Unit(start));
                if (c - 1 > start)
                {
                    sb.Append('-').Append(Unit(c - 1));
                }
                start = -1;
            }
        }
        return sb.ToString();
    }

    /// <summary>Python's <c>\s</c>: .NET's and U+001C to U+001F, the set <see cref="SpaceRanges"/> spells.</summary>
    private static bool IsPythonSpace(char c) =>
        c is >= '\t' and <= '\r' or >= '\x1C' and <= ' ' or '\x85' or '\xA0' or '\u1680' or >= '\u2000' and <= '\u200A'
            or '\u2028' or '\u2029' or '\u202F' or '\u205F' or '\u3000';

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

    private static string Unit(int c) => "\\u" + c.ToString("X4", CultureInfo.InvariantCulture);
}
