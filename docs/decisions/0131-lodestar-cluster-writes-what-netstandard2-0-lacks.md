---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0093", "0129"]
---
# 0131 — `Lodestar.Cluster` writes what `netstandard2.0` lacks: DBSCAN and agglomerative clustering

**Status:** accepted · **Date:** 2026-09-14 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0093`](0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md), [`0129`](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)

## Context

`Lodestar.Cluster` is one class. `docs/equivalence.md` listed the rest of scikit-learn's clustering
as `MiniBatchKMeans`, `DBSCAN`, `AgglomerativeClustering`, `SpectralClustering`, `GaussianMixture`
— *no counterpart yet*, pointing at [#442](https://github.com/CyrilB1531/lodestar/issues/442), which
is closed. [#681](https://github.com/CyrilB1531/lodestar/issues/681) asked what the package is for
now that a maintained library ships four algorithms to its one, and asked for an answer that is
either *fill it* or *scope it down*, not a list of equals with nobody tracking it.

## The reading

[Decision 0129](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md) read the four
numerics libraries on 2026-09-14; the clustering members, read again with `tools/survey.cs`
([decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)), are:

| | NumFlat 1.3.4 | Meta.Numerics 4.2.0 | Numerics.NET 10.7.0 | MathNet.Numerics 5.0.0 |
| --- | --- | --- | --- | --- |
| licence, from the package | MIT | MS-PL | commercial | MIT |
| `lib/` | `net8.0` | `netstandard2.0` | `net462`, `netstandard2.0`, `net8.0`+ | `net461`, `net48`, `netstandard2.0`, `net5.0`, `net6.0` |
| k-means | `KMeans`: k-means++ with `TryCount` restarts, and `Update` for one Lloyd step from given centroids | `Multivariate.MeansClustering(columns, m)`: no seed, no starting centres | `KMeansClusterAnalysis` | none |
| DBSCAN | `Clustering.DbScan(xs, eps, minPoints)` | none | none | none |
| agglomerative | none | none | `HierarchicalClusterAnalysis` | none |
| Gaussian mixture | `GaussianMixtureModel`, `DiagonalGaussianMixtureModel` | none | `GaussianMixtureDistribution` | none |
| k-medoids | `KMedoids<T>`, any distance | none | none | none |
| spectral, mini-batch | none | none | none | none |

Among the numerics libraries, **on `netstandard2.0` and free, clustering is two k-means**: this
package's and Meta.Numerics'. `Accord.MachineLearning` 3.8.0 adds k-means and a Gaussian mixture under
LGPL-2.1, last released in 2017 and archived in 2020.

### The dedicated packages

nuget.org, 2026-09-14, searched for `dbscan`, `hierarchical clustering`, `agglomerative`,
`clustering` and `gaussian mixture`. Three single-purpose packages answer, and a reading that stopped
at the numerics libraries would have claimed a void that is not there:

| package | published | licence | `lib/` | surface | what it is |
| --- | --- | --- | --- | --- | --- |
| `Dbscan` 3.0.0 | 2022-07-12 | MIT, `<license type="expression">` | `netstandard1.2`, `netcoreapp3.1`, `net6.0` | 9 types, 31 members | DBSCAN over `IPointData`, whose `Point` carries **`X` and `Y` and nothing else**: two dimensions only. The repository was last pushed 2023-03-01. |
| `Aglomera` 1.1.1 | 2020-07-05 | MIT, by `licenseUrl` to the repository's `LICENSE.md` — the package carries no licence field | `netstandard1.3` | 39 types, 136 members | Agglomerative clustering over any dissimilarity: `SingleLinkage`, `CompleteLinkage`, `AverageLinkage`, `CentroidLinkage`, `WardsMinimumVarianceLinkage`, `MinimumEnergyLinkage`. No release in six years; the repository was last pushed 2022-12-08. |
| `HdbscanSharp` 3.0.1 | 2025-05-06 | MIT, by `licenseUrl` | `netstandard2.0`, `net8.0` | 23 types, 85 members | HDBSCAN — `HdbscanRunner.Run` returning `Labels` and `OutliersScore`. scikit-learn has carried `HDBSCAN` since 1.3. |

So the voids are narrower than the numerics libraries suggest. **An n-dimensional DBSCAN is absent
below `net8.0`**: the one free package is planar. **Agglomerative clustering is present** on
`netstandard2.0`, with every linkage scikit-learn offers, in a package not released since 2020 and
never compared with scikit-learn. **HDBSCAN is present and maintained.**

## The measurement

`bench/README.md` section 34 has the method and `docs/guides/performance.md` the numbers, on an AMD
Ryzen 7 8700G with the default job. Every pair agreed on the centres before it was timed, and the
largest difference was zero.

- **Lloyd's iterations from the same centres, against NumFlat's `Update`: ahead on every shape,
  1.52× to 3.66×.** This is the like-for-like pair.
- **A whole default fit, k-means++ included: 5.5× to 13× cheaper than NumFlat's, 4.1× to 20× cheaper
  than Meta.Numerics'.** Part of that is scikit-learn's scaled tolerance stopping after one or two
  iterations on separated blobs, and neither incumbent reports its own count.

The one algorithm the package has is not the weak point; its scope is.

## Decision

**`Lodestar.Cluster` is scikit-learn's clustering where `netstandard2.0` has none, and it fills that
scope in two lots.** The two grounds #681 offered decide it: `netstandard2.0` reach is where the
package's reason to exist lies, and sparse input decides nothing yet, since none of the incumbents
takes a `CsrMatrix` either.

| scikit-learn | verdict | why |
| --- | --- | --- |
| `DBSCAN` | **written**, [#759](https://github.com/CyrilB1531/lodestar/issues/759) | Below `net8.0` the only free one is `Dbscan`, and it is planar. Deterministic given the neighbourhood, so parity is exact labels. `Dbscan` is benchmarked on two-dimensional input and NumFlat on the rest. |
| `AgglomerativeClustering` | **written**, [#760](https://github.com/CyrilB1531/lodestar/issues/760), **with `Aglomera` as its measured incumbent** | `Aglomera` covers the linkages on `netstandard2.0` and has not been released since 2020, which is the "no *maintained* equivalent" this project writes against — the position `Accord.Statistics` put `Lodestar.Stats` in. The lot compares the two before claiming anything: if `Aglomera` agrees with scikit-learn on labels and merge tree, the lot says so and is re-weighed. Merges are deterministic, so parity is exact labels and an exact `children_`. |
| `HDBSCAN` | **delegated to `HdbscanSharp`** | MIT, `netstandard2.0`, released 2025. Not compared with scikit-learn here; the migration row says so. |
| `GaussianMixture` | **delegated to NumFlat** on `net8.0`; a scoped gap below it | NumFlat covers it for the targets most callers are on. Parity is possible — `means_init`, `weights_init` and `precisions_init` make EM an input-driven loop, as `init` made Lloyd's — so this is a lot not yet taken, not a refusal. |
| k-medoids | **delegated to NumFlat** | Not scikit-learn: it lives in `scikit-learn-extra`, outside the reference this package holds itself to. |
| `MiniBatchKMeans` | **not written** | Its batches are drawn inside the fit from `random_state` through numpy's generator, and no parameter hands them in. There is no input that makes a run reproducible from .NET, so there is nothing to hold a corpus to. |
| `SpectralClustering` | **not written until the eigensolver exists** | It needs the leading eigenvectors of an affinity Laplacian — ARPACK by default. [`PrincipalComponentVariance`](../reference/decomposition/factorization/principalcomponentvariance.md) computes eigenvalues of a small dense Gram matrix; that is not this. |

**Ties are recorded, not chased.** [Decision 0093](0093-an-exact-tie-between-centres-is-not-part-of-k-means-parity.md)
found scikit-learn breaking an exact distance tie both ways inside a BLAS-driven kernel. DBSCAN's
border points reachable from two clusters and agglomerative merges at equal distance are the same
kind of choice; each lot measures the reference's and states it in `docs/equivalence.md`, rather
than letting a tie decide what parity means.

## Options that lost

- **Delegate agglomerative clustering to `Aglomera` and DBSCAN to `Dbscan`.** Both install on
  `netstandard2.0` and both are MIT. It lost on dimensions for DBSCAN, and for `Aglomera` it is not
  lost yet so much as unproven: #760 measures it first.
- **Scope the package down to k-means and delegate the rest to NumFlat.** Honest, and it would
  close #681 in a day. It lost on the first table: a `.NET Framework`, Mono or Unity caller would be
  delegated to a library that does not install, which is the failure
  [decision 0116](0116-the-pca-gap-is-the-explained-variance-not-the-projection.md) refused for PCA's
  migration row.
- **Write all five.** The list reads like one scope and is not: two are exact and not maintained
  anywhere free below `net8.0`, one is present where most callers are, one has no reproducible
  reference, and one needs a solver this repository does not have. Writing them as equals is how the row sat untracked after #442 closed.
- **Write the Gaussian mixture first, since NumFlat proves the demand.** Demand is not the ground;
  reach is. On `net8.0` a caller has it today; below it, a caller has a planar DBSCAN and an
  agglomerative package six years without a release.

## Consequences

- `docs/equivalence.md`'s single row becomes one row per verdict, spectral clustering among them
  with its own reason.
- `docs/migration/sklearn.md` gains the clustering rows: `KMeans` here, `GaussianMixture` and
  k-medoids to NumFlat on `net8.0`, `HDBSCAN` to `HdbscanSharp`, DBSCAN and agglomerative as being
  written, with `Dbscan` and `Aglomera` named beside them.
- `README.md`'s incumbent row for `Lodestar.Cluster` reads from the measurement instead of *not
  measured*.
- `bench/Lodestar.Text.Benchmarks` references Meta.Numerics beside NumFlat, for this comparison
  alone.
