using System.Text;

namespace Lodestar.Text.Stemming;

// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithm, the reference implementations and the oracle corpus
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
// The suffix scans this file's siblings suppress S3267 for live in the base class.
#pragma warning disable CA1308

/// <summary>
/// The Swedish Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("swedish")</c>. An
/// original implementation of the published Snowball algorithm: three steps that
/// each search R1, no R2 and no RV region, R1 floored at three letters as in
/// German, and nothing marked or folded — see <c>docs/equivalence.md</c>'s
/// stemming row. Input is lowercased. Thread-safe.
/// </remarks>
public static class SwedishSnowballStemmer
{
    /// <summary>Returns the Swedish Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'ä' etc. are single code points, as the rules expect.
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
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'ä' or 'å' or 'ö';

        // The region before R1 must hold at least three letters, as in German.
        public Worker(string s) : base(s, Vowels, minR1: 3)
        {
        }

        public string Run()
        {
            Step1();
            Step2();
            Step3();
            return S;
        }

        // 'o' is the one vowel in the set; the letter it tests need not itself be in R1.
        private static bool IsValidSEnding(char c) =>
            c is 'b' or 'c' or 'd' or 'f' or 'g' or 'h' or 'j' or 'k' or 'l' or 'm'
              or 'n' or 'o' or 'p' or 'r' or 't' or 'v' or 'y';

        private const string BareS = "s";

        // Group (b)'s bare s is searched alongside group (a), not after it: "arens"
        // reaches neither "arens" nor "ens" inside R1, and it is the s that goes.
        private static readonly string[] Step1Suffixes =
        [
            "a", "arna", "erna", "heterna", "orna", "ad", "e", "ade", "ande", "arne",
            "are", "aste", "en", "anden", "aren", "heten", "ern", "ar", "er", "heter",
            "or", "as", "arnas", "ernas", "ornas", "es", "ades", "andes", "ens",
            "arens", "hetens", "erns", "at", "andet", "het", "ast",
            BareS,
        ];

        private void Step1()
        {
            string? hit = LongestSuffixInR1(Step1Suffixes);
            if (hit is null)
            {
                return;
            }
            // Group (b): the bare s needs a valid s-ending before it, which is the
            // one letter in these rules that may sit outside R1.
            if (hit == BareS && !(S.Length >= 2 && IsValidSEnding(S[S.Length - 2])))
            {
                return;
            }
            Delete(hit.Length);
        }

        private static readonly string[] Step2Suffixes = ["dd", "gd", "nn", "dt", "gt", "kt", "tt"];

        /// <summary>A consonant pair in R1 loses its second letter: "friskt" ends at "frisk".</summary>
        private void Step2()
        {
            if (LongestSuffixInR1(Step2Suffixes) is not null)
            {
                Delete(1);
            }
        }

        private const string Lost = "löst";
        private const string Fullt = "fullt";

        private static readonly string[] Step3Suffixes = ["lig", "ig", "els", Lost, Fullt];

        /// <summary>Two of the five are rewritten rather than stripped, keeping their stem a word.</summary>
        private void Step3()
        {
            string? hit = LongestSuffixInR1(Step3Suffixes);
            if (hit is null)
            {
                return;
            }
            if (hit == Lost)
            {
                Replace(hit.Length, "lös");
            }
            else if (hit == Fullt)
            {
                Replace(hit.Length, "full");
            }
            else
            {
                Delete(hit.Length);
            }
        }
    }
}
