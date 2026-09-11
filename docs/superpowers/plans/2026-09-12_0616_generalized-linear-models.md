# Generalized linear models implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fit a binomial/logit or Poisson/log GLM by IRLS and return the whole inference table, at
`statsmodels` 0.15.0 parity.

**Architecture:** IRLS scales each row by `sqrt(W)` and hands the result to the Householder QR
`Lodestar.Decomposition` already publishes — the same call `OrdinaryLeastSquares.Fit` makes. The
covariance comes out of `R^-1 R^-T`, which is the identity the OLS already computes, so Task 1
extracts that arithmetic into one internal helper both use rather than writing it twice.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3, `Lodestar.Decomposition` for the QR,
`Lodestar.Stats` for the normal tail. No external dependency: core tier.

**Spec:** [`docs/superpowers/specs/2026-09-11_0616_generalized-linear-models.md`](../specs/2026-09-11_0616_generalized-linear-models.md)

**Branch:** `feat/616-generalized-linear-models`

## Global Constraints

- Core tier: **no external dependency**, and `tools/check_nuspec_dependencies.py` enforces it.
- Both target frameworks, `net10.0;netstandard2.0`, one public API — no reduced surface.
- Warnings are errors. `AnalysisMode=All`, `SonarAnalyzer.CSharp` on every project.
- Comments: two lines inline, eight of XML prose, `long-comment:` and a reason past that.
- Oracle floats compare at `1e-9`; **p-values compare relatively**, through
  `StatsOracleAsserts` in `tests/Lodestar.Stats.Tests/Oracles/` — reused, never restated.
- Commit messages carry no `feat:`/`fix:` prefix.
- `docs/equivalence.md` rows land in the same commit as the function they describe.
- Convergence is the oracle's: `|D_i - D_{i+1}| <= atol + rtol * |D_{i+1}|`, `atol = rtol = 1e-8`,
  `maxiter = 100`, criterion = deviance.

---

### Task 1: One copy of the least-squares arithmetic

`OrdinaryLeastSquares` holds `Design`, `Solve`, `InvertUpper` and `StandardErrors` privately. The
GLM needs all four. Extracting them first means the duplication never exists — #645 had 61 lines
counted by SonarCloud for exactly this shape.

**Files:**

- Create: `src/Lodestar.Stats.Regression/Internal/LeastSquares.cs`
- Modify: `src/Lodestar.Stats.Regression/OrdinaryLeastSquares.cs` (delete the four private helpers,
  call the new ones)
- Test: `tests/Lodestar.Stats.Regression.Tests/OlsOracleTests.cs` (unchanged — that is the point)

**Interfaces:**

- Consumes: `QrDecomposition.Householder(double[], int, int)` from `Lodestar.Decomposition`.
- Produces: `LeastSquares.Design`, `LeastSquares.Solve`, `LeastSquares.InvertUpper`,
  `LeastSquares.StandardErrors`, with the signatures below.

- [ ] **Step 1: Run the OLS suite and record the count**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~Ols"
```

Expected: PASS. Write the number down; it must not change in this task.

- [ ] **Step 2: Create the helper with the four members moved verbatim**

```csharp
using Lodestar.Decomposition;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The least-squares arithmetic the OLS and the GLM both need.</summary>
/// <remarks>
/// Extracted rather than written twice: the GLM's IRLS solves a weighted least squares each
/// iteration, and a weighted solve is this one over rows scaled by the square root of the
/// weight (#616).
/// </remarks>
internal static class LeastSquares
{
    /// <summary>The design matrix, with an intercept column prepended when asked.</summary>
    public static double[] Design(
        ReadOnlySpan<double> design, int rowCount, int featureCount, bool withIntercept)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        var matrix = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * parameterCount;
            if (withIntercept)
            {
                matrix[at++] = 1.0;
            }

            for (int column = 0; column < featureCount; column++)
            {
                matrix[at + column] = design[(row * featureCount) + column];
            }
        }

        return matrix;
    }

    /// <summary>Least squares through a thin QR, reporting the inverse of R the covariance needs.</summary>
    public static double[] Solve(
        double[] matrix,
        int rowCount,
        int parameterCount,
        ReadOnlySpan<double> response,
        out double[] inverseUpper)
    {
        QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount, parameterCount);
        IReadOnlyList<double> q = qr.Q;

        var projected = new double[parameterCount];
        for (int column = 0; column < parameterCount; column++)
        {
            double total = 0.0;
            for (int row = 0; row < rowCount; row++)
            {
                total += q[(row * parameterCount) + column] * response[row];
            }

            projected[column] = total;
        }

        inverseUpper = InvertUpper(qr.R, parameterCount);
        var coefficients = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                total += inverseUpper[(i * parameterCount) + k] * projected[k];
            }

            coefficients[i] = total;
        }

        return coefficients;
    }

    /// <summary>The inverse of an upper-triangular matrix, by back substitution.</summary>
    public static double[] InvertUpper(IReadOnlyList<double> upper, int order)
    {
        var inverse = new double[order * order];
        for (int column = order - 1; column >= 0; column--)
        {
            inverse[(column * order) + column] = 1.0 / upper[(column * order) + column];
            for (int row = column - 1; row >= 0; row--)
            {
                double total = 0.0;
                for (int k = row + 1; k <= column; k++)
                {
                    total += upper[(row * order) + k] * inverse[(k * order) + column];
                }

                inverse[(row * order) + column] = -total / upper[(row * order) + row];
            }
        }

        return inverse;
    }

    /// <summary>The diagonal of the covariance, scaled, square-rooted.</summary>
    public static double[] StandardErrors(
        double[] inverseUpper, int parameterCount, double dispersion)
    {
        var errors = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                double value = inverseUpper[(i * parameterCount) + k];
                total += value * value;
            }

            errors[i] = Math.Sqrt(total * dispersion);
        }

        return errors;
    }
}
```

**Copy the four bodies from `OrdinaryLeastSquares.cs` rather than from this block if they differ** —
the block above is the current shape, and the task is a move, not a rewrite. If a body differs, the
file wins and this plan is wrong.

- [ ] **Step 3: Delete the four private helpers from `OrdinaryLeastSquares` and call the new ones**

Add `using Lodestar.Stats.Regression.Internal;` at the top, then replace each call site:
`Design(...)` becomes `LeastSquares.Design(...)`, and so on for `Solve`, `InvertUpper`,
`StandardErrors`.

- [ ] **Step 4: Run the OLS suite again**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~Ols"
```

Expected: PASS, **with the same count as Step 1**. A move that changes a number is not a move.

- [ ] **Step 5: Commit**

```bash
git add src/Lodestar.Stats.Regression/
git commit -m "Hold the least-squares arithmetic in one place, before a second caller needs it"
```

