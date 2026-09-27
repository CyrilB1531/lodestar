using System.Globalization;
using System.Text;

namespace Lodestar.Text.Internal;

// SonarLint S127: a surrogate pair advances the loop variable past its low half.
#pragma warning disable S127

// long-comment: the two Unicode facts jellyfish's phonetic encoders read and .NET does not expose.
// jellyfish 1.2.1 runs Rust's str::to_uppercase, the full case mapping, where char.ToUpperInvariant
// is the simple one: "ß" becomes "SS" there and stays "ß" here. Its match rating codex accepts a
// character by char::is_alphabetic, the Alphabetic property, which is char.IsLetter plus letter
// numbers plus the Other_Alphabetic marks (Devanagari vowel signs and the like). Both tables were
// measured against jellyfish 1.2.1 and Python 3.12's unicodedata 15.0 over every scalar value
// (#1198); a scalar Unicode assigned after 15.0 follows whatever the runtime's tables say.

/// <summary>The Unicode facts jellyfish's phonetic encoders read, in .NET terms.</summary>
internal static class PhoneticText
{
    // The scalars whose full uppercase is not their simple one, as str.upper() spells it.
    private static readonly Dictionary<int, string> FullUppercase = new()
    {
        [0x00DF] = "\u0053\u0053",
        [0x0131] = "\u0049",
        [0x0149] = "\u02BC\u004E",
        [0x017F] = "\u0053",
        [0x01F0] = "\u004A\u030C",
        [0x0390] = "\u0399\u0308\u0301",
        [0x03B0] = "\u03A5\u0308\u0301",
        [0x0587] = "\u0535\u0552",
        [0x1E96] = "\u0048\u0331",
        [0x1E97] = "\u0054\u0308",
        [0x1E98] = "\u0057\u030A",
        [0x1E99] = "\u0059\u030A",
        [0x1E9A] = "\u0041\u02BE",
        [0x1F50] = "\u03A5\u0313",
        [0x1F52] = "\u03A5\u0313\u0300",
        [0x1F54] = "\u03A5\u0313\u0301",
        [0x1F56] = "\u03A5\u0313\u0342",
        [0x1F80] = "\u1F08\u0399",
        [0x1F81] = "\u1F09\u0399",
        [0x1F82] = "\u1F0A\u0399",
        [0x1F83] = "\u1F0B\u0399",
        [0x1F84] = "\u1F0C\u0399",
        [0x1F85] = "\u1F0D\u0399",
        [0x1F86] = "\u1F0E\u0399",
        [0x1F87] = "\u1F0F\u0399",
        [0x1F88] = "\u1F08\u0399",
        [0x1F89] = "\u1F09\u0399",
        [0x1F8A] = "\u1F0A\u0399",
        [0x1F8B] = "\u1F0B\u0399",
        [0x1F8C] = "\u1F0C\u0399",
        [0x1F8D] = "\u1F0D\u0399",
        [0x1F8E] = "\u1F0E\u0399",
        [0x1F8F] = "\u1F0F\u0399",
        [0x1F90] = "\u1F28\u0399",
        [0x1F91] = "\u1F29\u0399",
        [0x1F92] = "\u1F2A\u0399",
        [0x1F93] = "\u1F2B\u0399",
        [0x1F94] = "\u1F2C\u0399",
        [0x1F95] = "\u1F2D\u0399",
        [0x1F96] = "\u1F2E\u0399",
        [0x1F97] = "\u1F2F\u0399",
        [0x1F98] = "\u1F28\u0399",
        [0x1F99] = "\u1F29\u0399",
        [0x1F9A] = "\u1F2A\u0399",
        [0x1F9B] = "\u1F2B\u0399",
        [0x1F9C] = "\u1F2C\u0399",
        [0x1F9D] = "\u1F2D\u0399",
        [0x1F9E] = "\u1F2E\u0399",
        [0x1F9F] = "\u1F2F\u0399",
        [0x1FA0] = "\u1F68\u0399",
        [0x1FA1] = "\u1F69\u0399",
        [0x1FA2] = "\u1F6A\u0399",
        [0x1FA3] = "\u1F6B\u0399",
        [0x1FA4] = "\u1F6C\u0399",
        [0x1FA5] = "\u1F6D\u0399",
        [0x1FA6] = "\u1F6E\u0399",
        [0x1FA7] = "\u1F6F\u0399",
        [0x1FA8] = "\u1F68\u0399",
        [0x1FA9] = "\u1F69\u0399",
        [0x1FAA] = "\u1F6A\u0399",
        [0x1FAB] = "\u1F6B\u0399",
        [0x1FAC] = "\u1F6C\u0399",
        [0x1FAD] = "\u1F6D\u0399",
        [0x1FAE] = "\u1F6E\u0399",
        [0x1FAF] = "\u1F6F\u0399",
        [0x1FB2] = "\u1FBA\u0399",
        [0x1FB3] = "\u0391\u0399",
        [0x1FB4] = "\u0386\u0399",
        [0x1FB6] = "\u0391\u0342",
        [0x1FB7] = "\u0391\u0342\u0399",
        [0x1FBC] = "\u0391\u0399",
        [0x1FC2] = "\u1FCA\u0399",
        [0x1FC3] = "\u0397\u0399",
        [0x1FC4] = "\u0389\u0399",
        [0x1FC6] = "\u0397\u0342",
        [0x1FC7] = "\u0397\u0342\u0399",
        [0x1FCC] = "\u0397\u0399",
        [0x1FD2] = "\u0399\u0308\u0300",
        [0x1FD3] = "\u0399\u0308\u0301",
        [0x1FD6] = "\u0399\u0342",
        [0x1FD7] = "\u0399\u0308\u0342",
        [0x1FE2] = "\u03A5\u0308\u0300",
        [0x1FE3] = "\u03A5\u0308\u0301",
        [0x1FE4] = "\u03A1\u0313",
        [0x1FE6] = "\u03A5\u0342",
        [0x1FE7] = "\u03A5\u0308\u0342",
        [0x1FF2] = "\u1FFA\u0399",
        [0x1FF3] = "\u03A9\u0399",
        [0x1FF4] = "\u038F\u0399",
        [0x1FF6] = "\u03A9\u0342",
        [0x1FF7] = "\u03A9\u0342\u0399",
        [0x1FFC] = "\u03A9\u0399",
        [0xFB00] = "\u0046\u0046",
        [0xFB01] = "\u0046\u0049",
        [0xFB02] = "\u0046\u004C",
        [0xFB03] = "\u0046\u0046\u0049",
        [0xFB04] = "\u0046\u0046\u004C",
        [0xFB05] = "\u0053\u0054",
        [0xFB06] = "\u0053\u0054",
        [0xFB13] = "\u0544\u0546",
        [0xFB14] = "\u0544\u0535",
        [0xFB15] = "\u0544\u053B",
        [0xFB16] = "\u054E\u0546",
        [0xFB17] = "\u0544\u053D",
    };

