# Nightly benchmark run

<!-- nightly-baseline: e501d10d5f1f806836c305d556184ea4d8a7e60a -->
<!-- nightly-owed: ArtifactLoadOnlyBenchmarks ArtifactTimeBenchmarks AsyncArtifactBenchmarks BitParallelEditDistanceBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DamerauLevenshteinBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks EmbeddingSearchBenchmarks EncoderIncumbentBenchmarks GlmBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks KsExactTableBenchmarks LevenshteinCodePointBenchmarks MannWhitneyExactBenchmarks ManyLabelsMetricsBenchmarks MathNetInteropBenchmarks MetricsBenchmarks MultinomialLogitBenchmarks MyersGateBenchmarks OlsBenchmarks OsaBenchmarks PartialRatioLongNeedleBenchmarks PersistenceBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RatcliffObershelpBenchmarks RegressionMetricsBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SimilaritySketchBenchmarks SmallArtifactBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks StopWordBenchmarks SurvivalBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorMathBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks cluster-weighted cox-extended cross-conformal glm iv nmf-transform ols onehot panel survival-aalen survival-family survival-parametric var -->
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

- Commit: `e501d10d5f1f806836c305d556184ea4d8a7e60a`
- Previous run: `e501d10d5f1f806836c305d556184ea4d8a7e60a`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `DoubleMetaphoneBenchmarks`
- `ProcessExtractBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `VectorStoreChurnBenchmarks`
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
- `BkTreeNearestBenchmarks`
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
- `CdistIncumbentBenchmarks`
- `CdistCutoffBenchmarks`
- `FuzzCodePointBenchmarks`
- `MetaNumericsPcaBenchmarks`
- `VarianceAndFitBenchmarks`
- `CorrelationBenchmarks`
- `MetaNumericsStatsBenchmarks`
- `KsAutoBenchmarks`
- `MetricsIncumbentBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `VectorizerIncumbentBenchmarks`
- `IndelBenchmarks`
- `IndelPatternBenchmarks`
- `IndelCodePointBenchmarks`
- `LcsGateBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinIncumbentBenchmarks`

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

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

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **928.4 ns** |          **9.48 ns** |       **0.52 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         722.7 ns |        111.67 ns |       6.12 ns |     0.78 |    0.01 |   0.0067 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       3,040.6 ns |        369.73 ns |      20.27 ns |     3.28 |    0.02 |   0.1640 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,844.3 ns |        740.44 ns |      40.59 ns |     4.14 |    0.04 |   0.0763 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,367.7 ns |        540.28 ns |      29.61 ns |     5.78 |    0.03 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,339.2 ns |      3,557.26 ns |     194.99 ns |    11.14 |    0.18 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,276.8 ns |        179.65 ns |       9.85 ns |     5.68 |    0.01 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **81,483.3 ns** |      **3,025.88 ns** |     **165.86 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      50,424.0 ns |      4,301.15 ns |     235.76 ns |     0.62 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,047,638.0 ns |    346,704.71 ns |  19,004.06 ns |    12.86 |    0.20 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,822,484.3 ns |     26,225.15 ns |   1,437.49 ns |    22.37 |    0.04 |   5.8594 |        - |        - |  160426 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,297,485.6 ns |     33,617.72 ns |   1,842.70 ns |    28.20 |    0.05 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 267,198,585.7 ns | 17,758,814.66 ns | 973,420.56 ns | 3,279.19 |   11.85 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,292,314.7 ns |    228,573.80 ns |  12,528.90 ns |    28.13 |    0.14 |        - |        - |        - |         - |          NA |

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
| **Fit**    | **1000**       |    **262.6 μs** |     **9.31 μs** |   **0.51 μs** |   **1.9531** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,821.4 μs** |    **90.84 μs** |   **4.98 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **28,658.4 μs** | **1,943.49 μs** | **106.53 μs** | **500.0000** | **500.0000** | **500.0000** | **5474.32 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

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

| Method     | SampleSize | Mean         | Error       | StdDev     | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|-----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **8.622 μs** |   **0.4475 μs** |  **0.0245 μs** |  **0.1831** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **122.680 μs** |  **15.8025 μs** |  **0.8662 μs** |  **0.9766** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,286.454 μs** | **382.4531 μs** | **20.9635 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **55.93 μs** |  **4.996 μs** | **0.274 μs** |  **1.00** | **0.6104** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    59.29 μs |  8.777 μs | 0.481 μs |  1.06 | 0.6104 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,725.45 μs** | **22.967 μs** | **1.259 μs** |  **1.00** | **5.8594** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,392.76 μs | 81.338 μs | 4.458 μs |  0.81 | 5.8594 |  156.3 KB |        1.00 |

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

| Method            | Regressors | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **127.7 μs** |      **26.66 μs** |      **1.46 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     216.4 μs |      44.23 μs |      2.42 μs |  1.69 |    0.02 |   1.2207 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     707.6 μs |     436.41 μs |     23.92 μs |  5.54 |    0.17 | 190.4297 | 190.4297 | 190.4297 |   705.62 KB |        3.75 |
|                   |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **84,253.5 μs** |   **6,016.19 μs** |    **329.77 μs** |  **1.00** |    **0.00** | **333.3333** | **333.3333** | **333.3333** |  **8865.82 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 357,991.6 μs |   7,910.37 μs |    433.59 μs |  4.25 |    0.02 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 554,032.7 μs | 526,830.62 μs | 28,877.36 μs |  6.58 |    0.30 |        - |        - |        - | 32670.65 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

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

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **4,616.8 ns** |     **610.27 ns** |     **33.45 ns** |   **1.00** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     1,952.0 ns |      88.65 ns |      4.86 ns |   0.42 |    0.00 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,377.1 ns |     167.58 ns |      9.19 ns |   0.51 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,995.9 ns |     990.01 ns |     54.27 ns |   0.87 |    0.01 | 0.0458 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     6,956.8 ns |     648.21 ns |     35.53 ns |   1.51 |    0.01 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,149.8 ns |     844.60 ns |     46.30 ns |   1.98 |    0.02 | 0.0610 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,580.9 ns |     187.07 ns |     10.25 ns |   0.56 |    0.00 | 0.0648 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,909.2 ns |     164.84 ns |      9.04 ns |   0.85 |    0.01 | 0.0458 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,913.6 ns |     222.99 ns |     12.22 ns |   0.41 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,787.5 ns |     140.77 ns |      7.72 ns |   0.60 |    0.00 | 0.0267 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,170.5 ns |     119.68 ns |      6.56 ns |   0.25 |    0.00 | 0.0324 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,161.7 ns |     112.70 ns |      6.18 ns |   0.25 |    0.00 | 0.0381 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       357.4 ns |      66.01 ns |      3.62 ns |   0.08 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,614.4 ns |     101.65 ns |      5.57 ns |   0.35 |    0.00 | 0.0362 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       112.9 ns |       1.87 ns |      0.10 ns |   0.02 |    0.00 | 0.0067 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       707.0 ns |       3.89 ns |      0.21 ns |   0.15 |    0.00 | 0.0381 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **72,360.2 ns** |   **1,173.48 ns** |     **64.32 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   156,164.2 ns |   4,419.34 ns |    242.24 ns |  2.158 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   471,979.5 ns |  11,645.23 ns |    638.31 ns |  6.523 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 1,945,988.2 ns | 206,280.40 ns | 11,306.92 ns | 26.893 |    0.14 |      - |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,179,508.7 ns | 122,035.84 ns |  6,689.20 ns | 16.301 |    0.08 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,432,870.0 ns | 360,954.92 ns | 19,785.16 ns | 47.441 |    0.24 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,378,344.1 ns |  32,387.84 ns |  1,775.29 ns | 19.048 |    0.03 | 5.8594 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,947,579.1 ns | 103,542.59 ns |  5,675.52 ns | 26.915 |    0.07 |      - |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |   108,909.3 ns |   1,610.49 ns |     88.28 ns |  1.505 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   234,664.3 ns |   4,577.41 ns |    250.90 ns |  3.243 |    0.00 |      - |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 10000      |   248,905.0 ns |  12,707.17 ns |    696.52 ns |  3.440 |    0.01 | 2.9297 |   80057 B |    1,250.89 |
| MetaNumerics_Wilcoxon          | 10000      |   390,541.2 ns |  47,094.71 ns |  2,581.42 ns |  5.397 |    0.03 | 2.9297 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       355.5 ns |       5.19 ns |      0.28 ns |  0.005 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,617.5 ns |     102.31 ns |      5.61 ns |  0.022 |    0.00 | 0.0362 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       112.0 ns |       7.30 ns |      0.40 ns |  0.002 |    0.00 | 0.0067 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       706.1 ns |      49.12 ns |      2.69 ns |  0.010 |    0.00 | 0.0381 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

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

| Method                   | GroupSize | Mean           | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **3,213.7 ns** |     **119.58 ns** |      **6.55 ns** |  **1.00** |    **0.00** |  **0.1984** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,477.1 ns |     217.89 ns |     11.94 ns |  1.39 |    0.00 |  0.2136 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       674.5 ns |      59.40 ns |      3.26 ns |  0.21 |    0.00 |  0.0029 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,307.8 ns |      50.91 ns |      2.79 ns |  0.41 |    0.00 |  0.0057 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       779.1 ns |      20.02 ns |      1.10 ns |  0.24 |    0.00 |  0.0019 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,233.1 ns |     626.27 ns |     34.33 ns |  1.32 |    0.01 |  0.0839 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     9,105.2 ns |     927.91 ns |     50.86 ns |  2.83 |    0.01 |  0.0305 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,148.0 ns |     742.08 ns |     40.68 ns |  1.91 |    0.01 |  0.0381 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,521.6 ns |     145.11 ns |      7.95 ns |  1.41 |    0.00 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **375,546.8 ns** |  **18,899.84 ns** |  **1,035.96 ns** | **1.000** |    **0.00** | **19.0430** | **4.3945** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,208,045.4 ns |  87,989.28 ns |  4,822.99 ns | 3.217 |    0.01 | 17.5781 | 3.9063 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    69,443.2 ns |   4,650.87 ns |    254.93 ns | 0.185 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   138,525.3 ns |   1,166.73 ns |     63.95 ns | 0.369 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,024.0 ns |     143.13 ns |      7.85 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   801,828.1 ns | 236,345.39 ns | 12,954.89 ns | 2.135 |    0.03 |  7.8125 | 0.9766 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,386,632.3 ns | 114,160.07 ns |  6,257.50 ns | 3.692 |    0.02 |  1.9531 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,231.8 ns |     349.45 ns |     19.15 ns | 0.030 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

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

