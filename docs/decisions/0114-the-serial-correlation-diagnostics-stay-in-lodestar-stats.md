---
status: accepted
supersedes: []
amends: []
applies: ["0076", "0096", "0105", "0111"]
---
# 0114 — The serial-correlation diagnostics stay in `Lodestar.Stats`, and the second lot re-measures

**Status:** accepted · **Date:** 2026-09-12

## Context

[Decision 0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md) named
four subjects for the time index and found the forecast already shipped by
`Microsoft.ML.TimeSeries`. [#617](https://github.com/CyrilB1531/lodestar/issues/617) takes three of
the four — the autocorrelation function, the partial autocorrelation function and the Ljung-Box
test — at `statsmodels` 0.15.0 parity, in namespace `Lodestar.Stats.TimeSeries`.

**Which package that namespace ships from was deferred on purpose.** The spec
(`docs/superpowers/specs/2026-09-12_0617_time-series-diagnostics.md`, "Placement, deliberately
deferred") refused to answer it in advance and named the method instead:

> The same method [decision 0111](0111-the-generalized-linear-model-does-not-earn-its-own-package.md)
> used for #616: write it, measure the lines, the public types and the members, then test the result
> against [decision 0076](0076-a-core-package-carries-no-external-dependency.md)'s three criteria —
> a distinct dependency profile, a distinct audience, a distinct cadence — and record the verdict as
> an ADR before the branch ends.

That is this record. It inherits one more thing from 0111, which 0111 inherited from
[decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md) at a cost:
**a count is worthless unless the record says what was counted.** The basis below is 0111's —
source-declared members, accessors excluded, constructors counted only when explicitly written —
and it is stated before the numbers rather than after.

## The measurement

Taken on `feat/617-time-series-diagnostics` at merge base `e4bc1376`, which is `origin/main`.

```console
$ git diff --stat $(git merge-base origin/main HEAD)..HEAD -- src/Lodestar.Stats
 .../TimeSeries/AutocorrelationOptions.cs           |  44 ++++
 .../TimeSeries/AutocorrelationResult.cs            |  24 ++
 .../TimeSeries/Internal/Autocovariance.cs          |  38 ++++
 .../TimeSeries/Internal/LevinsonDurbin.cs          |  48 ++++
 src/Lodestar.Stats/TimeSeries/LjungBoxOptions.cs   |  35 +++
 src/Lodestar.Stats/TimeSeries/LjungBoxResult.cs    |  28 +++
 src/Lodestar.Stats/TimeSeries/SerialCorrelation.cs | 253 +++++++++++++++++++++
 7 files changed, 470 insertions(+)
```

**470 lines added, none removed, in seven new files.** The same command over all of `src/` prints
the same seven rows: nothing outside `src/Lodestar.Stats/TimeSeries/` moved, so there is no
extraction here of the kind 0111 measured, where 109 removed lines were the OLS giving up three
private methods to a file the GLM also called. This lot arrived beside its neighbours rather than
out of them.

**Five public types and sixteen public members.** The types are `SerialCorrelation` (a static
class), `AutocorrelationOptions` and `LjungBoxOptions` (sealed records), and
`AutocorrelationResult` and `LjungBoxResult` (sealed classes — records were refused because a
record's equality would compare their lists by reference,
[#668](https://github.com/CyrilB1531/lodestar/issues/668)). The sixteen members:

| type | public members | count |
| --- | --- | --- |
| `SerialCorrelation` | `Autocorrelation`, `PartialAutocorrelation`, `LjungBox` | 3 |
| `AutocorrelationOptions` | `Adjusted`, `BartlettConfidenceInterval`, `ConfidenceLevel` | 3 |
| `AutocorrelationResult` | `Values`, `ConfidenceLower`, `ConfidenceUpper` | 3 |
| `LjungBoxOptions` | `ModelDegreesOfFreedom`, `BoxPierce` | 2 |
| `LjungBoxResult` | `Statistics`, `PValues`, `BoxPierceStatistics`, `BoxPiercePValues`, `DegreesOfFreedom` | 5 |

The basis and the check that it is the basis, which is where 0111's first draft went wrong:
`grep -c 'public ' src/Lodestar.Stats/TimeSeries/*.cs` prints 4, 4, 3, 4 and 6 — **21** lines
carrying the word — and the two files under `TimeSeries/Internal/` print 0. Twenty-one lines less
the five type declarations is the sixteen above, so the number and the command that produced it
agree. Neither constructor counts: `AutocorrelationResult` and `LjungBoxResult` each declare one
explicitly, and both are `internal`, which is how a caller is kept from building a result that
promises a band it never computed. `Autocovariance.Of` and
`LevinsonDurbin.ReflectionCoefficients` are `internal static` on `internal static` classes and are
not members of the public surface at all.

**No neighbour's surface opened.** `src/Lodestar.Stats/Distributions.cs` carries no diff on this
branch, and the two members `SerialCorrelation` reaches for were already public at the merge base:
`ChiSquaredSf` at line 74 and `NormalQuantile` at line 94. They are the only two things the lot
touches outside its own folder — four call sites, two each — which
`grep -n 'Distributions\.' src/Lodestar.Stats/TimeSeries/*.cs` shows and nothing else contradicts.
No `.csproj` changed, and `src/Lodestar.Stats/Version.props` still reads `0.4.0`, the version
`Lodestar.Stats/v0.4.0` tagged. As in 0111, the decision is taken before the bump, which is what
makes it a decision rather than a retrofit of one already made.

Placed provisionally beside the measurement, and moving with whatever it decides: 412 lines of
tests in `tests/Lodestar.Stats.Tests/` (`SerialCorrelationOracleTests.cs` and
`SerialCorrelationEdgeTests.cs`) replaying a 72-case corpus in
`tests/oracles/stats_timeseries.json`, and five files in `samples/Lodestar.Sample/` — one per
public type, which is what the packaging gate counts.

## Against 0076's three criteria

**Dependency profile — not distinct.** `Lodestar.Stats` declared no external dependency and no
inter-package edge before this branch, and declares neither after it: its row in
`tools/check_nuspec_dependencies.py`'s `EXPECTED` is empty on `net10.0` and holds only the
`netstandard2.0` polyfills, which 0076 rules out as dependencies for this purpose. The lot's only
reach outside its own folder is to two members of the very package a split would leave.

The honest half of that, which cuts the other way and belongs here rather than in a footnote:
those two members are public *precisely so another package can reach them*.
[Decision 0097](0097-the-chi-squared-tail-joins-the-published-four.md) published `ChiSquaredSf`
because `Lodestar.Survival` is a separate package and could not reach `Gamma.RegularizedQ`;
[decision 0098](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
published `NormalQuantile` for the Kaplan-Meier bound, on the same rule
[decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) wrote. So a
split here
would cost no publication at all — it would take the edge `Lodestar.Survival` already takes, and
nothing would have to be opened for it.

That is what makes the criterion answerable rather than merely convenient. 0076's word is
*distinct*, and a package whose entire dependency list is one edge onto the package it left is not
a different profile; it is the same profile with an edge added. The criterion asks whether shipping
these types together forces something on a caller who does not want it — ONNX Runtime on a caller
who wanted `Levenshtein`, in 0076's own case — and the answer is nothing: `Lodestar.Stats` restores
no runtime, no native asset and no third-party assembly either way.

**Audience — not distinct, by 0096's own test.**
[Decision 0096](0096-ordinary-least-squares-earns-its-own-package.md) gave
`Lodestar.Stats.Regression` a package on this criterion, and stated it as a *wants none of it*:
"a caller who wants a regression table wants none of [the ten hypothesis-test families], and a
caller running a Kruskal-Wallis wants no QR." Held against this lot it does not read the same way.
`LjungBox` returns statistics, p-values and degrees of freedom — the shape of `TestResult` one
directory up — and a reader who has just run `ShapiroWilk` on a model's residuals to ask whether
they are normal is one call from running `LjungBox` on the same residuals to ask whether they are
independent. The two questions are asked in the same sitting, by the same person, about the same
array.

The part of the audience that is genuinely its own is the reader who plots a correlogram to choose
a model order, and who wants no hypothesis test at all. That reader exists. What they do not
justify is a package: they are served by 470 lines that ship inside one they already have, and
`dotnet add package Lodestar.Stats` costs them nothing they would not otherwise pay.

**Cadence — none today, and the second lot is the whole of the question.**
[#671](https://github.com/CyrilB1531/lodestar/issues/671) is scoped, open, and on the *Next
release* milestone: the augmented Dickey-Fuller test with its MacKinnon response surface, KPSS, and
seasonal decomposition. It is comparable to this lot in size and arguably larger, and it says
something about itself that matters more than its size — ADF "regresses the differenced series on
its own lags and selects the lag order by AIC — that is an OLS per candidate lag, so it consumes
`Lodestar.Stats.Regression` rather than the correlation code."

`Lodestar.Stats.Regression` already depends on `Lodestar.Stats`. So `Lodestar.Stats` cannot take
the reverse edge, and on that design the second lot cannot ship from this package at all. That is a
real constraint and this record does not soften it. But it is a fact about a lot that has not been
written, whose design is an issue's sketch rather than a measurement, and which has at least two
other resolutions — a small internal solve that avoids the edge, or its own package taking both
edges. #671 says as much itself, in its acceptance criteria: "The placement follows whatever
ADR #617 produced, **or supersedes it with its own measurement**."

What this lot has is no cadence of its own. It has shipped nothing, it is unreleased, and every
line of it sits in an unbumped 0.4.0 beside the tests it was proved against.

None of the three reads distinct. 0076 requires one to license a split, and this measurement
supports none of them.

## Decision

**The serial-correlation diagnostics ship inside `Lodestar.Stats`, in namespace
`Lodestar.Stats.TimeSeries`, beside the ten hypothesis-test families and the five numerical members
0095's rule has published.**
No seventeenth package: no `Version.props`, no `EXPECTED` entry in
`tools/check_nuspec_dependencies.py`, no `FLOORS` row, no `*.NetStandard.Tests` mirror with its
`ProjectReference` pins, no sixth pack loop, no release allow-list entries. The repository stays at
sixteen packages, as it did after 0111.

This settles what the spec deferred, the way the spec said it would be settled: by measurement,
against 0076, before the version bump.

## Options considered

**A `Lodestar.Stats.TimeSeries` package now, on #671's arrival rather than on this lot.** It is the
option this record spent the longest on, because it is the one that would be cheap today and
expensive later. Refused on three grounds. The measurement licenses none of 0076's criteria, and
470 lines, five types and sixteen members is a *smaller* lot than the 661 lines, four types and
twenty-four members that 0111 kept in place under the same test. The package's whole dependency
list would be one edge back onto the package it left, for two members. And the argument that
remains once those two are gone is that the time index is a coherent subject that deserves its own
name — which is cohesion, and 0069's rule 1, carried forward by 0076, refuses it by name: *never
for tidiness*. Deciding a package on a lot nobody has written yet is deciding #671's design from a
branch that has not measured it.

**Move the lot into `Lodestar.Stats.Regression`, where the OLS that ADF wants already lives.**
Refused by 0096's own sentence, which drew this line before either issue existed: "This package is
OLS and its table; anything with a link function or a time index falls on that issue's side of the
line." It would also make a caller who wants nothing but a correlogram restore
`Lodestar.Decomposition` for a Householder QR no line of this lot uses.

**Split the subject when #671 lands, leaving this lot where it is.** Named so that it is not
mistaken for the default. Its cost is two packages contributing types to one namespace —
`Lodestar.Stats.TimeSeries` is the namespace here and would be the `RootNamespace` of a package of
that name — so a reader installing the package called after the subject would get half of it.
Should #671's measurement say a package is earned, the whole subject moves, and this record is
what that one supersedes.

## Consequences

- The public surface `Lodestar.Stats` ships past 0.4.0 gains `SerialCorrelation`,
  `AutocorrelationOptions`, `AutocorrelationResult`, `LjungBoxOptions` and `LjungBoxResult`. The
  reference pages, the `docs/wiki-map.json` entry, the `docs/equivalence.md` rows, the guide and the
  `bench/README.md` section that #617 still owes are `Lodestar.Stats`' debt, not a second package's
  cost — exactly as 0111 left #616's.
- **#671 re-measures rather than inherits.** Three readings would have to change for a split to be
  earned, and #671 is where each is testable: a dependency this lot never needed — ADF's OLS is
  precisely that — a caller who wants the stationarity tests and no hypothesis test at all, or a
  release `Lodestar.Stats` is not on. #671's own acceptance criteria already require the
  measurement; this record is what it would supersede, and nothing here asks it not to.
- **The move, if it comes, is owed before the tag.** `Lodestar.Stats/v0.4.0` is the last tag and
  carries none of these five types. The tag after it is the point of no return, because a published
  type does not change packages without a deprecation cycle — 0111's last consequence, restated
  because it applies here with a named boundary rather than in the abstract. The repository is
  pre-1.0, and [#427](https://github.com/CyrilB1531/lodestar/issues/427)'s line about that is the
  reason the boundary is worth naming: pre-1.0 is when the layout is still free.
- The namespace is the same either way, which is what keeps that cost bounded. These types are
  declared in `namespace Lodestar.Stats.TimeSeries`, and a package of that name would set
  `RootNamespace` to exactly it under 0076's rule, so a caller's `using` survives a move untouched
  and only the `PackageReference` changes. That was chosen in the spec before this measurement
  existed, and it is why deferring the question cost nothing at the call site.