    // Other_Alphabetic marks and symbols, as [first, last] pairs in ascending order.
    private static readonly int[] OtherAlphabetic =
    [
            0x0363, 0x036F, 0x05B0, 0x05BD, 0x05BF, 0x05BF, 0x05C1, 0x05C2, 0x05C4, 0x05C5,
            0x05C7, 0x05C7, 0x0610, 0x061A, 0x064B, 0x0657, 0x0659, 0x065F, 0x0670, 0x0670,
            0x06D6, 0x06DC, 0x06E1, 0x06E4, 0x06E7, 0x06E8, 0x06ED, 0x06ED, 0x0711, 0x0711,
            0x0730, 0x073F, 0x07A6, 0x07B0, 0x0816, 0x0817, 0x081B, 0x0823, 0x0825, 0x0827,
            0x0829, 0x082C, 0x08D4, 0x08DF, 0x08E3, 0x08E9, 0x08F0, 0x0903, 0x093A, 0x093B,
            0x093E, 0x094C, 0x094E, 0x094F, 0x0955, 0x0957, 0x0962, 0x0963, 0x0981, 0x0983,
            0x09BE, 0x09C4, 0x09C7, 0x09C8, 0x09CB, 0x09CC, 0x09D7, 0x09D7, 0x09E2, 0x09E3,
            0x0A01, 0x0A03, 0x0A3E, 0x0A42, 0x0A47, 0x0A48, 0x0A4B, 0x0A4C, 0x0A51, 0x0A51,
            0x0A70, 0x0A71, 0x0A75, 0x0A75, 0x0A81, 0x0A83, 0x0ABE, 0x0AC5, 0x0AC7, 0x0AC9,
            0x0ACB, 0x0ACC, 0x0AE2, 0x0AE3, 0x0AFA, 0x0AFC, 0x0B01, 0x0B03, 0x0B3E, 0x0B44,
            0x0B47, 0x0B48, 0x0B4B, 0x0B4C, 0x0B56, 0x0B57, 0x0B62, 0x0B63, 0x0B82, 0x0B82,
            0x0BBE, 0x0BC2, 0x0BC6, 0x0BC8, 0x0BCA, 0x0BCC, 0x0BD7, 0x0BD7, 0x0C00, 0x0C04,
            0x0C3E, 0x0C44, 0x0C46, 0x0C48, 0x0C4A, 0x0C4C, 0x0C55, 0x0C56, 0x0C62, 0x0C63,
            0x0C81, 0x0C83, 0x0CBE, 0x0CC4, 0x0CC6, 0x0CC8, 0x0CCA, 0x0CCC, 0x0CD5, 0x0CD6,
            0x0CE2, 0x0CE3, 0x0CF3, 0x0CF3, 0x0D00, 0x0D03, 0x0D3E, 0x0D44, 0x0D46, 0x0D48,
            0x0D4A, 0x0D4C, 0x0D57, 0x0D57, 0x0D62, 0x0D63, 0x0D81, 0x0D83, 0x0DCF, 0x0DD4,
            0x0DD6, 0x0DD6, 0x0DD8, 0x0DDF, 0x0DF2, 0x0DF3, 0x0E31, 0x0E31, 0x0E34, 0x0E3A,
            0x0E4D, 0x0E4D, 0x0EB1, 0x0EB1, 0x0EB4, 0x0EB9, 0x0EBB, 0x0EBC, 0x0ECD, 0x0ECD,
            0x0F71, 0x0F83, 0x0F8D, 0x0F97, 0x0F99, 0x0FBC, 0x102B, 0x1036, 0x1038, 0x1038,
            0x103B, 0x103E, 0x1056, 0x1059, 0x105E, 0x1060, 0x1062, 0x1064, 0x1067, 0x106D,
            0x1071, 0x1074, 0x1082, 0x108D, 0x108F, 0x108F, 0x109A, 0x109D, 0x1712, 0x1713,
            0x1732, 0x1733, 0x1752, 0x1753, 0x1772, 0x1773, 0x17B6, 0x17C8, 0x1885, 0x1886,
            0x18A9, 0x18A9, 0x1920, 0x192B, 0x1930, 0x1938, 0x1A17, 0x1A1B, 0x1A55, 0x1A5E,
            0x1A61, 0x1A74, 0x1ABF, 0x1AC0, 0x1ACC, 0x1ACE, 0x1B00, 0x1B04, 0x1B35, 0x1B43,
            0x1B80, 0x1B82, 0x1BA1, 0x1BA9, 0x1BAC, 0x1BAD, 0x1BE7, 0x1BF1, 0x1C24, 0x1C36,
            0x1DD3, 0x1DF4, 0x24B6, 0x24E9, 0x2DE0, 0x2DFF, 0xA674, 0xA67B, 0xA69E, 0xA69F,
            0xA802, 0xA802, 0xA80B, 0xA80B, 0xA823, 0xA827, 0xA880, 0xA881, 0xA8B4, 0xA8C3,
            0xA8C5, 0xA8C5, 0xA8FF, 0xA8FF, 0xA926, 0xA92A, 0xA947, 0xA952, 0xA980, 0xA983,
            0xA9B4, 0xA9BF, 0xA9E5, 0xA9E5, 0xAA29, 0xAA36, 0xAA43, 0xAA43, 0xAA4C, 0xAA4D,
            0xAA7B, 0xAA7D, 0xAAB0, 0xAAB0, 0xAAB2, 0xAAB4, 0xAAB7, 0xAAB8, 0xAABE, 0xAABE,
            0xAAEB, 0xAAEF, 0xAAF5, 0xAAF5, 0xABE3, 0xABEA, 0xFB1E, 0xFB1E, 0x10376, 0x1037A,
            0x10A01, 0x10A03, 0x10A05, 0x10A06, 0x10A0C, 0x10A0F, 0x10D24, 0x10D27, 0x10EAB, 0x10EAC,
            0x11000, 0x11002, 0x11038, 0x11045, 0x11073, 0x11074, 0x11080, 0x11082, 0x110B0, 0x110B8,
            0x110C2, 0x110C2, 0x11100, 0x11102, 0x11127, 0x11132, 0x11145, 0x11146, 0x11180, 0x11182,
            0x111B3, 0x111BF, 0x111CE, 0x111CF, 0x1122C, 0x11234, 0x11237, 0x11237, 0x1123E, 0x1123E,
            0x11241, 0x11241, 0x112DF, 0x112E8, 0x11300, 0x11303, 0x1133E, 0x11344, 0x11347, 0x11348,
            0x1134B, 0x1134C, 0x11357, 0x11357, 0x11362, 0x11363, 0x11435, 0x11441, 0x11443, 0x11445,
            0x114B0, 0x114C1, 0x115AF, 0x115B5, 0x115B8, 0x115BE, 0x115DC, 0x115DD, 0x11630, 0x1163E,
            0x11640, 0x11640, 0x116AB, 0x116B5, 0x1171D, 0x1172A, 0x1182C, 0x11838, 0x11930, 0x11935,
            0x11937, 0x11938, 0x1193B, 0x1193C, 0x11940, 0x11940, 0x11942, 0x11942, 0x119D1, 0x119D7,
            0x119DA, 0x119DF, 0x119E4, 0x119E4, 0x11A01, 0x11A0A, 0x11A35, 0x11A39, 0x11A3B, 0x11A3E,
            0x11A51, 0x11A5B, 0x11A8A, 0x11A97, 0x11C2F, 0x11C36, 0x11C38, 0x11C3E, 0x11C92, 0x11CA7,
            0x11CA9, 0x11CB6, 0x11D31, 0x11D36, 0x11D3A, 0x11D3A, 0x11D3C, 0x11D3D, 0x11D3F, 0x11D41,
            0x11D43, 0x11D43, 0x11D47, 0x11D47, 0x11D8A, 0x11D8E, 0x11D90, 0x11D91, 0x11D93, 0x11D96,
            0x11EF3, 0x11EF6, 0x11F00, 0x11F01, 0x11F03, 0x11F03, 0x11F34, 0x11F3A, 0x11F3E, 0x11F40,
            0x16F4F, 0x16F4F, 0x16F51, 0x16F87, 0x16F8F, 0x16F92, 0x16FF0, 0x16FF1, 0x1BC9E, 0x1BC9E,
            0x1E000, 0x1E006, 0x1E008, 0x1E018, 0x1E01B, 0x1E021, 0x1E023, 0x1E024, 0x1E026, 0x1E02A,
            0x1E08F, 0x1E08F, 0x1E947, 0x1E947, 0x1F130, 0x1F149, 0x1F150, 0x1F169, 0x1F170, 0x1F189,
    ];

