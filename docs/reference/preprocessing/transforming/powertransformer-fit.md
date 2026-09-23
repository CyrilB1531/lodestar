# PowerTransformer.Fit

Fits one exponent per feature by maximum likelihood.

<!-- docs-declaration -->

```csharp
public static PowerTransformer Fit(ReadOnlySpan<double> samples, int featureCount, PowerTransformerOptions options = null)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row; the span is
read, never modified. `options` chooses the family and whether to standardise after; `null` takes
the reference's defaults, Yeo-Johnson with standardisation.

**Returns** — a fitted [`PowerTransformer`](powertransformer.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive.
`ArgumentException` when `samples` holds no row, a partial one, or a non-finite value; or when
the family is [`PowerMethod.BoxCox`](powermethod.md) and a value is not strictly positive.

**Example** — the two families on the same positive column, which agree on the shape and not on
the number.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

PowerTransformer yeoJohnson = PowerTransformer.Fit(income, 1);

PowerTransformer boxCox = PowerTransformer.Fit(
    income, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox });

double byDefault = Math.Round(yeoJohnson.Lambdas[0], 4);   // => -0.8072
double byBoxCox = Math.Round(boxCox.Lambdas[0], 4);        // => -0.7802
```

**Remarks — the exponent is searched, not solved.** Box-Cox searches `[-2, 2]` and Yeo-Johnson a
range derived from the column, both by Brent's method on the negative log-likelihood, stopping at
`1.48e-8` on the argument — `scipy.optimize.fminbound`'s own tolerance, which is what the
reference calls. There is no closed form, and the objective is flat enough near its optimum that
a tighter stop would buy nothing; see [`PowerTransformer`](powertransformer.md) for the curvature
arithmetic behind the `1e-5` conformance tolerance.

**A negative exponent is normal, not a symptom.** It means the column is right-skewed enough that
a reciprocal-like power straightens it, which is what the income column above is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformer.Transform`](powertransformer-transform.md),
[`PowerTransformerOptions`](powertransformeroptions.md), [`PowerMethod`](powermethod.md).
