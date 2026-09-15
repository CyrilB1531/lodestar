# SeasonalModel

How the seasonal component combines with the trend.

<!-- docs-declaration -->

```csharp
public enum SeasonalModel
```

**Fields** — `Additive` is `series = trend + seasonal + residual`, the default. `Multiplicative` is
`series = trend · seasonal · residual`, for a season whose swing grows with the level.

**Example** — the multiplicative pattern is a factor per quarter, centred on one.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents scaled = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { Model = SeasonalModel.Multiplicative });

double secondFactor = Math.Round(scaled.Seasonal[1], 4);   // => 1.2577
double thirdResidual = Math.Round(scaled.Residual[2], 4);  // => 0.9804
```

**Remarks** — the multiplicative model divides by the level, so it refuses a series holding a value at
or below zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SeasonalDecompositionOptions`](seasonaldecompositionoptions.md).
