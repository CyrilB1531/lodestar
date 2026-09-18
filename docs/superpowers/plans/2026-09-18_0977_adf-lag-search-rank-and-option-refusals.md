# ADF Lag-Search Rank Check and Option Refusals Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refuse a rank-deficient candidate in the augmented Dickey-Fuller lag search, give each `MaxLag` refusal its own reason, and refuse an undeclared `KpssLagRule` where it is set.

**Architecture:** `DickeyFullerRegression.Candidates` calls `SharedReflections.RequireFullRank` once, on the widest design, after its loop, and re-raises with the ADF wording `Fit` already uses, now shared through one `RankDeficient` helper. `Stationarity.MaxLag` splits its message. `KpssOptions.LagRule` gains an `init` setter, and `Stationarity.Kpss`'s switch loses the arm it no longer needs.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform.

**Spec:** [`docs/superpowers/specs/2026-09-18_0977_adf-lag-search-rank-and-option-refusals.md`](../specs/2026-09-18_0977_adf-lag-search-rank-and-option-refusals.md)

**Branch:** `fix/977-adf-rank-check-and-minor-findings`

## Global Constraints

- Both target frameworks, one public API: no `#if` at a call site.
- Warnings are errors, and `SonarAnalyzer.CSharp` runs at `AnalysisMode=All`; S3358 refuses a nested ternary.
- Every refusal of the series names `series`, never `design`; an option setter names `value`.
- A changed behaviour lands in `docs/equivalence.md`, the reference page and `CHANGELOG.md` in the same commit.
- One commit for the branch; `Closes #977` and `Closes #984` in the pull request.

---

### Task 1: The lag search refuses a rank-deficient candidate

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Internal/DickeyFullerRegression.cs`
- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs` (the `<exception>` XML)
- Modify: `docs/reference/stats-timeseries/stationarity-tests/stationarity-augmenteddickeyfuller.md`, `docs/equivalence.md` (`adfuller` row), `CHANGELOG.md`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`

**Interfaces:**

- Consumes: `internal double[] SharedReflections.RequireFullRank(int order, string parameterName)`, which throws `ArgumentException` naming `parameterName`.
- Produces: `private static ArgumentException DickeyFullerRegression.RankDeficient(ArgumentException error, string seriesName)`.

- [ ] **Step 1: Write the failing test**

At the end of `StationarityEdgeTests`:

```csharp
    [Theory]
    [InlineData(LagSelection.Akaike)]
    [InlineData(LagSelection.Schwarz)]
    [InlineData(LagSelection.TStatistic)]
    public void The_lag_search_refuses_a_candidate_design_with_no_unique_solution(LagSelection selection)
    {
        // A line bent at its first point: not a line, but every difference the search reads is 1, so each lagged
        // difference repeats the intercept. The search answered a statistic of exactly 0 at lag 1 (#977).
        double[] bent = [.. Enumerable.Range(1, 50).Select(i => (double)i)];
        bent[0] = -5.0;

        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(bent, new DickeyFullerOptions { LagSelection = selection }));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("no unique least-squares solution", refusal.Message, StringComparison.Ordinal);
    }
```

- [ ] **Step 2: Run it to see it fail**

Run: `dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~The_lag_search_refuses"`
Expected: 3 failures, "No exception was thrown".

- [ ] **Step 3: Share the refusal and call the check**

In `DickeyFullerRegression.Fit`, replace the `throw new ArgumentException(...)` in the `catch` with `throw RankDeficient(error, nameof(series));`, and add below `Fit`:

```csharp
    /// <summary>The refusal of a lagged design with no unique solution, worded for a caller who passed only a series.</summary>
    private static ArgumentException RankDeficient(ArgumentException error, string seriesName) => new(
        "the lagged design this series builds has no unique least-squares solution: one of its columns lies "
        + "within rounding of the span of the others, as it does when the series follows a polynomial of low "
        + "degree over the rows the regression reads.",
        seriesName,
        error);
```

In `Candidates`, between the loop and `return (sums, statistics);`:

