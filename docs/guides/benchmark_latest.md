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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **928.4 ns** |          **9.48 ns** |       **0.52 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         722.7 ns |        111.67 ns |       6.12 ns |     0.78 |    0.01 |   0.0067 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       3,040.6 ns |        369.73 ns |      20.27 ns |     3.28 |    0.02 |   0.1640 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,844.3 ns |        740.44 ns |      40.59 ns |     4.14 |    0.04 |   0.0763 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,367.7 ns |        540.28 ns |      29.61 ns |     5.78 |    0.03 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,339.2 ns |      3,557.26 ns |     194.99 ns |    11.14 |    0.18 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,276.8 ns |        179.65 ns |       9.85 ns |     5.68 |    0.01 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **81,483.3 ns** |      **3,025.88 ns** |     **165.86 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      50,424.0 ns |      4,301.15 ns |     235.76 ns |     0.62 |    0.00 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,047,638.0 ns |    346,704.71 ns |  19,004.06 ns |    12.86 |    0.20 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,822,484.3 ns |     26,225.15 ns |   1,437.49 ns |    22.37 |    0.04 |   5.8594 |        - |        - |  160426 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,297,485.6 ns |     33,617.72 ns |   1,842.70 ns |    28.20 |    0.05 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 267,198,585.7 ns | 17,758,814.66 ns | 973,420.56 ns | 3,279.19 |   11.85 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,292,314.7 ns |    228,573.80 ns |  12,528.90 ns |    28.13 |    0.14 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **262.6 μs** |     **9.31 μs** |   **0.51 μs** |   **1.9531** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,821.4 μs** |    **90.84 μs** |   **4.98 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **28,658.4 μs** | **1,943.49 μs** | **106.53 μs** | **500.0000** | **500.0000** | **500.0000** | **5474.32 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method     | SampleSize | Mean         | Error       | StdDev     | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|-----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **8.622 μs** |   **0.4475 μs** |  **0.0245 μs** |  **0.1831** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **122.680 μs** |  **15.8025 μs** |  **0.8662 μs** |  **0.9766** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,286.454 μs** | **382.4531 μs** | **20.9635 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **55.93 μs** |  **4.996 μs** | **0.274 μs** |  **1.00** | **0.6104** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    59.29 μs |  8.777 μs | 0.481 μs |  1.06 | 0.6104 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,725.45 μs** | **22.967 μs** | **1.259 μs** |  **1.00** | **5.8594** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,392.76 μs | 81.338 μs | 4.458 μs |  0.81 | 5.8594 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method            | Regressors | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **127.7 μs** |      **26.66 μs** |      **1.46 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     216.4 μs |      44.23 μs |      2.42 μs |  1.69 |    0.02 |   1.2207 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     707.6 μs |     436.41 μs |     23.92 μs |  5.54 |    0.17 | 190.4297 | 190.4297 | 190.4297 |   705.62 KB |        3.75 |
|                   |            |              |               |              |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **84,253.5 μs** |   **6,016.19 μs** |    **329.77 μs** |  **1.00** |    **0.00** | **333.3333** | **333.3333** | **333.3333** |  **8865.82 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 357,991.6 μs |   7,910.37 μs |    433.59 μs |  4.25 |    0.02 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 554,032.7 μs | 526,830.62 μs | 28,877.36 μs |  6.58 |    0.30 |        - |        - |        - | 32670.65 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |     **4,616.8 ns** |     **610.27 ns** |     **33.45 ns** |   **1.00** |    **0.01** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     1,952.0 ns |      88.65 ns |      4.86 ns |   0.42 |    0.00 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,377.1 ns |     167.58 ns |      9.19 ns |   0.51 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,995.9 ns |     990.01 ns |     54.27 ns |   0.87 |    0.01 | 0.0458 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     6,956.8 ns |     648.21 ns |     35.53 ns |   1.51 |    0.01 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,149.8 ns |     844.60 ns |     46.30 ns |   1.98 |    0.02 | 0.0610 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,580.9 ns |     187.07 ns |     10.25 ns |   0.56 |    0.00 | 0.0648 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,909.2 ns |     164.84 ns |      9.04 ns |   0.85 |    0.01 | 0.0458 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |     1,913.6 ns |     222.99 ns |     12.22 ns |   0.41 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,787.5 ns |     140.77 ns |      7.72 ns |   0.60 |    0.00 | 0.0267 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,170.5 ns |     119.68 ns |      6.56 ns |   0.25 |    0.00 | 0.0324 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,161.7 ns |     112.70 ns |      6.18 ns |   0.25 |    0.00 | 0.0381 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       357.4 ns |      66.01 ns |      3.62 ns |   0.08 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,614.4 ns |     101.65 ns |      5.57 ns |   0.35 |    0.00 | 0.0362 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       112.9 ns |       1.87 ns |      0.10 ns |   0.02 |    0.00 | 0.0067 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       707.0 ns |       3.89 ns |      0.21 ns |   0.15 |    0.00 | 0.0381 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **72,360.2 ns** |   **1,173.48 ns** |     **64.32 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   156,164.2 ns |   4,419.34 ns |    242.24 ns |  2.158 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   471,979.5 ns |  11,645.23 ns |    638.31 ns |  6.523 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 1,945,988.2 ns | 206,280.40 ns | 11,306.92 ns | 26.893 |    0.14 |      - |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,179,508.7 ns | 122,035.84 ns |  6,689.20 ns | 16.301 |    0.08 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,432,870.0 ns | 360,954.92 ns | 19,785.16 ns | 47.441 |    0.24 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,378,344.1 ns |  32,387.84 ns |  1,775.29 ns | 19.048 |    0.03 | 5.8594 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,947,579.1 ns | 103,542.59 ns |  5,675.52 ns | 26.915 |    0.07 |      - |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |   108,909.3 ns |   1,610.49 ns |     88.28 ns |  1.505 |    0.00 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   234,664.3 ns |   4,577.41 ns |    250.90 ns |  3.243 |    0.00 |      - |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 10000      |   248,905.0 ns |  12,707.17 ns |    696.52 ns |  3.440 |    0.01 | 2.9297 |   80057 B |    1,250.89 |
| MetaNumerics_Wilcoxon          | 10000      |   390,541.2 ns |  47,094.71 ns |  2,581.42 ns |  5.397 |    0.03 | 2.9297 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       355.5 ns |       5.19 ns |      0.28 ns |  0.005 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,617.5 ns |     102.31 ns |      5.61 ns |  0.022 |    0.00 | 0.0362 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       112.0 ns |       7.30 ns |      0.40 ns |  0.002 |    0.00 | 0.0067 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       706.1 ns |      49.12 ns |      2.69 ns |  0.010 |    0.00 | 0.0381 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                   | GroupSize | Mean           | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **3,213.7 ns** |     **119.58 ns** |      **6.55 ns** |  **1.00** |    **0.00** |  **0.1984** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,477.1 ns |     217.89 ns |     11.94 ns |  1.39 |    0.00 |  0.2136 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       674.5 ns |      59.40 ns |      3.26 ns |  0.21 |    0.00 |  0.0029 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,307.8 ns |      50.91 ns |      2.79 ns |  0.41 |    0.00 |  0.0057 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       779.1 ns |      20.02 ns |      1.10 ns |  0.24 |    0.00 |  0.0019 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,233.1 ns |     626.27 ns |     34.33 ns |  1.32 |    0.01 |  0.0839 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     9,105.2 ns |     927.91 ns |     50.86 ns |  2.83 |    0.01 |  0.0305 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,148.0 ns |     742.08 ns |     40.68 ns |  1.91 |    0.01 |  0.0381 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,521.6 ns |     145.11 ns |      7.95 ns |  1.41 |    0.00 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **375,546.8 ns** |  **18,899.84 ns** |  **1,035.96 ns** | **1.000** |    **0.00** | **19.0430** | **4.3945** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,208,045.4 ns |  87,989.28 ns |  4,822.99 ns | 3.217 |    0.01 | 17.5781 | 3.9063 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    69,443.2 ns |   4,650.87 ns |    254.93 ns | 0.185 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   138,525.3 ns |   1,166.73 ns |     63.95 ns | 0.369 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,024.0 ns |     143.13 ns |      7.85 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   801,828.1 ns | 236,345.39 ns | 12,954.89 ns | 2.135 |    0.03 |  7.8125 | 0.9766 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,386,632.3 ns | 114,160.07 ns |  6,257.50 ns | 3.692 |    0.02 |  1.9531 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    11,231.8 ns |     349.45 ns |     19.15 ns | 0.030 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                  | Mean      | Error     | StdDev   | Gen0      | Allocated |
|------------------------ |----------:|----------:|---------:|----------:|----------:|
| ChatTemplate            | 106.50 ms |  7.186 ms | 0.394 ms | 1200.0000 |   33.3 MB |
| ProseWithoutAddedTokens |  93.30 ms | 48.844 ms | 2.677 ms | 1166.6667 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **2,334.5 μs** |     **944.15 μs** |     **51.75 μs** |   **1.00** |    **0.03** |   **246.0938** |  **246.0938** |  **246.0938** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    97,930.2 μs |   1,703.67 μs |     93.38 μs |  41.96 |    0.80 |  2333.3333 | 1166.6667 |  500.0000 |  59879.82 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **2,051.6 μs** |     **220.76 μs** |     **12.10 μs** |   **1.00** |    **0.01** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    98,713.0 μs |  42,210.30 μs |  2,313.69 μs |  48.12 |    1.01 |  3200.0000 | 1200.0000 |  600.0000 |  83111.22 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **749.0 μs** |      **90.03 μs** |      **4.93 μs** |   **1.00** |    **0.01** |     **0.9766** |         **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   143,107.7 μs |  58,735.03 μs |  3,219.47 μs | 191.07 |    3.88 |  3250.0000 | 1250.0000 |  500.0000 |  81289.45 KB |    2,678.87 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,360.9 μs** |   **1,442.10 μs** |     **79.05 μs** |   **1.00** |    **0.04** |   **246.0938** |  **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    75,290.1 μs |  22,550.49 μs |  1,236.07 μs |  31.91 |    1.04 |  2571.4286 | 1000.0000 |  428.5714 |  64190.65 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **16,352.3 μs** |   **4,045.87 μs** |    **221.77 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.16 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,736,605.8 μs | 674,774.80 μs | 36,986.68 μs | 106.21 |    2.32 | 23000.0000 | 9000.0000 | 3000.0000 | 537139.66 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,675.0 μs** |   **2,855.48 μs** |    **156.52 μs** |   **1.00** |    **0.01** |   **500.0000** |  **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,913,110.1 μs | 163,216.74 μs |  8,946.46 μs | 130.37 |    1.32 | 32000.0000 | 9000.0000 | 3000.0000 | 747340.08 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,199.9 μs** |     **446.66 μs** |     **24.48 μs** |   **1.00** |    **0.00** |          **-** |         **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,291,351.6 μs | 134,768.04 μs |  7,387.09 μs | 530.88 |    2.08 | 31000.0000 | 9000.0000 | 3000.0000 | 732668.17 KB |    8,147.83 |
|                        |      |          |                |               |              |        |         |            |           |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **18,000.2 μs** |   **6,423.52 μs** |    **352.09 μs** |   **1.00** |    **0.02** |   **500.0000** |  **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,512,506.5 μs | 848,016.18 μs | 46,482.63 μs |  84.05 |    2.65 | 24000.0000 | 7000.0000 | 2000.0000 | 573604.29 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.824 μs** |   **0.6981 μs** | **0.0383 μs** |  **1.00** |  **0.0916** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.217 μs |   0.3243 μs | 0.0178 μs |  1.07 |  0.1144 |      - |   2.81 KB |        1.24 |
| EmbedBatchBucketed | 1          |   6.288 μs |   0.3185 μs | 0.0175 μs |  1.08 |  0.1144 |      - |   2.81 KB |        1.24 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **58.446 μs** |   **1.7985 μs** | **0.0986 μs** |  **1.00** |  **1.1597** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  20.771 μs |   1.2936 μs | 0.0709 μs |  0.36 |  0.8850 |      - |  22.38 KB |        0.76 |
| EmbedBatchBucketed | 8          |  20.837 μs |   0.5516 μs | 0.0302 μs |  0.36 |  0.8850 |      - |  22.38 KB |        0.76 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **225.785 μs** |   **8.1823 μs** | **0.4485 μs** |  **1.00** |  **4.3945** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  78.742 μs |  31.1275 μs | 1.7062 μs |  0.35 |  3.2959 |      - |  82.75 KB |        0.75 |
| EmbedBatchBucketed | 32         |  70.183 μs |   9.8989 μs | 0.5426 μs |  0.31 |  2.8076 |      - |  69.23 KB |        0.62 |
|                    |            |            |             |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **947.004 μs** | **162.1650 μs** | **8.8888 μs** |  **1.00** | **17.5781** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 312.641 μs |  97.7678 μs | 5.3590 μs |  0.33 | 13.1836 | 1.4648 | 329.78 KB |        0.74 |
| EmbedBatchBucketed | 128        | 268.178 μs |  19.9584 μs | 1.0940 μs |  0.28 | 10.2539 | 0.9766 | 262.27 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **36.81 ms** |  **1.601 ms** | **0.088 ms** |  **428.5714** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **40.06 ms** |  **0.466 ms** | **0.026 ms** |  **461.5385** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **35.76 ms** |  **2.756 ms** | **0.151 ms** |  **466.6667** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **49.84 ms** |  **3.674 ms** | **0.201 ms** |  **800.0000** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.49 ms** |  **7.814 ms** | **0.428 ms** |  **846.1538** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **54.49 ms** | **11.623 ms** | **0.637 ms** | **1000.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **136.76 ms** |  **22.237 ms** | **1.219 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  81.19 ms |   7.782 ms | 0.427 ms |  0.59 |    0.01 |  103.71 KB |        3.81 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **142.80 ms** | **109.968 ms** | **6.028 ms** |  **1.00** |    **0.05** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  78.13 ms |  61.130 ms | 3.351 ms |  0.55 |    0.03 |  116.52 KB |        4.88 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **201.85 ms** |  **11.306 ms** | **0.620 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 308.77 ms |  14.322 ms | 0.785 ms |  1.53 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **210.92 ms** |  **16.563 ms** | **0.908 ms** |  **1.00** |    **0.01** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 274.64 ms | 148.355 ms | 8.132 ms |  1.30 |    0.03 |  192.95 KB |        3.54 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **253.49 ms** |  **12.460 ms** | **0.683 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 423.37 ms |  78.828 ms | 4.321 ms |  1.67 |    0.02 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **263.84 ms** |  **13.996 ms** | **0.767 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 410.16 ms |  99.395 ms | 5.448 ms |  1.55 |    0.02 | 1153.52 KB |        1.56 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **298.03 ms** |  **29.445 ms** | **1.614 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 472.11 ms |  17.708 ms | 0.971 ms |  1.58 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **304.91 ms** |  **27.124 ms** | **1.487 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 517.84 ms |  89.133 ms | 4.886 ms |  1.70 |    0.02 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method      | Count | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |----------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    |  **82.95 ms** |  **10.87 ms** | **0.596 ms** |  **1.00** |    **0.01** | **1833.3333** | **1833.3333** | **1833.3333** |   **9388.6 KB** |       **1.000** |
| TreeNearest | 10    |  65.20 ms | 102.10 ms | 5.596 ms |  0.79 |    0.06 |         - |         - |         - |    29.72 KB |       0.003 |
|             |       |           |           |          |       |         |           |           |           |             |             |
| **FullScan**    | **2000**  |  **85.18 ms** |  **13.21 ms** | **0.724 ms** |  **1.00** |    **0.01** | **1833.3333** | **1833.3333** | **1833.3333** | **10007.94 KB** |        **1.00** |
| TreeNearest | 2000  | 102.66 ms |  47.17 ms | 2.586 ms |  1.21 |    0.03 |         - |         - |         - |  1273.62 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method | length | Mean          | Error        | StdDev    | Allocated |
|------- |------- |--------------:|-------------:|----------:|----------:|
| **Latin**  | **1000**   |      **49.35 μs** |     **3.584 μs** |  **0.196 μs** |         **-** |
| Cjk    | 1000   |      66.64 μs |    19.315 μs |  1.059 μs |         - |
| **Latin**  | **10000**  |   **4,389.78 μs** |    **44.169 μs** |  **2.421 μs** |         **-** |
| Cjk    | 10000  |   6,944.49 μs |   266.855 μs | 14.627 μs |         - |
| **Latin**  | **65536**  | **183,735.64 μs** | **1,282.370 μs** | **70.291 μs** |         **-** |

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 27.50 ms |  2.541 ms | 0.139 ms |  1.00 |    0.01 |  312.5000 |   8.19 MB |        1.00 |
| Bpe     | 91.26 ms | 32.849 ms | 1.801 ms |  3.32 |    0.06 | 1166.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **19.41 μs** |  **1.352 μs** | **0.074 μs** | **0.3052** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **74.71 μs** | **16.433 μs** | **0.901 μs** | **0.4883** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **182.80 μs** |  **5.339 μs** | **0.293 μs** | **0.9766** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **388.37 μs** |  **4.001 μs** | **0.219 μs** | **1.9531** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 8.540 ms | 3.457 ms | 0.1895 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **17.252 μs** |  **1.333 μs** | **0.0731 μs** |         **-** |
| MyersGroup | cjk      | 241.133 μs | 18.802 μs | 1.0306 μs |         - |
| **DpGroup**    | **latin**    |   **9.397 μs** |  **1.671 μs** | **0.0916 μs** |         **-** |
| MyersGroup | latin    | 130.947 μs | 20.793 μs | 1.1397 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                   | Cutoff | Widths | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **347.86 μs** | **28.17 μs** | **1.544 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 331.61 μs | 40.99 μs | 2.247 μs |  0.95 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **373.39 μs** | **39.58 μs** | **2.170 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 337.65 μs | 66.17 μs | 3.627 μs |  0.90 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **359.79 μs** | **47.29 μs** | **2.592 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 340.33 μs | 27.00 μs | 1.480 μs |  0.95 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **312.35 μs** | **36.76 μs** | **2.015 μs** |  **1.00** |    **0.01** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 336.22 μs | 11.61 μs | 0.636 μs |  1.08 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **323.31 μs** | **13.50 μs** | **0.740 μs** |  **1.00** |    **0.00** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 373.10 μs | 18.69 μs | 1.025 μs |  1.15 |    0.00 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **196.15 μs** | **21.28 μs** | **1.166 μs** |  **1.00** |    **0.01** | **1.2207** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 324.15 μs | 37.33 μs | 2.046 μs |  1.65 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **257.76 μs** | **16.92 μs** | **0.927 μs** |  **1.00** |    **0.00** | **0.9766** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 356.90 μs | 26.25 μs | 1.439 μs |  1.38 |    0.01 | 0.9766 |  32.02 KB |        1.00 |
|                          |        |        |           |          |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.98 μs** | **13.77 μs** | **0.755 μs** |  **1.00** |    **0.01** | **1.2207** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 320.41 μs | 12.42 μs | 0.681 μs |  3.56 |    0.03 | 0.9766 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **191.4 μs** |   **2.39 μs** |  **0.13 μs** |  **1.00** |   **0.7324** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   171.9 μs |   4.99 μs |  0.27 μs |  0.90 |   0.7324 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   468.1 μs |  11.64 μs |  0.64 μs |  2.45 |  12.2070 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,667.7 μs** |  **47.10 μs** |  **2.58 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,154.3 μs | 578.99 μs | 31.74 μs |  0.86 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,614.7 μs | 899.89 μs | 49.33 μs |  2.08 | 187.5000 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method | Classes | Mean         | Error        | StdDev      | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|-------------:|------------:|-------:|-------:|----------:|
| **Report** | **10**      |     **562.4 ns** |      **6.75 ns** |     **0.37 ns** | **0.0677** |      **-** |   **1.66 KB** |
| **Report** | **1000**    | **358,244.3 ns** | **28,614.29 ns** | **1,568.45 ns** | **4.8828** | **0.9766** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 127.6 ms | 12.28 ms | 0.67 ms | 750.0000 | 750.0000 | 750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 117.2 ms | 11.91 ms | 0.65 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method       | Shape     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-----------:|----------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **139.10 ms** |  **32.977 ms** |  **1.808 ms** |  **1.00** |    **0.02** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.41 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,165.29 ms | 232.454 ms | 12.742 ms | 15.57 |    0.19 | 12000.0000 | 8000.0000 | 7000.0000 | 156.29 MB |        5.31 |
|              |           |             |            |           |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **49.82 ms** |   **2.660 ms** |  **0.146 ms** |  **1.00** |    **0.00** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   755.04 ms |  46.610 ms |  2.555 ms | 15.15 |    0.06 |  4000.0000 | 4000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **188.02 ms** |  **18.597 ms** |  **1.019 ms** |  **1.00** |    **0.01** |  **1333.3333** |  **1333.3333** |  **1333.3333** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,262.36 ms | 499.676 ms | 27.389 ms | 33.31 |    0.20 | 29000.0000 | 14000.0000 | 11000.0000 | 531.88 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 4,248.94 ms | 289.660 ms | 15.877 ms | 22.60 |    0.13 | 15000.0000 |  9000.0000 |  7000.0000 | 372.12 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **21.45 ms** |   **7.758 ms** |  **0.425 ms** |  **1.00** |    **0.02** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   452.69 ms |  52.928 ms |  2.901 ms | 21.11 |    0.38 |  3000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   285.51 ms | 106.933 ms |  5.861 ms | 13.31 |    0.33 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                                    | Mean       | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  15.749 ms |  2.394 ms | 0.1312 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 118.473 ms | 27.128 ms | 1.4870 ms |  7.52 |    0.10 |
| Nmf_Transform                             |   3.781 ms |  1.154 ms | 0.0632 ms |  0.24 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  29.443 ms |  3.648 ms | 0.2000 ms |  1.87 |    0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **24,248.8 μs** |   **1,505.38 μs** |    **82.52 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **11692.44 KB** |
| TruncatedSvd_Transform     | 500     |     585.8 μs |      50.71 μs |     2.78 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 212,129.2 μs |  56,488.38 μs | 3,096.32 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18141.17 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 275,933.4 μs |  98,748.77 μs | 5,412.75 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **181,364.3 μs** |  **34,104.96 μs** | **1,869.41 μs** | **3333.3333** | **3333.3333** | **3333.3333** | **115064.01 KB** |
| TruncatedSvd_Transform     | 20000   |     969.6 μs |      82.99 μs |     4.55 μs |   39.0625 |   39.0625 |   39.0625 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 894,156.2 μs | 128,798.34 μs | 7,059.87 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.79 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 667,505.5 μs |  90,614.05 μs | 4,966.86 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 299.2 μs | 20.26 μs | 1.11 μs | 19.5313 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method              | Rows   | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|-------------------- |------- |----------:|----------:|----------:|---------:|---------:|---------:|------------:|
| **FromBlockNormalized** | **1000**   |  **1.043 ms** | **0.1266 ms** | **0.0069 ms** | **248.0469** | **248.0469** | **248.0469** |  **1500.18 KB** |
| MmrSelect100        | 1000   |  9.061 ms | 1.3996 ms | 0.0767 ms |        - |        - |        - |    24.93 KB |
| **FromBlockNormalized** | **100000** | **90.417 ms** | **1.7650 ms** | **0.0967 ms** | **166.6667** | **166.6667** | **166.6667** | **150000.3 KB** |
| MmrSelect100        | 100000 |  8.963 ms | 0.0581 ms | 0.0032 ms |        - |        - |        - |    24.93 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **648.5 μs** |    **90.74 μs** |   **4.97 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    678.8 μs |    12.19 μs |   0.67 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    901.8 μs |   129.48 μs |   7.10 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,781.3 μs | 1,262.91 μs |  69.22 μs | 5.8594 |  342.49 KB |
| **FilteredTop10**   | **100000**  | **11,147.2 μs** | **1,461.82 μs** |  **80.13 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  | 13,360.9 μs | 7,159.10 μs | 392.41 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 13,955.9 μs | 1,885.62 μs | 103.36 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 22,928.3 μs | 1,972.72 μs | 108.13 μs |      - | 3155.14 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method         | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    91.77 ns |  10.53 ns |  0.577 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   757.72 ns |  22.40 ns |  1.228 ns |  8.26 |    0.05 |      - |         - |          NA |
| TokenSortRatio |   937.93 ns |  60.45 ns |  3.314 ns | 10.22 |    0.06 | 0.0439 |    1120 B |          NA |
| TokenSetRatio  |   988.49 ns | 125.22 ns |  6.864 ns | 10.77 |    0.09 | 0.0343 |     896 B |          NA |
| WRatio         | 1,316.80 ns | 224.30 ns | 12.295 ns | 14.35 |    0.14 | 0.0439 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                    | Mean               | Error              | StdDev          | Ratio  | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------:|--------:|--------:|-------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,991,096.5599 ns |    137,618.0470 ns |   7,543.3096 ns |  1.000 |    0.00 |  3.9063 |      - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    693,584.5785 ns |     58,634.9603 ns |   3,213.9800 ns |  0.348 |    0.00 | 23.4375 | 7.8125 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,181,076.9212 ns |     52,381.1288 ns |   2,871.1864 ns |  0.593 |    0.00 | 27.3438 | 9.7656 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 81,327,900.3889 ns | 11,736,466.4318 ns | 643,315.3307 ns | 40.846 |    0.31 |       - |      - |  510843 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |      - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **89.99 ns** |   **6.021 ns** |  **0.330 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    251.99 ns |  56.142 ns |  3.077 ns |  2.80 |    0.03 | 0.0029 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **754.03 ns** |  **66.925 ns** |  **3.668 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,616.52 ns | 426.806 ns | 23.395 ns | 14.08 |    0.07 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,016.48 ns** | **246.901 ns** | **13.533 ns** |  **1.00** |    **0.02** | **0.0343** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,178.36 ns |  54.734 ns |  3.000 ns |  2.14 |    0.02 | 0.0763 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,269.70 ns** |  **59.649 ns** |  **3.270 ns** |  **1.00** |    **0.00** | **0.0439** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,291.81 ns | 163.527 ns |  8.963 ns |  4.17 |    0.01 | 0.1221 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.730 ms** | **0.5257 ms** | **0.0288 ms** | **267.5781** | **257.8125** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **27.946 ms** | **0.8896 ms** | **0.0488 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **29.74 ns** |   **7.497 ns** |  **0.411 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    38.37 ns |   6.674 ns |  0.366 ns |  1.29 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.91 ns |   5.779 ns |  0.317 ns |  0.94 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    26.19 ns |   0.707 ns |  0.039 ns |  0.88 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **27.70 ns** |   **0.524 ns** |  **0.029 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    38.98 ns |   0.966 ns |  0.053 ns |  1.41 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.20 ns |   8.148 ns |  0.447 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.50 ns |   5.039 ns |  0.276 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **31.51 ns** |   **0.911 ns** |  **0.050 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    38.93 ns |   5.822 ns |  0.319 ns |  1.24 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    30.84 ns |   2.056 ns |  0.113 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.01 ns |   5.436 ns |  0.298 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **35.51 ns** |   **5.462 ns** |  **0.299 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    42.55 ns |   2.738 ns |  0.150 ns |  1.20 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    36.08 ns |   0.953 ns |  0.052 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    32.76 ns |  10.590 ns |  0.580 ns |  0.92 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **51.51 ns** |  **14.422 ns** |  **0.791 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.48 ns |   0.694 ns |  0.038 ns |  1.17 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.30 ns |   9.626 ns |  0.528 ns |  1.11 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    52.29 ns |   1.997 ns |  0.109 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **63.85 ns** |   **4.464 ns** |  **0.245 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    68.96 ns |   1.339 ns |  0.073 ns |  1.08 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.87 ns |   6.843 ns |  0.375 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    57.40 ns |   0.405 ns |  0.022 ns |  0.90 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **401.61 ns** |   **4.601 ns** |  **0.252 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   412.59 ns |  11.377 ns |  0.624 ns |  1.03 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   402.13 ns |   8.540 ns |  0.468 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   398.39 ns |  25.551 ns |  1.401 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,919.93 ns** |  **24.350 ns** |  **1.335 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 5,114.19 ns |  73.302 ns |  4.018 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,963.03 ns | 282.569 ns | 15.489 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,981.03 ns | 513.847 ns | 28.166 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method             | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **407.4 ns** |    **28.33 ns** |   **1.55 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    189.5 ns |    32.36 ns |   1.77 ns |  0.47 |    0.00 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **2,338.1 ns** |   **613.17 ns** |  **33.61 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,846.6 ns |   358.43 ns |  19.65 ns |  1.22 |    0.02 |         - |          NA |
|                    |        |             |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **13,457.5 ns** |    **96.56 ns** |   **5.29 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,489.3 ns | 6,103.50 ns | 334.55 ns |  2.34 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,859.8 ns** | **1,015.78 ns** |  **55.68 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,694.7 ns |   375.75 ns |  20.60 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,331.3 ns |   237.38 ns |  13.01 ns |  1.44 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,371.3 ns** |    **67.07 ns** |   **3.68 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,331.7 ns |   257.85 ns |  14.13 ns |  0.90 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 15,077.7 ns |   434.26 ns |  23.80 ns |  1.45 |    0.00 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **14,634.2 ns** |   **606.34 ns** |  **33.24 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 11,614.7 ns | 2,053.76 ns | 112.57 ns |  0.79 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 20,171.4 ns |   413.53 ns |  22.67 ns |  1.38 |    0.00 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,459.9 ns** |    **61.95 ns** |   **3.40 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  5,800.4 ns |   499.78 ns |  27.39 ns |  1.06 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,123.0 ns |   772.99 ns |  42.37 ns |  1.67 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **8,133.4 ns** |   **111.37 ns** |   **6.10 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,586.6 ns | 4,886.47 ns | 267.84 ns |  0.93 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 12,442.2 ns |   272.64 ns |  14.94 ns |  1.53 |    0.00 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **12,968.0 ns** |   **697.35 ns** |  **38.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,862.6 ns | 2,256.76 ns | 123.70 ns |  0.76 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 17,605.2 ns | 1,927.76 ns | 105.67 ns |  1.36 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **4,013.0 ns** |   **841.13 ns** |  **46.10 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,493.0 ns |   180.85 ns |   9.91 ns |  1.12 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,628.6 ns |   716.77 ns |  39.29 ns |  1.90 |    0.02 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **6,670.7 ns** |    **83.74 ns** |   **4.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,697.7 ns |   166.32 ns |   9.12 ns |  0.85 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 11,139.6 ns |   842.24 ns |  46.17 ns |  1.67 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **10,923.3 ns** |   **689.74 ns** |  **37.81 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  8,166.1 ns |   173.53 ns |   9.51 ns |  0.75 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 16,068.7 ns |   785.41 ns |  43.05 ns |  1.47 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,568.1 ns** |   **113.46 ns** |   **6.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,035.7 ns |    91.26 ns |   5.00 ns |  0.79 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,004.5 ns |   182.86 ns |  10.02 ns |  2.34 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **5,327.3 ns** |   **376.55 ns** |  **20.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,996.6 ns |   530.19 ns |  29.06 ns |  0.56 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  9,589.8 ns |   289.74 ns |  15.88 ns |  1.80 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **9,445.7 ns** |   **179.89 ns** |   **9.86 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  4,194.8 ns |   128.59 ns |   7.05 ns |  0.44 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 14,946.6 ns | 1,432.96 ns |  78.55 ns |  1.58 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **4,001.7 ns** |   **966.47 ns** |  **52.98 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,666.1 ns |    83.02 ns |   4.55 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,364.9 ns |   294.36 ns |  16.13 ns |  1.84 |    0.02 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **6,590.1 ns** |   **912.39 ns** |  **50.01 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,610.9 ns |   388.68 ns |  21.31 ns |  0.70 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 11,218.2 ns |   301.39 ns |  16.52 ns |  1.70 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **10,857.3 ns** | **1,112.99 ns** |  **61.01 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,354.7 ns |   105.59 ns |   5.79 ns |  0.77 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 16,248.6 ns |   433.22 ns |  23.75 ns |  1.50 |    0.01 | 0.1831 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,331.0 ns** |   **362.57 ns** |  **19.87 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    512.8 ns |   166.70 ns |   9.14 ns |  0.39 |    0.01 | 0.0010 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,919.2 ns |   154.84 ns |   8.49 ns |  3.70 |    0.05 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **4,060.8 ns** |   **415.13 ns** |  **22.75 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,273.2 ns |   725.66 ns |  39.78 ns |  0.31 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  8,304.5 ns |   196.08 ns |  10.75 ns |  2.05 |    0.01 | 0.1984 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **8,123.5 ns** | **1,913.03 ns** | **104.86 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,407.1 ns |   228.79 ns |  12.54 ns |  0.30 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 13,332.9 ns |   741.80 ns |  40.66 ns |  1.64 |    0.02 | 0.1984 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method           | Shape       | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **6.283 ms** |   **0.8382 ms** |  **0.0459 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  41.614 ms |   6.2673 ms |  0.3435 ms |  6.62 |    0.06 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  65.733 ms |   1.9923 ms |  0.1092 ms | 10.46 |    0.07 |        - |        - |        - |   41.87 KB |       0.028 |
|                  |             |            |             |            |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |   **1.155 ms** |   **0.9740 ms** |  **0.0534 ms** |  **1.00** |    **0.06** |  **48.8281** |  **48.8281** |  **48.8281** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   5.040 ms |   0.0895 ms |  0.0049 ms |  4.37 |    0.17 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2.502 ms |   0.1880 ms |  0.0103 ms |  2.17 |    0.09 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |            |             |            |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **28.933 ms** |   **2.2140 ms** |  **0.1214 ms** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 462.346 ms |   9.8997 ms |  0.5426 ms | 15.98 |    0.06 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 524.367 ms | 189.5151 ms | 10.3880 ms | 18.12 |    0.32 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **6.216 ms** |  **0.3208 ms** | **0.0176 ms** |  **1.00** |    **0.00** | **242.1875** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  17.615 ms |  0.0320 ms | 0.0018 ms |  2.83 |    0.01 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.222 ms** |  **2.1145 ms** | **0.1159 ms** |  **1.00** |    **0.01** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 108.191 ms | 11.7171 ms | 0.6423 ms |  4.13 |    0.03 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **54.185 ms** | **10.3916 ms** | **0.5696 ms** |  **1.00** |    **0.01** | **200.0000** | **200.0000** | **200.0000** | **3946.85 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 222.909 ms | 33.6892 ms | 1.8466 ms |  4.11 |    0.05 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **125.23 ns** |     **5.400 ns** |   **0.296 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     56.35 ns |     7.346 ns |   0.403 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    123.45 ns |     3.409 ns |   0.187 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     97.28 ns |    64.370 ns |   3.528 ns |  0.78 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **210.86 ns** |    **20.124 ns** |   **1.103 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |     65.50 ns |     0.736 ns |   0.040 ns |  0.31 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    210.48 ns |    20.463 ns |   1.122 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    107.82 ns |    16.698 ns |   0.915 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **268.56 ns** |     **4.255 ns** |   **0.233 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 14   |     68.56 ns |     5.596 ns |   0.307 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    297.85 ns |    11.692 ns |   0.641 ns |  1.11 |    0.00 |         - |          NA |
| Kernel_Cjk | 14   |    113.71 ns |    38.593 ns |   2.115 ns |  0.42 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **379.95 ns** |     **7.862 ns** |   **0.431 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |     71.71 ns |     8.802 ns |   0.482 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    381.62 ns |    64.427 ns |   3.531 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    118.92 ns |     5.933 ns |   0.325 ns |  0.31 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **464.37 ns** |    **24.079 ns** |   **1.320 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.93 ns |     3.109 ns |   0.170 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    462.99 ns |     4.748 ns |   0.260 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    128.26 ns |    40.905 ns |   2.242 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **563.36 ns** |    **39.370 ns** |   **2.158 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     79.33 ns |     0.215 ns |   0.012 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    561.27 ns |     6.612 ns |   0.362 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    130.79 ns |     3.953 ns |   0.217 ns |  0.23 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **765.68 ns** |   **781.614 ns** |  **42.843 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 24   |     88.46 ns |     7.104 ns |   0.389 ns |  0.12 |    0.01 |         - |          NA |
| Dp_Cjk     | 24   |    747.72 ns |   115.991 ns |   6.358 ns |  0.98 |    0.05 |         - |          NA |
| Kernel_Cjk | 24   |    144.16 ns |    12.229 ns |   0.670 ns |  0.19 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,176.77 ns** |   **226.677 ns** |  **12.425 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    103.83 ns |    11.582 ns |   0.635 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,181.48 ns |   149.283 ns |   8.183 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    169.88 ns |    16.859 ns |   0.924 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **2,793.79 ns** |   **295.250 ns** |  **16.184 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    126.25 ns |    16.925 ns |   0.928 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,667.54 ns | 1,668.164 ns |  91.438 ns |  0.95 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    234.47 ns |    11.952 ns |   0.655 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **4,969.27 ns** |   **774.596 ns** |  **42.458 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    153.18 ns |     4.212 ns |   0.231 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  4,953.47 ns |   541.020 ns |  29.655 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    291.83 ns |    20.511 ns |   1.124 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **10,128.33 ns** | **1,993.958 ns** | **109.296 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    346.57 ns |    11.008 ns |   0.603 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,175.78 ns | 1,328.081 ns |  72.797 ns |  1.10 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |    892.43 ns |    10.727 ns |   0.588 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                     | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **28.57 ns** |      **2.163 ns** |   **0.119 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     25.99 ns |      0.489 ns |   0.027 ns |  0.91 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    135.72 ns |      2.549 ns |   0.140 ns |  4.75 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     27.12 ns |      4.640 ns |   0.254 ns |  0.95 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **265.40 ns** |     **10.581 ns** |   **0.580 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    362.51 ns |      9.626 ns |   0.528 ns |  1.37 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    719.04 ns |     59.233 ns |   3.247 ns |  2.71 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    270.81 ns |     33.955 ns |   1.861 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **956.75 ns** |     **13.069 ns** |   **0.716 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,870.52 ns |     79.700 ns |   4.369 ns |  1.96 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,720.48 ns |    133.002 ns |   7.290 ns |  1.80 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    967.31 ns |    249.214 ns |  13.660 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |               |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **12,340.16 ns** |     **52.398 ns** |   **2.872 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 19,126.03 ns | 10,884.014 ns | 596.590 ns |  1.55 |    0.04 |         - |          NA |
| Distance_CodePoint         | 512    | 15,077.77 ns |  1,692.935 ns |  92.795 ns |  1.22 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 12,436.43 ns |    710.400 ns |  38.939 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **25.93 ns** |       **0.728 ns** |      **0.040 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      86.88 ns |       0.456 ns |      0.025 ns |  3.35 |    0.00 | 0.0021 |      56 B |          NA |
| Quickenshtein        | 8      |      88.03 ns |      12.028 ns |      0.659 ns |  3.39 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     170.94 ns |      22.292 ns |      1.222 ns |  6.59 |    0.04 | 0.0050 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **266.42 ns** |      **59.414 ns** |      **3.257 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,087.39 ns |     727.621 ns |     39.883 ns | 19.10 |    0.24 | 0.0076 |     280 B |          NA |
| Quickenshtein        | 64     |   1,199.10 ns |     222.869 ns |     12.216 ns |  4.50 |    0.06 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   8,978.53 ns |     185.627 ns |     10.175 ns | 33.70 |    0.36 | 0.0153 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |  **12,434.03 ns** |   **1,040.596 ns** |     **57.039 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 434,855.86 ns |  15,594.691 ns |    854.798 ns | 34.97 |    0.15 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,661.58 ns |   4,155.103 ns |    227.755 ns |  3.03 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 791,710.68 ns | 826,091.586 ns | 45,280.868 ns | 63.67 |    3.16 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                          | Shape   | Mean            | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |----------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |       **146.35 μs** |       **8.433 μs** |      **0.462 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |   319,641.66 μs | 101,128.373 μs |  5,543.187 μs | 2,184.13 |   33.34 | 500.0000 | 500.0000 | 500.0000 | 31412.47 KB |   12,805.08 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |     **3,343.90 μs** |     **201.152 μs** |     **11.026 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 1,472,219.04 μs | 453,996.658 μs | 24,885.089 μs |   440.27 |    6.57 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |                 |                |               |          |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |        **24.34 μs** |       **3.272 μs** |      **0.179 μs** |     **1.00** |    **0.01** |   **0.0916** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     1,003.85 μs |      27.257 μs |      1.494 μs |    41.25 |    0.27 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |  **10,155.79 μs** |  **1,085.865 μs** |    **59.520 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  33,016.20 μs |  2,302.378 μs |   126.201 μs |     3.25 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    3,927.59 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **16.41 μs** |      **0.532 μs** |     **0.029 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  33,819.95 μs |  1,099.386 μs |    60.261 μs | 2,060.63 |    4.49 | 600.0000 | 600.0000 | 600.0000 |  5090213 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **171,188.97 μs** | **84,861.051 μs** | **4,651.521 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 212,481.99 μs | 21,910.246 μs | 1,200.974 μs |     1.24 |    0.03 |        - |        - |        - | 23228972 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **237.01 μs** |      **5.101 μs** |     **0.280 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 211,040.29 μs | 34,024.548 μs | 1,865.000 μs |   890.42 |    6.87 |        - |        - |        - | 23228008 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method    | Samples | Mean        | Error     | StdDev    | Allocated |
|---------- |-------- |------------:|----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **49.85 ms** |  **12.96 ms** |  **0.710 ms** |         **-** |
| OneVsOne  | 100000  |    87.97 ms |  29.29 ms |  1.605 ms |  803531 B |
| **OneVsRest** | **1000000** |   **846.74 ms** | **136.03 ms** |  **7.456 ms** |         **-** |
| OneVsOne  | 1000000 | 1,255.41 ms | 536.27 ms | 29.395 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|------------:|
| PerLabel         | 100000 |  7.044 ms |  0.3609 ms | 0.0198 ms |         - |         - |        - |     7.84 KB |
| PerLabelWeighted | 100000 | 10.638 ms |  0.8929 ms | 0.0489 ms |         - |         - |        - |     7.85 KB |
| PerSample        | 100000 | 75.120 ms | 19.9700 ms | 1.0946 ms | 2000.0000 | 1857.1429 | 571.4286 | 39063.01 KB |
| PerClass         | 100000 |  4.136 ms |  0.5827 ms | 0.0319 ms |         - |         - |        - |     7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                | RowCount | BatchCount | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-------------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **267.61 μs** |     **7.083 μs** |   **0.388 μs** |  **1.00** |    **0.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   282.35 μs |    29.832 μs |   1.635 μs |  1.06 |    0.01 |      - |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    95.68 μs |     1.892 μs |   0.104 μs |  0.36 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   269.07 μs |     8.590 μs |   0.471 μs |  1.01 |    0.00 |      - |     800 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **273.01 μs** |   **183.688 μs** |  **10.069 μs** |  **1.00** |    **0.04** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   318.35 μs |     6.706 μs |   0.368 μs |  1.17 |    0.04 | 2.9297 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    96.38 μs |     2.097 μs |   0.115 μs |  0.35 |    0.01 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   273.23 μs |     5.357 μs |   0.294 μs |  1.00 |    0.03 |      - |     800 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **2,756.36 μs** |   **147.451 μs** |   **8.082 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 3,105.35 μs |   252.720 μs |  13.852 μs |  1.13 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,329.69 μs |   191.836 μs |  10.515 μs |  0.48 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 2,798.70 μs |   481.991 μs |  26.420 μs |  1.02 |    0.01 |      - |     803 B |        1.00 |
|                       |          |            |             |              |            |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,767.88 μs** |   **270.857 μs** |  **14.847 μs** |  **1.00** |    **0.01** |      **-** |     **802 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 3,151.81 μs | 2,483.356 μs | 136.121 μs |  1.14 |    0.04 |      - |   77627 B |       96.79 |
| SparseFit             | 100000   | 100        | 1,316.87 μs |   106.850 μs |   5.857 μs |  0.48 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 2,790.33 μs |   410.728 μs |  22.513 μs |  1.01 |    0.01 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 10.23 ms | 3.494 ms | 0.191 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 15.29 ms | 3.819 ms | 0.209 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method             | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------------------- |---------:|----------:|---------:|---------:|----------:|
| NormalizeDocuments | 14.61 ms |  0.323 ms | 0.018 ms | 109.3750 |   2.75 MB |
| EncodeDocuments    | 71.52 ms | 11.175 ms | 0.613 ms | 375.0000 |  10.74 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                     | Shape   | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **11,004.34 μs** | **4,273.875 μs** | **234.266 μs** |  **1.00** |    **0.03** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 18,359.11 μs | 4,253.165 μs | 233.130 μs |  1.67 |    0.04 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **149.28 μs** |     **9.747 μs** |   **0.534 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    230.38 μs |     3.643 μs |   0.200 μs |  1.54 |    0.00 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,394.92 μs** |   **259.484 μs** |  **14.223 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,209.41 μs |    65.592 μs |   3.595 μs |  0.95 |    0.00 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **23.80 μs** |     **3.336 μs** |   **0.183 μs** |  **1.00** |    **0.01** |   **0.0916** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     30.73 μs |     2.077 μs |   0.114 μs |  1.29 |    0.01 |   0.0610 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **7.293 ms** | **0.3213 ms** | **0.0176 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 7.098 ms | 1.0437 ms | 0.0572 ms |  0.97 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **8.208 ms** | **1.5779 ms** | **0.0865 ms** |  **1.00** |     **220 B** |        **1.00** |
| ExtractOne | 5     | 7.104 ms | 0.7397 ms | 0.0405 ms |  0.87 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method                            | Rows   | Mean      | Error      | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|-----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 32.823 ms |  5.5594 ms | 0.3047 ms |       - |       - |       - |  800342 B |
| NdcgIgnoringTies                  | 100000 | 29.749 ms |  0.5322 ms | 0.0292 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 19.508 ms | 12.6956 ms | 0.6959 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 15.235 ms |  0.9328 ms | 0.0511 ms |       - |       - |       - |     180 B |
| CoverageErrorScore                | 100000 |  6.478 ms |  0.6804 ms | 0.0373 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 53.235 ms |  0.6748 ms | 0.0370 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method          | Model   | Mean     | Error   | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|--------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **147.4 ms** | **6.55 ms** | **0.36 ms** | **1500.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **141.5 ms** | **7.07 ms** | **0.39 ms** | **1500.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method       | Model         | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **19.51 ms** |   **4.084 ms** | **0.224 ms** |  **1.00** |    **0.01** |  **343.7500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  54.27 ms |  14.363 ms | 0.787 ms |  2.78 |    0.04 |  100.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **55.09 ms** |  **16.452 ms** | **0.902 ms** |  **1.00** |    **0.02** |  **300.0000** |   **7.98 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  74.18 ms |  77.851 ms | 4.267 ms |  1.35 |    0.07 |  222.2222 |   5.65 MB |        0.71 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **88.43 ms** |   **0.830 ms** | **0.045 ms** |  **1.00** |    **0.00** | **1166.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 275.32 ms | 154.775 ms | 8.484 ms |  3.11 |    0.08 | 2000.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-CZMXIK : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **1000**    |     **75.37 μs** |     **1.035 μs** |   **1.151 μs** | **0.1221** |   **3.12 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 1000    |     95.48 μs |     0.704 μs |   0.810 μs | 0.7324 |  20.55 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 1000    |     88.74 μs |     0.149 μs |   0.153 μs | 0.6104 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     76.99 μs |    14.010 μs |   0.768 μs | 0.1221 |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |    104.07 μs |     2.014 μs |   0.110 μs | 0.7324 |  20.55 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     90.01 μs |    12.490 μs |   0.685 μs | 0.6104 |  15.52 KB |
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **10000**   |    **686.88 μs** |     **2.160 μs** |   **2.121 μs** |      **-** |   **3.12 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 10000   |  1,095.69 μs |    26.080 μs |  30.034 μs |      - |  26.13 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 10000   |  1,065.85 μs |     9.446 μs |   9.277 μs |      - |  21.15 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    667.35 μs |   184.525 μs |  10.114 μs |      - |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |  1,246.07 μs |   789.927 μs |  43.299 μs |      - |  26.13 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |  1,111.43 μs |   250.434 μs |  13.727 μs |      - |  21.15 KB |
| **UpsertThenSearch** | **Job-CZMXIK** | **20**             | **Throughput**  | **5**           | **100000**  | **12,916.27 μs** |   **164.961 μs** | **189.969 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-CZMXIK | 20             | Throughput  | 5           | 100000  | 17,404.09 μs |   176.540 μs | 203.304 μs |      - |  45.99 KB |
| Hybrid           | Job-CZMXIK | 20             | Throughput  | 5           | 100000  | 17,070.23 μs |   118.007 μs | 131.165 μs |      - |  41.05 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  | 12,884.17 μs | 3,253.045 μs | 178.310 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 17,956.78 μs | 2,724.424 μs | 149.335 μs |      - |     46 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 17,621.79 μs | 5,784.340 μs | 317.059 μs |      - |  41.06 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-11-16, measured at commit `c944fbc8ff204937b33c044d6d0924ec8795f37b`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.925 ms** |   **0.3273 ms** |  **0.0179 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  55.795 ms |  41.5075 ms |  2.2752 ms | 14.22 |    0.51 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **18.175 ms** |   **2.7121 ms** |  **0.1487 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 389.049 ms | 476.1196 ms | 26.0977 ms | 21.41 |    1.25 | 72000.0000 | 72000.0000 | 72000.0000 | 324.38 MB |       40.00 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.162 | 0.028 | 0.17x | 0.162 | 0.028 | 0.17x |
| cdist_wratio_n50 | 2.481 | 2.248 | 0.91x | 2.481 | 2.248 | 0.91x |
| cdist_ratio_varied_n50 | 0.138 | 0.023 | 0.17x | 0.138 | 0.023 | 0.17x |
| cdist_ratio_varied_cutoff90_n50 | 0.054 | 0.023 | 0.43x | 0.054 | 0.023 | 0.43x |
| cdist_ratio_n200 | 2.878 | 0.324 | 0.11x | 2.882 | 0.324 | 0.11x |
| cdist_wratio_n200 | 37.061 | 33.514 | 0.90x | 37.063 | 33.512 | 0.90x |
| cdist_ratio_varied_n200 | 2.499 | 0.263 | 0.11x | 2.503 | 0.263 | 0.10x |
| cdist_ratio_varied_cutoff90_n200 | 0.996 | 0.263 | 0.26x | 0.998 | 0.263 | 0.26x |
| cdist_ratio_n500 | 17.221 | 1.932 | 0.11x | 17.248 | 1.932 | 0.11x |
| cdist_wratio_n500 | 215.057 | 200.785 | 0.93x | 215.175 | 200.780 | 0.93x |
| cdist_ratio_varied_n500 | 18.961 | 1.533 | 0.08x | 19.039 | 1.533 | 0.08x |
| cdist_ratio_varied_cutoff90_n500 | 5.783 | 1.533 | 0.27x | 5.865 | 1.533 | 0.26x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 106.4 | 22.0 | 4.83x C# faster |
| latin | 32 | 147.5 | 87.7 | 1.68x C# faster |
| latin | 128 | 472.3 | 415.7 | 1.14x C# faster |
| latin | 512 | 4801.2 | 5122.6 | 1.07x Py faster |
| cjk | 8 | 123.0 | 21.9 | 5.61x C# faster |
| cjk | 32 | 274.5 | 222.1 | 1.24x C# faster |
| cjk | 128 | 1788.0 | 1308.8 | 1.37x C# faster |
| cjk | 512 | 16352.9 | 8429.5 | 1.94x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.16)
C#:     Lodestar.Text on .NET 10.0.15 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 127.8 | 19.1 | 6.71x C# faster |
| latin | 32 | 220.1 | 153.8 | 1.43x C# faster |
| latin | 128 | 1826.5 | 921.3 | 1.98x C# faster |
| latin | 512 | 15567.7 | 12859.3 | 1.21x C# faster |
| cjk | 8 | 135.9 | 19.0 | 7.16x C# faster |
| cjk | 32 | 352.8 | 288.9 | 1.22x C# faster |
| cjk | 128 | 2980.5 | 2162.5 | 1.38x C# faster |
| cjk | 512 | 26224.4 | 21475.0 | 1.22x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.003 | 0.815 | 240.59x | 0.003 | 0.815 | 240.59x |
| accuracy_n1000_k2 | 0.000 | 0.426 | 2606.24x | 0.000 | 0.426 | 2606.07x |
| precision_recall_f1_macro_n1000_k2 | 0.004 | 1.518 | 405.23x | 0.004 | 1.518 | 405.23x |
| classification_report_n1000_k2 | 0.006 | 5.896 | 930.27x | 0.006 | 5.896 | 930.27x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.636 | 93.39x | 0.018 | 1.636 | 93.39x |
| balanced_accuracy_n1000_k2 | 0.004 | 0.875 | 246.32x | 0.004 | 0.874 | 246.31x |
| matthews_n1000_k2 | 0.004 | 1.674 | 473.73x | 0.004 | 1.674 | 473.70x |
| cohen_kappa_n1000_k2 | 0.004 | 0.918 | 258.65x | 0.004 | 0.917 | 258.65x |
| mse_n1000_k2 | 0.001 | 0.230 | 407.07x | 0.001 | 0.230 | 407.08x |
| mae_n1000_k2 | 0.001 | 0.230 | 421.84x | 0.001 | 0.230 | 421.82x |
| median_ae_n1000_k2 | 0.005 | 0.244 | 45.50x | 0.005 | 0.244 | 45.50x |
| r2_n1000_k2 | 0.001 | 0.282 | 193.47x | 0.001 | 0.282 | 193.46x |
| confusion_matrix_n1000_k10 | 0.004 | 0.817 | 227.94x | 0.004 | 0.817 | 227.94x |
| accuracy_n1000_k10 | 0.000 | 0.428 | 2618.51x | 0.000 | 0.428 | 2618.54x |
| precision_recall_f1_macro_n1000_k10 | 0.004 | 1.516 | 352.89x | 0.004 | 1.516 | 352.90x |
| classification_report_n1000_k10 | 0.011 | 6.045 | 569.42x | 0.011 | 6.045 | 569.45x |
| roc_auc_ovr_macro_n1000_k10 | 0.488 | 8.482 | 17.37x | 0.488 | 8.481 | 17.37x |
| balanced_accuracy_n1000_k10 | 0.004 | 0.886 | 219.97x | 0.004 | 0.886 | 219.97x |
| matthews_n1000_k10 | 0.004 | 1.711 | 428.31x | 0.004 | 1.711 | 428.29x |
| cohen_kappa_n1000_k10 | 0.004 | 0.929 | 213.34x | 0.004 | 0.929 | 213.33x |
| mse_n1000_k10 | 0.001 | 0.231 | 407.43x | 0.001 | 0.231 | 407.39x |
| mae_n1000_k10 | 0.001 | 0.229 | 420.27x | 0.001 | 0.229 | 420.25x |
| median_ae_n1000_k10 | 0.005 | 0.244 | 45.23x | 0.005 | 0.244 | 45.23x |
| r2_n1000_k10 | 0.001 | 0.280 | 192.59x | 0.001 | 0.280 | 192.59x |
| confusion_matrix_n100000_k2 | 0.321 | 10.979 | 34.24x | 0.321 | 10.979 | 34.24x |
| accuracy_n100000_k2 | 0.017 | 3.762 | 221.55x | 0.017 | 3.762 | 221.53x |
| precision_recall_f1_macro_n100000_k2 | 0.331 | 12.732 | 38.49x | 0.331 | 12.732 | 38.49x |
| classification_report_n100000_k2 | 0.333 | 27.532 | 82.70x | 0.333 | 27.531 | 82.70x |
| roc_auc_binary_n100000_k2 | 4.904 | 26.239 | 5.35x | 4.904 | 26.236 | 5.35x |
| balanced_accuracy_n100000_k2 | 0.331 | 11.110 | 33.62x | 0.331 | 11.110 | 33.61x |
| matthews_n100000_k2 | 0.330 | 23.009 | 69.66x | 0.330 | 23.009 | 69.64x |
| cohen_kappa_n100000_k2 | 0.321 | 11.280 | 35.17x | 0.321 | 11.279 | 35.17x |
| mse_n100000_k2 | 0.056 | 0.485 | 8.65x | 0.056 | 0.485 | 8.65x |
| mae_n100000_k2 | 0.055 | 0.505 | 9.26x | 0.055 | 0.505 | 9.26x |
| median_ae_n100000_k2 | 0.640 | 1.684 | 2.63x | 0.658 | 1.684 | 2.56x |
| r2_n100000_k2 | 0.141 | 0.737 | 5.21x | 0.141 | 0.737 | 5.21x |
| confusion_matrix_n100000_k10 | 0.330 | 11.062 | 33.49x | 0.330 | 11.059 | 33.48x |
| accuracy_n100000_k10 | 0.017 | 3.792 | 223.20x | 0.017 | 3.792 | 223.20x |
| precision_recall_f1_macro_n100000_k10 | 0.331 | 13.193 | 39.80x | 0.331 | 13.193 | 39.80x |
| classification_report_n100000_k10 | 0.337 | 29.789 | 88.38x | 0.337 | 29.788 | 88.37x |
| roc_auc_ovr_macro_n100000_k10 | 41.408 | 214.016 | 5.17x | 41.409 | 214.005 | 5.17x |
| balanced_accuracy_n100000_k10 | 0.330 | 11.205 | 33.94x | 0.330 | 11.204 | 33.94x |
| matthews_n100000_k10 | 0.332 | 22.861 | 68.86x | 0.332 | 22.859 | 68.86x |
| cohen_kappa_n100000_k10 | 0.323 | 11.304 | 35.01x | 0.323 | 11.304 | 35.01x |
| mse_n100000_k10 | 0.056 | 0.484 | 8.69x | 0.056 | 0.484 | 8.69x |
| mae_n100000_k10 | 0.054 | 0.506 | 9.34x | 0.054 | 0.506 | 9.34x |
| median_ae_n100000_k10 | 0.681 | 1.677 | 2.46x | 0.745 | 1.677 | 2.25x |
| r2_n100000_k10 | 0.140 | 0.731 | 5.23x | 0.140 | 0.731 | 5.23x |
| confusion_matrix_n1000000_k2 | 3.258 | 104.896 | 32.20x | 3.258 | 104.872 | 32.19x |
| accuracy_n1000000_k2 | 0.253 | 34.771 | 137.69x | 0.253 | 34.760 | 137.65x |
| precision_recall_f1_macro_n1000000_k2 | 3.259 | 114.579 | 35.16x | 3.259 | 114.561 | 35.16x |
| classification_report_n1000000_k2 | 3.262 | 231.101 | 70.85x | 3.262 | 231.097 | 70.85x |
| roc_auc_binary_n1000000_k2 | 76.799 | 295.332 | 3.85x | 76.796 | 295.318 | 3.85x |
| balanced_accuracy_n1000000_k2 | 3.254 | 106.207 | 32.64x | 3.254 | 106.201 | 32.64x |
| matthews_n1000000_k2 | 3.251 | 213.479 | 65.66x | 3.251 | 213.466 | 65.66x |
| cohen_kappa_n1000000_k2 | 3.233 | 106.641 | 32.98x | 3.233 | 106.640 | 32.99x |
| mse_n1000000_k2 | 0.567 | 2.986 | 5.27x | 0.567 | 2.986 | 5.27x |
| mae_n1000000_k2 | 0.553 | 4.095 | 7.41x | 0.553 | 4.095 | 7.41x |
| median_ae_n1000000_k2 | 7.000 | 15.389 | 2.20x | 7.128 | 15.388 | 2.16x |
| r2_n1000000_k2 | 1.506 | 4.868 | 3.23x | 1.506 | 4.868 | 3.23x |
| confusion_matrix_n1000000_k10 | 3.249 | 105.904 | 32.60x | 3.249 | 105.898 | 32.60x |
| accuracy_n1000000_k10 | 0.230 | 34.983 | 151.88x | 0.230 | 34.980 | 151.88x |
| precision_recall_f1_macro_n1000000_k10 | 3.261 | 121.999 | 37.42x | 3.260 | 121.992 | 37.42x |
| classification_report_n1000000_k10 | 3.264 | 251.285 | 76.99x | 3.264 | 251.280 | 76.99x |
| balanced_accuracy_n1000000_k10 | 3.250 | 105.787 | 32.55x | 3.249 | 105.778 | 32.55x |
| matthews_n1000000_k10 | 3.259 | 218.005 | 66.90x | 3.259 | 217.994 | 66.90x |
| cohen_kappa_n1000000_k10 | 3.256 | 105.473 | 32.40x | 3.256 | 105.471 | 32.40x |
| mse_n1000000_k10 | 0.564 | 2.794 | 4.96x | 0.564 | 2.794 | 4.96x |
| mae_n1000000_k10 | 0.559 | 4.317 | 7.73x | 0.559 | 4.317 | 7.72x |
| median_ae_n1000000_k10 | 7.735 | 15.587 | 2.02x | 7.997 | 15.586 | 1.95x |
| r2_n1000000_k10 | 1.420 | 5.414 | 3.81x | 1.419 | 5.413 | 3.81x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'tokenizers': '0.23.3', 'sentencepiece': '0.2.3', 'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.541 | 9.615 | 2.12x | 4.697 | 9.614 | 2.05x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.596 | 16.707 | 1.33x | 12.963 | 16.706 | 1.29x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.385 | 51.060 | 4.12x | 12.509 | 51.054 | 4.08x | 1,990,038 | 1,990,038 |
| spiece_model | 6.358 | 15.608 | 2.45x | 6.591 | 15.607 | 2.37x | 347,061 | 347,061 |
| tfidf_save | 1.761 | 2.610 | 1.48x | 1.770 | 2.610 | 1.47x | 581,787 | 591,922 |
| tfidf_load | 4.749 | 3.908 | 0.82x | 4.914 | 3.907 | 0.80x | 581,787 | 591,922 |
| embedding_index_save | 6.245 | 2.573 | 0.41x | 6.591 | 2.572 | 0.39x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.478 | 37.329 | 0.75x | 10.144 | 5.378 | 0.53x | 20,589,007 | 15,360,128 |
| embedding_index_load | 9.729 | 1.957 | 0.20x | 10.358 | 1.956 | 0.19x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 9.439 | 1.359 | 0.14x | 10.077 | 1.359 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 7.530 | 1.906 | 0.25x | 7.959 | 1.903 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 4.119 | 1.818 | 0.44x | 4.217 | 1.817 | 0.43x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 76.82x | 0.000 | 0.001 | 76.82x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 428.062 | 547.478 | 1.28x | 428.031 | 547.440 | 1.28x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 72.885 | 70.033 | 0.96x | 72.882 | 70.032 | 0.96x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.106 | 0.110 | 1.04x | 0.106 | 0.110 | 1.04x |
| stratified_n10000 | 0.175 | 0.803 | 4.60x | 0.175 | 0.803 | 4.60x |
| traintest_n10000 | 0.005 | 0.189 | 35.60x | 0.005 | 0.189 | 35.59x |
| kfold_seeded_n10000 | 0.325 | 0.676 | 2.08x | 0.325 | 0.676 | 2.08x |
| stratified_seeded_n10000 | 0.405 | 1.466 | 3.62x | 0.405 | 1.466 | 3.62x |
| traintest_seeded_n10000 | 0.159 | 0.451 | 2.83x | 0.159 | 0.451 | 2.83x |
| stratified_traintest_n10000 | 0.678 | 2.543 | 3.75x | 0.678 | 2.543 | 3.75x |
| group_kfold_n10000 | 0.136 | 0.568 | 4.17x | 0.136 | 0.568 | 4.17x |
| stratified_group_kfold_n10000 | 0.242 | 25.782 | 106.45x | 0.242 | 25.780 | 106.44x |
| timeseries_n10000 | 0.000 | 0.012 | 89.97x | 0.000 | 0.012 | 89.97x |
| repeated_kfold_n10000 | 1.019 | 1.859 | 1.82x | 1.019 | 1.859 | 1.82x |
| kfold_n100000 | 1.231 | 2.284 | 1.86x | 1.329 | 2.284 | 1.72x |
| stratified_n100000 | 1.895 | 7.565 | 3.99x | 1.992 | 7.561 | 3.80x |
| traintest_n100000 | 0.153 | 0.438 | 2.87x | 0.154 | 0.438 | 2.85x |
| kfold_seeded_n100000 | 3.720 | 7.419 | 1.99x | 3.832 | 7.411 | 1.93x |
| stratified_seeded_n100000 | 4.252 | 13.464 | 3.17x | 4.388 | 13.457 | 3.07x |
| traintest_seeded_n100000 | 2.052 | 1.860 | 0.91x | 2.084 | 1.860 | 0.89x |
| stratified_traintest_n100000 | 8.314 | 22.965 | 2.76x | 8.379 | 22.963 | 2.74x |
| group_kfold_n100000 | 1.558 | 6.726 | 4.32x | 1.654 | 6.712 | 4.06x |
| stratified_group_kfold_n100000 | 2.555 | 261.220 | 102.25x | 2.654 | 261.215 | 98.42x |
| timeseries_n100000 | 0.000 | 0.045 | 353.31x | 0.000 | 0.045 | 353.31x |
| repeated_kfold_n100000 | 10.625 | 21.652 | 2.04x | 10.801 | 21.651 | 2.00x |
| kfold_n1000000 | 12.918 | 23.625 | 1.83x | 13.644 | 23.585 | 1.73x |
| stratified_n1000000 | 21.026 | 79.599 | 3.79x | 21.903 | 79.594 | 3.63x |
| traintest_n1000000 | 0.604 | 2.644 | 4.38x | 0.664 | 2.644 | 3.98x |
| kfold_seeded_n1000000 | 40.734 | 68.328 | 1.68x | 41.287 | 68.325 | 1.65x |
| stratified_seeded_n1000000 | 46.299 | 124.428 | 2.69x | 47.466 | 124.418 | 2.62x |
| traintest_seeded_n1000000 | 21.881 | 20.443 | 0.93x | 22.244 | 20.439 | 0.92x |
| stratified_traintest_n1000000 | 96.652 | 236.150 | 2.44x | 96.941 | 236.138 | 2.44x |
| group_kfold_n1000000 | 15.348 | 63.746 | 4.15x | 15.819 | 63.741 | 4.03x |
| stratified_group_kfold_n1000000 | 28.472 | 2663.355 | 93.54x | 29.685 | 2663.204 | 89.72x |
| timeseries_n1000000 | 0.000 | 0.378 | 3121.64x | 0.000 | 0.377 | 3119.15x |
| repeated_kfold_n1000000 | 116.769 | 197.645 | 1.69x | 118.451 | 197.631 | 1.67x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.009 | 0.641 | 72.06x | 0.009 | 0.641 | 72.06x |
| mann_whitney_n1000 | 0.050 | 0.625 | 12.62x | 0.050 | 0.625 | 12.62x |
| chi_square_n1000 | 0.000 | 0.248 | 1342.33x | 0.000 | 0.248 | 1342.21x |
| fligner_n1000 | 0.126 | 0.704 | 5.58x | 0.126 | 0.704 | 5.58x |
| anderson_ksamp_n1000 | 0.201 | 0.427 | 2.12x | 0.201 | 0.427 | 2.12x |
| spearman_matrix_n1000 | 0.187 | 0.723 | 3.87x | 0.187 | 0.723 | 3.87x |
| pointbiserial_n1000 | 0.010 | 0.463 | 45.32x | 0.010 | 0.463 | 45.32x |
| welch_t_n10000 | 0.066 | 0.694 | 10.50x | 0.066 | 0.694 | 10.50x |
| mann_whitney_n10000 | 0.535 | 2.483 | 4.64x | 0.535 | 2.483 | 4.64x |
| chi_square_n10000 | 0.001 | 0.252 | 212.39x | 0.001 | 0.252 | 212.37x |
| fligner_n10000 | 1.378 | 2.973 | 2.16x | 1.433 | 2.973 | 2.07x |
| anderson_ksamp_n10000 | 2.103 | 4.065 | 1.93x | 2.188 | 4.065 | 1.86x |
| spearman_matrix_n10000 | 2.227 | 4.168 | 1.87x | 2.270 | 4.168 | 1.84x |
| pointbiserial_n10000 | 0.117 | 0.549 | 4.70x | 0.117 | 0.549 | 4.70x |
| welch_t_n100000 | 0.643 | 1.329 | 2.07x | 0.643 | 1.329 | 2.07x |
| mann_whitney_n100000 | 6.039 | 24.584 | 4.07x | 6.039 | 24.584 | 4.07x |
| chi_square_n100000 | 0.012 | 0.272 | 22.74x | 0.012 | 0.272 | 22.74x |
| fligner_n100000 | 16.949 | 29.694 | 1.75x | 17.336 | 29.692 | 1.71x |
| anderson_ksamp_n100000 | 17.770 | 42.620 | 2.40x | 18.268 | 42.616 | 2.33x |
| spearman_matrix_n100000 | 20.933 | 45.517 | 2.17x | 21.371 | 45.516 | 2.13x |
| pointbiserial_n100000 | 1.355 | 1.774 | 1.31x | 1.369 | 7.075 | 5.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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

