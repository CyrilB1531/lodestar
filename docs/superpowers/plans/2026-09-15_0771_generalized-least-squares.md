# Generalized least squares Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship `GeneralizedLeastSquares.Fit` with a full error covariance at `statsmodels.GLS` 0.15.0 parity.

**Architecture:** An internal `Cholesky` factors the covariance and whitens by forward substitution; the whitened
design and response go through `OrdinaryLeastSquares.Summarise` with the GLS total sum of squares and the
unwhitened design for the VIFs, exactly as `WeightedLeastSquares` does.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0 as the oracle.

**Spec:** [`docs/superpowers/specs/2026-09-15_0771_generalized-least-squares.md`](../specs/2026-09-15_0771_generalized-least-squares.md)

**Branch:** `feat/771-gls` from `main` — one pull request closes #771, one commit, no assignee.

## Global Constraints

- **Everything in English**; no `feat:`/`fix:` prefix; `Closes #771`.
- **Warnings are errors**, `AnalysisMode=All`; S107 at seven parameters; S1244 suppressed at the call site with a reason.
- **Comment rules:** two lines inline, eight of XML prose, `long-comment:` past that.
- **`$MAIN`** is the main checkout; dotnet through `"$MAIN/.dotnet-guarded"`, foreground under 90 s.
- **No arithmetic on the OLS or WLS paths moves**: `stats_ols.json` and `stats_wls.json` replay unchanged.
- **Literals named** once they reach three uses in `tools/generate_oracles.py` (`check_repeated_literals.py`).

---

### Task 1: The Cholesky, test first

**Files:** Create `src/Lodestar.Stats.Regression/Internal/Cholesky.cs`, `tests/Lodestar.Stats.Regression.Tests/CholeskyTests.cs`.

- [ ] **Step 1: Tests** — `[[4,2,0.4],[2,5,1],[0.4,1,3]]` factors to a lower `L` with `L Lᵀ` equal to it within
  `1e-14`; `ForwardSubstitute` of `L` against a column returns `L⁻¹ b` checked by multiplying back; a matrix
  with a negative pivot and one with a pivot of `1e-13` of its diagonal are refused.
- [ ] **Step 2: Implement**, row by row (Cholesky–Banachiewicz), reading the lower triangle:

```csharp
internal static bool TryFactor(ReadOnlySpan<double> matrix, int order, out double[] lower)
{
    lower = new double[checked(order * order)];
    for (int row = 0; row < order; row++)
    {
        for (int column = 0; column <= row; column++)
        {
            double sum = matrix[(row * order) + column];
            for (int k = 0; k < column; k++)
            {
                sum -= lower[(row * order) + k] * lower[(column * order) + k];
            }

            if (row != column)
            {
                lower[(row * order) + column] = sum / lower[(column * order) + column];
            }
            else if (sum > PivotFloor * Math.Abs(matrix[(row * order) + row]) && !double.IsInfinity(sum))
            {
                lower[(row * order) + row] = Math.Sqrt(sum);
            }
            else
            {
                return false;
            }
        }
    }

    return true;
}

/// <summary>Overwrites <paramref name="column"/>, stride <paramref name="stride"/>, with <c>L⁻¹</c> applied to it.</summary>
internal static void ForwardSubstitute(double[] lower, int order, double[] values, int columnCount, int column)
```

### Task 2: The corpus

**Files:** `tools/generate_oracles.py`, `tests/oracles/stats_gls.json`, the test `.csproj`.

- [ ] **Step 1:** `_gls_fixtures()` carrying `COVARIANCE` (row-major `n²`) built from `ρ^|i−j|` or an
  equicorrelated block, and `generate_stats_gls()`:

```python
cases = [
    _linear_case(fixture, sm.GLS(np.array(fixture[RESPONSE]), _linear_exog(fixture),
                                 sigma=np.array(fixture[COVARIANCE]).reshape(n, n)))
    for fixture in _gls_fixtures()
]
```

  `_linear_case` echoes `COVARIANCE` when the fixture has it, as it echoes `WEIGHTS`. Register
  `"stats_gls.json"` after `stats_wls.json`; regenerate `stats_ols.json` and `stats_wls.json` and confirm
  `git status` shows them unchanged.

### Task 3: `GeneralizedLeastSquares.Fit`

**Files:** Create `src/Lodestar.Stats.Regression/GeneralizedLeastSquares.cs`, `tests/.../GlsOracleTests.cs`,
`tests/.../GlsEdgeTests.cs`.

- [ ] **Step 1: Oracle tests** — `WlsOracleTests` over `stats_gls.json`, reading `covariance`.
- [ ] **Step 2: Edge tests** — wrong length, a `NaN`, an asymmetric pair, a non-positive-definite matrix, each
  refused on `covariance`; `diag(σ)` against `WeightedLeastSquares.Fit(weights = 1/σ)` within `1e-12`; the
  identity against `OrdinaryLeastSquares.Fit` exactly.
- [ ] **Step 3: Implement**

```csharp
public static OlsSummary Fit(
    ReadOnlySpan<double> design, ReadOnlySpan<double> response, ReadOnlySpan<double> covariance,
    int featureCount, OlsOptions? options = null)
{
    Guard.NotLessThan(featureCount, 1);
    OlsOptions settings = options ?? new OlsOptions();
    int rowCount = LeastSquares.Rows(design, response, featureCount);
    int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
    OrdinaryLeastSquares.RequireResidualDegreesOfFreedom(rowCount, parameterCount, nameof(design));
    double[] lower = Factor(covariance, rowCount);          // length, finiteness, symmetry, positive definiteness

    double[] matrix = LeastSquares.Design(design, rowCount, featureCount, settings.WithIntercept);
    double[] whitened = (double[])matrix.Clone();
    for (int column = 0; column < parameterCount; column++)
    {
        Cholesky.ForwardSubstitute(lower, rowCount, whitened, parameterCount, column);
    }

    double[] whitenedResponse = response.ToArray();
    Cholesky.ForwardSubstitute(lower, rowCount, whitenedResponse, 1, 0);

    return OrdinaryLeastSquares.Summarise(
        whitened, whitenedResponse, matrix,
        TotalSumOfSquares(lower, response, whitenedResponse, settings.WithIntercept),
        rowCount, parameterCount, settings);
}
```

  `TotalSumOfSquares` with a constant whitens a column of ones, takes `m = (L⁻¹y)·(L⁻¹1) / (L⁻¹1)·(L⁻¹1)`, whitens
  `y − m` and sums its squares; without one it is `(L⁻¹y)·(L⁻¹y)`.

### Task 4: Documents and sample

- [ ] `docs/reference/stats-regression/gls.md` and `gls/generalizedleastsquares{,-fit}.md`, covered in
  `docs/wiki-map.json`; the `sm.GLS` equivalence row replaced by a counterpart with its divergences and the
  vector-`sigma` mapping; the `statsmodels.md` row; a guide paragraph in `regression-inference.md`; CHANGELOG;
  `samples/Lodestar.Sample/GeneralizedLeastSquaresSample.cs` and its `Program.cs` call; the package description,
  `CLAUDE.md` and `README.md` lines that name ordinary and weighted least squares.

### Task 5: The full gate, in the background

- [ ] Every `tools/check_*.py`, pytest, markdownlint, build, format, `dotnet test` (36 assemblies), pack of 18,
  `check_nuspec_dependencies.py --require-all`, sample, snippets. One commit; pull request `Closes #771`,
  *Next release*, `enhancement`, `api`, `Lodestar.Stats.Regression`.
