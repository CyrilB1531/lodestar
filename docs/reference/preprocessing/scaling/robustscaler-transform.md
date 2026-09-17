# RobustScaler.Transform

Centres and scales a row-major sample matrix with the fitted statistics.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

<!-- docs-declaration -->

```csharp
public CsrMatrix Transform(CsrMatrix samples)
```

The second overload takes a [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md) and returns a new one storing the same positions, each value divided by its column's `Scale`, or copied unchanged when the scaler does not scale: a zero stays a zero, so nothing absent becomes stored.

**Parameters** — `samples` is the matrix to transform, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

The sparse overload throws `ArgumentNullException` when `samples` is `null`, `ArgumentException` when its column count is not `FeatureCount` or it stores a non-finite value, and `InvalidOperationException` when the scaler centres — fit it with `WithCentring = false`, since subtracting a centre would make every absent zero a stored value.

**Example** — subtract the median, then divide by the interquartile range.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, 2.0, 3.0, 4.0, 5000.0];

RobustScaler scaler = RobustScaler.Fit(samples, featureCount: 1);

double[] robust = scaler.Transform(samples);

double smallest = robust[0];  // => -1
double middle = robust[2];    // => 0
```

**Example** — the sparse overload, on a matrix whose second column stores nothing.

```csharp
using Lodestar.Abstractions;
using Lodestar.Preprocessing;

// Three rows, two columns: 2 and -4 in the first column, nothing in the second.
var matrix = new CsrMatrix(3, 2, [2.0, -4.0], [0, 0], [0, 1, 1, 2]);
RobustScaler scaler = RobustScaler.Fit(matrix);

CsrMatrix scaled = scaler.Transform(matrix);

int stored = scaled.NonZeroCount;   // => 2
int[] columns = scaled.ColumnIndices;   // same positions as the input
```

**Remarks** — **subtract then divide, in that order**, and the inverse multiplies then adds. Both
steps are optional, so the order is part of the contract rather than an implementation detail: with
centring off, nothing is subtracted and the division is applied to the raw value.

Never writes to the input.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScaler.InverseTransform`](robustscaler-inversetransform.md), [`RobustScalerOptions`](robustscaleroptions.md).
