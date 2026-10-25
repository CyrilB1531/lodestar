# Distributions.ChiSquaredQuantile

The value a chi-squared falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double ChiSquaredQuantile(double p, double df)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `0` and `1` answers `+∞`. `df` is the degrees of freedom, which must be positive and finite.

**Returns** — `scipy.stats.chi2.ppf(p, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when `df` is not positive.

**Example** — the 95% point on one degree of freedom, and the lower bound of a variance interval.

```csharp
using Lodestar.Stats;

double critical = Distributions.ChiSquaredQuantile(0.95, 1.0);   // => 3.841458…
double lower = Distributions.ChiSquaredQuantile(0.025, 10.0);    // => 3.246972…
```

**Remarks** — The incomplete gamma inverted by Newton on its logarithm against `log x`, in a bracket that grows until an evaluation closes it, seeded by Wilson-Hilferty or, deep in the lower tail, by the leading term `x^a / Γ(a + 1)`. Whichever of `p` and `1 − p` is smaller is the one solved.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.ChiSquaredIsf`](distributions-chisquaredisf.md), [`Distributions.ChiSquaredCdf`](distributions-chisquaredcdf.md).
