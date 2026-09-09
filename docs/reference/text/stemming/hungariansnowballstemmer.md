# HungarianSnowballStemmer

Hungarian stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class HungarianSnowballStemmer
```

**Example** — a noun, its plural and two of its cases, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string nominative = HungarianSnowballStemmer.Stem("ház");  // => ház
string plural = HungarianSnowballStemmer.Stem("házak");  // => ház
string inessive = HungarianSnowballStemmer.Stem("házban");  // => ház
string possessive = HungarianSnowballStemmer.Stem("házam");  // => ház
```

**Remarks** — Hungarian is agglutinative, so a noun carries case, number and possession as a
stack of endings rather than one. That is why this algorithm has **nine steps** where the
Germanic ones have three: each step peels one layer, in the order the layers are built.

**Its R1 is its own shape**, and neither of the two the other languages use. If the word opens
with a vowel, R1 begins after the first consonant; if it opens with a consonant, after the first
vowel. There is no R2 and no RV region.

**A digraph is one consonant.** `cs`, `dz`, `dzs`, `gy`, `ly`, `ny`, `sz`, `ty` and `zs` each
count as a single letter when R1 is measured, so `ország` opens its region after the `sz`, not
inside it.

**A double consonant left by an ending is undoubled.** The instrumental and factive cases attach
by doubling — `vas` becomes `vassal`, `hús` becomes `hússá` — so those two steps drop the ending
*and* one letter of the pair, and both words come back to their stem.

```csharp
using Lodestar.Text.Stemming;

string instrumental = HungarianSnowballStemmer.Stem("vassal");  // => vas
string factive = HungarianSnowballStemmer.Stem("hússá");  // => hús
```

**The reference here is not `nltk`.** It is `snowballstemmer`, the Snowball project's own
package. `nltk` 3.10.1's Hungarian omits `ő` and `ű` from its vowel set and three suffixes from
step 2, which leaves `nők`, `szőlők`, `gyűrűk` and every `-ből` form unstemmed —
[decision 0091](../../../decisions/0091-hungarian-takes-snowballstemmer-as-its-oracle.md) has the
measurement. Every other language here still replays `nltk`.

Reference behaviour is `snowballstemmer.stemmer("hungarian")`, matched over 211 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`HungarianSnowballStemmer.Stem`](hungariansnowballstemmer-stem.md) | The Snowball stem of one Hungarian word. |
