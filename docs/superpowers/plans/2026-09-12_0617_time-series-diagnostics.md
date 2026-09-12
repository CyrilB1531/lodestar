# Serial Correlation Diagnostics Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship the autocorrelation function, the partial autocorrelation function and the Ljung-Box test at `statsmodels` 0.15.0 parity, so a caller can decide whether a series or a model's residuals still carry serial dependence.

**Architecture:** Three public static methods on one class, over one internal autocovariance kernel. `Autocorrelation` divides the autocovariance by its lag-zero value and bands it with Bartlett's formula; `PartialAutocorrelation` runs the Levinson-Durbin recursion over the *adjusted* autocovariance and bands it flat at `1/n`; `LjungBox` is a cumulative sum of squared autocorrelations against a chi-square tail `Lodestar.Stats` already publishes. Nothing here needs a dependency, an FFT, or a member the package does not already export.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, `statsmodels` 0.15.0 as the frozen oracle, BenchmarkDotNet against `Cortex.TimeSeries` 1.1.0 in `bench/` only.

**Spec:** [`docs/superpowers/specs/2026-09-12_0617_time-series-diagnostics.md`](../specs/2026-09-12_0617_time-series-diagnostics.md)

**Branch:** `feat/617-time-series-diagnostics`

## Global Constraints

- The code lands in `src/Lodestar.Stats/TimeSeries/`, namespace `Lodestar.Stats.TimeSeries`, **provisionally**. Task 6 measures it and an ADR decides whether it stays; no task before 6 may assume either answer.
- Core tier: **no external dependency**. `tools/check_nuspec_dependencies.py` enforces it.
- Both target frameworks `net10.0;netstandard2.0`, one public API, no reduced surface, no `#if` at a call site.
- Warnings are errors; `AnalysisMode=All` with `SonarAnalyzer.CSharp` on every project.
- Comments: two lines inline, eight of XML prose, a `long-comment:` marker with a reason past that. Say why, not what.
- Oracle floats compare at `1e-9`; **p-values compare relatively**, through `StatsOracleAsserts` in `tests/Lodestar.Stats.Tests/Oracles/` — reused, never restated.
- Commit messages carry no `feat:`/`fix:` prefix. Everything in English.
- `docs/equivalence.md` rows land in the same commit as the function they describe.
- Result types are `sealed class` with `init` properties and an `internal` constructor; options types are `sealed record` with `init`. The spec's "Public surface" section has the reasoning and it is not re-litigated in a task.
- Refusals are `ArgumentException` with a lowercase sentence fragment naming the offending value, and the `ParamName` of the public parameter.

---

### Task 1: The autocovariance kernel and `Autocorrelation`

**Files:**

- Create: `src/Lodestar.Stats/TimeSeries/Internal/Autocovariance.cs`
- Create: `src/Lodestar.Stats/TimeSeries/AutocorrelationOptions.cs`
- Create: `src/Lodestar.Stats/TimeSeries/AutocorrelationResult.cs`
- Create: `src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs`
- Test: `tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs`

**Interfaces:**

- Consumes: `Lodestar.Stats.Distributions.NormalQuantile(double p)`, already public.
- Produces: `Autocovariance.Of(ReadOnlySpan<double> series, int lagCount, bool adjusted)` returning `double[]` of length `lagCount + 1`; `SerialCorrelation.Autocorrelation(ReadOnlySpan<double>, int, AutocorrelationOptions?)` returning `AutocorrelationResult`; `AutocorrelationResult.Values/ConfidenceLower/ConfidenceUpper` as `IReadOnlyList<double>`; `SerialCorrelation.RefuseUnusableSeries(ReadOnlySpan<double> series, int lagCount, int lagCeiling)` as an internal shared guard Tasks 2 and 3 call.

- [ ] **Step 1: Write the failing test**

`tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs`:

```csharp
using Lodestar.Stats.TimeSeries;
using Xunit;

namespace Lodestar.Stats.Tests;

public sealed class SerialCorrelationEdgeTests
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0];

    [Fact]
    public void Lag_zero_is_exactly_one_and_its_interval_is_a_point()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, lagCount: 4);

        Assert.Equal(1.0, result.Values[0]);
        Assert.Equal(1.0, result.ConfidenceLower[0]);
        Assert.Equal(1.0, result.ConfidenceUpper[0]);
        Assert.Equal(5, result.Values.Count);
    }

    [Fact]
    public void A_series_shorter_than_two_points_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([1.0], lagCount: 1));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("two points", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_constant_series_is_refused_rather_than_divided_by_zero()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([2.0, 2.0, 2.0, 2.0], lagCount: 2));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("every value is", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_non_finite_value_is_refused(double value)
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation([1.0, 2.0, value, 4.0], lagCount: 2));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("row 2", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void A_lag_count_below_one_is_refused(int lagCount)
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation(Series, lagCount));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Fact]
    public void A_lag_count_reaching_the_series_length_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.Autocorrelation(Series, lagCount: 10));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new AutocorrelationOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void The_adjusted_estimator_divides_by_a_smaller_denominator()
    {
        AutocorrelationResult biased = SerialCorrelation.Autocorrelation(Series, 4);
        AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
            Series, 4, new AutocorrelationOptions { Adjusted = true });

        // n / (n - k) > 1 at every lag past zero, and the ratio is exactly that factor.
        for (int lag = 1; lag <= 4; lag++)
        {
            Assert.Equal(
                biased.Values[lag] * (Series.Length / (double)(Series.Length - lag)),
                adjusted.Values[lag],
                12);
        }
    }

    [Fact]
    public void The_flat_band_is_the_same_width_at_every_lag()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(
            Series, 4, new AutocorrelationOptions { BartlettConfidenceInterval = false });

        double width = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            Assert.Equal(width, result.ConfidenceUpper[lag] - result.ConfidenceLower[lag], 12);
        }
    }

    [Fact]
    public void The_bartlett_band_widens_with_the_lag()
    {
        AutocorrelationResult result = SerialCorrelation.Autocorrelation(Series, 4);

        double previous = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            double width = result.ConfidenceUpper[lag] - result.ConfidenceLower[lag];
            Assert.True(width >= previous, $"lag {lag}: {width} is not at least {previous}.");
            previous = width;
        }
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: FAIL to compile — `Lodestar.Stats.TimeSeries` does not exist. A compile failure is the correct first failure here; do not stub types to make it a runtime one.

- [ ] **Step 3: Write the autocovariance kernel**

`src/Lodestar.Stats/TimeSeries/Internal/Autocovariance.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The sample autocovariance the three diagnostics all start from.</summary>
internal static class Autocovariance
{
    /// <summary>Autocovariance at lags 0 through <paramref name="lagCount"/>, inclusive.</summary>
    /// <remarks>
    /// <paramref name="adjusted"/> false divides every lag by <c>n</c>, which is the reference's
    /// default and the estimator that keeps the sequence positive semi-definite; true divides lag
    /// <c>k</c> by <c>n - k</c> instead. Lag zero divides by <c>n</c> either way, because
    /// <c>n - 0</c> is <c>n</c>.
    /// </remarks>
    internal static double[] Of(ReadOnlySpan<double> series, int lagCount, bool adjusted)
    {
        int n = series.Length;
        double mean = 0.0;
        for (int i = 0; i < n; i++)
        {
            mean += series[i];
        }

        mean /= n;

        var result = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            double total = 0.0;
            for (int t = 0; t + lag < n; t++)
            {
                total += (series[t] - mean) * (series[t + lag] - mean);
            }

            result[lag] = total / (adjusted ? n - lag : n);
        }

