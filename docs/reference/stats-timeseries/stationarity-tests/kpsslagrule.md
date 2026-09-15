# KpssLagRule

How KPSS chooses the lag window of its long-run variance.

<!-- docs-declaration -->

```csharp
public enum KpssLagRule
```

**Fields** — `Automatic` is Hobijn, Franses and Ooms' (1998) window, estimated from the residuals —
the reference's `"auto"` and the default. `Legacy` is Schwert's `ceil(12·(n/100)^¼)`, the reference's
`"legacy"`. `Fixed` takes [`KpssOptions.LagCount`](kpssoptions.md) as given.

**Example** — three rules, three windows, on one series.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

int automatic = Stationarity.Kpss(walk).LagCount;                                                      // => 3
int legacy = Stationarity.Kpss(walk, new KpssOptions { LagRule = KpssLagRule.Legacy }).LagCount;         // => 9
int fixedWindow = Stationarity.Kpss(walk, new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = 2 }).LagCount;  // => 2
```

**Remarks** — both computed windows are capped at one below the series length.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KpssOptions`](kpssoptions.md), [`Stationarity.Kpss`](stationarity-kpss.md).
