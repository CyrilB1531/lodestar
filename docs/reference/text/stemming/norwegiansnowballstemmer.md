# NorwegianSnowballStemmer

Norwegian Bokmål stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class NorwegianSnowballStemmer
```

**Example** — a noun, its plural and its definite plural, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string singular = NorwegianSnowballStemmer.Stem("gutt");  // => gutt
string definite = NorwegianSnowballStemmer.Stem("gutten");  // => gutt
string plural = NorwegianSnowballStemmer.Stem("guttene");  // => gutt
string genitive = NorwegianSnowballStemmer.Stem("guttenes");  // => gutt
```

**Remarks** — **This is Bokmål, and only Bokmål.** The reference is nltk's `norwegian`, which
implements the Bokmål algorithm; Nynorsk has no oracle here and is out of scope for the same
reason Turkish is. A Nynorsk word will return *something*, and that something is noise.

Norwegian is built like the Swedish stemmer next door: three steps, all of them searching R1,
with no R2 and no RV region. Two things beyond that are worth knowing.

**The region is part of the search, not a test after it.** Each step looks for the longest suffix
that *lies in R1* — not the longest suffix, checked against R1 afterwards. This is the rule the
German stemmer states the other way round, and the one most likely to be got wrong by hand.

**One group of endings is rewritten rather than stripped.** `ert` and `erte` both become `er`, so
`servert` and `serverte` meet at `server` instead of at two different keys.

`æ`, `å` and `ø` are letters of the alphabet rather than accented forms. All three count as
vowels, and all three come back exactly as they went in — `hånd` and `hånden` stem to `hånd`, not
to `hand`.

R1 is floored at three characters as in German, which is what stops short words from being trimmed
to nothing — the job RV does in the Romance languages. `år` and `året` come back whole for that
reason, and so does `boks`, whose `s` follows a `k` that is itself after a vowel.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("norwegian")`, matched over 244 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`NorwegianSnowballStemmer.Stem`](norwegiansnowballstemmer-stem.md) | The Snowball stem of one Norwegian word. |
