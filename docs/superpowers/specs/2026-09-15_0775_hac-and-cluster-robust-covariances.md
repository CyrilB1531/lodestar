# 0775 — HAC and one-way cluster-robust covariances, at `statsmodels` parity

**Status:** written before the work, 2026-09-15.

Issue: [#775](https://github.com/CyrilB1531/lodestar/issues/775), a sub-issue of the econometrics umbrella
[#773](https://github.com/CyrilB1531/lodestar/issues/773).
Reading: [decision 0115](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0115-the-robust-covariances-come-first-and-the-tail-was-already-published.md),
which shipped HC0–HC3 and nothing past them; not amended or applied since.

## Problem

HC0–HC3 answer errors whose variance differs row to row and that are otherwise independent. Two common designs break the
second half: **serially correlated errors** — a regression on time-ordered rows — and **rows grouped** by firm, school or
patient, whose errors move together inside a group. The standard errors a caller reads from either are too small, and
`docs/migration/statsmodels.md` calls the cluster-robust half *unscoped*.

## Scope

- `sm.OLS(...).fit(cov_type="HAC", cov_kwds={"maxlags": L})` and `use_correction=True`, the Bartlett kernel.
- `sm.OLS(...).fit(cov_type="cluster", cov_kwds={"groups": g})` and `use_correction=False`, one-way.
- Both on `WeightedLeastSquares` too, which statsmodels computes on the whitened rows.
- **Out:** other HAC kernels (`weights_func`), `hac-panel` and `hac-groupsum`, two-way clustering, `cluster-crv3` and
  `cluster-jk`, `use_t=True`, `df_correction=False`, and `GeneralizedLeastSquares` (its `Tabulate` would accept the matrix,
  but nothing in the issue asks and no corpus would cover it).

## What the reference does, read and measured

From `statsmodels/stats/sandwich_covariance.py` and `RegressionResults.get_robustcov_results`, 0.15.0:

1. **The scores** are `xu = wexog · wresid`, the whitened design times the whitened residuals, one row per observation.
2. **HAC.** `S = xuᵀxu + Σₗ₌₁ᴸ wₗ (xu[l:]ᵀ xu[:-l] + its transpose)` with Bartlett weights `wₗ = 1 − l/(L+1)`, lags taken in
   row order; the covariance is `(XᵀX)⁻¹ S (XᵀX)⁻¹`. `maxlags` is required (`kwargs["maxlags"]`, a `KeyError` without it).
   `use_correction` defaults to **false**; true multiplies by `n/(n−k)`.
3. **Cluster.** `S = Σ_g (Σ_{i∈g} xuᵢ)(Σ_{i∈g} xuᵢ)ᵀ`, the same sandwich. `use_correction` defaults to **true**:
   `G/(G−1) · (n−1)/(n−k)`. `int64` labels go to `np.bincount` as they are — a negative one raises, a gap is an empty
   bin — and any other dtype through `np.unique`; either way only which rows share a label matters.
4. **The distribution.** `use_t` is false: coefficients read *z* against the normal, the interval multiplier with them —
   as for HC0–HC3. The overall test is the Wald statistic over the non-constant coefficients, read against F.
5. **The F test's denominator.** For HAC it is `df_resid`, `n − k`. For cluster, `df_correction` defaults on and sets
   `df_resid_inference = G − 1`, which the F p-value reads; `df_resid` itself stays `n − k`. Found while benchmarking:
   that holds only when `fvalue` is read before `f_pvalue`, because `fvalue` caches its `f_test`'s p-value; read first,
   `f_pvalue` uses `n − k` (0.00046 against 1.92e-12 on the reference page's example). `G − 1` is followed.

Measured on 40 rows, two regressors and a constant, AR(1) errors at 0.5, eight clusters of five — the slope's standard
error: HAC(3) 0.268513, HAC(3) corrected 0.279186; cluster 0.359133, cluster uncorrected 0.327211; WLS HAC(3) 0.297004.
F: 4.321321 (HAC), 2.400571 (cluster, on `(2, 7)`).

## Placement

- **`CovarianceType.Hac` and `CovarianceType.Cluster`**, new members beside HC0–HC3: the summary's `CovarianceType` keeps
  saying which estimator produced the table.
- **`OlsOptions.HacLags`, an `int?`**, required with `Hac` and refused with any other type; **`OlsOptions.SmallSampleCorrection`,
  a `bool?`**, `null` meaning the reference's default for the type (false for HAC, true for cluster) and refused with a type
  that has no correction. Both are values, so the record's equality stays honest (#668).
- **The groups are an argument, not an option**: `OrdinaryLeastSquares.Fit(design, response, clusters, featureCount, options)`
  and `WeightedLeastSquares.Fit(design, response, weights, clusters, featureCount, options)`, with `ReadOnlySpan<int> clusters`,
  one label per row. A per-row array on an options record is #668's problem, and the extra span before `featureCount` is the
  shape `WeightedLeastSquares` and `GeneralizedLeastSquares` already have. The published `Fit` signatures do not move.
- **Rejected:** a `ClusterOptions` companion record (a second options type per call for one array); overloading on
  `int[]` in `OlsOptions` (#668); an optional `clusters` parameter on the existing `Fit` (changes a published signature).

## Behaviour

- **Parity** on points 1 to 5, the F test's `G − 1` included; `OlsSummary.ResidualDegreesOfFreedom` stays `n − k`, as the
  reference's `df_resid` does.
- **Refused:** `Hac` without `HacLags`, or with a negative one; `HacLags` with another type; `SmallSampleCorrection` with a
  non-robust type or HC0–HC3; `Cluster` through an overload without `clusters`, and `clusters` with any type but `Cluster`;
  `clusters` of the wrong length; fewer than two distinct clusters, where the reference's correction divides by `G − 1` and
  raises `ZeroDivisionError`. A negative `HacLags` is refused where the reference raises `IndexError`.
- **`HacLags` at or past the row count is accepted, at parity**: the lags past `n − 1` contribute no pairs, and the Bartlett
  weights still read `L` — measured on ten rows, `maxlags` 9, 10 and 12 give three different standard errors.

## Evidence

- `stats_ols.json` and `stats_wls.json` gain HAC and cluster cases from `generate_stats_ols`/`generate_stats_wls`: HAC at
  two lag counts, with and without correction, with and without an intercept; cluster with and without correction, uneven
  cluster sizes, and labels that are neither dense nor sorted. The existing cases regenerate unchanged.
- Edge tests for each refusal; HAC with zero lags equals HC0 to 1e-12; one cluster per row equals HC0, to 1e-12, up to the correction
  factor.
- **Benchmark:** no free .NET library computes either covariance — `tools/survey.cs` on `Accord.Statistics` 3.8.0,
  `MathNet.Numerics` 5.0.0, `Meta.Numerics` 4.2.0 and `NumFlat` 1.3.4 with
  `(NeweyWest|Newey|HeteroskedasticityAndAutocorrelation|Autocorrelation.?Consistent|Cluster.?Robust|ClusteredStandard|Sandwich|Hac[A-Z]|HAC)`
  matches nothing but a Math.NET test function — so the incumbent is `statsmodels` through the `compare-ols` harness, with
  `ols_hac` and `ols_cluster` rows; and an A/B/A of `RobustCovarianceBenchmarks` and `OlsBenchmarks` against `main` for the paths that
  already ship.
