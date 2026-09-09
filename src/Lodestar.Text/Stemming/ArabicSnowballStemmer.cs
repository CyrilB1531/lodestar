using System.Text;

namespace Lodestar.Text.Stemming;

// SonarLint S3267: the affix scans early-return on the first match and mutate
// the word in place, neither of which Where can express -- and they run per
// token, where a LINQ pipeline would allocate on every call.
#pragma warning disable S3267

/// <summary>
/// The Arabic Snowball stemming algorithm.
/// </summary>
/// <remarks>
/// Reference behavior: <c>nltk.stem.snowball.SnowballStemmer("arabic")</c>. An
/// original implementation of the published Snowball algorithm. Alone among the
/// thirteen it uses neither R1 nor R2, so it does not derive from
/// <c>SnowballWorkerBase</c> — decision 0094 says why. Thread-safe.
/// </remarks>
public static class ArabicSnowballStemmer
{
    /// <summary>Returns the Arabic Snowball stem of <paramref name="word"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    public static string Stem(string word)
    {
        Guard.NotNull(word);
        return new Worker(word).Run();
    }

    private sealed class Worker
    {
        private string _s;

        public Worker(string word) => _s = word;

        public string Run()
        {
            NormalizeBefore();
            // A word carrying the definite article is a defined noun, and the
            // suffix steps do not read one -- its article is what goes instead.
            _defined = StartsWithArticle();
            StripSuffixes();
            StripPrefixes();
            NormalizeAfter();
            return _s;
        }

        // The vocalisation marks and the kasheeda, none of which the rules read.
        // A char[] because netstandard2.0 has no Contains(char) to search a string with.
        private static readonly char[] Diacritics =
            ['\u064B', '\u064C', '\u064D', '\u064E', '\u064F', '\u0650', '\u0652', '\u0651', '\u0640'];

        private const char Alef = 'ا';
        private const char Hamza = 'ء';
        private const char Waw = 'و';
        private const char Yeh = 'ي';

        private void NormalizeBefore()
        {
            StringBuilder b = new(_s.Length);
            foreach (char c in _s)
            {
                if (Array.IndexOf(Diacritics, c) >= 0)
                {
                    continue;
                }
                // The lam-alef ligatures stand for two letters and are written as two.
                switch (c)
                {
                    case 'ﻻ' or 'ﻼ': b.Append('ل').Append(Alef); break;
                    case 'ﻵ' or 'ﻶ': b.Append('ل').Append('آ'); break;
                    case 'ﻷ' or 'ﻸ': b.Append('ل').Append('أ'); break;
                    case 'ﻹ' or 'ﻺ': b.Append('ل').Append('إ'); break;
                    // The Arabic-Indic digits are the ASCII ones.
                    case >= '\u0660' and <= '\u0669':
                        b.Append((char)('0' + (c - '\u0660'))); break;
                    default: b.Append(c); break;
                }
            }
            _s = b.ToString();
        }

        /// <summary>The hamza carriers, resolved once the affixes are off.</summary>
        private void NormalizeAfter()
        {
            StringBuilder b = new(_s.Length);
            for (int i = 0; i < _s.Length; i++)
            {
                char c = _s[i];
                bool last = i == _s.Length - 1;
                b.Append(c switch
                {
                    'ى' => Yeh,
                    'أ' or 'إ' or 'آ' => last ? Hamza : Alef,
                    'ؤ' => Waw,
                    'ئ' => Yeh,
                    _ => c,
                });
            }
            _s = b.ToString();
        }

        private bool Ends(string suffix) => _s.EndsWith(suffix, StringComparison.Ordinal);

        private bool Starts(string prefix) => _s.StartsWith(prefix, StringComparison.Ordinal);

        private void CutEnd(int n) => _s = _s.Substring(0, _s.Length - n);

        private void CutStart(int n) => _s = _s.Substring(n);

        /// <summary>Removes the first listed suffix the word carries, when it is long enough.</summary>
        private void StripOne((string Affix, int MinLength)[] rules)
        {
            foreach ((string affix, int min) in rules)
            {
                if (_s.Length >= min && Ends(affix))
                {
                    CutEnd(affix.Length);
                    return;
                }
            }
        }

        private static readonly (string, int)[] Suffix1 =
        [
            ("كما", 6), ("هما", 6), ("كمو", 6),
            ("نا", 5), ("كم", 5), ("كن", 5), ("ها", 5), ("هن", 5), ("هم", 5), ("ني", 5),
            ("ك", 4), ("ه", 4),
        ];

        private static readonly (string, int)[] Suffix2 =
        [
            ("تما", 6), ("ان", 6), ("ون", 6), ("ين", 6),
            ("نا", 5), ("تا", 5), ("تن", 5), ("وا", 5), ("تم", 5),
            ("ا", 4), ("ن", 5), ("و", 4),
            ("ت", 4),
        ];

        private static readonly (string, int)[] Suffix3 = [("ي", 4)];

        private bool _defined;

        private const char Lam = 'ل';

        private static readonly string[] ArticleOpenings = ["بال", "كال", "لل", "ال"];

        /// <summary>Whether the word opens with the definite article, alone or behind a preposition.</summary>
        private bool StartsWithArticle()
        {
            foreach (string opening in ArticleOpenings)
            {
                if (Starts(opening))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Whether the definite article stands anywhere in the word.</summary>
        /// <remarks>
        /// Hand-rolled rather than string.Contains: the overload that takes a
        /// StringComparison does not exist on netstandard2.0, and CA1307 and CA2249
        /// ask for opposite things about the IndexOf that does.
        /// </remarks>
        private bool CarriesArticle()
        {
            for (int i = 0; i + 1 < _s.Length; i++)
            {
                if (_s[i] == Alef && _s[i + 1] == Lam)
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly (string, int)[] FeminineMarker = [("ة", 4)];

        /// <summary>
        /// The feminine marker goes whatever the word is; the possessives and the
        /// case endings are read only on a word that does not open with the article,
        /// and the nisba only on one that does not carry it at all.
        /// </summary>
        private void StripSuffixes()
        {
            StripOne(FeminineMarker);
            if (!_defined)
            {
                StripOne(Suffix1);
                StripOne(Suffix2);
            }
            if (!CarriesArticle())
            {
                StripOne(Suffix3);
            }
        }

        private static readonly string[] VerbStep4 = ["يست", "نست", "تست"];

        private static readonly string[] FutureMarkers = ["سي", "ست", "سن", "سأ", "سؤ"];

        private static readonly (string, int)[] PrefixArticles =
        [
            ("بال", 6), ("كال", 6),
            ("لل", 5), ("ال", 5),
        ];

        private void StripPrefixes()
        {
            // The conjunctions, which never sit in front of an alef.
            if (_s.Length > 3 && (Starts("ف") || Starts("و")) && _s[1] != Alef)
            {
                CutStart(1);
            }
            // The imperfect stems that carry a seen: the marker resolves to alef.
            foreach (string tenth in VerbStep4)
            {
                if (_s.Length > 4 && Starts(tenth))
                {
                    _s = Alef + _s.Substring(1);
                    return;
                }
            }
            foreach (string marker in FutureMarkers)
            {
                if (_s.Length > 4 && Starts(marker))
                {
                    CutStart(1);
                    break;
                }
            }
            foreach ((string affix, int min) in PrefixArticles)
            {
                if (_s.Length >= min && Starts(affix))
                {
                    CutStart(affix.Length);
                    return;
                }
            }
        }
    }
}
