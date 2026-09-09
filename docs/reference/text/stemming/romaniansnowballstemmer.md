# RomanianSnowballStemmer

Romanian stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class RomanianSnowballStemmer
```

**Example** — a noun with its enclitic article, and the same noun in the genitive.

```csharp
using Lodestar.Text.Stemming;

string bare = RomanianSnowballStemmer.Stem("copil");  // => copil
string articled = RomanianSnowballStemmer.Stem("copilul");  // => copil
string genitive = RomanianSnowballStemmer.Stem("copilului");  // => copil
```

**Remarks** — Romanian is the one Romance language among the languages added after the first six,
so it is built on the same RV region as Spanish, Portuguese and Italian rather than on R1 alone.
Four things about it are worth knowing before reading a stem.

**It has five steps, and one of them loops.** Step 0 takes the enclitic article and the plural it
attaches to; step 1 reduces the combining derivational endings *repeatedly*, so `activitate`
loses `ivitate` for `iv` and stops at `activ`; step 2 takes the standard endings against R2;
step 3 takes the verb endings, but only if neither step 1 nor step 2 removed anything; step 4
takes a final vowel.

**Only step 3's region qualifies the search.** Everywhere else the longest ending wins outright,
and one that reaches past its region stops the step. In step 3 a rejected ending falls through to
a shorter one — `casem` ends with `asem`, which reaches past RV, and with `em`, which does not, so
`em` is what goes.

**`ş` and `ș` are two different letters here.** The published description and `nltk` both write the
cedilla forms — `ş` U+015F and `ţ` U+0163 — in every suffix table, and Romanian has been correctly
written with the comma-below `ș` U+0219 and `ț` U+021B since 1993. A word typed the modern way
matches only the endings that contain neither letter, so `informaţie` stems to `inform` while
`informație` stems to `informaț`. Nothing is folded, deliberately —
[decision 0092](../../../decisions/0092-romanian-keeps-the-cedilla-alphabet-the-description-writes.md)
has why, and what a caller does about it.

**`i` and `u` between two vowels are marked before the regions are measured**, so they count as
consonants and do not close a region early. They come back as themselves.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("romanian")`, matched over 349 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`RomanianSnowballStemmer.Stem`](romaniansnowballstemmer-stem.md) | The Snowball stem of one Romanian word. |
