# MaxAbsScaler.Transform

Divides a row-major sample matrix by the fitted maxima.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

<!-- docs-declaration -->

```csharp
public CsrMatrix Transform(CsrMatrix samples)
```

The second overload takes a [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md) and returns a new one storing the same positions, each value divided by its column's `Scale`: a zero stays a zero, so nothing absent becomes stored.

**Parameters** — `samples` is the matrix to scale, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length, inside `[−1, 1]` for any value the fit saw.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

The sparse overload throws `ArgumentNullException` when `samples` is `null`, `ArgumentException` when it holds no row or its column count is not `FeatureCount` or it stores a non-finite value.

**Example** — a value beyond the fitted maximum leaves the unit interval, unless clipping is asked for.

```csharp
using Lodestar.Preprocessing;

double[] seen = [0.0, 10.0];

MaxAbsScaler plain = MaxAbsScaler.Fit(seen, featureCount: 1);
MaxAbsScaler clipped = MaxAbsScaler.Fit(seen, 1, new MaxAbsScalerOptions { Clip = true });

double outside = plain.Transform([50.0])[0];    // => 5
double bounded = clipped.Transform([50.0])[0];  // => 1
double below = clipped.Transform([-50.0])[0];   // => -1
```

**Example** — the sparse overload, on a matrix whose second column stores nothing.

```csharp
using Lodestar.Abstractions;
using Lodestar.Preprocessing;

// Three rows, two columns: 2 and -4 in the first column, nothing in the second.
var matrix = new CsrMatrix(3, 2, [2.0, -4.0], [0, 0], [0, 1, 1, 2]);
MaxAbsScaler scaler = MaxAbsScaler.Fit(matrix);

CsrMatrix scaled = scaler.Transform(matrix);

int stored = scaled.NonZeroCount;   // => 2
double second = scaled.Values[1];   // => -1
```

**Remarks** — never writes to the input. Clipping bounds this direction to `[−1, 1]` — the fitted
range, which is fixed here rather than chosen as it is for
[`MinMaxScaler`](minmaxscaler.md) — and leaves
[`InverseTransform`](maxabsscaler-inversetransform.md) alone.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MaxAbsScalerOptions`](maxabsscaleroptions.md), [`MaxAbsScaler.InverseTransform`](maxabsscaler-inversetransform.md).
