# FrenchSnowballStemmer

French stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class FrenchSnowballStemmer
```

**Example** — a masculine, a feminine and an adverb, all one key.

```csharp
using Lodestar.Text.Stemming;

string masculine = FrenchSnowballStemmer.Stem("heureux");  // => heureux
string feminine = FrenchSnowballStemmer.Stem("heureuse");  // => heureux
string adverb = FrenchSnowballStemmer.Stem("heureusement");  // => heureux
```

**Remarks** — French inflects heavily at the end of the word, which is exactly what a suffix
stemmer is good at: `national`, `nationale` and `nationaux` all reduce to `national`, and the
`-issait`/`-issant` forms of second-group verbs reduce with their infinitive.

The algorithm uses **RV** alongside R1 and R2 — a region defined from the start of the word rather
than from a suffix, which is what lets it protect short stems that the R1/R2 pair alone would eat.
Six steps run in order, and which of them run at all depends on whether the previous one changed
the word.

Input is normalised to NFC before the rules see it, so `é` written as a combining accent behaves
like `é` written as one character.

Reference behaviour is `snowballstemmer.stemmer("french")` 3.1.1, the Snowball project's own
implementation, matched over the 571-word corpus and over all 346,244 words of a French dictionary.
`nltk`'s `SnowballStemmer("french")` implements an older reading and gives another stem for 278 of
those words: `indicatrice` is `ind` there and `indiqu` here, `bijoux` stays `bijoux`, and `l'avion`
keeps its elision ([decision 0145](../../../decisions/0145-french-takes-snowballstemmer-as-its-oracle.md)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md),
[`ItalianSnowballStemmer`](italiansnowballstemmer.md) and
[`SpanishSnowballStemmer`](spanishsnowballstemmer.md), which share the Romance region scheme.

## Members

| Member | What it does |
| --- | --- |
| [`FrenchSnowballStemmer.Stem`](frenchsnowballstemmer-stem.md) | The Snowball stem of one French word. |
