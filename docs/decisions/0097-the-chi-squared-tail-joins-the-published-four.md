---
status: accepted
supersedes: []
amends: []
applies: ["0095"]
---
# 0097 — The chi-squared tail joins the published four, by the rule 0095 already set

**Status:** accepted · **Date:** 2026-09-10

## Context

[Decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) published
four numerical members and wrote the rule it published them under:

> **Four members become public, and nothing else.** […] Nothing has asked for them, and 0081's
> asymmetry holds in both directions: publishing later is always available, unpublishing never is.

[#569](https://github.com/CyrilB1531/lodestar/issues/569) asks. A log-rank test compares two
survival curves and reports a chi-squared p-value on one degree of freedom. `Lodestar.Survival`
is a separate package and cannot reach `Gamma.RegularizedQ`, which 0095 listed by name among the
members that stay internal.

The issue put three options, and required that they not be answered twice, differently, from the
way [#566](https://github.com/CyrilB1531/lodestar/issues/566) answered the same question for the
Student and Fisher tails.

## Decision

**[`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md) becomes public.** `Lodestar.Stats` goes 0.2.0 → 0.3.0, and
`Lodestar.Survival` takes an edge onto it, exactly as `Lodestar.Stats.Regression` takes one for
the three tails before it.

This record does not amend 0095 so much as apply it. 0095's sentence is *"the numerical layer
publishes what one caller needs, and no more"*; a second caller has now named a fifth member, and
the answer is the one 0095 wrote down in advance. `RegularizedIncomplete`, `LogGamma`,
`RegularizedP`, `RegularizedQ`, `Erfc`, `Normal.Sf`, `Normal.Quantile`, the finite-sample
Kolmogorov distribution, `PartialPivotLu`, `JacobiSvd` and the `DenseBlock` helpers **stay
internal**, unchanged.

## Options refused

**Re-derive a chi-squared tail inside `Lodestar.Survival`.** Refused on the ground 0081 already
stated against exactly this, in the note that `erfc` is `Q(1/2, x²)`: a second far-tail
approximation in one repository has to agree with the first, and nothing makes it. The published
member and the tail [`ChiSquare.GoodnessOfFit`](../reference/stats/tests/chisquare-goodnessoffit.md)
and [`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) already use are now the
same function, and a test asserts that a statistic routed either way gives the same p-value.

**Publish the whole numerical layer.** Refused, as 0081 and 0095 both refused it.

**Move the layer into `Lodestar.Abstractions`.** Refused, as 0095 refused it and generalised the
refusal: `Abstractions` carries the types packages exchange, and a function is not a type two
packages exchange.

## Consequences

- One member, one reference page, one row in the tails index, one use in the packaging sample, and
  six cases added to `tests/oracles/stats_distributions.json` — reaching `7.7e-26`, compared
  **relatively**, because an absolute `1e-9` there accepts an implementation returning zero.
- **`x` is not validated.** The three members before it refuse a non-positive degrees of freedom
  and nothing else; this one also leaves `x` unguarded, and returns one below the support rather
  than surfacing the internal helper's own `ParamName` from a public method.
- **`Lodestar.Survival` cannot land in the same pull request.** `src/` reaches a sibling through a
  `PackageReference` on a published floor ([decision 0012](0012-per-package-versioning.md)), so
  0.3.0 must be on the feed before a package may floor on it — CONTRIBUTING's *Working across two
  packages* states it, and #566 followed it: its four members landed in their own commit and were
  tagged before `Lodestar.Stats.Regression` raised the floor. This lot is split the same way, for
  the same reason.
