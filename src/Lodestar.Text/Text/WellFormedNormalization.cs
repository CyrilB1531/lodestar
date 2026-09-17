using System.Text;

namespace Lodestar.Text.Internal;

/// <summary>Unicode normalization that tolerates a lone surrogate, as Python's does.</summary>
internal static class WellFormedNormalization
{
    /// <summary>
    /// Normalizes <paramref name="s"/> to <paramref name="form"/>, leaving each unpaired
    /// surrogate in place and normalizing the well-formed runs between them.
    /// </summary>
    /// <remarks>
    /// <see cref="string.Normalize(NormalizationForm)"/> throws on invalid UTF-16, where
    /// <c>unicodedata.normalize</c> treats a lone surrogate as an unassigned starter. A
    /// starter blocks reordering and composition across it, so normalizing each run apart
    /// gives Python's result (#880).
    /// </remarks>
    public static string Normalize(string s, NormalizationForm form)
    {
        int lone = NextLoneSurrogate(s, 0);
        if (lone < 0)
        {
            return s.Normalize(form);
        }

        var sb = new StringBuilder(s.Length);
        int start = 0;
        while (lone >= 0)
        {
            sb.Append(s.Substring(start, lone - start).Normalize(form)).Append(s[lone]);
            start = lone + 1;
            lone = NextLoneSurrogate(s, start);
        }
        return sb.Append(s.Substring(start).Normalize(form)).ToString();
    }

    private static int NextLoneSurrogate(string s, int from)
    {
        int i = from;
        while (i < s.Length)
        {
            char c = s[i];
            if (char.IsHighSurrogate(c) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]))
            {
                i += 2;
            }
            else if (char.IsSurrogate(c))
            {
                return i;
            }
            else
            {
                i++;
            }
        }
        return -1;
    }
}
