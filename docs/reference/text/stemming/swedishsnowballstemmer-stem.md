# SwedishSnowballStemmer.Stem

The Snowball stem of one Swedish word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Swedish word. It is lowercased and NFC-normalised before the
rules run, so a decomposed `ä` and a composed one are the same letter to the algorithm.

**Returns** — `string`, the stem, always lowercase, and still carrying whatever `ä`, `å` or `ö`
the input had.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — an inflected noun stripped, and an adjective rewritten rather than stripped.

```csharp
using Lodestar.Text.Stemming;

string plural = SwedishSnowballStemmer.Stem("nyheter");  // => nyhet
string definite = SwedishSnowballStemmer.Stem("nyheterna");  // => nyhet
string adjective = SwedishSnowballStemmer.Stem("hjälplöst");  // => hjälplös
```

**Remarks** — `härlig` → `här` is the result most likely to look like a bug. It is not: `lig` is
one of the three derivational endings the last step removes, and the algorithm has no dictionary
to tell it that what remains happens to be another word. A stemmer produces keys, not words, and
`härlig`, `härligt` and `härligheten` all reaching `här` is the point of it.

The consonant-pair step is the other one worth knowing. A word left ending in `dd`, `gd`, `nn`,
`dt`, `gt`, `kt` or `tt` inside R1 loses the second letter, which is what makes an adjective and
its neuter form one key — and what lets the derivational step then see the `ig` underneath.

```csharp
using Lodestar.Text.Stemming;

string neuter = SwedishSnowballStemmer.Stem("friskt");  // => frisk
string common = SwedishSnowballStemmer.Stem("frisk");  // => frisk
string derived = SwedishSnowballStemmer.Stem("härligt");  // => här
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SwedishSnowballStemmer`](swedishsnowballstemmer.md),
[the stemming index](../stemming.md).
