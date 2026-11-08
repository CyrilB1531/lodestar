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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method         | Documents | Permutations | Device               | Mean         | Error         | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |-------------:|--------------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |     **87.24 ms** |      **6.588 ms** |   **0.361 ms** |  **1.00** |    **0.01** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |  4,986.04 ms |  3,044.722 ms | 166.891 ms | 57.15 |    1.67 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |  4,920.48 ms |  1,722.213 ms |  94.400 ms | 56.40 |    0.96 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |    **103.94 ms** |      **4.345 ms** |   **0.238 ms** |  **1.00** |    **0.00** |   **200.0000** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  9,931.53 ms |  3,137.327 ms | 171.967 ms | 95.55 |    1.45 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] | 10,125.18 ms |  2,681.159 ms | 146.963 ms | 97.41 |    1.24 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |    **884.55 ms** |     **73.910 ms** |   **4.051 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] | 49,836.05 ms |  1,388.327 ms |  76.099 ms | 56.34 |    0.24 |  4000.0000 |  3000.0000 | 2000.0000 | 160.06 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] | 49,758.69 ms | 10,816.181 ms | 592.871 ms | 56.25 |    0.62 |  7000.0000 |  4000.0000 | 2000.0000 | 211.62 MB |        7.11 |
|                |           |              |                      |              |               |            |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |  **1,028.81 ms** |     **13.512 ms** |   **0.741 ms** |  **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 99,131.13 ms | 13,220.052 ms | 724.636 ms | 96.35 |    0.61 | 12000.0000 | 11000.0000 | 8000.0000 | 536.09 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 98,763.93 ms | 13,328.966 ms | 730.606 ms | 96.00 |    0.62 | 15000.0000 | 12000.0000 | 8000.0000 | 587.68 MB |       14.00 |

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

| Method                         | PairCount | Mean             | Error            | StdDev          | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|----------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |       **1,012.3 ns** |        **157.85 ns** |         **8.65 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         797.9 ns |         31.95 ns |         1.75 ns |     0.79 |    0.01 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,657.4 ns |        348.97 ns |        19.13 ns |     2.63 |    0.03 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,716.3 ns |         61.83 ns |         3.39 ns |     3.67 |    0.03 |   0.1183 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,353.4 ns |        386.19 ns |        21.17 ns |     5.29 |    0.04 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,882.8 ns |      2,125.22 ns |       116.49 ns |    10.75 |    0.13 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,314.2 ns |        277.99 ns |        15.24 ns |     5.25 |    0.04 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |                 |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,780.5 ns** |     **30,365.60 ns** |     **1,664.44 ns** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,780.9 ns |      1,746.78 ns |        95.75 ns |     0.65 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,144,467.3 ns |    251,932.23 ns |    13,809.26 ns |    13.19 |    0.26 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,942,567.9 ns |     67,091.80 ns |     3,677.53 ns |    22.39 |    0.37 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,141,410.3 ns |     16,798.45 ns |       920.78 ns |    24.68 |    0.41 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,577,860.5 ns | 24,056,910.86 ns | 1,318,640.47 ns | 3,326.18 |   56.21 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,126,410.9 ns |     39,723.10 ns |     2,177.36 ns |    24.51 |    0.40 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| Lodestar_StudentCdf             |   295.58 ns |  16.975 ns |  0.930 ns |  1.00 |    0.00 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.95 ns |   5.273 ns |  0.289 ns |  1.01 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   338.35 ns |  15.830 ns |  0.868 ns |  1.14 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        |   678.35 ns |  27.806 ns |  1.524 ns |  2.30 |    0.01 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,653.51 ns | 162.085 ns |  8.884 ns | 19.13 |    0.06 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,201.43 ns |  30.689 ns |  1.682 ns |  4.06 |    0.01 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   136.07 ns |  50.204 ns |  2.752 ns |  0.46 |    0.01 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.90 ns |   2.663 ns |  0.146 ns |  0.45 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   138.07 ns |   2.740 ns |  0.150 ns |  0.47 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         |   203.29 ns |   3.877 ns |  0.213 ns |  0.69 |    0.00 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,677.03 ns | 234.326 ns | 12.844 ns |  9.06 |    0.05 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   320.98 ns |  17.768 ns |  0.974 ns |  1.09 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    30.03 ns |   2.238 ns |  0.123 ns |  0.10 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    60.80 ns |   1.659 ns |  0.091 ns |  0.21 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    72.76 ns |   2.548 ns |  0.140 ns |  0.25 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   368.65 ns |  38.174 ns |  2.092 ns |  1.25 |    0.01 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   474.29 ns |  57.771 ns |  3.167 ns |  1.60 |    0.01 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   391.06 ns |   3.841 ns |  0.211 ns |  1.32 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    21.53 ns |   0.049 ns |  0.003 ns |  0.07 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.31 ns |   2.399 ns |  0.131 ns |  0.08 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.07 ns |   1.049 ns |  0.058 ns |  0.22 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.80 ns |   1.101 ns |  0.060 ns |  0.10 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.75 ns |   2.364 ns |  0.130 ns |  0.10 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.63 ns |   3.204 ns |  0.176 ns |  0.61 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  31.07 ns |  0.209 ns | 0.011 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  13.80 ns |  0.305 ns | 0.017 ns |  0.44 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  43.58 ns |  0.068 ns | 0.004 ns |  1.40 |    0.00 |         - |          NA |
| ChiSquaredSfHundredDf      |  76.48 ns | 11.226 ns | 0.615 ns |  2.46 |    0.02 |         - |          NA |
| ChiSquaredSfFractionalDf   | 118.51 ns |  2.656 ns | 0.146 ns |  3.81 |    0.00 |         - |          NA |
| NormalQuantile             |  28.92 ns |  4.526 ns | 0.248 ns |  0.93 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Lodestar_Glm** | **200**        | **1**          |  **36.13 μs** | **13.862 μs** | **0.760 μs** |  **1.00** |    **0.03** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  52.73 μs | 17.010 μs | 0.932 μs |  1.46 |    0.03 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.70 μs** |  **1.683 μs** | **0.092 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 109.82 μs | 18.023 μs | 0.988 μs |  2.63 |    0.02 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **351.18 μs** |  **6.393 μs** | **0.350 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 375.98 μs | 80.815 μs | 4.430 μs |  1.07 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **414.32 μs** |  **8.461 μs** | **0.464 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 740.44 μs | 76.319 μs | 4.183 μs |  1.79 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **207.6 μs** |    **44.97 μs** |   **2.46 μs** |   **0.4883** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,570.5 μs** |   **423.01 μs** |  **23.19 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **26,605.9 μs** | **8,147.65 μs** | **446.60 μs** | **437.5000** | **437.5000** | **437.5000** | **5473.82 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method              | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.99 μs** |   **1.126 μs** |  **0.062 μs** |  **1.00** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.67 μs |   0.792 μs |  0.043 μs |  2.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |            |           |       |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,019.15 μs** | **348.002 μs** | **19.075 μs** |  **1.00** | **328.1250** | **328.1250** | **328.1250** | **1097.74 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,777.77 μs |  21.138 μs |  1.159 μs |  1.94 | 546.8750 | 546.8750 | 546.8750 | 1878.09 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **322.0 μs** |  **1.53 μs** | **0.08 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **327.1 μs** | **10.02 μs** | **0.55 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **309.9 μs** |  **9.20 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **28.94 μs** |      **0.194 μs** |     **0.011 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     107.06 μs |     33.142 μs |     1.817 μs |  3.70 |    0.05 |   1.9531 |        - |        - |   33.01 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **671.53 μs** |     **78.374 μs** |     **4.296 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,794.11 μs |    185.902 μs |    10.190 μs |  2.67 |    0.02 |  97.6563 |  97.6563 |  97.6563 |  370.13 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **7,246.24 μs** |    **681.132 μs** |    **37.335 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  22,251.97 μs |  3,103.963 μs |   170.139 μs |  3.07 |    0.02 | 281.2500 | 281.2500 | 281.2500 | 2227.51 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **52,403.36 μs** |  **1,386.598 μs** |    **76.004 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,086.10 μs | 61,047.027 μs | 3,346.194 μs |  2.69 |    0.06 |        - |        - |        - | 8848.61 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |     **6.610 μs** |   **0.7211 μs** |  **0.0395 μs** |  **1.00** |    **0.01** |   **0.1602** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |     9.593 μs |   0.1980 μs |  0.0109 μs |  1.45 |    0.01 |   0.5341 |        - |        - |    8.8 KB |        3.32 |
| Hac       | 100        | 4    |    19.171 μs |   0.5663 μs |  0.0310 μs |  2.90 |    0.02 |   0.7629 |        - |        - |  12.95 KB |        4.89 |
| Cluster   | 100        | 4    |     8.468 μs |   1.2451 μs |  0.0682 μs |  1.28 |    0.01 |   0.5951 |        - |        - |   9.76 KB |        3.68 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **434.863 μs** |  **16.1614 μs** |  **0.8859 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   906.148 μs |  25.9953 μs |  1.4249 μs |  2.08 |    0.00 | 124.0234 | 124.0234 | 124.0234 | 472.94 KB |        5.91 |
| Hac       | 10000      | 4    | 1,967.774 μs | 285.0405 μs | 15.6240 μs |  4.53 |    0.03 | 246.0938 | 246.0938 | 246.0938 | 863.94 KB |       10.80 |
| Cluster   | 10000      | 4    |   769.054 μs |  52.8429 μs |  2.8965 μs |  1.77 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 565.22 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | SampleSize | Mean         | Error       | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **7.773 μs** |   **0.3981 μs** | **0.0218 μs** |  **0.0458** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **120.895 μs** |  **15.1288 μs** | **0.8293 μs** |  **0.2441** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,176.126 μs** | **111.3224 μs** | **6.1020 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.04 μs** |  **9.958 μs** | **0.546 μs** |  **1.00** |    **0.02** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    33.43 μs |  2.113 μs | 0.116 μs |  1.08 |    0.02 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,001.90 μs** | **68.478 μs** | **3.753 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,456.30 μs | 51.686 μs | 2.833 μs |  0.73 |    0.00 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method        | Shape    | Mean        | Error      | StdDev   | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|---------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,428.66 μs** | **112.403 μs** | **6.161 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **46.98 μs** |   **4.282 μs** | **0.235 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **110.2 μs** |      **39.56 μs** |     **2.17 μs** |  **1.00** |    **0.02** |  **49.9268** |  **49.9268** |  **49.9268** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     182.4 μs |       5.18 μs |     0.28 μs |  1.65 |    0.03 |   0.2441 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     576.0 μs |     321.17 μs |    17.60 μs |  5.23 |    0.16 | 190.4297 | 190.4297 | 190.4297 |   705.13 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **79,403.3 μs** |  **39,876.07 μs** | **2,185.74 μs** |  **1.00** |    **0.03** | **428.5714** | **428.5714** | **428.5714** |  **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 310,427.8 μs |  68,082.47 μs | 3,731.83 μs |  3.91 |    0.10 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 499,210.3 μs | 176,831.78 μs | 9,692.75 μs |  6.29 |    0.19 |        - |        - |        - | 32672.16 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Exact**  | **141x141** | **563.1 ms** | **112.8 ms** | **6.18 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **456.1 ms** | **156.8 ms** | **8.60 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method           | Size | Mean          | Error         | StdDev      | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|--------------:|------------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.131 μs** |     **0.3987 μs** |   **0.0219 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,561.287 μs |    58.3898 μs |   3.2005 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     18.349 μs |     5.1178 μs |   0.2805 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     93.343 μs |     7.3341 μs |   0.4020 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.136 μs** |     **3.7306 μs** |   **0.2045 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,673.426 μs | 2,091.7505 μs | 114.6559 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    542.669 μs |   317.3872 μs |  17.3971 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,340.915 μs |   376.6995 μs |  20.6482 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

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

