# Nightly benchmark run

<!-- nightly-baseline: fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BkTreeNearestBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks WeightedLeastSquaresBenchmarks -->
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

- `VectorizerBenchmarks`
- `TokenPatternBenchmarks`
- `SimilaritySketchBenchmarks`
- `Bm25Benchmarks`
- `CoxBenchmarks`
- `SurvivalBenchmarks`
- `CosineTopKSelectionBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Texts  | TextLength | Device               | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.206 ms** |  **0.1401 ms** | **0.0077 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   4.417 ms |  8.6152 ms | 0.4722 ms |  2.00 |    0.19 |        - |        - |        - |    42832 B |          NA |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.191 ms |  7.6277 ms | 0.4181 ms |  2.35 |    0.16 |  93.7500 |  93.7500 |  93.7500 |   471051 B |          NA |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **10.702 ms** |  **0.0452 ms** | **0.0025 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   6.743 ms |  7.4163 ms | 0.4065 ms |  0.63 |    0.03 |        - |        - |        - |    42840 B |          NA |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |   9.600 ms |  6.6430 ms | 0.3641 ms |  0.90 |    0.03 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |          NA |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **45.753 ms** |  **0.7169 ms** | **0.0393 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  36.352 ms | 40.6641 ms | 2.2289 ms |  0.79 |    0.04 |        - |        - |        - |   802909 B |          NA |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  43.333 ms | 29.6994 ms | 1.6279 ms |  0.95 |    0.03 | 454.5455 | 454.5455 | 454.5455 |  8071853 B |          NA |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **214.959 ms** |  **0.2871 ms** | **0.0157 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 119.582 ms | 16.6947 ms | 0.9151 ms |  0.56 |    0.00 |        - |        - |        - |   803029 B |          NA |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 193.391 ms | 65.4286 ms | 3.5864 ms |  0.90 |    0.01 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,439.671 ms** |   **183.1393 ms** |  **10.0385 ms** | **1.000** |    **0.01** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,594.555 ms | 2,073.4169 ms | 113.6510 ms | 1.108 |    0.07 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.140 ms |     0.0579 ms |   0.0032 ms | 0.001 |    0.00 |   308.5938 |   308.5938 |   308.5938 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **6,068.606 ms** | **4,562.5821 ms** | **250.0905 ms** | **1.001** |    **0.05** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **37.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  6,220.553 ms | 1,602.5821 ms |  87.8429 ms | 1.026 |    0.04 |  1000.0000 |  1000.0000 |  1000.0000 |  35.45 MB |        0.94 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      5.128 ms |     0.6662 ms |   0.0365 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.15 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **6,065.032 ms** | **1,688.9975 ms** |  **92.5797 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  6,111.526 ms | 1,540.3258 ms |  84.4305 ms | 1.008 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  42.21 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      6.203 ms |     2.0169 ms |   0.1106 ms | 0.001 |    0.00 |   390.6250 |   390.6250 |   390.6250 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **25,113.270 ms** | **2,402.8009 ms** | **131.7056 ms** | **1.000** |    **0.01** | **15000.0000** | **14000.0000** | **13000.0000** | **388.64 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 25,186.220 ms | 3,449.3382 ms | 189.0699 ms | 1.003 |    0.01 | 20000.0000 | 19000.0000 | 18000.0000 | 384.96 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     12.416 ms |     5.9356 ms |   0.3254 ms | 0.000 |    0.00 |   484.3750 |   484.3750 |   484.3750 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.CosineTopKSelectionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Documents | K    | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Allocated   | Alloc Ratio |
|------------- |---------- |----- |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|------------:|------------:|
| **SimdBaseline** | **10000**     | **10**   | **CPUAc(...) GPU) [26]** |       **7.007 ms** |      **0.5991 ms** |     **0.0328 ms** |     **1.00** |    **0.01** |    **26.01 KB** |        **1.00** |
| GpuResident  | 10000     | 10   | CPUAc(...) GPU) [26] |  23,185.352 ms |  3,317.0949 ms |   181.8212 ms | 3,309.16 |   26.17 |  1519.13 KB |       58.41 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **100**  | **CPUAc(...) GPU) [26]** |       **7.494 ms** |      **0.0707 ms** |     **0.0039 ms** |     **1.00** |    **0.00** |    **37.26 KB** |        **1.00** |
| GpuResident  | 10000     | 100  | CPUAc(...) GPU) [26] |  24,372.652 ms | 13,636.7450 ms |   747.4760 ms | 3,252.10 |   86.39 |  1541.63 KB |       41.38 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **1000** | **CPUAc(...) GPU) [26]** |      **11.598 ms** |      **0.1826 ms** |     **0.0100 ms** |     **1.00** |    **0.00** |   **149.77 KB** |        **1.00** |
| GpuResident  | 10000     | 1000 | CPUAc(...) GPU) [26] |  27,127.554 ms |  1,756.8404 ms |    96.2984 ms | 2,338.93 |    7.40 |   1765.7 KB |       11.79 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **10**   | **CPUAc(...) GPU) [26]** |      **83.646 ms** |     **47.5531 ms** |     **2.6065 ms** |     **1.00** |    **0.04** |    **26.14 KB** |        **1.00** |
| GpuResident  | 100000    | 10   | CPUAc(...) GPU) [26] | 227,672.594 ms | 44,967.3860 ms | 2,464.8142 ms | 2,723.60 |   77.17 | 39277.98 KB |    1,502.45 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **100**  | **CPUAc(...) GPU) [26]** |      **77.132 ms** |      **3.5019 ms** |     **0.1920 ms** |     **1.00** |    **0.00** |    **37.39 KB** |        **1.00** |
| GpuResident  | 100000    | 100  | CPUAc(...) GPU) [26] | 228,960.147 ms | 35,223.1903 ms | 1,930.7019 ms | 2,968.44 |   22.60 | 39300.76 KB |    1,051.03 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **1000** | **CPUAc(...) GPU) [26]** |      **83.152 ms** |      **3.9236 ms** |     **0.2151 ms** |     **1.00** |    **0.00** |   **149.92 KB** |        **1.00** |
| GpuResident  | 100000    | 1000 | CPUAc(...) GPU) [26] | 233,960.983 ms | 12,308.2118 ms |   674.6546 ms | 2,813.66 |    9.44 |  39526.7 KB |      263.66 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **279.5 μs** |   **9.03 μs** |  **0.50 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,596.5 μs** |  **71.92 μs** |  **3.94 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **4,558.9 μs** | **157.80 μs** |  **8.65 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **16,846.1 μs** | **589.49 μs** | **32.31 μs** | **281.2500** | **250.0000** | **250.0000** | **1735.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **12.75 μs** |     **0.485 μs** |   **0.027 μs** |  **1.00** |    **0.00** |    **0.9766** |    **0.0153** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     11.58 μs |     0.486 μs |   0.027 μs |  0.91 |    0.00 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     16.75 μs |     1.029 μs |   0.056 μs |  1.31 |    0.00 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **47.96 μs** |     **5.181 μs** |   **0.284 μs** |  **1.00** |    **0.01** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     19.38 μs |    11.252 μs |   0.617 μs |  0.40 |    0.01 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     36.81 μs |     3.993 μs |   0.219 μs |  0.77 |    0.01 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **7,656.79 μs** |   **170.683 μs** |   **9.356 μs** |  **1.00** |    **0.00** |  **335.9375** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  7,480.87 μs |   211.218 μs |  11.578 μs |  0.98 |    0.00 |  335.9375 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  8,629.94 μs |   526.372 μs |  28.852 μs |  1.13 |    0.00 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **13,342.11 μs** | **5,034.953 μs** | **275.983 μs** |  **1.00** |    **0.03** | **1125.0000** | **1078.1250** | **968.7500** | **6650.01 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 10,743.61 μs | 3,140.923 μs | 172.165 μs |  0.81 |    0.02 |  875.0000 |  859.3750 | 718.7500 | 5660.25 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 13,677.94 μs |   168.087 μs |   9.213 μs |  1.03 |    0.02 |  500.0000 |  500.0000 | 500.0000 | 4916.38 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error          | StdDev      | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.587 μs** |      **0.1494 μs** |   **0.0082 μs** |     **1.00** |    **0.01** |    **0.0248** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.168 μs |      0.3979 μs |   0.0218 μs |     1.37 |    0.01 |    0.0229 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       2.519 μs |      0.3591 μs |   0.0197 μs |     1.59 |    0.01 |    0.3128 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   2,933.817 μs |    530.7547 μs |  29.0925 μs | 1,848.83 |   17.89 |  914.0625 |  890.6250 |  882.8125 |  3723446 B |    8,781.71 |
| LuceneFromText         | 1000      |   5,806.540 μs |    309.7757 μs |  16.9799 μs | 3,659.16 |   18.76 |   85.9375 |   78.1250 |    7.8125 |  1375080 B |    3,243.11 |
|                        |           |                |                |             |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **20.922 μs** |     **29.9984 μs** |   **1.6443 μs** |     **1.00** |    **0.10** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      29.665 μs |      1.0083 μs |   0.0553 μs |     1.42 |    0.10 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      14.199 μs |      0.4799 μs |   0.0263 μs |     0.68 |    0.05 |    0.5035 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  56,228.574 μs |  5,734.7029 μs | 314.3384 μs | 2,699.14 |  192.86 | 1666.6667 | 1666.6667 | 1666.6667 | 61663525 B |  145,432.84 |
| LuceneFromText         | 20000     | 114,813.689 μs | 16,313.0509 μs | 894.1734 μs | 5,511.40 |  394.65 | 1200.0000 | 1000.0000 |         - | 22417093 B |   52,870.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|------------:|-----------:|------:|--------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **41.758 ms** |   **1.0502 ms** |  **0.0576 ms** |  **1.00** |    **0.00** |  **416.6667** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   8.632 ms |   1.1539 ms |  0.0632 ms |  0.21 |    0.00 |  171.8750 |  156.2500 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   6.817 ms |   0.2308 ms |  0.0127 ms |  0.16 |    0.00 |   15.6250 |         - |        - |    304.73 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   6.496 ms |   0.5918 ms |  0.0324 ms |  0.16 |    0.00 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   8.168 ms |   0.6065 ms |  0.0332 ms |  0.20 |    0.00 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **43.496 ms** |   **4.6141 ms** |  **0.2529 ms** |  **1.00** |    **0.01** |  **416.6667** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  11.287 ms |   0.6029 ms |  0.0330 ms |  0.26 |    0.00 |  328.1250 |  312.5000 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   8.222 ms |   0.9068 ms |  0.0497 ms |  0.19 |    0.00 |   15.6250 |         - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   5.405 ms |   0.8045 ms |  0.0441 ms |  0.12 |    0.00 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   8.242 ms |   2.8085 ms |  0.1539 ms |  0.19 |    0.00 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **759.450 ms** | **145.6830 ms** |  **7.9854 ms** |  **1.00** |    **0.01** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  36.862 ms |   0.6983 ms |  0.0383 ms |  0.05 |    0.00 |  928.5714 |  928.5714 | 500.0000 |  10335.83 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  31.703 ms |   3.0283 ms |  0.1660 ms |  0.04 |    0.00 |   62.5000 |         - |        - |   1218.83 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  21.370 ms |   1.5878 ms |  0.0870 ms |  0.03 |    0.00 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  32.528 ms |   1.2248 ms |  0.0671 ms |  0.04 |    0.00 |   62.5000 |         - |        - |   1718.75 KB |       0.014 |
|                      |           |              |            |             |            |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **771.191 ms** | **261.0651 ms** | **14.3099 ms** |  **1.00** |    **0.02** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  58.362 ms |  17.0622 ms |  0.9352 ms |  0.08 |    0.00 | 1555.5556 | 1555.5556 | 888.8889 |  18665.05 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  37.274 ms |   0.2958 ms |  0.0162 ms |  0.05 |    0.00 |   71.4286 |         - |        - |   1718.84 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  21.806 ms |   0.3299 ms |  0.0181 ms |  0.03 |    0.00 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  37.374 ms |   0.3459 ms |  0.0190 ms |  0.05 |    0.00 |   71.4286 |         - |        - |    1718.8 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean            | Error           | StdDev        | Gen0     | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |----------------:|----------------:|--------------:|---------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **28,303,004.5 ns** |   **524,638.13 ns** |  **28,757.19 ns** |  **62.5000** | **31.2500** |       **-** | **1554.58 KB** |
| Construct                  | [^ ]+         |      1,494.5 ns |        59.73 ns |       3.27 ns |   0.3319 |  0.0019 |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      2,495.1 ns |       268.26 ns |      14.70 ns |   0.4539 |  0.0038 |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **24,073,506.6 ns** |   **648,576.53 ns** |  **35,550.67 ns** |        **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1,442.0 ns |       403.00 ns |      22.09 ns |   0.3300 |  0.0019 |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2,173.4 ns |       278.54 ns |      15.27 ns |   0.4387 |  0.0038 |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **32,385,570.6 ns** |   **750,415.47 ns** |  **41,132.80 ns** |  **62.5000** |       **-** |       **-** | **1555.98 KB** |
| Construct                  | [A-Za-z]\\w+  |      2,687.4 ns |       524.72 ns |      28.76 ns |   0.4158 |  0.0038 |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      3,624.1 ns |     1,070.47 ns |      58.68 ns |   0.5188 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **16,515,674.2 ns** |   **775,166.63 ns** |  **42,489.50 ns** | **125.0000** | **93.7500** | **62.5000** | **9743.56 KB** |
| Construct                  | \\b\\w\\w+\\b |        831.7 ns |        57.52 ns |       3.15 ns |   0.1135 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1,536.9 ns |        73.77 ns |       4.04 ns |   0.2346 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **29,176,210.6 ns** | **2,321,165.15 ns** | **127,230.89 ns** |  **62.5000** | **31.2500** |       **-** | **1554.45 KB** |
| Construct                  | \\S+          |      1,639.6 ns |        86.08 ns |       4.72 ns |   0.3223 |  0.0019 |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      2,581.1 ns |       305.30 ns |      16.73 ns |   0.4425 |  0.0038 |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **152.2 μs** |     **9.32 μs** |   **0.51 μs** |  **1.00** |    **0.00** |   **12.4512** |    **2.1973** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    168.3 μs |    19.25 μs |   1.06 μs |  1.11 |    0.01 |   14.1602 |    3.4180 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    534.8 μs |    55.51 μs |   3.04 μs |  3.51 |    0.02 |   35.1563 |   12.6953 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  1,679.6 μs |    24.14 μs |   1.32 μs | 11.04 |    0.03 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    150.0 μs |     9.68 μs |   0.53 μs |  0.99 |    0.00 |    8.3008 |    0.9766 |         - |   136.13 KB |        0.66 |
|                       |           |             |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,267.8 μs** |    **51.66 μs** |   **2.83 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,417.5 μs |   280.36 μs |  15.37 μs |  1.12 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,085.2 μs | 1,477.56 μs |  80.99 μs |  2.43 |    0.06 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 10,970.5 μs | 1,905.12 μs | 104.43 μs |  8.65 |    0.07 | 1046.8750 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,352.5 μs |   571.10 μs |  31.30 μs |  1.07 |    0.02 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

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
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 4.277 | 3.018 | 0.71x | 4.282 | 6.033 | 1.41x |
| kmeans_restarts_10000 | 17.373 | 7.946 | 0.46x | 17.397 | 15.858 | 0.91x |
| kmeans_weighted_100000 | 41.581 | 22.277 | 0.54x | 42.016 | 39.605 | 0.94x |
| kmeans_restarts_100000 | 166.265 | 58.270 | 0.35x | 166.834 | 109.896 | 0.66x |
| dbscan_weighted_5000 | 6.539 | 25.343 | 3.88x | 6.978 | 25.342 | 3.63x |
| dbscan_weighted_20000 | 77.683 | 163.033 | 2.10x | 79.190 | 163.024 | 2.06x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.638 | 83.142 | 130.28x | 0.638 | 83.137 | 130.28x |
| ridge_robust_1000 | 0.710 | 78.467 | 110.45x | 0.710 | 78.465 | 110.45x |
| lasso_1000 | 5.197 | 716.112 | 137.80x | 5.197 | 716.040 | 137.79x |
| ph_test_1000 | 0.097 | 14.619 | 151.24x | 0.097 | 14.618 | 151.24x |
| predict_survival_1000 | 0.025 | 1.221 | 48.37x | 0.025 | 1.221 | 48.37x |
| time_varying_1000 | 57.179 | 227.449 | 3.98x | 57.175 | 227.423 | 3.98x |
| stratified_weighted_10000 | 7.346 | 466.554 | 63.51x | 7.379 | 466.524 | 63.23x |
| ridge_robust_10000 | 8.160 | 805.342 | 98.69x | 8.326 | 805.244 | 96.71x |
| lasso_10000 | 48.473 | 4727.613 | 97.53x | 48.497 | 4727.453 | 97.48x |
| ph_test_10000 | 1.667 | 154.944 | 92.97x | 1.720 | 154.939 | 90.06x |
| predict_survival_10000 | 0.027 | 1.225 | 45.55x | 0.027 | 1.225 | 45.55x |
| time_varying_10000 | 2182.482 | 3753.357 | 1.72x | 2182.393 | 3753.213 | 1.72x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 3.344 | 30.908 | 9.24x | 3.344 | 122.966 | 36.77x |
| cv_minmax_n1000 | 2.563 | 4.421 | 1.72x | 2.563 | 17.649 | 6.89x |
| cv_plus_gamma_n1000 | 3.941 | 31.489 | 7.99x | 3.941 | 125.821 | 31.92x |
| bootstrap_plus_n1000 | 12.683 | 31.454 | 2.48x | 12.685 | 125.554 | 9.90x |
| cv_plus_n10000 | 32.351 | 93.432 | 2.89x | 32.351 | 372.948 | 11.53x |
| cv_minmax_n10000 | 27.137 | 20.361 | 0.75x | 27.135 | 81.273 | 3.00x |
| cv_plus_gamma_n10000 | 47.534 | 101.201 | 2.13x | 47.532 | 403.770 | 8.49x |
| bootstrap_plus_n10000 | 115.256 | 95.549 | 0.83x | 115.249 | 380.968 | 3.31x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.299 | 2.261 | 7.55x | 0.299 | 2.261 | 7.55x |
| glm_gamma_n1000 | 0.359 | 3.021 | 8.42x | 0.359 | 3.020 | 8.42x |
| glm_poisson_exposure_n1000 | 0.300 | 2.623 | 8.73x | 0.300 | 2.623 | 8.73x |
| mnlogit_n1000 | 0.642 | 10.533 | 16.42x | 0.641 | 10.532 | 16.42x |
| glm_negative_binomial_n10000 | 2.694 | 8.048 | 2.99x | 2.700 | 8.047 | 2.98x |
| glm_gamma_n10000 | 3.045 | 9.494 | 3.12x | 3.051 | 9.493 | 3.11x |
| glm_poisson_exposure_n10000 | 3.225 | 9.182 | 2.85x | 3.231 | 9.182 | 2.84x |
| mnlogit_n10000 | 6.443 | 56.132 | 8.71x | 6.448 | 56.127 | 8.70x |
| glm_negative_binomial_n100000 | 28.039 | 74.124 | 2.64x | 28.493 | 295.747 | 10.38x |
| glm_gamma_n100000 | 32.202 | 87.638 | 2.72x | 32.527 | 349.957 | 10.76x |
| glm_poisson_exposure_n100000 | 41.481 | 81.301 | 1.96x | 42.087 | 324.676 | 7.71x |
| mnlogit_n100000 | 64.968 | 525.853 | 8.09x | 65.004 | 1107.892 | 17.04x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-iv

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.332 | 39.138 | 29.37x | 1.332 | 39.133 | 29.37x |
| liml_robust_n1000 | 1.458 | 39.263 | 26.94x | 1.457 | 39.260 | 26.94x |
| gmm_robust_n1000 | 1.342 | 38.801 | 28.91x | 1.342 | 38.798 | 28.91x |
| 2sls_kernel_n1000 | 7.295 | 41.115 | 5.64x | 7.295 | 41.111 | 5.64x |
| 2sls_clustered_n1000 | 1.294 | 40.349 | 31.18x | 1.294 | 40.344 | 31.17x |
| 2sls_robust_n10000 | 13.783 | 183.464 | 13.31x | 14.119 | 366.700 | 25.97x |
| liml_robust_n10000 | 15.073 | 184.899 | 12.27x | 15.474 | 369.641 | 23.89x |
| gmm_robust_n10000 | 13.778 | 178.419 | 12.95x | 14.100 | 356.654 | 25.29x |
| 2sls_kernel_n10000 | 77.891 | 192.617 | 2.47x | 78.213 | 385.353 | 4.93x |
| 2sls_clustered_n10000 | 13.191 | 199.319 | 15.11x | 13.493 | 398.562 | 29.54x |
| 2sls_robust_n100000 | 151.411 | 1947.008 | 12.86x | 153.697 | 6361.652 | 41.39x |
| liml_robust_n100000 | 165.153 | 1974.396 | 11.95x | 169.349 | 6441.831 | 38.04x |
| gmm_robust_n100000 | 153.234 | 1899.290 | 12.39x | 155.692 | 6127.808 | 39.36x |
| 2sls_kernel_n100000 | 2898.043 | 2716.147 | 0.94x | 2899.524 | 8631.507 | 2.98x |
| 2sls_clustered_n100000 | 143.446 | 2101.436 | 14.65x | 146.786 | 6999.952 | 47.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 1.908 | 0.537 | 0.28x | 1.908 | 0.537 | 0.28x |
| transform_frobenius_n500 | 5.644 | 1.019 | 0.18x | 5.644 | 1.019 | 0.18x |
| transform_frobenius_n2000 | 19.428 | 3.337 | 0.17x | 19.470 | 13.326 | 0.68x |
| transform_kl_n100 | 0.890 | 11.067 | 12.44x | 0.890 | 11.067 | 12.44x |
| transform_kl_n500 | 3.969 | 23.590 | 5.94x | 3.969 | 23.589 | 5.94x |
| transform_kl_n2000 | 15.744 | 90.914 | 5.77x | 15.795 | 363.285 | 23.00x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.071 | 1.929 | 27.06x | 0.071 | 1.929 | 27.06x |
| ols_hac_n1000 | 0.134 | 2.370 | 17.62x | 0.134 | 2.369 | 17.62x |
| ols_cluster_n1000 | 0.051 | 2.422 | 47.94x | 0.051 | 2.422 | 47.94x |
| ols_summary_n10000 | 0.379 | 8.838 | 23.34x | 0.379 | 8.837 | 23.33x |
| ols_hac_n10000 | 1.381 | 9.769 | 7.08x | 1.386 | 9.769 | 7.05x |
| ols_cluster_n10000 | 0.499 | 10.101 | 20.22x | 0.502 | 10.100 | 20.12x |
| ols_summary_n100000 | 3.677 | 82.727 | 22.50x | 3.692 | 330.394 | 89.48x |
| ols_hac_n100000 | 14.359 | 92.827 | 6.46x | 14.691 | 370.828 | 25.24x |
| ols_cluster_n100000 | 5.768 | 94.871 | 16.45x | 6.005 | 378.571 | 63.05x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.413 | 3.352 | 2.37x | 1.417 | 3.352 | 2.37x |
| sparse_infrequent_10000 | 1.784 | 4.107 | 2.30x | 1.788 | 4.107 | 2.30x |
| sparse_100000 | 14.248 | 24.381 | 1.71x | 14.552 | 24.379 | 1.68x |
| sparse_infrequent_100000 | 16.078 | 29.179 | 1.81x | 16.404 | 29.178 | 1.78x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3', 'pandas': '2.3.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.202 | 13.780 | 68.15x | 0.202 | 13.779 | 68.15x |
| fe_twoway_clustered_n1000 | 0.333 | 21.522 | 64.59x | 0.333 | 21.522 | 64.60x |
| fe_kernel_n1000 | 0.215 | 14.156 | 65.81x | 0.215 | 14.156 | 65.81x |
| between_robust_n1000 | 0.121 | 13.313 | 109.90x | 0.121 | 13.312 | 109.90x |
| first_difference_robust_n1000 | 0.229 | 15.760 | 68.69x | 0.229 | 15.760 | 68.69x |
| random_effects_n1000 | 0.213 | 18.187 | 85.23x | 0.213 | 18.186 | 85.23x |
| fe_entity_n10000 | 2.146 | 21.354 | 9.95x | 2.218 | 21.353 | 9.63x |
| fe_twoway_clustered_n10000 | 3.867 | 41.167 | 10.65x | 4.085 | 82.352 | 20.16x |
| fe_kernel_n10000 | 2.335 | 21.801 | 9.33x | 2.362 | 21.799 | 9.23x |
| between_robust_n10000 | 1.454 | 20.591 | 14.16x | 1.457 | 20.590 | 14.14x |
| first_difference_robust_n10000 | 2.394 | 24.601 | 10.27x | 2.400 | 24.600 | 10.25x |
| random_effects_n10000 | 2.265 | 28.450 | 12.56x | 2.294 | 28.448 | 12.40x |
| fe_entity_n100000 | 28.247 | 134.121 | 4.75x | 30.043 | 535.679 | 17.83x |
| fe_twoway_clustered_n100000 | 41.070 | 262.730 | 6.40x | 43.152 | 1010.587 | 23.42x |
| fe_kernel_n100000 | 27.800 | 124.186 | 4.47x | 28.451 | 495.739 | 17.42x |
| between_robust_n100000 | 15.826 | 126.249 | 7.98x | 16.473 | 465.417 | 28.25x |
| first_difference_robust_n100000 | 27.426 | 142.535 | 5.20x | 28.717 | 569.488 | 19.83x |
| random_effects_n100000 | 27.162 | 167.598 | 6.17x | 28.556 | 669.352 | 23.44x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 6.883 | 49.532 | 7.20x | 6.883 | 197.417 | 28.68x |
| fit_penalised_weighted_1000 | 6.572 | 53.731 | 8.18x | 6.575 | 214.536 | 32.63x |
| predict_survival_1000 | 0.717 | 1.455 | 2.03x | 0.719 | 1.455 | 2.02x |
| fit_10000 | 195.772 | 498.089 | 2.54x | 196.025 | 846.591 | 4.32x |
| fit_penalised_weighted_10000 | 183.611 | 511.471 | 2.79x | 183.855 | 857.973 | 4.67x |
| predict_survival_10000 | 2.888 | 2.008 | 0.70x | 3.074 | 8.002 | 2.60x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.019 | 9.293 | 486.18x | 0.019 | 9.292 | 486.15x |
| logrank_fleming_harrington_1000 | 0.027 | 14.106 | 530.59x | 0.027 | 14.105 | 530.57x |
| multigroup_1000 | 0.090 | 19.155 | 212.69x | 0.090 | 19.153 | 212.68x |
| pairwise_1000 | 0.223 | 97.844 | 439.29x | 0.223 | 97.839 | 439.27x |
| rmst_1000 | 0.072 | 803.353 | 11214.44x | 0.072 | 803.313 | 11214.47x |
| fixed_point_1000 | 0.029 | 10.117 | 349.10x | 0.029 | 10.116 | 349.09x |
| concordance_1000 | 0.040 | 6.031 | 149.17x | 0.040 | 6.031 | 149.16x |
| logrank_wilcoxon_weighted_10000 | 0.203 | 10.790 | 53.27x | 0.203 | 10.789 | 53.28x |
| logrank_fleming_harrington_10000 | 0.275 | 17.548 | 63.80x | 0.275 | 17.547 | 63.79x |
| multigroup_10000 | 2.053 | 29.556 | 14.40x | 2.059 | 29.555 | 14.35x |
| pairwise_10000 | 3.833 | 112.503 | 29.35x | 3.833 | 112.498 | 29.35x |
| rmst_10000 | 1.536 | 868.429 | 565.22x | 1.567 | 868.408 | 554.27x |
| fixed_point_10000 | 0.296 | 12.202 | 41.29x | 0.296 | 12.202 | 41.28x |
| concordance_10000 | 2.049 | 72.710 | 35.49x | 2.049 | 72.708 | 35.49x |
| logrank_wilcoxon_weighted_100000 | 3.715 | 18.338 | 4.94x | 3.803 | 18.338 | 4.82x |
| logrank_fleming_harrington_100000 | 3.680 | 28.899 | 7.85x | 3.731 | 28.897 | 7.74x |
| multigroup_100000 | 9.555 | 41.391 | 4.33x | 9.734 | 41.390 | 4.25x |
| pairwise_100000 | 38.409 | 185.694 | 4.83x | 39.473 | 185.686 | 4.70x |
| rmst_100000 | 8.060 | 322.456 | 40.01x | 8.149 | 322.447 | 39.57x |
| fixed_point_100000 | 4.151 | 22.953 | 5.53x | 4.276 | 22.952 | 5.37x |
| concordance_100000 | 25.555 | 603.202 | 23.60x | 25.732 | 603.172 | 23.44x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 2.656 | 45.665 | 17.19x | 2.656 | 45.663 | 17.19x |
| loglogistic_left_1000 | 4.748 | 65.535 | 13.80x | 4.747 | 65.531 | 13.80x |
| lognormal_interval_1000 | 4.271 | 118.058 | 27.64x | 4.271 | 118.053 | 27.64x |
| generalized_gamma_1000 | 32.904 | 641.485 | 19.50x | 32.959 | 641.442 | 19.46x |
| weibull_aft_1000 | 5.305 | 85.961 | 16.20x | 5.330 | 85.959 | 16.13x |
| lognormal_aft_ridge_robust_1000 | 8.395 | 3128.746 | 372.68x | 8.471 | 3128.504 | 369.31x |
| loglogistic_aft_interval_1000 | 8.209 | 135.271 | 16.48x | 8.211 | 135.257 | 16.47x |
| breslow_fleming_harrington_1000 | 0.114 | 4.673 | 41.15x | 0.114 | 4.673 | 41.15x |
| kaplan_meier_left_1000 | 0.069 | 5.495 | 79.32x | 0.069 | 5.495 | 79.31x |
| weibull_10000 | 26.185 | 51.739 | 1.98x | 26.181 | 51.738 | 1.98x |
| loglogistic_left_10000 | 52.726 | 108.342 | 2.05x | 52.723 | 108.333 | 2.05x |
| lognormal_interval_10000 | 43.531 | 221.341 | 5.08x | 43.527 | 221.324 | 5.08x |
| generalized_gamma_10000 | 334.091 | 1596.846 | 4.78x | 334.074 | 1596.794 | 4.78x |
| weibull_aft_10000 | 53.776 | 96.514 | 1.79x | 53.772 | 96.509 | 1.79x |
| lognormal_aft_ridge_robust_10000 | 108.621 | 29990.667 | 276.10x | 108.619 | 29989.419 | 276.10x |
| loglogistic_aft_interval_10000 | 77.387 | 179.638 | 2.32x | 77.390 | 179.624 | 2.32x |
| breslow_fleming_harrington_10000 | 2.121 | 6.146 | 2.90x | 2.121 | 6.146 | 2.90x |
| kaplan_meier_left_10000 | 0.836 | 7.225 | 8.65x | 0.836 | 7.225 | 8.65x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.032 | 1.933 | 61.08x | 0.032 | 1.933 | 61.08x |
| var_n10000 | 0.381 | 12.453 | 32.69x | 0.383 | 12.452 | 32.52x |
| var_n100000 | 3.156 | 118.393 | 37.51x | 3.287 | 118.385 | 36.02x |

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
- `ClusteringAgreementBenchmarks`
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
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorStoreChurnBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 1.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `ChainedProductBenchmarks` | Rows=20000, Width=128, Device=CPUAc(...) GPU) [26] | `CpuBaseline` | step | 0 | 0.001 | -100% | 30% |

**Still away from their median: 11.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `VectorizerBenchmarks` | Documents=200 | `CountCharWordBoundary` | drift | 11.04 | 8.21 | +46% | 20% |
| `Bm25Benchmarks` | Documents=1000 | `LodestarFromText` | drift | 1848.83 | 3973.42 | -40% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `AffineSignaturesOnly` | drift | 0.03 | 0.03 | -33% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `AffineSignaturesOnly` | drift | 0.03 | 0.03 | -33% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `SignaturesOnly` | drift | 0.04 | 0.04 | -25% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `FingerprintsOnly` | drift | 0.05 | 0.04 | +25% | 20% |
| `Bm25Benchmarks` | Documents=20000 | `LodestarFromText` | drift | 2699.14 | 4337.02 | -24% | 20% |
| `SimilaritySketchBenchmarks` | Documents=500, Permutations=64 | `AffineSignaturesOnly` | drift | 0.16 | 0.14 | -21% | 20% |
| `SimilaritySketchBenchmarks` | Documents=500, Permutations=128 | `AffineSignaturesOnly` | drift | 0.12 | 0.14 | -21% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=64 | `FingerprintsOnly` | drift | 0.04 | 0.05 | -20% | 20% |
| `SimilaritySketchBenchmarks` | Documents=2000, Permutations=128 | `SignaturesOnly` | drift | 0.05 | 0.05 | -20% | 20% |
