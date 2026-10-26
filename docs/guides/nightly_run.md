# Nightly benchmark run

<!-- nightly-baseline: 6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90 -->
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

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **976.0 ns** |        **226.07 ns** |      **12.39 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         802.8 ns |         54.65 ns |       3.00 ns |     0.82 |    0.01 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,707.0 ns |        181.19 ns |       9.93 ns |     2.77 |    0.03 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,976.8 ns |         57.00 ns |       3.12 ns |     4.08 |    0.05 |   0.1144 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,286.4 ns |        101.46 ns |       5.56 ns |     5.42 |    0.06 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,619.8 ns |      4,209.46 ns |     230.73 ns |    10.88 |    0.24 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,384.9 ns |        461.76 ns |      25.31 ns |     5.52 |    0.07 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,257.1 ns** |        **326.73 ns** |      **17.91 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,943.6 ns |      1,046.06 ns |      57.34 ns |     0.66 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,130,157.6 ns |    110,060.26 ns |   6,032.77 ns |    13.10 |    0.06 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,949,249.0 ns |     55,327.44 ns |   3,032.68 ns |    22.60 |    0.03 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,129,965.0 ns |     58,584.74 ns |   3,211.23 ns |    24.69 |    0.03 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 287,745,102.5 ns | 10,514,446.25 ns | 576,332.28 ns | 3,335.90 |    5.82 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,147,648.3 ns |    461,936.63 ns |  25,320.31 ns |    24.90 |    0.25 |        - |        - |        - |         - |          NA |

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **32.61 μs** |  **1.599 μs** | **0.088 μs** |  **1.00** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    35.32 μs |  0.961 μs | 0.053 μs |  1.08 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,992.21 μs** | **54.343 μs** | **2.979 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,459.25 μs | 56.299 μs | 3.086 μs |  0.73 | 7.8125 |  156.3 KB |        1.00 |

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

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **1,045.0 ns** |     **205.64 ns** |     **11.27 ns** |   **1.00** |    **0.01** | **0.0038** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,193.9 ns |     216.01 ns |     11.84 ns |   2.10 |    0.02 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,234.7 ns |     741.96 ns |     40.67 ns |   2.14 |    0.04 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,860.7 ns |     373.49 ns |     20.47 ns |   3.69 |    0.04 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,417.1 ns |   1,038.56 ns |     56.93 ns |   7.10 |    0.08 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,509.8 ns |     406.89 ns |     22.30 ns |   9.10 |    0.09 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,494.5 ns |     180.36 ns |      9.89 ns |   2.39 |    0.02 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,083.1 ns |   1,127.79 ns |     61.82 ns |   3.91 |    0.06 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |       959.8 ns |      59.89 ns |      3.28 ns |   0.92 |    0.01 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,181.6 ns |      64.48 ns |      3.53 ns |   3.04 |    0.03 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,102.4 ns |      24.20 ns |      1.33 ns |   1.05 |    0.01 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,030.8 ns |      24.63 ns |      1.35 ns |   0.99 |    0.01 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       461.1 ns |      36.16 ns |      1.98 ns |   0.44 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,543.9 ns |      36.04 ns |      1.98 ns |   1.48 |    0.01 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       114.2 ns |      13.34 ns |      0.73 ns |   0.11 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       677.9 ns |      94.13 ns |      5.16 ns |   0.65 |    0.01 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,816.9 ns** |  **13,911.96 ns** |    **762.56 ns** |  **1.000** |    **0.02** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,794.8 ns |   1,555.37 ns |     85.25 ns |  2.940 |    0.03 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   655,774.9 ns |  93,306.52 ns |  5,114.45 ns | 10.784 |    0.14 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,092,403.6 ns | 132,425.92 ns |  7,258.71 ns | 34.409 |    0.39 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,471,748.9 ns | 112,006.05 ns |  6,139.43 ns | 24.202 |    0.28 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,600,483.0 ns | 196,319.90 ns | 10,760.96 ns | 59.208 |    0.66 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,458,182.0 ns |  54,757.46 ns |  3,001.44 ns | 23.979 |    0.26 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,227,932.8 ns | 109,073.10 ns |  5,978.66 ns | 36.637 |    0.40 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    84,857.1 ns |   1,881.43 ns |    103.13 ns |  1.395 |    0.02 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   268,720.9 ns |   7,741.96 ns |    424.36 ns |  4.419 |    0.05 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   187,360.4 ns |   2,417.63 ns |    132.52 ns |  3.081 |    0.03 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   380,599.3 ns |  22,372.01 ns |  1,226.29 ns |  6.259 |    0.07 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       462.5 ns |       4.90 ns |      0.27 ns |  0.008 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,537.7 ns |      92.43 ns |      5.07 ns |  0.025 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       113.3 ns |       3.29 ns |      0.18 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       677.5 ns |      89.81 ns |      4.92 ns |  0.011 |    0.00 | 0.0572 |     968 B |       15.12 |

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

| Method                   | GroupSize | Mean           | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,028.5 ns** |     **246.80 ns** |     **13.53 ns** |  **1.00** |    **0.01** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,185.0 ns |     508.89 ns |     27.89 ns |  2.06 |    0.02 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       606.2 ns |      31.03 ns |      1.70 ns |  0.30 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,201.0 ns |     380.41 ns |     20.85 ns |  0.59 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       759.8 ns |      61.52 ns |      3.37 ns |  0.37 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,699.0 ns |     144.89 ns |      7.94 ns |  2.32 |    0.01 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,622.6 ns |     671.44 ns |     36.80 ns |  4.25 |    0.03 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,285.8 ns |     392.19 ns |     21.50 ns |  3.10 |    0.02 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,104.6 ns |     490.87 ns |     26.91 ns |  2.02 |    0.02 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **376,435.3 ns** |  **51,255.43 ns** |  **2,809.48 ns** | **1.000** |    **0.01** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,342,705.8 ns | 107,326.20 ns |  5,882.91 ns | 3.567 |    0.03 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,358.0 ns |   2,739.76 ns |    150.18 ns | 0.150 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,462.4 ns |   3,133.55 ns |    171.76 ns | 0.299 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       992.5 ns |      76.47 ns |      4.19 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   929,410.6 ns | 152,305.08 ns |  8,348.36 ns | 2.469 |    0.02 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,403,894.6 ns | 194,386.70 ns | 10,654.99 ns | 3.730 |    0.03 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,726.7 ns |     723.11 ns |     39.64 ns | 0.028 |    0.00 |       - |      - |      48 B |       0.000 |

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

| Method                 | Rows | Method   | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,912.4 μs** |     **55.33 μs** |     **3.03 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    89,160.8 μs |  4,336.23 μs |   237.68 μs |  46.62 |    0.13 |  3666.6667 |  1500.0000 |  833.3333 |  59880.82 KB |       58.56 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,815.0 μs** |    **123.57 μs** |     **6.77 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    97,696.2 μs | 37,789.69 μs | 2,071.38 μs |  53.83 |    1.00 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **773.2 μs** |     **44.31 μs** |     **2.43 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   145,819.5 μs | 16,000.24 μs |   877.03 μs | 188.59 |    1.11 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,203.7 μs** |    **276.77 μs** |    **15.17 μs** |   **1.00** |    **0.01** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    68,787.2 μs |  2,250.43 μs |   123.35 μs |  31.21 |    0.19 |  3875.0000 |  1750.0000 |  875.0000 |  64191.18 KB |       62.78 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **14,155.4 μs** |  **1,804.49 μs** |    **98.91 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,556,670.1 μs | 51,185.76 μs | 2,805.66 μs | 109.97 |    0.69 | 35000.0000 | 10000.0000 | 4000.0000 | 537153.95 KB |       60.18 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **13,020.9 μs** |  **2,300.27 μs** |   **126.09 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,862,789.3 μs | 57,438.31 μs | 3,148.39 μs | 143.07 |    1.22 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,514.2 μs** |    **108.96 μs** |     **5.97 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,147,772.4 μs | 23,713.74 μs | 1,299.83 μs | 483.22 |    0.42 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.18 KB |    8,147.90 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,751.8 μs** |  **1,486.11 μs** |    **81.46 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,733,898.7 μs | 41,009.86 μs | 2,247.89 μs | 103.51 |    0.45 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

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
| CountLoadAsync2M | 25.50 ms | 10.984 ms | 0.602 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 45.82 ms | 25.112 ms | 1.376 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.93 MB |
| CountLoad4M      | 44.03 ms |  4.873 ms | 0.267 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

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
| CountFitLongDocuments | 181,371.85 μs | 13,505.620 μs |   740.289 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     455.97 μs |     53.798 μs |     2.949 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     436.75 μs |    159.110 μs |     8.721 μs |   10.7422 |    4.3945 |   4.3945 |   739.44 KB |
| TfidfSave45K          |      50.12 μs |      3.901 μs |     0.214 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      51.37 μs |      7.071 μs |     0.388 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     287.84 μs |     49.339 μs |     2.704 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     288.16 μs |     30.893 μs |     1.693 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  26,922.25 μs | 29,569.985 μs | 1,620.831 μs |  593.7500 |  562.5000 | 187.5000 | 22049.03 KB |
| CountLoadAsync4M      |  54,668.58 μs | 45,955.301 μs | 2,518.965 μs | 1181.8182 | 1090.9091 | 363.6364 | 44983.59 KB |
| CountLoad4M           |  52,611.13 μs | 18,910.436 μs | 1,036.545 μs | 1181.8182 | 1090.9091 | 363.6364 | 44976.85 KB |

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

