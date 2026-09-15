# SeasonalComponents

A series split into its trend, its seasonal pattern and what neither explains.

<!-- docs-declaration -->

```csharp
public sealed class SeasonalComponents
```

**Properties** — `Trend` is the centred, or trailing, moving average. `Seasonal` is the average
detrended value at each phase, centred, tiled over the series. `Residual` is the series less the
trend and the seasonal component, or divided by them under the multiplicative model.

**Example** — the additive components add back to the series wherever the trend is defined.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents parts = SeasonalDecomposition.Decompose(quarterly, period: 4);

double rebuilt = Math.Round(parts.Trend[5] + parts.Seasonal[5] + parts.Residual[5], 4);  // => 15
int length = parts.Seasonal.Count;                                                       // => 12
```

**Remarks — there is no public constructor.** The components are what
[`SeasonalDecomposition.Decompose`](seasonaldecomposition-decompose.md) returns. The trend and the
residual are `NaN` where the moving average has no full window, unless
[`SeasonalDecompositionOptions.ExtrapolateTrend`](seasonaldecompositionoptions.md) filled them.

A class rather than a record, for the reason
[`AutocorrelationResult`](../correlation/autocorrelationresult.md) gives.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SeasonalDecomposition.Decompose`](seasonaldecomposition-decompose.md).