---

### Task 2: The family, and what it computes

**Files:**

- Create: `src/Lodestar.Stats.Regression/GlmFamily.cs`
- Create: `src/Lodestar.Stats.Regression/Internal/Families.cs`
- Test: `tests/Lodestar.Stats.Regression.Tests/GlmFamilyTests.cs`

**Interfaces:**

- Produces: `public enum GlmFamily { Binomial, Poisson }`; `Families.InverseLink(GlmFamily, double)`,
  `Families.LinkDerivative(GlmFamily, double)`, `Families.Variance(GlmFamily, double)`,
  `Families.UnitDeviance(GlmFamily, double y, double mu)`.

- [ ] **Step 1: Write the failing test**

```csharp
using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class GlmFamilyTests
{
    [Fact]
    public void Logit_and_its_inverse_are_inverses()
    {
        double mu = Families.InverseLink(GlmFamily.Binomial, 0.75);

        // logit(mu) = log(mu / (1 - mu)), so the inverse of 0.75 back through the link is 0.75.
        Assert.Equal(0.75, Math.Log(mu / (1.0 - mu)), 12);
    }

    [Fact]
    public void The_log_link_inverts_to_exp()
    {
        Assert.Equal(Math.Exp(1.5), Families.InverseLink(GlmFamily.Poisson, 1.5), 12);
    }

    [Fact]
    public void A_perfect_binomial_prediction_has_zero_unit_deviance()
    {
        Assert.Equal(0.0, Families.UnitDeviance(GlmFamily.Binomial, 1.0, 1.0), 12);
        Assert.Equal(0.0, Families.UnitDeviance(GlmFamily.Binomial, 0.0, 0.0), 12);
    }

    [Fact]
    public void A_perfect_poisson_prediction_has_zero_unit_deviance()
    {
        Assert.Equal(0.0, Families.UnitDeviance(GlmFamily.Poisson, 3.0, 3.0), 12);
        Assert.Equal(0.0, Families.UnitDeviance(GlmFamily.Poisson, 0.0, 0.0), 12);
    }

    [Fact]
    public void An_undeclared_family_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Families.Variance((GlmFamily)7, 0.5));
    }
}
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmFamilyTests"
```

Expected: FAIL — `GlmFamily` and `Families` do not exist.

- [ ] **Step 3: Write the enum**

```csharp
namespace Lodestar.Stats.Regression;

/// <summary>The response distribution a <see cref="GeneralizedLinearModel"/> fits, with its canonical link.</summary>
/// <remarks>
/// Closed on purpose. IRLS cannot check that a caller-supplied family is internally consistent,
/// and an incoherent one produces a plausible inference table rather than an error — so a family
/// is chosen from this set rather than described by an interface (#616). Adding a member is not a
/// breaking change, which is what the follow-up families rely on.
/// </remarks>
public enum GlmFamily
{
    /// <summary>A response in <c>{0, 1}</c>, through the logit link.</summary>
    Binomial = 0,

    /// <summary>A non-negative count, through the log link.</summary>
    Poisson = 1,
}
```

- [ ] **Step 4: Write the arithmetic**

```csharp
namespace Lodestar.Stats.Regression.Internal;

/// <summary>What each family contributes to IRLS: the link, its derivative, the variance and the deviance.</summary>
internal static class Families
{
    /// <summary>The mean a linear predictor maps to.</summary>
    public static double InverseLink(GlmFamily family, double eta) => family switch
    {
        GlmFamily.Binomial => 1.0 / (1.0 + Math.Exp(-eta)),
        GlmFamily.Poisson => Math.Exp(eta),
        _ => throw Undeclared(family),
    };

    /// <summary>The derivative of the link at a mean, which IRLS needs for the working response.</summary>
    public static double LinkDerivative(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => 1.0 / (mu * (1.0 - mu)),
        GlmFamily.Poisson => 1.0 / mu,
        _ => throw Undeclared(family),
    };

    /// <summary>The variance function.</summary>
    public static double Variance(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => mu * (1.0 - mu),
        GlmFamily.Poisson => mu,
        _ => throw Undeclared(family),
    };

    /// <summary>One observation's contribution to the deviance.</summary>
    /// <remarks>
    /// The <c>y == 0</c> and <c>y == 1</c> arms are not an optimisation: the general formula
    /// carries <c>y log(y / mu)</c>, which is <c>0 * -inf</c> at the boundary and NaN in
    /// floating point, where the limit is 0.
    /// </remarks>
    public static double UnitDeviance(GlmFamily family, double y, double mu) => family switch
    {
        GlmFamily.Binomial => 2.0 * (Xlogy(y, y / mu) + Xlogy(1.0 - y, (1.0 - y) / (1.0 - mu))),
        GlmFamily.Poisson => 2.0 * (Xlogy(y, y / mu) - (y - mu)),
        _ => throw Undeclared(family),
    };

    /// <summary><c>x log(y)</c>, and zero where <c>x</c> is, which is the limit rather than NaN.</summary>
    private static double Xlogy(double x, double y) => x == 0.0 ? 0.0 : x * Math.Log(y);

    private static ArgumentOutOfRangeException Undeclared(GlmFamily family) =>
        new(nameof(family), family, $"{family} is not a declared {nameof(GlmFamily)}.");
}
```

- [ ] **Step 5: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmFamilyTests"
```

Expected: PASS, 5 tests.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Stats.Regression/ tests/Lodestar.Stats.Regression.Tests/
git commit -m "Add the two families, with the boundary the general deviance formula cannot take"
```

---

### Task 3: The IRLS loop, and what it reports about itself

**Files:**

- Create: `src/Lodestar.Stats.Regression/GlmOptions.cs`
- Create: `src/Lodestar.Stats.Regression/Internal/Irls.cs`
- Test: `tests/Lodestar.Stats.Regression.Tests/IrlsTests.cs`

**Interfaces:**

- Consumes: `LeastSquares.Design`, `LeastSquares.Solve` (Task 1); `Families.*` (Task 2).
- Produces: `Irls.Fit(...)` returning `IrlsResult`, a record carrying
  `double[] Coefficients`, `double[] InverseUpper`, `double[] Mean`, `double Deviance`,
  `bool Converged`, `int Iterations`, `double DevianceChange`.

- [ ] **Step 1: Write the failing test**

