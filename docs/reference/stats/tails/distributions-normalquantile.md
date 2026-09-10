# Distributions.NormalQuantile

The value a standard normal falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double NormalQuantile(double p)
```

**Parameters** — `p` is a probability strictly inside `(0, 1)`. The endpoints are refused rather
than answered with the two infinities.

**Returns** — `scipy.stats.norm.ppf(p)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is not strictly inside `(0, 1)`, `NaN`
included.

**Example** — the multiplier a large-sample confidence interval takes.

```csharp
using Lodestar.Stats;

double ninetyFive = Distributions.NormalQuantile(0.975);  // => 1.959963…
double ninetyNine = Distributions.NormalQuantile(0.995);  // => 2.575829…
double median = Distributions.NormalQuantile(0.5);  // => 0
```

**Remarks** — **this is the quantile, not the inverse survival function.** The internal helper
solves `P(Z > z) = p` and carries the opposite sign; the published member negates, which is the
distribution's symmetry about zero rather than a correction. The same trap
[`Distributions.StudentQuantile`](distributions-studentquantile.md) documents, and the same answer.

At the median it returns **positive** zero. Negating the helper's exact zero would otherwise hand a
caller `-0` from a published method — true of `0.0 == -0.0`, but not something an API should print.

**Why this exists rather than a Student quantile at a large degrees of freedom.** Student converges
on the normal, so the obvious substitute is `StudentQuantile(p, df)` with `df` very large. Measured
against `scipy.stats.norm.ppf(0.975)`, that substitute stops improving at about **9e-9** relative:

| `df` | relative error |
| --- | --- |
| 1e6 | 1.2e-6 |
| 1e7 | 1.2e-7 |
| **1e8** | **9.1e-9** |
| 1e9 | 1.5e-7 |
| 1e12 | 3.3e-5 |

Below the best point the convergence is incomplete; above it the bisection loses ground. A
Kaplan-Meier confidence bound is built on the log-log transform of the estimate, which amplifies
that error into the seventh digit of a bound — past the `1e-9` its corpus compares at. This member
answers to about `1e-15` instead. [Decision 0098](../../../decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
has the whole measurement.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentQuantile`](distributions-studentquantile.md),
[`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md).