| Method                         | SampleSize | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **4,615.5 ns** |     **813.77 ns** |    **44.61 ns** |   **1.00** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,189.3 ns |     167.06 ns |     9.16 ns |   0.47 |    0.00 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,228.3 ns |      70.77 ns |     3.88 ns |   0.48 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,843.5 ns |     737.95 ns |    40.45 ns |   0.83 |    0.01 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,479.9 ns |   2,218.77 ns |   121.62 ns |   1.62 |    0.03 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |    10,216.1 ns |   5,670.87 ns |   310.84 ns |   2.21 |    0.06 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,484.3 ns |     380.39 ns |    20.85 ns |   0.54 |    0.01 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,031.1 ns |     110.95 ns |     6.08 ns |   0.87 |    0.01 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,658.6 ns |     116.87 ns |     6.41 ns |   0.36 |    0.00 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,158.5 ns |     134.25 ns |     7.36 ns |   0.68 |    0.01 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,144.6 ns |      63.16 ns |     3.46 ns |   0.25 |    0.00 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,049.4 ns |      69.18 ns |     3.79 ns |   0.23 |    0.00 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       456.4 ns |      11.35 ns |     0.62 ns |   0.10 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,533.8 ns |     166.76 ns |     9.14 ns |   0.33 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       111.6 ns |       6.10 ns |     0.33 ns |   0.02 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       673.5 ns |      40.56 ns |     2.22 ns |   0.15 |    0.00 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |             |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **62,154.2 ns** |   **3,623.33 ns** |   **198.61 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,922.5 ns |   2,862.10 ns |   156.88 ns |  2.879 |    0.01 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   695,264.1 ns |  57,478.14 ns | 3,150.57 ns | 11.186 |    0.05 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,074,834.0 ns |  87,822.55 ns | 4,813.85 ns | 33.382 |    0.11 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,475,834.5 ns |  20,824.93 ns | 1,141.48 ns | 23.745 |    0.07 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,632,962.8 ns | 170,341.30 ns | 9,336.98 ns | 58.451 |    0.21 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,454,505.2 ns |  70,563.72 ns | 3,867.84 ns | 23.402 |    0.08 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,152,492.8 ns | 155,497.09 ns | 8,523.32 ns | 34.632 |    0.15 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    85,719.5 ns |   1,934.11 ns |   106.02 ns |  1.379 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   268,211.6 ns |   4,320.18 ns |   236.80 ns |  4.315 |    0.01 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   190,286.6 ns |   7,577.52 ns |   415.35 ns |  3.062 |    0.01 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   384,495.1 ns |  10,525.29 ns |   576.93 ns |  6.186 |    0.02 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       460.3 ns |       4.56 ns |     0.25 ns |  0.007 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,544.1 ns |      33.42 ns |     1.83 ns |  0.025 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       114.5 ns |      10.24 ns |     0.56 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       662.1 ns |      41.01 ns |     2.25 ns |  0.011 |    0.00 | 0.0572 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Lodestar** | **200**        |   **134.9 μs** |    **30.98 μs** |  **1.70 μs** |  **1.00** |    **0.02** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   940.6 μs |    33.35 μs |  1.83 μs |  6.97 |    0.08 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,311.5 μs** |   **105.06 μs** |  **5.76 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,410.9 μs | 1,051.72 μs | 57.65 μs |  5.65 |    0.04 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method       | SampleSize | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |     **6.616 μs** |   **0.0735 μs** | **0.0040 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   264.564 μs |  13.0068 μs | 0.7129 μs | 39.99 |    0.10 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    31.501 μs |   4.4987 μs | 0.2466 μs |  4.76 |    0.03 |   2.1973 |   0.0610 |        - |   35.67 KB |       13.47 |
|              |            |              |             |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **449.217 μs** |  **14.9848 μs** | **0.8214 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 5,137.617 μs |  27.1630 μs | 1.4889 μs | 11.44 |    0.02 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,760.460 μs | 169.4404 μs | 9.2876 μs |  3.92 |    0.02 | 441.4063 | 441.4063 | 441.4063 |  1765.4 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                       | Mean        | Error     | StdDev    | Ratio     | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |------------:|----------:|----------:|----------:|--------:|----------:|------------:|
| NormalQuantile               |   0.0408 ns | 0.0303 ns | 0.0017 ns |      1.00 |    0.05 |         - |          NA |
| NormalQuantileFarTail        |   0.0398 ns | 0.0210 ns | 0.0011 ns |      0.98 |    0.04 |         - |          NA |
| StudentQuantile              | 559.3424 ns | 5.8360 ns | 0.3199 ns | 13,710.47 |  494.13 |         - |          NA |
| StudentQuantileCauchyFarTail | 154.6985 ns | 2.9769 ns | 0.1632 ns |  3,791.93 |  136.69 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **KruskalWallisTest**         | **10000**      | **False** |  **1,484.9 μs** |    **59.12 μs** |   **3.24 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    421.1 μs |    49.79 μs |   2.73 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    613.8 μs |   117.92 μs |   6.46 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,223.3 μs |    38.11 μs |   2.09 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    710.4 μs |    29.63 μs |   1.62 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **620.1 μs** |     **8.73 μs** |   **0.48 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    245.1 μs |    35.89 μs |   1.97 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    521.9 μs |    30.69 μs |   1.68 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    790.6 μs |    16.95 μs |   0.93 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    393.1 μs |     5.55 μs |   0.30 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,846.1 μs** |   **938.42 μs** |  **51.44 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,520.3 μs |   117.90 μs |   6.46 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,660.0 μs |   400.38 μs |  21.95 μs | 640.6250 | 640.6250 | 640.6250 |  5062774 B |
| WilcoxonPooledDifferences | 100000     | False | 17,398.1 μs | 2,503.57 μs | 137.23 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,122.4 μs |   256.03 μs |  14.03 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,072.7 μs** |    **87.22 μs** |   **4.78 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,838.4 μs |   201.35 μs |  11.04 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,299.8 μs |   490.72 μs |  26.90 μs | 578.1250 | 578.1250 | 578.1250 |  5062733 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,677.5 μs | 1,743.56 μs |  95.57 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,763.9 μs |    79.41 μs |   4.35 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method | SampleSize | Covariance | Mean         | Error       | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-------------:|------------:|-----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |     **6.902 μs** |   **0.2502 μs** |  **0.0137 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |     **9.995 μs** |   **3.8078 μs** |  **0.2087 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc1**        |     **9.967 μs** |   **1.2980 μs** |  **0.0712 μs** |   **0.5341** |        **-** |        **-** |   **8.75 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.431 μs** |   **2.8350 μs** |  **0.1554 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.461 μs** |   **0.3627 μs** |  **0.0199 μs** |   **0.5798** |        **-** |        **-** |   **9.62 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **452.606 μs** |  **24.3880 μs** |  **1.3368 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **911.711 μs** |  **14.8746 μs** |  **0.8153 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **943.792 μs** | **399.4210 μs** | **21.8936 μs** | **124.0234** | **124.0234** | **124.0234** | **472.89 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,044.861 μs** | **216.5296 μs** | **11.8687 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,056.927 μs** |  **73.6290 μs** |  **4.0359 μs** | **123.0469** | **123.0469** | **123.0469** | **551.11 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.467 μs** | **0.5187 μs** | **0.0284 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.547 μs | 0.5569 μs | 0.0305 μs |  2.58 |    0.02 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.240 μs | 0.5154 μs | 0.0283 μs |  1.17 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.169 μs | 0.3492 μs | 0.0191 μs |  2.72 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.023 μs | 0.4510 μs | 0.0247 μs |  1.12 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.664 μs | 0.4756 μs | 0.0261 μs |  2.61 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **48.946 μs** | **0.6391 μs** | **0.0350 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 120.933 μs | 1.8771 μs | 0.1029 μs |  2.47 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.794 μs | 1.1045 μs | 0.0605 μs |  1.02 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.643 μs | 2.1429 μs | 0.1175 μs |  2.49 |    0.00 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.840 μs | 5.5184 μs | 0.3025 μs |  1.02 |    0.01 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 121.102 μs | 2.3553 μs | 0.1291 μs |  2.47 |    0.00 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                        | SampleSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.885 μs** |   **6.0130 μs** | **0.3296 μs** |  **1.00** |    **0.05** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  30.841 μs |   8.0446 μs | 0.4410 μs |  3.92 |    0.15 |   4.0283 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  25.323 μs |   5.7309 μs | 0.3141 μs |  3.22 |    0.12 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.864 μs |   0.1856 μs | 0.0102 μs |  0.49 |    0.02 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.039 μs |   0.5058 μs | 0.0277 μs |  0.51 |    0.02 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.568 μs |   0.9203 μs | 0.0504 μs |  0.20 |    0.01 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.903 μs |   0.8079 μs | 0.0443 μs |  0.37 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |             |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **116.205 μs** |  **12.4216 μs** | **0.6809 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 978.923 μs | 110.7417 μs | 6.0701 μs |  8.42 |    0.06 | 248.0469 | 248.0469 | 248.0469 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 292.130 μs |  26.1419 μs | 1.4329 μs |  2.51 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.806 μs |   3.4089 μs | 0.1869 μs |  0.82 |    0.00 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.841 μs |  13.7544 μs | 0.7539 μs |  0.84 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.132 μs |  13.9070 μs | 0.7623 μs |  0.12 |    0.01 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  28.112 μs |   6.8910 μs | 0.3777 μs |  0.24 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **1,140.1 ns** |      **51.60 ns** |      **2.83 ns** |    **1.00** |    **0.00** |   **0.0038** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,815.5 ns |   2,443.49 ns |    133.94 ns |   34.05 |    0.13 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,153.5 ns |      61.92 ns |      3.39 ns |    1.89 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     24,148.9 ns |   2,685.86 ns |    147.22 ns |   21.18 |    0.12 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        119.5 ns |      38.90 ns |      2.13 ns |    0.10 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        222.1 ns |       4.40 ns |      0.24 ns |    0.19 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **52,030.0 ns** |   **5,576.97 ns** |    **305.69 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    141,792.3 ns |  17,917.04 ns |    982.09 ns |   2.725 |    0.02 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    690,316.7 ns |   3,041.01 ns |    166.69 ns |  13.268 |    0.07 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 12,490,768.1 ns | 343,220.67 ns | 18,813.08 ns | 240.074 |    1.26 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        114.7 ns |       4.45 ns |      0.24 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        215.3 ns |      14.06 ns |      0.77 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

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

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,811.6 ns** |     **704.34 ns** |    **38.61 ns** |  **1.00** |    **0.02** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,915.5 ns |     431.48 ns |    23.65 ns |  1.39 |    0.02 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       607.0 ns |      84.86 ns |     4.65 ns |  0.22 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,155.2 ns |      41.22 ns |     2.26 ns |  0.41 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       812.5 ns |      48.34 ns |     2.65 ns |  0.29 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,610.7 ns |      93.23 ns |     5.11 ns |  1.64 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,588.2 ns |     312.59 ns |    17.13 ns |  3.05 |    0.04 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,316.1 ns |     397.99 ns |    21.82 ns |  2.25 |    0.03 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,336.5 ns |     385.11 ns |    21.11 ns |  1.54 |    0.02 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **373,272.0 ns** |  **22,845.24 ns** | **1,252.22 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,326,298.1 ns |  75,622.12 ns | 4,145.10 ns | 3.553 |    0.01 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,286.3 ns |     866.98 ns |    47.52 ns | 0.151 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,738.7 ns |   5,184.12 ns |   284.16 ns | 0.302 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,047.7 ns |     176.88 ns |     9.70 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   900,934.2 ns | 112,131.46 ns | 6,146.30 ns | 2.414 |    0.02 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,390,507.1 ns |  37,958.47 ns | 2,080.63 ns | 3.725 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,010.0 ns |     195.39 ns |    10.71 ns | 0.029 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **16.91 μs** |   **4.064 μs** |  **0.223 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **37.71 μs** |   **3.724 μs** |  **0.204 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **55.27 μs** |   **5.428 μs** |  **0.298 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **164.32 μs** |  **14.860 μs** |  **0.815 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **217.62 μs** |  **34.698 μs** |  **1.902 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **575.14 μs** | **201.466 μs** | **11.043 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **741.93 μs** |  **84.536 μs** |  **4.634 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,278.83 μs** | **239.297 μs** | **13.117 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **11.60 μs** |      **0.719 μs** |     **0.039 μs** |  **1.00** |    **0.00** |    **0.1984** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     19.65 μs |      3.483 μs |     0.191 μs |  1.69 |    0.02 |    3.6926 |    0.1831 |         - |    58.16 KB |       16.96 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **91.68 μs** |      **3.901 μs** |     **0.214 μs** |  **1.00** |    **0.00** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    124.51 μs |     10.087 μs |     0.553 μs |  1.36 |    0.01 |   32.9590 |   14.4043 |         - |   508.26 KB |       29.06 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **971.67 μs** |    **102.314 μs** |     **5.608 μs** |  **1.00** |    **0.01** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.51 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,517.71 μs |  2,063.153 μs |   113.088 μs |  1.56 |    0.10 | 1074.2188 | 1074.2188 | 1074.2188 |  5024.68 KB |       31.70 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,676.54 μs** |    **325.847 μs** |    **17.861 μs** |  **1.00** |    **0.00** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.47 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 20,329.24 μs | 26,605.045 μs | 1,458.312 μs |  2.10 |    0.13 |  937.5000 |  937.5000 |  937.5000 | 50025.66 KB |       31.98 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------------------------ |---------:|----------:|---------:|---------:|----------:|
| ChatTemplate            | 95.97 ms |  8.396 ms | 0.460 ms | 333.3333 |   33.3 MB |
| ProseWithoutAddedTokens | 79.84 ms | 13.983 ms | 0.766 ms | 285.7143 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,936.9 μs** |      **78.87 μs** |     **4.32 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    88,764.3 μs |   4,073.04 μs |   223.26 μs |  45.83 |    0.13 |  3666.6667 |  1500.0000 |  833.3333 |   59880.8 KB |       58.56 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,835.9 μs** |      **35.26 μs** |     **1.93 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    90,019.1 μs |  25,266.76 μs | 1,384.96 μs |  49.03 |    0.65 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **774.8 μs** |      **13.72 μs** |     **0.75 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   144,615.9 μs |  26,631.37 μs | 1,459.76 μs | 186.65 |    1.64 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,243.6 μs** |     **657.48 μs** |    **36.04 μs** |   **1.00** |    **0.02** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    78,116.0 μs |  55,512.64 μs | 3,042.84 μs |  34.82 |    1.27 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,747.4 μs** |   **3,668.46 μs** |   **201.08 μs** |   **1.00** |    **0.02** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,766,251.3 μs |  54,896.41 μs | 3,009.06 μs | 112.17 |    1.25 | 35000.0000 | 10000.0000 | 4000.0000 | 537149.55 KB |       60.18 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,294.9 μs** |   **1,818.43 μs** |    **99.67 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,668,401.9 μs | 134,594.78 μs | 7,377.59 μs | 116.72 |    0.84 | 48000.0000 | 12000.0000 | 4000.0000 | 747349.05 KB |       83.74 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,485.7 μs** |     **394.80 μs** |    **21.64 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,005,208.1 μs |  82,845.08 μs | 4,541.02 μs | 463.36 |    1.47 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.18 KB |    8,147.90 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,771.3 μs** |   **1,628.35 μs** |    **89.26 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,732,605.7 μs | 101,768.34 μs | 5,578.27 μs |  97.50 |    0.50 | 38000.0000 | 11000.0000 | 4000.0000 | 573618.52 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 26.05 ms |  5.661 ms | 0.310 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 47.53 ms | 12.061 ms | 0.661 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.93 MB |
| CountLoad4M      | 46.60 ms | 14.462 ms | 0.793 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

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

