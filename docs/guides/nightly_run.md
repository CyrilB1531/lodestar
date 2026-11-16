# Nightly benchmark run

<!-- nightly-baseline: c944fbc8ff204937b33c044d6d0924ec8795f37b -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BkTreeNearestBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks DecompositionWidthBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchHelpersBenchmarks FilteredVectorSearchBenchmarks GlmNegativeBinomialBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks LeastSquaresRoutingBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks RankingMetricsBenchmarks SentencePieceBpeLineageBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks VectorStoreChurnBenchmarks -->
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

- Commit: `c944fbc8ff204937b33c044d6d0924ec8795f37b`
- Previous run: `c944fbc8ff204937b33c044d6d0924ec8795f37b`
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

| Method                         | PairCount | Mean             | Error            | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **907.2 ns** |         **22.46 ns** |         **1.23 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         697.8 ns |         24.20 ns |         1.33 ns |     0.77 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,680.8 ns |        472.66 ns |        25.91 ns |     2.96 |    0.02 |   0.0458 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,712.9 ns |        377.45 ns |        20.69 ns |     4.09 |    0.02 |   0.0229 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       4,852.0 ns |        975.74 ns |        53.48 ns |     5.35 |    0.05 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,539.7 ns |        327.00 ns |        17.92 ns |    11.62 |    0.02 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       4,753.8 ns |        728.21 ns |        39.92 ns |     5.24 |    0.04 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **80,682.3 ns** |        **389.88 ns** |        **21.37 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      50,422.7 ns |      1,921.45 ns |       105.32 ns |     0.62 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,102,732.7 ns |    159,315.95 ns |     8,732.64 ns |    13.67 |    0.09 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,969,945.2 ns |    425,254.70 ns |    23,309.65 ns |    24.42 |    0.25 |        - |        - |        - |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,492,603.3 ns |    142,583.09 ns |     7,815.46 ns |    30.89 |    0.08 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 296,973,533.3 ns | 37,520,470.37 ns | 2,056,623.60 ns | 3,680.78 |   22.09 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,478,421.3 ns |     23,878.45 ns |     1,308.86 ns |    30.72 |    0.02 |        - |        - |        - |         - |          NA |

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

| Method            | SampleSize | Mean        | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **63.05 μs** | **18.59 μs** | **1.019 μs** |  **1.00** |    **0.02** | **0.1221** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    57.05 μs | 20.09 μs | 1.101 μs |  0.91 |    0.02 | 0.1831 |  15.67 KB |        1.00 |
|                   |            |             |          |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,746.87 μs** | **69.66 μs** | **3.819 μs** |  **1.00** |    **0.00** |      **-** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,437.77 μs | 49.68 μs | 2.723 μs |  0.82 |    0.00 |      - |  156.3 KB |        1.00 |

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

