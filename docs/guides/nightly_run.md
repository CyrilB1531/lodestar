# Nightly benchmark run

<!-- nightly-baseline: fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SimilaritySketchBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks cluster-weighted cox-extended cross-conformal glm iv nmf-transform ols onehot panel survival-aalen survival-family survival-parametric var -->
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

- Commit: `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`
- Previous run: `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`
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
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error            | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |       **1,012.3 ns** |        **157.85 ns** |         **8.65 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         797.9 ns |         31.95 ns |         1.75 ns |     0.79 |    0.01 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,657.4 ns |        348.97 ns |        19.13 ns |     2.63 |    0.03 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,716.3 ns |         61.83 ns |         3.39 ns |     3.67 |    0.03 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,353.4 ns |        386.19 ns |        21.17 ns |     5.29 |    0.04 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,882.8 ns |      2,125.22 ns |       116.49 ns |    10.75 |    0.13 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,314.2 ns |        277.99 ns |        15.24 ns |     5.25 |    0.04 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,780.5 ns** |     **30,365.60 ns** |     **1,664.44 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,780.9 ns |      1,746.78 ns |        95.75 ns |     0.65 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,144,467.3 ns |    251,932.23 ns |    13,809.26 ns |    13.19 |    0.26 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,942,567.9 ns |     67,091.80 ns |     3,677.53 ns |    22.39 |    0.37 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,141,410.3 ns |     16,798.45 ns |       920.78 ns |    24.68 |    0.41 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,577,860.5 ns | 24,056,910.86 ns | 1,318,640.47 ns | 3,326.18 |   56.21 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,126,410.9 ns |     39,723.10 ns |     2,177.36 ns |    24.51 |    0.40 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.04 μs** |  **9.958 μs** | **0.546 μs** |  **1.00** |    **0.02** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    33.43 μs |  2.113 μs | 0.116 μs |  1.08 |    0.02 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,001.90 μs** | **68.478 μs** | **3.753 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,456.30 μs | 51.686 μs | 2.833 μs |  0.73 |    0.00 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

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

| Method                         | SampleSize | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **4,615.5 ns** |     **813.77 ns** |    **44.61 ns** |   **1.00** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,189.3 ns |     167.06 ns |     9.16 ns |   0.47 |    0.00 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,228.3 ns |      70.77 ns |     3.88 ns |   0.48 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,843.5 ns |     737.95 ns |    40.45 ns |   0.83 |    0.01 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,479.9 ns |   2,218.77 ns |   121.62 ns |   1.62 |    0.03 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |    10,216.1 ns |   5,670.87 ns |   310.84 ns |   2.21 |    0.06 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,484.3 ns |     380.39 ns |    20.85 ns |   0.54 |    0.01 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,031.1 ns |     110.95 ns |     6.08 ns |   0.87 |    0.01 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,658.6 ns |     116.87 ns |     6.41 ns |   0.36 |    0.00 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,158.5 ns |     134.25 ns |     7.36 ns |   0.68 |    0.01 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,144.6 ns |      63.16 ns |     3.46 ns |   0.25 |    0.00 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,049.4 ns |      69.18 ns |     3.79 ns |   0.23 |    0.00 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       456.4 ns |      11.35 ns |     0.62 ns |   0.10 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,533.8 ns |     166.76 ns |     9.14 ns |   0.33 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       111.6 ns |       6.10 ns |     0.33 ns |   0.02 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       673.5 ns |      40.56 ns |     2.22 ns |   0.15 |    0.00 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |             |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **62,154.2 ns** |   **3,623.33 ns** |   **198.61 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,922.5 ns |   2,862.10 ns |   156.88 ns |  2.879 |    0.01 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   695,264.1 ns |  57,478.14 ns | 3,150.57 ns | 11.186 |    0.05 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,074,834.0 ns |  87,822.55 ns | 4,813.85 ns | 33.382 |    0.11 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,475,834.5 ns |  20,824.93 ns | 1,141.48 ns | 23.745 |    0.07 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,632,962.8 ns | 170,341.30 ns | 9,336.98 ns | 58.451 |    0.21 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,454,505.2 ns |  70,563.72 ns | 3,867.84 ns | 23.402 |    0.08 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,152,492.8 ns | 155,497.09 ns | 8,523.32 ns | 34.632 |    0.15 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    85,719.5 ns |   1,934.11 ns |   106.02 ns |  1.379 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   268,211.6 ns |   4,320.18 ns |   236.80 ns |  4.315 |    0.01 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   190,286.6 ns |   7,577.52 ns |   415.35 ns |  3.062 |    0.01 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   384,495.1 ns |  10,525.29 ns |   576.93 ns |  6.186 |    0.02 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       460.3 ns |       4.56 ns |     0.25 ns |  0.007 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,544.1 ns |      33.42 ns |     1.83 ns |  0.025 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       114.5 ns |      10.24 ns |     0.56 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       662.1 ns |      41.01 ns |     2.25 ns |  0.011 |    0.00 | 0.0572 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

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

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,811.6 ns** |     **704.34 ns** |    **38.61 ns** |  **1.00** |    **0.02** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,915.5 ns |     431.48 ns |    23.65 ns |  1.39 |    0.02 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       607.0 ns |      84.86 ns |     4.65 ns |  0.22 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,155.2 ns |      41.22 ns |     2.26 ns |  0.41 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       812.5 ns |      48.34 ns |     2.65 ns |  0.29 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,610.7 ns |      93.23 ns |     5.11 ns |  1.64 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,588.2 ns |     312.59 ns |    17.13 ns |  3.05 |    0.04 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,316.1 ns |     397.99 ns |    21.82 ns |  2.25 |    0.03 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,336.5 ns |     385.11 ns |    21.11 ns |  1.54 |    0.02 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **373,272.0 ns** |  **22,845.24 ns** | **1,252.22 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,326,298.1 ns |  75,622.12 ns | 4,145.10 ns | 3.553 |    0.01 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,286.3 ns |     866.98 ns |    47.52 ns | 0.151 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,738.7 ns |   5,184.12 ns |   284.16 ns | 0.302 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,047.7 ns |     176.88 ns |     9.70 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   900,934.2 ns | 112,131.46 ns | 6,146.30 ns | 2.414 |    0.02 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,390,507.1 ns |  37,958.47 ns | 2,080.63 ns | 3.725 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,010.0 ns |     195.39 ns |    10.71 ns | 0.029 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,936.9 μs** |      **78.87 μs** |     **4.32 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    88,764.3 μs |   4,073.04 μs |   223.26 μs |  45.83 |    0.13 |  3666.6667 |  1500.0000 |  833.3333 |   59880.8 KB |       58.56 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,835.9 μs** |      **35.26 μs** |     **1.93 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    90,019.1 μs |  25,266.76 μs | 1,384.96 μs |  49.03 |    0.65 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **774.8 μs** |      **13.72 μs** |     **0.75 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   144,615.9 μs |  26,631.37 μs | 1,459.76 μs | 186.65 |    1.64 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,243.6 μs** |     **657.48 μs** |    **36.04 μs** |   **1.00** |    **0.02** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    78,116.0 μs |  55,512.64 μs | 3,042.84 μs |  34.82 |    1.27 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,747.4 μs** |   **3,668.46 μs** |   **201.08 μs** |   **1.00** |    **0.02** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,766,251.3 μs |  54,896.41 μs | 3,009.06 μs | 112.17 |    1.25 | 35000.0000 | 10000.0000 | 4000.0000 | 537149.55 KB |       60.18 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,294.9 μs** |   **1,818.43 μs** |    **99.67 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,668,401.9 μs | 134,594.78 μs | 7,377.59 μs | 116.72 |    0.84 | 48000.0000 | 12000.0000 | 4000.0000 | 747349.05 KB |       83.74 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,485.7 μs** |     **394.80 μs** |    **21.64 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,005,208.1 μs |  82,845.08 μs | 4,541.02 μs | 463.36 |    1.47 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.18 KB |    8,147.90 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,771.3 μs** |   **1,628.35 μs** |    **89.26 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,732,605.7 μs | 101,768.34 μs | 5,578.27 μs |  97.50 |    0.50 | 38000.0000 | 11000.0000 | 4000.0000 | 573618.52 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 26.05 ms |  5.661 ms | 0.310 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 47.53 ms | 12.061 ms | 0.661 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.93 MB |
| CountLoad4M      | 46.60 ms | 14.462 ms | 0.793 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

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

