# Distributions.ChiSquaredPdf

The chi-squared density.

<!-- docs-declaration -->

```csharp
public static double ChiSquaredPdf(double x, double df)
```

**Parameters** — `x` is the point; below zero the density is zero, and `NaN` answers `NaN`. `df` is the degrees of freedom, which must be positive and finite.

**Returns** — `scipy.stats.chi2.pdf(x, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included.

**Example** — the density at two, on four degrees of freedom.

```csharp
using Lodestar.Stats;

double atTwo = Distributions.ChiSquaredPdf(2.0, 4.0);   // => 0.183939…
```

**Remarks** — The incomplete gamma's prefactor `(x/2)^(k/2) e^(−x/2) / Γ(k/2)` over `x`, which past ten degrees of freedom forms no term of order `k log k`. At zero the density follows `df` as scipy's does: infinite below two, one half at two, zero above.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.ChiSquaredCdf`](distributions-chisquaredcdf.md), [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md).
