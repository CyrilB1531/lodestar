# NorwegianSnowballStemmer.Stem

The Snowball stem of one Norwegian word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Norwegian Bokmål word. It is lowercased and NFC-normalised
before the rules run, so a decomposed `å` and a composed one are the same letter to the algorithm.

**Returns** — `string`, the stem, always lowercase, and still carrying whatever `æ`, `å` or `ø`
the input had.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — a participle rewritten rather than stripped, and a consonant pair losing its `t`.

```csharp
using Lodestar.Text.Stemming;

string participle = NorwegianSnowballStemmer.Stem("servert");  // => server
string past = NorwegianSnowballStemmer.Stem("serverte");  // => server
string neuter = NorwegianSnowballStemmer.Stem("halvt");  // => halv
```

**Remarks** — `mulighet` → `mul` is the result most likely to look like a bug, and it is two
steps rather than one: `het` goes in step 1, which uncovers `mulig`, and `ig` then goes in
step 3. The algorithm has no dictionary to tell it the remainder is not a word, and it does not
need one — `mulighet`, `muligheten` and `mulighetene` all reaching `mul` is exactly the point.

```csharp
using Lodestar.Text.Stemming;

string noun = NorwegianSnowballStemmer.Stem("mulighet");  // => mul
string definite = NorwegianSnowballStemmer.Stem("muligheten");  // => mul
string plural = NorwegianSnowballStemmer.Stem("mulighetene");  // => mul
```

The bare `s` is the other rule worth knowing, and the only one that inspects a letter outside R1.
It is removed only after one of eighteen letters, or after a `k` that is not itself preceded by a
vowel. That last clause is what parts two words that look alike:

```csharp
using Lodestar.Text.Stemming;

string genitive = NorwegianSnowballStemmer.Stem("folks");  // => folk
string noun = NorwegianSnowballStemmer.Stem("boks");  // => boks
```

`folks` has an `l` before its `k`, so the `s` goes; `boks` has an `o`, so the `s` stays and the
word is its own stem.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NorwegianSnowballStemmer`](norwegiansnowballstemmer.md),
[the stemming index](../stemming.md).