| Method                | Mean          | Error         | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 183,090.67 μs | 37,835.195 μs | 2,073.875 μs |         - |         - |        - | 54334.63 KB |
| CountSave240K         |     405.40 μs |     51.879 μs |     2.844 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     439.78 μs |    174.458 μs |     9.563 μs |   11.2305 |    4.8828 |   4.8828 |   739.44 KB |
| TfidfSave45K          |      48.04 μs |      1.402 μs |     0.077 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      50.55 μs |      8.308 μs |     0.455 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     290.00 μs |     47.116 μs |     2.583 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     299.58 μs |     37.957 μs |     2.081 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  26,773.25 μs | 37,501.387 μs | 2,055.578 μs |  593.7500 |  562.5000 | 187.5000 | 22048.77 KB |
| CountLoadAsync4M      |  58,952.36 μs | 14,978.324 μs |   821.013 μs | 1181.8182 | 1090.9091 | 363.6364 | 44983.59 KB |
| CountLoad4M           |  53,941.09 μs | 18,500.920 μs | 1,014.098 μs | 1181.8182 | 1090.9091 | 363.6364 | 44976.85 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

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

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     18.999 μs |    26.556 μs |   1.4556 μs |   1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    476.991 μs |    49.899 μs |   2.7352 μs |  36.1328 |  30.2734 |  30.2734 |   740.16 KB |
| HashingLoadUndeclared  |      5.122 μs |     4.156 μs |   0.2278 μs |   5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     27.413 μs |     2.699 μs |   0.1479 μs |   7.4158 |   0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |    123.864 μs |    16.298 μs |   0.8933 μs |  16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    862.067 μs |   334.358 μs |  18.3273 μs |  38.0859 |  36.1328 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 10,881.495 μs |   506.912 μs |  27.7855 μs | 500.0000 | 484.3750 | 281.2500 | 10793.96 KB |
| CountSaveAsync1M       |  2,529.657 μs |   317.971 μs |  17.4291 μs | 132.8125 | 125.0000 | 125.0000 |  3162.55 KB |
| IndexSaveAsyncLongHead |  3,348.908 μs |   710.458 μs |  38.9426 μs |  97.6563 |  93.7500 |  93.7500 |  2124.19 KB |
| CountLoadUndeclared2M  | 22,046.043 μs | 6,374.508 μs | 349.4083 μs | 875.0000 | 843.7500 | 468.7500 | 22045.22 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.646 μs** |   **0.2217 μs** | **0.0122 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.813 μs |   0.1822 μs | 0.0100 μs |  1.03 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   5.932 μs |   0.4276 μs | 0.0234 μs |  1.05 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **57.746 μs** |   **3.6557 μs** | **0.2004 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.062 μs |   1.5711 μs | 0.0861 μs |  0.36 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.943 μs |   2.4477 μs | 0.1342 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **217.982 μs** |   **6.4672 μs** | **0.3545 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  79.794 μs |   0.4331 μs | 0.0237 μs |  0.37 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  70.687 μs |   6.3115 μs | 0.3460 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **875.540 μs** | **127.1299 μs** | **6.9684 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 319.523 μs |  22.2336 μs | 1.2187 μs |  0.36 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 270.599 μs |  24.7936 μs | 1.3590 μs |  0.31 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **133.49 ms** |  **6.856 ms** | **0.376 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.94 ms |  4.606 ms | 0.252 ms |  0.49 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **139.42 ms** | **72.462 ms** | **3.972 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  60.67 ms |  2.242 ms | 0.123 ms |  0.44 |    0.01 |  116.47 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **202.00 ms** |  **2.940 ms** | **0.161 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 242.96 ms |  7.001 ms | 0.384 ms |  1.20 |    0.00 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **205.55 ms** | **16.715 ms** | **0.916 ms** |  **1.00** |    **0.01** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 219.01 ms |  5.790 ms | 0.317 ms |  1.07 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **252.84 ms** |  **9.569 ms** | **0.525 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 343.56 ms | 19.193 ms | 1.052 ms |  1.36 |    0.00 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **258.25 ms** |  **3.259 ms** | **0.179 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 314.18 ms | 12.650 ms | 0.693 ms |  1.22 |    0.00 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **296.90 ms** |  **8.880 ms** | **0.487 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 399.10 ms | 27.395 ms | 1.502 ms |  1.34 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **302.07 ms** | **20.343 ms** | **1.115 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 391.37 ms | 29.679 ms | 1.627 ms |  1.30 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **44.74 μs** |     **0.095 μs** |   **0.005 μs** |         **-** |
| Cjk    | 1000   |      59.25 μs |     1.980 μs |   0.109 μs |         - |
| **Latin**  | **10000**  |   **4,139.74 μs** |   **507.277 μs** |  **27.806 μs** |         **-** |
| Cjk    | 10000  |   6,814.02 μs |   122.362 μs |   6.707 μs |         - |
| **Latin**  | **65536**  | **184,925.44 μs** | **8,056.895 μs** | **441.626 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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

| Method  | Mean     | Error    | StdDev   | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|----------:|----------:|------------:|
| Unigram | 27.99 ms | 0.516 ms | 0.028 ms |  1.00 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 82.86 ms | 1.397 ms | 0.077 ms |  2.96 | 1714.2857 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **18.56 μs** |  **7.735 μs** | **0.424 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **65.68 μs** | **16.659 μs** | **0.913 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **177.73 μs** | **15.241 μs** | **0.835 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **379.69 μs** | **30.702 μs** | **1.683 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 9.136 ms | 5.274 ms | 0.2891 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean      | Error     | StdDev   | Allocated |
|----------- |--------- |----------:|----------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **18.82 μs** |  **0.456 μs** | **0.025 μs** |         **-** |
| MyersGroup | cjk      | 242.07 μs | 70.501 μs | 3.864 μs |         - |
| **DpGroup**    | **latin**    |  **10.39 μs** |  **3.900 μs** | **0.214 μs** |         **-** |
| MyersGroup | latin    | 132.57 μs |  8.492 μs | 0.465 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **311.72 μs** |  **1.805 μs** | **0.099 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 304.72 μs | 47.467 μs | 2.602 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **336.89 μs** | **20.345 μs** | **1.115 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 306.79 μs | 18.551 μs | 1.017 μs |  0.91 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **317.34 μs** | **29.568 μs** | **1.621 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 311.73 μs | 66.073 μs | 3.622 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **269.35 μs** | **14.347 μs** | **0.786 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 320.81 μs | 61.353 μs | 3.363 μs |  1.19 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **298.84 μs** | **55.234 μs** | **3.028 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 307.35 μs | 49.847 μs | 2.732 μs |  1.03 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **164.72 μs** | **30.976 μs** | **1.698 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 305.11 μs |  7.302 μs | 0.400 μs |  1.85 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **231.28 μs** | **59.317 μs** | **3.251 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 312.52 μs | 52.517 μs | 2.879 μs |  1.35 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **86.66 μs** | **31.090 μs** | **1.704 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 317.51 μs | 16.260 μs | 0.891 μs |  3.66 |    0.06 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **183.6 μs** |   **6.94 μs** |  **0.38 μs** |  **1.00** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   168.1 μs |   6.93 μs |  0.38 μs |  0.92 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   471.6 μs |   9.75 μs |  0.53 μs |  2.57 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,240.8 μs** | **188.48 μs** | **10.33 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,108.5 μs | 132.69 μs |  7.27 μs |  0.96 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,531.5 μs | 116.14 μs |  6.37 μs |  2.32 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

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

