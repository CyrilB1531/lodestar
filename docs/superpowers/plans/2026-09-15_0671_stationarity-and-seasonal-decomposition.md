# Stationarity and seasonal decomposition Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship the augmented Dickey-Fuller test, KPSS and seasonal decomposition at `statsmodels` 0.15.0 parity, in a new `Lodestar.Stats.TimeSeries` package that #617's serial-correlation lot moves into.

**Architecture:** A static `Stationarity` class holds both tests and a static `SeasonalDecomposition` the decomposition; options are records of value types, results sealed classes with an internal constructor. ADF fits each candidate lag through `Lodestar.Stats.Regression`'s `OrdinaryLeastSquares.Fit` and reads MacKinnon's response surfaces from a frozen internal table; KPSS and the decomposition's trend extrapolation share a closed-form `LineFit`. The whole `Lodestar.Stats.TimeSeries` namespace leaves `Lodestar.Stats` for the new package before any `Lodestar.Stats` tag carries it.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, `Lodestar.Stats` 0.4.0 and `Lodestar.Stats.Regression` 0.1.0 as published floors, `statsmodels` 0.15.0 as the oracle.

**Spec:** [`docs/superpowers/specs/2026-09-15_0671_stationarity-and-seasonal-decomposition.md`](../specs/2026-09-15_0671_stationarity-and-seasonal-decomposition.md)

**Branch:** `docs/671-stationarity-decomposition-spec` — one pull request closes #671, spec, plan, code and documents together, squashed to one commit.

## Global Constraints

- **Everything in English**; no `feat:`/`fix:` prefix; `Closes #671` in the pull request.
- **Both target frameworks, one public API.** `net10.0;netstandard2.0`. `double.IsFinite` is absent on
  `netstandard2.0`: ask `IsNaN` and `IsInfinity` separately, as `SerialCorrelation` does.
- **Warnings are errors**, `AnalysisMode=All`, `AnalysisLevel` 10.0 — S1226, S1192, CA1819, S3776 (15),
  S1244 on exact float comparison (suppress with the reason above it, never globally).
- **Comment rules:** why, not what; two lines inline, eight of XML prose; `long-comment:` first line past
  that. `python3 tools/check_comment_length.py`.
- **`$MAIN`** is the main checkout: `MAIN="$(cd "$(git rev-parse --git-common-dir)/.." && pwd -P)"`.
- **Every dotnet command goes through the lock**: `"$MAIN/.dotnet-guarded" dotnet <args>`, a background
  chain inside `acquire`/`release`.
- **The ADR number comes from `"$MAIN/.next-adr"`** when the record is written.
- **Floors, exactly:** `Lodestar.Stats` **0.4.0** (unchanged), `Lodestar.Stats.Regression` **0.1.0** (new
  `PackageVersion`, and the version whose `OrdinaryLeastSquares.Fit`, `OlsOptions.WithIntercept`,
  `OlsSummary.TStatistics`, `ResidualStandardError` and `ResidualDegreesOfFreedom` this lot calls — all
  verified present at the `Lodestar.Stats.Regression/v0.1.0` tag).
- **New package version `0.1.0`.** `Lodestar.Stats`' version does not move in this branch.
- **Test count, not colour:** `dotnet test Lodestar.slnx -c Release` reports **36 assemblies** after
  Task 1 (34 today).
- **The oracle is `statsmodels` 0.15.0**, run from `/var/tmp` with
  `PYTHONSAFEPATH=1 "$MAIN/.venv-oracles/bin/python"`; read the generator's own exit code.
- **Tolerances:** statistics, critical values and components `1e-9` absolute; p-values relative through
  `StatsOracleAsserts.PValue`; integers and `PValueBound` exactly; `NaN` positions exactly.

---

### Task 1: The package, the move, and every guard that counts packages

**Files:**

- Create: `src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj`, `src/Lodestar.Stats.TimeSeries/Version.props`
- Move (`git mv`): `src/Lodestar.Stats/TimeSeries/*` → `src/Lodestar.Stats.TimeSeries/`
- Create: `tests/Lodestar.Stats.TimeSeries.Tests/` (csproj, `Documentation/ReferenceDocumentationTests.cs`, `Oracles/StatsCorpus.cs`, `Oracles/StatsOracleAsserts.cs`)
- Move: `tests/Lodestar.Stats.Tests/SerialCorrelation{Oracle,Edge}Tests.cs` → `tests/Lodestar.Stats.TimeSeries.Tests/`
- Create: `tests/Lodestar.Stats.TimeSeries.NetStandard.Tests/` (csproj, `NetStandardAssemblyGuardTests.cs`)
- Move: `docs/reference/stats/timeseries.md` → `docs/reference/stats-timeseries/correlation.md`, `docs/reference/stats/timeseries/*` → `docs/reference/stats-timeseries/correlation/`
- Modify: `Lodestar.slnx`, `src/Directory.Packages.props`, `tools/check_nuspec_dependencies.py`, `tools/check_version_floor.py`, `tools/check_sample_coverage.py`, `docs/wiki-map.json`, `CLAUDE.md`, `README.md`, `CONTRIBUTING.md` if it lists packages, `.github/workflows/{ci,release,release-nuget-org,sonarcloud}.yml`, `samples/Lodestar.Sample/Lodestar.Sample.csproj`, `samples/Lodestar.DocSnippets/Lodestar.DocSnippets.csproj`, `bench/Lodestar.Stats.Benchmarks/Lodestar.Stats.Benchmarks.csproj`, `docs/reference/stats.md`, every link the reference gate reports

**Interfaces:**

- Produces: the assembly `Lodestar.Stats.TimeSeries`, root namespace `Lodestar.Stats.TimeSeries`, carrying
  `SerialCorrelation`, `AutocorrelationOptions`, `AutocorrelationResult`, `LjungBoxOptions`,
  `LjungBoxResult` unchanged; `Lodestar.Stats` no longer declares any of them.

- [ ] **Step 1: Prove the edge before building on it**

The spec's fourth "what would make this wrong". A throwaway file-based app in the scratchpad, not the
branch, replaying 56 `statsmodels` cases through `Lodestar.Stats.Regression` 0.1.0 from nuget.org:

```bash
"$MAIN/.dotnet-guarded" dotnet run "$SCRATCH/spike671/spike.cs" -- "$SCRATCH/spike671/ref.json"
```

Expected: `worst` below `1e-9` across ADF (four regressions × four lag rules × two series), KPSS and the
decomposition. Measured on 2026-09-15: `worst 5,46E-012 at ar|ctt|t-stat p`.

- [ ] **Step 2: The project and its version**

`src/Lodestar.Stats.TimeSeries/Version.props`:

```xml
<Project>

  <!--
    Lodestar.Stats.TimeSeries owns its version here, independently of the other packages
    (see docs/decisions/0012-per-package-versioning.md).

    0.1.0 is this package's first release. It takes Lodestar.Stats for the chi-squared tail and
    the normal quantile, and Lodestar.Stats.Regression for the least-squares fits the augmented
    Dickey-Fuller test runs; both floors live in src/Directory.Packages.props.
  -->
  <PropertyGroup>
    <LodestarStatsTimeSeriesVersion>0.1.0</LodestarStatsTimeSeriesVersion>
  </PropertyGroup>

</Project>
```

`src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <!-- This package's version, owned here rather than repository-wide. -->
  <Import Project="Version.props" />

  <PropertyGroup>
    <Version>$(LodestarStatsTimeSeriesVersion)</Version>
    <TargetFrameworks>net10.0;netstandard2.0</TargetFrameworks>
    <RootNamespace>Lodestar.Stats.TimeSeries</RootNamespace>

    <PackageId>Lodestar.Stats.TimeSeries</PackageId>
    <Description>Time-series diagnostics for .NET, at statsmodels parity: the autocorrelation and partial autocorrelation functions with their bands, the Ljung-Box test, the augmented Dickey-Fuller test with MacKinnon p-values, KPSS, and classical seasonal decomposition. Spans in, numbers out, and no dependencies outside Lodestar.</Description>
    <PackageTags>time-series;stationarity;unit-root;dickey-fuller;adf;kpss;autocorrelation;ljung-box;seasonal-decomposition;statistics;statsmodels;lodestar</PackageTags>
  </PropertyGroup>

  <!--
    Two edges, both to core packages: Lodestar.Stats for the tails, and Lodestar.Stats.Regression
    for the fits the augmented Dickey-Fuller test runs per candidate lag. The second is the reason
    this is a package at all (decision 0114 named it as the one that would earn one).

    PackageReference against the published floors, like every other src/ edge (decision 0012):

        export LodestarUseProjectRefs=true

    is the developer loop, never set in CI, and the packed .nuspec is asserted there.
  -->
  <ItemGroup Condition="'$(LodestarUseProjectRefs)' == 'true'">
    <ProjectReference Include="../Lodestar.Stats/Lodestar.Stats.csproj" />
    <ProjectReference Include="../Lodestar.Stats.Regression/Lodestar.Stats.Regression.csproj" />
  </ItemGroup>

  <ItemGroup Condition="'$(LodestarUseProjectRefs)' != 'true'">
    <PackageReference Include="Lodestar.Stats" />
    <PackageReference Include="Lodestar.Stats.Regression" />
  </ItemGroup>

  <Target Name="WarnOnLocalProjectRefs" BeforeTargets="Build"
          Condition="'$(LodestarUseProjectRefs)' == 'true'">
    <Message Importance="high"
             Text="Lodestar.Stats.TimeSeries: LodestarUseProjectRefs=true — referencing Lodestar.Stats and Lodestar.Stats.Regression by project, not by package. This is the developer loop, not what ships." />
  </Target>

  <ItemGroup>
    <InternalsVisibleTo Include="Lodestar.Stats.TimeSeries.Tests" />
    <InternalsVisibleTo Include="Lodestar.Stats.TimeSeries.NetStandard.Tests" />
  </ItemGroup>

</Project>
```

`src/Directory.Packages.props` gains, beside `Lodestar.Stats`:

```xml
    <PackageVersion Include="Lodestar.Stats.Regression" Version="0.1.0" />
```

and its comment's list of floors names `Lodestar.Stats.TimeSeries` on `Lodestar.Stats` and
`Lodestar.Stats.Regression`.

- [ ] **Step 3: Move the lot**

```bash
git mv src/Lodestar.Stats/TimeSeries/AutocorrelationOptions.cs src/Lodestar.Stats/TimeSeries/AutocorrelationResult.cs \
       src/Lodestar.Stats/TimeSeries/LjungBoxOptions.cs src/Lodestar.Stats/TimeSeries/LjungBoxResult.cs \
       src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs src/Lodestar.Stats.TimeSeries/
git mv src/Lodestar.Stats/TimeSeries/Internal src/Lodestar.Stats.TimeSeries/Internal
mkdir -p tests/Lodestar.Stats.TimeSeries.Tests
git mv tests/Lodestar.Stats.Tests/SerialCorrelationOracleTests.cs tests/Lodestar.Stats.Tests/SerialCorrelationEdgeTests.cs \
       tests/Lodestar.Stats.TimeSeries.Tests/
git mv docs/reference/stats/timeseries docs/reference/stats-timeseries/correlation
git mv docs/reference/stats/timeseries.md docs/reference/stats-timeseries/correlation.md
```

In the two moved test files, `namespace Lodestar.Stats.Tests;` becomes
`namespace Lodestar.Stats.TimeSeries.Tests;` and `using Lodestar.Stats.Tests.Oracles;` becomes
`using Lodestar.Stats.TimeSeries.Tests.Oracles;`. `SerialCorrelation` calls `Distributions.ChiSquaredSf`
and `Distributions.NormalQuantile` unqualified, which resolved through the enclosing namespace
`Lodestar.Stats`; in `src/Lodestar.Stats.TimeSeries/SerialCorrelation.cs` add `using Lodestar.Stats;`
under `using System.Globalization;`.

- [ ] **Step 4: The two test projects**

`tests/Lodestar.Stats.TimeSeries.Tests/Lodestar.Stats.TimeSeries.Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <IsPackable>false</IsPackable>
    <GenerateDocumentationFile>false</GenerateDocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
    <PackageReference Include="Microsoft.Testing.Extensions.TrxReport" />
    <PackageReference Include="xunit.v3" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="../../src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj" />
  </ItemGroup>

  <ItemGroup>
    <None Include="../oracles/stats_timeseries.json" CopyToOutputDirectory="PreserveNewest" LinkBase="oracles" />
  </ItemGroup>

  <!-- The gate's engine is shared by every package's suite, so it is linked rather than copied. -->
  <ItemGroup>
    <Compile Include="../Shared/ReferenceDocumentation.cs" Link="Documentation/ReferenceDocumentation.cs" />
    <None Include="../../docs/reference/stats-timeseries/**/*.md" CopyToOutputDirectory="PreserveNewest"
          LinkBase="reference" />
    <None Include="../../docs/wiki-map.json" CopyToOutputDirectory="PreserveNewest" />
    <None Include="../../docs/**/*.md" Exclude="../../docs/superpowers/**"
          CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
  </ItemGroup>

</Project>
```

