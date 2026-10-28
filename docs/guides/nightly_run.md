# Nightly benchmark run

<!-- nightly-baseline: b3fd40aa4e70bd587819396620eee61914103d94 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BkTreeNearestBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks DecompositionWidthBenchmarks EmbeddingSearchHelpersBenchmarks FilteredVectorSearchBenchmarks GlmNegativeBinomialBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks LeastSquaresRoutingBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks RankingMetricsBenchmarks SentencePieceBpeLineageBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks VectorStoreChurnBenchmarks -->
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

- Commit: `b3fd40aa4e70bd587819396620eee61914103d94`
- Previous run: `b3fd40aa4e70bd587819396620eee61914103d94`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
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

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

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

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.550 ms** |   **0.0518 ms** |  **0.0028 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   5.033 ms |   5.4144 ms |  0.2968 ms |  1.97 |    0.10 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.067 ms |   5.6990 ms |  0.3124 ms |  1.99 |    0.11 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **11.996 ms** |   **0.1312 ms** |  **0.0072 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   8.312 ms |   9.8104 ms |  0.5377 ms |  0.69 |    0.04 |        - |        - |        - |    42840 B |    2,677.50 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  11.184 ms |   7.7147 ms |  0.4229 ms |  0.93 |    0.03 | 328.1250 | 328.1250 | 328.1250 |  2711044 B |  169,440.25 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **52.187 ms** |   **0.3593 ms** |  **0.0197 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **102 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  44.825 ms |  30.3931 ms |  1.6659 ms |  0.86 |    0.03 |        - |        - |        - |   802917 B |    7,871.74 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  61.858 ms | 278.6547 ms | 15.2740 ms |  1.19 |    0.25 | 375.0000 | 375.0000 | 375.0000 |  8072041 B |   79,137.66 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **243.834 ms** |   **1.2616 ms** |  **0.0692 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 146.483 ms |  48.7896 ms |  2.6743 ms |  0.60 |    0.01 |        - |        - |        - |   803008 B |          NA |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 234.047 ms |  24.4166 ms |  1.3384 ms |  0.96 |    0.00 | 333.3333 | 333.3333 | 333.3333 | 52871389 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

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

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,706.629 ms** |   **880.8460 ms** |  **48.2821 ms** | **1.001** |    **0.03** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,721.246 ms | 1,339.9912 ms |  73.4494 ms | 1.009 |    0.04 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.565 ms |     0.7808 ms |   0.0428 ms | 0.001 |    0.00 |   318.3594 |   318.3594 |   318.3594 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,193.026 ms** | **1,998.8525 ms** | **109.5639 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,183.956 ms | 1,088.2274 ms |  59.6494 ms | 0.999 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      7.299 ms |     1.6631 ms |   0.0912 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,018.449 ms** | **4,203.0589 ms** | **230.3838 ms** | **1.001** |    **0.04** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  6,941.202 ms | 2,598.9018 ms | 142.4546 ms | 0.990 |    0.03 |  1000.0000 |  1000.0000 |  1000.0000 |  42.13 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      8.261 ms |     1.4803 ms |   0.0811 ms | 0.001 |    0.00 |   343.7500 |   343.7500 |   343.7500 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **28,968.134 ms** | **2,834.4578 ms** | **155.3662 ms** | **1.000** |    **0.01** | **15000.0000** | **14000.0000** | **13000.0000** |  **388.8 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 29,014.979 ms | 3,401.2615 ms | 186.4346 ms | 1.002 |    0.01 | 20000.0000 | 19000.0000 | 18000.0000 |  385.2 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     17.805 ms |     1.8695 ms |   0.1025 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

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

| Method         | Documents | Permutations | Device               | Mean         | Error         | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |-------------:|--------------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |     **87.24 ms** |      **6.588 ms** |   **0.361 ms** |  **1.00** |    **0.01** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |  4,986.04 ms |  3,044.722 ms | 166.891 ms | 57.15 |    1.67 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |  4,920.48 ms |  1,722.213 ms |  94.400 ms | 56.40 |    0.96 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |    **103.94 ms** |      **4.345 ms** |   **0.238 ms** |  **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  9,931.53 ms |  3,137.327 ms | 171.967 ms | 95.55 |    1.45 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] | 10,125.18 ms |  2,681.159 ms | 146.963 ms | 97.41 |    1.24 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |    **884.55 ms** |     **73.910 ms** |   **4.051 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] | 49,836.05 ms |  1,388.327 ms |  76.099 ms | 56.34 |    0.24 |  4000.0000 |  3000.0000 | 2000.0000 | 160.06 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] | 49,758.69 ms | 10,816.181 ms | 592.871 ms | 56.25 |    0.62 |  7000.0000 |  4000.0000 | 2000.0000 | 211.62 MB |        7.11 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |  **1,028.81 ms** |     **13.512 ms** |   **0.741 ms** |  **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 99,131.13 ms | 13,220.052 ms | 724.636 ms | 96.35 |    0.61 | 12000.0000 | 11000.0000 | 8000.0000 | 536.09 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 98,763.93 ms | 13,328.966 ms | 730.606 ms | 96.00 |    0.62 | 15000.0000 | 12000.0000 | 8000.0000 | 587.68 MB |       14.00 |

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
| Lodestar_StudentCdf             |   295.58 ns |  16.975 ns |  0.930 ns |  1.00 |    0.00 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.95 ns |   5.273 ns |  0.289 ns |  1.01 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   338.35 ns |  15.830 ns |  0.868 ns |  1.14 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        |   678.35 ns |  27.806 ns |  1.524 ns |  2.30 |    0.01 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,653.51 ns | 162.085 ns |  8.884 ns | 19.13 |    0.06 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,201.43 ns |  30.689 ns |  1.682 ns |  4.06 |    0.01 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   136.07 ns |  50.204 ns |  2.752 ns |  0.46 |    0.01 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.90 ns |   2.663 ns |  0.146 ns |  0.45 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   138.07 ns |   2.740 ns |  0.150 ns |  0.47 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         |   203.29 ns |   3.877 ns |  0.213 ns |  0.69 |    0.00 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,677.03 ns | 234.326 ns | 12.844 ns |  9.06 |    0.05 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   320.98 ns |  17.768 ns |  0.974 ns |  1.09 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    30.03 ns |   2.238 ns |  0.123 ns |  0.10 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    60.80 ns |   1.659 ns |  0.091 ns |  0.21 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    72.76 ns |   2.548 ns |  0.140 ns |  0.25 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   368.65 ns |  38.174 ns |  2.092 ns |  1.25 |    0.01 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   474.29 ns |  57.771 ns |  3.167 ns |  1.60 |    0.01 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   391.06 ns |   3.841 ns |  0.211 ns |  1.32 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    21.53 ns |   0.049 ns |  0.003 ns |  0.07 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.31 ns |   2.399 ns |  0.131 ns |  0.08 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.07 ns |   1.049 ns |  0.058 ns |  0.22 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.80 ns |   1.101 ns |  0.060 ns |  0.10 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.75 ns |   2.364 ns |  0.130 ns |  0.10 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.63 ns |   3.204 ns |  0.176 ns |  0.61 |    0.00 |      - |         - |          NA |

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

