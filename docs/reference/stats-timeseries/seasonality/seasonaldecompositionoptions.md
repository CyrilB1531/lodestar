# SeasonalDecompositionOptions

What a seasonal decomposition may be told.

<!-- docs-declaration -->

```csharp
public sealed record SeasonalDecompositionOptions
```

**Properties** — `Model` is additive or multiplicative, a [`SeasonalModel`](seasonalmodel.md);
`Additive` by default. `TwoSided` says whether the moving average is centred, `true` by default, or
trails the point. `ExtrapolateTrend` is how many of the nearest defined trend points, less one, fit
the lines that fill the trend's undefined ends; `0` by default, which leaves them `NaN`.

**Exceptions** — `ArgumentOutOfRangeException` when `ExtrapolateTrend` is negative, checked where the
value is set.

**Example** — the ends filled by lines through the nearest two defined points.

```csharp
using Lodestar.Stats.TimeSeries;

double[] quarterly = [10.0, 14.0, 8.0, 12.0, 11.0, 15.0, 9.0, 13.0, 12.0, 16.0, 10.0, 14.0];

SeasonalComponents filled = SeasonalDecomposition.Decompose(
    quarterly, 4, new SeasonalDecompositionOptions { ExtrapolateTrend = 1 });

double firstTrend = Math.Round(filled.Trend[0], 4);  // => 10.625
double lastTrend = Math.Round(filled.Trend[11], 4);  // => 13.375
```

**Remarks** — the reference's `extrapolate_trend="period"` is `period − 1` here. Its back window
leaves the last defined point out of the fit, and so does this one: the corpus freezes that.

Being a `record` of value types, two option sets with the same three values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SeasonalDecomposition.Decompose`](seasonaldecomposition-decompose.md),
[`SeasonalComponents`](seasonalcomponents.md).
