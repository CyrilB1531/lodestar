# Nightly benchmark run

<!-- nightly-baseline: c4a19d86a5f8c9899f169f26ac7e107de05eedd1 -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks PartitionValidityBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks SimilaritySketchBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks VectorAutoregressionBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks -->
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
- `VectorMathBenchmarks`

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
| **Lodestar_Pearson**               | **100**       |         **981.0 ns** |        **21.02 ns** |       **1.15 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         799.9 ns |        26.04 ns |       1.43 ns |     0.82 |    0.00 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,673.6 ns |       436.60 ns |      23.93 ns |     2.73 |    0.02 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,690.7 ns |       133.32 ns |       7.31 ns |     3.76 |    0.01 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,396.8 ns |     1,883.23 ns |     103.23 ns |     5.50 |    0.09 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,744.6 ns |     3,691.04 ns |     202.32 ns |    10.95 |    0.18 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,348.2 ns |       153.82 ns |       8.43 ns |     5.45 |    0.01 |        - |        - |        - |         - |          NA |
|                                |           |                  |                 |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **88,046.5 ns** |     **7,045.12 ns** |     **386.17 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,789.3 ns |     2,525.95 ns |     138.46 ns |     0.65 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,119,224.6 ns |   120,535.76 ns |   6,606.97 ns |    12.71 |    0.08 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,955,763.1 ns |   300,448.90 ns |  16,468.62 ns |    22.21 |    0.18 |   7.8125 |        - |        - |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,140,641.8 ns |   186,911.99 ns |  10,245.28 ns |    24.31 |    0.14 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,450,183.3 ns | 6,180,687.55 ns | 338,784.34 ns | 3,276.16 |   12.90 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,172,670.6 ns |   186,677.32 ns |  10,232.41 ns |    24.68 |    0.14 |        - |        - |        - |         - |          NA |

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

| Method            | SampleSize | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **32.03 μs** |  **27.173 μs** | **1.489 μs** |  **1.00** |    **0.06** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    33.94 μs |   2.691 μs | 0.148 μs |  1.06 |    0.04 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |            |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,997.02 μs** | **103.268 μs** | **5.660 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,455.42 μs |  25.098 μs | 1.376 μs |  0.73 |    0.00 | 7.8125 |  156.3 KB |        1.00 |

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
| **Lodestar_StudentT**              | **100**        |     **1,008.2 ns** |     **123.60 ns** |      **6.78 ns** |   **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     2,207.2 ns |     705.78 ns |     38.69 ns |   2.19 |    0.04 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,367.7 ns |     883.48 ns |     48.43 ns |   2.35 |    0.04 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     4,046.7 ns |     180.61 ns |      9.90 ns |   4.01 |    0.02 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     7,449.3 ns |     671.36 ns |     36.80 ns |   7.39 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     9,356.5 ns |   3,120.59 ns |    171.05 ns |   9.28 |    0.16 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,541.6 ns |     772.63 ns |     42.35 ns |   2.52 |    0.04 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,099.2 ns |     516.07 ns |     28.29 ns |   4.07 |    0.03 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       974.8 ns |     194.51 ns |     10.66 ns |   0.97 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     3,171.4 ns |      66.34 ns |      3.64 ns |   3.15 |    0.02 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |     1,129.3 ns |     123.57 ns |      6.77 ns |   1.12 |    0.01 | 0.0477 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,080.6 ns |      25.61 ns |      1.40 ns |   1.07 |    0.01 | 0.0572 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       460.8 ns |      64.19 ns |      3.52 ns |   0.46 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,556.9 ns |     326.46 ns |     17.89 ns |   1.54 |    0.02 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |       118.8 ns |      44.55 ns |      2.44 ns |   0.12 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       680.4 ns |     194.87 ns |     10.68 ns |   0.67 |    0.01 | 0.0572 |     968 B |          NA |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,858.5 ns** |   **6,125.89 ns** |    **335.78 ns** |  **1.000** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   178,838.8 ns |   1,637.56 ns |     89.76 ns |  2.939 |    0.01 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   700,352.6 ns |  27,661.97 ns |  1,516.25 ns | 11.508 |    0.06 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 2,081,623.8 ns | 172,554.99 ns |  9,458.32 ns | 34.205 |    0.21 | 3.9063 |   80382 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,487,978.6 ns |  59,589.67 ns |  3,266.31 ns | 24.450 |    0.13 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,636,433.0 ns | 286,938.24 ns | 15,728.05 ns | 59.753 |    0.36 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,457,701.6 ns |  37,946.71 ns |  2,079.99 ns | 23.953 |    0.12 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,150,357.1 ns | 128,206.31 ns |  7,027.42 ns | 35.334 |    0.20 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    84,445.0 ns |   1,241.58 ns |     68.06 ns |  1.388 |    0.01 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   268,724.6 ns |   7,569.18 ns |    414.89 ns |  4.416 |    0.02 |      - |     745 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   197,113.9 ns |  12,246.65 ns |    671.28 ns |  3.239 |    0.02 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   381,281.1 ns |  33,386.42 ns |  1,830.02 ns |  6.265 |    0.04 | 4.3945 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       459.7 ns |      56.71 ns |      3.11 ns |  0.008 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,562.7 ns |     176.44 ns |      9.67 ns |  0.026 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |       123.0 ns |      15.89 ns |      0.87 ns |  0.002 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       691.0 ns |      52.91 ns |      2.90 ns |  0.011 |    0.00 | 0.0572 |     968 B |          NA |

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
| **Lodestar_Levene**          | **100**       |     **2,143.2 ns** |    **547.26 ns** |    **30.00 ns** |  **1.00** |    **0.02** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,083.3 ns |  1,480.22 ns |    81.14 ns |  1.91 |    0.04 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       607.7 ns |     39.97 ns |     2.19 ns |  0.28 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,185.7 ns |     61.70 ns |     3.38 ns |  0.55 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       767.2 ns |     13.61 ns |     0.75 ns |  0.36 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,685.6 ns |    126.82 ns |     6.95 ns |  2.19 |    0.03 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,643.6 ns |    531.34 ns |    29.12 ns |  4.03 |    0.05 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,298.4 ns |    248.74 ns |    13.63 ns |  2.94 |    0.04 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,145.8 ns |    466.92 ns |    25.59 ns |  1.93 |    0.03 |       - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **396,160.5 ns** | **10,492.51 ns** |   **575.13 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,348,212.6 ns |  8,708.54 ns |   477.34 ns | 3.403 |    0.00 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,284.7 ns |    942.50 ns |    51.66 ns | 0.142 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,570.6 ns |  1,700.49 ns |    93.21 ns | 0.284 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,002.4 ns |     47.99 ns |     2.63 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   897,236.5 ns | 51,751.14 ns | 2,836.65 ns | 2.265 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,397,759.8 ns | 31,601.35 ns | 1,732.18 ns | 3.528 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,869.8 ns |    270.12 ns |    14.81 ns | 0.027 |    0.00 |       - |      - |      48 B |       0.000 |

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

| Method                 | Rows | Method   | Mean           | Error        | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,943.5 μs** |     **280.3 μs** |     **15.36 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   103,563.2 μs |  13,028.8 μs |    714.15 μs |  53.29 |    0.49 |  3666.6667 |  1500.0000 |  833.3333 |  59880.73 KB |       58.56 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,833.5 μs** |     **214.5 μs** |     **11.76 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |   100,296.0 μs |  36,714.4 μs |  2,012.44 μs |  54.70 |    1.00 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **798.9 μs** |     **230.2 μs** |     **12.62 μs** |   **1.00** |    **0.02** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   138,543.7 μs |  84,963.3 μs |  4,657.12 μs | 173.45 |    5.59 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,330.2 μs** |     **278.0 μs** |     **15.24 μs** |   **1.00** |    **0.01** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    73,752.2 μs |  57,747.2 μs |  3,165.32 μs |  31.65 |    1.19 |  3857.1429 |  1714.2857 |  857.1429 |  64191.22 KB |       62.78 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **16,345.7 μs** |   **8,626.5 μs** |    **472.85 μs** |   **1.00** |    **0.04** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,813,527.9 μs | 154,018.4 μs |  8,442.27 μs | 111.01 |    2.84 | 35000.0000 | 10000.0000 | 4000.0000 | 537147.58 KB |       60.18 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **13,995.4 μs** |   **1,050.4 μs** |     **57.58 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,713,405.9 μs | 150,401.5 μs |  8,244.01 μs | 122.43 |    0.67 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,451.7 μs** |     **167.6 μs** |      **9.19 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,165,821.7 μs | 289,114.1 μs | 15,847.32 μs | 490.70 |    2.21 | 47000.0000 | 12000.0000 | 4000.0000 | 732673.15 KB |    8,147.89 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,877.7 μs** |   **2,526.8 μs** |    **138.50 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,607,813.9 μs |  94,098.0 μs |  5,157.83 μs |  95.27 |    0.72 | 38000.0000 | 11000.0000 | 4000.0000 | 573613.83 KB |       64.27 |

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