| Method                | Mean          | Error         | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 183,090.67 μs | 37,835.195 μs | 2,073.875 μs |         - |         - |        - | 54334.63 KB |
| CountSave240K         |     405.40 μs |     51.879 μs |     2.844 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     439.78 μs |    174.458 μs |     9.563 μs |   11.2305 |    4.8828 |   4.8828 |   739.44 KB |
| TfidfSave45K          |      48.04 μs |      1.402 μs |     0.077 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      50.55 μs |      8.308 μs |     0.455 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     290.00 μs |     47.116 μs |     2.583 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     299.58 μs |     37.957 μs |     2.081 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  26,773.25 μs | 37,501.387 μs | 2,055.578 μs |  593.7500 |  562.5000 | 187.5000 | 22048.77 KB |
| CountLoadAsync4M      |  58,952.36 μs | 14,978.324 μs |   821.013 μs | 1181.8182 | 1090.9091 | 363.6364 | 44983.59 KB |
| CountLoad4M           |  53,941.09 μs | 18,500.920 μs | 1,014.098 μs | 1181.8182 | 1090.9091 | 363.6364 | 44976.85 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

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

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     18.999 μs |    26.556 μs |   1.4556 μs |   1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    476.991 μs |    49.899 μs |   2.7352 μs |  36.1328 |  30.2734 |  30.2734 |   740.16 KB |
| HashingLoadUndeclared  |      5.122 μs |     4.156 μs |   0.2278 μs |   5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     27.413 μs |     2.699 μs |   0.1479 μs |   7.4158 |   0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |    123.864 μs |    16.298 μs |   0.8933 μs |  16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    862.067 μs |   334.358 μs |  18.3273 μs |  38.0859 |  36.1328 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 10,881.495 μs |   506.912 μs |  27.7855 μs | 500.0000 | 484.3750 | 281.2500 | 10793.96 KB |
| CountSaveAsync1M       |  2,529.657 μs |   317.971 μs |  17.4291 μs | 132.8125 | 125.0000 | 125.0000 |  3162.55 KB |
| IndexSaveAsyncLongHead |  3,348.908 μs |   710.458 μs |  38.9426 μs |  97.6563 |  93.7500 |  93.7500 |  2124.19 KB |
| CountLoadUndeclared2M  | 22,046.043 μs | 6,374.508 μs | 349.4083 μs | 875.0000 | 843.7500 | 468.7500 | 22045.22 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.646 μs** |   **0.2217 μs** | **0.0122 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.813 μs |   0.1822 μs | 0.0100 μs |  1.03 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   5.932 μs |   0.4276 μs | 0.0234 μs |  1.05 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **57.746 μs** |   **3.6557 μs** | **0.2004 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.062 μs |   1.5711 μs | 0.0861 μs |  0.36 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.943 μs |   2.4477 μs | 0.1342 μs |  0.38 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **217.982 μs** |   **6.4672 μs** | **0.3545 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  79.794 μs |   0.4331 μs | 0.0237 μs |  0.37 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  70.687 μs |   6.3115 μs | 0.3460 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **875.540 μs** | **127.1299 μs** | **6.9684 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 319.523 μs |  22.2336 μs | 1.2187 μs |  0.36 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 270.599 μs |  24.7936 μs | 1.3590 μs |  0.31 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|---------:|----------:|
| **Encode** | **Ascii**    | **False**     | **31.09 ms** |  **0.701 ms** | **0.038 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **34.94 ms** |  **4.173 ms** | **0.229 ms** | **133.3333** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **30.87 ms** |  **0.623 ms** | **0.034 ms** | **125.0000** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **44.38 ms** |  **4.787 ms** | **0.262 ms** | **250.0000** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **35.93 ms** | **10.049 ms** | **0.551 ms** | **266.6667** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **48.99 ms** |  **7.198 ms** | **0.395 ms** | **272.7273** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **133.49 ms** |  **6.856 ms** | **0.376 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.94 ms |  4.606 ms | 0.252 ms |  0.49 |    0.00 |  103.68 KB |        3.80 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **139.42 ms** | **72.462 ms** | **3.972 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  60.67 ms |  2.242 ms | 0.123 ms |  0.44 |    0.01 |  116.47 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **202.00 ms** |  **2.940 ms** | **0.161 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 242.96 ms |  7.001 ms | 0.384 ms |  1.20 |    0.00 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **205.55 ms** | **16.715 ms** | **0.916 ms** |  **1.00** |    **0.01** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 219.01 ms |  5.790 ms | 0.317 ms |  1.07 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **252.84 ms** |  **9.569 ms** | **0.525 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 343.56 ms | 19.193 ms | 1.052 ms |  1.36 |    0.00 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **258.25 ms** |  **3.259 ms** | **0.179 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 314.18 ms | 12.650 ms | 0.693 ms |  1.22 |    0.00 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **296.90 ms** |  **8.880 ms** | **0.487 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 399.10 ms | 27.395 ms | 1.502 ms |  1.34 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **302.07 ms** | **20.343 ms** | **1.115 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 391.37 ms | 29.679 ms | 1.627 ms |  1.30 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count | Mean     | Error     | StdDev   | Ratio | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |---------:|----------:|---------:|------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    | **79.49 ms** | **12.872 ms** | **0.706 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** |  **9386.09 KB** |       **1.000** |
| TreeNearest | 10    | 51.44 ms |  1.420 ms | 0.078 ms |  0.65 |         - |         - |         - |    29.67 KB |       0.003 |
|             |       |          |           |          |       |           |           |           |             |             |
| **FullScan**    | **2000**  | **81.81 ms** |  **5.709 ms** | **0.313 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** | **10007.93 KB** |        **1.00** |
| TreeNearest | 2000  | 82.82 ms |  3.899 ms | 0.214 ms |  1.01 |         - |         - |         - |  1273.52 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **44.74 μs** |     **0.095 μs** |   **0.005 μs** |         **-** |
| Cjk    | 1000   |      59.25 μs |     1.980 μs |   0.109 μs |         - |
| **Latin**  | **10000**  |   **4,139.74 μs** |   **507.277 μs** |  **27.806 μs** |         **-** |
| Cjk    | 10000  |   6,814.02 μs |   122.362 μs |   6.707 μs |         - |
| **Latin**  | **65536**  | **184,925.44 μs** | **8,056.895 μs** | **441.626 μs** |         **-** |

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

| Method  | Mean     | Error    | StdDev   | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|----------:|----------:|------------:|
| Unigram | 27.99 ms | 0.516 ms | 0.028 ms |  1.00 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 82.86 ms | 1.397 ms | 0.077 ms |  2.96 | 1714.2857 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **18.56 μs** |  **7.735 μs** | **0.424 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **65.68 μs** | **16.659 μs** | **0.913 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **177.73 μs** | **15.241 μs** | **0.835 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **379.69 μs** | **30.702 μs** | **1.683 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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
| EncodeUnseenProse | 9.136 ms | 5.274 ms | 0.2891 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean      | Error     | StdDev   | Allocated |
|----------- |--------- |----------:|----------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **18.82 μs** |  **0.456 μs** | **0.025 μs** |         **-** |
| MyersGroup | cjk      | 242.07 μs | 70.501 μs | 3.864 μs |         - |
| **DpGroup**    | **latin**    |  **10.39 μs** |  **3.900 μs** | **0.214 μs** |         **-** |
| MyersGroup | latin    | 132.57 μs |  8.492 μs | 0.465 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **311.72 μs** |  **1.805 μs** | **0.099 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 304.72 μs | 47.467 μs | 2.602 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **336.89 μs** | **20.345 μs** | **1.115 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 306.79 μs | 18.551 μs | 1.017 μs |  0.91 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **317.34 μs** | **29.568 μs** | **1.621 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 311.73 μs | 66.073 μs | 3.622 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **269.35 μs** | **14.347 μs** | **0.786 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 320.81 μs | 61.353 μs | 3.363 μs |  1.19 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **298.84 μs** | **55.234 μs** | **3.028 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 307.35 μs | 49.847 μs | 2.732 μs |  1.03 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **164.72 μs** | **30.976 μs** | **1.698 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 305.11 μs |  7.302 μs | 0.400 μs |  1.85 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **231.28 μs** | **59.317 μs** | **3.251 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 312.52 μs | 52.517 μs | 2.879 μs |  1.35 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **86.66 μs** | **31.090 μs** | **1.704 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 317.51 μs | 16.260 μs | 0.891 μs |  3.66 |    0.06 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **183.6 μs** |   **6.94 μs** |  **0.38 μs** |  **1.00** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   168.1 μs |   6.93 μs |  0.38 μs |  0.92 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   471.6 μs |   9.75 μs |  0.53 μs |  2.57 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,240.8 μs** | **188.48 μs** | **10.33 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,108.5 μs | 132.69 μs |  7.27 μs |  0.96 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,531.5 μs | 116.14 μs |  6.37 μs |  2.32 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error        | StdDev      | Gen0   | Allocated |
|------- |-------- |-------------:|-------------:|------------:|-------:|----------:|
| **Report** | **10**      |     **516.7 ns** |     **36.30 ns** |     **1.99 ns** | **0.0200** |   **1.66 KB** |
| **Report** | **1000**    | **310,012.8 ns** | **21,429.61 ns** | **1,174.63 ns** | **1.4648** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| Roc             | 1000000 | 138.1 ms | 33.03 ms | 1.81 ms |  750.0000 |  750.0000 |  750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 125.6 ms | 25.78 ms | 1.41 ms | 1000.0000 | 1000.0000 | 1000.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.151 ms** | **0.4981 ms** | **0.0273 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.101 ms | 0.0558 ms | 0.0031 ms |        - |        - |        - |   12.06 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.397 ms | 0.3604 ms | 0.0198 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.010 ms** | **0.2458 ms** | **0.0135 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.183 ms | 0.2229 ms | 0.0122 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.130 ms | 1.2055 ms | 0.0661 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method              | Length | Mean            | Error           | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|----------------:|-------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **906.5 ns** |        **92.72 ns** |      **5.08 ns** |  **1.00** |    **0.01** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,846.9 ns |       201.49 ns |     11.04 ns |  2.04 |    0.01 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |              |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **66,082.9 ns** |    **24,802.40 ns** |  **1,359.50 ns** |  **1.00** |    **0.03** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,919.7 ns |     6,745.94 ns |    369.77 ns |  2.30 |    0.04 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |              |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,364,285.2 ns** | **1,747,160.28 ns** | **95,767.75 ns** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 11,614,297.0 ns |   538,317.47 ns | 29,507.00 ns |  2.66 |    0.05 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

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

