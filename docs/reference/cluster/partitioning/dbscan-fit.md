# Dbscan.Fit

Clusters a row-major sample matrix by euclidean distance.

<!-- docs-declaration -->

```csharp
public static Dbscan Fit(ReadOnlySpan<double> samples, int featureCount, double epsilon, int minimumSamples)
```

<!-- docs-declaration -->

```csharp
public static Dbscan Fit(ReadOnlySpan<double> samples, int featureCount, double epsilon, int minimumSamples, ReadOnlySpan<double> sampleWeights)
```

The second overload weighs each sample, scikit-learn's `fit(X, sample_weight=w)`: a sample is core
when the weights in its neighbourhood, its own included, sum to at least `minimumSamples`.

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `epsilon` is the inclusive radius of a
neighbourhood, scikit-learn's `eps`. `minimumSamples` is how many samples a neighbourhood needs to
be dense, the sample itself counted, or the weight it needs when `sampleWeights` is given. `sampleWeights` is one finite weight per
sample; zero and negative ones are accepted, a negative one keeping its neighbours from being core,
as the reference documents.

**Returns** — a fitted `Dbscan`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` or `minimumSamples` is not
positive, or `epsilon` is not positive or not finite. `ArgumentException` when `samples` holds no
row, a partial one, or a `NaN` or infinite value, or when `sampleWeights` is not one finite value
per row.

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

Weighted, a row can stand for the several identical rows it replaced.

```csharp
using Lodestar.Cluster;

// The last row stands for three; alone it is noise at a minimum of three.
double[] rows = [0.0, 1.0, 5.0];
Dbscan weighted = Dbscan.Fit(rows, featureCount: 1, epsilon: 0.5, minimumSamples: 3, [1.0, 1.0, 3.0]);
int dense = weighted.Labels[2];   // => 0
int alone = weighted.Labels[0];   // => -1
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
can put it out. scikit-learn's own search is not that exact at the boundary: on small inputs it
goes brute force, which computes `x² + y² − 2xy` and can place a sample exactly `epsilon` away a
rounding step outside — `1.3` and `0.3` at `epsilon: 1.0` — where its tree searches and this method
keep it in. Away from exact ties the labels are identical.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Dbscan`](dbscan.md), [`Dbscan.FitPrecomputed`](dbscan-fitprecomputed.md),
[`KMeans.Fit`](kmeans-fit.md).
