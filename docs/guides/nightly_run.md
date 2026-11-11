# Nightly benchmark run

<!-- nightly-baseline: 59a57f7257ac13a8a57dcee55e218a7f45b3b458 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks Bm25Benchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SimilaritySketchBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks StopWordBenchmarks SurvivalBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorMathBenchmarks VectorStoreChurnBenchmarks VectorizerBenchmarks WeightedLeastSquaresBenchmarks cluster-weighted cox-extended cross-conformal glm iv ols onehot panel survival-aalen survival-family survival-parametric var -->
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

- Commit: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
- Previous run: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
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

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error            | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **994.5 ns** |        **235.48 ns** |        **12.91 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         797.4 ns |         13.07 ns |         0.72 ns |     0.80 |    0.01 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,815.8 ns |        537.24 ns |        29.45 ns |     2.83 |    0.04 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,764.4 ns |        321.86 ns |        17.64 ns |     3.79 |    0.04 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,322.2 ns |        186.15 ns |        10.20 ns |     5.35 |    0.06 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,686.2 ns |      3,356.78 ns |       184.00 ns |    10.75 |    0.20 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,326.3 ns |        625.35 ns |        34.28 ns |     5.36 |    0.07 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,193.2 ns** |      **4,220.36 ns** |       **231.33 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,781.1 ns |      1,940.50 ns |       106.37 ns |     0.66 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,187,253.5 ns |     51,602.40 ns |     2,828.50 ns |    13.77 |    0.04 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,947,312.8 ns |    109,999.25 ns |     6,029.43 ns |    22.59 |    0.08 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,131,296.6 ns |    143,763.56 ns |     7,880.17 ns |    24.73 |    0.10 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 290,929,022.8 ns | 48,811,305.04 ns | 2,675,512.35 ns | 3,375.33 |   28.00 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,125,222.2 ns |    119,327.12 ns |     6,540.72 ns |    24.66 |    0.09 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.27 μs** |  **3.796 μs** | **0.208 μs** |  **1.00** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    35.32 μs |  2.926 μs | 0.160 μs |  1.13 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,985.53 μs** | **82.889 μs** | **4.543 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,461.53 μs | 72.159 μs | 3.955 μs |  0.74 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **4,601.9 ns** |   **1,060.57 ns** |     **58.13 ns** |   **1.00** |    **0.02** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,200.9 ns |      13.54 ns |      0.74 ns |   0.48 |    0.01 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,330.1 ns |     504.44 ns |     27.65 ns |   0.51 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,891.5 ns |     753.96 ns |     41.33 ns |   0.85 |    0.01 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,715.9 ns |   1,967.47 ns |    107.84 ns |   1.68 |    0.03 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,681.3 ns |   1,430.04 ns |     78.39 ns |   2.10 |    0.03 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,625.7 ns |      55.04 ns |      3.02 ns |   0.57 |    0.01 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,061.8 ns |     754.07 ns |     41.33 ns |   0.88 |    0.01 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,665.2 ns |      95.83 ns |      5.25 ns |   0.36 |    0.00 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,159.3 ns |      90.34 ns |      4.95 ns |   0.69 |    0.01 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,141.6 ns |     285.59 ns |     15.65 ns |   0.25 |    0.00 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,076.8 ns |     263.66 ns |     14.45 ns |   0.23 |    0.00 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       459.6 ns |       6.74 ns |      0.37 ns |   0.10 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,543.4 ns |      46.58 ns |      2.55 ns |   0.34 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       117.8 ns |       5.46 ns |      0.30 ns |   0.03 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       697.0 ns |      47.78 ns |      2.62 ns |   0.15 |    0.00 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **59,955.8 ns** |   **5,767.49 ns** |    **316.14 ns** |  **1.000** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,730.7 ns |   1,813.27 ns |     99.39 ns |  2.981 |    0.01 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   686,331.3 ns |  25,017.35 ns |  1,371.29 ns | 11.447 |    0.06 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,100,516.6 ns | 105,629.08 ns |  5,789.89 ns | 35.035 |    0.18 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,469,606.2 ns |  99,978.30 ns |  5,480.15 ns | 24.512 |    0.14 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,583,938.3 ns | 196,158.61 ns | 10,752.12 ns | 59.777 |    0.31 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,460,150.7 ns |  98,543.76 ns |  5,401.52 ns | 24.354 |    0.14 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,161,099.2 ns |  89,185.33 ns |  4,888.55 ns | 36.046 |    0.18 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    85,483.3 ns |   1,345.16 ns |     73.73 ns |  1.426 |    0.01 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   268,490.3 ns |   5,934.88 ns |    325.31 ns |  4.478 |    0.02 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   190,207.8 ns |  19,346.64 ns |  1,060.45 ns |  3.173 |    0.02 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   386,668.4 ns | 145,417.38 ns |  7,970.82 ns |  6.449 |    0.12 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       460.6 ns |      20.69 ns |      1.13 ns |  0.008 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,564.6 ns |     473.37 ns |     25.95 ns |  0.026 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       118.9 ns |       8.58 ns |      0.47 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       697.5 ns |      70.12 ns |      3.84 ns |  0.012 |    0.00 | 0.0572 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,857.0 ns** |     **293.95 ns** |    **16.11 ns** |  **1.00** |    **0.01** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,100.2 ns |     432.20 ns |    23.69 ns |  1.44 |    0.01 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       604.1 ns |      21.28 ns |     1.17 ns |  0.21 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,185.1 ns |      64.91 ns |     3.56 ns |  0.41 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       810.3 ns |      19.46 ns |     1.07 ns |  0.28 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,565.2 ns |     141.80 ns |     7.77 ns |  1.60 |    0.01 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     9,069.0 ns |   1,696.98 ns |    93.02 ns |  3.17 |    0.03 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,238.4 ns |     165.64 ns |     9.08 ns |  2.18 |    0.01 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,311.9 ns |     122.99 ns |     6.74 ns |  1.51 |    0.01 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **389,120.1 ns** |  **26,693.13 ns** | **1,463.14 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,347,393.2 ns |  62,665.94 ns | 3,434.93 ns | 3.463 |    0.01 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,296.6 ns |   1,329.44 ns |    72.87 ns | 0.145 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,502.2 ns |   6,562.99 ns |   359.74 ns | 0.289 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,032.5 ns |      65.13 ns |     3.57 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   887,048.7 ns |  93,462.09 ns | 5,122.97 ns | 2.280 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,441,847.4 ns | 118,221.95 ns | 6,480.14 ns | 3.705 |    0.02 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,046.1 ns |   2,203.60 ns |   120.79 ns | 0.028 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,953.9 μs** |     **140.98 μs** |      **7.73 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    89,115.5 μs |   5,192.60 μs |    284.62 μs |  45.61 |    0.20 |  3666.6667 |  1500.0000 |  833.3333 |  59880.37 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,880.0 μs** |     **406.67 μs** |     **22.29 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    99,685.4 μs |  48,813.89 μs |  2,675.65 μs |  53.03 |    1.35 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **776.7 μs** |      **81.88 μs** |      **4.49 μs** |   **1.00** |    **0.01** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   143,517.6 μs |  29,425.66 μs |  1,612.92 μs | 184.78 |    2.02 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,232.9 μs** |     **555.77 μs** |     **30.46 μs** |   **1.00** |    **0.02** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    80,153.9 μs |   7,401.13 μs |    405.68 μs |  35.90 |    0.45 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,593.0 μs** |   **2,632.51 μs** |    **144.30 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,792,095.7 μs | 253,935.42 μs | 13,919.06 μs | 114.94 |    1.20 | 35000.0000 | 10000.0000 | 4000.0000 | 537152.06 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,413.2 μs** |   **5,807.93 μs** |    **318.35 μs** |   **1.00** |    **0.03** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,884,224.1 μs | 133,581.81 μs |  7,322.07 μs | 130.77 |    2.53 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,376.1 μs** |     **222.15 μs** |     **12.18 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,138,453.6 μs | 160,454.76 μs |  8,795.07 μs | 492.22 |    1.45 | 47000.0000 | 12000.0000 | 4000.0000 | 732673.02 KB |    8,147.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **18,186.5 μs** |   **2,190.64 μs** |    **120.08 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,613,604.3 μs |  88,773.14 μs |  4,865.96 μs |  88.73 |    0.56 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error      | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|-----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 26.17 ms |  22.549 ms | 1.236 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 53.85 ms | 110.222 ms | 6.042 ms | 1272.7273 | 1181.8182 | 454.5455 |  43.93 MB |
| CountLoad4M      | 45.92 ms |   4.753 ms | 0.261 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error        | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|-------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 178,954.81 μs | 28,965.88 μs | 1,587.718 μs |         - |         - |        - | 54334.63 KB |
| CountSave240K         |     404.93 μs |     24.56 μs |     1.346 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     445.07 μs |     41.25 μs |     2.261 μs |   11.2305 |    4.8828 |   4.8828 |   739.44 KB |
| TfidfSave45K          |      51.87 μs |     25.62 μs |     1.404 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      52.42 μs |     13.80 μs |     0.756 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     288.38 μs |     52.54 μs |     2.880 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     289.21 μs |     54.99 μs |     3.014 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,629.03 μs | 27,480.93 μs | 1,506.323 μs |  593.7500 |  562.5000 | 187.5000 | 22048.96 KB |
| CountLoadAsync4M      |  58,927.28 μs | 42,409.03 μs | 2,324.582 μs | 1100.0000 | 1000.0000 | 300.0000 | 44983.53 KB |
| CountLoad4M           |  53,792.81 μs | 14,457.11 μs |   792.443 μs | 1181.8182 | 1090.9091 | 363.6364 |  44976.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error         | StdDev      | Gen0      | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|--------------:|------------:|----------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     17.095 μs |    41.0941 μs |   2.2525 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    513.955 μs |   141.8196 μs |   7.7736 μs |   35.1563 |  29.2969 |  29.2969 |   740.13 KB |
| HashingLoadUndeclared  |      5.520 μs |     0.8645 μs |   0.0474 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     28.194 μs |     1.0449 μs |   0.0573 μs |    7.4158 |   0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |    128.090 μs |     7.7814 μs |   0.4265 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    882.854 μs |   309.9054 μs |  16.9870 μs |   38.0859 |  37.1094 |   8.7891 |   656.32 KB |
| CountLoadUndeclared1M  | 11,200.079 μs | 4,864.8579 μs | 266.6593 μs |  515.6250 | 500.0000 | 296.8750 | 10793.89 KB |
| CountSaveAsync1M       |  2,183.803 μs | 2,457.8766 μs | 134.7245 μs |  109.3750 | 101.5625 | 101.5625 |  3162.11 KB |
| IndexSaveAsyncLongHead |  3,438.531 μs |    96.0010 μs |   5.2621 μs |   93.7500 |  89.8438 |  89.8438 |   2124.2 KB |
| CountLoadUndeclared2M  | 19,259.420 μs | 1,797.5350 μs |  98.5290 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error       | StdDev     | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.412 μs** |   **0.4920 μs** |  **0.0270 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.891 μs |   0.0995 μs |  0.0055 μs |  1.09 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   5.871 μs |   0.0895 μs |  0.0049 μs |  1.08 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |             |            |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **57.396 μs** |   **7.4596 μs** |  **0.4089 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.769 μs |   3.8469 μs |  0.2109 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  22.002 μs |   1.8600 μs |  0.1020 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |             |            |       |         |        |           |             |
| **UnitLoop**           | **32**         | **216.397 μs** |  **18.6862 μs** |  **1.0243 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  80.187 μs |   0.7624 μs |  0.0418 μs |  0.37 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  70.383 μs |  12.0873 μs |  0.6625 μs |  0.33 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |             |            |       |         |        |           |             |
| **UnitLoop**           | **128**        | **848.872 μs** | **185.5053 μs** | **10.1682 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 320.062 μs |  39.7549 μs |  2.1791 μs |  0.38 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 273.408 μs |  38.6394 μs |  2.1180 μs |  0.32 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **132.88 ms** |  **6.529 ms** | **0.358 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.79 ms |  2.346 ms | 0.129 ms |  0.50 |    0.00 |  103.65 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **140.28 ms** | **59.576 ms** | **3.266 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  61.26 ms |  6.395 ms | 0.351 ms |  0.44 |    0.01 |  116.47 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **202.96 ms** | **20.580 ms** | **1.128 ms** |  **1.00** |    **0.01** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 249.50 ms | 12.765 ms | 0.700 ms |  1.23 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **205.87 ms** |  **3.489 ms** | **0.191 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 215.04 ms | 35.860 ms | 1.966 ms |  1.04 |    0.01 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **255.89 ms** | **82.143 ms** | **4.503 ms** |  **1.00** |    **0.02** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 343.08 ms |  1.939 ms | 0.106 ms |  1.34 |    0.02 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **259.01 ms** | **55.166 ms** | **3.024 ms** |  **1.00** |    **0.01** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 326.08 ms | 22.624 ms | 1.240 ms |  1.26 |    0.01 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **294.83 ms** | **25.630 ms** | **1.405 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 395.45 ms | 21.577 ms | 1.183 ms |  1.34 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **304.93 ms** | **34.331 ms** | **1.882 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 396.30 ms | 14.014 ms | 0.768 ms |  1.30 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **44.85 μs** |     **4.238 μs** |   **0.232 μs** |         **-** |
| Cjk    | 1000   |      62.23 μs |    17.300 μs |   0.948 μs |         - |
| **Latin**  | **10000**  |   **4,119.58 μs** |   **137.029 μs** |   **7.511 μs** |         **-** |
| Cjk    | 10000  |   7,884.73 μs | 2,034.084 μs | 111.495 μs |         - |
| **Latin**  | **65536**  | **184,608.86 μs** | **4,693.602 μs** | **257.272 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 28.22 ms |  2.376 ms | 0.130 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 84.03 ms | 17.995 ms | 0.986 ms |  2.98 |    0.03 | 1666.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **19.04 μs** |  **1.015 μs** | **0.056 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **66.55 μs** |  **2.902 μs** | **0.159 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **180.25 μs** | **14.785 μs** | **0.810 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **389.09 μs** | **28.772 μs** | **1.577 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| EncodeUnseenProse | 9.968 ms | 18.156 ms | 0.9952 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean      | Error     | StdDev   | Allocated |
|----------- |--------- |----------:|----------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **19.05 μs** |  **2.704 μs** | **0.148 μs** |         **-** |
| MyersGroup | cjk      | 241.58 μs | 21.001 μs | 1.151 μs |         - |
| **DpGroup**    | **latin**    |  **10.22 μs** |  **3.269 μs** | **0.179 μs** |         **-** |
| MyersGroup | latin    | 132.59 μs | 15.357 μs | 0.842 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **304.04 μs** | **145.195 μs** | **7.959 μs** |  **1.00** |    **0.03** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 322.01 μs |  30.583 μs | 1.676 μs |  1.06 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **335.98 μs** |  **14.661 μs** | **0.804 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 324.65 μs |  78.878 μs | 4.324 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **320.00 μs** |  **38.113 μs** | **2.089 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 306.68 μs |  25.489 μs | 1.397 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **273.53 μs** |  **12.902 μs** | **0.707 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 324.63 μs |  13.657 μs | 0.749 μs |  1.19 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **305.98 μs** |  **24.946 μs** | **1.367 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 305.68 μs |  37.825 μs | 2.073 μs |  1.00 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **166.53 μs** |  **15.889 μs** | **0.871 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 324.49 μs | 117.965 μs | 6.466 μs |  1.95 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **228.29 μs** |  **14.255 μs** | **0.781 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 305.10 μs |   9.308 μs | 0.510 μs |  1.34 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.91 μs** |  **11.856 μs** | **0.650 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 323.46 μs |  18.948 μs | 1.039 μs |  3.60 |    0.02 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **181.3 μs** |  **56.28 μs** |  **3.08 μs** |  **1.00** |    **0.02** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   169.2 μs |  14.54 μs |  0.80 μs |  0.93 |    0.01 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   484.9 μs |  35.52 μs |  1.95 μs |  2.67 |    0.04 |  17.5781 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,251.4 μs** | **103.37 μs** |  **5.67 μs** |  **1.00** |    **0.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,205.3 μs | 837.64 μs | 45.91 μs |  0.99 |    0.01 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,627.8 μs | 827.85 μs | 45.38 μs |  2.35 |    0.01 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|----------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **154.80 ms** |  **66.765 ms** |  **3.660 ms** |  **1.00** |    **0.03** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,314.73 ms | 204.976 ms | 11.235 ms | 14.96 |    0.32 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |            |           |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **56.51 ms** |   **4.662 ms** |  **0.256 ms** |  **1.00** |    **0.01** |  **1444.4444** | **1444.4444** | **1444.4444** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   816.82 ms |  72.311 ms |  3.964 ms | 14.45 |    0.08 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **185.31 ms** |  **10.268 ms** |  **0.563 ms** |  **1.00** |    **0.00** |  **1333.3333** |  **1333.3333** |  **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,698.39 ms | 232.297 ms | 12.733 ms | 36.15 |    0.11 | 38000.0000 | 14000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 4,009.57 ms | 592.769 ms | 32.492 ms | 21.64 |    0.16 | 22000.0000 | 12000.0000 | 10000.0000 | 372.14 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **20.72 ms** |   **0.760 ms** |  **0.042 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   474.80 ms |  67.303 ms |  3.689 ms | 22.91 |    0.16 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   267.64 ms |  28.925 ms |  1.586 ms | 12.92 |    0.07 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean       | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  13.387 ms |  2.514 ms | 0.1378 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 109.046 ms | 11.975 ms | 0.6564 ms |  8.15 |    0.08 |
| Nmf_Transform                             |   2.982 ms |  1.079 ms | 0.0591 ms |  0.22 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.506 ms |  2.182 ms | 0.1196 ms |  1.83 |    0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.67 ns |  12.53 ns |  0.687 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   850.15 ns |  30.10 ns |  1.650 ns |  8.70 |    0.05 |      - |         - |          NA |
| TokenSortRatio |   834.71 ns | 109.85 ns |  6.021 ns |  8.55 |    0.07 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 1,004.56 ns | 237.78 ns | 13.034 ns | 10.29 |    0.13 | 0.0534 |     896 B |          NA |
| WRatio         | 1,292.11 ns | 184.58 ns | 10.118 ns | 13.23 |    0.12 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,029,046.3424 ns |   349,000.3044 ns |  19,129.8844 ns |  1.000 |    0.01 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    672,436.7337 ns |   127,090.2615 ns |   6,966.2461 ns |  0.331 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,123,312.3281 ns |   313,825.0226 ns |  17,201.8085 ns |  0.554 |    0.01 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 70,881,350.7619 ns | 3,554,144.2716 ns | 194,814.6412 ns | 34.935 |    0.30 |       - |       - |  510817 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.49 ns** |   **9.530 ns** |  **0.522 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    208.71 ns |  31.552 ns |  1.729 ns |  2.12 |    0.02 | 0.0048 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **753.21 ns** |  **51.939 ns** |  **2.847 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,565.73 ns | 934.799 ns | 51.240 ns | 14.03 |    0.07 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,029.99 ns** | **341.266 ns** | **18.706 ns** |  **1.00** |    **0.02** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,130.73 ns | 385.982 ns | 21.157 ns |  2.07 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,273.40 ns** | **184.778 ns** | **10.128 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,035.12 ns | 250.338 ns | 13.722 ns |  3.95 |    0.03 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.61 ns** |   **2.500 ns** |  **0.137 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    40.87 ns |   0.064 ns |  0.003 ns |  1.54 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.51 ns |   2.435 ns |  0.133 ns |  1.03 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.31 ns |   2.699 ns |  0.148 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.17 ns** |   **3.132 ns** |  **0.172 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    42.37 ns |   5.950 ns |  0.326 ns |  1.50 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.81 ns |   0.852 ns |  0.047 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    28.29 ns |   3.782 ns |  0.207 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **30.57 ns** |   **3.172 ns** |  **0.174 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    46.50 ns |   2.113 ns |  0.116 ns |  1.52 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    31.69 ns |   1.603 ns |  0.088 ns |  1.04 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.93 ns |  10.858 ns |  0.595 ns |  1.01 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **35.42 ns** |   **1.110 ns** |  **0.061 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    46.23 ns |   3.283 ns |  0.180 ns |  1.31 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    36.58 ns |   3.845 ns |  0.211 ns |  1.03 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    35.02 ns |  14.755 ns |  0.809 ns |  0.99 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **69.76 ns** |  **32.844 ns** |  **1.800 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    61.47 ns |   1.301 ns |  0.071 ns |  0.88 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.02 ns |   0.424 ns |  0.023 ns |  0.82 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    56.28 ns |   4.206 ns |  0.231 ns |  0.81 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |   **182.17 ns** |  **56.914 ns** |  **3.120 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.09 ns |   1.329 ns |  0.073 ns |  0.38 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.09 ns |   0.208 ns |  0.011 ns |  0.36 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    62.62 ns |   1.674 ns |  0.092 ns |  0.34 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **378.71 ns** |  **10.085 ns** |  **0.553 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   387.84 ns |  70.776 ns |  3.879 ns |  1.02 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   381.98 ns | 108.734 ns |  5.960 ns |  1.01 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   375.05 ns |   5.447 ns |  0.299 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,562.39 ns** | **180.380 ns** |  **9.887 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,777.97 ns |  73.017 ns |  4.002 ns |  1.05 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,665.94 ns | 860.018 ns | 47.140 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,649.02 ns | 947.274 ns | 51.923 ns |  1.02 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **386.2 ns** |     **15.12 ns** |   **0.83 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    197.1 ns |    136.13 ns |   7.46 ns |  0.51 |    0.02 |         - |          NA |
|                    |        |             |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,399.4 ns** |    **426.74 ns** |  **23.39 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,049.4 ns |    162.53 ns |   8.91 ns |  1.27 |    0.01 |         - |          NA |
|                    |        |             |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,470.1 ns** |    **431.16 ns** |  **23.63 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,552.8 ns | 11,011.35 ns | 603.57 ns |  2.42 |    0.04 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,719.1 ns** | **3,331.66 ns** | **182.62 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  8,091.2 ns | 4,531.95 ns | 248.41 ns |  0.93 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,763.0 ns | 3,019.31 ns | 165.50 ns |  1.35 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,110.7 ns** | **1,810.90 ns** |  **99.26 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,355.4 ns | 1,620.46 ns |  88.82 ns |  0.93 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,591.8 ns | 1,905.94 ns | 104.47 ns |  1.44 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,788.0 ns** | **4,136.73 ns** | **226.75 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,280.6 ns |   114.90 ns |   6.30 ns |  0.88 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,041.3 ns | 1,807.43 ns |  99.07 ns |  1.41 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,608.8 ns** | **3,506.08 ns** | **192.18 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,413.3 ns | 1,307.82 ns |  71.69 ns |  1.14 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,344.8 ns | 1,572.37 ns |  86.19 ns |  1.67 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **6,901.5 ns** | **3,550.69 ns** | **194.63 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,909.1 ns |   507.73 ns |  27.83 ns |  1.00 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,323.6 ns | 1,160.36 ns |  63.60 ns |  1.64 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,202.8 ns** | **3,486.55 ns** | **191.11 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,989.0 ns | 6,085.76 ns | 333.58 ns |  0.98 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,051.5 ns |   955.24 ns |  52.36 ns |  1.64 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,728.5 ns** |   **417.05 ns** |  **22.86 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,082.2 ns |   390.97 ns |  21.43 ns |  1.09 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,454.3 ns |   463.22 ns |  25.39 ns |  2.00 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,344.9 ns** |   **655.12 ns** |  **35.91 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,358.8 ns | 1,344.58 ns |  73.70 ns |  1.00 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,973.0 ns |   810.46 ns |  44.42 ns |  1.87 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,780.6 ns** | **1,307.89 ns** |  **71.69 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,364.1 ns |   502.14 ns |  27.52 ns |  0.95 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,813.3 ns | 1,868.26 ns | 102.41 ns |  1.78 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,373.6 ns** | **1,269.23 ns** |  **69.57 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,129.4 ns |   221.52 ns |  12.14 ns |  0.90 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,911.2 ns |   493.34 ns |  27.04 ns |  2.49 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,040.4 ns** | **1,155.54 ns** |  **63.34 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,871.7 ns |   206.66 ns |  11.33 ns |  0.71 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,492.4 ns |   623.47 ns |  34.17 ns |  2.10 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,577.2 ns** |   **574.27 ns** |  **31.48 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,800.8 ns |   395.86 ns |  21.70 ns |  0.58 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,639.1 ns |   610.80 ns |  33.48 ns |  1.92 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,659.3 ns** |   **412.93 ns** |  **22.63 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,383.9 ns |   268.46 ns |  14.72 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,206.2 ns | 1,169.11 ns |  64.08 ns |  1.97 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,214.1 ns** |   **840.69 ns** |  **46.08 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,098.6 ns |   187.19 ns |  10.26 ns |  0.79 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,346.3 ns | 2,603.94 ns | 142.73 ns |  1.98 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,761.2 ns** |   **493.36 ns** |  **27.04 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,306.8 ns |   304.01 ns |  16.66 ns |  1.07 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,003.7 ns |   500.06 ns |  27.41 ns |  1.80 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,440.7 ns** |    **89.58 ns** |   **4.91 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    571.6 ns |    34.84 ns |   1.91 ns |  0.40 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,115.3 ns |   243.20 ns |  13.33 ns |  3.55 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **2,933.9 ns** |   **364.09 ns** |  **19.96 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,395.7 ns |   249.92 ns |  13.70 ns |  0.48 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,635.7 ns |   737.49 ns |  40.42 ns |  2.60 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,253.7 ns** |   **145.46 ns** |   **7.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,355.7 ns |   452.49 ns |  24.80 ns |  0.45 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,889.3 ns |   998.41 ns |  54.73 ns |  2.26 |    0.01 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,523.6 μs** |     **297.77 μs** |    **16.32 μs** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  39,789.7 μs |  22,604.40 μs | 1,239.02 μs |  7.20 |    0.20 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  84,565.1 μs |   6,199.92 μs |   339.84 μs | 15.31 |    0.07 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **806.8 μs** |      **56.63 μs** |     **3.10 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,796.1 μs |     145.83 μs |     7.99 μs |  5.94 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,041.8 μs |     172.30 μs |     9.44 μs |  3.77 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **27,358.2 μs** |   **4,236.95 μs** |   **232.24 μs** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 451,374.8 μs | 125,566.18 μs | 6,882.71 μs | 16.50 |    0.25 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 755,246.8 μs | 173,090.21 μs | 9,487.66 μs | 27.61 |    0.36 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.912 ms** |  **0.4225 ms** | **0.0232 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.623 ms |  0.2190 ms | 0.0120 ms |  3.38 |    0.01 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **25.779 ms** |  **3.4786 ms** | **0.1907 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 114.504 ms | 10.9973 ms | 0.6028 ms |  4.44 |    0.03 |        - |        - |        - |   96.47 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **48.205 ms** |  **8.9061 ms** | **0.4882 ms** |  **1.00** |    **0.01** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 207.116 ms | 23.7249 ms | 1.3004 ms |  4.30 |    0.04 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.60 ns** |    **27.070 ns** |   **1.484 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |     56.24 ns |    11.950 ns |   0.655 ns |  0.43 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    130.64 ns |     3.254 ns |   0.178 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |     98.49 ns |     8.541 ns |   0.468 ns |  0.75 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **219.70 ns** |    **86.348 ns** |   **4.733 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 12   |     62.21 ns |     0.898 ns |   0.049 ns |  0.28 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    224.80 ns |   146.443 ns |   8.027 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    109.46 ns |     3.930 ns |   0.215 ns |  0.50 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **282.65 ns** |   **152.343 ns** |   **8.350 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 14   |     68.54 ns |    15.898 ns |   0.871 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    280.19 ns |   375.319 ns |  20.572 ns |  0.99 |    0.07 |         - |          NA |
| Kernel_Cjk | 14   |    115.36 ns |     3.270 ns |   0.179 ns |  0.41 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **370.47 ns** |   **297.662 ns** |  **16.316 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 16   |     72.37 ns |     2.516 ns |   0.138 ns |  0.20 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    347.40 ns |     6.113 ns |   0.335 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 16   |    120.31 ns |    21.754 ns |   1.192 ns |  0.33 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **436.48 ns** |    **11.979 ns** |   **0.657 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.29 ns |    13.850 ns |   0.759 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    446.05 ns |   153.069 ns |   8.390 ns |  1.02 |    0.02 |         - |          NA |
| Kernel_Cjk | 18   |    127.29 ns |    59.586 ns |   3.266 ns |  0.29 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **779.00 ns** |   **202.601 ns** |  **11.105 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 20   |     81.06 ns |     0.707 ns |   0.039 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    772.27 ns |     6.188 ns |   0.339 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    142.14 ns |     0.971 ns |   0.053 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **996.33 ns** |    **56.884 ns** |   **3.118 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |     88.37 ns |     2.385 ns |   0.131 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    972.06 ns |   175.368 ns |   9.613 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    141.61 ns |     7.956 ns |   0.436 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,536.44 ns** |   **338.577 ns** |  **18.559 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    105.30 ns |     0.176 ns |   0.010 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,566.11 ns |   123.704 ns |   6.781 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    167.72 ns |    31.233 ns |   1.712 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,196.80 ns** |   **545.600 ns** |  **29.906 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    133.94 ns |    25.720 ns |   1.410 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,259.42 ns |   370.936 ns |  20.332 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 48   |    241.43 ns |     7.346 ns |   0.403 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,186.52 ns** | **3,846.784 ns** | **210.855 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 64   |    165.36 ns |    22.865 ns |   1.253 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,157.25 ns | 1,233.798 ns |  67.629 ns |  0.83 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    296.20 ns |   101.831 ns |   5.582 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **13,518.92 ns** | **9,688.116 ns** | **531.038 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 96   |    320.65 ns |     7.354 ns |   0.403 ns |  0.02 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 13,733.97 ns | 5,961.090 ns | 326.747 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |    927.41 ns |   116.981 ns |   6.412 ns |  0.07 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.87 ns** |     **2.770 ns** |  **0.152 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.75 ns |     0.230 ns |  0.013 ns |  1.00 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    120.11 ns |    40.662 ns |  2.229 ns |  4.83 |    0.08 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.30 ns |     5.074 ns |  0.278 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **278.92 ns** |    **76.000 ns** |  **4.166 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    363.31 ns |    35.079 ns |  1.923 ns |  1.30 |    0.02 |         - |          NA |
| Distance_CodePoint         | 64     |    659.74 ns |    20.275 ns |  1.111 ns |  2.37 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    278.07 ns |     2.019 ns |  0.111 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **895.61 ns** |    **15.406 ns** |  **0.844 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,896.21 ns |   214.417 ns | 11.753 ns |  2.12 |    0.01 |         - |          NA |
| Distance_CodePoint         | 128    |  1,628.58 ns |   163.740 ns |  8.975 ns |  1.82 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    911.33 ns |    17.316 ns |  0.949 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,151.84 ns** |   **676.012 ns** | **37.054 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,781.80 ns | 1,740.843 ns | 95.422 ns |  1.50 |    0.01 |         - |          NA |
| Distance_CodePoint         | 512    | 13,480.59 ns |   625.235 ns | 34.271 ns |  1.21 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,941.47 ns |    89.284 ns |  4.894 ns |  0.98 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **342.4 ns** |      **79.95 ns** |      **4.38 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     237.1 ns |      22.23 ns |      1.22 ns |  0.69 |    0.01 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **332.3 ns** |      **38.28 ns** |      **2.10 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     243.6 ns |      26.15 ns |      1.43 ns |  0.73 |    0.01 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **427.4 ns** |      **23.41 ns** |      **1.28 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.3 ns |      25.91 ns |      1.42 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **416.3 ns** |      **75.48 ns** |      **4.14 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     323.4 ns |      12.45 ns |      0.68 ns |  0.78 |    0.01 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **512.0 ns** |      **97.51 ns** |      **5.34 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     414.9 ns |      58.88 ns |      3.23 ns |  0.81 |    0.01 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **525.3 ns** |       **9.99 ns** |      **0.55 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     416.2 ns |       7.16 ns |      0.39 ns |  0.79 |    0.00 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **603.1 ns** |     **184.30 ns** |     **10.10 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,256.2 ns |      70.06 ns |      3.84 ns |  2.08 |    0.03 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **590.6 ns** |     **183.85 ns** |     **10.08 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,277.9 ns |      30.71 ns |      1.68 ns |  2.16 |    0.03 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,940.8 ns** |     **329.84 ns** |     **18.08 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,562.9 ns |     437.36 ns |     23.97 ns |  2.87 |    0.03 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,932.9 ns** |     **294.13 ns** |     **16.12 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,424.9 ns |     252.83 ns |     13.86 ns |  2.81 |    0.02 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,727.7 ns** |     **497.40 ns** |     **27.26 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  63,966.6 ns |   3,953.18 ns |    216.69 ns |  4.34 |    0.01 |         - |          NA |
|                    |        |          |              |               |              |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **451,807.7 ns** | **194,471.60 ns** | **10,659.64 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  61,180.4 ns |   1,902.82 ns |    104.30 ns |  0.14 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.01 ns** |      **5.908 ns** |     **0.324 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      85.11 ns |     43.865 ns |     2.404 ns |  3.40 |    0.09 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      85.49 ns |      9.493 ns |     0.520 ns |  3.42 |    0.04 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     178.69 ns |     11.974 ns |     0.656 ns |  7.15 |    0.08 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.48 ns** |     **23.559 ns** |     **1.291 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,804.92 ns |  6,769.100 ns |   371.037 ns | 21.00 |    1.17 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,373.20 ns |    477.138 ns |    26.154 ns |  4.97 |    0.08 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,620.62 ns |  2,108.984 ns |   115.601 ns | 38.41 |    0.39 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,020.42 ns** |    **354.182 ns** |    **19.414 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 391,386.07 ns | 75,631.651 ns | 4,145.626 ns | 35.51 |    0.33 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  38,861.51 ns |  5,220.594 ns |   286.158 ns |  3.53 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 705,503.88 ns | 60,921.726 ns | 3,339.325 ns | 64.02 |    0.28 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error    | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|---------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **602.8 μs** | **37.10 μs** | **2.03 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 605.5 μs | 91.74 μs | 5.03 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 601.7 μs | 57.11 μs | 3.13 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **769.9 μs** | **53.57 μs** | **2.94 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 801.7 μs | 27.00 μs | 1.48 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 918.0 μs |  6.98 μs | 0.38 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **127.93 μs** |      **3.019 μs** |     **0.165 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 164,841.36 μs | 62,701.689 μs | 3,436.891 μs | 1,288.52 |   23.31 | 750.0000 | 750.0000 | 750.0000 | 31413.75 KB |   12,805.60 |
|                                 |         |               |               |              |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,791.14 μs** |    **216.094 μs** |    **11.845 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 758,740.96 μs | 46,345.098 μs | 2,540.331 μs |   271.84 |    1.27 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.46 μs** |      **1.080 μs** |     **0.059 μs** |     **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     891.45 μs |     32.906 μs |     1.804 μs |    39.70 |    0.11 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,564.7 ns** |     **2,551.85 ns** |    **139.88 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,273.7 ns |       798.04 ns |     43.74 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       110.6 ns |        25.91 ns |      1.42 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,420.1 ns |       680.96 ns |     37.33 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,460.5 ns |       216.64 ns |     11.88 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **5,056.2 ns** |       **515.11 ns** |     **28.24 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,452.3 ns |       511.31 ns |     28.03 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       110.9 ns |        63.25 ns |      3.47 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,325.7 ns |       911.33 ns |     49.95 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,776.2 ns |       964.29 ns |     52.86 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **461,028.0 ns** |     **2,528.44 ns** |    **138.59 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   839,140.1 ns |   176,586.12 ns |  9,679.28 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,539.4 ns |     1,101.11 ns |     60.36 ns |      - |         - |
| F1Macro        | 100000  | 2       |   460,963.5 ns |    29,809.38 ns |  1,633.95 ns |      - |     656 B |
| Report         | 100000  | 2       |   482,775.4 ns |   103,721.23 ns |  5,685.31 ns |      - |    5489 B |
| **Matrix**         | **100000**  | **10**      |   **507,442.7 ns** |    **55,749.63 ns** |  **3,055.83 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   944,366.6 ns |    27,360.27 ns |  1,499.71 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,150.3 ns |       796.54 ns |     43.66 ns |      - |         - |
| F1Macro        | 100000  | 10      |   494,328.4 ns |    12,887.64 ns |    706.42 ns |      - |    1881 B |
| Report         | 100000  | 10      |   501,865.8 ns |    47,912.71 ns |  2,626.26 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,761,677.0 ns** |    **35,181.97 ns** |  **1,928.44 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,360,905.7 ns |   225,109.12 ns | 12,338.99 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,964.9 ns |     6,765.07 ns |    370.82 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,598,766.5 ns |   116,150.50 ns |  6,366.60 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,635,543.7 ns |   296,188.51 ns | 16,235.09 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,923,088.3 ns** |    **84,846.61 ns** |  **4,650.73 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,395,241.4 ns | 1,476,496.74 ns | 80,931.77 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,923.5 ns |    11,063.55 ns |    606.43 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 4,938,611.4 ns |   402,608.45 ns | 22,068.33 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,109,723.8 ns |   717,743.75 ns | 39,341.96 ns |      - |   12878 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **10,223.77 μs** |    **425.607 μs** |    **23.329 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,881.92 μs |  2,306.082 μs |   126.404 μs |     3.41 |    0.01 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.17 μs** |      **1.796 μs** |     **0.098 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  35,022.17 μs |  7,723.659 μs |   423.360 μs | 3,136.74 |   40.70 | 538.4615 | 538.4615 | 538.4615 |  5089463 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **143,944.08 μs** | **37,372.673 μs** | **2,048.522 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 224,334.49 μs | 20,373.476 μs | 1,116.739 μs |     1.56 |    0.02 |        - |        - |        - | 23228008 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **136.25 μs** |     **15.668 μs** |     **0.859 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 224,526.36 μs |  7,966.915 μs |   436.694 μs | 1,647.90 |    9.39 |        - |        - |        - | 23228008 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **73.99 ns** |      **1.968 ns** |   **0.108 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.99 ns |     15.210 ns |   0.834 ns |  1.00 |    0.01 |         - |          NA |
| Dp_Cjk     | 4    |     75.04 ns |     14.683 ns |   0.805 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |     73.24 ns |     11.233 ns |   0.616 ns |  0.99 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **6**    |    **104.94 ns** |      **3.524 ns** |   **0.193 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     80.35 ns |     18.086 ns |   0.991 ns |  0.77 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    106.39 ns |     40.766 ns |   2.235 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    128.13 ns |      9.392 ns |   0.515 ns |  1.22 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **8**    |    **161.96 ns** |      **2.060 ns** |   **0.113 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     92.55 ns |      6.032 ns |   0.331 ns |  0.57 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    165.76 ns |     33.273 ns |   1.824 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    186.71 ns |     13.578 ns |   0.744 ns |  1.15 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **10**   |    **203.99 ns** |      **4.454 ns** |   **0.244 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    102.39 ns |      6.954 ns |   0.381 ns |  0.50 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    197.79 ns |     17.841 ns |   0.978 ns |  0.97 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    149.54 ns |      7.521 ns |   0.412 ns |  0.73 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **246.03 ns** |      **9.615 ns** |   **0.527 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    108.59 ns |      5.341 ns |   0.293 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    245.14 ns |      5.370 ns |   0.294 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    162.57 ns |      3.161 ns |   0.173 ns |  0.66 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **416.34 ns** |    **178.404 ns** |   **9.779 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 16   |    133.11 ns |     21.444 ns |   1.175 ns |  0.32 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    413.08 ns |    140.171 ns |   7.683 ns |  0.99 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    184.98 ns |     75.911 ns |   4.161 ns |  0.44 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **844.79 ns** |     **59.856 ns** |   **3.281 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    160.06 ns |      0.755 ns |   0.041 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    859.38 ns |    129.940 ns |   7.122 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    224.51 ns |      1.123 ns |   0.062 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,450.81 ns** |     **16.071 ns** |   **0.881 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    197.41 ns |     13.472 ns |   0.738 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,455.54 ns |    162.573 ns |   8.911 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    266.72 ns |      1.641 ns |   0.090 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,384.41 ns** |  **2,289.327 ns** | **125.486 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 48   |    268.96 ns |     13.878 ns |   0.761 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,363.11 ns |    870.020 ns |  47.689 ns |  0.99 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    359.21 ns |     32.588 ns |   1.786 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **6,332.46 ns** |  **2,537.791 ns** | **139.105 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 64   |    343.32 ns |     21.591 ns |   1.183 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  6,916.61 ns |  5,559.145 ns | 304.715 ns |  1.09 |    0.05 |         - |          NA |
| Kernel_Cjk | 64   |    440.57 ns |     21.123 ns |   1.158 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **14,164.04 ns** |  **3,011.813 ns** | **165.088 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    738.77 ns |    144.012 ns |   7.894 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,334.47 ns | 10,473.812 ns | 574.105 ns |  1.01 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |  1,547.29 ns |    669.977 ns |  36.724 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **205.23 μs** |   **8.574 μs** |  **0.470 μs** |  **1.00** |    **0.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   209.01 μs |  25.709 μs |  1.409 μs |  1.02 |    0.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    59.17 μs |   7.806 μs |  0.428 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   199.01 μs |  48.503 μs |  2.659 μs |  0.97 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **205.40 μs** |  **48.544 μs** |  **2.661 μs** |  **1.00** |    **0.02** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   232.62 μs |  33.068 μs |  1.813 μs |  1.13 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    58.91 μs |   4.317 μs |  0.237 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   200.13 μs |  71.807 μs |  3.936 μs |  0.97 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,075.13 μs** | **517.371 μs** | **28.359 μs** |  **1.00** |    **0.02** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,035.15 μs | 104.863 μs |  5.748 μs |  0.98 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,081.17 μs |  92.501 μs |  5.070 μs |  0.52 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,970.18 μs |  46.589 μs |  2.554 μs |  0.95 |    0.01 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,969.68 μs** |  **17.134 μs** |  **0.939 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,077.74 μs | 140.101 μs |  7.679 μs |  1.05 |    0.00 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,083.57 μs |  75.504 μs |  4.139 μs |  0.55 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,988.47 μs | 421.416 μs | 23.099 μs |  1.01 |    0.01 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error       | StdDev     | Allocated |
|------------ |------------- |-------------:|------------:|-----------:|----------:|
| **Embedded**    | **65**           |    **10.623 μs** |   **0.9313 μs** |  **0.0510 μs** |         **-** |
| EqualLength | 65           |     2.992 μs |   1.2071 μs |  0.0662 μs |         - |
| **Embedded**    | **128**          |    **40.172 μs** |   **1.6329 μs** |  **0.0895 μs** |         **-** |
| EqualLength | 128          |     5.422 μs |   0.4074 μs |  0.0223 μs |         - |
| **Embedded**    | **512**          | **1,326.770 μs** | **299.6222 μs** | **16.4233 μs** |         **-** |
| EqualLength | 512          |    43.254 μs |   4.2797 μs |  0.2346 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.996 ms | 1.8464 ms | 0.1012 ms | 109.3750 | 101.5625 |  31.2500 |  3711.62 KB |
| TokenizerJsonWordPiece | 11.547 ms | 3.3845 ms | 0.1855 ms | 187.5000 | 171.8750 |  46.8750 |  5852.41 KB |
| TokenizerJsonUnigram   | 12.687 ms | 0.2482 ms | 0.0136 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  5.500 ms | 1.3627 ms | 0.0747 ms | 101.5625 |  93.7500 |  31.2500 |   4292.3 KB |
| TfidfSave              |  1.619 ms | 0.9125 ms | 0.0500 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.412 ms | 0.1601 ms | 0.0088 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  5.284 ms | 0.6521 ms | 0.0357 ms | 257.8125 | 257.8125 | 257.8125 | 20349.96 KB |
| EmbeddingIndexLoad     |  8.387 ms | 3.1169 ms | 0.1708 ms | 179.6875 | 148.4375 | 117.1875 | 16094.38 KB |
| EmbeddingIndexSaveFile | 49.559 ms | 7.7984 ms | 0.4275 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 73.815 ms | 5.1199 ms | 0.2806 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,202.73 μs** | **1,460.147 μs** |  **80.036 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,299.65 μs | 2,724.888 μs | 149.360 μs |  1.66 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **133.22 μs** |     **3.642 μs** |   **0.200 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    180.83 μs |    12.336 μs |   0.676 μs |  1.36 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,808.86 μs** |   **324.605 μs** |  **17.793 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,587.09 μs |   182.817 μs |  10.021 μs |  0.92 |    0.01 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.80 μs** |     **1.599 μs** |   **0.088 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.40 μs |     2.523 μs |   0.138 μs |  1.20 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.71 μs** |   **7.276 μs** |  **0.399 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    45.21 μs |  15.112 μs |  0.828 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.23 μs |   3.509 μs |  0.192 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   546.48 μs | 186.976 μs | 10.249 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **464.91 μs** |  **98.268 μs** |  **5.386 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   451.61 μs |  44.748 μs |  2.453 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,110.18 μs |  32.298 μs |  1.770 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,912.72 μs | 513.648 μs | 28.155 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean            | Error          | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|---------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,196.8 ns |       149.1 ns |      8.17 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,445.9 ns |       479.7 ns |     26.29 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,228.8 ns |       478.8 ns |     26.24 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        472.3 ns |       109.4 ns |      5.99 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 12,177,124.7 ns | 1,549,748.5 ns | 84,946.95 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **20.82 ms** |  **0.844 ms** | **0.046 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.94 ms |  6.963 ms | 0.382 ms |  2.59 |    0.02 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **63.40 ms** |  **5.916 ms** | **0.324 ms** |  **1.00** |    **0.01** |  **500.0000** |   **7.98 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  68.55 ms | 33.633 ms | 1.844 ms |  1.08 |    0.03 |  250.0000 |   5.65 MB |        0.71 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **86.41 ms** | **29.799 ms** | **1.633 ms** |  **1.00** |    **0.02** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 255.04 ms | 31.931 ms | 1.750 ms |  2.95 |    0.05 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.404
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.785 ms** |   **1.020 ms** |  **0.0559 ms** |  **1.00** |    **0.02** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  54.625 ms | 121.909 ms |  6.6822 ms | 14.43 |    1.54 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |            |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.714 ms** |   **3.656 ms** |  **0.2004 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 395.451 ms | 302.671 ms | 16.5904 ms | 22.33 |    0.84 | 76000.0000 | 76000.0000 | 76000.0000 | 324.81 MB |       40.06 |

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
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.174 | 0.036 | 0.20x | 0.174 | 0.036 | 0.20x |
| cdist_wratio_n50 | 2.415 | 2.272 | 0.94x | 2.415 | 2.272 | 0.94x |
| cdist_ratio_varied_n50 | 0.136 | 0.031 | 0.23x | 0.136 | 0.031 | 0.23x |
| cdist_ratio_varied_cutoff90_n50 | 0.056 | 0.031 | 0.55x | 0.056 | 0.031 | 0.55x |
| cdist_ratio_n200 | 2.800 | 0.441 | 0.16x | 2.804 | 0.441 | 0.16x |
| cdist_wratio_n200 | 36.958 | 34.476 | 0.93x | 36.958 | 34.476 | 0.93x |
| cdist_ratio_varied_n200 | 2.367 | 0.388 | 0.16x | 2.368 | 0.388 | 0.16x |
| cdist_ratio_varied_cutoff90_n200 | 1.016 | 0.388 | 0.38x | 1.018 | 0.388 | 0.38x |
| cdist_ratio_n500 | 17.347 | 2.663 | 0.15x | 17.366 | 2.663 | 0.15x |
| cdist_wratio_n500 | 232.313 | 209.204 | 0.90x | 232.450 | 209.192 | 0.90x |
| cdist_ratio_varied_n500 | 14.135 | 2.296 | 0.16x | 14.278 | 2.296 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 5.751 | 2.300 | 0.40x | 5.862 | 2.300 | 0.39x |

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
| latin | 8 | 119.4 | 27.6 | 4.33x C# faster |
| latin | 32 | 181.5 | 86.4 | 2.10x C# faster |
| latin | 128 | 447.4 | 400.2 | 1.12x C# faster |
| latin | 512 | 4465.9 | 4989.9 | 1.12x Py faster |
| cjk | 8 | 145.3 | 25.8 | 5.63x C# faster |
| cjk | 32 | 340.0 | 212.6 | 1.60x C# faster |
| cjk | 128 | 1838.5 | 1395.1 | 1.32x C# faster |
| cjk | 512 | 14917.0 | 9888.6 | 1.51x C# faster |

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
| latin | 8 | 149.9 | 18.1 | 8.30x C# faster |
| latin | 32 | 282.0 | 159.2 | 1.77x C# faster |
| latin | 128 | 2338.4 | 862.0 | 2.71x C# faster |
| latin | 512 | 13987.0 | 11512.4 | 1.21x C# faster |
| cjk | 8 | 159.8 | 17.9 | 8.93x C# faster |
| cjk | 32 | 385.1 | 293.3 | 1.31x C# faster |
| cjk | 128 | 3442.5 | 2095.8 | 1.64x C# faster |
| cjk | 512 | 23736.0 | 19230.9 | 1.23x C# faster |

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
| confusion_matrix_n1000_k2 | 0.005 | 0.961 | 212.97x | 0.005 | 0.961 | 212.95x |
| accuracy_n1000_k2 | 0.000 | 0.512 | 4154.96x | 0.000 | 0.512 | 4155.29x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.753 | 365.88x | 0.005 | 1.753 | 365.89x |
| classification_report_n1000_k2 | 0.008 | 6.757 | 889.75x | 0.008 | 6.757 | 889.76x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.868 | 107.63x | 0.017 | 1.868 | 107.60x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.019 | 209.82x | 0.005 | 1.019 | 209.82x |
| matthews_n1000_k2 | 0.005 | 1.953 | 424.65x | 0.005 | 1.953 | 424.65x |
| cohen_kappa_n1000_k2 | 0.005 | 1.062 | 217.28x | 0.005 | 1.062 | 217.30x |
| mse_n1000_k2 | 0.000 | 0.301 | 607.63x | 0.000 | 0.301 | 607.63x |
| mae_n1000_k2 | 0.000 | 0.300 | 613.70x | 0.000 | 0.299 | 613.69x |
| median_ae_n1000_k2 | 0.006 | 0.312 | 54.36x | 0.006 | 0.312 | 54.36x |
| r2_n1000_k2 | 0.001 | 0.361 | 302.69x | 0.001 | 0.361 | 302.67x |
| confusion_matrix_n1000_k10 | 0.005 | 0.957 | 186.70x | 0.005 | 0.957 | 186.71x |
| accuracy_n1000_k10 | 0.000 | 0.509 | 4168.89x | 0.000 | 0.509 | 4169.03x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.747 | 294.48x | 0.006 | 1.747 | 294.51x |
| classification_report_n1000_k10 | 0.012 | 6.818 | 571.44x | 0.012 | 6.817 | 571.42x |
| roc_auc_ovr_macro_n1000_k10 | 0.522 | 9.666 | 18.51x | 0.522 | 9.666 | 18.51x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.024 | 175.00x | 0.006 | 1.024 | 175.00x |
| matthews_n1000_k10 | 0.006 | 1.971 | 347.51x | 0.006 | 1.971 | 347.53x |
| cohen_kappa_n1000_k10 | 0.006 | 1.067 | 180.41x | 0.006 | 1.067 | 180.40x |
| mse_n1000_k10 | 0.001 | 0.297 | 591.38x | 0.001 | 0.297 | 591.44x |
| mae_n1000_k10 | 0.000 | 0.294 | 606.53x | 0.000 | 0.294 | 606.53x |
| median_ae_n1000_k10 | 0.006 | 0.308 | 53.64x | 0.006 | 0.308 | 53.64x |
| r2_n1000_k10 | 0.001 | 0.360 | 303.04x | 0.001 | 0.360 | 303.02x |
| confusion_matrix_n100000_k2 | 0.476 | 10.732 | 22.54x | 0.476 | 10.732 | 22.54x |
| accuracy_n100000_k2 | 0.012 | 3.755 | 305.17x | 0.012 | 3.755 | 305.14x |
| precision_recall_f1_macro_n100000_k2 | 0.476 | 12.344 | 25.93x | 0.476 | 12.343 | 25.92x |
| classification_report_n100000_k2 | 0.480 | 27.062 | 56.42x | 0.480 | 27.060 | 56.42x |
| roc_auc_binary_n100000_k2 | 4.474 | 26.830 | 6.00x | 4.474 | 26.828 | 6.00x |
| balanced_accuracy_n100000_k2 | 0.476 | 10.844 | 22.78x | 0.476 | 10.843 | 22.78x |
| matthews_n100000_k2 | 0.476 | 21.627 | 45.47x | 0.476 | 21.625 | 45.47x |
| cohen_kappa_n100000_k2 | 0.475 | 10.862 | 22.85x | 0.475 | 10.861 | 22.85x |
| mse_n100000_k2 | 0.045 | 0.442 | 9.82x | 0.045 | 0.441 | 9.82x |
| mae_n100000_k2 | 0.045 | 0.455 | 10.11x | 0.045 | 0.454 | 10.11x |
| median_ae_n100000_k2 | 0.706 | 1.798 | 2.55x | 0.735 | 1.798 | 2.44x |
| r2_n100000_k2 | 0.110 | 0.653 | 5.95x | 0.110 | 0.653 | 5.95x |
| confusion_matrix_n100000_k10 | 0.508 | 10.809 | 21.28x | 0.508 | 10.807 | 21.28x |
| accuracy_n100000_k10 | 0.012 | 3.763 | 306.39x | 0.012 | 3.763 | 306.42x |
| precision_recall_f1_macro_n100000_k10 | 0.509 | 12.955 | 25.44x | 0.509 | 12.955 | 25.44x |
| classification_report_n100000_k10 | 0.516 | 29.637 | 57.41x | 0.516 | 29.636 | 57.41x |
| roc_auc_ovr_macro_n100000_k10 | 36.467 | 217.990 | 5.98x | 36.464 | 217.979 | 5.98x |
| balanced_accuracy_n100000_k10 | 0.508 | 10.833 | 21.31x | 0.508 | 10.833 | 21.31x |
| matthews_n100000_k10 | 0.508 | 22.665 | 44.57x | 0.508 | 22.663 | 44.57x |
| cohen_kappa_n100000_k10 | 0.508 | 10.929 | 21.49x | 0.509 | 10.929 | 21.49x |
| mse_n100000_k10 | 0.045 | 0.446 | 9.93x | 0.045 | 0.446 | 9.93x |
| mae_n100000_k10 | 0.045 | 0.452 | 10.06x | 0.045 | 0.452 | 10.06x |
| median_ae_n100000_k10 | 0.685 | 1.804 | 2.63x | 0.731 | 1.804 | 2.47x |
| r2_n100000_k10 | 0.110 | 0.656 | 5.97x | 0.110 | 0.656 | 5.97x |
| confusion_matrix_n1000000_k2 | 4.757 | 101.863 | 21.41x | 4.757 | 101.821 | 21.40x |
| accuracy_n1000000_k2 | 0.135 | 33.508 | 248.29x | 0.135 | 33.496 | 248.22x |
| precision_recall_f1_macro_n1000000_k2 | 4.765 | 109.329 | 22.94x | 4.765 | 109.321 | 22.94x |
| classification_report_n1000000_k2 | 4.770 | 213.874 | 44.84x | 4.770 | 213.843 | 44.83x |
| roc_auc_binary_n1000000_k2 | 58.972 | 296.864 | 5.03x | 58.970 | 296.847 | 5.03x |
| balanced_accuracy_n1000000_k2 | 4.759 | 101.249 | 21.28x | 4.759 | 101.237 | 21.27x |
| matthews_n1000000_k2 | 4.758 | 202.576 | 42.57x | 4.758 | 202.550 | 42.57x |
| cohen_kappa_n1000000_k2 | 4.763 | 101.296 | 21.27x | 4.763 | 101.291 | 21.27x |
| mse_n1000000_k2 | 0.459 | 1.882 | 4.10x | 0.459 | 1.880 | 4.10x |
| mae_n1000000_k2 | 0.458 | 2.460 | 5.37x | 0.458 | 2.460 | 5.37x |
| median_ae_n1000000_k2 | 7.495 | 14.577 | 1.94x | 7.716 | 14.577 | 1.89x |
| r2_n1000000_k2 | 1.116 | 3.137 | 2.81x | 1.116 | 3.131 | 2.81x |
| confusion_matrix_n1000000_k10 | 5.076 | 101.702 | 20.03x | 5.076 | 101.529 | 20.00x |
| accuracy_n1000000_k10 | 0.132 | 33.928 | 257.16x | 0.132 | 33.923 | 257.14x |
| precision_recall_f1_macro_n1000000_k10 | 5.077 | 116.280 | 22.90x | 5.078 | 116.273 | 22.90x |
| classification_report_n1000000_k10 | 5.084 | 239.941 | 47.19x | 5.084 | 239.927 | 47.19x |
| balanced_accuracy_n1000000_k10 | 5.075 | 102.269 | 20.15x | 5.075 | 102.265 | 20.15x |
| matthews_n1000000_k10 | 5.073 | 211.202 | 41.63x | 5.073 | 211.177 | 41.63x |
| cohen_kappa_n1000000_k10 | 5.070 | 102.084 | 20.14x | 5.070 | 102.080 | 20.13x |
| mse_n1000000_k10 | 0.464 | 2.135 | 4.61x | 0.464 | 2.131 | 4.60x |
| mae_n1000000_k10 | 0.465 | 2.526 | 5.43x | 0.465 | 2.526 | 5.43x |
| median_ae_n1000000_k10 | 6.418 | 14.682 | 2.29x | 6.473 | 14.681 | 2.27x |
| r2_n1000000_k10 | 1.142 | 3.389 | 2.97x | 1.142 | 3.387 | 2.97x |

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
| transform_frobenius_n100 | 2.463 | 0.867 | 0.35x | 2.463 | 0.867 | 0.35x |
| transform_frobenius_n500 | 7.818 | 1.683 | 0.22x | 7.818 | 1.683 | 0.22x |
| transform_frobenius_n2000 | 28.345 | 5.814 | 0.21x | 28.387 | 23.048 | 0.81x |
| transform_kl_n100 | 1.131 | 17.653 | 15.61x | 1.131 | 17.653 | 15.61x |
| transform_kl_n500 | 5.082 | 34.623 | 6.81x | 5.082 | 34.622 | 6.81x |
| transform_kl_n2000 | 20.274 | 123.314 | 6.08x | 20.361 | 448.869 | 22.05x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.3', 'sentencepiece': '0.2.3', 'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.652 | 9.731 | 1.72x | 5.844 | 9.731 | 1.67x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.520 | 17.246 | 1.38x | 12.902 | 17.245 | 1.34x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.712 | 41.528 | 3.03x | 13.859 | 41.524 | 3.00x | 1,990,038 | 1,990,038 |
| spiece_model | 6.340 | 15.088 | 2.38x | 6.611 | 15.087 | 2.28x | 347,061 | 347,061 |
| tfidf_save | 1.611 | 2.513 | 1.56x | 1.653 | 2.513 | 1.52x | 581,787 | 591,922 |
| tfidf_load | 4.963 | 4.134 | 0.83x | 5.230 | 4.134 | 0.79x | 581,787 | 591,922 |
| embedding_index_save | 5.003 | 1.775 | 0.35x | 5.193 | 1.775 | 0.34x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.455 | 37.484 | 0.76x | 11.353 | 5.176 | 0.46x | 20,589,007 | 15,360,128 |
| embedding_index_load | 7.835 | 1.640 | 0.21x | 8.305 | 1.635 | 0.20x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 7.786 | 1.058 | 0.14x | 8.332 | 1.058 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 6.371 | 1.645 | 0.26x | 6.747 | 1.636 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 2.682 | 1.596 | 0.60x | 3.020 | 1.596 | 0.53x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 88.20x | 0.000 | 0.001 | 88.20x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 413.694 | 584.562 | 1.41x | 416.589 | 584.540 | 1.40x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 73.984 | 66.942 | 0.90x | 74.024 | 66.942 | 0.90x | 15,250,490 | 14,022,374 |

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
| kfold_n10000 | 0.092 | 0.113 | 1.22x | 0.092 | 0.113 | 1.22x |
| stratified_n10000 | 0.188 | 0.859 | 4.56x | 0.188 | 0.859 | 4.56x |
| traintest_n10000 | 0.005 | 0.234 | 47.42x | 0.005 | 0.234 | 47.41x |
| kfold_seeded_n10000 | 0.307 | 0.655 | 2.13x | 0.307 | 0.655 | 2.13x |
| stratified_seeded_n10000 | 0.438 | 1.436 | 3.28x | 0.438 | 1.436 | 3.28x |
| traintest_seeded_n10000 | 0.165 | 0.497 | 3.02x | 0.165 | 0.497 | 3.02x |
| stratified_traintest_n10000 | 0.667 | 2.672 | 4.01x | 0.667 | 2.672 | 4.01x |
| group_kfold_n10000 | 0.122 | 0.580 | 4.74x | 0.122 | 0.580 | 4.74x |
| stratified_group_kfold_n10000 | 0.241 | 30.769 | 127.89x | 0.241 | 30.766 | 127.89x |
| timeseries_n10000 | 0.000 | 0.012 | 86.54x | 0.000 | 0.012 | 86.54x |
| repeated_kfold_n10000 | 0.992 | 1.758 | 1.77x | 0.992 | 1.758 | 1.77x |
| kfold_n100000 | 1.196 | 2.433 | 2.03x | 1.359 | 2.433 | 1.79x |
| stratified_n100000 | 2.174 | 7.139 | 3.28x | 2.337 | 7.119 | 3.05x |
| traintest_n100000 | 0.222 | 0.399 | 1.80x | 0.224 | 0.399 | 1.78x |
| kfold_seeded_n100000 | 3.591 | 7.267 | 2.02x | 3.781 | 7.251 | 1.92x |
| stratified_seeded_n100000 | 4.811 | 12.586 | 2.62x | 5.066 | 12.569 | 2.48x |
| traintest_seeded_n100000 | 2.030 | 2.003 | 0.99x | 2.077 | 2.003 | 0.96x |
| stratified_traintest_n100000 | 8.090 | 23.733 | 2.93x | 8.204 | 23.715 | 2.89x |
| group_kfold_n100000 | 1.645 | 7.111 | 4.32x | 1.824 | 7.092 | 3.89x |
| stratified_group_kfold_n100000 | 2.772 | 308.106 | 111.15x | 2.962 | 308.082 | 104.02x |
| timeseries_n100000 | 0.000 | 0.043 | 324.05x | 0.000 | 0.043 | 324.11x |
| repeated_kfold_n100000 | 10.436 | 21.091 | 2.02x | 10.630 | 21.090 | 1.98x |
| kfold_n1000000 | 11.468 | 20.720 | 1.81x | 12.372 | 20.718 | 1.67x |
| stratified_n1000000 | 23.514 | 67.689 | 2.88x | 24.962 | 67.515 | 2.70x |
| traintest_n1000000 | 0.803 | 2.116 | 2.64x | 0.948 | 2.109 | 2.22x |
| kfold_seeded_n1000000 | 39.483 | 62.558 | 1.58x | 41.193 | 62.482 | 1.52x |
| stratified_seeded_n1000000 | 52.596 | 109.271 | 2.08x | 54.643 | 109.266 | 2.00x |
| traintest_seeded_n1000000 | 20.730 | 19.196 | 0.93x | 21.136 | 19.195 | 0.91x |
| stratified_traintest_n1000000 | 93.846 | 235.609 | 2.51x | 94.361 | 235.600 | 2.50x |
| group_kfold_n1000000 | 18.579 | 57.014 | 3.07x | 19.864 | 56.973 | 2.87x |
| stratified_group_kfold_n1000000 | 31.294 | 3081.712 | 98.48x | 33.263 | 3079.309 | 92.57x |
| timeseries_n1000000 | 0.000 | 0.328 | 2925.63x | 0.000 | 0.328 | 2925.82x |
| repeated_kfold_n1000000 | 112.376 | 182.911 | 1.63x | 115.079 | 182.906 | 1.59x |

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
| welch_t_n1000 | 0.008 | 0.803 | 100.46x | 0.008 | 0.803 | 100.43x |
| mann_whitney_n1000 | 0.030 | 0.723 | 23.79x | 0.030 | 0.723 | 23.79x |
| chi_square_n1000 | 0.000 | 0.332 | 1657.56x | 0.000 | 0.332 | 1657.50x |
| fligner_n1000 | 0.073 | 0.796 | 10.83x | 0.073 | 0.796 | 10.84x |
| anderson_ksamp_n1000 | 0.164 | 0.393 | 2.40x | 0.164 | 0.393 | 2.40x |
| spearman_matrix_n1000 | 0.128 | 0.910 | 7.10x | 0.128 | 0.910 | 7.10x |
| pointbiserial_n1000 | 0.010 | 0.566 | 54.09x | 0.010 | 0.566 | 54.08x |
| welch_t_n10000 | 0.053 | 0.853 | 15.98x | 0.053 | 0.853 | 15.98x |
| mann_whitney_n10000 | 0.682 | 2.803 | 4.11x | 0.682 | 2.803 | 4.11x |
| chi_square_n10000 | 0.001 | 0.335 | 239.97x | 0.001 | 0.335 | 239.93x |
| fligner_n10000 | 1.443 | 3.228 | 2.24x | 1.533 | 3.228 | 2.11x |
| anderson_ksamp_n10000 | 2.054 | 4.025 | 1.96x | 2.188 | 4.025 | 1.84x |
| spearman_matrix_n10000 | 2.149 | 4.774 | 2.22x | 2.215 | 4.774 | 2.16x |
| pointbiserial_n10000 | 0.100 | 0.622 | 6.21x | 0.100 | 0.622 | 6.21x |
| welch_t_n100000 | 0.518 | 1.245 | 2.40x | 0.518 | 1.245 | 2.40x |
| mann_whitney_n100000 | 7.746 | 28.891 | 3.73x | 7.746 | 28.889 | 3.73x |
| chi_square_n100000 | 0.014 | 0.360 | 25.70x | 0.014 | 0.360 | 25.70x |
| fligner_n100000 | 14.892 | 33.398 | 2.24x | 15.345 | 33.396 | 2.18x |
| anderson_ksamp_n100000 | 15.780 | 43.762 | 2.77x | 16.185 | 43.760 | 2.70x |
| spearman_matrix_n100000 | 19.245 | 54.632 | 2.84x | 19.831 | 54.629 | 2.75x |
| pointbiserial_n100000 | 1.300 | 1.380 | 1.06x | 1.318 | 5.506 | 4.18x |

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
| normalize_n10000 | 0.282 | 0.457 | 1.62x | 0.283 | 0.457 | 1.61x |
| polynomial_n10000 | 0.778 | 1.015 | 1.30x | 0.785 | 1.015 | 1.29x |
| kbins_fit_n10000 | 1.469 | 2.592 | 1.76x | 1.469 | 2.592 | 1.76x |
| kbins_n10000 | 0.740 | 4.723 | 6.39x | 0.853 | 4.723 | 5.54x |
| quantile_fit_n10000 | 1.568 | 3.807 | 2.43x | 1.569 | 3.807 | 2.43x |
| quantile_n10000 | 1.843 | 1.708 | 0.93x | 1.847 | 1.708 | 0.92x |
| power_fit_n10000 | 5.919 | 128.264 | 21.67x | 5.923 | 128.256 | 21.65x |
| power_n10000 | 1.568 | 1.434 | 0.91x | 1.571 | 1.434 | 0.91x |
| knn_impute_n10000 | 7.326 | 25.315 | 3.46x | 7.326 | 99.906 | 13.64x |
| label_n10000 | 1.162 | 0.775 | 0.67x | 1.162 | 0.775 | 0.67x |
| normalize_n100000 | 1.578 | 1.925 | 1.22x | 2.018 | 1.925 | 0.95x |
| polynomial_n100000 | 3.910 | 7.544 | 1.93x | 4.770 | 7.542 | 1.58x |
| kbins_fit_n100000 | 15.083 | 8.354 | 0.55x | 15.961 | 8.353 | 0.52x |
| kbins_n100000 | 8.030 | 13.231 | 1.65x | 9.053 | 13.230 | 1.46x |
| quantile_fit_n100000 | 15.430 | 18.782 | 1.22x | 16.223 | 18.780 | 1.16x |
| quantile_n100000 | 16.941 | 15.557 | 0.92x | 17.429 | 15.556 | 0.89x |
| power_fit_n100000 | 63.743 | 873.707 | 13.71x | 66.698 | 873.675 | 13.10x |
| power_n100000 | 14.491 | 9.648 | 0.67x | 14.993 | 9.647 | 0.64x |
| knn_impute_n100000 | 7.316 | 19.632 | 2.68x | 7.316 | 78.356 | 10.71x |
| label_n100000 | 15.513 | 7.425 | 0.48x | 15.810 | 7.425 | 0.47x |

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
- `StopWordBenchmarks`
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

