# MaxAbsScaler.InverseTransform

Undoes [`Transform`](maxabsscaler-transform.md), returning values on the original scale.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the scaled matrix, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, back on the input scale.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — the inverse leaves the unit interval that clipping bounded.

```csharp
using Lodestar.Preprocessing;

MaxAbsScaler scaler = MaxAbsScaler.Fit([0.0, 10.0], 1, new MaxAbsScalerOptions { Clip = true });

double back = scaler.InverseTransform([5.0])[0];  // => 50
```

**Remarks** — **it never clips, even when the scaler does**, for the reason
[`MinMaxScaler.InverseTransform`](minmaxscaler-inversetransform.md) gives: a clipped value has lost
what it was.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScaler.Transform`](maxabsscaler-transform.md), [`MaxAbsScaler`](maxabsscaler.md).