| Method                         | SampleSize | Mean            | Error            | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|-----------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **3,266.75 ns** |        **12.403 ns** |      **0.680 ns** |   **1.00** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     1,975.65 ns |        53.381 ns |      2.926 ns |   0.60 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,395.29 ns |        96.240 ns |      5.275 ns |   0.73 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,711.62 ns |       652.635 ns |     35.773 ns |   1.14 |    0.01 | 0.0114 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     6,846.13 ns |       165.671 ns |      9.081 ns |   2.10 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     8,207.65 ns |     3,166.471 ns |    173.565 ns |   2.51 |    0.05 | 0.0153 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,584.52 ns |        81.691 ns |      4.478 ns |   0.79 |    0.00 | 0.0191 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,650.49 ns |       405.653 ns |     22.235 ns |   1.12 |    0.01 | 0.0114 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,228.15 ns |        30.960 ns |      1.697 ns |   0.38 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,709.65 ns |        34.318 ns |      1.881 ns |   0.83 |    0.00 | 0.0076 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,170.23 ns |     1,113.817 ns |     61.052 ns |   0.36 |    0.02 | 0.0095 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,209.28 ns |       229.711 ns |     12.591 ns |   0.37 |    0.00 | 0.0114 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       297.59 ns |         2.045 ns |      0.112 ns |   0.09 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,276.77 ns |        67.700 ns |      3.711 ns |   0.39 |    0.00 | 0.0095 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |        99.28 ns |         2.366 ns |      0.130 ns |   0.03 |    0.00 | 0.0019 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       632.88 ns |        46.936 ns |      2.573 ns |   0.19 |    0.00 | 0.0114 |     968 B |       15.12 |
|                                |            |                 |                  |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **54,638.86 ns** |     **2,516.975 ns** |    **137.964 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   163,274.25 ns |     3,583.899 ns |    196.446 ns |  2.988 |    0.01 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   500,580.03 ns |    45,772.008 ns |  2,508.918 ns |  9.162 |    0.04 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,133,113.21 ns | 1,197,271.754 ns | 65,626.505 ns | 39.040 |    1.04 |      - |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,173,165.26 ns |    80,005.623 ns |  4,385.378 ns | 21.471 |    0.08 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,592,182.36 ns |    93,434.161 ns |  5,121.442 ns | 65.744 |    0.16 |      - |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,467,125.24 ns |    67,665.930 ns |  3,708.998 ns | 26.851 |    0.08 |      - |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,126,317.39 ns |   127,224.481 ns |  6,973.603 ns | 38.916 |    0.14 |      - |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    60,792.66 ns |     6,440.204 ns |    353.009 ns |  1.113 |    0.01 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   244,937.75 ns |     5,355.027 ns |    293.527 ns |  4.483 |    0.01 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   250,164.52 ns |    20,710.374 ns |  1,135.205 ns |  4.579 |    0.02 | 0.4883 |   80057 B |    1,250.89 |
| MetaNumerics_Wilcoxon          | 10000      |   445,076.20 ns |     6,374.769 ns |    349.423 ns |  8.146 |    0.02 | 0.4883 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       299.42 ns |        31.385 ns |      1.720 ns |  0.005 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,278.65 ns |       121.670 ns |      6.669 ns |  0.023 |    0.00 | 0.0095 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |        99.78 ns |         5.907 ns |      0.324 ns |  0.002 |    0.00 | 0.0019 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       637.11 ns |        15.396 ns |      0.844 ns |  0.012 |    0.00 | 0.0114 |     968 B |       15.12 |

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
| **Lodestar_Levene**          | **100**       |     **2,683.8 ns** |    **184.40 ns** |    **10.11 ns** |  **1.00** |    **0.00** | **0.0572** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,580.0 ns |     59.45 ns |     3.26 ns |  1.33 |    0.00 | 0.0648 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       387.3 ns |      6.10 ns |     0.33 ns |  0.14 |    0.00 | 0.0010 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |       753.7 ns |     44.39 ns |     2.43 ns |  0.28 |    0.00 | 0.0019 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       694.4 ns |      4.78 ns |     0.26 ns |  0.26 |    0.00 |      - |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     3,465.4 ns |    418.42 ns |    22.93 ns |  1.29 |    0.01 | 0.0229 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,273.5 ns |    442.23 ns |    24.24 ns |  3.08 |    0.01 |      - |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     5,353.8 ns |    556.38 ns |    30.50 ns |  1.99 |    0.01 | 0.0076 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,238.4 ns |    104.59 ns |     5.73 ns |  1.58 |    0.01 |      - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |        |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **362,884.7 ns** | **19,220.56 ns** | **1,053.54 ns** | **1.000** |    **0.00** | **5.3711** | **1.4648** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,249,953.7 ns | 42,255.77 ns | 2,316.18 ns | 3.445 |    0.01 | 3.9063 |      - |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    39,281.1 ns |    861.50 ns |    47.22 ns | 0.108 |    0.00 |      - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |    80,287.4 ns | 61,943.88 ns | 3,395.35 ns | 0.221 |    0.01 |      - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       903.8 ns |    161.12 ns |     8.83 ns | 0.002 |    0.00 |      - |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   687,035.1 ns | 67,032.43 ns | 3,674.27 ns | 1.893 |    0.01 | 1.9531 |      - |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,350,978.8 ns | 71,173.50 ns | 3,901.26 ns | 3.723 |    0.01 |      - |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |     NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,680.0 ns |    588.56 ns |    32.26 ns | 0.029 |    0.00 |      - |      - |      48 B |       0.000 |

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,780.3 μs** |     **382.72 μs** |    **20.98 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    97,681.9 μs |   7,141.80 μs |   391.47 μs |  54.87 |    0.59 |   666.6667 |  500.0000 |         - |  59878.93 KB |       58.56 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,633.4 μs** |     **139.82 μs** |     **7.66 μs** |   **1.00** |    **0.01** |   **248.0469** |  **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    95,935.9 μs |  24,855.35 μs | 1,362.41 μs |  58.74 |    0.76 |  1000.0000 |  800.0000 |         - |  83110.15 KB |       81.29 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **777.2 μs** |      **16.03 μs** |     **0.88 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   123,285.7 μs |  61,018.22 μs | 3,344.61 μs | 158.62 |    3.73 |   750.0000 |  250.0000 |         - |  81288.43 KB |    2,678.83 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,096.3 μs** |   **1,410.10 μs** |    **77.29 μs** |   **1.00** |    **0.04** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    84,121.8 μs |   6,732.31 μs |   369.02 μs |  40.16 |    1.27 |   750.0000 |  500.0000 |         - |  64189.79 KB |       62.78 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,256.2 μs** |   **3,174.79 μs** |   **174.02 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,707,270.3 μs |  31,718.43 μs | 1,738.59 μs | 111.92 |    1.11 |  6000.0000 | 3000.0000 | 1000.0000 | 537131.64 KB |       60.18 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **15,681.0 μs** |   **2,439.24 μs** |   **133.70 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,842,385.0 μs |  47,112.26 μs | 2,582.38 μs | 117.50 |    0.88 | 10000.0000 | 5000.0000 | 2000.0000 | 747334.48 KB |       83.73 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,346.4 μs** |   **1,080.06 μs** |    **59.20 μs** |   **1.00** |    **0.01** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 2,853,271.0 μs | 157,622.28 μs | 8,639.81 μs | 449.62 |    3.81 |  9000.0000 | 5000.0000 | 2000.0000 | 732662.05 KB |    8,147.76 |
|                        |      |          |                |               |             |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,797.3 μs** |   **5,907.43 μs** |   **323.81 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,520,506.1 μs | 106,535.98 μs | 5,839.60 μs |  85.45 |    1.36 |  8000.0000 | 5000.0000 | 2000.0000 | 573606.88 KB |       64.27 |

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
| CountLoadAsync2M | 18.01 ms |  4.410 ms | 0.242 ms | 437.5000 | 406.2500 | 375.0000 |  21.53 MB |
| CountLoadAsync4M | 43.89 ms |  9.509 ms | 0.521 ms | 500.0000 | 416.6667 | 333.3333 |  43.93 MB |
| CountLoad4M      | 43.24 ms | 11.069 ms | 0.607 ms | 500.0000 | 416.6667 | 333.3333 |  43.92 MB |

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
| CountFitLongDocuments | 179,014.82 μs | 15,870.130 μs |   869.895 μs |        - |        - |        - | 54334.42 KB |
| CountSave240K         |     350.40 μs |     25.800 μs |     1.414 μs |   3.9063 |   2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     467.46 μs |     52.173 μs |     2.860 μs |   6.8359 |   5.8594 |   5.8594 |   739.44 KB |
| TfidfSave45K          |      51.78 μs |      7.624 μs |     0.418 μs |   1.6479 |   0.3052 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      57.42 μs |     18.804 μs |     1.031 μs |   2.1973 |   0.4883 |        - |   181.88 KB |
| IndexSave942K         |     225.53 μs |     18.595 μs |     1.019 μs |   1.4648 |   0.4883 |   0.4883 |    243.5 KB |
| IndexSaveAsync942K    |     231.40 μs |     29.564 μs |     1.620 μs |   2.1973 |   0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  21,979.77 μs | 30,295.141 μs | 1,660.579 μs | 187.5000 | 156.2500 | 125.0000 | 22048.84 KB |
| CountLoadAsync4M      |  41,437.17 μs |  8,209.397 μs |   449.985 μs | 272.7273 | 181.8182 | 181.8182 | 44984.64 KB |
| CountLoad4M           |  43,696.56 μs | 77,532.666 μs | 4,249.827 μs | 272.7273 | 181.8182 | 181.8182 | 44976.66 KB |

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
| TfidfSaveAsyncSmall    |     17.449 μs |    31.694 μs |   1.7372 μs |   0.3357 |   0.0153 |        - |    27.43 KB |
| CountSaveAsyncWide     |    451.750 μs |    97.715 μs |   5.3561 μs |  28.3203 |  27.3438 |  27.3438 |   740.16 KB |
| HashingLoadUndeclared  |      8.825 μs |     6.215 μs |   0.3407 μs |   1.0223 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     29.272 μs |     2.704 μs |   0.1482 μs |   1.4648 |   0.2747 |        - |   121.83 KB |
| CountLoadUndeclared24K |    118.871 μs |    11.372 μs |   0.6233 μs |   3.1738 |        - |        - |   264.67 KB |
| CountLoadUndeclared78K |    640.525 μs |   152.538 μs |   8.3611 μs |  12.6953 |  11.7188 |   6.8359 |   656.33 KB |
| CountLoadUndeclared1M  |  8,199.369 μs | 1,348.707 μs |  73.9272 μs | 359.3750 | 343.7500 | 328.1250 | 10793.86 KB |
| CountSaveAsync1M       |  2,462.387 μs | 1,091.935 μs |  59.8526 μs |  89.8438 |  89.8438 |  89.8438 |  3162.23 KB |
| IndexSaveAsyncLongHead |  3,198.726 μs |   145.964 μs |   8.0008 μs |  62.5000 |  62.5000 |  62.5000 |  2124.08 KB |
| CountLoadUndeclared2M  | 17,448.030 μs | 6,299.109 μs | 345.2754 μs | 656.2500 | 625.0000 | 593.7500 | 22045.14 KB |

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|-------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.387 μs** |  **0.5099 μs** | **0.0279 μs** |  **1.00** | **0.0229** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.615 μs |  0.9157 μs | 0.0502 μs |  1.04 | 0.0305 |      - |   2.81 KB |        1.24 |
| EmbedBatchBucketed | 1          |   5.642 μs |  1.1937 μs | 0.0654 μs |  1.05 | 0.0305 |      - |   2.81 KB |        1.24 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **8**          |  **53.004 μs** |  **2.5960 μs** | **0.1423 μs** |  **1.00** | **0.3052** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  18.793 μs |  2.4220 μs | 0.1328 μs |  0.35 | 0.2441 |      - |  22.38 KB |        0.76 |
| EmbedBatchBucketed | 8          |  18.906 μs |  1.4019 μs | 0.0768 μs |  0.36 | 0.2441 |      - |  22.38 KB |        0.76 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **32**         | **208.273 μs** | **34.4909 μs** | **1.8906 μs** |  **1.00** | **1.2207** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  69.357 μs |  1.9611 μs | 0.1075 μs |  0.33 | 0.9766 |      - |  82.75 KB |        0.75 |
| EmbedBatchBucketed | 32         |  63.512 μs |  3.1631 μs | 0.1734 μs |  0.30 | 0.7324 |      - |  69.23 KB |        0.62 |
|                    |            |            |            |           |       |        |        |           |             |
| **UnitLoop**           | **128**        | **826.510 μs** | **64.6846 μs** | **3.5456 μs** |  **1.00** | **4.8828** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 276.917 μs | 11.0363 μs | 0.6049 μs |  0.34 | 3.9063 | 0.4883 | 329.78 KB |        0.74 |
| EmbedBatchBucketed | 128        | 244.973 μs | 63.4206 μs | 3.4763 μs |  0.30 | 3.1738 | 0.2441 | 262.27 KB |        0.59 |

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
| **LengthFilteredScan** | **1**      | **clustered** | **136.64 ms** | **64.822 ms** | **3.553 ms** |  **1.00** |    **0.03** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  80.37 ms | 15.745 ms | 0.863 ms |  0.59 |    0.01 |  103.67 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **143.98 ms** | **52.984 ms** | **2.904 ms** |  **1.00** |    **0.02** |   **23.79 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  76.43 ms | 11.734 ms | 0.643 ms |  0.53 |    0.01 |  116.52 KB |        4.90 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **203.41 ms** |  **8.532 ms** | **0.468 ms** |  **1.00** |    **0.00** |  **103.35 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 327.15 ms |  7.089 ms | 0.389 ms |  1.61 |    0.00 |  259.28 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **210.12 ms** | **26.238 ms** | **1.438 ms** |  **1.00** |    **0.01** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 278.24 ms | 11.720 ms | 0.642 ms |  1.32 |    0.01 |  193.09 KB |        3.54 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **256.51 ms** | **46.463 ms** | **2.547 ms** |  **1.00** |    **0.01** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 440.55 ms | 33.614 ms | 1.842 ms |  1.72 |    0.02 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **254.22 ms** | **11.569 ms** | **0.634 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 414.34 ms | 29.968 ms | 1.643 ms |  1.63 |    0.01 |  1153.8 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **294.78 ms** | **22.715 ms** | **1.245 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 534.15 ms | 78.454 ms | 4.300 ms |  1.81 |    0.01 | 7215.92 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **301.68 ms** | **16.373 ms** | **0.897 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 531.07 ms | 73.699 ms | 4.040 ms |  1.76 |    0.01 |  7964.5 KB |        1.44 |

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

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **43.26 μs** |     **4.732 μs** |   **0.259 μs** |         **-** |
| Cjk    | 1000   |      56.68 μs |     3.640 μs |   0.200 μs |         - |
| **Latin**  | **10000**  |   **4,051.37 μs** |   **214.311 μs** |  **11.747 μs** |         **-** |
| Cjk    | 10000  |   6,940.26 μs | 3,084.873 μs | 169.092 μs |         - |
| **Latin**  | **65536**  | **174,661.54 μs** | **1,510.672 μs** |  **82.805 μs** |         **-** |

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
| **LodestarQuery**          | **1000**      |       **1.836 μs** |      **0.0616 μs** |     **0.0034 μs** |     **1.00** |    **0.00** |    **0.0038** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.359 μs |      0.1509 μs |     0.0083 μs |     1.28 |    0.00 |    0.0038 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.059 μs |      0.1581 μs |     0.0087 μs |     1.67 |    0.00 |    0.0610 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   3,614.120 μs |    443.7085 μs |    24.3212 μs | 1,968.28 |   11.89 |  890.6250 |  886.7188 |  886.7188 |  3723091 B |    8,780.88 |
| LuceneFromText         | 1000      |   6,524.497 μs |  1,287.7284 μs |    70.5847 μs | 3,553.29 |   33.77 |   15.6250 |    7.8125 |         - |  1375027 B |    3,242.99 |
|                        |           |                |                |               |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **27.509 μs** |      **6.1128 μs** |     **0.3351 μs** |     **1.00** |    **0.01** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      39.269 μs |      8.9770 μs |     0.4921 μs |     1.43 |    0.02 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      15.443 μs |      0.0567 μs |     0.0031 μs |     0.56 |    0.01 |    0.0916 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  75,078.390 μs | 17,530.0140 μs |   960.8792 μs | 2,729.52 |   41.67 | 1571.4286 | 1571.4286 | 1571.4286 | 61663496 B |  145,432.77 |
| LuceneFromText         | 20000     | 133,240.448 μs | 42,304.8178 μs | 2,318.8698 μs | 4,844.03 |   88.98 |  250.0000 |         - |         - | 22417452 B |   52,871.35 |

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
| Unigram | 25.09 ms | 2.520 ms | 0.138 ms |  1.00 |    0.01 |  93.7500 |   8.19 MB |        1.00 |
| Bpe     | 83.82 ms | 3.336 ms | 0.183 ms |  3.34 |    0.02 | 333.3333 |  28.47 MB |        3.48 |

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
| **BpeOnOnePathologicalToken** | **512**    |  **17.83 μs** |  **1.206 μs** | **0.066 μs** | **0.0916** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **79.93 μs** |  **4.443 μs** | **0.244 μs** | **0.1221** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **197.36 μs** | **16.932 μs** | **0.928 μs** | **0.2441** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **415.56 μs** | **39.867 μs** | **2.185 μs** | **0.4883** |  **56.63 KB** |

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
| EncodeUnseenProse | 7.613 ms | 2.398 ms | 0.1314 ms |    1.7 MB |

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
| **DpGroup**    | **cjk**      |  **15.382 μs** |  **1.6208 μs** | **0.0888 μs** |         **-** |
| MyersGroup | cjk      | 246.230 μs | 32.2526 μs | 1.7679 μs |         - |
| **DpGroup**    | **latin**    |   **8.331 μs** |  **0.9361 μs** | **0.0513 μs** |         **-** |
| MyersGroup | latin    | 135.253 μs |  3.8423 μs | 0.2106 μs |         - |

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

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **295.76 μs** |  **20.486 μs** | **1.123 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 303.60 μs | 101.678 μs | 5.573 μs |  1.03 |    0.02 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **321.64 μs** |  **28.705 μs** | **1.573 μs** |  **1.00** |    **0.01** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 319.82 μs |  34.987 μs | 1.918 μs |  0.99 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **296.68 μs** |  **31.326 μs** | **1.717 μs** |  **1.00** |    **0.01** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 309.83 μs |  13.749 μs | 0.754 μs |  1.04 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **257.37 μs** |  **11.665 μs** | **0.639 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 327.94 μs |  75.472 μs | 4.137 μs |  1.27 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **292.69 μs** |  **14.144 μs** | **0.775 μs** |  **1.00** |    **0.00** |      **-** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 308.69 μs |  18.065 μs | 0.990 μs |  1.05 |    0.00 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **181.12 μs** |   **8.406 μs** | **0.461 μs** |  **1.00** |    **0.00** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 332.56 μs |  34.246 μs | 1.877 μs |  1.84 |    0.01 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **215.74 μs** |  **55.642 μs** | **3.050 μs** |  **1.00** |    **0.02** | **0.2441** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 301.52 μs |  50.282 μs | 2.756 μs |  1.40 |    0.02 |      - |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **73.06 μs** |   **1.051 μs** | **0.058 μs** |  **1.00** |    **0.00** | **0.3662** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 332.21 μs |  66.202 μs | 3.629 μs |  4.55 |    0.04 |      - |  32.02 KB |        1.00 |

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

