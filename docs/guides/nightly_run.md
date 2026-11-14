# Nightly benchmark run

<!-- nightly-baseline: f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks CoxBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks SurvivalBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks WeightedLeastSquaresBenchmarks cluster-weighted cox-extended cross-conformal glm iv nmf-transform ols onehot panel survival-aalen survival-family survival-parametric var -->
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

- Commit: `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`
- Previous run: `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`
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

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error             | StdDev           | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|------------------:|-----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **932.4 ns** |          **17.35 ns** |          **0.95 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         720.7 ns |          34.59 ns |          1.90 ns |     0.77 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,793.2 ns |         402.63 ns |         22.07 ns |     3.00 |    0.02 |   0.0458 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,726.5 ns |         214.04 ns |         11.73 ns |     4.00 |    0.01 |   0.0229 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       4,901.3 ns |         628.38 ns |         34.44 ns |     5.26 |    0.03 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,843.0 ns |         920.90 ns |         50.48 ns |    11.63 |    0.05 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       4,902.3 ns |         241.89 ns |         13.26 ns |     5.26 |    0.01 |        - |        - |        - |         - |          NA |
|                                |           |                  |                   |                  |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **83,453.7 ns** |       **1,222.00 ns** |         **66.98 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      52,058.4 ns |       2,171.23 ns |        119.01 ns |     0.62 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,059,710.9 ns |      33,597.63 ns |      1,841.60 ns |    12.70 |    0.02 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   2,008,540.7 ns |      70,217.72 ns |      3,848.87 ns |    24.07 |    0.04 |        - |        - |        - |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,562,808.0 ns |      95,808.00 ns |      5,251.56 ns |    30.71 |    0.06 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 309,614,836.3 ns | 230,259,287.15 ns | 12,621,288.55 ns | 3,710.02 |  131.00 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,563,228.5 ns |     146,790.48 ns |      8,046.08 ns |    30.71 |    0.09 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **55.94 μs** |  **18.99 μs** | **1.041 μs** |  **1.00** |    **0.02** | **0.1831** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    73.25 μs |  19.49 μs | 1.069 μs |  1.31 |    0.03 | 0.1221 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,798.00 μs** | **106.54 μs** | **5.840 μs** |  **1.00** |    **0.00** |      **-** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,507.56 μs |  82.02 μs | 4.496 μs |  0.84 |    0.00 |      - |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|-------------:|------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **3,383.5 ns** |    **109.35 ns** |     **5.99 ns** |   **1.00** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,044.9 ns |    164.64 ns |     9.02 ns |   0.60 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,489.9 ns |    255.63 ns |    14.01 ns |   0.74 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,837.0 ns |    311.83 ns |    17.09 ns |   1.13 |    0.00 | 0.0076 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,125.8 ns |    134.02 ns |     7.35 ns |   2.11 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     8,353.6 ns |    821.65 ns |    45.04 ns |   2.47 |    0.01 | 0.0153 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,659.0 ns |     33.78 ns |     1.85 ns |   0.79 |    0.00 | 0.0191 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,759.4 ns |    337.24 ns |    18.49 ns |   1.11 |    0.01 | 0.0114 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,267.9 ns |     10.07 ns |     0.55 ns |   0.37 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,796.9 ns |    118.21 ns |     6.48 ns |   0.83 |    0.00 | 0.0076 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,170.0 ns |     77.00 ns |     4.22 ns |   0.35 |    0.00 | 0.0095 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,255.3 ns |     55.55 ns |     3.04 ns |   0.37 |    0.00 | 0.0114 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       307.0 ns |      3.03 ns |     0.17 ns |   0.09 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,319.6 ns |    250.56 ns |    13.73 ns |   0.39 |    0.00 | 0.0095 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       102.1 ns |      9.29 ns |     0.51 ns |   0.03 |    0.00 | 0.0019 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       655.0 ns |     68.77 ns |     3.77 ns |   0.19 |    0.00 | 0.0114 |     968 B |       15.12 |
|                                |            |                |              |             |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **55,984.2 ns** |  **9,924.03 ns** |   **543.97 ns** |  **1.000** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   168,402.1 ns |  1,609.30 ns |    88.21 ns |  3.008 |    0.03 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   511,926.6 ns |  5,555.12 ns |   304.50 ns |  9.145 |    0.08 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,132,640.5 ns | 64,979.99 ns | 3,561.77 ns | 38.096 |    0.33 |      - |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,209,873.9 ns | 99,722.69 ns | 5,466.14 ns | 21.612 |    0.20 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,651,902.3 ns | 57,552.44 ns | 3,154.64 ns | 65.235 |    0.55 |      - |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,510,253.9 ns | 23,511.84 ns | 1,288.76 ns | 26.978 |    0.23 |      - |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,143,135.0 ns | 90,513.96 ns | 4,961.38 ns | 38.283 |    0.33 |      - |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    62,399.9 ns |  1,124.80 ns |    61.65 ns |  1.115 |    0.01 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   253,093.0 ns |  4,879.76 ns |   267.48 ns |  4.521 |    0.04 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   252,222.4 ns | 10,987.75 ns |   602.28 ns |  4.506 |    0.04 | 0.4883 |   80057 B |    1,250.89 |
| MetaNumerics_Wilcoxon          | 10000      |   448,462.4 ns | 27,320.14 ns | 1,497.51 ns |  8.011 |    0.07 | 0.4883 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       309.6 ns |     28.38 ns |     1.56 ns |  0.006 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,316.5 ns |     31.63 ns |     1.73 ns |  0.024 |    0.00 | 0.0095 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       102.4 ns |      2.45 ns |     0.13 ns |  0.002 |    0.00 | 0.0019 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       651.1 ns |     77.87 ns |     4.27 ns |  0.012 |    0.00 | 0.0114 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean           | Error        | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|-------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,693.3 ns** |    **248.95 ns** |    **13.65 ns** |  **1.00** |    **0.01** | **0.0572** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,794.3 ns |    137.81 ns |     7.55 ns |  1.41 |    0.01 | 0.0648 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       400.4 ns |     14.54 ns |     0.80 ns |  0.15 |    0.00 | 0.0010 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |       780.3 ns |     74.77 ns |     4.10 ns |  0.29 |    0.00 | 0.0019 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       717.0 ns |     30.54 ns |     1.67 ns |  0.27 |    0.00 |      - |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     3,565.2 ns |    479.97 ns |    26.31 ns |  1.32 |    0.01 | 0.0229 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,540.0 ns |    651.58 ns |    35.72 ns |  3.17 |    0.02 |      - |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     5,554.5 ns |    680.36 ns |    37.29 ns |  2.06 |    0.02 | 0.0076 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,386.5 ns |    270.39 ns |    14.82 ns |  1.63 |    0.01 |      - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |        |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **380,379.6 ns** | **14,597.75 ns** |   **800.15 ns** | **1.000** |    **0.00** | **5.3711** | **1.4648** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,291,139.6 ns | 38,580.49 ns | 2,114.73 ns | 3.394 |    0.01 | 3.9063 |      - |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    40,612.2 ns |  1,804.81 ns |    98.93 ns | 0.107 |    0.00 |      - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |    81,019.7 ns |  2,257.01 ns |   123.71 ns | 0.213 |    0.00 |      - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       927.9 ns |     51.28 ns |     2.81 ns | 0.002 |    0.00 |      - |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   691,946.9 ns | 58,561.40 ns | 3,209.95 ns | 1.819 |    0.01 | 1.9531 |      - |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,393,206.9 ns | 30,955.21 ns | 1,696.76 ns | 3.663 |    0.01 |      - |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |     NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,056.9 ns |    168.93 ns |     9.26 ns | 0.029 |    0.00 |      - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,809.5 μs** |     **115.59 μs** |      **6.34 μs** |   **1.00** |    **0.00** |   **248.0469** |  **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   101,627.3 μs |   2,974.75 μs |    163.06 μs |  56.16 |    0.19 |   666.6667 |  500.0000 |         - |  59880.93 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **2,616.0 μs** |     **142.64 μs** |      **7.82 μs** |   **1.00** |    **0.00** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    98,048.4 μs |  17,700.46 μs |    970.22 μs |  37.48 |    0.34 |  1000.0000 |  800.0000 |         - |  83110.15 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **767.5 μs** |      **32.46 μs** |      **1.78 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   129,464.6 μs |  28,968.37 μs |  1,587.85 μs | 168.68 |    1.82 |   750.0000 |  250.0000 |         - |  81288.43 KB |    2,678.83 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,968.6 μs** |     **130.20 μs** |      **7.14 μs** |   **1.00** |    **0.00** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    79,790.2 μs | 164,830.98 μs |  9,034.94 μs |  40.53 |    3.98 |   714.2857 |  428.5714 |         - |  64189.77 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **14,769.2 μs** |     **477.46 μs** |     **26.17 μs** |   **1.00** |    **0.00** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,631,113.2 μs | 262,767.31 μs | 14,403.16 μs | 110.44 |    0.86 |  6000.0000 | 3000.0000 | 1000.0000 | 537131.64 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,064.7 μs** |   **3,066.96 μs** |    **168.11 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,869,367.4 μs |   3,482.25 μs |    190.87 μs | 132.92 |    1.39 | 10000.0000 | 5000.0000 | 2000.0000 | 747334.48 KB |       83.73 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,250.8 μs** |     **312.36 μs** |     **17.12 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,024,853.4 μs |  87,663.23 μs |  4,805.12 μs | 483.91 |    1.33 |  9000.0000 | 5000.0000 | 2000.0000 | 732665.26 KB |    8,147.80 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,250.5 μs** |   **4,577.94 μs** |    **250.93 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,522,229.7 μs |  62,356.41 μs |  3,417.97 μs |  93.69 |    1.27 |  8000.0000 | 5000.0000 | 2000.0000 | 573606.88 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| CountLoadAsync2M | 18.62 ms |  7.219 ms | 0.396 ms | 468.7500 | 437.5000 | 406.2500 |  21.53 MB |
| CountLoadAsync4M | 42.96 ms | 40.160 ms | 2.201 ms | 500.0000 | 416.6667 | 333.3333 |  43.93 MB |
| CountLoad4M      | 42.93 ms | 50.041 ms | 2.743 ms | 538.4615 | 461.5385 | 384.6154 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error         | StdDev       | Gen0     | Gen1     | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|---------:|---------:|---------:|------------:|
| CountFitLongDocuments | 179,745.71 μs | 16,931.868 μs |   928.093 μs |        - |        - |        - | 54334.53 KB |
| CountSave240K         |     362.37 μs |     64.213 μs |     3.520 μs |   3.9063 |   2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     433.57 μs |     13.036 μs |     0.715 μs |   5.8594 |   4.8828 |   4.8828 |   739.44 KB |
| TfidfSave45K          |      51.67 μs |      0.409 μs |     0.022 μs |   1.6479 |   0.3052 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      57.32 μs |      7.198 μs |     0.395 μs |   2.1973 |   0.4883 |        - |   181.88 KB |
| IndexSave942K         |     234.01 μs |     11.014 μs |     0.604 μs |   1.4648 |   0.4883 |   0.4883 |    243.5 KB |
| IndexSaveAsync942K    |     234.98 μs |     15.531 μs |     0.851 μs |   1.4648 |   0.4883 |   0.4883 |   243.62 KB |
| CountLoadAsync2M      |  22,759.77 μs | 24,739.633 μs | 1,356.063 μs | 156.2500 | 125.0000 |  93.7500 | 22048.73 KB |
| CountLoadAsync4M      |  52,097.59 μs | 78,304.875 μs | 4,292.154 μs | 333.3333 | 250.0000 | 166.6667 | 44985.12 KB |
| CountLoad4M           |  45,382.77 μs | 90,180.420 μs | 4,943.093 μs | 272.7273 | 181.8182 | 181.8182 | 44976.65 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     16.612 μs |    18.893 μs |   1.0356 μs |   0.3357 |        - |        - |    27.43 KB |
| CountSaveAsyncWide     |    535.778 μs |   371.458 μs |  20.3609 μs |  27.3438 |  26.3672 |  26.3672 |   740.17 KB |
| HashingLoadUndeclared  |      7.665 μs |     7.715 μs |   0.4229 μs |   1.0223 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     29.123 μs |    11.517 μs |   0.6313 μs |   1.4648 |   0.2747 |        - |   121.83 KB |
| CountLoadUndeclared24K |    122.637 μs |     5.197 μs |   0.2848 μs |   3.1738 |        - |        - |   264.67 KB |
| CountLoadUndeclared78K |    647.269 μs |   375.935 μs |  20.6063 μs |  11.7188 |   9.7656 |   5.8594 |   656.33 KB |
| CountLoadUndeclared1M  |  8,432.434 μs | 2,693.503 μs | 147.6400 μs | 359.3750 | 343.7500 | 328.1250 | 10793.88 KB |
| CountSaveAsync1M       |  2,572.104 μs | 5,012.978 μs | 274.7782 μs | 101.5625 | 101.5625 | 101.5625 |  3162.03 KB |
| IndexSaveAsyncLongHead |  3,395.208 μs |   124.743 μs |   6.8376 μs |  70.3125 |  70.3125 |  70.3125 |  2124.17 KB |
| CountLoadUndeclared2M  | 16,952.668 μs | 1,216.641 μs |  66.6882 μs | 593.7500 | 562.5000 | 531.2500 | 22045.65 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.402 μs** |   **0.8863 μs** | **0.0486 μs** |  **1.00** |    **0.01** | **0.0229** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.678 μs |   0.4268 μs | 0.0234 μs |  1.05 |    0.01 | 0.0305 |      - |   2.81 KB |        1.24 |
| EmbedBatchBucketed | 1          |   5.769 μs |   0.9679 μs | 0.0531 μs |  1.07 |    0.01 | 0.0305 |      - |   2.81 KB |        1.24 |
|                    |            |            |             |           |       |         |        |        |           |             |
| **UnitLoop**           | **8**          |  **54.281 μs** |   **6.7210 μs** | **0.3684 μs** |  **1.00** |    **0.01** | **0.3052** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  19.527 μs |   0.4322 μs | 0.0237 μs |  0.36 |    0.00 | 0.2441 |      - |  22.38 KB |        0.76 |
| EmbedBatchBucketed | 8          |  19.327 μs |   1.5343 μs | 0.0841 μs |  0.36 |    0.00 | 0.2441 |      - |  22.38 KB |        0.76 |
|                    |            |            |             |           |       |         |        |        |           |             |
| **UnitLoop**           | **32**         | **209.265 μs** |  **10.1001 μs** | **0.5536 μs** |  **1.00** |    **0.00** | **1.2207** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  75.806 μs | 129.9333 μs | 7.1221 μs |  0.36 |    0.03 | 0.9766 |      - |  82.75 KB |        0.75 |
| EmbedBatchBucketed | 32         |  69.426 μs |   4.5719 μs | 0.2506 μs |  0.33 |    0.00 | 0.7324 |      - |  69.23 KB |        0.62 |
|                    |            |            |             |           |       |         |        |        |           |             |
| **UnitLoop**           | **128**        | **844.320 μs** |  **66.1532 μs** | **3.6261 μs** |  **1.00** |    **0.01** | **4.8828** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 284.584 μs |  41.6839 μs | 2.2848 μs |  0.34 |    0.00 | 3.9063 | 0.4883 | 329.78 KB |        0.74 |
| EmbedBatchBucketed | 128        | 251.090 μs |  40.9620 μs | 2.2453 μs |  0.30 |    0.00 | 2.9297 |      - | 262.27 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **141.96 ms** | **38.043 ms** | **2.085 ms** |  **1.00** |    **0.02** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  80.52 ms |  3.476 ms | 0.191 ms |  0.57 |    0.01 |  103.71 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **146.44 ms** | **60.222 ms** | **3.301 ms** |  **1.00** |    **0.03** |   **23.79 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  75.27 ms |  8.520 ms | 0.467 ms |  0.51 |    0.01 |  116.52 KB |        4.90 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **210.73 ms** |  **2.998 ms** | **0.164 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 321.76 ms | 37.831 ms | 2.074 ms |  1.53 |    0.01 |  258.63 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **213.40 ms** |  **5.204 ms** | **0.285 ms** |  **1.00** |    **0.00** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 279.82 ms | 25.734 ms | 1.411 ms |  1.31 |    0.01 |  192.45 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **264.02 ms** |  **3.857 ms** | **0.211 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 443.19 ms | 46.442 ms | 2.546 ms |  1.68 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **265.94 ms** | **46.264 ms** | **2.536 ms** |  **1.00** |    **0.01** |  **741.42 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 411.16 ms | 22.725 ms | 1.246 ms |  1.55 |    0.01 | 1153.52 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **304.88 ms** |  **3.296 ms** | **0.181 ms** |  **1.00** |    **0.00** | **5113.42 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 513.16 ms |  5.646 ms | 0.309 ms |  1.68 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **307.75 ms** | **15.758 ms** | **0.864 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 508.37 ms | 33.876 ms | 1.857 ms |  1.65 |    0.01 | 7964.22 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **44.57 μs** |      **1.493 μs** |     **0.082 μs** |         **-** |
| Cjk    | 1000   |      58.80 μs |      5.571 μs |     0.305 μs |         - |
| **Latin**  | **10000**  |   **4,185.19 μs** |    **376.538 μs** |    **20.639 μs** |         **-** |
| Cjk    | 10000  |   6,723.08 μs |  1,175.217 μs |    64.418 μs |         - |
| **Latin**  | **65536**  | **189,090.01 μs** | **24,198.421 μs** | **1,326.397 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.910 μs** |      **0.1641 μs** |     **0.0090 μs** |     **1.00** |    **0.01** |    **0.0038** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.464 μs |      0.1634 μs |     0.0090 μs |     1.29 |    0.01 |    0.0038 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.153 μs |      0.0821 μs |     0.0045 μs |     1.65 |    0.01 |    0.0610 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   3,672.071 μs |    310.1428 μs |    17.0000 μs | 1,922.36 |   10.99 |  882.8125 |  882.8125 |  882.8125 |  3723179 B |    8,781.08 |
| LuceneFromText         | 1000      |   6,621.905 μs |  2,205.7782 μs |   120.9061 μs | 3,466.62 |   56.60 |   15.6250 |         - |         - |  1375046 B |    3,243.03 |
|                        |           |                |                |               |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **28.334 μs** |      **4.1086 μs** |     **0.2252 μs** |     **1.00** |    **0.01** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      39.300 μs |      1.6925 μs |     0.0928 μs |     1.39 |    0.01 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      15.954 μs |      1.4005 μs |     0.0768 μs |     0.56 |    0.00 |    0.0916 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  76,758.307 μs | 10,950.4419 μs |   600.2307 μs | 2,709.16 |   26.10 | 1571.4286 | 1571.4286 | 1571.4286 | 61663537 B |  145,432.87 |
| LuceneFromText         | 20000     | 131,681.104 μs | 61,217.7275 μs | 3,355.5502 μs | 4,647.64 |  107.40 |  250.0000 |         - |         - | 22417500 B |   52,871.46 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|---------:|----------:|------------:|
| Unigram | 25.47 ms | 8.441 ms | 0.463 ms |  1.00 |    0.02 |  93.7500 |   8.19 MB |        1.00 |
| Bpe     | 84.73 ms | 2.467 ms | 0.135 ms |  3.33 |    0.05 | 333.3333 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **18.33 μs** |  **0.229 μs** | **0.013 μs** | **0.0916** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **80.14 μs** | **10.471 μs** | **0.574 μs** | **0.1221** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **205.35 μs** |  **7.675 μs** | **0.421 μs** | **0.2441** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **426.93 μs** | **32.747 μs** | **1.795 μs** | **0.4883** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 8.175 ms | 3.627 ms | 0.1988 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error      | StdDev    | Allocated |
|----------- |--------- |-----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **15.838 μs** |  **0.4367 μs** | **0.0239 μs** |         **-** |
| MyersGroup | cjk      | 254.452 μs | 17.8749 μs | 0.9798 μs |         - |
| **DpGroup**    | **latin**    |   **8.491 μs** |  **0.3862 μs** | **0.0212 μs** |         **-** |
| MyersGroup | latin    | 140.113 μs |  6.9701 μs | 0.3821 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **292.59 μs** | **24.521 μs** | **1.344 μs** |  **1.00** |    **0.01** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 309.47 μs |  9.731 μs | 0.533 μs |  1.06 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **332.48 μs** |  **6.079 μs** | **0.333 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 343.95 μs | 14.321 μs | 0.785 μs |  1.03 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **305.72 μs** | **13.969 μs** | **0.766 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 331.98 μs |  5.069 μs | 0.278 μs |  1.09 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **276.16 μs** | **12.284 μs** | **0.673 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 337.74 μs |  9.179 μs | 0.503 μs |  1.22 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **288.48 μs** |  **3.000 μs** | **0.164 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 307.50 μs | 22.811 μs | 1.250 μs |  1.07 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **184.16 μs** |  **7.695 μs** | **0.422 μs** |  **1.00** |    **0.00** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 340.79 μs | 11.829 μs | 0.648 μs |  1.85 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **223.81 μs** |  **0.793 μs** | **0.043 μs** |  **1.00** |    **0.00** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 314.14 μs | 30.347 μs | 1.663 μs |  1.40 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **76.96 μs** |  **3.105 μs** | **0.170 μs** |  **1.00** |    **0.00** | **0.3662** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 339.76 μs | 22.738 μs | 1.246 μs |  4.41 |    0.02 |      - |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0    | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **170.7 μs** |  **14.31 μs** |  **0.78 μs** |  **1.00** |       **-** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   167.2 μs |  18.72 μs |  1.03 μs |  0.98 |       - |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   418.5 μs |  24.38 μs |  1.34 μs |  2.45 |  3.4180 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,264.6 μs** |   **8.91 μs** |  **0.49 μs** |  **1.00** | **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,218.6 μs | 349.07 μs | 19.13 μs |  0.99 | 97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 6,868.8 μs | 711.59 μs | 39.00 μs |  2.10 | 54.6875 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **140.22 ms** | **16.410 ms** | **0.899 ms** |  **1.00** |    **0.01** | **1750.0000** | **1750.0000** | **1750.0000** |  **29.41 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,778.79 ms |  3.519 ms | 0.193 ms | 12.69 |    0.07 | 8000.0000 | 7000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |           |          |       |         |           |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **50.94 ms** | **64.348 ms** | **3.527 ms** |  **1.00** |    **0.08** | **1454.5455** | **1454.5455** | **1454.5455** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   656.39 ms | 68.430 ms | 3.751 ms | 12.93 |    0.75 | 3000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|----------:|---------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **200.11 ms** | **58.143 ms** | **3.187 ms** |  **1.00** |    **0.02** |  **1333.3333** |  **1333.3333** | **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 5,301.71 ms | 26.283 ms | 1.441 ms | 26.50 |    0.36 | 11000.0000 |  7000.0000 | 6000.0000 | 531.85 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,154.69 ms | 24.285 ms | 1.331 ms | 15.77 |    0.22 | 11000.0000 | 10000.0000 | 9000.0000 | 372.11 MB |        3.30 |
|                          |            |             |           |          |       |         |            |            |           |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.46 ms** |  **1.277 ms** | **0.070 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** | **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   390.10 ms | 56.325 ms | 3.087 ms | 18.18 |    0.13 |  2000.0000 |  2000.0000 | 2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   225.88 ms |  9.229 ms | 0.506 ms | 10.53 |    0.04 |  3666.6667 |  3666.6667 | 3666.6667 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  14.004 ms |  3.5041 ms | 0.1921 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 109.387 ms | 24.5817 ms | 1.3474 ms |  7.81 |    0.12 |
| Nmf_Transform                             |   3.625 ms |  0.2522 ms | 0.0138 ms |  0.26 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  19.857 ms |  1.2223 ms | 0.0670 ms |  1.42 |    0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    91.67 ns |   4.440 ns |  0.243 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   714.07 ns | 352.552 ns | 19.325 ns |  7.79 |    0.18 |      - |         - |          NA |
| TokenSortRatio |   870.74 ns |  22.376 ns |  1.227 ns |  9.50 |    0.02 | 0.0134 |    1120 B |          NA |
| TokenSetRatio  |   889.36 ns |  23.202 ns |  1.272 ns |  9.70 |    0.03 | 0.0105 |     896 B |          NA |
| WRatio         | 1,268.13 ns | 136.028 ns |  7.456 ns | 13.83 |    0.08 | 0.0134 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|-------:|-------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,918,851.0293 ns |    77,928.5235 ns |   4,271.5253 ns |  1.000 |    0.00 |      - |      - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    657,775.9629 ns |    13,704.0785 ns |     751.1668 ns |  0.343 |    0.00 | 6.8359 | 1.9531 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,180,512.1270 ns |    38,692.3799 ns |   2,120.8599 ns |  0.615 |    0.00 | 7.8125 |      - |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 89,501,322.1111 ns | 9,949,748.6614 ns | 545,379.3003 ns | 46.643 |    0.26 |      - |      - |  512899 B |        3.20 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |      - |      - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **90.55 ns** |   **1.926 ns** |  **0.106 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   201.76 ns |   9.910 ns |  0.543 ns |  2.23 |    0.01 | 0.0010 |      80 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **708.66 ns** |  **63.541 ns** |  **3.483 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 9,068.91 ns | 158.926 ns |  8.711 ns | 12.80 |    0.06 |      - |     160 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **863.04 ns** |   **7.280 ns** |  **0.399 ns** |  **1.00** |    **0.00** | **0.0105** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,963.72 ns | 278.921 ns | 15.289 ns |  2.28 |    0.02 | 0.0229 |    1944 B |        2.17 |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        | **1,085.61 ns** |  **74.722 ns** |  **4.096 ns** |  **1.00** |    **0.00** | **0.0134** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 4,614.69 ns | 342.804 ns | 18.790 ns |  4.25 |    0.02 | 0.0305 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **25.39 ns** |   **0.709 ns** |  **0.039 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    33.50 ns |   5.169 ns |  0.283 ns |  1.32 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    26.11 ns |   2.292 ns |  0.126 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    23.55 ns |   2.803 ns |  0.154 ns |  0.93 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **27.73 ns** |   **4.135 ns** |  **0.227 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    35.44 ns |   4.235 ns |  0.232 ns |  1.28 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.65 ns |   0.618 ns |  0.034 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    24.14 ns |   0.086 ns |  0.005 ns |  0.87 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **29.07 ns** |   **3.975 ns** |  **0.218 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    39.34 ns |   1.760 ns |  0.096 ns |  1.35 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    30.52 ns |   7.253 ns |  0.398 ns |  1.05 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    27.48 ns |   0.322 ns |  0.018 ns |  0.95 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **32.31 ns** |   **0.552 ns** |  **0.030 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    39.65 ns |   3.127 ns |  0.171 ns |  1.23 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.01 ns |   0.411 ns |  0.023 ns |  1.08 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    31.22 ns |   0.903 ns |  0.049 ns |  0.97 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **49.70 ns** |   **0.675 ns** |  **0.037 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    55.72 ns |   3.533 ns |  0.194 ns |  1.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    53.15 ns |   3.306 ns |  0.181 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    48.81 ns |   1.484 ns |  0.081 ns |  0.98 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **60.51 ns** |   **4.227 ns** |  **0.232 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    63.82 ns |   8.484 ns |  0.465 ns |  1.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    62.98 ns |   9.820 ns |  0.538 ns |  1.04 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    56.12 ns |   1.660 ns |  0.091 ns |  0.93 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **344.88 ns** |  **26.606 ns** |  **1.458 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   385.65 ns |   3.593 ns |  0.197 ns |  1.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   348.20 ns |   9.519 ns |  0.522 ns |  1.01 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   341.29 ns |  24.601 ns |  1.348 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,656.00 ns** |  **28.863 ns** |  **1.582 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,664.74 ns | 106.142 ns |  5.818 ns |  1.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,639.67 ns | 329.233 ns | 18.046 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,584.84 ns | 172.539 ns |  9.457 ns |  0.98 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **365.1 ns** |  **23.89 ns** |  **1.31 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    165.5 ns |   3.71 ns |  0.20 ns |  0.45 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,139.4 ns** | **277.89 ns** | **15.23 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,579.8 ns | 240.73 ns | 13.20 ns |  1.21 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **13,636.1 ns** | **296.67 ns** | **16.26 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 33,330.3 ns | 714.49 ns | 39.16 ns |  2.44 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,634.1 ns** |   **358.99 ns** |  **19.68 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,385.1 ns |   451.04 ns |  24.72 ns |  0.97 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 10,482.7 ns |   206.33 ns |  11.31 ns |  1.37 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,562.8 ns** |   **182.47 ns** |  **10.00 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  8,829.4 ns | 1,661.56 ns |  91.08 ns |  0.84 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 13,710.2 ns |   235.40 ns |  12.90 ns |  1.30 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **15,276.6 ns** |   **327.74 ns** |  **17.96 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,307.3 ns | 1,467.45 ns |  80.44 ns |  0.74 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,123.5 ns | 1,785.51 ns |  97.87 ns |  1.19 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,396.4 ns** |   **446.18 ns** |  **24.46 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,420.5 ns |   346.54 ns |  18.99 ns |  1.00 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  8,622.1 ns | 1,204.40 ns |  66.02 ns |  1.60 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,977.4 ns** |   **333.98 ns** |  **18.31 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,187.0 ns | 1,941.84 ns | 106.44 ns |  0.90 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,245.6 ns | 1,083.44 ns |  59.39 ns |  1.41 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **13,115.9 ns** |   **397.76 ns** |  **21.80 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,585.4 ns | 2,649.56 ns | 145.23 ns |  0.73 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 16,277.2 ns | 2,041.24 ns | 111.89 ns |  1.24 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,677.9 ns** |   **713.75 ns** |  **39.12 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  3,749.8 ns |   100.04 ns |   5.48 ns |  1.02 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  6,884.0 ns |   159.00 ns |   8.72 ns |  1.87 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **6,508.8 ns** |   **550.25 ns** |  **30.16 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,473.9 ns |   571.89 ns |  31.35 ns |  0.84 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,756.5 ns |   785.68 ns |  43.07 ns |  1.50 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **11,534.5 ns** |   **595.01 ns** |  **32.61 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  8,313.5 ns | 1,532.09 ns |  83.98 ns |  0.72 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,636.9 ns |   520.94 ns |  28.55 ns |  1.18 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,360.9 ns** |   **121.05 ns** |   **6.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  1,917.8 ns |    78.42 ns |   4.30 ns |  0.81 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,525.3 ns |   164.15 ns |   9.00 ns |  2.34 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **5,242.0 ns** |   **262.65 ns** |  **14.40 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,604.2 ns |   219.30 ns |  12.02 ns |  0.50 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,380.1 ns |   348.18 ns |  19.08 ns |  1.60 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     | **10,288.6 ns** |   **340.73 ns** |  **18.68 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,569.0 ns |   143.03 ns |   7.84 ns |  0.35 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,201.2 ns | 1,166.92 ns |  63.96 ns |  1.19 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,944.8 ns** |   **356.37 ns** |  **19.53 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,178.0 ns |   205.66 ns |  11.27 ns |  0.81 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,058.0 ns |   212.24 ns |  11.63 ns |  1.79 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **6,594.3 ns** |   **248.30 ns** |  **13.61 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  3,880.9 ns |   219.38 ns |  12.02 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     |  9,867.9 ns |   187.94 ns |  10.30 ns |  1.50 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **11,608.5 ns** |   **576.34 ns** |  **31.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,069.4 ns | 2,313.03 ns | 126.79 ns |  0.70 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 13,745.1 ns |   381.05 ns |  20.89 ns |  1.18 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,429.2 ns** |    **47.19 ns** |   **2.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    467.8 ns |    31.15 ns |   1.71 ns |  0.33 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,571.3 ns |   120.76 ns |   6.62 ns |  3.20 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,748.7 ns** |   **200.24 ns** |  **10.98 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,102.2 ns |    52.62 ns |   2.88 ns |  0.29 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,499.5 ns | 1,579.12 ns |  86.56 ns |  2.00 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **8,740.5 ns** |   **199.88 ns** |  **10.96 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,117.1 ns |   217.13 ns |  11.90 ns |  0.24 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,387.0 ns |    72.13 ns |   3.95 ns |  1.30 |    0.00 | 0.0610 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error       | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,614.5 μs** |    **490.8 μs** |    **26.90 μs** |  **1.00** |    **0.01** | **242.1875** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  35,656.2 μs |  6,724.2 μs |   368.58 μs |  6.35 |    0.06 |        - |        - |        - |   10.38 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  63,174.1 μs |  1,177.9 μs |    64.57 μs | 11.25 |    0.05 |        - |        - |        - |   41.86 KB |       0.028 |
|                  |             |              |             |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **861.0 μs** |    **104.5 μs** |     **5.73 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,080.7 μs |    344.1 μs |    18.86 μs |  4.74 |    0.03 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2,327.3 μs |    129.3 μs |     7.09 μs |  2.70 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |             |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **27,383.1 μs** |    **224.1 μs** |    **12.28 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.01 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 391,227.6 μs | 26,138.7 μs | 1,432.75 μs | 14.29 |    0.05 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 479,805.6 μs | 35,699.4 μs | 1,956.81 μs | 17.52 |    0.06 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.800 ms** | **0.3008 ms** | **0.0165 ms** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  14.643 ms | 0.1532 ms | 0.0084 ms |  2.52 |    0.01 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **29.277 ms** | **1.3358 ms** | **0.0732 ms** |  **1.00** |    **0.00** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  93.708 ms | 3.0647 ms | 0.1680 ms |  3.20 |    0.01 |        - |        - |        - |   96.44 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **55.901 ms** | **7.4052 ms** | **0.4059 ms** |  **1.00** |    **0.01** | **200.0000** | **200.0000** | **200.0000** | **3946.85 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 187.228 ms | 8.7749 ms | 0.4810 ms |  3.35 |    0.02 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error        | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|----------:|------:|----------:|------------:|
| **Dp**         | **8**    |    **121.66 ns** |    **11.754 ns** |  **0.644 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 8    |     54.28 ns |     1.660 ns |  0.091 ns |  0.45 |         - |          NA |
| Dp_Cjk     | 8    |    123.52 ns |    18.246 ns |  1.000 ns |  1.02 |         - |          NA |
| Kernel_Cjk | 8    |     88.63 ns |     1.230 ns |  0.067 ns |  0.73 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **12**   |    **223.18 ns** |    **21.564 ns** |  **1.182 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 12   |     64.83 ns |     6.793 ns |  0.372 ns |  0.29 |         - |          NA |
| Dp_Cjk     | 12   |    222.81 ns |     7.151 ns |  0.392 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 12   |    100.11 ns |     3.700 ns |  0.203 ns |  0.45 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **14**   |    **285.42 ns** |    **29.941 ns** |  **1.641 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 14   |     68.90 ns |     2.735 ns |  0.150 ns |  0.24 |         - |          NA |
| Dp_Cjk     | 14   |    286.29 ns |    23.044 ns |  1.263 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 14   |    105.18 ns |     6.765 ns |  0.371 ns |  0.37 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **16**   |    **347.98 ns** |     **8.306 ns** |  **0.455 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 16   |     72.72 ns |     8.427 ns |  0.462 ns |  0.21 |         - |          NA |
| Dp_Cjk     | 16   |    357.54 ns |    25.068 ns |  1.374 ns |  1.03 |         - |          NA |
| Kernel_Cjk | 16   |    113.11 ns |     2.235 ns |  0.123 ns |  0.33 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **18**   |    **418.52 ns** |    **10.588 ns** |  **0.580 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.91 ns |     3.660 ns |  0.201 ns |  0.18 |         - |          NA |
| Dp_Cjk     | 18   |    418.83 ns |    17.021 ns |  0.933 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 18   |    118.03 ns |     1.392 ns |  0.076 ns |  0.28 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **20**   |    **497.48 ns** |    **29.940 ns** |  **1.641 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 20   |     78.77 ns |    10.376 ns |  0.569 ns |  0.16 |         - |          NA |
| Dp_Cjk     | 20   |    495.76 ns |    14.569 ns |  0.799 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 20   |    121.05 ns |     1.893 ns |  0.104 ns |  0.24 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **24**   |    **673.19 ns** |    **10.684 ns** |  **0.586 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 24   |     83.94 ns |     1.744 ns |  0.096 ns |  0.12 |         - |          NA |
| Dp_Cjk     | 24   |    675.17 ns |    69.491 ns |  3.809 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 24   |    133.21 ns |    15.258 ns |  0.836 ns |  0.20 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **32**   |  **1,148.07 ns** |    **93.211 ns** |  **5.109 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 32   |    101.01 ns |     2.564 ns |  0.141 ns |  0.09 |         - |          NA |
| Dp_Cjk     | 32   |  1,147.36 ns |    44.951 ns |  2.464 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 32   |    154.22 ns |     9.152 ns |  0.502 ns |  0.13 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **48**   |  **2,616.58 ns** |   **435.084 ns** | **23.848 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 48   |    123.16 ns |    11.430 ns |  0.627 ns |  0.05 |         - |          NA |
| Dp_Cjk     | 48   |  2,453.15 ns |   218.909 ns | 11.999 ns |  0.94 |         - |          NA |
| Kernel_Cjk | 48   |    213.96 ns |    24.165 ns |  1.325 ns |  0.08 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **64**   |  **4,429.71 ns** |   **561.572 ns** | **30.782 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 64   |    148.20 ns |     5.477 ns |  0.300 ns |  0.03 |         - |          NA |
| Dp_Cjk     | 64   |  4,396.70 ns |   373.868 ns | 20.493 ns |  0.99 |         - |          NA |
| Kernel_Cjk | 64   |    260.32 ns |     1.241 ns |  0.068 ns |  0.06 |         - |          NA |
|            |      |              |              |           |       |           |             |
| **Dp**         | **96**   |  **9,773.62 ns** | **1,565.375 ns** | **85.804 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 96   |    305.81 ns |    39.728 ns |  2.178 ns |  0.03 |         - |          NA |
| Dp_Cjk     | 96   | 11,139.08 ns |   343.542 ns | 18.831 ns |  1.14 |         - |          NA |
| Kernel_Cjk | 96   |    799.19 ns |    64.419 ns |  3.531 ns |  0.08 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.77 ns** |     **3.315 ns** |  **0.182 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.81 ns |     0.472 ns |  0.026 ns |  1.00 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    139.31 ns |    14.335 ns |  0.786 ns |  5.62 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.39 ns |     3.563 ns |  0.195 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **281.17 ns** |     **1.209 ns** |  **0.066 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    352.19 ns |    13.876 ns |  0.761 ns |  1.25 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    690.33 ns |     5.084 ns |  0.279 ns |  2.46 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    285.89 ns |    13.919 ns |  0.763 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **852.72 ns** |   **129.834 ns** |  **7.117 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,695.24 ns |    64.313 ns |  3.525 ns |  1.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 128    |  1,557.14 ns |    84.298 ns |  4.621 ns |  1.83 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    889.18 ns |   113.566 ns |  6.225 ns |  1.04 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,944.37 ns** | **1,101.034 ns** | **60.351 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,712.82 ns |   531.839 ns | 29.152 ns |  1.40 |    0.01 |         - |          NA |
| Distance_CodePoint         | 512    | 13,566.35 ns |   832.937 ns | 45.656 ns |  1.14 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,241.05 ns |   191.381 ns | 10.490 ns |  0.94 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **344.7 ns** |     **17.20 ns** |     **0.94 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     215.4 ns |      3.88 ns |     0.21 ns |  0.62 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **342.8 ns** |      **0.70 ns** |     **0.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     215.4 ns |      7.58 ns |     0.42 ns |  0.63 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **433.3 ns** |     **87.02 ns** |     **4.77 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     305.6 ns |     13.55 ns |     0.74 ns |  0.71 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **431.8 ns** |     **23.00 ns** |     **1.26 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     308.6 ns |     59.32 ns |     3.25 ns |  0.71 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **523.2 ns** |     **51.81 ns** |     **2.84 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     395.3 ns |      4.44 ns |     0.24 ns |  0.76 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **532.0 ns** |     **85.02 ns** |     **4.66 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     393.3 ns |     11.14 ns |     0.61 ns |  0.74 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **618.1 ns** |     **19.02 ns** |     **1.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,117.6 ns |     97.88 ns |     5.37 ns |  1.81 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **614.7 ns** |     **48.90 ns** |     **2.68 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,143.3 ns |     21.62 ns |     1.19 ns |  1.86 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,886.2 ns** |     **67.93 ns** |     **3.72 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,033.2 ns |    481.07 ns |    26.37 ns |  2.67 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,871.1 ns** |    **133.41 ns** |     **7.31 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,862.3 ns |    187.61 ns |    10.28 ns |  2.60 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **15,058.4 ns** |    **122.49 ns** |     **6.71 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  61,756.9 ns | 27,370.04 ns | 1,500.24 ns |  4.10 |    0.09 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **512,475.7 ns** | **14,092.79 ns** |   **772.47 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  61,100.7 ns |    727.61 ns |    39.88 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.13 ns** |      **1.931 ns** |     **0.106 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |     110.73 ns |     13.580 ns |     0.744 ns |  4.41 |    0.03 | 0.0006 |      56 B |          NA |
| Quickenshtein        | 8      |      82.11 ns |      2.212 ns |     0.121 ns |  3.27 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     190.72 ns |     34.509 ns |     1.892 ns |  7.59 |    0.07 | 0.0014 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **281.57 ns** |     **10.893 ns** |     **0.597 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,327.07 ns |    361.113 ns |    19.794 ns | 18.92 |    0.07 |      - |     280 B |          NA |
| Quickenshtein        | 64     |   1,278.69 ns |     38.736 ns |     2.123 ns |  4.54 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   8,814.58 ns |  1,792.438 ns |    98.250 ns | 31.31 |    0.31 |      - |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,265.79 ns** |    **638.403 ns** |    **34.993 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 522,564.72 ns | 36,233.293 ns | 1,986.069 ns | 46.39 |    0.20 |      - |    2073 B |          NA |
| Quickenshtein        | 512    |  39,839.67 ns |  1,860.554 ns |   101.983 ns |  3.54 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 781,749.97 ns |  7,774.008 ns |   426.120 ns | 69.39 |    0.19 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error       | StdDev   | Gen0   | Gen1   | Allocated |
|------------------ |------- |---------:|------------:|---------:|-------:|-------:|----------:|
| **F1Macro**           | **1000**   | **499.8 μs** |    **23.14 μs** |  **1.27 μs** |      **-** |      **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 499.4 μs |    24.41 μs |  1.34 μs |      - |      - |  67.71 KB |
| ReportRows        | 1000   | 531.5 μs |    77.07 μs |  4.22 μs | 0.9766 |      - | 153.99 KB |
| **F1Macro**           | **5000**   | **678.0 μs** | **1,248.40 μs** | **68.43 μs** | **3.9063** | **0.9766** | **337.24 KB** |
| PrecisionWeighted | 5000   | 657.4 μs |    21.73 μs |  1.19 μs | 3.9063 | 0.9766 | 337.24 KB |
| ReportRows        | 5000   | 818.9 μs |    47.22 μs |  2.59 μs | 8.7891 | 3.9063 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean            | Error          | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |----------------:|---------------:|-------------:|---------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |       **145.41 μs** |      **12.742 μs** |     **0.698 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |   218,186.99 μs |  10,786.482 μs |   591.243 μs | 1,500.53 |    7.16 | 666.6667 | 666.6667 | 666.6667 | 31413.76 KB |   12,805.61 |
|                                 |         |                 |                |              |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |     **2,984.37 μs** |      **95.987 μs** |     **5.261 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 1,035,403.17 μs | 156,028.107 μs | 8,552.427 μs |   346.94 |    2.54 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |                 |                |              |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |        **23.51 μs** |       **2.832 μs** |     **0.155 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |       788.26 μs |      21.773 μs |     1.193 μs |    33.53 |    0.20 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,345.7 ns** |       **157.79 ns** |       **8.65 ns** | **0.0038** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     6,039.7 ns |       267.76 ns |      14.68 ns |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       137.1 ns |         0.99 ns |       0.05 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,717.6 ns |       207.05 ns |      11.35 ns | 0.0076 |     656 B |
| Report         | 1000    | 2       |     6,069.0 ns |       312.60 ns |      17.13 ns | 0.0610 |    5336 B |
| **Matrix**         | **1000**    | **10**      |     **3,861.3 ns** |       **176.98 ns** |       **9.70 ns** | **0.0153** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     6,210.6 ns |       530.60 ns |      29.08 ns | 0.0153 |    1336 B |
| AccuracyScore  | 1000    | 10      |       137.1 ns |        10.72 ns |       0.59 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,765.0 ns |       135.56 ns |       7.43 ns | 0.0191 |    1880 B |
| Report         | 1000    | 10      |     9,935.8 ns |       880.03 ns |      48.24 ns | 0.1373 |   12360 B |
| **Matrix**         | **100000**  | **2**       |   **341,797.7 ns** |    **22,729.29 ns** |   **1,245.87 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   733,949.1 ns |    40,937.52 ns |   2,243.92 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    15,990.1 ns |     1,170.25 ns |      64.15 ns |      - |         - |
| F1Macro        | 100000  | 2       |   361,891.2 ns |    11,831.09 ns |     648.50 ns |      - |     656 B |
| Report         | 100000  | 2       |   340,958.1 ns |    12,033.13 ns |     659.58 ns |      - |    5360 B |
| **Matrix**         | **100000**  | **10**      |   **337,526.8 ns** |    **39,394.26 ns** |   **2,159.33 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   887,754.9 ns |    47,656.79 ns |   2,612.23 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    15,905.0 ns |        56.24 ns |       3.08 ns |      - |         - |
| F1Macro        | 100000  | 10      |   313,604.8 ns |    18,034.57 ns |     988.54 ns |      - |    1880 B |
| Report         | 100000  | 10      |   339,692.5 ns |    22,969.68 ns |   1,259.05 ns |      - |   12704 B |
| **Matrix**         | **1000000** | **2**       | **3,709,609.8 ns** |   **407,408.31 ns** |  **22,331.42 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,788,331.9 ns | 1,937,317.93 ns | 106,190.93 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   253,751.0 ns |     4,303.03 ns |     235.86 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,789,317.8 ns |   294,973.56 ns |  16,168.50 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,701,343.1 ns |   389,352.57 ns |  21,341.73 ns |      - |    5379 B |
| **Matrix**         | **1000000** | **10**      | **3,621,563.7 ns** |   **330,494.44 ns** |  **18,115.52 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 9,252,968.7 ns |   434,659.58 ns |  23,825.16 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   255,617.6 ns |     2,669.02 ns |     146.30 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,515,947.8 ns |    65,445.75 ns |   3,587.30 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,485,835.7 ns | 1,313,414.88 ns |  71,992.70 ns |      - |   12747 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean          | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **10,664.98 μs** |     **376.528 μs** |     **20.639 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1292 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,141.27 μs |   5,680.807 μs |    311.384 μs |     3.30 |    0.03 | 666.6667 | 666.6667 | 666.6667 |  5089207 B |    3,939.01 |
|          |         |               |               |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **16.01 μs** |       **0.787 μs** |      **0.043 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  35,533.49 μs |   3,197.926 μs |    175.289 μs | 2,218.84 |   10.80 | 600.0000 | 600.0000 | 600.0000 |  5089603 B |          NA |
|          |         |               |               |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **175,720.01 μs** | **216,537.117 μs** | **11,869.130 μs** |     **1.00** |    **0.08** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 220,545.14 μs |  23,345.534 μs |  1,279.648 μs |     1.26 |    0.07 |        - |        - |        - | 23228948 B |          NA |
|          |         |               |               |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **255.85 μs** |       **3.730 μs** |      **0.204 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 217,987.75 μs |  15,900.787 μs |    871.576 μs |   852.02 |    3.01 |        - |        - |        - | 23229116 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-----------:|----------:|------:|----------:|------------:|
| **Dp**         | **4**    |     **72.40 ns** |   **1.038 ns** |  **0.057 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 4    |     72.67 ns |   4.713 ns |  0.258 ns |  1.00 |         - |          NA |
| Dp_Cjk     | 4    |     73.10 ns |   6.731 ns |  0.369 ns |  1.01 |         - |          NA |
| Kernel_Cjk | 4    |     72.46 ns |   5.611 ns |  0.308 ns |  1.00 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **6**    |     **98.32 ns** |   **3.638 ns** |  **0.199 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 6    |     71.73 ns |   8.414 ns |  0.461 ns |  0.73 |         - |          NA |
| Dp_Cjk     | 6    |     98.09 ns |   7.110 ns |  0.390 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 6    |    120.01 ns |  12.250 ns |  0.671 ns |  1.22 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **8**    |    **132.96 ns** |   **7.493 ns** |  **0.411 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 8    |     80.58 ns |   7.967 ns |  0.437 ns |  0.61 |         - |          NA |
| Dp_Cjk     | 8    |    133.16 ns |  11.743 ns |  0.644 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 8    |    153.51 ns |  12.086 ns |  0.662 ns |  1.15 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **10**   |    **180.40 ns** |  **12.490 ns** |  **0.685 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 10   |     89.43 ns |   0.951 ns |  0.052 ns |  0.50 |         - |          NA |
| Dp_Cjk     | 10   |    180.60 ns |  14.288 ns |  0.783 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 10   |    125.65 ns |   2.973 ns |  0.163 ns |  0.70 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **12**   |    **242.86 ns** |   **4.630 ns** |  **0.254 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 12   |    101.35 ns |   5.258 ns |  0.288 ns |  0.42 |         - |          NA |
| Dp_Cjk     | 12   |    243.91 ns |  24.371 ns |  1.336 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 12   |    138.61 ns |  13.726 ns |  0.752 ns |  0.57 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **16**   |    **373.20 ns** |  **10.848 ns** |  **0.595 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 16   |    119.58 ns |   2.929 ns |  0.161 ns |  0.32 |         - |          NA |
| Dp_Cjk     | 16   |    372.95 ns |  17.596 ns |  0.964 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 16   |    173.33 ns |  13.509 ns |  0.740 ns |  0.46 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **24**   |    **767.43 ns** |  **24.471 ns** |  **1.341 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 24   |    157.58 ns |  11.093 ns |  0.608 ns |  0.21 |         - |          NA |
| Dp_Cjk     | 24   |    770.50 ns |  44.808 ns |  2.456 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 24   |    203.30 ns |   7.921 ns |  0.434 ns |  0.26 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **32**   |  **1,392.50 ns** |  **54.480 ns** |  **2.986 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 32   |    195.54 ns |   0.619 ns |  0.034 ns |  0.14 |         - |          NA |
| Dp_Cjk     | 32   |  1,369.75 ns |  86.130 ns |  4.721 ns |  0.98 |         - |          NA |
| Kernel_Cjk | 32   |    248.77 ns |  19.002 ns |  1.042 ns |  0.18 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **48**   |  **2,983.09 ns** |  **33.137 ns** |  **1.816 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 48   |    272.50 ns |  37.824 ns |  2.073 ns |  0.09 |         - |          NA |
| Dp_Cjk     | 48   |  2,987.53 ns | 171.423 ns |  9.396 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 48   |    337.32 ns |  14.465 ns |  0.793 ns |  0.11 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **64**   |  **5,352.65 ns** | **133.554 ns** |  **7.321 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 64   |    346.20 ns |   6.682 ns |  0.366 ns |  0.06 |         - |          NA |
| Dp_Cjk     | 64   |  5,389.86 ns | 332.575 ns | 18.230 ns |  1.01 |         - |          NA |
| Kernel_Cjk | 64   |    435.24 ns |  17.098 ns |  0.937 ns |  0.08 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **96**   | **11,712.38 ns** | **161.923 ns** |  **8.876 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 96   |    715.89 ns |  41.111 ns |  2.253 ns |  0.06 |         - |          NA |
| Dp_Cjk     | 96   | 11,750.54 ns | 602.055 ns | 33.001 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 96   |  1,384.95 ns |  93.599 ns |  5.130 ns |  0.12 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **257.85 μs** |   **5.600 μs** |  **0.307 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   280.86 μs |   2.006 μs |  0.110 μs |  1.09 |      - |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    95.90 μs |  13.323 μs |  0.730 μs |  0.37 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   257.39 μs |  23.760 μs |  1.302 μs |  1.00 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **257.82 μs** |   **5.493 μs** |  **0.301 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   282.00 μs |  26.943 μs |  1.477 μs |  1.09 | 0.4883 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    95.92 μs |   5.662 μs |  0.310 μs |  0.37 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   258.31 μs |  22.692 μs |  1.244 μs |  1.00 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,837.10 μs** | **343.451 μs** | **18.826 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,787.53 μs | 369.608 μs | 20.259 μs |  0.98 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,258.43 μs | 239.008 μs | 13.101 μs |  0.44 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 2,820.18 μs | 454.457 μs | 24.910 μs |  0.99 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,878.64 μs** | **581.211 μs** | **31.858 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,877.29 μs | 205.362 μs | 11.257 μs |  1.00 |      - |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,256.61 μs |  79.150 μs |  4.338 μs |  0.44 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 2,808.62 μs | 342.918 μs | 18.796 μs |  0.98 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **8.104 μs** |  **1.0923 μs** | **0.0599 μs** |         **-** |
| EqualLength | 65           |     2.452 μs |  0.1078 μs | 0.0059 μs |         - |
| **Embedded**    | **128**          |    **30.570 μs** |  **3.3356 μs** | **0.1828 μs** |         **-** |
| EqualLength | 128          |     4.701 μs |  0.5919 μs | 0.0324 μs |         - |
| **Embedded**    | **512**          | **1,100.259 μs** | **54.6463 μs** | **2.9953 μs** |         **-** |
| EqualLength | 512          |    39.689 μs |  0.6586 μs | 0.0361 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.442 ms | 3.6622 ms | 0.2007 ms |  31.2500 |  23.4375 |  15.6250 |  3711.57 KB |
| TokenizerJsonWordPiece |  7.744 ms | 0.6465 ms | 0.0354 ms |  15.6250 |        - |        - |  5852.23 KB |
| TokenizerJsonUnigram   |  9.504 ms | 1.1189 ms | 0.0613 ms |        - |        - |        - |  4748.81 KB |
| SpieceModel            |  3.776 ms | 0.9340 ms | 0.0512 ms |  39.0625 |  35.1563 |  27.3438 |  4292.16 KB |
| TfidfSave              |  1.535 ms | 0.8176 ms | 0.0448 ms |  15.6250 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  3.456 ms | 5.2864 ms | 0.2898 ms |  15.6250 |   7.8125 |   7.8125 |  2921.36 KB |
| EmbeddingIndexSave     |  6.594 ms | 1.0359 ms | 0.0568 ms | 257.8125 | 257.8125 | 257.8125 |  20349.9 KB |
| EmbeddingIndexLoad     |  8.896 ms | 0.8089 ms | 0.0443 ms |  93.7500 |  93.7500 |  93.7500 | 16094.37 KB |
| EmbeddingIndexSaveFile | 89.583 ms | 7.6503 ms | 0.4193 ms |        - |        - |        - |   323.56 KB |
| EmbeddingIndexLoadGzip | 84.604 ms | 3.9096 ms | 0.2143 ms |        - |        - |        - | 16095.28 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **7,750.29 μs** |   **600.335 μs** |  **32.906 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,972.31 μs | 1,736.079 μs |  95.160 μs |  2.19 |    0.01 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **145.14 μs** |     **1.117 μs** |   **0.061 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    203.22 μs |     6.080 μs |   0.333 μs |  1.40 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,032.23 μs** |    **86.425 μs** |   **4.737 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,433.60 μs | 1,948.178 μs | 106.786 μs |  1.13 |    0.03 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.32 μs** |     **0.538 μs** |   **0.030 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.78 μs |     0.502 μs |   0.028 μs |  1.19 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error        | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|-----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **54.77 μs** |     **0.651 μs** |   **0.036 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    53.29 μs |     1.469 μs |   0.081 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   135.32 μs |     1.664 μs |   0.091 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   554.10 μs |   155.860 μs |   8.543 μs | 199.2188 | 199.2188 | 199.2188 |  800185 B |
| **Mse**      | **1000000** |   **582.76 μs** |    **72.005 μs** |   **3.947 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   577.13 μs |     3.632 μs |   0.199 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,387.84 μs |    52.752 μs |   2.892 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 7,652.45 μs | 2,009.334 μs | 110.138 μs | 296.8750 | 296.8750 | 296.8750 | 8000260 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean         | Error      | StdDev    | Ratio | Gen0      | Gen1     | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-------------:|-----------:|----------:|------:|----------:|---------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |    **60.050 ms** |  **3.8498 ms** | **0.2110 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |    10.754 ms |  2.0940 ms | 0.1148 ms |  0.18 |   78.1250 |  78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |     9.302 ms |  0.2873 ms | 0.0157 ms |  0.15 |         - |        - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |     7.431 ms |  0.3193 ms | 0.0175 ms |  0.12 |         - |        - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |    11.521 ms |  0.5030 ms | 0.0276 ms |  0.19 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |    **58.987 ms** |  **8.9708 ms** | **0.4917 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |    13.872 ms |  0.4967 ms | 0.0272 ms |  0.24 |  218.7500 | 218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |    11.142 ms |  0.1504 ms | 0.0082 ms |  0.19 |         - |        - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |     7.724 ms |  0.3507 ms | 0.0192 ms |  0.13 |         - |        - |        - |    430.79 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |    11.620 ms |  0.3642 ms | 0.0200 ms |  0.20 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **1,010.446 ms** |  **7.9878 ms** | **0.4378 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.81 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |    44.615 ms |  2.9324 ms | 0.1607 ms |  0.04 |  545.4545 | 545.4545 | 454.5455 |  10334.76 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |    38.084 ms |  3.0465 ms | 0.1670 ms |  0.04 |         - |        - |        - |   1218.82 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |    30.228 ms |  2.9464 ms | 0.1615 ms |  0.03 |         - |        - |        - |   1219.35 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |    46.293 ms |  0.7581 ms | 0.0416 ms |  0.05 |         - |        - |        - |   1718.79 KB |       0.014 |
|                      |           |              |              |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **1,009.939 ms** | **29.0723 ms** | **1.5936 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.48 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |    58.153 ms |  8.1167 ms | 0.4449 ms |  0.06 |  888.8889 | 888.8889 | 777.7778 |  18664.12 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |    44.710 ms |  2.7064 ms | 0.1483 ms |  0.04 |         - |        - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |    30.919 ms |  3.3085 ms | 0.1813 ms |  0.03 |         - |        - |        - |   1719.88 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |    46.315 ms |  4.0897 ms | 0.2242 ms |  0.05 |         - |        - |        - |   1718.79 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean            | Error           | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,235.9 ns |       107.41 ns |      5.89 ns |   0.0648 |        - |        - |     5464 B |
| TfidfSaveAsync         |      1,452.6 ns |        46.92 ns |      2.57 ns |   0.0668 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,270.6 ns |        20.69 ns |      1.13 ns |   0.0648 |        - |        - |     5496 B |
| IndexSaveAsync         |        383.1 ns |        13.13 ns |      0.72 ns |   0.0076 |        - |        - |      648 B |
| CountLoadSeekable1400K | 10,265,716.1 ns | 1,605,549.07 ns | 88,005.56 ns | 656.2500 | 640.6250 | 609.3750 | 12227071 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **2.181 ms** | **0.0538 ms** | **0.0030 ms** |  **1.00** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |  1.581 ms | 0.1227 ms | 0.0067 ms |  0.73 |   3.9063 |        - |        - |   407.6 KB |        0.47 |
| Hashing              | 200       |  2.276 ms | 0.2084 ms | 0.0114 ms |  1.04 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       |  1.791 ms | 0.0822 ms | 0.0045 ms |  0.82 |   1.9531 |        - |        - |  273.65 KB |        0.31 |
|                      |           |           |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **10.093 ms** | **0.4421 ms** | **0.0242 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.54 KB** |        **1.00** |
| CountWithStopWords   | 1000      |  7.968 ms | 0.3155 ms | 0.0173 ms |  0.79 | 375.0000 | 375.0000 | 375.0000 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 11.194 ms | 0.6084 ms | 0.0334 ms |  1.11 | 500.0000 | 500.0000 | 500.0000 | 2349.21 KB |        0.68 |
| HashingWithStopWords | 1000      |  9.214 ms | 0.3640 ms | 0.0200 ms |  0.91 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean          | Error         | StdDev     | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|-----------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **36,207.931 μs** | **1,522.3114 μs** | **83.4430 μs** |       **-** |       **-** |       **-** | **1554.61 KB** |
| Construct                  | [^ ]+         |      2.085 μs |     0.0483 μs |  0.0026 μs |  0.0648 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.312 μs |     0.0954 μs |  0.0052 μs |  0.0877 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **29,305.386 μs** |   **201.7181 μs** | **11.0569 μs** |       **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1.996 μs |     0.1467 μs |  0.0080 μs |  0.0648 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.733 μs |     0.1910 μs |  0.0105 μs |  0.0877 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **40,227.102 μs** |   **903.9799 μs** | **49.5502 μs** |       **-** |       **-** |       **-** |    **1556 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.372 μs |     0.2809 μs |  0.0154 μs |  0.0801 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.695 μs |     0.5650 μs |  0.0310 μs |  0.1068 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **21,973.441 μs** |   **690.7939 μs** | **37.8647 μs** | **62.5000** | **62.5000** | **62.5000** | **9743.61 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.078 μs |     0.1422 μs |  0.0078 μs |  0.0210 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1.833 μs |     0.0176 μs |  0.0010 μs |  0.0458 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **37,234.541 μs** |   **988.5189 μs** | **54.1841 μs** |       **-** |       **-** |       **-** | **1554.49 KB** |
| Construct                  | \\S+          |      2.335 μs |     0.1376 μs |  0.0075 μs |  0.0610 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.535 μs |     0.3645 μs |  0.0200 μs |  0.0877 |       - |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **17.59 ms** |  **2.084 ms** | **0.114 ms** |  **1.00** |    **0.01** |  **93.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  47.57 ms | 10.114 ms | 0.554 ms |  2.71 |    0.03 |        - |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **SentencePiece** |  **53.76 ms** |  **6.892 ms** | **0.378 ms** |  **1.00** |    **0.01** | **100.0000** |   **7.98 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  65.65 ms |  9.468 ms | 0.519 ms |  1.22 |    0.01 |        - |   5.65 MB |        0.71 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **85.38 ms** |  **4.560 ms** | **0.250 ms** |  **1.00** |    **0.00** | **333.3333** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 244.30 ms | 46.085 ms | 2.526 ms |  2.86 |    0.03 | 500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **41.82 ns** |  **1.420 ns** | **0.078 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  37.02 ns |  0.987 ns | 0.054 ns |  0.89 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **768**  |  **80.81 ns** |  **4.044 ns** | **0.222 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  69.36 ns | 10.786 ns | 0.591 ns |  0.86 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **1024** | **106.68 ns** |  **3.853 ns** | **0.211 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 |  89.52 ns |  3.414 ns | 0.187 ns |  0.84 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **255.4 μs** |     **9.04 μs** |   **0.50 μs** |  **1.00** |    **0.00** |    **2.4414** |    **0.4883** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    279.2 μs |    25.01 μs |   1.37 μs |  1.09 |    0.00 |    2.4414 |         - |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    768.6 μs |    27.57 μs |   1.51 μs |  3.01 |    0.01 |    6.8359 |    1.9531 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,139.4 μs |   145.31 μs |   7.96 μs |  8.38 |    0.03 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    282.7 μs |    25.98 μs |   1.42 μs |  1.11 |    0.01 |    1.4648 |         - |         - |   136.13 KB |        0.66 |
|                       |           |             |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,641.1 μs** |   **195.67 μs** |  **10.73 μs** |  **1.00** |    **0.01** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,790.4 μs |   287.60 μs |  15.76 μs |  1.09 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,938.0 μs |   326.50 μs |  17.90 μs |  2.40 |    0.02 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 13,294.8 μs | 3,189.15 μs | 174.81 μs |  8.10 |    0.10 | 1015.6250 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,708.8 μs |   208.56 μs |  11.43 μs |  1.04 |    0.01 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.956 ms** |   **0.6191 ms** |  **0.0339 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  43.466 ms |  63.0092 ms |  3.4537 ms | 10.99 |    0.76 |  7333.3333 |  7333.3333 |  7333.3333 |  28.26 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.803 ms** |   **1.8135 ms** |  **0.0994 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 334.508 ms | 188.7273 ms | 10.3448 ms | 18.79 |    0.51 | 70000.0000 | 70000.0000 | 70000.0000 |  324.4 MB |       40.00 |

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
| cdist_ratio_n50 | 0.144 | 0.027 | 0.19x | 0.144 | 0.027 | 0.19x |
| cdist_wratio_n50 | 2.159 | 2.366 | 1.10x | 2.159 | 2.366 | 1.10x |
| cdist_ratio_varied_n50 | 0.125 | 0.022 | 0.18x | 0.124 | 0.022 | 0.18x |
| cdist_ratio_varied_cutoff90_n50 | 0.048 | 0.022 | 0.46x | 0.048 | 0.022 | 0.46x |
| cdist_ratio_n200 | 2.602 | 0.328 | 0.13x | 2.605 | 0.328 | 0.13x |
| cdist_wratio_n200 | 31.503 | 34.759 | 1.10x | 31.503 | 34.759 | 1.10x |
| cdist_ratio_varied_n200 | 2.302 | 0.255 | 0.11x | 2.306 | 0.255 | 0.11x |
| cdist_ratio_varied_cutoff90_n200 | 0.887 | 0.255 | 0.29x | 0.888 | 0.255 | 0.29x |
| cdist_ratio_n500 | 15.738 | 1.972 | 0.13x | 15.758 | 1.972 | 0.13x |
| cdist_wratio_n500 | 185.643 | 205.644 | 1.11x | 185.710 | 205.637 | 1.11x |
| cdist_ratio_varied_n500 | 13.285 | 1.503 | 0.11x | 13.387 | 1.503 | 0.11x |
| cdist_ratio_varied_cutoff90_n500 | 5.014 | 1.501 | 0.30x | 5.087 | 1.501 | 0.29x |

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
| latin | 8 | 108.8 | 16.7 | 6.52x C# faster |
| latin | 32 | 155.7 | 89.6 | 1.74x C# faster |
| latin | 128 | 448.8 | 379.3 | 1.18x C# faster |
| latin | 512 | 4033.3 | 4685.7 | 1.16x Py faster |
| cjk | 8 | 123.1 | 16.7 | 7.37x C# faster |
| cjk | 32 | 297.1 | 247.5 | 1.20x C# faster |
| cjk | 128 | 1983.4 | 1305.9 | 1.52x C# faster |
| cjk | 512 | 17635.9 | 8315.5 | 2.12x C# faster |

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
| latin | 8 | 133.9 | 15.4 | 8.72x C# faster |
| latin | 32 | 229.8 | 160.9 | 1.43x C# faster |
| latin | 128 | 1557.7 | 843.4 | 1.85x C# faster |
| latin | 512 | 12527.0 | 11404.7 | 1.10x C# faster |
| cjk | 8 | 135.5 | 15.3 | 8.86x C# faster |
| cjk | 32 | 366.6 | 299.2 | 1.23x C# faster |
| cjk | 128 | 2964.8 | 2177.3 | 1.36x C# faster |
| cjk | 512 | 26279.6 | 18640.2 | 1.41x C# faster |

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
| confusion_matrix_n1000_k2 | 0.004 | 0.781 | 219.94x | 0.004 | 0.781 | 219.99x |
| accuracy_n1000_k2 | 0.000 | 0.396 | 2798.75x | 0.000 | 0.396 | 2798.85x |
| precision_recall_f1_macro_n1000_k2 | 0.004 | 1.430 | 377.69x | 0.004 | 1.429 | 377.69x |
| classification_report_n1000_k2 | 0.006 | 5.361 | 910.72x | 0.006 | 5.360 | 910.43x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.575 | 91.21x | 0.017 | 1.575 | 91.21x |
| balanced_accuracy_n1000_k2 | 0.004 | 0.831 | 230.31x | 0.004 | 0.831 | 230.33x |
| matthews_n1000_k2 | 0.004 | 1.595 | 443.63x | 0.004 | 1.595 | 443.56x |
| cohen_kappa_n1000_k2 | 0.004 | 0.880 | 244.14x | 0.004 | 0.880 | 244.15x |
| mse_n1000_k2 | 0.001 | 0.214 | 387.83x | 0.001 | 0.213 | 387.82x |
| mae_n1000_k2 | 0.001 | 0.214 | 392.80x | 0.001 | 0.214 | 392.81x |
| median_ae_n1000_k2 | 0.006 | 0.229 | 41.14x | 0.006 | 0.229 | 41.14x |
| r2_n1000_k2 | 0.001 | 0.257 | 182.88x | 0.001 | 0.257 | 182.85x |
| confusion_matrix_n1000_k10 | 0.004 | 0.773 | 213.51x | 0.004 | 0.773 | 213.49x |
| accuracy_n1000_k10 | 0.000 | 0.394 | 2778.03x | 0.000 | 0.394 | 2777.94x |
| precision_recall_f1_macro_n1000_k10 | 0.004 | 1.430 | 343.17x | 0.004 | 1.430 | 343.17x |
| classification_report_n1000_k10 | 0.010 | 5.352 | 557.62x | 0.010 | 5.352 | 557.63x |
| roc_auc_ovr_macro_n1000_k10 | 0.531 | 8.098 | 15.26x | 0.531 | 8.098 | 15.26x |
| balanced_accuracy_n1000_k10 | 0.004 | 0.825 | 208.27x | 0.004 | 0.825 | 208.27x |
| matthews_n1000_k10 | 0.004 | 1.580 | 402.47x | 0.004 | 1.580 | 402.47x |
| cohen_kappa_n1000_k10 | 0.004 | 0.871 | 206.47x | 0.004 | 0.871 | 206.47x |
| mse_n1000_k10 | 0.001 | 0.210 | 381.43x | 0.001 | 0.210 | 381.43x |
| mae_n1000_k10 | 0.001 | 0.209 | 383.94x | 0.001 | 0.209 | 383.95x |
| median_ae_n1000_k10 | 0.006 | 0.222 | 39.79x | 0.006 | 0.222 | 39.79x |
| r2_n1000_k10 | 0.001 | 0.252 | 178.11x | 0.001 | 0.252 | 178.11x |
| confusion_matrix_n100000_k2 | 0.353 | 12.070 | 34.22x | 0.353 | 12.070 | 34.22x |
| accuracy_n100000_k2 | 0.016 | 4.172 | 257.96x | 0.016 | 4.172 | 257.94x |
| precision_recall_f1_macro_n100000_k2 | 0.353 | 13.713 | 38.88x | 0.353 | 13.713 | 38.88x |
| classification_report_n100000_k2 | 0.356 | 29.031 | 81.60x | 0.356 | 29.030 | 81.60x |
| roc_auc_binary_n100000_k2 | 4.950 | 28.140 | 5.69x | 4.949 | 28.140 | 5.69x |
| balanced_accuracy_n100000_k2 | 0.353 | 12.172 | 34.48x | 0.353 | 12.171 | 34.48x |
| matthews_n100000_k2 | 0.353 | 24.445 | 69.22x | 0.353 | 24.443 | 69.22x |
| cohen_kappa_n100000_k2 | 0.354 | 12.203 | 34.50x | 0.354 | 12.203 | 34.50x |
| mse_n100000_k2 | 0.058 | 0.439 | 7.55x | 0.058 | 0.439 | 7.55x |
| mae_n100000_k2 | 0.057 | 0.468 | 8.15x | 0.057 | 0.468 | 8.15x |
| median_ae_n100000_k2 | 0.661 | 1.810 | 2.74x | 0.677 | 1.810 | 2.67x |
| r2_n100000_k2 | 0.137 | 0.663 | 4.83x | 0.137 | 0.663 | 4.83x |
| confusion_matrix_n100000_k10 | 0.334 | 12.060 | 36.06x | 0.334 | 12.058 | 36.06x |
| accuracy_n100000_k10 | 0.016 | 4.159 | 257.92x | 0.016 | 4.159 | 257.90x |
| precision_recall_f1_macro_n100000_k10 | 0.337 | 14.302 | 42.47x | 0.337 | 14.302 | 42.47x |
| classification_report_n100000_k10 | 0.342 | 31.392 | 91.84x | 0.342 | 31.389 | 91.83x |
| roc_auc_ovr_macro_n100000_k10 | 41.443 | 231.260 | 5.58x | 41.440 | 231.248 | 5.58x |
| balanced_accuracy_n100000_k10 | 0.335 | 12.145 | 36.27x | 0.335 | 12.145 | 36.27x |
| matthews_n100000_k10 | 0.334 | 25.185 | 75.31x | 0.334 | 25.183 | 75.30x |
| cohen_kappa_n100000_k10 | 0.335 | 12.266 | 36.61x | 0.335 | 12.265 | 36.61x |
| mse_n100000_k10 | 0.053 | 0.429 | 8.06x | 0.053 | 0.429 | 8.06x |
| mae_n100000_k10 | 0.053 | 0.468 | 8.89x | 0.053 | 0.468 | 8.89x |
| median_ae_n100000_k10 | 0.702 | 1.805 | 2.57x | 0.764 | 1.805 | 2.36x |
| r2_n100000_k10 | 0.137 | 0.659 | 4.82x | 0.137 | 0.659 | 4.82x |
| confusion_matrix_n1000000_k2 | 3.748 | 119.406 | 31.86x | 3.748 | 119.399 | 31.86x |
| accuracy_n1000000_k2 | 0.260 | 39.523 | 152.19x | 0.260 | 39.513 | 152.15x |
| precision_recall_f1_macro_n1000000_k2 | 3.753 | 129.122 | 34.40x | 3.753 | 129.109 | 34.40x |
| classification_report_n1000000_k2 | 3.732 | 254.933 | 68.30x | 3.732 | 254.928 | 68.31x |
| roc_auc_binary_n1000000_k2 | 81.009 | 320.673 | 3.96x | 81.008 | 320.632 | 3.96x |
| balanced_accuracy_n1000000_k2 | 3.720 | 118.065 | 31.74x | 3.720 | 118.059 | 31.74x |
| matthews_n1000000_k2 | 3.741 | 238.891 | 63.86x | 3.741 | 238.874 | 63.86x |
| cohen_kappa_n1000000_k2 | 3.717 | 118.773 | 31.95x | 3.717 | 118.767 | 31.95x |
| mse_n1000000_k2 | 0.584 | 2.297 | 3.93x | 0.584 | 2.297 | 3.93x |
| mae_n1000000_k2 | 0.577 | 2.588 | 4.48x | 0.577 | 2.588 | 4.48x |
| median_ae_n1000000_k2 | 7.189 | 15.796 | 2.20x | 7.296 | 15.794 | 2.16x |
| r2_n1000000_k2 | 1.388 | 4.451 | 3.21x | 1.388 | 4.450 | 3.21x |
| confusion_matrix_n1000000_k10 | 3.481 | 117.770 | 33.83x | 3.481 | 117.739 | 33.83x |
| accuracy_n1000000_k10 | 0.257 | 38.683 | 150.65x | 0.257 | 38.682 | 150.65x |
| precision_recall_f1_macro_n1000000_k10 | 3.475 | 133.185 | 38.33x | 3.475 | 133.178 | 38.33x |
| classification_report_n1000000_k10 | 3.482 | 273.075 | 78.43x | 3.482 | 272.916 | 78.39x |
| balanced_accuracy_n1000000_k10 | 3.508 | 117.808 | 33.59x | 3.507 | 117.801 | 33.59x |
| matthews_n1000000_k10 | 3.491 | 244.064 | 69.91x | 3.491 | 244.050 | 69.91x |
| cohen_kappa_n1000000_k10 | 3.492 | 117.946 | 33.77x | 3.492 | 117.918 | 33.77x |
| mse_n1000000_k10 | 0.585 | 2.313 | 3.95x | 0.585 | 2.312 | 3.95x |
| mae_n1000000_k10 | 0.579 | 2.593 | 4.48x | 0.579 | 2.593 | 4.48x |
| median_ae_n1000000_k10 | 6.905 | 15.630 | 2.26x | 7.025 | 15.630 | 2.22x |
| r2_n1000000_k10 | 1.410 | 4.552 | 3.23x | 1.410 | 4.550 | 3.23x |

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
| vocab_txt | 3.262 | 9.607 | 2.94x | 3.433 | 9.606 | 2.80x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 8.968 | 16.952 | 1.89x | 9.181 | 16.951 | 1.85x | 706,526 | 706,526 |
| tokenizer_json_unigram | 10.561 | 49.955 | 4.73x | 10.832 | 49.950 | 4.61x | 1,990,038 | 1,990,038 |
| spiece_model | 3.811 | 15.870 | 4.16x | 3.924 | 15.870 | 4.04x | 347,061 | 347,061 |
| tfidf_save | 1.593 | 2.521 | 1.58x | 1.611 | 2.521 | 1.57x | 581,787 | 591,922 |
| tfidf_load | 3.588 | 4.002 | 1.12x | 3.811 | 4.002 | 1.05x | 581,787 | 591,922 |
| embedding_index_save | 6.020 | 2.235 | 0.37x | 6.753 | 2.235 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 89.005 | 66.014 | 0.74x | 10.590 | 5.814 | 0.55x | 20,589,007 | 15,360,128 |
| embedding_index_load | 8.494 | 1.494 | 0.18x | 8.996 | 1.493 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 8.834 | 1.167 | 0.13x | 9.395 | 1.165 | 0.12x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 7.191 | 1.496 | 0.21x | 7.594 | 1.493 | 0.20x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 4.302 | 1.501 | 0.35x | 4.373 | 1.499 | 0.34x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 63.33x | 0.000 | 0.001 | 63.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 438.450 | 605.306 | 1.38x | 438.428 | 605.257 | 1.38x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 82.153 | 93.565 | 1.14x | 82.152 | 93.549 | 1.14x | 15,250,490 | 14,022,374 |

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
| kfold_n10000 | 0.100 | 0.100 | 1.01x | 0.100 | 0.100 | 1.01x |
| stratified_n10000 | 0.154 | 0.779 | 5.05x | 0.154 | 0.779 | 5.06x |
| traintest_n10000 | 0.007 | 0.184 | 27.63x | 0.007 | 0.184 | 27.63x |
| kfold_seeded_n10000 | 0.348 | 0.708 | 2.03x | 0.348 | 0.708 | 2.03x |
| stratified_seeded_n10000 | 0.418 | 1.495 | 3.57x | 0.418 | 1.495 | 3.57x |
| traintest_seeded_n10000 | 0.168 | 0.444 | 2.64x | 0.168 | 0.444 | 2.64x |
| stratified_traintest_n10000 | 0.747 | 2.490 | 3.33x | 0.747 | 2.490 | 3.33x |
| group_kfold_n10000 | 0.136 | 0.498 | 3.67x | 0.136 | 0.498 | 3.67x |
| stratified_group_kfold_n10000 | 0.239 | 21.907 | 91.56x | 0.239 | 21.905 | 91.56x |
| timeseries_n10000 | 0.000 | 0.011 | 75.43x | 0.000 | 0.011 | 75.43x |
| repeated_kfold_n10000 | 1.089 | 1.943 | 1.78x | 1.089 | 1.943 | 1.78x |
| kfold_n100000 | 1.121 | 2.133 | 1.90x | 1.224 | 2.133 | 1.74x |
| stratified_n100000 | 1.755 | 7.378 | 4.20x | 1.866 | 7.375 | 3.95x |
| traintest_n100000 | 0.157 | 0.459 | 2.92x | 0.158 | 0.459 | 2.90x |
| kfold_seeded_n100000 | 4.250 | 7.293 | 1.72x | 4.380 | 7.290 | 1.66x |
| stratified_seeded_n100000 | 4.495 | 13.069 | 2.91x | 4.607 | 13.067 | 2.84x |
| traintest_seeded_n100000 | 2.305 | 1.924 | 0.83x | 2.336 | 1.924 | 0.82x |
| stratified_traintest_n100000 | 9.050 | 21.432 | 2.37x | 9.122 | 21.432 | 2.35x |
| group_kfold_n100000 | 1.613 | 6.313 | 3.91x | 1.715 | 6.313 | 3.68x |
| stratified_group_kfold_n100000 | 2.487 | 227.547 | 91.48x | 2.598 | 227.527 | 87.59x |
| timeseries_n100000 | 0.000 | 0.042 | 289.12x | 0.000 | 0.042 | 289.18x |
| repeated_kfold_n100000 | 11.541 | 21.451 | 1.86x | 11.669 | 21.448 | 1.84x |
| kfold_n1000000 | 9.079 | 20.785 | 2.29x | 9.611 | 20.776 | 2.16x |
| stratified_n1000000 | 17.394 | 78.162 | 4.49x | 18.207 | 78.158 | 4.29x |
| traintest_n1000000 | 0.571 | 2.901 | 5.08x | 0.621 | 2.899 | 4.67x |
| kfold_seeded_n1000000 | 45.514 | 71.262 | 1.57x | 46.423 | 71.261 | 1.54x |
| stratified_seeded_n1000000 | 53.309 | 127.850 | 2.40x | 54.163 | 127.839 | 2.36x |
| traintest_seeded_n1000000 | 24.668 | 18.084 | 0.73x | 24.984 | 18.082 | 0.72x |
| stratified_traintest_n1000000 | 106.104 | 237.766 | 2.24x | 106.494 | 237.451 | 2.23x |
| group_kfold_n1000000 | 16.782 | 57.194 | 3.41x | 17.494 | 57.192 | 3.27x |
| stratified_group_kfold_n1000000 | 27.807 | 2320.740 | 83.46x | 28.882 | 2320.654 | 80.35x |
| timeseries_n1000000 | 0.000 | 0.359 | 2699.72x | 0.000 | 0.358 | 2697.62x |
| repeated_kfold_n1000000 | 129.032 | 212.118 | 1.64x | 129.740 | 212.112 | 1.63x |

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
| welch_t_n1000 | 0.007 | 0.574 | 83.35x | 0.007 | 0.574 | 83.29x |
| mann_whitney_n1000 | 0.050 | 0.610 | 12.20x | 0.050 | 0.610 | 12.20x |
| chi_square_n1000 | 0.000 | 0.225 | 1264.30x | 0.000 | 0.225 | 1264.36x |
| fligner_n1000 | 0.123 | 0.683 | 5.54x | 0.123 | 0.683 | 5.54x |
| anderson_ksamp_n1000 | 0.206 | 0.376 | 1.83x | 0.206 | 0.376 | 1.83x |
| spearman_matrix_n1000 | 0.191 | 0.701 | 3.67x | 0.191 | 0.701 | 3.67x |
| pointbiserial_n1000 | 0.011 | 0.420 | 39.47x | 0.011 | 0.420 | 39.47x |
| welch_t_n10000 | 0.048 | 0.621 | 13.05x | 0.048 | 0.621 | 13.05x |
| mann_whitney_n10000 | 0.600 | 2.511 | 4.19x | 0.600 | 2.511 | 4.19x |
| chi_square_n10000 | 0.001 | 0.224 | 199.68x | 0.001 | 0.224 | 199.69x |
| fligner_n10000 | 1.325 | 2.987 | 2.25x | 1.365 | 2.987 | 2.19x |
| anderson_ksamp_n10000 | 2.195 | 3.606 | 1.64x | 2.279 | 3.606 | 1.58x |
| spearman_matrix_n10000 | 2.100 | 4.319 | 2.06x | 2.142 | 4.318 | 2.02x |
| pointbiserial_n10000 | 0.126 | 0.502 | 3.98x | 0.126 | 0.502 | 3.98x |
| welch_t_n100000 | 0.491 | 1.239 | 2.52x | 0.491 | 1.239 | 2.52x |
| mann_whitney_n100000 | 6.310 | 26.367 | 4.18x | 6.310 | 26.366 | 4.18x |
| chi_square_n100000 | 0.012 | 0.242 | 20.81x | 0.012 | 0.242 | 20.81x |
| fligner_n100000 | 17.872 | 31.170 | 1.74x | 18.162 | 31.169 | 1.72x |
| anderson_ksamp_n100000 | 19.643 | 41.436 | 2.11x | 19.973 | 41.432 | 2.07x |
| spearman_matrix_n100000 | 21.034 | 50.339 | 2.39x | 21.441 | 50.335 | 2.35x |
| pointbiserial_n100000 | 1.423 | 1.733 | 1.22x | 1.435 | 6.874 | 4.79x |

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
| normalize_n10000 | 0.219 | 0.365 | 1.66x | 0.221 | 0.365 | 1.65x |
| polynomial_n10000 | 0.611 | 0.875 | 1.43x | 0.614 | 0.875 | 1.43x |
| kbins_fit_n10000 | 1.480 | 2.107 | 1.42x | 1.480 | 2.106 | 1.42x |
| kbins_n10000 | 0.463 | 3.788 | 8.18x | 0.544 | 3.788 | 6.96x |
| quantile_fit_n10000 | 1.593 | 3.755 | 2.36x | 1.593 | 3.754 | 2.36x |
| quantile_n10000 | 1.977 | 1.774 | 0.90x | 1.980 | 1.774 | 0.90x |
| power_fit_n10000 | 5.681 | 73.591 | 12.95x | 5.685 | 73.580 | 12.94x |
| power_n10000 | 1.204 | 0.847 | 0.70x | 1.207 | 0.847 | 0.70x |
| knn_impute_n10000 | 6.063 | 25.240 | 4.16x | 6.063 | 100.223 | 16.53x |
| label_n10000 | 1.264 | 0.890 | 0.70x | 1.264 | 0.890 | 0.70x |
| normalize_n100000 | 1.662 | 2.051 | 1.23x | 2.075 | 2.051 | 0.99x |
| polynomial_n100000 | 4.513 | 6.973 | 1.55x | 5.457 | 6.972 | 1.28x |
| kbins_fit_n100000 | 15.634 | 8.700 | 0.56x | 16.481 | 8.700 | 0.53x |
| kbins_n100000 | 5.805 | 14.421 | 2.48x | 6.638 | 14.420 | 2.17x |
| quantile_fit_n100000 | 15.824 | 18.846 | 1.19x | 16.611 | 18.844 | 1.13x |
| quantile_n100000 | 19.257 | 18.156 | 0.94x | 19.876 | 18.155 | 0.91x |
| power_fit_n100000 | 62.253 | 509.404 | 8.18x | 65.423 | 509.318 | 7.79x |
| power_n100000 | 11.502 | 6.499 | 0.57x | 12.020 | 6.499 | 0.54x |
| knn_impute_n100000 | 6.073 | 20.475 | 3.37x | 6.072 | 81.736 | 13.46x |
| label_n100000 | 15.670 | 8.788 | 0.56x | 15.954 | 8.788 | 0.55x |

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
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `SurvivalBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorStoreChurnBenchmarks`
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

