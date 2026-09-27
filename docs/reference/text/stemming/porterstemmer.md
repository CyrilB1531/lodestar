# PorterStemmer

English stemming by Martin Porter's original 1980 algorithm.

<!-- docs-declaration -->

```csharp
public static class PorterStemmer
```

**Example** — the textbook plural.

```csharp
using Lodestar.Text.Stemming;

string stem = PorterStemmer.Stem("caresses");  // => caress
```

**Remarks** — reach for this **only to match an index that already exists**. Porter himself
replaced it with Porter2, published as Snowball and available here as
[`EnglishSnowballStemmer`](englishsnowballstemmer.md), which is his own correction of it.
[The index page](../stemming.md) lists all six words the two disagree about.

The algorithm is five steps of suffix rules, gated on a measure of vowel-consonant alternation
rather than on named regions — the mechanism Porter2 replaced with R1 and R2. It knows only ASCII
letters, so an accented word is not something it was built to receive.

Two forms of the algorithm are published, and [`PorterStemmerMode`](porterstemmermode.md) picks
one: the 1980 paper as printed, the default, or the departures Martin Porter's own reference
implementation makes from it. Reference behaviour is `nltk.stem.porter.PorterStemmer` in the
matching mode — `ORIGINAL_ALGORITHM` or `MARTIN_EXTENSIONS` — matched in both over every entry of a
73,456-word English dictionary.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PorterStemmerMode`](porterstemmermode.md),
[`EnglishSnowballStemmer`](englishsnowballstemmer.md), [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`PorterStemmer.Stem`](porterstemmer-stem.md) | The Porter stem of one English word, by the paper or by Martin's extensions. |
