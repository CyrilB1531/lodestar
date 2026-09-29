# Lodestar.Metrics' citations of renumbered decision records

**Issues:** [#1396](https://github.com/CyrilB1531/lodestar/issues/1396).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

`src/Lodestar.Metrics` cited `docs/decisions/0018`, `0021`, `0024` to `0033` twenty-seven times, and `docs/equivalence.md`'s Metrics rows `0020` and `0021` three times.
Issue #1103 restarted the numbering at 0001 and deleted the records that stated a mechanism, so today
those numbers name nothing, and a reader following one lands on no file or on a different record.

## Decisions

- **Each citation reads `decision NNNN at 53af23c2`**, the tree CLAUDE.md names for reading an older
  citation, rather than a guess at where each mechanism's substance moved: the reasoning they point
  to is the record's own, and it still reads there whole.

## Verification

- No `docs/decisions/00NN` above 0009 is cited under `src/Lodestar.Metrics` or in
  `docs/equivalence.md`; the build carries the comments unchanged otherwise.
- Review A found one citation without the `docs/` prefix (`Outputs.cs`), and an equivalence row
  crediting today's 0003 with a ruling that was 0016 at `53af23c2`; both corrected. The
  `Lodestar.Extensions.AI` changelog's 0089 is released history and stays; the two in
  `Lodestar.Embeddings` are #1397's.
