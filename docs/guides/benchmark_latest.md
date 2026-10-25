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

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method      | Texts  | TextLength | Device               | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.645 ms** |  **0.9078 ms** | **0.0498 ms** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   4.765 ms |  3.4168 ms | 0.1873 ms |  1.80 |    0.07 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.282 ms |  2.1732 ms | 0.1191 ms |  2.00 |    0.05 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.116 ms** |  **0.0559 ms** | **0.0031 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   8.140 ms |  2.1307 ms | 0.1168 ms |  0.67 |    0.01 |        - |        - |        - |    42840 B |    2,677.50 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  11.192 ms |  7.0395 ms | 0.3859 ms |  0.92 |    0.03 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |  169,441.19 |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **51.306 ms** |  **1.1179 ms** | **0.0613 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **102 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  42.655 ms | 44.6622 ms | 2.4481 ms |  0.83 |    0.04 |        - |        - |        - |   802891 B |    7,871.48 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  53.512 ms | 47.5592 ms | 2.6069 ms |  1.04 |    0.04 | 400.0000 | 400.0000 | 400.0000 |  8071871 B |   79,135.99 |
|             |        |            |                      |            |            |           |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **241.133 ms** |  **1.9517 ms** | **0.1070 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 147.124 ms | 62.5500 ms | 3.4286 ms |  0.61 |    0.01 |        - |        - |        - |   803080 B |    2,355.07 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 235.585 ms | 76.7675 ms | 4.2079 ms |  0.98 |    0.02 | 500.0000 | 500.0000 | 500.0000 | 52872052 B |  155,050.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,739.222 ms** |   **788.8929 ms** |  **43.2419 ms** | **1.000** |    **0.03** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,728.405 ms |   758.9168 ms |  41.5988 ms | 0.994 |    0.03 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.584 ms |     0.1477 ms |   0.0081 ms | 0.001 |    0.00 |   326.1719 |   326.1719 |   326.1719 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,117.922 ms** | **4,117.2223 ms** | **225.6788 ms** | **1.001** |    **0.04** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **37.82 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  6,932.132 ms | 2,441.2498 ms | 133.8131 ms | 0.975 |    0.03 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.94 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      7.482 ms |     2.8566 ms |   0.1566 ms | 0.001 |    0.00 |   484.3750 |   484.3750 |   484.3750 |   5.86 MB |        0.15 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,167.498 ms** | **3,722.2665 ms** | **204.0300 ms** | **1.001** |    **0.03** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,328.069 ms | 3,322.3455 ms | 182.1090 ms | 1.023 |    0.03 |  1000.0000 |  1000.0000 |  1000.0000 |  42.13 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      7.990 ms |     1.2302 ms |   0.0674 ms | 0.001 |    0.00 |   359.3750 |   359.3750 |   359.3750 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **29,090.894 ms** | **4,188.8583 ms** | **229.6055 ms** | **1.000** |    **0.01** | **16000.0000** | **15000.0000** | **14000.0000** | **388.75 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 28,909.363 ms | 7,347.1823 ms | 402.7239 ms | 0.994 |    0.01 | 19000.0000 | 18000.0000 | 17000.0000 | 385.09 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     19.453 ms |     1.4479 ms |   0.0794 ms | 0.001 |    0.00 |   312.5000 |   312.5000 |   312.5000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.CosineTopKSelectionBenchmarks-report-github

_As of 2026-10-25, measured at commit `b7818719585e35e8c1448884e1c36a152a3cd170`._

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

