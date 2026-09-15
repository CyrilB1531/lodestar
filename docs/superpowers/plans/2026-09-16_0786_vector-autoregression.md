# Vector autoregression Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** `VectorAutoregression.Fit` estimates a VAR(p) with its inference table at `statsmodels` 0.15.0 parity, with a
benchmark against `statsmodels`.

**Architecture:**

- `Lodestar.Stats.TimeSeries` gains three public types and one internal.
- `Internal/LagDesign.cs` stacks the lags once: `T = n − p` rows of `1 + K·p` columns, the constant dropped when
  the options say so.
- Each equation is fitted through `OrdinaryLeastSquares.Estimate`, the published surface `DickeyFullerRegression`
  already uses, so the solve is the regression package's Householder QR.
- The residual covariance, the log-likelihood and the four criteria are computed once from the residual matrix.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0.

**Spec:** [`docs/superpowers/specs/2026-09-16_0786_vector-autoregression.md`](../specs/2026-09-16_0786_vector-autoregression.md)

**Branch:** `feat/786-var` — one pull request closes #786, one commit, assigned on start, no assignee on the PR.

## Global Constraints

- **Language and conventions:** English; no `feat:`/`fix:` prefix; `Closes #786`.
- **Build discipline:** compile after every scripted edit; warnings are errors; every dotnet command through
  `.dotnet-guarded`.
- **Public surface needs its paperwork:** a reference page per type registered in `docs/wiki-map.json`, and a sample per
  type, or `check_sample_coverage` fails.
- **Benchmark in the pull request before it leaves draft.**

---

### Task 1: The lag design and the fit

```csharp
internal static class LagDesign
{
    // series is row-major in time; returns T x (1 + K*p) row-major, oldest usable observation first.
    public static double[] Stack(ReadOnlySpan<double> series, int variableCount, int lagOrder, bool withIntercept, out int usable);
}
```

`VectorAutoregression.Fit` then, per equation `j`:

```csharp
OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, responses[j], featureCount, withIntercept: false);
```

with the constant already in the design, so the reference's row order — constant, lag 1's `K`, lag 2's `K` — is the
order the estimate reports.

### Task 2: The table

- **Residuals** `T × K`, then `S = residualᵀresidual`, `sigma_u = S/(T − df_model)` and `sigma_u_mle = S/T`.
- **`llf`, `aic`, `bic`, `hqic`, `fpe`** from `Σ̂ = sigma_u_mle` and `m = K²p + K`, the five formulas the spec pins.
- **The tests read the normal**: `PValues = ChiSquaredSf(t², 1)`, as the robust covariances do in the regression
  package. No intervals: the reference publishes none.
- **Refusals** as the spec lists, each with the parameter it names.

### Task 3: Corpus and tests

- `generate_stats_var` over four fixtures — two variables at lag 1 and 2, three variables at lag 2, and a fit without
  an intercept — written from a seeded draw and frozen as literals.
- `VectorAutoregressionOracleTests` replays every field at `1e-9`.
- `VectorAutoregressionEdgeTests`: each refusal; one equation against `OrdinaryLeastSquares.Fit` on the same design;
  `sigma_u` against residuals computed by hand.

### Task 4: Documents, samples, benchmark

- Reference pages for the three types under `docs/reference/stats-timeseries/var/`, the index registered in
  `docs/wiki-map.json`, an equivalence row, the migration row moved from "not written" to native, CHANGELOG, and one
  sample per type.
- `compare-var`: `StatsCrossLang.RunVar` and a `bench_stats.py` row, both taking the first two columns of the corpus
  design as the series, with `bench/compare.py var`.
- `VectorAutoregressionBenchmarks` for the allocations, with the agreement checked in its setup.
- `bench/README.md` and `docs/guides/performance.md` sections, and the table in the pull request.

### Task 5: The full gate, in the background; one commit; the pull request once the table is in