| Method                        | Size | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|------------:|----------:|------:|--------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **165.9 μs** |    **10.61 μs** |   **0.58 μs** |  **1.00** |    **0.00** |       **-** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   181.6 μs |    10.28 μs |   0.56 μs |  1.09 |    0.00 |       - |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   407.0 μs |    11.29 μs |   0.62 μs |  2.45 |    0.01 |  3.4180 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |             |           |       |         |         |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,169.8 μs** |   **476.61 μs** |  **26.12 μs** |  **1.00** |    **0.01** | **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,175.9 μs |   251.49 μs |  13.78 μs |  1.00 |    0.01 | 97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 6,840.9 μs | 1,899.26 μs | 104.11 μs |  2.16 |    0.03 | 54.6875 |       - |       - | 4723.45 KB |       15.11 |

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

| Method       | Shape     | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **136.29 ms** |  **11.59 ms** |  **0.635 ms** |  **1.00** |    **0.01** | **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,710.95 ms | 406.42 ms | 22.278 ms | 12.55 |    0.15 | 8000.0000 | 7000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |           |           |       |         |           |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **46.15 ms** |  **15.60 ms** |  **0.855 ms** |  **1.00** |    **0.02** | **1454.5455** | **1454.5455** | **1454.5455** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   626.97 ms |  82.25 ms |  4.508 ms | 13.59 |    0.23 | 3000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

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

| Method                   | Shape      | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-------------:|----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **195.21 ms** |    **35.204 ms** |  **1.930 ms** |  **1.00** |    **0.01** |  **1333.3333** |  **1333.3333** | **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 4,981.97 ms | 1,024.858 ms | 56.176 ms | 25.52 |    0.33 | 11000.0000 |  7000.0000 | 6000.0000 | 531.85 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,065.73 ms |    52.428 ms |  2.874 ms | 15.71 |    0.14 | 11000.0000 | 10000.0000 | 9000.0000 | 372.12 MB |        3.30 |
|                          |            |             |              |           |       |         |            |            |           |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.54 ms** |     **0.445 ms** |  **0.024 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** | **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   376.98 ms |   291.057 ms | 15.954 ms | 17.50 |    0.64 |  2000.0000 |  2000.0000 | 2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   217.28 ms |     5.706 ms |  0.313 ms | 10.09 |    0.02 |  3666.6667 |  3666.6667 | 3666.6667 |   40.4 MB |        2.85 |

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
| TruncatedSvd_Rank20                       |  13.022 ms |  4.7193 ms | 0.2587 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 105.406 ms | 44.5301 ms | 2.4408 ms |  8.10 |    0.21 |
| Nmf_Transform                             |   3.597 ms |  0.0801 ms | 0.0044 ms |  0.28 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  18.294 ms |  2.4013 ms | 0.1316 ms |  1.41 |    0.03 |

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

| Method         | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    90.32 ns |  11.00 ns | 0.603 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   677.34 ns | 115.91 ns | 6.354 ns |  7.50 |    0.07 |      - |         - |          NA |
| TokenSortRatio |   841.97 ns |  65.59 ns | 3.595 ns |  9.32 |    0.06 | 0.0134 |    1120 B |          NA |
| TokenSetRatio  |   820.40 ns |  44.09 ns | 2.416 ns |  9.08 |    0.06 | 0.0105 |     896 B |          NA |
| WRatio         | 1,118.70 ns |  14.78 ns | 0.810 ns | 12.39 |    0.07 | 0.0134 |    1120 B |          NA |

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
| Ratio_RepeatedEmoji       |  1,862,043.6797 ns |   120,684.8205 ns |   6,615.1423 ns |  1.000 |    0.00 |      - |      - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    647,208.5288 ns |    67,546.8088 ns |   3,702.4685 ns |  0.348 |    0.00 | 6.8359 | 1.9531 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,130,233.2148 ns |    41,180.8930 ns |   2,257.2637 ns |  0.607 |    0.00 | 7.8125 |      - |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 85,989,960.3889 ns | 2,282,459.9857 ns | 125,109.3341 ns | 46.181 |    0.15 |      - |      - |  510843 B |        3.18 |
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

| Method     | Operation     | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **88.64 ns** |  **13.43 ns** |  **0.736 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   197.05 ns |  59.23 ns |  3.247 ns |  2.22 |    0.04 | 0.0010 |      80 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **677.11 ns** |  **55.24 ns** |  **3.028 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 8,690.01 ns | 710.18 ns | 38.927 ns | 12.83 |    0.07 |      - |     160 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **838.75 ns** |  **57.36 ns** |  **3.144 ns** |  **1.00** |    **0.00** | **0.0105** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,875.38 ns |  87.18 ns |  4.779 ns |  2.24 |    0.01 | 0.0229 |    1944 B |        2.17 |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        | **1,103.10 ns** |  **51.15 ns** |  **2.803 ns** |  **1.00** |    **0.00** | **0.0134** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 4,535.02 ns | 409.18 ns | 22.428 ns |  4.11 |    0.02 | 0.0305 |    3096 B |        2.76 |

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

| Method                     | Length | Mean        | Error        | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **23.61 ns** |     **3.969 ns** |  **0.218 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    31.08 ns |     3.715 ns |  0.204 ns |  1.32 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    24.58 ns |     2.102 ns |  0.115 ns |  1.04 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    23.10 ns |     1.595 ns |  0.087 ns |  0.98 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **24.48 ns** |     **2.443 ns** |  **0.134 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    34.99 ns |     2.471 ns |  0.135 ns |  1.43 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    27.27 ns |     2.007 ns |  0.110 ns |  1.11 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    26.72 ns |     1.495 ns |  0.082 ns |  1.09 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **27.58 ns** |     **5.318 ns** |  **0.291 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    38.90 ns |     2.077 ns |  0.114 ns |  1.41 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    30.45 ns |     3.186 ns |  0.175 ns |  1.10 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    25.57 ns |     1.530 ns |  0.084 ns |  0.93 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **31.57 ns** |     **3.731 ns** |  **0.204 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    37.56 ns |     1.124 ns |  0.062 ns |  1.19 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    34.21 ns |     9.096 ns |  0.499 ns |  1.08 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    29.76 ns |     2.856 ns |  0.157 ns |  0.94 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **47.26 ns** |     **3.798 ns** |  **0.208 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    56.67 ns |     3.788 ns |  0.208 ns |  1.20 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    54.86 ns |     3.447 ns |  0.189 ns |  1.16 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    47.48 ns |     7.643 ns |  0.419 ns |  1.00 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **58.47 ns** |     **8.451 ns** |  **0.463 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    61.09 ns |     4.622 ns |  0.253 ns |  1.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    60.44 ns |     3.636 ns |  0.199 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    54.01 ns |     0.976 ns |  0.054 ns |  0.92 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **337.78 ns** |    **41.966 ns** |  **2.300 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   346.46 ns |    46.847 ns |  2.568 ns |  1.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   334.77 ns |     8.091 ns |  0.444 ns |  0.99 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   331.87 ns |    28.067 ns |  1.538 ns |  0.98 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,421.47 ns** |   **475.768 ns** | **26.078 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,568.09 ns |   771.547 ns | 42.291 ns |  1.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,422.56 ns |   126.199 ns |  6.917 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,530.71 ns | 1,006.378 ns | 55.163 ns |  1.02 |         - |          NA |

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