| Method                     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  31.07 ns |  0.209 ns | 0.011 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  13.80 ns |  0.305 ns | 0.017 ns |  0.44 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  43.58 ns |  0.068 ns | 0.004 ns |  1.40 |    0.00 |         - |          NA |
| ChiSquaredSfHundredDf      |  76.48 ns | 11.226 ns | 0.615 ns |  2.46 |    0.02 |         - |          NA |
| ChiSquaredSfFractionalDf   | 118.51 ns |  2.656 ns | 0.146 ns |  3.81 |    0.00 |         - |          NA |
| NormalQuantile             |  28.92 ns |  4.526 ns | 0.248 ns |  0.93 |    0.01 |         - |          NA |

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
| **Lodestar_Glm** | **200**        | **1**          |  **36.13 μs** | **13.862 μs** | **0.760 μs** |  **1.00** |    **0.03** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  52.73 μs | 17.010 μs | 0.932 μs |  1.46 |    0.03 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.70 μs** |  **1.683 μs** | **0.092 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 109.82 μs | 18.023 μs | 0.988 μs |  2.63 |    0.02 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **351.18 μs** |  **6.393 μs** | **0.350 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 375.98 μs | 80.815 μs | 4.430 μs |  1.07 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **414.32 μs** |  **8.461 μs** | **0.464 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 740.44 μs | 76.319 μs | 4.183 μs |  1.79 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

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

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.99 μs** |   **1.126 μs** |  **0.062 μs** |  **1.00** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.67 μs |   0.792 μs |  0.043 μs |  2.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,019.15 μs** | **348.002 μs** | **19.075 μs** |  **1.00** | **328.1250** | **328.1250** | **328.1250** | **1097.74 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,777.77 μs |  21.138 μs |  1.159 μs |  1.94 | 546.8750 | 546.8750 | 546.8750 | 1878.09 KB |        1.71 |

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

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **322.0 μs** |  **1.53 μs** | **0.08 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **327.1 μs** | **10.02 μs** | **0.55 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **309.9 μs** |  **9.20 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **28.94 μs** |      **0.194 μs** |     **0.011 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     107.06 μs |     33.142 μs |     1.817 μs |  3.70 |    0.05 |   1.9531 |        - |        - |   33.01 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **671.53 μs** |     **78.374 μs** |     **4.296 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,794.11 μs |    185.902 μs |    10.190 μs |  2.67 |    0.02 |  97.6563 |  97.6563 |  97.6563 |  370.13 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **7,246.24 μs** |    **681.132 μs** |    **37.335 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  22,251.97 μs |  3,103.963 μs |   170.139 μs |  3.07 |    0.02 | 281.2500 | 281.2500 | 281.2500 | 2227.51 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **52,403.36 μs** |  **1,386.598 μs** |    **76.004 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,086.10 μs | 61,047.027 μs | 3,346.194 μs |  2.69 |    0.06 |        - |        - |        - | 8848.61 KB |        1.12 |

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |     **6.610 μs** |   **0.7211 μs** |  **0.0395 μs** |  **1.00** |    **0.01** |   **0.1602** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |     9.593 μs |   0.1980 μs |  0.0109 μs |  1.45 |    0.01 |   0.5341 |        - |        - |    8.8 KB |        3.32 |
| Hac       | 100        | 4    |    19.171 μs |   0.5663 μs |  0.0310 μs |  2.90 |    0.02 |   0.7629 |        - |        - |  12.95 KB |        4.89 |
| Cluster   | 100        | 4    |     8.468 μs |   1.2451 μs |  0.0682 μs |  1.28 |    0.01 |   0.5951 |        - |        - |   9.76 KB |        3.68 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **434.863 μs** |  **16.1614 μs** |  **0.8859 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   906.148 μs |  25.9953 μs |  1.4249 μs |  2.08 |    0.00 | 124.0234 | 124.0234 | 124.0234 | 472.94 KB |        5.91 |
| Hac       | 10000      | 4    | 1,967.774 μs | 285.0405 μs | 15.6240 μs |  4.53 |    0.03 | 246.0938 | 246.0938 | 246.0938 | 863.94 KB |       10.80 |
| Cluster   | 10000      | 4    |   769.054 μs |  52.8429 μs |  2.8965 μs |  1.77 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 565.22 KB |        7.07 |

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

| Method        | Shape    | Mean        | Error      | StdDev   | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|---------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,428.66 μs** | **112.403 μs** | **6.161 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **46.98 μs** |   **4.282 μs** | **0.235 μs** | **0.2441** |   **4.12 KB** |

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
| **Exact**  | **141x141** | **563.1 ms** | **112.8 ms** | **6.18 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **456.1 ms** | **156.8 ms** | **8.60 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

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

| Method           | Size | Mean          | Error         | StdDev      | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|--------------:|------------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.131 μs** |     **0.3987 μs** |   **0.0219 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,561.287 μs |    58.3898 μs |   3.2005 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     18.349 μs |     5.1178 μs |   0.2805 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     93.343 μs |     7.3341 μs |   0.4020 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.136 μs** |     **3.7306 μs** |   **0.2045 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,673.426 μs | 2,091.7505 μs | 114.6559 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    542.669 μs |   317.3872 μs |  17.3971 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,340.915 μs |   376.6995 μs |  20.6482 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

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
| **Lodestar** | **200**        |   **134.9 μs** |    **30.98 μs** |  **1.70 μs** |  **1.00** |    **0.02** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   940.6 μs |    33.35 μs |  1.83 μs |  6.97 |    0.08 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,311.5 μs** |   **105.06 μs** |  **5.76 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,410.9 μs | 1,051.72 μs | 57.65 μs |  5.65 |    0.04 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

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

| Method       | SampleSize | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |     **6.616 μs** |   **0.0735 μs** | **0.0040 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   264.564 μs |  13.0068 μs | 0.7129 μs | 39.99 |    0.10 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    31.501 μs |   4.4987 μs | 0.2466 μs |  4.76 |    0.03 |   2.1973 |   0.0610 |        - |   35.67 KB |       13.47 |
|              |            |              |             |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **449.217 μs** |  **14.9848 μs** | **0.8214 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 5,137.617 μs |  27.1630 μs | 1.4889 μs | 11.44 |    0.02 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,760.460 μs | 169.4404 μs | 9.2876 μs |  3.92 |    0.02 | 441.4063 | 441.4063 | 441.4063 |  1765.4 KB |       22.07 |

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

| Method                       | Mean        | Error     | StdDev    | Ratio     | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |------------:|----------:|----------:|----------:|--------:|----------:|------------:|
| NormalQuantile               |   0.0408 ns | 0.0303 ns | 0.0017 ns |      1.00 |    0.05 |         - |          NA |
| NormalQuantileFarTail        |   0.0398 ns | 0.0210 ns | 0.0011 ns |      0.98 |    0.04 |         - |          NA |
| StudentQuantile              | 559.3424 ns | 5.8360 ns | 0.3199 ns | 13,710.47 |  494.13 |         - |          NA |
| StudentQuantileCauchyFarTail | 154.6985 ns | 2.9769 ns | 0.1632 ns |  3,791.93 |  136.69 |         - |          NA |

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
| **KruskalWallisTest**         | **10000**      | **False** |  **1,484.9 μs** |    **59.12 μs** |   **3.24 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    421.1 μs |    49.79 μs |   2.73 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    613.8 μs |   117.92 μs |   6.46 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,223.3 μs |    38.11 μs |   2.09 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    710.4 μs |    29.63 μs |   1.62 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **620.1 μs** |     **8.73 μs** |   **0.48 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    245.1 μs |    35.89 μs |   1.97 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    521.9 μs |    30.69 μs |   1.68 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    790.6 μs |    16.95 μs |   0.93 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    393.1 μs |     5.55 μs |   0.30 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,846.1 μs** |   **938.42 μs** |  **51.44 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,520.3 μs |   117.90 μs |   6.46 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,660.0 μs |   400.38 μs |  21.95 μs | 640.6250 | 640.6250 | 640.6250 |  5062774 B |
| WilcoxonPooledDifferences | 100000     | False | 17,398.1 μs | 2,503.57 μs | 137.23 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,122.4 μs |   256.03 μs |  14.03 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,072.7 μs** |    **87.22 μs** |   **4.78 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,838.4 μs |   201.35 μs |  11.04 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,299.8 μs |   490.72 μs |  26.90 μs | 578.1250 | 578.1250 | 578.1250 |  5062733 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,677.5 μs | 1,743.56 μs |  95.57 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,763.9 μs |    79.41 μs |   4.35 μs |        - |        - |        - |          - |

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

