using System.Text;

namespace Lodestar.Text.Phonetics;

/// <summary>A word's two Double Metaphone codes.</summary>
/// <param name="Primary">The primary code, empty when the word carries no encodable letter.</param>
/// <param name="Secondary">
/// The alternate code, or empty when the word has no alternate pronunciation. The reference
/// repeats the primary there; empty is the convention <c>jellyfish</c>, <c>metaphone</c> and
/// <c>phonetics</c> share, and <c>docs/decisions/0075</c> takes it for this API.
/// </param>
public readonly record struct DoubleMetaphoneCode(string Primary, string Secondary);

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S1479: the letter switch has one arm per letter by design, which is the shape the published description has.
#pragma warning disable S3776, S1479

/// <summary>
/// Double Metaphone phonetic encoding (Lawrence Philips, 2000), which returns two codes
/// so that a word with more than one plausible pronunciation can match on either.
/// </summary>
/// <remarks>
/// Reference: <c>doublemetaphone.doublemetaphone</c> 1.2, not <c>jellyfish</c>, which exports none
/// (<c>docs/decisions/0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md</c>). Codes are
/// <c>A F H J K L M N P R S T X 0</c> (<c>X</c> is "sh", <c>0</c> is "th") and untruncated because
/// the reference is; only ASCII letters are read. Thread-safe.
/// </remarks>
/// <seealso href="../../../docs/reference/text/phonetics/doublemetaphone.md">What to compare, and why two codes.</seealso>
public static class DoubleMetaphone
{
    /// <summary>How many spaces pad the working string so a lookahead cannot run off its end.</summary>
    /// <remarks>Four is the longest lookahead any rule makes (<c>StringAt(current + 4, …)</c>); a fifth is slack.</remarks>
    private const int Padding = 5;

    /// <summary>Encodes a string to its two Double Metaphone codes.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    public static DoubleMetaphoneCode Encode(string value)
    {
        Guard.NotNull(value);
        return Encode(value.AsSpan());
    }