The two corpora Task 2 writes are added to this item group there, not here: `CopyToOutputDirectory` on a
missing file is `MSB3030`, an error, found on the first build of this task.

`tests/Lodestar.Stats.TimeSeries.Tests/Documentation/ReferenceDocumentationTests.cs`:

```csharp
using Lodestar.Tests.Documentation;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests.Documentation;

/// <summary>The reference gate over the pages <c>Lodestar.Stats.TimeSeries</c> declares covered.</summary>
public sealed class ReferenceDocumentationTests
{
    private static string Root => Path.Combine(AppContext.BaseDirectory, "reference");

    private static string Map => Path.Combine(AppContext.BaseDirectory, "wiki-map.json");

    private static string Docs => Path.Combine(AppContext.BaseDirectory, "docs");

    [Fact]
    public void Every_covered_namespace_is_documented()
    {
        IReadOnlyList<string> complaints = ReferenceDocumentation.Check(
            typeof(SerialCorrelation).Assembly, "Lodestar.Stats.TimeSeries", Map, Root);

        Assert.Empty(complaints);
    }

    [Fact]
    public void Every_documented_member_named_in_the_docs_links_to_its_entry()
    {
        IReadOnlyList<string> complaints = ReferenceDocumentation.CheckLinks(
            typeof(SerialCorrelation).Assembly, "Lodestar.Stats.TimeSeries", Map, Docs);

        Assert.Empty(complaints);
    }
}
```

`Oracles/StatsCorpus.cs` and `Oracles/StatsOracleAsserts.cs` are copies of the `Lodestar.Stats.Tests`
files trimmed to the members this suite calls — `Load`, `Number`, `Doubles`, and `Statistic`, `PValue`
— with the namespace `Lodestar.Stats.TimeSeries.Tests.Oracles`. A copy, not a link: the originals carry
`Alternative` and `ExactMethod` helpers that name `Lodestar.Stats` types this suite has no business
compiling against.

`tests/Lodestar.Stats.TimeSeries.NetStandard.Tests/Lodestar.Stats.TimeSeries.NetStandard.Tests.csproj`
is `tests/Lodestar.Survival.NetStandard.Tests`' file with `Survival` replaced by `Stats.TimeSeries`, its
`docs/reference/survival/**` include replaced by `docs/reference/stats-timeseries/**`, and four
pinned references — every `Lodestar.*` assembly the suite loads:

```xml
    <ProjectReference Include="../../src/Lodestar.Stats.TimeSeries/Lodestar.Stats.TimeSeries.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Stats/Lodestar.Stats.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Stats.Regression/Lodestar.Stats.Regression.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Decomposition/Lodestar.Decomposition.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
```

`NetStandardAssemblyGuardTests.cs` asserts `.NETStandard,Version=v2.0` for `typeof(SerialCorrelation)`,
`typeof(Lodestar.Stats.Distributions)`, `typeof(Lodestar.Stats.Regression.OrdinaryLeastSquares)` and
`typeof(Lodestar.Decomposition.QrDecomposition)` — one `[Fact]` each, the Survival file's shape.
`Lodestar.Abstractions` is reached only if the suite loads a type from it; the guard script says so if it
does.

- [ ] **Step 5: Run the guards and fix each finding they print**

```bash
python3 tools/check_nuspec_dependencies.py --help >/dev/null
for s in tools/check_*.py; do case $s in *repeated_literals*|*adr_immutable*) a="--base origin/main";; *nuspec_dependencies*) continue;; *) a="";; esac; python3 "$s" $a || echo "FAIL $s"; done
```

Each list the guards compare carries the package; the edits, all of them additions beside
`Lodestar.Survival` or `Lodestar.Stats.Regression`:

- `tools/check_nuspec_dependencies.py`: `STATS_TIMESERIES = "Lodestar.Stats.TimeSeries"`,
  `STATS_REGRESSION_FLOOR = "0.1.0"`, and
  `STATS_TIMESERIES: {NET: {STATS: STATS_FLOOR, STATS_REGRESSION: STATS_REGRESSION_FLOOR}, NETSTANDARD: {STATS: STATS_FLOOR, STATS_REGRESSION: STATS_REGRESSION_FLOOR, **POLYFILLS}}`
  with a comment naming the two edges.
- `tools/check_version_floor.py`: `"Lodestar.Stats.TimeSeries"` added to the `Lodestar.Stats` row's
  dependents, and a new `Floor("Lodestar.Stats.Regression", "LodestarStatsRegressionVersion", "STATS_REGRESSION_FLOOR", ("Lodestar.Stats.TimeSeries",))`.
- `tools/check_sample_coverage.py`: `"Lodestar.Stats.TimeSeries"` in `CONVERTED`.
- `CLAUDE.md`: the pack loop, the package table row
  (`| Lodestar.Stats.TimeSeries | core | the autocorrelation functions, Ljung-Box, the augmented Dickey-Fuller test, KPSS and seasonal decomposition, at statsmodels parity. |`),
  "Seventeen" → "Eighteen", the edge count and sentence (`Stats.TimeSeries` → `Stats` and
  `Stats.Regression`), and "34 assemblies" → "36".
- `README.md`: the pack loop, the Structure tree, the Publishing list, and its package count.
- `.github/workflows/ci.yml` and `sonarcloud.yml`: every `for proj in src/…` loop;
  `release.yml`'s package alternation; `release-nuget-org.yml`'s choice list.
- `Lodestar.slnx`: the source project and both test projects.
- `docs/wiki-map.json`: `Lodestar.Stats`' `covered` loses `Lodestar.Stats.TimeSeries`; a new package
  entry:

```json
    "Lodestar.Stats.TimeSeries": {
      "wiki": "Stats-TimeSeries",
      "pages": ["docs/guides/time-series-diagnostics.md", "docs/reference/stats-timeseries/*.md", "docs/reference/stats-timeseries/*/*.md"],
      "covered": {
        "Lodestar.Stats.TimeSeries": ["docs/reference/stats-timeseries/correlation", "docs/reference/stats-timeseries/stationarity-tests", "docs/reference/stats-timeseries/seasonality"]
      },
      "exceptionsUnchecked": []
    }
```

- `samples/Lodestar.Sample/Lodestar.Sample.csproj` and `samples/Lodestar.DocSnippets/Lodestar.DocSnippets.csproj`:
  `<Import Project="../../src/Lodestar.Stats.TimeSeries/Version.props" />` and
  `<PackageReference Include="Lodestar.Stats.TimeSeries" Version="$(LodestarStatsTimeSeriesVersion)" />`.
- `bench/Lodestar.Stats.Benchmarks/Lodestar.Stats.Benchmarks.csproj`: a `ProjectReference` to the new
  project beside `Lodestar.Stats`.
- `docs/reference/stats.md`: the time-series sentence points at `../stats-timeseries/correlation.md`
  and names the package.

Expected after the edits: every guard prints `ok`.

- [ ] **Step 6: Build, test, count**

```bash
"$MAIN/.dotnet-guarded" dotnet build Lodestar.slnx -c Release 2>&1 | grep -E "Avertissement\(s\)|Erreur\(s\)"
"$MAIN/.dotnet-guarded" dotnet test Lodestar.slnx -c Release 2>&1 | grep -E "Lodestar\..*\.dll|total :|échec :"
```

Expected: 0 warnings, 0 errors; 36 assemblies; the `SerialCorrelation` cases now counted under
`Lodestar.Stats.TimeSeries.Tests` and its mirror. Failures here are the reference gate's links into the
moved pages — `docs/equivalence.md`, `docs/migration/statsmodels.md`, `bench/README.md`, the pages' own
`../../../equivalence.md` depth (one folder shallower now: `../../equivalence.md`) — fixed where the test
names them.

- [ ] **Step 7: Commit**

```bash
git add -A && git commit -m "Move the serial-correlation diagnostics into Lodestar.Stats.TimeSeries"
```

---

### Task 2: The oracle corpora

**Files:**

- Modify: `tools/generate_oracles.py`
- Create: `tests/oracles/stats_stationarity.json`, `tests/oracles/stats_seasonal.json` (generated)

**Interfaces:**

- Produces, `stats_stationarity.json`: `metadata {library: "statsmodels", version: "0.15.0", family: "stationarity", count}` and cases of two shapes —
  `{name, call: "adfuller", series, regression: "n"|"c"|"ct"|"ctt", autolag: "AIC"|"BIC"|"t-stat"|null, maxlag: int|null, statistic, pvalue, usedlag, nobs, critical: [1 %, 5 %, 10 %], icbest: number|null}` and
  `{name, call: "kpss", series, regression: "c"|"ct", nlags: "auto"|"legacy"|int, statistic, pvalue, lags, critical: [10 %, 5 %, 2.5 %, 1 %], bound: "none"|"smaller"|"greater"}`.
- Produces, `stats_seasonal.json`: `metadata {…, family: "seasonal", count}` and
  `{name, series, period, model: "additive"|"multiplicative", two_sided, extrapolate_trend, trend, seasonal, resid}` with `NaN` as the string `"NaN"`.

- [ ] **Step 1: The fixtures and the two generators**

Beside `generate_stats_timeseries()` in `tools/generate_oracles.py`:

