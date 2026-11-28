# Nightly benchmark run

<!-- nightly-baseline: 52d30eae62e4222014d789d30dd0de8c5e07bb4d -->
<!-- nightly-owed: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
<!-- nightly-cut: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `52d30eae62e4222014d789d30dd0de8c5e07bb4d`
- Previous run: `52d30eae62e4222014d789d30dd0de8c5e07bb4d`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `StationarityBenchmarks`
- `OlsBenchmarks`
- `GlsBenchmarks`
- `WeightedLeastSquaresBenchmarks`
- `RobustCovarianceBenchmarks`
- `HacClusterBenchmarks`
- `GlmBenchmarks`
- `GlmPoissonBenchmarks`
- `GlmOffsetBenchmarks`
- `MultinomialLogitBenchmarks`
- `VectorAutoregressionBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `LeastSquaresRoutingBenchmarks`

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **16.69 μs** |  **3.872 μs** | **0.212 μs** |  **1.00** |    **0.02** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  26.00 μs |  2.129 μs | 0.117 μs |  1.56 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **19.68 μs** |  **0.331 μs** | **0.018 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          |  51.19 μs |  4.384 μs | 0.240 μs |  2.60 |    0.01 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **189.51 μs** |  **6.643 μs** | **0.364 μs** |  **1.00** |    **0.00** |  **6.5918** |  **0.9766** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 192.51 μs |  3.091 μs | 0.169 μs |  1.02 |    0.00 | 53.9551 | 16.6016 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **194.58 μs** | **32.352 μs** | **1.773 μs** |  **1.00** |    **0.01** | **10.4980** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 363.83 μs | 55.151 μs | 3.023 μs |  1.87 |    0.02 | 77.1484 | 33.6914 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **141.9 μs** |    **48.16 μs** |   **2.64 μs** |   **3.4180** |   **0.2441** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **1,532.9 μs** |    **30.76 μs** |   **1.69 μs** |  **99.6094** |  **99.6094** |  **99.6094** |  **550.45 KB** |
| **Fit**    | **100000**     | **15,805.2 μs** | **1,972.05 μs** | **108.09 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.04 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **14.89 μs** |   **7.796 μs** |  **0.427 μs** |  **1.00** |    **0.04** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    29.65 μs |   0.933 μs |  0.051 μs |  1.99 |    0.05 |   1.2512 |   0.0305 |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **1,931.96 μs** |  **40.375 μs** |  **2.213 μs** |  **1.00** |    **0.00** | **332.0313** | **332.0313** | **332.0313** | **1097.84 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 3,763.69 μs | 440.667 μs | 24.154 μs |  1.95 |    0.01 | 554.6875 | 554.6875 | 554.6875 | 1878.27 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **153.0 μs** | **39.94 μs** | **2.19 μs** | **6.5918** | **0.9766** | **110.56 KB** |
| **Fit**    | **50000**     | **151.3 μs** |  **9.01 μs** | **0.49 μs** | **6.5918** | **0.9766** | **110.56 KB** |
| **Fit**    | **5000000**   | **159.8 μs** |  **8.95 μs** | **0.49 μs** | **6.5918** | **0.9766** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |     **27.54 μs** |      **0.777 μs** |     **0.043 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     28.21 μs |     11.177 μs |     0.613 μs |  1.02 |    0.02 |   2.0142 |   0.1221 |        - |    33.3 KB |        1.23 |
|              |            |              |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |    **367.71 μs** |     **34.830 μs** |     **1.909 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |  1,541.82 μs |      4.980 μs |     0.273 μs |  4.19 |    0.02 |  99.6094 |  99.6094 |  99.6094 |  370.39 KB |        1.10 |
|              |            |              |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |  **3,019.07 μs** |  **2,498.607 μs** |   **136.957 μs** |  **1.00** |    **0.06** | **324.2188** | **324.2188** | **324.2188** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        | 10,677.69 μs |    621.600 μs |    34.072 μs |  3.54 |    0.14 | 312.5000 | 296.8750 | 296.8750 | 2228.43 KB |        1.11 |
|              |            |              |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       | **26,822.83 μs** | **25,377.080 μs** | **1,391.003 μs** |  **1.00** |    **0.06** | **281.2500** | **281.2500** | **281.2500** | **7924.26 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 73,473.96 μs | 17,240.685 μs |   945.020 μs |  2.74 |    0.12 |        - |        - |        - | 8866.22 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **19.430 μs** |   **5.906 μs** | **0.3237 μs** |  **1.00** |    **0.02** |   **0.1526** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |     5.614 μs |   2.074 μs | 0.1137 μs |  0.29 |    0.01 |   0.5112 |        - |        - |   8.42 KB |        3.18 |
| Hac       | 100        | 4    |    11.405 μs |   3.793 μs | 0.2079 μs |  0.59 |    0.01 |   0.7629 |   0.0153 |        - |   12.6 KB |        4.76 |
| Cluster   | 100        | 4    |     5.036 μs |   2.197 μs | 0.1204 μs |  0.26 |    0.01 |   0.5722 |   0.0153 |        - |   9.46 KB |        3.57 |
|           |            |      |              |            |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **257.281 μs** | **114.628 μs** | **6.2832 μs** |  **1.00** |    **0.03** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   523.067 μs |  44.248 μs | 2.4254 μs |  2.03 |    0.04 | 124.0234 | 124.0234 | 124.0234 | 472.57 KB |        5.91 |
| Hac       | 10000      | 4    | 1,149.498 μs |  32.281 μs | 1.7694 μs |  4.47 |    0.09 | 248.0469 | 248.0469 | 248.0469 | 863.55 KB |       10.80 |
| Cluster   | 10000      | 4    |   465.087 μs |  18.803 μs | 1.0307 μs |  1.81 |    0.04 | 124.5117 | 124.5117 | 124.5117 | 564.93 KB |        7.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------------ |----------- |--------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |      **89.34 μs** |      **5.983 μs** |     **0.328 μs** |  **1.00** |    **0.00** |   **49.9268** |   **49.9268** |   **49.9268** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     108.20 μs |      6.728 μs |     0.369 μs |  1.21 |    0.01 |    1.9531 |         - |         - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     351.05 μs |      9.802 μs |     0.537 μs |  3.93 |    0.01 |  190.4297 |  190.4297 |  190.4297 |   705.88 KB |        3.75 |
|                   |            |               |               |              |       |         |           |           |           |             |             |
| **Lodestar_Estimate** | **250**        |  **32,748.67 μs** |  **7,197.339 μs** |   **394.510 μs** |  **1.00** |    **0.01** |  **312.5000** |  **312.5000** |  **312.5000** |  **8865.73 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 148,737.20 μs | 47,468.572 μs | 2,601.913 μs |  4.54 |    0.08 |         - |         - |         - |  3499.13 KB |        0.39 |
| MathNet_Qr        | 250        | 323,800.43 μs | 35,942.713 μs | 1,970.141 μs |  9.89 |    0.11 | 1000.0000 | 1000.0000 | 1000.0000 | 32694.46 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |    **72.48 μs** |   **9.976 μs** |  **0.547 μs** |  **1.00** |    **0.01** |   **1.0986** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   470.35 μs |  16.921 μs |  0.928 μs |  6.49 |    0.04 |  89.3555 |   8.3008 |  1466.83 KB |       76.23 |
|          |            |             |            |           |       |         |          |          |             |             |
| **Lodestar** | **2000**       |   **717.19 μs** |  **41.692 μs** |  **2.285 μs** |  **1.00** |    **0.00** |   **7.8125** |   **0.9766** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 3,796.46 μs | 313.559 μs | 17.187 μs |  5.29 |    0.03 | 734.3750 | 363.2813 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|---------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **19.29 μs** |   **0.120 μs** | **0.007 μs** |  **1.00** |    **0.00** |   **0.1526** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   129.37 μs |   7.010 μs | 0.384 μs |  6.71 |    0.02 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    15.06 μs |   1.970 μs | 0.108 μs |  0.78 |    0.00 |   2.1667 |   0.0610 |        - |   35.59 KB |       13.44 |
|              |            |             |            |          |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **257.90 μs** |   **5.499 μs** | **0.301 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 2,425.44 μs |  80.975 μs | 4.439 μs |  9.40 |    0.02 | 214.8438 | 195.3125 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      |   941.07 μs | 127.921 μs | 7.012 μs |  3.65 |    0.02 | 443.3594 | 443.3594 | 443.3594 | 1766.58 KB |       22.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-----------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |  **19.289 μs** |  **0.7931 μs** | **0.0435 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |   **5.383 μs** |  **0.2108 μs** | **0.0116 μs** |   **0.5112** |   **0.0076** |        **-** |   **8.38 KB** |
| **Fit**    | **100**        | **Hc1**        |   **5.543 μs** |  **3.2141 μs** | **0.1762 μs** |   **0.5112** |   **0.0076** |        **-** |   **8.38 KB** |
| **Fit**    | **100**        | **Hc2**        |   **6.226 μs** |  **0.2575 μs** | **0.0141 μs** |   **0.5569** |   **0.0076** |        **-** |   **9.18 KB** |
| **Fit**    | **100**        | **Hc3**        |   **6.314 μs** |  **2.4425 μs** | **0.1339 μs** |   **0.5569** |   **0.0076** |        **-** |   **9.18 KB** |
| **Fit**    | **10000**      | **Nonrobust**  | **255.062 μs** | **27.9803 μs** | **1.5337 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        | **524.291 μs** | **36.0203 μs** | **1.9744 μs** | **124.0234** | **124.0234** | **124.0234** | **472.52 KB** |
| **Fit**    | **10000**      | **Hc1**        | **529.220 μs** |  **6.0925 μs** | **0.3339 μs** | **124.0234** | **124.0234** | **124.0234** | **472.52 KB** |
| **Fit**    | **10000**      | **Hc2**        | **598.755 μs** | **34.4742 μs** | **1.8896 μs** | **124.0234** | **124.0234** | **124.0234** | **550.67 KB** |
| **Fit**    | **10000**      | **Hc3**        | **590.298 μs** | **29.6350 μs** | **1.6244 μs** | **124.0234** | **124.0234** | **124.0234** | **550.67 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **3,665.6 ns** |  **1,999.80 ns** |   **109.62 ns** |  **1.00** |    **0.04** |   **1.4648** |   **0.0496** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  13,828.1 ns |  1,192.58 ns |    65.37 ns |  3.77 |    0.10 |   4.0588 |   0.5646 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |  12,831.9 ns |  1,055.78 ns |    57.87 ns |  3.50 |    0.09 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   1,944.2 ns |    481.85 ns |    26.41 ns |  0.53 |    0.01 |   0.1030 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   1,909.0 ns |     59.81 ns |     3.28 ns |  0.52 |    0.01 |   0.2098 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |     855.9 ns |     52.88 ns |     2.90 ns |  0.23 |    0.01 |   0.3977 |   0.0048 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   1,415.7 ns |    712.55 ns |    39.06 ns |  0.39 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |              |              |             |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |  **67,116.0 ns** |  **3,930.54 ns** |   **215.45 ns** |  **1.00** |    **0.00** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 571,376.9 ns | 37,920.53 ns | 2,078.55 ns |  8.51 |    0.04 | 249.0234 | 249.0234 | 249.0234 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 151,592.2 ns |  3,558.76 ns |   195.07 ns |  2.26 |    0.01 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  46,924.1 ns |  2,096.68 ns |   114.93 ns |  0.70 |    0.00 |   0.9155 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  47,092.9 ns |    577.67 ns |    31.66 ns |  0.70 |    0.00 |   1.8921 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |   7,895.6 ns |     92.92 ns |     5.09 ns |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  13,039.2 ns |  1,627.37 ns |    89.20 ns |  0.19 |    0.00 |   4.7913 |   0.4730 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean         | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |-------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |     **7.165 μs** |  **0.7396 μs** | **0.0405 μs** |   **2.9602** |   **0.3510** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **17.555 μs** |  **0.2922 μs** | **0.0160 μs** |   **5.8594** |   **1.4343** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **24.995 μs** |  **8.8387 μs** | **0.4845 μs** |   **6.6833** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |    **72.215 μs** |  **2.2938 μs** | **0.1257 μs** |  **14.2822** |   **7.0801** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **114.525 μs** |  **7.1358 μs** | **0.3911 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **323.472 μs** | **12.4347 μs** | **0.6816 μs** | **222.1680** | **222.1680** | **222.1680** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **396.520 μs** | **31.6100 μs** | **1.7326 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **1,246.943 μs** | **42.5822 μs** | **2.3341 μs** | **498.0469** | **498.0469** | **498.0469** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **23.888 μs** |     **8.9459 μs** |   **0.4904 μs** |  **1.00** |    **0.03** |    **0.1831** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |      8.469 μs |     0.9952 μs |   0.0545 μs |  0.35 |    0.01 |    3.7079 |    0.1831 |         - |    58.31 KB |       17.00 |
|              |            |               |               |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **73.186 μs** |     **6.5893 μs** |   **0.3612 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |     44.431 μs |     2.5954 μs |   0.1423 μs |  0.61 |    0.00 |   33.2031 |   13.9160 |         - |   508.39 KB |       29.06 |
|              |            |               |               |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **523.620 μs** |   **261.4218 μs** |  **14.3294 μs** |  **1.00** |    **0.03** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.51 KB** |        **1.00** |
| MathNet_Wls  | 20000      |    909.960 μs |   270.7377 μs |  14.8400 μs |  1.74 |    0.05 | 1001.9531 | 1000.0000 | 1000.0000 |  5024.81 KB |       31.70 |
|              |            |               |               |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **5,162.151 μs** |   **475.4994 μs** |  **26.0637 μs** |  **1.00** |    **0.01** |  **117.1875** |  **117.1875** |  **117.1875** |  **1564.86 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 11,370.217 μs | 4,322.0666 μs | 236.9071 μs |  2.20 |    0.04 |  734.3750 |  734.3750 |  734.3750 | 50020.21 KB |       31.96 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `glm`
- `iv`
- `panel`
- `ols`

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.203 | 1.367 | 6.72x | 0.203 | 1.366 | 6.72x |
| glm_gamma_n1000 | 0.248 | 1.722 | 6.95x | 0.248 | 1.722 | 6.95x |
| glm_poisson_exposure_n1000 | 0.210 | 1.591 | 7.59x | 0.210 | 1.591 | 7.59x |
| mnlogit_n1000 | 0.432 | 6.836 | 15.84x | 0.432 | 6.835 | 15.84x |
| glm_negative_binomial_n10000 | 1.801 | 5.579 | 3.10x | 1.803 | 5.578 | 3.09x |
| glm_gamma_n10000 | 2.085 | 6.494 | 3.11x | 2.088 | 6.494 | 3.11x |
| glm_poisson_exposure_n10000 | 2.248 | 6.264 | 2.79x | 2.251 | 6.264 | 2.78x |
| mnlogit_n10000 | 4.290 | 36.286 | 8.46x | 4.293 | 36.283 | 8.45x |
| glm_negative_binomial_n100000 | 19.208 | 68.349 | 3.56x | 19.497 | 273.095 | 14.01x |
| glm_gamma_n100000 | 22.144 | 69.734 | 3.15x | 22.445 | 272.392 | 12.14x |
| glm_poisson_exposure_n100000 | 30.402 | 62.986 | 2.07x | 31.044 | 251.328 | 8.10x |
| mnlogit_n100000 | 43.794 | 367.513 | 8.39x | 44.196 | 864.479 | 19.56x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 0.846 | 25.882 | 30.60x | 0.845 | 25.880 | 30.61x |
| liml_robust_n1000 | 0.926 | 25.974 | 28.05x | 0.926 | 25.972 | 28.05x |
| gmm_robust_n1000 | 0.854 | 25.744 | 30.15x | 0.854 | 25.741 | 30.15x |
| 2sls_kernel_n1000 | 6.190 | 27.277 | 4.41x | 6.189 | 27.276 | 4.41x |
| 2sls_clustered_n1000 | 0.831 | 26.794 | 32.24x | 0.831 | 26.793 | 32.24x |
| 2sls_robust_n10000 | 8.992 | 121.711 | 13.53x | 9.244 | 243.444 | 26.34x |
| liml_robust_n10000 | 9.774 | 126.144 | 12.91x | 10.065 | 252.383 | 25.07x |
| gmm_robust_n10000 | 9.092 | 118.315 | 13.01x | 9.349 | 236.551 | 25.30x |
| 2sls_kernel_n10000 | 68.803 | 132.905 | 1.93x | 69.046 | 265.744 | 3.85x |
| 2sls_clustered_n10000 | 8.457 | 132.629 | 15.68x | 8.666 | 265.371 | 30.62x |
| 2sls_robust_n100000 | 104.909 | 1498.024 | 14.28x | 107.406 | 5322.371 | 49.55x |
| liml_robust_n100000 | 119.956 | 1519.161 | 12.66x | 124.401 | 5428.875 | 43.64x |
| gmm_robust_n100000 | 113.940 | 1416.604 | 12.43x | 118.191 | 5105.678 | 43.20x |
| 2sls_kernel_n100000 | 2700.106 | 1984.029 | 0.73x | 2702.227 | 7190.777 | 2.66x |
| 2sls_clustered_n100000 | 104.533 | 1563.086 | 14.95x | 105.964 | 5714.815 | 53.93x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.047 | 1.151 | 24.26x | 0.047 | 1.151 | 24.26x |
| ols_hac_n1000 | 0.095 | 1.415 | 14.82x | 0.095 | 1.414 | 14.82x |
| ols_cluster_n1000 | 0.034 | 1.442 | 43.01x | 0.034 | 1.442 | 43.00x |
| ols_summary_n10000 | 0.251 | 5.803 | 23.08x | 0.251 | 5.802 | 23.08x |
| ols_hac_n10000 | 0.955 | 6.571 | 6.88x | 0.957 | 6.570 | 6.86x |
| ols_cluster_n10000 | 0.339 | 6.846 | 20.22x | 0.339 | 6.845 | 20.18x |
| ols_summary_n100000 | 2.462 | 64.615 | 26.25x | 2.476 | 257.169 | 103.87x |
| ols_hac_n100000 | 10.256 | 71.987 | 7.02x | 10.642 | 285.448 | 26.82x |
| ols_cluster_n100000 | 4.206 | 73.232 | 17.41x | 4.405 | 290.496 | 65.95x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4', 'pandas': '2.3.3'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.135 | 9.577 | 71.17x | 0.135 | 9.576 | 71.17x |
| fe_twoway_clustered_n1000 | 0.218 | 14.839 | 67.93x | 0.218 | 14.838 | 67.92x |
| fe_kernel_n1000 | 0.141 | 9.814 | 69.46x | 0.141 | 9.814 | 69.46x |
| between_robust_n1000 | 0.079 | 9.203 | 116.39x | 0.079 | 9.202 | 116.39x |
| first_difference_robust_n1000 | 0.154 | 10.725 | 69.53x | 0.154 | 10.724 | 69.53x |
| random_effects_n1000 | 0.139 | 12.105 | 86.98x | 0.139 | 12.104 | 86.98x |
| fe_entity_n10000 | 1.479 | 14.312 | 9.68x | 1.538 | 14.311 | 9.31x |
| fe_twoway_clustered_n10000 | 2.664 | 28.210 | 10.59x | 2.840 | 56.388 | 19.85x |
| fe_kernel_n10000 | 1.638 | 14.722 | 8.99x | 1.666 | 14.721 | 8.83x |
| between_robust_n10000 | 1.060 | 13.482 | 12.72x | 1.062 | 13.482 | 12.70x |
| first_difference_robust_n10000 | 1.638 | 16.081 | 9.82x | 1.642 | 16.081 | 9.79x |
| random_effects_n10000 | 1.583 | 19.253 | 12.16x | 1.606 | 19.252 | 11.99x |
| fe_entity_n100000 | 19.223 | 101.243 | 5.27x | 20.523 | 404.085 | 19.69x |
| fe_twoway_clustered_n100000 | 31.026 | 191.619 | 6.18x | 32.579 | 764.605 | 23.47x |
| fe_kernel_n100000 | 20.832 | 90.521 | 4.35x | 21.855 | 361.001 | 16.52x |
| between_robust_n100000 | 11.555 | 94.521 | 8.18x | 11.942 | 377.371 | 31.60x |
| first_difference_robust_n100000 | 19.129 | 103.873 | 5.43x | 20.465 | 410.212 | 20.04x |
| random_effects_n100000 | 19.647 | 124.319 | 6.33x | 20.890 | 491.493 | 23.53x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 2.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `GlsBenchmarks` | SampleSize=200 | `MathNet_Gls` | step | 4.19 | 2.63 | +59% | 30% |
| `StationarityBenchmarks` | SampleSize=2000 | `CortexKpss` | drift | 0.7 | 0.88 | -20% | 20% |

**Still away from their median: 3.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `LeastSquaresRoutingBenchmarks` | Regressors=250 | `MathNet_Qr` | step | 9.89 | 6.58 | +50% | 30% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 1.02 | 2.17 | -37% | 20% |
| `LeastSquaresRoutingBenchmarks` | Regressors=4 | `Lodestar_Fit` | drift | 1.21 | 1.43 | +24% | 20% |
