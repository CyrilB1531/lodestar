# Distributions.FisherQuantile

The value an *F* falls below with probability `p`.

<!-- docs-declaration -->

```csharp
public static double FisherQuantile(double p, double numeratorDf, double denominatorDf)
```

**Parameters** — `p` is a probability in `[0, 1]`; `0` answers `0` and `1` answers `+∞`. `numeratorDf` and `denominatorDf` are the two degrees of freedom, each of which must be positive and finite.

**Returns** — `scipy.stats.f.ppf(p, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included, or when either degrees of freedom is not positive.

**Example** — the 5% critical value of an overall test on two and twenty degrees of freedom.

```csharp
using Lodestar.Stats;

double critical = Distributions.FisherQuantile(0.95, 2.0, 20.0);   // => 3.492828…
```

**Remarks** — Inverts the incomplete beta for both `y` and `1 − y`, refining by Newton on whichever of the two is below one half, so neither is one minus a number near one: at shapes `(1e10, 1)` the root sits `2e-10` below one, and forming `1 − y` from it was `1e-7` off. **Where scipy disagrees, scipy is checked against itself**: at `p = 1e-100` with `dfn = 0.5` it answers `1.3e-307`, whose own lower tail is `1.4e-77`; the true point underflows, and this answers zero. `docs/equivalence.md` lists these points.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.FisherIsf`](distributions-fisherisf.md), [`Distributions.FisherCdf`](distributions-fishercdf.md).
