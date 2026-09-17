using System.Globalization;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>
/// <c>pre_tokenizers.Whitespace()</c>'s split, <c>\w+|[^\w\s]+</c> as Oniguruma reads it, over code points.
/// </summary>
/// <remarks>
/// Not a .NET <see cref="System.Text.RegularExpressions.Regex"/>: its <c>\w</c> leaves out spacing and
/// enclosing marks, letter numbers and ZWJ/ZWNJ, and it classifies UTF-16 units, so no class can keep an
/// astral letter apart from an astral symbol. Swept against <c>tokenizers</c> 0.23.2 over every code point
/// assigned in Unicode 15.0 -- issue #887, and its spec for the residue left by Unicode versions.
/// </remarks>
internal static class WhitespaceScanner
{
    private const byte Other = 0;
    private const byte Word = 1;
    private const byte Space = 2;

    // Letters, the three marks, Nd, Nl (bits 0 to 9 but OtherNumber's 10) and ConnectorPunctuation's 18.
    private const int WordCategories = 0x3FF | (1 << (int)UnicodeCategory.ConnectorPunctuation);

    // The same answer as the slow path below for every ASCII unit, looked up rather than derived.
    private static ReadOnlySpan<byte> Ascii =>
    [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1,
        0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
    ];

    /// <summary>
    /// Finds the next pre-token in <c>text[position..end)</c>: its start, and <paramref name="position"/>
    /// moved past it. <see langword="false"/> when only whitespace is left.
    /// </summary>
    public static bool TryNext(string text, int end, ref int position, out int start)
    {
        int at = position;
        byte kind;
        int width;
        do
        {
            if (at >= end)
            {
                position = end;
                start = end;
                return false;
            }
            kind = Classify(text, at, end, out width);
            start = at;
            at += width;
        }
        while (kind == Space);

        while (at < end && Classify(text, at, end, out width) == kind)
        {
            at += width;
        }
        position = at;
        return true;
    }

    private static byte Classify(string text, int at, int end, out int width)
    {
        char c = text[at];
        width = 1;
        if (c < 0x80)
        {
            return Ascii[c];
        }
        if (char.IsHighSurrogate(c) && at + 1 < end && char.IsLowSurrogate(text[at + 1]))
        {
            width = 2;
            return IsWord(CharUnicodeInfo.GetUnicodeCategory(text, at), char.ConvertToUtf32(c, text[at + 1]))
                ? Word
                : Other;
        }
        if (IsSpace(c))
        {
            return Space;
        }
        return IsWord(char.GetUnicodeCategory(c), c) ? Word : Other;
    }

    // Oniguruma's \w is Alphabetic, Mark, Nd, Pc and Join_Control: the categories cover all of it but
    // ZWNJ, ZWJ and the circled and squared Latin letters, which are So yet Other_Alphabetic.
    private static bool IsWord(UnicodeCategory category, int codePoint) =>
        ((1 << (int)category) & WordCategories) != 0
        || codePoint is 0x200C or 0x200D
            or (>= 0x24B6 and <= 0x24E9)
            or (>= 0x1F130 and <= 0x1F149)
            or (>= 0x1F150 and <= 0x1F169)
            or (>= 0x1F170 and <= 0x1F189);

    // White_Space, which Oniguruma's \s is; spelled out rather than char.IsWhiteSpace, whose table
    // follows the runtime's Unicode version on .NET Framework and Mono. No astral code point has it.
    private static bool IsSpace(char c) =>
        c is '\u0085' or '\u00A0' or '\u1680' or (>= '\u2000' and <= '\u200A')
            or '\u2028' or '\u2029' or '\u202F' or '\u205F' or '\u3000';
}
