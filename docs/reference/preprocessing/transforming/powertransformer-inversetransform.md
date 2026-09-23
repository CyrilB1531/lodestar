# PowerTransformer.InverseTransform

Undoes [`Transform`](powertransformer-transform.md).

<!-- docs-declaration -->

```csharp
public double[] InverseTransform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is a matrix this transformer produced, row-major,
[`FeatureCount`](powertransformer.md) values per row.

**Returns** — a new matrix of the same shape, each value read back through the standardisation and
then the power.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or a non-finite value.

**Example** — a value through and back.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

PowerTransformer transformer = PowerTransformer.Fit(income, 1);

double[] there = transformer.Transform([35.0]);

double back = Math.Round(transformer.InverseTransform(there)[0], 4);   // => 35
```

**Remarks — unlike the two neighbours in this section, this round trip is genuine.** The power is
a monotone function with an exact inverse, so nothing is discarded on the way out and the value
returns to within rounding — which is what makes this transformer usable on a target variable, a
model fitted on the transformed scale and its predictions read back on the original one.

**The one place it does not is where the power cannot be undone.** Under
[`PowerMethod.BoxCox`](powermethod.md) the transformed scale is bounded on one side — below by
`-1/λ` for a positive exponent, above by it for a negative one — because no positive value maps
past that asymptote. A value outside comes back as `NaN`, which is what the reference returns
there too, rather than a number the family never produced.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformer.Transform`](powertransformer-transform.md),
[`PowerTransformer`](powertransformer.md).
