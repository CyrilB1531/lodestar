# Nightly benchmark run

<!-- nightly-baseline: 6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SimilaritySketchBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorMathBenchmarks VectorStoreChurnBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks cluster-weighted cox-extended cross-conformal glm iv ols onehot panel survival-aalen survival-family survival-parametric var -->
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

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **964.6 ns** |         **12.26 ns** |       **0.67 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         800.8 ns |         14.32 ns |       0.79 ns |     0.83 |    0.00 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,761.5 ns |        811.99 ns |      44.51 ns |     2.86 |    0.04 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,938.1 ns |        222.11 ns |      12.17 ns |     4.08 |    0.01 |   0.1144 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,312.7 ns |         45.29 ns |       2.48 ns |     5.51 |    0.00 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,642.8 ns |      3,325.77 ns |     182.30 ns |    11.03 |    0.16 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,308.8 ns |          2.21 ns |       0.12 ns |     5.50 |    0.00 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,248.7 ns** |      **3,017.57 ns** |     **165.40 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      57,285.7 ns |     12,086.48 ns |     662.50 ns |     0.66 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,211,364.8 ns |    406,983.59 ns |  22,308.14 ns |    14.05 |    0.23 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,944,398.0 ns |     53,596.49 ns |   2,937.80 ns |    22.54 |    0.05 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,133,363.9 ns |     19,572.09 ns |   1,072.81 ns |    24.74 |    0.04 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,143,160.7 ns | 12,498,236.42 ns | 685,070.51 ns | 3,340.85 |    8.84 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,140,558.9 ns |      5,375.50 ns |     294.65 ns |    24.82 |    0.04 |        - |        - |        - |         - |          NA |

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

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.93 μs** |  **24.050 μs** |  **1.318 μs** |  **1.00** |    **0.05** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    35.25 μs |   2.283 μs |  0.125 μs |  1.11 |    0.04 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,002.65 μs** |  **47.256 μs** |  **2.590 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,467.98 μs | 373.339 μs | 20.464 μs |  0.73 |    0.01 | 7.8125 |  156.3 KB |        1.00 |

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
| **Lodestar_StudentT**              | **100**        |     **1,033.3 ns** |      **27.59 ns** |      **1.51 ns** |   **1.00** |    **0.00** | **0.0038** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,221.3 ns |     545.24 ns |     29.89 ns |   2.15 |    0.03 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,261.0 ns |     128.39 ns |      7.04 ns |   2.19 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,906.7 ns |     224.94 ns |     12.33 ns |   3.78 |    0.01 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,330.4 ns |     436.39 ns |     23.92 ns |   7.09 |    0.02 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,698.2 ns |   3,849.77 ns |    211.02 ns |   9.39 |    0.18 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,519.6 ns |     101.83 ns |      5.58 ns |   2.44 |    0.01 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,028.4 ns |     154.73 ns |      8.48 ns |   3.90 |    0.01 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |       966.8 ns |     155.27 ns |      8.51 ns |   0.94 |    0.01 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,162.4 ns |      17.97 ns |      0.98 ns |   3.06 |    0.00 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,067.7 ns |      77.74 ns |      4.26 ns |   1.03 |    0.00 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,030.3 ns |      36.47 ns |      2.00 ns |   1.00 |    0.00 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       461.2 ns |      39.62 ns |      2.17 ns |   0.45 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,563.6 ns |      98.05 ns |      5.37 ns |   1.51 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       117.8 ns |      20.32 ns |      1.11 ns |   0.11 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       692.1 ns |     130.36 ns |      7.15 ns |   0.67 |    0.01 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,328.8 ns** |     **568.42 ns** |     **31.16 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,846.2 ns |   2,340.85 ns |    128.31 ns |  2.965 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   687,993.5 ns |  14,975.72 ns |    820.87 ns | 11.404 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,078,287.3 ns | 104,967.24 ns |  5,753.61 ns | 34.449 |    0.08 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,468,022.5 ns |  50,106.04 ns |  2,746.48 ns | 24.334 |    0.04 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,644,554.0 ns | 557,930.43 ns | 30,582.05 ns | 60.411 |    0.44 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,471,655.4 ns | 399,533.35 ns | 21,899.77 ns | 24.394 |    0.31 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,141,357.6 ns |  82,690.91 ns |  4,532.57 ns | 35.495 |    0.07 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    85,628.7 ns |  32,744.54 ns |  1,794.84 ns |  1.419 |    0.03 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   269,799.8 ns |  36,571.12 ns |  2,004.59 ns |  4.472 |    0.03 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   186,929.7 ns |   8,070.56 ns |    442.37 ns |  3.099 |    0.01 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   379,505.7 ns |  12,085.25 ns |    662.43 ns |  6.291 |    0.01 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       459.4 ns |      23.85 ns |      1.31 ns |  0.008 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,546.1 ns |     210.45 ns |     11.54 ns |  0.026 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       116.7 ns |       9.12 ns |      0.50 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       684.5 ns |      52.96 ns |      2.90 ns |  0.011 |    0.00 | 0.0572 |     968 B |       15.12 |

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

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,128.9 ns** |     **119.71 ns** |     **6.56 ns** |  **1.00** |    **0.00** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,186.3 ns |     424.73 ns |    23.28 ns |  1.97 |    0.01 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       602.6 ns |      39.98 ns |     2.19 ns |  0.28 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,161.1 ns |      22.08 ns |     1.21 ns |  0.55 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       759.0 ns |      74.15 ns |     4.06 ns |  0.36 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,705.0 ns |     969.78 ns |    53.16 ns |  2.21 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,594.8 ns |     375.86 ns |    20.60 ns |  4.04 |    0.01 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,355.4 ns |     537.47 ns |    29.46 ns |  2.99 |    0.01 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,087.5 ns |     226.61 ns |    12.42 ns |  1.92 |    0.01 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **383,900.3 ns** |  **29,159.37 ns** | **1,598.32 ns** | **1.000** |    **0.01** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,349,935.8 ns | 123,685.81 ns | 6,779.64 ns | 3.516 |    0.02 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,844.7 ns |  16,288.03 ns |   892.80 ns | 0.148 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,640.3 ns |   3,224.74 ns |   176.76 ns | 0.293 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       991.9 ns |     120.93 ns |     6.63 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   919,043.8 ns |  71,419.39 ns | 3,914.74 ns | 2.394 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,395,357.1 ns |  32,994.15 ns | 1,808.52 ns | 3.635 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,712.7 ns |     775.95 ns |    42.53 ns | 0.028 |    0.00 |       - |      - |      48 B |       0.000 |

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
| **Lodestar_Fit**           | **500**  | **average**  |     **2,011.0 μs** |     **718.75 μs** |     **39.40 μs** |   **1.00** |    **0.02** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   101,740.6 μs |   6,068.70 μs |    332.65 μs |  50.61 |    0.87 |  3666.6667 |  1500.0000 |  833.3333 |  59880.82 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,888.0 μs** |      **66.25 μs** |      **3.63 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    93,061.3 μs |  24,151.90 μs |  1,323.85 μs |  49.29 |    0.61 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **773.0 μs** |      **49.62 μs** |      **2.72 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   138,407.8 μs |  95,687.14 μs |  5,244.93 μs | 179.06 |    5.90 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,272.3 μs** |      **94.66 μs** |      **5.19 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    70,493.5 μs |   8,232.07 μs |    451.23 μs |  31.02 |    0.18 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,578.2 μs** |   **2,446.76 μs** |    **134.12 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,621,925.6 μs | 736,052.94 μs | 40,345.55 μs | 104.12 |    2.37 | 35000.0000 | 10000.0000 | 4000.0000 | 537149.55 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,917.1 μs** |  **15,955.43 μs** |    **874.57 μs** |   **1.00** |    **0.07** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,724,902.8 μs | 321,922.83 μs | 17,645.68 μs | 115.89 |    5.81 | 48000.0000 | 12000.0000 | 4000.0000 | 747353.12 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,635.5 μs** |     **301.32 μs** |     **16.52 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,095,108.5 μs | 340,386.45 μs | 18,657.73 μs | 466.45 |    2.63 | 47000.0000 | 12000.0000 | 4000.0000 | 732672.84 KB |    8,147.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,544.8 μs** |   **1,161.32 μs** |     **63.66 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,620,159.5 μs | 253,275.29 μs | 13,882.87 μs |  92.34 |    0.74 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

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
| CountLoadAsync2M | 26.12 ms |  3.781 ms | 0.207 ms |  718.7500 |  656.2500 | 312.5000 |  21.53 MB |
| CountLoadAsync4M | 47.92 ms | 17.652 ms | 0.968 ms | 1454.5455 | 1363.6364 | 636.3636 |  43.93 MB |
| CountLoad4M      | 45.95 ms |  7.807 ms | 0.428 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

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
| CountFitLongDocuments | 174,805.25 μs |  5,359.642 μs |   293.780 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     397.18 μs |     53.964 μs |     2.958 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     449.87 μs |     45.718 μs |     2.506 μs |   10.7422 |    4.3945 |   4.3945 |   739.44 KB |
| TfidfSave45K          |      52.88 μs |      7.345 μs |     0.403 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      56.20 μs |      4.054 μs |     0.222 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     290.03 μs |     56.922 μs |     3.120 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     291.24 μs |     24.475 μs |     1.342 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,368.98 μs | 20,248.214 μs | 1,109.873 μs |  593.7500 |  562.5000 | 187.5000 | 22049.03 KB |
| CountLoadAsync4M      |  55,339.91 μs | 40,707.576 μs | 2,231.320 μs | 1100.0000 | 1000.0000 | 300.0000 | 44983.52 KB |
| CountLoad4M           |  51,537.37 μs | 30,499.748 μs | 1,671.794 μs | 1100.0000 | 1000.0000 | 300.0000 |  44976.8 KB |

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
| TfidfSaveAsyncSmall    |     17.036 μs |     22.938 μs |   1.2573 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    500.665 μs |     76.273 μs |   4.1808 μs |   34.1797 |  28.3203 |  28.3203 |   740.15 KB |
| HashingLoadUndeclared  |      5.318 μs |      2.978 μs |   0.1632 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     28.745 μs |      5.849 μs |   0.3206 μs |    7.4158 |   0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |    129.107 μs |      7.726 μs |   0.4235 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    846.189 μs |    349.690 μs |  19.1677 μs |   38.0859 |  37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,769.911 μs | 11,757.628 μs | 644.4752 μs |  515.6250 | 500.0000 | 296.8750 | 10793.89 KB |
| CountSaveAsync1M       |  2,011.254 μs |     52.106 μs |   2.8561 μs |  132.8125 | 125.0000 | 125.0000 |  3162.17 KB |
| IndexSaveAsyncLongHead |  3,403.559 μs |    680.746 μs |  37.3140 μs |   93.7500 |  89.8438 |  89.8438 |  2124.22 KB |
| CountLoadUndeclared2M  | 19,325.516 μs |  1,433.826 μs |  78.5928 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.13 KB |

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
| **UnitLoop**           | **1**          |   **5.890 μs** |  **0.4964 μs** | **0.0272 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.128 μs |  0.8539 μs | 0.0468 μs |  1.04 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   6.061 μs |  0.2562 μs | 0.0140 μs |  1.03 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **56.834 μs** |  **0.8676 μs** | **0.0476 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  22.323 μs |  3.0951 μs | 0.1697 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.966 μs |  0.7815 μs | 0.0428 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **227.625 μs** | **26.6875 μs** | **1.4628 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  83.005 μs |  7.2367 μs | 0.3967 μs |  0.36 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  72.264 μs |  7.5433 μs | 0.4135 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **889.729 μs** | **25.4257 μs** | **1.3937 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 332.142 μs | 30.6522 μs | 1.6802 μs |  0.37 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 276.084 μs | 17.0914 μs | 0.9368 μs |  0.31 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **133.69 ms** |  **20.511 ms** | **1.124 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.85 ms |  10.435 ms | 0.572 ms |  0.49 |    0.01 |  103.68 KB |        3.80 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **140.05 ms** |  **58.689 ms** | **3.217 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  62.47 ms |   5.521 ms | 0.303 ms |  0.45 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **201.73 ms** |   **3.018 ms** | **0.165 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 241.45 ms |  22.289 ms | 1.222 ms |  1.20 |    0.01 |     259 KB |        2.50 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **211.35 ms** | **129.144 ms** | **7.079 ms** |  **1.00** |    **0.04** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 219.26 ms |  15.923 ms | 0.873 ms |  1.04 |    0.03 |   192.8 KB |        3.53 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **256.15 ms** |  **36.259 ms** | **1.987 ms** |  **1.00** |    **0.01** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 349.68 ms | 132.386 ms | 7.257 ms |  1.37 |    0.03 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **257.13 ms** |   **4.585 ms** | **0.251 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 320.41 ms |  72.974 ms | 4.000 ms |  1.25 |    0.01 | 1152.95 KB |        1.55 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **294.65 ms** |  **20.474 ms** | **1.122 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 398.32 ms |  21.178 ms | 1.161 ms |  1.35 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **301.68 ms** |  **23.701 ms** | **1.299 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 400.07 ms | 141.660 ms | 7.765 ms |  1.33 |    0.02 | 7964.22 KB |        1.44 |

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

