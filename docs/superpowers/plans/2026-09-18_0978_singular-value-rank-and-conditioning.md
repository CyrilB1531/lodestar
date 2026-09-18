# Singular-value rank refusal and conditioning gate — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** Measure the collinear refusal and the normal-equations gate on the singular values of
the `p × p` triangular factor each already claims to measure, so `Lodestar.Stats.Regression`
refuses what `numpy.linalg.matrix_rank` refuses and stops sending every wide design to the
reflections.

**Architecture:** One one-sided Jacobi sweep moves into `src/Shared/`, opted into by the three
libraries that need it, so it crosses the package boundary as source rather than as API. Both
gates keep a cheap bound on each side of their threshold and reach the sweep only when the two
bounds disagree, so nothing that is fast today gets slower.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform,
BenchmarkDotNet, oracle corpora replayed from statsmodels 0.15.0 / numpy 2.5.3.

**Spec:** [`docs/superpowers/specs/2026-09-18_0978_singular-value-rank-and-conditioning.md`](../specs/2026-09-18_0978_singular-value-rank-and-conditioning.md)

**Branch:** `fix/978-collinear-refusal-and-normal-equations-bound`

**Closes:** #978, #985

## Global Constraints

- Both target frameworks, one public API. No `#if` at a call site; `src/Shared/` carries the
  compat. Warnings are errors.
- **No public API is added, removed or changed.** No reference page gains an entry, no
  `Version.props` moves, no `docs/wiki-map.json` change.
- `src/` projects reference packages, not projects. Nothing in this plan needs
  `LodestarUseProjectRefs`, because every cross-package piece travels as shared source.
- Machine epsilon is the literal `2.220446049250313e-16`, already declared in each file that
  uses it. Do not add a third copy.
- Comments: why not what, two lines inline or eight in XML docs,
  `python3 tools/check_comment_length.py` counts them.
- Every Sonar finding is cleared before the commit, not after.
- One commit for the whole pull request; amend rather than stacking fix-ups.
- Attribution: commit messages end with `Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>`.

---

### Task 1: The shared Jacobi spectrum kernel

The sweep that `Lodestar.Decomposition.Internal.JacobiSvd.SingularValues` already runs moves to
`src/Shared/`, so `Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries` can run it without
an inter-package API and without a release. `JacobiSvd` then delegates, leaving one copy in the
tree.

**Files:**

- Create: `src/Shared/JacobiSpectrum.cs`
- Modify: `src/Directory.Build.props` (a new opt-in item group, after the `ElementWise` one at :52)
- Modify: `src/Lodestar.Decomposition/Internal/JacobiSvd.cs:89-133` (`SingularValues` and `RotateTrackedPair`)
- Modify: `src/Lodestar.Decomposition/Lodestar.Decomposition.csproj:14` (add the opt-in)
- Modify: `src/Lodestar.Stats.Regression/Lodestar.Stats.Regression.csproj:14` (add the opt-in and `ElementWise`)
- Modify: `src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj:14` (add the opt-in and `ElementWise`)
- Test: `tests/Lodestar.Stats.Regression.Tests/JacobiSpectrumTests.cs` (new)

**Interfaces:**

- Produces: `Lodestar.Internal.JacobiSpectrum.SingularValues(double[] columnMajor, int rows, int columns)`
  → `double[]`, largest first, `columns` long. It **overwrites** `columnMajor`, which must hold
  `rows * columns` values with `rows >= columns`, column by column.
- Consumes: `Lodestar.Internal.ElementWise.Rotate(Span<double>, Span<double>, double, double)`,
  already in `src/Shared/ElementWise.cs`.

- [ ] **Step 1: Write the failing test**

Create `tests/Lodestar.Stats.Regression.Tests/JacobiSpectrumTests.cs`:

```csharp
using Lodestar.Internal;

namespace Lodestar.Stats.Regression.Tests;

public sealed class JacobiSpectrumTests
{
    [Fact]
    public void SingularValues_OfADiagonalBlock_AreItsDiagonalSortedDescending()
    {
        // Column-major 3 x 3 with 2, 5, 1 on the diagonal.
        double[] block = [2, 0, 0, 0, 5, 0, 0, 0, 1];

        double[] values = JacobiSpectrum.SingularValues(block, 3, 3);

        Assert.Equal(5.0, values[0], 12);
        Assert.Equal(2.0, values[1], 12);
        Assert.Equal(1.0, values[2], 12);
    }

    [Fact]
    public void SingularValues_OfASingularTriangle_EndAtZero()
    {
        // Column-major upper triangular [[1, 2], [0, 0]]: rank 1.
        double[] block = [1, 0, 2, 0];

        double[] values = JacobiSpectrum.SingularValues(block, 2, 2);

        Assert.Equal(Math.Sqrt(5.0), values[0], 12);
        Assert.Equal(0.0, values[1], 12);
    }

    [Fact]
    public void SingularValues_MatchNumpy_OnTheIssue978Design()
    {
        // numpy.linalg.svd on [1, x1, x2, x1 - x2] with delta 1e-2, compute_uv=False.
        double[] expected = [2.0361e+01, 1.2732e+00, 4.4662e-02, 6.9940e-16];
        double[] r = [0.3, -1.1, 0.7, 2.2, -0.4, 1.5, -2.0, 0.9];
        var block = new double[8 * 4];
        for (int i = 0; i < 8; i++)
        {
            double x1 = i + 1;
            double x2 = x1 + (1e-2 * r[i]);
            block[i] = 1.0;
            block[8 + i] = x1;
            block[16 + i] = x2;
            block[24 + i] = x1 - x2;
        }

        double[] values = JacobiSpectrum.SingularValues(block, 8, 4);

        for (int k = 0; k < 3; k++)
        {
            Assert.Equal(expected[k], values[k], expected[k] * 1e-4);
        }

        // The fourth is rounding on both sides; only its order of magnitude is meaningful.
        Assert.True(values[3] < 1e-14, $"sigma_min was {values[3]}");
    }
}
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~JacobiSpectrum"
```

