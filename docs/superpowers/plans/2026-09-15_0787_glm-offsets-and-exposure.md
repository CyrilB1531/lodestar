# GLM offsets and exposure Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** `GeneralizedLinearModel.Fit` accepts `offset` and `exposure` at `statsmodels` 0.15.0 parity, the null
deviance's refit included, with a benchmark against `statsmodels`.

**Architecture:**

- `Irls.Fit` takes one combined offset (`offset + log(exposure)`, or none) and uses it in two places: the
  working response and the linear predictor.
- A new overload validates the two spans and combines them.
- `NullDeviance` refits the intercept-only model through the same `Irls.Fit` when an offset is present.
- With no offset, the loop takes the branch it takes on `main`: one test before the row loop, never one per
  row.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0.

**Spec:** [`docs/superpowers/specs/2026-09-15_0787_glm-offsets-and-exposure.md`](../specs/2026-09-15_0787_glm-offsets-and-exposure.md)

**Branch:** `feat/787-glm-offsets` — one pull request closes #787, one commit, assigned on start, no assignee on the PR.

## Global Constraints

- **Language and conventions:** everything in English; no `feat:`/`fix:` prefix; `Closes #787`.
- **Build discipline:**
  - compile right after every scripted edit;
  - warnings are errors (S107 above seven parameters, S3776 at 15);
  - every dotnet command goes through `.dotnet-guarded`.
- **No arithmetic without an offset moves:** the existing `stats_glm.json` cases regenerate unchanged and
  replay unchanged.
- **No per-row branch in the IRLS loop.** #775 measured one costing 3% at 100 rows.
- **Benchmark in the pull request before it leaves draft.**

---

### Task 1: IRLS with an offset

`Irls.Fit(design, response, featureCount, in shape, options, double[]? offset)`:

```csharp
// working response
double eta = Families.Link(shape, mu);
working[row] = root * (eta + ((response[row] - mu) * derivative) - (offset is null ? 0.0 : offset[row]));
```

The ternary above is the per-row branch the constraints forbid. `BuildWeightedSystem` takes a
`ReadOnlySpan<double> offset`, and a second loop is used when it is non-empty, so no row pays a test.

The mean update is `eta = Σ xβ (+ offset[row])`, handled the same way.

### Task 2: The overload, the refusals, the null refit

```csharp
public static GlmSummary Fit(
    ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> offset,
    ReadOnlySpan<double> exposure, int featureCount, GlmFamily family, GlmOptions? options = null)
```

- **Refusals:**
  - length ≠ rows, as `ArgumentException` on the span's own name;
  - a non-finite offset, as `ArgumentOutOfRangeException(nameof(offset))`;
  - an exposure that is not finite or not above zero, as `ArgumentOutOfRangeException(nameof(exposure))`;
  - `exposure` with a resolved link other than log, as `ArgumentException(nameof(exposure))`.
- **Combined offset:** `offset[i] + Math.Log(exposure[i])`, or either alone.
- **Shared body:** the existing `Fit` and the new one share a private `FitCore(..., double[]? offset)`.
- **`NullDeviance(shape, response, offset)`:** returns today's value when `offset` is null. Otherwise it runs
  `Irls.Fit` over a column of ones, with `WithIntercept = false`, default tolerance and budget, and the
  caller's family, link and alpha. `start` is `link(mean(y))`. The spec measured that the refit's path moves
  the result by `3.3e-15` at most, so the family's own starting mean serves.

### Task 3: Corpus and tests

- **Generator:**
  - `_glm_fixtures` gains `"offset"` and `"exposure"` keys on the cases the spec lists;
  - `generate_stats_glm` passes them to `sm.GLM` and echoes them;
  - regenerate, and confirm the old cases are unchanged.
- **`GlmOracleTests`:** routes a case with either key to the new overload.
- **Edge tests:**
  - each refusal;
  - a zero offset against no offset to `1e-12` on coefficients and errors;
  - exposure against an offset of its logarithm, exactly;
  - the existing overload against the new one with both spans empty, exactly.

### Task 4: Documents, sample, benchmark

- **Documents:**
  - `generalizedlinearmodel-fit.md`: a second `docs-declaration` and an exposure example;
  - equivalence rows;
  - the migration row;
  - CHANGELOG;
  - an exposure fit in the GLM sample.
- **Harness:** `compare-glm` gains `glm_poisson_exposure_*`. Both sides derive the exposure from the row
  index as `1 + row % 4`. The standard errors are checked against `statsmodels` before timing.
- **A/B/A** of `GlmBenchmarks` and `GlmPoissonBenchmarks` against `main` under the lock.
- **Write-up:** `bench/README.md` and `docs/guides/performance.md` sections, and the table in the pull request.

### Task 5: The full gate, in the background; one commit; the pull request once the table is in