| Method | length | Mean          | Error        | StdDev    | Allocated |
|------- |------- |--------------:|-------------:|----------:|----------:|
| **Latin**  | **1000**   |      **43.74 μs** |     **3.098 μs** |  **0.170 μs** |         **-** |
| Cjk    | 1000   |      62.79 μs |     5.926 μs |  0.325 μs |         - |
| **Latin**  | **10000**  |   **4,098.69 μs** |    **28.109 μs** |  **1.541 μs** |         **-** |
| Cjk    | 10000  |   6,765.44 μs |   892.134 μs | 48.901 μs |         - |
| **Latin**  | **65536**  | **184,487.71 μs** | **1,425.633 μs** | **78.144 μs** |         **-** |

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
| Unigram | 28.07 ms |  2.789 ms | 0.153 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 85.68 ms | 10.632 ms | 0.583 ms |  3.05 |    0.02 | 1666.6667 |  28.47 MB |        3.48 |

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
| **BpeOnOnePathologicalToken** | **512**    |  **18.44 μs** |  **1.769 μs** | **0.097 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **42.42 μs** | **14.061 μs** | **0.771 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **191.16 μs** | **10.952 μs** | **0.600 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **386.13 μs** | **24.227 μs** | **1.328 μs** | **3.4180** |  **56.63 KB** |

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

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 9.156 ms | 1.485 ms | 0.0814 ms |    1.7 MB |

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
| **DpGroup**    | **cjk**      |  **18.997 μs** |  **5.910 μs** | **0.3240 μs** |         **-** |
| MyersGroup | cjk      | 242.279 μs |  7.347 μs | 0.4027 μs |         - |
| **DpGroup**    | **latin**    |   **9.593 μs** |  **1.067 μs** | **0.0585 μs** |         **-** |
| MyersGroup | latin    | 134.791 μs | 63.040 μs | 3.4554 μs |         - |

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
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **304.16 μs** |  **47.129 μs** | **2.583 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 302.04 μs |  28.991 μs | 1.589 μs |  0.99 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **335.49 μs** |  **13.367 μs** | **0.733 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 326.97 μs |  12.249 μs | 0.671 μs |  0.97 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **319.24 μs** |  **24.703 μs** | **1.354 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 308.67 μs |  66.787 μs | 3.661 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **269.01 μs** |  **49.948 μs** | **2.738 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 319.48 μs |  35.765 μs | 1.960 μs |  1.19 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **306.84 μs** |   **6.293 μs** | **0.345 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 306.83 μs |  43.393 μs | 2.379 μs |  1.00 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **164.52 μs** |  **52.161 μs** | **2.859 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 306.43 μs |  17.168 μs | 0.941 μs |  1.86 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **226.37 μs** | **116.734 μs** | **6.399 μs** |  **1.00** |    **0.03** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 308.58 μs | 159.665 μs | 8.752 μs |  1.36 |    0.05 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.17 μs** |   **4.738 μs** | **0.260 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 310.43 μs |  80.784 μs | 4.428 μs |  3.48 |    0.04 | 1.9531 |  32.02 KB |        1.00 |

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
| **Lodestar_Cdist**                | **50**   |   **183.1 μs** |  **61.43 μs** |  **3.37 μs** |  **1.00** |    **0.02** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   178.6 μs |   5.43 μs |  0.30 μs |  0.98 |    0.02 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   473.0 μs |   6.00 μs |  0.33 μs |  2.58 |    0.04 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,269.6 μs** | **193.48 μs** | **10.61 μs** |  **1.00** |    **0.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,002.0 μs | 305.02 μs | 16.72 μs |  0.92 |    0.01 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,613.2 μs | 376.08 μs | 20.61 μs |  2.33 |    0.01 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

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
| **Lodestar_Fit** | **10000x8x8** |   **152.07 ms** |  **27.841 ms** | **1.526 ms** |  **1.00** |    **0.01** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,317.32 ms | 140.301 ms | 7.690 ms | 15.24 |    0.14 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |            |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **53.86 ms** |   **4.063 ms** | **0.223 ms** |  **1.00** |    **0.01** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   812.91 ms |  15.099 ms | 0.828 ms | 15.09 |    0.06 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **160.53 ms** |  **16.880 ms** |  **0.925 ms** |  **1.00** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 7,070.34 ms | 107.062 ms |  5.868 ms | 44.04 |    0.22 | 38000.0000 | 13000.0000 | 11000.0000 | 531.86 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,912.93 ms | 239.422 ms | 13.124 ms | 24.38 |    0.14 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **18.80 ms** |   **1.482 ms** |  **0.081 ms** |  **1.00** |    **0.01** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   475.03 ms |  22.421 ms |  1.229 ms | 25.27 |    0.11 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   264.46 ms |  20.011 ms |  1.097 ms | 14.07 |    0.07 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

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
| TruncatedSvd_Rank20                       |  13.054 ms |  3.8054 ms | 0.2086 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 102.045 ms | 15.9130 ms | 0.8722 ms |  7.82 |    0.12 |
| Nmf_Transform                             |   2.833 ms |  0.2278 ms | 0.0125 ms |  0.22 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.434 ms |  1.6904 ms | 0.0927 ms |  1.87 |    0.03 |

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
| Ratio          |    97.30 ns |   9.250 ns | 0.507 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   749.65 ns |  51.559 ns | 2.826 ns |  7.70 |    0.04 |      - |         - |          NA |
| TokenSortRatio |   870.82 ns | 128.177 ns | 7.026 ns |  8.95 |    0.07 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 1,016.78 ns |  75.950 ns | 4.163 ns | 10.45 |    0.06 | 0.0534 |     896 B |          NA |
| WRatio         | 1,279.52 ns |  85.666 ns | 4.696 ns | 13.15 |    0.07 | 0.0668 |    1120 B |          NA |

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,932,122.7611 ns |    20,673.1460 ns |   1,133.1649 ns |  1.000 |    0.00 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    678,752.6729 ns |     8,882.2039 ns |     486.8636 ns |  0.351 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,123,385.5651 ns |   110,235.8594 ns |   6,042.3995 ns |  0.581 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,382,920.1250 ns | 6,957,406.3629 ns | 381,358.9210 ns | 35.910 |    0.17 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

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

