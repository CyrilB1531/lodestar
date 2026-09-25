# Distributions.ChiSquaredIsf

The `x` with `P(X > x) = p`: the inverse of `ChiSquaredSf`.

<!-- docs-declaration -->

```csharp
public static double ChiSquaredIsf(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `+∞` and `1` answers `0`. `df` is the degrees of freedom, which must be positive and finite.

**Returns** — `scipy.stats.chi2.isf(p, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when `df` is not positive.

**Example** — the 5% critical value of a test on four degrees of freedom.

```csharp
using Lodestar.Stats;

double critical = Distributions.ChiSquaredIsf(0.05, 4.0);   // => 9.487729…
```

**Remarks** — The mirror of [`ChiSquaredQuantile`](distributions-chisquaredquantile.md), inverting the upper tail directly so a small `p` keeps its digits.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.ChiSquaredQuantile`](distributions-chisquaredquantile.md), [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md).