| Method                  | Mean      | Error     | StdDev   | Gen0      | Allocated |
|------------------------ |----------:|----------:|---------:|----------:|----------:|
| ChatTemplate            | 106.50 ms |  7.186 ms | 0.394 ms | 1200.0000 |   33.3 MB |
| ProseWithoutAddedTokens |  93.30 ms | 48.844 ms | 2.677 ms | 1166.6667 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **2,334.5 μs** |     **944.15 μs** |     **51.75 μs** |   **1.00** |    **0.03** |   **246.0938** |  **246.0938** |  **246.0938** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    97,930.2 μs |   1,703.67 μs |     93.38 μs |  41.96 |    0.80 |  2333.3333 | 1166.6667 |  500.0000 |  59879.82 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **2,051.6 μs** |     **220.76 μs** |     **12.10 μs** |   **1.00** |    **0.01** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    98,713.0 μs |  42,210.30 μs |  2,313.69 μs |  48.12 |    1.01 |  3200.0000 | 1200.0000 |  600.0000 |  83111.22 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **749.0 μs** |      **90.03 μs** |      **4.93 μs** |   **1.00** |    **0.01** |     **0.9766** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   143,107.7 μs |  58,735.03 μs |  3,219.47 μs | 191.07 |    3.88 |  3250.0000 | 1250.0000 |  500.0000 |  81289.45 KB |    2,678.87 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,360.9 μs** |   **1,442.10 μs** |     **79.05 μs** |   **1.00** |    **0.04** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    75,290.1 μs |  22,550.49 μs |  1,236.07 μs |  31.91 |    1.04 |  2571.4286 | 1000.0000 |  428.5714 |  64190.65 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **16,352.3 μs** |   **4,045.87 μs** |    **221.77 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,736,605.8 μs | 674,774.80 μs | 36,986.68 μs | 106.21 |    2.32 | 23000.0000 | 9000.0000 | 3000.0000 | 537139.66 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,675.0 μs** |   **2,855.48 μs** |    **156.52 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,913,110.1 μs | 163,216.74 μs |  8,946.46 μs | 130.37 |    1.32 | 32000.0000 | 9000.0000 | 3000.0000 | 747340.08 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,199.9 μs** |     **446.66 μs** |     **24.48 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,291,351.6 μs | 134,768.04 μs |  7,387.09 μs | 530.88 |    2.08 | 31000.0000 | 9000.0000 | 3000.0000 | 732668.17 KB |    8,147.83 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **18,000.2 μs** |   **6,423.52 μs** |    **352.09 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,512,506.5 μs | 848,016.18 μs | 46,482.63 μs |  84.05 |    2.65 | 24000.0000 | 7000.0000 | 2000.0000 | 573604.29 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.824 μs** |   **0.6981 μs** | **0.0383 μs** |  **1.00** |  **0.0916** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.217 μs |   0.3243 μs | 0.0178 μs |  1.07 |  0.1144 |      - |   2.81 KB |        1.24 |
| EmbedBatchBucketed | 1          |   6.288 μs |   0.3185 μs | 0.0175 μs |  1.08 |  0.1144 |      - |   2.81 KB |        1.24 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **58.446 μs** |   **1.7985 μs** | **0.0986 μs** |  **1.00** |  **1.1597** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  20.771 μs |   1.2936 μs | 0.0709 μs |  0.36 |  0.8850 |      - |  22.38 KB |        0.76 |
| EmbedBatchBucketed | 8          |  20.837 μs |   0.5516 μs | 0.0302 μs |  0.36 |  0.8850 |      - |  22.38 KB |        0.76 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **225.785 μs** |   **8.1823 μs** | **0.4485 μs** |  **1.00** |  **4.3945** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  78.742 μs |  31.1275 μs | 1.7062 μs |  0.35 |  3.2959 |      - |  82.75 KB |        0.75 |
| EmbedBatchBucketed | 32         |  70.183 μs |   9.8989 μs | 0.5426 μs |  0.31 |  2.8076 |      - |  69.23 KB |        0.62 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **947.004 μs** | **162.1650 μs** | **8.8888 μs** |  **1.00** | **17.5781** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 312.641 μs |  97.7678 μs | 5.3590 μs |  0.33 | 13.1836 | 1.4648 | 329.78 KB |        0.74 |
| EmbedBatchBucketed | 128        | 268.178 μs |  19.9584 μs | 1.0940 μs |  0.28 | 10.2539 | 0.9766 | 262.27 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

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

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **36.81 ms** |  **1.601 ms** | **0.088 ms** |  **428.5714** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **40.06 ms** |  **0.466 ms** | **0.026 ms** |  **461.5385** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **35.76 ms** |  **2.756 ms** | **0.151 ms** |  **466.6667** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **49.84 ms** |  **3.674 ms** | **0.201 ms** |  **800.0000** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.49 ms** |  **7.814 ms** | **0.428 ms** |  **846.1538** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **54.49 ms** | **11.623 ms** | **0.637 ms** | **1000.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **136.76 ms** |  **22.237 ms** | **1.219 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  81.19 ms |   7.782 ms | 0.427 ms |  0.59 |    0.01 |  103.71 KB |        3.81 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **142.80 ms** | **109.968 ms** | **6.028 ms** |  **1.00** |    **0.05** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  78.13 ms |  61.130 ms | 3.351 ms |  0.55 |    0.03 |  116.52 KB |        4.88 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **201.85 ms** |  **11.306 ms** | **0.620 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 308.77 ms |  14.322 ms | 0.785 ms |  1.53 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **210.92 ms** |  **16.563 ms** | **0.908 ms** |  **1.00** |    **0.01** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 274.64 ms | 148.355 ms | 8.132 ms |  1.30 |    0.03 |  192.95 KB |        3.54 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **253.49 ms** |  **12.460 ms** | **0.683 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 423.37 ms |  78.828 ms | 4.321 ms |  1.67 |    0.02 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **263.84 ms** |  **13.996 ms** | **0.767 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 410.16 ms |  99.395 ms | 5.448 ms |  1.55 |    0.02 | 1153.52 KB |        1.56 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **298.03 ms** |  **29.445 ms** | **1.614 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 472.11 ms |  17.708 ms | 0.971 ms |  1.58 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **304.91 ms** |  **27.124 ms** | **1.487 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 517.84 ms |  89.133 ms | 4.886 ms |  1.70 |    0.02 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

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

| Method      | Count | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |----------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    |  **82.95 ms** |  **10.87 ms** | **0.596 ms** |  **1.00** |    **0.01** | **1833.3333** | **1833.3333** | **1833.3333** |   **9388.6 KB** |       **1.000** |
| TreeNearest | 10    |  65.20 ms | 102.10 ms | 5.596 ms |  0.79 |    0.06 |         - |         - |         - |    29.72 KB |       0.003 |
|             |       |           |           |          |       |         |           |           |           |             |             |
| **FullScan**    | **2000**  |  **85.18 ms** |  **13.21 ms** | **0.724 ms** |  **1.00** |    **0.01** | **1833.3333** | **1833.3333** | **1833.3333** | **10007.94 KB** |        **1.00** |
| TreeNearest | 2000  | 102.66 ms |  47.17 ms | 2.586 ms |  1.21 |    0.03 |         - |         - |         - |  1273.62 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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

| Method | length | Mean          | Error        | StdDev    | Allocated |
|------- |------- |--------------:|-------------:|----------:|----------:|
| **Latin**  | **1000**   |      **49.35 μs** |     **3.584 μs** |  **0.196 μs** |         **-** |
| Cjk    | 1000   |      66.64 μs |    19.315 μs |  1.059 μs |         - |
| **Latin**  | **10000**  |   **4,389.78 μs** |    **44.169 μs** |  **2.421 μs** |         **-** |
| Cjk    | 10000  |   6,944.49 μs |   266.855 μs | 14.627 μs |         - |
| **Latin**  | **65536**  | **183,735.64 μs** | **1,282.370 μs** | **70.291 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 27.50 ms |  2.541 ms | 0.139 ms |  1.00 |    0.01 |  312.5000 |   8.19 MB |        1.00 |
| Bpe     | 91.26 ms | 32.849 ms | 1.801 ms |  3.32 |    0.06 | 1166.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **19.41 μs** |  **1.352 μs** | **0.074 μs** | **0.3052** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **74.71 μs** | **16.433 μs** | **0.901 μs** | **0.4883** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **182.80 μs** |  **5.339 μs** | **0.293 μs** | **0.9766** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **388.37 μs** |  **4.001 μs** | **0.219 μs** | **1.9531** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 8.540 ms | 3.457 ms | 0.1895 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **17.252 μs** |  **1.333 μs** | **0.0731 μs** |         **-** |
| MyersGroup | cjk      | 241.133 μs | 18.802 μs | 1.0306 μs |         - |
| **DpGroup**    | **latin**    |   **9.397 μs** |  **1.671 μs** | **0.0916 μs** |         **-** |
| MyersGroup | latin    | 130.947 μs | 20.793 μs | 1.1397 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

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

| Method                   | Cutoff | Widths | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **347.86 μs** | **28.17 μs** | **1.544 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 331.61 μs | 40.99 μs | 2.247 μs |  0.95 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **373.39 μs** | **39.58 μs** | **2.170 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 337.65 μs | 66.17 μs | 3.627 μs |  0.90 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **359.79 μs** | **47.29 μs** | **2.592 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 340.33 μs | 27.00 μs | 1.480 μs |  0.95 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **312.35 μs** | **36.76 μs** | **2.015 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 336.22 μs | 11.61 μs | 0.636 μs |  1.08 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **323.31 μs** | **13.50 μs** | **0.740 μs** |  **1.00** |    **0.00** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 373.10 μs | 18.69 μs | 1.025 μs |  1.15 |    0.00 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **196.15 μs** | **21.28 μs** | **1.166 μs** |  **1.00** |    **0.01** | **1.2207** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 324.15 μs | 37.33 μs | 2.046 μs |  1.65 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **257.76 μs** | **16.92 μs** | **0.927 μs** |  **1.00** |    **0.00** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 356.90 μs | 26.25 μs | 1.439 μs |  1.38 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.98 μs** | **13.77 μs** | **0.755 μs** |  **1.00** |    **0.01** | **1.2207** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 320.41 μs | 12.42 μs | 0.681 μs |  3.56 |    0.03 | 0.9766 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **191.4 μs** |   **2.39 μs** |  **0.13 μs** |  **1.00** |   **0.7324** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   171.9 μs |   4.99 μs |  0.27 μs |  0.90 |   0.7324 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   468.1 μs |  11.64 μs |  0.64 μs |  2.45 |  12.2070 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,667.7 μs** |  **47.10 μs** |  **2.58 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,154.3 μs | 578.99 μs | 31.74 μs |  0.86 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,614.7 μs | 899.89 μs | 49.33 μs |  2.08 | 187.5000 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

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