```csharp
using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class IrlsTests
{
    /// <summary>A separable design: every y = 1 sits above the threshold and every y = 0 below.</summary>
    private static readonly double[] SeparableDesign = [-2.0, -1.0, 1.0, 2.0];
    private static readonly double[] SeparableResponse = [0.0, 0.0, 1.0, 1.0];

    [Fact]
    public void A_converged_fit_reports_the_iterations_it_took()
    {
        double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
        double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

        IrlsResult result = Irls.Fit(
            design, response, featureCount: 1, GlmFamily.Binomial, new GlmOptions());

        Assert.True(result.Converged);
        Assert.InRange(result.Iterations, 1, 100);
        Assert.True(result.DevianceChange >= 0.0);
    }

    [Fact]
    public void Perfect_separation_does_not_converge()
    {
        IrlsResult result = Irls.Fit(
            SeparableDesign, SeparableResponse, featureCount: 1, GlmFamily.Binomial,
            new GlmOptions { MaximumIterations = 25 });

        Assert.False(result.Converged);
        Assert.Equal(25, result.Iterations);
    }
}
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~IrlsTests"
```

Expected: FAIL — `Irls`, `IrlsResult` and `GlmOptions` do not exist.

- [ ] **Step 3: Write the options**

```csharp
namespace Lodestar.Stats.Regression;

/// <summary>What a <see cref="GeneralizedLinearModel"/> fit may be told.</summary>
public sealed class GlmOptions
{
    /// <summary>Whether a column of ones is prepended to the design. Default true.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    public double ConfidenceLevel { get; init; } = 0.95;

    /// <summary>How many IRLS iterations are allowed. Default 100, which is the reference's.</summary>
    public int MaximumIterations { get; init; } = 100;

    /// <summary>The tolerance, used as both the absolute and relative term. Default 1e-8.</summary>
    public double Tolerance { get; init; } = 1e-8;

    /// <summary>Whether a fit that did not converge throws instead of returning. Default true.</summary>
    /// <remarks>
    /// A non-converged inference table is plausible and wrong — enormous standard errors and
    /// p-values that read like p-values. Turn this off to inspect one, or to freeze one in a
    /// corpus, and read <see cref="GlmSummary.Converged"/> before anything else (#616).
    /// </remarks>
    public bool ThrowOnNonConvergence { get; init; } = true;
}
```

- [ ] **Step 4: Write the loop**

```csharp
namespace Lodestar.Stats.Regression.Internal;

/// <summary>One IRLS fit: the coefficients, and everything the summary is computed from.</summary>
internal sealed record IrlsResult(
    double[] Coefficients,
    double[] InverseUpper,
    double[] Mean,
    double Deviance,
    bool Converged,
    int Iterations,
    double DevianceChange);

/// <summary>Iteratively reweighted least squares, over the QR the OLS already uses.</summary>
internal static class Irls
{
    public static IrlsResult Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        GlmFamily family,
        GlmOptions options)
    {
        int rowCount = response.Length;
        int parameterCount = featureCount + (options.WithIntercept ? 1 : 0);
        double[] matrix = LeastSquares.Design(design, rowCount, featureCount, options.WithIntercept);

        // The reference's start: mu from the response nudged off the boundary, which is what
        // keeps the first link evaluation finite for a binomial 0 or 1.
        var mean = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            mean[row] = family == GlmFamily.Binomial
                ? (response[row] + 0.5) / 2.0
                : response[row] + 0.1;
        }

        var scaled = new double[rowCount * parameterCount];
        var working = new double[rowCount];
        double[] coefficients = new double[parameterCount];
        double[] inverseUpper = new double[parameterCount * parameterCount];
        double deviance = Deviance(family, response, mean);
        double change = double.PositiveInfinity;
        bool converged = false;
        int iteration = 0;

        while (iteration < options.MaximumIterations)
        {
            iteration++;
            for (int row = 0; row < rowCount; row++)
            {
                double mu = mean[row];
                double derivative = Families.LinkDerivative(family, mu);
                double weight = 1.0 / (Families.Variance(family, mu) * derivative * derivative);
                double root = Math.Sqrt(weight);
                double eta = Link(family, mu);

                working[row] = root * (eta + ((response[row] - mu) * derivative));
                for (int column = 0; column < parameterCount; column++)
                {
                    int at = (row * parameterCount) + column;
                    scaled[at] = root * matrix[at];
                }
            }

            coefficients = LeastSquares.Solve(
                scaled, rowCount, parameterCount, working, out inverseUpper);

            for (int row = 0; row < rowCount; row++)
            {
                double eta = 0.0;
                for (int column = 0; column < parameterCount; column++)
                {
                    eta += matrix[(row * parameterCount) + column] * coefficients[column];
                }

                mean[row] = Families.InverseLink(family, eta);
            }

            double next = Deviance(family, response, mean);
            change = Math.Abs(deviance - next);
            // numpy.allclose, which is the reference's criterion: neither purely relative nor
            // purely absolute but their sum, with atol and rtol both the tolerance.
            converged = change <= options.Tolerance + (options.Tolerance * Math.Abs(next));
            deviance = next;
            if (converged)
            {
                break;
            }
        }

        return new IrlsResult(
            coefficients, inverseUpper, mean, deviance, converged, iteration, change);
    }

    public static double Deviance(GlmFamily family, ReadOnlySpan<double> response, double[] mean)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            total += Families.UnitDeviance(family, response[row], mean[row]);
        }

        return total;
    }

    private static double Link(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => Math.Log(mu / (1.0 - mu)),
        GlmFamily.Poisson => Math.Log(mu),
        _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
    };
}
```

- [ ] **Step 5: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~IrlsTests"
```

Expected: PASS, 2 tests.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Stats.Regression/ tests/Lodestar.Stats.Regression.Tests/
git commit -m "Add the IRLS loop, stopping on the criterion the reference stops on"
```

---

### Task 4: The summary, the refusals, and the entry point

**Files:**

- Create: `src/Lodestar.Stats.Regression/GlmSummary.cs`
- Create: `src/Lodestar.Stats.Regression/GeneralizedLinearModel.cs`
- Test: `tests/Lodestar.Stats.Regression.Tests/GlmEdgeTests.cs`

**Interfaces:**

- Consumes: `Irls.Fit` and `IrlsResult` (Task 3); `LeastSquares.StandardErrors` (Task 1);
  `Distributions.NormalQuantile` from `Lodestar.Stats`.
- Produces: `GeneralizedLinearModel.Fit(design, response, featureCount, family, options)` →
  `GlmSummary`, with the members the spec lists.

- [ ] **Step 1: Write the failing test**

