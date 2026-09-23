# KBinsDiscretizerOptions

How [`KBinsDiscretizer`](kbinsdiscretizer.md) places its bins, and what it emits.

<!-- docs-declaration -->

```csharp
public sealed record KBinsDiscretizerOptions
```

**Properties** — `BinCount` is how many bins each feature is cut into; scikit-learn's `n_bins`,
default 5. `Strategy` is where the edges go; `strategy`, default
[`BinStrategy.Quantile`](binstrategy.md). `Encoding` is what a transformed row carries; `encode`,
default [`BinEncoding.OneHot`](binencoding.md). `QuantileMethod` is which percentile convention a
quantile fit reads; `quantile_method`, default
[`QuantileMethod.AveragedInvertedCdf`](quantilemethod.md).

**Example** — the percentile convention changes the edges, not merely how they are reached.

```csharp
using Lodestar.Preprocessing;

double[] six = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];

var ordinal = new KBinsDiscretizerOptions { BinCount = 3, Encoding = BinEncoding.Ordinal };

KBinsDiscretizer averaged = KBinsDiscretizer.Fit(six, 1, ordinal);
KBinsDiscretizer linear = KBinsDiscretizer.Fit(
    six, 1, ordinal with { QuantileMethod = QuantileMethod.Linear });

double byDefault = averaged.BinEdges[0][1];   // => 2.5
double byLinear = linear.BinEdges[0][1];      // => 2.666666666666667
```

**Remarks — `AveragedInvertedCdf` is the default because it is the reference's**, since
scikit-learn 1.9 deprecated leaving the convention unstated. `Linear` is numpy's own default and
what the reference used before; a caller comparing against an older pipeline wants it, and a
caller starting here does not.

**`subsample` is absent**, as it is on [`QuantileTransformer`](quantiletransformer.md), and for
the same reason: the reference draws it from numpy's generator, so an unseeded default cannot be
frozen into a corpus.

`random_state` is absent with it — it exists there only to seed that subsample and the `kmeans`
initialisation, and [`BinStrategy.KMeans`](binstrategy.md) here starts from the uniform bin
midpoints, which is deterministic.

Being a `record`, two option sets with the same four values are equal, and `with` copies one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizer.Fit`](kbinsdiscretizer-fit.md),
[`BinStrategy`](binstrategy.md), [`QuantileMethod`](quantilemethod.md).
