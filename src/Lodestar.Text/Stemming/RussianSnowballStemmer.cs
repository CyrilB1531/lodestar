using System.Text;

namespace Lodestar.Text.Stemming;

// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithm, the reference implementations and the oracle corpus
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
#pragma warning disable CA1308

/// <summary>
/// The Russian Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("russian")</c>. An
/// original implementation of the published Snowball algorithm: four steps over an
/// RV region that is the rest of the word after its first vowel — not the Romance
/// RV — plus one R2 test in step 3. Decision 0086 settles the alphabet questions
/// and the one place this parts from <c>nltk</c>. Thread-safe.
/// </remarks>
public static class RussianSnowballStemmer
{
    /// <summary>Returns the Russian Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose (NFC) so a decomposed 'ё' is one letter, then fold it: the
        // algorithm's alphabet has no 'ё' and the description spells it 'е'.
        string s = word.ToLowerInvariant().Normalize(NormalizationForm.FormC).Replace('ё', 'е');
        return new Worker(s).Run();
    }

    private sealed class Worker : SnowballWorkerBase
    {
        private static readonly Func<char, bool> Vowels = c =>
            c is 'а' or 'е' or 'и' or 'о' or 'у' or 'ы' or 'э' or 'ю' or 'я';

        /// <summary>Region RV: the rest of the word after its first vowel.</summary>
        /// <remarks>
        /// Nothing like the Romance RV, which counts letters two and three. R2 is
        /// measured the standard way by the base class and is used by step 3 alone.
        /// </remarks>
        private readonly int _rv;

        public Worker(string s) : base(s, Vowels)
        {
            _rv = ComputeRv(s);
        }

        private static int ComputeRv(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (Vowels(s[i]))
                {
                    return i + 1;
                }
            }
            return s.Length;
        }

        public string Run()
        {
            Step1();
            Step2();
            Step3();
            Step4();
            return S;
        }

        /// <summary>Whether a suffix of this length starts at or after RV.</summary>
        private bool InRv(int suffixLen) => S.Length - suffixLen >= _rv;

        /// <summary>The longest candidate that ends the word and lies in RV, or null.</summary>
        private string? LongestSuffixInRv(string[] candidates)
        {
            string? best = null;
            foreach (string c in candidates)
            {
                if (Ends(c) && InRv(c.Length) && (best is null || c.Length > best.Length))
                {
                    best = c;
                }
            }
            return best;
        }

        /// <summary>
        /// The ending a two-group step removes: the longest candidate in RV that also
        /// passes its group's condition, or null.
        /// </summary>
        /// <remarks>
        /// Three of the four ending classes come in two groups, where group 1 counts
        /// only when the letter before it is <c>а</c> or <c>я</c>. A group 1 candidate
        /// that fails that test does not end the search — the next-longest is tried,
        /// which is how "рискующая" gets past "ющая" and reaches "ая".
        /// </remarks>
        private string? LongestSuffixInRv(string[] afterAOrYa, string[] unconditional)
        {
            string? best = null;
            foreach (string c in afterAOrYa)
            {
                if ((best is null || c.Length > best.Length)
                    && Ends(c) && InRv(c.Length) && PrecededByAOrYa(c.Length))
                {
                    best = c;
                }
            }
            foreach (string c in unconditional)
            {
                if ((best is null || c.Length > best.Length) && Ends(c) && InRv(c.Length))
                {
                    best = c;
                }
            }
            return best;
        }

        // The 'а' or 'я' must itself lie in RV: a group 1 ending sitting at the very
        // start of RV has nothing before it that the step is allowed to look at.
        private bool PrecededByAOrYa(int suffixLen)
        {
            int i = S.Length - suffixLen - 1;
            return i >= _rv && (S[i] == 'а' || S[i] == 'я');
        }

        private static readonly string[] PerfectiveGerundAfterAOrYa = ["вшись", "вши", "в"];

        private static readonly string[] PerfectiveGerund =
            ["ившись", "ывшись", "ивши", "ывши", "ив", "ыв"];

        private static readonly string[] Reflexive = ["ся", "сь"];

        private static readonly string[] Adjective =
        [
            "ее", "ие", "ые", "ое", "ими", "ыми", "ей", "ий", "ый", "ой", "ем", "им",
            "ым", "ом", "его", "ого", "ему", "ому", "их", "ых", "ую", "юю", "ая", "яя",
            "ою", "ею",
        ];

        private const string ParticipleUyushch = "ующ";

        private const string AdjectiveAya = "ая";

        private static readonly string[] ParticipleAfterAOrYa = ["ем", "нн", "вш", "ющ", "щ"];

        private static readonly string[] Participle = ["ивш", "ывш", ParticipleUyushch];

        private static readonly string[] VerbAfterAOrYa =
        [
            "ла", "на", "ете", "йте", "ли", "й", "л", "ем", "н", "ло", "но", "ет", "ют",
            "ны", "ть", "ешь", "нно",
        ];

        private static readonly string[] Verb =
        [
            "ила", "ыла", "ена", "ейте", "уйте", "ите", "или", "ыли", "ило", "ыло",
            "ено", "ят", "ует", "уют", "ены", "ить", "ыть", "ишь", "ую", "ей", "уй",
            "ил", "ыл", "им", "ым", "ен", "ит", "ыт", "ю",
        ];

        private static readonly string[] Noun =
        [
            "а", "ев", "ов", "ие", "ье", "е", "иями", "ями", "ами", "еи", "ии", "и",
            "ией", "ей", "ой", "ий", "й", "иям", "ям", "ием", "ем", "ам", "ом", "о",
            "у", "ах", "иях", "ях", "ы", "ь", "ию", "ью", "ю", "ия", "ья", "я",
        ];

        /// <summary>
        /// A perfective gerund ending, or else a reflexive ending followed by the
        /// first of an adjectival, a verb and a noun ending that matches.
        /// </summary>
        private void Step1()
        {
            string? gerund = LongestSuffixInRv(PerfectiveGerundAfterAOrYa, PerfectiveGerund);
            if (gerund is not null)
            {
                Delete(gerund.Length);
                return;
            }

            string? reflexive = LongestSuffixInRv(Reflexive);
            if (reflexive is not null)
            {
                Delete(reflexive.Length);
            }

            if (TryAdjectival())
            {
                return;
            }

            string? verb = LongestSuffixInRv(VerbAfterAOrYa, Verb);
            if (verb is not null)
            {
                Delete(verb.Length);
                return;
            }

            string? noun = LongestSuffixInRv(Noun);
            if (noun is not null)
            {
                Delete(noun.Length);
            }
        }

        /// <summary>An adjective ending, optionally preceded by a participle ending.</summary>
        private bool TryAdjectival()
        {
            string? adjective = LongestSuffixInRv(Adjective);
            if (adjective is null)
            {
                return false;
            }
            Delete(adjective.Length);

            string? participle = LongestSuffixInRv(ParticipleAfterAOrYa, Participle);
            // Decision 0086: nltk's table spells this one pair wrong and so strips
            // only the "ая". Parity with nltk is the contract, as in decision 0008.
            if (participle is not null
                && !(participle == ParticipleUyushch && adjective == AdjectiveAya))
            {
                Delete(participle.Length);
            }
            return true;
        }

        private const string BareI = "и";

        /// <summary>A final <c>и</c> in RV goes, whatever step 1 did or did not remove.</summary>
        private void Step2()
        {
            if (Ends(BareI) && InRv(1))
            {
                Delete(1);
            }
        }

        private static readonly string[] Derivational = ["ость", "ост"];

        /// <summary>The one step measured against R2 rather than RV.</summary>
        /// <remarks>
        /// No retry on the region here, unlike step 1: a word ending in "ость"
        /// cannot also end in "ост", so at most one of the two ever matches.
        /// </remarks>
        private void Step3()
        {
            string? derivational = LongestSuffix(Derivational);
            if (derivational is not null && InR2(derivational.Length))
            {
                Delete(derivational.Length);
            }
        }

        private const string DoubleN = "нн";

        private static readonly string[] Superlative = ["ейше", "ейш"];

        /// <summary>
        /// One of three, over the whole word rather than a region: undouble <c>нн</c>,
        /// or remove a superlative ending and undouble what it uncovers, or drop a
        /// final <c>ь</c>. A word that reaches the second keeps its <c>ь</c>.
        /// </summary>
        private void Step4()
        {
            if (Ends(DoubleN))
            {
                Delete(1);
                return;
            }

            string? superlative = LongestSuffix(Superlative);
            if (superlative is not null)
            {
                Delete(superlative.Length);
                if (Ends(DoubleN))
                {
                    Delete(1);
                }
                return;
            }

            if (Ends("ь"))
            {
                Delete(1);
            }
        }
    }
}