| Method | SampleSize | Covariance | Mean         | Error       | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-------------:|------------:|-----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |     **6.902 μs** |   **0.2502 μs** |  **0.0137 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |     **9.995 μs** |   **3.8078 μs** |  **0.2087 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc1**        |     **9.967 μs** |   **1.2980 μs** |  **0.0712 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.431 μs** |   **2.8350 μs** |  **0.1554 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.461 μs** |   **0.3627 μs** |  **0.0199 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **452.606 μs** |  **24.3880 μs** |  **1.3368 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **911.711 μs** |  **14.8746 μs** |  **0.8153 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **943.792 μs** | **399.4210 μs** | **21.8936 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,044.861 μs** | **216.5296 μs** | **11.8687 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,056.927 μs** |  **73.6290 μs** |  **4.0359 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |

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

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.467 μs** | **0.5187 μs** | **0.0284 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.547 μs | 0.5569 μs | 0.0305 μs |  2.58 |    0.02 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.240 μs | 0.5154 μs | 0.0283 μs |  1.17 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.169 μs | 0.3492 μs | 0.0191 μs |  2.72 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.023 μs | 0.4510 μs | 0.0247 μs |  1.12 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.664 μs | 0.4756 μs | 0.0261 μs |  2.61 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.946 μs** | **0.6391 μs** | **0.0350 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.933 μs | 1.8771 μs | 0.1029 μs |  2.47 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.794 μs | 1.1045 μs | 0.0605 μs |  1.02 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.643 μs | 2.1429 μs | 0.1175 μs |  2.49 |    0.00 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.840 μs | 5.5184 μs | 0.3025 μs |  1.02 |    0.01 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 121.102 μs | 2.3553 μs | 0.1291 μs |  2.47 |    0.00 |      - |     224 B |        0.01 |

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

| Method                        | SampleSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.885 μs** |   **6.0130 μs** | **0.3296 μs** |  **1.00** |    **0.05** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  30.841 μs |   8.0446 μs | 0.4410 μs |  3.92 |    0.15 |   4.0283 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  25.323 μs |   5.7309 μs | 0.3141 μs |  3.22 |    0.12 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.864 μs |   0.1856 μs | 0.0102 μs |  0.49 |    0.02 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.039 μs |   0.5058 μs | 0.0277 μs |  0.51 |    0.02 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.568 μs |   0.9203 μs | 0.0504 μs |  0.20 |    0.01 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.903 μs |   0.8079 μs | 0.0443 μs |  0.37 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |             |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **116.205 μs** |  **12.4216 μs** | **0.6809 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 978.923 μs | 110.7417 μs | 6.0701 μs |  8.42 |    0.06 | 248.0469 | 248.0469 | 248.0469 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 292.130 μs |  26.1419 μs | 1.4329 μs |  2.51 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.806 μs |   3.4089 μs | 0.1869 μs |  0.82 |    0.00 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.841 μs |  13.7544 μs | 0.7539 μs |  0.84 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.132 μs |  13.9070 μs | 0.7623 μs |  0.12 |    0.01 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  28.112 μs |   6.8910 μs | 0.3777 μs |  0.24 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

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

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **1,140.1 ns** |      **51.60 ns** |      **2.83 ns** |    **1.00** |    **0.00** |   **0.0038** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,815.5 ns |   2,443.49 ns |    133.94 ns |   34.05 |    0.13 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,153.5 ns |      61.92 ns |      3.39 ns |    1.89 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     24,148.9 ns |   2,685.86 ns |    147.22 ns |   21.18 |    0.12 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        119.5 ns |      38.90 ns |      2.13 ns |    0.10 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        222.1 ns |       4.40 ns |      0.24 ns |    0.19 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **52,030.0 ns** |   **5,576.97 ns** |    **305.69 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    141,792.3 ns |  17,917.04 ns |    982.09 ns |   2.725 |    0.02 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    690,316.7 ns |   3,041.01 ns |    166.69 ns |  13.268 |    0.07 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 12,490,768.1 ns | 343,220.67 ns | 18,813.08 ns | 240.074 |    1.26 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        114.7 ns |       4.45 ns |      0.24 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        215.3 ns |      14.06 ns |      0.77 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **16.91 μs** |   **4.064 μs** |  **0.223 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **37.71 μs** |   **3.724 μs** |  **0.204 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **55.27 μs** |   **5.428 μs** |  **0.298 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **164.32 μs** |  **14.860 μs** |  **0.815 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **217.62 μs** |  **34.698 μs** |  **1.902 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **575.14 μs** | **201.466 μs** | **11.043 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **741.93 μs** |  **84.536 μs** |  **4.634 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,278.83 μs** | **239.297 μs** | **13.117 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

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

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **11.60 μs** |      **0.719 μs** |     **0.039 μs** |  **1.00** |    **0.00** |    **0.1984** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     19.65 μs |      3.483 μs |     0.191 μs |  1.69 |    0.02 |    3.6926 |    0.1831 |         - |    58.16 KB |       16.96 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **91.68 μs** |      **3.901 μs** |     **0.214 μs** |  **1.00** |    **0.00** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    124.51 μs |     10.087 μs |     0.553 μs |  1.36 |    0.01 |   32.9590 |   14.4043 |         - |   508.26 KB |       29.06 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **971.67 μs** |    **102.314 μs** |     **5.608 μs** |  **1.00** |    **0.01** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.51 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,517.71 μs |  2,063.153 μs |   113.088 μs |  1.56 |    0.10 | 1074.2188 | 1074.2188 | 1074.2188 |  5024.68 KB |       31.70 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,676.54 μs** |    **325.847 μs** |    **17.861 μs** |  **1.00** |    **0.00** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.47 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 20,329.24 μs | 26,605.045 μs | 1,458.312 μs |  2.10 |    0.13 |  937.5000 |  937.5000 |  937.5000 | 50025.66 KB |       31.98 |

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
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.151 ms** | **0.4981 ms** | **0.0273 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.101 ms | 0.0558 ms | 0.0031 ms |        - |        - |        - |   12.06 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.397 ms | 0.3604 ms | 0.0198 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.010 ms** | **0.2458 ms** | **0.0135 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.183 ms | 0.2229 ms | 0.0122 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.130 ms | 1.2055 ms | 0.0661 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

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