| Method           | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 27.66 ms | 19.47 ms | 1.067 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 47.15 ms | 36.55 ms | 2.003 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.93 MB |
| CountLoad4M      | 46.73 ms | 20.69 ms | 1.134 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

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
| CountFitLongDocuments | 177,431.59 μs | 40,469.673 μs | 2,218.279 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     424.73 μs |     95.771 μs |     5.250 μs |    9.2773 |    2.9297 |   2.9297 |   504.67 KB |
| CountSaveAsync240K    |     503.54 μs |     70.626 μs |     3.871 μs |    9.7656 |    3.9063 |   3.9063 |   739.44 KB |
| TfidfSave45K          |      51.23 μs |      2.888 μs |     0.158 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      54.39 μs |      3.088 μs |     0.169 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     300.25 μs |     60.841 μs |     3.335 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     286.48 μs |     46.856 μs |     2.568 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,784.35 μs | 38,659.659 μs | 2,119.066 μs |  593.7500 |  562.5000 | 187.5000 |  22048.8 KB |
| CountLoadAsync4M      |  56,721.72 μs | 26,529.222 μs | 1,454.156 μs | 1100.0000 | 1000.0000 | 300.0000 | 44983.52 KB |
| CountLoad4M           |  51,104.33 μs | 22,049.221 μs | 1,208.592 μs | 1100.0000 | 1000.0000 | 300.0000 |  44976.8 KB |

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
| TfidfSaveAsyncSmall    |     18.272 μs |    16.8865 μs |   0.9256 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    501.110 μs |   129.9107 μs |   7.1208 μs |   35.1563 |  29.2969 |  29.2969 |   740.13 KB |
| HashingLoadUndeclared  |      5.729 μs |     0.1608 μs |   0.0088 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     30.387 μs |     7.7689 μs |   0.4258 μs |    7.3853 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    129.573 μs |    22.4383 μs |   1.2299 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    850.967 μs |   746.9098 μs |  40.9406 μs |   38.0859 |  37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,054.515 μs | 2,504.5029 μs | 137.2803 μs |  515.6250 | 500.0000 | 296.8750 | 10793.92 KB |
| CountSaveAsync1M       |  2,454.067 μs |   924.9651 μs |  50.7005 μs |  109.3750 | 101.5625 | 101.5625 |  3162.04 KB |
| IndexSaveAsyncLongHead |  3,440.599 μs |   654.4688 μs |  35.8736 μs |   93.7500 |  89.8438 |  89.8438 |  2124.36 KB |
| CountLoadUndeclared2M  | 19,519.867 μs | 2,008.7314 μs | 110.1053 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.12 KB |

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.964 μs** |  **0.4599 μs** | **0.0252 μs** |  **1.00** |    **0.01** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.233 μs |  0.6233 μs | 0.0342 μs |  1.05 |    0.01 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   6.150 μs |  0.7754 μs | 0.0425 μs |  1.03 |    0.01 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **59.159 μs** |  **7.8402 μs** | **0.4298 μs** |  **1.00** |    **0.01** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  22.575 μs |  1.5865 μs | 0.0870 μs |  0.38 |    0.00 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  22.616 μs |  4.6747 μs | 0.2562 μs |  0.38 |    0.00 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **232.333 μs** | **64.2683 μs** | **3.5228 μs** |  **1.00** |    **0.02** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  84.371 μs | 14.5624 μs | 0.7982 μs |  0.36 |    0.01 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  73.096 μs |  5.2529 μs | 0.2879 μs |  0.31 |    0.00 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **910.750 μs** | **55.4149 μs** | **3.0375 μs** |  **1.00** |    **0.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 331.808 μs | 39.7734 μs | 2.1801 μs |  0.36 |    0.00 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 281.834 μs | 25.4979 μs | 1.3976 μs |  0.31 |    0.00 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

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
| **LengthFilteredScan** | **1**      | **clustered** | **134.11 ms** | **19.718 ms** | **1.081 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.38 ms |  2.792 ms | 0.153 ms |  0.49 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **141.58 ms** | **53.754 ms** | **2.946 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  62.03 ms |  3.637 ms | 0.199 ms |  0.44 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **200.58 ms** | **10.575 ms** | **0.580 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 243.24 ms |  8.264 ms | 0.453 ms |  1.21 |    0.00 |   258.9 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **206.22 ms** |  **9.802 ms** | **0.537 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 212.17 ms | 15.242 ms | 0.835 ms |  1.03 |    0.00 |  192.71 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **254.58 ms** | **16.435 ms** | **0.901 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 344.63 ms | 36.261 ms | 1.988 ms |  1.35 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **260.01 ms** | **13.634 ms** | **0.747 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 314.35 ms |  4.405 ms | 0.241 ms |  1.21 |    0.00 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **293.88 ms** | **22.719 ms** | **1.245 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 396.06 ms | 18.107 ms | 0.993 ms |  1.35 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **299.40 ms** | **24.905 ms** | **1.365 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 392.67 ms | 31.416 ms | 1.722 ms |  1.31 |    0.01 |  7964.5 KB |        1.44 |

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

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **44.73 μs** |      **0.345 μs** |     **0.019 μs** |         **-** |
| Cjk    | 1000   |      63.34 μs |      1.083 μs |     0.059 μs |         - |
| **Latin**  | **10000**  |   **4,122.74 μs** |     **67.946 μs** |     **3.724 μs** |         **-** |
| Cjk    | 10000  |   6,826.37 μs |  1,147.257 μs |    62.885 μs |         - |
| **Latin**  | **65536**  | **185,165.24 μs** | **30,348.469 μs** | **1,663.502 μs** |         **-** |

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
| Unigram | 30.02 ms |  5.184 ms | 0.284 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 94.74 ms | 39.726 ms | 2.178 ms |  3.16 |    0.07 | 1666.6667 |  28.47 MB |        3.48 |

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
| **BpeOnOnePathologicalToken** | **512**    |  **18.71 μs** |  **0.565 μs** | **0.031 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **66.04 μs** |  **2.125 μs** | **0.116 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **180.91 μs** | **14.670 μs** | **0.804 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **389.04 μs** | **25.786 μs** | **1.413 μs** | **3.4180** |  **56.63 KB** |

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

| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| EncodeUnseenProse | 9.555 ms | 0.9741 ms | 0.0534 ms |    1.7 MB |

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

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **19.305 μs** |  **3.034 μs** | **0.1663 μs** |         **-** |
| MyersGroup | cjk      | 241.918 μs | 42.707 μs | 2.3409 μs |         - |
| **DpGroup**    | **latin**    |   **9.717 μs** |  **1.105 μs** | **0.0605 μs** |         **-** |
| MyersGroup | latin    | 133.242 μs | 12.117 μs | 0.6642 μs |         - |

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **308.20 μs** | **33.939 μs** | **1.860 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 302.81 μs |  7.483 μs | 0.410 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **329.23 μs** | **18.219 μs** | **0.999 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 319.90 μs | 34.501 μs | 1.891 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **319.38 μs** | **63.202 μs** | **3.464 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 306.96 μs | 69.507 μs | 3.810 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **274.10 μs** | **22.520 μs** | **1.234 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 322.08 μs | 10.355 μs | 0.568 μs |  1.18 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **314.83 μs** | **86.216 μs** | **4.726 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 308.07 μs |  6.672 μs | 0.366 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **162.99 μs** |  **7.092 μs** | **0.389 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 327.61 μs | 14.088 μs | 0.772 μs |  2.01 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **230.92 μs** |  **8.589 μs** | **0.471 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 294.42 μs | 25.228 μs | 1.383 μs |  1.27 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **92.64 μs** | **17.693 μs** | **0.970 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 320.67 μs | 49.145 μs | 2.694 μs |  3.46 |    0.04 | 1.9531 |  32.02 KB |        1.00 |

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **182.4 μs** |  **27.45 μs** |  **1.50 μs** |  **1.00** |    **0.01** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   176.8 μs |   8.15 μs |  0.45 μs |  0.97 |    0.01 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   473.4 μs |  16.34 μs |  0.90 μs |  2.59 |    0.02 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,269.0 μs** | **840.11 μs** | **46.05 μs** |  **1.00** |    **0.02** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,210.1 μs | 819.27 μs | 44.91 μs |  0.98 |    0.02 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,741.7 μs | 740.98 μs | 40.62 μs |  2.37 |    0.03 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

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

| Method       | Shape     | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **155.71 ms** |  **98.227 ms** | **5.384 ms** |  **1.00** |    **0.04** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,324.94 ms | 129.531 ms | 7.100 ms | 14.94 |    0.44 | 14000.0000 | 8000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |            |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **54.22 ms** |   **3.046 ms** | **0.167 ms** |  **1.00** |    **0.00** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   814.04 ms |  20.549 ms | 1.126 ms | 15.01 |    0.04 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

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
| **Lodestar_Fit**             | **20000x2x10** |   **163.46 ms** |  **49.149 ms** | **2.694 ms** |  **1.00** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,679.47 ms | 126.575 ms | 6.938 ms | 40.87 |    0.59 | 38000.0000 | 14000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,885.39 ms |  84.642 ms | 4.640 ms | 23.77 |    0.34 | 21000.0000 | 11000.0000 |  9000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |          |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **18.98 ms** |   **0.694 ms** | **0.038 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   474.47 ms |  51.365 ms | 2.816 ms | 24.99 |    0.14 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   269.32 ms |  76.108 ms | 4.172 ms | 14.19 |    0.19 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

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