_As of 2026-11-17, measured at commit `e501d10d5f1f806836c305d556184ea4d8a7e60a`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.210 | 0.401 | 1.91x | 0.211 | 0.401 | 1.90x |
| polynomial_n10000 | 0.616 | 0.864 | 1.40x | 0.619 | 0.863 | 1.39x |
| kbins_fit_n10000 | 1.472 | 2.195 | 1.49x | 1.471 | 2.195 | 1.49x |
| kbins_n10000 | 0.495 | 3.863 | 7.81x | 0.574 | 3.861 | 6.72x |
| quantile_fit_n10000 | 1.580 | 3.537 | 2.24x | 1.580 | 3.537 | 2.24x |
| quantile_n10000 | 2.098 | 1.779 | 0.85x | 2.102 | 1.778 | 0.85x |
| power_fit_n10000 | 5.944 | 88.306 | 14.86x | 5.946 | 88.301 | 14.85x |
| power_n10000 | 1.605 | 1.072 | 0.67x | 1.608 | 1.072 | 0.67x |
| knn_impute_n10000 | 5.846 | 29.944 | 5.12x | 5.846 | 118.434 | 20.26x |
| label_n10000 | 1.287 | 0.802 | 0.62x | 1.287 | 0.802 | 0.62x |
| normalize_n100000 | 1.690 | 1.922 | 1.14x | 2.080 | 1.922 | 0.92x |
| polynomial_n100000 | 4.251 | 7.041 | 1.66x | 4.996 | 7.040 | 1.41x |
| kbins_fit_n100000 | 14.398 | 8.363 | 0.58x | 15.292 | 8.363 | 0.55x |
| kbins_n100000 | 5.544 | 16.545 | 2.98x | 6.344 | 16.545 | 2.61x |
| quantile_fit_n100000 | 14.606 | 16.911 | 1.16x | 15.496 | 16.910 | 1.09x |
| quantile_n100000 | 19.095 | 17.420 | 0.91x | 19.690 | 17.419 | 0.88x |
| power_fit_n100000 | 65.133 | 668.468 | 10.26x | 68.587 | 668.468 | 9.75x |
| power_n100000 | 14.347 | 8.056 | 0.56x | 14.943 | 8.056 | 0.54x |
| knn_impute_n100000 | 5.841 | 26.614 | 4.56x | 5.841 | 105.230 | 18.02x |
| label_n100000 | 16.248 | 8.020 | 0.49x | 16.706 | 8.019 | 0.48x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-11-16, measured at commit `355c69cb791e6b85f3cc0bf0a325310dbcf09f09`._

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
