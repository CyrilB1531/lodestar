using System.Globalization;
using System.Text;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

// CA5394: a seeded Random draws reproducible cases, not anything security-sensitive.
#pragma warning disable CA5394

/// <summary>How a refusal shows a caller's text: bare when it is one visible word, quoted and escaped otherwise (#1820, #1825).</summary>
public sealed class ShownTests
{
    // Default_Ignorable_Code_Point as Perl 5's Unicode 15.0 tables list it, by perl -e over \p{Default_Ignorable_Code_Point}:
    // one copy for the sweep and Rule, as it is Unicode's list to check against Perl, not a reading of the categories.
    private static readonly (int First, int Last)[] Ignorable =
    [
        (0x00AD, 0x00AD), (0x034F, 0x034F), (0x061C, 0x061C), (0x115F, 0x1160), (0x17B4, 0x17B5), (0x180B, 0x180F),
        (0x200B, 0x200F), (0x202A, 0x202E), (0x2060, 0x206F), (0x3164, 0x3164), (0xFE00, 0xFE0F), (0xFEFF, 0xFEFF),
        (0xFFA0, 0xFFA0), (0xFFF0, 0xFFF8), (0x1BCA0, 0x1BCA3), (0x1D173, 0x1D17A), (0xE0000, 0xE0FFF),
    ];

    // "\uD800" before "\uDC00" splices into U+10000, a letter Rule and Shown.Value both read as one; apart, each is lone.
    private static readonly string[] Alphabet =
    [
        "a", "b", " ", "\t", "\"", "\\", "\n", "\u200B", "\u2028", "\u00A0", "\uFE0F", "\uD800", "\uDC00", "\uD83D\uDE00",
        "\u0301", "u", "0", "-", "_", "\u2014", "\u2160", "\u2488", "\u0903", "\u20DD", "\u30FC",
    ];

    // An astral mark, U+2800, Lo, Mn and astral Mn and Cf ignorables, an uppercase, titlecase and other letter, and what is
    // blank by category alone: an astral format and private-use character, a paragraph separator and an unassigned unit.
    private static readonly string[] Extra =
    [
        "\uD834\uDD67", "\u2800", "\u3164", "\u034F", "\uDB40\uDD00", "\uD82F\uDCA0", "\uD834\uDD73", "A", "\u01C5", "\u4E00",
        "\uD804\uDCBD", "\uDB80\uDC00", "\u2029", "\uFFFF",
    ];

    public static TheoryData<string, string> Shows => new()
    {
        { "docs", "docs" },
        { @"a\b", @"""a\\b""" },
        { @"a\u00A0b", @"""a\\u00A0b""" },
        { "docs\uFFFF", "\"docs\\uFFFF\"" },
        { string.Empty, "\"\"" },
        { " ", "\" \"" },
        { "a carries", "\"a carries\"" },
        { " docs ", "\" docs \"" },
        { "a\"b", "\"a\\\"b\"" },
        { "\"docs\"", "\"\\\"docs\\\"\"" },
        { "a\\ b", "\"a\\\\ b\"" },
        { "a\nb", "\"a\\u000Ab\"" },
        { "docs\u200B", "\"docs\\u200B\"" },
        { "a\u2028b", "\"a\\u2028b\"" },
        { "a\u00a0b", "\"a\\u00A0b\"" },
        { "docs\ufe0f", "\"docs\\uFE0F\"" },
        { "a\uD800", "\"a\\uD800\"" },
        { "caf\u00e9", "caf\u00e9" },
        { "my-docs_2", "my-docs_2" },
        { "a\"\u0301", "\"a\\\"\\u0301\"" },
        { "a\\\u0301", "\"a\\\\\\u0301\"" },
        { "docs,", "\"docs,\"" },
        { "v1.2", "\"v1.2\"" },
        { "\ud83d\ude00", "\"\ud83d\ude00\"" },
        { "a\udb40\udd00", "\"a\\uDB40\\uDD00\"" },
        { "\u2800", "\"\\u2800\"" },
        { "\u0301a", "\"\\u0301a\"" },
        { "\u0903a", "\"\\u0903a\"" },
        { "\uD834\uDD67a", "\"\\uD834\\uDD67a\"" },
        { "a\uD834\uDD67", "a\uD834\uDD67" },
        { "\u20DDa", "\"\\u20DDa\"" },
        { "Docs", "Docs" },
        { "a\uD804\uDCBDb", "\"a\\uD804\\uDCBDb\"" },
        { "\u01C5a", "\u01C5a" },
        { "\u4E00", "\u4E00" },
        { "e\u0301", "e\u0301" },
        { "\u200B\u0301", "\"\\u200B\\u0301\"" },
        { "a \u0301", "\"a \\u0301\"" },
        { "-", "\"-\"" },
        { "_", "\"_\"" },
        { "\u2014", "\"\u2014\"" },
        { "docs\u2014", "\"docs\u2014\"" },
        { "\u2160", "\"\u2160\"" },
        { "1\u2488", "\"1\u2488\"" },
        { "a\u0903", "a\u0903" },
        { "a\u20dd", "\"a\u20dd\"" },
        { "\u30fc", "\"\u30fc\"" },
        { "\u02bc", "\"\u02bc\"" },
    };

