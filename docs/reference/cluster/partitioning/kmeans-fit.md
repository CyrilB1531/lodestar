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
positive, or `options` asks for fewer than one iteration or a `Tolerance` that is negative, infinite
or `NaN`. `ArgumentException` when `samples` holds no row, a partial one or a `NaN` or infinite
value, when there are fewer rows than clusters, or when the given initial centres are the wrong
shape or not finite.

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
[`decisions/0007`](../../../decisions/0007-the-deliberate-divergences.md)
has both and says why neither rule reproduces the pair. No frozen case turns on a tie.

**Empty clusters are relocated together, as the reference relocates them.** From one pass of
distances, each empty cluster takes a distinct one of the samples furthest from their centres, in
cluster order, and that sample's old cluster gives it up; the labels are left alone. When every
sample already sits on its centre nothing moves, and a cluster still empty takes the largest
cluster's centre — **before that centre is averaged** when the largest cluster comes later, the
reference's own order, which the corpus freezes. The furthest samples are taken in descending
distance, ties lowest row first. The reference reads them through `numpy.argpartition`, whose
result **changes with the CPU tier numpy dispatches to**: numpy 2.5.3 on one machine returned three
different selections from the same distances under its AVX-512, AVX2 and baseline kernels. No rule
reproduces all three, so **which sample fills which empty cluster can differ even with no tie**, and
when the furthest distances are tied **the samples chosen, the centres and `Inertia` can differ
too** — scikit-learn 1.9.0 returned `inertia_` `2.5e-31` under AVX-512 and `193.26` under the
baseline for one input. Over 1,000 random fits with distinct distances the centre sets and `Inertia`
matched on all 1,000, whatever the pairing; over 1,000 fits of a few points repeated, scikit-learn
disagreed with itself across tiers on 35 centre sets and this method disagreed with it on 214, and
taking ties highest row first instead leaves 210. With no cluster emptied the same kind of input
matched on all 1,000. The same reasoning as decision 0007; no frozen case turns on it yet.

**The starting centres are an input, not a seed.** Passing
[`KMeansOptions.InitialCentres`](kmeansoptions.md) replaces the choice entirely and makes the run an
ordinary parity target — the move [`decisions/0004`](../../../decisions/0004-what-is-written-here-and-what-is-delegated.md)
made for Ω. Without them, k-means++ draws from this package's own generator, which reproduces a run
of Lodestar and never a run of scikit-learn.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeans`](kmeans.md), [`KMeans.Predict`](kmeans-predict.md),
[`KMeansOptions`](kmeansoptions.md).