| Method                 | Mean          | Error          | StdDev        | Gen0      | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|---------------:|--------------:|----------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     16.611 μs |     19.8519 μs |     1.0881 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    506.632 μs |    104.0304 μs |     5.7023 μs |   35.1563 |  29.2969 |  29.2969 |   740.12 KB |
| HashingLoadUndeclared  |      5.224 μs |      0.8214 μs |     0.0450 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     34.988 μs |      7.1882 μs |     0.3940 μs |    7.3853 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    125.525 μs |     23.5166 μs |     1.2890 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    851.473 μs |    268.1056 μs |    14.6958 μs |   38.0859 |  35.1563 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,795.180 μs | 11,153.0237 μs |   611.3349 μs |  515.6250 | 500.0000 | 296.8750 | 10793.88 KB |
| CountSaveAsync1M       |  2,278.434 μs |  2,674.8973 μs |   146.6201 μs |  121.0938 | 113.2813 | 113.2813 |  3162.15 KB |
| IndexSaveAsyncLongHead |  3,397.409 μs |    855.9961 μs |    46.9200 μs |   93.7500 |  89.8438 |  89.8438 |  2124.16 KB |
| CountLoadUndeclared2M  | 20,637.054 μs | 31,767.4113 μs | 1,741.2790 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.14 KB |

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
| **UnitLoop**           | **1**          |   **5.789 μs** |   **0.0477 μs** | **0.0026 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.163 μs |   0.0660 μs | 0.0036 μs |  1.06 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   5.983 μs |   0.6593 μs | 0.0361 μs |  1.03 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **57.058 μs** |   **1.5683 μs** | **0.0860 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.895 μs |   0.8671 μs | 0.0475 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.730 μs |   1.1609 μs | 0.0636 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **225.755 μs** |   **7.4204 μs** | **0.4067 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  80.865 μs |   2.1210 μs | 0.1163 μs |  0.36 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  71.591 μs |   9.7087 μs | 0.5322 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **918.127 μs** | **104.8100 μs** | **5.7450 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 322.215 μs |  16.5282 μs | 0.9060 μs |  0.35 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 273.585 μs |  20.8198 μs | 1.1412 μs |  0.30 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

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
| **LengthFilteredScan** | **1**      | **clustered** | **133.85 ms** | **20.957 ms** | **1.149 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  66.63 ms |  3.436 ms | 0.188 ms |  0.50 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **140.20 ms** | **68.448 ms** | **3.752 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  61.18 ms |  1.528 ms | 0.084 ms |  0.44 |    0.01 |  116.29 KB |        4.87 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **202.05 ms** | **20.167 ms** | **1.105 ms** |  **1.00** |    **0.01** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 242.24 ms |  7.317 ms | 0.401 ms |  1.20 |    0.01 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **205.85 ms** |  **3.036 ms** | **0.166 ms** |  **1.00** |    **0.00** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 209.99 ms |  7.323 ms | 0.401 ms |  1.02 |    0.00 |  192.25 KB |        3.52 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **253.31 ms** |  **6.674 ms** | **0.366 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 338.59 ms | 27.245 ms | 1.493 ms |  1.34 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **259.83 ms** | **29.622 ms** | **1.624 ms** |  **1.00** |    **0.01** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 312.78 ms | 12.030 ms | 0.659 ms |  1.20 |    0.01 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **295.12 ms** | **19.856 ms** | **1.088 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 395.64 ms | 17.212 ms | 0.943 ms |  1.34 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **298.62 ms** |  **9.697 ms** | **0.532 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 391.35 ms | 13.948 ms | 0.765 ms |  1.31 |    0.00 |  7964.5 KB |        1.44 |

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

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **45.04 μs** |      **0.392 μs** |     **0.021 μs** |         **-** |
| Cjk    | 1000   |      56.52 μs |      3.594 μs |     0.197 μs |         - |
| **Latin**  | **10000**  |   **4,105.28 μs** |    **112.415 μs** |     **6.162 μs** |         **-** |
| Cjk    | 10000  |   6,827.95 μs |    960.432 μs |    52.644 μs |         - |
| **Latin**  | **65536**  | **185,379.90 μs** | **18,897.669 μs** | **1,035.845 μs** |         **-** |

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

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 28.52 ms | 0.735 ms | 0.040 ms |  1.00 |    0.00 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 83.45 ms | 9.234 ms | 0.506 ms |  2.93 |    0.02 | 1666.6667 |  28.47 MB |        3.48 |

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
| **BpeOnOnePathologicalToken** | **512**    |  **18.57 μs** |  **1.900 μs** | **0.104 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **68.16 μs** | **15.904 μs** | **0.872 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **182.25 μs** | **17.894 μs** | **0.981 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **389.92 μs** | **69.163 μs** | **3.791 μs** | **3.4180** |  **56.63 KB** |

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
| EncodeUnseenProse | 9.360 ms | 3.773 ms | 0.2068 ms |    1.7 MB |

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
| **DpGroup**    | **cjk**      |  **18.93 μs** |  **0.862 μs** | **0.047 μs** |         **-** |
| MyersGroup | cjk      | 239.28 μs | 39.780 μs | 2.180 μs |         - |
| **DpGroup**    | **latin**    |  **10.13 μs** |  **2.244 μs** | **0.123 μs** |         **-** |
| MyersGroup | latin    | 133.14 μs | 12.831 μs | 0.703 μs |         - |

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
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **306.95 μs** | **21.297 μs** | **1.167 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 305.88 μs | 29.435 μs | 1.613 μs |  1.00 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **339.16 μs** | **47.982 μs** | **2.630 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 330.02 μs | 16.767 μs | 0.919 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **319.09 μs** | **11.849 μs** | **0.649 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 314.68 μs | 68.791 μs | 3.771 μs |  0.99 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **276.40 μs** |  **6.405 μs** | **0.351 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 309.56 μs | 20.649 μs | 1.132 μs |  1.12 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **304.02 μs** | **77.651 μs** | **4.256 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 311.50 μs | 40.157 μs | 2.201 μs |  1.02 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **165.92 μs** | **24.524 μs** | **1.344 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 323.16 μs | 23.549 μs | 1.291 μs |  1.95 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **234.68 μs** | **64.934 μs** | **3.559 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 307.09 μs | 34.070 μs | 1.867 μs |  1.31 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **91.47 μs** | **27.527 μs** | **1.509 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 320.96 μs | 31.997 μs | 1.754 μs |  3.51 |    0.05 | 1.9531 |  32.02 KB |        1.00 |

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **181.6 μs** |  **30.75 μs** |  **1.69 μs** |  **1.00** |    **0.01** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   170.3 μs |   4.86 μs |  0.27 μs |  0.94 |    0.01 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   474.3 μs |  11.30 μs |  0.62 μs |  2.61 |    0.02 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,271.9 μs** |  **14.86 μs** |  **0.81 μs** |  **1.00** |    **0.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,157.4 μs | 281.09 μs | 15.41 μs |  0.97 |    0.00 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,597.4 μs | 448.45 μs | 24.58 μs |  2.32 |    0.01 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

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

| Method       | Shape     | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **151.54 ms** |  **25.826 ms** | **1.416 ms** |  **1.00** |    **0.01** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,320.04 ms | 150.143 ms | 8.230 ms | 15.31 |    0.13 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |            |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **53.57 ms** |   **4.758 ms** | **0.261 ms** |  **1.00** |    **0.01** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   814.19 ms |  35.529 ms | 1.947 ms | 15.20 |    0.07 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

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
| **Lodestar_Fit**             | **20000x2x10** |   **157.27 ms** |  **18.419 ms** |  **1.010 ms** |  **1.00** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,698.99 ms | 360.611 ms | 19.766 ms | 42.60 |    0.26 | 38000.0000 | 14000.0000 | 11000.0000 | 531.88 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,912.37 ms | 194.530 ms | 10.663 ms | 24.88 |    0.15 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **18.46 ms** |   **1.747 ms** |  **0.096 ms** |  **1.00** |    **0.01** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   473.34 ms |  26.183 ms |  1.435 ms | 25.64 |    0.13 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   264.97 ms |  25.665 ms |  1.407 ms | 14.35 |    0.09 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

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
| TruncatedSvd_Rank20                       |  13.051 ms |  4.3378 ms | 0.2378 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 107.298 ms | 20.9823 ms | 1.1501 ms |  8.22 |    0.15 |
| Nmf_Transform                             |   3.379 ms |  0.1071 ms | 0.0059 ms |  0.26 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.528 ms |  1.1711 ms | 0.0642 ms |  1.88 |    0.03 |

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
| Ratio          |    97.60 ns |   6.127 ns |  0.336 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   752.59 ns |  15.582 ns |  0.854 ns |  7.71 |    0.02 |      - |         - |          NA |
| TokenSortRatio |   967.60 ns | 211.029 ns | 11.567 ns |  9.91 |    0.11 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   977.58 ns | 147.565 ns |  8.089 ns | 10.02 |    0.08 | 0.0534 |     896 B |          NA |
| WRatio         | 1,311.23 ns | 209.543 ns | 11.486 ns | 13.43 |    0.11 | 0.0668 |    1120 B |          NA |

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
| Ratio_RepeatedEmoji       |  1,945,204.3646 ns |    20,412.6438 ns |   1,118.8859 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    663,455.2424 ns |   114,003.3782 ns |   6,248.9099 ns |  0.341 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,114,294.7725 ns |   385,805.7049 ns |  21,147.3126 ns |  0.573 |    0.01 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,498,656.4167 ns | 2,988,983.1369 ns | 163,836.2523 ns | 35.728 |    0.08 |       - |       - |  510798 B |        3.18 |
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

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **99.01 ns** |   **1.825 ns** |  **0.100 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    205.86 ns |   8.108 ns |  0.444 ns |  2.08 |    0.00 | 0.0048 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **765.03 ns** | **205.242 ns** | **11.250 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,204.70 ns | 780.893 ns | 42.803 ns | 13.34 |    0.18 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **960.86 ns** | **227.365 ns** | **12.463 ns** |  **1.00** |    **0.02** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,201.18 ns |  94.218 ns |  5.164 ns |  2.29 |    0.03 | 0.1144 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,234.32 ns** | **111.727 ns** |  **6.124 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,132.41 ns | 405.663 ns | 22.236 ns |  4.16 |    0.02 | 0.1831 |    3096 B |        2.76 |

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

