---
status: accepted
supersedes: ["0114"]
amends: []
applies: ["0076", "0095", "0096", "0105", "0111"]
---
# 0133 — `Lodestar.Stats.TimeSeries` is a package, and the serial-correlation lot moves into it

**Status:** accepted · **Date:** 2026-09-15 · **Supersedes:** [0114](0114-the-serial-correlation-diagnostics-stay-in-lodestar-stats.md)

## Context

[Decision 0114](0114-the-serial-correlation-diagnostics-stay-in-lodestar-stats.md) measured
[#617](https://github.com/CyrilB1531/lodestar/issues/617)'s lot — the autocorrelation functions
and the Ljung-Box test — against [decision 0076](0076-a-core-package-carries-no-external-dependency.md)'s
three criteria, found none distinct, and kept it in `Lodestar.Stats`. It also wrote down what would
change that, and where it would be tested:

> **#671 re-measures rather than inherits.** Three readings would have to change for a split to be
> earned, and #671 is where each is testable: a dependency this lot never needed — ADF's OLS is
> precisely that — a caller who wants the stationarity tests and no hypothesis test at all, or a
> release `Lodestar.Stats` is not on.

[#671](https://github.com/CyrilB1531/lodestar/issues/671) is that lot: the augmented Dickey-Fuller
test, KPSS and seasonal decomposition, at `statsmodels` 0.15.0 parity. Its spec
(`docs/superpowers/specs/2026-09-15_0671_stationarity-and-seasonal-decomposition.md`) takes the
placement question first, because the answer decides where every other line goes.

## The measurement

0114's basis, stated before the numbers: lines per `git diff --stat` against `origin/main`;
source-declared public members, accessors excluded, constructors counted only when explicitly
written and public; enum fields counted apart.

**The new lot: 1,040 lines of C# in seventeen files, thirteen public types and twenty-six public
members**, plus sixteen enum fields.

| type | kind | public members |
| --- | --- | ---: |
| `Stationarity` | static class | 2 — `AugmentedDickeyFuller`, `Kpss` |
| `SeasonalDecomposition` | static class | 1 — `Decompose` |
| `DickeyFullerOptions`, `KpssOptions`, `SeasonalDecompositionOptions` | sealed records | 3 each |
| `DickeyFullerResult` | sealed class | 6 |
| `KpssResult` | sealed class | 5 |
| `SeasonalComponents` | sealed class | 3 |
| `TrendTerms`, `LagSelection`, `KpssLagRule`, `SeasonalModel`, `PValueBound` | enums | 16 fields |

`grep -c 'public ' src/Lodestar.Stats.TimeSeries/*.cs` over the thirteen files sums to 39 lines;
less the thirteen type declarations that is the twenty-six, and the arithmetic and the command agree.

**With #617's five types the subject is eighteen types and forty-two members in 1,510 lines** — a
lot three times 0114's, which is the case the #617 spec's "What would make this spec wrong" named:
*"If the second lot turns out to be three times this one, the measurement taken here answers a
question about a third of the eventual surface."*

The dependency the lot needs, measured rather than assumed. A throwaway replay of 56 `statsmodels`
cases through the published `Lodestar.Stats.Regression` 0.1.0 agreed to `5.5e-12` worst, and the
frozen corpora now agree on 199 stationarity and 32 decomposition cases.

## Against 0076's three criteria

**Dependency profile — distinct.** `adfuller` fits an ordinary least squares per candidate lag and
reads each fit's t statistics and residual sum of squares. `Lodestar.Stats.Regression`'s Householder
least squares is that computation — as
[`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md) with the
whole table, and as
[`OrdinaryLeastSquares.Estimate`](../reference/stats-regression/ols/ordinaryleastsquares-estimate.md)
without it, which is what a lag search reads. It restores
`Lodestar.Decomposition` and, through it, `Lodestar.Abstractions` behind it. `Lodestar.Stats`
depends on nothing, and `Lodestar.Stats.Regression` already depends on it, so the edge cannot run
from `Lodestar.Stats` at all. Put in 0076's own terms — does shipping these types together force
something on a caller who does not want it? — a Kruskal-Wallis caller would restore a QR, a
Householder factorisation and a sparse-matrix package for a unit-root test they never call.

The two alternatives that keep the lot in `Lodestar.Stats` were weighed in the spec and refused
there. **A private least-squares solve** would be a second OLS in one repository, which
[decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) refused for the
tails on the ground that two copies of one computation disagree eventually. **An edge from
`Lodestar.Stats` to `Lodestar.Decomposition`** is the imposition above, one step removed.

**Audience — still not distinct, and this record does not claim it is.** 0114's reading holds: a
reader checking residuals for normality is one call from checking them for independence. Neither
does the audience argue *against* the split: the tests move, the reader's `using` does not.

**Cadence — none yet.** Nothing in the lot has been released, and `Lodestar.Stats/v0.4.0`, the last
tag, carries none of #617's types.

One criterion is enough, and the dependency profile is the one 0076 itself was written about.

## Decision

**`Lodestar.Stats.TimeSeries` ships as its own core package, with two edges — `Lodestar.Stats` 0.4.0
and `Lodestar.Stats.Regression` 0.2.0 — and #617's five types move into it.** The eighteenth
package.

**The whole namespace moves, not the half that needs the edge.** 0114's third option, splitting the
subject across two packages that share `Lodestar.Stats.TimeSeries`, stays refused for 0114's reason: a
reader installing the package named after the subject would get half of it. The namespace was chosen
in the #617 spec for exactly this contingency, so no type changes name and no caller's `using`
changes — only the `PackageReference`.

**The move is owed now, and costs nothing now.** No `Lodestar.Stats` release carries these types. The
next `Lodestar.Stats` tag would have been the point after which moving them needs a deprecation cycle;
this record lands before it.

## Options considered

- **Keep everything in `Lodestar.Stats` with a private least-squares solve.** Refused above on 0095's
  ground. The regression is not a line of arithmetic: [`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md) solves through a
  Householder QR precisely because the normal equations square the condition number, and a private
  copy would either repeat that work or quietly skip it.
- **Put the stationarity tests in `Lodestar.Stats.Regression`.** Refused by 0096's sentence, which 0114
  already quoted: *"anything with a link function or a time index falls on that issue's side of the
  line."* It would also make a correlogram cost a QR.
- **A package for the new lot only, leaving `SerialCorrelation` in `Lodestar.Stats`.** Refused with
  0114's third option, above.

## Consequences

- `Lodestar.Stats` loses `SerialCorrelation`, `AutocorrelationOptions`, `AutocorrelationResult`,
  `LjungBoxOptions` and `LjungBoxResult` before any release carried them; its `CHANGELOG.md` entry
  moves with them. `docs/reference/stats/timeseries/serialcorrelation.md` stays as a pointer, because
  [decision 0122](0122-erfc-is-an-interpolant-sampled-from-the-incomplete-gamma.md) links to it and a
  decision record is not edited.
- The fixed cost #566 measured is paid once: `Version.props`, the `EXPECTED` edges and a `FLOORS`
  dependent, the `*.NetStandard.Tests` mirror
  pinning four assemblies, every pack loop and release allow-list, `wiki-map.json`, the samples and
  the reference pages. `dotnet test Lodestar.slnx` reports thirty-six assemblies.
- **A `Lodestar.Stats.TimeSeries` release follows `Lodestar.Stats.Regression` 0.2.0's**, the floor its
  packed nuspec declares; `Lodestar.Stats` 0.4.0 is already on nuget.org.
- If a later lot needs a newer `OrdinaryLeastSquares` — an HAC covariance for Newey-West, say — the
  floor rises and `Lodestar.Stats.Regression` publishes first, the order `CONTRIBUTING.md` already
  gives for two packages edited together.
