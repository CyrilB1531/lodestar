---
status: accepted
supersedes: []
amends: []
applies: ["0095", "0116"]
---
# 0119 — The explained variance lives in Lodestar.Decomposition

**Status:** accepted · **Date:** 2026-09-13 · **Applies:** [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0116`](0116-the-pca-gap-is-the-explained-variance-not-the-projection.md)

## Context

[Decision 0116](0116-the-pca-gap-is-the-explained-variance-not-the-projection.md) read both .NET
PCA incumbents and found one gap: below `net8.0`, nothing reports how much variance a principal
component explains. ML.NET projects and exposes no eigenvalue; NumFlat has `EigenValues` and ships
`net8.0` only. 0116 scoped the lot, left it for a caller under
[decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s rule, and
left one question open on purpose: **which package it goes in.**

[#701](https://github.com/CyrilB1531/lodestar/issues/701) is that caller, and it framed the
question on a premise:

> `Lodestar.Decomposition`'s stated subject is decompositions *over a `CsrMatrix`*, so a
> dense-only member would be the first thing in it that does not take one.

**That premise is false, and the exported surface says so.** `Lodestar.Decomposition` 0.2.0
already publishes [`QrDecomposition.Householder`](../reference/decomposition/factorization/qrdecomposition-householder.md), which takes a `ReadOnlySpan<double>` and two dimensions: dense, row-major,
no `CsrMatrix` anywhere in the signature. Decision 0095 published it because
`Lodestar.Stats.Regression` needed it. The package's subject was already "the decompositions this
repository writes by hand", with sparse input as the reason most of them exist.

## Decision

**[`PrincipalComponentVariance.Compute`](../reference/decomposition/factorization/principalcomponentvariance-compute.md)
ships in `Lodestar.Decomposition` 0.3.0**, taking a row-major span and its two dimensions. It returns the explained variance, its ratio, the
cumulative curve and the total variance, one entry per component up to `min(n, p)`, at
scikit-learn's `PCA` parity.

- **Not a type named `Pca`.** It has no components, no mean and no `Transform`; 0116 delegated
  all three, and a type called PCA would promise them.
- **The eigenvalues of the centred block's Gram matrix**, `XᵀX` or `XXᵀ` whichever is smaller,
  solved by the one-sided Jacobi kernel this package already carries. Measured on an AMD Ryzen 7
  8700G with a `Stopwatch`, 2,000 × 50: Jacobi over the whole centred block took **21.9 ms**
  against NumFlat's 1.6 ms; reducing to `R` first, 7.5 ms; the Gram route, 4.9 ms; the Gram route
  with a row-major Gram and tracked column norms, **1.9 ms**. The `BenchmarkDotNet` numbers are in
  [`docs/guides/performance.md`](../guides/performance.md).
- **Two refusals scikit-learn does not make.** Fewer than two rows throws, since the variance
  divides by `n − 1`. A matrix with no variance at all throws, where scikit-learn divides by the
  zero total and returns `NaN` for every ratio. A `NaN` or an infinity in the input throws, as
  scikit-learn's input validation does.

## Options that lost

- **`Lodestar.Preprocessing`.** The pipeline argument is real: a scaler usually comes before a
  PCA, and that package already fits on row-major spans. But it has no linear algebra, so it would
  need either a second copy of the Jacobi kernel or an eleventh inter-package edge, to
  `Lodestar.Decomposition`, for one member. Both cost more than the adjacency is worth, and
  scikit-learn itself files `PCA` under `sklearn.decomposition`.
- **`Lodestar.Stats`.** Covariance is statistics. But decision 0095 publishes that package's
  numerical layer member by member, and it holds tails and quantiles, not matrices. The kernel this
  needs is in `Lodestar.Decomposition`.
- **scikit-learn's `svd_solver="full"` computation as written**, the singular values of the
  centred block. It keeps the condition number unsquared, and it lost on cost: 21.9 ms, or 7.5 ms
  through `R`, against 1.9 ms. Squaring the condition costs absolute accuracy near `ε · λ₁`, which
  no ratio can show, and scikit-learn's own `covariance_eigh` solver makes the same trade for tall
  blocks. The frozen corpus pins `svd_solver="full"` and agrees at `1e-9`.
- **A projection alongside it.** Refused by 0116 and not reopened: both incumbents project, and
  what they lack is this number.

## Consequences

- `Lodestar.Decomposition` goes to 0.3.0: a new public type, nothing removed.
- `docs/migration/sklearn.md`'s `explained_variance_ratio_` row stops reading **gap** and points
  here; the projection row is unchanged.
- `JacobiSvd` gains a values-only entry point that tracks column norms. The public
  [`QrDecomposition`](../reference/decomposition/factorization/qrdecomposition.md), the [`TruncatedSvd`](../reference/decomposition/factorization/truncatedsvd.md) path and their corpora do not move.
- NumFlat 1.3.4 joins `bench/Lodestar.Text.Benchmarks` as the incumbent. It is MIT-licensed and
  ships nothing from there.