```csharp
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class GlmEdgeTests
{
    private static readonly double[] Design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
    private static readonly double[] Response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

    [Fact]
    public void A_design_and_response_of_mismatched_length_are_refused()
    {
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0], featureCount: 1, GlmFamily.Binomial));
    }

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GeneralizedLinearModel.Fit(
            Design, Response, featureCount: 0, GlmFamily.Binomial));
    }

    [Fact]
    public void A_binomial_response_outside_zero_and_one_is_refused()
    {
        // statsmodels reads a proportion here and means a grouped fit by it. Accepting it
        // silently would answer a question the caller did not ask (#616).
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 0.5, 1.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.Binomial));
    }

    [Fact]
    public void A_negative_poisson_response_is_refused()
    {
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, -1.0, 2.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.Poisson));
    }

    [Fact]
    public void Perfect_separation_throws_by_default()
    {
        Assert.Throws<InvalidOperationException>(() => GeneralizedLinearModel.Fit(
            [-2.0, -1.0, 1.0, 2.0], [0.0, 0.0, 1.0, 1.0],
            featureCount: 1, GlmFamily.Binomial));
    }

    [Fact]
    public void Perfect_separation_returns_when_the_throw_is_turned_off()
    {
        GlmSummary summary = GeneralizedLinearModel.Fit(
            [-2.0, -1.0, 1.0, 2.0], [0.0, 0.0, 1.0, 1.0],
            featureCount: 1, GlmFamily.Binomial,
            new GlmOptions { ThrowOnNonConvergence = false });

        Assert.False(summary.Converged);
    }
}
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmEdgeTests"
```

Expected: FAIL — `GeneralizedLinearModel` and `GlmSummary` do not exist.

- [ ] **Step 3: Write the summary**

```csharp
namespace Lodestar.Stats.Regression;

/// <summary>What a generalized linear fit reports, at <c>statsmodels</c> parity.</summary>
public sealed class GlmSummary
{
    internal GlmSummary(
        double[] coefficients, double[] standardErrors, double[] zStatistics, double[] pValues,
        double[] lower, double[] upper, double deviance, double nullDeviance, double dispersion,
        double logLikelihood, double akaike, int residualDegreesOfFreedom, bool hasIntercept,
        bool converged, int iterations, double devianceChange)
    {
        Coefficients = coefficients;
        StandardErrors = standardErrors;
        ZStatistics = zStatistics;
        PValues = pValues;
        ConfidenceLower = lower;
        ConfidenceUpper = upper;
        Deviance = deviance;
        NullDeviance = nullDeviance;
        Dispersion = dispersion;
        LogLikelihood = logLikelihood;
        Akaike = akaike;
        ResidualDegreesOfFreedom = residualDegreesOfFreedom;
        HasIntercept = hasIntercept;
        Converged = converged;
        Iterations = iterations;
        DevianceChange = devianceChange;
    }

    /// <summary>One per parameter, the intercept first when there is one.</summary>
    public IReadOnlyList<double> Coefficients { get; }

    /// <summary>The square root of the covariance's diagonal.</summary>
    public IReadOnlyList<double> StandardErrors { get; }

    /// <summary>The Wald statistic, a coefficient over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; }

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; }

    /// <summary>The lower end of each interval, at the level asked for.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; }

    /// <summary>The upper end of each interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; }

    /// <summary>Twice the log-likelihood gap to a saturated fit.</summary>
    public double Deviance { get; }

    /// <summary>The same, for the intercept-only fit.</summary>
    public double NullDeviance { get; }

    /// <summary>Fixed at 1 for both families here; estimated when a Gamma family lands.</summary>
    public double Dispersion { get; }

    /// <summary>The fitted log-likelihood.</summary>
    public double LogLikelihood { get; }

    /// <summary>Akaike's criterion, <c>2k - 2 logL</c>.</summary>
    public double Akaike { get; }

    /// <summary>Rows less parameters.</summary>
    public int ResidualDegreesOfFreedom { get; }

    /// <summary>Whether a column of ones was fitted.</summary>
    public bool HasIntercept { get; }

    /// <summary>Whether IRLS reached the tolerance. <strong>Read this first.</strong></summary>
    public bool Converged { get; }

    /// <summary>How many iterations it took, or the budget when it did not converge.</summary>
    public int Iterations { get; }

    /// <summary>The absolute deviance change at the last iteration.</summary>
    public double DevianceChange { get; }
}
```

- [ ] **Step 4: Write the entry point**

```csharp
using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>A generalized linear model, fitted by IRLS, with the whole inference table.</summary>
/// <remarks>
/// Reference behavior: <c>statsmodels</c> 0.15.0's <c>GLM(...).fit()</c>. The response is a
/// count or a 0/1 outcome; for a real-valued one, <see cref="OrdinaryLeastSquares"/> is the
/// same table without a link.
/// </remarks>
public static class GeneralizedLinearModel
{
    /// <summary>Fits one model and reports its inference table.</summary>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row.</param>
    /// <param name="response">One value per row: 0 or 1 for binomial, a count for Poisson.</param>
    /// <param name="featureCount">How many regressors a row carries.</param>
    /// <param name="family">The response distribution, with its canonical link.</param>
    /// <param name="options">The fit's settings, or null for the defaults.</param>
    /// <exception cref="ArgumentException">The lengths disagree, or a response is outside its family.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is below one.</exception>
    /// <exception cref="InvalidOperationException">IRLS did not converge and the option says throw.</exception>
    public static GlmSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        GlmFamily family,
        GlmOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        GlmOptions settings = options ?? new GlmOptions();
        int rowCount = Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        RefuseResponseOutsideTheFamily(family, response);

        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows and {parameterCount} parameters leave no residual degree of "
                + "freedom, so no standard error exists.", nameof(response));
        }

        IrlsResult fit = Irls.Fit(design, response, featureCount, family, settings);
        if (!fit.Converged && settings.ThrowOnNonConvergence)
        {
            throw new InvalidOperationException(
                $"IRLS reached {fit.Iterations} iterations with a deviance change of "
                + $"{fit.DevianceChange:G3}, above the {settings.Tolerance:G3} tolerance. The "
                + $"fit is not usable; set {nameof(GlmOptions.ThrowOnNonConvergence)} to false "
                + "to inspect it.");
        }

        const double dispersion = 1.0;
        double[] errors = LeastSquares.StandardErrors(fit.InverseUpper, parameterCount, dispersion);
        var z = new double[parameterCount];
        var p = new double[parameterCount];
        var lower = new double[parameterCount];
        var upper = new double[parameterCount];
        double multiplier = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));

        for (int j = 0; j < parameterCount; j++)
        {
            z[j] = fit.Coefficients[j] / errors[j];
            // The two-sided normal tail, through the chi-square with one degree of freedom:
            // z^2 is chi-square(1) distributed, so this is the same number. Lodestar.Stats
            // publishes NormalQuantile and ChiSquaredSf and no normal CDF, and one member
            // that already exists beats a second that would have to be published.
            p[j] = Distributions.ChiSquaredSf(z[j] * z[j], 1.0);
            lower[j] = fit.Coefficients[j] - (multiplier * errors[j]);
            upper[j] = fit.Coefficients[j] + (multiplier * errors[j]);
        }

        double nullDeviance = NullDeviance(family, response, settings);
        double logLikelihood = LogLikelihood.Of(family, response, fit.Mean);
        double akaike = (2.0 * parameterCount) - (2.0 * logLikelihood);

        return new GlmSummary(
            fit.Coefficients, errors, z, p, lower, upper, fit.Deviance, nullDeviance, dispersion,
            logLikelihood, akaike, residualDegreesOfFreedom, settings.WithIntercept,
            fit.Converged, fit.Iterations, fit.DevianceChange);
    }

    /// <summary>The deviance of the intercept-only fit, which is the mean response everywhere.</summary>
    private static double NullDeviance(
        GlmFamily family, ReadOnlySpan<double> response, GlmOptions settings)
    {
        if (!settings.WithIntercept)
        {
            // Without an intercept the null model is the link's zero, not the mean.
            var atZero = new double[response.Length];
            for (int row = 0; row < atZero.Length; row++)
            {
                atZero[row] = Families.InverseLink(family, 0.0);
            }

            return Irls.Deviance(family, response, atZero);
        }

        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            total += response[row];
        }

        double mean = total / response.Length;
        var constant = new double[response.Length];
        for (int row = 0; row < constant.Length; row++)
        {
            constant[row] = mean;
        }

        return Irls.Deviance(family, response, constant);
    }

    private static void RefuseResponseOutsideTheFamily(
        GlmFamily family, ReadOnlySpan<double> response)
    {
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            bool ok = family switch
            {
                GlmFamily.Binomial => y is 0.0 or 1.0,
                GlmFamily.Poisson => y >= 0.0,
                _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
            };

            if (!ok)
            {
                throw new ArgumentException(
                    $"row {row} carries {y}, which {family} cannot fit: binomial takes 0 or 1 "
                    + "and Poisson a non-negative count.", nameof(response));
            }
        }
    }

    private static int Rows(
        ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount)
    {
        if (design.Length != response.Length * featureCount)
        {
            throw new ArgumentException(
                $"{design.Length} design values and {response.Length} responses do not describe "
                + $"rows of {featureCount}.", nameof(design));
        }

        return response.Length;
    }
}
```

