# Performance — Lodestar.Decomposition

What `Lodestar.Decomposition` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Truncated SVD and NMF against ML.NET 5.0.0's `ProjectToPrincipalComponents`

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, 2026-09-14, one run per class. The
class and its agreement check:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438)
section 15, and the decomposition class in
section 16.

A 2,000 × 500 term-document matrix at 2% density, rank 20:

| Row | Mean | Ratio |
| --- | ---: | ---: |
| `TruncatedSvd`, over the sparse matrix | 17.58 ms | 1.00 |
| `Nmf`, capped at 50 iterations | 118.72 ms | 6.76 |
| ML.NET's centred PCA, over the dense twin | **14.85 ms** | 0.85 |

**ML.NET's PCA is 1.18× faster, and it is a different decomposition**: centred and dense against
uncentred and sparse. Section 16 of `bench/README.md` says what is checked instead of agreement, and
decision 0004 why the gap that matters is the explained variance, which ML.NET does not report.

## The variance principal components explain, against NumFlat (issue #701)

Full method and what the pair does and does not compare:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#30-the-variance-principal-components-explain-against-numflat-issue-701).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: one `BenchmarkDotNet` run, **default job**, on 2026-09-13, 8 benchmarks. NumFlat
1.3.4. Both sides were checked to return the same spectrum before either was timed.

| Shape | [`PrincipalComponentVariance`](../../docs/reference/decomposition/factorization/principalcomponentvariance.md) | NumFlat `PrincipalComponentAnalysis` | NumFlat / Lodestar | Allocated, Lodestar | Allocated, NumFlat |
| --- | ---: | ---: | ---: | ---: | ---: |
| 200 × 10 | 14.74 μs | 15.35 μs | 1.04 | 2.38 KB | 1.94 KB |
| 2,000 × 10 | 82.53 μs | 111.83 μs | **1.36** | 2.38 KB | 1.94 KB |
| 2,000 × 50 | 1,977.65 μs | 1,634.81 μs | **0.83** | 42.07 KB | 40.06 KB |
| 100 × 200 | 7,834.02 μs | 9,306.55 μs | **1.19** | 318.27 KB | 628.54 KB |

**This package is faster on three shapes of four and slower on one**, while NumFlat's row also
computes the eigenvectors and the mean. The one it loses is the one where the eigen solve
dominates: 50 × 50 is where a one-sided Jacobi, several sweeps of `O(p³)`, falls behind a
tridiagonal solver. At 10 columns the Gram matrix is most of the work, and centring each row into
a buffer of one row's width keeps it in cache.

The wide block is solved through the 100 × 100 Gram matrix of its rows rather than the 200 × 200
one of its columns, which is where the 1.19 and half the allocation come from.

The path to these numbers is in
[decision 0003](../../docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md): a
Jacobi solve over the whole centred block measured 13× slower than NumFlat at 2,000 × 50 before
the Gram route replaced it. **Below `net8.0` the second row is Meta.Numerics**: NumFlat does not install there and ML.NET
reports no eigenvalue, and [its section](../Lodestar.Stats/performance.md#metanumerics-against-lodestarstats-and-principalcomponentvariance-issue-756)
has the numbers.

## The factorization applied to unseen rows (issue #1124)

Full method and why both losses are run:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#57-the-factorization-applied-to-unseen-rows-against-scikit-learn-issue-1124).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `BenchmarkDotNet` 0.14.0, default job;
2,000 × 500 at 2% density for the fit, 200 unseen rows for the transform.

**There is no .NET incumbent**: neither ML.NET nor NumFlat publishes a non-negative factorization,
so the row that matters is what a batch of unseen rows costs against the fit it is scored on.

| operation | cost | against the fit |
| --- | ---: | ---: |
| [`TruncatedSvd.Fit`](../../docs/reference/decomposition/factorization/truncatedsvd-fit.md), rank 20 | 11.527 ms | — |
| [`Nmf.Fit`](../../docs/reference/decomposition/factorization/nmf-fit.md), rank 20, 50 updates | 66.647 ms | 1.00 |
| [`Nmf.Transform`](../../docs/reference/decomposition/factorization/nmf-transform.md), 200 unseen rows | **2.102 ms** | **0.032** |

**A batch of unseen rows costs about a thirtieth of the fit**, which is the number a caller sizing
a scoring loop needs. It iterates — it is a factorization with `H` held fixed, not a projection —
so it is not free the way
[`TruncatedSvd.Transform`](../../docs/reference/decomposition/factorization/truncatedsvd-transform.md)'s
multiply is; it is also not another fit.

Against `scikit-learn` 1.9.1 through `compare-nmf-transform`, one run of each side, milliseconds
per operation, best of five, `solver='mu'` and `tol=0.0` pinned so both compute the same thing:

| unseen rows | loss | Lodestar | `scikit-learn`, wall / cpu | ratio, wall | ratio, cpu |
| ---: | --- | ---: | ---: | ---: | ---: |
| 100 | Frobenius | 1.264 ms | **0.345** / 0.345 ms | 0.27 | 0.27 |
| 500 | Frobenius | 4.054 ms | **0.705** / 0.705 ms | 0.17 | 0.17 |
| 2,000 | Frobenius | **14.708 ms** | 2.567 / 40.553 ms | 0.17 | **2.75** |
| 100 | Kullback-Leibler | **0.732 ms** | 7.400 / 7.400 ms | **10.11** | **10.11** |
| 500 | Kullback-Leibler | **3.299 ms** | 16.913 / 16.912 ms | **5.13** | **5.13** |
| 2,000 | Kullback-Leibler | **13.015 ms** | 72.064 / 1,007.735 ms | **5.54** | **77.22** |

**The two losses are two different computations, and the table is two different answers.** The
Frobenius update is three dense products; scikit-learn hands them to a parallel BLAS, which is why
it is 5.9× ahead on elapsed time at 2,000 rows and **2.75× behind on processor time** — it spends
40.6 ms of CPU to finish in 2.6 ms. This runs on one thread and spends 14.7 ms of CPU to finish in
14.7 ms.

**The Kullback-Leibler update is where the sparse representation pays.** The ratio `X / WH` is
needed only where `X` is stored, which is 2% of the cells here; scikit-learn densifies `W H` and
pays for all of them. That is 5.5× on elapsed time and **77× on processor time** at 2,000 rows —
it burns a full second of CPU for 72 ms of wall clock.

A caller choosing between the two losses on a sparse corpus should know that the one this package
is faster at is also the one a term-document matrix wants: Kullback-Leibler fits a Poisson noise
model, which is what counts are.
