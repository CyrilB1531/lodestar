using System.Text;

namespace Lodestar.Text.Stemming;

// SonarLint S3776: cognitive complexity: faithful port of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S3267: the suffix scans early-return and mutate in place, which Where cannot express.
// CA1307 (specify StringComparison): the overload it asks for —
// string.IndexOf(char, StringComparison) — does not exist on netstandard2.0,
// which this assembly targets. The call is ordinal on every runtime that has
// it, so the suggestion would change nothing but the compilation.
// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithm, the reference implementations and the oracle corpus
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
#pragma warning disable S3776, S3267, CA1307, CA1308

/// <summary>
/// The Dutch Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("dutch")</c>. An
/// original implementation of the published Snowball algorithm: no RV region, R1
/// floored at three letters as in German, umlauts and acutes folded before the
/// rules run, and a stressed vowel undoubled after them — see
/// <c>docs/equivalence.md</c>'s stemming row. Input is lowercased. Thread-safe.
/// </remarks>
public static class DutchSnowballStemmer
{
    /// <summary>Returns the Dutch Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        // Compose accents (NFC) so 'ë' etc. are single code points, then fold
        // them: the accent is removed before the word has a length to test.
        string s = FoldAccents(word.ToLowerInvariant().Normalize(NormalizationForm.FormC));
        if (s.Length < 2)
        {
            return s;
        }
        return new Worker(s).Run();
    }

    /// <summary>Umlauts and acutes fold to the bare vowel; the grave on 'è' does not.</summary>
    private static string FoldAccents(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (char c in s)
        {
            sb.Append(c switch
            {
                'ä' or 'á' => 'a',
                'ë' or 'é' => 'e',
                'ï' or 'í' => 'i',
                'ö' or 'ó' => 'o',
                'ü' or 'ú' => 'u',
                _ => c,
            });
        }
        return sb.ToString();
    }

    private sealed class Worker : SnowballWorkerBase
    {
        private static readonly Func<char, bool> Vowels = c =>
            c is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'è';

        /// <summary>Whether step 2 actually removed an e, which is what step 3b's "bar" turns on.</summary>
        private bool _eRemoved;

        // The region before R1 must hold at least three letters, as in German.
        public Worker(string s) : base(MarkConsonants(s), Vowels, minR1: 3)
        {
        }

        public string Run()
        {
            Step1();
            Step2();
            Step3a();
            Step3b();
            Step4();
            return Unmark(S);
        }

        /// <summary>Initial y, y after a vowel and i between vowels are marked as consonants.</summary>
        private static string MarkConsonants(string s)
        {
            char[] a = s.ToCharArray();
            if (a[0] == 'y')
            {
                a[0] = 'Y';
            }
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] == 'y' && Vowels(s[i - 1]))
                {
                    a[i] = 'Y';
                }
                else if (a[i] == 'i' && i + 1 < a.Length && Vowels(s[i - 1]) && Vowels(s[i + 1]))
                {
                    a[i] = 'I';
                }
            }
            return new string(a);
        }

        /// <summary>Unmark the y and i the preprocessing lifted out of the vowel set.</summary>
        private static string Unmark(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                sb.Append(c switch
                {
                    'I' => 'i',
                    'Y' => 'y',
                    _ => c,
                });
            }
            return sb.ToString();
        }

        /// <summary>The letter before a suffix of this length, or NUL when the suffix is the whole word.</summary>
        private char CharBefore(int suffixLen)
        {
            int i = S.Length - suffixLen - 1;
            return i >= 0 ? S[i] : '\0';
        }

        private bool PrecededByNonVowel(int suffixLen)
        {
            int i = S.Length - suffixLen - 1;
            return i >= 0 && !IsVowel(S[i]);
        }

        /// <summary>A valid en-ending is a non-vowel, and not "gem".</summary>
        private bool IsValidEnEnding(int suffixLen)
        {
            if (!PrecededByNonVowel(suffixLen))
            {
                return false;
            }
            int start = S.Length - suffixLen - 3;
            return start < 0 || S.Substring(start, 3) != "gem";
        }

        /// <summary>A valid s-ending is a non-vowel other than j.</summary>
        private bool IsValidSEnding(int suffixLen) =>
            PrecededByNonVowel(suffixLen) && CharBefore(suffixLen) != 'j';

        /// <summary>A word left ending in a doubled k, d or t loses one of them.</summary>
        private void Undouble()
        {
            if (Ends("kk") || Ends("dd") || Ends("tt"))
            {
                Delete(1);
            }
        }

        private static readonly string[] Step1Heden = ["heden"];
        private static readonly string[] Step1EnEne = ["ene", "en"];
        private static readonly string[] Step1SSe = ["se", "s"];

        private void Step1() => ApplyLongestRule(
        [
            new(Step1Heden, ReplaceHeden),
            new(Step1EnEne, DeleteEn),
            new(Step1SSe, DeleteS),
        ]);

        /// <summary>"heden" becomes "heid", which step 3a may then take in R2.</summary>
        private void ReplaceHeden(int n)
        {
            if (InR1(n))
            {
                Replace(n, "heid");
            }
        }

        private void DeleteEn(int n)
        {
            if (!InR1(n) || !IsValidEnEnding(n))
            {
                return;
            }
            Delete(n);
            Undouble();
        }

        private void DeleteS(int n)
        {
            if (InR1(n) && IsValidSEnding(n))
            {
                Delete(n);
            }
        }

        /// <summary>A final e in R1 after a non-vowel goes, and the word then undoubles.</summary>
        private void Step2()
        {
            if (!Ends("e") || !InR1(1) || !PrecededByNonVowel(1))
            {
                return;
            }
            _eRemoved = true;
            Delete(1);
            Undouble();
        }

        /// <summary>"heid" goes in R2 unless a c precedes it; a preceding "en" then goes as in step 1.</summary>
        private void Step3a()
        {
            if (!Ends("heid") || !InR2(4) || CharBefore(4) == 'c')
            {
                return;
            }
            Delete(4);
            if (Ends("en") && InR1(2) && IsValidEnEnding(2))
            {
                Delete(2);
                Undouble();
            }
        }

        private static readonly string[] Step3bEndIng = ["end", "ing"];
        private static readonly string[] Step3bIg = ["ig"];
        private static readonly string[] Step3bLijk = ["lijk"];
        private static readonly string[] Step3bBaar = ["baar"];
        private static readonly string[] Step3bBar = ["bar"];

        private void Step3b() => ApplyLongestRule(
        [
            new(Step3bEndIng, DeleteEndIng),
            new(Step3bIg, DeleteIg),
            new(Step3bLijk, DeleteLijk),
            new(Step3bBaar, DeleteIfInR2),
            new(Step3bBar, DeleteBar),
        ]);

        /// <summary>"end"/"ing" go in R2; an "ig" they uncover goes too, otherwise the word undoubles.</summary>
        private void DeleteEndIng(int n)
        {
            if (!InR2(n))
            {
                return;
            }
            Delete(n);
            if (Ends("ig") && InR2(2) && CharBefore(2) != 'e')
            {
                Delete(2);
            }
            else
            {
                Undouble();
            }
        }

        /// <summary>"ig" goes in R2, but never straight after an e.</summary>
        private void DeleteIg(int n)
        {
            if (InR2(n) && CharBefore(n) != 'e')
            {
                Delete(n);
            }
        }

        /// <summary>"lijk" goes in R2, and step 2 then runs again on what is left.</summary>
        private void DeleteLijk(int n)
        {
            if (!InR2(n))
            {
                return;
            }
            Delete(n);
            Step2();
        }

        /// <summary>"bar" goes in R2 only when step 2 was the thing that exposed it.</summary>
        private void DeleteBar(int n)
        {
            if (InR2(n) && _eRemoved)
            {
                Delete(n);
            }
        }

        /// <summary>A doubled a, e, o or u between two consonants loses one letter: "manen" ends at "man".</summary>
        private void Step4()
        {
            if (S.Length < 4)
            {
                return;
            }
            char last = S[S.Length - 1];
            // The marked I is a consonant everywhere else, but not here.
            if (IsVowel(last) || last == 'I' || IsVowel(S[S.Length - 4]))
            {
                return;
            }
            string doubled = S.Substring(S.Length - 3, 2);
            if (doubled is "aa" or "ee" or "oo" or "uu")
            {
                S = S.Substring(0, S.Length - 2) + last;
            }
        }
    }
}
