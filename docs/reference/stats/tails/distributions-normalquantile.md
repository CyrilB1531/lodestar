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
against `scipy.stats.norm.ppf(0.975)`, that substitute stops improving at about **1e-8** relative:

| `df` | relative error |
| --- | --- |
| 1e6 | 1.2e-6 |
| 1e7 | 1.2e-7 |
| **1e8** | **1.2e-8** |
| 1e9 | 1.5e-7 |
| 1e12 | 3.3e-5 |

Below the best point the convergence is incomplete; above it the Student tail being inverted loses ground. A
Kaplan-Meier confidence bound is built on the log-log transform of the estimate, which amplifies
that error into the seventh digit of a bound — past the `1e-9` its corpus compares at. This member
answers to the last bit on Linux instead. [Decision 0003](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md), as `docs/guides/performance.md` amended it,
has the whole measurement.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md),
[`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md).
