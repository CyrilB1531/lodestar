# Performance — Lodestar.Cluster

What `Lodestar.Cluster` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Weighted k-means, restarts and weighted DBSCAN against scikit-learn (issue #1163)

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#65-weighted-k-means-restarts-and-weighted-dbscan-against-scikit-learn-issue-1163).
No .NET library weighs a sample in either algorithm, so scikit-learn 1.9.1 on numpy 2.5.3 is the
incumbent. Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical
cores, .NET 10.0.12, under the repository's machine lock on 2026-09-26: the Python side 07:07 UTC,
the C# side 07:14 UTC. Milliseconds per call, best of five.

| operation | rows | Lodestar | scikit-learn, wall / cpu | ratio, cpu | ratio, wall |
| --- | ---: | ---: | ---: | ---: | ---: |
| weighted k-means, one start (8 features, 16 clusters) | 10,000 | **2.58 ms** | 1.87 / 14.5 ms | **5.61** | 0.72 |
| weighted k-means, four starts | 10,000 | **11.5 ms** | 4.70 / 35.6 ms | **3.11** | 0.41 |
| weighted k-means, one start | 100,000 | **26.1 ms** | 13.5 / 70.1 ms | **2.67** | 0.52 |
| weighted k-means, four starts | 100,000 | **113 ms** | 27.4 / 169 ms | **1.49** | 0.24 |
| weighted DBSCAN (2 features) | 5,000 | **5.23 ms** | 16.9 / 16.9 ms | **2.99** | 3.23 |
| weighted DBSCAN | 20,000 | **69.1 ms** | 114 / 114 ms | **1.59** | 1.65 |

**How it reads.** Ahead on every row in processor time. k-means is behind in elapsed time because
scikit-learn spreads Lloyd's loop over OpenMP threads, spending five to eight times its elapsed
time to do it, where this package runs on one core; DBSCAN is single-threaded on both sides, and
ahead on both clocks.

**Two kernels paid for it.** The first reading was behind on two rows: four starts at 100,000 rows
(0.92) and DBSCAN at 20,000 (0.48). The E-step now puts one centre per vector lane, each lane
summing in the scalar loop's order, so every label is unchanged to the bit and the unweighted fit
is 1.6× faster than before; DBSCAN's neighbour search sorts the rows along their widest feature and
stops each scan where that feature alone is out of reach, instead of testing every pair.

## k-means against NumFlat and Meta.Numerics (issue #681)

Full method, the two classes and why only one is like-for-like:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#34-k-means-against-numflat-and-metanumerics-issue-681).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: one `BenchmarkDotNet` 0.14.0 run, **default job**, on 2026-09-14, 15 benchmarks.
NumFlat 1.3.4, Meta.Numerics 4.2.0. Every pair was checked to return the same centres before either
side was timed; the largest difference was zero.

**Lloyd's iterations from the same centres** — the like-for-like pair:

| Shape (rows × features × k) | Iterations | [`KMeans.Fit`](../../docs/reference/cluster/partitioning/kmeans-fit.md) | NumFlat `KMeans.Update` | NumFlat / Lodestar | Allocated, Lodestar | Allocated, NumFlat |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 10,000 × 2 × 8 | 101 | 19.21 ms | 70.34 ms | **3.66** | 98.92 KB | 96.40 KB |
| 10,000 × 16 × 16 | 4 | 6.51 ms | 9.87 ms | **1.52** | 88.70 KB | 13.58 KB |
| 50,000 × 8 × 32 | 8 | 64.02 ms | 127.26 ms | **1.99** | 410.24 KB | 36.39 KB |

**This package is ahead on every shape, 1.52× to 3.66×**, and allocates more on the two wider ones:
its current and previous labels are one `int` per row each, 80 KB at 10,000 rows, where NumFlat's
allocation grows with the iteration count instead — which is why the 101-iteration row is level.

**A whole fit, k-means++ included**, under each library's own stopping rule:

| Shape | [`KMeans.Fit`](../../docs/reference/cluster/partitioning/kmeans-fit.md) | NumFlat `KMeans` | Meta.Numerics `MeansClustering` | NumFlat / Lodestar | Meta.Numerics / Lodestar |
| --- | ---: | ---: | ---: | ---: | ---: |
| 10,000 × 2 × 8 | 0.41 ms | 3.10 ms | 1.69 ms | 7.49 | 4.09 |
| 10,000 × 16 × 16 | 3.93 ms | 21.53 ms | 54.62 ms | 5.48 | 13.90 |
| 50,000 × 8 × 32 | 19.98 ms | 260.88 ms | 408.07 ms | 13.06 | 20.42 |

**Read this table as what a caller pays, not as a kernel ratio.** On these well-separated blobs
scikit-learn's scaled tolerance stops this package after one or two iterations, and neither incumbent
reports how many it ran. The second table is the one that compares arithmetic; this one says that a
default fit here is 4× to 20× cheaper, and part of that is when it decides to stop.

**Meta.Numerics is the comparison that holds below `net8.0`**, and the only one: NumFlat does not
install there.

## DBSCAN against NumFlat and `Dbscan` (issue #759)

Full method, the two classes and what the agreement check refused:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#48-dbscan-against-numflat-and-dbscan-issue-759).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET 10.0.12 runtime, AVX-512. Window: one
`BenchmarkDotNet` 0.14.0 run, **`--job short`**, on 2026-09-16, 10 benchmarks. NumFlat 1.3.4,
`Dbscan` 3.0.0. Every shape was checked to return the same *partition* — cluster numbering
ignored — on all the libraries in its table before any was timed.

**Planar, where all three compete:**

| Shape (rows × features × blobs) | radius | [`Dbscan.Fit`](../../docs/reference/cluster/partitioning/dbscan-fit.md) | NumFlat `DbScan.Fit` | `Dbscan` 3.0.0 | NumFlat / Lodestar | Dbscan / Lodestar |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 5,000 × 2 × 5 | 1.0 | 63.70 ms | 287.31 ms | 157.27 ms | **4.51** | **2.47** |
| 20,000 × 2 × 10 | 1.0 | 889.93 ms | 9,716.85 ms | 2,365.60 ms | **10.92** | **2.66** |

**Above two features, where only NumFlat follows:**

| Shape | radius | [`Dbscan.Fit`](../../docs/reference/cluster/partitioning/dbscan-fit.md) | NumFlat `DbScan.Fit` | NumFlat / Lodestar |
| --- | ---: | ---: | ---: | ---: |
| 10,000 × 8 × 8 | 3.0 | 413.80 ms | 1,536.70 ms | **3.71** |
| 5,000 × 16 × 8 | 5.0 | 193.20 ms | 518.30 ms | **2.68** |

**Ahead on every shape, 2.47× to 10.92×, and allocating 2.89× to 5.46× less**: 13.98 MB against
NumFlat's 66.59 MB and `Dbscan`'s 40.40 MB at 5,000 planar points, 111.97 MB against 531.88 MB and
372.11 MB at 20,000.

**`Dbscan` 3.0.0 has no row in the second table because it has no entry point there.** Its `Point`
carries `X` and `Y` and nothing else, which is
[decision 0004](../../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)'s whole
argument for writing this class — the gap is reach below `net8.0`, and the speed is a second
finding rather than the claim.

**One measured defect in an incumbent, and it is not ours.** NumFlat 1.3.4 marks a non-core sample
noise permanently when the ascending scan reaches it before any cluster has been grown, where the
original algorithm and scikit-learn let a later expansion take it as a border sample. Nine points
in one dimension separate them, and at sixteen features with a radius of 3 it costs 1,017 rows of
5,000 — where this package matches scikit-learn exactly. `bench/README.md` section 48 has the
reproducer and the alternative explanation that was tested and refused.

## Agglomerative clustering against `Aglomera` (issue #760)

Full method, what was measured before any code and why the benchmark runs on blobs:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#49-agglomerative-clustering-against-aglomera-issue-760).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET 10.0.12 runtime, AVX-512. Window: one
`BenchmarkDotNet` 0.14.0 run, **`--job short`**, on 2026-09-16, 16 benchmarks, with nothing else
running — a first run that shared the machine with the oracle generator was discarded. `Aglomera`
1.1.1. Every combination was checked to return the same merge heights in order, and the same
partition at the cut, before either side was timed.

| samples | linkage | [`AgglomerativeClustering.Fit`](../../docs/reference/cluster/partitioning/agglomerativeclustering-fit.md) | `Aglomera` `GetClustering` | Aglomera / Lodestar | allocated, Lodestar | allocated, Aglomera |
| ---: | --- | ---: | ---: | ---: | ---: | ---: |
| 500 | ward | 2.03 ms | 48.91 ms | **24.14** | 1.04 MB | 64.19 MB |
| 500 | complete | 1.98 ms | 61.70 ms | **31.21** | 1.04 MB | 83.11 MB |
| 500 | average | 1.92 ms | 56.28 ms | **29.37** | 1.04 MB | 59.88 MB |
| 500 | single | 0.45 ms | 87.45 ms | **194.69** | 0.06 MB | 81.29 MB |
| 1,500 | ward | 17.44 ms | 1,110.66 ms | **63.70** | 8.97 MB | 573.62 MB |
| 1,500 | complete | 16.98 ms | 1,209.18 ms | **71.23** | 8.97 MB | 747.36 MB |
| 1,500 | average | 17.28 ms | 1,224.95 ms | **70.91** | 8.97 MB | 537.15 MB |
| 1,500 | single | 3.53 ms | 2,081.80 ms | **589.97** | 0.18 MB | 732.69 MB |

**Ahead on every row, 24× to 590×, and the gap widens with the sample count** — the three
nearest-neighbour-chain linkages go from 24–31× at 500 to 64–71× at 1,500. Single linkage holds no
distance matrix at all, 185 KB where `Aglomera` allocates 733 MB.

**These rows are the only ones where the comparison is fair.** Blobs have no tied merge heights; on
data that does, `Aglomera` breaks ties the other way and can build a different tree, and its Ward
height is reported as `d²/2`. The speed is a second finding — the reason this class exists is that
`Aglomera`'s answer is not scikit-learn's.
