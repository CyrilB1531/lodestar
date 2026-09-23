# QuantileTransformer.InverseTransform

Maps back onto the fitted values.

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is a matrix this transformer produced, row-major,
[`FeatureCount`](quantiletransformer.md) values per row.

**Returns** — a new matrix of the same shape, each value read back through the fitted quantiles.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or a non-finite value.

**Example** — the middle of the distribution, in the units it came from.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer transformer = QuantileTransformer.Fit(skew, 1);

double median = transformer.InverseTransform([0.5])[0];   // => 6.5
```

**Remarks — the round trip is not exact, and the loss is in the forward direction.** Transforming
and inverting a fitted value returns it, because it is one of the knots; transforming and
inverting anything else returns the point the piecewise-linear map lands on, which is only as
close as the quantile count allows. Two distinct values that shared a rank come back as the same
number.

Under [`QuantileOutput.Normal`](quantileoutput.md) the input is read as a normal quantile first,
with the same `1e-7` threshold pinning the two ends, so an inverse of ±5.2 or beyond returns the
fitted extreme rather than running off the end of the knots.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformer.Transform`](quantiletransformer-transform.md),
[`QuantileTransformer`](quantiletransformer.md).
