# Distributions.NormalCdf

The standard normal's lower tail: `P(Z ≤ z)`.

<!-- docs-declaration -->

```csharp
public static double NormalCdf(double z)
```

**Parameters** — `z` is the point. `NaN` answers `NaN`.

**Returns** — `scipy.stats.norm.cdf(z)`.

**Exceptions** — None.

**Example** — the lower 2.5% point, and a tail too far for `1 − sf` to hold a digit of.

```csharp
using Lodestar.Stats;

double lower = Distributions.NormalCdf(-1.96);   // => 0.024997…
double far = Distributions.NormalCdf(-10.0);     // => 7.619853…
```

**Remarks** — **It is the upper tail of `−z`, not one minus the upper tail of `z`.** Below the median the lower tail is the small number, and `1 − sf` would round it to zero long before a double runs out of range.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.NormalSf`](distributions-normalsf.md), [`Distributions.NormalQuantile`](distributions-normalquantile.md).
