# Nightly benchmark run

<!-- nightly-baseline: 6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks WeightedLeastSquaresBenchmarks cdist cluster-weighted cox-extended cross-conformal glm iv nmf-transform ols onehot panel survival-aalen survival-family survival-parametric transformers var -->
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

- Commit: `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`
- Previous run: `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

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
- `LevenshteinCodePointBenchmarks`
- `MetricsBenchmarks`
- `ManyLabelsMetricsBenchmarks`
- `RegressionMetricsBenchmarks`
- `MyersGateBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `AsyncArtifactBenchmarks`
- `ArtifactTimeBenchmarks`
- `ArtifactLoadOnlyBenchmarks`
- `SmallArtifactBenchmarks`
- `PersistenceBenchmarks`
- `StopWordBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `TokenPatternBenchmarks`
- `SimilaritySketchBenchmarks`
- `Bm25Benchmarks`
- `CoxBenchmarks`
- `SurvivalBenchmarks`

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **622.8 ns** |        **292.61 ns** |      **16.04 ns** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         560.3 ns |         54.70 ns |       3.00 ns |     0.90 |    0.02 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       1,447.0 ns |         77.62 ns |       4.25 ns |     2.32 |    0.05 |   0.2460 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       1,880.0 ns |         75.06 ns |       4.11 ns |     3.02 |    0.07 |   0.1202 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       2,872.7 ns |        282.33 ns |      15.48 ns |     4.61 |    0.10 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |       5,814.2 ns |        504.56 ns |      27.66 ns |     9.34 |    0.21 |   0.0076 |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       2,956.4 ns |      1,180.19 ns |      64.69 ns |     4.75 |    0.14 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **56,773.5 ns** |      **7,052.32 ns** |     **386.56 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      42,609.8 ns |     10,688.42 ns |     585.87 ns |     0.75 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |     574,785.5 ns |    315,261.52 ns |  17,280.55 ns |    10.12 |    0.27 | 166.0156 | 166.0156 | 166.0156 | 1324848 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,346,122.4 ns |    517,506.81 ns |  28,366.29 ns |    23.71 |    0.45 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   1,704,477.9 ns |    696,985.76 ns |  38,204.14 ns |    30.02 |    0.61 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 186,032,477.8 ns | 14,763,802.45 ns | 809,253.83 ns | 3,276.85 |   22.90 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   1,693,905.3 ns |    120,927.41 ns |   6,628.44 ns |    29.84 |    0.20 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **13.53 μs** |   **4.489 μs** |  **0.246 μs** |  **1.00** |    **0.02** | **0.9613** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    15.13 μs |   0.612 μs |  0.034 μs |  1.12 |    0.02 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,413.20 μs** | **634.573 μs** | **34.783 μs** |  **1.00** |    **0.03** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,062.88 μs | 135.753 μs |  7.441 μs |  0.75 |    0.02 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **562.18 ns** |      **49.001 ns** |      **2.686 ns** |   **1.00** |    **0.01** | **0.0038** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     1,602.81 ns |     527.844 ns |     28.933 ns |   2.85 |    0.05 | 0.0057 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     1,220.73 ns |     113.173 ns |      6.203 ns |   2.17 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     2,076.54 ns |   1,505.575 ns |     82.526 ns |   3.69 |    0.13 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     3,772.34 ns |     878.369 ns |     48.146 ns |   6.71 |    0.08 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     5,014.05 ns |     272.633 ns |     14.944 ns |   8.92 |    0.04 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     1,252.20 ns |     632.128 ns |     34.649 ns |   2.23 |    0.05 | 0.0973 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     2,082.90 ns |     215.221 ns |     11.797 ns |   3.71 |    0.02 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |       460.26 ns |      33.655 ns |      1.845 ns |   0.82 |    0.00 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,180.28 ns |   1,370.462 ns |     75.120 ns |   3.88 |    0.12 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |       684.69 ns |     100.442 ns |      5.506 ns |   1.22 |    0.01 | 0.0486 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |       547.95 ns |     149.714 ns |      8.206 ns |   0.97 |    0.01 | 0.0582 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       257.66 ns |     148.384 ns |      8.133 ns |   0.46 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |       905.24 ns |     118.520 ns |      6.496 ns |   1.61 |    0.01 | 0.0563 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |        66.96 ns |      39.131 ns |      2.145 ns |   0.12 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       410.61 ns |     218.113 ns |     11.956 ns |   0.73 |    0.02 | 0.0577 |     968 B |       15.12 |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **30,221.80 ns** |   **1,842.177 ns** |    **100.976 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   131,868.39 ns |  24,205.602 ns |  1,326.791 ns |  4.363 |    0.04 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   271,615.62 ns |  35,878.948 ns |  1,966.646 ns |  8.987 |    0.06 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 1,415,391.66 ns | 151,506.100 ns |  8,304.561 ns | 46.834 |    0.27 | 3.9063 |   80379 B |    1,255.92 |
| Lodestar_KruskalWallis         | 10000      |   660,352.28 ns |  84,897.144 ns |  4,653.499 ns | 21.850 |    0.15 |      - |      33 B |        0.52 |
| MetaNumerics_KruskalWallis     | 10000      | 2,394,124.78 ns | 444,526.455 ns | 24,365.995 ns | 79.219 |    0.73 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,041,838.93 ns | 128,440.649 ns |  7,040.265 ns | 34.473 |    0.23 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,453,107.73 ns |  35,664.424 ns |  1,954.887 ns | 48.082 |    0.15 | 3.9063 |   80371 B |    1,255.80 |
| Lodestar_OneWayAnova           | 10000      |    42,818.35 ns |   9,592.312 ns |    525.787 ns |  1.417 |    0.02 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   196,938.35 ns |   3,796.811 ns |    208.116 ns |  6.516 |    0.02 |      - |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 10000      |   111,481.55 ns |  17,607.426 ns |    965.122 ns |  3.689 |    0.03 | 4.7607 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   201,808.22 ns | 138,066.254 ns |  7,567.877 ns |  6.678 |    0.22 | 4.6387 |   80176 B |    1,252.75 |
| Lodestar_FisherExact           | 10000      |       260.96 ns |     172.023 ns |      9.429 ns |  0.009 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |       906.99 ns |     221.628 ns |     12.148 ns |  0.030 |    0.00 | 0.0563 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |        66.84 ns |       9.693 ns |      0.531 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       414.80 ns |     201.263 ns |     11.032 ns |  0.014 |    0.00 | 0.0577 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |-------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |   **1,001.7 ns** |     **188.33 ns** |     **10.32 ns** |  **1.00** |    **0.01** |  **0.3014** | **0.0019** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |   1,981.6 ns |   1,359.32 ns |     74.51 ns |  1.98 |    0.07 |  0.3242 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |     279.6 ns |     109.97 ns |      6.03 ns |  0.28 |    0.01 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     518.6 ns |      88.27 ns |      4.84 ns |  0.52 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |     463.1 ns |     108.03 ns |      5.92 ns |  0.46 |    0.01 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |   2,381.5 ns |     212.84 ns |     11.67 ns |  2.38 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |   4,940.5 ns |   2,646.05 ns |    145.04 ns |  4.93 |    0.13 |  0.0534 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |   3,604.8 ns |     460.55 ns |     25.24 ns |  3.60 |    0.04 |  0.0648 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |   2,837.1 ns |     679.41 ns |     37.24 ns |  2.83 |    0.04 |       - |      - |      48 B |       0.009 |
|                          |           |              |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     | **184,221.7 ns** | **195,437.48 ns** | **10,712.59 ns** | **1.002** |    **0.07** | **28.5645** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 869,594.0 ns | 271,441.08 ns | 14,878.60 ns | 4.731 |    0.24 | 28.3203 | 6.8359 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |  28,559.8 ns |   9,761.76 ns |    535.07 ns | 0.155 |    0.01 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |  55,284.6 ns |  22,515.81 ns |  1,234.17 ns | 0.301 |    0.02 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     608.1 ns |     444.63 ns |     24.37 ns | 0.003 |    0.00 |  0.0029 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     | 495,331.6 ns | 328,496.33 ns | 18,005.99 ns | 2.695 |    0.16 | 11.7188 | 2.9297 |  200216 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 937,823.0 ns | 474,562.95 ns | 26,012.40 ns | 5.102 |    0.28 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |           NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |   7,587.1 ns |     708.88 ns |     38.86 ns | 0.041 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **203.6 μs** |    **28.43 μs** |   **1.56 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,254.1 μs** | **1,163.95 μs** |  **63.80 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **3,789.7 μs** |   **194.09 μs** |  **10.64 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **12,807.9 μs** | **5,021.07 μs** | **275.22 μs** | **281.2500** | **265.6250** | **250.0000** | **1735.44 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |--------------:|--------------:|------------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |      **8.650 μs** |     **0.6687 μs** |   **0.0367 μs** |  **1.00** |    **0.01** |    **0.9766** |    **0.0153** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |      7.910 μs |     1.5519 μs |   0.0851 μs |  0.91 |    0.01 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     12.075 μs |     6.2894 μs |   0.3447 μs |  1.40 |    0.03 |    1.6327 |    0.0763 |        - |   26.84 KB |        1.68 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **33.886 μs** |     **4.7055 μs** |   **0.2579 μs** |  **1.00** |    **0.01** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     12.268 μs |     0.9491 μs |   0.0520 μs |  0.36 |    0.00 |    3.7994 |    0.4120 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     24.004 μs |     3.4924 μs |   0.1914 μs |  0.71 |    0.01 |    3.0212 |    0.3357 |        - |   49.57 KB |        0.69 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **6,284.026 μs** |   **801.4958 μs** |  **43.9327 μs** |  **1.00** |    **0.01** |  **335.9375** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  6,486.788 μs | 6,029.2933 μs | 330.4859 μs |  1.03 |    0.05 |  335.9375 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  6,922.388 μs |   913.1291 μs |  50.0517 μs |  1.10 |    0.01 |  507.8125 |  500.0000 | 500.0000 | 2598.83 KB |        1.71 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **10,651.490 μs** | **5,496.0068 μs** | **301.2547 μs** |  **1.00** |    **0.03** | **1125.0000** | **1109.3750** | **968.7500** | **6650.01 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             |  9,036.409 μs | 3,692.5193 μs | 202.3994 μs |  0.85 |    0.03 |  843.7500 |  828.1250 | 687.5000 | 5660.22 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 10,864.444 μs | 2,512.5796 μs | 137.7230 μs |  1.02 |    0.03 |  500.0000 |  500.0000 | 500.0000 | 4916.38 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error        | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,126.4 μs** |     **662.8 μs** |     **36.33 μs** |   **1.00** |    **0.04** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    52,541.3 μs |   1,608.8 μs |     88.18 μs |  46.68 |    1.28 |  3900.0000 |  1600.0000 |  900.0000 |  59881.36 KB |       58.56 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,141.4 μs** |     **170.6 μs** |      **9.35 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    56,782.3 μs |  29,760.3 μs |  1,631.26 μs |  49.75 |    1.29 |  5111.1111 |  1888.8889 |  888.8889 |   83111.6 KB |       81.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **398.5 μs** |     **196.4 μs** |     **10.77 μs** |   **1.00** |    **0.03** |     **1.4648** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |    75,533.0 μs |   8,716.0 μs |    477.76 μs | 189.63 |    4.58 |  4857.1429 |  1857.1429 |  857.1429 |  81289.81 KB |    2,678.88 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,435.2 μs** |     **129.9 μs** |      **7.12 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    40,731.3 μs |  36,232.6 μs |  1,986.03 μs |  28.38 |    1.20 |  3928.5714 |  1857.1429 |  928.5714 |  64191.23 KB |       62.78 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |     **8,240.7 μs** |     **991.5 μs** |     **54.35 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  |   857,194.8 μs |  64,577.4 μs |  3,539.71 μs | 104.02 |    0.70 | 35000.0000 | 10000.0000 | 4000.0000 | 537147.63 KB |       60.18 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |     **8,627.4 μs** |   **1,640.2 μs** |     **89.91 μs** |   **1.00** |    **0.01** |   **507.8125** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete |   988,936.3 μs | 540,270.6 μs | 29,614.05 μs | 114.64 |    3.15 | 48000.0000 | 12000.0000 | 4000.0000 | 747351.66 KB |       83.74 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **3,513.5 μs** |     **259.1 μs** |     **14.20 μs** |   **1.00** |    **0.00** |     **3.9063** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 1,822,690.4 μs | 788,600.0 μs | 43,225.83 μs | 518.77 |   10.81 | 47000.0000 | 12000.0000 | 4000.0000 |  732677.8 KB |    8,148.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **11,175.0 μs** |   **2,500.0 μs** |    **137.03 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     |   985,492.0 μs | 635,878.7 μs | 34,854.66 μs |  88.20 |    2.86 | 38000.0000 | 11000.0000 | 4000.0000 | 573626.15 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 14.68 ms | 25.89 ms | 1.419 ms |  875.0000 |  859.3750 | 453.1250 |  21.53 MB |
| CountLoadAsync4M | 30.56 ms | 21.92 ms | 1.202 ms | 1500.0000 | 1437.5000 | 687.5000 |  43.93 MB |
| CountLoad4M      | 30.84 ms | 11.01 ms | 0.604 ms | 1468.7500 | 1406.2500 | 656.2500 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error         | StdDev     | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-----------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 105,975.71 μs |  8,857.222 μs | 485.494 μs |  200.0000 |  200.0000 | 200.0000 | 54334.54 KB |
| CountSave240K         |     222.41 μs |     69.552 μs |   3.812 μs |   10.0098 |    3.6621 |   3.4180 |   504.67 KB |
| CountSaveAsync240K    |     248.43 μs |     77.875 μs |   4.269 μs |   11.2305 |    4.8828 |   4.8828 |   739.45 KB |
| TfidfSave45K          |      26.87 μs |     13.863 μs |   0.760 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      27.86 μs |      7.391 μs |   0.405 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     148.19 μs |    285.147 μs |  15.630 μs |    7.5684 |    1.4648 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     137.76 μs |     55.152 μs |   3.023 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  16,411.38 μs | 18,016.557 μs | 987.548 μs |  593.7500 |  562.5000 | 187.5000 | 22048.79 KB |
| CountLoadAsync4M      |  34,260.47 μs |  2,681.035 μs | 146.957 μs | 1187.5000 | 1125.0000 | 375.0000 | 44983.65 KB |
| CountLoad4M           |  33,274.67 μs | 17,334.927 μs | 950.186 μs | 1187.5000 | 1125.0000 | 375.0000 |  44977.3 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error        | StdDev      | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|----------:|----------:|---------:|------------:|
| TfidfSaveAsyncSmall    |      9.078 μs |     1.211 μs |   0.0664 μs |    1.6785 |    0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    323.296 μs |   382.142 μs |  20.9465 μs |   34.6680 |   28.3203 |  28.3203 |    740.1 KB |
| HashingLoadUndeclared  |      2.574 μs |     2.596 μs |   0.1423 μs |    5.1003 |         - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     15.478 μs |     2.709 μs |   0.1485 μs |    7.4158 |    0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |     75.450 μs |    57.522 μs |   3.1530 μs |   16.1133 |    3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    542.402 μs |   486.290 μs |  26.6552 μs |   38.0859 |   37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  |  6,656.242 μs | 1,684.206 μs |  92.3170 μs |  554.6875 |  546.8750 | 335.9375 | 10793.97 KB |
| CountSaveAsync1M       |  1,299.949 μs |   674.885 μs |  36.9927 μs |  121.0938 |  113.2813 | 113.2813 |     3162 KB |
| IndexSaveAsyncLongHead |  1,944.243 μs |   720.088 μs |  39.4704 μs |   82.0313 |   78.1250 |  78.1250 |  2124.09 KB |
| CountLoadUndeclared2M  | 12,084.618 μs | 2,374.255 μs | 130.1410 μs | 1046.8750 | 1031.2500 | 625.0000 | 22045.19 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **2.929 μs** |  **0.4625 μs** | **0.0254 μs** |  **1.00** |    **0.01** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   3.208 μs |  2.4710 μs | 0.1354 μs |  1.10 |    0.04 |  0.1640 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   3.222 μs |  1.0502 μs | 0.0576 μs |  1.10 |    0.02 |  0.1640 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **30.746 μs** | **16.0466 μs** | **0.8796 μs** |  **1.00** |    **0.03** |  **1.8005** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  10.415 μs |  2.9280 μs | 0.1605 μs |  0.34 |    0.01 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  10.069 μs |  0.9579 μs | 0.0525 μs |  0.33 |    0.01 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **116.983 μs** |  **5.7831 μs** | **0.3170 μs** |  **1.00** |    **0.00** |  **6.7139** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  37.987 μs |  6.4336 μs | 0.3526 μs |  0.32 |    0.00 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  35.374 μs | 34.8992 μs | 1.9129 μs |  0.30 |    0.01 |  4.2114 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **465.990 μs** | **79.9272 μs** | **4.3811 μs** |  **1.00** |    **0.01** | **26.8555** | **0.4883** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 150.041 μs | 16.3969 μs | 0.8988 μs |  0.32 |    0.00 | 20.0195 | 2.6855 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 132.734 μs | 17.6599 μs | 0.9680 μs |  0.28 |    0.00 | 15.8691 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0     | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|---------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** |  **93.04 ms** |  **25.255 ms** | **1.384 ms** |  **1.00** |    **0.02** |        **-** |   **27.11 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  45.35 ms |   2.732 ms | 0.150 ms |  0.49 |    0.01 |        - |  103.61 KB |        3.82 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   |  **99.85 ms** |   **8.736 ms** | **0.479 ms** |  **1.00** |    **0.01** |        **-** |   **23.72 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  43.78 ms |  33.027 ms | 1.810 ms |  0.44 |    0.02 |        - |  116.39 KB |        4.91 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **141.33 ms** |  **14.819 ms** | **0.812 ms** |  **1.00** |    **0.01** |        **-** |   **103.3 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 171.61 ms | 120.973 ms | 6.631 ms |  1.21 |    0.04 |        - |     259 KB |        2.51 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **143.83 ms** |  **34.888 ms** | **1.912 ms** |  **1.00** |    **0.02** |        **-** |   **54.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 147.59 ms |  18.791 ms | 1.030 ms |  1.03 |    0.01 |        - |  192.25 KB |        3.53 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **182.00 ms** | **127.136 ms** | **6.969 ms** |  **1.00** |    **0.05** |        **-** |  **949.52 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 241.22 ms |  74.545 ms | 4.086 ms |  1.33 |    0.05 |        - |  1365.5 KB |        1.44 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **184.66 ms** | **168.532 ms** | **9.238 ms** |  **1.00** |    **0.06** |        **-** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 225.14 ms | 143.877 ms | 7.886 ms |  1.22 |    0.06 |        - | 1152.56 KB |        1.55 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **205.09 ms** |  **30.935 ms** | **1.696 ms** |  **1.00** |    **0.01** |        **-** | **5113.18 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 272.95 ms |  18.922 ms | 1.037 ms |  1.33 |    0.01 |        - | 7215.35 KB |        1.41 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **204.46 ms** |  **41.333 ms** | **2.266 ms** |  **1.00** |    **0.01** | **333.3333** | **5513.85 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 271.39 ms |  64.451 ms | 3.533 ms |  1.33 |    0.02 |        - | 7963.65 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **28.19 μs** |      **1.721 μs** |     **0.094 μs** |         **-** |
| Cjk    | 1000   |      34.64 μs |      8.473 μs |     0.464 μs |         - |
| **Latin**  | **10000**  |   **2,631.46 μs** |    **177.142 μs** |     **9.710 μs** |         **-** |
| Cjk    | 10000  |   3,509.45 μs |    447.228 μs |    24.514 μs |         - |
| **Latin**  | **65536**  | **122,706.17 μs** | **96,816.727 μs** | **5,306.852 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean            | Error            | StdDev          | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |----------------:|-----------------:|----------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |        **968.5 ns** |         **86.59 ns** |         **4.75 ns** |     **1.00** |    **0.01** |    **0.0248** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |      1,495.9 ns |      1,237.51 ns |        67.83 ns |     1.54 |    0.06 |    0.0248 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |      1,944.8 ns |        228.29 ns |        12.51 ns |     2.01 |    0.01 |    0.3128 |    0.0019 |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |  2,364,235.9 ns |    380,473.69 ns |    20,855.05 ns | 2,441.19 |   21.34 |  917.9688 |  898.4375 |  886.7188 |  3722770 B |    8,780.12 |
| LuceneFromText         | 1000      |  4,735,821.0 ns |    269,991.19 ns |    14,799.13 ns | 4,889.97 |   24.64 |   85.9375 |   78.1250 |    7.8125 |  1375046 B |    3,243.03 |
|                        |           |                 |                  |                 |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |     **12,884.6 ns** |      **2,356.37 ns** |       **129.16 ns** |     **1.00** |    **0.01** |    **0.0153** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |     20,134.5 ns |      4,180.50 ns |       229.15 ns |     1.56 |    0.02 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |     10,927.3 ns |      5,413.82 ns |       296.75 ns |     0.85 |    0.02 |    0.5035 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 42,480,183.7 ns | 36,795,169.08 ns | 2,016,867.39 ns | 3,297.19 |  138.53 | 1846.1538 | 1769.2308 | 1769.2308 | 61664446 B |  145,435.01 |
| LuceneFromText         | 20000     | 83,510,138.4 ns |  5,359,089.19 ns |   293,749.76 ns | 6,481.81 |   59.34 | 1166.6667 | 1000.0000 |         - | 22416485 B |   52,869.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 14.65 ms | 17.03 ms | 0.934 ms |  1.00 |    0.08 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 49.34 ms | 18.13 ms | 0.994 ms |  3.38 |    0.19 | 1727.2727 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean       | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |-----------:|-----------:|----------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **8.661 μs** |   **4.280 μs** | **0.2346 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **18.985 μs** |   **5.473 μs** | **0.3000 μs** | **0.8850** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **37.812 μs** |   **3.227 μs** | **0.1769 μs** | **1.7090** | **0.0610** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **246.235 μs** | **128.636 μs** | **7.0510 μs** | **3.4180** |      **-** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| EncodeUnseenProse | 4.021 ms | 0.6090 ms | 0.0334 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean      | Error      | StdDev    | Allocated |
|----------- |--------- |----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **9.342 μs** |  **0.7357 μs** | **0.0403 μs** |         **-** |
| MyersGroup | cjk      | 86.852 μs |  0.7454 μs | 0.0409 μs |         - |
| **DpGroup**    | **latin**    |  **5.231 μs** |  **0.6962 μs** | **0.0382 μs** |         **-** |
| MyersGroup | latin    | 71.397 μs | 21.0102 μs | 1.1516 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **168.30 μs** |  **17.595 μs** | **0.964 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 161.11 μs |  29.702 μs | 1.628 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **197.38 μs** | **104.504 μs** | **5.728 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 175.74 μs |  19.198 μs | 1.052 μs |  0.89 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **177.25 μs** | **119.900 μs** | **6.572 μs** |  **1.00** |    **0.05** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 167.63 μs |  19.710 μs | 1.080 μs |  0.95 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **152.58 μs** | **154.780 μs** | **8.484 μs** |  **1.00** |    **0.07** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 184.85 μs | 174.229 μs | 9.550 μs |  1.21 |    0.08 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **162.27 μs** |  **14.713 μs** | **0.806 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 163.46 μs |  42.382 μs | 2.323 μs |  1.01 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   |  **88.86 μs** |  **64.874 μs** | **3.556 μs** |  **1.00** |    **0.05** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 181.10 μs | 100.051 μs | 5.484 μs |  2.04 |    0.09 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **126.83 μs** |  **68.623 μs** | **3.761 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 163.49 μs |  21.112 μs | 1.157 μs |  1.29 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **46.14 μs** |   **7.171 μs** | **0.393 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 177.44 μs |  18.016 μs | 0.988 μs |  3.85 |    0.03 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |------------:|------------:|----------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |    **99.44 μs** |    **60.05 μs** |  **3.292 μs** |  **1.00** |    **0.04** |   **1.0986** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |    91.88 μs |    86.67 μs |  4.751 μs |  0.92 |    0.05 |   1.0986 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   248.26 μs |   209.09 μs | 11.461 μs |  2.50 |    0.12 |  18.3105 |       - |       - |  301.95 KB |       15.44 |
|                               |      |             |             |           |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **2,152.50 μs** |   **254.57 μs** | **13.954 μs** |  **1.00** |    **0.01** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 1,927.56 μs |   709.77 μs | 38.905 μs |  0.90 |    0.02 |  99.6094 | 99.6094 | 99.6094 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 4,106.69 μs | 1,111.41 μs | 60.920 μs |  1.91 |    0.03 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-------------:|----------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **103.21 ms** |    **27.660 ms** |  **1.516 ms** |  **1.00** |    **0.02** |  **1800.0000** | **1800.0000** | **1800.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,242.52 ms |   711.131 ms | 38.980 ms | 12.04 |    0.36 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |              |           |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **33.81 ms** |     **7.317 ms** |  **0.401 ms** |  **1.00** |    **0.01** |  **1466.6667** | **1466.6667** | **1466.6667** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   485.64 ms | 1,389.688 ms | 76.173 ms | 14.37 |    1.96 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **108.03 ms** |  **38.719 ms** |  **2.122 ms** |  **1.00** |    **0.02** |  **1200.0000** |  **1200.0000** |  **1200.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 3,736.84 ms | 389.149 ms | 21.331 ms | 34.60 |    0.61 | 38000.0000 | 14000.0000 | 11000.0000 | 531.88 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 2,150.62 ms | 980.024 ms | 53.718 ms | 19.91 |    0.55 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **12.75 ms** |   **2.858 ms** |  **0.157 ms** |  **1.00** |    **0.02** |  **1343.7500** |  **1343.7500** |  **1343.7500** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   254.80 ms |  92.852 ms |  5.090 ms | 19.99 |    0.41 |  5000.0000 |  3500.0000 |  3000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   177.62 ms |  18.693 ms |  1.025 ms | 13.94 |    0.16 |  3750.0000 |  3750.0000 |  3750.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean      | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  7.289 ms |  0.5765 ms | 0.0316 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 65.951 ms | 74.8307 ms | 4.1017 ms |  9.05 |    0.49 |
| Nmf_Transform                             |  1.806 ms |  0.5768 ms | 0.0316 ms |  0.25 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 12.284 ms |  5.9101 ms | 0.3240 ms |  1.69 |    0.04 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  70.64 ns |  11.83 ns |  0.649 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   | 434.02 ns |  53.24 ns |  2.918 ns |  6.14 |    0.06 |      - |         - |          NA |
| TokenSortRatio | 460.11 ns | 259.78 ns | 14.239 ns |  6.51 |    0.18 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 515.85 ns | 129.60 ns |  7.104 ns |  7.30 |    0.10 | 0.0534 |     896 B |          NA |
| WRatio         | 665.98 ns | 577.64 ns | 31.662 ns |  9.43 |    0.40 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error              | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,218,346.8776 ns |    124,726.5870 ns |   6,836.6851 ns |  1.000 |    0.01 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    343,249.5693 ns |    136,181.5472 ns |   7,464.5702 ns |  0.282 |    0.01 | 35.6445 | 13.6719 |  602664 B |        3.75 |
| WRatio_ProseAndOneEmoji   |    708,464.1951 ns |    130,585.7989 ns |   7,157.8483 ns |  0.582 |    0.01 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 52,560,395.6000 ns | 13,441,046.2911 ns | 736,749.1050 ns | 43.142 |    0.56 |       - |       - |  510776 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **62.72 ns** |     **2.667 ns** |  **0.146 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   131.31 ns |    46.067 ns |  2.525 ns |  2.09 |    0.04 | 0.0048 |      80 B |          NA |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **428.56 ns** |    **37.811 ns** |  **2.073 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 5,440.45 ns | 1,666.881 ns | 91.367 ns | 12.69 |    0.19 | 0.0076 |     160 B |          NA |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **563.54 ns** |   **316.024 ns** | **17.322 ns** |  **1.00** |    **0.04** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,077.06 ns |   508.788 ns | 27.888 ns |  1.91 |    0.07 | 0.1144 |    1944 B |        2.17 |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **647.47 ns** |   **227.676 ns** | **12.480 ns** |  **1.00** |    **0.02** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 2,746.35 ns |   432.705 ns | 23.718 ns |  4.24 |    0.08 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **14.89 ns** |     **4.849 ns** |  **0.266 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    19.08 ns |     2.158 ns |  0.118 ns |  1.28 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    14.93 ns |     1.229 ns |  0.067 ns |  1.00 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    14.66 ns |    15.263 ns |  0.837 ns |  0.98 |    0.05 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **15.44 ns** |    **11.948 ns** |  **0.655 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    20.07 ns |     7.085 ns |  0.388 ns |  1.30 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    15.37 ns |     1.176 ns |  0.064 ns |  1.00 |    0.04 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    14.55 ns |     0.939 ns |  0.051 ns |  0.94 |    0.03 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **17.75 ns** |    **22.853 ns** |  **1.253 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    21.37 ns |     4.671 ns |  0.256 ns |  1.21 |    0.08 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    16.52 ns |     1.081 ns |  0.059 ns |  0.93 |    0.06 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    16.11 ns |     0.984 ns |  0.054 ns |  0.91 |    0.06 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **21.48 ns** |    **12.475 ns** |  **0.684 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    24.53 ns |     9.397 ns |  0.515 ns |  1.14 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    23.83 ns |     5.768 ns |  0.316 ns |  1.11 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    20.70 ns |     5.858 ns |  0.321 ns |  0.96 |    0.03 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **32.42 ns** |     **3.573 ns** |  **0.196 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    35.98 ns |     5.412 ns |  0.297 ns |  1.11 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    34.20 ns |     4.329 ns |  0.237 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    31.37 ns |     2.507 ns |  0.137 ns |  0.97 |    0.01 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **37.79 ns** |     **7.274 ns** |  **0.399 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    41.57 ns |     7.965 ns |  0.437 ns |  1.10 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    39.86 ns |     7.081 ns |  0.388 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    37.45 ns |    14.467 ns |  0.793 ns |  0.99 |    0.02 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **257.33 ns** |    **59.066 ns** |  **3.238 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   264.98 ns |    88.032 ns |  4.825 ns |  1.03 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   265.92 ns |   140.488 ns |  7.701 ns |  1.03 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   264.52 ns |    95.576 ns |  5.239 ns |  1.03 |    0.02 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **3,362.98 ns** | **1,186.543 ns** | **65.038 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 3,190.48 ns |   378.606 ns | 20.753 ns |  0.95 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 3,039.12 ns |   262.890 ns | 14.410 ns |  0.90 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,187.33 ns |   489.105 ns | 26.809 ns |  0.95 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **238.6 ns** |   **152.71 ns** |  **8.37 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    106.8 ns |    69.99 ns |  3.84 ns |  0.45 |    0.02 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **1,449.1 ns** |   **686.71 ns** | **37.64 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  1,932.7 ns | 1,061.42 ns | 58.18 ns |  1.33 |    0.05 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **512**    |  **8,214.0 ns** |   **736.36 ns** | **40.36 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 19,785.3 ns |   729.35 ns | 39.98 ns |  2.41 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      | **4,154.6 ns** | **2,857.84 ns** | **156.65 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      | 4,766.0 ns | 5,366.85 ns | 294.18 ns |  1.15 |    0.07 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 5,842.3 ns | 3,990.37 ns | 218.73 ns |  1.41 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **5,020.5 ns** |   **607.89 ns** |  **33.32 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     | 4,678.5 ns |   720.97 ns |  39.52 ns |  0.93 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 7,574.6 ns | 3,743.93 ns | 205.22 ns |  1.51 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **6,322.9 ns** | **2,446.91 ns** | **134.12 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 5,963.3 ns | 4,163.23 ns | 228.20 ns |  0.94 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 9,943.5 ns | 3,266.04 ns | 179.02 ns |  1.57 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      | **2,710.3 ns** |   **292.62 ns** |  **16.04 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      | 4,325.8 ns | 1,817.64 ns |  99.63 ns |  1.60 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      | 4,689.6 ns |   126.27 ns |   6.92 ns |  1.73 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     | **3,843.8 ns** | **1,945.22 ns** | **106.62 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     | 5,215.7 ns |   169.88 ns |   9.31 ns |  1.36 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 6,300.2 ns | 1,177.13 ns |  64.52 ns |  1.64 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **5,182.5 ns** | **2,047.49 ns** | **112.23 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     | 6,587.5 ns | 2,953.09 ns | 161.87 ns |  1.27 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 8,560.8 ns | 1,992.76 ns | 109.23 ns |  1.65 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      | **1,959.3 ns** |   **254.32 ns** |  **13.94 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      | 2,728.5 ns |   377.00 ns |  20.66 ns |  1.39 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      | 3,743.1 ns |   443.49 ns |  24.31 ns |  1.91 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     | **2,999.2 ns** |   **548.34 ns** |  **30.06 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     | 3,455.0 ns | 2,566.49 ns | 140.68 ns |  1.15 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 5,663.6 ns | 2,394.29 ns | 131.24 ns |  1.89 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **4,415.4 ns** | **2,168.93 ns** | **118.89 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     | 5,076.7 ns |   574.27 ns |  31.48 ns |  1.15 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 7,701.0 ns |   722.42 ns |  39.60 ns |  1.74 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      | **1,212.9 ns** |   **250.32 ns** |  **13.72 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      | 1,271.2 ns |   375.51 ns |  20.58 ns |  1.05 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      | 3,165.0 ns | 2,274.51 ns | 124.67 ns |  2.61 |    0.09 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     | **2,359.8 ns** |   **546.34 ns** |  **29.95 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     | 1,758.5 ns |   285.81 ns |  15.67 ns |  0.75 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     | 4,805.2 ns |   110.66 ns |   6.07 ns |  2.04 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     | **3,661.4 ns** |   **836.68 ns** |  **45.86 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     | 2,453.3 ns |   530.95 ns |  29.10 ns |  0.67 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 7,355.6 ns | 5,412.50 ns | 296.68 ns |  2.01 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      | **1,953.2 ns** | **1,165.59 ns** |  **63.89 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      | 2,177.0 ns |   543.75 ns |  29.80 ns |  1.12 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      | 3,735.7 ns | 2,013.95 ns | 110.39 ns |  1.91 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     | **2,917.4 ns** |   **302.07 ns** |  **16.56 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     | 2,673.3 ns |   496.78 ns |  27.23 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 5,336.8 ns |   271.93 ns |  14.91 ns |  1.83 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **4,394.4 ns** |   **173.75 ns** |   **9.52 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     | 4,306.7 ns |   395.96 ns |  21.70 ns |  0.98 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 7,508.4 ns | 1,065.50 ns |  58.40 ns |  1.71 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |   **819.0 ns** |   **308.47 ns** |  **16.91 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |   328.0 ns |    54.82 ns |   3.00 ns |  0.40 |    0.01 | 0.0024 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      | 2,769.6 ns |   745.34 ns |  40.85 ns |  3.38 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     | **1,766.8 ns** |   **211.33 ns** |  **11.58 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |   779.4 ns |    77.76 ns |   4.26 ns |  0.44 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     | 4,617.5 ns | 3,865.62 ns | 211.89 ns |  2.61 |    0.10 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     | **3,151.3 ns** | **1,504.18 ns** |  **82.45 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     | 1,495.5 ns |   867.66 ns |  47.56 ns |  0.47 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 6,749.3 ns | 3,267.37 ns | 179.10 ns |  2.14 |    0.07 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **2,944.5 μs** |    **608.88 μs** |    **33.37 μs** |  **1.00** |    **0.01** | **257.8125** | **246.0938** | **246.0938** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  19,565.6 μs |  6,102.05 μs |   334.47 μs |  6.65 |    0.12 |        - |        - |        - |   10.33 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  48,406.6 μs |  1,966.95 μs |   107.82 μs | 16.44 |    0.16 |        - |        - |        - |   41.84 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **429.5 μs** |     **83.95 μs** |     **4.60 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   2,571.3 μs |    241.14 μs |    13.22 μs |  5.99 |    0.06 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   1,521.8 μs |    549.05 μs |    30.10 μs |  3.54 |    0.07 |   1.9531 |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **13,253.3 μs** |  **2,282.40 μs** |   **125.11 μs** |  **1.00** |    **0.01** | **375.0000** | **375.0000** | **375.0000** | **4309.04 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 232,758.3 μs | 26,595.91 μs | 1,457.81 μs | 17.56 |    0.17 |        - |        - |        - |   15.06 KB |       0.003 |
| MetaNumerics_Fit | 50000x8x32  | 364,453.3 μs | 31,860.59 μs | 1,746.39 μs | 27.50 |    0.25 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **2.647 ms** |  **0.3163 ms** | **0.0173 ms** |  **1.00** |    **0.01** | **253.9063** | **246.0938** | **246.0938** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |   8.248 ms |  0.9398 ms | 0.0515 ms |  3.12 |    0.02 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **13.471 ms** |  **0.7811 ms** | **0.0428 ms** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  62.798 ms | 11.9878 ms | 0.6571 ms |  4.66 |    0.04 |        - |        - |        - |   96.38 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **24.570 ms** |  **4.3112 ms** | **0.2363 ms** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **3947.64 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 104.303 ms |  3.4599 ms | 0.1897 ms |  4.25 |    0.04 |        - |        - |        - |   36.34 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **65.76 ns** |     **66.104 ns** |     **3.623 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 8    |    31.73 ns |      4.520 ns |     0.248 ns |  0.48 |    0.02 |         - |          NA |
| Dp_Cjk     | 8    |    63.57 ns |      4.993 ns |     0.274 ns |  0.97 |    0.05 |         - |          NA |
| Kernel_Cjk | 8    |    52.95 ns |      3.618 ns |     0.198 ns |  0.81 |    0.04 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **12**   |   **134.30 ns** |    **213.115 ns** |    **11.682 ns** |  **1.00** |    **0.11** |         **-** |          **NA** |
| Kernel     | 12   |    36.97 ns |     21.330 ns |     1.169 ns |  0.28 |    0.02 |         - |          NA |
| Dp_Cjk     | 12   |   121.39 ns |     34.746 ns |     1.905 ns |  0.91 |    0.07 |         - |          NA |
| Kernel_Cjk | 12   |    65.83 ns |      7.569 ns |     0.415 ns |  0.49 |    0.04 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **14**   |   **147.04 ns** |     **31.709 ns** |     **1.738 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |    38.58 ns |     11.071 ns |     0.607 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |   147.32 ns |     92.529 ns |     5.072 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 14   |    62.03 ns |      8.503 ns |     0.466 ns |  0.42 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **16**   |   **444.32 ns** |    **264.537 ns** |    **14.500 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 16   |    41.32 ns |     13.047 ns |     0.715 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   426.28 ns |    583.959 ns |    32.009 ns |  0.96 |    0.07 |         - |          NA |
| Kernel_Cjk | 16   |    67.26 ns |     29.434 ns |     1.613 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **18**   |   **437.34 ns** |    **290.705 ns** |    **15.935 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 18   |    44.14 ns |     23.640 ns |     1.296 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |   435.21 ns |    168.631 ns |     9.243 ns |  1.00 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |    67.86 ns |      6.344 ns |     0.348 ns |  0.16 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **20**   |   **498.00 ns** |     **38.379 ns** |     **2.104 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |    47.91 ns |      7.565 ns |     0.415 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   497.12 ns |    129.331 ns |     7.089 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    72.61 ns |     62.624 ns |     3.433 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **24**   |   **787.94 ns** |     **24.262 ns** |     **1.330 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    52.66 ns |     11.483 ns |     0.629 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   644.56 ns |    280.912 ns |    15.398 ns |  0.82 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    80.05 ns |     37.190 ns |     2.039 ns |  0.10 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **32**   |   **989.39 ns** |     **96.433 ns** |     **5.286 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    64.05 ns |      4.759 ns |     0.261 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |   932.50 ns |    117.990 ns |     6.467 ns |  0.94 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    92.07 ns |      8.265 ns |     0.453 ns |  0.09 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **48**   | **1,950.15 ns** |    **721.644 ns** |    **39.556 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    78.45 ns |     36.547 ns |     2.003 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 1,871.26 ns |    300.587 ns |    16.476 ns |  0.96 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |   132.72 ns |     15.630 ns |     0.857 ns |  0.07 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **64**   | **2,894.75 ns** |  **1,157.933 ns** |    **63.470 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 64   |   151.89 ns |    355.970 ns |    19.512 ns |  0.05 |    0.01 |         - |          NA |
| Dp_Cjk     | 64   | 2,921.56 ns |  1,333.588 ns |    73.098 ns |  1.01 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |   155.36 ns |     18.495 ns |     1.014 ns |  0.05 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **96**   | **6,631.53 ns** |    **731.813 ns** |    **40.113 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   232.88 ns |     70.914 ns |     3.887 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 7,688.86 ns | 37,186.348 ns | 2,038.309 ns |  1.16 |    0.27 |         - |          NA |
| Kernel_Cjk | 96   |   588.66 ns |    360.317 ns |    19.750 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **14.36 ns** |     **1.877 ns** |   **0.103 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     14.18 ns |     2.903 ns |   0.159 ns |  0.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |     93.80 ns |    25.790 ns |   1.414 ns |  6.53 |    0.09 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     14.58 ns |     1.297 ns |   0.071 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **208.75 ns** |     **8.259 ns** |   **0.453 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    236.55 ns |    28.325 ns |   1.553 ns |  1.13 |    0.01 |         - |          NA |
| Distance_CodePoint         | 64     |    440.46 ns |    45.919 ns |   2.517 ns |  2.11 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    206.67 ns |    28.446 ns |   1.559 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **587.75 ns** |   **277.943 ns** |  **15.235 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,194.15 ns |   819.356 ns |  44.912 ns |  2.03 |    0.08 |         - |          NA |
| Distance_CodePoint         | 128    |    993.26 ns |    53.617 ns |   2.939 ns |  1.69 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    578.38 ns |    23.183 ns |   1.271 ns |  0.98 |    0.02 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    |  **7,534.69 ns** | **3,463.635 ns** | **189.854 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 10,635.53 ns |   770.871 ns |  42.254 ns |  1.41 |    0.03 |         - |          NA |
| Distance_CodePoint         | 512    |  8,834.24 ns | 1,960.874 ns | 107.482 ns |  1.17 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  7,414.32 ns | 4,968.016 ns | 272.314 ns |  0.98 |    0.04 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **217.6 ns** |     **46.03 ns** |     **2.52 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     146.1 ns |     19.39 ns |     1.06 ns |  0.67 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **216.5 ns** |     **17.61 ns** |     **0.97 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     150.3 ns |     61.02 ns |     3.34 ns |  0.69 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **267.3 ns** |     **40.18 ns** |     **2.20 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     201.4 ns |      7.88 ns |     0.43 ns |  0.75 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **267.8 ns** |     **78.54 ns** |     **4.31 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     201.5 ns |      7.74 ns |     0.42 ns |  0.75 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **319.2 ns** |     **28.74 ns** |     **1.58 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     263.1 ns |     50.71 ns |     2.78 ns |  0.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **319.5 ns** |    **176.07 ns** |     **9.65 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     263.7 ns |    141.08 ns |     7.73 ns |  0.83 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **372.1 ns** |     **29.40 ns** |     **1.61 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |     713.5 ns |    110.40 ns |     6.05 ns |  1.92 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **375.6 ns** |     **53.98 ns** |     **2.96 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |     774.9 ns |     65.70 ns |     3.60 ns |  2.06 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,256.8 ns** |    **520.50 ns** |    **28.53 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   3,581.4 ns |  1,020.17 ns |    55.92 ns |  2.85 |    0.07 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,256.6 ns** |    **430.72 ns** |    **23.61 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   3,638.7 ns |  1,107.50 ns |    60.71 ns |  2.90 |    0.06 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |   **9,580.2 ns** |    **908.76 ns** |    **49.81 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  37,389.2 ns |  2,809.49 ns |   154.00 ns |  3.90 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **325,335.2 ns** | **90,220.89 ns** | **4,945.31 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  38,287.4 ns |  4,301.69 ns |   235.79 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **14.13 ns** |       **0.581 ns** |      **0.032 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      52.23 ns |      17.285 ns |      0.947 ns |  3.70 |    0.06 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      47.98 ns |       5.650 ns |      0.310 ns |  3.40 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |      93.92 ns |      38.006 ns |      2.083 ns |  6.65 |    0.13 | 0.0076 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **204.19 ns** |      **18.666 ns** |      **1.023 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   3,253.25 ns |   3,567.401 ns |    195.541 ns | 15.93 |    0.83 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |     738.88 ns |     195.362 ns |     10.708 ns |  3.62 |    0.05 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   5,811.43 ns |   1,875.537 ns |    102.804 ns | 28.46 |    0.45 | 0.0305 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |   **7,190.58 ns** |     **652.280 ns** |     **35.754 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 239,314.94 ns | 215,577.489 ns | 11,816.530 ns | 33.28 |    1.43 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  21,080.09 ns |  24,159.398 ns |  1,324.258 ns |  2.93 |    0.16 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 394,680.33 ns |  84,123.863 ns |  4,611.113 ns | 54.89 |    0.60 |      - |    4160 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **269.3 μs** |  **31.44 μs** |  **1.72 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 271.2 μs |  19.68 μs |  1.08 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 296.4 μs |  37.31 μs |  2.05 μs |  9.2773 |  2.9297 | 153.98 KB |
| **F1Macro**           | **5000**   | **348.9 μs** | **216.33 μs** | **11.86 μs** | **20.5078** |  **6.3477** | **337.24 KB** |
| PrecisionWeighted | 5000   | 361.5 μs |  29.94 μs |  1.64 μs | 20.5078 |  6.3477 | 337.24 KB |
| ReportRows        | 5000   | 504.5 μs | 158.05 μs |  8.66 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |      **81.19 μs** |     **51.349 μs** |     **2.815 μs** |   **1.00** |    **0.04** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  53,105.53 μs | 22,286.712 μs | 1,221.610 μs | 654.59 |   23.28 | 900.0000 | 900.0000 | 900.0000 | 31413.68 KB |   12,805.58 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **1,723.47 μs** |    **243.002 μs** |    **13.320 μs** |   **1.00** |    **0.01** |   **1.9531** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 262,421.47 μs | 97,048.736 μs | 5,319.569 μs | 152.27 |    2.86 | 500.0000 | 500.0000 | 500.0000 | 32058.39 KB |      760.73 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **13.55 μs** |      **1.381 μs** |     **0.076 μs** |   **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     484.78 μs |    105.941 μs |     5.807 μs |  35.77 |    0.41 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Gen1   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **2,435.4 ns** |       **192.27 ns** |      **10.54 ns** | **0.0229** |      **-** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     4,098.5 ns |       191.40 ns |      10.49 ns | 0.0229 |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       119.4 ns |        10.95 ns |       0.60 ns |      - |      - |         - |
| F1Macro        | 1000    | 2       |     2,577.0 ns |     1,471.00 ns |      80.63 ns | 0.0381 |      - |     656 B |
| Report         | 1000    | 2       |     3,817.8 ns |       867.52 ns |      47.55 ns | 0.3204 |      - |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **2,221.2 ns** |       **423.52 ns** |      **23.21 ns** | **0.0763** |      **-** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     4,392.8 ns |     1,066.44 ns |      58.46 ns | 0.0763 |      - |    1336 B |
| AccuracyScore  | 1000    | 10      |       121.9 ns |        11.87 ns |       0.65 ns |      - |      - |         - |
| F1Macro        | 1000    | 10      |     2,465.7 ns |     1,856.30 ns |     101.75 ns | 0.1106 |      - |    1880 B |
| Report         | 1000    | 10      |     5,908.2 ns |       360.69 ns |      19.77 ns | 0.7401 | 0.0076 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **233,750.3 ns** |   **128,668.12 ns** |   **7,052.73 ns** |      **-** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   412,239.4 ns |   172,590.70 ns |   9,460.28 ns |      - |      - |     400 B |
| AccuracyScore  | 100000  | 2       |    11,902.0 ns |       991.59 ns |      54.35 ns |      - |      - |         - |
| F1Macro        | 100000  | 2       |   231,372.7 ns |    17,549.69 ns |     961.96 ns |      - |      - |     656 B |
| Report         | 100000  | 2       |   238,731.4 ns |   121,332.92 ns |   6,650.67 ns | 0.2441 |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **200,914.4 ns** |   **152,067.15 ns** |   **8,335.31 ns** |      **-** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   542,695.8 ns |   100,257.63 ns |   5,495.46 ns |      - |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |     6,218.8 ns |       829.61 ns |      45.47 ns |      - |      - |         - |
| F1Macro        | 100000  | 10      |   200,269.7 ns |    93,198.96 ns |   5,108.55 ns |      - |      - |    1880 B |
| Report         | 100000  | 10      |   198,795.8 ns |    29,503.24 ns |   1,617.17 ns | 0.7324 |      - |   12832 B |
| **Matrix**         | **1000000** | **2**       | **2,344,515.9 ns** |   **209,534.89 ns** |  **11,485.31 ns** |      **-** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 4,772,989.2 ns |   588,064.10 ns |  32,233.78 ns |      - |      - |     406 B |
| AccuracyScore  | 1000000 | 2       |    83,466.0 ns |    65,498.65 ns |   3,590.20 ns |      - |      - |         - |
| F1Macro        | 1000000 | 2       | 2,366,036.3 ns |   627,751.71 ns |  34,409.19 ns |      - |      - |     659 B |
| Report         | 1000000 | 2       | 2,372,954.9 ns |   348,671.47 ns |  19,111.86 ns |      - |      - |    5507 B |
| **Matrix**         | **1000000** | **10**      | **1,886,587.6 ns** |   **426,980.28 ns** |  **23,404.23 ns** |      **-** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 5,758,749.4 ns | 1,722,002.39 ns |  94,388.76 ns |      - |      - |    1342 B |
| AccuracyScore  | 1000000 | 10      |    68,462.0 ns |     4,803.19 ns |     263.28 ns |      - |      - |         - |
| F1Macro        | 1000000 | 10      | 2,072,345.9 ns | 2,000,118.30 ns | 109,633.23 ns |      - |      - |    1883 B |
| Report         | 1000000 | 10      | 2,189,416.1 ns | 1,554,754.38 ns |  85,221.33 ns |      - |      - |   12875 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |---------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **5,214.103 μs** |  **1,018.1966 μs** |    **55.8108 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1280 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  25,041.613 μs |  4,844.7353 μs |   265.5563 μs |     4.80 |    0.06 | 593.7500 | 593.7500 | 593.7500 |  5090172 B |    3,976.70 |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |       **6.253 μs** |      **0.5507 μs** |     **0.0302 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  25,036.423 μs |  8,677.2769 μs |   475.6308 μs | 4,003.72 |   67.95 | 593.7500 | 593.7500 | 593.7500 |  5089534 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **105,987.700 μs** | **30,739.2969 μs** | **1,684.9246 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 151,242.237 μs | 18,771.8945 μs | 1,028.9509 μs |     1.43 |    0.02 | 250.0000 | 250.0000 | 250.0000 | 23229826 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |      **69.147 μs** |     **25.5638 μs** |     **1.4012 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 149,934.051 μs | 47,036.3950 μs | 2,578.2235 μs | 2,168.92 |   50.01 | 250.0000 | 250.0000 | 250.0000 | 23229826 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **41.14 ns** |     **9.278 ns** |   **0.509 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 4    |    43.69 ns |    15.061 ns |   0.826 ns |  1.06 |    0.02 |         - |          NA |
| Dp_Cjk     | 4    |    42.73 ns |    15.999 ns |   0.877 ns |  1.04 |    0.02 |         - |          NA |
| Kernel_Cjk | 4    |    41.16 ns |     4.940 ns |   0.271 ns |  1.00 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **6**    |    **60.00 ns** |    **23.186 ns** |   **1.271 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 6    |    41.40 ns |     2.478 ns |   0.136 ns |  0.69 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    58.32 ns |    10.785 ns |   0.591 ns |  0.97 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    71.15 ns |     8.193 ns |   0.449 ns |  1.19 |    0.02 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **8**    |    **77.42 ns** |     **7.232 ns** |   **0.396 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |    50.69 ns |     7.167 ns |   0.393 ns |  0.65 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    80.71 ns |     6.291 ns |   0.345 ns |  1.04 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    90.58 ns |     7.495 ns |   0.411 ns |  1.17 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **10**   |   **105.12 ns** |    **20.601 ns** |   **1.129 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |    54.07 ns |     1.320 ns |   0.072 ns |  0.51 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |   118.39 ns |    84.668 ns |   4.641 ns |  1.13 |    0.04 |         - |          NA |
| Kernel_Cjk | 10   |    78.02 ns |     6.369 ns |   0.349 ns |  0.74 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **167.76 ns** |    **13.517 ns** |   **0.741 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    63.21 ns |     9.693 ns |   0.531 ns |  0.38 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   168.12 ns |     5.833 ns |   0.320 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    85.67 ns |    11.170 ns |   0.612 ns |  0.51 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **237.53 ns** |    **31.112 ns** |   **1.705 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    75.82 ns |     5.722 ns |   0.314 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   232.12 ns |    19.976 ns |   1.095 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    99.11 ns |     8.397 ns |   0.460 ns |  0.42 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **474.62 ns** |   **120.058 ns** |   **6.581 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |    99.47 ns |     8.557 ns |   0.469 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   479.18 ns |    14.398 ns |   0.789 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |   131.74 ns |    73.128 ns |   4.008 ns |  0.28 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,140.45 ns** | **1,774.045 ns** |  **97.241 ns** |  **1.01** |    **0.11** |         **-** |          **NA** |
| Kernel     | 32   |   128.11 ns |     7.372 ns |   0.404 ns |  0.11 |    0.01 |         - |          NA |
| Dp_Cjk     | 32   | 1,084.79 ns |   608.780 ns |  33.369 ns |  0.96 |    0.08 |         - |          NA |
| Kernel_Cjk | 32   |   164.56 ns |    17.321 ns |   0.949 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **1,927.35 ns** |   **405.738 ns** |  **22.240 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |   178.41 ns |    30.082 ns |   1.649 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 1,994.19 ns | 2,862.678 ns | 156.913 ns |  1.03 |    0.07 |         - |          NA |
| Kernel_Cjk | 48   |   218.47 ns |    60.396 ns |   3.310 ns |  0.11 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **3,254.51 ns** |   **247.015 ns** |  **13.540 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |   233.29 ns |    63.943 ns |   3.505 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 3,291.01 ns |   229.521 ns |  12.581 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |   272.17 ns |    35.158 ns |   1.927 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **7,550.84 ns** | **1,302.789 ns** |  **71.410 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   455.12 ns |    79.185 ns |   4.340 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 7,638.21 ns | 2,796.261 ns | 153.273 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |   893.64 ns |   163.543 ns |   8.964 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **145.45 μs** |  **14.236 μs** |  **0.780 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   151.72 μs |  33.899 μs |  1.858 μs |  1.04 |    0.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    32.71 μs |   4.840 μs |  0.265 μs |  0.22 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   162.26 μs |   7.229 μs |  0.396 μs |  1.12 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **146.25 μs** |  **26.644 μs** |  **1.460 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   167.42 μs |  32.769 μs |  1.796 μs |  1.14 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    32.92 μs |   3.358 μs |  0.184 μs |  0.23 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   153.06 μs |  20.560 μs |  1.127 μs |  1.05 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,451.99 μs** |  **22.699 μs** |  **1.244 μs** |  **1.00** |    **0.00** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 1,501.07 μs | 252.545 μs | 13.843 μs |  1.03 |    0.01 |      - |    7785 B |        9.72 |
| SparseFit             | 100000   | 10         |   830.96 μs | 109.757 μs |  6.016 μs |  0.57 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,591.87 μs | 207.846 μs | 11.393 μs |  1.10 |    0.01 |      - |     801 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,462.09 μs** | **654.152 μs** | **35.856 μs** |  **1.00** |    **0.03** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 1,519.11 μs | 164.444 μs |  9.014 μs |  1.04 |    0.02 | 3.9063 |   77625 B |       96.91 |
| SparseFit             | 100000   | 100        |   794.56 μs |  72.157 μs |  3.955 μs |  0.54 |    0.01 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,543.76 μs |  45.893 μs |  2.516 μs |  1.06 |    0.02 |      - |     801 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean       | Error       | StdDev     | Allocated |
|------------ |------------- |-----------:|------------:|-----------:|----------:|
| **Embedded**    | **65**           |   **6.261 μs** |   **0.6407 μs** |  **0.0351 μs** |         **-** |
| EqualLength | 65           |   1.617 μs |   0.1954 μs |  0.0107 μs |         - |
| **Embedded**    | **128**          |  **24.001 μs** |  **10.6158 μs** |  **0.5819 μs** |         **-** |
| EqualLength | 128          |   3.166 μs |   0.4942 μs |  0.0271 μs |         - |
| **Embedded**    | **512**          | **869.329 μs** | **265.1636 μs** | **14.5345 μs** |         **-** |
| EqualLength | 512          |  26.164 μs |  14.7456 μs |  0.8083 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean        | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |------------:|-------------:|------------:|---------:|---------:|---------:|-----------:|
| VocabTxt               |  2,799.5 μs |   5,658.7 μs |   310.17 μs | 117.1875 | 113.2813 |  35.1563 | 3711.63 KB |
| TokenizerJsonWordPiece |  7,327.8 μs |   4,951.6 μs |   271.41 μs | 187.5000 | 171.8750 |  46.8750 | 5852.31 KB |
| TokenizerJsonUnigram   |  6,633.6 μs |   3,952.7 μs |   216.66 μs | 109.3750 | 101.5625 |  39.0625 |    4749 KB |
| SpieceModel            |  3,661.4 μs |   4,018.7 μs |   220.28 μs | 144.5313 | 140.6250 |  46.8750 | 4807.72 KB |
| TfidfSave              |    929.4 μs |     474.5 μs |    26.01 μs |  27.3438 |  21.4844 |  21.4844 | 2137.07 KB |
| TfidfLoad              |  2,787.0 μs |   1,187.9 μs |    65.11 μs |  89.8438 |  85.9375 |  27.3438 | 2921.38 KB |
| EmbeddingIndexSave     |  1,668.4 μs |     572.4 μs |    31.38 μs | 181.6406 | 175.7813 | 175.7813 | 20350.6 KB |
| EmbeddingIndexLoad     |  3,513.7 μs |   5,855.0 μs |   320.93 μs | 175.7813 | 140.6250 | 109.3750 | 16094.6 KB |
| EmbeddingIndexSaveFile | 74,092.7 μs |  26,280.4 μs | 1,440.52 μs |        - |        - |        - |  323.53 KB |
| EmbeddingIndexLoadGzip | 52,339.3 μs | 138,195.4 μs | 7,574.95 μs |        - |        - |        - | 16095.2 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **5,204.46 μs** |   **361.552 μs** | **19.818 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 9,752.82 μs |   714.976 μs | 39.190 μs |  1.87 |    0.01 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **79.66 μs** |     **7.723 μs** |  **0.423 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |   109.12 μs |     8.840 μs |  0.485 μs |  1.37 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** | **1,718.31 μs** |    **97.031 μs** |  **5.319 μs** |  **1.00** |    **0.00** |   **1.9531** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 | 1,644.12 μs | 1,100.906 μs | 60.344 μs |  0.96 |    0.03 |   1.9531 |        - |        - |  40.06 KB |        0.95 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |    **13.92 μs** |     **1.099 μs** |  **0.060 μs** |  **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |    15.57 μs |     1.487 μs |  0.082 μs |  1.12 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **31.68 μs** |   **2.602 μs** |  **0.143 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    31.48 μs |   7.184 μs |  0.394 μs |        - |        - |        - |         - |
| R2Score  | 100000  |    79.16 μs |   3.581 μs |  0.196 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   313.08 μs | 100.016 μs |  5.482 μs | 199.2188 | 199.2188 | 199.2188 |  800193 B |
| **Mse**      | **1000000** |   **329.07 μs** | **111.896 μs** |  **6.133 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   313.14 μs | 107.946 μs |  5.917 μs |        - |        - |        - |         - |
| R2Score  | 1000000 |   796.69 μs |  24.928 μs |  1.366 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 4,117.63 μs | 671.596 μs | 36.812 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|------------:|----------:|------:|--------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **36.732 ms** |   **1.8033 ms** | **0.0988 ms** |  **1.00** |    **0.00** |  **428.5714** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   7.519 ms |   1.4707 ms | 0.0806 ms |  0.20 |    0.00 |  171.8750 |  164.0625 |  85.9375 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   5.556 ms |   3.2493 ms | 0.1781 ms |  0.15 |    0.00 |   15.6250 |         - |        - |    304.73 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   3.930 ms |   0.2696 ms | 0.0148 ms |  0.11 |    0.00 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   7.245 ms |   0.4451 ms | 0.0244 ms |  0.20 |    0.00 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **37.270 ms** |   **2.5960 ms** | **0.1423 ms** |  **1.00** |    **0.00** |  **428.5714** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |   9.681 ms |   8.0996 ms | 0.4440 ms |  0.26 |    0.01 |  328.1250 |  312.5000 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   6.684 ms |   3.1103 ms | 0.1705 ms |  0.18 |    0.00 |   23.4375 |         - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   4.242 ms |   3.5838 ms | 0.1964 ms |  0.11 |    0.00 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   7.121 ms |   3.1267 ms | 0.1714 ms |  0.19 |    0.00 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **664.606 ms** | **154.3776 ms** | **8.4620 ms** |  **1.00** |    **0.02** | **7000.0000** |         **-** |        **-** | **126359.48 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  30.490 ms |   9.8405 ms | 0.5394 ms |  0.05 |    0.00 | 1000.0000 | 1000.0000 | 500.0000 |  10335.26 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  21.494 ms |   2.0640 ms | 0.1131 ms |  0.03 |    0.00 |   62.5000 |         - |        - |   1218.81 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  15.664 ms |   1.0743 ms | 0.0589 ms |  0.02 |    0.00 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  28.176 ms |   7.0290 ms | 0.3853 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |       0.014 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **661.294 ms** |  **89.3918 ms** | **4.8999 ms** |  **1.00** |    **0.01** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  46.155 ms |  19.0266 ms | 1.0429 ms |  0.07 |    0.00 | 1636.3636 | 1636.3636 | 909.0909 |  18666.49 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  27.379 ms |  19.0088 ms | 1.0419 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.81 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  15.978 ms |   3.1165 ms | 0.1708 ms |  0.02 |    0.00 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  29.936 ms |   3.0172 ms | 0.1654 ms |  0.05 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean           | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |       623.2 ns |        64.27 ns |       3.52 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |       763.2 ns |        46.53 ns |       2.55 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |       626.2 ns |       169.30 ns |       9.28 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |       217.9 ns |        32.30 ns |       1.77 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 7,677,232.8 ns | 1,913,353.64 ns | 104,877.37 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1,393.5 μs** |   **524.92 μs** |  **28.77 μs** |  **1.00** |    **0.03** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |   960.5 μs |    56.72 μs |   3.11 μs |  0.69 |    0.01 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1,417.3 μs |    78.93 μs |   4.33 μs |  1.02 |    0.02 |  72.2656 |  72.2656 |  72.2656 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1,133.6 μs |    65.87 μs |   3.61 μs |  0.81 |    0.01 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |            |             |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **6,615.7 μs** | **2,107.29 μs** | **115.51 μs** |  **1.00** |    **0.02** | **867.1875** | **867.1875** | **867.1875** | **3436.57 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 4,967.4 μs | 2,318.12 μs | 127.06 μs |  0.75 |    0.02 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 6,966.5 μs | 2,076.92 μs | 113.84 μs |  1.05 |    0.02 | 515.6250 | 500.0000 | 500.0000 | 2349.75 KB |        0.68 |
| HashingWithStopWords | 1000      | 5,919.5 μs | 4,030.15 μs | 220.91 μs |  0.89 |    0.03 | 265.6250 | 265.6250 | 265.6250 | 1139.45 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2    | Allocated  |
|--------------------------- |-------------- |----------------:|----------------:|--------------:|---------:|---------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **21,110,705.8 ns** |  **2,666,792.5 ns** | **146,175.90 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.58 KB** |
| Construct                  | [^ ]+         |      1,249.6 ns |        112.5 ns |       6.17 ns |   0.3319 |   0.0019 |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      1,972.4 ns |        412.8 ns |      22.62 ns |   0.4539 |   0.0038 |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **17,817,297.7 ns** |  **1,359,217.5 ns** |  **74,503.30 ns** |        **-** |        **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1,197.7 ns |        178.9 ns |       9.81 ns |   0.3300 |   0.0019 |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      1,787.1 ns |      1,694.6 ns |      92.89 ns |   0.4387 |   0.0038 |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **23,951,196.3 ns** | **12,047,567.0 ns** | **660,367.81 ns** |  **62.5000** |  **31.2500** |       **-** | **1555.96 KB** |
| Construct                  | [A-Za-z]\\w+  |      2,077.0 ns |        308.0 ns |      16.88 ns |   0.4158 |   0.0038 |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      2,866.0 ns |        531.3 ns |      29.12 ns |   0.5341 |        - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **12,647,526.4 ns** |    **467,177.3 ns** |  **25,607.56 ns** | **156.2500** | **140.6250** | **93.7500** | **9743.47 KB** |
| Construct                  | \\b\\w\\w+\\b |        664.3 ns |        389.9 ns |      21.37 ns |   0.1135 |        - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1,287.8 ns |      1,898.7 ns |     104.08 ns |   0.2346 |        - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **21,542,300.6 ns** |  **2,521,996.3 ns** | **138,239.13 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.45 KB** |
| Construct                  | \\S+          |      1,334.2 ns |        783.0 ns |      42.92 ns |   0.3223 |   0.0019 |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      2,125.2 ns |        233.1 ns |      12.77 ns |   0.4425 |   0.0038 |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|----------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **11.05 ms** |   **5.546 ms** |  **0.304 ms** |  **1.00** |    **0.03** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  32.09 ms |   1.070 ms |  0.059 ms |  2.91 |    0.07 |  218.7500 |   3.55 MB |        0.41 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **27.37 ms** |   **9.369 ms** |  **0.514 ms** |  **1.00** |    **0.02** |  **312.5000** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  34.78 ms |   3.464 ms |  0.190 ms |  1.27 |    0.02 |  133.3333 |   3.09 MB |        0.57 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **56.70 ms** |  **28.469 ms** |  **1.560 ms** |  **1.00** |    **0.03** | **1777.7778** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 202.03 ms | 270.874 ms | 14.848 ms |  3.56 |    0.24 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean     | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |----- |---------:|----------:|---------:|------:|--------:|----------:|------------:|
| **Dot**    | **384**  | **33.35 ns** |  **4.495 ns** | **0.246 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| L2Norm | 384  | 23.65 ns |  3.356 ns | 0.184 ns |  0.71 |    0.01 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **768**  | **68.65 ns** | **32.450 ns** | **1.779 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| L2Norm | 768  | 47.30 ns |  2.579 ns | 0.141 ns |  0.69 |    0.02 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **1024** | **89.79 ns** | **21.259 ns** | **1.165 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| L2Norm | 1024 | 63.96 ns | 13.601 ns | 0.745 ns |  0.71 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |-----------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |   **112.4 μs** |  **42.46 μs** |  **2.33 μs** |  **1.00** |    **0.03** |   **12.4512** |    **2.1973** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |   118.9 μs |  12.19 μs |  0.67 μs |  1.06 |    0.02 |   14.2822 |    3.5400 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |   405.2 μs |  24.48 μs |  1.34 μs |  3.60 |    0.07 |   35.6445 |   13.1836 |         - |   586.98 KB |        2.86 |
| CountCharWordBoundary | 200       | 1,342.9 μs | 778.14 μs | 42.65 μs | 11.95 |    0.39 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |   103.3 μs |  16.98 μs |  0.93 μs |  0.92 |    0.02 |    8.3008 |    1.0986 |         - |   136.13 KB |        0.66 |
|                       |           |            |           |          |       |         |           |           |           |             |             |
| **Count**                 | **1000**      | **1,041.2 μs** | **173.84 μs** |  **9.53 μs** |  **1.00** |    **0.01** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      | 1,160.2 μs |  61.07 μs |  3.35 μs |  1.11 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      | 2,609.0 μs | 974.22 μs | 53.40 μs |  2.51 |    0.05 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 9,352.0 μs | 293.45 μs | 16.09 μs |  8.98 |    0.07 | 1046.8750 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      | 1,089.4 μs |  46.77 μs |  2.56 μs |  1.05 |    0.01 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **2.602 ms** |   **2.535 ms** | **0.1390 ms** |  **1.00** |    **0.06** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  25.638 ms |  15.622 ms | 0.8563 ms |  9.87 |    0.53 |  3968.7500 |  3875.0000 |  3687.5000 |  28.44 MB |       15.94 |
|          |           |            |            |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **12.378 ms** |   **9.919 ms** | **0.5437 ms** |  **1.00** |    **0.05** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 230.377 ms | 162.679 ms | 8.9170 ms | 18.64 |    0.94 | 75000.0000 | 75000.0000 | 75000.0000 | 324.33 MB |       40.00 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `levenshtein`
- `indel`
- `metrics`
- `persistence`
- `stats`
- `splitters`

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 71.6 | 12.3 | 5.82x C# faster |
| latin | 32 | 101.5 | 37.7 | 2.69x C# faster |
| latin | 128 | 288.4 | 256.9 | 1.12x C# faster |
| latin | 512 | 3022.3 | 3310.2 | 1.10x Py faster |
| cjk | 8 | 84.6 | 11.7 | 7.24x C# faster |
| cjk | 32 | 153.7 | 81.6 | 1.88x C# faster |
| cjk | 128 | 1350.9 | 845.5 | 1.60x C# faster |
| cjk | 512 | 11088.0 | 5566.1 | 1.99x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 85.2 | 9.1 | 9.41x C# faster |
| latin | 32 | 170.5 | 79.8 | 2.14x C# faster |
| latin | 128 | 1153.9 | 518.9 | 2.22x C# faster |
| latin | 512 | 9202.7 | 7349.4 | 1.25x C# faster |
| cjk | 8 | 87.1 | 9.7 | 8.99x C# faster |
| cjk | 32 | 195.4 | 116.9 | 1.67x C# faster |
| cjk | 128 | 1993.6 | 1390.8 | 1.43x C# faster |
| cjk | 512 | 17355.7 | 11383.8 | 1.52x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.002 | 0.445 | 188.54x | 0.002 | 0.445 | 188.54x |
| accuracy_n1000_k2 | 0.000 | 0.239 | 1996.23x | 0.000 | 0.238 | 1996.38x |
| precision_recall_f1_macro_n1000_k2 | 0.003 | 0.807 | 317.73x | 0.003 | 0.807 | 317.72x |
| classification_report_n1000_k2 | 0.004 | 3.156 | 825.83x | 0.004 | 3.156 | 825.86x |
| roc_auc_binary_n1000_k2 | 0.009 | 0.891 | 103.77x | 0.009 | 0.891 | 103.77x |
| balanced_accuracy_n1000_k2 | 0.003 | 0.493 | 196.97x | 0.003 | 0.493 | 196.98x |
| matthews_n1000_k2 | 0.002 | 0.937 | 375.77x | 0.002 | 0.937 | 375.77x |
| cohen_kappa_n1000_k2 | 0.002 | 0.512 | 206.66x | 0.002 | 0.512 | 206.66x |
| mse_n1000_k2 | 0.000 | 0.132 | 401.89x | 0.000 | 0.132 | 401.89x |
| mae_n1000_k2 | 0.000 | 0.131 | 419.73x | 0.000 | 0.131 | 419.68x |
| median_ae_n1000_k2 | 0.003 | 0.135 | 46.78x | 0.003 | 0.135 | 46.78x |
| r2_n1000_k2 | 0.001 | 0.153 | 184.74x | 0.001 | 0.153 | 184.74x |
| confusion_matrix_n1000_k10 | 0.002 | 0.447 | 223.52x | 0.002 | 0.447 | 223.52x |
| accuracy_n1000_k10 | 0.000 | 0.238 | 1979.62x | 0.000 | 0.238 | 1979.59x |
| precision_recall_f1_macro_n1000_k10 | 0.002 | 0.799 | 351.11x | 0.002 | 0.799 | 351.10x |
| classification_report_n1000_k10 | 0.006 | 2.998 | 534.55x | 0.006 | 2.998 | 534.57x |
| roc_auc_ovr_macro_n1000_k10 | 0.333 | 4.536 | 13.63x | 0.333 | 4.536 | 13.63x |
| balanced_accuracy_n1000_k10 | 0.002 | 0.477 | 200.71x | 0.002 | 0.476 | 200.71x |
| matthews_n1000_k10 | 0.002 | 0.895 | 383.90x | 0.002 | 0.894 | 383.91x |
| cohen_kappa_n1000_k10 | 0.003 | 0.489 | 188.68x | 0.003 | 0.489 | 188.68x |
| mse_n1000_k10 | 0.000 | 0.129 | 405.67x | 0.000 | 0.129 | 405.65x |
| mae_n1000_k10 | 0.000 | 0.124 | 397.97x | 0.000 | 0.124 | 397.96x |
| median_ae_n1000_k10 | 0.003 | 0.131 | 46.72x | 0.003 | 0.131 | 46.72x |
| r2_n1000_k10 | 0.001 | 0.153 | 185.56x | 0.001 | 0.153 | 185.56x |
| confusion_matrix_n100000_k2 | 0.229 | 6.569 | 28.66x | 0.229 | 6.569 | 28.66x |
| accuracy_n100000_k2 | 0.012 | 2.263 | 196.37x | 0.012 | 2.263 | 196.38x |
| precision_recall_f1_macro_n100000_k2 | 0.230 | 7.876 | 34.22x | 0.230 | 7.875 | 34.22x |
| classification_report_n100000_k2 | 0.232 | 16.973 | 73.27x | 0.232 | 16.973 | 73.27x |
| roc_auc_binary_n100000_k2 | 2.268 | 16.995 | 7.49x | 2.267 | 16.993 | 7.49x |
| balanced_accuracy_n100000_k2 | 0.229 | 6.777 | 29.55x | 0.229 | 6.776 | 29.55x |
| matthews_n100000_k2 | 0.232 | 13.452 | 57.90x | 0.232 | 13.451 | 57.89x |
| cohen_kappa_n100000_k2 | 0.232 | 6.606 | 28.45x | 0.232 | 6.604 | 28.45x |
| mse_n100000_k2 | 0.031 | 0.226 | 7.24x | 0.031 | 0.226 | 7.24x |
| mae_n100000_k2 | 0.031 | 0.226 | 7.39x | 0.031 | 0.226 | 7.39x |
| median_ae_n100000_k2 | 0.323 | 1.155 | 3.58x | 0.335 | 1.155 | 3.45x |
| r2_n100000_k2 | 0.078 | 0.355 | 4.58x | 0.078 | 0.355 | 4.58x |
| confusion_matrix_n100000_k10 | 0.191 | 6.722 | 35.24x | 0.191 | 6.719 | 35.23x |
| accuracy_n100000_k10 | 0.012 | 2.316 | 195.29x | 0.012 | 2.315 | 195.27x |
| precision_recall_f1_macro_n100000_k10 | 0.189 | 8.274 | 43.82x | 0.189 | 8.273 | 43.82x |
| classification_report_n100000_k10 | 0.189 | 18.387 | 97.04x | 0.189 | 18.385 | 97.03x |
| roc_auc_ovr_macro_n100000_k10 | 14.801 | 138.136 | 9.33x | 14.800 | 138.112 | 9.33x |
| balanced_accuracy_n100000_k10 | 0.195 | 6.540 | 33.48x | 0.195 | 6.539 | 33.47x |
| matthews_n100000_k10 | 0.190 | 13.687 | 72.03x | 0.190 | 13.685 | 72.02x |
| cohen_kappa_n100000_k10 | 0.191 | 6.582 | 34.55x | 0.190 | 6.581 | 34.55x |
| mse_n100000_k10 | 0.031 | 0.227 | 7.30x | 0.031 | 0.227 | 7.30x |
| mae_n100000_k10 | 0.030 | 0.222 | 7.42x | 0.030 | 0.222 | 7.42x |
| median_ae_n100000_k10 | 0.346 | 1.111 | 3.21x | 0.375 | 1.111 | 2.97x |
| r2_n100000_k10 | 0.077 | 0.348 | 4.55x | 0.077 | 0.348 | 4.55x |
| confusion_matrix_n1000000_k2 | 2.314 | 63.068 | 27.26x | 2.314 | 63.062 | 27.26x |
| accuracy_n1000000_k2 | 0.121 | 20.853 | 171.93x | 0.121 | 20.851 | 171.92x |
| precision_recall_f1_macro_n1000000_k2 | 2.285 | 69.635 | 30.47x | 2.285 | 69.624 | 30.47x |
| classification_report_n1000000_k2 | 2.277 | 140.998 | 61.91x | 2.277 | 140.970 | 61.91x |
| roc_auc_binary_n1000000_k2 | 35.921 | 192.620 | 5.36x | 35.917 | 192.597 | 5.36x |
| balanced_accuracy_n1000000_k2 | 2.306 | 62.856 | 27.25x | 2.306 | 62.831 | 27.25x |
| matthews_n1000000_k2 | 2.326 | 127.207 | 54.69x | 2.326 | 127.196 | 54.68x |
| cohen_kappa_n1000000_k2 | 2.243 | 63.054 | 28.11x | 2.243 | 63.046 | 28.11x |
| mse_n1000000_k2 | 0.309 | 1.568 | 5.07x | 0.309 | 1.567 | 5.07x |
| mae_n1000000_k2 | 0.301 | 1.562 | 5.19x | 0.301 | 1.562 | 5.19x |
| median_ae_n1000000_k2 | 3.285 | 9.757 | 2.97x | 3.370 | 9.755 | 2.89x |
| r2_n1000000_k2 | 0.772 | 2.550 | 3.30x | 0.772 | 2.550 | 3.30x |
| confusion_matrix_n1000000_k10 | 1.809 | 60.111 | 33.22x | 1.809 | 60.086 | 33.21x |
| accuracy_n1000000_k10 | 0.122 | 20.691 | 169.83x | 0.122 | 20.688 | 169.82x |
| precision_recall_f1_macro_n1000000_k10 | 1.813 | 72.442 | 39.95x | 1.813 | 72.433 | 39.95x |
| classification_report_n1000000_k10 | 1.803 | 152.271 | 84.44x | 1.803 | 152.260 | 84.44x |
| balanced_accuracy_n1000000_k10 | 1.809 | 62.563 | 34.59x | 1.809 | 62.556 | 34.59x |
| matthews_n1000000_k10 | 1.801 | 128.996 | 71.62x | 1.801 | 128.972 | 71.61x |
| cohen_kappa_n1000000_k10 | 1.810 | 62.688 | 34.63x | 1.810 | 62.686 | 34.63x |
| mse_n1000000_k10 | 0.320 | 1.625 | 5.07x | 0.320 | 1.624 | 5.07x |
| mae_n1000000_k10 | 0.311 | 1.619 | 5.21x | 0.311 | 1.619 | 5.21x |
| median_ae_n1000000_k10 | 3.644 | 9.788 | 2.69x | 3.883 | 9.786 | 2.52x |
| r2_n1000000_k10 | 0.770 | 2.533 | 3.29x | 0.770 | 2.532 | 3.29x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 3.309 | 5.875 | 1.78x | 3.838 | 5.875 | 1.53x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 7.629 | 9.448 | 1.24x | 7.978 | 9.447 | 1.18x | 706,526 | 706,526 |
| tokenizer_json_unigram | 7.604 | 28.637 | 3.77x | 7.886 | 28.633 | 3.63x | 1,990,038 | 1,990,038 |
| spiece_model | 4.941 | 19.105 | 3.87x | 5.194 | 19.105 | 3.68x | 533,084 | 533,084 |
| tfidf_save | 1.205 | 1.541 | 1.28x | 1.234 | 1.541 | 1.25x | 581,787 | 591,922 |
| tfidf_load | 3.434 | 2.677 | 0.78x | 3.620 | 2.677 | 0.74x | 581,787 | 591,922 |
| embedding_index_save | 1.964 | 0.892 | 0.45x | 2.228 | 0.892 | 0.40x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 74.879 | 54.838 | 0.73x | 7.964 | 4.267 | 0.54x | 20,589,007 | 15,360,128 |
| embedding_index_load | 3.149 | 0.575 | 0.18x | 3.403 | 0.575 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 3.129 | 0.439 | 0.14x | 3.393 | 0.439 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 2.508 | 0.572 | 0.23x | 2.667 | 0.572 | 0.21x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 0.984 | 0.574 | 0.58x | 1.179 | 0.574 | 0.49x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.000 | 63.29x | 0.000 | 0.000 | 63.29x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 296.863 | 417.457 | 1.41x | 299.061 | 417.426 | 1.40x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 48.424 | 48.765 | 1.01x | 48.722 | 48.761 | 1.00x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.042 | 0.061 | 1.46x | 0.042 | 0.061 | 1.46x |
| stratified_n10000 | 0.092 | 0.420 | 4.55x | 0.092 | 0.420 | 4.55x |
| traintest_n10000 | 0.003 | 0.100 | 33.84x | 0.003 | 0.100 | 33.79x |
| kfold_seeded_n10000 | 0.174 | 0.409 | 2.34x | 0.174 | 0.409 | 2.34x |
| stratified_seeded_n10000 | 0.173 | 0.835 | 4.84x | 0.173 | 0.835 | 4.84x |
| traintest_seeded_n10000 | 0.038 | 0.226 | 5.90x | 0.038 | 0.226 | 5.90x |
| stratified_traintest_n10000 | 0.500 | 1.522 | 3.04x | 0.500 | 1.522 | 3.04x |
| group_kfold_n10000 | 0.062 | 0.306 | 4.93x | 0.062 | 0.306 | 4.93x |
| stratified_group_kfold_n10000 | 0.130 | 14.471 | 111.04x | 0.130 | 14.470 | 111.02x |
| timeseries_n10000 | 0.000 | 0.007 | 90.84x | 0.000 | 0.007 | 90.84x |
| repeated_kfold_n10000 | 0.690 | 1.237 | 1.79x | 0.690 | 1.237 | 1.79x |
| kfold_n100000 | 0.684 | 1.879 | 2.75x | 0.777 | 1.879 | 2.42x |
| stratified_n100000 | 1.171 | 4.566 | 3.90x | 1.282 | 4.565 | 3.56x |
| traintest_n100000 | 0.154 | 0.206 | 1.34x | 0.154 | 0.206 | 1.33x |
| kfold_seeded_n100000 | 2.663 | 5.135 | 1.93x | 2.783 | 5.135 | 1.85x |
| stratified_seeded_n100000 | 3.122 | 7.848 | 2.51x | 3.239 | 7.848 | 2.42x |
| traintest_seeded_n100000 | 1.559 | 1.127 | 0.72x | 1.586 | 1.127 | 0.71x |
| stratified_traintest_n100000 | 6.260 | 13.758 | 2.20x | 6.326 | 13.757 | 2.17x |
| group_kfold_n100000 | 0.843 | 4.276 | 5.07x | 0.932 | 4.276 | 4.59x |
| stratified_group_kfold_n100000 | 1.555 | 142.592 | 91.69x | 1.649 | 142.573 | 86.45x |
| timeseries_n100000 | 0.000 | 0.029 | 372.46x | 0.000 | 0.029 | 372.47x |
| repeated_kfold_n100000 | 7.300 | 14.015 | 1.92x | 7.412 | 14.014 | 1.89x |
| kfold_n1000000 | 5.262 | 11.409 | 2.17x | 5.666 | 11.407 | 2.01x |
| stratified_n1000000 | 11.085 | 44.144 | 3.98x | 11.540 | 44.141 | 3.82x |
| traintest_n1000000 | 0.313 | 1.172 | 3.75x | 0.353 | 1.172 | 3.32x |
| kfold_seeded_n1000000 | 27.516 | 44.021 | 1.60x | 28.363 | 44.016 | 1.55x |
| stratified_seeded_n1000000 | 35.202 | 74.213 | 2.11x | 36.510 | 74.206 | 2.03x |
| traintest_seeded_n1000000 | 15.437 | 10.082 | 0.65x | 15.577 | 10.080 | 0.65x |
| stratified_traintest_n1000000 | 74.531 | 147.843 | 1.98x | 74.747 | 147.834 | 1.98x |
| group_kfold_n1000000 | 7.787 | 34.980 | 4.49x | 8.257 | 34.973 | 4.24x |
| stratified_group_kfold_n1000000 | 16.695 | 1454.660 | 87.13x | 17.740 | 1454.542 | 81.99x |
| timeseries_n1000000 | 0.000 | 0.245 | 3897.22x | 0.000 | 0.245 | 3896.96x |
| repeated_kfold_n1000000 | 78.297 | 135.254 | 1.73x | 80.046 | 135.226 | 1.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.003 | 0.334 | 122.14x | 0.003 | 0.334 | 122.11x |
| mann_whitney_n1000 | 0.019 | 0.286 | 14.74x | 0.019 | 0.285 | 14.72x |
| chi_square_n1000 | 0.000 | 0.139 | 1538.34x | 0.000 | 0.139 | 1538.25x |
| fligner_n1000 | 0.039 | 0.312 | 8.07x | 0.039 | 0.311 | 8.07x |
| anderson_ksamp_n1000 | 0.037 | 0.175 | 4.72x | 0.037 | 0.175 | 4.72x |
| spearman_matrix_n1000 | 0.050 | 0.309 | 6.16x | 0.050 | 0.308 | 6.16x |
| pointbiserial_n1000 | 0.006 | 0.241 | 38.62x | 0.006 | 0.241 | 38.62x |
| welch_t_n10000 | 0.025 | 0.355 | 14.12x | 0.025 | 0.355 | 14.12x |
| mann_whitney_n10000 | 0.288 | 1.549 | 5.37x | 0.288 | 1.549 | 5.37x |
| chi_square_n10000 | 0.001 | 0.135 | 198.34x | 0.001 | 0.135 | 198.32x |
| fligner_n10000 | 0.644 | 1.878 | 2.92x | 0.684 | 1.878 | 2.75x |
| anderson_ksamp_n10000 | 0.959 | 2.364 | 2.47x | 1.017 | 2.364 | 2.33x |
| spearman_matrix_n10000 | 0.896 | 2.748 | 3.07x | 0.923 | 2.748 | 2.98x |
| pointbiserial_n10000 | 0.057 | 0.265 | 4.61x | 0.057 | 0.265 | 4.61x |
| welch_t_n100000 | 0.253 | 0.580 | 2.29x | 0.253 | 0.580 | 2.29x |
| mann_whitney_n100000 | 3.272 | 16.297 | 4.98x | 3.272 | 16.295 | 4.98x |
| chi_square_n100000 | 0.008 | 0.144 | 18.27x | 0.008 | 0.144 | 18.27x |
| fligner_n100000 | 7.911 | 20.249 | 2.56x | 8.385 | 20.247 | 2.41x |
| anderson_ksamp_n100000 | 8.076 | 26.955 | 3.34x | 8.417 | 26.951 | 3.20x |
| spearman_matrix_n100000 | 8.038 | 32.977 | 4.10x | 8.501 | 32.974 | 3.88x |
| pointbiserial_n100000 | 0.881 | 0.680 | 0.77x | 0.889 | 2.713 | 3.05x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `BkTreeNearestBenchmarks`
- `ChainedProductBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `ClusteringAgreementBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `DecompositionWidthBenchmarks`
- `DistributionIncumbentBenchmarks`
- `DistributionTailBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `EncoderIncumbentBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `GlmBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `HouseholderQrBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `KsExactTableBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MannWhitneyExactBenchmarks`
- `MathNetInteropBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `MultinomialLogitBenchmarks`
- `OlsBenchmarks`
- `OsaBenchmarks`
- `PartitionValidityBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `ProcessExtractBenchmarks`
- `QgramBenchmarks`
- `QuantileBenchmarks`
- `RankTestBenchmarks`
- `RankingMetricsBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RobustCovarianceBenchmarks`
- `RobustScalerSparseBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `SerialCorrelationBenchmarks`
- `SilhouetteBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorStoreChurnBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Comparisons selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `cdist`
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
- `transformers`
- `var`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 22.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `LcsGateBenchmarks` | Band=64 | `Kernel` | step | 0.05 | 0.03 | +67% | 30% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=4 | `Lodestar_HeldPattern` | step | 1.6 | 1.19 | +34% | 30% |
| `LcsGateBenchmarks` | Band=96 | `Kernel` | drift | 0.04 | 0.03 | +33% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | drift | 6.678 | 5.026 | +33% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.82 | 1.19 | -30% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel` | drift | 0.07 | 0.1 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 2.23 | 3.06 | -27% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=AccuracyAlone | `MlNet` | drift | 4003.72 | 3161.99 | +25% | 20% |
| `MyersGateBenchmarks` | Band=32 | `Kernel_Cjk` | drift | 0.15 | 0.2 | -25% | 20% |
| `VectorMathBenchmarks` | Dim=768 | `L2Norm` | drift | 0.69 | 0.92 | -25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `SignaturesOnly` | drift | 0.03 | 0.04 | -25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `FingerprintsOnly` | drift | 0.05 | 0.04 | +25% | 20% |
| `VectorMathBenchmarks` | Dim=1024 | `L2Norm` | drift | 0.71 | 0.94 | -24% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=1500, Method=complete | `Aglomera_GetClustering` | drift | 114.64 | 148.9 | -23% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=500, Method=ward | `Aglomera_GetClustering` | drift | 28.38 | 38.8 | -22% | 20% |
| `BpeBenchmarks` | — | `Bpe` | drift | 3.38 | 2.77 | +22% | 20% |
| `StopWordBenchmarks` | Documents=200 | `CountWithStopWords` | drift | 0.69 | 0.88 | -22% | 20% |
| `SimilaritySketchBenchmarks` | Documents=500, Permutations=64 | `AffineSignaturesOnly` | drift | 0.11 | 0.14 | -21% | 20% |
| `SimilaritySketchBenchmarks` | Documents=500, Permutations=128 | `AffineSignaturesOnly` | drift | 0.11 | 0.14 | -21% | 20% |
| `Bm25Benchmarks` | Documents=1000 | `LodestarQueryMultiTerm` | drift | 1.54 | 1.26 | +21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | drift | 0.97 | 1.22 | -20% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `SignaturesOnly` | drift | 0.04 | 0.05 | -20% | 20% |

