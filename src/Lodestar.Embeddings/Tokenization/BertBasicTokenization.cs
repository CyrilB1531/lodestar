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
    public static string Normalize(string text, bool lowercase)
    {
        var builder = new StringBuilder(text.Length);
        bool changed = false;
        bool unassigned = false;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            int width = Width(text, i);
            int codePoint = width == 2 ? char.ConvertToUtf32(text[i], text[i + 1]) : text[i];
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, i);
            unassigned |= category == UnicodeCategory.OtherNotAssigned;
            if (codePoint == 0 || codePoint == 0xFFFD || IsControl(codePoint, category))
            {
                changed = true;
                continue;
            }

            if (IsWhitespace(text, i, codePoint))
            {
                builder.Append(' ');
                changed |= text[i] != ' ';
            }
            else if (IsCjk(codePoint))
            {
                builder.Append(' ').Append(text, i, width).Append(' ');
                changed = true;
            }
            else
            {
                builder.Append(text, i, width);
            }
        }

        // The input itself when nothing was dropped, padded or mapped — ASCII letters, digits and
        // punctuation never are, where tab, newline and the C0 controls are (#992, #1091).
        string cleaned = changed ? builder.ToString() : text;
        return lowercase ? Lowered(cleaned, unassigned) : cleaned;
    }

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

    /// <summary>The accent strip and the lowercasing, which only text holding a mark pays a second pass for.</summary>
    // CA1308: lowercasing is the normalizer's own step, not a comparison key; an upper-cased text
    // would match no uncased vocabulary entry.
#pragma warning disable CA1308
    private static string Lowered(string cleaned, bool unassigned) =>
        StripAccents(cleaned, unassigned).ToLowerInvariant();
#pragma warning restore CA1308

    /// <summary>The NFD form without its nonspacing marks, which is <c>BertNormalizer</c>'s accent stripping.</summary>
    private static string StripAccents(string text, bool unassigned)
    {
        string decomposed = Decompose(text, unassigned);
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

    /// <summary>The NFD form, taken around the code points <see cref="Segmented"/> must not let move.</summary>
    /// <remarks>
    /// The whole-string form is ICU's and <see cref="Segmented"/> cuts at what <c>CharUnicodeInfo</c>
    /// calls unassigned; a code point .NET's tables do not know and ICU gives a combining class is
    /// reordered by the first and left alone by the second, where <c>tokenizers</c> leaves it alone.
    /// So one anywhere sends the whole text through the walk, on the flag <see cref="Normalize"/>
    /// already had the category for (#1087). A lone surrogate, the other refusal, cannot arrive:
    /// <see cref="IsControl"/> has already dropped it.
    /// </remarks>
    private static string Decompose(string text, bool unassigned)
    {
        if (unassigned)
        {
            return Segmented(text);
        }

        try
        {
            return text.Normalize(NormalizationForm.FormD);
        }
        catch (ArgumentException)
        {
            // NLS reads the OS's tables and not .NET's, so it can refuse what CharUnicodeInfo calls
            // assigned; the exception is the only place a runtime says which (#1050, #1090).
            return Segmented(text);
        }
    }

    /// <summary>The NFD form of each stretch between the unassigned code points, which pass through as they are.</summary>
    /// <remarks>
    /// An unassigned code point has combining class 0 to <c>CharUnicodeInfo</c> and to the reference's
    /// tables, so cutting at it changes no decomposition either side would make. ICU's are newer and
    /// give some of them a class, which is why this walk is taken whenever one is present rather than
    /// only where the runtime refuses the string (#983, #1087).
    /// </remarks>
    private static string Segmented(string text)
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
    private static bool IsControl(int codePoint, UnicodeCategory category)
    {
        if (codePoint is '\t' or '\n' or '\r')
        {
            return false;
        }

        return category is UnicodeCategory.Control
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