        return result;
    }
}
```

- [ ] **Step 4: Write the options and the result**

`src/Lodestar.Stats/TimeSeries/AutocorrelationOptions.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>What an autocorrelation may be told.</summary>
/// <remarks>
/// Checked where it is set rather than where it is read: a confidence level read three functions
/// later reaches the caller as a band of the wrong width instead of an exception naming it (#617).
/// </remarks>
public sealed record AutocorrelationOptions
{
    private double _confidenceLevel = 0.95;

    /// <summary>
    /// Whether lag <c>k</c> divides by <c>n - k</c> rather than by <c>n</c>. Default false, which
    /// is the reference's, and the estimator that keeps the sequence positive semi-definite.
    /// </summary>
    public bool Adjusted { get; init; }

    /// <summary>
    /// Whether the band widens with the lag by Bartlett's formula. Default true, which is the
    /// reference's. False gives the flat band a correlogram usually draws.
    /// </summary>
    /// <remarks>
    /// <see cref="SerialCorrelation.PartialAutocorrelation"/> ignores this: its band is flat at
    /// <c>1/n</c> whatever it says, because Bartlett's formula is about an autocorrelation.
    /// </remarks>
    public bool BartlettConfidenceInterval { get; init; } = true;

    /// <summary>The two-sided level the band is reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }
}
```

`src/Lodestar.Stats/TimeSeries/AutocorrelationResult.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>An autocorrelation sequence and the band around it, indexed by lag.</summary>
/// <remarks>
/// A class rather than a record: a record's equality compares these three lists by reference, so
/// two results holding the same numbers would compare unequal
/// ([#668](https://github.com/CyrilB1531/lodestar/issues/668)). Nobody compares two correlograms,
/// so this promises no equality at all rather than a broken one.
/// </remarks>
public sealed class AutocorrelationResult
{
    internal AutocorrelationResult()
    {
    }

    /// <summary>The correlation at each lag, from 0 — where it is always 1 — upwards.</summary>
    public IReadOnlyList<double> Values { get; init; } = [];

    /// <summary>The lower end of the band, centred on <see cref="Values"/> rather than on zero.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of the band.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];
}
```

- [ ] **Step 5: Write the guards and `Autocorrelation`**

`src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs`:

```csharp
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a series carries serial dependence, and at which lags.</summary>
public static class SerialCorrelation
{
    /// <summary>The autocorrelation function, with its confidence band.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lagCount">
    /// How many lags past zero to report. Required rather than defaulted: the reference defaults
    /// it to <c>min(10*log10(n), n - 1)</c> here and to <c>min(10*log10(n), n/2 - 1)</c> for the
    /// partial function, and a default that differs between two functions read side by side is a
    /// trap. Pass either rule deliberately to reproduce a reference plot.
    /// </param>
    /// <param name="options">The estimator, the band and its level, or null for the defaults.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> holds fewer than two points, is constant, or carries a non-finite
    /// value; or <paramref name="lagCount"/> is below one or reaches the series length.
    /// </exception>
    public static AutocorrelationResult Autocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null)
    {
        AutocorrelationOptions settings = options ?? new AutocorrelationOptions();
        RefuseUnusableSeries(series, lagCount, series.Length - 1);

        // Lag zero divides by n - 0, which is n, whichever flag was passed -- so the adjusted
        // estimator is a bigger numerator over the same denominator, as the reference's is.
        double[] covariance = Autocovariance.Of(series, lagCount, settings.Adjusted);

        var values = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            values[lag] = covariance[lag] / covariance[0];
        }

        double multiplier = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));
        double[] variance = BandVariance(values, series.Length, settings.BartlettConfidenceInterval);

        var lower = new double[lagCount + 1];
        var upper = new double[lagCount + 1];
        for (int lag = 0; lag <= lagCount; lag++)
        {
            double half = multiplier * Math.Sqrt(variance[lag]);
            lower[lag] = values[lag] - half;
            upper[lag] = values[lag] + half;
        }

        return new AutocorrelationResult
        {
            Values = values,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
        };
    }

    /// <summary>Bartlett's widening variance, or the flat one.</summary>
    /// <remarks>
    /// Bartlett's is <c>(1 + 2*sum_{j&lt;k} r_j^2) / n</c> past lag one, which is the reference's
    /// default. Lag zero is exactly zero either way, so its interval is the point 1.
    /// </remarks>
    private static double[] BandVariance(double[] values, int n, bool bartlett)
    {
        var variance = new double[values.Length];
        if (!bartlett)
        {
            for (int lag = 1; lag < values.Length; lag++)
            {
                variance[lag] = 1.0 / n;
            }

            return variance;
        }

        double running = 0.0;
        for (int lag = 1; lag < values.Length; lag++)
        {
            variance[lag] = (1.0 + (2.0 * running)) / n;
            running += values[lag] * values[lag];
        }

        return variance;
    }

    /// <summary>The refusals every member of this class shares.</summary>
    /// <param name="series">The observations.</param>
    /// <param name="lagCount">The requested lag count.</param>
    /// <param name="lagCeiling">The largest lag this member can answer for.</param>
    internal static void RefuseUnusableSeries(
        ReadOnlySpan<double> series, int lagCount, int lagCeiling)
    {
        if (series.Length < 2)
        {
            throw new ArgumentException(
                $"a series of {series.Length} carries no correlation: two points are the "
                + "minimum.", nameof(series));
        }

        for (int row = 0; row < series.Length; row++)
        {
            // double.IsFinite is not on netstandard2.0, so the two halves are asked separately --
            // Lodestar.Stats.Regression's Irls.AllFinite records the same constraint.
            if (double.IsNaN(series[row]) || double.IsInfinity(series[row]))
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row]}, which would propagate through every lag. "
                    + "A gapped series needs the interpolation this does not do.", nameof(series));
            }
        }

        // S1244: a constant series is exactly constant or it is not -- a tolerance band here
        // would refuse a series that merely varies little, which is a different thing.
#pragma warning disable S1244
        bool constant = true;
        for (int row = 1; row < series.Length && constant; row++)
        {
            constant = series[row] == series[0];
        }
