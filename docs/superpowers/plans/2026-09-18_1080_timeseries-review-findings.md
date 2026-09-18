# Six Lodestar.Stats.TimeSeries Review Findings Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refuse a straight line once, for both stationarity tests and at every lag, on a bar that does not grow with the series — and translate only the refusal that is this package's to translate.

**Architecture:** One new internal check, `SeriesChecks.RefuseStraightLine`, holds both halves of the test (residual root-mean-square against the fit's rounding, largest second difference against 8 ulps) and is called by `Stationarity.Kpss` under `ConstantAndTrend` and by `Stationarity.AugmentedDickeyFuller` unconditionally. The detrending both it and `Kpss` need moves into `LineFit.Residuals` so it is written once. `DickeyFullerRegression.Fit` gains a row-count guard on its `catch` filter, which is what separates the estimate's rank refusal from its missing-degree-of-freedom refusal.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, `Lodestar.Stats.TimeSeries` and its two test projects.

**Spec:** [`docs/superpowers/specs/2026-09-18_1080_timeseries-review-findings.md`](../specs/2026-09-18_1080_timeseries-review-findings.md)

## Global Constraints

- Both target frameworks, one public API: no `#if` at a call site, no API reduced on `netstandard2.0`. `double.IsFinite` does not exist there; `SeriesChecks` already asks the two halves separately.
- `LodestarUseProjectRefs` stays unset. `Lodestar.Stats.TimeSeries` reaches `Lodestar.Stats.Regression` through the published floor, so nothing in `src/Lodestar.Stats.Regression` may be changed by this branch.
- Warnings are errors, and `SonarAnalyzer.CSharp` runs at `AnalysisMode=All`. A suppression carries a reason a reviewer can disagree with.
- Machine epsilon is `2.220446049250313e-16`, never `double.Epsilon`.
- The refusal names `series`, the public parameter, never `design`.
- Every exception the XML documents must match what the reference page under `docs/reference/` says, and a changed behaviour lands in `docs/equivalence.md` and `CHANGELOG.md` in the same commit.
- One commit for the branch; `Closes #1080` in its message.

---

### Task 1: The straight-line check, and the detrending it shares with KPSS

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Internal/LineFit.cs`
- Modify: `src/Lodestar.Stats.TimeSeries/Internal/SeriesChecks.cs`
- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs` (delete `RefuseExactFit`, call the new check)
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`

**Interfaces:**

- Consumes: `LineFit.Through(ReadOnlySpan<double> abscissae, ReadOnlySpan<double> ordinates)` returning `(double Slope, double Intercept)`, already there.
- Produces: `internal static double[] LineFit.Residuals(ReadOnlySpan<double> series)` and
  `internal static void SeriesChecks.RefuseStraightLine(ReadOnlySpan<double> series, double[] lineResiduals, string consequence)`, both used by Task 2.

- [ ] **Step 1: Write the failing tests**

In `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`, beside `Kpss_tests_a_series_that_merely_lies_close_to_a_line`:

```csharp
    [Fact]
    public void Kpss_tests_a_long_series_whose_noise_is_thousands_of_ulps()
    {
        // 1e-10 on 1.0 is 4.5e5 ulps, and its residual root-mean-square is 5.77e-11. The bar that
        // grew as n made it 2.22e-10 at a million points, so white noise was reported as a line (#1080).
        var random = new Random(1);
        double[] series = new double[1_000_000];
        for (int i = 0; i < series.Length; i++)
        {
            series[i] = 1.0 + (((random.NextDouble() * 2.0) - 1.0) * 1e-10);
        }

        KpssResult result = Stationarity.Kpss(series, new KpssOptions { Regression = TrendTerms.ConstantAndTrend });

        Assert.True(double.IsFinite(result.Statistic));
    }

    [Fact]
    public void Kpss_tests_a_line_whose_points_miss_it_by_hundreds_of_ulps()
    {
        // 1e-7 on values up to 26,000 is 6.8e4 ulps of the largest, and n·ε of it is 2.89e-7 (#1080).
        var random = new Random(2);
        double[] series = new double[50_000];
        for (int i = 0; i < series.Length; i++)
        {
            series[i] = 1000.0 + (0.5 * i) + (((random.NextDouble() * 2.0) - 1.0) * 1e-7);
        }

        KpssResult result = Stationarity.Kpss(series, new KpssOptions { Regression = TrendTerms.ConstantAndTrend });

        Assert.True(double.IsFinite(result.Statistic));
    }

    [Fact]
    public void Kpss_does_not_call_an_overflowing_fit_a_straight_line()
    {
        // The line fit's mean overflows, every residual is NaN, and `>` sent NaN to the refusal (#1080).
        double[] series = [.. Enumerable.Range(0, 30).Select(i => i % 2 == 0 ? 1e307 : 2e307)];

        KpssResult result = Stationarity.Kpss(series, new KpssOptions { Regression = TrendTerms.ConstantAndTrend });

        Assert.True(double.IsNaN(result.Statistic));
    }
```

- [ ] **Step 2: Run them to watch them fail**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~StationarityEdgeTests"
```

Expected: the three new tests fail, the first two with `ArgumentException … every value lies on one straight line`, the third the same. The count reported must be the suite's, not zero.

- [ ] **Step 3: Give `LineFit` the residuals both callers need**

Append to `src/Lodestar.Stats.TimeSeries/Internal/LineFit.cs`, inside the class:

```csharp
    /// <summary>The series less its least-squares line over 1…n: the residuals a trend null leaves.</summary>
    /// <remarks>
    /// Written here rather than in each caller: the KPSS statistic and the straight-line refusal must
    /// measure the same residuals, or the refusal guards a fit nobody computed (#1080).
    /// </remarks>
    internal static double[] Residuals(ReadOnlySpan<double> series)
    {
        int count = series.Length;
        var time = new double[count];
        for (int i = 0; i < count; i++)
        {
            time[i] = i + 1;
        }

        (double slope, double intercept) = Through(time, series);
        var residuals = new double[count];
        for (int i = 0; i < count; i++)
        {
            residuals[i] = series[i] - (intercept + (slope * time[i]));
        }

        return residuals;
    }
```

- [ ] **Step 4: Write the check**

Append to `src/Lodestar.Stats.TimeSeries/Internal/SeriesChecks.cs`, inside the class:

```csharp
    /// <summary>Machine epsilon for <see cref="double"/>, which <see cref="double.Epsilon"/> is not.</summary>
    private const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary>How many ulps of the largest observation a stored line's second difference may reach.</summary>
    /// <remarks>
    /// Four times the worst of 200 random <c>a + b·i</c> per length, which measured 1.4 to 2.2 ulps from
    /// 30 points to a million: a second difference reads three neighbours and sums nothing, so its floor
    /// does not grow with the series the way the line fit's residuals do (#1080).
    /// </remarks>
    private const double SecondDifferenceUlps = 8.0;

    /// <summary>Refuses a series that lies on one straight line, which leaves a regression only rounding.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lineResiduals">The same series less its least-squares line, from <see cref="LineFit.Residuals"/>.</param>
    /// <param name="consequence">What a line costs this caller, completing the message after the colon.</param>
    /// <remarks>
    /// A line has to fail both tests. The residual root-mean-square against <c>n·ε</c> of the largest
    /// observation is the global one and cannot be tightened, because the line fit sums naively and a
    /// perfect line already leaves 1.05e4 ulps at a million points. The largest second difference against
    /// eight ulps is the local one and cannot stand alone, because second differences of <c>δ</c> still
    /// allow a bend of <c>δ·n²/8</c>. Both are written <c>≤</c>, so the NaN residuals an overflowing fit
    /// leaves are answered rather than reported as a line (#1080).
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="series"/> lies on one straight line.</exception>
    internal static void RefuseStraightLine(
        ReadOnlySpan<double> series, double[] lineResiduals, string consequence)
    {
        double scale = 0.0;
        foreach (double value in series)
        {
            scale = Math.Max(scale, Math.Abs(value));
        }

        double sumOfSquares = 0.0;
        foreach (double residual in lineResiduals)
        {
            sumOfSquares += residual * residual;
        }

        double largestBend = 0.0;
        for (int row = 2; row < series.Length; row++)
        {
            largestBend = Math.Max(largestBend, Math.Abs(series[row] - (2.0 * series[row - 1]) + series[row - 2]));
        }

        if (Math.Sqrt(sumOfSquares / lineResiduals.Length) <= lineResiduals.Length * MachineEpsilon * scale
            && largestBend <= SecondDifferenceUlps * MachineEpsilon * scale)
        {
            throw new ArgumentException(
                $"every value lies on one straight line: {consequence}", nameof(series));
        }
    }
```

- [ ] **Step 5: Call it from `Kpss`, and delete what it replaces**

In `src/Lodestar.Stats.TimeSeries/Stationarity.cs`, replace the call

```csharp
            RefuseExactFit(series, residuals, nameof(series));
```

with

```csharp
            SeriesChecks.RefuseStraightLine(series, residuals, "the trend leaves no variance to test.");
```

then delete the whole `RefuseExactFit` method with its `<summary>`/`<remarks>` and the
`MachineEpsilon` constant above it, and replace the body of the `ConstantAndTrend` branch of
`KpssResiduals` — the `time` array, the `LineFit.Through` call and the residual loop — with

```csharp
            return LineFit.Residuals(series);
```

- [ ] **Step 6: Run the three new tests and the whole edge suite**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~StationarityEdgeTests"
```

Expected: PASS, including the three `Kpss_refuses_a_line_whose_fit_leaves_only_rounding` cases from #976
and `Kpss_refuses_a_straight_line_its_trend_fits_exactly` from #874, which must still refuse.

---

### Task 2: The augmented Dickey-Fuller test refuses the same line, at every lag

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs` (the `AugmentedDickeyFuller` body and its `<exception>`)
- Modify: `src/Lodestar.Stats.TimeSeries/Internal/DickeyFullerRegression.cs:35-52`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs`, `tests/Lodestar.Stats.TimeSeries.Tests/SharedReflectionsTests.cs`

**Interfaces:**

- Consumes: `SeriesChecks.RefuseStraightLine(ReadOnlySpan<double> series, double[] lineResiduals, string consequence)` from Task 1.
- Produces: nothing later tasks call; Task 3 documents what this task changes.

- [ ] **Step 1: Write the failing tests**

In `StationarityEdgeTests.cs`, replace `Augmented_dickey_fuller_refuses_a_straight_line_naming_the_series`
with the same theory over the message the entry check now gives, and add the lag-zero case finding 3 names:

```csharp
    [Theory]
    [InlineData(TrendTerms.ConstantAndTrend, LagSelection.TStatistic, 1)]
    [InlineData(TrendTerms.Constant, LagSelection.Fixed, 1)]
    [InlineData(TrendTerms.Constant, LagSelection.Fixed, 0)]
    [InlineData(TrendTerms.None, LagSelection.Fixed, 0)]
    public void Augmented_dickey_fuller_refuses_a_straight_line_naming_the_series(
        TrendTerms regression, LagSelection selection, int maxLag)
    {
        // At a lag above zero the lagged differences repeat the intercept and the estimate refuses the
        // design. At lag zero under a constant the design is full rank and the fit exact, and the
        // statistic was 0.1474698045709202 of pure rounding on an intercept t of 1.6e15 (#1080).
        double[] line = [.. Enumerable.Range(0, 40).Select(i => 0.3 + (0.1 * i))];

        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(
                line,
                new DickeyFullerOptions { Regression = regression, LagSelection = selection, MaxLag = maxLag }));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("straight line", refusal.Message, StringComparison.Ordinal);
    }