| Method       | Documents | K    | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Allocated   | Alloc Ratio |
|------------- |---------- |----- |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|------------:|------------:|
| **SimdBaseline** | **10000**     | **10**   | **CPUAc(...) GPU) [26]** |       **8.479 ms** |      **0.5641 ms** |     **0.0309 ms** |     **1.00** |    **0.00** |    **26.02 KB** |        **1.00** |
| GpuResident  | 10000     | 10   | CPUAc(...) GPU) [26] |  26,692.585 ms |  4,250.1151 ms |   232.9631 ms | 3,147.95 |   25.79 |  1520.07 KB |       58.43 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **100**  | **CPUAc(...) GPU) [26]** |       **9.084 ms** |      **0.5902 ms** |     **0.0324 ms** |     **1.00** |    **0.00** |    **37.27 KB** |        **1.00** |
| GpuResident  | 10000     | 100  | CPUAc(...) GPU) [26] |  27,129.368 ms |  7,874.6844 ms |   431.6380 ms | 2,986.39 |   42.16 |  1541.63 KB |       41.37 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **1000** | **CPUAc(...) GPU) [26]** |      **13.641 ms** |      **0.1274 ms** |     **0.0070 ms** |     **1.00** |    **0.00** |   **149.77 KB** |        **1.00** |
| GpuResident  | 10000     | 1000 | CPUAc(...) GPU) [26] |  31,855.072 ms |  9,890.5393 ms |   542.1338 ms | 2,335.19 |   34.43 |   1765.7 KB |       11.79 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **10**   | **CPUAc(...) GPU) [26]** |     **121.644 ms** |     **15.2202 ms** |     **0.8343 ms** |     **1.00** |    **0.01** |     **26.2 KB** |        **1.00** |
| GpuResident  | 100000    | 10   | CPUAc(...) GPU) [26] | 261,675.572 ms | 20,343.0269 ms | 1,115.0700 ms | 2,151.23 |   15.03 |  39279.2 KB |    1,499.19 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **100**  | **CPUAc(...) GPU) [26]** |     **127.896 ms** |     **32.2741 ms** |     **1.7691 ms** |     **1.00** |    **0.02** |     **37.5 KB** |        **1.00** |
| GpuResident  | 100000    | 100  | CPUAc(...) GPU) [26] | 258,529.330 ms | 62,412.6367 ms | 3,421.0472 ms | 2,021.66 |   33.38 |  39301.7 KB |    1,048.05 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **1000** | **CPUAc(...) GPU) [26]** |     **132.921 ms** |     **20.3016 ms** |     **1.1128 ms** |     **1.00** |    **0.01** |   **150.08 KB** |        **1.00** |
| GpuResident  | 100000    | 1000 | CPUAc(...) GPU) [26] | 266,315.941 ms | 39,079.5304 ms | 2,142.0809 ms | 2,003.66 |   20.17 | 39525.48 KB |      263.36 |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **964.6 ns** |         **12.26 ns** |       **0.67 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         800.8 ns |         14.32 ns |       0.79 ns |     0.83 |    0.00 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,761.5 ns |        811.99 ns |      44.51 ns |     2.86 |    0.04 |   0.2441 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,938.1 ns |        222.11 ns |      12.17 ns |     4.08 |    0.01 |   0.1144 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,312.7 ns |         45.29 ns |       2.48 ns |     5.51 |    0.00 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,642.8 ns |      3,325.77 ns |     182.30 ns |    11.03 |    0.16 |        - |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,308.8 ns |          2.21 ns |       0.12 ns |     5.50 |    0.00 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,248.7 ns** |      **3,017.57 ns** |     **165.40 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      57,285.7 ns |     12,086.48 ns |     662.50 ns |     0.66 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,211,364.8 ns |    406,983.59 ns |  22,308.14 ns |    14.05 |    0.23 | 166.0156 | 166.0156 | 166.0156 | 1324850 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,944,398.0 ns |     53,596.49 ns |   2,937.80 ns |    22.54 |    0.05 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,133,363.9 ns |     19,572.09 ns |   1,072.81 ns |    24.74 |    0.04 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 288,143,160.7 ns | 12,498,236.42 ns | 685,070.51 ns | 3,340.85 |    8.84 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,140,558.9 ns |      5,375.50 ns |     294.65 ns |    24.82 |    0.04 |        - |        - |        - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionIncumbentBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| Lodestar_StudentCdf             |   320.08 ns |  40.614 ns |  2.226 ns |  1.00 |    0.01 |      - |         - |          NA |
| MathNet_StudentCdf              |   299.78 ns |   1.677 ns |  0.092 ns |  0.94 |    0.01 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   343.13 ns |  74.974 ns |  4.110 ns |  1.07 |    0.01 |      - |         - |          NA |
| Lodestar_StudentQuantile        |   686.42 ns |  19.631 ns |  1.076 ns |  2.14 |    0.01 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,635.27 ns | 178.818 ns |  9.802 ns | 17.61 |    0.11 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,191.08 ns | 120.784 ns |  6.621 ns |  3.72 |    0.03 | 0.0172 |     288 B |          NA |
| Lodestar_FisherCdf              |   109.09 ns |   3.659 ns |  0.201 ns |  0.34 |    0.00 |      - |         - |          NA |
| MathNet_FisherCdf               |   132.93 ns |  14.387 ns |  0.789 ns |  0.42 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   140.41 ns |  17.983 ns |  0.986 ns |  0.44 |    0.00 |      - |         - |          NA |
| Lodestar_FisherQuantile         |   201.00 ns |  23.571 ns |  1.292 ns |  0.63 |    0.01 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,676.19 ns | 253.633 ns | 13.902 ns |  8.36 |    0.06 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   319.97 ns |  27.005 ns |  1.480 ns |  1.00 |    0.01 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    72.33 ns |   0.565 ns |  0.031 ns |  0.23 |    0.00 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    61.39 ns |   5.436 ns |  0.298 ns |  0.19 |    0.00 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    73.11 ns |   0.886 ns |  0.049 ns |  0.23 |    0.00 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   340.52 ns |  13.087 ns |  0.717 ns |  1.06 |    0.01 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   477.50 ns |  34.342 ns |  1.882 ns |  1.49 |    0.01 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   391.35 ns |   4.401 ns |  0.241 ns |  1.22 |    0.01 |      - |         - |          NA |
| Lodestar_NormalCdf              |    21.50 ns |   0.593 ns |  0.032 ns |  0.07 |    0.00 |      - |         - |          NA |
| MathNet_NormalCdf               |    22.28 ns |   4.280 ns |  0.235 ns |  0.07 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    65.25 ns |   7.230 ns |  0.396 ns |  0.20 |    0.00 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    28.32 ns |   0.240 ns |  0.013 ns |  0.09 |    0.00 |      - |         - |          NA |
| MathNet_NormalQuantile          |    30.63 ns |   3.235 ns |  0.177 ns |  0.10 |    0.00 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   180.09 ns |   8.384 ns |  0.460 ns |  0.56 |    0.00 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| ChiSquaredSfOneDf          |  29.23 ns |  1.011 ns | 0.055 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  12.67 ns |  0.508 ns | 0.028 ns |  0.43 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  42.11 ns |  0.291 ns | 0.016 ns |  1.44 |    0.00 |         - |          NA |
| ChiSquaredSfHundredDf      |  72.50 ns |  3.325 ns | 0.182 ns |  2.48 |    0.01 |         - |          NA |
| ChiSquaredSfFractionalDf   | 119.41 ns | 16.612 ns | 0.911 ns |  4.09 |    0.03 |         - |          NA |
| NormalQuantile             |  28.43 ns |  0.925 ns | 0.051 ns |  0.97 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Lodestar_Glm** | **200**        | **1**          |  **35.87 μs** |  **4.754 μs** | **0.261 μs** |  **1.00** |    **0.01** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  49.17 μs | 14.671 μs | 0.804 μs |  1.37 |    0.02 |  7.2021 |  0.3052 |  117.67 KB |        9.70 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **42.67 μs** |  **2.505 μs** | **0.137 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 102.45 μs | 16.861 μs | 0.924 μs |  2.40 |    0.02 | 10.2539 |  0.6104 |  168.44 KB |        8.69 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **349.48 μs** | **20.920 μs** | **1.147 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 384.18 μs | 65.980 μs | 3.617 μs |  1.10 |    0.01 | 53.7109 | 16.1133 |  881.77 KB |        7.98 |
|              |            |            |           |           |          |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **411.29 μs** | **17.133 μs** | **0.939 μs** |  **1.00** |    **0.00** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 724.45 μs | 24.580 μs | 1.347 μs |  1.76 |    0.00 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method | SampleSize | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|----------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **282.9 μs** |  **45.46 μs** |  **2.49 μs** |   **3.4180** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **3,137.5 μs** | **124.09 μs** |  **6.80 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **31,965.9 μs** | **880.69 μs** | **48.27 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method              | SampleSize | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **30.31 μs** |     **1.579 μs** |   **0.087 μs** |  **1.00** |    **0.00** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    61.02 μs |    16.182 μs |   0.887 μs |  2.01 |    0.03 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |            |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,215.99 μs** | **2,361.837 μs** | **129.460 μs** |  **1.00** |    **0.04** | **328.1250** | **328.1250** | **328.1250** | **1097.75 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,717.16 μs |   191.252 μs |  10.483 μs |  1.83 |    0.05 | 531.2500 | 531.2500 | 531.2500 |  1878.2 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Fit**    | **5**         | **298.2 μs** | **21.47 μs** | **1.18 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **310.3 μs** |  **4.50 μs** | **0.25 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **310.7 μs** | **14.85 μs** | **0.81 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Lodestar_Gls** | **50**         |      **28.56 μs** |      **3.254 μs** |     **0.178 μs** |  **1.00** |    **0.01** |   **1.6479** |   **0.0916** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     105.15 μs |     15.107 μs |     0.828 μs |  3.68 |    0.03 |   1.9531 |   0.1221 |        - |   32.99 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **666.43 μs** |     **76.042 μs** |     **4.168 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,976.02 μs |    351.432 μs |    19.263 μs |  2.97 |    0.03 |  97.6563 |  97.6563 |  97.6563 |  370.11 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **6,872.01 μs** |    **259.964 μs** |    **14.250 μs** |  **1.00** |    **0.00** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  22,022.20 μs |  4,431.903 μs |   242.928 μs |  3.20 |    0.03 | 281.2500 | 281.2500 | 281.2500 | 2226.99 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **51,762.29 μs** |  **1,262.786 μs** |    **69.218 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.17 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 141,574.98 μs | 98,415.481 μs | 5,394.485 μs |  2.74 |    0.09 |        - |        - |        - | 8849.77 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method    | SampleSize | Lags | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |     **6.795 μs** |   **0.0524 μs** |  **0.0029 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |     9.447 μs |   0.3641 μs |  0.0200 μs |  1.39 |    0.00 |   0.5341 |        - |        - |   8.75 KB |        3.30 |
| Hac       | 100        | 4    |    19.153 μs |   0.3194 μs |  0.0175 μs |  2.82 |    0.00 |   0.7629 |        - |        - |   12.9 KB |        4.87 |
| Cluster   | 100        | 4    |     8.452 μs |   0.5540 μs |  0.0304 μs |  1.24 |    0.00 |   0.5798 |        - |        - |   9.71 KB |        3.67 |
|           |            |      |              |             |            |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **434.629 μs** |  **10.5516 μs** |  **0.5784 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   895.037 μs | 118.2049 μs |  6.4792 μs |  2.06 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 472.89 KB |        5.91 |
| Hac       | 10000      | 4    | 1,978.002 μs |  51.6940 μs |  2.8335 μs |  4.55 |    0.01 | 246.0938 | 246.0938 | 246.0938 | 863.84 KB |       10.80 |
| Cluster   | 10000      | 4    |   799.554 μs | 225.3647 μs | 12.3530 μs |  1.84 |    0.02 | 124.0234 | 124.0234 | 124.0234 | 565.18 KB |        7.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method     | SampleSize | Mean         | Error      | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|-----------:|----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **7.132 μs** |  **0.2130 μs** | **0.0117 μs** |  **0.2823** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |    **86.217 μs** | **21.6615 μs** | **1.1873 μs** |  **1.4648** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,139.462 μs** | **63.7931 μs** | **3.4967 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **31.93 μs** |  **24.050 μs** |  **1.318 μs** |  **1.00** |    **0.05** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    35.25 μs |   2.283 μs |  0.125 μs |  1.11 |    0.04 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,002.65 μs** |  **47.256 μs** |  **2.590 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,467.98 μs | 373.339 μs | 20.464 μs |  0.73 |    0.01 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method        | Shape    | Mean        | Error    | StdDev   | Gen0   | Allocated |
|-------------- |--------- |------------:|---------:|---------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,390.82 μs** | **87.20 μs** | **4.780 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **47.60 μs** | **12.44 μs** | **0.682 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Estimate** | **4**          |     **143.6 μs** |      **35.65 μs** |     **1.95 μs** |  **1.00** |    **0.02** |  **49.8047** |  **49.8047** |  **49.8047** |  **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     174.4 μs |      26.51 μs |     1.45 μs |  1.21 |    0.02 |   1.9531 |        - |        - |   33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     631.0 μs |     105.50 μs |     5.78 μs |  4.39 |    0.06 | 190.4297 | 190.4297 | 190.4297 |  705.98 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |            |             |
| **Lodestar_Estimate** | **250**        |  **61,344.4 μs** |  **45,081.67 μs** | **2,471.08 μs** |  **1.00** |    **0.05** | **375.0000** | **375.0000** | **375.0000** | **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 298,260.2 μs |   7,159.51 μs |   392.44 μs |  4.87 |    0.17 |        - |        - |        - |  3499.3 KB |        0.39 |
| MathNet_Qr        | 250        | 483,009.4 μs | 102,461.31 μs | 5,616.25 μs |  7.88 |    0.28 |        - |        - |        - | 32662.2 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Exact**  | **141x141** | **567.8 ms** | **142.11 ms** | **7.79 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **441.4 ms** |  **31.79 ms** | **1.74 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar_StudentT**              | **100**        |     **1,033.3 ns** |      **27.59 ns** |      **1.51 ns** |   **1.00** |    **0.00** | **0.0038** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     2,221.3 ns |     545.24 ns |     29.89 ns |   2.15 |    0.03 | 0.0038 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     2,261.0 ns |     128.39 ns |      7.04 ns |   2.19 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     3,906.7 ns |     224.94 ns |     12.33 ns |   3.78 |    0.01 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     7,330.4 ns |     436.39 ns |     23.92 ns |   7.09 |    0.02 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     9,698.2 ns |   3,849.77 ns |    211.02 ns |   9.39 |    0.18 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     2,519.6 ns |     101.83 ns |      5.58 ns |   2.44 |    0.01 | 0.0954 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,028.4 ns |     154.73 ns |      8.48 ns |   3.90 |    0.01 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |       966.8 ns |     155.27 ns |      8.51 ns |   0.94 |    0.01 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     3,162.4 ns |      17.97 ns |      0.98 ns |   3.06 |    0.00 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |     1,067.7 ns |      77.74 ns |      4.26 ns |   1.03 |    0.00 | 0.0477 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |     1,030.3 ns |      36.47 ns |      2.00 ns |   1.00 |    0.00 | 0.0572 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       461.2 ns |      39.62 ns |      2.17 ns |   0.45 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |     1,563.6 ns |      98.05 ns |      5.37 ns |   1.51 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |       117.8 ns |      20.32 ns |      1.11 ns |   0.11 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       692.1 ns |     130.36 ns |      7.15 ns |   0.67 |    0.01 | 0.0572 |     968 B |       15.12 |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **60,328.8 ns** |     **568.42 ns** |     **31.16 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   178,846.2 ns |   2,340.85 ns |    128.31 ns |  2.965 |    0.00 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   687,993.5 ns |  14,975.72 ns |    820.87 ns | 11.404 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 2,078,287.3 ns | 104,967.24 ns |  5,753.61 ns | 34.449 |    0.08 | 3.9063 |   80382 B |    1,255.97 |
| Lodestar_KruskalWallis         | 10000      | 1,468,022.5 ns |  50,106.04 ns |  2,746.48 ns | 24.334 |    0.04 |      - |      35 B |        0.55 |
| MetaNumerics_KruskalWallis     | 10000      | 3,644,554.0 ns | 557,930.43 ns | 30,582.05 ns | 60.411 |    0.44 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,471,655.4 ns | 399,533.35 ns | 21,899.77 ns | 24.394 |    0.31 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,141,357.6 ns |  82,690.91 ns |  4,532.57 ns | 35.495 |    0.07 | 3.9063 |   80374 B |    1,255.84 |
| Lodestar_OneWayAnova           | 10000      |    85,628.7 ns |  32,744.54 ns |  1,794.84 ns |  1.419 |    0.03 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   269,799.8 ns |  36,571.12 ns |  2,004.59 ns |  4.472 |    0.03 |      - |     745 B |       11.64 |
| Lodestar_Wilcoxon              | 10000      |   186,929.7 ns |   8,070.56 ns |    442.37 ns |  3.099 |    0.01 | 4.6387 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   379,505.7 ns |  12,085.25 ns |    662.43 ns |  6.291 |    0.01 | 4.3945 |   80177 B |    1,252.77 |
| Lodestar_FisherExact           | 10000      |       459.4 ns |      23.85 ns |      1.31 ns |  0.008 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |     1,546.1 ns |     210.45 ns |     11.54 ns |  0.026 |    0.00 | 0.0553 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |       116.7 ns |       9.12 ns |      0.50 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       684.5 ns |      52.96 ns |      2.90 ns |  0.011 |    0.00 | 0.0572 |     968 B |       15.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Lodestar** | **200**        |   **137.3 μs** |  **15.92 μs** |  **0.87 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   890.0 μs |  46.41 μs |  2.54 μs |  6.48 |    0.04 |  88.8672 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |           |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,302.7 μs** | **150.03 μs** |  **8.22 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 7,034.9 μs | 272.46 μs | 14.93 μs |  5.40 |    0.03 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Lodestar_Ols** | **100**        |     **6.785 μs** |   **0.1734 μs** |  **0.0095 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   265.577 μs |  20.8152 μs |  1.1410 μs | 39.14 |    0.15 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    31.246 μs |   4.0789 μs |  0.2236 μs |  4.61 |    0.03 |   2.1362 |        - |        - |   35.47 KB |       13.39 |
|              |            |              |             |            |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **437.846 μs** |  **27.3102 μs** |  **1.4970 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,875.939 μs | 369.6963 μs | 20.2643 μs | 11.14 |    0.05 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,688.512 μs | 268.1955 μs | 14.7007 μs |  3.86 |    0.03 | 441.4063 | 441.4063 | 441.4063 | 1766.06 KB |       22.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method                       | Mean        | Error       | StdDev    | Ratio     | RatioSD  | Allocated | Alloc Ratio |
|----------------------------- |------------:|------------:|----------:|----------:|---------:|----------:|------------:|
| NormalQuantile               |   0.0430 ns |   0.0952 ns | 0.0052 ns |      1.01 |     0.15 |         - |          NA |
| NormalQuantileFarTail        |   0.0377 ns |   0.0453 ns | 0.0025 ns |      0.88 |     0.10 |         - |          NA |
| StudentQuantile              | 560.4466 ns | 118.5525 ns | 6.4983 ns | 13,150.92 | 1,298.99 |         - |          NA |
| StudentQuantileCauchyFarTail | 168.2090 ns |   6.9630 ns | 0.3817 ns |  3,947.04 |   387.91 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,502.7 μs** |    **14.91 μs** |   **0.82 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    423.4 μs |    34.49 μs |   1.89 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    603.2 μs |    63.56 μs |   3.48 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |  1,272.8 μs |   113.08 μs |   6.20 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    698.1 μs |    63.88 μs |   3.50 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **634.3 μs** |    **60.40 μs** |   **3.31 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    236.3 μs |    50.88 μs |   2.79 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    521.9 μs |    39.55 μs |   2.17 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    779.0 μs |    28.02 μs |   1.54 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    388.3 μs |    19.72 μs |   1.08 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **15,019.5 μs** |   **173.64 μs** |   **9.52 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,527.5 μs |   184.82 μs |  10.13 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  4,606.4 μs |   714.84 μs |  39.18 μs | 625.0000 | 625.0000 | 625.0000 |  5062764 B |
| WilcoxonPooledDifferences | 100000     | False | 16,956.1 μs |   916.31 μs |  50.23 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  7,187.4 μs |   224.62 μs |  12.31 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,061.9 μs** |    **71.76 μs** |   **3.93 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,824.1 μs |   989.98 μs |  54.26 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,318.6 μs |   805.84 μs |  44.17 μs | 523.4375 | 523.4375 | 523.4375 |  5062700 B |
| WilcoxonPooledDifferences | 100000     | True  | 13,790.1 μs | 4,596.23 μs | 251.94 μs | 531.2500 | 531.2500 | 531.2500 | 19281691 B |
| MannWhitneyTest           | 100000     | True  |  3,763.7 μs |    59.82 μs |   3.28 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **Fit**    | **100**        | **Nonrobust**  |     **6.717 μs** |   **0.4319 μs** |  **0.0237 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |     **9.779 μs** |   **5.8395 μs** |  **0.3201 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc1**        |     **9.471 μs** |   **0.8944 μs** |  **0.0490 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.257 μs** |   **0.3786 μs** |  **0.0208 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.276 μs** |   **1.0017 μs** |  **0.0549 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **423.920 μs** |   **2.6348 μs** |  **0.1444 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **937.660 μs** | **259.3509 μs** | **14.2159 μs** | **124.0234** | **124.0234** | **124.0234** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **911.257 μs** | **248.1243 μs** | **13.6005 μs** | **124.0234** | **124.0234** | **124.0234** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,048.558 μs** |  **73.9392 μs** |  **4.0529 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,038.033 μs** |  **97.8271 μs** |  **5.3622 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **LodestarAutocorrelation**        | **200**        |   **4.438 μs** | **0.5778 μs** | **0.0317 μs** |  **1.00** |    **0.01** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.544 μs | 0.4200 μs | 0.0230 μs |  2.60 |    0.02 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.871 μs | 0.5979 μs | 0.0328 μs |  1.10 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.167 μs | 0.4903 μs | 0.0269 μs |  2.74 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.968 μs | 0.7217 μs | 0.0396 μs |  1.12 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.666 μs | 0.3789 μs | 0.0208 μs |  2.63 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **49.045 μs** | **0.3272 μs** | **0.0179 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 121.008 μs | 3.5673 μs | 0.1955 μs |  2.47 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  49.734 μs | 2.3884 μs | 0.1309 μs |  1.01 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.841 μs | 5.9071 μs | 0.3238 μs |  2.48 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  49.682 μs | 2.3761 μs | 0.1302 μs |  1.01 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 120.993 μs | 1.6612 μs | 0.0911 μs |  2.47 |    0.00 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **LodestarAugmentedDickeyFuller** | **200**        |   **7.722 μs** |  **2.3552 μs** | **0.1291 μs** |  **1.00** |    **0.02** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  31.087 μs |  3.0355 μs | 0.1664 μs |  4.03 |    0.06 |   4.0283 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  25.402 μs |  1.6169 μs | 0.0886 μs |  3.29 |    0.05 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   3.894 μs |  0.2099 μs | 0.0115 μs |  0.50 |    0.01 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   4.085 μs |  0.9509 μs | 0.0521 μs |  0.53 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.628 μs |  0.1423 μs | 0.0078 μs |  0.21 |    0.00 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.966 μs |  0.3166 μs | 0.0174 μs |  0.38 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       | **119.390 μs** | **13.4744 μs** | **0.7386 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 988.287 μs | 24.2770 μs | 1.3307 μs |  8.28 |    0.05 | 248.0469 | 248.0469 | 248.0469 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 291.256 μs | 37.4951 μs | 2.0552 μs |  2.44 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  95.898 μs |  3.6432 μs | 0.1997 μs |  0.80 |    0.00 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  97.739 μs |  2.1856 μs | 0.1198 μs |  0.82 |    0.00 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  14.512 μs |  4.3635 μs | 0.2392 μs |  0.12 |    0.00 |   3.8300 |   0.4120 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  27.993 μs |  4.2970 μs | 0.2355 μs |  0.23 |    0.00 |   4.7913 |   0.4578 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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
| **LodestarWelchT**      | **100**        |      **1,154.4 ns** |     **284.06 ns** |     **15.57 ns** |    **1.00** |    **0.02** |   **0.0038** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     38,633.1 ns |   4,010.96 ns |    219.85 ns |   33.47 |    0.42 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,142.5 ns |       7.15 ns |      0.39 ns |    1.86 |    0.02 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,591.1 ns |   8,099.26 ns |    443.95 ns |   20.44 |    0.41 |   1.3733 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        113.6 ns |      16.16 ns |      0.89 ns |    0.10 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        215.5 ns |      25.59 ns |      1.40 ns |    0.19 |    0.00 |   0.0100 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **52,314.7 ns** |  **16,264.32 ns** |    **891.50 ns** |   **1.000** |    **0.02** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    140,998.9 ns |   7,762.66 ns |    425.50 ns |   2.696 |    0.04 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    692,253.3 ns |  27,748.95 ns |  1,521.01 ns |  13.235 |    0.20 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 12,499,734.8 ns | 337,236.43 ns | 18,485.07 ns | 238.979 |    3.51 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        112.5 ns |      30.52 ns |      1.67 ns |   0.002 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        214.1 ns |      31.09 ns |      1.70 ns |   0.004 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                   | GroupSize | Mean           | Error         | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|--------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,128.9 ns** |     **119.71 ns** |     **6.56 ns** |  **1.00** |    **0.00** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,186.3 ns |     424.73 ns |    23.28 ns |  1.97 |    0.01 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       602.6 ns |      39.98 ns |     2.19 ns |  0.28 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,161.1 ns |      22.08 ns |     1.21 ns |  0.55 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       759.0 ns |      74.15 ns |     4.06 ns |  0.36 |    0.00 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,705.0 ns |     969.78 ns |    53.16 ns |  2.21 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,594.8 ns |     375.86 ns |    20.60 ns |  4.04 |    0.01 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,355.4 ns |     537.47 ns |    29.46 ns |  2.99 |    0.01 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,087.5 ns |     226.61 ns |    12.42 ns |  1.92 |    0.01 |       - |      - |      48 B |       0.009 |
|                          |           |                |               |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **383,900.3 ns** |  **29,159.37 ns** | **1,598.32 ns** | **1.000** |    **0.01** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,349,935.8 ns | 123,685.81 ns | 6,779.64 ns | 3.516 |    0.02 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,844.7 ns |  16,288.03 ns |   892.80 ns | 0.148 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,640.3 ns |   3,224.74 ns |   176.76 ns | 0.293 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       991.9 ns |     120.93 ns |     6.63 ns | 0.003 |    0.00 |  0.0019 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   919,043.8 ns |  71,419.39 ns | 3,914.74 ns | 2.394 |    0.01 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,395,357.1 ns |  32,994.15 ns | 1,808.52 ns | 3.635 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |            NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    10,712.7 ns |     775.95 ns |    42.53 ns | 0.028 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **15.65 μs** |   **4.416 μs** |  **0.242 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **36.12 μs** |   **8.542 μs** |  **0.468 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **51.72 μs** |   **7.568 μs** |  **0.415 μs** |   **6.6528** |   **1.6479** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **157.34 μs** |  **24.801 μs** |  **1.359 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **214.30 μs** |  **62.207 μs** |  **3.410 μs** |  **73.9746** |  **73.9746** |  **73.9746** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **566.00 μs** |  **45.111 μs** |  **2.473 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **745.54 μs** |  **56.458 μs** |  **3.095 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,271.48 μs** | **371.251 μs** | **20.350 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method       | SampleSize | Mean         | Error         | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **11.59 μs** |      **0.176 μs** |   **0.010 μs** |  **1.00** |    **0.00** |    **0.1984** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     18.64 μs |      4.850 μs |   0.266 μs |  1.61 |    0.02 |    3.6926 |    0.1831 |         - |    58.22 KB |       16.98 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **93.81 μs** |     **15.583 μs** |   **0.854 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    116.21 μs |      5.877 μs |   0.322 μs |  1.24 |    0.01 |   33.0811 |   14.0381 |         - |   508.29 KB |       29.06 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **888.92 μs** |     **37.075 μs** |   **2.032 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |    **158.5 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,427.13 μs |  1,425.559 μs |  78.140 μs |  1.61 |    0.08 | 1035.1563 | 1033.2031 | 1033.2031 |  5026.63 KB |       31.71 |
|              |            |              |               |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,459.01 μs** |  **1,018.407 μs** |  **55.822 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.71 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 19,738.89 μs | 13,858.898 μs | 759.653 μs |  2.09 |    0.07 |  687.5000 |  687.5000 |  687.5000 | 50022.58 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **2,011.0 μs** |     **718.75 μs** |     **39.40 μs** |   **1.00** |    **0.02** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   101,740.6 μs |   6,068.70 μs |    332.65 μs |  50.61 |    0.87 |  3666.6667 |  1500.0000 |  833.3333 |  59880.82 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,888.0 μs** |      **66.25 μs** |      **3.63 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    93,061.3 μs |  24,151.90 μs |  1,323.85 μs |  49.29 |    0.61 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **773.0 μs** |      **49.62 μs** |      **2.72 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   138,407.8 μs |  95,687.14 μs |  5,244.93 μs | 179.06 |    5.90 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,272.3 μs** |      **94.66 μs** |      **5.19 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    70,493.5 μs |   8,232.07 μs |    451.23 μs |  31.02 |    0.18 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **15,578.2 μs** |   **2,446.76 μs** |    **134.12 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,621,925.6 μs | 736,052.94 μs | 40,345.55 μs | 104.12 |    2.37 | 35000.0000 | 10000.0000 | 4000.0000 | 537149.55 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **14,917.1 μs** |  **15,955.43 μs** |    **874.57 μs** |   **1.00** |    **0.07** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,724,902.8 μs | 321,922.83 μs | 17,645.68 μs | 115.89 |    5.81 | 48000.0000 | 12000.0000 | 4000.0000 | 747353.12 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,635.5 μs** |     **301.32 μs** |     **16.52 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,095,108.5 μs | 340,386.45 μs | 18,657.73 μs | 466.45 |    2.63 | 47000.0000 | 12000.0000 | 4000.0000 | 732672.84 KB |    8,147.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **17,544.8 μs** |   **1,161.32 μs** |     **63.66 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,620,159.5 μs | 253,275.29 μs | 13,882.87 μs |  92.34 |    0.74 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 26.12 ms |  3.781 ms | 0.207 ms |  718.7500 |  656.2500 | 312.5000 |  21.53 MB |
| CountLoadAsync4M | 47.92 ms | 17.652 ms | 0.968 ms | 1454.5455 | 1363.6364 | 636.3636 |  43.93 MB |
| CountLoad4M      | 45.95 ms |  7.807 ms | 0.428 ms | 1363.6364 | 1272.7273 | 545.4545 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| CountFitLongDocuments | 174,805.25 μs |  5,359.642 μs |   293.780 μs |         - |         - |        - | 54334.51 KB |
| CountSave240K         |     397.18 μs |     53.964 μs |     2.958 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     449.87 μs |     45.718 μs |     2.506 μs |   10.7422 |    4.3945 |   4.3945 |   739.44 KB |
| TfidfSave45K          |      52.88 μs |      7.345 μs |     0.403 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      56.20 μs |      4.054 μs |     0.222 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     290.03 μs |     56.922 μs |     3.120 μs |    7.3242 |    0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     291.24 μs |     24.475 μs |     1.342 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  27,368.98 μs | 20,248.214 μs | 1,109.873 μs |  593.7500 |  562.5000 | 187.5000 | 22049.03 KB |
| CountLoadAsync4M      |  55,339.91 μs | 40,707.576 μs | 2,231.320 μs | 1100.0000 | 1000.0000 | 300.0000 | 44983.52 KB |
| CountLoad4M           |  51,537.37 μs | 30,499.748 μs | 1,671.794 μs | 1100.0000 | 1000.0000 | 300.0000 |  44976.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| TfidfSaveAsyncSmall    |     17.036 μs |     22.938 μs |   1.2573 μs |    1.6785 |   0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    500.665 μs |     76.273 μs |   4.1808 μs |   34.1797 |  28.3203 |  28.3203 |   740.15 KB |
| HashingLoadUndeclared  |      5.318 μs |      2.978 μs |   0.1632 μs |    5.0964 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     28.745 μs |      5.849 μs |   0.3206 μs |    7.4158 |   0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |    129.107 μs |      7.726 μs |   0.4235 μs |   16.1133 |   3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    846.189 μs |    349.690 μs |  19.1677 μs |   38.0859 |  37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  | 11,769.911 μs | 11,757.628 μs | 644.4752 μs |  515.6250 | 500.0000 | 296.8750 | 10793.89 KB |
| CountSaveAsync1M       |  2,011.254 μs |     52.106 μs |   2.8561 μs |  132.8125 | 125.0000 | 125.0000 |  3162.17 KB |
| IndexSaveAsyncLongHead |  3,403.559 μs |    680.746 μs |  37.3140 μs |   93.7500 |  89.8438 |  89.8438 |  2124.22 KB |
| CountLoadUndeclared2M  | 19,325.516 μs |  1,433.826 μs |  78.5928 μs | 1000.0000 | 968.7500 | 593.7500 | 22045.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.890 μs** |  **0.4964 μs** | **0.0272 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.128 μs |  0.8539 μs | 0.0468 μs |  1.04 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   6.061 μs |  0.2562 μs | 0.0140 μs |  1.03 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **56.834 μs** |  **0.8676 μs** | **0.0476 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  22.323 μs |  3.0951 μs | 0.1697 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.966 μs |  0.7815 μs | 0.0428 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **227.625 μs** | **26.6875 μs** | **1.4628 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  83.005 μs |  7.2367 μs | 0.3967 μs |  0.36 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  72.264 μs |  7.5433 μs | 0.4135 μs |  0.32 |  4.1504 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **889.729 μs** | **25.4257 μs** | **1.3937 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 332.142 μs | 30.6522 μs | 1.6802 μs |  0.37 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 276.084 μs | 17.0914 μs | 0.9368 μs |  0.31 | 15.6250 | 1.9531 | 262.16 KB |        0.59 |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **133.69 ms** |  **20.511 ms** | **1.124 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  65.85 ms |  10.435 ms | 0.572 ms |  0.49 |    0.01 |  103.68 KB |        3.80 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **140.05 ms** |  **58.689 ms** | **3.217 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  62.47 ms |   5.521 ms | 0.303 ms |  0.45 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **201.73 ms** |   **3.018 ms** | **0.165 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 241.45 ms |  22.289 ms | 1.222 ms |  1.20 |    0.01 |     259 KB |        2.50 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **211.35 ms** | **129.144 ms** | **7.079 ms** |  **1.00** |    **0.04** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 219.26 ms |  15.923 ms | 0.873 ms |  1.04 |    0.03 |   192.8 KB |        3.53 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **256.15 ms** |  **36.259 ms** | **1.987 ms** |  **1.00** |    **0.01** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 349.68 ms | 132.386 ms | 7.257 ms |  1.37 |    0.03 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **257.13 ms** |   **4.585 ms** | **0.251 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 320.41 ms |  72.974 ms | 4.000 ms |  1.25 |    0.01 | 1152.95 KB |        1.55 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **294.65 ms** |  **20.474 ms** | **1.122 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 398.32 ms |  21.178 ms | 1.161 ms |  1.35 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **301.68 ms** |  **23.701 ms** | **1.299 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 400.07 ms | 141.660 ms | 7.765 ms |  1.33 |    0.02 | 7964.22 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method      | Count | Mean     | Error     | StdDev   | Ratio | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |---------:|----------:|---------:|------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    | **75.31 ms** |  **3.340 ms** | **0.183 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** |  **9386.06 KB** |       **1.000** |
| TreeNearest | 10    | 44.22 ms |  0.655 ms | 0.036 ms |  0.59 |         - |         - |         - |    29.67 KB |       0.003 |
|             |       |          |           |          |       |           |           |           |             |             |
| **FullScan**    | **2000**  | **77.08 ms** | **13.287 ms** | **0.728 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** | **10007.93 KB** |        **1.00** |
| TreeNearest | 2000  | 71.96 ms |  3.458 ms | 0.190 ms |  0.93 |         - |         - |         - |  1273.52 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method | length | Mean          | Error        | StdDev    | Allocated |
|------- |------- |--------------:|-------------:|----------:|----------:|
| **Latin**  | **1000**   |      **43.74 μs** |     **3.098 μs** |  **0.170 μs** |         **-** |
| Cjk    | 1000   |      62.79 μs |     5.926 μs |  0.325 μs |         - |
| **Latin**  | **10000**  |   **4,098.69 μs** |    **28.109 μs** |  **1.541 μs** |         **-** |
| Cjk    | 10000  |   6,765.44 μs |   892.134 μs | 48.901 μs |         - |
| **Latin**  | **65536**  | **184,487.71 μs** | **1,425.633 μs** | **78.144 μs** |         **-** |

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| Unigram | 28.07 ms |  2.789 ms | 0.153 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 85.68 ms | 10.632 ms | 0.583 ms |  3.05 |    0.02 | 1666.6667 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **BpeOnOnePathologicalToken** | **512**    |  **18.44 μs** |  **1.769 μs** | **0.097 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **42.42 μs** | **14.061 μs** | **0.771 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **191.16 μs** | **10.952 μs** | **0.600 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **386.13 μs** | **24.227 μs** | **1.328 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 9.156 ms | 1.485 ms | 0.0814 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **DpGroup**    | **cjk**      |  **18.997 μs** |  **5.910 μs** | **0.3240 μs** |         **-** |
| MyersGroup | cjk      | 242.279 μs |  7.347 μs | 0.4027 μs |         - |
| **DpGroup**    | **latin**    |   **9.593 μs** |  **1.067 μs** | **0.0585 μs** |         **-** |
| MyersGroup | latin    | 134.791 μs | 63.040 μs | 3.4554 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **304.16 μs** |  **47.129 μs** | **2.583 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 302.04 μs |  28.991 μs | 1.589 μs |  0.99 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **335.49 μs** |  **13.367 μs** | **0.733 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 326.97 μs |  12.249 μs | 0.671 μs |  0.97 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **319.24 μs** |  **24.703 μs** | **1.354 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 308.67 μs |  66.787 μs | 3.661 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **269.01 μs** |  **49.948 μs** | **2.738 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 319.48 μs |  35.765 μs | 1.960 μs |  1.19 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **306.84 μs** |   **6.293 μs** | **0.345 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 306.83 μs |  43.393 μs | 2.379 μs |  1.00 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **164.52 μs** |  **52.161 μs** | **2.859 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 306.43 μs |  17.168 μs | 0.941 μs |  1.86 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **226.37 μs** | **116.734 μs** | **6.399 μs** |  **1.00** |    **0.03** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 308.58 μs | 159.665 μs | 8.752 μs |  1.36 |    0.05 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **89.17 μs** |   **4.738 μs** | **0.260 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 310.43 μs |  80.784 μs | 4.428 μs |  3.48 |    0.04 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar_Cdist**                | **50**   |   **183.1 μs** |  **61.43 μs** |  **3.37 μs** |  **1.00** |    **0.02** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   178.6 μs |   5.43 μs |  0.30 μs |  0.98 |    0.02 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   473.0 μs |   6.00 μs |  0.33 μs |  2.58 |    0.04 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,269.6 μs** | **193.48 μs** | **10.61 μs** |  **1.00** |    **0.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,002.0 μs | 305.02 μs | 16.72 μs |  0.92 |    0.01 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,613.2 μs | 376.08 μs | 20.61 μs |  2.33 |    0.01 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method | Classes | Mean         | Error       | StdDev    | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|------------:|----------:|-------:|-------:|----------:|
| **Report** | **10**      |     **574.2 ns** |    **84.16 ns** |   **4.61 ns** | **0.1011** |      **-** |   **1.66 KB** |
| **Report** | **1000**    | **213,127.7 ns** | **9,266.44 ns** | **507.92 ns** | **7.3242** | **1.7090** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method          | Samples | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| Roc             | 1000000 | 147.8 ms | 31.20 ms | 1.71 ms |  750.0000 |  750.0000 |  750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 134.6 ms | 28.06 ms | 1.54 ms | 1000.0000 | 1000.0000 | 1000.0000 |  68.66 MB |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar_Fit** | **10000x8x8** |   **152.07 ms** |  **27.841 ms** | **1.526 ms** |  **1.00** |    **0.01** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,317.32 ms | 140.301 ms | 7.690 ms | 15.24 |    0.14 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |            |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **53.86 ms** |   **4.063 ms** | **0.223 ms** |  **1.00** |    **0.01** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   812.91 ms |  15.099 ms | 0.828 ms | 15.09 |    0.06 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **160.53 ms** |  **16.880 ms** |  **0.925 ms** |  **1.00** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 7,070.34 ms | 107.062 ms |  5.868 ms | 44.04 |    0.22 | 38000.0000 | 13000.0000 | 11000.0000 | 531.86 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,912.93 ms | 239.422 ms | 13.124 ms | 24.38 |    0.14 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **18.80 ms** |   **1.482 ms** |  **0.081 ms** |  **1.00** |    **0.01** |  **1312.5000** |  **1312.5000** |  **1312.5000** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   475.03 ms |  22.421 ms |  1.229 ms | 25.27 |    0.11 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   264.46 ms |  20.011 ms |  1.097 ms | 14.07 |    0.07 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                                    | Mean       | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  13.054 ms |  3.8054 ms | 0.2086 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 102.045 ms | 15.9130 ms | 0.8722 ms |  7.82 |    0.12 |
| Nmf_Transform                             |   2.833 ms |  0.2278 ms | 0.0125 ms |  0.22 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  24.434 ms |  1.6904 ms | 0.0927 ms |  1.87 |    0.03 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method                     | Columns | Mean         | Error        | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|-------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **19,942.7 μs** |  **2,909.66 μs** |   **159.49 μs** | **3031.2500** | **3000.0000** | **3000.0000** |  **11692.44 KB** |
| TruncatedSvd_Transform     | 500     |     542.4 μs |     55.69 μs |     3.05 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 188,841.7 μs | 22,260.08 μs | 1,220.15 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18143.46 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 268,737.2 μs | 99,311.51 μs | 5,443.60 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **143,583.2 μs** | **37,082.63 μs** | **2,032.62 μs** | **3750.0000** | **3750.0000** | **3750.0000** | **115064.21 KB** |
| TruncatedSvd_Transform     | 20000   |     976.4 μs |    292.35 μs |    16.02 μs |   39.0625 |   39.0625 |   39.0625 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 722,472.7 μs | 18,983.69 μs | 1,040.56 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.81 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 590,085.5 μs | 38,530.66 μs | 2,112.00 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| Ratio          |    97.30 ns |   9.250 ns | 0.507 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   |   749.65 ns |  51.559 ns | 2.826 ns |  7.70 |    0.04 |      - |         - |          NA |
| TokenSortRatio |   870.82 ns | 128.177 ns | 7.026 ns |  8.95 |    0.07 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 1,016.78 ns |  75.950 ns | 4.163 ns | 10.45 |    0.06 | 0.0534 |     896 B |          NA |
| WRatio         | 1,279.52 ns |  85.666 ns | 4.696 ns | 13.15 |    0.07 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,932,122.7611 ns |    20,673.1460 ns |   1,133.1649 ns |  1.000 |    0.00 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    678,752.6729 ns |     8,882.2039 ns |     486.8636 ns |  0.351 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,123,385.5651 ns |   110,235.8594 ns |   6,042.3995 ns |  0.581 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,382,920.1250 ns | 6,957,406.3629 ns | 381,358.9210 ns | 35.910 |    0.17 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method     | Operation     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **100.2 ns** |   **6.33 ns** |  **0.35 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    208.3 ns |   9.35 ns |  0.51 ns |  2.08 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **769.1 ns** |  **95.06 ns** |  **5.21 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,010.8 ns | 397.85 ns | 21.81 ns | 13.02 |    0.08 |      - |     160 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **987.9 ns** |   **6.88 ns** |  **0.38 ns** |  **1.00** |    **0.00** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,251.9 ns | 754.04 ns | 41.33 ns |  2.28 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,307.0 ns** | **122.54 ns** |  **6.72 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,245.9 ns | 158.52 ns |  8.69 ns |  4.01 |    0.02 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.540 ms** | **0.2573 ms** | **0.0141 ms** | **277.3438** | **267.5781** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **17.923 ms** | **1.8142 ms** | **0.0994 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **26.48 ns** |   **1.263 ns** |  **0.069 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.07 ns |   0.299 ns |  0.016 ns |  1.55 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    27.33 ns |   0.298 ns |  0.016 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    27.08 ns |   0.778 ns |  0.043 ns |  1.02 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **28.54 ns** |   **6.091 ns** |  **0.334 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.41 ns |   0.879 ns |  0.048 ns |  1.45 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    28.48 ns |   0.577 ns |  0.032 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    27.93 ns |   0.287 ns |  0.016 ns |  0.98 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **29.95 ns** |   **1.053 ns** |  **0.058 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.50 ns |   0.478 ns |  0.026 ns |  1.52 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    32.13 ns |   1.157 ns |  0.063 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    30.41 ns |   0.487 ns |  0.027 ns |  1.02 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **34.84 ns** |   **0.957 ns** |  **0.052 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    46.20 ns |   1.797 ns |  0.099 ns |  1.33 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    35.91 ns |  10.822 ns |  0.593 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    33.39 ns |   0.187 ns |  0.010 ns |  0.96 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **57.53 ns** |   **2.807 ns** |  **0.154 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    60.83 ns |   2.701 ns |  0.148 ns |  1.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    57.11 ns |   0.958 ns |  0.053 ns |  0.99 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    61.94 ns |   9.455 ns |  0.518 ns |  1.08 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **62.45 ns** |   **0.724 ns** |  **0.040 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    69.70 ns |   4.506 ns |  0.247 ns |  1.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    66.53 ns |   0.861 ns |  0.047 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    61.59 ns |   0.876 ns |  0.048 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **406.67 ns** |  **16.077 ns** |  **0.881 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   417.34 ns |  14.067 ns |  0.771 ns |  1.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   413.82 ns |   1.058 ns |  0.058 ns |  1.02 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   404.63 ns |   9.782 ns |  0.536 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,678.21 ns** | **201.796 ns** | **11.061 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,766.32 ns | 770.743 ns | 42.247 ns |  1.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,668.84 ns |  39.612 ns |  2.171 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,611.34 ns |  47.402 ns |  2.598 ns |  0.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **388.2 ns** |   **5.40 ns** |  **0.30 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    192.5 ns |   2.85 ns |  0.16 ns |  0.50 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,395.9 ns** |  **26.63 ns** |  **1.46 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  3,094.8 ns |  15.13 ns |  0.83 ns |  1.29 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **13,401.0 ns** | **337.06 ns** | **18.48 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 31,627.8 ns | 626.86 ns | 34.36 ns |  2.36 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **8,654.7 ns** |   **339.07 ns** |  **18.59 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,947.7 ns |   302.49 ns |  16.58 ns |  0.92 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,615.9 ns |   919.30 ns |  50.39 ns |  1.34 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **10,039.1 ns** | **5,052.72 ns** | **276.96 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,325.6 ns |   536.27 ns |  29.39 ns |  0.93 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,556.2 ns |   264.92 ns |  14.52 ns |  1.45 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,420.8 ns** | **3,464.19 ns** | **189.88 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,115.9 ns |   392.69 ns |  21.52 ns |  0.98 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 18,180.1 ns |   912.67 ns |  50.03 ns |  1.46 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,146.7 ns** |   **113.37 ns** |   **6.21 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,118.6 ns | 2,138.02 ns | 117.19 ns |  1.19 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,304.0 ns |   150.23 ns |   8.23 ns |  1.81 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,246.1 ns** | **1,055.08 ns** |  **57.83 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,925.8 ns | 2,694.50 ns | 147.69 ns |  0.96 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 11,417.3 ns |   717.32 ns |  39.32 ns |  1.58 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **9,454.0 ns** | **2,802.66 ns** | **153.62 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,019.3 ns | 5,938.44 ns | 325.51 ns |  0.95 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 15,457.5 ns | 3,580.93 ns | 196.28 ns |  1.64 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **4,598.9 ns** |   **687.64 ns** |  **37.69 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,083.1 ns |   758.36 ns |  41.57 ns |  0.89 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  7,410.4 ns | 2,424.32 ns | 132.89 ns |  1.61 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,503.7 ns** |   **216.05 ns** |  **11.84 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  5,440.9 ns | 1,611.42 ns |  88.33 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  9,859.8 ns |   842.05 ns |  46.16 ns |  1.79 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **7,751.3 ns** |   **834.80 ns** |  **45.76 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  7,342.8 ns |   887.75 ns |  48.66 ns |  0.95 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 13,730.9 ns | 1,933.04 ns | 105.96 ns |  1.77 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,369.7 ns** |   **924.86 ns** |  **50.69 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,134.4 ns |   195.38 ns |  10.71 ns |  0.90 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  5,935.4 ns |   289.95 ns |  15.89 ns |  2.51 |    0.05 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,049.0 ns** |    **19.79 ns** |   **1.08 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,868.1 ns |   157.84 ns |   8.65 ns |  0.71 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  8,645.1 ns |   665.91 ns |  36.50 ns |  2.14 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,456.1 ns** |   **115.38 ns** |   **6.32 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,856.9 ns |   195.29 ns |  10.70 ns |  0.60 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 12,316.5 ns |   625.88 ns |  34.31 ns |  1.91 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,618.2 ns** |   **655.32 ns** |  **35.92 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,417.0 ns |   142.63 ns |   7.82 ns |  0.94 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,313.0 ns |   501.02 ns |  27.46 ns |  2.02 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,232.1 ns** |    **52.59 ns** |   **2.88 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,140.2 ns |   429.65 ns |  23.55 ns |  0.79 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,219.2 ns |   738.51 ns |  40.48 ns |  1.95 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **7,554.3 ns** |   **407.91 ns** |  **22.36 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,312.6 ns | 2,714.59 ns | 148.80 ns |  1.10 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 14,349.2 ns | 1,685.26 ns |  92.37 ns |  1.90 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,314.2 ns** |    **42.01 ns** |   **2.30 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    594.8 ns |    41.93 ns |   2.30 ns |  0.45 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,341.6 ns |   309.77 ns |  16.98 ns |  4.06 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **2,909.8 ns** |    **19.31 ns** |   **1.06 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,420.0 ns |    79.04 ns |   4.33 ns |  0.49 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  7,774.2 ns |   372.54 ns |  20.42 ns |  2.67 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,341.7 ns** |   **361.33 ns** |  **19.81 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,386.2 ns |   283.43 ns |  15.54 ns |  0.45 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,245.2 ns | 8,366.98 ns | 458.62 ns |  2.29 |    0.07 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method           | Shape       | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,409.3 μs** | **1,895.3 μs** | **103.89 μs** |  **1.00** |    **0.02** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  38,763.3 μs | 4,547.6 μs | 249.27 μs |  7.17 |    0.12 |        - |        - |        - |   10.39 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  83,555.3 μs | 1,115.1 μs |  61.12 μs | 15.45 |    0.25 |        - |        - |        - |   41.95 KB |       0.028 |
|                  |             |              |            |           |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **798.4 μs** |   **102.8 μs** |   **5.64 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,770.6 μs |   178.9 μs |   9.81 μs |  5.98 |    0.04 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   3,080.9 μs |   942.3 μs |  51.65 μs |  3.86 |    0.06 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |            |           |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **26,397.7 μs** | **2,055.0 μs** | **112.64 μs** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 442,091.7 μs | 1,183.4 μs |  64.86 μs | 16.75 |    0.06 |        - |        - |        - |   16.29 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 735,659.1 μs | 9,775.2 μs | 535.81 μs | 27.87 |    0.10 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **4.999 ms** | **0.4573 ms** | **0.0251 ms** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  16.919 ms | 8.3355 ms | 0.4569 ms |  3.38 |    0.08 |        - |        - |        - |    13.6 KB |       0.010 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **26.547 ms** | **9.6732 ms** | **0.5302 ms** |  **1.00** |    **0.02** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.035 ms | 5.1809 ms | 0.2840 ms |  4.26 |    0.07 |        - |        - |        - |   96.48 KB |        0.27 |
|                |             |            |           |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **46.026 ms** | **2.1590 ms** | **0.1183 ms** |  **1.00** |    **0.00** | **181.8182** | **181.8182** | **181.8182** | **3946.72 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 206.851 ms | 3.6019 ms | 0.1974 ms |  4.49 |    0.01 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.41 ns** |      **2.317 ns** |   **0.127 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.70 ns |      1.541 ns |   0.084 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.09 ns |      1.470 ns |   0.081 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     96.21 ns |      3.093 ns |   0.170 ns |  0.73 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **222.97 ns** |    **176.211 ns** |   **9.659 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 12   |     62.17 ns |      0.766 ns |   0.042 ns |  0.28 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    226.22 ns |    113.286 ns |   6.210 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 12   |    112.14 ns |      2.168 ns |   0.119 ns |  0.50 |    0.02 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **277.46 ns** |    **165.390 ns** |   **9.066 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 14   |     66.26 ns |      0.431 ns |   0.024 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    278.29 ns |    244.303 ns |  13.391 ns |  1.00 |    0.05 |         - |          NA |
| Kernel_Cjk | 14   |    114.01 ns |     28.404 ns |   1.557 ns |  0.41 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **363.12 ns** |    **145.816 ns** |   **7.993 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 16   |     71.26 ns |      5.441 ns |   0.298 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    361.65 ns |    215.354 ns |  11.804 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    123.35 ns |     76.148 ns |   4.174 ns |  0.34 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **436.86 ns** |     **40.728 ns** |   **2.232 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.72 ns |      0.471 ns |   0.026 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    435.36 ns |     21.043 ns |   1.153 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    128.89 ns |     63.482 ns |   3.480 ns |  0.30 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **773.83 ns** |     **14.005 ns** |   **0.768 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     78.10 ns |      0.946 ns |   0.052 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    774.92 ns |     22.759 ns |   1.247 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    127.76 ns |      0.761 ns |   0.042 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |  **1,002.38 ns** |     **21.361 ns** |   **1.171 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |     86.46 ns |      4.232 ns |   0.232 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    997.38 ns |     68.851 ns |   3.774 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    141.34 ns |      6.206 ns |   0.340 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,545.86 ns** |    **510.540 ns** |  **27.984 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 32   |    105.97 ns |      5.710 ns |   0.313 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,522.33 ns |     85.284 ns |   4.675 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |    169.49 ns |     46.152 ns |   2.530 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,286.68 ns** |  **4,476.907 ns** | **245.394 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Kernel     | 48   |    132.86 ns |      0.752 ns |   0.041 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,626.32 ns |    906.060 ns |  49.664 ns |  1.11 |    0.07 |         - |          NA |
| Kernel_Cjk | 48   |    241.69 ns |      1.718 ns |   0.094 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,910.43 ns** |    **364.086 ns** |  **19.957 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    165.32 ns |      0.704 ns |   0.039 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,497.01 ns |     60.403 ns |   3.311 ns |  0.93 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |    291.30 ns |      2.120 ns |   0.116 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **11,979.62 ns** |    **657.405 ns** |  **36.035 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    351.80 ns |      8.316 ns |   0.456 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,039.09 ns | 14,054.602 ns | 770.380 ns |  1.00 |    0.06 |         - |          NA |
| Kernel_Cjk | 96   |    954.28 ns |     18.653 ns |   1.022 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                     | Length | Mean         | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **24.85 ns** |   **6.029 ns** |  **0.330 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     24.64 ns |   0.652 ns |  0.036 ns |  0.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |    119.65 ns |   2.718 ns |  0.149 ns |  4.82 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.25 ns |   1.746 ns |  0.096 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **284.20 ns** |  **28.321 ns** |  **1.552 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    369.73 ns | 104.799 ns |  5.744 ns |  1.30 |    0.02 |         - |          NA |
| Distance_CodePoint         | 64     |    668.61 ns |  11.732 ns |  0.643 ns |  2.35 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    278.36 ns |  42.118 ns |  2.309 ns |  0.98 |    0.01 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **900.73 ns** |   **5.791 ns** |  **0.317 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,894.79 ns |  51.690 ns |  2.833 ns |  2.10 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,664.94 ns | 488.963 ns | 26.802 ns |  1.85 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    920.43 ns |   7.540 ns |  0.413 ns |  1.02 |    0.00 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **11,078.14 ns** | **681.683 ns** | **37.365 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 16,403.05 ns | 354.430 ns | 19.427 ns |  1.48 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 13,529.13 ns | 866.409 ns | 47.491 ns |  1.22 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 10,987.25 ns | 177.507 ns |  9.730 ns |  0.99 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method             | Length | Distinct | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **337.0 ns** |    **48.03 ns** |   **2.63 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     239.6 ns |     3.28 ns |   0.18 ns |  0.71 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **352.1 ns** |    **12.27 ns** |   **0.67 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     237.9 ns |    23.13 ns |   1.27 ns |  0.68 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **424.5 ns** |    **32.58 ns** |   **1.79 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     318.9 ns |    11.44 ns |   0.63 ns |  0.75 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **422.8 ns** |     **9.48 ns** |   **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     325.4 ns |     1.01 ns |   0.06 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **521.3 ns** |     **9.43 ns** |   **0.52 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     406.6 ns |    23.42 ns |   1.28 ns |  0.78 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **537.7 ns** |    **16.21 ns** |   **0.89 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     416.0 ns |     9.58 ns |   0.53 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **598.6 ns** |    **45.88 ns** |   **2.51 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,359.5 ns |   105.87 ns |   5.80 ns |  2.27 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **601.7 ns** |     **8.67 ns** |   **0.48 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,313.2 ns |    55.18 ns |   3.02 ns |  2.18 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,875.6 ns** |    **19.71 ns** |   **1.08 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,393.8 ns |   160.36 ns |   8.79 ns |  2.88 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,938.9 ns** |    **28.66 ns** |   **1.57 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,548.6 ns | 1,939.28 ns | 106.30 ns |  2.86 |    0.05 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **14,728.1 ns** |   **459.37 ns** |  **25.18 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  57,861.5 ns | 6,330.44 ns | 346.99 ns |  3.93 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **431,775.0 ns** | **3,082.75 ns** | **168.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  58,709.7 ns |   950.96 ns |  52.13 ns |  0.14 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **24.91 ns** |      **6.502 ns** |     **0.356 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      82.81 ns |      6.583 ns |     0.361 ns |  3.32 |    0.04 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      85.85 ns |      2.136 ns |     0.117 ns |  3.45 |    0.04 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     167.95 ns |     57.963 ns |     3.177 ns |  6.74 |    0.14 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.99 ns** |     **17.442 ns** |     **0.956 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,943.36 ns |  3,288.274 ns |   180.241 ns | 21.46 |    0.57 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,438.75 ns |     33.702 ns |     1.847 ns |  5.19 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,552.27 ns |  3,359.154 ns |   184.127 ns | 38.10 |    0.59 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **11,053.47 ns** |     **52.682 ns** |     **2.888 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 433,165.70 ns | 24,476.915 ns | 1,341.662 ns | 39.19 |    0.11 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  38,389.34 ns |  1,339.992 ns |    73.449 ns |  3.47 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 715,146.96 ns | 56,403.084 ns | 3,091.643 ns | 64.70 |    0.24 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **F1Macro**           | **1000**   | **601.7 μs** | **39.19 μs** | **2.15 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 632.3 μs | 66.86 μs | 3.66 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 600.2 μs | 27.63 μs | 1.51 μs |  8.7891 |  1.9531 | 153.99 KB |
| **F1Macro**           | **5000**   | **780.5 μs** | **20.26 μs** | **1.11 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 782.4 μs | 25.71 μs | 1.41 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 937.1 μs | 67.60 μs | 3.71 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **133.14 μs** |     **19.407 μs** |     **1.064 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 100,114.16 μs |  4,203.098 μs |   230.386 μs | 751.97 |    5.39 | 833.3333 | 833.3333 | 833.3333 | 31414.69 KB |   12,805.99 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,790.21 μs** |    **326.306 μs** |    **17.886 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 477,911.70 μs | 50,469.690 μs | 2,766.414 μs | 171.29 |    1.28 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **22.74 μs** |      **6.435 μs** |     **0.353 μs** |   **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     891.97 μs |     71.786 μs |     3.935 μs |  39.24 |    0.54 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,280.5 ns** |       **242.17 ns** |     **13.27 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     7,121.7 ns |       370.73 ns |     20.32 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |       110.2 ns |         7.80 ns |      0.43 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,465.9 ns |       442.21 ns |     24.24 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     7,401.2 ns |       443.00 ns |     24.28 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **4,850.6 ns** |     **1,649.49 ns** |     **90.41 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     7,615.2 ns |       301.03 ns |     16.50 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |       110.0 ns |        29.17 ns |      1.60 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,524.6 ns |     1,633.34 ns |     89.53 ns | 0.1068 |    1880 B |
| Report         | 1000    | 10      |    11,899.9 ns |       472.49 ns |     25.90 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **449,376.9 ns** |    **96,380.64 ns** |  **5,282.95 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   835,049.8 ns |    24,700.43 ns |  1,353.91 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    12,251.0 ns |     1,069.77 ns |     58.64 ns |      - |         - |
| F1Macro        | 100000  | 2       |   477,153.1 ns |     7,737.84 ns |    424.14 ns |      - |     656 B |
| Report         | 100000  | 2       |   465,023.2 ns |    23,898.63 ns |  1,309.96 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **509,081.7 ns** |    **30,207.88 ns** |  **1,655.80 ns** |      **-** |    **1337 B** |
| MatrixWeighted | 100000  | 10      |   929,816.1 ns |    10,342.17 ns |    566.89 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    12,692.6 ns |     1,079.86 ns |     59.19 ns |      - |         - |
| F1Macro        | 100000  | 10      |   493,910.3 ns |    27,930.42 ns |  1,530.96 ns |      - |    1881 B |
| Report         | 100000  | 10      |   502,877.2 ns |    22,139.99 ns |  1,213.57 ns |      - |   12833 B |
| **Matrix**         | **1000000** | **2**       | **4,754,136.8 ns** |    **55,344.68 ns** |  **3,033.63 ns** |      **-** |     **406 B** |
| MatrixWeighted | 1000000 | 2       | 8,703,428.4 ns |   927,513.52 ns | 50,840.15 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   120,831.8 ns |     3,923.41 ns |    215.06 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,764,158.7 ns |    46,303.21 ns |  2,538.04 ns |      - |     662 B |
| Report         | 1000000 | 2       | 4,762,768.8 ns |   173,814.77 ns |  9,527.37 ns |      - |    5510 B |
| **Matrix**         | **1000000** | **10**      | **4,863,213.5 ns** |   **260,599.36 ns** | **14,284.33 ns** |      **-** |    **1342 B** |
| MatrixWeighted | 1000000 | 10      | 9,488,478.1 ns |   506,055.75 ns | 27,738.62 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   120,385.4 ns |     3,309.82 ns |    181.42 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 5,065,115.6 ns |   116,027.65 ns |  6,359.87 ns |      - |    1886 B |
| Report         | 1000000 | 10      | 5,063,904.7 ns | 1,627,746.89 ns | 89,222.30 ns |      - |   12878 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar** | **100000**  | **Bundle**        |  **10,460.64 μs** |  **2,211.157 μs** |   **121.201 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1296 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  35,275.88 μs |  2,721.521 μs |   149.176 μs |     3.37 |    0.04 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |    3,927.18 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.20 μs** |      **3.062 μs** |     **0.168 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,732.86 μs |  3,223.773 μs |   176.706 μs | 3,101.35 |   42.64 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **140,970.95 μs** | **65,200.731 μs** | **3,573.872 μs** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 251,702.72 μs | 29,010.955 μs | 1,590.188 μs |     1.79 |    0.04 |        - |        - |        - | 23228948 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **108.19 μs** |     **16.368 μs** |     **0.897 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 229,029.64 μs | 10,616.000 μs |   581.899 μs | 2,116.97 |   15.86 |        - |        - |        - | 23228008 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method    | Samples | Mean        | Error     | StdDev    | Allocated |
|---------- |-------- |------------:|----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **42.53 ms** |  **18.47 ms** |  **1.012 ms** |         **-** |
| OneVsOne  | 100000  |    95.79 ms |  36.41 ms |  1.996 ms |  803531 B |
| **OneVsRest** | **1000000** |   **653.95 ms** | **250.50 ms** | **13.730 ms** |         **-** |
| OneVsOne  | 1000000 | 1,296.55 ms | 794.70 ms | 43.560 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated  |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|-----------:|
| PerLabel         | 100000 |  6.786 ms |  0.8260 ms | 0.0453 ms |         - |         - |        - |    7.84 KB |
| PerLabelWeighted | 100000 | 11.477 ms |  0.9911 ms | 0.0543 ms |         - |         - |        - |    7.85 KB |
| PerSample        | 100000 | 72.764 ms | 57.9848 ms | 3.1783 ms | 3142.8571 | 3000.0000 | 857.1429 | 39063.2 KB |
| PerClass         | 100000 |  4.623 ms |  0.2082 ms | 0.0114 ms |         - |         - |        - |    7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Dp**         | **4**    |     **73.44 ns** |     **1.266 ns** |   **0.069 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     73.43 ns |     1.473 ns |   0.081 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     74.83 ns |    25.876 ns |   1.418 ns |  1.02 |    0.02 |         - |          NA |
| Kernel_Cjk | 4    |     73.39 ns |     0.476 ns |   0.026 ns |  1.00 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **106.23 ns** |    **53.077 ns** |   **2.909 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 6    |     90.20 ns |    23.785 ns |   1.304 ns |  0.85 |    0.02 |         - |          NA |
| Dp_Cjk     | 6    |    105.70 ns |     2.239 ns |   0.123 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    130.17 ns |     0.822 ns |   0.045 ns |  1.23 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **156.74 ns** |     **1.916 ns** |   **0.105 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    102.82 ns |     2.205 ns |   0.121 ns |  0.66 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    157.08 ns |     7.267 ns |   0.398 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    181.26 ns |    17.036 ns |   0.934 ns |  1.16 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **202.42 ns** |    **28.722 ns** |   **1.574 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |    109.53 ns |     8.421 ns |   0.462 ns |  0.54 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    198.44 ns |    43.335 ns |   2.375 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    149.96 ns |     1.511 ns |   0.083 ns |  0.74 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **244.85 ns** |     **5.111 ns** |   **0.280 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    121.20 ns |     0.511 ns |   0.028 ns |  0.49 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    245.67 ns |    30.207 ns |   1.656 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    162.56 ns |     4.488 ns |   0.246 ns |  0.66 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **409.78 ns** |    **10.037 ns** |   **0.550 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    132.80 ns |     3.562 ns |   0.195 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    411.69 ns |    13.265 ns |   0.727 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    179.37 ns |     3.997 ns |   0.219 ns |  0.44 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **847.19 ns** |   **156.392 ns** |   **8.572 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |    169.26 ns |    10.756 ns |   0.590 ns |  0.20 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    842.04 ns |    10.644 ns |   0.583 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    223.87 ns |     6.092 ns |   0.334 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,451.33 ns** |    **56.414 ns** |   **3.092 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    205.53 ns |     5.970 ns |   0.327 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,427.36 ns |   178.792 ns |   9.800 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    273.65 ns |     6.646 ns |   0.364 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,352.38 ns** |   **175.475 ns** |   **9.618 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    278.81 ns |    24.064 ns |   1.319 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,406.53 ns |   196.684 ns |  10.781 ns |  1.02 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    355.66 ns |     7.115 ns |   0.390 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,349.97 ns** | **4,984.504 ns** | **273.217 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 64   |    345.61 ns |     2.667 ns |   0.146 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  7,067.59 ns |   577.383 ns |  31.648 ns |  1.11 |    0.04 |         - |          NA |
| Kernel_Cjk | 64   |    449.02 ns |     9.021 ns |   0.494 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **14,227.11 ns** | **6,192.859 ns** | **339.452 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    740.59 ns |    52.677 ns |   2.887 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 14,191.88 ns | 5,208.618 ns | 285.502 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |  1,558.13 ns |   173.044 ns |   9.485 ns |  0.11 |    0.00 |         - |          NA |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **WholeFit**              | **10000**    | **10**         |   **206.34 μs** |  **43.103 μs** |  **2.363 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   229.38 μs |  20.121 μs |  1.103 μs |  1.11 |    0.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    58.81 μs |   5.703 μs |  0.313 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   197.48 μs |  10.556 μs |  0.579 μs |  0.96 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **204.99 μs** |  **20.342 μs** |  **1.115 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   232.12 μs |   7.224 μs |  0.396 μs |  1.13 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    58.81 μs |   1.944 μs |  0.107 μs |  0.29 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   199.21 μs |  60.448 μs |  3.313 μs |  0.97 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,979.80 μs** | **178.060 μs** |  **9.760 μs** |  **1.00** |    **0.01** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,042.52 μs | 195.398 μs | 10.710 μs |  1.03 |    0.01 |      - |    7787 B |        9.70 |
| SparseFit             | 100000   | 10         | 1,088.19 μs |  43.907 μs |  2.407 μs |  0.55 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,969.54 μs |  17.917 μs |  0.982 μs |  0.99 |    0.00 |      - |     803 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **2,037.82 μs** |  **50.779 μs** |  **2.783 μs** |  **1.00** |    **0.00** |      **-** |     **803 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 2,127.82 μs | 788.511 μs | 43.221 μs |  1.04 |    0.02 | 3.9063 |   77627 B |       96.67 |
| SparseFit             | 100000   | 100        | 1,093.00 μs |  52.478 μs |  2.877 μs |  0.54 |    0.00 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,967.80 μs |  60.215 μs |  3.301 μs |  0.97 |    0.00 |      - |     803 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.558 μs** |  **0.5178 μs** | **0.0284 μs** |         **-** |
| EqualLength | 65           |     2.989 μs |  0.0072 μs | 0.0004 μs |         - |
| **Embedded**    | **128**          |    **39.271 μs** |  **1.8581 μs** | **0.1018 μs** |         **-** |
| EqualLength | 128          |     5.418 μs |  0.0447 μs | 0.0025 μs |         - |
| **Embedded**    | **512**          | **1,356.692 μs** | **38.0978 μs** | **2.0883 μs** |         **-** |
| EqualLength | 512          |    44.586 μs |  1.1312 μs | 0.0620 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 10.68 ms | 2.073 ms | 0.114 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 16.33 ms | 1.313 ms | 0.072 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| VocabTxt               |  3.902 ms |  2.3917 ms | 0.1311 ms | 109.3750 | 101.5625 |  31.2500 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.238 ms |  2.9354 ms | 0.1609 ms | 187.5000 | 171.8750 |  46.8750 |  5852.41 KB |
| TokenizerJsonUnigram   | 12.448 ms |  1.1742 ms | 0.0644 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  6.092 ms |  1.9460 ms | 0.1067 ms | 132.8125 | 125.0000 |  39.0625 |  4807.66 KB |
| TfidfSave              |  1.541 ms |  0.1897 ms | 0.0104 ms |  23.4375 |  17.5781 |  17.5781 |  2137.06 KB |
| TfidfLoad              |  4.214 ms |  0.3069 ms | 0.0168 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  4.143 ms |  0.3663 ms | 0.0201 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.296 ms |  0.1249 ms | 0.0068 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.516 ms | 12.3163 ms | 0.6751 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 70.833 ms |  2.0688 ms | 0.1134 ms |        - |        - |        - | 16095.25 KB |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar_ExplainedVariance** | **100x200** |  **9,518.10 μs** | **1,242.481 μs** |  **68.105 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,290.66 μs | 3,044.766 μs | 166.894 μs |  1.61 |    0.02 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **130.83 μs** |    **12.877 μs** |   **0.706 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    184.50 μs |    15.829 μs |   0.868 μs |  1.41 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,917.59 μs** |   **357.864 μs** |  **19.616 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,614.34 μs |   121.279 μs |   6.648 μs |  0.90 |    0.01 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |              |            |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **22.57 μs** |     **1.235 μs** |   **0.068 μs** |  **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.81 μs |     4.834 μs |   0.265 μs |  1.14 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

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