Expected: **compile error** `CS0103: The name 'JacobiSpectrum' does not exist`. Read the count,
not the colour — a filter matching nothing exits 8 under Microsoft.Testing.Platform and says
`Zéro tests exécutés`, which is not a pass.

- [ ] **Step 3: Create the shared kernel**

Create `src/Shared/JacobiSpectrum.cs`:

```csharp
namespace Lodestar.Internal;

/// <summary>The singular values of a dense block, by one-sided Jacobi rotations.</summary>
/// <remarks>
/// Shared source rather than an API: <c>Lodestar.Decomposition</c> factors with it,
/// <c>Lodestar.Stats.Regression</c> and <c>Lodestar.Stats.TimeSeries</c> measure a triangular
/// factor's rank and condition with it, and <c>src/</c> reaches its neighbours through a
/// published floor — so one copy compiled into each is what keeps them in step without a
/// release between them (#978, and #843 for the precedent).
/// </remarks>
internal static class JacobiSpectrum
{
    // Rotating a pair whose off-diagonal is already at the rounding floor changes nothing
    // and costs a sweep, so the sweep stops when every pair is below it.
    private const double Threshold = 1e-15;
    private const int MaximumSweeps = 60;

    /// <summary>Orthogonalizes a tall column-major block's columns in place and reports their norms, largest first.</summary>
    /// <remarks>
    /// One-sided Jacobi: the norms the columns converge to are the singular values, and no
    /// bidiagonalization, shift or deflation is needed to reach them. <c>V</c> is not
    /// accumulated, which is a rotation per pair a caller reading only the spectrum would
    /// otherwise pay for a factor it drops.
    /// </remarks>
    /// <param name="columnMajor">The block, column by column; overwritten with its orthogonalized columns.</param>
    /// <param name="rows">Rows in the block, at least <paramref name="columns"/>.</param>
    /// <param name="columns">Columns in the block.</param>
    /// <returns>The singular values, largest first, <paramref name="columns"/> of them.</returns>
    /// <exception cref="ArgumentException"><paramref name="columnMajor"/> is not a tall <paramref name="rows"/> × <paramref name="columns"/> block.</exception>
    internal static double[] SingularValues(double[] columnMajor, int rows, int columns)
    {
        if (columnMajor is null || rows < columns || columnMajor.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {columnMajor?.Length ?? 0} is not a tall {rows} × {columns}.",
                nameof(columnMajor));
        }

        double[] squared = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            double norm = Norm(columnMajor.AsSpan(j * rows, rows));
            squared[j] = norm * norm;
        }

        for (int sweep = 0; sweep < MaximumSweeps; sweep++)
        {
            bool rotated = false;
            for (int p = 0; p < columns - 1; p++)
            {
                for (int q = p + 1; q < columns; q++)
                {
                    rotated |= RotateTrackedPair(columnMajor, squared, rows, p, q);
                }
            }

            if (!rotated)
            {
                break;
            }
        }

        // The tracked norms steered the sweeps; the answer is read off the columns themselves,
        // so rounding accumulated in the updates never reaches it.
        double[] norms = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            norms[j] = Norm(columnMajor.AsSpan(j * rows, rows));
        }

        Array.Sort(norms, (left, right) => right.CompareTo(left));
        return norms;
    }

    /// <summary>Orthogonalizes one pair of columns with the two squared norms carried rather than recomputed, and reports whether it had to.</summary>
    /// <remarks>
    /// The rotation that zeroes <c>γ</c> moves <c>tγ</c> of squared norm from one column to the
    /// other, so <c>α − tγ</c> and <c>β + tγ</c> are exact up to rounding and one product over
    /// the rows replaces three.
    /// </remarks>
    private static bool RotateTrackedPair(double[] work, double[] squared, int rows, int p, int q)
    {
        double alpha = squared[p];
        double beta = squared[q];
        Span<double> left = work.AsSpan(p * rows, rows);
        Span<double> right = work.AsSpan(q * rows, rows);
        double gamma = 0;
        for (int i = 0; i < left.Length; i++)
        {
            gamma += left[i] * right[i];
        }

        if (!Rotation(alpha, beta, gamma, out double t, out double cosine, out double sine))
        {
            return false;
        }

        ElementWise.Rotate(left, right, cosine, sine);
        squared[p] = alpha - (t * gamma);
        squared[q] = beta + (t * gamma);
        return true;
    }

    /// <summary>The rotation that orthogonalizes a pair, or false when the pair already is.</summary>
    /// <remarks><paramref name="t"/> is its tangent, which the caller needs for the tracked norms.</remarks>
    private static bool Rotation(
        double alpha, double beta, double gamma, out double t, out double cosine, out double sine)
    {
        t = 0;
        cosine = 1;
        sine = 0;
        // S1244: whether the pair is already orthogonal (gamma vanished entirely), not
        // whether two computed quantities are close.
#pragma warning disable S1244
        if (gamma == 0 || Math.Abs(gamma) <= Threshold * Math.Sqrt(alpha * beta))
#pragma warning restore S1244
        {
            return false;
        }

        double zeta = (beta - alpha) / (2.0 * gamma);
        t = Math.Sign(zeta) / (Math.Abs(zeta) + Math.Sqrt(1.0 + (zeta * zeta)));
        // S1244: whether the columns already have equal norm (zeta vanished entirely),
        // not whether two computed quantities are close.
#pragma warning disable S1244
        if (zeta == 0)
#pragma warning restore S1244
        {
            t = 1.0;
        }

        cosine = 1.0 / Math.Sqrt(1.0 + (t * t));
        sine = cosine * t;
        return true;
    }

    private static double Norm(ReadOnlySpan<double> column)
    {
        double sum = 0;
        foreach (double value in column)
        {
            sum += value * value;
        }

        return Math.Sqrt(sum);
    }
}
```