| Method | Classes | Mean         | Error        | StdDev      | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|-------------:|------------:|-------:|-------:|----------:|
| **Report** | **10**      |     **562.4 ns** |      **6.75 ns** |     **0.37 ns** | **0.0677** |      **-** |   **1.66 KB** |
| **Report** | **1000**    | **358,244.3 ns** | **28,614.29 ns** | **1,568.45 ns** | **4.8828** | **0.9766** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

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

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 127.6 ms | 12.28 ms | 0.67 ms | 750.0000 | 750.0000 | 750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 117.2 ms | 11.91 ms | 0.65 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

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

| Method       | Shape     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|----------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **139.10 ms** |  **32.977 ms** |  **1.808 ms** |  **1.00** |    **0.02** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.41 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,165.29 ms | 232.454 ms | 12.742 ms | 15.57 |    0.19 | 12000.0000 | 8000.0000 | 7000.0000 | 156.29 MB |        5.31 |
|              |           |             |            |           |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **49.82 ms** |   **2.660 ms** |  **0.146 ms** |  **1.00** |    **0.00** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   755.04 ms |  46.610 ms |  2.555 ms | 15.15 |    0.06 |  4000.0000 | 4000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **188.02 ms** |  **18.597 ms** |  **1.019 ms** |  **1.00** |    **0.01** |  **1333.3333** |  **1333.3333** |  **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,262.36 ms | 499.676 ms | 27.389 ms | 33.31 |    0.20 | 29000.0000 | 14000.0000 | 11000.0000 | 531.88 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 4,248.94 ms | 289.660 ms | 15.877 ms | 22.60 |    0.13 | 15000.0000 |  9000.0000 |  7000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.45 ms** |   **7.758 ms** |  **0.425 ms** |  **1.00** |    **0.02** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   452.69 ms |  52.928 ms |  2.901 ms | 21.11 |    0.38 |  3000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   285.51 ms | 106.933 ms |  5.861 ms | 13.31 |    0.33 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean       | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  15.749 ms |  2.394 ms | 0.1312 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 118.473 ms | 27.128 ms | 1.4870 ms |  7.52 |    0.10 |
| Nmf_Transform                             |   3.781 ms |  1.154 ms | 0.0632 ms |  0.24 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  29.443 ms |  3.648 ms | 0.2000 ms |  1.87 |    0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

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

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **24,248.8 μs** |   **1,505.38 μs** |    **82.52 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **11692.44 KB** |
| TruncatedSvd_Transform     | 500     |     585.8 μs |      50.71 μs |     2.78 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 212,129.2 μs |  56,488.38 μs | 3,096.32 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18141.17 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 275,933.4 μs |  98,748.77 μs | 5,412.75 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **181,364.3 μs** |  **34,104.96 μs** | **1,869.41 μs** | **3333.3333** | **3333.3333** | **3333.3333** | **115064.01 KB** |
| TruncatedSvd_Transform     | 20000   |     969.6 μs |      82.99 μs |     4.55 μs |   39.0625 |   39.0625 |   39.0625 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 894,156.2 μs | 128,798.34 μs | 7,059.87 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.79 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 667,505.5 μs |  90,614.05 μs | 4,966.86 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 299.2 μs | 20.26 μs | 1.11 μs | 19.5313 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

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

| Method              | Rows   | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|-------------------- |------- |----------:|----------:|----------:|---------:|---------:|---------:|------------:|
| **FromBlockNormalized** | **1000**   |  **1.043 ms** | **0.1266 ms** | **0.0069 ms** | **248.0469** | **248.0469** | **248.0469** |  **1500.18 KB** |
| MmrSelect100        | 1000   |  9.061 ms | 1.3996 ms | 0.0767 ms |        - |        - |        - |    24.93 KB |
| **FromBlockNormalized** | **100000** | **90.417 ms** | **1.7650 ms** | **0.0967 ms** | **166.6667** | **166.6667** | **166.6667** | **150000.3 KB** |
| MmrSelect100        | 100000 |  8.963 ms | 0.0581 ms | 0.0032 ms |        - |        - |        - |    24.93 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

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

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **648.5 μs** |    **90.74 μs** |   **4.97 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    678.8 μs |    12.19 μs |   0.67 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    901.8 μs |   129.48 μs |   7.10 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,781.3 μs | 1,262.91 μs |  69.22 μs | 5.8594 |  342.49 KB |
| **FilteredTop10**   | **100000**  | **11,147.2 μs** | **1,461.82 μs** |  **80.13 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  | 13,360.9 μs | 7,159.10 μs | 392.41 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 13,955.9 μs | 1,885.62 μs | 103.36 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 22,928.3 μs | 1,972.72 μs | 108.13 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    91.77 ns |  10.53 ns |  0.577 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   757.72 ns |  22.40 ns |  1.228 ns |  8.26 |    0.05 |      - |         - |          NA |
| TokenSortRatio |   937.93 ns |  60.45 ns |  3.314 ns | 10.22 |    0.06 | 0.0439 |    1120 B |          NA |
| TokenSetRatio  |   988.49 ns | 125.22 ns |  6.864 ns | 10.77 |    0.09 | 0.0343 |     896 B |          NA |
| WRatio         | 1,316.80 ns | 224.30 ns | 12.295 ns | 14.35 |    0.14 | 0.0439 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error              | StdDev          | Ratio  | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------:|--------:|--------:|-------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,991,096.5599 ns |    137,618.0470 ns |   7,543.3096 ns |  1.000 |    0.00 |  3.9063 |      - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    693,584.5785 ns |     58,634.9603 ns |   3,213.9800 ns |  0.348 |    0.00 | 23.4375 | 7.8125 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,181,076.9212 ns |     52,381.1288 ns |   2,871.1864 ns |  0.593 |    0.00 | 27.3438 | 9.7656 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 81,327,900.3889 ns | 11,736,466.4318 ns | 643,315.3307 ns | 40.846 |    0.31 |       - |      - |  510843 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |      - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **89.99 ns** |   **6.021 ns** |  **0.330 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    251.99 ns |  56.142 ns |  3.077 ns |  2.80 |    0.03 | 0.0029 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **754.03 ns** |  **66.925 ns** |  **3.668 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,616.52 ns | 426.806 ns | 23.395 ns | 14.08 |    0.07 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,016.48 ns** | **246.901 ns** | **13.533 ns** |  **1.00** |    **0.02** | **0.0343** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,178.36 ns |  54.734 ns |  3.000 ns |  2.14 |    0.02 | 0.0763 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,269.70 ns** |  **59.649 ns** |  **3.270 ns** |  **1.00** |    **0.00** | **0.0439** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,291.81 ns | 163.527 ns |  8.963 ns |  4.17 |    0.01 | 0.1221 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

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

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.730 ms** | **0.5257 ms** | **0.0288 ms** | **267.5781** | **257.8125** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **27.946 ms** | **0.8896 ms** | **0.0488 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **29.74 ns** |   **7.497 ns** |  **0.411 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    38.37 ns |   6.674 ns |  0.366 ns |  1.29 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.91 ns |   5.779 ns |  0.317 ns |  0.94 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.19 ns |   0.707 ns |  0.039 ns |  0.88 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **27.70 ns** |   **0.524 ns** |  **0.029 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    38.98 ns |   0.966 ns |  0.053 ns |  1.41 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.20 ns |   8.148 ns |  0.447 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.50 ns |   5.039 ns |  0.276 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **31.51 ns** |   **0.911 ns** |  **0.050 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    38.93 ns |   5.822 ns |  0.319 ns |  1.24 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    30.84 ns |   2.056 ns |  0.113 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.01 ns |   5.436 ns |  0.298 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **35.51 ns** |   **5.462 ns** |  **0.299 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    42.55 ns |   2.738 ns |  0.150 ns |  1.20 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    36.08 ns |   0.953 ns |  0.052 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    32.76 ns |  10.590 ns |  0.580 ns |  0.92 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **51.51 ns** |  **14.422 ns** |  **0.791 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.48 ns |   0.694 ns |  0.038 ns |  1.17 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.30 ns |   9.626 ns |  0.528 ns |  1.11 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    52.29 ns |   1.997 ns |  0.109 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **63.85 ns** |   **4.464 ns** |  **0.245 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    68.96 ns |   1.339 ns |  0.073 ns |  1.08 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.87 ns |   6.843 ns |  0.375 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    57.40 ns |   0.405 ns |  0.022 ns |  0.90 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **401.61 ns** |   **4.601 ns** |  **0.252 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   412.59 ns |  11.377 ns |  0.624 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   402.13 ns |   8.540 ns |  0.468 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   398.39 ns |  25.551 ns |  1.401 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,919.93 ns** |  **24.350 ns** |  **1.335 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 5,114.19 ns |  73.302 ns |  4.018 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,963.03 ns | 282.569 ns | 15.489 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,981.03 ns | 513.847 ns | 28.166 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **407.4 ns** |    **28.33 ns** |   **1.55 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    189.5 ns |    32.36 ns |   1.77 ns |  0.47 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,338.1 ns** |   **613.17 ns** |  **33.61 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,846.6 ns |   358.43 ns |  19.65 ns |  1.22 |    0.02 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,457.5 ns** |    **96.56 ns** |   **5.29 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,489.3 ns | 6,103.50 ns | 334.55 ns |  2.34 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,859.8 ns** | **1,015.78 ns** |  **55.68 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,694.7 ns |   375.75 ns |  20.60 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,331.3 ns |   237.38 ns |  13.01 ns |  1.44 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,371.3 ns** |    **67.07 ns** |   **3.68 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,331.7 ns |   257.85 ns |  14.13 ns |  0.90 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 15,077.7 ns |   434.26 ns |  23.80 ns |  1.45 |    0.00 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **14,634.2 ns** |   **606.34 ns** |  **33.24 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,614.7 ns | 2,053.76 ns | 112.57 ns |  0.79 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 20,171.4 ns |   413.53 ns |  22.67 ns |  1.38 |    0.00 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,459.9 ns** |    **61.95 ns** |   **3.40 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,800.4 ns |   499.78 ns |  27.39 ns |  1.06 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,123.0 ns |   772.99 ns |  42.37 ns |  1.67 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **8,133.4 ns** |   **111.37 ns** |   **6.10 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,586.6 ns | 4,886.47 ns | 267.84 ns |  0.93 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 12,442.2 ns |   272.64 ns |  14.94 ns |  1.53 |    0.00 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **12,968.0 ns** |   **697.35 ns** |  **38.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,862.6 ns | 2,256.76 ns | 123.70 ns |  0.76 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 17,605.2 ns | 1,927.76 ns | 105.67 ns |  1.36 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **4,013.0 ns** |   **841.13 ns** |  **46.10 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,493.0 ns |   180.85 ns |   9.91 ns |  1.12 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,628.6 ns |   716.77 ns |  39.29 ns |  1.90 |    0.02 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **6,670.7 ns** |    **83.74 ns** |   **4.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,697.7 ns |   166.32 ns |   9.12 ns |  0.85 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 11,139.6 ns |   842.24 ns |  46.17 ns |  1.67 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **10,923.3 ns** |   **689.74 ns** |  **37.81 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  8,166.1 ns |   173.53 ns |   9.51 ns |  0.75 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 16,068.7 ns |   785.41 ns |  43.05 ns |  1.47 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,568.1 ns** |   **113.46 ns** |   **6.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,035.7 ns |    91.26 ns |   5.00 ns |  0.79 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,004.5 ns |   182.86 ns |  10.02 ns |  2.34 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **5,327.3 ns** |   **376.55 ns** |  **20.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,996.6 ns |   530.19 ns |  29.06 ns |  0.56 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  9,589.8 ns |   289.74 ns |  15.88 ns |  1.80 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **9,445.7 ns** |   **179.89 ns** |   **9.86 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  4,194.8 ns |   128.59 ns |   7.05 ns |  0.44 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 14,946.6 ns | 1,432.96 ns |  78.55 ns |  1.58 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **4,001.7 ns** |   **966.47 ns** |  **52.98 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,666.1 ns |    83.02 ns |   4.55 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,364.9 ns |   294.36 ns |  16.13 ns |  1.84 |    0.02 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **6,590.1 ns** |   **912.39 ns** |  **50.01 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,610.9 ns |   388.68 ns |  21.31 ns |  0.70 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 11,218.2 ns |   301.39 ns |  16.52 ns |  1.70 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **10,857.3 ns** | **1,112.99 ns** |  **61.01 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,354.7 ns |   105.59 ns |   5.79 ns |  0.77 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 16,248.6 ns |   433.22 ns |  23.75 ns |  1.50 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,331.0 ns** |   **362.57 ns** |  **19.87 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    512.8 ns |   166.70 ns |   9.14 ns |  0.39 |    0.01 | 0.0010 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,919.2 ns |   154.84 ns |   8.49 ns |  3.70 |    0.05 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **4,060.8 ns** |   **415.13 ns** |  **22.75 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,273.2 ns |   725.66 ns |  39.78 ns |  0.31 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  8,304.5 ns |   196.08 ns |  10.75 ns |  2.05 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **8,123.5 ns** | **1,913.03 ns** | **104.86 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,407.1 ns |   228.79 ns |  12.54 ns |  0.30 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 13,332.9 ns |   741.80 ns |  40.66 ns |  1.64 |    0.02 | 0.1984 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

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

