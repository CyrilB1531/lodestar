# 0816 — TextRank's unreachable words dropped in one compaction

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#816](https://github.com/CyrilB1531/lodestar/issues/816), found by a performance review of `main`.

## Change

`TextRank.Extract` drops every isolated word in one compaction, where it rebuilt the word matrix once per word.

## Measured

| words | `main` | fix |
| ---: | ---: | ---: |
| 2,000 | 25.9 ms, 99 MB | 9.6 ms, 3.1 MB |
| 8,000 | 3.14 s, 5.26 GB | **364 ms, 35 MB** |

Kept cells and their order are unchanged: keywords and scores bit-identical. `TextRankBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