| Method                                    | Mean      | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       | 13.330 ms | 0.6252 ms | 0.0343 ms |  1.00 |    0.00 |
| Nmf_Rank20                                | 95.925 ms | 2.2844 ms | 0.1252 ms |  7.20 |    0.02 |
| Nmf_Transform                             |  3.172 ms | 0.1495 ms | 0.0082 ms |  0.24 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 24.835 ms | 2.0554 ms | 0.1127 ms |  1.86 |    0.01 |

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

| Method         | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.94 ns |   1.486 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   751.40 ns |  75.555 ns | 4.141 ns |  7.67 |    0.04 |      - |         - |          NA |
| TokenSortRatio |   863.62 ns | 133.053 ns | 7.293 ns |  8.82 |    0.06 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   924.08 ns | 124.193 ns | 6.807 ns |  9.43 |    0.06 | 0.0534 |     896 B |          NA |
| WRatio         | 1,297.90 ns | 120.444 ns | 6.602 ns | 13.25 |    0.06 | 0.0668 |    1120 B |          NA |

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

| Method                    | Mean               | Error              | StdDev            | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|------------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,038,584.6764 ns |     18,378.9352 ns |     1,007.4115 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    666,562.4105 ns |     77,102.4190 ns |     4,226.2438 ns |  0.327 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,120,643.4961 ns |     73,068.6644 ns |     4,005.1401 ns |  0.550 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 71,815,277.5833 ns | 20,148,159.5431 ns | 1,104,388.6159 ns | 35.228 |    0.47 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |         0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

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

