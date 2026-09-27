# PorterStemmerMode

Which of the Porter algorithm's two published forms `PorterStemmer.Stem` applies.

<!-- docs-declaration -->

```csharp
public enum PorterStemmerMode { OriginalAlgorithm, MartinExtensions }
```

**Members** — `OriginalAlgorithm` is the 1980 paper's rules as printed: `abli` becomes `able`,
`logi` is kept, and a word of one or two letters is stemmed like any other. It is nltk's
`ORIGINAL_ALGORITHM`, and the default. `MartinExtensions` is the three departures Martin Porter's
own reference implementation makes from the paper: `bli` becomes `ble`, `logi` becomes `log`, and
a word of one or two letters comes back lowercased and unstemmed. It is nltk's `MARTIN_EXTENSIONS`.

**Example** — the two modes on the words they part over.

```csharp
using Lodestar.Text.Stemming;

string paper = PorterStemmer.Stem("accessibly", PorterStemmerMode.OriginalAlgorithm);  // => accessibli
string martin = PorterStemmer.Stem("accessibly", PorterStemmerMode.MartinExtensions);  // => access
```

**Remarks** — pick the mode the index you are matching was built with; neither is better English.
An index built by Martin's C or Java implementation, or by a port of it, reads `MartinExtensions`.
nltk's own default, `NLTK_EXTENSIONS`, is a third form with rules of its own and is not offered:
[`EnglishSnowballStemmer`](englishsnowballstemmer.md) is the stemmer to reach for when no existing
index fixes the choice.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PorterStemmer.Stem`](porterstemmer-stem.md), [`PorterStemmer`](porterstemmer.md).
