# MinMaxScaler.PartialFit

Folds another batch into the range and the scale it implies, as `partial_fit` does.

<!-- docs-declaration -->

```csharp
public MinMaxScaler PartialFit(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the next batch, row-major, with `FeatureCount` values per row.

**Returns** — a new scaler summarising every batch seen so far; the one it was called on is unchanged.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — a second batch that widens the range, and one that does not.

```csharp
using Lodestar.Preprocessing;

MinMaxScaler scaler = MinMaxScaler.Fit([0.0, 10.0], featureCount: 1);

MinMaxScaler wider = scaler.PartialFit([20.0]);
MinMaxScaler same = scaler.PartialFit([5.0]);

double widened = wider.DataMaximum[0];  // => 20
double unmoved = same.DataMaximum[0];   // => 10
```

**Remarks** — a running minimum and maximum, and the scale recomputed from them by the same code
`Fit` uses, so the two cannot drift. A batch inside the range moves nothing but `SampleCount`.

**It returns a new scaler and leaves this one alone** — see
[`StandardScaler.PartialFit`](standardscaler-partialfit.md) for why.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScaler`](minmaxscaler.md), [`MinMaxScaler.Fit`](minmaxscaler-fit.md).
