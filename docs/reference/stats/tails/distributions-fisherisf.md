# Distributions.FisherIsf

The `f` with `P(F > f) = p`: the inverse of `FisherSf`.

<!-- docs-declaration -->

```csharp
public static double FisherIsf(double p, double numeratorDf, double denominatorDf)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `+∞` and `1` answers `0`. `numeratorDf` and `denominatorDf` are the two degrees of freedom, each of which must be positive and finite.

**Returns** — `scipy.stats.f.isf(p, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when either degrees of freedom is not positive.

**Example** — the same 5% critical value, read from the upper tail.

```csharp
using Lodestar.Stats;

double critical = Distributions.FisherIsf(0.05, 2.0, 20.0);   // => 3.492828…
```

**Remarks** — **More exact than scipy's at small `p`.** `scipy.stats.f.isf(1e-8, 1, 1)` is `5.0e-9` off in relative terms, measured against the closed form `1/tan²(π·p/2)` and against scipy's own `sf`; this one reproduces both to `4e-15`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.FisherQuantile`](distributions-fisherquantile.md), [`Distributions.FisherSf`](distributions-fishersf.md).
