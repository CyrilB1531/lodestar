# Nightly benchmark run

<!-- nightly-baseline: fac280c117f7e86c65fd70bf1cf12e07b683e38a -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks ChainedProductBenchmarks CoxBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MultinomialLogitBenchmarks OlsBenchmarks QuantileBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks VectorAutoregressionBenchmarks WeightedLeastSquaresBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `fac280c117f7e86c65fd70bf1cf12e07b683e38a`
- Previous run: `fac280c117f7e86c65fd70bf1cf12e07b683e38a`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `BatchEmbeddingBenchmarks`
- `BkTreeBenchmarks`
- `BlockedTableBenchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `BpeWordCacheBenchmarks`
- `BucketRouteDiagnostics`
- `DecompositionBenchmarks`
- `KMeansFitIncumbentBenchmarks`
- `KMeansLloydIncumbentBenchmarks`
- `DbscanIncumbentBenchmarks`
- `DbscanDimensionBenchmarks`
- `AgglomerativeIncumbentBenchmarks`
- `PartialFitBenchmarks`
- `PrincipalComponentVarianceBenchmarks`
- `FuzzBenchmarks`
- `FuzzIncumbentBenchmarks`
- `FuzzCodePointBenchmarks`
- `MetaNumericsPcaBenchmarks`
- `MetaNumericsStatsBenchmarks`
- `KsAutoBenchmarks`
- `MetricsIncumbentBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `VectorizerIncumbentBenchmarks`
- `IndelBenchmarks`
- `IndelCodePointBenchmarks`
- `LcsGateBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinIncumbentBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `MetricsBenchmarks`
- `RegressionMetricsBenchmarks`
- `MyersGateBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `PersistenceBenchmarks`
- `StopWordBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `SimilaritySketchBenchmarks`
- `Bm25Benchmarks`
- `CoxBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `StatsBenchmarks`
- `RankTestBenchmarks`
- `DistributionTailBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `EncoderIncumbentBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SerialCorrelationBenchmarks`
- `QuantileBenchmarks`
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
- `SentencePieceBpeLineageBenchmarks`
- `BertNormalizerBenchmarks`
- `AddedTokenScanBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `RankingMetricsBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `ClassifierCurveBenchmarks`
- `PartitionValidityBenchmarks`
- `ClassificationReportBenchmarks`
- `MultiClassRocAucBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `DecompositionWidthBenchmarks`
- `HouseholderQrBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `LeastSquaresRoutingBenchmarks`

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          | 18.454 ns |  3.6493 ns | 0.2000 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  6.992 ns |  0.2986 ns | 0.0164 ns |  0.38 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail | 24.646 ns |  8.5655 ns | 0.4695 ns |  1.34 |    0.03 |         - |          NA |
| ChiSquaredSfHundredDf      | 53.795 ns | 15.3009 ns | 0.8387 ns |  2.92 |    0.05 |         - |          NA |
| ChiSquaredSfFractionalDf   | 62.592 ns | 30.8356 ns | 1.6902 ns |  3.39 |    0.09 |         - |          NA |
| NormalQuantile             | 94.382 ns |  3.8566 ns | 0.2114 ns |  5.11 |    0.05 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **55.39 μs** |   **5.156 μs** |  **0.283 μs** |  **1.00** | **0.1831** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    52.08 μs |  11.331 μs |  0.621 μs |  0.94 | 0.1831 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,517.53 μs** |  **39.269 μs** |  **2.152 μs** |  **1.00** |      **-** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,264.22 μs | 244.318 μs | 13.392 μs |  0.83 |      - |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error            | StdDev        | Ratio   | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|-----------------:|--------------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **539.99 ns** |         **4.945 ns** |      **0.271 ns** |    **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,702.48 ns |        36.437 ns |      1.997 ns |    3.15 |    0.00 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,193.56 ns |        68.850 ns |      3.774 ns |    4.06 |    0.01 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,251.33 ns |       871.926 ns |     47.793 ns |    6.02 |    0.08 | 0.0114 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     5,437.69 ns |       558.770 ns |     30.628 ns |   10.07 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     7,032.80 ns |     2,617.391 ns |    143.468 ns |   13.02 |    0.23 | 0.0153 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,163.05 ns |       159.473 ns |      8.741 ns |    4.01 |    0.01 | 0.0191 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,243.11 ns |     1,222.371 ns |     67.002 ns |    6.01 |    0.11 | 0.0114 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       571.19 ns |        15.745 ns |      0.863 ns |    1.06 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,345.64 ns |       120.530 ns |      6.607 ns |    4.34 |    0.01 | 0.0076 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       988.28 ns |        15.936 ns |      0.874 ns |    1.83 |    0.00 | 0.0095 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,024.87 ns |       555.239 ns |     30.434 ns |    1.90 |    0.05 | 0.0114 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       264.57 ns |       161.935 ns |      8.876 ns |    0.49 |    0.01 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,115.75 ns |       244.696 ns |     13.413 ns |    2.07 |    0.02 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        88.82 ns |        13.823 ns |      0.758 ns |    0.16 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       556.44 ns |       102.908 ns |      5.641 ns |    1.03 |    0.01 | 0.0114 |     968 B |          NA |
|                                |            |                 |                  |               |         |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **23,074.69 ns** |     **9,763.196 ns** |    **535.154 ns** |   **1.000** |    **0.03** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   140,915.39 ns |    13,737.395 ns |    752.993 ns |   6.109 |    0.12 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   441,043.40 ns |     3,017.042 ns |    165.374 ns |  19.120 |    0.38 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,772,657.53 ns |   104,293.541 ns |  5,716.681 ns |  76.850 |    1.54 |      - |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,001,039.09 ns |   118,234.095 ns |  6,480.810 ns |  43.398 |    0.89 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,112,775.39 ns | 1,366,615.458 ns | 74,908.805 ns | 134.948 |    3.88 |      - |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,240,560.07 ns |    74,694.849 ns |  4,094.277 ns |  53.782 |    1.08 |      - |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,787,746.94 ns |    91,732.793 ns |  5,028.184 ns |  77.504 |    1.55 |      - |   80371 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    51,232.48 ns |    15,921.842 ns |    872.730 ns |   2.221 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   212,166.12 ns |    30,835.509 ns |  1,690.198 ns |   9.198 |    0.19 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   215,935.62 ns |    31,532.313 ns |  1,728.392 ns |   9.361 |    0.20 | 0.7324 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   369,656.25 ns |    19,765.774 ns |  1,083.429 ns |  16.026 |    0.32 | 0.4883 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       256.10 ns |         6.269 ns |      0.344 ns |   0.011 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,099.53 ns |         1.897 ns |      0.104 ns |   0.048 |    0.00 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        88.71 ns |         9.226 ns |      0.506 ns |   0.004 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       552.20 ns |       163.898 ns |      8.984 ns |   0.024 |    0.00 | 0.0114 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,053.1 μs** |    **50.38 μs** |   **2.76 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    271.3 μs |    25.37 μs |   1.39 μs |   0.4883 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    585.0 μs |    20.97 μs |   1.15 μs |   2.9297 |        - |        - |   279681 B |
| WilcoxonPooledDifferences | 10000      | False |    885.4 μs |   194.55 μs |  10.66 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    472.1 μs |    25.10 μs |   1.38 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **647.4 μs** |    **47.38 μs** |   **2.60 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    246.2 μs |     7.32 μs |   0.40 μs |   0.4883 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    406.4 μs |     6.41 μs |   0.35 μs |   2.9297 |   0.4883 |        - |   279680 B |
| WilcoxonPooledDifferences | 10000      | True  |    796.7 μs |   358.34 μs |  19.64 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    440.9 μs |   118.62 μs |   6.50 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **11,085.7 μs** | **4,716.03 μs** | **258.50 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  3,249.2 μs |   141.91 μs |   7.78 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  8,077.0 μs | 3,123.19 μs | 171.19 μs | 328.1250 | 328.1250 | 328.1250 |  2800349 B |
| WilcoxonPooledDifferences | 100000     | False | 28,339.3 μs | 1,659.90 μs |  90.98 μs | 968.7500 | 968.7500 | 968.7500 | 13200772 B |
| MannWhitneyTest           | 100000     | False |  4,923.1 μs |   116.42 μs |   6.38 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,471.1 μs** |   **141.59 μs** |   **7.76 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,847.9 μs |   105.20 μs |   5.77 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  4,722.4 μs |   982.16 μs |  53.84 μs | 390.6250 | 390.6250 | 390.6250 |  2800386 B |
| WilcoxonPooledDifferences | 100000     | True  | 18,215.2 μs | 1,637.67 μs |  89.77 μs | 968.7500 | 968.7500 | 968.7500 | 13076300 B |
| MannWhitneyTest           | 100000     | True  |  4,349.9 μs |   594.41 μs |  32.58 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **6.733 μs** |  **6.7601 μs** | **0.3705 μs** |  **1.00** |    **0.07** |   **0.2899** |        **-** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  24.819 μs |  5.5951 μs | 0.3067 μs |  3.69 |    0.18 |   0.7935 |   0.0610 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  22.372 μs |  0.4249 μs | 0.0233 μs |  3.33 |    0.15 |   0.1831 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   2.791 μs |  0.9966 μs | 0.0546 μs |  0.42 |    0.02 |   0.0191 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   2.867 μs |  1.0379 μs | 0.0569 μs |  0.43 |    0.02 |   0.0420 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.312 μs |  0.3312 μs | 0.0182 μs |  0.20 |    0.01 |   0.0782 |        - |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.679 μs |  0.3479 μs | 0.0191 μs |  0.40 |    0.02 |   0.0992 |        - |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |  **86.184 μs** |  **6.4010 μs** | **0.3509 μs** |  **1.00** |    **0.00** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 712.069 μs | 45.5692 μs | 2.4978 μs |  8.26 |    0.04 | 249.0234 | 249.0234 | 249.0234 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 229.283 μs |  5.1119 μs | 0.2802 μs |  2.66 |    0.01 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  61.617 μs |  3.7231 μs | 0.2041 μs |  0.71 |    0.00 |   0.1221 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  62.911 μs |  1.8635 μs | 0.1021 μs |  0.73 |    0.00 |   0.3662 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  13.185 μs |  4.1048 μs | 0.2250 μs |  0.15 |    0.00 |   0.7629 |   0.0610 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  26.997 μs |  4.2490 μs | 0.2329 μs |  0.31 |    0.00 |   0.9460 |   0.0305 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean             | Error            | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |-----------------:|-----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |        **669.87 ns** |        **19.511 ns** |      **1.069 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     23,629.59 ns |       603.202 ns |     33.064 ns |   35.27 |    0.06 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      1,986.22 ns |        96.090 ns |      5.267 ns |    2.97 |    0.01 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 100        |     20,667.85 ns |       999.370 ns |     54.779 ns |   30.85 |    0.08 |   0.2747 |        - |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |         87.26 ns |        19.402 ns |      1.063 ns |    0.13 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 100        |        153.46 ns |        19.933 ns |      1.093 ns |    0.23 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
|                     |            |                  |                  |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **22,932.79 ns** |     **4,125.021 ns** |    **226.106 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |     83,731.61 ns |    10,688.338 ns |    585.864 ns |   3.651 |    0.04 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    446,946.17 ns |    10,482.870 ns |    574.601 ns |  19.491 |    0.17 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 11,363,433.51 ns | 1,068,112.602 ns | 58,546.856 ns | 495.542 |    4.78 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |         87.54 ns |         5.840 ns |      0.320 ns |   0.004 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        153.72 ns |        12.448 ns |      0.682 ns |   0.007 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------------ |---------:|---------:|---------:|---------:|----------:|
| ChatTemplate            | 84.48 ms | 2.469 ms | 0.135 ms | 333.3333 |   33.3 MB |
| ProseWithoutAddedTokens | 71.52 ms | 1.393 ms | 0.076 ms | 285.7143 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,423.5 μs** |     **534.03 μs** |     **29.27 μs** |   **1.00** |    **0.03** |   **248.0469** |  **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    69,734.7 μs |   4,997.22 μs |    273.91 μs |  49.00 |    0.88 |   625.0000 |  500.0000 |         - |  59880.37 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,341.7 μs** |     **109.30 μs** |      **5.99 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    81,868.7 μs |  94,723.87 μs |  5,192.14 μs |  61.02 |    3.36 |  1000.0000 |  857.1429 |         - |  83110.08 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **777.2 μs** |      **95.30 μs** |      **5.22 μs** |   **1.00** |    **0.01** |          **-** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   107,686.3 μs |  26,507.59 μs |  1,452.97 μs | 138.55 |    1.81 |   800.0000 |  200.0000 |         - |  81288.37 KB |    2,678.83 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,545.8 μs** |     **303.81 μs** |     **16.65 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    61,229.4 μs |  20,942.11 μs |  1,147.91 μs |  39.61 |    0.74 |   750.0000 |  500.0000 |         - |  64189.75 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **11,392.9 μs** |   **1,945.69 μs** |    **106.65 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,379,678.3 μs | 245,939.90 μs | 13,480.80 μs | 121.11 |    1.42 |  6000.0000 | 3000.0000 | 1000.0000 | 537131.64 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **10,676.9 μs** |     **795.73 μs** |     **43.62 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,589,810.2 μs | 388,722.45 μs | 21,307.19 μs | 148.90 |    1.81 | 10000.0000 | 5000.0000 | 2000.0000 | 747334.45 KB |       83.73 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **5,271.1 μs** |     **526.57 μs** |     **28.86 μs** |   **1.00** |    **0.01** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 2,503,266.5 μs | 552,615.56 μs | 30,290.72 μs | 474.91 |    5.46 |  9000.0000 | 5000.0000 | 2000.0000 | 732665.26 KB |    8,147.80 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **12,355.0 μs** |   **1,101.38 μs** |     **60.37 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,409,563.7 μs |  40,272.34 μs |  2,207.46 μs | 114.09 |    0.51 |  8000.0000 | 5000.0000 | 2000.0000 | 573606.88 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **4.694 μs** |  **0.9976 μs** | **0.0547 μs** |  **1.00** |    **0.01** | **0.0229** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.035 μs |  0.5388 μs | 0.0295 μs |  1.07 |    0.01 | 0.0305 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |   4.974 μs |  2.5731 μs | 0.1410 μs |  1.06 |    0.03 | 0.0305 |      - |   2.63 KB |        1.18 |
|                    |            |            |            |           |       |         |        |        |           |             |
| **UnitLoop**           | **8**          |  **47.313 μs** | **11.6639 μs** | **0.6393 μs** |  **1.00** |    **0.02** | **0.3052** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |  16.720 μs |  3.0364 μs | 0.1664 μs |  0.35 |    0.01 | 0.2441 |      - |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |  16.340 μs |  2.8889 μs | 0.1583 μs |  0.35 |    0.00 | 0.2441 |      - |  22.19 KB |        0.76 |
|                    |            |            |            |           |       |         |        |        |           |             |
| **UnitLoop**           | **32**         | **185.133 μs** |  **9.9685 μs** | **0.5464 μs** |  **1.00** |    **0.00** | **1.2207** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |  62.328 μs | 30.8909 μs | 1.6932 μs |  0.34 |    0.01 | 0.9766 |      - |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |  57.105 μs |  1.4536 μs | 0.0797 μs |  0.31 |    0.00 | 0.7935 |      - |  68.84 KB |        0.63 |
|                    |            |            |            |           |       |         |        |        |           |             |
| **UnitLoop**           | **128**        | **741.587 μs** | **20.8333 μs** | **1.1419 μs** |  **1.00** |    **0.00** | **4.8828** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 242.561 μs | 11.4175 μs | 0.6258 μs |  0.33 |    0.00 | 3.9063 |      - | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        | 213.609 μs |  6.7738 μs | 0.3713 μs |  0.29 |    0.00 | 3.1738 | 0.2441 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|---------:|----------:|
| **Encode** | **Ascii**    | **False**     | **28.14 ms** | **12.156 ms** | **0.666 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **30.11 ms** |  **0.993 ms** | **0.054 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **27.10 ms** |  **5.908 ms** | **0.324 ms** | **125.0000** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **38.36 ms** |  **0.901 ms** | **0.049 ms** | **214.2857** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **32.11 ms** |  **1.659 ms** | **0.091 ms** | **250.0000** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **44.79 ms** | **21.665 ms** | **1.188 ms** | **250.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **114.18 ms** |  **17.877 ms** | **0.980 ms** |  **1.00** |    **0.01** |   **27.17 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  68.08 ms |  27.707 ms | 1.519 ms |  0.60 |    0.01 |  103.65 KB |        3.81 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **122.11 ms** |  **13.500 ms** | **0.740 ms** |  **1.00** |    **0.01** |   **23.72 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  63.30 ms |   4.684 ms | 0.257 ms |  0.52 |    0.00 |  116.45 KB |        4.91 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **169.73 ms** |   **1.570 ms** | **0.086 ms** |  **1.00** |    **0.00** |  **103.22 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 283.31 ms |  36.581 ms | 2.005 ms |  1.67 |    0.01 |  258.46 KB |        2.50 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **174.76 ms** |  **23.821 ms** | **1.306 ms** |  **1.00** |    **0.01** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 239.85 ms |   3.777 ms | 0.207 ms |  1.37 |    0.01 |  192.71 KB |        3.53 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **212.49 ms** |   **4.094 ms** | **0.224 ms** |  **1.00** |    **0.00** |  **949.61 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 379.04 ms |  17.309 ms | 0.949 ms |  1.78 |    0.00 | 1366.35 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **223.94 ms** | **147.663 ms** | **8.094 ms** |  **1.00** |    **0.04** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 361.36 ms |  62.349 ms | 3.418 ms |  1.62 |    0.05 | 1153.52 KB |        1.56 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **250.97 ms** |  **50.878 ms** | **2.789 ms** |  **1.00** |    **0.01** | **5113.18 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 441.41 ms |  36.776 ms | 2.016 ms |  1.76 |    0.02 | 7215.27 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **253.12 ms** |  **33.416 ms** | **1.832 ms** |  **1.00** |    **0.01** | **5513.99 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 448.69 ms | 177.699 ms | 9.740 ms |  1.77 |    0.04 | 7964.22 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error          | StdDev       | Allocated |
|------- |------- |--------------:|---------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **36.52 μs** |       **0.218 μs** |     **0.012 μs** |         **-** |
| Cjk    | 1000   |      49.28 μs |      17.270 μs |     0.947 μs |         - |
| **Latin**  | **10000**  |   **3,488.78 μs** |     **212.278 μs** |    **11.636 μs** |         **-** |
| Cjk    | 10000  |   5,371.85 μs |      82.631 μs |     4.529 μs |         - |
| **Latin**  | **65536**  | **160,524.22 μs** | **103,424.070 μs** | **5,669.022 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.619 μs** |      **0.2716 μs** |     **0.0149 μs** |     **1.00** |    **0.01** |   **0.0038** |        **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.033 μs |      0.0949 μs |     0.0052 μs |     1.26 |    0.01 |   0.0038 |        - |        - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       2.674 μs |      0.3797 μs |     0.0208 μs |     1.65 |    0.02 |   0.0610 |        - |        - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   6,662.116 μs |    260.9418 μs |    14.3031 μs | 4,114.47 |   33.49 | 875.0000 | 859.3750 | 859.3750 |  4838298 B |   11,411.08 |
| LuceneFromText         | 1000      |   5,477.774 μs |    183.9075 μs |    10.0806 μs | 3,383.03 |   27.35 |  15.6250 |   7.8125 |        - |  1375027 B |    3,242.99 |
|                        |           |                |                |               |          |         |          |          |          |            |             |
| **LodestarQuery**          | **20000**     |      **23.606 μs** |      **8.0122 μs** |     **0.4392 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      32.922 μs |      3.9991 μs |     0.2192 μs |     1.39 |    0.02 |        - |        - |        - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      13.388 μs |      1.7359 μs |     0.0952 μs |     0.57 |    0.01 |   0.0916 |        - |        - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 102,354.706 μs | 39,268.2309 μs | 2,152.4242 μs | 4,337.02 |  105.01 | 600.0000 | 400.0000 | 400.0000 | 83748102 B |  197,519.11 |
| LuceneFromText         | 20000     | 109,561.935 μs | 13,066.5118 μs |   716.2196 μs | 4,642.41 |   78.58 | 200.0000 |        - |        - | 22417147 B |   52,870.63 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev   | Ratio | Gen0     | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|---------:|----------:|------------:|
| Unigram | 23.55 ms | 1.128 ms | 0.062 ms |  1.00 |  62.5000 |   5.43 MB |        1.00 |
| Bpe     | 71.03 ms | 3.210 ms | 0.176 ms |  3.02 | 285.7143 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error      | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|-----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **15.00 μs** |   **1.106 μs** | **0.061 μs** | **0.0916** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **58.82 μs** |   **5.633 μs** | **0.309 μs** | **0.1221** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **151.57 μs** |   **3.161 μs** | **0.173 μs** | **0.2441** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **373.90 μs** | **110.610 μs** | **6.063 μs** | **0.4883** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 7.954 ms | 7.747 ms | 0.4246 ms |   1.45 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error      | StdDev    | Allocated |
|----------- |--------- |-----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **12.827 μs** |  **2.1294 μs** | **0.1167 μs** |         **-** |
| MyersGroup | cjk      | 209.638 μs | 16.9866 μs | 0.9311 μs |         - |
| **DpGroup**    | **latin**    |   **7.132 μs** |  **0.1716 μs** | **0.0094 μs** |         **-** |
| MyersGroup | latin    | 112.286 μs | 13.2482 μs | 0.7262 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error       | StdDev      | Gen0   | Allocated |
|------- |-------- |-------------:|------------:|------------:|-------:|----------:|
| **Report** | **10**      |     **373.5 ns** |    **339.9 ns** |    **18.63 ns** | **0.0186** |   **1.54 KB** |
| **Report** | **1000**    | **621,786.8 ns** | **61,086.9 ns** | **3,348.38 ns** | **0.9766** | **117.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 113.3 ms | 13.36 ms | 0.73 ms | 800.0000 | 800.0000 | 800.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 103.0 ms | 14.85 ms | 0.81 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | Samples | Clusters | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **1.689 ms** | **0.1542 ms** | **0.0085 ms** |        **-** |        **-** |        **-** |   **12.06 KB** |
| MutualInformationScore         | 100000  | 10       |   1.765 ms | 0.5935 ms | 0.0325 ms |        - |        - |        - |   12.06 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  11.874 ms | 0.6473 ms | 0.0355 ms | 234.3750 | 234.3750 | 234.3750 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **2.474 ms** | **0.0826 ms** | **0.0045 ms** | **199.2188** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   2.572 ms | 0.1314 ms | 0.0072 ms | 199.2188 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 101.790 ms | 7.7430 ms | 0.4244 ms | 333.3333 | 333.3333 | 333.3333 |  1744.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Length | Mean        | Error      | StdDev    | Gen0   | Allocated |
|--------- |------- |------------:|-----------:|----------:|-------:|----------:|
| **Distance** | **12**     |    **657.0 ns** |   **341.7 ns** |  **18.73 ns** | **0.0095** |     **840 B** |
| **Distance** | **120**    | **44,924.1 ns** | **2,595.5 ns** | **142.27 ns** |      **-** |    **2128 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **201.94 ms** |   **4.882 ms** | **0.268 ms** |  **1.00** |    **0.00** | **1333.3333** | **1333.3333** | **1333.3333** |  **28.69 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,453.97 ms |  22.979 ms | 1.260 ms |  7.20 |    0.01 | 8000.0000 | 7000.0000 | 7000.0000 |  156.3 MB |        5.45 |
|              |           |             |            |          |       |         |           |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **63.96 ms** | **164.487 ms** | **9.016 ms** |  **1.01** |    **0.18** | **1285.7143** | **1285.7143** | **1285.7143** |  **13.29 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   535.22 ms |  68.811 ms | 3.772 ms |  8.49 |    1.10 | 3000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **561.27 ms** |  **68.530 ms** |  **3.756 ms** |  **1.00** |    **0.01** |  **2000.0000** |  **2000.0000** | **2000.0000** | **112.13 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 4,289.74 ms | 745.279 ms | 40.851 ms |  7.64 |    0.08 | 11000.0000 |  7000.0000 | 6000.0000 | 531.84 MB |        4.74 |
| Dbscan_CalculateClusters | 20000x2x10 | 2,612.29 ms | 158.412 ms |  8.683 ms |  4.65 |    0.03 | 11000.0000 | 10000.0000 | 9000.0000 | 372.12 MB |        3.32 |
|                          |            |             |            |           |       |         |            |            |           |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **44.66 ms** |   **3.580 ms** |  **0.196 ms** |  **1.00** |    **0.01** |  **1272.7273** |  **1272.7273** | **1272.7273** |  **14.02 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   316.14 ms |  47.129 ms |  2.583 ms |  7.08 |    0.06 |  2500.0000 |  2500.0000 | 2500.0000 |  66.59 MB |        4.75 |
| Dbscan_CalculateClusters | 5000x2x5   |   184.71 ms |  16.813 ms |  0.922 ms |  4.14 |    0.02 |  3666.6667 |  3666.6667 | 3666.6667 |   40.4 MB |        2.88 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean     | Error     | StdDev   | Ratio | RatioSD |
|------------------------------------------ |---------:|----------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       | 16.84 ms |  5.095 ms | 0.279 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 93.97 ms | 25.100 ms | 1.376 ms |  5.58 |    0.11 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 15.70 ms |  8.217 ms | 0.450 ms |  0.93 |    0.03 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error       | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **32,990.2 μs** |  **1,333.0 μs** |    **73.07 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **12151.27 KB** |
| TruncatedSvd_Transform     | 500     |     436.2 μs |    104.5 μs |     5.73 μs |   91.3086 |   90.8203 |   90.8203 |    390.73 KB |
| Nmf_Frobenius_Rank20       | 500     | 192,641.1 μs | 87,800.1 μs | 4,812.62 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18143.39 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 228,977.9 μs | 30,919.5 μs | 1,694.80 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.11 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **194,674.0 μs** | **25,927.3 μs** | **1,421.16 μs** | **2666.6667** | **2666.6667** | **2666.6667** | **105766.49 KB** |
| TruncatedSvd_Transform     | 20000   |   1,917.4 μs |    243.2 μs |    13.33 μs |  496.0938 |  496.0938 |  496.0938 |   3437.86 KB |
| Nmf_Frobenius_Rank20       | 20000   | 732,028.8 μs | 48,777.2 μs | 2,673.65 μs | 5000.0000 | 5000.0000 | 5000.0000 |  156850.7 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 590,860.4 μs | 37,207.7 μs | 2,039.48 μs | 5000.0000 | 5000.0000 | 5000.0000 |  157305.7 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------- |---------:|---------:|--------:|-------:|----------:|
| Encode | 224.0 μs | 64.65 μs | 3.54 μs | 5.8594 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count  | Mean       | Error        | StdDev    | Allocated |
|------------ |------- |-----------:|-------------:|----------:|----------:|
| **SearchTop10** | **10000**  |   **531.7 μs** |     **30.72 μs** |   **1.68 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,243.3 μs** | **12,481.91 μs** | **684.18 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Rows   | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|-----------:|---------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **627.0 μs** |   **181.5 μs** |  **9.95 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  5,230.6 μs |   279.6 μs | 15.33 μs |        - |        - |        - |     21.02 KB |
| **FromBlockNormalized** | **100000** | **60,250.5 μs** | **1,271.1 μs** | **69.67 μs** | **222.2222** | **222.2222** | **222.2222** | **150000.24 KB** |
| MmrSelect100        | 100000 |  5,640.8 μs |   423.7 μs | 23.23 μs |        - |        - |        - |     21.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ETYCTQ : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean        | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-ETYCTQ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |   **204.86 μs** |    **18.461 μs** |    **20.519 μs** |  **1.01** |    **0.14** |        **-** |        **-** |        **-** |  **166.08 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |   180.16 μs |    18.340 μs |    19.623 μs |  0.89 |    0.12 |        - |        - |        - |   17.53 KB |        0.11 |
| Lodestar_Impute                 | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |   251.08 μs |    11.579 μs |    12.870 μs |  1.24 |    0.13 |        - |        - |        - |   93.25 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |   271.72 μs |    15.742 μs |    17.498 μs |  1.34 |    0.15 |        - |        - |        - |   34.19 KB |        0.21 |
| MlNet_OneHotEncoding_Read       | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 1000     | 1,261.48 μs |    95.598 μs |   106.257 μs |  6.21 |    0.77 |        - |        - |        - |  363.73 KB |        2.19 |
| MlNet_ReplaceMissingValues_Read | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 1000     | 1,148.00 μs |    66.040 μs |    73.403 μs |  5.65 |    0.63 |        - |        - |        - |  342.21 KB |        2.06 |
|                                 |            |                 |                |             |              |             |          |             |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |   128.62 μs |    42.840 μs |     2.348 μs |  1.00 |    0.02 |  49.8047 |  49.8047 |  49.8047 |  165.37 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    60.49 μs |     6.314 μs |     0.346 μs |  0.47 |    0.01 |   0.1831 |        - |        - |   16.81 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    16.24 μs |     1.274 μs |     0.070 μs |  0.13 |    0.00 |   1.1292 |        - |        - |   92.53 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     | 1,636.95 μs | 4,926.166 μs |   270.020 μs | 12.73 |    1.83 |  23.4375 |   5.8594 |        - | 1938.37 KB |       11.72 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |   746.99 μs |   450.999 μs |    24.721 μs |  5.81 |    0.19 |   9.7656 |   3.9063 |        - |   801.9 KB |        4.85 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |   208.54 μs |    54.624 μs |     2.994 μs |  1.62 |    0.03 |   3.4180 |   2.9297 |        - |  291.97 KB |        1.77 |
|                                 |            |                 |                |             |              |             |          |             |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-ETYCTQ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    | **3,785.59 μs** |   **318.094 μs** |   **312.411 μs** |  **1.01** |    **0.13** |        **-** |        **-** |        **-** | **3282.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 3,566.71 μs |    25.546 μs |    29.419 μs |  0.95 |    0.09 |        - |        - |        - |  314.41 KB |        0.10 |
| Lodestar_Impute                 | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 1,054.60 μs |    22.598 μs |    23.206 μs |  0.28 |    0.03 |        - |        - |        - | 1843.77 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 2,203.59 μs |    54.448 μs |    60.519 μs |  0.59 |    0.06 |        - |        - |        - |   33.53 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 9,217.02 μs | 1,021.322 μs | 1,176.156 μs |  2.45 |    0.39 |        - |        - |        - |  442.96 KB |        0.13 |
| MlNet_ReplaceMissingValues_Read | Job-ETYCTQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 8,421.96 μs |   261.279 μs |   290.411 μs |  2.24 |    0.23 |        - |        - |        - |  532.95 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |             |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 2,975.83 μs | 1,430.037 μs |    78.385 μs |  1.00 |    0.03 | 992.1875 | 992.1875 | 992.1875 | 3290.94 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 2,669.35 μs | 1,418.743 μs |    77.766 μs |  0.90 |    0.03 |  89.8438 |  89.8438 |  89.8438 |  314.36 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |   535.75 μs |   103.969 μs |     5.699 μs |  0.18 |    0.00 | 258.7891 | 258.7891 | 258.7891 | 1845.37 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 1,463.03 μs | 2,184.970 μs |   119.766 μs |  0.49 |    0.04 |  11.7188 |   2.9297 |        - |  986.36 KB |        0.30 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 3,065.01 μs | 1,533.274 μs |    84.044 μs |  1.03 |    0.03 |        - |        - |        - |   575.2 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 1,882.92 μs |   644.015 μs |    35.301 μs |  0.63 |    0.02 |   3.9063 |        - |        - |   435.5 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Records | Mean        | Error        | StdDev      | Gen0    | Gen1    | Gen2    | Allocated   |
|---------------- |-------- |------------:|-------------:|------------:|--------:|--------:|--------:|------------:|
| **FilteredTop10**   | **10000**   |    **512.2 μs** |     **30.55 μs** |     **1.67 μs** |       **-** |       **-** |       **-** |      **7.2 KB** |
| Unfiltered      | 10000   |    545.7 μs |     34.20 μs |     1.87 μs |       - |       - |       - |      2.8 KB |
| HybridSelective | 10000   |  2,174.4 μs |    126.41 μs |     6.93 μs | 70.3125 | 58.5938 | 54.6875 |  2577.95 KB |
| HybridBroad     | 10000   |  3,353.1 μs |     93.34 μs |     5.12 μs | 89.8438 | 78.1250 | 70.3125 |  3221.13 KB |
| **FilteredTop10**   | **100000**  |  **5,483.0 μs** |  **4,578.65 μs** |   **250.97 μs** |       **-** |       **-** |       **-** |      **7.2 KB** |
| Unfiltered      | 100000  |  8,531.8 μs | 25,121.76 μs | 1,377.01 μs |       - |       - |       - |     2.81 KB |
| HybridSelective | 100000  | 36,883.4 μs |  2,635.00 μs |   144.43 μs |       - |       - |       - | 23470.93 KB |
| HybridBroad     | 100000  | 51,698.1 μs |  3,415.01 μs |   187.19 μs |       - |       - |       - | 29359.82 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  74.28 ns |   4.324 ns |  0.237 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   | 537.41 ns |   6.182 ns |  0.339 ns |  7.24 |    0.02 |      - |         - |          NA |
| TokenSortRatio | 696.30 ns | 472.670 ns | 25.909 ns |  9.37 |    0.30 | 0.0134 |    1120 B |          NA |
| TokenSetRatio  | 752.29 ns |  19.381 ns |  1.062 ns | 10.13 |    0.03 | 0.0105 |     896 B |          NA |
| WRatio         | 980.93 ns |  71.854 ns |  3.939 ns | 13.21 |    0.06 | 0.0134 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error              | StdDev            | Ratio  | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|------------------:|-------:|--------:|-------:|-------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,638,686.6260 ns |     74,445.6488 ns |     4,080.6172 ns |  1.000 |    0.00 |      - |      - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    558,978.7793 ns |     29,150.9202 ns |     1,597.8603 ns |  0.341 |    0.00 | 6.8359 | 1.9531 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,000,000.1842 ns |     15,620.1919 ns |       856.1954 ns |  0.610 |    0.00 | 7.8125 |      - |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 75,915,267.6190 ns | 45,152,102.3908 ns | 2,474,939.1010 ns | 46.327 |    1.31 |      - |      - |  510817 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |         0.0000 ns |  0.000 |    0.00 |      - |      - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-------------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **73.05 ns** |     **5.454 ns** |   **0.299 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   167.90 ns |   139.829 ns |   7.664 ns |  2.30 |    0.09 | 0.0010 |      80 B |          NA |
|            |               |             |              |            |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **528.82 ns** |    **18.676 ns** |   **1.024 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 7,555.49 ns | 3,335.313 ns | 182.820 ns | 14.29 |    0.30 |      - |     160 B |          NA |
|            |               |             |              |            |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **777.91 ns** |    **52.984 ns** |   **2.904 ns** |  **1.00** |    **0.00** | **0.0105** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,498.63 ns |   199.952 ns |  10.960 ns |  1.93 |    0.01 | 0.0229 |    1944 B |        2.17 |
|            |               |             |              |            |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **935.20 ns** |     **6.059 ns** |   **0.332 ns** |  **1.00** |    **0.00** | **0.0134** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 3,992.06 ns | 2,761.825 ns | 151.385 ns |  4.27 |    0.14 | 0.0305 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.351 ms** | **0.0625 ms** | **0.0034 ms** | **253.9063** | **253.9063** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **20.482 ms** | **0.3915 ms** | **0.0215 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **20.44 ns** |     **3.451 ns** |   **0.189 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    26.93 ns |     1.963 ns |   0.108 ns |  1.32 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    20.39 ns |     1.214 ns |   0.067 ns |  1.00 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    19.24 ns |     2.103 ns |   0.115 ns |  0.94 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **23.28 ns** |     **4.042 ns** |   **0.222 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    29.46 ns |     7.039 ns |   0.386 ns |  1.27 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    23.24 ns |     8.931 ns |   0.490 ns |  1.00 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    20.24 ns |     3.407 ns |   0.187 ns |  0.87 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **24.18 ns** |    **11.463 ns** |   **0.628 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    33.30 ns |     2.257 ns |   0.124 ns |  1.38 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    24.67 ns |     3.278 ns |   0.180 ns |  1.02 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    21.91 ns |     2.040 ns |   0.112 ns |  0.91 |    0.02 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **26.60 ns** |     **3.064 ns** |   **0.168 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    33.15 ns |    10.099 ns |   0.554 ns |  1.25 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    29.37 ns |     2.635 ns |   0.144 ns |  1.10 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    25.55 ns |     0.946 ns |   0.052 ns |  0.96 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **40.61 ns** |     **1.691 ns** |   **0.093 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    48.39 ns |     6.924 ns |   0.380 ns |  1.19 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    47.55 ns |    16.049 ns |   0.880 ns |  1.17 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    39.40 ns |     0.446 ns |   0.024 ns |  0.97 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **52.31 ns** |    **18.774 ns** |   **1.029 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    53.41 ns |     9.025 ns |   0.495 ns |  1.02 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    51.67 ns |     1.527 ns |   0.084 ns |  0.99 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    46.59 ns |     1.969 ns |   0.108 ns |  0.89 |    0.02 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **299.51 ns** |   **316.545 ns** |  **17.351 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   320.23 ns |    29.268 ns |   1.604 ns |  1.07 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   286.03 ns |    34.755 ns |   1.905 ns |  0.96 |    0.05 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   289.43 ns |   142.338 ns |   7.802 ns |  0.97 |    0.05 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **3,729.13 ns** | **1,182.494 ns** |  **64.817 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,006.56 ns | 3,109.706 ns | 170.453 ns |  1.07 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 3,768.48 ns |   746.025 ns |  40.892 ns |  1.01 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,865.77 ns |   285.996 ns |  15.676 ns |  1.04 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **303.4 ns** |    **27.03 ns** |   **1.48 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    137.8 ns |    37.96 ns |   2.08 ns |  0.45 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **1,734.4 ns** |   **201.30 ns** |  **11.03 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,066.2 ns |   377.60 ns |  20.70 ns |  1.77 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **11,190.9 ns** |   **477.34 ns** |  **26.16 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 28,828.4 ns | 9,048.22 ns | 495.96 ns |  2.58 |    0.04 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,512.8 μs** |     **714.97 μs** |     **39.19 μs** |  **1.00** |    **0.01** |      **-** | **162.63 KB** |        **1.00** |
| NumFlat_Fit      | 10000x16x16 |  29,266.3 μs |   3,648.12 μs |    199.97 μs |  5.31 |    0.05 |      - |  10.33 KB |        0.06 |
| MetaNumerics_Fit | 10000x16x16 |  46,705.0 μs |  12,312.66 μs |    674.90 μs |  8.47 |    0.12 |      - |  41.83 KB |        0.26 |
|                  |             |              |               |              |       |         |        |           |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **640.6 μs** |      **43.98 μs** |      **2.41 μs** |  **1.00** |    **0.00** | **0.9766** | **156.73 KB** |        **1.00** |
| NumFlat_Fit      | 10000x2x8   |   3,352.8 μs |     489.45 μs |     26.83 μs |  5.23 |    0.04 |      - |      3 KB |        0.02 |
| MetaNumerics_Fit | 10000x2x8   |   2,460.9 μs |     189.50 μs |     10.39 μs |  3.84 |    0.02 |      - |  39.57 KB |        0.25 |
|                  |             |              |               |              |       |         |        |           |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **28,716.3 μs** |   **8,452.93 μs** |    **463.33 μs** |  **1.00** |    **0.02** |      **-** | **787.78 KB** |        **1.00** |
| NumFlat_Fit      | 50000x8x32  | 325,741.0 μs |   9,311.96 μs |    510.42 μs | 11.35 |    0.16 |      - |  15.46 KB |        0.02 |
| MetaNumerics_Fit | 50000x8x32  | 403,261.0 μs | 270,418.93 μs | 14,822.57 μs | 14.05 |    0.49 |      - | 199.91 KB |        0.25 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **7.276 ms** |  **0.9735 ms** | **0.0534 ms** |  **1.00** |    **0.01** |   **88.7 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x16x16 |  12.205 ms |  0.1826 ms | 0.0100 ms |  1.68 |    0.01 |  13.58 KB |        0.15 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.347 ms** |  **3.1185 ms** | **0.1709 ms** |  **1.00** |    **0.01** |  **98.92 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  78.324 ms |  4.9309 ms | 0.2703 ms |  2.97 |    0.02 |  96.42 KB |        0.97 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **70.025 ms** |  **1.8664 ms** | **0.1023 ms** |  **1.00** |    **0.00** | **410.26 KB** |        **1.00** |
| NumFlat_Lloyd  | 50000x8x32  | 157.674 ms | 82.7912 ms | 4.5381 ms |  2.25 |    0.06 |   36.4 KB |        0.09 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |   **104.08 ns** |     **4.578 ns** |   **0.251 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    44.63 ns |     0.383 ns |   0.021 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   103.28 ns |     6.785 ns |   0.372 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    69.12 ns |     0.284 ns |   0.016 ns |  0.66 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **183.06 ns** |     **3.872 ns** |   **0.212 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    53.88 ns |    20.134 ns |   1.104 ns |  0.29 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |   185.19 ns |    14.403 ns |   0.789 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    86.38 ns |    43.108 ns |   2.363 ns |  0.47 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **14**   |   **237.25 ns** |    **20.316 ns** |   **1.114 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |    57.06 ns |     9.766 ns |   0.535 ns |  0.24 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |   240.19 ns |    71.797 ns |   3.935 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 14   |    87.09 ns |    10.456 ns |   0.573 ns |  0.37 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **288.43 ns** |    **54.697 ns** |   **2.998 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    59.37 ns |     1.930 ns |   0.106 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   289.56 ns |    72.643 ns |   3.982 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 16   |    92.99 ns |     2.563 ns |   0.140 ns |  0.32 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **18**   |   **350.45 ns** |    **19.932 ns** |   **1.093 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |    63.20 ns |     2.916 ns |   0.160 ns |  0.18 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |   347.21 ns |     7.915 ns |   0.434 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    96.41 ns |     5.821 ns |   0.319 ns |  0.28 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **20**   |   **412.06 ns** |    **23.617 ns** |   **1.295 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |    64.15 ns |    11.419 ns |   0.626 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   412.54 ns |     5.339 ns |   0.293 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    96.91 ns |     1.352 ns |   0.074 ns |  0.24 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **557.62 ns** |    **37.507 ns** |   **2.056 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    69.57 ns |     1.270 ns |   0.070 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   559.62 ns |    92.237 ns |   5.056 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |   112.04 ns |     5.753 ns |   0.315 ns |  0.20 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   |   **949.12 ns** |   **156.118 ns** |   **8.557 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    85.15 ns |    37.042 ns |   2.030 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |   964.24 ns |   237.367 ns |  13.011 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |   126.27 ns |     7.517 ns |   0.412 ns |  0.13 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **2,304.90 ns** | **1,345.350 ns** |  **73.743 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 48   |    99.42 ns |     3.053 ns |   0.167 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,140.79 ns | 1,129.607 ns |  61.918 ns |  0.93 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |   180.40 ns |    37.380 ns |   2.049 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **3,671.30 ns** |   **610.884 ns** |  **33.485 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |   121.09 ns |    10.565 ns |   0.579 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 3,648.97 ns |   221.184 ns |  12.124 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |   219.09 ns |    16.389 ns |   0.898 ns |  0.06 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **8,296.05 ns** | **6,329.136 ns** | **346.921 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 96   |   253.66 ns |    32.597 ns |   1.787 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 8,050.51 ns |   516.316 ns |  28.301 ns |  0.97 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |   663.21 ns |    27.203 ns |   1.491 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **20.65 ns** |     **4.566 ns** |   **0.250 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     21.17 ns |     0.282 ns |   0.015 ns |  1.03 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    111.92 ns |     3.200 ns |   0.175 ns |  5.42 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     19.02 ns |     2.581 ns |   0.141 ns |  0.92 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **231.42 ns** |     **6.258 ns** |   **0.343 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    291.74 ns |    59.206 ns |   3.245 ns |  1.26 |    0.01 |         - |          NA |
| Distance_CodePoint         | 64     |    554.47 ns |    41.779 ns |   2.290 ns |  2.40 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    235.02 ns |    23.100 ns |   1.266 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **707.07 ns** |    **27.092 ns** |   **1.485 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,367.37 ns |   184.952 ns |  10.138 ns |  1.93 |    0.01 |         - |          NA |
| Distance_CodePoint         | 128    |  1,316.57 ns | 1,057.102 ns |  57.943 ns |  1.86 |    0.07 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    743.14 ns |   212.765 ns |  11.662 ns |  1.05 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    |  **9,795.42 ns** |   **947.925 ns** |  **51.959 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 13,972.56 ns | 2,390.658 ns | 131.040 ns |  1.43 |    0.01 |         - |          NA |
| Distance_CodePoint         | 512    | 11,175.80 ns |   429.426 ns |  23.538 ns |  1.14 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  9,800.24 ns | 3,082.810 ns | 168.979 ns |  1.00 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error         | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|--------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **278.6 ns** |      **11.15 ns** |     **0.61 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     181.9 ns |      70.32 ns |     3.85 ns |  0.65 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **272.3 ns** |      **22.39 ns** |     **1.23 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     182.9 ns |      59.67 ns |     3.27 ns |  0.67 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **354.4 ns** |      **65.49 ns** |     **3.59 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     255.7 ns |      27.87 ns |     1.53 ns |  0.72 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **357.0 ns** |      **27.37 ns** |     **1.50 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     264.1 ns |      78.35 ns |     4.29 ns |  0.74 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **434.8 ns** |      **18.11 ns** |     **0.99 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     326.1 ns |       4.81 ns |     0.26 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **428.0 ns** |      **61.29 ns** |     **3.36 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     328.4 ns |      25.83 ns |     1.42 ns |  0.77 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **512.6 ns** |     **110.58 ns** |     **6.06 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |     918.9 ns |      49.95 ns |     2.74 ns |  1.79 |    0.02 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **495.2 ns** |       **3.15 ns** |     **0.17 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |     897.0 ns |      32.12 ns |     1.76 ns |  1.81 |    0.00 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,535.5 ns** |      **62.03 ns** |     **3.40 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   4,166.0 ns |      29.35 ns |     1.61 ns |  2.71 |    0.01 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,581.2 ns** |      **89.45 ns** |     **4.90 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,084.9 ns |   2,445.96 ns |   134.07 ns |  2.58 |    0.07 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **12,441.5 ns** |     **743.65 ns** |    **40.76 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  51,215.7 ns |   8,731.15 ns |   478.58 ns |  4.12 |    0.04 |         - |          NA |
|                    |        |          |              |               |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **445,181.5 ns** | **119,186.22 ns** | **6,533.00 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  51,019.3 ns |   4,196.02 ns |   230.00 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **20.85 ns** |       **0.433 ns** |     **0.024 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      79.09 ns |       6.608 ns |     0.362 ns |  3.79 |    0.02 | 0.0006 |      56 B |          NA |
| Quickenshtein        | 8      |      67.82 ns |      71.344 ns |     3.911 ns |  3.25 |    0.16 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     161.21 ns |      53.864 ns |     2.952 ns |  7.73 |    0.12 | 0.0014 |     128 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **231.40 ns** |       **7.760 ns** |     **0.425 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   4,560.13 ns |   2,729.584 ns |   149.618 ns | 19.71 |    0.56 |      - |     280 B |          NA |
| Quickenshtein        | 64     |   1,236.87 ns |     599.495 ns |    32.860 ns |  5.35 |    0.12 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   6,944.77 ns |     969.780 ns |    53.157 ns | 30.01 |    0.20 |      - |     576 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **512**    |   **9,313.50 ns** |     **172.475 ns** |     **9.454 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 380,748.71 ns |  19,731.458 ns | 1,081.548 ns | 40.88 |    0.11 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  34,051.35 ns |   1,712.960 ns |    93.893 ns |  3.66 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 649,192.94 ns | 136,035.397 ns | 7,456.559 ns | 69.70 |    0.70 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **123.32 μs** |     **13.901 μs** |     **0.762 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 103,493.96 μs | 18,023.447 μs |   987.926 μs | 839.23 |    8.26 | 800.0000 | 800.0000 | 800.0000 | 31412.86 KB |   13,226.47 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,491.54 μs** |    **235.623 μs** |    **12.915 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.07 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 508,021.51 μs | 34,138.348 μs | 1,871.238 μs | 203.90 |    1.12 |        - |        - |        - | 32054.48 KB |      762.01 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **19.37 μs** |      **5.265 μs** |     **0.289 μs** |   **1.00** |    **0.02** |        **-** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     654.19 μs |    249.849 μs |    13.695 μs |  33.78 |    0.75 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      138.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,088.2 ns** |       **850.69 ns** |      **46.63 ns** | **0.0038** |     **376 B** |
| MatrixWeighted | 1000    | 2       |     5,056.7 ns |       409.70 ns |      22.46 ns |      - |     376 B |
| AccuracyScore  | 1000    | 2       |       113.8 ns |         1.73 ns |       0.09 ns |      - |         - |
| F1Macro        | 1000    | 2       |     2,876.8 ns |     2,465.72 ns |     135.15 ns | 0.0038 |     536 B |
| Report         | 1000    | 2       |     5,011.3 ns |       819.47 ns |      44.92 ns | 0.0610 |    5344 B |
| **Matrix**         | **1000**    | **10**      |     **3,135.3 ns** |       **168.50 ns** |       **9.24 ns** | **0.0153** |    **1312 B** |
| MatrixWeighted | 1000    | 10      |     5,331.2 ns |        91.08 ns |       4.99 ns | 0.0153 |    1312 B |
| AccuracyScore  | 1000    | 10      |       114.3 ns |         4.41 ns |       0.24 ns |      - |         - |
| F1Macro        | 1000    | 10      |     2,959.5 ns |       126.23 ns |       6.92 ns | 0.0191 |    1728 B |
| Report         | 1000    | 10      |     7,776.4 ns |       359.55 ns |      19.71 ns | 0.1373 |   12336 B |
| **Matrix**         | **100000**  | **2**       |   **295,770.5 ns** |     **9,805.21 ns** |     **537.46 ns** |      **-** |     **376 B** |
| MatrixWeighted | 100000  | 2       |   620,868.9 ns |   147,563.59 ns |   8,088.46 ns |      - |     377 B |
| AccuracyScore  | 100000  | 2       |    13,400.6 ns |     4,590.11 ns |     251.60 ns |      - |         - |
| F1Macro        | 100000  | 2       |   295,350.8 ns |    30,760.05 ns |   1,686.06 ns |      - |     536 B |
| Report         | 100000  | 2       |   276,604.6 ns |    12,962.77 ns |     710.53 ns |      - |    5368 B |
| **Matrix**         | **100000**  | **10**      |   **252,469.4 ns** |    **25,140.57 ns** |   **1,378.04 ns** |      **-** |    **1312 B** |
| MatrixWeighted | 100000  | 10      |   746,983.1 ns |   366,370.95 ns |  20,082.03 ns |      - |    1313 B |
| AccuracyScore  | 100000  | 10      |    13,401.5 ns |       800.48 ns |      43.88 ns |      - |         - |
| F1Macro        | 100000  | 10      |   274,709.7 ns |   144,847.68 ns |   7,939.59 ns |      - |    1728 B |
| Report         | 100000  | 10      |   277,229.8 ns |    84,309.79 ns |   4,621.30 ns |      - |   12680 B |
| **Matrix**         | **1000000** | **2**       | **2,760,355.7 ns** |   **183,873.96 ns** |  **10,078.75 ns** |      **-** |     **379 B** |
| MatrixWeighted | 1000000 | 2       | 6,442,240.8 ns | 4,285,686.26 ns | 234,912.93 ns |      - |     382 B |
| AccuracyScore  | 1000000 | 2       |   217,662.4 ns |    18,099.05 ns |     992.07 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,000,681.3 ns | 1,271,612.67 ns |  69,701.38 ns |      - |     539 B |
| Report         | 1000000 | 2       | 2,821,188.6 ns | 1,644,781.70 ns |  90,156.04 ns |      - |    5387 B |
| **Matrix**         | **1000000** | **10**      | **2,519,368.3 ns** |   **180,674.02 ns** |   **9,903.35 ns** |      **-** |    **1315 B** |
| MatrixWeighted | 1000000 | 10      | 7,577,830.0 ns |   655,101.14 ns |  35,908.30 ns |      - |    1318 B |
| AccuracyScore  | 1000000 | 10      |   220,762.8 ns |     1,304.41 ns |      71.50 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 2,721,907.4 ns | 1,338,409.81 ns |  73,362.76 ns |      - |    1731 B |
| Report         | 1000000 | 10      | 2,669,688.0 ns |   159,102.10 ns |   8,720.92 ns |      - |   12723 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **7,413.73 μs** |     **30.246 μs** |     **1.658 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1064 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  30,006.37 μs |  1,107.383 μs |    60.699 μs |     4.05 |    0.01 | 593.7500 | 593.7500 | 593.7500 |  5089912 B |    4,783.75 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **13.39 μs** |      **4.032 μs** |     **0.221 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  30,105.78 μs |  9,123.647 μs |   500.098 μs | 2,249.26 |   45.41 | 593.7500 | 593.7500 | 593.7500 |  5089895 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **141,907.52 μs** | **74,292.792 μs** | **4,072.239 μs** |     **1.00** |    **0.04** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 209,724.91 μs |  9,516.196 μs |   521.615 μs |     1.48 |    0.04 |        - |        - |        - | 23229116 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **221.49 μs** |     **26.823 μs** |     **1.470 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 184,425.45 μs | 14,660.434 μs |   803.588 μs |   832.69 |    5.72 |        - |        - |        - | 23228972 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean        | Error      | StdDev    | Allocated |
|---------- |-------- |------------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **36.02 ms** |   **1.731 ms** |  **0.095 ms** |         **-** |
| OneVsOne  | 100000  |    75.23 ms |  15.062 ms |  0.826 ms |  801865 B |
| **OneVsRest** | **1000000** |   **678.58 ms** |  **65.703 ms** |  **3.601 ms** |         **-** |
| OneVsOne  | 1000000 | 1,127.26 ms | 314.592 ms | 17.244 ms | 8003648 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| PerLabel         | 100000 |  6.887 ms |  2.2742 ms | 0.1247 ms |        - |        - |        - |     6.28 KB |
| PerLabelWeighted | 100000 |  9.484 ms |  1.4048 ms | 0.0770 ms |        - |        - |        - |     6.29 KB |
| PerSample        | 100000 | 44.840 ms | 44.5859 ms | 2.4439 ms | 437.5000 | 375.0000 | 125.0000 | 31250.15 KB |
| PerClass         | 100000 |  3.813 ms |  0.0259 ms | 0.0014 ms |        - |        - |        - |     6.42 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **59.97 ns** |     **3.925 ns** |  **0.215 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |    60.26 ns |     1.970 ns |  0.108 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |    60.90 ns |    14.283 ns |  0.783 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |    61.40 ns |    40.327 ns |  2.210 ns |  1.02 |    0.03 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **6**    |    **81.37 ns** |    **16.482 ns** |  **0.903 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 6    |    56.96 ns |     0.733 ns |  0.040 ns |  0.70 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    81.53 ns |     1.628 ns |  0.089 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    94.45 ns |     6.373 ns |  0.349 ns |  1.16 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **8**    |   **110.98 ns** |    **16.197 ns** |  **0.888 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |    67.97 ns |     0.959 ns |  0.053 ns |  0.61 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   110.37 ns |     2.922 ns |  0.160 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |   123.59 ns |     6.328 ns |  0.347 ns |  1.11 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **10**   |   **152.71 ns** |    **38.868 ns** |  **2.130 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 10   |    78.40 ns |    22.327 ns |  1.224 ns |  0.51 |    0.01 |         - |          NA |
| Dp_Cjk     | 10   |   150.33 ns |    10.718 ns |  0.587 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |   108.52 ns |    33.585 ns |  1.841 ns |  0.71 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **12**   |   **207.60 ns** |     **8.484 ns** |  **0.465 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    82.82 ns |     0.732 ns |  0.040 ns |  0.40 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   206.27 ns |   115.185 ns |  6.314 ns |  0.99 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |   116.21 ns |    31.256 ns |  1.713 ns |  0.56 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **16**   |   **309.16 ns** |     **6.135 ns** |  **0.336 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |   102.38 ns |    34.811 ns |  1.908 ns |  0.33 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |   323.03 ns |   110.557 ns |  6.060 ns |  1.04 |    0.02 |         - |          NA |
| Kernel_Cjk | 16   |   161.46 ns |     5.848 ns |  0.321 ns |  0.52 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **24**   |   **673.13 ns** |   **666.399 ns** | **36.528 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 24   |   131.60 ns |    58.237 ns |  3.192 ns |  0.20 |    0.01 |         - |          NA |
| Dp_Cjk     | 24   |   640.21 ns |    44.420 ns |  2.435 ns |  0.95 |    0.04 |         - |          NA |
| Kernel_Cjk | 24   |   171.69 ns |    15.246 ns |  0.836 ns |  0.26 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **32**   | **1,135.57 ns** |    **79.480 ns** |  **4.357 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |   160.34 ns |     5.947 ns |  0.326 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,139.77 ns |    71.823 ns |  3.937 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |   216.12 ns |     9.455 ns |  0.518 ns |  0.19 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **48**   | **2,509.83 ns** | **1,075.375 ns** | **58.945 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 48   |   227.36 ns |   101.823 ns |  5.581 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,482.35 ns |   300.676 ns | 16.481 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |   305.58 ns |   384.847 ns | 21.095 ns |  0.12 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **64**   | **4,623.16 ns** |   **782.302 ns** | **42.881 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |   286.39 ns |     3.365 ns |  0.184 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,474.60 ns |   678.537 ns | 37.193 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |   358.01 ns |    12.964 ns |  0.711 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **96**   | **9,755.91 ns** |   **160.477 ns** |  **8.796 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |   594.54 ns |    43.305 ns |  2.374 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 9,787.17 ns |   116.758 ns |  6.400 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   | 1,162.75 ns |    86.087 ns |  4.719 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **24.55 ns** |    **10.792 ns** |   **0.592 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    121.27 ns |    25.220 ns |   1.382 ns |  4.94 |    0.11 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **117.09 ns** |    **19.747 ns** |   **1.082 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,037.09 ns |    40.758 ns |   2.234 ns |  8.86 |    0.07 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **261.79 ns** |     **6.934 ns** |   **0.380 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  5,979.51 ns |   786.793 ns |  43.127 ns | 22.84 |    0.15 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **31,929.73 ns** | **1,935.726 ns** | **106.104 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 32,193.02 ns | 2,436.007 ns | 133.526 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean       | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |-----------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **331.2 μs** |    **125.93 μs** |   **6.90 μs** |  **1.00** |    **0.03** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   421.1 μs |     21.28 μs |   1.17 μs |  1.27 |    0.02 |    3680 B |        4.55 |
| SparseFit             | 10000    | 10         |   119.4 μs |     36.08 μs |   1.98 μs |  0.36 |    0.01 |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 10         |   414.0 μs |    128.61 μs |   7.05 μs |  1.25 |    0.03 |     784 B |        0.97 |
|                       |          |            |            |              |           |       |         |           |             |
| **WholeFit**              | **10000**    | **100**        |   **291.0 μs** |     **17.24 μs** |   **0.94 μs** |  **1.00** |    **0.00** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   559.1 μs |     60.02 μs |   3.29 μs |  1.92 |    0.01 |   36801 B |       45.55 |
| SparseFit             | 10000    | 100        |   120.5 μs |      7.54 μs |   0.41 μs |  0.41 |    0.00 |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 100        |   418.2 μs |    130.14 μs |   7.13 μs |  1.44 |    0.02 |     784 B |        0.97 |
|                       |          |            |            |              |           |       |         |           |             |
| **WholeFit**              | **100000**   | **10**         | **3,039.1 μs** |    **106.26 μs** |   **5.82 μs** |  **1.00** |    **0.00** |     **811 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 4,054.3 μs |    212.72 μs |  11.66 μs |  1.33 |    0.00 |    3686 B |        4.55 |
| SparseFit             | 100000   | 10         | 1,664.2 μs |    151.37 μs |   8.30 μs |  0.55 |    0.00 |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 10         | 4,561.7 μs | 13,685.20 μs | 750.13 μs |  1.50 |    0.21 |     790 B |        0.97 |
|                       |          |            |            |              |           |       |         |           |             |
| **WholeFit**              | **100000**   | **100**        | **3,255.2 μs** |    **596.35 μs** |  **32.69 μs** |  **1.00** |    **0.01** |     **811 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 4,640.5 μs |    275.26 μs |  15.09 μs |  1.43 |    0.01 |   36806 B |       45.38 |
| SparseFit             | 100000   | 100        | 1,676.4 μs |    175.91 μs |   9.64 μs |  0.52 |    0.01 |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 100        | 4,000.1 μs |    704.08 μs |  38.59 μs |  1.23 |    0.01 |     790 B |        0.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean       | Error       | StdDev    | Allocated |
|------------ |------------- |-----------:|------------:|----------:|----------:|
| **Embedded**    | **65**           |   **6.840 μs** |   **2.7418 μs** | **0.1503 μs** |         **-** |
| EqualLength | 65           |   2.023 μs |   0.0832 μs | 0.0046 μs |         - |
| **Embedded**    | **128**          |  **25.086 μs** |   **3.3146 μs** | **0.1817 μs** |         **-** |
| EqualLength | 128          |   3.945 μs |   1.0926 μs | 0.0599 μs |         - |
| **Embedded**    | **512**          | **890.485 μs** | **100.0917 μs** | **5.4864 μs** |         **-** |
| EqualLength | 512          |  32.976 μs |   7.2542 μs | 0.3976 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 |  7.769 ms | 0.5825 ms | 0.0319 ms | 156.2500 | 156.2500 | 156.2500 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 11.010 ms | 4.7985 ms | 0.2630 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean       | Error       | StdDev     | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |-----------:|------------:|-----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |   2.748 ms |   1.9967 ms |  0.1094 ms |  39.0625 |  35.1563 |  23.4375 |  3711.56 KB |
| TokenizerJsonWordPiece |   7.494 ms |   5.3543 ms |  0.2935 ms |  54.6875 |  46.8750 |  31.2500 |  5852.24 KB |
| TokenizerJsonUnigram   |   8.087 ms |   2.6216 ms |  0.1437 ms |        - |        - |        - |  4748.79 KB |
| SpieceModel            |   2.414 ms |   1.8542 ms |  0.1016 ms |  35.1563 |  31.2500 |  19.5313 |  3440.04 KB |
| TfidfSave              |   1.435 ms |   0.4300 ms |  0.0236 ms |  19.5313 |  19.5313 |  19.5313 |  2137.03 KB |
| TfidfLoad              |   2.934 ms |   1.4139 ms |  0.0775 ms |  15.6250 |   7.8125 |   7.8125 |  2930.47 KB |
| EmbeddingIndexSave     |   4.834 ms |   3.8278 ms |  0.2098 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |   6.269 ms |   0.2894 ms |  0.0159 ms | 125.0000 | 117.1875 | 117.1875 | 16094.35 KB |
| EmbeddingIndexSaveFile | 120.718 ms | 432.6402 ms | 23.7145 ms |        - |        - |        - |   323.67 KB |
| EmbeddingIndexLoadGzip |  70.104 ms |   4.6278 ms |  0.2537 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 11.42 ms | 2.303 ms | 0.126 ms |  31.2500 |   2.75 MB |
| EncodeDocuments    | 49.62 ms | 2.625 ms | 0.144 ms | 100.0000 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **6,327.87 μs** |   **240.301 μs** |  **13.172 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.27 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 14,090.38 μs | 2,686.778 μs | 147.271 μs |  2.23 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **119.16 μs** |     **8.138 μs** |   **0.446 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    169.47 μs |    13.932 μs |   0.764 μs |  1.42 |    0.01 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,577.10 μs** | **1,087.746 μs** |  **59.623 μs** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,790.48 μs |    82.736 μs |   4.535 μs |  1.08 |    0.02 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **19.43 μs** |     **4.740 μs** |   **0.260 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     23.22 μs |     1.957 μs |   0.107 μs |  1.20 |    0.01 |        - |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|--------:|----------:|------------:|
| **Extract**    | **1**     | **5.650 ms** | **1.5889 ms** | **0.0871 ms** |  **1.00** |    **0.02** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 5.785 ms | 2.2498 ms | 0.1233 ms |  1.02 |    0.02 |         - |        0.00 |
|            |       |          |           |           |       |         |           |             |
| **Extract**    | **5**     | **5.560 ms** | **0.3533 ms** | **0.0194 ms** |  **1.00** |    **0.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 5.785 ms | 0.3755 ms | 0.0206 ms |  1.04 |    0.00 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Q | Mean     | Error     | StdDev    | Allocated |
|------------------ |-- |---------:|----------:|----------:|----------:|
| **JaccardSimilarity** | **1** | **3.585 μs** | **0.5498 μs** | **0.0301 μs** |     **192 B** |
| **JaccardSimilarity** | **3** | **4.459 μs** | **0.0531 μs** | **0.0029 μs** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 27.242 ms | 6.6191 ms | 0.3628 ms |       - |       - |       - |  800310 B |
| NdcgIgnoringTies                  | 100000 | 25.029 ms | 0.7141 ms | 0.0391 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 15.862 ms | 0.3850 ms | 0.0211 ms |       - |       - |       - |  800215 B |
| ReciprocalRankScore               | 100000 | 13.428 ms | 0.5142 ms | 0.0282 ms |       - |       - |       - |     180 B |
| CoverageErrorScore                | 100000 |  6.018 ms | 0.1836 ms | 0.0101 ms | 15.6250 | 15.6250 | 15.6250 |  800271 B |
| LabelRankingAveragePrecisionScore | 100000 | 47.588 ms | 0.7737 ms | 0.0424 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Length | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **311.8 ns** |     **91.66 ns** |     **5.02 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   3,742.1 ns |    237.49 ns |    13.02 ns | 12.00 |    0.17 |         - |          NA |
|                          |        |              |              |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **15,351.4 ns** |  **5,252.43 ns** |   **287.90 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 465,508.9 ns | 58,435.29 ns | 3,203.04 ns | 30.33 |    0.52 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **46.01 μs** |   **4.365 μs** |  **0.239 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    45.23 μs |  16.874 μs |  0.925 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   116.27 μs |  39.724 μs |  2.177 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   465.30 μs | 117.948 μs |  6.465 μs | 199.7070 | 199.7070 | 199.7070 |  800186 B |
| **Mse**      | **1000000** |   **494.03 μs** |  **51.164 μs** |  **2.804 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   485.73 μs |  12.703 μs |  0.696 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,195.21 μs | 653.774 μs | 35.836 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,620.36 μs | 576.458 μs | 31.598 μs | 320.3125 | 320.3125 | 320.3125 | 8000267 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **21.59 ms** | **0.239 ms** | **0.013 ms** | **93.7500** | **93.7500** | **93.7500** |  **375.3 KB** |
| **Fit**    | **500x20000x5**  | **41.07 ms** | **2.231 ms** | **0.122 ms** |       **-** |       **-** |       **-** | **492.46 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-UAQQOP : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_MinMax**                   | **Job-UAQQOP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **259.50 μs** |    **30.922 μs** |    **35.609 μs** |  **1.02** |    **0.20** |        **-** |        **-** |        **-** |   **79.59 KB** |        **1.00** |
| Lodestar_MaxAbs                   | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    285.91 μs |    10.795 μs |    11.998 μs |  1.12 |    0.17 |        - |        - |        - |   79.14 KB |        0.99 |
| Lodestar_Robust                   | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,759.04 μs |    21.492 μs |    23.888 μs |  6.91 |    0.98 |        - |        - |        - |  157.23 KB |        1.98 |
| Lodestar_Standard                 | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    238.04 μs |     8.936 μs |     9.932 μs |  0.93 |    0.14 |        - |        - |        - |   79.66 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    265.70 μs |    10.717 μs |    11.912 μs |  1.04 |    0.15 |        - |        - |        - |    8.98 KB |        0.11 |
| MlNet_NormalizeMinMax_Read        | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,779.56 μs |    50.255 μs |    55.858 μs |  6.99 |    1.01 |        - |        - |        - |  324.38 KB |        4.08 |
| MlNet_NormalizeRobustScaling_Read | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  4,026.56 μs |   116.577 μs |   129.575 μs | 15.81 |    2.30 |        - |        - |        - |  467.07 KB |        5.87 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     38.12 μs |    22.229 μs |     1.218 μs |  1.00 |    0.04 |   0.9155 |        - |        - |   78.87 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     35.73 μs |     5.855 μs |     0.321 μs |  0.94 |    0.03 |   0.9155 |        - |        - |    78.4 KB |        0.99 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    415.08 μs |   139.046 μs |     7.622 μs | 10.90 |    0.34 |   1.4648 |        - |        - |  156.79 KB |        1.99 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     54.09 μs |     5.962 μs |     0.327 μs |  1.42 |    0.04 |   0.9155 |        - |        - |   78.71 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,320.20 μs | 4,430.169 μs |   242.833 μs | 34.66 |    5.60 |  16.6016 |   4.3945 |        - |  1366.2 KB |       17.32 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    868.15 μs | 1,219.680 μs |    66.855 μs | 22.79 |    1.64 |   7.8125 |        - |        - |  638.91 KB |        8.10 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,491.16 μs |   697.729 μs |    38.245 μs | 39.14 |    1.38 |   3.9063 |        - |        - |  626.03 KB |        7.94 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_MinMax**                   | **Job-UAQQOP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,269.16 μs** |    **14.165 μs** |    **16.312 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **1563.68 KB** |       **1.000** |
| Lodestar_MaxAbs                   | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,233.24 μs |    34.276 μs |    36.675 μs |  0.97 |    0.03 |        - |        - |        - | 1563.52 KB |       1.000 |
| Lodestar_Robust                   | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,998.17 μs |    87.483 μs |    97.238 μs | 10.24 |    0.15 |        - |        - |        - | 3126.26 KB |       1.999 |
| Lodestar_Standard                 | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,585.19 μs |    33.941 μs |    39.087 μs |  1.25 |    0.03 |        - |        - |        - | 1563.75 KB |       1.000 |
| MlNet_NormalizeMinMax_Fit         | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,559.72 μs |    23.113 μs |    26.617 μs |  2.81 |    0.04 |        - |        - |        - |     9.3 KB |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,514.10 μs | 5,278.612 μs | 6,078.858 μs | 10.65 |    4.68 |        - |        - |        - |  505.91 KB |       0.324 |
| MlNet_NormalizeRobustScaling_Read | Job-UAQQOP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,777.26 μs |   819.071 μs |   910.395 μs | 13.22 |    0.72 |        - |        - |        - | 4176.37 KB |       2.671 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,002.78 μs |   815.564 μs |    44.704 μs |  1.00 |    0.05 | 332.0313 | 332.0313 | 332.0313 | 1563.68 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    896.68 μs |    91.111 μs |     4.994 μs |  0.90 |    0.04 | 332.0313 | 332.0313 | 332.0313 | 1562.98 KB |        1.00 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 12,612.52 μs | 4,857.859 μs |   266.276 μs | 12.59 |    0.54 | 890.6250 | 890.6250 | 890.6250 | 3126.11 KB |        2.00 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,269.44 μs |   315.006 μs |    17.267 μs |  1.27 |    0.05 | 332.0313 | 332.0313 | 332.0313 |  1563.3 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,255.20 μs | 2,049.974 μs |   112.366 μs |  1.25 |    0.11 |   7.8125 |   1.9531 |        - |  686.19 KB |        0.44 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,386.19 μs | 3,379.568 μs |   185.246 μs |  3.38 |    0.21 |        - |        - |        - |  581.16 KB |        0.37 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 15,849.18 μs | 3,171.000 μs |   173.813 μs | 15.83 |    0.64 |  31.2500 |        - |        - | 4195.55 KB |        2.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Model   | Mean     | Error    | StdDev  | Gen0     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|----------:|
| **EncodeDocuments** | **Llama2**  | **131.0 ms** | **73.33 ms** | **4.02 ms** | **250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **122.5 ms** |  **7.49 ms** | **0.41 ms** | **400.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **22.03 ms** | **0.708 ms** | **0.039 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **141.00 ms** | **9.018 ms** | **0.494 ms** |       **-** |       **-** |       **-** | **371.54 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0      | Gen1     | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|------------:|-----------:|------:|--------:|----------:|---------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **49.498 ms** |   **4.1933 ms** |  **0.2298 ms** |  **1.00** |    **0.01** |         **-** |        **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   9.905 ms |   2.4422 ms |  0.1339 ms |  0.20 |    0.00 |   78.1250 |  78.1250 |  78.1250 |   2492.48 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   8.439 ms |   0.3915 ms |  0.0215 ms |  0.17 |    0.00 |         - |        - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   7.119 ms |   0.7983 ms |  0.0438 ms |  0.14 |    0.00 |         - |        - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  10.492 ms |   0.6197 ms |  0.0340 ms |  0.21 |    0.00 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |          |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **49.215 ms** |   **1.2682 ms** |  **0.0695 ms** |  **1.00** |    **0.00** |         **-** |        **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  12.579 ms |   6.6481 ms |  0.3644 ms |  0.26 |    0.01 |  218.7500 | 218.7500 | 218.7500 |   4538.22 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  10.279 ms |   0.9553 ms |  0.0524 ms |  0.21 |    0.00 |         - |        - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   7.066 ms |   0.4505 ms |  0.0247 ms |  0.14 |    0.00 |         - |        - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  10.306 ms |   0.7697 ms |  0.0422 ms |  0.21 |    0.00 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **838.935 ms** | **266.1737 ms** | **14.5899 ms** |  **1.00** |    **0.02** | **1000.0000** |        **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  40.741 ms |   7.3691 ms |  0.4039 ms |  0.05 |    0.00 |  538.4615 | 538.4615 | 461.5385 |  10165.44 KB |       0.080 |
| SignaturesOnly       | 2000      | 64           |  34.134 ms |   8.5843 ms |  0.4705 ms |  0.04 |    0.00 |         - |        - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  27.425 ms |   2.3196 ms |  0.1271 ms |  0.03 |    0.00 |         - |        - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  41.053 ms |   4.8087 ms |  0.2636 ms |  0.05 |    0.00 |         - |        - |        - |   1718.81 KB |       0.014 |
|                      |           |              |            |             |            |       |         |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **845.350 ms** | **155.6310 ms** |  **8.5307 ms** |  **1.00** |    **0.01** | **1000.0000** |        **-** |        **-** | **126359.44 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  53.639 ms |  11.8219 ms |  0.6480 ms |  0.06 |    0.00 |  900.0000 | 900.0000 | 800.0000 |  18495.21 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  40.191 ms |   7.3683 ms |  0.4039 ms |  0.05 |    0.00 |         - |        - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  28.039 ms |   1.3981 ms |  0.0766 ms |  0.03 |    0.00 |         - |        - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  41.573 ms |   1.5665 ms |  0.0859 ms |  0.05 |    0.00 |         - |        - |        - |   1718.81 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | SampleCount | Mean          | Error         | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|--------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **70.045 μs** |    **27.0904 μs** |  **1.4849 μs** |  **1.00** |    **0.03** |   **3.2959** |   **1.2207** |        **-** |  **273.94 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    145.485 μs |    58.6136 μs |  3.2128 μs |  2.08 |    0.06 |   3.6621 |   1.7090 |        - |  313.53 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.717 μs |     0.3791 μs |  0.0208 μs |  0.07 |    0.00 |   0.4730 |   0.0458 |        - |   39.14 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    244.030 μs |   162.2494 μs |  8.8934 μs |  3.48 |    0.13 |        - |        - |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  3,719.771 μs |    97.9481 μs |  5.3689 μs | 53.12 |    0.99 |        - |        - |        - |  148.65 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     62.482 μs |    45.0013 μs |  2.4667 μs |  0.89 |    0.03 |        - |        - |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |    672.966 μs |    62.0913 μs |  3.4034 μs |  9.61 |    0.18 |        - |        - |        - |   34.17 KB |        0.12 |
|                                 |             |               |               |            |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,035.896 μs** |   **553.4838 μs** | **30.3383 μs** |  **1.00** |    **0.04** | **263.6719** | **259.7656** | **259.7656** | **2736.79 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  1,656.689 μs |   562.4641 μs | 30.8306 μs |  1.60 |    0.05 | 541.0156 | 537.1094 | 537.1094 | 3129.79 KB |       1.144 |
| Lodestar_TrainTest              | 100000      |     91.887 μs |    96.9577 μs |  5.3146 μs |  0.09 |    0.00 |  41.0156 |  41.0156 |  41.0156 |  391.04 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    244.842 μs |    74.8901 μs |  4.1050 μs |  0.24 |    0.01 |        - |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 29,561.106 μs |   558.2106 μs | 30.5974 μs | 28.55 |    0.72 |        - |        - |        - |  148.71 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     60.003 μs |     6.9828 μs |  0.3827 μs |  0.06 |    0.00 |        - |        - |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  5,160.449 μs | 1,392.6108 μs | 76.3337 μs |  4.98 |    0.14 |        - |        - |        - |   34.17 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **4.394 ms** |  **2.7165 ms** | **0.1489 ms** |  **1.00** |    **0.04** | **109.3750** | **109.3750** | **109.3750** | **1366.79 KB** |        **1.00** |
| CountWithStopWords   | 200       |  3.673 ms |  0.4303 ms | 0.0236 ms |  0.84 |    0.02 |   7.8125 |        - |        - |  725.83 KB |        0.53 |
| Hashing              | 200       |  4.365 ms |  0.7889 ms | 0.0432 ms |  0.99 |    0.03 |  70.3125 |  70.3125 |  70.3125 | 1051.15 KB |        0.77 |
| HashingWithStopWords | 200       |  3.885 ms |  0.2136 ms | 0.0117 ms |  0.88 |    0.03 |        - |        - |        - |  591.84 KB |        0.43 |
|                      |           |           |            |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **14.012 ms** | **11.0331 ms** | **0.6048 ms** |  **1.00** |    **0.05** | **828.1250** | **828.1250** | **828.1250** | **5880.87 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 11.439 ms |  0.8453 ms | 0.0463 ms |  0.82 |    0.03 | 375.0000 | 375.0000 | 375.0000 | 3155.02 KB |        0.54 |
| Hashing              | 1000      | 13.751 ms |  0.4813 ms | 0.0264 ms |  0.98 |    0.04 | 531.2500 | 515.6250 | 515.6250 | 4793.15 KB |        0.82 |
| HashingWithStopWords | 1000      | 12.667 ms |  5.1942 ms | 0.2847 ms |  0.91 |    0.04 | 265.6250 | 265.6250 | 265.6250 | 2633.78 KB |        0.45 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Words | Mean       | Error     | StdDev    | Gen0     | Gen1    | Allocated |
|-------- |------ |-----------:|----------:|----------:|---------:|--------:|----------:|
| **Extract** | **2000**  |   **9.090 ms** | **0.8352 ms** | **0.0458 ms** |  **31.2500** | **15.6250** |   **3.13 MB** |
| **Extract** | **8000**  | **221.069 ms** | **3.5191 ms** | **0.1929 ms** | **333.3333** |       **-** |  **35.34 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **15.00 ms** |  **0.487 ms** | **0.027 ms** |  **1.00** |    **0.00** |  **93.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  40.20 ms |  0.479 ms | 0.026 ms |  2.68 |    0.00 |        - |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **SentencePiece** |  **36.93 ms** |  **5.627 ms** | **0.308 ms** |  **1.00** |    **0.01** |        **-** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  44.14 ms |  1.366 ms | 0.075 ms |  1.20 |    0.01 |        - |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **72.17 ms** |  **1.767 ms** | **0.097 ms** |  **1.00** |    **0.00** | **285.7143** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 216.84 ms | 99.439 ms | 5.451 ms |  3.00 |    0.07 | 666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.30 ms** | **4.772 ms** | **0.262 ms** |         **-** |
| **TopTwo** | **100**     | **80.34 ms** | **3.546 ms** | **0.194 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean     | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |----- |---------:|----------:|---------:|------:|--------:|----------:|------------:|
| **Dot**    | **384**  | **36.69 ns** | **15.240 ns** | **0.835 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| L2Norm | 384  | 30.75 ns |  5.910 ns | 0.324 ns |  0.84 |    0.02 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **768**  | **69.73 ns** |  **2.788 ns** | **0.153 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 768  | 56.70 ns |  1.542 ns | 0.085 ns |  0.81 |    0.00 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **1024** | **91.65 ns** |  **1.403 ns** | **0.077 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 1024 | 74.74 ns |  6.103 ns | 0.335 ns |  0.82 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |  **2.135 ms** | **0.5316 ms** | **0.0291 ms** |  **1.00** |    **0.02** |         **-** |         **-** |         **-** |   **302.94 KB** |        **1.00** |
| Tfidf                 | 200       |  2.152 ms | 0.1734 ms | 0.0095 ms |  1.01 |    0.01 |    3.9063 |         - |         - |   332.14 KB |        1.10 |
| CountBigrams          | 200       |  2.527 ms | 0.1477 ms | 0.0081 ms |  1.18 |    0.01 |    3.9063 |         - |         - |   590.31 KB |        1.95 |
| CountCharWordBoundary | 200       |  1.770 ms | 0.3391 ms | 0.0186 ms |  0.83 |    0.01 |  382.8125 |  382.8125 |  382.8125 |   1685.7 KB |        5.56 |
| Hashing               | 200       |  2.112 ms | 0.1026 ms | 0.0056 ms |  0.99 |    0.01 |         - |         - |         - |   233.59 KB |        0.77 |
|                       |           |           |           |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **3.949 ms** | **0.5961 ms** | **0.0327 ms** |  **1.00** |    **0.01** |  **109.3750** |  **109.3750** |  **109.3750** |  **1280.03 KB** |        **1.00** |
| Tfidf                 | 1000      |  4.090 ms | 0.6233 ms | 0.0342 ms |  1.04 |    0.01 |  140.6250 |  140.6250 |  140.6250 |  1424.43 KB |        1.11 |
| CountBigrams          | 1000      |  5.642 ms | 1.0713 ms | 0.0587 ms |  1.43 |    0.02 |  398.4375 |  398.4375 |  398.4375 |  2291.91 KB |        1.79 |
| CountCharWordBoundary | 1000      | 11.084 ms | 1.3093 ms | 0.0718 ms |  2.81 |    0.03 | 1015.6250 | 1015.6250 | 1015.6250 | 12022.21 KB |        9.39 |
| Hashing               | 1000      |  3.937 ms | 0.1037 ms | 0.0057 ms |  1.00 |    0.01 |   70.3125 |   70.3125 |   70.3125 |  1015.48 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **4.644 ms** |   **0.1692 ms** |  **0.0093 ms** |  **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |   **2.01 MB** |        **1.00** |
| MlNet    | 200       |  41.864 ms |  33.3280 ms |  1.8268 ms |  9.01 |    0.34 |  7500.0000 |  7500.0000 |  7500.0000 |  28.28 MB |       14.10 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **16.895 ms** |   **2.6893 ms** |  **0.1474 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **9.17 MB** |        **1.00** |
| MlNet    | 1000      | 302.545 ms | 370.8883 ms | 20.3296 ms | 17.91 |    1.05 | 71000.0000 | 71000.0000 | 71000.0000 | 324.28 MB |       35.35 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `levenshtein`
- `indel`
- `metrics`
- `persistence`
- `stats`
- `splitters`
- `glm`
- `var`
- `ols`

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.313 | 2.115 | 6.76x | 0.313 | 2.114 | 6.76x |
| glm_gamma_n1000 | 0.409 | 2.577 | 6.30x | 0.409 | 2.577 | 6.30x |
| glm_poisson_exposure_n1000 | 0.327 | 2.421 | 7.41x | 0.327 | 2.421 | 7.41x |
| mnlogit_n1000 | 0.687 | 9.495 | 13.82x | 0.687 | 9.494 | 13.82x |
| glm_negative_binomial_n10000 | 2.768 | 8.307 | 3.00x | 2.773 | 8.306 | 3.00x |
| glm_gamma_n10000 | 3.440 | 9.912 | 2.88x | 3.444 | 9.912 | 2.88x |
| glm_poisson_exposure_n10000 | 3.346 | 9.749 | 2.91x | 3.351 | 9.748 | 2.91x |
| mnlogit_n10000 | 6.745 | 49.893 | 7.40x | 6.748 | 49.891 | 7.39x |
| glm_negative_binomial_n100000 | 29.578 | 95.243 | 3.22x | 29.773 | 379.937 | 12.76x |
| glm_gamma_n100000 | 36.433 | 108.263 | 2.97x | 36.561 | 432.761 | 11.84x |
| glm_poisson_exposure_n100000 | 42.564 | 103.502 | 2.43x | 42.780 | 413.082 | 9.66x |
| mnlogit_n100000 | 67.951 | 557.270 | 8.20x | 67.992 | 1226.156 | 18.03x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 91.3 | 13.8 | 6.59x C# faster |
| latin | 32 | 132.1 | 73.0 | 1.81x C# faster |
| latin | 128 | 379.8 | 312.7 | 1.21x C# faster |
| latin | 512 | 3357.5 | 3910.5 | 1.16x Py faster |
| cjk | 8 | 100.6 | 14.0 | 7.17x C# faster |
| cjk | 32 | 246.9 | 202.1 | 1.22x C# faster |
| cjk | 128 | 1665.6 | 1093.7 | 1.52x C# faster |
| cjk | 512 | 14665.4 | 6909.8 | 2.12x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 112.1 | 12.8 | 8.75x C# faster |
| latin | 32 | 191.7 | 129.9 | 1.48x C# faster |
| latin | 128 | 1307.6 | 706.8 | 1.85x C# faster |
| latin | 512 | 10403.9 | 9494.2 | 1.10x C# faster |
| cjk | 8 | 112.9 | 12.7 | 8.90x C# faster |
| cjk | 32 | 300.9 | 246.5 | 1.22x C# faster |
| cjk | 128 | 2481.1 | 1796.3 | 1.38x C# faster |
| cjk | 512 | 21929.1 | 15659.5 | 1.40x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.003 | 0.646 | 214.32x | 0.003 | 0.646 | 214.31x |
| accuracy_n1000_k2 | 0.000 | 0.327 | 2730.26x | 0.000 | 0.327 | 2730.02x |
| precision_recall_f1_macro_n1000_k2 | 0.003 | 1.186 | 409.40x | 0.003 | 1.186 | 409.37x |
| classification_report_n1000_k2 | 0.005 | 4.399 | 933.23x | 0.005 | 4.399 | 933.17x |
| roc_auc_binary_n1000_k2 | 0.013 | 1.318 | 104.27x | 0.013 | 1.318 | 104.26x |
| balanced_accuracy_n1000_k2 | 0.003 | 0.697 | 248.53x | 0.003 | 0.697 | 248.52x |
| matthews_n1000_k2 | 0.003 | 1.320 | 470.67x | 0.003 | 1.320 | 470.66x |
| cohen_kappa_n1000_k2 | 0.003 | 0.728 | 258.74x | 0.003 | 0.728 | 258.74x |
| mse_n1000_k2 | 0.000 | 0.178 | 380.99x | 0.000 | 0.178 | 381.00x |
| mae_n1000_k2 | 0.000 | 0.178 | 386.32x | 0.000 | 0.178 | 386.32x |
| median_ae_n1000_k2 | 0.005 | 0.186 | 40.16x | 0.005 | 0.186 | 40.16x |
| r2_n1000_k2 | 0.001 | 0.215 | 181.54x | 0.001 | 0.215 | 181.54x |
| confusion_matrix_n1000_k10 | 0.003 | 0.645 | 201.20x | 0.003 | 0.645 | 201.17x |
| accuracy_n1000_k10 | 0.000 | 0.327 | 2755.41x | 0.000 | 0.327 | 2755.22x |
| precision_recall_f1_macro_n1000_k10 | 0.003 | 1.181 | 364.01x | 0.003 | 1.181 | 363.99x |
| classification_report_n1000_k10 | 0.008 | 4.449 | 583.73x | 0.008 | 4.449 | 583.69x |
| roc_auc_ovr_macro_n1000_k10 | 0.455 | 6.778 | 14.91x | 0.455 | 6.777 | 14.91x |
| balanced_accuracy_n1000_k10 | 0.003 | 0.697 | 226.62x | 0.003 | 0.697 | 226.62x |
| matthews_n1000_k10 | 0.003 | 1.339 | 437.37x | 0.003 | 1.339 | 437.33x |
| cohen_kappa_n1000_k10 | 0.003 | 0.736 | 223.55x | 0.003 | 0.736 | 223.54x |
| mse_n1000_k10 | 0.000 | 0.177 | 384.20x | 0.000 | 0.177 | 384.16x |
| mae_n1000_k10 | 0.000 | 0.177 | 387.30x | 0.000 | 0.177 | 387.29x |
| median_ae_n1000_k10 | 0.005 | 0.189 | 40.79x | 0.005 | 0.189 | 40.79x |
| r2_n1000_k10 | 0.001 | 0.213 | 181.50x | 0.001 | 0.213 | 181.48x |
| confusion_matrix_n100000_k2 | 0.288 | 10.058 | 34.90x | 0.288 | 10.057 | 34.90x |
| accuracy_n100000_k2 | 0.013 | 3.469 | 261.41x | 0.013 | 3.469 | 261.40x |
| precision_recall_f1_macro_n100000_k2 | 0.273 | 11.421 | 41.82x | 0.273 | 11.421 | 41.82x |
| classification_report_n100000_k2 | 0.275 | 24.303 | 88.27x | 0.275 | 24.299 | 88.26x |
| roc_auc_binary_n100000_k2 | 3.503 | 23.293 | 6.65x | 3.503 | 23.293 | 6.65x |
| balanced_accuracy_n100000_k2 | 0.273 | 10.116 | 37.06x | 0.273 | 10.116 | 37.06x |
| matthews_n100000_k2 | 0.272 | 20.419 | 75.20x | 0.272 | 20.417 | 75.18x |
| cohen_kappa_n100000_k2 | 0.273 | 10.200 | 37.33x | 0.273 | 10.200 | 37.33x |
| mse_n100000_k2 | 0.044 | 0.418 | 9.46x | 0.044 | 0.418 | 9.46x |
| mae_n100000_k2 | 0.044 | 0.411 | 9.43x | 0.044 | 0.410 | 9.43x |
| median_ae_n100000_k2 | 0.518 | 1.529 | 2.95x | 0.526 | 1.529 | 2.91x |
| r2_n100000_k2 | 0.112 | 0.647 | 5.76x | 0.112 | 0.646 | 5.76x |
| confusion_matrix_n100000_k10 | 0.255 | 10.041 | 39.31x | 0.255 | 10.037 | 39.29x |
| accuracy_n100000_k10 | 0.013 | 3.479 | 261.04x | 0.013 | 3.479 | 261.02x |
| precision_recall_f1_macro_n100000_k10 | 0.254 | 11.919 | 46.94x | 0.254 | 11.919 | 46.95x |
| classification_report_n100000_k10 | 0.263 | 26.269 | 99.96x | 0.263 | 26.267 | 99.96x |
| roc_auc_ovr_macro_n100000_k10 | 35.549 | 182.085 | 5.12x | 35.547 | 182.076 | 5.12x |
| balanced_accuracy_n100000_k10 | 0.254 | 10.087 | 39.69x | 0.254 | 10.087 | 39.68x |
| matthews_n100000_k10 | 0.256 | 20.883 | 81.53x | 0.256 | 20.881 | 81.53x |
| cohen_kappa_n100000_k10 | 0.279 | 10.108 | 36.28x | 0.279 | 10.107 | 36.29x |
| mse_n100000_k10 | 0.046 | 0.406 | 8.90x | 0.046 | 0.406 | 8.90x |
| mae_n100000_k10 | 0.045 | 0.398 | 8.84x | 0.045 | 0.398 | 8.84x |
| median_ae_n100000_k10 | 0.556 | 1.502 | 2.70x | 0.592 | 1.502 | 2.54x |
| r2_n100000_k10 | 0.116 | 0.627 | 5.38x | 0.116 | 0.627 | 5.38x |
| confusion_matrix_n1000000_k2 | 2.722 | 95.475 | 35.07x | 2.722 | 95.445 | 35.06x |
| accuracy_n1000000_k2 | 0.226 | 31.604 | 140.09x | 0.226 | 31.602 | 140.09x |
| precision_recall_f1_macro_n1000000_k2 | 2.711 | 102.816 | 37.93x | 2.711 | 102.792 | 37.92x |
| classification_report_n1000000_k2 | 2.728 | 199.592 | 73.17x | 2.728 | 199.577 | 73.17x |
| roc_auc_binary_n1000000_k2 | 58.133 | 260.570 | 4.48x | 58.130 | 260.561 | 4.48x |
| balanced_accuracy_n1000000_k2 | 2.697 | 95.895 | 35.56x | 2.697 | 95.891 | 35.56x |
| matthews_n1000000_k2 | 2.703 | 191.153 | 70.71x | 2.703 | 191.143 | 70.71x |
| cohen_kappa_n1000000_k2 | 2.725 | 96.221 | 35.31x | 2.725 | 96.216 | 35.31x |
| mse_n1000000_k2 | 0.494 | 2.386 | 4.83x | 0.494 | 2.386 | 4.83x |
| mae_n1000000_k2 | 0.488 | 2.377 | 4.87x | 0.488 | 2.377 | 4.87x |
| median_ae_n1000000_k2 | 5.515 | 12.478 | 2.26x | 5.569 | 12.477 | 2.24x |
| r2_n1000000_k2 | 1.165 | 5.006 | 4.30x | 1.165 | 5.006 | 4.30x |
| confusion_matrix_n1000000_k10 | 2.526 | 95.748 | 37.90x | 2.526 | 95.717 | 37.89x |
| accuracy_n1000000_k10 | 0.214 | 31.640 | 148.12x | 0.214 | 31.639 | 148.12x |
| precision_recall_f1_macro_n1000000_k10 | 2.528 | 109.109 | 43.15x | 2.528 | 109.086 | 43.15x |
| classification_report_n1000000_k10 | 2.561 | 218.977 | 85.51x | 2.560 | 218.950 | 85.51x |
| balanced_accuracy_n1000000_k10 | 2.525 | 96.645 | 38.27x | 2.525 | 96.637 | 38.28x |
| matthews_n1000000_k10 | 2.530 | 198.810 | 78.58x | 2.530 | 198.801 | 78.58x |
| cohen_kappa_n1000000_k10 | 2.524 | 96.402 | 38.20x | 2.524 | 96.394 | 38.20x |
| mse_n1000000_k10 | 0.512 | 2.398 | 4.69x | 0.512 | 2.398 | 4.69x |
| mae_n1000000_k10 | 0.505 | 2.393 | 4.74x | 0.505 | 2.393 | 4.74x |
| median_ae_n1000000_k10 | 4.942 | 12.513 | 2.53x | 4.968 | 12.512 | 2.52x |
| r2_n1000000_k10 | 1.184 | 5.024 | 4.24x | 1.184 | 5.024 | 4.24x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.043 | 1.618 | 37.70x | 0.043 | 1.618 | 37.69x |
| ols_hac_n1000 | 0.155 | 2.070 | 13.35x | 0.155 | 2.070 | 13.36x |
| ols_cluster_n1000 | 0.059 | 2.112 | 36.07x | 0.059 | 2.111 | 36.06x |
| ols_summary_n10000 | 0.419 | 7.791 | 18.60x | 0.419 | 7.791 | 18.60x |
| ols_hac_n10000 | 1.558 | 8.787 | 5.64x | 1.560 | 8.786 | 5.63x |
| ols_cluster_n10000 | 0.562 | 9.128 | 16.23x | 0.563 | 9.127 | 16.20x |
| ols_summary_n100000 | 4.103 | 89.042 | 21.70x | 4.112 | 355.410 | 86.43x |
| ols_hac_n100000 | 15.908 | 100.229 | 6.30x | 16.076 | 400.243 | 24.90x |
| ols_cluster_n100000 | 6.068 | 102.016 | 16.81x | 6.168 | 407.660 | 66.09x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 2.834 | 8.266 | 2.92x | 2.955 | 8.265 | 2.80x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 7.002 | 13.541 | 1.93x | 7.177 | 13.540 | 1.89x | 706,526 | 706,526 |
| tokenizer_json_unigram | 9.346 | 31.931 | 3.42x | 10.174 | 31.931 | 3.14x | 1,990,038 | 1,990,038 |
| spiece_model | 2.986 | 25.432 | 8.52x | 3.154 | 25.431 | 8.06x | 533,084 | 533,084 |
| tfidf_save | 1.543 | 2.130 | 1.38x | 1.555 | 2.130 | 1.37x | 581,787 | 591,922 |
| tfidf_load | 3.201 | 3.492 | 1.09x | 3.333 | 3.492 | 1.05x | 581,787 | 591,922 |
| embedding_index_save | 4.633 | 1.957 | 0.42x | 4.802 | 1.957 | 0.41x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 81.060 | 70.503 | 0.87x | 8.254 | 4.902 | 0.59x | 20,589,007 | 15,360,128 |
| embedding_index_load | 6.101 | 1.224 | 0.20x | 6.472 | 1.224 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 7.506 | 1.030 | 0.14x | 7.893 | 1.024 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.722 | 1.224 | 0.26x | 5.018 | 1.224 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.951 | 1.223 | 0.63x | 2.105 | 1.223 | 0.58x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.000 | 82.11x | 0.000 | 0.000 | 82.10x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 365.920 | 521.716 | 1.43x | 365.896 | 521.687 | 1.43x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 67.857 | 79.488 | 1.17x | 68.230 | 79.484 | 1.16x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.081 | 0.086 | 1.06x | 0.081 | 0.086 | 1.06x |
| stratified_n10000 | 0.134 | 0.644 | 4.79x | 0.134 | 0.644 | 4.79x |
| traintest_n10000 | 0.004 | 0.154 | 34.41x | 0.004 | 0.154 | 34.41x |
| kfold_n100000 | 1.064 | 1.838 | 1.73x | 1.128 | 1.838 | 1.63x |
| stratified_n100000 | 1.482 | 6.521 | 4.40x | 1.559 | 6.521 | 4.18x |
| traintest_n100000 | 0.128 | 0.386 | 3.02x | 0.129 | 0.386 | 3.00x |
| kfold_n1000000 | 10.734 | 13.525 | 1.26x | 11.838 | 13.520 | 1.14x |
| stratified_n1000000 | 16.057 | 60.884 | 3.79x | 16.795 | 60.880 | 3.62x |
| traintest_n1000000 | 0.766 | 2.445 | 3.19x | 0.844 | 2.445 | 2.90x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.002 | 0.478 | 196.48x | 0.002 | 0.478 | 196.46x |
| mann_whitney_n1000 | 0.043 | 0.506 | 11.86x | 0.043 | 0.506 | 11.86x |
| chi_square_n1000 | 0.000 | 0.188 | 1277.82x | 0.000 | 0.188 | 1277.80x |
| welch_t_n10000 | 0.023 | 0.523 | 23.01x | 0.023 | 0.523 | 23.01x |
| mann_whitney_n10000 | 0.494 | 2.108 | 4.27x | 0.494 | 2.108 | 4.27x |
| chi_square_n10000 | 0.001 | 0.195 | 206.95x | 0.001 | 0.195 | 206.96x |
| welch_t_n100000 | 0.225 | 1.044 | 4.63x | 0.225 | 1.044 | 4.63x |
| mann_whitney_n100000 | 5.653 | 21.800 | 3.86x | 5.652 | 21.799 | 3.86x |
| chi_square_n100000 | 0.010 | 0.204 | 21.20x | 0.010 | 0.204 | 21.20x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.049 | 1.808 | 36.77x | 0.049 | 1.808 | 36.77x |
| var_n10000 | 0.563 | 10.987 | 19.51x | 0.565 | 10.987 | 19.44x |
| var_n100000 | 5.465 | 102.518 | 18.76x | 5.572 | 102.514 | 18.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Selected, and not reached tonight

The run stops starting classes when the budget left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are selected again tomorrow whether or not anything else changes.

- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `CoxBenchmarks`
- `GlmBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `KsExactTableBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MannWhitneyExactBenchmarks`
- `MultinomialLogitBenchmarks`
- `OlsBenchmarks`
- `QuantileBenchmarks`
- `RobustCovarianceBenchmarks`
- `SerialCorrelationBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `VectorAutoregressionBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 38.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `SparseFit` | step | 0.55 | 0.06 | +817% | 30% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `SparseFit` | step | 0.52 | 0.06 | +767% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | step | 0.41 | 0.06 | +583% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | step | 0.36 | 0.06 | +500% | 30% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | step | 1.5 | 0.69 | +117% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | step | 1.44 | 0.67 | +115% | 30% |
| `EncoderIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `MlNet_OneHotEncoding_Fit` | step | 12.73 | 5.93 | +115% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | step | 1.25 | 0.67 | +87% | 30% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `DenseFitOfTheSameData` | step | 1.23 | 0.68 | +81% | 30% |
| `StatsBenchmarks` | SampleSize=10000 | `LodestarMannWhitney` | step | 19.491 | 10.8775 | +79% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_TrainTestSplit` | step | 0.89 | 0.54 | +65% | 30% |
| `OsaBenchmarks` | Length=8 | `Distance_CodePoint` | step | 4.94 | 3.01 | +64% | 30% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `BatchedFit` | step | 1.33 | 0.85 | +56% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `BatchedFit` | step | 1.27 | 0.83 | +53% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=100000 | `MlNet_TrainTestSplit` | step | 0.06 | 0.04 | +50% | 30% |
| `StatsBenchmarks` | SampleSize=10000 | `AccordMannWhitney` | step | 495.542 | 330.894 | +50% | 30% |
| `LcsGateBenchmarks` | Band=20 | `Kernel` | step | 0.16 | 0.11 | +45% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x16x16 | `MetaNumerics_Fit` | step | 8.47 | 15.345 | -45% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | step | 1.92 | 1.35 | +42% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=50000x8x32 | `MetaNumerics_Fit` | step | 14.05 | 24.035 | -42% | 30% |
| `LcsGateBenchmarks` | Band=20 | `Kernel_Cjk` | step | 0.24 | 0.17 | +41% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `Lodestar_TrainTest` | step | 0.07 | 0.05 | +40% | 30% |
| `StatsBenchmarks` | SampleSize=10000 | `AccordChiSquare` | step | 0.007 | 0.005 | +40% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `AccordMannWhitney` | step | 30.85 | 22.12 | +39% | 30% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | step | 8.49 | 6.22 | +36% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | step | 1.48 | 2.28 | -35% | 30% |
| `StatsBenchmarks` | SampleSize=10000 | `LodestarChiSquare` | step | 0.004 | 0.003 | +33% | 30% |
| `BkTreeBenchmarks` | Radius=2, Shape=clustered | `TreeWithinDistance` | step | 1.67 | 1.26 | +33% | 30% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | step | 1.43 | 1.08 | +32% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Fit` | step | 5.23 | 7.635 | -31% | 30% |
| `DistributionTailBenchmarks` | — | `NormalQuantile` | step | 5.11 | 3.89 | +31% | 30% |
| `LevenshteinCodePointBenchmarks` | Length=512, Distinct=512 | `Distance_Utf16` | step | 0.11 | 0.16 | -31% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=64 | `Quickenshtein` | step | 5.35 | 4.08 | +31% | 31% |
| `AgglomerativeIncumbentBenchmarks` | Rows=500, Method=single | `Aglomera_GetClustering` | step | 138.55 | 198.36 | -30% | 30% |
| `BkTreeBenchmarks` | Radius=4, Shape=uniform | `TreeWithinDistance` | step | 1.77 | 1.36 | +30% | 30% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Lloyd` | step | 2.97 | 4.245 | -30% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=512 | `F23_StringSimilarity` | drift | 69.7 | 50.37 | +26% | 20% |
| `LevenshteinCodePointBenchmarks` | Length=512, Distinct=32 | `Distance_Utf16` | drift | 4.12 | 3.41 | +21% | 20% |