**New tonight: 13.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 0.48 | 2.15 | -78% | 20% |
| `IndelBenchmarks` | Length=32 | `Distance_CodePoint` | step | 0.38 | 1.11 | -66% | 30% |
| `IndelBenchmarks` | Length=32 | `NormalizedSimilarity_Utf16` | step | 0.36 | 1.05 | -66% | 30% |
| `IndelBenchmarks` | Length=32 | `SubsequenceLength_Utf16` | step | 0.34 | 0.99 | -66% | 30% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x50 | `MetaNumerics_VarianceFraction` | step | 271.84 | 171.29 | +59% | 32% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Binomial` | step | 1.6 | 2.32 | -31% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_AndersonDarling` | drift | 2.18 | 3.1 | -27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Levene` | drift | 1.44 | 1.98 | -27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_AndersonDarling` | drift | 3.17 | 4.25 | -25% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_ClopperPearson` | drift | 1.51 | 2.02 | -24% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Binomial` | drift | 0.28 | 0.37 | -22% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Bartlett` | drift | 0.21 | 0.28 | -21% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Bartlett` | drift | 0.41 | 0.52 | -21% | 20% |

**Still away from their median: 15.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_ChiSquare` | step | 0.15 | 0.67 | -78% | 34% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | step | 0.34 | 1.51 | -77% | 31% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | step | 0.69 | 3.06 | -77% | 74% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | step | 0.1 | 0.44 | -77% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | step | 0.85 | 3.69 | -77% | 35% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | step | 2.1 | 8.92 | -76% | 45% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | step | 0.23 | 0.97 | -76% | 57% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | step | 0.88 | 3.71 | -76% | 40% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_Wilcoxon` | step | 0.25 | 1.05 | -76% | 38% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | step | 0.51 | 2.14 | -76% | 49% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | step | 1.68 | 6.71 | -75% | 35% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | step | 0.57 | 2.23 | -74% | 47% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | step | 0.03 | 0.11 | -73% | 35% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x10 | `MetaNumerics_VarianceFraction` | step | 1288.52 | 751.97 | +71% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | step | 0.36 | 0.83 | -57% | 43% |
