# Nightly benchmark run

<!-- nightly-baseline: 1a87da350fbdf0a6f569275fd6868c72f0bd8371 -->
<!-- nightly-owed: ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks DamerauLevenshteinBenchmarks EmbeddingSearchBenchmarks GlmBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks KsExactTableBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks QgramBenchmarks QuantileBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks TopKAccuracyBenchmarks VectorAutoregressionBenchmarks WeightedLeastSquaresBenchmarks -->
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

- Commit: `1a87da350fbdf0a6f569275fd6868c72f0bd8371`
- Previous run: `1a87da350fbdf0a6f569275fd6868c72f0bd8371`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

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
- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `StatsBenchmarks`
- `RankTestBenchmarks`
- `DistributionTailBenchmarks`
- `DistributionIncumbentBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `EncoderIncumbentBenchmarks`
- `TransformerIncumbentBenchmarks`
- `ScalerIncumbentBenchmarks`

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.529 ms** |   **0.2771 ms** |  **0.0152 ms** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   5.296 ms |  10.3488 ms |  0.5673 ms |  2.09 |    0.19 |        - |        - |        - |    42832 B |          NA |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.701 ms |   6.7377 ms |  0.3693 ms |  2.25 |    0.13 |  93.7500 |  93.7500 |  93.7500 |   470832 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **11.863 ms** |   **0.2998 ms** |  **0.0164 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |  10.621 ms |   8.1655 ms |  0.4476 ms |  0.90 |    0.03 |        - |        - |        - |    42840 B |          NA |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  13.392 ms |  16.7007 ms |  0.9154 ms |  1.13 |    0.07 | 312.5000 | 312.5000 | 312.5000 |  2711060 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **51.147 ms** |   **3.3914 ms** |  **0.1859 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  51.749 ms |  24.6987 ms |  1.3538 ms |  1.01 |    0.02 |        - |        - |        - |   802898 B |          NA |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  67.928 ms | 186.0545 ms | 10.1983 ms |  1.33 |    0.17 | 375.0000 | 375.0000 | 375.0000 |  8072038 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **236.248 ms** |   **8.4129 ms** |  **0.4611 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 176.982 ms | 114.9586 ms |  6.3013 ms |  0.75 |    0.02 |        - |        - |        - |   803165 B |          NA |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 265.765 ms |  61.8055 ms |  3.3878 ms |  1.12 |    0.01 | 500.0000 | 500.0000 | 500.0000 | 52871716 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,592.988 ms** |   **155.6830 ms** |   **8.5335 ms** | **1.000** |    **0.01** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,687.188 ms |   442.4952 ms |  24.2547 ms | 1.059 |    0.01 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.514 ms |     0.3264 ms |   0.0179 ms | 0.001 |    0.00 |   330.0781 |   330.0781 |   330.0781 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **6,319.729 ms** | **1,401.0987 ms** |  **76.7989 ms** | **1.000** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **37.83 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  6,478.407 ms | 2,584.1785 ms | 141.6475 ms | 1.025 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.94 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      9.156 ms |     1.9763 ms |   0.1083 ms | 0.001 |    0.00 |   484.3750 |   484.3750 |   484.3750 |   5.86 MB |        0.15 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **6,452.793 ms** | **1,781.4627 ms** |  **97.6480 ms** | **1.000** |    **0.02** |  **2000.0000** |  **2000.0000** |  **2000.0000** |  **44.12 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  6,462.409 ms |   386.5934 ms |  21.1905 ms | 1.002 |    0.01 |  1000.0000 |  1000.0000 |  1000.0000 |  42.15 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      8.013 ms |     1.5923 ms |   0.0873 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **26,866.125 ms** | **7,077.6237 ms** | **387.9484 ms** | **1.000** |    **0.02** | **15000.0000** | **14000.0000** | **14000.0000** | **388.84 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 26,831.569 ms | 1,388.8500 ms |  76.1276 ms | 0.999 |    0.01 | 17000.0000 | 16000.0000 | 16000.0000 | 385.31 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     23.795 ms |     1.8830 ms |   0.1032 ms | 0.001 |    0.00 |   312.5000 |   312.5000 |   312.5000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Documents | Permutations | Device               | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |-------------:|--------------:|-------------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |     **88.08 ms** |      **5.474 ms** |     **0.300 ms** |  **1.00** |    **0.00** |          **-** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |  4,572.75 ms |  2,829.875 ms |   155.115 ms | 51.91 |    1.53 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |  4,584.78 ms |  2,337.456 ms |   128.124 ms | 52.05 |    1.27 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |    **106.29 ms** |     **16.010 ms** |     **0.878 ms** |  **1.00** |    **0.01** |          **-** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  8,530.43 ms |  5,095.588 ms |   279.306 ms | 80.26 |    2.35 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  9,006.34 ms |  1,701.221 ms |    93.250 ms | 84.74 |    0.97 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |    **877.98 ms** |    **146.065 ms** |     **8.006 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] | 43,189.71 ms |  1,724.803 ms |    94.542 ms | 49.19 |    0.40 |  3000.0000 |  2000.0000 | 2000.0000 | 160.07 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] | 44,100.17 ms |  2,678.312 ms |   146.807 ms | 50.23 |    0.42 |  5000.0000 |  3000.0000 | 2000.0000 | 211.63 MB |        7.11 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |  **1,062.07 ms** |    **177.276 ms** |     **9.717 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 86,186.90 ms | 11,179.375 ms |   612.779 ms | 81.15 |    0.82 | 10000.0000 |  9000.0000 | 8000.0000 | 536.25 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 88,871.79 ms | 42,422.425 ms | 2,325.316 ms | 83.68 |    2.01 | 12000.0000 | 10000.0000 | 8000.0000 | 587.68 MB |       14.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,491.70 ns | 204.184 ns | 11.192 ns | 1.000 |      - |         - |          NA |
| MathNet_StudentCdf              |   294.67 ns |  12.207 ns |  0.669 ns | 0.118 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   345.98 ns |  17.039 ns |  0.934 ns | 0.139 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,666.44 ns |   8.689 ns |  0.476 ns | 3.077 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,190.41 ns | 407.575 ns | 22.341 ns | 2.083 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,230.46 ns |  38.617 ns |  2.117 ns | 0.494 | 0.0114 |     288 B |          NA |
| Lodestar_FisherCdf              |   926.03 ns |  39.985 ns |  2.192 ns | 0.372 |      - |         - |          NA |
| MathNet_FisherCdf               |   117.42 ns |   6.356 ns |  0.348 ns | 0.047 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   121.44 ns |   0.867 ns |  0.048 ns | 0.049 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 3,234.34 ns | 492.025 ns | 26.970 ns | 1.298 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,252.24 ns | 118.215 ns |  6.480 ns | 0.904 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   308.65 ns |  38.918 ns |  2.133 ns | 0.124 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    37.53 ns |   5.417 ns |  0.297 ns | 0.015 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    51.34 ns |   6.389 ns |  0.350 ns | 0.021 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    69.20 ns |   1.017 ns |  0.056 ns | 0.028 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   337.71 ns |  11.690 ns |  0.641 ns | 0.136 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   428.60 ns |  36.049 ns |  1.976 ns | 0.172 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   381.58 ns | 147.769 ns |  8.100 ns | 0.153 |      - |         - |          NA |
| Lodestar_NormalCdf              |    22.77 ns |   1.404 ns |  0.077 ns | 0.009 |      - |         - |          NA |
| MathNet_NormalCdf               |    19.30 ns |   2.034 ns |  0.111 ns | 0.008 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    56.30 ns |   0.682 ns |  0.037 ns | 0.023 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    27.00 ns |   1.212 ns |  0.066 ns | 0.011 |      - |         - |          NA |
| MathNet_NormalQuantile          |    28.82 ns |   4.508 ns |  0.247 ns | 0.012 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   159.08 ns |   0.856 ns |  0.047 ns | 0.064 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  30.62 ns | 3.203 ns | 0.176 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  13.18 ns | 2.840 ns | 0.156 ns |  0.43 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  37.67 ns | 5.686 ns | 0.312 ns |  1.23 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  65.79 ns | 0.871 ns | 0.048 ns |  2.15 |    0.01 |         - |          NA |
| ChiSquaredSfFractionalDf   | 270.43 ns | 1.321 ns | 0.072 ns |  8.83 |    0.04 |         - |          NA |
| NormalQuantile             |  26.87 ns | 0.542 ns | 0.030 ns |  0.88 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,189.4 μs** |   **206.69 μs** |  **11.33 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    308.6 μs |    10.12 μs |   0.55 μs |   2.9297 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    534.5 μs |    56.44 μs |   3.09 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |    935.9 μs |    71.59 μs |   3.92 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    494.0 μs |     6.67 μs |   0.37 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **680.1 μs** |    **16.02 μs** |   **0.88 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    281.6 μs |    15.81 μs |   0.87 μs |   2.9297 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    511.3 μs |   266.21 μs |  14.59 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    800.8 μs |     4.39 μs |   0.24 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    436.7 μs |     4.21 μs |   0.23 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **12,805.9 μs** |   **413.08 μs** |  **22.64 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  3,568.7 μs |   257.11 μs |  14.09 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  4,807.1 μs | 2,671.02 μs | 146.41 μs | 656.2500 | 656.2500 | 656.2500 |  5062783 B |
| WilcoxonPooledDifferences | 100000     | False | 21,433.5 μs | 4,577.42 μs | 250.90 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  5,528.3 μs |   527.17 μs |  28.90 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **7,011.0 μs** |   **638.01 μs** |  **34.97 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  3,063.7 μs |   289.31 μs |  15.86 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,800.4 μs |   284.49 μs |  15.59 μs | 660.1563 | 660.1563 | 660.1563 |  5062782 B |
| WilcoxonPooledDifferences | 100000     | True  | 16,303.1 μs | 2,577.20 μs | 141.27 μs | 562.5000 | 562.5000 | 562.5000 | 19281722 B |
| MannWhitneyTest           | 100000     | True  |  4,454.6 μs |    95.55 μs |   5.24 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |     **9.385 μs** |   **0.5024 μs** |  **0.0275 μs** |  **1.00** |    **0.00** |   **0.9766** |   **0.0153** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |    35.654 μs |   1.1665 μs |  0.0639 μs |  3.80 |    0.01 |   2.6855 |   0.2441 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |    27.408 μs |  14.6721 μs |  0.8042 μs |  2.92 |    0.07 |   0.6409 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |     4.358 μs |   1.3593 μs |  0.0745 μs |  0.46 |    0.01 |   0.0687 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |     4.529 μs |   0.7694 μs |  0.0422 μs |  0.48 |    0.00 |   0.1373 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |     1.810 μs |   0.2175 μs |  0.0119 μs |  0.19 |    0.00 |   0.2651 |   0.0019 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |     3.140 μs |   0.2502 μs |  0.0137 μs |  0.33 |    0.00 |   0.3319 |        - |        - |    8.16 KB |        0.34 |
|                               |            |              |             |            |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |   **111.380 μs** |  **20.7132 μs** |  **1.1354 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 1,002.983 μs | 395.5346 μs | 21.6806 μs |  9.01 |    0.19 | 248.0469 | 248.0469 | 248.0469 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       |   289.038 μs |  64.0064 μs |  3.5084 μs |  2.60 |    0.04 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |   116.657 μs |   1.8218 μs |  0.0999 μs |  1.05 |    0.01 |   0.6104 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |   118.608 μs |   0.9871 μs |  0.0541 μs |  1.06 |    0.01 |   1.2207 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |    17.781 μs |   4.9330 μs |  0.2704 μs |  0.16 |    0.00 |   2.5330 |   0.2747 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |    29.528 μs |  12.2569 μs |  0.6718 μs |  0.27 |    0.01 |   3.1738 |   0.1831 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,051.1 ns** |     **206.76 ns** |     **11.33 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     30,577.1 ns |   6,402.94 ns |    350.97 ns |    5.05 |    0.05 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,354.0 ns |     360.17 ns |     19.74 ns |    0.39 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,560.6 ns |   3,711.16 ns |    203.42 ns |    3.89 |    0.03 |   0.9155 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        111.2 ns |      10.47 ns |      0.57 ns |    0.02 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        210.2 ns |       4.80 ns |      0.26 ns |    0.03 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **66,193.9 ns** |     **825.78 ns** |     **45.26 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    132,744.7 ns |   1,039.81 ns |     57.00 ns |   2.005 |    0.00 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    488,557.2 ns |   4,451.91 ns |    244.02 ns |   7.381 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,571,045.0 ns | 380,070.07 ns | 20,832.92 ns | 189.912 |    0.29 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        111.3 ns |      11.18 ns |      0.61 ns |   0.002 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        211.3 ns |      14.88 ns |      0.82 ns |   0.003 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **460.8 μs** |   **211.19 μs** | **11.58 μs** |   **5.3711** |   **0.4883** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **1,828.3 μs** |    **76.29 μs** |  **4.18 μs** |   **7.8125** |   **1.9531** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,261.4 μs** |   **164.79 μs** |  **9.03 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **16,333.8 μs** | **1,546.84 μs** | **84.79 μs** | **250.0000** | **250.0000** | **250.0000** | **1737.34 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.78 μs** |     **2.317 μs** |  **0.127 μs** |  **1.00** |    **0.6409** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     14.67 μs |     0.788 μs |  0.043 μs |  0.87 |    0.6104 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     20.60 μs |     2.372 μs |  0.130 μs |  1.23 |    1.0681 |    0.0305 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **63.93 μs** |    **12.199 μs** |  **0.669 μs** |  **1.00** |    **2.9297** |    **0.3662** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.93 μs |     2.862 μs |  0.157 μs |  0.37 |    2.5330 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     45.32 μs |     1.307 μs |  0.072 μs |  0.71 |    2.0142 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **7,655.18 μs** |   **166.951 μs** |  **9.151 μs** |  **1.00** |  **328.1250** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  7,566.64 μs |   366.431 μs | 20.085 μs |  0.99 |  328.1250 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  8,399.18 μs |   252.646 μs | 13.848 μs |  1.10 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **14,295.32 μs** | **1,343.338 μs** | **73.633 μs** |  **1.00** | **1078.1250** | **1062.5000** | **984.3750** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 11,301.06 μs |   796.977 μs | 43.685 μs |  0.79 |  750.0000 |  734.3750 | 656.2500 | 5660.21 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 14,243.18 μs | 1,586.919 μs | 86.984 μs |  1.00 |  546.8750 |  546.8750 | 546.8750 | 4916.41 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1     | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|---------:|---------:|----------:|
| CountLoadAsync2M | 24.13 ms |  4.321 ms | 0.237 ms |  656.2500 | 625.0000 | 375.0000 |  21.53 MB |
| CountLoadAsync4M | 51.07 ms | 10.282 ms | 0.564 ms | 1000.0000 | 909.0909 | 454.5455 |  43.93 MB |
| CountLoad4M      | 49.34 ms | 43.200 ms | 2.368 ms | 1000.0000 | 909.0909 | 454.5455 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error         | StdDev       | Gen0     | Gen1     | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|---------:|---------:|---------:|------------:|
| CountFitLongDocuments | 192,642.31 μs | 34,284.847 μs | 1,879.268 μs |        - |        - |        - | 54334.51 KB |
| CountSave240K         |     401.93 μs |     74.010 μs |     4.057 μs |   7.3242 |   2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     471.77 μs |     35.127 μs |     1.925 μs |   7.8125 |   3.9063 |   3.9063 |   739.43 KB |
| TfidfSave45K          |      55.88 μs |     24.415 μs |     1.338 μs |   5.5542 |   0.9155 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      56.27 μs |      2.564 μs |     0.141 μs |   7.3853 |        - |        - |   181.88 KB |
| IndexSave942K         |     279.51 μs |     37.371 μs |     2.048 μs |   5.3711 |   0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     295.28 μs |     24.211 μs |     1.327 μs |   5.3711 |   0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  28,722.54 μs | 33,441.569 μs | 1,833.045 μs | 437.5000 | 406.2500 | 156.2500 | 22049.27 KB |
| CountLoadAsync4M      |  59,417.75 μs | 32,540.325 μs | 1,783.645 μs | 777.7778 | 666.6667 | 222.2222 | 44983.41 KB |
| CountLoad4M           |  56,457.16 μs | 35,869.661 μs | 1,966.137 μs | 700.0000 | 600.0000 | 200.0000 |  44976.7 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     16.677 μs |     4.784 μs |   0.2622 μs |   1.0986 |   0.0610 |        - |    27.43 KB |
| CountSaveAsyncWide     |    517.251 μs |   103.771 μs |   5.6881 μs |  33.2031 |  29.2969 |  29.2969 |   740.17 KB |
| HashingLoadUndeclared  |      6.766 μs |     2.922 μs |   0.1602 μs |   3.4103 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     28.637 μs |     8.744 μs |   0.4793 μs |   4.9438 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    137.746 μs |    20.205 μs |   1.1075 μs |  10.7422 |   1.7090 |        - |   264.67 KB |
| CountLoadUndeclared78K |    906.426 μs |   493.733 μs |  27.0632 μs |  25.3906 |  23.4375 |   5.8594 |   656.32 KB |
| CountLoadUndeclared1M  | 10,126.691 μs | 1,883.210 μs | 103.2251 μs | 437.5000 | 406.2500 | 312.5000 | 10793.88 KB |
| CountSaveAsync1M       |  2,199.012 μs | 1,047.392 μs |  57.4111 μs | 117.1875 | 113.2813 | 113.2813 |     3162 KB |
| IndexSaveAsyncLongHead |  3,462.120 μs |   521.500 μs |  28.5852 μs |  78.1250 |  74.2188 |  74.2188 |   2124.2 KB |
| CountLoadUndeclared2M  | 20,871.457 μs | 1,747.732 μs |  95.7991 μs | 875.0000 | 843.7500 | 593.7500 | 22045.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.733 μs** |      **0.2009 μs** |     **0.0110 μs** |     **1.00** |    **0.01** |    **0.0153** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.328 μs |      0.1318 μs |     0.0072 μs |     1.34 |    0.01 |    0.0153 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.360 μs |      0.2416 μs |     0.0132 μs |     1.94 |    0.01 |    0.2060 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   3,546.187 μs |    441.4643 μs |    24.1981 μs | 2,046.22 |   16.49 |  898.4375 |  882.8125 |  882.8125 |  3723236 B |    8,781.22 |
| LuceneFromText         | 1000      |   7,412.038 μs |    408.4636 μs |    22.3893 μs | 4,276.89 |   25.98 |   46.8750 |   31.2500 |         - |  1375073 B |    3,243.10 |
|                        |           |                |                |               |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **24.647 μs** |      **4.5962 μs** |     **0.2519 μs** |     **1.00** |    **0.01** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      37.515 μs |      5.1664 μs |     0.2832 μs |     1.52 |    0.02 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      17.936 μs |      0.9837 μs |     0.0539 μs |     0.73 |    0.01 |    0.3357 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  74,196.263 μs | 32,222.2065 μs | 1,766.2079 μs | 3,010.61 |   67.49 | 1571.4286 | 1571.4286 | 1571.4286 | 61663530 B |  145,432.85 |
| LuceneFromText         | 20000     | 149,525.658 μs | 31,882.0256 μs | 1,747.5614 μs | 6,067.20 |   81.39 |  750.0000 |  500.0000 |         - | 22423360 B |   52,885.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-WUJWRU : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-WUJWRU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **345.13 μs** |    **55.936 μs** |    **62.172 μs** |  **1.03** |    **0.25** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    263.12 μs |    19.087 μs |    19.601 μs |  0.79 |    0.14 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |     74.63 μs |     6.632 μs |     7.096 μs |  0.22 |    0.04 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    357.81 μs |    23.264 μs |    26.791 μs |  1.07 |    0.20 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,583.94 μs |    92.747 μs |   103.088 μs |  4.73 |    0.85 |        - |        - |        - |  289.64 KB |        1.74 |
| MlNet_ReplaceMissingValues_Read | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,186.03 μs |    71.637 μs |    79.624 μs |  3.54 |    0.64 |        - |        - |        - |  309.91 KB |        1.86 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    170.33 μs |    10.573 μs |     0.580 μs |  1.00 |    0.00 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     85.92 μs |     5.619 μs |     0.308 μs |  0.50 |    0.00 |   0.6104 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.43 μs |     2.299 μs |     0.126 μs |  0.10 |    0.00 |   1.2512 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,137.44 μs | 3,317.032 μs |   181.818 μs |  6.68 |    0.92 |  40.0391 |   6.8359 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,014.02 μs | 1,308.978 μs |    71.749 μs |  5.95 |    0.37 |  31.2500 |   5.8594 |        - |   792.4 KB |        4.78 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    246.79 μs |    63.458 μs |     3.478 μs |  1.45 |    0.02 |  12.2070 |  11.7188 |        - |  291.22 KB |        1.76 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-WUJWRU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,620.41 μs** |   **322.909 μs** |   **302.049 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,766.77 μs |   144.852 μs |   142.264 μs |  0.85 |    0.06 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |    600.50 μs |    14.254 μs |    16.415 μs |  0.11 |    0.01 |        - |        - |        - |  625.65 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,579.04 μs |    25.307 μs |    29.143 μs |  0.64 |    0.04 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,922.72 μs | 3,241.637 μs | 3,733.074 μs |  2.31 |    0.67 |        - |        - |        - |  496.45 KB |        0.15 |
| MlNet_ReplaceMissingValues_Read | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,325.13 μs | 3,834.239 μs | 4,261.746 μs |  1.66 |    0.75 |        - |        - |        - |  445.48 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,223.81 μs |   522.981 μs |    28.666 μs |  1.00 |    0.01 | 984.3750 | 984.3750 | 984.3750 | 3291.54 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,703.20 μs |   200.414 μs |    10.985 μs |  0.88 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.42 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    395.99 μs |   107.622 μs |     5.899 μs |  0.09 |    0.00 | 115.2344 | 115.2344 | 115.2344 |  626.12 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,590.77 μs | 1,412.537 μs |    77.426 μs |  0.38 |    0.02 |  19.5313 |   3.9063 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,211.39 μs |   961.341 μs |    52.694 μs |  1.00 |    0.01 |  23.4375 |        - |        - |  568.95 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,426.21 μs | 1,579.508 μs |    86.578 μs |  0.57 |    0.02 |  19.5313 |   7.8125 |        - |  472.64 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **374.0 ns** |     **58.26 ns** |   **3.19 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     220.5 ns |      1.69 ns |   0.09 ns |  0.59 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **374.5 ns** |     **86.97 ns** |   **4.77 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     228.2 ns |      4.75 ns |   0.26 ns |  0.61 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **457.6 ns** |     **17.84 ns** |   **0.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     307.9 ns |     18.96 ns |   1.04 ns |  0.67 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **459.7 ns** |     **23.70 ns** |   **1.30 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     306.3 ns |     28.98 ns |   1.59 ns |  0.67 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **549.2 ns** |     **53.77 ns** |   **2.95 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     406.5 ns |     37.73 ns |   2.07 ns |  0.74 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **552.9 ns** |     **30.63 ns** |   **1.68 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     394.5 ns |      6.44 ns |   0.35 ns |  0.71 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **649.5 ns** |     **74.61 ns** |   **4.09 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,261.9 ns |    135.41 ns |   7.42 ns |  1.94 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **628.8 ns** |      **2.47 ns** |   **0.14 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,276.8 ns |     77.33 ns |   4.24 ns |  2.03 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,050.2 ns** |     **56.49 ns** |   **3.10 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,689.4 ns |  2,129.35 ns | 116.72 ns |  2.78 |    0.05 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,048.7 ns** |    **426.40 ns** |  **23.37 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,384.9 ns |    613.67 ns |  33.64 ns |  2.63 |    0.03 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **16,718.7 ns** |  **1,250.34 ns** |  **68.54 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  65,239.8 ns |  1,288.21 ns |  70.61 ns |  3.90 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **482,347.2 ns** | **18,004.24 ns** | **986.87 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  65,858.7 ns | 11,158.54 ns | 611.64 ns |  0.14 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **531.7 μs** | **958.18 μs** | **52.52 μs** |  **1.9531** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 505.5 μs |  20.08 μs |  1.10 μs |  1.9531 |       - |  67.71 KB |
| ReportRows        | 1000   | 538.8 μs |  65.19 μs |  3.57 μs |  5.8594 |  0.9766 | 153.99 KB |
| **F1Macro**           | **5000**   | **642.6 μs** |  **36.20 μs** |  **1.98 μs** | **13.6719** |  **3.9063** | **337.24 KB** |
| PrecisionWeighted | 5000   | 654.7 μs |  55.69 μs |  3.05 μs | 13.6719 |  3.9063 | 337.24 KB |
| ReportRows        | 5000   | 797.0 μs |  84.78 μs |  4.65 μs | 31.2500 | 30.2734 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,466.0 ns** |       **104.28 ns** |       **5.72 ns** | **0.0153** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     6,715.4 ns |       632.12 ns |      34.65 ns | 0.0153 |     400 B |
| AccuracyScore  | 1000    | 2       |       156.8 ns |        11.07 ns |       0.61 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,740.4 ns |     1,018.46 ns |      55.83 ns | 0.0229 |     656 B |
| Report         | 1000    | 2       |     6,343.6 ns |     1,004.71 ns |      55.07 ns | 0.2060 |    5336 B |
| **Matrix**         | **1000**    | **10**      |     **3,862.7 ns** |       **707.04 ns** |      **38.76 ns** | **0.0458** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     6,926.9 ns |       166.63 ns |       9.13 ns | 0.0458 |    1336 B |
| AccuracyScore  | 1000    | 10      |       155.9 ns |         1.19 ns |       0.07 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,952.8 ns |     1,056.46 ns |      57.91 ns | 0.0687 |    1880 B |
| Report         | 1000    | 10      |    10,761.6 ns |     1,941.14 ns |     106.40 ns | 0.4883 |   12360 B |
| **Matrix**         | **100000**  | **2**       |   **340,848.1 ns** |     **4,602.11 ns** |     **252.26 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   774,591.4 ns |    83,403.65 ns |   4,571.64 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    16,610.8 ns |       181.16 ns |       9.93 ns |      - |         - |
| F1Macro        | 100000  | 2       |   337,097.0 ns |    55,747.35 ns |   3,055.70 ns |      - |     656 B |
| Report         | 100000  | 2       |   354,250.5 ns |     7,263.46 ns |     398.13 ns |      - |    5360 B |
| **Matrix**         | **100000**  | **10**      |   **359,011.5 ns** |    **45,177.87 ns** |   **2,476.35 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   913,320.8 ns |   127,660.84 ns |   6,997.52 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    17,192.6 ns |     2,100.02 ns |     115.11 ns |      - |         - |
| F1Macro        | 100000  | 10      |   330,970.0 ns |    34,096.94 ns |   1,868.97 ns |      - |    1880 B |
| Report         | 100000  | 10      |   339,835.2 ns |    32,229.25 ns |   1,766.59 ns | 0.4883 |   12704 B |
| **Matrix**         | **1000000** | **2**       | **3,394,906.8 ns** |   **768,389.59 ns** |  **42,118.03 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,951,105.9 ns |   875,275.12 ns |  47,976.78 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   274,458.0 ns |   136,475.54 ns |   7,480.68 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,517,982.6 ns |   443,987.03 ns |  24,336.43 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,473,116.6 ns |   223,148.46 ns |  12,231.52 ns |      - |    5379 B |
| **Matrix**         | **1000000** | **10**      | **3,460,124.5 ns** |    **46,398.27 ns** |   **2,543.25 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 9,055,219.0 ns | 3,066,021.61 ns | 168,058.99 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   278,159.3 ns |   296,620.63 ns |  16,258.78 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,261,984.5 ns |   583,189.36 ns |  31,966.58 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,409,024.5 ns |   362,565.72 ns |  19,873.45 ns |      - |   12747 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **77.50 ns** |      **3.825 ns** |   **0.210 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     78.56 ns |     28.201 ns |   1.546 ns |  1.01 |    0.02 |         - |          NA |
| Dp_Cjk     | 4    |     77.23 ns |      9.630 ns |   0.528 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |     76.40 ns |      1.327 ns |   0.073 ns |  0.99 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **6**    |    **107.36 ns** |      **7.868 ns** |   **0.431 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     77.34 ns |     16.091 ns |   0.882 ns |  0.72 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    106.21 ns |      8.911 ns |   0.488 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    130.84 ns |      3.362 ns |   0.184 ns |  1.22 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **8**    |    **150.26 ns** |      **3.678 ns** |   **0.202 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     89.52 ns |     89.954 ns |   4.931 ns |  0.60 |    0.03 |         - |          NA |
| Dp_Cjk     | 8    |    150.26 ns |      1.580 ns |   0.087 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    180.55 ns |     22.823 ns |   1.251 ns |  1.20 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **10**   |    **196.97 ns** |      **7.521 ns** |   **0.412 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |     94.67 ns |      0.941 ns |   0.052 ns |  0.48 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    198.93 ns |     30.215 ns |   1.656 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    136.71 ns |      1.589 ns |   0.087 ns |  0.69 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **279.64 ns** |     **21.008 ns** |   **1.152 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    103.54 ns |      1.915 ns |   0.105 ns |  0.37 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    279.12 ns |     36.979 ns |   2.027 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    150.24 ns |      6.690 ns |   0.367 ns |  0.54 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **423.49 ns** |     **47.896 ns** |   **2.625 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    118.72 ns |      1.580 ns |   0.087 ns |  0.28 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    421.60 ns |     31.102 ns |   1.705 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    169.74 ns |      3.813 ns |   0.209 ns |  0.40 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **815.39 ns** |      **4.878 ns** |   **0.267 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    152.73 ns |      1.608 ns |   0.088 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    820.88 ns |     48.962 ns |   2.684 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    214.11 ns |     10.552 ns |   0.578 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,437.11 ns** |    **156.309 ns** |   **8.568 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    187.14 ns |     17.647 ns |   0.967 ns |  0.13 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,573.30 ns |  1,182.981 ns |  64.843 ns |  1.09 |    0.04 |         - |          NA |
| Kernel_Cjk | 32   |    255.88 ns |      1.903 ns |   0.104 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,153.71 ns** |    **103.958 ns** |   **5.698 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    254.51 ns |      2.160 ns |   0.118 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,174.12 ns |  1,235.550 ns |  67.725 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    341.49 ns |      1.974 ns |   0.108 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,495.77 ns** |    **254.486 ns** |  **13.949 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    323.57 ns |     30.515 ns |   1.673 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,490.43 ns |    344.785 ns |  18.899 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |    432.28 ns |      4.528 ns |   0.248 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **12,283.30 ns** |    **407.397 ns** |  **22.331 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    814.90 ns |    115.770 ns |   6.346 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 13,025.43 ns | 16,099.213 ns | 882.452 ns |  1.06 |    0.06 |         - |          NA |
| Kernel_Cjk | 96   |  1,545.03 ns |    187.172 ns |  10.260 ns |  0.13 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **8.698 μs** |  **0.5907 μs** | **0.0324 μs** |         **-** |
| EqualLength | 65           |     3.096 μs |  0.0357 μs | 0.0020 μs |         - |
| **Embedded**    | **128**          |    **31.264 μs** |  **4.4754 μs** | **0.2453 μs** |         **-** |
| EqualLength | 128          |     5.745 μs |  0.4972 μs | 0.0273 μs |         - |
| **Embedded**    | **512**          | **1,182.206 μs** | **46.7120 μs** | **2.5604 μs** |         **-** |
| EqualLength | 512          |    44.374 μs |  1.4707 μs | 0.0806 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  4.357 ms |  0.1418 ms | 0.0078 ms |  78.1250 |  70.3125 |  23.4375 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.687 ms |  0.9886 ms | 0.0542 ms | 125.0000 | 109.3750 |  31.2500 |  5852.26 KB |
| TokenizerJsonUnigram   | 12.182 ms |  1.0729 ms | 0.0588 ms |  62.5000 |  46.8750 |  15.6250 |   4748.9 KB |
| SpieceModel            |  4.981 ms |  4.0049 ms | 0.2195 ms |  78.1250 |  70.3125 |  31.2500 |  4292.23 KB |
| TfidfSave              |  1.604 ms |  0.2307 ms | 0.0126 ms |  19.5313 |  15.6250 |  15.6250 |  2137.06 KB |
| TfidfLoad              |  4.511 ms |  0.1915 ms | 0.0105 ms |  46.8750 |  39.0625 |   7.8125 |  2921.36 KB |
| EmbeddingIndexSave     |  5.875 ms |  2.4848 ms | 0.1362 ms | 257.8125 | 257.8125 | 257.8125 | 20349.96 KB |
| EmbeddingIndexLoad     |  8.607 ms |  1.5290 ms | 0.0838 ms | 125.0000 |  93.7500 |  93.7500 | 16094.43 KB |
| EmbeddingIndexSaveFile | 49.577 ms | 11.6901 ms | 0.6408 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 73.622 ms |  5.6850 ms | 0.3116 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **59.31 μs** |     **3.610 μs** |  **0.198 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    57.21 μs |     3.454 μs |  0.189 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   141.67 μs |     6.649 μs |  0.364 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   547.82 μs |    74.467 μs |  4.082 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **718.23 μs** |   **929.748 μs** | **50.963 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   606.64 μs |    26.575 μs |  1.457 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,525.19 μs |   241.739 μs | 13.251 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 7,464.44 μs | 1,343.387 μs | 73.636 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-YEIKII : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-YEIKII** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **99.045 μs** |     **4.1561 μs** |     **4.4470 μs** |  **1.00** |    **0.06** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     94.099 μs |     7.0902 μs |     7.8807 μs |  0.95 |    0.09 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,045.478 μs |    31.0013 μs |    35.7012 μs | 20.69 |    0.92 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |    102.181 μs |     6.7362 μs |     7.4873 μs |  1.03 |    0.09 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     70.576 μs |    11.2517 μs |    12.0392 μs |  0.71 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     62.455 μs |     1.8690 μs |     1.8356 μs |  0.63 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     91.043 μs |     7.1932 μs |     7.0647 μs |  0.92 |    0.08 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |    352.243 μs |    15.1314 μs |    17.4253 μs |  3.56 |    0.23 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,242.920 μs |   139.1484 μs |   154.6631 μs | 22.69 |    1.79 |        - |        - |        - |  331160 B |        4.06 |
| MlNet_NormalizeRobustScaling_Read | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  5,444.374 μs |    90.4801 μs |   100.5684 μs | 55.07 |    2.46 |        - |        - |        - |  461776 B |        5.66 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.087 μs |    10.1848 μs |     0.5583 μs |  1.00 |    0.01 |   3.1738 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     40.940 μs |     7.9238 μs |     0.4343 μs |  0.87 |    0.01 |   3.1738 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    451.769 μs |    17.2377 μs |     0.9449 μs |  9.60 |    0.10 |   6.3477 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     55.609 μs |     0.8095 μs |     0.0444 μs |  1.18 |    0.01 |   3.1738 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      9.808 μs |     0.3594 μs |     0.0197 μs |  0.21 |    0.00 |   0.0305 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      6.926 μs |     0.8193 μs |     0.0449 μs |  0.15 |    0.00 |   0.0076 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.975 μs |     1.4421 μs |     0.0790 μs |  0.25 |    0.00 |   1.0681 |   0.0153 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,856.174 μs | 5,034.6519 μs |   275.9663 μs | 39.42 |    5.09 |  55.6641 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,150.716 μs |   505.7121 μs |    27.7198 μs | 24.44 |    0.57 |  25.3906 |        - |        - |  655969 B |       8.109 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,737.889 μs | 3,038.8128 μs |   166.5676 μs | 36.91 |    3.09 |  23.4375 |        - |        - |  623401 B |       7.706 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-YEIKII** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,364.599 μs** |    **13.4171 μs** |    **14.3561 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,251.493 μs |    12.4791 μs |    13.3525 μs |  0.92 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,338.034 μs |    61.0404 μs |    65.3125 μs | 10.51 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,486.996 μs |    18.6393 μs |    21.4650 μs |  1.09 |    0.02 |        - |        - |        - | 1601272 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    448.651 μs |    11.8778 μs |    13.2021 μs |  0.33 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    398.089 μs |    16.7585 μs |    19.2992 μs |  0.29 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    489.546 μs |    17.9825 μs |    19.2410 μs |  0.36 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  5,376.579 μs |   424.4134 μs |   488.7551 μs |  3.94 |    0.35 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 22,105.078 μs | 3,375.4475 μs | 3,887.1703 μs | 16.20 |    2.79 |        - |        - |        - |  435712 B |       0.272 |
| MlNet_NormalizeRobustScaling_Read | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,263.635 μs |   323.0295 μs |   359.0464 μs | 14.12 |    0.29 |        - |        - |        - | 4277384 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,186.825 μs |   572.3945 μs |    31.3749 μs |  1.00 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601382 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,083.402 μs | 1,012.2115 μs |    55.4827 μs |  0.91 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,779.375 μs | 1,079.3633 μs |    59.1635 μs | 11.62 |    0.27 | 875.0000 | 875.0000 | 875.0000 | 3201194 B |       1.999 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,449.311 μs | 1,714.8171 μs |    93.9949 μs |  1.22 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1601054 B |       1.000 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    340.474 μs |    53.6593 μs |     2.9412 μs |  0.29 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    283.694 μs |    56.4080 μs |     3.0919 μs |  0.24 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    428.826 μs |   268.7840 μs |    14.7330 μs |  0.36 |    0.01 | 126.9531 | 124.0234 | 124.0234 |  560100 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,373.617 μs |   811.7776 μs |    44.4963 μs |  1.16 |    0.04 |  13.6719 |   3.9063 |        - |  354153 B |       0.221 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,605.213 μs | 1,793.4379 μs |    98.3044 μs |  3.88 |    0.11 |  23.4375 |        - |        - |  591640 B |       0.369 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 18,690.535 μs |   867.0907 μs |    47.5282 μs | 15.76 |    0.36 | 156.2500 |  62.5000 |        - | 4298542 B |       2.684 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean         | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-------------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |    **60.079 ms** |  **9.2072 ms** | **0.5047 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |    10.284 ms |  1.5174 ms | 0.0832 ms |  0.17 |   78.1250 |   78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |     8.766 ms |  0.5161 ms | 0.0283 ms |  0.15 |         - |         - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |     7.022 ms |  0.1873 ms | 0.0103 ms |  0.12 |    7.8125 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |    10.563 ms |  0.4431 ms | 0.0243 ms |  0.18 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |    **60.391 ms** |  **3.1359 ms** | **0.1719 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |    13.297 ms |  0.8599 ms | 0.0471 ms |  0.22 |  218.7500 |  218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |    10.487 ms |  0.8665 ms | 0.0475 ms |  0.17 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |     7.232 ms |  0.5014 ms | 0.0275 ms |  0.12 |   15.6250 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |    10.781 ms |  0.7021 ms | 0.0385 ms |  0.18 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **1,026.100 ms** | **51.5777 ms** | **2.8271 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126359.81 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |    44.421 ms |  5.4270 ms | 0.2975 ms |  0.04 |  750.0000 |  750.0000 | 500.0000 |  10334.84 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |    34.884 ms |  1.3465 ms | 0.0738 ms |  0.03 |         - |         - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |    28.091 ms |  2.1022 ms | 0.1152 ms |  0.03 |   31.2500 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |    43.114 ms |  4.6561 ms | 0.2552 ms |  0.04 |         - |         - |        - |   1718.81 KB |       0.014 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **1,025.474 ms** | **20.9645 ms** | **1.1491 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |    63.193 ms | 25.2958 ms | 1.3865 ms |  0.06 | 1250.0000 | 1250.0000 | 750.0000 |  18666.59 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |    41.947 ms |  5.1433 ms | 0.2819 ms |  0.04 |         - |         - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |    28.796 ms |  0.5276 ms | 0.0289 ms |  0.03 |   62.5000 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |    42.768 ms |  1.0193 ms | 0.0559 ms |  0.04 |         - |         - |        - |   1718.81 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean            | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|-------------:|------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,207.2 ns |    276.14 ns |    15.14 ns |   0.2174 |   0.0038 |        - |     5464 B |
| TfidfSaveAsync         |      1,470.2 ns |    177.75 ns |     9.74 ns |   0.2213 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,245.5 ns |    209.37 ns |    11.48 ns |   0.2174 |   0.0038 |        - |     5496 B |
| IndexSaveAsync         |        459.9 ns |     22.01 ns |     1.21 ns |   0.0257 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,337,956.8 ns | 52,510.87 ns | 2,878.30 ns | 765.6250 | 750.0000 | 593.7500 | 12227249 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | SampleCount | Mean          | Error      | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-----------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **96.128 μs** |   **4.780 μs** |  **0.2620 μs** |  **1.00** |    **0.00** |  **11.1084** |   **4.6387** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    177.315 μs |  23.551 μs |  1.2909 μs |  1.84 |    0.01 |  12.6953 |   4.3945 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      5.644 μs |   3.409 μs |  0.1869 μs |  0.06 |    0.00 |   1.5945 |   0.1755 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    389.083 μs | 222.888 μs | 12.2173 μs |  4.05 |    0.11 |        - |        - |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,598.473 μs | 192.379 μs | 10.5450 μs | 58.24 |    0.17 |        - |        - |        - |  148.65 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     89.368 μs |  25.125 μs |  1.3772 μs |  0.93 |    0.01 |   0.4883 |        - |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,050.963 μs |  48.554 μs |  2.6614 μs | 10.93 |    0.04 |        - |        - |        - |   34.17 KB |        0.12 |
|                                 |             |               |            |            |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,312.387 μs** | **737.690 μs** | **40.4353 μs** |  **1.00** |    **0.04** | **439.4531** | **429.6875** | **423.8281** | **2738.36 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,037.914 μs |  63.652 μs |  3.4890 μs |  1.55 |    0.04 | 519.5313 | 507.8125 | 503.9063 | 3130.31 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     92.383 μs |  26.134 μs |  1.4325 μs |  0.07 |    0.00 |  47.4854 |  47.4854 |  47.4854 |   391.1 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    454.771 μs |  75.973 μs |  4.1643 μs |  0.35 |    0.01 |   1.4648 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 41,167.258 μs | 323.221 μs | 17.7168 μs | 31.39 |    0.85 |        - |        - |        - |  148.78 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     90.572 μs |  12.386 μs |  0.6789 μs |  0.07 |    0.00 |   0.4883 |   0.1221 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,293.571 μs | 534.326 μs | 29.2882 μs |  5.56 |    0.15 |        - |        - |        - |   34.17 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **2.049 ms** | **0.1541 ms** | **0.0084 ms** |  **1.00** |    **0.01** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |  1.505 ms | 0.0195 ms | 0.0011 ms |  0.73 |    0.00 |  15.6250 |   3.9063 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       |  2.122 ms | 0.1457 ms | 0.0080 ms |  1.04 |    0.01 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       |  1.735 ms | 0.3307 ms | 0.0181 ms |  0.85 |    0.01 |   9.7656 |   1.9531 |        - |  273.65 KB |        0.31 |
|                      |           |           |           |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      |  **9.630 ms** | **9.5186 ms** | **0.5217 ms** |  **1.00** |    **0.07** | **859.3750** | **859.3750** | **859.3750** | **3436.53 KB** |        **1.00** |
| CountWithStopWords   | 1000      |  7.502 ms | 0.1640 ms | 0.0090 ms |  0.78 |    0.04 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 10.150 ms | 1.5418 ms | 0.0845 ms |  1.06 |    0.05 | 500.0000 | 500.0000 | 500.0000 | 2348.46 KB |        0.68 |
| HashingWithStopWords | 1000      |  8.617 ms | 0.4579 ms | 0.0251 ms |  0.90 |    0.04 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean          | Error         | StdDev      | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|------------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **42,309.349 μs** | **7,973.8889 μs** | **437.0758 μs** |       **-** |       **-** |       **-** | **1554.62 KB** |
| Construct                  | [^ ]+         |      2.167 μs |     0.1405 μs |   0.0077 μs |  0.2213 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.352 μs |     0.8153 μs |   0.0447 μs |  0.3014 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **35,810.216 μs** |   **342.3008 μs** |  **18.7627 μs** |       **-** |       **-** |       **-** |    **7.23 KB** |
| Construct                  | [a-z]+        |      2.004 μs |     0.4825 μs |   0.0264 μs |  0.2174 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.849 μs |     0.6366 μs |   0.0349 μs |  0.2899 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **47,652.564 μs** | **1,831.1686 μs** | **100.3725 μs** |       **-** |       **-** |       **-** | **1556.01 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.525 μs |     0.9291 μs |   0.0509 μs |  0.2747 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.772 μs |     1.1457 μs |   0.0628 μs |  0.3586 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **24,433.611 μs** | **1,918.3313 μs** | **105.1502 μs** | **93.7500** | **62.5000** | **62.5000** | **9743.62 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.137 μs |     0.1509 μs |   0.0083 μs |  0.0744 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      2.014 μs |     0.5082 μs |   0.0279 μs |  0.1564 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **42,725.933 μs** | **1,539.4778 μs** |  **84.3840 μs** |       **-** |       **-** |       **-** | **1554.51 KB** |
| Construct                  | \\S+          |      2.345 μs |     0.5773 μs |   0.0316 μs |  0.2136 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.664 μs |     0.9998 μs |   0.0548 μs |  0.2937 |       - |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-OUFSZU : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-OUFSZU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **53.73 μs** |     **11.029 μs** |     **12.701 μs** |     **49.03 μs** |   **1.05** |    **0.33** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |     67.84 μs |      1.368 μs |      1.576 μs |     68.25 μs |   1.32 |    0.27 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    164.99 μs |     10.212 μs |     11.760 μs |    165.02 μs |   3.22 |    0.69 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    252.32 μs |     11.636 μs |     12.933 μs |    250.24 μs |   4.92 |    1.03 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    866.35 μs |     26.745 μs |     28.617 μs |    858.29 μs |  16.90 |    3.47 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,445.99 μs |     11.406 μs |     12.678 μs |  1,443.05 μs |  28.21 |    5.73 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    947.16 μs |     20.806 μs |     22.262 μs |    938.63 μs |  18.48 |    3.77 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    197.55 μs |      7.332 μs |      7.845 μs |    196.60 μs |   3.85 |    0.80 |        - |        - |        - |   31.71 KB |        0.99 |
| Lodestar_Power_Fit          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,793.11 μs |     17.216 μs |     19.136 μs |  1,788.14 μs |  34.98 |    7.11 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     | 12,861.47 μs |  4,258.638 μs |  4,904.254 μs | 16,703.09 μs | 250.91 |  108.01 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,176.26 μs |     92.434 μs |     98.903 μs |  1,181.56 μs |  22.95 |    5.03 |        - |        - |        - |  339.98 KB |       10.63 |
| MlNet_NormalizeBinning_Read | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,933.92 μs |    161.459 μs |    172.759 μs |  2,874.77 μs |  57.24 |   12.09 |        - |        - |        - |  399.35 KB |       12.48 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.09 μs |      2.733 μs |      0.150 μs |     13.03 μs |   1.00 |    0.01 |   1.2665 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     62.53 μs |     12.384 μs |      0.679 μs |     62.81 μs |   4.78 |    0.07 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.24 μs |     14.567 μs |      0.798 μs |     83.09 μs |   6.36 |    0.08 |  49.9268 |  49.9268 |  49.9268 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     64.16 μs |      8.135 μs |      0.446 μs |     63.92 μs |   4.90 |    0.06 |   1.2207 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    136.06 μs |      1.872 μs |      0.103 μs |    136.04 μs |  10.40 |    0.10 |   1.2207 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    587.38 μs |    147.581 μs |      8.089 μs |    584.08 μs |  44.89 |    0.69 |   0.9766 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    251.69 μs |     39.606 μs |      2.171 μs |    250.94 μs |  19.24 |    0.24 |   4.3945 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    142.03 μs |      5.499 μs |      0.301 μs |    141.90 μs |  10.86 |    0.11 |   1.2207 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    694.75 μs |     61.612 μs |      3.377 μs |    694.02 μs |  53.10 |    0.57 |   4.8828 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  6,733.37 μs |    385.079 μs |     21.107 μs |  6,724.29 μs | 514.63 |    5.26 |        - |        - |        - |  103.15 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    266.07 μs |     98.123 μs |      5.378 μs |    265.93 μs |  20.34 |    0.41 |  12.2070 |  11.7188 |        - |  291.59 KB |        9.32 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,099.47 μs |  1,212.096 μs |     66.439 μs |  1,093.50 μs |  84.03 |    4.48 |  29.2969 |   5.8594 |        - |   705.1 KB |       22.55 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-OUFSZU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **421.48 μs** |     **13.177 μs** |     **15.174 μs** |    **419.09 μs** |   **1.00** |    **0.05** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,199.29 μs |     20.298 μs |     23.376 μs |  1,187.06 μs |   2.85 |    0.11 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,952.85 μs |     27.743 μs |     31.949 μs |  2,947.51 μs |   7.01 |    0.25 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,872.48 μs |    908.895 μs |  1,010.235 μs |  4,285.35 μs |   9.20 |    2.36 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,972.40 μs |  4,691.660 μs |  5,402.923 μs |  6,168.82 μs |  23.69 |   12.56 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,971.62 μs |     32.905 μs |     33.791 μs | 11,973.86 μs |  28.44 |    0.98 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,019.61 μs |  4,620.992 μs |  5,321.541 μs |  6,190.44 μs |  23.80 |   12.37 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,913.97 μs |     11.980 μs |     12.303 μs |  2,915.48 μs |   6.92 |    0.24 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,256.96 μs |  3,591.556 μs |  4,136.041 μs | 13,641.24 μs |  38.62 |    9.69 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,832.73 μs |  4,420.705 μs |  5,090.891 μs |  7,163.82 μs |  25.73 |   11.84 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,947.07 μs |  1,902.046 μs |  2,035.168 μs | 11,103.56 μs |  23.63 |    4.78 |        - |        - |        - |   548.3 KB |        0.88 |
| MlNet_NormalizeBinning_Read | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 25,746.35 μs | 12,088.985 μs | 13,921.693 μs | 15,374.31 μs |  61.16 |   32.36 |        - |        - |        - | 2154.53 KB |        3.44 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    550.09 μs |     74.859 μs |      4.103 μs |    548.27 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    680.08 μs |    501.352 μs |     27.481 μs |    667.31 μs |   1.24 |    0.04 | 296.8750 | 296.8750 | 296.8750 | 2346.46 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,158.65 μs |    320.293 μs |     17.556 μs |  2,152.37 μs |   3.92 |    0.04 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,608.88 μs |    162.696 μs |      8.918 μs |  1,613.89 μs |   2.92 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,226.92 μs |     57.113 μs |      3.131 μs |  5,228.48 μs |   9.50 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 12,429.56 μs |    284.626 μs |     15.601 μs | 12,423.64 μs |  22.60 |    0.15 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,416.86 μs |    111.190 μs |      6.095 μs |  5,414.49 μs |   9.85 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,137.83 μs |    132.243 μs |      7.249 μs |  3,140.06 μs |   5.70 |    0.04 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,417.20 μs |  1,565.199 μs |     85.794 μs | 13,425.99 μs |  24.39 |    0.21 | 750.0000 | 750.0000 | 750.0000 | 2660.69 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  6,919.35 μs |    131.924 μs |      7.231 μs |  6,920.08 μs |  12.58 |    0.08 |        - |        - |        - |  104.65 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,295.90 μs |    504.426 μs |     27.649 μs |  2,280.74 μs |   4.17 |    0.05 |  19.5313 |   7.8125 |        - |   477.9 KB |        0.76 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,409.71 μs |  1,332.915 μs |     73.062 μs | 10,417.55 μs |  18.92 |    0.17 | 234.3750 | 203.1250 | 187.5000 | 2138.32 KB |        3.42 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **51.82 ns** |  **1.820 ns** | **0.100 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  51.31 ns |  4.026 ns | 0.221 ns |  0.99 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **768**  | **114.92 ns** |  **3.046 ns** | **0.167 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  | 101.41 ns |  6.460 ns | 0.354 ns |  0.88 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **1024** | **144.81 ns** | **14.502 ns** | **0.795 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 141.64 ns |  2.363 ns | 0.130 ns |  0.98 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **264.5 μs** |    **10.03 μs** |   **0.55 μs** |  **1.00** |    **0.00** |    **8.3008** |    **0.9766** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    274.6 μs |    38.67 μs |   2.12 μs |  1.04 |    0.01 |    9.2773 |    1.4648 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    743.8 μs |    29.35 μs |   1.61 μs |  2.81 |    0.01 |   23.4375 |    8.7891 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,118.6 μs |   115.60 μs |   6.34 μs |  8.01 |    0.03 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    280.4 μs |    29.18 μs |   1.60 μs |  1.06 |    0.01 |    5.3711 |    0.4883 |         - |   136.13 KB |        0.66 |
|                       |           |             |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,549.1 μs** |    **82.08 μs** |   **4.50 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,696.3 μs |   169.84 μs |   9.31 μs |  1.10 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,702.5 μs |   623.20 μs |  34.16 μs |  2.39 |    0.02 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 13,070.9 μs | 7,776.69 μs | 426.27 μs |  8.44 |    0.24 | 1031.2500 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,647.9 μs |    90.85 μs |   4.98 μs |  1.06 |    0.00 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

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

### compare-cluster-weighted

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.743 | 4.197 | 0.73x | 5.749 | 8.371 | 1.46x |
| kmeans_restarts_10000 | 22.804 | 11.908 | 0.52x | 22.811 | 23.740 | 1.04x |
| kmeans_weighted_100000 | 57.618 | 33.903 | 0.59x | 58.023 | 57.020 | 0.98x |
| kmeans_restarts_100000 | 226.499 | 92.241 | 0.41x | 226.901 | 171.514 | 0.76x |
| dbscan_weighted_5000 | 8.565 | 31.521 | 3.68x | 9.001 | 31.519 | 3.50x |
| dbscan_weighted_20000 | 128.387 | 196.788 | 1.53x | 130.418 | 196.780 | 1.51x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.809 | 108.678 | 134.28x | 0.809 | 108.671 | 134.28x |
| ridge_robust_1000 | 0.889 | 100.124 | 112.61x | 0.889 | 100.121 | 112.61x |
| lasso_1000 | 5.933 | 916.777 | 154.53x | 5.932 | 916.733 | 154.54x |
| ph_test_1000 | 0.155 | 18.550 | 119.63x | 0.155 | 18.548 | 119.62x |
| predict_survival_1000 | 0.032 | 1.640 | 51.36x | 0.032 | 1.640 | 51.37x |
| time_varying_1000 | 54.676 | 330.889 | 6.05x | 54.676 | 330.883 | 6.05x |
| stratified_weighted_10000 | 8.512 | 617.802 | 72.58x | 8.561 | 617.783 | 72.17x |
| ridge_robust_10000 | 9.738 | 1037.864 | 106.58x | 9.945 | 1037.857 | 104.36x |
| lasso_10000 | 53.664 | 6194.494 | 115.43x | 53.687 | 6194.057 | 115.37x |
| ph_test_10000 | 2.222 | 175.012 | 78.75x | 2.285 | 174.996 | 76.59x |
| predict_survival_10000 | 0.034 | 1.618 | 47.23x | 0.034 | 1.618 | 47.24x |
| time_varying_10000 | 2005.624 | 5145.334 | 2.57x | 2006.814 | 5144.983 | 2.56x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 4.941 | 47.898 | 9.69x | 4.940 | 189.365 | 38.33x |
| cv_minmax_n1000 | 3.536 | 6.008 | 1.70x | 3.536 | 23.979 | 6.78x |
| cv_plus_gamma_n1000 | 5.736 | 48.855 | 8.52x | 5.736 | 195.235 | 34.04x |
| bootstrap_plus_n1000 | 16.221 | 48.854 | 3.01x | 16.223 | 193.563 | 11.93x |
| cv_plus_n10000 | 57.100 | 160.230 | 2.81x | 57.095 | 462.820 | 8.11x |
| cv_minmax_n10000 | 33.904 | 39.880 | 1.18x | 33.904 | 156.967 | 4.63x |
| cv_plus_gamma_n10000 | 69.017 | 170.219 | 2.47x | 69.013 | 479.639 | 6.95x |
| bootstrap_plus_n10000 | 172.616 | 161.417 | 0.94x | 172.592 | 473.933 | 2.75x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.392 | 3.305 | 8.44x | 0.392 | 3.305 | 8.44x |
| glm_gamma_n1000 | 0.472 | 4.122 | 8.73x | 0.472 | 4.122 | 8.73x |
| glm_poisson_exposure_n1000 | 0.392 | 3.910 | 9.98x | 0.392 | 3.910 | 9.98x |
| mnlogit_n1000 | 0.831 | 14.911 | 17.95x | 0.830 | 14.910 | 17.95x |
| glm_negative_binomial_n10000 | 3.580 | 12.683 | 3.54x | 3.588 | 12.683 | 3.53x |
| glm_gamma_n10000 | 4.061 | 14.780 | 3.64x | 4.069 | 14.780 | 3.63x |
| glm_poisson_exposure_n10000 | 4.190 | 14.556 | 3.47x | 4.197 | 14.554 | 3.47x |
| mnlogit_n10000 | 8.739 | 77.915 | 8.92x | 8.747 | 77.913 | 8.91x |
| glm_negative_binomial_n100000 | 39.511 | 154.722 | 3.92x | 39.748 | 617.040 | 15.52x |
| glm_gamma_n100000 | 46.437 | 171.150 | 3.69x | 46.664 | 671.133 | 14.38x |
| glm_poisson_exposure_n100000 | 55.731 | 151.392 | 2.72x | 55.460 | 590.597 | 10.65x |
| mnlogit_n100000 | 81.606 | 731.893 | 8.97x | 81.696 | 1421.772 | 17.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.787 | 49.618 | 27.76x | 1.787 | 49.617 | 27.76x |
| liml_robust_n1000 | 1.957 | 49.724 | 25.41x | 1.957 | 49.722 | 25.41x |
| gmm_robust_n1000 | 1.811 | 48.929 | 27.02x | 1.811 | 48.927 | 27.02x |
| 2sls_kernel_n1000 | 10.027 | 52.425 | 5.23x | 10.027 | 52.423 | 5.23x |
| 2sls_clustered_n1000 | 1.726 | 51.608 | 29.90x | 1.726 | 51.606 | 29.90x |
| 2sls_robust_n10000 | 17.693 | 219.589 | 12.41x | 17.930 | 439.074 | 24.49x |
| liml_robust_n10000 | 19.744 | 221.707 | 11.23x | 20.151 | 443.328 | 22.00x |
| gmm_robust_n10000 | 18.162 | 217.575 | 11.98x | 18.473 | 434.757 | 23.53x |
| 2sls_kernel_n10000 | 107.913 | 235.895 | 2.19x | 108.257 | 471.626 | 4.36x |
| 2sls_clustered_n10000 | 17.179 | 234.753 | 13.67x | 17.362 | 469.486 | 27.04x |
| 2sls_robust_n100000 | 195.854 | 2749.671 | 14.04x | 200.109 | 8429.050 | 42.12x |
| liml_robust_n100000 | 206.799 | 2833.069 | 13.70x | 210.024 | 8519.852 | 40.57x |
| gmm_robust_n100000 | 199.634 | 2680.101 | 13.43x | 204.135 | 8048.302 | 39.43x |
| 2sls_kernel_n100000 | 3963.376 | 3953.757 | 1.00x | 3965.884 | 11242.833 | 2.83x |
| 2sls_clustered_n100000 | 193.105 | 2986.280 | 15.46x | 195.057 | 9343.145 | 47.90x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.598 | 0.878 | 0.34x | 2.597 | 0.878 | 0.34x |
| transform_frobenius_n500 | 8.110 | 1.835 | 0.23x | 8.110 | 1.835 | 0.23x |
| transform_frobenius_n2000 | 29.040 | 6.516 | 0.22x | 29.086 | 25.997 | 0.89x |
| transform_kl_n100 | 1.293 | 14.689 | 11.36x | 1.293 | 14.689 | 11.36x |
| transform_kl_n500 | 5.899 | 31.127 | 5.28x | 5.899 | 31.126 | 5.28x |
| transform_kl_n2000 | 23.440 | 124.031 | 5.29x | 23.492 | 408.673 | 17.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.093 | 2.722 | 29.16x | 0.093 | 2.722 | 29.17x |
| ols_hac_n1000 | 0.192 | 3.292 | 17.12x | 0.192 | 3.292 | 17.12x |
| ols_cluster_n1000 | 0.072 | 3.335 | 46.61x | 0.072 | 3.335 | 46.62x |
| ols_summary_n10000 | 0.524 | 11.120 | 21.24x | 0.524 | 11.120 | 21.24x |
| ols_hac_n10000 | 1.959 | 12.555 | 6.41x | 1.962 | 12.555 | 6.40x |
| ols_cluster_n10000 | 0.678 | 13.275 | 19.58x | 0.679 | 13.274 | 19.54x |
| ols_summary_n100000 | 4.980 | 140.169 | 28.15x | 4.996 | 556.325 | 111.36x |
| ols_hac_n100000 | 20.231 | 154.256 | 7.62x | 20.617 | 607.727 | 29.48x |
| ols_cluster_n100000 | 7.839 | 157.637 | 20.11x | 8.045 | 621.183 | 77.22x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.773 | 4.108 | 2.32x | 1.776 | 4.108 | 2.31x |
| sparse_infrequent_10000 | 2.195 | 4.361 | 1.99x | 2.198 | 4.361 | 1.98x |
| sparse_100000 | 17.653 | 29.790 | 1.69x | 17.948 | 29.788 | 1.66x |
| sparse_infrequent_100000 | 19.129 | 25.237 | 1.32x | 19.464 | 25.237 | 1.30x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4', 'pandas': '2.3.3'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.277 | 18.473 | 66.61x | 0.301 | 18.472 | 61.40x |
| fe_twoway_clustered_n1000 | 0.459 | 29.719 | 64.75x | 0.459 | 29.716 | 64.75x |
| fe_kernel_n1000 | 0.292 | 19.123 | 65.60x | 0.291 | 19.122 | 65.60x |
| between_robust_n1000 | 0.165 | 17.651 | 106.94x | 0.165 | 17.648 | 106.93x |
| first_difference_robust_n1000 | 0.318 | 21.273 | 66.97x | 0.318 | 21.271 | 66.97x |
| random_effects_n1000 | 0.288 | 24.222 | 84.25x | 0.287 | 24.222 | 84.25x |
| fe_entity_n10000 | 2.826 | 27.803 | 9.84x | 2.908 | 27.802 | 9.56x |
| fe_twoway_clustered_n10000 | 4.872 | 52.670 | 10.81x | 5.067 | 105.350 | 20.79x |
| fe_kernel_n10000 | 3.012 | 28.657 | 9.51x | 3.054 | 28.655 | 9.38x |
| between_robust_n10000 | 1.883 | 26.734 | 14.20x | 1.885 | 26.731 | 14.18x |
| first_difference_robust_n10000 | 3.265 | 31.641 | 9.69x | 3.283 | 31.639 | 9.64x |
| random_effects_n10000 | 2.913 | 37.321 | 12.81x | 2.945 | 37.304 | 12.67x |
| fe_entity_n100000 | 33.630 | 205.245 | 6.10x | 35.080 | 819.883 | 23.37x |
| fe_twoway_clustered_n100000 | 54.602 | 385.065 | 7.05x | 56.410 | 1366.523 | 24.22x |
| fe_kernel_n100000 | 36.421 | 196.807 | 5.40x | 37.250 | 784.278 | 21.05x |
| between_robust_n100000 | 20.091 | 165.626 | 8.24x | 20.329 | 492.397 | 24.22x |
| first_difference_robust_n100000 | 37.308 | 219.688 | 5.89x | 38.583 | 815.164 | 21.13x |
| random_effects_n100000 | 33.932 | 251.640 | 7.42x | 35.071 | 955.514 | 27.24x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 0.728 | 76.296 | 104.83x | 0.728 | 304.913 | 418.97x |
| fit_penalised_weighted_1000 | 0.726 | 82.569 | 113.74x | 0.726 | 327.531 | 451.11x |
| predict_survival_1000 | 0.797 | 2.129 | 2.67x | 0.800 | 2.128 | 2.66x |
| fit_10000 | 7.879 | 692.339 | 87.87x | 8.433 | 1034.236 | 122.64x |
| fit_penalised_weighted_10000 | 7.999 | 714.382 | 89.31x | 8.559 | 1036.081 | 121.05x |
| predict_survival_10000 | 3.359 | 3.293 | 0.98x | 3.509 | 13.149 | 3.75x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.025 | 12.012 | 474.62x | 0.025 | 12.012 | 474.64x |
| logrank_fleming_harrington_1000 | 0.036 | 18.479 | 518.33x | 0.036 | 18.478 | 518.24x |
| multigroup_1000 | 0.118 | 24.687 | 209.73x | 0.118 | 24.683 | 209.71x |
| pairwise_1000 | 0.345 | 125.728 | 364.86x | 0.345 | 125.719 | 364.85x |
| rmst_1000 | 0.103 | 1080.450 | 10464.06x | 0.103 | 1080.425 | 10463.88x |
| fixed_point_1000 | 0.043 | 13.765 | 317.72x | 0.043 | 13.764 | 317.71x |
| concordance_1000 | 0.130 | 7.569 | 58.33x | 0.130 | 7.569 | 58.33x |
| logrank_wilcoxon_weighted_10000 | 0.365 | 13.919 | 38.10x | 0.365 | 13.919 | 38.10x |
| logrank_fleming_harrington_10000 | 0.465 | 22.718 | 48.81x | 0.465 | 22.716 | 48.81x |
| multigroup_10000 | 2.270 | 37.229 | 16.40x | 2.273 | 37.228 | 16.38x |
| pairwise_10000 | 4.292 | 145.243 | 33.84x | 4.292 | 145.234 | 33.84x |
| rmst_10000 | 1.827 | 1215.183 | 665.13x | 1.829 | 1215.137 | 664.47x |
| fixed_point_10000 | 0.524 | 16.369 | 31.27x | 0.523 | 16.368 | 31.27x |
| concordance_10000 | 2.241 | 88.960 | 39.70x | 2.241 | 88.952 | 39.70x |
| logrank_wilcoxon_weighted_100000 | 3.941 | 21.988 | 5.58x | 4.062 | 21.986 | 5.41x |
| logrank_fleming_harrington_100000 | 3.820 | 35.830 | 9.38x | 3.877 | 35.822 | 9.24x |
| multigroup_100000 | 9.880 | 50.863 | 5.15x | 10.004 | 50.861 | 5.08x |
| pairwise_100000 | 39.703 | 225.202 | 5.67x | 40.681 | 225.121 | 5.53x |
| rmst_100000 | 8.327 | 445.781 | 53.53x | 8.440 | 445.763 | 52.82x |
| fixed_point_100000 | 4.734 | 29.560 | 6.24x | 4.922 | 29.535 | 6.00x |
| concordance_100000 | 27.550 | 712.368 | 25.86x | 27.729 | 712.320 | 25.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.570 | 67.600 | 18.93x | 3.570 | 67.599 | 18.93x |
| loglogistic_left_1000 | 9.744 | 95.945 | 9.85x | 9.855 | 95.946 | 9.74x |
| lognormal_interval_1000 | 3.347 | 181.758 | 54.30x | 3.348 | 181.747 | 54.29x |
| generalized_gamma_1000 | 13.317 | 847.569 | 63.65x | 13.316 | 847.537 | 63.65x |
| weibull_aft_1000 | 7.500 | 128.000 | 17.07x | 7.499 | 127.990 | 17.07x |
| lognormal_aft_ridge_robust_1000 | 9.506 | 4024.046 | 423.30x | 9.602 | 4023.788 | 419.06x |
| loglogistic_aft_interval_1000 | 10.461 | 193.657 | 18.51x | 10.460 | 193.638 | 18.51x |
| breslow_fleming_harrington_1000 | 0.284 | 6.055 | 21.35x | 0.284 | 6.054 | 21.35x |
| kaplan_meier_left_1000 | 0.097 | 7.595 | 78.67x | 0.097 | 7.594 | 78.66x |
| weibull_10000 | 36.450 | 79.095 | 2.17x | 36.449 | 79.090 | 2.17x |
| loglogistic_left_10000 | 110.998 | 170.182 | 1.53x | 111.007 | 170.172 | 1.53x |
| lognormal_interval_10000 | 32.045 | 343.449 | 10.72x | 32.043 | 343.440 | 10.72x |
| generalized_gamma_10000 | 111.790 | 1965.315 | 17.58x | 111.783 | 1965.187 | 17.58x |
| weibull_aft_10000 | 76.632 | 139.961 | 1.83x | 77.012 | 139.954 | 1.82x |
| lognormal_aft_ridge_robust_10000 | 132.218 | 38384.164 | 290.31x | 132.210 | 38381.966 | 290.31x |
| loglogistic_aft_interval_10000 | 98.856 | 267.547 | 2.71x | 98.887 | 267.540 | 2.71x |
| breslow_fleming_harrington_10000 | 2.759 | 7.801 | 2.83x | 2.759 | 7.801 | 2.83x |
| kaplan_meier_left_10000 | 0.962 | 9.359 | 9.72x | 0.962 | 9.359 | 9.72x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.065 | 2.541 | 38.84x | 0.065 | 2.541 | 38.84x |
| var_n10000 | 0.735 | 15.255 | 20.76x | 0.737 | 15.253 | 20.69x |
| var_n100000 | 7.233 | 143.309 | 19.81x | 7.375 | 143.304 | 19.43x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `ClusteringAgreementBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `EmbeddingSearchBenchmarks`
- `GlmBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `KsExactTableBenchmarks`
- `MannWhitneyExactBenchmarks`
- `MathNetInteropBenchmarks`
- `MultinomialLogitBenchmarks`
- `OlsBenchmarks`
- `OsaBenchmarks`
- `QgramBenchmarks`
- `QuantileBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RobustCovarianceBenchmarks`
- `RobustScalerSparseBenchmarks`
- `SerialCorrelationBenchmarks`
- `SilhouetteBenchmarks`
- `TextRankBenchmarks`
- `TopKAccuracyBenchmarks`
- `VectorAutoregressionBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 13.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Discretize_Fit` | step | 10.4 | 4.12 | +152% | 30% |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Quantile_Fit` | step | 19.24 | 12.95 | +49% | 30% |
| `StatsBenchmarks` | SampleSize=10000 | `LodestarMannWhitney` | step | 7.381 | 13.076 | -44% | 30% |
| `ScalerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=20000 | `Lodestar_MaxAbs_SparseFit` | step | 0.24 | 0.17 | +41% | 30% |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=20000 | `Lodestar_Quantile` | step | 22.6 | 16.04 | +41% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=100000 | `MlNet_CrossValidationSplit` | step | 0.35 | 0.25 | +40% | 35% |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Quantile` | step | 44.89 | 33.13 | +35% | 30% |
| `ScalerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=20000 | `Lodestar_Standard_SparseFit` | step | 0.29 | 0.215 | +35% | 30% |
| `StationarityBenchmarks` | SampleSize=2000 | `LodestarDecompose` | step | 0.16 | 0.12 | +33% | 31% |
| `TransformerIncumbentBenchmarks` | Job=ShortRun, InvocationCount=Default, IterationCount=3, RunStrategy=Default, UnrollFactor=16, WarmupCount=3, RowCount=1000 | `Lodestar_Discretize` | step | 6.36 | 9.52 | -33% | 30% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `MlNet_CrossValidationSplit_Read` | drift | 58.24 | 75.01 | -22% | 20% |
| `SplitterIncumbentBenchmarks` | SampleCount=10000 | `Lodestar_StratifiedKFold` | drift | 1.84 | 2.35 | -22% | 20% |
| `BitParallelEditDistanceBenchmarks` | Texts=200000, TextLength=32, Device=CPUAc(...) GPU) [26] | `GpuResident` | drift | 1.01 | 0.83 | +22% | 20% |

**Still away from their median: 33.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `DistributionTailBenchmarks` | — | `ChiSquaredSfFractionalDf` | step | 8.83 | 4.09 | +116% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredCdf` | drift | 0.015 | 0.165 | -90% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalQuantile` | drift | 0.012 | 0.1 | -88% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredCdf` | drift | 0.028 | 0.23 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherCdf` | drift | 0.047 | 0.42 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredCdf` | drift | 0.021 | 0.19 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherCdf` | drift | 0.049 | 0.435 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherQuantile` | drift | 0.124 | 1.005 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredQuantile` | drift | 0.172 | 1.49 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredQuantile` | drift | 0.153 | 1.225 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherQuantile` | drift | 0.904 | 8.35 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalCdf` | drift | 0.008 | 0.07 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalQuantile` | drift | 0.064 | 0.565 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentCdf` | drift | 0.118 | 0.94 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentCdf` | drift | 0.139 | 1.07 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentQuantile` | drift | 2.083 | 17.7 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentQuantile` | drift | 0.494 | 3.745 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalQuantile` | drift | 0.011 | 0.09 | -87% | 20% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalCdf` | drift | 0.023 | 0.2 | -86% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredQuantile` | drift | 0.136 | 1.065 | -86% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalCdf` | drift | 0.009 | 0.07 | -86% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `AccordWelchT` | drift | 5.05 | 33.47 | -81% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `AccordMannWhitney` | drift | 3.89 | 20.44 | -81% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarMannWhitney` | drift | 0.39 | 1.89 | -81% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarChiSquare` | drift | 0.02 | 0.1 | -80% | 20% |
| `StatsBenchmarks` | SampleSize=100 | `AccordChiSquare` | drift | 0.03 | 0.19 | -79% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_FisherQuantile` | drift | 1.298 | 0.69 | +67% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_StudentQuantile` | drift | 3.077 | 2.23 | +38% | 20% |
| `Bm25Benchmarks` | Documents=20000 | `LuceneQuery` | drift | 0.73 | 0.78 | -28% | 20% |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | drift | 8.01 | 11.27 | -27% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `FingerprintsOnly` | drift | 0.04 | 0.04 | +25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `SketchThenVerify` | drift | 0.04 | 0.05 | -20% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `SignaturesOnly` | drift | 0.04 | 0.05 | -20% | 20% |