    [Theory]
    [MemberData(nameof(Shows))]
    public void A_value_is_bare_only_when_it_is_one_visible_word(string text, string shown) =>
        Assert.Equal(shown, Shown.Value(text));

    // Seed 1820 over Alphabet alone keeps the draws #1820 checked; seed 1825 draws from Extra too.
    public static TheoryData<int, string[]> Runs => new()
    {
        { 1820, Alphabet },
        { 1825, [.. Alphabet, .. Extra] },
    };

    // The alphabets hold lone surrogates, which a serialized test case would turn into U+FFFD.
    [Theory]
    [MemberData(nameof(Runs), DisableDiscoveryEnumeration = true)]
    public void A_shown_value_reads_back_as_its_text_and_escapes_what_the_rule_asks_and_nothing_else(int seed, string[] letters)
    {
        var random = new Random(seed);
        for (int draw = 0; draw < 20000; draw++)
        {
            string text = string.Concat(Enumerable.Range(0, random.Next(0, 7)).Select(_ => letters[random.Next(letters.Length)]));
            string? fault = Fault(text);
            if (fault is not null)
            {
                Assert.Fail($"Seed {seed}, draw {draw}: text {Units(text)} {fault}");
            }
        }
    }

    // How the shown form of the text breaks the rule, or null; a message is built only for a draw that fails.
    private static string? Fault(string text)
    {
        // Nullable, as the annotation is the code's promise and this is what checks it.
        string? shown;
        try
        {
            shown = Shown.Value(text);
        }
        // CA1031: whatever the code under test throws fails this draw, reported with the seed and text that replay it.
#pragma warning disable CA1031
        catch (Exception thrown)
#pragma warning restore CA1031
        {
            return $"throws {thrown.GetType().Name}: {thrown.Message}";
        }

        return shown is null ? "returns null" : Check(text, shown);
    }

    private static string? Check(string text, string shown)
    {
        string back;
        List<int> escapedAt;
        try
        {
            (back, escapedAt) = ReadBack(shown);
        }
        catch (FormatException syntax)
        {
            return $"shows as {Show(shown)}, which {syntax.Message}";
        }

        if (back != text)
        {
            return $"shows as {Show(shown)}, which reads back as {Units(back)}";
        }

        (List<int> due, bool word) = Rule(text);
        bool bare = due.Count == 0 && word;
        if ((shown == text) != bare)
        {
            return $"shows as {Show(shown)}, {(bare ? "quoted" : "bare")} where it should be {(bare ? "bare" : "quoted")}";
        }

        return escapedAt.SequenceEqual(due) ? null
            : $"shows as {Show(shown)}, escaping units [{string.Join(", ", escapedAt)}], not [{string.Join(", ", due)}]";
    }

    private static string Show(string shown) => $"{shown} {Units(shown)}";

    [Fact]
    public void Every_default_ignorable_code_point_is_escaped()
    {
        foreach ((int first, int last) in Ignorable)
        {
            for (int codePoint = first; codePoint <= last; codePoint++)
            {
                string character = char.ConvertFromUtf32(codePoint);
                string escapes = string.Concat(character.Select(unit => "\\u" + Hex(unit)));
                Assert.Equal($"\"a{escapes}b\"", Shown.Value($"a{character}b"));

                // Opening the text, where the marks among them are also due as marks with no base: the form stays the same.
                Assert.Equal($"\"{escapes}a\"", Shown.Value($"{character}a"));
            }
        }
    }

