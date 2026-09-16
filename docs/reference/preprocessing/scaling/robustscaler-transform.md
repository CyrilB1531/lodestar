# RobustScaler.Transform

Centres and scales a row-major sample matrix with the fitted statistics.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to transform, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — subtract the median, then divide by the interquartile range.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 2.0, 3.0, 4.0, 5000.0];

RobustScaler scaler = RobustScaler.Fit(samples, featureCount: 1);

double[] robust = scaler.Transform(samples);

double smallest = robust[0];  // => -1
double middle = robust[2];    // => 0
```

**Remarks** — **subtract then divide, in that order**, and the inverse multiplies then adds. Both
steps are optional, so the order is part of the contract rather than an implementation detail: with
centring off, nothing is subtracted and the division is applied to the raw value.

Never writes to the input.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScaler.InverseTransform`](robustscaler-inversetransform.md), [`RobustScalerOptions`](robustscaleroptions.md).
