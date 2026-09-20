# 0845 — Dense kernels in decomposition and regression, walked in memory order

**Status:** **retrospective** — written 2026-09-17, after the measurement it records.

Issue: [#845](https://github.com/CyrilB1531/lodestar/issues/845), found by the performance review of `main` (2026-09-16).

## Problem

The dense kernels under `TruncatedSvd`, `Nmf`, `QrDecomposition.Householder` and `CsrMatrix`'s block products walked
a column of a row-major block, striding a whole row per element, and the regression fits copied or recomputed what an
iteration had already built: three copies of the weighted design per IRLS step, forty logarithms per negative binomial
row, every exponential twice and every Hessian block twice in the multinomial Newton step.

## Change

- `Lodestar.Abstractions`: `CsrMatrix.Multiply` and `CsrMatrix.TransposeMultiply` add each non-zero's scaled row over
  spans, with `Vector256` lanes on net10.0.
- `Lodestar.Decomposition`: the Jacobi SVD sweeps a column-major copy with vectorised rotations, and the randomized
  SVD hands its small block over without transposing it twice; the NMF updates hold H column-major, mirror the Gram
  triangle and reuse their buffers; the Householder reflections are applied row by row, from column k on;
  `TruncatedSvd` projects through transposed components.
- `Lodestar.Stats.Regression`: the least-squares inner product keeps its four running sums as four `Vector256` lanes
  and the reflection update is vectorised; IRLS writes its weighted system column-major into reused buffers and solves
  it in place; the negative binomial log-likelihood memoises lnΓ(y + θ) for counts below 256; the multinomial Newton
  step takes each exponential once, mirrors the Hessian's symmetric blocks and folds the score into the same pass;
  GLS reads L⁻¹·1 off its whitened design.

Each change reorders memory access or reuses a computed value, never the arithmetic, so results are bit-identical
against `main`; `DenseKernelPathTests`, `LeastSquaresTests` and `LogLikelihoodTests` compare the bits of both walks.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| `CsrMatrix.Multiply`, 5,000 documents × 64 (`TiledSparseDenseProductBenchmarks`) | 8.26 ms | 3.32 ms |
| `CsrMatrix.Multiply`, 50,000 documents × 256 | 408 ms | 240 ms |
| `QrDecomposition.Householder`, 20,000 × 30 (`HouseholderQrBenchmarks`) | 61.1 ms | **11.4 ms** |
| `TruncatedSvd.Fit`, rank 20 (`DecompositionBenchmarks`) | 17.4 ms | 12.1 ms |
| `TruncatedSvd.Fit`, rank 20, 20,000 columns (`DecompositionWidthBenchmarks`) | 251 ms, 114 MB | 142 ms, 103 MB |
| `TruncatedSvd.Transform`, 500 columns | 1.09 ms, 313 KB | 306 µs, 391 KB |
| `TruncatedSvd.Transform`, 20,000 columns | 1.44 ms, 313 KB | 1.13 ms, 3.36 MB |
| `PrincipalComponentVariance.Compute`, 100 × 200 | 8.06 ms | 5.95 ms |
| `Nmf.Fit`, rank 20 (`DecompositionBenchmarks`) | 121 ms | 67.3 ms |
| `Nmf.Fit`, Frobenius, 20,000 columns | 1.1 s, 493 MB | **521 ms, 153 MB** |
| `Nmf.Fit`, Kullback-Leibler, 20,000 columns | 753 ms, 554 MB | 417 ms, 154 MB |
| `GeneralizedLeastSquares.Fit`, n = 1,000 (`GlsBenchmarks`) | 50.4 ms | 31.4 ms |
| `GeneralizedLinearModel.Fit`, Poisson, mean 5 (`GlmPoissonBenchmarks`) | 197 µs, 423 KB | 162 µs, 111 KB |
| `GeneralizedLinearModel.Fit`, Poisson, 20,000 rows (`GlmOffsetBenchmarks`) | 3 ms | 2.22 ms |
| `GeneralizedLinearModel.Fit`, negative binomial, 100,000 rows (`GlmNegativeBinomialBenchmarks`) | 35.5 ms, 28.2 MB | **17.7 ms, 5.35 MB** |
| `MultinomialLogit.Fit`, 2,000 rows (`MultinomialLogitBenchmarks`) | 861 µs | 752 µs |

The sparse products, the Householder reflections, the Jacobi rotations and the NMF updates walk a column as one contiguous span, with `Vector256` lanes that compute each element on its own; the least-squares inner product keeps its four running sums as four lanes; negative binomial fits memoise lnΓ(y + θ) per small count; IRLS solves its weighted system in place; the multinomial Hessian mirrors its symmetric blocks; GLS reads L⁻¹·1 off its whitened design. Every cell sums the same products in the same order, so results are bit-identical against `main`, held by tests that compare the bits of the old and new walks. `TruncatedSvd.Transform` now allocates the transposed components, which at 20,000 columns costs 3 MB for its 1.28×. The `Lodestar.Abstractions` and `Lodestar.Decomposition` rows were measured with `LodestarUseProjectRefs=true` on both sides, because `Lodestar.Decomposition` reaches `Lodestar.Abstractions` through a published floor. Pinned to four cores.

A/B/A on an AMD Ryzen 7 8700G, pinned to four cores, 2026-09-17; both `main` runs agreed within 5%. The
`Lodestar.Abstractions` and `Lodestar.Decomposition` rows were measured with `LodestarUseProjectRefs=true` on both
sides, because `Lodestar.Decomposition` reaches `Lodestar.Abstractions` through a published floor. `TruncatedSvd.Transform`
trades 3 MB of transposed components at 20,000 columns for its 1.28×.

## Rejected

- **Mirroring the heteroskedastic meat.** `(w·x_a)·x_b` is not `(w·x_b)·x_a` to the bit, so results would move by
  about 1e-16 relative.
- **Whitening the weighted OLS design in place.** Every `RobustCovarianceBenchmarks` and `WeightedLeastSquaresBenchmarks`
  row stayed within 3%, with no allocation drop.
- **`LeastSquares.InvertUpper` over an array, and the Wald inverse copied out of its QR.** Also within 3% on every
  row; ADR 0125's reopening condition, a runtime without PGO, was not measured.
- **Deriving GLS's centred response from the whitened response.** It is linear algebra rather than the same
  substitution, and moves rounding at about 1e-15.
