# PowerTransformer.Transform

Raises each feature to its fitted power.

<!-- docs-declaration -->

```csharp
public double[] Transform(ReadOnlySpan<double> samples)
```

**Parameters** — `samples` is the matrix to transform, row-major,
[`FeatureCount`](powertransformer.md) values per row.

**Returns** — a new matrix of the same shape, each feature raised to its fitted exponent and, when
[`PowerTransformerOptions.Standardize`](powertransformeroptions.md) is on, centred and scaled
afterwards.

**Exceptions** — `ArgumentException` when `samples` holds a partial row or a non-finite value; when
the family is [`PowerMethod.BoxCox`](powermethod.md) and a value is not strictly positive; or,
standardised, when the power overflows a value, as the reference's `StandardScaler` refuses the
infinity — unstandardised, the infinity is returned ([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)).

**Example** — with and without the standardisation step.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

PowerTransformer standardised = PowerTransformer.Fit(income, 1);
PowerTransformer raw = PowerTransformer.Fit(
    income, 1, new PowerTransformerOptions { Standardize = false });

double centred = Math.Round(standardised.Transform([22.0])[0], 4);   // => -1.4543
double powered = Math.Round(raw.Transform([22.0])[0], 4);            // => 1.1403
```

**Remarks — a value outside the fitted range is transformed, not clamped.** The exponent is a
formula, so a later row larger than anything fitted comes out larger than anything fitted, which
is the behaviour [`QuantileTransformer`](quantiletransformer.md) cannot offer. What the fit
decided was the exponent and, under standardisation, the mean and deviation of the transformed
column; none of the three bounds a later value.

**The standardisation is a `StandardScaler`'s, as the reference's is.** It is fitted after the power,
on the fitted rows — so a fitted column comes out with mean 0 and deviation 1, and a later row does
not necessarily — except on a constant Yeo-Johnson feature, which the reference leaves untransformed
at fit and so standardises as it was ([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformer.InverseTransform`](powertransformer-inversetransform.md),
[`PowerTransformerOptions`](powertransformeroptions.md).