- [ ] **Step 5: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmEdgeTests"
```

Expected: PASS, 6 tests.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Stats.Regression/ tests/Lodestar.Stats.Regression.Tests/
git commit -m "Add the entry point and its table, refusing a response its family cannot fit"
```

---

### Task 5: The log-likelihood, and the `LogGamma` decision it forces

**Files:**

- Create: `src/Lodestar.Stats.Regression/Internal/LogLikelihood.cs`
- Modify: `src/Lodestar.Stats/Distributions.cs` **only if** the measurement below says so
- Test: `tests/Lodestar.Stats.Regression.Tests/LogLikelihoodTests.cs`

**Interfaces:**

- Produces: `LogLikelihood.Of(GlmFamily, ReadOnlySpan<double> response, double[] mean)` → `double`.

- [ ] **Step 1: Establish whether `LogGamma` has to be published**

```bash
grep -rn 'LogGamma' src/Lodestar.Stats/Distributions.cs src/Lodestar.Stats/Internal/Gamma.cs
grep -n 'InternalsVisibleTo' src/Lodestar.Stats/Lodestar.Stats.csproj
```

Expected: `Gamma.LogGamma` is `internal`, and `InternalsVisibleTo` names only the two test
assemblies. **Then publishing it is required**, under decision 0095's own rule — it lists
`LogGamma` among the members that stay internal because nothing had asked, and Poisson's AIC now
asks. If the grep shows otherwise, the file wins and this step is the decision point.

- [ ] **Step 2: Write the failing test**

```csharp
using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class LogLikelihoodTests
{
    [Fact]
    public void A_binomial_log_likelihood_is_the_sum_of_log_probabilities()
    {
        double[] mean = [0.25, 0.75];
        double[] response = [0.0, 1.0];

        // log(1 - 0.25) + log(0.75)
        Assert.Equal(
            Math.Log(0.75) + Math.Log(0.75),
            LogLikelihood.Of(GlmFamily.Binomial, response, mean), 12);
    }

    [Fact]
    public void A_poisson_log_likelihood_carries_the_factorial_term()
    {
        double[] mean = [2.0];
        double[] response = [3.0];

        // 3 log 2 - 2 - log(3!) = 3 log 2 - 2 - log 6
        Assert.Equal(
            (3.0 * Math.Log(2.0)) - 2.0 - Math.Log(6.0),
            LogLikelihood.Of(GlmFamily.Poisson, response, mean), 12);
    }
}
```

- [ ] **Step 3: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~LogLikelihoodTests"
```

Expected: FAIL — `LogLikelihood` does not exist.

- [ ] **Step 4: Publish `LogGamma`, if Step 1 said so**

In `src/Lodestar.Stats/Distributions.cs`:

```csharp
    /// <summary>The natural log of the gamma function.</summary>
    /// <param name="x">A positive argument.</param>
    /// <returns><c>log Γ(x)</c>.</returns>
    /// <remarks>
    /// Published for <c>Lodestar.Stats.Regression</c>'s Poisson log-likelihood, whose AIC needs
    /// <c>log Γ(y + 1)</c> and therefore this member (#616). Decision 0095 listed it among the
    /// internals that stay so while nothing had asked; 0081's asymmetry is why asking is what
    /// changes it.
    /// </remarks>
    public static double LogGamma(double x) => Internal.Gamma.LogGamma(x);
```

Then bump `src/Lodestar.Stats/Version.props` by a minor, add a `CHANGELOG.md` entry under
`### Lodestar.Stats` → `#### Added`, and add a reference page under `docs/reference/stats/tails/`
beside the four 0095 published — the reference gate fails without it.

- [ ] **Step 5: Write the log-likelihood**

```csharp
namespace Lodestar.Stats.Regression.Internal;

/// <summary>The fitted log-likelihood, which is what the AIC is computed from.</summary>
/// <remarks>
/// These are the reference's formulae rather than a rearrangement: an AIC compared across
/// libraries is only comparable if the constant terms agree, and the Poisson's <c>log(y!)</c>
/// is exactly such a term.
/// </remarks>
internal static class LogLikelihood
{
    public static double Of(GlmFamily family, ReadOnlySpan<double> response, double[] mean)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            double mu = mean[row];
            total += family switch
            {
                GlmFamily.Binomial => (y * Math.Log(mu)) + ((1.0 - y) * Math.Log(1.0 - mu)),
                GlmFamily.Poisson => (y * Math.Log(mu)) - mu - Distributions.LogGamma(y + 1.0),
                _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
            };
        }

        return total;
    }
}
```

