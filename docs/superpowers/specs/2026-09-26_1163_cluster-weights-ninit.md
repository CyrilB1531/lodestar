# Sample weights for k-means and DBSCAN, and k-means restarts

**Issue:** [#1163](https://github.com/CyrilB1531/lodestar/issues/1163).
**Status:** written before the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

`docs/equivalence.md` carried `sample_weight=` on `KMeans` and `DBSCAN`, `n_init` above 1 and
`algorithm="elkan"` as *no counterpart*. A weight is how a deduplicated matrix is clustered as the
full one, and `n_init` is how a caller escapes a poor start; both are in the reference's default
path.

## What scikit-learn 1.9.1 does

- `KMeans.fit(X, sample_weight=w)`, Lloyd: each centre is its members' weighted mean and `inertia_`
  is `Σ w·d²`. A cluster is empty when `weight_in_clusters` is exactly zero; it is relocated onto
  the furthest sample by unweighted distance, taking that sample's weight from its old cluster, and
  a cluster still weighing nothing takes the heaviest cluster's centre. `_tolerance` stays the
  unweighted mean column variance. Negative weights are accepted; all-zero ones divide by zero.
  k-means++ draws in proportion to the weights.
- `n_init`: one run per start; the first is kept, and a later one replaces it only with a strictly
  lower inertia **and** a partition `_is_same_clustering` calls different. A callable `init`
  receives `X` less its column means.
- `DBSCAN.fit(X, sample_weight=w)`: a sample is core when its neighbourhood's weights, its own
  included, sum to `min_samples`; negative weights inhibit a core. On small inputs the neighbour
  search is brute force, computing `x² + y² − 2xy`, which can put a sample at exactly `eps` a
  rounding step outside — `1.3` against `0.3` at `eps = 1`.

## Decisions

As approved on the issue:

1. `KMeans.Fit(samples, sampleWeights, featureCount, clusterCount, options)`; negative weights
   accepted, all-zero refused with `ArgumentException`.
2. `Dbscan.Fit` and `Dbscan.FitPrecomputed` each take a trailing `sampleWeights` overload.
3. `KMeansOptions.InitialCentreSets`, one run per given block, keeping the reference's winner.
4. `KMeansOptions.Restarts`, k-means++ draws seeded `Seed + i`, under the same rule.
5. Elkan's algorithm stays out: an optimisation reaching the same partition.
6. Proof by a frozen corpus, a random differential and a benchmark against scikit-learn.

`KMeansOptions` lives in `Lodestar.Abstractions`, so `Lodestar.Cluster` reaches it by project until
the next publication.

## Proof

- `cluster_weighted.json`, 16 cases from scikit-learn 1.9.1: weighted k-means (whole, fractional,
  zero and negative weights, an emptied cluster), restarts over given starts (a worse start, a
  permuted start reaching the same partition, weighted restarts) and weighted DBSCAN on both entry
  points. Two fixtures were moved off ties the corpus is not about: a relocation among equally far
  samples (#990) and a sample at exactly `eps`.
- A random differential against scikit-learn: 1,800 cases under two seeds. One differed, three
  clusters emptied at once among pairwise-tied distances — the relocation tie the k-means page
  already documents.
- A benchmark against scikit-learn, ahead on every row in processor time. The first reading was
  behind at four starts over 100,000 rows and on DBSCAN at 20,000: the E-step now vectorises across
  centres with every label bit-identical, and the neighbour search sorts along the widest feature and
  stops each scan out of reach. k-means stays behind in elapsed time, where scikit-learn threads it.
