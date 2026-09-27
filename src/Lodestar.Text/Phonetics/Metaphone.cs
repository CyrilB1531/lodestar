using System.Text;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Phonetics;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S127: digraph consumption advances the loop variable by design.
#pragma warning disable S3776, S127

/// <summary>
/// Metaphone phonetic encoding (Lawrence Philips, 1990).
/// </summary>
/// <remarks>
/// Reference behavior: <c>jellyfish.metaphone</c> 1.2.1, rule for rule: the input is uppercased by
/// the full case mapping (<c>ß</c> becomes <c>SS</c>) and decomposed (NFKD), a space separates words
/// in the code, and any other character outside the rules is skipped where it stands — so it still
/// parts a double (<c>Keats's</c> keeps both S sounds). Codes: <c>B X S K J T F H L M N P R 0 W Y</c>
/// (<c>X</c> is "sh", <c>0</c> is "th"); vowels appear only word-initial. Thread-safe.
/// </remarks>
public static class Metaphone
{
    // What jellyfish reads past either end of the word; a literal '*' in the input reads the same.
    private const char Past = '*';

    /// <summary>Encodes a string to its Metaphone code.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    public static string Encode(string value)
    {
        Guard.NotNull(value);
        return Encode(value.AsSpan());
    }

    /// <summary>Encodes <paramref name="value"/> to its Metaphone code (or empty).</summary>
    public static string Encode(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        string upper = PhoneticText.ToUpperFull(value);
        string v = WellFormedNormalization.Normalize(upper, NormalizationForm.FormKD);
        // The prefix is read before decomposition, as jellyfish does; its first letter never decomposes.
        if (upper.StartsWith("KN", StringComparison.Ordinal) || upper.StartsWith("GN", StringComparison.Ordinal)
            || upper.StartsWith("PN", StringComparison.Ordinal) || upper.StartsWith("WR", StringComparison.Ordinal)
            || upper.StartsWith("AE", StringComparison.Ordinal))
        {
            v = v.Substring(1);
        }

        var result = new StringBuilder(v.Length);
        int n = v.Length;
        for (int i = 0; i < n; i++)
        {
            char c = v[i];
            char next = i + 1 < n ? v[i + 1] : Past;
            char next2 = i + 2 < n ? v[i + 2] : Past;

            // A doubled letter sounds once, except CC.
            if (c == next && c != 'C')
            {
                continue;
            }

            switch (c)
            {
                case 'A' or 'E' or 'I' or 'O' or 'U':
                    if (i == 0 || v[i - 1] == ' ')
                    {
                        result.Append(c);
                    }
                    break;

                case 'B':
                    // Silent only in a terminal "MB".
                    if (i == 0 || v[i - 1] != 'M' || next != Past)
                    {
                        result.Append('B');
                    }
                    break;

                case 'C':
                    if ((next == 'I' && next2 == 'A') || next == 'H')
                    {
                        result.Append('X');
                        i++;
                    }
                    else if (next is 'I' or 'E' or 'Y')
                    {
                        result.Append('S');
                        i++;
                    }
                    else
                    {
                        result.Append('K');
                    }
                    break;

                case 'D':
                    if (next == 'G' && next2 is 'I' or 'E' or 'Y')
                    {
                        result.Append('J');
                        i += 2;
                    }
                    else
                    {
                        result.Append('T');
                    }
                    break;

                case 'F' or 'J' or 'L' or 'M' or 'N' or 'R':
                    result.Append(c);
                    break;

                case 'G':
                    if (next is 'I' or 'E' or 'Y')
                    {
                        result.Append('J');
                    }
                    else if ((next == 'H' && next2 != Past && !IsVowel(next2)) || (next == 'N' && next2 == Past))
                    {
                        i++; // GH before a consonant, and a terminal GN: both letters silent
                    }
                    else
                    {
                        result.Append('K');
                    }
                    break;

                case 'H':
                    if (i == 0 || IsVowel(next) || !IsVowel(v[i - 1]))
                    {
                        result.Append('H');
                    }
                    break;

                case 'K':
                    if (i == 0 || v[i - 1] != 'C')
                    {
                        result.Append('K');
                    }
                    break;

                case 'P':
                    if (next == 'H')
                    {
                        result.Append('F');
                        i++;
                    }
                    else
                    {
                        result.Append('P');
                    }
                    break;

                case 'Q':
                    result.Append('K');
                    break;

                case 'S':
                    if (next == 'H')
                    {
                        result.Append('X');
                        i++;
                    }
                    else if (next == 'I' && next2 is 'O' or 'A')
                    {
                        result.Append('X');
                        i += 2;
                    }
                    else
                    {
                        result.Append('S');
                    }
                    break;

                case 'T':
                    if (next == 'I' && next2 is 'O' or 'A')
                    {
                        result.Append('X');
                    }
                    else if (next == 'H')
                    {
                        result.Append('0');
                        i++;
                    }
                    else if (next != 'C' || next2 != 'H')
                    {
                        result.Append('T'); // silent before CH
                    }
                    break;

                case 'V':
                    result.Append('F');
                    break;

                case 'W':
                    if (i == 0 && next == 'H')
                    {
                        result.Append('W');
                        i++;
                    }
                    else if (IsVowel(next))
                    {
                        result.Append('W');
                    }
                    break;

                case 'X':
                    if (i != 0)
                    {
                        result.Append("KS");
                    }
                    else
                    {
                        result.Append(next == 'H' || (next == 'I' && next2 is 'O' or 'A') ? 'X' : 'S');
                    }
                    break;

                case 'Y':
                    if (IsVowel(next))
                    {
                        result.Append('Y');
                    }
                    break;

                case 'Z':
                    result.Append('S');
                    break;

                case ' ':
                    if (result.Length > 0 && result[result.Length - 1] != ' ')
                    {
                        result.Append(' ');
                    }
                    break;
            }
        }

        return result.ToString();
    }

    private static bool IsVowel(char c) => c is 'A' or 'E' or 'I' or 'O' or 'U';
}