- [ ] **Step 6: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~LogLikelihoodTests"
```

Expected: PASS, 2 tests.

- [ ] **Step 7: Commit**

```bash
git add src/ tests/ docs/ CHANGELOG.md
git commit -m "Add the log-likelihood, and publish the one member its Poisson arm needs"
```

---

### Task 6: The corpora

**Files:**

- Modify: `tools/generate_oracles.py`
- Create: `tests/oracles/stats_glm.json` (generated, committed)

**Interfaces:**

- Produces: a corpus with `metadata`, and `binomial` and `poisson` blocks, each holding `cases`
  with `design`, `response`, `featureCount`, `withIntercept`, `confidenceLevel` and every field of
  `GlmSummary`.

- [ ] **Step 1: Add the generator function**

In `tools/generate_oracles.py`, beside the other `Lodestar.Stats.Regression` generator:

```python
def _glm_fixtures() -> list[dict]:
    """Designs chosen for what a link can get wrong, not for what a solve can.

    Hand-written rather than drawn: `_ols_fixtures` beside this is the idiom, and a
    generator sharing the module's random stream makes an unrelated corpus move when a
    case is added here.
    """
    return [
        {
            "name": "logistic, one regressor, intercept fitted",
            "family": "binomial",
            DESIGN: [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0],
            RESPONSE: [0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 0.0, 1.0, 1.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Two regressors at 99%, so the multiplier is visibly not 1.96 and an
            # interval that used the wrong one fails on its own rather than on the
            # coefficient it wraps.
            "name": "logistic, two regressors, 99%",
            "family": "binomial",
            DESIGN: [
                1.0, 0.5, 2.0, 1.5, 3.0, 0.5, 4.0, 2.5, 5.0, 1.0,
                6.0, 3.5, 7.0, 2.0, 8.0, 4.5, 9.0, 3.0, 10.0, 5.5,
            ],
            RESPONSE: [0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 1.0, 1.0, 1.0],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            # No intercept: the null deviance is the link's zero rather than the mean,
            # which is the arm of NullDeviance nothing else reaches.
            "name": "logistic, no intercept",
            "family": "binomial",
            DESIGN: [-2.0, -1.5, -0.5, 0.5, 1.0, 1.5, 2.0, 2.5],
            RESPONSE: [0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 1.0, 1.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "poisson, one regressor, intercept fitted",
            "family": "poisson",
            DESIGN: [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0],
            RESPONSE: [1.0, 0.0, 2.0, 3.0, 4.0, 3.0, 7.0, 6.0, 9.0, 11.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # A zero response, which is where the deviance's x-log-y term is 0 * -inf and
            # NaN in floating point if the limit is not taken.
            "name": "poisson, zeros in the response",
            "family": "poisson",
            DESIGN: [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5],
            RESPONSE: [0.0, 0.0, 1.0, 0.0, 2.0, 1.0, 3.0, 4.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "poisson, two regressors",
            "family": "poisson",
            DESIGN: [
                1.0, 0.5, 2.0, 1.0, 3.0, 1.5, 4.0, 2.0, 5.0, 2.5,
                6.0, 3.0, 7.0, 3.5, 8.0, 4.0, 9.0, 4.5, 10.0, 5.0,
            ],
            RESPONSE: [1.0, 2.0, 2.0, 4.0, 5.0, 7.0, 8.0, 12.0, 15.0, 20.0],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
    ]


def generate_stats_glm() -> dict:
    """statsmodels' GLM, one block per family (#616).

    Separate blocks rather than one flat list, the shape #645 settled for two MinHash
    schemes: a family added later grows the file instead of rewriting it.
    """
    import numpy as np
    import statsmodels.api as sm

    families = {"binomial": sm.families.Binomial(), "poisson": sm.families.Poisson()}
    blocks: dict = {name: {"cases": []} for name in families}
    for fixture in _glm_fixtures():
        feature_count = fixture[OLS_FEATURE_COUNT]
        design = np.array(fixture[DESIGN]).reshape(-1, feature_count)
        response = np.array(fixture[RESPONSE])
        exog = sm.add_constant(design, prepend=True) if fixture[WITH_INTERCEPT] else design
        fit = sm.GLM(response, exog, family=families[fixture["family"]]).fit()
        interval = fit.conf_int(alpha=1.0 - fixture[CONFIDENCE_LEVEL])
        blocks[fixture["family"]]["cases"].append({
            "name": fixture["name"],
            DESIGN: [float(v) for v in fixture[DESIGN]],
            RESPONSE: [float(v) for v in fixture[RESPONSE]],
            OLS_FEATURE_COUNT: feature_count,
            WITH_INTERCEPT: fixture[WITH_INTERCEPT],
            CONFIDENCE_LEVEL: fixture[CONFIDENCE_LEVEL],
            "coefficients": [float(v) for v in fit.params],
            "standardErrors": [float(v) for v in fit.bse],
            "zStatistics": [float(v) for v in fit.tvalues],
            "pValues": [float(v) for v in fit.pvalues],
            "confidenceLower": [float(v) for v in interval[:, 0]],
            "confidenceUpper": [float(v) for v in interval[:, 1]],
            "deviance": float(fit.deviance),
            "nullDeviance": float(fit.null_deviance),
            "dispersion": float(fit.scale),
            "logLikelihood": float(fit.llf),
            "akaike": float(fit.aic),
            "residualDegreesOfFreedom": int(fit.df_resid),
            "converged": bool(fit.converged),
            "iterations": int(fit.fit_history["iteration"]),
        })

    # One separable design, which does not converge. Frozen like any other case so the
    # non-converged branch is replayed rather than asserted by hand.
    separable_x = np.array([[-2.0], [-1.0], [1.0], [2.0]])
    separable_y = np.array([0.0, 0.0, 1.0, 1.0])
    separable = sm.GLM(
        separable_y, sm.add_constant(separable_x), family=sm.families.Binomial()
    ).fit(maxiter=25)
    blocks["separable"] = {
        DESIGN: [float(v) for v in separable_x.ravel()],
        RESPONSE: [float(v) for v in separable_y],
        OLS_FEATURE_COUNT: 1,
        "maximumIterations": 25,
        "converged": bool(separable.converged),
        "iterations": int(separable.fit_history["iteration"]),
    }

    return {
        "metadata": {
            "library": "statsmodels",
            "version": version("statsmodels"),
            FAMILY: "glm",
            VARIANT: "GLM(family=Binomial|Poisson).fit(), IRLS, canonical links",
            "count": sum(len(b["cases"]) for b in blocks.values() if "cases" in b),
        },
        **blocks,
    }
```

Register it in the dispatch table beside `"stats_ols.json": generate_stats_ols`:

```python
        "stats_glm.json": generate_stats_glm,
```

`DESIGN`, `RESPONSE`, `OLS_FEATURE_COUNT`, `WITH_INTERCEPT`, `CONFIDENCE_LEVEL`, `FAMILY`,
`VARIANT` and `version` are module-level names the OLS generator beside this one already uses —
reuse them rather than spelling the strings again, which is what `check_repeated_literals.py`
stops at the third occurrence.

- [ ] **Step 2: Generate, from a neutral directory**

```bash
repo=$(git rev-parse --show-toplevel)
cd /var/tmp && PYTHONSAFEPATH=1 "$repo/.venv-oracles/bin/python" "$repo/tools/generate_oracles.py"
echo "exit=$?"
```

`/var/tmp` rather than `/tmp`: the working directory must not be an ancestor of the checkout, or
`nltk` refuses to import, and a hosted session puts the worktree under `/tmp` itself.

**Read the generator's own exit code, never a pipeline's.** Expected: 0, and
`tests/oracles/stats_glm.json` written.

- [ ] **Step 3: Check only the new corpus appeared**

```bash
git status --porcelain tests/oracles
```

Expected: one untracked `stats_glm.json` and nothing modified. A modified neighbour means the
generator's shared random stream moved and the change is wrong.

- [ ] **Step 4: Commit**

```bash
git add tools/generate_oracles.py tests/oracles/stats_glm.json
git commit -m "Freeze the GLM corpora, one block per family and one that does not converge"
```

---

### Task 7: Replay the corpora

**Files:**

- Create: `tests/Lodestar.Stats.Regression.Tests/GlmOracleTests.cs`
- Modify: `tests/Lodestar.Stats.Regression.Tests/Lodestar.Stats.Regression.Tests.csproj` (copy the
  new corpus to the output, the way `stats_ols.json` already is)

**Interfaces:**

- Consumes: `GeneralizedLinearModel.Fit` (Task 4); `OracleLoader.Load` already in the test project.

- [ ] **Step 1: Write the failing test**

```csharp
using System.Text.Json;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>Replays <c>statsmodels.GLM(...).fit()</c> over the frozen cases of <c>stats_glm.json</c>.</summary>
public sealed class GlmOracleTests
{
    private const double Absolute = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("stats_glm.json");

    private static IReadOnlyList<JsonElement> Cases(string block) =>
        [.. Corpus.RootElement.GetProperty(block).GetProperty("cases").EnumerateArray()];

    public static TheoryData<string, int> Indices()
    {
        var data = new TheoryData<string, int>();
        foreach (string block in new[] { "binomial", "poisson" })
        {
            for (int i = 0; i < Cases(block).Count; i++)
            {
                data.Add(block, i);
            }
        }

        return data;
    }

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    [Theory]
    [MemberData(nameof(Indices))]
    public void Every_case_matches_statsmodels(string block, int index)
    {
        JsonElement expected = Cases(block)[index];
        GlmFamily family = block == "binomial" ? GlmFamily.Binomial : GlmFamily.Poisson;

        GlmSummary actual = GeneralizedLinearModel.Fit(
            Doubles(expected, "design"),
            Doubles(expected, "response"),
            expected.GetProperty("featureCount").GetInt32(),
            family,
            new GlmOptions
            {
                WithIntercept = expected.GetProperty("withIntercept").GetBoolean(),
                ConfidenceLevel = expected.GetProperty("confidenceLevel").GetDouble(),
            });

        AssertVector(Doubles(expected, "coefficients"), actual.Coefficients);
        AssertVector(Doubles(expected, "standardErrors"), actual.StandardErrors);
        AssertVector(Doubles(expected, "zStatistics"), actual.ZStatistics);
        AssertVector(Doubles(expected, "confidenceLower"), actual.ConfidenceLower);
        AssertVector(Doubles(expected, "confidenceUpper"), actual.ConfidenceUpper);
        AssertRelative(Doubles(expected, "pValues"), actual.PValues);

        Assert.Equal(expected.GetProperty("deviance").GetDouble(), actual.Deviance, Absolute);
        Assert.Equal(expected.GetProperty("nullDeviance").GetDouble(), actual.NullDeviance, Absolute);
        Assert.Equal(expected.GetProperty("dispersion").GetDouble(), actual.Dispersion, Absolute);
        Assert.Equal(expected.GetProperty("logLikelihood").GetDouble(), actual.LogLikelihood, Absolute);
        Assert.Equal(expected.GetProperty("akaike").GetDouble(), actual.Akaike, Absolute);
        Assert.Equal(
            expected.GetProperty("residualDegreesOfFreedom").GetInt32(),
            actual.ResidualDegreesOfFreedom);
        Assert.True(actual.Converged);
    }

    [Fact]
    public void The_separable_case_does_not_converge_and_says_how_far_it_got()
    {
        JsonElement block = Corpus.RootElement.GetProperty("separable");

        GlmSummary actual = GeneralizedLinearModel.Fit(
            Doubles(block, "design"),
            Doubles(block, "response"),
            block.GetProperty("featureCount").GetInt32(),
            GlmFamily.Binomial,
            new GlmOptions
            {
                MaximumIterations = block.GetProperty("maximumIterations").GetInt32(),
                ThrowOnNonConvergence = false,
            });

        Assert.Equal(block.GetProperty("converged").GetBoolean(), actual.Converged);
        Assert.Equal(block.GetProperty("iterations").GetInt32(), actual.Iterations);
    }

    private static void AssertVector(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i], Absolute);
        }
    }

    /// <summary>A p-value compares relatively, per decision 0081.</summary>
    private static void AssertRelative(double[] expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            double tolerance = Math.Max(Math.Abs(expected[i]), Math.Abs(actual[i])) * 1e-9;
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= tolerance,
                $"p-value {i}: expected {expected[i]:G17}, got {actual[i]:G17}");
        }
    }
}
```

**Before writing `AssertRelative`, check whether `StatsOracleAsserts` is reachable from this
project.** It lives in `tests/Lodestar.Stats.Tests/Oracles/` and decision 0081 names it as where
the rule lives. If it can be linked the way `tests/Shared/` files are, link it and delete the local
helper — the tolerance must not be spelled twice.

- [ ] **Step 2: Run and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmOracleTests"
```

