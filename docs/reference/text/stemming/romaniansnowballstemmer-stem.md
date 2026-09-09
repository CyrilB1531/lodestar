# RomanianSnowballStemmer.Stem

The Snowball stem of one Romanian word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Romanian word. It is lowercased and NFC-normalised before the
rules run, so a decomposed `ă` and a composed one are the same letter to the algorithm. The
cedilla `ş` and the comma-below `ș` are **not** the same letter — see the remarks below.

**Returns** — `string`, the stem, always lowercase, carrying whatever `ă`, `â`, `î`, `ş` or `ș`
the input had.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — a verb reduced to the stem its tenses share.

```csharp
using Lodestar.Text.Stemming;

string infinitive = RomanianSnowballStemmer.Stem("cânta");  // => cânt
string imperfect = RomanianSnowballStemmer.Stem("cântam");  // => cânt
string gerund = RomanianSnowballStemmer.Stem("cântând");  // => cânt
```

**Remarks** — the result most likely to look like a bug is a modern spelling that comes back
almost whole. Every suffix table is written with the cedilla `ş`/`ţ`, as the published description
writes them, so a word using the correct comma-below `ș`/`ț` matches only the endings that contain
neither letter. `informaţie` reaches `inform`; `informație` reaches `informaț`. That is the
reference's behaviour, not an oversight — and if your text is modern Romanian, replace the two
letters before you call, so both spellings meet on one key.

```csharp
using Lodestar.Text.Stemming;

string old = RomanianSnowballStemmer.Stem("informaţie");  // => inform
string modern = RomanianSnowballStemmer.Stem("informație");  // => informaț
string normalised = RomanianSnowballStemmer.Stem("informație".Replace('ș', 'ş').Replace('ț', 'ţ'));  // => inform
```

The other one worth knowing is that a stemmer produces keys, not words. `muncitor`, `muncitori`
and `muncitoare` all reach `muncit`, which is also where `muncit` itself lands — the point of the
derivational step is exactly that collision, and the algorithm has no dictionary to tell it that
what remains happens to be a participle.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RomanianSnowballStemmer`](romaniansnowballstemmer.md),
[the stemming index](../stemming.md).
