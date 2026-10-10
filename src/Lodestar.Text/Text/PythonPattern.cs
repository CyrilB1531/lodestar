using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Lodestar.Text.Internal;

// SonarLint S127: an escape or a surrogate pair advances the loop variable past what it consumed.
#pragma warning disable S127

/// <summary>Spells a token pattern written for Python's <c>re</c> so .NET reads its classes as Python does (#1239).</summary>
/// <remarks>
/// Measured over every scalar against Python 3.12: <c>\w</c> is exactly <c>L*</c>, <c>N*</c> and
/// <c>_</c>, <c>\d</c> is <c>Nd</c>, <c>\s</c> is .NET's plus U+001C to U+001F. A supplementary
/// character is one, spelled as the surrogate pairs the runtime's tables list; a leading
/// <c>(?u)</c>, Python's default and scikit-learn's spelling, is dropped since .NET refuses it.
/// </remarks>
internal static partial class PythonPattern
{
    private const string AnyPair = @"[\uD800-\uDBFF][\uDC00-\uDFFF]";

    // Not between the two halves of a pair, where Python has no position: a lone low surrogate
    // must not start a match once the pair it belongs to was passed over.
    private const string NotMidPair = @"(?!(?<=[\uD800-\uDBFF])[\uDC00-\uDFFF])";

    // Python's '.': a pair, or a unit that starts none and is no pair's low half. Its branches take disjoint units, so
    // none gives a pair up; atomic, .NET's compiled engine mis-read a lazy bounded repeat of it, .{1,3}? (#1665).
    private const string AnyCodePoint = $"(?:{AnyPair}|(?!{AnyPair}){NotMidPair}.)";
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

    /// <summary>The .NET spelling of a Python pattern; see the type's remarks for what changes.</summary>
    /// <param name="pattern">The Python pattern.</param>
    /// <param name="surrogateFree">
    /// Spelled for a text without a surrogate: every branch that matches a pair or guards against splitting one is
    /// dropped, which matches nothing such a text holds, so the two spellings agree on it, and each class stays a plain
    /// class .NET scans vectorised, five to eight times faster than the pair-aware one (#1645).
    /// </param>
    public static string Translate(string pattern, bool surrogateFree = false) =>
        Translate(pattern, surrogateFree, out _, out _, out _);

    /// <summary>
    /// <see cref="Translate(string, bool)"/>, whether the pattern can match the empty string, whether a repeat in it
    /// binds an item that can, and whether a lazy repeat with a bound binds a capturing group.
    /// </summary>
    public static string Translate(
        string pattern, bool surrogateFree, out bool mayMatchEmpty, out bool repeatsEmpty, out bool capturesLazily)
    {
        Translator translated = TranslateOnStack(pattern, surrogateFree ? Spelling.SurrogateFree : Spelling.PairAware);
        mayMatchEmpty = translated.MayMatchEmpty;
        repeatsEmpty = translated.RepeatsEmpty;
        capturesLazily = translated.CapturesLazily;
        return translated.Result!;
    }

    // Sixteen times the stack the deepest translation takes.
    private const int TranslationStack = 16 << 20;

    private enum Spelling
    {
        SurrogateFree,
        PairAware,
    }

    /// <summary>The pattern without a leading <c>(?u)</c>, Python's default, which every reading of it drops (#1239).</summary>
    public static string WithoutDefaultFlag(string pattern) =>
        pattern.StartsWith("(?u)", StringComparison.Ordinal) ? pattern.Substring(4) : pattern;

