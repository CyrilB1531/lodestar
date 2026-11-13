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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **98.61 ms** |      **3.705 ms** |   **0.203 ms** |  **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   4,737.31 ms |  3,258.235 ms | 178.595 ms | 48.04 |    1.57 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,010.71 ms |    587.103 ms |  32.181 ms | 50.81 |    0.30 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |     **103.16 ms** |      **4.485 ms** |   **0.246 ms** |  **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |   9,812.77 ms |  5,548.253 ms | 304.118 ms | 95.12 |    2.56 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  10,083.91 ms |  6,615.448 ms | 362.615 ms | 97.75 |    3.05 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **998.84 ms** |     **36.428 ms** |   **1.997 ms** |  **1.00** |    **0.00** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  49,259.59 ms | 10,516.422 ms | 576.441 ms | 49.32 |    0.51 |  4000.0000 |  3000.0000 | 2000.0000 | 160.03 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  49,773.96 ms | 11,530.669 ms | 632.035 ms | 49.83 |    0.55 |  7000.0000 |  4000.0000 | 2000.0000 | 211.59 MB |        7.11 |
|                |           |              |                      |               |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |   **1,020.81 ms** |     **21.754 ms** |   **1.192 ms** |  **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 100,167.39 ms | 15,148.210 ms | 830.325 ms | 98.13 |    0.71 | 12000.0000 | 10000.0000 | 8000.0000 | 536.13 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] |  99,654.64 ms |  8,276.622 ms | 453.670 ms | 97.62 |    0.40 | 15000.0000 | 12000.0000 | 8000.0000 | 587.65 MB |       14.00 |

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                          | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,438.82 ns | 318.644 ns | 17.466 ns | 1.000 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.70 ns |   0.486 ns |  0.027 ns | 0.123 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   342.07 ns |  64.842 ns |  3.554 ns | 0.140 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,395.61 ns |  63.382 ns |  3.474 ns | 3.033 |    0.02 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,632.60 ns | 278.939 ns | 15.290 ns | 2.310 |    0.02 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,189.67 ns |  97.357 ns |  5.336 ns | 0.488 |    0.00 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   782.53 ns |  12.052 ns |  0.661 ns | 0.321 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.85 ns |   9.888 ns |  0.542 ns | 0.054 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   139.73 ns |   4.788 ns |  0.262 ns | 0.057 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 2,840.45 ns |  43.729 ns |  2.397 ns | 1.165 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,703.21 ns | 437.575 ns | 23.985 ns | 1.108 |    0.01 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   320.51 ns |  19.808 ns |  1.086 ns | 0.131 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    29.92 ns |   0.241 ns |  0.013 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    61.60 ns |   6.437 ns |  0.353 ns | 0.025 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    75.83 ns |   3.817 ns |  0.209 ns | 0.031 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   367.96 ns |  16.822 ns |  0.922 ns | 0.151 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   474.11 ns |   5.462 ns |  0.299 ns | 0.194 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   390.46 ns |   7.371 ns |  0.404 ns | 0.160 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    21.78 ns |   2.984 ns |  0.164 ns | 0.009 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.31 ns |   0.107 ns |  0.006 ns | 0.009 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.10 ns |   0.135 ns |  0.007 ns | 0.027 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.53 ns |   0.477 ns |  0.026 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.50 ns |   0.227 ns |  0.012 ns | 0.013 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.68 ns |   0.502 ns |  0.028 ns | 0.074 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                     | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|----------:|------------:|
| ChiSquaredSfOneDf          |  31.05 ns | 0.436 ns | 0.024 ns |  1.00 |         - |          NA |
| ChiSquaredSfFourDf         |  13.85 ns | 0.779 ns | 0.043 ns |  0.45 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  43.84 ns | 0.128 ns | 0.007 ns |  1.41 |         - |          NA |
| ChiSquaredSfHundredDf      |  76.87 ns | 4.268 ns | 0.234 ns |  2.48 |         - |          NA |
| ChiSquaredSfFractionalDf   | 118.43 ns | 2.746 ns | 0.151 ns |  3.81 |         - |          NA |
| NormalQuantile             |  28.75 ns | 0.129 ns | 0.007 ns |  0.93 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method       | SampleSize | Regressors | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **35.02 μs** |  **3.472 μs** | **0.190 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  50.85 μs | 14.241 μs | 0.781 μs |  1.45 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **42.59 μs** |  **2.056 μs** | **0.113 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 102.73 μs |  5.060 μs | 0.277 μs |  2.41 |    0.01 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **370.96 μs** |  **6.210 μs** | **0.340 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 371.20 μs | 52.102 μs | 2.856 μs |  1.00 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **411.10 μs** | **38.199 μs** | **2.094 μs** |  **1.00** |    **0.01** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 739.40 μs | 54.887 μs | 3.009 μs |  1.80 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **276.4 μs** |    **20.94 μs** |   **1.15 μs** |   **3.4180** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **3,059.5 μs** |   **105.63 μs** |   **5.79 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **31,815.2 μs** | **2,651.37 μs** | **145.33 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **30.21 μs** |   **4.064 μs** |  **0.223 μs** |  **1.00** |    **0.01** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.69 μs |  12.708 μs |  0.697 μs |  2.01 |    0.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,998.64 μs** | **216.307 μs** | **11.857 μs** |  **1.00** |    **0.00** | **328.1250** | **328.1250** | **328.1250** | **1097.71 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,616.45 μs | 789.580 μs | 43.280 μs |  1.90 |    0.01 | 546.8750 | 546.8750 | 546.8750 | 1877.96 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | MeanCount | Mean     | Error     | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|----------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **304.7 μs** | **157.11 μs** | **8.61 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **306.0 μs** |   **9.11 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **307.2 μs** |  **34.30 μs** | **1.88 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **50.69 μs** |       **6.661 μs** |     **0.365 μs** |  **1.00** |    **0.01** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     106.82 μs |      15.684 μs |     0.860 μs |  2.11 |    0.02 |   1.9531 |   0.1221 |        - |   33.02 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **697.84 μs** |      **52.052 μs** |     **2.853 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,957.68 μs |     584.190 μs |    32.021 μs |  2.81 |    0.04 |  97.6563 |  97.6563 |  97.6563 |  370.09 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,864.98 μs** |     **965.753 μs** |    **52.936 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  23,041.40 μs |   8,892.739 μs |   487.441 μs |  3.36 |    0.07 | 281.2500 | 281.2500 | 281.2500 |  2227.2 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **51,756.89 μs** |   **2,705.446 μs** |   **148.295 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,294.34 μs | 111,292.465 μs | 6,100.316 μs |  2.73 |    0.10 |        - |        - |        - |    8846 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **37.385 μs** |   **0.9716 μs** | **0.0533 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.741 μs |   6.7387 μs | 0.3694 μs |  0.29 |    0.01 |   0.5341 |        - |        - |    8.8 KB |        3.32 |
| Hac       | 100        | 4    |    20.034 μs |   2.3301 μs | 0.1277 μs |  0.54 |    0.00 |   0.7629 |        - |        - |  12.95 KB |        4.89 |
| Cluster   | 100        | 4    |     9.159 μs |   0.4580 μs | 0.0251 μs |  0.24 |    0.00 |   0.5951 |        - |        - |   9.76 KB |        3.68 |
|           |            |      |              |             |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **461.294 μs** |  **38.3742 μs** | **2.1034 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   890.402 μs | 118.1500 μs | 6.4762 μs |  1.93 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.94 KB |        5.91 |
| Hac       | 10000      | 4    | 1,949.544 μs | 102.7058 μs | 5.6296 μs |  4.23 |    0.02 | 246.0938 | 246.0938 | 246.0938 | 863.89 KB |       10.80 |
| Cluster   | 10000      | 4    |   767.062 μs |  29.3895 μs | 1.6109 μs |  1.66 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 565.22 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method     | SampleSize | Mean         | Error       | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **7.777 μs** |   **0.5679 μs** | **0.0311 μs** |  **0.2747** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |    **96.404 μs** |  **19.0445 μs** | **1.0439 μs** |  **1.4648** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,301.736 μs** | **148.0936 μs** | **8.1175 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,431.91 μs** | **381.164 μs** | **20.893 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **46.92 μs** |   **3.740 μs** |  **0.205 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method            | Regressors | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **142.1 μs** |      **32.27 μs** |      **1.77 μs** |  **1.00** |    **0.02** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     203.8 μs |       4.10 μs |      0.22 μs |  1.43 |    0.02 |   1.9531 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     660.1 μs |     258.03 μs |     14.14 μs |  4.65 |    0.10 | 190.4297 | 190.4297 | 190.4297 |   706.08 KB |        3.75 |
|                   |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **62,179.2 μs** |  **46,550.84 μs** |  **2,551.61 μs** |  **1.00** |    **0.05** | **375.0000** | **375.0000** | **375.0000** |  **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 297,560.1 μs |  14,383.71 μs |    788.42 μs |  4.79 |    0.17 |        - |        - |        - |   3499.3 KB |        0.39 |
| MathNet_Qr        | 250        | 490,262.8 μs | 261,082.00 μs | 14,310.79 μs |  7.89 |    0.34 |        - |        - |        - | 32662.32 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Shape   | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **545.0 ms** | **50.44 ms** | **2.77 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **441.5 ms** | **22.34 ms** | **1.22 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method           | Size | Mean          | Error      | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|-----------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.264 μs** |   **2.099 μs** |  **0.1151 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,573.114 μs |  59.579 μs |  3.2657 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     23.155 μs |   2.350 μs |  0.1288 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     92.845 μs |   2.759 μs |  0.1513 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **15.822 μs** |   **1.840 μs** |  **0.1009 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,194.222 μs | 276.324 μs | 15.1463 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    528.651 μs | 476.153 μs | 26.0996 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,343.936 μs |  89.162 μs |  4.8873 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **135.6 μs** |     **5.92 μs** |  **0.32 μs** |  **1.00** |    **0.00** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   892.2 μs |    13.68 μs |  0.75 μs |  6.58 |    0.01 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,312.2 μs** |   **193.17 μs** | **10.59 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,104.2 μs | 1,293.60 μs | 70.91 μs |  5.41 |    0.06 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **36.59 μs** |   **0.462 μs** |  **0.025 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   265.50 μs |  20.770 μs |  1.138 μs |  7.26 |    0.03 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    30.88 μs |   1.954 μs |  0.107 μs |  0.84 |    0.00 |   2.1973 |   0.0610 |        - |   35.66 KB |       13.46 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **458.86 μs** |  **17.757 μs** |  **0.973 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,561.71 μs | 946.986 μs | 51.908 μs |  9.94 |    0.10 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,688.32 μs | 207.871 μs | 11.394 μs |  3.68 |    0.02 | 441.4063 | 441.4063 | 441.4063 | 1765.79 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                       | Mean           | Error         | StdDev      | Median         | Ratio      | RatioSD   | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|------------:|---------------:|-----------:|----------:|----------:|------------:|
| NormalQuantile               |      0.0550 ns |     0.4986 ns |   0.0273 ns |      0.0394 ns |       1.15 |      0.64 |         - |          NA |
| NormalQuantileFarTail        |      0.0562 ns |     0.3409 ns |   0.0187 ns |      0.0493 ns |       1.17 |      0.53 |         - |          NA |
| StudentQuantile              | 14,224.2219 ns | 2,304.6747 ns | 126.3270 ns | 14,292.7388 ns | 296,352.22 | 99,099.41 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,329.8424 ns |    23.2012 ns |   1.2717 ns |  2,329.1242 ns |  48,540.72 | 16,227.19 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,486.7 μs** |    **38.08 μs** |   **2.09 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    419.2 μs |    49.99 μs |   2.74 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    618.0 μs |   303.10 μs |  16.61 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,263.0 μs |    31.32 μs |   1.72 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    691.9 μs |    33.26 μs |   1.82 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **620.8 μs** |    **12.52 μs** |   **0.69 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    244.8 μs |    29.28 μs |   1.61 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    517.7 μs |    14.12 μs |   0.77 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    781.0 μs |    12.50 μs |   0.69 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    398.4 μs |    14.52 μs |   0.80 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,915.0 μs** | **1,175.43 μs** |  **64.43 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,484.1 μs |    99.16 μs |   5.44 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,528.7 μs | 1,515.33 μs |  83.06 μs | 507.8125 | 507.8125 | 507.8125 |  5062691 B |
| WilcoxonPooledDifferences | 100000     | False | 17,198.1 μs | 1,290.14 μs |  70.72 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,193.4 μs |   763.15 μs |  41.83 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,072.4 μs** |   **408.86 μs** |  **22.41 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,798.7 μs |   115.21 μs |   6.32 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,335.4 μs |   338.28 μs |  18.54 μs | 523.4375 | 523.4375 | 523.4375 |  5062699 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,623.3 μs | 3,866.66 μs | 211.94 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,839.6 μs |   158.47 μs |   8.69 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **36.68 μs** |   **2.279 μs** |  **0.125 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.78 μs** |   **1.457 μs** |  **0.080 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.89 μs** |   **0.873 μs** |  **0.048 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc2**        |    **12.05 μs** |   **0.071 μs** |  **0.004 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.93 μs** |   **0.240 μs** |  **0.013 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **452.90 μs** |  **19.627 μs** |  **1.076 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **895.39 μs** |  **30.469 μs** |  **1.670 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **892.15 μs** |  **57.603 μs** |  **3.157 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,079.66 μs** | **828.688 μs** | **45.423 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,059.66 μs** | **213.410 μs** | **11.698 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                         | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.395 μs** |  **0.3710 μs** | **0.0203 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.539 μs |  0.4925 μs | 0.0270 μs |  2.63 |    0.01 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.161 μs |  0.2404 μs | 0.0132 μs |  1.17 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.165 μs |  0.4394 μs | 0.0241 μs |  2.77 |    0.01 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.971 μs |  0.4211 μs | 0.0231 μs |  1.13 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.665 μs |  0.4122 μs | 0.0226 μs |  2.65 |    0.01 |      - |     224 B |        0.09 |
|                                |            |            |            |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.781 μs** |  **1.4305 μs** | **0.0784 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.995 μs |  1.7669 μs | 0.0968 μs |  2.48 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.592 μs |  0.6844 μs | 0.0375 μs |  1.02 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.715 μs |  4.2076 μs | 0.2306 μs |  2.50 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.241 μs |  0.6126 μs | 0.0336 μs |  1.01 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 122.325 μs | 41.2171 μs | 2.2592 μs |  2.51 |    0.04 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.520 μs** |  **1.3971 μs** | **0.0766 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  30.301 μs |  1.6230 μs | 0.0890 μs |  4.03 |    0.04 |   4.0588 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  26.730 μs |  7.8007 μs | 0.4276 μs |  3.55 |    0.06 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.876 μs |  0.1763 μs | 0.0097 μs |  0.52 |    0.00 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.056 μs |  0.7058 μs | 0.0387 μs |  0.54 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.538 μs |  0.4980 μs | 0.0273 μs |  0.20 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.936 μs |  0.8623 μs | 0.0473 μs |  0.39 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **117.502 μs** | **36.1880 μs** | **1.9836 μs** |  **1.00** |    **0.02** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 999.996 μs | 92.9118 μs | 5.0928 μs |  8.51 |    0.13 | 248.0469 | 248.0469 | 248.0469 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 295.414 μs | 30.9536 μs | 1.6967 μs |  2.51 |    0.04 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.805 μs |  2.6006 μs | 0.1425 μs |  0.82 |    0.01 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.584 μs |  3.2444 μs | 0.1778 μs |  0.83 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.404 μs |  2.5213 μs | 0.1382 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  28.893 μs |  1.9534 μs | 0.1071 μs |  0.25 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method              | SampleSize | Mean            | Error           | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,116.0 ns** |       **737.12 ns** |      **40.40 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,431.4 ns |       574.22 ns |      31.47 ns |    6.28 |    0.04 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,160.6 ns |       181.86 ns |       9.97 ns |    0.35 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,775.4 ns |       863.24 ns |      47.32 ns |    3.89 |    0.02 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        118.8 ns |        33.48 ns |       1.83 ns |    0.02 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        221.8 ns |        12.53 ns |       0.69 ns |    0.04 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |                 |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **53,526.8 ns** |       **315.12 ns** |      **17.27 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    141,119.0 ns |     9,439.72 ns |     517.42 ns |   2.636 |    0.01 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    694,243.7 ns |    15,747.61 ns |     863.18 ns |  12.970 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,696,886.8 ns | 2,728,287.46 ns | 149,546.64 ns | 237.206 |    2.42 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        118.3 ns |        12.77 ns |       0.70 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        215.3 ns |        11.95 ns |       0.65 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **14.99 μs** |  **2.408 μs** | **0.132 μs** |   **2.9602** |   **0.3510** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **35.81 μs** |  **2.199 μs** | **0.121 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **51.27 μs** |  **6.390 μs** | **0.350 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **158.64 μs** | **37.033 μs** | **2.030 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **215.03 μs** | **11.573 μs** | **0.634 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **555.95 μs** | **32.174 μs** | **1.764 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **742.05 μs** | **25.079 μs** | **1.375 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,319.70 μs** | **73.536 μs** | **4.031 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **Lodestar_Wls** | **200**        |     **45.09 μs** |      **1.718 μs** |     **0.094 μs** |  **1.00** |    **0.00** |    **0.1831** |         **-** |         **-** |    **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     18.90 μs |      0.333 μs |     0.018 μs |  0.42 |    0.00 |    3.6926 |    0.1831 |         - |   58.29 KB |       16.99 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **2000**       |    **136.16 μs** |     **10.785 μs** |     **0.591 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |   **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    121.05 μs |     21.304 μs |     1.168 μs |  0.89 |    0.01 |   33.2031 |   13.6719 |         - |  508.29 KB |       29.06 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **20000**      |    **931.50 μs** |     **14.989 μs** |     **0.822 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |  **158.51 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,634.31 μs |  2,243.496 μs |   122.974 μs |  1.75 |    0.11 | 1019.5313 | 1019.5313 | 1019.5313 | 5027.72 KB |       31.72 |
|              |            |              |               |              |       |         |           |           |           |            |             |
| **Lodestar_Wls** | **200000**     |  **9,136.69 μs** |    **312.581 μs** |    **17.134 μs** |  **1.00** |    **0.00** |   **93.7500** |   **93.7500** |   **93.7500** | **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 18,766.16 μs | 31,228.612 μs | 1,711.746 μs |  2.05 |    0.16 |  718.7500 |  718.7500 |  718.7500 | 50025.1 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-11-12, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **331.1 μs** |    **20.53 μs** |   **1.13 μs** |   **8.3008** |   **0.9766** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **2,041.3 μs** |   **205.33 μs** |  **11.25 μs** |  **11.7188** |        **-** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,567.9 μs** |   **332.36 μs** |  **18.22 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **18,695.6 μs** | **6,907.10 μs** | **378.60 μs** | **281.2500** | **250.0000** | **250.0000** | **1737.31 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