| Method       | Shape     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **157.68 ms** | **33.241 ms** | **1.822 ms** |  **1.00** |    **0.01** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,522.94 ms |  9.343 ms | 0.512 ms | 16.00 |    0.16 | 14000.0000 | 8000.0000 | 7000.0000 |  156.3 MB |        5.31 |
|              |           |             |           |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **57.36 ms** | **23.627 ms** | **1.295 ms** |  **1.00** |    **0.03** |  **1444.4444** | **1444.4444** | **1444.4444** |  **13.95 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   840.44 ms | 38.829 ms | 2.128 ms | 14.66 |    0.29 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **177.96 ms** |  **60.593 ms** |  **3.321 ms** |  **1.00** |    **0.02** |  **1333.3333** |  **1333.3333** |  **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,729.97 ms | 205.330 ms | 11.255 ms | 37.83 |    0.61 | 38000.0000 | 12000.0000 | 11000.0000 | 531.87 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,937.89 ms | 146.696 ms |  8.041 ms | 22.13 |    0.36 | 22000.0000 | 12000.0000 | 10000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.04 ms** |   **1.248 ms** |  **0.068 ms** |  **1.00** |    **0.00** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   476.14 ms |  40.486 ms |  2.219 ms | 22.63 |    0.11 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   263.40 ms |  20.405 ms |  1.118 ms | 12.52 |    0.06 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  12.847 ms |  1.0341 ms | 0.0567 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 108.139 ms | 27.8378 ms | 1.5259 ms |  8.42 |    0.11 |
| Nmf_Transform                             |   2.929 ms |  0.2823 ms | 0.0155 ms |  0.23 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.002 ms |  0.6804 ms | 0.0373 ms |  1.87 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **19,732.5 μs** |   **4,278.16 μs** |   **234.50 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **11692.44 KB** |
| TruncatedSvd_Transform     | 500     |     529.6 μs |      33.77 μs |     1.85 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 183,847.5 μs | 111,613.03 μs | 6,117.89 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18141.12 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 204,194.7 μs |  62,079.69 μs | 3,402.80 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **170,230.1 μs** |  **28,277.98 μs** | **1,550.01 μs** | **3333.3333** | **3333.3333** | **3333.3333** | **115064.01 KB** |
| TruncatedSvd_Transform     | 20000   |     908.0 μs |     119.36 μs |     6.54 μs |   43.9453 |   43.9453 |   43.9453 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 781,012.0 μs |  65,690.49 μs | 3,600.72 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.81 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 559,905.3 μs |  62,451.92 μs | 3,423.20 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 283.7 μs | 66.14 μs | 3.63 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **SearchTop10** | **10000**  |   **552.8 μs** | **196.09 μs** | **10.75 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,472.5 μs** |  **92.15 μs** |  **5.05 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Rows   | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|-------------------- |------- |------------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| **FromBlockNormalized** | **1000**   |    **703.8 μs** |   **124.1 μs** |   **6.80 μs** | **249.0234** | **249.0234** | **249.0234** |  **1500.18 KB** |
| MmrSelect100        | 1000   |  6,299.2 μs |   513.3 μs |  28.13 μs |        - |        - |        - |    24.93 KB |
| **FromBlockNormalized** | **100000** | **79,968.9 μs** | **7,927.3 μs** | **434.52 μs** | **166.6667** | **166.6667** | **166.6667** | **150000.3 KB** |
| MmrSelect100        | 100000 |  6,068.8 μs |   617.1 μs |  33.82 μs |        - |        - |        - |    24.93 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-KBRPOB : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-KBRPOB** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **277.96 μs** |    **14.569 μs** |    **16.194 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    273.38 μs |    19.144 μs |    21.279 μs |  0.99 |    0.09 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    106.06 μs |     7.236 μs |     8.043 μs |  0.38 |    0.04 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |    428.10 μs |    24.216 μs |    26.916 μs |  1.54 |    0.13 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,637.27 μs |   116.377 μs |   129.352 μs |  5.91 |    0.56 |        - |        - |        - |  339.67 KB |        2.04 |
| MlNet_ReplaceMissingValues_Read | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,243.28 μs |    34.844 μs |    38.729 μs |  4.49 |    0.28 |        - |        - |        - |   312.3 KB |        1.87 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    173.69 μs |    39.506 μs |     2.165 μs |  1.00 |    0.02 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     70.07 μs |    11.188 μs |     0.613 μs |  0.40 |    0.01 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     17.89 μs |     1.682 μs |     0.092 μs |  0.10 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    968.94 μs | 2,502.907 μs |   137.193 μs |  5.58 |    0.69 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    939.05 μs |   652.935 μs |    35.790 μs |  5.41 |    0.19 |  48.8281 |   7.8125 |        - |  796.77 KB |        4.80 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    257.58 μs |    48.024 μs |     2.632 μs |  1.48 |    0.02 |  19.0430 |  18.5547 |   0.4883 |  295.94 KB |        1.78 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-KBRPOB** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **6,137.53 μs** |   **814.614 μs** |   **871.627 μs** |  **1.02** |    **0.21** |        **-** |        **-** |        **-** |  **3283.7 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,764.37 μs |   472.845 μs |   485.577 μs |  0.79 |    0.14 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |    702.37 μs |     9.926 μs |    11.431 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,459.68 μs |    41.332 μs |    45.941 μs |  0.74 |    0.11 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,757.43 μs | 6,257.248 μs | 7,205.856 μs |  2.29 |    1.23 |        - |        - |        - |  442.19 KB |        0.13 |
| MlNet_ReplaceMissingValues_Read | Job-KBRPOB | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,990.46 μs | 1,707.029 μs | 1,826.502 μs |  1.99 |    0.42 |        - |        - |        - |  522.03 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,521.77 μs |   755.776 μs |    41.427 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3291.66 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,216.24 μs |   635.609 μs |    34.840 μs |  0.91 |    0.01 |  89.8438 |  89.8438 |  89.8438 |   314.4 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    423.14 μs |    60.728 μs |     3.329 μs |  0.12 |    0.00 | 126.4648 | 126.4648 | 126.4648 |  626.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,549.77 μs | 1,212.673 μs |    66.471 μs |  0.44 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,257.62 μs | 1,180.101 μs |    64.685 μs |  1.21 |    0.02 |  31.2500 |  15.6250 |        - |  567.08 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,459.06 μs |   275.208 μs |    15.085 μs |  0.70 |    0.01 |  27.3438 |  11.7188 |        - |  457.22 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Records | Mean        | Error        | StdDev      | Gen0   | Allocated  |
|---------------- |-------- |------------:|-------------:|------------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **528.8 μs** |     **59.55 μs** |     **3.26 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    609.9 μs |     15.29 μs |     0.84 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    682.9 μs |     47.29 μs |     2.59 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,540.6 μs |  1,162.60 μs |    63.73 μs | 1.9531 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **8,840.5 μs** | **18,528.64 μs** | **1,015.62 μs** |      **-** |    **7.18 KB** |
| Unfiltered      | 100000  | 16,043.5 μs | 21,970.27 μs | 1,204.26 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 16,524.2 μs |  5,732.74 μs |   314.23 μs |      - |   15.16 KB |
| HybridBroad     | 100000  | 25,926.9 μs |  1,139.85 μs |    62.48 μs |      - | 3155.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.04 ns |   2.214 ns |  0.121 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   745.95 ns |  65.403 ns |  3.585 ns |  7.69 |    0.03 |      - |         - |          NA |
| TokenSortRatio |   862.70 ns |  48.985 ns |  2.685 ns |  8.89 |    0.03 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   991.08 ns | 201.501 ns | 11.045 ns | 10.21 |    0.10 | 0.0534 |     896 B |          NA |
| WRatio         | 1,259.56 ns |  57.904 ns |  3.174 ns | 12.98 |    0.03 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,024,127.1211 ns |   119,691.7868 ns |   6,560.7107 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    660,653.8698 ns |    37,207.4879 ns |   2,039.4680 ns |  0.326 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,091,535.6536 ns |    47,448.4116 ns |   2,600.8076 ns |  0.539 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,689,598.5000 ns | 7,936,006.8197 ns | 434,999.3144 ns | 34.430 |    0.21 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean         | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **98.49 ns** |     **7.271 ns** |  **0.399 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    205.22 ns |     5.977 ns |  0.328 ns |  2.08 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **761.03 ns** |    **47.707 ns** |  **2.615 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,059.43 ns |   403.389 ns | 22.111 ns | 13.22 |    0.05 |      - |     160 B |          NA |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **924.17 ns** |    **68.572 ns** |  **3.759 ns** |  **1.00** |    **0.00** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,097.95 ns | 1,039.258 ns | 56.965 ns |  2.27 |    0.05 | 0.1144 |    1944 B |        2.17 |
|            |               |              |              |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,190.53 ns** |   **237.343 ns** | **13.010 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,880.18 ns |   200.343 ns | 10.981 ns |  4.10 |    0.04 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.503 ms** | **0.9667 ms** | **0.0530 ms** | **253.9063** | **251.9531** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **25.289 ms** | **3.5004 ms** | **0.1919 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.34 ns** |   **0.506 ns** |  **0.028 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.45 ns |   1.377 ns |  0.075 ns |  1.57 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.40 ns |   0.134 ns |  0.007 ns |  1.04 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.28 ns |   0.091 ns |  0.005 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **28.20 ns** |   **1.169 ns** |  **0.064 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    43.53 ns |  26.714 ns |  1.464 ns |  1.54 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.69 ns |   0.271 ns |  0.015 ns |  1.02 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.85 ns |   0.353 ns |  0.019 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **30.82 ns** |   **3.326 ns** |  **0.182 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.52 ns |   0.198 ns |  0.011 ns |  1.48 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    31.51 ns |   0.914 ns |  0.050 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.27 ns |   1.690 ns |  0.093 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **45.22 ns** |  **15.266 ns** |  **0.837 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    50.85 ns |  15.137 ns |  0.830 ns |  1.12 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.46 ns |   1.112 ns |  0.061 ns |  0.78 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    33.95 ns |   0.283 ns |  0.016 ns |  0.75 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **58.26 ns** |   **3.473 ns** |  **0.190 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.92 ns |   1.068 ns |  0.059 ns |  1.05 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.28 ns |   1.893 ns |  0.104 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    60.28 ns |   9.007 ns |  0.494 ns |  1.03 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **62.58 ns** |   **3.682 ns** |  **0.202 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    70.51 ns |   5.804 ns |  0.318 ns |  1.13 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    65.77 ns |   0.433 ns |  0.024 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    61.66 ns |   0.744 ns |  0.041 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **405.96 ns** |  **22.062 ns** |  **1.209 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   416.96 ns |   1.226 ns |  0.067 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   406.62 ns |   7.823 ns |  0.429 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   412.79 ns |  22.896 ns |  1.255 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,622.30 ns** | **332.710 ns** | **18.237 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,717.91 ns |  26.292 ns |  1.441 ns |  1.02 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,669.73 ns |  19.068 ns |  1.045 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,619.95 ns | 105.635 ns |  5.790 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **405.2 ns** |    **13.83 ns** |   **0.76 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    192.8 ns |    62.52 ns |   3.43 ns |  0.48 |    0.01 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,464.5 ns** |    **67.55 ns** |   **3.70 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,039.0 ns |    10.88 ns |   0.60 ns |  1.23 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,572.4 ns** |   **769.99 ns** |  **42.21 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32,265.7 ns | 9,043.15 ns | 495.69 ns |  2.38 |    0.03 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,643.3 ns** |   **190.27 ns** |  **10.43 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  8,430.7 ns |   298.37 ns |  16.35 ns |  0.98 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,576.6 ns |   778.50 ns |  42.67 ns |  1.34 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,897.0 ns** | **4,382.71 ns** | **240.23 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,516.8 ns | 3,110.69 ns | 170.51 ns |  0.96 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,108.2 ns |   709.30 ns |  38.88 ns |  1.43 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,729.1 ns** | **1,821.92 ns** |  **99.87 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,676.1 ns | 1,057.44 ns |  57.96 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 17,909.2 ns | 2,028.72 ns | 111.20 ns |  1.41 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,382.7 ns** | **2,239.28 ns** | **122.74 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,921.7 ns | 2,674.41 ns | 146.59 ns |  1.10 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,172.2 ns | 1,034.85 ns |  56.72 ns |  1.70 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **6,930.8 ns** | **3,937.40 ns** | **215.82 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,932.8 ns | 1,368.57 ns |  75.02 ns |  1.00 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,153.4 ns | 1,391.94 ns |  76.30 ns |  1.61 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,169.0 ns** | **2,163.07 ns** | **118.57 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,644.1 ns | 1,809.56 ns |  99.19 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,174.3 ns | 3,154.53 ns | 172.91 ns |  1.66 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,754.4 ns** |   **422.63 ns** |  **23.17 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,093.4 ns |   188.36 ns |  10.32 ns |  1.09 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,164.1 ns |    33.06 ns |   1.81 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,340.0 ns** | **1,039.97 ns** |  **57.00 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,396.9 ns |   718.66 ns |  39.39 ns |  1.01 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,885.6 ns |   143.51 ns |   7.87 ns |  1.85 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,846.7 ns** | **2,042.43 ns** | **111.95 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,473.6 ns | 1,697.76 ns |  93.06 ns |  0.95 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,814.1 ns |   714.09 ns |  39.14 ns |  1.76 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,303.9 ns** |    **35.88 ns** |   **1.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,136.4 ns |    69.17 ns |   3.79 ns |  0.93 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,890.5 ns |   646.13 ns |  35.42 ns |  2.56 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,081.5 ns** |    **65.13 ns** |   **3.57 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,870.5 ns |   331.30 ns |  18.16 ns |  0.70 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,717.2 ns | 5,592.68 ns | 306.55 ns |  2.14 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,434.6 ns** |    **44.82 ns** |   **2.46 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,828.8 ns |   201.81 ns |  11.06 ns |  0.60 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,306.9 ns | 1,588.44 ns |  87.07 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,559.9 ns** |    **84.46 ns** |   **4.63 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,416.0 ns |   124.94 ns |   6.85 ns |  0.96 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,063.9 ns |   348.27 ns |  19.09 ns |  1.98 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,347.9 ns** |   **248.06 ns** |  **13.60 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,152.9 ns |   214.24 ns |  11.74 ns |  0.78 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,871.7 ns | 1,180.21 ns |  64.69 ns |  2.03 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,560.8 ns** |   **436.21 ns** |  **23.91 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,279.8 ns |   408.96 ns |  22.42 ns |  1.10 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,018.4 ns | 1,158.89 ns |  63.52 ns |  1.85 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,297.9 ns** |    **19.80 ns** |   **1.09 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    602.8 ns |   225.70 ns |  12.37 ns |  0.46 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,969.8 ns |   517.20 ns |  28.35 ns |  3.83 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,066.4 ns** |    **41.07 ns** |   **2.25 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,432.9 ns |   119.58 ns |   6.55 ns |  0.47 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,490.4 ns |   345.73 ns |  18.95 ns |  2.44 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,454.2 ns** |    **72.37 ns** |   **3.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,390.4 ns |   121.98 ns |   6.69 ns |  0.44 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 11,997.0 ns |   495.54 ns |  27.16 ns |  2.20 |    0.00 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

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

| Method           | Shape       | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,336.5 μs** |     **559.21 μs** |    **30.65 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.91 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,941.7 μs |     799.26 μs |    43.81 μs |  7.30 |    0.04 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  82,977.3 μs |   2,734.30 μs |   149.88 μs | 15.55 |    0.08 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **798.2 μs** |      **71.05 μs** |     **3.89 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,766.5 μs |      34.62 μs |     1.90 μs |  5.97 |    0.03 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,041.6 μs |     142.83 μs |     7.83 μs |  3.81 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,859.4 μs** |   **1,144.60 μs** |    **62.74 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 443,866.9 μs | 116,764.53 μs | 6,400.26 μs | 16.53 |    0.21 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 734,574.3 μs |   8,247.68 μs |   452.08 μs | 27.35 |    0.06 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.900 ms** | **0.5552 ms** | **0.0304 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.656 ms | 0.2677 ms | 0.0147 ms |  3.40 |    0.02 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **25.666 ms** | **2.8793 ms** | **0.1578 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.211 ms | 1.9198 ms | 0.1052 ms |  4.41 |    0.02 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.189 ms** | **1.3412 ms** | **0.0735 ms** |  **1.00** |    **0.00** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 210.909 ms | 1.8022 ms | 0.0988 ms |  4.57 |    0.01 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.68 ns** |      **0.899 ns** |   **0.049 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     56.22 ns |      0.266 ns |   0.015 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    130.86 ns |      6.440 ns |   0.353 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     98.66 ns |     13.421 ns |   0.736 ns |  0.75 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **216.21 ns** |     **14.409 ns** |   **0.790 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     62.71 ns |      0.883 ns |   0.048 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    231.11 ns |    179.782 ns |   9.854 ns |  1.07 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    110.50 ns |      5.821 ns |   0.319 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **290.76 ns** |    **206.012 ns** |  **11.292 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 14   |     67.34 ns |      2.196 ns |   0.120 ns |  0.23 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    271.93 ns |    173.003 ns |   9.483 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 14   |    114.84 ns |      0.357 ns |   0.020 ns |  0.40 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **357.72 ns** |     **69.521 ns** |   **3.811 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |     79.87 ns |      3.156 ns |   0.173 ns |  0.22 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    360.14 ns |     56.434 ns |   3.093 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    119.32 ns |      6.416 ns |   0.352 ns |  0.33 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **443.08 ns** |    **164.054 ns** |   **8.992 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 18   |     73.79 ns |      1.681 ns |   0.092 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    444.97 ns |    145.161 ns |   7.957 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 18   |    123.82 ns |      9.956 ns |   0.546 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **763.06 ns** |    **293.969 ns** |  **16.113 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 20   |     79.85 ns |      0.904 ns |   0.050 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    772.93 ns |      7.363 ns |   0.404 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 20   |    127.86 ns |      8.241 ns |   0.452 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **990.15 ns** |    **192.110 ns** |  **10.530 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     86.43 ns |      5.700 ns |   0.312 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    987.78 ns |    105.550 ns |   5.786 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    142.07 ns |      2.126 ns |   0.117 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,530.99 ns** |    **142.968 ns** |   **7.837 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    105.93 ns |      2.790 ns |   0.153 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,529.09 ns |     67.476 ns |   3.699 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    167.30 ns |      0.498 ns |   0.027 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,133.27 ns** |    **945.865 ns** |  **51.846 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    133.43 ns |      7.264 ns |   0.398 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,146.57 ns |    922.000 ns |  50.538 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    241.87 ns |      5.305 ns |   0.291 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,203.00 ns** |    **639.832 ns** |  **35.071 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    164.31 ns |     28.738 ns |   1.575 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,478.77 ns |    337.934 ns |  18.523 ns |  1.05 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    291.42 ns |     12.142 ns |   0.666 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **12,220.97 ns** | **10,979.447 ns** | **601.821 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 96   |    353.01 ns |      1.554 ns |   0.085 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,508.44 ns |    523.844 ns |  28.714 ns |  0.94 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |    957.87 ns |    137.460 ns |   7.535 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **25.21 ns** |     **0.288 ns** |   **0.016 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.74 ns |     0.105 ns |   0.006 ns |  0.98 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    119.82 ns |     1.556 ns |   0.085 ns |  4.75 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     24.87 ns |     0.358 ns |   0.020 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **276.34 ns** |    **15.513 ns** |   **0.850 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    367.33 ns |     9.568 ns |   0.524 ns |  1.33 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    675.49 ns |     8.013 ns |   0.439 ns |  2.44 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    278.46 ns |    46.428 ns |   2.545 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **920.61 ns** |    **51.826 ns** |   **2.841 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,895.61 ns |   286.282 ns |  15.692 ns |  2.06 |    0.02 |         - |          NA |
| Distance_CodePoint         | 128    |  1,626.98 ns |    15.189 ns |   0.833 ns |  1.77 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    922.28 ns |    45.450 ns |   2.491 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **10,990.18 ns** |   **332.505 ns** |  **18.226 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,741.63 ns | 4,913.356 ns | 269.318 ns |  1.52 |    0.02 |         - |          NA |
| Distance_CodePoint         | 512    | 13,425.87 ns |   705.711 ns |  38.682 ns |  1.22 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 11,013.60 ns |    31.729 ns |   1.739 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **343.9 ns** |      **2.32 ns** |     **0.13 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     239.5 ns |     39.35 ns |     2.16 ns |  0.70 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **349.9 ns** |     **22.95 ns** |     **1.26 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     241.7 ns |      4.34 ns |     0.24 ns |  0.69 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **433.2 ns** |     **35.98 ns** |     **1.97 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.2 ns |     18.18 ns |     1.00 ns |  0.74 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **424.2 ns** |     **18.89 ns** |     **1.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     326.5 ns |      8.98 ns |     0.49 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **509.4 ns** |     **16.75 ns** |     **0.92 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     408.7 ns |     16.04 ns |     0.88 ns |  0.80 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **520.6 ns** |     **12.57 ns** |     **0.69 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     416.4 ns |      9.83 ns |     0.54 ns |  0.80 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **612.0 ns** |     **56.91 ns** |     **3.12 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,301.2 ns |    152.23 ns |     8.34 ns |  2.13 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **616.5 ns** |     **11.88 ns** |     **0.65 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,302.6 ns |    104.03 ns |     5.70 ns |  2.11 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,910.0 ns** |    **107.22 ns** |     **5.88 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,569.2 ns |    876.38 ns |    48.04 ns |  2.92 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,955.9 ns** |     **84.09 ns** |     **4.61 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,484.7 ns |    120.15 ns |     6.59 ns |  2.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,756.9 ns** |  **1,342.79 ns** |    **73.60 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,529.6 ns |  2,151.47 ns |   117.93 ns |  3.90 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **425,674.1 ns** | **84,819.72 ns** | **4,649.26 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  65,355.6 ns |  5,202.80 ns |   285.18 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.59 ns** |      **1.252 ns** |     **0.069 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      81.45 ns |     10.008 ns |     0.549 ns |  3.18 |    0.02 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.15 ns |      0.363 ns |     0.020 ns |  3.17 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     167.96 ns |     41.344 ns |     2.266 ns |  6.56 |    0.08 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.19 ns** |      **1.703 ns** |     **0.093 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,265.04 ns |    758.766 ns |    41.591 ns | 22.68 |    0.13 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,352.97 ns |     22.681 ns |     1.243 ns |  4.90 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,602.03 ns |  1,675.068 ns |    91.816 ns | 38.39 |    0.29 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,042.69 ns** |     **87.865 ns** |     **4.816 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 432,918.25 ns |  9,127.726 ns |   500.321 ns | 39.20 |    0.04 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  36,939.57 ns |  5,038.080 ns |   276.154 ns |  3.35 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 712,764.54 ns | 42,115.184 ns | 2,308.475 ns | 64.55 |    0.18 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

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