| Method                     | Length | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.30 ns** |     **0.099 ns** |   **0.005 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.47 ns |     8.234 ns |   0.451 ns |  1.58 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.48 ns |     1.260 ns |   0.069 ns |  1.04 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.81 ns |     0.157 ns |   0.009 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.46 ns** |     **1.032 ns** |   **0.057 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.62 ns |     3.414 ns |   0.187 ns |  1.46 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    29.23 ns |     1.084 ns |   0.059 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    28.87 ns |     0.475 ns |   0.026 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **31.01 ns** |     **0.601 ns** |   **0.033 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.49 ns |     2.294 ns |   0.126 ns |  1.47 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    31.53 ns |     2.211 ns |   0.121 ns |  1.02 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.53 ns |     0.547 ns |   0.030 ns |  0.98 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **34.63 ns** |     **0.970 ns** |   **0.053 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    54.33 ns |    25.993 ns |   1.425 ns |  1.57 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.52 ns |     0.445 ns |   0.024 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    33.34 ns |     4.216 ns |   0.231 ns |  0.96 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **61.78 ns** |     **3.834 ns** |   **0.210 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    62.49 ns |     2.819 ns |   0.155 ns |  1.01 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.07 ns |     0.372 ns |   0.020 ns |  0.92 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    60.76 ns |     8.522 ns |   0.467 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **63.10 ns** |     **1.777 ns** |   **0.097 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    70.35 ns |     0.697 ns |   0.038 ns |  1.11 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.61 ns |     1.956 ns |   0.107 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |   195.57 ns |     1.994 ns |   0.109 ns |  3.10 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **407.15 ns** |     **8.042 ns** |   **0.441 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   417.50 ns |     9.510 ns |   0.521 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   406.94 ns |    10.419 ns |   0.571 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   407.16 ns |     9.017 ns |   0.494 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,681.55 ns** |    **23.590 ns** |   **1.293 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,852.40 ns |   173.851 ns |   9.529 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,778.34 ns | 3,271.582 ns | 179.326 ns |  1.02 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,687.18 ns |    50.862 ns |   2.788 ns |  1.00 |    0.00 |         - |          NA |

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

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **389.4 ns** |  **19.99 ns** |  **1.10 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    190.1 ns |   1.79 ns |  0.10 ns |  0.49 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,410.9 ns** |  **41.30 ns** |  **2.26 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,058.3 ns | 342.54 ns | 18.78 ns |  1.27 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **13,412.6 ns** | **110.49 ns** |  **6.06 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,480.0 ns | 762.70 ns | 41.81 ns |  2.42 |         - |          NA |

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,623.1 ns** |   **469.69 ns** |  **25.75 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  8,138.0 ns | 1,814.63 ns |  99.47 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,930.1 ns |   692.43 ns |  37.95 ns |  1.38 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,750.0 ns** |   **119.19 ns** |   **6.53 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,501.0 ns |   970.40 ns |  53.19 ns |  0.97 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,368.1 ns | 1,281.79 ns |  70.26 ns |  1.47 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,328.2 ns** | **3,079.92 ns** | **168.82 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,671.0 ns | 2,368.01 ns | 129.80 ns |  0.95 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,523.7 ns | 1,234.81 ns |  67.68 ns |  1.50 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,395.7 ns** | **1,231.87 ns** |  **67.52 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,953.1 ns | 1,214.71 ns |  66.58 ns |  1.10 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,067.8 ns |   971.25 ns |  53.24 ns |  1.68 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,105.5 ns** | **5,604.83 ns** | **307.22 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,911.7 ns | 3,796.63 ns | 208.11 ns |  0.97 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,348.7 ns |   584.84 ns |  32.06 ns |  1.60 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,341.7 ns** | **6,952.49 ns** | **381.09 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,987.9 ns | 5,417.94 ns | 296.98 ns |  0.96 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,783.8 ns | 1,702.12 ns |  93.30 ns |  1.69 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,782.5 ns** |   **138.02 ns** |   **7.57 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,066.4 ns |   274.10 ns |  15.02 ns |  1.08 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,176.6 ns |   212.72 ns |  11.66 ns |  1.90 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,653.1 ns** |   **203.83 ns** |  **11.17 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,614.9 ns |    21.90 ns |   1.20 ns |  0.99 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 10,278.9 ns |   560.84 ns |  30.74 ns |  1.82 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,827.0 ns** |   **160.98 ns** |   **8.82 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,515.8 ns |   122.91 ns |   6.74 ns |  0.96 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,605.1 ns |   725.36 ns |  39.76 ns |  1.74 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,376.5 ns** |    **77.30 ns** |   **4.24 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,141.9 ns |   145.68 ns |   7.99 ns |  0.90 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,023.2 ns |    55.97 ns |   3.07 ns |  2.53 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **3,975.0 ns** |    **68.60 ns** |   **3.76 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,879.8 ns |   156.66 ns |   8.59 ns |  0.72 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,679.3 ns |   712.86 ns |  39.07 ns |  2.18 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,538.7 ns** |   **298.62 ns** |  **16.37 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,823.0 ns |   212.64 ns |  11.66 ns |  0.58 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,258.5 ns |   391.81 ns |  21.48 ns |  1.87 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,583.6 ns** |    **40.08 ns** |   **2.20 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,422.0 ns |   181.07 ns |   9.92 ns |  0.95 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,384.2 ns |   268.92 ns |  14.74 ns |  2.06 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,381.7 ns** |   **177.79 ns** |   **9.75 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,161.7 ns |   201.42 ns |  11.04 ns |  0.77 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,170.6 ns | 1,173.68 ns |  64.33 ns |  1.89 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,804.9 ns** |    **90.10 ns** |   **4.94 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,230.0 ns |   696.08 ns |  38.15 ns |  1.05 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 27,549.7 ns | 3,230.72 ns | 177.09 ns |  3.53 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,353.6 ns** |    **92.54 ns** |   **5.07 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    593.6 ns |    22.00 ns |   1.21 ns |  0.44 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,077.4 ns | 1,584.92 ns |  86.87 ns |  3.75 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,074.4 ns** |   **339.49 ns** |  **18.61 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,415.8 ns |   117.30 ns |   6.43 ns |  0.46 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,792.5 ns |   681.52 ns |  37.36 ns |  2.53 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,331.8 ns** |    **67.57 ns** |   **3.70 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,375.1 ns |   167.50 ns |   9.18 ns |  0.45 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,043.0 ns |   708.38 ns |  38.83 ns |  2.26 |    0.01 | 0.3052 |    5120 B |          NA |

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
| **Lodestar_Fit**     | **10000x16x16** |   **5,512.6 μs** |     **868.55 μs** |    **47.61 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,779.9 μs |   1,579.46 μs |    86.58 μs |  7.04 |    0.05 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  83,549.4 μs |   1,490.05 μs |    81.67 μs | 15.16 |    0.11 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **798.8 μs** |      **49.33 μs** |     **2.70 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,758.5 μs |      29.90 μs |     1.64 μs |  5.96 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,019.5 μs |     346.15 μs |    18.97 μs |  3.78 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,732.2 μs** |   **1,306.73 μs** |    **71.63 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 445,624.9 μs |  53,864.19 μs | 2,952.48 μs | 16.67 |    0.10 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 739,038.7 μs | 106,134.72 μs | 5,817.60 μs | 27.65 |    0.20 |        - |        - |        - |  199.91 KB |       0.046 |

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.817 ms** | **0.0819 ms** | **0.0045 ms** |  **1.00** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.611 ms | 0.5020 ms | 0.0275 ms |  3.45 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.061 ms** | **0.4006 ms** | **0.0220 ms** |  **1.00** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.012 ms | 4.4672 ms | 0.2449 ms |  4.34 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.230 ms** | **1.4389 ms** | **0.0789 ms** |  **1.00** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 210.018 ms | 9.4672 ms | 0.5189 ms |  4.54 |        - |        - |        - |   36.47 KB |       0.009 |

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
| **Dp**         | **8**    |    **131.45 ns** |      **1.638 ns** |   **0.090 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.74 ns |      3.451 ns |   0.189 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.04 ns |      2.851 ns |   0.156 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     97.42 ns |      0.298 ns |   0.016 ns |  0.74 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **234.26 ns** |    **199.746 ns** |  **10.949 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 12   |     62.52 ns |      1.107 ns |   0.061 ns |  0.27 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    236.71 ns |     46.102 ns |   2.527 ns |  1.01 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    113.20 ns |      4.309 ns |   0.236 ns |  0.48 |    0.02 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **276.40 ns** |    **291.701 ns** |  **15.989 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 14   |     67.85 ns |      1.080 ns |   0.059 ns |  0.25 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    274.68 ns |    246.245 ns |  13.498 ns |  1.00 |    0.06 |         - |          NA |
| Kernel_Cjk | 14   |    112.45 ns |      0.666 ns |   0.036 ns |  0.41 |    0.02 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **359.61 ns** |    **104.282 ns** |   **5.716 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 16   |     70.21 ns |      2.238 ns |   0.123 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    363.92 ns |    209.150 ns |  11.464 ns |  1.01 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    119.30 ns |     14.286 ns |   0.783 ns |  0.33 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **436.70 ns** |     **40.399 ns** |   **2.214 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.83 ns |      0.666 ns |   0.036 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    435.91 ns |     83.138 ns |   4.557 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 18   |    124.57 ns |      1.586 ns |   0.087 ns |  0.29 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **769.70 ns** |     **20.795 ns** |   **1.140 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     79.81 ns |      7.186 ns |   0.394 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    763.29 ns |    202.004 ns |  11.073 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    128.22 ns |      5.763 ns |   0.316 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **997.36 ns** |     **56.541 ns** |   **3.099 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |     86.34 ns |      0.902 ns |   0.049 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    993.53 ns |    218.824 ns |  11.995 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    147.49 ns |      8.502 ns |   0.466 ns |  0.15 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,587.06 ns** |    **222.985 ns** |  **12.223 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    106.71 ns |      1.465 ns |   0.080 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,798.84 ns |    181.072 ns |   9.925 ns |  1.13 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    165.60 ns |      6.066 ns |   0.332 ns |  0.10 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,469.61 ns** |  **1,660.464 ns** |  **91.016 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 48   |    134.77 ns |      4.885 ns |   0.268 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,264.19 ns |  2,256.682 ns | 123.696 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 48   |    240.38 ns |      7.130 ns |   0.391 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,539.41 ns** |  **1,493.001 ns** |  **81.836 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 64   |    162.54 ns |      4.941 ns |   0.271 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,573.96 ns |  1,466.164 ns |  80.365 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 64   |    294.27 ns |     21.937 ns |   1.202 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **12,280.17 ns** | **11,106.317 ns** | **608.775 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 96   |    351.63 ns |      3.570 ns |   0.196 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,103.31 ns |  1,876.865 ns | 102.877 ns |  0.99 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |    945.68 ns |     13.634 ns |   0.747 ns |  0.08 |    0.00 |         - |          NA |

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.70 ns** |     **0.772 ns** |   **0.042 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.82 ns |     0.574 ns |   0.031 ns |  1.00 |         - |          NA |
| Distance_CodePoint         | 8      |    120.72 ns |     4.301 ns |   0.236 ns |  4.89 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.30 ns |     0.477 ns |   0.026 ns |  1.02 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **64**     |    **278.20 ns** |    **10.515 ns** |   **0.576 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    368.63 ns |     6.236 ns |   0.342 ns |  1.33 |         - |          NA |
| Distance_CodePoint         | 64     |    691.08 ns |    12.844 ns |   0.704 ns |  2.48 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    277.85 ns |    42.036 ns |   2.304 ns |  1.00 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **128**    |    **916.34 ns** |     **2.166 ns** |   **0.119 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,946.46 ns |   203.660 ns |  11.163 ns |  2.12 |         - |          NA |
| Distance_CodePoint         | 128    |  1,614.72 ns |    27.142 ns |   1.488 ns |  1.76 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    920.39 ns |    10.810 ns |   0.593 ns |  1.00 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **512**    | **11,105.95 ns** | **1,127.419 ns** |  **61.798 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,687.56 ns |   709.193 ns |  38.873 ns |  1.50 |         - |          NA |
| Distance_CodePoint         | 512    | 13,409.33 ns | 3,223.438 ns | 176.687 ns |  1.21 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,091.50 ns |   156.568 ns |   8.582 ns |  1.00 |         - |          NA |

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
| **Distance_CodePoint** | **16**     | **32**       |     **348.2 ns** |     **10.98 ns** |     **0.60 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     237.5 ns |      8.22 ns |     0.45 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **346.5 ns** |      **5.92 ns** |     **0.32 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     242.3 ns |      6.96 ns |     0.38 ns |  0.70 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **428.7 ns** |     **43.60 ns** |     **2.39 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     320.7 ns |      7.07 ns |     0.39 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **430.8 ns** |     **19.04 ns** |     **1.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     457.8 ns |      8.21 ns |     0.45 ns |  1.06 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **520.7 ns** |      **8.90 ns** |     **0.49 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     414.9 ns |     66.06 ns |     3.62 ns |  0.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **515.7 ns** |     **92.58 ns** |     **5.07 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     419.4 ns |     11.20 ns |     0.61 ns |  0.81 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **593.0 ns** |     **28.74 ns** |     **1.58 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,281.8 ns |     28.94 ns |     1.59 ns |  2.16 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **604.9 ns** |     **22.89 ns** |     **1.25 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,299.6 ns |     41.70 ns |     2.29 ns |  2.15 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,910.0 ns** |     **51.04 ns** |     **2.80 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,442.7 ns |    662.08 ns |    36.29 ns |  2.85 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,911.8 ns** |     **27.96 ns** |     **1.53 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,379.7 ns |    879.38 ns |    48.20 ns |  2.81 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,895.0 ns** |  **1,996.68 ns** |   **109.44 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  68,560.9 ns |    322.63 ns |    17.68 ns |  4.60 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **449,211.0 ns** | **86,783.66 ns** | **4,756.91 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  60,589.4 ns |    423.86 ns |    23.23 ns |  0.13 |    0.00 |         - |          NA |

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
| **Lodestar**             | **8**      |      **24.76 ns** |      **1.808 ns** |     **0.099 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      83.34 ns |      8.899 ns |     0.488 ns |  3.37 |    0.02 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.40 ns |      0.423 ns |     0.023 ns |  3.29 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     175.40 ns |     45.957 ns |     2.519 ns |  7.08 |    0.09 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **279.55 ns** |      **8.640 ns** |     **0.474 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,146.23 ns |  3,097.565 ns |   169.788 ns | 21.99 |    0.53 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,382.13 ns |    114.869 ns |     6.296 ns |  4.94 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,405.38 ns |    822.558 ns |    45.087 ns | 37.22 |    0.15 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **10,999.62 ns** |     **97.551 ns** |     **5.347 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 435,646.46 ns | 28,632.330 ns | 1,569.435 ns | 39.61 |    0.12 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,430.36 ns |  2,971.253 ns |   162.864 ns |  3.40 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 715,632.40 ns | 15,185.731 ns |   832.381 ns | 65.06 |    0.07 |      - |    4161 B |          NA |

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

