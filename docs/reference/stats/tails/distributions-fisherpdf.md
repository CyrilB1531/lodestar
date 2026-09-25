# Distributions.FisherPdf

The *F* density.

<!-- docs-declaration -->

```csharp
public static double FisherPdf(double f, double numeratorDf, double denominatorDf)
```

**Parameters** — `f` is the point; below zero the density is zero, and `NaN` answers `NaN`. `numeratorDf` and `denominatorDf` are the two degrees of freedom, each of which must be positive and finite.

**Returns** — `scipy.stats.f.pdf(f, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when either degrees of freedom is not positive, `NaN` included.

**Example** — the density at one, for a model with two regressors and twenty residual degrees of freedom.

```csharp
using Lodestar.Stats;

double atOne = Distributions.FisherPdf(1.0, 2.0, 20.0);   // => 0.350493…
```

**Remarks** — At zero the density follows the first shape as scipy's does: infinite when `numeratorDf < 2`, finite at `2`, zero above. At `+∞`, and where `numeratorDf · f` overflows, it is zero.

**Past shapes of a million this is more exact than scipy's.** The density reduces to `y^a (1 − y)^b / (B(a, b) f)` with `y = d₁f/(d₁f + d₂)`, both `y` and `1 − y` formed by division rather than one as one minus the other, and `B` from Stirling's series. scipy's own density is `5e-7` off at `(1e8, 1e8)` and `1e-5` at `(1e10, 1)` against a 60-digit reference, which this one holds to `1e-12`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.FisherCdf`](distributions-fishercdf.md), [`Distributions.FisherSf`](distributions-fishersf.md).
