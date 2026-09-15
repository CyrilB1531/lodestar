# SeasonalDecomposition.Decompose

Splits a series into its trend, seasonal and residual components.

<!-- docs-declaration -->

```csharp
public static SeasonalComponents Decompose(ReadOnlySpan<double> series, int period, SeasonalDecompositionOptions options = null)
```

**Parameters** — `series` are the observations, in time order, at least two full periods of them.
`period` is the season's length in observations: 12 for monthly data with a yearly season. `options`
sets the model, the filter's sides and the trend extrapolation, or null for the defaults.

**Returns** — [`SeasonalComponents`](seasonalcomponents.md): three lists the length of the series.

**Exceptions** — `ArgumentOutOfRangeException` when `period` is below two. `ArgumentException` when
`series` carries a non-finite value, holds fewer than two periods, or, under
`SeasonalModel.Multiplicative`, a value at or below zero.

**Example** — the trend is undefined for the first and last two quarters, where a centred window of
five does not fit; the pattern is exact here, so the residual is zero wherever the trend exists.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents parts = SeasonalDecomposition.Decompose(quarterly, period: 4);

bool undefinedStart = double.IsNaN(parts.Trend[0]);     // => True
double thirdTrend = Math.Round(parts.Trend[2], 4);      // => 11.125
double thirdResidual = Math.Round(parts.Residual[2], 4);  // => 0
```

**Remarks** — **`period` is required.** The reference infers it from a pandas index, which a span
does not have, and a guessed period is a wrong decomposition that still looks plausible.

An even period's filter weights its two end points by one half over `period + 1` observations; an
odd one weights `period` observations equally. One-sided, the window trails the point instead of
centring on it, and only the start is undefined.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents trailing = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { TwoSided = false });

bool fourthUndefined = double.IsNaN(trailing.Trend[3]);  // => True
double fifthTrend = Math.Round(trailing.Trend[4], 4);    // => 11.125
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SeasonalDecompositionOptions`](seasonaldecompositionoptions.md),
[`SeasonalComponents`](seasonalcomponents.md), the [Python equivalence table](../../../equivalence.md).