```

In `SharedReflectionsTests.cs`, replace the body of
`A_search_reaching_no_degree_of_freedom_is_refused_as_the_estimate_refuses_it` with one that reads the
message, since the type alone let the wrong refusal through:

```csharp
    [Fact]
    public void A_search_reaching_no_degree_of_freedom_is_refused_as_the_estimate_refuses_it()
    {
        double[] series = Walk(12, seed: 3);

        ArgumentException refusal = Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Candidates(series, TrendTerms.None, maxLag: 5, rows: 6, withTStatistics: true));
        Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Candidates(series, TrendTerms.None, maxLag: 5, rows: 6, withTStatistics: false));

        // A full-rank random walk: the design this search runs out of rows for is not collinear, and the
        // translation that names a straight line reported it as one for naming `design` too (#1080).
        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("degrees of freedom left", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_rank_deficient_design_is_still_translated_into_the_series_the_caller_passed()
    {
        double[] line = [.. Enumerable.Range(0, 40).Select(i => 0.3 + (0.1 * i))];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Fit(line, TrendTerms.Constant, lag: 1, rows: 38));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("no unique least-squares solution", refusal.Message, StringComparison.Ordinal);
    }
```

- [ ] **Step 2: Run them to watch them fail**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~StationarityEdgeTests|FullyQualifiedName~SharedReflectionsTests"
```