| Method             | Length | Mean        | Error       | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **373.7 ns** |    **13.13 ns** |  **0.72 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    159.4 ns |    37.26 ns |  2.04 ns |  0.43 |    0.00 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,050.2 ns** |   **528.40 ns** | **28.96 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,544.9 ns |   238.67 ns | 13.08 ns |  1.24 |    0.02 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,195.7 ns** | **1,209.07 ns** | **66.27 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,329.7 ns |   534.64 ns | 29.31 ns |  2.45 |    0.01 |         - |          NA |

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,203.0 ns** |   **721.62 ns** |  **39.55 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,095.2 ns |   304.39 ns |  16.68 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 10,494.2 ns | 1,044.79 ns |  57.27 ns |  1.46 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,833.3 ns** |   **638.77 ns** |  **35.01 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  8,935.7 ns | 1,308.46 ns |  71.72 ns |  0.91 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 18,031.6 ns | 2,494.48 ns | 136.73 ns |  1.83 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **14,923.0 ns** |   **306.43 ns** |  **16.80 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,192.0 ns |   352.45 ns |  19.32 ns |  0.75 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 16,696.1 ns |   986.13 ns |  54.05 ns |  1.12 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,064.5 ns** |   **303.82 ns** |  **16.65 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,681.2 ns |   480.57 ns |  26.34 ns |  1.12 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  8,315.4 ns | 1,132.74 ns |  62.09 ns |  1.64 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,683.2 ns** |   **970.83 ns** |  **53.21 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,972.1 ns |   797.15 ns |  43.69 ns |  0.91 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,254.0 ns | 3,232.58 ns | 177.19 ns |  1.46 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **12,548.9 ns** |   **494.47 ns** |  **27.10 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,081.5 ns | 1,909.49 ns | 104.67 ns |  0.72 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 14,666.5 ns | 1,105.04 ns |  60.57 ns |  1.17 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,704.4 ns** |   **340.78 ns** |  **18.68 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  3,680.9 ns |   386.28 ns |  21.17 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  6,794.7 ns | 1,468.76 ns |  80.51 ns |  1.83 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **6,297.3 ns** |   **221.93 ns** |  **12.16 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,447.2 ns |   942.45 ns |  51.66 ns |  0.87 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,484.6 ns |   438.83 ns |  24.05 ns |  1.51 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **11,303.6 ns** |   **684.03 ns** |  **37.49 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  8,004.9 ns | 2,121.53 ns | 116.29 ns |  0.71 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,254.8 ns |   868.05 ns |  47.58 ns |  1.17 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,290.2 ns** |   **226.66 ns** |  **12.42 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  1,829.9 ns |   373.09 ns |  20.45 ns |  0.80 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,417.1 ns |   984.68 ns |  53.97 ns |  2.37 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **5,020.5 ns** |   **650.34 ns** |  **35.65 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,561.7 ns |   367.61 ns |  20.15 ns |  0.51 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,114.4 ns | 1,346.13 ns |  73.79 ns |  1.62 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     | **10,056.9 ns** |   **380.20 ns** |  **20.84 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,446.0 ns |   232.67 ns |  12.75 ns |  0.34 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 11,820.2 ns | 3,307.56 ns | 181.30 ns |  1.18 |    0.02 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,759.0 ns** |   **486.93 ns** |  **26.69 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,046.3 ns |   506.77 ns |  27.78 ns |  0.81 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  6,789.9 ns |   356.45 ns |  19.54 ns |  1.81 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **6,496.2 ns** |    **88.85 ns** |   **4.87 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  3,746.3 ns |   243.17 ns |  13.33 ns |  0.58 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     |  9,631.0 ns | 1,115.64 ns |  61.15 ns |  1.48 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **11,296.2 ns** |   **425.63 ns** |  **23.33 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  7,795.6 ns | 5,713.83 ns | 313.19 ns |  0.69 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 13,184.0 ns |   573.11 ns |  31.41 ns |  1.17 |    0.00 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,327.9 ns** |    **80.17 ns** |   **4.39 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    458.0 ns |   187.92 ns |  10.30 ns |  0.34 |    0.01 | 0.0005 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,451.8 ns |    55.20 ns |   3.03 ns |  3.35 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **4,004.9 ns** |   **233.91 ns** |  **12.82 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,074.9 ns |    69.31 ns |   3.80 ns |  0.27 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,237.1 ns |   358.20 ns |  19.63 ns |  1.81 |    0.01 | 0.0610 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **7,741.9 ns** |   **477.05 ns** |  **26.15 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,046.4 ns |   169.22 ns |   9.28 ns |  0.26 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,067.7 ns | 2,842.99 ns | 155.83 ns |  1.43 |    0.02 | 0.0610 |    5120 B |          NA |

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

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,494.5 μs** |    **396.40 μs** |    **21.73 μs** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1495.91 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  34,608.7 μs | 11,931.14 μs |   653.99 μs |  6.30 |    0.11 |        - |        - |        - |   10.38 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  54,034.9 μs | 10,972.20 μs |   601.42 μs |  9.83 |    0.10 |        - |        - |        - |   41.86 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **874.5 μs** |     **93.52 μs** |     **5.13 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,001.9 μs |    231.38 μs |    12.68 μs |  4.58 |    0.03 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2,501.2 μs |    202.01 μs |    11.07 μs |  2.86 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,319.9 μs** |    **701.95 μs** |    **38.48 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 377,777.1 μs | 53,610.98 μs | 2,938.60 μs | 14.35 |    0.10 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 460,385.2 μs | 62,117.17 μs | 3,404.85 μs | 17.49 |    0.11 |        - |        - |        - |  199.63 KB |       0.046 |

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.507 ms** |  **0.9727 ms** | **0.0533 ms** |  **1.00** |    **0.01** | **242.1875** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  14.279 ms |  2.5884 ms | 0.1419 ms |  2.59 |    0.03 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **28.666 ms** |  **6.2531 ms** | **0.3428 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  95.016 ms | 13.3587 ms | 0.7322 ms |  3.31 |    0.04 |        - |        - |        - |   96.39 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **53.633 ms** |  **2.5297 ms** | **0.1387 ms** |  **1.00** |    **0.00** | **200.0000** | **200.0000** | **200.0000** | **3946.82 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 180.207 ms |  6.8138 ms | 0.3735 ms |  3.36 |    0.01 |        - |        - |        - |   36.38 KB |       0.009 |

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

| Method     | Band | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **117.97 ns** |     **7.216 ns** |  **0.396 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     53.01 ns |     1.679 ns |  0.092 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    119.12 ns |     4.556 ns |  0.250 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     85.30 ns |     1.216 ns |  0.067 ns |  0.72 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **12**   |    **217.16 ns** |    **45.966 ns** |  **2.520 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |     62.18 ns |     4.588 ns |  0.251 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    215.90 ns |     9.050 ns |  0.496 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |     96.59 ns |     0.864 ns |  0.047 ns |  0.44 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **14**   |    **276.69 ns** |    **21.100 ns** |  **1.157 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |     66.52 ns |     7.749 ns |  0.425 ns |  0.24 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    275.71 ns |    29.116 ns |  1.596 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 14   |    104.46 ns |     7.329 ns |  0.402 ns |  0.38 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **16**   |    **334.49 ns** |    **25.944 ns** |  **1.422 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |     69.88 ns |     2.007 ns |  0.110 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    335.97 ns |    13.967 ns |  0.766 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    106.87 ns |     3.877 ns |  0.213 ns |  0.32 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **18**   |    **406.13 ns** |    **57.722 ns** |  **3.164 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.46 ns |     2.897 ns |  0.159 ns |  0.18 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    405.09 ns |    14.139 ns |  0.775 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 18   |    112.78 ns |     3.395 ns |  0.186 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **20**   |    **482.91 ns** |    **85.533 ns** |  **4.688 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |     75.06 ns |     8.076 ns |  0.443 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    481.54 ns |    56.575 ns |  3.101 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    124.76 ns |     5.457 ns |  0.299 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **24**   |    **739.98 ns** |   **132.797 ns** |  **7.279 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     81.78 ns |    18.425 ns |  1.010 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    647.79 ns |    39.267 ns |  2.152 ns |  0.88 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    130.70 ns |     9.489 ns |  0.520 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **32**   |  **1,105.71 ns** |   **125.908 ns** |  **6.901 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |     97.98 ns |    11.641 ns |  0.638 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,101.38 ns |    46.765 ns |  2.563 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    149.24 ns |     1.600 ns |  0.088 ns |  0.13 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **48**   |  **2,447.48 ns** |   **520.416 ns** | **28.526 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    118.39 ns |    13.834 ns |  0.758 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,409.06 ns |   784.316 ns | 42.991 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    207.82 ns |    15.982 ns |  0.876 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **64**   |  **4,246.53 ns** |   **274.870 ns** | **15.067 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    143.38 ns |    16.865 ns |  0.924 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  4,246.56 ns |   458.597 ns | 25.137 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    257.03 ns |    89.980 ns |  4.932 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **96**   |  **9,508.61 ns** | **1,298.472 ns** | **71.174 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    299.68 ns |    33.985 ns |  1.863 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 10,336.88 ns |   760.915 ns | 41.708 ns |  1.09 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    779.46 ns |    16.606 ns |  0.910 ns |  0.08 |    0.00 |         - |          NA |

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.64 ns** |     **2.229 ns** |   **0.122 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.53 ns |     4.298 ns |   0.236 ns |  1.00 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    134.53 ns |     4.098 ns |   0.225 ns |  5.46 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     23.28 ns |     2.793 ns |   0.153 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **272.17 ns** |     **5.840 ns** |   **0.320 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    343.81 ns |    96.552 ns |   5.292 ns |  1.26 |    0.02 |         - |          NA |
| Distance_CodePoint         | 64     |    662.65 ns |    33.866 ns |   1.856 ns |  2.43 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    277.67 ns |     9.375 ns |   0.514 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **824.51 ns** |    **37.811 ns** |   **2.073 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,638.33 ns |   201.948 ns |  11.069 ns |  1.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 128    |  1,530.94 ns |    52.172 ns |   2.860 ns |  1.86 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    856.57 ns |    33.846 ns |   1.855 ns |  1.04 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,582.81 ns** | **1,779.447 ns** |  **97.537 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,227.21 ns | 1,992.604 ns | 109.221 ns |  1.40 |    0.01 |         - |          NA |
| Distance_CodePoint         | 512    | 13,068.74 ns |   464.543 ns |  25.463 ns |  1.13 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,867.01 ns |   326.249 ns |  17.883 ns |  0.94 |    0.01 |         - |          NA |

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

| Method             | Length | Distinct | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **335.2 ns** |     **92.59 ns** |   **5.08 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     209.9 ns |     12.84 ns |   0.70 ns |  0.63 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **334.0 ns** |     **42.22 ns** |   **2.31 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     207.7 ns |      4.63 ns |   0.25 ns |  0.62 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **416.1 ns** |     **20.18 ns** |   **1.11 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     296.5 ns |     15.07 ns |   0.83 ns |  0.71 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **434.2 ns** |     **42.29 ns** |   **2.32 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     293.4 ns |      9.41 ns |   0.52 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **507.0 ns** |     **73.28 ns** |   **4.02 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     383.1 ns |     26.97 ns |   1.48 ns |  0.76 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **515.8 ns** |     **69.91 ns** |   **3.83 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     380.1 ns |     29.22 ns |   1.60 ns |  0.74 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **595.8 ns** |     **29.59 ns** |   **1.62 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,084.4 ns |     80.33 ns |   4.40 ns |  1.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **599.6 ns** |     **43.41 ns** |   **2.38 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,042.2 ns |     65.53 ns |   3.59 ns |  1.74 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,835.4 ns** |    **202.02 ns** |  **11.07 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,091.4 ns |  4,382.35 ns | 240.21 ns |  2.77 |    0.11 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,797.6 ns** |     **64.50 ns** |   **3.54 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,694.5 ns |    877.92 ns |  48.12 ns |  2.61 |    0.02 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,395.3 ns** |    **141.47 ns** |   **7.75 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  59,196.7 ns | 10,541.67 ns | 577.82 ns |  4.11 |    0.03 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **489,912.5 ns** | **13,849.46 ns** | **759.14 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  59,119.0 ns |  2,763.10 ns | 151.45 ns |  0.12 |    0.00 |         - |          NA |

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

| Method               | Length | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **24.56 ns** |       **0.545 ns** |     **0.030 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |     109.03 ns |       7.488 ns |     0.410 ns |  4.44 |    0.02 | 0.0006 |      56 B |          NA |
| Quickenshtein        | 8      |      75.13 ns |       6.232 ns |     0.342 ns |  3.06 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     180.73 ns |      46.066 ns |     2.525 ns |  7.36 |    0.09 | 0.0014 |     128 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **272.64 ns** |      **14.285 ns** |     **0.783 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,240.39 ns |     125.715 ns |     6.891 ns | 19.22 |    0.05 |      - |     280 B |          NA |
| Quickenshtein        | 64     |   1,263.82 ns |     170.336 ns |     9.337 ns |  4.64 |    0.03 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   8,858.76 ns |     892.668 ns |    48.930 ns | 32.49 |    0.18 |      - |     576 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,521.54 ns** |   **1,719.764 ns** |    **94.266 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 506,524.56 ns |  44,723.433 ns | 2,451.442 ns | 43.97 |    0.36 |      - |    2073 B |          NA |
| Quickenshtein        | 512    |  37,298.19 ns |   2,571.660 ns |   140.961 ns |  3.24 |    0.03 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 752,071.24 ns | 111,803.896 ns | 6,128.349 ns | 65.28 |    0.65 |      - |    4161 B |          NA |

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

