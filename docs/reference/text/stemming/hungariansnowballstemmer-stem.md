# HungarianSnowballStemmer.Stem

The Snowball stem of one Hungarian word.

<!-- docs-declaration -->

```csharp
public static string Stem(string word)
```

**Parameters** — `word` is a single Hungarian word. It is lowercased and NFC-normalised before
the rules run, which matters more here than elsewhere: `ő` and `ű` are the letters the algorithm
is most often got wrong on, and a decomposed one would not be recognised as a vowel.

**Returns** — `string`, the stem, always lowercase, with every accent it carried still on it.
Nothing is folded: `á` and `a` are different letters to this algorithm.

**Exceptions** — `ArgumentNullException` when `word` is `null`. An empty string, or a word of one
character, is returned lowercased and otherwise untouched.

**Example** — the two long vowels that make Hungarian awkward, stemmed correctly.

```csharp
using Lodestar.Text.Stemming;

string singular = HungarianSnowballStemmer.Stem("gyűrű");  // => gyűrű
string plural = HungarianSnowballStemmer.Stem("gyűrűk");  // => gyűrű
string vine = HungarianSnowballStemmer.Stem("szőlők");  // => szőlő
```

**Remarks** — those three are the reason this stemmer is checked against `snowballstemmer` and
not against `nltk` like its siblings. `ő` and `ű` are missing from `nltk` 3.10.1's Hungarian vowel
set, so R1 lands past the end of the word and nothing is stripped: `gyűrű` and `gyűrűk` stay two
different keys there, which is the one thing a stemmer exists to prevent. See
[decision 0091](../../../decisions/0091-hungarian-takes-snowballstemmer-as-its-oracle.md).

A word can lose several layers in one call, because the nine steps run in sequence and each may
fire. `erdőből` is `erdő` plus the elative `-ből`, and comes back as the bare noun:

```csharp
using Lodestar.Text.Stemming;

string elative = HungarianSnowballStemmer.Stem("erdőből");  // => erdő
string plural = HungarianSnowballStemmer.Stem("erdők");  // => erdő
string bare = HungarianSnowballStemmer.Stem("erdő");  // => erdő
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`HungarianSnowballStemmer`](hungariansnowballstemmer.md),
[the stemming index](../stemming.md).
