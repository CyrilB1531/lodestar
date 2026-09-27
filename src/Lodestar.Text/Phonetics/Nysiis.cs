using Lodestar.Text.Internal;

namespace Lodestar.Text.Phonetics;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S127: digraph consumption advances the loop variable by design.
#pragma warning disable S3776, S127

/// <summary>
/// NYSIIS (New York State Identification and Intelligence System) phonetic encoding.
/// </summary>
/// <remarks>
/// Reference behavior: <c>jellyfish.nysiis</c> 1.2.1 — the modern, non-truncated variant (the
/// original 6-character limit is not applied), rule for rule. The input is uppercased by the full
/// case mapping and read one grapheme cluster at a time; a character outside the rules, an
/// apostrophe or a space among them, is kept in the code as it stands (<c>O'Brien</c> is
/// <c>O'BRAN</c>). The empty string encodes to the empty string. Thread-safe.
/// </remarks>
public static class Nysiis
{
    private const string A = "A";

    /// <summary>Encodes a string to its NYSIIS code.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    public static string Encode(string value)
    {
        Guard.NotNull(value);
        return Encode(value.AsSpan());
    }

    /// <summary>Encodes <paramref name="value"/> to its NYSIIS code (or empty).</summary>
    public static string Encode(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        string s = PhoneticText.ToUpperFull(value);
        List<string> v = PhoneticText.Graphemes(s);

        // Prefixes and suffixes are read on the string, and rewritten on its clusters.
        if (s.StartsWith("MAC", StringComparison.Ordinal))
        {
            v[1] = "C";
        }
        else if (s.StartsWith("KN", StringComparison.Ordinal))
        {
            v.RemoveAt(0);
        }
        else if (s.StartsWith('K'))
        {
            v[0] = "C";
        }
        else if (s.StartsWith("PH", StringComparison.Ordinal) || s.StartsWith("PF", StringComparison.Ordinal))
        {
            v[0] = "F";
            v[1] = "F";
        }
        else if (s.StartsWith("SCH", StringComparison.Ordinal))
        {
            v[1] = "S";
            v[2] = "S";
        }

        if (s.EndsWith("IE", StringComparison.Ordinal) || s.EndsWith("EE", StringComparison.Ordinal))
        {
            ReplaceLastTwo(v, "Y");
        }
        else if (s.EndsWith("DT", StringComparison.Ordinal) || s.EndsWith("RT", StringComparison.Ordinal)
            || s.EndsWith("RD", StringComparison.Ordinal) || s.EndsWith("NT", StringComparison.Ordinal)
            || s.EndsWith("ND", StringComparison.Ordinal))
        {
            ReplaceLastTwo(v, "D");
        }

        var key = new List<string>(v.Count) { v[0] };
        for (int i = 1; i < v.Count; i++)
        {
            string c = v[i];
            string? next = i + 1 < v.Count ? v[i + 1] : null;
            string first;
            string? second = null;

            if (c == "E" && next == "V")
            {
                first = A;
                second = "F";
                i++;
            }
            else if (IsVowel(c))
            {
                first = A;
            }
            else if (c == "S" && next == "C" && i + 2 < v.Count && v[i + 2] == "H")
            {
                first = "S";
                second = "S";
                i += 2;
            }
            else if (c == "P" && next == "H")
            {
                first = "F";
                i++;
            }
            else if (c == "H" && (!IsVowel(v[i - 1]) || next is null || !IsVowel(next)))
            {
                // After a vowel it becomes A; after anything else it repeats that, which then collapses.
                first = IsVowel(v[i - 1]) ? A : v[i - 1];
            }
            else if (c == "W" && IsVowel(v[i - 1]))
            {
                first = v[i - 1];
            }
            else
            {
                first = c switch
                {
                    "Q" => "G",
                    "Z" => "S",
                    "M" => "N",
                    "K" => next == "N" ? "N" : "C",
                    _ => c,
                };
            }

            // jellyfish compares only the replacement's last letter with the key's last, and then
            // appends the replacement whole: "EV" after an A still writes "AF" (#1195).
            if ((second ?? first) != key[key.Count - 1])
            {
                key.Add(first);
                if (second is not null)
                {
                    key.Add(second);
                }
            }
        }

        if (key.Count > 1 && key[key.Count - 1] == "S")
        {
            key.RemoveAt(key.Count - 1);
        }
        if (key.Count >= 2 && key[key.Count - 2] == A && key[key.Count - 1] == "Y")
        {
            key.RemoveAt(key.Count - 2);
        }
        if (key.Count > 1 && key[key.Count - 1] == A)
        {
            key.RemoveAt(key.Count - 1);
        }

        return string.Concat(key);
    }

    // Reached only when the string ends in a two-letter suffix, so two clusters are always there.
    private static void ReplaceLastTwo(List<string> v, string replacement)
    {
        v.RemoveRange(v.Count - 2, 2);
        v.Add(replacement);
    }

    private static bool IsVowel(string c) => c is "A" or "E" or "I" or "O" or "U";
}
