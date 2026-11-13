# Nightly benchmark run

<!-- nightly-baseline: 59a57f7257ac13a8a57dcee55e218a7f45b3b458 -->
<!-- nightly-owed: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
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

- Commit: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
- Previous run: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `StatsBenchmarks`
- `RankTestBenchmarks`
- `DistributionTailBenchmarks`
- `DistributionIncumbentBenchmarks`
- `StationarityBenchmarks`
- `SerialCorrelationBenchmarks`
- `QuantileBenchmarks`
- `OlsBenchmarks`
- `GlsBenchmarks`
- `MathNetInteropBenchmarks`
- `WeightedLeastSquaresBenchmarks`
- `RobustCovarianceBenchmarks`
- `HacClusterBenchmarks`
- `GlmBenchmarks`
- `GlmPoissonBenchmarks`
- `GlmOffsetBenchmarks`
- `MultinomialLogitBenchmarks`
- `VectorAutoregressionBenchmarks`
- `MannWhitneyExactBenchmarks`
- `KsExactTableBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `LeastSquaresRoutingBenchmarks`

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.561 ms** |   **0.0293 ms** |  **0.0016 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   4.320 ms |   6.2310 ms |  0.3415 ms |  1.69 |    0.12 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.255 ms |   6.6808 ms |  0.3662 ms |  2.05 |    0.12 |  93.7500 |  93.7500 |  93.7500 |   471051 B |  117,762.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.125 ms** |   **0.3909 ms** |  **0.0214 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   9.422 ms |  15.7905 ms |  0.8655 ms |  0.78 |    0.06 |        - |        - |        - |    42840 B |    2,677.50 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  11.349 ms |  10.6712 ms |  0.5849 ms |  0.94 |    0.04 | 312.5000 | 312.5000 | 312.5000 |  2711060 B |  169,441.25 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **52.129 ms** |   **6.3435 ms** |  **0.3477 ms** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |       **74 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  53.029 ms |  37.5388 ms |  2.0576 ms |  1.02 |    0.03 |        - |        - |        - |   802898 B |   10,849.97 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  63.577 ms | 241.1979 ms | 13.2209 ms |  1.22 |    0.22 | 375.0000 | 375.0000 | 375.0000 |  8072041 B |  109,081.64 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **240.940 ms** |   **1.2859 ms** |  **0.0705 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 145.970 ms |  58.2925 ms |  3.1952 ms |  0.61 |    0.01 |        - |        - |        - |   803080 B |    2,355.07 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 242.603 ms | 152.6404 ms |  8.3667 ms |  1.01 |    0.03 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |  155,047.78 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Rows  | Width | Device               | Mean          | Error          | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|---------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,745.699 ms** |  **1,094.1194 ms** |  **59.9724 ms** | **1.001** |    **0.04** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,820.582 ms |  1,168.5223 ms |  64.0506 ms | 1.044 |    0.04 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.641 ms |      0.6747 ms |   0.0370 ms | 0.001 |    0.00 |   324.2188 |   324.2188 |   324.2188 |   1.47 MB |        0.20 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,308.384 ms** |    **627.3300 ms** |  **34.3861 ms** | **1.000** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,309.148 ms |  1,343.4272 ms |  73.6378 ms | 1.000 |    0.01 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      7.349 ms |      3.3219 ms |   0.1821 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,324.327 ms** |  **7,270.5795 ms** | **398.5250 ms** | **1.002** |    **0.07** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,286.712 ms |    936.7696 ms |  51.3475 ms | 0.997 |    0.05 |  1000.0000 |  1000.0000 |  1000.0000 |  42.13 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      8.170 ms |      3.3881 ms |   0.1857 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |   5.86 MB |        0.13 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **29,927.921 ms** | **10,169.2435 ms** | **557.4106 ms** | **1.000** |    **0.02** | **17000.0000** | **16000.0000** | **15000.0000** | **388.77 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 30,456.174 ms |  6,462.3870 ms | 354.2252 ms | 1.018 |    0.02 | 20000.0000 | 19000.0000 | 18000.0000 | 384.89 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     25.432 ms |      1.5303 ms |   0.0839 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev     | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-----------:|-------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **84.57 ms** |      **5.643 ms** |   **0.309 ms** |   **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   4,941.82 ms |  3,412.734 ms | 187.063 ms |  58.44 |    1.92 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,373.11 ms |  8,535.872 ms | 467.880 ms |  63.54 |    4.80 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |            |        |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |     **100.48 ms** |      **1.044 ms** |   **0.057 ms** |   **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  10,154.13 ms |  4,940.349 ms | 270.797 ms | 101.05 |    2.33 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  10,315.25 ms |  8,104.924 ms | 444.258 ms | 102.66 |    3.83 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |            |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **850.78 ms** |     **21.598 ms** |   **1.184 ms** |   **1.00** |    **0.00** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  51,661.04 ms |  7,153.540 ms | 392.110 ms |  60.72 |    0.41 |  4000.0000 |  3000.0000 | 2000.0000 | 160.07 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  51,541.66 ms |  9,767.480 ms | 535.389 ms |  60.58 |    0.55 |  7000.0000 |  4000.0000 | 2000.0000 |  211.6 MB |        7.11 |
|                |           |              |                      |               |               |            |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |     **994.38 ms** |     **16.192 ms** |   **0.888 ms** |   **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 103,509.29 ms | 15,964.133 ms | 875.048 ms | 104.09 |    0.77 | 12000.0000 | 10000.0000 | 8000.0000 | 536.13 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 104,388.55 ms |  6,703.773 ms | 367.456 ms | 104.98 |    0.33 | 15000.0000 | 12000.0000 | 8000.0000 | 587.62 MB |       14.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,432.10 ns | 307.815 ns | 16.872 ns | 1.000 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   304.11 ns |  99.496 ns |  5.454 ns | 0.125 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   338.58 ns |  35.885 ns |  1.967 ns | 0.139 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,448.33 ns | 113.472 ns |  6.220 ns | 3.063 |    0.02 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,630.94 ns | 230.883 ns | 12.655 ns | 2.315 |    0.01 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,191.16 ns | 166.650 ns |  9.135 ns | 0.490 |    0.00 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   774.65 ns |   2.604 ns |  0.143 ns | 0.319 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.97 ns |  30.744 ns |  1.685 ns | 0.055 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   135.25 ns |   2.277 ns |  0.125 ns | 0.056 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 2,805.61 ns | 132.985 ns |  7.289 ns | 1.154 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,657.58 ns | 146.410 ns |  8.025 ns | 1.093 |    0.01 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   314.28 ns |  20.088 ns |  1.101 ns | 0.129 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    40.97 ns |   5.492 ns |  0.301 ns | 0.017 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    60.10 ns |   0.759 ns |  0.042 ns | 0.025 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    71.59 ns |  10.048 ns |  0.551 ns | 0.029 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   358.59 ns |  11.919 ns |  0.653 ns | 0.147 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   465.34 ns |  21.833 ns |  1.197 ns | 0.191 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   386.43 ns |   9.217 ns |  0.505 ns | 0.159 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    24.67 ns |   0.508 ns |  0.028 ns | 0.010 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    21.90 ns |   0.115 ns |  0.006 ns | 0.009 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.00 ns |   2.066 ns |  0.113 ns | 0.027 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.92 ns |   3.239 ns |  0.178 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.90 ns |  11.737 ns |  0.643 ns | 0.013 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.18 ns |   4.758 ns |  0.261 ns | 0.074 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  31.10 ns |  0.641 ns | 0.035 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  14.06 ns |  1.792 ns | 0.098 ns |  0.45 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  43.44 ns |  1.043 ns | 0.057 ns |  1.40 |    0.00 |         - |          NA |
| ChiSquaredSfHundredDf      |  75.61 ns |  0.705 ns | 0.039 ns |  2.43 |    0.00 |         - |          NA |
| ChiSquaredSfFractionalDf   | 271.04 ns | 27.843 ns | 1.526 ns |  8.72 |    0.04 |         - |          NA |
| NormalQuantile             |  28.75 ns |  0.224 ns | 0.012 ns |  0.92 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **35.04 μs** |  **3.518 μs** | **0.193 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  51.92 μs |  7.617 μs | 0.418 μs |  1.48 |    0.01 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.51 μs** |  **6.572 μs** | **0.360 μs** |  **1.00** |    **0.01** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 107.76 μs | 16.315 μs | 0.894 μs |  2.60 |    0.03 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **349.44 μs** | **88.454 μs** | **4.848 μs** |  **1.00** |    **0.02** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 391.72 μs | 89.151 μs | 4.887 μs |  1.12 |    0.02 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **402.26 μs** |  **3.924 μs** | **0.215 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 751.67 μs | 74.623 μs | 4.090 μs |  1.87 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **266.3 μs** |     **3.81 μs** |  **0.21 μs** |   **3.4180** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **3,076.0 μs** |   **266.02 μs** | **14.58 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **30,611.7 μs** | **1,401.51 μs** | **76.82 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.04 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.15 μs** |     **1.751 μs** |  **0.096 μs** |  **1.00** |    **0.00** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    59.28 μs |     4.973 μs |  0.273 μs |  2.03 |    0.01 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,056.68 μs** |   **551.097 μs** | **30.207 μs** |  **1.00** |    **0.01** | **328.1250** | **328.1250** | **328.1250** | **1097.72 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,683.11 μs | 1,002.977 μs | 54.977 μs |  1.89 |    0.02 | 531.2500 | 531.2500 | 531.2500 | 1878.08 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **296.8 μs** | **51.95 μs** | **2.85 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **294.8 μs** | **24.12 μs** | **1.32 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **297.7 μs** | **29.23 μs** | **1.60 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **50.85 μs** |       **7.389 μs** |     **0.405 μs** |  **1.00** |    **0.01** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     103.54 μs |       9.339 μs |     0.512 μs |  2.04 |    0.02 |   1.9531 |   0.1221 |        - |   32.98 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **691.93 μs** |      **22.420 μs** |     **1.229 μs** |  **1.00** |    **0.00** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,783.34 μs |     686.957 μs |    37.654 μs |  2.58 |    0.05 |  99.6094 |  99.6094 |  99.6094 |  370.16 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,896.57 μs** |     **313.098 μs** |    **17.162 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  21,806.40 μs |   1,424.566 μs |    78.085 μs |  3.16 |    0.01 | 281.2500 | 281.2500 | 281.2500 | 2226.55 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **52,453.98 μs** |   **4,776.004 μs** |   **261.789 μs** |  **1.00** |    **0.01** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 144,326.14 μs | 115,135.452 μs | 6,310.963 μs |  2.75 |    0.10 |        - |        - |        - |  8852.8 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | SampleSize | Lags | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **36.590 μs** |   **3.4284 μs** | **0.1879 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.722 μs |   2.4780 μs | 0.1358 μs |  0.29 |    0.00 |   0.5341 |        - |        - |    8.8 KB |        3.32 |
| Hac       | 100        | 4    |    19.427 μs |   2.2341 μs | 0.1225 μs |  0.53 |    0.00 |   0.7629 |        - |        - |  12.95 KB |        4.89 |
| Cluster   | 100        | 4    |     9.395 μs |   0.9190 μs | 0.0504 μs |  0.26 |    0.00 |   0.5951 |        - |        - |   9.76 KB |        3.68 |
|           |            |      |              |             |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **455.945 μs** |  **27.8242 μs** | **1.5251 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   902.379 μs |  69.0971 μs | 3.7874 μs |  1.98 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.94 KB |        5.91 |
| Hac       | 10000      | 4    | 2,038.001 μs | 149.6566 μs | 8.2032 μs |  4.47 |    0.02 | 246.0938 | 246.0938 | 246.0938 | 863.89 KB |       10.80 |
| Cluster   | 10000      | 4    |   794.894 μs | 143.1285 μs | 7.8454 μs |  1.74 |    0.02 | 124.0234 | 124.0234 | 124.0234 | 565.22 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | SampleSize | Mean         | Error      | StdDev     | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|-----------:|-----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **8.520 μs** |   **2.810 μs** |  **0.1540 μs** |  **0.2747** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **130.928 μs** |  **20.539 μs** |  **1.1258 μs** |  **1.4648** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,118.886 μs** | **330.664 μs** | **18.1248 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,432.69 μs** | **622.510 μs** | **34.122 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **49.07 μs** |   **7.970 μs** |  **0.437 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **137.2 μs** |      **27.39 μs** |      **1.50 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     192.2 μs |      27.96 μs |      1.53 μs |  1.40 |    0.02 |   1.9531 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     631.4 μs |      20.26 μs |      1.11 μs |  4.60 |    0.04 | 190.4297 | 190.4297 | 190.4297 |   706.05 KB |        3.75 |
|                   |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **60,553.3 μs** |  **40,174.87 μs** |  **2,202.12 μs** |  **1.00** |    **0.04** | **375.0000** | **375.0000** | **375.0000** |  **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 298,232.0 μs |  20,555.19 μs |  1,126.70 μs |  4.93 |    0.15 |        - |        - |        - |   3499.3 KB |        0.39 |
| MathNet_Qr        | 250        | 489,518.6 μs | 276,493.03 μs | 15,155.52 μs |  8.09 |    0.33 |        - |        - |        - | 32661.27 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape   | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **579.9 ms** | **87.04 ms** | **4.77 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **455.4 ms** | **40.75 ms** | **2.23 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Size | Mean          | Error        | StdDev      | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|-------------:|------------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.366 μs** |     **1.254 μs** |   **0.0687 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,584.510 μs |   221.639 μs |  12.1488 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     21.262 μs |     1.278 μs |   0.0701 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     91.288 μs |    12.360 μs |   0.6775 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.579 μs** |     **1.287 μs** |   **0.0705 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,961.378 μs | 3,190.618 μs | 174.8886 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    532.815 μs |    47.058 μs |   2.5794 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,302.078 μs |    75.125 μs |   4.1179 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | SampleSize | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|----------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **133.8 μs** |  **42.96 μs** |  **2.35 μs** |  **1.00** |    **0.02** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   897.4 μs | 201.75 μs | 11.06 μs |  6.71 |    0.12 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |           |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,369.8 μs** | **232.03 μs** | **12.72 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,102.5 μs | 687.86 μs | 37.70 μs |  5.19 |    0.05 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **37.22 μs** |   **1.822 μs** |  **0.100 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   264.32 μs |  47.056 μs |  2.579 μs |  7.10 |    0.06 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    32.26 μs |  14.033 μs |  0.769 μs |  0.87 |    0.02 |   1.9531 |        - |        - |   35.58 KB |       13.43 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **474.67 μs** |  **22.515 μs** |  **1.234 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,508.28 μs | 322.990 μs | 17.704 μs |  9.50 |    0.04 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,669.43 μs | 149.715 μs |  8.206 μs |  3.52 |    0.02 | 443.3594 | 443.3594 | 443.3594 | 1766.06 KB |       22.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                       | Mean           | Error         | StdDev      | Ratio      | RatioSD   | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|------------:|-----------:|----------:|----------:|------------:|
| NormalQuantile               |      0.0388 ns |     0.0347 ns |   0.0019 ns |       1.00 |      0.06 |         - |          NA |
| NormalQuantileFarTail        |      0.3457 ns |     0.2280 ns |   0.0125 ns |       8.93 |      0.47 |         - |          NA |
| StudentQuantile              | 14,168.0590 ns | 2,297.4253 ns | 125.9296 ns | 366,120.79 | 15,506.62 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,325.1429 ns |    48.3564 ns |   2.6506 ns |  60,084.67 |  2,503.07 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,436.0 μs** |     **6.49 μs** |   **0.36 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    420.3 μs |    17.65 μs |   0.97 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    597.1 μs |    80.33 μs |   4.40 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,260.8 μs |    80.57 μs |   4.42 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    700.2 μs |    24.34 μs |   1.33 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **623.0 μs** |    **14.89 μs** |   **0.82 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    241.2 μs |    79.91 μs |   4.38 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    516.7 μs |    79.36 μs |   4.35 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    770.9 μs |    54.89 μs |   3.01 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    384.3 μs |    18.56 μs |   1.02 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,926.3 μs** |   **660.80 μs** |  **36.22 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,495.1 μs |   246.25 μs |  13.50 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,592.8 μs |   914.30 μs |  50.12 μs | 500.0000 | 500.0000 | 500.0000 |  5062689 B |
| WilcoxonPooledDifferences | 100000     | False | 16,984.5 μs | 1,122.48 μs |  61.53 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,068.9 μs |   154.25 μs |   8.46 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,137.4 μs** | **1,644.64 μs** |  **90.15 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,764.2 μs |   231.70 μs |  12.70 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,218.3 μs |   263.54 μs |  14.45 μs | 472.6563 | 472.6563 | 472.6563 |  5062670 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,110.3 μs | 3,914.71 μs | 214.58 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,761.5 μs |    73.82 μs |   4.05 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **36.74 μs** |   **2.216 μs** |  **0.121 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.35 μs** |   **0.802 μs** |  **0.044 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.47 μs** |   **0.700 μs** |  **0.038 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.73 μs** |   **1.445 μs** |  **0.079 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.89 μs** |   **0.696 μs** |  **0.038 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **452.47 μs** |   **4.810 μs** |  **0.264 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **886.93 μs** |  **12.718 μs** |  **0.697 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **892.70 μs** |  **77.573 μs** |  **4.252 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,047.36 μs** |  **44.149 μs** |  **2.420 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,042.65 μs** | **199.175 μs** | **10.917 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.478 μs** | **0.1302 μs** | **0.0071 μs** |  **1.00** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.536 μs | 0.2557 μs | 0.0140 μs |  2.58 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.240 μs | 0.8155 μs | 0.0447 μs |  1.17 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.163 μs | 0.2862 μs | 0.0157 μs |  2.72 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.982 μs | 0.2605 μs | 0.0143 μs |  1.11 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.653 μs | 0.4927 μs | 0.0270 μs |  2.60 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.712 μs** | **1.4626 μs** | **0.0802 μs** |  **1.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.851 μs | 2.3741 μs | 0.1301 μs |  2.48 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.280 μs | 1.0103 μs | 0.0554 μs |  1.01 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.519 μs | 3.3821 μs | 0.1854 μs |  2.49 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.597 μs | 5.8597 μs | 0.3212 μs |  1.02 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 120.911 μs | 1.7156 μs | 0.0940 μs |  2.48 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.097 μs** |  **1.5151 μs** | **0.0831 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  28.591 μs |  3.2074 μs | 0.1758 μs |  4.03 |    0.05 |   4.0588 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  26.062 μs |  4.0625 μs | 0.2227 μs |  3.67 |    0.05 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.833 μs |  0.0853 μs | 0.0047 μs |  0.54 |    0.01 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   3.945 μs |  0.2101 μs | 0.0115 μs |  0.56 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.422 μs |  0.3629 μs | 0.0199 μs |  0.20 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.724 μs |  0.0852 μs | 0.0047 μs |  0.38 |    0.00 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **111.109 μs** | **12.2060 μs** | **0.6691 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 940.958 μs |  7.3210 μs | 0.4013 μs |  8.47 |    0.04 | 249.0234 | 249.0234 | 249.0234 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 288.674 μs | 68.0629 μs | 3.7308 μs |  2.60 |    0.03 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.666 μs |  1.5327 μs | 0.0840 μs |  0.86 |    0.00 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.232 μs |  2.7549 μs | 0.1510 μs |  0.88 |    0.00 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  13.639 μs |  3.7600 μs | 0.2061 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  28.605 μs |  3.5292 μs | 0.1934 μs |  0.26 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean            | Error           | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,151.8 ns** |     **1,376.20 ns** |     **75.43 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,106.9 ns |     1,819.59 ns |     99.74 ns |    6.20 |    0.07 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,185.9 ns |       439.10 ns |     24.07 ns |    0.36 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,877.5 ns |     1,766.72 ns |     96.84 ns |    3.88 |    0.04 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        112.7 ns |         6.33 ns |      0.35 ns |    0.02 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        218.0 ns |        16.78 ns |      0.92 ns |    0.04 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |                 |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **53,353.0 ns** |       **243.52 ns** |     **13.35 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    140,627.5 ns |     4,184.91 ns |    229.39 ns |   2.636 |    0.00 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    697,667.0 ns |    31,215.21 ns |  1,711.01 ns |  13.076 |    0.03 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,516,680.0 ns | 1,373,591.49 ns | 75,291.18 ns | 234.601 |    1.22 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        117.1 ns |         9.77 ns |      0.54 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        216.9 ns |        14.25 ns |      0.78 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **15.83 μs** |   **2.863 μs** |  **0.157 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **36.96 μs** |   **5.079 μs** |  **0.278 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **50.50 μs** |   **7.316 μs** |  **0.401 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **149.58 μs** |   **8.304 μs** |  **0.455 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **206.72 μs** |  **25.461 μs** |  **1.396 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **543.14 μs** |  **60.725 μs** |  **3.329 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **704.23 μs** | **117.349 μs** |  **6.432 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,297.91 μs** | **274.899 μs** | **15.068 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **45.79 μs** |      **4.779 μs** |     **0.262 μs** |  **1.00** |    **0.01** |    **0.1831** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     20.49 μs |      0.685 μs |     0.038 μs |  0.45 |    0.00 |    3.6926 |    0.1526 |         - |    58.27 KB |       16.99 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **135.22 μs** |     **24.819 μs** |     **1.360 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    118.71 μs |     10.043 μs |     0.551 μs |  0.88 |    0.01 |   33.2031 |   14.2822 |         - |   508.31 KB |       29.06 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **925.90 μs** |     **90.883 μs** |     **4.982 μs** |  **1.00** |    **0.01** |   **46.8750** |   **46.8750** |   **46.8750** |    **158.5 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,814.75 μs |  2,807.397 μs |   153.883 μs |  1.96 |    0.14 | 1011.7188 | 1009.7656 | 1009.7656 |  5028.85 KB |       31.73 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,415.54 μs** |  **1,553.911 μs** |    **85.175 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 19,076.76 μs | 19,060.861 μs | 1,044.790 μs |  2.03 |    0.10 |  718.7500 |  718.7500 |  718.7500 | 50023.16 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Failed tonight

These exited with an error, raised, shared a script that failed tonight, or are named by the map and declared by no project; the log's annotation on each says which. The run went on, so the rest are on this page. A failure is not owed: it is selected again only when something it reads changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BkTreeNearestBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `ClusteringAgreementBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `DecompositionWidthBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `EncoderIncumbentBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `HouseholderQrBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `OsaBenchmarks`
- `PartitionValidityBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `ProcessExtractBenchmarks`
- `QgramBenchmarks`
- `RankingMetricsBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RobustScalerSparseBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `SilhouetteBenchmarks`
- `SplitterIncumbentBenchmarks`
- `TextRankBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorStoreChurnBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 4.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `NormalQuantileFarTail` | step | 8.93 | 0.98 | +811% | 38% |
| `DistributionTailBenchmarks` | — | `ChiSquaredSfFractionalDf` | step | 8.72 | 3.97 | +120% | 30% |
| `GlsBenchmarks` | SampleSize=50 | `MathNet_Gls` | drift | 2.04 | 3.6 | -41% | 20% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=2000 | `MathNet_Wls` | drift | 0.88 | 1.34 | -34% | 20% |