**New tonight: 15.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `LcsGateBenchmarks` | Band=20 | `Kernel` | step | 0.16 | 0.1 | +60% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `Lodestar_HeldPattern` | step | 0.24 | 0.45 | -47% | 30% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | step | 0.2 | 0.14 | +43% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `FuzzySharp_PerText` | step | 1.3 | 2.26 | -42% | 30% |
| `LcsGateBenchmarks` | Band=20 | `Kernel_Cjk` | step | 0.24 | 0.17 | +41% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `Lodestar_HeldPattern` | step | 0.35 | 0.58 | -40% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=16 | `Lodestar_HeldPattern` | step | 0.29 | 0.47 | -38% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `FuzzySharp_PerText` | step | 1.19 | 1.92 | -38% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=50000x8x32 | `MetaNumerics_Fit` | step | 17.52 | 27.61 | -37% | 30% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `Lodestar_HeldPattern` | step | 0.7 | 1.07 | -35% | 30% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `FuzzySharp_PerText` | step | 1.18 | 1.8 | -34% | 30% |
| `FuzzCodePointBenchmarks` | — | `Ratio_DistinctAstral` | step | 46.643 | 34.935 | +34% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_Binomial` | step | 0.002 | 0.003 | -33% | 30% |
| `IndelPatternBenchmarks` | Corpus=oneWordAffix, Length=32 | `FuzzySharp_PerText` | step | 1.18 | 1.76 | -33% | 30% |
| `PrincipalComponentVarianceBenchmarks` | Shape=100x200 | `NumFlat_Pca` | step | 2.19 | 1.66 | +32% | 30% |

**Still away from their median: 31.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 0.6 | 2.15 | -78% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_ChiSquare` | drift | 0.19 | 0.67 | -78% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | drift | 0.39 | 1.51 | -77% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 0.83 | 3.06 | -77% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | drift | 0.09 | 0.44 | -77% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | drift | 1.13 | 3.69 | -77% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 2.47 | 8.92 | -76% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | drift | 0.37 | 0.97 | -76% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | drift | 1.11 | 3.71 | -76% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_Wilcoxon` | drift | 0.35 | 1.05 | -76% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 0.74 | 2.14 | -76% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | drift | 2.11 | 6.71 | -75% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 0.79 | 2.23 | -74% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | drift | 0.03 | 0.11 | -73% | 20% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x50 | `MetaNumerics_VarianceFraction` | step | 346.94 | 203.36 | +71% | 35% |
| `IndelBenchmarks` | Length=32 | `Distance_CodePoint` | drift | 1.05 | 1.11 | -66% | 20% |
| `IndelBenchmarks` | Length=32 | `NormalizedSimilarity_Utf16` | drift | 1.04 | 1.05 | -66% | 20% |
| `IndelBenchmarks` | Length=32 | `SubsequenceLength_Utf16` | drift | 0.93 | 0.99 | -66% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.37 | 0.83 | -57% | 20% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x10 | `MetaNumerics_VarianceFraction` | step | 1500.53 | 992.93 | +51% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=AccuracyAlone | `MlNet` | step | 852.02 | 1647.9 | -48% | 41% |
| `LcsGateBenchmarks` | Band=96 | `Kernel` | drift | 0.03 | 0.03 | -33% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Bartlett` | step | 0.15 | 0.22 | -32% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Binomial` | drift | 1.32 | 2.32 | -31% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_AndersonDarling` | drift | 2.06 | 3.1 | -30% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Levene` | drift | 1.41 | 1.98 | -27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_AndersonDarling` | drift | 3.17 | 4.25 | -25% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_ClopperPearson` | drift | 1.63 | 2.02 | -25% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Binomial` | drift | 0.27 | 0.37 | -24% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Bartlett` | drift | 0.29 | 0.52 | -21% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `SignaturesOnly` | drift | 0.04 | 0.05 | -20% | 20% |
