---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0006 — The stemmers' references

**Status:** accepted · **Date:** 2026-09-20

## Context

`Lodestar.Text.Stemming` ships sixteen stemmers: the Porter algorithm of 1980 and
fifteen Snowball languages. Each is an original implementation of a published
algorithm — never a transcription of a reference implementation, which `0003`
forbids — and each is proven by a corpus in `tests/oracles/` frozen from a Python
library and compared exactly. Which library, and what happens when that library and
the published description disagree, is one question asked sixteen times, and eight
records answered it one language at a time.

The published description and the reference implementation part on four kinds of
difference, and the eight records resolve them in three different directions.

**The reference's rule tables.** Italian's step 1 replaces `enza`/`enze` with `te`
where the prose gives `ente`, so `esistenza` reaches `esistt` (`0008`). Danish's
published R1 has an apostrophe clause that `nltk`'s shared Scandinavian region
routine has not, so `pc'er` comes back whole (`0087`). Finnish's step 3 ends on a
failed condition everywhere except `siin`, `den`, `tten` and `seen` — the four long
spellings of the genitive — where the `n` goes anyway (`0090`). Russian's adjectival
table misspells one pair in 234, so `рискующая` reaches `рискующ` (`0086`). All four
follow the reference: parity with the library a user migrates from is the contract,
and a rule table is what parity is about.

**The reference's representation, leaking through its output.** `nltk`'s Russian runs
on a Roman transliteration in which `ь` is one apostrophe and `ъ` is two, so step 4
halves a final `ъ` and a cut inside `ц` emits `отт^`, which is not a Cyrillic string
(`0086`). `nltk`'s Romanian keeps R1, R2 and RV as strings sliced before step 0 and
refreshed in only some branches, which parts from a region read off the current word
on 35 of 30 252 swept forms and on no Romanian word (`0092`). Neither is followed:
matching them means porting the reference implementation rather than the algorithm,
which is the line `0003` draws.

**The reference is missing part of the algorithm.** `nltk`'s Hungarian omits `ő` and
`ű` from its vowel set and `től`, `ről`, `ből` from step 2, so `nő`/`nők` and
`gyűrű`/`gyűrűk` never collide (`0091`). `nltk`'s French implements an older
revision — no elision, no `ë`/`ï`, no `-oux`, `-aise` or `ni-` — and differs from the
current algorithm on 278 words of a 346 244-word dictionary (`0145`). An omission that
covers a letter class or a rule set is not a divergence worth freezing.

**The reference is not a function.** `nltk`'s Arabic stemmer keeps `is_verb`,
`is_noun`, `is_defined` and a dozen success flags on the instance, so `كتبوا` stems
to `كتب` on a fresh instance and `كتبو` after other words. A corpus cut from a shared
instance would freeze the word list's order, and no thread-safe implementation could
reproduce it (`0094`).

## Decision

**A stemmer is oracled by `nltk`, whose rule tables win over the published
description where the two part but whose internal representation does not; the oracle
moves to `snowballstemmer` 3.1.1 only where `nltk` is not an implementation of the
current algorithm — a letter class missing, a superseded revision, or a stemmer that
is not a pure function — and that move is decided per language on measured evidence,
never once for the package.**

Which reference a corpus froze is readable from the corpus: every
`tests/oracles/snowball_*.json` carries the library and its version in its metadata,
and `docs/equivalence.md` names the same call in the language's row.

| language | reference, as the corpus names it | what the choice settles | from |
| --- | --- | --- | --- |
| Arabic | `snowballstemmer` 3.1.1 | `nltk`'s Arabic is stateful across calls, carries rules the description has not and omits two it states; the same record puts `ArabicSnowballStemmer` outside `SnowballWorkerBase`, the algorithm using neither R1 nor R2 | `0094` |
| Danish | `nltk` 3.10.3 | R1 is set the standard way and floored at three, with no apostrophe clause; the six apostrophe words are in the corpus so the divergence is pinned | `0087` |
| Dutch | `nltk` 3.10.3 | no recorded divergence — the default | — |
| English (Porter2) | `nltk` 3.10.3 | no recorded divergence — the default | — |
| English (Porter 1980) | `nltk` 3.10.3, `mode=ORIGINAL_ALGORITHM` | no recorded divergence — the default, and the one non-Snowball stemmer | — |
| Finnish | `nltk` 3.10.3 | a failed condition ends step 3's search, except on the four genitive spellings; constructed forms pin the boundary from both sides | `0090` |
| French | `snowballstemmer` 3.1.1 | `nltk` is an older revision with region artefacts; the class is named for Snowball, so it implements Snowball | `0145` |
| German | `nltk` 3.10.3 | no recorded divergence — the default | — |
| Hungarian | `snowballstemmer` 3.1.1 | `nltk` omits `ő`, `ű` and three step 2 suffixes, which is a letter class rather than a rule | `0091` |
| Italian | `nltk` 3.10.3 | `enza`/`enze` become `te`, not `ente` — the record that set the rule the others lean on | `0008` |
| Norwegian | `nltk` 3.10.3 | no recorded divergence — Bokmål, `nltk`'s `norwegian`; Nynorsk is out of scope | — |
| Portuguese | `nltk` 3.10.3 | no recorded divergence — the default | — |
| Romanian | `nltk` 3.10.3 | the suffix tables carry the cedilla `ş`/`ţ` the description writes, and the regions are read off the word rather than off `nltk`'s stale copies | `0092` |
| Russian | `nltk` 3.10.3 | the `ующая` hole in the ending table is reproduced; step 4 runs on Cyrillic letters, so `подъём` keeps its `ъ`; `ё` folds to `е` and the scan is over UTF-16 units | `0086` |
| Spanish | `nltk` 3.10.3 | no recorded divergence — the default | — |
| Swedish | `nltk` 3.10.3 | no recorded divergence — the default | — |

Thirteen corpora are frozen from `nltk`, three from `snowballstemmer`. The three
exceptions are all languages added late; no corpus that began on `nltk` and agreed
with it has been moved.

## Consequences

- `tools/requirements.txt` pins both references, `nltk==3.10.3` and
  `snowballstemmer==3.1.1`. The second is a leaf with no dependencies of its own, so
  three languages cost the resolved graph one package.
- Four stems look wrong read as their own language and are right as parity:
  `esistenza` → `esistt`, `pc'er` → `pc'er`, `ihmisiin` → `ihmis`, `рискующая` →
  `рискующ`. So does a Romanian word typed with the modern `ș`/`ț`, which matches only
  the endings without those letters and is mostly left whole; that one is said on the
  reference page rather than left to the corpus.
- A divergence a corpus cannot carry is pinned by a test instead, because the corpora
  are compared exactly: Russian's `подъём`/`объём` and Romanian's two swept shapes are
  in their stemmer test classes, not in `snowball_ru.json` or `snowball_ro.json`.
- If a reference ever closes one of these gaps, the corpus drifts and the
  `Oracles are reproducible` job reports it. The decision is then revisited — the
  named condition deleted — rather than the corpus quietly regenerated.
- The eight records disagree on one number. `0086`, `0087`, `0090`, `0091` and `0092`
  measured against `nltk` 3.10.1; `0145` and every corpus metadata block name
  `nltk` 3.10.3, which is what `tools/requirements.txt` pins. The pin moved after the
  first five were written and no oracle changed, so the measurements stand; 3.10.3 is
  the version in force.
