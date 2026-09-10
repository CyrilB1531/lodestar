# 0098 — The normal quantile is the third member decision 0095's rule publishes

**Status:** accepted · **Date:** 2026-09-10

## Context

[Decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) published four
numerical members and wrote the rule they were published under: the layer publishes what one caller
needs, and *publishing later is always available*.
[Decision 0097](0097-the-chi-squared-tail-joins-the-published-four.md) applied it a second time, for
the chi-squared tail a log-rank test reports.

[#569](https://github.com/CyrilB1531/lodestar/issues/569)'s Kaplan-Meier curve asks a third time.
Its confidence bounds are the ones `lifelines` reports, built on the **log-log transform** of the
estimate, and their multiplier is `scipy.stats.norm.ppf` — a standard normal quantile.
`Normal.Quantile` exists in `Lodestar.Stats.Internal`, and 0095 listed it by name among the members
that stay internal.

## The substitute was tried first, and measured

Student converges on the normal as its degrees of freedom grow, and
[`Distributions.StudentQuantile`](../reference/stats/tails/distributions-studentquantile.md) is
already public. `StudentQuantile(0.975, df)` at a very large `df` is therefore the obvious way to
avoid publishing anything. It does not work, and the reason is not the one expected:

| `df` | `StudentQuantile(0.975, df)` | relative error against `norm.ppf(0.975)` |
| --- | --- | --- |
| 1e6 | 1.9599663569861794 | 1.2e-6 |
| 1e7 | 1.959964222613035 | 1.2e-7 |
| **1e8** | **1.9599639666317055** | **9.1e-9** |
| 1e9 | 1.9599636901568092 | 1.5e-7 |
| 1e12 | 1.9598992657185477 | 3.3e-5 |

Accuracy has an optimum rather than a limit. Below 1e8 the convergence to the normal is still
incomplete; above it the bisection inside the Student quantile loses more than the convergence
gains. The floor is about **9e-9**, and the log-log transform amplifies that into the seventh digit
of a survival bound — past the `1e-9` the corpora are compared at. The failure was found by the
frozen corpus, not predicted.

## Decision

**[`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md) becomes
public.** `Lodestar.Stats` goes 0.3.0 → 0.4.0. It answers to about `1e-15` across the range, seven
orders of magnitude better than the substitute.

Two details carry over from 0095's experience of publishing `StudentQuantile`:

- **It is the quantile, not the inverse survival function.** The internal helper solves
  `P(Z > z) = p` and carries the opposite sign. The published member negates — the distribution's
  symmetry, not a correction — exactly as 0095 found and fixed for Student.
- **The median returns positive zero.** Negating the helper's exact zero would publish `-0`.

Everything else in the layer stays internal, unchanged: `RegularizedIncomplete`, `LogGamma`,
`RegularizedP`, `RegularizedQ`, `Erfc`, `Normal.Sf`, the finite-sample Kolmogorov distribution,
`PartialPivotLu`, `JacobiSvd` and the `DenseBlock` helpers.

## Options refused

**Approximate it with a large-`df` Student quantile.** Refused on the measurement above: it cannot
reach the tolerance, and burying a 9e-9 error inside a confidence bound is the kind of avoidable
numerical divergence [decision 0081](0081-the-stats-numerical-layer-stays-internal.md) exists to
prevent.

**Re-derive a normal quantile inside `Lodestar.Survival`.** Refused for the reason 0097 refused the
same move for the chi-squared tail: a second implementation of a function this repository already
has, with nothing making the two agree.

## Consequences

- One member, one reference page, one row in the tails index, one use in the packaging sample, and
  six cases added to `tests/oracles/stats_distributions.json`, compared relatively.
- A test pins the substitute's failure rather than only the member's success: the Student quantile
  at 1e8 degrees of freedom is asserted to differ from this one at `1e-12` and agree at `1e-7`, so
  the measurement above cannot rot silently.
- **`Lodestar.Survival` waits on 0.4.0 reaching the feed**, as it waited on 0.3.0. Same reason,
  same shape: `src/` floors on published versions ([decision 0012](0012-per-package-versioning.md)),
  and CONTRIBUTING's *Working across two packages* refuses to merge two independently released
  packages as one.
