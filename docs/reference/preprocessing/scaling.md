# Feature scaling — `Lodestar.Preprocessing`

One type, [`StandardScaler`](scaling/standardscaler.md): it centres each feature on its mean and
scales it to unit variance, at `sklearn.preprocessing.StandardScaler` parity.

**Spans in, arrays out.** A sample matrix is row-major — `FeatureCount` values per row — which is
the shape [`Lodestar.Metrics`](../metrics/classification.md) already uses, so a matrix does not have
to be reshaped to cross between the two packages. There is no pipeline object to build, no data
view to construct, and nothing to adopt beyond the call.

## Why this exists when ML.NET has normalizers

ML.NET has every brick this package will grow: `NormalizeMeanVariance`, `NormalizeMinMax`,
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

## See also

- [scikit-learn → .NET](../../migration/sklearn.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
