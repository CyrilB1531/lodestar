# 0086 — Russian follows nltk's ending table, and the published description's alphabet

**Status:** accepted · **Date:** 2026-09-09

## Context

Russian is the first Cyrillic stemmer in the package, and the first whose reference
implementation does not run on the letters the algorithm is written in. `nltk`'s
`RussianStemmer.stem` transliterates the word into a Roman scheme first —
`ж` → `zh`, `щ` → `shch`, `ц` → `t^s`, `я` → `i^a`, `ь` → `'`, `ъ` → `''` — runs
every step on that string, and transliterates back at the end.

Two consequences of that, measured against `nltk` 3.10.1, have to be decided
before the implementation can be read against the published description.

### 1. One pair in the adjectival table is spelled wrong

The published algorithm gives an ADJECTIVAL ending as an ADJECTIVE ending
optionally preceded by a PARTICIPLE ending. `nltk` flattens that into 234 entries:
the 26 bare adjective endings, plus 26 × 8 pairs. All 234 are there, and exactly
one is misspelled — `ующая` is written `ui^ushchaia` where the scheme spells it
`ui^ushchai^a`, so it is the string `ующаиа`, which no word can end with.

The entry is therefore dead, and a `-ующая` word falls through to `ющая`, whose
group requires an `а` or `я` before it — a `-ующая` word always has `у` there — and
then to the bare `ая`:

```text
рискующая   nltk 3.10.1 → рискующ        published description → риск
рискующий   nltk 3.10.1 → риск           published description → риск
```

The masculine, neuter and plural forms of the same word are unaffected. It is the
one ending in 234.

### 2. The Roman scheme is not a bijection under suffix stripping

`ь` is one apostrophe and `ъ` is two, so step 4's "if the word ends with `ь`,
remove it" removes half of a final `ъ` and leaves a `ь` behind. `ц` is `t^s`, and
a step that cuts inside it leaves a literal caret in the output:

```text
подъём    nltk 3.10.1 → подь      объём  nltk 3.10.1 → обь
отць      nltk 3.10.1 → отт^
```

`отт^` is not a Cyrillic string at all. Sweeping 9 454 words built from stems
ending `ъ`, `ь`, `й`, `ж`, `ц`, `щ`, `х`, `ч`, `ш` crossed with every ending of
every table, the artifacts fall into three shapes: a stem left ending in `ъ`, a
stem left ending in `ц`, and a word beginning with `й` — whose `i` the region
scan reads as a vowel, because in the Roman scheme it is one. Of the three, only
the first is reachable from real Russian: `подъём` and `объём`, whose oblique
forms (`подъёма`, `объёмы`) agree because step 4 never fires on them.

## Decision

**Follow `nltk` on the table. Follow the published description on the alphabet.**

`RussianSnowballStemmer` reproduces the `ующая` hole — one named pair, one
condition, cited to this decision in `TryAdjectival` — and implements step 4 over
Cyrillic letters, so `подъём` stems to `подъ` and not to `подь`.

The two resolve differently because they are different kinds of difference. The
first is the reference library's *rules*: a user migrating off `nltk` has been
getting `рискующ` and their index is built on it, which is the case
[`0008`](0008-italian-enza-nltk-divergence.md) settled for Italian `enza` and
[`0005`](0005-hamming-jellyfish-divergence.md) for `jellyfish`'s Hamming — parity
is the contract, and a rule table is exactly what parity is about.

The second is the reference library's *representation* leaking through its own
output. Matching it is not a matter of copying a row: `ъ` would have to be modelled
as two `ь` in every suffix match and in the region scan, which is no longer an
implementation of the published description but a port of the transliteration —
what [`0003`](0003-provenance-and-licensing.md) and this lot's brief both refuse.
And it would ship `отт^`, a stem the algorithm's alphabet cannot express.

Two smaller points the lot also had to settle, both of which follow the published
description and agree with `nltk`:

- **`ё` folds to `е`** before the rules run. It is not a letter of the alphabet the
  algorithm is written on, and `nltk` transliterates it to `e` as well, so `ёлка`
  and `елка` are one key on both sides.
- **The scan is over UTF-16 units.** Every letter of the Russian alphabet is one
  code point in the BMP and one `char`, so a code-point mode would read the same
  word; there is no `TextElement` choice to offer here, and none is offered.

## Consequences

- The divergence is `подъём` → `подъ` and `объём` → `объ`, and nothing else that
  occurs in Russian text. `tests/oracles/snowball_ru.json` is frozen from `nltk`
  and does not carry those two words, because it is compared exactly;
  `RussianSnowballStemmerTests` pins them instead, so the decision is asserted
  rather than merely written down.
- `docs/equivalence.md`'s Russian row says "exact parity except a stem left ending
  in `ъ`" rather than "exact parity", and is the only stemming row that does.
- If `nltk` ever fixes the `ующая` spelling, the corpus drifts and the
  `Oracles are reproducible` job catches it. At that point this decision should be
  revisited — the one-line condition in `TryAdjectival` deleted — rather than the
  corpus quietly regenerated.
