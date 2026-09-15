# Weighted least squares Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship `WeightedLeastSquares.Fit` at `statsmodels.WLS` 0.15.0 parity, returning the `OlsSummary` table.

**Architecture:** `OrdinaryLeastSquares.Fit` keeps its validation and moves everything from the solve onward
into an internal `Summarise`. `WeightedLeastSquares.Fit` validates the weights, whitens the design and the
response by `√w`, computes the weighted total sum of squares, and calls `Summarise` with the unweighted
matrix for the VIFs.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0 as the oracle.

**Spec:** [`docs/superpowers/specs/2026-09-15_0768_weighted-least-squares.md`](../specs/2026-09-15_0768_weighted-least-squares.md)

**Branch:** `feat/768-wls` — one pull request closes #768, spec, plan, code and documents together, one commit.
Based on #671's branch until #767 merges, then rebased onto `main`.

## Global Constraints

- **Everything in English**; no `feat:`/`fix:` prefix; `Closes #768` in the pull request.
- **Both target frameworks, one public API.** `double.IsFinite` is absent on `netstandard2.0`: ask `IsNaN`
  and `IsInfinity` separately.
- **Warnings are errors**, `AnalysisMode=All`: S107 caps a method at seven parameters, S3776 at complexity 15.
- **Comment rules:** why, not what; two lines inline, eight of XML prose; `long-comment:` past that.
- **`$MAIN`** is the main checkout, `MAIN="$(cd "$(git rev-parse --git-common-dir)/.." && pwd -P)"`; every dotnet command goes through
  `"$MAIN/.dotnet-guarded"`, with `LodestarUseProjectRefs` unset.
- **No arithmetic in the OLS path moves**: `stats_ols.json` and `OlsEstimateTests` replay unchanged.
- **Test count:** `dotnet test Lodestar.slnx -c Release` reports **36 assemblies**.
- **The oracle** runs from `/var/tmp` with `PYTHONSAFEPATH=1 "$MAIN/.venv-oracles/bin/python"`; read its own
  exit code.

---

### Task 1: Extract `Summarise` from `OrdinaryLeastSquares.Fit`

**Files:**

- Modify: `src/Lodestar.Stats.Regression/OrdinaryLeastSquares.cs`

- [ ] **Step 1: Split `Fit`**

`Fit` keeps the guard, `LeastSquares.Rows`, the degrees-of-freedom refusal and `LeastSquares.Design`, then:

```csharp
return Summarise(
    matrix, response, matrix, TotalSumOfSquares(response, settings.WithIntercept),
    rowCount, parameterCount, settings);
```

`internal static OlsSummary Summarise(double[] matrix, ReadOnlySpan<double> response, double[] unweighted,
double totalSumOfSquares, int rowCount, int parameterCount, OlsOptions settings)` holds the rest of today's
body, with `RSquared(...)` replaced by `1.0 - (residualSumOfSquares / totalSumOfSquares)` and `Vif(matrix, …)`
by `Vif(unweighted, …)`. `RSquared` becomes `internal static double TotalSumOfSquares(ReadOnlySpan<double>
response, bool withIntercept)` returning `total` unchanged.

The degrees-of-freedom refusal moves to `internal static void RequireResidualDegreesOfFreedom(int rowCount,
int parameterCount, string parameterName)`, throwing the same `ArgumentException`; `Fit` passes `nameof(design)`.

- [ ] **Step 2: Replay OLS**

Run: `"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.Regression.Tests -c Release`
Expected: the same count as before the change, zero failures.

### Task 2: The corpus

**Files:**

- Modify: `tools/generate_oracles.py`
- Create: `tests/oracles/stats_wls.json`
- Modify: `tests/Lodestar.Stats.Regression.Tests/Lodestar.Stats.Regression.Tests.csproj`

- [ ] **Step 1: Fixtures and generator**

Beside `_ols_fixtures`, a `_wls_fixtures()` of hand-written cases each carrying `WEIGHTS`:
simple regression with intercept; no intercept; a zero weight; unit weights; constant weights of 4;
three regressors on twelve rows at 99%; a near-collinear pair; HC0, HC1, HC2, HC3 on the simple design, and HC3
with a zero weight. Robust cases stay on one regressor: a Wald statistic in the tens of thousands breaks decision
0073's absolute `1e-9` across machines. `generate_stats_wls()` mirrors `generate_stats_ols` with

```python
model = sm.WLS(response, exog, weights=np.array(fixture[WEIGHTS]))
fitted = model.fit() if kind == "nonrobust" else model.fit(cov_type=kind)
```

and the same keys plus `WEIGHTS`, the VIF read by `variance_inflation_factor(exog, i)` on the unweighted
`exog`. Register `"stats_wls.json": generate_stats_wls` after `stats_ols.json`.

- [ ] **Step 2: Generate and link**

`main()` writes every corpus and takes no selection, so the one file is written by importing the module:

```bash
cd /var/tmp && PYTHONSAFEPATH=1 "$MAIN/.venv-oracles/bin/python" -c "
import importlib.util, json
spec = importlib.util.spec_from_file_location('g', '<worktree>/tools/generate_oracles.py')
g = importlib.util.module_from_spec(spec); spec.loader.exec_module(g)
p = g.ORACLE_DIR / 'stats_wls.json'
with p.open('w', encoding='utf-8', newline='\n') as f:
    json.dump(g.generate_stats_wls(), f, ensure_ascii=False, indent=1, allow_nan=False); f.write('\n')
"; echo rc=$?
```

