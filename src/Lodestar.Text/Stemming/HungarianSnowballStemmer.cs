using System.Text;

namespace Lodestar.Text.Stemming;

// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithm, the reference implementation and the oracle corpus
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
#pragma warning disable CA1308

/// <summary>
/// The Hungarian Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>snowballstemmer.stemmer("hungarian")</c> — the one
/// language here oracled by the Snowball project's own package rather than by
/// <c>nltk</c>, whose Hungarian omits two vowels and three suffixes
/// (<see href="https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0091-hungarian-takes-snowballstemmer-as-its-oracle.md">decision 0091</see>).
/// Nine steps, all searching R1. Input is lowercased. Thread-safe.
/// </remarks>
public static class HungarianSnowballStemmer
{
    /// <summary>Returns the Hungarian Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'ő' and 'ű' are single code points, as the rules expect.
        string s = word.ToLowerInvariant().Normalize(NormalizationForm.FormC);
        if (s.Length < 2)
        {
            return s;
        }
        return new Worker(s).Run();
    }

    private sealed class Worker : SnowballWorkerBase
    {
        // All fourteen vowels. The two double-acute ones, ő and ű, are what
        // decision 0091 is about: nltk's Hungarian carries only the other twelve.
        private static readonly Func<char, bool> Vowels = c =>
            c is 'a' or 'á' or 'e' or 'é' or 'i' or 'í' or 'o' or 'ó' or 'ö'
              or 'ő' or 'u' or 'ú' or 'ü' or 'ű';

        // Hungarian has no R2, so the base is given the word's length for it.
        public Worker(string s) : base(s, Vowels, HungarianR1(s), s.Length)
        {
        }

        /// <summary>Hungarian's own R1, which is neither of the two standard shapes.</summary>
        /// <remarks>
        /// After the first consonant when the word opens with a vowel, after the
        /// first vowel when it opens with a consonant, and the null region at the
        /// end when the word holds only one of the two.
        /// </remarks>
        private static int HungarianR1(string s)
        {
            if (Vowels(s[0]))
            {
                int i = 1;
                while (i < s.Length && Vowels(s[i]))
                {
                    i++;
                }
                if (i >= s.Length)
                {
                    return s.Length;
                }
                // A digraph is one consonant to this alphabet, so the region opens after it.
                foreach (string d in Digraphs)
                {
                    if (i + d.Length <= s.Length && string.CompareOrdinal(s, i, d, 0, d.Length) == 0)
                    {
                        return Math.Min(i + d.Length, s.Length);
                    }
                }
                return i + 1;
            }

            int j = 0;
            while (j < s.Length && !Vowels(s[j]))
            {
                j++;
            }
            return j >= s.Length ? s.Length : j + 1;
        }

        private static readonly string[] Digraphs = ["dzs", "cs", "dz", "gy", "ly", "ny", "sz", "ty", "zs"];

        // Longest first, so "ssz" is found before "ss" would be.
        private static readonly string[] Doubles =
        [
            "ccs", "ggy", "lly", "nny", "ssz", "tty", "zzs",
            "bb", "cc", "dd", "ff", "gg", "jj", "kk", "ll", "mm",
            "nn", "pp", "rr", "ss", "tt", "vv", "zz",
        ];

        private const string A = "a";
        private const string E = "e";

        public string Run()
        {
            StepInstrumental();
            StepFrequentCases();
            StepSpecialCases();
            StepOtherCases();
            StepFactive();
            StepOwned();
            StepSingularOwner();
            StepPluralOwner();
            StepPlural();
            return S;
        }

        /// <summary>The double consonant sitting just before a suffix of this length, or null.</summary>
        private string? DoubleBefore(int suffixLen)
        {
            int end = S.Length - suffixLen;
            foreach (string d in Doubles)
            {
                if (end - d.Length >= 0 && string.CompareOrdinal(S, end - d.Length, d, 0, d.Length) == 0)
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>Deletes the suffix, then drops one letter of the double it uncovers.</summary>
        private void DeleteAndUndouble(string[] suffixes)
        {
            string? hit = LongestSuffix(suffixes);
            if (hit is null || !InR1(hit.Length))
            {
                return;
            }
            if (DoubleBefore(hit.Length) is not { } pair)
            {
                return;
            }
            Delete(hit.Length);
            // "ll" becomes "l", "ssz" becomes "sz": the double minus its first letter.
            Replace(pair.Length, pair.Substring(1));
        }

        private static readonly string[] Instrumental = ["al", "el"];

        private void StepInstrumental() => DeleteAndUndouble(Instrumental);

        private static readonly string[] Factive = ["á", "é"];

        private void StepFactive() => DeleteAndUndouble(Factive);

        private static readonly string[] FrequentCases =
        [
            "ban", "ben", "ba", "be", "ra", "re", "nak", "nek", "val", "vel",
            "tól", "től", "ról", "ről", "ból", "ből", "hoz", "hez", "höz",
            "nál", "nél", "ig", "at", "et", "ot", "öt", "ért", "képp", "képpen",
            "kor", "ul", "ül", "vá", "vé", "onként", "enként", "anként", "ként",
            "en", "on", "an", "ön", "n", "t",
        ];

        /// <summary>The case endings, and the long vowel a deletion may expose.</summary>
        private void StepFrequentCases()
        {
            string? hit = LongestSuffix(FrequentCases);
            if (hit is null || !InR1(hit.Length))
            {
                return;
            }
            Delete(hit.Length);
            // The rewrite carries the same "if in R1" condition the deletion did.
            if (!InR1(1))
            {
                return;
            }
            if (Ends("á"))
            {
                Replace(1, A);
            }
            else if (Ends("é"))
            {
                Replace(1, E);
            }
        }

        private static readonly string[] SpecialToA = ["ánként", "án"];
        private static readonly string[] SpecialToE = ["én"];

        private void StepSpecialCases() => ApplyLongestRule(
        [
            new(SpecialToA, n => ReplaceIfInR1(n, A)),
            new(SpecialToE, n => ReplaceIfInR1(n, E)),
        ]);

        private static readonly string[] OtherDelete = ["astul", "estül", "stul", "stül"];
        private static readonly string[] OtherToA = ["ástul"];
        private static readonly string[] OtherToE = ["éstül"];

        private void StepOtherCases() => ThreeGroups(OtherDelete, OtherToA, OtherToE);

        private static readonly string[] OwnedDelete = ["oké", "öké", "aké", "eké", "ké", "éi", "é"];
        private static readonly string[] OwnedToA = ["áké", "áéi"];
        private static readonly string[] OwnedToE = ["éké", "ééi", "éé"];

        private void StepOwned() => ThreeGroups(OwnedDelete, OwnedToA, OwnedToE);

        private static readonly string[] SingularOwnerDelete =
        [
            "ünk", "unk", "nk", "juk", "jük", "uk", "ük", "em", "om", "am", "m",
            "od", "ed", "ad", "öd", "d", "ja", "je", "a", "e", "o",
        ];
        private static readonly string[] SingularOwnerToA = ["ánk", "ájuk", "ám", "ád", "á"];
        private static readonly string[] SingularOwnerToE = ["énk", "éjük", "ém", "éd", "é"];

        private void StepSingularOwner() => ThreeGroups(SingularOwnerDelete, SingularOwnerToA, SingularOwnerToE);

        private static readonly string[] PluralOwnerDelete =
        [
            "jaim", "jeim", "aim", "eim", "im", "jaid", "jeid", "aid", "eid", "id",
            "jai", "jei", "ai", "ei", "i", "jaink", "jeink", "eink", "aink", "ink",
            "jaitok", "jeitek", "aitok", "eitek", "itek", "jeik", "jaik", "aik",
            "eik", "ik",
        ];
        private static readonly string[] PluralOwnerToA = ["áim", "áid", "ái", "áink", "áitok", "áik"];
        private static readonly string[] PluralOwnerToE = ["éim", "éid", "éi", "éink", "éitek", "éik"];

        private void StepPluralOwner() => ThreeGroups(PluralOwnerDelete, PluralOwnerToA, PluralOwnerToE);

        private static readonly string[] PluralDelete = ["ök", "ok", "ek", "ak", "k"];
        private static readonly string[] PluralToA = ["ák"];
        private static readonly string[] PluralToE = ["ék"];

        private void StepPlural() => ThreeGroups(PluralDelete, PluralToA, PluralToE);

        /// <summary>The shape six of the nine steps share: delete, or rewrite to a or to e.</summary>
        private void ThreeGroups(string[] delete, string[] toA, string[] toE) => ApplyLongestRule(
        [
            new(delete, DeleteIfInR1),
            new(toA, n => ReplaceIfInR1(n, A)),
            new(toE, n => ReplaceIfInR1(n, E)),
        ]);
    }
}