Expected: the two lag-zero rows fail with "Assert.Throws() Failure: No exception was thrown", and
`A_search_reaching_no_degree_of_freedom…` fails on `Assert.Equal("design", refusal.ParamName)` with
`series`.

- [ ] **Step 3: Refuse the line in `AugmentedDickeyFuller`**

In `Stationarity.cs`, after `SeriesChecks.RefuseConstant(series);` in `AugmentedDickeyFuller`, add:

```csharp
        SeriesChecks.RefuseStraightLine(
            series,
            LineFit.Residuals(series),
            "a deterministic trend leaves no stochastic component for a unit-root test to find.");
```

- [ ] **Step 4: Narrow the translation to the refusal it translates**

In `src/Lodestar.Stats.TimeSeries/Internal/DickeyFullerRegression.cs`, inside `Fit`, add the row count
above the `try` and the guard to the filter:

```csharp
        double[] design = Design(series, regression, lag, rows, out double[] response);

        // The estimate names `design` for a rank-deficient design and for a fit with no residual degree
        // of freedom alike, and `Candidates` raises the second one through here on purpose, at the lag
        // its search reaches it. Only the first is this package's to translate (#1080).
        bool hasResidualDegreesOfFreedom = rows - (TermCount(regression) + 1 + lag) >= 1;
        try
        {
            OlsEstimate estimate = OrdinaryLeastSquares.Estimate(
                design, response, TrendColumns(regression) + 1 + lag, withIntercept: regression != TrendTerms.None);
            return (estimate.TStatistics, estimate.ResidualSumOfSquares);
        }
        catch (ArgumentException error) when (error.ParamName == DesignParameter && hasResidualDegreesOfFreedom)
        {
```

