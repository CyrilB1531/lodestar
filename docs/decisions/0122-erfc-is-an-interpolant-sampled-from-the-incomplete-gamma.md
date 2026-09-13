---
status: accepted
supersedes: []
amends: ["0081"]
applies: []
---
# 0122 — `erfc` is an interpolant sampled from the incomplete gamma, and whole or half-integer tails are finite sums

**Status:** accepted · **Date:** 2026-09-13 · **Amends:** [0081](0081-the-stats-numerical-layer-stays-internal.md)

## Context

[Decision 0081](0081-the-stats-numerical-layer-stays-internal.md) recorded that `Normal.Erfc(x)` is
computed as `Q(1/2, x²)`, "not a rational fit of its own". It gave the reason: one far-tail
approximation to get right, not two whose disagreement would show between a z-test and a
chi-square test on the same squared statistic.

[#710](https://github.com/CyrilB1531/lodestar/issues/710) measured what that mechanism costs. On a
2×2 table, [`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) was the one
`Lodestar.Stats` family `Accord.Statistics` beat, 192 ns against 123 ns on an AMD Ryzen 7 8700G.
[`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md) at `(4.41, 1)`
alone took 198 ns. The four cells cost a few nanoseconds; the rest was the incomplete gamma's
continued fraction, iterated to convergence on every call at one degree of freedom.
[`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md), which
then bisected on that tail, took 11 µs.

## Decision

**`Normal.Erfc` is a piecewise Chebyshev interpolant of `erfcx(x) = e^(x²) erfc(x)`, sampled
from `Q(1/2, x²)` once, at type initialization.** The domain `[0, 27.3]` maps through
`y = 4/(4+x)` into 32 intervals of degree 8. A call runs one polynomial and one `exp`. Past 27.3
the function returns zero, because `erfc(27.3)` is below half the smallest subnormal.

0081's reason survives the change. There is still one far-tail approximation: the table has no
coefficient of its own, only samples of the continued fraction 0081 chose. What changes is when
that fraction runs. Against `scipy.special.erfc` on 60,001 points over `[-6, 27.5]`, the worst
relative gap is **7.2e-15**, where the fraction called directly reached 1.1e-13.

**`Gamma.RegularizedQ` at an integer or half-integer shape is a finite sum when `2a ≤ 100` and
`x ≤ 700`.** Integration by parts gives `Q(b+1, x) = Q(b, x) + x^b e^-x / Γ(b+1)`, started from
`Q(1, x) = e^-x` or `Q(1/2, x) = erfc(√x)`. Every term is positive, so the error grows with the term
count and not with `x`. Every chi-squared tail has a shape of that kind, since `a = dof/2`. Any
other shape, and any `x` past 700 where `e^-x` nears the subnormals, keeps the iteration unchanged.

**The bound is measured, on the default job.** At `2a = 100` the sum took 44 ns, against 177 ns
for the iteration at `x = a` and 71 ns at `x = 2a`. At `2a = 60` it took 25 ns against 146 ns and
70 ns. The sum grows linearly with its term count, so its cost reaches the fraction's in the tail
near `2a = 160`; a short run there had the fraction ahead. 100 keeps the margin.

Measured after, same machine and window: `Contingency` 64–67 ns against `Accord`'s 122 ns;
`ChiSquaredSf(4.41, 1)` 16 ns; `NormalQuantile(0.975)` 654 ns. A fractional degree of freedom, which
still iterates, did not move: 66 ns before and 68 ns after.

## Options refused

**W. J. Cody's (1969) rational approximations, as Cephes and scipy's `ndtr` carry them.** They are
the usual `erfc`, and about as fast. Refused on 0081's own ground: a fitted rational function is a
second far-tail approximation with its own coefficients, which the incomplete gamma would then have
to agree with independently. Taking Cephes' coefficients would also have meant transcribing an
implementation, which [decision 0003](0003-provenance-and-licensing.md) allows as a behavioural
reference only.

**A dedicated incomplete-gamma evaluation for every shape (Temme's uniform expansion, with
DiDonato and Morris's regime selection).** Refused as more than #710 needs. The shapes the package's
tests hand the tail are whole or half integers, and the one shape outside them, a caller's
fractional `df` through [`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md), was not what trailed.

**A finite sum with no bound on `2a`.** Refused on the measurement above: past about 160 the sum
costs more than the iteration it replaces.

## Consequences

- `Normal.Erfc`, `Normal.Sf`, `Normal.Quantile`, and every chi-squared tail in `Lodestar.Stats`
  ([`ChiSquare`](../reference/stats/tests/chisquare.md),
  [`KruskalWallis`](../reference/stats/tests/kruskalwallis.md),
  [`SerialCorrelation`](../reference/stats/timeseries/serialcorrelation.md) and
  [`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md)) now go through
  this path. So do `Lodestar.Stats.Regression` and `Lodestar.Survival` once they reference a release
  that carries it.
- Type initialization of `Normal` samples the continued fraction 288 times.
- Past `x ≈ 26.6`, scipy's `erfc` answers exactly zero. This one keeps returning the subnormal value
  up to 27.3, as the continued fraction did before; neither is a normal double there.
- Everything else 0081 decided, the layer staying internal and the relative p-value tolerance
  included, stands unchanged.
