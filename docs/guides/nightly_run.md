# Nightly benchmark run

<!-- nightly-baseline: 355c69cb791e6b85f3cc0bf0a325310dbcf09f09 -->
<!-- nightly-owed: AddedTokenScanBenchmarks AgglomerativeIncumbentBenchmarks ArtifactLoadOnlyBenchmarks ArtifactTimeBenchmarks AsyncArtifactBenchmarks BatchEmbeddingBenchmarks BertNormalizerBenchmarks BkTreeBenchmarks BkTreeNearestBenchmarks BlockedTableBenchmarks Bm25Benchmarks BpeBenchmarks BpeScalingBenchmarks BpeWordCacheBenchmarks BucketRouteDiagnostics CdistCutoffBenchmarks CdistIncumbentBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CorrelationBenchmarks CosineTopKSelectionBenchmarks DbscanDimensionBenchmarks DbscanIncumbentBenchmarks DecompositionBenchmarks DecompositionWidthBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchHelpersBenchmarks FilteredVectorSearchBenchmarks FuzzBenchmarks FuzzCodePointBenchmarks FuzzIncumbentBenchmarks GlmNegativeBinomialBenchmarks HouseholderQrBenchmarks IndelBenchmarks IndelCodePointBenchmarks IndelPatternBenchmarks KMeansFitIncumbentBenchmarks KMeansLloydIncumbentBenchmarks KolmogorovDurbinBenchmarks KsAutoBenchmarks LcsGateBenchmarks LeastSquaresRoutingBenchmarks LevenshteinBenchmarks LevenshteinCodePointBenchmarks LevenshteinIncumbentBenchmarks ManyLabelsMetricsBenchmarks MetaNumericsPcaBenchmarks MetaNumericsStatsBenchmarks MetricsBenchmarks MetricsIncumbentBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MyersGateBenchmarks PartialFitBenchmarks PartialRatioLongNeedleBenchmarks PartitionValidityBenchmarks PersistenceBenchmarks PrecompiledNormalizerBenchmarks PrincipalComponentVarianceBenchmarks ProcessExtractBenchmarks RankingMetricsBenchmarks RegressionMetricsBenchmarks SentencePieceBpeLineageBenchmarks SimilaritySketchBenchmarks SmallArtifactBenchmarks StopWordBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TokenizerIncumbentBenchmarks VarianceAndFitBenchmarks VectorMathBenchmarks VectorStoreChurnBenchmarks VectorizerBenchmarks VectorizerIncumbentBenchmarks cdist persistence splitters stats transformers -->
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