_As of 2026-10-22, measured at commit `e5d07e37e3fcd62ac524e3ebc1069dc02e0a0e66`._

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

| Method                            | Rows   | Mean      | Error      | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|-----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 37.290 ms |  0.7043 ms | 0.0386 ms |       - |       - |       - |  800349 B |
| NdcgIgnoringTies                  | 100000 | 34.895 ms |  1.5690 ms | 0.0860 ms |       - |       - |       - |  800345 B |
| DcgTieAveraged                    | 100000 | 21.089 ms |  0.9330 ms | 0.0511 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 18.223 ms |  0.2726 ms | 0.0149 ms |       - |       - |       - |         - |
| CoverageErrorScore                | 100000 |  5.536 ms |  0.3410 ms | 0.0187 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 53.992 ms | 18.5449 ms | 1.0165 ms |       - |       - |       - |         - |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **45.40 μs** |   **4.750 μs** |  **0.260 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    44.96 μs |   3.401 μs |  0.186 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.01 μs |   5.719 μs |  0.313 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   570.16 μs |  65.596 μs |  3.596 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **462.41 μs** | **179.148 μs** |  **9.820 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   466.19 μs |  10.635 μs |  0.583 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,132.16 μs |  98.141 μs |  5.379 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 6,226.51 μs | 314.729 μs | 17.251 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method                 | Mean            | Error           | StdDev       | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|----------------:|-------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,154.0 ns |       203.26 ns |     11.14 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |      1,390.4 ns |       744.21 ns |     40.79 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,209.4 ns |       725.92 ns |     39.79 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |        461.0 ns |        72.58 ns |      3.98 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,285,814.4 ns | 1,711,152.89 ns | 93,794.06 ns | 875.0000 | 843.7500 | 609.3750 | 12227071 B |

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Count**                | **200**       | **1.886 ms** | **0.1688 ms** | **0.0093 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.376 ms | 0.0661 ms | 0.0036 ms |  0.73 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 2.018 ms | 0.1224 ms | 0.0067 ms |  1.07 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.667 ms | 0.0368 ms | 0.0020 ms |  0.88 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **9.179 ms** | **1.4995 ms** | **0.0822 ms** |  **1.00** | **859.3750** | **859.3750** | **859.3750** |  **3436.5 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 7.172 ms | 0.5946 ms | 0.0326 ms |  0.78 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 9.650 ms | 1.2916 ms | 0.0708 ms |  1.05 | 515.6250 | 500.0000 | 500.0000 | 2349.22 KB |        0.68 |
| HashingWithStopWords | 1000      | 8.655 ms | 0.3191 ms | 0.0175 ms |  0.94 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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
| **Lodestar**     | **WordPiece**     |  **21.79 ms** |   **1.233 ms** | **0.068 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  55.24 ms |  10.169 ms | 0.557 ms |  2.53 |    0.02 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.04 ms** |   **1.862 ms** | **0.102 ms** |  **1.00** |    **0.00** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  56.58 ms |   1.232 ms | 0.068 ms |  1.20 |    0.00 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **87.13 ms** |  **17.819 ms** | **0.977 ms** |  **1.00** |    **0.01** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 281.26 ms | 111.259 ms | 6.098 ms |  3.23 |    0.07 | 3500.0000 |  59.08 MB |        2.08 |

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

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

