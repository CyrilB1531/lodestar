# 0827 — `TopKAccuracy.Score` by counting the true class's rank

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#827](https://github.com/CyrilB1531/lodestar/issues/827), found by a performance review of `main`.

## Change

`TopKAccuracy.Score` counts the true class's rank in each row instead of sorting the row, and allocates nothing per row.

## Measured

| classes (200,000 rows, k = 2) | `main` | fix |
| ---: | ---: | ---: |
| 10 | 23.8 ms, 32 MB | 8.07 ms, 0 B |
| 100 | 471 ms, 238 MB | **62.3 ms, 0 B** |

A tie is ordered by descending index, so a class's position is the count of higher scores plus equal scores at a higher index; a row holding a NaN keeps the sort. Bit-identical against `main` on 318 scores with ties, NaNs, signed zeros and weights. `TopKAccuracyBenchmarks`, pinned to four cores.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