```csharp
        // The widest design alone: every candidate is a leading block of it, and a leading block's singular values
        // interlace inside the whole's, so this one full-rank answer is every lag's. Without it a rank-deficient
        // candidate took part in the search with a criterion or t statistic made of rounding (#977).
        try
        {
            _ = reflections.RequireFullRank(terms + 1 + maxLag, nameof(series));
        }
        catch (ArgumentException error)
        {
            throw RankDeficient(error, nameof(series));
        }
```

- [ ] **Step 4: Run it to see it pass**

Run: the Step 2 command. Expected: 3 passed.

- [ ] **Step 5: Say so where a reader looks**

`AugmentedDickeyFuller`'s `<exception>` and the reference page's **Exceptions** both gain "or builds a lagged design with no unique least-squares solution at the lag used or at any lag the search tries". The `adfuller` row gains, after its `#1080` citation:

```markdown
The lag search is held to the same contract: a candidate design with no unique solution is refused naming `series`, where the reference warns `SingularMatrixWarning` and ranks the candidates on their minimum-norm fits (`1.7802` at lag 3 on `1, 2, …, 50` with its first point moved to `−5`, whose differences over the search's rows are all 1) ([#977](https://github.com/CyrilB1531/lodestar/issues/977)).
```

`CHANGELOG.md`, `Lodestar.Stats.TimeSeries` → `Fixed`:

```markdown
- The augmented Dickey-Fuller lag search refuses a candidate design with no unique solution instead of ranking it on rounding, which answered a statistic of exactly 0 on a line bent at its first point. ([#977](https://github.com/CyrilB1531/lodestar/issues/977))
```

### Task 2: `MaxLag`'s refusal says which limit it hit

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs` (`MaxLag`)
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
    [Fact]
    public void A_maximum_lag_above_the_ceiling_says_so_rather_than_blaming_the_degrees_of_freedom()
    {
        // 101 points under a trend: the ceiling is 101/2 − 2 − 1 = 47, and lag 48 still leaves one degree of
        // freedom, which the single message used to deny (#984).
        double[] series = [.. Enumerable.Range(0, 101).Select(i => Math.Sin(i * 1.3) + (0.01 * i))];

        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(
                series, new DickeyFullerOptions { Regression = TrendTerms.ConstantAndTrend, MaxLag = 48 }));

        Assert.Equal("options", refusal.ParamName);
        Assert.Contains("n/2 − terms − 1 = 47", refusal.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("degree of freedom", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_series_too_short_for_any_lag_does_not_offer_a_negative_one()
    {
        // The single message printed "at most -2 does" here (#984).
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(
                [1.0, 4.0, 2.0, 8.0, 3.0],
                new DickeyFullerOptions { Regression = TrendTerms.ConstantAndQuadraticTrend, MaxLag = 3 }));

        Assert.Equal("options", refusal.ParamName);
        Assert.Contains("at any lag", refusal.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("-", refusal.Message, StringComparison.Ordinal);
    }
```

- [ ] **Step 2: Run them to see them fail**

Run: `dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~A_maximum_lag_above|FullyQualifiedName~too_short_for_any_lag"`
Expected: 2 failures on the message assertions.

- [ ] **Step 3: Split the message**

Replace the body of `if (settings.MaxLag is int given)` in `Stationarity.MaxLag`:

```csharp
            // Two limits with two reasons, the ceiling being the reference's own refusal (#984). The second is
            // floored by hand: C#'s division truncates toward zero, which would allow lag 0 when n − terms = 2.
            int spare = n - terms - 3;
            int allowed = Math.Min(ceiling, spare < 0 ? -1 : spare / 2);
            if (given > ceiling || n - (2 * given) - terms - 2 < 1)
            {
                string reason = given > ceiling
                    ? $"exceeds n/2 − terms − 1 = {ceiling}, the reference's ceiling; at most {allowed} is allowed"
                    : $"leaves the widest regression no degree of freedom; at most {allowed} leaves one";
                if (allowed < 0)
                {
                    reason = $"cannot be met: a series of {n} is too short for {terms} trend terms at any lag";
                }

                throw new ArgumentException($"a maximum lag of {given} {reason}.", optionsName);
            }

            return given;
```