| Method     | Operation     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **100.2 ns** |   **6.33 ns** |  **0.35 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    208.3 ns |   9.35 ns |  0.51 ns |  2.08 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **769.1 ns** |  **95.06 ns** |  **5.21 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,010.8 ns | 397.85 ns | 21.81 ns | 13.02 |    0.08 |      - |     160 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **987.9 ns** |   **6.88 ns** |  **0.38 ns** |  **1.00** |    **0.00** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,251.9 ns | 754.04 ns | 41.33 ns |  2.28 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,307.0 ns** | **122.54 ns** |  **6.72 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,245.9 ns | 158.52 ns |  8.69 ns |  4.01 |    0.02 | 0.1831 |    3096 B |        2.76 |

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.48 ns** |   **1.263 ns** |  **0.069 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.07 ns |   0.299 ns |  0.016 ns |  1.55 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.33 ns |   0.298 ns |  0.016 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    27.08 ns |   0.778 ns |  0.043 ns |  1.02 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **28.54 ns** |   **6.091 ns** |  **0.334 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.41 ns |   0.879 ns |  0.048 ns |  1.45 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.48 ns |   0.577 ns |  0.032 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.93 ns |   0.287 ns |  0.016 ns |  0.98 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **29.95 ns** |   **1.053 ns** |  **0.058 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.50 ns |   0.478 ns |  0.026 ns |  1.52 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    32.13 ns |   1.157 ns |  0.063 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.41 ns |   0.487 ns |  0.027 ns |  1.02 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **34.84 ns** |   **0.957 ns** |  **0.052 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    46.20 ns |   1.797 ns |  0.099 ns |  1.33 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.91 ns |  10.822 ns |  0.593 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    33.39 ns |   0.187 ns |  0.010 ns |  0.96 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **57.53 ns** |   **2.807 ns** |  **0.154 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.83 ns |   2.701 ns |  0.148 ns |  1.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.11 ns |   0.958 ns |  0.053 ns |  0.99 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    61.94 ns |   9.455 ns |  0.518 ns |  1.08 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **62.45 ns** |   **0.724 ns** |  **0.040 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.70 ns |   4.506 ns |  0.247 ns |  1.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.53 ns |   0.861 ns |  0.047 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    61.59 ns |   0.876 ns |  0.048 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **406.67 ns** |  **16.077 ns** |  **0.881 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   417.34 ns |  14.067 ns |  0.771 ns |  1.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   413.82 ns |   1.058 ns |  0.058 ns |  1.02 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   404.63 ns |   9.782 ns |  0.536 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,678.21 ns** | **201.796 ns** | **11.061 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,766.32 ns | 770.743 ns | 42.247 ns |  1.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,668.84 ns |  39.612 ns |  2.171 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,611.34 ns |  47.402 ns |  2.598 ns |  0.99 |         - |          NA |

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

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **388.2 ns** |   **5.40 ns** |  **0.30 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    192.5 ns |   2.85 ns |  0.16 ns |  0.50 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,395.9 ns** |  **26.63 ns** |  **1.46 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,094.8 ns |  15.13 ns |  0.83 ns |  1.29 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **13,401.0 ns** | **337.06 ns** | **18.48 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,627.8 ns | 626.86 ns | 34.36 ns |  2.36 |         - |          NA |

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,654.7 ns** |   **339.07 ns** |  **18.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,947.7 ns |   302.49 ns |  16.58 ns |  0.92 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,615.9 ns |   919.30 ns |  50.39 ns |  1.34 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,039.1 ns** | **5,052.72 ns** | **276.96 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,325.6 ns |   536.27 ns |  29.39 ns |  0.93 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,556.2 ns |   264.92 ns |  14.52 ns |  1.45 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,420.8 ns** | **3,464.19 ns** | **189.88 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,115.9 ns |   392.69 ns |  21.52 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,180.1 ns |   912.67 ns |  50.03 ns |  1.46 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,146.7 ns** |   **113.37 ns** |   **6.21 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,118.6 ns | 2,138.02 ns | 117.19 ns |  1.19 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,304.0 ns |   150.23 ns |   8.23 ns |  1.81 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,246.1 ns** | **1,055.08 ns** |  **57.83 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,925.8 ns | 2,694.50 ns | 147.69 ns |  0.96 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,417.3 ns |   717.32 ns |  39.32 ns |  1.58 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,454.0 ns** | **2,802.66 ns** | **153.62 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,019.3 ns | 5,938.44 ns | 325.51 ns |  0.95 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,457.5 ns | 3,580.93 ns | 196.28 ns |  1.64 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **4,598.9 ns** |   **687.64 ns** |  **37.69 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,083.1 ns |   758.36 ns |  41.57 ns |  0.89 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,410.4 ns | 2,424.32 ns | 132.89 ns |  1.61 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,503.7 ns** |   **216.05 ns** |  **11.84 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,440.9 ns | 1,611.42 ns |  88.33 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,859.8 ns |   842.05 ns |  46.16 ns |  1.79 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,751.3 ns** |   **834.80 ns** |  **45.76 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,342.8 ns |   887.75 ns |  48.66 ns |  0.95 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,730.9 ns | 1,933.04 ns | 105.96 ns |  1.77 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,369.7 ns** |   **924.86 ns** |  **50.69 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,134.4 ns |   195.38 ns |  10.71 ns |  0.90 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,935.4 ns |   289.95 ns |  15.89 ns |  2.51 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,049.0 ns** |    **19.79 ns** |   **1.08 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,868.1 ns |   157.84 ns |   8.65 ns |  0.71 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,645.1 ns |   665.91 ns |  36.50 ns |  2.14 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,456.1 ns** |   **115.38 ns** |   **6.32 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,856.9 ns |   195.29 ns |  10.70 ns |  0.60 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,316.5 ns |   625.88 ns |  34.31 ns |  1.91 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,618.2 ns** |   **655.32 ns** |  **35.92 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,417.0 ns |   142.63 ns |   7.82 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,313.0 ns |   501.02 ns |  27.46 ns |  2.02 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,232.1 ns** |    **52.59 ns** |   **2.88 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,140.2 ns |   429.65 ns |  23.55 ns |  0.79 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,219.2 ns |   738.51 ns |  40.48 ns |  1.95 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,554.3 ns** |   **407.91 ns** |  **22.36 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,312.6 ns | 2,714.59 ns | 148.80 ns |  1.10 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,349.2 ns | 1,685.26 ns |  92.37 ns |  1.90 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,314.2 ns** |    **42.01 ns** |   **2.30 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    594.8 ns |    41.93 ns |   2.30 ns |  0.45 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,341.6 ns |   309.77 ns |  16.98 ns |  4.06 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **2,909.8 ns** |    **19.31 ns** |   **1.06 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,420.0 ns |    79.04 ns |   4.33 ns |  0.49 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,774.2 ns |   372.54 ns |  20.42 ns |  2.67 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,341.7 ns** |   **361.33 ns** |  **19.81 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,386.2 ns |   283.43 ns |  15.54 ns |  0.45 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,245.2 ns | 8,366.98 ns | 458.62 ns |  2.29 |    0.07 | 0.3052 |    5120 B |          NA |

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

