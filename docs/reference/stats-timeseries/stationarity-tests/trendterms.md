# TrendTerms

The deterministic terms a unit-root or stationarity regression carries.

<!-- docs-declaration -->

```csharp
public enum TrendTerms
```

**Fields** — `None` is no constant and no trend, the reference's `"n"`, for the augmented
Dickey-Fuller test only. `Constant` is a constant, `"c"`, the default of both tests.
`ConstantAndTrend` adds a linear trend, `"ct"`. `ConstantAndQuadraticTrend` adds a linear and a
quadratic trend, `"ctt"`, for the augmented Dickey-Fuller test only.

**Example** — the same series against a quadratic trend, where the unit root is rejected outright.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult quadratic = Stationarity.AugmentedDickeyFuller(
    walk, new DickeyFullerOptions { Regression = TrendTerms.ConstantAndQuadraticTrend });

double statistic = Math.Round(quadratic.Statistic, 4);  // => -15.9543
int usedLag = quadratic.UsedLag;                        // => 1
```

**Remarks** — each choice reads its own MacKinnon table, so the same statistic means different
things under different terms. [`KpssOptions.Regression`](kpssoptions.md) refuses the two the KPSS
test does not define.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`DickeyFullerOptions`](dickeyfulleroptions.md), [`KpssOptions`](kpssoptions.md).
