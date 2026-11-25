# Nightly benchmark run

<!-- nightly-baseline: 4889a155d5a4354becd095a83d9ba98b10a8213b -->
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

- Commit: `4889a155d5a4354becd095a83d9ba98b10a8213b`
- Previous run: `4889a155d5a4354becd095a83d9ba98b10a8213b`
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
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1   | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|-------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **27.47 μs** |  **0.962 μs** | **0.053 μs** |  **1.00** |    **0.00** |  **0.1221** |      **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  49.73 μs | 21.843 μs | 1.197 μs |  1.81 |    0.04 |  1.4038 | 0.0610 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |        |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **35.54 μs** |  **6.681 μs** | **0.366 μs** |  **1.00** |    **0.01** |  **0.1831** |      **-** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          |  99.45 μs |  8.843 μs | 0.485 μs |  2.80 |    0.03 |  1.9531 | 0.1221 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |        |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **322.13 μs** | **10.585 μs** | **0.580 μs** |  **1.00** |    **0.00** |  **0.9766** |      **-** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 382.60 μs | 14.843 μs | 0.814 μs |  1.19 |    0.00 | 10.7422 | 2.9297 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |        |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **340.17 μs** | **32.899 μs** | **1.803 μs** |  **1.00** |    **0.01** |  **1.9531** |      **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 670.29 μs | 34.584 μs | 1.896 μs |  1.97 |    0.01 | 14.6484 | 4.8828 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **237.0 μs** |    **18.50 μs** |  **1.01 μs** |   **0.4883** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,629.6 μs** |   **178.05 μs** |  **9.76 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **25,672.6 μs** | **1,178.22 μs** | **64.58 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.04 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **24.85 μs** |     **1.160 μs** |  **0.064 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    49.95 μs |     4.080 μs |  0.224 μs |  2.01 |    0.01 |   0.2441 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,651.24 μs** |   **697.904 μs** | **38.254 μs** |  **1.00** |    **0.01** | **328.1250** | **328.1250** | **328.1250** | **1097.74 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 6,568.89 μs | 1,214.541 μs | 66.573 μs |  1.80 |    0.02 | 531.2500 | 531.2500 | 531.2500 | 1878.01 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|----------:|
| **Fit**    | **5**         | **265.2 μs** | **20.82 μs** | **1.14 μs** | **0.9766** | **110.56 KB** |
| **Fit**    | **50000**     | **269.7 μs** | **20.47 μs** | **1.12 μs** | **0.9766** | **110.56 KB** |
| **Fit**    | **5000000**   | **274.9 μs** | **19.04 μs** | **1.04 μs** | **0.9766** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **44.39 μs** |      **1.416 μs** |     **0.078 μs** |  **1.00** |    **0.00** |   **0.3052** |        **-** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |      60.22 μs |     11.065 μs |     0.606 μs |  1.36 |    0.01 |   0.3662 |        - |        - |   33.05 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **640.45 μs** |      **8.566 μs** |     **0.470 μs** |  **1.00** |    **0.00** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,996.99 μs |    414.079 μs |    22.697 μs |  3.12 |    0.03 |  97.6563 |  97.6563 |  97.6563 |  369.54 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,562.48 μs** |    **215.207 μs** |    **11.796 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  23,809.92 μs |  2,037.156 μs |   111.663 μs |  3.63 |    0.02 | 281.2500 | 281.2500 | 281.2500 | 2227.61 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       | **102,374.36 μs** | **17,418.099 μs** |   **954.745 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **7924.18 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 270,242.84 μs | 42,287.628 μs | 2,317.928 μs |  2.64 |    0.03 |        - |        - |        - |  8902.2 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **29.202 μs** |   **1.7514 μs** |  **0.0960 μs** |  **1.00** |    **0.00** |   **0.0305** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.394 μs |   0.2419 μs |  0.0133 μs |  0.36 |    0.00 |   0.0916 |        - |        - |   8.42 KB |        3.18 |
| Hac       | 100        | 4    |    21.509 μs |   1.4381 μs |  0.0788 μs |  0.74 |    0.00 |   0.1526 |        - |        - |   12.6 KB |        4.76 |
| Cluster   | 100        | 4    |     9.456 μs |   0.1019 μs |  0.0056 μs |  0.32 |    0.00 |   0.1068 |        - |        - |   9.46 KB |        3.57 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **555.756 μs** |  **38.1459 μs** |  **2.0909 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   966.364 μs | 557.7884 μs | 30.5743 μs |  1.74 |    0.05 | 124.0234 | 124.0234 | 124.0234 | 472.57 KB |        5.91 |
| Hac       | 10000      | 4    | 2,069.819 μs | 217.4407 μs | 11.9187 μs |  3.72 |    0.02 | 246.0938 | 246.0938 | 246.0938 | 863.55 KB |       10.80 |
| Cluster   | 10000      | 4    |   817.193 μs |  29.1499 μs |  1.5978 μs |  1.47 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.93 KB |        7.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Estimate** | **4**          |     **127.3 μs** |       **6.75 μs** |     **0.37 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     228.4 μs |       2.30 μs |     0.13 μs |  1.79 |    0.00 |   0.2441 |        - |        - |   33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     653.4 μs |     302.52 μs |    16.58 μs |  5.13 |    0.11 | 190.4297 | 190.4297 | 190.4297 |  705.25 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Estimate** | **250**        |  **88,589.0 μs** |   **1,157.36 μs** |    **63.44 μs** |  **1.00** |    **0.00** | **333.3333** | **333.3333** | **333.3333** | **8865.82 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 303,201.7 μs |  50,167.27 μs | 2,749.84 μs |  3.42 |    0.03 |        - |        - |        - |  3499.3 KB |        0.39 |
| MathNet_Qr        | 250        | 542,523.3 μs | 115,738.15 μs | 6,344.00 μs |  6.12 |    0.06 |        - |        - |        - | 32676.3 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1    | Allocated  | Alloc Ratio |
|--------- |----------- |-----------:|----------:|---------:|------:|--------:|---------:|--------:|-----------:|------------:|
| **Lodestar** | **200**        |   **135.7 μs** |  **12.29 μs** |  **0.67 μs** |  **1.00** |    **0.01** |        **-** |       **-** |   **19.24 KB** |        **1.00** |
| Accord   | 200        |   794.1 μs | 195.80 μs | 10.73 μs |  5.85 |    0.07 |  17.5781 |  0.9766 | 1466.83 KB |       76.23 |
|          |            |            |           |          |       |         |          |         |            |             |
| **Lodestar** | **2000**       | **1,361.5 μs** | **163.24 μs** |  **8.95 μs** |  **1.00** |    **0.01** |        **-** |       **-** |  **131.74 KB** |        **1.00** |
| Accord   | 2000       | 5,948.7 μs |  26.67 μs |  1.46 μs |  4.37 |    0.02 | 140.6250 | 62.5000 | 12015.2 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **29.42 μs** |   **1.290 μs** |  **0.071 μs** |  **1.00** |    **0.00** |   **0.0305** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   203.10 μs |  15.192 μs |  0.833 μs |  6.90 |    0.03 |   0.4883 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    34.60 μs |   9.412 μs |  0.516 μs |  1.18 |    0.02 |   0.4272 |        - |        - |   35.32 KB |       13.34 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **545.26 μs** |  **54.603 μs** |  **2.993 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,403.80 μs | 521.422 μs | 28.581 μs |  8.08 |    0.06 |  39.0625 |  31.2500 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,580.18 μs | 152.147 μs |  8.340 μs |  2.90 |    0.02 | 441.4063 | 441.4063 | 441.4063 | 1765.02 KB |       22.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|---------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **29.44 μs** |   **2.800 μs** | **0.153 μs** |   **0.0305** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.55 μs** |   **0.789 μs** | **0.043 μs** |   **0.0916** |        **-** |        **-** |   **8.38 KB** |
| **Fit**    | **100**        | **Hc1**        |    **11.25 μs** |   **0.310 μs** | **0.017 μs** |   **0.0916** |        **-** |        **-** |   **8.38 KB** |
| **Fit**    | **100**        | **Hc2**        |    **12.38 μs** |   **3.226 μs** | **0.177 μs** |   **0.1068** |        **-** |        **-** |   **9.18 KB** |
| **Fit**    | **100**        | **Hc3**        |    **12.43 μs** |   **1.024 μs** | **0.056 μs** |   **0.1068** |        **-** |        **-** |   **9.18 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **506.63 μs** |   **6.420 μs** | **0.352 μs** |        **-** |        **-** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **980.61 μs** |  **60.163 μs** | **3.298 μs** | **124.0234** | **124.0234** | **124.0234** | **472.52 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **996.90 μs** |  **41.196 μs** | **2.258 μs** | **123.0469** | **123.0469** | **123.0469** | **472.52 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,163.95 μs** | **176.300 μs** | **9.664 μs** | **123.0469** | **123.0469** | **123.0469** | **550.67 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,152.35 μs** |  **29.573 μs** | **1.621 μs** | **123.0469** | **123.0469** | **123.0469** | **550.67 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **3.525 μs** |  **0.0811 μs** | **0.0044 μs** |  **1.00** |    **0.00** | **0.0305** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |   9.782 μs |  1.1002 μs | 0.0603 μs |  2.78 |    0.02 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.073 μs |  0.3009 μs | 0.0165 μs |  1.16 |    0.00 | 0.0305 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  11.333 μs |  2.0182 μs | 0.1106 μs |  3.22 |    0.03 |      - |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   3.964 μs |  0.4426 μs | 0.0243 μs |  1.12 |    0.01 | 0.0229 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |   9.600 μs |  0.5892 μs | 0.0323 μs |  2.72 |    0.01 |      - |     224 B |        0.09 |
|                                |            |            |            |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **38.182 μs** |  **0.9763 μs** | **0.0535 μs** |  **1.00** |    **0.00** | **0.1831** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 107.055 μs | 20.4958 μs | 1.1234 μs |  2.80 |    0.03 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  38.714 μs |  1.0566 μs | 0.0579 μs |  1.01 |    0.00 | 0.1831 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 108.817 μs |  7.4463 μs | 0.4082 μs |  2.85 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  38.477 μs |  2.0605 μs | 0.1129 μs |  1.01 |    0.00 | 0.1831 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 105.667 μs | 28.2529 μs | 1.5486 μs |  2.77 |    0.04 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **8.749 μs** |  **0.5496 μs** | **0.0301 μs** |  **1.00** |    **0.00** |   **0.2899** |        **-** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  30.281 μs |  0.6619 μs | 0.0363 μs |  3.46 |    0.01 |   0.7935 |   0.1221 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |  26.960 μs |  2.0513 μs | 0.1124 μs |  3.08 |    0.01 |   0.1831 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.371 μs |  0.1878 μs | 0.0103 μs |  0.39 |    0.00 |   0.0191 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   3.376 μs |  0.2307 μs | 0.0126 μs |  0.39 |    0.00 |   0.0420 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.880 μs |  0.1363 μs | 0.0075 μs |  0.21 |    0.00 |   0.0782 |        - |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   3.400 μs |  0.0801 μs | 0.0044 μs |  0.39 |    0.00 |   0.0992 |        - |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **108.312 μs** | **10.5432 μs** | **0.5779 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 846.327 μs | 82.2938 μs | 4.5108 μs |  7.81 |    0.05 | 249.0234 | 249.0234 | 249.0234 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 282.426 μs | 27.4271 μs | 1.5034 μs |  2.61 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  75.168 μs | 15.9867 μs | 0.8763 μs |  0.69 |    0.01 |   0.1221 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  75.751 μs | 12.9259 μs | 0.7085 μs |  0.70 |    0.01 |   0.3662 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  17.907 μs |  1.3550 μs | 0.0743 μs |  0.17 |    0.00 |   0.7629 |   0.0610 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  33.017 μs | 48.8651 μs | 2.6785 μs |  0.30 |    0.02 |   0.9155 |        - |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **24.53 μs** |   **1.489 μs** |  **0.082 μs** |   **0.5798** |   **0.0610** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **50.48 μs** |   **2.662 μs** |  **0.146 μs** |   **1.1597** |   **0.2441** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **79.23 μs** |   **7.328 μs** |  **0.402 μs** |   **1.2207** |   **0.2441** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **199.75 μs** |   **8.037 μs** |  **0.441 μs** |   **2.6855** |   **1.2207** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **283.92 μs** |  **32.934 μs** |  **1.805 μs** |  **73.7305** |  **73.7305** |  **73.7305** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **638.55 μs** |  **18.360 μs** |  **1.006 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **905.30 μs** |  **30.475 μs** |  **1.670 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,516.43 μs** | **186.313 μs** | **10.212 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **36.35 μs** |      **0.995 μs** |     **0.055 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     20.01 μs |      0.462 μs |     0.025 μs |  0.55 |    0.00 |   0.7324 |   0.0305 |        - |    58.35 KB |       17.01 |
|              |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **2000**       |    **127.71 μs** |      **6.076 μs** |     **0.333 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    130.24 μs |     17.452 μs |     0.957 μs |  1.02 |    0.01 |   6.3477 |   2.9297 |        - |   508.04 KB |       29.04 |
|              |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **20000**      |  **1,105.34 μs** |     **78.906 μs** |     **4.325 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** |   **158.48 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,535.38 μs |  1,097.245 μs |    60.144 μs |  1.39 |    0.05 | 992.1875 | 992.1875 | 992.1875 |  5022.76 KB |       31.69 |
|              |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **200000**     | **11,373.51 μs** |    **534.561 μs** |    **29.301 μs** |  **1.00** |    **0.00** |  **93.7500** |  **93.7500** |  **93.7500** |  **1564.47 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 19,665.83 μs | 20,273.609 μs | 1,111.265 μs |  1.73 |    0.08 | 687.5000 | 687.5000 | 687.5000 | 50022.25 KB |       31.97 |

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
| glm_negative_binomial_n1000 | 0.361 | 2.518 | 6.98x | 0.361 | 2.518 | 6.98x |
| glm_gamma_n1000 | 0.456 | 3.148 | 6.91x | 0.456 | 3.148 | 6.91x |
| glm_poisson_exposure_n1000 | 0.371 | 2.918 | 7.86x | 0.371 | 2.918 | 7.85x |
| mnlogit_n1000 | 0.824 | 12.015 | 14.59x | 0.824 | 12.015 | 14.59x |
| glm_negative_binomial_n10000 | 3.326 | 10.982 | 3.30x | 3.333 | 10.981 | 3.29x |
| glm_gamma_n10000 | 4.005 | 12.274 | 3.06x | 4.013 | 12.272 | 3.06x |
| glm_poisson_exposure_n10000 | 4.127 | 12.055 | 2.92x | 4.138 | 12.055 | 2.91x |
| mnlogit_n10000 | 8.156 | 66.816 | 8.19x | 8.168 | 66.799 | 8.18x |
| glm_negative_binomial_n100000 | 35.499 | 136.105 | 3.83x | 35.714 | 539.980 | 15.12x |
| glm_gamma_n100000 | 43.089 | 134.337 | 3.12x | 43.307 | 527.988 | 12.19x |
| glm_poisson_exposure_n100000 | 51.417 | 125.703 | 2.44x | 51.753 | 501.839 | 9.70x |
| mnlogit_n100000 | 81.582 | 641.103 | 7.86x | 81.685 | 1357.922 | 16.62x |

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
| 2sls_robust_n1000 | 1.681 | 48.186 | 28.67x | 1.681 | 48.180 | 28.67x |
| liml_robust_n1000 | 1.853 | 48.541 | 26.20x | 1.853 | 48.541 | 26.20x |
| gmm_robust_n1000 | 1.700 | 47.875 | 28.16x | 1.700 | 47.871 | 28.15x |
| 2sls_kernel_n1000 | 9.447 | 50.714 | 5.37x | 9.447 | 50.708 | 5.37x |
| 2sls_clustered_n1000 | 1.618 | 49.865 | 30.83x | 1.617 | 49.860 | 30.83x |
| 2sls_robust_n10000 | 17.572 | 230.377 | 13.11x | 17.895 | 460.871 | 25.75x |
| liml_robust_n10000 | 19.525 | 229.781 | 11.77x | 19.897 | 459.494 | 23.09x |
| gmm_robust_n10000 | 18.078 | 222.007 | 12.28x | 18.379 | 444.004 | 24.16x |
| 2sls_kernel_n10000 | 101.204 | 238.271 | 2.35x | 101.517 | 476.505 | 4.69x |
| 2sls_clustered_n10000 | 16.643 | 241.032 | 14.48x | 16.928 | 482.019 | 28.47x |
| 2sls_robust_n100000 | 195.478 | 2582.484 | 13.21x | 197.683 | 8151.096 | 41.23x |
| liml_robust_n100000 | 215.266 | 2594.670 | 12.05x | 218.659 | 8094.335 | 37.02x |
| gmm_robust_n100000 | 198.867 | 2479.596 | 12.47x | 202.164 | 7730.681 | 38.24x |
| 2sls_kernel_n100000 | 3771.745 | 3511.397 | 0.93x | 3773.362 | 10708.251 | 2.84x |
| 2sls_clustered_n100000 | 180.786 | 2683.259 | 14.84x | 182.513 | 8676.846 | 47.54x |

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
| ols_summary_n1000 | 0.079 | 1.952 | 24.66x | 0.079 | 1.952 | 24.66x |
| ols_hac_n1000 | 0.177 | 2.536 | 14.29x | 0.177 | 2.536 | 14.29x |
| ols_cluster_n1000 | 0.067 | 2.546 | 38.24x | 0.067 | 2.546 | 38.24x |
| ols_summary_n10000 | 0.496 | 9.928 | 20.01x | 0.496 | 9.927 | 20.01x |
| ols_hac_n10000 | 1.816 | 11.244 | 6.19x | 1.820 | 11.244 | 6.18x |
| ols_cluster_n10000 | 0.647 | 11.643 | 18.00x | 0.648 | 11.643 | 17.95x |
| ols_summary_n100000 | 4.743 | 117.345 | 24.74x | 4.767 | 465.959 | 97.75x |
| ols_hac_n100000 | 19.242 | 132.138 | 6.87x | 19.777 | 524.156 | 26.50x |
| ols_cluster_n100000 | 7.640 | 134.233 | 17.57x | 7.843 | 535.889 | 68.33x |

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
| fe_entity_n1000 | 0.265 | 16.784 | 63.38x | 0.265 | 16.784 | 63.38x |
| fe_twoway_clustered_n1000 | 0.457 | 26.308 | 57.62x | 0.457 | 26.306 | 57.62x |
| fe_kernel_n1000 | 0.277 | 17.148 | 61.81x | 0.277 | 17.146 | 61.82x |
| between_robust_n1000 | 0.151 | 16.271 | 108.02x | 0.151 | 16.271 | 108.03x |
| first_difference_robust_n1000 | 0.275 | 19.266 | 70.00x | 0.275 | 19.266 | 70.00x |
| random_effects_n1000 | 0.273 | 22.477 | 82.45x | 0.273 | 22.476 | 82.44x |
| fe_entity_n10000 | 2.752 | 26.588 | 9.66x | 2.815 | 26.587 | 9.44x |
| fe_twoway_clustered_n10000 | 4.979 | 50.377 | 10.12x | 5.184 | 100.746 | 19.43x |
| fe_kernel_n10000 | 2.945 | 27.062 | 9.19x | 2.969 | 27.059 | 9.11x |
| between_robust_n10000 | 1.794 | 25.626 | 14.29x | 1.797 | 25.625 | 14.26x |
| first_difference_robust_n10000 | 2.923 | 30.548 | 10.45x | 2.932 | 30.545 | 10.42x |
| random_effects_n10000 | 2.829 | 35.830 | 12.67x | 2.858 | 35.826 | 12.54x |
| fe_entity_n100000 | 34.158 | 181.582 | 5.32x | 35.794 | 725.621 | 20.27x |
| fe_twoway_clustered_n100000 | 56.980 | 348.373 | 6.11x | 58.466 | 1272.146 | 21.76x |
| fe_kernel_n100000 | 38.308 | 167.958 | 4.38x | 39.537 | 670.850 | 16.97x |
| between_robust_n100000 | 19.777 | 163.590 | 8.27x | 20.236 | 553.047 | 27.33x |
| first_difference_robust_n100000 | 33.321 | 192.241 | 5.77x | 34.801 | 757.882 | 21.78x |
| random_effects_n100000 | 34.124 | 224.617 | 6.58x | 35.032 | 886.369 | 25.30x |

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
| var_n1000 | 0.067 | 2.137 | 32.11x | 0.067 | 2.136 | 32.11x |
| var_n10000 | 0.705 | 13.692 | 19.43x | 0.708 | 13.690 | 19.35x |
| var_n100000 | 6.815 | 126.785 | 18.60x | 6.944 | 126.784 | 18.26x |

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

**New tonight: 3.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `StationarityBenchmarks` | SampleSize=2000 | `LodestarDecompose` | step | 0.17 | 0.12 | +42% | 41% |
| `LeastSquaresRoutingBenchmarks` | Regressors=250 | `MathNet_Qr` | drift | 6.12 | 7.89 | -22% | 20% |
| `OlsBenchmarks` | SampleSize=10000 | `MathNet_Ols` | drift | 2.9 | 3.68 | -21% | 20% |

**Still away from their median: 3.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | step | 1.18 | 0.87 | +36% | 30% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 1.36 | 2.17 | -28% | 20% |
| `LeastSquaresRoutingBenchmarks` | Regressors=4 | `Lodestar_Fit` | drift | 1.79 | 1.4 | +27% | 20% |
