# 0087 — Danish's apostrophe rule: follow nltk, not the published description

**Status:** accepted · **Date:** 2026-09-09

## Context

Danish is the one Snowball language whose published description gives R1 two
definitions. Every other language sets it the standard way; Danish sets it in
three steps, because the apostrophe separates a Danish suffix from a word of
foreign origin:

> 1. If the word contains an apostrophe, R1 is set to start after the first
>    apostrophe.
> 2. Otherwise, R1 is set in the standard way.
> 3. In either case, R1 is then adjusted so that the region before it contains
>    at least 3 characters.

`nltk` implements step 2 and step 3 and not step 1: its Scandinavian stemmers
share one region routine, and that routine has no apostrophe in it.

The two readings part on the words the rule exists for. `pc'er` has its first
vowel at index 3, so the standard R1 lands at 5 — past the end of a five-letter
word — and no suffix can lie in it. Under the published rule R1 starts after the
apostrophe, at 3, so `er` is inside it:

| word | published description | `nltk` 3.10.1 |
| --- | --- | --- |
| `pc'er` | `pc'` | `pc'er` |
| `cd'er` | `cd'` | `cd'er` |
| `tv'et` | `tv'` | `tv'et` |

The rule is invisible on longer words. `computer'en` and `bil'er` reach `er`/`en`
through the standard R1 as well, and stem to `computer'` and `bil'` either way,
which is why a corpus of ordinary Danish never meets this.

## Decision

**Match `nltk`.** `DanishSnowballStemmer` sets R1 the standard way and floors it
at three characters, with no apostrophe case.

This is the same call as [`0008`](0008-italian-enza-nltk-divergence.md), where
the published Italian description and `nltk` part on `enza`/`enze`, and as
[`0005`](0005-hamming-jellyfish-divergence.md), where `jellyfish` diverges from
the textbook definition. The contract this package offers is behavioural parity
with the Python library a user is migrating from, and for the stemmers that
library is `nltk` — it is what the oracle corpora are frozen from and what
`docs/equivalence.md` names as the reference.

## Consequences

- `pc'er`, `cd'er` and `tv'et` come back whole, which the published description
  says they should not. They are correct as *parity*, and that is the guarantee
  this library offers.
- The six apostrophe words are in `tests/oracles/snowball_da.json` rather than
  left out of it. A divergence nothing exercises is a divergence that gets
  quietly re-introduced by the next reader of the description; these pin it, and
  the `Oracles are reproducible` job is what would report `nltk` adopting the
  rule.
- Found before the implementation rather than by a failing case, which is why
  this decision reads as a choice and 0008 reads as a diagnosis: the published
  description states the apostrophe rule in the same paragraph as R1, so the
  question was unavoidable rather than latent.