| Method   | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **3.776 ms** |   **0.3529 ms** | **0.0193 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  58.549 ms | 119.8265 ms | 6.5681 ms | 15.51 |    1.51 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       15.84 |
|          |           |            |             |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **17.227 ms** |   **1.0022 ms** | **0.0549 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 413.859 ms | 141.0443 ms | 7.7311 ms | 24.02 |    0.39 | 75000.0000 | 75000.0000 | 75000.0000 | 324.64 MB |       40.03 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.169 | 0.036 | 0.21x | 0.169 | 0.036 | 0.21x |
| cdist_wratio_n50 | 2.241 | 2.487 | 1.11x | 2.241 | 2.486 | 1.11x |
| cdist_ratio_varied_n50 | 0.147 | 0.031 | 0.21x | 0.147 | 0.031 | 0.21x |
| cdist_ratio_varied_cutoff90_n50 | 0.064 | 0.031 | 0.49x | 0.064 | 0.031 | 0.49x |
| cdist_ratio_n200 | 2.970 | 0.439 | 0.15x | 2.975 | 0.439 | 0.15x |
| cdist_wratio_n200 | 34.168 | 37.332 | 1.09x | 34.167 | 37.328 | 1.09x |
| cdist_ratio_varied_n200 | 2.483 | 0.385 | 0.16x | 2.487 | 0.385 | 0.15x |
| cdist_ratio_varied_cutoff90_n200 | 1.102 | 0.387 | 0.35x | 1.105 | 0.387 | 0.35x |
| cdist_ratio_n500 | 17.329 | 2.642 | 0.15x | 17.353 | 2.642 | 0.15x |
| cdist_wratio_n500 | 206.334 | 227.534 | 1.10x | 206.489 | 227.513 | 1.10x |
| cdist_ratio_varied_n500 | 14.380 | 2.292 | 0.16x | 14.496 | 2.292 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 6.215 | 2.290 | 0.37x | 6.326 | 2.290 | 0.36x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

