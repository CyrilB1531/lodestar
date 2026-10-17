# Distributions.NormalQuantile

The value a standard normal falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double NormalQuantile(double p)
```

**Parameters** — `p` is a probability in `[0, 1]`. `0` answers `−∞` and `1` answers `+∞`, as
scipy's do; they were refused until #1158.

**Returns** — `scipy.stats.norm.ppf(p)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included.

**Example** — the multiplier a large-sample confidence interval takes.

```csharp
using Lodestar.Stats;

double ninetyFive = Distributions.NormalQuantile(0.975);  // => 1.959963…
double ninetyNine = Distributions.NormalQuantile(0.995);  // => 2.575829…
double median = Distributions.NormalQuantile(0.5);  // => 0
```

**Remarks** — **this is the quantile, not the inverse survival function**: `P(Z ≤ z) = p`, where
[`Distributions.NormalIsf`](distributions-normalisf.md) solves `P(Z > z) = p`. It is
`scipy.special.ndtri`, the function `norm.ppf` calls, ported statement for statement from Cephes'
rational approximations, so the value is scipy's to the last bit on both tails and the centre where
.NET's logarithm is glibc's, as on Linux, and to `1e-14` relative elsewhere
([#1684](https://github.com/CyrilB1531/lodestar/issues/1684)); at the median it is exactly zero.

**Why this exists rather than a Student quantile at a large degrees of freedom.** Student converges
on the normal, so the obvious substitute is `StudentQuantile(p, df)` with `df` very large. Measured
against `scipy.stats.norm.ppf(0.975)`, its relative error is the two laws' own distance, about
`1.2 / df` until rounding takes over:

| `df` | relative error |
| --- | --- |
| 1e6 | 1.2e-6 |
| 1e8 | 1.2e-8 |
| 1e9 | 1.2e-9 |
| 1e12 | 1.2e-12 |
| 1e16 | 1.1e-16 |

Past that rounding decides: one ulp off at `1e16`, two at `1e17` and at `9.9e19`, and exact from
`df = 1e20`, where the Student law is the normal one. A Kaplan-Meier confidence bound is
built on the log-log transform of the estimate, which amplifies a smaller `df`'s distance into the
digits of a bound its corpus compares at `1e-9`. This member answers to the last bit on Linux
instead. [Decision 0003](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
publishes a member once another package names it, and `Lodestar.Survival`, `Lodestar.Preprocessing`,
`Lodestar.Stats.Regression` and `Lodestar.Stats.TimeSeries` call this one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md),
[`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md).
