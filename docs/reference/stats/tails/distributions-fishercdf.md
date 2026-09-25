# Distributions.FisherCdf

The *F* lower tail: `P(F ≤ f)`.

<!-- docs-declaration -->

```csharp
public static double FisherCdf(double f, double numeratorDf, double denominatorDf)
```

**Parameters** — `f` is the statistic; below zero the tail is zero, and `NaN` answers `NaN`. `numeratorDf` and `denominatorDf` are the two degrees of freedom, each of which must be positive and finite.

**Returns** — `scipy.stats.f.cdf(f, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when either degrees of freedom is not positive, `NaN` included.

**Example** — the mass below an overall test statistic of four.

```csharp
using Lodestar.Stats;

double below = Distributions.FisherCdf(4.0, 2.0, 20.0);   // => 0.965428…
```

**Remarks** — The regularized incomplete beta `I_y(d₁/2, d₂/2)` at `y = d₁f/(d₁f + d₂)`, evaluated on its own side, never as one minus [`FisherSf`](distributions-fishersf.md). `y` and `1 − y` are each formed by division, so at shapes `(1e10, 1)`, where `y` rounds to one, the complement still carries its digits; at `+∞` the tail is one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.FisherSf`](distributions-fishersf.md), [`Distributions.FisherQuantile`](distributions-fisherquantile.md).