| Method       | Shape     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **157.68 ms** | **33.241 ms** | **1.822 ms** |  **1.00** |    **0.01** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,522.94 ms |  9.343 ms | 0.512 ms | 16.00 |    0.16 | 14000.0000 | 8000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |           |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **57.36 ms** | **23.627 ms** | **1.295 ms** |  **1.00** |    **0.03** |  **1444.4444** | **1444.4444** | **1444.4444** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   840.44 ms | 38.829 ms | 2.128 ms | 14.66 |    0.29 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **177.96 ms** |  **60.593 ms** |  **3.321 ms** |  **1.00** |    **0.02** |  **1333.3333** |  **1333.3333** |  **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,729.97 ms | 205.330 ms | 11.255 ms | 37.83 |    0.61 | 38000.0000 | 12000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,937.89 ms | 146.696 ms |  8.041 ms | 22.13 |    0.36 | 22000.0000 | 12000.0000 | 10000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.04 ms** |   **1.248 ms** |  **0.068 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   476.14 ms |  40.486 ms |  2.219 ms | 22.63 |    0.11 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   263.40 ms |  20.405 ms |  1.118 ms | 12.52 |    0.06 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  12.847 ms |  1.0341 ms | 0.0567 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 108.139 ms | 27.8378 ms | 1.5259 ms |  8.42 |    0.11 |
| Nmf_Transform                             |   2.929 ms |  0.2823 ms | 0.0155 ms |  0.23 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.002 ms |  0.6804 ms | 0.0373 ms |  1.87 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.04 ns |   2.214 ns |  0.121 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   745.95 ns |  65.403 ns |  3.585 ns |  7.69 |    0.03 |      - |         - |          NA |
| TokenSortRatio |   862.70 ns |  48.985 ns |  2.685 ns |  8.89 |    0.03 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   991.08 ns | 201.501 ns | 11.045 ns | 10.21 |    0.10 | 0.0534 |     896 B |          NA |
| WRatio         | 1,259.56 ns |  57.904 ns |  3.174 ns | 12.98 |    0.03 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,024,127.1211 ns |   119,691.7868 ns |   6,560.7107 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    660,653.8698 ns |    37,207.4879 ns |   2,039.4680 ns |  0.326 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,091,535.6536 ns |    47,448.4116 ns |   2,600.8076 ns |  0.539 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,689,598.5000 ns | 7,936,006.8197 ns | 434,999.3144 ns | 34.430 |    0.21 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean         | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.49 ns** |     **7.271 ns** |  **0.399 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    205.22 ns |     5.977 ns |  0.328 ns |  2.08 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **761.03 ns** |    **47.707 ns** |  **2.615 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,059.43 ns |   403.389 ns | 22.111 ns | 13.22 |    0.05 |      - |     160 B |          NA |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **924.17 ns** |    **68.572 ns** |  **3.759 ns** |  **1.00** |    **0.00** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,097.95 ns | 1,039.258 ns | 56.965 ns |  2.27 |    0.05 | 0.1144 |    1944 B |        2.17 |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,190.53 ns** |   **237.343 ns** | **13.010 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,880.18 ns |   200.343 ns | 10.981 ns |  4.10 |    0.04 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.34 ns** |   **0.506 ns** |  **0.028 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.45 ns |   1.377 ns |  0.075 ns |  1.57 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.40 ns |   0.134 ns |  0.007 ns |  1.04 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.28 ns |   0.091 ns |  0.005 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.20 ns** |   **1.169 ns** |  **0.064 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    43.53 ns |  26.714 ns |  1.464 ns |  1.54 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.69 ns |   0.271 ns |  0.015 ns |  1.02 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.85 ns |   0.353 ns |  0.019 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **30.82 ns** |   **3.326 ns** |  **0.182 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.52 ns |   0.198 ns |  0.011 ns |  1.48 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    31.51 ns |   0.914 ns |  0.050 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.27 ns |   1.690 ns |  0.093 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **45.22 ns** |  **15.266 ns** |  **0.837 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    50.85 ns |  15.137 ns |  0.830 ns |  1.12 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.46 ns |   1.112 ns |  0.061 ns |  0.78 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    33.95 ns |   0.283 ns |  0.016 ns |  0.75 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **58.26 ns** |   **3.473 ns** |  **0.190 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.92 ns |   1.068 ns |  0.059 ns |  1.05 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.28 ns |   1.893 ns |  0.104 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    60.28 ns |   9.007 ns |  0.494 ns |  1.03 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **62.58 ns** |   **3.682 ns** |  **0.202 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    70.51 ns |   5.804 ns |  0.318 ns |  1.13 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    65.77 ns |   0.433 ns |  0.024 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    61.66 ns |   0.744 ns |  0.041 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **405.96 ns** |  **22.062 ns** |  **1.209 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   416.96 ns |   1.226 ns |  0.067 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   406.62 ns |   7.823 ns |  0.429 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   412.79 ns |  22.896 ns |  1.255 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,622.30 ns** | **332.710 ns** | **18.237 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,717.91 ns |  26.292 ns |  1.441 ns |  1.02 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,669.73 ns |  19.068 ns |  1.045 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,619.95 ns | 105.635 ns |  5.790 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **405.2 ns** |    **13.83 ns** |   **0.76 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    192.8 ns |    62.52 ns |   3.43 ns |  0.48 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,464.5 ns** |    **67.55 ns** |   **3.70 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,039.0 ns |    10.88 ns |   0.60 ns |  1.23 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,572.4 ns** |   **769.99 ns** |  **42.21 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,265.7 ns | 9,043.15 ns | 495.69 ns |  2.38 |    0.03 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,643.3 ns** |   **190.27 ns** |  **10.43 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  8,430.7 ns |   298.37 ns |  16.35 ns |  0.98 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,576.6 ns |   778.50 ns |  42.67 ns |  1.34 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,897.0 ns** | **4,382.71 ns** | **240.23 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,516.8 ns | 3,110.69 ns | 170.51 ns |  0.96 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,108.2 ns |   709.30 ns |  38.88 ns |  1.43 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,729.1 ns** | **1,821.92 ns** |  **99.87 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,676.1 ns | 1,057.44 ns |  57.96 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 17,909.2 ns | 2,028.72 ns | 111.20 ns |  1.41 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,382.7 ns** | **2,239.28 ns** | **122.74 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,921.7 ns | 2,674.41 ns | 146.59 ns |  1.10 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,172.2 ns | 1,034.85 ns |  56.72 ns |  1.70 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **6,930.8 ns** | **3,937.40 ns** | **215.82 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,932.8 ns | 1,368.57 ns |  75.02 ns |  1.00 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,153.4 ns | 1,391.94 ns |  76.30 ns |  1.61 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,169.0 ns** | **2,163.07 ns** | **118.57 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,644.1 ns | 1,809.56 ns |  99.19 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,174.3 ns | 3,154.53 ns | 172.91 ns |  1.66 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,754.4 ns** |   **422.63 ns** |  **23.17 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,093.4 ns |   188.36 ns |  10.32 ns |  1.09 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,164.1 ns |    33.06 ns |   1.81 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,340.0 ns** | **1,039.97 ns** |  **57.00 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,396.9 ns |   718.66 ns |  39.39 ns |  1.01 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,885.6 ns |   143.51 ns |   7.87 ns |  1.85 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,846.7 ns** | **2,042.43 ns** | **111.95 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,473.6 ns | 1,697.76 ns |  93.06 ns |  0.95 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,814.1 ns |   714.09 ns |  39.14 ns |  1.76 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,303.9 ns** |    **35.88 ns** |   **1.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,136.4 ns |    69.17 ns |   3.79 ns |  0.93 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,890.5 ns |   646.13 ns |  35.42 ns |  2.56 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,081.5 ns** |    **65.13 ns** |   **3.57 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,870.5 ns |   331.30 ns |  18.16 ns |  0.70 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,717.2 ns | 5,592.68 ns | 306.55 ns |  2.14 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,434.6 ns** |    **44.82 ns** |   **2.46 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,828.8 ns |   201.81 ns |  11.06 ns |  0.60 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,306.9 ns | 1,588.44 ns |  87.07 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,559.9 ns** |    **84.46 ns** |   **4.63 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,416.0 ns |   124.94 ns |   6.85 ns |  0.96 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,063.9 ns |   348.27 ns |  19.09 ns |  1.98 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,347.9 ns** |   **248.06 ns** |  **13.60 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,152.9 ns |   214.24 ns |  11.74 ns |  0.78 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,871.7 ns | 1,180.21 ns |  64.69 ns |  2.03 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,560.8 ns** |   **436.21 ns** |  **23.91 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,279.8 ns |   408.96 ns |  22.42 ns |  1.10 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,018.4 ns | 1,158.89 ns |  63.52 ns |  1.85 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,297.9 ns** |    **19.80 ns** |   **1.09 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    602.8 ns |   225.70 ns |  12.37 ns |  0.46 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,969.8 ns |   517.20 ns |  28.35 ns |  3.83 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,066.4 ns** |    **41.07 ns** |   **2.25 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,432.9 ns |   119.58 ns |   6.55 ns |  0.47 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,490.4 ns |   345.73 ns |  18.95 ns |  2.44 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,454.2 ns** |    **72.37 ns** |   **3.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,390.4 ns |   121.98 ns |   6.69 ns |  0.44 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,997.0 ns |   495.54 ns |  27.16 ns |  2.20 |    0.00 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

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

