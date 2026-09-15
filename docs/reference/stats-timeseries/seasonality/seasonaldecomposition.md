# SeasonalDecomposition

Classical decomposition of a series into trend, seasonal pattern and residual, by moving averages.

<!-- docs-declaration -->

```csharp
public static class SeasonalDecomposition
```

**Example** — three years of quarterly figures, a rising level with a strong second quarter.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents parts = SeasonalDecomposition.Decompose(quarterly, period: 4);

double secondQuarter = Math.Round(parts.Seasonal[1], 4);  // => 3.125
```

**Remarks** — classical decomposition, not STL: the trend is a centred moving average and the
seasonal pattern the average detrended value at each phase. STL's loess smoothing is a different
algorithm and is not in this package.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the seasonality index](../seasonality.md), [`Stationarity`](../stationarity-tests/stationarity.md).

## Members

| Member | What it does |
| --- | --- |
| [`SeasonalDecomposition.Decompose`](seasonaldecomposition-decompose.md) | Splits a series into its trend, seasonal and residual components. |