| Method            | Labels | Mean     | Error     | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **601.4 μs** |  **55.48 μs** | **3.04 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 602.4 μs |  60.20 μs | 3.30 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 603.5 μs |  30.94 μs | 1.70 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **775.2 μs** | **100.34 μs** | **5.50 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 776.0 μs |  10.34 μs | 0.57 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 916.8 μs |  48.71 μs | 2.67 μs | 46.8750 | 18.5547 | 767.27 KB |

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
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **132.42 μs** |      **7.977 μs** |     **0.437 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  97,955.15 μs |  3,742.040 μs |   205.114 μs | 739.71 |    2.50 | 833.3333 | 833.3333 | 833.3333 | 31414.77 KB |   12,806.02 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,909.88 μs** |    **115.611 μs** |     **6.337 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 461,004.42 μs | 38,053.203 μs | 2,085.824 μs | 158.43 |    0.69 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.52 μs** |      **6.550 μs** |     **0.359 μs** |   **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     858.27 μs |     49.797 μs |     2.730 μs |  38.12 |    0.53 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

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
| **Matrix**         | **1000**    | **2**       |     **4,611.4 ns** |     **265.85 ns** |     **14.57 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,116.7 ns |      43.51 ns |      2.38 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       110.2 ns |      21.83 ns |      1.20 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,998.1 ns |     260.45 ns |     14.28 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,334.1 ns |     119.81 ns |      6.57 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **5,235.0 ns** |     **314.08 ns** |     **17.22 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,470.7 ns |     177.66 ns |      9.74 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       109.9 ns |      18.85 ns |      1.03 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,405.0 ns |     651.50 ns |     35.71 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,619.9 ns |     543.50 ns |     29.79 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **461,523.1 ns** |     **580.73 ns** |     **31.83 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   826,840.3 ns |  20,365.25 ns |  1,116.29 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,758.2 ns |     415.32 ns |     22.76 ns |      - |         - |
| F1Macro        | 100000  | 2       |   460,771.4 ns |  36,981.27 ns |  2,027.07 ns |      - |     656 B |
| Report         | 100000  | 2       |   466,590.8 ns |  21,906.49 ns |  1,200.77 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **493,884.8 ns** |  **12,146.13 ns** |    **665.77 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   914,070.9 ns |  28,557.86 ns |  1,565.35 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,568.3 ns |     586.22 ns |     32.13 ns |      - |         - |
| F1Macro        | 100000  | 10      |   512,063.1 ns |  54,839.86 ns |  3,005.96 ns |      - |    1881 B |
| Report         | 100000  | 10      |   502,195.9 ns |  28,348.19 ns |  1,553.86 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,766,127.5 ns** |  **92,641.06 ns** |  **5,077.97 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,352,689.4 ns | 939,257.53 ns | 51,483.87 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,245.8 ns |   8,537.08 ns |    467.95 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,611,104.7 ns | 133,456.30 ns |  7,315.19 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,780,815.3 ns | 194,174.34 ns | 10,643.35 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,931,845.9 ns** | **124,762.01 ns** |  **6,838.63 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,465,678.8 ns | 265,785.98 ns | 14,568.63 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,881.2 ns |  10,875.45 ns |    596.12 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 4,930,626.7 ns | 433,944.95 ns | 23,785.99 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,007,719.4 ns | 235,702.00 ns | 12,919.62 ns |      - |   12878 B |

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
| **Lodestar** | **100000**  | **Bundle**        |  **10,743.04 μs** |    **757.076 μs** |    **41.498 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1292 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,887.49 μs |    742.536 μs |    40.701 μs |     3.34 |    0.01 | 642.8571 | 642.8571 | 642.8571 |  5090419 B |    3,939.95 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **13.06 μs** |      **1.032 μs** |     **0.057 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,519.42 μs |  3,269.462 μs |   179.210 μs | 2,644.05 |   15.50 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **134,085.02 μs** | **27,866.534 μs** | **1,527.459 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 226,990.54 μs | 15,603.037 μs |   855.255 μs |     1.69 |    0.02 |        - |        - |        - | 23228008 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **144.60 μs** |     **22.795 μs** |     **1.249 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 225,464.76 μs | 11,738.121 μs |   643.406 μs | 1,559.34 |   12.34 |        - |        - |        - | 23227992 B |          NA |

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
| **Dp**         | **4**    |     **75.55 ns** |     **5.682 ns** |   **0.311 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 4    |     75.14 ns |     3.313 ns |   0.182 ns |  0.99 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     74.33 ns |     0.437 ns |   0.024 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     75.39 ns |     2.232 ns |   0.122 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **104.13 ns** |     **1.370 ns** |   **0.075 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     90.16 ns |     2.841 ns |   0.156 ns |  0.87 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    104.23 ns |     2.074 ns |   0.114 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 6    |    128.92 ns |     1.793 ns |   0.098 ns |  1.24 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **157.19 ns** |     **4.712 ns** |   **0.258 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.91 ns |     3.081 ns |   0.169 ns |  0.65 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.43 ns |     7.135 ns |   0.391 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    180.36 ns |    23.313 ns |   1.278 ns |  1.15 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **197.06 ns** |     **3.698 ns** |   **0.203 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    110.61 ns |     1.826 ns |   0.100 ns |  0.56 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    196.60 ns |     1.846 ns |   0.101 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    151.20 ns |     3.881 ns |   0.213 ns |  0.77 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **247.79 ns** |    **94.010 ns** |   **5.153 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 12   |    117.74 ns |     2.414 ns |   0.132 ns |  0.48 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    251.68 ns |   109.700 ns |   6.013 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |    161.71 ns |     2.894 ns |   0.159 ns |  0.65 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **420.38 ns** |   **316.544 ns** |  **17.351 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 16   |    133.40 ns |     2.132 ns |   0.117 ns |  0.32 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    411.18 ns |    20.089 ns |   1.101 ns |  0.98 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    180.19 ns |     2.150 ns |   0.118 ns |  0.43 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **842.13 ns** |    **12.094 ns** |   **0.663 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    168.90 ns |     2.047 ns |   0.112 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    842.67 ns |    15.542 ns |   0.852 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    225.83 ns |     1.473 ns |   0.081 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,452.62 ns** |    **98.308 ns** |   **5.389 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    206.09 ns |     2.772 ns |   0.152 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,449.79 ns |    63.583 ns |   3.485 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    268.54 ns |     3.120 ns |   0.171 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,319.95 ns** |    **73.106 ns** |   **4.007 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    275.74 ns |     7.220 ns |   0.396 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,388.45 ns | 2,126.867 ns | 116.581 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    354.73 ns |     3.004 ns |   0.165 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,344.12 ns** | **2,916.569 ns** | **159.867 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 64   |    347.00 ns |    20.445 ns |   1.121 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  7,098.69 ns | 1,334.232 ns |  73.134 ns |  1.12 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    448.85 ns |     0.806 ns |   0.044 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **13,946.13 ns** | **1,540.600 ns** |  **84.445 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    764.41 ns |    82.711 ns |   4.534 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,060.07 ns | 1,274.348 ns |  69.851 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |  1,547.86 ns |    86.895 ns |   4.763 ns |  0.11 |    0.00 |         - |          NA |

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
| **WholeFit**              | **10000**    | **10**         |   **205.84 μs** |   **0.483 μs** |  **0.026 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   228.40 μs |  12.302 μs |  0.674 μs |  1.11 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    58.77 μs |   3.348 μs |  0.184 μs |  0.29 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.72 μs |   7.709 μs |  0.423 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **204.98 μs** |  **18.699 μs** |  **1.025 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   233.51 μs |  58.360 μs |  3.199 μs |  1.14 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    61.38 μs |   3.417 μs |  0.187 μs |  0.30 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   202.11 μs |  13.149 μs |  0.721 μs |  0.99 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,037.93 μs** |  **94.459 μs** |  **5.178 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,041.83 μs | 105.894 μs |  5.804 μs |  1.00 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,080.78 μs | 228.246 μs | 12.511 μs |  0.53 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,971.83 μs |  98.741 μs |  5.412 μs |  0.97 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,064.27 μs** | **178.242 μs** |  **9.770 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,287.62 μs | 331.874 μs | 18.191 μs |  1.11 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,090.15 μs |  28.274 μs |  1.550 μs |  0.53 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,973.06 μs |  73.244 μs |  4.015 μs |  0.96 |      - |     803 B |        1.00 |

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