| Method     | Operation     | Mean         | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.96 ns** |  **20.94 ns** |  **1.148 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    206.19 ns |  11.30 ns |  0.619 ns |  2.08 |    0.02 | 0.0048 |      80 B |          NA |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **760.16 ns** |  **94.26 ns** |  **5.166 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,541.72 ns | 627.37 ns | 34.388 ns | 13.87 |    0.09 |      - |     160 B |          NA |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,019.69 ns** | **151.72 ns** |  **8.317 ns** |  **1.00** |    **0.01** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,143.75 ns | 165.61 ns |  9.077 ns |  2.10 |    0.02 | 0.1144 |    1944 B |        2.17 |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,206.79 ns** |  **78.23 ns** |  **4.288 ns** |  **1.00** |    **0.00** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,914.38 ns | 133.73 ns |  7.330 ns |  4.07 |    0.01 | 0.1831 |    3096 B |        2.76 |

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
| **Distance_Utf16**             | **8**      |    **26.34 ns** |   **0.677 ns** |  **0.037 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.10 ns |   0.579 ns |  0.032 ns |  1.56 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    28.32 ns |   0.852 ns |  0.047 ns |  1.08 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.35 ns |   1.359 ns |  0.074 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.39 ns** |   **0.594 ns** |  **0.033 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.85 ns |   0.991 ns |  0.054 ns |  1.47 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.44 ns |   0.128 ns |  0.007 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.62 ns |   0.225 ns |  0.012 ns |  0.97 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **31.62 ns** |   **3.046 ns** |  **0.167 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.67 ns |   9.134 ns |  0.501 ns |  1.44 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    35.18 ns |   0.793 ns |  0.043 ns |  1.11 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.16 ns |   6.664 ns |  0.365 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **34.83 ns** |   **0.157 ns** |  **0.009 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    48.35 ns |   8.376 ns |  0.459 ns |  1.39 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    36.31 ns |   0.241 ns |  0.013 ns |  1.04 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    32.89 ns |   0.709 ns |  0.039 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **58.32 ns** |   **9.969 ns** |  **0.546 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    61.59 ns |   5.920 ns |  0.325 ns |  1.06 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.68 ns |  18.415 ns |  1.009 ns |  0.99 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    56.99 ns |  15.451 ns |  0.847 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **62.52 ns** |   **0.530 ns** |  **0.029 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.74 ns |   2.182 ns |  0.120 ns |  1.12 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    67.03 ns |  11.501 ns |  0.630 ns |  1.07 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    62.13 ns |  12.636 ns |  0.693 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **405.30 ns** |   **4.344 ns** |  **0.238 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   421.04 ns | 108.933 ns |  5.971 ns |  1.04 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   407.91 ns |   9.492 ns |  0.520 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   404.38 ns |   2.505 ns |  0.137 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,680.65 ns** |  **52.577 ns** |  **2.882 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,889.40 ns | 358.746 ns | 19.664 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,613.86 ns | 189.085 ns | 10.364 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,694.95 ns | 795.059 ns | 43.580 ns |  1.00 |    0.01 |         - |          NA |

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **389.9 ns** |    **67.53 ns** |   **3.70 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    190.2 ns |    42.03 ns |   2.30 ns |  0.49 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,415.8 ns** |    **53.90 ns** |   **2.95 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,050.9 ns |    66.99 ns |   3.67 ns |  1.26 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,370.2 ns** |   **322.85 ns** |  **17.70 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,518.2 ns | 6,663.54 ns | 365.25 ns |  2.36 |    0.02 |         - |          NA |

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,738.1 ns** | **4,025.28 ns** | **220.64 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  9,145.6 ns | 3,538.23 ns | 193.94 ns |  1.05 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 12,021.5 ns | 1,412.59 ns |  77.43 ns |  1.38 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,998.8 ns** | **4,598.80 ns** | **252.08 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,387.6 ns | 1,306.56 ns |  71.62 ns |  0.94 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,588.3 ns | 1,274.46 ns |  69.86 ns |  1.46 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,214.4 ns** |   **272.82 ns** |  **14.95 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,435.0 ns | 1,145.02 ns |  62.76 ns |  1.02 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,245.3 ns | 2,331.98 ns | 127.82 ns |  1.49 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,365.0 ns** | **3,687.45 ns** | **202.12 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,615.2 ns |   839.00 ns |  45.99 ns |  1.23 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,467.6 ns |   699.11 ns |  38.32 ns |  1.77 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **6,983.3 ns** | **4,126.17 ns** | **226.17 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,206.8 ns | 3,129.57 ns | 171.54 ns |  1.03 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,337.0 ns | 1,451.21 ns |  79.55 ns |  1.62 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,032.0 ns** | **3,536.16 ns** | **193.83 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,200.4 ns | 6,560.74 ns | 359.62 ns |  1.02 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,699.6 ns | 2,343.71 ns | 128.47 ns |  1.74 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,771.8 ns** |    **73.29 ns** |   **4.02 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,068.0 ns |   394.13 ns |  21.60 ns |  1.08 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,308.1 ns |   232.17 ns |  12.73 ns |  1.94 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,384.2 ns** |   **567.32 ns** |  **31.10 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,269.2 ns |   749.51 ns |  41.08 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,899.1 ns |   839.57 ns |  46.02 ns |  1.84 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,941.5 ns** |    **43.82 ns** |   **2.40 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,531.2 ns |   244.63 ns |  13.41 ns |  0.95 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 14,236.8 ns |   674.39 ns |  36.97 ns |  1.79 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,323.0 ns** |    **18.25 ns** |   **1.00 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,134.5 ns |   109.50 ns |   6.00 ns |  0.92 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,072.4 ns |   311.08 ns |  17.05 ns |  2.61 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,071.2 ns** |   **120.50 ns** |   **6.60 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,863.7 ns |   214.60 ns |  11.76 ns |  0.70 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,668.9 ns | 1,346.45 ns |  73.80 ns |  2.13 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,538.0 ns** |   **174.02 ns** |   **9.54 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,836.9 ns |   306.64 ns |  16.81 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,811.5 ns |   598.51 ns |  32.81 ns |  1.96 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,638.6 ns** |    **75.77 ns** |   **4.15 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,417.7 ns |   276.09 ns |  15.13 ns |  0.94 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,325.9 ns |   242.71 ns |  13.30 ns |  2.01 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,357.3 ns** |    **69.62 ns** |   **3.82 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,130.6 ns |   249.44 ns |  13.67 ns |  0.77 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,239.3 ns |   733.12 ns |  40.19 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,557.4 ns** |   **162.32 ns** |   **8.90 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,210.5 ns |   650.49 ns |  35.66 ns |  1.09 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,506.5 ns | 1,925.68 ns | 105.55 ns |  1.92 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,300.6 ns** |    **40.55 ns** |   **2.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    609.0 ns |    29.63 ns |   1.62 ns |  0.47 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,321.9 ns |   195.06 ns |  10.69 ns |  4.09 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,085.4 ns** |    **49.55 ns** |   **2.72 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,428.8 ns |   176.63 ns |   9.68 ns |  0.46 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,847.8 ns |   306.61 ns |  16.81 ns |  2.54 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,187.6 ns** |    **55.17 ns** |   **3.02 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,386.3 ns |   124.67 ns |   6.83 ns |  0.46 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,049.4 ns |   828.95 ns |  45.44 ns |  2.32 |    0.01 | 0.3052 |    5120 B |          NA |

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
| **Lodestar_Fit**     | **10000x16x16** |   **5,439.4 μs** |    **585.15 μs** |    **32.07 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  39,365.3 μs |  4,307.10 μs |   236.09 μs |  7.24 |    0.05 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  83,550.0 μs |  3,263.67 μs |   178.89 μs | 15.36 |    0.08 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **793.8 μs** |     **54.54 μs** |     **2.99 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,769.4 μs |    153.38 μs |     8.41 μs |  6.01 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,054.3 μs |    509.27 μs |    27.91 μs |  3.85 |    0.03 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,328.8 μs** |  **1,766.96 μs** |    **96.85 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 441,388.8 μs | 57,507.17 μs | 3,152.16 μs | 16.76 |    0.12 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 744,914.0 μs | 66,516.74 μs | 3,646.01 μs | 28.29 |    0.15 |        - |        - |        - |  199.91 KB |       0.046 |

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.049 ms** |  **0.1936 ms** | **0.0106 ms** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.632 ms |  0.5668 ms | 0.0311 ms |  3.29 |    0.01 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.409 ms** |  **3.1353 ms** | **0.1719 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 112.440 ms |  1.2059 ms | 0.0661 ms |  4.26 |    0.02 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.871 ms** |  **6.4612 ms** | **0.3542 ms** |  **1.00** |    **0.01** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 207.864 ms | 10.1287 ms | 0.5552 ms |  4.43 |    0.03 |        - |        - |        - |   36.47 KB |       0.009 |

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
| **Dp**         | **8**    |    **133.53 ns** |     **9.836 ns** |   **0.539 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.89 ns |     4.536 ns |   0.249 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.38 ns |    15.246 ns |   0.836 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |     97.92 ns |    10.263 ns |   0.563 ns |  0.73 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **216.48 ns** |    **11.691 ns** |   **0.641 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     62.71 ns |     0.811 ns |   0.044 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    227.29 ns |   210.724 ns |  11.551 ns |  1.05 |    0.05 |         - |          NA |
| Kernel_Cjk | 12   |    110.16 ns |     5.553 ns |   0.304 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **274.98 ns** |   **239.775 ns** |  **13.143 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 14   |     66.18 ns |     0.213 ns |   0.012 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    284.11 ns |   244.040 ns |  13.377 ns |  1.03 |    0.06 |         - |          NA |
| Kernel_Cjk | 14   |    114.74 ns |     4.582 ns |   0.251 ns |  0.42 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **354.44 ns** |    **89.536 ns** |   **4.908 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 16   |     80.45 ns |     4.992 ns |   0.274 ns |  0.23 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    351.03 ns |    53.305 ns |   2.922 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    119.29 ns |     2.060 ns |   0.113 ns |  0.34 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **438.37 ns** |    **91.155 ns** |   **4.997 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.72 ns |     1.055 ns |   0.058 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    443.11 ns |   167.549 ns |   9.184 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 18   |    123.08 ns |     1.796 ns |   0.098 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **773.39 ns** |    **35.399 ns** |   **1.940 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     79.33 ns |     6.898 ns |   0.378 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    753.96 ns |   137.389 ns |   7.531 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    134.37 ns |     2.375 ns |   0.130 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **991.52 ns** |    **82.904 ns** |   **4.544 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     87.18 ns |    21.580 ns |   1.183 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    983.00 ns |   427.378 ns |  23.426 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    139.80 ns |     2.582 ns |   0.142 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,558.96 ns** |   **439.142 ns** |  **24.071 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 32   |    105.75 ns |     7.827 ns |   0.429 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,558.19 ns |   512.154 ns |  28.073 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |    167.50 ns |     0.589 ns |   0.032 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,207.60 ns** |    **48.364 ns** |   **2.651 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    132.97 ns |     4.185 ns |   0.229 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,306.59 ns | 3,892.702 ns | 213.372 ns |  1.03 |    0.06 |         - |          NA |
| Kernel_Cjk | 48   |    240.82 ns |     6.736 ns |   0.369 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,773.81 ns** | **9,645.424 ns** | **528.698 ns** |  **1.01** |    **0.11** |         **-** |          **NA** |
| Kernel     | 64   |    163.20 ns |     2.684 ns |   0.147 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,494.60 ns |   108.517 ns |   5.948 ns |  0.96 |    0.08 |         - |          NA |
| Kernel_Cjk | 64   |    290.08 ns |     6.375 ns |   0.349 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,833.37 ns** |   **703.312 ns** |  **38.551 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    351.14 ns |     6.217 ns |   0.341 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,164.81 ns | 3,661.771 ns | 200.714 ns |  1.03 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    942.78 ns |    15.878 ns |   0.870 ns |  0.08 |    0.00 |         - |          NA |

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

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.65 ns** |     **0.574 ns** |  **0.031 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.77 ns |     0.847 ns |  0.046 ns |  1.00 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    121.60 ns |    10.706 ns |  0.587 ns |  4.93 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.04 ns |     0.155 ns |  0.009 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **277.22 ns** |    **20.428 ns** |  **1.120 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    363.06 ns |     6.187 ns |  0.339 ns |  1.31 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    664.29 ns |   106.362 ns |  5.830 ns |  2.40 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    276.11 ns |     7.118 ns |  0.390 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **916.29 ns** |    **27.611 ns** |  **1.513 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,994.82 ns | 1,656.128 ns | 90.778 ns |  2.18 |    0.09 |         - |          NA |
| Distance_CodePoint         | 128    |  1,611.76 ns |   161.703 ns |  8.863 ns |  1.76 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    925.88 ns |   108.595 ns |  5.952 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,079.00 ns** |   **480.597 ns** | **26.343 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,552.05 ns |   685.884 ns | 37.596 ns |  1.49 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 13,290.86 ns |   381.873 ns | 20.932 ns |  1.20 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,988.56 ns |   716.176 ns | 39.256 ns |  0.99 |    0.00 |         - |          NA |

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
| **Distance_CodePoint** | **16**     | **32**       |     **330.7 ns** |     **31.06 ns** |     **1.70 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     238.3 ns |      3.64 ns |     0.20 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **353.9 ns** |     **43.01 ns** |     **2.36 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     243.3 ns |     50.64 ns |     2.78 ns |  0.69 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **429.5 ns** |      **9.42 ns** |     **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.4 ns |      2.65 ns |     0.15 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **426.4 ns** |     **14.82 ns** |     **0.81 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     324.6 ns |      1.53 ns |     0.08 ns |  0.76 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **500.7 ns** |     **29.11 ns** |     **1.60 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     410.2 ns |     61.36 ns |     3.36 ns |  0.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **518.9 ns** |     **17.81 ns** |     **0.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     418.3 ns |      3.75 ns |     0.21 ns |  0.81 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **676.6 ns** |    **457.94 ns** |    **25.10 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,296.3 ns |     50.06 ns |     2.74 ns |  1.92 |    0.06 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **608.2 ns** |     **93.59 ns** |     **5.13 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,306.1 ns |     65.06 ns |     3.57 ns |  2.15 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,960.4 ns** |    **763.18 ns** |    **41.83 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,455.1 ns |    186.17 ns |    10.20 ns |  2.78 |    0.05 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,939.3 ns** |     **95.46 ns** |     **5.23 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,483.8 ns |    731.25 ns |    40.08 ns |  2.83 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,963.1 ns** |    **959.42 ns** |    **52.59 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,232.5 ns |    793.06 ns |    43.47 ns |  3.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **448,725.7 ns** | **54,911.84 ns** | **3,009.90 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  61,652.8 ns |  1,005.88 ns |    55.14 ns |  0.14 |    0.00 |         - |          NA |

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

| Method               | Length | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **24.68 ns** |       **0.171 ns** |     **0.009 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      85.22 ns |      21.123 ns |     1.158 ns |  3.45 |    0.04 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      83.34 ns |       4.391 ns |     0.241 ns |  3.38 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     183.79 ns |       6.272 ns |     0.344 ns |  7.45 |    0.01 | 0.0076 |     128 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **277.08 ns** |      **45.346 ns** |     **2.486 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,233.73 ns |     654.459 ns |    35.873 ns | 22.50 |    0.21 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,422.79 ns |      68.320 ns |     3.745 ns |  5.14 |    0.04 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,292.65 ns |   2,111.410 ns |   115.734 ns | 37.15 |    0.46 | 0.0305 |     576 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,201.38 ns** |     **457.324 ns** |    **25.067 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 390,768.08 ns | 115,036.836 ns | 6,305.557 ns | 34.89 |    0.49 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,112.92 ns |   1,626.403 ns |    89.149 ns |  3.31 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 710,703.46 ns |  37,139.909 ns | 2,035.764 ns | 63.45 |    0.20 |      - |    4161 B |          NA |

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

