# Linkage

How the distance between two clusters is measured when deciding which two to merge next.

<!-- docs-declaration -->

```csharp
public enum Linkage { Ward, Complete, Average, Single }
```

**Members** — `Ward` merges the pair that least increases the within-cluster variance, and is the
default. `Complete` measures the largest distance between a member of one cluster and a member of
the other, `Average` the mean of those distances, and `Single` the smallest.

**Example** — the same five points under the two extremes, where only the height of the last merge
differs.

```csharp
using Lodestar.Cluster;

double[] samples = [0.0, 1.0, 5.0, 6.0, 20.0];

AgglomerativeClustering single = AgglomerativeClustering.Fit(samples, 1, 2, Linkage.Single);
AgglomerativeClustering complete = AgglomerativeClustering.Fit(samples, 1, 2, Linkage.Complete);

double nearest = single.Distances[3];     // => 14
double farthest = complete.Distances[3];  // => 20
```

**Remarks** — **the choice changes the algorithm, not just the arithmetic.** `Single` runs a
minimum spanning tree and never holds a distance matrix; the other three run the nearest-neighbour
chain over one, `n(n − 1)/2` doubles. The two paths are the reference's own, and they break ties
differently, which is why both are written rather than one generic loop.

**`Ward`'s height is a distance, not a variance**: `√(2·n₁n₂/(n₁+n₂))` times the distance between
the two centroids, so two samples three apart merge at `3`. A library that reports the increase in
the sum of squares instead reports `d²/2` for the same tree, and its threshold does not carry across.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AgglomerativeClustering`](agglomerativeclustering.md),
[`AgglomerativeClustering.Fit`](agglomerativeclustering-fit.md).

## Members

| Member | What it does |
| --- | --- |