    private static Translator TranslateOnStack(string pattern, Spelling spelling)
    {
        pattern = WithoutDefaultFlag(pattern);

        try
        {
            return Translated(pattern, spelling);
        }
        catch (InsufficientExecutionStackException)
        {
            // The translation does not depend on the thread that asks: MaxFrames levels take about 1 MB (#1667).
            Translator? spelled = null;
            ExceptionDispatchInfo? failed = null;
            var thread = new Thread(
                () =>
                {
                    try
                    {
                        spelled = Translated(pattern, spelling);
                    }
                    // CA1031: any failure goes back to the caller's thread, where an unhandled one would end the process.
#pragma warning disable CA1031
                    catch (Exception e)
#pragma warning restore CA1031
                    {
                        failed = ExceptionDispatchInfo.Capture(e);
                    }
                },
                TranslationStack);
            try
            {
                thread.Start();
            }
            catch (Exception e) when (e is OutOfMemoryException or PlatformNotSupportedException)
            {
                // No thread to translate on: read as written, as 0.7.0 did.
                throw NotPython();
            }
            thread.Join();
            failed?.Throw();
            return spelled!;
        }
    }

    private static Translator Translated(string pattern, Spelling spelling)
    {
        var translator = new Translator(pattern, spelling);
        translator.Result = translator.Run();
        return translator;
    }

    /// <summary>A class escape outside brackets, or null when .NET already reads it as Python does.</summary>
    private static string? Escape(char escape, Spelling spelling) => escape switch
    {
        'w' => Positive(WordBmp, WordPairs, spelling),
        'W' => Negative(WordBmp, WordPairs, spelling),
        'd' => Positive(DigitBmp, DigitPairs, spelling),
        'D' => Negative(DigitBmp, DigitPairs, spelling),
        's' => $"[{SpaceRanges}]",
        'S' => spelling == Spelling.SurrogateFree
            ? $"[^{SpaceRanges}]"
            : $"(?:{AnyPair}|(?!{AnyPair}){NotMidPair}[^{SpaceRanges}])",
        'b' => $"(?:(?<={Behind(spelling)})(?!{Ahead(spelling)})|(?<!{Behind(spelling)})(?={Ahead(spelling)}))",
        // Python's \B never matches in an empty string, where .NET's does (#1669).
        'B' => $"(?!\\A\\z)(?:(?<={Behind(spelling)})(?={Ahead(spelling)})|(?<!{Behind(spelling)})(?!{Ahead(spelling)}))",
        _ => null,
    };

    private static string Ahead(Spelling spelling) => Positive(WordBmp, WordPairs, spelling);

    // Inside a lookbehind .NET matches right to left, so the pairs are listed without the leading
    // lookahead that only speeds the forward match.
    private static string Behind(Spelling spelling) =>
        spelling == Spelling.SurrogateFree ? $"[{WordBmp}]" : $"[{WordBmp}]|{WordPairs.Value}";

    private static string Positive(string bmp, Lazy<string> pairs, Spelling spelling) => spelling == Spelling.SurrogateFree
        ? $"[{bmp}]"
        : $"(?:[{bmp}]|(?=[\\uD800-\\uDBFF])(?:{pairs.Value}))";

    // A pair that is not in the set is one character, as Python counts it; a lone surrogate is too.
    private static string Negative(string bmp, Lazy<string> pairs, Spelling spelling) => spelling == Spelling.SurrogateFree
        ? $"[^{bmp}]"
        : $"(?:(?!{pairs.Value}){AnyPair}|(?!{AnyPair}){NotMidPair}[^{bmp}])";

    /// <summary>A bracketed class, read item by item as Python's <c>re</c> reads one, and spelled so .NET reads it alike.</summary>
    /// <remarks>
    /// Every literal is written as a <c>\uXXXX</c> escape and every range as one, so no '-' or '[' is left for .NET to read
    /// as a range or a subtraction Python never made: <c>[a-z-[aeiou]]</c> is a class and a literal <c>]</c>, as in
    /// Python. <c>\w</c>, <c>\d</c> and <c>\s</c> are widened to Python's sets. A class Python refuses — a range touching a
    /// class escape, a reversed range, an unknown escape — is refused here, and the token pattern reads it as 0.7.0 did,
    /// by .NET as written (#1645).
    /// </remarks>
    /// <exception cref="ArgumentException">Python's <c>re</c> refuses the class.</exception>
    private static string Class(string body, Spelling spelling, bool ignoreCase, out string? unitClass)
    {
        bool negated = body.Length > 0 && body[0] == '^';
        ClassParts parts = Parts([new SetItem(negated ? body.Substring(1) : body, 0)], ignoreCase);

        // A negated class that excludes no surrogate and no pair takes both halves of every pair as units.
        unitClass = negated && parts.Astral.Count == 0 && !parts.AllLone && parts.Lone.Length == 0 ? NegatedUnits(parts) : null;
        return Spell(negated, parts, spelling);
    }