#pragma warning restore S1244

        if (constant)
        {
            throw new ArgumentException(
                $"every value is {series[0]}, so the lag-zero autocovariance is zero and every "
                + "correlation would be 0/0. The reference answers NaN; this refuses, as "
                + "KruskalWallis.Test refuses a fully tied sample.", nameof(series));
        }

        if (lagCount < 1)
        {
            throw new ArgumentException(
                $"a lag count of {lagCount} asks for nothing: one or more.", nameof(lagCount));
        }

        if (lagCount > lagCeiling)
        {
            throw new ArgumentException(
                $"a lag count of {lagCount} is above the {lagCeiling} a series of "
                + $"{series.Length} supports.", nameof(lagCount));
        }
    }
}
```

- [ ] **Step 6: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: PASS, 12 tests. **Read the count, not the colour** — a `--filter` matching nothing exits 8 under Microsoft.Testing.Platform, but a typo that matches only half the class still exits 0.

- [ ] **Step 7: Run the whole suite and the offline guards**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
sh .githooks/pre-commit
```

Expected: 32 assemblies, no new failures, guards clean. Run `.githooks/pre-commit` on its own line, never chained behind `git commit` with `&&`.

- [ ] **Step 8: Commit**

```bash
git add src/Lodestar.Stats/TimeSeries tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs
git commit -m "Compute the autocorrelation function, with Bartlett's band and the flat one"
```

---

### Task 2: `PartialAutocorrelation`

**Files:**

- Create: `src/Lodestar.Stats/TimeSeries/Internal/LevinsonDurbin.cs`
- Modify: `src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs` (add one public method)
- Test: `tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs` (add cases)

**Interfaces:**

- Consumes: `Autocovariance.Of(ReadOnlySpan<double>, int, bool)`, `SerialCorrelation.RefuseUnusableSeries(ReadOnlySpan<double>, int, int)`, `AutocorrelationResult`, `AutocorrelationOptions.ConfidenceLevel`.
- Produces: `LevinsonDurbin.ReflectionCoefficients(double[] covariance, int order)` returning `double[]` of length `order + 1` with `[0] == 1.0`; `SerialCorrelation.PartialAutocorrelation(ReadOnlySpan<double>, int, AutocorrelationOptions?)`.

- [ ] **Step 1: Write the failing tests**

Append to `tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs`, inside the class:

```csharp
    [Fact]
    public void The_partial_function_reports_lag_zero_as_one()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 4);

        Assert.Equal(1.0, result.Values[0]);
        Assert.Equal(1.0, result.ConfidenceLower[0]);
        Assert.Equal(1.0, result.ConfidenceUpper[0]);
    }

    [Fact]
    public void The_first_partial_coefficient_equals_the_first_autocorrelation()
    {
        // Levinson-Durbin's first reflection coefficient is r1/r0 by construction, and the
        // adjusted autocorrelation is the same ratio: numerator over n - 1, denominator over n.
        AutocorrelationResult partial =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 3);
        AutocorrelationResult adjusted = SerialCorrelation.Autocorrelation(
            Series, 3, new AutocorrelationOptions { Adjusted = true });

        Assert.Equal(adjusted.Values[1], partial.Values[1], 12);
    }

    [Fact]
    public void The_partial_band_is_flat_even_though_the_autocorrelation_band_is_not()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 4);

        double width = result.ConfidenceUpper[1] - result.ConfidenceLower[1];
        for (int lag = 2; lag <= 4; lag++)
        {
            Assert.Equal(width, result.ConfidenceUpper[lag] - result.ConfidenceLower[lag], 12);
        }
    }

    [Fact]
    public void A_partial_lag_count_past_half_the_series_is_refused()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.PartialAutocorrelation(Series, lagCount: 6));

        Assert.Equal("lagCount", refusal.ParamName);
    }

    [Fact]
    public void A_partial_lag_count_at_half_the_series_is_allowed()
    {
        AutocorrelationResult result =
            SerialCorrelation.PartialAutocorrelation(Series, lagCount: 5);

        Assert.Equal(6, result.Values.Count);
    }
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: FAIL to compile — `PartialAutocorrelation` does not exist.

- [ ] **Step 3: Write the recursion**

`src/Lodestar.Stats/TimeSeries/Internal/LevinsonDurbin.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The Levinson-Durbin recursion, whose reflection coefficients are the partial
/// autocorrelations.</summary>
/// <remarks>
/// The reference solves a separate Yule-Walker system per order and keeps each solution's last
/// coefficient, which is quartic in the order. This recursion produces the same numbers in
/// <c>O(order^2)</c>, because solving order <c>k</c> from order <c>k - 1</c> is what it does --
/// the corpus in task 5 is what proves the two agree rather than this sentence.
/// </remarks>
internal static class LevinsonDurbin
{
    /// <summary>The reflection coefficients of an autocovariance sequence.</summary>
    /// <param name="covariance">Autocovariance at lags 0 through <paramref name="order"/>.</param>
    /// <param name="order">The highest lag to report.</param>
    /// <returns>Length <paramref name="order"/> + 1, with index 0 fixed at 1.</returns>
    internal static double[] ReflectionCoefficients(double[] covariance, int order)
    {
        var reflection = new double[order + 1];
        reflection[0] = 1.0;

        var coefficients = new double[order + 1];
        var previous = new double[order + 1];
        double error = covariance[0];

        for (int k = 1; k <= order; k++)
        {
            double numerator = covariance[k];
            for (int j = 1; j < k; j++)
            {
                numerator -= previous[j] * covariance[k - j];
            }

            double current = numerator / error;
            reflection[k] = current;
            coefficients[k] = current;
            for (int j = 1; j < k; j++)
            {
                coefficients[j] = previous[j] - (current * previous[k - j]);
            }

            error *= 1.0 - (current * current);
            Array.Copy(coefficients, previous, k + 1);
        }

        return reflection;
    }
}
```

- [ ] **Step 4: Write the public method**

Add to `src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs`, after `Autocorrelation`:

```csharp
    /// <summary>The partial autocorrelation function, with its confidence band.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lagCount">
    /// How many lags past zero to report, at most half the series length. The reference defaults
    /// it to <c>min(10*log10(n), n/2 - 1)</c>; this asks rather than defaulting, for the reason
    /// <see cref="Autocorrelation"/> gives.
    /// </param>
    /// <param name="options">
    /// Only <see cref="AutocorrelationOptions.ConfidenceLevel"/> is read. <c>Adjusted</c> is fixed
    /// here — the reference's <c>ywadjusted</c> method is the adjusted estimator by definition —
    /// and Bartlett's formula does not apply to a partial autocorrelation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> holds fewer than two points, is constant, or carries a non-finite
    /// value; or <paramref name="lagCount"/> is below one or above half the series length.
    /// </exception>
    public static AutocorrelationResult PartialAutocorrelation(
        ReadOnlySpan<double> series, int lagCount, AutocorrelationOptions? options = null)
    {
        AutocorrelationOptions settings = options ?? new AutocorrelationOptions();
        RefuseUnusableSeries(series, lagCount, series.Length / 2);

        double[] covariance = Autocovariance.Of(series, lagCount, adjusted: true);
        double[] values = LevinsonDurbin.ReflectionCoefficients(covariance, lagCount);

        // Quenouille: a partial autocorrelation past the true order is asymptotically N(0, 1/n),
        // so the band is flat rather than Bartlett's widening one.
        double half = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0)) / Math.Sqrt(series.Length);

        var lower = new double[lagCount + 1];
        var upper = new double[lagCount + 1];
        lower[0] = values[0];
        upper[0] = values[0];
        for (int lag = 1; lag <= lagCount; lag++)
        {
            lower[lag] = values[lag] - half;
            upper[lag] = values[lag] + half;
        }

        return new AutocorrelationResult
        {
            Values = values,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
        };
    }
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: PASS, 17 tests.

