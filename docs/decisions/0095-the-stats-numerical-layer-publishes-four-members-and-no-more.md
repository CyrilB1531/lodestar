---
status: accepted
supersedes: []
amends: []
applies: ["0081"]
---
# 0095 — The numerical layer publishes what one caller needs, and no more

**Status:** accepted · **Date:** 2026-09-10

## Context

[Decision 0081](0081-the-stats-numerical-layer-stays-internal.md) kept `Lodestar.Stats`'
numerical layer internal and refused two ways of exposing it — a public
`Lodestar.Stats.Special` namespace, and a move into `Lodestar.Abstractions`. It also wrote the
condition for changing its mind:

> Publishing later stays possible the day a second package needs the same functions;
> unpublishing a public API once shipped does not.

[#566](https://github.com/CyrilB1531/lodestar/issues/566) is that day. `Lodestar.Stats.Regression`
ships an OLS table, and a table is a Student tail per coefficient, a Student quantile per
confidence interval, and an *F* tail for the overall test. A separate package cannot reach an
`internal`.

The same day arrives for `Lodestar.Decomposition`. A numerically sound least squares wants a QR of
the design matrix, and this repository already writes one — by hand, because
[decision 0059](0059-phase-0-verifications-two-confirmed-voids-do-not-survive-nuget.md) refused
Math.NET on freshness. The alternative to publishing it is a second copy of a delicate kernel.

## Decision

**Four members become public, and nothing else.**

| package | published | version |
| --- | --- | --- |
| `Lodestar.Stats` | [`Distributions.StudentSf`](../reference/stats/tails/distributions-studentsf.md), [`Distributions.StudentQuantile`](../reference/stats/tails/distributions-studentquantile.md), [`Distributions.FisherSf`](../reference/stats/tails/distributions-fishersf.md) | 0.1.0 → 0.2.0 |
| `Lodestar.Decomposition` | [`QrDecomposition.Householder`](../reference/decomposition/factorization/qrdecomposition-householder.md), with `Q`, `R`, `RowCount`, `ColumnCount` | 0.1.1 → 0.2.0 |

`RegularizedIncomplete`, `LogGamma`, `RegularizedP`, `RegularizedQ`, `Erfc`, `Normal.Sf`,
`Normal.Quantile`, the finite-sample Kolmogorov distribution, `PartialPivotLu`, `JacobiSvd` and the
`DenseBlock` helpers **stay internal**. Nothing has asked for them, and 0081's asymmetry holds in
both directions: publishing later is always available, unpublishing never is.

**They do not go to `Lodestar.Abstractions`.** 0081 refused that for this layer specifically;
this record generalises the refusal — `Abstractions` carries the types packages exchange, and a
function is not a type two packages exchange. A shared implementation lives in the package that
owns it, and its neighbour takes an edge.

## What publishing cost, measured rather than predicted

0081 listed the price: a reference page each, a `wiki-map` entry, a use in the packaging sample,
and *"its own oracle corpus checked at the tolerance a general-purpose caller would need, not the
one the ten tests above it happen to need."* Paying it found two things.

**The QR's corpus already existed.** `tests/oracles/decomposition_qr.json` is replayed by the
internal kernel's own tests, so the public wrapper owes shape and refusals, not numbers.

**The Student quantile was not a quantile.** `Beta.StudentQuantile` solves `P(T > x) = p` — an
inverse *survival* function — and returns the opposite sign to `scipy.stats.t.ppf`. Internally
nothing noticed, because its one caller wanted exactly that. Published under its own name it would
have handed a caller `-2.1788` where every printed table shows `+2.1788`, and a confidence interval
built on it would have been reflected through its own estimate. [`Distributions.StudentQuantile`](../reference/stats/tails/distributions-studentquantile.md)
publishes the conventional meaning and negates, which is the distribution's symmetry about zero
rather than a correction; the internal helper is unchanged.

That is the concrete form of 0081's warning that each published member becomes *"a parity promise
in its own right"*. The new corpus, `tests/oracles/stats_distributions.json`, reaches `3.1e-24` and
is compared **relatively** — an absolute `1e-9` there accepts an implementation returning zero,
which is what the internal tests' closed forms could never have caught.

## Options refused

**Publish the whole layer.** Refused, as 0081 refused it: every member would become a promise
nobody asked for, at a tolerance nobody stated.

**Keep the layer internal and duplicate the four functions in the regression package.** Refused:
two incomplete betas in one repository disagree eventually, and the disagreement surfaces as two
p-values for one statistic.

**Publish `StudentQuantile` with the internal helper's sign**, documenting the convention.
Refused: a member named for a quantile that returns the inverse survival function is a trap a
document cannot undo, and every reader who does not read the remark gets an interval the wrong way
round.

## What enforces it

The reference gate covers `Lodestar.Stats` through two directories now — `tests` and
`distributions` — so a fifth published member without a page fails CI. `check_sample_coverage.py`
requires a sample per public class in both packages, which are both on its enforced list.