| Method           | Shape       | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,336.5 μs** |     **559.21 μs** |    **30.65 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.91 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,941.7 μs |     799.26 μs |    43.81 μs |  7.30 |    0.04 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  82,977.3 μs |   2,734.30 μs |   149.88 μs | 15.55 |    0.08 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **798.2 μs** |      **71.05 μs** |     **3.89 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,766.5 μs |      34.62 μs |     1.90 μs |  5.97 |    0.03 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,041.6 μs |     142.83 μs |     7.83 μs |  3.81 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,859.4 μs** |   **1,144.60 μs** |    **62.74 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 443,866.9 μs | 116,764.53 μs | 6,400.26 μs | 16.53 |    0.21 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 734,574.3 μs |   8,247.68 μs |   452.08 μs | 27.35 |    0.06 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.900 ms** | **0.5552 ms** | **0.0304 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.656 ms | 0.2677 ms | 0.0147 ms |  3.40 |    0.02 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **25.666 ms** | **2.8793 ms** | **0.1578 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.211 ms | 1.9198 ms | 0.1052 ms |  4.41 |    0.02 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.189 ms** | **1.3412 ms** | **0.0735 ms** |  **1.00** |    **0.00** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 210.909 ms | 1.8022 ms | 0.0988 ms |  4.57 |    0.01 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.68 ns** |      **0.899 ns** |   **0.049 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     56.22 ns |      0.266 ns |   0.015 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    130.86 ns |      6.440 ns |   0.353 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     98.66 ns |     13.421 ns |   0.736 ns |  0.75 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **216.21 ns** |     **14.409 ns** |   **0.790 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     62.71 ns |      0.883 ns |   0.048 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    231.11 ns |    179.782 ns |   9.854 ns |  1.07 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    110.50 ns |      5.821 ns |   0.319 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **290.76 ns** |    **206.012 ns** |  **11.292 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 14   |     67.34 ns |      2.196 ns |   0.120 ns |  0.23 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    271.93 ns |    173.003 ns |   9.483 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 14   |    114.84 ns |      0.357 ns |   0.020 ns |  0.40 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **357.72 ns** |     **69.521 ns** |   **3.811 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |     79.87 ns |      3.156 ns |   0.173 ns |  0.22 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    360.14 ns |     56.434 ns |   3.093 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    119.32 ns |      6.416 ns |   0.352 ns |  0.33 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **443.08 ns** |    **164.054 ns** |   **8.992 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 18   |     73.79 ns |      1.681 ns |   0.092 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    444.97 ns |    145.161 ns |   7.957 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 18   |    123.82 ns |      9.956 ns |   0.546 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **763.06 ns** |    **293.969 ns** |  **16.113 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 20   |     79.85 ns |      0.904 ns |   0.050 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    772.93 ns |      7.363 ns |   0.404 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 20   |    127.86 ns |      8.241 ns |   0.452 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **990.15 ns** |    **192.110 ns** |  **10.530 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     86.43 ns |      5.700 ns |   0.312 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    987.78 ns |    105.550 ns |   5.786 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    142.07 ns |      2.126 ns |   0.117 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,530.99 ns** |    **142.968 ns** |   **7.837 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    105.93 ns |      2.790 ns |   0.153 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,529.09 ns |     67.476 ns |   3.699 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    167.30 ns |      0.498 ns |   0.027 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,133.27 ns** |    **945.865 ns** |  **51.846 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    133.43 ns |      7.264 ns |   0.398 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,146.57 ns |    922.000 ns |  50.538 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    241.87 ns |      5.305 ns |   0.291 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,203.00 ns** |    **639.832 ns** |  **35.071 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    164.31 ns |     28.738 ns |   1.575 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,478.77 ns |    337.934 ns |  18.523 ns |  1.05 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    291.42 ns |     12.142 ns |   0.666 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **12,220.97 ns** | **10,979.447 ns** | **601.821 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 96   |    353.01 ns |      1.554 ns |   0.085 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,508.44 ns |    523.844 ns |  28.714 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |    957.87 ns |    137.460 ns |   7.535 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **25.21 ns** |     **0.288 ns** |   **0.016 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.74 ns |     0.105 ns |   0.006 ns |  0.98 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    119.82 ns |     1.556 ns |   0.085 ns |  4.75 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     24.87 ns |     0.358 ns |   0.020 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **276.34 ns** |    **15.513 ns** |   **0.850 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    367.33 ns |     9.568 ns |   0.524 ns |  1.33 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    675.49 ns |     8.013 ns |   0.439 ns |  2.44 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    278.46 ns |    46.428 ns |   2.545 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **920.61 ns** |    **51.826 ns** |   **2.841 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,895.61 ns |   286.282 ns |  15.692 ns |  2.06 |    0.02 |         - |          NA |
| Distance_CodePoint         | 128    |  1,626.98 ns |    15.189 ns |   0.833 ns |  1.77 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    922.28 ns |    45.450 ns |   2.491 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **10,990.18 ns** |   **332.505 ns** |  **18.226 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,741.63 ns | 4,913.356 ns | 269.318 ns |  1.52 |    0.02 |         - |          NA |
| Distance_CodePoint         | 512    | 13,425.87 ns |   705.711 ns |  38.682 ns |  1.22 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,013.60 ns |    31.729 ns |   1.739 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **343.9 ns** |      **2.32 ns** |     **0.13 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     239.5 ns |     39.35 ns |     2.16 ns |  0.70 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **349.9 ns** |     **22.95 ns** |     **1.26 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     241.7 ns |      4.34 ns |     0.24 ns |  0.69 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **433.2 ns** |     **35.98 ns** |     **1.97 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.2 ns |     18.18 ns |     1.00 ns |  0.74 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **424.2 ns** |     **18.89 ns** |     **1.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     326.5 ns |      8.98 ns |     0.49 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **509.4 ns** |     **16.75 ns** |     **0.92 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     408.7 ns |     16.04 ns |     0.88 ns |  0.80 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **520.6 ns** |     **12.57 ns** |     **0.69 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     416.4 ns |      9.83 ns |     0.54 ns |  0.80 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **612.0 ns** |     **56.91 ns** |     **3.12 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,301.2 ns |    152.23 ns |     8.34 ns |  2.13 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **616.5 ns** |     **11.88 ns** |     **0.65 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,302.6 ns |    104.03 ns |     5.70 ns |  2.11 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,910.0 ns** |    **107.22 ns** |     **5.88 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,569.2 ns |    876.38 ns |    48.04 ns |  2.92 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,955.9 ns** |     **84.09 ns** |     **4.61 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,484.7 ns |    120.15 ns |     6.59 ns |  2.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,756.9 ns** |  **1,342.79 ns** |    **73.60 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,529.6 ns |  2,151.47 ns |   117.93 ns |  3.90 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **425,674.1 ns** | **84,819.72 ns** | **4,649.26 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  65,355.6 ns |  5,202.80 ns |   285.18 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.59 ns** |      **1.252 ns** |     **0.069 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      81.45 ns |     10.008 ns |     0.549 ns |  3.18 |    0.02 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.15 ns |      0.363 ns |     0.020 ns |  3.17 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     167.96 ns |     41.344 ns |     2.266 ns |  6.56 |    0.08 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.19 ns** |      **1.703 ns** |     **0.093 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,265.04 ns |    758.766 ns |    41.591 ns | 22.68 |    0.13 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,352.97 ns |     22.681 ns |     1.243 ns |  4.90 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,602.03 ns |  1,675.068 ns |    91.816 ns | 38.39 |    0.29 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,042.69 ns** |     **87.865 ns** |     **4.816 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 432,918.25 ns |  9,127.726 ns |   500.321 ns | 39.20 |    0.04 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  36,939.57 ns |  5,038.080 ns |   276.154 ns |  3.35 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 712,764.54 ns | 42,115.184 ns | 2,308.475 ns | 64.55 |    0.18 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

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

| Method            | Labels | Mean     | Error    | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|---------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **599.2 μs** | **45.18 μs** | **2.48 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 640.1 μs | 63.09 μs | 3.46 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 599.0 μs | 24.10 μs | 1.32 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **775.0 μs** | **23.60 μs** | **1.29 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 771.2 μs | 11.38 μs | 0.62 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 916.9 μs | 74.94 μs | 4.11 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

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

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.06 μs** |     **20.291 μs** |     **1.112 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 132,112.72 μs | 57,384.657 μs | 3,145.447 μs | 992.93 |   21.70 | 750.0000 | 750.0000 | 750.0000 | 31414.41 KB |   12,805.87 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,910.55 μs** |    **276.891 μs** |    **15.177 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 591,889.75 μs | 13,035.510 μs |   714.520 μs | 203.36 |    0.94 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.47 μs** |      **2.769 μs** |     **0.152 μs** |   **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     868.85 μs |     56.429 μs |     3.093 μs |  38.66 |    0.25 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

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

