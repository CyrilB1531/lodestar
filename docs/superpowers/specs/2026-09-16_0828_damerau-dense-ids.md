# 0828 — `DamerauLevenshtein.Distance` with dense symbol ids

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#828](https://github.com/CyrilB1531/lodestar/issues/828), found by a performance review of `main`.

## Change

`DamerauLevenshtein.Distance` reads its last-row table by dense symbol id instead of a dictionary lookup in every cell.

## Measured

| length | `main` | fix |
| ---: | ---: | ---: |
| 12 | 690 ns, 696 B | 566 ns, 840 B |
| 120 | 62.8 µs, 1,488 B | **37.0 µs**, 2,128 B |

Allocation rises by the id arrays. Distances identical against `main` on 6,000 pairs over chars, code points and integers. `DamerauLevenshteinBenchmarks`, pinned to four cores.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
