using System.Text;

namespace Lodestar.Text.Stemming;

// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithm, the reference implementations and the oracle corpus
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
// The suffix scans this file's siblings suppress S3267 for live in the base class.
#pragma warning disable CA1308
// SonarLint S3267: DeleteAfterAny early-returns on the first preceder that matches
// and mutates the word in place, neither of which Where can express -- and it runs
// per token, where a LINQ pipeline would allocate on every call.
#pragma warning disable S3267

/// <summary>
/// The Finnish Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("finnish")</c>. An
/// original implementation of the published Snowball algorithm: six steps over
/// standard R1 and R2, with no floor and no RV region — see
/// <c>docs/equivalence.md</c>'s stemming row. Input is lowercased. Thread-safe.
/// </remarks>
public static class FinnishSnowballStemmer
{
    /// <summary>Returns the Finnish Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'ä' and 'ö' are single code points, as the rules expect.
        string s = word.ToLowerInvariant().Normalize(NormalizationForm.FormC);
        if (s.Length < 2)
        {
            return s;
        }
        return new Worker(s).Run();
    }

    private sealed class Worker : SnowballWorkerBase
    {
        private static readonly Func<char, bool> Vowels = c =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'ä' or 'ö';

        /// <summary>Whether step 3 removed an ending, which is what step 5 branches on.</summary>
        private bool _caseRemoved;

        public Worker(string s) : base(s, Vowels)
        {
        }

        public string Run()
        {
            Step1();
            Step2();
            Step3();
            Step4();
            Step5();
            Step6();
            return S;
        }

        // V: the vowels the case rules restrict themselves to. 'y' is a vowel of
        // the language but not one of these, which is why it has its own test.
        private static bool IsRestrictedVowel(char c) =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'ä' or 'ö';

        // c: an ASCII letter that is not a vowel. 'ä' and 'ö' are not ASCII, so
        // the range test excludes them without naming them.
        private static bool IsConsonant(char c) => c is >= 'a' and <= 'z' && !Vowels(c);

        /// <summary>Whether the two characters ending at <paramref name="end"/> are a long vowel.</summary>
        private bool EndsLongVowelAt(int end) =>
            end >= 1 && S[end] == S[end - 1] && IsRestrictedVowel(S[end]);

        /// <summary>Whether the character at <paramref name="i"/> exists and lies in R1.</summary>
        private bool InsideR1(int i) => i >= R1 && i < S.Length;

        private const string Sti = "sti";

        private static readonly string[] Step1Suffixes =
            ["kin", "kaan", "kään", "ko", "kö", "han", "hän", "pa", "pä", Sti];

        /// <summary>Particles, each admitted only after an n, a t or a vowel — and "sti" only in R2.</summary>
        private void Step1()
        {
            string? hit = LongestSuffixInR1(Step1Suffixes);
            if (hit is null)
            {
                return;
            }
            if (hit == Sti)
            {
                if (InR2(hit.Length))
                {
                    Delete(hit.Length);
                }
                return;
            }
            int before = S.Length - hit.Length - 1;
            if (before >= 0 && (S[before] is 'n' or 't' || IsVowel(S[before])))
            {
                Delete(hit.Length);
            }
        }

        private const string Si = "si";
        private const string Ni = "ni";
        private const string An = "an";
        private const string Aan = "än";
        private const string En = "en";

        private static readonly string[] Step2Suffixes =
            [Si, Ni, "nsa", "nsä", "mme", "nne", An, Aan, En];

        private static readonly string[] BackVowelCases = ["ta", "ssa", "sta", "lla", "lta", "na"];
        private static readonly string[] FrontVowelCases = ["tä", "ssä", "stä", "llä", "ltä", "nä"];
        private static readonly string[] ComitativeCases = ["lle", "ine"];

        /// <summary>
        /// The possessives. Three of them attach only behind a case ending, and
        /// vowel harmony is what decides which list applies.
        /// </summary>
        private void Step2()
        {
            string? hit = LongestSuffixInR1(Step2Suffixes);
            if (hit is null)
            {
                return;
            }
            switch (hit)
            {
                case Si:
                    // The one possessive with a letter it refuses to follow.
                    if (!Preceded(hit, "k"))
                    {
                        Delete(hit.Length);
                    }
                    return;
                case Ni:
                    Delete(hit.Length);
                    // "kse" is what the stem of a -ksi word looks like once -ni is off.
                    if (Ends("kse"))
                    {
                        Replace(3, "ksi");
                    }
                    return;
                case An:
                    DeleteAfterAny(hit, BackVowelCases);
                    return;
                case Aan:
                    DeleteAfterAny(hit, FrontVowelCases);
                    return;
                case En:
                    DeleteAfterAny(hit, ComitativeCases);
                    return;
                default:
                    Delete(hit.Length);
                    return;
            }
        }

        /// <summary>Whether <paramref name="text"/> sits immediately before <paramref name="suffix"/>.</summary>
        private bool Preceded(string suffix, string text)
        {
            int start = S.Length - suffix.Length - text.Length;
            return start >= 0 && string.CompareOrdinal(S, start, text, 0, text.Length) == 0;
        }

        private void DeleteAfterAny(string suffix, string[] preceders)
        {
            foreach (string pre in preceders)
            {
                if (Preceded(suffix, pre))
                {
                    Delete(suffix.Length);
                    return;
                }
            }
        }

        private const string Seen = "seen";
        private const string N = "n";
        private const string Tta = "tta";
        private const string Ttae = "ttä";

        // hXn, written out: the vowel between the h and the n has to be repeated
        // in front of the whole ending, which is what makes these seven and not one.
        private static readonly string[] Illatives =
            ["han", "hen", "hin", "hon", "hun", "hän", "hön"];

        private static readonly string[] PluralCases = ["siin", "den", "tten"];

        private static readonly string[] PlainCases =
        [
            "ta", "tä", "ssa", "ssä", "sta", "stä", "lla", "llä",
            "lta", "ltä", "lle", "na", "nä", "ksi", "ine",
        ];

        private static readonly string[] Partitives = ["a", "ä"];

        private static readonly string[] Step3Suffixes =
        [
            .. Illatives, .. PluralCases, Seen, .. Partitives, .. PlainCases, Tta, Ttae, N,
        ];

        /// <summary>
        /// The cases. Every group but one carries a condition on what precedes it,
        /// and a condition that fails leaves the word alone rather than falling
        /// through to a shorter ending.
        /// </summary>
        private void Step3()
        {
            string? hit = LongestSuffixInR1(Step3Suffixes);
            if (hit is null)
            {
                return;
            }
            int before = S.Length - hit.Length - 1;
            if (!Step3Allows(hit, before))
            {
                // A failed condition ends the search, except on the four longer
                // spellings of the genitive -- decision 0090 has the measurement.
                if (Array.IndexOf(GenitiveSpellings, hit) >= 0)
                {
                    StripGenitive();
                }
                return;
            }
            if (hit == N)
            {
                StripGenitive();
                return;
            }
            Delete(hit.Length);
            _caseRemoved = true;
        }

        private static readonly string[] GenitiveSpellings = [.. PluralCases, Seen];

        /// <summary>Removes a final n, and the last vowel when that uncovers a long vowel or "ie".</summary>
        private void StripGenitive()
        {
            Delete(1);
            _caseRemoved = true;
            DropVowelAfterGenitive();
        }

        /// <summary>Whether the condition attached to <paramref name="hit"/> holds.</summary>
        private bool Step3Allows(string hit, int before)
        {
            if (Array.IndexOf(Illatives, hit) >= 0)
            {
                // The vowel in the middle of the ending, repeated in front of it.
                return InsideR1(before) && (S[before] == hit[1] || S[before] == '\'');
            }
            if (Array.IndexOf(PluralCases, hit) >= 0)
            {
                return InsideR1(before)
                    && (S[before] == '\''
                        || (S[before] == 'i' && before >= 1 && IsRestrictedVowel(S[before - 1])));
            }
            if (hit == Seen)
            {
                return InsideR1(before - 1) && EndsLongVowelAt(before);
            }
            if (Array.IndexOf(Partitives, hit) >= 0)
            {
                return before >= 1 && IsConsonant(S[before - 1]) && IsVowel(S[before]);
            }
            if (hit == Tta || hit == Ttae)
            {
                return before >= 0 && S[before] == 'e';
            }
            return true;
        }

        /// <summary>The genitive takes the last vowel with it when it uncovers a long vowel or "ie".</summary>
        private void DropVowelAfterGenitive()
        {
            int last = S.Length - 1;
            bool ie = last >= 1 && S[last] == 'e' && S[last - 1] == 'i';
            if (last >= 1 && (EndsLongVowelAt(last) || ie))
            {
                Delete(1);
            }
        }

        private const string Po = "po";
        private const string Mma = "mma";
        private const string Imma = "imma";

        private static readonly string[] Comparatives = ["mpi", "mpa", "mpä", "mmi", Mma, "mmä"];

        private static readonly string[] Superlatives =
            ["impi", "impa", "impä", "immi", Imma, "immä", "eja", "ejä"];

        private static readonly string[] Step4Suffixes = [.. Comparatives, .. Superlatives];

        /// <summary>The comparative and superlative endings, and the one stem that keeps them.</summary>
        private void Step4()
        {
            string? hit = LongestSuffixInR2(Step4Suffixes);
            if (hit is null)
            {
                return;
            }
            if (Array.IndexOf(Comparatives, hit) >= 0 && Preceded(hit, Po))
            {
                return;
            }
            Delete(hit.Length);
        }

        private static readonly string[] PluralComparatives = [Imma, Mma];

        /// <summary>
        /// The plurals, which read step 3's outcome: an i or a j after a case was
        /// removed, a t and what it uncovers when none was.
        /// </summary>
        private void Step5()
        {
            if (_caseRemoved)
            {
                if (S.Length > 0 && S[S.Length - 1] is 'i' or 'j' && InR1(1))
                {
                    Delete(1);
                }
                return;
            }
            if (!Ends("t") || !InR1(1) || S.Length < 2 || !IsVowel(S[S.Length - 2]))
            {
                return;
            }
            Delete(1);
            string? hit = LongestSuffixInR2(PluralComparatives);
            if (hit is not null && !(hit == Mma && Preceded(hit, Po)))
            {
                Delete(hit.Length);
            }
        }

        private static readonly char[] TidyVowels = ['a', 'ä', 'e', 'i'];

        /// <summary>
        /// Tidying up: four tests inside R1, taken in turn, then two over the whole
        /// word. Each fires at most once, so "kotimaahan" loses its long vowel to
        /// (a) and then its "ma" to (b), ending at "kotim".
        /// </summary>
        private void Step6()
        {
            int last = S.Length - 1;
            if (InR1(2) && EndsLongVowelAt(last))
            {
                Delete(1);
            }
            last = S.Length - 1;
            if (InR1(2) && last >= 1 && Array.IndexOf(TidyVowels, S[last]) >= 0 && IsConsonant(S[last - 1]))
            {
                Delete(1);
            }
            if (InR1(2) && (Ends("oj") || Ends("uj")))
            {
                Delete(1);
            }
            if (InR1(2) && Ends("jo"))
            {
                Delete(1);
            }
            StripDoubledConsonant();
            if (Ends("'"))
            {
                Delete(1);
            }
        }

        /// <summary>
        /// A doubled consonant loses its second letter, whatever vowels trail it —
        /// the one test in this step that is not restricted to R1.
        /// </summary>
        private void StripDoubledConsonant()
        {
            int i = S.Length - 1;
            while (i >= 0 && IsVowel(S[i]))
            {
                i--;
            }
            if (i >= 1 && IsConsonant(S[i]) && S[i] == S[i - 1])
            {
                S = S.Remove(i, 1);
            }
        }
    }
}
