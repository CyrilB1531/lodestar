# Distributions.ChiSquaredSf

The upper tail of the chi-squared distribution: `P(X > x)`.

<!-- docs-declaration -->

```csharp
public static double ChiSquaredSf(double x, double df)
```

**Parameters** — `x` is the statistic. `df` is the degrees of freedom, which must be positive.

**Returns** — `scipy.stats.chi2.sf(x, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included. `x` is
**not** refused: the distribution has no mass below zero, so a non-positive statistic returns one
rather than throwing.

**Example** — a log-rank test on two groups carries one degree of freedom.

```csharp
using Lodestar.Stats;

double logRank = Distributions.ChiSquaredSf(3.84, 1.0);  // => 0.0500…

// Four degrees of freedom, as a k-sample comparison of five groups would have.
double kSample = Distributions.ChiSquaredSf(9.488, 4.0);  // => 0.0499…

// Far into the tail, where an absolute tolerance would accept a zero.
double extreme = Distributions.ChiSquaredSf(120.0, 3.0);  // => 7.716…
```

**Remarks** — the same tail [`ChiSquare.GoodnessOfFit`](../tests/chisquare-goodnessoffit.md) and
[`ChiSquare.Contingency`](../tests/chisquare-contingency.md) already report, exposed for a caller
that computed its own statistic rather than handing this package a table. A log-rank test is the
case that asked for it.

**Below the support it answers one, and does not throw.** The regularized incomplete gamma
underneath validates its own argument, so a negative statistic would otherwise surface an internal
helper's parameter name out of a public method. Returning one is also the right answer: the whole
mass lies above.

Routing a statistic through here and through the chi-squared tests above gives the same p-value to
the last bit, because it is the same function and not a second approximation — which is the
agreement [decision 0081](../../../decisions/0081-the-stats-numerical-layer-stays-internal.md)
argued a re-derived tail would lose.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ChiSquare.Contingency`](../tests/chisquare-contingency.md),
[`Distributions.FisherSf`](distributions-fishersf.md).
