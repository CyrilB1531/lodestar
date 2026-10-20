# Latest known benchmark result, per method

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml` from the
> wiki's own history, alongside [nightly_run](nightly_run.md).

**Not a comparison across methods.** Each section below is the last night that method was
actually re-run -- whichever night touched the source near it, not necessarily last night,
and not the same night as its neighbours here. Every run measures on a GitHub hosted
runner whose hardware differs night to night, so a number here says "this is the last
known reading", never "faster than the section above it".

## Per method

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

_As of 2026-10-16, measured at commit `724c791262e7139428ab02fd4e70a6e05305bac7`._

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

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.599 ms** |   **0.0188 ms** |  **0.0010 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   5.034 ms |   9.5171 ms |  0.5217 ms |  1.94 |    0.17 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.581 ms |  20.5345 ms |  1.1256 ms |  2.15 |    0.38 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.099 ms** |   **0.0882 ms** |  **0.0048 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   9.266 ms |  19.2470 ms |  1.0550 ms |  0.77 |    0.08 |        - |        - |        - |    42840 B |    2,677.50 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  11.473 ms |  10.0008 ms |  0.5482 ms |  0.95 |    0.04 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |  169,441.19 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **51.284 ms** |   **0.4519 ms** |  **0.0248 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **102 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  42.680 ms |  21.2882 ms |  1.1669 ms |  0.83 |    0.02 |        - |        - |        - |   802891 B |    7,871.48 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  60.410 ms | 293.5430 ms | 16.0901 ms |  1.18 |    0.27 | 375.0000 | 375.0000 | 375.0000 |  8071921 B |   79,136.48 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **242.980 ms** |   **1.9698 ms** |  **0.1080 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 141.782 ms |  59.7071 ms |  3.2727 ms |  0.58 |    0.01 |        - |        - |        - |   803080 B |    2,355.07 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 196.560 ms |  48.9818 ms |  2.6849 ms |  0.81 |    0.01 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |  155,047.78 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

_As of 2026-10-16, measured at commit `724c791262e7139428ab02fd4e70a6e05305bac7`._

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

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,833.506 ms** |   **322.6532 ms** |  **17.6857 ms** | **1.000** |    **0.01** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,785.777 ms | 1,150.7003 ms |  63.0738 ms | 0.974 |    0.03 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.582 ms |     1.0514 ms |   0.0576 ms | 0.001 |    0.00 |   330.0781 |   330.0781 |   330.0781 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,201.796 ms** | **1,851.2048 ms** | **101.4708 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.88 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,318.144 ms | 2,547.7987 ms | 139.6534 ms | 1.016 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      6.098 ms |     1.5072 ms |   0.0826 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,356.624 ms** | **1,534.9147 ms** |  **84.1339 ms** | **1.000** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,288.447 ms |   632.5131 ms |  34.6702 ms | 0.991 |    0.01 |  1000.0000 |  1000.0000 |  1000.0000 |  42.18 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      6.309 ms |     4.4251 ms |   0.2426 ms | 0.001 |    0.00 |   390.6250 |   390.6250 |   390.6250 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **29,110.264 ms** | **9,991.8254 ms** | **547.6857 ms** | **1.000** |    **0.02** | **16000.0000** | **15000.0000** | **14000.0000** | **388.65 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 29,249.586 ms | 5,162.4763 ms | 282.9727 ms | 1.005 |    0.02 | 20000.0000 | 19000.0000 | 18000.0000 | 385.11 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     15.542 ms |     0.5409 ms |   0.0296 ms | 0.001 |    0.00 |   296.8750 |   296.8750 |   296.8750 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **99.71 ms** |      **0.858 ms** |   **0.047 ms** |  **1.00** |    **0.00** |          **-** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   5,037.75 ms |  1,937.654 ms | 106.209 ms | 50.52 |    0.92 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,868.51 ms | 15,057.419 ms | 825.348 ms | 58.85 |    7.17 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |     **101.47 ms** |      **4.147 ms** |   **0.227 ms** |  **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  10,111.69 ms |  2,011.745 ms | 110.271 ms | 99.66 |    0.96 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |   9,703.57 ms |  2,192.934 ms | 120.202 ms | 95.63 |    1.04 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **878.15 ms** |     **26.612 ms** |   **1.459 ms** |  **1.00** |    **0.00** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  49,853.54 ms |  7,649.816 ms | 419.312 ms | 56.77 |    0.42 |  4000.0000 |  3000.0000 | 2000.0000 | 160.03 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  48,916.00 ms |  6,665.311 ms | 365.348 ms | 55.70 |    0.37 |  7000.0000 |  4000.0000 | 2000.0000 | 211.63 MB |        7.11 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |   **1,163.32 ms** |     **27.464 ms** |   **1.505 ms** |  **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 100,253.03 ms |  2,952.221 ms | 161.821 ms | 86.18 |    0.15 | 12000.0000 | 11000.0000 | 8000.0000 | 536.05 MB |       12.77 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 101,382.87 ms | 12,440.716 ms | 681.918 ms | 87.15 |    0.52 | 15000.0000 | 12000.0000 | 8000.0000 |  587.6 MB |       14.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledSparseDenseProductBenchmarks-report-github

_As of 2026-10-16, measured at commit `724c791262e7139428ab02fd4e70a6e05305bac7`._

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

| Method      | Documents | Width | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0       | Gen1       | Gen2       | Allocated  | Alloc Ratio |
|------------ |---------- |------ |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|------------:|
| **CpuBaseline** | **5000**      | **64**    | **CPUAc(...) GPU) [26]** |       **4.736 ms** |      **0.1452 ms** |     **0.0080 ms** |     **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |    **2.44 MB** |        **1.00** |
| GpuResident | 5000      | 64    | CPUAc(...) GPU) [26] |   7,936.388 ms | 10,448.8434 ms |   572.7364 ms | 1,675.93 |  104.77 |          - |          - |          - |   20.95 MB |        8.58 |
| GpuFromHost | 5000      | 64    | CPUAc(...) GPU) [26] |   7,583.749 ms |  3,533.5361 ms |   193.6850 ms | 1,601.47 |   35.50 |          - |          - |          - |   23.26 MB |        9.53 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **5000**      | **256**   | **CPUAc(...) GPU) [26]** |      **15.436 ms** |      **2.7440 ms** |     **0.1504 ms** |     **1.00** |    **0.01** |   **125.0000** |   **125.0000** |   **125.0000** |    **9.77 MB** |        **1.00** |
| GpuResident | 5000      | 256   | CPUAc(...) GPU) [26] |  30,068.332 ms |  6,222.5518 ms |   341.0791 ms | 1,948.06 |   25.18 |  1000.0000 |          - |          - |  139.74 MB |       14.31 |
| GpuFromHost | 5000      | 256   | CPUAc(...) GPU) [26] |  29,886.177 ms |  6,531.8107 ms |   358.0306 ms | 1,936.26 |   25.85 |  1000.0000 |          - |          - |  146.05 MB |       14.96 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **50000**     | **64**    | **CPUAc(...) GPU) [26]** |      **46.911 ms** |      **1.4874 ms** |     **0.0815 ms** |     **1.00** |    **0.00** |   **181.8182** |   **181.8182** |   **181.8182** |   **24.41 MB** |        **1.00** |
| GpuResident | 50000     | 64    | CPUAc(...) GPU) [26] |  74,761.614 ms | 15,795.4996 ms |   865.8046 ms | 1,593.68 |   16.16 | 10000.0000 |  9000.0000 |  8000.0000 |  519.77 MB |       21.29 |
| GpuFromHost | 50000     | 64    | CPUAc(...) GPU) [26] |  75,025.414 ms | 14,813.2074 ms |   811.9619 ms | 1,599.30 |   15.18 | 11000.0000 | 10000.0000 |  9000.0000 |  546.85 MB |       22.40 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **50000**     | **256**   | **CPUAc(...) GPU) [26]** |     **168.351 ms** |     **79.0823 ms** |     **4.3348 ms** |     **1.00** |    **0.03** |   **250.0000** |   **250.0000** |   **250.0000** |   **97.66 MB** |        **1.00** |
| GpuResident | 50000     | 256   | CPUAc(...) GPU) [26] | 308,955.099 ms | 21,708.5470 ms | 1,189.9187 ms | 1,836.01 |   42.01 | 83000.0000 | 82000.0000 | 72000.0000 |  7354.9 MB |       75.31 |
| GpuFromHost | 50000     | 256   | CPUAc(...) GPU) [26] | 303,791.917 ms | 16,859.3839 ms |   924.1197 ms | 1,805.33 |   41.14 | 81000.0000 | 80000.0000 | 70000.0000 | 7381.63 MB |       75.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                         | PairCount | Mean             | Error           | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **981.0 ns** |        **21.02 ns** |       **1.15 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         799.9 ns |        26.04 ns |       1.43 ns |     0.82 |    0.00 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,673.6 ns |       436.60 ns |      23.93 ns |     2.73 |    0.02 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,690.7 ns |       133.32 ns |       7.31 ns |     3.76 |    0.01 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,396.8 ns |     1,883.23 ns |     103.23 ns |     5.50 |    0.09 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,744.6 ns |     3,691.04 ns |     202.32 ns |    10.95 |    0.18 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,348.2 ns |       153.82 ns |       8.43 ns |     5.45 |    0.01 |        - |        - |        - |         - |          NA |
|                                |           |                  |                 |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **88,046.5 ns** |     **7,045.12 ns** |     **386.17 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,789.3 ns |     2,525.95 ns |     138.46 ns |     0.65 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,119,224.6 ns |   120,535.76 ns |   6,606.97 ns |    12.71 |    0.08 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,955,763.1 ns |   300,448.90 ns |  16,468.62 ns |    22.21 |    0.18 |   7.8125 |        - |        - |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,140,641.8 ns |   186,911.99 ns |  10,245.28 ns |    24.31 |    0.14 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,450,183.3 ns | 6,180,687.55 ns | 338,784.34 ns | 3,276.16 |   12.90 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,172,670.6 ns |   186,677.32 ns |  10,232.41 ns |    24.68 |    0.14 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                          | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             |   317.88 ns |   6.281 ns |  0.344 ns |  1.00 |    0.00 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.42 ns |   6.123 ns |  0.336 ns |  0.94 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   338.89 ns |   1.506 ns |  0.083 ns |  1.07 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        |   686.87 ns |  60.587 ns |  3.321 ns |  2.16 |    0.01 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,655.21 ns |  24.076 ns |  1.320 ns | 17.79 |    0.02 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,198.57 ns |  25.813 ns |  1.415 ns |  3.77 |    0.01 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   134.26 ns |  20.246 ns |  1.110 ns |  0.42 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   133.25 ns |  14.018 ns |  0.768 ns |  0.42 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   137.76 ns |   9.638 ns |  0.528 ns |  0.43 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         |   218.92 ns |  46.574 ns |  2.553 ns |  0.69 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,652.05 ns | 304.285 ns | 16.679 ns |  8.34 |    0.05 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   321.16 ns |  29.148 ns |  1.598 ns |  1.01 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    72.77 ns |   1.823 ns |  0.100 ns |  0.23 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    60.72 ns |   0.498 ns |  0.027 ns |  0.19 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    73.18 ns |   8.211 ns |  0.450 ns |  0.23 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   340.02 ns |   0.464 ns |  0.025 ns |  1.07 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   474.18 ns |  19.466 ns |  1.067 ns |  1.49 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   391.71 ns |  22.013 ns |  1.207 ns |  1.23 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    22.96 ns |   0.466 ns |  0.026 ns |  0.07 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.23 ns |   0.568 ns |  0.031 ns |  0.07 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    64.95 ns |   0.294 ns |  0.016 ns |  0.20 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.73 ns |   0.811 ns |  0.044 ns |  0.09 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.70 ns |   0.084 ns |  0.005 ns |  0.10 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.74 ns |   4.534 ns |  0.249 ns |  0.57 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  28.89 ns |  0.420 ns | 0.023 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  12.65 ns |  0.767 ns | 0.042 ns |  0.44 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  42.13 ns |  3.388 ns | 0.186 ns |  1.46 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  72.66 ns |  0.839 ns | 0.046 ns |  2.52 |    0.00 |         - |          NA |
| ChiSquaredSfFractionalDf   | 119.87 ns | 27.281 ns | 1.495 ns |  4.15 |    0.04 |         - |          NA |
| NormalQuantile             |  28.18 ns |  0.792 ns | 0.043 ns |  0.98 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **35.32 μs** |  **7.627 μs** | **0.418 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  55.64 μs |  7.481 μs | 0.410 μs |  1.58 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.94 μs** |  **0.256 μs** | **0.014 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 105.24 μs | 12.401 μs | 0.680 μs |  2.51 |    0.01 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **346.50 μs** | **16.013 μs** | **0.878 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 375.34 μs | 27.526 μs | 1.509 μs |  1.08 |    0.00 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **415.80 μs** | **22.969 μs** | **1.259 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 741.02 μs | 24.266 μs | 1.330 μs |  1.78 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **280.4 μs** |    **15.86 μs** |  **0.87 μs** |   **3.4180** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **3,140.5 μs** |    **47.08 μs** |  **2.58 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **31,508.2 μs** | **1,070.50 μs** | **58.68 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **30.81 μs** |   **4.286 μs** |  **0.235 μs** |  **1.00** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.20 μs |   1.917 μs |  0.105 μs |  1.95 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,212.64 μs** |  **40.262 μs** |  **2.207 μs** |  **1.00** | **328.1250** | **328.1250** | **328.1250** | **1097.77 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,765.05 μs | 309.026 μs | 16.939 μs |  1.84 | 500.0000 | 500.0000 | 500.0000 | 1877.55 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **302.1 μs** | **36.12 μs** | **1.98 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **305.9 μs** |  **4.04 μs** | **0.22 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **305.8 μs** | **10.14 μs** | **0.56 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **28.79 μs** |      **0.631 μs** |     **0.035 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |      97.37 μs |     64.605 μs |     3.541 μs |  3.38 |    0.11 |   1.9531 |   0.1221 |        - |   32.96 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **653.76 μs** |     **53.521 μs** |     **2.934 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,865.87 μs |     79.886 μs |     4.379 μs |  2.85 |    0.01 |  97.6563 |  97.6563 |  97.6563 |  369.98 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,827.31 μs** |    **172.189 μs** |     **9.438 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  24,171.88 μs | 10,115.291 μs |   554.453 μs |  3.54 |    0.07 | 281.2500 | 281.2500 | 281.2500 |  2227.3 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **51,495.77 μs** |    **854.900 μs** |    **46.860 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,082.42 μs | 81,695.832 μs | 4,478.024 μs |  2.74 |    0.08 |        - |        - |        - | 8867.03 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |     **6.737 μs** |   **0.5857 μs** | **0.0321 μs** |  **1.00** |    **0.01** |   **0.1602** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |     9.640 μs |   0.2245 μs | 0.0123 μs |  1.43 |    0.01 |   0.5341 |        - |        - |   8.75 KB |        3.30 |
| Hac       | 100        | 4    |    19.275 μs |   2.3284 μs | 0.1276 μs |  2.86 |    0.02 |   0.7629 |        - |        - |   12.9 KB |        4.87 |
| Cluster   | 100        | 4    |     8.586 μs |   0.3416 μs | 0.0187 μs |  1.27 |    0.01 |   0.5798 |        - |        - |   9.71 KB |        3.67 |
|           |            |      |              |             |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **434.396 μs** |  **22.5522 μs** | **1.2362 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   909.735 μs |  90.7482 μs | 4.9742 μs |  2.09 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.89 KB |        5.91 |
| Hac       | 10000      | 4    | 1,938.105 μs | 114.6969 μs | 6.2869 μs |  4.46 |    0.02 | 246.0938 | 246.0938 | 246.0938 | 863.84 KB |       10.80 |
| Cluster   | 10000      | 4    |   768.125 μs |  77.6042 μs | 4.2538 μs |  1.77 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 565.18 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | SampleSize | Mean         | Error       | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **7.152 μs** |   **0.5511 μs** | **0.0302 μs** |  **0.2823** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **121.875 μs** |  **11.8451 μs** | **0.6493 μs** |  **1.4648** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,285.832 μs** | **110.5349 μs** | **6.0588 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method            | SampleSize | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **32.03 μs** |  **27.173 μs** | **1.489 μs** |  **1.00** |    **0.06** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    33.94 μs |   2.691 μs | 0.148 μs |  1.06 |    0.04 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |            |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,997.02 μs** | **103.268 μs** | **5.660 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,455.42 μs |  25.098 μs | 1.376 μs |  0.73 |    0.00 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,432.72 μs** | **353.427 μs** | **19.373 μs** |      **-** |  **39.23 KB** |
| **TwoSidedExact** | **99x101**   |    **47.04 μs** |   **5.700 μs** |  **0.312 μs** | **0.2441** |   **4.07 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **131.4 μs** |      **16.39 μs** |      **0.90 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     174.6 μs |      24.21 μs |      1.33 μs |  1.33 |    0.01 |   1.9531 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     560.7 μs |     423.12 μs |     23.19 μs |  4.27 |    0.15 | 190.4297 | 190.4297 | 190.4297 |   705.61 KB |        3.75 |
|                   |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **59,390.2 μs** |  **45,493.90 μs** |  **2,493.67 μs** |  **1.00** |    **0.05** | **375.0000** | **375.0000** | **375.0000** |  **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 296,193.9 μs |  22,940.20 μs |  1,257.43 μs |  4.99 |    0.18 |        - |        - |        - |   3499.3 KB |        0.39 |
| MathNet_Qr        | 250        | 475,282.1 μs | 205,081.75 μs | 11,241.22 μs |  8.01 |    0.33 |        - |        - |        - | 32660.53 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | Shape   | Mean     | Error     | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|----------:|--------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **527.1 ms** | **116.56 ms** | **6.39 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **441.5 ms** |  **43.51 ms** | **2.38 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method           | Size | Mean          | Error       | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|------------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.447 μs** |   **0.6584 μs** |  **0.0361 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,566.190 μs |  61.8869 μs |  3.3922 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     19.643 μs |   2.3840 μs |  0.1307 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     94.962 μs |  17.9805 μs |  0.9856 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **18.273 μs** |   **3.0655 μs** |  **0.1680 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,087.106 μs | 477.7927 μs | 26.1894 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    532.739 μs | 271.3294 μs | 14.8725 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,287.730 μs | 178.2124 μs |  9.7684 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar_StudentT**              | **100**        |     **1,008.2 ns** |     **123.60 ns** |      **6.78 ns** |   **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     2,207.2 ns |     705.78 ns |     38.69 ns |   2.19 |    0.04 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,367.7 ns |     883.48 ns |     48.43 ns |   2.35 |    0.04 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     4,046.7 ns |     180.61 ns |      9.90 ns |   4.01 |    0.02 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     7,449.3 ns |     671.36 ns |     36.80 ns |   7.39 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     9,356.5 ns |   3,120.59 ns |    171.05 ns |   9.28 |    0.16 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,541.6 ns |     772.63 ns |     42.35 ns |   2.52 |    0.04 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,099.2 ns |     516.07 ns |     28.29 ns |   4.07 |    0.03 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       974.8 ns |     194.51 ns |     10.66 ns |   0.97 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     3,171.4 ns |      66.34 ns |      3.64 ns |   3.15 |    0.02 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |     1,129.3 ns |     123.57 ns |      6.77 ns |   1.12 |    0.01 | 0.0477 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,080.6 ns |      25.61 ns |      1.40 ns |   1.07 |    0.01 | 0.0572 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       460.8 ns |      64.19 ns |      3.52 ns |   0.46 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,556.9 ns |     326.46 ns |     17.89 ns |   1.54 |    0.02 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |       118.8 ns |      44.55 ns |      2.44 ns |   0.12 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       680.4 ns |     194.87 ns |     10.68 ns |   0.67 |    0.01 | 0.0572 |     968 B |          NA |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,858.5 ns** |   **6,125.89 ns** |    **335.78 ns** |  **1.000** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   178,838.8 ns |   1,637.56 ns |     89.76 ns |  2.939 |    0.01 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   700,352.6 ns |  27,661.97 ns |  1,516.25 ns | 11.508 |    0.06 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 2,081,623.8 ns | 172,554.99 ns |  9,458.32 ns | 34.205 |    0.21 | 3.9063 |   80382 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,487,978.6 ns |  59,589.67 ns |  3,266.31 ns | 24.450 |    0.13 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,636,433.0 ns | 286,938.24 ns | 15,728.05 ns | 59.753 |    0.36 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,457,701.6 ns |  37,946.71 ns |  2,079.99 ns | 23.953 |    0.12 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,150,357.1 ns | 128,206.31 ns |  7,027.42 ns | 35.334 |    0.20 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    84,445.0 ns |   1,241.58 ns |     68.06 ns |  1.388 |    0.01 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   268,724.6 ns |   7,569.18 ns |    414.89 ns |  4.416 |    0.02 |      - |     745 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   197,113.9 ns |  12,246.65 ns |    671.28 ns |  3.239 |    0.02 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   381,281.1 ns |  33,386.42 ns |  1,830.02 ns |  6.265 |    0.04 | 4.3945 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       459.7 ns |      56.71 ns |      3.11 ns |  0.008 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,562.7 ns |     176.44 ns |      9.67 ns |  0.026 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |       123.0 ns |      15.89 ns |      0.87 ns |  0.002 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       691.0 ns |      52.91 ns |      2.90 ns |  0.011 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method   | SampleSize | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|----------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **141.0 μs** |  **21.83 μs** |  **1.20 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   941.9 μs |  49.60 μs |  2.72 μs |  6.68 |    0.05 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |           |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,329.5 μs** |  **34.90 μs** |  **1.91 μs** |  **1.00** |    **0.00** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,476.1 μs | 991.09 μs | 54.32 μs |  5.62 |    0.04 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method       | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |     **6.787 μs** |   **0.2764 μs** |  **0.0151 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   265.466 μs |  13.1705 μs |  0.7219 μs | 39.12 |    0.12 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    31.317 μs |   1.9817 μs |  0.1086 μs |  4.61 |    0.02 |   2.1362 |   0.0610 |        - |   35.61 KB |       13.45 |
|              |            |              |             |            |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **436.406 μs** |  **39.9760 μs** |  **2.1912 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 5,060.867 μs | 219.2535 μs | 12.0180 μs | 11.60 |    0.06 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,722.808 μs | 117.9021 μs |  6.4626 μs |  3.95 |    0.02 | 441.4063 | 441.4063 | 441.4063 | 1764.85 KB |       22.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                       | Mean        | Error      | StdDev    | Ratio     | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |------------:|-----------:|----------:|----------:|--------:|----------:|------------:|
| NormalQuantile               |   0.0421 ns |  0.0706 ns | 0.0039 ns |      1.01 |    0.11 |         - |          NA |
| NormalQuantileFarTail        |   0.0392 ns |  0.0100 ns | 0.0005 ns |      0.93 |    0.07 |         - |          NA |
| StudentQuantile              | 553.4152 ns | 32.4881 ns | 1.7808 ns | 13,202.32 |  998.90 |         - |          NA |
| StudentQuantileCauchyFarTail | 155.9463 ns | 34.4088 ns | 1.8861 ns |  3,720.27 |  283.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0      | Gen1      | Gen2      | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|----------:|----------:|----------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,488.1 μs** |    **42.23 μs** |   **2.31 μs** |         **-** |         **-** |         **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    422.2 μs |    39.72 μs |   2.18 μs |    4.3945 |         - |         - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    616.6 μs |    66.66 μs |   3.65 μs |   83.0078 |   83.0078 |   83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,266.7 μs |    30.25 μs |   1.66 μs |         - |         - |         - |          - |
| MannWhitneyTest           | 10000      | False |    733.4 μs |    34.07 μs |   1.87 μs |         - |         - |         - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **622.5 μs** |     **7.03 μs** |   **0.39 μs** |         **-** |         **-** |         **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    242.1 μs |    43.00 μs |   2.36 μs |    4.6387 |         - |         - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    532.3 μs |    91.23 μs |   5.00 μs |   83.0078 |   83.0078 |   83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    775.3 μs |    33.52 μs |   1.84 μs |         - |         - |         - |          - |
| MannWhitneyTest           | 10000      | True  |    418.1 μs |     9.02 μs |   0.49 μs |         - |         - |         - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,877.6 μs** |   **223.76 μs** |  **12.26 μs** |         **-** |         **-** |         **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,567.5 μs |   154.86 μs |   8.49 μs |  101.5625 |  101.5625 |  101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,566.5 μs | 3,759.21 μs | 206.05 μs |  406.2500 |  406.2500 |  406.2500 |  5062630 B |
| WilcoxonPooledDifferences | 100000     | False | 15,408.2 μs |   346.08 μs |  18.97 μs | 1468.7500 | 1468.7500 | 1468.7500 | 19463348 B |
| MannWhitneyTest           | 100000     | False |  6,985.3 μs |   685.14 μs |  37.55 μs |         - |         - |         - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,066.5 μs** |   **232.25 μs** |  **12.73 μs** |         **-** |         **-** |         **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,841.9 μs |    52.83 μs |   2.90 μs |  113.2813 |  113.2813 |  113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,129.0 μs | 4,829.97 μs | 264.75 μs |  496.0938 |  496.0938 |  496.0938 |  5062685 B |
| WilcoxonPooledDifferences | 100000     | True  | 11,461.8 μs |   760.41 μs |  41.68 μs | 1484.3750 | 1484.3750 | 1484.3750 | 19282298 B |
| MannWhitneyTest           | 100000     | True  |  4,052.1 μs |   345.11 μs |  18.92 μs |         - |         - |         - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | SampleSize | Covariance | Mean         | Error       | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-------------:|------------:|-----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |     **6.788 μs** |   **0.3095 μs** |  **0.0170 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |     **9.744 μs** |   **0.4512 μs** |  **0.0247 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc1**        |     **9.410 μs** |   **0.2427 μs** |  **0.0133 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.164 μs** |   **0.1956 μs** |  **0.0107 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.176 μs** |   **0.2849 μs** |  **0.0156 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **438.510 μs** |  **36.0068 μs** |  **1.9737 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **896.082 μs** |  **93.0700 μs** |  **5.1015 μs** | **124.0234** | **124.0234** | **124.0234** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **906.922 μs** |  **54.9114 μs** |  **3.0099 μs** | **124.0234** | **124.0234** | **124.0234** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,055.840 μs** | **179.3369 μs** |  **9.8301 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,066.686 μs** | **182.7798 μs** | **10.0188 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.451 μs** | **0.6835 μs** | **0.0375 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.602 μs | 2.3843 μs | 0.1307 μs |  2.61 |    0.03 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.863 μs | 0.7518 μs | 0.0412 μs |  1.09 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.178 μs | 0.7513 μs | 0.0412 μs |  2.74 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.957 μs | 0.3820 μs | 0.0209 μs |  1.11 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.670 μs | 0.3993 μs | 0.0219 μs |  2.62 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.852 μs** | **1.1614 μs** | **0.0637 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.938 μs | 2.3597 μs | 0.1293 μs |  2.48 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.248 μs | 0.9864 μs | 0.0541 μs |  1.01 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.643 μs | 5.9137 μs | 0.3241 μs |  2.49 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.665 μs | 1.4744 μs | 0.0808 μs |  1.02 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 121.056 μs | 3.0772 μs | 0.1687 μs |  2.48 |    0.00 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.823 μs** |  **1.7166 μs** | **0.0941 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  31.258 μs |  3.0641 μs | 0.1680 μs |  4.00 |    0.05 |   4.0283 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  25.292 μs |  0.2348 μs | 0.0129 μs |  3.23 |    0.03 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.893 μs |  0.1942 μs | 0.0106 μs |  0.50 |    0.01 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.037 μs |  0.0424 μs | 0.0023 μs |  0.52 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.602 μs |  0.1092 μs | 0.0060 μs |  0.20 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.978 μs |  0.4188 μs | 0.0230 μs |  0.38 |    0.00 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **114.828 μs** |  **4.9165 μs** | **0.2695 μs** |  **1.00** |    **0.00** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 922.245 μs | 84.9147 μs | 4.6545 μs |  8.03 |    0.04 | 249.0234 | 249.0234 | 249.0234 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 289.876 μs |  4.3066 μs | 0.2361 μs |  2.52 |    0.01 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.510 μs |  2.9737 μs | 0.1630 μs |  0.83 |    0.00 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.527 μs |  2.3246 μs | 0.1274 μs |  0.85 |    0.00 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.571 μs |  2.9500 μs | 0.1617 μs |  0.13 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  29.047 μs |  0.9608 μs | 0.0527 μs |  0.25 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **1,158.6 ns** |      **80.47 ns** |      **4.41 ns** |    **1.00** |    **0.00** |   **0.0038** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,417.9 ns |   3,889.49 ns |    213.20 ns |   33.16 |    0.19 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,469.9 ns |      34.07 ns |      1.87 ns |    2.13 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,608.2 ns |   1,867.55 ns |    102.37 ns |   20.38 |    0.10 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        120.2 ns |       5.67 ns |      0.31 ns |    0.10 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        215.9 ns |      12.39 ns |      0.68 ns |    0.19 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **54,182.0 ns** |   **3,148.24 ns** |    **172.57 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    141,843.0 ns |  14,470.55 ns |    793.18 ns |   2.618 |    0.01 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    716,714.1 ns |  26,195.96 ns |  1,435.89 ns |  13.228 |    0.04 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 12,509,788.7 ns | 423,064.24 ns | 23,189.58 ns | 230.886 |    0.74 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        121.4 ns |       6.06 ns |      0.33 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        216.4 ns |      17.30 ns |      0.95 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                   | GroupSize | Mean           | Error        | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|-------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,143.2 ns** |    **547.26 ns** |    **30.00 ns** |  **1.00** |    **0.02** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,083.3 ns |  1,480.22 ns |    81.14 ns |  1.91 |    0.04 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       607.7 ns |     39.97 ns |     2.19 ns |  0.28 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,185.7 ns |     61.70 ns |     3.38 ns |  0.55 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       767.2 ns |     13.61 ns |     0.75 ns |  0.36 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,685.6 ns |    126.82 ns |     6.95 ns |  2.19 |    0.03 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,643.6 ns |    531.34 ns |    29.12 ns |  4.03 |    0.05 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,298.4 ns |    248.74 ns |    13.63 ns |  2.94 |    0.04 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,145.8 ns |    466.92 ns |    25.59 ns |  1.93 |    0.03 |       - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **396,160.5 ns** | **10,492.51 ns** |   **575.13 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,348,212.6 ns |  8,708.54 ns |   477.34 ns | 3.403 |    0.00 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,284.7 ns |    942.50 ns |    51.66 ns | 0.142 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,570.6 ns |  1,700.49 ns |    93.21 ns | 0.284 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,002.4 ns |     47.99 ns |     2.63 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   897,236.5 ns | 51,751.14 ns | 2,836.65 ns | 2.265 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,397,759.8 ns | 31,601.35 ns | 1,732.18 ns | 3.528 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,869.8 ns |    270.12 ns |    14.81 ns | 0.027 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **15.73 μs** |   **1.946 μs** | **0.107 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **38.85 μs** |   **4.490 μs** | **0.246 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **55.40 μs** |  **13.670 μs** | **0.749 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **158.97 μs** |  **24.771 μs** | **1.358 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **206.81 μs** |  **21.554 μs** | **1.181 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **553.88 μs** |  **86.434 μs** | **4.738 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **706.89 μs** | **134.554 μs** | **7.375 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,231.81 μs** |  **78.145 μs** | **4.283 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method       | SampleSize | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **11.31 μs** |     **0.757 μs** |   **0.041 μs** |  **1.00** |    **0.00** |    **0.1984** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     20.35 μs |     5.320 μs |   0.292 μs |  1.80 |    0.02 |    3.6926 |    0.1831 |         - |     58.3 KB |       17.00 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **93.86 μs** |     **3.854 μs** |   **0.211 μs** |  **1.00** |    **0.00** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    125.67 μs |    14.753 μs |   0.809 μs |  1.34 |    0.01 |   32.9590 |   13.9160 |         - |   508.18 KB |       29.05 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **949.81 μs** |   **247.119 μs** |  **13.545 μs** |  **1.00** |    **0.02** |   **46.8750** |   **46.8750** |   **46.8750** |    **158.5 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,581.70 μs | 3,202.745 μs | 175.553 μs |  1.67 |    0.16 | 1015.6250 | 1015.6250 | 1015.6250 |  5027.18 KB |       31.72 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,292.27 μs** | **1,488.205 μs** |  **81.574 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 16,437.74 μs | 4,530.065 μs | 248.308 μs |  1.77 |    0.03 |  750.0000 |  750.0000 |  750.0000 | 50021.97 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

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

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **463.0 μs** |    **16.10 μs** |   **0.88 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,916.1 μs** |    **42.07 μs** |   **2.31 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **5,428.9 μs** |   **512.37 μs** |  **28.08 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **20,448.9 μs** | **2,543.12 μs** | **139.40 μs** | **281.2500** | **250.0000** | **250.0000** |  **1734.9 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

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

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.42 μs** |     **0.982 μs** |  **0.054 μs** |  **1.00** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     15.23 μs |     4.568 μs |  0.250 μs |  0.93 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.75 μs |     1.574 μs |  0.086 μs |  1.32 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **59.59 μs** |     **6.032 μs** |  **0.331 μs** |  **1.00** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.42 μs |     8.178 μs |  0.448 μs |  0.39 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     48.66 μs |     5.018 μs |  0.275 μs |  0.82 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **8,704.26 μs** |   **360.805 μs** | **19.777 μs** |  **1.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  8,536.26 μs | 1,246.004 μs | 68.298 μs |  0.98 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               | 10,004.64 μs |   981.577 μs | 53.804 μs |  1.15 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **15,862.51 μs** |   **969.664 μs** | **53.151 μs** |  **1.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 13,039.73 μs | 1,148.164 μs | 62.935 μs |  0.82 |  843.7500 |  828.1250 | 687.5000 | 5660.23 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 16,195.42 μs |   829.808 μs | 45.485 μs |  1.02 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                  | Mean      | Error     | StdDev   | Gen0      | Allocated |
|------------------------ |----------:|----------:|---------:|----------:|----------:|
| ChatTemplate            | 100.97 ms | 12.566 ms | 0.689 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens |  86.60 ms |  1.648 ms | 0.090 ms | 1666.6667 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                 | Rows | Method   | Mean           | Error        | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,943.5 μs** |     **280.3 μs** |     **15.36 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   103,563.2 μs |  13,028.8 μs |    714.15 μs |  53.29 |    0.49 |  3666.6667 |  1500.0000 |  833.3333 |  59880.73 KB |       58.56 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,833.5 μs** |     **214.5 μs** |     **11.76 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |   100,296.0 μs |  36,714.4 μs |  2,012.44 μs |  54.70 |    1.00 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **798.9 μs** |     **230.2 μs** |     **12.62 μs** |   **1.00** |    **0.02** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   138,543.7 μs |  84,963.3 μs |  4,657.12 μs | 173.45 |    5.59 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,330.2 μs** |     **278.0 μs** |     **15.24 μs** |   **1.00** |    **0.01** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    73,752.2 μs |  57,747.2 μs |  3,165.32 μs |  31.65 |    1.19 |  3857.1429 |  1714.2857 |  857.1429 |  64191.22 KB |       62.78 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **16,345.7 μs** |   **8,626.5 μs** |    **472.85 μs** |   **1.00** |    **0.04** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,813,527.9 μs | 154,018.4 μs |  8,442.27 μs | 111.01 |    2.84 | 35000.0000 | 10000.0000 | 4000.0000 | 537147.58 KB |       60.18 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **13,995.4 μs** |   **1,050.4 μs** |     **57.58 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,713,405.9 μs | 150,401.5 μs |  8,244.01 μs | 122.43 |    0.67 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,451.7 μs** |     **167.6 μs** |      **9.19 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,165,821.7 μs | 289,114.1 μs | 15,847.32 μs | 490.70 |    2.21 | 47000.0000 | 12000.0000 | 4000.0000 | 732673.15 KB |    8,147.89 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,877.7 μs** |   **2,526.8 μs** |    **138.50 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,607,813.9 μs |  94,098.0 μs |  5,157.83 μs |  95.27 |    0.72 | 38000.0000 | 11000.0000 | 4000.0000 | 573613.83 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method           | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 27.66 ms | 19.47 ms | 1.067 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 47.15 ms | 36.55 ms | 2.003 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.93 MB |
| CountLoad4M      | 46.73 ms | 20.69 ms | 1.134 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| CountFitLongDocuments | 177,431.59 μs | 40,469.673 μs | 2,218.279 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     424.73 μs |     95.771 μs |     5.250 μs |    9.2773 |    2.9297 |   2.9297 |   504.67 KB |
| CountSaveAsync240K    |     503.54 μs |     70.626 μs |     3.871 μs |    9.7656 |    3.9063 |   3.9063 |   739.44 KB |
| TfidfSave45K          |      51.23 μs |      2.888 μs |     0.158 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      54.39 μs |      3.088 μs |     0.169 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     300.25 μs |     60.841 μs |     3.335 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     286.48 μs |     46.856 μs |     2.568 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,784.35 μs | 38,659.659 μs | 2,119.066 μs |  593.7500 |  562.5000 | 187.5000 |  22048.8 KB |
| CountLoadAsync4M      |  56,721.72 μs | 26,529.222 μs | 1,454.156 μs | 1100.0000 | 1000.0000 | 300.0000 | 44983.52 KB |
| CountLoad4M           |  51,104.33 μs | 22,049.221 μs | 1,208.592 μs | 1100.0000 | 1000.0000 | 300.0000 |  44976.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| TfidfSaveAsyncSmall    |     18.272 μs |    16.8865 μs |   0.9256 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    501.110 μs |   129.9107 μs |   7.1208 μs |   35.1563 |  29.2969 |  29.2969 |   740.13 KB |
| HashingLoadUndeclared  |      5.729 μs |     0.1608 μs |   0.0088 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     30.387 μs |     7.7689 μs |   0.4258 μs |    7.3853 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    129.573 μs |    22.4383 μs |   1.2299 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    850.967 μs |   746.9098 μs |  40.9406 μs |   38.0859 |  37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,054.515 μs | 2,504.5029 μs | 137.2803 μs |  515.6250 | 500.0000 | 296.8750 | 10793.92 KB |
| CountSaveAsync1M       |  2,454.067 μs |   924.9651 μs |  50.7005 μs |  109.3750 | 101.5625 | 101.5625 |  3162.04 KB |
| IndexSaveAsyncLongHead |  3,440.599 μs |   654.4688 μs |  35.8736 μs |   93.7500 |  89.8438 |  89.8438 |  2124.36 KB |
| CountLoadUndeclared2M  | 19,519.867 μs | 2,008.7314 μs | 110.1053 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.12 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.964 μs** |  **0.4599 μs** | **0.0252 μs** |  **1.00** |    **0.01** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.233 μs |  0.6233 μs | 0.0342 μs |  1.05 |    0.01 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   6.150 μs |  0.7754 μs | 0.0425 μs |  1.03 |    0.01 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **59.159 μs** |  **7.8402 μs** | **0.4298 μs** |  **1.00** |    **0.01** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  22.575 μs |  1.5865 μs | 0.0870 μs |  0.38 |    0.00 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  22.616 μs |  4.6747 μs | 0.2562 μs |  0.38 |    0.00 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **232.333 μs** | **64.2683 μs** | **3.5228 μs** |  **1.00** |    **0.02** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  84.371 μs | 14.5624 μs | 0.7982 μs |  0.36 |    0.01 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  73.096 μs |  5.2529 μs | 0.2879 μs |  0.31 |    0.00 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **910.750 μs** | **55.4149 μs** | **3.0375 μs** |  **1.00** |    **0.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 331.808 μs | 39.7734 μs | 2.1801 μs |  0.36 |    0.00 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 281.834 μs | 25.4979 μs | 1.3976 μs |  0.31 |    0.00 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | Text     | Lowercase | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|---------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **36.33 ms** | **4.067 ms** | **0.223 ms** |  **714.2857** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **40.86 ms** | **4.764 ms** | **0.261 ms** |  **692.3077** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **35.83 ms** | **2.137 ms** | **0.117 ms** |  **714.2857** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **49.91 ms** | **0.714 ms** | **0.039 ms** | **1200.0000** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.90 ms** | **3.444 ms** | **0.189 ms** | **1307.6923** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **54.38 ms** | **2.928 ms** | **0.160 ms** | **1500.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **134.11 ms** | **19.718 ms** | **1.081 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.38 ms |  2.792 ms | 0.153 ms |  0.49 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **141.58 ms** | **53.754 ms** | **2.946 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  62.03 ms |  3.637 ms | 0.199 ms |  0.44 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **200.58 ms** | **10.575 ms** | **0.580 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 243.24 ms |  8.264 ms | 0.453 ms |  1.21 |    0.00 |   258.9 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **206.22 ms** |  **9.802 ms** | **0.537 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 212.17 ms | 15.242 ms | 0.835 ms |  1.03 |    0.00 |  192.71 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **254.58 ms** | **16.435 ms** | **0.901 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 344.63 ms | 36.261 ms | 1.988 ms |  1.35 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **260.01 ms** | **13.634 ms** | **0.747 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 314.35 ms |  4.405 ms | 0.241 ms |  1.21 |    0.00 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **293.88 ms** | **22.719 ms** | **1.245 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 396.06 ms | 18.107 ms | 0.993 ms |  1.35 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **299.40 ms** | **24.905 ms** | **1.365 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 392.67 ms | 31.416 ms | 1.722 ms |  1.31 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **44.73 μs** |      **0.345 μs** |     **0.019 μs** |         **-** |
| Cjk    | 1000   |      63.34 μs |      1.083 μs |     0.059 μs |         - |
| **Latin**  | **10000**  |   **4,122.74 μs** |     **67.946 μs** |     **3.724 μs** |         **-** |
| Cjk    | 10000  |   6,826.37 μs |  1,147.257 μs |    62.885 μs |         - |
| **Latin**  | **65536**  | **185,165.24 μs** | **30,348.469 μs** | **1,663.502 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github (run 2)

_As of 2026-08-27, measured at commit `2483f1a00691271083f00baf3835de96bf0a4076`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host]   : .NET 10.0.11 (10.0.1126.37416), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.11 (10.0.1126.37416), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **51.93 μs** |     **0.631 μs** |   **0.035 μs** |         **-** |
| Cjk    | 1000   |      56.06 μs |     2.402 μs |   0.132 μs |         - |
| **Latin**  | **10000**  |   **5,514.80 μs** | **1,122.506 μs** |  **61.528 μs** |         **-** |
| Cjk    | 10000  |   6,234.71 μs |   289.373 μs |  15.862 μs |         - |
| **Latin**  | **65536**  | **202,152.38 μs** | **5,826.234 μs** | **319.356 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error         | StdDev      | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|--------------:|------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.415 μs** |     **0.5015 μs** |   **0.0275 μs** |     **1.00** |    **0.02** |    **0.0248** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.165 μs |     1.0193 μs |   0.0559 μs |     1.53 |    0.04 |    0.0229 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       2.546 μs |     0.6739 μs |   0.0369 μs |     1.80 |    0.04 |    0.3128 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   3,072.235 μs | 2,876.2947 μs | 157.6594 μs | 2,171.98 |  103.36 |  906.2500 |  875.0000 |  875.0000 |  3723191 B |    8,781.11 |
| LuceneFromText         | 1000      |   5,816.089 μs |   508.5427 μs |  27.8749 μs | 4,111.81 |   71.95 |   85.9375 |   78.1250 |    7.8125 |  1375127 B |    3,243.22 |
|                        |           |                |               |             |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **21.804 μs** |     **0.9885 μs** |   **0.0542 μs** |     **1.00** |    **0.00** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      32.090 μs |     3.2079 μs |   0.1758 μs |     1.47 |    0.01 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      14.133 μs |     1.4614 μs |   0.0801 μs |     0.65 |    0.00 |    0.5035 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  52,807.099 μs | 7,521.9632 μs | 412.3042 μs | 2,421.92 |   17.19 | 1700.0000 | 1700.0000 | 1700.0000 | 61665538 B |  145,437.59 |
| LuceneFromText         | 20000     | 116,145.104 μs | 5,633.7369 μs | 308.8041 μs | 5,326.83 |   16.80 | 1200.0000 | 1000.0000 |         - | 22417157 B |   52,870.65 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| Unigram | 30.02 ms |  5.184 ms | 0.284 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 94.74 ms | 39.726 ms | 2.178 ms |  3.16 |    0.07 | 1666.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **BpeOnOnePathologicalToken** | **512**    |  **18.71 μs** |  **0.565 μs** | **0.031 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **66.04 μs** |  **2.125 μs** | **0.116 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **180.91 μs** | **14.670 μs** | **0.804 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **389.04 μs** | **25.786 μs** | **1.413 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| EncodeUnseenProse | 9.555 ms | 0.9741 ms | 0.0534 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **DpGroup**    | **cjk**      |  **19.305 μs** |  **3.034 μs** | **0.1663 μs** |         **-** |
| MyersGroup | cjk      | 241.918 μs | 42.707 μs | 2.3409 μs |         - |
| **DpGroup**    | **latin**    |   **9.717 μs** |  **1.105 μs** | **0.0605 μs** |         **-** |
| MyersGroup | latin    | 133.242 μs | 12.117 μs | 0.6642 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **308.20 μs** | **33.939 μs** | **1.860 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 302.81 μs |  7.483 μs | 0.410 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **329.23 μs** | **18.219 μs** | **0.999 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 319.90 μs | 34.501 μs | 1.891 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **319.38 μs** | **63.202 μs** | **3.464 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 306.96 μs | 69.507 μs | 3.810 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **274.10 μs** | **22.520 μs** | **1.234 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 322.08 μs | 10.355 μs | 0.568 μs |  1.18 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **314.83 μs** | **86.216 μs** | **4.726 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 308.07 μs |  6.672 μs | 0.366 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **162.99 μs** |  **7.092 μs** | **0.389 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 327.61 μs | 14.088 μs | 0.772 μs |  2.01 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **230.92 μs** |  **8.589 μs** | **0.471 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 294.42 μs | 25.228 μs | 1.383 μs |  1.27 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **92.64 μs** | **17.693 μs** | **0.970 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 320.67 μs | 49.145 μs | 2.694 μs |  3.46 |    0.04 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar_Cdist**                | **50**   |   **182.4 μs** |  **27.45 μs** |  **1.50 μs** |  **1.00** |    **0.01** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   176.8 μs |   8.15 μs |  0.45 μs |  0.97 |    0.01 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   473.4 μs |  16.34 μs |  0.90 μs |  2.59 |    0.02 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,269.0 μs** | **840.11 μs** | **46.05 μs** |  **1.00** |    **0.02** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,210.1 μs | 819.27 μs | 44.91 μs |  0.98 |    0.02 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,741.7 μs | 740.98 μs | 40.62 μs |  2.37 |    0.03 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error        | StdDev      | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|-------------:|------------:|-------:|-------:|----------:|
| **Report** | **10**      |     **577.8 ns** |     **94.61 ns** |     **5.19 ns** | **0.0935** |      **-** |   **1.54 KB** |
| **Report** | **1000**    | **690,140.0 ns** | **28,041.95 ns** | **1,537.07 ns** | **6.8359** | **0.9766** | **117.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 137.5 ms | 11.17 ms | 0.61 ms | 750.0000 | 750.0000 | 750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 124.6 ms | 10.61 ms | 0.58 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                         | Samples | Clusters | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.132 ms** |  **0.1380 ms** | **0.0076 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.151 ms |  0.2474 ms | 0.0136 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.528 ms |  0.3826 ms | 0.0210 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.028 ms** |  **0.2518 ms** | **0.0138 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.203 ms |  1.3212 ms | 0.0724 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.440 ms | 17.3897 ms | 0.9532 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method              | Length | Mean            | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **908.5 ns** |      **28.50 ns** |      **1.56 ns** |  **1.00** |    **0.00** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,895.4 ns |      58.65 ns |      3.21 ns |  2.09 |    0.00 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **69,344.9 ns** |  **10,568.02 ns** |    **579.27 ns** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    154,735.6 ns |   3,548.05 ns |    194.48 ns |  2.23 |    0.02 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,514,031.3 ns** | **522,796.74 ns** | **28,656.25 ns** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,379,178.6 ns | 402,612.89 ns | 22,068.57 ns |  2.74 |    0.02 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar_Fit** | **10000x8x8** |   **155.71 ms** |  **98.227 ms** | **5.384 ms** |  **1.00** |    **0.04** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,324.94 ms | 129.531 ms | 7.100 ms | 14.94 |    0.44 | 14000.0000 | 8000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |            |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **54.22 ms** |   **3.046 ms** | **0.167 ms** |  **1.00** |    **0.00** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   814.04 ms |  20.549 ms | 1.126 ms | 15.01 |    0.04 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                   | Shape      | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **163.46 ms** |  **49.149 ms** | **2.694 ms** |  **1.00** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,679.47 ms | 126.575 ms | 6.938 ms | 40.87 |    0.59 | 38000.0000 | 14000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,885.39 ms |  84.642 ms | 4.640 ms | 23.77 |    0.34 | 21000.0000 | 11000.0000 |  9000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |          |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **18.98 ms** |   **0.694 ms** | **0.038 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   474.47 ms |  51.365 ms | 2.816 ms | 24.99 |    0.14 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   269.32 ms |  76.108 ms | 4.172 ms | 14.19 |    0.19 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                                    | Mean      | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       | 13.330 ms | 0.6252 ms | 0.0343 ms |  1.00 |    0.00 |
| Nmf_Rank20                                | 95.925 ms | 2.2844 ms | 0.1252 ms |  7.20 |    0.02 |
| Nmf_Transform                             |  3.172 ms | 0.1495 ms | 0.0082 ms |  0.24 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 24.835 ms | 2.0554 ms | 0.1127 ms |  1.86 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error       | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **20,712.3 μs** |    **343.3 μs** |    **18.82 μs** | **3031.2500** | **3000.0000** | **3000.0000** |  **12151.25 KB** |
| TruncatedSvd_Transform     | 500     |     549.9 μs |    105.0 μs |     5.76 μs |   94.7266 |   90.8203 |   90.8203 |    390.73 KB |
| Nmf_Frobenius_Rank20       | 500     | 189,099.4 μs | 16,053.1 μs |   879.92 μs | 4500.0000 | 4500.0000 | 4500.0000 |   18143.4 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 252,092.6 μs | 16,264.0 μs |   891.48 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18596.16 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **189,667.9 μs** |  **6,631.7 μs** |   **363.51 μs** | **2666.6667** | **2666.6667** | **2666.6667** | **105766.49 KB** |
| TruncatedSvd_Transform     | 20000   |   1,898.6 μs |    633.2 μs |    34.71 μs |  496.0938 |  496.0938 |  496.0938 |   3437.86 KB |
| Nmf_Frobenius_Rank20       | 20000   | 786,090.9 μs | 29,795.0 μs | 1,633.16 μs | 5000.0000 | 5000.0000 | 5000.0000 | 156850.69 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 634,963.3 μs | 34,318.6 μs | 1,881.12 μs | 5000.0000 | 5000.0000 | 5000.0000 |  157305.7 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 288.2 μs | 16.45 μs | 0.90 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method      | Count  | Mean       | Error      | StdDev   | Allocated |
|------------ |------- |-----------:|-----------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **526.3 μs** |   **111.8 μs** |  **6.13 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **7,653.2 μs** | **1,409.1 μs** | **77.24 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method              | Rows   | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|------------:|----------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **698.1 μs** |    **12.42 μs** |   **0.68 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  7,183.2 μs |    92.21 μs |   5.05 μs |        - |        - |        - |     24.93 KB |
| **FromBlockNormalized** | **100000** | **57,651.9 μs** | **3,113.93 μs** | **170.69 μs** | **222.2222** | **222.2222** | **222.2222** | **150000.24 KB** |
| MmrSelect100        | 100000 |  7,240.2 μs |   223.54 μs |  12.25 μs |        - |        - |        - |     24.93 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]     : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  Job-KCLZQT : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-KCLZQT** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **290.36 μs** |    **14.569 μs** |    **16.193 μs** |    **287.97 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    386.78 μs |    65.247 μs |    72.522 μs |    399.56 μs |  1.34 |    0.25 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    112.04 μs |    14.252 μs |    16.413 μs |    108.74 μs |  0.39 |    0.06 |        - |        - |        - |   31.85 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    419.38 μs |    21.787 μs |    25.090 μs |    419.78 μs |  1.45 |    0.11 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,692.83 μs |    79.118 μs |    87.940 μs |  1,682.95 μs |  5.85 |    0.42 |        - |        - |        - |     338 KB |        2.03 |
| MlNet_ReplaceMissingValues_Read | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,291.20 μs |    72.765 μs |    74.724 μs |  1,306.38 μs |  4.46 |    0.34 |        - |        - |        - |  311.92 KB |        1.87 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    172.25 μs |    15.091 μs |     0.827 μs |    172.56 μs |  1.00 |    0.01 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     70.95 μs |     6.108 μs |     0.335 μs |     70.96 μs |  0.41 |    0.00 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     17.40 μs |     1.164 μs |     0.064 μs |     17.41 μs |  0.10 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    968.53 μs | 2,528.888 μs |   138.617 μs |    963.86 μs |  5.62 |    0.70 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    936.83 μs | 1,051.480 μs |    57.635 μs |    964.48 μs |  5.44 |    0.29 |  48.8281 |   7.8125 |        - |  793.84 KB |        4.79 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    319.88 μs |   354.473 μs |    19.430 μs |    325.16 μs |  1.86 |    0.10 |  17.5781 |  15.6250 |        - |  293.06 KB |        1.77 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-KCLZQT** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,727.59 μs** |   **163.852 μs** |   **175.320 μs** |  **5,624.36 μs** |  **1.00** |    **0.04** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,435.94 μs |   714.106 μs |   793.727 μs |  4,966.14 μs |  0.78 |    0.14 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 20000    |    696.30 μs |    13.502 μs |    14.447 μs |    692.84 μs |  0.12 |    0.00 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,683.81 μs |    97.301 μs |    95.563 μs |  4,663.72 μs |  0.82 |    0.03 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,471.62 μs | 1,635.376 μs | 1,679.411 μs | 18,296.72 μs |  3.05 |    0.30 |        - |        - |        - |  525.28 KB |        0.16 |
| MlNet_ReplaceMissingValues_Read | Job-KCLZQT | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,055.73 μs | 4,928.100 μs | 5,675.207 μs | 13,220.18 μs |  1.93 |    0.97 |        - |        - |        - |  456.54 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,758.62 μs |   292.236 μs |    16.018 μs |  3,761.67 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3291.57 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,151.74 μs |   226.389 μs |    12.409 μs |  3,155.34 μs |  0.84 |    0.00 |  89.8438 |  89.8438 |  89.8438 |  314.42 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    437.23 μs |   217.304 μs |    11.911 μs |    434.25 μs |  0.12 |    0.00 | 129.3945 | 129.3945 | 129.3945 |   626.2 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,550.36 μs | 1,162.832 μs |    63.739 μs |  1,544.43 μs |  0.41 |    0.01 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,148.03 μs |   650.459 μs |    35.654 μs |  4,147.21 μs |  1.10 |    0.01 |  31.2500 |  15.6250 |        - |  567.76 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,519.20 μs |   779.704 μs |    42.738 μs |  2,499.08 μs |  0.67 |    0.01 |  27.3438 |  11.7188 |        - |  457.88 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method          | Records | Mean        | Error       | StdDev   | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|---------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **472.7 μs** |   **566.27 μs** | **31.04 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    547.7 μs |    56.78 μs |  3.11 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    642.3 μs |    48.33 μs |  2.65 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,497.7 μs |   488.77 μs | 26.79 μs | 9.7656 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **8,886.1 μs** |   **406.08 μs** | **22.26 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  |  7,954.0 μs | 1,370.70 μs | 75.13 μs |      - |    2.95 KB |
| HybridSelective | 100000  |  8,614.1 μs |   910.46 μs | 49.91 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 16,667.4 μs | 1,314.47 μs | 72.05 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| Ratio          |    97.94 ns |   1.486 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   751.40 ns |  75.555 ns | 4.141 ns |  7.67 |    0.04 |      - |         - |          NA |
| TokenSortRatio |   863.62 ns | 133.053 ns | 7.293 ns |  8.82 |    0.06 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   924.08 ns | 124.193 ns | 6.807 ns |  9.43 |    0.06 | 0.0534 |     896 B |          NA |
| WRatio         | 1,297.90 ns | 120.444 ns | 6.602 ns | 13.25 |    0.06 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                    | Mean               | Error              | StdDev            | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|------------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,038,584.6764 ns |     18,378.9352 ns |     1,007.4115 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    666,562.4105 ns |     77,102.4190 ns |     4,226.2438 ns |  0.327 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,120,643.4961 ns |     73,068.6644 ns |     4,005.1401 ns |  0.550 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 71,815,277.5833 ns | 20,148,159.5431 ns | 1,104,388.6159 ns | 35.228 |    0.47 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |         0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method     | Operation     | Mean         | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.96 ns** |  **20.94 ns** |  **1.148 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    206.19 ns |  11.30 ns |  0.619 ns |  2.08 |    0.02 | 0.0048 |      80 B |          NA |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **760.16 ns** |  **94.26 ns** |  **5.166 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,541.72 ns | 627.37 ns | 34.388 ns | 13.87 |    0.09 |      - |     160 B |          NA |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,019.69 ns** | **151.72 ns** |  **8.317 ns** |  **1.00** |    **0.01** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,143.75 ns | 165.61 ns |  9.077 ns |  2.10 |    0.02 | 0.1144 |    1944 B |        2.17 |
|            |               |              |           |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,206.79 ns** |  **78.23 ns** |  **4.288 ns** |  **1.00** |    **0.00** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,914.38 ns | 133.73 ns |  7.330 ns |  4.07 |    0.01 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.629 ms** | **0.1298 ms** | **0.0071 ms** | **277.3438** | **259.7656** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **17.176 ms** | **0.6729 ms** | **0.0369 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.34 ns** |   **0.677 ns** |  **0.037 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.10 ns |   0.579 ns |  0.032 ns |  1.56 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    28.32 ns |   0.852 ns |  0.047 ns |  1.08 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.35 ns |   1.359 ns |  0.074 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.39 ns** |   **0.594 ns** |  **0.033 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.85 ns |   0.991 ns |  0.054 ns |  1.47 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.44 ns |   0.128 ns |  0.007 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.62 ns |   0.225 ns |  0.012 ns |  0.97 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **31.62 ns** |   **3.046 ns** |  **0.167 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.67 ns |   9.134 ns |  0.501 ns |  1.44 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    35.18 ns |   0.793 ns |  0.043 ns |  1.11 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.16 ns |   6.664 ns |  0.365 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **34.83 ns** |   **0.157 ns** |  **0.009 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    48.35 ns |   8.376 ns |  0.459 ns |  1.39 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    36.31 ns |   0.241 ns |  0.013 ns |  1.04 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    32.89 ns |   0.709 ns |  0.039 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **58.32 ns** |   **9.969 ns** |  **0.546 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    61.59 ns |   5.920 ns |  0.325 ns |  1.06 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.68 ns |  18.415 ns |  1.009 ns |  0.99 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    56.99 ns |  15.451 ns |  0.847 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **62.52 ns** |   **0.530 ns** |  **0.029 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.74 ns |   2.182 ns |  0.120 ns |  1.12 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    67.03 ns |  11.501 ns |  0.630 ns |  1.07 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    62.13 ns |  12.636 ns |  0.693 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **405.30 ns** |   **4.344 ns** |  **0.238 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   421.04 ns | 108.933 ns |  5.971 ns |  1.04 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   407.91 ns |   9.492 ns |  0.520 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   404.38 ns |   2.505 ns |  0.137 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,680.65 ns** |  **52.577 ns** |  **2.882 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,889.40 ns | 358.746 ns | 19.664 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,613.86 ns | 189.085 ns | 10.364 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,694.95 ns | 795.059 ns | 43.580 ns |  1.00 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **389.9 ns** |    **67.53 ns** |   **3.70 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    190.2 ns |    42.03 ns |   2.30 ns |  0.49 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,415.8 ns** |    **53.90 ns** |   **2.95 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,050.9 ns |    66.99 ns |   3.67 ns |  1.26 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,370.2 ns** |   **322.85 ns** |  **17.70 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,518.2 ns | 6,663.54 ns | 365.25 ns |  2.36 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,738.1 ns** | **4,025.28 ns** | **220.64 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  9,145.6 ns | 3,538.23 ns | 193.94 ns |  1.05 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 12,021.5 ns | 1,412.59 ns |  77.43 ns |  1.38 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,998.8 ns** | **4,598.80 ns** | **252.08 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,387.6 ns | 1,306.56 ns |  71.62 ns |  0.94 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,588.3 ns | 1,274.46 ns |  69.86 ns |  1.46 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,214.4 ns** |   **272.82 ns** |  **14.95 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,435.0 ns | 1,145.02 ns |  62.76 ns |  1.02 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,245.3 ns | 2,331.98 ns | 127.82 ns |  1.49 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,365.0 ns** | **3,687.45 ns** | **202.12 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,615.2 ns |   839.00 ns |  45.99 ns |  1.23 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,467.6 ns |   699.11 ns |  38.32 ns |  1.77 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **6,983.3 ns** | **4,126.17 ns** | **226.17 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,206.8 ns | 3,129.57 ns | 171.54 ns |  1.03 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,337.0 ns | 1,451.21 ns |  79.55 ns |  1.62 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,032.0 ns** | **3,536.16 ns** | **193.83 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,200.4 ns | 6,560.74 ns | 359.62 ns |  1.02 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,699.6 ns | 2,343.71 ns | 128.47 ns |  1.74 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,771.8 ns** |    **73.29 ns** |   **4.02 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,068.0 ns |   394.13 ns |  21.60 ns |  1.08 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,308.1 ns |   232.17 ns |  12.73 ns |  1.94 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,384.2 ns** |   **567.32 ns** |  **31.10 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,269.2 ns |   749.51 ns |  41.08 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,899.1 ns |   839.57 ns |  46.02 ns |  1.84 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,941.5 ns** |    **43.82 ns** |   **2.40 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,531.2 ns |   244.63 ns |  13.41 ns |  0.95 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 14,236.8 ns |   674.39 ns |  36.97 ns |  1.79 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,323.0 ns** |    **18.25 ns** |   **1.00 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,134.5 ns |   109.50 ns |   6.00 ns |  0.92 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,072.4 ns |   311.08 ns |  17.05 ns |  2.61 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,071.2 ns** |   **120.50 ns** |   **6.60 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,863.7 ns |   214.60 ns |  11.76 ns |  0.70 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,668.9 ns | 1,346.45 ns |  73.80 ns |  2.13 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,538.0 ns** |   **174.02 ns** |   **9.54 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,836.9 ns |   306.64 ns |  16.81 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,811.5 ns |   598.51 ns |  32.81 ns |  1.96 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,638.6 ns** |    **75.77 ns** |   **4.15 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,417.7 ns |   276.09 ns |  15.13 ns |  0.94 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,325.9 ns |   242.71 ns |  13.30 ns |  2.01 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,357.3 ns** |    **69.62 ns** |   **3.82 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,130.6 ns |   249.44 ns |  13.67 ns |  0.77 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,239.3 ns |   733.12 ns |  40.19 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,557.4 ns** |   **162.32 ns** |   **8.90 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,210.5 ns |   650.49 ns |  35.66 ns |  1.09 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,506.5 ns | 1,925.68 ns | 105.55 ns |  1.92 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,300.6 ns** |    **40.55 ns** |   **2.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    609.0 ns |    29.63 ns |   1.62 ns |  0.47 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,321.9 ns |   195.06 ns |  10.69 ns |  4.09 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,085.4 ns** |    **49.55 ns** |   **2.72 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,428.8 ns |   176.63 ns |   9.68 ns |  0.46 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,847.8 ns |   306.61 ns |  16.81 ns |  2.54 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,187.6 ns** |    **55.17 ns** |   **3.02 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,386.3 ns |   124.67 ns |   6.83 ns |  0.46 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,049.4 ns |   828.95 ns |  45.44 ns |  2.32 |    0.01 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,439.4 μs** |    **585.15 μs** |    **32.07 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  39,365.3 μs |  4,307.10 μs |   236.09 μs |  7.24 |    0.05 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  83,550.0 μs |  3,263.67 μs |   178.89 μs | 15.36 |    0.08 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **793.8 μs** |     **54.54 μs** |     **2.99 μs** |  **1.00** |    **0.00** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,769.4 μs |    153.38 μs |     8.41 μs |  6.01 |    0.02 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,054.3 μs |    509.27 μs |    27.91 μs |  3.85 |    0.03 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,328.8 μs** |  **1,766.96 μs** |    **96.85 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 441,388.8 μs | 57,507.17 μs | 3,152.16 μs | 16.76 |    0.12 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 744,914.0 μs | 66,516.74 μs | 3,646.01 μs | 28.29 |    0.15 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.049 ms** |  **0.1936 ms** | **0.0106 ms** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.632 ms |  0.5668 ms | 0.0311 ms |  3.29 |    0.01 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.409 ms** |  **3.1353 ms** | **0.1719 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 112.440 ms |  1.2059 ms | 0.0661 ms |  4.26 |    0.02 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.871 ms** |  **6.4612 ms** | **0.3542 ms** |  **1.00** |    **0.01** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 207.864 ms | 10.1287 ms | 0.5552 ms |  4.43 |    0.03 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Dp**         | **8**    |    **133.53 ns** |     **9.836 ns** |   **0.539 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.89 ns |     4.536 ns |   0.249 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.38 ns |    15.246 ns |   0.836 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |     97.92 ns |    10.263 ns |   0.563 ns |  0.73 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **216.48 ns** |    **11.691 ns** |   **0.641 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     62.71 ns |     0.811 ns |   0.044 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    227.29 ns |   210.724 ns |  11.551 ns |  1.05 |    0.05 |         - |          NA |
| Kernel_Cjk | 12   |    110.16 ns |     5.553 ns |   0.304 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **274.98 ns** |   **239.775 ns** |  **13.143 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 14   |     66.18 ns |     0.213 ns |   0.012 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    284.11 ns |   244.040 ns |  13.377 ns |  1.03 |    0.06 |         - |          NA |
| Kernel_Cjk | 14   |    114.74 ns |     4.582 ns |   0.251 ns |  0.42 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **354.44 ns** |    **89.536 ns** |   **4.908 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 16   |     80.45 ns |     4.992 ns |   0.274 ns |  0.23 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    351.03 ns |    53.305 ns |   2.922 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    119.29 ns |     2.060 ns |   0.113 ns |  0.34 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **438.37 ns** |    **91.155 ns** |   **4.997 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.72 ns |     1.055 ns |   0.058 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    443.11 ns |   167.549 ns |   9.184 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 18   |    123.08 ns |     1.796 ns |   0.098 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **773.39 ns** |    **35.399 ns** |   **1.940 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     79.33 ns |     6.898 ns |   0.378 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    753.96 ns |   137.389 ns |   7.531 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    134.37 ns |     2.375 ns |   0.130 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **991.52 ns** |    **82.904 ns** |   **4.544 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     87.18 ns |    21.580 ns |   1.183 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    983.00 ns |   427.378 ns |  23.426 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    139.80 ns |     2.582 ns |   0.142 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,558.96 ns** |   **439.142 ns** |  **24.071 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 32   |    105.75 ns |     7.827 ns |   0.429 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,558.19 ns |   512.154 ns |  28.073 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |    167.50 ns |     0.589 ns |   0.032 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,207.60 ns** |    **48.364 ns** |   **2.651 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    132.97 ns |     4.185 ns |   0.229 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,306.59 ns | 3,892.702 ns | 213.372 ns |  1.03 |    0.06 |         - |          NA |
| Kernel_Cjk | 48   |    240.82 ns |     6.736 ns |   0.369 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,773.81 ns** | **9,645.424 ns** | **528.698 ns** |  **1.01** |    **0.11** |         **-** |          **NA** |
| Kernel     | 64   |    163.20 ns |     2.684 ns |   0.147 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,494.60 ns |   108.517 ns |   5.948 ns |  0.96 |    0.08 |         - |          NA |
| Kernel_Cjk | 64   |    290.08 ns |     6.375 ns |   0.349 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,833.37 ns** |   **703.312 ns** |  **38.551 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    351.14 ns |     6.217 ns |   0.341 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,164.81 ns | 3,661.771 ns | 200.714 ns |  1.03 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    942.78 ns |    15.878 ns |   0.870 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.65 ns** |     **0.574 ns** |  **0.031 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.77 ns |     0.847 ns |  0.046 ns |  1.00 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    121.60 ns |    10.706 ns |  0.587 ns |  4.93 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.04 ns |     0.155 ns |  0.009 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **277.22 ns** |    **20.428 ns** |  **1.120 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    363.06 ns |     6.187 ns |  0.339 ns |  1.31 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    664.29 ns |   106.362 ns |  5.830 ns |  2.40 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    276.11 ns |     7.118 ns |  0.390 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **916.29 ns** |    **27.611 ns** |  **1.513 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,994.82 ns | 1,656.128 ns | 90.778 ns |  2.18 |    0.09 |         - |          NA |
| Distance_CodePoint         | 128    |  1,611.76 ns |   161.703 ns |  8.863 ns |  1.76 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    925.88 ns |   108.595 ns |  5.952 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,079.00 ns** |   **480.597 ns** | **26.343 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,552.05 ns |   685.884 ns | 37.596 ns |  1.49 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 13,290.86 ns |   381.873 ns | 20.932 ns |  1.20 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,988.56 ns |   716.176 ns | 39.256 ns |  0.99 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **330.7 ns** |     **31.06 ns** |     **1.70 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     238.3 ns |      3.64 ns |     0.20 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **353.9 ns** |     **43.01 ns** |     **2.36 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     243.3 ns |     50.64 ns |     2.78 ns |  0.69 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **429.5 ns** |      **9.42 ns** |     **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.4 ns |      2.65 ns |     0.15 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **426.4 ns** |     **14.82 ns** |     **0.81 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     324.6 ns |      1.53 ns |     0.08 ns |  0.76 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **500.7 ns** |     **29.11 ns** |     **1.60 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     410.2 ns |     61.36 ns |     3.36 ns |  0.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **518.9 ns** |     **17.81 ns** |     **0.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     418.3 ns |      3.75 ns |     0.21 ns |  0.81 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **676.6 ns** |    **457.94 ns** |    **25.10 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,296.3 ns |     50.06 ns |     2.74 ns |  1.92 |    0.06 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **608.2 ns** |     **93.59 ns** |     **5.13 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,306.1 ns |     65.06 ns |     3.57 ns |  2.15 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,960.4 ns** |    **763.18 ns** |    **41.83 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,455.1 ns |    186.17 ns |    10.20 ns |  2.78 |    0.05 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,939.3 ns** |     **95.46 ns** |     **5.23 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,483.8 ns |    731.25 ns |    40.08 ns |  2.83 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,963.1 ns** |    **959.42 ns** |    **52.59 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,232.5 ns |    793.06 ns |    43.47 ns |  3.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **448,725.7 ns** | **54,911.84 ns** | **3,009.90 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  61,652.8 ns |  1,005.88 ns |    55.14 ns |  0.14 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method               | Length | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **24.68 ns** |       **0.171 ns** |     **0.009 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      85.22 ns |      21.123 ns |     1.158 ns |  3.45 |    0.04 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      83.34 ns |       4.391 ns |     0.241 ns |  3.38 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     183.79 ns |       6.272 ns |     0.344 ns |  7.45 |    0.01 | 0.0076 |     128 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **277.08 ns** |      **45.346 ns** |     **2.486 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,233.73 ns |     654.459 ns |    35.873 ns | 22.50 |    0.21 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,422.79 ns |      68.320 ns |     3.745 ns |  5.14 |    0.04 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,292.65 ns |   2,111.410 ns |   115.734 ns | 37.15 |    0.46 | 0.0305 |     576 B |          NA |
|                      |        |               |                |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,201.38 ns** |     **457.324 ns** |    **25.067 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 390,768.08 ns | 115,036.836 ns | 6,305.557 ns | 34.89 |    0.49 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,112.92 ns |   1,626.403 ns |    89.149 ns |  3.31 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 710,703.46 ns |  37,139.909 ns | 2,035.764 ns | 63.45 |    0.20 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **F1Macro**           | **1000**   | **595.1 μs** | **45.75 μs** | **2.51 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 602.4 μs | 66.58 μs | 3.65 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 593.9 μs |  7.27 μs | 0.40 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **773.5 μs** | **23.26 μs** | **1.27 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 767.3 μs | 44.77 μs | 2.45 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 907.9 μs | 49.69 μs | 2.72 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                          | Shape   | Mean          | Error          | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.94 μs** |       **1.390 μs** |     **0.076 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 100,088.02 μs |  29,867.530 μs | 1,637.140 μs | 747.24 |   10.59 | 833.3333 | 833.3333 | 833.3333 | 31414.76 KB |   12,806.02 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,914.65 μs** |     **354.972 μs** |    **19.457 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 485,828.50 μs | 147,606.039 μs | 8,090.785 μs | 166.69 |    2.59 |        - |        - |        - | 32054.48 KB |      760.62 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.61 μs** |       **2.375 μs** |     **0.130 μs** |   **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     873.11 μs |     121.577 μs |     6.664 μs |  38.62 |    0.32 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method         | Samples | Classes | Mean           | Error         | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|--------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,452.0 ns** |   **1,527.29 ns** |     **83.72 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,168.2 ns |     643.81 ns |     35.29 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       109.6 ns |       1.65 ns |      0.09 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,998.1 ns |     272.69 ns |     14.95 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,198.7 ns |     111.90 ns |      6.13 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,633.8 ns** |      **46.83 ns** |      **2.57 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,414.9 ns |     616.25 ns |     33.78 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       109.5 ns |      32.50 ns |      1.78 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,439.6 ns |     848.78 ns |     46.52 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,381.9 ns |   2,572.82 ns |    141.03 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **462,779.0 ns** | **121,502.74 ns** |  **6,659.98 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   824,199.8 ns |  26,867.32 ns |  1,472.69 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,226.1 ns |     780.26 ns |     42.77 ns |      - |         - |
| F1Macro        | 100000  | 2       |   474,280.9 ns |  38,692.82 ns |  2,120.88 ns |      - |     656 B |
| Report         | 100000  | 2       |   480,937.9 ns |  20,055.22 ns |  1,099.29 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **495,237.8 ns** |  **28,834.21 ns** |  **1,580.50 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   952,726.6 ns |  78,729.52 ns |  4,315.43 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,113.7 ns |   3,340.67 ns |    183.11 ns |      - |         - |
| F1Macro        | 100000  | 10      |   509,173.9 ns |  18,252.24 ns |  1,000.47 ns |      - |    1881 B |
| Report         | 100000  | 10      |   515,861.9 ns | 181,104.76 ns |  9,926.96 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,605,443.4 ns** |  **43,085.30 ns** |  **2,361.65 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,484,223.2 ns | 970,288.74 ns | 53,184.80 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,698.5 ns |   3,912.91 ns |    214.48 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,605,882.5 ns | 158,133.07 ns |  8,667.81 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,779,270.4 ns | 203,503.43 ns | 11,154.71 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,928,876.5 ns** | **162,037.88 ns** |  **8,881.84 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,500,225.2 ns | 312,113.11 ns | 17,107.97 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,107.2 ns |   6,128.45 ns |    335.92 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 4,920,337.4 ns | 340,462.05 ns | 18,661.87 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 4,956,473.5 ns | 144,310.74 ns |  7,910.16 ns |      - |   12878 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar** | **100000**  | **Bundle**        |  **10,583.92 μs** |  **1,150.650 μs** |    **63.071 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,988.73 μs | 10,227.200 μs |   560.587 μs |     3.31 |    0.05 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |    3,927.18 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **12.30 μs** |      **1.259 μs** |     **0.069 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,480.21 μs |  3,112.502 μs |   170.607 μs | 2,802.89 |   18.17 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **133,025.31 μs** | **66,714.579 μs** | **3,656.851 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 223,206.24 μs |  9,093.863 μs |   498.465 μs |     1.68 |    0.04 |        - |        - |        - | 23231768 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **108.27 μs** |      **5.867 μs** |     **0.322 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 221,409.63 μs | 12,575.386 μs |   689.299 μs | 2,044.95 |    7.62 |        - |        - |        - | 23231816 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean        | Error      | StdDev    | Allocated |
|---------- |-------- |------------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **38.24 ms** |   **4.834 ms** |  **0.265 ms** |         **-** |
| OneVsOne  | 100000  |    84.51 ms |   4.855 ms |  0.266 ms |  801915 B |
| **OneVsRest** | **1000000** |   **517.91 ms** | **482.609 ms** | **26.453 ms** |         **-** |
| OneVsOne  | 1000000 | 1,138.77 ms | 232.889 ms | 12.765 ms | 8003648 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|------------:|
| PerLabel         | 100000 |  6.848 ms |  0.1289 ms | 0.0071 ms |         - |         - |        - |     6.28 KB |
| PerLabelWeighted | 100000 | 11.381 ms |  0.3830 ms | 0.0210 ms |         - |         - |        - |     6.29 KB |
| PerSample        | 100000 | 57.732 ms | 28.7069 ms | 1.5735 ms | 2555.5556 | 2444.4444 | 777.7778 | 31250.62 KB |
| PerClass         | 100000 |  4.508 ms |  0.2631 ms | 0.0144 ms |         - |         - |        - |     6.43 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Dp**         | **4**    |     **73.51 ns** |     **1.428 ns** |   **0.078 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.70 ns |     2.042 ns |   0.112 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     74.13 ns |     2.518 ns |   0.138 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     73.49 ns |     2.727 ns |   0.150 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **109.48 ns** |    **86.073 ns** |   **4.718 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 6    |     89.53 ns |     0.561 ns |   0.031 ns |  0.82 |    0.03 |         - |          NA |
| Dp_Cjk     | 6    |    105.03 ns |    17.056 ns |   0.935 ns |  0.96 |    0.04 |         - |          NA |
| Kernel_Cjk | 6    |    130.99 ns |    13.055 ns |   0.716 ns |  1.20 |    0.05 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **159.93 ns** |     **0.400 ns** |   **0.022 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.42 ns |     3.447 ns |   0.189 ns |  0.64 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.07 ns |    10.858 ns |   0.595 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    182.74 ns |    14.610 ns |   0.801 ns |  1.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **196.16 ns** |     **5.290 ns** |   **0.290 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    109.05 ns |     0.365 ns |   0.020 ns |  0.56 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    197.87 ns |    29.637 ns |   1.624 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    150.26 ns |     5.399 ns |   0.296 ns |  0.77 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **254.56 ns** |    **57.066 ns** |   **3.128 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 12   |    119.51 ns |     7.928 ns |   0.435 ns |  0.47 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    247.14 ns |    59.777 ns |   3.277 ns |  0.97 |    0.02 |         - |          NA |
| Kernel_Cjk | 12   |    168.66 ns |    31.531 ns |   1.728 ns |  0.66 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **411.77 ns** |    **30.372 ns** |   **1.665 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    131.67 ns |    15.214 ns |   0.834 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    414.97 ns |   114.199 ns |   6.260 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    179.00 ns |     1.635 ns |   0.090 ns |  0.43 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **842.77 ns** |     **9.818 ns** |   **0.538 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    171.86 ns |     0.519 ns |   0.028 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    847.79 ns |   134.181 ns |   7.355 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    224.75 ns |     3.709 ns |   0.203 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,447.83 ns** |    **60.816 ns** |   **3.334 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    205.17 ns |    25.889 ns |   1.419 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,458.86 ns |   410.125 ns |  22.480 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    271.79 ns |     4.516 ns |   0.248 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,352.07 ns** | **1,231.593 ns** |  **67.508 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    275.13 ns |     4.534 ns |   0.249 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,323.71 ns |   664.344 ns |  36.415 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    354.40 ns |     8.265 ns |   0.453 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,272.60 ns** |   **167.387 ns** |   **9.175 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    346.20 ns |    32.633 ns |   1.789 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  6,371.64 ns | 3,626.335 ns | 198.772 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    449.75 ns |     0.495 ns |   0.027 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,335.94 ns** | **6,297.380 ns** | **345.181 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    750.56 ns |   370.224 ns |  20.293 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,001.45 ns |   605.566 ns |  33.193 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |  1,557.95 ns |   171.711 ns |   9.412 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **38.47 ns** |      **2.329 ns** |   **0.128 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    124.96 ns |      1.317 ns |   0.072 ns |  3.25 |    0.01 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **144.93 ns** |      **4.145 ns** |   **0.227 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,590.72 ns |    756.534 ns |  41.468 ns | 10.98 |    0.25 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **322.80 ns** |      **3.029 ns** |   **0.166 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,021.89 ns |  3,122.324 ns | 171.145 ns | 27.95 |    0.46 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **49,837.13 ns** |  **9,399.944 ns** | **515.243 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 48,256.79 ns | 17,074.956 ns | 935.936 ns |  0.97 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **WholeFit**              | **10000**    | **10**         |   **200.66 μs** |  **72.898 μs** |  **3.996 μs** |  **1.00** |    **0.02** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   226.48 μs |  15.922 μs |  0.873 μs |  1.13 |    0.02 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    61.32 μs |   5.011 μs |  0.275 μs |  0.31 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.87 μs |   9.715 μs |  0.533 μs |  0.99 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **205.45 μs** |  **20.490 μs** |  **1.123 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   266.73 μs |   3.476 μs |  0.191 μs |  1.30 |    0.01 | 4.3945 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    61.32 μs |   5.941 μs |  0.326 μs |  0.30 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   204.09 μs |  64.814 μs |  3.553 μs |  0.99 |    0.02 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,041.62 μs** |  **53.964 μs** |  **2.958 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,046.63 μs | 212.055 μs | 11.623 μs |  1.00 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,079.63 μs |  87.616 μs |  4.803 μs |  0.53 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,967.19 μs |  18.753 μs |  1.028 μs |  0.96 |    0.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,982.47 μs** | **185.462 μs** | **10.166 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,099.33 μs | 188.550 μs | 10.335 μs |  1.06 |    0.01 | 3.9063 |   77627 B |       97.03 |
| SparseFit             | 100000   | 100        | 1,080.12 μs |  69.760 μs |  3.824 μs |  0.54 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,970.64 μs |  49.193 μs |  2.696 μs |  0.99 |    0.00 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method      | NeedleLength | Mean         | Error     | StdDev    | Allocated |
|------------ |------------- |-------------:|----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.760 μs** | **0.3552 μs** | **0.0195 μs** |         **-** |
| EqualLength | 65           |     2.962 μs | 0.1777 μs | 0.0097 μs |         - |
| **Embedded**    | **128**          |    **38.063 μs** | **0.2936 μs** | **0.0161 μs** |         **-** |
| EqualLength | 128          |     5.294 μs | 0.0476 μs | 0.0026 μs |         - |
| **Embedded**    | **512**          | **1,317.416 μs** | **5.1653 μs** | **0.2831 μs** |         **-** |
| EqualLength | 512          |    44.870 μs | 7.9229 μs | 0.4343 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 10.36 ms | 1.496 ms | 0.082 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 15.19 ms | 0.929 ms | 0.051 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| VocabTxt               |  4.003 ms |  2.5647 ms | 0.1406 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.828 ms |  4.9145 ms | 0.2694 ms | 187.5000 | 171.8750 |  46.8750 |  5852.41 KB |
| TokenizerJsonUnigram   | 12.494 ms |  0.4583 ms | 0.0251 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.044 ms |  1.3579 ms | 0.0744 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.537 ms |  0.5750 ms | 0.0315 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.299 ms |  1.3374 ms | 0.0733 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.602 ms |  2.2803 ms | 0.1250 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.512 ms |  1.7816 ms | 0.0977 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.532 ms | 11.9264 ms | 0.6537 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 71.470 ms |  4.6345 ms | 0.2540 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 13.52 ms | 1.805 ms | 0.099 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 61.68 ms | 4.889 ms | 0.268 ms | 444.4444 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar_ExplainedVariance** | **100x200** |  **9,209.84 μs** |   **642.578 μs** |  **35.222 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,262.75 μs | 2,214.691 μs | 121.395 μs |  1.66 |    0.01 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **132.44 μs** |     **7.456 μs** |   **0.409 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    180.60 μs |    11.876 μs |   0.651 μs |  1.36 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,808.23 μs** |   **136.256 μs** |   **7.469 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **42.19 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,673.95 μs |    79.187 μs |   4.340 μs |  0.95 |    0.00 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.20 μs** |     **8.419 μs** |   **0.461 μs** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.66 μs |     2.801 μs |   0.154 μs |  1.11 |    0.02 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **6.901 ms** | **0.3639 ms** | **0.0199 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.797 ms | 0.1999 ms | 0.0110 ms |  0.99 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.883 ms** | **0.3079 ms** | **0.0169 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.743 ms | 0.2321 ms | 0.0127 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method            | Q | Element   | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.767 μs** | **0.8832 μs** | **0.0484 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **3.872 μs** | **0.4939 μs** | **0.0271 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.743 μs** | **0.4445 μs** | **0.0244 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.812 μs** | **0.4026 μs** | **0.0221 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 37.273 ms | 1.8178 ms | 0.0996 ms |       - |       - |       - |  800349 B |
| NdcgIgnoringTies                  | 100000 | 34.279 ms | 0.3214 ms | 0.0176 ms |       - |       - |       - |  800345 B |
| DcgTieAveraged                    | 100000 | 21.815 ms | 0.5919 ms | 0.0324 ms |       - |       - |       - |  800215 B |
| ReciprocalRankScore               | 100000 | 17.982 ms | 0.0847 ms | 0.0046 ms |       - |       - |       - |         - |
| CoverageErrorScore                | 100000 |  5.869 ms | 0.6993 ms | 0.0383 ms | 15.6250 | 15.6250 | 15.6250 |  800271 B |
| LabelRankingAveragePrecisionScore | 100000 | 53.006 ms | 0.3355 ms | 0.0184 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                   | Length | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **369.4 ns** |    **23.25 ns** |   **1.27 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,294.9 ns |   139.06 ns |   7.62 ns | 14.34 |    0.05 |         - |          NA |
|                          |        |              |             |           |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,803.5 ns** | **2,501.95 ns** | **137.14 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 596,407.3 ns | 9,986.04 ns | 547.37 ns | 26.15 |    0.14 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.69 μs** |     **4.800 μs** |  **0.263 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    44.40 μs |     0.449 μs |  0.025 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   109.90 μs |     2.271 μs |  0.124 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   520.25 μs |    96.340 μs |  5.281 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **454.94 μs** |    **19.855 μs** |  **1.088 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   447.76 μs |    37.548 μs |  2.058 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,101.90 μs |    62.294 μs |  3.415 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,101.73 μs | 1,320.799 μs | 72.397 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **23.93 ms** | **1.245 ms** | **0.068 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **44.23 ms** | **0.706 ms** | **0.039 ms** |       **-** |       **-** |       **-** | **414.33 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]     : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  Job-SNCAST : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Median        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-SNCAST** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **165.539 μs** |    **31.3795 μs** |    **36.1367 μs** |    **176.814 μs** |  **1.05** |    **0.35** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |    107.458 μs |     9.0800 μs |    10.0924 μs |    104.385 μs |  0.68 |    0.18 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,425.354 μs |    26.9679 μs |    31.0563 μs |  2,420.698 μs | 15.45 |    3.84 |        - |        - |        - |  161064 B |        1.97 |
| Lodestar_Standard                 | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |    116.293 μs |     6.2096 μs |     6.6442 μs |    115.354 μs |  0.74 |    0.19 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |     66.339 μs |    12.3079 μs |    13.1693 μs |     59.906 μs |  0.42 |    0.13 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |     59.469 μs |     1.5716 μs |     1.6139 μs |     59.285 μs |  0.38 |    0.09 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |    100.371 μs |     2.8411 μs |     2.7903 μs |     99.585 μs |  0.64 |    0.16 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |    426.007 μs |    16.1503 μs |    18.5987 μs |    420.288 μs |  2.71 |    0.68 |        - |        - |        - |    8856 B |        0.11 |
| MlNet_NormalizeMinMax_Read        | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,270.400 μs |    82.9055 μs |    92.1492 μs |  2,274.702 μs | 14.46 |    3.64 |        - |        - |        - |  333568 B |        4.09 |
| MlNet_NormalizeRobustScaling_Read | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,436.239 μs |   127.1567 μs |   136.0562 μs |  6,400.529 μs | 41.00 |   10.21 |        - |        - |        - |  460504 B |        5.64 |
|                                   |            |                 |                |             |              |             |          |               |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.477 μs |     2.2561 μs |     0.1237 μs |     46.548 μs |  1.00 |    0.00 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     42.274 μs |     3.0288 μs |     0.1660 μs |     42.360 μs |  0.91 |    0.00 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    464.419 μs |    16.2403 μs |     0.8902 μs |    463.990 μs |  9.99 |    0.03 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     49.238 μs |     1.8348 μs |     0.1006 μs |     49.252 μs |  1.06 |    0.00 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.513 μs |     0.6415 μs |     0.0352 μs |     10.498 μs |  0.23 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.691 μs |     1.0310 μs |     0.0565 μs |      7.666 μs |  0.17 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     12.339 μs |     0.8434 μs |     0.0462 μs |     12.324 μs |  0.27 |    0.00 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,601.288 μs | 4,727.5607 μs |   259.1336 μs |  1,604.226 μs | 34.45 |    4.83 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,065.753 μs | 2,271.9326 μs |   124.5323 μs |  1,039.510 μs | 22.93 |    2.32 |  39.0625 |   1.9531 |        - |  649729 B |       8.032 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,654.623 μs | 2,155.1631 μs |   118.1318 μs |  1,614.569 μs | 35.60 |    2.20 |  35.1563 |        - |        - |  624674 B |       7.722 |
|                                   |            |                 |                |             |              |             |          |               |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-SNCAST** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,542.220 μs** |    **14.4780 μs** |    **16.0923 μs** |  **1,546.908 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,424.138 μs |    26.2008 μs |    30.1729 μs |  1,423.698 μs |  0.92 |    0.02 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,365.478 μs |    90.0779 μs |    96.3824 μs | 15,324.402 μs |  9.96 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,504.269 μs |     8.2704 μs |     8.8492 μs |  1,505.856 μs |  0.98 |    0.01 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |    427.394 μs |    10.6813 μs |    12.3006 μs |    427.992 μs |  0.28 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |    323.342 μs |    10.4025 μs |    11.9795 μs |    327.870 μs |  0.21 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |    416.861 μs |    12.4055 μs |    13.7887 μs |    411.285 μs |  0.27 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,706.238 μs |    26.9533 μs |    29.9585 μs |  6,704.929 μs |  4.35 |    0.05 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    | 18,852.629 μs | 8,594.5389 μs | 9,897.4838 μs | 13,986.528 μs | 12.23 |    6.27 |        - |        - |        - |  521760 B |       0.326 |
| MlNet_NormalizeRobustScaling_Read | Job-SNCAST | 1               | 20             | Throughput  | 1            | 5           | 20000    | 25,515.742 μs | 5,880.5219 μs | 6,772.0178 μs | 20,551.436 μs | 16.55 |    4.29 |        - |        - |        - | 4283384 B |       2.674 |
|                                   |            |                 |                |             |              |             |          |               |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,380.239 μs | 1,495.4752 μs |    81.9720 μs |  1,416.306 μs |  1.00 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1601935 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,197.124 μs |   538.3511 μs |    29.5088 μs |  1,190.525 μs |  0.87 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 15,000.080 μs |   284.2439 μs |    15.5804 μs | 15,000.432 μs | 10.89 |    0.58 | 875.0000 | 875.0000 | 875.0000 | 3201322 B |       1.998 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,232.217 μs |   212.0387 μs |    11.6226 μs |  1,236.467 μs |  0.89 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1601039 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    306.826 μs |    33.5918 μs |     1.8413 μs |    306.703 μs |  0.22 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    243.696 μs |    11.5097 μs |     0.6309 μs |    243.458 μs |  0.18 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    402.766 μs |    35.5020 μs |     1.9460 μs |    401.799 μs |  0.29 |    0.02 | 128.9063 | 124.0234 | 124.0234 |  560085 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,267.290 μs | 1,018.4839 μs |    55.8265 μs |  1,272.619 μs |  0.92 |    0.06 |  19.5313 |   3.9063 |        - |  354152 B |       0.221 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,496.883 μs | 1,596.4266 μs |    87.5055 μs |  4,524.183 μs |  3.27 |    0.18 |  31.2500 |   7.8125 |        - |  598774 B |       0.374 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 20,027.087 μs | 5,460.1782 μs |   299.2908 μs | 19,926.119 μs | 14.55 |    0.80 | 250.0000 | 125.0000 |        - | 4303779 B |       2.687 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method          | Model   | Mean     | Error    | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **145.4 ms** | **47.91 ms** | **2.63 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **134.8 ms** | **15.82 ms** | **0.87 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method    | Samples | Mean      | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.29 ms** |  **0.488 ms** | **0.027 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **193.54 ms** | **10.540 ms** | **0.578 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-10-18, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1     | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|---------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **58.640 ms** |  **5.5875 ms** | **0.3063 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |  11.877 ms |  1.7621 ms | 0.0966 ms |  0.20 |   78.1250 |  78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |  10.384 ms |  0.9973 ms | 0.0547 ms |  0.18 |         - |        - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   8.357 ms |  0.4376 ms | 0.0240 ms |  0.14 |         - |        - |        - |    305.29 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  12.547 ms |  0.4289 ms | 0.0235 ms |  0.21 |         - |        - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **60.908 ms** | **13.0670 ms** | **0.7162 ms** |  **1.00** |         **-** |        **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  14.982 ms |  0.1425 ms | 0.0078 ms |  0.25 |  218.7500 | 218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  12.247 ms |  0.4077 ms | 0.0223 ms |  0.20 |         - |        - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   8.506 ms |  1.7446 ms | 0.0956 ms |  0.14 |         - |        - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  12.286 ms |  1.2357 ms | 0.0677 ms |  0.20 |         - |        - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **998.748 ms** | **12.3853 ms** | **0.6789 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.81 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  49.101 ms | 10.1488 ms | 0.5563 ms |  0.05 |  545.4545 | 545.4545 | 454.5455 |  10335.34 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  40.332 ms |  1.1123 ms | 0.0610 ms |  0.04 |         - |        - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  33.128 ms |  3.9506 ms | 0.2165 ms |  0.03 |         - |        - |        - |   1219.38 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  49.261 ms | 11.7754 ms | 0.6454 ms |  0.05 |         - |        - |        - |   1718.82 KB |       0.014 |
|                      |           |              |            |            |           |       |           |          |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **999.208 ms** | **61.0968 ms** | **3.3489 ms** |  **1.00** | **1000.0000** |        **-** |        **-** | **126359.44 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  62.493 ms |  4.5117 ms | 0.2473 ms |  0.06 |  875.0000 | 875.0000 | 750.0000 |  18664.76 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  48.750 ms |  7.3380 ms | 0.4022 ms |  0.05 |         - |        - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  34.305 ms |  5.1819 ms | 0.2840 ms |  0.03 |         - |        - |        - |   1719.88 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  49.168 ms |  0.9818 ms | 0.0538 ms |  0.05 |         - |        - |        - |   1718.82 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method                 | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,215.6 ns |       210.16 ns |      11.52 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,439.3 ns |        81.38 ns |       4.46 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,234.5 ns |       116.74 ns |       6.40 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        436.8 ns |        20.51 ns |       1.12 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,976,747.6 ns | 3,868,283.65 ns | 212,033.68 ns | 875.0000 | 843.7500 | 609.3750 | 12227072 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **80.726 μs** |    **20.845 μs** |   **1.1426 μs** |  **1.00** |    **0.02** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    187.434 μs |     9.637 μs |   0.5283 μs |  2.32 |    0.03 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      5.267 μs |     1.982 μs |   0.1086 μs |  0.07 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    335.989 μs |     3.408 μs |   0.1868 μs |  4.16 |    0.05 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  6,054.064 μs |   642.000 μs |  35.1902 μs | 75.01 |    1.00 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     63.380 μs |     3.915 μs |   0.2146 μs |  0.79 |    0.01 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,124.031 μs |   124.564 μs |   6.8278 μs | 13.93 |    0.19 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,107.339 μs** |   **596.302 μs** |  **32.6853 μs** |  **1.00** |    **0.04** | **423.8281** | **410.1563** | **400.3906** | **2738.42 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,384.574 μs |   499.733 μs |  27.3920 μs |  2.15 |    0.06 | 609.3750 | 593.7500 | 585.9375 | 3130.38 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     84.921 μs |    45.077 μs |   2.4708 μs |  0.08 |    0.00 |  57.8613 |  57.8613 |  57.8613 |  391.18 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    279.969 μs |    90.540 μs |   4.9628 μs |  0.25 |    0.01 |   1.9531 |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 41,871.875 μs | 4,792.166 μs | 262.6748 μs | 37.84 |    1.00 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     62.241 μs |     8.761 μs |   0.4802 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,647.245 μs |   382.093 μs |  20.9438 μs |  6.91 |    0.18 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Count**                | **200**       | **1.912 ms** | **0.0958 ms** | **0.0053 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.397 ms | 0.0748 ms | 0.0041 ms |  0.73 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.977 ms | 0.1124 ms | 0.0062 ms |  1.03 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.709 ms | 0.2250 ms | 0.0123 ms |  0.89 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **9.030 ms** | **0.8797 ms** | **0.0482 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.52 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.059 ms | 0.3356 ms | 0.0184 ms |  0.78 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.582 ms | 0.7961 ms | 0.0436 ms |  1.06 | 515.6250 | 500.0000 | 500.0000 | 2349.22 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.717 ms | 0.2030 ms | 0.0111 ms |  0.97 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method  | Words | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **647.1 μs** |  **51.25 μs** |  **2.81 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,817.2 μs** | **222.95 μs** | **12.22 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **12,230.1 μs** | **993.40 μs** | **54.45 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.73 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

_As of 2026-10-18, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean          | Error         | StdDev      | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|------------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **36,316.102 μs** | **3,160.2023 μs** | **173.2214 μs** |       **-** |       **-** |       **-** | **1554.62 KB** |
| Construct                  | [^ ]+         |      2.111 μs |     0.1674 μs |   0.0092 μs |  0.0648 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.356 μs |     0.4405 μs |   0.0241 μs |  0.0877 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **29,640.577 μs** |   **220.0778 μs** |  **12.0632 μs** |       **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1.993 μs |     0.0605 μs |   0.0033 μs |  0.0648 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.882 μs |     0.3495 μs |   0.0192 μs |  0.0877 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **40,475.280 μs** |   **840.9211 μs** |  **46.0937 μs** |       **-** |       **-** |       **-** |    **1556 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.423 μs |     0.1630 μs |   0.0089 μs |  0.0801 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.690 μs |     0.5264 μs |   0.0289 μs |  0.1068 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **22,468.929 μs** | **1,055.4676 μs** |  **57.8537 μs** | **62.5000** | **62.5000** | **62.5000** |  **9743.6 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.060 μs |     0.0492 μs |   0.0027 μs |  0.0210 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1.859 μs |     0.2243 μs |   0.0123 μs |  0.0458 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **37,377.245 μs** | **4,644.8275 μs** | **254.5987 μs** |       **-** |       **-** |       **-** | **1554.49 KB** |
| Construct                  | \\S+          |      2.333 μs |     0.1630 μs |   0.0089 μs |  0.0610 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.606 μs |     0.3429 μs |   0.0188 μs |  0.0877 |       - |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
| **Lodestar**     | **WordPiece**     |  **20.64 ms** |   **1.464 ms** | **0.080 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  52.87 ms |   4.322 ms | 0.237 ms |  2.56 |    0.01 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.71 ms** |   **6.032 ms** | **0.331 ms** |  **1.00** |    **0.01** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  55.58 ms |   0.450 ms | 0.025 ms |  1.16 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **84.63 ms** |   **4.229 ms** | **0.232 ms** |  **1.00** |    **0.00** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 253.36 ms | 167.381 ms | 9.175 ms |  2.99 |    0.09 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

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

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.66 ms** | **0.153 ms** | **0.008 ms** |         **-** |
| **TopTwo** | **100**     | **86.61 ms** | **1.541 ms** | **0.084 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]     : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  Job-GJFWIQ : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-GJFWIQ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **58.70 μs** |      **1.945 μs** |      **1.997 μs** |     **59.36 μs** |   **1.00** |    **0.05** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |     77.11 μs |      2.267 μs |      2.226 μs |     76.78 μs |   1.32 |    0.06 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    260.59 μs |     14.170 μs |     16.318 μs |    257.65 μs |   4.44 |    0.31 |        - |        - |        - |  156.71 KB |        4.90 |
| Lodestar_Discretize_64Bins  | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    283.49 μs |     22.324 μs |     24.813 μs |    282.34 μs |   4.84 |    0.45 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,413.73 μs |    133.546 μs |    153.791 μs |  1,470.30 μs |  24.11 |    2.70 |        - |        - |        - |    33.7 KB |        1.05 |
| Lodestar_Quantile           | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,616.25 μs |     16.701 μs |     16.402 μs |  1,618.68 μs |  27.57 |    1.00 |        - |        - |        - |   31.71 KB |        0.99 |
| Lodestar_Quantile_Fit       | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,088.00 μs |     19.912 μs |     21.306 μs |  1,078.27 μs |  18.56 |    0.74 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    218.21 μs |     15.252 μs |     17.564 μs |    213.14 μs |   3.72 |    0.32 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,896.58 μs |     20.302 μs |     22.565 μs |  1,890.38 μs |  32.35 |    1.19 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     | 13,732.21 μs |  5,003.578 μs |  5,762.128 μs |  9,217.93 μs | 234.23 |   96.34 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,277.79 μs |     71.397 μs |     73.319 μs |  1,300.74 μs |  21.79 |    1.43 |        - |        - |        - |  316.17 KB |        9.88 |
| MlNet_NormalizeBinning_Read | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,237.04 μs |    144.137 μs |    160.208 μs |  3,234.63 μs |  55.21 |    3.29 |        - |        - |        - |  400.01 KB |       12.50 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     14.36 μs |      1.416 μs |      0.078 μs |     14.39 μs |   1.00 |    0.01 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     78.30 μs |      6.818 μs |      0.374 μs |     78.42 μs |   5.45 |    0.03 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    133.67 μs |     11.002 μs |      0.603 μs |    133.39 μs |   9.31 |    0.06 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     74.61 μs |      3.636 μs |      0.199 μs |     74.54 μs |   5.20 |    0.03 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     90.68 μs |    261.207 μs |     14.318 μs |     82.78 μs |   6.32 |    0.86 |   2.0142 |   0.0610 |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    499.01 μs |     74.532 μs |      4.085 μs |    497.92 μs |  34.76 |    0.30 |   0.9766 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    205.71 μs |     27.678 μs |      1.517 μs |    206.51 μs |  14.33 |    0.11 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    140.11 μs |      4.306 μs |      0.236 μs |    140.04 μs |   9.76 |    0.05 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    708.28 μs |     22.728 μs |      1.246 μs |    707.89 μs |  49.34 |    0.24 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,812.16 μs |  1,214.228 μs |     66.556 μs |  8,814.27 μs | 613.81 |    4.94 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    285.14 μs |      6.553 μs |      0.359 μs |    285.19 μs |  19.86 |    0.10 |  18.5547 |  18.0664 |        - |  295.06 KB |        9.43 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,026.56 μs |  1,116.649 μs |     61.207 μs |  1,016.42 μs |  71.50 |    3.71 |  42.9688 |   5.8594 |        - |  698.97 KB |       22.35 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-GJFWIQ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **493.68 μs** |     **16.072 μs** |     **17.196 μs** |    **490.75 μs** |   **1.00** |    **0.05** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,653.90 μs |     34.189 μs |     39.372 μs |  1,661.18 μs |   3.35 |    0.14 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,585.67 μs |     32.113 μs |     36.981 μs |  3,581.27 μs |   7.27 |    0.25 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,564.49 μs |     36.819 μs |     42.401 μs |  4,566.52 μs |   9.26 |    0.32 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,545.41 μs |  6,169.770 μs |  7,105.116 μs | 10,155.07 μs |  25.44 |   14.10 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,023.91 μs |    748.233 μs |    734.865 μs | 10,765.01 μs |  22.36 |    1.63 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,738.54 μs |  5,529.805 μs |  6,368.131 μs |  7,066.05 μs |  23.80 |   12.64 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,080.51 μs |     32.953 μs |     36.627 μs |  3,073.22 μs |   6.25 |    0.22 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Power_Fit          | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,192.18 μs |    417.016 μs |    463.513 μs | 15,524.41 μs |  30.81 |    1.38 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,735.68 μs |     34.404 μs |     36.812 μs |  9,734.32 μs |  19.74 |    0.66 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,409.88 μs |  3,847.592 μs |  4,116.880 μs | 10,700.85 μs |  19.08 |    8.16 |        - |        - |        - |  454.19 KB |        0.73 |
| MlNet_NormalizeBinning_Read | Job-GJFWIQ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 25,627.82 μs | 11,539.248 μs | 13,288.615 μs | 22,001.09 μs |  51.97 |   26.37 |        - |        - |        - | 2079.22 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    670.35 μs |     89.520 μs |      4.907 μs |    667.81 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    704.45 μs |    835.420 μs |     45.792 μs |    719.21 μs |   1.05 |    0.06 | 300.7813 | 300.7813 | 300.7813 | 2346.37 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,094.24 μs |    175.787 μs |      9.635 μs |  3,096.52 μs |   4.62 |    0.03 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,887.34 μs |     30.302 μs |      1.661 μs |  1,886.63 μs |   2.82 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,795.68 μs |    102.262 μs |      5.605 μs |  5,794.43 μs |   8.65 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,424.83 μs |    646.289 μs |     35.425 μs | 10,407.74 μs |  15.55 |    0.11 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,948.07 μs |     63.609 μs |      3.487 μs |  5,947.62 μs |   8.87 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,232.60 μs |    293.009 μs |     16.061 μs |  3,224.68 μs |   4.82 |    0.04 | 199.2188 | 199.2188 | 199.2188 |  625.15 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,954.80 μs |  1,134.125 μs |     62.165 μs | 13,942.28 μs |  20.82 |    0.15 | 750.0000 | 750.0000 | 750.0000 | 2662.15 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,165.86 μs |    497.474 μs |     27.268 μs |  9,155.14 μs |  13.67 |    0.09 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,398.85 μs |  1,041.817 μs |     57.106 μs |  2,387.71 μs |   3.58 |    0.08 |  27.3438 |  11.7188 |        - |  461.63 KB |        0.74 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,744.63 μs |    967.080 μs |     53.009 μs | 10,768.75 μs |  16.03 |    0.12 | 265.6250 | 218.7500 | 187.5000 | 2134.85 KB |        3.41 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **49.61 ns** | **3.401 ns** | **0.186 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  49.66 ns | 9.316 ns | 0.511 ns |  1.00 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **95.62 ns** | **8.994 ns** | **0.493 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  93.60 ns | 1.403 ns | 0.077 ns |  0.98 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **124.85 ns** | **9.016 ns** | **0.494 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 123.41 ns | 1.940 ns | 0.106 ns |  0.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]     : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  Job-DPQCRY : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev    | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-DPQCRY** | **20**             | **Throughput**  | **5**           | **1000**    |     **59.38 μs** |     **0.097 μs** |  **0.112 μs** | **0.1831** |   **3.15 KB** |
| UpsertThenHybrid | Job-DPQCRY | 20             | Throughput  | 5           | 1000    |     78.74 μs |     0.129 μs |  0.143 μs | 1.2207 |  20.58 KB |
| Hybrid           | Job-DPQCRY | 20             | Throughput  | 5           | 1000    |     72.95 μs |     0.081 μs |  0.090 μs | 0.8545 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     59.27 μs |     2.107 μs |  0.115 μs | 0.1831 |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     77.80 μs |     6.134 μs |  0.336 μs | 1.2207 |  20.58 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     71.43 μs |     3.128 μs |  0.171 μs | 0.8545 |  15.52 KB |
| **UpsertThenSearch** | **Job-DPQCRY** | **20**             | **Throughput**  | **5**           | **10000**   |    **581.41 μs** |    **16.768 μs** | **19.311 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-DPQCRY | 20             | Throughput  | 5           | 10000   |    950.46 μs |    22.490 μs | 25.900 μs | 0.9766 |  26.16 KB |
| Hybrid           | Job-DPQCRY | 20             | Throughput  | 5           | 10000   |    889.45 μs |    19.792 μs | 22.793 μs | 0.9766 |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    625.69 μs |   267.009 μs | 14.636 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |    954.01 μs |   414.627 μs | 22.727 μs | 0.9766 |  26.16 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    930.57 μs |   293.619 μs | 16.094 μs | 0.9766 |  21.14 KB |
| **UpsertThenSearch** | **Job-DPQCRY** | **20**             | **Throughput**  | **5**           | **100000**  |  **7,945.89 μs** |    **64.380 μs** | **74.140 μs** |      **-** |   **3.19 KB** |
| UpsertThenHybrid | Job-DPQCRY | 20             | Throughput  | 5           | 100000  | 11,898.96 μs |    46.109 μs | 53.099 μs |      - |     46 KB |
| Hybrid           | Job-DPQCRY | 20             | Throughput  | 5           | 100000  | 11,776.21 μs |    52.538 μs | 60.503 μs |      - |  41.02 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  |  7,923.41 μs | 1,553.909 μs | 85.175 μs |      - |   3.19 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 12,104.10 μs | 1,492.648 μs | 81.817 μs |      - |     46 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 11,695.78 μs | 1,065.403 μs | 58.398 μs |      - |  41.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-10-18, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean        | Error       | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **261.5 μs** |    **18.46 μs** |  **1.01 μs** |  **1.00** |    **0.00** |    **2.4414** |    **0.4883** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    288.9 μs |    26.91 μs |  1.48 μs |  1.10 |    0.01 |    2.4414 |         - |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    776.8 μs |    11.16 μs |  0.61 μs |  2.97 |    0.01 |    6.8359 |    1.9531 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,146.0 μs |   145.49 μs |  7.97 μs |  8.21 |    0.04 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    291.7 μs |     3.56 μs |  0.20 μs |  1.12 |    0.00 |    1.4648 |         - |         - |   136.13 KB |        0.66 |
|                       |           |             |             |          |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,648.8 μs** |    **36.33 μs** |  **1.99 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,795.0 μs |    47.30 μs |  2.59 μs |  1.09 |    0.00 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,966.8 μs |   484.69 μs | 26.57 μs |  2.41 |    0.01 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 12,990.8 μs | 1,031.88 μs | 56.56 μs |  7.88 |    0.03 | 1015.6250 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,738.9 μs |    92.87 μs |  5.09 μs |  1.05 |    0.00 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.763 ms** |   **0.1402 ms** |  **0.0077 ms** |  **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  52.099 ms | 135.0579 ms |  7.4030 ms | 13.84 |    1.70 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.119 ms** |   **5.8278 ms** |  **0.3194 ms** |  **1.00** |    **0.02** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 395.893 ms | 477.3007 ms | 26.1625 ms | 23.13 |    1.38 | 79000.0000 | 79000.0000 | 79000.0000 |  324.7 MB |       40.04 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.166 | 0.036 | 0.22x | 0.166 | 0.036 | 0.22x |
| cdist_wratio_n50 | 2.309 | 2.487 | 1.08x | 2.309 | 2.486 | 1.08x |
| cdist_ratio_varied_n50 | 0.133 | 0.031 | 0.24x | 0.133 | 0.031 | 0.24x |
| cdist_ratio_varied_cutoff90_n50 | 0.060 | 0.031 | 0.52x | 0.060 | 0.031 | 0.52x |
| cdist_ratio_n200 | 2.733 | 0.441 | 0.16x | 2.737 | 0.441 | 0.16x |
| cdist_wratio_n200 | 35.306 | 37.433 | 1.06x | 35.306 | 37.430 | 1.06x |
| cdist_ratio_varied_n200 | 2.328 | 0.387 | 0.17x | 2.332 | 0.387 | 0.17x |
| cdist_ratio_varied_cutoff90_n200 | 1.043 | 0.387 | 0.37x | 1.046 | 0.387 | 0.37x |
| cdist_ratio_n500 | 16.600 | 2.679 | 0.16x | 16.624 | 2.679 | 0.16x |
| cdist_wratio_n500 | 213.288 | 226.952 | 1.06x | 213.417 | 226.932 | 1.06x |
| cdist_ratio_varied_n500 | 13.713 | 2.322 | 0.17x | 13.823 | 2.322 | 0.17x |
| cdist_ratio_varied_cutoff90_n500 | 5.897 | 2.323 | 0.39x | 6.000 | 2.323 | 0.39x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.104 | 3.958 | 0.78x | 5.112 | 7.916 | 1.55x |
| kmeans_restarts_10000 | 20.980 | 10.241 | 0.49x | 20.984 | 20.466 | 0.98x |
| kmeans_weighted_100000 | 49.967 | 28.333 | 0.57x | 50.134 | 47.713 | 0.95x |
| kmeans_restarts_100000 | 204.222 | 71.939 | 0.35x | 204.636 | 132.567 | 0.65x |
| dbscan_weighted_5000 | 7.413 | 33.395 | 4.50x | 7.799 | 33.389 | 4.28x |
| dbscan_weighted_20000 | 92.753 | 195.626 | 2.11x | 93.612 | 195.604 | 2.09x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.869 | 127.291 | 146.49x | 0.869 | 127.271 | 146.47x |
| ridge_robust_1000 | 0.949 | 118.254 | 124.62x | 0.949 | 118.234 | 124.62x |
| lasso_1000 | 7.246 | 1108.952 | 153.05x | 7.245 | 1108.807 | 153.05x |
| ph_test_1000 | 0.131 | 21.825 | 166.64x | 0.131 | 21.826 | 166.65x |
| predict_survival_1000 | 0.033 | 1.834 | 55.13x | 0.033 | 1.834 | 55.13x |
| time_varying_1000 | 78.403 | 353.189 | 4.50x | 78.604 | 353.146 | 4.49x |
| stratified_weighted_10000 | 9.061 | 708.147 | 78.16x | 9.097 | 708.043 | 77.83x |
| ridge_robust_10000 | 10.169 | 1107.300 | 108.89x | 10.398 | 1107.240 | 106.49x |
| lasso_10000 | 59.339 | 7109.097 | 119.80x | 59.369 | 7106.051 | 119.69x |
| ph_test_10000 | 1.999 | 212.611 | 106.36x | 2.059 | 212.596 | 103.27x |
| predict_survival_10000 | 0.038 | 1.861 | 49.41x | 0.038 | 1.861 | 49.41x |
| time_varying_10000 | 3032.236 | 5192.440 | 1.71x | 3032.005 | 5191.677 | 1.71x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 4.307 | 49.420 | 11.47x | 4.307 | 196.853 | 45.71x |
| cv_minmax_n1000 | 3.166 | 5.018 | 1.58x | 3.166 | 19.999 | 6.32x |
| cv_plus_gamma_n1000 | 4.827 | 51.509 | 10.67x | 4.827 | 205.234 | 42.52x |
| bootstrap_plus_n1000 | 16.751 | 51.936 | 3.10x | 16.750 | 206.829 | 12.35x |
| cv_plus_n10000 | 42.349 | 131.049 | 3.09x | 42.345 | 476.167 | 11.24x |
| cv_minmax_n10000 | 31.393 | 27.208 | 0.87x | 31.388 | 108.367 | 3.45x |
| cv_plus_gamma_n10000 | 60.793 | 137.804 | 2.27x | 60.787 | 479.434 | 7.89x |
| bootstrap_plus_n10000 | 154.552 | 131.662 | 0.85x | 154.538 | 481.648 | 3.12x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.401 | 3.411 | 8.50x | 0.401 | 3.411 | 8.50x |
| glm_gamma_n1000 | 0.485 | 4.250 | 8.77x | 0.485 | 4.250 | 8.77x |
| glm_poisson_exposure_n1000 | 0.404 | 4.039 | 9.99x | 0.404 | 4.039 | 10.00x |
| mnlogit_n1000 | 0.804 | 18.270 | 22.74x | 0.804 | 18.269 | 22.73x |
| glm_negative_binomial_n10000 | 3.635 | 12.095 | 3.33x | 3.645 | 12.093 | 3.32x |
| glm_gamma_n10000 | 4.181 | 13.178 | 3.15x | 4.190 | 13.177 | 3.14x |
| glm_poisson_exposure_n10000 | 4.389 | 13.190 | 3.00x | 4.400 | 13.187 | 3.00x |
| mnlogit_n10000 | 8.150 | 98.652 | 12.11x | 8.160 | 98.639 | 12.09x |
| glm_negative_binomial_n100000 | 36.575 | 136.535 | 3.73x | 36.817 | 545.147 | 14.81x |
| glm_gamma_n100000 | 42.453 | 156.844 | 3.69x | 42.839 | 626.223 | 14.62x |
| glm_poisson_exposure_n100000 | 54.779 | 134.504 | 2.46x | 55.153 | 536.774 | 9.73x |
| mnlogit_n100000 | 81.115 | 949.404 | 11.70x | 81.163 | 1695.850 | 20.89x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 118.7 | 28.1 | 4.22x C# faster |
| latin | 32 | 184.2 | 86.0 | 2.14x C# faster |
| latin | 128 | 485.3 | 424.6 | 1.14x C# faster |
| latin | 512 | 4484.9 | 5005.3 | 1.12x Py faster |
| cjk | 8 | 143.8 | 26.1 | 5.52x C# faster |
| cjk | 32 | 334.4 | 217.8 | 1.54x C# faster |
| cjk | 128 | 1916.4 | 1373.8 | 1.39x C# faster |
| cjk | 512 | 14937.6 | 8860.4 | 1.69x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.619 | 56.706 | 35.03x | 1.619 | 56.698 | 35.02x |
| liml_robust_n1000 | 1.770 | 57.829 | 32.68x | 1.770 | 57.823 | 32.68x |
| gmm_robust_n1000 | 1.610 | 55.748 | 34.63x | 1.610 | 55.667 | 34.58x |
| 2sls_kernel_n1000 | 8.561 | 60.331 | 7.05x | 8.561 | 60.321 | 7.05x |
| 2sls_clustered_n1000 | 1.573 | 56.426 | 35.88x | 1.572 | 56.415 | 35.88x |
| 2sls_robust_n10000 | 16.442 | 244.566 | 14.87x | 16.804 | 489.034 | 29.10x |
| liml_robust_n10000 | 18.068 | 241.364 | 13.36x | 18.604 | 482.831 | 25.95x |
| gmm_robust_n10000 | 16.480 | 236.333 | 14.34x | 16.780 | 472.546 | 28.16x |
| 2sls_kernel_n10000 | 90.724 | 257.601 | 2.84x | 91.010 | 515.551 | 5.66x |
| 2sls_clustered_n10000 | 15.484 | 265.171 | 17.13x | 15.812 | 530.128 | 33.53x |
| 2sls_robust_n100000 | 167.784 | 2561.161 | 15.26x | 169.640 | 7830.784 | 46.16x |
| liml_robust_n100000 | 181.604 | 2557.535 | 14.08x | 183.725 | 7943.582 | 43.24x |
| gmm_robust_n100000 | 169.745 | 2494.132 | 14.69x | 172.572 | 7603.568 | 44.06x |
| 2sls_kernel_n100000 | 3329.648 | 3251.464 | 0.98x | 3331.798 | 10100.081 | 3.03x |
| 2sls_clustered_n100000 | 159.061 | 2780.375 | 17.48x | 160.367 | 8873.294 | 55.33x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 147.6 | 18.2 | 8.12x C# faster |
| latin | 32 | 280.4 | 157.1 | 1.79x C# faster |
| latin | 128 | 1687.7 | 885.4 | 1.91x C# faster |
| latin | 512 | 14032.2 | 11604.0 | 1.21x C# faster |
| cjk | 8 | 158.9 | 18.2 | 8.72x C# faster |
| cjk | 32 | 386.1 | 287.6 | 1.34x C# faster |
| cjk | 128 | 2848.6 | 2041.9 | 1.40x C# faster |
| cjk | 512 | 23707.8 | 17900.6 | 1.32x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 0.972 | 223.06x | 0.004 | 0.971 | 223.03x |
| accuracy_n1000_k2 | 0.000 | 0.517 | 3892.66x | 0.000 | 0.517 | 3892.20x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.778 | 359.33x | 0.005 | 1.778 | 359.32x |
| classification_report_n1000_k2 | 0.007 | 6.841 | 928.09x | 0.007 | 6.841 | 928.18x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.898 | 105.19x | 0.018 | 1.897 | 105.10x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.034 | 220.94x | 0.005 | 1.034 | 220.94x |
| matthews_n1000_k2 | 0.005 | 1.994 | 433.25x | 0.005 | 1.994 | 433.25x |
| cohen_kappa_n1000_k2 | 0.005 | 1.072 | 231.50x | 0.005 | 1.071 | 231.49x |
| mse_n1000_k2 | 0.000 | 0.302 | 622.50x | 0.000 | 0.301 | 622.47x |
| mae_n1000_k2 | 0.000 | 0.304 | 630.64x | 0.000 | 0.304 | 630.62x |
| median_ae_n1000_k2 | 0.005 | 0.315 | 65.30x | 0.005 | 0.315 | 65.30x |
| r2_n1000_k2 | 0.001 | 0.368 | 311.76x | 0.001 | 0.368 | 311.77x |
| confusion_matrix_n1000_k10 | 0.005 | 0.971 | 181.15x | 0.005 | 0.970 | 181.15x |
| accuracy_n1000_k10 | 0.000 | 0.521 | 3886.73x | 0.000 | 0.521 | 3886.36x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.767 | 302.17x | 0.006 | 1.767 | 302.15x |
| classification_report_n1000_k10 | 0.012 | 6.909 | 592.23x | 0.012 | 6.908 | 592.18x |
| roc_auc_ovr_macro_n1000_k10 | 0.522 | 9.755 | 18.68x | 0.522 | 9.754 | 18.68x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.030 | 179.80x | 0.006 | 1.029 | 179.79x |
| matthews_n1000_k10 | 0.006 | 1.995 | 354.16x | 0.006 | 1.995 | 354.15x |
| cohen_kappa_n1000_k10 | 0.006 | 1.084 | 179.71x | 0.006 | 1.084 | 179.72x |
| mse_n1000_k10 | 0.000 | 0.302 | 624.47x | 0.000 | 0.302 | 624.49x |
| mae_n1000_k10 | 0.000 | 0.301 | 627.16x | 0.000 | 0.301 | 627.14x |
| median_ae_n1000_k10 | 0.005 | 0.313 | 65.09x | 0.005 | 0.313 | 65.09x |
| r2_n1000_k10 | 0.001 | 0.368 | 310.71x | 0.001 | 0.368 | 310.72x |
| confusion_matrix_n100000_k2 | 0.459 | 10.862 | 23.65x | 0.459 | 10.862 | 23.65x |
| accuracy_n100000_k2 | 0.012 | 3.783 | 312.08x | 0.012 | 3.783 | 312.07x |
| precision_recall_f1_macro_n100000_k2 | 0.461 | 12.468 | 27.07x | 0.460 | 12.466 | 27.07x |
| classification_report_n100000_k2 | 0.463 | 27.285 | 58.90x | 0.463 | 27.283 | 58.90x |
| roc_auc_binary_n100000_k2 | 4.533 | 27.362 | 6.04x | 4.532 | 27.361 | 6.04x |
| balanced_accuracy_n100000_k2 | 0.458 | 10.951 | 23.89x | 0.458 | 10.951 | 23.90x |
| matthews_n100000_k2 | 0.459 | 21.948 | 47.84x | 0.459 | 21.946 | 47.84x |
| cohen_kappa_n100000_k2 | 0.460 | 10.983 | 23.88x | 0.460 | 10.982 | 23.88x |
| mse_n100000_k2 | 0.045 | 0.453 | 9.99x | 0.045 | 0.453 | 9.99x |
| mae_n100000_k2 | 0.045 | 0.456 | 10.20x | 0.045 | 0.456 | 10.20x |
| median_ae_n100000_k2 | 0.601 | 1.794 | 2.98x | 0.625 | 1.794 | 2.87x |
| r2_n100000_k2 | 0.110 | 0.675 | 6.15x | 0.110 | 0.675 | 6.15x |
| confusion_matrix_n100000_k10 | 0.491 | 10.899 | 22.20x | 0.491 | 10.895 | 22.19x |
| accuracy_n100000_k10 | 0.012 | 3.835 | 310.96x | 0.012 | 3.834 | 310.94x |
| precision_recall_f1_macro_n100000_k10 | 0.492 | 13.161 | 26.73x | 0.492 | 13.160 | 26.74x |
| classification_report_n100000_k10 | 0.500 | 29.873 | 59.75x | 0.500 | 29.869 | 59.75x |
| roc_auc_ovr_macro_n100000_k10 | 37.076 | 220.561 | 5.95x | 37.074 | 220.552 | 5.95x |
| balanced_accuracy_n100000_k10 | 0.492 | 11.051 | 22.48x | 0.492 | 11.050 | 22.48x |
| matthews_n100000_k10 | 0.492 | 22.730 | 46.24x | 0.492 | 22.729 | 46.24x |
| cohen_kappa_n100000_k10 | 0.493 | 11.004 | 22.33x | 0.493 | 11.004 | 22.33x |
| mse_n100000_k10 | 0.045 | 0.462 | 10.35x | 0.045 | 0.462 | 10.35x |
| mae_n100000_k10 | 0.044 | 0.454 | 10.29x | 0.044 | 0.454 | 10.29x |
| median_ae_n100000_k10 | 0.578 | 1.798 | 3.11x | 0.618 | 1.797 | 2.91x |
| r2_n100000_k10 | 0.109 | 0.693 | 6.34x | 0.109 | 0.693 | 6.34x |
| confusion_matrix_n1000000_k2 | 4.626 | 101.151 | 21.87x | 4.624 | 101.148 | 21.87x |
| accuracy_n1000000_k2 | 0.120 | 33.584 | 279.82x | 0.120 | 33.584 | 279.85x |
| precision_recall_f1_macro_n1000000_k2 | 4.619 | 108.864 | 23.57x | 4.619 | 108.840 | 23.57x |
| classification_report_n1000000_k2 | 4.623 | 212.702 | 46.01x | 4.622 | 212.680 | 46.01x |
| roc_auc_binary_n1000000_k2 | 58.752 | 296.792 | 5.05x | 58.749 | 296.744 | 5.05x |
| balanced_accuracy_n1000000_k2 | 4.620 | 101.537 | 21.98x | 4.619 | 101.527 | 21.98x |
| matthews_n1000000_k2 | 4.618 | 202.683 | 43.89x | 4.618 | 202.665 | 43.89x |
| cohen_kappa_n1000000_k2 | 4.624 | 101.168 | 21.88x | 4.623 | 101.153 | 21.88x |
| mse_n1000000_k2 | 0.457 | 2.414 | 5.28x | 0.457 | 2.413 | 5.28x |
| mae_n1000000_k2 | 0.453 | 2.432 | 5.37x | 0.453 | 2.432 | 5.37x |
| median_ae_n1000000_k2 | 5.724 | 14.562 | 2.54x | 5.825 | 14.561 | 2.50x |
| r2_n1000000_k2 | 1.104 | 4.386 | 3.97x | 1.104 | 4.386 | 3.97x |
| confusion_matrix_n1000000_k10 | 4.855 | 101.022 | 20.81x | 4.855 | 101.012 | 20.81x |
| accuracy_n1000000_k10 | 0.120 | 33.763 | 280.51x | 0.120 | 33.750 | 280.41x |
| precision_recall_f1_macro_n1000000_k10 | 4.858 | 116.176 | 23.91x | 4.858 | 116.164 | 23.91x |
| classification_report_n1000000_k10 | 4.848 | 237.903 | 49.07x | 4.848 | 237.874 | 49.07x |
| balanced_accuracy_n1000000_k10 | 4.867 | 100.878 | 20.73x | 4.867 | 100.873 | 20.72x |
| matthews_n1000000_k10 | 4.847 | 211.597 | 43.65x | 4.847 | 211.576 | 43.65x |
| cohen_kappa_n1000000_k10 | 4.848 | 101.898 | 21.02x | 4.847 | 101.882 | 21.02x |
| mse_n1000000_k10 | 0.463 | 2.479 | 5.36x | 0.463 | 2.479 | 5.36x |
| mae_n1000000_k10 | 0.456 | 2.500 | 5.49x | 0.456 | 2.499 | 5.49x |
| median_ae_n1000000_k10 | 5.751 | 14.678 | 2.55x | 5.855 | 14.676 | 2.51x |
| r2_n1000000_k10 | 1.130 | 4.584 | 4.06x | 1.129 | 4.584 | 4.06x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.045 | 0.860 | 0.42x | 2.045 | 0.859 | 0.42x |
| transform_frobenius_n500 | 6.560 | 1.656 | 0.25x | 6.559 | 1.656 | 0.25x |
| transform_frobenius_n2000 | 23.819 | 5.876 | 0.25x | 23.861 | 23.445 | 0.98x |
| transform_kl_n100 | 1.251 | 17.872 | 14.29x | 1.251 | 17.872 | 14.29x |
| transform_kl_n500 | 5.697 | 34.771 | 6.10x | 5.696 | 34.769 | 6.10x |
| transform_kl_n2000 | 22.631 | 123.961 | 5.48x | 22.698 | 452.622 | 19.94x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.045 | 3.014 | 66.96x | 0.045 | 3.014 | 66.95x |
| ols_hac_n1000 | 0.188 | 3.491 | 18.55x | 0.188 | 3.491 | 18.55x |
| ols_cluster_n1000 | 0.063 | 3.539 | 56.08x | 0.063 | 3.539 | 56.08x |
| ols_summary_n10000 | 0.434 | 11.450 | 26.40x | 0.434 | 11.449 | 26.40x |
| ols_hac_n10000 | 1.932 | 12.986 | 6.72x | 1.944 | 12.984 | 6.68x |
| ols_cluster_n10000 | 0.603 | 13.328 | 22.12x | 0.607 | 13.327 | 21.95x |
| ols_summary_n100000 | 4.326 | 111.997 | 25.89x | 4.344 | 447.281 | 102.96x |
| ols_hac_n100000 | 19.200 | 123.277 | 6.42x | 19.534 | 492.119 | 25.19x |
| ols_cluster_n100000 | 6.644 | 127.855 | 19.24x | 6.859 | 510.634 | 74.45x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.680 | 4.479 | 2.67x | 1.685 | 4.479 | 2.66x |
| sparse_infrequent_10000 | 2.127 | 4.718 | 2.22x | 2.132 | 4.717 | 2.21x |
| sparse_100000 | 16.767 | 28.995 | 1.73x | 17.209 | 28.992 | 1.68x |
| sparse_infrequent_100000 | 19.055 | 24.521 | 1.29x | 19.518 | 24.518 | 1.26x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3', 'pandas': '2.3.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.227 | 20.415 | 89.93x | 0.227 | 20.413 | 89.94x |
| fe_twoway_clustered_n1000 | 0.392 | 32.062 | 81.80x | 0.392 | 32.060 | 81.81x |
| fe_kernel_n1000 | 0.246 | 20.692 | 84.15x | 0.246 | 20.690 | 84.15x |
| between_robust_n1000 | 0.125 | 19.534 | 156.59x | 0.125 | 19.529 | 156.56x |
| first_difference_robust_n1000 | 0.250 | 24.106 | 96.46x | 0.250 | 24.099 | 96.46x |
| random_effects_n1000 | 0.237 | 26.687 | 112.83x | 0.237 | 26.682 | 112.82x |
| fe_entity_n10000 | 2.726 | 29.487 | 10.82x | 2.822 | 29.483 | 10.45x |
| fe_twoway_clustered_n10000 | 4.910 | 55.611 | 11.33x | 5.162 | 111.232 | 21.55x |
| fe_kernel_n10000 | 2.957 | 30.025 | 10.15x | 3.005 | 30.024 | 9.99x |
| between_robust_n10000 | 1.788 | 28.506 | 15.94x | 1.795 | 28.504 | 15.88x |
| first_difference_robust_n10000 | 2.953 | 34.158 | 11.57x | 2.962 | 34.156 | 11.53x |
| random_effects_n10000 | 2.827 | 39.705 | 14.05x | 2.866 | 39.700 | 13.85x |
| fe_entity_n100000 | 31.285 | 179.499 | 5.74x | 32.503 | 716.150 | 22.03x |
| fe_twoway_clustered_n100000 | 48.739 | 348.748 | 7.16x | 50.047 | 1256.178 | 25.10x |
| fe_kernel_n100000 | 32.137 | 164.846 | 5.13x | 33.552 | 656.780 | 19.57x |
| between_robust_n100000 | 18.762 | 161.800 | 8.62x | 19.346 | 529.826 | 27.39x |
| first_difference_robust_n100000 | 33.986 | 192.641 | 5.67x | 35.337 | 766.756 | 21.70x |
| random_effects_n100000 | 31.565 | 218.059 | 6.91x | 32.549 | 869.232 | 26.71x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.421 | 10.383 | 1.92x | 5.765 | 10.382 | 1.80x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.676 | 17.485 | 1.38x | 13.053 | 17.484 | 1.34x | 706,526 | 706,526 |
| tokenizer_json_unigram | 14.260 | 41.393 | 2.90x | 14.591 | 41.389 | 2.84x | 1,990,038 | 1,990,038 |
| spiece_model | 6.705 | 29.495 | 4.40x | 6.975 | 29.493 | 4.23x | 533,084 | 533,084 |
| tfidf_save | 1.548 | 2.411 | 1.56x | 1.580 | 2.411 | 1.53x | 581,787 | 591,922 |
| tfidf_load | 4.776 | 4.201 | 0.88x | 5.016 | 4.200 | 0.84x | 581,787 | 591,922 |
| embedding_index_save | 4.185 | 1.532 | 0.37x | 4.366 | 1.531 | 0.35x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.377 | 37.447 | 0.74x | 10.283 | 5.299 | 0.52x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.520 | 1.527 | 0.28x | 5.839 | 1.527 | 0.26x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.301 | 0.991 | 0.16x | 6.729 | 0.990 | 0.15x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.383 | 1.563 | 0.36x | 4.714 | 1.563 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.417 | 1.558 | 1.10x | 1.725 | 1.558 | 0.90x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 85.34x | 0.000 | 0.001 | 85.35x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 410.374 | 557.093 | 1.36x | 410.325 | 557.023 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 70.403 | 67.228 | 0.95x | 70.441 | 67.226 | 0.95x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.096 | 0.120 | 1.25x | 0.096 | 0.120 | 1.25x |
| stratified_n10000 | 0.187 | 0.863 | 4.61x | 0.187 | 0.863 | 4.61x |
| traintest_n10000 | 0.005 | 0.237 | 49.25x | 0.005 | 0.237 | 49.24x |
| kfold_seeded_n10000 | 0.323 | 0.675 | 2.09x | 0.323 | 0.675 | 2.09x |
| stratified_seeded_n10000 | 0.442 | 1.469 | 3.32x | 0.442 | 1.469 | 3.32x |
| traintest_seeded_n10000 | 0.167 | 0.518 | 3.10x | 0.167 | 0.518 | 3.10x |
| stratified_traintest_n10000 | 0.669 | 2.774 | 4.14x | 0.669 | 2.773 | 4.14x |
| group_kfold_n10000 | 0.127 | 0.600 | 4.73x | 0.127 | 0.600 | 4.73x |
| stratified_group_kfold_n10000 | 0.244 | 31.862 | 130.78x | 0.244 | 31.858 | 130.78x |
| timeseries_n10000 | 0.000 | 0.012 | 91.13x | 0.000 | 0.012 | 91.13x |
| repeated_kfold_n10000 | 0.982 | 1.790 | 1.82x | 0.982 | 1.790 | 1.82x |
| kfold_n100000 | 1.106 | 2.542 | 2.30x | 1.238 | 2.542 | 2.05x |
| stratified_n100000 | 2.220 | 7.175 | 3.23x | 2.348 | 7.174 | 3.06x |
| traintest_n100000 | 0.207 | 0.408 | 1.97x | 0.208 | 0.408 | 1.96x |
| kfold_seeded_n100000 | 3.689 | 6.939 | 1.88x | 3.846 | 6.938 | 1.80x |
| stratified_seeded_n100000 | 4.766 | 11.477 | 2.41x | 4.971 | 11.476 | 2.31x |
| traintest_seeded_n100000 | 2.027 | 2.010 | 0.99x | 2.069 | 2.010 | 0.97x |
| stratified_traintest_n100000 | 8.123 | 23.531 | 2.90x | 8.231 | 23.529 | 2.86x |
| group_kfold_n100000 | 1.618 | 6.782 | 4.19x | 1.762 | 6.781 | 3.85x |
| stratified_group_kfold_n100000 | 2.747 | 321.584 | 117.05x | 2.916 | 321.526 | 110.27x |
| timeseries_n100000 | 0.000 | 0.043 | 333.18x | 0.000 | 0.043 | 333.16x |
| repeated_kfold_n100000 | 10.460 | 19.370 | 1.85x | 10.642 | 19.367 | 1.82x |
| kfold_n1000000 | 10.386 | 18.038 | 1.74x | 11.170 | 18.037 | 1.61x |
| stratified_n1000000 | 22.065 | 69.988 | 3.17x | 22.954 | 69.984 | 3.05x |
| traintest_n1000000 | 0.742 | 2.181 | 2.94x | 0.865 | 2.181 | 2.52x |
| kfold_seeded_n1000000 | 37.746 | 58.788 | 1.56x | 38.667 | 58.783 | 1.52x |
| stratified_seeded_n1000000 | 48.491 | 111.062 | 2.29x | 49.635 | 111.050 | 2.24x |
| traintest_seeded_n1000000 | 21.493 | 21.311 | 0.99x | 21.940 | 21.310 | 0.97x |
| stratified_traintest_n1000000 | 93.874 | 237.426 | 2.53x | 94.332 | 237.408 | 2.52x |
| group_kfold_n1000000 | 14.520 | 54.286 | 3.74x | 15.127 | 54.280 | 3.59x |
| stratified_group_kfold_n1000000 | 28.220 | 3181.079 | 112.72x | 29.473 | 3180.765 | 107.92x |
| timeseries_n1000000 | 0.000 | 0.327 | 2914.79x | 0.000 | 0.327 | 2915.00x |
| repeated_kfold_n1000000 | 108.050 | 178.382 | 1.65x | 109.541 | 178.367 | 1.63x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.006 | 0.821 | 145.10x | 0.006 | 0.821 | 145.10x |
| mann_whitney_n1000 | 0.030 | 0.754 | 25.43x | 0.030 | 0.753 | 25.43x |
| chi_square_n1000 | 0.000 | 0.341 | 1644.57x | 0.000 | 0.341 | 1644.60x |
| fligner_n1000 | 0.077 | 0.821 | 10.71x | 0.077 | 0.821 | 10.71x |
| anderson_ksamp_n1000 | 0.171 | 0.397 | 2.33x | 0.171 | 0.397 | 2.33x |
| spearman_matrix_n1000 | 0.099 | 0.924 | 9.29x | 0.099 | 0.924 | 9.29x |
| pointbiserial_n1000 | 0.011 | 0.578 | 53.79x | 0.011 | 0.578 | 53.79x |
| welch_t_n10000 | 0.054 | 0.878 | 16.25x | 0.054 | 0.878 | 16.25x |
| mann_whitney_n10000 | 0.764 | 2.832 | 3.71x | 0.764 | 2.832 | 3.71x |
| chi_square_n10000 | 0.001 | 0.344 | 246.83x | 0.001 | 0.344 | 246.85x |
| fligner_n10000 | 1.433 | 3.237 | 2.26x | 1.501 | 3.237 | 2.16x |
| anderson_ksamp_n10000 | 2.004 | 3.993 | 1.99x | 2.115 | 3.992 | 1.89x |
| spearman_matrix_n10000 | 2.130 | 4.808 | 2.26x | 2.194 | 4.808 | 2.19x |
| pointbiserial_n10000 | 0.103 | 0.641 | 6.24x | 0.103 | 0.641 | 6.25x |
| welch_t_n100000 | 0.541 | 1.244 | 2.30x | 0.541 | 1.244 | 2.30x |
| mann_whitney_n100000 | 7.763 | 29.092 | 3.75x | 7.762 | 29.088 | 3.75x |
| chi_square_n100000 | 0.014 | 0.363 | 25.29x | 0.014 | 0.362 | 25.29x |
| fligner_n100000 | 14.536 | 33.175 | 2.28x | 14.855 | 33.172 | 2.23x |
| anderson_ksamp_n100000 | 15.402 | 44.162 | 2.87x | 15.795 | 44.152 | 2.80x |
| spearman_matrix_n100000 | 17.847 | 54.208 | 3.04x | 18.464 | 54.202 | 2.94x |
| pointbiserial_n100000 | 1.325 | 1.403 | 1.06x | 1.339 | 5.597 | 4.18x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 8.523 | 87.039 | 10.21x | 8.521 | 347.124 | 40.74x |
| fit_penalised_weighted_1000 | 7.992 | 94.960 | 11.88x | 7.991 | 379.073 | 47.43x |
| predict_survival_1000 | 0.845 | 2.393 | 2.83x | 0.853 | 2.393 | 2.81x |
| fit_10000 | 238.993 | 626.470 | 2.62x | 239.231 | 1005.662 | 4.20x |
| fit_penalised_weighted_10000 | 218.486 | 653.442 | 2.99x | 218.787 | 1031.855 | 4.72x |
| predict_survival_10000 | 3.336 | 5.182 | 1.55x | 3.541 | 20.673 | 5.84x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.027 | 14.212 | 533.83x | 0.027 | 14.209 | 533.79x |
| logrank_fleming_harrington_1000 | 0.036 | 21.570 | 607.39x | 0.036 | 21.566 | 607.16x |
| multigroup_1000 | 0.117 | 29.260 | 249.85x | 0.117 | 29.256 | 249.83x |
| pairwise_1000 | 0.309 | 150.719 | 487.15x | 0.309 | 150.707 | 487.13x |
| rmst_1000 | 0.094 | 1325.158 | 14163.70x | 0.094 | 1325.003 | 14163.05x |
| fixed_point_1000 | 0.037 | 15.559 | 417.14x | 0.037 | 15.557 | 417.15x |
| concordance_1000 | 0.051 | 8.400 | 164.44x | 0.051 | 8.399 | 164.44x |
| logrank_wilcoxon_weighted_10000 | 0.361 | 16.185 | 44.82x | 0.361 | 16.184 | 44.83x |
| logrank_fleming_harrington_10000 | 0.440 | 25.449 | 57.77x | 0.440 | 25.446 | 57.77x |
| multigroup_10000 | 2.458 | 40.310 | 16.40x | 2.467 | 40.308 | 16.34x |
| pairwise_10000 | 4.868 | 166.556 | 34.21x | 4.869 | 166.542 | 34.21x |
| rmst_10000 | 1.860 | 1405.045 | 755.50x | 1.893 | 1404.935 | 742.06x |
| fixed_point_10000 | 0.425 | 17.708 | 41.69x | 0.425 | 17.707 | 41.69x |
| concordance_10000 | 2.359 | 98.863 | 41.91x | 2.359 | 98.861 | 41.91x |
| logrank_wilcoxon_weighted_100000 | 4.418 | 23.919 | 5.41x | 4.540 | 23.917 | 5.27x |
| logrank_fleming_harrington_100000 | 4.321 | 37.977 | 8.79x | 4.393 | 37.976 | 8.64x |
| multigroup_100000 | 11.023 | 52.813 | 4.79x | 11.168 | 52.810 | 4.73x |
| pairwise_100000 | 45.980 | 247.509 | 5.38x | 47.385 | 247.482 | 5.22x |
| rmst_100000 | 9.360 | 519.167 | 55.47x | 9.487 | 519.128 | 54.72x |
| fixed_point_100000 | 4.808 | 30.517 | 6.35x | 4.963 | 30.514 | 6.15x |
| concordance_100000 | 30.144 | 810.139 | 26.88x | 30.377 | 810.118 | 26.67x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.440 | 75.371 | 21.91x | 3.440 | 75.367 | 21.91x |
| loglogistic_left_1000 | 6.231 | 104.686 | 16.80x | 6.244 | 104.678 | 16.76x |
| lognormal_interval_1000 | 5.751 | 193.488 | 33.64x | 5.751 | 193.486 | 33.64x |
| generalized_gamma_1000 | 45.952 | 857.254 | 18.66x | 45.958 | 857.212 | 18.65x |
| weibull_aft_1000 | 7.152 | 135.979 | 19.01x | 7.153 | 135.974 | 19.01x |
| lognormal_aft_ridge_robust_1000 | 11.011 | 4810.137 | 436.87x | 11.013 | 4809.778 | 436.75x |
| loglogistic_aft_interval_1000 | 10.508 | 212.781 | 20.25x | 10.509 | 212.776 | 20.25x |
| breslow_fleming_harrington_1000 | 0.254 | 6.936 | 27.27x | 0.254 | 6.935 | 27.27x |
| kaplan_meier_left_1000 | 0.090 | 8.178 | 90.67x | 0.090 | 8.177 | 90.67x |
| weibull_10000 | 33.119 | 89.714 | 2.71x | 33.116 | 89.711 | 2.71x |
| loglogistic_left_10000 | 68.276 | 186.729 | 2.73x | 68.277 | 186.725 | 2.73x |
| lognormal_interval_10000 | 57.634 | 347.342 | 6.03x | 57.631 | 347.291 | 6.03x |
| generalized_gamma_10000 | 427.244 | 2092.049 | 4.90x | 427.185 | 2091.823 | 4.90x |
| weibull_aft_10000 | 74.867 | 161.999 | 2.16x | 74.867 | 161.965 | 2.16x |
| lognormal_aft_ridge_robust_10000 | 154.273 | 46686.595 | 302.62x | 154.242 | 46680.378 | 302.64x |
| loglogistic_aft_interval_10000 | 108.426 | 298.202 | 2.75x | 108.432 | 298.146 | 2.75x |
| breslow_fleming_harrington_10000 | 2.578 | 9.608 | 3.73x | 2.578 | 9.606 | 3.73x |
| kaplan_meier_left_10000 | 0.965 | 10.549 | 10.94x | 0.964 | 10.547 | 10.94x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

_As of 2026-10-17, measured at commit `881e9415ead6c6be3c805f7cb2bf0bace0cb5681`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.267 | 0.465 | 1.74x | 0.268 | 0.465 | 1.73x |
| polynomial_n10000 | 0.728 | 1.033 | 1.42x | 0.733 | 1.033 | 1.41x |
| kbins_fit_n10000 | 1.468 | 2.650 | 1.81x | 1.468 | 2.650 | 1.81x |
| kbins_n10000 | 0.723 | 4.575 | 6.33x | 0.816 | 4.575 | 5.60x |
| quantile_fit_n10000 | 1.562 | 3.809 | 2.44x | 1.561 | 3.808 | 2.44x |
| quantile_n10000 | 1.875 | 1.708 | 0.91x | 1.878 | 1.708 | 0.91x |
| power_fit_n10000 | 6.187 | 138.079 | 22.32x | 6.190 | 138.065 | 22.30x |
| power_n10000 | 1.550 | 1.531 | 0.99x | 1.553 | 1.531 | 0.99x |
| knn_impute_n10000 | 8.883 | 24.456 | 2.75x | 8.932 | 97.343 | 10.90x |
| label_n10000 | 1.171 | 0.768 | 0.66x | 1.171 | 0.768 | 0.66x |
| normalize_n100000 | 1.456 | 1.878 | 1.29x | 1.824 | 1.877 | 1.03x |
| polynomial_n100000 | 2.742 | 7.563 | 2.76x | 3.351 | 7.562 | 2.26x |
| kbins_fit_n100000 | 15.010 | 8.436 | 0.56x | 15.831 | 8.435 | 0.53x |
| kbins_n100000 | 6.487 | 13.025 | 2.01x | 7.131 | 13.023 | 1.83x |
| quantile_fit_n100000 | 15.303 | 18.534 | 1.21x | 16.069 | 18.532 | 1.15x |
| quantile_n100000 | 17.795 | 15.118 | 0.85x | 18.292 | 15.117 | 0.83x |
| power_fit_n100000 | 66.616 | 931.523 | 13.98x | 69.229 | 931.441 | 13.45x |
| power_n100000 | 14.176 | 10.081 | 0.71x | 14.638 | 10.081 | 0.69x |
| knn_impute_n100000 | 8.984 | 19.978 | 2.22x | 8.983 | 79.812 | 8.88x |
| label_n100000 | 15.412 | 7.395 | 0.48x | 15.741 | 7.394 | 0.47x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-10-20, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.041 | 2.683 | 66.20x | 0.041 | 2.683 | 66.19x |
| var_n10000 | 0.471 | 16.755 | 35.55x | 0.474 | 16.754 | 35.33x |
| var_n100000 | 4.208 | 195.728 | 46.51x | 4.423 | 420.110 | 94.98x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