| Method      | NeedleLength | Mean         | Error     | StdDev    | Allocated |
|------------ |------------- |-------------:|----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.574 μs** | **0.3423 μs** | **0.0188 μs** |         **-** |
| EqualLength | 65           |     2.950 μs | 0.1290 μs | 0.0071 μs |         - |
| **Embedded**    | **128**          |    **40.017 μs** | **0.1594 μs** | **0.0087 μs** |         **-** |
| EqualLength | 128          |     5.373 μs | 0.3299 μs | 0.0181 μs |         - |
| **Embedded**    | **512**          | **1,357.162 μs** | **7.3333 μs** | **0.4020 μs** |         **-** |
| EqualLength | 512          |    44.599 μs | 1.2624 μs | 0.0692 μs |         - |

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
| VocabTxt               |  3.938 ms |  2.2510 ms | 0.1234 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.238 ms |  2.7188 ms | 0.1490 ms | 187.5000 | 171.8750 |  46.8750 |  5852.31 KB |
| TokenizerJsonUnigram   | 12.426 ms |  0.9044 ms | 0.0496 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.134 ms |  1.4763 ms | 0.0809 ms | 132.8125 | 125.0000 |  39.0625 |  4807.68 KB |
| TfidfSave              |  1.523 ms |  0.0867 ms | 0.0048 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.343 ms |  0.4827 ms | 0.0265 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.164 ms |  0.5414 ms | 0.0297 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.307 ms |  0.7939 ms | 0.0435 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.821 ms | 10.6651 ms | 0.5846 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 72.639 ms |  4.0636 ms | 0.2227 ms |        - |        - |        - | 16095.25 KB |

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

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,180.45 μs** | **201.882 μs** | **11.066 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,060.85 μs | 694.054 μs | 38.043 μs |  1.64 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **129.05 μs** |   **6.973 μs** |  **0.382 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    181.21 μs |  10.636 μs |  0.583 μs |  1.40 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,811.30 μs** |  **68.405 μs** |  **3.750 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,675.80 μs |  97.485 μs |  5.343 μs |  0.95 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.39 μs** |   **0.482 μs** |  **0.026 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.66 μs |   1.540 μs |  0.084 μs |  1.15 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

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
| **Mse**      | **100000**  |    **45.39 μs** |   **1.272 μs** |  **0.070 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    45.00 μs |   4.741 μs |  0.260 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.50 μs |   2.476 μs |  0.136 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   532.19 μs | 219.090 μs | 12.009 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **453.40 μs** |  **38.441 μs** |  **2.107 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   451.13 μs |   7.228 μs |  0.396 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,107.11 μs |  40.235 μs |  2.205 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,991.03 μs | 157.951 μs |  8.658 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

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