### compare-indel

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 119.3 | 25.8 | 4.63x C# faster |
| latin | 32 | 183.3 | 86.2 | 2.13x C# faster |
| latin | 128 | 486.3 | 423.0 | 1.15x C# faster |
| latin | 512 | 4473.4 | 4942.1 | 1.10x Py faster |
| cjk | 8 | 142.7 | 26.6 | 5.36x C# faster |
| cjk | 32 | 331.8 | 222.0 | 1.49x C# faster |
| cjk | 128 | 1901.7 | 1380.6 | 1.38x C# faster |
| cjk | 512 | 14860.8 | 8833.4 | 1.68x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

### compare-levenshtein

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 147.8 | 18.1 | 8.18x C# faster |
| latin | 32 | 282.8 | 157.8 | 1.79x C# faster |
| latin | 128 | 1689.9 | 884.2 | 1.91x C# faster |
| latin | 512 | 14026.4 | 11561.1 | 1.21x C# faster |
| cjk | 8 | 159.3 | 17.8 | 8.95x C# faster |
| cjk | 32 | 387.2 | 290.8 | 1.33x C# faster |
| cjk | 128 | 2850.1 | 2082.6 | 1.37x C# faster |
| cjk | 512 | 23656.2 | 19128.6 | 1.24x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 1.014 | 236.90x | 0.004 | 1.014 | 236.92x |
| accuracy_n1000_k2 | 0.000 | 0.539 | 4035.91x | 0.000 | 0.539 | 4035.97x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.830 | 368.74x | 0.005 | 1.830 | 368.74x |
| classification_report_n1000_k2 | 0.007 | 7.015 | 964.88x | 0.007 | 7.015 | 964.93x |
| roc_auc_binary_n1000_k2 | 0.017 | 1.955 | 112.74x | 0.017 | 1.955 | 112.66x |
| balanced_accuracy_n1000_k2 | 0.004 | 1.073 | 242.81x | 0.004 | 1.073 | 242.75x |
| matthews_n1000_k2 | 0.004 | 2.037 | 468.08x | 0.004 | 2.037 | 468.07x |
| cohen_kappa_n1000_k2 | 0.004 | 1.091 | 250.85x | 0.004 | 1.091 | 250.83x |
| mse_n1000_k2 | 0.000 | 0.308 | 627.45x | 0.000 | 0.307 | 627.40x |
| mae_n1000_k2 | 0.000 | 0.306 | 625.81x | 0.000 | 0.306 | 625.76x |
| median_ae_n1000_k2 | 0.005 | 0.325 | 59.46x | 0.005 | 0.325 | 59.45x |
| r2_n1000_k2 | 0.001 | 0.376 | 314.78x | 0.001 | 0.376 | 314.79x |
| confusion_matrix_n1000_k10 | 0.005 | 0.998 | 216.46x | 0.005 | 0.998 | 216.45x |
| accuracy_n1000_k10 | 0.000 | 0.532 | 3974.74x | 0.000 | 0.532 | 3974.53x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.837 | 315.88x | 0.006 | 1.837 | 315.90x |
| classification_report_n1000_k10 | 0.012 | 7.077 | 610.72x | 0.012 | 7.077 | 610.69x |
| roc_auc_ovr_macro_n1000_k10 | 0.514 | 9.901 | 19.24x | 0.514 | 9.900 | 19.24x |
| balanced_accuracy_n1000_k10 | 0.005 | 1.058 | 203.13x | 0.005 | 1.058 | 203.13x |
| matthews_n1000_k10 | 0.005 | 2.064 | 399.33x | 0.005 | 2.064 | 399.33x |
| cohen_kappa_n1000_k10 | 0.006 | 1.115 | 193.09x | 0.006 | 1.115 | 193.10x |
| mse_n1000_k10 | 0.000 | 0.308 | 636.79x | 0.000 | 0.308 | 636.73x |
| mae_n1000_k10 | 0.000 | 0.309 | 638.23x | 0.000 | 0.309 | 638.23x |
| median_ae_n1000_k10 | 0.006 | 0.320 | 56.31x | 0.006 | 0.320 | 56.31x |
| r2_n1000_k10 | 0.001 | 0.379 | 316.89x | 0.001 | 0.379 | 316.87x |
| confusion_matrix_n100000_k2 | 0.460 | 10.923 | 23.77x | 0.460 | 10.922 | 23.77x |
| accuracy_n100000_k2 | 0.012 | 3.800 | 308.47x | 0.012 | 3.800 | 308.45x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.581 | 27.32x | 0.460 | 12.579 | 27.32x |
| classification_report_n100000_k2 | 0.463 | 27.779 | 60.05x | 0.463 | 27.777 | 60.05x |
| roc_auc_binary_n100000_k2 | 4.299 | 27.428 | 6.38x | 4.298 | 27.423 | 6.38x |
| balanced_accuracy_n100000_k2 | 0.459 | 11.124 | 24.23x | 0.459 | 11.123 | 24.23x |
| matthews_n100000_k2 | 0.458 | 22.061 | 48.13x | 0.458 | 22.058 | 48.13x |
| cohen_kappa_n100000_k2 | 0.456 | 11.090 | 24.34x | 0.456 | 11.089 | 24.34x |
| mse_n100000_k2 | 0.046 | 0.474 | 10.42x | 0.046 | 0.474 | 10.42x |
| mae_n100000_k2 | 0.045 | 0.466 | 10.34x | 0.045 | 0.465 | 10.34x |
| median_ae_n100000_k2 | 0.698 | 1.820 | 2.61x | 0.719 | 1.820 | 2.53x |
| r2_n100000_k2 | 0.110 | 0.722 | 6.56x | 0.110 | 0.722 | 6.56x |
| confusion_matrix_n100000_k10 | 0.492 | 10.972 | 22.29x | 0.492 | 10.968 | 22.28x |
| accuracy_n100000_k10 | 0.013 | 3.820 | 300.59x | 0.013 | 3.819 | 300.54x |
| precision_recall_f1_macro_n100000_k10 | 0.493 | 13.109 | 26.57x | 0.493 | 13.108 | 26.57x |
| classification_report_n100000_k10 | 0.500 | 30.014 | 60.02x | 0.500 | 30.012 | 60.02x |
| roc_auc_ovr_macro_n100000_k10 | 35.076 | 221.562 | 6.32x | 35.071 | 221.546 | 6.32x |
| balanced_accuracy_n100000_k10 | 0.493 | 10.960 | 22.23x | 0.493 | 10.960 | 22.23x |
| matthews_n100000_k10 | 0.493 | 22.609 | 45.87x | 0.493 | 22.607 | 45.87x |
| cohen_kappa_n100000_k10 | 0.494 | 10.964 | 22.21x | 0.494 | 10.964 | 22.21x |
| mse_n100000_k10 | 0.045 | 0.474 | 10.43x | 0.045 | 0.474 | 10.43x |
| mae_n100000_k10 | 0.045 | 0.463 | 10.32x | 0.045 | 0.463 | 10.32x |
| median_ae_n100000_k10 | 0.698 | 1.820 | 2.61x | 0.744 | 1.819 | 2.45x |
| r2_n100000_k10 | 0.110 | 0.718 | 6.53x | 0.110 | 0.718 | 6.53x |
| confusion_matrix_n1000000_k2 | 4.603 | 101.549 | 22.06x | 4.603 | 101.542 | 22.06x |
| accuracy_n1000000_k2 | 0.134 | 33.431 | 249.79x | 0.134 | 33.429 | 249.78x |
| precision_recall_f1_macro_n1000000_k2 | 4.603 | 109.037 | 23.69x | 4.603 | 109.018 | 23.68x |
| classification_report_n1000000_k2 | 4.619 | 212.670 | 46.04x | 4.620 | 212.645 | 46.03x |
| roc_auc_binary_n1000000_k2 | 56.994 | 293.028 | 5.14x | 56.994 | 292.990 | 5.14x |
| balanced_accuracy_n1000000_k2 | 4.600 | 100.675 | 21.89x | 4.599 | 100.643 | 21.88x |
| matthews_n1000000_k2 | 4.602 | 200.919 | 43.66x | 4.602 | 200.890 | 43.65x |
| cohen_kappa_n1000000_k2 | 4.606 | 100.544 | 21.83x | 4.605 | 100.531 | 21.83x |
| mse_n1000000_k2 | 0.452 | 2.447 | 5.41x | 0.452 | 2.446 | 5.41x |
| mae_n1000000_k2 | 0.452 | 2.407 | 5.33x | 0.452 | 2.407 | 5.33x |
| median_ae_n1000000_k2 | 6.437 | 14.440 | 2.24x | 6.517 | 14.438 | 2.22x |
| r2_n1000000_k2 | 1.105 | 4.412 | 3.99x | 1.105 | 4.410 | 3.99x |
| confusion_matrix_n1000000_k10 | 4.909 | 100.581 | 20.49x | 4.909 | 100.558 | 20.49x |
| accuracy_n1000000_k10 | 0.136 | 33.479 | 246.36x | 0.136 | 33.464 | 246.28x |
| precision_recall_f1_macro_n1000000_k10 | 4.910 | 115.227 | 23.47x | 4.909 | 115.217 | 23.47x |
| classification_report_n1000000_k10 | 4.913 | 237.443 | 48.33x | 4.913 | 237.424 | 48.32x |
| balanced_accuracy_n1000000_k10 | 4.908 | 101.116 | 20.60x | 4.907 | 101.106 | 20.60x |
| matthews_n1000000_k10 | 4.911 | 209.850 | 42.73x | 4.910 | 209.823 | 42.73x |
| cohen_kappa_n1000000_k10 | 4.908 | 101.168 | 20.61x | 4.908 | 101.160 | 20.61x |
| mse_n1000000_k10 | 0.443 | 2.530 | 5.71x | 0.443 | 2.530 | 5.71x |
| mae_n1000000_k10 | 0.447 | 2.502 | 5.60x | 0.446 | 2.502 | 5.60x |
| median_ae_n1000000_k10 | 6.290 | 14.714 | 2.34x | 6.346 | 14.712 | 2.32x |
| r2_n1000000_k10 | 1.122 | 4.192 | 3.74x | 1.122 | 4.191 | 3.74x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.118 | 0.869 | 0.41x | 2.118 | 0.869 | 0.41x |
| transform_frobenius_n500 | 6.949 | 1.639 | 0.24x | 6.949 | 1.639 | 0.24x |
| transform_frobenius_n2000 | 25.401 | 5.895 | 0.23x | 25.640 | 23.520 | 0.92x |
| transform_kl_n100 | 1.127 | 17.852 | 15.84x | 1.127 | 17.852 | 15.84x |
| transform_kl_n500 | 5.067 | 34.393 | 6.79x | 5.066 | 34.391 | 6.79x |
| transform_kl_n2000 | 20.136 | 122.497 | 6.08x | 20.200 | 451.329 | 22.34x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

