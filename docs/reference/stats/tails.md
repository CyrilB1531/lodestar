# Distribution tails — `Lodestar.Stats`

One type, [`Distributions`](tails/distributions.md): the density, both tails and both inverses of
the standard normal, Student's *t*, *F* and chi-squared laws, for a caller holding its own
statistic, without going through a hypothesis test to reach them.

## Why five members first, and twenty now

This package computes every tail it needs from its own log-gamma, incomplete beta and incomplete
gamma, and [`decisions/0003`](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
kept that layer **internal** — deliberately, and with the condition for changing its mind written
down: *"publishing later stays possible the day a second package needs the same functions."*

`Lodestar.Stats.Regression` is that day. What it needs is a Student tail for each coefficient's
p-value, a Student quantile for each confidence interval, and an *F* tail for the overall test.
Those three are published. The log-gamma, the incomplete beta and gamma, the normal tail and the
finite-sample Kolmogorov distribution underneath them stay internal, because nothing has asked for
them and an unpublished API can still be published later — the reverse is not true.

## The other fifteen

A statistics package at 1.0 that computes these laws and hands out five of their functions leaves a
caller writing their own test, interval or power calculation to find `t.cdf` or `norm.ppf`
elsewhere. #1158 published the rest: `Pdf`, `Cdf`, `Sf`, `Quantile` and `Isf` for each of the four
laws. It needed one new piece of numerics, the inverse of the incomplete gamma behind the
chi-squared quantiles, and it fixed one defect of the members already out:
[`Distributions.StudentQuantile`](tails/distributions-studentquantile.md) stopped at `1.3e154`,
where squaring the statistic overflowed, so the Cauchy's `1e-300` point, `3.18e299`, was out of
reach. The two quantiles published first also answer `0` and `1` now, as scipy's do.

## What publishing cost

A parity promise of its own. The internal tests check these functions against **closed forms**,
which is enough for a p-value near 0.05 and says nothing about the far tail. Two corpora replay
`scipy.stats`, compared **relatively**, since an absolute `1e-9` would accept an implementation
returning zero: the first down to `3.1e-24`, and the second, for the twenty members, over a grid
reaching `1e-300` on either side. That grid keeps a scipy quantile only where scipy's own tail,
read at it, gives the probability back: at `p = 1e-8`, `f.isf` is `5e-9` off its closed form, and
several inverses clamp in the far tail to a value whose tail is another probability entirely.

It also cost a correction. The internal helper behind
[`Distributions.StudentQuantile`](tails/distributions-studentquantile.md) solves
`P(T > x) = p` and therefore carries the opposite sign to a quantile. Published under its own name
unchanged, it would have handed a caller `-2.18` where every table prints `+2.18`.

## Types

| Type | What it is |
| --- | --- |
| [`Distributions`](tails/distributions.md) | `Pdf`, `Cdf`, `Sf`, `Quantile` and `Isf` of the normal, *t*, *F* and chi-squared laws. |

## See also

- [Hypothesis tests](tests.md) — the ten families built on the same machinery.
- [Python → C# equivalence](../../equivalence.md).
