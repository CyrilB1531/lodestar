using System.Text;

namespace Lodestar.Text.Stemming;

// SonarLint S3267: the suffix scans early-return and mutate in place, which Where
// cannot express — and they run per token.
// CA1845 (use span-based string.Concat): that overload does not exist on
// netstandard2.0. The Substring form is what makes this file compile there.
// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input — the
// published algorithm, the reference implementation and the oracle corpus this
// suite is checked against all lowercase first. ToUpperInvariant would return
// different stems, which is a wrong answer rather than a differently-cased one.
#pragma warning disable S3267, CA1845, CA1308

/// <summary>
/// The Romanian Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("romanian")</c>. An
/// original implementation of the published Snowball algorithm, and the only one of
/// the recent languages built on <see cref="RomanceSnowballWorker"/>'s RV. Five
/// steps, of which step 1 loops. Decision 0092 settles the alphabet: the tables
/// carry the cedilla <c>ş</c>/<c>ţ</c>, as the description writes them. Thread-safe.
/// </remarks>
public static class RomanianSnowballStemmer
{
    /// <summary>Returns the Romanian Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'ă' and 'â' are single code points, as the rules expect.
        string s = word.ToLowerInvariant().Normalize(NormalizationForm.FormC);
        if (s.Length < 2)
        {
            return s;
        }
        return new Worker(s).Run();
    }

    private sealed class Worker : RomanceSnowballWorker
    {
        private static readonly Func<char, bool> Vowels = c =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'ă' or 'â' or 'î';

        /// <summary>Whether step 1 or step 2 removed something, which stops step 3.</summary>
        private bool _reduced;

        public Worker(string s) : base(Mark(s), Vowels)
        {
        }

        /// <summary>Upper-cases i and u between vowels, so the regions read them as consonants.</summary>
        /// <remarks>
        /// The scan reads what it has already written: a marked letter is no longer a
        /// vowel, so "aiiu" marks the first i and leaves the second alone.
        /// </remarks>
        private static string Mark(string s)
        {
            char[] a = s.ToCharArray();
            for (int i = 1; i + 1 < a.Length; i++)
            {
                if (!Vowels(a[i - 1]) || !Vowels(a[i + 1]))
                {
                    continue;
                }
                if (a[i] == 'u')
                {
                    a[i] = 'U';
                }
                else if (a[i] == 'i')
                {
                    a[i] = 'I';
                }
            }
            return new string(a);
        }

        private static string Unmark(string s) => s.Replace('I', 'i').Replace('U', 'u');

        public string Run()
        {
            Step0();
            Step1();
            Step2();
            if (!_reduced)
            {
                Step3();
            }
            Step4();
            return Unmark(S);
        }

        private const string Ile = "ile";

        private static readonly string[] Step0Suffixes =
        [
            "iilor", "ului", "elor", "iile", "ilor", "atei", "aţie", "aţia",
            "aua", "ele", "iua", "iei", Ile, "ul", "ea", "ii",
        ];

        private static readonly string[] Step0Delete = ["ului", "ul"];
        private static readonly string[] Step0Chop2 = ["atei", "aua", Ile];
        private static readonly string[] Step0ToE = ["elor", "ele", "ea"];
        private static readonly string[] Step0ToI = ["iilor", "iile", "ilor", "iua", "iei", "ii"];
        private static readonly string[] Step0Chop1 = ["aţie", "aţia"];

        /// <summary>The enclitic article and the plural endings it attaches to, in R1.</summary>
        /// <remarks>
        /// The longest ending wins outright: one that reaches past R1 stops the step
        /// rather than letting a shorter one through, unlike step 3 below.
        /// </remarks>
        private void Step0()
        {
            string? hit = LongestSuffix(Step0Suffixes);
            if (hit is null || !InR1(hit.Length))
            {
                return;
            }
            if (Contains(Step0Delete, hit))
            {
                Delete(hit.Length);
            }
            else if (Contains(Step0Chop2, hit))
            {
                // "abile" keeps its "ab": the exception belongs to "ile" alone.
                if (hit != Ile || !EndsBeforeSuffix("ab", 3))
                {
                    Delete(2);
                }
            }
            else if (Contains(Step0ToE, hit))
            {
                Replace(hit.Length, "e");
            }
            else if (Contains(Step0ToI, hit))
            {
                Replace(hit.Length, "i");
            }
            else if (Contains(Step0Chop1, hit))
            {
                Delete(1);
            }
        }

        private static bool Contains(string[] set, string value)
        {
            foreach (string s in set)
            {
                if (s == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Whether the two letters before a suffix of this length read <paramref name="pair"/>.</summary>
        private bool EndsBeforeSuffix(string pair, int suffixLen)
        {
            int start = S.Length - suffixLen - pair.Length;
            return start >= 0 && string.CompareOrdinal(S, start, pair, 0, pair.Length) == 0;
        }

        private static readonly string[] Step1ToAbil =
            ["abilitate", "abilitati", "abilităţi", "abilităi"];

        private const string Ibilitate = "ibilitate";

        private static readonly string[] Step1ToIv = ["ivitate", "ivitati", "ivităţi", "ivităi"];

        private static readonly string[] Step1ToIc =
        [
            "icitate", "icitati", "icităţi", "icităi", "icatori", "icator",
            "iciva", "icive", "icivi", "icivă", "iciv",
            "icala", "icale", "icali", "icală", "ical",
        ];

        private static readonly string[] Step1ToAt =
        [
            "ativa", "ative", "ativi", "ativă", "ativ", "aţiune", "atoare",
            "atori", "ator", "ătoare", "ători", "ător",
        ];

        private static readonly string[] Step1ToIt =
        [
            "itiva", "itive", "itivi", "itivă", "itiv", "iţiune", "itoare",
            "itori", "itor",
        ];

        private static readonly string[] Step1Suffixes =
            [.. Step1ToAbil, Ibilitate, .. Step1ToIv, .. Step1ToIc, .. Step1ToAt, .. Step1ToIt];

        /// <summary>
        /// The combining derivational endings, reduced until nothing matches: "icitate"
        /// leaves "ic", and a chain like "ivitate" over "ativ" is walked one link a turn.
        /// </summary>
        /// <remarks>
        /// An ending that matches but reaches past R1 clears the flag as well as ending
        /// the loop, so a word stopped part-way through the chain still reaches step 3.
        /// </remarks>
        private void Step1()
        {
            while (true)
            {
                string? hit = LongestSuffix(Step1Suffixes);
                if (hit is null)
                {
                    return;
                }
                if (!InR1(hit.Length))
                {
                    _reduced = false;
                    return;
                }
                _reduced = true;
                if (Contains(Step1ToAbil, hit))
                {
                    Replace(hit.Length, "abil");
                }
                else if (hit == Ibilitate)
                {
                    Replace(hit.Length, "ibil");
                }
                else if (Contains(Step1ToIv, hit))
                {
                    Replace(hit.Length, "iv");
                }
                else if (Contains(Step1ToIc, hit))
                {
                    Replace(hit.Length, "ic");
                }
                else if (Contains(Step1ToAt, hit))
                {
                    Replace(hit.Length, "at");
                }
                else
                {
                    Replace(hit.Length, "it");
                }
            }
        }

        private static readonly string[] Step2ToIst =
            ["isme", "ista", "iste", "isti", "istă", "işti", "ism", "ist"];

        private static readonly string[] Step2Iune = ["iune", "iuni"];

        private static readonly string[] Step2Suffixes =
        [
            "abila", "abile", "abili", "abilă", "ibila", "ibile", "ibili", "ibilă",
            "atori", "itate", "itati", "ităţi",
            "abil", "ibil", "oasa", "oasă", "oase", "anta", "ante", "anti", "antă",
            "ator", "ităi", "iune", "iuni", "isme", "ista", "iste", "isti", "istă",
            "işti", "ata", "ată", "ati", "ate", "uta", "ută", "uti", "ute", "ita",
            "ită", "iti", "ite", "ica", "ice", "ici", "ică", "osi", "oşi", "ant",
            "iva", "ive", "ivi", "ivă", "ism", "ist", "at", "ut", "it", "ic", "os", "iv",
        ];

        /// <summary>The standard endings, all of them measured against R2.</summary>
        /// <remarks>
        /// "iune"/"iuni" are the odd pair: they are not removed, they turn a preceding
        /// "ţ" into "t" — "naţiune" to "naţiun" is step 4's work, not this step's.
        /// </remarks>
        private void Step2()
        {
            string? hit = LongestSuffix(Step2Suffixes);
            if (hit is null || !InR2(hit.Length))
            {
                return;
            }
            _reduced = true;
            if (Contains(Step2Iune, hit))
            {
                if (S.Length >= 5 && S[S.Length - 5] == 'ţ')
                {
                    Replace(5, "t");
                }
            }
            else if (Contains(Step2ToIst, hit))
            {
                Replace(hit.Length, "ist");
            }
            else
            {
                Delete(hit.Length);
            }
        }

        private static readonly string[] Step3Free =
        [
            "seserăţi", "seserăm", "serăţi", "seseşi", "seseră", "serăm", "sesem",
            "seşi", "seră", "sese", "aţi", "eţi", "iţi", "âţi", "sei", "ăm", "em",
            "im", "âm", "se",
        ];

        private static readonly string[] Step3Suffixes =
        [
            "seserăţi", "aserăţi", "iserăţi", "âserăţi", "userăţi", "seserăm",
            "aserăm", "iserăm", "âserăm", "userăm", "serăţi", "seseşi", "seseră",
            "ească", "arăţi", "urăţi", "irăţi", "ârăţi", "aseşi", "aseră", "iseşi",
            "iseră", "âseşi", "âseră", "useşi", "useră", "serăm", "sesem", "indu",
            "ându", "ează", "eşti", "eşte", "ăşti", "ăşte", "eaţi", "iaţi", "arăm",
            "urăm", "irăm", "ârăm", "asem", "isem", "âsem", "usem", "seşi", "seră",
            "sese", "are", "ere", "ire", "âre", "ind", "ând", "eze", "ezi", "esc",
            "ăsc", "eam", "eai", "eau", "iam", "iai", "iau", "aşi", "ară", "uşi",
            "ură", "işi", "iră", "âşi", "âră", "ase", "ise", "âse", "use", "aţi",
            "eţi", "iţi", "âţi", "sei", "ez", "am", "ai", "au", "ea", "ia", "ui",
            "âi", "ăm", "em", "im", "âm", "se",
        ];

        /// <summary>
        /// The verb endings, and the only step whose region qualifies the search: an
        /// ending that reaches past RV falls through to a shorter one, which is what
        /// takes "casem" to "cas" by way of "em" rather than stopping at "asem".
        /// </summary>
        /// <remarks>
        /// The endings outside <c>Step3Free</c> also need the letter before them to be
        /// a consonant — by a list that leaves <c>u</c> out, so it is not the algorithm's
        /// own vowel set.
        /// </remarks>
        private void Step3()
        {
            string? hit = LongestSuffixInRv(Step3Suffixes);
            if (hit is null)
            {
                return;
            }
            if (Contains(Step3Free, hit))
            {
                Delete(hit.Length);
                return;
            }
            int before = S.Length - hit.Length - 1;
            if (before >= Rv && !IsBlockingVowel(S[before]))
            {
                Delete(hit.Length);
            }
        }

        private static bool IsBlockingVowel(char c) =>
            c is 'a' or 'e' or 'i' or 'o' or 'ă' or 'â' or 'î';

        private static readonly string[] Step4Suffixes = ["ie", "a", "e", "i", "ă"];

        /// <summary>The final vowel, in RV.</summary>
        private void Step4()
        {
            string? hit = LongestSuffix(Step4Suffixes);
            if (hit is not null && InRv(hit.Length))
            {
                Delete(hit.Length);
            }
        }
    }
}
