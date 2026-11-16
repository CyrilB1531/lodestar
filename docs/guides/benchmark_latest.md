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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.597 ms** |   **0.1898 ms** |  **0.0104 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   4.517 ms |   6.2754 ms |  0.3440 ms |  1.74 |    0.11 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.501 ms |   7.8414 ms |  0.4298 ms |  2.12 |    0.14 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.096 ms** |   **0.0553 ms** |  **0.0030 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   9.019 ms |  19.0112 ms |  1.0421 ms |  0.75 |    0.07 |        - |        - |        - |    42825 B |    2,676.56 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  12.046 ms |   6.8267 ms |  0.3742 ms |  1.00 |    0.03 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |  169,441.19 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **52.745 ms** |   **4.3181 ms** |  **0.2367 ms** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |       **74 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  44.509 ms |  51.2368 ms |  2.8085 ms |  0.84 |    0.05 |        - |        - |        - |   802917 B |   10,850.23 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  61.729 ms | 234.4416 ms | 12.8505 ms |  1.17 |    0.21 | 375.0000 | 375.0000 | 375.0000 |  8072041 B |  109,081.64 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **242.787 ms** |   **1.0282 ms** |  **0.0564 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 152.457 ms | 158.7680 ms |  8.7026 ms |  0.63 |    0.03 |        - |        - |        - |   803165 B |    2,355.32 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 227.816 ms | 143.6343 ms |  7.8731 ms |  0.94 |    0.03 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |  155,047.78 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,809.749 ms** |  **1,374.6083 ms** |  **75.3469 ms** | **1.001** |    **0.05** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,794.193 ms |    774.1584 ms |  42.4342 ms | 0.993 |    0.04 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.572 ms |      0.2256 ms |   0.0124 ms | 0.001 |    0.00 |   318.3594 |   318.3594 |   318.3594 |   1.47 MB |        0.20 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,253.863 ms** |  **1,972.1795 ms** | **108.1018 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,202.034 ms |  1,531.7843 ms |  83.9623 ms | 0.993 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      7.079 ms |      1.5423 ms |   0.0845 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,070.006 ms** |  **2,549.1643 ms** | **139.7283 ms** | **1.000** |    **0.02** |  **2000.0000** |  **2000.0000** |  **2000.0000** |   **45.1 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,219.446 ms |  1,215.9715 ms |  66.6515 ms | 1.021 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  42.13 MB |        0.93 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      7.894 ms |      0.2953 ms |   0.0162 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |   5.86 MB |        0.13 |
|              |       |       |                      |               |                |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **30,026.843 ms** | **11,957.2736 ms** | **655.4185 ms** | **1.000** |    **0.03** | **16000.0000** | **15000.0000** | **14000.0000** | **388.67 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 29,660.793 ms |  5,346.1841 ms | 293.0424 ms | 0.988 |    0.02 | 20000.0000 | 19000.0000 | 18000.0000 | 385.01 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     19.551 ms |      2.8899 ms |   0.1584 ms | 0.001 |    0.00 |   312.5000 |   312.5000 |   312.5000 |  23.44 MB |        0.06 |

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method         | Documents | Permutations | Device               | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |--------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |      **84.34 ms** |      **5.580 ms** |     **0.306 ms** |   **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |   4,982.88 ms |  2,311.549 ms |   126.704 ms |  59.08 |    1.31 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |   5,372.52 ms |  8,830.648 ms |   484.038 ms |  63.70 |    4.97 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |      **99.48 ms** |      **1.591 ms** |     **0.087 ms** |   **1.00** |    **0.00** |   **166.6667** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  10,388.92 ms | 11,246.132 ms |   616.438 ms | 104.43 |    5.37 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  10,252.74 ms |  3,128.734 ms |   171.496 ms | 103.06 |    1.50 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |     **846.53 ms** |    **113.082 ms** |     **6.198 ms** |   **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] |  51,158.93 ms | 23,610.747 ms | 1,294.185 ms |  60.44 |    1.38 |  3000.0000 |  3000.0000 | 2000.0000 | 159.99 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] |  51,940.78 ms |  9,296.297 ms |   509.561 ms |  61.36 |    0.65 |  7000.0000 |  4000.0000 | 2000.0000 | 211.57 MB |        7.11 |
|                |           |              |                      |               |               |              |        |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |     **993.07 ms** |     **57.094 ms** |     **3.129 ms** |   **1.00** |    **0.00** |  **2000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 103,214.60 ms | 33,061.379 ms | 1,812.206 ms | 103.94 |    1.61 | 12000.0000 | 10000.0000 | 8000.0000 | 536.13 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 104,095.06 ms |  5,578.882 ms |   305.797 ms | 104.82 |    0.39 | 15000.0000 | 12000.0000 | 8000.0000 | 587.56 MB |       14.00 |

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| Lodestar_StudentCdf             | 2,453.70 ns | 249.864 ns | 13.696 ns | 1.000 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.37 ns |  12.230 ns |  0.670 ns | 0.122 |    0.00 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   336.75 ns |  10.130 ns |  0.555 ns | 0.137 |    0.00 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,541.25 ns | 180.961 ns |  9.919 ns | 3.073 |    0.02 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,646.64 ns | 146.661 ns |  8.039 ns | 2.301 |    0.01 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,197.63 ns |  23.707 ns |  1.299 ns | 0.488 |    0.00 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   774.49 ns |  11.994 ns |  0.657 ns | 0.316 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   131.17 ns |   6.555 ns |  0.359 ns | 0.053 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   134.84 ns |   2.375 ns |  0.130 ns | 0.055 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 2,811.75 ns |  43.033 ns |  2.359 ns | 1.146 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,631.71 ns |  71.587 ns |  3.924 ns | 1.073 |    0.01 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   314.99 ns |  16.890 ns |  0.926 ns | 0.128 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    40.54 ns |   2.654 ns |  0.145 ns | 0.017 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    59.88 ns |   1.117 ns |  0.061 ns | 0.024 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    71.17 ns |   0.640 ns |  0.035 ns | 0.029 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   358.11 ns |  12.703 ns |  0.696 ns | 0.146 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   465.34 ns |  13.207 ns |  0.724 ns | 0.190 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   386.36 ns |   6.675 ns |  0.366 ns | 0.157 |    0.00 |      - |         - |          NA |
| Lodestar_NormalCdf              |    24.14 ns |   3.985 ns |  0.218 ns | 0.010 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    21.93 ns |   0.683 ns |  0.037 ns | 0.009 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.83 ns |   8.526 ns |  0.467 ns | 0.027 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.95 ns |   3.410 ns |  0.187 ns | 0.012 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.53 ns |   4.425 ns |  0.243 ns | 0.012 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   179.84 ns |   1.314 ns |  0.072 ns | 0.073 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method                     | Mean      | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  30.88 ns | 2.360 ns | 0.129 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  14.64 ns | 1.850 ns | 0.101 ns |  0.47 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  44.09 ns | 2.718 ns | 0.149 ns |  1.43 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  75.63 ns | 4.300 ns | 0.236 ns |  2.45 |    0.01 |         - |          NA |
| ChiSquaredSfFractionalDf   | 266.99 ns | 3.803 ns | 0.208 ns |  8.65 |    0.03 |         - |          NA |
| NormalQuantile             |  28.86 ns | 0.825 ns | 0.045 ns |  0.93 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **34.96 μs** |   **5.662 μs** | **0.310 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  55.84 μs |  15.638 μs | 0.857 μs |  1.60 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **42.16 μs** |   **3.237 μs** | **0.177 μs** |  **1.00** |    **0.01** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 101.21 μs |  13.321 μs | 0.730 μs |  2.40 |    0.02 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **333.52 μs** |  **45.795 μs** | **2.510 μs** |  **1.00** |    **0.01** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 363.03 μs |  40.799 μs | 2.236 μs |  1.09 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |            |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **403.01 μs** |  **23.685 μs** | **1.298 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 739.88 μs | 116.422 μs | 6.381 μs |  1.84 |    0.01 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-11-13, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **Poisson**             | **200**        |    **29.69 μs** |     **2.709 μs** |  **0.149 μs** |  **1.00** |    **0.01** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    60.29 μs |     0.571 μs |  0.031 μs |  2.03 |    0.01 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,959.39 μs** |   **489.814 μs** | **26.848 μs** |  **1.00** |    **0.01** | **328.1250** | **328.1250** | **328.1250** | **1097.74 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,542.64 μs | 1,461.022 μs | 80.084 μs |  1.91 |    0.02 | 531.2500 | 531.2500 | 531.2500 | 1878.07 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **Fit**    | **5**         | **304.6 μs** |  **9.06 μs** | **0.50 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **316.1 μs** | **47.24 μs** | **2.59 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **296.5 μs** | **13.45 μs** | **0.74 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **49.78 μs** |      **2.986 μs** |     **0.164 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     108.26 μs |      7.390 μs |     0.405 μs |  2.17 |    0.01 |   1.9531 |   0.1221 |        - |   32.99 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **676.83 μs** |     **98.009 μs** |     **5.372 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   2,042.60 μs |    896.181 μs |    49.123 μs |  3.02 |    0.07 |  97.6563 |  97.6563 |  97.6563 |  370.17 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,724.33 μs** |    **345.753 μs** |    **18.952 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  22,029.87 μs |  9,946.318 μs |   545.191 μs |  3.28 |    0.07 | 281.2500 | 281.2500 | 281.2500 | 2228.14 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **52,234.47 μs** |  **5,814.739 μs** |   **318.725 μs** |  **1.00** |    **0.01** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 140,074.53 μs | 18,579.930 μs | 1,018.429 μs |  2.68 |    0.02 |        - |        - |        - | 8850.45 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **36.894 μs** |   **2.9762 μs** |  **0.1631 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    10.069 μs |   1.3066 μs |  0.0716 μs |  0.27 |    0.00 |   0.5035 |        - |        - |   8.45 KB |        3.19 |
| Hac       | 100        | 4    |    20.117 μs |   2.7778 μs |  0.1523 μs |  0.55 |    0.00 |   0.7629 |        - |        - |  12.63 KB |        4.77 |
| Cluster   | 100        | 4    |     9.144 μs |   0.8031 μs |  0.0440 μs |  0.25 |    0.00 |   0.5798 |        - |        - |   9.51 KB |        3.59 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **461.031 μs** |  **72.3671 μs** |  **3.9667 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   923.417 μs |  25.4276 μs |  1.3938 μs |  2.00 |    0.02 | 124.0234 | 124.0234 | 124.0234 | 472.59 KB |        5.91 |
| Hac       | 10000      | 4    | 1,970.637 μs | 285.6322 μs | 15.6565 μs |  4.27 |    0.04 | 246.0938 | 246.0938 | 246.0938 | 863.57 KB |       10.80 |
| Cluster   | 10000      | 4    |   791.072 μs |  74.3791 μs |  4.0770 μs |  1.72 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.97 KB |        7.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