- [ ] **Step 6: Run the whole suite and the guards**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
sh .githooks/pre-commit
```

Expected: 32 assemblies, guards clean.

- [ ] **Step 7: Commit**

```bash
git add src/Lodestar.Stats/TimeSeries tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs
git commit -m "Compute the partial autocorrelation by the Levinson-Durbin recursion"
```

---

### Task 3: `LjungBox`

**Files:**

- Create: `src/Lodestar.Stats/TimeSeries/LjungBoxOptions.cs`
- Create: `src/Lodestar.Stats/TimeSeries/LjungBoxResult.cs`
- Modify: `src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs` (add one public method)
- Test: `tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs` (add cases)

**Interfaces:**

- Consumes: `Autocovariance.Of`, `SerialCorrelation.RefuseUnusableSeries`, `Lodestar.Stats.Distributions.ChiSquaredSf(double x, double df)`.
- Produces: `SerialCorrelation.LjungBox(ReadOnlySpan<double>, int, LjungBoxOptions?)` returning `LjungBoxResult`, whose five lists are `Statistics`, `PValues`, `BoxPierceStatistics`, `BoxPiercePValues` (all `IReadOnlyList<double>`) and `DegreesOfFreedom` (`IReadOnlyList<int>`), each of length `lagCount` and indexed from lag 1.

- [ ] **Step 1: Write the failing tests**

Append to `tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs`, inside the class:

```csharp
    [Fact]
    public void The_statistic_is_indexed_from_lag_one_and_never_decreases()
    {
        LjungBoxResult result = SerialCorrelation.LjungBox(Series, lagCount: 4);

        Assert.Equal(4, result.Statistics.Count);
        Assert.Equal(4, result.PValues.Count);
        Assert.Equal([1, 2, 3, 4], result.DegreesOfFreedom);
        for (int i = 1; i < result.Statistics.Count; i++)
        {
            Assert.True(
                result.Statistics[i] >= result.Statistics[i - 1],
                $"lag {i + 1}: {result.Statistics[i]} is below {result.Statistics[i - 1]}.");
        }
    }

    [Fact]
    public void Box_pierce_is_empty_unless_it_is_asked_for()
    {
        LjungBoxResult without = SerialCorrelation.LjungBox(Series, lagCount: 3);
        LjungBoxResult with = SerialCorrelation.LjungBox(
            Series, 3, new LjungBoxOptions { BoxPierce = true });

        Assert.Empty(without.BoxPierceStatistics);
        Assert.Empty(without.BoxPiercePValues);
        Assert.Equal(3, with.BoxPierceStatistics.Count);
        Assert.Equal(3, with.BoxPiercePValues.Count);

        // Ljung-Box weights each squared correlation by n/(n - k), so it is the larger of the two.
        for (int i = 0; i < 3; i++)
        {
            Assert.True(with.Statistics[i] >= with.BoxPierceStatistics[i]);
        }
    }

    [Fact]
    public void A_model_degrees_of_freedom_that_swallows_a_lag_gives_NaN_at_that_lag_only()
    {
        LjungBoxResult result = SerialCorrelation.LjungBox(
            Series, 4, new LjungBoxOptions { ModelDegreesOfFreedom = 2 });

        Assert.Equal(double.NaN, result.PValues[0]);
        Assert.Equal(double.NaN, result.PValues[1]);
        Assert.False(double.IsNaN(result.PValues[2]));
        Assert.Equal([-1, 0, 1, 2], result.DegreesOfFreedom);
        Assert.False(double.IsNaN(result.Statistics[0]));
    }

    [Fact]
    public void A_negative_model_degrees_of_freedom_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new LjungBoxOptions { ModelDegreesOfFreedom = -1 });
    }

    [Fact]
    public void A_constant_series_is_refused_by_ljung_box_too()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => SerialCorrelation.LjungBox([3.0, 3.0, 3.0, 3.0], lagCount: 2));

        Assert.Equal("series", refusal.ParamName);
    }
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: FAIL to compile — `LjungBox` does not exist.

- [ ] **Step 3: Write the options and the result**

`src/Lodestar.Stats/TimeSeries/LjungBoxOptions.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>What a Ljung-Box test may be told.</summary>
public sealed record LjungBoxOptions
{
    private int _modelDegreesOfFreedom;

    /// <summary>
    /// How many parameters the model whose residuals these are consumed. Default 0, for a raw
    /// series. Each lag's degrees of freedom is its own lag less this.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int ModelDegreesOfFreedom
    {
        get => _modelDegreesOfFreedom;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A model cannot consume a negative number of parameters.");
            }

            _modelDegreesOfFreedom = value;
        }
    }

    /// <summary>Whether the Box-Pierce statistic is reported beside Ljung-Box. Default false.</summary>
    /// <remarks>
    /// Box-Pierce is the same sum without the <c>n/(n - k)</c> weight, so it is the smaller of the
    /// two and the less powerful in a short series. It is here because the reference offers it and
    /// a reader comparing an old paper's numbers needs it.
    /// </remarks>
    public bool BoxPierce { get; init; }
}
```

`src/Lodestar.Stats/TimeSeries/LjungBoxResult.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>A Ljung-Box test at each lag, indexed from lag 1.</summary>
/// <remarks>
/// Five parallel lists rather than a list of five-field rows, because a caller plots a column.
/// A class rather than a record, for the reason <see cref="AutocorrelationResult"/> gives.
/// </remarks>
public sealed class LjungBoxResult
{
    internal LjungBoxResult()
    {
    }

    /// <summary>The Ljung-Box statistic cumulated to each lag.</summary>
    public IReadOnlyList<double> Statistics { get; init; } = [];

    /// <summary>Its chi-square p-value, NaN where no degree of freedom is left.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The Box-Pierce statistic, empty unless <see cref="LjungBoxOptions.BoxPierce"/>.</summary>
    public IReadOnlyList<double> BoxPierceStatistics { get; init; } = [];

    /// <summary>Box-Pierce's p-value, empty unless it was asked for.</summary>
    public IReadOnlyList<double> BoxPiercePValues { get; init; } = [];

    /// <summary>The lag less the model's parameters, which may be zero or negative.</summary>
    public IReadOnlyList<int> DegreesOfFreedom { get; init; } = [];
}
```

- [ ] **Step 4: Write the public method**

Add to `src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs`, after `PartialAutocorrelation`:

