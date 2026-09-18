# KpssOptions

What a KPSS test may be told.

<!-- docs-declaration -->

```csharp
public sealed record KpssOptions
```

**Properties** — `Regression` is stationarity around a level (`TrendTerms.Constant`, the default) or
around a line (`TrendTerms.ConstantAndTrend`). `LagRule` is how the long-run variance's window is
chosen, a [`KpssLagRule`](kpsslagrule.md); `Automatic` by default. `LagCount` is the window under
`KpssLagRule.Fixed`, and ignored otherwise; `0` by default.

**Exceptions** — `ArgumentOutOfRangeException` when `Regression` is `TrendTerms.None` or
`TrendTerms.ConstantAndQuadraticTrend`, which KPSS does not define, when `LagRule` is a value
`KpssLagRule` does not declare, or when `LagCount` is negative; each checked where the value is set.

**Example** — Schwert's legacy window is wider than Hobijn's on this series, and the statistic falls
with it.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

KpssResult legacy = Stationarity.Kpss(walk, new KpssOptions { LagRule = KpssLagRule.Legacy });

int window = legacy.LagCount;                        // => 9
double statistic = Math.Round(legacy.Statistic, 4);  // => 0.397
double p = Math.Round(legacy.PValue, 4);             // => 0.0784
```

**Remarks** — being a `record` of value types, two option sets with the same three values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.Kpss`](stationarity-kpss.md), [`KpssResult`](kpssresult.md), the
[Python equivalence table](../../../equivalence.md).
