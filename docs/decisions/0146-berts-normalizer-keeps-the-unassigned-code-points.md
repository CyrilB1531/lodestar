---
status: accepted
supersedes: []
amends: ["0144"]
applies: []
---
# 0146 — BERT's normalizer keeps the unassigned code points

**Status:** accepted · **Date:** 2026-09-18 · **Amends:** [0144](0144-a-vocab-txt-runs-berts-basic-tokenizer.md)

## Context

[Decision 0144](0144-a-vocab-txt-runs-berts-basic-tokenizer.md) gave `VocabTxtLoader` BERT's basic
tokenization and wrote the pipeline out: "NUL, U+FFFD and the Cc, Cf, Cs, Co and **Cn** characters
but tab, newline and return dropped". The list was read off `tokenizers`' own `is_control`, whose
comment names Cn among the categories it covers.

The comment is not the behaviour. Measured through `BertNormalizer(clean_text=True, …)` of
`tokenizers` 0.23.2, one code point at a time between two letters: `U+0001` (Cc), `U+00AD` and
`U+200B` (Cf) and `U+E000` (Co) are dropped, and `U+0378`, `U+FFFE` and `U+1FAE9` — all three
unassigned — are **kept**. `is_control` tests whether a code point is a control in the narrow
sense, not whether its category begins with C.

The difference is invisible on a Unicode version both sides agree about and grows with every new
one: a code point .NET's tables do not know yet is Cn to them and assigned to the model's. This
package dropped them, so a text carrying a 2023-or-later emoji lost it silently where BERT reads
`[UNK]` ([#983](https://github.com/CyrilB1531/lodestar/issues/983)).

## Decision

**The normalizer keeps unassigned code points.** `IsControl` drops Cc, Cf, Cs and Co but tab,
newline and return, and Cn is not among them — one clause of 0144's Decision, and nothing else in
it, is replaced. Everything 0144 decided stands: `VocabTxtLoader` sets `BasicTokenization`,
`TokenizerJsonLoader` leaves it off, and `vocab_txt.json` is the proof on a cased and an uncased
model.

**Cs stays on the list, and has no counterpart upstream.** A Rust `char` cannot hold a surrogate,
so `tokenizers` never meets one and says nothing about it; a .NET `string` can. Dropping a lone
surrogate is what keeps the text that reaches `string.Normalize` well-formed, and it is the reason
the decomposition can treat a refusal as being about an unassigned code point
([#1050](https://github.com/CyrilB1531/lodestar/issues/1050)).

## Options refused

**Keeping Cn and calling the difference a documented divergence.** It is a divergence that grows
with Unicode's own release cadence, on the side of losing text rather than marking it unknown, in
a pipeline whose whole claim is that a `vocab.txt` tokenizes as `BertTokenizer` does. The ~600
code points `docs/equivalence.md` records for this row are the residue of two libraries reading
two Unicode tables; this was not that — it was a category the algorithm never tested.

**Leaving 0144 to be read as written.** An ADR is immutable, so the record cannot correct itself,
and `tools/check_adr_immutable.py` checks that the body did not move rather than that it stayed
true. A reader implementing the pipeline from 0144 alone drops exactly the code points #983 kept —
the shape CLAUDE.md names for 0101 and 0103.

## Consequences

- `docs/decisions/index.yaml` carries `amended_by: ["0146"]` on 0144, so following the reverse
  edges reaches this record before the stale clause is acted on.
- Ids changed once more on the `vocab.txt` route, for text holding an unassigned code point only;
  `CHANGELOG.md` says so under *Fixed* for #983.
- The XML remark on `BertBasicTokenization` and the `docs/equivalence.md` row say "keeping
  unassigned ones" and are now the two places that agree with this record rather than with 0144.
