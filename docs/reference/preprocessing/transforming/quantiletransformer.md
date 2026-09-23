# QuantileTransformer

Maps each feature onto a uniform or normal distribution by its own ranks, at
`sklearn.preprocessing.QuantileTransformer` parity.

<!-- docs-declaration -->

```csharp
public sealed class QuantileTransformer
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on. `References`
are the quantile levels read, evenly spaced over `[0, 1]` — the reference's `references_`.
`Quantiles` are each feature's values at those levels, one row per feature — `quantiles_`.

**Example** — a column whose largest value is three times the next.

```csharp
using Lodestar.Preprocessing;

double[] skew = [1.0, 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 100.0];

QuantileTransformer transformer = QuantileTransformer.Fit(skew, featureCount: 1);

double[] flat = transformer.Transform(skew);

double outlier = flat[9];   // => 1
double middle = flat[5];    // => 0.5555555555555556
```

**Remarks — this is the blunt instrument, and that is a recommendation as often as a warning.**
The scalers move and stretch a feature; this one reshapes it. What survives is the order of the
values and nothing else, so an outlier stops being far away and a skew stops being a skew — which
is exactly what a distance-based model wants from a column like the one above, where 100 would
otherwise dominate every distance on its own.

**The cost is that two values a thousand apart can come out adjacent**, and no inverse recovers
what the ranks discarded. [`PowerTransformer`](powertransformer.md) is the gentle alternative: it
keeps the values and looks for a single exponent, so a small difference stays small.

**`subsample` is deliberately absent.** At the reference's own default of 10,000 rows it draws a
subsample from numpy's generator, and two seeds were measured moving a quantile by `0.19` over
twenty thousand rows — a default that cannot be frozen into a corpus. This transformer always
reads every row, which is the reference's `subsample=None`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QuantileTransformerOptions`](quantiletransformeroptions.md),
[`QuantileOutput`](quantileoutput.md), [`PowerTransformer`](powertransformer.md), the
[feature transforming index](../transforming.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`QuantileTransformer.Fit`](quantiletransformer-fit.md) | Fits the quantiles of every feature. |
| [`QuantileTransformer.InverseTransform`](quantiletransformer-inversetransform.md) | Maps back onto the fitted values. |
| [`QuantileTransformer.Transform`](quantiletransformer-transform.md) | Maps each value onto its rank. |
