# Nightly benchmark run

<!-- nightly-baseline: 64dcf7b37e167677244d26cb8bf82db1915bf6cf -->
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

- Commit: `64dcf7b37e167677244d26cb8bf82db1915bf6cf`
- Previous run: `64dcf7b37e167677244d26cb8bf82db1915bf6cf`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `StationarityBenchmarks`
- `SerialCorrelationBenchmarks`
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
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|----------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **32.92 μs** |   **5.847 μs** |  **0.320 μs** |  **1.00** |    **0.01** |  **0.4883** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  49.19 μs |   3.891 μs |  0.213 μs |  1.49 |    0.01 |  4.7607 |  0.1831 |  117.67 KB |        9.70 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.00 μs** |   **2.293 μs** |  **0.126 μs** |  **1.00** |    **0.00** |  **0.7324** |       **-** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 101.06 μs |   1.906 μs |  0.104 μs |  2.46 |    0.01 |  6.8359 |  0.3662 |  168.44 KB |        8.69 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **328.78 μs** |  **42.494 μs** |  **2.329 μs** |  **1.00** |    **0.01** |  **4.3945** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 367.05 μs |  62.704 μs |  3.437 μs |  1.12 |    0.01 | 35.6445 | 10.7422 |  881.77 KB |        7.98 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **396.99 μs** |  **15.441 μs** |  **0.846 μs** |  **1.00** |    **0.00** |  **6.8359** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 745.12 μs | 213.756 μs | 11.717 μs |  1.88 |    0.03 | 50.7813 | 20.5078 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **265.9 μs** |    **34.48 μs** |   **1.89 μs** |   **1.9531** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,801.6 μs** |   **352.83 μs** |  **19.34 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **28,649.8 μs** | **2,111.92 μs** | **115.76 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.04 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.07 μs** |     **0.351 μs** |   **0.019 μs** |  **1.00** |    **0.00** |   **0.4883** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    56.92 μs |     5.879 μs |   0.322 μs |  1.96 |    0.01 |   0.7935 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |            |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,794.86 μs** | **1,030.666 μs** |  **56.494 μs** |  **1.00** |    **0.02** | **328.1250** | **328.1250** | **328.1250** | **1097.72 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 6,914.06 μs | 1,862.791 μs | 102.106 μs |  1.82 |    0.03 | 500.0000 | 500.0000 | 500.0000 | 1877.48 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error     | StdDev   | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|----------:|---------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **303.2 μs** | **360.90 μs** | **19.78 μs** | **4.3945** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **286.6 μs** |   **7.07 μs** |  **0.39 μs** | **4.3945** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **303.4 μs** |   **5.07 μs** |  **0.28 μs** | **4.3945** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **51.25 μs** |       **2.081 μs** |     **0.114 μs** |  **1.00** |    **0.00** |   **1.0986** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |      80.40 μs |       1.140 μs |     0.062 μs |  1.57 |    0.00 |   1.3428 |        - |        - |   32.97 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **699.55 μs** |     **285.969 μs** |    **15.675 μs** |  **1.00** |    **0.03** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,842.06 μs |      94.999 μs |     5.207 μs |  2.63 |    0.05 |  99.6094 |  99.6094 |  99.6094 |  369.43 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **7,007.87 μs** |     **922.459 μs** |    **50.563 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  25,900.46 μs |   9,805.733 μs |   537.485 μs |  3.70 |    0.07 | 281.2500 | 281.2500 | 281.2500 |  2228.6 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **98,212.28 μs** |   **2,503.767 μs** |   **137.240 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** | **7924.15 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 245,881.61 μs | 111,197.640 μs | 6,095.118 μs |  2.50 |    0.05 |        - |        - |        - |  8898.8 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **37.57 μs** |   **3.409 μs** |  **0.187 μs** |  **1.00** |    **0.01** |   **0.0610** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    11.62 μs |   1.172 μs |  0.064 μs |  0.31 |    0.00 |   0.3357 |        - |        - |   8.45 KB |        3.19 |
| Hac       | 100        | 4    |    22.64 μs |   3.083 μs |  0.169 μs |  0.60 |    0.00 |   0.4883 |        - |        - |  12.63 KB |        4.77 |
| Cluster   | 100        | 4    |    10.40 μs |   0.193 μs |  0.011 μs |  0.28 |    0.00 |   0.3815 |        - |        - |   9.51 KB |        3.59 |
|           |            |      |             |            |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **546.27 μs** |  **17.529 μs** |  **0.961 μs** |  **1.00** |    **0.00** |   **2.9297** |        **-** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   961.38 μs |  87.826 μs |  4.814 μs |  1.76 |    0.01 | 123.0469 | 123.0469 | 123.0469 | 472.59 KB |        5.91 |
| Hac       | 10000      | 4    | 2,198.07 μs | 322.328 μs | 17.668 μs |  4.02 |    0.03 | 246.0938 | 246.0938 | 246.0938 | 863.57 KB |       10.80 |
| Cluster   | 10000      | 4    |   795.01 μs | 149.745 μs |  8.208 μs |  1.46 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.97 KB |        7.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **130.2 μs** |      **32.24 μs** |     **1.77 μs** |  **1.00** |    **0.02** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     231.6 μs |      18.36 μs |     1.01 μs |  1.78 |    0.02 |   1.2207 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     679.1 μs |     144.96 μs |     7.95 μs |  5.22 |    0.08 | 190.4297 | 190.4297 | 190.4297 |   705.49 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **86,041.6 μs** |   **8,310.36 μs** |   **455.52 μs** |  **1.00** |    **0.01** | **333.3333** | **333.3333** | **333.3333** |  **8865.82 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 356,982.6 μs |   4,424.47 μs |   242.52 μs |  4.15 |    0.02 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 521,270.7 μs | 171,078.38 μs | 9,377.38 μs |  6.06 |    0.10 |        - |        - |        - | 32666.45 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **138.5 μs** |    **35.70 μs** |  **1.96 μs** |  **1.00** |    **0.02** |   **0.7324** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   882.0 μs |   103.01 μs |  5.65 μs |  6.37 |    0.09 |  59.5703 |   5.8594 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,352.0 μs** |   **166.87 μs** |  **9.15 μs** |  **1.00** |    **0.01** |   **3.9063** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 6,765.9 μs | 1,014.21 μs | 55.59 μs |  5.00 |    0.05 | 484.3750 | 234.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **37.49 μs** |     **2.306 μs** |   **0.126 μs** |  **1.00** |    **0.00** |   **0.0610** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   224.12 μs |    10.812 μs |   0.593 μs |  5.98 |    0.02 |   1.7090 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    35.12 μs |     8.912 μs |   0.488 μs |  0.94 |    0.01 |   1.4038 |        - |        - |   35.26 KB |       13.31 |
|              |            |             |              |            |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **512.83 μs** |    **44.320 μs** |   **2.429 μs** |  **1.00** |    **0.01** |   **2.9297** |        **-** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 5,210.23 μs | 1,895.308 μs | 103.888 μs | 10.16 |    0.18 | 140.6250 | 132.8125 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,737.86 μs |   259.827 μs |  14.242 μs |  3.39 |    0.03 | 441.4063 | 441.4063 | 441.4063 | 1765.35 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **37.55 μs** |   **2.468 μs** |  **0.135 μs** |   **0.0610** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **11.64 μs** |   **0.096 μs** |  **0.005 μs** |   **0.3357** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc1**        |    **11.67 μs** |   **0.308 μs** |  **0.017 μs** |   **0.3357** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc2**        |    **13.61 μs** |   **0.891 μs** |  **0.049 μs** |   **0.3662** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **100**        | **Hc3**        |    **13.45 μs** |   **0.999 μs** |  **0.055 μs** |   **0.3662** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **531.86 μs** |  **57.387 μs** |  **3.146 μs** |   **2.9297** |        **-** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **974.96 μs** | **138.392 μs** |  **7.586 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **999.76 μs** | **324.708 μs** | **17.798 μs** | **123.0469** | **123.0469** | **123.0469** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,172.96 μs** | **104.643 μs** |  **5.736 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,144.59 μs** | **118.260 μs** |  **6.482 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean      | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |  **5.204 μs** | **0.6454 μs** | **0.0354 μs** |  **1.00** | **0.0992** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  6.240 μs | 0.1465 μs | 0.0080 μs |  1.20 | 0.0076 |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |  5.679 μs | 0.2571 μs | 0.0141 μs |  1.09 | 0.1068 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  7.084 μs | 0.5124 μs | 0.0281 μs |  1.36 | 0.0305 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |  5.749 μs | 0.4557 μs | 0.0250 μs |  1.10 | 0.0916 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  6.413 μs | 0.4575 μs | 0.0251 μs |  1.23 | 0.0076 |     224 B |        0.09 |
|                                |            |           |           |           |       |        |           |             |
| **LodestarAutocorrelation**        | **2000**       | **57.990 μs** | **2.0744 μs** | **0.1137 μs** |  **1.00** | **0.6714** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 65.002 μs | 0.7047 μs | 0.0386 μs |  1.12 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       | 58.573 μs | 0.7144 μs | 0.0392 μs |  1.01 | 0.6714 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 67.604 μs | 1.6412 μs | 0.0900 μs |  1.17 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       | 58.629 μs | 1.3651 μs | 0.0748 μs |  1.01 | 0.6104 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 65.441 μs | 6.7721 μs | 0.3712 μs |  1.13 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |     **9.650 μs** |   **1.0262 μs** |  **0.0562 μs** |  **1.00** |    **0.01** |   **0.9766** |   **0.0153** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |    35.948 μs |   3.3427 μs |  0.1832 μs |  3.73 |    0.02 |   2.6855 |   0.2441 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |    26.466 μs |   3.5720 μs |  0.1958 μs |  2.74 |    0.02 |   0.6409 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |     4.336 μs |   0.3598 μs |  0.0197 μs |  0.45 |    0.00 |   0.0687 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |     4.529 μs |   0.2009 μs |  0.0110 μs |  0.47 |    0.00 |   0.1373 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |     1.905 μs |   0.1046 μs |  0.0057 μs |  0.20 |    0.00 |   0.2632 |        - |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |     3.095 μs |   0.3096 μs |  0.0170 μs |  0.32 |    0.00 |   0.3319 |        - |        - |    8.16 KB |        0.34 |
|                               |            |              |             |            |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |   **128.047 μs** |  **21.2556 μs** |  **1.1651 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 1,085.687 μs | 383.3373 μs | 21.0120 μs |  8.48 |    0.16 | 248.0469 | 248.0469 | 248.0469 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       |   293.701 μs |   4.3719 μs |  0.2396 μs |  2.29 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |   116.949 μs |   2.8074 μs |  0.1539 μs |  0.91 |    0.01 |   0.6104 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |   119.080 μs |  17.9361 μs |  0.9831 μs |  0.93 |    0.01 |   1.2207 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |    17.578 μs |   4.4142 μs |  0.2420 μs |  0.14 |    0.00 |   2.5330 |   0.2747 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |    30.160 μs |   2.7522 μs |  0.1509 μs |  0.24 |    0.00 |   3.1738 |   0.1831 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **27.16 μs** |   **5.242 μs** |  **0.287 μs** |   **1.9531** |   **0.1526** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **53.45 μs** |   **3.523 μs** |  **0.193 μs** |   **3.9063** |   **0.9155** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **82.52 μs** |   **0.976 μs** |  **0.053 μs** |   **4.3945** |   **1.3428** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **215.37 μs** |  **13.368 μs** |  **0.733 μs** |   **9.5215** |   **4.6387** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **303.54 μs** |  **73.611 μs** |  **4.035 μs** |  **73.7305** |  **73.7305** |  **73.7305** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **649.21 μs** |  **34.098 μs** |  **1.869 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **976.32 μs** | **119.363 μs** |  **6.543 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,906.76 μs** | **192.551 μs** | **10.554 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **46.19 μs** |     **1.858 μs** |   **0.102 μs** |  **1.00** |    **0.00** |    **0.1221** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     20.46 μs |     3.799 μs |   0.208 μs |  0.44 |    0.00 |    2.4719 |    0.1221 |         - |    58.35 KB |       17.01 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **149.21 μs** |    **22.415 μs** |   **1.229 μs** |  **1.00** |    **0.01** |    **0.4883** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    136.54 μs |    28.515 μs |   1.563 μs |  0.92 |    0.01 |   21.7285 |    8.7891 |         - |   507.87 KB |       29.03 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |  **1,185.97 μs** |   **302.931 μs** |  **16.605 μs** |  **1.00** |    **0.02** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.49 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,711.55 μs |   645.256 μs |  35.369 μs |  1.44 |    0.03 | 1066.4063 | 1066.4063 | 1066.4063 |  5024.09 KB |       31.70 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     | **10,125.20 μs** | **1,457.191 μs** |  **79.874 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.47 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 20,102.85 μs | 4,436.156 μs | 243.161 μs |  1.99 |    0.02 |  843.7500 |  843.7500 |  843.7500 | 50023.17 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `glm`
- `var`
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
| glm_negative_binomial_n1000 | 0.401 | 3.316 | 8.28x | 0.401 | 3.316 | 8.28x |
| glm_gamma_n1000 | 0.489 | 4.297 | 8.79x | 0.489 | 4.297 | 8.79x |
| glm_poisson_exposure_n1000 | 0.396 | 3.939 | 9.94x | 0.396 | 3.939 | 9.94x |
| mnlogit_n1000 | 0.826 | 15.510 | 18.77x | 0.826 | 15.509 | 18.77x |
| glm_negative_binomial_n10000 | 3.741 | 13.414 | 3.59x | 3.750 | 13.414 | 3.58x |
| glm_gamma_n10000 | 4.298 | 15.620 | 3.63x | 4.310 | 15.619 | 3.62x |
| glm_poisson_exposure_n10000 | 4.357 | 15.163 | 3.48x | 4.369 | 15.162 | 3.47x |
| mnlogit_n10000 | 8.428 | 80.191 | 9.52x | 8.436 | 80.188 | 9.51x |
| glm_negative_binomial_n100000 | 39.891 | 140.564 | 3.52x | 40.127 | 548.575 | 13.67x |
| glm_gamma_n100000 | 47.709 | 203.993 | 4.28x | 47.951 | 805.933 | 16.81x |
| glm_poisson_exposure_n100000 | 56.113 | 145.030 | 2.58x | 57.035 | 578.406 | 10.14x |
| mnlogit_n100000 | 82.346 | 793.276 | 9.63x | 82.648 | 1552.082 | 18.78x |

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
| 2sls_robust_n1000 | 1.842 | 51.657 | 28.04x | 1.842 | 51.481 | 27.95x |
| liml_robust_n1000 | 1.998 | 51.559 | 25.80x | 1.999 | 51.558 | 25.79x |
| gmm_robust_n1000 | 1.923 | 49.406 | 25.70x | 1.923 | 49.404 | 25.70x |
| 2sls_kernel_n1000 | 10.044 | 53.196 | 5.30x | 10.044 | 53.195 | 5.30x |
| 2sls_clustered_n1000 | 1.789 | 52.185 | 29.18x | 1.789 | 52.185 | 29.18x |
| 2sls_robust_n10000 | 17.648 | 227.033 | 12.86x | 17.863 | 454.017 | 25.42x |
| liml_robust_n10000 | 19.569 | 226.361 | 11.57x | 19.934 | 452.841 | 22.72x |
| gmm_robust_n10000 | 18.272 | 217.940 | 11.93x | 18.638 | 435.682 | 23.38x |
| 2sls_kernel_n10000 | 107.016 | 234.374 | 2.19x | 107.283 | 468.616 | 4.37x |
| 2sls_clustered_n10000 | 17.278 | 237.195 | 13.73x | 17.637 | 474.117 | 26.88x |
| 2sls_robust_n100000 | 198.697 | 2787.036 | 14.03x | 202.305 | 8496.162 | 42.00x |
| liml_robust_n100000 | 209.265 | 2774.219 | 13.26x | 211.909 | 8432.846 | 39.79x |
| gmm_robust_n100000 | 201.395 | 2714.971 | 13.48x | 204.807 | 8118.453 | 39.64x |
| 2sls_kernel_n100000 | 3929.261 | 3878.684 | 0.99x | 3930.802 | 11013.008 | 2.80x |
| 2sls_clustered_n100000 | 177.697 | 2996.341 | 16.86x | 179.151 | 9332.982 | 52.10x |

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
| ols_summary_n1000 | 0.093 | 2.641 | 28.46x | 0.093 | 2.641 | 28.46x |
| ols_hac_n1000 | 0.193 | 3.235 | 16.80x | 0.193 | 3.235 | 16.80x |
| ols_cluster_n1000 | 0.072 | 3.382 | 47.22x | 0.072 | 3.382 | 47.23x |
| ols_summary_n10000 | 0.529 | 11.428 | 21.62x | 0.529 | 11.428 | 21.62x |
| ols_hac_n10000 | 2.013 | 13.198 | 6.56x | 2.018 | 13.198 | 6.54x |
| ols_cluster_n10000 | 0.716 | 13.611 | 19.02x | 0.718 | 13.610 | 18.97x |
| ols_summary_n100000 | 4.959 | 147.855 | 29.82x | 4.981 | 587.725 | 117.98x |
| ols_hac_n100000 | 20.984 | 164.790 | 7.85x | 21.489 | 654.357 | 30.45x |
| ols_cluster_n100000 | 8.035 | 169.370 | 21.08x | 8.226 | 676.868 | 82.29x |

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
| fe_entity_n1000 | 0.277 | 18.079 | 65.35x | 0.277 | 18.078 | 65.35x |
| fe_twoway_clustered_n1000 | 0.465 | 28.008 | 60.22x | 0.465 | 28.006 | 60.22x |
| fe_kernel_n1000 | 0.295 | 18.400 | 62.37x | 0.295 | 18.399 | 62.37x |
| between_robust_n1000 | 0.165 | 17.254 | 104.45x | 0.165 | 17.254 | 104.45x |
| first_difference_robust_n1000 | 0.321 | 20.486 | 63.74x | 0.321 | 20.486 | 63.74x |
| random_effects_n1000 | 0.291 | 23.725 | 81.49x | 0.291 | 23.723 | 81.48x |
| fe_entity_n10000 | 3.048 | 27.569 | 9.05x | 3.124 | 27.568 | 8.82x |
| fe_twoway_clustered_n10000 | 5.474 | 51.316 | 9.38x | 5.667 | 102.588 | 18.10x |
| fe_kernel_n10000 | 3.310 | 27.798 | 8.40x | 3.355 | 27.797 | 8.28x |
| between_robust_n10000 | 1.779 | 26.408 | 14.85x | 1.783 | 26.406 | 14.81x |
| first_difference_robust_n10000 | 3.658 | 31.280 | 8.55x | 3.671 | 31.280 | 8.52x |
| random_effects_n10000 | 3.219 | 37.125 | 11.53x | 3.255 | 37.124 | 11.41x |
| fe_entity_n100000 | 35.150 | 215.935 | 6.14x | 36.650 | 857.505 | 23.40x |
| fe_twoway_clustered_n100000 | 56.334 | 391.329 | 6.95x | 57.742 | 1390.953 | 24.09x |
| fe_kernel_n100000 | 38.530 | 202.272 | 5.25x | 39.287 | 802.485 | 20.43x |
| between_robust_n100000 | 20.978 | 166.429 | 7.93x | 21.347 | 497.305 | 23.30x |
| first_difference_robust_n100000 | 39.017 | 217.784 | 5.58x | 40.424 | 820.584 | 20.30x |
| random_effects_n100000 | 35.961 | 264.705 | 7.36x | 37.336 | 990.788 | 26.54x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.067 | 2.607 | 38.63x | 0.067 | 2.607 | 38.62x |
| var_n10000 | 0.742 | 15.855 | 21.38x | 0.744 | 15.854 | 21.30x |
| var_n100000 | 7.570 | 144.497 | 19.09x | 7.795 | 144.493 | 18.54x |

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

