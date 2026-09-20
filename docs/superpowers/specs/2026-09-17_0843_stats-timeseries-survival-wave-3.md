# 0843 — Time-series fits on one QR, and rank, Durbin and Cox buffers

**Status:** **retrospective** — written 2026-09-17, after the measurement it records.

Issue: [#843](https://github.com/CyrilB1531/lodestar/issues/843), found by a performance review of `main`.

## Problem

- The augmented Dickey-Fuller lag search ran one Householder QR per candidate lag, 27 of them at 2,000 points.
- `VectorAutoregression.Fit` ran one QR per equation on a design every equation shares.
- The autocovariance centred each value twice per product.
- `KruskalWallis.Test` past sixteen groups and `Wilcoxon`'s sorting path ranked a sample, then sorted it again for the tie term.
- Durbin's matrix power allocated a jagged matrix for every product.
- The Cox fit allocated its linear predictor and moment sums on every Newton iteration, and the concordance searched an event's level twice.

## Change

- One QR of the widest lag design serves every candidate lag, and one QR and one inverse of R serve every VAR equation. `SharedReflections`, internal to `Lodestar.Stats.TimeSeries`, keeps `OrdinaryLeastSquares.Estimate`'s operation order; the reflection and its four-lane inner product moved to shared source (`src/Shared/Reflections.cs`) compiled into both packages, since that kernel is internal to `Lodestar.Stats.Regression`.
- The autocovariance centres the series once into a buffer.
- `Ranks.AverageWithTies` returns the tie correction and the has-ties flag from the ranking's own tie groups.
- Durbin's matrix power ping-pongs two flat buffers and skips the square nobody reads.
- The Efron likelihood keeps its predictor, moment sums and adjusted first moments as fields; the concordance precomputes each subject's level once and deduplicates in place.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| ADF lag search, 2,000 points | 7.98 ms, 13.1 MB | **941 µs, 958 KB** |
| VAR, 5,000 × 5 variables × 4 lags | 4.62 ms, 5.5 MB | **2.05 ms, 2.2 MB** |
| autocorrelation, 2,000 points | 80.1 µs, 1,000 B | **32.8 µs**, 16.6 KB |
| Kruskal-Wallis, 32 groups, 100,000 values | 11.7 ms, 3.6 MB | 6.59 ms, 2.8 MB |
| Wilcoxon, 300,000 differences | 44.0 ms, 18.0 MB | **24.0 ms, 13.2 MB** |
| Kolmogorov-Smirnov asymptotic, 280 values | 81.8 µs, 102 KB | **55.6 µs, 24.7 KB** |
| Cox, 10,000 × 2 | 3.88 ms, 1.2 MB | **3.25 ms, 680 KB** |

Bit-identical against `main`, asserted per lag, per equation and on the pooled Kruskal-Wallis route. The full table is in [`performance.md`](../../guides/performance.md).

## Rejected

- **Mirroring the Shapiro-Wilk Blom scores.** `1 − p[n−1−i]` equals `p[i]` in only about a third of the pairs, so the weights would move.
- **Accumulating only the upper triangle of the Cox second moments.** `w·x_a·x_b` against `w·x_b·x_a` moves the last bits, so the fit would no longer be bit-identical.
- **A SIMD autocovariance kernel.** It reorders the additions, so the lags would move by about 1e-15.
- **Reusing the likelihood's sort in the concordance.** Not carried in this batch: the concordance's own time-group order was kept, so its integer tallies stay what they were with no argument about how the two orders treat tied durations.
- **An internal member in `Lodestar.Stats.Regression` for the shared QR.** It would take a published floor bump for an internal kernel; the operation order is replicated instead, and held to the estimate by tests.
