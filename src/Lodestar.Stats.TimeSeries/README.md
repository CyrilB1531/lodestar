# Lodestar.Stats.TimeSeries

The diagnostics around a time-series model rather than the forecast: the autocorrelation
and partial autocorrelation functions with their confidence bands, the Ljung-Box test, the
augmented Dickey-Fuller and KPSS stationarity tests with MacKinnon p-values, seasonal
decomposition, and vector autoregression.

## Install

```bash
dotnet add package Lodestar.Stats.TimeSeries
```

## Example

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents parts = SeasonalDecomposition.Decompose(quarterly, period: 4);
double secondQuarter = parts.Seasonal[1];   // 3.125
```

## Parity

Replayed against `statsmodels.tsa`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Stats` 0.5.0 or later
- `Lodestar.Stats.Regression` 0.2.0 or later
- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [time series diagnostics](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/time-series-diagnostics.md)
- Reference: [stats-timeseries/correlation](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-timeseries/correlation.md)
- Reference: [stats-timeseries/seasonality](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-timeseries/seasonality.md)
- Reference: [stats-timeseries/stationarity-tests](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-timeseries/stationarity-tests.md)
- Reference: [stats-timeseries/var](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-timeseries/var.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats.TimeSeries/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats.TimeSeries/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