- [ ] **Step 4: Add the opt-in to `src/Directory.Build.props`**

Insert after the `LodestarIncludesElementWise` group (currently ending at :53). MSBuild comments
cannot contain a double hyphen — keep the text free of `--`.

```xml
  <!-- The one-sided Jacobi spectrum, compiled into the three libraries that read singular values.
       Lodestar.Decomposition factors with it; Lodestar.Stats.Regression and Lodestar.Stats.TimeSeries
       measure a triangular factor's rank and condition with it, and neither can reach the other's
       internals across a published floor, so shared source keeps one copy (#978). -->
  <ItemGroup Condition="'$(LodestarIncludesJacobiSpectrum)' == 'true'">
    <Compile Include="$(MSBuildThisFileDirectory)Shared/JacobiSpectrum.cs" Link="Internal/JacobiSpectrum.cs" />
  </ItemGroup>
```

- [ ] **Step 5: Turn the opt-in on in the three libraries**

In `src/Lodestar.Decomposition/Lodestar.Decomposition.csproj`, beside the existing
`LodestarIncludesElementWise`:

```xml
    <LodestarIncludesJacobiSpectrum>true</LodestarIncludesJacobiSpectrum>
```

In `src/Lodestar.Stats.Regression/Lodestar.Stats.Regression.csproj` and
`src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj`, beside the existing
`LodestarIncludesReflections` — both need `ElementWise` too, which the kernel rotates with and
neither opts into today:

```xml
    <LodestarIncludesElementWise>true</LodestarIncludesElementWise>
    <LodestarIncludesJacobiSpectrum>true</LodestarIncludesJacobiSpectrum>
```

- [ ] **Step 6: Delegate from `JacobiSvd`, leaving one copy**

In `src/Lodestar.Decomposition/Internal/JacobiSvd.cs`, replace the whole body of
`SingularValues` (:89-133) with the transpose plus a call, and **delete** `RotateTrackedPair`
(:136-163), which moved:

```csharp
    /// <summary>The singular values of a tall row-major block, largest first, with no vectors.</summary>
    /// <remarks>
    /// <see cref="JacobiSpectrum"/> holds the sweep: <c>Lodestar.Stats.Regression</c> and
    /// <c>Lodestar.Stats.TimeSeries</c> read a triangular factor's spectrum with the same
    /// arithmetic in the same order, and cannot reach this assembly's internals (#978).
    /// </remarks>
    internal static double[] SingularValues(ReadOnlySpan<double> a, int rows, int columns)
    {
        if (rows < columns || a.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {a.Length} is not a tall {rows} × {columns}.", nameof(a));
        }

        return JacobiSpectrum.SingularValues(DenseBlock.Transpose(a, rows, columns), rows, columns);
    }
```

Add `using Lodestar.Internal;` at the top if the global usings do not already cover it — check
`src/Shared/GlobalUsings.cs` first and do not add a redundant using, which is an analyzer finding.

