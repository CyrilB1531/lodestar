---
status: accepted
supersedes: []
amends: ["0098"]
applies: []
---
# 0121 — The quantiles invert by Newton, and the large-`df` residual is the tail's

**Status:** accepted · **Date:** 2026-09-13 · **Amends:** [`0098`](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)

## Context

[`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md) and
[`Distributions.StudentQuantile`](../reference/stats/tails/distributions-studentquantile.md) found
the root of their upper tail by bisection, about sixty tail evaluations per call. The reason given
in the source was **one approximation to keep right instead of two that must agree**: a rational
approximation of the quantile would be a second function that has to match the tail.

[#705](https://github.com/CyrilB1531/lodestar/pull/707)'s measurements put a price on that. One call
cost 11.4 μs for the normal and 16.6 μs for Student, which was the **whole** gap between
`SerialCorrelation`'s autocorrelations and `Cortex.TimeSeries` at 200 points, and half of a 100-row
least-squares fit. [#709](https://github.com/CyrilB1531/lodestar/issues/709) is that cost.

[Decision 0098](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
published the normal quantile because a large-`df` Student quantile cannot stand in for it. It
measured the substitute's error at a best of **9.1e-9** at `df = 1e8` and attributed the loss
above that to "the bisection inside the Student quantile".

## Decision

**Both quantiles solve the same equation by safeguarded Newton from a seed.** Newton runs on
`log Sf` against `log x`; a step that leaves the bracket the evaluations have established falls
back to growth or bisection, so convergence never depends on the step. The seed is Wichura's
AS 241 for the normal (one tail evaluation), and for Student the closed forms at `df = 1` and `2`,
the Cornish-Fisher expansion while `z² <= df`, and the power-law tail beyond (1.84 evaluations on
average for `1 <= df <= 1e6`). The answer is still the root of the package's own tail, so the
reason bisection was chosen survives: **the seed decides how many steps, never which root.**

**0098 is amended on one sentence and not on its conclusion.** Once the inversion is replaced, the
same table reads:

| `df` | `StudentQuantile(0.975, df)` | relative error against `norm.ppf(0.975)` |
| --- | --- | --- |
| 1e6 | 1.9599663569612706 | 1.2e-6 |
| 1e7 | 1.9599642223756817 | 1.2e-7 |
| **1e8** | **1.959963960540754** | **1.2e-8** |
| 1e9 | 1.9599636999069967 | 1.5e-7 |
| 1e12 | 1.9598992657185477 | 3.3e-5 |

The shape is the same with a different root-finder, so the loss above `1e8` was never the
bisection's: it is `StudentSf` itself, whose incomplete beta and log-gamma lose digits at that many
degrees of freedom (the comment on `StudentQuantileTests`' `1e8` case already traces it to
`Gamma.LogGamma`). The floor moves from 9.1e-9 to 1.2e-8, both digits the tail does not resolve.
**The conclusion stands**: the substitute still cannot reach `1e-9`, and the normal quantile stays
published for the reason 0098 gave.

## Options refused

**AS 241 alone for the normal, with no Newton step.** About 55 ns against 250 ns, and as accurate
as the tail on every probe. Refused because it reintroduces the second approximation bisection was
chosen to avoid: the published quantile would no longer be the inverse of the tail the same
package reports, and a later change to either would move them apart silently. The polish is one
tail evaluation, and it also makes the call's cost follow `Normal.Sf`'s.

**Hill's algorithm (CACM 396) for Student.** Its one implementation this project could read is R's
GPL `qt.c`, which [decision 0003](0003-provenance-and-licensing.md) rules out as a source; the
expansion in Abramowitz and Stegun 26.7.5 is public and, as a seed, sufficient.

**Keep bisection.** Refused on the measurement: a quantile was the most expensive thing in every
caller that took one.

## Consequences

- `NormalQuantile(0.975)` goes from 11.4 μs to 249 ns and `StudentQuantile(0.975, 95)` from 16.6 μs
  to 366 ns; `docs/guides/performance.md` carries the run.
- Results move in the last digits only: across 24 `df` by 36 `p` the normal agrees with the
  bisection to 2.8e-13 and Student to 2.3e-10 over the practical range. One executed reference value
  moved in its sixteenth digit (`GlmOptions`' 95% lower bound).
- Where `StudentSf` itself fails — `t` past 1.34e154, where `t²` overflows, or `df` above about
  1e9 — both inversions return the same point of that failure; this decision does not repair the
  tail.
