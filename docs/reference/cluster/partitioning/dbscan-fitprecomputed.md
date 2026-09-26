# Dbscan.FitPrecomputed

Clusters from a square distance matrix rather than from the samples themselves.

<!-- docs-declaration -->

```csharp
public static Dbscan FitPrecomputed(ReadOnlySpan<double> distances, int sampleCount, double epsilon, int minimumSamples)
```

<!-- docs-declaration -->

```csharp
public static Dbscan FitPrecomputed(ReadOnlySpan<double> distances, int sampleCount, double epsilon, int minimumSamples, ReadOnlySpan<double> sampleWeights)
```

The second overload weighs each sample, scikit-learn's `fit(X, sample_weight=w)`: a sample is core
when the weights in its neighbourhood, its own included, sum to at least `minimumSamples`.

**Parameters** — `distances` is the pairwise distances, row-major and square. `sampleCount` is the
side of that matrix. `epsilon` is the inclusive radius of a neighbourhood, scikit-learn's `eps`.
`minimumSamples` is how many samples a neighbourhood needs to be dense, the sample itself counted, or the weight it needs when `sampleWeights` is given. `sampleWeights` is one finite weight per
sample; zero and negative ones are accepted, a negative one keeping its neighbours from being core,
as the reference documents.

**Returns** — a fitted `Dbscan`.

**Exceptions** — `ArgumentOutOfRangeException` when `sampleCount` or `minimumSamples` is not
positive, or `epsilon` is not positive or not finite. `ArgumentException` when `distances` is not
`sampleCount` squared values, or holds a `NaN` or infinite one — an infinite distance is refused
too, as the reference refuses it, rather than read as unreachable, or when `sampleWeights` is not
one finite value per sample.

**Example** — three samples, given as the distances between them.

```csharp
using Lodestar.Cluster;

// Row-major and square: the first two samples are one apart, the third is far from both.
double[] distances =
[
    0.0, 1.0, 5.0,
    1.0, 0.0, 4.0,
    5.0, 4.0, 0.0,
];

Dbscan model = Dbscan.FitPrecomputed(distances, sampleCount: 3, epsilon: 1.0, minimumSamples: 2);

int pair = model.Labels[0];      // => 0
int alone = model.Labels[2];     // => -1
int clusters = model.ClusterCount;   // => 1
```

**Remarks** — this is `metric="precomputed"`, and it answers exactly what
[`Dbscan.Fit`](dbscan-fit.md) answers on the samples those distances came from. The corpus freezes
one case both ways to hold that true rather than to assert it.

**The diagonal is a zero distance, so a sample is its own neighbour here too** — which is what keeps
one meaning of `minimumSamples` across both entry points. A matrix whose diagonal is not zero is
not rejected, because nothing here requires a metric: it is read as given, which is also what lets
a caller pass a dissimilarity that is not a distance at all.

**Named rather than an overload of `Fit`.** Both take a row-major `ReadOnlySpan<double>` and an
`int`, so an overload pair would be told apart only by what the second argument *means* — and a
caller who passed a sample matrix where a distance matrix was expected would get an answer rather
than an error.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Dbscan`](dbscan.md), [`Dbscan.Fit`](dbscan-fit.md),
[clustering metrics](../../metrics/clustering.md).
