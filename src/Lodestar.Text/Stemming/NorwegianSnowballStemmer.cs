namespace Lodestar.Text.Stemming;

/// <summary>
/// The Norwegian Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("norwegian")</c>,
/// which is Bokmål; Nynorsk has no oracle here and is out of scope. An original
/// implementation of the published Snowball algorithm: three steps that each
/// search R1, no R2 and no RV region, R1 floored at three letters as in German —
/// see <c>docs/equivalence.md</c>'s stemming row. Input is lowercased. Thread-safe.
/// </remarks>
public static class NorwegianSnowballStemmer
{
    /// <summary>Returns the Norwegian Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word) =>
        ScandinavianSnowballWorker.Stem(word, IsVowel, Run);

    // æ, å and ø are letters of this alphabet rather than accented forms, so
    // nothing is folded or stripped before the rules run.
    private static bool IsVowel(char c) =>
        c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'æ' or 'å' or 'ø';

    private static void Run(ScandinavianSnowballWorker w)
    {
        Step1(w);
        w.StripPair(Step2Suffixes);
        w.StripInR1(Step3Suffixes);
    }

    private const string Ert = "ert";
    private const string Erte = "erte";

    // Groups (a), (b) and (c) are searched together, not one after the other:
    // the published step takes the longest suffix lying in R1 across all three.
    private static readonly string[] Step1Suffixes =
    [
        "a", "e", "ede", "ande", "ende", "ane", "ene", "hetene", "en", "heten",
        "ar", "er", "heter", "as", "es", "edes", "endes", "enes", "hetenes",
        "ens", "hetens", "ers", "ets", "et", "het", "ast",
        ScandinavianSnowballWorker.BareS, Ert, Erte,
    ];

    /// <summary>
    /// Step 1 does not use the shared strip: group (c) rewrites rather than deletes,
    /// and group (b)'s s-ending reads two letters rather than one.
    /// </summary>
    private static void Step1(ScandinavianSnowballWorker w)
    {
        if (w.LongestInR1(Step1Suffixes) is not { } hit)
        {
            return;
        }
        if (hit == Ert || hit == Erte)
        {
            // Group (c) is rewritten rather than stripped: "servert" ends at "server".
            w.Rewrite(hit.Length, "er");
            return;
        }
        if (hit != ScandinavianSnowballWorker.BareS)
        {
            w.Remove(hit.Length);
            return;
        }
        if (HasValidSEnding(w))
        {
            w.Remove(1);
        }
    }

    /// <summary>Whether the letter before the final s is a valid s-ending.</summary>
    /// <remarks>
    /// 'o' and 'y' are vowels in this alphabet and are on the published list anyway.
    /// A 'k' qualifies only when it is not itself after a vowel, which is what parts
    /// "folks" — stemmed to "folk" — from "boks", which keeps its s.
    /// </remarks>
    private static bool HasValidSEnding(ScandinavianSnowballWorker w)
    {
        string s = w.Word;
        int i = s.Length - 2;
        if (i < 0)
        {
            return false;
        }
        char c = s[i];
        if (c is 'b' or 'c' or 'd' or 'f' or 'g' or 'h' or 'j' or 'l' or 'm'
              or 'n' or 'o' or 'p' or 'r' or 't' or 'v' or 'y' or 'z')
        {
            return true;
        }
        return c == 'k' && !w.IsVowelAt(i - 1);
    }

    private static readonly string[] Step2Suffixes = ["dt", "vt"];

    private static readonly string[] Step3Suffixes =
        ["leg", "eleg", "ig", "eig", "lig", "elig", "els", "lov", "elov", "slov", "hetslov"];
}