| Method            | Labels | Mean     | Error     | StdDev  | Gen0   | Gen1   | Allocated |
|------------------ |------- |---------:|----------:|--------:|-------:|-------:|----------:|
| **F1Macro**           | **1000**   | **472.3 μs** | **102.20 μs** | **5.60 μs** | **0.4883** |      **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 481.7 μs |  27.04 μs | 1.48 μs |      - |      - |  67.71 KB |
| ReportRows        | 1000   | 512.3 μs |  61.75 μs | 3.38 μs | 0.9766 |      - | 153.99 KB |
| **F1Macro**           | **5000**   | **624.0 μs** |  **32.57 μs** | **1.79 μs** | **3.9063** | **0.9766** | **337.24 KB** |
| PrecisionWeighted | 5000   | 624.0 μs |  27.35 μs | 1.50 μs | 3.9063 | 0.9766 | 337.24 KB |
| ReportRows        | 5000   | 801.2 μs |  42.67 μs | 2.34 μs | 8.7891 | 3.9063 | 767.27 KB |

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

| Method                          | Shape   | Mean            | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |----------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |       **140.79 μs** |      **16.463 μs** |      **0.902 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |   232,138.02 μs |  29,842.986 μs |  1,635.795 μs | 1,648.90 |   13.58 | 666.6667 | 666.6667 | 666.6667 | 31413.77 KB |   12,805.61 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |     **2,923.63 μs** |     **285.408 μs** |     **15.644 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 1,065,617.72 μs | 448,085.169 μs | 24,561.060 μs |   364.49 |    7.47 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |        **22.88 μs** |       **3.599 μs** |      **0.197 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |       770.24 μs |     107.256 μs |      5.879 μs |    33.66 |    0.33 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

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

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,313.0 ns** |       **251.43 ns** |     **13.78 ns** | **0.0038** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     5,466.5 ns |       178.60 ns |      9.79 ns |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       134.0 ns |        28.14 ns |      1.54 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,771.0 ns |       685.38 ns |     37.57 ns | 0.0076 |     656 B |
| Report         | 1000    | 2       |     5,555.1 ns |       215.93 ns |     11.84 ns | 0.0610 |    5336 B |
| **Matrix**         | **1000**    | **10**      |     **3,441.7 ns** |       **313.61 ns** |     **17.19 ns** | **0.0153** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     5,942.7 ns |       500.21 ns |     27.42 ns | 0.0153 |    1336 B |
| AccuracyScore  | 1000    | 10      |       133.3 ns |        13.62 ns |      0.75 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,528.2 ns |       229.80 ns |     12.60 ns | 0.0191 |    1880 B |
| Report         | 1000    | 10      |     8,915.3 ns |        61.72 ns |      3.38 ns | 0.1373 |   12360 B |
| **Matrix**         | **100000**  | **2**       |   **312,998.6 ns** |     **9,701.49 ns** |    **531.77 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   692,983.2 ns |    46,439.31 ns |  2,545.50 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    15,418.5 ns |       278.24 ns |     15.25 ns |      - |         - |
| F1Macro        | 100000  | 2       |   316,807.4 ns |    17,051.03 ns |    934.62 ns |      - |     656 B |
| Report         | 100000  | 2       |   336,288.3 ns |    18,281.07 ns |  1,002.05 ns |      - |    5360 B |
| **Matrix**         | **100000**  | **10**      |   **310,532.8 ns** |    **26,683.00 ns** |  **1,462.59 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   865,848.7 ns |    98,701.15 ns |  5,410.14 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    15,412.4 ns |       196.25 ns |     10.76 ns |      - |         - |
| F1Macro        | 100000  | 10      |   311,291.9 ns |    20,148.23 ns |  1,104.39 ns |      - |    1880 B |
| Report         | 100000  | 10      |   300,951.8 ns |    22,045.36 ns |  1,208.38 ns |      - |   12704 B |
| **Matrix**         | **1000000** | **2**       | **3,335,811.7 ns** |   **283,146.36 ns** | **15,520.21 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,264,119.4 ns |   853,865.95 ns | 46,803.27 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   238,871.7 ns |    83,523.30 ns |  4,578.19 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,240,498.4 ns | 1,116,810.12 ns | 61,216.13 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,251,240.4 ns |   840,670.51 ns | 46,079.99 ns |      - |    5379 B |
| **Matrix**         | **1000000** | **10**      | **3,117,207.9 ns** |   **320,110.35 ns** | **17,546.33 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 8,867,870.3 ns |   706,843.26 ns | 38,744.46 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   249,835.8 ns |   169,675.89 ns |  9,300.51 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,099,194.6 ns |   152,271.14 ns |  8,346.49 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,003,182.8 ns |   366,542.73 ns | 20,091.44 ns |      - |   12747 B |

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

| Method   | Samples | Request       | Mean          | Error          | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|---------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **10,680.48 μs** |     **742.192 μs** |    **40.682 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,936.22 μs |   5,307.852 μs |   290.941 μs |     3.36 |    0.03 | 600.0000 | 600.0000 | 600.0000 |  5090192 B |    3,927.62 |
|          |         |               |               |                |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **15.46 μs** |       **0.699 μs** |     **0.038 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,517.21 μs |   1,632.905 μs |    89.505 μs | 2,232.53 |    6.93 | 600.0000 | 600.0000 | 600.0000 |  5089705 B |          NA |
|          |         |               |               |                |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **194,080.96 μs** | **100,807.721 μs** | **5,525.611 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 213,602.82 μs |   4,058.779 μs |   222.475 μs |     1.10 |    0.03 |        - |        - |        - | 23228948 B |          NA |
|          |         |               |               |                |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **257.71 μs** |       **3.391 μs** |     **0.186 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 223,321.56 μs |  46,860.999 μs | 2,568.609 μs |   866.57 |    8.65 |        - |        - |        - | 23229116 B |          NA |

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

| Method     | Band | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **70.90 ns** |    **19.450 ns** |  **1.066 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 4    |     70.12 ns |     0.803 ns |  0.044 ns |  0.99 |    0.01 |         - |          NA |
| Dp_Cjk     | 4    |     70.34 ns |     6.513 ns |  0.357 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |     69.93 ns |    11.849 ns |  0.650 ns |  0.99 |    0.02 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **6**    |     **95.21 ns** |     **4.883 ns** |  **0.268 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     68.85 ns |     6.135 ns |  0.336 ns |  0.72 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |     95.39 ns |    16.541 ns |  0.907 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    116.17 ns |     4.027 ns |  0.221 ns |  1.22 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **8**    |    **131.62 ns** |    **10.770 ns** |  **0.590 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |     78.56 ns |    14.565 ns |  0.798 ns |  0.60 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    128.50 ns |    16.283 ns |  0.893 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    147.94 ns |     7.366 ns |  0.404 ns |  1.12 |    0.01 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **10**   |    **174.81 ns** |     **3.463 ns** |  **0.190 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |     85.79 ns |    21.495 ns |  1.178 ns |  0.49 |    0.01 |         - |          NA |
| Dp_Cjk     | 10   |    176.15 ns |     7.009 ns |  0.384 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    123.40 ns |    15.330 ns |  0.840 ns |  0.71 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **12**   |    **239.05 ns** |    **16.581 ns** |  **0.909 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     97.52 ns |     9.716 ns |  0.533 ns |  0.41 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    237.17 ns |    64.524 ns |  3.537 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    133.97 ns |    14.813 ns |  0.812 ns |  0.56 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **16**   |    **362.26 ns** |    **41.030 ns** |  **2.249 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    116.58 ns |    10.212 ns |  0.560 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    361.07 ns |    20.187 ns |  1.107 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    194.81 ns |    13.375 ns |  0.733 ns |  0.54 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **24**   |    **746.96 ns** |    **70.678 ns** |  **3.874 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |    152.38 ns |    13.491 ns |  0.739 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    747.70 ns |    78.011 ns |  4.276 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    196.91 ns |     8.416 ns |  0.461 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **32**   |  **1,327.22 ns** |    **89.466 ns** |  **4.904 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    189.53 ns |     2.921 ns |  0.160 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,330.99 ns |   190.248 ns | 10.428 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    239.06 ns |     4.528 ns |  0.248 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **48**   |  **2,887.08 ns** |   **261.070 ns** | **14.310 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    262.67 ns |     6.673 ns |  0.366 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,905.10 ns |   460.468 ns | 25.240 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 48   |    332.81 ns |    23.313 ns |  1.278 ns |  0.12 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **64**   |  **5,357.35 ns** |   **616.749 ns** | **33.806 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    337.02 ns |    18.987 ns |  1.041 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,170.12 ns | 1,037.539 ns | 56.871 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    414.31 ns |     6.374 ns |  0.349 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **96**   | **11,446.66 ns** |   **914.259 ns** | **50.114 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    695.47 ns |   146.906 ns |  8.052 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,438.22 ns | 1,126.021 ns | 61.721 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |  1,313.75 ns |    36.710 ns |  2.012 ns |  0.11 |    0.00 |         - |          NA |

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **250.60 μs** |  **24.560 μs** |  **1.346 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   254.22 μs |  27.617 μs |  1.514 μs |  1.01 |    0.01 |      - |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    91.15 μs |   8.726 μs |  0.478 μs |  0.36 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   251.68 μs |  53.025 μs |  2.906 μs |  1.00 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **254.60 μs** | **153.850 μs** |  **8.433 μs** |  **1.00** |    **0.04** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   288.94 μs |   6.924 μs |  0.380 μs |  1.14 |    0.03 | 0.4883 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    93.01 μs |   1.830 μs |  0.100 μs |  0.37 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   251.58 μs |  80.097 μs |  4.390 μs |  0.99 |    0.03 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,546.01 μs** | **623.326 μs** | **34.167 μs** |  **1.00** |    **0.02** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,526.88 μs | 202.214 μs | 11.084 μs |  0.99 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,182.44 μs |  87.144 μs |  4.777 μs |  0.46 |    0.01 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 2,525.71 μs | 279.624 μs | 15.327 μs |  0.99 |    0.01 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,510.91 μs** |  **67.137 μs** |  **3.680 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,587.90 μs | 258.986 μs | 14.196 μs |  1.03 |    0.01 |      - |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,182.24 μs | 178.442 μs |  9.781 μs |  0.47 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 2,510.89 μs | 162.385 μs |  8.901 μs |  1.00 |    0.00 |      - |     803 B |        1.00 |

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

