---
status: accepted
supersedes: []
amends: []
applies: ["0012", "0071", "0132", "0138"]
---
# 0139 — The sparse overloads take the second edge, under 0138's precedent

**Status:** accepted · **Date:** 2026-09-16 · **Applies:** [`0012`](0012-per-package-versioning.md), [`0071`](0071-csrmatrix-moves-to-an-abstractions-package.md), [`0132`](0132-preprocessing-writes-splitters-scalers-and-encoders-and-not-smote.md), [`0138`](0138-lodestar-preprocessing-takes-an-edge-on-lodestar-stats-for-the-normal-quantile.md)

## Context

[#765](https://github.com/CyrilB1531/lodestar/issues/765) gives the scalers the sparse overloads
`docs/equivalence.md` has scoped out since `Lodestar.Preprocessing` 0.1.0. The sparse primitive is
`CsrMatrix`, which [0071](0071-csrmatrix-moves-to-an-abstractions-package.md) moved into
`Lodestar.Abstractions` so that every package could share one.

So this package needs a second inter-package edge, in the same lot as the sparse overloads
themselves.

## Decision

**`Lodestar.Preprocessing` → `Lodestar.Abstractions`**, on the published floor **0.1.1**, declared
the way [0012](0012-per-package-versioning.md) requires: a `PackageReference` with a
`ProjectReference` behind `LodestarUseProjectRefs` for the developer loop, and a `ProjectReference`
of its own in the netstandard mirror, since `SetTargetFramework` does not cross a
`PackageReference`.

**This record applies [0138](0138-lodestar-preprocessing-takes-an-edge-on-lodestar-stats-for-the-normal-quantile.md)
rather than re-arguing it.** That decision settled the principle a month of lots will keep meeting —
*a member another `Lodestar` package publishes is depended on, not copied* — weighed the two
alternatives, and named the five files an edge touches. Nothing here is new except which package and
which member: `CsrMatrix` instead of [`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md), and a type rather than a
function.

The one thing worth writing down separately is that **this is the second edge in the same package
inside one lot**, and it did not need a fresh argument. That is what a precedent is for.

## Consequences

- The sixteenth edge. `tools/check_nuspec_dependencies.py`'s `EXPECTED` is the authority and
  `CLAUDE.md`'s table and count follow it.
- `Lodestar.Abstractions` carries no dependency of its own, so a consumer of the scalers restores two
  Lodestar packages and nothing else.
- **Only three scalers take a `CsrMatrix`**, which is the reference's own line rather than this
  package's: `StandardScaler` with centring off, `MaxAbsScaler`, and `RobustScaler` with centring
  off. Centring is refused because subtracting a mean or a median makes every absent zero a stored
  value, and `MinMaxScaler` has **no sparse overload at all** — the reference raises for it, and a
  missing overload is the same refusal at compile time.
- A third edge from this package would be routine under this record and 0138; a first edge from
  another package is not, and earns its own.
