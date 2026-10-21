# Nightly benchmark run

<!-- nightly-baseline: c4a19d86a5f8c9899f169f26ac7e107de05eedd1 -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks CosineTopKSelectionBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks PartitionValidityBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks StationarityBenchmarks StatsBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks VectorAutoregressionBenchmarks WeightedLeastSquaresBenchmarks -->
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
- `Bm25Benchmarks`
- `CoxBenchmarks`
- `SurvivalBenchmarks`

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error            | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **624.9 ns** |        **174.09 ns** |         **9.54 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         568.4 ns |         11.58 ns |         0.63 ns |     0.91 |    0.01 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       1,546.6 ns |        117.17 ns |         6.42 ns |     2.48 |    0.03 |   0.2460 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       2,085.6 ns |      1,909.08 ns |       104.64 ns |     3.34 |    0.15 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       2,900.6 ns |      1,410.87 ns |        77.33 ns |     4.64 |    0.12 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |       5,789.8 ns |        285.85 ns |        15.67 ns |     9.27 |    0.12 |   0.0076 |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       2,938.6 ns |        740.61 ns |        40.60 ns |     4.70 |    0.08 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **55,141.3 ns** |     **17,628.57 ns** |       **966.28 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      42,848.1 ns |      2,650.98 ns |       145.31 ns |     0.78 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |     550,863.3 ns |     30,235.75 ns |     1,657.32 ns |     9.99 |    0.15 | 166.0156 | 166.0156 | 166.0156 | 1324848 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,332,138.4 ns |     83,483.39 ns |     4,576.01 ns |    24.16 |    0.37 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   1,744,121.7 ns |    306,499.60 ns |    16,800.28 ns |    31.64 |    0.54 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 190,527,890.6 ns | 33,125,569.98 ns | 1,815,724.28 ns | 3,455.96 |   59.25 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   1,776,690.4 ns |    402,453.19 ns |    22,059.82 ns |    32.23 |    0.60 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **13.69 μs** |   **1.614 μs** |  **0.088 μs** |  **1.00** |    **0.01** | **0.9613** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    16.14 μs |  21.712 μs |  1.190 μs |  1.18 |    0.08 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,345.96 μs** |  **55.228 μs** |  **3.027 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,054.99 μs | 235.587 μs | 12.913 μs |  0.78 |    0.01 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **575.18 ns** |     **333.150 ns** |     **18.261 ns** |   **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,599.44 ns |     367.072 ns |     20.120 ns |   2.78 |    0.08 | 0.0057 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     1,159.43 ns |     218.785 ns |     11.992 ns |   2.02 |    0.06 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     2,052.22 ns |     473.052 ns |     25.930 ns |   3.57 |    0.10 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     3,676.59 ns |     937.005 ns |     51.360 ns |   6.40 |    0.19 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     4,908.23 ns |     174.818 ns |      9.582 ns |   8.54 |    0.23 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     1,264.65 ns |     115.882 ns |      6.352 ns |   2.20 |    0.06 | 0.0973 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     1,965.06 ns |     280.704 ns |     15.386 ns |   3.42 |    0.10 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       476.78 ns |      95.261 ns |      5.222 ns |   0.83 |    0.02 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,126.42 ns |   1,161.495 ns |     63.665 ns |   3.70 |    0.14 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       663.34 ns |     340.085 ns |     18.641 ns |   1.15 |    0.04 | 0.0486 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |       531.78 ns |      71.636 ns |      3.927 ns |   0.93 |    0.03 | 0.0582 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       252.50 ns |      55.588 ns |      3.047 ns |   0.44 |    0.01 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |       888.19 ns |     236.153 ns |     12.944 ns |   1.55 |    0.05 | 0.0563 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        63.73 ns |       9.717 ns |      0.533 ns |   0.11 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       383.58 ns |     111.759 ns |      6.126 ns |   0.67 |    0.02 | 0.0577 |     968 B |          NA |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **30,081.32 ns** |   **1,009.971 ns** |     **55.360 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   131,839.34 ns |   7,890.685 ns |    432.515 ns |  4.383 |    0.01 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   281,359.55 ns | 116,073.213 ns |  6,362.364 ns |  9.353 |    0.18 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,395,727.63 ns | 694,138.460 ns | 38,048.071 ns | 46.399 |    1.10 | 3.9063 |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      |   675,265.18 ns | 361,468.981 ns | 19,813.335 ns | 22.448 |    0.57 |      - |      33 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 2,505,343.95 ns | 874,215.505 ns | 47,918.702 ns | 83.286 |    1.39 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,051,162.19 ns | 122,919.632 ns |  6,737.640 ns | 34.944 |    0.20 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,490,373.29 ns |  49,801.645 ns |  2,729.796 ns | 49.545 |    0.11 | 3.9063 |   80371 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    42,493.77 ns |  14,148.619 ns |    775.534 ns |  1.413 |    0.02 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   201,648.03 ns |  55,233.293 ns |  3,027.523 ns |  6.703 |    0.09 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   111,653.32 ns |  44,723.365 ns |  2,451.439 ns |  3.712 |    0.07 | 4.7607 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   210,755.81 ns |  28,673.512 ns |  1,571.692 ns |  7.006 |    0.05 | 4.6387 |   80176 B |          NA |
| Lodestar_FisherExact           | 10000      |       253.66 ns |      54.071 ns |      2.964 ns |  0.008 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |       865.64 ns |     149.281 ns |      8.183 ns |  0.029 |    0.00 | 0.0563 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        64.77 ns |      24.537 ns |      1.345 ns |  0.002 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       393.27 ns |      29.559 ns |      1.620 ns |  0.013 |    0.00 | 0.0577 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |-------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |   **1,056.2 ns** |     **595.86 ns** |     **32.66 ns** |  **1.00** |    **0.04** |  **0.3014** | **0.0019** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |   2,154.0 ns |   3,567.14 ns |    195.53 ns |  2.04 |    0.17 |  0.3242 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |     270.1 ns |      59.26 ns |      3.25 ns |  0.26 |    0.01 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     518.5 ns |     149.10 ns |      8.17 ns |  0.49 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |     473.8 ns |      84.94 ns |      4.66 ns |  0.45 |    0.01 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |   2,450.4 ns |     347.03 ns |     19.02 ns |  2.32 |    0.06 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |   4,904.7 ns |     802.33 ns |     43.98 ns |  4.65 |    0.13 |  0.0534 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |   3,724.4 ns |   1,576.78 ns |     86.43 ns |  3.53 |    0.12 |  0.0648 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |   2,916.2 ns |     256.80 ns |     14.08 ns |  2.76 |    0.07 |       - |      - |      48 B |       0.009 |
|                          |           |              |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     | **151,734.4 ns** | **263,253.33 ns** | **14,429.80 ns** | **1.006** |    **0.12** | **28.5645** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 898,752.7 ns | 117,048.90 ns |  6,415.85 ns | 5.961 |    0.52 | 28.3203 | 6.8359 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |  29,231.2 ns |   5,794.11 ns |    317.59 ns | 0.194 |    0.02 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |  55,281.0 ns |   3,346.50 ns |    183.43 ns | 0.367 |    0.03 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     627.6 ns |     100.63 ns |      5.52 ns | 0.004 |    0.00 |  0.0029 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     | 510,244.0 ns | 325,523.31 ns | 17,843.03 ns | 3.384 |    0.31 | 11.7188 | 2.9297 |  200216 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 882,177.3 ns | 158,287.74 ns |  8,676.29 ns | 5.851 |    0.51 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |           NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |   7,534.1 ns |   2,349.42 ns |    128.78 ns | 0.050 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|-----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **206.9 μs** |   **102.1 μs** |  **5.59 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,205.2 μs** |   **362.4 μs** | **19.87 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **3,717.8 μs** |   **454.3 μs** | **24.90 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **12,516.1 μs** | **1,353.4 μs** | **74.18 μs** | **281.2500** | **265.6250** | **250.0000** | **1734.99 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |--------------:|--------------:|------------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |      **8.274 μs** |     **1.4573 μs** |   **0.0799 μs** |  **1.00** |    **0.01** |    **0.9766** |    **0.0153** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |      7.411 μs |     2.1075 μs |   0.1155 μs |  0.90 |    0.01 |    0.9308 |    0.0229 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     11.252 μs |     0.8133 μs |   0.0446 μs |  1.36 |    0.01 |    1.6327 |    0.0763 |        - |   26.84 KB |        1.68 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **31.812 μs** |     **1.8130 μs** |   **0.0994 μs** |  **1.00** |    **0.00** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     11.944 μs |     1.7401 μs |   0.0954 μs |  0.38 |    0.00 |    3.7994 |    0.4120 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     24.535 μs |    13.4002 μs |   0.7345 μs |  0.77 |    0.02 |    3.0212 |    0.3357 |        - |   49.57 KB |        0.69 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **6,285.534 μs** | **2,680.6150 μs** | **146.9336 μs** |  **1.00** |    **0.03** |  **335.9375** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  6,229.353 μs | 1,766.3688 μs |  96.8206 μs |  0.99 |    0.02 |  335.9375 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  6,787.542 μs | 1,804.3694 μs |  98.9036 μs |  1.08 |    0.03 |  507.8125 |  500.0000 | 500.0000 | 2598.82 KB |        1.71 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **10,577.636 μs** | **7,373.1557 μs** | **404.1475 μs** |  **1.00** |    **0.05** | **1046.8750** | **1031.2500** | **890.6250** | **6649.96 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             |  8,719.171 μs |   911.6292 μs |  49.9695 μs |  0.83 |    0.03 |  843.7500 |  828.1250 | 687.5000 | 5660.23 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 11,379.141 μs | 5,636.7760 μs | 308.9707 μs |  1.08 |    0.04 |  500.0000 |  500.0000 | 500.0000 | 4916.38 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,213.5 μs** |     **202.45 μs** |     **11.10 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    53,189.1 μs |  39,710.93 μs |  2,176.69 μs |  43.83 |    1.59 |  3900.0000 |  1600.0000 |  900.0000 |   59881.2 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,141.9 μs** |     **202.03 μs** |     **11.07 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    62,296.1 μs |  41,706.77 μs |  2,286.09 μs |  54.56 |    1.79 |  5111.1111 |  1888.8889 |  888.8889 |  83111.71 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **409.8 μs** |      **36.59 μs** |      **2.01 μs** |   **1.00** |    **0.01** |     **1.4648** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |    79,098.6 μs |  11,990.36 μs |    657.23 μs | 193.00 |    1.61 |  4857.1429 |  1857.1429 |  857.1429 |  81289.91 KB |    2,678.97 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,411.3 μs** |      **61.77 μs** |      **3.39 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    42,625.7 μs |  13,656.53 μs |    748.56 μs |  30.20 |    0.46 |  3923.0769 |  1846.1538 |  923.0769 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |     **9,013.8 μs** |   **9,538.15 μs** |    **522.82 μs** |   **1.00** |    **0.07** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,045,368.7 μs | 448,339.84 μs | 24,575.02 μs | 116.23 |    6.13 | 35000.0000 | 10000.0000 | 4000.0000 | 537148.98 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |     **9,327.9 μs** |   **4,412.39 μs** |    **241.86 μs** |   **1.00** |    **0.03** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete |   968,705.2 μs | 417,659.08 μs | 22,893.30 μs | 103.90 |    3.14 | 48000.0000 | 12000.0000 | 4000.0000 | 747349.54 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **3,772.8 μs** |     **854.22 μs** |     **46.82 μs** |   **1.00** |    **0.02** |     **3.9063** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 1,760,005.1 μs | 283,210.08 μs | 15,523.70 μs | 466.55 |    6.17 | 47000.0000 | 12000.0000 | 4000.0000 | 732680.99 KB |    8,148.33 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **10,824.5 μs** |   **1,071.71 μs** |     **58.74 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     |   753,523.7 μs | 433,410.04 μs | 23,756.67 μs |  69.61 |    1.93 | 38000.0000 | 11000.0000 | 4000.0000 | 573623.49 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 13.89 ms |  3.641 ms | 0.200 ms |  812.5000 |  781.2500 | 406.2500 |  21.53 MB |
| CountLoadAsync4M | 34.59 ms | 11.979 ms | 0.657 ms | 1538.4615 | 1461.5385 | 692.3077 |  43.93 MB |
| CountLoad4M      | 34.75 ms | 32.139 ms | 1.762 ms | 1466.6667 | 1400.0000 | 666.6667 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error          | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|---------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 113,063.49 μs | 108,174.720 μs | 5,929.421 μs |  200.0000 |  200.0000 | 200.0000 | 54334.54 KB |
| CountSave240K         |     236.96 μs |      77.954 μs |     4.273 μs |    9.7656 |    3.4180 |   3.1738 |   504.67 KB |
| CountSaveAsync240K    |     252.35 μs |      21.152 μs |     1.159 μs |   11.2305 |    4.8828 |   4.8828 |   739.45 KB |
| TfidfSave45K          |      27.67 μs |       3.590 μs |     0.197 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      29.20 μs |       9.022 μs |     0.495 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     139.63 μs |      70.835 μs |     3.883 μs |    7.5684 |    1.4648 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     137.18 μs |      67.766 μs |     3.714 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  17,461.60 μs |  21,319.719 μs | 1,168.606 μs |  593.7500 |  562.5000 | 187.5000 |    22049 KB |
| CountLoadAsync4M      |  41,440.61 μs |  24,969.036 μs | 1,368.637 μs | 1230.7692 | 1153.8462 | 384.6154 |  44983.7 KB |
| CountLoad4M           |  37,318.85 μs |  12,624.824 μs |   692.009 μs | 1142.8571 | 1071.4286 | 357.1429 | 44977.35 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error         | StdDev      | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |--------------:|--------------:|------------:|----------:|----------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     11.576 μs |    11.1230 μs |   0.6097 μs |    1.6785 |    0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    277.620 μs |   226.9724 μs |  12.4411 μs |   35.6445 |   29.2969 |  29.2969 |   740.12 KB |
| HashingLoadUndeclared  |      2.589 μs |     0.2125 μs |   0.0116 μs |    5.1003 |         - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     16.285 μs |     0.7119 μs |   0.0390 μs |    7.4158 |    0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |     72.429 μs |    10.7216 μs |   0.5877 μs |   16.1133 |    3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    536.364 μs |   320.9846 μs |  17.5943 μs |   38.0859 |   37.1094 |   8.7891 |   656.34 KB |
| CountLoadUndeclared1M  |  7,419.112 μs | 9,004.4437 μs | 493.5639 μs |  500.0000 |  484.3750 | 281.2500 | 10794.05 KB |
| CountSaveAsync1M       |  1,227.102 μs |   779.5081 μs |  42.7275 μs |  105.4688 |   97.6563 |  97.6563 |  3162.03 KB |
| IndexSaveAsyncLongHead |  1,922.453 μs | 1,973.6795 μs | 108.1840 μs |   93.7500 |   89.8438 |  89.8438 |  2123.87 KB |
| CountLoadUndeclared2M  | 11,940.373 μs | 1,834.4142 μs | 100.5504 μs | 1046.8750 | 1031.2500 | 625.0000 | 22045.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|-----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **3.198 μs** |   **0.2812 μs** |  **0.0154 μs** |  **1.00** |    **0.01** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   3.339 μs |   1.7548 μs |  0.0962 μs |  1.04 |    0.03 |  0.1640 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   3.405 μs |   1.0849 μs |  0.0595 μs |  1.07 |    0.02 |  0.1640 |      - |    2.7 KB |        1.19 |
|                    |            |            |             |            |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **30.859 μs** |   **8.3246 μs** |  **0.4563 μs** |  **1.00** |    **0.02** |  **1.8005** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  10.482 μs |   1.0900 μs |  0.0597 μs |  0.34 |    0.00 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  10.462 μs |   2.2704 μs |  0.1244 μs |  0.34 |    0.01 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
|                    |            |            |             |            |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **124.686 μs** |  **72.6967 μs** |  **3.9848 μs** |  **1.00** |    **0.04** |  **6.7139** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  40.338 μs |   1.0874 μs |  0.0596 μs |  0.32 |    0.01 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  37.993 μs |  26.8667 μs |  1.4727 μs |  0.30 |    0.01 |  4.2114 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |             |            |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **503.822 μs** | **591.8660 μs** | **32.4422 μs** |  **1.00** |    **0.08** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 154.862 μs |  23.9771 μs |  1.3143 μs |  0.31 |    0.02 | 20.0195 | 2.6855 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 137.258 μs |  19.2740 μs |  1.0565 μs |  0.27 |    0.01 | 15.8691 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0     | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|----------:|------:|--------:|---------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** |  **94.89 ms** |   **4.554 ms** |  **0.250 ms** |  **1.00** |    **0.00** |        **-** |   **27.11 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  50.72 ms |  51.972 ms |  2.849 ms |  0.53 |    0.03 |        - |   103.6 KB |        3.82 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **102.96 ms** |  **13.720 ms** |  **0.752 ms** |  **1.00** |    **0.01** |        **-** |   **23.78 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  49.31 ms |  69.776 ms |  3.825 ms |  0.48 |    0.03 |        - |  116.39 KB |        4.89 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **146.57 ms** |  **13.320 ms** |  **0.730 ms** |  **1.00** |    **0.01** |        **-** |   **103.3 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 235.85 ms | 793.343 ms | 43.486 ms |  1.61 |    0.26 |        - |  258.45 KB |        2.50 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **150.83 ms** |  **29.553 ms** |  **1.620 ms** |  **1.00** |    **0.01** |        **-** |   **54.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 185.57 ms | 147.821 ms |  8.103 ms |  1.23 |    0.05 |        - |   192.8 KB |        3.54 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **184.20 ms** |  **58.528 ms** |  **3.208 ms** |  **1.00** |    **0.02** |        **-** |  **949.61 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 277.75 ms |  96.925 ms |  5.313 ms |  1.51 |    0.03 |        - | 1365.78 KB |        1.44 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **187.90 ms** |   **9.928 ms** |  **0.544 ms** |  **1.00** |    **0.00** |        **-** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 262.44 ms | 232.151 ms | 12.725 ms |  1.40 |    0.06 |        - | 1152.67 KB |        1.55 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **211.04 ms** |  **19.733 ms** |  **1.082 ms** |  **1.00** |    **0.01** |        **-** | **5112.73 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 303.80 ms |  54.477 ms |  2.986 ms |  1.44 |    0.01 |        - | 7215.35 KB |        1.41 |
|                    |        |           |           |            |           |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **213.40 ms** |  **17.743 ms** |  **0.973 ms** |  **1.00** |    **0.01** | **333.3333** | **5513.74 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 303.64 ms | 280.701 ms | 15.386 ms |  1.42 |    0.06 |        - | 7962.86 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **29.28 μs** |      **7.213 μs** |     **0.395 μs** |         **-** |
| Cjk    | 1000   |      36.22 μs |      8.093 μs |     0.444 μs |         - |
| **Latin**  | **10000**  |   **2,729.59 μs** |     **98.503 μs** |     **5.399 μs** |         **-** |
| Cjk    | 10000  |   3,804.23 μs |  1,289.087 μs |    70.659 μs |         - |
| **Latin**  | **65536**  | **129,101.75 μs** | **48,320.528 μs** | **2,648.611 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean          | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |--------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |      **1.005 μs** |      **0.9780 μs** |     **0.0536 μs** |     **1.00** |    **0.06** |    **0.0248** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |      1.388 μs |      0.1291 μs |     0.0071 μs |     1.38 |    0.06 |    0.0248 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |      2.019 μs |      2.2181 μs |     0.1216 μs |     2.01 |    0.14 |    0.3128 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |  2,374.589 μs |  2,167.5188 μs |   118.8090 μs | 2,368.26 |  148.16 |  921.8750 |  890.6250 |  886.7188 |  3723055 B |    8,780.79 |
| LuceneFromText         | 1000      |  3,897.186 μs |  3,224.5363 μs |   176.7477 μs | 3,886.80 |  232.50 |   85.9375 |   78.1250 |    7.8125 |  1375101 B |    3,243.16 |
|                        |           |               |                |               |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |     **12.589 μs** |      **3.7944 μs** |     **0.2080 μs** |     **1.00** |    **0.02** |    **0.0153** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |     20.043 μs |      4.9759 μs |     0.2727 μs |     1.59 |    0.03 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      9.960 μs |      2.3492 μs |     0.1288 μs |     0.79 |    0.01 |    0.5035 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 45,545.372 μs | 48,562.0233 μs | 2,661.8484 μs | 3,618.39 |  190.38 | 1727.2727 | 1727.2727 | 1727.2727 | 61663337 B |  145,432.40 |
| LuceneFromText         | 20000     | 79,818.246 μs | 32,263.5609 μs | 1,768.4747 μs | 6,341.23 |  151.95 | 1166.6667 | 1000.0000 |         - | 22417061 B |   52,870.43 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 15.41 ms |  2.619 ms | 0.144 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 55.94 ms | 42.435 ms | 2.326 ms |  3.63 |    0.13 | 1777.7778 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean       | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |-----------:|----------:|----------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **9.084 μs** |  **1.276 μs** | **0.0700 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **19.637 μs** |  **5.355 μs** | **0.2935 μs** | **0.8850** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **41.527 μs** | **11.607 μs** | **0.6362 μs** | **1.7090** | **0.0610** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **261.936 μs** | **27.315 μs** | **1.4972 μs** | **3.4180** |      **-** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 4.790 ms | 2.175 ms | 0.1192 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean      | Error    | StdDev    | Allocated |
|----------- |--------- |----------:|---------:|----------:|----------:|
| **DpGroup**    | **cjk**      | **10.262 μs** | **1.234 μs** | **0.0677 μs** |         **-** |
| MyersGroup | cjk      | 92.818 μs | 8.088 μs | 0.4433 μs |         - |
| **DpGroup**    | **latin**    |  **5.668 μs** | **2.139 μs** | **0.1173 μs** |         **-** |
| MyersGroup | latin    | 74.849 μs | 6.480 μs | 0.3552 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **180.70 μs** |  **66.963 μs** | **3.670 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 165.22 μs |   6.694 μs | 0.367 μs |  0.91 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **193.13 μs** |  **15.554 μs** | **0.853 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 180.02 μs |  30.710 μs | 1.683 μs |  0.93 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **173.42 μs** |  **18.355 μs** | **1.006 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 165.63 μs |  21.453 μs | 1.176 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **149.69 μs** |   **4.341 μs** | **0.238 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 182.33 μs |  60.879 μs | 3.337 μs |  1.22 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **169.21 μs** | **102.942 μs** | **5.643 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 167.63 μs |   9.741 μs | 0.534 μs |  0.99 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   |  **87.20 μs** |   **9.100 μs** | **0.499 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 182.97 μs |  74.757 μs | 4.098 μs |  2.10 |    0.04 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **123.72 μs** |  **18.044 μs** | **0.989 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 172.88 μs | 138.200 μs | 7.575 μs |  1.40 |    0.05 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **49.32 μs** |  **27.635 μs** | **1.515 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 185.06 μs |  60.801 μs | 3.333 μs |  3.75 |    0.11 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |------------:|-------------:|----------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **105.92 μs** |   **138.954 μs** |  **7.617 μs** |  **1.00** |    **0.09** |   **1.0986** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |    93.66 μs |     6.646 μs |  0.364 μs |  0.89 |    0.05 |   1.0986 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   252.75 μs |     4.059 μs |  0.223 μs |  2.39 |    0.14 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |             |              |           |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **2,224.67 μs** |   **365.762 μs** | **20.049 μs** |  **1.00** |    **0.01** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 2,004.07 μs | 1,076.480 μs | 59.005 μs |  0.90 |    0.02 |  99.6094 | 99.6094 | 99.6094 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 4,058.85 μs |   550.358 μs | 30.167 μs |  1.82 |    0.02 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error    | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|---------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **113.84 ms** | **11.82 ms** | **0.648 ms** |  **1.00** |    **0.01** |  **1800.0000** | **1800.0000** | **1800.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,292.93 ms | 72.26 ms | 3.961 ms | 11.36 |    0.06 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |          |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **38.91 ms** | **35.01 ms** | **1.919 ms** |  **1.00** |    **0.06** |  **1428.5714** | **1428.5714** | **1428.5714** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   453.68 ms | 61.25 ms | 3.357 ms | 11.68 |    0.49 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **141.76 ms** |  **90.221 ms** |  **4.945 ms** |  **1.00** |    **0.04** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 3,880.87 ms | 137.801 ms |  7.553 ms | 27.40 |    0.85 | 38000.0000 | 14000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 2,270.50 ms | 468.458 ms | 25.678 ms | 16.03 |    0.52 | 22000.0000 | 12000.0000 | 10000.0000 | 372.11 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **14.51 ms** |   **3.866 ms** |  **0.212 ms** |  **1.00** |    **0.02** |  **1343.7500** |  **1343.7500** |  **1343.7500** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   284.78 ms | 364.381 ms | 19.973 ms | 19.62 |    1.22 |  5000.0000 |  3500.0000 |  3000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   161.98 ms |  16.299 ms |  0.893 ms | 11.16 |    0.15 |  3750.0000 |  3750.0000 |  3750.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean      | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  7.982 ms |  6.5162 ms | 0.3572 ms |  1.00 |    0.05 |
| Nmf_Rank20                                | 60.999 ms | 13.2808 ms | 0.7280 ms |  7.65 |    0.30 |
| Nmf_Transform                             |  1.937 ms |  0.1887 ms | 0.0103 ms |  0.24 |    0.01 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 12.434 ms |  0.4980 ms | 0.0273 ms |  1.56 |    0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  62.19 ns |   6.659 ns |  0.365 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   | 436.11 ns |   7.658 ns |  0.420 ns |  7.01 |    0.04 |      - |         - |          NA |
| TokenSortRatio | 491.72 ns | 222.726 ns | 12.208 ns |  7.91 |    0.17 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 540.63 ns |  97.773 ns |  5.359 ns |  8.69 |    0.09 | 0.0534 |     896 B |          NA |
| WRatio         | 704.91 ns | 213.958 ns | 11.728 ns | 11.34 |    0.17 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error              | StdDev          | Median             | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,302,953.5368 ns |    448,596.6611 ns |  24,589.0968 ns |  1,314,946.9053 ns |  1.000 |    0.02 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    332,322.1763 ns |     66,182.9199 ns |   3,627.7092 ns |    332,834.8062 ns |  0.255 |    0.00 | 35.6445 | 13.6719 |  602664 B |        3.75 |
| WRatio_ProseAndOneEmoji   |    736,175.0140 ns |    586,334.7897 ns |  32,138.9885 ns |    718,668.1543 ns |  0.565 |    0.02 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 53,871,438.6000 ns | 11,740,916.1235 ns | 643,559.2333 ns | 53,872,439.1000 ns | 41.356 |    0.81 |       - |       - |  510776 B |        3.18 |
| WRatio_EmptyOperand       |          0.0020 ns |          0.0453 ns |       0.0025 ns |          0.0012 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **64.06 ns** |  **20.20 ns** |  **1.107 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   134.66 ns |  18.12 ns |  0.993 ns |  2.10 |    0.03 | 0.0048 |      80 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **432.01 ns** |  **48.21 ns** |  **2.642 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 5,908.26 ns | 767.32 ns | 42.059 ns | 13.68 |    0.11 | 0.0076 |     160 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **586.50 ns** | **710.20 ns** | **38.929 ns** |  **1.00** |    **0.08** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,184.94 ns | 378.60 ns | 20.752 ns |  2.03 |    0.12 | 0.1144 |    1944 B |        2.17 |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **695.60 ns** |  **91.33 ns** |  **5.006 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 2,890.81 ns | 577.85 ns | 31.674 ns |  4.16 |    0.05 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **14.97 ns** |   **2.733 ns** |  **0.150 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    19.54 ns |  10.782 ns |  0.591 ns |  1.31 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    15.44 ns |   4.545 ns |  0.249 ns |  1.03 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    14.71 ns |  10.020 ns |  0.549 ns |  0.98 |    0.03 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **15.59 ns** |   **3.304 ns** |  **0.181 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    19.92 ns |   1.965 ns |  0.108 ns |  1.28 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    15.71 ns |   1.959 ns |  0.107 ns |  1.01 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    16.00 ns |   7.332 ns |  0.402 ns |  1.03 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **17.32 ns** |   **5.946 ns** |  **0.326 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    22.67 ns |   9.844 ns |  0.540 ns |  1.31 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    16.74 ns |   1.782 ns |  0.098 ns |  0.97 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    17.31 ns |   2.390 ns |  0.131 ns |  1.00 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **22.10 ns** |   **4.249 ns** |  **0.233 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    24.98 ns |   1.220 ns |  0.067 ns |  1.13 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    25.34 ns |   4.612 ns |  0.253 ns |  1.15 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    22.35 ns |   2.785 ns |  0.153 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **33.73 ns** |   **2.513 ns** |  **0.138 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    37.66 ns |   6.329 ns |  0.347 ns |  1.12 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    36.06 ns |  12.475 ns |  0.684 ns |  1.07 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    33.40 ns |   3.770 ns |  0.207 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **40.31 ns** |   **4.446 ns** |  **0.244 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    43.76 ns |   5.293 ns |  0.290 ns |  1.09 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    41.52 ns |   4.978 ns |  0.273 ns |  1.03 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    39.68 ns |   2.334 ns |  0.128 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **281.11 ns** |  **73.187 ns** |  **4.012 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   287.44 ns |  38.281 ns |  2.098 ns |  1.02 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   273.67 ns |  44.481 ns |  2.438 ns |  0.97 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   270.47 ns |  31.577 ns |  1.731 ns |  0.96 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **3,404.03 ns** | **604.617 ns** | **33.141 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 3,252.69 ns | 983.831 ns | 53.927 ns |  0.96 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 3,415.59 ns | 695.115 ns | 38.102 ns |  1.00 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,382.14 ns | 272.800 ns | 14.953 ns |  0.99 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **222.8 ns** |    **29.01 ns** |  **1.59 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    104.1 ns |    41.76 ns |  2.29 ns |  0.47 |    0.01 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **1,448.8 ns** |   **759.22 ns** | **41.62 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  1,859.2 ns |   186.61 ns | 10.23 ns |  1.28 |    0.03 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **512**    |  **8,171.6 ns** |   **322.57 ns** | **17.68 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 19,821.0 ns | 1,560.89 ns | 85.56 ns |  2.43 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **3,834.9 ns** | **1,267.01 ns** |  **69.45 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  4,231.9 ns | 4,473.12 ns | 245.19 ns |  1.10 |    0.06 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      |  6,010.9 ns | 2,456.88 ns | 134.67 ns |  1.57 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **4,806.7 ns** | **1,890.28 ns** | **103.61 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  4,867.5 ns | 2,418.98 ns | 132.59 ns |  1.01 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     |  7,875.9 ns | 3,625.57 ns | 198.73 ns |  1.64 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     |  **6,584.6 ns** | **1,891.61 ns** | **103.69 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     |  6,209.9 ns |   715.71 ns |  39.23 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 10,514.2 ns | 5,726.73 ns | 313.90 ns |  1.60 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **2,714.6 ns** |   **353.22 ns** |  **19.36 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  3,493.2 ns |   192.78 ns |  10.57 ns |  1.29 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  4,362.5 ns |     7.12 ns |   0.39 ns |  1.61 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **3,887.4 ns** |   **867.91 ns** |  **47.57 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  5,417.7 ns | 2,022.46 ns | 110.86 ns |  1.39 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     |  6,293.1 ns | 1,024.44 ns |  56.15 ns |  1.62 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **5,238.0 ns** | **2,937.69 ns** | **161.02 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  5,569.1 ns |   586.18 ns |  32.13 ns |  1.06 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     |  8,861.7 ns | 3,598.23 ns | 197.23 ns |  1.69 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **2,038.5 ns** |   **624.43 ns** |  **34.23 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  2,749.7 ns |   318.91 ns |  17.48 ns |  1.35 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  3,937.8 ns |   610.99 ns |  33.49 ns |  1.93 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **3,067.9 ns** | **1,099.25 ns** |  **60.25 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  3,848.1 ns |   155.88 ns |   8.54 ns |  1.25 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  5,387.8 ns |   791.94 ns |  43.41 ns |  1.76 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **4,498.0 ns** |   **825.40 ns** |  **45.24 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  5,557.8 ns | 3,516.75 ns | 192.76 ns |  1.24 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     |  7,902.9 ns |   468.43 ns |  25.68 ns |  1.76 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **1,330.9 ns** |   **440.72 ns** |  **24.16 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  1,313.1 ns |   239.90 ns |  13.15 ns |  0.99 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  3,000.0 ns |    86.29 ns |   4.73 ns |  2.25 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **2,409.6 ns** |   **543.89 ns** |  **29.81 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  1,866.9 ns |   498.45 ns |  27.32 ns |  0.77 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  4,807.0 ns |   339.58 ns |  18.61 ns |  2.00 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **3,757.4 ns** |   **207.27 ns** |  **11.36 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  2,543.7 ns |   168.70 ns |   9.25 ns |  0.68 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     |  6,924.5 ns | 2,135.21 ns | 117.04 ns |  1.84 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **1,972.9 ns** | **1,036.48 ns** |  **56.81 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  2,259.3 ns | 1,909.09 ns | 104.64 ns |  1.15 |    0.05 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  3,753.6 ns | 1,513.50 ns |  82.96 ns |  1.90 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **3,008.1 ns** |   **901.85 ns** |  **49.43 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  2,749.9 ns |   347.27 ns |  19.04 ns |  0.91 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     |  5,490.8 ns | 1,087.43 ns |  59.61 ns |  1.83 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **4,427.7 ns** | **1,410.16 ns** |  **77.30 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  4,347.0 ns |   940.75 ns |  51.57 ns |  0.98 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     |  7,789.2 ns | 4,392.67 ns | 240.78 ns |  1.76 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,018.7 ns** |    **77.20 ns** |   **4.23 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    339.0 ns |    59.01 ns |   3.23 ns |  0.33 |    0.00 | 0.0024 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  2,502.1 ns |   112.57 ns |   6.17 ns |  2.46 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **1,809.1 ns** |   **566.13 ns** |  **31.03 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |    798.3 ns |   727.12 ns |  39.86 ns |  0.44 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  4,377.2 ns | 3,692.21 ns | 202.38 ns |  2.42 |    0.10 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **3,305.3 ns** |   **636.87 ns** |  **34.91 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  1,503.2 ns |   434.96 ns |  23.84 ns |  0.45 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     |  6,542.8 ns | 2,699.56 ns | 147.97 ns |  1.98 |    0.04 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **3,107.6 μs** |     **473.0 μs** |     **25.93 μs** |  **1.00** |    **0.01** | **257.8125** | **250.0000** | **246.0938** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  21,532.8 μs |  22,073.8 μs |  1,209.94 μs |  6.93 |    0.34 |        - |        - |        - |   10.33 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  50,237.0 μs |   3,178.2 μs |    174.21 μs | 16.17 |    0.13 |        - |        - |        - |   41.85 KB |       0.028 |
|                  |             |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **465.9 μs** |     **133.6 μs** |      **7.32 μs** |  **1.00** |    **0.02** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   2,772.1 μs |     163.2 μs |      8.95 μs |  5.95 |    0.08 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   1,555.2 μs |     422.6 μs |     23.17 μs |  3.34 |    0.06 |   1.9531 |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **14,024.2 μs** |     **508.6 μs** |     **27.88 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.01 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 242,164.9 μs | 228,570.1 μs | 12,528.70 μs | 17.27 |    0.77 |        - |        - |        - |   15.37 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 382,982.1 μs |  41,865.2 μs |  2,294.77 μs | 27.31 |    0.15 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **2.752 ms** |  **0.5864 ms** | **0.0321 ms** |  **1.00** |    **0.01** | **253.9063** | **246.0938** | **246.0938** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |   8.866 ms |  2.8405 ms | 0.1557 ms |  3.22 |    0.06 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **14.998 ms** |  **5.8022 ms** | **0.3180 ms** |  **1.00** |    **0.03** |  **46.8750** |  **46.8750** |  **46.8750** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  67.311 ms | 15.8864 ms | 0.8708 ms |  4.49 |    0.10 |        - |        - |        - |    97.9 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **26.774 ms** | **25.3461 ms** | **1.3893 ms** |  **1.00** |    **0.06** | **343.7500** | **343.7500** | **343.7500** | **3947.64 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 113.075 ms |  1.8310 ms | 0.1004 ms |  4.23 |    0.19 |        - |        - |        - |   36.34 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **65.58 ns** |     **7.179 ns** |   **0.394 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |    32.14 ns |    14.358 ns |   0.787 ns |  0.49 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    65.72 ns |    22.254 ns |   1.220 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 8    |    52.81 ns |     3.747 ns |   0.205 ns |  0.81 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **122.08 ns** |    **13.354 ns** |   **0.732 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    36.55 ns |     0.714 ns |   0.039 ns |  0.30 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   131.19 ns |   131.385 ns |   7.202 ns |  1.07 |    0.05 |         - |          NA |
| Kernel_Cjk | 12   |    60.11 ns |     9.696 ns |   0.531 ns |  0.49 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **14**   |   **150.23 ns** |    **68.897 ns** |   **3.776 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 14   |    38.83 ns |     5.122 ns |   0.281 ns |  0.26 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |   147.76 ns |    30.834 ns |   1.690 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 14   |    63.47 ns |    16.207 ns |   0.888 ns |  0.42 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **422.82 ns** |   **409.426 ns** |  **22.442 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 16   |    41.08 ns |    12.246 ns |   0.671 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   452.22 ns |   170.263 ns |   9.333 ns |  1.07 |    0.05 |         - |          NA |
| Kernel_Cjk | 16   |    74.84 ns |    71.500 ns |   3.919 ns |  0.18 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **18**   |   **445.09 ns** |   **235.150 ns** |  **12.889 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 18   |    44.78 ns |    12.106 ns |   0.664 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |   444.33 ns |    98.676 ns |   5.409 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 18   |    75.49 ns |    42.028 ns |   2.304 ns |  0.17 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **20**   |   **508.52 ns** |    **84.661 ns** |   **4.641 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |    49.06 ns |    10.700 ns |   0.587 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   509.68 ns |    49.538 ns |   2.715 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    75.82 ns |     8.854 ns |   0.485 ns |  0.15 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **818.68 ns** |   **738.510 ns** |  **40.480 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 24   |    54.02 ns |     5.549 ns |   0.304 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   666.20 ns |    64.090 ns |   3.513 ns |  0.82 |    0.03 |         - |          NA |
| Kernel_Cjk | 24   |    82.99 ns |    50.587 ns |   2.773 ns |  0.10 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   |   **993.69 ns** |   **528.495 ns** |  **28.969 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 32   |    66.15 ns |    23.099 ns |   1.266 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,195.02 ns | 3,127.504 ns | 171.429 ns |  1.20 |    0.15 |         - |          NA |
| Kernel_Cjk | 32   |    98.40 ns |     8.702 ns |   0.477 ns |  0.10 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **1,828.78 ns** |   **195.411 ns** |  **10.711 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    77.67 ns |     9.798 ns |   0.537 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,192.50 ns | 8,103.690 ns | 444.191 ns |  1.20 |    0.21 |         - |          NA |
| Kernel_Cjk | 48   |   141.56 ns |    47.919 ns |   2.627 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **3,197.94 ns** |   **553.219 ns** |  **30.324 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    96.76 ns |    58.558 ns |   3.210 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,620.23 ns | 1,555.346 ns |  85.254 ns |  1.44 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |   162.51 ns |    21.832 ns |   1.197 ns |  0.05 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **6,348.70 ns** | **3,271.131 ns** | **179.302 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |   238.96 ns |    71.020 ns |   3.893 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 6,095.14 ns | 3,892.500 ns | 213.361 ns |  0.96 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |   566.02 ns |    78.494 ns |   4.303 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **14.21 ns** |     **1.499 ns** |   **0.082 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     14.26 ns |     4.389 ns |   0.241 ns |  1.00 |    0.02 |         - |          NA |
| Distance_CodePoint         | 8      |     92.63 ns |    18.531 ns |   1.016 ns |  6.52 |    0.07 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     14.82 ns |     7.830 ns |   0.429 ns |  1.04 |    0.03 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **205.11 ns** |    **17.453 ns** |   **0.957 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    247.74 ns |   125.864 ns |   6.899 ns |  1.21 |    0.03 |         - |          NA |
| Distance_CodePoint         | 64     |    437.65 ns |    77.656 ns |   4.257 ns |  2.13 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    202.60 ns |    12.210 ns |   0.669 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **554.61 ns** |    **12.967 ns** |   **0.711 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,103.84 ns |   289.301 ns |  15.858 ns |  1.99 |    0.02 |         - |          NA |
| Distance_CodePoint         | 128    |    988.54 ns |   393.825 ns |  21.587 ns |  1.78 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    573.27 ns |   566.928 ns |  31.075 ns |  1.03 |    0.05 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    |  **7,264.01 ns** | **4,520.271 ns** | **247.771 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 10,729.35 ns | 1,841.925 ns | 100.962 ns |  1.48 |    0.05 |         - |          NA |
| Distance_CodePoint         | 512    |  8,395.89 ns |   475.621 ns |  26.070 ns |  1.16 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  7,360.20 ns |   624.470 ns |  34.229 ns |  1.01 |    0.03 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **220.8 ns** |     **90.93 ns** |     **4.98 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     149.7 ns |     41.63 ns |     2.28 ns |  0.68 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **221.3 ns** |     **28.28 ns** |     **1.55 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     155.2 ns |     60.65 ns |     3.32 ns |  0.70 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **278.6 ns** |     **22.49 ns** |     **1.23 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     209.9 ns |     16.69 ns |     0.91 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **278.4 ns** |    **135.61 ns** |     **7.43 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     207.8 ns |     20.52 ns |     1.12 ns |  0.75 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **343.3 ns** |    **116.35 ns** |     **6.38 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     270.6 ns |     22.49 ns |     1.23 ns |  0.79 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **332.2 ns** |     **59.66 ns** |     **3.27 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     275.1 ns |    133.42 ns |     7.31 ns |  0.83 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **385.5 ns** |     **73.22 ns** |     **4.01 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |     748.0 ns |     31.14 ns |     1.71 ns |  1.94 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **398.4 ns** |     **29.25 ns** |     **1.60 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |     759.5 ns |    212.82 ns |    11.67 ns |  1.91 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,293.5 ns** |    **112.38 ns** |     **6.16 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   3,757.7 ns |  5,249.41 ns |   287.74 ns |  2.91 |    0.19 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,276.8 ns** |    **375.10 ns** |    **20.56 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   3,682.7 ns |     74.07 ns |     4.06 ns |  2.88 |    0.04 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **10,035.9 ns** |  **3,905.98 ns** |   **214.10 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  39,099.5 ns | 16,613.89 ns |   910.66 ns |  3.90 |    0.11 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **277,282.4 ns** | **45,258.31 ns** | **2,480.76 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  38,738.3 ns |  1,143.98 ns |    62.71 ns |  0.14 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **14.58 ns** |       **2.231 ns** |      **0.122 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      53.46 ns |       6.315 ns |      0.346 ns |  3.67 |    0.03 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      48.74 ns |       7.676 ns |      0.421 ns |  3.34 |    0.03 |      - |         - |          NA |
| F23_StringSimilarity | 8      |      95.11 ns |      76.245 ns |      4.179 ns |  6.52 |    0.25 | 0.0076 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **206.94 ns** |      **23.940 ns** |      **1.312 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   2,662.66 ns |   1,720.910 ns |     94.329 ns | 12.87 |    0.40 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |     790.17 ns |     539.433 ns |     29.568 ns |  3.82 |    0.13 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   6,243.63 ns |   5,240.698 ns |    287.260 ns | 30.17 |    1.21 | 0.0305 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |   **7,496.03 ns** |   **3,668.697 ns** |    **201.094 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 266,500.85 ns | 240,685.038 ns | 13,192.759 ns | 35.57 |    1.73 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  21,160.52 ns |  14,899.741 ns |    816.705 ns |  2.82 |    0.11 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 443,423.69 ns | 347,476.274 ns | 19,046.347 ns | 59.18 |    2.59 |      - |    4160 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **259.4 μs** |  **88.09 μs** |  **4.83 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 267.1 μs |  73.35 μs |  4.02 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 293.0 μs | 210.35 μs | 11.53 μs |  9.2773 |  2.9297 | 153.98 KB |
| **F1Macro**           | **5000**   | **333.9 μs** | **167.09 μs** |  **9.16 μs** | **20.5078** |  **6.3477** | **337.24 KB** |
| PrecisionWeighted | 5000   | 326.1 μs |  56.54 μs |  3.10 μs | 20.5078 |  6.3477 | 337.24 KB |
| ReportRows        | 5000   | 455.2 μs |  90.48 μs |  4.96 μs | 46.8750 | 17.0898 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error          | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |      **82.88 μs** |      **66.234 μs** |     **3.631 μs** |   **1.00** |    **0.05** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  64,592.67 μs |  26,722.409 μs | 1,464.745 μs | 780.40 |   33.66 | 875.0000 | 875.0000 | 875.0000 | 31414.08 KB |   12,805.74 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **1,718.25 μs** |     **213.720 μs** |    **11.715 μs** |   **1.00** |    **0.01** |   **1.9531** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 321,713.35 μs | 112,455.647 μs | 6,164.073 μs | 187.24 |    3.30 | 500.0000 | 500.0000 | 500.0000 | 32058.25 KB |      760.73 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **14.63 μs** |       **3.065 μs** |     **0.168 μs** |   **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     504.53 μs |     149.314 μs |     8.184 μs |  34.49 |    0.59 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Gen1   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **2,456.1 ns** |       **560.72 ns** |      **30.73 ns** | **0.0229** |      **-** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     4,146.1 ns |       334.83 ns |      18.35 ns | 0.0229 |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       121.1 ns |        37.72 ns |       2.07 ns |      - |      - |         - |
| F1Macro        | 1000    | 2       |     2,500.4 ns |       162.07 ns |       8.88 ns | 0.0381 |      - |     656 B |
| Report         | 1000    | 2       |     4,010.7 ns |     3,404.86 ns |     186.63 ns | 0.3204 |      - |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **2,167.9 ns** |     **1,955.12 ns** |     **107.17 ns** | **0.0763** |      **-** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     4,293.2 ns |       352.03 ns |      19.30 ns | 0.0763 |      - |    1336 B |
| AccuracyScore  | 1000    | 10      |       122.3 ns |        38.64 ns |       2.12 ns |      - |      - |         - |
| F1Macro        | 1000    | 10      |     2,436.4 ns |       208.89 ns |      11.45 ns | 0.1106 |      - |    1880 B |
| Report         | 1000    | 10      |     5,709.0 ns |       364.22 ns |      19.96 ns | 0.7401 | 0.0076 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **224,106.0 ns** |    **50,028.63 ns** |   **2,742.24 ns** |      **-** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   383,500.3 ns |    20,437.89 ns |   1,120.27 ns |      - |      - |     400 B |
| AccuracyScore  | 100000  | 2       |     6,215.4 ns |     1,826.42 ns |     100.11 ns |      - |      - |         - |
| F1Macro        | 100000  | 2       |   234,633.3 ns |   143,928.12 ns |   7,889.19 ns |      - |      - |     656 B |
| Report         | 100000  | 2       |   238,892.0 ns |   139,056.79 ns |   7,622.17 ns | 0.2441 |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **179,963.3 ns** |    **45,047.62 ns** |   **2,469.21 ns** |      **-** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   528,086.2 ns |   124,748.34 ns |   6,837.88 ns |      - |      - |    1336 B |
| AccuracyScore  | 100000  | 10      |    11,970.9 ns |     1,433.84 ns |      78.59 ns |      - |      - |         - |
| F1Macro        | 100000  | 10      |   192,243.2 ns |    12,746.94 ns |     698.70 ns |      - |      - |    1880 B |
| Report         | 100000  | 10      |   218,760.1 ns |   299,613.63 ns |  16,422.83 ns | 0.7324 |      - |   12832 B |
| **Matrix**         | **1000000** | **2**       | **2,463,373.9 ns** | **1,925,052.40 ns** | **105,518.62 ns** |      **-** |      **-** |     **402 B** |
| MatrixWeighted | 1000000 | 2       | 4,725,675.9 ns |   509,545.50 ns |  27,929.91 ns |      - |      - |     406 B |
| AccuracyScore  | 1000000 | 2       |    81,202.4 ns |    15,849.63 ns |     868.77 ns |      - |      - |         - |
| F1Macro        | 1000000 | 2       | 2,320,117.8 ns |   883,884.19 ns |  48,448.67 ns |      - |      - |     659 B |
| Report         | 1000000 | 2       | 2,316,604.8 ns |    71,622.54 ns |   3,925.87 ns |      - |      - |    5507 B |
| **Matrix**         | **1000000** | **10**      | **1,916,583.0 ns** |   **166,104.68 ns** |   **9,104.76 ns** |      **-** |      **-** |    **1337 B** |
| MatrixWeighted | 1000000 | 10      | 5,984,594.3 ns | 2,165,275.84 ns | 118,686.08 ns |      - |      - |    1342 B |
| AccuracyScore  | 1000000 | 10      |    79,272.4 ns |    21,520.48 ns |   1,179.61 ns |      - |      - |         - |
| F1Macro        | 1000000 | 10      | 1,961,063.3 ns | 2,296,667.94 ns | 125,888.12 ns |      - |      - |    1883 B |
| Report         | 1000000 | 10      | 1,867,309.9 ns |   804,690.14 ns |  44,107.78 ns |      - |      - |   12873 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean           | Error          | StdDev         | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |---------------:|---------------:|---------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **5,707.400 μs** |     **659.200 μs** |     **36.1330 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1280 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  26,647.593 μs |   1,337.888 μs |     73.3342 μs |     4.67 |    0.03 | 593.7500 | 593.7500 | 593.7500 |  5089290 B |    3,976.01 |
|          |         |               |                |                |                |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |       **6.498 μs** |       **2.993 μs** |      **0.1640 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  25,768.164 μs |   5,614.547 μs |    307.7522 μs | 3,967.06 |   95.55 | 562.5000 | 562.5000 | 562.5000 |  5089542 B |          NA |
|          |         |               |                |                |                |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **123,614.137 μs** | **219,979.397 μs** | **12,057.8131 μs** |     **1.01** |    **0.12** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 152,950.833 μs |  29,256.323 μs |  1,603.6378 μs |     1.24 |    0.10 | 250.0000 | 250.0000 | 250.0000 | 23229742 B |          NA |
|          |         |               |                |                |                |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |      **83.781 μs** |      **33.669 μs** |      **1.8455 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 152,477.399 μs |   9,735.233 μs |    533.6210 μs | 1,820.53 |   34.78 |        - |        - |        - | 23228008 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **41.97 ns** |     **6.255 ns** |  **0.343 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 4    |    44.23 ns |     6.622 ns |  0.363 ns |  1.05 |    0.01 |         - |          NA |
| Dp_Cjk     | 4    |    46.38 ns |    26.399 ns |  1.447 ns |  1.11 |    0.03 |         - |          NA |
| Kernel_Cjk | 4    |    44.57 ns |     0.916 ns |  0.050 ns |  1.06 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **6**    |    **60.20 ns** |     **8.287 ns** |  **0.454 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 6    |    42.10 ns |     2.986 ns |  0.164 ns |  0.70 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    58.99 ns |    13.465 ns |  0.738 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    70.04 ns |    58.675 ns |  3.216 ns |  1.16 |    0.05 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **8**    |    **89.02 ns** |    **97.402 ns** |  **5.339 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 8    |    50.86 ns |    14.803 ns |  0.811 ns |  0.57 |    0.03 |         - |          NA |
| Dp_Cjk     | 8    |    86.86 ns |    73.951 ns |  4.054 ns |  0.98 |    0.06 |         - |          NA |
| Kernel_Cjk | 8    |    91.55 ns |    11.630 ns |  0.637 ns |  1.03 |    0.05 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **10**   |   **108.82 ns** |    **14.474 ns** |  **0.793 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |    58.43 ns |    57.473 ns |  3.150 ns |  0.54 |    0.03 |         - |          NA |
| Dp_Cjk     | 10   |   112.76 ns |    51.826 ns |  2.841 ns |  1.04 |    0.02 |         - |          NA |
| Kernel_Cjk | 10   |    83.97 ns |     8.095 ns |  0.444 ns |  0.77 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **12**   |   **143.72 ns** |    **30.202 ns** |  **1.655 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    62.51 ns |     0.517 ns |  0.028 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   144.87 ns |    23.936 ns |  1.312 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    88.12 ns |     9.556 ns |  0.524 ns |  0.61 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **16**   |   **281.27 ns** |    **24.823 ns** |  **1.361 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    76.22 ns |     8.388 ns |  0.460 ns |  0.27 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   233.35 ns |    47.888 ns |  2.625 ns |  0.83 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |   101.51 ns |    11.839 ns |  0.649 ns |  0.36 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **24**   |   **607.79 ns** |   **164.035 ns** |  **8.991 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |   103.09 ns |    22.693 ns |  1.244 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   477.77 ns |   139.304 ns |  7.636 ns |  0.79 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |   128.87 ns |    11.103 ns |  0.609 ns |  0.21 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **32**   | **1,077.46 ns** |    **86.621 ns** |  **4.748 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |   129.94 ns |    56.813 ns |  3.114 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |   852.97 ns |   121.280 ns |  6.648 ns |  0.79 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |   165.98 ns |   125.124 ns |  6.858 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **48**   | **2,462.92 ns** |   **715.752 ns** | **39.233 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |   173.99 ns |    51.955 ns |  2.848 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 1,898.76 ns |   601.060 ns | 32.946 ns |  0.77 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |   223.71 ns |    10.038 ns |  0.550 ns |  0.09 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **64**   | **4,797.64 ns** | **1,763.090 ns** | **96.641 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 64   |   236.55 ns |    54.547 ns |  2.990 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 3,289.19 ns |   354.335 ns | 19.422 ns |  0.69 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |   278.95 ns |    11.289 ns |  0.619 ns |  0.06 |    0.00 |         - |          NA |
|            |      |             |              |           |       |         |           |             |
| **Dp**         | **96**   | **7,644.10 ns** |   **952.015 ns** | **52.183 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   473.50 ns |    24.928 ns |  1.366 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 7,463.92 ns | 1,633.945 ns | 89.562 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |   915.28 ns |   153.322 ns |  8.404 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **145.14 μs** |    **26.285 μs** |  **1.441 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   152.63 μs |    58.352 μs |  3.198 μs |  1.05 |    0.02 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    32.45 μs |     1.337 μs |  0.073 μs |  0.22 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   157.13 μs |    38.086 μs |  2.088 μs |  1.08 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **144.04 μs** |    **10.268 μs** |  **0.563 μs** |  **1.00** |    **0.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   166.23 μs |    21.542 μs |  1.181 μs |  1.15 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    32.74 μs |     2.675 μs |  0.147 μs |  0.23 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   153.54 μs |     6.438 μs |  0.353 μs |  1.07 |    0.00 |      - |     800 B |        1.00 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,507.22 μs** | **1,550.817 μs** | **85.006 μs** |  **1.00** |    **0.07** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 1,532.77 μs |   729.288 μs | 39.975 μs |  1.02 |    0.05 |      - |    7785 B |        9.72 |
| SparseFit             | 100000   | 10         |   826.76 μs |    85.131 μs |  4.666 μs |  0.55 |    0.03 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,563.99 μs |   165.442 μs |  9.068 μs |  1.04 |    0.05 |      - |     801 B |        1.00 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,437.51 μs** |    **24.036 μs** |  **1.317 μs** |  **1.00** |    **0.00** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 1,519.60 μs |   991.917 μs | 54.370 μs |  1.06 |    0.03 | 3.9063 |   77625 B |       96.91 |
| SparseFit             | 100000   | 100        |   806.75 μs |   167.921 μs |  9.204 μs |  0.56 |    0.01 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,570.44 μs |   439.079 μs | 24.067 μs |  1.09 |    0.01 |      - |     801 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean       | Error       | StdDev     | Allocated |
|------------ |------------- |-----------:|------------:|-----------:|----------:|
| **Embedded**    | **65**           |   **6.426 μs** |   **4.5635 μs** |  **0.2501 μs** |         **-** |
| EqualLength | 65           |   1.622 μs |   0.0895 μs |  0.0049 μs |         - |
| **Embedded**    | **128**          |  **23.880 μs** |   **0.8605 μs** |  **0.0472 μs** |         **-** |
| EqualLength | 128          |   3.115 μs |   0.3842 μs |  0.0211 μs |         - |
| **Embedded**    | **512**          | **885.814 μs** | **600.8427 μs** | **32.9342 μs** |         **-** |
| EqualLength | 512          |  26.672 μs |   2.2769 μs |  0.1248 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean        | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  2,751.5 μs |    182.96 μs |    10.03 μs | 117.1875 | 113.2813 |  35.1563 |  3711.63 KB |
| TokenizerJsonWordPiece |  6,716.5 μs |  4,585.22 μs |   251.33 μs | 187.5000 | 171.8750 |  46.8750 |   5852.3 KB |
| TokenizerJsonUnigram   |  6,790.8 μs |  1,678.94 μs |    92.03 μs | 101.5625 |  93.7500 |  31.2500 |  4748.89 KB |
| SpieceModel            |  3,670.5 μs |  2,495.34 μs |   136.78 μs | 144.5313 | 140.6250 |  46.8750 |  4807.75 KB |
| TfidfSave              |    982.9 μs |     44.87 μs |     2.46 μs |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  3,040.0 μs |  1,617.05 μs |    88.64 μs |  85.9375 |  78.1250 |  23.4375 |  2921.39 KB |
| EmbeddingIndexSave     |  2,314.3 μs |  1,602.00 μs |    87.81 μs | 203.1250 | 197.2656 | 197.2656 | 20349.83 KB |
| EmbeddingIndexLoad     |  5,606.9 μs |  1,310.09 μs |    71.81 μs | 179.6875 | 148.4375 | 117.1875 | 16094.44 KB |
| EmbeddingIndexSaveFile | 72,623.9 μs | 13,857.30 μs |   759.57 μs |        - |        - |        - |   323.53 KB |
| EmbeddingIndexLoadGzip | 51,971.1 μs | 56,196.22 μs | 3,080.30 μs |        - |        - |        - | 16095.17 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **5,131.72 μs** | **1,058.197 μs** |  **58.003 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 10,173.45 μs | 4,376.224 μs | 239.876 μs |  1.98 |    0.04 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |     **86.33 μs** |    **15.948 μs** |   **0.874 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    105.40 μs |     3.257 μs |   0.179 μs |  1.22 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **1,716.31 μs** |   **271.205 μs** |  **14.866 μs** |  **1.00** |    **0.01** |   **1.9531** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  1,640.40 μs |   342.043 μs |  18.749 μs |  0.96 |    0.01 |   1.9531 |        - |        - |  40.06 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **13.97 μs** |     **2.606 μs** |   **0.143 μs** |  **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     15.53 μs |     1.535 μs |   0.084 μs |  1.11 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **29.63 μs** |     **3.951 μs** |  **0.217 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    28.51 μs |     0.531 μs |  0.029 μs |        - |        - |        - |         - |
| R2Score  | 100000  |    73.73 μs |     0.527 μs |  0.029 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   289.34 μs |    62.673 μs |  3.435 μs | 199.7070 | 199.7070 | 199.7070 |  800186 B |
| **Mse**      | **1000000** |   **301.59 μs** |    **76.367 μs** |  **4.186 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   299.76 μs |    34.515 μs |  1.892 μs |        - |        - |        - |         - |
| R2Score  | 1000000 |   752.61 μs |    32.078 μs |  1.758 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 4,005.03 μs | 1,387.036 μs | 76.028 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|------------:|-----------:|------:|--------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **40.018 ms** |  **21.4617 ms** |  **1.1764 ms** |  **1.00** |    **0.04** |  **428.5714** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   6.743 ms |   3.0061 ms |  0.1648 ms |  0.17 |    0.01 |  171.8750 |  164.0625 |  85.9375 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   5.389 ms |   2.0494 ms |  0.1123 ms |  0.13 |    0.00 |   15.6250 |         - |        - |    304.73 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   4.029 ms |   0.6487 ms |  0.0356 ms |  0.10 |    0.00 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   7.200 ms |   1.8343 ms |  0.1005 ms |  0.18 |    0.01 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **40.720 ms** |  **27.0491 ms** |  **1.4827 ms** |  **1.00** |    **0.04** |  **461.5385** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |   9.131 ms |   1.7796 ms |  0.0975 ms |  0.22 |    0.01 |  328.1250 |  312.5000 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   6.751 ms |   3.8074 ms |  0.2087 ms |  0.17 |    0.01 |   23.4375 |         - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   3.980 ms |   0.6570 ms |  0.0360 ms |  0.10 |    0.00 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   7.182 ms |   1.3783 ms |  0.0755 ms |  0.18 |    0.01 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **664.929 ms** | **324.2083 ms** | **17.7710 ms** |  **1.00** |    **0.03** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  30.384 ms |  30.4950 ms |  1.6715 ms |  0.05 |    0.00 |  937.5000 |  937.5000 | 500.0000 |  10334.83 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  21.611 ms |   4.6818 ms |  0.2566 ms |  0.03 |    0.00 |   62.5000 |         - |        - |   1218.81 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  15.728 ms |   1.3011 ms |  0.0713 ms |  0.02 |    0.00 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  30.100 ms |   4.2942 ms |  0.2354 ms |  0.05 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |       0.014 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **675.849 ms** | **131.8438 ms** |  **7.2268 ms** |  **1.00** |    **0.01** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  42.332 ms |  22.4793 ms |  1.2322 ms |  0.06 |    0.00 | 1636.3636 | 1636.3636 | 909.0909 |  18665.71 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  26.381 ms |   9.2133 ms |  0.5050 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.81 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  16.282 ms |   0.8344 ms |  0.0457 ms |  0.02 |    0.00 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  29.671 ms |   5.0452 ms |  0.2765 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean           | Error          | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|---------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |       614.1 ns |       462.3 ns |      25.34 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |       793.5 ns |       684.3 ns |      37.51 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |       658.2 ns |       479.1 ns |      26.26 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |       216.7 ns |       110.1 ns |       6.03 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 7,363,675.7 ns | 3,950,483.4 ns | 216,539.33 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1,453.7 μs** |   **260.69 μs** |  **14.29 μs** |  **1.00** |    **0.01** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |   978.0 μs |    93.99 μs |   5.15 μs |  0.67 |    0.01 |  24.4141 |   7.8125 |        - |  407.59 KB |        0.47 |
| Hashing              | 200       | 1,475.2 μs |   105.56 μs |   5.79 μs |  1.01 |    0.01 |  72.2656 |  72.2656 |  72.2656 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1,185.0 μs | 1,317.96 μs |  72.24 μs |  0.82 |    0.04 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |            |             |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **6,885.6 μs** |   **378.28 μs** |  **20.73 μs** |  **1.00** |    **0.00** | **867.1875** | **867.1875** | **867.1875** | **3436.55 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 5,201.4 μs |   843.11 μs |  46.21 μs |  0.76 |    0.01 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 7,458.2 μs | 2,907.32 μs | 159.36 μs |  1.08 |    0.02 | 515.6250 | 500.0000 | 500.0000 | 2349.63 KB |        0.68 |
| HashingWithStopWords | 1000      | 6,042.8 μs |   223.45 μs |  12.25 μs |  0.88 |    0.00 | 265.6250 | 265.6250 | 265.6250 | 1139.45 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean            | Error            | StdDev        | Gen0     | Gen1     | Gen2    | Allocated  |
|--------------------------- |-------------- |----------------:|-----------------:|--------------:|---------:|---------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **22,357,935.8 ns** | **14,453,515.25 ns** | **792,245.95 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.56 KB** |
| Construct                  | [^ ]+         |      1,300.8 ns |        110.06 ns |       6.03 ns |   0.3319 |   0.0019 |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      2,070.0 ns |        299.02 ns |      16.39 ns |   0.4539 |   0.0038 |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **18,614,126.0 ns** |  **5,232,545.77 ns** | **286,813.49 ns** |        **-** |        **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1,234.8 ns |        204.44 ns |      11.21 ns |   0.3300 |   0.0019 |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      1,802.0 ns |        349.23 ns |      19.14 ns |   0.4387 |   0.0038 |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **24,636,277.7 ns** |  **2,347,350.70 ns** | **128,666.21 ns** |  **62.5000** |  **31.2500** |       **-** | **1555.96 KB** |
| Construct                  | [A-Za-z]\\w+  |      2,254.9 ns |      1,993.39 ns |     109.26 ns |   0.4158 |   0.0038 |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      2,946.9 ns |        255.42 ns |      14.00 ns |   0.5341 |        - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **13,663,621.8 ns** |  **2,219,930.76 ns** | **121,681.90 ns** | **156.2500** | **140.6250** | **93.7500** | **9743.56 KB** |
| Construct                  | \\b\\w\\w+\\b |        706.3 ns |         72.83 ns |       3.99 ns |   0.1135 |        - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1,269.3 ns |        289.83 ns |      15.89 ns |   0.2346 |        - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **22,071,303.1 ns** | **16,697,853.32 ns** | **915,265.69 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.42 KB** |
| Construct                  | \\S+          |      1,390.1 ns |        100.30 ns |       5.50 ns |   0.3223 |   0.0019 |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      2,178.1 ns |        267.39 ns |      14.66 ns |   0.4425 |   0.0038 |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **10.83 ms** |   **2.160 ms** | **0.118 ms** |  **1.00** |    **0.01** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  32.28 ms |  13.983 ms | 0.766 ms |  2.98 |    0.07 |  218.7500 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **29.83 ms** |  **16.100 ms** | **0.883 ms** |  **1.00** |    **0.04** |  **312.5000** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  35.11 ms |   8.646 ms | 0.474 ms |  1.18 |    0.03 |  142.8571 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **54.47 ms** |  **39.289 ms** | **2.154 ms** |  **1.00** |    **0.05** | **1700.0000** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 168.66 ms | 117.917 ms | 6.463 ms |  3.10 |    0.15 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean     | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |---------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  | **34.79 ns** |  **0.990 ns** | **0.054 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  | 24.66 ns |  3.933 ns | 0.216 ns |  0.71 |         - |          NA |
|        |      |          |           |          |       |           |             |
| **Dot**    | **768**  | **69.72 ns** |  **5.657 ns** | **0.310 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  | 48.01 ns |  1.305 ns | 0.072 ns |  0.69 |         - |          NA |
|        |      |          |           |          |       |           |             |
| **Dot**    | **1024** | **93.42 ns** | **10.337 ns** | **0.567 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 65.57 ns |  6.102 ns | 0.334 ns |  0.70 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |-----------:|------------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |   **117.0 μs** |    **12.43 μs** |   **0.68 μs** |  **1.00** |    **0.01** |   **12.4512** |    **2.1973** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |   123.2 μs |     7.13 μs |   0.39 μs |  1.05 |    0.01 |   14.1602 |    3.4180 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |   415.0 μs |    52.31 μs |   2.87 μs |  3.55 |    0.03 |   35.6445 |   13.1836 |         - |   586.98 KB |        2.86 |
| CountCharWordBoundary | 200       | 1,437.8 μs |   413.12 μs |  22.64 μs | 12.29 |    0.18 |  382.8125 |  382.8125 |  382.8125 |  1676.52 KB |        8.16 |
| Hashing               | 200       |   109.3 μs |    20.82 μs |   1.14 μs |  0.93 |    0.01 |    8.3008 |    1.0986 |         - |   136.13 KB |        0.66 |
|                       |           |            |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      | **1,153.6 μs** | **1,181.53 μs** |  **64.76 μs** |  **1.00** |    **0.07** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      | 1,215.6 μs |    62.74 μs |   3.44 μs |  1.06 |    0.05 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      | 2,778.9 μs |   695.51 μs |  38.12 μs |  2.41 |    0.12 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 9,902.9 μs | 6,448.82 μs | 353.48 μs |  8.60 |    0.49 | 1046.8750 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      | 1,124.3 μs |   712.58 μs |  39.06 μs |  0.98 |    0.05 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **2.771 ms** |   **1.111 ms** |  **0.0609 ms** |  **1.00** |    **0.03** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  29.592 ms |  24.488 ms |  1.3423 ms | 10.68 |    0.47 |  5600.0000 |  5600.0000 |  5600.0000 |  28.37 MB |       15.90 |
|          |           |            |            |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **12.314 ms** |   **5.937 ms** |  **0.3254 ms** |  **1.00** |    **0.03** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 246.049 ms | 198.376 ms | 10.8737 ms | 19.99 |    0.89 | 75500.0000 | 75500.0000 | 75500.0000 | 324.33 MB |       40.00 |

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
| kmeans_weighted_10000 | 3.006 | 1.990 | 0.66x | 3.013 | 3.973 | 1.32x |
| kmeans_restarts_10000 | 11.715 | 5.113 | 0.44x | 11.721 | 10.218 | 0.87x |
| kmeans_weighted_100000 | 28.756 | 15.729 | 0.55x | 29.172 | 29.241 | 1.00x |
| kmeans_restarts_100000 | 111.969 | 40.771 | 0.36x | 112.425 | 77.832 | 0.69x |
| dbscan_weighted_5000 | 5.388 | 20.110 | 3.73x | 5.901 | 20.108 | 3.41x |
| dbscan_weighted_20000 | 65.543 | 131.617 | 2.01x | 67.324 | 131.614 | 1.95x |

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
| stratified_weighted_1000 | 0.424 | 63.374 | 149.51x | 0.424 | 63.371 | 149.52x |
| ridge_robust_1000 | 0.464 | 61.662 | 132.90x | 0.464 | 61.657 | 132.90x |
| lasso_1000 | 3.857 | 549.438 | 142.47x | 3.856 | 549.424 | 142.48x |
| ph_test_1000 | 0.072 | 11.855 | 165.71x | 0.072 | 11.854 | 165.71x |
| predict_survival_1000 | 0.018 | 0.818 | 45.29x | 0.018 | 0.818 | 45.29x |
| time_varying_1000 | 41.074 | 163.899 | 3.99x | 41.072 | 163.885 | 3.99x |
| stratified_weighted_10000 | 5.599 | 351.298 | 62.75x | 5.632 | 351.281 | 62.37x |
| ridge_robust_10000 | 6.342 | 576.695 | 90.93x | 6.480 | 576.654 | 88.99x |
| lasso_10000 | 35.232 | 3577.197 | 101.53x | 35.261 | 3576.962 | 101.44x |
| ph_test_10000 | 1.284 | 99.169 | 77.24x | 1.322 | 99.157 | 75.00x |
| predict_survival_10000 | 0.019 | 0.831 | 43.48x | 0.019 | 0.831 | 43.50x |
| time_varying_10000 | 1600.093 | 2553.470 | 1.60x | 1599.977 | 2553.137 | 1.60x |

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
| cv_plus_n1000 | 2.332 | 25.617 | 10.99x | 2.332 | 102.343 | 43.89x |
| cv_minmax_n1000 | 1.926 | 4.113 | 2.14x | 1.926 | 16.406 | 8.52x |
| cv_plus_gamma_n1000 | 2.721 | 26.017 | 9.56x | 2.721 | 103.927 | 38.20x |
| bootstrap_plus_n1000 | 7.975 | 26.352 | 3.30x | 7.975 | 105.210 | 13.19x |
| cv_plus_n10000 | 21.591 | 83.704 | 3.88x | 21.590 | 334.277 | 15.48x |
| cv_minmax_n10000 | 18.881 | 20.418 | 1.08x | 18.881 | 81.421 | 4.31x |
| cv_plus_gamma_n10000 | 34.731 | 90.200 | 2.60x | 34.727 | 359.679 | 10.36x |
| bootstrap_plus_n10000 | 80.244 | 84.573 | 1.05x | 80.238 | 336.979 | 4.20x |

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
| glm_negative_binomial_n1000 | 0.233 | 1.402 | 6.01x | 0.233 | 1.402 | 6.01x |
| glm_gamma_n1000 | 0.278 | 1.752 | 6.30x | 0.278 | 1.752 | 6.30x |
| glm_poisson_exposure_n1000 | 0.236 | 1.636 | 6.93x | 0.236 | 1.636 | 6.93x |
| mnlogit_n1000 | 0.466 | 6.909 | 14.82x | 0.466 | 6.908 | 14.82x |
| glm_negative_binomial_n10000 | 1.997 | 5.655 | 2.83x | 1.999 | 5.654 | 2.83x |
| glm_gamma_n10000 | 2.313 | 6.551 | 2.83x | 2.316 | 6.550 | 2.83x |
| glm_poisson_exposure_n10000 | 2.477 | 6.825 | 2.76x | 2.480 | 6.824 | 2.75x |
| mnlogit_n10000 | 4.749 | 40.377 | 8.50x | 4.751 | 40.375 | 8.50x |
| glm_negative_binomial_n100000 | 21.438 | 73.260 | 3.42x | 21.791 | 292.294 | 13.41x |
| glm_gamma_n100000 | 24.891 | 74.846 | 3.01x | 25.262 | 298.777 | 11.83x |
| glm_poisson_exposure_n100000 | 33.647 | 68.649 | 2.04x | 34.397 | 273.668 | 7.96x |
| mnlogit_n100000 | 48.705 | 405.453 | 8.32x | 49.243 | 933.737 | 18.96x |

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
| 2sls_robust_n1000 | 0.929 | 29.696 | 31.98x | 0.929 | 29.694 | 31.98x |
| liml_robust_n1000 | 1.036 | 29.227 | 28.21x | 1.036 | 29.224 | 28.21x |
| gmm_robust_n1000 | 0.941 | 28.534 | 30.33x | 0.941 | 28.532 | 30.33x |
| 2sls_kernel_n1000 | 5.302 | 30.491 | 5.75x | 5.301 | 30.489 | 5.75x |
| 2sls_clustered_n1000 | 0.904 | 29.879 | 33.04x | 0.904 | 29.878 | 33.04x |
| 2sls_robust_n10000 | 9.739 | 141.437 | 14.52x | 9.980 | 282.675 | 28.32x |
| liml_robust_n10000 | 10.785 | 142.834 | 13.24x | 11.075 | 285.572 | 25.79x |
| gmm_robust_n10000 | 10.050 | 136.327 | 13.57x | 10.301 | 272.567 | 26.46x |
| 2sls_kernel_n10000 | 57.205 | 149.289 | 2.61x | 57.422 | 298.529 | 5.20x |
| 2sls_clustered_n10000 | 9.237 | 151.442 | 16.40x | 9.412 | 302.907 | 32.18x |
| 2sls_robust_n100000 | 119.578 | 1625.014 | 13.59x | 122.323 | 5656.484 | 46.24x |
| liml_robust_n100000 | 132.786 | 1629.172 | 12.27x | 137.145 | 5671.183 | 41.35x |
| gmm_robust_n100000 | 128.352 | 1583.639 | 12.34x | 128.406 | 5493.887 | 42.79x |
| 2sls_kernel_n100000 | 2209.982 | 2192.643 | 0.99x | 2212.913 | 7835.094 | 3.54x |
| 2sls_clustered_n100000 | 109.731 | 1778.596 | 16.21x | 112.994 | 6285.865 | 55.63x |

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
| transform_frobenius_n100 | 1.245 | 0.389 | 0.31x | 1.245 | 0.389 | 0.31x |
| transform_frobenius_n500 | 4.224 | 0.692 | 0.16x | 4.224 | 0.692 | 0.16x |
| transform_frobenius_n2000 | 14.729 | 2.270 | 0.15x | 14.759 | 9.059 | 0.61x |
| transform_kl_n100 | 0.676 | 8.184 | 12.10x | 0.676 | 8.184 | 12.11x |
| transform_kl_n500 | 3.144 | 16.759 | 5.33x | 3.143 | 16.757 | 5.33x |
| transform_kl_n2000 | 12.429 | 62.039 | 4.99x | 12.469 | 247.590 | 19.86x |

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
| ols_summary_n1000 | 0.027 | 1.160 | 42.89x | 0.027 | 1.160 | 42.88x |
| ols_hac_n1000 | 0.096 | 1.419 | 14.75x | 0.096 | 1.418 | 14.75x |
| ols_cluster_n1000 | 0.035 | 1.447 | 41.13x | 0.035 | 1.446 | 41.13x |
| ols_summary_n10000 | 0.257 | 5.575 | 21.70x | 0.257 | 5.574 | 21.70x |
| ols_hac_n10000 | 0.980 | 6.317 | 6.45x | 0.983 | 6.316 | 6.43x |
| ols_cluster_n10000 | 0.328 | 7.034 | 21.46x | 0.329 | 7.033 | 21.41x |
| ols_summary_n100000 | 2.465 | 72.961 | 29.59x | 2.477 | 290.949 | 117.44x |
| ols_hac_n100000 | 10.639 | 76.738 | 7.21x | 11.168 | 306.452 | 27.44x |
| ols_cluster_n100000 | 4.407 | 78.804 | 17.88x | 4.742 | 314.674 | 66.36x |

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
| sparse_10000 | 1.241 | 2.508 | 2.02x | 1.243 | 2.507 | 2.02x |
| sparse_infrequent_10000 | 1.552 | 2.583 | 1.66x | 1.556 | 2.583 | 1.66x |
| sparse_100000 | 12.977 | 20.033 | 1.54x | 13.320 | 20.032 | 1.50x |
| sparse_infrequent_100000 | 14.312 | 15.985 | 1.12x | 14.654 | 15.984 | 1.09x |

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
| fe_entity_n1000 | 0.125 | 10.458 | 83.61x | 0.128 | 10.457 | 81.88x |
| fe_twoway_clustered_n1000 | 0.212 | 16.437 | 77.44x | 0.212 | 16.437 | 77.45x |
| fe_kernel_n1000 | 0.131 | 10.894 | 82.92x | 0.131 | 10.893 | 82.92x |
| between_robust_n1000 | 0.069 | 10.320 | 149.69x | 0.069 | 10.319 | 149.68x |
| first_difference_robust_n1000 | 0.131 | 12.214 | 93.54x | 0.131 | 12.213 | 93.54x |
| random_effects_n1000 | 0.131 | 13.924 | 106.03x | 0.131 | 13.922 | 106.03x |
| fe_entity_n10000 | 1.613 | 16.579 | 10.28x | 1.678 | 16.577 | 9.88x |
| fe_twoway_clustered_n10000 | 2.953 | 32.731 | 11.08x | 3.166 | 65.477 | 20.68x |
| fe_kernel_n10000 | 1.759 | 16.559 | 9.41x | 1.783 | 16.557 | 9.28x |
| between_robust_n10000 | 0.888 | 15.150 | 17.05x | 0.890 | 15.149 | 17.02x |
| first_difference_robust_n10000 | 1.693 | 18.404 | 10.87x | 1.698 | 18.402 | 10.84x |
| random_effects_n10000 | 1.758 | 21.456 | 12.20x | 1.778 | 21.454 | 12.07x |
| fe_entity_n100000 | 24.178 | 116.092 | 4.80x | 25.273 | 463.520 | 18.34x |
| fe_twoway_clustered_n100000 | 31.920 | 224.389 | 7.03x | 33.434 | 895.624 | 26.79x |
| fe_kernel_n100000 | 24.306 | 102.777 | 4.23x | 25.469 | 410.297 | 16.11x |
| between_robust_n100000 | 13.807 | 110.768 | 8.02x | 14.051 | 441.190 | 31.40x |
| first_difference_robust_n100000 | 21.336 | 118.510 | 5.55x | 22.796 | 472.620 | 20.73x |
| random_effects_n100000 | 22.023 | 136.017 | 6.18x | 23.505 | 542.786 | 23.09x |

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
| welch_t_n1000 | 0.003 | 0.322 | 116.59x | 0.003 | 0.322 | 116.55x |
| mann_whitney_n1000 | 0.019 | 0.273 | 14.24x | 0.019 | 0.273 | 14.23x |
| chi_square_n1000 | 0.000 | 0.135 | 1460.00x | 0.000 | 0.135 | 1459.63x |
| fligner_n1000 | 0.038 | 0.306 | 8.00x | 0.038 | 0.306 | 8.00x |
| anderson_ksamp_n1000 | 0.042 | 0.167 | 4.00x | 0.042 | 0.167 | 4.00x |
| spearman_matrix_n1000 | 0.052 | 0.286 | 5.52x | 0.052 | 0.286 | 5.52x |
| pointbiserial_n1000 | 0.006 | 0.221 | 34.54x | 0.006 | 0.221 | 34.53x |
| welch_t_n10000 | 0.026 | 0.334 | 13.04x | 0.026 | 0.334 | 13.04x |
| mann_whitney_n10000 | 0.295 | 1.468 | 4.97x | 0.295 | 1.468 | 4.97x |
| chi_square_n10000 | 0.001 | 0.131 | 180.34x | 0.001 | 0.131 | 180.34x |
| fligner_n10000 | 0.606 | 1.814 | 3.00x | 0.650 | 1.814 | 2.79x |
| anderson_ksamp_n10000 | 1.032 | 2.276 | 2.21x | 1.097 | 2.276 | 2.08x |
| spearman_matrix_n10000 | 0.921 | 2.620 | 2.85x | 0.948 | 2.619 | 2.76x |
| pointbiserial_n10000 | 0.060 | 0.254 | 4.21x | 0.060 | 0.254 | 4.21x |
| welch_t_n100000 | 0.260 | 0.585 | 2.25x | 0.260 | 0.585 | 2.25x |
| mann_whitney_n100000 | 3.372 | 17.043 | 5.05x | 3.371 | 17.042 | 5.06x |
| chi_square_n100000 | 0.008 | 0.141 | 17.20x | 0.008 | 0.141 | 17.20x |
| fligner_n100000 | 8.250 | 21.125 | 2.56x | 8.625 | 21.122 | 2.45x |
| anderson_ksamp_n100000 | 7.976 | 27.234 | 3.41x | 8.129 | 27.232 | 3.35x |
| spearman_matrix_n100000 | 7.284 | 32.747 | 4.50x | 7.412 | 32.744 | 4.42x |
| pointbiserial_n100000 | 0.903 | 0.674 | 0.75x | 0.911 | 2.690 | 2.95x |

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
| fit_1000 | 5.031 | 32.375 | 6.44x | 5.031 | 129.334 | 25.71x |
| fit_penalised_weighted_1000 | 4.766 | 35.270 | 7.40x | 4.766 | 140.555 | 29.49x |
| predict_survival_1000 | 0.549 | 0.991 | 1.81x | 0.551 | 0.991 | 1.80x |
| fit_10000 | 143.190 | 340.700 | 2.38x | 143.410 | 685.022 | 4.78x |
| fit_penalised_weighted_10000 | 133.630 | 351.964 | 2.63x | 133.808 | 695.108 | 5.19x |
| predict_survival_10000 | 2.113 | 1.398 | 0.66x | 2.269 | 5.574 | 2.46x |

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
| logrank_wilcoxon_weighted_1000 | 0.014 | 6.966 | 485.00x | 0.014 | 6.965 | 485.03x |
| logrank_fleming_harrington_1000 | 0.020 | 10.186 | 518.00x | 0.020 | 10.185 | 518.00x |
| multigroup_1000 | 0.065 | 13.836 | 212.42x | 0.065 | 13.836 | 212.43x |
| pairwise_1000 | 0.159 | 70.517 | 444.06x | 0.159 | 70.510 | 444.06x |
| rmst_1000 | 0.055 | 527.166 | 9615.32x | 0.055 | 527.088 | 9614.51x |
| fixed_point_1000 | 0.022 | 7.449 | 341.32x | 0.022 | 7.449 | 341.33x |
| concordance_1000 | 0.028 | 4.289 | 154.08x | 0.028 | 4.288 | 154.06x |
| logrank_wilcoxon_weighted_10000 | 0.141 | 7.929 | 56.34x | 0.141 | 7.929 | 56.34x |
| logrank_fleming_harrington_10000 | 0.200 | 12.725 | 63.75x | 0.200 | 12.724 | 63.75x |
| multigroup_10000 | 1.709 | 23.463 | 13.73x | 1.711 | 23.461 | 13.71x |
| pairwise_10000 | 2.927 | 80.548 | 27.52x | 2.927 | 80.539 | 27.52x |
| rmst_10000 | 1.190 | 575.255 | 483.42x | 1.221 | 575.193 | 471.27x |
| fixed_point_10000 | 0.219 | 8.518 | 38.92x | 0.219 | 8.517 | 38.92x |
| concordance_10000 | 1.746 | 49.910 | 28.59x | 1.746 | 49.905 | 28.59x |
| logrank_wilcoxon_weighted_100000 | 2.920 | 12.839 | 4.40x | 3.003 | 12.838 | 4.28x |
| logrank_fleming_harrington_100000 | 2.876 | 19.974 | 6.94x | 2.933 | 19.972 | 6.81x |
| multigroup_100000 | 7.622 | 30.160 | 3.96x | 7.747 | 30.159 | 3.89x |
| pairwise_100000 | 30.969 | 135.194 | 4.37x | 31.707 | 135.179 | 4.26x |
| rmst_100000 | 6.576 | 216.608 | 32.94x | 6.663 | 216.584 | 32.51x |
| fixed_point_100000 | 3.119 | 16.562 | 5.31x | 3.241 | 16.562 | 5.11x |
| concordance_100000 | 22.621 | 401.718 | 17.76x | 22.828 | 401.685 | 17.60x |

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
| weibull_1000 | 1.810 | 32.819 | 18.13x | 1.810 | 32.814 | 18.13x |
| loglogistic_left_1000 | 3.405 | 48.077 | 14.12x | 3.405 | 48.075 | 14.12x |
| lognormal_interval_1000 | 3.091 | 79.442 | 25.70x | 3.091 | 79.435 | 25.70x |
| generalized_gamma_1000 | 26.320 | 508.809 | 19.33x | 26.318 | 508.744 | 19.33x |
| weibull_aft_1000 | 3.790 | 64.142 | 16.92x | 3.791 | 64.137 | 16.92x |
| lognormal_aft_ridge_robust_1000 | 6.338 | 2274.734 | 358.91x | 6.341 | 2274.504 | 358.71x |
| loglogistic_aft_interval_1000 | 6.086 | 97.009 | 15.94x | 6.088 | 97.003 | 15.93x |
| breslow_fleming_harrington_1000 | 0.079 | 3.306 | 41.88x | 0.079 | 3.306 | 41.88x |
| kaplan_meier_left_1000 | 0.051 | 3.959 | 76.88x | 0.051 | 3.959 | 76.88x |
| weibull_10000 | 17.147 | 35.767 | 2.09x | 17.146 | 35.765 | 2.09x |
| loglogistic_left_10000 | 36.179 | 78.094 | 2.16x | 36.177 | 78.084 | 2.16x |
| lognormal_interval_10000 | 30.827 | 147.474 | 4.78x | 30.827 | 147.461 | 4.78x |
| generalized_gamma_10000 | 240.648 | 1254.036 | 5.21x | 240.645 | 1253.872 | 5.21x |
| weibull_aft_10000 | 36.892 | 68.794 | 1.86x | 36.896 | 68.783 | 1.86x |
| lognormal_aft_ridge_robust_10000 | 78.583 | 21482.659 | 273.37x | 78.583 | 21480.472 | 273.35x |
| loglogistic_aft_interval_10000 | 53.630 | 128.823 | 2.40x | 53.639 | 128.813 | 2.40x |
| breslow_fleming_harrington_10000 | 1.709 | 4.348 | 2.54x | 1.709 | 4.348 | 2.54x |
| kaplan_meier_left_10000 | 0.620 | 4.940 | 7.97x | 0.620 | 4.940 | 7.97x |

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
| var_n1000 | 0.020 | 1.268 | 62.40x | 0.020 | 1.268 | 62.39x |
| var_n10000 | 0.270 | 9.210 | 34.09x | 0.272 | 9.209 | 33.88x |
| var_n100000 | 2.193 | 89.872 | 40.98x | 2.281 | 89.865 | 39.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `BitParallelEditDistanceBenchmarks`
- `BkTreeNearestBenchmarks`
- `ChainedProductBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
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
- `VectorAutoregressionBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 24.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `LcsGateBenchmarks` | Band=16 | `Kernel` | step | 0.1 | 0.22 | -55% | 30% |
| `LcsGateBenchmarks` | Band=16 | `Kernel_Cjk` | step | 0.18 | 0.36 | -50% | 30% |
| `MetricsIncumbentBenchmarks` | Samples=1000000, Request=Bundle | `MlNet` | step | 1.24 | 2.28 | -46% | 39% |
| `LcsGateBenchmarks` | Band=64 | `Dp_Cjk` | step | 1.44 | 0.99 | +45% | 30% |
| `LevenshteinBenchmarks` | Length=8 | `Distance_CodePoint` | step | 6.52 | 4.68 | +39% | 30% |
| `AgglomerativeIncumbentBenchmarks` | Rows=1500, Method=ward | `Aglomera_GetClustering` | step | 69.61 | 105.73 | -34% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_MannWhitney` | step | 2.02 | 3.06 | -34% | 30% |
| `LcsGateBenchmarks` | Band=24 | `Kernel_Cjk` | step | 0.1 | 0.15 | -33% | 30% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `AffineSignaturesOnly` | step | 0.02 | 0.03 | -33% | 30% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `AffineSignaturesOnly` | step | 0.02 | 0.03 | -33% | 30% |
| `BpeBenchmarks` | — | `Bpe` | step | 3.63 | 2.77 | +31% | 30% |
| `MyersGateBenchmarks` | Band=64 | `Dp_Cjk` | step | 0.69 | 0.99 | -30% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_OneWayAnova` | step | 0.83 | 1.19 | -30% | 30% |
| `AgglomerativeIncumbentBenchmarks` | Rows=1500, Method=complete | `Aglomera_GetClustering` | step | 103.9 | 148.9 | -30% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KruskalWallis` | drift | 6.4 | 8.94 | -28% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KruskalWallis` | drift | 8.54 | 11.855 | -27% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_KolmogorovSmirnov` | drift | 3.42 | 4.93 | -27% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_MannWhitney` | drift | 3.57 | 4.84 | -26% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `MetaNumerics_Wilcoxon` | drift | 0.93 | 1.3 | -26% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=100 | `BatchedFit` | drift | 1.06 | 1.43 | -26% | 20% |
| `PartialFitBenchmarks` | RowCount=100000, BatchCount=10 | `DenseFitOfTheSameData` | drift | 1.04 | 1.38 | -25% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_ChiSquare` | drift | 0.11 | 0.145 | -24% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `DenseFitOfTheSameData` | drift | 1.07 | 1.39 | -23% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=100 | `Lodestar_KolmogorovSmirnov` | drift | 2.2 | 3.135 | -22% | 20% |

**Still away from their median: 30.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | step | 12.29 | 0.91 | +1251% | 30% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `NumFlat_Fit` | step | 27.4 | 7.64 | +259% | 50% |
| `DbscanIncumbentBenchmarks` | Shape=20000x2x10 | `Dbscan_CalculateClusters` | step | 16.03 | 4.53 | +254% | 82% |
| `VectorizerBenchmarks` | Documents=200 | `CountBigrams` | step | 3.55 | 1.19 | +198% | 30% |
| `VectorizerBenchmarks` | Documents=1000 | `CountCharWordBoundary` | step | 8.6 | 2.94 | +193% | 30% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `Dbscan_CalculateClusters` | step | 11.16 | 3.92 | +185% | 71% |
| `DbscanIncumbentBenchmarks` | Shape=5000x2x5 | `NumFlat_Fit` | step | 19.62 | 7.01 | +180% | 86% |
| `DbscanDimensionBenchmarks` | Shape=5000x16x8 | `NumFlat_Fit` | drift | 11.68 | 6.82 | +71% | 20% |
| `VectorizerBenchmarks` | Documents=1000 | `CountBigrams` | step | 2.41 | 1.45 | +66% | 30% |
| `DbscanDimensionBenchmarks` | Shape=10000x8x8 | `NumFlat_Fit` | step | 11.36 | 7.04 | +61% | 34% |
| `KMeansLloydIncumbentBenchmarks` | Shape=10000x16x16 | `NumFlat_Lloyd` | drift | 3.22 | 2.16 | +49% | 20% |
| `KMeansLloydIncumbentBenchmarks` | Shape=50000x8x32 | `NumFlat_Lloyd` | drift | 4.23 | 2.88 | +47% | 20% |
| `Bm25Benchmarks` | Documents=1000 | `LodestarFromText` | drift | 2368.26 | 3973.42 | -40% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_Wilcoxon` | step | 7.006 | 5.026 | +39% | 30% |
| `IndelCodePointBenchmarks` | Length=128 | `Distance_Utf16` | step | 1.28 | 2.04 | -37% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_MannWhitney` | drift | 9.353 | 14.8845 | -37% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_OneWayAnova` | step | 1.413 | 2.243 | -37% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_ChiSquare` | step | 0.002 | 0.003 | -33% | 30% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_KruskalWallis` | drift | 22.448 | 33.582 | -33% | 20% |
| `DecompositionBenchmarks` | — | `Nmf_Rank20` | drift | 7.65 | 5.77 | +33% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `BatchedFit` | drift | 1.15 | 1.64 | -30% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_FisherExact` | drift | 0.029 | 0.0385 | -25% | 20% |
| `DecompositionBenchmarks` | — | `MlNet_ProjectToPrincipalComponents_Rank20` | drift | 1.56 | 1.3 | +24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_FisherExact` | drift | 0.008 | 0.0105 | -24% | 20% |
| `Bm25Benchmarks` | Documents=20000 | `LodestarFromText` | drift | 3618.39 | 4742.68 | -24% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `MetaNumerics_ChiSquare` | drift | 0.013 | 0.017 | -24% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=10 | `SparseFit` | drift | 0.22 | 0.18 | +22% | 20% |
| `PartialFitBenchmarks` | RowCount=10000, BatchCount=100 | `SparseFit` | drift | 0.23 | 0.19 | +21% | 20% |
| `MetaNumericsStatsBenchmarks` | SampleSize=10000 | `Lodestar_Wilcoxon` | drift | 3.712 | 4.658 | -20% | 20% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.04 | 0.05 | -20% | 20% |