    // Every unit of the BMP, against the categories Blank names, a sweep that catches a unit the code lets through bare; the
    // ignorables and U+2800, which Blank adds, are the test above's, the theory's and seed 1825's.
    [Fact]
    public void Every_unit_that_prints_nothing_or_blank_is_escaped()
    {
        for (int unit = 0; unit <= char.MaxValue; unit++)
        {
            string shown = Shown.Value($"a{(char)unit}b");
            for (int i = 0; i < shown.Length; i++)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(shown, i);
                bool blank = category is UnicodeCategory.Control or UnicodeCategory.Format or UnicodeCategory.Surrogate
                    or UnicodeCategory.PrivateUse or UnicodeCategory.OtherNotAssigned or UnicodeCategory.LineSeparator
                    or UnicodeCategory.ParagraphSeparator || (category is UnicodeCategory.SpaceSeparator && shown[i] != ' ');
                Assert.False(blank, $"U+{unit:X4} shows as {shown}, a blank unit at {i}");
            }
        }
    }

    // long-comment: the two halves of Shown.Value's summary this reads, stated one by one.
    // Read off the text alone, so the draw holds Shown.Value to its summary rather than to itself. Due: each unit of a code
    // point that prints nothing or as blank, the space aside, or of a mark opening the text or following a space or an
    // escape, a backslashed quote or backslash being one. Word: letters but modifier ones, decimal digits, non-enclosing
    // marks, '_' and '-', with a letter or digit among them.
    private static (List<int> Due, bool Word) Rule(string text)
    {
        var due = new List<int>();
        bool noBase = true;
        bool core = false;
        bool allowed = true;
        int width;
        for (int i = 0; i < text.Length; i += width)
        {
            width = char.IsSurrogatePair(text, i) ? 2 : 1;
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, i);
            bool mark = category is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark
                or UnicodeCategory.EnclosingMark;
            bool isDue = Blank(text, i, width, category) || (noBase && mark);
            if (isDue)
            {
                due.AddRange(Enumerable.Range(i, width));
            }

            noBase = isDue || text[i] is '"' or '\\' or ' ';
            bool letter = char.IsLetterOrDigit(text, i) && category != UnicodeCategory.ModifierLetter;
            core |= letter;
            allowed &= letter || text[i] is '_' or '-' || (mark && category != UnicodeCategory.EnclosingMark);
        }

        return (due, core && allowed);
    }

    // What prints nothing or as blank: the categories, the default-ignorable code points and the blank Braille cell.
    private static bool Blank(string text, int i, int width, UnicodeCategory category)
    {
        int codePoint = width == 2 ? char.ConvertToUtf32(text, i) : text[i];
        return BlankCategory(category, text[i]) || codePoint == 0x2800 || IsIgnorable(codePoint);
    }

    private static bool IsIgnorable(int codePoint) =>
        Array.Exists(Ignorable, range => codePoint >= range.First && codePoint <= range.Last);

    // The categories that print nothing or as blank, Rule's own reading of Unicode's tables, apart from the sweep's copy so
    // each checks the code on its own; the space is aside because the quotes around it show where it is.
    private static bool BlankCategory(UnicodeCategory category, char unit) =>
        category is UnicodeCategory.Control or UnicodeCategory.Format or UnicodeCategory.Surrogate
            or UnicodeCategory.PrivateUse or UnicodeCategory.OtherNotAssigned or UnicodeCategory.LineSeparator
            or UnicodeCategory.ParagraphSeparator || (category is UnicodeCategory.SpaceSeparator && unit != ' ');

    // The inverse the quoting promises, a quote at the start meaning quoted, with the text's index of each unit a \u
    // escape gave; a FormatException says what in the shown form breaks the quoting's syntax.
    private static (string Text, List<int> Escaped) ReadBack(string shown)
    {
        var escapedAt = new List<int>();
        if (shown.Length == 0 || shown[0] != '"')
        {
            return (shown, escapedAt);
        }

        if (shown.Length < 2 || shown[^1] != '"')
        {
            throw new FormatException("opens a quote it does not close");
        }

        var text = new StringBuilder();
        int i = 1;
        while (i < shown.Length - 1)
        {
            if (shown[i] == '"')
            {
                throw new FormatException("holds an unescaped quote");
            }

            if (shown[i] != '\\')
            {
                text.Append(shown[i]);
                i++;
                continue;
            }

            i += ReadEscape(shown, i, text, escapedAt);
        }

        return (text.ToString(), escapedAt);
    }

    // Only a quote or a backslash takes a bare backslash, and a \u four hex digits, all before the closing quote.
    private static int ReadEscape(string shown, int i, StringBuilder text, List<int> escapedAt)
    {
        if (i + 1 >= shown.Length - 1)
        {
            throw new FormatException("ends in an escape the closing quote cuts");
        }

        char kind = shown[i + 1];
        if (kind is '"' or '\\')
        {
            text.Append(kind);
            return 2;
        }

        if (kind != 'u')
        {
            throw new FormatException("takes a backslash before neither a quote, a backslash nor a u");
        }

        if (i + 6 > shown.Length - 1
            || !int.TryParse(shown.AsSpan(i + 2, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int unit))
        {
            throw new FormatException("writes a \\u escape of other than four hex digits before the closing quote");
        }

        escapedAt.Add(text.Length);
        text.Append((char)unit);
        return 6;
    }

    private static string Units(string text) => $"[{string.Join(" ", text.Select(Hex))}]";

    // Uppercase, as the theory rows and the ignorable sweep pin Shown.Value's escapes; the draw accepts either case.
    private static string Hex(char unit) => ((int)unit).ToString("X4", CultureInfo.InvariantCulture);
}
