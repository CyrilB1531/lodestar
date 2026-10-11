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
`ArgumentException` when `samples` holds no row, a partial one, or a non-finite value; or when the
family is [`PowerMethod.BoxCox`](powermethod.md) and a value is not strictly positive, a feature is
constant — "Data must not be constant.", as `scipy.stats.boxcox` refuses it — or the exponent search
finds no bracket; or when the family is Yeo-Johnson and a mixed-sign column past about `1e145`
inverts the search's bounds — "The lower bound exceeds the upper bound.", as `fminbound` refuses
them; or, standardised, when the fitted power overflows a value — "Input X contains
infinity or a value too large for dtype('float64').", the reference's `StandardScaler` refusal
([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)).

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

**Remarks — the exponent is searched, not solved.** Yeo-Johnson searches a range derived from the
column, by `scipy.optimize.fminbound`'s bounded Brent; Box-Cox walks downhill from `(-2, 2)` until
it holds a bracket and runs `scipy.optimize.brent` unbounded, so its exponent can leave that
interval — `11.13` for a column clustered near `10` with one low value — and is pulled back through
Lambert's W only where the transform would pass `double.MaxValue / 10000`, as
`scipy.stats.boxcox_normmax` does ([#1227](https://github.com/CyrilB1531/lodestar/issues/1227)).
Both stop at `1.48e-8` on the argument, scipy's own tolerance. A constant Yeo-Johnson feature keeps
the exponent `1`, the identity, as the reference's `_is_constant_feature` decides
([#1253](https://github.com/CyrilB1531/lodestar/issues/1253)); the standardisation is a
`StandardScaler`'s, the reference's own
([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)). There is no closed form, and the
objective is flat enough near its optimum that a tighter stop would buy nothing; see
[`PowerTransformer`](powertransformer.md) for the curvature arithmetic behind the `1e-5` conformance
tolerance.

**A negative exponent is normal, not a symptom.** It means the column is right-skewed enough that
a reciprocal-like power straightens it, which is what the income column above is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformer.Transform`](powertransformer-transform.md),
[`PowerTransformerOptions`](powertransformeroptions.md), [`PowerMethod`](powermethod.md).
