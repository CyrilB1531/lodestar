using System.Globalization;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>Extended grapheme clusters, as far as <c>tokenizers</c>' <c>Precompiled</c> normalizer can tell them apart.</summary>
/// <remarks>
/// That normalizer treats a cluster whole only below six UTF-8 bytes (#1260), and a longer one character by
/// character, so only the short clusters of UAX #29 need exact boundaries: CR LF, and a base followed by
/// extending marks, spacing marks, the zero-width joiners or emoji modifiers. Hangul, regional-indicator and
/// emoji sequences reach six bytes, so splitting them further changes nothing. Written here rather than read
/// from <c>StringInfo</c>, whose netstandard2.0 rules are not UAX #29's.
/// </remarks>
internal static class Graphemes
{
    /// <summary>The UTF-16 length of the cluster starting at <paramref name="at"/>.</summary>
    public static int Length(string text, int at)
    {
        if (text[at] == '\r')
        {
            return at + 1 < text.Length && text[at + 1] == '\n' ? 2 : 1;
        }

        int length = CodeUnits(text, at);
        if (IsControl(text, at))
        {
            return length;
        }

        while (at + length < text.Length && Extends(text, at + length))
        {
            length += CodeUnits(text, at + length);
        }

        return length;
    }

    private static int CodeUnits(string text, int at) =>
        char.IsHighSurrogate(text[at]) && at + 1 < text.Length && char.IsLowSurrogate(text[at + 1]) ? 2 : 1;

    private static bool IsControl(string text, int at)
    {
        int code = CodeUnits(text, at) == 2 ? char.ConvertToUtf32(text, at) : text[at];
        UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, at);
        return code is not (0x200C or 0x200D)
            && category is UnicodeCategory.Control or UnicodeCategory.LineSeparator or UnicodeCategory.ParagraphSeparator
                or UnicodeCategory.Format;
    }

    private static bool Extends(string text, int at)
    {
        if (text[at] is '\r' or '\n')
        {
            return false;
        }

        int code = CodeUnits(text, at) == 2 ? char.ConvertToUtf32(text, at) : text[at];
        if (code is 0x200C or 0x200D or 0xFF9E or 0xFF9F || code is >= 0x1F3FB and <= 0x1F3FF || code is >= 0xE0020 and <= 0xE007F)
        {
            return true;
        }

        return CharUnicodeInfo.GetUnicodeCategory(text, at)
            is UnicodeCategory.NonSpacingMark or UnicodeCategory.EnclosingMark or UnicodeCategory.SpacingCombiningMark;
    }
}
