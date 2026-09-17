# AgglomerativeClustering.FitToThreshold

Clusters by cutting the tree at a height rather than at a count.

<!-- docs-declaration -->

```csharp
public static AgglomerativeClustering FitToThreshold(ReadOnlySpan<double> samples, int featureCount, double distanceThreshold, Linkage linkage = Linkage.Ward)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `distanceThreshold` is the height at and above
which a merge is not made, scikit-learn's `distance_threshold`. `linkage` is how the distance
between two clusters is measured; Ward by default.

**Returns** — a fitted `AgglomerativeClustering`, whose `ClusterCount` says how many clusters the
height left.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is not positive, when
`distanceThreshold` is negative, infinite or not a number, or when `linkage` is not a defined value.
`ArgumentException` when `samples` holds fewer than two rows, a partial one, or a `NaN` or infinite
value.

**Example** — three points whose gaps are one and two, cut exactly at a gap and just above it.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 1.0, 3.0];

// Exactly at the first merge's height: that merge is not made.
AgglomerativeClustering at = AgglomerativeClustering.FitToThreshold(
    samples, featureCount: 1, distanceThreshold: 1.0, Linkage.Single);
int atCount = at.ClusterCount;       // => 3

// Just above it, it is.
AgglomerativeClustering above = AgglomerativeClustering.FitToThreshold(
    samples, featureCount: 1, distanceThreshold: 1.001, Linkage.Single);
int aboveCount = above.ClusterCount;   // => 2
```

**Remarks** — **the threshold is exclusive.** The cluster count is one more than the number of
merges whose height is at or above it, so a merge exactly at the threshold is not made — the
example's first pair. Zero is allowed and leaves every sample its own cluster; infinity is refused,
because the reference's range is `[0, inf)`, open at the top.

**This is the same tree [`Fit`](agglomerativeclustering-fit.md) builds**, cut by height rather than
by count. A threshold just above the height a count leaves gives that count's labels exactly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AgglomerativeClustering`](agglomerativeclustering.md),
[`AgglomerativeClustering.Fit`](agglomerativeclustering-fit.md), [`Linkage`](linkage.md).
