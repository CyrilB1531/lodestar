using System.Globalization;
using System.Text;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>
/// BERT's BasicTokenizer as <c>tokenizers</c> spells it: <c>BertNormalizer</c>, then <c>BertPreTokenizer</c>.
/// </summary>
/// <remarks>
/// The normalizer drops NUL, U+FFFD and every Cc, Cf, Cs and Co character but tab, newline and return, keeping unassigned ones,
/// maps whitespace to a space, pads CJK ideographs, and when lowercasing strips the Mn marks of the NFD form
/// before lowercasing. The pre-tokenizer splits on whitespace and cuts each punctuation character out on its own.
/// Replayed against <c>tokenizers</c> 0.23.2 by <c>vocab_txt.json</c> (#883).
/// </remarks>
internal static class BertBasicTokenization
{
    /// <summary><c>BertNormalizer(clean_text, handle_chinese_chars, strip_accents=None, lowercase)</c>.</summary>
    // CA1308: lowercasing is the normalizer's own step; an upper-cased text would match no uncased vocabulary entry.
#pragma warning disable CA1308
    public static string Normalize(string text, bool lowercase)
    {
        var builder = new StringBuilder(text.Length);
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            int width = Width(text, i);
            int codePoint = width == 2 ? char.ConvertToUtf32(text[i], text[i + 1]) : text[i];
            if (codePoint == 0 || codePoint == 0xFFFD || IsControl(text, i, codePoint))
            {
                continue;
            }

            if (IsWhitespace(text, i, codePoint))
            {
                builder.Append(' ');
            }
            else if (IsCjk(codePoint))
            {
                builder.Append(' ').Append(text, i, width).Append(' ');
            }
            else
            {
                builder.Append(text, i, width);
            }
        }

        // The input itself when nothing was dropped, padded or mapped: ASCII text is the common
        // case and copying it three more times was most of what this path cost (#992).
        string cleaned = builder.Length == text.Length && Same(builder, text) ? text : builder.ToString();
        return lowercase ? Lowered(cleaned) : cleaned;
    }
