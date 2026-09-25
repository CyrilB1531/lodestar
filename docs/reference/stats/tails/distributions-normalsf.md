# Distributions.NormalSf

The standard normal's upper tail: `P(Z > z)`.

<!-- docs-declaration -->

```csharp
public static double NormalSf(double z)
```

**Parameters** — `z` is the point. `NaN` answers `NaN`.

**Returns** — `scipy.stats.norm.sf(z)`.

**Exceptions** — None.

**Example** — three standard deviations up.

```csharp
using Lodestar.Stats;

double threeSigma = Distributions.NormalSf(3.0);   // => 0.001349…
```

**Remarks** — The same tail the package's tests read internally, published under #1158.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.NormalCdf`](distributions-normalcdf.md), [`Distributions.NormalIsf`](distributions-normalisf.md).
