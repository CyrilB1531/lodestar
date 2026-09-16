# Feature scaling — `Lodestar.Preprocessing`

Four scalers, at `sklearn.preprocessing` parity: [`StandardScaler`](scaling/standardscaler.md)
centres on the mean and scales to unit variance, [`MinMaxScaler`](scaling/minmaxscaler.md) maps onto
a fixed range, [`MaxAbsScaler`](scaling/maxabsscaler.md) divides by the largest absolute value
without ever subtracting, and [`RobustScaler`](scaling/robustscaler.md) centres on the median and
scales by an interpercentile range.

**Which one, in one line each.** `StandardScaler` is the default for data without outliers;
`RobustScaler` is the one for data with them, since a mean and a deviation both move with whatever
is furthest away; `MaxAbsScaler` is the one for data whose zeros are meaningful, because centring
turns every zero into a non-zero; `MinMaxScaler` is the one for a bounded range a downstream model
asks for.

**Spans in, arrays out.** A sample matrix is row-major — `FeatureCount` values per row — which is
the shape [`Lodestar.Metrics`](../metrics/classification.md) already uses, so a matrix does not have
to be reshaped to cross between the two packages. There is no pipeline object to build, no data
view to construct, and nothing to adopt beyond the call.

## Why this exists when ML.NET has normalizers

ML.NET has every brick this package grows: `NormalizeMeanVariance`, `NormalizeMinMax`,
`OneHotEncoding`, `ReplaceMissingValues`, `TrainTestSplit`, `CrossValidationSplit`. Read on
`Microsoft.ML` 5.0.0's exported surface, **each one of them is reached through an `IDataView` or
through a catalog naming columns** — `NormalizeMeanVariance(TransformsCatalog, string inputColumn,
string outputColumn, …)` never sees a value, and `TrainTestSplit(IDataView, double, …)` takes and
returns data views. The capability is not missing; the array-in, array-out entry point is.

That is the same shape as `TfidfVectorizer` against `FeaturizeText`, and the same reason: a caller
who holds a `double[]` and wants a `double[]` back should not have to adopt a framework to get one.

## Types

| Type | What it is |
| --- | --- |
| [`StandardScaler`](scaling/standardscaler.md) | Centres and scales each feature, and reports the statistics it fitted. |
| [`StandardScalerOptions`](scaling/standardscaleroptions.md) | Which of the two steps to apply. |
| [`MinMaxScaler`](scaling/minmaxscaler.md) | Maps each feature onto a fixed range. |
| [`MinMaxScalerOptions`](scaling/minmaxscaleroptions.md) | The range, and whether the transform clips to it. |
| [`MaxAbsScaler`](scaling/maxabsscaler.md) | Divides each feature by its largest absolute value. |
| [`MaxAbsScalerOptions`](scaling/maxabsscaleroptions.md) | Whether the transform clips to `[−1, 1]`. |
| [`RobustScaler`](scaling/robustscaler.md) | Centres on the median, scales by an interpercentile range. |
| [`RobustScalerOptions`](scaling/robustscaleroptions.md) | Which steps to apply, and between which percentiles. |

## See also

- [scikit-learn → .NET](../../migration/sklearn.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