| Method         | Samples | Classes | Mean           | Error         | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|--------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,472.9 ns** |     **132.44 ns** |      **7.26 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,423.0 ns |     155.60 ns |      8.53 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       109.9 ns |      24.57 ns |      1.35 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,599.3 ns |     379.02 ns |     20.78 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,389.8 ns |      34.30 ns |      1.88 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,969.2 ns** |     **888.44 ns** |     **48.70 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,401.9 ns |     249.26 ns |     13.66 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       108.4 ns |      13.21 ns |      0.72 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,167.8 ns |   1,330.36 ns |     72.92 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,602.4 ns |   1,244.33 ns |     68.21 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **470,304.4 ns** |  **66,952.04 ns** |  **3,669.87 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   820,923.1 ns |  20,646.05 ns |  1,131.68 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,291.6 ns |     656.64 ns |     35.99 ns |      - |         - |
| F1Macro        | 100000  | 2       |   461,049.0 ns |   9,576.61 ns |    524.93 ns |      - |     656 B |
| Report         | 100000  | 2       |   464,497.8 ns |  15,826.85 ns |    867.52 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **493,893.4 ns** |  **16,097.62 ns** |    **882.36 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   937,911.7 ns |  27,787.13 ns |  1,523.11 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    11,064.1 ns |     288.62 ns |     15.82 ns |      - |         - |
| F1Macro        | 100000  | 10      |   511,606.6 ns |  39,009.09 ns |  2,138.22 ns |      - |    1881 B |
| Report         | 100000  | 10      |   501,716.4 ns |  20,077.42 ns |  1,100.51 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,782,135.3 ns** | **295,879.11 ns** | **16,218.13 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,197,178.0 ns | 586,833.23 ns | 32,166.31 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   121,041.4 ns |   6,988.31 ns |    383.05 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,624,993.0 ns | 236,093.66 ns | 12,941.09 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,791,481.2 ns | 501,878.73 ns | 27,509.67 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,941,134.0 ns** | **524,578.08 ns** | **28,753.89 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,432,637.5 ns | 377,141.51 ns | 20,672.40 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,852.9 ns |   9,891.70 ns |    542.20 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 5,076,182.1 ns |  56,493.01 ns |  3,096.57 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,061,718.6 ns |  47,164.79 ns |  2,585.26 ns |      - |   12878 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **9,865.54 μs** |    **829.525 μs** |    **45.469 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,254.71 μs |  1,726.505 μs |    94.636 μs |     3.47 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.16 μs** |      **3.428 μs** |     **0.188 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,699.06 μs |    477.558 μs |    26.177 μs | 3,109.69 |   44.95 | 600.0000 | 600.0000 | 600.0000 |  5089266 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **137,081.11 μs** | **38,328.599 μs** | **2,100.920 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 225,981.92 μs | 51,115.242 μs | 2,801.799 μs |     1.65 |    0.03 |        - |        - |        - | 23228104 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **110.13 μs** |      **7.709 μs** |     **0.423 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 249,811.88 μs |  9,347.541 μs |   512.370 μs | 2,268.27 |    8.53 |        - |        - |        - | 23229116 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **72.94 ns** |     **1.545 ns** |   **0.085 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.39 ns |     4.274 ns |   0.234 ns |  1.01 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     73.23 ns |     1.871 ns |   0.103 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     73.16 ns |     1.655 ns |   0.091 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **105.66 ns** |    **33.781 ns** |   **1.852 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 6    |     89.34 ns |     1.822 ns |   0.100 ns |  0.85 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    105.07 ns |     3.782 ns |   0.207 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    132.52 ns |    34.200 ns |   1.875 ns |  1.25 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **156.83 ns** |     **4.055 ns** |   **0.222 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.11 ns |     1.760 ns |   0.096 ns |  0.65 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.69 ns |    17.405 ns |   0.954 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    180.54 ns |     3.244 ns |   0.178 ns |  1.15 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **197.91 ns** |    **48.699 ns** |   **2.669 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 10   |    108.07 ns |     2.330 ns |   0.128 ns |  0.55 |    0.01 |         - |          NA |
| Dp_Cjk     | 10   |    196.45 ns |     2.029 ns |   0.111 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    153.68 ns |     2.462 ns |   0.135 ns |  0.78 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **253.61 ns** |   **217.199 ns** |  **11.905 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 12   |    117.58 ns |     1.562 ns |   0.086 ns |  0.46 |    0.02 |         - |          NA |
| Dp_Cjk     | 12   |    245.40 ns |     3.712 ns |   0.203 ns |  0.97 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    161.13 ns |     2.260 ns |   0.124 ns |  0.64 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **412.66 ns** |   **121.372 ns** |   **6.653 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 16   |    131.54 ns |     4.777 ns |   0.262 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    412.70 ns |    76.351 ns |   4.185 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 16   |    180.97 ns |    10.329 ns |   0.566 ns |  0.44 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **844.79 ns** |    **50.174 ns** |   **2.750 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    169.26 ns |     6.025 ns |   0.330 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    843.01 ns |    14.213 ns |   0.779 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    228.29 ns |     1.560 ns |   0.085 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,450.89 ns** |   **189.180 ns** |  **10.370 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    207.89 ns |    75.135 ns |   4.118 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,451.95 ns |    40.123 ns |   2.199 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    268.44 ns |     1.441 ns |   0.079 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,593.05 ns** | **2,493.081 ns** | **136.654 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 48   |    277.56 ns |    21.424 ns |   1.174 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,305.43 ns |    68.328 ns |   3.745 ns |  0.92 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    368.39 ns |     2.080 ns |   0.114 ns |  0.10 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,252.78 ns** | **1,191.209 ns** |  **65.294 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    345.76 ns |     7.306 ns |   0.400 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  7,032.77 ns | 2,722.731 ns | 149.242 ns |  1.12 |    0.02 |         - |          NA |
| Kernel_Cjk | 64   |    449.74 ns |     6.505 ns |   0.357 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,251.54 ns** | **7,942.876 ns** | **435.376 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 96   |    743.77 ns |    45.601 ns |   2.500 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,480.08 ns | 8,351.647 ns | 457.782 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |  1,545.71 ns |   412.925 ns |  22.634 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **206.13 μs** |  **10.539 μs** |  **0.578 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   227.50 μs |  13.494 μs |  0.740 μs |  1.10 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    58.62 μs |   1.722 μs |  0.094 μs |  0.28 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   198.15 μs |  13.330 μs |  0.731 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **207.00 μs** |  **14.029 μs** |  **0.769 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   264.94 μs |  11.310 μs |  0.620 μs |  1.28 | 4.3945 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    61.44 μs |   5.740 μs |  0.315 μs |  0.30 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   199.09 μs |  34.624 μs |  1.898 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,979.83 μs** |  **59.539 μs** |  **3.264 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,034.92 μs |  90.570 μs |  4.964 μs |  1.03 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,080.46 μs |   7.832 μs |  0.429 μs |  0.55 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,977.88 μs | 102.628 μs |  5.625 μs |  1.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,052.56 μs** |  **56.259 μs** |  **3.084 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,086.18 μs | 113.374 μs |  6.214 μs |  1.02 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,082.69 μs |  47.013 μs |  2.577 μs |  0.53 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,989.88 μs | 261.379 μs | 14.327 μs |  0.97 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.437 μs** |  **0.0499 μs** | **0.0027 μs** |         **-** |
| EqualLength | 65           |     2.975 μs |  0.0753 μs | 0.0041 μs |         - |
| **Embedded**    | **128**          |    **39.971 μs** |  **0.1467 μs** | **0.0080 μs** |         **-** |
| EqualLength | 128          |     5.333 μs |  0.2449 μs | 0.0134 μs |         - |
| **Embedded**    | **512**          | **1,356.766 μs** | **85.5242 μs** | **4.6879 μs** |         **-** |
| EqualLength | 512          |    44.554 μs |  1.0580 μs | 0.0580 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.859 ms |  1.8735 ms | 0.1027 ms | 109.3750 | 101.5625 |  31.2500 |  3711.64 KB |
| TokenizerJsonWordPiece | 11.389 ms |  1.0906 ms | 0.0598 ms | 187.5000 | 171.8750 |  46.8750 |  5852.31 KB |
| TokenizerJsonUnigram   | 12.790 ms |  0.7445 ms | 0.0408 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.087 ms |  2.1188 ms | 0.1161 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.559 ms |  0.8083 ms | 0.0443 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.313 ms |  1.0495 ms | 0.0575 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.972 ms |  0.9418 ms | 0.0516 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  8.025 ms |  2.1729 ms | 0.1191 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.434 ms | 11.9310 ms | 0.6540 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 73.197 ms |  2.8298 ms | 0.1551 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,193.54 μs** | **2,050.129 μs** | **112.375 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 14,903.93 μs |   803.039 μs |  44.017 μs |  1.62 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **133.01 μs** |     **1.188 μs** |   **0.065 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    180.48 μs |     6.860 μs |   0.376 μs |  1.36 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,837.38 μs** |   **262.196 μs** |  **14.372 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,768.31 μs | 1,980.059 μs | 108.534 μs |  0.98 |    0.03 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.80 μs** |     **8.212 μs** |   **0.450 μs** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.41 μs |     0.743 μs |   0.041 μs |  1.20 |    0.02 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.44 μs** |   **6.180 μs** |  **0.339 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    45.01 μs |   3.744 μs |  0.205 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.00 μs |   1.301 μs |  0.071 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   527.57 μs |  57.230 μs |  3.137 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **460.48 μs** |  **23.978 μs** |  **1.314 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   458.01 μs |  23.722 μs |  1.300 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,109.69 μs |  30.941 μs |  1.696 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,983.99 μs | 233.592 μs | 12.804 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

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

