using System.Text;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Phonetics;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S127: a surrogate pair advances the loop variable past its low half.
#pragma warning disable S3776, S127

/// <summary>
/// Match Rating Approach: a phonetic codex, and the rule for deciding whether two
/// codices name a match (Western Airlines, 1977).
/// </summary>
/// <remarks>
/// Reference behavior: <c>jellyfish.match_rating_codex</c> and <c>match_rating_comparison</c> 1.2.1,
/// over the full uppercase mapping, by grapheme cluster, lengths in UTF-8 bytes. A character neither
/// alphabetic nor a space is <b>refused</b>: <see cref="Codex(string)"/> throws and
/// <see cref="Compare(string, string)"/> returns <c>null</c>. Thread-safe.
/// </remarks>
public static class MatchRatingApproach
{
    /// <summary>Encodes a string to its Match Rating codex.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> holds a character that is
    /// neither alphabetic nor a space.</exception>
    public static string Codex(string value)
    {
        Guard.NotNull(value);
        return Codex(value.AsSpan());
    }

    /// <summary>Encodes <paramref name="value"/> to its Match Rating codex (or empty).</summary>
    /// <exception cref="ArgumentException"><paramref name="value"/> holds a character that is
    /// neither alphabetic nor a space.</exception>
    public static string Codex(ReadOnlySpan<char> value) =>
        TryCodex(value, out List<int> codex, out _, out string? refused)
            ? FromScalars(codex)
            : throw new ArgumentException(refused, nameof(value));

    /// <summary>
    /// Compares the Match Rating codices of two names. Returns <c>null</c>, rather than
    /// <c>false</c>, when either holds a character that is neither alphabetic nor a space, or when
    /// the codices' lengths differ too much for a rating to mean anything.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="a"/> or <paramref name="b"/> is null.</exception>
    public static bool? Compare(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return Compare(a.AsSpan(), b.AsSpan());
    }

    /// <summary>
    /// Compares the Match Rating codices of <paramref name="a"/> and <paramref name="b"/>.
    /// Returns <c>null</c>, rather than <c>false</c>, when either holds a character that is neither
    /// alphabetic nor a space, or when the codices' lengths differ too much for a rating to mean
    /// anything.
    /// </summary>
    public static bool? Compare(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        if (!TryCodex(a, out List<int> codexA, out int bytesA, out _)
            || !TryCodex(b, out List<int> codexB, out int bytesB, out _))
        {
            return null;
        }

        // Lengths 3 or more apart cannot be rated at all — measured against jellyfish 1.2.1.
        if (Math.Abs(bytesA - bytesB) >= 3)
        {
            return null;
        }

        // jellyfish cancels from the longer codex's side; the count is symmetric but the residue is not.
        (List<int> longer, List<int> shorter) = bytesA > bytesB ? (codexA, codexB) : (codexB, codexA);
        return SimilarityRating(longer, shorter) >= MinimumRating(bytesA + bytesB);
    }

    /// <summary>The codex as scalars and its UTF-8 length, or the message refusing the input.</summary>
    private static bool TryCodex(ReadOnlySpan<char> value, out List<int> codex, out int bytes, out string? refused)
    {
        string s = PhoneticText.ToUpperFull(value);
        for (int i = 0; i < s.Length; i++)
        {
            bool pair = char.IsSurrogatePair(s, i);
            if (s[i] != ' ' && !PhoneticText.IsAlphabetic(s, i))
            {
                int scalar = pair ? char.ConvertToUtf32(s, i) : s[i];
                codex = [];
                bytes = 0;
                refused = $"U+{scalar:X4} is neither alphabetic nor a space.";
                return false;
            }
            if (pair)
            {
                i++;
            }
        }

        // Compared against the *raw* previous cluster, not the previously kept one, so
        // doubles collapse even across a dropped vowel ("Mississippi" -> "MSSP").
        var kept = new List<int>(s.Length);
        string previous = string.Empty;
        List<string> clusters = PhoneticText.Graphemes(s);
        for (int i = 0; i < clusters.Count; i++)
        {
            string c = clusters[i];
            bool isVowel = c is "A" or "E" or "I" or "O" or "U";
            if (i == 0 || (!isVowel && c != previous))
            {
                AddScalars(c, kept);
            }
            previous = c;
        }

        bytes = 0;
        foreach (int cp in kept)
        {
            bytes += Utf8Length(cp);
        }

        // Six bytes, not six letters: jellyfish measures the codex in UTF-8 and then keeps up to
        // three characters from each end, so two four-byte letters come back twice.
        if (bytes > 6)
        {
            int take = Math.Min(3, kept.Count);
            List<int> truncated = kept.GetRange(0, take);
            truncated.AddRange(kept.GetRange(kept.Count - take, take));
            kept = truncated;
            bytes = 0;
            foreach (int cp in kept)
            {
                bytes += Utf8Length(cp);
            }
        }

        codex = kept;
        refused = null;
        return true;
    }

    private static int Utf8Length(int cp) => cp switch
    {
        < 0x80 => 1,
        < 0x800 => 2,
        < 0x10000 => 3,
        _ => 4,
    };

    private static void AddScalars(string s, List<int> scalars)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsSurrogatePair(s, i))
            {
                scalars.Add(char.ConvertToUtf32(s[i], s[i + 1]));
                i++;
            }
            else
            {
                scalars.Add(s[i]);
            }
        }
    }

    private static string FromScalars(List<int> scalars)
    {
        var sb = new StringBuilder(scalars.Count);
        foreach (int cp in scalars)
        {
            if (cp > 0xFFFF)
            {
                sb.Append(char.ConvertFromUtf32(cp));
            }
            else
            {
                sb.Append((char)cp);
            }
        }
        return sb.ToString();
    }

    // Combined-codex-length bucket, coarser the longer the codices are — measured by
    // bisection against jellyfish 1.2.1, not a textbook table (see the reference page).
    private static int MinimumRating(int combinedLength) => combinedLength switch
    {
        <= 4 => 5,
        <= 7 => 4,
        <= 11 => 3,
        _ => 2,
    };

    // Cancel same-index characters from the start, then cancel again over what is left,
    // from the end. What survives both passes on the longer side is the unmatched count.
    private static int SimilarityRating(List<int> longer, List<int> shorter)
    {
        var residualA = new List<int>();
        var residualB = new List<int>();
        int n = Math.Max(longer.Count, shorter.Count);
        for (int i = 0; i < n; i++)
        {
            bool hasA = i < longer.Count;
            bool hasB = i < shorter.Count;
            if (hasA && hasB && longer[i] == shorter[i])
            {
                continue;
            }
            if (hasA)
            {
                residualA.Add(longer[i]);
            }
            if (hasB)
            {
                residualB.Add(shorter[i]);
            }
        }

        int unmatchedA = 0;
        int unmatchedB = 0;
        n = Math.Max(residualA.Count, residualB.Count);
        for (int i = 0; i < n; i++)
        {
            int ia = residualA.Count - 1 - i;
            int ib = residualB.Count - 1 - i;
            if (ia >= 0 && ib >= 0 && residualA[ia] == residualB[ib])
            {
                continue;
            }
            if (ia >= 0)
            {
                unmatchedA++;
            }
            if (ib >= 0)
            {
                unmatchedB++;
            }
        }
        return 6 - Math.Max(unmatchedA, unmatchedB);
    }
}