- [ ] **Step 4: Run them to see them pass**

Run: the Step 2 command. Expected: 2 passed.

### Task 3: `KpssOptions.LagRule` refuses where it is set

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/KpssOptions.cs`, `src/Lodestar.Stats.TimeSeries/Stationarity.cs` (`Kpss`)
- Modify: `docs/reference/stats-timeseries/stationarity-tests/kpssoptions.md`, `CHANGELOG.md`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`

- [ ] **Step 1: Write the failing assertion**

At the end of `Undeclared_option_values_are_refused_where_they_are_set`:

```csharp
        // Kpss used to accept it here and refuse it at the call, naming `options` (#984).
        ArgumentOutOfRangeException lagRule = Assert.Throws<ArgumentOutOfRangeException>(
            () => new KpssOptions { LagRule = (KpssLagRule)42 });
        Assert.Equal("value", lagRule.ParamName);
```

- [ ] **Step 2: Run it to see it fail**

Run: `dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~Undeclared_option_values"`
Expected: FAIL, "No exception was thrown".

- [ ] **Step 3: Add the setter and trim the switch**

In `KpssOptions`, add the field `private KpssLagRule _lagRule = KpssLagRule.Automatic;` and replace the auto-property:

```csharp
    /// <summary>How the lag window is chosen. Default <see cref="KpssLagRule.Automatic"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A value <see cref="KpssLagRule"/> does not declare.</exception>
    public KpssLagRule LagRule
    {
        get => _lagRule;
        init
        {
            // Checked here, as every other time-series option enum is, rather than when Kpss reads it (#984).
            if (value is not (KpssLagRule.Automatic or KpssLagRule.Legacy or KpssLagRule.Fixed))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Not a KPSS lag rule.");
            }

            _lagRule = value;
        }
    }
```

In `Stationarity.Kpss`, the last three arms of the `lagCount` switch become:

```csharp
            // The setter admits only the three declared rules, so what is left is Fixed (#984).
            _ when settings.LagCount < n => settings.LagCount,
            _ => throw new ArgumentException(
                $"a lag window of {settings.LagCount} reaches the {n} observations; it must stay below them.",
                nameof(options)),
```

- [ ] **Step 4: Run it to see it pass**

Run: the Step 2 command. Expected: 1 passed.

- [ ] **Step 5: Document it**

`kpssoptions.md`'s **Exceptions** lists "when `LagRule` is a value `KpssLagRule` does not declare" beside the other two, "each checked where the value is set". `CHANGELOG.md`, `Lodestar.Stats.TimeSeries` → `Fixed`:

```markdown
- A too-large `DickeyFullerOptions.MaxLag` is refused with the reason that applies, and `KpssOptions.LagRule` refuses an undeclared value where it is set. ([#984](https://github.com/CyrilB1531/lodestar/issues/984))
```

### Task 4: Verify and commit

- [ ] **Step 1: Both suites**

Run: `dotnet test Lodestar.slnx -c Release --filter "FullyQualifiedName~Lodestar.Stats.TimeSeries"`
Expected: 0 failed, two assemblies.

- [ ] **Step 2: Random differential test against statsmodels**

Generate 600 series with `.venv-oracles` from a neutral directory, run `adfuller` over every `regression` and `autolag`, and replay them through `Stationarity.AugmentedDickeyFuller`. Expected: every answer statsmodels gives without the documented short-series refusal agrees on the lag and to `1e-9` on the statistic.

- [ ] **Step 3: Benchmark A/B/A**

Run, under `./.dotnet-guarded acquire`, in a `main` worktree, this branch and `main` again:
`dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*StationarityBenchmarks.LodestarA*'`
Expected: `LodestarAdfAutolag` within the A/A spread.

- [ ] **Step 4: Every check script, format, markdownlint, `pytest tools/tests`, then one commit**
