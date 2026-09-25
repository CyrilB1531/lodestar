# Distributions.NormalPdf

The standard normal density.

<!-- docs-declaration -->

```csharp
public static double NormalPdf(double z)
```

**Parameters** — `z` is the point. `NaN` answers `NaN`.

**Returns** — `scipy.stats.norm.pdf(z)`.

**Exceptions** — None.

**Example** — the density at one standard deviation.

```csharp
using Lodestar.Stats;

double atOne = Distributions.NormalPdf(1.0);   // => 0.241970…
```

**Remarks** — `exp(−z²/2) / √(2π)`, which underflows to zero past `|z| ≈ 38.6`, where scipy's does too.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.NormalCdf`](distributions-normalcdf.md), [`Distributions.NormalSf`](distributions-normalsf.md).
