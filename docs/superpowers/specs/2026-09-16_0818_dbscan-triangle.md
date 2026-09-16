# 0818 — DBSCAN neighbourhoods from the upper triangle

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#818](https://github.com/CyrilB1531/lodestar/issues/818), found by a performance review of `main`.

## Change

`Dbscan.Fit` computes each pair's distance once and stops a sum past the radius.

## Measured

| samples × features | `main` | fix |
| --- | ---: | ---: |
| 5,000 × 2 | 63.8 ms | 36.8 ms |
| 20,000 × 2 | 893 ms | 667 ms |
| 10,000 × 8 | 408 ms | 251 ms |
| 5,000 × 16 | 188 ms | **86 ms** |

The partial sums never fall, so every decision is the full sum's; labels and core samples identical on five datasets. `DbscanIncumbentBenchmarks` and `DbscanDimensionBenchmarks`, this package's rows.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