**Still away from their median: 38.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | drift | 11.95 | 0.91 | +1213% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | drift | 34.6 | 7.64 | +353% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | drift | 19.91 | 4.53 | +340% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | drift | 13.94 | 3.92 | +256% | 20% |
| `VectorizerBenchmarks` | Documents=200 | `CountBigrams` | drift | 3.6 | 1.19 | +198% | 20% |
| `VectorizerBenchmarks` | Documents=1000 | `CountCharWordBoundary` | drift | 8.98 | 2.94 | +193% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | drift | 19.99 | 7.01 | +185% | 20% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | drift | 14.37 | 6.98 | +106% | 20% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | drift | 12.04 | 7.04 | +71% | 20% |
| `VectorizerBenchmarks` | Documents=1000 | `CountBigrams` | drift | 2.51 | 1.45 | +66% | 20% |
| `LcsGateBenchmarks` | Band=16 | `Kernel` | step | 0.09 | 0.21 | -57% | 35% |
| `LcsGateBenchmarks` | Band=16 | `Kernel_Cjk` | step | 0.15 | 0.33 | -55% | 30% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 3.12 | 2.19 | +47% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.25 | 2.9 | +47% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_AndersonDarling` | step | 5.102 | 3.544 | +44% | 30% |
| `Bm25Benchmarks` | Documents=1000 | `LodestarFromText` | drift | 2441.19 | 3973.42 | -40% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Accord_Levene` | step | 4.731 | 3.408 | +39% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | drift | 1.43 | 2.28 | -37% | 20% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | drift | 1.33 | 2.04 | -37% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | drift | 1.417 | 2.243 | -37% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | drift | 0.002 | 0.003 | -33% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | drift | 0.1 | 0.15 | -33% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `AffineSignaturesOnly` | step | 0.02 | 0.03 | -33% | 30% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `AffineSignaturesOnly` | step | 0.02 | 0.03 | -33% | 30% |
| `LevenshteinBenchmarks` | Length=8 | `Distance_CodePoint` | step | 6.53 | 4.93 | +32% | 30% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=16 | `Lodestar_HeldPattern` | step | 1.36 | 1.03 | +32% | 30% |
| `Bm25Benchmarks` | Documents=20000 | `LodestarFromText` | drift | 3297.19 | 4742.68 | -30% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.14 | 1.64 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 2.17 | 3.06 | -29% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.04 | 1.43 | -26% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.69 | 1.35 | +25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `FingerprintsOnly` | drift | 0.04 | 0.04 | +25% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 1.1 | 1.38 | -25% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 1.05 | 1.39 | -24% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 9.05 | 6.21 | +23% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel` | drift | 0.1 | 0.13 | -23% | 20% |
| `MyersGateBenchmarks` | Band=64 | `Kernel_Cjk` | drift | 0.08 | 0.09 | -22% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.23 | 0.19 | +21% | 20% |