```python
ADFULLER = "adfuller"
KPSS = "kpss"


def _adf_near_switch(rng: SeededRandom, below: bool) -> list[float]:
    """An AR(1) whose ADF statistic under 'c' falls within 0.3 of MacKinnon's switch point."""
    from statsmodels.tsa.stattools import adfuller

    for step in range(400):
        phi = 0.80 + step * 0.0005
        series = [0.0]
        for _ in range(119):
            series.append(phi * series[-1] + rng.gauss(0.0, 1.0))
        statistic = adfuller(series, result_object=False)[0]
        if (-1.91 < statistic <= -1.61) if below else (-1.61 < statistic < -1.31):
            return [round(v, 10) for v in series]
    raise SystemExit("no series near ADF's switch point; widen the search")


def _kpss_near(rng: SeededRandom, low: float, high: float, walk_weight: float) -> list[float]:
    """Noise plus a scaled random walk whose KPSS level statistic lands in (low, high)."""
    from statsmodels.tsa.stattools import kpss

    for step in range(400):
        weight = walk_weight * (1.0 + step * 0.01)
        walk, series = 0.0, []
        for _ in range(150):
            walk += rng.gauss(0.0, 1.0)
            series.append(rng.gauss(0.0, 1.0) + weight * walk)
        with warnings.catch_warnings():
            warnings.simplefilter("ignore")
            statistic = kpss(series, regression="c", nlags="auto", result_object=False)[0]
        if low < statistic < high:
            return [round(v, 10) for v in series]
    raise SystemExit(f"no series with a KPSS statistic in ({low}, {high}); widen the search")


def _stationarity_fixtures() -> list[dict]:
    """Series chosen for which branch of the reference each one exercises (#671)."""
    rng = SeededRandom(SEED + 671)

    walk = [0.0]
    for _ in range(199):
        walk.append(walk[-1] + rng.gauss(0.0, 1.0))
    ar = [0.0]
    for _ in range(199):
        ar.append(0.5 * ar[-1] + rng.gauss(0.0, 1.0))
    trend = [0.04 * i + rng.gauss(0.0, 1.0) for i in range(150)]
    noise = [rng.gauss(0.0, 1.0) for _ in range(400)]
    explosive = [1.0]
    for _ in range(79):
        explosive.append(1.03 * explosive[-1] + rng.gauss(0.0, 0.1))

    def fixture(name: str, series: list[float]) -> dict:
        return {"name": name, SERIES: [round(v, 10) for v in series]}

    return [
        fixture("random walk, 200 points", walk),
        fixture("AR(1) at 0.5, 200 points", ar),
        fixture("trend-stationary, 150 points", trend),
        fixture("white noise, 400 points", noise),
        fixture("explosive AR(1) at 1.03, 80 points", explosive),
        {"name": "ADF just below the 'c' switch point", SERIES: _adf_near_switch(rng, below=True)},
        {"name": "ADF just above the 'c' switch point", SERIES: _adf_near_switch(rng, below=False)},
        {"name": "KPSS just inside the 10 % end", SERIES: _kpss_near(rng, 0.347, 0.40, 0.02)},
        {"name": "KPSS just inside the 1 % end", SERIES: _kpss_near(rng, 0.68, 0.739, 0.05)},
    ]


def _kpss_bound(caught: list) -> str:
    """The direction statsmodels' InterpolationWarning names, or 'none'."""
    for warning in caught:
        text = str(warning.message)
        if "p-value is smaller" in text:
            return "smaller"
        if "p-value is greater" in text:
            return "greater"
    return "none"


def generate_stats_stationarity() -> dict:
    """The augmented Dickey-Fuller test and KPSS, against statsmodels 0.15.0 (#671)."""
    from statsmodels.tsa.stattools import adfuller, kpss

    cases: list[dict] = []
    for fx in _stationarity_fixtures():
        x = fx[SERIES]
        for regression in ("n", "c", "ct", "ctt"):
            for autolag in ("AIC", "BIC", "t-stat", None):
                stat, p, used, nobs, crit, *rest = adfuller(
                    x, regression=regression, autolag=autolag, result_object=False)
                cases.append({
                    "name": f"{fx['name']} | adfuller | {regression} | {autolag}",
                    "call": ADFULLER, SERIES: x, "regression": regression,
                    "autolag": autolag, "maxlag": None,
                    "statistic": float(stat), "pvalue": float(p), "usedlag": int(used),
                    "nobs": int(nobs),
                    "critical": [float(crit["1%"]), float(crit["5%"]), float(crit["10%"])],
                    "icbest": float(rest[0]) if autolag else None,
                })
        for regression in ("c", "ct"):
            for nlags in ("auto", "legacy", 4):
                with warnings.catch_warnings(record=True) as caught:
                    warnings.simplefilter("always")
                    stat, p, lags, crit = kpss(
                        x, regression=regression, nlags=nlags, result_object=False)
                cases.append({
                    "name": f"{fx['name']} | kpss | {regression} | {nlags}",
                    "call": KPSS, SERIES: x, "regression": regression, "nlags": nlags,
                    "statistic": float(stat), "pvalue": float(p), "lags": int(lags),
                    "critical": [float(crit[k]) for k in ("10%", "5%", "2.5%", "1%")],
                    "bound": _kpss_bound(caught),
                })

    noise = _stationarity_fixtures()[3][SERIES]
    stat, p, used, nobs, crit = adfuller(
        noise, maxlag=0, autolag=None, result_object=False)
    cases.append({
        "name": "white noise, 400 points | adfuller | c | fixed at 0",
        "call": ADFULLER, SERIES: noise, "regression": "c", "autolag": None, "maxlag": 0,
        "statistic": float(stat), "pvalue": float(p), "usedlag": int(used), "nobs": int(nobs),
        "critical": [float(crit["1%"]), float(crit["5%"]), float(crit["10%"])], "icbest": None,
    })

    return {
        "metadata": {"library": STATSMODELS, "version": version(STATSMODELS),
                     FAMILY: "stationarity", "count": len(cases)},
        CASES: cases,
    }


def generate_stats_seasonal() -> dict:
    """Classical seasonal decomposition, against statsmodels 0.15.0 (#671)."""
    import numpy as np
    from statsmodels.tsa.seasonal import seasonal_decompose

    rng = SeededRandom(SEED + 6710)
    monthly = [20.0 + 0.1 * i + 4.0 * math.sin(2.0 * math.pi * i / 12.0) + rng.gauss(0.0, 0.5)
               for i in range(96)]
    weekly = [10.0 + 2.0 * math.cos(2.0 * math.pi * i / 7.0) + rng.gauss(0.0, 0.3)
              for i in range(50)]
    minimal = [3.0, 5.0, 4.0, 6.0]

    cases: list[dict] = []
    for name, series, period, extrapolations in (
        ("monthly, 96 points", monthly, 12, (0, 1, 11)),
        ("period 7, 50 points", weekly, 7, (0, 1, 6)),
        ("period 2, 4 points", minimal, 2, (0, 1)),
    ):
        x = np.array([round(v, 10) for v in series])
        for model in ("additive", "multiplicative"):
            for two_sided in (True, False):
                for extrapolate in extrapolations:
                    result = seasonal_decompose(
                        x, model=model, period=period, two_sided=two_sided,
                        extrapolate_trend=extrapolate)
                    cases.append({
                        "name": f"{name} | {model} | two_sided={two_sided} | extrapolate={extrapolate}",
                        SERIES: x.tolist(), "period": period, "model": model,
                        "two_sided": two_sided, "extrapolate_trend": extrapolate,
                        "trend": [_stats_number(v) for v in result.trend],
                        "seasonal": [_stats_number(v) for v in result.seasonal],
                        "resid": [_stats_number(v) for v in result.resid],
                    })

    return {
        "metadata": {"library": STATSMODELS, "version": version(STATSMODELS),
                     FAMILY: "seasonal", "count": len(cases)},
        CASES: cases,
    }
```

Registered beside `"stats_timeseries.json": generate_stats_timeseries,`:

```python
        "stats_stationarity.json": generate_stats_stationarity,
        "stats_seasonal.json": generate_stats_seasonal,
```

Then `tests/Lodestar.Stats.TimeSeries.Tests/Lodestar.Stats.TimeSeries.Tests.csproj` gains the two
`<None Include="../oracles/stats_stationarity.json" …/>` and `stats_seasonal.json` items beside
`stats_timeseries.json`. `warnings` is imported at the top of the module if it is not already; `_stats_number` writes `NaN` as
`"NaN"`, and is the helper the timeseries corpus already uses.

- [ ] **Step 2: Generate and check the fixtures do what they are for**

```bash
cd /var/tmp && PYTHONSAFEPATH=1 "$MAIN/.venv-oracles/bin/python" "$OLDPWD/tools/generate_oracles.py"; echo "exit=$?"
cd "$OLDPWD" && python3 - <<'EOF'
import json
d = json.load(open("tests/oracles/stats_stationarity.json"))
adf_c = [c for c in d["cases"] if c["call"] == "adfuller" and c["regression"] == "c" and c["autolag"] == "AIC"]
print("p exactly 1:", any(c["pvalue"] == 1.0 for c in d["cases"] if c["call"] == "adfuller"))
print("p exactly 0:", any(c["pvalue"] == 0.0 for c in d["cases"] if c["call"] == "adfuller"))
print("either side of -1.61:", sorted(round(c["statistic"], 3) for c in adf_c if -1.91 < c["statistic"] < -1.31))
print("bounds:", sorted({c["bound"] for c in d["cases"] if c["call"] == "kpss"}))
EOF
```

Expected: `exit=0`; both exact p-values present; at least one statistic on each side of `-1.61`; the
bounds `greater`, `none`, `smaller` all present. If a search raises, widen its range in the generator —
never hand-edit a corpus.

- [ ] **Step 3: Commit**

```bash
git add tools/generate_oracles.py tests/oracles/stats_stationarity.json tests/oracles/stats_seasonal.json
git commit -m "Freeze the stationarity and seasonal decomposition corpora"
```

---

### Task 3: The types every member shares

**Files:**

- Create: `src/Lodestar.Stats.TimeSeries/TrendTerms.cs`, `LagSelection.cs`, `KpssLagRule.cs`, `SeasonalModel.cs`, `PValueBound.cs`
- Create: `src/Lodestar.Stats.TimeSeries/DickeyFullerOptions.cs`, `DickeyFullerResult.cs`, `KpssOptions.cs`, `KpssResult.cs`, `SeasonalDecompositionOptions.cs`, `SeasonalComponents.cs`
- Create: `src/Lodestar.Stats.TimeSeries/Internal/SeriesChecks.cs`, `Internal/LineFit.cs`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/OptionsValidationTests.cs`, `LineFitTests.cs`

**Interfaces:**

- Produces: the enums and records exactly as the spec's "Public surface" writes them;
  `internal static class SeriesChecks { void RefuseNonFinite(ReadOnlySpan<double>); void RefuseConstant(ReadOnlySpan<double>); }`;
  `internal static class LineFit { (double Slope, double Intercept) Through(ReadOnlySpan<double> abscissae, ReadOnlySpan<double> ordinates); }`.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Stats.TimeSeries.Tests/OptionsValidationTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Every option refuses where it is set, naming the value.</summary>
public sealed class OptionsValidationTests
{
    [Fact]
    public void A_negative_maximum_lag_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new DickeyFullerOptions { MaxLag = -1 });

    [Fact]
    public void A_null_maximum_lag_means_the_default_rule() =>
        Assert.Null(new DickeyFullerOptions().MaxLag);

    [Theory]
    [InlineData(TrendTerms.None)]
    [InlineData(TrendTerms.ConstantAndQuadraticTrend)]
    public void Kpss_refuses_terms_it_does_not_define(TrendTerms terms) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new KpssOptions { Regression = terms });

    [Fact]
    public void A_negative_kpss_lag_count_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new KpssOptions { LagCount = -1 });

    [Fact]
    public void A_negative_extrapolation_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new SeasonalDecompositionOptions { ExtrapolateTrend = -1 });

    [Fact]
    public void The_defaults_are_the_references() =>
        Assert.Equal(
            (TrendTerms.Constant, LagSelection.Akaike, TrendTerms.Constant, KpssLagRule.Automatic, SeasonalModel.Additive, true, 0),
            (new DickeyFullerOptions().Regression, new DickeyFullerOptions().LagSelection,
             new KpssOptions().Regression, new KpssOptions().LagRule,
             new SeasonalDecompositionOptions().Model, new SeasonalDecompositionOptions().TwoSided,
             new SeasonalDecompositionOptions().ExtrapolateTrend));
}
```

`tests/Lodestar.Stats.TimeSeries.Tests/LineFitTests.cs`:

```csharp
using Lodestar.Stats.TimeSeries.Internal;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>The two-parameter fit KPSS and the trend extrapolation share.</summary>
public sealed class LineFitTests
{
    [Fact]
    public void An_exact_line_comes_back_exactly()
    {
        (double slope, double intercept) = LineFit.Through([1.0, 2.0, 3.0, 4.0], [5.0, 7.0, 9.0, 11.0]);

        Assert.Equal(2.0, slope, 12);
        Assert.Equal(3.0, intercept, 12);
    }

    [Fact]
    public void One_point_takes_the_minimum_norm_solution_lstsq_returns()
    {
        // np.linalg.lstsq([[3, 1]], [5]) is [1.5, 0.5]: the solution of least norm among the line's many.
        (double slope, double intercept) = LineFit.Through([3.0], [5.0]);

        Assert.Equal(1.5, slope, 12);
        Assert.Equal(0.5, intercept, 12);
    }
}
```

- [ ] **Step 2: Run to see them fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```

Expected: build error — `DickeyFullerOptions`, `LineFit` and the rest do not exist.

- [ ] **Step 3: The enums**

`TrendTerms.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>The deterministic terms a unit-root or stationarity regression carries.</summary>
public enum TrendTerms
{
    /// <summary>No constant and no trend — the reference's <c>"n"</c>. ADF only.</summary>
    None,

    /// <summary>A constant — <c>"c"</c>, the default of both tests.</summary>
    Constant,

    /// <summary>A constant and a linear trend — <c>"ct"</c>.</summary>
    ConstantAndTrend,

    /// <summary>A constant, a linear and a quadratic trend — <c>"ctt"</c>. ADF only.</summary>
    ConstantAndQuadraticTrend,
}
```

`LagSelection.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>How the augmented Dickey-Fuller test chooses its lag order.</summary>
public enum LagSelection
{
    /// <summary>The smallest Akaike criterion — the reference's <c>"AIC"</c>, and the default.</summary>
    Akaike,

    /// <summary>The smallest Schwarz criterion — <c>"BIC"</c>.</summary>
    Schwarz,

    /// <summary>Down from the maximum, the first lag whose own t statistic reaches 1.645 — <c>"t-stat"</c>.</summary>
    TStatistic,

    /// <summary>No search: the maximum lag is the lag — <c>autolag=None</c>.</summary>
    Fixed,
}
```

`KpssLagRule.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>How KPSS chooses the lag window of its long-run variance.</summary>
public enum KpssLagRule
{
    /// <summary>Hobijn, Franses and Ooms (1998), from the residuals — <c>"auto"</c>, the default.</summary>
    Automatic,

    /// <summary><c>ceil(12·(n/100)^¼)</c> — <c>"legacy"</c>, the rule before statsmodels 0.12.</summary>
    Legacy,

    /// <summary><see cref="KpssOptions.LagCount"/>, as given.</summary>
    Fixed,
}
```

`SeasonalModel.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>How the seasonal component combines with the trend.</summary>
public enum SeasonalModel
{
    /// <summary><c>series = trend + seasonal + residual</c>, the default.</summary>
    Additive,

    /// <summary><c>series = trend · seasonal · residual</c>, for a season that grows with the level.</summary>
    Multiplicative,
}
```

`PValueBound.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a tabulated p-value was clamped at the end of its table, and which way the truth lies.</summary>
public enum PValueBound
{
    /// <summary>The statistic fell inside the table, and the p-value is interpolated.</summary>
    None,

