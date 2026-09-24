# Performance — Lodestar.Stats.TimeSeries

What `Lodestar.Stats.TimeSeries` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Stationarity and seasonal decomposition against Cortex.TimeSeries (issue #671)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#35-stationarity-and-seasonal-decomposition-against-cortextimeseries-issue-671).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-15. `Cortex.TimeSeries` 1.1.0. Each
pair returned the same statistic before it was timed.

| n | function | [`Lodestar.Stats.TimeSeries`](../../docs/reference/stats-timeseries/stationarity-tests.md) | `Cortex.TimeSeries` | Cortex / Lodestar | Allocated, Lodestar | Allocated, Cortex |
| ---: | --- | ---: | ---: | ---: | ---: | ---: |
| 200 | ADF, lag 4 | **6.987 μs** | 15.819 μs | **2.26** | 20.68 KB | 16.18 KB |
| 200 | KPSS, level | 2.219 μs | 2.328 μs | 1.05 | 1.69 KB | 3.43 KB |
| 200 | decomposition, period 12 | **0.810 μs** | 1.499 μs | **1.85** | 6.50 KB | 8.16 KB |
| 2,000 | ADF, lag 4 | **101.382 μs** | 181.199 μs | **1.79** | 203.51 KB | 142.76 KB |
| 2,000 | KPSS, level | 59.554 μs | 60.223 μs | 1.01 | 15.75 KB | 31.55 KB |
| 2,000 | decomposition, period 12 | **8.253 μs** | 15.391 μs | **1.86** | 62.75 KB | 78.48 KB |

**The augmented Dickey-Fuller test is 1.79× to 2.26× faster than Cortex's.** Its lag search fits
through
[`OrdinaryLeastSquares.Estimate`](../../docs/reference/stats-regression/ols/ordinaryleastsquares-estimate.md),
which applies the reflections to the response and stops at the coefficients, their standard errors
and the residual sum of squares, rather than forming Q explicitly and pricing variance inflation
factors and a Student quantile per coefficient to hand back one t statistic. It allocates more than
Cortex at 2,000 points: the design and its column-major copy. The p-value stays what the test is
for — Cortex's is clamped at `0.01`, where MacKinnon's surface gives `1.1e-9` to `9.5e-4` on the
regressions checked.

**The decomposition is 1.85× faster**: the moving average is a running
sum over each window's interior rather than a weighted pass over the whole window, `O(n)` against
`O(n·period)`. **KPSS is level**, with half the allocation; its cost is the lagged products of the
long-run variance, which both sides compute.

## Lodestar.Stats' serial-correlation diagnostics against Cortex.TimeSeries (issue #617)

Full method, and why the partial autocorrelations of the two libraries differ:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#28-lodestarstatstimeseriess-serial-correlation-diagnostics-against-cortextimeseries-issue-617).

Machine: the AMD Ryzen 7 8700G named above. Window: one `BenchmarkDotNet` run, **default job**, on 2026-09-13,
12 benchmarks, 20 lags throughout.

| Method | SampleSize | Mean | Ratio | Allocated |
| --- | ---: | ---: | ---: | ---: |
| `LodestarAutocorrelation` | 200 | 18.582 μs | 1.00 | 1,000 B |
| `CortexAutocorrelation` | 200 | 7.308 μs | 0.39 | 192 B |
| `LodestarPartialAutocorrelation` | 200 | 18.829 μs | 1.01 | 1,192 B |
| `CortexPartialAutocorrelation` | 200 | 7.696 μs | 0.41 | 768 B |
| `LodestarLjungBox` | 200 | 8.407 μs | 0.45 | 720 B |
| `CortexLjungBox` | 200 | 7.375 μs | 0.40 | 224 B |
| `LodestarAutocorrelation` | 2,000 | 88.121 μs | 1.00 | 1,000 B |
| `CortexAutocorrelation` | 2,000 | 76.709 μs | 0.87 | 192 B |
| `LodestarPartialAutocorrelation` | 2,000 | 88.041 μs | 1.00 | 1,192 B |
| `CortexPartialAutocorrelation` | 2,000 | 77.123 μs | 0.88 | 768 B |
| `LodestarLjungBox` | 2,000 | 77.513 μs | 0.88 | 720 B |
| `CortexLjungBox` | 2,000 | 76.915 μs | 0.87 | 224 B |

**The gap is one function call.** Lodestar's ACF and PACF are 11.3 and 11.1 μs slower at 200
points and 11.4 and 10.9 μs slower at 2,000 — constant while the series grows tenfold, which a
kernel difference could not be. Both compute the confidence band `Cortex.TimeSeries` does not,
through one `NormalQuantile` call, and the Ljung-Box pair, which has no band, is 1.14× at 200
points and **level at 2,000**. Subtracting the 11.5 μs quantile leaves 7.1 μs against Cortex's
7.3 at 200 points and 76.6 μs against 76.7 at 2,000. The allocation difference is the band and
the result record that carries it, and
[#709](https://github.com/CyrilB1531/lodestar/issues/709) confirmed it by removing that cost:
without the bisection the pairs are level.

## The vector autoregression against statsmodels (issue #786)

Full method and what agrees:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#43-the-vector-autoregression-against-statsmodels-issue-786).
Nothing in .NET estimates a VAR, so the incumbent is `statsmodels` 0.15.0 through `compare-var`.
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16; one run of each side. A two-variable series at lag 2, milliseconds per fit, best of five.

| n | [`VectorAutoregression.Fit`](../../docs/reference/stats-timeseries/var/vectorautoregression-fit.md) | `statsmodels`, wall / cpu | ratio, wall |
| ---: | ---: | ---: | ---: |
| 1,000 | **0.042 ms** | 1.263 / 1.263 ms | **30.23** |
| 10,000 | **0.485 ms** | 8.565 / 8.564 ms | **17.65** |
| 100,000 | **5.391 ms** | 79.616 / 79.609 ms | **14.77** |

What the fit costs by shape, from `VectorAutoregressionBenchmarks`, `BenchmarkDotNet` 0.14.0 default job:

| observations | variables | lags | mean | Allocated |
| ---: | ---: | ---: | ---: | ---: |
| 500 | 2 | 1 | 14.76 μs | 60.53 KB |
| 500 | 2 | 4 | 49.28 μs | 132.84 KB |
| 500 | 5 | 1 | 72.24 μs | 206.76 KB |
| 500 | 5 | 4 | 422.41 μs | 590.05 KB |
| 5,000 | 2 | 1 | 244.97 μs | 587.95 KB |
| 5,000 | 2 | 4 | 829.14 μs | 1,293.21 KB |
| 5,000 | 5 | 1 | 911.36 μs | 2,000.01 KB |
| 5,000 | 5 | 4 | 4,701.02 μs | 5,548.08 KB |

Both axes multiply: a system of `K` variables at lag `p` fits `K` least squares over a design of `1 + K·p` columns.
