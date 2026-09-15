# HAC and cluster-robust covariances Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship `CovarianceType.Hac` and `CovarianceType.Cluster` on `OrdinaryLeastSquares.Fit` and
`WeightedLeastSquares.Fit` at `statsmodels` 0.15.0 parity, with a benchmark against `statsmodels`.

**Architecture:** `RobustCovariance` gains a HAC and a cluster filling beside the HC weights, all reading the scores
`xᵢ·rᵢ` of the rows the solve ran on and the same bread `R⁻¹R⁻ᵀ`. `OrdinaryLeastSquares.SolvedFit` carries the cluster
labels; `Tabulate` dispatches on the type and reads the F test on `G − 1` for a cluster fit. The labels reach the pipeline
through two new overloads with a `ReadOnlySpan<int> clusters`.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0.

**Spec:** [`docs/superpowers/specs/2026-09-15_0775_hac-and-cluster-robust-covariances.md`](../specs/2026-09-15_0775_hac-and-cluster-robust-covariances.md)

**Branch:** `feat/775-hac-cluster` — one pull request closes #775, one commit, assigned on start, no assignee on the PR.

## Global Constraints

- **Everything in English**; no `feat:`/`fix:` prefix; `Closes #775`.
- **Compile right after every scripted edit**, before anything longer runs.
- **Warnings are errors**, `AnalysisMode=All`: S107 at seven parameters, S3776 at 15, S3218 on shadowing.
- **Every dotnet command through `"$MAIN/.dotnet-guarded"`**; foreground under 90 s.
- **No arithmetic on the non-robust and HC paths moves**: the existing corpus cases replay unchanged.
- **Benchmark in the pull request before it leaves draft.**

---

### Task 1: The options and the refusals

- [ ] `CovarianceType.Hac = 5`, `CovarianceType.Cluster = 6`, each documented.
- [ ] `OlsOptions.HacLags` (`int?`, `init` refuses a negative) and `OlsOptions.SmallSampleCorrection` (`bool?`).
- [ ] A private `ResolveCovariance(OlsOptions settings, int clusterCount, string optionsName)` in `OrdinaryLeastSquares`,
  shared by both entry points, refusing: `Hac` with no `HacLags`; `HacLags` with another type; `SmallSampleCorrection`
  with a type that has no correction; `Cluster` without clusters, clusters without `Cluster`; fewer than two clusters.

### Task 2: The fillings

```csharp
/// HAC: S = Σ uᵢuᵢᵀ + Σₗ wₗ Σᵢ (uᵢuᵢ₋ₗᵀ + uᵢ₋ₗuᵢᵀ), wₗ = 1 − l/(L+1), l ≤ min(L, n−1).
public static double[] Hac(double[] matrix, double[] residuals, int rowCount, int parameterCount, int lags)

/// Cluster: S = Σ_g s_g s_gᵀ with s_g = Σ_{i∈g} uᵢ, labels made dense in first-seen order.
public static double[] Cluster(double[] matrix, double[] residuals, ReadOnlySpan<int> clusters, int rowCount, int parameterCount, out int clusterCount)
```

Both return the meat; `Sandwich` becomes `Bread · meat · Bread` for every type, the HC weights computing their meat as
today and the leverages computed only for HC2 and HC3. Corrections: HC1 `n/(n−k)`; HAC `n/(n−k)` when asked;
cluster `G/(G−1)·(n−1)/(n−k)` unless refused.

### Task 3: The entry points and the table

- [ ] `OrdinaryLeastSquares.Fit(design, response, ReadOnlySpan<int> clusters, featureCount, options)` and
  `WeightedLeastSquares.Fit(design, response, weights, ReadOnlySpan<int> clusters, featureCount, options)`, each checking
  `clusters.Length == rowCount` and handing the labels to `Summarise`.
- [ ] `SolvedFit` gains `int[]? Clusters`; `Tabulate` passes it to the sandwich and uses `G − 1` as the F test's
  denominator for a cluster fit.
- [ ] The three `<see cref="Fit"/>` in `OrdinaryLeastSquares.Estimate`'s remarks name the overload they mean.

### Task 4: Corpus and tests

- [ ] `_ols_fixtures` and `_wls_fixtures` gain `COVARIANCE_TYPE` values `"HAC"` and `"cluster"` with `"maxlags"`,
  `"useCorrection"` and `"groups"` keys, fitted through `cov_kwds`; `_linear_case` echoes them. Regenerate both corpora
  and confirm the existing cases are unchanged.
- [ ] `LinearOracle.Options` and the oracle tests route a case with `groups` to the cluster overload.
- [ ] Edge tests: each refusal; HAC with no lags equals HC0 to `1e-12`; one cluster per row equals HC0 times
  `n/(n−1)·(n−1)/(n−k)` to `1e-12`; labels relabelled or reordered by group give the same table.

### Task 5: Documents, sample, benchmark

- [ ] `covariancetype.md` members, `olsoptions.md` properties, the two overloads' declarations on
  `ordinaryleastsquares-fit.md` and `weightedleastsquares-fit.md`, equivalence rows, the `statsmodels.md` row, a guide
  paragraph, CHANGELOG, a use in `OlsOptionsSample`.
- [ ] `bench_stats.py` and `StatsCrossLang` gain `ols_hac_*` and `ols_cluster_*` rows over the stats corpus (groups and a
  lag count derived from the row index, written by `generate_stats.py` on a stream of its own); `RobustCovarianceBenchmarks`
  gains `Hac` and `Cluster`; A/B/A against `main` under the lock; `bench/README.md` and `docs/guides/performance.md`
  sections; the table in the pull request.

### Task 6: The full gate, in the background; one commit; pull request as draft until the table is in
