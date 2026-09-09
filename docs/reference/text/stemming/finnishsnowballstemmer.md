# FinnishSnowballStemmer

Finnish stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class FinnishSnowballStemmer
```

**Example** — a noun in four of its fifteen cases, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string inessive = FinnishSnowballStemmer.Stem("talossa");  // => talo
string adessive = FinnishSnowballStemmer.Stem("talolla");  // => talo
string illative = FinnishSnowballStemmer.Stem("taloon");  // => talo
string possessive = FinnishSnowballStemmer.Stem("talonsa");  // => talo
```

**Remarks** — Finnish is the longest of the twelve, and the reason is grammatical rather than
incidental: it is agglutinative, so one noun carries fifteen cases, and each of those can take a
possessive and then a particle on top. The algorithm peels them in that order — six steps over
standard R1 and R2, with no floor and no RV region.

**The steps come off in the order they went on.** Particles first (`-kin`, `-ko`, `-han`), then
possessives (`-ni`, `-nsa`, `-mme`), then cases, then comparatives, then plurals, then a tidying
pass. `talossaan` gives up its `-an` as a possessive and only then its `-ssa` as a case, which is
why a word can lose three endings in one pass and still reach the same key as the bare stem.

**Vowel harmony decides which list applies.** Finnish suffixes come in back and front pairs, and
the algorithm keeps them apart: `-ssa` after a back vowel, `-ssä` after a front one, and a
possessive `-an` that follows only the first while `-än` follows only the second. This is what the
other eleven languages have no equivalent of.

**Three of the steps read what sits in front of the ending.** `hXn` needs its own vowel repeated
before it, `-siin` needs a vowel and an `i`, `-seen` needs a long vowel, and the partitive `-a`
needs a consonant and a vowel. A condition that fails leaves the word alone rather than falling
through to a shorter ending — with four measured exceptions, in
[decision 0090](../../../decisions/0090-finnish-falls-back-to-the-genitive-where-nltk-does.md).

The last step is what makes a Finnish stem look short. It removes a long vowel, then a consonant
and one of `a ä e i`, then an `-oj`, `-uj` or `-jo`, and finally the second letter of any doubled
consonant — so `kissaa` reaches `kis` and `kotimaahan` reaches `kotim`.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("finnish")`, matched over 217 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`FinnishSnowballStemmer.Stem`](finnishsnowballstemmer-stem.md) | The Snowball stem of one Finnish word. |
