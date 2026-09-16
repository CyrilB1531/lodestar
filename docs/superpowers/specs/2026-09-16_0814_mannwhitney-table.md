# 0814 — The exact Mann-Whitney table, sized by the smaller sample

**Status:** accepted, 2026-09-16. Written after the measurement it records.

Issue: [#814](https://github.com/CyrilB1531/lodestar/issues/814), found by a performance review of `main`.

## Change

The exact Mann-Whitney distribution sizes its table by the smaller sample and reuses two buffers, where 8 against 2,500 allocated 3.35 GB.

## Measured

| sizes (`ExactMethod.Exact`) | `main` | fix |
| --- | ---: | ---: |
| 141 × 141 | 906 ms, 2.99 GB | 410 ms, 43 MB |
| 8 × 2,500 | 750 ms, 3.35 GB | **285 ms, 2.9 MB** |

U's null distribution is symmetric in the two sizes. Counts past 2^53 round in a different order: of 24 lopsided cases 19 p-values are bit-identical and the rest within 2.9e-16 relative. `MannWhitneyExactBenchmarks`.

## Rejected

- **A change that moves results.** Each fix keeps the corpus replaying at its tolerance, and was compared against `main` on inputs the corpus does not hold.
