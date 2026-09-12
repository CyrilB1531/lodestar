# KMeansOptions

Where [`KMeans.Fit`](kmeans-fit.md) starts, and when it stops.

<!-- docs-declaration -->

```csharp
public sealed record KMeansOptions
```

**Properties** — `MaxIterations` caps the Lloyd loop (`max_iter`, default 300). `Tolerance` is the
convergence threshold before scaling (`tol`, default `1e-4`). `Seed` drives this package's own
generator when no centres are given. `InitialCentres` is the starting block itself, row-major and
`clusterCount × featureCount`, or `null` to choose one.

**Example** — the same data, started two ways.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

// Given centres: reproducible anywhere, and comparable against Python.
KMeans given = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] });

// Drawn centres: reproducible run to run here, and nowhere else.
KMeans drawn = KMeans.Fit(samples, featureCount: 2, clusterCount: 3,
    new KMeansOptions { Seed = 11 });

double reachedTheSamePartition = drawn.Inertia - given.Inertia;  // => 0
```

**Remarks** — **`Seed` and `InitialCentres` answer different questions, and only one of them travels.**
A seed reproduces a run of Lodestar: the two libraries draw from different generators, so a shared
seed shares nothing. `InitialCentres` is what the oracle corpus passes, and what a caller comparing
against Python must pass too.

`Tolerance` is multiplied by the mean feature variance before use, so it is scale-free; `0` removes
the shift test and iterates until the labels settle.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeans.Fit`](kmeans-fit.md), [`KMeans`](kmeans.md),
[`decisions/0072`](../../../decisions/0072-omega-is-an-input-not-a-seed.md).

## Members

| member | what it does |
| --- | --- |
| [`KMeansOptions.Equals`](kmeansoptions-equals.md) | Value equality, the centres element by element. |
| [`KMeansOptions.GetHashCode`](kmeansoptions-gethashcode.md) | A hash consistent with it. |