    /// <summary>The statistic is at or past the table's smallest p-value, which came back; the true one is smaller.</summary>
    ActualIsSmaller,

    /// <summary>The statistic is at or before the table's largest p-value, which came back; the true one is greater.</summary>
    ActualIsGreater,
}
```

- [ ] **Step 4: The options**

`DickeyFullerOptions.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>What an augmented Dickey-Fuller test may be told.</summary>
public sealed record DickeyFullerOptions
{
    private int? _maxLag;

    /// <summary>The deterministic terms of the regression. Default <see cref="TrendTerms.Constant"/>.</summary>
    public TrendTerms Regression { get; init; } = TrendTerms.Constant;

    /// <summary>How the lag order is chosen. Default <see cref="LagSelection.Akaike"/>.</summary>
    public LagSelection LagSelection { get; init; } = LagSelection.Akaike;

    /// <summary>
    /// The largest lag the search considers, or the lag itself under <see cref="LagSelection.Fixed"/>.
    /// Null takes Schwert's <c>ceil(12·(n/100)^¼)</c>, capped at <c>n/2 − terms − 1</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int? MaxLag
    {
        get => _maxLag;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A lag order cannot be negative.");
            }

            _maxLag = value;
        }
    }
}
```

`KpssOptions.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>What a KPSS test may be told.</summary>
public sealed record KpssOptions
{
    private TrendTerms _regression = TrendTerms.Constant;
    private int _lagCount;

    /// <summary>
    /// Stationarity around a level (<see cref="TrendTerms.Constant"/>, the default) or around a line
    /// (<see cref="TrendTerms.ConstantAndTrend"/>).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Any other value: KPSS defines neither.</exception>
    public TrendTerms Regression
    {
        get => _regression;
        init
        {
            if (value is not (TrendTerms.Constant or TrendTerms.ConstantAndTrend))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "KPSS is defined around a level or a line, and nothing else.");
            }

            _regression = value;
        }
    }

    /// <summary>How the lag window is chosen. Default <see cref="KpssLagRule.Automatic"/>.</summary>
    public KpssLagRule LagRule { get; init; } = KpssLagRule.Automatic;

    /// <summary>The lag window under <see cref="KpssLagRule.Fixed"/>; ignored otherwise.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int LagCount
    {
        get => _lagCount;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A lag window cannot be negative.");
            }

            _lagCount = value;
        }
    }
}
```

`SeasonalDecompositionOptions.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>What a seasonal decomposition may be told.</summary>
public sealed record SeasonalDecompositionOptions
{
    private int _extrapolateTrend;

    /// <summary>Additive (the default) or multiplicative.</summary>
    public SeasonalModel Model { get; init; } = SeasonalModel.Additive;

    /// <summary>Whether the moving average is centred (the default) or trails the point.</summary>
    public bool TwoSided { get; init; } = true;

    /// <summary>
    /// How many of the nearest defined trend points, less one, fit the lines that fill the trend's
    /// undefined ends. Zero, the default, leaves them <c>NaN</c>; the reference's <c>"period"</c> is
    /// <c>period − 1</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int ExtrapolateTrend
    {
        get => _extrapolateTrend;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "An extrapolation window cannot be negative.");
            }

            _extrapolateTrend = value;
        }
    }
}
```

- [ ] **Step 5: The results**

`DickeyFullerResult.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>An augmented Dickey-Fuller test: the statistic, its MacKinnon p-value and what the regression used.</summary>
/// <remarks>
/// A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason: a record's equality
/// would compare <see cref="CriticalValues"/> by reference.
/// </remarks>
public sealed class DickeyFullerResult
{
    internal DickeyFullerResult()
    {
    }

    /// <summary>The lagged level's t statistic in the chosen regression.</summary>
    public double Statistic { get; init; }

    /// <summary>MacKinnon's (1994) approximate p-value, against the null of a unit root.</summary>
    public double PValue { get; init; }

    /// <summary>The lag order the regression was fitted at.</summary>
    public int UsedLag { get; init; }

    /// <summary>How many rows that regression fitted: the series length less the lag, less one.</summary>
    public int ObservationCount { get; init; }

    /// <summary>MacKinnon's (2010) critical values at 1 %, 5 % and 10 %, for <see cref="ObservationCount"/>.</summary>
    public IReadOnlyList<double> CriticalValues { get; init; } = [];

    /// <summary>The winning criterion of the lag search; <c>NaN</c> under <see cref="LagSelection.Fixed"/>.</summary>
    public double InformationCriterion { get; init; }
}
```

`KpssResult.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>A KPSS test: the statistic, its tabulated p-value and whether that p-value was clamped.</summary>
/// <remarks>A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason.</remarks>
public sealed class KpssResult
{
    internal KpssResult()
    {
    }

    /// <summary>The residual partial-sum statistic over the long-run variance.</summary>
    public double Statistic { get; init; }

    /// <summary>The p-value against the null of stationarity, interpolated in Kwiatkowski et al.'s table.</summary>
    public double PValue { get; init; }

    /// <summary>The lag window the long-run variance used.</summary>
    public int LagCount { get; init; }

    /// <summary>The critical values at 10 %, 5 %, 2.5 % and 1 %.</summary>
    public IReadOnlyList<double> CriticalValues { get; init; } = [];

    /// <summary>Whether <see cref="PValue"/> is the table's end rather than an interpolation, and which way the truth lies.</summary>
    public PValueBound PValueBound { get; init; }
}
```

`SeasonalComponents.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries;

/// <summary>A series split into its trend, its seasonal pattern and what neither explains.</summary>
/// <remarks>
/// Three lists the length of the series. The trend and the residual are <c>NaN</c> where the moving
/// average has no full window, unless <see cref="SeasonalDecompositionOptions.ExtrapolateTrend"/> filled
/// them. A class rather than a record, for <see cref="AutocorrelationResult"/>'s reason.
/// </remarks>
public sealed class SeasonalComponents
{
    internal SeasonalComponents()
    {
    }

    /// <summary>The centred (or trailing) moving average.</summary>
    public IReadOnlyList<double> Trend { get; init; } = [];

    /// <summary>The average detrended value at each phase, centred, tiled over the series.</summary>
    public IReadOnlyList<double> Seasonal { get; init; } = [];

    /// <summary>The series less (or divided by) the trend and the seasonal component.</summary>
    public IReadOnlyList<double> Residual { get; init; } = [];
}
```

- [ ] **Step 6: The two internals**

`Internal/SeriesChecks.cs`:

```csharp
using System.Globalization;

namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The refusals the stationarity tests and the decomposition share.</summary>
internal static class SeriesChecks
{
    internal static void RefuseNonFinite(ReadOnlySpan<double> series)
    {
        for (int row = 0; row < series.Length; row++)
        {
            // double.IsFinite is not on netstandard2.0, so the two halves are asked separately.
            if (double.IsNaN(series[row]) || double.IsInfinity(series[row]))
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row].ToString(CultureInfo.InvariantCulture)}, which the "
                    + "reference refuses too: a gapped series needs interpolating first.", nameof(series));
            }
        }
    }

    internal static void RefuseConstant(ReadOnlySpan<double> series)
    {
        if (series.Length < 2)
        {
            throw new ArgumentException(
                $"a series of {series.Length} cannot be tested: two points are the minimum.", nameof(series));
        }

        // S1244: constant means exactly constant; a series that merely varies little is testable.
#pragma warning disable S1244
        for (int row = 1; row < series.Length; row++)
        {
            if (series[row] != series[0])
            {
                return;
            }
        }
#pragma warning restore S1244

        throw new ArgumentException(
            $"every value is {series[0].ToString(CultureInfo.InvariantCulture)}: a constant series has no "
            + "variance to test.", nameof(series));
    }
}
```

`Internal/LineFit.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The least-squares line through a handful of points, in closed form.</summary>
/// <remarks>
/// Not <c>OrdinaryLeastSquares.Fit</c>: that refuses a design with no residual degrees of freedom, which
/// a two-point extrapolation window is, and computes an intercept test and VIFs a line does not need.
/// One point takes the minimum-norm solution <c>numpy.linalg.lstsq</c> returns for the reference.
/// </remarks>
internal static class LineFit
{
    internal static (double Slope, double Intercept) Through(
        ReadOnlySpan<double> abscissae, ReadOnlySpan<double> ordinates)
    {
        int count = abscissae.Length;
        if (count == 1)
        {
            double norm = (abscissae[0] * abscissae[0]) + 1.0;
            return (ordinates[0] * abscissae[0] / norm, ordinates[0] / norm);
        }

        double meanX = 0.0;
        double meanY = 0.0;
        for (int i = 0; i < count; i++)
        {
            meanX += abscissae[i];
            meanY += ordinates[i];
        }

        meanX /= count;
        meanY /= count;

        double sxy = 0.0;
        double sxx = 0.0;
        for (int i = 0; i < count; i++)
        {
            double dx = abscissae[i] - meanX;
            sxy += dx * (ordinates[i] - meanY);
            sxx += dx * dx;
        }

        double slope = sxy / sxx;
        return (slope, meanY - (slope * meanX));
    }
}
```

- [ ] **Step 7: Run the tests**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~OptionsValidation|FullyQualifiedName~LineFit"
```

Expected: 9 passed.

- [ ] **Step 8: Commit**

```bash
git add -A && git commit -m "Add the options, results and shared internals of the stationarity lot"
```

---

### Task 4: The augmented Dickey-Fuller test

**Files:**

- Create: `src/Lodestar.Stats.TimeSeries/Internal/MacKinnon.cs`, `Internal/DickeyFullerRegression.cs`, `Stationarity.cs`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityOracleTests.cs`, `StationarityEdgeTests.cs`, `MacKinnonTests.cs`

**Interfaces:**

- Consumes: `DickeyFullerOptions`, `DickeyFullerResult`, `TrendTerms`, `LagSelection`, `SeriesChecks` (Task 3).
- Produces: `public static DickeyFullerResult Stationarity.AugmentedDickeyFuller(ReadOnlySpan<double> series, DickeyFullerOptions? options = null)`;
  `internal static int Stationarity.SchwertLag(int n)`, reused by KPSS in Task 5.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Stats.TimeSeries.Tests/StationarityOracleTests.cs`:

```csharp
using System.Text.Json;
using Lodestar.Stats.TimeSeries.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Replays <c>tests/oracles/stats_stationarity.json</c>.</summary>
public sealed class StationarityOracleTests
{
    [Fact]
    public void Every_adfuller_case_matches_statsmodels() => Replay("adfuller", ReplayDickeyFuller);

    [Fact]
    public void Every_kpss_case_matches_statsmodels() => Replay("kpss", ReplayKpss);

    private static void Replay(string call, Action<JsonElement, string, double[]> replay)
    {
        using JsonDocument document = StatsCorpus.Load("stats_stationarity.json");
        int replayed = 0;
        int expected = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("call").GetString() != call)
            {
                continue;
            }

            expected++;
            replay(c, c.GetProperty("name").GetString()!, StatsCorpus.Doubles(c.GetProperty("series")));
            replayed++;
        }

        Assert.True(expected > 0, $"the corpus holds no {call} case.");
        Assert.Equal(expected, replayed);
    }

    private static void ReplayDickeyFuller(JsonElement c, string name, double[] series)
    {
        JsonElement autolag = c.GetProperty("autolag");
        JsonElement maxlag = c.GetProperty("maxlag");
        DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(series, new DickeyFullerOptions
        {
            Regression = Terms(c.GetProperty("regression").GetString()!),
            LagSelection = autolag.ValueKind == JsonValueKind.Null ? LagSelection.Fixed : autolag.GetString() switch
            {
                "AIC" => LagSelection.Akaike,
                "BIC" => LagSelection.Schwarz,
                "t-stat" => LagSelection.TStatistic,
                var other => throw new InvalidDataException($"Unknown autolag '{other}'."),
            },
            MaxLag = maxlag.ValueKind == JsonValueKind.Null ? null : maxlag.GetInt32(),
        });

        StatsOracleAsserts.Statistic(c.GetProperty("statistic").GetDouble(), result.Statistic, name);
        StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
        Assert.Equal(c.GetProperty("usedlag").GetInt32(), result.UsedLag);
        Assert.Equal(c.GetProperty("nobs").GetInt32(), result.ObservationCount);

        double[] critical = StatsCorpus.Doubles(c.GetProperty("critical"));
        for (int level = 0; level < critical.Length; level++)
        {
            StatsOracleAsserts.Statistic(critical[level], result.CriticalValues[level], $"{name} critical[{level}]");
        }

        JsonElement icbest = c.GetProperty("icbest");
        if (icbest.ValueKind == JsonValueKind.Null)
        {
            Assert.True(double.IsNaN(result.InformationCriterion), $"{name}: a fixed lag reports no criterion.");
        }
        else
        {
            StatsOracleAsserts.PValue(icbest.GetDouble(), result.InformationCriterion, $"{name} icbest");
        }
    }

    private static void ReplayKpss(JsonElement c, string name, double[] series)
    {
        JsonElement nlags = c.GetProperty("nlags");
        KpssResult result = Stationarity.Kpss(series, new KpssOptions
        {
            Regression = Terms(c.GetProperty("regression").GetString()!),
            LagRule = nlags.ValueKind == JsonValueKind.Number ? KpssLagRule.Fixed
                : nlags.GetString() == "legacy" ? KpssLagRule.Legacy : KpssLagRule.Automatic,
            LagCount = nlags.ValueKind == JsonValueKind.Number ? nlags.GetInt32() : 0,
        });

        StatsOracleAsserts.Statistic(c.GetProperty("statistic").GetDouble(), result.Statistic, name);
        StatsOracleAsserts.PValue(c.GetProperty("pvalue").GetDouble(), result.PValue, name);
        Assert.Equal(c.GetProperty("lags").GetInt32(), result.LagCount);

        double[] critical = StatsCorpus.Doubles(c.GetProperty("critical"));
        Assert.Equal(critical, result.CriticalValues);

        PValueBound bound = c.GetProperty("bound").GetString() switch
        {
            "smaller" => PValueBound.ActualIsSmaller,
            "greater" => PValueBound.ActualIsGreater,
            _ => PValueBound.None,
        };
        Assert.Equal(bound, result.PValueBound);
    }

    private static TrendTerms Terms(string regression) => regression switch
    {
        "n" => TrendTerms.None,
        "c" => TrendTerms.Constant,
        "ct" => TrendTerms.ConstantAndTrend,
        "ctt" => TrendTerms.ConstantAndQuadraticTrend,
        _ => throw new InvalidDataException($"Unknown regression '{regression}'."),
    };
}
```

