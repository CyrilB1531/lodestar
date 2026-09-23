# QuantileTransformer.Transform

Maps each value onto its rank.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to transform, row-major,
[`FeatureCount`](quantiletransformer.md) values per row.

**Returns** — a new matrix of the same shape: each value's position in the fitted distribution,
on `[0, 1]` under [`QuantileOutput.Uniform`](quantileoutput.md), or the standard normal quantile
of that position under `Normal`.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or a non-finite value.

**Example** — the same column, uniform and normal.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer uniform = QuantileTransformer.Fit(skew, 1);
QuantileTransformer normal = QuantileTransformer.Fit(
    skew, 1, new QuantileTransformerOptions { Output = QuantileOutput.Normal });

double middleUniform = uniform.Transform([8.0])[0];   // => 0.5555555555555556
double middleNormal = normal.Transform([8.0])[0];     // => 0.13971029888186234
```

**Remarks — a value outside the fitted range is clamped.** Anything below the smallest fitted
value maps to 0 and anything above the largest to 1, so a later row cannot leave the interval.
Under `Normal` those two ends would be infinite, so they are clipped at the quantile of `1e-7`
one ulp in — the reference's own threshold and the reason its output stops near ±5.2 rather than
running to infinity.

**A tie maps to the average of its two interpolations.** The mapping is computed forwards and
backwards through the fitted quantiles and averaged, which is what puts a repeated value in the
middle of the range its copies occupy instead of at one end of it. The reference does the same,
and it is why the first two entries of a column starting `1, 1` both map to 0 rather than to 0
and 0.111.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformer.InverseTransform`](quantiletransformer-inversetransform.md),
[`QuantileOutput`](quantileoutput.md).