### compare-persistence

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.112 | 9.896 | 1.94x | 5.463 | 9.895 | 1.81x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.061 | 16.085 | 1.33x | 12.343 | 16.084 | 1.30x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.886 | 39.621 | 2.85x | 14.217 | 39.616 | 2.79x | 1,990,038 | 1,990,038 |
| spiece_model | 7.948 | 29.742 | 3.74x | 8.215 | 29.740 | 3.62x | 533,084 | 533,084 |
| tfidf_save | 1.483 | 2.332 | 1.57x | 1.501 | 2.332 | 1.55x | 581,787 | 591,922 |
| tfidf_load | 4.677 | 4.134 | 0.88x | 4.880 | 4.134 | 0.85x | 581,787 | 591,922 |
| embedding_index_save | 4.338 | 1.508 | 0.35x | 4.538 | 1.508 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.009 | 37.386 | 0.75x | 10.874 | 5.065 | 0.47x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.685 | 1.359 | 0.24x | 6.052 | 1.359 | 0.22x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.483 | 0.957 | 0.15x | 6.867 | 0.955 | 0.14x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.497 | 1.384 | 0.31x | 4.781 | 1.383 | 0.29x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.463 | 1.436 | 0.98x | 1.785 | 1.436 | 0.80x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 81.30x | 0.000 | 0.001 | 81.31x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 409.471 | 555.195 | 1.36x | 409.398 | 555.123 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 70.827 | 66.729 | 0.94x | 70.796 | 66.719 | 0.94x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.092 | 0.113 | 1.23x | 0.092 | 0.113 | 1.23x |
| stratified_n10000 | 0.180 | 0.884 | 4.91x | 0.180 | 0.884 | 4.91x |
| traintest_n10000 | 0.005 | 0.239 | 50.65x | 0.005 | 0.239 | 50.64x |
| kfold_seeded_n10000 | 0.308 | 0.670 | 2.17x | 0.308 | 0.670 | 2.17x |
| stratified_seeded_n10000 | 0.437 | 1.483 | 3.40x | 0.437 | 1.483 | 3.40x |
| traintest_seeded_n10000 | 0.174 | 0.525 | 3.01x | 0.174 | 0.525 | 3.01x |
| stratified_traintest_n10000 | 0.669 | 2.756 | 4.12x | 0.669 | 2.756 | 4.12x |
| group_kfold_n10000 | 0.129 | 0.603 | 4.67x | 0.129 | 0.603 | 4.67x |
| stratified_group_kfold_n10000 | 0.245 | 32.000 | 130.50x | 0.245 | 31.997 | 130.49x |
| timeseries_n10000 | 0.000 | 0.012 | 87.72x | 0.000 | 0.012 | 87.72x |
| repeated_kfold_n10000 | 0.990 | 1.784 | 1.80x | 0.990 | 1.784 | 1.80x |
| kfold_n100000 | 1.151 | 2.535 | 2.20x | 1.302 | 2.535 | 1.95x |
| stratified_n100000 | 2.186 | 7.095 | 3.25x | 2.318 | 7.094 | 3.06x |
| traintest_n100000 | 0.209 | 0.406 | 1.94x | 0.211 | 0.406 | 1.93x |
| kfold_seeded_n100000 | 3.532 | 6.906 | 1.96x | 3.685 | 6.906 | 1.87x |
| stratified_seeded_n100000 | 4.600 | 11.395 | 2.48x | 4.776 | 11.393 | 2.39x |
| traintest_seeded_n100000 | 2.058 | 2.004 | 0.97x | 2.102 | 2.004 | 0.95x |
| stratified_traintest_n100000 | 8.087 | 23.747 | 2.94x | 8.185 | 23.745 | 2.90x |
| group_kfold_n100000 | 1.588 | 6.778 | 4.27x | 1.722 | 6.778 | 3.94x |
| stratified_group_kfold_n100000 | 2.604 | 323.799 | 124.35x | 2.744 | 323.776 | 118.00x |
| timeseries_n100000 | 0.000 | 0.042 | 350.95x | 0.000 | 0.042 | 350.96x |
| repeated_kfold_n100000 | 9.948 | 18.965 | 1.91x | 10.153 | 18.963 | 1.87x |
| kfold_n1000000 | 9.377 | 17.311 | 1.85x | 9.940 | 17.310 | 1.74x |
| stratified_n1000000 | 20.348 | 66.685 | 3.28x | 20.891 | 66.662 | 3.19x |
| traintest_n1000000 | 0.481 | 2.069 | 4.30x | 0.554 | 2.069 | 3.74x |
| kfold_seeded_n1000000 | 36.254 | 58.664 | 1.62x | 36.987 | 58.653 | 1.59x |
| stratified_seeded_n1000000 | 47.931 | 109.014 | 2.27x | 49.174 | 109.003 | 2.22x |
| traintest_seeded_n1000000 | 20.681 | 19.218 | 0.93x | 20.954 | 19.216 | 0.92x |
| stratified_traintest_n1000000 | 93.697 | 234.430 | 2.50x | 94.239 | 234.399 | 2.49x |
| group_kfold_n1000000 | 14.827 | 53.277 | 3.59x | 15.585 | 53.270 | 3.42x |
| stratified_group_kfold_n1000000 | 27.405 | 3216.178 | 117.36x | 28.482 | 3215.802 | 112.91x |
| timeseries_n1000000 | 0.000 | 0.328 | 3097.57x | 0.000 | 0.328 | 3097.93x |
| repeated_kfold_n1000000 | 106.545 | 173.143 | 1.63x | 108.103 | 173.125 | 1.60x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.005 | 0.823 | 151.96x | 0.005 | 0.823 | 151.96x |
| mann_whitney_n1000 | 0.031 | 0.748 | 24.50x | 0.031 | 0.748 | 24.50x |
| chi_square_n1000 | 0.000 | 0.340 | 1673.73x | 0.000 | 0.340 | 1673.84x |
| fligner_n1000 | 0.075 | 0.820 | 11.01x | 0.075 | 0.820 | 11.01x |
| anderson_ksamp_n1000 | 0.167 | 0.396 | 2.37x | 0.167 | 0.396 | 2.37x |
| spearman_matrix_n1000 | 0.094 | 0.931 | 9.90x | 0.094 | 0.931 | 9.90x |
| pointbiserial_n1000 | 0.010 | 0.573 | 55.77x | 0.010 | 0.573 | 55.77x |
| welch_t_n10000 | 0.052 | 0.872 | 16.88x | 0.052 | 0.872 | 16.88x |
| mann_whitney_n10000 | 0.706 | 2.840 | 4.03x | 0.705 | 2.840 | 4.03x |
| chi_square_n10000 | 0.001 | 0.349 | 254.72x | 0.001 | 0.349 | 254.72x |
| fligner_n10000 | 1.405 | 3.250 | 2.31x | 1.471 | 3.250 | 2.21x |
| anderson_ksamp_n10000 | 2.034 | 4.021 | 1.98x | 2.156 | 4.020 | 1.86x |
| spearman_matrix_n10000 | 2.199 | 4.781 | 2.17x | 2.276 | 4.780 | 2.10x |
| pointbiserial_n10000 | 0.099 | 0.646 | 6.52x | 0.099 | 0.646 | 6.52x |
| welch_t_n100000 | 0.517 | 1.246 | 2.41x | 0.517 | 1.245 | 2.41x |
| mann_whitney_n100000 | 7.745 | 28.716 | 3.71x | 7.744 | 28.714 | 3.71x |
| chi_square_n100000 | 0.014 | 0.366 | 26.62x | 0.014 | 0.366 | 26.62x |
| fligner_n100000 | 14.486 | 32.957 | 2.28x | 14.902 | 32.955 | 2.21x |
| anderson_ksamp_n100000 | 15.603 | 44.008 | 2.82x | 15.995 | 44.005 | 2.75x |
| spearman_matrix_n100000 | 18.078 | 54.171 | 3.00x | 18.500 | 54.168 | 2.93x |
| pointbiserial_n100000 | 1.288 | 1.440 | 1.12x | 1.302 | 5.743 | 4.41x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

