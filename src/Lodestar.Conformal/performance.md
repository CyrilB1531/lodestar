# Performance — Lodestar.Conformal

What `Lodestar.Conformal` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Cross-conformal intervals against MAPIE (issue #1159)

Full method:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#63-cross-conformal-intervals-against-mapie-issue-1159).
**No .NET library computes a conformal interval**, so MAPIE 1.5.0 on numpy 2.5.3 is the only
incumbent. Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical
cores, .NET 10.0.12, under the repository's machine lock: the Python side 2026-09-25 13:50 to 13:52
UTC, the C# side 13:53 to 13:54 UTC, after the selection change below. Milliseconds for the
intervals at 500 test points, best of five: ten unshuffled folds, or thirty bootstrap models.

| training samples | intervals | Lodestar | MAPIE, wall / cpu | ratio, cpu |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | CV+, absolute | **2.12 ms** | 24.9 / 387 ms | **182.6** |
| 1,000 | min-max, absolute | **1.33 ms** | 4.26 / 67.1 ms | **50.4** |
| 1,000 | CV+, gamma | **2.39 ms** | 26.1 / 403 ms | **168.3** |
| 1,000 | jackknife-after-bootstrap, plus | **8.57 ms** | 26.2 / 404 ms | **47.2** |
| 10,000 | CV+, absolute | **19.8 ms** | 83.2 / 1,181 ms | **59.6** |
| 10,000 | min-max, absolute | **13.7 ms** | 31.7 / 449 ms | **32.7** |
| 10,000 | CV+, gamma | **29.9 ms** | 86.9 / 1,205 ms | **40.3** |
| 10,000 | jackknife-after-bootstrap, plus | **85.3 ms** | 86.1 / 1,222 ms | **14.3** |

**How it reads.** Ahead on every row in processor time, and in elapsed time by 1.01× to 11.8×.
MAPIE builds the whole test-by-training matrix and reduces it on numpy's threads, fourteen times more
processor time than elapsed; this runs one test point at a time on one thread. The MAPIE column
includes each model's `predict` on the test block, 0.057 ms at both sizes, which this package is
handed instead. **The jackknife-after-bootstrap is the close row at 10,000 samples**: each test
point averages thirty models over each training sample's out-of-bag mask, `n × M` work that MAPIE
does once as a matrix product.

**Each side of an interval reads one rank**, and it was first read by sorting: 373 ms for CV+ at
10,000 samples, 0.22× MAPIE's elapsed time. Selecting the rank instead, in linear time, took it to
19.8 ms.