| Method            | Labels | Mean     | Error    | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|---------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **595.1 μs** | **45.75 μs** | **2.51 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 602.4 μs | 66.58 μs | 3.65 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 593.9 μs |  7.27 μs | 0.40 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **773.5 μs** | **23.26 μs** | **1.27 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 767.3 μs | 44.77 μs | 2.45 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 907.9 μs | 49.69 μs | 2.72 μs | 46.8750 | 18.5547 | 767.27 KB |

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

| Method                          | Shape   | Mean          | Error          | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.94 μs** |       **1.390 μs** |     **0.076 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 100,088.02 μs |  29,867.530 μs | 1,637.140 μs | 747.24 |   10.59 | 833.3333 | 833.3333 | 833.3333 | 31414.76 KB |   12,806.02 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,914.65 μs** |     **354.972 μs** |    **19.457 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 485,828.50 μs | 147,606.039 μs | 8,090.785 μs | 166.69 |    2.59 |        - |        - |        - | 32054.48 KB |      760.62 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.61 μs** |       **2.375 μs** |     **0.130 μs** |   **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     873.11 μs |     121.577 μs |     6.664 μs |  38.62 |    0.32 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

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
| **Matrix**         | **1000**    | **2**       |     **4,452.0 ns** |   **1,527.29 ns** |     **83.72 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,168.2 ns |     643.81 ns |     35.29 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       109.6 ns |       1.65 ns |      0.09 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,998.1 ns |     272.69 ns |     14.95 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,198.7 ns |     111.90 ns |      6.13 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,633.8 ns** |      **46.83 ns** |      **2.57 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,414.9 ns |     616.25 ns |     33.78 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       109.5 ns |      32.50 ns |      1.78 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,439.6 ns |     848.78 ns |     46.52 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,381.9 ns |   2,572.82 ns |    141.03 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **462,779.0 ns** | **121,502.74 ns** |  **6,659.98 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   824,199.8 ns |  26,867.32 ns |  1,472.69 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,226.1 ns |     780.26 ns |     42.77 ns |      - |         - |
| F1Macro        | 100000  | 2       |   474,280.9 ns |  38,692.82 ns |  2,120.88 ns |      - |     656 B |
| Report         | 100000  | 2       |   480,937.9 ns |  20,055.22 ns |  1,099.29 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **495,237.8 ns** |  **28,834.21 ns** |  **1,580.50 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   952,726.6 ns |  78,729.52 ns |  4,315.43 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,113.7 ns |   3,340.67 ns |    183.11 ns |      - |         - |
| F1Macro        | 100000  | 10      |   509,173.9 ns |  18,252.24 ns |  1,000.47 ns |      - |    1881 B |
| Report         | 100000  | 10      |   515,861.9 ns | 181,104.76 ns |  9,926.96 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,605,443.4 ns** |  **43,085.30 ns** |  **2,361.65 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,484,223.2 ns | 970,288.74 ns | 53,184.80 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,698.5 ns |   3,912.91 ns |    214.48 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,605,882.5 ns | 158,133.07 ns |  8,667.81 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,779,270.4 ns | 203,503.43 ns | 11,154.71 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,928,876.5 ns** | **162,037.88 ns** |  **8,881.84 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,500,225.2 ns | 312,113.11 ns | 17,107.97 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,107.2 ns |   6,128.45 ns |    335.92 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 4,920,337.4 ns | 340,462.05 ns | 18,661.87 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 4,956,473.5 ns | 144,310.74 ns |  7,910.16 ns |      - |   12878 B |

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
| **Lodestar** | **100000**  | **Bundle**        |  **10,583.92 μs** |  **1,150.650 μs** |    **63.071 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,988.73 μs | 10,227.200 μs |   560.587 μs |     3.31 |    0.05 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |    3,927.18 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **12.30 μs** |      **1.259 μs** |     **0.069 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,480.21 μs |  3,112.502 μs |   170.607 μs | 2,802.89 |   18.17 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **133,025.31 μs** | **66,714.579 μs** | **3,656.851 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 223,206.24 μs |  9,093.863 μs |   498.465 μs |     1.68 |    0.04 |        - |        - |        - | 23231768 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **108.27 μs** |      **5.867 μs** |     **0.322 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 221,409.63 μs | 12,575.386 μs |   689.299 μs | 2,044.95 |    7.62 |        - |        - |        - | 23231816 B |          NA |

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **73.51 ns** |     **1.428 ns** |   **0.078 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.70 ns |     2.042 ns |   0.112 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     74.13 ns |     2.518 ns |   0.138 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     73.49 ns |     2.727 ns |   0.150 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **109.48 ns** |    **86.073 ns** |   **4.718 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 6    |     89.53 ns |     0.561 ns |   0.031 ns |  0.82 |    0.03 |         - |          NA |
| Dp_Cjk     | 6    |    105.03 ns |    17.056 ns |   0.935 ns |  0.96 |    0.04 |         - |          NA |
| Kernel_Cjk | 6    |    130.99 ns |    13.055 ns |   0.716 ns |  1.20 |    0.05 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **159.93 ns** |     **0.400 ns** |   **0.022 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.42 ns |     3.447 ns |   0.189 ns |  0.64 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.07 ns |    10.858 ns |   0.595 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    182.74 ns |    14.610 ns |   0.801 ns |  1.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **196.16 ns** |     **5.290 ns** |   **0.290 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    109.05 ns |     0.365 ns |   0.020 ns |  0.56 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    197.87 ns |    29.637 ns |   1.624 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    150.26 ns |     5.399 ns |   0.296 ns |  0.77 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **254.56 ns** |    **57.066 ns** |   **3.128 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 12   |    119.51 ns |     7.928 ns |   0.435 ns |  0.47 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    247.14 ns |    59.777 ns |   3.277 ns |  0.97 |    0.02 |         - |          NA |
| Kernel_Cjk | 12   |    168.66 ns |    31.531 ns |   1.728 ns |  0.66 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **411.77 ns** |    **30.372 ns** |   **1.665 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    131.67 ns |    15.214 ns |   0.834 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    414.97 ns |   114.199 ns |   6.260 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    179.00 ns |     1.635 ns |   0.090 ns |  0.43 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **842.77 ns** |     **9.818 ns** |   **0.538 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    171.86 ns |     0.519 ns |   0.028 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    847.79 ns |   134.181 ns |   7.355 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    224.75 ns |     3.709 ns |   0.203 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,447.83 ns** |    **60.816 ns** |   **3.334 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    205.17 ns |    25.889 ns |   1.419 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,458.86 ns |   410.125 ns |  22.480 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    271.79 ns |     4.516 ns |   0.248 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,352.07 ns** | **1,231.593 ns** |  **67.508 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    275.13 ns |     4.534 ns |   0.249 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,323.71 ns |   664.344 ns |  36.415 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    354.40 ns |     8.265 ns |   0.453 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,272.60 ns** |   **167.387 ns** |   **9.175 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    346.20 ns |    32.633 ns |   1.789 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  6,371.64 ns | 3,626.335 ns | 198.772 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    449.75 ns |     0.495 ns |   0.027 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,335.94 ns** | **6,297.380 ns** | **345.181 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    750.56 ns |   370.224 ns |  20.293 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,001.45 ns |   605.566 ns |  33.193 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |  1,557.95 ns |   171.711 ns |   9.412 ns |  0.11 |    0.00 |         - |          NA |

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **200.66 μs** |  **72.898 μs** |  **3.996 μs** |  **1.00** |    **0.02** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   226.48 μs |  15.922 μs |  0.873 μs |  1.13 |    0.02 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    61.32 μs |   5.011 μs |  0.275 μs |  0.31 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.87 μs |   9.715 μs |  0.533 μs |  0.99 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **205.45 μs** |  **20.490 μs** |  **1.123 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   266.73 μs |   3.476 μs |  0.191 μs |  1.30 |    0.01 | 4.3945 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    61.32 μs |   5.941 μs |  0.326 μs |  0.30 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   204.09 μs |  64.814 μs |  3.553 μs |  0.99 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,041.62 μs** |  **53.964 μs** |  **2.958 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,046.63 μs | 212.055 μs | 11.623 μs |  1.00 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,079.63 μs |  87.616 μs |  4.803 μs |  0.53 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,967.19 μs |  18.753 μs |  1.028 μs |  0.96 |    0.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,982.47 μs** | **185.462 μs** | **10.166 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,099.33 μs | 188.550 μs | 10.335 μs |  1.06 |    0.01 | 3.9063 |   77627 B |       97.03 |
| SparseFit             | 100000   | 100        | 1,080.12 μs |  69.760 μs |  3.824 μs |  0.54 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,970.64 μs |  49.193 μs |  2.696 μs |  0.99 |    0.00 |      - |     803 B |        1.00 |

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