| Method           | Shape       | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,409.3 μs** | **1,895.3 μs** | **103.89 μs** |  **1.00** |    **0.02** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,763.3 μs | 4,547.6 μs | 249.27 μs |  7.17 |    0.12 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  83,555.3 μs | 1,115.1 μs |  61.12 μs | 15.45 |    0.25 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |            |           |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **798.4 μs** |   **102.8 μs** |   **5.64 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,770.6 μs |   178.9 μs |   9.81 μs |  5.98 |    0.04 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,080.9 μs |   942.3 μs |  51.65 μs |  3.86 |    0.06 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |            |           |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,397.7 μs** | **2,055.0 μs** | **112.64 μs** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 442,091.7 μs | 1,183.4 μs |  64.86 μs | 16.75 |    0.06 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 735,659.1 μs | 9,775.2 μs | 535.81 μs | 27.87 |    0.10 |        - |        - |        - |  199.91 KB |       0.046 |

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
| **Lodestar_Lloyd** | **10000x16x16** |   **4.999 ms** | **0.4573 ms** | **0.0251 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.919 ms | 8.3355 ms | 0.4569 ms |  3.38 |    0.08 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.547 ms** | **9.6732 ms** | **0.5302 ms** |  **1.00** |    **0.02** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.035 ms | 5.1809 ms | 0.2840 ms |  4.26 |    0.07 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.026 ms** | **2.1590 ms** | **0.1183 ms** |  **1.00** |    **0.00** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 206.851 ms | 3.6019 ms | 0.1974 ms |  4.49 |    0.01 |        - |        - |        - |   36.47 KB |       0.009 |

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

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.41 ns** |      **2.317 ns** |   **0.127 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.70 ns |      1.541 ns |   0.084 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.09 ns |      1.470 ns |   0.081 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     96.21 ns |      3.093 ns |   0.170 ns |  0.73 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **222.97 ns** |    **176.211 ns** |   **9.659 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 12   |     62.17 ns |      0.766 ns |   0.042 ns |  0.28 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    226.22 ns |    113.286 ns |   6.210 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    112.14 ns |      2.168 ns |   0.119 ns |  0.50 |    0.02 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **277.46 ns** |    **165.390 ns** |   **9.066 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 14   |     66.26 ns |      0.431 ns |   0.024 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    278.29 ns |    244.303 ns |  13.391 ns |  1.00 |    0.05 |         - |          NA |
| Kernel_Cjk | 14   |    114.01 ns |     28.404 ns |   1.557 ns |  0.41 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **363.12 ns** |    **145.816 ns** |   **7.993 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 16   |     71.26 ns |      5.441 ns |   0.298 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    361.65 ns |    215.354 ns |  11.804 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    123.35 ns |     76.148 ns |   4.174 ns |  0.34 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **436.86 ns** |     **40.728 ns** |   **2.232 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.72 ns |      0.471 ns |   0.026 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    435.36 ns |     21.043 ns |   1.153 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    128.89 ns |     63.482 ns |   3.480 ns |  0.30 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **773.83 ns** |     **14.005 ns** |   **0.768 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     78.10 ns |      0.946 ns |   0.052 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    774.92 ns |     22.759 ns |   1.247 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    127.76 ns |      0.761 ns |   0.042 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |  **1,002.38 ns** |     **21.361 ns** |   **1.171 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |     86.46 ns |      4.232 ns |   0.232 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    997.38 ns |     68.851 ns |   3.774 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    141.34 ns |      6.206 ns |   0.340 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,545.86 ns** |    **510.540 ns** |  **27.984 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 32   |    105.97 ns |      5.710 ns |   0.313 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,522.33 ns |     85.284 ns |   4.675 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |    169.49 ns |     46.152 ns |   2.530 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,286.68 ns** |  **4,476.907 ns** | **245.394 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Kernel     | 48   |    132.86 ns |      0.752 ns |   0.041 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,626.32 ns |    906.060 ns |  49.664 ns |  1.11 |    0.07 |         - |          NA |
| Kernel_Cjk | 48   |    241.69 ns |      1.718 ns |   0.094 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,910.43 ns** |    **364.086 ns** |  **19.957 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    165.32 ns |      0.704 ns |   0.039 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,497.01 ns |     60.403 ns |   3.311 ns |  0.93 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |    291.30 ns |      2.120 ns |   0.116 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **11,979.62 ns** |    **657.405 ns** |  **36.035 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    351.80 ns |      8.316 ns |   0.456 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,039.09 ns | 14,054.602 ns | 770.380 ns |  1.00 |    0.06 |         - |          NA |
| Kernel_Cjk | 96   |    954.28 ns |     18.653 ns |   1.022 ns |  0.08 |    0.00 |         - |          NA |

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

| Method                     | Length | Mean         | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.85 ns** |   **6.029 ns** |  **0.330 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.64 ns |   0.652 ns |  0.036 ns |  0.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    119.65 ns |   2.718 ns |  0.149 ns |  4.82 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.25 ns |   1.746 ns |  0.096 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **284.20 ns** |  **28.321 ns** |  **1.552 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    369.73 ns | 104.799 ns |  5.744 ns |  1.30 |    0.02 |         - |          NA |
| Distance_CodePoint         | 64     |    668.61 ns |  11.732 ns |  0.643 ns |  2.35 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    278.36 ns |  42.118 ns |  2.309 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **900.73 ns** |   **5.791 ns** |  **0.317 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,894.79 ns |  51.690 ns |  2.833 ns |  2.10 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,664.94 ns | 488.963 ns | 26.802 ns |  1.85 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    920.43 ns |   7.540 ns |  0.413 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,078.14 ns** | **681.683 ns** | **37.365 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,403.05 ns | 354.430 ns | 19.427 ns |  1.48 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 13,529.13 ns | 866.409 ns | 47.491 ns |  1.22 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,987.25 ns | 177.507 ns |  9.730 ns |  0.99 |    0.00 |         - |          NA |

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

