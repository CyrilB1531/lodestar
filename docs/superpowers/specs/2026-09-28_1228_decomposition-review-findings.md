# Lodestar.Decomposition review findings before 1.0

**Issues:** [#1228](https://github.com/CyrilB1531/lodestar/issues/1228),
[#1231](https://github.com/CyrilB1531/lodestar/issues/1231),
[#1255](https://github.com/CyrilB1531/lodestar/issues/1255),
[#1256](https://github.com/CyrilB1531/lodestar/issues/1256).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

Two reviews of `main` (at `d228321f` and `9b142ab2`) found four places where
`Lodestar.Decomposition`, and the two packages sharing its arithmetic, answered differently from
scikit-learn 1.9.1:

1. A sparse column's variance was `E[x²] − E[x]²`, which cancels on a column far from zero: a
   column at `1e8 + 1 … 1e8 + 4` read `2` where the variance is `1.25`, in `StandardScaler.Fit`
   over a `CsrMatrix` and in the total `TruncatedSvd`'s variance ratio divides by (#1228).
2. `TruncatedSvd.Fit` refused `n_components == n_features`, which the randomized path accepts, and
   refused a rank above the row count, which the reference fits with one component per row (#1231).
3. `JacobiSpectrum.Rotation` compared an off-diagonal entry with `√(αβ)`, which overflows once the
   squared norms multiply past `1.8e308`: `PrincipalComponentVariance.Compute` on a block scaled by
   `1e40` returned the column norms as its spectrum, and past `1e154` threw `ArithmeticException`
   out of `Math.Sign(NaN)` (#1255).
4. `randomized_svd(transpose="auto")` factors `Xᵀ` when a matrix has fewer rows than columns, and
   so does `_initialize_nmf`; this package always factored `X`, so no wide matrix could reproduce
   the reference, and the NMF rows did not say so (#1256).

## Decisions

- **One two-pass moment, shared.** `src/Shared/SparseMoments.cs` is `_csr_mean_variance_axis0`,
  unweighted: the mean, then the stored deviations with Chan, Golub and LeVeque's correction and
  the absent zeros' `(n − nnz)·mean²`. Both `StandardScaler` and `TruncatedSvd.TotalVariance` read
  it, compiled in by `LodestarIncludesSparseMoments`. A row storing a column twice is summed first,
  as #1044 settled for the other sparse fits; the reference reads each copy apart there and answers
  a negative variance, so that input is not frozen.
- **The rank bound is the reference's.** `1 ≤ k ≤ n_features`; the kept count is `min(k, rank)`,
  which is what the reference's `Vt[:k]` keeps on a matrix with fewer rows than `k`.
- **Transpose, not document.** The issue offered stating the divergence; the project aligns with
  the reference instead. When `rows < columns` the range finder runs on `Xᵀ` through
  `CsrMatrix.TransposeMultiply`, so nothing is materialised, Ω is `rows × (k + p)` drawn as
  `RandomState.normal(size=(rows, k + p))` draws it, and the factors swap back. `svd_flip` decides
  the signs on the same `U` of `X` in both branches, so no sign rule changes. This moves Ω's shape
  for a wide matrix, which is a change to `RandomMatrix`'s contract, stated in its remark and in
  the Abstractions changelog.
- **Two square roots, and a power of two when needed.** `Rotation` compares against `√α · √β`,
  which cannot overflow before the entries do. `PrincipalComponentVariance` forms the Gram as
  `main` did and rescales by `2^−e`, `e` the largest centred magnitude's exponent within ±1022, only
  when the Gram's diagonal leaves `[1e-140, 1e140]`, where the sweep's squares would leave the
  normal doubles: a power of two is exact, and an ordinary block takes the old path bit for bit. A
  variance past the largest double is refused, as scikit-learn's `PCA` fails there with
  `LinAlgError`, and so is a column mean that overflows; a `NaN` or infinity is refused with
  scikit-learn's own messages.
- **Performance kept at `main`'s level.** The two-pass sparse moment reads the matrix
  `RequireFinite` already consolidated, so the duplicate scan runs once, and carries no `NaN`
  branch, since both callers refuse `NaN` first. `TruncatedSvd` transposes its components once at
  the fit rather than on every `Transform`, a 3.2 MB block at 20 × 20,000 that made the projection
  depend on the heap the fit left behind.

## Verification

- The corpora gain the cases the reference answers and the old code could not: tall and wide
  `randomized_svd(transpose="auto")` settings, the issue's offset matrix, `TruncatedSVD` fitted as
  an estimator on wide matrices (singular values, components, explained variance and ratio,
  projection), three wide NNDSVD and NNDSVDA initialisations, and the offset sparse column. Every
  existing case is unchanged; the new ones are appended.
- A random differential against scikit-learn 1.9.1, 1,600 cases over truncated SVD, the NNDSVD
  initialisation, `PCA` variances and the sparse `StandardScaler`, shapes tall and wide, scales from
  `1e-150` to `1e238`: `main` disagrees on 558, this branch on 7, every one of them either the
  `none` normaliser after four or five unnormalised power iterations, where the components move at
  `2e-8` because the block's conditioning is squared per iteration, or a component in the null
  space of a matrix with a zero singular value, which neither side determines. A further 201 cases
  pass `Oversampling = 0`, which `TruncatedSVD` refuses and `randomized_svd` accepts; the option
  page documents that this package accepts it, so there is no estimator answer to
  compare them with.

- A/B/A against `main` at `52f82a4f` on the developer machine (BenchmarkDotNet, `DecompositionBenchmarks`,
  `DecompositionWidthBenchmarks`, `PrincipalComponentVarianceBenchmarks`, `ScalerIncumbentBenchmarks`):
  `TruncatedSvd` at 2,000 × 20,000 fits 32 % faster, the transposed path, and projects 46 % faster,
  the components no longer transposed per call; the two NMF losses at that width 12 and 14 % faster;
  `PrincipalComponentVariance` level with `main` at every size. The sparse `StandardScaler` fit is
  6 % slower at 1,000 rows and 25 % at 20,000: the second pass `mean_variance_axis` makes, which is
  the correction itself.

## Rejected

- **Documenting the transpose divergence**, #1256's first direction: a wide matrix is the common
  shape of a term-document matrix with a real vocabulary, and a reproduction that fails there is
  not one.
- **Scaling every block**: the first version did, and its extra pass cost 30 % on a 2000 × 10
  block; the scale is now paid only where the Gram needs it.
- **Materialising `Xᵀ`** for the wide branch: `TransposeMultiply` already exists and reads the
  matrix in place.
