# PowerMethod

Which power family [`PowerTransformer`](powertransformer.md) fits.

<!-- docs-declaration -->

```csharp
public enum PowerMethod
```

**Values** — `YeoJohnson` is Yeo and Johnson's family, which admits zero and negative values;
scikit-learn's `'yeo-johnson'`, and the default. `BoxCox` is Box and Cox's, which needs strictly
positive values; `'box-cox'`.

**Example** — the two exponents on the same positive column.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

double yeoJohnson = Math.Round(PowerTransformer.Fit(income, 1).Lambdas[0], 4);   // => -0.8072

PowerTransformer fitted = PowerTransformer.Fit(
    income, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox });

double boxCox = Math.Round(fitted.Lambdas[0], 4);                               // => -0.7802
```

**Remarks — the default is Yeo-Johnson because it always applies.** Box-Cox refuses a column
holding a zero or a negative value, and refusing is the right answer there: its family is
`(xᶫ − 1) / λ`, which is not defined for them. Yeo-Johnson is built to extend it, applying the
Box-Cox form to the positive side and a reflected one to the negative, so a column of temperature
anomalies or profit-and-loss figures can be transformed at all.

**On a strictly positive column the two are different fits, not the same fit twice.** They agree
on the direction and disagree on the number, as above, because the likelihoods being maximised
are different functions — Yeo-Johnson's shifts each positive value by one before raising it.
Prefer Box-Cox when the column is positive by construction and the exponent will be reported,
since it is the family a reader will expect; prefer the default otherwise.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformerOptions`](powertransformeroptions.md),
[`PowerTransformer.Fit`](powertransformer-fit.md).