| Method             | Length | Distinct | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **337.0 ns** |    **48.03 ns** |   **2.63 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     239.6 ns |     3.28 ns |   0.18 ns |  0.71 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **352.1 ns** |    **12.27 ns** |   **0.67 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     237.9 ns |    23.13 ns |   1.27 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **424.5 ns** |    **32.58 ns** |   **1.79 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     318.9 ns |    11.44 ns |   0.63 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **422.8 ns** |     **9.48 ns** |   **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     325.4 ns |     1.01 ns |   0.06 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **521.3 ns** |     **9.43 ns** |   **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     406.6 ns |    23.42 ns |   1.28 ns |  0.78 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **537.7 ns** |    **16.21 ns** |   **0.89 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     416.0 ns |     9.58 ns |   0.53 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **598.6 ns** |    **45.88 ns** |   **2.51 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,359.5 ns |   105.87 ns |   5.80 ns |  2.27 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **601.7 ns** |     **8.67 ns** |   **0.48 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,313.2 ns |    55.18 ns |   3.02 ns |  2.18 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,875.6 ns** |    **19.71 ns** |   **1.08 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,393.8 ns |   160.36 ns |   8.79 ns |  2.88 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,938.9 ns** |    **28.66 ns** |   **1.57 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,548.6 ns | 1,939.28 ns | 106.30 ns |  2.86 |    0.05 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,728.1 ns** |   **459.37 ns** |  **25.18 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,861.5 ns | 6,330.44 ns | 346.99 ns |  3.93 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **431,775.0 ns** | **3,082.75 ns** | **168.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  58,709.7 ns |   950.96 ns |  52.13 ns |  0.14 |    0.00 |         - |          NA |

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
| **Lodestar**             | **8**      |      **24.91 ns** |      **6.502 ns** |     **0.356 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      82.81 ns |      6.583 ns |     0.361 ns |  3.32 |    0.04 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      85.85 ns |      2.136 ns |     0.117 ns |  3.45 |    0.04 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     167.95 ns |     57.963 ns |     3.177 ns |  6.74 |    0.14 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.99 ns** |     **17.442 ns** |     **0.956 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,943.36 ns |  3,288.274 ns |   180.241 ns | 21.46 |    0.57 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,438.75 ns |     33.702 ns |     1.847 ns |  5.19 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,552.27 ns |  3,359.154 ns |   184.127 ns | 38.10 |    0.59 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,053.47 ns** |     **52.682 ns** |     **2.888 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 433,165.70 ns | 24,476.915 ns | 1,341.662 ns | 39.19 |    0.11 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  38,389.34 ns |  1,339.992 ns |    73.449 ns |  3.47 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 715,146.96 ns | 56,403.084 ns | 3,091.643 ns | 64.70 |    0.24 |      - |    4161 B |          NA |

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
| **F1Macro**           | **1000**   | **601.7 μs** | **39.19 μs** | **2.15 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 632.3 μs | 66.86 μs | 3.66 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 600.2 μs | 27.63 μs | 1.51 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **780.5 μs** | **20.26 μs** | **1.11 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 782.4 μs | 25.71 μs | 1.41 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 937.1 μs | 67.60 μs | 3.71 μs | 46.8750 | 18.5547 | 767.27 KB |

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

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.14 μs** |     **19.407 μs** |     **1.064 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 100,114.16 μs |  4,203.098 μs |   230.386 μs | 751.97 |    5.39 | 833.3333 | 833.3333 | 833.3333 | 31414.69 KB |   12,805.99 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,790.21 μs** |    **326.306 μs** |    **17.886 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 477,911.70 μs | 50,469.690 μs | 2,766.414 μs | 171.29 |    1.28 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.74 μs** |      **6.435 μs** |     **0.353 μs** |   **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     891.97 μs |     71.786 μs |     3.935 μs |  39.24 |    0.54 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

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

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,280.5 ns** |       **242.17 ns** |     **13.27 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,121.7 ns |       370.73 ns |     20.32 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       110.2 ns |         7.80 ns |      0.43 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,465.9 ns |       442.21 ns |     24.24 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,401.2 ns |       443.00 ns |     24.28 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,850.6 ns** |     **1,649.49 ns** |     **90.41 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,615.2 ns |       301.03 ns |     16.50 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       110.0 ns |        29.17 ns |      1.60 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,524.6 ns |     1,633.34 ns |     89.53 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,899.9 ns |       472.49 ns |     25.90 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **449,376.9 ns** |    **96,380.64 ns** |  **5,282.95 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   835,049.8 ns |    24,700.43 ns |  1,353.91 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,251.0 ns |     1,069.77 ns |     58.64 ns |      - |         - |
| F1Macro        | 100000  | 2       |   477,153.1 ns |     7,737.84 ns |    424.14 ns |      - |     656 B |
| Report         | 100000  | 2       |   465,023.2 ns |    23,898.63 ns |  1,309.96 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **509,081.7 ns** |    **30,207.88 ns** |  **1,655.80 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   929,816.1 ns |    10,342.17 ns |    566.89 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,692.6 ns |     1,079.86 ns |     59.19 ns |      - |         - |
| F1Macro        | 100000  | 10      |   493,910.3 ns |    27,930.42 ns |  1,530.96 ns |      - |    1881 B |
| Report         | 100000  | 10      |   502,877.2 ns |    22,139.99 ns |  1,213.57 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,754,136.8 ns** |    **55,344.68 ns** |  **3,033.63 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,703,428.4 ns |   927,513.52 ns | 50,840.15 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,831.8 ns |     3,923.41 ns |    215.06 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,764,158.7 ns |    46,303.21 ns |  2,538.04 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,762,768.8 ns |   173,814.77 ns |  9,527.37 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,863,213.5 ns** |   **260,599.36 ns** | **14,284.33 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,488,478.1 ns |   506,055.75 ns | 27,738.62 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,385.4 ns |     3,309.82 ns |    181.42 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 5,065,115.6 ns |   116,027.65 ns |  6,359.87 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,063,904.7 ns | 1,627,746.89 ns | 89,222.30 ns |      - |   12878 B |

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
| **Lodestar** | **100000**  | **Bundle**        |  **10,460.64 μs** |  **2,211.157 μs** |   **121.201 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,275.88 μs |  2,721.521 μs |   149.176 μs |     3.37 |    0.04 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |    3,927.18 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.20 μs** |      **3.062 μs** |     **0.168 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,732.86 μs |  3,223.773 μs |   176.706 μs | 3,101.35 |   42.64 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **140,970.95 μs** | **65,200.731 μs** | **3,573.872 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 251,702.72 μs | 29,010.955 μs | 1,590.188 μs |     1.79 |    0.04 |        - |        - |        - | 23228948 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **108.19 μs** |     **16.368 μs** |     **0.897 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 229,029.64 μs | 10,616.000 μs |   581.899 μs | 2,116.97 |   15.86 |        - |        - |        - | 23228008 B |          NA |

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
| **Dp**         | **4**    |     **73.44 ns** |     **1.266 ns** |   **0.069 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.43 ns |     1.473 ns |   0.081 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     74.83 ns |    25.876 ns |   1.418 ns |  1.02 |    0.02 |         - |          NA |
| Kernel_Cjk | 4    |     73.39 ns |     0.476 ns |   0.026 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **106.23 ns** |    **53.077 ns** |   **2.909 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 6    |     90.20 ns |    23.785 ns |   1.304 ns |  0.85 |    0.02 |         - |          NA |
| Dp_Cjk     | 6    |    105.70 ns |     2.239 ns |   0.123 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    130.17 ns |     0.822 ns |   0.045 ns |  1.23 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **156.74 ns** |     **1.916 ns** |   **0.105 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.82 ns |     2.205 ns |   0.121 ns |  0.66 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.08 ns |     7.267 ns |   0.398 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    181.26 ns |    17.036 ns |   0.934 ns |  1.16 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **202.42 ns** |    **28.722 ns** |   **1.574 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |    109.53 ns |     8.421 ns |   0.462 ns |  0.54 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    198.44 ns |    43.335 ns |   2.375 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    149.96 ns |     1.511 ns |   0.083 ns |  0.74 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **244.85 ns** |     **5.111 ns** |   **0.280 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    121.20 ns |     0.511 ns |   0.028 ns |  0.49 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    245.67 ns |    30.207 ns |   1.656 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    162.56 ns |     4.488 ns |   0.246 ns |  0.66 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **409.78 ns** |    **10.037 ns** |   **0.550 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    132.80 ns |     3.562 ns |   0.195 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    411.69 ns |    13.265 ns |   0.727 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    179.37 ns |     3.997 ns |   0.219 ns |  0.44 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **847.19 ns** |   **156.392 ns** |   **8.572 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |    169.26 ns |    10.756 ns |   0.590 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    842.04 ns |    10.644 ns |   0.583 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    223.87 ns |     6.092 ns |   0.334 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,451.33 ns** |    **56.414 ns** |   **3.092 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    205.53 ns |     5.970 ns |   0.327 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,427.36 ns |   178.792 ns |   9.800 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    273.65 ns |     6.646 ns |   0.364 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,352.38 ns** |   **175.475 ns** |   **9.618 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    278.81 ns |    24.064 ns |   1.319 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,406.53 ns |   196.684 ns |  10.781 ns |  1.02 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    355.66 ns |     7.115 ns |   0.390 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,349.97 ns** | **4,984.504 ns** | **273.217 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 64   |    345.61 ns |     2.667 ns |   0.146 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  7,067.59 ns |   577.383 ns |  31.648 ns |  1.11 |    0.04 |         - |          NA |
| Kernel_Cjk | 64   |    449.02 ns |     9.021 ns |   0.494 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,227.11 ns** | **6,192.859 ns** | **339.452 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    740.59 ns |    52.677 ns |   2.887 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,191.88 ns | 5,208.618 ns | 285.502 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |  1,558.13 ns |   173.044 ns |   9.485 ns |  0.11 |    0.00 |         - |          NA |

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
| **WholeFit**              | **10000**    | **10**         |   **206.34 μs** |  **43.103 μs** |  **2.363 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   229.38 μs |  20.121 μs |  1.103 μs |  1.11 |    0.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    58.81 μs |   5.703 μs |  0.313 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.48 μs |  10.556 μs |  0.579 μs |  0.96 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **204.99 μs** |  **20.342 μs** |  **1.115 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   232.12 μs |   7.224 μs |  0.396 μs |  1.13 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    58.81 μs |   1.944 μs |  0.107 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   199.21 μs |  60.448 μs |  3.313 μs |  0.97 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,979.80 μs** | **178.060 μs** |  **9.760 μs** |  **1.00** |    **0.01** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,042.52 μs | 195.398 μs | 10.710 μs |  1.03 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,088.19 μs |  43.907 μs |  2.407 μs |  0.55 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,969.54 μs |  17.917 μs |  0.982 μs |  0.99 |    0.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,037.82 μs** |  **50.779 μs** |  **2.783 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,127.82 μs | 788.511 μs | 43.221 μs |  1.04 |    0.02 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,093.00 μs |  52.478 μs |  2.877 μs |  0.54 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,967.80 μs |  60.215 μs |  3.301 μs |  0.97 |    0.00 |      - |     803 B |        1.00 |

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.558 μs** |  **0.5178 μs** | **0.0284 μs** |         **-** |
| EqualLength | 65           |     2.989 μs |  0.0072 μs | 0.0004 μs |         - |
| **Embedded**    | **128**          |    **39.271 μs** |  **1.8581 μs** | **0.1018 μs** |         **-** |
| EqualLength | 128          |     5.418 μs |  0.0447 μs | 0.0025 μs |         - |
| **Embedded**    | **512**          | **1,356.692 μs** | **38.0978 μs** | **2.0883 μs** |         **-** |
| EqualLength | 512          |    44.586 μs |  1.1312 μs | 0.0620 μs |         - |

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
| VocabTxt               |  3.902 ms |  2.3917 ms | 0.1311 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.238 ms |  2.9354 ms | 0.1609 ms | 187.5000 | 171.8750 |  46.8750 |  5852.41 KB |
| TokenizerJsonUnigram   | 12.448 ms |  1.1742 ms | 0.0644 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.092 ms |  1.9460 ms | 0.1067 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.541 ms |  0.1897 ms | 0.0104 ms |  23.4375 |  17.5781 |  17.5781 |  2137.06 KB |
| TfidfLoad              |  4.214 ms |  0.3069 ms | 0.0168 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.143 ms |  0.3663 ms | 0.0201 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.296 ms |  0.1249 ms | 0.0068 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.516 ms | 12.3163 ms | 0.6751 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 70.833 ms |  2.0688 ms | 0.1134 ms |        - |        - |        - | 16095.25 KB |

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
| **Lodestar_ExplainedVariance** | **100x200** |  **9,518.10 μs** | **1,242.481 μs** |  **68.105 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,290.66 μs | 3,044.766 μs | 166.894 μs |  1.61 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **130.83 μs** |    **12.877 μs** |   **0.706 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    184.50 μs |    15.829 μs |   0.868 μs |  1.41 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,917.59 μs** |   **357.864 μs** |  **19.616 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,614.34 μs |   121.279 μs |   6.648 μs |  0.90 |    0.01 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.57 μs** |     **1.235 μs** |   **0.068 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.81 μs |     4.834 μs |   0.265 μs |  1.14 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

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
| **Mse**      | **100000**  |    **45.40 μs** |   **4.750 μs** |  **0.260 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    44.96 μs |   3.401 μs |  0.186 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.01 μs |   5.719 μs |  0.313 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   570.16 μs |  65.596 μs |  3.596 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **462.41 μs** | **179.148 μs** |  **9.820 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   466.19 μs |  10.635 μs |  0.583 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,132.16 μs |  98.141 μs |  5.379 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,226.51 μs | 314.729 μs | 17.251 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

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

