# 0817 — The sparse `RobustScaler` fit, grouped by column once

**Status:** **retrospective** — written 2026-09-16, after the measurement it records.

Issue: [#817](https://github.com/CyrilB1531/lodestar/issues/817), found by a performance review of `main`.

## Change

`RobustScaler.Fit(CsrMatrix)` groups the stored values by column in one pass, where it scanned every stored value for each column.

## Measured

| rows × columns × stored per row | `main` | fix |
| --- | ---: | ---: |
| 2,000 × 2,000 × 20 | 49.5 ms, 30.6 MB | **15.2 ms, 360 KB** |
| 500 × 20,000 × 5 | 52.5 ms, 76.9 MB | 27.1 ms, 336 KB |

Each column's sort receives the same sequence as before: bit-identical. The per-column sort is the remaining cost. `RobustScalerSparseBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
