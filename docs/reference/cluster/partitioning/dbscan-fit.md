# Dbscan.Fit

Clusters a row-major sample matrix by euclidean distance.

<!-- docs-declaration -->

```csharp
public static Dbscan Fit(ReadOnlySpan<double> samples, int featureCount, double epsilon, int minimumSamples)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `epsilon` is the inclusive radius of a
neighbourhood, scikit-learn's `eps`. `minimumSamples` is how many samples a neighbourhood needs to
be dense, the sample itself counted.

**Returns** — a fitted `Dbscan`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` or `minimumSamples` is not
positive, or `epsilon` is not positive or not finite. `ArgumentException` when `samples` holds no
row or a partial one.

**Example** — the same three points at two radii, where the whole boundary rule is visible.

```csharp
using Lodestar.Cluster;

// One feature per row: three points, each exactly one unit from the next.
double[] samples = [0.0, 1.0, 2.0];

// A radius a hair under the gap reaches nobody, so every point is noise.
Dbscan under = Dbscan.Fit(samples, featureCount: 1, epsilon: 0.999, minimumSamples: 2);
int none = under.ClusterCount;   // => 0

// A radius exactly at the gap does reach: the test is <=, not <.
Dbscan exact = Dbscan.Fit(samples, featureCount: 1, epsilon: 1.0, minimumSamples: 2);
int one = exact.ClusterCount;    // => 1
int middle = exact.Labels[1];    // => 0
```

**Remarks** — **`epsilon` is inclusive.** A sample exactly that far away is a neighbour, as the
example shows; the reference tests `<=` and so does this.

**`minimumSamples` counts the sample itself.** Two samples inside `epsilon` of each other are both
core at `2` and both noise at `3`, so a neighbourhood of *n* holds *n − 1* other samples. A lone
sample is therefore a cluster of one at `minimumSamples: 1`.

**Neither value is defaulted.** scikit-learn defaults `eps=0.5`, which is meaningful only on data
already scaled to unit variance: on a matrix of euros it is one cluster and on a matrix of
milliseconds all noise. A default wrong on most matrices reads as advice, so this asks — the one
shape difference from [`KMeans.Fit`](kmeans-fit.md), whose options are optional because each of
them has a defensible default.

**Distances are compared squared**, against a squared radius, so no square root is taken. That is
exactness rather than speed: a sample at exactly `epsilon` has to stay inside, and rounding a root
can put it out.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Dbscan`](dbscan.md), [`Dbscan.FitPrecomputed`](dbscan-fitprecomputed.md),
[`KMeans.Fit`](kmeans-fit.md).
