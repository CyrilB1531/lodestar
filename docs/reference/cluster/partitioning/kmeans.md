# KMeans

Partitions samples into `k` clusters by Lloyd's algorithm.

<!-- docs-declaration -->

```csharp
public sealed class KMeans
```

**Properties** — `ClusterCount` and `FeatureCount` are the shape. `Centres` is the cluster centres
row-major (`cluster_centers_`), `Labels` the cluster each fitted sample belongs to (`labels_`),
`Inertia` the summed squared distance from each sample to its centre (`inertia_`), and `Iterations`
how many Lloyd passes ran (`n_iter_`).

**Example** — three groups on a line and a plane, from centres sitting on them.

```csharp
using Lodestar.Cluster;

// Row-major, two features per row: two low points, two high, one in between.
double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

KMeans model = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] });

int firstLabel = model.Labels[0];   // => 0
double inertia = model.Inertia;     // => 1
int iterations = model.Iterations;  // => 2
```

**Remarks** — the loop is the reference's: assign, update, then stop when the labels stop moving
(*strict* convergence) or when the summed squared centre shift falls to or below the scaled
tolerance. When it stops on the shift rather than on the labels, a final assignment runs, so
`Labels` always matches `Centres` rather than trailing one update behind — a property
[`KMeans.Predict`](kmeans-predict.md) makes checkable.

An empty cluster is relocated onto the sample furthest from its own centre, so a cluster count
larger than the data supports still answers rather than dividing by zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeansOptions`](kmeansoptions.md), the [partitioning index](../partitioning.md),
[clustering metrics](../../metrics/clustering.md).

## Members

| Member | What it does |
| --- | --- |
| [`KMeans.Fit`](kmeans-fit.md) | Fits k-means on a row-major sample matrix. |
| [`KMeans.Predict`](kmeans-predict.md) | Assigns unseen samples to the fitted centres. |
