# DutchSnowballStemmer

Dutch stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class DutchSnowballStemmer
```

**Example** — a noun and its plural, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string singular = DutchSnowballStemmer.Stem("boek");  // => boek
string plural = DutchSnowballStemmer.Stem("boeken");  // => boek
string diminutive = DutchSnowballStemmer.Stem("boekje");  // => boekj
```

**Remarks** — Dutch is built like the German stemmer next door and differs from it in three
places worth knowing before reading a stem.

**Accents are folded before the rules run, not after.** `ä ë ï ö ü` and `á é í ó ú` become their
bare vowel first, so `één` stems as `een` does. The grave on `è` is the exception: it is a letter
of the Dutch alphabet rather than a diacritic, counts as a vowel, and survives.

**A stressed vowel is undoubled at the end.** `maan` and `manen` are one word spelled two ways,
and the last step is what makes them one key: a doubled `aa`, `ee`, `oo` or `uu` between two
consonants loses a letter, so both come back as `man`.

And there is **no RV region** — R1 and R2 only, with R1 floored at three characters as in German.
That floor is what stops short words from being trimmed to nothing, the job RV does in the Romance
languages.

Derivational endings go further than inflectional ones. `wandeling`, `wandelingen` and
`wandelend` all reach `wandel`, but only because `ing` and `end` sit inside R2; `lopend` is too
short for that and comes back whole.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("dutch")`, matched over 177 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`DutchSnowballStemmer.Stem`](dutchsnowballstemmer-stem.md) | The Snowball stem of one Dutch word. |
