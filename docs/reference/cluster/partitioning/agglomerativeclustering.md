# AgglomerativeClustering

Builds a merge tree from the bottom up, joining the two nearest clusters at every step, and cuts it.

<!-- docs-declaration -->

```csharp
public sealed class AgglomerativeClustering
```

**Properties** — `FeatureCount` is how many values each fitted row carried, and `Linkage` how the
distance between two clusters was measured. `ClusterCount` is how many clusters the tree was cut
into (`n_clusters_`), and `Labels` the cluster each sample belongs to (`labels_`). `Children` is
every merge as two node ids, row-major (`children_`), and `Distances` the height of each merge
(`distances_`).

**Example** — five points on a line, cut into two clusters.

```csharp
using Lodestar.Cluster;

// One feature per row: two pairs a unit apart, and one point far from both.
double[] samples = [0.0, 1.0, 5.0, 6.0, 20.0];

AgglomerativeClustering model = AgglomerativeClustering.Fit(samples, featureCount: 1, clusterCount: 2);

int clusters = model.ClusterCount;   // => 2
int far = model.Labels[4];           // => 1
int merges = model.Children.Count / 2;   // => 4
```

**Remarks** — **the whole tree is built, whatever the cut.** `Children` and `Distances` always hold
all `n − 1` merges, so one fit answers every cluster count its tree can be cut at — the reference
builds the full tree too when there is no connectivity.

**Row `k` of `Children` creates node `n + k`**, at indices `2k` and `2k + 1`; an id below `n` is a
sample. Ward, complete and average write each pair smaller id first, and single linkage writes
them in the order its spanning tree found them — `[7, 4]` where the other three write `[4, 7]` for
the same merge. Both are the reference's own layouts.

**Labels are numbered as the reference numbers them, which is not by first appearance.** A
cluster's label is its position in the heap the cut walks from the root, so
`0, 1, 5, 6, 20` cut into three under complete linkage labels as `2, 2, 0, 0, 1`. Compare two
clusterings with an adjusted Rand index rather than label by label.

**Ties are part of the answer.** Where two merges are equally near, the reference's order decides
the tree, and it is reproduced exactly — including the floating-point order of the distance update,
without which one merge in several hundred comes out differently on integer data.

**Memory is quadratic for three linkages and linear for one.** Ward, complete and average hold the
distances between every pair of samples, `n(n − 1)/2` doubles — 400 MB at 10,000 samples. Single
linkage computes each distance when it needs it and holds none.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AgglomerativeClustering.Fit`](agglomerativeclustering-fit.md),
[`AgglomerativeClustering.FitToThreshold`](agglomerativeclustering-fittothreshold.md),
[`Linkage`](linkage.md), the [partitioning index](../partitioning.md),
[clustering metrics](../../metrics/clustering.md).
