# DanishSnowballStemmer

Danish stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class DanishSnowballStemmer
```

**Example** — a noun, its definite form and the verb it comes from, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string noun = DanishSnowballStemmer.Stem("bestemmelse");  // => bestem
string definite = DanishSnowballStemmer.Stem("bestemmelsen");  // => bestem
string verb = DanishSnowballStemmer.Stem("bestemmer");  // => bestem
```

**Remarks** — Danish reads as Swedish with one step added: four steps rather than three, all of
them in R1, with no R2 and no RV region. Three things about it are worth knowing before reading a
stem.

**The fourth step undoubles.** A word left ending in `bb`, `dd`, `ff`, `gg`, `kk`, `ll`, `mm`,
`nn`, `pp`, `rr`, `ss` or `tt` loses the second letter, and it runs last — after the deletions that
uncover the pair. `bestemmelse` loses its `e`, then its `els`, reaches `bestemm`, and only then
becomes `bestem`. This is the step Swedish does not have, and the reason a Danish stem is often one
letter shorter than it looks like it should be.

**The third step runs the second one again.** Deleting `ig`, `lig`, `elig` or `els` can uncover a
`gd`, `dt`, `gt` or `kt` that was not there before, so the consonant-pair step is repeated
afterwards. `fjendtligt` loses its `t` to step 2, its `lig` to step 3, and then its second `t` to
step 2 a second time, ending at `fjend`.

**The region is part of the search, not a test after it.** As in Swedish, each step looks for the
longest suffix that *lies in R1* — not the longest suffix, checked against R1 afterwards.
`nyheder` ends with `heder`, which reaches past R1, and with `er`, which does not; `er` is the one
that goes, and the word stems to `nyhed`. `frihed` comes back whole for the same reason.

R1 is floored at three characters as in Swedish and German, which is what stops short words from
being trimmed to nothing. `virus` also comes back whole, but for the other reason: its `s` follows
a `u`, which is not one of the twenty letters the algorithm accepts before a bare `s`.

The published description gives R1 a second definition for words containing an apostrophe, which
`nltk` does not implement and neither does this — see
[decision 0087](../../../decisions/0087-danish-follows-nltk-on-the-apostrophe.md).

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("danish")`, matched over 240 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`DanishSnowballStemmer.Stem`](danishsnowballstemmer-stem.md) | The Snowball stem of one Danish word. |