| Method                 | Mean            | Error           | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,154.0 ns |       203.26 ns |     11.14 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,390.4 ns |       744.21 ns |     40.79 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,209.4 ns |       725.92 ns |     39.79 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        461.0 ns |        72.58 ns |      3.98 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,285,814.4 ns | 1,711,152.89 ns | 93,794.06 ns | 875.0000 | 843.7500 | 609.3750 | 12227071 B |

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
| **Count**                | **200**       | **1.886 ms** | **0.1688 ms** | **0.0093 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.376 ms | 0.0661 ms | 0.0036 ms |  0.73 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 2.018 ms | 0.1224 ms | 0.0067 ms |  1.07 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.667 ms | 0.0368 ms | 0.0020 ms |  0.88 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **9.179 ms** | **1.4995 ms** | **0.0822 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** |  **3436.5 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.172 ms | 0.5946 ms | 0.0326 ms |  0.78 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.650 ms | 1.2916 ms | 0.0708 ms |  1.05 | 515.6250 | 500.0000 | 500.0000 | 2349.22 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.655 ms | 0.3191 ms | 0.0175 ms |  0.94 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

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
| **Lodestar**     | **WordPiece**     |  **21.79 ms** |   **1.233 ms** | **0.068 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  55.24 ms |  10.169 ms | 0.557 ms |  2.53 |    0.02 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.04 ms** |   **1.862 ms** | **0.102 ms** |  **1.00** |    **0.00** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  56.58 ms |   1.232 ms | 0.068 ms |  1.20 |    0.00 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **87.13 ms** |  **17.819 ms** | **0.977 ms** |  **1.00** |    **0.01** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 281.26 ms | 111.259 ms | 6.098 ms |  3.23 |    0.07 | 3500.0000 |  59.08 MB |        2.08 |

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

| Method   | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.776 ms** |   **0.3529 ms** | **0.0193 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  58.549 ms | 119.8265 ms | 6.5681 ms | 15.51 |    1.51 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.227 ms** |   **1.0022 ms** | **0.0549 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 413.859 ms | 141.0443 ms | 7.7311 ms | 24.02 |    0.39 | 75000.0000 | 75000.0000 | 75000.0000 | 324.64 MB |       40.03 |

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
- `nmf-transform`