| Method              | Length | Mean            | Error           | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|----------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **906.5 ns** |        **92.72 ns** |      **5.08 ns** |  **1.00** |    **0.01** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,846.9 ns |       201.49 ns |     11.04 ns |  2.04 |    0.01 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |              |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **66,082.9 ns** |    **24,802.40 ns** |  **1,359.50 ns** |  **1.00** |    **0.03** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,919.7 ns |     6,745.94 ns |    369.77 ns |  2.30 |    0.04 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |              |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,364,285.2 ns** | **1,747,160.28 ns** | **95,767.75 ns** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 11,614,297.0 ns |   538,317.47 ns | 29,507.00 ns |  2.66 |    0.05 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 283.7 μs | 66.14 μs | 3.63 μs | 29.7852 |  488.6 KB |

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
| **SearchTop10** | **10000**  |   **552.8 μs** | **196.09 μs** | **10.75 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,472.5 μs** |  **92.15 μs** |  **5.05 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-KBRPOB : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-KBRPOB** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **277.96 μs** |    **14.569 μs** |    **16.194 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    273.38 μs |    19.144 μs |    21.279 μs |  0.99 |    0.09 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    106.06 μs |     7.236 μs |     8.043 μs |  0.38 |    0.04 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    428.10 μs |    24.216 μs |    26.916 μs |  1.54 |    0.13 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,637.27 μs |   116.377 μs |   129.352 μs |  5.91 |    0.56 |        - |        - |        - |  339.67 KB |        2.04 |
| MlNet_ReplaceMissingValues_Read | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,243.28 μs |    34.844 μs |    38.729 μs |  4.49 |    0.28 |        - |        - |        - |   312.3 KB |        1.87 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    173.69 μs |    39.506 μs |     2.165 μs |  1.00 |    0.02 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     70.07 μs |    11.188 μs |     0.613 μs |  0.40 |    0.01 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     17.89 μs |     1.682 μs |     0.092 μs |  0.10 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    968.94 μs | 2,502.907 μs |   137.193 μs |  5.58 |    0.69 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    939.05 μs |   652.935 μs |    35.790 μs |  5.41 |    0.19 |  48.8281 |   7.8125 |        - |  796.77 KB |        4.80 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    257.58 μs |    48.024 μs |     2.632 μs |  1.48 |    0.02 |  19.0430 |  18.5547 |   0.4883 |  295.94 KB |        1.78 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-KBRPOB** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **6,137.53 μs** |   **814.614 μs** |   **871.627 μs** |  **1.02** |    **0.21** |        **-** |        **-** |        **-** |  **3283.7 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,764.37 μs |   472.845 μs |   485.577 μs |  0.79 |    0.14 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |    702.37 μs |     9.926 μs |    11.431 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,459.68 μs |    41.332 μs |    45.941 μs |  0.74 |    0.11 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,757.43 μs | 6,257.248 μs | 7,205.856 μs |  2.29 |    1.23 |        - |        - |        - |  442.19 KB |        0.13 |
| MlNet_ReplaceMissingValues_Read | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,990.46 μs | 1,707.029 μs | 1,826.502 μs |  1.99 |    0.42 |        - |        - |        - |  522.03 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,521.77 μs |   755.776 μs |    41.427 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3291.66 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,216.24 μs |   635.609 μs |    34.840 μs |  0.91 |    0.01 |  89.8438 |  89.8438 |  89.8438 |   314.4 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    423.14 μs |    60.728 μs |     3.329 μs |  0.12 |    0.00 | 126.4648 | 126.4648 | 126.4648 |  626.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,549.77 μs | 1,212.673 μs |    66.471 μs |  0.44 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,257.62 μs | 1,180.101 μs |    64.685 μs |  1.21 |    0.02 |  31.2500 |  15.6250 |        - |  567.08 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,459.06 μs |   275.208 μs |    15.085 μs |  0.70 |    0.01 |  27.3438 |  11.7188 |        - |  457.22 KB |        0.14 |

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
| **Distance_Utf16**     | **8**      |     **38.38 ns** |      **0.742 ns** |   **0.041 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    125.90 ns |     15.844 ns |   0.868 ns |  3.28 |    0.02 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **144.36 ns** |     **10.205 ns** |   **0.559 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,484.10 ns |     51.951 ns |   2.848 ns | 10.28 |    0.04 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **324.54 ns** |     **48.453 ns** |   **2.656 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,391.47 ns |  2,616.182 ns | 143.402 ns | 28.94 |    0.43 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **47,636.89 ns** | **16,491.392 ns** | **903.949 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 49,364.60 ns |  1,925.474 ns | 105.542 ns |  1.04 |    0.02 |         - |          NA |

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
| **Extract**    | **1**     | **6.870 ms** | **0.1199 ms** | **0.0066 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.718 ms | 0.6038 ms | 0.0331 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.867 ms** | **0.1624 ms** | **0.0089 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.775 ms | 1.0389 ms | 0.0569 ms |  0.99 |         - |        0.00 |

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
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.802 μs** | **0.4628 μs** | **0.0254 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **4.017 μs** | **0.3995 μs** | **0.0219 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.879 μs** | **0.2624 μs** | **0.0144 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.958 μs** | **0.7458 μs** | **0.0409 μs** | **0.0076** |     **192 B** |

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

| Method                   | Length | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **377.2 ns** |     **33.32 ns** |     **1.83 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,312.5 ns |     67.75 ns |     3.71 ns | 14.08 |    0.06 |         - |          NA |
|                          |        |              |              |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,727.6 ns** |    **306.34 ns** |    **16.79 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 597,167.2 ns | 21,281.11 ns | 1,166.49 ns | 26.27 |    0.05 |         - |          NA |

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
| **Fit**    | **2000x2000x20** | **27.74 ms** | **0.314 ms** | **0.017 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.70 ms** | **0.841 ms** | **0.046 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-URSTNN : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error          | StdDev        | Median        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|---------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-URSTNN** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **108.310 μs** |      **4.9076 μs** |     **4.8199 μs** |    **108.578 μs** |  **1.00** |    **0.06** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    120.010 μs |     11.9127 μs |    12.2335 μs |    117.801 μs |  1.11 |    0.12 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,447.141 μs |     36.1895 μs |    41.6759 μs |  2,439.638 μs | 22.64 |    1.06 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    117.695 μs |     24.1000 μs |    27.7536 μs |    107.204 μs |  1.09 |    0.26 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     69.517 μs |      1.2489 μs |     1.2265 μs |     69.434 μs |  0.64 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     59.183 μs |      1.8339 μs |     1.7154 μs |     58.791 μs |  0.55 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     97.377 μs |      6.1720 μs |     6.0618 μs |     96.400 μs |  0.90 |    0.07 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    441.020 μs |     18.9928 μs |    21.8721 μs |    440.596 μs |  4.08 |    0.27 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,336.484 μs |     80.5581 μs |    89.5401 μs |  2,352.125 μs | 21.61 |    1.24 |        - |        - |        - |  297216 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,524.794 μs |    199.2712 μs |   221.4895 μs |  6,541.446 μs | 60.35 |    3.31 |        - |        - |        - |  441928 B |        5.41 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.943 μs |      7.1607 μs |     0.3925 μs |     48.131 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     41.225 μs |      4.9336 μs |     0.2704 μs |     41.113 μs |  0.86 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.192 μs |     15.4023 μs |     0.8442 μs |    465.027 μs |  9.70 |    0.07 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     49.039 μs |     11.1971 μs |     0.6138 μs |     48.783 μs |  1.02 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.503 μs |      0.7460 μs |     0.0409 μs |     10.483 μs |  0.22 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.695 μs |      0.6208 μs |     0.0340 μs |      7.682 μs |  0.16 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.989 μs |      2.5880 μs |     0.1419 μs |     12.032 μs |  0.25 |    0.00 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,631.691 μs |  4,433.8538 μs |   243.0345 μs |  1,635.675 μs | 34.04 |    4.40 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,033.857 μs |    826.4159 μs |    45.2986 μs |  1,051.448 μs | 21.57 |    0.83 |  39.0625 |   1.9531 |        - |  650157 B |       8.037 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,629.417 μs |  1,829.4391 μs |   100.2777 μs |  1,629.426 μs | 33.99 |    1.83 |  35.1563 |        - |        - |  627325 B |       7.755 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-URSTNN** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,611.860 μs** |     **15.2683 μs** |    **17.5830 μs** |  **1,613.027 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,481.423 μs |     10.7627 μs |    11.9627 μs |  1,477.695 μs |  0.92 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,375.404 μs |     94.8542 μs |   101.4929 μs | 15,378.748 μs |  9.54 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,573.580 μs |     12.5761 μs |    13.9782 μs |  1,574.230 μs |  0.98 |    0.01 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    422.618 μs |      9.9406 μs |    11.0489 μs |    417.528 μs |  0.26 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    318.649 μs |      9.8249 μs |    11.3144 μs |    313.014 μs |  0.20 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    433.017 μs |     13.6537 μs |    15.1761 μs |    433.508 μs |  0.27 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  5,866.833 μs |  1,950.5136 μs | 2,167.9907 μs |  6,807.608 μs |  3.64 |    1.31 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 18,424.451 μs |  8,563.5513 μs | 9,861.7984 μs | 12,808.826 μs | 11.43 |    5.97 |        - |        - |        - |  516192 B |       0.322 |
| MlNet_NormalizeRobustScaling_Read | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 21,973.432 μs |    689.6923 μs |   737.9630 μs | 22,205.928 μs | 13.63 |    0.47 |        - |        - |        - | 4279848 B |       2.672 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,523.490 μs |    668.8562 μs |    36.6623 μs |  1,521.155 μs |  1.00 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601922 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,439.488 μs |  2,317.5304 μs |   127.0317 μs |  1,479.197 μs |  0.95 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,802.830 μs |  1,450.6683 μs |    79.5160 μs | 14,819.685 μs |  9.72 |    0.21 | 875.0000 | 875.0000 | 875.0000 | 3201322 B |       1.998 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,616.658 μs |    813.3229 μs |    44.5810 μs |  1,594.456 μs |  1.06 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601052 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    307.581 μs |     28.7203 μs |     1.5743 μs |    306.959 μs |  0.20 |    0.00 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    244.912 μs |      8.3977 μs |     0.4603 μs |    244.715 μs |  0.16 |    0.00 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    432.573 μs |    231.1587 μs |    12.6706 μs |    433.063 μs |  0.28 |    0.01 | 128.9063 | 124.0234 | 124.0234 |  560074 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,251.112 μs |    983.5143 μs |    53.9097 μs |  1,247.066 μs |  0.82 |    0.04 |  19.5313 |   3.9063 |        - |  354153 B |       0.221 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,447.853 μs | 19,238.3780 μs | 1,054.5204 μs |  5,225.582 μs |  3.58 |    0.60 |  31.2500 |        - |        - |  545588 B |       0.341 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,743.218 μs |  1,407.5005 μs |    77.1499 μs | 19,719.509 μs | 12.96 |    0.27 | 250.0000 |  93.7500 |        - | 4299910 B |       2.684 |

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
| **PerSample** | **2000**    |  **27.33 ms** | **0.169 ms** | **0.009 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **193.20 ms** | **2.533 ms** | **0.139 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

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

