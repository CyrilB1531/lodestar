# Changelog — Lodestar.Decomposition

What changed in `Lodestar.Decomposition`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.3.0] — 2026-09-24

### Added

- `Nmf.Transform` applies a fitted factorization to rows it never saw, holding `H` fixed, at `sklearn.decomposition.NMF.transform` parity — the fitted object now remembers the loss, the cap and the tolerance it ran under, as the reference replays its own. ([#1124](https://github.com/CyrilB1531/lodestar/issues/1124))
- `PrincipalComponentVariance.Compute` reports the variance each principal component explains. ([#701](https://github.com/CyrilB1531/lodestar/issues/701), [`e311b2c3`](https://github.com/CyrilB1531/lodestar/commit/e311b2c3))

### Changed

- `NmfBetaLoss`, `NmfInitialization`, `PowerIterationNormalizer`, `NmfOptions` and `TruncatedSvdOptions` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `TruncatedSvd`, `Nmf` and `QrDecomposition.Householder` walk their dense blocks in memory order, with the Householder QR up to 5.3 times faster. ([#845](https://github.com/CyrilB1531/lodestar/issues/845))

### Fixed

- `Nmf.Fit` refuses a matrix with no row, no column or an infinity and checks its options before initialising, and `TruncatedSvd` refuses a `NaN` or an infinity, where they threw `DivideByZeroException` or `ArithmeticException` or answered `NaN`. ([#872](https://github.com/CyrilB1531/lodestar/issues/872))
- `QrDecomposition.Householder` compares the span's length with the declared shape in `long`, so a shape whose product overflows `int` is `ArgumentException` rather than `OverflowException`. ([#906](https://github.com/CyrilB1531/lodestar/issues/906))

## [0.2.0] — 2026-09-10

### Added

- **`QrDecomposition` publishes the thin Householder QR this package already writes.** `Householder` returns `Q` and `R` row-major with the shape a least-squares solve wants — `m × n` and `n × n` — beside `RowCount` and `ColumnCount`. It is published rather than copied because `Lodestar.Stats.Regression` needs the same kernel, and two hand-written QRs in one repository disagree eventually. The LU and the one-sided Jacobi SVD beside it stay internal. No new corpus: `tests/oracles/decomposition_qr.json` already exists and the internal kernel replays it, so what the public wrapper owes is its shape and its refusals — a wide matrix has no thin QR and is refused rather than answered with a factorization of a different shape. The signs are the reflections', not a convention, and the page says so where a reader comparing against `numpy.linalg.qr` would otherwise expect entry-for-entry agreement. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))

## [0.1.1] — 2026-09-08

### Changed

- **`Nmf.Fit(matrix, k)` accepts `k == min(rows, columns)`**, scikit-learn's own bound, where it refused any `k` at or above the column count — a bound inherited from the validation `TruncatedSvd` needs rather than from anything NMF does, so a square matrix at full rank was a fit there and an `ArgumentOutOfRangeException` here. The oracle corpus now freezes a `24 × 8` fit at `k = 8` against `NMF` itself, and `TruncatedSvd`'s own bound is untouched: `n_components >= n_features` is what scikit-learn refuses there too. ([#519](https://github.com/CyrilB1531/lodestar/issues/519))

## [0.1.0] — 2026-09-01

### Added

- **`TruncatedSvd` — `sklearn.decomposition.TruncatedSVD(algorithm="randomized")` at parity, over a `CsrMatrix` and without centring it.** Fit, transform, components, singular values, explained variance and its ratio; all three power-iteration normalizers, including `Auto`'s rule. Ω is an input rather than a seed, which is what makes a randomized algorithm an ordinary parity target — [decision 0072](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0072-omega-is-an-input-not-a-seed.md) has the measurement and what it refuses. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))
- **`Nmf` — `sklearn.decomposition.NMF(solver="mu")` at parity, on both β losses, from the NNDSVD family.** The dense kernels it needs — thin Householder QR, LU with partial pivoting, one-sided Jacobi SVD — are written here, so the package's only dependency is `Lodestar.Abstractions`. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))