_As of 2026-11-12, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.27 μs** |     **2.767 μs** |   **0.152 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     15.02 μs |     0.702 μs |   0.038 μs |  0.92 |    0.01 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.66 μs |     3.509 μs |   0.192 μs |  1.33 |    0.01 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **59.88 μs** |     **8.940 μs** |   **0.490 μs** |  **1.00** |    **0.01** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     22.88 μs |     1.454 μs |   0.080 μs |  0.38 |    0.00 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     45.50 μs |     0.990 μs |   0.054 μs |  0.76 |    0.01 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **9,941.86 μs** |   **160.039 μs** |   **8.772 μs** |  **1.00** |    **0.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  9,647.80 μs | 1,281.320 μs |  70.233 μs |  0.97 |    0.01 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               | 11,016.75 μs |   694.078 μs |  38.045 μs |  1.11 |    0.00 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **16,903.37 μs** |   **297.976 μs** |  **16.333 μs** |  **1.00** |    **0.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.01 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 13,593.09 μs | 3,595.161 μs | 197.063 μs |  0.80 |    0.01 |  875.0000 |  828.1250 | 718.7500 | 5660.24 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 17,858.19 μs | 6,967.616 μs | 381.919 μs |  1.06 |    0.02 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                  | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------------------------ |---------:|---------:|---------:|----------:|----------:|
| ChatTemplate            | 97.60 ms | 0.956 ms | 0.052 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens | 82.51 ms | 6.132 ms | 0.336 ms | 1714.2857 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **35.87 ms** |  **3.802 ms** | **0.208 ms** |  **714.2857** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **40.12 ms** |  **6.564 ms** | **0.360 ms** |  **692.3077** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **34.48 ms** |  **0.668 ms** | **0.037 ms** |  **666.6667** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **49.08 ms** |  **3.455 ms** | **0.189 ms** | **1272.7273** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.46 ms** | **10.567 ms** | **0.579 ms** | **1307.6923** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **53.33 ms** |  **2.604 ms** | **0.143 ms** | **1500.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method      | Count | Mean     | Error     | StdDev   | Ratio | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |---------:|----------:|---------:|------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    | **76.84 ms** |  **1.942 ms** | **0.106 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** |  **9386.06 KB** |       **1.000** |
| TreeNearest | 10    | 50.33 ms | 22.360 ms | 1.226 ms |  0.66 |         - |         - |         - |    29.69 KB |       0.003 |
|             |       |          |           |          |       |           |           |           |             |             |
| **FullScan**    | **2000**  | **76.02 ms** |  **1.965 ms** | **0.108 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** | **10007.93 KB** |        **1.00** |
| TreeNearest | 2000  | 72.60 ms |  8.500 ms | 0.466 ms |  0.96 |         - |         - |         - |  1273.29 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Classes | Mean         | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|------------:|------------:|-------:|-------:|----------:|
| **Report** | **10**      |     **605.0 ns** |    **162.8 ns** |     **8.93 ns** | **0.1011** |      **-** |   **1.66 KB** |
| **Report** | **1000**    | **213,525.8 ns** | **19,358.7 ns** | **1,061.11 ns** | **7.3242** | **1.7090** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method          | Samples | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| Roc             | 1000000 | 149.1 ms | 12.09 ms | 0.66 ms |  750.0000 |  750.0000 |  750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 135.2 ms | 21.81 ms | 1.20 ms | 1000.0000 | 1000.0000 | 1000.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                         | Samples | Clusters | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.081 ms** | **0.1340 ms** | **0.0073 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.114 ms | 0.1338 ms | 0.0073 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.352 ms | 0.1813 ms | 0.0099 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.070 ms** | **1.5101 ms** | **0.0828 ms** | **187.5000** | **187.5000** | **187.5000** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.159 ms | 0.7838 ms | 0.0430 ms | 210.9375 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.409 ms | 9.3612 ms | 0.5131 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method              | Length | Mean            | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **875.5 ns** |     **232.46 ns** |     **12.74 ns** |  **1.00** |    **0.02** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,802.5 ns |      45.83 ns |      2.51 ns |  2.06 |    0.03 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **68,036.0 ns** |   **5,926.27 ns** |    **324.84 ns** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,137.4 ns |   5,380.07 ns |    294.90 ns |  2.22 |    0.01 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |               |              |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,335,356.7 ns** | **407,025.52 ns** | **22,310.44 ns** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,319,400.8 ns | 121,617.04 ns |  6,666.24 ns |  2.84 |    0.01 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **20,144.8 μs** |   **2,675.79 μs** |   **146.67 μs** | **3031.2500** | **3000.0000** | **3000.0000** |  **11692.42 KB** |
| TruncatedSvd_Transform     | 500     |     538.7 μs |      98.61 μs |     5.40 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 182,884.0 μs |  20,189.33 μs | 1,106.65 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18143.46 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 255,943.6 μs |  60,194.00 μs | 3,299.44 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **142,515.5 μs** |  **45,258.22 μs** | **2,480.76 μs** | **3750.0000** | **3750.0000** | **3750.0000** | **115064.21 KB** |
| TruncatedSvd_Transform     | 20000   |     924.0 μs |     150.04 μs |     8.22 μs |   43.9453 |   43.9453 |   43.9453 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 715,381.8 μs | 132,077.63 μs | 7,239.62 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.79 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 576,987.8 μs |  31,521.80 μs | 1,727.82 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Mean     | Error   | StdDev  | Gen0    | Allocated |
|------- |---------:|--------:|--------:|--------:|----------:|
| Encode | 282.6 μs | 8.02 μs | 0.44 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method      | Count  | Mean       | Error     | StdDev   | Allocated |
|------------ |------- |-----------:|----------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **525.6 μs** |  **24.75 μs** |  **1.36 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,326.0 μs** | **607.90 μs** | **33.32 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method              | Rows   | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|-------------:|----------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **705.6 μs** |    **236.36 μs** |  **12.96 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  7,364.9 μs |    111.51 μs |   6.11 μs |        - |        - |        - |     24.93 KB |
| **FromBlockNormalized** | **100000** | **73,221.6 μs** | **10,908.94 μs** | **597.96 μs** | **166.6667** | **166.6667** | **166.6667** | **150002.31 KB** |
| MmrSelect100        | 100000 |  7,218.5 μs |     85.64 μs |   4.69 μs |        - |        - |        - |     24.92 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-PVBJKR : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-PVBJKR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **280.88 μs** |    **12.920 μs** |    **14.361 μs** |  **1.00** |    **0.07** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    280.06 μs |    13.464 μs |    14.966 μs |  1.00 |    0.07 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    106.92 μs |     5.990 μs |     6.657 μs |  0.38 |    0.03 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    483.38 μs |    82.617 μs |    95.141 μs |  1.73 |    0.34 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,730.35 μs |   142.857 μs |   158.785 μs |  6.18 |    0.63 |        - |        - |        - |  339.34 KB |        2.03 |
| MlNet_ReplaceMissingValues_Read | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,244.83 μs |    57.526 μs |    63.940 μs |  4.44 |    0.31 |        - |        - |        - |  312.66 KB |        1.87 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    175.90 μs |    20.978 μs |     1.150 μs |  1.00 |    0.01 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     70.66 μs |    21.747 μs |     1.192 μs |  0.40 |    0.01 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.25 μs |     0.870 μs |     0.048 μs |  0.09 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    955.40 μs | 2,552.454 μs |   139.909 μs |  5.43 |    0.69 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    935.41 μs |   338.541 μs |    18.557 μs |  5.32 |    0.10 |  48.8281 |   7.8125 |        - |   797.9 KB |        4.81 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    254.35 μs |    69.575 μs |     3.814 μs |  1.45 |    0.02 |  19.0430 |  18.5547 |        - |  299.33 KB |        1.80 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-PVBJKR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,721.90 μs** |   **861.411 μs** |   **921.701 μs** |  **1.03** |    **0.23** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,742.58 μs |   853.193 μs |   982.538 μs |  0.85 |    0.22 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |    690.00 μs |    15.139 μs |    17.434 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,453.76 μs |    43.815 μs |    44.994 μs |  0.80 |    0.13 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,567.29 μs | 1,268.289 μs | 1,302.440 μs |  2.97 |    0.52 |        - |        - |        - |   445.1 KB |        0.14 |
| MlNet_ReplaceMissingValues_Read | Job-PVBJKR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,616.54 μs | 4,793.414 μs | 5,520.103 μs |  1.90 |    1.02 |        - |        - |        - |  537.01 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,545.25 μs |   738.958 μs |    40.505 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3291.64 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,120.47 μs |   307.465 μs |    16.853 μs |  0.88 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.42 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    453.33 μs |   931.810 μs |    51.076 μs |  0.13 |    0.01 | 128.9063 | 128.9063 | 128.9063 |  626.19 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,541.66 μs | 1,081.474 μs |    59.279 μs |  0.43 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,109.26 μs |   435.681 μs |    23.881 μs |  1.16 |    0.01 |  31.2500 |  15.6250 |        - |  561.76 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,396.05 μs |   945.286 μs |    51.814 μs |  0.68 |    0.01 |  27.3438 |  11.7188 |        - |  454.13 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **496.4 μs** |   **294.86 μs** |  **16.16 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    563.7 μs |    38.83 μs |   2.13 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    676.0 μs |    59.72 μs |   3.27 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,410.1 μs |   190.69 μs |  10.45 μs | 9.7656 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **9,645.5 μs** | **2,293.52 μs** | **125.72 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  |  8,699.8 μs | 1,063.07 μs |  58.27 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 10,071.5 μs |   151.19 μs |   8.29 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 18,105.6 μs | 3,159.36 μs | 173.18 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.574 ms** | **0.1505 ms** | **0.0082 ms** | **277.3438** | **267.5781** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **18.539 ms** | **2.6219 ms** | **0.1437 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method    | Samples | Mean        | Error       | StdDev    | Allocated |
|---------- |-------- |------------:|------------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **40.63 ms** |    **17.79 ms** |  **0.975 ms** |         **-** |
| OneVsOne  | 100000  |    96.33 ms |    15.37 ms |  0.843 ms |  803600 B |
| **OneVsRest** | **1000000** |   **744.39 ms** | **1,484.84 ms** | **81.389 ms** |         **-** |
| OneVsOne  | 1000000 | 1,332.74 ms |   325.45 ms | 17.839 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated  |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|-----------:|
| PerLabel         | 100000 |  6.822 ms |  0.1232 ms | 0.0068 ms |         - |         - |        - |    7.84 KB |
| PerLabelWeighted | 100000 | 11.475 ms |  0.3531 ms | 0.0194 ms |         - |         - |        - |    7.85 KB |
| PerSample        | 100000 | 72.602 ms | 60.2367 ms | 3.3018 ms | 3142.8571 | 3000.0000 | 857.1429 | 39063.2 KB |
| PerClass         | 100000 |  4.617 ms |  0.1025 ms | 0.0056 ms |         - |         - |        - |    7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **39.77 ns** |      **0.872 ns** |   **0.048 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    125.21 ns |      0.509 ns |   0.028 ns |  3.15 |    0.00 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **144.36 ns** |      **1.472 ns** |   **0.081 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,441.91 ns |    842.781 ns |  46.196 ns |  9.99 |    0.28 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **494.38 ns** |     **40.325 ns** |   **2.210 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,046.63 ns |  4,049.234 ns | 221.952 ns | 18.30 |    0.40 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **48,950.78 ns** | **16,947.603 ns** | **928.955 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 46,853.36 ns |  5,585.059 ns | 306.136 ns |  0.96 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 11.06 ms | 3.345 ms | 0.183 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 16.59 ms | 1.521 ms | 0.083 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 13.57 ms | 0.730 ms | 0.040 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 61.33 ms | 3.889 ms | 0.213 ms | 444.4444 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **6.862 ms** | **0.5160 ms** | **0.0283 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.738 ms | 0.1715 ms | 0.0094 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.850 ms** | **0.2531 ms** | **0.0139 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.742 ms | 0.1639 ms | 0.0090 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method            | Q | Element   | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.792 μs** | **0.3528 μs** | **0.0193 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **3.949 μs** | **0.4233 μs** | **0.0232 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.695 μs** | **0.4756 μs** | **0.0261 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **5.072 μs** | **0.5430 μs** | **0.0298 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

