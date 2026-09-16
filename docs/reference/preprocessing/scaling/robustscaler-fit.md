# RobustScaler.Fit

Fits a scaler on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static RobustScaler Fit(ReadOnlySpan<double> samples, int featureCount, RobustScalerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `options` chooses which steps to apply and
between which percentiles; `null` applies both, at the quartiles.

**Returns** — a fitted `RobustScaler`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, or the
percentile range is not `0 ≤ lower ≤ upper ≤ 100`. `ArgumentException` when `samples` holds no row,
a partial one, or a non-finite value.

**Example** — the percentile interpolates between two values when it falls between them.

```csharp
using Lodestar.Preprocessing;

// Six values: the lower quartile sits at h = (6 - 1) * 0.25 = 1.25, between 1 and 2.
double[] six = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];

RobustScaler scaler = RobustScaler.Fit(six, featureCount: 1);

double centre = scaler.Centre![0];  // => 2.5
double scale = scaler.Scale![0];    // => 2.5
```

**Remarks** — **the percentile is `numpy.percentile`'s linear interpolation** (Hyndman and Fan's
type 7): at `h = (n − 1)·q/100` the value is `x[⌊h⌋] + (h − ⌊h⌋)·(x[⌊h⌋+1] − x[⌊h⌋])` over the
sorted column. Neither quantile already in this repository is that convention —
`SplitConformal.Quantile` takes the conformal order statistic `⌈(n+1)(1−α)⌉` and `Lodestar.Metrics`
averages a weighted percentile — so it is written here rather than shared, and this page says so
instead of leaving a reader to assume.

Five values put the quartiles **on** an index (`h = 1` and `h = 3`) and six put them between two,
which is the pair worth reading twice.

The interpercentile range is floored to 1 below `10·eps`, the rule
[`MinMaxScaler.Fit`](minmaxscaler-fit.md) carries.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScaler`](robustscaler.md), [`RobustScalerOptions`](robustscaleroptions.md).
