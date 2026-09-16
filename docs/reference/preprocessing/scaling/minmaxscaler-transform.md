# MinMaxScaler.Transform

Maps a row-major sample matrix onto the fitted range.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to map, row-major, with `FeatureCount` values per row.

**Returns** — a new array of the same length.

**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.

**Example** — a value the fit never saw lands outside the range, unless clipping is asked for.

```csharp
using Lodestar.Preprocessing;

double[] seen = [0.0, 10.0];

MinMaxScaler plain = MinMaxScaler.Fit(seen, featureCount: 1);
MinMaxScaler clipped = MinMaxScaler.Fit(seen, 1, new MinMaxScalerOptions { Clip = true });

double outside = plain.Transform([20.0])[0];    // => 2
double bounded = clipped.Transform([20.0])[0];  // => 1
```

**Remarks** — never writes to the input; scikit-learn's `copy=False` has no counterpart, since a
span the caller owns is not this package's to overwrite.

**Clipping belongs to this direction only.** [`InverseTransform`](minmaxscaler-inversetransform.md)
does not clip, which is the reference's asymmetry and not an oversight — see that page.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinMaxScalerOptions`](minmaxscaleroptions.md), [`MinMaxScaler.InverseTransform`](minmaxscaler-inversetransform.md).