    /// <summary>
    /// One class holding what each of <paramref name="items"/> holds, none negated: each read on its own, so a '-' or an
    /// escape closing one never runs into the next, as Python's parser joins the charsets of single-item branches.
    /// </summary>
    private static string Union(List<SetItem> items, Spelling spelling, bool ignoreCase) =>
        Spell(negated: false, Parts(items, ignoreCase), spelling);

    // What the items hold, each read on its own, and under IGNORECASE what each code point they name is folded with.
    private static ClassParts Parts(List<SetItem> items, bool ignoreCase)
    {
        var parts = new ClassParts(items.Sum(i => i.Body?.Length ?? 2) * 6);
        List<(int First, int Last)>? named = ignoreCase ? [] : null;
        foreach (SetItem item in items)
        {
            if (item.Body is not null)
            {
                ReadItems(item.Body, parts, named);
            }
            else
            {
                AppendRange(parts, new ClassItem('\0', item.CodePoint), new ClassItem('\0', item.CodePoint));
                named?.Add((item.CodePoint, item.CodePoint));
            }
        }
        if (named is not null)
        {
            AppendCaseSiblings(parts, named);
        }
        return parts;
    }

    private static void ReadItems(string items, ClassParts parts, List<(int First, int Last)>? named)
    {
        int i = 0;
        while (i < items.Length)
        {
            ClassItem first = ReadClassItem(items, ref i);
            // A '-' last in the class is a literal, as is one after a range: the loop reads it as an item of its own.
            if (i + 1 < items.Length && items[i] == '-')
            {
                i++;
                ClassItem last = ReadClassItem(items, ref i);
                AppendRange(parts, first, last);
                named?.Add((first.CodePoint, last.CodePoint));
            }
            else
            {
                AppendItem(parts, first);
                if (first.ClassEscape == '\0')
                {
                    named?.Add((first.CodePoint, first.CodePoint));
                }
            }
        }
    }

    /// <summary>A branch's one item as Python's parser folds it into a charset: a class body, or a literal's code point.</summary>
    private readonly record struct SetItem(string? Body, int CodePoint);

    /// <summary>What a class holds: BMP units, the surrogate code points it names alone, and its supplementary sets.</summary>
    private sealed class ClassParts
    {
        public ClassParts(int capacity) => Bmp = new StringBuilder(capacity);

        public StringBuilder Bmp { get; }

        // Surrogate code points, which Python matches only where the text holds one alone (#1650).
        public StringBuilder Lone { get; } = new();

        public List<Lazy<string>> Astral { get; } = [];

        // Every lone surrogate, as a complement holds them.
        public bool AllLone { get; set; }
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

    /// <summary>
    /// A range of two code points, as Python accepts one: neither a class escape, nor reversed. Its BMP units, its
    /// surrogate code points and its supplementary characters each go where a single one would (#1650).
    /// </summary>
    private static void AppendRange(ClassParts parts, ClassItem first, ClassItem last)
    {
        if (first.ClassEscape != '\0' || last.ClassEscape != '\0' || first.CodePoint > last.CodePoint)
        {
            throw NotPython();
        }

        int lo = first.CodePoint;
        int hi = last.CodePoint;
        AppendUnits(parts.Bmp, lo, Math.Min(hi, 0xD7FF));
        AppendUnits(parts.Lone, Math.Max(lo, 0xD800), Math.Min(hi, 0xDFFF));
        AppendUnits(parts.Bmp, Math.Max(lo, 0xE000), Math.Min(hi, 0xFFFF));
        if (hi > 0xFFFF)
        {
            string spelled = AstralRange(Math.Max(lo, 0x10000), hi);
            parts.Astral.Add(new Lazy<string>(() => spelled));
        }
    }