| Method           | Shape       | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **6.283 ms** |   **0.8382 ms** |  **0.0459 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  41.614 ms |   6.2673 ms |  0.3435 ms |  6.62 |    0.06 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  65.733 ms |   1.9923 ms |  0.1092 ms | 10.46 |    0.07 |        - |        - |        - |   41.87 KB |       0.028 |
|                  |             |            |             |            |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |   **1.155 ms** |   **0.9740 ms** |  **0.0534 ms** |  **1.00** |    **0.06** |  **48.8281** |  **48.8281** |  **48.8281** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   5.040 ms |   0.0895 ms |  0.0049 ms |  4.37 |    0.17 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2.502 ms |   0.1880 ms |  0.0103 ms |  2.17 |    0.09 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |            |             |            |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **28.933 ms** |   **2.2140 ms** |  **0.1214 ms** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 462.346 ms |   9.8997 ms |  0.5426 ms | 15.98 |    0.06 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 524.367 ms | 189.5151 ms | 10.3880 ms | 18.12 |    0.32 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **6.216 ms** |  **0.3208 ms** | **0.0176 ms** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  17.615 ms |  0.0320 ms | 0.0018 ms |  2.83 |    0.01 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.222 ms** |  **2.1145 ms** | **0.1159 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 108.191 ms | 11.7171 ms | 0.6423 ms |  4.13 |    0.03 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **54.185 ms** | **10.3916 ms** | **0.5696 ms** |  **1.00** |    **0.01** | **200.0000** | **200.0000** | **200.0000** | **3946.85 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 222.909 ms | 33.6892 ms | 1.8466 ms |  4.11 |    0.05 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **125.23 ns** |     **5.400 ns** |   **0.296 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     56.35 ns |     7.346 ns |   0.403 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    123.45 ns |     3.409 ns |   0.187 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     97.28 ns |    64.370 ns |   3.528 ns |  0.78 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **210.86 ns** |    **20.124 ns** |   **1.103 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |     65.50 ns |     0.736 ns |   0.040 ns |  0.31 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    210.48 ns |    20.463 ns |   1.122 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    107.82 ns |    16.698 ns |   0.915 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **268.56 ns** |     **4.255 ns** |   **0.233 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 14   |     68.56 ns |     5.596 ns |   0.307 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    297.85 ns |    11.692 ns |   0.641 ns |  1.11 |    0.00 |         - |          NA |
| Kernel_Cjk | 14   |    113.71 ns |    38.593 ns |   2.115 ns |  0.42 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **379.95 ns** |     **7.862 ns** |   **0.431 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |     71.71 ns |     8.802 ns |   0.482 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    381.62 ns |    64.427 ns |   3.531 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    118.92 ns |     5.933 ns |   0.325 ns |  0.31 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **464.37 ns** |    **24.079 ns** |   **1.320 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.93 ns |     3.109 ns |   0.170 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    462.99 ns |     4.748 ns |   0.260 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    128.26 ns |    40.905 ns |   2.242 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **563.36 ns** |    **39.370 ns** |   **2.158 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     79.33 ns |     0.215 ns |   0.012 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    561.27 ns |     6.612 ns |   0.362 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    130.79 ns |     3.953 ns |   0.217 ns |  0.23 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **765.68 ns** |   **781.614 ns** |  **42.843 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 24   |     88.46 ns |     7.104 ns |   0.389 ns |  0.12 |    0.01 |         - |          NA |
| Dp_Cjk     | 24   |    747.72 ns |   115.991 ns |   6.358 ns |  0.98 |    0.05 |         - |          NA |
| Kernel_Cjk | 24   |    144.16 ns |    12.229 ns |   0.670 ns |  0.19 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,176.77 ns** |   **226.677 ns** |  **12.425 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    103.83 ns |    11.582 ns |   0.635 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,181.48 ns |   149.283 ns |   8.183 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    169.88 ns |    16.859 ns |   0.924 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **2,793.79 ns** |   **295.250 ns** |  **16.184 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    126.25 ns |    16.925 ns |   0.928 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,667.54 ns | 1,668.164 ns |  91.438 ns |  0.95 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    234.47 ns |    11.952 ns |   0.655 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **4,969.27 ns** |   **774.596 ns** |  **42.458 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    153.18 ns |     4.212 ns |   0.231 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  4,953.47 ns |   541.020 ns |  29.655 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    291.83 ns |    20.511 ns |   1.124 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **10,128.33 ns** | **1,993.958 ns** | **109.296 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    346.57 ns |    11.008 ns |   0.603 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,175.78 ns | 1,328.081 ns |  72.797 ns |  1.10 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    892.43 ns |    10.727 ns |   0.588 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **28.57 ns** |      **2.163 ns** |   **0.119 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     25.99 ns |      0.489 ns |   0.027 ns |  0.91 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    135.72 ns |      2.549 ns |   0.140 ns |  4.75 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     27.12 ns |      4.640 ns |   0.254 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **265.40 ns** |     **10.581 ns** |   **0.580 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    362.51 ns |      9.626 ns |   0.528 ns |  1.37 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    719.04 ns |     59.233 ns |   3.247 ns |  2.71 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    270.81 ns |     33.955 ns |   1.861 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **956.75 ns** |     **13.069 ns** |   **0.716 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,870.52 ns |     79.700 ns |   4.369 ns |  1.96 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,720.48 ns |    133.002 ns |   7.290 ns |  1.80 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    967.31 ns |    249.214 ns |  13.660 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **12,340.16 ns** |     **52.398 ns** |   **2.872 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 19,126.03 ns | 10,884.014 ns | 596.590 ns |  1.55 |    0.04 |         - |          NA |
| Distance_CodePoint         | 512    | 15,077.77 ns |  1,692.935 ns |  92.795 ns |  1.22 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 12,436.43 ns |    710.400 ns |  38.939 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.93 ns** |       **0.728 ns** |      **0.040 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      86.88 ns |       0.456 ns |      0.025 ns |  3.35 |    0.00 | 0.0021 |      56 B |          NA |
| Quickenshtein        | 8      |      88.03 ns |      12.028 ns |      0.659 ns |  3.39 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     170.94 ns |      22.292 ns |      1.222 ns |  6.59 |    0.04 | 0.0050 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **266.42 ns** |      **59.414 ns** |      **3.257 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,087.39 ns |     727.621 ns |     39.883 ns | 19.10 |    0.24 | 0.0076 |     280 B |          NA |
| Quickenshtein        | 64     |   1,199.10 ns |     222.869 ns |     12.216 ns |  4.50 |    0.06 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   8,978.53 ns |     185.627 ns |     10.175 ns | 33.70 |    0.36 | 0.0153 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |  **12,434.03 ns** |   **1,040.596 ns** |     **57.039 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 434,855.86 ns |  15,594.691 ns |    854.798 ns | 34.97 |    0.15 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,661.58 ns |   4,155.103 ns |    227.755 ns |  3.03 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 791,710.68 ns | 826,091.586 ns | 45,280.868 ns | 63.67 |    3.16 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

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

