# Time-series diagnostics

A regression that ignores time assumes the rows are independent and the process does not drift.
This guide is about checking both, in the order a reader meets them: before a model is fitted, what
the season is, and after the model, what it left behind. Everything here is in
`Lodestar.Stats.TimeSeries`, at `statsmodels` 0.15.0 parity.

## Before modelling: is the series stationary?

A series with a unit root — a random walk, or anything that drifts with its own past — breaks the
arithmetic every regression and correlogram rests on: two unrelated walks correlate strongly by
accident. Two tests ask the question from opposite sides:
[`Stationarity.AugmentedDickeyFuller`](../reference/stats-timeseries/stationarity-tests/stationarity-augmenteddickeyfuller.md)
and [`Stationarity.Kpss`](../reference/stats-timeseries/stationarity-tests/stationarity-kpss.md).

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult adf = Stationarity.AugmentedDickeyFuller(walk);
KpssResult kpss = Stationarity.Kpss(walk);

double adfP = Math.Round(adf.PValue, 4);   // => 0.9986
double kpssP = Math.Round(kpss.PValue, 4);  // => 0.0128
```

The augmented Dickey-Fuller test's null is a unit root; KPSS's is stationarity. Read together:

| ADF rejects? | KPSS rejects? | reading |
| --- | --- | --- |
| yes | no | stationary: both point the same way |
| no | yes | a unit root: difference the series and test again — the case above |
| yes | yes | stationary around something the test did not model; try `TrendTerms.ConstantAndTrend` |
| no | no | not enough data to tell |

**The trend terms change the question.** Against a linear trend the same series is a clean
trend-stationary one, and ADF rejects outright:

```csharp
using Lodestar.Stats.TimeSeries;

double[] walk = [0.0, 1.2, 0.7, 2.1, 3.0, 2.4, 3.9, 5.1, 4.6, 6.0, 7.3, 6.8,
                 8.2, 9.5, 9.1, 10.4, 11.8, 11.2, 12.7, 14.0, 13.5, 14.9, 16.2, 15.8];

DickeyFullerResult trend = Stationarity.AugmentedDickeyFuller(
    walk,
    new DickeyFullerOptions
    {
        Regression = TrendTerms.ConstantAndTrend,
        LagSelection = LagSelection.Fixed,
        MaxLag = 1,
    });

double p = Math.Round(trend.PValue, 4);  // => 0
```

**A KPSS p-value of 0.01 or 0.10 may be the end of its table.** KPSS interpolates in four tabulated
critical values; past either end it returns the end value, and
[`KpssResult.PValueBound`](../reference/stats-timeseries/stationarity-tests/kpssresult.md) says which
way the truth lies. Report the bound, not the number.

## The season: what repeats, and how much

[`SeasonalDecomposition.Decompose`](../reference/stats-timeseries/seasonality/seasonaldecomposition-decompose.md)
splits a series into a centred moving-average trend, the average deviation at each phase, and the
rest.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents additive = SeasonalDecomposition.Decompose(quarterly, period: 4);
double secondQuarter = Math.Round(additive.Seasonal[1], 4);  // => 3.125

SeasonalComponents multiplicative = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { Model = SeasonalModel.Multiplicative });
double secondFactor = Math.Round(multiplicative.Seasonal[1], 4);  // => 1.2577
```

Additive when the seasonal swing is the same size at every level; multiplicative when it grows with
the level, and then the pattern is a factor. The period is always given — nothing here guesses it —
and the trend's first and last half-period are `NaN` unless
[`SeasonalDecompositionOptions.ExtrapolateTrend`](../reference/stats-timeseries/seasonality/seasonaldecompositionoptions.md)
fills them.

## After modelling: did the model leave anything behind?

A model that captured the dynamics leaves residuals with no serial correlation.
[`SerialCorrelation.LjungBox`](../reference/stats-timeseries/correlation/serialcorrelation-ljungbox.md)
asks that of the residuals, with the model's parameter count taken off each lag's degrees of freedom:

```csharp
using Lodestar.Stats.TimeSeries;

double[] residuals = [0.3, -0.5, 0.9, -0.2, 0.1, -0.8, 0.6, -0.4, 0.2, -0.1, 0.7, -0.6];

LjungBoxResult test = SerialCorrelation.LjungBox(
    residuals, lagCount: 4, new LjungBoxOptions { ModelDegreesOfFreedom = 1 });

int degreesOfFreedomAtLag4 = test.DegreesOfFreedom[3];  // => 3
```

The correlogram behind it — [`SerialCorrelation.Autocorrelation`](../reference/stats-timeseries/correlation/serialcorrelation-autocorrelation.md)
and its partial counterpart — is where to look for *which* lag carries what is left.

## See also

- [the stationarity tests](../reference/stats-timeseries/stationarity-tests.md),
  [seasonality](../reference/stats-timeseries/seasonality.md) and
  [serial correlation](../reference/stats-timeseries/correlation.md) reference sections
- [statsmodels → .NET](../migration/statsmodels.md) — what is native and what is delegated
- [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) —
  why this is a package of its own
