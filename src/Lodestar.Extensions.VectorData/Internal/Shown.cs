using System.Globalization;
using System.Text;

namespace Lodestar.Extensions.VectorData;

/// <summary>How a refusal shows a value the caller gave it, a name or a key's text.</summary>
internal static class Shown
{
    /// <summary>
    /// The text as it is when it is one word, of letters but modifier ones, decimal digits, non-enclosing marks, <c>_</c>
    /// and <c>-</c> alone, a letter or digit among them, and quoted otherwise, so a reader sees where it ends: no
    /// punctuation of the sentence around it, and no escape, can read as part of it. Inside the quotes a quote and a
    /// backslash take a backslash, and each UTF-16 unit of a character that prints nothing or as blank, the space aside,
    /// or of a combining mark opening the text or following a space or an escape, which it would sit on, is written
    /// <c>\u</c> and four hex digits. Characters that print alike, as Latin and Cyrillic letters may, are not told apart,
    /// nor is right-to-left text's displayed order undone. The categories are the runtime's Unicode tables, but Unicode
    /// 15.0's default-ignorable code points and U+2800 are escaped whatever category those tables give them.
    /// </summary>
    internal static string Value(string text)
    {
        var quoted = new StringBuilder(text.Length + 2).Append('"');
        bool quotes = false;
        bool core = false;

        // Whether what precedes is the opening quote, a space or an escape, which a mark would sit on: such a mark is escaped.
        bool noBase = true;
        int width;
        for (int i = 0; i < text.Length; i += width)
        {
            width = char.IsSurrogatePair(text, i) ? 2 : 1;
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, i);
            bool escaped = Blank(text, i, width, category) || (noBase && IsMark(category));
            noBase = escaped || text[i] is '"' or '\\' or ' ';
            quotes |= escaped || !Word(text, i, category);
            core |= Core(category);
            Append(quoted, text, i, width, escaped);
        }

        return quotes || !core ? quoted.Append('"').ToString() : text;
    }

    // One character inside the quotes: as \u escapes, one per UTF-16 unit, or as it is, a quote or a backslash escaped.
    private static void Append(StringBuilder quoted, string text, int i, int width, bool escaped)
    {
        if (!escaped)
        {
            quoted.Append(text[i] is '"' or '\\' ? "\\" : string.Empty).Append(text, i, width);
            return;
        }

        for (int unit = i; unit < i + width; unit++)
        {
            quoted.Append("\\u").Append(((int)text[unit]).ToString("X4", CultureInfo.InvariantCulture));
        }
    }

    // A letter, a modifier letter aside, as some draw as a dash or an apostrophe, or a decimal digit: a bare word holds one.
    private static bool Core(UnicodeCategory category) => category is UnicodeCategory.UppercaseLetter
        or UnicodeCategory.LowercaseLetter or UnicodeCategory.TitlecaseLetter or UnicodeCategory.OtherLetter
        or UnicodeCategory.DecimalDigitNumber;

    // What a bare word may hold beside its letters and digits; a mark opening the text is escaped, which quotes it.
    private static bool Word(string text, int i, UnicodeCategory category) => Core(category)
        || (IsMark(category) && category != UnicodeCategory.EnclosingMark) || text[i] is '_' or '-';

    private static bool IsMark(UnicodeCategory category) =>
        category is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark or UnicodeCategory.EnclosingMark;

    // What prints nothing or as blank: a separator, the space aside, a control, format, private-use, unassigned or lone
    // surrogate unit by the runtime's tables, a default-ignorable code point by Unicode's own list, or the blank Braille cell.
    private static bool Blank(string text, int i, int width, UnicodeCategory category) =>
        (category is UnicodeCategory.SpaceSeparator && text[i] != ' ')
        || category is UnicodeCategory.LineSeparator or UnicodeCategory.ParagraphSeparator or UnicodeCategory.Control
            or UnicodeCategory.Format or UnicodeCategory.PrivateUse or UnicodeCategory.OtherNotAssigned
            or UnicodeCategory.Surrogate
        || Ignorable(width == 2 ? char.ConvertToUtf32(text, i) : text[i]) || text[i] == '\u2800';

    // Default_Ignorable_Code_Point, whole as of Unicode 15.0 (Perl's \p{Default_Ignorable_Code_Point} lists the same): held
    // here rather than read off the categories, which name some of these Mn or Lo and old runtimes' tables leave unassigned.
    private static bool Ignorable(int codePoint) => codePoint is 0x00AD or 0x034F or 0x061C or 0x115F or 0x1160 or 0x17B4
        or 0x17B5 or (>= 0x180B and <= 0x180F) or (>= 0x200B and <= 0x200F) or (>= 0x202A and <= 0x202E)
        or (>= 0x2060 and <= 0x206F) or 0x3164 or (>= 0xFE00 and <= 0xFE0F) or 0xFEFF or 0xFFA0 or (>= 0xFFF0 and <= 0xFFF8)
        or (>= 0x1BCA0 and <= 0x1BCA3) or (>= 0x1D173 and <= 0x1D17A) or (>= 0xE0000 and <= 0xE0FFF);
}
