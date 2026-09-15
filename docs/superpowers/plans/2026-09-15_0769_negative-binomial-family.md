# Negative binomial family Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship `GlmFamily.NegativeBinomial` with a given `alpha`, at `statsmodels.GLM` 0.15.0 parity.

**Architecture:** The family is one more arm in `Internal/Families` and `Internal/LogLikelihood`, with `alpha`
threaded as an argument every family function takes and the two shipped families ignore. `GlmOptions` gains a
nullable `NegativeBinomialAlpha`; `GeneralizedLinearModel.Fit` resolves it (null → 1.0) and refuses it for the
other families. An internal `LogLikelihood.LogGamma` gives `lnΓ` at non-integer arguments.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `statsmodels` 0.15.0 and `scipy` as oracles.

**Spec:** [`docs/superpowers/specs/2026-09-15_0769_negative-binomial-family.md`](../specs/2026-09-15_0769_negative-binomial-family.md)

**Branch:** `feat/769-negative-binomial` — one pull request closes #769, spec, plan, code and documents, one commit.

## Global Constraints

- **Everything in English**; no `feat:`/`fix:` prefix; `Closes #769`.
- **Warnings are errors**, `AnalysisMode=All`; S1244 on exact float comparison is suppressed at the call site with its reason.
- **Comment rules:** two lines inline, eight of XML prose, `long-comment:` past that.
- **`$MAIN`** is the main checkout, `MAIN="$(cd "$(git rev-parse --git-common-dir)/.." && pwd -P)"`; every dotnet
  command goes through `"$MAIN/.dotnet-guarded"`, `LodestarUseProjectRefs` unset; foreground commands under 90 s,
  longer ones in the background.
- **The binomial and Poisson blocks of `stats_glm.json` regenerate unchanged**, and replay unchanged.
- **Test count:** 36 assemblies.

---

### Task 1: `LogGamma`, test first

**Files:**

- Modify: `tools/generate_oracles.py` (`generate_regression_log_gamma`, registered after `regression_log_factorial.json`)
- Create: `tests/oracles/regression_log_gamma.json`
- Modify: `src/Lodestar.Stats.Regression/Internal/LogLikelihood.cs`, `tests/Lodestar.Stats.Regression.Tests/LogLikelihoodTests.cs`, the test `.csproj`

- [ ] **Step 1: Corpus**

```python
def generate_regression_log_gamma() -> dict:
    """``scipy.special.gammaln(x)`` at non-integer x, the lnΓ(y + 1/alpha) the negative binomial reads (#769)."""
    import scipy
    from scipy.special import gammaln

    points = [1e-3, 0.01, 0.1, 0.5, 1.0, 1.5, 2.5, 10.0 / 3.0, 7.25, 19.5, 20.0, 20.5, 101.0 / 3.0,
              1000.5, 12345.678, 1e6 + 0.25]
    return {"metadata": {"library": "scipy", "version": scipy.__version__, FAMILY: "gammaln",
                         VARIANT: "gammaln(x)", "count": len(points)},
            "cases": [{"x": x, "logGamma": float(gammaln(x))} for x in points]}
```

- [ ] **Step 2: Failing test** — a `[Theory]` over the corpus asserting
  `Math.Abs(LogLikelihood.LogGamma(x) - expected) <= 1e-9 * Math.Max(1.0, Math.Abs(expected))`.

- [ ] **Step 3: Implement**

```csharp
internal static double LogGamma(double x)
{
    double shift = 0.0;
    while (x < 40.0)
    {
        shift += Math.Log(x);
        x += 1.0;
    }

    double inverse = 1.0 / x;
    double inverseSquared = inverse * inverse;
    double series = inverse * ((1.0 / 12.0) - (inverseSquared * ((1.0 / 360.0) - (inverseSquared / 1260.0))));
    return ((x - 0.5) * Math.Log(x)) - x + HalfLogTwoPi + series - shift;
}
```

