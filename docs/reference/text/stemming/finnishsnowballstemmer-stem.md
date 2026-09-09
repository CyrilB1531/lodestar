# FinnishSnowballStemmer.Stem

The Snowball stem of one Finnish word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Finnish word. It is lowercased and NFC-normalised before the
rules run, so a decomposed `ä` and a composed one are the same letter to the algorithm.

**Returns** — `string`, the stem, always lowercase, and still carrying whatever `ä` or `ö` the
input had.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — a plural, a partitive and a compound illative.

```csharp
using Lodestar.Text.Stemming;

string plural = FinnishSnowballStemmer.Stem("talot");  // => talo
string partitive = FinnishSnowballStemmer.Stem("taloja");  // => talo
string illative = FinnishSnowballStemmer.Stem("kotimaahan");  // => kotim
```

**Remarks** — `kotimaahan` → `kotim` is the result most likely to look like a bug. It is not. The
illative `-han` goes with the third step, which leaves `kotimaa`; the tidying step then removes the
long vowel `aa`, leaving `kotima`, and then the consonant-plus-`a` at the end, leaving `kotim`.
Each of those runs at most once, in turn. A stemmer produces keys, not words.

The same step explains the other short answers. `kissaa` loses its partitive `-a`, then its `sa`,
then one of its doubled `s`, and ends at `kis`.

The `-sti` adverb is the one ending in the first step that has to sit in R2 rather than R1, and
that is the whole difference between the two results below: `nopeasti` is too short for it, so only
the tidying step touches the word.

```csharp
using Lodestar.Text.Stemming;

string shortAdverb = FinnishSnowballStemmer.Stem("nopeasti");  // => nopeast
string longAdverb = FinnishSnowballStemmer.Stem("todellisesti");  // => todellis
string genitive = FinnishSnowballStemmer.Stem("huoneen");  // => huone
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`FinnishSnowballStemmer`](finnishsnowballstemmer.md),
[the stemming index](../stemming.md).
