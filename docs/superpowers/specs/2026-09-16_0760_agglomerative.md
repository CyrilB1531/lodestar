# 0760 — Agglomerative clustering in `Lodestar.Cluster`

**Status:** accepted, 2026-09-16. Written before the work.

Issue: [#760](https://github.com/CyrilB1531/lodestar/issues/760).

Reading: [decision 0131](../../decisions/0131-lodestar-cluster-writes-what-netstandard2-0-lacks.md),
which writes agglomerative clustering here and names `Aglomera` as the incumbent to measure first;
[decision 0129](../../decisions/0129-four-numerics-libraries-read-and-three-absences-withdrawn.md),
the reading beneath it. `sklearn.cluster.AgglomerativeClustering` 1.9.0 and
`scipy.cluster.hierarchy` 1.18.1, read by probe and — for the single-linkage path — from
scikit-learn's own shipped Cython, on 2026-09-16.

## Problem

`Lodestar.Cluster` has `KMeans` and, with #759, `Dbscan`. `docs/equivalence.md` lists
`AgglomerativeClustering` as *being written*. Decision 0131 found one free .NET implementation,
**`Aglomera` 1.1.1** — MIT by `licenseUrl`, `netstandard1.3`, every linkage scikit-learn has, not
released since 2020-07-05 and never compared with scikit-learn — and the issue asks for that
comparison **before any code**, with the explicit alternative of delegating to it.

## First: `Aglomera`, measured

Five fixtures, four linkages, merge trees compared as leaf sets in merge order:

| fixture | single | complete | average | ward |
| --- | --- | --- | --- | --- |
| three blobs, 30 points, 3-D, no ties | same tree, same order | same | same | same |
| a line of five, one tie | **same tree, reversed tie** | **reversed** | **reversed** | **reversed** |
| duplicate rows | **reversed** | **reversed** | **reversed** | **reversed** |
| an evenly spaced line, every gap tied | **different tree** | **reversed** | **reversed** | **reversed** |
| a unit square, every side tied | **different tree** | **different tree** | **different tree** | **different tree** |

**Where no two merge distances tie, `Aglomera` is exact** — heights within `3.4e-13` of the reference
on every linkage. **Where they tie, it diverges on every linkage**: it breaks ties toward the higher
index where the reference breaks them toward the lower, and on a square a different first merge
makes a different tree, so `labels_` at a given cluster count differs. Ties are not exotic — integer
features, duplicate rows and grids all produce them.

**Its Ward height is on another scale as well**: it reports the increase in within-cluster sum of
squares, `d²/2` where the reference reports `d` (1 → 0.5, 7.07 → 25, 21.5 → 231.2). The tree is the
same and every height is different, so a `distance_threshold` does not carry across.

**So delegation does not give parity**, and the issue's condition for re-weighing it is not met.
This lot writes the class. `Aglomera` stays the benchmark incumbent, measured on tie-free data where
the two agree.

## What the reference does, measured and read

1. **Two algorithms, not one.** With no connectivity and a Euclidean metric, scikit-learn sends
   `single` to its own **minimum spanning tree** — Prim's algorithm, `mst_linkage_core`, then a
   stable sort of the edges by weight and a union-find labelling — and sends `ward`, `complete` and
   `average` to **`scipy.cluster.hierarchy.linkage`**, which runs the **nearest-neighbour chain**
   and then sorts its merges stably by distance and relabels them with a union-find. The two paths
   break ties differently, so both are written.
2. **`children_` pairs are ordered differently by the two paths.** scipy writes `[min, max]` of the
   two node ids; the MST path writes `[left, right]` in edge order. Measured on `0, 1, 5, 6, 20`: the
   final merge is `[4, 7]` under complete and `[7, 4]` under single. Exact `children_` parity has to
   reproduce both.
3. **Ward's height is `√(2·n₁n₂/(n₁+n₂))·‖c₁ − c₂‖`**, the Lance–Williams form scipy updates:
   two points three apart merge at `3.0`, and `{0, 2}` meets `10` at `√(4/3)·9 = 10.392…`.
4. **Labels are not numbered by first appearance.** `_hc_cut` walks the tree from the root with a
   `heapq` of negated node ids, and a sample's label is **its cluster's position in the heap's
   array**, not in sorted order. `0, 1, 5, 6, 20` cut at three gives `2 2 0 0 1`.
5. **`distance_threshold` is strict.** The cluster count is one more than the number of merges whose
   height is **at or above** the threshold: points at `0, 1, 3` under single linkage stay three
   clusters at a threshold of exactly `1.0` and become two at `1.001`.
6. **With no connectivity the whole tree is always built**, whatever `compute_full_tree` says, so
   `labels_` always comes from `_hc_cut` and never from the early-stopping route.
7. **The distance update has to be written in scipy's floating-point order, not merely its
   algebra.** Ward multiplies through by `t = 1/(nx + ny + ni)`; the equal form that divides once at
   the end gives **a different tree on 16 of 400 random integer datasets**, and heights that are not
   bit-identical on 323, because one ulp turns a tie into an order. Average has the same trap on 1
   of 400. Written in scipy's order, both are bit-identical on all 400 — and a fixture set of
   hand-built ties passed under the wrong form, which is why the corpus carries random integer data
   as well.

## Placement

```csharp
public sealed class AgglomerativeClustering
{
    public static AgglomerativeClustering Fit(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, Linkage linkage = Linkage.Ward);
    public static AgglomerativeClustering FitToThreshold(
        ReadOnlySpan<double> samples, int featureCount, double distanceThreshold, Linkage linkage = Linkage.Ward);

    public IReadOnlyList<int> Labels { get; }          // labels_
    public IReadOnlyList<int> Children { get; }        // children_, row-major pairs
    public IReadOnlyList<double> Distances { get; }    // distances_
    public int ClusterCount { get; }                   // n_clusters_
    public int FeatureCount { get; }
    public Linkage Linkage { get; }
}

public enum Linkage { Ward, Complete, Average, Single }
```

- **Two named factories**, as `Dbscan` has, rather than a nullable pair where exactly one must be
  set. The reference raises at run time when both or neither of `n_clusters` and
  `distance_threshold` are given; two methods make that unrepresentable.
- **`Distances` is always populated.** The reference computes them only under
  `compute_distances=True`, to save memory it has already spent on the distance matrix. Here they
  are `n − 1` doubles beside an `n²` matrix, and a flag to withhold them would save nothing a caller
  could measure.
- **`Children` is row-major**, two ids per merge, as `KMeans.Centres` is row-major: no rectangular
  array (CA1814) and no array-typed property (CA1819).
- **`Ward` is the default**, as it is the reference's.
- **Rejected: delegating to `Aglomera`.** Measured above: it diverges on ties under every linkage
  and reports Ward on another scale.
- **Rejected: one algorithm for all four linkages.** A generic O(n³) agglomeration gives the same
  heights and different tie orders from both reference paths; parity means writing the two the
  reference actually runs.
- **Out of scope, from the issue:** `connectivity`, metrics other than Euclidean, sparse input,
  `pooling_func`. **The `O(n²)` distance matrix is stated on the reference page**, not hidden.

## Evidence

- **`cluster_agglomerative.json`** from `sklearn.cluster.AgglomerativeClustering`, **`labels_` and
  `children_` compared exactly and `distances_` at 1e-9**, over the four linkages: the five fixtures
  of the `Aglomera` table, because the tied ones are exactly where an implementation that is right
  on tie-free data goes wrong; a cut at more than one cluster count, so the heap order of fact 4 is
  pinned; `distance_threshold` at a merge height and just above it, for fact 5; and **random
  integer datasets**, dense with ties, because fact 7's trap passed every hand-built fixture.
- **Edge tests:** a cluster count below one or above the sample count; a negative, NaN or infinite
  threshold — the reference's range is `[0, inf)`, open at the top, and zero is allowed; a single
  sample, which the reference refuses with *"a minimum of 2 is required"*; a partial row; and an
  undefined `Linkage` value.
- **Benchmark:** against **`Aglomera` 1.1.1** on tie-free blobs, where the agreement check can pass.
