---
status: accepted
supersedes: []
amends: []
applies: ["0015", "0019"]
---
# 0107 — xunit v3 arrives whole, and takes the coverage format with it

**Status:** accepted · **Date:** 2026-09-11

## Context

[#623](https://github.com/CyrilB1531/lodestar/issues/623) asked for xunit v3 across the 32 test
projects — sixteen suites and their sixteen `*.NetStandard.Tests` mirrors — and asked for it in
stages: port one suite, measure, then port the rest. It also left an exit open, in its own words:

> If the measurement on the first suite says the theory churn is large, **"stay on v2 for now" is
> an acceptable outcome** and should be recorded as a decision rather than silently abandoned.

The measurement said the opposite, and the staging turned out to be impossible. Both are recorded
here because both change what a later reader would otherwise assume.

## What the first suite measured

`Lodestar.Cluster.Tests` and its mirror, ported 2026-09-11.

**The theory churn the issue feared is zero.** 130 files use `[Theory]` with `MemberData`,
`ClassData` or `InlineData`, and not one needed touching. The port is four lines of `.csproj`:
swap `xunit` for `xunit.v3`, add `<OutputType>Exe</OutputType>`, drop `Microsoft.NET.Test.Sdk`
and `xunit.runner.visualstudio`. 42 tests before, 42 after, zero warnings — and warnings are
errors here.

**What it costs instead is one analyzer rule.** `xUnit1051` arrives with v3 and fires at 54 sites
across 14 files: a call accepting a `CancellationToken` should be given
`TestContext.Current.CancellationToken`. Eight of those are `PersistenceOverloadTests`, whose
whole subject is that the overload *without* a token exists and behaves like the one with it, so
a blanket fix deletes what they assert. The rule is suppressed for `tests/` with that reason and
the other 46 are [#647](https://github.com/CyrilB1531/lodestar/issues/647).

## Decision

**All 32 projects move together.** Not a preference — a constraint, measured twice:

- The .NET 10 SDK opts into Microsoft.Testing.Platform through **`global.json`**, and that is
  repository-wide. A v2 project under it is refused outright: *"global.json définit le test runner
  en tant que Microsoft.Testing.Platform. Tous les projets doivent utiliser ce test runner."*
- There is no bridge. `xunit.runner.visualstudio` 4.0.0 ships `build/net472` and `build/net8.0`
  holding a VSTest adapter and nothing else — no target sets `IsTestingPlatformApplication`, so
  `UseMicrosoftTestingPlatformRunner` has nothing to switch on. A v2 project cannot be coaxed
  onto MTP, whether the property is set in the `.csproj` or passed as `-p:`.

And **VSTest is not an option to stay on**: `Microsoft.Testing.Platform.MSBuild` errors on the
`VSTest` target under a .NET 10 SDK by design, so the moment `xunit.v3` is referenced anywhere,
`dotnet test` fails for that project until `global.json` flips.

So the first suite could be *measured* in isolation, but it could never be *merged* in isolation.
The staged plan in #623 was written before that was known, and this record replaces it.

**Coverage changes format, from OpenCover to Visual Studio coverage XML.** `coverlet.collector`
is a VSTest data collector and collects nothing under MTP; its replacement,
`Microsoft.Testing.Extensions.CodeCoverage` 18.11.2, emits `xml`, `coverage` and `cobertura` —
**no OpenCover at all**, which is what `sonarcloud.yml` read. `xml` is the
`<results><modules><module …>` shape SonarQube Cloud's .NET path reads through
`sonar.cs.vscoveragexml.reportsPaths`, so that is the one taken. `cobertura` is rejected because
the .NET path does not read it, and `coverage` because it is binary.

## The trap this decision walked into twice, and what it costs to avoid

`--coverage-output` and `--report-trx-filename` take a **path, not a pattern**. Under VSTest one
process wrote one report; under MTP there are 32, and a pinned name makes all 32 write to the
same file. The last writer wins, silently, and the run stays green:

| pinned | reported | actual |
| --- | --- | --- |
| `--coverage-output coverage.xml` | **1** module | 16 |
| `--report-trx-filename test-results.trx` | **777** tests | 6904 |

Both numbers came from green runs. So neither name is pinned: the generated names are kept, one
file per assembly under `TestResults/`, and both workflows glob. This is the same failure the
oracle gate and the `--filter` habit already exist to catch, arriving through a third door.

## Consequences

- `dotnet test Lodestar.slnx -c Release` reports **6904 tests across 32 assemblies**, unchanged,
  in 11 seconds.
- **A `--filter` matching nothing now exits 8**, where VSTest exited zero and reported success.
  `CLAUDE.md`'s trap is rewritten: the habit of reading the count survives, because a *missing
  suite* still has no exit code, and 32 is the number that says none went missing.
- `tests/Directory.Build.props`' `CA2007` note is corrected. The rule skipped these projects
  because `Microsoft.NET.Test.Sdk` made them `OutputType=Exe`; v3 requires that property
  directly, so the exemption survives its cause.
- `ci.yml` and `sonarcloud.yml` pass their options after `--`; `release.yml`,
  `release-nuget-org.yml` and `bench-nightly.yml` call plain `dotnet test` and need no change.
- **The SonarCloud coverage number cannot be verified off a runner.** `sonarcloud.yml`'s own
  comment says a format or path mismatch "fails silently as 0% coverage", and this decision
  changes both the format and the path. The pull request's coverage must be compared against the
  previous run's, which is #623's stated acceptance criterion and is not something a local
  command can discharge.
- `coverlet.collector`, `Microsoft.NET.Test.Sdk`, `xunit` and `xunit.runner.visualstudio` leave
  `tests/Directory.Packages.props` entirely. Four pins become three.
- **Coverage stops counting test code, and the headline number moves because of it.** Measured on
  the first run: `lines_to_cover` falls from 14 533 to 12 539 and `coverage` rises from 92.6% to
  95.4%. Nothing was lost — all 265 files under `src/` that hold an executable body appear in the
  reports, and the 19 that do not are enums, interfaces, positional records and `GlobalUsings`,
  which have nothing to cover. The 1 994 lines that left were **test sources**, which coverlet
  reported and `sonar.coverage.exclusions` never excluded; the platform's collector attributes
  loaded modules to `src/` alone. `tests/**` joins that exclusion list anyway, so the scope is a
  decision rather than a property of whichever collector happens to be installed.
- **Coverage on new code reads as 0% for a pull request that changes no C#**, which this one does
  not. The measure is *absent*, not zero — `new_lines_to_cover` has no value — and SonarQube
  Cloud renders an absent measure as 0%. The quality gate carries no coverage condition, so
  nothing fails on it; the line-level import is proven by the 95.4% computed from the same
  reports.