Expected: `rc=0`, `tests/oracles/stats_wls.json` written. Add the `None` item beside `stats_ols.json`. Regenerate
`stats_ols.json` the same way and confirm `git status` shows it unchanged; CI's reproducibility job runs the full
`main()`.

### Task 3: `WeightedLeastSquares.Fit`, test first

**Files:**

- Create: `tests/Lodestar.Stats.Regression.Tests/WlsOracleTests.cs`, `tests/Lodestar.Stats.Regression.Tests/WlsEdgeTests.cs`
- Create: `src/Lodestar.Stats.Regression/WeightedLeastSquares.cs`

- [ ] **Step 1: Oracle tests**

`WlsOracleTests` is `OlsOracleTests` over `stats_wls.json`, its `Fit` calling
`WeightedLeastSquares.Fit(Doubles(frozen, "design"), Doubles(frozen, "response"), Doubles(frozen, "weights"),
featureCount, options)`, with the four value theories (the covariance echo stays OLS's). Both replays share a `LinearOracle` helper extracted from `OlsOracleTests`.

- [ ] **Step 2: Edge tests**

```csharp
[Fact]
public void Unit_weights_reproduce_the_ordinary_fit_exactly()
{
    OlsSummary ordinary = OrdinaryLeastSquares.Fit(Design, Response, 1);
    OlsSummary weighted = WeightedLeastSquares.Fit(Design, Response, [1, 1, 1, 1, 1, 1, 1, 1], 1);
    Assert.Equal(ordinary.Coefficients, weighted.Coefficients);
    Assert.Equal(ordinary.StandardErrors, weighted.StandardErrors);
    Assert.Equal(ordinary.RSquared, weighted.RSquared);
    Assert.Equal(ordinary.FStatistic, weighted.FStatistic);
}
```

plus: weights of the wrong length refused on `weights`; a negative, a `NaN` and an infinite weight refused
(`[Theory]`); all-zero weights refused; one positive weight for two parameters refused; a zero weight keeps
the row's degree of freedom (`ResidualDegreesOfFreedom == 6` on eight rows with one zero weight); the
feature-count, whole-rows and degrees-of-freedom refusals shared with OLS.

- [ ] **Step 3: Implement**

```csharp
public static OlsSummary Fit(
    ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> weights,
    int featureCount, OlsOptions? options = null)
{
    Guard.NotLessThan(featureCount, 1);
    OlsOptions settings = options ?? new OlsOptions();
    int rowCount = LeastSquares.Rows(design, response, featureCount);
    int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
    OrdinaryLeastSquares.RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
    CheckWeights(weights, rowCount, parameterCount);

    double[] matrix = LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept);
    var whitened = new double[matrix.Length];
    var whitenedResponse = new double[rowCount];
    for (int row = 0; row < rowCount; row++)
    {
        double root = Math.Sqrt(weights[row]);
        for (int column = 0; column < parameterCount; column++)
        {
            int at = (row * parameterCount) + column;
            whitened[at] = matrix[at] * root;
        }

        whitenedResponse[row] = response[row] * root;
    }

    return OrdinaryLeastSquares.Summarise(
        whitened, whitenedResponse, matrix,
        TotalSumOfSquares(response, weights, settings.WithIntercept),
        rowCount, parameterCount, settings);
}
```

`TotalSumOfSquares` is `Σ wᵢ(yᵢ − Σwy/Σw)²` with a constant and `Σ wᵢyᵢ²` without.

- [ ] **Step 4: Run**

Run: `"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.Regression.Tests -c Release`
Expected: every WLS theory green, OLS unchanged.

### Task 4: Documents and sample

**Files:**

- Create: `docs/reference/stats-regression/wls.md`, `docs/reference/stats-regression/wls/weightedleastsquares.md`,
  `docs/reference/stats-regression/wls/weightedleastsquares-fit.md`
- Modify: `docs/wiki-map.json` (`covered` gains `docs/reference/stats-regression/wls`), `docs/equivalence.md`
  (the `sm.WLS, sm.GLS` row becomes a `sm.WLS` row with its three divergences, GLS keeps its own),
  `docs/migration/statsmodels.md` (the `WLS` row reads native), `docs/guides/regression-inference.md`,
  `CHANGELOG.md` (`Lodestar.Stats.Regression` Added), `docs/reference/stats-regression/ols.md` see-also
- Create: `samples/Lodestar.Sample/WeightedLeastSquaresSample.cs`; Modify: `samples/Lodestar.Sample/Program.cs`

- [ ] **Step 1: Write the pages** with executed snippets whose `// =>` values come from running them.
- [ ] **Step 2: Sample** in the shape of `OlsEstimateSample`, `Inv.F4` for every number.

### Task 5: The full gate

- [ ] Every `tools/check_*.py` (`--base origin/main` where it takes one), `pytest tools/tests`,
  `dotnet format --verify-no-changes`, markdownlint, `dotnet test Lodestar.slnx -c Release` (36 assemblies),
  a fresh pack of the 18 packages with `check_nuspec_dependencies.py artifacts --require-all`, the sample
  run and the doc snippets with an isolated `NUGET_PACKAGES`.
- [ ] One commit, pushed; pull request `Closes #768`, milestone *Next release*, labels
  `enhancement`, `api`, `Lodestar.Stats.Regression`.