leaving the `throw new ArgumentException(...)` body as it stands.

- [ ] **Step 5: Run both suites**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~StationarityEdgeTests|FullyQualifiedName~SharedReflectionsTests"
```

Expected: PASS, with the count the filter reported in Step 2 plus the two new tests.

- [ ] **Step 6: Run the whole package, both frameworks**

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests tests/Lodestar.Stats.TimeSeries.NetStandard.Tests -c Release
```

Expected: PASS. A `ct` KPSS oracle case must not move: `LineFit.Residuals` is the same arithmetic in the
same order as the branch it replaced.

---

### Task 3: The two `<exception>` blocks, the two reference pages, the two equivalence rows

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs:24` and `:66` (the `<exception>` blocks)
- Modify: `docs/reference/stats-timeseries/stationarity-tests/stationarity-augmenteddickeyfuller.md`
- Modify: `docs/reference/stats-timeseries/stationarity-tests/stationarity-kpss.md`
- Modify: `docs/equivalence.md:480` and `:483`
- Modify: `CHANGELOG.md`

**Interfaces:**

- Consumes: the behaviour Tasks 1 and 2 produced.
- Produces: nothing.

- [ ] **Step 1: Rewrite `AugmentedDickeyFuller`'s `<exception>`**

```csharp
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, is constant, lies on one straight line, or is
    /// too short for its trend terms and default lag; or <paramref name="options"/> asks for a maximum lag
    /// above <c>n/2 − terms − 1</c> or one that leaves the widest regression no degree of freedom.
    /// </exception>