| Method                 | Mean            | Error           | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,056.7 ns |       177.24 ns |      9.72 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,305.9 ns |       136.44 ns |      7.48 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,073.9 ns |       299.15 ns |     16.40 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        438.8 ns |        41.21 ns |      2.26 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,713,283.5 ns | 1,017,034.76 ns | 55,747.11 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

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

| Method               | Documents | Mean     | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |---------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1.872 ms** | **0.2206 ms** | **0.0121 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.387 ms | 0.0924 ms | 0.0051 ms |  0.74 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.994 ms | 0.0767 ms | 0.0042 ms |  1.07 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.707 ms | 0.1451 ms | 0.0080 ms |  0.91 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **8.849 ms** | **0.2746 ms** | **0.0151 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.53 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.075 ms | 0.2018 ms | 0.0111 ms |  0.80 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.556 ms | 0.1343 ms | 0.0074 ms |  1.08 | 531.2500 | 515.6250 | 515.6250 | 2349.23 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.688 ms | 0.2021 ms | 0.0111 ms |  0.98 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **20.89 ms** |  **2.553 ms** | **0.140 ms** |  **1.00** |    **0.01** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.74 ms |  1.874 ms | 0.103 ms |  2.57 |    0.02 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **45.92 ms** |  **3.029 ms** | **0.166 ms** |  **1.00** |    **0.00** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  55.56 ms |  0.882 ms | 0.048 ms |  1.21 |    0.00 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **82.93 ms** |  **2.900 ms** | **0.159 ms** |  **1.00** |    **0.00** | **1714.2857** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 257.33 ms | 27.218 ms | 1.492 ms |  3.10 |    0.02 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **48.26 ns** | **5.224 ns** | **0.286 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  49.43 ns | 0.881 ns | 0.048 ns |  1.02 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **95.29 ns** | **3.078 ns** | **0.169 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  93.51 ns | 0.973 ns | 0.053 ns |  0.98 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **124.09 ns** | **5.627 ns** | **0.308 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 123.41 ns | 1.620 ns | 0.089 ns |  0.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

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

