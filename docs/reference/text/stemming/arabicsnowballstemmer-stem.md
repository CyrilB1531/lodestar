# ArabicSnowballStemmer.Stem

The Snowball stem of one Arabic word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Arabic word. Vocalisation marks, the kasheeda and the
lam-alef ligatures are resolved before the rules run, so a fully vocalised spelling and a bare one
reach the same stem.

**Returns** — `string`, the stem. Text the algorithm has no rule for — Latin letters, ASCII digits
— comes back unchanged.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned unchanged.

**Example** — the definite article and a preposition in front of it, and a feminine noun.

```csharp
using Lodestar.Text.Stemming;

string article = ArabicSnowballStemmer.Stem("بالكتاب");  // => كتاب
string feminine = ArabicSnowballStemmer.Stem("المدرسة");  // => مدرس
string vocalised = ArabicSnowballStemmer.Stem("مُحَمَّد");  // => محمد
```

**Remarks** — `المدرسة` → `مدرس` shows both ends working at once: the feminine `ة` goes with the
suffix steps and the article `ال` with the prefix steps, and the same `مدرس` is what `مدرسة` and
`مدرستها` reach. A stemmer produces keys, not words.

The two results below are the ones most likely to look wrong. `معلمات` keeps its `ا`, because the
rule that removes a plural `ات` needs a longer word than this one and only the `ت` is taken; and
`١٢٣` becomes `123`, because folding the Arabic-Indic digits onto the ASCII ones is part of the
published normalisation rather than an accident.

```csharp
using Lodestar.Text.Stemming;

string plural = ArabicSnowballStemmer.Stem("معلمات");  // => معلما
string digits = ArabicSnowballStemmer.Stem("١٢٣");  // => 123
string verb = ArabicSnowballStemmer.Stem("يستخدم");  // => استخدم
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ArabicSnowballStemmer`](arabicsnowballstemmer.md),
[the stemming index](../stemming.md).