- [ ] **Step 7: Run the new tests and the whole Decomposition suite**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~JacobiSpectrum"
```

Expected: **3 passed**. Then the package whose kernel moved, both frameworks:

```bash
dotnet test tests/Lodestar.Decomposition.Tests tests/Lodestar.Decomposition.NetStandard.Tests -c Release
```

Expected: every test that passed before still passes, and the counts are unchanged. A moved
kernel that changed an answer shows up here, in `TruncatedSvd` and `PrincipalComponentVariance`.

- [ ] **Step 8: Commit**

```bash
git add src/Shared/JacobiSpectrum.cs src/Directory.Build.props src/Lodestar.Decomposition src/Lodestar.Stats.Regression/Lodestar.Stats.Regression.csproj src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj tests/Lodestar.Stats.Regression.Tests/JacobiSpectrumTests.cs
git commit -m "Share the Jacobi spectrum sweep with the packages that measure a triangular factor"
```

---

### Task 2: The rank refusal reads the spectrum

**Files:**

- Modify: `src/Lodestar.Stats.Regression/Internal/LeastSquares.cs:146-183` (the remark and `RequireFullRank`)
- Modify: `src/Lodestar.Stats.Regression/Internal/LeastSquares.cs:124-142` (`FromTriangle`, for the ordering the fast accept needs)
- Test: `tests/Lodestar.Stats.Regression.Tests/CollinearDesignTests.cs`

**Interfaces:**

- Consumes: `JacobiSpectrum.SingularValues(double[], int, int)` from Task 1.
- Produces: `LeastSquares.RequireFullRank(double[] a, int rowCount, int parameterCount, string parameterName)`
  keeps its signature. A new overload is **not** added — `HouseholderEstimate.cs:65` calls the
  same one.

- [ ] **Step 1: Write the failing test**

Append to `tests/Lodestar.Stats.Regression.Tests/CollinearDesignTests.cs`:

```csharp
    [Theory]
    [InlineData(1e-2)]
    [InlineData(1e-4)]
    [InlineData(1e-6)]
    [InlineData(1e-8)]
    [InlineData(1e-10)]
    public void Fit_RefusesADependentColumnFarSmallerThanTheColumnsItDependsOn(double delta)
    {
        // x3 = x1 - x2 is exact in double, so the design has rank 3 whatever delta is.
        // statsmodels 0.15.0 finds rank 3 and df_resid 5 at every one of these (#978).
        double[] r = [0.3, -1.1, 0.7, 2.2, -0.4, 1.5, -2.0, 0.9];
        double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];
        var design = new double[8 * 3];
        for (int i = 0; i < 8; i++)
        {
            double x1 = i + 1;
            double x2 = x1 + (delta * r[i]);
            design[(i * 3) + 0] = x1;
            design[(i * 3) + 1] = x2;
            design[(i * 3) + 2] = x1 - x2;
        }

        ArgumentException failure = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(design, response, 3));

        Assert.Equal("design", failure.ParamName);
        Assert.Contains("rank-deficient or collinear", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Fit_StillAcceptsAFullRankDesignWhoseColumnsDifferInScale()
    {
        // numpy.linalg.matrix_rank calls this one full rank: sigma_min 4.1e-3 against a
        // tolerance of 3.7e-4. The spectral test must agree with it, not merely be stricter.
        var seeded = new Random(978);
        var design = new double[200 * 3];
        var response = new double[200];
        for (int i = 0; i < 200; i++)
        {
            design[(i * 3) + 0] = 1e7 + (seeded.NextDouble() * 9.9e8);
            design[(i * 3) + 1] = 1e-6 + (seeded.NextDouble() * 9.99e-4);
            design[(i * 3) + 2] = seeded.NextDouble() - 0.5;
            response[i] = seeded.NextDouble();
        }

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, 3);

        Assert.Equal(196, summary.ResidualDegreesOfFreedom);
    }
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~CollinearDesign"
```

Expected: the five `Theory` cases **fail** — `Assert.Throws` reports that no exception was
thrown. The second test already passes; it is the guard that the fix does not overshoot.

- [ ] **Step 3: Reorder `FromTriangle` so the inverse is available to the fast accept**

`RequireFullRank` runs before `InvertUpper` today. The cheap accept needs `‖R⁻¹‖_F`, so the
refusal splits in two around the inversion. Replace `FromTriangle` (:124-142) with:

```csharp
    /// <summary>R's inverse and the coefficients it gives, from a triangularized block and its projected response.</summary>
    private static (double[] Coefficients, double[] InverseUpper) FromTriangle(
        double[] a, int rowCount, int parameterCount, double[] projected)
    {
        double[] upper = Upper(a, rowCount, parameterCount);
        RequireNonSingularDiagonal(upper, rowCount, parameterCount, DesignParameter);
        double[] inverseUpper = InvertUpper(upper, parameterCount);
        RequireFullRank(a, rowCount, parameterCount, inverseUpper, DesignParameter);
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

        return (coefficients, inverseUpper);
    }
```

`Upper(a, rowCount, parameterCount)` is the existing helper and `InvertUpper` already takes its
result, so this only hoists a call that was nested.

- [ ] **Step 4: Write the two-bracket refusal**

Replace `RequireFullRank` (:146-183) with the diagonal fast reject, the Frobenius fast accept and
the sweep between them:

```csharp
    /// <summary>Refuses a triangularized design whose diagonal alone proves it rank-deficient, before its inverse is formed.</summary>
    /// <remarks>
    /// <c>σmin ≤ minₖ|Rₖₖ|</c> and <c>σmax ≥ maxₖ|Rₖₖ|</c>, so a diagonal ratio at or below the
    /// tolerance bounds <c>σmin/σmax</c> there too and settles the refusal with no sweep. It also
    /// keeps an exact zero — <c>x₂ = 2·x₁</c> puts one there — out of <see cref="InvertUpper"/>.
    /// </remarks>
    private static void RequireNonSingularDiagonal(
        double[] upper, int rowCount, int parameterCount, string parameterName)
    {
        double largest = 0.0;
        double smallest = double.PositiveInfinity;
        for (int k = 0; k < parameterCount; k++)
        {
            double pivot = Math.Abs(upper[(k * parameterCount) + k]);
            largest = Math.Max(largest, pivot);
            smallest = Math.Min(smallest, pivot);
        }

        // Not negated into a > test: a NaN from the caller's data is not a collinear column.
        if (smallest <= Tolerance(rowCount, parameterCount) * largest)
        {
            throw Collinear(parameterName);
        }
    }

    /// <summary>Refuses a triangularized design whose smallest singular value is rounding beside its largest.</summary>
    /// <remarks>
    /// <c>numpy.linalg.matrix_rank</c>'s test, which is <c>σmin ≤ σmax·max(n, p)·ε</c> on the
    /// design; R carries the design's singular values unchanged, the reflections being
    /// orthogonal. The Frobenius bracket answers first — <c>σmax ≤ ‖R‖_F</c> and
    /// <c>σmin ≥ 1/‖R⁻¹‖_F</c> give away at most a factor p, so every design conditioned better
    /// than about <c>1/(p·n·ε)</c> is accepted without a sweep (#978, #867).
    /// </remarks>
    /// <param name="a">The triangularized block, column-major.</param>
    /// <param name="rowCount">How many rows it holds.</param>
    /// <param name="parameterCount">How many columns it holds.</param>
    /// <param name="inverseUpper">R's inverse, row-major, which the caller has already formed.</param>
    /// <param name="parameterName">The public parameter the design arrived as, which the refusal names.</param>
    /// <exception cref="ArgumentException">The design is rank-deficient.</exception>
    internal static void RequireFullRank(
        double[] a, int rowCount, int parameterCount, double[] inverseUpper, string parameterName)
    {
        double tolerance = Tolerance(rowCount, parameterCount);
        double squaredUpper = 0.0;
        double squaredInverse = 0.0;
        for (int k = 0; k < parameterCount; k++)
        {
            int column = k * rowCount;
            for (int i = 0; i <= k; i++)
            {
                squaredUpper += a[column + i] * a[column + i];
            }

            for (int i = 0; i < parameterCount; i++)
            {
                double entry = inverseUpper[(k * parameterCount) + i];
                squaredInverse += entry * entry;
            }
        }

        double frobenius = Math.Sqrt(squaredUpper) * Math.Sqrt(squaredInverse);
        if (frobenius * tolerance < 1.0)
        {
            return;
        }

        double[] singular = JacobiSpectrum.SingularValues(
            Triangle(a, rowCount, parameterCount), parameterCount, parameterCount);

        // Not negated into a > test: a NaN from the caller's data is not a collinear column.
        if (singular[parameterCount - 1] <= tolerance * singular[0])
        {
            throw Collinear(parameterName);
        }
    }

    /// <summary><c>numpy.linalg.matrix_rank</c>'s relative tolerance for an <c>n × p</c> block.</summary>
    private static double Tolerance(int rowCount, int parameterCount)
        => Math.Max(rowCount, parameterCount) * MachineEpsilon;

    /// <summary>R's leading square block, column-major, with the rows below the diagonal dropped.</summary>
    private static double[] Triangle(double[] a, int rowCount, int parameterCount)
    {
        var triangle = new double[parameterCount * parameterCount];
        for (int k = 0; k < parameterCount; k++)
        {
            for (int i = 0; i <= k; i++)
            {
                triangle[(k * parameterCount) + i] = a[(k * rowCount) + i];
            }
        }

        return triangle;
    }

    /// <summary>The one refusal both brackets raise, so the two cannot drift apart in wording.</summary>
    private static ArgumentException Collinear(string parameterName)
        => new(
            "design is rank-deficient or collinear: a column, intercept first when one is "
            + "fitted, lies within rounding of the span of the others, so the fit has no unique "
            + "solution. Drop the dependent regressor.",
            parameterName);
