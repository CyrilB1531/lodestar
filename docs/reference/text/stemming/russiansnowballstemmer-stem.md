# RussianSnowballStemmer.Stem

The Snowball stem of one Russian word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Russian word. It is lowercased and NFC-normalised before the
rules run, and `ё` is then folded to `е`, so a decomposed `ё`, a composed one and a plain `е` are
the same letter to the algorithm.

**Returns** — `string`, the stem, always lowercase, and always spelled with `е` where the input
had `ё`.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string is returned empty,
and a word with no Russian vowel — `в`, or a word in another script — comes back whole: its RV is
empty and no table can reach it.

**Example** — a verb reduced to the stem its participle and its finite forms share.

```csharp
using Lodestar.Text.Stemming;

string infinitive = RussianSnowballStemmer.Stem("читать");  // => чита
string past = RussianSnowballStemmer.Stem("читал");  // => чита
string participle = RussianSnowballStemmer.Stem("читающий");  // => чита
```

**Remarks** — `возможность` → `возможн` and `радость` → `радост` is the pair most likely to look
like a bug. Both end in the same derivational `ость`, and step 3 removes it only when the whole
ending lies in R2: in `возможность` it does, and in the shorter `радость` it does not. `радости`
reaches the same `радост` by a different route — its `и` goes in step 1 — so the two forms still
collide, which is what the step is for.

The last step is the other one worth knowing. It does exactly one of three things: undouble a
final `нн`, or remove a superlative `ейш`/`ейше` and undouble what that uncovers, or drop a final
`ь`. A word that took the second keeps its `ь`, and that is why `сильнейший` and `сильный` meet at
`сильн` while `мысль` keeps its own.

```csharp
using Lodestar.Text.Stemming;

string abstractNoun = RussianSnowballStemmer.Stem("возможности");  // => возможн
string shortNoun = RussianSnowballStemmer.Stem("радости");  // => радост
string superlative = RussianSnowballStemmer.Stem("сильнейший");  // => сильн
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RussianSnowballStemmer`](russiansnowballstemmer.md),
[the stemming index](../stemming.md).