#pragma warning restore CA1308

    /// <summary>
    /// Finds the next pre-token in <c>text[position..end)</c>, as <see cref="WhitespaceScanner.TryNext"/> does:
    /// a run of neither whitespace nor punctuation, or one punctuation character.
    /// </summary>
    public static bool TryNext(string text, int end, ref int position, out int start)
    {
        int at = position;
        while (at < end && IsWhitespace(text, at, text[at]))
        {
            at += Width(text, at);
        }

        start = at;
        if (at >= end)
        {
            position = end;
            return false;
        }

        if (IsPunctuation(text, at))
        {
            position = at + Width(text, at);
            return true;
        }

        while (at < end && !IsWhitespace(text, at, text[at]) && !IsPunctuation(text, at))
        {
            at += Width(text, at);
        }

        position = at;
        return true;
    }

    /// <summary>Whether the builder holds exactly <paramref name="text"/>, so the input can stand in for it.</summary>
    private static bool Same(StringBuilder builder, string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (builder[i] != text[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>The accent strip and the lowercasing, which only text holding a mark pays a second pass for.</summary>
    // CA1308: lowercasing is the normalizer's own step; see Normalize.
#pragma warning disable CA1308
    private static string Lowered(string cleaned) => StripAccents(cleaned).ToLowerInvariant();
#pragma warning restore CA1308

    /// <summary>The NFD form without its nonspacing marks, which is <c>BertNormalizer</c>'s accent stripping.</summary>
    private static string StripAccents(string text)
    {
        string decomposed = Decompose(text);
        if (!HasMark(decomposed))
        {
            // The decomposition itself, not the input: NFD also maps singletons such as U+212A
            // to K, without changing the length and without leaving a mark behind (#992).
            return decomposed;
        }

        var builder = new StringBuilder(decomposed.Length);
        for (int i = 0; i < decomposed.Length; i += Width(decomposed, i))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(decomposed, i) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(decomposed, i, Width(decomposed, i));
            }
        }

        return builder.ToString();
    }

    /// <summary>Whether any code point is a nonspacing mark, which is what the strip removes.</summary>
    private static bool HasMark(string text)
    {
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(text, i) == UnicodeCategory.NonSpacingMark)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>The NFD form, taken around the unassigned code points <see cref="string.Normalize(NormalizationForm)"/> refuses.</summary>
    /// <remarks>
    /// An unassigned code point has combining class 0, so it starts a new sequence and cutting the text at it
    /// changes no decomposition; it passes through as NFD leaves it (#983).
    /// </remarks>
    private static string Decompose(string text)
    {
        StringBuilder? builder = null;
        int start = 0;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(text, i) == UnicodeCategory.OtherNotAssigned)
            {
                builder ??= new StringBuilder(text.Length);
                builder.Append(text.Substring(start, i - start).Normalize(NormalizationForm.FormD));
                builder.Append(text, i, Width(text, i));
                start = i + Width(text, i);
            }
        }

        return builder is null
            ? text.Normalize(NormalizationForm.FormD)
            : builder.Append(text.Substring(start).Normalize(NormalizationForm.FormD)).ToString();
    }

    /// <summary>Two for a well-formed surrogate pair, one for anything else, a lone surrogate included.</summary>
    private static int Width(string text, int index) =>
        char.IsHighSurrogate(text[index]) && index + 1 < text.Length && char.IsLowSurrogate(text[index + 1]) ? 2 : 1;

    /// <summary><c>tokenizers</c>' <c>is_control</c>: Cc, Cf, Cs and Co but tab, newline and return, and not Cn, which it keeps (#983).</summary>
    private static bool IsControl(string text, int index, int codePoint)
    {
        if (codePoint is '\t' or '\n' or '\r')
        {
            return false;
        }

        return CharUnicodeInfo.GetUnicodeCategory(text, index) is UnicodeCategory.Control
            or UnicodeCategory.Format
            or UnicodeCategory.Surrogate
            or UnicodeCategory.PrivateUse;
    }

    /// <summary>Unicode's White_Space, which no astral code point has.</summary>
    private static bool IsWhitespace(string text, int index, int codePoint) =>
        codePoint <= char.MaxValue && char.IsWhiteSpace(text, index);

    /// <summary>ASCII punctuation, symbols included, or any Unicode P category.</summary>
    private static bool IsPunctuation(string text, int index)
    {
        char unit = text[index];
        if (unit < 128)
        {
            return unit is (>= '!' and <= '/') or (>= ':' and <= '@') or (>= '[' and <= '`') or (>= '{' and <= '~');
        }

        return CharUnicodeInfo.GetUnicodeCategory(text, index) is UnicodeCategory.ConnectorPunctuation
            or UnicodeCategory.DashPunctuation
            or UnicodeCategory.OpenPunctuation
            or UnicodeCategory.ClosePunctuation
            or UnicodeCategory.InitialQuotePunctuation
            or UnicodeCategory.FinalQuotePunctuation
            or UnicodeCategory.OtherPunctuation;
    }

    /// <summary>The CJK ideograph blocks BERT pads, the same eight ranges as <c>BertNormalizer</c>; kana and hangul are not among them.</summary>
    private static bool IsCjk(int codePoint) =>
        codePoint is (>= 0x4E00 and <= 0x9FFF)
            or (>= 0x3400 and <= 0x4DBF)
            or (>= 0x20000 and <= 0x2A6DF)
            or (>= 0x2A700 and <= 0x2B73F)
            or (>= 0x2B740 and <= 0x2B81F)
            or (>= 0x2B920 and <= 0x2CEAF)
            or (>= 0xF900 and <= 0xFAFF)
            or (>= 0x2F800 and <= 0x2FA1F);
}
