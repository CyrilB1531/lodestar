# 0091 — Hungarian takes `snowballstemmer` as its oracle, not `nltk`

**Status:** accepted · **Date:** 2026-09-09

## Context

Every Snowball language in this package replays a corpus frozen from
`nltk.stem.snowball.SnowballStemmer`. Hungarian is the first that cannot: `nltk`
3.10.1's Hungarian is missing part of the published algorithm, and the gaps are
not edge cases.

**Its vowel set omits `ő` and `ű`.** Twelve of Hungarian's fourteen vowels are
there; the two double-acute long vowels are not. R1 is defined from the first
vowel, so a word whose first vowel is one of those two gets a region past the end
of itself and nothing is ever stripped:

| word | published algorithm | `nltk` 3.10.1 |
| --- | --- | --- |
| `kőben` | `kő` | `kőben` |
| `kűben` | `kű` | `kűben` |
| `kaben` | `ka` | `ka` |

**Its step 2 omits `től`, `ről` and `ből`.** Probing all 44 suffixes of that step
against one stem, three are never removed — the front-vowel halves of `tól`/`től`,
`ról`/`ről` and `ból`/`ből`, whose back-vowel partners all work.

The two together are one class of omission, and they reach ordinary words. A
stemmer's whole job is to make inflected forms collide, and under `nltk` these
do not:

| words | published algorithm | `nltk` 3.10.1 |
| --- | --- | --- |
| `nő` / `nők` | `nő` / `nő` | `nő` / `nők` |
| `szőlő` / `szőlők` | `szőlő` / `szőlő` | `szőlő` / `szőlők` |
| `gyűrű` / `gyűrűk` | `gyűrű` / `gyűrű` | `gyűrű` / `gyűrűk` |
| `kertből` | `kert` | `kertből` |

## Decision

Hungarian's corpus is frozen from **`snowballstemmer` 3.1.1**, the Snowball
project's own Python package, generated from the same source the published
description documents. Every other language keeps `nltk`.

`docs/equivalence.md`'s Hungarian row names that library rather than `nltk`, and
`tests/oracles/snowball_hu.json`'s metadata says `snowballstemmer`, so which
reference a corpus froze is readable from the corpus.

## Options

**Follow `nltk` and document the divergence**, as [`0008`](0008-italian-enza-nltk-divergence.md)
did for Italian `enza` and [`0087`](0087-danish-follows-nltk-on-the-apostrophe.md)
for Danish's apostrophe. Rejected, and the difference from those two is size.
Both were one rule reaching a handful of words, and parity with the library a
user migrates from was worth more than the prose. Here the omission covers a
letter class: `ő` and `ű` are common — `idő`, `mező`, `első`, `tető`, `felhő`,
`tűz`, `fű`, `gyűrű` — and freezing it would ship a Hungarian stemmer that leaves
those words unstemmed and call the corpus proof of it.

**Implement `nltk`'s behaviour but exclude the affected words from the corpus.**
Rejected: the corpus would then pin the part that agrees and stay silent on the
part that does not, which is the opposite of what a frozen oracle is for.

**Take `snowballstemmer`.** Chosen. It is the reference implementation rather
than a transcription of it, BSD-3-Clause, and carries no dependencies of its own,
so the resolved graph grows by one leaf. [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)
already set the precedent that a second oracle library is preferable to a corpus
that freezes the wrong answer, and [`0003`](0003-provenance-and-licensing.md)'s
licence rule is satisfied.

## Consequences

- `tools/requirements.txt` gains `snowballstemmer==3.1.1`, and the lock is
  regenerated. No other pin moved.
- `HungarianSnowballStemmer` is written from the published description like its
  siblings, and agrees with `snowballstemmer` on all 576 words the implementation
  was developed against before the corpus was cut.
- The eight `nltk` corpora are untouched. Nothing about the other languages is
  reopened by this: where `nltk` implements the published algorithm, it stays the
  oracle, divergences included.
- A future language may need the same call. It is made per language, on measured
  evidence, and not once for the package.
