namespace Lodestar.Text.Stemming;

/// <summary>
/// The Danish Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("danish")</c>. An
/// original implementation of the published Snowball algorithm: four steps that
/// each search R1, no R2 and no RV region, R1 floored at three as in Swedish,
/// and a final undoubling step Swedish has not. The published apostrophe rule is
/// absent by decision 0087 — see <c>docs/equivalence.md</c>. Input is lowercased. Thread-safe.
/// </remarks>
public static class DanishSnowballStemmer
{
    /// <summary>Returns the Danish Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word) =>
        ScandinavianSnowballWorker.Stem(word, static s => new Worker(s));

    private sealed class Worker : ScandinavianSnowballWorker
    {
        private static readonly Func<char, bool> Vowels = c =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'æ' or 'å' or 'ø';

        public Worker(string s) : base(s, Vowels)
        {
        }

        protected override void Run()
        {
            Step1();
            Step2();
            Step3();
            Step4();
        }

        // 'a', 'o', 'y' and 'å' are the four vowels in the set; the letter it tests
        // need not itself be in R1.
        private static bool IsValidSEnding(char c) =>
            c is 'a' or 'b' or 'c' or 'd' or 'f' or 'g' or 'h' or 'j' or 'k' or 'l'
              or 'm' or 'n' or 'o' or 'p' or 'r' or 't' or 'v' or 'y' or 'z' or 'å';

        private static readonly string[] Step1Suffixes =
        [
            "hed", "ethed", "ered", "e", "erede", "ende", "erende", "ene", "erne",
            "ere", "en", "heden", "eren", "er", "heder", "erer", "heds", "es",
            "endes", "erendes", "enes", "ernes", "eres", "ens", "hedens", "erens",
            "ers", "ets", "erets", "et", "eret",
            BareS,
        ];

        private void Step1() => StripLongestInR1(Step1Suffixes, IsValidSEnding);

        private static readonly string[] Step2Suffixes = ["gd", "dt", "gt", "kt"];

        private void Step2() => StripConsonantPair(Step2Suffixes);

        private const string Igst = "igst";
        private const string Lost = "løst";

        private static readonly string[] Step3Suffixes = ["ig", "lig", "elig", "els", Lost];

        /// <summary>
        /// The one step that runs another: a derivational ending deleted here can
        /// uncover a consonant pair, so step 2 is repeated afterwards.
        /// </summary>
        private void Step3()
        {
            // The igst rule carries no region condition, and it runs before the search
            // rather than as one of its cases: "hurtigst" has to reach "hurtig" first.
            if (Ends(Igst))
            {
                Delete(2);
            }
            string? hit = LongestSuffixInR1(Step3Suffixes);
            if (hit is null)
            {
                return;
            }
            if (hit == Lost)
            {
                Replace(hit.Length, "løs");
                return;
            }
            Delete(hit.Length);
            Step2();
        }

        private static readonly string[] DoubleConsonants =
            ["bb", "dd", "ff", "gg", "kk", "ll", "mm", "nn", "pp", "rr", "ss", "tt"];

        /// <summary>
        /// The last letter of a doubled consonant goes when that letter lies in R1:
        /// "bestemmelse" reaches "bestemm" through step 3 and ends at "bestem".
        /// </summary>
        /// <remarks>
        /// What has to be in R1 is the letter removed, not the pair — Snowball states
        /// a region condition against the suffix an action deletes, and here that is
        /// one character. "hyggelig" reaches "hygg", whose second 'g' is in R1 while
        /// the pair is not, and stems to "hyg".
        /// </remarks>
        private void Step4()
        {
            if (LongestSuffix(DoubleConsonants) is not null && InR1(1))
            {
                Delete(1);
            }
        }
    }
}