| Method   | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.721 ms** |   **0.3213 ms** | **0.0176 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  53.966 ms | 172.4447 ms | 9.4523 ms | 14.50 |    2.20 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **16.638 ms** |   **0.6472 ms** | **0.0355 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 377.202 ms | 160.7733 ms | 8.8125 ms | 22.67 |    0.46 | 76000.0000 | 76000.0000 | 76000.0000 | 324.65 MB |       40.04 |

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
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.173 | 0.035 | 0.20x | 0.173 | 0.035 | 0.20x |
| cdist_wratio_n50 | 2.256 | 2.452 | 1.09x | 2.256 | 2.452 | 1.09x |
| cdist_ratio_varied_n50 | 0.140 | 0.031 | 0.22x | 0.140 | 0.031 | 0.22x |
| cdist_ratio_varied_cutoff90_n50 | 0.056 | 0.031 | 0.56x | 0.056 | 0.031 | 0.56x |
| cdist_ratio_n200 | 2.862 | 0.438 | 0.15x | 2.893 | 0.438 | 0.15x |
| cdist_wratio_n200 | 34.389 | 37.065 | 1.08x | 34.389 | 37.064 | 1.08x |
| cdist_ratio_varied_n200 | 2.396 | 0.385 | 0.16x | 2.399 | 0.385 | 0.16x |
| cdist_ratio_varied_cutoff90_n200 | 1.008 | 0.385 | 0.38x | 1.010 | 0.385 | 0.38x |
| cdist_ratio_n500 | 17.086 | 2.654 | 0.16x | 17.101 | 2.654 | 0.16x |
| cdist_wratio_n500 | 207.766 | 225.825 | 1.09x | 207.903 | 225.816 | 1.09x |
| cdist_ratio_varied_n500 | 13.789 | 2.305 | 0.17x | 13.894 | 2.305 | 0.17x |
| cdist_ratio_varied_cutoff90_n500 | 5.556 | 2.305 | 0.41x | 5.670 | 2.305 | 0.41x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 118.0 | 28.5 | 4.13x C# faster |
| latin | 32 | 180.8 | 86.4 | 2.09x C# faster |
| latin | 128 | 465.0 | 423.4 | 1.10x C# faster |
| latin | 512 | 4463.0 | 4864.1 | 1.09x Py faster |
| cjk | 8 | 143.6 | 28.4 | 5.06x C# faster |
| cjk | 32 | 334.1 | 220.6 | 1.51x C# faster |
| cjk | 128 | 1910.9 | 1391.1 | 1.37x C# faster |
| cjk | 512 | 14832.3 | 8914.3 | 1.66x C# faster |

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
| latin | 8 | 148.4 | 18.3 | 8.11x C# faster |
| latin | 32 | 279.6 | 159.0 | 1.76x C# faster |
| latin | 128 | 1693.3 | 904.1 | 1.87x C# faster |
| latin | 512 | 14055.3 | 11723.2 | 1.20x C# faster |
| cjk | 8 | 159.7 | 18.0 | 8.86x C# faster |
| cjk | 32 | 387.2 | 289.4 | 1.34x C# faster |
| cjk | 128 | 2846.3 | 2031.9 | 1.40x C# faster |
| cjk | 512 | 23615.6 | 17010.2 | 1.39x C# faster |

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
| confusion_matrix_n1000_k2 | 0.004 | 0.961 | 214.15x | 0.004 | 0.961 | 214.14x |
| accuracy_n1000_k2 | 0.000 | 0.514 | 3866.88x | 0.000 | 0.514 | 3866.07x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.778 | 348.38x | 0.005 | 1.778 | 348.39x |
| classification_report_n1000_k2 | 0.007 | 6.840 | 932.43x | 0.007 | 6.839 | 932.39x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.877 | 107.60x | 0.017 | 1.877 | 107.50x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.024 | 224.70x | 0.005 | 1.024 | 224.66x |
| matthews_n1000_k2 | 0.005 | 1.992 | 439.45x | 0.005 | 1.991 | 439.35x |
| cohen_kappa_n1000_k2 | 0.005 | 1.075 | 238.52x | 0.005 | 1.075 | 238.53x |
| mse_n1000_k2 | 0.000 | 0.306 | 623.36x | 0.000 | 0.306 | 623.37x |
| mae_n1000_k2 | 0.000 | 0.306 | 632.06x | 0.000 | 0.306 | 631.97x |
| median_ae_n1000_k2 | 0.005 | 0.318 | 65.38x | 0.005 | 0.318 | 65.38x |
| r2_n1000_k2 | 0.001 | 0.372 | 312.13x | 0.001 | 0.372 | 312.12x |
| confusion_matrix_n1000_k10 | 0.005 | 0.983 | 195.74x | 0.005 | 0.983 | 195.75x |
| accuracy_n1000_k10 | 0.000 | 0.512 | 3815.94x | 0.000 | 0.512 | 3816.00x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.794 | 307.73x | 0.006 | 1.793 | 307.73x |
| classification_report_n1000_k10 | 0.011 | 6.939 | 606.28x | 0.011 | 6.938 | 606.24x |
| roc_auc_ovr_macro_n1000_k10 | 0.523 | 9.626 | 18.40x | 0.523 | 9.626 | 18.40x |
| balanced_accuracy_n1000_k10 | 0.005 | 1.034 | 195.18x | 0.005 | 1.034 | 195.17x |
| matthews_n1000_k10 | 0.005 | 1.993 | 388.20x | 0.005 | 1.993 | 388.20x |
| cohen_kappa_n1000_k10 | 0.006 | 1.079 | 186.60x | 0.006 | 1.079 | 186.60x |
| mse_n1000_k10 | 0.000 | 0.299 | 610.43x | 0.000 | 0.299 | 610.42x |
| mae_n1000_k10 | 0.000 | 0.302 | 622.20x | 0.000 | 0.302 | 622.20x |
| median_ae_n1000_k10 | 0.005 | 0.315 | 64.33x | 0.005 | 0.315 | 64.33x |
| r2_n1000_k10 | 0.001 | 0.367 | 307.49x | 0.001 | 0.367 | 307.48x |
| confusion_matrix_n100000_k2 | 0.473 | 10.716 | 22.67x | 0.473 | 10.716 | 22.67x |
| accuracy_n100000_k2 | 0.012 | 3.763 | 306.68x | 0.012 | 3.763 | 306.67x |
| precision_recall_f1_macro_n100000_k2 | 0.476 | 12.320 | 25.86x | 0.476 | 12.319 | 25.86x |
| classification_report_n100000_k2 | 0.480 | 27.093 | 56.46x | 0.480 | 27.092 | 56.46x |
| roc_auc_binary_n100000_k2 | 4.350 | 26.584 | 6.11x | 4.350 | 26.583 | 6.11x |
| balanced_accuracy_n100000_k2 | 0.471 | 10.806 | 22.93x | 0.471 | 10.806 | 22.92x |
| matthews_n100000_k2 | 0.475 | 21.556 | 45.41x | 0.475 | 21.554 | 45.41x |
| cohen_kappa_n100000_k2 | 0.462 | 10.876 | 23.55x | 0.462 | 10.875 | 23.55x |
| mse_n100000_k2 | 0.045 | 0.446 | 9.80x | 0.045 | 0.446 | 9.80x |
| mae_n100000_k2 | 0.045 | 0.437 | 9.70x | 0.045 | 0.437 | 9.70x |
| median_ae_n100000_k2 | 0.600 | 1.801 | 3.00x | 0.625 | 1.801 | 2.88x |
| r2_n100000_k2 | 0.110 | 0.664 | 6.02x | 0.110 | 0.664 | 6.02x |
| confusion_matrix_n100000_k10 | 0.509 | 10.741 | 21.09x | 0.509 | 10.737 | 21.08x |
| accuracy_n100000_k10 | 0.012 | 3.776 | 308.07x | 0.012 | 3.776 | 308.07x |
| precision_recall_f1_macro_n100000_k10 | 0.509 | 12.967 | 25.47x | 0.509 | 12.967 | 25.47x |
| classification_report_n100000_k10 | 0.516 | 29.658 | 57.47x | 0.516 | 29.657 | 57.47x |
| roc_auc_ovr_macro_n100000_k10 | 34.534 | 215.631 | 6.24x | 34.532 | 215.621 | 6.24x |
| balanced_accuracy_n100000_k10 | 0.509 | 10.811 | 21.22x | 0.509 | 10.810 | 21.22x |
| matthews_n100000_k10 | 0.509 | 22.247 | 43.74x | 0.509 | 22.246 | 43.74x |
| cohen_kappa_n100000_k10 | 0.510 | 10.868 | 21.32x | 0.510 | 10.868 | 21.32x |
| mse_n100000_k10 | 0.046 | 0.448 | 9.81x | 0.046 | 0.448 | 9.81x |
| mae_n100000_k10 | 0.045 | 0.438 | 9.75x | 0.045 | 0.438 | 9.75x |
| median_ae_n100000_k10 | 0.599 | 1.792 | 2.99x | 0.643 | 1.791 | 2.79x |
| r2_n100000_k10 | 0.110 | 0.663 | 6.00x | 0.110 | 0.663 | 6.00x |
| confusion_matrix_n1000000_k2 | 4.763 | 100.103 | 21.02x | 4.763 | 100.098 | 21.02x |
| accuracy_n1000000_k2 | 0.122 | 33.148 | 272.18x | 0.122 | 33.147 | 272.18x |
| precision_recall_f1_macro_n1000000_k2 | 4.761 | 107.928 | 22.67x | 4.761 | 107.921 | 22.67x |
| classification_report_n1000000_k2 | 4.763 | 210.162 | 44.12x | 4.763 | 210.140 | 44.12x |
| roc_auc_binary_n1000000_k2 | 56.143 | 289.651 | 5.16x | 56.140 | 289.643 | 5.16x |
| balanced_accuracy_n1000000_k2 | 4.762 | 99.921 | 20.98x | 4.762 | 99.915 | 20.98x |
| matthews_n1000000_k2 | 4.767 | 200.199 | 41.99x | 4.767 | 200.179 | 41.99x |
| cohen_kappa_n1000000_k2 | 4.766 | 99.846 | 20.95x | 4.766 | 99.834 | 20.95x |
| mse_n1000000_k2 | 0.447 | 2.130 | 4.76x | 0.447 | 2.130 | 4.76x |
| mae_n1000000_k2 | 0.451 | 2.066 | 4.58x | 0.451 | 2.066 | 4.58x |
| median_ae_n1000000_k2 | 6.199 | 14.207 | 2.29x | 6.410 | 14.207 | 2.22x |
| r2_n1000000_k2 | 1.109 | 3.483 | 3.14x | 1.109 | 3.483 | 3.14x |
| confusion_matrix_n1000000_k10 | 5.014 | 100.247 | 19.99x | 5.014 | 100.237 | 19.99x |
| accuracy_n1000000_k10 | 0.121 | 33.134 | 273.50x | 0.121 | 33.133 | 273.50x |
| precision_recall_f1_macro_n1000000_k10 | 5.021 | 114.498 | 22.81x | 5.020 | 114.487 | 22.80x |
| classification_report_n1000000_k10 | 5.036 | 234.275 | 46.52x | 5.036 | 234.265 | 46.52x |
| balanced_accuracy_n1000000_k10 | 5.020 | 100.386 | 20.00x | 5.020 | 100.382 | 20.00x |
| matthews_n1000000_k10 | 5.019 | 207.162 | 41.27x | 5.019 | 207.144 | 41.27x |
| cohen_kappa_n1000000_k10 | 5.034 | 100.499 | 19.96x | 5.034 | 100.489 | 19.96x |
| mse_n1000000_k10 | 0.459 | 2.117 | 4.61x | 0.459 | 2.116 | 4.61x |
| mae_n1000000_k10 | 0.453 | 2.090 | 4.62x | 0.453 | 2.090 | 4.62x |
| median_ae_n1000000_k10 | 6.439 | 14.158 | 2.20x | 6.720 | 14.157 | 2.11x |
| r2_n1000000_k10 | 1.110 | 3.528 | 3.18x | 1.110 | 3.528 | 3.18x |

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
| vocab_txt | 5.236 | 9.774 | 1.87x | 5.516 | 9.774 | 1.77x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.138 | 16.400 | 1.35x | 12.540 | 16.399 | 1.31x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.576 | 38.602 | 2.84x | 13.936 | 38.600 | 2.77x | 1,990,038 | 1,990,038 |
| spiece_model | 7.519 | 28.471 | 3.79x | 7.813 | 28.470 | 3.64x | 533,084 | 533,084 |
| tfidf_save | 1.520 | 2.365 | 1.56x | 1.543 | 2.365 | 1.53x | 581,787 | 591,922 |
| tfidf_load | 4.759 | 4.094 | 0.86x | 4.924 | 4.094 | 0.83x | 581,787 | 591,922 |
| embedding_index_save | 4.613 | 1.611 | 0.35x | 4.794 | 1.611 | 0.34x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.489 | 34.164 | 0.69x | 10.493 | 4.701 | 0.45x | 20,589,007 | 15,360,128 |
| embedding_index_load | 8.190 | 1.444 | 0.18x | 8.730 | 1.439 | 0.16x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 8.213 | 0.881 | 0.11x | 8.714 | 0.881 | 0.10x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 6.853 | 1.380 | 0.20x | 7.217 | 1.380 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 3.215 | 1.385 | 0.43x | 3.375 | 1.385 | 0.41x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 93.48x | 0.000 | 0.001 | 93.48x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 411.288 | 566.817 | 1.38x | 413.618 | 566.778 | 1.37x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 70.840 | 66.523 | 0.94x | 70.965 | 66.517 | 0.94x | 15,250,490 | 14,022,374 |

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
| kfold_n10000 | 0.089 | 0.115 | 1.29x | 0.089 | 0.115 | 1.29x |
| stratified_n10000 | 0.179 | 0.884 | 4.95x | 0.179 | 0.884 | 4.95x |
| traintest_n10000 | 0.004 | 0.233 | 54.18x | 0.004 | 0.233 | 54.18x |
| kfold_seeded_n10000 | 0.308 | 0.672 | 2.18x | 0.308 | 0.672 | 2.18x |
| stratified_seeded_n10000 | 0.430 | 1.471 | 3.42x | 0.430 | 1.471 | 3.42x |
| traintest_seeded_n10000 | 0.151 | 0.518 | 3.43x | 0.151 | 0.518 | 3.43x |
| stratified_traintest_n10000 | 0.658 | 2.791 | 4.24x | 0.658 | 2.791 | 4.24x |
| group_kfold_n10000 | 0.128 | 0.594 | 4.64x | 0.128 | 0.594 | 4.64x |
| stratified_group_kfold_n10000 | 0.235 | 31.592 | 134.55x | 0.235 | 31.592 | 134.55x |
| timeseries_n10000 | 0.000 | 0.012 | 97.54x | 0.000 | 0.012 | 97.54x |
| repeated_kfold_n10000 | 0.959 | 1.790 | 1.87x | 0.959 | 1.790 | 1.87x |
| kfold_n100000 | 1.140 | 2.712 | 2.38x | 1.293 | 2.712 | 2.10x |
| stratified_n100000 | 2.182 | 7.261 | 3.33x | 2.344 | 7.260 | 3.10x |
| traintest_n100000 | 0.223 | 0.400 | 1.79x | 0.224 | 0.400 | 1.78x |
| kfold_seeded_n100000 | 3.559 | 7.038 | 1.98x | 3.732 | 7.038 | 1.89x |
| stratified_seeded_n100000 | 4.753 | 11.478 | 2.41x | 4.986 | 11.478 | 2.30x |
| traintest_seeded_n100000 | 1.981 | 2.006 | 1.01x | 2.026 | 2.006 | 0.99x |
| stratified_traintest_n100000 | 7.957 | 25.254 | 3.17x | 8.056 | 25.253 | 3.13x |
| group_kfold_n100000 | 1.614 | 6.799 | 4.21x | 1.767 | 6.799 | 3.85x |
| stratified_group_kfold_n100000 | 2.763 | 319.468 | 115.63x | 2.930 | 319.447 | 109.02x |
| timeseries_n100000 | 0.000 | 0.042 | 350.87x | 0.000 | 0.042 | 350.87x |
| repeated_kfold_n100000 | 10.099 | 19.350 | 1.92x | 10.290 | 19.349 | 1.88x |
| kfold_n1000000 | 11.528 | 17.806 | 1.54x | 12.554 | 17.804 | 1.42x |
| stratified_n1000000 | 23.932 | 68.535 | 2.86x | 25.412 | 68.531 | 2.70x |
| traintest_n1000000 | 0.768 | 2.051 | 2.67x | 0.901 | 2.051 | 2.28x |
| kfold_seeded_n1000000 | 37.898 | 58.616 | 1.55x | 38.751 | 58.610 | 1.51x |
| stratified_seeded_n1000000 | 51.596 | 107.986 | 2.09x | 53.727 | 107.979 | 2.01x |
| traintest_seeded_n1000000 | 21.623 | 17.208 | 0.80x | 22.243 | 17.204 | 0.77x |
| stratified_traintest_n1000000 | 92.862 | 233.760 | 2.52x | 93.555 | 233.747 | 2.50x |
| group_kfold_n1000000 | 14.935 | 51.616 | 3.46x | 15.682 | 51.611 | 3.29x |
| stratified_group_kfold_n1000000 | 28.846 | 3178.055 | 110.17x | 30.021 | 3177.892 | 105.86x |
| timeseries_n1000000 | 0.000 | 0.328 | 3176.40x | 0.000 | 0.328 | 3176.44x |
| repeated_kfold_n1000000 | 112.875 | 174.363 | 1.54x | 115.279 | 174.356 | 1.51x |

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
| welch_t_n1000 | 0.008 | 0.822 | 102.70x | 0.008 | 0.822 | 102.68x |
| mann_whitney_n1000 | 0.030 | 0.748 | 24.91x | 0.030 | 0.748 | 24.91x |
| chi_square_n1000 | 0.000 | 0.339 | 1734.70x | 0.000 | 0.339 | 1734.71x |
| fligner_n1000 | 0.074 | 0.806 | 10.87x | 0.074 | 0.806 | 10.87x |
| anderson_ksamp_n1000 | 0.163 | 0.396 | 2.44x | 0.163 | 0.396 | 2.44x |
| spearman_matrix_n1000 | 0.106 | 0.920 | 8.68x | 0.106 | 0.920 | 8.68x |
| pointbiserial_n1000 | 0.010 | 0.571 | 54.55x | 0.010 | 0.571 | 54.55x |
| welch_t_n10000 | 0.053 | 0.869 | 16.27x | 0.053 | 0.869 | 16.27x |
| mann_whitney_n10000 | 0.743 | 2.824 | 3.80x | 0.743 | 2.824 | 3.80x |
| chi_square_n10000 | 0.001 | 0.344 | 250.74x | 0.001 | 0.344 | 250.69x |
| fligner_n10000 | 1.404 | 3.224 | 2.30x | 1.487 | 3.224 | 2.17x |
| anderson_ksamp_n10000 | 2.020 | 3.983 | 1.97x | 2.141 | 3.983 | 1.86x |
| spearman_matrix_n10000 | 2.078 | 4.755 | 2.29x | 2.134 | 4.755 | 2.23x |
| pointbiserial_n10000 | 0.100 | 0.630 | 6.28x | 0.100 | 0.630 | 6.28x |
| welch_t_n100000 | 0.519 | 1.234 | 2.38x | 0.519 | 1.234 | 2.38x |
| mann_whitney_n100000 | 7.773 | 28.102 | 3.62x | 7.773 | 28.101 | 3.62x |
| chi_square_n100000 | 0.014 | 0.364 | 26.33x | 0.014 | 0.364 | 26.33x |
| fligner_n100000 | 14.895 | 32.276 | 2.17x | 15.335 | 32.275 | 2.10x |
| anderson_ksamp_n100000 | 15.589 | 43.489 | 2.79x | 15.947 | 43.486 | 2.73x |
| spearman_matrix_n100000 | 18.279 | 51.938 | 2.84x | 18.797 | 51.936 | 2.76x |
| pointbiserial_n100000 | 1.301 | 1.400 | 1.08x | 1.314 | 5.589 | 4.25x |

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
| normalize_n10000 | 0.277 | 0.455 | 1.64x | 0.278 | 0.455 | 1.64x |
| polynomial_n10000 | 0.781 | 1.025 | 1.31x | 0.785 | 1.024 | 1.30x |
| kbins_fit_n10000 | 1.461 | 2.598 | 1.78x | 1.461 | 2.598 | 1.78x |
| kbins_n10000 | 0.718 | 4.639 | 6.46x | 0.822 | 4.639 | 5.64x |
| quantile_fit_n10000 | 1.556 | 3.779 | 2.43x | 1.556 | 3.779 | 2.43x |
| quantile_n10000 | 1.840 | 1.700 | 0.92x | 1.843 | 1.700 | 0.92x |
| power_fit_n10000 | 6.091 | 134.902 | 22.15x | 6.094 | 134.898 | 22.14x |
| power_n10000 | 1.549 | 1.504 | 0.97x | 1.552 | 1.504 | 0.97x |
| knn_impute_n10000 | 7.445 | 23.580 | 3.17x | 7.444 | 94.080 | 12.64x |
| label_n10000 | 1.169 | 0.760 | 0.65x | 1.169 | 0.759 | 0.65x |
| normalize_n100000 | 1.538 | 1.846 | 1.20x | 1.957 | 1.846 | 0.94x |
| polynomial_n100000 | 3.732 | 6.974 | 1.87x | 4.569 | 6.974 | 1.53x |
| kbins_fit_n100000 | 14.690 | 8.178 | 0.56x | 15.509 | 8.178 | 0.53x |
| kbins_n100000 | 7.823 | 12.223 | 1.56x | 8.813 | 12.221 | 1.39x |
| quantile_fit_n100000 | 14.992 | 18.083 | 1.21x | 15.751 | 18.082 | 1.15x |
| quantile_n100000 | 17.260 | 14.690 | 0.85x | 17.789 | 14.689 | 0.83x |
| power_fit_n100000 | 65.080 | 887.616 | 13.64x | 67.884 | 887.553 | 13.07x |
| power_n100000 | 14.163 | 9.870 | 0.70x | 14.620 | 9.869 | 0.68x |
| knn_impute_n100000 | 7.444 | 18.717 | 2.51x | 7.444 | 74.756 | 10.04x |
| label_n100000 | 15.178 | 7.272 | 0.48x | 15.504 | 7.272 | 0.47x |

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
- `Bm25Benchmarks`
- `ChainedProductBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `ClusteringAgreementBenchmarks`
- `CosineTopKSelectionBenchmarks`
- `CoxBenchmarks`
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
- `SimilaritySketchBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `SurvivalBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TokenPatternBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorStoreChurnBenchmarks`
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

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 11.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | step | 0.02 | 0.11 | -82% | 33% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | step | 0.33 | 1.54 | -79% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | step | 0.1 | 0.45 | -78% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | step | 0.87 | 3.9 | -78% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_Wilcoxon` | step | 0.25 | 1.12 | -78% | 36% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_ChiSquare` | step | 0.15 | 0.67 | -78% | 33% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | step | 0.83 | 3.69 | -78% | 32% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | step | 1.62 | 7.09 | -77% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | step | 0.23 | 0.99 | -77% | 52% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | step | 2.21 | 9.1 | -76% | 39% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x10 | `MetaNumerics_VarianceFraction` | step | 992.93 | 747.24 | +33% | 30% |

**Still away from their median: 4.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | step | 0.68 | 3.15 | -78% | 64% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | step | 0.48 | 2.17 | -78% | 43% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | step | 0.54 | 2.39 | -77% | 34% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | step | 0.36 | 0.92 | -61% | 34% |
