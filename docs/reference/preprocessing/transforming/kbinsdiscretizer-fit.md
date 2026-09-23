# KBinsDiscretizer.Fit

Fits bin edges on a row-major matrix.

<!-- docs-declaration -->

```csharp
public static KBinsDiscretizer Fit(ReadOnlySpan<double> samples, int featureCount, KBinsDiscretizerOptions options = null)
```

**Parameters** — `samples` is the matrix, row-major, `featureCount` values per row; the span is
read, never modified. `options` chooses the bin count, the strategy, the encoding and the
percentile convention; `null` takes the reference's defaults, which are five quantile bins encoded
one-hot.

**Returns** — a fitted [`KBinsDiscretizer`](kbinsdiscretizer.md).

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive or the bin
count is below two. `ArgumentException` when `samples` holds no row, a partial one, or a
non-finite value.

**Example** — the same column under two strategies, which do not agree.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

KBinsDiscretizer byCount = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal });

KBinsDiscretizer byWidth = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions
    {
        BinCount = 4,
        Strategy = BinStrategy.Uniform,
        Encoding = BinEncoding.Ordinal,
    });

string equalCounts = string.Join(",", byCount.Transform(ages));  // => 0,0,1,1,1,2,2,3,3,3
string equalWidths = string.Join(",", byWidth.Transform(ages));  // => 0,0,0,0,1,1,2,3,3,3
```

**Remarks — the strategy is the decision, not the bin count.**
[`BinStrategy.Quantile`](binstrategy.md) gives every bin the same number of rows and lets the
widths vary; `Uniform` gives every bin the same width and lets the counts vary, which on a skewed
column puts most of the data in the first bin; `KMeans` lets the data decide both, at the cost of
an iterative fit.

**The fit reads every row of every feature and sorts each column**, so it costs
`features × n log n`. There is no subsample here, for the reason
[`QuantileTransformer`](quantiletransformer.md) has none either.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizer.Transform`](kbinsdiscretizer-transform.md),
[`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md), the
[Python equivalence table](../../../equivalence.md).
