# Nightly benchmark run

<!-- nightly-baseline: c4a19d86a5f8c9899f169f26ac7e107de05eedd1 -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks PartitionValidityBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks VectorAutoregressionBenchmarks WeightedLeastSquaresBenchmarks -->
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
- `VectorizerBenchmarks`
- `TokenPatternBenchmarks`
- `SimilaritySketchBenchmarks`

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error             | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|------------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **923.0 ns** |          **14.14 ns** |         **0.78 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         725.9 ns |           0.67 ns |         0.04 ns |     0.79 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,767.2 ns |         116.19 ns |         6.37 ns |     3.00 |    0.01 |   0.0458 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,698.4 ns |          90.45 ns |         4.96 ns |     4.01 |    0.01 |   0.0229 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       4,934.8 ns |          80.88 ns |         4.43 ns |     5.35 |    0.01 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      11,270.7 ns |         352.36 ns |        19.31 ns |    12.21 |    0.02 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,031.2 ns |         348.90 ns |        19.12 ns |     5.45 |    0.02 |        - |        - |        - |         - |          NA |
|                                |           |                  |                   |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **83,448.3 ns** |       **3,155.14 ns** |       **172.94 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      51,902.7 ns |       1,361.42 ns |        74.62 ns |     0.62 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,061,895.6 ns |      56,656.08 ns |     3,105.51 ns |    12.73 |    0.04 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   2,028,830.4 ns |     200,856.22 ns |    11,009.61 ns |    24.31 |    0.12 |        - |        - |        - |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,539,882.1 ns |      40,141.92 ns |     2,200.31 ns |    30.44 |    0.06 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 313,260,290.2 ns | 155,906,867.21 ns | 8,545,781.51 ns | 3,753.96 |   88.94 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,562,061.0 ns |      40,722.27 ns |     2,232.13 ns |    30.70 |    0.06 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **66.44 μs** |  **25.448 μs** |  **1.395 μs** |  **1.00** |    **0.03** | **0.1221** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    67.70 μs |   6.118 μs |  0.335 μs |  1.02 |    0.02 | 0.1221 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,812.04 μs** |  **12.981 μs** |  **0.712 μs** |  **1.00** |    **0.00** |      **-** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,505.42 μs | 215.293 μs | 11.801 μs |  0.83 |    0.01 |      - |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **886.9 ns** |      **14.77 ns** |      **0.81 ns** |   **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     2,039.8 ns |      34.24 ns |      1.88 ns |   2.30 |    0.00 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,328.3 ns |      67.64 ns |      3.71 ns |   2.63 |    0.00 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,768.7 ns |     133.41 ns |      7.31 ns |   4.25 |    0.01 | 0.0076 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     7,059.2 ns |     476.04 ns |     26.09 ns |   7.96 |    0.03 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     8,515.2 ns |     293.59 ns |     16.09 ns |   9.60 |    0.02 | 0.0153 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,703.2 ns |      91.41 ns |      5.01 ns |   3.05 |    0.01 | 0.0191 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,824.2 ns |   1,127.32 ns |     61.79 ns |   4.31 |    0.06 | 0.0114 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       704.9 ns |      10.52 ns |      0.58 ns |   0.79 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,803.0 ns |     109.04 ns |      5.98 ns |   3.16 |    0.01 | 0.0076 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |     1,173.1 ns |      50.16 ns |      2.75 ns |   1.32 |    0.00 | 0.0095 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,196.2 ns |     151.76 ns |      8.32 ns |   1.35 |    0.01 | 0.0114 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       308.1 ns |       4.09 ns |      0.22 ns |   0.35 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,317.0 ns |      42.48 ns |      2.33 ns |   1.49 |    0.00 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |       101.8 ns |       2.60 ns |      0.14 ns |   0.11 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       650.3 ns |      46.59 ns |      2.55 ns |   0.73 |    0.00 | 0.0114 |     968 B |          NA |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **54,448.2 ns** |   **9,301.90 ns** |    **509.87 ns** |  **1.000** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   168,098.6 ns |   4,514.51 ns |    247.46 ns |  3.087 |    0.03 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   518,796.6 ns |  36,925.16 ns |  2,023.99 ns |  9.529 |    0.08 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 2,149,212.4 ns |  97,795.67 ns |  5,360.51 ns | 39.475 |    0.33 |      - |   80382 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,202,860.1 ns |  70,196.13 ns |  3,847.69 ns | 22.093 |    0.19 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,710,163.6 ns |  21,064.84 ns |  1,154.63 ns | 68.145 |    0.55 |      - |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,513,300.3 ns | 428,468.68 ns | 23,485.81 ns | 27.795 |    0.44 |      - |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,192,115.0 ns | 237,016.75 ns | 12,991.69 ns | 40.263 |    0.39 |      - |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    60,881.2 ns |   2,146.76 ns |    117.67 ns |  1.118 |    0.01 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   252,572.0 ns |  17,524.18 ns |    960.56 ns |  4.639 |    0.04 |      - |     745 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   255,983.9 ns |  30,967.13 ns |  1,697.41 ns |  4.702 |    0.05 | 0.4883 |   80057 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   452,461.1 ns | 114,788.36 ns |  6,291.94 ns |  8.310 |    0.12 | 0.4883 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       307.8 ns |       8.27 ns |      0.45 ns |  0.006 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,309.1 ns |      19.20 ns |      1.05 ns |  0.024 |    0.00 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |       102.0 ns |       1.23 ns |      0.07 ns |  0.002 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       651.4 ns |      87.90 ns |      4.82 ns |  0.012 |    0.00 | 0.0114 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,141.2 ns** |     **358.75 ns** |    **19.66 ns** |  **1.00** |    **0.01** | **0.0572** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,710.4 ns |     137.35 ns |     7.53 ns |  1.73 |    0.01 | 0.0648 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       397.6 ns |      18.99 ns |     1.04 ns |  0.19 |    0.00 | 0.0010 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |       814.8 ns |     104.36 ns |     5.72 ns |  0.38 |    0.00 | 0.0019 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       695.2 ns |      24.86 ns |     1.36 ns |  0.32 |    0.00 |      - |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     3,770.2 ns |     258.36 ns |    14.16 ns |  1.76 |    0.02 | 0.0229 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,061.4 ns |     373.54 ns |    20.48 ns |  3.77 |    0.03 |      - |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     5,516.4 ns |     354.71 ns |    19.44 ns |  2.58 |    0.02 | 0.0076 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,274.1 ns |     358.37 ns |    19.64 ns |  2.00 |    0.02 |      - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |        |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **381,093.8 ns** |     **883.70 ns** |    **48.44 ns** | **1.000** |    **0.00** | **5.3711** | **1.4648** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,294,332.8 ns |  39,602.44 ns | 2,170.74 ns | 3.396 |    0.00 | 3.9063 |      - |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    44,358.9 ns | 123,001.32 ns | 6,742.12 ns | 0.116 |    0.02 |      - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |    80,844.9 ns |     333.35 ns |    18.27 ns | 0.212 |    0.00 |      - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       904.0 ns |      44.21 ns |     2.42 ns | 0.002 |    0.00 |      - |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   767,636.2 ns |  61,209.35 ns | 3,355.09 ns | 2.014 |    0.01 | 1.9531 |      - |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,333,073.0 ns |  68,696.49 ns | 3,765.49 ns | 3.498 |    0.01 |      - |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |     NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,930.7 ns |     461.96 ns |    25.32 ns | 0.029 |    0.00 |      - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,869.4 μs** |    **176.66 μs** |     **9.68 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    82,762.4 μs |  2,134.78 μs |   117.01 μs |  44.27 |    0.21 |   666.6667 |  500.0000 |         - |  59878.93 KB |       58.56 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,693.7 μs** |     **65.88 μs** |     **3.61 μs** |   **1.00** |    **0.00** |   **248.0469** |  **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    94,011.3 μs | 12,105.76 μs |   663.56 μs |  55.51 |    0.35 |  1000.0000 |  800.0000 |         - |  83110.15 KB |       81.29 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **818.3 μs** |    **360.96 μs** |    **19.79 μs** |   **1.00** |    **0.03** |          **-** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   123,984.9 μs | 18,800.58 μs | 1,030.52 μs | 151.57 |    3.31 |   800.0000 |  200.0000 |         - |  81288.37 KB |    2,678.83 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,870.9 μs** |    **285.91 μs** |    **15.67 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    73,132.3 μs |  5,740.98 μs |   314.68 μs |  39.09 |    0.32 |   714.2857 |  428.5714 |         - |  64189.81 KB |       62.78 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **16,325.8 μs** |  **4,467.79 μs** |   **244.89 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,763,611.2 μs | 81,627.68 μs | 4,474.29 μs | 108.04 |    1.43 |  6000.0000 | 3000.0000 | 1000.0000 | 537131.64 KB |       60.18 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **13,429.2 μs** |  **2,465.22 μs** |   **135.13 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,753,134.9 μs | 66,794.69 μs | 3,661.24 μs | 130.56 |    1.16 | 10000.0000 | 5000.0000 | 2000.0000 | 747334.45 KB |       83.73 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,164.6 μs** |    **101.31 μs** |     **5.55 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,017,739.8 μs | 67,612.70 μs | 3,706.08 μs | 489.52 |    0.65 |  9000.0000 | 5000.0000 | 2000.0000 | 732661.26 KB |    8,147.75 |
|                        |      |          |                |              |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **15,848.5 μs** |    **857.43 μs** |    **47.00 μs** |   **1.00** |    **0.00** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,566,927.3 μs | 69,294.46 μs | 3,798.26 μs |  98.87 |    0.33 |  8000.0000 | 5000.0000 | 2000.0000 | 573606.88 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|----------------- |---------:|-----------:|---------:|---------:|---------:|---------:|----------:|
| CountLoadAsync2M | 19.21 ms |   8.727 ms | 0.478 ms | 468.7500 | 437.5000 | 406.2500 |  21.53 MB |
| CountLoadAsync4M | 46.27 ms |   3.555 ms | 0.195 ms | 500.0000 | 416.6667 | 333.3333 |  43.93 MB |
| CountLoad4M      | 44.51 ms | 111.773 ms | 6.127 ms | 500.0000 | 416.6667 | 333.3333 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error          | StdDev       | Gen0     | Gen1     | Gen2     | Allocated   |
|---------------------- |--------------:|---------------:|-------------:|---------:|---------:|---------:|------------:|
| CountFitLongDocuments | 182,294.75 μs |   6,243.388 μs |   342.221 μs |        - |        - |        - | 54334.51 KB |
| CountSave240K         |     406.96 μs |      11.659 μs |     0.639 μs |   3.9063 |   2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     484.12 μs |      39.869 μs |     2.185 μs |   4.8828 |   3.9063 |   3.9063 |   739.44 KB |
| TfidfSave45K          |      52.84 μs |       1.345 μs |     0.074 μs |   1.6479 |   0.3052 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      58.57 μs |       2.671 μs |     0.146 μs |   2.1973 |   0.4883 |        - |   181.88 KB |
| IndexSave942K         |     245.91 μs |      77.357 μs |     4.240 μs |   1.4648 |   0.4883 |   0.4883 |    243.5 KB |
| IndexSaveAsync942K    |     266.31 μs |      19.574 μs |     1.073 μs |   1.4648 |   0.4883 |   0.4883 |   243.62 KB |
| CountLoadAsync2M      |  25,757.25 μs |   4,303.137 μs |   235.869 μs | 156.2500 | 125.0000 |  93.7500 | 22048.86 KB |
| CountLoadAsync4M      |  51,586.18 μs | 104,959.569 μs | 5,753.188 μs | 222.2222 | 111.1111 | 111.1111 | 44983.96 KB |
| CountLoad4M           |  47,134.51 μs |   2,526.403 μs |   138.481 μs | 333.3333 | 250.0000 | 166.6667 | 44976.63 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     15.395 μs |    26.255 μs |   1.4391 μs |   0.3357 |        - |        - |    27.43 KB |
| CountSaveAsyncWide     |    551.782 μs |   227.301 μs |  12.4592 μs |  28.3203 |  27.3438 |  27.3438 |   740.17 KB |
| HashingLoadUndeclared  |      9.068 μs |     9.963 μs |   0.5461 μs |   1.0223 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     31.218 μs |     1.827 μs |   0.1001 μs |   1.4648 |   0.2441 |        - |   121.83 KB |
| CountLoadUndeclared24K |    121.715 μs |     8.806 μs |   0.4827 μs |   3.1738 |        - |        - |   264.67 KB |
| CountLoadUndeclared78K |    678.814 μs |   284.141 μs |  15.5747 μs |  12.6953 |  11.7188 |   6.8359 |   656.32 KB |
| CountLoadUndeclared1M  |  8,942.420 μs | 4,460.829 μs | 244.5131 μs | 343.7500 | 328.1250 | 312.5000 | 10793.91 KB |
| CountSaveAsync1M       |  2,649.689 μs | 3,229.204 μs | 177.0036 μs |  85.9375 |  85.9375 |  85.9375 |  3162.11 KB |
| IndexSaveAsyncLongHead |  3,372.427 μs |   919.765 μs |  50.4154 μs |  66.4063 |  66.4063 |  66.4063 |  2124.38 KB |
| CountLoadUndeclared2M  | 17,137.648 μs | 5,834.034 μs | 319.7831 μs | 625.0000 | 593.7500 | 562.5000 | 22045.51 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|-------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.530 μs** |  **0.8225 μs** | **0.0451 μs** |  **1.00** | **0.0229** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.913 μs |  0.3229 μs | 0.0177 μs |  1.07 | 0.0305 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   5.917 μs |  0.4189 μs | 0.0230 μs |  1.07 | 0.0305 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **8**          |  **55.134 μs** |  **3.3887 μs** | **0.1857 μs** |  **1.00** | **0.3052** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  19.207 μs |  3.1685 μs | 0.1737 μs |  0.35 | 0.2441 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  19.438 μs |  1.3716 μs | 0.0752 μs |  0.35 | 0.2441 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **32**         | **218.646 μs** | **10.3038 μs** | **0.5648 μs** |  **1.00** | **1.2207** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  75.105 μs |  4.1664 μs | 0.2284 μs |  0.34 | 0.9766 |      - |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  65.254 μs | 14.6525 μs | 0.8032 μs |  0.30 | 0.7324 |      - |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **128**        | **881.204 μs** | **76.8487 μs** | **4.2123 μs** |  **1.00** | **4.8828** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 287.193 μs | 14.8462 μs | 0.8138 μs |  0.33 | 3.9063 | 0.4883 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 257.310 μs | 17.7422 μs | 0.9725 μs |  0.29 | 2.9297 |      - | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **139.79 ms** | **18.384 ms** | **1.008 ms** |  **1.00** |    **0.01** |   **27.18 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  78.50 ms |  5.155 ms | 0.283 ms |  0.56 |    0.00 |  103.71 KB |        3.82 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **142.81 ms** | **65.774 ms** | **3.605 ms** |  **1.00** |    **0.03** |   **23.79 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  74.94 ms |  2.498 ms | 0.137 ms |  0.52 |    0.01 |  116.48 KB |        4.90 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **209.84 ms** | **28.798 ms** | **1.579 ms** |  **1.00** |    **0.01** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 336.41 ms | 78.548 ms | 4.305 ms |  1.60 |    0.02 |  259.85 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **210.84 ms** | **22.839 ms** | **1.252 ms** |  **1.00** |    **0.01** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 293.02 ms | 27.821 ms | 1.525 ms |  1.39 |    0.01 |  192.92 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **258.16 ms** | **11.169 ms** | **0.612 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 459.13 ms | 45.588 ms | 2.499 ms |  1.78 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **263.00 ms** | **15.898 ms** | **0.871 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 416.47 ms | 31.217 ms | 1.711 ms |  1.58 |    0.01 |  1153.8 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **298.96 ms** | **16.241 ms** | **0.890 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 532.90 ms | 12.815 ms | 0.702 ms |  1.78 |    0.01 | 7215.92 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **304.20 ms** | **25.235 ms** | **1.383 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 514.26 ms |  6.180 ms | 0.339 ms |  1.69 |    0.01 | 7962.86 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **43.43 μs** |      **9.387 μs** |     **0.515 μs** |         **-** |
| Cjk    | 1000   |      57.60 μs |      2.389 μs |     0.131 μs |         - |
| **Latin**  | **10000**  |   **4,129.41 μs** |    **155.738 μs** |     **8.537 μs** |         **-** |
| Cjk    | 10000  |   6,624.26 μs |    738.314 μs |    40.469 μs |         - |
| **Latin**  | **65536**  | **186,747.22 μs** | **40,029.082 μs** | **2,194.129 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error      | StdDev   | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|-------- |---------:|-----------:|---------:|------:|--------:|---------:|----------:|------------:|
| Unigram | 26.17 ms |   1.066 ms | 0.058 ms |  1.00 |    0.00 |  93.7500 |   8.19 MB |        1.00 |
| Bpe     | 89.19 ms | 125.556 ms | 6.882 ms |  3.41 |    0.23 | 333.3333 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **18.02 μs** |  **0.757 μs** | **0.041 μs** | **0.0916** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **77.37 μs** |  **6.383 μs** | **0.350 μs** | **0.1221** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **196.84 μs** |  **5.962 μs** | **0.327 μs** | **0.2441** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **413.02 μs** | **24.046 μs** | **1.318 μs** | **0.4883** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error     | StdDev   | Median   | Allocated |
|------------------ |---------:|----------:|---------:|---------:|----------:|
| EncodeUnseenProse | 9.141 ms | 63.147 ms | 3.461 ms | 7.189 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error      | StdDev    | Allocated |
|----------- |--------- |-----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **15.301 μs** |  **1.2532 μs** | **0.0687 μs** |         **-** |
| MyersGroup | cjk      | 251.275 μs | 10.0943 μs | 0.5533 μs |         - |
| **DpGroup**    | **latin**    |   **8.466 μs** |  **0.3996 μs** | **0.0219 μs** |         **-** |
| MyersGroup | latin    | 136.042 μs |  9.8725 μs | 0.5411 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **289.95 μs** |  **4.026 μs** | **0.221 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 310.05 μs | 64.362 μs | 3.528 μs |  1.07 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **340.99 μs** | **11.882 μs** | **0.651 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 336.24 μs | 16.230 μs | 0.890 μs |  0.99 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **306.69 μs** |  **8.136 μs** | **0.446 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 317.61 μs |  9.061 μs | 0.497 μs |  1.04 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **273.99 μs** |  **7.781 μs** | **0.427 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 340.65 μs | 14.899 μs | 0.817 μs |  1.24 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **300.25 μs** |  **7.342 μs** | **0.402 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 312.36 μs |  5.357 μs | 0.294 μs |  1.04 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **161.46 μs** | **15.909 μs** | **0.872 μs** |  **1.00** |    **0.01** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 343.55 μs |  5.727 μs | 0.314 μs |  2.13 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **213.32 μs** |  **2.929 μs** | **0.161 μs** |  **1.00** |    **0.00** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 306.88 μs | 59.936 μs | 3.285 μs |  1.44 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **77.35 μs** |  **7.274 μs** | **0.399 μs** |  **1.00** |    **0.01** | **0.3662** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 337.50 μs |  8.778 μs | 0.481 μs |  4.36 |    0.02 |      - |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0    | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **173.2 μs** |   **6.40 μs** |  **0.35 μs** |  **1.00** |       **-** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   166.9 μs |   7.92 μs |  0.43 μs |  0.96 |       - |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   420.0 μs |  10.88 μs |  0.60 μs |  2.43 |  3.4180 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,287.9 μs** | **175.10 μs** |  **9.60 μs** |  **1.00** | **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,320.7 μs | 780.81 μs | 42.80 μs |  1.01 | 97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 6,917.8 μs | 145.62 μs |  7.98 μs |  2.10 | 54.6875 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **134.62 ms** |  **15.954 ms** | **0.875 ms** |  **1.00** |    **0.01** | **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,795.87 ms | 147.075 ms | 8.062 ms | 13.34 |    0.09 | 8000.0000 | 7000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |            |          |       |         |           |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **46.14 ms** |   **3.172 ms** | **0.174 ms** |  **1.00** |    **0.00** | **1454.5455** | **1454.5455** | **1454.5455** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   647.43 ms |  78.644 ms | 4.311 ms | 14.03 |    0.09 | 3000.0000 | 3000.0000 | 3000.0000 |  59.79 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **183.74 ms** |  **29.345 ms** |  **1.608 ms** |  **1.00** |    **0.01** |  **1333.3333** |  **1333.3333** | **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 5,105.17 ms | 405.585 ms | 22.231 ms | 27.79 |    0.23 | 11000.0000 |  7000.0000 | 6000.0000 | 531.84 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,129.42 ms |  49.469 ms |  2.712 ms | 17.03 |    0.13 | 11000.0000 | 10000.0000 | 9000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |           |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **19.86 ms** |   **0.729 ms** |  **0.040 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** | **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   375.53 ms |  10.263 ms |  0.563 ms | 18.91 |    0.04 |  2000.0000 |  2000.0000 | 2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   223.97 ms |   3.627 ms |  0.199 ms | 11.28 |    0.02 |  3666.6667 |  3666.6667 | 3666.6667 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  13.478 ms |  1.8535 ms | 0.1016 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 102.938 ms | 35.5464 ms | 1.9484 ms |  7.64 |    0.13 |
| Nmf_Transform                             |   3.859 ms |  0.0903 ms | 0.0050 ms |  0.29 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  18.913 ms |  1.5045 ms | 0.0825 ms |  1.40 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    92.25 ns |  4.114 ns | 0.225 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   698.22 ns | 78.560 ns | 4.306 ns |  7.57 |    0.04 |      - |         - |          NA |
| TokenSortRatio |   871.13 ns | 25.995 ns | 1.425 ns |  9.44 |    0.02 | 0.0134 |    1120 B |          NA |
| TokenSetRatio  |   864.21 ns | 32.747 ns | 1.795 ns |  9.37 |    0.03 | 0.0105 |     896 B |          NA |
| WRatio         | 1,118.92 ns | 22.582 ns | 1.238 ns | 12.13 |    0.03 | 0.0134 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error             | StdDev         | Ratio  | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|---------------:|-------:|--------:|-------:|-------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,914,061.3659 ns |   203,342.2990 ns | 11,145.8776 ns |  1.000 |    0.01 |      - |      - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    680,815.5378 ns |    17,931.0928 ns |    982.8637 ns |  0.356 |    0.00 | 6.8359 | 1.9531 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,160,445.5345 ns |    51,922.4986 ns |  2,846.0474 ns |  0.606 |    0.00 | 7.8125 |      - |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 88,949,461.8889 ns | 1,468,401.3388 ns | 80,488.0326 ns | 46.473 |    0.24 |      - |      - |  510843 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |      0.0000 ns |  0.000 |    0.00 |      - |      - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **90.73 ns** |   **2.633 ns** |  **0.144 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   199.32 ns |  19.739 ns |  1.082 ns |  2.20 |    0.01 | 0.0010 |      80 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **696.21 ns** |  **12.928 ns** |  **0.709 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 9,048.59 ns | 826.215 ns | 45.288 ns | 13.00 |    0.06 |      - |     160 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **887.75 ns** |  **16.223 ns** |  **0.889 ns** |  **1.00** |    **0.00** | **0.0105** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,856.39 ns | 220.708 ns | 12.098 ns |  2.09 |    0.01 | 0.0229 |    1944 B |        2.17 |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        | **1,152.67 ns** |  **54.340 ns** |  **2.979 ns** |  **1.00** |    **0.00** | **0.0134** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 4,430.20 ns | 122.607 ns |  6.721 ns |  3.84 |    0.01 | 0.0305 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **21.98 ns** |   **1.397 ns** |  **0.077 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    32.54 ns |   1.488 ns |  0.082 ns |  1.48 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    24.99 ns |   1.138 ns |  0.062 ns |  1.14 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    22.85 ns |   0.558 ns |  0.031 ns |  1.04 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **26.56 ns** |   **4.268 ns** |  **0.234 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    34.04 ns |   0.727 ns |  0.040 ns |  1.28 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    27.85 ns |   5.106 ns |  0.280 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    26.85 ns |   0.485 ns |  0.027 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **28.81 ns** |   **1.764 ns** |  **0.097 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    37.95 ns |   2.197 ns |  0.120 ns |  1.32 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    30.59 ns |   1.643 ns |  0.090 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    25.87 ns |   0.680 ns |  0.037 ns |  0.90 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **32.34 ns** |   **0.281 ns** |  **0.015 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    39.09 ns |   4.739 ns |  0.260 ns |  1.21 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    34.82 ns |   0.349 ns |  0.019 ns |  1.08 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    31.05 ns |   1.778 ns |  0.097 ns |  0.96 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **49.81 ns** |   **4.693 ns** |  **0.257 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    54.82 ns |   2.877 ns |  0.158 ns |  1.10 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    52.86 ns |   7.371 ns |  0.404 ns |  1.06 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    48.58 ns |   4.094 ns |  0.224 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **61.29 ns** |   **3.934 ns** |  **0.216 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    62.14 ns |   2.632 ns |  0.144 ns |  1.01 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    61.61 ns |   1.954 ns |  0.107 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    56.08 ns |   3.490 ns |  0.191 ns |  0.92 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **346.97 ns** |   **3.329 ns** |  **0.183 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   356.94 ns | 117.085 ns |  6.418 ns |  1.03 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   345.21 ns |  23.586 ns |  1.293 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   341.00 ns |  11.289 ns |  0.619 ns |  0.98 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,671.96 ns** | **665.371 ns** | **36.471 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,645.01 ns |  39.090 ns |  2.143 ns |  0.99 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,616.42 ns |  86.279 ns |  4.729 ns |  0.99 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,543.56 ns | 166.033 ns |  9.101 ns |  0.97 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **369.1 ns** |    **13.58 ns** |   **0.74 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    173.5 ns |    83.24 ns |   4.56 ns |  0.47 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,326.7 ns** |   **118.80 ns** |   **6.51 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,582.2 ns |   147.17 ns |   8.07 ns |  1.11 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,661.2 ns** |   **234.12 ns** |  **12.83 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 33,644.5 ns | 5,158.74 ns | 282.77 ns |  2.46 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,624.4 ns** |   **205.92 ns** |  **11.29 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,547.9 ns |   974.36 ns |  53.41 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 10,868.1 ns |   259.75 ns |  14.24 ns |  1.43 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,016.3 ns** |   **232.38 ns** |  **12.74 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  8,917.1 ns |   494.49 ns |  27.10 ns |  0.89 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 13,742.8 ns | 1,076.17 ns |  58.99 ns |  1.37 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **14,953.3 ns** |   **543.94 ns** |  **29.82 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,470.0 ns | 2,145.18 ns | 117.58 ns |  0.77 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 17,699.0 ns |   286.63 ns |  15.71 ns |  1.18 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,237.5 ns** |   **201.76 ns** |  **11.06 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,970.5 ns |   797.29 ns |  43.70 ns |  1.14 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  8,641.2 ns |   455.79 ns |  24.98 ns |  1.65 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,926.5 ns** |   **260.26 ns** |  **14.27 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,627.1 ns |   476.82 ns |  26.14 ns |  0.96 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,201.5 ns |   293.24 ns |  16.07 ns |  1.41 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **13,090.1 ns** |   **100.98 ns** |   **5.53 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,437.2 ns |   632.21 ns |  34.65 ns |  0.72 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,782.2 ns | 6,804.52 ns | 372.98 ns |  1.21 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,708.7 ns** |   **440.37 ns** |  **24.14 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  3,859.0 ns |    44.18 ns |   2.42 ns |  1.04 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,113.1 ns |   638.08 ns |  34.98 ns |  1.92 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **6,678.0 ns** |   **115.68 ns** |   **6.34 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,798.3 ns |   401.39 ns |  22.00 ns |  0.87 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,821.3 ns |   996.23 ns |  54.61 ns |  1.47 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **11,694.5 ns** | **4,120.63 ns** | **225.87 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,528.2 ns |   624.64 ns |  34.24 ns |  0.64 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,565.4 ns |   478.95 ns |  26.25 ns |  1.16 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,360.0 ns** |    **28.89 ns** |   **1.58 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  1,930.9 ns |    93.38 ns |   5.12 ns |  0.82 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,685.3 ns |   334.21 ns |  18.32 ns |  2.41 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **5,186.3 ns** |   **160.54 ns** |   **8.80 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,617.9 ns |   160.53 ns |   8.80 ns |  0.50 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,370.4 ns | 1,024.74 ns |  56.17 ns |  1.61 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     | **10,181.5 ns** |   **395.99 ns** |  **21.71 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,612.0 ns |   297.06 ns |  16.28 ns |  0.35 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,203.2 ns |   396.83 ns |  21.75 ns |  1.20 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,876.0 ns** |   **205.45 ns** |  **11.26 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,179.5 ns |   144.74 ns |   7.93 ns |  0.82 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,121.9 ns |   709.42 ns |  38.89 ns |  1.84 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **6,593.3 ns** |    **90.14 ns** |   **4.94 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  3,916.8 ns |   322.90 ns |  17.70 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     |  9,996.6 ns |   607.53 ns |  33.30 ns |  1.52 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **11,489.8 ns** |   **296.32 ns** |  **16.24 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  7,244.1 ns |   435.80 ns |  23.89 ns |  0.63 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 13,649.6 ns |   764.40 ns |  41.90 ns |  1.19 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,329.2 ns** |    **29.17 ns** |   **1.60 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    480.9 ns |    22.57 ns |   1.24 ns |  0.36 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,791.3 ns |    45.00 ns |   2.47 ns |  3.60 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,764.1 ns** |   **321.42 ns** |  **17.62 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,140.2 ns |    23.73 ns |   1.30 ns |  0.30 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,465.5 ns |   732.64 ns |  40.16 ns |  1.98 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **8,140.9 ns** |   **687.29 ns** |  **37.67 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,155.4 ns |   171.91 ns |   9.42 ns |  0.26 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,477.4 ns | 1,460.68 ns |  80.06 ns |  1.41 |    0.01 | 0.0610 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,571.0 μs** |    **201.90 μs** |    **11.07 μs** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  35,230.3 μs | 12,148.86 μs |   665.92 μs |  6.32 |    0.10 |        - |        - |        - |   10.38 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  55,075.5 μs |  4,021.30 μs |   220.42 μs |  9.89 |    0.04 |        - |        - |        - |   41.86 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **829.7 μs** |     **23.63 μs** |     **1.30 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   3,997.4 μs |    335.97 μs |    18.42 μs |  4.82 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2,485.5 μs |    210.55 μs |    11.54 μs |  3.00 |    0.01 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **27,206.2 μs** |  **3,445.15 μs** |   **188.84 μs** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 382,397.8 μs |  1,283.65 μs |    70.36 μs | 14.06 |    0.08 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 466,223.7 μs | 27,582.31 μs | 1,511.88 μs | 17.14 |    0.11 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.785 ms** | **0.2062 ms** | **0.0113 ms** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  14.550 ms | 0.6245 ms | 0.0342 ms |  2.52 |    0.01 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **29.246 ms** | **1.3932 ms** | **0.0764 ms** |  **1.00** |    **0.00** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  91.733 ms | 2.9345 ms | 0.1609 ms |  3.14 |    0.01 |        - |        - |        - |   96.44 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **55.784 ms** | **5.8745 ms** | **0.3220 ms** |  **1.00** |    **0.01** | **222.2222** | **222.2222** | **222.2222** | **3946.98 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 184.398 ms | 4.6121 ms | 0.2528 ms |  3.31 |    0.02 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **121.53 ns** |    **33.806 ns** |   **1.853 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 8    |     54.72 ns |     7.707 ns |   0.422 ns |  0.45 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    123.44 ns |     9.465 ns |   0.519 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |     87.45 ns |     0.791 ns |   0.043 ns |  0.72 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **219.48 ns** |     **4.700 ns** |   **0.258 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     63.48 ns |     2.686 ns |   0.147 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    220.28 ns |     2.587 ns |   0.142 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |     99.73 ns |     7.757 ns |   0.425 ns |  0.45 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **283.10 ns** |    **22.334 ns** |   **1.224 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |     68.03 ns |     3.054 ns |   0.167 ns |  0.24 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    284.37 ns |    20.151 ns |   1.105 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 14   |    107.49 ns |     3.469 ns |   0.190 ns |  0.38 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **342.41 ns** |     **9.076 ns** |   **0.497 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |     72.13 ns |     1.719 ns |   0.094 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    342.13 ns |    19.178 ns |   1.051 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    113.01 ns |     3.649 ns |   0.200 ns |  0.33 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **415.05 ns** |     **5.242 ns** |   **0.287 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.06 ns |     1.453 ns |   0.080 ns |  0.18 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    416.29 ns |     7.509 ns |   0.412 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    117.41 ns |     3.676 ns |   0.202 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **491.57 ns** |    **17.636 ns** |   **0.967 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     76.29 ns |     0.927 ns |   0.051 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    494.74 ns |    44.134 ns |   2.419 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    119.96 ns |     3.202 ns |   0.175 ns |  0.24 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **673.84 ns** |   **196.084 ns** |  **10.748 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |     83.27 ns |     1.917 ns |   0.105 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    668.03 ns |    33.467 ns |   1.834 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    133.15 ns |     5.346 ns |   0.293 ns |  0.20 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,141.39 ns** |    **38.772 ns** |   **2.125 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    100.47 ns |     1.059 ns |   0.058 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,189.38 ns |   113.461 ns |   6.219 ns |  1.04 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    154.34 ns |     0.795 ns |   0.044 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **2,471.28 ns** |   **506.979 ns** |  **27.789 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    120.37 ns |     1.379 ns |   0.076 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,499.04 ns |   134.508 ns |   7.373 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 48   |    212.77 ns |     7.387 ns |   0.405 ns |  0.09 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **4,008.42 ns** |    **78.584 ns** |   **4.307 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    147.02 ns |    17.779 ns |   0.975 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  4,361.04 ns |   197.868 ns |  10.846 ns |  1.09 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |    260.23 ns |    13.517 ns |   0.741 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   |  **9,784.15 ns** | **1,502.809 ns** |  **82.374 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    307.34 ns |     9.438 ns |   0.517 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 10,223.58 ns | 2,004.937 ns | 109.897 ns |  1.04 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    798.20 ns |    33.532 ns |   1.838 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **23.79 ns** |     **1.553 ns** |   **0.085 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.10 ns |     3.553 ns |   0.195 ns |  1.01 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    133.34 ns |     4.865 ns |   0.267 ns |  5.61 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     22.89 ns |     5.620 ns |   0.308 ns |  0.96 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **278.10 ns** |    **10.173 ns** |   **0.558 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    346.39 ns |    10.036 ns |   0.550 ns |  1.25 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    680.03 ns |    54.143 ns |   2.968 ns |  2.45 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    281.55 ns |     6.398 ns |   0.351 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **843.56 ns** |    **54.380 ns** |   **2.981 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,646.98 ns |    17.102 ns |   0.937 ns |  1.95 |    0.01 |         - |          NA |
| Distance_CodePoint         | 128    |  1,525.57 ns |    79.739 ns |   4.371 ns |  1.81 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    839.86 ns |    28.972 ns |   1.588 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,836.81 ns** | **2,617.227 ns** | **143.459 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,625.55 ns | 1,840.991 ns | 100.911 ns |  1.40 |    0.02 |         - |          NA |
| Distance_CodePoint         | 512    | 13,352.58 ns |   118.502 ns |   6.495 ns |  1.13 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,138.82 ns |   104.005 ns |   5.701 ns |  0.94 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **343.0 ns** |   **109.42 ns** |   **6.00 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     213.6 ns |     6.76 ns |   0.37 ns |  0.62 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **344.0 ns** |    **56.74 ns** |   **3.11 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     212.4 ns |     5.09 ns |   0.28 ns |  0.62 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **433.4 ns** |     **5.24 ns** |   **0.29 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     302.5 ns |    20.54 ns |   1.13 ns |  0.70 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **430.2 ns** |    **14.23 ns** |   **0.78 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     302.0 ns |     6.26 ns |   0.34 ns |  0.70 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **522.9 ns** |    **27.41 ns** |   **1.50 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     392.4 ns |    67.87 ns |   3.72 ns |  0.75 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **523.6 ns** |    **45.25 ns** |   **2.48 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     389.9 ns |    19.07 ns |   1.05 ns |  0.74 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **623.7 ns** |    **18.89 ns** |   **1.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,062.7 ns |    25.63 ns |   1.40 ns |  1.70 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **619.8 ns** |    **22.86 ns** |   **1.25 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,125.9 ns |    84.63 ns |   4.64 ns |  1.82 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,850.8 ns** |    **57.40 ns** |   **3.15 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   4,979.3 ns |   245.38 ns |  13.45 ns |  2.69 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,844.3 ns** |    **29.43 ns** |   **1.61 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,788.6 ns |   112.51 ns |   6.17 ns |  2.60 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **15,005.8 ns** | **1,220.93 ns** |  **66.92 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  60,583.7 ns |   394.01 ns |  21.60 ns |  4.04 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **506,299.1 ns** | **8,907.14 ns** | **488.23 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  60,778.3 ns | 7,144.29 ns | 391.60 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **23.94 ns** |      **0.128 ns** |     **0.007 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      85.81 ns |     13.082 ns |     0.717 ns |  3.58 |    0.03 | 0.0006 |      56 B |          NA |
| Quickenshtein        | 8      |      82.73 ns |      4.610 ns |     0.253 ns |  3.46 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     180.63 ns |      9.629 ns |     0.528 ns |  7.55 |    0.02 | 0.0014 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **279.87 ns** |     **11.584 ns** |     **0.635 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,287.85 ns |     68.535 ns |     3.757 ns | 18.89 |    0.04 |      - |     280 B |          NA |
| Quickenshtein        | 64     |   1,329.57 ns |    133.301 ns |     7.307 ns |  4.75 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   8,274.51 ns |    234.415 ns |    12.849 ns | 29.57 |    0.07 |      - |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,147.72 ns** |    **114.502 ns** |     **6.276 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 458,439.25 ns | 53,538.606 ns | 2,934.632 ns | 41.12 |    0.23 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  39,239.65 ns |  1,641.733 ns |    89.989 ns |  3.52 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 773,031.92 ns | 64,230.320 ns | 3,520.681 ns | 69.34 |    0.28 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------------------ |------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **F1Macro**           | **1000**   | **500.4 μs** | **94.64 μs** | **5.19 μs** |      **-** |      **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 486.6 μs | 27.29 μs | 1.50 μs |      - |      - |  67.71 KB |
| ReportRows        | 1000   | 529.6 μs |  8.14 μs | 0.45 μs | 0.9766 |      - | 153.99 KB |
| **F1Macro**           | **5000**   | **656.5 μs** | **63.98 μs** | **3.51 μs** | **3.9063** | **0.9766** | **337.24 KB** |
| PrecisionWeighted | 5000   | 661.9 μs | 69.03 μs | 3.78 μs | 3.9063 | 0.9766 | 337.24 KB |
| ReportRows        | 5000   | 828.6 μs | 58.26 μs | 3.19 μs | 8.7891 | 3.9063 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error          | StdDev        | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|--------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **144.82 μs** |       **5.629 μs** |      **0.309 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 130,630.22 μs |  57,374.287 μs |  3,144.878 μs | 902.04 |   18.88 | 750.0000 | 750.0000 | 750.0000 | 31414.39 KB |   12,805.87 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,976.86 μs** |     **602.909 μs** |     **33.047 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 639,349.25 μs | 200,769.820 μs | 11,004.871 μs | 214.79 |    3.80 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **24.16 μs** |       **1.819 μs** |      **0.100 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     813.82 μs |      75.521 μs |      4.140 μs |  33.68 |    0.19 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,309.0 ns** |       **177.99 ns** |      **9.76 ns** | **0.0038** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     6,262.5 ns |        69.79 ns |      3.83 ns |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       135.5 ns |         7.79 ns |      0.43 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,421.7 ns |       429.98 ns |     23.57 ns | 0.0076 |     656 B |
| Report         | 1000    | 2       |     5,969.8 ns |        73.97 ns |      4.05 ns | 0.0610 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **3,398.1 ns** |       **251.86 ns** |     **13.81 ns** | **0.0153** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     6,243.8 ns |       994.15 ns |     54.49 ns | 0.0153 |    1336 B |
| AccuracyScore  | 1000    | 10      |       135.0 ns |         9.63 ns |      0.53 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,837.2 ns |       312.48 ns |     17.13 ns | 0.0153 |    1880 B |
| Report         | 1000    | 10      |     9,528.8 ns |       155.07 ns |      8.50 ns | 0.1373 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **366,461.9 ns** |    **14,187.75 ns** |    **777.68 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   710,824.1 ns |    81,471.56 ns |  4,465.73 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    15,779.1 ns |       358.57 ns |     19.65 ns |      - |         - |
| F1Macro        | 100000  | 2       |   338,598.9 ns |    18,071.74 ns |    990.57 ns |      - |     656 B |
| Report         | 100000  | 2       |   371,071.3 ns |    84,190.96 ns |  4,614.79 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **304,818.7 ns** |    **11,025.53 ns** |    **604.35 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   888,573.4 ns |    31,889.09 ns |  1,747.95 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    15,844.6 ns |       249.78 ns |     13.69 ns |      - |         - |
| F1Macro        | 100000  | 10      |   326,268.2 ns |     5,727.02 ns |    313.92 ns |      - |    1880 B |
| Report         | 100000  | 10      |   337,727.3 ns |    31,058.97 ns |  1,702.45 ns |      - |   12832 B |
| **Matrix**         | **1000000** | **2**       | **3,338,761.0 ns** |   **109,002.17 ns** |  **5,974.78 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,549,578.7 ns | 1,187,023.64 ns | 65,064.77 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   257,252.9 ns |     7,460.80 ns |    408.95 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,492,488.4 ns |   715,372.88 ns | 39,212.00 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,548,724.2 ns |   138,186.86 ns |  7,574.49 ns |      - |    5507 B |
| **Matrix**         | **1000000** | **10**      | **3,264,459.9 ns** |   **258,636.63 ns** | **14,176.75 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 9,166,862.8 ns |   797,548.28 ns | 43,716.31 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   264,940.9 ns |     9,952.56 ns |    545.53 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,157,421.4 ns |   346,803.74 ns | 19,009.48 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,241,740.2 ns |   293,852.73 ns | 16,107.06 ns |      - |   12875 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **11,364.77 μs** |    **601.362 μs** |    **32.963 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  37,122.44 μs |  1,701.153 μs |    93.246 μs |     3.27 |    0.01 | 642.8571 | 642.8571 | 642.8571 |  5090397 B |    3,927.78 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **15.93 μs** |      **0.161 μs** |     **0.009 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  35,743.68 μs |  2,108.264 μs |   115.561 μs | 2,244.33 |    6.38 | 600.0000 | 600.0000 | 600.0000 |  5089596 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **201,955.13 μs** | **54,184.593 μs** | **2,970.040 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 228,118.25 μs |  1,120.547 μs |    61.421 μs |     1.13 |    0.01 |        - |        - |        - | 23229116 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **263.88 μs** |      **8.758 μs** |     **0.480 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 228,503.43 μs | 12,586.891 μs |   689.930 μs |   865.94 |    2.64 |        - |        - |        - | 23229116 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-----------:|----------:|------:|----------:|------------:|
| **Dp**         | **4**    |     **72.43 ns** |   **5.576 ns** |  **0.306 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 4    |     72.07 ns |   1.659 ns |  0.091 ns |  1.00 |         - |          NA |
| Dp_Cjk     | 4    |     72.31 ns |   0.868 ns |  0.048 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 4    |     71.81 ns |   0.855 ns |  0.047 ns |  0.99 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **6**    |     **97.34 ns** |   **3.991 ns** |  **0.219 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 6    |     71.38 ns |   5.201 ns |  0.285 ns |  0.73 |         - |          NA |
| Dp_Cjk     | 6    |     97.33 ns |   7.441 ns |  0.408 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 6    |    116.42 ns |   1.057 ns |  0.058 ns |  1.20 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **8**    |    **131.97 ns** |   **4.527 ns** |  **0.248 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 8    |     80.36 ns |   5.024 ns |  0.275 ns |  0.61 |         - |          NA |
| Dp_Cjk     | 8    |    131.71 ns |   1.880 ns |  0.103 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 8    |    152.18 ns |   4.347 ns |  0.238 ns |  1.15 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **10**   |    **179.98 ns** |  **25.562 ns** |  **1.401 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 10   |     87.11 ns |   1.831 ns |  0.100 ns |  0.48 |         - |          NA |
| Dp_Cjk     | 10   |    179.50 ns |   3.662 ns |  0.201 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 10   |    125.06 ns |   3.724 ns |  0.204 ns |  0.69 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **12**   |    **241.08 ns** |   **4.458 ns** |  **0.244 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 12   |    101.09 ns |   6.075 ns |  0.333 ns |  0.42 |         - |          NA |
| Dp_Cjk     | 12   |    242.13 ns |   8.358 ns |  0.458 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 12   |    136.92 ns |   2.288 ns |  0.125 ns |  0.57 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **16**   |    **371.00 ns** |  **10.261 ns** |  **0.562 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 16   |    119.30 ns |   2.587 ns |  0.142 ns |  0.32 |         - |          NA |
| Dp_Cjk     | 16   |    371.49 ns |  10.039 ns |  0.550 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 16   |    158.86 ns |   5.454 ns |  0.299 ns |  0.43 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **24**   |    **765.90 ns** |  **23.284 ns** |  **1.276 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 24   |    156.76 ns |   3.419 ns |  0.187 ns |  0.20 |         - |          NA |
| Dp_Cjk     | 24   |    765.85 ns |  33.437 ns |  1.833 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 24   |    202.26 ns |  21.269 ns |  1.166 ns |  0.26 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **32**   |  **1,364.25 ns** |  **22.029 ns** |  **1.207 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 32   |    194.95 ns |   3.294 ns |  0.181 ns |  0.14 |         - |          NA |
| Dp_Cjk     | 32   |  1,561.27 ns | 382.781 ns | 20.982 ns |  1.14 |         - |          NA |
| Kernel_Cjk | 32   |    249.05 ns |  40.335 ns |  2.211 ns |  0.18 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **48**   |  **2,996.25 ns** | **301.633 ns** | **16.534 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 48   |    271.40 ns |   8.558 ns |  0.469 ns |  0.09 |         - |          NA |
| Dp_Cjk     | 48   |  3,007.71 ns |  18.617 ns |  1.020 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 48   |    335.73 ns |   6.791 ns |  0.372 ns |  0.11 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **64**   |  **5,305.12 ns** | **187.044 ns** | **10.253 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 64   |    345.39 ns |  13.027 ns |  0.714 ns |  0.07 |         - |          NA |
| Dp_Cjk     | 64   |  5,340.42 ns | 270.960 ns | 14.852 ns |  1.01 |         - |          NA |
| Kernel_Cjk | 64   |    425.78 ns |   1.516 ns |  0.083 ns |  0.08 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **96**   | **11,777.13 ns** | **606.316 ns** | **33.234 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 96   |    712.72 ns |  55.924 ns |  3.065 ns |  0.06 |         - |          NA |
| Dp_Cjk     | 96   | 12,218.44 ns | 455.080 ns | 24.944 ns |  1.04 |         - |          NA |
| Kernel_Cjk | 96   |  1,382.28 ns |  26.415 ns |  1.448 ns |  0.12 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **261.76 μs** |  **42.178 μs** |  **2.312 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   282.77 μs |  44.293 μs |  2.428 μs |  1.08 |      - |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    95.01 μs |  10.532 μs |  0.577 μs |  0.36 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   258.15 μs |  10.513 μs |  0.576 μs |  0.99 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **259.71 μs** |  **15.161 μs** |  **0.831 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   298.22 μs |  27.025 μs |  1.481 μs |  1.15 | 0.4883 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    96.37 μs |   6.421 μs |  0.352 μs |  0.37 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   258.17 μs |  22.083 μs |  1.210 μs |  0.99 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,649.82 μs** | **159.469 μs** |  **8.741 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,646.97 μs | 120.529 μs |  6.607 μs |  1.00 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,250.30 μs | 140.367 μs |  7.694 μs |  0.47 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 2,626.02 μs | 381.252 μs | 20.898 μs |  0.99 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,640.25 μs** | **396.356 μs** | **21.726 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,812.15 μs | 137.448 μs |  7.534 μs |  1.07 |      - |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,253.09 μs |  47.972 μs |  2.629 μs |  0.47 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 2,653.61 μs | 268.399 μs | 14.712 μs |  1.01 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error     | StdDev    | Allocated |
|------------ |------------- |-------------:|----------:|----------:|----------:|
| **Embedded**    | **65**           |     **8.052 μs** | **1.0107 μs** | **0.0554 μs** |         **-** |
| EqualLength | 65           |     2.453 μs | 0.2052 μs | 0.0112 μs |         - |
| **Embedded**    | **128**          |    **30.946 μs** | **4.3510 μs** | **0.2385 μs** |         **-** |
| EqualLength | 128          |     4.657 μs | 0.1022 μs | 0.0056 μs |         - |
| **Embedded**    | **512**          | **1,061.764 μs** | **5.7420 μs** | **0.3147 μs** |         **-** |
| EqualLength | 512          |    39.721 μs | 9.9556 μs | 0.5457 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.449 ms |  0.4103 ms | 0.0225 ms |  39.0625 |  35.1563 |  23.4375 |  3711.57 KB |
| TokenizerJsonWordPiece |  7.820 ms |  0.5365 ms | 0.0294 ms |  15.6250 |        - |        - |  5852.23 KB |
| TokenizerJsonUnigram   |  9.458 ms |  0.3903 ms | 0.0214 ms |        - |        - |        - |  4748.81 KB |
| SpieceModel            |  4.878 ms |  5.5562 ms | 0.3046 ms |  39.0625 |  31.2500 |  23.4375 |  4807.65 KB |
| TfidfSave              |  1.491 ms |  0.9640 ms | 0.0528 ms |  17.5781 |  17.5781 |  17.5781 |  2137.07 KB |
| TfidfLoad              |  3.561 ms |  0.2081 ms | 0.0114 ms |  27.3438 |  23.4375 |  15.6250 |  2921.38 KB |
| EmbeddingIndexSave     |  5.520 ms |  2.8853 ms | 0.1582 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  7.533 ms |  2.4163 ms | 0.1324 ms | 125.0000 | 117.1875 | 117.1875 | 16094.39 KB |
| EmbeddingIndexSaveFile | 81.766 ms | 10.5029 ms | 0.5757 ms |        - |        - |        - |   323.53 KB |
| EmbeddingIndexLoadGzip | 83.647 ms |  5.1662 ms | 0.2832 ms |        - |        - |        - | 16095.28 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **7,584.35 μs** |  **60.503 μs** |  **3.316 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,907.68 μs | 542.714 μs | 29.748 μs |  2.23 | 187.5000 | 187.5000 | 187.5000 | 628.53 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **144.98 μs** |  **11.752 μs** |  **0.644 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    203.48 μs |   6.360 μs |  0.349 μs |  1.40 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,992.15 μs** | **183.176 μs** | **10.041 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,347.19 μs |  43.486 μs |  2.384 μs |  1.12 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.24 μs** |   **0.607 μs** |  **0.033 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.77 μs |   1.211 μs |  0.066 μs |  1.19 |        - |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **55.43 μs** |     **0.295 μs** |  **0.016 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    57.59 μs |     1.759 μs |  0.096 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   138.92 μs |     1.700 μs |  0.093 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   566.72 μs |    64.363 μs |  3.528 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **582.96 μs** |    **15.697 μs** |  **0.860 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   575.83 μs |    46.998 μs |  2.576 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,398.41 μs |    19.278 μs |  1.057 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 7,149.40 μs | 1,054.344 μs | 57.792 μs | 320.3125 | 320.3125 | 320.3125 | 8000267 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1     | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|---------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **58.640 ms** |  **5.5875 ms** | **0.3063 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |  11.877 ms |  1.7621 ms | 0.0966 ms |  0.20 |   78.1250 |  78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |  10.384 ms |  0.9973 ms | 0.0547 ms |  0.18 |         - |        - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   8.357 ms |  0.4376 ms | 0.0240 ms |  0.14 |         - |        - |        - |    305.29 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  12.547 ms |  0.4289 ms | 0.0235 ms |  0.21 |         - |        - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **60.908 ms** | **13.0670 ms** | **0.7162 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  14.982 ms |  0.1425 ms | 0.0078 ms |  0.25 |  218.7500 | 218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  12.247 ms |  0.4077 ms | 0.0223 ms |  0.20 |         - |        - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   8.506 ms |  1.7446 ms | 0.0956 ms |  0.14 |         - |        - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  12.286 ms |  1.2357 ms | 0.0677 ms |  0.20 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **998.748 ms** | **12.3853 ms** | **0.6789 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.81 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  49.101 ms | 10.1488 ms | 0.5563 ms |  0.05 |  545.4545 | 545.4545 | 454.5455 |  10335.34 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  40.332 ms |  1.1123 ms | 0.0610 ms |  0.04 |         - |        - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  33.128 ms |  3.9506 ms | 0.2165 ms |  0.03 |         - |        - |        - |   1219.38 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  49.261 ms | 11.7754 ms | 0.6454 ms |  0.05 |         - |        - |        - |   1718.82 KB |       0.014 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **999.208 ms** | **61.0968 ms** | **3.3489 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.44 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  62.493 ms |  4.5117 ms | 0.2473 ms |  0.06 |  875.0000 | 875.0000 | 750.0000 |  18664.76 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  48.750 ms |  7.3380 ms | 0.4022 ms |  0.05 |         - |        - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  34.305 ms |  5.1819 ms | 0.2840 ms |  0.03 |         - |        - |        - |   1719.88 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  49.168 ms |  0.9818 ms | 0.0538 ms |  0.05 |         - |        - |        - |   1718.82 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,243.3 ns |       112.87 ns |       6.19 ns |   0.0648 |        - |        - |     5464 B |
| TfidfSaveAsync         |      1,467.5 ns |        77.28 ns |       4.24 ns |   0.0668 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,294.9 ns |        66.69 ns |       3.66 ns |   0.0648 |        - |        - |     5496 B |
| IndexSaveAsync         |        395.4 ns |        10.27 ns |       0.56 ns |   0.0076 |        - |        - |      648 B |
| CountLoadSeekable1400K | 10,322,258.6 ns | 3,248,741.36 ns | 178,074.48 ns | 656.2500 | 640.6250 | 609.3750 | 12227168 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **2.212 ms** | **0.3614 ms** | **0.0198 ms** |  **1.00** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |  1.585 ms | 0.0990 ms | 0.0054 ms |  0.72 |   3.9063 |        - |        - |   407.6 KB |        0.47 |
| Hashing              | 200       |  2.269 ms | 0.0337 ms | 0.0018 ms |  1.03 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       |  1.799 ms | 0.0819 ms | 0.0045 ms |  0.81 |   1.9531 |        - |        - |  273.65 KB |        0.31 |
|                      |           |           |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **10.340 ms** | **0.2290 ms** | **0.0126 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.52 KB** |        **1.00** |
| CountWithStopWords   | 1000      |  8.142 ms | 0.4683 ms | 0.0257 ms |  0.79 | 375.0000 | 375.0000 | 375.0000 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 11.176 ms | 0.9816 ms | 0.0538 ms |  1.08 | 500.0000 | 500.0000 | 500.0000 | 2348.47 KB |        0.68 |
| HashingWithStopWords | 1000      |  9.288 ms | 0.9394 ms | 0.0515 ms |  0.90 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean          | Error         | StdDev      | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|------------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **36,316.102 μs** | **3,160.2023 μs** | **173.2214 μs** |       **-** |       **-** |       **-** | **1554.62 KB** |
| Construct                  | [^ ]+         |      2.111 μs |     0.1674 μs |   0.0092 μs |  0.0648 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.356 μs |     0.4405 μs |   0.0241 μs |  0.0877 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **29,640.577 μs** |   **220.0778 μs** |  **12.0632 μs** |       **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1.993 μs |     0.0605 μs |   0.0033 μs |  0.0648 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.882 μs |     0.3495 μs |   0.0192 μs |  0.0877 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **40,475.280 μs** |   **840.9211 μs** |  **46.0937 μs** |       **-** |       **-** |       **-** |    **1556 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.423 μs |     0.1630 μs |   0.0089 μs |  0.0801 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.690 μs |     0.5264 μs |   0.0289 μs |  0.1068 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **22,468.929 μs** | **1,055.4676 μs** |  **57.8537 μs** | **62.5000** | **62.5000** | **62.5000** |  **9743.6 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.060 μs |     0.0492 μs |   0.0027 μs |  0.0210 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1.859 μs |     0.2243 μs |   0.0123 μs |  0.0458 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **37,377.245 μs** | **4,644.8275 μs** | **254.5987 μs** |       **-** |       **-** |       **-** | **1554.49 KB** |
| Construct                  | \\S+          |      2.333 μs |     0.1630 μs |   0.0089 μs |  0.0610 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.606 μs |     0.3429 μs |   0.0188 μs |  0.0877 |       - |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **18.84 ms** |  **1.749 ms** | **0.096 ms** |  **1.00** |    **0.01** |  **93.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  48.19 ms |  6.143 ms | 0.337 ms |  2.56 |    0.02 |        - |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **SentencePiece** |  **44.73 ms** |  **2.141 ms** | **0.117 ms** |  **1.00** |    **0.00** |        **-** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  54.12 ms |  1.900 ms | 0.104 ms |  1.21 |    0.00 |        - |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **87.08 ms** |  **6.717 ms** | **0.368 ms** |  **1.00** |    **0.01** | **333.3333** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 273.04 ms | 56.012 ms | 3.070 ms |  3.14 |    0.03 | 500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **41.32 ns** | **1.864 ns** | **0.102 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  37.00 ns | 6.842 ns | 0.375 ns |  0.90 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **80.87 ns** | **1.199 ns** | **0.066 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  68.06 ns | 4.313 ns | 0.236 ns |  0.84 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **106.00 ns** | **2.657 ns** | **0.146 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 |  89.98 ns | 2.806 ns | 0.154 ns |  0.85 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean        | Error       | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **261.5 μs** |    **18.46 μs** |  **1.01 μs** |  **1.00** |    **0.00** |    **2.4414** |    **0.4883** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    288.9 μs |    26.91 μs |  1.48 μs |  1.10 |    0.01 |    2.4414 |         - |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    776.8 μs |    11.16 μs |  0.61 μs |  2.97 |    0.01 |    6.8359 |    1.9531 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,146.0 μs |   145.49 μs |  7.97 μs |  8.21 |    0.04 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    291.7 μs |     3.56 μs |  0.20 μs |  1.12 |    0.00 |    1.4648 |         - |         - |   136.13 KB |        0.66 |
|                       |           |             |             |          |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,648.8 μs** |    **36.33 μs** |  **1.99 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,795.0 μs |    47.30 μs |  2.59 μs |  1.09 |    0.00 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,966.8 μs |   484.69 μs | 26.57 μs |  2.41 |    0.01 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 12,990.8 μs | 1,031.88 μs | 56.56 μs |  7.88 |    0.03 | 1015.6250 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,738.9 μs |    92.87 μs |  5.09 μs |  1.05 |    0.00 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.992 ms** |   **0.3569 ms** |  **0.0196 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  48.426 ms |  49.8321 ms |  2.7315 ms | 12.13 |    0.59 |  7400.0000 |  7400.0000 |  7400.0000 |  28.27 MB |       15.85 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **18.575 ms** |   **0.7016 ms** |  **0.0385 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 353.283 ms | 379.6507 ms | 20.8099 ms | 19.02 |    0.97 | 72000.0000 | 72000.0000 | 72000.0000 | 324.45 MB |       40.01 |

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
| kmeans_weighted_10000 | 5.222 | 3.839 | 0.74x | 5.228 | 7.661 | 1.47x |
| kmeans_restarts_10000 | 21.435 | 9.943 | 0.46x | 21.445 | 19.866 | 0.93x |
| kmeans_weighted_100000 | 54.643 | 30.319 | 0.55x | 54.833 | 48.483 | 0.88x |
| kmeans_restarts_100000 | 213.495 | 80.954 | 0.38x | 213.789 | 147.488 | 0.69x |
| dbscan_weighted_5000 | 7.403 | 30.863 | 4.17x | 7.688 | 30.860 | 4.01x |
| dbscan_weighted_20000 | 115.567 | 195.306 | 1.69x | 116.608 | 195.281 | 1.67x |

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
| stratified_weighted_1000 | 1.587 | 95.172 | 59.97x | 1.587 | 95.153 | 59.97x |
| ridge_robust_1000 | 1.651 | 89.846 | 54.41x | 1.651 | 89.836 | 54.41x |
| lasso_1000 | 14.590 | 795.066 | 54.49x | 14.590 | 795.001 | 54.49x |
| ph_test_1000 | 0.143 | 16.735 | 117.29x | 0.143 | 16.732 | 117.27x |
| predict_survival_1000 | 0.033 | 1.545 | 47.34x | 0.033 | 1.544 | 47.34x |
| time_varying_1000 | 58.402 | 263.683 | 4.51x | 58.398 | 263.668 | 4.52x |
| stratified_weighted_10000 | 16.117 | 516.708 | 32.06x | 16.183 | 516.675 | 31.93x |
| ridge_robust_10000 | 17.499 | 947.944 | 54.17x | 17.774 | 947.916 | 53.33x |
| lasso_10000 | 137.596 | 5159.870 | 37.50x | 137.663 | 5159.256 | 37.48x |
| ph_test_10000 | 2.040 | 164.638 | 80.69x | 2.091 | 164.632 | 78.74x |
| predict_survival_10000 | 0.036 | 1.546 | 43.18x | 0.036 | 1.546 | 43.18x |
| time_varying_10000 | 2145.271 | 4844.964 | 2.26x | 2145.136 | 4844.515 | 2.26x |

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
| cv_plus_n1000 | 5.028 | 37.684 | 7.50x | 5.027 | 150.527 | 29.94x |
| cv_minmax_n1000 | 3.509 | 4.705 | 1.34x | 3.509 | 18.743 | 5.34x |
| cv_plus_gamma_n1000 | 5.806 | 39.204 | 6.75x | 5.805 | 156.123 | 26.89x |
| bootstrap_plus_n1000 | 19.505 | 40.133 | 2.06x | 19.503 | 160.214 | 8.21x |
| cv_plus_n10000 | 67.046 | 156.796 | 2.34x | 67.052 | 526.962 | 7.86x |
| cv_minmax_n10000 | 37.844 | 34.606 | 0.91x | 37.836 | 137.617 | 3.64x |
| cv_plus_gamma_n10000 | 76.066 | 166.245 | 2.19x | 76.063 | 535.430 | 7.04x |
| bootstrap_plus_n10000 | 209.283 | 154.980 | 0.74x | 209.266 | 527.615 | 2.52x |

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
| glm_negative_binomial_n1000 | 0.359 | 2.553 | 7.11x | 0.359 | 2.553 | 7.11x |
| glm_gamma_n1000 | 0.471 | 3.251 | 6.90x | 0.471 | 3.250 | 6.90x |
| glm_poisson_exposure_n1000 | 0.381 | 2.976 | 7.81x | 0.381 | 2.976 | 7.81x |
| mnlogit_n1000 | 0.836 | 12.100 | 14.47x | 0.836 | 12.100 | 14.47x |
| glm_negative_binomial_n10000 | 3.293 | 10.236 | 3.11x | 3.301 | 10.235 | 3.10x |
| glm_gamma_n10000 | 4.114 | 12.130 | 2.95x | 4.124 | 12.129 | 2.94x |
| glm_poisson_exposure_n10000 | 4.166 | 11.919 | 2.86x | 4.171 | 11.918 | 2.86x |
| mnlogit_n10000 | 8.386 | 63.845 | 7.61x | 8.396 | 63.831 | 7.60x |
| glm_negative_binomial_n100000 | 34.900 | 126.110 | 3.61x | 35.179 | 498.787 | 14.18x |
| glm_gamma_n100000 | 42.941 | 134.727 | 3.14x | 43.272 | 538.160 | 12.44x |
| glm_poisson_exposure_n100000 | 51.913 | 129.121 | 2.49x | 52.306 | 515.868 | 9.86x |
| mnlogit_n100000 | 84.069 | 676.934 | 8.05x | 84.164 | 1414.168 | 16.80x |

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
| 2sls_robust_n1000 | 1.640 | 49.562 | 30.22x | 1.640 | 49.557 | 30.21x |
| liml_robust_n1000 | 1.802 | 49.087 | 27.23x | 1.802 | 49.084 | 27.24x |
| gmm_robust_n1000 | 1.652 | 48.315 | 29.24x | 1.652 | 48.312 | 29.24x |
| 2sls_kernel_n1000 | 9.501 | 51.085 | 5.38x | 9.501 | 51.080 | 5.38x |
| 2sls_clustered_n1000 | 1.587 | 50.381 | 31.75x | 1.587 | 50.380 | 31.75x |
| 2sls_robust_n10000 | 16.846 | 231.006 | 13.71x | 17.106 | 461.993 | 27.01x |
| liml_robust_n10000 | 19.023 | 227.121 | 11.94x | 19.336 | 454.352 | 23.50x |
| gmm_robust_n10000 | 17.340 | 225.023 | 12.98x | 17.736 | 449.996 | 25.37x |
| 2sls_kernel_n10000 | 101.002 | 241.275 | 2.39x | 101.525 | 482.517 | 4.75x |
| 2sls_clustered_n10000 | 16.482 | 247.077 | 14.99x | 16.754 | 494.047 | 29.49x |
| 2sls_robust_n100000 | 177.621 | 2554.402 | 14.38x | 178.705 | 8054.764 | 45.07x |
| liml_robust_n100000 | 192.451 | 2564.533 | 13.33x | 194.264 | 8108.882 | 41.74x |
| gmm_robust_n100000 | 180.488 | 2532.858 | 14.03x | 182.476 | 7781.155 | 42.64x |
| 2sls_kernel_n100000 | 3784.786 | 3539.244 | 0.94x | 3786.116 | 10790.087 | 2.85x |
| 2sls_clustered_n100000 | 169.881 | 2742.662 | 16.14x | 171.124 | 8856.443 | 51.75x |

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
| transform_frobenius_n100 | 2.649 | 0.692 | 0.26x | 2.648 | 0.692 | 0.26x |
| transform_frobenius_n500 | 8.075 | 1.542 | 0.19x | 8.075 | 1.541 | 0.19x |
| transform_frobenius_n2000 | 29.017 | 5.451 | 0.19x | 29.043 | 21.746 | 0.75x |
| transform_kl_n100 | 1.288 | 12.992 | 10.09x | 1.288 | 12.991 | 10.09x |
| transform_kl_n500 | 5.686 | 29.241 | 5.14x | 5.686 | 29.236 | 5.14x |
| transform_kl_n2000 | 22.476 | 108.900 | 4.85x | 22.532 | 434.605 | 19.29x |

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
| ols_summary_n1000 | 0.047 | 1.988 | 42.15x | 0.047 | 1.988 | 42.15x |
| ols_hac_n1000 | 0.174 | 2.551 | 14.65x | 0.174 | 2.551 | 14.65x |
| ols_cluster_n1000 | 0.065 | 2.583 | 39.53x | 0.065 | 2.583 | 39.53x |
| ols_summary_n10000 | 0.448 | 9.766 | 21.81x | 0.448 | 9.764 | 21.81x |
| ols_hac_n10000 | 1.774 | 10.920 | 6.16x | 1.778 | 10.918 | 6.14x |
| ols_cluster_n10000 | 0.619 | 11.413 | 18.43x | 0.621 | 11.411 | 18.39x |
| ols_summary_n100000 | 4.595 | 113.594 | 24.72x | 4.616 | 453.360 | 98.21x |
| ols_hac_n100000 | 18.275 | 127.671 | 6.99x | 18.483 | 509.347 | 27.56x |
| ols_cluster_n100000 | 7.089 | 130.420 | 18.40x | 7.232 | 520.000 | 71.90x |

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
| sparse_10000 | 1.899 | 4.226 | 2.23x | 1.902 | 4.226 | 2.22x |
| sparse_infrequent_10000 | 2.349 | 4.320 | 1.84x | 2.354 | 4.298 | 1.83x |
| sparse_100000 | 18.704 | 31.533 | 1.69x | 18.979 | 31.530 | 1.66x |
| sparse_infrequent_100000 | 20.333 | 25.606 | 1.26x | 20.686 | 25.604 | 1.24x |

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
| fe_entity_n1000 | 0.227 | 17.219 | 75.92x | 0.227 | 17.218 | 75.93x |
| fe_twoway_clustered_n1000 | 0.410 | 27.179 | 66.21x | 0.411 | 27.177 | 66.20x |
| fe_kernel_n1000 | 0.246 | 17.769 | 72.30x | 0.246 | 17.768 | 72.30x |
| between_robust_n1000 | 0.128 | 16.611 | 129.78x | 0.128 | 16.609 | 129.77x |
| first_difference_robust_n1000 | 0.243 | 19.880 | 81.67x | 0.243 | 19.878 | 81.67x |
| random_effects_n1000 | 0.237 | 22.987 | 97.19x | 0.236 | 22.984 | 97.19x |
| fe_entity_n10000 | 2.663 | 26.815 | 10.07x | 2.725 | 26.811 | 9.84x |
| fe_twoway_clustered_n10000 | 4.987 | 50.080 | 10.04x | 5.239 | 100.140 | 19.12x |
| fe_kernel_n10000 | 2.849 | 27.534 | 9.66x | 2.873 | 27.532 | 9.58x |
| between_robust_n10000 | 1.539 | 26.172 | 17.01x | 1.543 | 26.169 | 16.96x |
| first_difference_robust_n10000 | 2.854 | 30.964 | 10.85x | 2.861 | 30.963 | 10.82x |
| random_effects_n10000 | 2.766 | 36.115 | 13.06x | 2.788 | 36.115 | 12.95x |
| fe_entity_n100000 | 33.115 | 173.501 | 5.24x | 34.163 | 692.024 | 20.26x |
| fe_twoway_clustered_n100000 | 51.162 | 327.261 | 6.40x | 52.175 | 1235.248 | 23.68x |
| fe_kernel_n100000 | 33.035 | 165.562 | 5.01x | 33.657 | 660.766 | 19.63x |
| between_robust_n100000 | 19.301 | 162.219 | 8.40x | 19.742 | 550.324 | 27.88x |
| first_difference_robust_n100000 | 34.184 | 191.863 | 5.61x | 35.125 | 765.194 | 21.79x |
| random_effects_n100000 | 32.126 | 222.844 | 6.94x | 32.966 | 888.659 | 26.96x |

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
| welch_t_n1000 | 0.005 | 0.592 | 120.10x | 0.005 | 0.592 | 120.09x |
| mann_whitney_n1000 | 0.051 | 0.615 | 12.07x | 0.051 | 0.615 | 12.07x |
| chi_square_n1000 | 0.000 | 0.228 | 1289.14x | 0.000 | 0.228 | 1289.03x |
| fligner_n1000 | 0.124 | 0.698 | 5.64x | 0.124 | 0.697 | 5.64x |
| anderson_ksamp_n1000 | 0.218 | 0.365 | 1.68x | 0.218 | 0.365 | 1.68x |
| spearman_matrix_n1000 | 0.197 | 0.710 | 3.60x | 0.197 | 0.710 | 3.60x |
| pointbiserial_n1000 | 0.011 | 0.434 | 40.87x | 0.011 | 0.434 | 40.86x |
| welch_t_n10000 | 0.047 | 0.637 | 13.65x | 0.047 | 0.637 | 13.65x |
| mann_whitney_n10000 | 0.587 | 2.522 | 4.29x | 0.587 | 2.522 | 4.29x |
| chi_square_n10000 | 0.001 | 0.235 | 211.35x | 0.001 | 0.235 | 211.35x |
| fligner_n10000 | 1.343 | 3.006 | 2.24x | 1.392 | 3.005 | 2.16x |
| anderson_ksamp_n10000 | 2.145 | 3.609 | 1.68x | 2.218 | 3.608 | 1.63x |
| spearman_matrix_n10000 | 2.083 | 4.358 | 2.09x | 2.125 | 4.358 | 2.05x |
| pointbiserial_n10000 | 0.126 | 0.515 | 4.08x | 0.126 | 0.515 | 4.08x |
| welch_t_n100000 | 0.497 | 1.268 | 2.55x | 0.497 | 1.268 | 2.55x |
| mann_whitney_n100000 | 6.276 | 27.399 | 4.37x | 6.275 | 27.394 | 4.37x |
| chi_square_n100000 | 0.012 | 0.251 | 21.62x | 0.012 | 0.251 | 21.62x |
| fligner_n100000 | 19.164 | 32.441 | 1.69x | 19.665 | 32.438 | 1.65x |
| anderson_ksamp_n100000 | 21.067 | 43.579 | 2.07x | 21.451 | 43.576 | 2.03x |
| spearman_matrix_n100000 | 21.190 | 53.215 | 2.51x | 21.572 | 53.211 | 2.47x |
| pointbiserial_n100000 | 1.439 | 1.627 | 1.13x | 1.451 | 6.484 | 4.47x |

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
| fit_1000 | 9.146 | 59.748 | 6.53x | 9.145 | 238.279 | 26.06x |
| fit_penalised_weighted_1000 | 8.755 | 64.527 | 7.37x | 8.764 | 257.789 | 29.41x |
| predict_survival_1000 | 0.766 | 1.893 | 2.47x | 0.769 | 1.892 | 2.46x |
| fit_10000 | 255.881 | 617.954 | 2.42x | 257.132 | 1020.451 | 3.97x |
| fit_penalised_weighted_10000 | 239.357 | 624.648 | 2.61x | 239.951 | 1022.931 | 4.26x |
| predict_survival_10000 | 3.383 | 2.923 | 0.86x | 3.568 | 11.652 | 3.27x |

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
| logrank_wilcoxon_weighted_1000 | 0.023 | 11.533 | 508.89x | 0.023 | 11.529 | 508.77x |
| logrank_fleming_harrington_1000 | 0.031 | 17.592 | 564.33x | 0.031 | 17.586 | 564.19x |
| multigroup_1000 | 0.105 | 23.914 | 226.93x | 0.105 | 23.909 | 226.90x |
| pairwise_1000 | 0.327 | 119.785 | 365.94x | 0.327 | 119.771 | 365.93x |
| rmst_1000 | 0.099 | 972.726 | 9813.04x | 0.099 | 972.641 | 9813.96x |
| fixed_point_1000 | 0.040 | 12.382 | 310.53x | 0.040 | 12.379 | 310.50x |
| concordance_1000 | 0.136 | 6.693 | 49.27x | 0.136 | 6.692 | 49.27x |
| logrank_wilcoxon_weighted_10000 | 0.353 | 13.121 | 37.21x | 0.353 | 13.120 | 37.20x |
| logrank_fleming_harrington_10000 | 0.438 | 21.125 | 48.24x | 0.438 | 21.122 | 48.24x |
| multigroup_10000 | 2.259 | 35.707 | 15.81x | 2.263 | 35.706 | 15.78x |
| pairwise_10000 | 4.203 | 136.257 | 32.42x | 4.202 | 136.252 | 32.42x |
| rmst_10000 | 1.595 | 1054.969 | 661.28x | 1.597 | 1054.864 | 660.54x |
| fixed_point_10000 | 0.501 | 14.719 | 29.36x | 0.501 | 14.718 | 29.36x |
| concordance_10000 | 2.512 | 80.163 | 31.91x | 2.512 | 80.164 | 31.91x |
| logrank_wilcoxon_weighted_100000 | 4.131 | 21.174 | 5.13x | 4.228 | 21.171 | 5.01x |
| logrank_fleming_harrington_100000 | 4.025 | 34.473 | 8.57x | 4.068 | 34.471 | 8.47x |
| multigroup_100000 | 10.746 | 49.342 | 4.59x | 10.841 | 49.339 | 4.55x |
| pairwise_100000 | 42.634 | 222.395 | 5.22x | 43.454 | 222.377 | 5.12x |
| rmst_100000 | 9.209 | 391.513 | 42.52x | 9.347 | 391.496 | 41.88x |
| fixed_point_100000 | 4.742 | 27.360 | 5.77x | 4.959 | 27.355 | 5.52x |
| concordance_100000 | 31.520 | 664.044 | 21.07x | 31.677 | 663.984 | 20.96x |

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
| weibull_1000 | 3.720 | 51.430 | 13.82x | 3.720 | 51.426 | 13.82x |
| loglogistic_left_1000 | 6.985 | 74.661 | 10.69x | 6.984 | 74.655 | 10.69x |
| lognormal_interval_1000 | 5.857 | 133.963 | 22.87x | 5.856 | 133.951 | 22.87x |
| generalized_gamma_1000 | 40.100 | 710.966 | 17.73x | 40.183 | 710.884 | 17.69x |
| weibull_aft_1000 | 7.799 | 100.364 | 12.87x | 7.798 | 100.344 | 12.87x |
| lognormal_aft_ridge_robust_1000 | 11.805 | 3799.011 | 321.81x | 11.901 | 3798.681 | 319.19x |
| loglogistic_aft_interval_1000 | 11.946 | 158.683 | 13.28x | 11.946 | 158.675 | 13.28x |
| breslow_fleming_harrington_1000 | 0.272 | 5.847 | 21.47x | 0.272 | 5.846 | 21.47x |
| kaplan_meier_left_1000 | 0.089 | 6.879 | 77.65x | 0.089 | 6.879 | 77.65x |
| weibull_10000 | 37.165 | 62.695 | 1.69x | 37.164 | 62.690 | 1.69x |
| loglogistic_left_10000 | 77.756 | 145.474 | 1.87x | 77.753 | 145.456 | 1.87x |
| lognormal_interval_10000 | 61.534 | 277.514 | 4.51x | 61.535 | 277.481 | 4.51x |
| generalized_gamma_10000 | 375.337 | 1850.414 | 4.93x | 375.279 | 1850.241 | 4.93x |
| weibull_aft_10000 | 79.794 | 124.696 | 1.56x | 79.794 | 124.682 | 1.56x |
| lognormal_aft_ridge_robust_10000 | 154.590 | 35480.436 | 229.51x | 154.571 | 35477.230 | 229.52x |
| loglogistic_aft_interval_10000 | 112.377 | 228.304 | 2.03x | 112.370 | 228.280 | 2.03x |
| breslow_fleming_harrington_10000 | 2.867 | 7.308 | 2.55x | 2.866 | 7.307 | 2.55x |
| kaplan_meier_left_10000 | 0.957 | 8.394 | 8.77x | 0.957 | 8.393 | 8.77x |

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
| var_n1000 | 0.063 | 2.175 | 34.55x | 0.063 | 2.175 | 34.55x |
| var_n10000 | 0.672 | 13.671 | 20.34x | 0.675 | 13.670 | 20.26x |
| var_n100000 | 6.463 | 128.108 | 19.82x | 6.604 | 128.103 | 19.40x |

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
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `SurvivalBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `VectorAutoregressionBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 21.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=AccuracyAlone | `MlNet` | step | 865.94 | 2030.61 | -57% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | step | 1.13 | 2.28 | -50% | 39% |
| `LcsGateBenchmarks` | Band=20 | `Kernel` | step | 0.16 | 0.11 | +45% | 30% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x50 | `MetaNumerics_VarianceFraction` | step | 214.79 | 156.11 | +38% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | step | 0.79 | 1.19 | -34% | 30% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | step | 0.2 | 0.15 | +33% | 30% |
| `LcsGateBenchmarks` | Band=64 | `Kernel` | step | 0.04 | 0.03 | +33% | 30% |
| `LcsGateBenchmarks` | Band=20 | `Kernel_Cjk` | step | 0.24 | 0.18 | +33% | 30% |
| `FuzzCodePointBenchmarks` | — | `Ratio_DistinctAstral` | step | 46.473 | 35.356 | +31% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | step | 0.35 | 0.51 | -31% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Fit` | step | 4.82 | 7.01 | -31% | 30% |
| `BkTreeBenchmarks` | Radius=2, Shape=uniform | `TreeWithinDistance` | step | 1.39 | 1.06 | +31% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 0.99 | 1.39 | -29% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.99 | 1.38 | -28% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.07 | 1.43 | -25% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | drift | 0.11 | 0.145 | -24% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 3.27 | 5.43 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 3.16 | 4.11 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | drift | 1.49 | 1.89 | -21% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.99 | 1.25 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 2.3 | 2.89 | -20% | 20% |

**Still away from their median: 31.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | step | 8.21 | 0.91 | +802% | 30% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | step | 17.03 | 4.53 | +276% | 82% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | step | 27.79 | 7.64 | +264% | 50% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | step | 11.28 | 3.92 | +188% | 71% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | step | 18.91 | 7.01 | +170% | 86% |
| `VectorizerBenchmarks` | Documents=1000 | `CountCharWordBoundary` | step | 7.88 | 2.94 | +168% | 30% |
| `VectorizerBenchmarks` | Documents=200 | `CountBigrams` | step | 2.97 | 1.19 | +150% | 30% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | step | 14.03 | 6.98 | +101% | 82% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | step | 13.34 | 7.04 | +89% | 34% |
| `VectorizerBenchmarks` | Documents=1000 | `CountBigrams` | step | 2.41 | 1.45 | +66% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | step | 8.31 | 5.026 | +65% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | step | 1.118 | 2.243 | -50% | 30% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | step | 1.11 | 2.04 | -46% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | step | 0.006 | 0.01 | -40% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.36 | 0.18 | +39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_MannWhitney` | drift | 9.529 | 14.8845 | -36% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_StudentT` | step | 3.087 | 4.818 | -36% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_OneWayAnova` | step | 4.639 | 7.24 | -36% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KruskalWallis` | drift | 22.093 | 33.582 | -34% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | step | 0.002 | 0.003 | -33% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | step | 0.024 | 0.036 | -33% | 30% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 7.64 | 5.77 | +32% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.37 | 0.19 | +32% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KolmogorovSmirnov` | step | 40.263 | 57.615 | -30% | 30% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.15 | 1.64 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_MannWhitney` | drift | 39.475 | 56.0405 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KruskalWallis` | drift | 68.145 | 96.686 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | drift | 0.012 | 0.017 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KolmogorovSmirnov` | drift | 27.795 | 39.166 | -29% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.4 | 1.3 | +24% | 20% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | drift | 12.13 | 10.02 | +21% | 20% |
