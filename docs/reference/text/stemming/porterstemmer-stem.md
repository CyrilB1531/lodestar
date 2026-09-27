# PorterStemmer.Stem

The Porter stem of one English word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
public static string Stem(string word, PorterStemmerMode mode)
```

**Parameters** — `word` is a single word, not a sentence. It is lowercased before the first step
runs. `mode` picks the published form: [`PorterStemmerMode.OriginalAlgorithm`](porterstemmermode.md),
what the first overload applies, or `PorterStemmerMode.MartinExtensions`.

**Returns** — `string`, the stem, always lowercase.

**Exceptions** — `ArgumentNullException` when `word` is `null`; `ArgumentException` when `mode` is
not a defined `PorterStemmerMode`. An empty string is returned unchanged. A word of one or two
letters is stemmed like any other by the paper — `"as"` becomes `"a"` — and returned lowercased and
unchanged under Martin's extensions.

**Example** — the same word under both English stemmers, which is the only reason to prefer this
one.

```csharp
using Lodestar.Text.Stemming;

string original = PorterStemmer.Stem("ties");  // => ti
string revised = EnglishSnowballStemmer.Stem("ties");  // => tie
```

**Remarks** — `ti` is not a mistake in this implementation; it is what the 1980 algorithm
specifies, and `nltk` returns it too. Preserving that is the point of shipping the original at
all. If the answer looks wrong, the fix is
[`EnglishSnowballStemmer.Stem`](englishsnowballstemmer-stem.md), not a patch here.

The two modes part on three rules, and only there: the paper turns `abli` into `able` where Martin's
implementation turns any `bli` into `ble`, only Martin's turns `logi` into `log`, and only Martin's
leaves a word of two letters alone.

```csharp
using Lodestar.Text.Stemming;

string paper = PorterStemmer.Stem("analogy");  // => analogi
string martin = PorterStemmer.Stem("analogy", PorterStemmerMode.MartinExtensions);  // => analog
```

Only ASCII letters are treated as letters. Accented input is neither rejected nor folded — it is
simply not what the rules were written for, and another language's stemmer is the right answer for
another language's text.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PorterStemmer`](porterstemmer.md), [`PorterStemmerMode`](porterstemmermode.md),
[`EnglishSnowballStemmer.Stem`](englishsnowballstemmer-stem.md),
[the stemming index](../stemming.md).
