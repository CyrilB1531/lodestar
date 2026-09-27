using System.Text;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Phonetics;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
#pragma warning disable S3776

/// <summary>
/// American Soundex phonetic encoding: an initial character followed by three digits.
/// </summary>
/// <remarks>
/// Reference behavior: <c>jellyfish.soundex</c> 1.2.1 over the full uppercase and NFKD; the first
/// character leads whatever it is. <c>bfpv→1</c>, <c>cgjkqsxz→2</c>, <c>dt→3</c>, <c>l→4</c>,
/// <c>mn→5</c>, <c>r→6</c>; <c>h</c>/<c>w</c> are transparent, and anything else — a vowel, an
/// apostrophe — separates equal codes (<c>Keats's</c> is <c>K322</c>). Thread-safe.
/// </remarks>
public static class Soundex
{
    /// <summary>Encodes <paramref name="value"/> to its 4-character Soundex code (or empty).</summary>
    public static string Encode(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        string v = WellFormedNormalization.Normalize(PhoneticText.ToUpperFull(value), NormalizationForm.FormKD);
        int firstLength = char.IsSurrogatePair(v, 0) ? 2 : 1;
        var sb = new StringBuilder(firstLength + 3);
        sb.Append(v, 0, firstLength);
        int previous = Code(v[0]);
        int digits = 0;

        for (int i = firstLength; i < v.Length && digits < 3; i++)
        {
            char c = v[i];
            int code = Code(c);
            if (code != 0)
            {
                if (code != previous)
                {
                    sb.Append((char)('0' + code));
                    digits++;
                }
                previous = code;
            }
            else if (c is not ('H' or 'W'))
            {
                previous = 0;
            }
        }

        return sb.Append('0', 3 - digits).ToString();
    }

    /// <summary>Encodes a string to its Soundex code.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    public static string Encode(string value)
    {
        Guard.NotNull(value);
        return Encode(value.AsSpan());
    }

    private static int Code(char c) => c switch
    {
        'B' or 'F' or 'P' or 'V' => 1,
        'C' or 'G' or 'J' or 'K' or 'Q' or 'S' or 'X' or 'Z' => 2,
        'D' or 'T' => 3,
        'L' => 4,
        'M' or 'N' => 5,
        'R' => 6,
        _ => 0, // everything else, H and W included
    };
}
