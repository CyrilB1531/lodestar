# RobustScaler.InverseTransform

Undoes [`Transform`](robustscaler-transform.md), returning values on the original scale.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

<!-- docs-declaration -->

```csharp
public CsrMatrix InverseTransform(CsrMatrix samples)
```

The second overload takes a [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md) and returns a new one storing the same positions, each value multiplied by its column's `Scale`, or copied unchanged when the scaler does not scale: a zero stays a zero, so nothing absent becomes stored.

**Parameters** — `samples` is the transformed matrix, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, back on the input scale.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

The sparse overload throws `ArgumentNullException` when `samples` is `null`, `ArgumentException` when it holds no row or its column count is not `FeatureCount` or it stores a non-finite value, and `InvalidOperationException` when the scaler centres — fit it with `WithCentring = false`, since subtracting a centre would make every absent zero a stored value.

**Example** — the round trip returns what went in.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 2.0, 3.0, 4.0, 5000.0];

RobustScaler scaler = RobustScaler.Fit(samples, featureCount: 1);

double back = scaler.InverseTransform(scaler.Transform(samples))[4];  // => 5000
```

**Remarks** — multiplies then adds, the reverse of the transform's order. Exact up to floating-point
rounding, except for a feature whose range was floored to 1: that step threw the spread away rather
than recording it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScaler.Transform`](robustscaler-transform.md), [`RobustScaler`](robustscaler.md).