| Method            | Labels | Mean     | Error    | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|---------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **599.2 μs** | **45.18 μs** | **2.48 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 640.1 μs | 63.09 μs | 3.46 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 599.0 μs | 24.10 μs | 1.32 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **775.0 μs** | **23.60 μs** | **1.29 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 771.2 μs | 11.38 μs | 0.62 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 916.9 μs | 74.94 μs | 4.11 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

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

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.06 μs** |     **20.291 μs** |     **1.112 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 132,112.72 μs | 57,384.657 μs | 3,145.447 μs | 992.93 |   21.70 | 750.0000 | 750.0000 | 750.0000 | 31414.41 KB |   12,805.87 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,910.55 μs** |    **276.891 μs** |    **15.177 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 591,889.75 μs | 13,035.510 μs |   714.520 μs | 203.36 |    0.94 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.47 μs** |      **2.769 μs** |     **0.152 μs** |   **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     868.85 μs |     56.429 μs |     3.093 μs |  38.66 |    0.25 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

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

| Method         | Samples | Classes | Mean           | Error         | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|--------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,472.9 ns** |     **132.44 ns** |      **7.26 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,423.0 ns |     155.60 ns |      8.53 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       109.9 ns |      24.57 ns |      1.35 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,599.3 ns |     379.02 ns |     20.78 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,389.8 ns |      34.30 ns |      1.88 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,969.2 ns** |     **888.44 ns** |     **48.70 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,401.9 ns |     249.26 ns |     13.66 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       108.4 ns |      13.21 ns |      0.72 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,167.8 ns |   1,330.36 ns |     72.92 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,602.4 ns |   1,244.33 ns |     68.21 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **470,304.4 ns** |  **66,952.04 ns** |  **3,669.87 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   820,923.1 ns |  20,646.05 ns |  1,131.68 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,291.6 ns |     656.64 ns |     35.99 ns |      - |         - |
| F1Macro        | 100000  | 2       |   461,049.0 ns |   9,576.61 ns |    524.93 ns |      - |     656 B |
| Report         | 100000  | 2       |   464,497.8 ns |  15,826.85 ns |    867.52 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **493,893.4 ns** |  **16,097.62 ns** |    **882.36 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   937,911.7 ns |  27,787.13 ns |  1,523.11 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    11,064.1 ns |     288.62 ns |     15.82 ns |      - |         - |
| F1Macro        | 100000  | 10      |   511,606.6 ns |  39,009.09 ns |  2,138.22 ns |      - |    1881 B |
| Report         | 100000  | 10      |   501,716.4 ns |  20,077.42 ns |  1,100.51 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,782,135.3 ns** | **295,879.11 ns** | **16,218.13 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,197,178.0 ns | 586,833.23 ns | 32,166.31 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   121,041.4 ns |   6,988.31 ns |    383.05 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,624,993.0 ns | 236,093.66 ns | 12,941.09 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,791,481.2 ns | 501,878.73 ns | 27,509.67 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,941,134.0 ns** | **524,578.08 ns** | **28,753.89 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,432,637.5 ns | 377,141.51 ns | 20,672.40 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,852.9 ns |   9,891.70 ns |    542.20 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 5,076,182.1 ns |  56,493.01 ns |  3,096.57 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,061,718.6 ns |  47,164.79 ns |  2,585.26 ns |      - |   12878 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **9,865.54 μs** |    **829.525 μs** |    **45.469 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,254.71 μs |  1,726.505 μs |    94.636 μs |     3.47 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.16 μs** |      **3.428 μs** |     **0.188 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,699.06 μs |    477.558 μs |    26.177 μs | 3,109.69 |   44.95 | 600.0000 | 600.0000 | 600.0000 |  5089266 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **137,081.11 μs** | **38,328.599 μs** | **2,100.920 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 225,981.92 μs | 51,115.242 μs | 2,801.799 μs |     1.65 |    0.03 |        - |        - |        - | 23228104 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **110.13 μs** |      **7.709 μs** |     **0.423 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 249,811.88 μs |  9,347.541 μs |   512.370 μs | 2,268.27 |    8.53 |        - |        - |        - | 23229116 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean        | Error      | StdDev    | Allocated |
|---------- |-------- |------------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **40.29 ms** |   **2.225 ms** |  **0.122 ms** |         **-** |
| OneVsOne  | 100000  |    81.25 ms |  11.097 ms |  0.608 ms |  803417 B |
| **OneVsRest** | **1000000** |   **786.96 ms** | **491.069 ms** | **26.917 ms** |         **-** |
| OneVsOne  | 1000000 | 1,331.57 ms | 222.724 ms | 12.208 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| PerLabel         | 100000 |  7.582 ms |  0.6259 ms | 0.0343 ms |        - |        - |        - |     7.85 KB |
| PerLabelWeighted | 100000 | 10.720 ms |  3.3214 ms | 0.1821 ms |        - |        - |        - |     7.85 KB |
| PerSample        | 100000 | 82.249 ms | 88.4704 ms | 4.8494 ms | 571.4286 | 428.5714 | 142.8571 | 39062.72 KB |
| PerClass         | 100000 |  4.327 ms |  0.3258 ms | 0.0179 ms |        - |        - |        - |     7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **72.94 ns** |     **1.545 ns** |   **0.085 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.39 ns |     4.274 ns |   0.234 ns |  1.01 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     73.23 ns |     1.871 ns |   0.103 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     73.16 ns |     1.655 ns |   0.091 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **105.66 ns** |    **33.781 ns** |   **1.852 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 6    |     89.34 ns |     1.822 ns |   0.100 ns |  0.85 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    105.07 ns |     3.782 ns |   0.207 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    132.52 ns |    34.200 ns |   1.875 ns |  1.25 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **156.83 ns** |     **4.055 ns** |   **0.222 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.11 ns |     1.760 ns |   0.096 ns |  0.65 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.69 ns |    17.405 ns |   0.954 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    180.54 ns |     3.244 ns |   0.178 ns |  1.15 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **197.91 ns** |    **48.699 ns** |   **2.669 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 10   |    108.07 ns |     2.330 ns |   0.128 ns |  0.55 |    0.01 |         - |          NA |
| Dp_Cjk     | 10   |    196.45 ns |     2.029 ns |   0.111 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    153.68 ns |     2.462 ns |   0.135 ns |  0.78 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **253.61 ns** |   **217.199 ns** |  **11.905 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 12   |    117.58 ns |     1.562 ns |   0.086 ns |  0.46 |    0.02 |         - |          NA |
| Dp_Cjk     | 12   |    245.40 ns |     3.712 ns |   0.203 ns |  0.97 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    161.13 ns |     2.260 ns |   0.124 ns |  0.64 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **412.66 ns** |   **121.372 ns** |   **6.653 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 16   |    131.54 ns |     4.777 ns |   0.262 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    412.70 ns |    76.351 ns |   4.185 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 16   |    180.97 ns |    10.329 ns |   0.566 ns |  0.44 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **844.79 ns** |    **50.174 ns** |   **2.750 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    169.26 ns |     6.025 ns |   0.330 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    843.01 ns |    14.213 ns |   0.779 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    228.29 ns |     1.560 ns |   0.085 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,450.89 ns** |   **189.180 ns** |  **10.370 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    207.89 ns |    75.135 ns |   4.118 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,451.95 ns |    40.123 ns |   2.199 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    268.44 ns |     1.441 ns |   0.079 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,593.05 ns** | **2,493.081 ns** | **136.654 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 48   |    277.56 ns |    21.424 ns |   1.174 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,305.43 ns |    68.328 ns |   3.745 ns |  0.92 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    368.39 ns |     2.080 ns |   0.114 ns |  0.10 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,252.78 ns** | **1,191.209 ns** |  **65.294 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    345.76 ns |     7.306 ns |   0.400 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  7,032.77 ns | 2,722.731 ns | 149.242 ns |  1.12 |    0.02 |         - |          NA |
| Kernel_Cjk | 64   |    449.74 ns |     6.505 ns |   0.357 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,251.54 ns** | **7,942.876 ns** | **435.376 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 96   |    743.77 ns |    45.601 ns |   2.500 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,480.08 ns | 8,351.647 ns | 457.782 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |  1,545.71 ns |   412.925 ns |  22.634 ns |  0.11 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Distance_Utf16**     | **8**      |     **38.38 ns** |      **0.742 ns** |   **0.041 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    125.90 ns |     15.844 ns |   0.868 ns |  3.28 |    0.02 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **144.36 ns** |     **10.205 ns** |   **0.559 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,484.10 ns |     51.951 ns |   2.848 ns | 10.28 |    0.04 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **324.54 ns** |     **48.453 ns** |   **2.656 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,391.47 ns |  2,616.182 ns | 143.402 ns | 28.94 |    0.43 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **47,636.89 ns** | **16,491.392 ns** | **903.949 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 49,364.60 ns |  1,925.474 ns | 105.542 ns |  1.04 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **206.13 μs** |  **10.539 μs** |  **0.578 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   227.50 μs |  13.494 μs |  0.740 μs |  1.10 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    58.62 μs |   1.722 μs |  0.094 μs |  0.28 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   198.15 μs |  13.330 μs |  0.731 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **207.00 μs** |  **14.029 μs** |  **0.769 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   264.94 μs |  11.310 μs |  0.620 μs |  1.28 | 4.3945 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    61.44 μs |   5.740 μs |  0.315 μs |  0.30 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   199.09 μs |  34.624 μs |  1.898 μs |  0.96 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,979.83 μs** |  **59.539 μs** |  **3.264 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,034.92 μs |  90.570 μs |  4.964 μs |  1.03 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,080.46 μs |   7.832 μs |  0.429 μs |  0.55 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,977.88 μs | 102.628 μs |  5.625 μs |  1.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,052.56 μs** |  **56.259 μs** |  **3.084 μs** |  **1.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,086.18 μs | 113.374 μs |  6.214 μs |  1.02 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,082.69 μs |  47.013 μs |  2.577 μs |  0.53 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,989.88 μs | 261.379 μs | 14.327 μs |  0.97 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.437 μs** |  **0.0499 μs** | **0.0027 μs** |         **-** |
| EqualLength | 65           |     2.975 μs |  0.0753 μs | 0.0041 μs |         - |
| **Embedded**    | **128**          |    **39.971 μs** |  **0.1467 μs** | **0.0080 μs** |         **-** |
| EqualLength | 128          |     5.333 μs |  0.2449 μs | 0.0134 μs |         - |
| **Embedded**    | **512**          | **1,356.766 μs** | **85.5242 μs** | **4.6879 μs** |         **-** |
| EqualLength | 512          |    44.554 μs |  1.0580 μs | 0.0580 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 |  8.345 ms | 0.9436 ms | 0.0517 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 13.562 ms | 2.7631 ms | 0.1515 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.859 ms |  1.8735 ms | 0.1027 ms | 109.3750 | 101.5625 |  31.2500 |  3711.64 KB |
| TokenizerJsonWordPiece | 11.389 ms |  1.0906 ms | 0.0598 ms | 187.5000 | 171.8750 |  46.8750 |  5852.31 KB |
| TokenizerJsonUnigram   | 12.790 ms |  0.7445 ms | 0.0408 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.087 ms |  2.1188 ms | 0.1161 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.559 ms |  0.8083 ms | 0.0443 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.313 ms |  1.0495 ms | 0.0575 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.972 ms |  0.9418 ms | 0.0516 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  8.025 ms |  2.1729 ms | 0.1191 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.434 ms | 11.9310 ms | 0.6540 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 73.197 ms |  2.8298 ms | 0.1551 ms |        - |        - |        - | 16095.25 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 12.24 ms | 0.460 ms | 0.025 ms |  31.2500 |   2.75 MB |
| EncodeDocuments    | 55.25 ms | 3.057 ms | 0.168 ms | 100.0000 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,193.54 μs** | **2,050.129 μs** | **112.375 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 14,903.93 μs |   803.039 μs |  44.017 μs |  1.62 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **133.01 μs** |     **1.188 μs** |   **0.065 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    180.48 μs |     6.860 μs |   0.376 μs |  1.36 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,837.38 μs** |   **262.196 μs** |  **14.372 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,768.31 μs | 1,980.059 μs | 108.534 μs |  0.98 |    0.03 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.80 μs** |     **8.212 μs** |   **0.450 μs** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.41 μs |     0.743 μs |   0.041 μs |  1.20 |    0.02 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Extract**    | **1**     | **6.870 ms** | **0.1199 ms** | **0.0066 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.718 ms | 0.6038 ms | 0.0331 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.867 ms** | **0.1624 ms** | **0.0089 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.775 ms | 1.0389 ms | 0.0569 ms |  0.99 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.802 μs** | **0.4628 μs** | **0.0254 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **4.017 μs** | **0.3995 μs** | **0.0219 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.879 μs** | **0.2624 μs** | **0.0144 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.958 μs** | **0.7458 μs** | **0.0409 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 32.125 ms | 3.5679 ms | 0.1956 ms |       - |       - |       - |  800342 B |
| NdcgIgnoringTies                  | 100000 | 29.777 ms | 0.2947 ms | 0.0162 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 18.417 ms | 0.3234 ms | 0.0177 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 15.206 ms | 0.7361 ms | 0.0404 ms |       - |       - |       - |     180 B |
| CoverageErrorScore                | 100000 |  6.578 ms | 0.2187 ms | 0.0120 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 54.212 ms | 4.1338 ms | 0.2266 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                   | Length | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **377.2 ns** |     **33.32 ns** |     **1.83 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,312.5 ns |     67.75 ns |     3.71 ns | 14.08 |    0.06 |         - |          NA |
|                          |        |              |              |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,727.6 ns** |    **306.34 ns** |    **16.79 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 597,167.2 ns | 21,281.11 ns | 1,166.49 ns | 26.27 |    0.05 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.44 μs** |   **6.180 μs** |  **0.339 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    45.01 μs |   3.744 μs |  0.205 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.00 μs |   1.301 μs |  0.071 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   527.57 μs |  57.230 μs |  3.137 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **460.48 μs** |  **23.978 μs** |  **1.314 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   458.01 μs |  23.722 μs |  1.300 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,109.69 μs |  30.941 μs |  1.696 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,983.99 μs | 233.592 μs | 12.804 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **Fit**    | **2000x2000x20** | **27.74 ms** | **0.314 ms** | **0.017 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.70 ms** | **0.841 ms** | **0.046 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-URSTNN : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error          | StdDev        | Median        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|---------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-URSTNN** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **108.310 μs** |      **4.9076 μs** |     **4.8199 μs** |    **108.578 μs** |  **1.00** |    **0.06** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    120.010 μs |     11.9127 μs |    12.2335 μs |    117.801 μs |  1.11 |    0.12 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,447.141 μs |     36.1895 μs |    41.6759 μs |  2,439.638 μs | 22.64 |    1.06 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    117.695 μs |     24.1000 μs |    27.7536 μs |    107.204 μs |  1.09 |    0.26 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     69.517 μs |      1.2489 μs |     1.2265 μs |     69.434 μs |  0.64 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     59.183 μs |      1.8339 μs |     1.7154 μs |     58.791 μs |  0.55 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |     97.377 μs |      6.1720 μs |     6.0618 μs |     96.400 μs |  0.90 |    0.07 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |    441.020 μs |     18.9928 μs |    21.8721 μs |    440.596 μs |  4.08 |    0.27 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,336.484 μs |     80.5581 μs |    89.5401 μs |  2,352.125 μs | 21.61 |    1.24 |        - |        - |        - |  297216 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,524.794 μs |    199.2712 μs |   221.4895 μs |  6,541.446 μs | 60.35 |    3.31 |        - |        - |        - |  441928 B |        5.41 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.943 μs |      7.1607 μs |     0.3925 μs |     48.131 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     41.225 μs |      4.9336 μs |     0.2704 μs |     41.113 μs |  0.86 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.192 μs |     15.4023 μs |     0.8442 μs |    465.027 μs |  9.70 |    0.07 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     49.039 μs |     11.1971 μs |     0.6138 μs |     48.783 μs |  1.02 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.503 μs |      0.7460 μs |     0.0409 μs |     10.483 μs |  0.22 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.695 μs |      0.6208 μs |     0.0340 μs |      7.682 μs |  0.16 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.989 μs |      2.5880 μs |     0.1419 μs |     12.032 μs |  0.25 |    0.00 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,631.691 μs |  4,433.8538 μs |   243.0345 μs |  1,635.675 μs | 34.04 |    4.40 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,033.857 μs |    826.4159 μs |    45.2986 μs |  1,051.448 μs | 21.57 |    0.83 |  39.0625 |   1.9531 |        - |  650157 B |       8.037 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,629.417 μs |  1,829.4391 μs |   100.2777 μs |  1,629.426 μs | 33.99 |    1.83 |  35.1563 |        - |        - |  627325 B |       7.755 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-URSTNN** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,611.860 μs** |     **15.2683 μs** |    **17.5830 μs** |  **1,613.027 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,481.423 μs |     10.7627 μs |    11.9627 μs |  1,477.695 μs |  0.92 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,375.404 μs |     94.8542 μs |   101.4929 μs | 15,378.748 μs |  9.54 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,573.580 μs |     12.5761 μs |    13.9782 μs |  1,574.230 μs |  0.98 |    0.01 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    422.618 μs |      9.9406 μs |    11.0489 μs |    417.528 μs |  0.26 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    318.649 μs |      9.8249 μs |    11.3144 μs |    313.014 μs |  0.20 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |    433.017 μs |     13.6537 μs |    15.1761 μs |    433.508 μs |  0.27 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    |  5,866.833 μs |  1,950.5136 μs | 2,167.9907 μs |  6,807.608 μs |  3.64 |    1.31 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 18,424.451 μs |  8,563.5513 μs | 9,861.7984 μs | 12,808.826 μs | 11.43 |    5.97 |        - |        - |        - |  516192 B |       0.322 |
| MlNet_NormalizeRobustScaling_Read | Job-URSTNN | 1               | 20             | Throughput  | 1            | 5           | 20000    | 21,973.432 μs |    689.6923 μs |   737.9630 μs | 22,205.928 μs | 13.63 |    0.47 |        - |        - |        - | 4279848 B |       2.672 |
|                                   |            |                 |                |             |              |             |          |               |                |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,523.490 μs |    668.8562 μs |    36.6623 μs |  1,521.155 μs |  1.00 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601922 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,439.488 μs |  2,317.5304 μs |   127.0317 μs |  1,479.197 μs |  0.95 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,802.830 μs |  1,450.6683 μs |    79.5160 μs | 14,819.685 μs |  9.72 |    0.21 | 875.0000 | 875.0000 | 875.0000 | 3201322 B |       1.998 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,616.658 μs |    813.3229 μs |    44.5810 μs |  1,594.456 μs |  1.06 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601052 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    307.581 μs |     28.7203 μs |     1.5743 μs |    306.959 μs |  0.20 |    0.00 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    244.912 μs |      8.3977 μs |     0.4603 μs |    244.715 μs |  0.16 |    0.00 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    432.573 μs |    231.1587 μs |    12.6706 μs |    433.063 μs |  0.28 |    0.01 | 128.9063 | 124.0234 | 124.0234 |  560074 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,251.112 μs |    983.5143 μs |    53.9097 μs |  1,247.066 μs |  0.82 |    0.04 |  19.5313 |   3.9063 |        - |  354153 B |       0.221 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,447.853 μs | 19,238.3780 μs | 1,054.5204 μs |  5,225.582 μs |  3.58 |    0.60 |  31.2500 |        - |        - |  545588 B |       0.341 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,743.218 μs |  1,407.5005 μs |    77.1499 μs | 19,719.509 μs | 12.96 |    0.27 | 250.0000 |  93.7500 |        - | 4299910 B |       2.684 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Model   | Mean     | Error   | StdDev  | Gen0     | Allocated |
|---------------- |-------- |---------:|--------:|--------:|---------:|----------:|
| **EncodeDocuments** | **Llama2**  | **143.9 ms** | **6.45 ms** | **0.35 ms** | **250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **138.9 ms** | **2.95 ms** | **0.16 ms** | **250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **PerSample** | **2000**    |  **27.33 ms** | **0.169 ms** | **0.009 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **193.20 ms** | **2.533 ms** | **0.139 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