- [ ] **Step 4:** run `tests/Lodestar.Stats.Regression.Tests` filtered to `LogLikelihood`; green.

### Task 2: The family

**Files:**

- Modify: `src/Lodestar.Stats.Regression/GlmFamily.cs`, `GlmOptions.cs`, `GeneralizedLinearModel.cs`,
  `Internal/Families.cs`, `Internal/Irls.cs`, `Internal/LogLikelihood.cs`

- [ ] **Step 1: Enum and option**

```csharp
/// <summary>A non-negative count with variance <c>μ + αμ²</c>, through the log link.</summary>
NegativeBinomial = 2,
```

```csharp
public double? NegativeBinomialAlpha
{
    get => _negativeBinomialAlpha;
    init
    {
        if (value is { } alpha && (double.IsNaN(alpha) || double.IsInfinity(alpha) || alpha <= 0.0))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "alpha is finite and above zero.");
        }

        _negativeBinomialAlpha = value;
    }
}
```

- [ ] **Step 2: Arms.** Every `Families` function and `LogLikelihood.Of` take `double alpha`:
  `InverseLink`/`LinkDerivative`/`Link` and `Clamp` as Poisson; `Variance` → `mu + (alpha * mu * mu)`;
  `UnitDeviance` → `2.0 * (Xlogy(y, Math.Max(y / mu, Epsilon)) - ((y + theta) * Math.Log((y + theta) / (mu + theta))))`
  with `theta = 1.0 / alpha`; log-likelihood →
  `(y * Math.Log(alpha * mu)) - ((y + theta) * Math.Log(1.0 + (alpha * mu))) + LogGamma(y + theta) - LogGamma(theta) - LogFactorial(y)`.
  The starting mean takes Poisson's arm.

- [ ] **Step 3: Fit.** `double alpha = ResolveAlpha(family, settings)`: `NegativeBinomialAlpha ?? 1.0` for the new
  family, `ArgumentException` on `options` when it is set for another; the response check treats the family as Poisson.

### Task 3: The corpus and the replay

- [ ] **Step 1:** in `_glm_fixtures`, negative binomial cases carrying `"alpha"`: the Poisson designs at
  `alpha` 1, 0.5 and 2, no intercept, zeros, 99%, and `1e-3`. `generate_stats_glm` builds
  `sm.families.NegativeBinomial(alpha=fixture["alpha"])` for them, writes `"alpha"` into the case and a
  `negativeBinomial` block. Regenerate `stats_glm.json` and confirm with `tools/compare_oracles.py` that the
  binomial and Poisson blocks did not move.
- [ ] **Step 2:** `GlmOracleTests.Indices` walks `negativeBinomial` too; the family and the options come from the
  block name and the case's `alpha`.
- [ ] **Step 3: Edge tests** — alpha zero, negative, `NaN`, infinite refused by the option; alpha set for Poisson
  refused on `options`; a null alpha equals an explicit 1.0 exactly; a fractional count refused; an `alpha` of
  `1e-8` lands within `1e-6` of the Poisson coefficients.

### Task 4: Documents and sample

- [ ] `docs/reference/stats-regression/glm/glmfamily.md` (member row), `glmoptions.md` (the property),
  `generalizedlinearmodel-fit.md` (exceptions); equivalence rows for the family and its divergences; the
  `statsmodels.md` GLM row; `CHANGELOG.md`; a `NegativeBinomial` use in `samples/Lodestar.Sample/GlmOptionsSample.cs`.
  Every `// =>` value comes from running the snippet.

### Task 5: The full gate

- [ ] Every `tools/check_*.py`, pytest, format, markdownlint, `dotnet test Lodestar.slnx -c Release` (36
  assemblies), pack of 18 with `check_nuspec_dependencies.py --require-all`, sample, snippets — in the background.
- [ ] One commit; pull request `Closes #769`, milestone *Next release*, labels `enhancement`, `api`, `Lodestar.Stats.Regression`.