```csharp
    /// <summary>The Ljung-Box test for serial dependence, cumulated lag by lag.</summary>
    /// <param name="series">The observations, or a model's residuals, in time order.</param>
    /// <param name="lagCount">The highest lag to test, at most one below the series length.</param>
    /// <param name="options">The model's parameter count and whether Box-Pierce comes too.</param>
    /// <returns>Five lists of length <paramref name="lagCount"/>, indexed from lag 1.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> holds fewer than two points, is constant, or carries a non-finite
    /// value; or <paramref name="lagCount"/> is below one or reaches the series length.
    /// </exception>
    public static LjungBoxResult LjungBox(
        ReadOnlySpan<double> series, int lagCount, LjungBoxOptions? options = null)
    {
        LjungBoxOptions settings = options ?? new LjungBoxOptions();
        RefuseUnusableSeries(series, lagCount, series.Length - 1);

        int n = series.Length;
        double[] covariance = Autocovariance.Of(series, lagCount, adjusted: false);

        var statistics = new double[lagCount];
        var pValues = new double[lagCount];
        var degreesOfFreedom = new int[lagCount];
        double[] boxPierce = settings.BoxPierce ? new double[lagCount] : [];
        double[] boxPierceP = settings.BoxPierce ? new double[lagCount] : [];

        double ljung = 0.0;
        double pierce = 0.0;
        for (int lag = 1; lag <= lagCount; lag++)
        {
            double r = covariance[lag] / covariance[0];
            ljung += r * r / (n - lag);
            pierce += r * r;

            int index = lag - 1;
            int df = lag - settings.ModelDegreesOfFreedom;
            statistics[index] = n * (n + 2) * ljung;
            degreesOfFreedom[index] = df;
            pValues[index] = df > 0
                ? Distributions.ChiSquaredSf(statistics[index], df)
                : double.NaN;

            if (settings.BoxPierce)
            {
                boxPierce[index] = n * pierce;
                boxPierceP[index] = df > 0
                    ? Distributions.ChiSquaredSf(boxPierce[index], df)
                    : double.NaN;
            }
        }

        return new LjungBoxResult
        {
            Statistics = statistics,
            PValues = pValues,
            BoxPierceStatistics = boxPierce,
            BoxPiercePValues = boxPierceP,
            DegreesOfFreedom = degreesOfFreedom,
        };
    }
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationEdgeTests"
```

Expected: PASS, 22 tests.

- [ ] **Step 6: Run the whole suite and the guards**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
sh .githooks/pre-commit
```

Expected: 32 assemblies, guards clean.

- [ ] **Step 7: Commit**

```bash
git add src/Lodestar.Stats/TimeSeries tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs
git commit -m "Test a series for serial dependence with Ljung-Box, and Box-Pierce beside it"
```

---

### Task 4: The oracle corpus

**Files:**

- Modify: `tools/generate_oracles.py` (add two functions and one registry row)
- Create: `tests/oracles/stats_timeseries.json` (generated, committed)

**Interfaces:**

- Produces: `tests/oracles/stats_timeseries.json` with `metadata.count` and a `cases` array. Each case carries `name`, `call`, `series`, `lag_count`, and the frozen fields the call returns. Task 5 reads exactly these keys.

- [ ] **Step 1: Add the fixtures**

In `tools/generate_oracles.py`, beside the other `_stats_*` fixture helpers:

```python
def _timeseries_fixtures() -> list[dict]:
    """Series whose correlograms differ in ways a wrong implementation cannot fake."""
    rng = SeededRandom(SEED + 617)

    ar1 = [0.0]
    for _ in range(39):
        ar1.append(0.7 * ar1[-1] + rng.gauss(0.0, 1.0))

    shocks = [rng.gauss(0.0, 1.0) for _ in range(61)]
    ma1 = [shocks[i] + 0.6 * shocks[i - 1] for i in range(1, 61)]

    noise = [rng.gauss(0.0, 1.0) for _ in range(100)]
    trend = [0.05 * i + rng.gauss(0.0, 0.3) for i in range(80)]
    seasonal = [
        math.sin(2.0 * math.pi * i / 12.0) + rng.gauss(0.0, 0.2) for i in range(96)
    ]

    return [
        {"name": "AR(1) at 0.7, 40 points", SERIES: [round(v, 10) for v in ar1], LAG_COUNT: 8},
        {"name": "MA(1) at 0.6, 60 points", SERIES: [round(v, 10) for v in ma1], LAG_COUNT: 8},
        {"name": "white noise, 100 points", SERIES: [round(v, 10) for v in noise], LAG_COUNT: 10},
        {"name": "linear trend, 80 points", SERIES: [round(v, 10) for v in trend], LAG_COUNT: 10},
        {"name": "seasonal period 12, 96 points",
         SERIES: [round(v, 10) for v in seasonal], LAG_COUNT: 14},
        {"name": "short series, 12 points", SERIES: [round(v, 10) for v in noise[:12]],
         LAG_COUNT: 5},
    ]