| Method      | NeedleLength | Mean         | Error       | StdDev    | Allocated |
|------------ |------------- |-------------:|------------:|----------:|----------:|
| **Embedded**    | **65**           |     **7.842 μs** |   **0.5765 μs** | **0.0316 μs** |         **-** |
| EqualLength | 65           |     2.390 μs |   0.2366 μs | 0.0130 μs |         - |
| **Embedded**    | **128**          |    **29.596 μs** |   **4.3301 μs** | **0.2373 μs** |         **-** |
| EqualLength | 128          |     4.570 μs |   0.7622 μs | 0.0418 μs |         - |
| **Embedded**    | **512**          | **1,079.355 μs** | **130.9322 μs** | **7.1768 μs** |         **-** |
| EqualLength | 512          |    38.380 μs |   1.3030 μs | 0.0714 μs |         - |

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.348 ms |  3.0408 ms | 0.1667 ms |  39.0625 |  35.1563 |  23.4375 |   3711.6 KB |
| TokenizerJsonWordPiece |  7.546 ms |  0.4847 ms | 0.0266 ms |  15.6250 |        - |        - |  5852.22 KB |
| TokenizerJsonUnigram   |  9.330 ms |  0.7171 ms | 0.0393 ms |        - |        - |        - |  4748.81 KB |
| SpieceModel            |  3.745 ms |  0.2030 ms | 0.0111 ms |  23.4375 |  15.6250 |  15.6250 |  4292.21 KB |
| TfidfSave              |  1.487 ms |  0.3070 ms | 0.0168 ms |  15.6250 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  3.479 ms |  4.7252 ms | 0.2590 ms |  15.6250 |   7.8125 |   7.8125 |  2921.36 KB |
| EmbeddingIndexSave     |  5.390 ms |  0.2723 ms | 0.0149 ms | 257.8125 | 257.8125 | 257.8125 | 20349.89 KB |
| EmbeddingIndexLoad     |  8.616 ms |  1.7948 ms | 0.0984 ms |  93.7500 |  93.7500 |  93.7500 | 16094.37 KB |
| EmbeddingIndexSaveFile | 81.664 ms | 13.2087 ms | 0.7240 ms |        - |        - |        - |   323.53 KB |
| EmbeddingIndexLoadGzip | 83.254 ms | 59.7696 ms | 3.2762 ms |        - |        - |        - | 16095.28 KB |

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

| Method                     | Shape   | Mean         | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **7,402.37 μs** | **1,146.936 μs** | **62.867 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,032.83 μs |   366.471 μs | 20.088 μs |  2.17 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **141.53 μs** |     **9.651 μs** |  **0.529 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    197.58 μs |    35.169 μs |  1.928 μs |  1.40 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,916.39 μs** |    **77.080 μs** |  **4.225 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,277.42 μs |   849.629 μs | 46.571 μs |  1.12 |    0.01 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.08 μs** |     **0.546 μs** |  **0.030 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     26.90 μs |     0.531 μs |  0.029 μs |  1.17 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |

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

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **52.66 μs** |     **4.156 μs** |  **0.228 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    58.82 μs |     3.115 μs |  0.171 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   133.60 μs |    13.914 μs |  0.763 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   541.61 μs |    29.191 μs |  1.600 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **577.89 μs** |    **41.396 μs** |  **2.269 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   595.03 μs |   275.110 μs | 15.080 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,369.96 μs |   201.398 μs | 11.039 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 7,694.23 μs | 1,688.993 μs | 92.579 μs | 296.8750 | 296.8750 | 296.8750 | 8000260 B |

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

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1     | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|---------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **57.621 ms** |  **1.6066 ms** | **0.0881 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |  10.345 ms |  0.8275 ms | 0.0454 ms |  0.18 |   78.1250 |  78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   8.960 ms |  0.5445 ms | 0.0298 ms |  0.16 |         - |        - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   7.311 ms |  0.5603 ms | 0.0307 ms |  0.13 |         - |        - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  11.257 ms |  0.3545 ms | 0.0194 ms |  0.20 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **59.533 ms** |  **3.0221 ms** | **0.1656 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  13.701 ms |  6.0984 ms | 0.3343 ms |  0.23 |  218.7500 | 218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  10.812 ms |  0.9121 ms | 0.0500 ms |  0.18 |         - |        - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   7.466 ms |  0.0960 ms | 0.0053 ms |  0.13 |         - |        - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  11.136 ms |  0.9083 ms | 0.0498 ms |  0.19 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **982.583 ms** | **99.6525 ms** | **5.4623 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  43.558 ms |  7.7498 ms | 0.4248 ms |  0.04 |  583.3333 | 583.3333 | 500.0000 |  10335.43 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  36.351 ms |  2.6652 ms | 0.1461 ms |  0.04 |         - |        - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  29.201 ms |  0.6399 ms | 0.0351 ms |  0.03 |         - |        - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  45.614 ms | 14.6146 ms | 0.8011 ms |  0.05 |         - |        - |        - |   1718.81 KB |       0.014 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **984.315 ms** | **84.7264 ms** | **4.6441 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  56.957 ms |  5.7031 ms | 0.3126 ms |  0.06 |  888.8889 | 888.8889 | 777.7778 |  18664.86 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  43.807 ms |  4.0788 ms | 0.2236 ms |  0.04 |         - |        - |        - |    1718.8 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  29.965 ms |  3.3843 ms | 0.1855 ms |  0.03 |         - |        - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  44.972 ms |  1.3416 ms | 0.0735 ms |  0.05 |         - |        - |        - |   1718.79 KB |        0.01 |

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

| Method                 | Mean           | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |     1,229.6 ns |        72.74 ns |       3.99 ns |   0.0648 |        - |        - |     5464 B |
| TfidfSaveAsync         |     1,404.4 ns |       186.42 ns |      10.22 ns |   0.0668 |        - |        - |     5592 B |
| HashingSaveAsync       |     1,237.2 ns |       404.94 ns |      22.20 ns |   0.0648 |        - |        - |     5496 B |
| IndexSaveAsync         |       375.3 ns |        44.87 ns |       2.46 ns |   0.0076 |        - |        - |      648 B |
| CountLoadSeekable1400K | 9,688,298.8 ns | 7,970,681.30 ns | 436,899.94 ns | 656.2500 | 640.6250 | 609.3750 | 12227071 B |

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
| **Count**                | **200**       |  **2.104 ms** | **0.0973 ms** | **0.0053 ms** |  **1.00** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |  1.547 ms | 0.0846 ms | 0.0046 ms |  0.74 |   3.9063 |        - |        - |   407.6 KB |        0.47 |
| Hashing              | 200       |  2.217 ms | 0.1918 ms | 0.0105 ms |  1.05 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       |  1.745 ms | 0.0335 ms | 0.0018 ms |  0.83 |   1.9531 |        - |        - |  273.65 KB |        0.31 |
|                      |           |           |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      |  **9.843 ms** | **0.6235 ms** | **0.0342 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.54 KB** |        **1.00** |
| CountWithStopWords   | 1000      |  7.715 ms | 0.4058 ms | 0.0222 ms |  0.78 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 10.868 ms | 1.5124 ms | 0.0829 ms |  1.10 | 500.0000 | 500.0000 | 500.0000 | 2348.47 KB |        0.68 |
| HashingWithStopWords | 1000      |  9.037 ms | 1.1886 ms | 0.0652 ms |  0.92 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

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