    /// <summary>The full-mapping uppercase of <paramref name="value"/>, as Rust's <c>to_uppercase</c> gives it.</summary>
    public static string ToUpperFull(ReadOnlySpan<char> value)
    {
        var sb = new StringBuilder(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (char.IsHighSurrogate(c) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]))
            {
                int cp = char.ConvertToUtf32(c, value[i + 1]);
                sb.Append(FullUppercase.TryGetValue(cp, out string? full)
                    ? full
                    : new string([c, value[i + 1]]).ToUpperInvariant());
                i++;
            }
            else
            {
                sb.Append(FullUppercase.TryGetValue(c, out string? full) ? full : char.ToUpperInvariant(c).ToString());
            }
        }
        return sb.ToString();
    }

    /// <summary>The extended grapheme clusters of <paramref name="s"/>, each as a string.</summary>
    public static List<string> Graphemes(string s)
    {
        var clusters = new List<string>(s.Length);
        TextElementEnumerator e = StringInfo.GetTextElementEnumerator(s);
        while (e.MoveNext())
        {
            clusters.Add(e.GetTextElement());
        }
        return clusters;
    }

    /// <summary>Whether the scalar at <paramref name="index"/> of <paramref name="s"/> is Alphabetic, as Rust's <c>char::is_alphabetic</c> reads it.</summary>
    public static bool IsAlphabetic(string s, int index)
    {
        UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(s, index);
        if (category is UnicodeCategory.UppercaseLetter or UnicodeCategory.LowercaseLetter
            or UnicodeCategory.TitlecaseLetter or UnicodeCategory.ModifierLetter
            or UnicodeCategory.OtherLetter or UnicodeCategory.LetterNumber)
        {
            return true;
        }
        int cp = char.IsSurrogatePair(s, index) ? char.ConvertToUtf32(s, index) : s[index];
        int lo = 0;
        int hi = (OtherAlphabetic.Length / 2) - 1;
        while (lo <= hi)
        {
            int mid = (lo + hi) >> 1;
            if (cp < OtherAlphabetic[2 * mid])
            {
                hi = mid - 1;
            }
            else if (cp > OtherAlphabetic[(2 * mid) + 1])
            {
                lo = mid + 1;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