**New tonight: 7.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `SerialCorrelationBenchmarks` | SampleSize=2000 | `CortexAutocorrelation` | step | 1.12 | 2.47 | -55% | 30% |
| `SerialCorrelationBenchmarks` | SampleSize=2000 | `CortexLjungBox` | step | 1.13 | 2.47 | -54% | 30% |
| `SerialCorrelationBenchmarks` | SampleSize=200 | `CortexAutocorrelation` | step | 1.2 | 2.58 | -53% | 30% |
| `SerialCorrelationBenchmarks` | SampleSize=2000 | `CortexPartialAutocorrelation` | step | 1.17 | 2.49 | -53% | 30% |
| `SerialCorrelationBenchmarks` | SampleSize=200 | `CortexLjungBox` | step | 1.23 | 2.61 | -53% | 30% |
| `SerialCorrelationBenchmarks` | SampleSize=200 | `CortexPartialAutocorrelation` | step | 1.36 | 2.72 | -50% | 30% |
| `LeastSquaresRoutingBenchmarks` | Regressors=4 | `Lodestar_Fit` | drift | 1.78 | 1.4 | +21% | 20% |

**Still away from their median: 8.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `OlsBenchmarks` | SampleSize=100 | `Accord_Ols` | drift | 5.98 | 39.12 | -82% | 20% |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | drift | 0.94 | 4.61 | -81% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hc0` | drift | 0.31 | 1.39 | -81% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hac` | drift | 0.6 | 2.82 | -80% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Cluster` | drift | 0.28 | 1.24 | -80% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200 | `MathNet_Wls` | drift | 0.44 | 1.61 | -74% | 20% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 1.57 | 3.38 | -38% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=2000 | `MathNet_Wls` | drift | 0.92 | 1.24 | -26% | 20% |