```

- [ ] **Step 2: Rewrite `Kpss`'s `<exception>`**

```csharp
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, is constant, or lies on one straight line under
    /// <see cref="TrendTerms.ConstantAndTrend"/>; or <paramref name="options"/> fixes a window at or above the
    /// series length.
    /// </exception>
```

- [ ] **Step 3: Rewrite the two reference pages' **Exceptions** paragraphs**

In `stationarity-augmenteddickeyfuller.md`:

```markdown
**Exceptions** — `ArgumentException` when `series` carries a non-finite value, is constant, lies on
one straight line — a deterministic trend, with no stochastic component to test — is too short for its
trend terms and default lag, or builds a lagged design with no unique least-squares solution; or when
`options` asks for a maximum lag above `n/2 − terms − 1` or one that leaves the widest regression no
degree of freedom.
```

In `stationarity-kpss.md`:

```markdown
**Exceptions** — `ArgumentException` when `series` carries a non-finite value, is constant, or lies on one
straight line under `TrendTerms.ConstantAndTrend` — its least-squares residuals inside `n·ε` of its largest
observation *and* its second differences inside eight ulps of it; or when `options` fixes a window at or
above the series length.
```

- [ ] **Step 4: Rewrite the two equivalence rows' divergences**

In `docs/equivalence.md`, in the `adfuller` row replace the sentence beginning "A series whose lagged
design has no unique solution" with:

```markdown
A series lying on one straight line is refused naming `series`, at every trend specification and every lag: above lag zero its lagged differences repeat the intercept, and at lag zero under `regression="c"` the design is full rank but the fit exact, so the statistic is one rounding error over another (`0.1475` on `0.3 + 0.1·i`, 40 points). The reference's pseudo-inverse answers the minimum-norm fit in the first case and that rounding in the second ([#979](https://github.com/CyrilB1531/lodestar/issues/979), [#1080](https://github.com/CyrilB1531/lodestar/issues/1080)).
```

In the `kpss` row replace the sentence beginning "The line is measured against `n·ε`" with:

```markdown
The line is measured twice, and refused only when both agree: its least-squares residuals within `n·ε` of the largest observation, which caught 100 of 100 random lines where a test for exact zeros caught 1 ([#976](https://github.com/CyrilB1531/lodestar/issues/976)), and its largest second difference within eight ulps of it, which is what keeps a million points of `1.0 ± 1e-10` and 50,000 of `1000 + 0.5·i ± 1e-7` answered ([#1080](https://github.com/CyrilB1531/lodestar/issues/1080)). 300 random series that are not lines agree with statsmodels to 3.3e-13 on the statistic, with the same window.
```

- [ ] **Step 5: Add the changelog entry**

Under the unreleased `Lodestar.Stats.TimeSeries` heading, one sentence:

```markdown
- The stationarity tests refuse a series lying on one straight line on a bar that does not grow with its length, and the augmented Dickey-Fuller test refuses it at lag zero too ([#1080](https://github.com/CyrilB1531/lodestar/issues/1080), `<commit>`).
```

- [ ] **Step 6: Run the documentation gates**

```bash
python3 tools/extract_doc_snippets.py && dotnet build samples/Lodestar.DocSnippets -c Release
```

```bash
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

Expected: both clean. The reference gate compares declarations and exception *types*, neither of which moved.

---

### Task 4: The gates, and the branch

**Files:** none of its own.

- [ ] **Step 1: Every check script, not a subset**

```bash
for check in tools/check_*.py; do echo "== $check"; python3 "$check" || echo "FAILED $check"; done
```

Expected: every one clean. `tools/check_repeated_literals.py --base origin/main` is run separately with its base.

- [ ] **Step 2: Build and format**

```bash
dotnet build Lodestar.slnx -c Release && dotnet format Lodestar.slnx --verify-no-changes
```

- [ ] **Step 3: The whole suite, read the assembly count**

```bash
dotnet test Lodestar.slnx -c Release
```

Expected: 36 assemblies, all green. Read the count, not the colour.

- [ ] **Step 4: Commit and open the pull request**

```bash
git add -A && git commit
gh pr create --fill --milestone "Next release"
```

The message closes #1080 and carries the co-author line; the body names the six findings and the
measurement the new bar rests on.