| Method                 | Mean            | Error           | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,056.7 ns |       177.24 ns |      9.72 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,305.9 ns |       136.44 ns |      7.48 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,073.9 ns |       299.15 ns |     16.40 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        438.8 ns |        41.21 ns |      2.26 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,713,283.5 ns | 1,017,034.76 ns | 55,747.11 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method                          | SampleCount | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **75.856 μs** |     **33.782 μs** |   **1.8517 μs** |  **1.00** |    **0.03** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    184.198 μs |      5.008 μs |   0.2745 μs |  2.43 |    0.05 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.744 μs |      1.923 μs |   0.1054 μs |  0.06 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    335.243 μs |      3.112 μs |   0.1706 μs |  4.42 |    0.09 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,721.511 μs |    561.935 μs |  30.8015 μs | 75.46 |    1.63 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     62.045 μs |      2.201 μs |   0.1207 μs |  0.82 |    0.02 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,112.152 μs |    293.771 μs |  16.1026 μs | 14.67 |    0.36 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |               |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,104.352 μs** |    **584.553 μs** |  **32.0413 μs** |  **1.00** |    **0.04** | **441.4063** | **427.7344** | **417.9688** | **2738.33 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,343.239 μs |    256.226 μs |  14.0446 μs |  2.12 |    0.05 | 535.1563 | 515.6250 | 511.7188 | 3129.98 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     84.112 μs |     25.565 μs |   1.4013 μs |  0.08 |    0.00 |  55.5420 |  55.5420 |  55.5420 |  391.16 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    278.424 μs |    227.381 μs |  12.4635 μs |  0.25 |    0.01 |   1.9531 |        - |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 45,251.294 μs | 12,758.285 μs | 699.3247 μs | 41.00 |    1.17 |        - |        - |        - |  148.74 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     64.489 μs |      3.756 μs |   0.2059 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,620.893 μs |    299.776 μs |  16.4318 μs |  6.90 |    0.17 |        - |        - |        - |   34.19 KB |       0.012 |

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

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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

