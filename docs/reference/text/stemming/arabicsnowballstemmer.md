# ArabicSnowballStemmer

Arabic stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class ArabicSnowballStemmer
```

**Example** — a noun, the same noun with the definite article, and with a possessive, on one key.

```csharp
using Lodestar.Text.Stemming;

string bare = ArabicSnowballStemmer.Stem("كتاب");  // => كتاب
string defined = ArabicSnowballStemmer.Stem("الكتاب");  // => كتاب
string possessive = ArabicSnowballStemmer.Stem("كتابها");  // => كتاب
```

**Remarks** — Arabic is the odd one of the sixteen, and deliberately so:
[#176](https://github.com/CyrilB1531/lodestar/issues/176) took it last because its algorithm shares
least with the others. Three things follow from that.

**It uses neither R1 nor R2.** Every other stemmer here computes a region from the first
vowel-then-consonant boundary and asks whether a suffix lies inside it. The published Arabic
algorithm has no region at all — each rule carries an explicit character count instead, and a
four-letter word is simply refused a rule a five-letter word gets. This is why
`ArabicSnowballStemmer` is the one stemmer that does not derive from the shared worker base; see
[decision 0094](../../../decisions/0094-arabic-takes-snowballstemmer-and-stands-outside-the-worker-base.md).

**Normalisation runs first, and it is half the algorithm.** The vocalisation marks and the kasheeda
are dropped, the lam-alef ligatures are written back as two letters, and the Arabic-Indic digits
become the ASCII ones — so `مُحَمَّد` and `محمد` are the same word to the rules, and `١٢٣` comes
back as `123`. The hamza carriers are resolved *last* instead: they survive the affix steps and
then fold, an alef-hamza becoming a bare alef inside a word and a lone hamza at the end of one.

**Affixes come off both ends.** The suffixes go first — possessives, then case and verb endings,
then the nisba `ي` — and the prefixes after: the conjunctions `و` and `ف` when they do not sit in
front of an alef, the definite article alone or behind a preposition, and the future markers, whose
`س` is dropped. A word carrying the article is read as a defined noun, and its possessive endings
are left alone because the article is what identifies it.

Reference behaviour is `snowballstemmer.stemmer("arabic")` rather than `nltk` — the only other
language whose corpus is frozen that way is Hungarian, and
[decision 0094](../../../decisions/0094-arabic-takes-snowballstemmer-and-stands-outside-the-worker-base.md)
has the measurement. Matched over 146 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`ArabicSnowballStemmer.Stem`](arabicsnowballstemmer-stem.md) | The Snowball stem of one Arabic word. |