```

- [ ] **Step 5: Fix the other caller**

`src/Lodestar.Stats.Regression/Internal/HouseholderEstimate.cs:65` calls the four-argument
`RequireFullRank`. It must move to the same two-bracket order. Read its surroundings, then apply
the same shape it uses for `Upper`/`InvertUpper`; if it does not form an inverse, call
`RequireNonSingularDiagonal` followed by `InvertUpper` and then `RequireFullRank`, exactly as
`FromTriangle` now does.

- [ ] **Step 6: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~CollinearDesign|FullyQualifiedName~LeastSquares"
```

Expected: every case passes, the five new `Theory` rows included.

- [ ] **Step 7: Run the whole package, both frameworks**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests tests/Lodestar.Stats.Regression.NetStandard.Tests -c Release
```

Expected: no regression. The oracle suites (`OlsOracleTests`, the WLS and GLS corpora) are the
ones that would show a design the new test refuses and the corpus expects an answer for. If one
turns red, the corpus design is the finding — check it against
`numpy.linalg.matrix_rank` before touching the tolerance.

- [ ] **Step 8: Commit**

```bash
git add src/Lodestar.Stats.Regression tests/Lodestar.Stats.Regression.Tests
git commit --amend --no-edit
```

---

### Task 3: The same refusal in `Lodestar.Stats.TimeSeries`

`SharedReflections.RequireFullRank` carries a copy of the old test and the same remark, which is
why #978 names it. It keeps its own copy of the arithmetic (it works on a reused `_a` buffer
across a lag search), so the bracket is rewritten there rather than shared.

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Internal/SharedReflections.cs:98-129`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/SharedReflectionsTests.cs`

**Interfaces:**

- Consumes: `JacobiSpectrum.SingularValues` from Task 1, and `SharedReflections.InverseUpper(int order)`,
  which already exists at :131.

- [ ] **Step 1: Write the failing test**

Append to `tests/Lodestar.Stats.TimeSeries.Tests/SharedReflectionsTests.cs`:

```csharp
    [Fact]
    public void RequireFullRank_RefusesADependentColumnFarSmallerThanItsSources()
    {
        // The #978 design, as a lagged block: column 2 is column 0 minus column 1, exact in
        // double, and is 380 times smaller than either.
        double[] r = [0.3, -1.1, 0.7, 2.2, -0.4, 1.5, -2.0, 0.9];
        var columnMajor = new double[8 * 3];
        for (int i = 0; i < 8; i++)
        {
            double x1 = i + 1;
            double x2 = x1 + (1e-2 * r[i]);
            columnMajor[i] = x1;
            columnMajor[8 + i] = x2;
            columnMajor[16 + i] = x1 - x2;
        }

        var reflections = new SharedReflections(columnMajor, 8, 3);
        reflections.Triangularize(3);

        ArgumentException failure = Assert.Throws<ArgumentException>(
            () => reflections.RequireFullRank(3, "series"));

        Assert.Equal("series", failure.ParamName);
    }
```

Check `SharedReflections`' real constructor and triangularize entry point before running — read
`src/Lodestar.Stats.TimeSeries/Internal/SharedReflections.cs:1-95` and the existing tests in the
file, and match what they do rather than the names above if they differ.

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~SharedReflections"
```

Expected: the new case **fails**, no exception thrown.

- [ ] **Step 3: Rewrite the refusal**

Replace `RequireFullRank` (:101-129) with:

```csharp
    /// <summary>Refuses a design whose smallest singular value is rounding beside its largest.</summary>
    /// <remarks>
    /// <c>numpy.linalg.matrix_rank</c>'s <c>σmin ≤ σmax·max(n, p)·ε</c>, which R carries unchanged
    /// from the design, and the test <c>Lodestar.Stats.Regression</c>'s fits refuse at. A diagonal
    /// ratio settles the blatant case first and a Frobenius bracket the well-conditioned one, so
    /// the sweep runs only between them — a variable proportional to another gave VAR coefficients
    /// near 1e13 (#873), and one far smaller than its sources passed the per-column test (#978).
    /// </remarks>
    internal void RequireFullRank(int order, string parameterName)
    {
        double tolerance = Math.Max(_rows, order) * MachineEpsilon;
        double largest = 0.0;
        double smallest = double.PositiveInfinity;
        for (int k = 0; k < order; k++)
        {
            double pivot = Math.Abs(_a[(k * _rows) + k]);
            largest = Math.Max(largest, pivot);
            smallest = Math.Min(smallest, pivot);
        }

        // Not negated into a > test: a NaN from the caller's data is not a collinear column.
        if (smallest <= tolerance * largest)
        {
            throw Collinear(parameterName);
        }

        var triangle = new double[order * order];
        double squaredUpper = 0.0;
        for (int k = 0; k < order; k++)
        {
            for (int i = 0; i <= k; i++)
            {
                double entry = _a[(k * _rows) + i];
                triangle[(k * order) + i] = entry;
                squaredUpper += entry * entry;
            }
        }

        double[] inverse = InverseUpper(order);
        double squaredInverse = 0.0;
        foreach (double entry in inverse)
        {
            squaredInverse += entry * entry;
        }

        if (Math.Sqrt(squaredUpper) * Math.Sqrt(squaredInverse) * tolerance < 1.0)
        {
            return;
        }

        double[] singular = JacobiSpectrum.SingularValues(triangle, order, order);
        if (singular[order - 1] <= tolerance * singular[0])
        {
            throw Collinear(parameterName);
        }
    }

    /// <summary>The one refusal both brackets raise, so the two cannot drift apart in wording.</summary>
    private static ArgumentException Collinear(string parameterName)
        => new(
            "the lagged design is rank-deficient or collinear: a column, intercept first when "
            + "one is fitted, lies within rounding of the span of the others, so the fit has no "
            + "unique solution. Drop a variable that is a combination of the others.",
            parameterName);
```

- [ ] **Step 4: Run the tests, both frameworks**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests tests/Lodestar.Stats.TimeSeries.NetStandard.Tests -c Release
```

Expected: the new case passes and nothing else moved. `VectorAutoregressionEdgeTests` asserts on
the old message — update the assertion to the new text, and only the text.

- [ ] **Step 5: Commit**

```bash
git add src/Lodestar.Stats.TimeSeries tests/Lodestar.Stats.TimeSeries.Tests
git commit --amend --no-edit
```

---

### Task 4: The conditioning gate reads the condition number

> **Revised mid-execution, 2026-09-18.** This task was written to take the exact `κ₂` from a
> Jacobi spectrum of the scaled factor. Measured before it was committed, that sweep costs
> 82–350 ms at order 251 where a whole 4 000-row reflections fit costs 41 ms — about twice the
> detour it exists to avoid. Cyril's condition was that a slower result keeps the current
> algorithm, so the exact route was replaced by power iteration on `AᵀA` in
> `Internal/ConditionEstimate.cs`: 2.45 ms at that order, within 1% of the spectrum. The steps
> below are kept as written, with `ConditionEstimate.Of` standing where `JacobiSpectrum` stood;
> `docs/guides/performance.md` carries the A/B that decided it.

**Files:**

- Modify: `src/Lodestar.Stats.Regression/Internal/LeastSquares.cs:186-192` (the constant's remark)
- Modify: `src/Lodestar.Stats.Regression/Internal/LeastSquares.cs:296-320` (`WellConditioned`)
- Test: `tests/Lodestar.Stats.Regression.Tests/LeastSquaresTests.cs`

**Interfaces:**

- Consumes: `JacobiSpectrum.SingularValues` from Task 1.
- Produces: `WellConditioned(double[] upper, double[] inverseUpper, int order)` keeps its
  signature and its meaning — `true` means the normal equations may answer.

- [ ] **Step 1: Write the failing test**

Append to `tests/Lodestar.Stats.Regression.Tests/LeastSquaresTests.cs`:

```csharp
    [Fact]
    public void Fit_OnAWideWellConditionedDesign_AgreesWithTheReflections()
    {
        // p = 250 scores 258 on the Frobenius bound though its true kappa is 1.65, so every
        // design this wide took the reflections before #985. The two paths must still agree.
        const int Rows = 1200;
        const int Features = 250;
        var seeded = new Random(985);
        var design = new double[Rows * Features];
        var response = new double[Rows];
        for (int i = 0; i < design.Length; i++)
        {
            design[i] = (seeded.NextDouble() * 2.0) - 1.0;
        }

        for (int row = 0; row < Rows; row++)
        {
            response[row] = (seeded.NextDouble() * 2.0) - 1.0;
        }

        (double[] fast, _) = LeastSquares.Solve(design, Rows, Features, withIntercept: true, response);
        (double[] reflected, _) = LeastSquares.SolveByReflections(
            design, Rows, Features, withIntercept: true, response);

        for (int k = 0; k < fast.Length; k++)
        {
            Assert.Equal(reflected[k], fast[k], Math.Abs(reflected[k]) * 1e-9 + 1e-11);
        }
    }
```

This test passes today (both sides take the reflections) and must keep passing once the wide
design starts taking the normal equations — it is the correctness guard on the change, and the
benchmark in Task 5 is what shows the routing actually moved.

- [ ] **Step 2: Run it and confirm it passes before the change**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release --filter "FullyQualifiedName~AgreesWithTheReflections"
```