| Method                          | SampleCount | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **75.856 μs** |     **33.782 μs** |   **1.8517 μs** |  **1.00** |    **0.03** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    184.198 μs |      5.008 μs |   0.2745 μs |  2.43 |    0.05 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.744 μs |      1.923 μs |   0.1054 μs |  0.06 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    335.243 μs |      3.112 μs |   0.1706 μs |  4.42 |    0.09 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,721.511 μs |    561.935 μs |  30.8015 μs | 75.46 |    1.63 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     62.045 μs |      2.201 μs |   0.1207 μs |  0.82 |    0.02 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,112.152 μs |    293.771 μs |  16.1026 μs | 14.67 |    0.36 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |               |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,104.352 μs** |    **584.553 μs** |  **32.0413 μs** |  **1.00** |    **0.04** | **441.4063** | **427.7344** | **417.9688** | **2738.33 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,343.239 μs |    256.226 μs |  14.0446 μs |  2.12 |    0.05 | 535.1563 | 515.6250 | 511.7188 | 3129.98 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     84.112 μs |     25.565 μs |   1.4013 μs |  0.08 |    0.00 |  55.5420 |  55.5420 |  55.5420 |  391.16 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    278.424 μs |    227.381 μs |  12.4635 μs |  0.25 |    0.01 |   1.9531 |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 45,251.294 μs | 12,758.285 μs | 699.3247 μs | 41.00 |    1.17 |        - |        - |        - |  148.74 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     64.489 μs |      3.756 μs |   0.2059 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,620.893 μs |    299.776 μs |  16.4318 μs |  6.90 |    0.17 |        - |        - |        - |   34.19 KB |       0.012 |

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

| Method  | Words | Mean        | Error     | StdDev  | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|----------:|--------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **637.4 μs** |  **11.88 μs** | **0.65 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,818.9 μs** | **106.11 μs** | **5.82 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,864.7 μs** |  **20.49 μs** | **1.12 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

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
| **TopTwo** | **10**      | **11.65 ms** | **1.087 ms** | **0.060 ms** |         **-** |
| **TopTwo** | **100**     | **88.27 ms** | **0.948 ms** | **0.052 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-MKGWOJ : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-MKGWOJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **54.59 μs** |    **11.893 μs** |    **13.696 μs** |     **48.29 μs** |   **1.05** |    **0.34** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |     89.66 μs |     1.790 μs |     1.838 μs |     89.39 μs |   1.73 |    0.35 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    171.26 μs |     5.989 μs |     6.150 μs |    170.62 μs |   3.30 |    0.68 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    262.92 μs |     6.781 μs |     7.255 μs |    263.03 μs |   5.06 |    1.04 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,003.99 μs |    19.321 μs |    22.251 μs |    997.22 μs |  19.34 |    3.94 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,623.45 μs |    19.390 μs |    21.552 μs |  1,621.88 μs |  31.27 |    6.35 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,113.17 μs |    18.593 μs |    19.894 μs |  1,114.73 μs |  21.44 |    4.36 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    211.33 μs |     9.557 μs |     9.814 μs |    209.79 μs |   4.07 |    0.85 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,894.86 μs |    14.485 μs |    16.100 μs |  1,893.88 μs |  36.50 |    7.41 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     | 14,218.72 μs | 5,047.879 μs | 5,813.145 μs | 10,349.89 μs | 273.91 |  124.56 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,280.56 μs |    57.047 μs |    61.040 μs |  1,286.18 μs |  24.67 |    5.14 |        - |        - |        - |  315.95 KB |        9.88 |
| MlNet_NormalizeBinning_Read | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,151.48 μs |   143.970 μs |   160.023 μs |  3,163.46 μs |  60.71 |   12.68 |        - |        - |        - |  426.02 KB |       13.32 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.87 μs |     2.549 μs |     0.140 μs |     13.95 μs |   1.00 |    0.01 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.08 μs |     8.841 μs |     0.485 μs |     83.02 μs |   5.99 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    139.15 μs |    12.500 μs |     0.685 μs |    139.15 μs |  10.03 |    0.10 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     75.19 μs |    13.953 μs |     0.765 μs |     74.82 μs |   5.42 |    0.07 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     52.95 μs |    23.046 μs |     1.263 μs |     52.43 μs |   3.82 |    0.09 |   2.0142 |   0.0610 |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.62 μs |    22.581 μs |     1.238 μs |    466.24 μs |  33.56 |    0.30 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    179.71 μs |    38.759 μs |     2.125 μs |    180.76 μs |  12.95 |    0.17 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    140.44 μs |     8.389 μs |     0.460 μs |    140.27 μs |  10.12 |    0.09 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    725.72 μs |   132.181 μs |     7.245 μs |    722.80 μs |  52.31 |    0.64 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,753.96 μs |   410.992 μs |    22.528 μs |  8,746.03 μs | 631.02 |    5.71 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    258.97 μs |    29.433 μs |     1.613 μs |    259.00 μs |  18.67 |    0.19 |  19.0430 |  18.5547 |        - |  300.56 KB |        9.61 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,002.62 μs | 1,216.802 μs |    66.697 μs |    998.44 μs |  72.27 |    4.21 |  42.9688 |   5.8594 |        - |  698.35 KB |       22.33 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-MKGWOJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **481.65 μs** |    **10.440 μs** |    **12.023 μs** |    **479.87 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,707.25 μs |    16.383 μs |    18.867 μs |  1,707.17 μs |   3.55 |    0.09 |        - |        - |        - | 2344.45 KB |        3.75 |
| Lodestar_Discretize         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,748.19 μs |    29.051 μs |    32.290 μs |  3,744.75 μs |   7.79 |    0.20 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,522.49 μs |    18.927 μs |    20.252 μs |  4,521.26 μs |   9.40 |    0.23 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,189.80 μs | 5,674.587 μs | 6,534.862 μs |  5,830.30 μs |  23.25 |   13.26 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,942.95 μs |   719.178 μs |   706.329 μs | 10,654.64 μs |  22.73 |    1.53 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,610.30 μs | 5,754.001 μs | 6,626.316 μs |  5,921.54 μs |  24.12 |   13.45 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,069.41 μs |    12.493 μs |    14.387 μs |  3,066.36 μs |   6.38 |    0.16 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Power_Fit          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,761.35 μs | 3,588.118 μs | 4,132.082 μs | 14,818.96 μs |  36.90 |    8.43 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,392.88 μs | 5,092.480 μs | 5,864.508 μs | 10,531.51 μs |  29.90 |   11.91 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,137.72 μs | 1,303.121 μs | 1,500.676 μs | 12,786.83 μs |  25.22 |    3.10 |        - |        - |        - |   555.3 KB |        0.89 |
| MlNet_NormalizeBinning_Read | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,483.14 μs | 2,744.243 μs | 2,695.213 μs | 15,576.75 μs |  32.17 |    5.49 |        - |        - |        - | 2074.73 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    686.53 μs |    83.562 μs |     4.580 μs |    688.24 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    640.89 μs |   126.270 μs |     6.921 μs |    637.96 μs |   0.93 |    0.01 | 255.8594 | 255.8594 | 255.8594 | 2346.09 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,226.20 μs |   133.898 μs |     7.339 μs |  3,227.96 μs |   4.70 |    0.03 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,972.22 μs |   694.558 μs |    38.071 μs |  1,982.87 μs |   2.87 |    0.05 | 199.2188 | 199.2188 | 199.2188 |  625.15 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,799.46 μs |   499.765 μs |    27.394 μs |  5,809.56 μs |   8.45 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 11,012.29 μs |   235.020 μs |    12.882 μs | 11,014.43 μs |  16.04 |    0.09 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,956.76 μs |   170.413 μs |     9.341 μs |  5,959.81 μs |   8.68 |    0.05 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,226.44 μs |   112.281 μs |     6.155 μs |  3,226.23 μs |   4.70 |    0.03 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,767.64 μs |   389.995 μs |    21.377 μs | 13,773.16 μs |  20.05 |    0.12 | 750.0000 | 750.0000 | 750.0000 |  2662.4 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,178.91 μs |   426.792 μs |    23.394 μs |  9,180.47 μs |  13.37 |    0.08 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,383.13 μs |   991.693 μs |    54.358 μs |  2,401.33 μs |   3.47 |    0.07 |  27.3438 |  11.7188 |        - |  467.76 KB |        0.75 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,813.47 μs | 1,377.963 μs |    75.531 μs | 10,848.19 μs |  15.75 |    0.13 | 265.6250 | 218.7500 | 187.5000 | 2132.33 KB |        3.41 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `transformers`
- `cdist`
- `nmf-transform`
- `glm`
- `var`
- `iv`
- `panel`
- `cross-conformal`
- `onehot`
- `cluster-weighted`
- `survival-family`
- `cox-extended`
- `survival-parametric`
- `survival-aalen`
- `ols`