| Method                 | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,085.2 ns |       170.26 ns |       9.33 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,327.5 ns |       379.37 ns |      20.79 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,105.1 ns |       286.96 ns |      15.73 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        434.1 ns |        49.52 ns |       2.71 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,438,881.6 ns | 3,336,390.56 ns | 182,878.83 ns | 875.0000 | 843.7500 | 609.3750 | 12227071 B |

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
| **Count**                | **200**       | **1.871 ms** | **0.1341 ms** | **0.0074 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.399 ms | 0.0281 ms | 0.0015 ms |  0.75 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.992 ms | 0.1249 ms | 0.0068 ms |  1.07 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.738 ms | 0.2784 ms | 0.0153 ms |  0.93 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **8.997 ms** | **0.8313 ms** | **0.0456 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.51 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.085 ms | 0.0419 ms | 0.0023 ms |  0.79 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.652 ms | 0.9968 ms | 0.0546 ms |  1.07 | 531.2500 | 515.6250 | 515.6250 |  2348.6 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.674 ms | 0.5634 ms | 0.0309 ms |  0.96 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

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
| **Lodestar**     | **WordPiece**     |  **21.82 ms** |  **2.907 ms** | **0.159 ms** |  **1.00** |    **0.01** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  57.70 ms | 16.911 ms | 0.927 ms |  2.64 |    0.04 |  222.2222 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.80 ms** |  **4.907 ms** | **0.269 ms** |  **1.00** |    **0.01** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  55.01 ms |  3.069 ms | 0.168 ms |  1.15 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **87.39 ms** |  **3.957 ms** | **0.217 ms** |  **1.00** |    **0.00** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 287.41 ms | 41.116 ms | 2.254 ms |  3.29 |    0.02 | 3500.0000 |  59.08 MB |        2.08 |

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

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| **Dot**    | **384**  |  **51.17 ns** |  **1.646 ns** | **0.090 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 384  |  85.79 ns | 23.606 ns | 1.294 ns |  1.68 |    0.02 |         - |          NA |
|        |      |           |           |          |       |         |           |             |
| **Dot**    | **768**  |  **94.99 ns** |  **1.070 ns** | **0.059 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 768  |  93.58 ns |  3.230 ns | 0.177 ns |  0.99 |    0.00 |         - |          NA |
|        |      |           |           |          |       |         |           |             |
| **Dot**    | **1024** | **124.23 ns** |  **6.025 ns** | **0.330 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 1024 | 123.56 ns |  5.731 ns | 0.314 ns |  0.99 |    0.00 |         - |          NA |

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.472 ms** |   **0.4851 ms** |  **0.0266 ms** |  **1.00** |    **0.01** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  54.056 ms | 145.4113 ms |  7.9705 ms | 15.57 |    1.99 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.650 ms** |   **0.7951 ms** |  **0.0436 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 402.470 ms | 196.6131 ms | 10.7770 ms | 22.80 |    0.53 | 79000.0000 | 79000.0000 | 79000.0000 | 324.82 MB |       40.05 |

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
| cdist_ratio_n50 | 0.166 | 0.036 | 0.21x | 0.166 | 0.036 | 0.21x |
| cdist_wratio_n50 | 2.221 | 2.484 | 1.12x | 2.221 | 2.483 | 1.12x |
| cdist_ratio_varied_n50 | 0.137 | 0.031 | 0.23x | 0.137 | 0.031 | 0.23x |
| cdist_ratio_varied_cutoff90_n50 | 0.058 | 0.031 | 0.54x | 0.058 | 0.031 | 0.54x |
| cdist_ratio_n200 | 2.782 | 0.439 | 0.16x | 2.784 | 0.439 | 0.16x |
| cdist_wratio_n200 | 33.941 | 37.618 | 1.11x | 33.940 | 37.616 | 1.11x |
| cdist_ratio_varied_n200 | 2.320 | 0.386 | 0.17x | 2.323 | 0.386 | 0.17x |
| cdist_ratio_varied_cutoff90_n200 | 0.990 | 0.386 | 0.39x | 0.992 | 0.386 | 0.39x |
| cdist_ratio_n500 | 16.634 | 2.651 | 0.16x | 16.648 | 2.651 | 0.16x |
| cdist_wratio_n500 | 204.162 | 228.844 | 1.12x | 204.317 | 228.837 | 1.12x |
| cdist_ratio_varied_n500 | 13.997 | 2.297 | 0.16x | 14.090 | 2.297 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 5.452 | 2.297 | 0.42x | 5.539 | 2.297 | 0.41x |

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
| latin | 8 | 117.5 | 27.0 | 4.34x C# faster |
| latin | 32 | 187.5 | 86.8 | 2.16x C# faster |
| latin | 128 | 486.8 | 422.8 | 1.15x C# faster |
| latin | 512 | 4492.4 | 4955.6 | 1.10x Py faster |
| cjk | 8 | 152.2 | 27.7 | 5.49x C# faster |
| cjk | 32 | 368.0 | 230.0 | 1.60x C# faster |
| cjk | 128 | 1920.8 | 1385.0 | 1.39x C# faster |
| cjk | 512 | 14851.0 | 8722.5 | 1.70x C# faster |

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
| latin | 8 | 149.8 | 18.8 | 7.95x C# faster |
| latin | 32 | 284.6 | 158.6 | 1.79x C# faster |
| latin | 128 | 1686.9 | 904.6 | 1.86x C# faster |
| latin | 512 | 14053.8 | 11816.7 | 1.19x C# faster |
| cjk | 8 | 158.9 | 18.4 | 8.66x C# faster |
| cjk | 32 | 384.3 | 287.0 | 1.34x C# faster |
| cjk | 128 | 2854.8 | 1987.1 | 1.44x C# faster |
| cjk | 512 | 23729.9 | 16900.3 | 1.40x C# faster |

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
| confusion_matrix_n1000_k2 | 0.005 | 1.006 | 216.44x | 0.005 | 1.006 | 216.41x |
| accuracy_n1000_k2 | 0.000 | 0.538 | 4041.51x | 0.000 | 0.538 | 4040.43x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.851 | 376.02x | 0.005 | 1.851 | 375.98x |
| classification_report_n1000_k2 | 0.007 | 7.110 | 964.70x | 0.007 | 7.109 | 964.69x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.962 | 109.58x | 0.018 | 1.962 | 109.59x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.069 | 228.43x | 0.005 | 1.069 | 228.41x |
| matthews_n1000_k2 | 0.005 | 2.060 | 440.04x | 0.005 | 2.060 | 440.01x |
| cohen_kappa_n1000_k2 | 0.005 | 1.114 | 239.06x | 0.005 | 1.113 | 239.06x |
| mse_n1000_k2 | 0.000 | 0.313 | 645.46x | 0.000 | 0.313 | 645.27x |
| mae_n1000_k2 | 0.000 | 0.312 | 647.56x | 0.000 | 0.312 | 647.51x |
| median_ae_n1000_k2 | 0.005 | 0.327 | 69.51x | 0.005 | 0.327 | 69.51x |
| r2_n1000_k2 | 0.001 | 0.378 | 318.93x | 0.001 | 0.378 | 318.95x |
| confusion_matrix_n1000_k10 | 0.005 | 1.005 | 198.48x | 0.005 | 1.005 | 198.48x |
| accuracy_n1000_k10 | 0.000 | 0.536 | 3978.14x | 0.000 | 0.536 | 3977.71x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.842 | 317.96x | 0.006 | 1.841 | 317.97x |
| classification_report_n1000_k10 | 0.012 | 7.162 | 619.74x | 0.012 | 7.162 | 619.76x |
| roc_auc_ovr_macro_n1000_k10 | 0.528 | 10.116 | 19.17x | 0.528 | 10.114 | 19.17x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.085 | 194.77x | 0.006 | 1.085 | 194.77x |
| matthews_n1000_k10 | 0.006 | 2.069 | 373.69x | 0.006 | 2.069 | 373.70x |
| cohen_kappa_n1000_k10 | 0.006 | 1.114 | 186.33x | 0.006 | 1.114 | 186.33x |
| mse_n1000_k10 | 0.000 | 0.316 | 650.57x | 0.000 | 0.316 | 650.52x |
| mae_n1000_k10 | 0.000 | 0.315 | 652.89x | 0.000 | 0.315 | 652.79x |
| median_ae_n1000_k10 | 0.005 | 0.328 | 69.35x | 0.005 | 0.328 | 69.35x |
| r2_n1000_k10 | 0.001 | 0.381 | 320.33x | 0.001 | 0.381 | 320.28x |
| confusion_matrix_n100000_k2 | 0.458 | 11.031 | 24.07x | 0.458 | 11.029 | 24.07x |
| accuracy_n100000_k2 | 0.013 | 3.779 | 295.49x | 0.013 | 3.778 | 295.47x |
| precision_recall_f1_macro_n100000_k2 | 0.461 | 12.382 | 26.87x | 0.461 | 12.381 | 26.87x |
| classification_report_n100000_k2 | 0.465 | 27.254 | 58.66x | 0.465 | 27.252 | 58.67x |
| roc_auc_binary_n100000_k2 | 4.342 | 26.750 | 6.16x | 4.342 | 26.746 | 6.16x |
| balanced_accuracy_n100000_k2 | 0.460 | 10.842 | 23.59x | 0.460 | 10.841 | 23.59x |
| matthews_n100000_k2 | 0.461 | 21.593 | 46.81x | 0.461 | 21.592 | 46.81x |
| cohen_kappa_n100000_k2 | 0.459 | 10.866 | 23.66x | 0.459 | 10.865 | 23.66x |
| mse_n100000_k2 | 0.046 | 0.450 | 9.88x | 0.046 | 0.450 | 9.88x |
| mae_n100000_k2 | 0.045 | 0.444 | 9.91x | 0.045 | 0.444 | 9.91x |
| median_ae_n100000_k2 | 0.585 | 1.803 | 3.08x | 0.607 | 1.803 | 2.97x |
| r2_n100000_k2 | 0.110 | 0.672 | 6.09x | 0.110 | 0.672 | 6.09x |
| confusion_matrix_n100000_k10 | 0.492 | 10.810 | 21.97x | 0.492 | 10.809 | 21.97x |
| accuracy_n100000_k10 | 0.013 | 3.790 | 299.58x | 0.013 | 3.790 | 299.55x |
| precision_recall_f1_macro_n100000_k10 | 0.482 | 13.068 | 27.09x | 0.482 | 13.065 | 27.08x |
| classification_report_n100000_k10 | 0.495 | 30.073 | 60.71x | 0.495 | 30.069 | 60.71x |
| roc_auc_ovr_macro_n100000_k10 | 35.361 | 219.441 | 6.21x | 35.357 | 219.422 | 6.21x |
| balanced_accuracy_n100000_k10 | 0.478 | 10.884 | 22.76x | 0.478 | 10.883 | 22.76x |
| matthews_n100000_k10 | 0.484 | 22.331 | 46.16x | 0.484 | 22.328 | 46.15x |
| cohen_kappa_n100000_k10 | 0.485 | 10.896 | 22.47x | 0.485 | 10.895 | 22.47x |
| mse_n100000_k10 | 0.046 | 0.473 | 10.35x | 0.046 | 0.473 | 10.35x |
| mae_n100000_k10 | 0.045 | 0.466 | 10.35x | 0.045 | 0.466 | 10.34x |
| median_ae_n100000_k10 | 0.623 | 1.809 | 2.91x | 0.675 | 1.809 | 2.68x |
| r2_n100000_k10 | 0.110 | 0.723 | 6.55x | 0.110 | 0.723 | 6.55x |
| confusion_matrix_n1000000_k2 | 4.614 | 100.729 | 21.83x | 4.614 | 100.712 | 21.83x |
| accuracy_n1000000_k2 | 0.137 | 33.348 | 244.28x | 0.137 | 33.343 | 244.26x |
| precision_recall_f1_macro_n1000000_k2 | 4.574 | 109.368 | 23.91x | 4.573 | 109.357 | 23.91x |
| classification_report_n1000000_k2 | 4.591 | 212.385 | 46.26x | 4.591 | 212.364 | 46.25x |
| roc_auc_binary_n1000000_k2 | 56.250 | 292.531 | 5.20x | 56.245 | 292.494 | 5.20x |
| balanced_accuracy_n1000000_k2 | 4.604 | 100.687 | 21.87x | 4.604 | 100.668 | 21.87x |
| matthews_n1000000_k2 | 4.610 | 202.777 | 43.99x | 4.610 | 202.775 | 43.99x |
| cohen_kappa_n1000000_k2 | 4.611 | 100.706 | 21.84x | 4.611 | 100.696 | 21.84x |
| mse_n1000000_k2 | 0.455 | 2.430 | 5.34x | 0.455 | 2.429 | 5.34x |
| mae_n1000000_k2 | 0.451 | 2.435 | 5.40x | 0.451 | 2.434 | 5.40x |
| median_ae_n1000000_k2 | 5.487 | 14.510 | 2.64x | 5.590 | 14.509 | 2.60x |
| r2_n1000000_k2 | 1.099 | 4.206 | 3.83x | 1.099 | 4.205 | 3.83x |
| confusion_matrix_n1000000_k10 | 4.854 | 101.141 | 20.84x | 4.854 | 101.114 | 20.83x |
| accuracy_n1000000_k10 | 0.120 | 33.412 | 279.39x | 0.120 | 33.405 | 279.34x |
| precision_recall_f1_macro_n1000000_k10 | 4.906 | 115.221 | 23.49x | 4.906 | 115.205 | 23.48x |
| classification_report_n1000000_k10 | 4.918 | 238.165 | 48.42x | 4.918 | 238.133 | 48.42x |
| balanced_accuracy_n1000000_k10 | 4.904 | 101.066 | 20.61x | 4.903 | 101.060 | 20.61x |
| matthews_n1000000_k10 | 4.910 | 209.899 | 42.75x | 4.909 | 209.877 | 42.75x |
| cohen_kappa_n1000000_k10 | 4.907 | 101.095 | 20.60x | 4.906 | 101.078 | 20.60x |
| mse_n1000000_k10 | 0.435 | 2.508 | 5.77x | 0.435 | 2.508 | 5.76x |
| mae_n1000000_k10 | 0.449 | 2.485 | 5.53x | 0.449 | 2.484 | 5.53x |
| median_ae_n1000000_k10 | 5.455 | 14.582 | 2.67x | 5.542 | 14.580 | 2.63x |
| r2_n1000000_k10 | 1.118 | 4.300 | 3.85x | 1.118 | 4.300 | 3.85x |

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
| vocab_txt | 5.450 | 9.673 | 1.77x | 5.847 | 9.671 | 1.65x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.028 | 16.078 | 1.34x | 12.424 | 16.078 | 1.29x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.706 | 38.487 | 2.81x | 14.050 | 38.483 | 2.74x | 1,990,038 | 1,990,038 |
| spiece_model | 7.154 | 28.272 | 3.95x | 7.369 | 28.269 | 3.84x | 533,084 | 533,084 |
| tfidf_save | 1.515 | 2.278 | 1.50x | 1.547 | 2.278 | 1.47x | 581,787 | 591,922 |
| tfidf_load | 4.778 | 4.104 | 0.86x | 4.991 | 4.104 | 0.82x | 581,787 | 591,922 |
| embedding_index_save | 4.349 | 1.619 | 0.37x | 4.543 | 1.618 | 0.36x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.286 | 37.331 | 0.76x | 10.689 | 4.992 | 0.47x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.877 | 1.429 | 0.24x | 6.415 | 1.429 | 0.22x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.614 | 0.980 | 0.15x | 6.974 | 0.979 | 0.14x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.789 | 1.497 | 0.31x | 5.148 | 1.497 | 0.29x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.509 | 1.493 | 0.99x | 1.702 | 1.492 | 0.88x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 90.40x | 0.000 | 0.001 | 90.40x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 420.489 | 556.729 | 1.32x | 420.436 | 556.705 | 1.32x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 71.927 | 66.748 | 0.93x | 72.448 | 66.740 | 0.92x | 15,250,490 | 14,022,374 |

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
| kfold_n10000 | 0.093 | 0.117 | 1.25x | 0.093 | 0.117 | 1.25x |
| stratified_n10000 | 0.187 | 0.873 | 4.68x | 0.187 | 0.873 | 4.68x |
| traintest_n10000 | 0.004 | 0.241 | 53.93x | 0.004 | 0.241 | 53.94x |
| kfold_seeded_n10000 | 0.295 | 0.676 | 2.29x | 0.295 | 0.676 | 2.29x |
| stratified_seeded_n10000 | 0.427 | 1.487 | 3.48x | 0.427 | 1.487 | 3.48x |
| traintest_seeded_n10000 | 0.153 | 0.521 | 3.40x | 0.153 | 0.521 | 3.40x |
| stratified_traintest_n10000 | 0.664 | 2.769 | 4.17x | 0.664 | 2.768 | 4.17x |
| group_kfold_n10000 | 0.122 | 0.606 | 4.96x | 0.122 | 0.606 | 4.96x |
| stratified_group_kfold_n10000 | 0.236 | 32.483 | 137.65x | 0.236 | 32.481 | 137.66x |
| timeseries_n10000 | 0.000 | 0.013 | 102.95x | 0.000 | 0.013 | 102.95x |
| repeated_kfold_n10000 | 0.967 | 1.788 | 1.85x | 0.967 | 1.788 | 1.85x |
| kfold_n100000 | 1.139 | 3.087 | 2.71x | 1.295 | 3.087 | 2.38x |
| stratified_n100000 | 2.206 | 8.094 | 3.67x | 2.352 | 8.094 | 3.44x |
| traintest_n100000 | 0.207 | 0.406 | 1.96x | 0.208 | 0.406 | 1.95x |
| kfold_seeded_n100000 | 3.669 | 7.052 | 1.92x | 3.836 | 7.051 | 1.84x |
| stratified_seeded_n100000 | 4.527 | 11.394 | 2.52x | 4.693 | 11.393 | 2.43x |
| traintest_seeded_n100000 | 2.025 | 1.999 | 0.99x | 2.074 | 1.999 | 0.96x |
| stratified_traintest_n100000 | 7.976 | 23.148 | 2.90x | 8.067 | 23.145 | 2.87x |
| group_kfold_n100000 | 1.546 | 6.829 | 4.42x | 1.684 | 6.829 | 4.06x |
| stratified_group_kfold_n100000 | 2.699 | 328.929 | 121.88x | 2.853 | 328.914 | 115.29x |
| timeseries_n100000 | 0.000 | 0.043 | 351.21x | 0.000 | 0.043 | 351.16x |
| repeated_kfold_n100000 | 10.259 | 20.513 | 2.00x | 10.440 | 20.511 | 1.96x |
| kfold_n1000000 | 9.501 | 18.258 | 1.92x | 10.022 | 18.255 | 1.82x |
| stratified_n1000000 | 21.066 | 63.346 | 3.01x | 21.719 | 63.329 | 2.92x |
| traintest_n1000000 | 0.469 | 2.038 | 4.35x | 0.541 | 2.038 | 3.77x |
| kfold_seeded_n1000000 | 35.687 | 59.899 | 1.68x | 36.066 | 59.892 | 1.66x |
| stratified_seeded_n1000000 | 49.775 | 103.467 | 2.08x | 50.823 | 103.451 | 2.04x |
| traintest_seeded_n1000000 | 20.542 | 17.875 | 0.87x | 20.855 | 17.872 | 0.86x |
| stratified_traintest_n1000000 | 92.330 | 238.691 | 2.59x | 92.619 | 238.671 | 2.58x |
| group_kfold_n1000000 | 15.116 | 55.415 | 3.67x | 15.711 | 55.406 | 3.53x |
| stratified_group_kfold_n1000000 | 27.137 | 3265.918 | 120.35x | 28.161 | 3265.586 | 115.96x |
| timeseries_n1000000 | 0.000 | 0.329 | 3164.92x | 0.000 | 0.329 | 3165.76x |
| repeated_kfold_n1000000 | 107.206 | 175.112 | 1.63x | 108.763 | 175.065 | 1.61x |

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
| welch_t_n1000 | 0.006 | 0.821 | 143.20x | 0.006 | 0.821 | 143.20x |
| mann_whitney_n1000 | 0.030 | 0.747 | 24.85x | 0.030 | 0.747 | 24.85x |
| chi_square_n1000 | 0.000 | 0.342 | 1719.46x | 0.000 | 0.342 | 1719.22x |
| fligner_n1000 | 0.074 | 0.826 | 11.10x | 0.074 | 0.826 | 11.10x |
| anderson_ksamp_n1000 | 0.163 | 0.403 | 2.47x | 0.163 | 0.403 | 2.47x |
| spearman_matrix_n1000 | 0.095 | 0.920 | 9.69x | 0.095 | 0.920 | 9.69x |
| pointbiserial_n1000 | 0.010 | 0.579 | 55.19x | 0.010 | 0.579 | 55.19x |
| welch_t_n10000 | 0.055 | 0.876 | 15.98x | 0.055 | 0.876 | 15.98x |
| mann_whitney_n10000 | 0.760 | 2.840 | 3.74x | 0.760 | 2.840 | 3.74x |
| chi_square_n10000 | 0.001 | 0.349 | 262.58x | 0.001 | 0.349 | 262.62x |
| fligner_n10000 | 1.405 | 3.278 | 2.33x | 1.477 | 3.278 | 2.22x |
| anderson_ksamp_n10000 | 2.001 | 4.090 | 2.04x | 2.138 | 4.090 | 1.91x |
| spearman_matrix_n10000 | 2.120 | 4.802 | 2.26x | 2.180 | 4.801 | 2.20x |
| pointbiserial_n10000 | 0.101 | 0.643 | 6.39x | 0.101 | 0.643 | 6.39x |
| welch_t_n100000 | 0.545 | 1.272 | 2.33x | 0.545 | 1.272 | 2.33x |
| mann_whitney_n100000 | 7.695 | 28.349 | 3.68x | 7.693 | 28.346 | 3.68x |
| chi_square_n100000 | 0.014 | 0.367 | 26.51x | 0.014 | 0.367 | 26.51x |
| fligner_n100000 | 14.798 | 32.442 | 2.19x | 15.257 | 32.441 | 2.13x |
| anderson_ksamp_n100000 | 15.249 | 43.896 | 2.88x | 15.663 | 43.893 | 2.80x |
| spearman_matrix_n100000 | 18.562 | 52.432 | 2.82x | 19.126 | 52.424 | 2.74x |
| pointbiserial_n100000 | 1.287 | 1.432 | 1.11x | 1.303 | 5.714 | 4.39x |

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
| normalize_n10000 | 0.267 | 0.461 | 1.72x | 0.270 | 0.461 | 1.71x |
| polynomial_n10000 | 0.732 | 1.017 | 1.39x | 0.737 | 1.017 | 1.38x |
| kbins_fit_n10000 | 1.455 | 2.622 | 1.80x | 1.455 | 2.621 | 1.80x |
| kbins_n10000 | 0.727 | 4.452 | 6.12x | 0.830 | 4.452 | 5.37x |
| quantile_fit_n10000 | 1.543 | 3.779 | 2.45x | 1.543 | 3.778 | 2.45x |
| quantile_n10000 | 1.806 | 1.693 | 0.94x | 1.809 | 1.693 | 0.94x |
| power_fit_n10000 | 6.155 | 135.810 | 22.06x | 6.160 | 135.810 | 22.05x |
| power_n10000 | 1.538 | 1.516 | 0.99x | 1.540 | 1.516 | 0.98x |
| knn_impute_n10000 | 7.360 | 22.407 | 3.04x | 7.359 | 89.358 | 12.14x |
| label_n10000 | 1.161 | 0.765 | 0.66x | 1.161 | 0.765 | 0.66x |
| normalize_n100000 | 1.398 | 1.834 | 1.31x | 1.770 | 1.834 | 1.04x |
| polynomial_n100000 | 2.713 | 7.050 | 2.60x | 3.321 | 7.049 | 2.12x |
| kbins_fit_n100000 | 14.699 | 8.168 | 0.56x | 15.509 | 8.167 | 0.53x |
| kbins_n100000 | 6.579 | 12.315 | 1.87x | 7.334 | 12.313 | 1.68x |
| quantile_fit_n100000 | 15.046 | 18.144 | 1.21x | 15.791 | 18.142 | 1.15x |
| quantile_n100000 | 16.740 | 14.658 | 0.88x | 17.160 | 14.657 | 0.85x |
| power_fit_n100000 | 65.505 | 916.508 | 13.99x | 68.343 | 916.373 | 13.41x |
| power_n100000 | 14.129 | 10.075 | 0.71x | 14.565 | 10.074 | 0.69x |
| knn_impute_n100000 | 7.349 | 19.307 | 2.63x | 7.347 | 77.056 | 10.49x |
| label_n100000 | 15.429 | 7.159 | 0.46x | 15.728 | 7.158 | 0.46x |

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