```

Add the two string constants beside the file's existing ones, so `check_repeated_literals.py` does not fail on the branch — it is **not** in the pre-commit hook and has caught this repository twice:

```python
SERIES = "series"
LAG_COUNT = "lag_count"
BARTLETT = "bartlett"
```

- [ ] **Step 2: Add the generator**

```python
def generate_stats_timeseries() -> dict:
    """ACF, PACF and Ljung-Box, against statsmodels 0.15.0 (#617)."""
    import numpy as np
    from statsmodels.stats.diagnostic import acorr_ljungbox
    from statsmodels.tsa.stattools import acf, pacf

    cases: list[dict] = []
    for fx in _timeseries_fixtures():
        x = np.array(fx[SERIES])
        lags = fx[LAG_COUNT]

        for level in (0.95, 0.99):
            alpha = round(1.0 - level, 10)
            for adjusted in (False, True):
                for bartlett in (True, False):
                    values, confint = acf(
                        x, nlags=lags, adjusted=adjusted, fft=False,
                        alpha=alpha, bartlett_confint=bartlett)
                    cases.append({
                        "name": f"{fx['name']} | acf | {level} | "
                                f"adjusted={adjusted} | bartlett={bartlett}",
                        "call": "acf",
                        SERIES: fx[SERIES], LAG_COUNT: lags,
                        "level": level, "adjusted": adjusted, BARTLETT: bartlett,
                        "values": [float(v) for v in values],
                        "lower": [float(row[0]) for row in confint],
                        "upper": [float(row[1]) for row in confint],
                    })

            pvalues, pconfint = pacf(x, nlags=min(lags, len(x) // 2), alpha=alpha)
            cases.append({
                "name": f"{fx['name']} | pacf | {level}",
                "call": "pacf",
                SERIES: fx[SERIES], LAG_COUNT: min(lags, len(x) // 2),
                "level": level,
                "values": [float(v) for v in pvalues],
                "lower": [float(row[0]) for row in pconfint],
                "upper": [float(row[1]) for row in pconfint],
            })

        for model_df in (0, 2):
            frame = acorr_ljungbox(
                x, lags=list(range(1, lags + 1)), model_df=model_df, boxpierce=True)
            cases.append({
                "name": f"{fx['name']} | ljungbox | model_df={model_df}",
                "call": "acorr_ljungbox",
                SERIES: fx[SERIES], LAG_COUNT: lags, "model_df": model_df,
                "statistics": [float(v) for v in frame["lb_stat"]],
                "pvalues": [float(v) for v in frame["lb_pvalue"]],
                "bp_statistics": [float(v) for v in frame["bp_stat"]],
                "bp_pvalues": [float(v) for v in frame["bp_pvalue"]],
            })

    return {"metadata": _stats_metadata("timeseries", len(cases)), CASES: cases}
```

- [ ] **Step 3: Register it**

In the generator registry, beside `"stats_glm.json": generate_stats_glm,`:

```python
        "stats_timeseries.json": generate_stats_timeseries,
```

- [ ] **Step 4: Generate, from a neutral directory**

```bash
repo=$(git rev-parse --show-toplevel)
cd /var/tmp && PYTHONSAFEPATH=1 "$repo/.venv-oracles/bin/python" "$repo/tools/generate_oracles.py"
```

Expected: exit 0 and `tests/oracles/stats_timeseries.json` written. **Read the generator's own exit code**, never a pipeline's — `python … | tail` reports `tail`'s status and a failed generation then looks successful. Run it from `/var/tmp` rather than the repository, because `nltk` refuses to import under the checkout.

If the interpreter is missing or stale, rebuild it from the lock as CONTRIBUTING's *Oracle validation* step 2 describes; it must be 3.12 or later.

- [ ] **Step 5: Check nothing else moved**

```bash
git status --porcelain tests/oracles/
```

Expected: exactly one new file, `stats_timeseries.json`. Any other corpus showing as modified means the generator's shared seed moved, which is a defect in this task rather than an update to commit.

- [ ] **Step 6: Check the repeated-literal guard**

```bash
python3 tools/check_repeated_literals.py --base origin/main
```

Expected: exit 0. It is not part of `.githooks/pre-commit`, so it must be run separately; a failure means a string this task repeats needs the named constant from Step 1.

- [ ] **Step 7: Commit**

```bash
git add tools/generate_oracles.py tests/oracles/stats_timeseries.json
git commit -m "Freeze the autocorrelation and Ljung-Box corpus from statsmodels"
```

---

### Task 5: Replay the corpus

**Files:**

- Create: `tests/Lodestar.Stats.Tests/SerialCorrelationOracleTests.cs`

**Interfaces:**

- Consumes: `StatsCorpus.Load(string)`, `StatsCorpus.Doubles(JsonElement)`, `StatsOracleAsserts.Statistic(double, double, string)`, `StatsOracleAsserts.PValue(double, double, string)`, all already in `tests/Lodestar.Stats.Tests/Oracles/`; the three public methods from Tasks 1 to 3.

- [ ] **Step 1: Write the failing test**

`tests/Lodestar.Stats.Tests/SerialCorrelationOracleTests.cs`:

```csharp
using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Lodestar.Stats.TimeSeries;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_timeseries.json</c>.</summary>
public sealed class SerialCorrelationOracleTests
{
    [Fact]
    public void Every_case_matches_statsmodels()
    {
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            double[] series = StatsCorpus.Doubles(c.GetProperty("series"));
            int lagCount = c.GetProperty("lag_count").GetInt32();

            switch (c.GetProperty("call").GetString())
            {
                case "acf":
                    ReplayBand(
                        c, name, SerialCorrelation.Autocorrelation(
                            series, lagCount, new AutocorrelationOptions
                            {
                                ConfidenceLevel = c.GetProperty("level").GetDouble(),
                                Adjusted = c.GetProperty("adjusted").GetBoolean(),
                                BartlettConfidenceInterval = c.GetProperty("bartlett").GetBoolean(),
                            }));
                    break;

                case "pacf":
                    ReplayBand(
                        c, name, SerialCorrelation.PartialAutocorrelation(
                            series, lagCount, new AutocorrelationOptions
                            {
                                ConfidenceLevel = c.GetProperty("level").GetDouble(),
                            }));
                    break;

                default:
                    ReplayLjungBox(
                        c, name, SerialCorrelation.LjungBox(
                            series, lagCount, new LjungBoxOptions
                            {
                                ModelDegreesOfFreedom = c.GetProperty("model_df").GetInt32(),
                                BoxPierce = true,
                            }));
                    break;
            }

            replayed++;
        }

        Assert.Equal(
            document.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(),
            replayed);
    }

    private static void ReplayBand(JsonElement c, string name, AutocorrelationResult result)
    {
        Compare(c.GetProperty("values"), result.Values, $"{name} values");
        Compare(c.GetProperty("lower"), result.ConfidenceLower, $"{name} lower");
        Compare(c.GetProperty("upper"), result.ConfidenceUpper, $"{name} upper");
    }

    private static void ReplayLjungBox(JsonElement c, string name, LjungBoxResult result)
    {
        Compare(c.GetProperty("statistics"), result.Statistics, $"{name} statistic");
        Compare(c.GetProperty("bp_statistics"), result.BoxPierceStatistics, $"{name} bp statistic");
        ComparePValues(c.GetProperty("pvalues"), result.PValues, $"{name} p");
        ComparePValues(c.GetProperty("bp_pvalues"), result.BoxPiercePValues, $"{name} bp p");
    }

    private static void Compare(JsonElement expected, IReadOnlyList<double> actual, string caseName)
    {
        double[] values = StatsCorpus.Doubles(expected);
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            StatsOracleAsserts.Statistic(values[i], actual[i], $"{caseName}[{i}]");
        }
    }

    private static void ComparePValues(
        JsonElement expected, IReadOnlyList<double> actual, string caseName)
    {
        double[] values = StatsCorpus.Doubles(expected);
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            StatsOracleAsserts.PValue(values[i], actual[i], $"{caseName}[{i}]");
        }
    }
}
```

- [ ] **Step 2: Add the three invariants the corpus cannot state**

Append to the same file, inside the class:

```csharp
    [Fact]
    public void An_AR_one_has_a_decaying_autocorrelation_and_a_partial_one_that_cuts_off()
    {
        // The one assertion that catches the two functions being swapped: an AR(1)'s ACF decays
        // through the band while its PACF is inside it at every lag past the first.
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");
        JsonElement c = First(document, "AR(1) at 0.7, 40 points | pacf | 0.95");
        double[] series = StatsCorpus.Doubles(c.GetProperty("series"));

        AutocorrelationResult partial = SerialCorrelation.PartialAutocorrelation(series, 8);
        AutocorrelationResult full = SerialCorrelation.Autocorrelation(series, 8);

        for (int lag = 2; lag <= 8; lag++)
        {
            Assert.True(
                Math.Abs(partial.Values[lag]) < 2.0 / Math.Sqrt(series.Length),
                $"lag {lag}: partial {partial.Values[lag]} is outside the band.");
        }

        Assert.True(full.Values[1] > 0.5, $"the lag-1 autocorrelation is {full.Values[1]}.");
    }

    [Fact]
    public void Lag_zero_is_exactly_one_on_every_corpus_series()
    {
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            double[] series = StatsCorpus.Doubles(c.GetProperty("series"));
            Assert.Equal(1.0, SerialCorrelation.Autocorrelation(series, 3).Values[0]);
            Assert.Equal(1.0, SerialCorrelation.PartialAutocorrelation(series, 3).Values[0]);
        }
    }

    private static JsonElement First(JsonDocument document, string name)
    {
        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("name").GetString() == name)
            {
                return c;
            }
        }

        throw new InvalidDataException($"No corpus case named '{name}'.");
    }
