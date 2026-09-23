# QuantileTransformerOptions

How many quantiles [`QuantileTransformer`](quantiletransformer.md) fits, and what it maps onto.

<!-- docs-declaration -->

```csharp
public sealed record QuantileTransformerOptions
```

**Properties** — `QuantileCount` is how many quantiles to fit; scikit-learn's `n_quantiles`,
default 1000, clamped to the number of rows on both sides. `Output` is what the values are mapped
onto; `output_distribution`, default [`QuantileOutput.Uniform`](quantileoutput.md).

**Example** — a coarse fit, which is a piecewise-linear approximation of the distribution.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer coarse = QuantileTransformer.Fit(
    skew, 1, new QuantileTransformerOptions { QuantileCount = 5 });

double approximate = coarse.Transform([8.0])[0];   // => 0.53
```

**Remarks — `subsample` is deliberately absent.** At its own default of 10,000 the reference
draws a subsample from numpy's generator, and two seeds were measured moving a quantile by `0.19`
on twenty thousand rows. This transformer always reads every row, which is the reference's
`subsample=None` — a narrower parameter set that is provable, rather than a wider one that is not.

`random_state` goes with it: it exists there only to seed that draw. `copy` is absent too, as
everywhere in this package — a span goes in and a new array comes out, so there is nothing to
copy in place.

Being a `record`, two option sets with the same two values are equal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformer.Fit`](quantiletransformer-fit.md),
[`QuantileOutput`](quantileoutput.md).