_As of 2026-11-13, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method        | Shape    | Mean        | Error     | StdDev   | Gen0   | Allocated |
|-------------- |--------- |------------:|----------:|---------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,409.51 μs** | **87.210 μs** | **4.780 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **47.01 μs** |  **2.494 μs** | **0.137 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-11-13, measured at commit `59a57f7257ac13a8a57dcee55e218a7f45b3b458`._

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **Exact**  | **141x141** | **566.9 ms** | **85.47 ms** | **4.69 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **450.4 ms** | **42.01 ms** | **2.30 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method           | Size | Mean          | Error       | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|------------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.138 μs** |   **0.3582 μs** |  **0.0196 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,575.772 μs | 164.3304 μs |  9.0075 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     20.398 μs |   5.7462 μs |  0.3150 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     92.784 μs |  24.7383 μs |  1.3560 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.581 μs** |   **5.8178 μs** |  **0.3189 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,203.936 μs | 967.6309 μs | 53.0391 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    506.312 μs | 206.5260 μs | 11.3204 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,289.726 μs |  55.3177 μs |  3.0322 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **133.8 μs** |    **17.08 μs** |  **0.94 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   926.5 μs |    97.33 μs |  5.34 μs |  6.93 |    0.05 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,303.1 μs** |   **271.53 μs** | **14.88 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,058.4 μs | 1,015.13 μs | 55.64 μs |  5.42 |    0.06 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **Lodestar_Ols** | **100**        |    **37.41 μs** |   **4.993 μs** |  **0.274 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   264.28 μs |  14.236 μs |  0.780 μs |  7.06 |    0.05 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    32.60 μs |   7.909 μs |  0.434 μs |  0.87 |    0.01 |   2.1667 |   0.0610 |        - |   35.43 KB |       13.38 |
|              |            |             |            |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **492.37 μs** |  **13.320 μs** |  **0.730 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,552.38 μs | 298.807 μs | 16.379 μs |  9.25 |    0.03 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,686.39 μs | 391.464 μs | 21.457 μs |  3.43 |    0.04 | 441.4063 | 441.4063 | 441.4063 | 1765.64 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method                       | Mean           | Error         | StdDev     | Ratio      | RatioSD  | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|-----------:|-----------:|---------:|----------:|------------:|
| NormalQuantile               |      0.0398 ns |     0.0128 ns |  0.0007 ns |       1.00 |     0.02 |         - |          NA |
| NormalQuantileFarTail        |      0.0483 ns |     0.0267 ns |  0.0015 ns |       1.21 |     0.04 |         - |          NA |
| StudentQuantile              | 14,366.3245 ns | 1,697.0741 ns | 93.0224 ns | 360,663.26 | 5,896.10 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,319.1729 ns |    64.3563 ns |  3.5276 ns |  58,222.30 |   897.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **KruskalWallisTest**         | **10000**      | **False** |  **1,483.3 μs** |   **172.35 μs** |   **9.45 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    416.7 μs |    14.07 μs |   0.77 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    594.0 μs |    70.43 μs |   3.86 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,260.9 μs |   120.96 μs |   6.63 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    677.4 μs |    92.97 μs |   5.10 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **622.9 μs** |    **50.38 μs** |   **2.76 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    241.8 μs |    29.74 μs |   1.63 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    521.2 μs |   168.73 μs |   9.25 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    768.1 μs |    22.17 μs |   1.22 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    381.5 μs |    21.34 μs |   1.17 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **14,825.3 μs** | **1,439.21 μs** |  **78.89 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,573.3 μs | 1,117.08 μs |  61.23 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,658.2 μs |   931.82 μs |  51.08 μs | 515.6250 | 515.6250 | 515.6250 |  5062698 B |
| WilcoxonPooledDifferences | 100000     | False | 16,781.2 μs |   763.67 μs |  41.86 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,043.7 μs |   303.82 μs |  16.65 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,081.4 μs** |   **199.79 μs** |  **10.95 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,793.6 μs |   115.33 μs |   6.32 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,265.3 μs | 1,316.19 μs |  72.14 μs | 500.0000 | 500.0000 | 500.0000 |  5062686 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,267.1 μs | 2,090.09 μs | 114.56 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,745.7 μs |   234.10 μs |  12.83 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **Fit**    | **100**        | **Nonrobust**  |    **37.23 μs** |   **2.834 μs** |  **0.155 μs** |   **0.1221** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **10.18 μs** |   **1.455 μs** |  **0.080 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.11 μs** |   **0.456 μs** |  **0.025 μs** |   **0.5035** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.77 μs** |   **0.409 μs** |  **0.022 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.97 μs** |   **0.542 μs** |  **0.030 μs** |   **0.5493** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **456.01 μs** |  **30.294 μs** |  **1.661 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **934.36 μs** | **237.901 μs** | **13.040 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **908.45 μs** | **103.429 μs** |  **5.669 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,046.26 μs** |  **47.154 μs** |  **2.585 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,073.61 μs** | **261.144 μs** | **14.314 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **LodestarAutocorrelation**        | **200**        |   **4.455 μs** | **0.3933 μs** | **0.0216 μs** |  **1.00** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.537 μs | 0.4374 μs | 0.0240 μs |  2.59 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.967 μs | 0.1925 μs | 0.0106 μs |  1.11 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.152 μs | 0.2618 μs | 0.0143 μs |  2.73 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.063 μs | 0.3824 μs | 0.0210 μs |  1.14 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.663 μs | 0.3036 μs | 0.0166 μs |  2.62 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **49.030 μs** | **0.8364 μs** | **0.0458 μs** |  **1.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 121.116 μs | 7.2444 μs | 0.3971 μs |  2.47 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.408 μs | 1.2843 μs | 0.0704 μs |  1.01 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.501 μs | 3.0305 μs | 0.1661 μs |  2.48 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.828 μs | 5.1947 μs | 0.2847 μs |  1.02 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 120.957 μs | 1.3980 μs | 0.0766 μs |  2.47 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.293 μs** |  **1.2283 μs** | **0.0673 μs** |  **1.00** |    **0.01** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  28.407 μs |  1.4970 μs | 0.0821 μs |  3.90 |    0.03 |   4.0588 |   0.5493 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |  26.305 μs |  2.1048 μs | 0.1154 μs |  3.61 |    0.03 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.924 μs |  0.3079 μs | 0.0169 μs |  0.54 |    0.00 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.001 μs |  0.7179 μs | 0.0393 μs |  0.55 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.500 μs |  0.3827 μs | 0.0210 μs |  0.21 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.765 μs |  0.3444 μs | 0.0189 μs |  0.38 |    0.00 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **109.577 μs** | **20.1458 μs** | **1.1043 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 917.254 μs | 88.7950 μs | 4.8672 μs |  8.37 |    0.08 | 249.0234 | 249.0234 | 249.0234 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 282.597 μs | 18.0178 μs | 0.9876 μs |  2.58 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  96.291 μs | 12.3345 μs | 0.6761 μs |  0.88 |    0.01 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.039 μs |  3.3435 μs | 0.1833 μs |  0.89 |    0.01 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  13.674 μs |  0.1389 μs | 0.0076 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  27.350 μs |  3.7269 μs | 0.2043 μs |  0.25 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method              | SampleSize | Mean            | Error           | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,155.2 ns** |       **734.17 ns** |      **40.24 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,272.5 ns |     2,930.90 ns |     160.65 ns |    6.22 |    0.04 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,197.8 ns |       431.62 ns |      23.66 ns |    0.36 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,125.3 ns |     2,173.00 ns |     119.11 ns |    3.76 |    0.03 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        111.6 ns |         7.82 ns |       0.43 ns |    0.02 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        218.7 ns |        35.15 ns |       1.93 ns |    0.04 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |                 |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **53,653.5 ns** |    **11,232.11 ns** |     **615.67 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    140,856.1 ns |    11,617.25 ns |     636.78 ns |   2.626 |    0.03 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    686,785.2 ns |    23,711.77 ns |   1,299.72 ns |  12.801 |    0.13 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,516,450.0 ns | 3,452,976.98 ns | 189,269.32 ns | 233.303 |    3.83 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        110.8 ns |         3.48 ns |       0.19 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        214.4 ns |        20.14 ns |       1.10 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **15.90 μs** |   **4.214 μs** | **0.231 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **37.55 μs** |   **5.097 μs** | **0.279 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **53.42 μs** |   **7.442 μs** | **0.408 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **153.60 μs** |   **3.767 μs** | **0.206 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **212.44 μs** |  **16.248 μs** | **0.891 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **560.73 μs** |  **62.678 μs** | **3.436 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **708.62 μs** | **117.986 μs** | **6.467 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,246.72 μs** |  **16.537 μs** | **0.906 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method       | SampleSize | Mean         | Error         | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **46.17 μs** |      **1.149 μs** |   **0.063 μs** |  **1.00** |    **0.00** |    **0.1831** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     19.36 μs |      7.640 μs |   0.419 μs |  0.42 |    0.01 |    3.6926 |    0.1831 |         - |    58.23 KB |       16.98 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **139.05 μs** |     **10.989 μs** |   **0.602 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    115.62 μs |     19.244 μs |   1.055 μs |  0.83 |    0.01 |   33.2031 |   14.1602 |         - |   508.31 KB |       29.06 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **966.96 μs** |     **56.173 μs** |   **3.079 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |    **158.5 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,566.69 μs |  2,800.961 μs | 153.530 μs |  1.62 |    0.14 | 1023.4375 | 1021.4844 | 1021.4844 |  5027.18 KB |       31.72 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,344.06 μs** |  **2,037.593 μs** | **111.687 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 18,177.38 μs | 16,903.446 μs | 926.535 μs |  1.95 |    0.09 |  937.5000 |  937.5000 |  937.5000 | 50024.07 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method | SampleSize | Covariates | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **434.3 μs** |  **85.47 μs** |  **4.68 μs** |   **8.3008** |   **0.9766** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **1,910.0 μs** |  **78.47 μs** |  **4.30 μs** |  **11.7188** |   **1.9531** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,202.5 μs** | **305.29 μs** | **16.73 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **16,839.8 μs** | **900.33 μs** | **49.35 μs** | **281.2500** | **250.0000** | **250.0000** | **1737.22 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **15.85 μs** |     **3.018 μs** |  **0.165 μs** |  **1.00** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     14.38 μs |     1.662 μs |  0.091 μs |  0.91 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.65 μs |     1.593 μs |  0.087 μs |  1.37 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **58.33 μs** |     **8.699 μs** |  **0.477 μs** |  **1.00** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.41 μs |     3.966 μs |  0.217 μs |  0.40 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     48.01 μs |    12.028 μs |  0.659 μs |  0.82 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **8,579.72 μs** |   **462.383 μs** | **25.345 μs** |  **1.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  8,623.41 μs |   278.457 μs | 15.263 μs |  1.01 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  9,871.89 μs |   337.663 μs | 18.508 μs |  1.15 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **15,611.86 μs** | **1,625.512 μs** | **89.100 μs** |  **1.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 12,501.89 μs | 1,161.080 μs | 63.643 μs |  0.80 |  828.1250 |  812.5000 | 671.8750 | 5660.21 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 16,221.25 μs |   818.817 μs | 44.882 μs |  1.04 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method                         | Samples | Clusters | Mean       | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.121 ms** |   **0.2661 ms** | **0.0146 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.146 ms |   0.1402 ms | 0.0077 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  20.579 ms |   0.9507 ms | 0.0521 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.024 ms** |   **0.1044 ms** | **0.0057 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.177 ms |   0.3044 ms | 0.0167 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 182.359 ms | 113.2885 ms | 6.2097 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method              | Length | Mean            | Error           | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |------- |----------------:|----------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar**            | **12**     |        **887.1 ns** |        **47.03 ns** |       **2.58 ns** |  **1.00** |    **0.00** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,805.4 ns |        41.47 ns |       2.27 ns |  2.04 |    0.01 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **70,168.4 ns** |    **20,278.99 ns** |   **1,111.56 ns** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    151,754.2 ns |    10,612.12 ns |     581.69 ns |  2.16 |    0.03 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,347,733.0 ns** |   **700,199.14 ns** |  **38,380.28 ns** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,480,160.8 ns | 2,113,560.89 ns | 115,851.40 ns |  2.87 |    0.03 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method      | Count  | Mean       | Error       | StdDev    | Allocated |
|------------ |------- |-----------:|------------:|----------:|----------:|
| **SearchTop10** | **10000**  |   **538.9 μs** |    **35.55 μs** |   **1.95 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,811.1 μs** | **3,703.48 μs** | **203.00 μs** |   **1.64 KB** |

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-HUYUUP : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-HUYUUP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **284.15 μs** |    **15.721 μs** |    **17.474 μs** |    **281.63 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    265.74 μs |    10.322 μs |    11.045 μs |    266.85 μs |  0.94 |    0.07 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |     88.41 μs |     4.532 μs |     5.038 μs |     87.14 μs |  0.31 |    0.02 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |    432.11 μs |    22.760 μs |    25.298 μs |    431.28 μs |  1.53 |    0.12 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,669.49 μs |    79.465 μs |    88.326 μs |  1,705.99 μs |  5.90 |    0.46 |        - |        - |        - |  331.74 KB |        1.99 |
| MlNet_ReplaceMissingValues_Read | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,185.70 μs |    75.661 μs |    84.097 μs |  1,225.86 μs |  4.19 |    0.38 |        - |        - |        - |  296.71 KB |        1.78 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    183.91 μs |    17.062 μs |     0.935 μs |    183.69 μs |  1.00 |    0.01 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     69.07 μs |     1.995 μs |     0.109 μs |     69.11 μs |  0.38 |    0.00 |   0.9766 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.86 μs |     1.354 μs |     0.074 μs |     16.85 μs |  0.09 |    0.00 |   1.8921 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,016.00 μs | 2,584.235 μs |   141.651 μs |  1,014.21 μs |  5.52 |    0.67 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    908.54 μs |   616.504 μs |    33.793 μs |    905.98 μs |  4.94 |    0.16 |  48.8281 |   7.8125 |        - |  794.94 KB |        4.79 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    265.42 μs |   230.296 μs |    12.623 μs |    260.94 μs |  1.44 |    0.06 |  19.0430 |  18.5547 |   0.4883 |  296.91 KB |        1.79 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-HUYUUP** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,377.87 μs** |   **677.035 μs** |   **752.523 μs** |  **5,981.91 μs** |  **1.02** |    **0.20** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,912.59 μs |   593.903 μs |   635.469 μs |  4,838.23 μs |  0.93 |    0.18 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |    653.39 μs |    10.628 μs |    11.813 μs |    654.33 μs |  0.12 |    0.02 |        - |        - |        - |  625.93 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,488.84 μs |    46.848 μs |    46.011 μs |  4,498.43 μs |  0.85 |    0.12 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,116.90 μs | 6,066.463 μs | 6,986.148 μs | 12,422.02 μs |  2.49 |    1.35 |        - |        - |        - |   550.2 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | Job-HUYUUP | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,233.03 μs | 3,791.813 μs | 4,214.590 μs | 12,747.30 μs |  1.94 |    0.83 |        - |        - |        - |  530.17 KB |        0.16 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,839.34 μs | 5,459.734 μs |   299.266 μs |  4,009.32 μs |  1.00 |    0.10 | 992.1875 | 992.1875 | 992.1875 | 3291.63 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,153.64 μs |   394.538 μs |    21.626 μs |  3,151.15 μs |  0.82 |    0.06 |  89.8438 |  89.8438 |  89.8438 |  314.47 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    435.44 μs |   109.701 μs |     6.013 μs |    436.35 μs |  0.11 |    0.01 | 129.8828 | 129.8828 | 129.8828 |  626.25 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,560.18 μs | 1,158.821 μs |    63.519 μs |  1,560.15 μs |  0.41 |    0.03 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,139.60 μs | 1,502.406 μs |    82.352 μs |  4,172.54 μs |  1.08 |    0.08 |  31.2500 |  15.6250 |        - |  566.92 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,494.01 μs |   752.255 μs |    41.234 μs |  2,493.37 μs |  0.65 |    0.05 |  27.3438 |  11.7188 |        - |  455.47 KB |        0.14 |

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **39.99 ns** |      **6.806 ns** |   **0.373 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    129.06 ns |      5.917 ns |   0.324 ns |  3.23 |    0.03 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **153.55 ns** |    **102.598 ns** |   **5.624 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,581.17 ns |    497.790 ns |  27.286 ns | 10.31 |    0.37 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **321.77 ns** |     **26.843 ns** |   **1.471 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  9,140.55 ns |  4,371.640 ns | 239.624 ns | 28.41 |    0.65 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **49,636.13 ns** | **10,616.680 ns** | **581.936 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 47,355.77 ns |  8,285.972 ns | 454.182 ns |  0.95 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method            | Q | Element   | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.848 μs** | **1.5587 μs** | **0.0854 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **3.955 μs** | **0.8218 μs** | **0.0450 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.730 μs** | **0.6654 μs** | **0.0365 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.923 μs** | **2.3293 μs** | **0.1277 μs** | **0.0076** |     **192 B** |

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method                   | Length | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **377.0 ns** |     **59.64 ns** |     **3.27 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   7,044.4 ns |    231.30 ns |    12.68 ns | 18.69 |    0.14 |         - |          NA |
|                          |        |              |              |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,816.4 ns** |    **457.77 ns** |    **25.09 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 734,559.7 ns | 30,171.34 ns | 1,653.79 ns | 32.19 |    0.07 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **24.03 ms** | **0.999 ms** | **0.055 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.63 ms** | **0.818 ms** | **0.045 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-YQPELG : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-YQPELG** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **111.446 μs** |     **2.9062 μs** |     **2.8543 μs** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    103.921 μs |     3.4168 μs |     3.5088 μs |  0.93 |    0.04 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,280.166 μs |    26.2607 μs |    30.2419 μs | 20.47 |    0.55 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    107.524 μs |    20.0442 μs |    22.2791 μs |  0.97 |    0.20 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     65.267 μs |    11.6199 μs |    11.4123 μs |  0.59 |    0.10 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     68.102 μs |    10.3284 μs |    11.0513 μs |  0.61 |    0.10 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |     95.959 μs |     7.3500 μs |     7.2187 μs |  0.86 |    0.07 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |    422.466 μs |    14.8751 μs |    17.1302 μs |  3.79 |    0.17 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,301.150 μs |    30.6763 μs |    34.0967 μs | 20.66 |    0.57 |        - |        - |        - |  296984 B |        3.64 |
| MlNet_NormalizeRobustScaling_Read | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 1000     |  6,691.950 μs |   591.3584 μs |   607.2816 μs | 60.08 |    5.48 |        - |        - |        - |  442056 B |        5.42 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.261 μs |     9.3254 μs |     0.5112 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     40.093 μs |     8.3229 μs |     0.4562 μs |  0.87 |    0.01 |   4.7607 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    462.704 μs |     1.5234 μs |     0.0835 μs | 10.00 |    0.10 |   9.2773 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.062 μs |     3.2054 μs |     0.1757 μs |  1.02 |    0.01 |   4.7607 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     10.476 μs |     0.3585 μs |     0.0196 μs |  0.23 |    0.00 |   0.0458 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      7.716 μs |     0.2318 μs |     0.0127 μs |  0.17 |    0.00 |   0.0153 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.672 μs |     4.7630 μs |     0.2611 μs |  0.25 |    0.01 |   1.6174 |   0.1068 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,587.073 μs | 4,502.7186 μs |   246.8092 μs | 34.31 |    4.63 |  83.4961 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    976.988 μs |   709.1914 μs |    38.8732 μs | 21.12 |    0.76 |  39.0625 |   1.9531 |        - |  650629 B |       8.043 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,607.779 μs | 6,511.7540 μs |   356.9312 μs | 34.76 |    6.69 |  31.2500 |        - |        - |  518032 B |       6.404 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-YQPELG** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,528.851 μs** |    **15.8446 μs** |    **17.6112 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,421.385 μs |    14.1548 μs |    16.3007 μs |  0.93 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,125.625 μs |    67.5472 μs |    72.2747 μs |  9.89 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,506.561 μs |    26.8002 μs |    29.7883 μs |  0.99 |    0.02 |        - |        - |        - | 1601560 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    420.893 μs |     7.9266 μs |     9.1283 μs |  0.28 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    314.956 μs |     9.0975 μs |    10.1119 μs |  0.21 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |    404.635 μs |     9.3065 μs |     9.9578 μs |  0.26 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,144.045 μs | 1,721.1343 μs | 1,767.4782 μs |  4.02 |    1.12 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 18,883.603 μs | 7,510.6894 μs | 8,649.3211 μs | 12.35 |    5.52 |        - |        - |        - |  478864 B |       0.299 |
| MlNet_NormalizeRobustScaling_Read | Job-YQPELG | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,129.860 μs |   189.6923 μs |   210.8425 μs | 12.51 |    0.19 |        - |        - |        - | 4278440 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,474.616 μs | 1,460.6582 μs |    80.0636 μs |  1.00 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1601359 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,336.048 μs |   851.0157 μs |    46.6470 μs |  0.91 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 14,715.596 μs | 4,078.3906 μs |   223.5503 μs | 10.00 |    0.48 | 890.6250 | 890.6250 | 890.6250 | 3201205 B |       1.999 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,389.733 μs | 2,088.4296 μs |   114.4739 μs |  0.94 |    0.08 | 330.0781 | 330.0781 | 330.0781 | 1601039 B |       1.000 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    306.143 μs |     6.8890 μs |     0.3776 μs |  0.21 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    242.412 μs |    13.0954 μs |     0.7178 μs |  0.16 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    408.863 μs |   179.1534 μs |     9.8200 μs |  0.28 |    0.01 | 128.9063 | 124.0234 | 124.0234 |  560075 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,634.206 μs | 2,321.0749 μs |   127.2259 μs |  1.11 |    0.09 |  41.0156 |   6.8359 |        - |  702655 B |       0.439 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,398.803 μs | 2,190.2800 μs |   120.0566 μs |  2.99 |    0.15 |  31.2500 |        - |        - |  595734 B |       0.372 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 19,439.293 μs | 1,521.7828 μs |    83.4141 μs | 13.21 |    0.61 | 250.0000 | 125.0000 |        - | 4300870 B |       2.686 |

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

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method    | Samples | Mean      | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.29 ms** |  **1.123 ms** | **0.062 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **192.73 ms** | **13.304 ms** | **0.729 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **97.628 μs** |    **89.136 μs** |   **4.8859 μs** |  **1.00** |    **0.06** |  **16.7236** |   **4.7607** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    178.919 μs |     7.041 μs |   0.3859 μs |  1.84 |    0.08 |  19.0430 |   6.8359 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.667 μs |     3.270 μs |   0.1793 μs |  0.05 |    0.00 |   2.3956 |   0.2594 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    335.940 μs |    71.484 μs |   3.9183 μs |  3.45 |    0.15 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,662.503 μs |   187.354 μs |  10.2695 μs | 58.10 |    2.47 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     61.443 μs |     6.318 μs |   0.3463 μs |  0.63 |    0.03 |   0.7324 |   0.2441 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,108.965 μs |   132.231 μs |   7.2480 μs | 11.38 |    0.49 |   1.9531 |        - |        - |   34.26 KB |        0.12 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,072.033 μs** |   **473.182 μs** |  **25.9367 μs** |  **1.00** |    **0.03** | **484.3750** | **470.7031** | **460.9375** | **2738.75 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,328.969 μs |   844.175 μs |  46.2721 μs |  2.17 |    0.06 | 550.7813 | 535.1563 | 527.3438 | 3130.11 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     90.238 μs |    48.474 μs |   2.6570 μs |  0.08 |    0.00 |  59.2041 |  59.2041 |  59.2041 |   391.2 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    328.195 μs |   102.635 μs |   5.6258 μs |  0.31 |    0.01 |   2.4414 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 44,246.311 μs | 4,766.400 μs | 261.2624 μs | 41.29 |    0.88 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     62.340 μs |    24.561 μs |   1.3463 μs |  0.06 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,493.136 μs |   370.865 μs |  20.3284 μs |  6.99 |    0.15 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method  | Words | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **641.1 μs** |  **48.26 μs** |  **2.65 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,793.7 μs** | **107.12 μs** |  **5.87 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,874.0 μs** | **587.89 μs** | **32.22 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **11.48 ms** | **0.636 ms** | **0.035 ms** |         **-** |
| **TopTwo** | **100**     | **87.62 ms** | **2.983 ms** | **0.164 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-DJSVZW : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-DJSVZW** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **53.17 μs** |      **0.558 μs** |      **0.548 μs** |     **53.08 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |     88.09 μs |      1.335 μs |      1.484 μs |     87.89 μs |   1.66 |    0.03 |        - |        - |        - |  117.88 KB |        3.68 |
| Lodestar_Discretize         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    199.70 μs |     36.073 μs |     41.541 μs |    174.03 μs |   3.76 |    0.76 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    291.74 μs |     16.577 μs |     18.425 μs |    287.68 μs |   5.49 |    0.34 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    998.74 μs |     11.254 μs |     12.042 μs |    996.60 μs |  18.79 |    0.29 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,556.34 μs |     10.271 μs |     11.416 μs |  1,556.21 μs |  29.27 |    0.36 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,104.14 μs |     13.448 μs |     13.810 μs |  1,105.48 μs |  20.77 |    0.33 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |    223.82 μs |     16.777 μs |     19.321 μs |    213.26 μs |   4.21 |    0.36 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,853.17 μs |     12.400 μs |     13.782 μs |  1,850.05 μs |  34.86 |    0.43 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     | 13,642.58 μs |  5,535.863 μs |  6,375.108 μs |  9,124.48 μs | 256.62 |  117.09 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,183.77 μs |     77.943 μs |     86.633 μs |  1,223.56 μs |  22.27 |    1.60 |        - |        - |        - |   330.8 KB |       10.34 |
| MlNet_NormalizeBinning_Read | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,046.18 μs |    136.106 μs |    151.282 μs |  3,064.11 μs |  57.30 |    2.83 |        - |        - |        - |  349.24 KB |       10.92 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.98 μs |      2.127 μs |      0.117 μs |     13.99 μs |   1.00 |    0.01 |   1.9073 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.15 μs |     14.235 μs |      0.780 μs |     83.02 μs |   5.95 |    0.06 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    133.15 μs |     22.039 μs |      1.208 μs |    133.49 μs |   9.52 |    0.10 |  49.8047 |  49.8047 |  49.8047 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     74.74 μs |      7.919 μs |      0.434 μs |     74.81 μs |   5.35 |    0.05 |   1.8311 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     57.65 μs |     54.304 μs |      2.977 μs |     56.61 μs |   4.12 |    0.19 |   1.9531 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    463.12 μs |     21.876 μs |      1.199 μs |    462.72 μs |  33.13 |    0.25 |   1.4648 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    169.54 μs |    164.748 μs |      9.030 μs |    172.15 μs |  12.13 |    0.57 |   6.5918 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    141.05 μs |      0.977 μs |      0.054 μs |    141.07 μs |  10.09 |    0.07 |   1.7090 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    697.15 μs |     91.963 μs |      5.041 μs |    699.48 μs |  49.87 |    0.48 |   7.8125 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  8,761.50 μs |  1,032.969 μs |     56.620 μs |  8,731.03 μs | 626.71 |    5.73 |        - |        - |        - |  103.16 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    259.57 μs |    135.944 μs |      7.452 μs |    255.97 μs |  18.57 |    0.48 |  19.0430 |  18.5547 |   0.4883 |  297.75 KB |        9.52 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    975.70 μs |  1,059.195 μs |     58.058 μs |    970.04 μs |  69.79 |    3.63 |  42.9688 |   5.8594 |        - |  704.25 KB |       22.52 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-DJSVZW** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **460.38 μs** |     **10.221 μs** |     **10.039 μs** |    **456.05 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,637.32 μs |     19.378 μs |     21.539 μs |  1,639.15 μs |   3.56 |    0.09 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,518.50 μs |     23.095 μs |     25.670 μs |  3,518.15 μs |   7.65 |    0.17 |        - |        - |        - | 3125.46 KB |        4.99 |
| Lodestar_Discretize_64Bins  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,448.37 μs |     22.778 μs |     26.231 μs |  4,441.06 μs |   9.67 |    0.21 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,300.77 μs |  4,962.750 μs |  5,715.111 μs | 11,623.40 μs |  24.56 |   12.14 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,483.28 μs |     47.667 μs |     48.951 μs | 10,486.74 μs |  22.78 |    0.48 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,343.89 μs |  5,050.807 μs |  5,816.517 μs |  5,792.13 μs |  20.30 |   12.35 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,007.81 μs |     10.101 μs |     11.227 μs |  3,006.14 μs |   6.54 |    0.14 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,546.62 μs |  3,790.332 μs |  4,364.952 μs | 14,497.01 μs |  38.13 |    9.29 |        - |        - |        - | 2658.55 KB |        4.25 |
| Lodestar_KnnImpute          | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,699.08 μs |     34.123 μs |     36.512 μs |  9,692.68 μs |  21.08 |    0.44 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,505.08 μs |  4,013.913 μs |  4,461.454 μs |  8,377.19 μs |  20.66 |    9.46 |        - |        - |        - |  531.29 KB |        0.85 |
| MlNet_NormalizeBinning_Read | Job-DJSVZW | 1               | 20             | Throughput  | 1            | 5           | 20000    | 29,885.86 μs | 12,192.994 μs | 14,041.470 μs | 27,914.40 μs |  64.94 |   29.82 |        - |        - |        - |  2078.6 KB |        3.32 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    660.87 μs |     26.346 μs |      1.444 μs |    660.38 μs |   1.00 |    0.00 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    632.35 μs |    154.724 μs |      8.481 μs |    630.36 μs |   0.96 |    0.01 | 267.5781 | 267.5781 | 267.5781 | 2346.23 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,135.85 μs |    789.878 μs |     43.296 μs |  3,111.55 μs |   4.75 |    0.06 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,870.37 μs |    286.532 μs |     15.706 μs |  1,877.23 μs |   2.83 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,743.64 μs |    236.081 μs |     12.940 μs |  5,745.10 μs |   8.69 |    0.02 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,939.29 μs |    771.736 μs |     42.301 μs | 10,924.17 μs |  16.55 |    0.06 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,843.30 μs |    165.864 μs |      9.092 μs |  5,838.15 μs |   8.84 |    0.02 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,208.44 μs |    313.471 μs |     17.182 μs |  3,213.44 μs |   4.85 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,186.89 μs |  2,178.968 μs |    119.437 μs | 13,252.71 μs |  19.95 |    0.16 | 734.3750 | 734.3750 | 734.3750 |  2660.9 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  9,145.60 μs |    361.606 μs |     19.821 μs |  9,146.97 μs |  13.84 |    0.04 |        - |        - |        - |  104.66 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,419.30 μs |    199.420 μs |     10.931 μs |  2,425.56 μs |   3.66 |    0.02 |  27.3438 |  11.7188 |        - |  458.61 KB |        0.73 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,679.33 μs |  1,003.870 μs |     55.025 μs | 10,671.35 μs |  16.16 |    0.08 | 265.6250 | 218.7500 | 187.5000 |  2124.3 KB |        3.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### compare-cdist

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### compare-cluster-weighted

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.256 | 3.968 | 0.75x | 5.262 | 7.928 | 1.51x |
| kmeans_restarts_10000 | 21.611 | 10.361 | 0.48x | 21.619 | 20.660 | 0.96x |
| kmeans_weighted_100000 | 51.951 | 28.135 | 0.54x | 52.411 | 47.602 | 0.91x |
| kmeans_restarts_100000 | 208.767 | 71.292 | 0.34x | 209.810 | 131.818 | 0.63x |
| dbscan_weighted_5000 | 8.025 | 31.953 | 3.98x | 8.522 | 31.952 | 3.75x |
| dbscan_weighted_20000 | 98.953 | 191.543 | 1.94x | 100.411 | 191.524 | 1.91x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.826 | 127.483 | 154.32x | 0.826 | 127.471 | 154.33x |
| ridge_robust_1000 | 0.899 | 114.767 | 127.61x | 0.899 | 114.759 | 127.60x |
| lasso_1000 | 6.315 | 1079.401 | 170.94x | 6.314 | 1079.329 | 170.93x |
| ph_test_1000 | 0.130 | 20.998 | 160.98x | 0.130 | 20.996 | 160.96x |
| predict_survival_1000 | 0.032 | 1.886 | 59.82x | 0.032 | 1.886 | 59.82x |
| time_varying_1000 | 68.208 | 346.485 | 5.08x | 68.211 | 346.466 | 5.08x |
| stratified_weighted_10000 | 8.689 | 712.919 | 82.05x | 8.733 | 712.853 | 81.63x |
| ridge_robust_10000 | 9.888 | 1097.151 | 110.96x | 10.161 | 1097.069 | 107.96x |
| lasso_10000 | 56.515 | 7094.977 | 125.54x | 56.560 | 7094.562 | 125.43x |
| ph_test_10000 | 2.022 | 206.239 | 102.00x | 2.102 | 206.226 | 98.13x |
| predict_survival_10000 | 0.034 | 1.902 | 56.71x | 0.034 | 1.902 | 56.71x |
| time_varying_10000 | 2494.502 | 5175.332 | 2.07x | 2495.869 | 5174.936 | 2.07x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 3.925 | 48.182 | 12.28x | 3.925 | 190.042 | 48.42x |
| cv_minmax_n1000 | 3.031 | 5.516 | 1.82x | 3.030 | 22.007 | 7.26x |
| cv_plus_gamma_n1000 | 4.529 | 48.670 | 10.75x | 4.529 | 192.288 | 42.46x |
| bootstrap_plus_n1000 | 14.552 | 49.046 | 3.37x | 14.551 | 195.114 | 13.41x |
| cv_plus_n10000 | 40.260 | 122.372 | 3.04x | 40.260 | 460.598 | 11.44x |
| cv_minmax_n10000 | 30.536 | 25.461 | 0.83x | 30.534 | 101.588 | 3.33x |
| cv_plus_gamma_n10000 | 58.127 | 127.680 | 2.20x | 58.124 | 470.508 | 8.09x |
| bootstrap_plus_n10000 | 146.682 | 125.022 | 0.85x | 146.673 | 474.391 | 3.23x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.387 | 3.416 | 8.82x | 0.387 | 3.416 | 8.82x |
| glm_gamma_n1000 | 0.469 | 4.233 | 9.02x | 0.469 | 4.233 | 9.02x |
| glm_poisson_exposure_n1000 | 0.395 | 4.003 | 10.13x | 0.395 | 4.002 | 10.13x |
| mnlogit_n1000 | 0.823 | 17.950 | 21.82x | 0.822 | 17.948 | 21.82x |
| glm_negative_binomial_n10000 | 3.491 | 14.034 | 4.02x | 3.499 | 13.972 | 3.99x |
| glm_gamma_n10000 | 4.036 | 13.012 | 3.22x | 4.042 | 13.010 | 3.22x |
| glm_poisson_exposure_n10000 | 4.256 | 12.893 | 3.03x | 4.263 | 12.891 | 3.02x |
| mnlogit_n10000 | 8.293 | 94.951 | 11.45x | 8.299 | 94.945 | 11.44x |
| glm_negative_binomial_n100000 | 36.053 | 150.252 | 4.17x | 36.508 | 587.016 | 16.08x |
| glm_gamma_n100000 | 41.974 | 169.623 | 4.04x | 42.319 | 677.136 | 16.00x |
| glm_poisson_exposure_n100000 | 55.613 | 127.840 | 2.30x | 55.514 | 510.038 | 9.19x |
| mnlogit_n100000 | 82.749 | 909.286 | 10.99x | 82.956 | 1593.999 | 19.21x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 121.2 | 27.5 | 4.41x C# faster |
| latin | 32 | 178.1 | 89.1 | 2.00x C# faster |
| latin | 128 | 447.7 | 398.0 | 1.12x C# faster |
| latin | 512 | 4467.0 | 4894.7 | 1.10x Py faster |
| cjk | 8 | 148.7 | 25.3 | 5.87x C# faster |
| cjk | 32 | 338.9 | 226.6 | 1.50x C# faster |
| cjk | 128 | 1836.2 | 1362.1 | 1.35x C# faster |
| cjk | 512 | 14840.9 | 8780.7 | 1.69x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.640 | 55.381 | 33.77x | 1.640 | 55.377 | 33.77x |
| liml_robust_n1000 | 1.777 | 55.528 | 31.25x | 1.777 | 55.523 | 31.25x |
| gmm_robust_n1000 | 1.650 | 54.584 | 33.09x | 1.650 | 54.578 | 33.09x |
| 2sls_kernel_n1000 | 8.455 | 58.341 | 6.90x | 8.543 | 58.338 | 6.83x |
| 2sls_clustered_n1000 | 1.587 | 56.883 | 35.85x | 1.587 | 56.882 | 35.85x |
| 2sls_robust_n10000 | 16.691 | 238.368 | 14.28x | 17.140 | 476.601 | 27.81x |
| liml_robust_n10000 | 18.327 | 237.957 | 12.98x | 18.935 | 475.089 | 25.09x |
| gmm_robust_n10000 | 16.515 | 230.760 | 13.97x | 17.012 | 461.487 | 27.13x |
| 2sls_kernel_n10000 | 90.556 | 258.650 | 2.86x | 91.038 | 517.136 | 5.68x |
| 2sls_clustered_n10000 | 15.805 | 252.968 | 16.01x | 16.175 | 505.956 | 31.28x |
| 2sls_robust_n100000 | 183.886 | 2492.252 | 13.55x | 185.886 | 7719.681 | 41.53x |
| liml_robust_n100000 | 192.347 | 2554.132 | 13.28x | 194.585 | 7893.237 | 40.56x |
| gmm_robust_n100000 | 186.990 | 2435.310 | 13.02x | 191.467 | 7399.713 | 38.65x |
| 2sls_kernel_n100000 | 3357.937 | 3225.712 | 0.96x | 3362.642 | 10128.391 | 3.01x |
| 2sls_clustered_n100000 | 174.285 | 2678.161 | 15.37x | 175.883 | 8622.224 | 49.02x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 151.4 | 17.9 | 8.46x C# faster |
| latin | 32 | 282.2 | 157.6 | 1.79x C# faster |
| latin | 128 | 1649.0 | 897.1 | 1.84x C# faster |
| latin | 512 | 14132.9 | 11699.9 | 1.21x C# faster |
| cjk | 8 | 160.6 | 17.8 | 9.02x C# faster |
| cjk | 32 | 385.5 | 292.7 | 1.32x C# faster |
| cjk | 128 | 2773.8 | 2012.5 | 1.38x C# faster |
| cjk | 512 | 23932.1 | 17760.9 | 1.35x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 1.023 | 235.98x | 0.004 | 1.023 | 235.98x |
| accuracy_n1000_k2 | 0.000 | 0.555 | 4183.56x | 0.000 | 0.555 | 4183.63x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.871 | 376.10x | 0.005 | 1.871 | 376.09x |
| classification_report_n1000_k2 | 0.007 | 7.270 | 1003.40x | 0.007 | 7.269 | 1003.36x |
| roc_auc_binary_n1000_k2 | 0.018 | 2.006 | 113.77x | 0.018 | 2.006 | 113.77x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.100 | 231.40x | 0.005 | 1.100 | 231.40x |
| matthews_n1000_k2 | 0.004 | 2.105 | 469.64x | 0.004 | 2.105 | 469.63x |
| cohen_kappa_n1000_k2 | 0.004 | 1.136 | 257.65x | 0.004 | 1.136 | 257.63x |
| mse_n1000_k2 | 0.000 | 0.323 | 661.62x | 0.000 | 0.323 | 661.61x |
| mae_n1000_k2 | 0.000 | 0.320 | 660.34x | 0.000 | 0.320 | 660.33x |
| median_ae_n1000_k2 | 0.005 | 0.331 | 68.93x | 0.005 | 0.331 | 68.93x |
| r2_n1000_k2 | 0.001 | 0.383 | 323.95x | 0.001 | 0.383 | 323.94x |
| confusion_matrix_n1000_k10 | 0.005 | 1.030 | 222.75x | 0.005 | 1.030 | 222.74x |
| accuracy_n1000_k10 | 0.000 | 0.557 | 4179.50x | 0.000 | 0.557 | 4179.74x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.887 | 321.56x | 0.006 | 1.887 | 321.53x |
| classification_report_n1000_k10 | 0.012 | 7.341 | 628.74x | 0.012 | 7.340 | 628.74x |
| roc_auc_ovr_macro_n1000_k10 | 0.524 | 10.178 | 19.43x | 0.524 | 10.177 | 19.43x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.094 | 191.75x | 0.006 | 1.094 | 191.77x |
| matthews_n1000_k10 | 0.006 | 2.100 | 373.21x | 0.006 | 2.100 | 373.21x |
| cohen_kappa_n1000_k10 | 0.006 | 1.134 | 188.55x | 0.006 | 1.134 | 188.55x |
| mse_n1000_k10 | 0.000 | 0.319 | 648.75x | 0.000 | 0.319 | 648.73x |
| mae_n1000_k10 | 0.000 | 0.318 | 656.28x | 0.000 | 0.318 | 656.23x |
| median_ae_n1000_k10 | 0.005 | 0.330 | 69.38x | 0.005 | 0.330 | 69.39x |
| r2_n1000_k10 | 0.001 | 0.385 | 324.20x | 0.001 | 0.385 | 324.20x |
| confusion_matrix_n100000_k2 | 0.460 | 10.783 | 23.45x | 0.460 | 10.781 | 23.45x |
| accuracy_n100000_k2 | 0.013 | 3.789 | 299.38x | 0.013 | 3.789 | 299.37x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.437 | 27.03x | 0.460 | 12.437 | 27.03x |
| classification_report_n100000_k2 | 0.464 | 27.513 | 59.36x | 0.464 | 27.512 | 59.36x |
| roc_auc_binary_n100000_k2 | 4.455 | 26.763 | 6.01x | 4.454 | 26.761 | 6.01x |
| balanced_accuracy_n100000_k2 | 0.460 | 10.904 | 23.72x | 0.460 | 10.903 | 23.72x |
| matthews_n100000_k2 | 0.460 | 21.763 | 47.30x | 0.460 | 21.762 | 47.30x |
| cohen_kappa_n100000_k2 | 0.460 | 10.890 | 23.69x | 0.460 | 10.889 | 23.69x |
| mse_n100000_k2 | 0.045 | 0.467 | 10.35x | 0.045 | 0.467 | 10.36x |
| mae_n100000_k2 | 0.045 | 0.472 | 10.44x | 0.045 | 0.472 | 10.44x |
| median_ae_n100000_k2 | 0.596 | 1.813 | 3.04x | 0.623 | 1.813 | 2.91x |
| r2_n100000_k2 | 0.110 | 0.661 | 6.03x | 0.110 | 0.661 | 6.03x |
| confusion_matrix_n100000_k10 | 0.492 | 10.815 | 21.98x | 0.492 | 10.815 | 21.98x |
| accuracy_n100000_k10 | 0.012 | 3.798 | 309.49x | 0.012 | 3.797 | 309.48x |
| precision_recall_f1_macro_n100000_k10 | 0.494 | 13.040 | 26.41x | 0.494 | 13.039 | 26.41x |
| classification_report_n100000_k10 | 0.500 | 30.019 | 60.09x | 0.500 | 30.017 | 60.09x |
| roc_auc_ovr_macro_n100000_k10 | 35.768 | 217.031 | 6.07x | 35.767 | 217.019 | 6.07x |
| balanced_accuracy_n100000_k10 | 0.495 | 10.873 | 21.97x | 0.495 | 10.873 | 21.96x |
| matthews_n100000_k10 | 0.493 | 22.345 | 45.36x | 0.493 | 22.343 | 45.36x |
| cohen_kappa_n100000_k10 | 0.496 | 11.001 | 22.20x | 0.496 | 11.001 | 22.20x |
| mse_n100000_k10 | 0.045 | 0.469 | 10.41x | 0.045 | 0.469 | 10.41x |
| mae_n100000_k10 | 0.045 | 0.471 | 10.45x | 0.045 | 0.471 | 10.45x |
| median_ae_n100000_k10 | 0.620 | 1.813 | 2.92x | 0.666 | 1.813 | 2.72x |
| r2_n100000_k10 | 0.110 | 0.680 | 6.19x | 0.110 | 0.680 | 6.19x |
| confusion_matrix_n1000000_k2 | 4.595 | 99.915 | 21.74x | 4.595 | 99.911 | 21.74x |
| accuracy_n1000000_k2 | 0.122 | 33.037 | 270.60x | 0.122 | 33.036 | 270.60x |
| precision_recall_f1_macro_n1000000_k2 | 4.600 | 108.371 | 23.56x | 4.600 | 108.265 | 23.54x |
| classification_report_n1000000_k2 | 4.608 | 211.813 | 45.96x | 4.608 | 211.692 | 45.94x |
| roc_auc_binary_n1000000_k2 | 56.508 | 289.622 | 5.13x | 56.505 | 289.594 | 5.13x |
| balanced_accuracy_n1000000_k2 | 4.598 | 99.722 | 21.69x | 4.598 | 99.719 | 21.69x |
| matthews_n1000000_k2 | 4.601 | 200.044 | 43.48x | 4.601 | 200.026 | 43.48x |
| cohen_kappa_n1000000_k2 | 4.595 | 99.748 | 21.71x | 4.595 | 99.744 | 21.71x |
| mse_n1000000_k2 | 0.446 | 1.597 | 3.58x | 0.446 | 1.595 | 3.58x |
| mae_n1000000_k2 | 0.452 | 2.080 | 4.60x | 0.452 | 2.073 | 4.59x |
| median_ae_n1000000_k2 | 5.710 | 14.187 | 2.48x | 5.735 | 14.186 | 2.47x |
| r2_n1000000_k2 | 1.096 | 2.757 | 2.52x | 1.096 | 2.755 | 2.52x |
| confusion_matrix_n1000000_k10 | 4.909 | 100.163 | 20.40x | 4.909 | 100.138 | 20.40x |
| accuracy_n1000000_k10 | 0.120 | 33.213 | 276.15x | 0.120 | 33.213 | 276.15x |
| precision_recall_f1_macro_n1000000_k10 | 4.899 | 114.117 | 23.29x | 4.899 | 114.110 | 23.29x |
| classification_report_n1000000_k10 | 4.891 | 235.468 | 48.14x | 4.888 | 235.330 | 48.14x |
| balanced_accuracy_n1000000_k10 | 4.903 | 99.857 | 20.37x | 4.903 | 99.850 | 20.37x |
| matthews_n1000000_k10 | 4.888 | 207.202 | 42.39x | 4.888 | 207.189 | 42.39x |
| cohen_kappa_n1000000_k10 | 4.905 | 99.754 | 20.34x | 4.905 | 99.745 | 20.34x |
| mse_n1000000_k10 | 0.453 | 1.696 | 3.75x | 0.453 | 1.694 | 3.74x |
| mae_n1000000_k10 | 0.456 | 2.223 | 4.87x | 0.456 | 2.221 | 4.87x |
| median_ae_n1000000_k10 | 6.048 | 14.208 | 2.35x | 6.278 | 14.206 | 2.26x |
| r2_n1000000_k10 | 1.125 | 2.702 | 2.40x | 1.125 | 2.696 | 2.40x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.257 | 0.876 | 0.39x | 2.257 | 0.876 | 0.39x |
| transform_frobenius_n500 | 7.664 | 1.648 | 0.22x | 7.663 | 1.648 | 0.22x |
| transform_frobenius_n2000 | 28.190 | 5.752 | 0.20x | 28.239 | 22.964 | 0.81x |
| transform_kl_n100 | 1.128 | 17.768 | 15.76x | 1.128 | 17.767 | 15.76x |
| transform_kl_n500 | 5.061 | 33.749 | 6.67x | 5.060 | 33.749 | 6.67x |
| transform_kl_n2000 | 20.042 | 122.106 | 6.09x | 20.106 | 445.784 | 22.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.089 | 3.002 | 33.65x | 0.089 | 3.002 | 33.66x |
| ols_hac_n1000 | 0.168 | 3.501 | 20.79x | 0.168 | 3.501 | 20.79x |
| ols_cluster_n1000 | 0.062 | 3.550 | 57.09x | 0.062 | 3.550 | 57.09x |
| ols_summary_n10000 | 0.471 | 11.513 | 24.46x | 0.471 | 11.513 | 24.46x |
| ols_hac_n10000 | 1.730 | 12.753 | 7.37x | 1.736 | 12.752 | 7.34x |
| ols_cluster_n10000 | 0.612 | 13.072 | 21.35x | 0.615 | 13.071 | 21.27x |
| ols_summary_n100000 | 4.361 | 113.387 | 26.00x | 4.382 | 447.890 | 102.21x |
| ols_hac_n100000 | 18.013 | 123.774 | 6.87x | 18.501 | 492.336 | 26.61x |
| ols_cluster_n100000 | 7.095 | 127.287 | 17.94x | 7.402 | 508.382 | 68.68x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.658 | 4.395 | 2.65x | 1.663 | 4.395 | 2.64x |
| sparse_infrequent_10000 | 2.110 | 4.747 | 2.25x | 2.114 | 4.747 | 2.25x |
| sparse_100000 | 16.974 | 30.777 | 1.81x | 17.466 | 30.775 | 1.76x |
| sparse_infrequent_100000 | 18.827 | 25.560 | 1.36x | 19.268 | 25.559 | 1.33x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4', 'pandas': '2.3.3'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.262 | 20.156 | 76.95x | 0.262 | 20.154 | 76.95x |
| fe_twoway_clustered_n1000 | 0.416 | 31.622 | 76.10x | 0.416 | 31.618 | 76.09x |
| fe_kernel_n1000 | 0.277 | 20.739 | 74.93x | 0.277 | 20.738 | 74.93x |
| between_robust_n1000 | 0.155 | 19.557 | 126.31x | 0.155 | 19.556 | 126.31x |
| first_difference_robust_n1000 | 0.288 | 23.352 | 81.08x | 0.288 | 23.349 | 81.08x |
| random_effects_n1000 | 0.281 | 26.942 | 95.74x | 0.281 | 26.941 | 95.75x |
| fe_entity_n10000 | 2.700 | 29.358 | 10.87x | 2.809 | 29.355 | 10.45x |
| fe_twoway_clustered_n10000 | 4.892 | 55.316 | 11.31x | 5.248 | 110.540 | 21.06x |
| fe_kernel_n10000 | 2.950 | 29.737 | 10.08x | 2.998 | 29.714 | 9.91x |
| between_robust_n10000 | 1.845 | 28.226 | 15.30x | 1.847 | 28.225 | 15.28x |
| first_difference_robust_n10000 | 3.002 | 32.438 | 10.81x | 3.011 | 32.434 | 10.77x |
| random_effects_n10000 | 2.858 | 38.523 | 13.48x | 2.896 | 38.520 | 13.30x |
| fe_entity_n100000 | 30.420 | 173.731 | 5.71x | 31.800 | 693.509 | 21.81x |
| fe_twoway_clustered_n100000 | 49.172 | 352.372 | 7.17x | 50.728 | 1262.615 | 24.89x |
| fe_kernel_n100000 | 32.323 | 162.698 | 5.03x | 33.869 | 648.669 | 19.15x |
| between_robust_n100000 | 19.698 | 161.583 | 8.20x | 20.477 | 528.429 | 25.81x |
| first_difference_robust_n100000 | 37.811 | 185.657 | 4.91x | 40.288 | 740.554 | 18.38x |
| random_effects_n100000 | 35.849 | 217.976 | 6.08x | 37.762 | 861.551 | 22.82x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### compare-survival-aalen

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 0.647 | 83.369 | 128.76x | 0.647 | 332.902 | 514.19x |
| fit_penalised_weighted_1000 | 0.648 | 95.175 | 146.91x | 0.648 | 379.814 | 586.32x |
| predict_survival_1000 | 0.826 | 2.375 | 2.88x | 0.828 | 2.375 | 2.87x |
| fit_10000 | 7.246 | 613.820 | 84.71x | 7.491 | 989.296 | 132.06x |
| fit_penalised_weighted_10000 | 7.229 | 643.074 | 88.96x | 7.516 | 1018.850 | 135.56x |
| predict_survival_10000 | 3.322 | 4.793 | 1.44x | 3.554 | 19.157 | 5.39x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.026 | 14.223 | 542.67x | 0.026 | 14.218 | 542.58x |
| logrank_fleming_harrington_1000 | 0.034 | 21.518 | 625.09x | 0.034 | 21.517 | 625.06x |
| multigroup_1000 | 0.113 | 28.847 | 254.54x | 0.113 | 28.847 | 254.55x |
| pairwise_1000 | 0.305 | 150.130 | 492.23x | 0.305 | 150.123 | 492.26x |
| rmst_1000 | 0.088 | 1323.340 | 15030.29x | 0.088 | 1323.278 | 15030.41x |
| fixed_point_1000 | 0.037 | 15.460 | 418.60x | 0.037 | 15.459 | 418.58x |
| concordance_1000 | 0.049 | 8.199 | 165.68x | 0.049 | 8.198 | 165.67x |
| logrank_wilcoxon_weighted_10000 | 0.332 | 16.040 | 48.25x | 0.332 | 16.039 | 48.24x |
| logrank_fleming_harrington_10000 | 0.417 | 25.456 | 61.05x | 0.417 | 25.455 | 61.09x |
| multigroup_10000 | 2.482 | 41.300 | 16.64x | 2.486 | 41.297 | 16.61x |
| pairwise_10000 | 4.816 | 166.142 | 34.50x | 4.816 | 166.136 | 34.50x |
| rmst_10000 | 1.829 | 1411.278 | 771.44x | 1.866 | 1411.179 | 756.19x |
| fixed_point_10000 | 0.387 | 17.983 | 46.46x | 0.387 | 17.981 | 46.46x |
| concordance_10000 | 2.372 | 97.463 | 41.10x | 2.372 | 97.459 | 41.10x |
| logrank_wilcoxon_weighted_100000 | 4.441 | 24.387 | 5.49x | 4.572 | 24.386 | 5.33x |
| logrank_fleming_harrington_100000 | 4.302 | 38.853 | 9.03x | 4.385 | 38.850 | 8.86x |
| multigroup_100000 | 11.268 | 54.780 | 4.86x | 11.462 | 54.777 | 4.78x |
| pairwise_100000 | 45.551 | 250.321 | 5.50x | 47.163 | 250.118 | 5.30x |
| rmst_100000 | 9.173 | 518.287 | 56.50x | 9.305 | 518.274 | 55.70x |
| fixed_point_100000 | 4.742 | 31.907 | 6.73x | 4.890 | 31.906 | 6.52x |
| concordance_100000 | 29.917 | 822.851 | 27.50x | 30.168 | 822.778 | 27.27x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.385 | 76.301 | 22.54x | 3.385 | 76.295 | 22.54x |
| loglogistic_left_1000 | 8.918 | 107.058 | 12.00x | 8.925 | 107.050 | 11.99x |
| lognormal_interval_1000 | 3.318 | 197.398 | 59.49x | 3.318 | 197.382 | 59.49x |
| generalized_gamma_1000 | 12.791 | 849.480 | 66.41x | 12.836 | 849.430 | 66.18x |
| weibull_aft_1000 | 7.034 | 139.523 | 19.84x | 7.040 | 139.504 | 19.82x |
| lognormal_aft_ridge_robust_1000 | 8.798 | 4872.213 | 553.77x | 8.799 | 4871.915 | 553.67x |
| loglogistic_aft_interval_1000 | 9.853 | 217.926 | 22.12x | 9.856 | 217.905 | 22.11x |
| breslow_fleming_harrington_1000 | 0.258 | 7.202 | 27.93x | 0.258 | 7.202 | 27.93x |
| kaplan_meier_left_1000 | 0.088 | 8.515 | 96.22x | 0.089 | 8.514 | 96.21x |
| weibull_10000 | 34.302 | 91.706 | 2.67x | 34.298 | 91.700 | 2.67x |
| loglogistic_left_10000 | 99.386 | 185.200 | 1.86x | 99.381 | 185.180 | 1.86x |
| lognormal_interval_10000 | 30.197 | 344.575 | 11.41x | 30.195 | 344.550 | 11.41x |
| generalized_gamma_10000 | 103.301 | 2075.665 | 20.09x | 103.295 | 2075.520 | 20.09x |
| weibull_aft_10000 | 70.342 | 159.446 | 2.27x | 70.349 | 159.420 | 2.27x |
| lognormal_aft_ridge_robust_10000 | 123.345 | 47073.867 | 381.65x | 123.478 | 47071.346 | 381.21x |
| loglogistic_aft_interval_10000 | 88.934 | 290.727 | 3.27x | 88.937 | 290.698 | 3.27x |
| breslow_fleming_harrington_10000 | 2.544 | 8.838 | 3.47x | 2.544 | 8.838 | 3.47x |
| kaplan_meier_left_10000 | 0.956 | 10.196 | 10.67x | 0.956 | 10.195 | 10.67x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

_As of 2026-11-14, measured at commit `f4f54f2cc2b875d7570cc01f055c7eca95a1a0b6`._

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

### compare-var

_As of 2026-11-15, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.039 | 2.691 | 69.51x | 0.039 | 2.691 | 69.51x |
| var_n10000 | 0.486 | 16.772 | 34.50x | 0.490 | 16.771 | 34.24x |
| var_n100000 | 3.996 | 161.390 | 40.39x | 4.217 | 385.189 | 91.33x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
