# Nightly benchmark run

<!-- nightly-baseline: 9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264 -->
<!-- nightly-owed: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
<!-- nightly-cut: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`
- Previous run: `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `SerialCorrelationBenchmarks`
- `QuantileBenchmarks`
- `OlsBenchmarks`
- `GlsBenchmarks`
- `MathNetInteropBenchmarks`
- `WeightedLeastSquaresBenchmarks`
- `RobustCovarianceBenchmarks`
- `HacClusterBenchmarks`
- `GlmBenchmarks`
- `GlmPoissonBenchmarks`
- `GlmOffsetBenchmarks`
- `MultinomialLogitBenchmarks`
- `VectorAutoregressionBenchmarks`
- `TextRankBenchmarks`
- `SilhouetteBenchmarks`
- `ClusteringAgreementBenchmarks`
- `EmbeddingSearchBenchmarks`
- `MannWhitneyExactBenchmarks`
- `RobustScalerSparseBenchmarks`
- `TopKAccuracyBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `QgramBenchmarks`
- `KsExactTableBenchmarks`
- `OsaBenchmarks`
- `RatcliffObershelpBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `VectorStoreChurnBenchmarks`

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|----------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **34.57 μs** |   **3.991 μs** |  **0.219 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  53.97 μs |   4.887 μs |  0.268 μs |  1.56 |    0.01 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.64 μs** |   **2.156 μs** |  **0.118 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 107.70 μs |  29.903 μs |  1.639 μs |  2.59 |    0.03 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **346.31 μs** |  **14.635 μs** |  **0.802 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 361.91 μs |  31.953 μs |  1.751 μs |  1.05 |    0.00 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **409.86 μs** | **242.843 μs** | **13.311 μs** |  **1.00** |    **0.04** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 705.28 μs |  61.728 μs |  3.384 μs |  1.72 |    0.05 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.78 μs** |     **4.349 μs** |  **0.238 μs** |  **1.00** |    **0.01** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.92 μs |     8.614 μs |  0.472 μs |  2.05 |    0.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,990.72 μs** | **1,082.740 μs** | **59.349 μs** |  **1.00** |    **0.02** | **328.1250** | **328.1250** | **328.1250** | **1097.72 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,488.99 μs |    45.003 μs |  2.467 μs |  1.88 |    0.02 | 531.2500 | 531.2500 | 531.2500 | 1877.95 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **301.2 μs** | **68.94 μs** | **3.78 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **316.8 μs** |  **8.41 μs** | **0.46 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **296.5 μs** | **38.48 μs** | **2.11 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **50.15 μs** |       **1.238 μs** |     **0.068 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     105.60 μs |      45.189 μs |     2.477 μs |  2.11 |    0.04 |   1.9531 |        - |        - |   32.99 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **676.49 μs** |     **131.588 μs** |     **7.213 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,740.42 μs |     212.483 μs |    11.647 μs |  2.57 |    0.03 |  97.6563 |  97.6563 |  97.6563 |  370.15 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,725.70 μs** |     **501.825 μs** |    **27.507 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  21,936.84 μs |   3,695.458 μs |   202.561 μs |  3.26 |    0.03 | 281.2500 | 281.2500 | 281.2500 | 2226.12 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **51,725.25 μs** |   **3,194.217 μs** |   **175.086 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 143,061.54 μs | 102,433.274 μs | 5,614.713 μs |  2.77 |    0.09 |        - |        - |        - | 8845.79 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **37.274 μs** |   **0.4527 μs** |  **0.0248 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.101 μs |   0.8191 μs |  0.0449 μs |  0.27 |    0.00 |   0.5035 |        - |        - |   8.45 KB |        3.19 |
| Hac       | 100        | 4    |    19.151 μs |   1.0230 μs |  0.0561 μs |  0.51 |    0.00 |   0.7629 |        - |        - |  12.63 KB |        4.77 |
| Cluster   | 100        | 4    |     9.160 μs |   0.9282 μs |  0.0509 μs |  0.25 |    0.00 |   0.5798 |        - |        - |   9.51 KB |        3.59 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **481.693 μs** |  **46.4038 μs** |  **2.5435 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   927.975 μs |  76.5072 μs |  4.1936 μs |  1.93 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.59 KB |        5.91 |
| Hac       | 10000      | 4    | 2,022.370 μs | 678.9736 μs | 37.2168 μs |  4.20 |    0.07 | 246.0938 | 246.0938 | 246.0938 | 863.57 KB |       10.80 |
| Cluster   | 10000      | 4    |   796.385 μs | 105.2853 μs |  5.7710 μs |  1.65 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.97 KB |        7.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,452.34 μs** | **413.997 μs** | **22.693 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **48.67 μs** |   **2.297 μs** |  **0.126 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape   | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **553.2 ms** | **125.1 ms** |  **6.86 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **457.4 ms** | **227.1 ms** | **12.45 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Size | Mean          | Error        | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|-------------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.140 μs** |     **2.053 μs** |  **0.1126 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,577.648 μs |   242.911 μs | 13.3148 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     24.549 μs |     7.120 μs |  0.3903 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     89.322 μs |    10.653 μs |  0.5839 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.468 μs** |     **3.384 μs** |  **0.1855 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,578.037 μs | 1,338.125 μs | 73.3471 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    527.413 μs |   331.582 μs | 18.1751 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,356.136 μs |   343.140 μs | 18.8086 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|----------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **133.9 μs** |  **28.17 μs** |  **1.54 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   909.5 μs | 138.13 μs |  7.57 μs |  6.79 |    0.08 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |           |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,341.2 μs** | **830.23 μs** | **45.51 μs** |  **1.00** |    **0.04** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,077.4 μs | 883.14 μs | 48.41 μs |  5.28 |    0.16 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|---------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **37.32 μs** |   **3.210 μs** | **0.176 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   262.75 μs |  50.019 μs | 2.742 μs |  7.04 |    0.07 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    30.91 μs |   3.834 μs | 0.210 μs |  0.83 |    0.01 |   2.1973 |   0.0610 |        - |   35.66 KB |       13.46 |
|              |            |             |            |          |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **647.27 μs** |  **93.214 μs** | **5.109 μs** |  **1.00** |    **0.01** |   **3.9063** |        **-** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,861.47 μs | 114.688 μs | 6.286 μs |  7.51 |    0.05 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,669.94 μs | 156.462 μs | 8.576 μs |  2.58 |    0.02 | 441.4063 | 441.4063 | 441.4063 |  1765.6 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                       | Mean           | Error         | StdDev     | Ratio      | RatioSD   | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|-----------:|-----------:|----------:|----------:|------------:|
| NormalQuantile               |      0.0416 ns |     0.1921 ns |  0.0105 ns |       1.05 |      0.33 |         - |          NA |
| NormalQuantileFarTail        |      0.0441 ns |     0.2280 ns |  0.0125 ns |       1.11 |      0.37 |         - |          NA |
| StudentQuantile              | 14,184.9290 ns | 1,436.1284 ns | 78.7190 ns | 356,738.00 | 79,773.70 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,312.2612 ns |    69.7963 ns |  3.8258 ns |  58,151.26 | 13,000.92 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean         | Error         | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-------------:|--------------:|-----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **38.111 μs** |     **4.7937 μs** |  **0.2628 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.343 μs** |     **0.5418 μs** |  **0.0297 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc1**        |     **9.907 μs** |     **0.6808 μs** |  **0.0373 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.712 μs** |     **1.7524 μs** |  **0.0961 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.515 μs** |     **0.6732 μs** |  **0.0369 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **465.538 μs** |    **61.5394 μs** |  **3.3732 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **950.432 μs** |    **84.9322 μs** |  **4.6554 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **899.703 μs** |    **48.3370 μs** |  **2.6495 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,054.192 μs** |   **344.3979 μs** | **18.8776 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,108.969 μs** | **1,087.2229 μs** | **59.5944 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.930 μs** |  **0.8527 μs** | **0.0467 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.652 μs |  3.3441 μs | 0.1833 μs |  2.36 |    0.04 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.995 μs |  0.7381 μs | 0.0405 μs |  1.01 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.182 μs |  0.7172 μs | 0.0393 μs |  2.47 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.029 μs |  0.1770 μs | 0.0097 μs |  1.02 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.653 μs |  0.3232 μs | 0.0177 μs |  2.36 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |            |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **49.375 μs** |  **1.1173 μs** | **0.0612 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.861 μs |  1.6112 μs | 0.0883 μs |  2.45 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.847 μs |  1.6215 μs | 0.0889 μs |  1.01 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 122.124 μs | 15.5421 μs | 0.8519 μs |  2.47 |    0.02 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.851 μs |  1.4737 μs | 0.0808 μs |  1.01 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 121.015 μs |  1.2914 μs | 0.0708 μs |  2.45 |    0.00 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **14.97 μs** |   **2.791 μs** |  **0.153 μs** |   **2.9602** |   **0.3510** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **35.55 μs** |   **5.688 μs** |  **0.312 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **51.56 μs** |   **3.990 μs** |  **0.219 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **156.84 μs** | **146.882 μs** |  **8.051 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **205.54 μs** |  **19.123 μs** |  **1.048 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **558.80 μs** |  **39.469 μs** |  **2.163 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **725.97 μs** | **101.472 μs** |  **5.562 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,237.79 μs** | **336.025 μs** | **18.419 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **45.90 μs** |    **10.189 μs** |   **0.558 μs** |  **1.00** |    **0.01** |    **0.1831** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     19.07 μs |     6.524 μs |   0.358 μs |  0.42 |    0.01 |    3.6926 |    0.1831 |         - |    58.29 KB |       17.00 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **135.34 μs** |     **4.702 μs** |   **0.258 μs** |  **1.00** |    **0.00** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    128.10 μs |    40.302 μs |   2.209 μs |  0.95 |    0.01 |   33.2031 |   13.1836 |         - |   508.28 KB |       29.06 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **955.05 μs** |    **52.573 μs** |   **2.882 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.49 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,540.59 μs | 1,952.540 μs | 107.025 μs |  1.61 |    0.10 | 1097.6563 | 1095.7031 | 1095.7031 |  5025.52 KB |       31.71 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,610.32 μs** | **1,722.315 μs** |  **94.406 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 18,263.38 μs | 4,548.333 μs | 249.309 μs |  1.90 |    0.03 |  843.7500 |  843.7500 |  843.7500 | 50024.07 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | Samples | Clusters | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.151 ms** |  **0.0718 ms** | **0.0039 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.174 ms |  0.4616 ms | 0.0253 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  20.536 ms |  0.7474 ms | 0.0410 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.009 ms** |  **1.1113 ms** | **0.0609 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.244 ms |  0.6584 ms | 0.0361 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 192.981 ms | 31.1314 ms | 1.7064 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Length | Mean            | Error           | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|----------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **871.6 ns** |       **215.99 ns** |      **11.84 ns** |  **1.00** |    **0.02** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,788.2 ns |        35.93 ns |       1.97 ns |  2.05 |    0.02 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **68,075.3 ns** |     **6,336.81 ns** |     **347.34 ns** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    153,059.4 ns |    16,123.66 ns |     883.79 ns |  2.25 |    0.01 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,322,355.7 ns** | **2,128,783.87 ns** | **116,685.83 ns** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,247,365.2 ns |   630,251.87 ns |  34,546.23 ns |  2.83 |    0.07 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count  | Mean       | Error    | StdDev  | Allocated |
|------------ |------- |-----------:|---------:|--------:|----------:|
| **SearchTop10** | **10000**  |   **522.3 μs** | **31.89 μs** | **1.75 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,663.0 μs** | **87.87 μs** | **4.82 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **462.8 μs** |    **74.48 μs** |   **4.08 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    602.1 μs |    53.62 μs |   2.94 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    612.0 μs |    74.05 μs |   4.06 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,376.1 μs |   632.50 μs |  34.67 μs | 9.7656 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **9,152.8 μs** | **1,440.10 μs** |  **78.94 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  |  8,977.5 μs |   837.91 μs |  45.93 μs |      - |    2.94 KB |
| HybridSelective | 100000  | 10,012.5 μs | 2,243.93 μs | 123.00 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 17,400.6 μs | 1,840.68 μs | 100.89 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **38.54 ns** |      **0.488 ns** |   **0.027 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    129.45 ns |      7.306 ns |   0.400 ns |  3.36 |    0.01 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **158.14 ns** |    **115.448 ns** |   **6.328 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,385.39 ns |    476.885 ns |  26.140 ns |  8.77 |    0.33 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **324.30 ns** |     **20.775 ns** |   **1.139 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  8,708.20 ns |  2,109.838 ns | 115.647 ns | 26.85 |    0.32 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **50,206.33 ns** | **17,000.234 ns** | **931.840 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 49,647.82 ns |  4,185.563 ns | 229.425 ns |  0.99 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Q | Element   | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.768 μs** | **0.3152 μs** | **0.0173 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **4.009 μs** | **1.5858 μs** | **0.0869 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.854 μs** | **0.3659 μs** | **0.0201 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.874 μs** | **2.1363 μs** | **0.1171 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Length | Mean         | Error         | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|--------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **375.1 ns** |      **31.39 ns** |     **1.72 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   6,059.2 ns |     553.83 ns |    30.36 ns | 16.15 |    0.09 |         - |          NA |
|                          |        |              |               |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,779.6 ns** |     **995.95 ns** |    **54.59 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 736,186.8 ns | 159,391.85 ns | 8,736.81 ns | 32.32 |    0.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **23.94 ms** | **0.583 ms** | **0.032 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.30 ms** | **0.438 ms** | **0.024 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean      | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.27 ms** |  **0.070 ms** | **0.004 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **196.83 ms** | **56.096 ms** | **3.075 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Words | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **645.2 μs** |    **57.03 μs** |   **3.13 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,810.9 μs** |   **294.36 μs** |  **16.14 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,644.1 μs** | **1,843.01 μs** | **101.02 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean     | Error     | StdDev   | Allocated |
|------- |-------- |---------:|----------:|---------:|----------:|
| **TopTwo** | **10**      | **11.52 ms** |  **0.194 ms** | **0.011 ms** |         **-** |
| **TopTwo** | **100**     | **89.91 ms** | **24.380 ms** | **1.336 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-OLJJDR : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **1000**    |     **59.62 μs** |     **0.155 μs** |   **0.159 μs** | **0.1831** |   **3.12 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 1000    |     77.44 μs |     0.178 μs |   0.190 μs | 1.2207 |  20.55 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 1000    |     70.56 μs |     0.182 μs |   0.187 μs | 0.8545 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     69.25 μs |     5.761 μs |   0.316 μs | 0.1221 |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     78.31 μs |    42.750 μs |   2.343 μs | 1.2207 |  20.55 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     72.84 μs |    11.359 μs |   0.623 μs | 0.8545 |  15.52 KB |
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **10000**   |    **602.19 μs** |     **1.362 μs** |   **1.338 μs** |      **-** |   **3.12 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 10000   |    875.76 μs |     2.830 μs |   2.907 μs | 0.9766 |  26.13 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 10000   |    911.16 μs |     3.277 μs |   3.506 μs | 0.9766 |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    564.46 μs |    84.152 μs |   4.613 μs |      - |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |    854.32 μs |    38.536 μs |   2.112 μs | 0.9766 |  26.13 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    854.25 μs |    64.772 μs |   3.550 μs | 0.9766 |  21.14 KB |
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **100000**  |  **8,952.54 μs** |    **66.134 μs** |  **73.508 μs** |      **-** |   **3.16 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 100000  | 13,282.48 μs |    42.446 μs |  48.881 μs |      - |  45.96 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 100000  | 12,744.23 μs |    82.744 μs |  91.970 μs |      - |  41.02 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  |  9,512.02 μs | 1,053.014 μs |  57.719 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 13,600.15 μs | 3,816.049 μs | 209.171 μs |      - |  45.96 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 12,950.00 μs | 1,727.122 μs |  94.669 μs |      - |  41.02 KB |

<!-- markdownlint-enable MD060 -->

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 0.** Each of these had not moved on its previous reading.

None.

**Still away from their median: 11.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `StudentQuantile` | drift | 356738 | 13202.3 | +2632% | 20% |
| `QuantileBenchmarks` | — | `StudentQuantileCauchyFarTail` | drift | 58151.3 | 3791.93 | +1435% | 20% |
| `OlsBenchmarks` | SampleSize=100 | `Accord_Ols` | drift | 7.04 | 39.12 | -82% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hac` | drift | 0.51 | 2.86 | -81% | 20% |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | drift | 0.83 | 4.61 | -81% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hc0` | drift | 0.27 | 1.43 | -81% | 20% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Cluster` | drift | 0.25 | 1.27 | -80% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200 | `MathNet_Wls` | drift | 0.42 | 1.69 | -75% | 20% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 2.11 | 3.6 | -41% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=2000 | `MathNet_Wls` | drift | 0.95 | 1.34 | -34% | 20% |
| `QuantileBenchmarks` | — | `NormalQuantileFarTail` | drift | 1.11 | 0.98 | +23% | 20% |