```

- [ ] **Step 3: Run the tests**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~SerialCorrelationOracleTests"
```

Expected: PASS, 3 tests.

**If a case fails, do not widen a tolerance.** The three likely causes, in the order to check them:

1. the PACF band computed with Bartlett's formula rather than flat — the 0.99 cases fail and the 0.95 ones may not;
2. the adjusted autocorrelation dividing lag zero by `n - 0` in one place and `n` in another;
3. Ljung-Box reading the adjusted autocovariance, where the reference calls `acf(..., fft=False)` with its default `adjusted=False`.

Report a failure that is none of these rather than adjusting the corpus.

- [ ] **Step 4: Run the whole suite**

```bash
dotnet test Lodestar.slnx -c Release
```

Expected: 32 assemblies; the `Lodestar.Stats.NetStandard.Tests` mirror replays the same file because it links the same sources.

- [ ] **Step 5: Commit**

```bash
git add tests/Lodestar.Stats.Tests/SerialCorrelationOracleTests.cs
git commit -m "Replay the serial-correlation corpus, and assert what it cannot say"
```

---

### Task 6: Measure the lot, and decide where it lives

**Files:**

- Create: `docs/decisions/0113-<slug decided by the verdict>.md`
- Modify: `docs/decisions/index.yaml` (generated, not hand-edited)
- Modify: `docs/decisions/README.md` (one relationship row, and the two spelled-out counts)

**Interfaces:**

- Consumes: the finished code of Tasks 1 to 3.
- Produces: the placement verdict every later task depends on. **Task 7 cannot start until this one lands**, because the version bump, the reference pages and the `wiki-map.json` entry all name a package.

- [ ] **Step 1: Measure the lot**

```bash
git diff --stat $(git merge-base origin/main HEAD)..HEAD -- src/Lodestar.Stats
grep -c 'public ' src/Lodestar.Stats/TimeSeries/*.cs
```

Record: lines added and removed under `src/`, the count of new public types, the count of new public members, and whether any neighbouring package's surface was opened. Count members the way decision 0111 did — source-declared, accessors excluded, constructors only when explicitly written.

- [ ] **Step 2: Test the measurement against decision 0076's three criteria**

Answer each in a sentence, with the measurement behind it:

