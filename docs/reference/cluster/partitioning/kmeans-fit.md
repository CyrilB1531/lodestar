# KMeans.Fit

Fits k-means on a row-major sample matrix.

<!-- docs-declaration -->

```csharp
public static KMeans Fit(ReadOnlySpan<double> samples, int featureCount, int clusterCount, KMeansOptions options = null)
```

**Parameters** — `samples` is the sample matrix, row-major: `featureCount` values per row.
`featureCount` is how many values each row carries. `clusterCount` is how many clusters to find.
`options` says where to start and when to stop; `null` takes the defaults.

**Returns** — a fitted `KMeans`.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` or `clusterCount` is not
positive, or `options` asks for fewer than one iteration. `ArgumentException` when `samples` holds
no row or a partial one, when there are fewer rows than clusters, or when the given initial centres
are the wrong shape.

**Example** — a starting centre no sample is nearest to leaves its cluster empty, and the fit
recovers.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

// The third centre is five hundred units away from every sample.
KMeans model = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, -500.0, -500.0] });

// It is relocated onto the sample furthest from its own centre, and the fit lands
// on the same partition as a sensible start would.
double inertia = model.Inertia;   // => 1
int middle = model.Labels[4];     // => 2
```

**Remarks** — **`Tolerance` is scaled before it is used**, by the mean feature variance, exactly as
`sklearn.cluster._kmeans._tolerance` scales it. The same number therefore means the same thing on a
matrix of millimetres and one of kilometres. A `Tolerance` of `0` removes the shift test entirely
and iterates until the labels settle.

**A sample exactly equidistant from two centres takes the lowest-indexed one.** That is
`numpy.argmin`'s rule and a **measured divergence** from the reference, whose choice was observed
going both ways on two configurations;
[`decisions/0093`](../../../decisions/0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md)
has both and says why neither rule reproduces the pair. No frozen case turns on a tie.

**The starting centres are an input, not a seed.** Passing
[`KMeansOptions.InitialCentres`](kmeansoptions.md) replaces the choice entirely and makes the run an
ordinary parity target — the move [`decisions/0072`](../../../decisions/0072-omega-is-an-input-not-a-seed.md)
made for Ω. Without them, k-means++ draws from this package's own generator, which reproduces a run
of Lodestar and never a run of scikit-learn.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeans`](kmeans.md), [`KMeans.Predict`](kmeans-predict.md),
[`KMeansOptions`](kmeansoptions.md).