    private static void AppendUnits(StringBuilder sb, int lo, int hi)
    {
        if (lo > hi)
        {
            return;
        }
        sb.Append(Unit(lo));
        if (hi > lo)
        {
            sb.Append('-').Append(Unit(hi));
        }
    }

    /// <summary>The supplementary characters <paramref name="lo"/> to <paramref name="hi"/>, as pairs: at most three runs of highs.</summary>
    private static string AstralRange(int lo, int hi)
    {
        string a = char.ConvertFromUtf32(lo);
        string b = char.ConvertFromUtf32(hi);
        if (a[0] == b[0])
        {
            return $"{Unit(a[0])}[{Unit(a[1])}-{Unit(b[1])}]";
        }

        var sb = new StringBuilder();
        sb.Append(Unit(a[0])).Append('[').Append(Unit(a[1])).Append(@"-\uDFFF]");
        if (b[0] - a[0] > 1)
        {
            sb.Append("|[").Append(Unit(a[0] + 1)).Append('-').Append(Unit(b[0] - 1)).Append(@"][\uDC00-\uDFFF]");
        }
        sb.Append('|').Append(Unit(b[0])).Append(@"[\uDC00-").Append(Unit(b[1])).Append(']');
        return sb.ToString();
    }

    private static void AppendItem(ClassParts parts, ClassItem item)
    {
        StringBuilder bmp = parts.Bmp;
        List<Lazy<string>> pairs = parts.Astral;
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
                parts.AllLone = true;
                break;
            case 'D':
                bmp.Append(NonDigitBmp.Value);
                pairs.Add(NonDigitPairs);
                parts.AllLone = true;
                break;
            case 'S':
                bmp.Append(NonSpaceBmp.Value);
                pairs.Add(EveryPair);
                parts.AllLone = true;
                break;
            default:
                if (item.CodePoint is >= 0xD800 and <= 0xDFFF)
                {
                    parts.Lone.Append(Unit(item.CodePoint));
                }
                else if (item.CodePoint <= 0xFFFF)
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

    private static ArgumentException NotPython() => new("Python's re refuses the token pattern.");

    /// <summary>A class of what <paramref name="parts"/> holds, as one code point.</summary>
    /// <param name="negated">Whether the class is negated.</param>
    /// <param name="parts">The BMP units, the surrogate code points matched alone, and the supplementary sets.</param>
    /// <param name="spelling">The spelling asked for.</param>
    private static string Spell(bool negated, ClassParts parts, Spelling spelling)
    {
        string bmp = parts.Bmp.ToString();
        List<Lazy<string>> pairs = parts.Astral;
        string loneUnits = parts.AllLone ? @"\uD800-\uDFFF" : parts.Lone.ToString();
        // A class of supplementary characters alone has no BMP part: nothing for it to match there, everything for its
        // complement.
        string? positive = bmp.Length == 0 ? null : $"[{bmp}]";
        string excluded = bmp + loneUnits;
        string negative = excluded.Length == 0 ? @"[\u0000-\uFFFF]" : $"[^{excluded}]";
        if (spelling == Spelling.SurrogateFree)
        {
            return negated ? negative : positive ?? "(?!)";
        }

        // A surrogate it holds is matched where the text holds it alone, never as half a pair.
        string lone = loneUnits.Length > 0 ? $"|(?!{AnyPair}){NotMidPair}[{loneUnits}]" : "";
        if (pairs.Count == 0)
        {
            return SpellBmp(negated, positive, negative, lone);
        }
        string astral = SupplementarySet(pairs);
        if (negated)
        {
            return astral == AnyPair
                ? $"(?:(?!{AnyPair}){NotMidPair}{negative})"
                : $"(?:(?!{astral}){AnyPair}|(?!{AnyPair}){NotMidPair}{negative})";
        }
        string supplementary = astral == AnyPair ? AnyPair : $"(?=[\\uD800-\\uDBFF])(?:{astral})";
        return positive is null ? $"(?:{supplementary}{lone})" : $"(?:{positive}|{supplementary}{lone})";
    }

    // A charset is a set, as Python's _uniq makes it: a set written twice, or one another holds, is written once, and two
    // that hold every pair between them are any pair, so [\w\S] never spells the 5,000-unit word pairs (#1672).
    private static string SupplementarySet(List<Lazy<string>> pairs)
    {
        bool word = pairs.Contains(WordPairs);
        bool nonDigit = pairs.Contains(NonDigitPairs);
        if (pairs.Contains(EveryPair) || (word && (nonDigit || pairs.Contains(NonWordPairs)))
            || (nonDigit && pairs.Contains(DigitPairs)))
        {
            return AnyPair;
        }

        // Python's decimal digits are word characters, and a non-word character is no digit.
        IEnumerable<Lazy<string>> kept = pairs.Where(p => !(word && p == DigitPairs) && !(nonDigit && p == NonWordPairs));
        return string.Join("|", kept.Select(p => p.Value).Distinct(StringComparer.Ordinal));
    }

    // Under IGNORECASE a class holds what each code point it names is folded with, as _sre's charset is (#1668), added
    // as runs: A to Z one range, not 26 items.
    private static void AppendCaseSiblings(ClassParts parts, List<(int First, int Last)> named)
    {
        int[] cased = CasedCodePoints();
        var added = new List<int>();
        foreach ((int first, int last) in named)
        {
            int k = Array.BinarySearch(cased, first);
            for (k = k < 0 ? ~k : k; k < cased.Length && cased[k] <= last; k++)
            {
                ArraySegment<int> siblings = CaseSiblings(cased[k]);
                for (int j = 0; j < siblings.Count; j++)
                {
                    int sibling = siblings.Array![siblings.Offset + j];
                    if (sibling < first || sibling > last)
                    {
                        added.Add(sibling);
                    }
                }
            }
        }
        AppendRuns(parts, added);
    }

    private static void AppendRuns(ClassParts parts, List<int> codePoints)
    {
        codePoints.Sort();
        int n = 0;
        while (n < codePoints.Count)
        {
            int first = codePoints[n];
            int last = first;
            while (++n < codePoints.Count && codePoints[n] <= last + 1)
            {
                last = codePoints[n];
            }
            AppendRange(parts, new ClassItem('\0', first), new ClassItem('\0', last));
        }
    }

    /// <summary>A class of exactly <paramref name="codePoints"/>, a literal's case siblings.</summary>
    private static string Spelled(ArraySegment<int> codePoints, Spelling spelling)
    {
        var parts = new ClassParts(codePoints.Count * 6);
        AppendRuns(parts, [.. codePoints]);
        return Spell(negated: false, parts, spelling);
    }

    // Never empty: a negated class with no surrogate and no pair holds a BMP item.
    private static string NegatedUnits(ClassParts parts) => $"[^{parts.Bmp}]";

    /// <summary>The pair-aware spelling of a class holding no supplementary character.</summary>
    private static string SpellBmp(bool negated, string? positive, string negative, string lone)
    {
        if (negated)
        {
            return $"(?:{AnyPair}|(?!{AnyPair}){NotMidPair}{negative})";
        }
        if (lone.Length == 0)
        {
            return positive ?? "(?!)";
        }
        return positive is null ? $"(?:{lone.Substring(1)})" : $"(?:{positive}{lone})";
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