| Method                     | Pattern       | Mean          | Error         | StdDev      | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|------------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **35,381.318 μs** | **5,549.0624 μs** | **304.1628 μs** |       **-** |       **-** |       **-** |  **1554.6 KB** |
| Construct                  | [^ ]+         |      2.042 μs |     0.1371 μs |   0.0075 μs |  0.0648 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.247 μs |     0.4464 μs |   0.0245 μs |  0.0877 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **28,221.488 μs** | **1,647.8261 μs** |  **90.3229 μs** |       **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      2.027 μs |     1.9943 μs |   0.1093 μs |  0.0648 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.748 μs |     0.9614 μs |   0.0527 μs |  0.0877 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **39,178.814 μs** | **1,544.8755 μs** |  **84.6798 μs** |       **-** |       **-** |       **-** |    **1556 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.284 μs |     0.3585 μs |   0.0196 μs |  0.0801 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.716 μs |     2.8005 μs |   0.1535 μs |  0.1068 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **21,892.744 μs** |   **789.3313 μs** |  **43.2659 μs** | **62.5000** | **62.5000** | **62.5000** | **9743.45 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.068 μs |     0.9561 μs |   0.0524 μs |  0.0210 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1.803 μs |     0.1443 μs |   0.0079 μs |  0.0458 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **36,406.642 μs** | **2,132.5664 μs** | **116.8932 μs** |       **-** |       **-** |       **-** | **1554.49 KB** |
| Construct                  | \\S+          |      2.290 μs |     0.0674 μs |   0.0037 μs |  0.0610 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.450 μs |     0.8645 μs |   0.0474 μs |  0.0877 |       - |       - |    7.29 KB |

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
| **Lodestar**     | **WordPiece**     |  **17.84 ms** |  **1.913 ms** | **0.105 ms** |  **1.00** |    **0.01** |  **93.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  47.00 ms |  9.997 ms | 0.548 ms |  2.64 |    0.03 |        - |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **SentencePiece** |  **52.67 ms** |  **8.793 ms** | **0.482 ms** |  **1.00** |    **0.01** | **100.0000** |   **7.98 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  64.36 ms |  3.527 ms | 0.193 ms |  1.22 |    0.01 |        - |   5.65 MB |        0.71 |
|              |               |           |           |          |       |         |          |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **84.76 ms** | **11.682 ms** | **0.640 ms** |  **1.00** |    **0.01** | **333.3333** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 263.42 ms | 34.868 ms | 1.911 ms |  3.11 |    0.03 | 500.0000 |  59.08 MB |        2.08 |

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **40.32 ns** | **4.049 ns** | **0.222 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  35.65 ns | 1.140 ns | 0.063 ns |  0.88 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **78.84 ns** | **4.842 ns** | **0.265 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  66.20 ns | 2.678 ns | 0.147 ns |  0.84 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **103.59 ns** | **1.350 ns** | **0.074 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 |  88.25 ns | 7.995 ns | 0.438 ns |  0.85 |         - |          NA |

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
| **Count**                 | **200**       |    **251.8 μs** |    **97.01 μs** |   **5.32 μs** |  **1.00** |    **0.03** |    **2.4414** |    **0.4883** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    274.7 μs |    21.94 μs |   1.20 μs |  1.09 |    0.02 |    2.4414 |         - |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    749.3 μs |    66.57 μs |   3.65 μs |  2.98 |    0.06 |    6.8359 |    1.9531 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,078.5 μs |   165.90 μs |   9.09 μs |  8.26 |    0.15 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    281.7 μs |     8.56 μs |   0.47 μs |  1.12 |    0.02 |    1.4648 |         - |         - |   136.13 KB |        0.66 |
|                       |           |             |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,626.2 μs** |    **64.24 μs** |   **3.52 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,784.1 μs |    46.74 μs |   2.56 μs |  1.10 |    0.00 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,982.9 μs |   966.26 μs |  52.96 μs |  2.45 |    0.03 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 13,127.6 μs | 4,129.29 μs | 226.34 μs |  8.07 |    0.12 | 1015.6250 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,671.1 μs |    67.32 μs |   3.69 μs |  1.03 |    0.00 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

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
| **Lodestar** | **200**       |   **3.879 ms** |   **0.2391 ms** |  **0.0131 ms** |  **1.00** |    **0.00** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  47.660 ms |  47.0048 ms |  2.5765 ms | 12.29 |    0.58 |  7400.0000 |  7400.0000 |  7400.0000 |  28.27 MB |       15.85 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.950 ms** |   **1.4981 ms** |  **0.0821 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 358.497 ms | 399.8532 ms | 21.9173 ms | 19.97 |    1.06 | 72000.0000 | 72000.0000 | 72000.0000 | 324.33 MB |       40.00 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

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
| cdist_ratio_n50 | 0.139 | 0.027 | 0.19x | 0.139 | 0.027 | 0.19x |
| cdist_wratio_n50 | 2.076 | 2.217 | 1.07x | 2.076 | 2.217 | 1.07x |
| cdist_ratio_varied_n50 | 0.125 | 0.022 | 0.17x | 0.125 | 0.022 | 0.17x |
| cdist_ratio_varied_cutoff90_n50 | 0.049 | 0.022 | 0.44x | 0.049 | 0.022 | 0.44x |
| cdist_ratio_n200 | 2.474 | 0.321 | 0.13x | 2.476 | 0.321 | 0.13x |
| cdist_wratio_n200 | 29.715 | 32.671 | 1.10x | 29.717 | 32.670 | 1.10x |
| cdist_ratio_varied_n200 | 2.195 | 0.250 | 0.11x | 2.198 | 0.250 | 0.11x |
| cdist_ratio_varied_cutoff90_n200 | 0.825 | 0.250 | 0.30x | 0.826 | 0.250 | 0.30x |
| cdist_ratio_n500 | 15.039 | 1.919 | 0.13x | 15.055 | 1.919 | 0.13x |
| cdist_wratio_n500 | 174.965 | 193.239 | 1.10x | 175.102 | 193.231 | 1.10x |
| cdist_ratio_varied_n500 | 13.027 | 1.462 | 0.11x | 13.136 | 1.462 | 0.11x |
| cdist_ratio_varied_cutoff90_n500 | 4.835 | 1.462 | 0.30x | 4.917 | 1.462 | 0.30x |

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
| vocab_txt | 3.363 | 9.493 | 2.82x | 3.487 | 9.492 | 2.72x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 8.107 | 16.625 | 2.05x | 8.373 | 16.621 | 1.99x | 706,526 | 706,526 |
| tokenizer_json_unigram | 10.343 | 53.572 | 5.18x | 10.661 | 53.567 | 5.02x | 1,990,038 | 1,990,038 |
| spiece_model | 3.967 | 15.669 | 3.95x | 4.081 | 15.668 | 3.84x | 347,061 | 347,061 |
| tfidf_save | 1.587 | 2.595 | 1.64x | 1.591 | 2.595 | 1.63x | 581,787 | 591,922 |
| tfidf_load | 4.004 | 4.004 | 1.00x | 4.162 | 4.004 | 0.96x | 581,787 | 591,922 |
| embedding_index_save | 5.177 | 2.266 | 0.44x | 5.398 | 2.266 | 0.42x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 82.491 | 61.249 | 0.74x | 11.433 | 6.238 | 0.55x | 20,589,007 | 15,360,128 |
| embedding_index_load | 8.402 | 1.481 | 0.18x | 8.905 | 1.479 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 8.476 | 1.220 | 0.14x | 8.982 | 1.219 | 0.14x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 7.225 | 1.469 | 0.20x | 7.553 | 1.468 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 4.003 | 1.468 | 0.37x | 3.964 | 1.468 | 0.37x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 70.29x | 0.000 | 0.001 | 70.27x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 426.986 | 585.794 | 1.37x | 426.959 | 585.776 | 1.37x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 80.702 | 90.851 | 1.13x | 80.701 | 90.847 | 1.13x | 15,250,490 | 14,022,374 |

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
| kfold_n10000 | 0.097 | 0.098 | 1.01x | 0.097 | 0.098 | 1.01x |
| stratified_n10000 | 0.150 | 0.755 | 5.02x | 0.150 | 0.755 | 5.02x |
| traintest_n10000 | 0.007 | 0.177 | 25.21x | 0.007 | 0.177 | 25.21x |
| kfold_seeded_n10000 | 0.332 | 0.682 | 2.06x | 0.332 | 0.682 | 2.06x |
| stratified_seeded_n10000 | 0.416 | 1.455 | 3.49x | 0.416 | 1.455 | 3.49x |
| traintest_seeded_n10000 | 0.165 | 0.432 | 2.62x | 0.165 | 0.432 | 2.62x |
| stratified_traintest_n10000 | 0.735 | 2.365 | 3.22x | 0.735 | 2.364 | 3.22x |
| group_kfold_n10000 | 0.135 | 0.489 | 3.63x | 0.135 | 0.489 | 3.63x |
| stratified_group_kfold_n10000 | 0.231 | 21.322 | 92.18x | 0.231 | 21.320 | 92.18x |
| timeseries_n10000 | 0.000 | 0.010 | 71.96x | 0.000 | 0.010 | 71.96x |
| repeated_kfold_n10000 | 1.067 | 1.894 | 1.77x | 1.067 | 1.894 | 1.77x |
| kfold_n100000 | 1.103 | 2.113 | 1.92x | 1.209 | 2.110 | 1.75x |
| stratified_n100000 | 1.724 | 7.274 | 4.22x | 1.832 | 7.272 | 3.97x |
| traintest_n100000 | 0.153 | 0.448 | 2.92x | 0.155 | 0.448 | 2.90x |
| kfold_seeded_n100000 | 4.048 | 7.278 | 1.80x | 4.172 | 7.273 | 1.74x |
| stratified_seeded_n100000 | 4.380 | 12.959 | 2.96x | 4.490 | 12.951 | 2.88x |
| traintest_seeded_n100000 | 2.212 | 1.872 | 0.85x | 2.242 | 1.872 | 0.83x |
| stratified_traintest_n100000 | 8.945 | 21.793 | 2.44x | 9.038 | 21.792 | 2.41x |
| group_kfold_n100000 | 1.564 | 6.135 | 3.92x | 1.661 | 6.134 | 3.69x |
| stratified_group_kfold_n100000 | 2.458 | 219.891 | 89.45x | 2.557 | 219.878 | 86.00x |
| timeseries_n100000 | 0.000 | 0.040 | 276.76x | 0.000 | 0.040 | 276.85x |
| repeated_kfold_n100000 | 11.487 | 22.098 | 1.92x | 11.610 | 22.097 | 1.90x |
| kfold_n1000000 | 9.546 | 21.169 | 2.22x | 10.208 | 21.165 | 2.07x |
| stratified_n1000000 | 17.098 | 81.018 | 4.74x | 17.707 | 81.016 | 4.58x |
| traintest_n1000000 | 0.556 | 2.865 | 5.15x | 0.607 | 2.865 | 4.72x |
| kfold_seeded_n1000000 | 42.221 | 71.084 | 1.68x | 42.806 | 71.077 | 1.66x |
| stratified_seeded_n1000000 | 51.713 | 128.225 | 2.48x | 52.708 | 128.214 | 2.43x |
| traintest_seeded_n1000000 | 22.416 | 19.919 | 0.89x | 22.644 | 19.918 | 0.88x |
| stratified_traintest_n1000000 | 105.364 | 235.639 | 2.24x | 105.574 | 235.621 | 2.23x |
| group_kfold_n1000000 | 15.162 | 59.138 | 3.90x | 15.631 | 59.125 | 3.78x |
| stratified_group_kfold_n1000000 | 26.991 | 2234.684 | 82.79x | 28.037 | 2234.024 | 79.68x |
| timeseries_n1000000 | 0.000 | 0.348 | 2560.82x | 0.000 | 0.348 | 2558.32x |
| repeated_kfold_n1000000 | 133.891 | 212.343 | 1.59x | 136.016 | 212.329 | 1.56x |

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
| welch_t_n1000 | 0.007 | 0.559 | 85.33x | 0.007 | 0.559 | 85.32x |
| mann_whitney_n1000 | 0.057 | 0.577 | 10.13x | 0.057 | 0.577 | 10.13x |
| chi_square_n1000 | 0.000 | 0.212 | 1218.25x | 0.000 | 0.212 | 1218.28x |
| fligner_n1000 | 0.124 | 0.657 | 5.29x | 0.124 | 0.657 | 5.29x |
| anderson_ksamp_n1000 | 0.204 | 0.349 | 1.71x | 0.204 | 0.349 | 1.71x |
| spearman_matrix_n1000 | 0.194 | 0.671 | 3.45x | 0.194 | 0.671 | 3.45x |
| pointbiserial_n1000 | 0.010 | 0.402 | 39.38x | 0.010 | 0.402 | 39.38x |
| welch_t_n10000 | 0.046 | 0.593 | 12.86x | 0.046 | 0.593 | 12.86x |
| mann_whitney_n10000 | 0.575 | 2.446 | 4.25x | 0.575 | 2.446 | 4.25x |
| chi_square_n10000 | 0.001 | 0.215 | 195.87x | 0.001 | 0.215 | 195.88x |
| fligner_n10000 | 1.320 | 2.900 | 2.20x | 1.361 | 2.900 | 2.13x |
| anderson_ksamp_n10000 | 2.105 | 3.467 | 1.65x | 2.182 | 3.467 | 1.59x |
| spearman_matrix_n10000 | 2.174 | 4.182 | 1.92x | 2.221 | 4.182 | 1.88x |
| pointbiserial_n10000 | 0.122 | 0.485 | 3.97x | 0.122 | 0.485 | 3.97x |
| welch_t_n100000 | 0.469 | 1.207 | 2.58x | 0.469 | 1.207 | 2.58x |
| mann_whitney_n100000 | 6.182 | 26.242 | 4.25x | 6.181 | 26.240 | 4.25x |
| chi_square_n100000 | 0.011 | 0.235 | 20.65x | 0.011 | 0.235 | 20.65x |
| fligner_n100000 | 17.927 | 30.874 | 1.72x | 18.413 | 30.873 | 1.68x |
| anderson_ksamp_n100000 | 19.656 | 40.208 | 2.05x | 19.941 | 40.203 | 2.02x |
| spearman_matrix_n100000 | 20.646 | 49.600 | 2.40x | 21.009 | 49.600 | 2.36x |
| pointbiserial_n100000 | 1.388 | 1.620 | 1.17x | 1.399 | 6.470 | 4.62x |

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
| normalize_n10000 | 0.214 | 0.346 | 1.62x | 0.215 | 0.346 | 1.61x |
| polynomial_n10000 | 0.319 | 0.851 | 2.67x | 0.374 | 0.850 | 2.27x |
| kbins_fit_n10000 | 1.437 | 2.056 | 1.43x | 1.437 | 2.056 | 1.43x |
| kbins_n10000 | 0.456 | 3.701 | 8.11x | 0.541 | 3.700 | 6.84x |
| quantile_fit_n10000 | 1.545 | 3.654 | 2.36x | 1.545 | 3.653 | 2.36x |
| quantile_n10000 | 1.895 | 1.706 | 0.90x | 1.898 | 1.706 | 0.90x |
| power_fit_n10000 | 5.134 | 66.442 | 12.94x | 5.138 | 66.439 | 12.93x |
| power_n10000 | 1.160 | 0.768 | 0.66x | 1.162 | 0.768 | 0.66x |
| knn_impute_n10000 | 6.065 | 23.349 | 3.85x | 6.065 | 92.455 | 15.24x |
| label_n10000 | 1.233 | 0.863 | 0.70x | 1.233 | 0.863 | 0.70x |
| normalize_n100000 | 1.610 | 2.006 | 1.25x | 2.004 | 2.006 | 1.00x |
| polynomial_n100000 | 4.041 | 6.865 | 1.70x | 4.765 | 6.865 | 1.44x |
| kbins_fit_n100000 | 15.154 | 8.650 | 0.57x | 15.961 | 8.649 | 0.54x |
| kbins_n100000 | 5.454 | 13.606 | 2.49x | 6.272 | 13.605 | 2.17x |
| quantile_fit_n100000 | 15.254 | 18.429 | 1.21x | 15.963 | 18.428 | 1.15x |
| quantile_n100000 | 18.157 | 17.146 | 0.94x | 18.618 | 17.145 | 0.92x |
| power_fit_n100000 | 55.528 | 463.227 | 8.34x | 58.149 | 463.216 | 7.97x |
| power_n100000 | 10.881 | 6.313 | 0.58x | 11.392 | 6.313 | 0.55x |
| knn_impute_n100000 | 6.064 | 19.607 | 3.23x | 6.064 | 78.226 | 12.90x |
| label_n100000 | 15.296 | 8.504 | 0.56x | 15.595 | 8.503 | 0.55x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BkTreeNearestBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `DecompositionWidthBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `HouseholderQrBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `PartitionValidityBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `ProcessExtractBenchmarks`
- `RankingMetricsBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `VectorStoreChurnBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 51.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 4.579 | 3.099 | +45% | 20% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x16x16 | `MetaNumerics_Fit` | step | 9.83 | 15.31 | -36% | 30% |
| `BkTreeBenchmarks` | Radius=4, Shape=uniform | `TreeWithinDistance` | step | 1.76 | 1.3 | +35% | 30% |
| `BkTreeBenchmarks` | Radius=4, Shape=clustered | `TreeWithinDistance` | step | 1.81 | 1.34 | +35% | 30% |
| `BkTreeBenchmarks` | Radius=2, Shape=clustered | `TreeWithinDistance` | step | 1.61 | 1.23 | +31% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=8 | `Fastenshtein` | step | 4.44 | 3.4 | +31% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=16 | `FuzzySharp_PerText` | step | 1.81 | 2.6 | -30% | 30% |
| `LcsGateBenchmarks` | Band=32 | `Kernel_Cjk` | drift | 0.13 | 0.1 | +30% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | drift | 25.52 | 37.83 | -30% | 20% |
| `AgglomerativeIncumbentBenchmarks` | Rows=500, Method=ward | `Aglomera_GetClustering` | drift | 40.16 | 31.02 | +29% | 20% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=16 | `Lodestar_HeldPattern` | drift | 0.51 | 0.72 | -29% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | drift | 15.71 | 22.13 | -29% | 20% |
| `LcsGateBenchmarks` | Band=32 | `Kernel` | drift | 0.09 | 0.07 | +29% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.36 | 0.28 | +29% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_Bartlett` | drift | 0.108 | 0.151 | -28% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=100000, Request=AccuracyAlone | `MlNet` | drift | 2232.53 | 3109.69 | -28% | 20% |
| `Bm25Benchmarks` | Documents=20000 | `LuceneQuery` | drift | 0.56 | 0.78 | -28% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.37 | 0.29 | +28% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | drift | 8.146 | 6.291 | +27% | 20% |
| `BkTreeBenchmarks` | Radius=3, Shape=uniform | `TreeWithinDistance` | drift | 1.63 | 1.22 | +27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Accord_Bartlett` | drift | 0.221 | 0.301 | -27% | 20% |
| `BkTreeBenchmarks` | Radius=2, Shape=uniform | `TreeWithinDistance` | drift | 1.32 | 1.04 | +26% | 20% |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | drift | 8.26 | 11.27 | -26% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=16 | `Lodestar_HeldPattern` | drift | 0.58 | 0.79 | -25% | 20% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=32 | `FuzzySharp_PerText` | drift | 1.17 | 1.66 | -25% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 3.36 | 4.49 | -25% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | drift | 0.005 | 0.008 | -25% | 20% |
| `IndelPatternBenchmarks` | Corpus=oneWordAffix, Length=32 | `Lodestar_HeldPattern` | drift | 0.71 | 0.96 | -25% | 20% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.05 | 0.04 | +25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `FingerprintsOnly` | drift | 0.05 | 0.04 | +25% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Lloyd` | drift | 3.31 | 4.41 | -25% | 20% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | drift | 10.09 | 13.94 | -24% | 20% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `MetaNumerics_Fit` | drift | 2.86 | 3.78 | -24% | 20% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=16 | `FuzzySharp_PerText` | drift | 1.62 | 2.14 | -24% | 20% |
| `CorrelationBenchmarks` | PairCount=10000 | `Lodestar_KendallTau` | drift | 30.89 | 24.74 | +24% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.41 | 1.87 | -24% | 20% |
| `IndelPatternBenchmarks` | Corpus=longAffix, Length=32 | `Lodestar_HeldPattern` | drift | 0.72 | 0.96 | -24% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | drift | 1.1 | 1.65 | -24% | 20% |
| `BkTreeBenchmarks` | Radius=3, Shape=clustered | `TreeWithinDistance` | drift | 1.72 | 1.36 | +24% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 2.59 | 3.38 | -23% | 20% |
| `CorrelationBenchmarks` | PairCount=10000 | `Lodestar_KendallTau_Asymptotic` | drift | 30.72 | 24.9 | +23% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Accord_Binomial` | drift | 1.893 | 2.469 | -23% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=4 | `Lodestar_HeldPattern` | drift | 0.34 | 0.44 | -23% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel` | drift | 0.11 | 0.09 | +22% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | drift | 1.113 | 1.413 | -21% | 20% |
| `IndelPatternBenchmarks` | Corpus=beyondTable, Length=32 | `FuzzySharp_PerText` | drift | 1.12 | 1.5 | -21% | 20% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=16 | `FuzzySharp_PerText` | drift | 1.48 | 1.89 | -21% | 20% |
| `KMeansFitIncumbentBenchmarks` | Shape=10000x2x8 | `NumFlat_Fit` | drift | 4.58 | 5.97 | -21% | 20% |
| `CdistCutoffBenchmarks` | Cutoff=90, Widths=True | `Lodestar_Cdist_EveryPair` | drift | 4.55 | 3.66 | +20% | 20% |
| `IndelPatternBenchmarks` | Corpus=beyondTable, Length=32 | `Lodestar_HeldPattern` | drift | 0.75 | 0.94 | -20% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `SketchThenVerify` | drift | 0.04 | 0.05 | -20% | 20% |

