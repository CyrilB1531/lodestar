# AgglomerativeClustering.Fit

Clusters a row-major sample matrix into a given number of clusters.

<!-- docs-declaration -->

```csharp
public static AgglomerativeClustering Fit(ReadOnlySpan<double> samples, int featureCount, int clusterCount, Linkage linkage = Linkage.Ward)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `clusterCount` is how many clusters to cut the
tree into. `linkage` is how the distance between two clusters is measured; Ward by default, as in
the reference.

**Returns** — a fitted `AgglomerativeClustering`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` or `clusterCount` is not
positive, when `clusterCount` exceeds the sample count, or when `linkage` is not a defined value.
`ArgumentException` when `samples` holds fewer than two rows, a partial one, or a `NaN` or infinite
value.

**Example** — the same five points cut into three under complete linkage, where the labels show the
reference's numbering.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 1.0, 5.0, 6.0, 20.0];

AgglomerativeClustering model = AgglomerativeClustering.Fit(
    samples, featureCount: 1, clusterCount: 3, Linkage.Complete);

// Not 0, 0, 1, 1, 2: a label is a position in the heap the cut walks, not an order of appearance.
int first = model.Labels[0];    // => 2
int middle = model.Labels[2];   // => 0
int far = model.Labels[4];      // => 1

// Complete linkage's last merge is the largest distance between the two final clusters.
double top = model.Distances[3];   // => 20
```

**Remarks** — **one sample is refused**, as the reference refuses it: there is no merge to make and
no tree to cut.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AgglomerativeClustering`](agglomerativeclustering.md),
[`AgglomerativeClustering.FitToThreshold`](agglomerativeclustering-fittothreshold.md),
[`Linkage`](linkage.md).