**New tonight: 8.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `IndelBenchmarks` | Length=32 | `SubsequenceLength_Utf16` | step | 3.1 | 0.98 | +216% | 30% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `FuzzySharp_PerText` | step | 3.53 | 1.76 | +101% | 30% |
| `VectorMathBenchmarks` | Dim=384 | `L2Norm` | step | 1.68 | 0.9 | +87% | 30% |
| `LevenshteinCodePointBenchmarks` | Length=24, Distinct=512 | `Distance_Utf16` | step | 1.06 | 0.76 | +39% | 30% |
| `MyersGateBenchmarks` | Band=64 | `Kernel` | drift | 0.05 | 0.07 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | drift | 6.259 | 5.026 | +25% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.92 | 1.19 | -23% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 2.39 | 3.06 | -22% | 20% |

**Still away from their median: 40.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | drift | 42.6 | 7.64 | +435% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | drift | 24.88 | 4.53 | +425% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | drift | 14.35 | 3.92 | +262% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | drift | 25.64 | 7.01 | +256% | 20% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | drift | 15.2 | 6.98 | +115% | 20% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | drift | 15.31 | 7.04 | +112% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.3 | 0.19 | +58% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.54 | 2.9 | +53% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.29 | 0.19 | +53% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 3.45 | 2.19 | +50% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel_Cjk` | drift | 0.29 | 0.2 | +40% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_StudentT` | drift | 2.94 | 4.818 | -39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_OneWayAnova` | drift | 4.419 | 7.24 | -39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KolmogorovSmirnov` | drift | 23.979 | 39.164 | -39% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_MannWhitney` | drift | 34.409 | 55.807 | -38% | 20% |
| `VectorizerIncumbentBenchmarks` | Documents=200 | `MlNet` | drift | 15.57 | 10.02 | +38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KruskalWallis` | drift | 59.208 | 96.354 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | drift | 1.395 | 2.243 | -38% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.88 | 1.35 | +38% | 20% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | drift | 1.27 | 2.04 | -38% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_KolmogorovSmirnov` | drift | 36.637 | 57.615 | -36% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | drift | 0.002 | 0.003 | -33% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | drift | 0.011 | 0.016 | -31% | 20% |
| `LcsGateBenchmarks` | Band=18 | `Kernel` | drift | 0.17 | 0.13 | +31% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 2.14 | 3.06 | -30% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.14 | 1.64 | -30% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.97 | 1.38 | -30% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 0.99 | 1.39 | -29% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | drift | 0.025 | 0.036 | -28% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | drift | 1.69 | 2.28 | -26% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.11 | 1.43 | -26% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=Bundle | `MlNet` | drift | 3.34 | 4.42 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 2.1 | 2.89 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.081 | 4.272 | -24% | 20% |
| `LevenshteinIncumbentBenchmarks` | Length=64 | `F23_StringSimilarity` | drift | 37.22 | 30.01 | +24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 3.04 | 4.11 | -23% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 8.22 | 6.21 | +23% | 20% |
| `MyersGateBenchmarks` | Band=64 | `Kernel_Cjk` | drift | 0.07 | 0.09 | -22% | 20% |
| `LevenshteinIncumbentBenchmarks` | Length=64 | `Quickenshtein` | drift | 4.94 | 4.08 | +21% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 0.96 | 1.25 | -21% | 20% |
