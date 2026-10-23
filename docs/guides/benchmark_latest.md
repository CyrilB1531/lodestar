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

_As of 2026-10-23, measured at commit `b7818719585e35e8c1448884e1c36a152a3cd170`._

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

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **13.69 μs** |   **1.614 μs** |  **0.088 μs** |  **1.00** |    **0.01** | **0.9613** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    16.14 μs |  21.712 μs |  1.190 μs |  1.18 |    0.08 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,345.96 μs** |  **55.228 μs** |  **3.027 μs** |  **1.00** |    **0.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,054.99 μs | 235.587 μs | 12.913 μs |  0.78 |    0.01 | 7.8125 |  156.3 KB |        1.00 |

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 13.89 ms |  3.641 ms | 0.200 ms |  812.5000 |  781.2500 | 406.2500 |  21.53 MB |
| CountLoadAsync4M | 34.59 ms | 11.979 ms | 0.657 ms | 1538.4615 | 1461.5385 | 692.3077 |  43.93 MB |
| CountLoad4M      | 34.75 ms | 32.139 ms | 1.762 ms | 1466.6667 | 1400.0000 | 666.6667 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

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

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **29.28 μs** |      **7.213 μs** |     **0.395 μs** |         **-** |
| Cjk    | 1000   |      36.22 μs |      8.093 μs |     0.444 μs |         - |
| **Latin**  | **10000**  |   **2,729.59 μs** |     **98.503 μs** |     **5.399 μs** |         **-** |
| Cjk    | 10000  |   3,804.23 μs |  1,289.087 μs |    70.659 μs |         - |
| **Latin**  | **65536**  | **129,101.75 μs** | **48,320.528 μs** | **2,648.611 μs** |         **-** |

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

| Method  | Mean     | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 15.41 ms |  2.619 ms | 0.144 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 55.94 ms | 42.435 ms | 2.326 ms |  3.63 |    0.13 | 1777.7778 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean       | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |-----------:|----------:|----------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **9.084 μs** |  **1.276 μs** | **0.0700 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **19.637 μs** |  **5.355 μs** | **0.2935 μs** | **0.8850** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **41.527 μs** | **11.607 μs** | **0.6362 μs** | **1.7090** | **0.0610** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **261.936 μs** | **27.315 μs** | **1.4972 μs** | **3.4180** |      **-** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

| Method     | Alphabet | Mean      | Error    | StdDev    | Allocated |
|----------- |--------- |----------:|---------:|----------:|----------:|
| **DpGroup**    | **cjk**      | **10.262 μs** | **1.234 μs** | **0.0677 μs** |         **-** |
| MyersGroup | cjk      | 92.818 μs | 8.088 μs | 0.4433 μs |         - |
| **DpGroup**    | **latin**    |  **5.668 μs** | **2.139 μs** | **0.1173 μs** |         **-** |
| MyersGroup | latin    | 74.849 μs | 6.480 μs | 0.3552 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

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

| Method       | Shape     | Mean        | Error    | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|---------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **113.84 ms** | **11.82 ms** | **0.648 ms** |  **1.00** |    **0.01** |  **1800.0000** | **1800.0000** | **1800.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,292.93 ms | 72.26 ms | 3.961 ms | 11.36 |    0.06 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |          |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **38.91 ms** | **35.01 ms** | **1.919 ms** |  **1.00** |    **0.06** |  **1428.5714** | **1428.5714** | **1428.5714** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   453.68 ms | 61.25 ms | 3.357 ms | 11.68 |    0.49 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                                    | Mean      | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  7.982 ms |  6.5162 ms | 0.3572 ms |  1.00 |    0.05 |
| Nmf_Rank20                                | 60.999 ms | 13.2808 ms | 0.7280 ms |  7.65 |    0.30 |
| Nmf_Transform                             |  1.937 ms |  0.1887 ms | 0.0103 ms |  0.24 |    0.01 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 12.434 ms |  0.4980 ms | 0.0273 ms |  1.56 |    0.06 |

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

| Method         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  62.19 ns |   6.659 ns |  0.365 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   | 436.11 ns |   7.658 ns |  0.420 ns |  7.01 |    0.04 |      - |         - |          NA |
| TokenSortRatio | 491.72 ns | 222.726 ns | 12.208 ns |  7.91 |    0.17 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 540.63 ns |  97.773 ns |  5.359 ns |  8.69 |    0.09 | 0.0534 |     896 B |          NA |
| WRatio         | 704.91 ns | 213.958 ns | 11.728 ns | 11.34 |    0.17 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error              | StdDev          | Median             | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,302,953.5368 ns |    448,596.6611 ns |  24,589.0968 ns |  1,314,946.9053 ns |  1.000 |    0.02 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    332,322.1763 ns |     66,182.9199 ns |   3,627.7092 ns |    332,834.8062 ns |  0.255 |    0.00 | 35.6445 | 13.6719 |  602664 B |        3.75 |
| WRatio_ProseAndOneEmoji   |    736,175.0140 ns |    586,334.7897 ns |  32,138.9885 ns |    718,668.1543 ns |  0.565 |    0.02 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 53,871,438.6000 ns | 11,740,916.1235 ns | 643,559.2333 ns | 53,872,439.1000 ns | 41.356 |    0.81 |       - |       - |  510776 B |        3.18 |
| WRatio_EmptyOperand       |          0.0020 ns |          0.0453 ns |       0.0025 ns |          0.0012 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method      | NeedleLength | Mean       | Error       | StdDev     | Allocated |
|------------ |------------- |-----------:|------------:|-----------:|----------:|
| **Embedded**    | **65**           |   **6.426 μs** |   **4.5635 μs** |  **0.2501 μs** |         **-** |
| EqualLength | 65           |   1.622 μs |   0.0895 μs |  0.0049 μs |         - |
| **Embedded**    | **128**          |  **23.880 μs** |   **0.8605 μs** |  **0.0472 μs** |         **-** |
| EqualLength | 128          |   3.115 μs |   0.3842 μs |  0.0211 μs |         - |
| **Embedded**    | **512**          | **885.814 μs** | **600.8427 μs** | **32.9342 μs** |         **-** |
| EqualLength | 512          |  26.672 μs |   2.2769 μs |  0.1248 μs |         - |

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

| Method                 | Mean           | Error          | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|---------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |       614.1 ns |       462.3 ns |      25.34 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |       793.5 ns |       684.3 ns |      37.51 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |       658.2 ns |       479.1 ns |      26.26 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |       216.7 ns |       110.1 ns |       6.03 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 7,363,675.7 ns | 3,950,483.4 ns | 216,539.33 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

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

| Method   | Documents | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **2.771 ms** |   **1.111 ms** |  **0.0609 ms** |  **1.00** |    **0.03** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  29.592 ms |  24.488 ms |  1.3423 ms | 10.68 |    0.47 |  5600.0000 |  5600.0000 |  5600.0000 |  28.37 MB |       15.90 |
|          |           |            |            |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **12.314 ms** |   **5.937 ms** |  **0.3254 ms** |  **1.00** |    **0.03** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 246.049 ms | 198.376 ms | 10.8737 ms | 19.99 |    0.89 | 75500.0000 | 75500.0000 | 75500.0000 | 324.33 MB |       40.00 |

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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

_As of 2026-10-21, measured at commit `c4a19d86a5f8c9899f169f26ac7e107de05eedd1`._

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