    /// <summary>Encodes <paramref name="value"/> to its two Double Metaphone codes (or two empties).</summary>
    public static DoubleMetaphoneCode Encode(ReadOnlySpan<char> value)
    {
        var letters = new StringBuilder(value.Length);
        foreach (char ch in value)
        {
            // ASCII letters only, which is what the reference keeps. char.IsLetter would admit
            // 'é' and 'Ü', and the reference drops both -- see the remarks and the frozen corpus.
            if (ch is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'))
            {
                letters.Append(char.ToUpperInvariant(ch));
            }
        }
        if (letters.Length == 0)
        {
            return new DoubleMetaphoneCode(string.Empty, string.Empty);
        }

        int length = letters.Length;
        int last = length - 1;
        // Padded so every lookahead below can read past the last letter without a bounds test of
        // its own. Spaces are safe filler, because the letter filter above removed the real ones.
        string w = letters.Append(' ', Padding).ToString();

        var primary = new StringBuilder(length + 4);
        var secondary = new StringBuilder(length + 4);
        bool slavoGermanic = IsSlavoGermanic(w);
        int current = 0;

        // Silent initial letters, and an initial X that is heard as S ("Xavier").
        if (StringAt(w, 0, "GN", "KN", "PN", "WR", "PS"))
        {
            current = 1;
        }
        if (w[0] == 'X')
        {
            Add(primary, secondary, "S", "S");
            current = 1;
        }

        while (current < length)
        {
            switch (w[current])
            {
                case 'A' or 'E' or 'I' or 'O' or 'U' or 'Y':
                    // A vowel is heard only where nothing precedes it.
                    if (current == 0)
                    {
                        Add(primary, secondary, "A", "A");
                    }
                    current++;
                    break;

                case 'B':
                    Add(primary, secondary, "P", "P");
                    current += w[current + 1] == 'B' ? 2 : 1;
                    break;

                case 'C':
                    current = EncodeC(w, primary, secondary, current);
                    break;

                case 'D':
                    if (StringAt(w, current, "DG"))
                    {
                        if (StringAt(w, current + 2, "I", "E", "Y"))
                        {
                            Add(primary, secondary, "J", "J"); // "edge"
                            current += 3;
                        }
                        else
                        {
                            Add(primary, secondary, "TK", "TK"); // "Edgar"
                            current += 2;
                        }
                        break;
                    }
                    if (StringAt(w, current, "DT", "DD"))
                    {
                        Add(primary, secondary, "T", "T");
                        current += 2;
                        break;
                    }
                    Add(primary, secondary, "T", "T");
                    current++;
                    break;

                case 'F':
                    Add(primary, secondary, "F", "F");
                    current += w[current + 1] == 'F' ? 2 : 1;
                    break;

                case 'G':
                    current = EncodeG(w, primary, secondary, current, slavoGermanic);
                    break;

                case 'H':
                    // Heard only where it opens a syllable: first, or between two vowels.
                    if ((current == 0 || IsVowel(w[current - 1])) && IsVowel(w[current + 1]))
                    {
                        Add(primary, secondary, "H", "H");
                        current += 2;
                    }
                    else
                    {
                        current++;
                    }
                    break;

                case 'J':
                    current = EncodeJ(w, primary, secondary, current, last, slavoGermanic);
                    break;

                case 'K':
                    Add(primary, secondary, "K", "K");
                    current += w[current + 1] == 'K' ? 2 : 1;
                    break;

                case 'L':
                    if (w[current + 1] == 'L')
                    {
                        // Spanish "-illo", "-illa", "-alle": the second L is not heard, and the
                        // alternate drops the sound entirely so "Cabrillo" can meet "Cabriyo".
                        if ((current == length - 3 && StringAt(w, current - 1, "ILLO", "ILLA", "ALLE"))
                            || ((StringAt(w, last - 1, "AS", "OS") || StringAt(w, last, "A", "O"))
                                && StringAt(w, current - 1, "ALLE")))
                        {
                            Add(primary, secondary, "L", string.Empty);
                            current += 2;
                            break;
                        }
                        current += 2;
                    }
                    else
                    {
                        current++;
                    }
                    Add(primary, secondary, "L", "L");
                    break;

                case 'M':
                    // Silent in "-umb": "thumb", "dumber" -- but not "number", where the B is heard.
                    if ((StringAt(w, current - 1, "UMB") && (current + 1 == last || StringAt(w, current + 2, "ER")))
                        || w[current + 1] == 'M')
                    {
                        current += 2;
                    }
                    else
                    {
                        current++;
                    }
                    Add(primary, secondary, "M", "M");
                    break;

                case 'N':
                    Add(primary, secondary, "N", "N");
                    current += w[current + 1] == 'N' ? 2 : 1;
                    break;

                case 'P':
                    if (w[current + 1] == 'H')
                    {
                        Add(primary, secondary, "F", "F");
                        current += 2;
                        break;
                    }
                    Add(primary, secondary, "P", "P");
                    // "campbell", "raspberry": the second consonant is not heard.
                    current += StringAt(w, current + 1, "P", "B") ? 2 : 1;
                    break;

                case 'Q':
                    Add(primary, secondary, "K", "K");
                    current += w[current + 1] == 'Q' ? 2 : 1;
                    break;

                case 'R':
                    // French "-ier": "Rogier" keeps the R only on the alternate, but "Hochmeier"
                    // is Germanic and keeps it on both.
                    if (current == last && !slavoGermanic
                        && StringAt(w, current - 2, "IE") && !StringAt(w, current - 4, "ME", "MA"))
                    {
                        Add(primary, secondary, string.Empty, "R");
                    }
                    else
                    {
                        Add(primary, secondary, "R", "R");
                    }
                    current += w[current + 1] == 'R' ? 2 : 1;
                    break;

                case 'S':
                    current = EncodeS(w, primary, secondary, current, last, slavoGermanic);
                    break;

                case 'T':
                    if (StringAt(w, current, "TION") || StringAt(w, current, "TIA", "TCH"))
                    {
                        Add(primary, secondary, "X", "X");
                        current += 3;
                        break;
                    }
                    if (StringAt(w, current, "TH") || StringAt(w, current, "TTH"))
                    {
                        // "Thomas", "Thames" and the Germanic names keep a hard T.
                        if (StringAt(w, current + 2, "OM", "AM") || StringAt(w, 0, "VAN ", "VON ") || StringAt(w, 0, "SCH"))
                        {
                            Add(primary, secondary, "T", "T");
                        }
                        else
                        {
                            Add(primary, secondary, "0", "T");
                        }
                        current += 2;
                        break;
                    }
                    Add(primary, secondary, "T", "T");
                    current += StringAt(w, current + 1, "T", "D") ? 2 : 1;
                    break;

                case 'V':
                    Add(primary, secondary, "F", "F");
                    current += w[current + 1] == 'V' ? 2 : 1;
                    break;

                case 'W':
                    current = EncodeW(w, primary, secondary, current, last);
                    break;

                case 'X':
                    // French "-eaux", "-oux": the X is silent at the end ("Breaux").
                    if (!(current == last
                          && (StringAt(w, current - 3, "IAU", "EAU") || StringAt(w, current - 2, "AU", "OU"))))
                    {
                        Add(primary, secondary, "KS", "KS");
                    }
                    current += StringAt(w, current + 1, "C", "X") ? 2 : 1;
                    break;

                case 'Z':
                    if (w[current + 1] == 'H')
                    {
                        Add(primary, secondary, "J", "J"); // Chinese pinyin, "Zhao"
                        current += 2;
                        break;
                    }
                    if (StringAt(w, current + 1, "ZO", "ZI", "ZA") || (slavoGermanic && current > 0 && w[current - 1] != 'T'))
                    {
                        Add(primary, secondary, "S", "TS");
                    }
                    else
                    {
                        Add(primary, secondary, "S", "S");
                    }
                    current += w[current + 1] == 'Z' ? 2 : 1;
                    break;

                default:
                    current++;
                    break;
            }
        }

        string primaryCode = primary.ToString();
        string secondaryCode = secondary.ToString();
        // The pair is the word's two pronunciations; where they coincide there is only one, and
        // the API says so with an empty alternate rather than by repeating itself (decision 0075).
        return new DoubleMetaphoneCode(primaryCode, secondaryCode == primaryCode ? string.Empty : secondaryCode);
    }

    private static int EncodeC(string w, StringBuilder primary, StringBuilder secondary, int current)
    {
        // Germanic "-ACH-" as in "Bach", but not the Italian "Achilles" or the "-acher" names.
        if (current > 1 && !IsVowel(w[current - 2]) && StringAt(w, current - 1, "ACH")
            && w[current + 2] != 'I'
            && (w[current + 2] != 'E' || StringAt(w, current - 2, "BACHER", "MACHER")))
        {
            Add(primary, secondary, "K", "K");
            return current + 2;
        }
        if (current == 0 && StringAt(w, current, "CAESAR"))
        {
            Add(primary, secondary, "S", "S");
            return current + 2;
        }
        if (StringAt(w, current, "CHIA"))
        {
            Add(primary, secondary, "K", "K"); // Italian, "Chianti"
            return current + 2;
        }
        if (StringAt(w, current, "CH"))
        {
            // "Michael": hard on the primary, soft on the alternate.
            if (current > 0 && StringAt(w, current, "CHAE"))
            {
                Add(primary, secondary, "K", "X");
                return current + 2;
            }
            // Greek roots: "chemistry", "chorus" -- but "chore" is English.
            if (current == 0
                && (StringAt(w, current + 1, "HARAC", "HARIS") || StringAt(w, current + 1, "HOR", "HYM", "HIA", "HEM"))
                && !StringAt(w, 0, "CHORE"))
            {
                Add(primary, secondary, "K", "K");
                return current + 2;
            }
            // Germanic, Greek, or otherwise a "kh" sound rather than "ch".
            if (StringAt(w, 0, "VAN ", "VON ") || StringAt(w, 0, "SCH")
                || StringAt(w, current - 2, "ORCHES", "ARCHIT", "ORCHID")
                || StringAt(w, current + 2, "T", "S")
                || ((StringAt(w, current - 1, "A", "O", "U", "E") || current == 0)
                    && StringAt(w, current + 2, "L", "R", "N", "M", "B", "H", "F", "V", "W", " ")))
            {
                Add(primary, secondary, "K", "K");
            }
            else if (current > 0)
            {
                // "McHugh" is Scottish and hard; anything else is soft with a hard alternate.
                Add(primary, secondary, StringAt(w, 0, "MC") ? "K" : "X", "K");
            }
            else
            {
                Add(primary, secondary, "X", "X");
            }
            return current + 2;
        }
        if (StringAt(w, current, "CZ") && !StringAt(w, current - 2, "WICZ"))
        {
            Add(primary, secondary, "S", "X"); // "Czerny"
            return current + 2;
        }
        if (StringAt(w, current + 1, "CIA"))
        {
            Add(primary, secondary, "X", "X"); // "focaccia"
            return current + 3;
        }
        // A doubled C, except where the first belongs to a preceding "Mc".
        if (StringAt(w, current, "CC") && !(current == 1 && w[0] == 'M'))
        {
            if (StringAt(w, current + 2, "I", "E", "H") && !StringAt(w, current + 2, "HU"))
            {
                // "accident", "accede", "succeed" split into two sounds; the Italian names do not.
                if ((current == 1 && w[current - 1] == 'A') || StringAt(w, current - 1, "UCCEE", "UCCES"))
                {
                    Add(primary, secondary, "KS", "KS");
                }
                else
                {
                    Add(primary, secondary, "X", "X"); // "Bertucci"
                }
                return current + 3;
            }
            Add(primary, secondary, "K", "K"); // "Bacchus"
            return current + 2;
        }
        if (StringAt(w, current, "CK", "CG", "CQ"))
        {
            Add(primary, secondary, "K", "K");
            return current + 2;
        }
        if (StringAt(w, current, "CI", "CE", "CY"))
        {
            // Italian "-cio", "-cie", "-cia" take a "ch" alternate.
            Add(primary, secondary, "S", StringAt(w, current, "CIO", "CIE", "CIA") ? "X" : "S");
            return current + 2;
        }

        Add(primary, secondary, "K", "K");
        // long-comment: this branch is unreachable and deliberately kept, which is a claim a
        // reader will otherwise try to disprove. "Mac Caffrey" and "Mac Gregor" reach it only
        // with their space intact, and this API's letter filter removes the space before the
        // loop starts. It stays so the rule set matches the published description one for one,
        // which is what makes a divergence auditable against the reference -- the same trade
        // Metaphone.cs states next door, and the same posture Lodestar.Stats took for
        // Kolmogorov.Sf when it lost its last caller.
        if (StringAt(w, current + 1, " C", " Q", " G"))
        {
            return current + 3;
        }
        if (StringAt(w, current + 1, "C", "K", "Q") && !StringAt(w, current + 1, "CE", "CI"))
        {
            return current + 2;
        }
        return current + 1;
    }

    private static int EncodeG(string w, StringBuilder primary, StringBuilder secondary, int current, bool slavoGermanic)
    {
        if (w[current + 1] == 'H')
        {
            if (current > 0 && !IsVowel(w[current - 1]))
            {
                Add(primary, secondary, "K", "K");
                return current + 2;
            }
            if (current == 0)
            {
                // "Ghislaine" is soft; "Ghiradelli" and the rest are hard.
                Add(primary, secondary, w[current + 2] == 'I' ? "J" : "K", w[current + 2] == 'I' ? "J" : "K");
                return current + 2;
            }
            // Parker's rule: after B, H or D a couple of letters back, the GH is silent
            // ("Hugh", "bright", "Cavanaugh").
            if ((current > 1 && StringAt(w, current - 2, "B", "H", "D"))
                || (current > 2 && StringAt(w, current - 3, "B", "H", "D"))
                || (current > 3 && StringAt(w, current - 4, "B", "H")))
            {
                return current + 2;
            }
            // "laugh", "cough", "rough": the GH is an F after a U.
            if (current > 2 && w[current - 1] == 'U' && StringAt(w, current - 3, "C", "G", "L", "R", "T"))
            {
                Add(primary, secondary, "F", "F");
            }
            else if (current > 0 && w[current - 1] != 'I')
            {
                Add(primary, secondary, "K", "K");
            }
            return current + 2;
        }
        if (w[current + 1] == 'N')
        {
            if (current == 1 && IsVowel(w[0]) && !slavoGermanic)
            {
                Add(primary, secondary, "KN", "N");
            }
            else if (!StringAt(w, current + 2, "EY") && w[current + 1] != 'Y' && !slavoGermanic)
            {
                // "Cagney": English drops the G, the alternate keeps it.
                Add(primary, secondary, "N", "KN");
            }
            else
            {
                Add(primary, secondary, "KN", "KN");
            }
            return current + 2;
        }
        if (StringAt(w, current + 1, "LI") && !slavoGermanic)
        {
            Add(primary, secondary, "KL", "L"); // "Tagliaro"
            return current + 2;
        }
        // "-ges-", "-gep-", "-gel-", "-gie-" opening a word take a soft alternate.
        if (current == 0
            && (w[current + 1] == 'Y'
                || StringAt(w, current + 1, "ES", "EP", "EB", "EL", "EY", "IB", "IL", "IN", "IE", "EI", "ER")))
        {
            Add(primary, secondary, "K", "J");
            return current + 2;
        }
        // "-ger-", "-gy-", except where the word is one of the hard-G exceptions.
        if ((StringAt(w, current + 1, "ER") || w[current + 1] == 'Y')
            && !StringAt(w, 0, "DANGER", "RANGER", "MANGER")
            && !StringAt(w, current - 1, "E", "I")
            && !StringAt(w, current - 1, "RGY", "OGY"))
        {
            Add(primary, secondary, "K", "J");
            return current + 2;
        }
        if (StringAt(w, current + 1, "E", "I", "Y") || StringAt(w, current - 1, "AGGI", "OGGI"))
        {
            // Obviously Germanic names keep the hard G; the French ending is always soft.
            if (StringAt(w, 0, "VAN ", "VON ") || StringAt(w, 0, "SCH") || StringAt(w, current + 1, "ET"))
            {
                Add(primary, secondary, "K", "K");
            }
            else
            {
                Add(primary, secondary, "J", StringAt(w, current + 1, "IER ") ? "J" : "K");
            }
            return current + 2;
        }

        int next = w[current + 1] == 'G' ? current + 2 : current + 1;
        Add(primary, secondary, "K", "K");
        return next;
    }

    private static int EncodeJ(string w, StringBuilder primary, StringBuilder secondary, int current, int last, bool slavoGermanic)
    {
        // Spanish "Jose", "San Jacinto": an H sound rather than a J.
        if (StringAt(w, current, "JOSE") || StringAt(w, 0, "SAN "))
        {
            if ((current == 0 && w[current + 4] == ' ') || StringAt(w, 0, "SAN "))
            {
                Add(primary, secondary, "H", "H");
            }
            else
            {
                Add(primary, secondary, "J", "H");
            }
            return current + 1;
        }

        if (current == 0 && !StringAt(w, current, "JOSE"))
        {
            Add(primary, secondary, "J", "A"); // "Jankelowicz" meets "Yankelowicz"
        }
        else if (IsVowel(w[current - 1]) && !slavoGermanic && (w[current + 1] == 'A' || w[current + 1] == 'O'))
        {
            Add(primary, secondary, "J", "H"); // Spanish "bajador"
        }
        else if (current == last)
        {
            Add(primary, secondary, "J", string.Empty);
        }
        else if (!StringAt(w, current + 1, "L", "T", "K", "S", "N", "M", "B", "Z")
                 && !StringAt(w, current - 1, "S", "K", "L"))
        {
            Add(primary, secondary, "J", "J");
        }

        return w[current + 1] == 'J' ? current + 2 : current + 1;
    }

    private static int EncodeS(string w, StringBuilder primary, StringBuilder secondary, int current, int last, bool slavoGermanic)
    {
        // Silent in "island", "isle", "Carlisle".
        if (StringAt(w, current - 1, "ISL", "YSL"))
        {
            return current + 1;
        }
        if (current == 0 && StringAt(w, current, "SUGAR"))
        {
            Add(primary, secondary, "X", "S");
            return current + 1;
        }
        if (StringAt(w, current, "SH"))
        {
            // Germanic place-name endings keep a plain S ("Sholtz", "Holzheim").
            Add(primary, secondary, StringAt(w, current + 1, "HEIM", "HOEK", "HOLM", "HOLZ") ? "S" : "X", null);
            return current + 2;
        }
        if (StringAt(w, current, "SIO", "SIA") || StringAt(w, current, "SIAN"))
        {
            // Italian and Armenian: "Rossian" takes a "sh" alternate, the Slavic names do not.
            Add(primary, secondary, "S", slavoGermanic ? "S" : "X");
            return current + 3;
        }
        // German "Schmidt", "Snider": an initial S before these consonants is heard as "sh",
        // which is what lets "Smith" meet "Schmidt".
        if ((current == 0 && StringAt(w, current + 1, "M", "N", "L", "W")) || StringAt(w, current + 1, "Z"))
        {
            Add(primary, secondary, "S", "X");
            return StringAt(w, current + 1, "Z") ? current + 2 : current + 1;
        }
        if (StringAt(w, current, "SC"))
        {
            // Schlesinger's rule.
            if (w[current + 2] == 'H')
            {
                // Dutch origin: "school", "schooner" are hard.
                if (StringAt(w, current + 3, "OO", "ER", "EN", "UY", "ED", "EM"))
                {
                    // "Schermerhorn", "Schenker" split the two readings.
                    Add(primary, secondary, StringAt(w, current + 3, "ER", "EN") ? "X" : "SK", "SK");
                    return current + 3;
                }
                Add(primary, secondary, "X", current == 0 && !IsVowel(w[3]) && w[3] != 'W' ? "S" : "X");
                return current + 3;
            }
            if (StringAt(w, current + 2, "I", "E", "Y"))
            {
                Add(primary, secondary, "S", "S");
                return current + 3;
            }
            Add(primary, secondary, "SK", "SK");
            return current + 3;
        }
        // French "Resnais", "Artois": the final S is silent on the primary only.
        if (current == last && StringAt(w, current - 2, "AI", "OI"))
        {
            Add(primary, secondary, string.Empty, "S");
        }
        else
        {
            Add(primary, secondary, "S", "S");
        }
        return StringAt(w, current + 1, "S", "Z") ? current + 2 : current + 1;
    }

    private static int EncodeW(string w, StringBuilder primary, StringBuilder secondary, int current, int last)
    {
        if (StringAt(w, current, "WR"))
        {
            Add(primary, secondary, "R", "R");
            return current + 2;
        }
        if (current == 0 && (IsVowel(w[current + 1]) || StringAt(w, current, "WH")))
        {
            // "Wasserman" should meet "Vasserman", so the alternate carries an F; "Uomo" and
            // "Womo" need the plain vowel, which is what the WH branch leaves.
            Add(primary, secondary, "A", IsVowel(w[current + 1]) ? "F" : "A");
            return current + 1;
        }
        // "Arnow" should meet "Arnoff", and the Polish "-owski" endings behave the same way.
        if ((current == last && IsVowel(w[current - 1]))
            || StringAt(w, current - 1, "EWSKI", "EWSKY", "OWSKI", "OWSKY")
            || StringAt(w, 0, "SCH"))
        {
            Add(primary, secondary, string.Empty, "F");
            return current + 1;
        }
        if (StringAt(w, current, "WICZ", "WITZ"))
        {
            Add(primary, secondary, "TS", "FX"); // Polish "Filipowicz"
            return current + 4;
        }
        return current + 1;
    }

    /// <summary>Appends to both codes; a null <paramref name="alternate"/> repeats <paramref name="main"/>.</summary>
    private static void Add(StringBuilder primary, StringBuilder secondary, string main, string? alternate)
    {
        primary.Append(main);
        secondary.Append(alternate ?? main);
    }

    private static bool IsVowel(char c) => c is 'A' or 'E' or 'I' or 'O' or 'U' or 'Y';

    /// <summary>Whether <paramref name="w"/> looks Slavic or Germanic, which several rules turn on.</summary>
    /// <remarks>
    /// Scanned with <see cref="StringAt"/> rather than <c>string.Contains</c> because the
    /// comparison overload that takes a <see cref="StringComparison"/> does not exist on
    /// netstandard2.0, and this package ships one API at two target frameworks.
    /// "WITZ" is subsumed by "W" and is listed anyway, so the set matches the published rule.
    /// </remarks>
    private static bool IsSlavoGermanic(string w)
    {
        for (int i = 0; i < w.Length; i++)
        {
            if (w[i] is 'W' or 'K' || StringAt(w, i, "CZ", "WITZ"))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>Whether <paramref name="w"/> holds any of <paramref name="candidates"/> at <paramref name="start"/>.</summary>
    /// <remarks>
    /// A negative <paramref name="start"/> is a miss rather than an error: the rules above index
    /// backwards freely ("current - 4") and read the absence of context as a failed match, which
    /// is what the published description does with its own padded buffer.
    /// </remarks>
    private static bool StringAt(string w, int start, params string[] candidates)
    {
        if (start < 0)
        {
            return false;
        }
        foreach (string candidate in candidates)
        {
            if (start + candidate.Length <= w.Length
                && w.AsSpan(start, candidate.Length).SequenceEqual(candidate))
            {
                return true;
            }
        }
        return false;
    }
}

#pragma warning restore S3776, S1479
