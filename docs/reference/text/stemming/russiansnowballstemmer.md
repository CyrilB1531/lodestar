# RussianSnowballStemmer

Russian stemming by the Snowball algorithm.

<!-- docs-declaration -->

```csharp
public static class RussianSnowballStemmer
```

**Example** — a noun and two of its twelve case forms, collapsed onto one key.

```csharp
using Lodestar.Text.Stemming;

string nominative = RussianSnowballStemmer.Stem("город");  // => город
string genitive = RussianSnowballStemmer.Stem("города");  // => город
string instrumental = RussianSnowballStemmer.Stem("городами");  // => город
```

**Remarks** — Russian is the only Cyrillic stemmer here, and the one whose regions are measured
differently from every other. Four things about it are worth knowing before reading a stem.

**RV is the rest of the word after its first vowel**, and nothing like the Romance RV, which
counts letters two and three. Steps 1 and 2 search inside it, which is what stops a short word
from being cut to nothing. R2 is measured the ordinary way and is used by step 3 alone; R1 is
never used.

**Step 1 is four tables tried in order.** A perfective gerund ending ends the step on its own;
otherwise a reflexive `ся`/`сь` comes off first, and then the first of an adjectival, a verb and a
noun ending that matches. Three of those tables come in two groups, where the first group counts
only when the letter before it is `а` or `я` — `читаемый` loses its `ем` and `любимый` does not.

**`ё` is folded to `е` before the rules run.** It is not a letter of the alphabet the algorithm is
written on, so `ёлка` and `елка` are one key. Every other letter comes back as it went in, and
every one of them is a single UTF-16 unit, so there is no code-point mode to choose.

**One ending does not agree with the published description**, deliberately. `рискующая` stems to
`рискующ` and not to `риск`, because `nltk`'s table misspells that one pair of 234 and parity with
`nltk` is the contract — see [decision 0086](../../../decisions/0086-russian-follows-nltks-table-and-the-descriptions-alphabet.md),
which also names the one place this stemmer does *not* follow `nltk`.

Reference behaviour is `nltk.stem.snowball.SnowballStemmer("russian")`, matched over 291 words.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the stemming index](../stemming.md).

## Members

| Member | What it does |
| --- | --- |
| [`RussianSnowballStemmer.Stem`](russiansnowballstemmer-stem.md) | The Snowball stem of one Russian word. |