### compare-transformers

_As of 2026-10-25, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.270 | 0.465 | 1.72x | 0.273 | 0.465 | 1.70x |
| polynomial_n10000 | 0.737 | 1.032 | 1.40x | 0.742 | 1.032 | 1.39x |
| kbins_fit_n10000 | 1.450 | 2.622 | 1.81x | 1.450 | 2.622 | 1.81x |
| kbins_n10000 | 0.722 | 4.468 | 6.18x | 0.815 | 4.467 | 5.48x |
| quantile_fit_n10000 | 1.543 | 3.797 | 2.46x | 1.543 | 3.796 | 2.46x |
| quantile_n10000 | 1.838 | 1.702 | 0.93x | 1.842 | 1.702 | 0.92x |
| power_fit_n10000 | 6.187 | 135.365 | 21.88x | 6.190 | 135.343 | 21.87x |
| power_n10000 | 1.545 | 1.518 | 0.98x | 1.549 | 1.518 | 0.98x |
| knn_impute_n10000 | 7.351 | 23.650 | 3.22x | 7.350 | 94.270 | 12.83x |
| label_n10000 | 1.155 | 0.764 | 0.66x | 1.156 | 0.764 | 0.66x |
| normalize_n100000 | 1.423 | 1.859 | 1.31x | 1.799 | 1.859 | 1.03x |
| polynomial_n100000 | 2.875 | 7.192 | 2.50x | 3.515 | 7.191 | 2.05x |
| kbins_fit_n100000 | 14.854 | 8.270 | 0.56x | 15.666 | 8.270 | 0.53x |
| kbins_n100000 | 6.545 | 12.597 | 1.92x | 7.223 | 12.596 | 1.74x |
| quantile_fit_n100000 | 15.051 | 18.098 | 1.20x | 15.798 | 18.097 | 1.15x |
| quantile_n100000 | 17.013 | 14.897 | 0.88x | 17.406 | 14.896 | 0.86x |
| power_fit_n100000 | 67.106 | 917.186 | 13.67x | 69.896 | 917.142 | 13.12x |
| power_n100000 | 14.163 | 10.071 | 0.71x | 14.613 | 10.071 | 0.69x |
| knn_impute_n100000 | 7.367 | 19.384 | 2.63x | 7.366 | 77.267 | 10.49x |
| label_n100000 | 15.419 | 7.292 | 0.47x | 15.752 | 7.292 | 0.46x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
