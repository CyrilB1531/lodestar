# DanishSnowballStemmer.Stem

The Snowball stem of one Danish word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Danish word. It is lowercased and NFC-normalised before the
rules run, so a decomposed `æ` and a composed one are the same letter to the algorithm.

**Returns** — `string`, the stem, always lowercase, and still carrying whatever `æ`, `å` or `ø`
the input had.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — a derived adjective stripped, and one rewritten rather than stripped.

```csharp
using Lodestar.Text.Stemming;

string noun = DanishSnowballStemmer.Stem("kærlighed");  // => kær
string genitive = DanishSnowballStemmer.Stem("kærlighedens");  // => kær
string adjective = DanishSnowballStemmer.Stem("hjælpeløst");  // => hjælpeløs
```

**Remarks** — `kærlighed` → `kær` is the result most likely to look like a bug. It is not: `hed`
is one of the endings the first step removes, `lig` is one of the four the third step removes, and
the algorithm has no dictionary to tell it that what remains happens to be another word. A stemmer
produces keys, not words, and `kærlighed`, `kærlig` and `kærligt` all reaching `kær` is the point
of it.

The superlative is the other one worth knowing. A word ending `igst` loses the `st` before the
third step's suffix search runs at all, which is what puts a superlative and its positive on one
key — and the consonant-pair step then removes a `gt` or a `kt` inside R1, which does the same for
a neuter form.

```csharp
using Lodestar.Text.Stemming;

string superlative = DanishSnowballStemmer.Stem("hurtigst");  // => hurt
string positive = DanishSnowballStemmer.Stem("hurtigt");  // => hurt
string neuter = DanishSnowballStemmer.Stem("friskt");  // => frisk
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DanishSnowballStemmer`](danishsnowballstemmer.md),
[the stemming index](../stemming.md).
