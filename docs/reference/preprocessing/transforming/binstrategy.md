# BinStrategy

Where [`KBinsDiscretizer`](kbinsdiscretizer.md) places its bin edges.

<!-- docs-declaration -->

```csharp
public enum BinStrategy
```

**Values** — `Uniform` cuts equal widths between the feature's smallest and largest value;
scikit-learn's `'uniform'`. `Quantile` cuts equal counts, read off the feature's percentiles;
`'quantile'`, and the default. `KMeans` cuts midway between one-dimensional k-means centres;
`'kmeans'`.

**Example** — the same ten ages, three ways.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

var options = new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal };

KBinsDiscretizer byCount = KBinsDiscretizer.Fit(ages, 1, options);
KBinsDiscretizer byWidth = KBinsDiscretizer.Fit(
    ages, 1, options with { Strategy = BinStrategy.Uniform });

string equalCounts = string.Join(",", byCount.BinEdges[0]);   // => 19,25,41,61,74

double firstWidthEdge = byWidth.BinEdges[0][1];               // => 32.75
double secondWidthEdge = byWidth.BinEdges[0][2];              // => 46.5
```

**Remarks — which one depends on what the bin is for.** `Quantile` guarantees every bin is
equally populated, which is what a model wants and what makes the bins comparable across
features; `Uniform` guarantees every bin covers the same range, which is what a reader wants when
the bin will be printed as "30 to 45". On a skewed column the two disagree sharply: here the
first uniform bin holds four of the ten ages and the third holds one.

**`KMeans` is the only strategy that costs an iteration.** It runs Lloyd's algorithm over the
single column — `Lodestar.Cluster`'s, which is the package edge this member added — starting from
the uniform bin midpoints, then puts each edge midway between two consecutive centres. Starting
from the midpoints rather than a random draw is what makes it deterministic, and the reference
does the same.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md),
[`KBinsDiscretizer.Fit`](kbinsdiscretizer-fit.md).