| Method                          | Shape   | Mean            | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |----------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |       **146.35 μs** |       **8.433 μs** |      **0.462 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |   319,641.66 μs | 101,128.373 μs |  5,543.187 μs | 2,184.13 |   33.34 | 500.0000 | 500.0000 | 500.0000 | 31412.47 KB |   12,805.08 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |     **3,343.90 μs** |     **201.152 μs** |     **11.026 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 1,472,219.04 μs | 453,996.658 μs | 24,885.089 μs |   440.27 |    6.57 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |        **24.34 μs** |       **3.272 μs** |      **0.179 μs** |     **1.00** |    **0.01** |   **0.0916** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     1,003.85 μs |      27.257 μs |      1.494 μs |    41.25 |    0.27 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **10,155.79 μs** |  **1,085.865 μs** |    **59.520 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  33,016.20 μs |  2,302.378 μs |   126.201 μs |     3.25 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **16.41 μs** |      **0.532 μs** |     **0.029 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  33,819.95 μs |  1,099.386 μs |    60.261 μs | 2,060.63 |    4.49 | 600.0000 | 600.0000 | 600.0000 |  5090213 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **171,188.97 μs** | **84,861.051 μs** | **4,651.521 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 212,481.99 μs | 21,910.246 μs | 1,200.974 μs |     1.24 |    0.03 |        - |        - |        - | 23228972 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **237.01 μs** |      **5.101 μs** |     **0.280 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 211,040.29 μs | 34,024.548 μs | 1,865.000 μs |   890.42 |    6.87 |        - |        - |        - | 23228008 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

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

| Method    | Samples | Mean        | Error     | StdDev    | Allocated |
|---------- |-------- |------------:|----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **49.85 ms** |  **12.96 ms** |  **0.710 ms** |         **-** |
| OneVsOne  | 100000  |    87.97 ms |  29.29 ms |  1.605 ms |  803531 B |
| **OneVsRest** | **1000000** |   **846.74 ms** | **136.03 ms** |  **7.456 ms** |         **-** |
| OneVsOne  | 1000000 | 1,255.41 ms | 536.27 ms | 29.395 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

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

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|------------:|
| PerLabel         | 100000 |  7.044 ms |  0.3609 ms | 0.0198 ms |         - |         - |        - |     7.84 KB |
| PerLabelWeighted | 100000 | 10.638 ms |  0.8929 ms | 0.0489 ms |         - |         - |        - |     7.85 KB |
| PerSample        | 100000 | 75.120 ms | 19.9700 ms | 1.0946 ms | 2000.0000 | 1857.1429 | 571.4286 | 39063.01 KB |
| PerClass         | 100000 |  4.136 ms |  0.5827 ms | 0.0319 ms |         - |         - |        - |     7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

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

| Method                | RowCount | BatchCount | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-------------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **267.61 μs** |     **7.083 μs** |   **0.388 μs** |  **1.00** |    **0.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   282.35 μs |    29.832 μs |   1.635 μs |  1.06 |    0.01 |      - |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    95.68 μs |     1.892 μs |   0.104 μs |  0.36 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   269.07 μs |     8.590 μs |   0.471 μs |  1.01 |    0.00 |      - |     800 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **273.01 μs** |   **183.688 μs** |  **10.069 μs** |  **1.00** |    **0.04** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   318.35 μs |     6.706 μs |   0.368 μs |  1.17 |    0.04 | 2.9297 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    96.38 μs |     2.097 μs |   0.115 μs |  0.35 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   273.23 μs |     5.357 μs |   0.294 μs |  1.00 |    0.03 |      - |     800 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,756.36 μs** |   **147.451 μs** |   **8.082 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 3,105.35 μs |   252.720 μs |  13.852 μs |  1.13 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,329.69 μs |   191.836 μs |  10.515 μs |  0.48 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 2,798.70 μs |   481.991 μs |  26.420 μs |  1.02 |    0.01 |      - |     803 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,767.88 μs** |   **270.857 μs** |  **14.847 μs** |  **1.00** |    **0.01** |      **-** |     **802 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 3,151.81 μs | 2,483.356 μs | 136.121 μs |  1.14 |    0.04 |      - |   77627 B |       96.79 |
| SparseFit             | 100000   | 100        | 1,316.87 μs |   106.850 μs |   5.857 μs |  0.48 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 2,790.33 μs |   410.728 μs |  22.513 μs |  1.01 |    0.01 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

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

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 10.23 ms | 3.494 ms | 0.191 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 15.29 ms | 3.819 ms | 0.209 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

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

| Method             | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------------------- |---------:|----------:|---------:|---------:|----------:|
| NormalizeDocuments | 14.61 ms |  0.323 ms | 0.018 ms | 109.3750 |   2.75 MB |
| EncodeDocuments    | 71.52 ms | 11.175 ms | 0.613 ms | 375.0000 |  10.74 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **11,004.34 μs** | **4,273.875 μs** | **234.266 μs** |  **1.00** |    **0.03** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 18,359.11 μs | 4,253.165 μs | 233.130 μs |  1.67 |    0.04 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **149.28 μs** |     **9.747 μs** |   **0.534 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    230.38 μs |     3.643 μs |   0.200 μs |  1.54 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,394.92 μs** |   **259.484 μs** |  **14.223 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,209.41 μs |    65.592 μs |   3.595 μs |  0.95 |    0.00 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.80 μs** |     **3.336 μs** |   **0.183 μs** |  **1.00** |    **0.01** |   **0.0916** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     30.73 μs |     2.077 μs |   0.114 μs |  1.29 |    0.01 |   0.0610 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **7.293 ms** | **0.3213 ms** | **0.0176 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 7.098 ms | 1.0437 ms | 0.0572 ms |  0.97 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **8.208 ms** | **1.5779 ms** | **0.0865 ms** |  **1.00** |     **220 B** |        **1.00** |
| ExtractOne | 5     | 7.104 ms | 0.7397 ms | 0.0405 ms |  0.87 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

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

| Method                            | Rows   | Mean      | Error      | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|-----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 32.823 ms |  5.5594 ms | 0.3047 ms |       - |       - |       - |  800342 B |
| NdcgIgnoringTies                  | 100000 | 29.749 ms |  0.5322 ms | 0.0292 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 19.508 ms | 12.6956 ms | 0.6959 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 15.235 ms |  0.9328 ms | 0.0511 ms |       - |       - |       - |     180 B |
| CoverageErrorScore                | 100000 |  6.478 ms |  0.6804 ms | 0.0373 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 53.235 ms |  0.6748 ms | 0.0370 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

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

| Method          | Model   | Mean     | Error   | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|--------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **147.4 ms** | **6.55 ms** | **0.36 ms** | **1500.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **141.5 ms** | **7.07 ms** | **0.39 ms** | **1500.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **19.51 ms** |   **4.084 ms** | **0.224 ms** |  **1.00** |    **0.01** |  **343.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  54.27 ms |  14.363 ms | 0.787 ms |  2.78 |    0.04 |  100.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **55.09 ms** |  **16.452 ms** | **0.902 ms** |  **1.00** |    **0.02** |  **300.0000** |   **7.98 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  74.18 ms |  77.851 ms | 4.267 ms |  1.35 |    0.07 |  222.2222 |   5.65 MB |        0.71 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **88.43 ms** |   **0.830 ms** | **0.045 ms** |  **1.00** |    **0.00** | **1166.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 275.32 ms | 154.775 ms | 8.484 ms |  3.11 |    0.08 | 2000.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-CZMXIK : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **1000**    |     **75.37 μs** |     **1.035 μs** |   **1.151 μs** | **0.1221** |   **3.12 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 1000    |     95.48 μs |     0.704 μs |   0.810 μs | 0.7324 |  20.55 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 1000    |     88.74 μs |     0.149 μs |   0.153 μs | 0.6104 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     76.99 μs |    14.010 μs |   0.768 μs | 0.1221 |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |    104.07 μs |     2.014 μs |   0.110 μs | 0.7324 |  20.55 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     90.01 μs |    12.490 μs |   0.685 μs | 0.6104 |  15.52 KB |
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **10000**   |    **686.88 μs** |     **2.160 μs** |   **2.121 μs** |      **-** |   **3.12 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 10000   |  1,095.69 μs |    26.080 μs |  30.034 μs |      - |  26.13 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 10000   |  1,065.85 μs |     9.446 μs |   9.277 μs |      - |  21.15 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    667.35 μs |   184.525 μs |  10.114 μs |      - |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |  1,246.07 μs |   789.927 μs |  43.299 μs |      - |  26.13 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |  1,111.43 μs |   250.434 μs |  13.727 μs |      - |  21.15 KB |
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **100000**  | **12,916.27 μs** |   **164.961 μs** | **189.969 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 100000  | 17,404.09 μs |   176.540 μs | 203.304 μs |      - |  45.99 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 100000  | 17,070.23 μs |   118.007 μs | 131.165 μs |      - |  41.05 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  | 12,884.17 μs | 3,253.045 μs | 178.310 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 17,956.78 μs | 2,724.424 μs | 149.335 μs |      - |     46 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 17,621.79 μs | 5,784.340 μs | 317.059 μs |      - |  41.06 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.925 ms** |   **0.3273 ms** |  **0.0179 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  55.795 ms |  41.5075 ms |  2.2752 ms | 14.22 |    0.51 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **18.175 ms** |   **2.7121 ms** |  **0.1487 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 389.049 ms | 476.1196 ms | 26.0977 ms | 21.41 |    1.25 | 72000.0000 | 72000.0000 | 72000.0000 | 324.38 MB |       40.00 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `levenshtein`
- `indel`
- `metrics`
- `persistence`
- `stats`
- `splitters`
- `transformers`
- `cdist`

