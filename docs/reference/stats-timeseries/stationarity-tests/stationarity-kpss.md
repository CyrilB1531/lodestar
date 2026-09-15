# Stationarity.Kpss

The KPSS test, against the null of stationarity around a level or a line.

<!-- docs-declaration -->

```csharp
public static KpssResult Kpss(ReadOnlySpan<double> series, KpssOptions options = null)
```

**Parameters** — `series` are the observations, in time order. `options` sets the null's trend and
the lag window rule, or null for the reference's defaults: a level, and Hobijn's automatic window.

**Returns** — [`KpssResult`](kpssresult.md): the statistic, its tabulated p-value, the window used,
and whether the p-value was clamped at the end of its table.

**Exceptions** — `ArgumentException` when `series` carries a non-finite value or is constant; or
when `options` fixes a window at or above the series length.

**Example** — the drifting series
[`AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md) reads, around a level and around a
line.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

KpssResult level = Stationarity.Kpss(walk);
double levelStatistic = Math.Round(level.Statistic, 4);  // => 0.7082
double levelP = Math.Round(level.PValue, 4);             // => 0.0128
int levelWindow = level.LagCount;                        // => 3

KpssResult line = Stationarity.Kpss(walk, new KpssOptions { Regression = TrendTerms.ConstantAndTrend });
double lineStatistic = Math.Round(line.Statistic, 4);    // => 0.1533
double lineP = Math.Round(line.PValue, 4);               // => 0.0439
int lineWindow = line.LagCount;                          // => 9
```

**Remarks** — the statistic is the sum of the squared partial sums of the residuals, over `n²`, over
a Newey-West long-run variance with a Bartlett kernel. The residuals are the series less its mean,
or less its least-squares line under `TrendTerms.ConstantAndTrend`.

**The p-value is interpolated in a four-point table, and clamped at both ends.** Past either end the
reference returns the end value and warns; this sets
[`KpssResult.PValueBound`](kpssresult.md) instead, so the direction is a value a caller can read.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

KpssResult narrow = Stationarity.Kpss(walk, new KpssOptions { LagRule = KpssLagRule.Fixed, LagCount = 2 });

double statistic = Math.Round(narrow.Statistic, 4);  // => 0.8976
double p = narrow.PValue;                            // => 0.01
PValueBound bound = narrow.PValueBound;              // => ActualIsSmaller
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md),
[`KpssOptions`](kpssoptions.md), [`KpssResult`](kpssresult.md), the
[Python equivalence table](../../../equivalence.md).
