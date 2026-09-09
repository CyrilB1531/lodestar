# SwedishSnowballStemmer

Swedish stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class SwedishSnowballStemmer
```

**Example** — a noun, its plural and its definite plural, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string singular = SwedishSnowballStemmer.Stem("flickor");  // => flick
string plural = SwedishSnowballStemmer.Stem("flickorna");  // => flick
string genitive = SwedishSnowballStemmer.Stem("flickornas");  // => flick
```

**Remarks** — Swedish is the shortest of the eight: three steps, all of them in R1, with no R2
and no RV region. Three things about it are worth knowing before reading a stem.

**The region is part of the search, not a test after it.** Each step looks for the longest suffix
that *lies in R1* — not the longest suffix, checked against R1 afterwards. `nyheter` ends with
`heter`, which reaches past R1, and with `er`, which does not; `er` is the one that goes, and the
word stems to `nyhet`. This is the rule the German stemmer next door states the other way round,
and it is the one most likely to be got wrong by hand.

**Nothing is folded, before or after.** `ä`, `å` and `ö` are letters of the Swedish alphabet, all
three count as vowels, and all three come back exactly as they went in — so `tjänstemännens` stems
to `tjänstemän` and not to `tjansteman`.

**Two of the endings are rewritten rather than stripped.** `löst` becomes `lös` and `fullt`
becomes `full`, so `hjälplöst` and `kärleksfullt` keep a stem that is still a Swedish word.

R1 is floored at three characters as in German, which is what stops short words from being trimmed
to nothing — the job RV does in the Romance languages. `hos` and `os` come back whole for that
reason, and so does `cirkus`, whose `s` follows a `u` rather than one of the seventeen letters the
algorithm accepts before a bare `s`.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("swedish")`, matched over 211 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`SwedishSnowballStemmer.Stem`](swedishsnowballstemmer-stem.md) | The Snowball stem of one Swedish word. |