- **A distinct dependency profile?** This lot uses `Distributions.NormalQuantile` and `Distributions.ChiSquaredSf`, both published by `Lodestar.Stats` itself, and nothing else. A separate package would take a dependency on `Lodestar.Stats` to reach two members it already ships.
- **A distinct audience?** State who reaches for a correlogram and whether they are the person already running `TTest` or `KruskalWallis`.
- **A distinct cadence?** State whether this lot will move on a different schedule from the hypothesis tests beside it. Note that [#671](https://github.com/CyrilB1531/lodestar/issues/671) is a second lot of comparable size, and say what it does to this answer.

- [ ] **Step 3: Write the record**

**This record is `0113`.** `main` stops at `0111` today, but `0112` is reserved for a record
landing from another branch, so do not take "the next free number" literally here.

Check the reservation still holds before you write:

```bash
git fetch origin && git rebase origin/main
ls docs/decisions/ | grep -E '^[0-9]{4}' | tail -3
```

If `0112` has arrived, `0113` is yours as planned. If `0113` has *also* arrived, take the next one
above it and say so in your report. `tools/check_adr_immutable.py --base origin/main` proves you
renumbered rather than overwrote. Title states the verdict, not the question, in this repository's house style ("Ordinary least squares earns its own package"). Frontmatter carries `status`, `supersedes`, `amends`, `applies`; `applies: ["0076", "0111"]` at minimum, since this reuses 0111's method.

Sections: Context (what #617 asked, what 0105 decided, what 0111's method is), The measurement (the numbers), Decision (in bold, with the package named if it splits), Consequences.

An ADR is immutable once merged, so get the numbers right before committing: reproduce each one rather than carrying it from Step 1's scrollback.

- [ ] **Step 4: Regenerate the index and fix the prose counts**

```bash
python3 tools/regen_adr_index.py
python3 tools/check_adr_frontmatter.py
python3 tools/check_adr_index_sync.py
```

`docs/decisions/README.md` carries **two spelled-out counts** in prose that move with every record, and a relationships row. Both counts are asserted:

```bash
.venv-oracles/bin/python -m pytest tools/tests/test_adr_count_coherence.py -q
```

Expected: 6 passed.

- [ ] **Step 5: Lint and commit**

```bash
npx markdownlint-cli2 "docs/**/*.md"
sh .githooks/pre-commit
git add docs/decisions
git commit -m "Record where the serial-correlation diagnostics live, and what was measured"
```

---

### Task 7: Documentation, sample, benchmark and the version

**Files:**

- Create: reference pages under `docs/reference/stats/timeseries/`, one per public type plus one per public method — read a neighbouring page under `docs/reference/stats/` before writing, and follow the layout that is actually there
- Modify: `docs/reference/stats.md` (index rows), `docs/wiki-map.json` (the new namespace in `covered`)
- Modify: `docs/equivalence.md` (one row per public member, plus the four divergences)
- Create: `samples/Lodestar.Sample/SerialCorrelationSample.cs`, registered in `samples/Lodestar.Sample/Program.cs`
- Create: `bench/Lodestar.Stats.Benchmarks/SerialCorrelationBenchmarks.cs`
- Modify: `bench/bench-map.json`, `bench/README.md`, `CHANGELOG.md`, and the `Version.props` of whichever package Task 6 settled on

**Interfaces:**

- Consumes: everything from Tasks 1 to 3, and Task 6's verdict for every path and package name.

- [ ] **Step 1: Write the reference pages, and let the gate find what is missing**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
```

Expected: FAIL first, naming each undocumented member; PASS once every page exists and every signature matches. Add the new namespace to `docs/wiki-map.json`'s `covered` array for this package, or the gate enforces nothing and passes vacuously.

**A `csharp` fence in a reference page is compiled and executed, and a trailing `// =>` is an assertion on the value.** Every example must be true. Run the fence's own numbers rather than transcribing them.

- [ ] **Step 2: Write the equivalence rows**

One row per public member in `docs/equivalence.md`, plus these four divergences, each stated as a divergence rather than buried:

1. **The autocovariance is a direct double sum**, where the reference's `acf` defaults to `fft=True`. An ordering difference, not a definitional one; record the measured gap on the corpus.
2. **A constant series is refused**, where the reference returns `NaN` with a warning. Cite `KruskalWallis.Test`'s precedent.
3. **`lagCount` is required**, where the reference defaults it — and to two different rules, `min(10*log10(n), n - 1)` for `acf` and `min(10*log10(n), n/2 - 1)` for `pacf`. Give both so a caller can reproduce a reference plot.
4. **`ywadjusted` is the only partial method**, where the reference offers eight more spellings across four methods.

- [ ] **Step 3: Add the sample**

`samples/Lodestar.Sample/SerialCorrelationSample.cs`:

```csharp
using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>A correlogram, and the test that says whether it matters.</summary>
internal static class SerialCorrelationSample
{
    public static void Run()
    {
        double[] series = [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0];

        AutocorrelationResult acf = SerialCorrelation.Autocorrelation(
            series, lagCount: 4, new AutocorrelationOptions { ConfidenceLevel = 0.99 });
        AutocorrelationResult pacf = SerialCorrelation.PartialAutocorrelation(series, 4);
        LjungBoxResult ljung = SerialCorrelation.LjungBox(
            series, 4, new LjungBoxOptions { BoxPierce = true });

        Console.WriteLine("SerialCorrelation (Lodestar.Stats)");
        Console.WriteLine($"  acf  lag 1      : {acf.Values[1].ToString("F4", Culture)}");
        Console.WriteLine($"  its 99% band   : [{acf.ConfidenceLower[1].ToString("F4", Culture)}"
                          + $", {acf.ConfidenceUpper[1].ToString("F4", Culture)}]");
        Console.WriteLine($"  pacf lag 1     : {pacf.Values[1].ToString("F4", Culture)}");
        Console.WriteLine($"  Ljung-Box @ 4  : {ljung.Statistics[3].ToString("F4", Culture)}");
        Console.WriteLine($"  its p-value    : {ljung.PValues[3].ToString("F4", Culture)}");
        Console.WriteLine($"  Box-Pierce @ 4 : {ljung.BoxPierceStatistics[3].ToString("F4", Culture)}");
        Console.WriteLine();
    }

    private static System.Globalization.CultureInfo Culture =>
        System.Globalization.CultureInfo.InvariantCulture;
}
```

Register it in `samples/Lodestar.Sample/Program.cs` beside its neighbours, then:

```bash
python3 tools/check_sample_culture.py
python3 tools/check_sample_coverage.py
```

Expected: both exit 0. `check_sample_coverage.py` fails while any new public type has no member referenced from the sample, which is the packaging gate's rule.

- [ ] **Step 3b: Verify the sample against the packages, not the working tree**

```bash
for p in src/Lodestar.*/; do dotnet pack "$p" -c Release -o ./artifacts; done
python3 tools/extract_doc_snippets.py
dotnet build samples/Lodestar.DocSnippets -c Release
dotnet run --project samples/Lodestar.DocSnippets -c Release
```

Expected: the snippet run reports a count and exits 0. Without a fresh `pack` the sample judges the published packages rather than this branch (ADR 0009).

- [ ] **Step 4: Add the benchmark**

`bench/Lodestar.Stats.Benchmarks/SerialCorrelationBenchmarks.cs`, with `[Params(200, 2_000)]` series lengths, a `[Benchmark(Baseline = true)]` calling `SerialCorrelation.Autocorrelation` and a `[Benchmark]` calling `Cortex.TimeSeries`' `AutocorrelationTests` where the capability overlaps. `Cortex.TimeSeries` goes in `bench/` **only** — its `Cortex.ML` edge bars it from `src/` under decision 0076, and `tools/check_nuspec_dependencies.py` is what fails if that is confused.

If `Cortex.TimeSeries` will not build against this bench project's target framework, do not fight it: ship the benchmark with our own call alone, say so in the report, and note in `bench/README.md` that the comparison is not yet available. A broken bench build is worse than a missing baseline.

```bash
python3 tools/check_bench_map.py
```

Expected: FAIL first, naming `SerialCorrelationBenchmarks` as absent from `bench/bench-map.json`; add it against `src/Lodestar.Stats/**` and re-run until clean.

Then write the `bench/README.md` section in the register of the ones beside it — how to measure, the command, what each row means. **No measured number goes in `bench/README.md`**; numbers live in `docs/guides/performance.md` with their machine, and if no measurement has been taken, the section says so.

- [ ] **Step 5: Bump the version and write the changelog**

The package is whichever Task 6 settled on. If the lot stayed in `Lodestar.Stats`, its `src/Lodestar.Stats/Version.props` takes a minor bump for new public surface with nothing removed. If it split, the new package starts at `0.1.0` and pays the fixed cost the spec lists: five pack loops, both release allow-lists, the `EXPECTED` entry in `tools/check_nuspec_dependencies.py`, the `FLOORS` row, the `*.NetStandard.Tests` mirror with every `Lodestar.*` dependency pinned by `ProjectReference` rather than `PackageReference`, and the `Lodestar.slnx` entries.

`CHANGELOG.md` under `## [Unreleased]` → the package's heading → `#### Added`, one sentence with the issue.

- [ ] **Step 6: Run every gate**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
sh .githooks/pre-commit
python3 tools/check_repeated_literals.py --base origin/main
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

Expected: all clean, `dotnet test` reporting **32 assemblies** — or 34 if Task 6 split the package and added a suite and its mirror.

- [ ] **Step 7: Commit**

```bash
git add -A
git commit -m "Document the serial-correlation diagnostics, and ship them"
```

The pull request body carries `Closes #617`. **One pull request for this issue.**

---

## Self-Review

**1. Spec coverage.** Every section of the spec maps to a task: the public surface to Tasks 1 to 3; the arithmetic read from the reference to Tasks 1 to 3 with the corpus in Task 4 as its check; the refusals table to Task 1's shared guard and its tests, with Tasks 2 and 3 calling the same guard at their own ceilings; the oracle to Task 4; testing to Task 5; placement to Task 6; benchmarks and documentation to Task 7. The spec's "What would make this spec wrong" section is answered by Task 5's three named failure causes and by the two confidence levels in Task 4's corpus.

**2. Placeholders.** None. Every code step carries the code, every command step the command and its expected output. Task 7's reference-page step names the gate that enumerates what is missing rather than listing pages this plan cannot know the names of — the layout under `docs/reference/stats/` is what decides them, and the gate fails until they exist.

**3. Type consistency.** `AutocorrelationResult` is produced by both `Autocorrelation` (Task 1) and `PartialAutocorrelation` (Task 2) and consumed by Task 5's `ReplayBand`. `LjungBoxResult`'s five members are named identically in Task 3's definition, Task 3's tests, Task 5's `ReplayLjungBox` and Task 7's sample — `BoxPiercePValues`, not `BoxPierceValues`. `RefuseUnusableSeries` takes three arguments in its definition and all three call sites. `Autocovariance.Of`'s `adjusted` argument is `false` in `Autocorrelation` and `LjungBox`, `true` in `PartialAutocorrelation`, which is what the reference does and what the corpus checks.

One thing a reader will want to "fix": `Autocorrelation` divides every lag by `covariance[0]` from the *same* array, adjusted flag included. That is correct rather than sloppy — lag zero divides by `n - 0`, which is `n`, so the adjusted estimator is a bigger numerator over an unchanged denominator, exactly as the reference's is. Task 1's `The_adjusted_estimator_divides_by_a_smaller_denominator` asserts the resulting ratio is `n / (n - k)`, which is what would break if someone normalised both sides.
