---
status: accepted
supersedes: []
amends: []
applies: ["0076", "0096", "0104"]
---
# 0111 — The generalized linear model does not earn its own package

**Status:** accepted · **Date:** 2026-09-12

## Context

[Decision 0104](0104-generalized-linear-models-are-written-natively.md) named what would be built
and left where deliberately open:

> Whether it earns a package at all is the question 0096 answered on audience, and it is answered
> the same way — with a measurement — when #616's spec is written, not before.

The spec (`docs/superpowers/specs/2026-09-11_0616_generalized-linear-models.md`, "Placement,
deliberately deferred") named the test: [decision 0076](0076-a-core-package-carries-no-external-dependency.md)
allows a split only for a distinct dependency profile, audience or cadence. It also named one input
that might have decided the question on its own, before any of the three: whether Poisson's
log-likelihood forced `Lodestar.Stats`' internal `Gamma.LogGamma` public, which the spec called
*"one argument for being its own thing, and one argument against."*

That input resolved during implementation, before this record was due. `Lodestar.Stats.Regression`
reaches `Lodestar.Stats` through a `PackageReference` on a published floor
(CONTRIBUTING.md's *Working across two packages*), so a member opened there could not ship on this
branch regardless of which package the GLM lived in — publication was refused on that ground
alone, not on placement. `LogLikelihood.Of`
(`src/Lodestar.Stats.Regression/Internal/LogLikelihood.cs`) computes `log(y!)` from the response
itself, with a Kahan-compensated cumulative table, and `src/Lodestar.Stats/` carries no diff on
this branch. So the measurement that remains is the plain one #616 asked for: how much this added,
and whether it meets 0076's test.

## The measurement

Taken on this branch, `git diff --stat main...HEAD -- src/Lodestar.Stats.Regression/`:

```text
 GeneralizedLinearModel.cs |  166 ++++++++
 GlmFamily.cs               |   17 ++
 GlmOptions.cs              |   25 ++
 GlmSummary.cs              |   58 ++
 Internal/Families.cs       |   48 ++
 Internal/Irls.cs           |  154 ++++++++
 Internal/LeastSquares.cs   |  116 ++++++
 Internal/LogLikelihood.cs  |   72 ++++
 OrdinaryLeastSquares.cs    |  114 +----------
 9 files changed, 661 insertions(+), 109 deletions(-)
```

661 lines added, 109 removed — and the 109 are `OrdinaryLeastSquares.cs` losing three `private`
methods (`Design`, `Solve`, `StandardErrors`) to the new `Internal/LeastSquares.cs`, which the
GLM's IRLS loop calls too. The extraction is itself evidence: OLS and the GLM share the same
design-matrix and covariance arithmetic closely enough that separating them into two packages
would have to either duplicate `LeastSquares.cs` or draw a `PackageReference` between two packages
for code neither one exposes publicly.

Four new public types, twenty-four new public members: `GeneralizedLinearModel` (one method,
`Fit`), `GlmFamily` (two values, `Binomial` and `Poisson`), `GlmOptions` (five init-only
properties), `GlmSummary` (sixteen init-only properties). Everything else the diff adds —
`Families`, `Irls`, `LeastSquares`, `LogLikelihood` — is `internal static`, reachable only inside
the assembly and its two `InternalsVisibleTo` test projects
(`Lodestar.Stats.Regression.Tests` and its `NetStandard` mirror). None of it is a public member by
`tools/survey.cs`'s own counting basis — declared, accessors and operators excluded, constructors
kept — because none of it is declared `public` on a `public` type.

No neighbour's surface opened. `Lodestar.Stats/Internal/Gamma.cs`'s `LogGamma` (line 37) is still
`internal`, unchanged by this branch. `src/Lodestar.Stats.Regression.csproj` still declares exactly
the two edges 0096 recorded — `Lodestar.Stats` for the Student and Fisher tails, and
`Lodestar.Decomposition` for the Householder QR — and no `PackageReference` beyond them.
`Version.props` still reads `0.1.0`: the decision is taken before the bump, which is what makes it
a decision rather than a retrofit of one already made.

## Against 0076's three criteria

**Dependency profile.** Identical, not merely similar. Before this branch the package carried zero
external dependencies and two internal edges; after it, the same zero and the same two. The GLM
took no dependency the OLS beside it did not already have, and the one input that could have
forced a new edge open — `LogGamma` — resolved to *no publication at all*, on a constraint that
would have applied wherever the GLM lived.

**Audience.** Indistinguishable from the audience already in the package. 0096 gave
`Lodestar.Stats.Regression` its own package on this exact criterion: *"a caller who wants a
regression table wants none of [the ten hypothesis-test families in `Lodestar.Stats`], and a
caller running a Kruskal-Wallis wants no QR."* A caller who wants a GLM's inference table wants
exactly what an OLS caller wants — coefficients, standard errors, a test statistic, p-values,
intervals — the same shape, fitted through a link function instead of an identity one.
`GlmSummary` and `OlsSummary` sit next to each other in the namespace, and `GlmOracleTests` and
`OlsOracleTests` sit next to each other in the same test project, both replaying a frozen
`statsmodels` corpus at the tolerance regime [decision 0081](0081-the-stats-numerical-layer-stays-internal.md)
set — `GlmOracleTests` reuses `StatsOracleAsserts` for it directly, where `OlsOracleTests`
predates that helper and asserts the same rule inline. Nothing here is the "wants none of it"
split 0096 measured; it reads closer to "wants more of the same."

**Release cadence.** No evidence of a distinct one. The GLM shipped inside the same unreleased
0.1.0, in the same commit range, sharing `LeastSquares.cs` with the OLS it sits beside — code that
would need to move, be duplicated, or cross a package boundary if the two ever separated. A
distinct cadence is something a package earns by needing to ship on its own schedule; nothing
about this work has asked to.

None of the three reads distinct. 0076 requires one to license a split, and this measurement
supports none of them.

## Decision

**The generalized linear model ships inside `Lodestar.Stats.Regression`, beside the ordinary least
squares it was built next to.** No new package, no `Version.props`, no `EXPECTED` entry, no
`FLOORS` row, no `*.NetStandard.Tests` mirror, no `wiki-map.json` entry — none of the fixed cost
[#566](https://github.com/CyrilB1531/lodestar/issues/566) measured for a seventeenth package on
[#616](https://github.com/CyrilB1531/lodestar/issues/616) is paid, because nothing here asked for
one. The repository stays at sixteen packages.

This settles 0104's open question the way 0104 said it would be settled: by measurement, against
0076, before the version bump.

## Consequences

- The public surface `Lodestar.Stats.Regression` ships past 0.1.0 now carries
  `GeneralizedLinearModel`, `GlmFamily`, `GlmOptions` and `GlmSummary` next to
  `OrdinaryLeastSquares`, `OlsOptions` and `OlsSummary` — one package, seven public types doing
  regression.
- Nothing here is a package move, so none of #616's remaining documentation debt (a
  `bench/README.md` section against `Accord.Statistics`, a `docs/wiki-map.json` entry, reference
  pages, a sample, equivalence rows, a guide) is a second package's cost. It is
  `Lodestar.Stats.Regression`'s, exactly as it was for OLS.
- Should a later family (negative binomial, Gamma, inverse Gaussian — the ones 0104 named out of
  scope) change any of these three readings — a dependency the OLS half never needed, a caller who
  wants the GLM table and nothing OLS-shaped, a release the OLS half is not on — this record is
  what a future split cites as no longer true, rather than reopening 0076 from nothing.
- If a later branch judges differently, the move is owed **before** the next version bump, not
  after: `GeneralizedLinearModel`, `GlmFamily`, `GlmOptions` and `GlmSummary` are about to ship
  public in 0.1.0, and a published type cannot move packages without a deprecation cycle. That
  branch is its own issue and its own branch; this one ships inside `Lodestar.Stats.Regression`
  either way.