- Commit: `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`
- Previous run: `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `CoxBenchmarks`
- `SurvivalBenchmarks`
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

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

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

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.597 ms** |   **0.1898 ms** |  **0.0104 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   4.517 ms |   6.2754 ms |  0.3440 ms |  1.74 |    0.11 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.501 ms |   7.8414 ms |  0.4298 ms |  2.12 |    0.14 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.096 ms** |   **0.0553 ms** |  **0.0030 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   9.019 ms |  19.0112 ms |  1.0421 ms |  0.75 |    0.07 |        - |        - |        - |    42825 B |    2,676.56 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  12.046 ms |   6.8267 ms |  0.3742 ms |  1.00 |    0.03 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |  169,441.19 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **52.745 ms** |   **4.3181 ms** |  **0.2367 ms** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |       **74 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  44.509 ms |  51.2368 ms |  2.8085 ms |  0.84 |    0.05 |        - |        - |        - |   802917 B |   10,850.23 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  61.729 ms | 234.4416 ms | 12.8505 ms |  1.17 |    0.21 | 375.0000 | 375.0000 | 375.0000 |  8072041 B |  109,081.64 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **242.787 ms** |   **1.0282 ms** |  **0.0564 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 152.457 ms | 158.7680 ms |  8.7026 ms |  0.63 |    0.03 |        - |        - |        - |   803165 B |    2,355.32 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 227.816 ms | 143.6343 ms |  7.8731 ms |  0.94 |    0.03 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |  155,047.78 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

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

| Method       | Rows  | Width | Device               | Mean          | Error          | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|---------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,809.749 ms** |  **1,374.6083 ms** |  **75.3469 ms** | **1.001** |    **0.05** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,794.193 ms |    774.1584 ms |  42.4342 ms | 0.993 |    0.04 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.572 ms |      0.2256 ms |   0.0124 ms | 0.001 |    0.00 |   318.3594 |   318.3594 |   318.3594 |   1.47 MB |        0.20 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,253.863 ms** |  **1,972.1795 ms** | **108.1018 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,202.034 ms |  1,531.7843 ms |  83.9623 ms | 0.993 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      7.079 ms |      1.5423 ms |   0.0845 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,070.006 ms** |  **2,549.1643 ms** | **139.7283 ms** | **1.000** |    **0.02** |  **2000.0000** |  **2000.0000** |  **2000.0000** |   **45.1 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,219.446 ms |  1,215.9715 ms |  66.6515 ms | 1.021 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  42.13 MB |        0.93 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      7.894 ms |      0.2953 ms |   0.0162 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |   5.86 MB |        0.13 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **30,026.843 ms** | **11,957.2736 ms** | **655.4185 ms** | **1.000** |    **0.03** | **16000.0000** | **15000.0000** | **14000.0000** | **388.67 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 29,660.793 ms |  5,346.1841 ms | 293.0424 ms | 0.988 |    0.02 | 20000.0000 | 19000.0000 | 18000.0000 | 385.01 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     19.551 ms |      2.8899 ms |   0.1584 ms | 0.001 |    0.00 |   312.5000 |   312.5000 |   312.5000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

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

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **84.34 ms** |      **5.580 ms** |     **0.306 ms** |   **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   4,982.88 ms |  2,311.549 ms |   126.704 ms |  59.08 |    1.31 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,372.52 ms |  8,830.648 ms |   484.038 ms |  63.70 |    4.97 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |      **99.48 ms** |      **1.591 ms** |     **0.087 ms** |   **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  10,388.92 ms | 11,246.132 ms |   616.438 ms | 104.43 |    5.37 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  10,252.74 ms |  3,128.734 ms |   171.496 ms | 103.06 |    1.50 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **846.53 ms** |    **113.082 ms** |     **6.198 ms** |   **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  51,158.93 ms | 23,610.747 ms | 1,294.185 ms |  60.44 |    1.38 |  3000.0000 |  3000.0000 | 2000.0000 | 159.99 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  51,940.78 ms |  9,296.297 ms |   509.561 ms |  61.36 |    0.65 |  7000.0000 |  4000.0000 | 2000.0000 | 211.57 MB |        7.11 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |     **993.07 ms** |     **57.094 ms** |     **3.129 ms** |   **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 103,214.60 ms | 33,061.379 ms | 1,812.206 ms | 103.94 |    1.61 | 12000.0000 | 10000.0000 | 8000.0000 | 536.13 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 104,095.06 ms |  5,578.882 ms |   305.797 ms | 104.82 |    0.39 | 15000.0000 | 12000.0000 | 8000.0000 | 587.56 MB |       14.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

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

| Method                          | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,453.70 ns | 249.864 ns | 13.696 ns | 1.000 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.37 ns |  12.230 ns |  0.670 ns | 0.122 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   336.75 ns |  10.130 ns |  0.555 ns | 0.137 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,541.25 ns | 180.961 ns |  9.919 ns | 3.073 |    0.02 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,646.64 ns | 146.661 ns |  8.039 ns | 2.301 |    0.01 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,197.63 ns |  23.707 ns |  1.299 ns | 0.488 |    0.00 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   774.49 ns |  11.994 ns |  0.657 ns | 0.316 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   131.17 ns |   6.555 ns |  0.359 ns | 0.053 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   134.84 ns |   2.375 ns |  0.130 ns | 0.055 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 2,811.75 ns |  43.033 ns |  2.359 ns | 1.146 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,631.71 ns |  71.587 ns |  3.924 ns | 1.073 |    0.01 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   314.99 ns |  16.890 ns |  0.926 ns | 0.128 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    40.54 ns |   2.654 ns |  0.145 ns | 0.017 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    59.88 ns |   1.117 ns |  0.061 ns | 0.024 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    71.17 ns |   0.640 ns |  0.035 ns | 0.029 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   358.11 ns |  12.703 ns |  0.696 ns | 0.146 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   465.34 ns |  13.207 ns |  0.724 ns | 0.190 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   386.36 ns |   6.675 ns |  0.366 ns | 0.157 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    24.14 ns |   3.985 ns |  0.218 ns | 0.010 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    21.93 ns |   0.683 ns |  0.037 ns | 0.009 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.83 ns |   8.526 ns |  0.467 ns | 0.027 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.95 ns |   3.410 ns |  0.187 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.53 ns |   4.425 ns |  0.243 ns | 0.012 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.84 ns |   1.314 ns |  0.072 ns | 0.073 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

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

| Method                     | Mean      | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  30.88 ns | 2.360 ns | 0.129 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  14.64 ns | 1.850 ns | 0.101 ns |  0.47 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  44.09 ns | 2.718 ns | 0.149 ns |  1.43 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  75.63 ns | 4.300 ns | 0.236 ns |  2.45 |    0.01 |         - |          NA |
| ChiSquaredSfFractionalDf   | 266.99 ns | 3.803 ns | 0.208 ns |  8.65 |    0.03 |         - |          NA |
| NormalQuantile             |  28.86 ns | 0.825 ns | 0.045 ns |  0.93 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

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

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **34.96 μs** |   **5.662 μs** | **0.310 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  55.84 μs |  15.638 μs | 0.857 μs |  1.60 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **42.16 μs** |   **3.237 μs** | **0.177 μs** |  **1.00** |    **0.01** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 101.21 μs |  13.321 μs | 0.730 μs |  2.40 |    0.02 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **333.52 μs** |  **45.795 μs** | **2.510 μs** |  **1.00** |    **0.01** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 363.03 μs |  40.799 μs | 2.236 μs |  1.09 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **403.01 μs** |  **23.685 μs** | **1.298 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 739.88 μs | 116.422 μs | 6.381 μs |  1.84 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

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
| **Poisson**             | **200**        |    **29.69 μs** |     **2.709 μs** |  **0.149 μs** |  **1.00** |    **0.01** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.29 μs |     0.571 μs |  0.031 μs |  2.03 |    0.01 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,959.39 μs** |   **489.814 μs** | **26.848 μs** |  **1.00** |    **0.01** | **328.1250** | **328.1250** | **328.1250** | **1097.74 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,542.64 μs | 1,461.022 μs | 80.084 μs |  1.91 |    0.02 | 531.2500 | 531.2500 | 531.2500 | 1878.07 KB |        1.71 |

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
| **Fit**    | **5**         | **304.6 μs** |  **9.06 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **316.1 μs** | **47.24 μs** | **2.59 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **296.5 μs** | **13.45 μs** | **0.74 μs** | **6.3477** | **0.4883** | **110.56 KB** |

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **49.78 μs** |      **2.986 μs** |     **0.164 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     108.26 μs |      7.390 μs |     0.405 μs |  2.17 |    0.01 |   1.9531 |   0.1221 |        - |   32.99 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **676.83 μs** |     **98.009 μs** |     **5.372 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   2,042.60 μs |    896.181 μs |    49.123 μs |  3.02 |    0.07 |  97.6563 |  97.6563 |  97.6563 |  370.17 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,724.33 μs** |    **345.753 μs** |    **18.952 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  22,029.87 μs |  9,946.318 μs |   545.191 μs |  3.28 |    0.07 | 281.2500 | 281.2500 | 281.2500 | 2228.14 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **52,234.47 μs** |  **5,814.739 μs** |   **318.725 μs** |  **1.00** |    **0.01** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 140,074.53 μs | 18,579.930 μs | 1,018.429 μs |  2.68 |    0.02 |        - |        - |        - | 8850.45 KB |        1.12 |

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
| **Nonrobust** | **100**        | **4**    |    **36.894 μs** |   **2.9762 μs** |  **0.1631 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.069 μs |   1.3066 μs |  0.0716 μs |  0.27 |    0.00 |   0.5035 |        - |        - |   8.45 KB |        3.19 |
| Hac       | 100        | 4    |    20.117 μs |   2.7778 μs |  0.1523 μs |  0.55 |    0.00 |   0.7629 |        - |        - |  12.63 KB |        4.77 |
| Cluster   | 100        | 4    |     9.144 μs |   0.8031 μs |  0.0440 μs |  0.25 |    0.00 |   0.5798 |        - |        - |   9.51 KB |        3.59 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **461.031 μs** |  **72.3671 μs** |  **3.9667 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   923.417 μs |  25.4276 μs |  1.3938 μs |  2.00 |    0.02 | 124.0234 | 124.0234 | 124.0234 | 472.59 KB |        5.91 |
| Hac       | 10000      | 4    | 1,970.637 μs | 285.6322 μs | 15.6565 μs |  4.27 |    0.04 | 246.0938 | 246.0938 | 246.0938 | 863.57 KB |       10.80 |
| Cluster   | 10000      | 4    |   791.072 μs |  74.3791 μs |  4.0770 μs |  1.72 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.97 KB |        7.06 |

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

| Method        | Shape    | Mean        | Error     | StdDev   | Gen0   | Allocated |
|-------------- |--------- |------------:|----------:|---------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,409.51 μs** | **87.210 μs** | **4.780 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **47.01 μs** |  **2.494 μs** | **0.137 μs** | **0.2441** |   **4.12 KB** |

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

| Method | Shape   | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **566.9 ms** | **85.47 ms** | **4.69 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **450.4 ms** | **42.01 ms** | **2.30 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

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

| Method           | Size | Mean          | Error       | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|------------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.138 μs** |   **0.3582 μs** |  **0.0196 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,575.772 μs | 164.3304 μs |  9.0075 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     20.398 μs |   5.7462 μs |  0.3150 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     92.784 μs |  24.7383 μs |  1.3560 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.581 μs** |   **5.8178 μs** |  **0.3189 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,203.936 μs | 967.6309 μs | 53.0391 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    506.312 μs | 206.5260 μs | 11.3204 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,289.726 μs |  55.3177 μs |  3.0322 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

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

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **133.8 μs** |    **17.08 μs** |  **0.94 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   926.5 μs |    97.33 μs |  5.34 μs |  6.93 |    0.05 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,303.1 μs** |   **271.53 μs** | **14.88 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,058.4 μs | 1,015.13 μs | 55.64 μs |  5.42 |    0.06 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

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

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **37.41 μs** |   **4.993 μs** |  **0.274 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   264.28 μs |  14.236 μs |  0.780 μs |  7.06 |    0.05 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    32.60 μs |   7.909 μs |  0.434 μs |  0.87 |    0.01 |   2.1667 |   0.0610 |        - |   35.43 KB |       13.38 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **492.37 μs** |  **13.320 μs** |  **0.730 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,552.38 μs | 298.807 μs | 16.379 μs |  9.25 |    0.03 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,686.39 μs | 391.464 μs | 21.457 μs |  3.43 |    0.04 | 441.4063 | 441.4063 | 441.4063 | 1765.64 KB |       22.07 |

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

| Method                       | Mean           | Error         | StdDev     | Ratio      | RatioSD  | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|-----------:|-----------:|---------:|----------:|------------:|
| NormalQuantile               |      0.0398 ns |     0.0128 ns |  0.0007 ns |       1.00 |     0.02 |         - |          NA |
| NormalQuantileFarTail        |      0.0483 ns |     0.0267 ns |  0.0015 ns |       1.21 |     0.04 |         - |          NA |
| StudentQuantile              | 14,366.3245 ns | 1,697.0741 ns | 93.0224 ns | 360,663.26 | 5,896.10 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,319.1729 ns |    64.3563 ns |  3.5276 ns |  58,222.30 |   897.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,483.3 μs** |   **172.35 μs** |   **9.45 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    416.7 μs |    14.07 μs |   0.77 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    594.0 μs |    70.43 μs |   3.86 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,260.9 μs |   120.96 μs |   6.63 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    677.4 μs |    92.97 μs |   5.10 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **622.9 μs** |    **50.38 μs** |   **2.76 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    241.8 μs |    29.74 μs |   1.63 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    521.2 μs |   168.73 μs |   9.25 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    768.1 μs |    22.17 μs |   1.22 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    381.5 μs |    21.34 μs |   1.17 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,825.3 μs** | **1,439.21 μs** |  **78.89 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,573.3 μs | 1,117.08 μs |  61.23 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,658.2 μs |   931.82 μs |  51.08 μs | 515.6250 | 515.6250 | 515.6250 |  5062698 B |
| WilcoxonPooledDifferences | 100000     | False | 16,781.2 μs |   763.67 μs |  41.86 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,043.7 μs |   303.82 μs |  16.65 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,081.4 μs** |   **199.79 μs** |  **10.95 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,793.6 μs |   115.33 μs |   6.32 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,265.3 μs | 1,316.19 μs |  72.14 μs | 500.0000 | 500.0000 | 500.0000 |  5062686 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,267.1 μs | 2,090.09 μs | 114.56 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,745.7 μs |   234.10 μs |  12.83 μs |        - |        - |        - |          - |

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

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **37.23 μs** |   **2.834 μs** |  **0.155 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.18 μs** |   **1.455 μs** |  **0.080 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.11 μs** |   **0.456 μs** |  **0.025 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.77 μs** |   **0.409 μs** |  **0.022 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.97 μs** |   **0.542 μs** |  **0.030 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **456.01 μs** |  **30.294 μs** |  **1.661 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **934.36 μs** | **237.901 μs** | **13.040 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **908.45 μs** | **103.429 μs** |  **5.669 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,046.26 μs** |  **47.154 μs** |  **2.585 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,073.61 μs** | **261.144 μs** | **14.314 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |

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

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.455 μs** | **0.3933 μs** | **0.0216 μs** |  **1.00** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.537 μs | 0.4374 μs | 0.0240 μs |  2.59 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.967 μs | 0.1925 μs | 0.0106 μs |  1.11 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.152 μs | 0.2618 μs | 0.0143 μs |  2.73 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.063 μs | 0.3824 μs | 0.0210 μs |  1.14 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.663 μs | 0.3036 μs | 0.0166 μs |  2.62 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **49.030 μs** | **0.8364 μs** | **0.0458 μs** |  **1.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 121.116 μs | 7.2444 μs | 0.3971 μs |  2.47 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.408 μs | 1.2843 μs | 0.0704 μs |  1.01 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.501 μs | 3.0305 μs | 0.1661 μs |  2.48 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.828 μs | 5.1947 μs | 0.2847 μs |  1.02 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 120.957 μs | 1.3980 μs | 0.0766 μs |  2.47 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

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

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.293 μs** |  **1.2283 μs** | **0.0673 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  28.407 μs |  1.4970 μs | 0.0821 μs |  3.90 |    0.03 |   4.0588 |   0.5493 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |  26.305 μs |  2.1048 μs | 0.1154 μs |  3.61 |    0.03 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.924 μs |  0.3079 μs | 0.0169 μs |  0.54 |    0.00 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.001 μs |  0.7179 μs | 0.0393 μs |  0.55 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.500 μs |  0.3827 μs | 0.0210 μs |  0.21 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.765 μs |  0.3444 μs | 0.0189 μs |  0.38 |    0.00 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **109.577 μs** | **20.1458 μs** | **1.1043 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 917.254 μs | 88.7950 μs | 4.8672 μs |  8.37 |    0.08 | 249.0234 | 249.0234 | 249.0234 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 282.597 μs | 18.0178 μs | 0.9876 μs |  2.58 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  96.291 μs | 12.3345 μs | 0.6761 μs |  0.88 |    0.01 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.039 μs |  3.3435 μs | 0.1833 μs |  0.89 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  13.674 μs |  0.1389 μs | 0.0076 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  27.350 μs |  3.7269 μs | 0.2043 μs |  0.25 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

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

| Method              | SampleSize | Mean            | Error           | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,155.2 ns** |       **734.17 ns** |      **40.24 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,272.5 ns |     2,930.90 ns |     160.65 ns |    6.22 |    0.04 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,197.8 ns |       431.62 ns |      23.66 ns |    0.36 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,125.3 ns |     2,173.00 ns |     119.11 ns |    3.76 |    0.03 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        111.6 ns |         7.82 ns |       0.43 ns |    0.02 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        218.7 ns |        35.15 ns |       1.93 ns |    0.04 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |                 |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **53,653.5 ns** |    **11,232.11 ns** |     **615.67 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    140,856.1 ns |    11,617.25 ns |     636.78 ns |   2.626 |    0.03 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    686,785.2 ns |    23,711.77 ns |   1,299.72 ns |  12.801 |    0.13 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,516,450.0 ns | 3,452,976.98 ns | 189,269.32 ns | 233.303 |    3.83 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        110.8 ns |         3.48 ns |       0.19 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        214.4 ns |        20.14 ns |       1.10 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **15.90 μs** |   **4.214 μs** | **0.231 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **37.55 μs** |   **5.097 μs** | **0.279 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **53.42 μs** |   **7.442 μs** | **0.408 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **153.60 μs** |   **3.767 μs** | **0.206 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **212.44 μs** |  **16.248 μs** | **0.891 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **560.73 μs** |  **62.678 μs** | **3.436 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **708.62 μs** | **117.986 μs** | **6.467 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,246.72 μs** |  **16.537 μs** | **0.906 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

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

| Method       | SampleSize | Mean         | Error         | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **46.17 μs** |      **1.149 μs** |   **0.063 μs** |  **1.00** |    **0.00** |    **0.1831** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     19.36 μs |      7.640 μs |   0.419 μs |  0.42 |    0.01 |    3.6926 |    0.1831 |         - |    58.23 KB |       16.98 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **139.05 μs** |     **10.989 μs** |   **0.602 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    115.62 μs |     19.244 μs |   1.055 μs |  0.83 |    0.01 |   33.2031 |   14.1602 |         - |   508.31 KB |       29.06 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **966.96 μs** |     **56.173 μs** |   **3.079 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |    **158.5 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,566.69 μs |  2,800.961 μs | 153.530 μs |  1.62 |    0.14 | 1023.4375 | 1021.4844 | 1021.4844 |  5027.18 KB |       31.72 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,344.06 μs** |  **2,037.593 μs** | **111.687 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 18,177.38 μs | 16,903.446 μs | 926.535 μs |  1.95 |    0.09 |  937.5000 |  937.5000 |  937.5000 | 50024.07 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

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

| Method | SampleSize | Covariates | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **434.3 μs** |  **85.47 μs** |  **4.68 μs** |   **8.3008** |   **0.9766** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **1,910.0 μs** |  **78.47 μs** |  **4.30 μs** |  **11.7188** |   **1.9531** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,202.5 μs** | **305.29 μs** | **16.73 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **16,839.8 μs** | **900.33 μs** | **49.35 μs** | **281.2500** | **250.0000** | **250.0000** | **1737.22 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

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

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **15.85 μs** |     **3.018 μs** |  **0.165 μs** |  **1.00** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     14.38 μs |     1.662 μs |  0.091 μs |  0.91 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.65 μs |     1.593 μs |  0.087 μs |  1.37 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **58.33 μs** |     **8.699 μs** |  **0.477 μs** |  **1.00** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.41 μs |     3.966 μs |  0.217 μs |  0.40 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     48.01 μs |    12.028 μs |  0.659 μs |  0.82 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **8,579.72 μs** |   **462.383 μs** | **25.345 μs** |  **1.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  8,623.41 μs |   278.457 μs | 15.263 μs |  1.01 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  9,871.89 μs |   337.663 μs | 18.508 μs |  1.15 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **15,611.86 μs** | **1,625.512 μs** | **89.100 μs** |  **1.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 12,501.89 μs | 1,161.080 μs | 63.643 μs |  0.80 |  828.1250 |  812.5000 | 671.8750 | 5660.21 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 16,221.25 μs |   818.817 μs | 44.882 μs |  1.04 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

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

| Method                         | Samples | Clusters | Mean       | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.121 ms** |   **0.2661 ms** | **0.0146 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.146 ms |   0.1402 ms | 0.0077 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  20.579 ms |   0.9507 ms | 0.0521 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.024 ms** |   **0.1044 ms** | **0.0057 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.177 ms |   0.3044 ms | 0.0167 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 182.359 ms | 113.2885 ms | 6.2097 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

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
| **Lodestar**            | **12**     |        **887.1 ns** |        **47.03 ns** |       **2.58 ns** |  **1.00** |    **0.00** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,805.4 ns |        41.47 ns |       2.27 ns |  2.04 |    0.01 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **70,168.4 ns** |    **20,278.99 ns** |   **1,111.56 ns** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,754.2 ns |    10,612.12 ns |     581.69 ns |  2.16 |    0.03 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,347,733.0 ns** |   **700,199.14 ns** |  **38,380.28 ns** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,480,160.8 ns | 2,113,560.89 ns | 115,851.40 ns |  2.87 |    0.03 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

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

| Method      | Count  | Mean       | Error       | StdDev    | Allocated |
|------------ |------- |-----------:|------------:|----------:|----------:|
| **SearchTop10** | **10000**  |   **538.9 μs** |    **35.55 μs** |   **1.95 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,811.1 μs** | **3,703.48 μs** | **203.00 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-HUYUUP : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-HUYUUP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **284.15 μs** |    **15.721 μs** |    **17.474 μs** |    **281.63 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    265.74 μs |    10.322 μs |    11.045 μs |    266.85 μs |  0.94 |    0.07 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |     88.41 μs |     4.532 μs |     5.038 μs |     87.14 μs |  0.31 |    0.02 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    432.11 μs |    22.760 μs |    25.298 μs |    431.28 μs |  1.53 |    0.12 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,669.49 μs |    79.465 μs |    88.326 μs |  1,705.99 μs |  5.90 |    0.46 |        - |        - |        - |  331.74 KB |        1.99 |
| MlNet_ReplaceMissingValues_Read | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,185.70 μs |    75.661 μs |    84.097 μs |  1,225.86 μs |  4.19 |    0.38 |        - |        - |        - |  296.71 KB |        1.78 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    183.91 μs |    17.062 μs |     0.935 μs |    183.69 μs |  1.00 |    0.01 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     69.07 μs |     1.995 μs |     0.109 μs |     69.11 μs |  0.38 |    0.00 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.86 μs |     1.354 μs |     0.074 μs |     16.85 μs |  0.09 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,016.00 μs | 2,584.235 μs |   141.651 μs |  1,014.21 μs |  5.52 |    0.67 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    908.54 μs |   616.504 μs |    33.793 μs |    905.98 μs |  4.94 |    0.16 |  48.8281 |   7.8125 |        - |  794.94 KB |        4.79 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    265.42 μs |   230.296 μs |    12.623 μs |    260.94 μs |  1.44 |    0.06 |  19.0430 |  18.5547 |   0.4883 |  296.91 KB |        1.79 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-HUYUUP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,377.87 μs** |   **677.035 μs** |   **752.523 μs** |  **5,981.91 μs** |  **1.02** |    **0.20** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,912.59 μs |   593.903 μs |   635.469 μs |  4,838.23 μs |  0.93 |    0.18 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |    653.39 μs |    10.628 μs |    11.813 μs |    654.33 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,488.84 μs |    46.848 μs |    46.011 μs |  4,498.43 μs |  0.85 |    0.12 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,116.90 μs | 6,066.463 μs | 6,986.148 μs | 12,422.02 μs |  2.49 |    1.35 |        - |        - |        - |   550.2 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,233.03 μs | 3,791.813 μs | 4,214.590 μs | 12,747.30 μs |  1.94 |    0.83 |        - |        - |        - |  530.17 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,839.34 μs | 5,459.734 μs |   299.266 μs |  4,009.32 μs |  1.00 |    0.10 | 992.1875 | 992.1875 | 992.1875 | 3291.63 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,153.64 μs |   394.538 μs |    21.626 μs |  3,151.15 μs |  0.82 |    0.06 |  89.8438 |  89.8438 |  89.8438 |  314.47 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    435.44 μs |   109.701 μs |     6.013 μs |    436.35 μs |  0.11 |    0.01 | 129.8828 | 129.8828 | 129.8828 |  626.25 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,560.18 μs | 1,158.821 μs |    63.519 μs |  1,560.15 μs |  0.41 |    0.03 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,139.60 μs | 1,502.406 μs |    82.352 μs |  4,172.54 μs |  1.08 |    0.08 |  31.2500 |  15.6250 |        - |  566.92 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,494.01 μs |   752.255 μs |    41.234 μs |  2,493.37 μs |  0.65 |    0.05 |  27.3438 |  11.7188 |        - |  455.47 KB |        0.14 |

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
| **Distance_Utf16**     | **8**      |     **39.99 ns** |      **6.806 ns** |   **0.373 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    129.06 ns |      5.917 ns |   0.324 ns |  3.23 |    0.03 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **153.55 ns** |    **102.598 ns** |   **5.624 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,581.17 ns |    497.790 ns |  27.286 ns | 10.31 |    0.37 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **321.77 ns** |     **26.843 ns** |   **1.471 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,140.55 ns |  4,371.640 ns | 239.624 ns | 28.41 |    0.65 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **49,636.13 ns** | **10,616.680 ns** | **581.936 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 47,355.77 ns |  8,285.972 ns | 454.182 ns |  0.95 |    0.01 |         - |          NA |

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
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.848 μs** | **1.5587 μs** | **0.0854 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **3.955 μs** | **0.8218 μs** | **0.0450 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.730 μs** | **0.6654 μs** | **0.0365 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.923 μs** | **2.3293 μs** | **0.1277 μs** | **0.0076** |     **192 B** |

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

| Method                   | Length | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **377.0 ns** |     **59.64 ns** |     **3.27 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   7,044.4 ns |    231.30 ns |    12.68 ns | 18.69 |    0.14 |         - |          NA |
|                          |        |              |              |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,816.4 ns** |    **457.77 ns** |    **25.09 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 734,559.7 ns | 30,171.34 ns | 1,653.79 ns | 32.19 |    0.07 |         - |          NA |

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
| **Fit**    | **2000x2000x20** | **24.03 ms** | **0.999 ms** | **0.055 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.63 ms** | **0.818 ms** | **0.045 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-YQPELG : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-YQPELG** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **111.446 μs** |     **2.9062 μs** |     **2.8543 μs** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    103.921 μs |     3.4168 μs |     3.5088 μs |  0.93 |    0.04 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,280.166 μs |    26.2607 μs |    30.2419 μs | 20.47 |    0.55 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    107.524 μs |    20.0442 μs |    22.2791 μs |  0.97 |    0.20 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     65.267 μs |    11.6199 μs |    11.4123 μs |  0.59 |    0.10 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     68.102 μs |    10.3284 μs |    11.0513 μs |  0.61 |    0.10 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     95.959 μs |     7.3500 μs |     7.2187 μs |  0.86 |    0.07 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    422.466 μs |    14.8751 μs |    17.1302 μs |  3.79 |    0.17 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,301.150 μs |    30.6763 μs |    34.0967 μs | 20.66 |    0.57 |        - |        - |        - |  296984 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,691.950 μs |   591.3584 μs |   607.2816 μs | 60.08 |    5.48 |        - |        - |        - |  442056 B |        5.42 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.261 μs |     9.3254 μs |     0.5112 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     40.093 μs |     8.3229 μs |     0.4562 μs |  0.87 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    462.704 μs |     1.5234 μs |     0.0835 μs | 10.00 |    0.10 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.062 μs |     3.2054 μs |     0.1757 μs |  1.02 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.476 μs |     0.3585 μs |     0.0196 μs |  0.23 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.716 μs |     0.2318 μs |     0.0127 μs |  0.17 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.672 μs |     4.7630 μs |     0.2611 μs |  0.25 |    0.01 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,587.073 μs | 4,502.7186 μs |   246.8092 μs | 34.31 |    4.63 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    976.988 μs |   709.1914 μs |    38.8732 μs | 21.12 |    0.76 |  39.0625 |   1.9531 |        - |  650629 B |       8.043 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,607.779 μs | 6,511.7540 μs |   356.9312 μs | 34.76 |    6.69 |  31.2500 |        - |        - |  518032 B |       6.404 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-YQPELG** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,528.851 μs** |    **15.8446 μs** |    **17.6112 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,421.385 μs |    14.1548 μs |    16.3007 μs |  0.93 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,125.625 μs |    67.5472 μs |    72.2747 μs |  9.89 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,506.561 μs |    26.8002 μs |    29.7883 μs |  0.99 |    0.02 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    420.893 μs |     7.9266 μs |     9.1283 μs |  0.28 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    314.956 μs |     9.0975 μs |    10.1119 μs |  0.21 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    404.635 μs |     9.3065 μs |     9.9578 μs |  0.26 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,144.045 μs | 1,721.1343 μs | 1,767.4782 μs |  4.02 |    1.12 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 18,883.603 μs | 7,510.6894 μs | 8,649.3211 μs | 12.35 |    5.52 |        - |        - |        - |  478864 B |       0.299 |
| MlNet_NormalizeRobustScaling_Read | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,129.860 μs |   189.6923 μs |   210.8425 μs | 12.51 |    0.19 |        - |        - |        - | 4278440 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,474.616 μs | 1,460.6582 μs |    80.0636 μs |  1.00 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1601359 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,336.048 μs |   851.0157 μs |    46.6470 μs |  0.91 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,715.596 μs | 4,078.3906 μs |   223.5503 μs | 10.00 |    0.48 | 890.6250 | 890.6250 | 890.6250 | 3201205 B |       1.999 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,389.733 μs | 2,088.4296 μs |   114.4739 μs |  0.94 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601039 B |       1.000 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    306.143 μs |     6.8890 μs |     0.3776 μs |  0.21 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    242.412 μs |    13.0954 μs |     0.7178 μs |  0.16 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    408.863 μs |   179.1534 μs |     9.8200 μs |  0.28 |    0.01 | 128.9063 | 124.0234 | 124.0234 |  560075 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,634.206 μs | 2,321.0749 μs |   127.2259 μs |  1.11 |    0.09 |  41.0156 |   6.8359 |        - |  702655 B |       0.439 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,398.803 μs | 2,190.2800 μs |   120.0566 μs |  2.99 |    0.15 |  31.2500 |        - |        - |  595734 B |       0.372 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,439.293 μs | 1,521.7828 μs |    83.4141 μs | 13.21 |    0.61 | 250.0000 | 125.0000 |        - | 4300870 B |       2.686 |

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
| **PerSample** | **2000**    |  **27.29 ms** |  **1.123 ms** | **0.062 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **192.73 ms** | **13.304 ms** | **0.729 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

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

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **97.628 μs** |    **89.136 μs** |   **4.8859 μs** |  **1.00** |    **0.06** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    178.919 μs |     7.041 μs |   0.3859 μs |  1.84 |    0.08 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.667 μs |     3.270 μs |   0.1793 μs |  0.05 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    335.940 μs |    71.484 μs |   3.9183 μs |  3.45 |    0.15 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,662.503 μs |   187.354 μs |  10.2695 μs | 58.10 |    2.47 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     61.443 μs |     6.318 μs |   0.3463 μs |  0.63 |    0.03 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,108.965 μs |   132.231 μs |   7.2480 μs | 11.38 |    0.49 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,072.033 μs** |   **473.182 μs** |  **25.9367 μs** |  **1.00** |    **0.03** | **484.3750** | **470.7031** | **460.9375** | **2738.75 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,328.969 μs |   844.175 μs |  46.2721 μs |  2.17 |    0.06 | 550.7813 | 535.1563 | 527.3438 | 3130.11 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     90.238 μs |    48.474 μs |   2.6570 μs |  0.08 |    0.00 |  59.2041 |  59.2041 |  59.2041 |   391.2 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    328.195 μs |   102.635 μs |   5.6258 μs |  0.31 |    0.01 |   2.4414 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 44,246.311 μs | 4,766.400 μs | 261.2624 μs | 41.29 |    0.88 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     62.340 μs |    24.561 μs |   1.3463 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,493.136 μs |   370.865 μs |  20.3284 μs |  6.99 |    0.15 |        - |        - |        - |   34.19 KB |       0.012 |

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

| Method  | Words | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **641.1 μs** |  **48.26 μs** |  **2.65 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,793.7 μs** | **107.12 μs** |  **5.87 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,874.0 μs** | **587.89 μs** | **32.22 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

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

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.48 ms** | **0.636 ms** | **0.035 ms** |         **-** |
| **TopTwo** | **100**     | **87.62 ms** | **2.983 ms** | **0.164 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-DJSVZW : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-DJSVZW** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **53.17 μs** |      **0.558 μs** |      **0.548 μs** |     **53.08 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |     88.09 μs |      1.335 μs |      1.484 μs |     87.89 μs |   1.66 |    0.03 |        - |        - |        - |  117.88 KB |        3.68 |
| Lodestar_Discretize         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    199.70 μs |     36.073 μs |     41.541 μs |    174.03 μs |   3.76 |    0.76 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    291.74 μs |     16.577 μs |     18.425 μs |    287.68 μs |   5.49 |    0.34 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    998.74 μs |     11.254 μs |     12.042 μs |    996.60 μs |  18.79 |    0.29 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,556.34 μs |     10.271 μs |     11.416 μs |  1,556.21 μs |  29.27 |    0.36 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,104.14 μs |     13.448 μs |     13.810 μs |  1,105.48 μs |  20.77 |    0.33 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    223.82 μs |     16.777 μs |     19.321 μs |    213.26 μs |   4.21 |    0.36 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,853.17 μs |     12.400 μs |     13.782 μs |  1,850.05 μs |  34.86 |    0.43 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     | 13,642.58 μs |  5,535.863 μs |  6,375.108 μs |  9,124.48 μs | 256.62 |  117.09 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,183.77 μs |     77.943 μs |     86.633 μs |  1,223.56 μs |  22.27 |    1.60 |        - |        - |        - |   330.8 KB |       10.34 |
| MlNet_NormalizeBinning_Read | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,046.18 μs |    136.106 μs |    151.282 μs |  3,064.11 μs |  57.30 |    2.83 |        - |        - |        - |  349.24 KB |       10.92 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.98 μs |      2.127 μs |      0.117 μs |     13.99 μs |   1.00 |    0.01 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.15 μs |     14.235 μs |      0.780 μs |     83.02 μs |   5.95 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    133.15 μs |     22.039 μs |      1.208 μs |    133.49 μs |   9.52 |    0.10 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     74.74 μs |      7.919 μs |      0.434 μs |     74.81 μs |   5.35 |    0.05 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     57.65 μs |     54.304 μs |      2.977 μs |     56.61 μs |   4.12 |    0.19 |   1.9531 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    463.12 μs |     21.876 μs |      1.199 μs |    462.72 μs |  33.13 |    0.25 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    169.54 μs |    164.748 μs |      9.030 μs |    172.15 μs |  12.13 |    0.57 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    141.05 μs |      0.977 μs |      0.054 μs |    141.07 μs |  10.09 |    0.07 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    697.15 μs |     91.963 μs |      5.041 μs |    699.48 μs |  49.87 |    0.48 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,761.50 μs |  1,032.969 μs |     56.620 μs |  8,731.03 μs | 626.71 |    5.73 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    259.57 μs |    135.944 μs |      7.452 μs |    255.97 μs |  18.57 |    0.48 |  19.0430 |  18.5547 |   0.4883 |  297.75 KB |        9.52 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    975.70 μs |  1,059.195 μs |     58.058 μs |    970.04 μs |  69.79 |    3.63 |  42.9688 |   5.8594 |        - |  704.25 KB |       22.52 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-DJSVZW** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **460.38 μs** |     **10.221 μs** |     **10.039 μs** |    **456.05 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,637.32 μs |     19.378 μs |     21.539 μs |  1,639.15 μs |   3.56 |    0.09 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,518.50 μs |     23.095 μs |     25.670 μs |  3,518.15 μs |   7.65 |    0.17 |        - |        - |        - | 3125.46 KB |        4.99 |
| Lodestar_Discretize_64Bins  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,448.37 μs |     22.778 μs |     26.231 μs |  4,441.06 μs |   9.67 |    0.21 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,300.77 μs |  4,962.750 μs |  5,715.111 μs | 11,623.40 μs |  24.56 |   12.14 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,483.28 μs |     47.667 μs |     48.951 μs | 10,486.74 μs |  22.78 |    0.48 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,343.89 μs |  5,050.807 μs |  5,816.517 μs |  5,792.13 μs |  20.30 |   12.35 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,007.81 μs |     10.101 μs |     11.227 μs |  3,006.14 μs |   6.54 |    0.14 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,546.62 μs |  3,790.332 μs |  4,364.952 μs | 14,497.01 μs |  38.13 |    9.29 |        - |        - |        - | 2658.55 KB |        4.25 |
| Lodestar_KnnImpute          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,699.08 μs |     34.123 μs |     36.512 μs |  9,692.68 μs |  21.08 |    0.44 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,505.08 μs |  4,013.913 μs |  4,461.454 μs |  8,377.19 μs |  20.66 |    9.46 |        - |        - |        - |  531.29 KB |        0.85 |
| MlNet_NormalizeBinning_Read | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 29,885.86 μs | 12,192.994 μs | 14,041.470 μs | 27,914.40 μs |  64.94 |   29.82 |        - |        - |        - |  2078.6 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    660.87 μs |     26.346 μs |      1.444 μs |    660.38 μs |   1.00 |    0.00 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    632.35 μs |    154.724 μs |      8.481 μs |    630.36 μs |   0.96 |    0.01 | 267.5781 | 267.5781 | 267.5781 | 2346.23 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,135.85 μs |    789.878 μs |     43.296 μs |  3,111.55 μs |   4.75 |    0.06 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,870.37 μs |    286.532 μs |     15.706 μs |  1,877.23 μs |   2.83 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,743.64 μs |    236.081 μs |     12.940 μs |  5,745.10 μs |   8.69 |    0.02 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,939.29 μs |    771.736 μs |     42.301 μs | 10,924.17 μs |  16.55 |    0.06 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,843.30 μs |    165.864 μs |      9.092 μs |  5,838.15 μs |   8.84 |    0.02 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,208.44 μs |    313.471 μs |     17.182 μs |  3,213.44 μs |   4.85 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,186.89 μs |  2,178.968 μs |    119.437 μs | 13,252.71 μs |  19.95 |    0.16 | 734.3750 | 734.3750 | 734.3750 |  2660.9 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,145.60 μs |    361.606 μs |     19.821 μs |  9,146.97 μs |  13.84 |    0.04 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,419.30 μs |    199.420 μs |     10.931 μs |  2,425.56 μs |   3.66 |    0.02 |  27.3438 |  11.7188 |        - |  458.61 KB |        0.73 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,679.33 μs |  1,003.870 μs |     55.025 μs | 10,671.35 μs |  16.16 |    0.08 | 265.6250 | 218.7500 | 187.5000 |  2124.3 KB |        3.40 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

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
- `levenshtein`
- `indel`
- `metrics`

### compare-cluster-weighted

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.256 | 3.968 | 0.75x | 5.262 | 7.928 | 1.51x |
| kmeans_restarts_10000 | 21.611 | 10.361 | 0.48x | 21.619 | 20.660 | 0.96x |
| kmeans_weighted_100000 | 51.951 | 28.135 | 0.54x | 52.411 | 47.602 | 0.91x |
| kmeans_restarts_100000 | 208.767 | 71.292 | 0.34x | 209.810 | 131.818 | 0.63x |
| dbscan_weighted_5000 | 8.025 | 31.953 | 3.98x | 8.522 | 31.952 | 3.75x |
| dbscan_weighted_20000 | 98.953 | 191.543 | 1.94x | 100.411 | 191.524 | 1.91x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.826 | 127.483 | 154.32x | 0.826 | 127.471 | 154.33x |
| ridge_robust_1000 | 0.899 | 114.767 | 127.61x | 0.899 | 114.759 | 127.60x |
| lasso_1000 | 6.315 | 1079.401 | 170.94x | 6.314 | 1079.329 | 170.93x |
| ph_test_1000 | 0.130 | 20.998 | 160.98x | 0.130 | 20.996 | 160.96x |
| predict_survival_1000 | 0.032 | 1.886 | 59.82x | 0.032 | 1.886 | 59.82x |
| time_varying_1000 | 68.208 | 346.485 | 5.08x | 68.211 | 346.466 | 5.08x |
| stratified_weighted_10000 | 8.689 | 712.919 | 82.05x | 8.733 | 712.853 | 81.63x |
| ridge_robust_10000 | 9.888 | 1097.151 | 110.96x | 10.161 | 1097.069 | 107.96x |
| lasso_10000 | 56.515 | 7094.977 | 125.54x | 56.560 | 7094.562 | 125.43x |
| ph_test_10000 | 2.022 | 206.239 | 102.00x | 2.102 | 206.226 | 98.13x |
| predict_survival_10000 | 0.034 | 1.902 | 56.71x | 0.034 | 1.902 | 56.71x |
| time_varying_10000 | 2494.502 | 5175.332 | 2.07x | 2495.869 | 5174.936 | 2.07x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 3.925 | 48.182 | 12.28x | 3.925 | 190.042 | 48.42x |
| cv_minmax_n1000 | 3.031 | 5.516 | 1.82x | 3.030 | 22.007 | 7.26x |
| cv_plus_gamma_n1000 | 4.529 | 48.670 | 10.75x | 4.529 | 192.288 | 42.46x |
| bootstrap_plus_n1000 | 14.552 | 49.046 | 3.37x | 14.551 | 195.114 | 13.41x |
| cv_plus_n10000 | 40.260 | 122.372 | 3.04x | 40.260 | 460.598 | 11.44x |
| cv_minmax_n10000 | 30.536 | 25.461 | 0.83x | 30.534 | 101.588 | 3.33x |
| cv_plus_gamma_n10000 | 58.127 | 127.680 | 2.20x | 58.124 | 470.508 | 8.09x |
| bootstrap_plus_n10000 | 146.682 | 125.022 | 0.85x | 146.673 | 474.391 | 3.23x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.387 | 3.416 | 8.82x | 0.387 | 3.416 | 8.82x |
| glm_gamma_n1000 | 0.469 | 4.233 | 9.02x | 0.469 | 4.233 | 9.02x |
| glm_poisson_exposure_n1000 | 0.395 | 4.003 | 10.13x | 0.395 | 4.002 | 10.13x |
| mnlogit_n1000 | 0.823 | 17.950 | 21.82x | 0.822 | 17.948 | 21.82x |
| glm_negative_binomial_n10000 | 3.491 | 14.034 | 4.02x | 3.499 | 13.972 | 3.99x |
| glm_gamma_n10000 | 4.036 | 13.012 | 3.22x | 4.042 | 13.010 | 3.22x |
| glm_poisson_exposure_n10000 | 4.256 | 12.893 | 3.03x | 4.263 | 12.891 | 3.02x |
| mnlogit_n10000 | 8.293 | 94.951 | 11.45x | 8.299 | 94.945 | 11.44x |
| glm_negative_binomial_n100000 | 36.053 | 150.252 | 4.17x | 36.508 | 587.016 | 16.08x |
| glm_gamma_n100000 | 41.974 | 169.623 | 4.04x | 42.319 | 677.136 | 16.00x |
| glm_poisson_exposure_n100000 | 55.613 | 127.840 | 2.30x | 55.514 | 510.038 | 9.19x |
| mnlogit_n100000 | 82.749 | 909.286 | 10.99x | 82.956 | 1593.999 | 19.21x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 121.2 | 27.5 | 4.41x C# faster |
| latin | 32 | 178.1 | 89.1 | 2.00x C# faster |
| latin | 128 | 447.7 | 398.0 | 1.12x C# faster |
| latin | 512 | 4467.0 | 4894.7 | 1.10x Py faster |
| cjk | 8 | 148.7 | 25.3 | 5.87x C# faster |
| cjk | 32 | 338.9 | 226.6 | 1.50x C# faster |
| cjk | 128 | 1836.2 | 1362.1 | 1.35x C# faster |
| cjk | 512 | 14840.9 | 8780.7 | 1.69x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.640 | 55.381 | 33.77x | 1.640 | 55.377 | 33.77x |
| liml_robust_n1000 | 1.777 | 55.528 | 31.25x | 1.777 | 55.523 | 31.25x |
| gmm_robust_n1000 | 1.650 | 54.584 | 33.09x | 1.650 | 54.578 | 33.09x |
| 2sls_kernel_n1000 | 8.455 | 58.341 | 6.90x | 8.543 | 58.338 | 6.83x |
| 2sls_clustered_n1000 | 1.587 | 56.883 | 35.85x | 1.587 | 56.882 | 35.85x |
| 2sls_robust_n10000 | 16.691 | 238.368 | 14.28x | 17.140 | 476.601 | 27.81x |
| liml_robust_n10000 | 18.327 | 237.957 | 12.98x | 18.935 | 475.089 | 25.09x |
| gmm_robust_n10000 | 16.515 | 230.760 | 13.97x | 17.012 | 461.487 | 27.13x |
| 2sls_kernel_n10000 | 90.556 | 258.650 | 2.86x | 91.038 | 517.136 | 5.68x |
| 2sls_clustered_n10000 | 15.805 | 252.968 | 16.01x | 16.175 | 505.956 | 31.28x |
| 2sls_robust_n100000 | 183.886 | 2492.252 | 13.55x | 185.886 | 7719.681 | 41.53x |
| liml_robust_n100000 | 192.347 | 2554.132 | 13.28x | 194.585 | 7893.237 | 40.56x |
| gmm_robust_n100000 | 186.990 | 2435.310 | 13.02x | 191.467 | 7399.713 | 38.65x |
| 2sls_kernel_n100000 | 3357.937 | 3225.712 | 0.96x | 3362.642 | 10128.391 | 3.01x |
| 2sls_clustered_n100000 | 174.285 | 2678.161 | 15.37x | 175.883 | 8622.224 | 49.02x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 151.4 | 17.9 | 8.46x C# faster |
| latin | 32 | 282.2 | 157.6 | 1.79x C# faster |
| latin | 128 | 1649.0 | 897.1 | 1.84x C# faster |
| latin | 512 | 14132.9 | 11699.9 | 1.21x C# faster |
| cjk | 8 | 160.6 | 17.8 | 9.02x C# faster |
| cjk | 32 | 385.5 | 292.7 | 1.32x C# faster |
| cjk | 128 | 2773.8 | 2012.5 | 1.38x C# faster |
| cjk | 512 | 23932.1 | 17760.9 | 1.35x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 1.023 | 235.98x | 0.004 | 1.023 | 235.98x |
| accuracy_n1000_k2 | 0.000 | 0.555 | 4183.56x | 0.000 | 0.555 | 4183.63x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.871 | 376.10x | 0.005 | 1.871 | 376.09x |
| classification_report_n1000_k2 | 0.007 | 7.270 | 1003.40x | 0.007 | 7.269 | 1003.36x |
| roc_auc_binary_n1000_k2 | 0.018 | 2.006 | 113.77x | 0.018 | 2.006 | 113.77x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.100 | 231.40x | 0.005 | 1.100 | 231.40x |
| matthews_n1000_k2 | 0.004 | 2.105 | 469.64x | 0.004 | 2.105 | 469.63x |
| cohen_kappa_n1000_k2 | 0.004 | 1.136 | 257.65x | 0.004 | 1.136 | 257.63x |
| mse_n1000_k2 | 0.000 | 0.323 | 661.62x | 0.000 | 0.323 | 661.61x |
| mae_n1000_k2 | 0.000 | 0.320 | 660.34x | 0.000 | 0.320 | 660.33x |
| median_ae_n1000_k2 | 0.005 | 0.331 | 68.93x | 0.005 | 0.331 | 68.93x |
| r2_n1000_k2 | 0.001 | 0.383 | 323.95x | 0.001 | 0.383 | 323.94x |
| confusion_matrix_n1000_k10 | 0.005 | 1.030 | 222.75x | 0.005 | 1.030 | 222.74x |
| accuracy_n1000_k10 | 0.000 | 0.557 | 4179.50x | 0.000 | 0.557 | 4179.74x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.887 | 321.56x | 0.006 | 1.887 | 321.53x |
| classification_report_n1000_k10 | 0.012 | 7.341 | 628.74x | 0.012 | 7.340 | 628.74x |
| roc_auc_ovr_macro_n1000_k10 | 0.524 | 10.178 | 19.43x | 0.524 | 10.177 | 19.43x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.094 | 191.75x | 0.006 | 1.094 | 191.77x |
| matthews_n1000_k10 | 0.006 | 2.100 | 373.21x | 0.006 | 2.100 | 373.21x |
| cohen_kappa_n1000_k10 | 0.006 | 1.134 | 188.55x | 0.006 | 1.134 | 188.55x |
| mse_n1000_k10 | 0.000 | 0.319 | 648.75x | 0.000 | 0.319 | 648.73x |
| mae_n1000_k10 | 0.000 | 0.318 | 656.28x | 0.000 | 0.318 | 656.23x |
| median_ae_n1000_k10 | 0.005 | 0.330 | 69.38x | 0.005 | 0.330 | 69.39x |
| r2_n1000_k10 | 0.001 | 0.385 | 324.20x | 0.001 | 0.385 | 324.20x |
| confusion_matrix_n100000_k2 | 0.460 | 10.783 | 23.45x | 0.460 | 10.781 | 23.45x |
| accuracy_n100000_k2 | 0.013 | 3.789 | 299.38x | 0.013 | 3.789 | 299.37x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.437 | 27.03x | 0.460 | 12.437 | 27.03x |
| classification_report_n100000_k2 | 0.464 | 27.513 | 59.36x | 0.464 | 27.512 | 59.36x |
| roc_auc_binary_n100000_k2 | 4.455 | 26.763 | 6.01x | 4.454 | 26.761 | 6.01x |
| balanced_accuracy_n100000_k2 | 0.460 | 10.904 | 23.72x | 0.460 | 10.903 | 23.72x |
| matthews_n100000_k2 | 0.460 | 21.763 | 47.30x | 0.460 | 21.762 | 47.30x |
| cohen_kappa_n100000_k2 | 0.460 | 10.890 | 23.69x | 0.460 | 10.889 | 23.69x |
| mse_n100000_k2 | 0.045 | 0.467 | 10.35x | 0.045 | 0.467 | 10.36x |
| mae_n100000_k2 | 0.045 | 0.472 | 10.44x | 0.045 | 0.472 | 10.44x |
| median_ae_n100000_k2 | 0.596 | 1.813 | 3.04x | 0.623 | 1.813 | 2.91x |
| r2_n100000_k2 | 0.110 | 0.661 | 6.03x | 0.110 | 0.661 | 6.03x |
| confusion_matrix_n100000_k10 | 0.492 | 10.815 | 21.98x | 0.492 | 10.815 | 21.98x |
| accuracy_n100000_k10 | 0.012 | 3.798 | 309.49x | 0.012 | 3.797 | 309.48x |
| precision_recall_f1_macro_n100000_k10 | 0.494 | 13.040 | 26.41x | 0.494 | 13.039 | 26.41x |
| classification_report_n100000_k10 | 0.500 | 30.019 | 60.09x | 0.500 | 30.017 | 60.09x |
| roc_auc_ovr_macro_n100000_k10 | 35.768 | 217.031 | 6.07x | 35.767 | 217.019 | 6.07x |
| balanced_accuracy_n100000_k10 | 0.495 | 10.873 | 21.97x | 0.495 | 10.873 | 21.96x |
| matthews_n100000_k10 | 0.493 | 22.345 | 45.36x | 0.493 | 22.343 | 45.36x |
| cohen_kappa_n100000_k10 | 0.496 | 11.001 | 22.20x | 0.496 | 11.001 | 22.20x |
| mse_n100000_k10 | 0.045 | 0.469 | 10.41x | 0.045 | 0.469 | 10.41x |
| mae_n100000_k10 | 0.045 | 0.471 | 10.45x | 0.045 | 0.471 | 10.45x |
| median_ae_n100000_k10 | 0.620 | 1.813 | 2.92x | 0.666 | 1.813 | 2.72x |
| r2_n100000_k10 | 0.110 | 0.680 | 6.19x | 0.110 | 0.680 | 6.19x |
| confusion_matrix_n1000000_k2 | 4.595 | 99.915 | 21.74x | 4.595 | 99.911 | 21.74x |
| accuracy_n1000000_k2 | 0.122 | 33.037 | 270.60x | 0.122 | 33.036 | 270.60x |
| precision_recall_f1_macro_n1000000_k2 | 4.600 | 108.371 | 23.56x | 4.600 | 108.265 | 23.54x |
| classification_report_n1000000_k2 | 4.608 | 211.813 | 45.96x | 4.608 | 211.692 | 45.94x |
| roc_auc_binary_n1000000_k2 | 56.508 | 289.622 | 5.13x | 56.505 | 289.594 | 5.13x |
| balanced_accuracy_n1000000_k2 | 4.598 | 99.722 | 21.69x | 4.598 | 99.719 | 21.69x |
| matthews_n1000000_k2 | 4.601 | 200.044 | 43.48x | 4.601 | 200.026 | 43.48x |
| cohen_kappa_n1000000_k2 | 4.595 | 99.748 | 21.71x | 4.595 | 99.744 | 21.71x |
| mse_n1000000_k2 | 0.446 | 1.597 | 3.58x | 0.446 | 1.595 | 3.58x |
| mae_n1000000_k2 | 0.452 | 2.080 | 4.60x | 0.452 | 2.073 | 4.59x |
| median_ae_n1000000_k2 | 5.710 | 14.187 | 2.48x | 5.735 | 14.186 | 2.47x |
| r2_n1000000_k2 | 1.096 | 2.757 | 2.52x | 1.096 | 2.755 | 2.52x |
| confusion_matrix_n1000000_k10 | 4.909 | 100.163 | 20.40x | 4.909 | 100.138 | 20.40x |
| accuracy_n1000000_k10 | 0.120 | 33.213 | 276.15x | 0.120 | 33.213 | 276.15x |
| precision_recall_f1_macro_n1000000_k10 | 4.899 | 114.117 | 23.29x | 4.899 | 114.110 | 23.29x |
| classification_report_n1000000_k10 | 4.891 | 235.468 | 48.14x | 4.888 | 235.330 | 48.14x |
| balanced_accuracy_n1000000_k10 | 4.903 | 99.857 | 20.37x | 4.903 | 99.850 | 20.37x |
| matthews_n1000000_k10 | 4.888 | 207.202 | 42.39x | 4.888 | 207.189 | 42.39x |
| cohen_kappa_n1000000_k10 | 4.905 | 99.754 | 20.34x | 4.905 | 99.745 | 20.34x |
| mse_n1000000_k10 | 0.453 | 1.696 | 3.75x | 0.453 | 1.694 | 3.74x |
| mae_n1000000_k10 | 0.456 | 2.223 | 4.87x | 0.456 | 2.221 | 4.87x |
| median_ae_n1000000_k10 | 6.048 | 14.208 | 2.35x | 6.278 | 14.206 | 2.26x |
| r2_n1000000_k10 | 1.125 | 2.702 | 2.40x | 1.125 | 2.696 | 2.40x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.257 | 0.876 | 0.39x | 2.257 | 0.876 | 0.39x |
| transform_frobenius_n500 | 7.664 | 1.648 | 0.22x | 7.663 | 1.648 | 0.22x |
| transform_frobenius_n2000 | 28.190 | 5.752 | 0.20x | 28.239 | 22.964 | 0.81x |
| transform_kl_n100 | 1.128 | 17.768 | 15.76x | 1.128 | 17.767 | 15.76x |
| transform_kl_n500 | 5.061 | 33.749 | 6.67x | 5.060 | 33.749 | 6.67x |
| transform_kl_n2000 | 20.042 | 122.106 | 6.09x | 20.106 | 445.784 | 22.17x |

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
| ols_summary_n1000 | 0.089 | 3.002 | 33.65x | 0.089 | 3.002 | 33.66x |
| ols_hac_n1000 | 0.168 | 3.501 | 20.79x | 0.168 | 3.501 | 20.79x |
| ols_cluster_n1000 | 0.062 | 3.550 | 57.09x | 0.062 | 3.550 | 57.09x |
| ols_summary_n10000 | 0.471 | 11.513 | 24.46x | 0.471 | 11.513 | 24.46x |
| ols_hac_n10000 | 1.730 | 12.753 | 7.37x | 1.736 | 12.752 | 7.34x |
| ols_cluster_n10000 | 0.612 | 13.072 | 21.35x | 0.615 | 13.071 | 21.27x |
| ols_summary_n100000 | 4.361 | 113.387 | 26.00x | 4.382 | 447.890 | 102.21x |
| ols_hac_n100000 | 18.013 | 123.774 | 6.87x | 18.501 | 492.336 | 26.61x |
| ols_cluster_n100000 | 7.095 | 127.287 | 17.94x | 7.402 | 508.382 | 68.68x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.658 | 4.395 | 2.65x | 1.663 | 4.395 | 2.64x |
| sparse_infrequent_10000 | 2.110 | 4.747 | 2.25x | 2.114 | 4.747 | 2.25x |
| sparse_100000 | 16.974 | 30.777 | 1.81x | 17.466 | 30.775 | 1.76x |
| sparse_infrequent_100000 | 18.827 | 25.560 | 1.36x | 19.268 | 25.559 | 1.33x |

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
| fe_entity_n1000 | 0.262 | 20.156 | 76.95x | 0.262 | 20.154 | 76.95x |
| fe_twoway_clustered_n1000 | 0.416 | 31.622 | 76.10x | 0.416 | 31.618 | 76.09x |
| fe_kernel_n1000 | 0.277 | 20.739 | 74.93x | 0.277 | 20.738 | 74.93x |
| between_robust_n1000 | 0.155 | 19.557 | 126.31x | 0.155 | 19.556 | 126.31x |
| first_difference_robust_n1000 | 0.288 | 23.352 | 81.08x | 0.288 | 23.349 | 81.08x |
| random_effects_n1000 | 0.281 | 26.942 | 95.74x | 0.281 | 26.941 | 95.75x |
| fe_entity_n10000 | 2.700 | 29.358 | 10.87x | 2.809 | 29.355 | 10.45x |
| fe_twoway_clustered_n10000 | 4.892 | 55.316 | 11.31x | 5.248 | 110.540 | 21.06x |
| fe_kernel_n10000 | 2.950 | 29.737 | 10.08x | 2.998 | 29.714 | 9.91x |
| between_robust_n10000 | 1.845 | 28.226 | 15.30x | 1.847 | 28.225 | 15.28x |
| first_difference_robust_n10000 | 3.002 | 32.438 | 10.81x | 3.011 | 32.434 | 10.77x |
| random_effects_n10000 | 2.858 | 38.523 | 13.48x | 2.896 | 38.520 | 13.30x |
| fe_entity_n100000 | 30.420 | 173.731 | 5.71x | 31.800 | 693.509 | 21.81x |
| fe_twoway_clustered_n100000 | 49.172 | 352.372 | 7.17x | 50.728 | 1262.615 | 24.89x |
| fe_kernel_n100000 | 32.323 | 162.698 | 5.03x | 33.869 | 648.669 | 19.15x |
| between_robust_n100000 | 19.698 | 161.583 | 8.20x | 20.477 | 528.429 | 25.81x |
| first_difference_robust_n100000 | 37.811 | 185.657 | 4.91x | 40.288 | 740.554 | 18.38x |
| random_effects_n100000 | 35.849 | 217.976 | 6.08x | 37.762 | 861.551 | 22.82x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 0.647 | 83.369 | 128.76x | 0.647 | 332.902 | 514.19x |
| fit_penalised_weighted_1000 | 0.648 | 95.175 | 146.91x | 0.648 | 379.814 | 586.32x |
| predict_survival_1000 | 0.826 | 2.375 | 2.88x | 0.828 | 2.375 | 2.87x |
| fit_10000 | 7.246 | 613.820 | 84.71x | 7.491 | 989.296 | 132.06x |
| fit_penalised_weighted_10000 | 7.229 | 643.074 | 88.96x | 7.516 | 1018.850 | 135.56x |
| predict_survival_10000 | 3.322 | 4.793 | 1.44x | 3.554 | 19.157 | 5.39x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.026 | 14.223 | 542.67x | 0.026 | 14.218 | 542.58x |
| logrank_fleming_harrington_1000 | 0.034 | 21.518 | 625.09x | 0.034 | 21.517 | 625.06x |
| multigroup_1000 | 0.113 | 28.847 | 254.54x | 0.113 | 28.847 | 254.55x |
| pairwise_1000 | 0.305 | 150.130 | 492.23x | 0.305 | 150.123 | 492.26x |
| rmst_1000 | 0.088 | 1323.340 | 15030.29x | 0.088 | 1323.278 | 15030.41x |
| fixed_point_1000 | 0.037 | 15.460 | 418.60x | 0.037 | 15.459 | 418.58x |
| concordance_1000 | 0.049 | 8.199 | 165.68x | 0.049 | 8.198 | 165.67x |
| logrank_wilcoxon_weighted_10000 | 0.332 | 16.040 | 48.25x | 0.332 | 16.039 | 48.24x |
| logrank_fleming_harrington_10000 | 0.417 | 25.456 | 61.05x | 0.417 | 25.455 | 61.09x |
| multigroup_10000 | 2.482 | 41.300 | 16.64x | 2.486 | 41.297 | 16.61x |
| pairwise_10000 | 4.816 | 166.142 | 34.50x | 4.816 | 166.136 | 34.50x |
| rmst_10000 | 1.829 | 1411.278 | 771.44x | 1.866 | 1411.179 | 756.19x |
| fixed_point_10000 | 0.387 | 17.983 | 46.46x | 0.387 | 17.981 | 46.46x |
| concordance_10000 | 2.372 | 97.463 | 41.10x | 2.372 | 97.459 | 41.10x |
| logrank_wilcoxon_weighted_100000 | 4.441 | 24.387 | 5.49x | 4.572 | 24.386 | 5.33x |
| logrank_fleming_harrington_100000 | 4.302 | 38.853 | 9.03x | 4.385 | 38.850 | 8.86x |
| multigroup_100000 | 11.268 | 54.780 | 4.86x | 11.462 | 54.777 | 4.78x |
| pairwise_100000 | 45.551 | 250.321 | 5.50x | 47.163 | 250.118 | 5.30x |
| rmst_100000 | 9.173 | 518.287 | 56.50x | 9.305 | 518.274 | 55.70x |
| fixed_point_100000 | 4.742 | 31.907 | 6.73x | 4.890 | 31.906 | 6.52x |
| concordance_100000 | 29.917 | 822.851 | 27.50x | 30.168 | 822.778 | 27.27x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.385 | 76.301 | 22.54x | 3.385 | 76.295 | 22.54x |
| loglogistic_left_1000 | 8.918 | 107.058 | 12.00x | 8.925 | 107.050 | 11.99x |
| lognormal_interval_1000 | 3.318 | 197.398 | 59.49x | 3.318 | 197.382 | 59.49x |
| generalized_gamma_1000 | 12.791 | 849.480 | 66.41x | 12.836 | 849.430 | 66.18x |
| weibull_aft_1000 | 7.034 | 139.523 | 19.84x | 7.040 | 139.504 | 19.82x |
| lognormal_aft_ridge_robust_1000 | 8.798 | 4872.213 | 553.77x | 8.799 | 4871.915 | 553.67x |
| loglogistic_aft_interval_1000 | 9.853 | 217.926 | 22.12x | 9.856 | 217.905 | 22.11x |
| breslow_fleming_harrington_1000 | 0.258 | 7.202 | 27.93x | 0.258 | 7.202 | 27.93x |
| kaplan_meier_left_1000 | 0.088 | 8.515 | 96.22x | 0.089 | 8.514 | 96.21x |
| weibull_10000 | 34.302 | 91.706 | 2.67x | 34.298 | 91.700 | 2.67x |
| loglogistic_left_10000 | 99.386 | 185.200 | 1.86x | 99.381 | 185.180 | 1.86x |
| lognormal_interval_10000 | 30.197 | 344.575 | 11.41x | 30.195 | 344.550 | 11.41x |
| generalized_gamma_10000 | 103.301 | 2075.665 | 20.09x | 103.295 | 2075.520 | 20.09x |
| weibull_aft_10000 | 70.342 | 159.446 | 2.27x | 70.349 | 159.420 | 2.27x |
| lognormal_aft_ridge_robust_10000 | 123.345 | 47073.867 | 381.65x | 123.478 | 47071.346 | 381.21x |
| loglogistic_aft_interval_10000 | 88.934 | 290.727 | 3.27x | 88.937 | 290.698 | 3.27x |
| breslow_fleming_harrington_10000 | 2.544 | 8.838 | 3.47x | 2.544 | 8.838 | 3.47x |
| kaplan_meier_left_10000 | 0.956 | 10.196 | 10.67x | 0.956 | 10.195 | 10.67x |

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
| var_n1000 | 0.039 | 2.691 | 69.51x | 0.039 | 2.691 | 69.51x |
| var_n10000 | 0.486 | 16.772 | 34.50x | 0.490 | 16.771 | 34.24x |
| var_n100000 | 3.996 | 161.390 | 40.39x | 4.217 | 385.189 | 91.33x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `AgglomerativeIncumbentBenchmarks`
- `ArtifactLoadOnlyBenchmarks`
- `ArtifactTimeBenchmarks`
- `AsyncArtifactBenchmarks`
- `BatchEmbeddingBenchmarks`
- `BertNormalizerBenchmarks`
- `BkTreeBenchmarks`
- `BkTreeNearestBenchmarks`
- `BlockedTableBenchmarks`
- `Bm25Benchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `BpeWordCacheBenchmarks`
- `BucketRouteDiagnostics`
- `CdistCutoffBenchmarks`
- `CdistIncumbentBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `CorrelationBenchmarks`
- `DbscanDimensionBenchmarks`
- `DbscanIncumbentBenchmarks`
- `DecompositionBenchmarks`
- `DecompositionWidthBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `FuzzBenchmarks`
- `FuzzCodePointBenchmarks`
- `FuzzIncumbentBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `HouseholderQrBenchmarks`
- `IndelBenchmarks`
- `IndelCodePointBenchmarks`
- `IndelPatternBenchmarks`
- `KMeansFitIncumbentBenchmarks`
- `KMeansLloydIncumbentBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `KsAutoBenchmarks`
- `LcsGateBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `LevenshteinIncumbentBenchmarks`
- `ManyLabelsMetricsBenchmarks`
- `MetaNumericsPcaBenchmarks`
- `MetaNumericsStatsBenchmarks`
- `MetricsBenchmarks`
- `MetricsIncumbentBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `MyersGateBenchmarks`
- `PartialFitBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `PartitionValidityBenchmarks`
- `PersistenceBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `PrincipalComponentVarianceBenchmarks`
- `ProcessExtractBenchmarks`
- `RankingMetricsBenchmarks`
- `RegressionMetricsBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `SimilaritySketchBenchmarks`
- `SmallArtifactBenchmarks`
- `StopWordBenchmarks`
- `TokenPatternBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `VarianceAndFitBenchmarks`
- `VectorMathBenchmarks`
- `VectorStoreChurnBenchmarks`
- `VectorizerBenchmarks`
- `VectorizerIncumbentBenchmarks`

