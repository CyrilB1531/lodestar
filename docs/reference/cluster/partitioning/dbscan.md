# Dbscan

Finds clusters as dense regions separated by sparse ones, leaving the rest as noise.

<!-- docs-declaration -->

```csharp
public sealed class Dbscan
```

**Properties** — `Noise` is the label a sample in no dense region carries, `-1`. `ClusterCount` is
how many clusters were found and `FeatureCount` how many values each fitted row carried. `Labels`
is the cluster each sample belongs to (`labels_`), and `CoreSampleIndices` the rows of the core
samples, ascending (`core_sample_indices_`).

**Example** — two tight pairs and one point far from both.

```csharp
using Lodestar.Cluster;

// Row-major, two features per row: three points near the origin, two near (5, 5),
// and one out at (9, 9) that nothing reaches.
double[] samples = [0.0, 0.0, 0.0, 0.3, 0.3, 0.0, 5.0, 5.0, 5.0, 5.2, 9.0, 9.0];

Dbscan model = Dbscan.Fit(samples, featureCount: 2, epsilon: 0.5, minimumSamples: 2);

int clusters = model.ClusterCount;  // => 2
int first = model.Labels[0];        // => 0
int lonely = model.Labels[5];       // => -1
int cores = model.CoreSampleIndices.Count;  // => 5
```

**Remarks** — **the cluster count is an output, not a question.** Where
[`KMeans`](kmeans.md) is told how many clusters to find and puts every sample in one, this is told
how dense a region has to be and finds however many there are — a sample in none of them is labelled
`Noise` rather than forced into the nearest.

**A core sample is one whose neighbourhood, itself included, holds at least `minimumSamples`
samples.** Every other labelled sample is a *border* sample: near enough to a cluster to join it,
not dense enough to grow it. The distinction is what
[`CoreSampleIndices`](dbscan.md) is for, and it decides the one answer below that depends on row
order.

**A border sample two clusters can both reach joins whichever is grown first**, which is the cluster
whose lowest-indexed core sample comes first. Measured against the reference on four orderings of
one point set: the label follows the growth order and not the border sample's own position, so
**sorting a matrix can change this answer**. It is the reference's behaviour rather than a choice
made here, and no other result depends on row order.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Dbscan.Fit`](dbscan-fit.md),
[`Dbscan.FitPrecomputed`](dbscan-fitprecomputed.md), [`KMeans`](kmeans.md), the
[partitioning index](../partitioning.md), [clustering metrics](../../metrics/clustering.md).
