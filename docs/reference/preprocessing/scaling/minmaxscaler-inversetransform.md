# MinMaxScaler.InverseTransform

Undoes [`Transform`](minmaxscaler-transform.md), returning values on the original scale.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the mapped matrix, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, back on the input scale.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — the inverse walks straight back out of the range.

```csharp
using Lodestar.Preprocessing;

MinMaxScaler scaler = MinMaxScaler.Fit([0.0, 10.0], 1, new MinMaxScalerOptions { Clip = true });

double back = scaler.InverseTransform([2.0])[0];  // => 20
```

**Remarks** — **it never clips, even when the scaler does.** A value clipped on the way in has lost
what it was, and clipping again on the way out would hide that rather than undo it; the reference
draws the line in the same place.

A feature whose `Scale` was floored to 1 does not round-trip to its own spread either — that step
threw the spread away, which is the point of forcing it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScaler.Transform`](minmaxscaler-transform.md), [`MinMaxScaler`](minmaxscaler.md).