## Comparisons selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `cdist`
- `persistence`
- `splitters`
- `stats`
- `transformers`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 1.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `RatcliffObershelpBenchmarks` | Length=64 | `Similarity_NearDuplicate` | step | 18.69 | 14.08 | +33% | 32% |

**Still away from their median: 40.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `StudentQuantile` | drift | 360663 | 13202.3 | +2632% | 20% |
| `QuantileBenchmarks` | — | `StudentQuantileCauchyFarTail` | drift | 58222.3 | 3791.93 | +1435% | 20% |
| `DistributionTailBenchmarks` | — | `ChiSquaredSfFractionalDf` | step | 8.65 | 4.09 | +111% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalQuantile` | step | 0.012 | 0.1 | -88% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredCdf` | step | 0.029 | 0.23 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherCdf` | step | 0.053 | 0.42 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredCdf` | step | 0.024 | 0.19 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredQuantile` | step | 0.19 | 1.49 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherCdf` | step | 0.055 | 0.43 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherQuantile` | step | 0.128 | 1 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentCdf` | step | 0.137 | 1.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalCdf` | step | 0.009 | 0.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherQuantile` | step | 1.073 | 8.34 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredQuantile` | step | 0.157 | 1.22 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentCdf` | step | 0.122 | 0.94 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalQuantile` | step | 0.073 | 0.56 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentQuantile` | step | 2.301 | 17.61 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentQuantile` | step | 0.488 | 3.72 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalQuantile` | step | 0.012 | 0.09 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalCdf` | step | 0.027 | 0.2 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredQuantile` | step | 0.146 | 1.06 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalCdf` | step | 0.01 | 0.07 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredCdf` | step | 0.017 | 0.1 | -83% | 30% |
| `OlsBenchmarks` | SampleSize=100 | `Accord_Ols` | step | 7.06 | 39.12 | -82% | 46% |
| `StatsBenchmarks` | SampleSize=100 | `AccordMannWhitney` | step | 3.76 | 20.38 | -82% | 36% |
| `StatsBenchmarks` | SampleSize=100 | `AccordWelchT` | step | 6.22 | 33.16 | -81% | 31% |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | step | 0.87 | 4.61 | -81% | 30% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hac` | drift | 0.55 | 2.86 | -81% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarMannWhitney` | step | 0.36 | 1.86 | -81% | 80% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Cluster` | drift | 0.25 | 1.27 | -80% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarChiSquare` | step | 0.02 | 0.1 | -80% | 55% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hc0` | drift | 0.27 | 1.43 | -80% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `AccordChiSquare` | step | 0.04 | 0.19 | -79% | 55% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200 | `MathNet_Wls` | step | 0.42 | 1.61 | -74% | 53% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_FisherQuantile` | step | 1.146 | 0.69 | +66% | 30% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 2.17 | 3.6 | -41% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=2000 | `MathNet_Wls` | drift | 0.83 | 1.34 | -34% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_StudentQuantile` | step | 3.073 | 2.3 | +34% | 30% |
| `OsaBenchmarks` | Length=64 | `Distance_CodePoint` | drift | 28.41 | 22.84 | +24% | 20% |
| `QuantileBenchmarks` | — | `NormalQuantileFarTail` | drift | 1.21 | 0.98 | +23% | 20% |
