---
status: accepted
supersedes: []
amends: []
applies: ["0012", "0076", "0095", "0098", "0132"]
---
# 0138 — `Lodestar.Preprocessing` takes an edge on `Lodestar.Stats` for the normal quantile

**Status:** accepted · **Date:** 2026-09-16 · **Applies:** [`0012`](0012-per-package-versioning.md), [`0076`](0076-a-core-package-carries-no-external-dependency.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0098`](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md), [`0132`](0132-preprocessing-writes-splitters-scalers-and-encoders-and-not-smote.md)

## Context

[0132](0132-preprocessing-writes-splitters-scalers-and-encoders-and-not-smote.md) wrote the remaining scalers, and
[#763](https://github.com/CyrilB1531/lodestar/issues/763) put `RobustScaler` in scope with the options the reference
carries. One of them does not fit in a package that depends on nothing.

`RobustScaler(unit_variance=True)` divides the interpercentile range by

`Φ⁻¹(q_max/100) − Φ⁻¹(q_min/100)`

— 1.3489795 at the quartiles — so that a normally distributed column comes out with a standard deviation of 1 rather
than an interquartile range of 1. `Φ⁻¹` is the normal quantile, and this repository already publishes it:
`Lodestar.Stats`' [`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md), which [0098](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
published under [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s rule.

`Lodestar.Preprocessing` was one of three packages with **no inter-package edge at all**. Adding one is not a detail:
it changes what every consumer of a scaler restores, and `tools/check_nuspec_dependencies.py` asserts the graph rather
than reviewing it.

## Decision

**`Lodestar.Preprocessing` takes an edge on `Lodestar.Stats`**, and `unit_variance` is written against
[`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md).

The edge is declared the way [0012](0012-per-package-versioning.md) requires and
`Lodestar.Stats.TimeSeries` already does: a `PackageReference` on the published floor, with a
`ProjectReference` behind `LodestarUseProjectRefs` for the developer loop. The floor is **`Lodestar.Stats` 0.4.0**,
which is published and has carried the member since 0.2.0 — the depended-on package ships before the consumer's next
release, never after.

This is the fourteenth edge to become the fifteenth; `tools/check_nuspec_dependencies.py`'s `EXPECTED` is the
authority and `CLAUDE.md`'s table follows it.

## Alternatives rejected

- **Write a second normal quantile inside `Lodestar.Preprocessing`.** No edge, and a second implementation of a
  member this repository already publishes and tests against `scipy`. Two copies drift, and the reason to keep them
  apart would have to be written down here — there is none. [0076](0076-a-core-package-carries-no-external-dependency.md)
  bars an **external** dependency from a core package; it says nothing against depending on a sibling, which
  `Lodestar.Fuzzy`, `Lodestar.Survival`, `Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries` all do.
- **Ship #763 without `unit_variance`,** with an equivalence row recording the gap and its own issue. Honest, and it
  leaves a published scaler one option short of its reference for as long as nobody takes the edge — which was the
  spec's recommendation before this decision, and is what it replaces.
- **Take the adjustment as a caller-supplied `double`.** It moves the quantile to the caller rather than removing the
  need for it, and no reference spells the option that way.

## Consequences

- Anyone restoring `Lodestar.Preprocessing` now restores `Lodestar.Stats`, which is itself dependency-free — one
  package, no external dependency, and the same floor `Lodestar.Survival` and `Lodestar.Stats.Regression` already
  pull.
- `Lodestar.Preprocessing`'s description no longer claims "no dependencies".
- The precedent this sets is the one to apply next time: **a member another `Lodestar` package publishes is
  depended on, not copied.** Where that would create a cycle, the member moves to `Lodestar.Abstractions` instead, as
  `CsrMatrix` did under [0071](0071-csrmatrix-moves-to-an-abstractions-package.md).
- `unit_variance` refuses a percentile of 0 or 100, which have no finite quantile; the reference divides by an
  infinity and reports a scale of zero. `docs/equivalence.md` carries that divergence.
