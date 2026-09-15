# DickeyFullerResult

An augmented Dickey-Fuller test: the statistic, its MacKinnon p-value and what the regression used.

<!-- docs-declaration -->

```csharp
public sealed class DickeyFullerResult
```

**Properties** — `Statistic` is the lagged level's t statistic in the chosen regression. `PValue` is
MacKinnon's (1994) approximate p-value against the null of a unit root. `UsedLag` is the lag order
the regression was fitted at. `ObservationCount` is how many rows that regression fitted: the series
length less the lag, less one. `CriticalValues` holds MacKinnon's (2010) critical values at 1 %, 5 %
and 10 %, for `ObservationCount`. `InformationCriterion` is the winning criterion of the lag search,
`NaN` under `LagSelection.Fixed`.

**Example** — the statistic sits above every critical value, so no level rejects a unit root.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(walk);

double tenPercent = Math.Round(result.CriticalValues[2], 4);     // => -2.6507
bool rejectsAtTen = result.Statistic < result.CriticalValues[2];  // => False
```

**Remarks — there is no public constructor.** A result is what
[`Stationarity.AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md) returns. Under
`LagSelection.TStatistic`, `InformationCriterion` is the absolute t statistic of the last lag tried,
as the reference's `icbest` is.

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult result = Stationarity.AugmentedDickeyFuller(
    walk, new DickeyFullerOptions { LagSelection = LagSelection.TStatistic });

double lastLagT = Math.Round(result.InformationCriterion, 4);  // => 2.442
```

A class rather than a record, for the reason
[`AutocorrelationResult`](../correlation/autocorrelationresult.md) gives: a record's equality would
compare `CriticalValues` by reference.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Stationarity.AugmentedDickeyFuller`](stationarity-augmenteddickeyfuller.md),
[`DickeyFullerOptions`](dickeyfulleroptions.md), the [Python equivalence table](../../../equivalence.md).