### compare-cdist

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.169 | 0.036 | 0.21x | 0.169 | 0.036 | 0.21x |
| cdist_wratio_n50 | 2.241 | 2.487 | 1.11x | 2.241 | 2.486 | 1.11x |
| cdist_ratio_varied_n50 | 0.147 | 0.031 | 0.21x | 0.147 | 0.031 | 0.21x |
| cdist_ratio_varied_cutoff90_n50 | 0.064 | 0.031 | 0.49x | 0.064 | 0.031 | 0.49x |
| cdist_ratio_n200 | 2.970 | 0.439 | 0.15x | 2.975 | 0.439 | 0.15x |
| cdist_wratio_n200 | 34.168 | 37.332 | 1.09x | 34.167 | 37.328 | 1.09x |
| cdist_ratio_varied_n200 | 2.483 | 0.385 | 0.16x | 2.487 | 0.385 | 0.15x |
| cdist_ratio_varied_cutoff90_n200 | 1.102 | 0.387 | 0.35x | 1.105 | 0.387 | 0.35x |
| cdist_ratio_n500 | 17.329 | 2.642 | 0.15x | 17.353 | 2.642 | 0.15x |
| cdist_wratio_n500 | 206.334 | 227.534 | 1.10x | 206.489 | 227.513 | 1.10x |
| cdist_ratio_varied_n500 | 14.380 | 2.292 | 0.16x | 14.496 | 2.292 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 6.215 | 2.290 | 0.37x | 6.326 | 2.290 | 0.36x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 119.3 | 25.8 | 4.63x C# faster |
| latin | 32 | 183.3 | 86.2 | 2.13x C# faster |
| latin | 128 | 486.3 | 423.0 | 1.15x C# faster |
| latin | 512 | 4473.4 | 4942.1 | 1.10x Py faster |
| cjk | 8 | 142.7 | 26.6 | 5.36x C# faster |
| cjk | 32 | 331.8 | 222.0 | 1.49x C# faster |
| cjk | 128 | 1901.7 | 1380.6 | 1.38x C# faster |
| cjk | 512 | 14860.8 | 8833.4 | 1.68x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 147.8 | 18.1 | 8.18x C# faster |
| latin | 32 | 282.8 | 157.8 | 1.79x C# faster |
| latin | 128 | 1689.9 | 884.2 | 1.91x C# faster |
| latin | 512 | 14026.4 | 11561.1 | 1.21x C# faster |
| cjk | 8 | 159.3 | 17.8 | 8.95x C# faster |
| cjk | 32 | 387.2 | 290.8 | 1.33x C# faster |
| cjk | 128 | 2850.1 | 2082.6 | 1.37x C# faster |
| cjk | 512 | 23656.2 | 19128.6 | 1.24x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 1.014 | 236.90x | 0.004 | 1.014 | 236.92x |
| accuracy_n1000_k2 | 0.000 | 0.539 | 4035.91x | 0.000 | 0.539 | 4035.97x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.830 | 368.74x | 0.005 | 1.830 | 368.74x |
| classification_report_n1000_k2 | 0.007 | 7.015 | 964.88x | 0.007 | 7.015 | 964.93x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.955 | 112.74x | 0.017 | 1.955 | 112.66x |
| balanced_accuracy_n1000_k2 | 0.004 | 1.073 | 242.81x | 0.004 | 1.073 | 242.75x |
| matthews_n1000_k2 | 0.004 | 2.037 | 468.08x | 0.004 | 2.037 | 468.07x |
| cohen_kappa_n1000_k2 | 0.004 | 1.091 | 250.85x | 0.004 | 1.091 | 250.83x |
| mse_n1000_k2 | 0.000 | 0.308 | 627.45x | 0.000 | 0.307 | 627.40x |
| mae_n1000_k2 | 0.000 | 0.306 | 625.81x | 0.000 | 0.306 | 625.76x |
| median_ae_n1000_k2 | 0.005 | 0.325 | 59.46x | 0.005 | 0.325 | 59.45x |
| r2_n1000_k2 | 0.001 | 0.376 | 314.78x | 0.001 | 0.376 | 314.79x |
| confusion_matrix_n1000_k10 | 0.005 | 0.998 | 216.46x | 0.005 | 0.998 | 216.45x |
| accuracy_n1000_k10 | 0.000 | 0.532 | 3974.74x | 0.000 | 0.532 | 3974.53x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.837 | 315.88x | 0.006 | 1.837 | 315.90x |
| classification_report_n1000_k10 | 0.012 | 7.077 | 610.72x | 0.012 | 7.077 | 610.69x |
| roc_auc_ovr_macro_n1000_k10 | 0.514 | 9.901 | 19.24x | 0.514 | 9.900 | 19.24x |
| balanced_accuracy_n1000_k10 | 0.005 | 1.058 | 203.13x | 0.005 | 1.058 | 203.13x |
| matthews_n1000_k10 | 0.005 | 2.064 | 399.33x | 0.005 | 2.064 | 399.33x |
| cohen_kappa_n1000_k10 | 0.006 | 1.115 | 193.09x | 0.006 | 1.115 | 193.10x |
| mse_n1000_k10 | 0.000 | 0.308 | 636.79x | 0.000 | 0.308 | 636.73x |
| mae_n1000_k10 | 0.000 | 0.309 | 638.23x | 0.000 | 0.309 | 638.23x |
| median_ae_n1000_k10 | 0.006 | 0.320 | 56.31x | 0.006 | 0.320 | 56.31x |
| r2_n1000_k10 | 0.001 | 0.379 | 316.89x | 0.001 | 0.379 | 316.87x |
| confusion_matrix_n100000_k2 | 0.460 | 10.923 | 23.77x | 0.460 | 10.922 | 23.77x |
| accuracy_n100000_k2 | 0.012 | 3.800 | 308.47x | 0.012 | 3.800 | 308.45x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.581 | 27.32x | 0.460 | 12.579 | 27.32x |
| classification_report_n100000_k2 | 0.463 | 27.779 | 60.05x | 0.463 | 27.777 | 60.05x |
| roc_auc_binary_n100000_k2 | 4.299 | 27.428 | 6.38x | 4.298 | 27.423 | 6.38x |
| balanced_accuracy_n100000_k2 | 0.459 | 11.124 | 24.23x | 0.459 | 11.123 | 24.23x |
| matthews_n100000_k2 | 0.458 | 22.061 | 48.13x | 0.458 | 22.058 | 48.13x |
| cohen_kappa_n100000_k2 | 0.456 | 11.090 | 24.34x | 0.456 | 11.089 | 24.34x |
| mse_n100000_k2 | 0.046 | 0.474 | 10.42x | 0.046 | 0.474 | 10.42x |
| mae_n100000_k2 | 0.045 | 0.466 | 10.34x | 0.045 | 0.465 | 10.34x |
| median_ae_n100000_k2 | 0.698 | 1.820 | 2.61x | 0.719 | 1.820 | 2.53x |
| r2_n100000_k2 | 0.110 | 0.722 | 6.56x | 0.110 | 0.722 | 6.56x |
| confusion_matrix_n100000_k10 | 0.492 | 10.972 | 22.29x | 0.492 | 10.968 | 22.28x |
| accuracy_n100000_k10 | 0.013 | 3.820 | 300.59x | 0.013 | 3.819 | 300.54x |
| precision_recall_f1_macro_n100000_k10 | 0.493 | 13.109 | 26.57x | 0.493 | 13.108 | 26.57x |
| classification_report_n100000_k10 | 0.500 | 30.014 | 60.02x | 0.500 | 30.012 | 60.02x |
| roc_auc_ovr_macro_n100000_k10 | 35.076 | 221.562 | 6.32x | 35.071 | 221.546 | 6.32x |
| balanced_accuracy_n100000_k10 | 0.493 | 10.960 | 22.23x | 0.493 | 10.960 | 22.23x |
| matthews_n100000_k10 | 0.493 | 22.609 | 45.87x | 0.493 | 22.607 | 45.87x |
| cohen_kappa_n100000_k10 | 0.494 | 10.964 | 22.21x | 0.494 | 10.964 | 22.21x |
| mse_n100000_k10 | 0.045 | 0.474 | 10.43x | 0.045 | 0.474 | 10.43x |
| mae_n100000_k10 | 0.045 | 0.463 | 10.32x | 0.045 | 0.463 | 10.32x |
| median_ae_n100000_k10 | 0.698 | 1.820 | 2.61x | 0.744 | 1.819 | 2.45x |
| r2_n100000_k10 | 0.110 | 0.718 | 6.53x | 0.110 | 0.718 | 6.53x |
| confusion_matrix_n1000000_k2 | 4.603 | 101.549 | 22.06x | 4.603 | 101.542 | 22.06x |
| accuracy_n1000000_k2 | 0.134 | 33.431 | 249.79x | 0.134 | 33.429 | 249.78x |
| precision_recall_f1_macro_n1000000_k2 | 4.603 | 109.037 | 23.69x | 4.603 | 109.018 | 23.68x |
| classification_report_n1000000_k2 | 4.619 | 212.670 | 46.04x | 4.620 | 212.645 | 46.03x |
| roc_auc_binary_n1000000_k2 | 56.994 | 293.028 | 5.14x | 56.994 | 292.990 | 5.14x |
| balanced_accuracy_n1000000_k2 | 4.600 | 100.675 | 21.89x | 4.599 | 100.643 | 21.88x |
| matthews_n1000000_k2 | 4.602 | 200.919 | 43.66x | 4.602 | 200.890 | 43.65x |
| cohen_kappa_n1000000_k2 | 4.606 | 100.544 | 21.83x | 4.605 | 100.531 | 21.83x |
| mse_n1000000_k2 | 0.452 | 2.447 | 5.41x | 0.452 | 2.446 | 5.41x |
| mae_n1000000_k2 | 0.452 | 2.407 | 5.33x | 0.452 | 2.407 | 5.33x |
| median_ae_n1000000_k2 | 6.437 | 14.440 | 2.24x | 6.517 | 14.438 | 2.22x |
| r2_n1000000_k2 | 1.105 | 4.412 | 3.99x | 1.105 | 4.410 | 3.99x |
| confusion_matrix_n1000000_k10 | 4.909 | 100.581 | 20.49x | 4.909 | 100.558 | 20.49x |
| accuracy_n1000000_k10 | 0.136 | 33.479 | 246.36x | 0.136 | 33.464 | 246.28x |
| precision_recall_f1_macro_n1000000_k10 | 4.910 | 115.227 | 23.47x | 4.909 | 115.217 | 23.47x |
| classification_report_n1000000_k10 | 4.913 | 237.443 | 48.33x | 4.913 | 237.424 | 48.32x |
| balanced_accuracy_n1000000_k10 | 4.908 | 101.116 | 20.60x | 4.907 | 101.106 | 20.60x |
| matthews_n1000000_k10 | 4.911 | 209.850 | 42.73x | 4.910 | 209.823 | 42.73x |
| cohen_kappa_n1000000_k10 | 4.908 | 101.168 | 20.61x | 4.908 | 101.160 | 20.61x |
| mse_n1000000_k10 | 0.443 | 2.530 | 5.71x | 0.443 | 2.530 | 5.71x |
| mae_n1000000_k10 | 0.447 | 2.502 | 5.60x | 0.446 | 2.502 | 5.60x |
| median_ae_n1000000_k10 | 6.290 | 14.714 | 2.34x | 6.346 | 14.712 | 2.32x |
| r2_n1000000_k10 | 1.122 | 4.192 | 3.74x | 1.122 | 4.191 | 3.74x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.118 | 0.869 | 0.41x | 2.118 | 0.869 | 0.41x |
| transform_frobenius_n500 | 6.949 | 1.639 | 0.24x | 6.949 | 1.639 | 0.24x |
| transform_frobenius_n2000 | 25.401 | 5.895 | 0.23x | 25.640 | 23.520 | 0.92x |
| transform_kl_n100 | 1.127 | 17.852 | 15.84x | 1.127 | 17.852 | 15.84x |
| transform_kl_n500 | 5.067 | 34.393 | 6.79x | 5.066 | 34.391 | 6.79x |
| transform_kl_n2000 | 20.136 | 122.497 | 6.08x | 20.200 | 451.329 | 22.34x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.112 | 9.896 | 1.94x | 5.463 | 9.895 | 1.81x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.061 | 16.085 | 1.33x | 12.343 | 16.084 | 1.30x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.886 | 39.621 | 2.85x | 14.217 | 39.616 | 2.79x | 1,990,038 | 1,990,038 |
| spiece_model | 7.948 | 29.742 | 3.74x | 8.215 | 29.740 | 3.62x | 533,084 | 533,084 |
| tfidf_save | 1.483 | 2.332 | 1.57x | 1.501 | 2.332 | 1.55x | 581,787 | 591,922 |
| tfidf_load | 4.677 | 4.134 | 0.88x | 4.880 | 4.134 | 0.85x | 581,787 | 591,922 |
| embedding_index_save | 4.338 | 1.508 | 0.35x | 4.538 | 1.508 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.009 | 37.386 | 0.75x | 10.874 | 5.065 | 0.47x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.685 | 1.359 | 0.24x | 6.052 | 1.359 | 0.22x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.483 | 0.957 | 0.15x | 6.867 | 0.955 | 0.14x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.497 | 1.384 | 0.31x | 4.781 | 1.383 | 0.29x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.463 | 1.436 | 0.98x | 1.785 | 1.436 | 0.80x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 81.30x | 0.000 | 0.001 | 81.31x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 409.471 | 555.195 | 1.36x | 409.398 | 555.123 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 70.827 | 66.729 | 0.94x | 70.796 | 66.719 | 0.94x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.092 | 0.113 | 1.23x | 0.092 | 0.113 | 1.23x |
| stratified_n10000 | 0.180 | 0.884 | 4.91x | 0.180 | 0.884 | 4.91x |
| traintest_n10000 | 0.005 | 0.239 | 50.65x | 0.005 | 0.239 | 50.64x |
| kfold_seeded_n10000 | 0.308 | 0.670 | 2.17x | 0.308 | 0.670 | 2.17x |
| stratified_seeded_n10000 | 0.437 | 1.483 | 3.40x | 0.437 | 1.483 | 3.40x |
| traintest_seeded_n10000 | 0.174 | 0.525 | 3.01x | 0.174 | 0.525 | 3.01x |
| stratified_traintest_n10000 | 0.669 | 2.756 | 4.12x | 0.669 | 2.756 | 4.12x |
| group_kfold_n10000 | 0.129 | 0.603 | 4.67x | 0.129 | 0.603 | 4.67x |
| stratified_group_kfold_n10000 | 0.245 | 32.000 | 130.50x | 0.245 | 31.997 | 130.49x |
| timeseries_n10000 | 0.000 | 0.012 | 87.72x | 0.000 | 0.012 | 87.72x |
| repeated_kfold_n10000 | 0.990 | 1.784 | 1.80x | 0.990 | 1.784 | 1.80x |
| kfold_n100000 | 1.151 | 2.535 | 2.20x | 1.302 | 2.535 | 1.95x |
| stratified_n100000 | 2.186 | 7.095 | 3.25x | 2.318 | 7.094 | 3.06x |
| traintest_n100000 | 0.209 | 0.406 | 1.94x | 0.211 | 0.406 | 1.93x |
| kfold_seeded_n100000 | 3.532 | 6.906 | 1.96x | 3.685 | 6.906 | 1.87x |
| stratified_seeded_n100000 | 4.600 | 11.395 | 2.48x | 4.776 | 11.393 | 2.39x |
| traintest_seeded_n100000 | 2.058 | 2.004 | 0.97x | 2.102 | 2.004 | 0.95x |
| stratified_traintest_n100000 | 8.087 | 23.747 | 2.94x | 8.185 | 23.745 | 2.90x |
| group_kfold_n100000 | 1.588 | 6.778 | 4.27x | 1.722 | 6.778 | 3.94x |
| stratified_group_kfold_n100000 | 2.604 | 323.799 | 124.35x | 2.744 | 323.776 | 118.00x |
| timeseries_n100000 | 0.000 | 0.042 | 350.95x | 0.000 | 0.042 | 350.96x |
| repeated_kfold_n100000 | 9.948 | 18.965 | 1.91x | 10.153 | 18.963 | 1.87x |
| kfold_n1000000 | 9.377 | 17.311 | 1.85x | 9.940 | 17.310 | 1.74x |
| stratified_n1000000 | 20.348 | 66.685 | 3.28x | 20.891 | 66.662 | 3.19x |
| traintest_n1000000 | 0.481 | 2.069 | 4.30x | 0.554 | 2.069 | 3.74x |
| kfold_seeded_n1000000 | 36.254 | 58.664 | 1.62x | 36.987 | 58.653 | 1.59x |
| stratified_seeded_n1000000 | 47.931 | 109.014 | 2.27x | 49.174 | 109.003 | 2.22x |
| traintest_seeded_n1000000 | 20.681 | 19.218 | 0.93x | 20.954 | 19.216 | 0.92x |
| stratified_traintest_n1000000 | 93.697 | 234.430 | 2.50x | 94.239 | 234.399 | 2.49x |
| group_kfold_n1000000 | 14.827 | 53.277 | 3.59x | 15.585 | 53.270 | 3.42x |
| stratified_group_kfold_n1000000 | 27.405 | 3216.178 | 117.36x | 28.482 | 3215.802 | 112.91x |
| timeseries_n1000000 | 0.000 | 0.328 | 3097.57x | 0.000 | 0.328 | 3097.93x |
| repeated_kfold_n1000000 | 106.545 | 173.143 | 1.63x | 108.103 | 173.125 | 1.60x |

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
| welch_t_n1000 | 0.005 | 0.823 | 151.96x | 0.005 | 0.823 | 151.96x |
| mann_whitney_n1000 | 0.031 | 0.748 | 24.50x | 0.031 | 0.748 | 24.50x |
| chi_square_n1000 | 0.000 | 0.340 | 1673.73x | 0.000 | 0.340 | 1673.84x |
| fligner_n1000 | 0.075 | 0.820 | 11.01x | 0.075 | 0.820 | 11.01x |
| anderson_ksamp_n1000 | 0.167 | 0.396 | 2.37x | 0.167 | 0.396 | 2.37x |
| spearman_matrix_n1000 | 0.094 | 0.931 | 9.90x | 0.094 | 0.931 | 9.90x |
| pointbiserial_n1000 | 0.010 | 0.573 | 55.77x | 0.010 | 0.573 | 55.77x |
| welch_t_n10000 | 0.052 | 0.872 | 16.88x | 0.052 | 0.872 | 16.88x |
| mann_whitney_n10000 | 0.706 | 2.840 | 4.03x | 0.705 | 2.840 | 4.03x |
| chi_square_n10000 | 0.001 | 0.349 | 254.72x | 0.001 | 0.349 | 254.72x |
| fligner_n10000 | 1.405 | 3.250 | 2.31x | 1.471 | 3.250 | 2.21x |
| anderson_ksamp_n10000 | 2.034 | 4.021 | 1.98x | 2.156 | 4.020 | 1.86x |
| spearman_matrix_n10000 | 2.199 | 4.781 | 2.17x | 2.276 | 4.780 | 2.10x |
| pointbiserial_n10000 | 0.099 | 0.646 | 6.52x | 0.099 | 0.646 | 6.52x |
| welch_t_n100000 | 0.517 | 1.246 | 2.41x | 0.517 | 1.245 | 2.41x |
| mann_whitney_n100000 | 7.745 | 28.716 | 3.71x | 7.744 | 28.714 | 3.71x |
| chi_square_n100000 | 0.014 | 0.366 | 26.62x | 0.014 | 0.366 | 26.62x |
| fligner_n100000 | 14.486 | 32.957 | 2.28x | 14.902 | 32.955 | 2.21x |
| anderson_ksamp_n100000 | 15.603 | 44.008 | 2.82x | 15.995 | 44.005 | 2.75x |
| spearman_matrix_n100000 | 18.078 | 54.171 | 3.00x | 18.500 | 54.168 | 2.93x |
| pointbiserial_n100000 | 1.288 | 1.440 | 1.12x | 1.302 | 5.743 | 4.41x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.270 | 0.465 | 1.72x | 0.273 | 0.465 | 1.70x |
| polynomial_n10000 | 0.737 | 1.032 | 1.40x | 0.742 | 1.032 | 1.39x |
| kbins_fit_n10000 | 1.450 | 2.622 | 1.81x | 1.450 | 2.622 | 1.81x |
| kbins_n10000 | 0.722 | 4.468 | 6.18x | 0.815 | 4.467 | 5.48x |
| quantile_fit_n10000 | 1.543 | 3.797 | 2.46x | 1.543 | 3.796 | 2.46x |
| quantile_n10000 | 1.838 | 1.702 | 0.93x | 1.842 | 1.702 | 0.92x |
| power_fit_n10000 | 6.187 | 135.365 | 21.88x | 6.190 | 135.343 | 21.87x |
| power_n10000 | 1.545 | 1.518 | 0.98x | 1.549 | 1.518 | 0.98x |
| knn_impute_n10000 | 7.351 | 23.650 | 3.22x | 7.350 | 94.270 | 12.83x |
| label_n10000 | 1.155 | 0.764 | 0.66x | 1.156 | 0.764 | 0.66x |
| normalize_n100000 | 1.423 | 1.859 | 1.31x | 1.799 | 1.859 | 1.03x |
| polynomial_n100000 | 2.875 | 7.192 | 2.50x | 3.515 | 7.191 | 2.05x |
| kbins_fit_n100000 | 14.854 | 8.270 | 0.56x | 15.666 | 8.270 | 0.53x |
| kbins_n100000 | 6.545 | 12.597 | 1.92x | 7.223 | 12.596 | 1.74x |
| quantile_fit_n100000 | 15.051 | 18.098 | 1.20x | 15.798 | 18.097 | 1.15x |
| quantile_n100000 | 17.013 | 14.897 | 0.88x | 17.406 | 14.896 | 0.86x |
| power_fit_n100000 | 67.106 | 917.186 | 13.67x | 69.896 | 917.142 | 13.12x |
| power_n100000 | 14.163 | 10.071 | 0.71x | 14.613 | 10.071 | 0.69x |
| knn_impute_n100000 | 7.367 | 19.384 | 2.63x | 7.366 | 77.267 | 10.49x |
| label_n100000 | 15.419 | 7.292 | 0.47x | 15.752 | 7.292 | 0.46x |

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
- `VectorMathBenchmarks`
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