| Method  | Words | Mean        | Error     | StdDev  | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|----------:|--------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **637.4 μs** |  **11.88 μs** | **0.65 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,818.9 μs** | **106.11 μs** | **5.82 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,864.7 μs** |  **20.49 μs** | **1.12 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **20.89 ms** |  **2.553 ms** | **0.140 ms** |  **1.00** |    **0.01** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.74 ms |  1.874 ms | 0.103 ms |  2.57 |    0.02 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **45.92 ms** |  **3.029 ms** | **0.166 ms** |  **1.00** |    **0.00** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  55.56 ms |  0.882 ms | 0.048 ms |  1.21 |    0.00 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **82.93 ms** |  **2.900 ms** | **0.159 ms** |  **1.00** |    **0.00** | **1714.2857** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 257.33 ms | 27.218 ms | 1.492 ms |  3.10 |    0.02 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

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
| **TopTwo** | **10**      | **11.65 ms** | **1.087 ms** | **0.060 ms** |         **-** |
| **TopTwo** | **100**     | **88.27 ms** | **0.948 ms** | **0.052 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-10-28, measured at commit `b3fd40aa4e70bd587819396620eee61914103d94`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  Job-MKGWOJ : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-MKGWOJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **54.59 μs** |    **11.893 μs** |    **13.696 μs** |     **48.29 μs** |   **1.05** |    **0.34** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |     89.66 μs |     1.790 μs |     1.838 μs |     89.39 μs |   1.73 |    0.35 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    171.26 μs |     5.989 μs |     6.150 μs |    170.62 μs |   3.30 |    0.68 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    262.92 μs |     6.781 μs |     7.255 μs |    263.03 μs |   5.06 |    1.04 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,003.99 μs |    19.321 μs |    22.251 μs |    997.22 μs |  19.34 |    3.94 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,623.45 μs |    19.390 μs |    21.552 μs |  1,621.88 μs |  31.27 |    6.35 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,113.17 μs |    18.593 μs |    19.894 μs |  1,114.73 μs |  21.44 |    4.36 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    211.33 μs |     9.557 μs |     9.814 μs |    209.79 μs |   4.07 |    0.85 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,894.86 μs |    14.485 μs |    16.100 μs |  1,893.88 μs |  36.50 |    7.41 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     | 14,218.72 μs | 5,047.879 μs | 5,813.145 μs | 10,349.89 μs | 273.91 |  124.56 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,280.56 μs |    57.047 μs |    61.040 μs |  1,286.18 μs |  24.67 |    5.14 |        - |        - |        - |  315.95 KB |        9.88 |
| MlNet_NormalizeBinning_Read | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,151.48 μs |   143.970 μs |   160.023 μs |  3,163.46 μs |  60.71 |   12.68 |        - |        - |        - |  426.02 KB |       13.32 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.87 μs |     2.549 μs |     0.140 μs |     13.95 μs |   1.00 |    0.01 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.08 μs |     8.841 μs |     0.485 μs |     83.02 μs |   5.99 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    139.15 μs |    12.500 μs |     0.685 μs |    139.15 μs |  10.03 |    0.10 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     75.19 μs |    13.953 μs |     0.765 μs |     74.82 μs |   5.42 |    0.07 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     52.95 μs |    23.046 μs |     1.263 μs |     52.43 μs |   3.82 |    0.09 |   2.0142 |   0.0610 |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    465.62 μs |    22.581 μs |     1.238 μs |    466.24 μs |  33.56 |    0.30 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    179.71 μs |    38.759 μs |     2.125 μs |    180.76 μs |  12.95 |    0.17 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    140.44 μs |     8.389 μs |     0.460 μs |    140.27 μs |  10.12 |    0.09 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    725.72 μs |   132.181 μs |     7.245 μs |    722.80 μs |  52.31 |    0.64 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,753.96 μs |   410.992 μs |    22.528 μs |  8,746.03 μs | 631.02 |    5.71 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    258.97 μs |    29.433 μs |     1.613 μs |    259.00 μs |  18.67 |    0.19 |  19.0430 |  18.5547 |        - |  300.56 KB |        9.61 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,002.62 μs | 1,216.802 μs |    66.697 μs |    998.44 μs |  72.27 |    4.21 |  42.9688 |   5.8594 |        - |  698.35 KB |       22.33 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-MKGWOJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **481.65 μs** |    **10.440 μs** |    **12.023 μs** |    **479.87 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,707.25 μs |    16.383 μs |    18.867 μs |  1,707.17 μs |   3.55 |    0.09 |        - |        - |        - | 2344.45 KB |        3.75 |
| Lodestar_Discretize         | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,748.19 μs |    29.051 μs |    32.290 μs |  3,744.75 μs |   7.79 |    0.20 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,522.49 μs |    18.927 μs |    20.252 μs |  4,521.26 μs |   9.40 |    0.23 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,189.80 μs | 5,674.587 μs | 6,534.862 μs |  5,830.30 μs |  23.25 |   13.26 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,942.95 μs |   719.178 μs |   706.329 μs | 10,654.64 μs |  22.73 |    1.53 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,610.30 μs | 5,754.001 μs | 6,626.316 μs |  5,921.54 μs |  24.12 |   13.45 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,069.41 μs |    12.493 μs |    14.387 μs |  3,066.36 μs |   6.38 |    0.16 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Power_Fit          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,761.35 μs | 3,588.118 μs | 4,132.082 μs | 14,818.96 μs |  36.90 |    8.43 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,392.88 μs | 5,092.480 μs | 5,864.508 μs | 10,531.51 μs |  29.90 |   11.91 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,137.72 μs | 1,303.121 μs | 1,500.676 μs | 12,786.83 μs |  25.22 |    3.10 |        - |        - |        - |   555.3 KB |        0.89 |
| MlNet_NormalizeBinning_Read | Job-MKGWOJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,483.14 μs | 2,744.243 μs | 2,695.213 μs | 15,576.75 μs |  32.17 |    5.49 |        - |        - |        - | 2074.73 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    686.53 μs |    83.562 μs |     4.580 μs |    688.24 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    640.89 μs |   126.270 μs |     6.921 μs |    637.96 μs |   0.93 |    0.01 | 255.8594 | 255.8594 | 255.8594 | 2346.09 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,226.20 μs |   133.898 μs |     7.339 μs |  3,227.96 μs |   4.70 |    0.03 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,972.22 μs |   694.558 μs |    38.071 μs |  1,982.87 μs |   2.87 |    0.05 | 199.2188 | 199.2188 | 199.2188 |  625.15 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,799.46 μs |   499.765 μs |    27.394 μs |  5,809.56 μs |   8.45 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 11,012.29 μs |   235.020 μs |    12.882 μs | 11,014.43 μs |  16.04 |    0.09 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,956.76 μs |   170.413 μs |     9.341 μs |  5,959.81 μs |   8.68 |    0.05 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,226.44 μs |   112.281 μs |     6.155 μs |  3,226.23 μs |   4.70 |    0.03 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,767.64 μs |   389.995 μs |    21.377 μs | 13,773.16 μs |  20.05 |    0.12 | 750.0000 | 750.0000 | 750.0000 |  2662.4 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,178.91 μs |   426.792 μs |    23.394 μs |  9,180.47 μs |  13.37 |    0.08 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,383.13 μs |   991.693 μs |    54.358 μs |  2,401.33 μs |   3.47 |    0.07 |  27.3438 |  11.7188 |        - |  467.76 KB |        0.75 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,813.47 μs | 1,377.963 μs |    75.531 μs | 10,848.19 μs |  15.75 |    0.13 | 265.6250 | 218.7500 | 187.5000 | 2132.33 KB |        3.41 |

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

_As of 2026-10-29, measured at commit `116c2b60eb01f66d877a72dad27d55a719473120`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-HGOFTC : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-----------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **1000**    |     **62.22 μs** |   **1.377 μs** |   **1.586 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 1000    |     84.42 μs |   0.950 μs |   1.094 μs | 0.2441 |  20.58 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 1000    |     72.99 μs |   0.850 μs |   0.978 μs | 0.1221 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     65.91 μs |   9.134 μs |   0.501 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     81.38 μs |  31.265 μs |   1.714 μs | 0.2441 |  20.58 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     72.89 μs |  20.601 μs |   1.129 μs | 0.1221 |  15.52 KB |
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **10000**   |    **700.18 μs** |   **2.826 μs** |   **3.141 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 10000   |  1,000.33 μs |   9.425 μs |   9.256 μs |      - |  26.17 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 10000   |    957.22 μs |  11.123 μs |  12.809 μs |      - |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    616.61 μs |  95.114 μs |   5.214 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |  1,094.44 μs |  44.666 μs |   2.448 μs |      - |  26.17 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    948.42 μs | 101.213 μs |   5.548 μs |      - |  21.14 KB |
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **100000**  | **16,403.14 μs** | **333.482 μs** | **342.461 μs** |      **-** |   **3.23 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 100000  | 21,800.91 μs | 119.228 μs | 137.303 μs |      - |  45.96 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 100000  | 21,941.85 μs | 110.316 μs | 122.616 μs |      - |  41.06 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  | 16,659.77 μs | 765.179 μs |  41.942 μs |      - |   3.22 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 21,483.18 μs | 327.570 μs |  17.955 μs |      - |  46.03 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 21,158.80 μs | 564.834 μs |  30.960 μs |      - |  41.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