Expected: FAIL — the corpus is not copied to the output directory yet.

- [ ] **Step 3: Copy the corpus to the output**

In `Lodestar.Stats.Regression.Tests.csproj`, beside the existing `stats_ols.json` entry:

```xml
    <None Include="../oracles/stats_glm.json" CopyToOutputDirectory="PreserveNewest"
          LinkBase="oracles" />
```

- [ ] **Step 4: Run until green, and read the count**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~GlmOracleTests"
```

Expected: PASS, 10 tests — nine cases across the two families plus the separable one. **Read the
count, not the colour**: a filter matching nothing exits 8 under this runner, but a corpus that
loaded empty would pass with zero theory rows.

- [ ] **Step 5: Commit**

```bash
git add tests/
git commit -m "Replay the GLM corpora, including the case that does not converge"
```

---

### Task 8: The placement measurement, and the decision it settles

**Files:**

- Create: `docs/decisions/0111-<slug>.md` — the slug states the verdict, per this repository's
  house style, and cannot be written before the measurement
- Modify: `docs/decisions/README.md` (the row and both spelled-out counts)
- Modify: `docs/decisions/index.yaml` (regenerated, never hand-written)

- [ ] **Step 1: Measure**

```bash
git diff --stat main...HEAD -- src/Lodestar.Stats.Regression/
grep -c 'public ' src/Lodestar.Stats.Regression/*.cs
git diff main...HEAD --name-only -- src/Lodestar.Stats/
```

Record three numbers: source lines added under `Lodestar.Stats.Regression`, public members added,
and whether `Lodestar.Stats` had to open a member.

- [ ] **Step 2: Take the decision against 0076's test**

Decision 0076 allows a split for a **distinct dependency profile, audience or cadence**. Write the
ADR stating which of the three the measurement found, or that it found none. A package that forced
a neighbour's surface open is one argument for being its own thing and one against — say which way
the measurement pointed rather than leaving it implied.

If the verdict is **its own package**, that is its own issue and its own branch: this one ships
inside `Lodestar.Stats.Regression` and the ADR records the move as owed, **before the version bump
in Task 9**, so nothing is published under a name it must leave.

- [ ] **Step 3: Regenerate the index and commit**

```bash
python3 tools/regen_adr_index.py
.venv-oracles/bin/python -m pytest tools/tests/test_adr_count_coherence.py -q
git add docs/decisions/
git commit -m "Record where the generalized linear model lives, on the measurement"
```

---

### Task 9: Documentation, packaging and the version

**Files:**

- Create: `docs/reference/stats/regression/generalizedlinearmodel-fit.md`,
  `glmfamily.md`, `glmoptions.md`, `glmsummary.md` (paths follow the existing
  `docs/reference/stats/regression/` layout — read one before writing)
- Modify: `docs/reference/stats/regression.md` (the index rows)
- Modify: `docs/equivalence.md` (one row per public member)
- Create: `samples/Lodestar.Sample/GeneralizedLinearModelSample.cs`
- Modify: `CHANGELOG.md`, `src/Lodestar.Stats.Regression/Version.props`
- Modify: `bench/README.md` (a section against `Accord.Statistics` 3.8.0)

- [ ] **Step 1: Write the reference pages, then let the gate find what is missing**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
```

Expected: FAIL first, naming each undocumented member; PASS once every page exists. **A `csharp`
fence in a reference page is compiled and executed, and a trailing `// =>` is an assertion** — so
every example must be true, not illustrative.

- [ ] **Step 2: Add the sample, which the packaging gate requires**

```csharp
using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>A logistic fit, and the table that makes it inference.</summary>
internal static class GeneralizedLinearModelSample
{
    public static void Run()
    {
        double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
        double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

        GlmSummary fit = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Binomial);

        Console.WriteLine("GeneralizedLinearModel (Lodestar.Stats.Regression)");
        Console.WriteLine($"  slope            : {fit.Coefficients[1].ToString("F4", Culture)}");
        Console.WriteLine($"  its p-value      : {fit.PValues[1].ToString("F4", Culture)}");
        Console.WriteLine($"  deviance         : {fit.Deviance.ToString("F4", Culture)}");
        Console.WriteLine($"  converged in     : {fit.Iterations}");
        Console.WriteLine();
    }

    private static System.Globalization.CultureInfo Culture =>
        System.Globalization.CultureInfo.InvariantCulture;
}
```

Register it in the sample's entry point beside its neighbours, and run
`python3 tools/check_sample_culture.py` — a number printed in the contributor's culture fails CI.

- [ ] **Step 3: Add the benchmark, against the library the licence refused**

`Accord.Statistics` 3.8.0 is LGPL-2.1, which decision 0003 bars from `src/` and **not** from
`bench/` — the precedent is decision 0096, which benchmarked the OLS table against it. A
comparison against the library we refused is worth more than one against nothing.

Create `bench/Lodestar.Stats.Benchmarks/GlmBenchmarks.cs` with a `[Benchmark(Baseline = true)]`
calling `GeneralizedLinearModel.Fit` and a `[Benchmark]` calling Accord's
`IterativeReweightedLeastSquares`, over `[Params(200, 2_000)]` rows and one and three regressors.
Add `Accord.Statistics` to `bench/Directory.Packages.props` — **not** to `src/`, and
`tools/check_nuspec_dependencies.py` is what fails if that is confused.

Then register the class, or the nightly selects it and measures nothing:

```bash
python3 tools/check_bench_map.py
```

Expected: FAIL, naming `GlmBenchmarks` as absent from `bench/bench-map.json`. Add it against
`src/Lodestar.Stats.Regression/**`, re-run until clean, and write the `bench/README.md` section
in the register of the ones beside it — how to measure, the command, what each row means, and the
machine named. **Numbers go in `docs/guides/performance.md`, never in `bench/README.md`**, which
documents how to measure rather than what was measured.

- [ ] **Step 4: Bump the version and write the changelog entry**

`src/Lodestar.Stats.Regression/Version.props`: `0.1.0` → `0.2.0`. New public surface, nothing
removed.

`CHANGELOG.md`, under `## [Unreleased]` → `### Lodestar.Stats.Regression` → `#### Added`, one
sentence with the issue.

- [ ] **Step 5: Run every gate**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
sh .githooks/pre-commit
python3 tools/check_repeated_literals.py --base origin/main
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

Expected: all clean, and `dotnet test` reporting **32 assemblies**. Then the packaging and
doc-snippet gates, which need a fresh pack:

```bash
for p in src/Lodestar.*/; do dotnet pack "$p" -c Release -o ./artifacts; done
python3 tools/extract_doc_snippets.py
dotnet build samples/Lodestar.DocSnippets -c Release
dotnet run --project samples/Lodestar.DocSnippets -c Release
```

- [ ] **Step 6: Commit and open the pull request**

```bash
git add -A
git commit -m "Document the generalized linear model, and ship it"
git push -u origin feat/616-generalized-linear-models
```

The pull request body carries `Closes #616`. **One pull request for this issue** — the spec, the
implementation, the corpora, the documentation and the placement decision all land in it.
