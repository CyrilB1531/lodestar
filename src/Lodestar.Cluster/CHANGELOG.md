# Changelog — Lodestar.Cluster

What changed in `Lodestar.Cluster`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.2.0] — 2026-09-24

### Added

- `Dbscan` clusters by density, at scikit-learn parity. ([#759](https://github.com/CyrilB1531/lodestar/issues/759), [`6c245dd5`](https://github.com/CyrilB1531/lodestar/commit/6c245dd5))
- `AgglomerativeClustering` builds and cuts a merge tree under four linkages, at scikit-learn parity. ([#760](https://github.com/CyrilB1531/lodestar/issues/760), [`41f95c0b`](https://github.com/CyrilB1531/lodestar/commit/41f95c0b))

### Changed

- `Linkage` and `KMeansOptions` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `AgglomerativeClustering.Fit` scans only live clusters along precomputed row offsets, and `KMeans` assigns rows wider than four features over sliced spans. ([#853](https://github.com/CyrilB1531/lodestar/issues/853))
- `KMeans` lists its empty clusters in one walk over the counts, where a LINQ pass counted them first. ([#1047](https://github.com/CyrilB1531/lodestar/issues/1047))
- `Dbscan.Fit` computes each pair's distance once and stops a sum past the radius. ([#818](https://github.com/CyrilB1531/lodestar/issues/818))
- `KMeansOptions` compares its centres by value. ([#668](https://github.com/CyrilB1531/lodestar/issues/668), [`a2b11493`](https://github.com/CyrilB1531/lodestar/commit/a2b11493))

### Fixed

- `KMeans.Fit` relocates clusters emptied in the same iteration onto distinct furthest samples, as scikit-learn does, where it could leave a `NaN` centre. ([#862](https://github.com/CyrilB1531/lodestar/issues/862))
- `KMeans`, `Dbscan` and `AgglomerativeClustering` refuse a `NaN` or infinite sample with `ArgumentException`, as scikit-learn does, where they returned `NaN` centres or noise or threw an index out of range. ([#896](https://github.com/CyrilB1531/lodestar/issues/896))
- `KMeans.Fit` refuses a negative, infinite or `NaN` `KMeansOptions.Tolerance`, and `Dbscan.FitPrecomputed` checks a sample count past 46340 without wrapping. ([#911](https://github.com/CyrilB1531/lodestar/issues/911))
- `KMeans.Fit` no longer throws `IndexOutOfRangeException` when a relocation empties a cluster numbered above the one it fills, which the fix for #862 introduced. ([#975](https://github.com/CyrilB1531/lodestar/issues/975))
- The `KMeans.Fit` page says the relocation pairing can differ from `numpy.argpartition`'s even without a tie, which was recorded for ties alone. ([#990](https://github.com/CyrilB1531/lodestar/issues/990))
- The `KMeans.Fit` page says scikit-learn's relocation choice changes with the CPU tier numpy dispatches to, so with tied distances the centres and `Inertia` can differ, and drops a bound measured on one seed. ([#1043](https://github.com/CyrilB1531/lodestar/issues/1043), [#1066](https://github.com/CyrilB1531/lodestar/issues/1066))

## [0.1.0] — 2026-09-10

### Added

- **`Lodestar.Cluster` 0.1.0 — k-means by Lloyd's algorithm, at scikit-learn parity, with the starting centres as an input.** `KMeans.Fit` takes a row-major span and returns `Centres`, `Labels`, `Inertia` and `Iterations`, the four things scikit-learn reports; `Predict` assigns unseen rows without refitting. `Labels` is the shape `Lodestar.Metrics` already scores, so a clustering arrives with silhouette, adjusted Rand, AMI and V-measure on day one rather than needing them built — which is the argument [#442](https://github.com/CyrilB1531/lodestar/issues/442) made for this domain, and it held. The loop is the reference's: assign, update, stop on unchanged labels or on a centre shift within the tolerance, and when it stops on the shift a final assignment runs so `Labels` matches `Centres` rather than trailing one update behind. `Tolerance` is scaled by the mean feature variance, as `_tolerance` scales it, so the same number means the same thing at any scale. An empty cluster is relocated onto the sample furthest from its own centre. **`KMeansOptions.InitialCentres` is an input, not a seed** — [decision 0072](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0072-omega-is-an-input-not-a-seed.md)'s move, applied here: given a starting block the run is an ordinary parity target, and `tests/oracles/cluster_kmeans.json` compares every centre, label, inertia and iteration count across eight cases rather than comparing distributions. Left alone, k-means++ draws from this package's own generator and reproduces a run of Lodestar, never one of scikit-learn. **One measured divergence, and it is recorded rather than papered over**: a sample exactly equidistant from two centres takes the lowest-indexed one here, where the reference was observed choosing the second on one configuration and the first on another; [decision 0093](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md) has both, and the two corpus fixtures that hinged on a tie were replaced so no frozen case rests on a convention. Core tier — no external dependency, no inter-package edge. ([#567](https://github.com/CyrilB1531/lodestar/issues/567))