`tests/Lodestar.Stats.TimeSeries.Tests/MacKinnonTests.cs`:

```csharp
using Lodestar.Stats.TimeSeries.Internal;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>What holds of the response surface for every input, not only the corpus's.</summary>
public sealed class MacKinnonTests
{
    [Theory]
    [InlineData(TrendTerms.None)]
    [InlineData(TrendTerms.Constant)]
    [InlineData(TrendTerms.ConstantAndTrend)]
    [InlineData(TrendTerms.ConstantAndQuadraticTrend)]
    public void The_p_value_never_decreases_in_the_statistic(TrendTerms regression)
    {
        double previous = 0.0;
        for (double statistic = -20.0; statistic <= 3.0; statistic += 0.001)
        {
            double p = MacKinnon.PValue(statistic, regression);
            Assert.True(p >= previous - 1e-12, $"{regression}: p({statistic}) = {p} fell below {previous}.");
            previous = p;
        }
    }

    [Fact]
    public void The_normal_cdf_is_symmetric_and_halves_at_zero()
    {
        Assert.Equal(0.5, MacKinnon.NormalCdf(0.0), 15);
        Assert.Equal(1.0, MacKinnon.NormalCdf(1.3) + MacKinnon.NormalCdf(-1.3), 15);
    }
}
```

`tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs` (the ADF half; Task 5 appends KPSS):

```csharp
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Every refusal the spec's table lists, by parameter name and a fragment of the message.</summary>
public sealed class StationarityEdgeTests
{
    private static readonly double[] Walk =
        [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8, 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0];

    [Fact]
    public void Adf_refuses_a_non_finite_value()
    {
        double[] series = (double[])Walk.Clone();
        series[3] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(() => Stationarity.AugmentedDickeyFuller(series));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("row 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_constant_series()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller([2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0]));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("constant", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_maximum_lag_above_what_the_series_supports()
    {
        // 20 points under a constant: 20/2 − 1 − 1 = 8.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(Walk, new DickeyFullerOptions { MaxLag = 9 }));
        Assert.Equal("options", error.ParamName);
        Assert.Contains("8", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Adf_refuses_a_series_too_short_for_its_trend_terms()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.AugmentedDickeyFuller(
                [1.0, 3.0, 2.0, 5.0, 4.0, 7.0],
                new DickeyFullerOptions { Regression = TrendTerms.ConstantAndQuadraticTrend }));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("too short", error.Message, StringComparison.Ordinal);
    }
}
```

- [ ] **Step 2: Run to see them fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```

Expected: build error — `Stationarity`, `MacKinnon` do not exist. (`StationarityOracleTests.Every_kpss_case_matches_statsmodels`
also names `Stationarity.Kpss`; comment the KPSS replay's body out until Task 5 if the build must go green
here, and restore it there.)

- [ ] **Step 3: The response surfaces**

`Internal/MacKinnon.cs`:

```csharp
namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>MacKinnon's response surfaces for one series: the 1994 p-value and the 2010 critical values.</summary>
/// <remarks>
/// Row <c>N = 1</c> of statsmodels 0.15.0's <c>tsa/adfvalues.py</c> (BSD-3-Clause, a permitted reference
/// under decision 0003). The published coefficients are kept beside their scaling and multiplied here as
/// the reference multiplies them, so a transcription can be checked against the paper digit by digit.
/// </remarks>
internal static class MacKinnon
{
    private static readonly double[] SmallScaling = [1.0, 1.0, 1e-2];
    private static readonly double[] LargeScaling = [1.0, 1e-1, 1e-1, 1e-2];

    private static readonly Surface NoTrend = new(
        double.PositiveInfinity, -19.04, -1.04,
        Scaled([0.6344, 1.2378, 3.2496], SmallScaling),
        Scaled([0.4797, 9.3557, -0.6999, 3.3066], LargeScaling),
        [[-2.56574, -2.2358, -3.627, 0.0], [-1.941, -0.2686, -3.365, 31.223], [-1.61682, 0.2656, -2.714, 25.364]]);

    private static readonly Surface Level = new(
        2.74, -18.83, -1.61,
        Scaled([2.1659, 1.4412, 3.8269], SmallScaling),
        Scaled([1.7339, 9.3202, -1.2745, -1.0368], LargeScaling),
        [[-3.43035, -6.5393, -16.786, -79.433], [-2.86154, -2.8903, -4.234, -40.04], [-2.56677, -1.5384, -2.809, 0.0]]);

    private static readonly Surface Trend = new(
        0.7, -16.18, -2.89,
        Scaled([3.2512, 1.6047, 4.9588], SmallScaling),
        Scaled([2.5261, 6.1654, -3.7956, -6.0285], LargeScaling),
        [[-3.95877, -9.0531, -28.428, -134.155], [-3.41049, -4.3904, -9.036, -45.374], [-3.12705, -2.5856, -3.925, -22.38]]);

    private static readonly Surface QuadraticTrend = new(
        0.54, -17.17, -3.21,
        Scaled([4.0003, 1.658, 4.8288], SmallScaling),
        Scaled([3.0778, 4.9529, -4.1477, -5.9359], LargeScaling),
        [[-4.37113, -11.5882, -35.819, -334.047], [-3.83239, -5.9057, -12.49, -118.284], [-3.55326, -3.6596, -5.293, -63.559]]);

    /// <summary>MacKinnon's approximate p-value: exactly 1 above the table, exactly 0 below it.</summary>
    internal static double PValue(double statistic, TrendTerms regression)
    {
        Surface surface = For(regression);
        if (statistic > surface.Max)
        {
            return 1.0;
        }

        if (statistic < surface.Min)
        {
            return 0.0;
        }

        double[] coefficients = statistic <= surface.Switch ? surface.Small : surface.Large;
        return NormalCdf(Horner(coefficients, statistic));
    }

    /// <summary>The 1 %, 5 % and 10 % critical values for a regression of <paramref name="observations"/> rows.</summary>
    internal static double[] CriticalValues(int observations, TrendTerms regression)
    {
        Surface surface = For(regression);
        double inverse = 1.0 / observations;
        return [Horner(surface.Critical[0], inverse), Horner(surface.Critical[1], inverse), Horner(surface.Critical[2], inverse)];
    }

    /// <summary>The standard normal CDF, through the one tail <c>Lodestar.Stats</c> publishes that reaches it.</summary>
    /// <remarks><c>Φ(z) = ½·P(χ²₁ ≥ z²)</c> below zero, and one minus that above; no normal CDF is published.</remarks>
    internal static double NormalCdf(double z)
    {
        double half = 0.5 * Distributions.ChiSquaredSf(z * z, 1.0);
        return z < 0.0 ? half : 1.0 - half;
    }

    /// <summary><c>c₀ + c₁x + c₂x² + …</c> in numpy's <c>polyval</c> order, highest coefficient first.</summary>
    private static double Horner(double[] coefficients, double x)
    {
        double value = 0.0;
        for (int i = coefficients.Length - 1; i >= 0; i--)
        {
            value = (value * x) + coefficients[i];
        }

        return value;
    }

    private static double[] Scaled(double[] published, double[] scaling)
    {
        var scaled = new double[published.Length];
        for (int i = 0; i < published.Length; i++)
        {
            scaled[i] = published[i] * scaling[i];
        }

        return scaled;
    }

    private static Surface For(TrendTerms regression) => regression switch
    {
        TrendTerms.None => NoTrend,
        TrendTerms.Constant => Level,
        TrendTerms.ConstantAndTrend => Trend,
        TrendTerms.ConstantAndQuadraticTrend => QuadraticTrend,
        _ => throw new ArgumentOutOfRangeException(nameof(regression), regression, "Not a trend specification."),
    };

    private sealed record Surface(double Max, double Min, double Switch, double[] Small, double[] Large, double[][] Critical);
}
```

`using Lodestar.Stats;` at the top of the file, for `Distributions`.

- [ ] **Step 4: The regression**

`Internal/DickeyFullerRegression.cs`:

```csharp
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>One augmented Dickey-Fuller regression, laid out the way the reference lays out its lag search.</summary>
/// <remarks>
/// long-comment: why the trend columns come first.
/// Row <c>r</c> of <paramref name="rows"/> is series position <c>t = n − rows + r</c>: the response is
/// <c>Δx[t]</c>, and the regressors are the trend columns <c>r + 1</c> and <c>(r + 1)²</c>, then the lagged
/// level <c>x[t − 1]</c>, then <c>Δx[t − 1] … Δx[t − lag]</c>. The reference's lag search prepends its
/// trend terms, so the last coefficient is the highest lag — which is the one the t-statistic rule reads —
/// and the constant is the fit's intercept. Measured: with the trend columns last instead, the t-statistic
/// rule chose lag 0 where the reference chose 10 on a 200-point AR(1).
/// </remarks>
internal static class DickeyFullerRegression
{
    internal static int TrendColumns(TrendTerms regression) => regression switch
    {
        TrendTerms.ConstantAndTrend => 1,
        TrendTerms.ConstantAndQuadraticTrend => 2,
        _ => 0,
    };

