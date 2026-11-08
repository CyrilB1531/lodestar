# Nightly benchmark run

<!-- nightly-baseline: 3e48e17c68bff96a3371172656990689433f479e -->
<!-- nightly-owed: BkTreeNearestBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks DecompositionWidthBenchmarks EmbeddingSearchHelpersBenchmarks GlmNegativeBinomialBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks LeastSquaresRoutingBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks PartitionValidityBenchmarks RankingMetricsBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
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

- Commit: `3e48e17c68bff96a3371172656990689433f479e`
- Previous run: `3e48e17c68bff96a3371172656990689433f479e`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `TiledMinHashSignaturesBenchmarks`
- `StatsBenchmarks`
- `RankTestBenchmarks`
- `DistributionTailBenchmarks`
- `DistributionIncumbentBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `EncoderIncumbentBenchmarks`
- `TransformerIncumbentBenchmarks`
- `ScalerIncumbentBenchmarks`
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
- `DoubleMetaphoneBenchmarks`
- `ProcessExtractBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `VectorStoreChurnBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `BertNormalizerBenchmarks`
- `AddedTokenScanBenchmarks`
- `PrecompiledNormalizerBenchmarks`

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **98.61 ms** |      **3.705 ms** |   **0.203 ms** |  **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   4,737.31 ms |  3,258.235 ms | 178.595 ms | 48.04 |    1.57 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,010.71 ms |    587.103 ms |  32.181 ms | 50.81 |    0.30 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |     **103.16 ms** |      **4.485 ms** |   **0.246 ms** |  **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |   9,812.77 ms |  5,548.253 ms | 304.118 ms | 95.12 |    2.56 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  10,083.91 ms |  6,615.448 ms | 362.615 ms | 97.75 |    3.05 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **998.84 ms** |     **36.428 ms** |   **1.997 ms** |  **1.00** |    **0.00** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  49,259.59 ms | 10,516.422 ms | 576.441 ms | 49.32 |    0.51 |  4000.0000 |  3000.0000 | 2000.0000 | 160.03 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  49,773.96 ms | 11,530.669 ms | 632.035 ms | 49.83 |    0.55 |  7000.0000 |  4000.0000 | 2000.0000 | 211.59 MB |        7.11 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |   **1,020.81 ms** |     **21.754 ms** |   **1.192 ms** |  **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 100,167.39 ms | 15,148.210 ms | 830.325 ms | 98.13 |    0.71 | 12000.0000 | 10000.0000 | 8000.0000 | 536.13 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] |  99,654.64 ms |  8,276.622 ms | 453.670 ms | 97.62 |    0.40 | 15000.0000 | 12000.0000 | 8000.0000 | 587.65 MB |       14.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,438.82 ns | 318.644 ns | 17.466 ns | 1.000 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.70 ns |   0.486 ns |  0.027 ns | 0.123 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   342.07 ns |  64.842 ns |  3.554 ns | 0.140 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,395.61 ns |  63.382 ns |  3.474 ns | 3.033 |    0.02 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,632.60 ns | 278.939 ns | 15.290 ns | 2.310 |    0.02 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,189.67 ns |  97.357 ns |  5.336 ns | 0.488 |    0.00 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   782.53 ns |  12.052 ns |  0.661 ns | 0.321 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.85 ns |   9.888 ns |  0.542 ns | 0.054 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   139.73 ns |   4.788 ns |  0.262 ns | 0.057 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 2,840.45 ns |  43.729 ns |  2.397 ns | 1.165 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,703.21 ns | 437.575 ns | 23.985 ns | 1.108 |    0.01 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   320.51 ns |  19.808 ns |  1.086 ns | 0.131 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    29.92 ns |   0.241 ns |  0.013 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    61.60 ns |   6.437 ns |  0.353 ns | 0.025 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    75.83 ns |   3.817 ns |  0.209 ns | 0.031 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   367.96 ns |  16.822 ns |  0.922 ns | 0.151 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   474.11 ns |   5.462 ns |  0.299 ns | 0.194 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   390.46 ns |   7.371 ns |  0.404 ns | 0.160 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    21.78 ns |   2.984 ns |  0.164 ns | 0.009 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.31 ns |   0.107 ns |  0.006 ns | 0.009 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.10 ns |   0.135 ns |  0.007 ns | 0.027 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.53 ns |   0.477 ns |  0.026 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.50 ns |   0.227 ns |  0.012 ns | 0.013 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.68 ns |   0.502 ns |  0.028 ns | 0.074 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|----------:|------------:|
| ChiSquaredSfOneDf          |  31.05 ns | 0.436 ns | 0.024 ns |  1.00 |         - |          NA |
| ChiSquaredSfFourDf         |  13.85 ns | 0.779 ns | 0.043 ns |  0.45 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  43.84 ns | 0.128 ns | 0.007 ns |  1.41 |         - |          NA |
| ChiSquaredSfHundredDf      |  76.87 ns | 4.268 ns | 0.234 ns |  2.48 |         - |          NA |
| ChiSquaredSfFractionalDf   | 118.43 ns | 2.746 ns | 0.151 ns |  3.81 |         - |          NA |
| NormalQuantile             |  28.75 ns | 0.129 ns | 0.007 ns |  0.93 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **35.02 μs** |  **3.472 μs** | **0.190 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  50.85 μs | 14.241 μs | 0.781 μs |  1.45 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **42.59 μs** |  **2.056 μs** | **0.113 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 102.73 μs |  5.060 μs | 0.277 μs |  2.41 |    0.01 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **370.96 μs** |  **6.210 μs** | **0.340 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 371.20 μs | 52.102 μs | 2.856 μs |  1.00 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **411.10 μs** | **38.199 μs** | **2.094 μs** |  **1.00** |    **0.01** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 739.40 μs | 54.887 μs | 3.009 μs |  1.80 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **30.21 μs** |   **4.064 μs** |  **0.223 μs** |  **1.00** |    **0.01** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.69 μs |  12.708 μs |  0.697 μs |  2.01 |    0.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,998.64 μs** | **216.307 μs** | **11.857 μs** |  **1.00** |    **0.00** | **328.1250** | **328.1250** | **328.1250** | **1097.71 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,616.45 μs | 789.580 μs | 43.280 μs |  1.90 |    0.01 | 546.8750 | 546.8750 | 546.8750 | 1877.96 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error     | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|----------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **304.7 μs** | **157.11 μs** | **8.61 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **306.0 μs** |   **9.11 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **307.2 μs** |  **34.30 μs** | **1.88 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **50.69 μs** |       **6.661 μs** |     **0.365 μs** |  **1.00** |    **0.01** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     106.82 μs |      15.684 μs |     0.860 μs |  2.11 |    0.02 |   1.9531 |   0.1221 |        - |   33.02 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **697.84 μs** |      **52.052 μs** |     **2.853 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,957.68 μs |     584.190 μs |    32.021 μs |  2.81 |    0.04 |  97.6563 |  97.6563 |  97.6563 |  370.09 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,864.98 μs** |     **965.753 μs** |    **52.936 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  23,041.40 μs |   8,892.739 μs |   487.441 μs |  3.36 |    0.07 | 281.2500 | 281.2500 | 281.2500 |  2227.2 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **51,756.89 μs** |   **2,705.446 μs** |   **148.295 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,294.34 μs | 111,292.465 μs | 6,100.316 μs |  2.73 |    0.10 |        - |        - |        - |    8846 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **37.385 μs** |   **0.9716 μs** | **0.0533 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.741 μs |   6.7387 μs | 0.3694 μs |  0.29 |    0.01 |   0.5341 |        - |        - |    8.8 KB |        3.32 |
| Hac       | 100        | 4    |    20.034 μs |   2.3301 μs | 0.1277 μs |  0.54 |    0.00 |   0.7629 |        - |        - |  12.95 KB |        4.89 |
| Cluster   | 100        | 4    |     9.159 μs |   0.4580 μs | 0.0251 μs |  0.24 |    0.00 |   0.5951 |        - |        - |   9.76 KB |        3.68 |
|           |            |      |              |             |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **461.294 μs** |  **38.3742 μs** | **2.1034 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   890.402 μs | 118.1500 μs | 6.4762 μs |  1.93 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.94 KB |        5.91 |
| Hac       | 10000      | 4    | 1,949.544 μs | 102.7058 μs | 5.6296 μs |  4.23 |    0.02 | 246.0938 | 246.0938 | 246.0938 | 863.89 KB |       10.80 |
| Cluster   | 10000      | 4    |   767.062 μs |  29.3895 μs | 1.6109 μs |  1.66 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 565.22 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,431.91 μs** | **381.164 μs** | **20.893 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **46.92 μs** |   **3.740 μs** |  **0.205 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape   | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **545.0 ms** | **50.44 ms** | **2.77 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **441.5 ms** | **22.34 ms** | **1.22 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Size | Mean          | Error      | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|-----------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.264 μs** |   **2.099 μs** |  **0.1151 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,573.114 μs |  59.579 μs |  3.2657 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     23.155 μs |   2.350 μs |  0.1288 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     92.845 μs |   2.759 μs |  0.1513 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **15.822 μs** |   **1.840 μs** |  **0.1009 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,194.222 μs | 276.324 μs | 15.1463 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    528.651 μs | 476.153 μs | 26.0996 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,343.936 μs |  89.162 μs |  4.8873 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **135.6 μs** |     **5.92 μs** |  **0.32 μs** |  **1.00** |    **0.00** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   892.2 μs |    13.68 μs |  0.75 μs |  6.58 |    0.01 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,312.2 μs** |   **193.17 μs** | **10.59 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,104.2 μs | 1,293.60 μs | 70.91 μs |  5.41 |    0.06 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **36.59 μs** |   **0.462 μs** |  **0.025 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   265.50 μs |  20.770 μs |  1.138 μs |  7.26 |    0.03 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    30.88 μs |   1.954 μs |  0.107 μs |  0.84 |    0.00 |   2.1973 |   0.0610 |        - |   35.66 KB |       13.46 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **458.86 μs** |  **17.757 μs** |  **0.973 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,561.71 μs | 946.986 μs | 51.908 μs |  9.94 |    0.10 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,688.32 μs | 207.871 μs | 11.394 μs |  3.68 |    0.02 | 441.4063 | 441.4063 | 441.4063 | 1765.79 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                       | Mean           | Error         | StdDev      | Median         | Ratio      | RatioSD   | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|------------:|---------------:|-----------:|----------:|----------:|------------:|
| NormalQuantile               |      0.0550 ns |     0.4986 ns |   0.0273 ns |      0.0394 ns |       1.15 |      0.64 |         - |          NA |
| NormalQuantileFarTail        |      0.0562 ns |     0.3409 ns |   0.0187 ns |      0.0493 ns |       1.17 |      0.53 |         - |          NA |
| StudentQuantile              | 14,224.2219 ns | 2,304.6747 ns | 126.3270 ns | 14,292.7388 ns | 296,352.22 | 99,099.41 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,329.8424 ns |    23.2012 ns |   1.2717 ns |  2,329.1242 ns |  48,540.72 | 16,227.19 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,486.7 μs** |    **38.08 μs** |   **2.09 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    419.2 μs |    49.99 μs |   2.74 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    618.0 μs |   303.10 μs |  16.61 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,263.0 μs |    31.32 μs |   1.72 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    691.9 μs |    33.26 μs |   1.82 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **620.8 μs** |    **12.52 μs** |   **0.69 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    244.8 μs |    29.28 μs |   1.61 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    517.7 μs |    14.12 μs |   0.77 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    781.0 μs |    12.50 μs |   0.69 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    398.4 μs |    14.52 μs |   0.80 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,915.0 μs** | **1,175.43 μs** |  **64.43 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,484.1 μs |    99.16 μs |   5.44 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,528.7 μs | 1,515.33 μs |  83.06 μs | 507.8125 | 507.8125 | 507.8125 |  5062691 B |
| WilcoxonPooledDifferences | 100000     | False | 17,198.1 μs | 1,290.14 μs |  70.72 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,193.4 μs |   763.15 μs |  41.83 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,072.4 μs** |   **408.86 μs** |  **22.41 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,798.7 μs |   115.21 μs |   6.32 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,335.4 μs |   338.28 μs |  18.54 μs | 523.4375 | 523.4375 | 523.4375 |  5062699 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,623.3 μs | 3,866.66 μs | 211.94 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,839.6 μs |   158.47 μs |   8.69 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **36.68 μs** |   **2.279 μs** |  **0.125 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.78 μs** |   **1.457 μs** |  **0.080 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.89 μs** |   **0.873 μs** |  **0.048 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc2**        |    **12.05 μs** |   **0.071 μs** |  **0.004 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.93 μs** |   **0.240 μs** |  **0.013 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **452.90 μs** |  **19.627 μs** |  **1.076 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **895.39 μs** |  **30.469 μs** |  **1.670 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **892.15 μs** |  **57.603 μs** |  **3.157 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,079.66 μs** | **828.688 μs** | **45.423 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,059.66 μs** | **213.410 μs** | **11.698 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.395 μs** |  **0.3710 μs** | **0.0203 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.539 μs |  0.4925 μs | 0.0270 μs |  2.63 |    0.01 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.161 μs |  0.2404 μs | 0.0132 μs |  1.17 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.165 μs |  0.4394 μs | 0.0241 μs |  2.77 |    0.01 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.971 μs |  0.4211 μs | 0.0231 μs |  1.13 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.665 μs |  0.4122 μs | 0.0226 μs |  2.65 |    0.01 |      - |     224 B |        0.09 |
|                                |            |            |            |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.781 μs** |  **1.4305 μs** | **0.0784 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.995 μs |  1.7669 μs | 0.0968 μs |  2.48 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.592 μs |  0.6844 μs | 0.0375 μs |  1.02 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.715 μs |  4.2076 μs | 0.2306 μs |  2.50 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.241 μs |  0.6126 μs | 0.0336 μs |  1.01 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 122.325 μs | 41.2171 μs | 2.2592 μs |  2.51 |    0.04 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.520 μs** |  **1.3971 μs** | **0.0766 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  30.301 μs |  1.6230 μs | 0.0890 μs |  4.03 |    0.04 |   4.0588 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  26.730 μs |  7.8007 μs | 0.4276 μs |  3.55 |    0.06 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.876 μs |  0.1763 μs | 0.0097 μs |  0.52 |    0.00 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.056 μs |  0.7058 μs | 0.0387 μs |  0.54 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.538 μs |  0.4980 μs | 0.0273 μs |  0.20 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.936 μs |  0.8623 μs | 0.0473 μs |  0.39 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **117.502 μs** | **36.1880 μs** | **1.9836 μs** |  **1.00** |    **0.02** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 999.996 μs | 92.9118 μs | 5.0928 μs |  8.51 |    0.13 | 248.0469 | 248.0469 | 248.0469 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 295.414 μs | 30.9536 μs | 1.6967 μs |  2.51 |    0.04 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.805 μs |  2.6006 μs | 0.1425 μs |  0.82 |    0.01 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.584 μs |  3.2444 μs | 0.1778 μs |  0.83 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.404 μs |  2.5213 μs | 0.1382 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  28.893 μs |  1.9534 μs | 0.1071 μs |  0.25 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean            | Error           | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,116.0 ns** |       **737.12 ns** |      **40.40 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,431.4 ns |       574.22 ns |      31.47 ns |    6.28 |    0.04 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,160.6 ns |       181.86 ns |       9.97 ns |    0.35 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,775.4 ns |       863.24 ns |      47.32 ns |    3.89 |    0.02 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        118.8 ns |        33.48 ns |       1.83 ns |    0.02 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        221.8 ns |        12.53 ns |       0.69 ns |    0.04 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |                 |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **53,526.8 ns** |       **315.12 ns** |      **17.27 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    141,119.0 ns |     9,439.72 ns |     517.42 ns |   2.636 |    0.01 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    694,243.7 ns |    15,747.61 ns |     863.18 ns |  12.970 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,696,886.8 ns | 2,728,287.46 ns | 149,546.64 ns | 237.206 |    2.42 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        118.3 ns |        12.77 ns |       0.70 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        215.3 ns |        11.95 ns |       0.65 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **14.99 μs** |  **2.408 μs** | **0.132 μs** |   **2.9602** |   **0.3510** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **35.81 μs** |  **2.199 μs** | **0.121 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **51.27 μs** |  **6.390 μs** | **0.350 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **158.64 μs** | **37.033 μs** | **2.030 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **215.03 μs** | **11.573 μs** | **0.634 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **555.95 μs** | **32.174 μs** | **1.764 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **742.05 μs** | **25.079 μs** | **1.375 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,319.70 μs** | **73.536 μs** | **4.031 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **Lodestar_Wls** | **200**        |     **45.09 μs** |      **1.718 μs** |     **0.094 μs** |  **1.00** |    **0.00** |    **0.1831** |         **-** |         **-** |    **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     18.90 μs |      0.333 μs |     0.018 μs |  0.42 |    0.00 |    3.6926 |    0.1831 |         - |   58.29 KB |       16.99 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **2000**       |    **136.16 μs** |     **10.785 μs** |     **0.591 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |   **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    121.05 μs |     21.304 μs |     1.168 μs |  0.89 |    0.01 |   33.2031 |   13.6719 |         - |  508.29 KB |       29.06 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **20000**      |    **931.50 μs** |     **14.989 μs** |     **0.822 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |  **158.51 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,634.31 μs |  2,243.496 μs |   122.974 μs |  1.75 |    0.11 | 1019.5313 | 1019.5313 | 1019.5313 | 5027.72 KB |       31.72 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **200000**     |  **9,136.69 μs** |    **312.581 μs** |    **17.134 μs** |  **1.00** |    **0.00** |   **93.7500** |   **93.7500** |   **93.7500** | **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 18,766.16 μs | 31,228.612 μs | 1,711.746 μs |  2.05 |    0.16 |  718.7500 |  718.7500 |  718.7500 | 50025.1 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------------------------ |---------:|---------:|---------:|----------:|----------:|
| ChatTemplate            | 97.60 ms | 0.956 ms | 0.052 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens | 82.51 ms | 6.132 ms | 0.336 ms | 1714.2857 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **35.87 ms** |  **3.802 ms** | **0.208 ms** |  **714.2857** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **40.12 ms** |  **6.564 ms** | **0.360 ms** |  **692.3077** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **34.48 ms** |  **0.668 ms** | **0.037 ms** |  **666.6667** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **49.08 ms** |  **3.455 ms** | **0.189 ms** | **1272.7273** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.46 ms** | **10.567 ms** | **0.579 ms** | **1307.6923** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **53.33 ms** |  **2.604 ms** | **0.143 ms** | **1500.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | Samples | Clusters | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.081 ms** | **0.1340 ms** | **0.0073 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.114 ms | 0.1338 ms | 0.0073 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.352 ms | 0.1813 ms | 0.0099 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.070 ms** | **1.5101 ms** | **0.0828 ms** | **187.5000** | **187.5000** | **187.5000** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.159 ms | 0.7838 ms | 0.0430 ms | 210.9375 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.409 ms | 9.3612 ms | 0.5131 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Length | Mean            | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **875.5 ns** |     **232.46 ns** |     **12.74 ns** |  **1.00** |    **0.02** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,802.5 ns |      45.83 ns |      2.51 ns |  2.06 |    0.03 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **68,036.0 ns** |   **5,926.27 ns** |    **324.84 ns** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,137.4 ns |   5,380.07 ns |    294.90 ns |  2.22 |    0.01 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,335,356.7 ns** | **407,025.52 ns** | **22,310.44 ns** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,319,400.8 ns | 121,617.04 ns |  6,666.24 ns |  2.84 |    0.01 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Mean     | Error   | StdDev  | Gen0    | Allocated |
|------- |---------:|--------:|--------:|--------:|----------:|
| Encode | 282.6 μs | 8.02 μs | 0.44 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count  | Mean       | Error     | StdDev   | Allocated |
|------------ |------- |-----------:|----------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **525.6 μs** |  **24.75 μs** |  **1.36 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,326.0 μs** | **607.90 μs** | **33.32 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-PVBJKR : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-PVBJKR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **280.88 μs** |    **12.920 μs** |    **14.361 μs** |  **1.00** |    **0.07** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    280.06 μs |    13.464 μs |    14.966 μs |  1.00 |    0.07 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    106.92 μs |     5.990 μs |     6.657 μs |  0.38 |    0.03 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    483.38 μs |    82.617 μs |    95.141 μs |  1.73 |    0.34 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,730.35 μs |   142.857 μs |   158.785 μs |  6.18 |    0.63 |        - |        - |        - |  339.34 KB |        2.03 |
| MlNet_ReplaceMissingValues_Read | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,244.83 μs |    57.526 μs |    63.940 μs |  4.44 |    0.31 |        - |        - |        - |  312.66 KB |        1.87 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    175.90 μs |    20.978 μs |     1.150 μs |  1.00 |    0.01 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     70.66 μs |    21.747 μs |     1.192 μs |  0.40 |    0.01 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.25 μs |     0.870 μs |     0.048 μs |  0.09 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    955.40 μs | 2,552.454 μs |   139.909 μs |  5.43 |    0.69 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    935.41 μs |   338.541 μs |    18.557 μs |  5.32 |    0.10 |  48.8281 |   7.8125 |        - |   797.9 KB |        4.81 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    254.35 μs |    69.575 μs |     3.814 μs |  1.45 |    0.02 |  19.0430 |  18.5547 |        - |  299.33 KB |        1.80 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-PVBJKR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,721.90 μs** |   **861.411 μs** |   **921.701 μs** |  **1.03** |    **0.23** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,742.58 μs |   853.193 μs |   982.538 μs |  0.85 |    0.22 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |    690.00 μs |    15.139 μs |    17.434 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,453.76 μs |    43.815 μs |    44.994 μs |  0.80 |    0.13 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,567.29 μs | 1,268.289 μs | 1,302.440 μs |  2.97 |    0.52 |        - |        - |        - |   445.1 KB |        0.14 |
| MlNet_ReplaceMissingValues_Read | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,616.54 μs | 4,793.414 μs | 5,520.103 μs |  1.90 |    1.02 |        - |        - |        - |  537.01 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,545.25 μs |   738.958 μs |    40.505 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3291.64 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,120.47 μs |   307.465 μs |    16.853 μs |  0.88 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.42 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    453.33 μs |   931.810 μs |    51.076 μs |  0.13 |    0.01 | 128.9063 | 128.9063 | 128.9063 |  626.19 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,541.66 μs | 1,081.474 μs |    59.279 μs |  0.43 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,109.26 μs |   435.681 μs |    23.881 μs |  1.16 |    0.01 |  31.2500 |  15.6250 |        - |  561.76 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,396.05 μs |   945.286 μs |    51.814 μs |  0.68 |    0.01 |  27.3438 |  11.7188 |        - |  454.13 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **496.4 μs** |   **294.86 μs** |  **16.16 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    563.7 μs |    38.83 μs |   2.13 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    676.0 μs |    59.72 μs |   3.27 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,410.1 μs |   190.69 μs |  10.45 μs | 9.7656 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **9,645.5 μs** | **2,293.52 μs** | **125.72 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  |  8,699.8 μs | 1,063.07 μs |  58.27 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 10,071.5 μs |   151.19 μs |   8.29 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 18,105.6 μs | 3,159.36 μs | 173.18 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **39.77 ns** |      **0.872 ns** |   **0.048 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    125.21 ns |      0.509 ns |   0.028 ns |  3.15 |    0.00 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **144.36 ns** |      **1.472 ns** |   **0.081 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,441.91 ns |    842.781 ns |  46.196 ns |  9.99 |    0.28 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **494.38 ns** |     **40.325 ns** |   **2.210 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,046.63 ns |  4,049.234 ns | 221.952 ns | 18.30 |    0.40 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **48,950.78 ns** | **16,947.603 ns** | **928.955 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 46,853.36 ns |  5,585.059 ns | 306.136 ns |  0.96 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 13.57 ms | 0.730 ms | 0.040 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 61.33 ms | 3.889 ms | 0.213 ms | 444.4444 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **6.862 ms** | **0.5160 ms** | **0.0283 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.738 ms | 0.1715 ms | 0.0094 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.850 ms** | **0.2531 ms** | **0.0139 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.742 ms | 0.1639 ms | 0.0090 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Q | Element   | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.792 μs** | **0.3528 μs** | **0.0193 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **3.949 μs** | **0.4233 μs** | **0.0232 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.695 μs** | **0.4756 μs** | **0.0261 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **5.072 μs** | **0.5430 μs** | **0.0298 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Length | Mean         | Error         | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|--------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **372.9 ns** |      **18.62 ns** |     **1.02 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,366.8 ns |     403.72 ns |    22.13 ns | 14.39 |    0.06 |         - |          NA |
|                          |        |              |               |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,792.1 ns** |   **4,594.02 ns** |   **251.81 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 833,518.5 ns | 154,621.86 ns | 8,475.35 ns | 36.57 |    0.47 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **26.95 ms** | **1.550 ms** | **0.085 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **44.21 ms** | **0.229 ms** | **0.013 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-TSGYHD : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-TSGYHD** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **110.221 μs** |     **3.9999 μs** |     **4.1076 μs** |  **1.00** |    **0.05** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    119.479 μs |    19.4081 μs |    21.5721 μs |  1.09 |    0.20 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,296.173 μs |    26.4201 μs |    30.4255 μs | 20.86 |    0.82 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    123.460 μs |    23.6193 μs |    27.2000 μs |  1.12 |    0.24 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |     77.585 μs |    12.5319 μs |    13.4090 μs |  0.70 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |     67.220 μs |    12.6848 μs |    13.5726 μs |  0.61 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    104.644 μs |    15.9574 μs |    15.6723 μs |  0.95 |    0.14 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    440.296 μs |    16.8253 μs |    18.7013 μs |  4.00 |    0.22 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,270.320 μs |    87.4649 μs |    97.2170 μs | 20.63 |    1.15 |        - |        - |        - |  297008 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,477.092 μs |   128.5983 μs |   132.0610 μs | 58.84 |    2.48 |        - |        - |        - |  461192 B |        5.65 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     48.724 μs |     6.8228 μs |     0.3740 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     41.184 μs |     6.2599 μs |     0.3431 μs |  0.85 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.402 μs |    15.4942 μs |     0.8493 μs |  9.55 |    0.07 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.167 μs |     1.5703 μs |     0.0861 μs |  0.95 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.565 μs |     0.7846 μs |     0.0430 μs |  0.22 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.681 μs |     0.7368 μs |     0.0404 μs |  0.16 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.925 μs |     2.1837 μs |     0.1197 μs |  0.24 |    0.00 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,564.062 μs | 4,361.1427 μs |   239.0489 μs | 32.10 |    4.25 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,062.521 μs | 1,380.9001 μs |    75.6918 μs | 21.81 |    1.35 |  39.0625 |   1.9531 |        - |  652764 B |       8.069 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,644.308 μs | 2,783.0052 μs |   152.5459 μs | 33.75 |    2.72 |  35.1563 |        - |        - |  625462 B |       7.732 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-TSGYHD** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,545.853 μs** |    **14.5609 μs** |    **16.7683 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,451.713 μs |    11.0741 μs |    12.3089 μs |  0.94 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,236.034 μs |    77.9333 μs |    83.3877 μs |  9.86 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,541.842 μs |    17.3956 μs |    20.0329 μs |  1.00 |    0.02 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    417.040 μs |     7.2946 μs |     8.1080 μs |  0.27 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    310.750 μs |     2.1239 μs |     1.9867 μs |  0.20 |    0.00 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    428.206 μs |    11.9083 μs |    13.2361 μs |  0.28 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,764.834 μs |    41.6950 μs |    46.3438 μs |  4.38 |    0.05 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,660.220 μs | 8,421.3042 μs | 9,697.9864 μs | 11.43 |    6.12 |        - |        - |        - |  530424 B |       0.331 |
| MlNet_NormalizeRobustScaling_Read | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,310.536 μs |   194.5762 μs |   208.1943 μs | 12.49 |    0.19 |        - |        - |        - | 4278520 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,386.666 μs | 1,663.9432 μs |    91.2063 μs |  1.00 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601938 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,433.928 μs |   729.0498 μs |    39.9617 μs |  1.04 |    0.06 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,479.966 μs |    98.9343 μs |     5.4229 μs | 10.47 |    0.58 | 875.0000 | 875.0000 | 875.0000 | 3201194 B |       1.998 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,465.670 μs | 1,467.8257 μs |    80.4565 μs |  1.06 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601039 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    298.243 μs |    79.2462 μs |     4.3438 μs |  0.22 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    244.214 μs |    12.8615 μs |     0.7050 μs |  0.18 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    402.342 μs |    35.1962 μs |     1.9292 μs |  0.29 |    0.02 | 128.9063 | 124.0234 | 124.0234 |  560068 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,633.497 μs | 2,378.5702 μs |   130.3775 μs |  1.18 |    0.10 |  41.0156 |   6.8359 |        - |  702655 B |       0.439 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,413.796 μs | 1,620.0944 μs |    88.8028 μs |  3.19 |    0.19 |  31.2500 |   7.8125 |        - |  594265 B |       0.371 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,463.651 μs |   600.5423 μs |    32.9177 μs | 14.08 |    0.78 | 250.0000 | 125.0000 |        - | 4302324 B |       2.686 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Model   | Mean     | Error    | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **137.9 ms** |  **5.28 ms** | **0.29 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **134.1 ms** | **20.37 ms** | **1.12 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.30 ms** | **0.536 ms** | **0.029 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **193.75 ms** | **3.188 ms** | **0.175 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **79.000 μs** |    **31.907 μs** |   **1.7489 μs** |  **1.00** |    **0.03** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    189.465 μs |    42.886 μs |   2.3507 μs |  2.40 |    0.05 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.795 μs |     1.183 μs |   0.0648 μs |  0.06 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    332.459 μs |    34.246 μs |   1.8771 μs |  4.21 |    0.08 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  6,845.573 μs |   771.034 μs |  42.2630 μs | 86.68 |    1.71 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     64.248 μs |     7.287 μs |   0.3994 μs |  0.81 |    0.02 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,154.760 μs |   569.908 μs |  31.2386 μs | 14.62 |    0.44 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,104.476 μs** |   **271.748 μs** |  **14.8954 μs** |  **1.00** |    **0.02** | **458.9844** | **445.3125** | **435.5469** | **2738.56 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,417.115 μs |   619.176 μs |  33.9391 μs |  2.19 |    0.04 | 492.1875 | 476.5625 | 468.7500 | 3130.08 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     82.197 μs |    63.733 μs |   3.4934 μs |  0.07 |    0.00 |  52.4902 |  52.4902 |  52.4902 |  391.17 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    281.979 μs |   184.385 μs |  10.1068 μs |  0.26 |    0.01 |   1.9531 |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 44,128.260 μs | 4,712.400 μs | 258.3026 μs | 39.96 |    0.51 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     64.410 μs |     3.142 μs |   0.1722 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,674.297 μs |   598.047 μs |  32.7810 μs |  6.95 |    0.09 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Words | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **650.3 μs** |   **128.1 μs** |   **7.02 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,789.6 μs** |   **138.8 μs** |   **7.61 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **12,144.6 μs** | **2,254.5 μs** | **123.58 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.83 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.68 ms** | **5.008 ms** | **0.275 ms** |         **-** |
| **TopTwo** | **100**     | **87.64 ms** | **7.066 ms** | **0.387 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-UDACYX : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-UDACYX** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **47.42 μs** |      **5.275 μs** |      **5.644 μs** |     **46.06 μs** |   **1.01** |    **0.16** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |     82.03 μs |      4.526 μs |      5.031 μs |     80.94 μs |   1.75 |    0.21 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    243.73 μs |     20.008 μs |     23.042 μs |    245.32 μs |   5.20 |    0.72 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    273.53 μs |     13.996 μs |     16.117 μs |    267.00 μs |   5.84 |    0.68 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    999.18 μs |     18.847 μs |     20.948 μs |    993.76 μs |  21.32 |    2.21 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,632.43 μs |     20.220 μs |     22.475 μs |  1,630.44 μs |  34.83 |    3.58 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,087.16 μs |     13.226 μs |     13.582 μs |  1,087.68 μs |  23.20 |    2.38 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    219.39 μs |     12.833 μs |     14.779 μs |    213.02 μs |   4.68 |    0.57 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,898.50 μs |     19.069 μs |     21.196 μs |  1,896.51 μs |  40.51 |    4.15 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     | 14,245.99 μs |  5,041.560 μs |  5,805.868 μs | 10,179.52 μs | 303.96 |  125.41 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,217.14 μs |     94.421 μs |     96.964 μs |  1,240.46 μs |  25.97 |    3.33 |        - |        - |        - |  315.34 KB |        9.86 |
| MlNet_NormalizeBinning_Read | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,099.85 μs |    143.964 μs |    154.040 μs |  3,137.43 μs |  66.14 |    7.46 |        - |        - |        - |  401.91 KB |       12.56 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     14.01 μs |      0.504 μs |      0.028 μs |     14.02 μs |   1.00 |    0.00 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.50 μs |     18.449 μs |      1.011 μs |     83.21 μs |   5.96 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    137.57 μs |     14.006 μs |      0.768 μs |    137.68 μs |   9.82 |    0.05 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     74.23 μs |      1.980 μs |      0.109 μs |     74.21 μs |   5.30 |    0.01 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     87.59 μs |    200.948 μs |     11.015 μs |     93.40 μs |   6.25 |    0.68 |   1.9531 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    462.76 μs |     45.894 μs |      2.516 μs |    463.29 μs |  33.02 |    0.17 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    196.52 μs |     60.490 μs |      3.316 μs |    195.25 μs |  14.02 |    0.21 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    140.50 μs |      2.199 μs |      0.121 μs |    140.44 μs |  10.03 |    0.02 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    721.87 μs |     18.562 μs |      1.017 μs |    722.29 μs |  51.51 |    0.11 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,722.16 μs |    577.275 μs |     31.642 μs |  8,707.06 μs | 622.41 |    2.23 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    254.17 μs |     49.114 μs |      2.692 μs |    253.29 μs |  18.14 |    0.17 |  19.0430 |  18.5547 |   0.4883 |  297.69 KB |        9.52 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    982.44 μs |  1,175.145 μs |     64.414 μs |    971.23 μs |  70.11 |    3.98 |  42.9688 |   5.8594 |        - |  698.95 KB |       22.35 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-UDACYX** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **479.50 μs** |     **11.269 μs** |     **12.977 μs** |    **479.03 μs** |   **1.00** |    **0.04** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,655.58 μs |     12.738 μs |     13.629 μs |  1,651.38 μs |   3.46 |    0.09 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,575.06 μs |     20.853 μs |     24.015 μs |  3,567.09 μs |   7.46 |    0.20 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,886.45 μs |    962.966 μs |  1,108.954 μs |  4,465.50 μs |   8.11 |    2.27 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,669.42 μs |  5,762.901 μs |  6,636.565 μs |  6,241.56 μs |  24.35 |   13.54 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,878.86 μs |     36.820 μs |     39.397 μs | 10,872.37 μs |  22.70 |    0.60 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,642.81 μs |  5,222.015 μs |  6,013.680 μs |  5,813.72 μs |  20.12 |   12.26 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,083.43 μs |     21.758 μs |     25.057 μs |  3,083.37 μs |   6.43 |    0.18 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,680.35 μs |    135.792 μs |    150.932 μs | 14,728.15 μs |  30.64 |    0.86 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,696.00 μs |     56.740 μs |     58.267 μs |  9,673.02 μs |  20.24 |    0.54 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,110.09 μs |  1,773.907 μs |  1,898.060 μs | 13,118.57 μs |  25.27 |    3.91 |        - |        - |        - |  546.52 KB |        0.87 |
| MlNet_NormalizeBinning_Read | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 29,371.60 μs | 14,122.988 μs | 16,264.054 μs | 18,933.46 μs |  61.30 |   33.17 |        - |        - |        - |  2078.1 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    680.90 μs |    103.557 μs |      5.676 μs |    681.63 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    653.81 μs |    380.520 μs |     20.858 μs |    655.12 μs |   0.96 |    0.03 | 282.2266 | 282.2266 | 282.2266 | 2346.24 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,167.27 μs |    304.184 μs |     16.673 μs |  3,167.32 μs |   4.65 |    0.04 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,894.95 μs |     74.592 μs |      4.089 μs |  1,896.89 μs |   2.78 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,742.40 μs |     61.567 μs |      3.375 μs |  5,741.82 μs |   8.43 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,680.29 μs |    595.405 μs |     32.636 μs | 10,673.43 μs |  15.69 |    0.12 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,889.44 μs |    209.120 μs |     11.463 μs |  5,894.54 μs |   8.65 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,208.37 μs |    201.180 μs |     11.027 μs |  3,204.29 μs |   4.71 |    0.04 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,748.99 μs |    615.462 μs |     33.736 μs | 13,755.10 μs |  20.19 |    0.15 | 750.0000 | 750.0000 | 750.0000 | 2660.98 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,151.77 μs |    381.146 μs |     20.892 μs |  9,141.18 μs |  13.44 |    0.10 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,288.57 μs |  1,090.981 μs |     59.800 μs |  2,283.81 μs |   3.36 |    0.08 |  27.3438 |  11.7188 |        - |  454.68 KB |        0.73 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,670.80 μs |    633.926 μs |     34.748 μs | 10,659.19 μs |  15.67 |    0.12 | 265.6250 | 218.7500 | 187.5000 |  2122.9 KB |        3.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-AKIUDK : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **1000**    |     **61.40 μs** |     **0.054 μs** |   **0.056 μs** | **0.1221** |   **3.15 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 1000    |     76.65 μs |     0.183 μs |   0.203 μs | 1.2207 |  20.58 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 1000    |     70.69 μs |     0.097 μs |   0.103 μs | 0.8545 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     59.63 μs |     2.083 μs |   0.114 μs | 0.1831 |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     75.64 μs |     3.017 μs |   0.165 μs | 1.2207 |  20.58 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     71.50 μs |     4.410 μs |   0.242 μs | 0.8545 |  15.52 KB |
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **10000**   |    **551.78 μs** |     **1.342 μs** |   **1.436 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 10000   |    894.21 μs |    10.194 μs |  11.330 μs | 0.9766 |  26.16 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 10000   |    829.25 μs |     2.793 μs |   2.988 μs | 0.9766 |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    581.02 μs |   193.700 μs |  10.617 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |    854.67 μs |    59.998 μs |   3.289 μs | 0.9766 |  26.16 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    866.59 μs |    68.713 μs |   3.766 μs | 0.9766 |  21.14 KB |
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **100000**  |  **8,617.58 μs** |    **58.498 μs** |  **67.366 μs** |      **-** |   **3.16 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 100000  | 12,611.72 μs |    72.666 μs |  83.682 μs |      - |     46 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 100000  | 12,675.67 μs |    97.449 μs | 112.222 μs |      - |  41.02 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  |  8,822.61 μs |   308.219 μs |  16.895 μs |      - |   3.19 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 12,648.13 μs | 1,842.959 μs | 101.019 μs |      - |     46 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 12,842.94 μs | 1,514.929 μs |  83.038 μs |      - |  41.02 KB |

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `BkTreeNearestBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `DecompositionWidthBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `HouseholderQrBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `PartitionValidityBenchmarks`
- `RankingMetricsBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 34.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredCdf` | step | 0.012 | 0.23 | -95% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherCdf` | step | 0.054 | 0.42 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalCdf` | step | 0.009 | 0.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalCdf` | step | 0.009 | 0.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentQuantile` | step | 0.488 | 3.77 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherCdf` | step | 0.057 | 0.44 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherQuantile` | step | 0.131 | 1.01 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalQuantile` | step | 0.074 | 0.57 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentQuantile` | step | 2.31 | 17.79 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalQuantile` | step | 0.013 | 0.1 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredQuantile` | step | 0.16 | 1.23 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredQuantile` | step | 0.194 | 1.49 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentCdf` | step | 0.14 | 1.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentCdf` | step | 0.123 | 0.94 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredCdf` | step | 0.025 | 0.19 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherQuantile` | step | 1.108 | 8.36 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalQuantile` | step | 0.012 | 0.09 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredCdf` | step | 0.031 | 0.23 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalCdf` | step | 0.027 | 0.2 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredQuantile` | step | 0.151 | 1.07 | -86% | 30% |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | step | 0.84 | 4.61 | -82% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `AccordMannWhitney` | step | 3.89 | 21.18 | -82% | 34% |
| `StatsBenchmarks` | SampleSize=100 | `AccordWelchT` | step | 6.28 | 34.05 | -82% | 30% |
| `OlsBenchmarks` | SampleSize=100 | `Accord_Ols` | step | 7.26 | 39.12 | -81% | 45% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hac` | step | 0.54 | 2.88 | -81% | 30% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Cluster` | step | 0.24 | 1.275 | -81% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarChiSquare` | step | 0.02 | 0.1 | -80% | 36% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hc0` | step | 0.29 | 1.44 | -80% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `AccordChiSquare` | step | 0.04 | 0.19 | -79% | 36% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200 | `MathNet_Wls` | step | 0.42 | 1.69 | -75% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_FisherQuantile` | step | 1.165 | 0.69 | +69% | 30% |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Discretize_Fit` | step | 6.25 | 3.84 | +63% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_StudentQuantile` | step | 3.033 | 2.16 | +40% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_TrainTestSplit_Read` | drift | 14.62 | 11.74 | +25% | 20% |

**Still away from their median: 13.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `StudentQuantile` | drift | 296352 | 7.55 | +181496% | 20% |
| `QuantileBenchmarks` | — | `StudentQuantileCauchyFarTail` | drift | 48540.7 | 2.35 | +167859% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarMannWhitney` | step | 0.35 | 2.13 | -84% | 66% |
| `DistributionTailBenchmarks` | — | `NormalQuantile` | drift | 0.93 | 3.85 | -76% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_TrainTestSplit` | drift | 0.81 | 0.54 | +50% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=100000 | `MlNet_TrainTestSplit` | drift | 0.06 | 0.04 | +50% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_CrossValidationSplit` | drift | 4.21 | 2.89 | +46% | 20% |
| `EncoderIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=20000 | `Lodestar_Impute` | drift | 0.13 | 0.21 | -43% | 20% |
| `StatsBenchmarks` | SampleSize=10000 | `LodestarChiSquare` | drift | 0.002 | 0.003 | -33% | 20% |
| `OsaBenchmarks` | Length=64 | `Distance_CodePoint` | drift | 18.3 | 21.22 | +32% | 20% |
| `EncoderIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Impute` | drift | 0.09 | 0.14 | -29% | 20% |
| `StatsBenchmarks` | SampleSize=10000 | `AccordMannWhitney` | drift | 237.206 | 331.818 | -28% | 20% |
| `ScalerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Standard` | drift | 0.95 | 1.36 | -25% | 20% |