| Method   | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.721 ms** |   **0.3213 ms** | **0.0176 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  53.966 ms | 172.4447 ms | 9.4523 ms | 14.50 |    2.20 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **16.638 ms** |   **0.6472 ms** | **0.0355 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 377.202 ms | 160.7733 ms | 8.8125 ms | 22.67 |    0.46 | 76000.0000 | 76000.0000 | 76000.0000 | 324.65 MB |       40.04 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.173 | 0.035 | 0.20x | 0.173 | 0.035 | 0.20x |
| cdist_wratio_n50 | 2.256 | 2.452 | 1.09x | 2.256 | 2.452 | 1.09x |
| cdist_ratio_varied_n50 | 0.140 | 0.031 | 0.22x | 0.140 | 0.031 | 0.22x |
| cdist_ratio_varied_cutoff90_n50 | 0.056 | 0.031 | 0.56x | 0.056 | 0.031 | 0.56x |
| cdist_ratio_n200 | 2.862 | 0.438 | 0.15x | 2.893 | 0.438 | 0.15x |
| cdist_wratio_n200 | 34.389 | 37.065 | 1.08x | 34.389 | 37.064 | 1.08x |
| cdist_ratio_varied_n200 | 2.396 | 0.385 | 0.16x | 2.399 | 0.385 | 0.16x |
| cdist_ratio_varied_cutoff90_n200 | 1.008 | 0.385 | 0.38x | 1.010 | 0.385 | 0.38x |
| cdist_ratio_n500 | 17.086 | 2.654 | 0.16x | 17.101 | 2.654 | 0.16x |
| cdist_wratio_n500 | 207.766 | 225.825 | 1.09x | 207.903 | 225.816 | 1.09x |
| cdist_ratio_varied_n500 | 13.789 | 2.305 | 0.17x | 13.894 | 2.305 | 0.17x |
| cdist_ratio_varied_cutoff90_n500 | 5.556 | 2.305 | 0.41x | 5.670 | 2.305 | 0.41x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 118.0 | 28.5 | 4.13x C# faster |
| latin | 32 | 180.8 | 86.4 | 2.09x C# faster |
| latin | 128 | 465.0 | 423.4 | 1.10x C# faster |
| latin | 512 | 4463.0 | 4864.1 | 1.09x Py faster |
| cjk | 8 | 143.6 | 28.4 | 5.06x C# faster |
| cjk | 32 | 334.1 | 220.6 | 1.51x C# faster |
| cjk | 128 | 1910.9 | 1391.1 | 1.37x C# faster |
| cjk | 512 | 14832.3 | 8914.3 | 1.66x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 148.4 | 18.3 | 8.11x C# faster |
| latin | 32 | 279.6 | 159.0 | 1.76x C# faster |
| latin | 128 | 1693.3 | 904.1 | 1.87x C# faster |
| latin | 512 | 14055.3 | 11723.2 | 1.20x C# faster |
| cjk | 8 | 159.7 | 18.0 | 8.86x C# faster |
| cjk | 32 | 387.2 | 289.4 | 1.34x C# faster |
| cjk | 128 | 2846.3 | 2031.9 | 1.40x C# faster |
| cjk | 512 | 23615.6 | 17010.2 | 1.39x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 0.961 | 214.15x | 0.004 | 0.961 | 214.14x |
| accuracy_n1000_k2 | 0.000 | 0.514 | 3866.88x | 0.000 | 0.514 | 3866.07x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.778 | 348.38x | 0.005 | 1.778 | 348.39x |
| classification_report_n1000_k2 | 0.007 | 6.840 | 932.43x | 0.007 | 6.839 | 932.39x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.877 | 107.60x | 0.017 | 1.877 | 107.50x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.024 | 224.70x | 0.005 | 1.024 | 224.66x |
| matthews_n1000_k2 | 0.005 | 1.992 | 439.45x | 0.005 | 1.991 | 439.35x |
| cohen_kappa_n1000_k2 | 0.005 | 1.075 | 238.52x | 0.005 | 1.075 | 238.53x |
| mse_n1000_k2 | 0.000 | 0.306 | 623.36x | 0.000 | 0.306 | 623.37x |
| mae_n1000_k2 | 0.000 | 0.306 | 632.06x | 0.000 | 0.306 | 631.97x |
| median_ae_n1000_k2 | 0.005 | 0.318 | 65.38x | 0.005 | 0.318 | 65.38x |
| r2_n1000_k2 | 0.001 | 0.372 | 312.13x | 0.001 | 0.372 | 312.12x |
| confusion_matrix_n1000_k10 | 0.005 | 0.983 | 195.74x | 0.005 | 0.983 | 195.75x |
| accuracy_n1000_k10 | 0.000 | 0.512 | 3815.94x | 0.000 | 0.512 | 3816.00x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.794 | 307.73x | 0.006 | 1.793 | 307.73x |
| classification_report_n1000_k10 | 0.011 | 6.939 | 606.28x | 0.011 | 6.938 | 606.24x |
| roc_auc_ovr_macro_n1000_k10 | 0.523 | 9.626 | 18.40x | 0.523 | 9.626 | 18.40x |
| balanced_accuracy_n1000_k10 | 0.005 | 1.034 | 195.18x | 0.005 | 1.034 | 195.17x |
| matthews_n1000_k10 | 0.005 | 1.993 | 388.20x | 0.005 | 1.993 | 388.20x |
| cohen_kappa_n1000_k10 | 0.006 | 1.079 | 186.60x | 0.006 | 1.079 | 186.60x |
| mse_n1000_k10 | 0.000 | 0.299 | 610.43x | 0.000 | 0.299 | 610.42x |
| mae_n1000_k10 | 0.000 | 0.302 | 622.20x | 0.000 | 0.302 | 622.20x |
| median_ae_n1000_k10 | 0.005 | 0.315 | 64.33x | 0.005 | 0.315 | 64.33x |
| r2_n1000_k10 | 0.001 | 0.367 | 307.49x | 0.001 | 0.367 | 307.48x |
| confusion_matrix_n100000_k2 | 0.473 | 10.716 | 22.67x | 0.473 | 10.716 | 22.67x |
| accuracy_n100000_k2 | 0.012 | 3.763 | 306.68x | 0.012 | 3.763 | 306.67x |
| precision_recall_f1_macro_n100000_k2 | 0.476 | 12.320 | 25.86x | 0.476 | 12.319 | 25.86x |
| classification_report_n100000_k2 | 0.480 | 27.093 | 56.46x | 0.480 | 27.092 | 56.46x |
| roc_auc_binary_n100000_k2 | 4.350 | 26.584 | 6.11x | 4.350 | 26.583 | 6.11x |
| balanced_accuracy_n100000_k2 | 0.471 | 10.806 | 22.93x | 0.471 | 10.806 | 22.92x |
| matthews_n100000_k2 | 0.475 | 21.556 | 45.41x | 0.475 | 21.554 | 45.41x |
| cohen_kappa_n100000_k2 | 0.462 | 10.876 | 23.55x | 0.462 | 10.875 | 23.55x |
| mse_n100000_k2 | 0.045 | 0.446 | 9.80x | 0.045 | 0.446 | 9.80x |
| mae_n100000_k2 | 0.045 | 0.437 | 9.70x | 0.045 | 0.437 | 9.70x |
| median_ae_n100000_k2 | 0.600 | 1.801 | 3.00x | 0.625 | 1.801 | 2.88x |
| r2_n100000_k2 | 0.110 | 0.664 | 6.02x | 0.110 | 0.664 | 6.02x |
| confusion_matrix_n100000_k10 | 0.509 | 10.741 | 21.09x | 0.509 | 10.737 | 21.08x |
| accuracy_n100000_k10 | 0.012 | 3.776 | 308.07x | 0.012 | 3.776 | 308.07x |
| precision_recall_f1_macro_n100000_k10 | 0.509 | 12.967 | 25.47x | 0.509 | 12.967 | 25.47x |
| classification_report_n100000_k10 | 0.516 | 29.658 | 57.47x | 0.516 | 29.657 | 57.47x |
| roc_auc_ovr_macro_n100000_k10 | 34.534 | 215.631 | 6.24x | 34.532 | 215.621 | 6.24x |
| balanced_accuracy_n100000_k10 | 0.509 | 10.811 | 21.22x | 0.509 | 10.810 | 21.22x |
| matthews_n100000_k10 | 0.509 | 22.247 | 43.74x | 0.509 | 22.246 | 43.74x |
| cohen_kappa_n100000_k10 | 0.510 | 10.868 | 21.32x | 0.510 | 10.868 | 21.32x |
| mse_n100000_k10 | 0.046 | 0.448 | 9.81x | 0.046 | 0.448 | 9.81x |
| mae_n100000_k10 | 0.045 | 0.438 | 9.75x | 0.045 | 0.438 | 9.75x |
| median_ae_n100000_k10 | 0.599 | 1.792 | 2.99x | 0.643 | 1.791 | 2.79x |
| r2_n100000_k10 | 0.110 | 0.663 | 6.00x | 0.110 | 0.663 | 6.00x |
| confusion_matrix_n1000000_k2 | 4.763 | 100.103 | 21.02x | 4.763 | 100.098 | 21.02x |
| accuracy_n1000000_k2 | 0.122 | 33.148 | 272.18x | 0.122 | 33.147 | 272.18x |
| precision_recall_f1_macro_n1000000_k2 | 4.761 | 107.928 | 22.67x | 4.761 | 107.921 | 22.67x |
| classification_report_n1000000_k2 | 4.763 | 210.162 | 44.12x | 4.763 | 210.140 | 44.12x |
| roc_auc_binary_n1000000_k2 | 56.143 | 289.651 | 5.16x | 56.140 | 289.643 | 5.16x |
| balanced_accuracy_n1000000_k2 | 4.762 | 99.921 | 20.98x | 4.762 | 99.915 | 20.98x |
| matthews_n1000000_k2 | 4.767 | 200.199 | 41.99x | 4.767 | 200.179 | 41.99x |
| cohen_kappa_n1000000_k2 | 4.766 | 99.846 | 20.95x | 4.766 | 99.834 | 20.95x |
| mse_n1000000_k2 | 0.447 | 2.130 | 4.76x | 0.447 | 2.130 | 4.76x |
| mae_n1000000_k2 | 0.451 | 2.066 | 4.58x | 0.451 | 2.066 | 4.58x |
| median_ae_n1000000_k2 | 6.199 | 14.207 | 2.29x | 6.410 | 14.207 | 2.22x |
| r2_n1000000_k2 | 1.109 | 3.483 | 3.14x | 1.109 | 3.483 | 3.14x |
| confusion_matrix_n1000000_k10 | 5.014 | 100.247 | 19.99x | 5.014 | 100.237 | 19.99x |
| accuracy_n1000000_k10 | 0.121 | 33.134 | 273.50x | 0.121 | 33.133 | 273.50x |
| precision_recall_f1_macro_n1000000_k10 | 5.021 | 114.498 | 22.81x | 5.020 | 114.487 | 22.80x |
| classification_report_n1000000_k10 | 5.036 | 234.275 | 46.52x | 5.036 | 234.265 | 46.52x |
| balanced_accuracy_n1000000_k10 | 5.020 | 100.386 | 20.00x | 5.020 | 100.382 | 20.00x |
| matthews_n1000000_k10 | 5.019 | 207.162 | 41.27x | 5.019 | 207.144 | 41.27x |
| cohen_kappa_n1000000_k10 | 5.034 | 100.499 | 19.96x | 5.034 | 100.489 | 19.96x |
| mse_n1000000_k10 | 0.459 | 2.117 | 4.61x | 0.459 | 2.116 | 4.61x |
| mae_n1000000_k10 | 0.453 | 2.090 | 4.62x | 0.453 | 2.090 | 4.62x |
| median_ae_n1000000_k10 | 6.439 | 14.158 | 2.20x | 6.720 | 14.157 | 2.11x |
| r2_n1000000_k10 | 1.110 | 3.528 | 3.18x | 1.110 | 3.528 | 3.18x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.236 | 9.774 | 1.87x | 5.516 | 9.774 | 1.77x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.138 | 16.400 | 1.35x | 12.540 | 16.399 | 1.31x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.576 | 38.602 | 2.84x | 13.936 | 38.600 | 2.77x | 1,990,038 | 1,990,038 |
| spiece_model | 7.519 | 28.471 | 3.79x | 7.813 | 28.470 | 3.64x | 533,084 | 533,084 |
| tfidf_save | 1.520 | 2.365 | 1.56x | 1.543 | 2.365 | 1.53x | 581,787 | 591,922 |
| tfidf_load | 4.759 | 4.094 | 0.86x | 4.924 | 4.094 | 0.83x | 581,787 | 591,922 |
| embedding_index_save | 4.613 | 1.611 | 0.35x | 4.794 | 1.611 | 0.34x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.489 | 34.164 | 0.69x | 10.493 | 4.701 | 0.45x | 20,589,007 | 15,360,128 |
| embedding_index_load | 8.190 | 1.444 | 0.18x | 8.730 | 1.439 | 0.16x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 8.213 | 0.881 | 0.11x | 8.714 | 0.881 | 0.10x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 6.853 | 1.380 | 0.20x | 7.217 | 1.380 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 3.215 | 1.385 | 0.43x | 3.375 | 1.385 | 0.41x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 93.48x | 0.000 | 0.001 | 93.48x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 411.288 | 566.817 | 1.38x | 413.618 | 566.778 | 1.37x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 70.840 | 66.523 | 0.94x | 70.965 | 66.517 | 0.94x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.089 | 0.115 | 1.29x | 0.089 | 0.115 | 1.29x |
| stratified_n10000 | 0.179 | 0.884 | 4.95x | 0.179 | 0.884 | 4.95x |
| traintest_n10000 | 0.004 | 0.233 | 54.18x | 0.004 | 0.233 | 54.18x |
| kfold_seeded_n10000 | 0.308 | 0.672 | 2.18x | 0.308 | 0.672 | 2.18x |
| stratified_seeded_n10000 | 0.430 | 1.471 | 3.42x | 0.430 | 1.471 | 3.42x |
| traintest_seeded_n10000 | 0.151 | 0.518 | 3.43x | 0.151 | 0.518 | 3.43x |
| stratified_traintest_n10000 | 0.658 | 2.791 | 4.24x | 0.658 | 2.791 | 4.24x |
| group_kfold_n10000 | 0.128 | 0.594 | 4.64x | 0.128 | 0.594 | 4.64x |
| stratified_group_kfold_n10000 | 0.235 | 31.592 | 134.55x | 0.235 | 31.592 | 134.55x |
| timeseries_n10000 | 0.000 | 0.012 | 97.54x | 0.000 | 0.012 | 97.54x |
| repeated_kfold_n10000 | 0.959 | 1.790 | 1.87x | 0.959 | 1.790 | 1.87x |
| kfold_n100000 | 1.140 | 2.712 | 2.38x | 1.293 | 2.712 | 2.10x |
| stratified_n100000 | 2.182 | 7.261 | 3.33x | 2.344 | 7.260 | 3.10x |
| traintest_n100000 | 0.223 | 0.400 | 1.79x | 0.224 | 0.400 | 1.78x |
| kfold_seeded_n100000 | 3.559 | 7.038 | 1.98x | 3.732 | 7.038 | 1.89x |
| stratified_seeded_n100000 | 4.753 | 11.478 | 2.41x | 4.986 | 11.478 | 2.30x |
| traintest_seeded_n100000 | 1.981 | 2.006 | 1.01x | 2.026 | 2.006 | 0.99x |
| stratified_traintest_n100000 | 7.957 | 25.254 | 3.17x | 8.056 | 25.253 | 3.13x |
| group_kfold_n100000 | 1.614 | 6.799 | 4.21x | 1.767 | 6.799 | 3.85x |
| stratified_group_kfold_n100000 | 2.763 | 319.468 | 115.63x | 2.930 | 319.447 | 109.02x |
| timeseries_n100000 | 0.000 | 0.042 | 350.87x | 0.000 | 0.042 | 350.87x |
| repeated_kfold_n100000 | 10.099 | 19.350 | 1.92x | 10.290 | 19.349 | 1.88x |
| kfold_n1000000 | 11.528 | 17.806 | 1.54x | 12.554 | 17.804 | 1.42x |
| stratified_n1000000 | 23.932 | 68.535 | 2.86x | 25.412 | 68.531 | 2.70x |
| traintest_n1000000 | 0.768 | 2.051 | 2.67x | 0.901 | 2.051 | 2.28x |
| kfold_seeded_n1000000 | 37.898 | 58.616 | 1.55x | 38.751 | 58.610 | 1.51x |
| stratified_seeded_n1000000 | 51.596 | 107.986 | 2.09x | 53.727 | 107.979 | 2.01x |
| traintest_seeded_n1000000 | 21.623 | 17.208 | 0.80x | 22.243 | 17.204 | 0.77x |
| stratified_traintest_n1000000 | 92.862 | 233.760 | 2.52x | 93.555 | 233.747 | 2.50x |
| group_kfold_n1000000 | 14.935 | 51.616 | 3.46x | 15.682 | 51.611 | 3.29x |
| stratified_group_kfold_n1000000 | 28.846 | 3178.055 | 110.17x | 30.021 | 3177.892 | 105.86x |
| timeseries_n1000000 | 0.000 | 0.328 | 3176.40x | 0.000 | 0.328 | 3176.44x |
| repeated_kfold_n1000000 | 112.875 | 174.363 | 1.54x | 115.279 | 174.356 | 1.51x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.008 | 0.822 | 102.70x | 0.008 | 0.822 | 102.68x |
| mann_whitney_n1000 | 0.030 | 0.748 | 24.91x | 0.030 | 0.748 | 24.91x |
| chi_square_n1000 | 0.000 | 0.339 | 1734.70x | 0.000 | 0.339 | 1734.71x |
| fligner_n1000 | 0.074 | 0.806 | 10.87x | 0.074 | 0.806 | 10.87x |
| anderson_ksamp_n1000 | 0.163 | 0.396 | 2.44x | 0.163 | 0.396 | 2.44x |
| spearman_matrix_n1000 | 0.106 | 0.920 | 8.68x | 0.106 | 0.920 | 8.68x |
| pointbiserial_n1000 | 0.010 | 0.571 | 54.55x | 0.010 | 0.571 | 54.55x |
| welch_t_n10000 | 0.053 | 0.869 | 16.27x | 0.053 | 0.869 | 16.27x |
| mann_whitney_n10000 | 0.743 | 2.824 | 3.80x | 0.743 | 2.824 | 3.80x |
| chi_square_n10000 | 0.001 | 0.344 | 250.74x | 0.001 | 0.344 | 250.69x |
| fligner_n10000 | 1.404 | 3.224 | 2.30x | 1.487 | 3.224 | 2.17x |
| anderson_ksamp_n10000 | 2.020 | 3.983 | 1.97x | 2.141 | 3.983 | 1.86x |
| spearman_matrix_n10000 | 2.078 | 4.755 | 2.29x | 2.134 | 4.755 | 2.23x |
| pointbiserial_n10000 | 0.100 | 0.630 | 6.28x | 0.100 | 0.630 | 6.28x |
| welch_t_n100000 | 0.519 | 1.234 | 2.38x | 0.519 | 1.234 | 2.38x |
| mann_whitney_n100000 | 7.773 | 28.102 | 3.62x | 7.773 | 28.101 | 3.62x |
| chi_square_n100000 | 0.014 | 0.364 | 26.33x | 0.014 | 0.364 | 26.33x |
| fligner_n100000 | 14.895 | 32.276 | 2.17x | 15.335 | 32.275 | 2.10x |
| anderson_ksamp_n100000 | 15.589 | 43.489 | 2.79x | 15.947 | 43.486 | 2.73x |
| spearman_matrix_n100000 | 18.279 | 51.938 | 2.84x | 18.797 | 51.936 | 2.76x |
| pointbiserial_n100000 | 1.301 | 1.400 | 1.08x | 1.314 | 5.589 | 4.25x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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

_As of 2026-10-30, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.277 | 0.455 | 1.64x | 0.278 | 0.455 | 1.64x |
| polynomial_n10000 | 0.781 | 1.025 | 1.31x | 0.785 | 1.024 | 1.30x |
| kbins_fit_n10000 | 1.461 | 2.598 | 1.78x | 1.461 | 2.598 | 1.78x |
| kbins_n10000 | 0.718 | 4.639 | 6.46x | 0.822 | 4.639 | 5.64x |
| quantile_fit_n10000 | 1.556 | 3.779 | 2.43x | 1.556 | 3.779 | 2.43x |
| quantile_n10000 | 1.840 | 1.700 | 0.92x | 1.843 | 1.700 | 0.92x |
| power_fit_n10000 | 6.091 | 134.902 | 22.15x | 6.094 | 134.898 | 22.14x |
| power_n10000 | 1.549 | 1.504 | 0.97x | 1.552 | 1.504 | 0.97x |
| knn_impute_n10000 | 7.445 | 23.580 | 3.17x | 7.444 | 94.080 | 12.64x |
| label_n10000 | 1.169 | 0.760 | 0.65x | 1.169 | 0.759 | 0.65x |
| normalize_n100000 | 1.538 | 1.846 | 1.20x | 1.957 | 1.846 | 0.94x |
| polynomial_n100000 | 3.732 | 6.974 | 1.87x | 4.569 | 6.974 | 1.53x |
| kbins_fit_n100000 | 14.690 | 8.178 | 0.56x | 15.509 | 8.178 | 0.53x |
| kbins_n100000 | 7.823 | 12.223 | 1.56x | 8.813 | 12.221 | 1.39x |
| quantile_fit_n100000 | 14.992 | 18.083 | 1.21x | 15.751 | 18.082 | 1.15x |
| quantile_n100000 | 17.260 | 14.690 | 0.85x | 17.789 | 14.689 | 0.83x |
| power_fit_n100000 | 65.080 | 887.616 | 13.64x | 67.884 | 887.553 | 13.07x |
| power_n100000 | 14.163 | 9.870 | 0.70x | 14.620 | 9.869 | 0.68x |
| knn_impute_n100000 | 7.444 | 18.717 | 2.51x | 7.444 | 74.756 | 10.04x |
| label_n100000 | 15.178 | 7.272 | 0.48x | 15.504 | 7.272 | 0.47x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-10-31, measured at commit `fd0cf96a1a303125ff7b6b7941a2ca17e5613a1c`._

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