_As of 2026-11-02, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 37.211 ms | 1.6227 ms | 0.0889 ms |       - |       - |       - |  800349 B |
| NdcgIgnoringTies                  | 100000 | 35.029 ms | 2.0170 ms | 0.1106 ms |       - |       - |       - |  800345 B |
| DcgTieAveraged                    | 100000 | 21.868 ms | 3.4401 ms | 0.1886 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 17.850 ms | 0.8425 ms | 0.0462 ms |       - |       - |       - |         - |
| CoverageErrorScore                | 100000 |  5.535 ms | 0.1505 ms | 0.0082 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 53.414 ms | 1.0417 ms | 0.0571 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                   | Length | Mean         | Error         | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|--------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **372.9 ns** |      **18.62 ns** |     **1.02 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,366.8 ns |     403.72 ns |    22.13 ns | 14.39 |    0.06 |         - |          NA |
|                          |        |              |               |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,792.1 ns** |   **4,594.02 ns** |   **251.81 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 833,518.5 ns | 154,621.86 ns | 8,475.35 ns | 36.57 |    0.47 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **26.95 ms** | **1.550 ms** | **0.085 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **44.21 ms** | **0.229 ms** | **0.013 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-TSGYHD : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-TSGYHD** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **110.221 μs** |     **3.9999 μs** |     **4.1076 μs** |  **1.00** |    **0.05** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    119.479 μs |    19.4081 μs |    21.5721 μs |  1.09 |    0.20 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,296.173 μs |    26.4201 μs |    30.4255 μs | 20.86 |    0.82 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    123.460 μs |    23.6193 μs |    27.2000 μs |  1.12 |    0.24 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |     77.585 μs |    12.5319 μs |    13.4090 μs |  0.70 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |     67.220 μs |    12.6848 μs |    13.5726 μs |  0.61 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    104.644 μs |    15.9574 μs |    15.6723 μs |  0.95 |    0.14 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |    440.296 μs |    16.8253 μs |    18.7013 μs |  4.00 |    0.22 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,270.320 μs |    87.4649 μs |    97.2170 μs | 20.63 |    1.15 |        - |        - |        - |  297008 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,477.092 μs |   128.5983 μs |   132.0610 μs | 58.84 |    2.48 |        - |        - |        - |  461192 B |        5.65 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     48.724 μs |     6.8228 μs |     0.3740 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     41.184 μs |     6.2599 μs |     0.3431 μs |  0.85 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.402 μs |    15.4942 μs |     0.8493 μs |  9.55 |    0.07 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.167 μs |     1.5703 μs |     0.0861 μs |  0.95 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.565 μs |     0.7846 μs |     0.0430 μs |  0.22 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.681 μs |     0.7368 μs |     0.0404 μs |  0.16 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.925 μs |     2.1837 μs |     0.1197 μs |  0.24 |    0.00 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,564.062 μs | 4,361.1427 μs |   239.0489 μs | 32.10 |    4.25 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,062.521 μs | 1,380.9001 μs |    75.6918 μs | 21.81 |    1.35 |  39.0625 |   1.9531 |        - |  652764 B |       8.069 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,644.308 μs | 2,783.0052 μs |   152.5459 μs | 33.75 |    2.72 |  35.1563 |        - |        - |  625462 B |       7.732 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-TSGYHD** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,545.853 μs** |    **14.5609 μs** |    **16.7683 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,451.713 μs |    11.0741 μs |    12.3089 μs |  0.94 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,236.034 μs |    77.9333 μs |    83.3877 μs |  9.86 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,541.842 μs |    17.3956 μs |    20.0329 μs |  1.00 |    0.02 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    417.040 μs |     7.2946 μs |     8.1080 μs |  0.27 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    310.750 μs |     2.1239 μs |     1.9867 μs |  0.20 |    0.00 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |    428.206 μs |    11.9083 μs |    13.2361 μs |  0.28 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,764.834 μs |    41.6950 μs |    46.3438 μs |  4.38 |    0.05 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,660.220 μs | 8,421.3042 μs | 9,697.9864 μs | 11.43 |    6.12 |        - |        - |        - |  530424 B |       0.331 |
| MlNet_NormalizeRobustScaling_Read | Job-TSGYHD | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,310.536 μs |   194.5762 μs |   208.1943 μs | 12.49 |    0.19 |        - |        - |        - | 4278520 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,386.666 μs | 1,663.9432 μs |    91.2063 μs |  1.00 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601938 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,433.928 μs |   729.0498 μs |    39.9617 μs |  1.04 |    0.06 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,479.966 μs |    98.9343 μs |     5.4229 μs | 10.47 |    0.58 | 875.0000 | 875.0000 | 875.0000 | 3201194 B |       1.998 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,465.670 μs | 1,467.8257 μs |    80.4565 μs |  1.06 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601039 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    298.243 μs |    79.2462 μs |     4.3438 μs |  0.22 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    244.214 μs |    12.8615 μs |     0.7050 μs |  0.18 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    402.342 μs |    35.1962 μs |     1.9292 μs |  0.29 |    0.02 | 128.9063 | 124.0234 | 124.0234 |  560068 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,633.497 μs | 2,378.5702 μs |   130.3775 μs |  1.18 |    0.10 |  41.0156 |   6.8359 |        - |  702655 B |       0.439 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,413.796 μs | 1,620.0944 μs |    88.8028 μs |  3.19 |    0.19 |  31.2500 |   7.8125 |        - |  594265 B |       0.371 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,463.651 μs |   600.5423 μs |    32.9177 μs | 14.08 |    0.78 | 250.0000 | 125.0000 |        - | 4302324 B |       2.686 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method          | Model   | Mean     | Error    | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **137.9 ms** |  **5.28 ms** | **0.29 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **134.1 ms** | **20.37 ms** | **1.12 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.30 ms** | **0.536 ms** | **0.029 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **193.75 ms** | **3.188 ms** | **0.175 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **79.000 μs** |    **31.907 μs** |   **1.7489 μs** |  **1.00** |    **0.03** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    189.465 μs |    42.886 μs |   2.3507 μs |  2.40 |    0.05 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.795 μs |     1.183 μs |   0.0648 μs |  0.06 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    332.459 μs |    34.246 μs |   1.8771 μs |  4.21 |    0.08 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  6,845.573 μs |   771.034 μs |  42.2630 μs | 86.68 |    1.71 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     64.248 μs |     7.287 μs |   0.3994 μs |  0.81 |    0.02 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,154.760 μs |   569.908 μs |  31.2386 μs | 14.62 |    0.44 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,104.476 μs** |   **271.748 μs** |  **14.8954 μs** |  **1.00** |    **0.02** | **458.9844** | **445.3125** | **435.5469** | **2738.56 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,417.115 μs |   619.176 μs |  33.9391 μs |  2.19 |    0.04 | 492.1875 | 476.5625 | 468.7500 | 3130.08 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     82.197 μs |    63.733 μs |   3.4934 μs |  0.07 |    0.00 |  52.4902 |  52.4902 |  52.4902 |  391.17 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    281.979 μs |   184.385 μs |  10.1068 μs |  0.26 |    0.01 |   1.9531 |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 44,128.260 μs | 4,712.400 μs | 258.3026 μs | 39.96 |    0.51 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     64.410 μs |     3.142 μs |   0.1722 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,674.297 μs |   598.047 μs |  32.7810 μs |  6.95 |    0.09 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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
| **Count**                | **200**       | **1.872 ms** | **0.2206 ms** | **0.0121 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.387 ms | 0.0924 ms | 0.0051 ms |  0.74 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.994 ms | 0.0767 ms | 0.0042 ms |  1.07 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.707 ms | 0.1451 ms | 0.0080 ms |  0.91 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **8.849 ms** | **0.2746 ms** | **0.0151 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** | **3436.53 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.075 ms | 0.2018 ms | 0.0111 ms |  0.80 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.556 ms | 0.1343 ms | 0.0074 ms |  1.08 | 531.2500 | 515.6250 | 515.6250 | 2349.23 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.688 ms | 0.2021 ms | 0.0111 ms |  0.98 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method  | Words | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **650.3 μs** |   **128.1 μs** |   **7.02 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,789.6 μs** |   **138.8 μs** |   **7.61 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **12,144.6 μs** | **2,254.5 μs** | **123.58 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.83 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

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

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.68 ms** | **5.008 ms** | **0.275 ms** |         **-** |
| **TopTwo** | **100**     | **87.64 ms** | **7.066 ms** | **0.387 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-UDACYX : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-UDACYX** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **47.42 μs** |      **5.275 μs** |      **5.644 μs** |     **46.06 μs** |   **1.01** |    **0.16** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |     82.03 μs |      4.526 μs |      5.031 μs |     80.94 μs |   1.75 |    0.21 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    243.73 μs |     20.008 μs |     23.042 μs |    245.32 μs |   5.20 |    0.72 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    273.53 μs |     13.996 μs |     16.117 μs |    267.00 μs |   5.84 |    0.68 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    999.18 μs |     18.847 μs |     20.948 μs |    993.76 μs |  21.32 |    2.21 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,632.43 μs |     20.220 μs |     22.475 μs |  1,630.44 μs |  34.83 |    3.58 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,087.16 μs |     13.226 μs |     13.582 μs |  1,087.68 μs |  23.20 |    2.38 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |    219.39 μs |     12.833 μs |     14.779 μs |    213.02 μs |   4.68 |    0.57 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,898.50 μs |     19.069 μs |     21.196 μs |  1,896.51 μs |  40.51 |    4.15 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     | 14,245.99 μs |  5,041.560 μs |  5,805.868 μs | 10,179.52 μs | 303.96 |  125.41 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,217.14 μs |     94.421 μs |     96.964 μs |  1,240.46 μs |  25.97 |    3.33 |        - |        - |        - |  315.34 KB |        9.86 |
| MlNet_NormalizeBinning_Read | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,099.85 μs |    143.964 μs |    154.040 μs |  3,137.43 μs |  66.14 |    7.46 |        - |        - |        - |  401.91 KB |       12.56 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     14.01 μs |      0.504 μs |      0.028 μs |     14.02 μs |   1.00 |    0.00 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.50 μs |     18.449 μs |      1.011 μs |     83.21 μs |   5.96 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    137.57 μs |     14.006 μs |      0.768 μs |    137.68 μs |   9.82 |    0.05 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     74.23 μs |      1.980 μs |      0.109 μs |     74.21 μs |   5.30 |    0.01 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     87.59 μs |    200.948 μs |     11.015 μs |     93.40 μs |   6.25 |    0.68 |   1.9531 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    462.76 μs |     45.894 μs |      2.516 μs |    463.29 μs |  33.02 |    0.17 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    196.52 μs |     60.490 μs |      3.316 μs |    195.25 μs |  14.02 |    0.21 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    140.50 μs |      2.199 μs |      0.121 μs |    140.44 μs |  10.03 |    0.02 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    721.87 μs |     18.562 μs |      1.017 μs |    722.29 μs |  51.51 |    0.11 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,722.16 μs |    577.275 μs |     31.642 μs |  8,707.06 μs | 622.41 |    2.23 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    254.17 μs |     49.114 μs |      2.692 μs |    253.29 μs |  18.14 |    0.17 |  19.0430 |  18.5547 |   0.4883 |  297.69 KB |        9.52 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    982.44 μs |  1,175.145 μs |     64.414 μs |    971.23 μs |  70.11 |    3.98 |  42.9688 |   5.8594 |        - |  698.95 KB |       22.35 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-UDACYX** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **479.50 μs** |     **11.269 μs** |     **12.977 μs** |    **479.03 μs** |   **1.00** |    **0.04** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,655.58 μs |     12.738 μs |     13.629 μs |  1,651.38 μs |   3.46 |    0.09 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,575.06 μs |     20.853 μs |     24.015 μs |  3,567.09 μs |   7.46 |    0.20 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,886.45 μs |    962.966 μs |  1,108.954 μs |  4,465.50 μs |   8.11 |    2.27 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,669.42 μs |  5,762.901 μs |  6,636.565 μs |  6,241.56 μs |  24.35 |   13.54 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,878.86 μs |     36.820 μs |     39.397 μs | 10,872.37 μs |  22.70 |    0.60 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,642.81 μs |  5,222.015 μs |  6,013.680 μs |  5,813.72 μs |  20.12 |   12.26 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,083.43 μs |     21.758 μs |     25.057 μs |  3,083.37 μs |   6.43 |    0.18 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,680.35 μs |    135.792 μs |    150.932 μs | 14,728.15 μs |  30.64 |    0.86 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,696.00 μs |     56.740 μs |     58.267 μs |  9,673.02 μs |  20.24 |    0.54 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,110.09 μs |  1,773.907 μs |  1,898.060 μs | 13,118.57 μs |  25.27 |    3.91 |        - |        - |        - |  546.52 KB |        0.87 |
| MlNet_NormalizeBinning_Read | Job-UDACYX | 1               | 20             | Throughput  | 1            | 5           | 20000    | 29,371.60 μs | 14,122.988 μs | 16,264.054 μs | 18,933.46 μs |  61.30 |   33.17 |        - |        - |        - |  2078.1 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    680.90 μs |    103.557 μs |      5.676 μs |    681.63 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    653.81 μs |    380.520 μs |     20.858 μs |    655.12 μs |   0.96 |    0.03 | 282.2266 | 282.2266 | 282.2266 | 2346.24 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,167.27 μs |    304.184 μs |     16.673 μs |  3,167.32 μs |   4.65 |    0.04 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,894.95 μs |     74.592 μs |      4.089 μs |  1,896.89 μs |   2.78 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,742.40 μs |     61.567 μs |      3.375 μs |  5,741.82 μs |   8.43 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,680.29 μs |    595.405 μs |     32.636 μs | 10,673.43 μs |  15.69 |    0.12 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,889.44 μs |    209.120 μs |     11.463 μs |  5,894.54 μs |   8.65 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,208.37 μs |    201.180 μs |     11.027 μs |  3,204.29 μs |   4.71 |    0.04 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,748.99 μs |    615.462 μs |     33.736 μs | 13,755.10 μs |  20.19 |    0.15 | 750.0000 | 750.0000 | 750.0000 | 2660.98 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,151.77 μs |    381.146 μs |     20.892 μs |  9,141.18 μs |  13.44 |    0.10 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,288.57 μs |  1,090.981 μs |     59.800 μs |  2,283.81 μs |   3.36 |    0.08 |  27.3438 |  11.7188 |        - |  454.68 KB |        0.73 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,670.80 μs |    633.926 μs |     34.748 μs | 10,659.19 μs |  15.67 |    0.12 | 265.6250 | 218.7500 | 187.5000 |  2122.9 KB |        3.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **48.26 ns** | **5.224 ns** | **0.286 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  49.43 ns | 0.881 ns | 0.048 ns |  1.02 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **95.29 ns** | **3.078 ns** | **0.169 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  93.51 ns | 0.973 ns | 0.053 ns |  0.98 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **124.09 ns** | **5.627 ns** | **0.308 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 123.41 ns | 1.620 ns | 0.089 ns |  0.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