### compare-cdist

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.162 | 0.028 | 0.17x | 0.162 | 0.028 | 0.17x |
| cdist_wratio_n50 | 2.481 | 2.248 | 0.91x | 2.481 | 2.248 | 0.91x |
| cdist_ratio_varied_n50 | 0.138 | 0.023 | 0.17x | 0.138 | 0.023 | 0.17x |
| cdist_ratio_varied_cutoff90_n50 | 0.054 | 0.023 | 0.43x | 0.054 | 0.023 | 0.43x |
| cdist_ratio_n200 | 2.878 | 0.324 | 0.11x | 2.882 | 0.324 | 0.11x |
| cdist_wratio_n200 | 37.061 | 33.514 | 0.90x | 37.063 | 33.512 | 0.90x |
| cdist_ratio_varied_n200 | 2.499 | 0.263 | 0.11x | 2.503 | 0.263 | 0.10x |
| cdist_ratio_varied_cutoff90_n200 | 0.996 | 0.263 | 0.26x | 0.998 | 0.263 | 0.26x |
| cdist_ratio_n500 | 17.221 | 1.932 | 0.11x | 17.248 | 1.932 | 0.11x |
| cdist_wratio_n500 | 215.057 | 200.785 | 0.93x | 215.175 | 200.780 | 0.93x |
| cdist_ratio_varied_n500 | 18.961 | 1.533 | 0.08x | 19.039 | 1.533 | 0.08x |
| cdist_ratio_varied_cutoff90_n500 | 5.783 | 1.533 | 0.27x | 5.865 | 1.533 | 0.26x |

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
| latin | 8 | 106.4 | 22.0 | 4.83x C# faster |
| latin | 32 | 147.5 | 87.7 | 1.68x C# faster |
| latin | 128 | 472.3 | 415.7 | 1.14x C# faster |
| latin | 512 | 4801.2 | 5122.6 | 1.07x Py faster |
| cjk | 8 | 123.0 | 21.9 | 5.61x C# faster |
| cjk | 32 | 274.5 | 222.1 | 1.24x C# faster |
| cjk | 128 | 1788.0 | 1308.8 | 1.37x C# faster |
| cjk | 512 | 16352.9 | 8429.5 | 1.94x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 127.8 | 19.1 | 6.71x C# faster |
| latin | 32 | 220.1 | 153.8 | 1.43x C# faster |
| latin | 128 | 1826.5 | 921.3 | 1.98x C# faster |
| latin | 512 | 15567.7 | 12859.3 | 1.21x C# faster |
| cjk | 8 | 135.9 | 19.0 | 7.16x C# faster |
| cjk | 32 | 352.8 | 288.9 | 1.22x C# faster |
| cjk | 128 | 2980.5 | 2162.5 | 1.38x C# faster |
| cjk | 512 | 26224.4 | 21475.0 | 1.22x C# faster |

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
| confusion_matrix_n1000_k2 | 0.003 | 0.815 | 240.59x | 0.003 | 0.815 | 240.59x |
| accuracy_n1000_k2 | 0.000 | 0.426 | 2606.24x | 0.000 | 0.426 | 2606.07x |
| precision_recall_f1_macro_n1000_k2 | 0.004 | 1.518 | 405.23x | 0.004 | 1.518 | 405.23x |
| classification_report_n1000_k2 | 0.006 | 5.896 | 930.27x | 0.006 | 5.896 | 930.27x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.636 | 93.39x | 0.018 | 1.636 | 93.39x |
| balanced_accuracy_n1000_k2 | 0.004 | 0.875 | 246.32x | 0.004 | 0.874 | 246.31x |
| matthews_n1000_k2 | 0.004 | 1.674 | 473.73x | 0.004 | 1.674 | 473.70x |
| cohen_kappa_n1000_k2 | 0.004 | 0.918 | 258.65x | 0.004 | 0.917 | 258.65x |
| mse_n1000_k2 | 0.001 | 0.230 | 407.07x | 0.001 | 0.230 | 407.08x |
| mae_n1000_k2 | 0.001 | 0.230 | 421.84x | 0.001 | 0.230 | 421.82x |
| median_ae_n1000_k2 | 0.005 | 0.244 | 45.50x | 0.005 | 0.244 | 45.50x |
| r2_n1000_k2 | 0.001 | 0.282 | 193.47x | 0.001 | 0.282 | 193.46x |
| confusion_matrix_n1000_k10 | 0.004 | 0.817 | 227.94x | 0.004 | 0.817 | 227.94x |
| accuracy_n1000_k10 | 0.000 | 0.428 | 2618.51x | 0.000 | 0.428 | 2618.54x |
| precision_recall_f1_macro_n1000_k10 | 0.004 | 1.516 | 352.89x | 0.004 | 1.516 | 352.90x |
| classification_report_n1000_k10 | 0.011 | 6.045 | 569.42x | 0.011 | 6.045 | 569.45x |
| roc_auc_ovr_macro_n1000_k10 | 0.488 | 8.482 | 17.37x | 0.488 | 8.481 | 17.37x |
| balanced_accuracy_n1000_k10 | 0.004 | 0.886 | 219.97x | 0.004 | 0.886 | 219.97x |
| matthews_n1000_k10 | 0.004 | 1.711 | 428.31x | 0.004 | 1.711 | 428.29x |
| cohen_kappa_n1000_k10 | 0.004 | 0.929 | 213.34x | 0.004 | 0.929 | 213.33x |
| mse_n1000_k10 | 0.001 | 0.231 | 407.43x | 0.001 | 0.231 | 407.39x |
| mae_n1000_k10 | 0.001 | 0.229 | 420.27x | 0.001 | 0.229 | 420.25x |
| median_ae_n1000_k10 | 0.005 | 0.244 | 45.23x | 0.005 | 0.244 | 45.23x |
| r2_n1000_k10 | 0.001 | 0.280 | 192.59x | 0.001 | 0.280 | 192.59x |
| confusion_matrix_n100000_k2 | 0.321 | 10.979 | 34.24x | 0.321 | 10.979 | 34.24x |
| accuracy_n100000_k2 | 0.017 | 3.762 | 221.55x | 0.017 | 3.762 | 221.53x |
| precision_recall_f1_macro_n100000_k2 | 0.331 | 12.732 | 38.49x | 0.331 | 12.732 | 38.49x |
| classification_report_n100000_k2 | 0.333 | 27.532 | 82.70x | 0.333 | 27.531 | 82.70x |
| roc_auc_binary_n100000_k2 | 4.904 | 26.239 | 5.35x | 4.904 | 26.236 | 5.35x |
| balanced_accuracy_n100000_k2 | 0.331 | 11.110 | 33.62x | 0.331 | 11.110 | 33.61x |
| matthews_n100000_k2 | 0.330 | 23.009 | 69.66x | 0.330 | 23.009 | 69.64x |
| cohen_kappa_n100000_k2 | 0.321 | 11.280 | 35.17x | 0.321 | 11.279 | 35.17x |
| mse_n100000_k2 | 0.056 | 0.485 | 8.65x | 0.056 | 0.485 | 8.65x |
| mae_n100000_k2 | 0.055 | 0.505 | 9.26x | 0.055 | 0.505 | 9.26x |
| median_ae_n100000_k2 | 0.640 | 1.684 | 2.63x | 0.658 | 1.684 | 2.56x |
| r2_n100000_k2 | 0.141 | 0.737 | 5.21x | 0.141 | 0.737 | 5.21x |
| confusion_matrix_n100000_k10 | 0.330 | 11.062 | 33.49x | 0.330 | 11.059 | 33.48x |
| accuracy_n100000_k10 | 0.017 | 3.792 | 223.20x | 0.017 | 3.792 | 223.20x |
| precision_recall_f1_macro_n100000_k10 | 0.331 | 13.193 | 39.80x | 0.331 | 13.193 | 39.80x |
| classification_report_n100000_k10 | 0.337 | 29.789 | 88.38x | 0.337 | 29.788 | 88.37x |
| roc_auc_ovr_macro_n100000_k10 | 41.408 | 214.016 | 5.17x | 41.409 | 214.005 | 5.17x |
| balanced_accuracy_n100000_k10 | 0.330 | 11.205 | 33.94x | 0.330 | 11.204 | 33.94x |
| matthews_n100000_k10 | 0.332 | 22.861 | 68.86x | 0.332 | 22.859 | 68.86x |
| cohen_kappa_n100000_k10 | 0.323 | 11.304 | 35.01x | 0.323 | 11.304 | 35.01x |
| mse_n100000_k10 | 0.056 | 0.484 | 8.69x | 0.056 | 0.484 | 8.69x |
| mae_n100000_k10 | 0.054 | 0.506 | 9.34x | 0.054 | 0.506 | 9.34x |
| median_ae_n100000_k10 | 0.681 | 1.677 | 2.46x | 0.745 | 1.677 | 2.25x |
| r2_n100000_k10 | 0.140 | 0.731 | 5.23x | 0.140 | 0.731 | 5.23x |
| confusion_matrix_n1000000_k2 | 3.258 | 104.896 | 32.20x | 3.258 | 104.872 | 32.19x |
| accuracy_n1000000_k2 | 0.253 | 34.771 | 137.69x | 0.253 | 34.760 | 137.65x |
| precision_recall_f1_macro_n1000000_k2 | 3.259 | 114.579 | 35.16x | 3.259 | 114.561 | 35.16x |
| classification_report_n1000000_k2 | 3.262 | 231.101 | 70.85x | 3.262 | 231.097 | 70.85x |
| roc_auc_binary_n1000000_k2 | 76.799 | 295.332 | 3.85x | 76.796 | 295.318 | 3.85x |
| balanced_accuracy_n1000000_k2 | 3.254 | 106.207 | 32.64x | 3.254 | 106.201 | 32.64x |
| matthews_n1000000_k2 | 3.251 | 213.479 | 65.66x | 3.251 | 213.466 | 65.66x |
| cohen_kappa_n1000000_k2 | 3.233 | 106.641 | 32.98x | 3.233 | 106.640 | 32.99x |
| mse_n1000000_k2 | 0.567 | 2.986 | 5.27x | 0.567 | 2.986 | 5.27x |
| mae_n1000000_k2 | 0.553 | 4.095 | 7.41x | 0.553 | 4.095 | 7.41x |
| median_ae_n1000000_k2 | 7.000 | 15.389 | 2.20x | 7.128 | 15.388 | 2.16x |
| r2_n1000000_k2 | 1.506 | 4.868 | 3.23x | 1.506 | 4.868 | 3.23x |
| confusion_matrix_n1000000_k10 | 3.249 | 105.904 | 32.60x | 3.249 | 105.898 | 32.60x |
| accuracy_n1000000_k10 | 0.230 | 34.983 | 151.88x | 0.230 | 34.980 | 151.88x |
| precision_recall_f1_macro_n1000000_k10 | 3.261 | 121.999 | 37.42x | 3.260 | 121.992 | 37.42x |
| classification_report_n1000000_k10 | 3.264 | 251.285 | 76.99x | 3.264 | 251.280 | 76.99x |
| balanced_accuracy_n1000000_k10 | 3.250 | 105.787 | 32.55x | 3.249 | 105.778 | 32.55x |
| matthews_n1000000_k10 | 3.259 | 218.005 | 66.90x | 3.259 | 217.994 | 66.90x |
| cohen_kappa_n1000000_k10 | 3.256 | 105.473 | 32.40x | 3.256 | 105.471 | 32.40x |
| mse_n1000000_k10 | 0.564 | 2.794 | 4.96x | 0.564 | 2.794 | 4.96x |
| mae_n1000000_k10 | 0.559 | 4.317 | 7.73x | 0.559 | 4.317 | 7.72x |
| median_ae_n1000000_k10 | 7.735 | 15.587 | 2.02x | 7.997 | 15.586 | 1.95x |
| r2_n1000000_k10 | 1.420 | 5.414 | 3.81x | 1.419 | 5.413 | 3.81x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.3', 'sentencepiece': '0.2.3', 'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.541 | 9.615 | 2.12x | 4.697 | 9.614 | 2.05x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.596 | 16.707 | 1.33x | 12.963 | 16.706 | 1.29x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.385 | 51.060 | 4.12x | 12.509 | 51.054 | 4.08x | 1,990,038 | 1,990,038 |
| spiece_model | 6.358 | 15.608 | 2.45x | 6.591 | 15.607 | 2.37x | 347,061 | 347,061 |
| tfidf_save | 1.761 | 2.610 | 1.48x | 1.770 | 2.610 | 1.47x | 581,787 | 591,922 |
| tfidf_load | 4.749 | 3.908 | 0.82x | 4.914 | 3.907 | 0.80x | 581,787 | 591,922 |
| embedding_index_save | 6.245 | 2.573 | 0.41x | 6.591 | 2.572 | 0.39x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.478 | 37.329 | 0.75x | 10.144 | 5.378 | 0.53x | 20,589,007 | 15,360,128 |
| embedding_index_load | 9.729 | 1.957 | 0.20x | 10.358 | 1.956 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 9.439 | 1.359 | 0.14x | 10.077 | 1.359 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 7.530 | 1.906 | 0.25x | 7.959 | 1.903 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 4.119 | 1.818 | 0.44x | 4.217 | 1.817 | 0.43x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 76.82x | 0.000 | 0.001 | 76.82x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 428.062 | 547.478 | 1.28x | 428.031 | 547.440 | 1.28x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 72.885 | 70.033 | 0.96x | 72.882 | 70.032 | 0.96x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.106 | 0.110 | 1.04x | 0.106 | 0.110 | 1.04x |
| stratified_n10000 | 0.175 | 0.803 | 4.60x | 0.175 | 0.803 | 4.60x |
| traintest_n10000 | 0.005 | 0.189 | 35.60x | 0.005 | 0.189 | 35.59x |
| kfold_seeded_n10000 | 0.325 | 0.676 | 2.08x | 0.325 | 0.676 | 2.08x |
| stratified_seeded_n10000 | 0.405 | 1.466 | 3.62x | 0.405 | 1.466 | 3.62x |
| traintest_seeded_n10000 | 0.159 | 0.451 | 2.83x | 0.159 | 0.451 | 2.83x |
| stratified_traintest_n10000 | 0.678 | 2.543 | 3.75x | 0.678 | 2.543 | 3.75x |
| group_kfold_n10000 | 0.136 | 0.568 | 4.17x | 0.136 | 0.568 | 4.17x |
| stratified_group_kfold_n10000 | 0.242 | 25.782 | 106.45x | 0.242 | 25.780 | 106.44x |
| timeseries_n10000 | 0.000 | 0.012 | 89.97x | 0.000 | 0.012 | 89.97x |
| repeated_kfold_n10000 | 1.019 | 1.859 | 1.82x | 1.019 | 1.859 | 1.82x |
| kfold_n100000 | 1.231 | 2.284 | 1.86x | 1.329 | 2.284 | 1.72x |
| stratified_n100000 | 1.895 | 7.565 | 3.99x | 1.992 | 7.561 | 3.80x |
| traintest_n100000 | 0.153 | 0.438 | 2.87x | 0.154 | 0.438 | 2.85x |
| kfold_seeded_n100000 | 3.720 | 7.419 | 1.99x | 3.832 | 7.411 | 1.93x |
| stratified_seeded_n100000 | 4.252 | 13.464 | 3.17x | 4.388 | 13.457 | 3.07x |
| traintest_seeded_n100000 | 2.052 | 1.860 | 0.91x | 2.084 | 1.860 | 0.89x |
| stratified_traintest_n100000 | 8.314 | 22.965 | 2.76x | 8.379 | 22.963 | 2.74x |
| group_kfold_n100000 | 1.558 | 6.726 | 4.32x | 1.654 | 6.712 | 4.06x |
| stratified_group_kfold_n100000 | 2.555 | 261.220 | 102.25x | 2.654 | 261.215 | 98.42x |
| timeseries_n100000 | 0.000 | 0.045 | 353.31x | 0.000 | 0.045 | 353.31x |
| repeated_kfold_n100000 | 10.625 | 21.652 | 2.04x | 10.801 | 21.651 | 2.00x |
| kfold_n1000000 | 12.918 | 23.625 | 1.83x | 13.644 | 23.585 | 1.73x |
| stratified_n1000000 | 21.026 | 79.599 | 3.79x | 21.903 | 79.594 | 3.63x |
| traintest_n1000000 | 0.604 | 2.644 | 4.38x | 0.664 | 2.644 | 3.98x |
| kfold_seeded_n1000000 | 40.734 | 68.328 | 1.68x | 41.287 | 68.325 | 1.65x |
| stratified_seeded_n1000000 | 46.299 | 124.428 | 2.69x | 47.466 | 124.418 | 2.62x |
| traintest_seeded_n1000000 | 21.881 | 20.443 | 0.93x | 22.244 | 20.439 | 0.92x |
| stratified_traintest_n1000000 | 96.652 | 236.150 | 2.44x | 96.941 | 236.138 | 2.44x |
| group_kfold_n1000000 | 15.348 | 63.746 | 4.15x | 15.819 | 63.741 | 4.03x |
| stratified_group_kfold_n1000000 | 28.472 | 2663.355 | 93.54x | 29.685 | 2663.204 | 89.72x |
| timeseries_n1000000 | 0.000 | 0.378 | 3121.64x | 0.000 | 0.377 | 3119.15x |
| repeated_kfold_n1000000 | 116.769 | 197.645 | 1.69x | 118.451 | 197.631 | 1.67x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.009 | 0.641 | 72.06x | 0.009 | 0.641 | 72.06x |
| mann_whitney_n1000 | 0.050 | 0.625 | 12.62x | 0.050 | 0.625 | 12.62x |
| chi_square_n1000 | 0.000 | 0.248 | 1342.33x | 0.000 | 0.248 | 1342.21x |
| fligner_n1000 | 0.126 | 0.704 | 5.58x | 0.126 | 0.704 | 5.58x |
| anderson_ksamp_n1000 | 0.201 | 0.427 | 2.12x | 0.201 | 0.427 | 2.12x |
| spearman_matrix_n1000 | 0.187 | 0.723 | 3.87x | 0.187 | 0.723 | 3.87x |
| pointbiserial_n1000 | 0.010 | 0.463 | 45.32x | 0.010 | 0.463 | 45.32x |
| welch_t_n10000 | 0.066 | 0.694 | 10.50x | 0.066 | 0.694 | 10.50x |
| mann_whitney_n10000 | 0.535 | 2.483 | 4.64x | 0.535 | 2.483 | 4.64x |
| chi_square_n10000 | 0.001 | 0.252 | 212.39x | 0.001 | 0.252 | 212.37x |
| fligner_n10000 | 1.378 | 2.973 | 2.16x | 1.433 | 2.973 | 2.07x |
| anderson_ksamp_n10000 | 2.103 | 4.065 | 1.93x | 2.188 | 4.065 | 1.86x |
| spearman_matrix_n10000 | 2.227 | 4.168 | 1.87x | 2.270 | 4.168 | 1.84x |
| pointbiserial_n10000 | 0.117 | 0.549 | 4.70x | 0.117 | 0.549 | 4.70x |
| welch_t_n100000 | 0.643 | 1.329 | 2.07x | 0.643 | 1.329 | 2.07x |
| mann_whitney_n100000 | 6.039 | 24.584 | 4.07x | 6.039 | 24.584 | 4.07x |
| chi_square_n100000 | 0.012 | 0.272 | 22.74x | 0.012 | 0.272 | 22.74x |
| fligner_n100000 | 16.949 | 29.694 | 1.75x | 17.336 | 29.692 | 1.71x |
| anderson_ksamp_n100000 | 17.770 | 42.620 | 2.40x | 18.268 | 42.616 | 2.33x |
| spearman_matrix_n100000 | 20.933 | 45.517 | 2.17x | 21.371 | 45.516 | 2.13x |
| pointbiserial_n100000 | 1.355 | 1.774 | 1.31x | 1.369 | 7.075 | 5.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.210 | 0.401 | 1.91x | 0.211 | 0.401 | 1.90x |
| polynomial_n10000 | 0.616 | 0.864 | 1.40x | 0.619 | 0.863 | 1.39x |
| kbins_fit_n10000 | 1.472 | 2.195 | 1.49x | 1.471 | 2.195 | 1.49x |
| kbins_n10000 | 0.495 | 3.863 | 7.81x | 0.574 | 3.861 | 6.72x |
| quantile_fit_n10000 | 1.580 | 3.537 | 2.24x | 1.580 | 3.537 | 2.24x |
| quantile_n10000 | 2.098 | 1.779 | 0.85x | 2.102 | 1.778 | 0.85x |
| power_fit_n10000 | 5.944 | 88.306 | 14.86x | 5.946 | 88.301 | 14.85x |
| power_n10000 | 1.605 | 1.072 | 0.67x | 1.608 | 1.072 | 0.67x |
| knn_impute_n10000 | 5.846 | 29.944 | 5.12x | 5.846 | 118.434 | 20.26x |
| label_n10000 | 1.287 | 0.802 | 0.62x | 1.287 | 0.802 | 0.62x |
| normalize_n100000 | 1.690 | 1.922 | 1.14x | 2.080 | 1.922 | 0.92x |
| polynomial_n100000 | 4.251 | 7.041 | 1.66x | 4.996 | 7.040 | 1.41x |
| kbins_fit_n100000 | 14.398 | 8.363 | 0.58x | 15.292 | 8.363 | 0.55x |
| kbins_n100000 | 5.544 | 16.545 | 2.98x | 6.344 | 16.545 | 2.61x |
| quantile_fit_n100000 | 14.606 | 16.911 | 1.16x | 15.496 | 16.910 | 1.09x |
| quantile_n100000 | 19.095 | 17.420 | 0.91x | 19.690 | 17.419 | 0.88x |
| power_fit_n100000 | 65.133 | 668.468 | 10.26x | 68.587 | 668.468 | 9.75x |
| power_n100000 | 14.347 | 8.056 | 0.56x | 14.943 | 8.056 | 0.54x |
| knn_impute_n100000 | 5.841 | 26.614 | 4.56x | 5.841 | 105.230 | 18.02x |
| label_n100000 | 16.248 | 8.020 | 0.49x | 16.706 | 8.019 | 0.48x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `ArtifactLoadOnlyBenchmarks`
- `ArtifactTimeBenchmarks`
- `AsyncArtifactBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `Bm25Benchmarks`
- `ChainedProductBenchmarks`
- `ClusteringAgreementBenchmarks`
- `CoxBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `DistributionIncumbentBenchmarks`
- `DistributionTailBenchmarks`
- `EmbeddingSearchBenchmarks`
- `EncoderIncumbentBenchmarks`
- `GlmBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `KsExactTableBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `MannWhitneyExactBenchmarks`
- `ManyLabelsMetricsBenchmarks`
- `MathNetInteropBenchmarks`
- `MetricsBenchmarks`
- `MultinomialLogitBenchmarks`
- `MyersGateBenchmarks`
- `OlsBenchmarks`
- `OsaBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `PersistenceBenchmarks`
- `QgramBenchmarks`
- `QuantileBenchmarks`
- `RankTestBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RegressionMetricsBenchmarks`
- `RobustCovarianceBenchmarks`
- `RobustScalerSparseBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SerialCorrelationBenchmarks`
- `SilhouetteBenchmarks`
- `SimilaritySketchBenchmarks`
- `SmallArtifactBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `StopWordBenchmarks`
- `SurvivalBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TokenPatternBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Comparisons selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `cluster-weighted`
- `cox-extended`
- `cross-conformal`
- `glm`
- `iv`
- `nmf-transform`
- `ols`
- `onehot`
- `panel`
- `survival-aalen`
- `survival-family`
- `survival-parametric`
- `var`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 3.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `FuzzIncumbentBenchmarks` | Operation=Ratio | `FuzzySharp` | step | 2.8 | 2.12 | +32% | 30% |
| `BkTreeBenchmarks` | Radius=1, Shape=uniform | `TreeWithinDistance` | drift | 0.55 | 0.44 | +20% | 20% |
| `BkTreeBenchmarks` | Radius=1, Shape=clustered | `TreeWithinDistance` | drift | 0.59 | 0.49 | +20% | 20% |

**Still away from their median: 88.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | drift | 0.08 | 0.44 | -80% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | drift | 0.35 | 1.51 | -74% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 0.6 | 3.06 | -73% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | drift | 0.02 | 0.11 | -73% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 1.98 | 8.92 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 0.42 | 2.15 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_ChiSquare` | drift | 0.15 | 0.67 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | drift | 0.85 | 3.71 | -70% | 20% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x10 | `MetaNumerics_VarianceFraction` | step | 2184.13 | 1288.52 | +70% | 41% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | drift | 0.87 | 3.69 | -69% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | drift | 1.51 | 6.71 | -69% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_Wilcoxon` | drift | 0.25 | 1.05 | -67% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 0.51 | 2.14 | -66% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 0.56 | 2.23 | -65% | 20% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x50 | `MetaNumerics_VarianceFraction` | step | 440.27 | 271.84 | +62% | 42% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | drift | 0.25 | 0.97 | -62% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.41 | 0.83 | -54% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Bartlett` | drift | 0.21 | 0.28 | -46% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=AccuracyAlone | `MlNet` | step | 890.42 | 1647.9 | -46% | 42% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.44 | 3.099 | +45% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Bartlett` | drift | 0.41 | 0.52 | -44% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Binomial` | drift | 1.32 | 2.32 | -43% | 20% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `MetaNumerics_Fit` | step | 2.17 | 3.77 | -42% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `Lodestar_HeldPattern` | drift | 0.44 | 0.6 | -42% | 20% |
| `LcsGateBenchmarks` | Band=20 | `Kernel_Cjk` | drift | 0.23 | 0.17 | +41% | 20% |
| `LcsGateBenchmarks` | Band=20 | `Kernel` | step | 0.14 | 0.1 | +40% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `FuzzySharp_PerText` | drift | 1.58 | 1.91 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | drift | 0.005 | 0.008 | -38% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `FuzzySharp_PerText` | drift | 1.5 | 1.85 | -36% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_AndersonDarling` | drift | 1.91 | 3.1 | -36% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | step | 0.19 | 0.14 | +36% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `FuzzySharp_PerText` | drift | 1.64 | 2.2 | -35% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=16 | `Lodestar_HeldPattern` | step | 0.31 | 0.47 | -34% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=50000x8x32 | `MetaNumerics_Fit` | step | 18.12 | 27.35 | -34% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_Binomial` | drift | 0.003 | 0.003 | -33% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `Lodestar_HeldPattern` | drift | 0.77 | 1.05 | -33% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel` | drift | 0.12 | 0.09 | +33% | 20% |
| `IndelPatternBenchmarks` | Corpus=oneWordAffix, Length=32 | `FuzzySharp_PerText` | drift | 1.47 | 1.76 | -33% | 20% |
| `PrincipalComponentVarianceBenchmarks` | Shape=100x200 | `NumFlat_Pca` | drift | 1.67 | 1.64 | +32% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `Lodestar_HeldPattern` | step | 0.3 | 0.44 | -32% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x16x16 | `MetaNumerics_Fit` | step | 10.46 | 15.31 | -32% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=8 | `Fastenshtein` | drift | 3.35 | 3.37 | +31% | 20% |
| `BkTreeBenchmarks` | Radius=4, Shape=uniform | `TreeWithinDistance` | step | 1.7 | 1.3 | +31% | 30% |
| `LcsGateBenchmarks` | Band=32 | `Kernel_Cjk` | drift | 0.14 | 0.1 | +30% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | drift | 33.31 | 37.83 | -30% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Levene` | drift | 1.39 | 1.98 | -30% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Binomial` | drift | 0.24 | 0.37 | -30% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=500, Method=ward | `Aglomera_GetClustering` | drift | 31.91 | 31.02 | +29% | 20% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=16 | `Lodestar_HeldPattern` | drift | 0.56 | 0.72 | -29% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | drift | 22.6 | 22.13 | -29% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=AccuracyAlone | `MlNet` | drift | 2060.63 | 3109.69 | -29% | 20% |
| `FuzzCodePointBenchmarks` | — | `Ratio_DistinctAstral` | drift | 40.846 | 35.91 | +29% | 20% |
| `LcsGateBenchmarks` | Band=32 | `Kernel` | drift | 0.09 | 0.07 | +29% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.36 | 0.28 | +29% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_Bartlett` | drift | 0.185 | 0.151 | -28% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.35 | 0.29 | +28% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_AndersonDarling` | drift | 2.83 | 4.25 | -28% | 20% |
| `BkTreeBenchmarks` | Radius=2, Shape=clustered | `TreeWithinDistance` | drift | 1.53 | 1.2 | +28% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | drift | 5.397 | 6.291 | +27% | 20% |
| `BkTreeBenchmarks` | Radius=3, Shape=uniform | `TreeWithinDistance` | drift | 1.55 | 1.22 | +27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Accord_Bartlett` | drift | 0.369 | 0.301 | -27% | 20% |
| `BkTreeBenchmarks` | Radius=2, Shape=uniform | `TreeWithinDistance` | drift | 1.3 | 1.04 | +26% | 20% |
| `BkTreeBenchmarks` | Radius=4, Shape=clustered | `TreeWithinDistance` | drift | 1.58 | 1.34 | +25% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=16 | `Lodestar_HeldPattern` | drift | 0.7 | 0.79 | -25% | 20% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=32 | `FuzzySharp_PerText` | drift | 1.36 | 1.66 | -25% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.11 | 4.49 | -25% | 20% |
| `IndelPatternBenchmarks` | Corpus=oneWordAffix, Length=32 | `Lodestar_HeldPattern` | drift | 0.75 | 0.96 | -25% | 20% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.05 | 0.04 | +25% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Lloyd` | drift | 4.13 | 4.41 | -25% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | drift | 1.24 | 1.65 | -25% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | drift | 13.31 | 13.94 | -24% | 20% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=16 | `FuzzySharp_PerText` | drift | 1.8 | 2.14 | -24% | 20% |
| `CorrelationBenchmarks` | PairCount=10000 | `Lodestar_KendallTau` | drift | 28.2 | 24.74 | +24% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.87 | 1.87 | -24% | 20% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=32 | `Lodestar_HeldPattern` | drift | 0.76 | 0.96 | -24% | 20% |
| `BkTreeBenchmarks` | Radius=3, Shape=clustered | `TreeWithinDistance` | drift | 1.67 | 1.36 | +24% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 2.83 | 3.38 | -23% | 20% |
| `CorrelationBenchmarks` | PairCount=10000 | `Lodestar_KendallTau_Asymptotic` | drift | 28.13 | 24.9 | +23% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Accord_Binomial` | drift | 2.135 | 2.469 | -23% | 20% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Fit` | drift | 4.37 | 5.97 | -23% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=4 | `Lodestar_HeldPattern` | drift | 0.39 | 0.44 | -23% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_ClopperPearson` | drift | 1.41 | 2.02 | -22% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | drift | 1.505 | 1.413 | -21% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=16 | `FuzzySharp_PerText` | drift | 2.05 | 2.53 | -21% | 20% |
| `IndelPatternBenchmarks` | Corpus=beyondTable, Length=32 | `FuzzySharp_PerText` | drift | 1.38 | 1.5 | -21% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=16 | `FuzzySharp_PerText` | drift | 1.7 | 1.89 | -21% | 20% |
| `CdistCutoffBenchmarks` | Cutoff=90, Widths=True | `Lodestar_Cdist_EveryPair` | drift | 3.56 | 3.66 | +20% | 20% |
| `IndelPatternBenchmarks` | Corpus=beyondTable, Length=32 | `Lodestar_HeldPattern` | drift | 0.79 | 0.94 | -20% | 20% |