Expected: **1 passed**. Record that it was green before, so a red afterwards is unambiguous.

- [ ] **Step 3: Add the exact test behind the bound**

Replace `WellConditioned` (:296-320) with:

```csharp
    /// <summary>Whether the column-scaled design's condition number stays within <see cref="NormalEquationsConditionLimit"/>.</summary>
    /// <remarks>
    /// <c>D</c> the column norms, <c>U·D⁻¹</c> is the scaled design's factor. The diagonal ratio
    /// bounds its <c>κ₂</c> from below and <c>√p·‖D·U⁻¹‖_F</c> from above, and between the two the
    /// spectrum settles it: the Frobenius bound gives away a factor of <c>p</c>, so it is at least
    /// <c>p</c> for any design at all and sent every design past 200 parameters to the reflections
    /// however well conditioned (#985). The sweep costs <c>O(p³)</c> where that detour cost
    /// <c>O(np²)</c>.
    /// </remarks>
    private static bool WellConditioned(double[] upper, double[] inverseUpper, int order)
    {
        var scaled = new double[order * order];
        double total = 0.0;
        double largest = 0.0;
        double smallest = double.PositiveInfinity;
        for (int i = 0; i < order; i++)
        {
            // Column i of U has the norm of the design's column i, since UᵀU = XᵀX.
            double squaredScale = 0.0;
            double squaredRow = 0.0;
            for (int k = 0; k < order; k++)
            {
                squaredScale += upper[(k * order) + i] * upper[(k * order) + i];
                squaredRow += inverseUpper[(i * order) + k] * inverseUpper[(i * order) + k];
            }

            total += squaredScale * squaredRow;
            double scale = Math.Sqrt(squaredScale);
            double pivot = Math.Abs(upper[(i * order) + i]) / scale;
            largest = Math.Max(largest, pivot);
            smallest = Math.Min(smallest, pivot);
            for (int k = 0; k <= i; k++)
            {
                scaled[(i * order) + k] = upper[(k * order) + i] / scale;
            }
        }

        if (order * total <= NormalEquationsConditionLimit * NormalEquationsConditionLimit)
        {
            return true;
        }

        if (largest > NormalEquationsConditionLimit * smallest)
        {
            return false;
        }

        double[] singular = JacobiSpectrum.SingularValues(scaled, order, order);
        return singular[0] <= NormalEquationsConditionLimit * singular[order - 1];
    }
```

`scaled` is column-major: entry `(i * order) + k` is row `k` of column `i`, and `upper` is
row-major, so column `i` is read with the stride `k * order`.

- [ ] **Step 4: Update the constant's remark**

At :186-192, the `NormalEquationsConditionLimit` remark says the bound is what decides. Replace
the last sentence so it describes the gate as it now is:

```csharp
    /// <summary>The largest condition number of the column-scaled design the normal equations are trusted with.</summary>
    /// <remarks>
    /// The normal equations lose about <c>κ²·ε</c>, and a column's scale does not count towards <c>κ</c> for a Cholesky
    /// factorization (van der Sluis). At 200, <c>200²·ε</c> is 9e-12, a hundred times inside the corpora's 1e-9; a design
    /// past it — the near-collinear fixtures, a polynomial on a narrow range — goes through the reflections instead
    /// (#782, #870). <see cref="WellConditioned"/> measures <c>κ₂</c> itself when its two bounds disagree, which is what
    /// keeps the limit a statement about conditioning rather than about <c>p</c> (#985).
    /// </remarks>
```

- [ ] **Step 5: Run the package, both frameworks**

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests tests/Lodestar.Stats.Regression.NetStandard.Tests -c Release
```

Expected: everything green, the Task 4 test included. The oracle corpora are the real check — a
fit that used to take the reflections and now takes the normal equations must still land inside
the corpus's `1e-9`. A corpus failure here means the limit is wrong, not the test.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Stats.Regression tests/Lodestar.Stats.Regression.Tests
git commit --amend --no-edit
```

---

### Task 5: Benchmarks

No pull request touching `Lodestar.Stats.*` ships without a before/after on a named machine, and
speed was the condition attached to both decisions. Two questions: what the rank bracket costs a
narrow fit, and what the conditioning change wins a wide one.

**Files:**

- Modify: `bench/Lodestar.Stats.Benchmarks/OlsBenchmarks.cs`
- Modify: `bench/bench-map.json`
- Modify: `docs/guides/performance.md`

- [ ] **Step 1: Add the wide-design case**

Read `bench/Lodestar.Stats.Benchmarks/OlsBenchmarks.cs` first and follow its existing `[Params]`
and setup shape rather than the sketch below. It needs a case at `Features = 250` with
`Rows = 4000`, which is the routing that changes, and one at `Features = 4`, which is the
narrow fit the rank bracket must not slow down.

- [ ] **Step 2: Register it in the bench map**

`bench/bench-map.json` is JSON that a clean rebase can still break. After editing it:

```bash
python3 -c "import json;json.load(open('bench/bench-map.json'));print('valid')"
```

- [ ] **Step 3: Take the baseline on `main`, holding the lock**

The corpus under `bench/corpus/` is untracked, so a fresh worktree has none; check it is there
before believing a row. Hold the lock across the whole campaign, and release it in the same
command that ends it.

```bash
cd <repo> && ./.dotnet-guarded acquire "978/985 OLS baseline"
```

```bash
cd <baseline worktree> && git checkout 6346a2ae -- . && dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*Ols*'
```

- [ ] **Step 4: Take the branch numbers, then release**

