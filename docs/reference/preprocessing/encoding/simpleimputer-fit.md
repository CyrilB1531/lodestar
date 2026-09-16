# SimpleImputer.Fit

Fits an imputer on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static SimpleImputer Fit(ReadOnlySpan<double> samples, int featureCount, SimpleImputerOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row, `NaN`
where a value is missing. `featureCount` is how many values each row carries. `options` chooses the
statistic; `null` is the mean.

**Returns** — a fitted `SimpleImputer`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the fill value
is not finite. `ArgumentException` when `samples` holds no row, a partial one, or an infinity; or
when a feature has no value at all and
[`KeepEmptyFeatures`](simpleimputeroptions.md) is not set.

**Example** — the median of an even count is the average of the two middle values.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 2.0, 3.0, 4.0, double.NaN];

SimpleImputer imputer = SimpleImputer.Fit(
    samples, 1, new SimpleImputerOptions { Strategy = ImputationStrategy.Median });

double median = imputer.Statistics[0];  // => 2.5
```

**Remarks** — **a feature with nothing in it is refused**, where the reference **drops it** and
returns a matrix one column narrower than the one it was given. That is this package's one
divergence here, and it is deliberate: a transform whose output width depends on the fitted data
rather than on the input shape is a trap in a typed API. Set
[`SimpleImputerOptions.KeepEmptyFeatures`](simpleimputeroptions.md) to fill such a feature with zero
instead, which is the reference's `keep_empty_features=True`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimpleImputer`](simpleimputer.md), [`ImputationStrategy`](imputationstrategy.md).
