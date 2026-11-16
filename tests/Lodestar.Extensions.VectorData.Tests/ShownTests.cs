using System.Globalization;
using System.Text;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

// CA5394: a seeded Random draws reproducible cases, not anything security-sensitive.
#pragma warning disable CA5394

/// <summary>How a refusal shows a caller's text: bare when it is one visible word, quoted and escaped otherwise (#1820).</summary>
public sealed class ShownTests
{
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

    [Fact]
    public void A_shown_value_reads_back_as_the_text_it_shows_so_two_texts_never_show_alike()
    {
        string[] alphabet = ["a", "b", " ", "\t", "\"", "\\", "\n", "\u200B", "\u2028", "\u00A0", "\uFE0F", "\uD800", "\uDC00", "\uD83D\uDE00", "\u0301", "u", "0", "-", "_", "\u2014", "\u2160", "\u2488", "\u0903", "\u20DD", "\u30FC"];
        var random = new Random(1820);
        for (int draw = 0; draw < 20000; draw++)
        {
            string text = string.Concat(Enumerable.Range(0, random.Next(0, 7)).Select(_ => alphabet[random.Next(alphabet.Length)]));
            string shown = Shown.Value(text);

            Assert.True(ReadBack(shown) == text, $"Draw {draw}: {Escape(text)} showed as {shown}");
            Assert.True(shown.StartsWith('"') || IsWord(shown), $"Draw {draw}: {shown} is bare");
        }
    }

    // Default_Ignorable_Code_Point as Perl 5's Unicode 15.0 tables list it, by perl -e over \p{Default_Ignorable_Code_Point}.
    [Fact]
    public void Every_default_ignorable_code_point_is_escaped()
    {
        (int First, int Last)[] ignorable =
        [
            (0x00AD, 0x00AD), (0x034F, 0x034F), (0x061C, 0x061C), (0x115F, 0x1160), (0x17B4, 0x17B5), (0x180B, 0x180F),
            (0x200B, 0x200F), (0x202A, 0x202E), (0x2060, 0x206F), (0x3164, 0x3164), (0xFE00, 0xFE0F), (0xFEFF, 0xFEFF),
            (0xFFA0, 0xFFA0), (0xFFF0, 0xFFF8), (0x1BCA0, 0x1BCA3), (0x1D173, 0x1D17A), (0xE0000, 0xE0FFF),
        ];
        foreach ((int first, int last) in ignorable)
        {
            for (int codePoint = first; codePoint <= last; codePoint++)
            {
                string character = char.ConvertFromUtf32(codePoint);
                Assert.DoesNotContain(character, Shown.Value($"a{character}b"), StringComparison.Ordinal);
            }
        }
    }

    // Every unit of the BMP, against the categories Blank names: a sweep that catches a unit the code lets through bare.
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

    // Read per code point, apart from the code's own category list: letters, decimal digits, non-enclosing marks, '_' and
    // '-', with a letter or digit among them.
    private static bool IsWord(string shown) => Enumerable.Range(0, shown.Length).Any(i => char.IsLetterOrDigit(shown, i))
        && Enumerable.Range(0, shown.Length).All(i =>
            (char.IsLowSurrogate(shown[i]) && i > 0 && char.IsHighSurrogate(shown[i - 1]))
            || (char.IsLetterOrDigit(shown, i) && CharUnicodeInfo.GetUnicodeCategory(shown, i) != UnicodeCategory.ModifierLetter)
            || shown[i] is '_' or '-'
            || CharUnicodeInfo.GetUnicodeCategory(shown, i) is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark);

    // The inverse the quoting promises: bare text cannot open with a quote, so a quote at the start means quoted.
    private static string ReadBack(string shown)
    {
        if (shown.Length == 0 || shown[0] != '"')
        {
            return shown;
        }

        Assert.True(shown.Length >= 2 && shown[^1] == '"', $"{shown} opens a quote it does not close");
        var text = new StringBuilder();
        int i = 1;
        while (i < shown.Length - 1)
        {
            char c = shown[i];
            Assert.True(c != '"', $"{shown} holds an unescaped quote");
            if (c != '\\')
            {
                text.Append(c);
                i++;
                continue;
            }

            Assert.True(i + 1 < shown.Length - 1, $"{shown} ends in an escape the closing quote cuts");
            if (shown[i + 1] == 'u')
            {
                Assert.True(i + 6 <= shown.Length - 1, $"{shown} ends in a \\u escape the closing quote cuts");
                text.Append((char)int.Parse(shown.AsSpan(i + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                i += 6;
            }
            else
            {
                text.Append(shown[i + 1]);
                i += 2;
            }
        }

        return text.ToString();
    }

    private static string Escape(string text) =>
        string.Concat(text.Select(c => ((int)c).ToString("X4", CultureInfo.InvariantCulture) + " "));
}