    internal static int TermCount(TrendTerms regression) => regression switch
    {
        TrendTerms.None => 0,
        TrendTerms.Constant => 1,
        TrendTerms.ConstantAndTrend => 2,
        TrendTerms.ConstantAndQuadraticTrend => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(regression), regression, "Not a trend specification."),
    };

    /// <summary>Where the lagged level's coefficient sits in the summary: after the intercept and the trend columns.</summary>
    internal static int LevelIndex(TrendTerms regression) =>
        (regression == TrendTerms.None ? 0 : 1) + TrendColumns(regression);

    internal static OlsSummary Fit(ReadOnlySpan<double> series, TrendTerms regression, int lag, int rows)
    {
        int n = series.Length;
        int trend = TrendColumns(regression);
        int features = trend + 1 + lag;
        var design = new double[rows * features];
        var response = new double[rows];

        for (int row = 0; row < rows; row++)
        {
            int t = n - rows + row;
            int offset = row * features;
            response[row] = series[t] - series[t - 1];
            if (trend >= 1)
            {
                design[offset] = row + 1;
            }

            if (trend == 2)
            {
                design[offset + 1] = (double)(row + 1) * (row + 1);
            }

            design[offset + trend] = series[t - 1];
            for (int k = 1; k <= lag; k++)
            {
                design[offset + trend + k] = series[t - k] - series[t - k - 1];
            }
        }

        return OrdinaryLeastSquares.Fit(
            design, response, features, new OlsOptions { WithIntercept = regression != TrendTerms.None });
    }

    /// <summary>Akaike's or Schwarz's criterion, from what the summary exposes.</summary>
    /// <remarks>
    /// <c>ℓ = −rows/2·(log 2π + log(SSR/rows) + 1)</c>, and the parameter count includes the intercept, as
    /// statsmodels' <c>df_model + k_constant</c> does. SSR is <c>RSE²·df</c>: the summary carries no SSR.
    /// </remarks>
    internal static double Criterion(OlsSummary fit, int rows, LagSelection selection)
    {
        double ssr = fit.ResidualStandardError * fit.ResidualStandardError * fit.ResidualDegreesOfFreedom;
        double logLikelihood = -rows / 2.0 * (Math.Log(2.0 * Math.PI) + Math.Log(ssr / rows) + 1.0);
        int parameters = fit.Coefficients.Count;
        double penalty = selection == LagSelection.Schwarz ? Math.Log(rows) * parameters : 2.0 * parameters;
        return (-2.0 * logLikelihood) + penalty;
    }
}
```

- [ ] **Step 5: The entry point**

`Stationarity.cs`:

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a series may be modelled as it stands: the unit-root test and its complement.</summary>
/// <remarks>
/// The two nulls are opposite. The augmented Dickey-Fuller test assumes a unit root and KPSS assumes
/// stationarity, so a reader runs both: rejecting one and not the other, from opposite sides, is the
/// answer either alone cannot give.
/// </remarks>
public static class Stationarity
{
    // The reference's literal for norm.ppf(0.95), kept as written rather than recomputed.
    private const double OneSidedFivePercent = 1.6448536269514722;

    /// <summary>The augmented Dickey-Fuller test, against the null of a unit root.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="options">The trend terms, the lag rule and its maximum, or null for the reference's defaults.</param>
    /// <returns>The statistic, MacKinnon's p-value and critical values, and the lag the regression used.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, is constant, or is too short for its trend
    /// terms; or <paramref name="options"/> asks for a maximum lag above <c>n/2 − terms − 1</c>.
    /// </exception>
    public static DickeyFullerResult AugmentedDickeyFuller(
        ReadOnlySpan<double> series, DickeyFullerOptions? options = null)
    {
        DickeyFullerOptions settings = options ?? new DickeyFullerOptions();
        SeriesChecks.RefuseNonFinite(series);
        SeriesChecks.RefuseConstant(series);

        int n = series.Length;
        int maxLag = MaxLag(n, settings);

        int usedLag = maxLag;
        double criterion = double.NaN;
        if (settings.LagSelection != LagSelection.Fixed)
        {
            (usedLag, criterion) = SearchLag(series, settings, maxLag, n - maxLag - 1);
        }

        int observations = n - usedLag - 1;
        OlsSummary fit = DickeyFullerRegression.Fit(series, settings.Regression, usedLag, observations);
        double statistic = fit.TStatistics[DickeyFullerRegression.LevelIndex(settings.Regression)];

        return new DickeyFullerResult
        {
            Statistic = statistic,
            PValue = MacKinnon.PValue(statistic, settings.Regression),
            UsedLag = usedLag,
            ObservationCount = observations,
            CriticalValues = MacKinnon.CriticalValues(observations, settings.Regression),
            InformationCriterion = criterion,
        };
    }

    /// <summary>Schwert's rule, <c>ceil(12·(n/100)^¼)</c>, which both tests default to.</summary>
    internal static int SchwertLag(int n) => (int)Math.Ceiling(12.0 * Math.Pow(n / 100.0, 0.25));

    private static int MaxLag(int n, DickeyFullerOptions settings)
    {
        int terms = DickeyFullerRegression.TermCount(settings.Regression);
        int ceiling = (n / 2) - terms - 1;
        if (settings.MaxLag is int given)
        {
            if (given > ceiling)
            {
                throw new ArgumentException(
                    $"a maximum lag of {given} is above the {ceiling} a series of {n} supports with {terms} "
                    + "trend terms: n/2 − terms − 1.", nameof(options));
            }

            return given;
        }

        int schwert = Math.Min(ceiling, SchwertLag(n));
        if (schwert < 0)
        {
            throw new ArgumentException(
                $"a series of {n} is too short for {terms} trend terms and a lagged level.", nameof(series));
        }

        return schwert;
    }

    /// <summary>The reference's lag search, every candidate over the same <paramref name="rows"/> rows.</summary>
    private static (int Lag, double Criterion) SearchLag(
        ReadOnlySpan<double> series, DickeyFullerOptions settings, int maxLag, int rows)
    {
        if (settings.LagSelection == LagSelection.TStatistic)
        {
            double absolute = 0.0;
            for (int lag = maxLag; lag >= 0; lag--)
            {
                OlsSummary fit = DickeyFullerRegression.Fit(series, settings.Regression, lag, rows);
                absolute = Math.Abs(fit.TStatistics[fit.TStatistics.Count - 1]);
                if (absolute >= OneSidedFivePercent)
                {
                    return (lag, absolute);
                }
            }

            return (0, absolute);
        }

        int best = 0;
        double bestCriterion = double.PositiveInfinity;
        for (int lag = 0; lag <= maxLag; lag++)
        {
            double value = DickeyFullerRegression.Criterion(
                DickeyFullerRegression.Fit(series, settings.Regression, lag, rows), rows, settings.LagSelection);

            // Strictly smaller: a tie keeps the shorter lag, as the reference's min over (criterion, lag) does.
            if (value < bestCriterion)
            {
                bestCriterion = value;
                best = lag;
            }
        }

        return (best, bestCriterion);
    }
}
```

`nameof(options)` and `nameof(series)` inside `MaxLag` name the public method's parameters: pass both
through as `string` arguments if the analyzer refuses a `nameof` of a parameter the private method does
not have (CA2208) — `MaxLag(int n, DickeyFullerOptions settings, string optionsName, string seriesName)`.

- [ ] **Step 6: Run the tests**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~Stationarity|FullyQualifiedName~MacKinnon"
```

Expected: the ADF replay, the four ADF refusals and the MacKinnon properties pass.

- [ ] **Step 7: Commit**

```bash
git add -A && git commit -m "Add the augmented Dickey-Fuller test with MacKinnon's response surfaces"
```

---

### Task 5: KPSS

**Files:**

- Modify: `src/Lodestar.Stats.TimeSeries/Stationarity.cs`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/StationarityEdgeTests.cs` (append), `StationarityOracleTests.cs` (the KPSS replay, restored if Task 4 commented it)

**Interfaces:**

- Consumes: `KpssOptions`, `KpssResult`, `KpssLagRule`, `PValueBound`, `LineFit`, `SeriesChecks`, `Stationarity.SchwertLag`.
- Produces: `public static KpssResult Stationarity.Kpss(ReadOnlySpan<double> series, KpssOptions? options = null)`.

- [ ] **Step 1: The failing refusals**

Appended to `StationarityEdgeTests`:

```csharp
    [Fact]
    public void Kpss_refuses_a_constant_series()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => Stationarity.Kpss([4.0, 4.0, 4.0, 4.0, 4.0]));
        Assert.Equal("series", error.ParamName);
    }

    [Fact]
    public void Kpss_refuses_a_fixed_window_at_the_series_length()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Stationarity.Kpss(Walk, new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = Walk.Length }));
        Assert.Equal("options", error.ParamName);
        Assert.Contains("20", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Kpss_on_a_walk_rejects_what_adf_cannot()
    {
        double[] walk = new double[200];
        var random = new Random(671);
        for (int i = 1; i < walk.Length; i++)
        {
            walk[i] = walk[i - 1] + (random.NextDouble() - 0.5);
        }

        Assert.True(Stationarity.AugmentedDickeyFuller(walk).PValue > 0.05);
        Assert.True(Stationarity.Kpss(walk).PValue < 0.05);
    }
```

The last test is a property the two opposite nulls owe each other on a strong random walk, and a seeded
`Random` with no security use: `#pragma warning disable S2245, CA5394` around it, reason above.

- [ ] **Step 2: Run to see them fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```

Expected: build error — `Stationarity.Kpss` does not exist.

- [ ] **Step 3: Implement**

Added to `Stationarity`:

```csharp
    private static readonly double[] KpssPValues = [0.10, 0.05, 0.025, 0.01];
    private static readonly double[] LevelCritical = [0.347, 0.463, 0.574, 0.739];
    private static readonly double[] TrendCritical = [0.119, 0.146, 0.176, 0.216];

    /// <summary>The KPSS test, against the null of stationarity around a level or a line.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="options">The null's trend and the lag window rule, or null for the reference's defaults.</param>
    /// <returns>The statistic, its tabulated p-value, the window used, and whether the p-value was clamped.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value or is constant; or <paramref name="options"/> fixes a
    /// window at or above the series length.
    /// </exception>
    public static KpssResult Kpss(ReadOnlySpan<double> series, KpssOptions? options = null)
    {
        KpssOptions settings = options ?? new KpssOptions();
        SeriesChecks.RefuseNonFinite(series);
        SeriesChecks.RefuseConstant(series);

        int n = series.Length;
        double[] residuals = KpssResiduals(series, settings.Regression);
        int lagCount = settings.LagRule switch
        {
            KpssLagRule.Legacy => Math.Min(SchwertLag(n), n - 1),
            KpssLagRule.Automatic => Math.Min(HobijnLag(residuals), n - 1),
            KpssLagRule.Fixed when settings.LagCount < n => settings.LagCount,
            KpssLagRule.Fixed => throw new ArgumentException(
                $"a lag window of {settings.LagCount} reaches the {n} observations; it must stay below them.",
                nameof(options)),
            _ => throw new ArgumentOutOfRangeException(nameof(options), settings.LagRule, "Not a KPSS lag rule."),
        };

        double partial = 0.0;
        double eta = 0.0;
        foreach (double residual in residuals)
        {
            partial += residual;
            eta += partial * partial;
        }

        double statistic = eta / ((double)n * n) / LongRunVariance(residuals, lagCount);
        double[] critical = settings.Regression == TrendTerms.ConstantAndTrend ? TrendCritical : LevelCritical;
        double pValue = Interpolate(statistic, critical);

        return new KpssResult
        {
            Statistic = statistic,
            PValue = pValue,
            LagCount = lagCount,
            CriticalValues = (double[])critical.Clone(),
            PValueBound = BoundOf(pValue),
        };
    }

    private static double[] KpssResiduals(ReadOnlySpan<double> series, TrendTerms regression)
    {
        int n = series.Length;
        var residuals = new double[n];
        if (regression == TrendTerms.ConstantAndTrend)
        {
            var time = new double[n];
            for (int i = 0; i < n; i++)
            {
                time[i] = i + 1;
            }

            (double slope, double intercept) = LineFit.Through(time, series);
            for (int i = 0; i < n; i++)
            {
                residuals[i] = series[i] - (intercept + (slope * time[i]));
            }

            return residuals;
        }

        double mean = 0.0;
        foreach (double value in series)
        {
            mean += value;
        }

        mean /= n;
        for (int i = 0; i < n; i++)
        {
            residuals[i] = series[i] - mean;
        }

        return residuals;
    }

    /// <summary>Hobijn, Franses and Ooms' window, as the reference writes it.</summary>
    private static int HobijnLag(double[] residuals)
    {
        int n = residuals.Length;
        int covariances = (int)Math.Pow(n, 2.0 / 9.0);
        double s0 = SumOfSquares(residuals) / n;
        double s1 = 0.0;
        for (int i = 1; i <= covariances; i++)
        {
            double product = LaggedProduct(residuals, i) / (n / 2.0);
            s0 += product;
            s1 += i * product;
        }

        double ratio = s1 / s0;
        double gamma = 1.1447 * Math.Pow(ratio * ratio, 1.0 / 3.0);
        return (int)(gamma * Math.Pow(n, 1.0 / 3.0));
    }

    /// <summary>The Newey-West long-run variance with a Bartlett kernel.</summary>
    private static double LongRunVariance(double[] residuals, int lagCount)
    {
        double sum = SumOfSquares(residuals);
        for (int i = 1; i <= lagCount; i++)
        {
            sum += 2.0 * LaggedProduct(residuals, i) * (1.0 - (i / (lagCount + 1.0)));
        }

        return sum / residuals.Length;
    }

    /// <summary><c>numpy.interp</c> over the table: linear inside, clamped to the end values outside.</summary>
    private static double Interpolate(double statistic, double[] critical)
    {
        if (statistic <= critical[0])
        {
            return KpssPValues[0];
        }

        if (statistic >= critical[critical.Length - 1])
        {
            return KpssPValues[KpssPValues.Length - 1];
        }

        int segment = 0;
        while (statistic >= critical[segment + 1])
        {
            segment++;
        }

        double slope = (KpssPValues[segment + 1] - KpssPValues[segment]) / (critical[segment + 1] - critical[segment]);
        return (slope * (statistic - critical[segment])) + KpssPValues[segment];
    }

    /// <summary>The reference warns exactly when the returned p-value equals an end of the table.</summary>
    private static PValueBound BoundOf(double pValue)
    {
        // S1244: the reference compares p_value == pvals[-1] exactly; an interpolation never lands there.
#pragma warning disable S1244
        if (pValue == KpssPValues[KpssPValues.Length - 1])
        {
            return PValueBound.ActualIsSmaller;
        }

        return pValue == KpssPValues[0] ? PValueBound.ActualIsGreater : PValueBound.None;
#pragma warning restore S1244
    }

    private static double SumOfSquares(double[] values)
    {
        double sum = 0.0;
        foreach (double value in values)
        {
            sum += value * value;
        }

        return sum;
    }

    private static double LaggedProduct(double[] residuals, int lag)
    {
        double sum = 0.0;
        for (int t = lag; t < residuals.Length; t++)
        {
            sum += residuals[t] * residuals[t - lag];
        }

        return sum;
    }
