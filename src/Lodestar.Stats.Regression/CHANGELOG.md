# Changelog — Lodestar.Stats.Regression

What changed in `Lodestar.Stats.Regression`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `InstrumentalVariables` fits two-stage least squares, LIML with Fuller's correction and two-step GMM at `linearmodels` parity, with the unadjusted, robust, kernel and clustered covariances, the first-stage diagnostics and the overidentification tests. ([#1155](https://github.com/CyrilB1531/lodestar/issues/1155))

## [0.2.0] — 2026-09-24

### Added

- `GeneralizedLinearModel.Fit`, with `GlmFamily`, `GlmOptions` and `GlmSummary`, for binomial and Poisson responses. ([#616](https://github.com/CyrilB1531/lodestar/issues/616), [`8bd2dba6`](https://github.com/CyrilB1531/lodestar/commit/8bd2dba6))
- `CovarianceType` gives `OrdinaryLeastSquares` the heteroskedasticity-consistent estimators `Hc0` to `Hc3`. ([#686](https://github.com/CyrilB1531/lodestar/issues/686), [`1f596f4b`](https://github.com/CyrilB1531/lodestar/commit/1f596f4b))
- `OrdinaryLeastSquares.Estimate` and `OlsEstimate` fit the model without the inference table. ([#671](https://github.com/CyrilB1531/lodestar/issues/671), [`2986d69e`](https://github.com/CyrilB1531/lodestar/commit/2986d69e))
- `WeightedLeastSquares.Fit` fits a linear model with one weight per row. ([#768](https://github.com/CyrilB1531/lodestar/issues/768), [`1d0a3630`](https://github.com/CyrilB1531/lodestar/commit/1d0a3630))
- `GlmFamily.NegativeBinomial` and `GlmOptions.NegativeBinomialAlpha` fit over-dispersed counts. ([#769](https://github.com/CyrilB1531/lodestar/issues/769), [`94e13f2d`](https://github.com/CyrilB1531/lodestar/commit/94e13f2d))
- `GlmFamily.Gamma` and `GlmLink` fit a positive, skewed response. ([#770](https://github.com/CyrilB1531/lodestar/issues/770), [`b55b7835`](https://github.com/CyrilB1531/lodestar/commit/b55b7835))
- `GeneralizedLeastSquares.Fit` fits a linear model under a caller-supplied error covariance. ([#771](https://github.com/CyrilB1531/lodestar/issues/771), [`0ec985fa`](https://github.com/CyrilB1531/lodestar/commit/0ec985fa))
- `CovarianceType.Hac` and `CovarianceType.Cluster` give the least-squares fits Newey–West and cluster-robust errors. ([#775](https://github.com/CyrilB1531/lodestar/issues/775), [`c6f4dce0`](https://github.com/CyrilB1531/lodestar/commit/c6f4dce0))
- `GeneralizedLinearModel.Fit` takes an offset and an exposure. ([#787](https://github.com/CyrilB1531/lodestar/issues/787), [`242f8631`](https://github.com/CyrilB1531/lodestar/commit/242f8631))
- `MultinomialLogit.Fit` fits an unordered categorical response with its inference table. ([#788](https://github.com/CyrilB1531/lodestar/issues/788), [`969b4530`](https://github.com/CyrilB1531/lodestar/commit/969b4530))

### Changed

- `CovarianceType`, `GlmFamily` and `GlmLink` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `GeneralizedLinearModel.Fit`, `GeneralizedLeastSquares.Fit` and `MultinomialLogit.Fit` reuse their buffers and vectorise the least-squares inner product, halving a negative binomial fit. ([#845](https://github.com/CyrilB1531/lodestar/issues/845))
- `OlsOptions` is a record with `init` properties. ([#616](https://github.com/CyrilB1531/lodestar/issues/616), [`8bd2dba6`](https://github.com/CyrilB1531/lodestar/commit/8bd2dba6))
- A Poisson count above one million is fitted rather than refused. ([#665](https://github.com/CyrilB1531/lodestar/issues/665), [`6ecf9c05`](https://github.com/CyrilB1531/lodestar/commit/6ecf9c05))
- The HC2 and HC3 covariances no longer read Q through an interface. ([#670](https://github.com/CyrilB1531/lodestar/issues/670), [`88b32f78`](https://github.com/CyrilB1531/lodestar/commit/88b32f78))
- The least-squares pipeline under the OLS, WLS and GLM fits no longer forms Q. ([#782](https://github.com/CyrilB1531/lodestar/issues/782), [`37c71cb9`](https://github.com/CyrilB1531/lodestar/commit/37c71cb9))
- The negative binomial log-likelihood reads `lnΓ(1/α)` once per fit. ([#781](https://github.com/CyrilB1531/lodestar/issues/781), [`37c71cb9`](https://github.com/CyrilB1531/lodestar/commit/37c71cb9))

### Fixed

- `OrdinaryLeastSquares`' documentation says `Fit` tries the normal equations before the Householder reflections and `Estimate` always takes the reflections, where it described a Householder QR throughout. ([#869](https://github.com/CyrilB1531/lodestar/issues/869))
- `OlsOptions.CovarianceType` refuses an undeclared `CovarianceType`, which `OrdinaryLeastSquares`, `WeightedLeastSquares` and `GeneralizedLeastSquares` computed as `Hc0` and echoed as the undeclared value. ([#868](https://github.com/CyrilB1531/lodestar/issues/868))
- `OrdinaryLeastSquares.Fit`, `WeightedLeastSquares.Fit` and `GeneralizedLeastSquares.Fit` take the normal equations only when an upper bound on the column-scaled condition number stays within 200, where a cubic on a narrow range landed 1.4e-5 from statsmodels. ([#870](https://github.com/CyrilB1531/lodestar/issues/870))
- `OrdinaryLeastSquares.Fit`, `WeightedLeastSquares.Fit`, `GeneralizedLeastSquares.Fit` and `GeneralizedLinearModel.Fit` refuse a collinear design, where they returned a table of NaN or of coefficients near 1e14, or ran the IRLS budget out. ([#867](https://github.com/CyrilB1531/lodestar/issues/867))
- `GeneralizedLeastSquares.Fit` refuses an empty covariance at 65,536 rows, `CovarianceType.Hac` weights `int.MaxValue` lags below one, and `MultinomialLogit.Fit` names `response` when the label count disagrees. ([#905](https://github.com/CyrilB1531/lodestar/issues/905))
- `OrdinaryLeastSquares.Estimate` refuses a collinear design as `Fit` does, where it answered coefficients near 1e14. ([#979](https://github.com/CyrilB1531/lodestar/issues/979))
- A collinear design is refused on `numpy.linalg.matrix_rank`'s own tolerance, `σmin ≤ σmax·max(n, p)·ε`, where a per-column pivot missed a dependent column far smaller than its sources, and the normal-equations gate measures the column-scaled condition number rather than a bound that was always at least `p`. ([#978](https://github.com/CyrilB1531/lodestar/issues/978), [#985](https://github.com/CyrilB1531/lodestar/issues/985))

## [0.1.0] — 2026-09-10

### Changed

- **The variance inflation factors are read off one decomposition instead of one regression per regressor.** `Fit` ran an auxiliary least squares for every regressor to reach its VIF, which put five Householder QRs of the design behind a four-regressor fit; for standardised regressors a VIF is a diagonal entry of the inverse correlation matrix, so a single QR of the standardised block answers all of them. Same numbers — the frozen corpus agrees to 1.3e-12 relative on its near-collinear case and to 3.4e-15 or better on the other five, inside the 1e-9 it is compared at — and the twelfth significant digit of the reference page's 6e4 example moved with it. Measured on 10 000 rows and four regressors, an AMD Ryzen 7 8700G on Ubuntu 26.04.1 with .NET SDK 10.0.401: 3 628 µs to 1 646 µs and 6 490 KB to 2 893 KB, which turns 0.75× against `Accord.Statistics` into 1.63×. ([#591](https://github.com/CyrilB1531/lodestar/issues/591))

### Added

- **A fourteenth package: ordinary least squares with the table that makes it inference.** `OrdinaryLeastSquares.Fit` takes a row-major design and a response and returns an `OlsSummary` carrying the coefficients, their standard errors, t statistics, two-sided p-values and confidence intervals at a stated level, alongside R-squared and its adjusted form, the overall *F* and its p-value, the residual standard error and degrees of freedom, and a variance inflation factor per regressor — at `statsmodels` 0.15.0 parity, replayed over six frozen cases and compared **relatively**, as [decision 0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md) established for p-values. Solved through the Householder QR `Lodestar.Decomposition` 0.2.0 publishes rather than the normal equations, whose `XᵀX` squares the condition number of exactly the near-collinear designs a VIF exists to report; the tails come from `Lodestar.Stats` 0.2.0. Core tier holds: two Lodestar edges and nothing external. **The #427 protocol was run before the code, and it replaced the gap claim rather than confirming it** — read through a `MetadataLoadContext`, `Accord.Statistics` 3.8.0 exports the whole inference table and has been archived under LGPL-2.1 since 2017, while `MathNet.Numerics` 5.0.0 carries coefficient standard errors only in `Optimization.NonlinearMinimizationResult`, for the non-linear minimisers, unreachable from `LinearRegression` and followed by no t, no p-value, no interval, no adjusted R-squared, no *F* and no VIF across 5 333 exported members. [Decision 0096](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0096-ordinary-least-squares-earns-its-own-package.md) has the reading, and records that **`statsmodels` 0.15.0 changed what a VIF is**: `standardize=True` centres and scales each column before the auxiliary regression, which is a no-op with an intercept and moves the no-intercept answer from `87.43` to `21.0`. One divergence, in `docs/equivalence.md`: a single regressor with no intercept leaves the auxiliary design empty, where the reference raises and this reports `NaN`. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))