```bash
cd <worktree> && dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*Ols*'; cd <repo> && ./.dotnet-guarded release
```

- [ ] **Step 5: Write the numbers into the guide**

`docs/guides/performance.md` holds every number with its machine and its window. Add the two
rows — narrow fit before/after, wide fit before/after — and name the machine. If the narrow fit
regressed at all, the Frobenius fast accept is not firing; check that before writing the row.

- [ ] **Step 6: Commit**

```bash
git add bench docs/guides/performance.md
git commit --amend --no-edit
```

---

### Task 6: The record — ADR, equivalence, changelog

**Files:**

- Create: `docs/decisions/0147-the-rank-refusal-and-the-conditioning-gate-are-measured-on-singular-values.md`
- Modify: `docs/decisions/index.yaml` (regenerated, not hand-edited)
- Modify: `docs/decisions/README.md` (the prose counts `pytest tools/tests` checks)
- Modify: `docs/equivalence.md:513` (OLS), `:525` (WLS), `:526` (GLS), and the VAR row
- Modify: `CHANGELOG.md`

- [ ] **Step 1: Confirm the ADR number is still free**

```bash
cd <repo> && ./.next-adr --check
```

Expected: `next free is 0147`. If another branch took it while this one was open, use what it
prints now.

- [ ] **Step 2: Write the ADR**

Frontmatter first, then the body, in the shape `tools/regen_adr_index.py` reads:

```markdown
---
status: accepted
supersedes: []
amends: []
applies: ["0096", "0105"]
---
# 0147 — The rank refusal and the conditioning gate are measured on singular values

**Status:** accepted · **Date:** 2026-09-18 · **Applies:** [`0096`](0096-ordinary-least-squares-earns-its-own-package.md), [`0105`](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md)

## Context
```

The context is the spec's Problem section, the decision its Decision section, and the
consequence the scale-invariance table. The ADR states the decision and its loser — scale
invariance — and does not restate the code. Cross-link the spec.

- [ ] **Step 3: Regenerate the index and the prose**

```bash
cd <worktree> && python3 tools/regen_adr_index.py && python3 -m pytest tools/tests -q
```

Expected: the index gains `0147` and the prose counts in `docs/decisions/README.md` agree. The
pytest suite is what CI's Lint job runs; a red here is a red pull request.

- [ ] **Step 4: Correct the three equivalence rows**

In `docs/equivalence.md`, the OLS row (:513), the WLS row (:525) and the GLS row (:526) each say
the refusal is at "`max(n, p)·ε` of its norm, `numpy.linalg.matrix_rank`'s tolerance". Replace
that clause with what the code now does — `σmin ≤ σmax·max(n, p)·ε`, which **is** that tolerance
— and say that the refusal is no longer scale invariant, as the reference's is not. The rows
keep their `x₂ = 3·x₁` example, which still refuses. Do the same for the VAR row.

- [ ] **Step 5: Add the changelog entry**

One sentence, the issue and the commit, nothing else, under the per-package headings for
`Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries`.

- [ ] **Step 6: Commit**

```bash
git add docs CHANGELOG.md
git commit --amend --no-edit
```

---

### Task 7: The gates

- [ ] **Step 1: Every check script, not a subset**

```bash
cd <worktree> && for s in tools/check_*.py; do echo "== $s"; python3 "$s" || echo "FAILED $s"; done
```

`check_repeated_literals.py` takes `--base origin/main`; run that one with it.

- [ ] **Step 2: Format and lint**

```bash
cd <worktree> && dotnet format Lodestar.slnx --verify-no-changes && npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

- [ ] **Step 3: The doc snippets**

```bash
cd <worktree> && python3 tools/extract_doc_snippets.py && dotnet build samples/Lodestar.DocSnippets -c Release
```

- [ ] **Step 4: The whole suite, and read the count**

```bash
cd <worktree> && dotnet test Lodestar.slnx -c Release 2>&1 | tail -50
```

Expected: **36 assemblies**, eighteen suites and their eighteen mirrors. A count that is not 36
means a suite went missing, which has no exit code of its own.

- [ ] **Step 5: Build from outside the checkout before pushing**

A nested worktree silences Sonar rules. Build from the parent directory, then analyse the touched
files through the SonarQube MCP server: `toggle_automatic_analysis` off, `analyze_file_list` on
what changed, then back on. Look the project key up with `search_my_sonarqube_projects` rather
than guessing.

- [ ] **Step 6: Sync with `main`, then open the pull request**

```bash
cd <worktree> && git fetch origin && git rebase origin/main && python3 -m pytest tools/tests -q
```

A clean rebase can still break JSON, so `bench/bench-map.json` is re-validated after it. Then
open the pull request as a **draft**, with the before/after numbers and the machine named, and
mark it ready only once CI is green.

## Self-review

- **Spec coverage.** #978 → Tasks 2 and 3; #985 → Task 4; the shared-kernel decision → Task 1;
  the speed condition → the brackets in Tasks 2, 3 and 4 and the measurement in Task 5; the
  rejected options and the scale-invariance table → the ADR in Task 6.
- **Placeholders.** Task 5 steps 1 and 3 and Task 6 step 2 describe shapes rather than showing
  final code, because each depends on a file's existing conventions that must be read first;
  each says which file to read and what the result must contain. Task 3 step 1 flags that the
  constructor names must be checked against the file.
- **Type consistency.** `RequireFullRank` gains a fifth parameter in `Lodestar.Stats.Regression`
  (Task 2) and keeps its two in `Lodestar.Stats.TimeSeries` (Task 3) — they are different
  methods in different assemblies, which is deliberate. `JacobiSpectrum.SingularValues` takes a
  `double[]` it overwrites in every call site.
