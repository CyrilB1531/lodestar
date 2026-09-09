# StandardScaler.InverseTransform

Undoes `Transform`, returning values on the original scale.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the standardised matrix, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, back on the input scale.

**Exceptions** — `ArgumentException` when `samples` holds no row, or a partial one.

**Example** — there and back.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];
StandardScaler scaler = StandardScaler.Fit(samples, featureCount: 2);

double[] restored = scaler.InverseTransform(scaler.Transform(samples));

double first = restored[0];   // => 1
double second = restored[1];  // => 10
```

**Remarks** — exact only up to floating-point rounding: the round trip multiplies by a scale it
previously divided by, and neither operation is exact in binary.

**A feature whose `Scale` was forced to 1 does not come back to its own spread.** That step threw
the spread away rather than recording it, which is the point of forcing it — see
[`StandardScaler.Fit`](standardscaler-fit.md). Such a feature still round-trips to its original
*values*, because its values were all but identical to begin with; what is lost is the ability to
recover a spread that was never distinguishable from zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`StandardScaler.Transform`](standardscaler-transform.md),
[`StandardScaler`](standardscaler.md).
