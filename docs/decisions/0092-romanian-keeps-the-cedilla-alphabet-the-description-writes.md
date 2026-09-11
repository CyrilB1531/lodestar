---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0092 — Romanian keeps the cedilla alphabet the description writes, and reads its regions off the word

**Status:** accepted · **Date:** 2026-09-09

## Context

Romanian is the only Romance language of [#176](https://github.com/CyrilB1531/lodestar/issues/176)'s
nine, so it is the one that reaches `RomanceSnowballWorker` rather than only
`SnowballWorkerBase`. Two things had to be settled before it could be written, and
neither is visible from the algorithm alone.

### 1. Two Unicode spellings of the same two letters

Romanian writes `ș` and `ț` with a comma below (U+0219, U+021B). Before Unicode 3.0
had those code points, the same letters were typed with a cedilla — `ş` U+015F and
`ţ` U+0163 — and both spellings are still in circulation.

**The published Snowball description writes the cedilla, and so does `nltk`.** Its
four suffix tables carry `ş`/`ţ` and nothing else, which the tables themselves say:

```text
step 1  aţiune  iţiune          step 2  işti  ităţi
step 3  aţi  eţi  iţi  âţi  seserăţi  eşti  eşte  aşi  uşi  işi  âşi …
```

A word typed the modern way therefore matches none of them, and is stemmed only by
the endings that happen to have no `ș`/`ț` in them:

| word | cedilla | comma below |
| --- | --- | --- |
| `informaţie` / `informație` | `inform` | `informaţ` → `informaț` |
| `româneşte` / `românește` | `român` | `româneșt` |
| `ştiinţă` / `știință` | `ştiinţ` | `științ` |
| `mulţumesc` / `mulțumesc` | `mulţum` | `mulțum` (agree: `esc` has neither letter) |

### 2. `nltk` measures its regions against the word it started with

`nltk` keeps R1, R2 and RV as *strings*, sliced from the word before step 0, and
updates them by hand — but only in some branches. Step 1's `at` and `it`
replacements re-slice R2; its `abil`, `ibil`, `iv` and `ic` replacements do not, and
step 0's `aţie` and `aua` branches never touch RV. Step 3 then asks where a suffix
sits with `rv.index(suffix)`, which finds its **first** occurrence rather than the
one that ends the word.

Swept over 30 252 words — 47 stems crossed with every ending of every table, and
with a second ending appended — the two readings part on **35**, in exactly two
shapes:

| shape | example | `nltk` | this |
| --- | --- | --- | --- |
| two step 1 replacements chained, so step 2 measures R2 against a word that no longer exists | `dormativitate` | `dorm` | `dormat` |
| a step 3 ending that occurs twice in RV | `posândând` | `posândând` | `posând` |

Both need an ending repeated or a derivational chain Romanian does not build. Over
the 349 real words of `tests/oracles/snowball_ro.json` the two readings agree
everywhere, and so do the 27 real `-itate` nouns probed separately
(`activitate`, `responsabilitate`, `electricitate`, …), which are the shape the
first row would have to come from.

## Decision

**Keep the cedilla alphabet, and measure the regions against the word as it
stands.**

`RomanianSnowballStemmer` folds nothing: `ş` and `ș` are two letters, as they are to
the description and to `nltk`, and the corpus freezes both spellings so the
behaviour is visible rather than implied. A caller whose text is modern Romanian
normalises it before calling — one `Replace` per letter — and that is a decision
about their text, not one this library can make for them without changing what
`Stem` returns for everyone else.

On the regions, this is [`0086`](0086-russian-follows-nltks-table-and-the-descriptions-alphabet.md)'s
line again, in a second language: [`0008`](0008-italian-enza-nltk-divergence.md)'s
parity is about the reference's **rules**, and a region that has gone stale in one
branch of one step is its **bookkeeping**. Following it would mean carrying R1, R2
and RV as mutable strings updated branch by branch, which is a port of `nltk`'s
implementation rather than of the published algorithm — what
[`0003`](0003-provenance-and-licensing.md) and this lot's brief both refuse — and
`RomanceSnowballWorker`, which Spanish, Portuguese and Italian already share, does
not work that way.

## Consequences

- The library is exact against `nltk` on every Romanian word this corpus and the
  probes could reach. The two divergent shapes are named above and pinned by
  `RomanianSnowballStemmerTests`, since the corpus is compared exactly and cannot
  carry them.
- **A user with modern Romanian text gets a stemmer that mostly does not fire.**
  That is the reference's behaviour and the description's, but it is the one thing
  in this lot most likely to be read as a bug, so it is said plainly on
  [the reference page](../reference/text/stemming/romaniansnowballstemmer.md)
  rather than left to the corpus.
- If a later lot wants the two spellings to collide, it is a new decision with a
  new public shape — an option, or a documented normaliser — and not a quiet fold
  inside `Stem`.
