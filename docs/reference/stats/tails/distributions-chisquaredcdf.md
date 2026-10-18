# Distributions.ChiSquaredCdf

The chi-squared lower tail: `P(X ≤ x)`.

<!-- docs-declaration -->

```csharp
public static double ChiSquaredCdf(double x, double df)
```

**Parameters** — `x` is the statistic; below zero the tail is zero, and `NaN` answers `NaN`. `df` is the degrees of freedom, which must be positive and finite.

**Returns** — `scipy.stats.chi2.cdf(x, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included.

**Example** — the mass below the 5% critical value on one degree of freedom.

```csharp
using Lodestar.Stats;

double below = Distributions.ChiSquaredCdf(3.84, 1.0);   // => 0.949956…
```

**Remarks** — The regularized lower incomplete gamma `P(k/2, x/2)`, evaluated on its own side.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md), [`Distributions.ChiSquaredQuantile`](distributions-chisquaredquantile.md).