| Method      | NeedleLength | Mean         | Error     | StdDev    | Allocated |
|------------ |------------- |-------------:|----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.760 μs** | **0.3552 μs** | **0.0195 μs** |         **-** |
| EqualLength | 65           |     2.962 μs | 0.1777 μs | 0.0097 μs |         - |
| **Embedded**    | **128**          |    **38.063 μs** | **0.2936 μs** | **0.0161 μs** |         **-** |
| EqualLength | 128          |     5.294 μs | 0.0476 μs | 0.0026 μs |         - |
| **Embedded**    | **512**          | **1,317.416 μs** | **5.1653 μs** | **0.2831 μs** |         **-** |
| EqualLength | 512          |    44.870 μs | 7.9229 μs | 0.4343 μs |         - |

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
| VocabTxt               |  4.003 ms |  2.5647 ms | 0.1406 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.828 ms |  4.9145 ms | 0.2694 ms | 187.5000 | 171.8750 |  46.8750 |  5852.41 KB |
| TokenizerJsonUnigram   | 12.494 ms |  0.4583 ms | 0.0251 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.044 ms |  1.3579 ms | 0.0744 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.537 ms |  0.5750 ms | 0.0315 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.299 ms |  1.3374 ms | 0.0733 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.602 ms |  2.2803 ms | 0.1250 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.512 ms |  1.7816 ms | 0.0977 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.532 ms | 11.9264 ms | 0.6537 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 71.470 ms |  4.6345 ms | 0.2540 ms |        - |        - |        - | 16095.25 KB |

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

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,209.84 μs** |   **642.578 μs** |  **35.222 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,262.75 μs | 2,214.691 μs | 121.395 μs |  1.66 |    0.01 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **132.44 μs** |     **7.456 μs** |   **0.409 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    180.60 μs |    11.876 μs |   0.651 μs |  1.36 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,808.23 μs** |   **136.256 μs** |   **7.469 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **42.19 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,673.95 μs |    79.187 μs |   4.340 μs |  0.95 |    0.00 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.20 μs** |     **8.419 μs** |   **0.461 μs** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.66 μs |     2.801 μs |   0.154 μs |  1.11 |    0.02 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

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

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.69 μs** |     **4.800 μs** |  **0.263 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    44.40 μs |     0.449 μs |  0.025 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   109.90 μs |     2.271 μs |  0.124 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   520.25 μs |    96.340 μs |  5.281 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **454.94 μs** |    **19.855 μs** |  **1.088 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   447.76 μs |    37.548 μs |  2.058 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,101.90 μs |    62.294 μs |  3.415 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,101.73 μs | 1,320.799 μs | 72.397 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

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

| Method                 | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,215.6 ns |       210.16 ns |      11.52 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,439.3 ns |        81.38 ns |       4.46 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,234.5 ns |       116.74 ns |       6.40 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        436.8 ns |        20.51 ns |       1.12 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,976,747.6 ns | 3,868,283.65 ns | 212,033.68 ns | 875.0000 | 843.7500 | 609.3750 | 12227072 B |

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
| **Count**                | **200**       | **1.912 ms** | **0.0958 ms** | **0.0053 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.397 ms | 0.0748 ms | 0.0041 ms |  0.73 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.977 ms | 0.1124 ms | 0.0062 ms |  1.03 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.709 ms | 0.2250 ms | 0.0123 ms |  0.89 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **9.030 ms** | **0.8797 ms** | **0.0482 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.52 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.059 ms | 0.3356 ms | 0.0184 ms |  0.78 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.582 ms | 0.7961 ms | 0.0436 ms |  1.06 | 515.6250 | 500.0000 | 500.0000 | 2349.22 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.717 ms | 0.2030 ms | 0.0111 ms |  0.97 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

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

