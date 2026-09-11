---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0090 — Finnish falls back to the genitive where nltk does, and nowhere else

**Status:** accepted · **Date:** 2026-09-09

## Context

The published Finnish description gives step 3 as one search:

> Search for the longest among the following suffixes in _R_1, and perform the
> action indicated.

Snowball's reading of that is settled: the longest alternative is selected, its
action runs, and if the action's condition fails the search is over. Nothing
shorter is tried. Two constructed forms confirm `nltk` reads it that way too:

| word | ending selected | condition | `nltk` 3.10.1 |
| --- | --- | --- | --- |
| `kotimaahon` | `hon` | fails — `a` before it, not `o` | `kotimaahon`, whole |
| `kissatta` | `tta` | fails — `a` before it, not `e` | `kissat`, and not `kis` |

`kissatta` is the sharper of the two: `ta` is in the same list and would have
matched, and it is not used.

Four endings do not behave that way. When their condition fails, the final `n`
is removed anyway — the outcome the step's own shortest alternative would have
produced:

| word | ending selected | condition | `nltk` 3.10.1 | under the description |
| --- | --- | --- | --- | --- |
| `ihmisiin` | `siin` | fails — `mi` before it, not _V_`i` | `ihmis` | `ihmisiin`, whole |
| `ihmisden` | `den` | fails | `ihmisd` | `ihmisden`, whole |
| `ihmistten` | `tten` | fails | `ihmist` | `ihmistten`, whole |
| `ihmiseseen` | `seen` | fails — no long vowel before it | `ihmises` | `ihmiseseen`, whole |

`siin`, `den`, `tten` and `seen` are exactly the four endings in the step that
are a longer spelling of the genitive `n`. `hön` and the rest of the `hXn` group
end in `n` as well and do **not** fall back, so the shared final letter is not
the rule; being a spelling of the genitive is.

## Decision

**Match `nltk`.** `FinnishSnowballStemmer` ends the search on a failed condition,
except for `siin`, `den`, `tten` and `seen`, where it strips the genitive `n` —
including that rule's own "delete the last vowel if a long vowel or `ie` is
uncovered".

This is the same call as [`0008`](0008-italian-enza-nltk-divergence.md) on
Italian `enza`/`enze`, [`0087`](0087-danish-follows-nltk-on-the-apostrophe.md) on
Danish's apostrophe and [`0005`](0005-hamming-jellyfish-divergence.md) on
`jellyfish`. The contract this package offers is behavioural parity with the
Python library a user is migrating from, and for the stemmers that library is
`nltk` — it is what the oracle corpora are frozen from and what
`docs/equivalence.md` names as the reference.

## Consequences

- `ihmisiin` stems to `ihmis` rather than coming back whole. It is the **one real
  Finnish word** in the 217-word corpus that meets this rule, and it was the only
  failure of the first implementation, which had been written from the
  description and got the other 211 right.
- One real word is thin evidence for a rule, so the four constructed forms above
  are in `tests/oracles/snowball_fi.json` beside it, together with `kotimaahon`
  and `kissatta` for the side that does _not_ fall back. The boundary is pinned
  from both directions rather than inferred from a single case.
- Where the two readings agree — every other word in the corpus — this decision
  changes nothing, which is why it took a constructed vocabulary to find at all.
- If `nltk` ever makes the step uniform, the corpus will drift and the
  `Oracles are reproducible` job will catch it. At that point this decision
  should be revisited rather than the corpus quietly regenerated.
