# PowerTransformerOptions

Which power family [`PowerTransformer`](powertransformer.md) fits, and whether it standardises
after.

<!-- docs-declaration -->

```csharp
public sealed record PowerTransformerOptions
```

**Properties** — `Method` is which family; scikit-learn's `method`, default
[`PowerMethod.YeoJohnson`](powermethod.md). `Standardize` centres and scales the transformed
column; `standardize`, default `true`.

**Example** — turning the standardisation off leaves the raw power.

```csharp
using Lodestar.Preprocessing;

double[] income = [22.0, 25.0, 28.0, 31.0, 35.0, 42.0, 55.0, 78.0, 120.0, 260.0];

var options = new PowerTransformerOptions { Standardize = false };

PowerTransformer raw = PowerTransformer.Fit(income, 1, options);

double[] ends = raw.Transform([22.0, 260.0]);

double smallest = Math.Round(ends[0], 4);   // => 1.1403
double largest = Math.Round(ends[1], 4);    // => 1.225
```

**Remarks — `Standardize` changes the output, never the exponent.** The likelihood is maximised
on the powered column before anything is centred, so both settings fit the same `Lambdas`; the
switch decides only whether the mean and the population deviation of that column are divided out
afterwards.

**Leave it on unless something downstream needs the raw scale.** The powered values above span
`1.14` to `1.23`, a range that will disappear beside any unscaled neighbour in the same matrix —
which is the reason the reference defaults it on.

`copy` is absent, as everywhere in this package.

Being a `record`, two option sets with the same two values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PowerTransformer.Fit`](powertransformer-fit.md), [`PowerMethod`](powermethod.md).
