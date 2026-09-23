# KBinsDiscretizer

Cuts each feature into bins, at `sklearn.preprocessing.KBinsDiscretizer` parity.

<!-- docs-declaration -->

```csharp
public sealed class KBinsDiscretizer
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on. `BinEdges` is
each feature's edges, ascending, the reference's `bin_edges_`. `BinCounts` is how many bins each
feature ended with, the reference's `n_bins_`, **which can be fewer than asked for**.
`OutputFeatureCount` is how wide a transformed row is: one column per feature under
[`BinEncoding.Ordinal`](binencoding.md), one per bin under `OneHot`.

**Example** — ten ages into four bins of equal count.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

var options = new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal };

KBinsDiscretizer bins = KBinsDiscretizer.Fit(ages, featureCount: 1, options);

string edges = string.Join(",", bins.BinEdges[0]);        // => 19,25,41,61,74
string codes = string.Join(",", bins.Transform(ages));    // => 0,0,1,1,1,2,2,3,3,3
```

**Remarks — this is the transformer for a model that reads order but not distance.** A tree, a
naive Bayes over categories, or a human reading a report all want "the third quartile of age",
not 61.4; the discretizer is what turns one into the other, and
[`InverseTransform`](kbinsdiscretizer-inversetransform.md) is what reads it back — as the bin's
centre, since the value itself is gone.

**It is not ML.NET's `NormalizeBinning`.** That one also cuts a feature into bins, but emits a
position in `[0, 1]` rather than the bin: the binning is a rescaling there and a categorisation
here, and only the second answers "which group is this row in".

**A feature can come back with fewer bins than asked for.** Two edges within `1e-8` of each other
are one edge — the reference's own threshold — which happens on a column that is nearly constant,
or whose quantiles repeat because a value dominates it. `BinCounts` is where that shows, and it
is the number to read before indexing into a one-hot row. The gaps compared are the **successive**
ones of the edges as they were fitted, not the distance to the last edge kept: on a run of narrow
gaps the two readings give different bins, and the first is the reference's
([#1128](https://github.com/CyrilB1531/lodestar/issues/1128)).

**One bin is the floor, which is a deliberate divergence.** Where the removal leaves a single edge
the reference reports `n_bins_ = 0` and then breaks on itself — its `inverse_transform` raises
`IndexError` on such a fit, and so does its `fit` under one-hot encoding. This keeps one bin
spanning the feature's range, which transforms, inverts and encodes.
[Decision 0007](../../../decisions/0007-the-deliberate-divergences.md) admits a divergence exactly
where reproducing the reference would reproduce a defect; the
[Python equivalence table](../../../equivalence.md) carries the measurement.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md),
[`BinStrategy`](binstrategy.md), [`QuantileTransformer`](quantiletransformer.md), the
[feature transforming index](../transforming.md), the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`KBinsDiscretizer.Fit`](kbinsdiscretizer-fit.md) | Fits bin edges on a row-major matrix. |
| [`KBinsDiscretizer.InverseTransform`](kbinsdiscretizer-inversetransform.md) | Reads each bin back as its centre. |
| [`KBinsDiscretizer.Transform`](kbinsdiscretizer-transform.md) | Reads each value as the bin it falls in. |
