# Distribution tails — `Lodestar.Stats`

One type, [`Distributions`](tails/distributions.md): the tail probabilities and the
quantile a caller holding its own statistic needs, without going through a hypothesis test to
reach them.

## Why only four members

This package computes every tail it needs from its own log-gamma, incomplete beta and incomplete
gamma, and [`decisions/0081`](../../decisions/0081-the-stats-numerical-layer-stays-internal.md)
kept that layer **internal** — deliberately, and with the condition for changing its mind written
down: *"publishing later stays possible the day a second package needs the same functions."*

`Lodestar.Stats.Regression` is that day. What it needs is a Student tail for each coefficient's
p-value, a Student quantile for each confidence interval, and an *F* tail for the overall test.
Those three are published. The log-gamma, the incomplete beta and gamma, the normal tail and the
finite-sample Kolmogorov distribution underneath them stay internal, because nothing has asked for
them and an unpublished API can still be published later — the reverse is not true.

## What publishing cost

A parity promise of its own. The internal tests check these functions against **closed forms**,
which is enough for a p-value near 0.05 and says nothing about the far tail. The corpus behind
this page replays `scipy.stats` down to `3.1e-24`, compared **relatively**: an absolute `1e-9`
there would accept an implementation returning zero.

It also cost a correction. The internal helper behind
[`Distributions.StudentQuantile`](tails/distributions-studentquantile.md) solves
`P(T > x) = p` and therefore carries the opposite sign to a quantile. Published under its own name
unchanged, it would have handed a caller `-2.18` where every table prints `+2.18`.

## Types

| Type | What it is |
| --- | --- |
| [`Distributions`](tails/distributions.md) | Two Student tails and the *F* tail, for a caller holding its own statistic. |

## See also

- [Hypothesis tests](tests.md) — the ten families built on the same machinery.
- [Python → C# equivalence](../../equivalence.md).
