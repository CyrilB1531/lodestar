using System.Text;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Stemming;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S3267: the suffix scan early-returns, which Where cannot express, and it runs per token.
// CA1845 (use span-based string.Concat): that overload does not exist on
// netstandard2.0. The Substring form is what makes this file compile there.
#pragma warning disable CA1845
// CA1308 (normalize to uppercase): Snowball and Porter are *defined* on
// lowercase input — the published algorithms, the reference implementations and
// the oracle corpora this suite is checked against all lowercase first.
// ToUpperInvariant would return different stems, which is a wrong answer rather
// than a differently-cased one.
#pragma warning disable S3776, S3267, CA1308

/// <summary>
/// The French Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>snowballstemmer.stemmer("french")</c> 3.1.1 (decision 0006). An
/// original implementation of the published Snowball algorithm: elisions, the RV/R1/R2
/// regions and the standard step ordering. Input is lowercased. Thread-safe.
/// </remarks>
public static class FrenchSnowballStemmer
{
    /// <summary>Returns the French Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'è' etc. are single code points, as the rules expect.
        string s = WellFormedNormalization.Normalize(word.ToLowerInvariant(), NormalizationForm.FormC);
        if (s.Length < 2)
        {
            return s;
        }
        return new Worker(s).Run();
    }

    // Each table is ordered longest first, so the first suffix that fits is the longest one across every
    // group of its step, as Snowball's among requires (#973).
    private static readonly string[] StandardSuffixes =
    [
        "issements", "issement", "atrices", "atrice", "ateurs", "ations", "logies", "usions", "utions",
        "ements", "amment", "emment", "ances", "iqUes", "ismes", "ables", "istes", "ateur", "ation",
        "logie", "usion", "ution", "ences", "ement", "euses", "ments", "ance", "iqUe", "isme", "able",
        "iste", "ence", "ités", "ives", "eaux", "euse", "ment", "eux", "ité", "ive", "ifs", "aux", "oux",
        "if",
    ];

    private static readonly string[] IVerbSuffixes =
    [
        "issaIent", "issantes", "iraIent", "issante", "issants", "issions", "irions", "issais", "issait",
        "issant", "issent", "issiez", "issons", "irais", "irait", "irent", "iriez", "irons", "iront",
        "isses", "issez", "îmes", "îtes", "irai", "iras", "irez", "isse", "ies", "ira", "ît", "ie", "ir",
        "is", "it", "i",
    ];

    private static readonly string[] VerbSuffixes =
    [
        "eraIent", "assions", "erions", "assent", "assiez", "èrent", "erais", "erait", "eriez", "erons",
        "eront", "aIent", "antes", "asses", "aises", "ions", "erai", "eras", "erez", "âmes", "âtes", "ante",
        "ants", "asse", "aise", "eais", "ées", "era", "iez", "ais", "ait", "ant", "ée", "és", "er", "ez",
        "ât", "ai", "as", "é", "a",
    ];

    private static readonly string[] ResidualSuffixes = ["ière", "Ière", "ion", "ier", "Ier", "e"];

    private static readonly string[] DoubledEndings = ["eill", "ell", "enn", "onn", "ett"];

    private sealed class Worker
    {
        private string _s;
        private readonly int _rv;
        private readonly int _r1;
        private readonly int _r2;

        public Worker(string s)
        {
            _s = MarkNonVowels(RemoveElision(s));
            _rv = ComputeRv(_s);
            _r1 = Region(_s, 0);
            _r2 = Region(_s, _r1);
        }

        public string Run()
        {
            // Removing -ment, -amment or -emment leaves the standard step unsuccessful, so the verb steps
            // still run, and step 4 does when they remove nothing either.
            if (StandardSuffix() || IVerbSuffix() || VerbSuffix())
            {
                if (_s.EndsWith('Y'))
                {
                    Replace(_s.Length - 1, "i");
                }
                else if (_s.EndsWith('ç'))
                {
                    Replace(_s.Length - 1, "c");
                }
            }
            else
            {
                ResidualSuffix();
            }

            if (EndsWithAny(DoubledEndings))
            {
                Truncate(_s.Length - 1);
            }
            UnAccent();
            return Unmark(_s);
        }

        private static bool IsVowel(char c) =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y'
            or 'â' or 'à' or 'ë' or 'é' or 'ê' or 'è' or 'ï' or 'î' or 'ô' or 'û' or 'ù';

        // "l'avion" stems as "avion": one elidable letter, or "qu", then an apostrophe and something after it.
        private static string RemoveElision(string s)
        {
            int apostrophe = s[0] is 'c' or 'd' or 'j' or 'l' or 'm' or 'n' or 's' or 't' ? 1 : -1;
            if (apostrophe < 0 && s.StartsWith("qu", StringComparison.Ordinal))
            {
                apostrophe = 2;
            }
            return apostrophe > 0 && apostrophe + 1 < s.Length && s[apostrophe] == '\''
                ? s.Substring(apostrophe + 1)
                : s;
        }

        // Scans left to right, reading earlier marks. Diaeresis vowels become H plus the bare vowel, so a
        // suffix can start on that vowel; the H is dropped again at the end if the vowel went.
        private static string MarkNonVowels(string s)
        {
            var b = new StringBuilder(s);
            int p = 0;
            while (p < b.Length)
            {
                if (!MarkAt(b, p))
                {
                    p++;
                }
            }
            return b.ToString();
        }

        private static bool MarkAt(StringBuilder b, int p)
        {
            char c = b[p];
            bool hasNext = p + 1 < b.Length;
            if (IsVowel(c) && hasNext)
            {
                char next = b[p + 1];
                if ((next == 'u' || next == 'i') && p + 2 < b.Length && IsVowel(b[p + 2]))
                {
                    b[p + 1] = next == 'u' ? 'U' : 'I';
                    return true;
                }
                if (next == 'y')
                {
                    b[p + 1] = 'Y';
                    return true;
                }
            }
            if (c == 'ë' || c == 'ï')
            {
                b[p] = 'H';
                b.Insert(p + 1, c == 'ë' ? 'e' : 'i');
                return true;
            }
            if (c == 'y' && hasNext && IsVowel(b[p + 1]))
            {
                b[p] = 'Y';
                return true;
            }
            if (c == 'q' && hasNext && b[p + 1] == 'u')
            {
                b[p + 1] = 'U';
                return true;
            }
            return false;
        }

        private static string Unmark(string s)
        {
            var b = new StringBuilder(s.Length);
            int i = 0;
            while (i < s.Length)
            {
                char c = s[i++];
                if (c != 'H')
                {
                    b.Append(c switch { 'I' => 'i', 'U' => 'u', 'Y' => 'y', _ => c });
                }
                else if (i < s.Length && s[i] is 'e' or 'i')
                {
                    // An H whose vowel was removed goes with it.
                    b.Append(s[i++] == 'e' ? 'ë' : 'ï');
                }
            }
            return b.ToString();
        }

        private static int Region(string s, int from)
        {
            int i = from;
            while (i < s.Length && !IsVowel(s[i]))
            {
                i++;
            }
            while (i < s.Length && IsVowel(s[i]))
            {
                i++;
            }
            return i < s.Length ? i + 1 : s.Length;
        }

        private static int ComputeRv(string s)
        {
            int n = s.Length;
            if (n >= 3
                && ((IsVowel(s[0]) && IsVowel(s[1]))
                    || s.StartsWith("par", StringComparison.Ordinal)
                    || s.StartsWith("col", StringComparison.Ordinal)
                    || s.StartsWith("tap", StringComparison.Ordinal)
                    || (s.StartsWith("ni", StringComparison.Ordinal) && IsVowel(s[2]))))
            {
                return 3;
            }
            for (int i = 1; i < n; i++)
            {
                if (IsVowel(s[i]))
                {
                    return i + 1;
                }
            }
            return n;
        }

        // The longest suffix in the table that starts at or after lowerBound.
        private string? Longest(string[] table, int lowerBound)
        {
            foreach (string suffix in table)
            {
                if (_s.Length - suffix.Length >= lowerBound && Ends(suffix))
                {
                    return suffix;
                }
            }
            return null;
        }

        private bool EndsWithAny(string[] endings)
        {
            foreach (string ending in endings)
            {
                if (Ends(ending))
                {
                    return true;
                }
            }
            return false;
        }

        private bool Ends(string suffix) => _s.EndsWith(suffix, StringComparison.Ordinal);
        private bool EndsAt(int end, string part) =>
            end >= part.Length && string.CompareOrdinal(_s, end - part.Length, part, 0, part.Length) == 0;
        private void Truncate(int start) => _s = _s.Substring(0, start);
        private void Replace(int start, string replacement) => _s = _s.Substring(0, start) + replacement;
        private char Before(int start) => start > 0 ? _s[start - 1] : '\0';

        // After a deletion, an "ic" left at the end goes if it lies in R2, and is marked iqU otherwise.
        private void DeleteOrMarkIc()
        {
            if (Ends("ic"))
            {
                int start = _s.Length - 2;
                if (start >= _r2)
                {
                    Truncate(start);
                }
                else
                {
                    Replace(start, "iqU");
                }
            }
        }

        private bool TruncateIf(bool condition, int start)
        {
            if (condition)
            {
                Truncate(start);
            }
            return condition;
        }

        private bool ReplaceIf(bool condition, int start, string replacement)
        {
            if (condition)
            {
                Replace(start, replacement);
            }
            return condition;
        }

        private bool StandardSuffix()
        {
            string? suffix = Longest(StandardSuffixes, 0);
            if (suffix is null)
            {
                return false;
            }
            int start = _s.Length - suffix.Length;
            switch (suffix)
            {
                case "atrice" or "ateur" or "ation" or "atrices" or "ateurs" or "ations":
                    if (!TruncateIf(start >= _r2, start))
                    {
                        return false;
                    }
                    DeleteOrMarkIc();
                    return true;
                case "logie" or "logies":
                    return ReplaceIf(start >= _r2, start, "log");
                case "usion" or "ution" or "usions" or "utions":
                    return ReplaceIf(start >= _r2, start, "u");
                case "ence" or "ences":
                    return ReplaceIf(start >= _r2, start, "ent");
                case "ement" or "ements":
                    if (!TruncateIf(start >= _rv, start))
                    {
                        return false;
                    }
                    AfterEment();
                    return true;
                case "ité" or "ités":
                    if (!TruncateIf(start >= _r2, start))
                    {
                        return false;
                    }
                    AfterIte();
                    return true;
                case "if" or "ive" or "ifs" or "ives":
                    if (!TruncateIf(start >= _r2, start))
                    {
                        return false;
                    }
                    if (Ends("at") && _s.Length - 2 >= _r2)
                    {
                        Truncate(_s.Length - 2);
                        DeleteOrMarkIc();
                    }
                    return true;
                case "eaux":
                    Replace(start, "eau");
                    return true;
                case "aux":
                    return ReplaceIf(start >= _r1, start, "al");
                case "oux":
                    return ReplaceIf(Before(start) is 'b' or 'h' or 'j' or 'l' or 'n' or 'p', start, "ou");
                case "euse" or "euses":
                    return TruncateIf(start >= _r2, start) || ReplaceIf(start >= _r1, start, "eux");
                case "issement" or "issements":
                    return TruncateIf(start >= _r1 && start > 0 && !IsVowel(_s[start - 1]), start);
                case "amment":
                    ReplaceIf(start >= _rv, start, "ant");
                    return false;
                case "emment":
                    ReplaceIf(start >= _rv, start, "ent");
                    return false;
                case "ment" or "ments":
                    TruncateIf(start > 0 && IsVowel(_s[start - 1]) && start - 1 >= _rv, start);
                    return false;
                default:
                    // iqUe, ance, able, isme, iste, eux and their plurals.
                    return TruncateIf(start >= _r2, start);
            }
        }

        private void AfterEment()
        {
            int end = _s.Length;
            if (EndsAt(end, "iv"))
            {
                if (TruncateIf(end - 2 >= _r2, end - 2) && Ends("at"))
                {
                    TruncateIf(_s.Length - 2 >= _r2, _s.Length - 2);
                }
            }
            else if (EndsAt(end, "eus"))
            {
                _ = TruncateIf(end - 3 >= _r2, end - 3) || ReplaceIf(end - 3 >= _r1, end - 3, "eux");
            }
            else if (EndsAt(end, "abl") || EndsAt(end, "iqU"))
            {
                TruncateIf(end - 3 >= _r2, end - 3);
            }
            else if (EndsAt(end, "ièr") || EndsAt(end, "Ièr"))
            {
                ReplaceIf(end - 3 >= _rv, end - 3, "i");
            }
        }

        private void AfterIte()
        {
            int end = _s.Length;
            if (EndsAt(end, "abil"))
            {
                if (!TruncateIf(end - 4 >= _r2, end - 4))
                {
                    Replace(end - 4, "abl");
                }
            }
            else if (EndsAt(end, "ic"))
            {
                DeleteOrMarkIc();
            }
            else if (EndsAt(end, "iv"))
            {
                TruncateIf(end - 2 >= _r2, end - 2);
            }
        }

        // Step 2a: the suffix lies in RV and follows a non-vowel that also lies in RV, other than a diaeresis mark.
        private bool IVerbSuffix()
        {
            string? suffix = Longest(IVerbSuffixes, _rv);
            if (suffix is null)
            {
                return false;
            }
            int start = _s.Length - suffix.Length;
            return TruncateIf(start > _rv && _s[start - 1] != 'H' && !IsVowel(_s[start - 1]), start);
        }

        // Step 2b: the suffix lies in RV.
        private bool VerbSuffix()
        {
            string? suffix = Longest(VerbSuffixes, _rv);
            if (suffix is null)
            {
                return false;
            }
            int start = _s.Length - suffix.Length;
            switch (suffix)
            {
                case "ions":
                    return TruncateIf(start >= _r2, start);
                case "a" or "ai" or "as" or "ait" or "ant" or "ants" or "ante" or "antes" or "asse" or "asses"
                    or "assent" or "assions" or "assiez" or "âmes" or "âtes" or "ât" or "aIent":
                    // A preceding "e" in RV goes with the suffix.
                    Truncate(Before(start) == 'e' && start - 1 >= _rv ? start - 1 : start);
                    return true;
                case "ais" or "aise" or "aises":
                    // "épl-", "auv-" and a single letter before "al-" keep it: "chauvais", "balais".
                    if (EndsAt(start, "épl") || EndsAt(start, "auv") || (EndsAt(start, "al") && start == 3))
                    {
                        return false;
                    }
                    Truncate(start);
                    return true;
                default:
                    Truncate(start);
                    return true;
            }
        }

        // Step 4.
        private void ResidualSuffix()
        {
            int last = _s.Length - 1;
            if (last >= 1 && _s[last] == 's'
                && (EndsAt(last, "Hi") || _s[last - 1] is not ('a' or 'i' or 'o' or 's' or 'u' or 'è')))
            {
                Truncate(last);
            }

            string? suffix = Longest(ResidualSuffixes, _rv);
            if (suffix is null)
            {
                return;
            }
            int start = _s.Length - suffix.Length;
            switch (suffix)
            {
                case "ion":
                    TruncateIf(start >= _r2 && start > _rv && _s[start - 1] is 's' or 't', start);
                    return;
                case "e":
                    Truncate(start);
                    return;
                default:
                    Replace(start, "i");
                    return;
            }
        }

        // Step 6: an é or è followed by at least one non-vowel, and nothing but non-vowels, loses its accent.
        private void UnAccent()
        {
            int i = _s.Length - 1;
            while (i >= 0 && !IsVowel(_s[i]))
            {
                i--;
            }
            if (i < _s.Length - 1 && i >= 0 && (_s[i] == 'é' || _s[i] == 'è'))
            {
                _s = _s.Substring(0, i) + "e" + _s.Substring(i + 1);
            }
        }
    }
}
