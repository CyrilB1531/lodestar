# Nightly benchmark run

<!-- nightly-baseline: c4a19d86a5f8c9899f169f26ac7e107de05eedd1 -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks PartitionValidityBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks SimilaritySketchBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks VectorAutoregressionBenchmarks VectorMathBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks -->
<!-- nightly-cut: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`
- Previous run: `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`
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

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error           | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **982.0 ns** |        **20.88 ns** |       **1.14 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         800.3 ns |        20.77 ns |       1.14 ns |     0.81 |    0.00 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,756.4 ns |        93.48 ns |       5.12 ns |     2.81 |    0.01 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,723.7 ns |       154.26 ns |       8.46 ns |     3.79 |    0.01 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,461.2 ns |       726.01 ns |      39.80 ns |     5.56 |    0.04 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      11,474.3 ns |     2,902.71 ns |     159.11 ns |    11.68 |    0.14 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,330.7 ns |       308.51 ns |      16.91 ns |     5.43 |    0.02 |        - |        - |        - |         - |          NA |
|                                |           |                  |                 |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,083.8 ns** |     **1,155.65 ns** |      **63.34 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,773.8 ns |     2,077.98 ns |     113.90 ns |     0.66 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,191,175.9 ns |   119,498.95 ns |   6,550.14 ns |    13.84 |    0.07 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,953,745.6 ns |    62,918.76 ns |   3,448.79 ns |    22.70 |    0.04 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,152,574.2 ns |    23,906.77 ns |   1,310.41 ns |    25.01 |    0.02 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,965,458.8 ns | 8,080,434.33 ns | 442,915.87 ns | 3,356.79 |    4.94 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,132,623.2 ns |    43,762.14 ns |   2,398.75 ns |    24.77 |    0.03 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.34 μs** |   **2.516 μs** |  **0.138 μs** |  **1.00** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    34.26 μs |   4.514 μs |  0.247 μs |  1.09 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,009.45 μs** | **254.682 μs** | **13.960 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,458.37 μs |  58.193 μs |  3.190 μs |  0.73 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **997.3 ns** |      **85.35 ns** |      **4.68 ns** |   **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     2,194.5 ns |     202.36 ns |     11.09 ns |   2.20 |    0.01 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,335.0 ns |      68.64 ns |      3.76 ns |   2.34 |    0.01 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     4,072.9 ns |     707.91 ns |     38.80 ns |   4.08 |    0.04 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     7,467.2 ns |     211.99 ns |     11.62 ns |   7.49 |    0.03 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     9,422.9 ns |   1,347.26 ns |     73.85 ns |   9.45 |    0.07 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,604.2 ns |      26.20 ns |      1.44 ns |   2.61 |    0.01 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,867.7 ns |     262.83 ns |     14.41 ns |   3.88 |    0.02 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       971.8 ns |     148.42 ns |      8.14 ns |   0.97 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     3,181.3 ns |     125.22 ns |      6.86 ns |   3.19 |    0.01 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |     1,183.2 ns |     122.79 ns |      6.73 ns |   1.19 |    0.01 | 0.0477 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,092.6 ns |     182.76 ns |     10.02 ns |   1.10 |    0.01 | 0.0572 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       459.8 ns |       1.76 ns |      0.10 ns |   0.46 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,551.7 ns |      43.73 ns |      2.40 ns |   1.56 |    0.01 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |       121.6 ns |       5.11 ns |      0.28 ns |   0.12 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       705.1 ns |      34.26 ns |      1.88 ns |   0.71 |    0.00 | 0.0572 |     968 B |          NA |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,490.2 ns** |   **3,676.73 ns** |    **201.53 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   178,854.8 ns |   1,936.62 ns |    106.15 ns |  2.957 |    0.01 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   701,142.4 ns |  23,963.15 ns |  1,313.50 ns | 11.591 |    0.04 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 2,061,651.2 ns | 180,731.37 ns |  9,906.50 ns | 34.083 |    0.17 | 3.9063 |   80382 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,478,645.2 ns |  56,908.78 ns |  3,119.36 ns | 24.445 |    0.08 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,609,974.1 ns | 269,768.71 ns | 14,786.93 ns | 59.679 |    0.27 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,453,929.3 ns |  54,643.12 ns |  2,995.17 ns | 24.036 |    0.08 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,142,112.0 ns | 113,013.29 ns |  6,194.64 ns | 35.413 |    0.14 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    84,655.5 ns |     347.75 ns |     19.06 ns |  1.400 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   267,898.0 ns |  11,613.55 ns |    636.58 ns |  4.429 |    0.02 |      - |     745 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   192,503.9 ns |   4,389.91 ns |    240.63 ns |  3.182 |    0.01 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   381,465.5 ns |  31,378.70 ns |  1,719.97 ns |  6.306 |    0.03 | 4.3945 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       461.7 ns |       5.79 ns |      0.32 ns |  0.008 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,549.5 ns |      53.79 ns |      2.95 ns |  0.026 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |       116.6 ns |       2.20 ns |      0.12 ns |  0.002 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       702.6 ns |       8.62 ns |      0.47 ns |  0.012 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean           | Error        | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|-------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,182.4 ns** |    **267.84 ns** |    **14.68 ns** |  **1.00** |    **0.01** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,069.7 ns |    272.76 ns |    14.95 ns |  1.86 |    0.01 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       607.5 ns |     60.11 ns |     3.29 ns |  0.28 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,188.3 ns |     54.32 ns |     2.98 ns |  0.54 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       765.3 ns |     29.25 ns |     1.60 ns |  0.35 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,614.3 ns |    230.00 ns |    12.61 ns |  2.11 |    0.01 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,649.7 ns |    396.97 ns |    21.76 ns |  3.96 |    0.02 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,285.4 ns |    293.16 ns |    16.07 ns |  2.88 |    0.02 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,138.2 ns |    500.07 ns |    27.41 ns |  1.90 |    0.02 |       - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **393,909.5 ns** | **17,182.40 ns** |   **941.83 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,342,490.6 ns | 55,474.71 ns | 3,040.76 ns | 3.408 |    0.01 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,284.5 ns |  1,253.83 ns |    68.73 ns | 0.143 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,623.8 ns |  3,042.56 ns |   166.77 ns | 0.286 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,004.3 ns |    154.16 ns |     8.45 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   897,690.3 ns | 74,905.10 ns | 4,105.80 ns | 2.279 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,395,863.2 ns | 70,702.66 ns | 3,875.45 ns | 3.544 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,897.2 ns |  2,062.80 ns |   113.07 ns | 0.028 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **2,027.5 μs** |     **279.16 μs** |     **15.30 μs** |   **1.00** |    **0.01** |   **246.0938** |   **246.0938** |  **246.0938** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    97,528.9 μs |   5,842.93 μs |    320.27 μs |  48.10 |    0.34 |  3666.6667 |  1500.0000 |  833.3333 |  59880.37 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,958.6 μs** |     **631.28 μs** |     **34.60 μs** |   **1.00** |    **0.02** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    94,772.3 μs |  28,586.43 μs |  1,566.92 μs |  48.40 |    1.01 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **782.0 μs** |      **57.33 μs** |      **3.14 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   140,326.3 μs |  68,114.65 μs |  3,733.59 μs | 179.44 |    4.18 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,295.5 μs** |     **721.81 μs** |     **39.56 μs** |   **1.00** |    **0.02** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    70,389.6 μs |   8,258.45 μs |    452.67 μs |  30.67 |    0.49 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,471.0 μs** |   **1,149.08 μs** |     **62.98 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,606,247.3 μs | 147,506.78 μs |  8,085.34 μs | 103.82 |    0.58 | 35000.0000 | 10000.0000 | 4000.0000 | 537153.95 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,198.2 μs** |   **8,011.87 μs** |    **439.16 μs** |   **1.00** |    **0.04** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,729,403.2 μs | 420,583.36 μs | 23,053.59 μs | 121.88 |    3.55 | 48000.0000 | 12000.0000 | 4000.0000 | 747353.12 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,504.8 μs** |     **281.81 μs** |     **15.45 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,060,153.2 μs | 156,340.71 μs |  8,569.56 μs | 470.45 |    1.50 | 47000.0000 | 12000.0000 | 4000.0000 | 732672.81 KB |    8,147.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,965.0 μs** |   **4,500.72 μs** |    **246.70 μs** |   **1.00** |    **0.02** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,488,568.1 μs |  26,487.50 μs |  1,451.87 μs |  82.87 |    0.99 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 24.96 ms |  6.285 ms | 0.344 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 49.52 ms | 16.336 ms | 0.895 ms | 1416.6667 | 1333.3333 | 583.3333 |  43.93 MB |
| CountLoad4M      | 45.39 ms |  1.911 ms | 0.105 ms | 1500.0000 | 1416.6667 | 666.6667 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error         | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 186,399.35 μs | 12,500.680 μs |   685.204 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     440.05 μs |     27.125 μs |     1.487 μs |    9.2773 |    2.9297 |   2.9297 |   504.67 KB |
| CountSaveAsync240K    |     510.84 μs |    173.037 μs |     9.485 μs |    9.7656 |    3.9063 |   3.9063 |   739.44 KB |
| TfidfSave45K          |      51.97 μs |      3.846 μs |     0.211 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      54.93 μs |     12.270 μs |     0.673 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     290.40 μs |     23.656 μs |     1.297 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     294.89 μs |     37.884 μs |     2.077 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,775.63 μs | 20,068.712 μs | 1,100.034 μs |  593.7500 |  562.5000 | 187.5000 | 22049.02 KB |
| CountLoadAsync4M      |  55,025.31 μs | 70,307.932 μs | 3,853.815 μs | 1181.8182 | 1090.9091 | 363.6364 | 44983.59 KB |
| CountLoad4M           |  51,591.98 μs | 20,331.823 μs | 1,114.456 μs | 1181.8182 | 1090.9091 | 363.6364 | 44976.85 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error         | StdDev      | Gen0      | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|--------------:|------------:|----------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     18.631 μs |     24.830 μs |   1.3610 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    516.619 μs |     91.816 μs |   5.0327 μs |   35.1563 |  29.2969 |  29.2969 |   740.15 KB |
| HashingLoadUndeclared  |      6.611 μs |      3.193 μs |   0.1750 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     31.536 μs |      1.337 μs |   0.0733 μs |    7.3853 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    136.218 μs |      3.359 μs |   0.1841 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    867.042 μs |    308.767 μs |  16.9246 μs |   38.0859 |  37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,832.964 μs | 12,111.678 μs | 663.8820 μs |  453.1250 | 437.5000 | 234.3750 | 10793.89 KB |
| CountSaveAsync1M       |  2,110.815 μs |    513.322 μs |  28.1369 μs |  117.1875 | 109.3750 | 109.3750 |  3162.12 KB |
| IndexSaveAsyncLongHead |  3,377.344 μs |    264.581 μs |  14.5026 μs |   93.7500 |  89.8438 |  89.8438 |  2124.36 KB |
| CountLoadUndeclared2M  | 19,503.556 μs |  3,799.699 μs | 208.2743 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.12 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.605 μs** |  **0.9494 μs** | **0.0520 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.156 μs |  0.2094 μs | 0.0115 μs |  1.10 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   6.045 μs |  0.3528 μs | 0.0193 μs |  1.08 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **57.671 μs** |  **5.8142 μs** | **0.3187 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.639 μs |  3.7465 μs | 0.2054 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  22.524 μs |  2.2602 μs | 0.1239 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **224.788 μs** | **19.0373 μs** | **1.0435 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  80.585 μs |  1.3626 μs | 0.0747 μs |  0.36 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  71.907 μs |  8.6723 μs | 0.4754 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **911.706 μs** | **71.8877 μs** | **3.9404 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 322.532 μs | 31.8883 μs | 1.7479 μs |  0.35 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 277.280 μs | 32.1953 μs | 1.7647 μs |  0.30 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **132.67 ms** |  **2.245 ms** | **0.123 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.27 ms |  4.086 ms | 0.224 ms |  0.49 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **139.44 ms** | **74.856 ms** | **4.103 ms** |  **1.00** |    **0.04** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  61.05 ms |  1.864 ms | 0.102 ms |  0.44 |    0.01 |  116.47 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **201.52 ms** | **17.681 ms** | **0.969 ms** |  **1.00** |    **0.01** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 246.28 ms | 14.772 ms | 0.810 ms |  1.22 |    0.01 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **206.37 ms** | **14.937 ms** | **0.819 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 221.36 ms | 60.957 ms | 3.341 ms |  1.07 |    0.01 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **253.27 ms** |  **8.875 ms** | **0.486 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 345.43 ms | 86.158 ms | 4.723 ms |  1.36 |    0.02 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **257.45 ms** | **16.137 ms** | **0.885 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 316.18 ms | 25.985 ms | 1.424 ms |  1.23 |    0.01 | 1152.13 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **298.61 ms** | **13.973 ms** | **0.766 ms** |  **1.00** |    **0.00** | **5112.74 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 400.94 ms | 83.414 ms | 4.572 ms |  1.34 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **299.98 ms** |  **3.227 ms** | **0.177 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 392.23 ms | 66.194 ms | 3.628 ms |  1.31 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **44.89 μs** |     **2.032 μs** |   **0.111 μs** |         **-** |
| Cjk    | 1000   |      63.36 μs |     0.852 μs |   0.047 μs |         - |
| **Latin**  | **10000**  |   **4,098.66 μs** |   **174.908 μs** |   **9.587 μs** |         **-** |
| Cjk    | 10000  |   6,901.24 μs |   428.863 μs |  23.507 μs |         - |
| **Latin**  | **65536**  | **184,158.32 μs** | **5,101.241 μs** | **279.616 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 28.25 ms |  1.484 ms | 0.081 ms |  1.00 |    0.00 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 86.79 ms | 19.052 ms | 1.044 ms |  3.07 |    0.03 | 1666.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **18.73 μs** |  **1.046 μs** | **0.057 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **66.48 μs** | **16.322 μs** | **0.895 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **181.66 μs** | **17.199 μs** | **0.943 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **376.51 μs** | **15.839 μs** | **0.868 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev   | Allocated |
|------------------ |---------:|---------:|---------:|----------:|
| EncodeUnseenProse | 11.71 ms | 71.13 ms | 3.899 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error      | StdDev    | Allocated |
|----------- |--------- |-----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **19.123 μs** |  **0.4099 μs** | **0.0225 μs** |         **-** |
| MyersGroup | cjk      | 244.908 μs | 70.2295 μs | 3.8495 μs |         - |
| **DpGroup**    | **latin**    |   **9.864 μs** |  **2.3569 μs** | **0.1292 μs** |         **-** |
| MyersGroup | latin    | 130.348 μs |  3.4830 μs | 0.1909 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **305.55 μs** |   **9.550 μs** | **0.523 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 312.27 μs |  43.411 μs | 2.380 μs |  1.02 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **335.70 μs** |  **27.194 μs** | **1.491 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 338.50 μs |   8.817 μs | 0.483 μs |  1.01 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **321.76 μs** |  **15.198 μs** | **0.833 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 301.32 μs |  13.670 μs | 0.749 μs |  0.94 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **267.48 μs** |  **13.120 μs** | **0.719 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 323.59 μs |  10.105 μs | 0.554 μs |  1.21 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **288.05 μs** |  **61.685 μs** | **3.381 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 292.33 μs |   8.382 μs | 0.459 μs |  1.01 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **179.77 μs** |  **11.915 μs** | **0.653 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 308.01 μs |  48.279 μs | 2.646 μs |  1.71 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **234.74 μs** | **154.168 μs** | **8.450 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 298.12 μs |  49.645 μs | 2.721 μs |  1.27 |    0.04 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.28 μs** |  **19.974 μs** | **1.095 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 327.20 μs |  14.834 μs | 0.813 μs |  3.67 |    0.04 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **181.7 μs** |  **18.78 μs** |  **1.03 μs** |  **1.00** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   166.0 μs |  13.93 μs |  0.76 μs |  0.91 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   473.4 μs |  16.68 μs |  0.91 μs |  2.61 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,263.0 μs** |  **48.75 μs** |  **2.67 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,144.0 μs | 286.09 μs | 15.68 μs |  0.96 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,705.9 μs | 164.91 μs |  9.04 μs |  2.36 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **153.55 ms** | **38.585 ms** | **2.115 ms** |  **1.00** |    **0.02** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,334.95 ms | 45.005 ms | 2.467 ms | 15.21 |    0.18 | 14000.0000 | 8000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |           |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **53.99 ms** |  **1.331 ms** | **0.073 ms** |  **1.00** |    **0.00** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   843.49 ms | 46.789 ms | 2.565 ms | 15.62 |    0.05 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **162.50 ms** |  **14.871 ms** | **0.815 ms** |  **1.00** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 7,247.90 ms |  60.220 ms | 3.301 ms | 44.60 |    0.19 | 38000.0000 | 14000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,949.23 ms | 141.236 ms | 7.742 ms | 24.30 |    0.11 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |          |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **19.47 ms** |   **3.647 ms** | **0.200 ms** |  **1.00** |    **0.01** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   475.15 ms |  42.859 ms | 2.349 ms | 24.40 |    0.24 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   267.21 ms |  30.780 ms | 1.687 ms | 13.72 |    0.14 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  13.196 ms |  1.2151 ms | 0.0666 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 110.545 ms | 23.7607 ms | 1.3024 ms |  8.38 |    0.09 |
| Nmf_Transform                             |   3.377 ms |  0.1393 ms | 0.0076 ms |  0.26 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.632 ms |  0.8377 ms | 0.0459 ms |  1.87 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.41 ns |   1.520 ns |  0.083 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   748.05 ns |  55.161 ns |  3.024 ns |  7.68 |    0.03 |      - |         - |          NA |
| TokenSortRatio |   859.10 ns |  80.903 ns |  4.435 ns |  8.82 |    0.04 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 1,026.92 ns |  54.122 ns |  2.967 ns | 10.54 |    0.03 | 0.0534 |     896 B |          NA |
| WRatio         | 1,289.23 ns | 258.175 ns | 14.151 ns | 13.24 |    0.13 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error             | StdDev         | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|---------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,018,643.8405 ns |   315,269.4613 ns | 17,280.9831 ns |  1.000 |    0.01 |  7.8125 |       - |  160507 B |        1.00 |
| TokenSortRatio_EmojiWords |    672,003.0628 ns |    36,822.1243 ns |  2,018.3449 ns |  0.333 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,116,574.3701 ns |   116,900.1349 ns |  6,407.6909 ns |  0.553 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,519,469.0000 ns | 1,556,064.8720 ns | 85,293.1667 ns | 34.440 |    0.26 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |      0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.73 ns** |   **1.473 ns** |  **0.081 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    207.29 ns |   5.276 ns |  0.289 ns |  2.10 |    0.00 | 0.0048 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **774.18 ns** |   **6.754 ns** |  **0.370 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,567.79 ns | 824.857 ns | 45.213 ns | 13.65 |    0.05 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,003.38 ns** |  **50.407 ns** |  **2.763 ns** |  **1.00** |    **0.00** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,135.34 ns |  92.496 ns |  5.070 ns |  2.13 |    0.01 | 0.1144 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,213.79 ns** |  **15.238 ns** |  **0.835 ns** |  **1.00** |    **0.00** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,980.36 ns | 159.632 ns |  8.750 ns |  4.10 |    0.01 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.61 ns** |   **0.259 ns** |  **0.014 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.27 ns |   0.680 ns |  0.037 ns |  1.55 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    30.04 ns |   1.079 ns |  0.059 ns |  1.13 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.70 ns |   0.387 ns |  0.021 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **27.86 ns** |   **0.688 ns** |  **0.038 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    43.03 ns |   2.097 ns |  0.115 ns |  1.54 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.80 ns |   0.313 ns |  0.017 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    28.06 ns |   1.406 ns |  0.077 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **30.63 ns** |   **0.508 ns** |  **0.028 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.59 ns |   1.706 ns |  0.094 ns |  1.49 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    31.59 ns |   0.579 ns |  0.032 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.75 ns |   0.316 ns |  0.017 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **36.90 ns** |   **1.316 ns** |  **0.072 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    47.99 ns |   5.376 ns |  0.295 ns |  1.30 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.34 ns |   0.281 ns |  0.015 ns |  0.96 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    47.24 ns |  28.514 ns |  1.563 ns |  1.28 |    0.04 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **58.87 ns** |   **4.109 ns** |  **0.225 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    88.74 ns |  15.501 ns |  0.850 ns |  1.51 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.73 ns |   0.728 ns |  0.040 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    62.79 ns |   1.480 ns |  0.081 ns |  1.07 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **62.55 ns** |   **0.728 ns** |  **0.040 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.51 ns |   0.903 ns |  0.049 ns |  1.11 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    65.90 ns |   0.683 ns |  0.037 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    61.65 ns |   1.983 ns |  0.109 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **407.31 ns** |   **3.459 ns** |  **0.190 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   416.85 ns |  26.544 ns |  1.455 ns |  1.02 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   406.62 ns |   0.192 ns |  0.011 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   405.64 ns |  21.475 ns |  1.177 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,687.27 ns** | **150.803 ns** |  **8.266 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,815.24 ns | 369.234 ns | 20.239 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,611.45 ns |  62.192 ns |  3.409 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,653.73 ns |  17.825 ns |  0.977 ns |  0.99 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **398.1 ns** |    **14.24 ns** |  **0.78 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    190.0 ns |     1.81 ns |  0.10 ns |  0.48 |         - |          NA |
|                    |        |             |             |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,406.3 ns** |    **68.03 ns** |  **3.73 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,041.9 ns |    21.12 ns |  1.16 ns |  1.26 |         - |          NA |
|                    |        |             |             |          |       |           |             |
| **Distance_CodePoint** | **512**    | **13,098.2 ns** |    **38.55 ns** |  **2.11 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,348.4 ns | 1,534.23 ns | 84.10 ns |  2.47 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,710.8 ns** | **1,396.72 ns** |  **76.56 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,939.6 ns |   138.40 ns |   7.59 ns |  0.91 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 12,152.2 ns | 1,097.39 ns |  60.15 ns |  1.40 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,932.2 ns** | **2,606.25 ns** | **142.86 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,926.8 ns |   888.94 ns |  48.73 ns |  1.00 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,713.9 ns |   834.10 ns |  45.72 ns |  1.48 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,296.2 ns** | **3,273.09 ns** | **179.41 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,006.1 ns |   633.81 ns |  34.74 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,245.3 ns | 3,864.53 ns | 211.83 ns |  1.48 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,361.3 ns** |   **901.38 ns** |  **49.41 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,374.0 ns |   558.38 ns |  30.61 ns |  1.19 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,342.4 ns |   407.65 ns |  22.34 ns |  1.74 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,211.3 ns** | **5,110.75 ns** | **280.14 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,995.8 ns | 1,090.52 ns |  59.78 ns |  0.97 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,358.9 ns | 1,348.18 ns |  73.90 ns |  1.58 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,337.2 ns** | **4,937.27 ns** | **270.63 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,976.4 ns |   675.96 ns |  37.05 ns |  0.96 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,744.3 ns | 1,941.74 ns | 106.43 ns |  1.69 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,697.9 ns** |    **57.79 ns** |   **3.17 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,077.1 ns |   345.43 ns |  18.93 ns |  1.10 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,351.3 ns |   379.75 ns |  20.82 ns |  1.99 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,248.7 ns** |   **589.89 ns** |  **32.33 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,875.6 ns |   388.28 ns |  21.28 ns |  1.12 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 11,907.2 ns |   770.69 ns |  42.24 ns |  2.27 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,965.4 ns** |   **446.40 ns** |  **24.47 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,346.7 ns |   520.97 ns |  28.56 ns |  0.92 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 14,169.3 ns |   897.02 ns |  49.17 ns |  1.78 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,387.7 ns** |    **14.31 ns** |   **0.78 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,136.5 ns |   106.47 ns |   5.84 ns |  0.89 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,938.4 ns |   372.74 ns |  20.43 ns |  2.49 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **3,973.3 ns** |    **66.80 ns** |   **3.66 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,879.4 ns |   231.53 ns |  12.69 ns |  0.72 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,666.7 ns |   905.63 ns |  49.64 ns |  2.18 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,459.9 ns** |   **225.60 ns** |  **12.37 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,829.5 ns |   386.94 ns |  21.21 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,806.8 ns |   583.30 ns |  31.97 ns |  1.98 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,585.9 ns** |   **110.34 ns** |   **6.05 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,400.0 ns |   131.87 ns |   7.23 ns |  0.95 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,290.1 ns |   123.47 ns |   6.77 ns |  2.03 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,370.6 ns** | **1,109.95 ns** |  **60.84 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,254.8 ns |   247.24 ns |  13.55 ns |  0.79 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,546.0 ns |   610.04 ns |  33.44 ns |  1.96 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,762.9 ns** |   **402.13 ns** |  **22.04 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,021.2 ns |   777.19 ns |  42.60 ns |  1.03 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 13,591.8 ns |   446.54 ns |  24.48 ns |  1.75 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,335.0 ns** |    **22.59 ns** |   **1.24 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    596.4 ns |    40.12 ns |   2.20 ns |  0.45 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,075.4 ns |    62.63 ns |   3.43 ns |  3.80 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,010.5 ns** |   **115.67 ns** |   **6.34 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,418.9 ns |    96.27 ns |   5.28 ns |  0.47 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     | 14,023.9 ns |   722.60 ns |  39.61 ns |  4.66 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,245.5 ns** |    **29.86 ns** |   **1.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,373.7 ns |   149.75 ns |   8.21 ns |  0.45 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,082.9 ns |   647.32 ns |  35.48 ns |  2.30 |    0.01 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,615.2 μs** |    **211.30 μs** |    **11.58 μs** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,830.4 μs |  1,010.47 μs |    55.39 μs |  6.92 |    0.02 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  86,836.5 μs | 84,808.78 μs | 4,648.66 μs | 15.46 |    0.72 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **822.6 μs** |     **54.04 μs** |     **2.96 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,819.1 μs |     88.51 μs |     4.85 μs |  5.86 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,047.5 μs |    108.52 μs |     5.95 μs |  3.70 |    0.01 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,304.7 μs** |  **1,857.69 μs** |   **101.83 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 443,327.5 μs | 96,082.30 μs | 5,266.60 μs | 16.85 |    0.18 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 751,355.1 μs | 54,887.24 μs | 3,008.56 μs | 28.56 |    0.14 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.948 ms** | **0.1453 ms** | **0.0080 ms** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.819 ms | 2.0019 ms | 0.1097 ms |  3.40 |    0.02 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.121 ms** | **4.0838 ms** | **0.2238 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 112.508 ms | 5.7816 ms | 0.3169 ms |  4.31 |    0.03 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.749 ms** | **6.9836 ms** | **0.3828 ms** |  **1.00** |    **0.01** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 206.702 ms | 4.0770 ms | 0.2235 ms |  4.42 |    0.03 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **132.40 ns** |    **16.050 ns** |   **0.880 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |     58.78 ns |     4.078 ns |   0.224 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    131.91 ns |     2.101 ns |   0.115 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |     97.63 ns |     3.733 ns |   0.205 ns |  0.74 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **230.06 ns** |   **194.909 ns** |  **10.684 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 12   |     62.30 ns |     4.297 ns |   0.236 ns |  0.27 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    229.17 ns |    50.746 ns |   2.782 ns |  1.00 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    109.56 ns |     1.161 ns |   0.064 ns |  0.48 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **267.89 ns** |    **11.114 ns** |   **0.609 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 14   |     66.21 ns |     1.660 ns |   0.091 ns |  0.25 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    266.91 ns |    10.227 ns |   0.561 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 14   |    113.39 ns |     0.776 ns |   0.043 ns |  0.42 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **379.53 ns** |    **19.409 ns** |   **1.064 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |     71.84 ns |     1.725 ns |   0.095 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    354.70 ns |   114.946 ns |   6.301 ns |  0.93 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    119.22 ns |    14.243 ns |   0.781 ns |  0.31 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **436.17 ns** |    **17.210 ns** |   **0.943 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     73.75 ns |     0.730 ns |   0.040 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    439.73 ns |    68.423 ns |   3.751 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 18   |    125.93 ns |     3.187 ns |   0.175 ns |  0.29 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **759.40 ns** |    **71.533 ns** |   **3.921 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |     79.24 ns |     3.900 ns |   0.214 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    771.79 ns |     2.948 ns |   0.162 ns |  1.02 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    129.08 ns |     5.547 ns |   0.304 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **990.01 ns** |   **281.822 ns** |  **15.448 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |     86.30 ns |     1.431 ns |   0.078 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    993.51 ns |   166.172 ns |   9.108 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    140.76 ns |     3.989 ns |   0.219 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,580.40 ns** |   **186.417 ns** |  **10.218 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    115.24 ns |     7.144 ns |   0.392 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,526.43 ns |   159.393 ns |   8.737 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    164.96 ns |     5.691 ns |   0.312 ns |  0.10 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,156.31 ns** |   **838.318 ns** |  **45.951 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    132.73 ns |     0.295 ns |   0.016 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,316.27 ns | 2,036.438 ns | 111.624 ns |  1.05 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    240.93 ns |    12.992 ns |   0.712 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,214.39 ns** | **4,134.348 ns** | **226.618 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 64   |    162.26 ns |     2.191 ns |   0.120 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,500.64 ns |   103.918 ns |   5.696 ns |  1.06 |    0.04 |         - |          NA |
| Kernel_Cjk | 64   |    294.45 ns |    37.786 ns |   2.071 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,796.31 ns** | **6,109.193 ns** | **334.865 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    351.89 ns |     5.350 ns |   0.293 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,416.68 ns |   688.834 ns |  37.757 ns |  1.05 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |    939.76 ns |    53.768 ns |   2.947 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-----------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.60 ns** |   **0.278 ns** |  **0.015 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.72 ns |   0.394 ns |  0.022 ns |  1.00 |         - |          NA |
| Distance_CodePoint         | 8      |    119.58 ns |   3.676 ns |  0.202 ns |  4.86 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.13 ns |   0.636 ns |  0.035 ns |  1.02 |         - |          NA |
|                            |        |              |            |           |       |           |             |
| **Distance_Utf16**             | **64**     |    **276.20 ns** |  **22.725 ns** |  **1.246 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    362.54 ns |   5.376 ns |  0.295 ns |  1.31 |         - |          NA |
| Distance_CodePoint         | 64     |    672.39 ns |  45.069 ns |  2.470 ns |  2.43 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    277.67 ns |  24.196 ns |  1.326 ns |  1.01 |         - |          NA |
|                            |        |              |            |           |       |           |             |
| **Distance_Utf16**             | **128**    |    **904.73 ns** |  **18.072 ns** |  **0.991 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,899.33 ns | 110.824 ns |  6.075 ns |  2.10 |         - |          NA |
| Distance_CodePoint         | 128    |  1,593.90 ns | 112.756 ns |  6.181 ns |  1.76 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    921.56 ns |  61.776 ns |  3.386 ns |  1.02 |         - |          NA |
|                            |        |              |            |           |       |           |             |
| **Distance_Utf16**             | **512**    | **10,978.46 ns** |  **89.799 ns** |  **4.922 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,887.28 ns | 333.391 ns | 18.274 ns |  1.54 |         - |          NA |
| Distance_CodePoint         | 512    | 13,372.08 ns | 153.633 ns |  8.421 ns |  1.22 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,038.00 ns |  13.839 ns |  0.759 ns |  1.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **345.3 ns** |     **49.96 ns** |     **2.74 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     235.7 ns |      2.08 ns |     0.11 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **354.2 ns** |     **14.50 ns** |     **0.79 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     241.2 ns |      5.06 ns |     0.28 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **426.1 ns** |      **2.09 ns** |     **0.11 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     319.6 ns |      5.16 ns |     0.28 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **435.4 ns** |     **83.45 ns** |     **4.57 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     330.4 ns |     77.77 ns |     4.26 ns |  0.76 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **517.0 ns** |     **15.42 ns** |     **0.85 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     409.5 ns |     44.61 ns |     2.45 ns |  0.79 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **515.9 ns** |      **1.20 ns** |     **0.07 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     412.7 ns |     76.91 ns |     4.22 ns |  0.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **613.6 ns** |     **42.63 ns** |     **2.34 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,282.9 ns |     53.70 ns |     2.94 ns |  2.09 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **650.0 ns** |     **32.20 ns** |     **1.77 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,293.0 ns |     61.61 ns |     3.38 ns |  1.99 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,894.5 ns** |     **51.32 ns** |     **2.81 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,425.3 ns |    363.63 ns |    19.93 ns |  2.86 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,898.5 ns** |    **274.04 ns** |    **15.02 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,435.7 ns |    157.03 ns |     8.61 ns |  2.86 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,857.3 ns** |    **439.83 ns** |    **24.11 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  64,245.2 ns |    812.87 ns |    44.56 ns |  4.32 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **427,144.5 ns** | **77,409.95 ns** | **4,243.10 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  64,903.6 ns |    300.80 ns |    16.49 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.06 ns** |      **0.556 ns** |     **0.030 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      84.60 ns |      4.817 ns |     0.264 ns |  3.38 |    0.01 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      82.79 ns |      1.985 ns |     0.109 ns |  3.30 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     184.38 ns |     18.595 ns |     1.019 ns |  7.36 |    0.04 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **275.62 ns** |     **10.543 ns** |     **0.578 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,163.48 ns |  1,651.651 ns |    90.533 ns | 22.36 |    0.29 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,389.98 ns |     79.801 ns |     4.374 ns |  5.04 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  11,254.27 ns |    701.983 ns |    38.478 ns | 40.83 |    0.14 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,138.57 ns** |    **104.683 ns** |     **5.738 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 447,606.40 ns | 27,793.274 ns | 1,523.443 ns | 40.19 |    0.12 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,865.08 ns |  1,041.056 ns |    57.064 ns |  3.40 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 725,777.88 ns |  8,865.051 ns |   485.923 ns | 65.16 |    0.05 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **610.7 μs** |  **95.01 μs** |  **5.21 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 601.0 μs |  53.46 μs |  2.93 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 606.1 μs | 216.59 μs | 11.87 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **775.3 μs** |  **46.13 μs** |  **2.53 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 771.6 μs |  85.40 μs |  4.68 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 916.7 μs |  30.77 μs |  1.69 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error          | StdDev        | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|--------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **129.22 μs** |       **3.627 μs** |      **0.199 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  97,634.42 μs |   7,831.480 μs |    429.270 μs | 755.60 |    3.05 | 833.3333 | 833.3333 | 833.3333 | 31414.75 KB |   12,806.01 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,925.36 μs** |     **157.152 μs** |      **8.614 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 480,036.13 μs | 208,181.049 μs | 11,411.106 μs | 164.10 |    3.40 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.62 μs** |       **1.428 μs** |      **0.078 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     869.37 μs |      56.838 μs |      3.116 μs |  38.44 |    0.17 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error         | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|--------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,297.8 ns** |     **131.36 ns** |      **7.20 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,137.3 ns |     175.34 ns |      9.61 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       109.1 ns |      28.56 ns |      1.57 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,858.2 ns |     118.50 ns |      6.50 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,465.1 ns |     207.27 ns |     11.36 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,680.4 ns** |     **255.28 ns** |     **13.99 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,386.1 ns |     179.90 ns |      9.86 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       109.5 ns |      18.29 ns |      1.00 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,460.4 ns |     169.94 ns |      9.32 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,695.6 ns |     430.69 ns |     23.61 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **461,001.7 ns** |   **7,272.61 ns** |    **398.64 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   832,937.6 ns |  20,502.63 ns |  1,123.82 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,710.5 ns |   2,615.20 ns |    143.35 ns |      - |         - |
| F1Macro        | 100000  | 2       |   477,197.7 ns |  14,352.35 ns |    786.70 ns |      - |     656 B |
| Report         | 100000  | 2       |   464,795.3 ns |  22,023.59 ns |  1,207.19 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **506,725.5 ns** |  **72,929.71 ns** |  **3,997.52 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   945,080.6 ns |  28,867.49 ns |  1,582.32 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,307.6 ns |     152.62 ns |      8.37 ns |      - |         - |
| F1Macro        | 100000  | 10      |   509,153.9 ns |  19,363.03 ns |  1,061.35 ns |      - |    1881 B |
| Report         | 100000  | 10      |   501,916.2 ns |  36,004.23 ns |  1,973.51 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,767,375.0 ns** | **105,572.08 ns** |  **5,786.76 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,241,468.5 ns | 432,875.06 ns | 23,727.34 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   135,865.8 ns |   5,601.13 ns |    307.02 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,607,408.2 ns | 138,175.43 ns |  7,573.86 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,619,438.4 ns |  55,527.08 ns |  3,043.63 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,907,049.5 ns** | **227,761.09 ns** | **12,484.35 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,422,211.1 ns | 596,077.48 ns | 32,673.02 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   136,340.2 ns |  47,886.21 ns |  2,624.80 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 5,069,694.4 ns | 189,944.97 ns | 10,411.52 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,089,223.6 ns | 237,262.51 ns | 13,005.16 ns |      - |   12876 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **11,351.60 μs** |  **6,986.768 μs** |   **382.968 μs** |     **1.00** |    **0.04** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,486.18 μs |    823.878 μs |    45.160 μs |     3.13 |    0.09 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.73 μs** |      **6.932 μs** |     **0.380 μs** |     **1.00** |    **0.04** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  35,449.39 μs |  4,656.603 μs |   255.244 μs | 3,024.04 |   87.79 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **141,843.25 μs** |  **3,500.281 μs** |   **191.862 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 253,289.40 μs | 45,386.457 μs | 2,487.785 μs |     1.79 |    0.02 |        - |        - |        - | 23229116 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **116.39 μs** |     **55.632 μs** |     **3.049 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 228,596.90 μs | 16,431.423 μs |   900.662 μs | 1,964.94 |   45.35 |        - |        - |        - | 23228104 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **74.07 ns** |      **1.093 ns** |   **0.060 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     74.43 ns |      2.083 ns |   0.114 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     73.45 ns |      2.589 ns |   0.142 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     74.19 ns |      2.227 ns |   0.122 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **6**    |    **104.46 ns** |      **2.801 ns** |   **0.154 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     89.18 ns |      1.308 ns |   0.072 ns |  0.85 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    104.21 ns |      0.866 ns |   0.047 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 6    |    132.40 ns |      0.478 ns |   0.026 ns |  1.27 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **8**    |    **157.05 ns** |      **2.138 ns** |   **0.117 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    101.98 ns |      1.825 ns |   0.100 ns |  0.65 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    158.39 ns |     32.455 ns |   1.779 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    180.99 ns |      6.898 ns |   0.378 ns |  1.15 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **10**   |    **196.97 ns** |      **8.588 ns** |   **0.471 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    109.75 ns |      0.909 ns |   0.050 ns |  0.56 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    196.55 ns |      1.640 ns |   0.090 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    151.46 ns |      2.899 ns |   0.159 ns |  0.77 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **245.15 ns** |      **1.869 ns** |   **0.102 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    117.15 ns |      2.841 ns |   0.156 ns |  0.48 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    248.36 ns |    104.574 ns |   5.732 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 12   |    165.65 ns |     10.750 ns |   0.589 ns |  0.68 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **409.29 ns** |      **4.159 ns** |   **0.228 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    131.90 ns |     12.841 ns |   0.704 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    419.07 ns |    300.230 ns |  16.457 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    179.29 ns |      2.534 ns |   0.139 ns |  0.44 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **842.02 ns** |     **24.273 ns** |   **1.330 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    173.57 ns |      6.134 ns |   0.336 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    849.13 ns |    127.774 ns |   7.004 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    224.89 ns |      4.206 ns |   0.231 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,450.69 ns** |     **17.722 ns** |   **0.971 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    205.52 ns |      1.064 ns |   0.058 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,452.41 ns |    128.293 ns |   7.032 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    269.53 ns |      7.230 ns |   0.396 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,337.64 ns** |  **1,335.490 ns** |  **73.203 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 48   |    279.02 ns |      6.317 ns |   0.346 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,768.63 ns |    687.194 ns |  37.667 ns |  1.13 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    353.84 ns |      5.276 ns |   0.289 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **6,262.16 ns** |    **499.868 ns** |  **27.399 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    347.06 ns |      2.827 ns |   0.155 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  6,362.18 ns |  3,865.002 ns | 211.854 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    442.54 ns |      5.173 ns |   0.284 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **14,104.19 ns** |  **1,925.954 ns** | **105.568 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    757.41 ns |      5.161 ns |   0.283 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,329.27 ns | 11,382.738 ns | 623.926 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |  1,524.21 ns |    241.398 ns |  13.232 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|---------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **205.36 μs** |  **17.409 μs** | **0.954 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   207.21 μs |  14.616 μs | 0.801 μs |  1.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    61.44 μs |   4.762 μs | 0.261 μs |  0.30 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.31 μs |  12.006 μs | 0.658 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **204.37 μs** |  **15.335 μs** | **0.841 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   231.50 μs |  20.060 μs | 1.100 μs |  1.13 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    58.76 μs |   3.517 μs | 0.193 μs |  0.29 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   198.23 μs |  16.685 μs | 0.915 μs |  0.97 |      - |     800 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,969.11 μs** |  **32.801 μs** | **1.798 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,033.69 μs | 144.989 μs | 7.947 μs |  1.03 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,079.00 μs |  39.206 μs | 2.149 μs |  0.55 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,973.15 μs | 139.194 μs | 7.630 μs |  1.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,042.37 μs** |  **72.876 μs** | **3.995 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,081.13 μs | 133.655 μs | 7.326 μs |  1.02 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,078.02 μs |   3.743 μs | 0.205 μs |  0.53 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,974.81 μs |  54.502 μs | 2.987 μs |  0.97 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error       | StdDev    | Allocated |
|------------ |------------- |-------------:|------------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.784 μs** |   **0.1547 μs** | **0.0085 μs** |         **-** |
| EqualLength | 65           |     2.947 μs |   0.0230 μs | 0.0013 μs |         - |
| **Embedded**    | **128**          |    **38.148 μs** |   **0.9046 μs** | **0.0496 μs** |         **-** |
| EqualLength | 128          |     5.340 μs |   0.1450 μs | 0.0079 μs |         - |
| **Embedded**    | **512**          | **1,357.425 μs** | **131.9005 μs** | **7.2299 μs** |         **-** |
| EqualLength | 512          |    44.505 μs |   0.8930 μs | 0.0489 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.895 ms |  0.7227 ms | 0.0396 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.494 ms |  3.4834 ms | 0.1909 ms | 187.5000 | 171.8750 |  46.8750 |  5852.31 KB |
| TokenizerJsonUnigram   | 12.729 ms |  0.6017 ms | 0.0330 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.220 ms |  0.9862 ms | 0.0541 ms | 132.8125 | 125.0000 |  39.0625 |  4807.68 KB |
| TfidfSave              |  1.591 ms |  0.7116 ms | 0.0390 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.287 ms |  0.0666 ms | 0.0036 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.488 ms |  0.2415 ms | 0.0132 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.921 ms |  2.0961 ms | 0.1149 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.800 ms | 10.9472 ms | 0.6001 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 71.606 ms |  3.9262 ms | 0.2152 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,159.46 μs** | **401.744 μs** | **22.021 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,180.11 μs | 289.160 μs | 15.850 μs |  1.66 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **130.59 μs** |   **2.363 μs** |  **0.130 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    181.83 μs |  10.165 μs |  0.557 μs |  1.39 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,823.10 μs** | **317.768 μs** | **17.418 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,588.23 μs |  81.166 μs |  4.449 μs |  0.92 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.43 μs** |   **3.825 μs** |  **0.210 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     26.76 μs |   0.901 μs |  0.049 μs |  1.19 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.46 μs** |   **4.698 μs** |  **0.258 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    44.84 μs |   4.901 μs |  0.269 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   109.98 μs |   8.149 μs |  0.447 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   534.57 μs | 163.106 μs |  8.940 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **458.33 μs** |  **17.953 μs** |  **0.984 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   446.36 μs |   7.643 μs |  0.419 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,105.93 μs |  66.686 μs |  3.655 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,179.61 μs | 201.616 μs | 11.051 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean            | Error         | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|--------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,204.3 ns |     312.27 ns |     17.12 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,435.5 ns |     341.52 ns |     18.72 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,200.8 ns |     350.68 ns |     19.22 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        437.6 ns |      45.92 ns |      2.52 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,820,932.8 ns | 606,863.67 ns | 33,264.25 ns | 875.0000 | 843.7500 | 609.3750 | 12227072 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean     | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |---------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1.929 ms** | **0.0253 ms** | **0.0014 ms** |  **1.00** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.400 ms | 0.0925 ms | 0.0051 ms |  0.73 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 2.020 ms | 0.1232 ms | 0.0068 ms |  1.05 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.684 ms | 0.0200 ms | 0.0011 ms |  0.87 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **9.127 ms** | **0.2280 ms** | **0.0125 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.48 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.265 ms | 0.3668 ms | 0.0201 ms |  0.80 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.656 ms | 1.0578 ms | 0.0580 ms |  1.06 | 515.6250 | 500.0000 | 500.0000 | 2349.58 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.770 ms | 0.0868 ms | 0.0048 ms |  0.96 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **22.76 ms** |  **1.233 ms** | **0.068 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  59.16 ms | 18.470 ms | 1.012 ms |  2.60 |    0.04 |  222.2222 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **53.79 ms** | **11.222 ms** | **0.615 ms** |  **1.00** |    **0.01** |  **300.0000** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  56.84 ms |  4.886 ms | 0.268 ms |  1.06 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **93.84 ms** | **36.216 ms** | **1.985 ms** |  **1.00** |    **0.03** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 341.99 ms | 95.925 ms | 5.258 ms |  3.65 |    0.08 | 3000.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.593 ms** |   **0.2438 ms** |  **0.0134 ms** |  **1.00** |    **0.00** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  56.259 ms | 176.1937 ms |  9.6578 ms | 15.66 |    2.33 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.304 ms** |   **1.0733 ms** |  **0.0588 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 410.188 ms | 489.7395 ms | 26.8443 ms | 23.71 |    1.35 | 74000.0000 | 74000.0000 | 74000.0000 | 324.36 MB |       40.00 |

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
- `stats`

### compare-cluster-weighted

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.065 | 3.934 | 0.78x | 5.071 | 7.842 | 1.55x |
| kmeans_restarts_10000 | 20.839 | 10.260 | 0.49x | 20.848 | 20.439 | 0.98x |
| kmeans_weighted_100000 | 49.434 | 27.848 | 0.56x | 49.710 | 47.053 | 0.95x |
| kmeans_restarts_100000 | 201.222 | 71.206 | 0.35x | 201.630 | 131.459 | 0.65x |
| dbscan_weighted_5000 | 7.419 | 32.874 | 4.43x | 7.798 | 32.869 | 4.22x |
| dbscan_weighted_20000 | 91.870 | 197.412 | 2.15x | 92.727 | 197.389 | 2.13x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.861 | 128.508 | 149.25x | 0.861 | 128.501 | 149.25x |
| ridge_robust_1000 | 0.942 | 118.702 | 125.98x | 0.942 | 118.681 | 125.97x |
| lasso_1000 | 6.653 | 1105.443 | 166.15x | 6.654 | 1105.309 | 166.12x |
| ph_test_1000 | 0.128 | 21.792 | 170.36x | 0.128 | 21.789 | 170.34x |
| predict_survival_1000 | 0.031 | 1.878 | 60.35x | 0.031 | 1.878 | 60.34x |
| time_varying_1000 | 79.546 | 355.229 | 4.47x | 79.604 | 355.168 | 4.46x |
| stratified_weighted_10000 | 9.173 | 710.876 | 77.49x | 9.200 | 710.741 | 77.25x |
| ridge_robust_10000 | 10.218 | 1108.263 | 108.46x | 10.455 | 1108.155 | 105.99x |
| lasso_10000 | 60.354 | 7060.282 | 116.98x | 60.387 | 7059.662 | 116.91x |
| ph_test_10000 | 2.015 | 213.738 | 106.06x | 2.077 | 213.717 | 102.90x |
| predict_survival_10000 | 0.033 | 1.898 | 57.57x | 0.033 | 1.898 | 57.57x |
| time_varying_10000 | 3052.670 | 5255.176 | 1.72x | 3052.432 | 5254.772 | 1.72x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 4.198 | 46.268 | 11.02x | 4.198 | 184.304 | 43.91x |
| cv_minmax_n1000 | 3.067 | 3.899 | 1.27x | 3.067 | 15.517 | 5.06x |
| cv_plus_gamma_n1000 | 4.762 | 47.944 | 10.07x | 4.761 | 191.174 | 40.15x |
| bootstrap_plus_n1000 | 16.410 | 47.672 | 2.91x | 16.409 | 189.791 | 11.57x |
| cv_plus_n10000 | 41.497 | 127.696 | 3.08x | 41.496 | 470.343 | 11.33x |
| cv_minmax_n10000 | 30.842 | 25.556 | 0.83x | 30.840 | 101.668 | 3.30x |
| cv_plus_gamma_n10000 | 58.550 | 134.710 | 2.30x | 58.546 | 479.433 | 8.19x |
| bootstrap_plus_n10000 | 151.471 | 131.971 | 0.87x | 151.450 | 480.772 | 3.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.404 | 3.411 | 8.43x | 0.404 | 3.410 | 8.43x |
| glm_gamma_n1000 | 0.485 | 4.254 | 8.77x | 0.485 | 4.253 | 8.77x |
| glm_poisson_exposure_n1000 | 0.407 | 4.007 | 9.83x | 0.407 | 4.007 | 9.83x |
| mnlogit_n1000 | 0.825 | 18.314 | 22.20x | 0.825 | 18.313 | 22.20x |
| glm_negative_binomial_n10000 | 3.663 | 13.962 | 3.81x | 3.674 | 13.961 | 3.80x |
| glm_gamma_n10000 | 4.157 | 13.044 | 3.14x | 4.167 | 13.041 | 3.13x |
| glm_poisson_exposure_n10000 | 4.368 | 13.068 | 2.99x | 4.379 | 13.066 | 2.98x |
| mnlogit_n10000 | 8.660 | 98.381 | 11.36x | 8.669 | 98.368 | 11.35x |
| glm_negative_binomial_n100000 | 37.491 | 140.085 | 3.74x | 37.834 | 559.319 | 14.78x |
| glm_gamma_n100000 | 43.121 | 159.652 | 3.70x | 43.351 | 636.948 | 14.69x |
| glm_poisson_exposure_n100000 | 55.014 | 135.860 | 2.47x | 55.365 | 540.772 | 9.77x |
| mnlogit_n100000 | 83.262 | 954.103 | 11.46x | 83.302 | 1710.501 | 20.53x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.686 | 57.200 | 33.93x | 1.686 | 57.197 | 33.93x |
| liml_robust_n1000 | 1.839 | 58.358 | 31.74x | 1.839 | 58.354 | 31.74x |
| gmm_robust_n1000 | 1.702 | 56.819 | 33.38x | 1.702 | 56.816 | 33.38x |
| 2sls_kernel_n1000 | 9.978 | 59.047 | 5.92x | 10.068 | 59.038 | 5.86x |
| 2sls_clustered_n1000 | 1.636 | 57.419 | 35.10x | 1.636 | 57.416 | 35.10x |
| 2sls_robust_n10000 | 17.414 | 251.380 | 14.44x | 17.838 | 502.856 | 28.19x |
| liml_robust_n10000 | 19.056 | 249.492 | 13.09x | 19.532 | 498.953 | 25.55x |
| gmm_robust_n10000 | 17.019 | 240.021 | 14.10x | 17.395 | 479.988 | 27.59x |
| 2sls_kernel_n10000 | 107.328 | 270.406 | 2.52x | 107.709 | 540.880 | 5.02x |
| 2sls_clustered_n10000 | 16.006 | 260.923 | 16.30x | 16.385 | 521.884 | 31.85x |
| 2sls_robust_n100000 | 174.967 | 2566.367 | 14.67x | 176.611 | 7987.410 | 45.23x |
| liml_robust_n100000 | 187.760 | 2563.267 | 13.65x | 190.118 | 7988.808 | 42.02x |
| gmm_robust_n100000 | 177.933 | 2505.685 | 14.08x | 181.221 | 7682.991 | 42.40x |
| 2sls_kernel_n100000 | 4018.651 | 3333.684 | 0.83x | 4020.169 | 10250.484 | 2.55x |
| 2sls_clustered_n100000 | 167.110 | 2828.397 | 16.93x | 168.281 | 8954.419 | 53.21x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.029 | 0.867 | 0.43x | 2.028 | 0.867 | 0.43x |
| transform_frobenius_n500 | 6.560 | 1.669 | 0.25x | 6.560 | 1.669 | 0.25x |
| transform_frobenius_n2000 | 23.903 | 6.101 | 0.26x | 23.934 | 24.317 | 1.02x |
| transform_kl_n100 | 1.366 | 18.114 | 13.26x | 1.366 | 18.111 | 13.25x |
| transform_kl_n500 | 6.269 | 34.597 | 5.52x | 6.269 | 34.596 | 5.52x |
| transform_kl_n2000 | 25.027 | 123.487 | 4.93x | 25.080 | 452.083 | 18.03x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.046 | 3.030 | 65.33x | 0.046 | 3.030 | 65.34x |
| ols_hac_n1000 | 0.161 | 3.531 | 21.87x | 0.161 | 3.531 | 21.86x |
| ols_cluster_n1000 | 0.063 | 3.579 | 57.05x | 0.063 | 3.579 | 57.05x |
| ols_summary_n10000 | 0.432 | 11.580 | 26.81x | 0.432 | 11.578 | 26.82x |
| ols_hac_n10000 | 1.623 | 12.802 | 7.89x | 1.629 | 12.801 | 7.86x |
| ols_cluster_n10000 | 0.596 | 13.141 | 22.05x | 0.600 | 13.140 | 21.90x |
| ols_summary_n100000 | 4.280 | 115.331 | 26.95x | 4.297 | 460.097 | 107.07x |
| ols_hac_n100000 | 16.580 | 127.372 | 7.68x | 16.997 | 508.368 | 29.91x |
| ols_cluster_n100000 | 6.522 | 131.850 | 20.22x | 6.734 | 525.039 | 77.97x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.658 | 4.401 | 2.66x | 1.662 | 4.401 | 2.65x |
| sparse_infrequent_10000 | 2.119 | 4.729 | 2.23x | 2.124 | 4.729 | 2.23x |
| sparse_100000 | 16.793 | 29.705 | 1.77x | 17.218 | 29.703 | 1.73x |
| sparse_infrequent_100000 | 18.966 | 24.752 | 1.31x | 19.431 | 24.750 | 1.27x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3', 'pandas': '2.3.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.227 | 19.862 | 87.33x | 0.227 | 19.859 | 87.31x |
| fe_twoway_clustered_n1000 | 0.398 | 31.375 | 78.90x | 0.398 | 31.370 | 78.90x |
| fe_kernel_n1000 | 0.245 | 20.495 | 83.66x | 0.245 | 20.493 | 83.64x |
| between_robust_n1000 | 0.128 | 19.177 | 149.81x | 0.128 | 19.175 | 149.80x |
| first_difference_robust_n1000 | 0.260 | 23.432 | 90.04x | 0.260 | 23.429 | 90.03x |
| random_effects_n1000 | 0.241 | 26.440 | 109.55x | 0.241 | 26.436 | 109.54x |
| fe_entity_n10000 | 2.759 | 29.037 | 10.53x | 2.830 | 29.033 | 10.26x |
| fe_twoway_clustered_n10000 | 4.890 | 55.070 | 11.26x | 5.128 | 110.127 | 21.47x |
| fe_kernel_n10000 | 2.970 | 28.638 | 9.64x | 3.020 | 28.633 | 9.48x |
| between_robust_n10000 | 1.792 | 28.271 | 15.77x | 1.799 | 28.267 | 15.71x |
| first_difference_robust_n10000 | 2.945 | 33.497 | 11.37x | 2.959 | 33.494 | 11.32x |
| random_effects_n10000 | 2.923 | 39.334 | 13.46x | 2.958 | 39.330 | 13.30x |
| fe_entity_n100000 | 33.429 | 182.218 | 5.45x | 35.125 | 725.324 | 20.65x |
| fe_twoway_clustered_n100000 | 47.628 | 355.971 | 7.47x | 48.966 | 1275.938 | 26.06x |
| fe_kernel_n100000 | 31.529 | 171.246 | 5.43x | 32.517 | 681.988 | 20.97x |
| between_robust_n100000 | 18.711 | 167.264 | 8.94x | 19.213 | 537.117 | 27.96x |
| first_difference_robust_n100000 | 32.523 | 202.675 | 6.23x | 33.966 | 804.489 | 23.69x |
| random_effects_n100000 | 31.371 | 231.988 | 7.39x | 32.793 | 921.041 | 28.09x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.006 | 0.826 | 146.25x | 0.006 | 0.826 | 146.24x |
| mann_whitney_n1000 | 0.031 | 0.749 | 24.40x | 0.031 | 0.749 | 24.40x |
| chi_square_n1000 | 0.000 | 0.342 | 1702.02x | 0.000 | 0.342 | 1702.18x |
| fligner_n1000 | 0.076 | 0.819 | 10.79x | 0.076 | 0.819 | 10.79x |
| anderson_ksamp_n1000 | 0.165 | 0.397 | 2.41x | 0.165 | 0.397 | 2.41x |
| spearman_matrix_n1000 | 0.098 | 0.930 | 9.48x | 0.098 | 0.930 | 9.48x |
| pointbiserial_n1000 | 0.010 | 0.573 | 55.00x | 0.010 | 0.573 | 55.00x |
| welch_t_n10000 | 0.054 | 0.873 | 16.16x | 0.054 | 0.873 | 16.16x |
| mann_whitney_n10000 | 0.749 | 2.835 | 3.78x | 0.749 | 2.835 | 3.78x |
| chi_square_n10000 | 0.001 | 0.348 | 249.53x | 0.001 | 0.348 | 249.48x |
| fligner_n10000 | 1.443 | 3.258 | 2.26x | 1.512 | 3.258 | 2.15x |
| anderson_ksamp_n10000 | 2.032 | 4.008 | 1.97x | 2.138 | 4.008 | 1.87x |
| spearman_matrix_n10000 | 2.162 | 4.812 | 2.23x | 2.233 | 4.812 | 2.15x |
| pointbiserial_n10000 | 0.100 | 0.644 | 6.42x | 0.100 | 0.644 | 6.42x |
| welch_t_n100000 | 0.541 | 1.276 | 2.36x | 0.541 | 1.276 | 2.36x |
| mann_whitney_n100000 | 7.827 | 28.698 | 3.67x | 7.825 | 28.694 | 3.67x |
| chi_square_n100000 | 0.014 | 0.371 | 26.35x | 0.014 | 0.371 | 26.35x |
| fligner_n100000 | 14.657 | 32.600 | 2.22x | 15.057 | 32.597 | 2.16x |
| anderson_ksamp_n100000 | 15.792 | 43.966 | 2.78x | 16.206 | 43.962 | 2.71x |
| spearman_matrix_n100000 | 17.964 | 53.090 | 2.96x | 18.281 | 53.084 | 2.90x |
| pointbiserial_n100000 | 1.295 | 1.448 | 1.12x | 1.308 | 5.776 | 4.42x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 8.562 | 87.162 | 10.18x | 8.562 | 347.080 | 40.54x |
| fit_penalised_weighted_1000 | 8.040 | 94.766 | 11.79x | 8.039 | 377.856 | 47.00x |
| predict_survival_1000 | 0.841 | 2.408 | 2.86x | 0.849 | 2.407 | 2.84x |
| fit_10000 | 240.712 | 626.596 | 2.60x | 240.947 | 1000.890 | 4.15x |
| fit_penalised_weighted_10000 | 219.901 | 656.224 | 2.98x | 220.127 | 1030.314 | 4.68x |
| predict_survival_10000 | 3.318 | 5.054 | 1.52x | 3.516 | 20.144 | 5.73x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.027 | 14.237 | 533.89x | 0.027 | 14.235 | 533.87x |
| logrank_fleming_harrington_1000 | 0.035 | 21.536 | 606.67x | 0.035 | 21.532 | 606.57x |
| multigroup_1000 | 0.115 | 28.833 | 249.81x | 0.115 | 28.827 | 249.77x |
| pairwise_1000 | 0.312 | 149.201 | 478.56x | 0.312 | 149.175 | 478.54x |
| rmst_1000 | 0.092 | 1291.660 | 14065.90x | 0.092 | 1291.531 | 14064.04x |
| fixed_point_1000 | 0.038 | 15.629 | 413.65x | 0.038 | 15.626 | 413.62x |
| concordance_1000 | 0.051 | 8.420 | 166.21x | 0.051 | 8.419 | 166.21x |
| logrank_wilcoxon_weighted_10000 | 0.342 | 16.468 | 48.15x | 0.342 | 16.464 | 48.14x |
| logrank_fleming_harrington_10000 | 0.426 | 26.219 | 61.48x | 0.427 | 26.216 | 61.47x |
| multigroup_10000 | 2.487 | 42.278 | 17.00x | 2.494 | 42.273 | 16.95x |
| pairwise_10000 | 4.945 | 166.502 | 33.67x | 4.944 | 166.493 | 33.67x |
| rmst_10000 | 1.855 | 1386.986 | 747.67x | 1.891 | 1386.880 | 733.47x |
| fixed_point_10000 | 0.410 | 17.673 | 43.13x | 0.410 | 17.672 | 43.13x |
| concordance_10000 | 2.392 | 99.418 | 41.57x | 2.392 | 99.406 | 41.56x |
| logrank_wilcoxon_weighted_100000 | 4.508 | 24.061 | 5.34x | 4.623 | 24.058 | 5.20x |
| logrank_fleming_harrington_100000 | 4.365 | 37.855 | 8.67x | 4.446 | 37.853 | 8.51x |
| multigroup_100000 | 11.073 | 53.146 | 4.80x | 11.212 | 53.144 | 4.74x |
| pairwise_100000 | 45.773 | 244.617 | 5.34x | 47.080 | 244.594 | 5.20x |
| rmst_100000 | 9.263 | 501.437 | 54.13x | 9.375 | 501.356 | 53.48x |
| fixed_point_100000 | 4.920 | 31.020 | 6.31x | 5.074 | 31.016 | 6.11x |
| concordance_100000 | 30.368 | 812.883 | 26.77x | 30.572 | 812.842 | 26.59x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.535 | 74.760 | 21.15x | 3.534 | 74.757 | 21.15x |
| loglogistic_left_1000 | 6.447 | 103.132 | 16.00x | 6.451 | 103.120 | 15.98x |
| lognormal_interval_1000 | 5.736 | 190.173 | 33.15x | 5.736 | 190.147 | 33.15x |
| generalized_gamma_1000 | 46.918 | 853.389 | 18.19x | 47.002 | 853.267 | 18.15x |
| weibull_aft_1000 | 7.501 | 134.830 | 17.98x | 7.502 | 134.827 | 17.97x |
| lognormal_aft_ridge_robust_1000 | 11.712 | 4785.337 | 408.59x | 11.714 | 4784.969 | 408.47x |
| loglogistic_aft_interval_1000 | 11.142 | 209.910 | 18.84x | 11.144 | 209.894 | 18.83x |
| breslow_fleming_harrington_1000 | 0.257 | 6.952 | 27.01x | 0.257 | 6.951 | 27.01x |
| kaplan_meier_left_1000 | 0.091 | 8.106 | 89.44x | 0.091 | 8.105 | 89.44x |
| weibull_10000 | 33.350 | 89.100 | 2.67x | 33.348 | 89.091 | 2.67x |
| loglogistic_left_10000 | 70.962 | 183.671 | 2.59x | 70.957 | 183.653 | 2.59x |
| lognormal_interval_10000 | 56.595 | 344.201 | 6.08x | 56.589 | 344.167 | 6.08x |
| generalized_gamma_10000 | 421.557 | 2098.452 | 4.98x | 421.528 | 2098.240 | 4.98x |
| weibull_aft_10000 | 71.983 | 156.815 | 2.18x | 71.984 | 156.802 | 2.18x |
| lognormal_aft_ridge_robust_10000 | 146.906 | 45601.001 | 310.41x | 146.899 | 45594.043 | 310.38x |
| loglogistic_aft_interval_10000 | 100.085 | 286.249 | 2.86x | 100.089 | 286.230 | 2.86x |
| breslow_fleming_harrington_10000 | 2.554 | 8.937 | 3.50x | 2.554 | 8.936 | 3.50x |
| kaplan_meier_left_10000 | 0.961 | 10.537 | 10.97x | 0.961 | 10.536 | 10.97x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.042 | 2.688 | 64.17x | 0.042 | 2.687 | 64.18x |
| var_n10000 | 0.477 | 16.436 | 34.43x | 0.481 | 16.433 | 34.15x |
| var_n100000 | 4.448 | 195.652 | 43.99x | 4.598 | 420.089 | 91.36x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `BitParallelEditDistanceBenchmarks`
- `BkTreeNearestBenchmarks`
- `Bm25Benchmarks`
- `ChainedProductBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `CoxBenchmarks`
- `DecompositionWidthBenchmarks`
- `DistributionIncumbentBenchmarks`
- `DistributionTailBenchmarks`
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
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `MultinomialLogitBenchmarks`
- `OlsBenchmarks`
- `PartitionValidityBenchmarks`
- `QuantileBenchmarks`
- `RankTestBenchmarks`
- `RankingMetricsBenchmarks`
- `RobustCovarianceBenchmarks`
- `SerialCorrelationBenchmarks`
- `SimilaritySketchBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `SurvivalBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `TokenPatternBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 11.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `IndelBenchmarks` | Length=20 | `SubsequenceLength_Utf16` | step | 1.28 | 0.89 | +44% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 0.97 | 1.39 | -29% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.02 | 1.43 | -29% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 1 | 1.38 | -28% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 3.13 | 5.43 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | drift | 3.88 | 4.93 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 3.19 | 4.11 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 2.34 | 3.15 | -21% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.96 | 1.25 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 2.2 | 2.89 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 9.45 | 11.855 | -20% | 20% |

**Still away from their median: 30.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | step | 44.6 | 7.64 | +484% | 50% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | step | 24.3 | 4.53 | +436% | 82% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | step | 13.72 | 3.92 | +250% | 71% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | step | 24.4 | 7.01 | +248% | 86% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | step | 15.62 | 6.98 | +124% | 82% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | step | 15.21 | 7.04 | +116% | 34% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | step | 15.66 | 10.02 | +56% | 30% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | step | 3.4 | 2.19 | +55% | 51% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.42 | 2.88 | +53% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.87 | 1.3 | +44% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_MannWhitney` | step | 34.083 | 55.807 | -39% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.3 | 0.18 | +39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_OneWayAnova` | step | 4.429 | 7.24 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KolmogorovSmirnov` | step | 24.036 | 39.164 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_StudentT` | step | 2.957 | 4.818 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KolmogorovSmirnov` | step | 35.413 | 57.615 | -39% | 30% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | step | 1.26 | 2.04 | -38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KruskalWallis` | step | 59.679 | 96.354 | -38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | step | 1.4 | 2.243 | -38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | step | 0.002 | 0.003 | -33% | 30% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 8.38 | 5.77 | +33% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | drift | 0.026 | 0.0385 | -32% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.182 | 4.658 | -32% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.29 | 0.19 | +32% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.13 | 1.64 | -31% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | drift | 0.012 | 0.017 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KruskalWallis` | drift | 24.445 | 33.582 | -27% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | drift | 0.008 | 0.0105 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_MannWhitney` | drift | 11.591 | 14.8845 | -22% | 20% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.04 | 0.05 | -20% | 20% |