**New tonight: 6.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MyersGateBenchmarks` | Band=64 | `Kernel` | drift | 0.05 | 0.07 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | drift | 6.291 | 5.026 | +25% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=1500, Method=complete | `Aglomera_GetClustering` | drift | 115.89 | 148.9 | -22% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.94 | 1.19 | -21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 2.44 | 3.06 | -20% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=500, Method=ward | `Aglomera_GetClustering` | drift | 31.02 | 38.8 | -20% | 20% |

**Still away from their median: 40.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | drift | 44.04 | 7.64 | +435% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | drift | 24.38 | 4.53 | +425% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | drift | 14.07 | 3.92 | +259% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | drift | 25.27 | 7.01 | +256% | 20% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | drift | 15.09 | 6.98 | +115% | 20% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | drift | 15.24 | 7.04 | +112% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.49 | 2.9 | +53% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.29 | 0.19 | +53% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.29 | 0.19 | +53% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 3.38 | 2.19 | +50% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel_Cjk` | drift | 0.3 | 0.2 | +40% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_StudentT` | drift | 2.965 | 4.818 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KolmogorovSmirnov` | drift | 35.495 | 57.615 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_MannWhitney` | drift | 34.449 | 55.807 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_OneWayAnova` | drift | 4.472 | 7.24 | -38% | 20% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | drift | 15.51 | 10.02 | +38% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.87 | 1.35 | +38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KolmogorovSmirnov` | drift | 24.394 | 39.164 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KruskalWallis` | drift | 60.411 | 96.354 | -37% | 20% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | drift | 1.29 | 2.04 | -37% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | drift | 1.419 | 2.243 | -37% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | drift | 0.002 | 0.003 | -33% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | drift | 0.011 | 0.016 | -31% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel` | drift | 0.17 | 0.13 | +31% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.13 | 1.64 | -30% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 0.97 | 1.39 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 2.19 | 3.06 | -28% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.99 | 1.38 | -28% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | drift | 0.026 | 0.036 | -28% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | drift | 1.79 | 2.28 | -26% | 20% |
| `LevenshteinIncumbentBenchmarks` | Length=64 | `Quickenshtein` | drift | 5.19 | 4.08 | +26% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.04 | 1.43 | -26% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 2.15 | 2.89 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.099 | 4.272 | -24% | 20% |
| `LevenshteinIncumbentBenchmarks` | Length=64 | `F23_StringSimilarity` | drift | 38.1 | 30.01 | +24% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 3.37 | 4.42 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 3.06 | 4.11 | -23% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 7.82 | 6.21 | +23% | 20% |
| `MyersGateBenchmarks` | Band=64 | `Kernel_Cjk` | drift | 0.07 | 0.09 | -22% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.96 | 1.25 | -21% | 20% |