_As of 2026-11-01, measured at commit `3e48e17c68bff96a3371172656990689433f479e`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-AKIUDK : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **1000**    |     **61.40 μs** |     **0.054 μs** |   **0.056 μs** | **0.1221** |   **3.15 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 1000    |     76.65 μs |     0.183 μs |   0.203 μs | 1.2207 |  20.58 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 1000    |     70.69 μs |     0.097 μs |   0.103 μs | 0.8545 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     59.63 μs |     2.083 μs |   0.114 μs | 0.1831 |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     75.64 μs |     3.017 μs |   0.165 μs | 1.2207 |  20.58 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     71.50 μs |     4.410 μs |   0.242 μs | 0.8545 |  15.52 KB |
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **10000**   |    **551.78 μs** |     **1.342 μs** |   **1.436 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 10000   |    894.21 μs |    10.194 μs |  11.330 μs | 0.9766 |  26.16 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 10000   |    829.25 μs |     2.793 μs |   2.988 μs | 0.9766 |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    581.02 μs |   193.700 μs |  10.617 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |    854.67 μs |    59.998 μs |   3.289 μs | 0.9766 |  26.16 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    866.59 μs |    68.713 μs |   3.766 μs | 0.9766 |  21.14 KB |
| **UpsertThenSearch** | **Job-AKIUDK** | **20**             | **Throughput**  | **5**           | **100000**  |  **8,617.58 μs** |    **58.498 μs** |  **67.366 μs** |      **-** |   **3.16 KB** |
| UpsertThenHybrid | Job-AKIUDK | 20             | Throughput  | 5           | 100000  | 12,611.72 μs |    72.666 μs |  83.682 μs |      - |     46 KB |
| Hybrid           | Job-AKIUDK | 20             | Throughput  | 5           | 100000  | 12,675.67 μs |    97.449 μs | 112.222 μs |      - |  41.02 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  |  8,822.61 μs |   308.219 μs |  16.895 μs |      - |   3.19 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 12,648.13 μs | 1,842.959 μs | 101.019 μs |      - |     46 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 12,842.94 μs | 1,514.929 μs |  83.038 μs |      - |  41.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-cdist

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-cluster-weighted

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### compare-indel

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-iv

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### compare-levenshtein

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-ols

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### compare-persistence

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-survival-aalen

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### compare-transformers

_As of 2026-11-11, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### compare-var

_As of 2026-11-08, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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