```

- [ ] **Step 4: Run the tests**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release --filter "FullyQualifiedName~Stationarity"
```

Expected: both replays and seven refusals and properties pass.

- [ ] **Step 5: Commit**

```bash
git add -A && git commit -m "Add KPSS, its lag rules and the clamped p-value's direction"
```

---

### Task 6: Seasonal decomposition

**Files:**

- Create: `src/Lodestar.Stats.TimeSeries/SeasonalDecomposition.cs`
- Test: `tests/Lodestar.Stats.TimeSeries.Tests/SeasonalDecompositionOracleTests.cs`, `SeasonalDecompositionEdgeTests.cs`

**Interfaces:**

- Consumes: `SeasonalDecompositionOptions`, `SeasonalComponents`, `SeasonalModel`, `LineFit`, `SeriesChecks`.
- Produces: `public static SeasonalComponents SeasonalDecomposition.Decompose(ReadOnlySpan<double> series, int period, SeasonalDecompositionOptions? options = null)`.

- [ ] **Step 1: Write the failing tests**

`SeasonalDecompositionOracleTests.cs`:

```csharp
using System.Text.Json;
using Lodestar.Stats.TimeSeries.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Replays <c>tests/oracles/stats_seasonal.json</c>.</summary>
public sealed class SeasonalDecompositionOracleTests
{
    [Fact]
    public void Every_case_matches_statsmodels()
    {
        using JsonDocument document = StatsCorpus.Load("stats_seasonal.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            SeasonalComponents result = SeasonalDecomposition.Decompose(
                StatsCorpus.Doubles(c.GetProperty("series")),
                c.GetProperty("period").GetInt32(),
                new SeasonalDecompositionOptions
                {
                    Model = c.GetProperty("model").GetString() == "multiplicative"
                        ? SeasonalModel.Multiplicative : SeasonalModel.Additive,
                    TwoSided = c.GetProperty("two_sided").GetBoolean(),
                    ExtrapolateTrend = c.GetProperty("extrapolate_trend").GetInt32(),
                });

            Components(StatsCorpus.Doubles(c.GetProperty("trend")), result.Trend, $"{name} trend");
            Components(StatsCorpus.Doubles(c.GetProperty("seasonal")), result.Seasonal, $"{name} seasonal");
            Components(StatsCorpus.Doubles(c.GetProperty("resid")), result.Residual, $"{name} residual");
            replayed++;
        }

        Assert.Equal(
            document.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static void Components(double[] expected, IReadOnlyList<double> actual, string name)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            StatsOracleAsserts.Statistic(expected[i], actual[i], $"{name}[{i}]");
        }
    }
}
```

`SeasonalDecompositionEdgeTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>The decomposition's refusals, and what holds of it for every input.</summary>
public sealed class SeasonalDecompositionEdgeTests
{
    private static readonly double[] Quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

    [Fact]
    public void A_period_below_two_is_refused() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => SeasonalDecomposition.Decompose(Quarterly, 1));

    [Fact]
    public void Fewer_than_two_full_periods_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => SeasonalDecomposition.Decompose(Quarterly, 7));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("14", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Multiplicative_refuses_a_value_at_or_below_zero()
    {
        double[] series = (double[])Quarterly.Clone();
        series[5] = 0.0;

        ArgumentException error = Assert.Throws<ArgumentException>(() => SeasonalDecomposition.Decompose(
            series, 4, new SeasonalDecompositionOptions { Model = SeasonalModel.Multiplicative }));
        Assert.Equal("series", error.ParamName);
        Assert.Contains("row 5", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Additive_components_add_back_to_the_series_wherever_the_trend_is_defined()
    {
        SeasonalComponents components = SeasonalDecomposition.Decompose(Quarterly, 4);

        for (int i = 0; i < Quarterly.Length; i++)
        {
            if (!double.IsNaN(components.Trend[i]))
            {
                Assert.Equal(Quarterly[i], components.Trend[i] + components.Seasonal[i] + components.Residual[i], 12);
            }
        }
    }

    [Theory]
    [InlineData(SeasonalModel.Additive, 0.0)]
    [InlineData(SeasonalModel.Multiplicative, 1.0)]
    public void The_seasonal_pattern_centres_on_its_identity(SeasonalModel model, double identity)
    {
        SeasonalComponents components = SeasonalDecomposition.Decompose(
            Quarterly, 4, new SeasonalDecompositionOptions { Model = model });

        double mean = (components.Seasonal[0] + components.Seasonal[1] + components.Seasonal[2] + components.Seasonal[3]) / 4.0;
        Assert.Equal(identity, mean, 12);
    }
}
```

- [ ] **Step 2: Run to see them fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```

Expected: build error — `SeasonalDecomposition` does not exist.

- [ ] **Step 3: Implement**

`SeasonalDecomposition.cs`:

```csharp
using System.Globalization;
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Classical decomposition of a series into trend, seasonal pattern and residual, by moving averages.</summary>
public static class SeasonalDecomposition
{
    /// <summary>Splits <paramref name="series"/> into its trend, seasonal and residual components.</summary>
    /// <param name="series">The observations, in time order, at least two full periods of them.</param>
    /// <param name="period">
    /// The season's length in observations: 12 for monthly data with a yearly season. Required — the
    /// reference infers it from a pandas index, which a span does not have.
    /// </param>
    /// <param name="options">The model, the filter's sides and the trend extrapolation, or null for the defaults.</param>
    /// <returns>Three lists the length of the series.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is below two.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, holds fewer than two periods, or, under
    /// <see cref="SeasonalModel.Multiplicative"/>, a value at or below zero.
    /// </exception>
    public static SeasonalComponents Decompose(
        ReadOnlySpan<double> series, int period, SeasonalDecompositionOptions? options = null)
    {
        SeasonalDecompositionOptions settings = options ?? new SeasonalDecompositionOptions();
        Guard.NotLessThan(period, 2);
        SeriesChecks.RefuseNonFinite(series);
        Refuse(series, period, settings.Model);

        bool multiplicative = settings.Model == SeasonalModel.Multiplicative;
        double[] trend = MovingAverage(series, period, settings.TwoSided);
        if (settings.ExtrapolateTrend > 0)
        {
            Extrapolate(trend, settings.ExtrapolateTrend + 1);
        }

        int n = series.Length;
        var detrended = new double[n];
        for (int i = 0; i < n; i++)
        {
            detrended[i] = multiplicative ? series[i] / trend[i] : series[i] - trend[i];
        }

        double[] pattern = PhaseAverages(detrended, period, multiplicative);
        var seasonal = new double[n];
        var residual = new double[n];
        for (int i = 0; i < n; i++)
        {
            seasonal[i] = pattern[i % period];
            residual[i] = multiplicative ? series[i] / seasonal[i] / trend[i] : detrended[i] - seasonal[i];
        }

        return new SeasonalComponents { Trend = trend, Seasonal = seasonal, Residual = residual };
    }

    private static void Refuse(ReadOnlySpan<double> series, int period, SeasonalModel model)
    {
        if (series.Length < 2 * period)
        {
            throw new ArgumentException(
                $"a period of {period} needs two full cycles, {2 * period} observations; the series holds "
                + $"{series.Length}.", nameof(series));
        }

        if (model != SeasonalModel.Multiplicative)
        {
            return;
        }

        for (int row = 0; row < series.Length; row++)
        {
            if (series[row] <= 0.0)
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row].ToString(CultureInfo.InvariantCulture)}: a multiplicative "
                    + "season divides by the level, which must stay above zero.", nameof(series));
            }
        }
    }

    /// <summary>The reference's default filter, applied as <c>scipy.signal.convolve(mode="valid")</c> and padded with NaN.</summary>
    /// <remarks>
    /// An even period weights its two end points by one half over <c>period + 1</c> points, an odd one
    /// weights <c>period</c> points equally. Both filters are symmetric, so convolution and correlation agree.
    /// </remarks>
    private static double[] MovingAverage(ReadOnlySpan<double> series, int period, bool twoSided)
    {
        int length = period % 2 == 0 ? period + 1 : period;
        var weights = new double[length];
        for (int k = 0; k < length; k++)
        {
            bool halfWeight = period % 2 == 0 && (k == 0 || k == length - 1);
            weights[k] = (halfWeight ? 0.5 : 1.0) / period;
        }

        int n = series.Length;
        var trend = new double[n];
        for (int i = 0; i < n; i++)
        {
            trend[i] = double.NaN;
        }

        int head = twoSided ? ((length + 1) / 2) - 1 : length - 1;
        for (int start = 0; start + length <= n; start++)
        {
            double sum = 0.0;
            for (int k = 0; k < length; k++)
            {
                sum += series[start + k] * weights[length - 1 - k];
            }

            trend[start + head] = sum;
        }

        return trend;
    }

    /// <summary>Fills the NaN ends by least-squares lines, over the reference's own windows.</summary>
    /// <remarks>
    /// The back window is <c>[back − npoints, back)</c> and so leaves the last defined point out, exactly as
    /// <c>trend[back_first:back]</c> does in statsmodels; kept as written, since the corpus freezes it.
    /// </remarks>
    private static void Extrapolate(double[] trend, int points)
    {
        int n = trend.Length;
        int front = 0;
        while (double.IsNaN(trend[front]))
        {
            front++;
        }

        int back = n - 1;
        while (double.IsNaN(trend[back]))
        {
            back--;
        }

        int frontLast = Math.Min(front + points, back);
        (double frontSlope, double frontIntercept) = FitWindow(trend, front, frontLast);
        for (int i = 0; i < front; i++)
        {
            trend[i] = (i * frontSlope) + frontIntercept;
        }

        int backFirst = Math.Max(front, back - points);
        (double backSlope, double backIntercept) = FitWindow(trend, backFirst, back);
        for (int i = back + 1; i < n; i++)
        {
            trend[i] = (i * backSlope) + backIntercept;
        }
    }

    private static (double Slope, double Intercept) FitWindow(double[] trend, int from, int to)
    {
        var positions = new double[to - from];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = from + i;
        }

        return LineFit.Through(positions, trend.AsSpan(from, to - from));
    }

    /// <summary>The mean detrended value at each phase, ignoring NaN, centred on zero or one.</summary>
    private static double[] PhaseAverages(double[] detrended, int period, bool multiplicative)
    {
        var averages = new double[period];
        for (int phase = 0; phase < period; phase++)
        {
            double sum = 0.0;
            int count = 0;
            for (int i = phase; i < detrended.Length; i += period)
            {
                if (!double.IsNaN(detrended[i]))
                {
                    sum += detrended[i];
                    count++;
                }
            }

            averages[phase] = count == 0 ? double.NaN : sum / count;
        }

        double centre = 0.0;
        foreach (double average in averages)
        {
            centre += average;
        }

        centre /= period;
        for (int phase = 0; phase < period; phase++)
        {
            averages[phase] = multiplicative ? averages[phase] / centre : averages[phase] - centre;
        }

        return averages;
    }
}
```

`((length + 1) / 2) - 1` is `ceil(length/2) − 1` in integers; the spike's `Math.Ceiling(L / 2.0) - 1`
agreed with it on every case.

- [ ] **Step 4: Run the tests**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```

Expected: every test in the suite passes, the moved `SerialCorrelation` replay included.

- [ ] **Step 5: Commit**

```bash
git add -A && git commit -m "Add classical seasonal decomposition, additive and multiplicative"
```

---

### Task 7: The documents, the samples and the record

**Files:**