**Still away from their median: 42.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `MetaNumericsPcaBenchmarks` | Shape=2000x50 | `MetaNumerics_VarianceFraction` | drift | 364.49 | 171.29 | +103% | 20% |
| `MetaNumericsPcaBenchmarks` | Shape=2000x10 | `MetaNumerics_VarianceFraction` | drift | 1648.9 | 751.97 | +100% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_FisherExact` | drift | 0.09 | 0.44 | -80% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_FisherExact` | drift | 0.39 | 1.51 | -74% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_OneWayAnova` | drift | 0.83 | 3.06 | -73% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | drift | 0.03 | 0.11 | -73% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 2.51 | 8.92 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_StudentT` | drift | 0.6 | 2.15 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_ChiSquare` | drift | 0.19 | 0.67 | -72% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | drift | 1.12 | 3.71 | -70% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | drift | 1.14 | 3.69 | -69% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | drift | 2.1 | 6.71 | -69% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | drift | 0.73 | 2.14 | -66% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 0.79 | 2.23 | -65% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | drift | 0.37 | 0.97 | -62% | 20% |
| `LcsGateBenchmarks` | Band=20 | `Kernel` | step | 0.16 | 0.1 | +60% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | drift | 0.38 | 0.83 | -55% | 20% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=AccuracyAlone | `MlNet` | step | 866.57 | 1647.9 | -47% | 41% |
| `LcsGateBenchmarks` | Band=20 | `Kernel_Cjk` | step | 0.26 | 0.18 | +44% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Bartlett` | drift | 0.28 | 0.52 | -44% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_Wilcoxon` | step | 0.36 | 0.25 | +44% | 42% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Binomial` | drift | 1.29 | 2.32 | -43% | 20% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=16 | `Lodestar_HeldPattern` | step | 0.27 | 0.47 | -43% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `Lodestar_HeldPattern` | step | 0.26 | 0.45 | -42% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `Lodestar_HeldPattern` | step | 0.34 | 0.58 | -41% | 30% |
| `IndelPatternBenchmarks` | Corpus=sharing24, Length=32 | `FuzzySharp_PerText` | step | 1.18 | 1.92 | -39% | 30% |
| `KMeansFitIncumbentBenchmarks` | Shape=50000x8x32 | `MetaNumerics_Fit` | step | 17.49 | 27.5 | -36% | 30% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `Lodestar_HeldPattern` | step | 0.69 | 1.07 | -36% | 30% |
| `IndelPatternBenchmarks` | Corpus=suffixOnly, Length=32 | `FuzzySharp_PerText` | step | 1.17 | 1.8 | -35% | 30% |
| `IndelPatternBenchmarks` | Corpus=unrelated, Length=32 | `FuzzySharp_PerText` | step | 1.43 | 2.2 | -35% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_AndersonDarling` | drift | 1.99 | 3.1 | -34% | 20% |
| `IndelPatternBenchmarks` | Corpus=oneWordAffix, Length=32 | `FuzzySharp_PerText` | step | 1.17 | 1.76 | -34% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=10000 | `Lodestar_Binomial` | step | 0.002 | 0.003 | -33% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Bartlett` | step | 0.14 | 0.21 | -33% | 30% |
| `FuzzCodePointBenchmarks` | — | `Ratio_DistinctAstral` | step | 46.181 | 34.935 | +32% | 30% |
| `PrincipalComponentVarianceBenchmarks` | Shape=100x200 | `NumFlat_Pca` | step | 2.17 | 1.66 | +31% | 30% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Accord_Levene` | drift | 1.33 | 1.98 | -29% | 20% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | drift | 0.18 | 0.14 | +29% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_Binomial` | drift | 0.26 | 0.37 | -27% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_AndersonDarling` | drift | 3.08 | 4.25 | -25% | 20% |
| `VarianceAndFitBenchmarks` | GroupSize=100 | `Lodestar_ClopperPearson` | drift | 1.58 | 2.02 | -22% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `SignaturesOnly` | drift | 0.04 | 0.05 | -20% | 20% |