| Method       | Model         | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **20.64 ms** |   **1.464 ms** | **0.080 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  52.87 ms |   4.322 ms | 0.237 ms |  2.56 |    0.01 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.71 ms** |   **6.032 ms** | **0.331 ms** |  **1.00** |    **0.01** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  55.58 ms |   0.450 ms | 0.025 ms |  1.16 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **84.63 ms** |   **4.229 ms** | **0.232 ms** |  **1.00** |    **0.00** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 253.36 ms | 167.381 ms | 9.175 ms |  2.99 |    0.09 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **49.61 ns** | **3.401 ns** | **0.186 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  49.66 ns | 9.316 ns | 0.511 ns |  1.00 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **95.62 ns** | **8.994 ns** | **0.493 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  93.60 ns | 1.403 ns | 0.077 ns |  0.98 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **124.85 ns** | **9.016 ns** | **0.494 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 123.41 ns | 1.940 ns | 0.106 ns |  0.99 |         - |          NA |

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
| **Lodestar** | **200**       |   **3.763 ms** |   **0.1402 ms** |  **0.0077 ms** |  **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  52.099 ms | 135.0579 ms |  7.4030 ms | 13.84 |    1.70 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.119 ms** |   **5.8278 ms** |  **0.3194 ms** |  **1.00** |    **0.02** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 395.893 ms | 477.3007 ms | 26.1625 ms | 23.13 |    1.38 | 79000.0000 | 79000.0000 | 79000.0000 |  324.7 MB |       40.04 |

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
| kmeans_weighted_10000 | 5.104 | 3.958 | 0.78x | 5.112 | 7.916 | 1.55x |
| kmeans_restarts_10000 | 20.980 | 10.241 | 0.49x | 20.984 | 20.466 | 0.98x |
| kmeans_weighted_100000 | 49.967 | 28.333 | 0.57x | 50.134 | 47.713 | 0.95x |
| kmeans_restarts_100000 | 204.222 | 71.939 | 0.35x | 204.636 | 132.567 | 0.65x |
| dbscan_weighted_5000 | 7.413 | 33.395 | 4.50x | 7.799 | 33.389 | 4.28x |
| dbscan_weighted_20000 | 92.753 | 195.626 | 2.11x | 93.612 | 195.604 | 2.09x |

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
| stratified_weighted_1000 | 0.869 | 127.291 | 146.49x | 0.869 | 127.271 | 146.47x |
| ridge_robust_1000 | 0.949 | 118.254 | 124.62x | 0.949 | 118.234 | 124.62x |
| lasso_1000 | 7.246 | 1108.952 | 153.05x | 7.245 | 1108.807 | 153.05x |
| ph_test_1000 | 0.131 | 21.825 | 166.64x | 0.131 | 21.826 | 166.65x |
| predict_survival_1000 | 0.033 | 1.834 | 55.13x | 0.033 | 1.834 | 55.13x |
| time_varying_1000 | 78.403 | 353.189 | 4.50x | 78.604 | 353.146 | 4.49x |
| stratified_weighted_10000 | 9.061 | 708.147 | 78.16x | 9.097 | 708.043 | 77.83x |
| ridge_robust_10000 | 10.169 | 1107.300 | 108.89x | 10.398 | 1107.240 | 106.49x |
| lasso_10000 | 59.339 | 7109.097 | 119.80x | 59.369 | 7106.051 | 119.69x |
| ph_test_10000 | 1.999 | 212.611 | 106.36x | 2.059 | 212.596 | 103.27x |
| predict_survival_10000 | 0.038 | 1.861 | 49.41x | 0.038 | 1.861 | 49.41x |
| time_varying_10000 | 3032.236 | 5192.440 | 1.71x | 3032.005 | 5191.677 | 1.71x |

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
| cv_plus_n1000 | 4.307 | 49.420 | 11.47x | 4.307 | 196.853 | 45.71x |
| cv_minmax_n1000 | 3.166 | 5.018 | 1.58x | 3.166 | 19.999 | 6.32x |
| cv_plus_gamma_n1000 | 4.827 | 51.509 | 10.67x | 4.827 | 205.234 | 42.52x |
| bootstrap_plus_n1000 | 16.751 | 51.936 | 3.10x | 16.750 | 206.829 | 12.35x |
| cv_plus_n10000 | 42.349 | 131.049 | 3.09x | 42.345 | 476.167 | 11.24x |
| cv_minmax_n10000 | 31.393 | 27.208 | 0.87x | 31.388 | 108.367 | 3.45x |
| cv_plus_gamma_n10000 | 60.793 | 137.804 | 2.27x | 60.787 | 479.434 | 7.89x |
| bootstrap_plus_n10000 | 154.552 | 131.662 | 0.85x | 154.538 | 481.648 | 3.12x |

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
| glm_negative_binomial_n1000 | 0.401 | 3.411 | 8.50x | 0.401 | 3.411 | 8.50x |
| glm_gamma_n1000 | 0.485 | 4.250 | 8.77x | 0.485 | 4.250 | 8.77x |
| glm_poisson_exposure_n1000 | 0.404 | 4.039 | 9.99x | 0.404 | 4.039 | 10.00x |
| mnlogit_n1000 | 0.804 | 18.270 | 22.74x | 0.804 | 18.269 | 22.73x |
| glm_negative_binomial_n10000 | 3.635 | 12.095 | 3.33x | 3.645 | 12.093 | 3.32x |
| glm_gamma_n10000 | 4.181 | 13.178 | 3.15x | 4.190 | 13.177 | 3.14x |
| glm_poisson_exposure_n10000 | 4.389 | 13.190 | 3.00x | 4.400 | 13.187 | 3.00x |
| mnlogit_n10000 | 8.150 | 98.652 | 12.11x | 8.160 | 98.639 | 12.09x |
| glm_negative_binomial_n100000 | 36.575 | 136.535 | 3.73x | 36.817 | 545.147 | 14.81x |
| glm_gamma_n100000 | 42.453 | 156.844 | 3.69x | 42.839 | 626.223 | 14.62x |
| glm_poisson_exposure_n100000 | 54.779 | 134.504 | 2.46x | 55.153 | 536.774 | 9.73x |
| mnlogit_n100000 | 81.115 | 949.404 | 11.70x | 81.163 | 1695.850 | 20.89x |

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
| 2sls_robust_n1000 | 1.619 | 56.706 | 35.03x | 1.619 | 56.698 | 35.02x |
| liml_robust_n1000 | 1.770 | 57.829 | 32.68x | 1.770 | 57.823 | 32.68x |
| gmm_robust_n1000 | 1.610 | 55.748 | 34.63x | 1.610 | 55.667 | 34.58x |
| 2sls_kernel_n1000 | 8.561 | 60.331 | 7.05x | 8.561 | 60.321 | 7.05x |
| 2sls_clustered_n1000 | 1.573 | 56.426 | 35.88x | 1.572 | 56.415 | 35.88x |
| 2sls_robust_n10000 | 16.442 | 244.566 | 14.87x | 16.804 | 489.034 | 29.10x |
| liml_robust_n10000 | 18.068 | 241.364 | 13.36x | 18.604 | 482.831 | 25.95x |
| gmm_robust_n10000 | 16.480 | 236.333 | 14.34x | 16.780 | 472.546 | 28.16x |
| 2sls_kernel_n10000 | 90.724 | 257.601 | 2.84x | 91.010 | 515.551 | 5.66x |
| 2sls_clustered_n10000 | 15.484 | 265.171 | 17.13x | 15.812 | 530.128 | 33.53x |
| 2sls_robust_n100000 | 167.784 | 2561.161 | 15.26x | 169.640 | 7830.784 | 46.16x |
| liml_robust_n100000 | 181.604 | 2557.535 | 14.08x | 183.725 | 7943.582 | 43.24x |
| gmm_robust_n100000 | 169.745 | 2494.132 | 14.69x | 172.572 | 7603.568 | 44.06x |
| 2sls_kernel_n100000 | 3329.648 | 3251.464 | 0.98x | 3331.798 | 10100.081 | 3.03x |
| 2sls_clustered_n100000 | 159.061 | 2780.375 | 17.48x | 160.367 | 8873.294 | 55.33x |

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
| transform_frobenius_n100 | 2.045 | 0.860 | 0.42x | 2.045 | 0.859 | 0.42x |
| transform_frobenius_n500 | 6.560 | 1.656 | 0.25x | 6.559 | 1.656 | 0.25x |
| transform_frobenius_n2000 | 23.819 | 5.876 | 0.25x | 23.861 | 23.445 | 0.98x |
| transform_kl_n100 | 1.251 | 17.872 | 14.29x | 1.251 | 17.872 | 14.29x |
| transform_kl_n500 | 5.697 | 34.771 | 6.10x | 5.696 | 34.769 | 6.10x |
| transform_kl_n2000 | 22.631 | 123.961 | 5.48x | 22.698 | 452.622 | 19.94x |

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
| ols_summary_n1000 | 0.045 | 3.014 | 66.96x | 0.045 | 3.014 | 66.95x |
| ols_hac_n1000 | 0.188 | 3.491 | 18.55x | 0.188 | 3.491 | 18.55x |
| ols_cluster_n1000 | 0.063 | 3.539 | 56.08x | 0.063 | 3.539 | 56.08x |
| ols_summary_n10000 | 0.434 | 11.450 | 26.40x | 0.434 | 11.449 | 26.40x |
| ols_hac_n10000 | 1.932 | 12.986 | 6.72x | 1.944 | 12.984 | 6.68x |
| ols_cluster_n10000 | 0.603 | 13.328 | 22.12x | 0.607 | 13.327 | 21.95x |
| ols_summary_n100000 | 4.326 | 111.997 | 25.89x | 4.344 | 447.281 | 102.96x |
| ols_hac_n100000 | 19.200 | 123.277 | 6.42x | 19.534 | 492.119 | 25.19x |
| ols_cluster_n100000 | 6.644 | 127.855 | 19.24x | 6.859 | 510.634 | 74.45x |

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
| sparse_10000 | 1.680 | 4.479 | 2.67x | 1.685 | 4.479 | 2.66x |
| sparse_infrequent_10000 | 2.127 | 4.718 | 2.22x | 2.132 | 4.717 | 2.21x |
| sparse_100000 | 16.767 | 28.995 | 1.73x | 17.209 | 28.992 | 1.68x |
| sparse_infrequent_100000 | 19.055 | 24.521 | 1.29x | 19.518 | 24.518 | 1.26x |

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
| fe_entity_n1000 | 0.227 | 20.415 | 89.93x | 0.227 | 20.413 | 89.94x |
| fe_twoway_clustered_n1000 | 0.392 | 32.062 | 81.80x | 0.392 | 32.060 | 81.81x |
| fe_kernel_n1000 | 0.246 | 20.692 | 84.15x | 0.246 | 20.690 | 84.15x |
| between_robust_n1000 | 0.125 | 19.534 | 156.59x | 0.125 | 19.529 | 156.56x |
| first_difference_robust_n1000 | 0.250 | 24.106 | 96.46x | 0.250 | 24.099 | 96.46x |
| random_effects_n1000 | 0.237 | 26.687 | 112.83x | 0.237 | 26.682 | 112.82x |
| fe_entity_n10000 | 2.726 | 29.487 | 10.82x | 2.822 | 29.483 | 10.45x |
| fe_twoway_clustered_n10000 | 4.910 | 55.611 | 11.33x | 5.162 | 111.232 | 21.55x |
| fe_kernel_n10000 | 2.957 | 30.025 | 10.15x | 3.005 | 30.024 | 9.99x |
| between_robust_n10000 | 1.788 | 28.506 | 15.94x | 1.795 | 28.504 | 15.88x |
| first_difference_robust_n10000 | 2.953 | 34.158 | 11.57x | 2.962 | 34.156 | 11.53x |
| random_effects_n10000 | 2.827 | 39.705 | 14.05x | 2.866 | 39.700 | 13.85x |
| fe_entity_n100000 | 31.285 | 179.499 | 5.74x | 32.503 | 716.150 | 22.03x |
| fe_twoway_clustered_n100000 | 48.739 | 348.748 | 7.16x | 50.047 | 1256.178 | 25.10x |
| fe_kernel_n100000 | 32.137 | 164.846 | 5.13x | 33.552 | 656.780 | 19.57x |
| between_robust_n100000 | 18.762 | 161.800 | 8.62x | 19.346 | 529.826 | 27.39x |
| first_difference_robust_n100000 | 33.986 | 192.641 | 5.67x | 35.337 | 766.756 | 21.70x |
| random_effects_n100000 | 31.565 | 218.059 | 6.91x | 32.549 | 869.232 | 26.71x |

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
| welch_t_n1000 | 0.006 | 0.821 | 145.10x | 0.006 | 0.821 | 145.10x |
| mann_whitney_n1000 | 0.030 | 0.754 | 25.43x | 0.030 | 0.753 | 25.43x |
| chi_square_n1000 | 0.000 | 0.341 | 1644.57x | 0.000 | 0.341 | 1644.60x |
| fligner_n1000 | 0.077 | 0.821 | 10.71x | 0.077 | 0.821 | 10.71x |
| anderson_ksamp_n1000 | 0.171 | 0.397 | 2.33x | 0.171 | 0.397 | 2.33x |
| spearman_matrix_n1000 | 0.099 | 0.924 | 9.29x | 0.099 | 0.924 | 9.29x |
| pointbiserial_n1000 | 0.011 | 0.578 | 53.79x | 0.011 | 0.578 | 53.79x |
| welch_t_n10000 | 0.054 | 0.878 | 16.25x | 0.054 | 0.878 | 16.25x |
| mann_whitney_n10000 | 0.764 | 2.832 | 3.71x | 0.764 | 2.832 | 3.71x |
| chi_square_n10000 | 0.001 | 0.344 | 246.83x | 0.001 | 0.344 | 246.85x |
| fligner_n10000 | 1.433 | 3.237 | 2.26x | 1.501 | 3.237 | 2.16x |
| anderson_ksamp_n10000 | 2.004 | 3.993 | 1.99x | 2.115 | 3.992 | 1.89x |
| spearman_matrix_n10000 | 2.130 | 4.808 | 2.26x | 2.194 | 4.808 | 2.19x |
| pointbiserial_n10000 | 0.103 | 0.641 | 6.24x | 0.103 | 0.641 | 6.25x |
| welch_t_n100000 | 0.541 | 1.244 | 2.30x | 0.541 | 1.244 | 2.30x |
| mann_whitney_n100000 | 7.763 | 29.092 | 3.75x | 7.762 | 29.088 | 3.75x |
| chi_square_n100000 | 0.014 | 0.363 | 25.29x | 0.014 | 0.362 | 25.29x |
| fligner_n100000 | 14.536 | 33.175 | 2.28x | 14.855 | 33.172 | 2.23x |
| anderson_ksamp_n100000 | 15.402 | 44.162 | 2.87x | 15.795 | 44.152 | 2.80x |
| spearman_matrix_n100000 | 17.847 | 54.208 | 3.04x | 18.464 | 54.202 | 2.94x |
| pointbiserial_n100000 | 1.325 | 1.403 | 1.06x | 1.339 | 5.597 | 4.18x |

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
| fit_1000 | 8.523 | 87.039 | 10.21x | 8.521 | 347.124 | 40.74x |
| fit_penalised_weighted_1000 | 7.992 | 94.960 | 11.88x | 7.991 | 379.073 | 47.43x |
| predict_survival_1000 | 0.845 | 2.393 | 2.83x | 0.853 | 2.393 | 2.81x |
| fit_10000 | 238.993 | 626.470 | 2.62x | 239.231 | 1005.662 | 4.20x |
| fit_penalised_weighted_10000 | 218.486 | 653.442 | 2.99x | 218.787 | 1031.855 | 4.72x |
| predict_survival_10000 | 3.336 | 5.182 | 1.55x | 3.541 | 20.673 | 5.84x |

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
| logrank_wilcoxon_weighted_1000 | 0.027 | 14.212 | 533.83x | 0.027 | 14.209 | 533.79x |
| logrank_fleming_harrington_1000 | 0.036 | 21.570 | 607.39x | 0.036 | 21.566 | 607.16x |
| multigroup_1000 | 0.117 | 29.260 | 249.85x | 0.117 | 29.256 | 249.83x |
| pairwise_1000 | 0.309 | 150.719 | 487.15x | 0.309 | 150.707 | 487.13x |
| rmst_1000 | 0.094 | 1325.158 | 14163.70x | 0.094 | 1325.003 | 14163.05x |
| fixed_point_1000 | 0.037 | 15.559 | 417.14x | 0.037 | 15.557 | 417.15x |
| concordance_1000 | 0.051 | 8.400 | 164.44x | 0.051 | 8.399 | 164.44x |
| logrank_wilcoxon_weighted_10000 | 0.361 | 16.185 | 44.82x | 0.361 | 16.184 | 44.83x |
| logrank_fleming_harrington_10000 | 0.440 | 25.449 | 57.77x | 0.440 | 25.446 | 57.77x |
| multigroup_10000 | 2.458 | 40.310 | 16.40x | 2.467 | 40.308 | 16.34x |
| pairwise_10000 | 4.868 | 166.556 | 34.21x | 4.869 | 166.542 | 34.21x |
| rmst_10000 | 1.860 | 1405.045 | 755.50x | 1.893 | 1404.935 | 742.06x |
| fixed_point_10000 | 0.425 | 17.708 | 41.69x | 0.425 | 17.707 | 41.69x |
| concordance_10000 | 2.359 | 98.863 | 41.91x | 2.359 | 98.861 | 41.91x |
| logrank_wilcoxon_weighted_100000 | 4.418 | 23.919 | 5.41x | 4.540 | 23.917 | 5.27x |
| logrank_fleming_harrington_100000 | 4.321 | 37.977 | 8.79x | 4.393 | 37.976 | 8.64x |
| multigroup_100000 | 11.023 | 52.813 | 4.79x | 11.168 | 52.810 | 4.73x |
| pairwise_100000 | 45.980 | 247.509 | 5.38x | 47.385 | 247.482 | 5.22x |
| rmst_100000 | 9.360 | 519.167 | 55.47x | 9.487 | 519.128 | 54.72x |
| fixed_point_100000 | 4.808 | 30.517 | 6.35x | 4.963 | 30.514 | 6.15x |
| concordance_100000 | 30.144 | 810.139 | 26.88x | 30.377 | 810.118 | 26.67x |

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
| weibull_1000 | 3.440 | 75.371 | 21.91x | 3.440 | 75.367 | 21.91x |
| loglogistic_left_1000 | 6.231 | 104.686 | 16.80x | 6.244 | 104.678 | 16.76x |
| lognormal_interval_1000 | 5.751 | 193.488 | 33.64x | 5.751 | 193.486 | 33.64x |
| generalized_gamma_1000 | 45.952 | 857.254 | 18.66x | 45.958 | 857.212 | 18.65x |
| weibull_aft_1000 | 7.152 | 135.979 | 19.01x | 7.153 | 135.974 | 19.01x |
| lognormal_aft_ridge_robust_1000 | 11.011 | 4810.137 | 436.87x | 11.013 | 4809.778 | 436.75x |
| loglogistic_aft_interval_1000 | 10.508 | 212.781 | 20.25x | 10.509 | 212.776 | 20.25x |
| breslow_fleming_harrington_1000 | 0.254 | 6.936 | 27.27x | 0.254 | 6.935 | 27.27x |
| kaplan_meier_left_1000 | 0.090 | 8.178 | 90.67x | 0.090 | 8.177 | 90.67x |
| weibull_10000 | 33.119 | 89.714 | 2.71x | 33.116 | 89.711 | 2.71x |
| loglogistic_left_10000 | 68.276 | 186.729 | 2.73x | 68.277 | 186.725 | 2.73x |
| lognormal_interval_10000 | 57.634 | 347.342 | 6.03x | 57.631 | 347.291 | 6.03x |
| generalized_gamma_10000 | 427.244 | 2092.049 | 4.90x | 427.185 | 2091.823 | 4.90x |
| weibull_aft_10000 | 74.867 | 161.999 | 2.16x | 74.867 | 161.965 | 2.16x |
| lognormal_aft_ridge_robust_10000 | 154.273 | 46686.595 | 302.62x | 154.242 | 46680.378 | 302.64x |
| loglogistic_aft_interval_10000 | 108.426 | 298.202 | 2.75x | 108.432 | 298.146 | 2.75x |
| breslow_fleming_harrington_10000 | 2.578 | 9.608 | 3.73x | 2.578 | 9.606 | 3.73x |
| kaplan_meier_left_10000 | 0.965 | 10.549 | 10.94x | 0.964 | 10.547 | 10.94x |

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
| var_n1000 | 0.041 | 2.683 | 66.20x | 0.041 | 2.683 | 66.19x |
| var_n10000 | 0.471 | 16.755 | 35.55x | 0.474 | 16.754 | 35.33x |
| var_n100000 | 4.208 | 195.728 | 46.51x | 4.423 | 420.110 | 94.98x |

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
- `VectorizerBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 9.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 0.99 | 1.39 | -29% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.96 | 1.38 | -28% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.06 | 1.43 | -26% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 9.28 | 11.855 | -22% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 3.31 | 5.43 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 3.15 | 4.11 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 2.35 | 3.15 | -21% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.99 | 1.25 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 2.19 | 2.89 | -21% | 20% |

**Still away from their median: 30.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | step | 40.87 | 7.64 | +435% | 50% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | step | 23.77 | 4.53 | +425% | 82% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | step | 14.19 | 3.92 | +262% | 71% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | step | 24.99 | 7.01 | +256% | 86% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | step | 15.01 | 6.98 | +115% | 82% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | step | 14.94 | 7.04 | +112% | 34% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.43 | 2.88 | +54% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 3.29 | 2.16 | +52% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.86 | 1.3 | +43% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_OneWayAnova` | step | 4.416 | 7.24 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_StudentT` | step | 2.939 | 4.818 | -39% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.31 | 0.18 | +39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KolmogorovSmirnov` | step | 23.953 | 39.164 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_MannWhitney` | step | 34.205 | 55.807 | -39% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KolmogorovSmirnov` | step | 35.334 | 57.615 | -39% | 30% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | step | 1.26 | 2.04 | -38% | 30% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | step | 13.84 | 10.02 | +38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | step | 1.388 | 2.243 | -38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KruskalWallis` | step | 59.753 | 96.354 | -38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | step | 0.002 | 0.003 | -33% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | drift | 0.026 | 0.0385 | -32% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.3 | 0.19 | +32% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | step | 0.011 | 0.016 | -31% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.239 | 4.658 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KruskalWallis` | drift | 24.45 | 33.582 | -27% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 7.2 | 5.77 | +25% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | drift | 0.008 | 0.0105 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_MannWhitney` | drift | 11.508 | 14.8845 | -23% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.3 | 1.64 | -21% | 20% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.04 | 0.05 | -20% | 20% |
