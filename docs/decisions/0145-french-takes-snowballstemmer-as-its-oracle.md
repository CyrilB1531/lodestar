---
status: accepted
supersedes: []
amends: []
applies: ["0091"]
---
# 0145 — French takes `snowballstemmer` as its oracle, not `nltk`

**Status:** accepted · **Date:** 2026-09-17

## Context

`FrenchSnowballStemmer` replayed a corpus frozen from `nltk`'s `FrenchStemmer`, and
[#973](https://github.com/CyrilB1531/lodestar/issues/973) measured it against the whole of
`/usr/share/dict/french`: 8,009 of 346,244 stems differed from `nltk` 3.10.3. Rewriting the stemmer
to match `nltk` exactly brought that to zero. That version had to copy how `nltk` reads the algorithm,
and in three places the result is not the published algorithm:

| rule | Snowball | `nltk` 3.10.3 |
| --- | --- | --- |
| `-atrice` after `ic` | deletes the `ic` only in R2: `indicatrice` → `indiqu` | tests R2 on a string that still holds the deleted `atrice`: `ind` |
| step 4 `ion` after `s`/`t` | reads the letter before `ion` | reads RV as it was before a final `s` went |
| after `-emment` | RV is a position, unaffected | RV stays the string before the replacement |

`nltk` also implements an older revision of the algorithm. `snowballstemmer` 3.1.1, the Snowball
project's own package, removes elisions (`l'avion`), handles `ë` and `ï` (`canoë` → `cano`),
`-oux` (`bijoux` → `bijou`), `-aise` (`albanaise` → `alban`) and a `ni-` prefix. It also un-accents a
first letter (`ès` → `es`). Across the dictionary it and `nltk` disagree on 278 words.

## Decision

The class is named `FrenchSnowballStemmer`, so it implements the **Snowball** algorithm. Its corpus is
frozen from `snowballstemmer` 3.1.1, as [`0091`](0091-hungarian-takes-snowballstemmer-as-its-oracle.md)
did for Hungarian, and `nltk`'s readings are not reproduced.

## Options

**Follow `nltk`, as [`0008`](0008-italian-enza-nltk-divergence.md) did for Italian `enza`.**
Rejected by the maintainer on #973. Those three readings come from how `nltk` holds its regions, not
from a rule. `nltk` also lacks rules from the current algorithm, and they reach ordinary words
(`bijoux`, `albanaise`, `l'avion`).

**Take `snowballstemmer`.** Chosen. It is already pinned for Hungarian and Arabic, so the dependency
graph does not grow.

## Consequences

- `tools/generate_oracles.py` freezes `snowball_fr.json` from `snowballstemmer`. Of the 182 words the
  corpus held before #973, one moved: `ès`, now `es`.
- The stemmer agrees with `snowballstemmer` on all 346,244 dictionary words and on 155,400 generated
  words. `nltk`'s `FrenchStemmer` still differs on the 278 dictionary words above, and
  `docs/equivalence.md` says so.
- Under 0091's rule, a move is decided per language on measured evidence. This decision does not move
  the other languages.
