# Multinomial logit Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** `MultinomialLogit.Fit` at `statsmodels` 0.15.0 `MNLogit` parity, with its inference table and a benchmark
against Accord and `statsmodels`.

**Architecture:**

- **Public surface:** a static entry point, an options record and a summary class in `Lodestar.Stats.Regression`.
- **`Internal/MultinomialNewton.cs`** holds the likelihood, the analytic score and Hessian, and the Newton loop with
  the reference's ridge and stopping rule.
- **`Internal/Cholesky.TryFactor`** factors the Hessian, so separation and rank deficiency are refused where the
  factor fails.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0.

**Spec:** [`docs/superpowers/specs/2026-09-16_0788_multinomial-logit.md`](../specs/2026-09-16_0788_multinomial-logit.md)

**Branch:** `feat/788-multinomial-logit` — one pull request closes #788, one commit, assigned on start, no assignee on the PR.

## Global Constraints

- **Language and conventions:** English; no `feat:`/`fix:` prefix; `Closes #788`.
- **Build discipline:**
  - compile after every scripted edit;
  - warnings are errors (S107 above seven parameters, S3776 at 15);
  - every dotnet command goes through `.dotnet-guarded`.
- **No test per row inside the Newton loop's row sums.** #775 and #787 each measured one.
- **Benchmark in the pull request before it leaves draft.**

---

### Task 1: The Newton fit

```csharp
internal static class MultinomialNewton
{
    // matrix: n × K row-major with the intercept column when asked; labels: 0..J-1 per row.
    public static MultinomialFit Fit(double[] matrix, int[] labels, int rowCount, int columnCount, int categoryCount, MultinomialLogitOptions options);
}
```

- **Probabilities:** a stable softmax per row, with `ηⱼ = xᵢ·βⱼ` for `j ≥ 1` and `η₀ = 0`.
- **Score:** `gⱼ = Σᵢ xᵢ (dᵢⱼ − pᵢⱼ)`.
- **Hessian of `−loglike`:** `A[(j,k),(l,r)] = Σᵢ pᵢⱼ(δⱼₗ − pᵢₗ) xᵢₖ xᵢᵣ`.
- **Step:** `Δ = (A/n + 1e-10·I)⁻¹ (g/n)`, through a Cholesky factor and forward and back substitution.
- **Stopping:** every `|Δ| ≤ tol`, or `maxiter`.
- **Covariance:** the diagonal of `A⁻¹` at the end, as column norms of `L⁻¹`.

### Task 2: The entry point and the table

- **Labels:** the distinct response values sorted ascending, mapped to `0..J−1`.
- **Refusals:**
  - `J < 2`;
  - lengths that disagree;
  - `n − df_model − (J − 1) < 1`;
  - a failed factor or a non-finite step, as `ArgumentException` naming separation or a rank-deficient design.
- **The table:** z statistics, `ChiSquaredSf(z², 1)`, `NormalQuantile` intervals, the closed-form `llnull`,
  `df_model = (K − 1)(J − 1)`, then `llr`, `prsquared`, `aic` and `bic` as the spec states.

### Task 3: Corpus and tests

- **Generator:** `generate_stats_mnlogit` in `tools/generate_oracles.py` over the spec's five fixtures, registered in
  the generators map.
- **`MultinomialLogitOracleTests`:** replays every field at relative `1e-9`, and the iteration count exactly.
- **`MultinomialLogitEdgeTests`:**
  - each refusal;
  - two categories against `GeneralizedLinearModel.Fit(..., GlmFamily.Binomial)`;
  - an order-keeping relabelling.

### Task 4: Documents, sample, benchmark

- **Documents:**
  - reference pages under `docs/reference/stats-regression/mnlogit/`, with the index registered in
    `docs/wiki-map.json`;
  - equivalence rows for `MNLogit` and `OrderedModel`;
  - migration rows;
  - CHANGELOG;
  - a sample use;
  - the decisions README row and the regenerated index.
- **`MultinomialLogitBenchmarks`:** against Accord's `MultinomialLogisticRegression` and `LowerBoundNewtonRaphson`,
  with an agreement check before timing.
- **`compare-glm`:** `mnlogit_*` rows over the stats corpus, with the category derived from the count response.
- **Write-up:** `bench/README.md` and `docs/guides/performance.md` sections, and the table in the pull request.

### Task 5: The full gate, in the background; one commit; the pull request once the table is in
