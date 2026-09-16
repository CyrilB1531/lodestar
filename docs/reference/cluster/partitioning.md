# Partitioning — `Lodestar.Cluster`

Two algorithms. [`KMeans`](partitioning/kmeans.md) partitions samples into `k` clusters by Lloyd's
algorithm, at `sklearn.cluster.KMeans(algorithm="lloyd")` parity.
[`Dbscan`](partitioning/dbscan.md) finds however many dense regions there are and leaves the rest
as noise, at `sklearn.cluster.DBSCAN` parity.

**The difference is what each is told.** k-means is told a cluster count and puts every sample in
one; DBSCAN is told how dense a region has to be, and a sample in none of them is labelled `-1`
rather than forced into the nearest.

**Spans in, arrays out, row-major** — the shape [`Lodestar.Metrics`](../metrics/clustering.md)
already uses. `Labels` feeds its silhouette, adjusted Rand, AMI and V-measure without being
reshaped, which is the whole reason this package was cheap to make credible: the scoring half
shipped first.

## The starting centres are an input

k-means begins with a choice, and a choice drawn from a generator is not reproducible across two
libraries. [`KMeansOptions.InitialCentres`](partitioning/kmeansoptions.md) takes the centres
themselves, and when they are given they replace the choice entirely — the move
[`decisions/0072`](../../decisions/0072-omega-is-an-input-not-a-seed.md) made for Ω. That is what
lets the oracle corpus compare every centre, label and inertia rather than comparing distributions.

Left alone, k-means++ chooses them with this package's own generator. `Seed` reproduces a run of
Lodestar and never a run of scikit-learn, and nothing frozen depends on it.

## One measured divergence

A sample exactly equidistant from two centres takes the **lowest-indexed** one here.
[`decisions/0093`](../../decisions/0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md)
has the two configurations that send the reference's choice both ways, and why neither rule
reproduces both.

## Types

| Type | What it is |
| --- | --- |
| [`KMeans`](partitioning/kmeans.md) | The fitted clustering: centres, labels, inertia, iterations. |
| [`KMeansOptions`](partitioning/kmeansoptions.md) | Where the fit starts, and when it stops. |
| [`Dbscan`](partitioning/dbscan.md) | The fitted density clustering: labels, core samples, cluster count. |

## One answer that depends on row order

A border sample two DBSCAN clusters can both reach joins whichever is **grown first**, which is the
one whose lowest-indexed core sample comes first — measured against the reference on four orderings
of one point set. Sorting a matrix can therefore change it, and nothing else in this package does.
[`Dbscan.Fit`](partitioning/dbscan-fit.md) states it where a caller will read it.

## See also

- [Clustering metrics](../metrics/clustering.md) — how to score what comes out.
- [Python → C# equivalence](../../equivalence.md).