### compare-cdist

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.174 | 0.035 | 0.20x | 0.174 | 0.035 | 0.20x |
| cdist_wratio_n50 | 2.248 | 2.470 | 1.10x | 2.248 | 2.470 | 1.10x |
| cdist_ratio_varied_n50 | 0.147 | 0.031 | 0.21x | 0.147 | 0.031 | 0.21x |
| cdist_ratio_varied_cutoff90_n50 | 0.058 | 0.031 | 0.54x | 0.058 | 0.031 | 0.54x |
| cdist_ratio_n200 | 2.926 | 0.435 | 0.15x | 2.930 | 0.435 | 0.15x |
| cdist_wratio_n200 | 34.686 | 37.252 | 1.07x | 34.689 | 37.250 | 1.07x |
| cdist_ratio_varied_n200 | 2.445 | 0.384 | 0.16x | 2.449 | 0.384 | 0.16x |
| cdist_ratio_varied_cutoff90_n200 | 1.028 | 0.384 | 0.37x | 1.030 | 0.384 | 0.37x |
| cdist_ratio_n500 | 17.514 | 2.652 | 0.15x | 17.531 | 2.652 | 0.15x |
| cdist_wratio_n500 | 207.228 | 227.035 | 1.10x | 207.376 | 227.023 | 1.09x |
| cdist_ratio_varied_n500 | 14.244 | 2.308 | 0.16x | 14.362 | 2.308 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 5.635 | 2.308 | 0.41x | 5.738 | 2.307 | 0.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.274 | 3.917 | 0.74x | 5.283 | 7.827 | 1.48x |
| kmeans_restarts_10000 | 21.303 | 10.400 | 0.49x | 21.312 | 20.775 | 0.97x |
| kmeans_weighted_100000 | 51.789 | 28.201 | 0.54x | 52.368 | 47.478 | 0.91x |
| kmeans_restarts_100000 | 205.532 | 73.345 | 0.36x | 206.194 | 135.617 | 0.66x |
| dbscan_weighted_5000 | 8.231 | 33.719 | 4.10x | 8.834 | 33.717 | 3.82x |
| dbscan_weighted_20000 | 102.028 | 198.750 | 1.95x | 103.474 | 198.740 | 1.92x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.865 | 128.156 | 148.14x | 0.865 | 128.144 | 148.13x |
| ridge_robust_1000 | 0.935 | 119.601 | 127.88x | 0.935 | 119.589 | 127.87x |
| lasso_1000 | 6.697 | 1134.131 | 169.36x | 6.698 | 1134.043 | 169.32x |
| ph_test_1000 | 0.128 | 21.738 | 169.34x | 0.128 | 21.737 | 169.30x |
| predict_survival_1000 | 0.032 | 1.868 | 59.17x | 0.032 | 1.868 | 59.15x |
| time_varying_1000 | 79.338 | 356.972 | 4.50x | 80.474 | 356.946 | 4.44x |
| stratified_weighted_10000 | 9.141 | 733.966 | 80.30x | 9.181 | 733.939 | 79.94x |
| ridge_robust_10000 | 10.248 | 1138.689 | 111.11x | 10.484 | 1138.619 | 108.60x |
| lasso_10000 | 60.575 | 7347.119 | 121.29x | 60.599 | 7346.760 | 121.24x |
| ph_test_10000 | 2.020 | 216.769 | 107.30x | 2.090 | 216.763 | 103.73x |
| predict_survival_10000 | 0.033 | 1.842 | 56.34x | 0.033 | 1.842 | 56.34x |
| time_varying_10000 | 3060.417 | 5201.403 | 1.70x | 3060.465 | 5201.174 | 1.70x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 4.204 | 51.269 | 12.20x | 4.203 | 204.550 | 48.67x |
| cv_minmax_n1000 | 3.271 | 5.984 | 1.83x | 3.271 | 23.892 | 7.30x |
| cv_plus_gamma_n1000 | 4.751 | 51.253 | 10.79x | 4.751 | 203.862 | 42.91x |
| bootstrap_plus_n1000 | 16.469 | 51.483 | 3.13x | 16.474 | 205.437 | 12.47x |
| cv_plus_n10000 | 40.490 | 129.623 | 3.20x | 40.492 | 473.740 | 11.70x |
| cv_minmax_n10000 | 30.950 | 26.295 | 0.85x | 30.950 | 105.011 | 3.39x |
| cv_plus_gamma_n10000 | 58.629 | 138.738 | 2.37x | 58.627 | 483.720 | 8.25x |
| bootstrap_plus_n10000 | 152.746 | 133.564 | 0.87x | 152.735 | 484.776 | 3.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.401 | 3.457 | 8.61x | 0.401 | 3.456 | 8.61x |
| glm_gamma_n1000 | 0.479 | 4.282 | 8.93x | 0.479 | 4.282 | 8.93x |
| glm_poisson_exposure_n1000 | 0.403 | 4.059 | 10.06x | 0.403 | 4.058 | 10.06x |
| mnlogit_n1000 | 0.803 | 18.452 | 22.99x | 0.802 | 18.450 | 22.99x |
| glm_negative_binomial_n10000 | 3.623 | 11.785 | 3.25x | 3.632 | 11.784 | 3.24x |
| glm_gamma_n10000 | 4.145 | 13.143 | 3.17x | 4.154 | 13.142 | 3.16x |
| glm_poisson_exposure_n10000 | 4.378 | 13.156 | 3.01x | 4.387 | 13.155 | 3.00x |
| mnlogit_n10000 | 8.133 | 98.999 | 12.17x | 8.143 | 98.995 | 12.16x |
| glm_negative_binomial_n100000 | 37.333 | 147.681 | 3.96x | 37.634 | 589.565 | 15.67x |
| glm_gamma_n100000 | 42.990 | 173.247 | 4.03x | 43.303 | 691.746 | 15.97x |
| glm_poisson_exposure_n100000 | 56.639 | 130.921 | 2.31x | 56.533 | 522.311 | 9.24x |
| mnlogit_n100000 | 82.143 | 941.802 | 11.47x | 82.770 | 1691.267 | 20.43x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.612 | 55.022 | 34.12x | 1.612 | 55.019 | 34.12x |
| liml_robust_n1000 | 1.765 | 57.326 | 32.49x | 1.764 | 57.323 | 32.49x |
| gmm_robust_n1000 | 1.617 | 55.292 | 34.19x | 1.617 | 55.291 | 34.19x |
| 2sls_kernel_n1000 | 8.847 | 58.368 | 6.60x | 8.846 | 58.364 | 6.60x |
| 2sls_clustered_n1000 | 1.568 | 56.739 | 36.19x | 1.568 | 56.735 | 36.19x |
| 2sls_robust_n10000 | 16.407 | 240.142 | 14.64x | 16.879 | 480.375 | 28.46x |
| liml_robust_n10000 | 17.944 | 249.668 | 13.91x | 18.446 | 499.158 | 27.06x |
| gmm_robust_n10000 | 16.592 | 241.289 | 14.54x | 17.075 | 482.525 | 28.26x |
| 2sls_kernel_n10000 | 94.487 | 255.476 | 2.70x | 94.933 | 510.971 | 5.38x |
| 2sls_clustered_n10000 | 15.539 | 255.496 | 16.44x | 15.921 | 510.983 | 32.09x |
| 2sls_robust_n100000 | 184.511 | 2580.589 | 13.99x | 187.972 | 8044.809 | 42.80x |
| liml_robust_n100000 | 207.255 | 2585.549 | 12.48x | 214.108 | 8068.107 | 37.68x |
| gmm_robust_n100000 | 193.652 | 2500.300 | 12.91x | 194.165 | 7650.831 | 39.40x |
| 2sls_kernel_n100000 | 3496.212 | 3282.434 | 0.94x | 3498.504 | 10318.336 | 2.95x |
| 2sls_clustered_n100000 | 176.627 | 2794.905 | 15.82x | 180.428 | 8927.226 | 49.48x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.041 | 0.855 | 0.42x | 2.041 | 0.855 | 0.42x |
| transform_frobenius_n500 | 6.553 | 1.632 | 0.25x | 6.553 | 1.632 | 0.25x |
| transform_frobenius_n2000 | 23.818 | 5.881 | 0.25x | 23.854 | 23.487 | 0.98x |
| transform_kl_n100 | 1.254 | 17.897 | 14.28x | 1.254 | 17.896 | 14.28x |
| transform_kl_n500 | 5.704 | 34.598 | 6.07x | 5.703 | 34.597 | 6.07x |
| transform_kl_n2000 | 22.613 | 122.924 | 5.44x | 22.670 | 451.556 | 19.92x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.046 | 3.055 | 66.83x | 0.046 | 3.055 | 66.84x |
| ols_hac_n1000 | 0.161 | 3.521 | 21.86x | 0.161 | 3.521 | 21.85x |
| ols_cluster_n1000 | 0.062 | 3.567 | 57.40x | 0.062 | 3.566 | 57.41x |
| ols_summary_n10000 | 0.442 | 11.507 | 26.04x | 0.442 | 11.507 | 26.04x |
| ols_hac_n10000 | 1.636 | 12.838 | 7.85x | 1.642 | 12.838 | 7.82x |
| ols_cluster_n10000 | 0.592 | 13.198 | 22.31x | 0.594 | 13.196 | 22.23x |
| ols_summary_n100000 | 4.314 | 110.856 | 25.69x | 4.341 | 442.561 | 101.95x |
| ols_hac_n100000 | 17.120 | 120.988 | 7.07x | 17.645 | 483.193 | 27.38x |
| ols_cluster_n100000 | 7.049 | 125.338 | 17.78x | 7.376 | 499.877 | 67.77x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.656 | 4.423 | 2.67x | 1.661 | 4.423 | 2.66x |
| sparse_infrequent_10000 | 2.103 | 4.745 | 2.26x | 2.109 | 4.744 | 2.25x |
| sparse_100000 | 16.834 | 30.157 | 1.79x | 17.306 | 30.156 | 1.74x |
| sparse_infrequent_100000 | 19.168 | 25.466 | 1.33x | 19.560 | 25.462 | 1.30x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3', 'pandas': '2.3.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.226 | 19.956 | 88.30x | 0.226 | 19.956 | 88.30x |
| fe_twoway_clustered_n1000 | 0.384 | 31.332 | 81.57x | 0.384 | 31.332 | 81.56x |
| fe_kernel_n1000 | 0.245 | 20.477 | 83.54x | 0.245 | 20.477 | 83.55x |
| between_robust_n1000 | 0.128 | 19.283 | 151.01x | 0.128 | 19.281 | 151.00x |
| first_difference_robust_n1000 | 0.255 | 23.017 | 90.41x | 0.255 | 23.016 | 90.42x |
| random_effects_n1000 | 0.236 | 26.537 | 112.58x | 0.236 | 26.537 | 112.59x |
| fe_entity_n10000 | 2.748 | 29.759 | 10.83x | 2.844 | 29.757 | 10.46x |
| fe_twoway_clustered_n10000 | 4.913 | 57.226 | 11.65x | 5.218 | 114.411 | 21.93x |
| fe_kernel_n10000 | 3.001 | 30.634 | 10.21x | 3.041 | 30.634 | 10.07x |
| between_robust_n10000 | 1.547 | 28.352 | 18.33x | 1.551 | 28.349 | 18.28x |
| first_difference_robust_n10000 | 2.975 | 34.582 | 11.62x | 2.996 | 34.579 | 11.54x |
| random_effects_n10000 | 2.822 | 40.000 | 14.17x | 2.855 | 39.997 | 14.01x |
| fe_entity_n100000 | 36.356 | 186.862 | 5.14x | 38.696 | 745.500 | 19.27x |
| fe_twoway_clustered_n100000 | 51.945 | 357.455 | 6.88x | 53.963 | 1281.270 | 23.74x |
| fe_kernel_n100000 | 36.035 | 169.169 | 4.69x | 36.701 | 674.659 | 18.38x |
| between_robust_n100000 | 20.277 | 164.448 | 8.11x | 21.074 | 534.620 | 25.37x |
| first_difference_robust_n100000 | 34.477 | 195.503 | 5.67x | 36.012 | 780.232 | 21.67x |
| random_effects_n100000 | 34.537 | 232.370 | 6.73x | 36.319 | 913.733 | 25.16x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 8.342 | 86.075 | 10.32x | 8.342 | 343.992 | 41.24x |
| fit_penalised_weighted_1000 | 7.887 | 93.866 | 11.90x | 7.887 | 374.963 | 47.54x |
| predict_survival_1000 | 0.857 | 2.402 | 2.80x | 0.861 | 2.401 | 2.79x |
| fit_10000 | 231.337 | 621.438 | 2.69x | 231.648 | 997.237 | 4.30x |
| fit_penalised_weighted_10000 | 215.108 | 647.873 | 3.01x | 215.403 | 1024.724 | 4.76x |
| predict_survival_10000 | 3.467 | 5.109 | 1.47x | 3.714 | 20.380 | 5.49x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.027 | 13.956 | 518.17x | 0.027 | 13.956 | 518.20x |
| logrank_fleming_harrington_1000 | 0.036 | 21.019 | 585.20x | 0.036 | 21.018 | 585.23x |
| multigroup_1000 | 0.117 | 28.428 | 243.76x | 0.117 | 28.425 | 243.76x |
| pairwise_1000 | 0.318 | 147.291 | 463.84x | 0.318 | 147.277 | 463.84x |
| rmst_1000 | 0.094 | 1306.737 | 13872.66x | 0.094 | 1306.654 | 13872.72x |
| fixed_point_1000 | 0.038 | 15.260 | 400.53x | 0.038 | 15.259 | 400.50x |
| concordance_1000 | 0.051 | 8.543 | 167.44x | 0.051 | 8.542 | 167.43x |
| logrank_wilcoxon_weighted_10000 | 0.359 | 16.070 | 44.75x | 0.359 | 16.068 | 44.75x |
| logrank_fleming_harrington_10000 | 0.444 | 25.371 | 57.15x | 0.444 | 25.370 | 57.15x |
| multigroup_10000 | 2.504 | 41.686 | 16.65x | 2.510 | 41.684 | 16.61x |
| pairwise_10000 | 4.908 | 165.382 | 33.70x | 4.908 | 165.372 | 33.69x |
| rmst_10000 | 1.866 | 1405.631 | 753.37x | 1.906 | 1405.610 | 737.62x |
| fixed_point_10000 | 0.427 | 18.197 | 42.58x | 0.427 | 18.195 | 42.58x |
| concordance_10000 | 2.367 | 102.228 | 43.19x | 2.367 | 102.224 | 43.19x |
| logrank_wilcoxon_weighted_100000 | 4.417 | 24.512 | 5.55x | 4.546 | 24.510 | 5.39x |
| logrank_fleming_harrington_100000 | 4.353 | 39.211 | 9.01x | 4.445 | 39.210 | 8.82x |
| multigroup_100000 | 11.252 | 55.159 | 4.90x | 11.428 | 55.157 | 4.83x |
| pairwise_100000 | 46.778 | 255.607 | 5.46x | 48.264 | 255.598 | 5.30x |
| rmst_100000 | 9.270 | 515.376 | 55.60x | 9.398 | 515.290 | 54.83x |
| fixed_point_100000 | 4.930 | 32.423 | 6.58x | 5.086 | 32.420 | 6.37x |
| concordance_100000 | 30.058 | 837.320 | 27.86x | 30.303 | 837.292 | 27.63x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.198 | 74.488 | 23.29x | 3.198 | 74.484 | 23.29x |
| loglogistic_left_1000 | 5.893 | 104.253 | 17.69x | 5.893 | 104.248 | 17.69x |
| lognormal_interval_1000 | 5.392 | 192.228 | 35.65x | 5.392 | 192.228 | 35.65x |
| generalized_gamma_1000 | 45.360 | 855.230 | 18.85x | 45.444 | 855.192 | 18.82x |
| weibull_aft_1000 | 6.810 | 133.395 | 19.59x | 6.811 | 133.389 | 19.58x |
| lognormal_aft_ridge_robust_1000 | 10.804 | 4747.504 | 439.43x | 10.806 | 4747.320 | 439.31x |
| loglogistic_aft_interval_1000 | 10.522 | 210.299 | 19.99x | 10.526 | 210.284 | 19.98x |
| breslow_fleming_harrington_1000 | 0.253 | 6.947 | 27.44x | 0.253 | 6.946 | 27.43x |
| kaplan_meier_left_1000 | 0.090 | 8.202 | 91.13x | 0.090 | 8.202 | 91.13x |
| weibull_10000 | 32.078 | 89.717 | 2.80x | 32.077 | 89.715 | 2.80x |
| loglogistic_left_10000 | 65.005 | 186.123 | 2.86x | 65.010 | 186.113 | 2.86x |
| lognormal_interval_10000 | 54.504 | 344.362 | 6.32x | 54.501 | 344.346 | 6.32x |
| generalized_gamma_10000 | 415.453 | 2088.479 | 5.03x | 415.436 | 2088.404 | 5.03x |
| weibull_aft_10000 | 68.021 | 157.619 | 2.32x | 68.025 | 157.619 | 2.32x |
| lognormal_aft_ridge_robust_10000 | 139.167 | 45656.223 | 328.07x | 139.165 | 45654.197 | 328.06x |
| loglogistic_aft_interval_10000 | 97.239 | 286.019 | 2.94x | 97.245 | 286.015 | 2.94x |
| breslow_fleming_harrington_10000 | 2.576 | 8.721 | 3.39x | 2.576 | 8.721 | 3.39x |
| kaplan_meier_left_10000 | 0.963 | 10.073 | 10.46x | 0.963 | 10.073 | 10.46x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.281 | 0.468 | 1.67x | 0.282 | 0.467 | 1.66x |
| polynomial_n10000 | 0.782 | 1.028 | 1.31x | 0.786 | 1.028 | 1.31x |
| kbins_fit_n10000 | 1.456 | 2.619 | 1.80x | 1.456 | 2.619 | 1.80x |
| kbins_n10000 | 0.741 | 4.701 | 6.34x | 0.844 | 4.701 | 5.57x |
| quantile_fit_n10000 | 1.549 | 3.787 | 2.45x | 1.548 | 3.787 | 2.45x |
| quantile_n10000 | 1.815 | 1.699 | 0.94x | 1.818 | 1.699 | 0.93x |
| power_fit_n10000 | 6.184 | 137.172 | 22.18x | 6.188 | 137.166 | 22.17x |
| power_n10000 | 1.552 | 1.526 | 0.98x | 1.554 | 1.525 | 0.98x |
| knn_impute_n10000 | 7.452 | 23.931 | 3.21x | 7.452 | 95.563 | 12.82x |
| label_n10000 | 1.153 | 0.768 | 0.67x | 1.153 | 0.768 | 0.67x |
| normalize_n100000 | 1.599 | 1.857 | 1.16x | 2.021 | 1.857 | 0.92x |
| polynomial_n100000 | 3.966 | 7.090 | 1.79x | 4.881 | 7.090 | 1.45x |
| kbins_fit_n100000 | 14.447 | 8.216 | 0.57x | 15.273 | 8.215 | 0.54x |
| kbins_n100000 | 7.989 | 12.531 | 1.57x | 9.087 | 12.530 | 1.38x |
| quantile_fit_n100000 | 14.883 | 18.143 | 1.22x | 15.658 | 18.142 | 1.16x |
| quantile_n100000 | 16.875 | 14.764 | 0.87x | 17.305 | 14.763 | 0.85x |
| power_fit_n100000 | 66.840 | 918.934 | 13.75x | 69.711 | 918.866 | 13.18x |
| power_n100000 | 14.228 | 10.013 | 0.70x | 14.718 | 10.013 | 0.68x |
| knn_impute_n100000 | 7.456 | 19.038 | 2.55x | 7.455 | 75.972 | 10.19x |
| label_n100000 | 15.190 | 7.334 | 0.48x | 15.511 | 7.333 | 0.47x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.042 | 2.646 | 63.41x | 0.042 | 2.645 | 63.41x |
| var_n10000 | 0.481 | 16.447 | 34.16x | 0.485 | 16.447 | 33.94x |
| var_n100000 | 4.167 | 195.718 | 46.97x | 4.392 | 419.865 | 95.59x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BkTreeNearestBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `DecompositionWidthBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `HouseholderQrBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `PartitionValidityBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `RankingMetricsBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `VectorStoreChurnBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 2.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `StatsBenchmarks` | SampleSize=100 | `LodestarMannWhitney` | drift | 1.89 | 2.5 | -24% | 20% |
| `ScalerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Standard` | drift | 1.02 | 1.36 | -22% | 20% |

**Still away from their median: 13.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `StudentQuantile` | step | 13710.5 | 7.55 | +181496% | 30% |
| `QuantileBenchmarks` | — | `StudentQuantileCauchyFarTail` | step | 3791.93 | 2.35 | +161259% | 30% |
| `DistributionTailBenchmarks` | — | `NormalQuantile` | drift | 0.93 | 3.89 | -75% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=100000 | `MlNet_TrainTestSplit` | drift | 0.06 | 0.04 | +50% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_TrainTestSplit` | drift | 0.82 | 0.54 | +46% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_CrossValidationSplit` | drift | 4.42 | 2.89 | +44% | 20% |
| `EncoderIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=20000 | `Lodestar_Impute` | drift | 0.12 | 0.21 | -43% | 20% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 3.7 | 2.69 | +37% | 20% |
| `OsaBenchmarks` | Length=64 | `Distance_CodePoint` | step | 28.94 | 21.22 | +36% | 31% |
| `StatsBenchmarks` | SampleSize=10000 | `LodestarChiSquare` | drift | 0.002 | 0.003 | -33% | 20% |
| `EncoderIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Impute` | drift | 0.1 | 0.14 | -29% | 20% |
| `StatsBenchmarks` | SampleSize=10000 | `AccordMannWhitney` | drift | 240.074 | 331.818 | -28% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200000 | `MathNet_Wls` | drift | 2.1 | 1.735 | +20% | 20% |