**Still away from their median: 35.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `QuantileBenchmarks` | — | `StudentQuantile` | drift | 366121 | 13202.3 | +2145% | 20% |
| `QuantileBenchmarks` | — | `StudentQuantileCauchyFarTail` | drift | 60084.7 | 3791.93 | +1180% | 20% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredCdf` | step | 0.017 | 0.165 | -90% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredCdf` | step | 0.029 | 0.23 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredQuantile` | step | 0.191 | 1.49 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherQuantile` | step | 0.129 | 1.005 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalCdf` | step | 0.009 | 0.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_FisherCdf` | step | 0.056 | 0.435 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_ChiSquaredQuantile` | step | 0.159 | 1.225 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentCdf` | step | 0.139 | 1.07 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_NormalQuantile` | step | 0.013 | 0.1 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentQuantile` | step | 2.315 | 17.7 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_StudentQuantile` | step | 0.49 | 3.745 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherQuantile` | step | 1.093 | 8.35 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_FisherCdf` | step | 0.055 | 0.42 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalQuantile` | step | 0.074 | 0.565 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_ChiSquaredCdf` | step | 0.025 | 0.19 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MathNet_StudentCdf` | step | 0.125 | 0.94 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalQuantile` | step | 0.012 | 0.09 | -87% | 30% |
| `DistributionIncumbentBenchmarks` | — | `MetaNumerics_NormalCdf` | step | 0.027 | 0.2 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_ChiSquaredQuantile` | step | 0.147 | 1.065 | -86% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_NormalCdf` | step | 0.01 | 0.07 | -86% | 30% |
| `OlsBenchmarks` | SampleSize=100 | `Accord_Ols` | step | 7.1 | 39.12 | -82% | 46% |
| `StatsBenchmarks` | SampleSize=100 | `AccordWelchT` | step | 6.2 | 33.47 | -81% | 30% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hac` | step | 0.53 | 2.86 | -81% | 30% |
| `OlsBenchmarks` | SampleSize=100 | `MathNet_Ols` | step | 0.87 | 4.61 | -81% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `AccordMannWhitney` | step | 3.88 | 20.44 | -81% | 35% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarMannWhitney` | step | 0.36 | 1.89 | -81% | 67% |
| `StatsBenchmarks` | SampleSize=100 | `LodestarChiSquare` | step | 0.02 | 0.1 | -80% | 36% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Hc0` | step | 0.29 | 1.43 | -80% | 30% |
| `HacClusterBenchmarks` | SampleSize=100, Lags=4 | `Cluster` | step | 0.26 | 1.27 | -80% | 30% |
| `StatsBenchmarks` | SampleSize=100 | `AccordChiSquare` | step | 0.04 | 0.19 | -79% | 55% |
| `WeightedLeastSquaresBenchmarks` | SampleSize=200 | `MathNet_Wls` | step | 0.45 | 1.69 | -73% | 36% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_FisherQuantile` | step | 1.154 | 0.69 | +67% | 30% |
| `DistributionIncumbentBenchmarks` | — | `Lodestar_StudentQuantile` | step | 3.063 | 2.23 | +37% | 30% |
