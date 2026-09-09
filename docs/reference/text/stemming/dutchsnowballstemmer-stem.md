# DutchSnowballStemmer.Stem

The Snowball stem of one Dutch word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Dutch word. It is lowercased and NFC-normalised before the
rules run, so a decomposed `é` and a composed one fold to the same `e`.

**Returns** — `string`, the stem, always lowercase and free of the umlauts and acutes the input
may have carried.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and accent-folded and otherwise untouched.

**Example** — an accent folded away, and a doubled vowel undoubled on the way out.

```csharp
using Lodestar.Text.Stemming;

string accented = DutchSnowballStemmer.Stem("één");  // => een
string singular = DutchSnowballStemmer.Stem("maan");  // => man
string plural = DutchSnowballStemmer.Stem("manen");  // => man
```

**Remarks** — `maan` → `man` is the result most likely to look like a bug, and it is the point:
Dutch spells the same vowel with one letter in an open syllable and two in a closed one, so
`manen` would never meet `maan` without the last step undoubling it.

`heden` is the other rule worth knowing. It becomes `heid` before anything else runs, which is why
`mogelijkheden` and `mogelijkheid` both reach `mogelijk` — the plural is rewritten into the
singular and then stripped once, rather than stripped twice by two different rules.

```csharp
using Lodestar.Text.Stemming;

string plural = DutchSnowballStemmer.Stem("mogelijkheden");  // => mogelijk
string singular = DutchSnowballStemmer.Stem("mogelijkheid");  // => mogelijk
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DutchSnowballStemmer`](dutchsnowballstemmer.md),
[the stemming index](../stemming.md).