- Create: `docs/reference/stats-timeseries/stationarity-tests.md`, `stationarity-tests/{stationarity,stationarity-augmenteddickeyfuller,stationarity-kpss,dickeyfulleroptions,dickeyfullerresult,kpssoptions,kpssresult}.md`, `docs/reference/stats-timeseries/seasonality.md`, `seasonality/{seasonaldecomposition,seasonaldecomposition-decompose,seasonaldecompositionoptions,seasonalcomponents}.md`
- Create: `docs/guides/time-series-diagnostics.md`
- Create: `samples/Lodestar.Sample/{Stationarity,DickeyFullerOptions,DickeyFullerResult,KpssOptions,KpssResult,SeasonalDecomposition,SeasonalDecompositionOptions,SeasonalComponents}Sample.cs`
- Create: `docs/decisions/<next>-stats-timeseries-is-a-package-and-takes-the-serial-correlation-lot.md`
- Modify: `samples/Lodestar.Sample/Program.cs`, `docs/equivalence.md`, `docs/migration/statsmodels.md`, `docs/migration/README.md`, `docs/decisions/README.md`, `docs/decisions/index.yaml` (regenerated), `CHANGELOG.md`, `README.md`

- [ ] **Step 1: Reference pages**

Each page in the layout the moved `correlation/` pages use: title, one-line summary,
`<!-- docs-declaration -->`, the declaration fence, **Parameters**/**Properties**, **Returns**,
**Exceptions**, **Example** with `// =>` assertions, **Remarks**, **Applies to**, **See also**. The
examples use one series each, whose values were computed with `statsmodels` 0.15.0 on 2026-09-15:

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult adf = Stationarity.AugmentedDickeyFuller(walk);
double statistic = Math.Round(adf.Statistic, 4);          // => 1.9817
double p = Math.Round(adf.PValue, 4);                     // => 0.9986
int used = adf.UsedLag;                                   // => 3
int rows = adf.ObservationCount;                          // => 20
double fivePercent = Math.Round(adf.CriticalValues[1], 4);  // => -3.0216
double aic = Math.Round(adf.InformationCriterion, 4);     // => -37.8399

DickeyFullerResult trended = Stationarity.AugmentedDickeyFuller(
    walk, new DickeyFullerOptions { Regression = TrendTerms.ConstantAndTrend, LagSelection = LagSelection.Fixed, MaxLag = 1 });
double trendStatistic = Math.Round(trended.Statistic, 4);  // => -8.9132
double trendP = trended.PValue;                            // => 0

KpssResult level = Stationarity.Kpss(walk);
double kpssStatistic = Math.Round(level.Statistic, 4);     // => 0.7082
double kpssP = Math.Round(level.PValue, 4);                // => 0.0128
int window = level.LagCount;                               // => 3

KpssResult line = Stationarity.Kpss(walk, new KpssOptions { Regression = TrendTerms.ConstantAndTrend });
double lineStatistic = Math.Round(line.Statistic, 4);      // => 0.1533
double lineP = Math.Round(line.PValue, 4);                 // => 0.0439
int lineWindow = line.LagCount;                            // => 9

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];
SeasonalComponents parts = SeasonalDecomposition.Decompose(quarterly, period: 4);
bool undefinedStart = double.IsNaN(parts.Trend[0]);        // => True
double thirdTrend = parts.Trend[2];                        // => 11.125
double secondSeason = parts.Seasonal[1];                   // => 3.125

SeasonalComponents filled = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { ExtrapolateTrend = 1 });
double firstTrend = filled.Trend[0];                       // => 10.625
double lastTrend = filled.Trend[11];                       // => 13.375

SeasonalComponents scaled = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { Model = SeasonalModel.Multiplicative });
double secondFactor = Math.Round(scaled.Seasonal[1], 4);   // => 1.2577
```

Each fence is split across the page it documents; the doc-snippet runner executes every one, so a wrong
number fails CI rather than a reader. `stationarity-tests.md` and `seasonality.md` are the namespace-section
overviews in `correlation.md`'s shape; `correlation.md`'s first line gains the package name.

- [ ] **Step 2: `docs/equivalence.md`**

The section `## Lodestar.Stats.TimeSeries — serial-correlation diagnostics` keeps its rows with links one
folder deeper, and two sections follow it:

```markdown
## Lodestar.Stats.TimeSeries — stationarity

| Python | Library | C# | Differences |
| --- | --- | --- | --- |
| `adfuller(x, maxlag=None, regression="c", autolag="AIC")` | statsmodels | [`Stationarity.AugmentedDickeyFuller(series, options)`](reference/stats-timeseries/stationarity-tests/stationarity-augmenteddickeyfuller.md) | Identical statistic, p-value, lag, observation count, critical values and `icbest`, every `regression` and `autolag`. The regressions are solved by `OrdinaryLeastSquares.Fit`'s Householder QR where the reference uses a pseudo-inverse: an ordering difference, measured at `5.5e-12` worst across the corpus. A constant series is refused where the reference raises. |
| `regression=`, `autolag=`, `maxlag=` | statsmodels | [`DickeyFullerOptions`](reference/stats-timeseries/stationarity-tests/dickeyfulleroptions.md) | `TrendTerms` for `"n"`, `"c"`, `"ct"`, `"ctt"`; `LagSelection` for `"AIC"`, `"BIC"`, `"t-stat"`, `None`. |
| `store=`, `regresults=` | statsmodels | — (no counterpart) | The intermediate regressions are not returned. |
| `kpss(x, regression="c", nlags="auto")` | statsmodels | [`Stationarity.Kpss(series, options)`](reference/stats-timeseries/stationarity-tests/stationarity-kpss.md) | Identical statistic, p-value, lag window and critical values. **The `InterpolationWarning` is a property**: [`KpssResult.PValueBound`](reference/stats-timeseries/stationarity-tests/kpssresult.md) says the returned p-value is the table's end and which way the truth lies. The `ct` residuals come from a closed-form line rather than `OLS`, agreeing at the corpus's tolerance. |

## Lodestar.Stats.TimeSeries — seasonal decomposition

| Python | Library | C# | Differences |
| --- | --- | --- | --- |
| `seasonal_decompose(x, model="additive", period=p, two_sided=True, extrapolate_trend=0)` | statsmodels | [`SeasonalDecomposition.Decompose(series, period, options)`](reference/stats-timeseries/seasonality/seasonaldecomposition-decompose.md) | Identical trend, seasonal and residual, `NaN` positions included. **`period` is required**: the reference infers it from a pandas index. A period below two is refused. The extrapolation's back window leaves the last defined point out, as the reference's does. |
| `filt=` | statsmodels | — (no counterpart) | Only the default moving-average filter. |
| `extrapolate_trend="freq"` / `"period"` | statsmodels | `ExtrapolateTrend = period - 1` | The string spellings are the integer the reference turns them into. |
| `STL` | statsmodels | — (not written) | Loess-based, a different algorithm; `stlnet` ships it under MIT. |
```

- [ ] **Step 3: The guide, the migration rows, the samples, the changelog**

- `docs/guides/time-series-diagnostics.md`: *before modelling* (ADF and KPSS together, the four outcomes
  of their two verdicts), *what the season is* (decomposition, additive against multiplicative), *after
  modelling* (the residuals through `SerialCorrelation.LjungBox`), each with a compiled `csharp` fence
  reusing the series above; linked from `wiki-map.json`'s `pages`.
- `docs/migration/statsmodels.md`: the `adfuller, kpss, seasonal_decompose` row becomes **native** with
  its link; the verdict paragraph drops "still to come"; the pitfall about Numerics.NET keeps its sentence.
  `docs/migration/README.md`'s statsmodels row names the package.
- One sample per public class in `samples/Lodestar.Sample`, each printing through `Inv.F3` the numbers
  the reference examples assert, registered in `Program.cs` after `LjungBoxResultSample.Run();`.
- `CHANGELOG.md` under `## [Unreleased]`: a `Lodestar.Stats.TimeSeries` 0.1.0 section listing the two
  static classes, and under `Lodestar.Stats` a line saying `SerialCorrelation` and its four types moved
  to the new package before any `Lodestar.Stats` release carried them.
- `README.md`: the incumbent table gains a `Lodestar.Stats.TimeSeries` row (`Cortex.TimeSeries` for the
  free half, Numerics.NET for the commercial, from decision 0129).

- [ ] **Step 4: The record**

```bash
NEXT=$("$MAIN/.next-adr")
```

`docs/decisions/$NEXT-stats-timeseries-is-a-package-and-takes-the-serial-correlation-lot.md`, frontmatter
`supersedes: ["0114"]`, `applies: ["0076", "0095", "0096", "0105", "0111"]`. It records the measurement 0114
asked #671 to take, in 0114's own basis — lines added and removed per `git diff --stat` over
`src/Lodestar.Stats.TimeSeries`, public types and members counted as source-declared members less
accessors — and the three criteria: the dependency profile now distinct (`OrdinaryLeastSquares.Fit`, and
through it `Lodestar.Decomposition` and `Lodestar.Abstractions`, which no hypothesis-test caller should
restore); the audience and cadence readings; and why the whole namespace moves rather than splitting.
Then `docs/decisions/README.md`'s row and both counts, and:

```bash
python3 tools/regen_adr_index.py
```

- [ ] **Step 5: Commit**

```bash
git add -A && git commit -m "Document the stationarity lot and record the package it earned"
```

---

### Task 8: The benchmark

**Files:**

- Create: `bench/Lodestar.Stats.Benchmarks/StationarityBenchmarks.cs`
- Modify: `bench/bench-map.json`, `bench/README.md`, `docs/guides/performance.md`

- [ ] **Step 1: Read what `Cortex.TimeSeries` 1.1.0 computes before timing it**

```bash
"$MAIN/.dotnet-guarded" dotnet run tools/survey.cs -- Cortex.TimeSeries 1.1.0 '(Stationarity|Dickey|Kpss|SeasonalDecompose)'
```

Then run its ADF, KPSS and `SeasonalDecompose` once on the walk and the quarterly series above, in a
scratchpad app, against `statsmodels`' numbers. **Only a function that agrees is timed**; one that does not
is recorded in `bench/README.md` with the numbers it returned, the way section 28 recorded
`Cortex.TimeSeries`' autocorrelation.

- [ ] **Step 2: The class**

`StationarityBenchmarks` in `bench/Lodestar.Stats.Benchmarks/`, `[MemoryDiagnoser]`,
`[Params(200, 2000)] Length`, a seeded AR(1) at 0.5 built in `[GlobalSetup]`, with
`Lodestar_AugmentedDickeyFuller`, `Lodestar_Kpss` and `Lodestar_Decompose` (period 12) each as the
baseline of its pair and the agreeing `Cortex_*` counterparts beside them. `bench/bench-map.json`:

```json
    "StationarityBenchmarks": [
      "src/Lodestar.Stats.TimeSeries/**",
      "src/Lodestar.Stats.Regression/**"
    ],
```

and `SerialCorrelationBenchmarks`' entry moves from `src/Lodestar.Stats/TimeSeries/**` to
`src/Lodestar.Stats.TimeSeries/**`.

- [ ] **Step 3: Measure under the lock, default job**

```bash
"$MAIN/.dotnet-guarded" dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*StationarityBenchmarks*'
```

The table goes in `docs/guides/performance.md` with the machine named; `bench/README.md` gains section 35,
how to measure and what the pairs do and do not compare. The ADF row states its cost honestly: one
`OrdinaryLeastSquares.Fit` per candidate lag, VIFs included, which is the price of the edge.

- [ ] **Step 4: Commit**

```bash
git add -A && git commit -m "Measure the stationarity lot against Cortex.TimeSeries"
```

---

### Task 9: The full gate, one commit, the pull request

- [ ] **Step 1: Every gate, under the lock**

```bash
"$MAIN/.dotnet-guarded" acquire "full gate #671"
for s in tools/check_*.py; do case $s in *repeated_literals*|*adr_immutable*) a="--base origin/main";; *nuspec_dependencies*) continue;; *) a="";; esac; python3 "$s" $a || echo "FAIL $s"; done
python3 -m pytest -q tools/tests
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
dotnet format Lodestar.slnx --verify-no-changes
dotnet test Lodestar.slnx -c Release          # 36 assemblies
rm -rf artifacts && for p in <the eighteen src/ directories>; do dotnet pack "$p" -c Release -o ./artifacts; done
python3 tools/check_nuspec_dependencies.py artifacts --require-all
NUGET_PACKAGES=$(mktemp -d) dotnet build samples/Lodestar.Sample -c Release
python3 tools/extract_doc_snippets.py && NUGET_PACKAGES=$(mktemp -d) dotnet build samples/Lodestar.DocSnippets -c Release
"$MAIN/.dotnet-guarded" release
```

Expected: every guard `ok`; 36 assemblies, no failure; 18 packages; both samples built; every snippet run.

- [ ] **Step 2: One commit**

```bash
git reset --soft origin/main
git commit   # one message: what shipped, the placement, Closes nothing in the message body
```

- [ ] **Step 3: The pull request**

`gh pr create` with `Closes #671`, milestone *Next release*, labels `enhancement`, `api`,
`Lodestar.Stats`, `packaging`, assignee `CyrilB1531`; then poll CI and SonarCloud.