**Still away from their median: 38.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=AccuracyAlone | `MlNet` | drift | 2249.26 | 147.17 | +2082% | 20% |
| `FuzzIncumbentBenchmarks` | Operation=PartialRatio | `FuzzySharp` | drift | 14.29 | 0.85 | +1459% | 20% |
| `TokenizerIncumbentBenchmarks` | Model=SentencePiece | `MlTokenizers` | drift | 1.2 | 0.15 | +700% | 20% |
| `IndelBenchmarks` | Length=512 | `Distance_CodePoint` | drift | 1.07 | 43.93 | -98% | 20% |
| `IndelBenchmarks` | Length=128 | `Distance_CodePoint` | drift | 1.07 | 22.68 | -95% | 20% |
| `FuzzIncumbentBenchmarks` | Operation=WRatio | `FuzzySharp` | drift | 4.27 | 2.19 | +95% | 20% |
| `FuzzBenchmarks` | — | `PartialRatio` | drift | 7.24 | 117.71 | -94% | 20% |
| `IndelBenchmarks` | Length=32 | `Distance_CodePoint` | drift | 1.02 | 16.61 | -94% | 20% |
| `IndelBenchmarks` | Length=24 | `Distance_CodePoint` | drift | 1.19 | 12.86 | -91% | 20% |
| `IndelBenchmarks` | Length=20 | `Distance_CodePoint` | drift | 1.25 | 5.22 | -78% | 20% |
| `IndelBenchmarks` | Length=12 | `Distance_CodePoint` | drift | 1.27 | 5.26 | -76% | 20% |
| `IndelBenchmarks` | Length=16 | `Distance_CodePoint` | drift | 1.38 | 5.16 | -74% | 20% |
| `IndelBenchmarks` | Length=8 | `Distance_CodePoint` | drift | 1.32 | 4.82 | -73% | 20% |
| `TokenizerIncumbentBenchmarks` | Model=WordPiece | `MlTokenizers` | step | 2.68 | 1.56 | +72% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=AccuracyAlone | `MlNet` | step | 832.69 | 2082.84 | -60% | 38% |
| `VectorizerIncumbentBenchmarks` | Documents=1000 | `MlNet` | drift | 17.91 | 12.89 | +56% | 20% |
| `BpeBenchmarks` | — | `Bpe` | drift | 3.02 | 1.71 | +56% | 20% |
| `FuzzBenchmarks` | — | `WRatio` | drift | 13.21 | 23.78 | -54% | 20% |
| `LcsGateBenchmarks` | Band=96 | `Kernel` | drift | 0.03 | 0.06 | -50% | 20% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | drift | 9.01 | 7.09 | +48% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 0.93 | 0.84 | +44% | 20% |
| `MyersGateBenchmarks` | Band=96 | `Kernel` | drift | 0.06 | 0.1 | -40% | 20% |
| `BatchEmbeddingBenchmarks` | CorpusSize=32 | `EmbedBatchBucketed` | drift | 0.31 | 0.54 | -39% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 4.05 | 4.17 | +33% | 20% |
| `LevenshteinCodePointBenchmarks` | Length=128, Distinct=512 | `Distance_Utf16` | drift | 2.58 | 2.11 | +33% | 20% |
| `BatchEmbeddingBenchmarks` | CorpusSize=8 | `EmbedBatchBucketed` | step | 0.35 | 0.52 | -33% | 30% |
| `BatchEmbeddingBenchmarks` | CorpusSize=8 | `EmbedBatch` | step | 0.35 | 0.51 | -31% | 30% |
| `BatchEmbeddingBenchmarks` | CorpusSize=128 | `EmbedBatch` | step | 0.33 | 0.48 | -31% | 30% |
| `BatchEmbeddingBenchmarks` | CorpusSize=128 | `EmbedBatchBucketed` | step | 0.29 | 0.42 | -31% | 30% |
| `LevenshteinCodePointBenchmarks` | Length=128, Distinct=32 | `Distance_Utf16` | drift | 2.71 | 2.14 | +31% | 20% |
| `BatchEmbeddingBenchmarks` | CorpusSize=32 | `EmbedBatch` | step | 0.34 | 0.49 | -31% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=512 | `Fastenshtein` | drift | 40.88 | 29.94 | +30% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel_Cjk` | drift | 0.28 | 0.28 | -29% | 20% |
| `MyersGateBenchmarks` | Band=64 | `Kernel_Cjk` | drift | 0.08 | 0.07 | +29% | 20% |
| `FuzzBenchmarks` | — | `TokenSortRatio` | drift | 9.37 | 10.79 | -25% | 20% |
| `FuzzBenchmarks` | — | `TokenSetRatio` | drift | 10.13 | 11.76 | -24% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel` | drift | 0.18 | 0.17 | -24% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 5.58 | 7.17 | -22% | 20% |
