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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | PairCount | Mean             | Error            | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|-----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **622.8 ns** |        **292.61 ns** |      **16.04 ns** |     **1.00** |    **0.03** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         560.3 ns |         54.70 ns |       3.00 ns |     0.90 |    0.02 |   0.0095 |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 100       |       1,447.0 ns |         77.62 ns |       4.25 ns |     2.32 |    0.05 |   0.2460 |        - |        - |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       1,880.0 ns |         75.06 ns |       4.11 ns |     3.02 |    0.07 |   0.1202 |        - |        - |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       2,872.7 ns |        282.33 ns |      15.48 ns |     4.61 |    0.10 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |       5,814.2 ns |        504.56 ns |      27.66 ns |     9.34 |    0.21 |   0.0076 |        - |        - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       2,956.4 ns |      1,180.19 ns |      64.69 ns |     4.75 |    0.14 |        - |        - |        - |         - |          NA |
|                                |           |                  |                  |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **56,773.5 ns** |      **7,052.32 ns** |     **386.56 ns** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      42,609.8 ns |     10,688.42 ns |     585.87 ns |     0.75 |    0.01 |        - |        - |        - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |     574,785.5 ns |    315,261.52 ns |  17,280.55 ns |    10.12 |    0.27 | 166.0156 | 166.0156 | 166.0156 | 1324848 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,346,122.4 ns |    517,506.81 ns |  28,366.29 ns |    23.71 |    0.45 |   7.8125 |        - |        - |  160427 B |          NA |
| Lodestar_KendallTau            | 10000     |   1,704,477.9 ns |    696,985.76 ns |  38,204.14 ns |    30.02 |    0.61 |        - |        - |        - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 186,032,477.8 ns | 14,763,802.45 ns | 809,253.83 ns | 3,276.85 |   22.90 |        - |        - |        - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   1,693,905.3 ns |    120,927.41 ns |   6,628.44 ns |    29.84 |    0.20 |        - |        - |        - |         - |          NA |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **13.53 μs** |   **4.489 μs** |  **0.246 μs** |  **1.00** |    **0.02** | **0.9613** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    15.13 μs |   0.612 μs |  0.034 μs |  1.12 |    0.02 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |         |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,413.20 μs** | **634.573 μs** | **34.783 μs** |  **1.00** |    **0.03** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,062.88 μs | 135.753 μs |  7.441 μs |  0.75 |    0.02 | 7.8125 |  156.3 KB |        1.00 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **562.18 ns** |      **49.001 ns** |      **2.686 ns** |   **1.00** |    **0.01** | **0.0038** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 100        |     1,602.81 ns |     527.844 ns |     28.933 ns |   2.85 |    0.05 | 0.0057 |     104 B |        1.62 |
| Lodestar_MannWhitney           | 100        |     1,220.73 ns |     113.173 ns |      6.203 ns |   2.17 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 100        |     2,076.54 ns |   1,505.575 ns |     82.526 ns |   3.69 |    0.13 | 0.0687 |    1176 B |       18.38 |
| Lodestar_KruskalWallis         | 100        |     3,772.34 ns |     878.369 ns |     48.146 ns |   6.71 |    0.08 |      - |      32 B |        0.50 |
| MetaNumerics_KruskalWallis     | 100        |     5,014.05 ns |     272.633 ns |     14.944 ns |   8.92 |    0.04 | 0.1068 |    1896 B |       29.62 |
| Lodestar_KolmogorovSmirnov     | 100        |     1,252.20 ns |     632.128 ns |     34.649 ns |   2.23 |    0.05 | 0.0973 |    1648 B |       25.75 |
| MetaNumerics_KolmogorovSmirnov | 100        |     2,082.90 ns |     215.221 ns |     11.797 ns |   3.71 |    0.02 | 0.0687 |    1168 B |       18.25 |
| Lodestar_OneWayAnova           | 100        |       460.26 ns |      33.655 ns |      1.845 ns |   0.82 |    0.00 | 0.0019 |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 100        |     2,180.28 ns |   1,370.462 ns |     75.120 ns |   3.88 |    0.12 | 0.0420 |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 100        |       684.69 ns |     100.442 ns |      5.506 ns |   1.22 |    0.01 | 0.0486 |     824 B |       12.88 |
| MetaNumerics_Wilcoxon          | 100        |       547.95 ns |     149.714 ns |      8.206 ns |   0.97 |    0.01 | 0.0582 |     976 B |       15.25 |
| Lodestar_FisherExact           | 100        |       257.66 ns |     148.384 ns |      8.133 ns |   0.46 |    0.01 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 100        |       905.24 ns |     118.520 ns |      6.496 ns |   1.61 |    0.01 | 0.0563 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 100        |        66.96 ns |      39.131 ns |      2.145 ns |   0.12 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 100        |       410.61 ns |     218.113 ns |     11.956 ns |   0.73 |    0.02 | 0.0577 |     968 B |       15.12 |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **30,221.80 ns** |   **1,842.177 ns** |    **100.976 ns** |  **1.000** |    **0.00** |      **-** |      **64 B** |        **1.00** |
| MetaNumerics_StudentT          | 10000      |   131,868.39 ns |  24,205.602 ns |  1,326.791 ns |  4.363 |    0.04 |      - |     104 B |        1.62 |
| Lodestar_MannWhitney           | 10000      |   271,615.62 ns |  35,878.948 ns |  1,966.646 ns |  8.987 |    0.06 |      - |         - |        0.00 |
| MetaNumerics_MannWhitney       | 10000      | 1,415,391.66 ns | 151,506.100 ns |  8,304.561 ns | 46.834 |    0.27 | 3.9063 |   80379 B |    1,255.92 |
| Lodestar_KruskalWallis         | 10000      |   660,352.28 ns |  84,897.144 ns |  4,653.499 ns | 21.850 |    0.15 |      - |      33 B |        0.52 |
| MetaNumerics_KruskalWallis     | 10000      | 2,394,124.78 ns | 444,526.455 ns | 24,365.995 ns | 79.219 |    0.73 | 3.9063 |  120702 B |    1,885.97 |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,041,838.93 ns | 128,440.649 ns |  7,040.265 ns | 34.473 |    0.23 | 7.8125 |  160051 B |    2,500.80 |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,453,107.73 ns |  35,664.424 ns |  1,954.887 ns | 48.082 |    0.15 | 3.9063 |   80371 B |    1,255.80 |
| Lodestar_OneWayAnova           | 10000      |    42,818.35 ns |   9,592.312 ns |    525.787 ns |  1.417 |    0.02 |      - |      32 B |        0.50 |
| MetaNumerics_OneWayAnova       | 10000      |   196,938.35 ns |   3,796.811 ns |    208.116 ns |  6.516 |    0.02 |      - |     744 B |       11.62 |
| Lodestar_Wilcoxon              | 10000      |   111,481.55 ns |  17,607.426 ns |    965.122 ns |  3.689 |    0.03 | 4.7607 |   80056 B |    1,250.88 |
| MetaNumerics_Wilcoxon          | 10000      |   201,808.22 ns | 138,066.254 ns |  7,567.877 ns |  6.678 |    0.22 | 4.6387 |   80176 B |    1,252.75 |
| Lodestar_FisherExact           | 10000      |       260.96 ns |     172.023 ns |      9.429 ns |  0.009 |    0.00 |      - |         - |        0.00 |
| MetaNumerics_FisherExact       | 10000      |       906.99 ns |     221.628 ns |     12.148 ns |  0.030 |    0.00 | 0.0563 |     944 B |       14.75 |
| Lodestar_ChiSquare             | 10000      |        66.84 ns |       9.693 ns |      0.531 ns |  0.002 |    0.00 | 0.0100 |     168 B |        2.62 |
| MetaNumerics_ChiSquare         | 10000      |       414.80 ns |     201.263 ns |     11.032 ns |  0.014 |    0.00 | 0.0577 |     968 B |       15.12 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | GroupSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |-------------:|--------------:|-------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |   **1,001.7 ns** |     **188.33 ns** |     **10.32 ns** |  **1.00** |    **0.01** |  **0.3014** | **0.0019** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |   1,981.6 ns |   1,359.32 ns |     74.51 ns |  1.98 |    0.07 |  0.3242 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |     279.6 ns |     109.97 ns |      6.03 ns |  0.28 |    0.01 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     518.6 ns |      88.27 ns |      4.84 ns |  0.52 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |     463.1 ns |     108.03 ns |      5.92 ns |  0.46 |    0.01 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |   2,381.5 ns |     212.84 ns |     11.67 ns |  2.38 |    0.02 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |   4,940.5 ns |   2,646.05 ns |    145.04 ns |  4.93 |    0.13 |  0.0534 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |   3,604.8 ns |     460.55 ns |     25.24 ns |  3.60 |    0.04 |  0.0648 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |   2,837.1 ns |     679.41 ns |     37.24 ns |  2.83 |    0.04 |       - |      - |      48 B |       0.009 |
|                          |           |              |               |              |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     | **184,221.7 ns** | **195,437.48 ns** | **10,712.59 ns** | **1.002** |    **0.07** | **28.5645** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 869,594.0 ns | 271,441.08 ns | 14,878.60 ns | 4.731 |    0.24 | 28.3203 | 6.8359 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |  28,559.8 ns |   9,761.76 ns |    535.07 ns | 0.155 |    0.01 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |  55,284.6 ns |  22,515.81 ns |  1,234.17 ns | 0.301 |    0.02 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     608.1 ns |     444.63 ns |     24.37 ns | 0.003 |    0.00 |  0.0029 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     | 495,331.6 ns | 328,496.33 ns | 18,005.99 ns | 2.695 |    0.16 | 11.7188 | 2.9297 |  200216 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 937,823.0 ns | 474,562.95 ns | 26,012.40 ns | 5.102 |    0.28 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |           NA |            NA |           NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |   7,587.1 ns |     708.88 ns |     38.86 ns | 0.041 |    0.00 |       - |      - |      48 B |       0.000 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **203.6 μs** |    **28.43 μs** |   **1.56 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,254.1 μs** | **1,163.95 μs** |  **63.80 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **3,789.7 μs** |   **194.09 μs** |  **10.64 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **12,807.9 μs** | **5,021.07 μs** | **275.22 μs** | **281.2500** | **265.6250** | **250.0000** | **1735.44 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |--------------:|--------------:|------------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |      **8.650 μs** |     **0.6687 μs** |   **0.0367 μs** |  **1.00** |    **0.01** |    **0.9766** |    **0.0153** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |      7.910 μs |     1.5519 μs |   0.0851 μs |  0.91 |    0.01 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     12.075 μs |     6.2894 μs |   0.3447 μs |  1.40 |    0.03 |    1.6327 |    0.0763 |        - |   26.84 KB |        1.68 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **33.886 μs** |     **4.7055 μs** |   **0.2579 μs** |  **1.00** |    **0.01** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     12.268 μs |     0.9491 μs |   0.0520 μs |  0.36 |    0.00 |    3.7994 |    0.4120 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     24.004 μs |     3.4924 μs |   0.1914 μs |  0.71 |    0.01 |    3.0212 |    0.3357 |        - |   49.57 KB |        0.69 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **6,284.026 μs** |   **801.4958 μs** |  **43.9327 μs** |  **1.00** |    **0.01** |  **335.9375** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  6,486.788 μs | 6,029.2933 μs | 330.4859 μs |  1.03 |    0.05 |  335.9375 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  6,922.388 μs |   913.1291 μs |  50.0517 μs |  1.10 |    0.01 |  507.8125 |  500.0000 | 500.0000 | 2598.83 KB |        1.71 |
|                     |            |                 |               |               |             |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **10,651.490 μs** | **5,496.0068 μs** | **301.2547 μs** |  **1.00** |    **0.03** | **1125.0000** | **1109.3750** | **968.7500** | **6650.01 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             |  9,036.409 μs | 3,692.5193 μs | 202.3994 μs |  0.85 |    0.03 |  843.7500 |  828.1250 | 687.5000 | 5660.22 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 10,864.444 μs | 2,512.5796 μs | 137.7230 μs |  1.02 |    0.03 |  500.0000 |  500.0000 | 500.0000 | 4916.38 KB |        0.74 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error        | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,126.4 μs** |     **662.8 μs** |     **36.33 μs** |   **1.00** |    **0.04** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    52,541.3 μs |   1,608.8 μs |     88.18 μs |  46.68 |    1.28 |  3900.0000 |  1600.0000 |  900.0000 |  59881.36 KB |       58.56 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,141.4 μs** |     **170.6 μs** |      **9.35 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    56,782.3 μs |  29,760.3 μs |  1,631.26 μs |  49.75 |    1.29 |  5111.1111 |  1888.8889 |  888.8889 |   83111.6 KB |       81.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **398.5 μs** |     **196.4 μs** |     **10.77 μs** |   **1.00** |    **0.03** |     **1.4648** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |    75,533.0 μs |   8,716.0 μs |    477.76 μs | 189.63 |    4.58 |  4857.1429 |  1857.1429 |  857.1429 |  81289.81 KB |    2,678.88 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,435.2 μs** |     **129.9 μs** |      **7.12 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    40,731.3 μs |  36,232.6 μs |  1,986.03 μs |  28.38 |    1.20 |  3928.5714 |  1857.1429 |  928.5714 |  64191.23 KB |       62.78 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |     **8,240.7 μs** |     **991.5 μs** |     **54.35 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  |   857,194.8 μs |  64,577.4 μs |  3,539.71 μs | 104.02 |    0.70 | 35000.0000 | 10000.0000 | 4000.0000 | 537147.63 KB |       60.18 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |     **8,627.4 μs** |   **1,640.2 μs** |     **89.91 μs** |   **1.00** |    **0.01** |   **507.8125** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete |   988,936.3 μs | 540,270.6 μs | 29,614.05 μs | 114.64 |    3.15 | 48000.0000 | 12000.0000 | 4000.0000 | 747351.66 KB |       83.74 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **3,513.5 μs** |     **259.1 μs** |     **14.20 μs** |   **1.00** |    **0.00** |     **3.9063** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 1,822,690.4 μs | 788,600.0 μs | 43,225.83 μs | 518.77 |   10.81 | 47000.0000 | 12000.0000 | 4000.0000 |  732677.8 KB |    8,148.29 |
|                        |      |          |                |              |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **11,175.0 μs** |   **2,500.0 μs** |    **137.03 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     |   985,492.0 μs | 635,878.7 μs | 34,854.66 μs |  88.20 |    2.86 | 38000.0000 | 11000.0000 | 4000.0000 | 573626.15 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 14.68 ms | 25.89 ms | 1.419 ms |  875.0000 |  859.3750 | 453.1250 |  21.53 MB |
| CountLoadAsync4M | 30.56 ms | 21.92 ms | 1.202 ms | 1500.0000 | 1437.5000 | 687.5000 |  43.93 MB |
| CountLoad4M      | 30.84 ms | 11.01 ms | 0.604 ms | 1468.7500 | 1406.2500 | 656.2500 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Mean          | Error         | StdDev     | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-----------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 105,975.71 μs |  8,857.222 μs | 485.494 μs |  200.0000 |  200.0000 | 200.0000 | 54334.54 KB |
| CountSave240K         |     222.41 μs |     69.552 μs |   3.812 μs |   10.0098 |    3.6621 |   3.4180 |   504.67 KB |
| CountSaveAsync240K    |     248.43 μs |     77.875 μs |   4.269 μs |   11.2305 |    4.8828 |   4.8828 |   739.45 KB |
| TfidfSave45K          |      26.87 μs |     13.863 μs |   0.760 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      27.86 μs |      7.391 μs |   0.405 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     148.19 μs |    285.147 μs |  15.630 μs |    7.5684 |    1.4648 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     137.76 μs |     55.152 μs |   3.023 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  16,411.38 μs | 18,016.557 μs | 987.548 μs |  593.7500 |  562.5000 | 187.5000 | 22048.79 KB |
| CountLoadAsync4M      |  34,260.47 μs |  2,681.035 μs | 146.957 μs | 1187.5000 | 1125.0000 | 375.0000 | 44983.65 KB |
| CountLoad4M           |  33,274.67 μs | 17,334.927 μs | 950.186 μs | 1187.5000 | 1125.0000 | 375.0000 |  44977.3 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean          | Error        | StdDev      | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|----------:|----------:|---------:|------------:|
| TfidfSaveAsyncSmall    |      9.078 μs |     1.211 μs |   0.0664 μs |    1.6785 |    0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    323.296 μs |   382.142 μs |  20.9465 μs |   34.6680 |   28.3203 |  28.3203 |    740.1 KB |
| HashingLoadUndeclared  |      2.574 μs |     2.596 μs |   0.1423 μs |    5.1003 |         - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     15.478 μs |     2.709 μs |   0.1485 μs |    7.4158 |    0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |     75.450 μs |    57.522 μs |   3.1530 μs |   16.1133 |    3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    542.402 μs |   486.290 μs |  26.6552 μs |   38.0859 |   37.1094 |   8.7891 |   656.33 KB |
| CountLoadUndeclared1M  |  6,656.242 μs | 1,684.206 μs |  92.3170 μs |  554.6875 |  546.8750 | 335.9375 | 10793.97 KB |
| CountSaveAsync1M       |  1,299.949 μs |   674.885 μs |  36.9927 μs |  121.0938 |  113.2813 | 113.2813 |     3162 KB |
| IndexSaveAsyncLongHead |  1,944.243 μs |   720.088 μs |  39.4704 μs |   82.0313 |   78.1250 |  78.1250 |  2124.09 KB |
| CountLoadUndeclared2M  | 12,084.618 μs | 2,374.255 μs | 130.1410 μs | 1046.8750 | 1031.2500 | 625.0000 | 22045.19 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **2.929 μs** |  **0.4625 μs** | **0.0254 μs** |  **1.00** |    **0.01** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   3.208 μs |  2.4710 μs | 0.1354 μs |  1.10 |    0.04 |  0.1640 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   3.222 μs |  1.0502 μs | 0.0576 μs |  1.10 |    0.02 |  0.1640 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **30.746 μs** | **16.0466 μs** | **0.8796 μs** |  **1.00** |    **0.03** |  **1.8005** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  10.415 μs |  2.9280 μs | 0.1605 μs |  0.34 |    0.01 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  10.069 μs |  0.9579 μs | 0.0525 μs |  0.33 |    0.01 |  1.3580 | 0.0153 |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **116.983 μs** |  **5.7831 μs** | **0.3170 μs** |  **1.00** |    **0.00** |  **6.7139** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  37.987 μs |  6.4336 μs | 0.3526 μs |  0.32 |    0.00 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  35.374 μs | 34.8992 μs | 1.9129 μs |  0.30 |    0.01 |  4.2114 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **465.990 μs** | **79.9272 μs** | **4.3811 μs** |  **1.00** |    **0.01** | **26.8555** | **0.4883** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 150.041 μs | 16.3969 μs | 0.8988 μs |  0.32 |    0.00 | 20.0195 | 2.6855 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 132.734 μs | 17.6599 μs | 0.9680 μs |  0.28 |    0.00 | 15.8691 | 1.9531 | 262.16 KB |        0.59 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0     | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|---------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** |  **93.04 ms** |  **25.255 ms** | **1.384 ms** |  **1.00** |    **0.02** |        **-** |   **27.11 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  45.35 ms |   2.732 ms | 0.150 ms |  0.49 |    0.01 |        - |  103.61 KB |        3.82 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   |  **99.85 ms** |   **8.736 ms** | **0.479 ms** |  **1.00** |    **0.01** |        **-** |   **23.72 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  43.78 ms |  33.027 ms | 1.810 ms |  0.44 |    0.02 |        - |  116.39 KB |        4.91 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **141.33 ms** |  **14.819 ms** | **0.812 ms** |  **1.00** |    **0.01** |        **-** |   **103.3 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 171.61 ms | 120.973 ms | 6.631 ms |  1.21 |    0.04 |        - |     259 KB |        2.51 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **143.83 ms** |  **34.888 ms** | **1.912 ms** |  **1.00** |    **0.02** |        **-** |   **54.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 147.59 ms |  18.791 ms | 1.030 ms |  1.03 |    0.01 |        - |  192.25 KB |        3.53 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **182.00 ms** | **127.136 ms** | **6.969 ms** |  **1.00** |    **0.05** |        **-** |  **949.52 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 241.22 ms |  74.545 ms | 4.086 ms |  1.33 |    0.05 |        - |  1365.5 KB |        1.44 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **184.66 ms** | **168.532 ms** | **9.238 ms** |  **1.00** |    **0.06** |        **-** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 225.14 ms | 143.877 ms | 7.886 ms |  1.22 |    0.06 |        - | 1152.56 KB |        1.55 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **205.09 ms** |  **30.935 ms** | **1.696 ms** |  **1.00** |    **0.01** |        **-** | **5113.18 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 272.95 ms |  18.922 ms | 1.037 ms |  1.33 |    0.01 |        - | 7215.35 KB |        1.41 |
|                    |        |           |           |            |          |       |         |          |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **204.46 ms** |  **41.333 ms** | **2.266 ms** |  **1.00** |    **0.01** | **333.3333** | **5513.85 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 271.39 ms |  64.451 ms | 3.533 ms |  1.33 |    0.02 |        - | 7963.65 KB |        1.44 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **28.19 μs** |      **1.721 μs** |     **0.094 μs** |         **-** |
| Cjk    | 1000   |      34.64 μs |      8.473 μs |     0.464 μs |         - |
| **Latin**  | **10000**  |   **2,631.46 μs** |    **177.142 μs** |     **9.710 μs** |         **-** |
| Cjk    | 10000  |   3,509.45 μs |    447.228 μs |    24.514 μs |         - |
| **Latin**  | **65536**  | **122,706.17 μs** | **96,816.727 μs** | **5,306.852 μs** |         **-** |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean            | Error            | StdDev          | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |----------------:|-----------------:|----------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |        **968.5 ns** |         **86.59 ns** |         **4.75 ns** |     **1.00** |    **0.01** |    **0.0248** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |      1,495.9 ns |      1,237.51 ns |        67.83 ns |     1.54 |    0.06 |    0.0248 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |      1,944.8 ns |        228.29 ns |        12.51 ns |     2.01 |    0.01 |    0.3128 |    0.0019 |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |  2,364,235.9 ns |    380,473.69 ns |    20,855.05 ns | 2,441.19 |   21.34 |  917.9688 |  898.4375 |  886.7188 |  3722770 B |    8,780.12 |
| LuceneFromText         | 1000      |  4,735,821.0 ns |    269,991.19 ns |    14,799.13 ns | 4,889.97 |   24.64 |   85.9375 |   78.1250 |    7.8125 |  1375046 B |    3,243.03 |
|                        |           |                 |                  |                 |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |     **12,884.6 ns** |      **2,356.37 ns** |       **129.16 ns** |     **1.00** |    **0.01** |    **0.0153** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |     20,134.5 ns |      4,180.50 ns |       229.15 ns |     1.56 |    0.02 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |     10,927.3 ns |      5,413.82 ns |       296.75 ns |     0.85 |    0.02 |    0.5035 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 42,480,183.7 ns | 36,795,169.08 ns | 2,016,867.39 ns | 3,297.19 |  138.53 | 1846.1538 | 1769.2308 | 1769.2308 | 61664446 B |  145,435.01 |
| LuceneFromText         | 20000     | 83,510,138.4 ns |  5,359,089.19 ns |   293,749.76 ns | 6,481.81 |   59.34 | 1166.6667 | 1000.0000 |         - | 22416485 B |   52,869.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 14.65 ms | 17.03 ms | 0.934 ms |  1.00 |    0.08 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 49.34 ms | 18.13 ms | 0.994 ms |  3.38 |    0.19 | 1727.2727 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean       | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |-----------:|-----------:|----------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **8.661 μs** |   **4.280 μs** | **0.2346 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **18.985 μs** |   **5.473 μs** | **0.3000 μs** | **0.8850** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **37.812 μs** |   **3.227 μs** | **0.1769 μs** | **1.7090** | **0.0610** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **246.235 μs** | **128.636 μs** | **7.0510 μs** | **3.4180** |      **-** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| EncodeUnseenProse | 4.021 ms | 0.6090 ms | 0.0334 ms |    1.7 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean      | Error      | StdDev    | Allocated |
|----------- |--------- |----------:|-----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **9.342 μs** |  **0.7357 μs** | **0.0403 μs** |         **-** |
| MyersGroup | cjk      | 86.852 μs |  0.7454 μs | 0.0409 μs |         - |
| **DpGroup**    | **latin**    |  **5.231 μs** |  **0.6962 μs** | **0.0382 μs** |         **-** |
| MyersGroup | latin    | 71.397 μs | 21.0102 μs | 1.1516 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **168.30 μs** |  **17.595 μs** | **0.964 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 161.11 μs |  29.702 μs | 1.628 μs |  0.96 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **197.38 μs** | **104.504 μs** | **5.728 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 175.74 μs |  19.198 μs | 1.052 μs |  0.89 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **177.25 μs** | **119.900 μs** | **6.572 μs** |  **1.00** |    **0.05** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 167.63 μs |  19.710 μs | 1.080 μs |  0.95 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **152.58 μs** | **154.780 μs** | **8.484 μs** |  **1.00** |    **0.07** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 184.85 μs | 174.229 μs | 9.550 μs |  1.21 |    0.08 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **162.27 μs** |  **14.713 μs** | **0.806 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 163.46 μs |  42.382 μs | 2.323 μs |  1.01 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   |  **88.86 μs** |  **64.874 μs** | **3.556 μs** |  **1.00** |    **0.05** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 181.10 μs | 100.051 μs | 5.484 μs |  2.04 |    0.09 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **126.83 μs** |  **68.623 μs** | **3.761 μs** |  **1.00** |    **0.04** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 163.49 μs |  21.112 μs | 1.157 μs |  1.29 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **46.14 μs** |   **7.171 μs** | **0.393 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 177.44 μs |  18.016 μs | 0.988 μs |  3.85 |    0.03 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | Size | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |------------:|------------:|----------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |    **99.44 μs** |    **60.05 μs** |  **3.292 μs** |  **1.00** |    **0.04** |   **1.0986** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |    91.88 μs |    86.67 μs |  4.751 μs |  0.92 |    0.05 |   1.0986 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   248.26 μs |   209.09 μs | 11.461 μs |  2.50 |    0.12 |  18.3105 |       - |       - |  301.95 KB |       15.44 |
|                               |      |             |             |           |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **2,152.50 μs** |   **254.57 μs** | **13.954 μs** |  **1.00** |    **0.01** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 1,927.56 μs |   709.77 μs | 38.905 μs |  0.90 |    0.02 |  99.6094 | 99.6094 | 99.6094 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 4,106.69 μs | 1,111.41 μs | 60.920 μs |  1.91 |    0.03 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|-------------:|----------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **103.21 ms** |    **27.660 ms** |  **1.516 ms** |  **1.00** |    **0.02** |  **1800.0000** | **1800.0000** | **1800.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 1,242.52 ms |   711.131 ms | 38.980 ms | 12.04 |    0.36 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.31 |
|              |           |             |              |           |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **33.81 ms** |     **7.317 ms** |  **0.401 ms** |  **1.00** |    **0.01** |  **1466.6667** | **1466.6667** | **1466.6667** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   485.64 ms | 1,389.688 ms | 76.173 ms | 14.37 |    1.96 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **108.03 ms** |  **38.719 ms** |  **2.122 ms** |  **1.00** |    **0.02** |  **1200.0000** |  **1200.0000** |  **1200.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 3,736.84 ms | 389.149 ms | 21.331 ms | 34.60 |    0.61 | 38000.0000 | 14000.0000 | 11000.0000 | 531.88 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 2,150.62 ms | 980.024 ms | 53.718 ms | 19.91 |    0.55 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **12.75 ms** |   **2.858 ms** |  **0.157 ms** |  **1.00** |    **0.02** |  **1343.7500** |  **1343.7500** |  **1343.7500** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   254.80 ms |  92.852 ms |  5.090 ms | 19.99 |    0.41 |  5000.0000 |  3500.0000 |  3000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   177.62 ms |  18.693 ms |  1.025 ms | 13.94 |    0.16 |  3750.0000 |  3750.0000 |  3750.0000 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean      | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  7.289 ms |  0.5765 ms | 0.0316 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 65.951 ms | 74.8307 ms | 4.1017 ms |  9.05 |    0.49 |
| Nmf_Transform                             |  1.806 ms |  0.5768 ms | 0.0316 ms |  0.25 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 12.284 ms |  5.9101 ms | 0.3240 ms |  1.69 |    0.04 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  70.64 ns |  11.83 ns |  0.649 ns |  1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   | 434.02 ns |  53.24 ns |  2.918 ns |  6.14 |    0.06 |      - |         - |          NA |
| TokenSortRatio | 460.11 ns | 259.78 ns | 14.239 ns |  6.51 |    0.18 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 515.85 ns | 129.60 ns |  7.104 ns |  7.30 |    0.10 | 0.0534 |     896 B |          NA |
| WRatio         | 665.98 ns | 577.64 ns | 31.662 ns |  9.43 |    0.40 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Mean               | Error              | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|-------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,218,346.8776 ns |    124,726.5870 ns |   6,836.6851 ns |  1.000 |    0.01 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    343,249.5693 ns |    136,181.5472 ns |   7,464.5702 ns |  0.282 |    0.01 | 35.6445 | 13.6719 |  602664 B |        3.75 |
| WRatio_ProseAndOneEmoji   |    708,464.1951 ns |    130,585.7989 ns |   7,157.8483 ns |  0.582 |    0.01 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 52,560,395.6000 ns | 13,441,046.2911 ns | 736,749.1050 ns | 43.142 |    0.56 |       - |       - |  510776 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |          0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **62.72 ns** |     **2.667 ns** |  **0.146 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   131.31 ns |    46.067 ns |  2.525 ns |  2.09 |    0.04 | 0.0048 |      80 B |          NA |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **428.56 ns** |    **37.811 ns** |  **2.073 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 5,440.45 ns | 1,666.881 ns | 91.367 ns | 12.69 |    0.19 | 0.0076 |     160 B |          NA |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **563.54 ns** |   **316.024 ns** | **17.322 ns** |  **1.00** |    **0.04** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,077.06 ns |   508.788 ns | 27.888 ns |  1.91 |    0.07 | 0.1144 |    1944 B |        2.17 |
|            |               |             |              |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **647.47 ns** |   **227.676 ns** | **12.480 ns** |  **1.00** |    **0.02** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 2,746.35 ns |   432.705 ns | 23.718 ns |  4.24 |    0.08 | 0.1831 |    3096 B |        2.76 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **14.89 ns** |     **4.849 ns** |  **0.266 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    19.08 ns |     2.158 ns |  0.118 ns |  1.28 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    14.93 ns |     1.229 ns |  0.067 ns |  1.00 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    14.66 ns |    15.263 ns |  0.837 ns |  0.98 |    0.05 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **15.44 ns** |    **11.948 ns** |  **0.655 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    20.07 ns |     7.085 ns |  0.388 ns |  1.30 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    15.37 ns |     1.176 ns |  0.064 ns |  1.00 |    0.04 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    14.55 ns |     0.939 ns |  0.051 ns |  0.94 |    0.03 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **17.75 ns** |    **22.853 ns** |  **1.253 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    21.37 ns |     4.671 ns |  0.256 ns |  1.21 |    0.08 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    16.52 ns |     1.081 ns |  0.059 ns |  0.93 |    0.06 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    16.11 ns |     0.984 ns |  0.054 ns |  0.91 |    0.06 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **21.48 ns** |    **12.475 ns** |  **0.684 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    24.53 ns |     9.397 ns |  0.515 ns |  1.14 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    23.83 ns |     5.768 ns |  0.316 ns |  1.11 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    20.70 ns |     5.858 ns |  0.321 ns |  0.96 |    0.03 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **32.42 ns** |     **3.573 ns** |  **0.196 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    35.98 ns |     5.412 ns |  0.297 ns |  1.11 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    34.20 ns |     4.329 ns |  0.237 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    31.37 ns |     2.507 ns |  0.137 ns |  0.97 |    0.01 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **37.79 ns** |     **7.274 ns** |  **0.399 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    41.57 ns |     7.965 ns |  0.437 ns |  1.10 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    39.86 ns |     7.081 ns |  0.388 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    37.45 ns |    14.467 ns |  0.793 ns |  0.99 |    0.02 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **257.33 ns** |    **59.066 ns** |  **3.238 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   264.98 ns |    88.032 ns |  4.825 ns |  1.03 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   265.92 ns |   140.488 ns |  7.701 ns |  1.03 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   264.52 ns |    95.576 ns |  5.239 ns |  1.03 |    0.02 |         - |          NA |
|                            |        |             |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **3,362.98 ns** | **1,186.543 ns** | **65.038 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 3,190.48 ns |   378.606 ns | 20.753 ns |  0.95 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 3,039.12 ns |   262.890 ns | 14.410 ns |  0.90 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,187.33 ns |   489.105 ns | 26.809 ns |  0.95 |    0.02 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error       | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **238.6 ns** |   **152.71 ns** |  **8.37 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    106.8 ns |    69.99 ns |  3.84 ns |  0.45 |    0.02 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **128**    |  **1,449.1 ns** |   **686.71 ns** | **37.64 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  1,932.7 ns | 1,061.42 ns | 58.18 ns |  1.33 |    0.05 |         - |          NA |
|                    |        |             |             |          |       |         |           |             |
| **Distance_CodePoint** | **512**    |  **8,214.0 ns** |   **736.36 ns** | **40.36 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 19,785.3 ns |   729.35 ns | 39.98 ns |  2.41 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Corpus       | Length | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |-----------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      | **4,154.6 ns** | **2,857.84 ns** | **156.65 ns** |  **1.00** |    **0.05** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      | 4,766.0 ns | 5,366.85 ns | 294.18 ns |  1.15 |    0.07 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 5,842.3 ns | 3,990.37 ns | 218.73 ns |  1.41 |    0.06 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     | **5,020.5 ns** |   **607.89 ns** |  **33.32 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     | 4,678.5 ns |   720.97 ns |  39.52 ns |  0.93 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 7,574.6 ns | 3,743.93 ns | 205.22 ns |  1.51 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **6,322.9 ns** | **2,446.91 ns** | **134.12 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 5,963.3 ns | 4,163.23 ns | 228.20 ns |  0.94 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 9,943.5 ns | 3,266.04 ns | 179.02 ns |  1.57 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      | **2,710.3 ns** |   **292.62 ns** |  **16.04 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      | 4,325.8 ns | 1,817.64 ns |  99.63 ns |  1.60 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      | 4,689.6 ns |   126.27 ns |   6.92 ns |  1.73 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     | **3,843.8 ns** | **1,945.22 ns** | **106.62 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     | 5,215.7 ns |   169.88 ns |   9.31 ns |  1.36 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 6,300.2 ns | 1,177.13 ns |  64.52 ns |  1.64 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **5,182.5 ns** | **2,047.49 ns** | **112.23 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     | 6,587.5 ns | 2,953.09 ns | 161.87 ns |  1.27 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 8,560.8 ns | 1,992.76 ns | 109.23 ns |  1.65 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      | **1,959.3 ns** |   **254.32 ns** |  **13.94 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      | 2,728.5 ns |   377.00 ns |  20.66 ns |  1.39 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      | 3,743.1 ns |   443.49 ns |  24.31 ns |  1.91 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     | **2,999.2 ns** |   **548.34 ns** |  **30.06 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     | 3,455.0 ns | 2,566.49 ns | 140.68 ns |  1.15 |    0.04 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 5,663.6 ns | 2,394.29 ns | 131.24 ns |  1.89 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     | **4,415.4 ns** | **2,168.93 ns** | **118.89 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     | 5,076.7 ns |   574.27 ns |  31.48 ns |  1.15 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 7,701.0 ns |   722.42 ns |  39.60 ns |  1.74 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      | **1,212.9 ns** |   **250.32 ns** |  **13.72 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      | 1,271.2 ns |   375.51 ns |  20.58 ns |  1.05 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      | 3,165.0 ns | 2,274.51 ns | 124.67 ns |  2.61 |    0.09 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     | **2,359.8 ns** |   **546.34 ns** |  **29.95 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     | 1,758.5 ns |   285.81 ns |  15.67 ns |  0.75 |    0.01 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     | 4,805.2 ns |   110.66 ns |   6.07 ns |  2.04 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     | **3,661.4 ns** |   **836.68 ns** |  **45.86 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     | 2,453.3 ns |   530.95 ns |  29.10 ns |  0.67 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 7,355.6 ns | 5,412.50 ns | 296.68 ns |  2.01 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      | **1,953.2 ns** | **1,165.59 ns** |  **63.89 ns** |  **1.00** |    **0.04** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      | 2,177.0 ns |   543.75 ns |  29.80 ns |  1.12 |    0.03 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      | 3,735.7 ns | 2,013.95 ns | 110.39 ns |  1.91 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     | **2,917.4 ns** |   **302.07 ns** |  **16.56 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     | 2,673.3 ns |   496.78 ns |  27.23 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 5,336.8 ns |   271.93 ns |  14.91 ns |  1.83 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     | **4,394.4 ns** |   **173.75 ns** |   **9.52 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     | 4,306.7 ns |   395.96 ns |  21.70 ns |  0.98 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 7,508.4 ns | 1,065.50 ns |  58.40 ns |  1.71 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |   **819.0 ns** |   **308.47 ns** |  **16.91 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |   328.0 ns |    54.82 ns |   3.00 ns |  0.40 |    0.01 | 0.0024 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      | 2,769.6 ns |   745.34 ns |  40.85 ns |  3.38 |    0.07 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     | **1,766.8 ns** |   **211.33 ns** |  **11.58 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |   779.4 ns |    77.76 ns |   4.26 ns |  0.44 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     | 4,617.5 ns | 3,865.62 ns | 211.89 ns |  2.61 |    0.10 | 0.3052 |    5120 B |          NA |
|                      |              |        |            |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     | **3,151.3 ns** | **1,504.18 ns** |  **82.45 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     | 1,495.5 ns |   867.66 ns |  47.56 ns |  0.47 |    0.02 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 6,749.3 ns | 3,267.37 ns | 179.10 ns |  2.14 |    0.07 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **2,944.5 μs** |    **608.88 μs** |    **33.37 μs** |  **1.00** |    **0.01** | **257.8125** | **246.0938** | **246.0938** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  19,565.6 μs |  6,102.05 μs |   334.47 μs |  6.65 |    0.12 |        - |        - |        - |   10.33 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  48,406.6 μs |  1,966.95 μs |   107.82 μs | 16.44 |    0.16 |        - |        - |        - |   41.84 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **429.5 μs** |     **83.95 μs** |     **4.60 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   2,571.3 μs |    241.14 μs |    13.22 μs |  5.99 |    0.06 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   1,521.8 μs |    549.05 μs |    30.10 μs |  3.54 |    0.07 |   1.9531 |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **13,253.3 μs** |  **2,282.40 μs** |   **125.11 μs** |  **1.00** |    **0.01** | **375.0000** | **375.0000** | **375.0000** | **4309.04 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 232,758.3 μs | 26,595.91 μs | 1,457.81 μs | 17.56 |    0.17 |        - |        - |        - |   15.06 KB |       0.003 |
| MetaNumerics_Fit | 50000x8x32  | 364,453.3 μs | 31,860.59 μs | 1,746.39 μs | 27.50 |    0.25 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **2.647 ms** |  **0.3163 ms** | **0.0173 ms** |  **1.00** |    **0.01** | **253.9063** | **246.0938** | **246.0938** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |   8.248 ms |  0.9398 ms | 0.0515 ms |  3.12 |    0.02 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **13.471 ms** |  **0.7811 ms** | **0.0428 ms** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  62.798 ms | 11.9878 ms | 0.6571 ms |  4.66 |    0.04 |        - |        - |        - |   96.38 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **24.570 ms** |  **4.3112 ms** | **0.2363 ms** |  **1.00** |    **0.01** | **343.7500** | **343.7500** | **343.7500** | **3947.64 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 104.303 ms |  3.4599 ms | 0.1897 ms |  4.25 |    0.04 |        - |        - |        - |   36.34 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **65.76 ns** |     **66.104 ns** |     **3.623 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 8    |    31.73 ns |      4.520 ns |     0.248 ns |  0.48 |    0.02 |         - |          NA |
| Dp_Cjk     | 8    |    63.57 ns |      4.993 ns |     0.274 ns |  0.97 |    0.05 |         - |          NA |
| Kernel_Cjk | 8    |    52.95 ns |      3.618 ns |     0.198 ns |  0.81 |    0.04 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **12**   |   **134.30 ns** |    **213.115 ns** |    **11.682 ns** |  **1.00** |    **0.11** |         **-** |          **NA** |
| Kernel     | 12   |    36.97 ns |     21.330 ns |     1.169 ns |  0.28 |    0.02 |         - |          NA |
| Dp_Cjk     | 12   |   121.39 ns |     34.746 ns |     1.905 ns |  0.91 |    0.07 |         - |          NA |
| Kernel_Cjk | 12   |    65.83 ns |      7.569 ns |     0.415 ns |  0.49 |    0.04 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **14**   |   **147.04 ns** |     **31.709 ns** |     **1.738 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |    38.58 ns |     11.071 ns |     0.607 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |   147.32 ns |     92.529 ns |     5.072 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 14   |    62.03 ns |      8.503 ns |     0.466 ns |  0.42 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **16**   |   **444.32 ns** |    **264.537 ns** |    **14.500 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 16   |    41.32 ns |     13.047 ns |     0.715 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   426.28 ns |    583.959 ns |    32.009 ns |  0.96 |    0.07 |         - |          NA |
| Kernel_Cjk | 16   |    67.26 ns |     29.434 ns |     1.613 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **18**   |   **437.34 ns** |    **290.705 ns** |    **15.935 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 18   |    44.14 ns |     23.640 ns |     1.296 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |   435.21 ns |    168.631 ns |     9.243 ns |  1.00 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |    67.86 ns |      6.344 ns |     0.348 ns |  0.16 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **20**   |   **498.00 ns** |     **38.379 ns** |     **2.104 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |    47.91 ns |      7.565 ns |     0.415 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   497.12 ns |    129.331 ns |     7.089 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    72.61 ns |     62.624 ns |     3.433 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **24**   |   **787.94 ns** |     **24.262 ns** |     **1.330 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    52.66 ns |     11.483 ns |     0.629 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   644.56 ns |    280.912 ns |    15.398 ns |  0.82 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    80.05 ns |     37.190 ns |     2.039 ns |  0.10 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **32**   |   **989.39 ns** |     **96.433 ns** |     **5.286 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    64.05 ns |      4.759 ns |     0.261 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |   932.50 ns |    117.990 ns |     6.467 ns |  0.94 |    0.01 |         - |          NA |
| Kernel_Cjk | 32   |    92.07 ns |      8.265 ns |     0.453 ns |  0.09 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **48**   | **1,950.15 ns** |    **721.644 ns** |    **39.556 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 48   |    78.45 ns |     36.547 ns |     2.003 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 1,871.26 ns |    300.587 ns |    16.476 ns |  0.96 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |   132.72 ns |     15.630 ns |     0.857 ns |  0.07 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **64**   | **2,894.75 ns** |  **1,157.933 ns** |    **63.470 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 64   |   151.89 ns |    355.970 ns |    19.512 ns |  0.05 |    0.01 |         - |          NA |
| Dp_Cjk     | 64   | 2,921.56 ns |  1,333.588 ns |    73.098 ns |  1.01 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |   155.36 ns |     18.495 ns |     1.014 ns |  0.05 |    0.00 |         - |          NA |
|            |      |             |               |              |       |         |           |             |
| **Dp**         | **96**   | **6,631.53 ns** |    **731.813 ns** |    **40.113 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   232.88 ns |     70.914 ns |     3.887 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 7,688.86 ns | 37,186.348 ns | 2,038.309 ns |  1.16 |    0.27 |         - |          NA |
| Kernel_Cjk | 96   |   588.66 ns |    360.317 ns |    19.750 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **14.36 ns** |     **1.877 ns** |   **0.103 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     14.18 ns |     2.903 ns |   0.159 ns |  0.99 |    0.01 |         - |          NA |
| Distance_CodePoint         | 8      |     93.80 ns |    25.790 ns |   1.414 ns |  6.53 |    0.09 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     14.58 ns |     1.297 ns |   0.071 ns |  1.02 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **208.75 ns** |     **8.259 ns** |   **0.453 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    236.55 ns |    28.325 ns |   1.553 ns |  1.13 |    0.01 |         - |          NA |
| Distance_CodePoint         | 64     |    440.46 ns |    45.919 ns |   2.517 ns |  2.11 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    206.67 ns |    28.446 ns |   1.559 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **587.75 ns** |   **277.943 ns** |  **15.235 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,194.15 ns |   819.356 ns |  44.912 ns |  2.03 |    0.08 |         - |          NA |
| Distance_CodePoint         | 128    |    993.26 ns |    53.617 ns |   2.939 ns |  1.69 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    578.38 ns |    23.183 ns |   1.271 ns |  0.98 |    0.02 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    |  **7,534.69 ns** | **3,463.635 ns** | **189.854 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 10,635.53 ns |   770.871 ns |  42.254 ns |  1.41 |    0.03 |         - |          NA |
| Distance_CodePoint         | 512    |  8,834.24 ns | 1,960.874 ns | 107.482 ns |  1.17 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  7,414.32 ns | 4,968.016 ns | 272.314 ns |  0.98 |    0.04 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **217.6 ns** |     **46.03 ns** |     **2.52 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     146.1 ns |     19.39 ns |     1.06 ns |  0.67 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **216.5 ns** |     **17.61 ns** |     **0.97 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     150.3 ns |     61.02 ns |     3.34 ns |  0.69 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **267.3 ns** |     **40.18 ns** |     **2.20 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     201.4 ns |      7.88 ns |     0.43 ns |  0.75 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **267.8 ns** |     **78.54 ns** |     **4.31 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     201.5 ns |      7.74 ns |     0.42 ns |  0.75 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **319.2 ns** |     **28.74 ns** |     **1.58 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     263.1 ns |     50.71 ns |     2.78 ns |  0.82 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **319.5 ns** |    **176.07 ns** |     **9.65 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     263.7 ns |    141.08 ns |     7.73 ns |  0.83 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **372.1 ns** |     **29.40 ns** |     **1.61 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |     713.5 ns |    110.40 ns |     6.05 ns |  1.92 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **375.6 ns** |     **53.98 ns** |     **2.96 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |     774.9 ns |     65.70 ns |     3.60 ns |  2.06 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,256.8 ns** |    **520.50 ns** |    **28.53 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   3,581.4 ns |  1,020.17 ns |    55.92 ns |  2.85 |    0.07 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,256.6 ns** |    **430.72 ns** |    **23.61 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   3,638.7 ns |  1,107.50 ns |    60.71 ns |  2.90 |    0.06 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |   **9,580.2 ns** |    **908.76 ns** |    **49.81 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  37,389.2 ns |  2,809.49 ns |   154.00 ns |  3.90 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **325,335.2 ns** | **90,220.89 ns** | **4,945.31 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  38,287.4 ns |  4,301.69 ns |   235.79 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **14.13 ns** |       **0.581 ns** |      **0.032 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      52.23 ns |      17.285 ns |      0.947 ns |  3.70 |    0.06 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      47.98 ns |       5.650 ns |      0.310 ns |  3.40 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |      93.92 ns |      38.006 ns |      2.083 ns |  6.65 |    0.13 | 0.0076 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **204.19 ns** |      **18.666 ns** |      **1.023 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   3,253.25 ns |   3,567.401 ns |    195.541 ns | 15.93 |    0.83 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |     738.88 ns |     195.362 ns |     10.708 ns |  3.62 |    0.05 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   5,811.43 ns |   1,875.537 ns |    102.804 ns | 28.46 |    0.45 | 0.0305 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |   **7,190.58 ns** |     **652.280 ns** |     **35.754 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 239,314.94 ns | 215,577.489 ns | 11,816.530 ns | 33.28 |    1.43 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  21,080.09 ns |  24,159.398 ns |  1,324.258 ns |  2.93 |    0.16 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 394,680.33 ns |  84,123.863 ns |  4,611.113 ns | 54.89 |    0.60 |      - |    4160 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **269.3 μs** |  **31.44 μs** |  **1.72 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 271.2 μs |  19.68 μs |  1.08 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 296.4 μs |  37.31 μs |  2.05 μs |  9.2773 |  2.9297 | 153.98 KB |
| **F1Macro**           | **5000**   | **348.9 μs** | **216.33 μs** | **11.86 μs** | **20.5078** |  **6.3477** | **337.24 KB** |
| PrecisionWeighted | 5000   | 361.5 μs |  29.94 μs |  1.64 μs | 20.5078 |  6.3477 | 337.24 KB |
| ReportRows        | 5000   | 504.5 μs | 158.05 μs |  8.66 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |      **81.19 μs** |     **51.349 μs** |     **2.815 μs** |   **1.00** |    **0.04** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  53,105.53 μs | 22,286.712 μs | 1,221.610 μs | 654.59 |   23.28 | 900.0000 | 900.0000 | 900.0000 | 31413.68 KB |   12,805.58 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **1,723.47 μs** |    **243.002 μs** |    **13.320 μs** |   **1.00** |    **0.01** |   **1.9531** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 262,421.47 μs | 97,048.736 μs | 5,319.569 μs | 152.27 |    2.86 | 500.0000 | 500.0000 | 500.0000 | 32058.39 KB |      760.73 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **13.55 μs** |      **1.381 μs** |     **0.076 μs** |   **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     484.78 μs |    105.941 μs |     5.807 μs |  35.77 |    0.41 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Gen1   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **2,435.4 ns** |       **192.27 ns** |      **10.54 ns** | **0.0229** |      **-** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     4,098.5 ns |       191.40 ns |      10.49 ns | 0.0229 |      - |     400 B |
| AccuracyScore  | 1000    | 2       |       119.4 ns |        10.95 ns |       0.60 ns |      - |      - |         - |
| F1Macro        | 1000    | 2       |     2,577.0 ns |     1,471.00 ns |      80.63 ns | 0.0381 |      - |     656 B |
| Report         | 1000    | 2       |     3,817.8 ns |       867.52 ns |      47.55 ns | 0.3204 |      - |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **2,221.2 ns** |       **423.52 ns** |      **23.21 ns** | **0.0763** |      **-** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     4,392.8 ns |     1,066.44 ns |      58.46 ns | 0.0763 |      - |    1336 B |
| AccuracyScore  | 1000    | 10      |       121.9 ns |        11.87 ns |       0.65 ns |      - |      - |         - |
| F1Macro        | 1000    | 10      |     2,465.7 ns |     1,856.30 ns |     101.75 ns | 0.1106 |      - |    1880 B |
| Report         | 1000    | 10      |     5,908.2 ns |       360.69 ns |      19.77 ns | 0.7401 | 0.0076 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **233,750.3 ns** |   **128,668.12 ns** |   **7,052.73 ns** |      **-** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   412,239.4 ns |   172,590.70 ns |   9,460.28 ns |      - |      - |     400 B |
| AccuracyScore  | 100000  | 2       |    11,902.0 ns |       991.59 ns |      54.35 ns |      - |      - |         - |
| F1Macro        | 100000  | 2       |   231,372.7 ns |    17,549.69 ns |     961.96 ns |      - |      - |     656 B |
| Report         | 100000  | 2       |   238,731.4 ns |   121,332.92 ns |   6,650.67 ns | 0.2441 |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **200,914.4 ns** |   **152,067.15 ns** |   **8,335.31 ns** |      **-** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   542,695.8 ns |   100,257.63 ns |   5,495.46 ns |      - |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |     6,218.8 ns |       829.61 ns |      45.47 ns |      - |      - |         - |
| F1Macro        | 100000  | 10      |   200,269.7 ns |    93,198.96 ns |   5,108.55 ns |      - |      - |    1880 B |
| Report         | 100000  | 10      |   198,795.8 ns |    29,503.24 ns |   1,617.17 ns | 0.7324 |      - |   12832 B |
| **Matrix**         | **1000000** | **2**       | **2,344,515.9 ns** |   **209,534.89 ns** |  **11,485.31 ns** |      **-** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 4,772,989.2 ns |   588,064.10 ns |  32,233.78 ns |      - |      - |     406 B |
| AccuracyScore  | 1000000 | 2       |    83,466.0 ns |    65,498.65 ns |   3,590.20 ns |      - |      - |         - |
| F1Macro        | 1000000 | 2       | 2,366,036.3 ns |   627,751.71 ns |  34,409.19 ns |      - |      - |     659 B |
| Report         | 1000000 | 2       | 2,372,954.9 ns |   348,671.47 ns |  19,111.86 ns |      - |      - |    5507 B |
| **Matrix**         | **1000000** | **10**      | **1,886,587.6 ns** |   **426,980.28 ns** |  **23,404.23 ns** |      **-** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 5,758,749.4 ns | 1,722,002.39 ns |  94,388.76 ns |      - |      - |    1342 B |
| AccuracyScore  | 1000000 | 10      |    68,462.0 ns |     4,803.19 ns |     263.28 ns |      - |      - |         - |
| F1Macro        | 1000000 | 10      | 2,072,345.9 ns | 2,000,118.30 ns | 109,633.23 ns |      - |      - |    1883 B |
| Report         | 1000000 | 10      | 2,189,416.1 ns | 1,554,754.38 ns |  85,221.33 ns |      - |      - |   12875 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |---------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **5,214.103 μs** |  **1,018.1966 μs** |    **55.8108 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1280 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  25,041.613 μs |  4,844.7353 μs |   265.5563 μs |     4.80 |    0.06 | 593.7500 | 593.7500 | 593.7500 |  5090172 B |    3,976.70 |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |       **6.253 μs** |      **0.5507 μs** |     **0.0302 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  25,036.423 μs |  8,677.2769 μs |   475.6308 μs | 4,003.72 |   67.95 | 593.7500 | 593.7500 | 593.7500 |  5089534 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **105,987.700 μs** | **30,739.2969 μs** | **1,684.9246 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 151,242.237 μs | 18,771.8945 μs | 1,028.9509 μs |     1.43 |    0.02 | 250.0000 | 250.0000 | 250.0000 | 23229826 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |      **69.147 μs** |     **25.5638 μs** |     **1.4012 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 149,934.051 μs | 47,036.3950 μs | 2,578.2235 μs | 2,168.92 |   50.01 | 250.0000 | 250.0000 | 250.0000 | 23229826 B |          NA |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **41.14 ns** |     **9.278 ns** |   **0.509 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 4    |    43.69 ns |    15.061 ns |   0.826 ns |  1.06 |    0.02 |         - |          NA |
| Dp_Cjk     | 4    |    42.73 ns |    15.999 ns |   0.877 ns |  1.04 |    0.02 |         - |          NA |
| Kernel_Cjk | 4    |    41.16 ns |     4.940 ns |   0.271 ns |  1.00 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **6**    |    **60.00 ns** |    **23.186 ns** |   **1.271 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 6    |    41.40 ns |     2.478 ns |   0.136 ns |  0.69 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    58.32 ns |    10.785 ns |   0.591 ns |  0.97 |    0.02 |         - |          NA |
| Kernel_Cjk | 6    |    71.15 ns |     8.193 ns |   0.449 ns |  1.19 |    0.02 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **8**    |    **77.42 ns** |     **7.232 ns** |   **0.396 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |    50.69 ns |     7.167 ns |   0.393 ns |  0.65 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    80.71 ns |     6.291 ns |   0.345 ns |  1.04 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    90.58 ns |     7.495 ns |   0.411 ns |  1.17 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **10**   |   **105.12 ns** |    **20.601 ns** |   **1.129 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |    54.07 ns |     1.320 ns |   0.072 ns |  0.51 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |   118.39 ns |    84.668 ns |   4.641 ns |  1.13 |    0.04 |         - |          NA |
| Kernel_Cjk | 10   |    78.02 ns |     6.369 ns |   0.349 ns |  0.74 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **167.76 ns** |    **13.517 ns** |   **0.741 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    63.21 ns |     9.693 ns |   0.531 ns |  0.38 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   168.12 ns |     5.833 ns |   0.320 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    85.67 ns |    11.170 ns |   0.612 ns |  0.51 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **237.53 ns** |    **31.112 ns** |   **1.705 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    75.82 ns |     5.722 ns |   0.314 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   232.12 ns |    19.976 ns |   1.095 ns |  0.98 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    99.11 ns |     8.397 ns |   0.460 ns |  0.42 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **474.62 ns** |   **120.058 ns** |   **6.581 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |    99.47 ns |     8.557 ns |   0.469 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   479.18 ns |    14.398 ns |   0.789 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |   131.74 ns |    73.128 ns |   4.008 ns |  0.28 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,140.45 ns** | **1,774.045 ns** |  **97.241 ns** |  **1.01** |    **0.11** |         **-** |          **NA** |
| Kernel     | 32   |   128.11 ns |     7.372 ns |   0.404 ns |  0.11 |    0.01 |         - |          NA |
| Dp_Cjk     | 32   | 1,084.79 ns |   608.780 ns |  33.369 ns |  0.96 |    0.08 |         - |          NA |
| Kernel_Cjk | 32   |   164.56 ns |    17.321 ns |   0.949 ns |  0.15 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **1,927.35 ns** |   **405.738 ns** |  **22.240 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |   178.41 ns |    30.082 ns |   1.649 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 1,994.19 ns | 2,862.678 ns | 156.913 ns |  1.03 |    0.07 |         - |          NA |
| Kernel_Cjk | 48   |   218.47 ns |    60.396 ns |   3.310 ns |  0.11 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **3,254.51 ns** |   **247.015 ns** |  **13.540 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |   233.29 ns |    63.943 ns |   3.505 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 3,291.01 ns |   229.521 ns |  12.581 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |   272.17 ns |    35.158 ns |   1.927 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **7,550.84 ns** | **1,302.789 ns** |  **71.410 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   455.12 ns |    79.185 ns |   4.340 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 7,638.21 ns | 2,796.261 ns | 153.273 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |   893.64 ns |   163.543 ns |   8.964 ns |  0.12 |    0.00 |         - |          NA |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **145.45 μs** |  **14.236 μs** |  **0.780 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   151.72 μs |  33.899 μs |  1.858 μs |  1.04 |    0.01 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    32.71 μs |   4.840 μs |  0.265 μs |  0.22 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   162.26 μs |   7.229 μs |  0.396 μs |  1.12 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **146.25 μs** |  **26.644 μs** |  **1.460 μs** |  **1.00** |    **0.01** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   167.42 μs |  32.769 μs |  1.796 μs |  1.14 |    0.01 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    32.92 μs |   3.358 μs |  0.184 μs |  0.23 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   153.06 μs |  20.560 μs |  1.127 μs |  1.05 |    0.01 |      - |     800 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,451.99 μs** |  **22.699 μs** |  **1.244 μs** |  **1.00** |    **0.00** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 1,501.07 μs | 252.545 μs | 13.843 μs |  1.03 |    0.01 |      - |    7785 B |        9.72 |
| SparseFit             | 100000   | 10         |   830.96 μs | 109.757 μs |  6.016 μs |  0.57 |    0.00 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,591.87 μs | 207.846 μs | 11.393 μs |  1.10 |    0.01 |      - |     801 B |        1.00 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,462.09 μs** | **654.152 μs** | **35.856 μs** |  **1.00** |    **0.03** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 1,519.11 μs | 164.444 μs |  9.014 μs |  1.04 |    0.02 | 3.9063 |   77625 B |       96.91 |
| SparseFit             | 100000   | 100        |   794.56 μs |  72.157 μs |  3.955 μs |  0.54 |    0.01 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,543.76 μs |  45.893 μs |  2.516 μs |  1.06 |    0.02 |      - |     801 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean       | Error       | StdDev     | Allocated |
|------------ |------------- |-----------:|------------:|-----------:|----------:|
| **Embedded**    | **65**           |   **6.261 μs** |   **0.6407 μs** |  **0.0351 μs** |         **-** |
| EqualLength | 65           |   1.617 μs |   0.1954 μs |  0.0107 μs |         - |
| **Embedded**    | **128**          |  **24.001 μs** |  **10.6158 μs** |  **0.5819 μs** |         **-** |
| EqualLength | 128          |   3.166 μs |   0.4942 μs |  0.0271 μs |         - |
| **Embedded**    | **512**          | **869.329 μs** | **265.1636 μs** | **14.5345 μs** |         **-** |
| EqualLength | 512          |  26.164 μs |  14.7456 μs |  0.8083 μs |         - |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean        | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |------------:|-------------:|------------:|---------:|---------:|---------:|-----------:|
| VocabTxt               |  2,799.5 μs |   5,658.7 μs |   310.17 μs | 117.1875 | 113.2813 |  35.1563 | 3711.63 KB |
| TokenizerJsonWordPiece |  7,327.8 μs |   4,951.6 μs |   271.41 μs | 187.5000 | 171.8750 |  46.8750 | 5852.31 KB |
| TokenizerJsonUnigram   |  6,633.6 μs |   3,952.7 μs |   216.66 μs | 109.3750 | 101.5625 |  39.0625 |    4749 KB |
| SpieceModel            |  3,661.4 μs |   4,018.7 μs |   220.28 μs | 144.5313 | 140.6250 |  46.8750 | 4807.72 KB |
| TfidfSave              |    929.4 μs |     474.5 μs |    26.01 μs |  27.3438 |  21.4844 |  21.4844 | 2137.07 KB |
| TfidfLoad              |  2,787.0 μs |   1,187.9 μs |    65.11 μs |  89.8438 |  85.9375 |  27.3438 | 2921.38 KB |
| EmbeddingIndexSave     |  1,668.4 μs |     572.4 μs |    31.38 μs | 181.6406 | 175.7813 | 175.7813 | 20350.6 KB |
| EmbeddingIndexLoad     |  3,513.7 μs |   5,855.0 μs |   320.93 μs | 175.7813 | 140.6250 | 109.3750 | 16094.6 KB |
| EmbeddingIndexSaveFile | 74,092.7 μs |  26,280.4 μs | 1,440.52 μs |        - |        - |        - |  323.53 KB |
| EmbeddingIndexLoadGzip | 52,339.3 μs | 138,195.4 μs | 7,574.95 μs |        - |        - |        - | 16095.2 KB |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **5,204.46 μs** |   **361.552 μs** | **19.818 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 9,752.82 μs |   714.976 μs | 39.190 μs |  1.87 |    0.01 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **79.66 μs** |     **7.723 μs** |  **0.423 μs** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |   109.12 μs |     8.840 μs |  0.485 μs |  1.37 |    0.01 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** | **1,718.31 μs** |    **97.031 μs** |  **5.319 μs** |  **1.00** |    **0.00** |   **1.9531** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 | 1,644.12 μs | 1,100.906 μs | 60.344 μs |  0.96 |    0.03 |   1.9531 |        - |        - |  40.06 KB |        0.95 |
|                            |         |             |              |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |    **13.92 μs** |     **1.099 μs** |  **0.060 μs** |  **1.00** |    **0.01** |   **0.1373** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |    15.57 μs |     1.487 μs |  0.082 μs |  1.12 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **31.68 μs** |   **2.602 μs** |  **0.143 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    31.48 μs |   7.184 μs |  0.394 μs |        - |        - |        - |         - |
| R2Score  | 100000  |    79.16 μs |   3.581 μs |  0.196 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   313.08 μs | 100.016 μs |  5.482 μs | 199.2188 | 199.2188 | 199.2188 |  800193 B |
| **Mse**      | **1000000** |   **329.07 μs** | **111.896 μs** |  **6.133 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   313.14 μs | 107.946 μs |  5.917 μs |        - |        - |        - |         - |
| R2Score  | 1000000 |   796.69 μs |  24.928 μs |  1.366 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 4,117.63 μs | 671.596 μs | 36.812 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|------------:|----------:|------:|--------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **36.732 ms** |   **1.8033 ms** | **0.0988 ms** |  **1.00** |    **0.00** |  **428.5714** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   7.519 ms |   1.4707 ms | 0.0806 ms |  0.20 |    0.00 |  171.8750 |  164.0625 |  85.9375 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   5.556 ms |   3.2493 ms | 0.1781 ms |  0.15 |    0.00 |   15.6250 |         - |        - |    304.73 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   3.930 ms |   0.2696 ms | 0.0148 ms |  0.11 |    0.00 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   7.245 ms |   0.4451 ms | 0.0244 ms |  0.20 |    0.00 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **37.270 ms** |   **2.5960 ms** | **0.1423 ms** |  **1.00** |    **0.00** |  **428.5714** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |   9.681 ms |   8.0996 ms | 0.4440 ms |  0.26 |    0.01 |  328.1250 |  312.5000 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   6.684 ms |   3.1103 ms | 0.1705 ms |  0.18 |    0.00 |   23.4375 |         - |        - |    429.73 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   4.242 ms |   3.5838 ms | 0.1964 ms |  0.11 |    0.00 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   7.121 ms |   3.1267 ms | 0.1714 ms |  0.19 |    0.00 |   23.4375 |         - |        - |    429.69 KB |        0.05 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **664.606 ms** | **154.3776 ms** | **8.4620 ms** |  **1.00** |    **0.02** | **7000.0000** |         **-** |        **-** | **126359.48 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  30.490 ms |   9.8405 ms | 0.5394 ms |  0.05 |    0.00 | 1000.0000 | 1000.0000 | 500.0000 |  10335.26 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  21.494 ms |   2.0640 ms | 0.1131 ms |  0.03 |    0.00 |   62.5000 |         - |        - |   1218.81 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  15.664 ms |   1.0743 ms | 0.0589 ms |  0.02 |    0.00 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  28.176 ms |   7.0290 ms | 0.3853 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |       0.014 |
|                      |           |              |            |             |           |       |         |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **661.294 ms** |  **89.3918 ms** | **4.8999 ms** |  **1.00** |    **0.01** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  46.155 ms |  19.0266 ms | 1.0429 ms |  0.07 |    0.00 | 1636.3636 | 1636.3636 | 909.0909 |  18666.49 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  27.379 ms |  19.0088 ms | 1.0419 ms |  0.04 |    0.00 |   93.7500 |         - |        - |   1718.81 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  15.978 ms |   3.1165 ms | 0.1708 ms |  0.02 |    0.00 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  29.936 ms |   3.0172 ms | 0.1654 ms |  0.05 |    0.00 |   93.7500 |         - |        - |   1718.77 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean           | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |       623.2 ns |        64.27 ns |       3.52 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |       763.2 ns |        46.53 ns |       2.55 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |       626.2 ns |       169.30 ns |       9.28 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |       217.9 ns |        32.30 ns |       1.77 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 7,677,232.8 ns | 1,913,353.64 ns | 104,877.37 ns | 890.6250 | 859.3750 | 625.0000 | 12227088 B |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1,393.5 μs** |   **524.92 μs** |  **28.77 μs** |  **1.00** |    **0.03** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |   960.5 μs |    56.72 μs |   3.11 μs |  0.69 |    0.01 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1,417.3 μs |    78.93 μs |   4.33 μs |  1.02 |    0.02 |  72.2656 |  72.2656 |  72.2656 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1,133.6 μs |    65.87 μs |   3.61 μs |  0.81 |    0.01 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |            |             |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **6,615.7 μs** | **2,107.29 μs** | **115.51 μs** |  **1.00** |    **0.02** | **867.1875** | **867.1875** | **867.1875** | **3436.57 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 4,967.4 μs | 2,318.12 μs | 127.06 μs |  0.75 |    0.02 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 6,966.5 μs | 2,076.92 μs | 113.84 μs |  1.05 |    0.02 | 515.6250 | 500.0000 | 500.0000 | 2349.75 KB |        0.68 |
| HashingWithStopWords | 1000      | 5,919.5 μs | 4,030.15 μs | 220.91 μs |  0.89 |    0.03 | 265.6250 | 265.6250 | 265.6250 | 1139.45 KB |        0.33 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Pattern       | Mean            | Error           | StdDev        | Gen0     | Gen1     | Gen2    | Allocated  |
|--------------------------- |-------------- |----------------:|----------------:|--------------:|---------:|---------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **21,110,705.8 ns** |  **2,666,792.5 ns** | **146,175.90 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.58 KB** |
| Construct                  | [^ ]+         |      1,249.6 ns |        112.5 ns |       6.17 ns |   0.3319 |   0.0019 |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      1,972.4 ns |        412.8 ns |      22.62 ns |   0.4539 |   0.0038 |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **17,817,297.7 ns** |  **1,359,217.5 ns** |  **74,503.30 ns** |        **-** |        **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1,197.7 ns |        178.9 ns |       9.81 ns |   0.3300 |   0.0019 |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      1,787.1 ns |      1,694.6 ns |      92.89 ns |   0.4387 |   0.0038 |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **23,951,196.3 ns** | **12,047,567.0 ns** | **660,367.81 ns** |  **62.5000** |  **31.2500** |       **-** | **1555.96 KB** |
| Construct                  | [A-Za-z]\\w+  |      2,077.0 ns |        308.0 ns |      16.88 ns |   0.4158 |   0.0038 |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      2,866.0 ns |        531.3 ns |      29.12 ns |   0.5341 |        - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **12,647,526.4 ns** |    **467,177.3 ns** |  **25,607.56 ns** | **156.2500** | **140.6250** | **93.7500** | **9743.47 KB** |
| Construct                  | \\b\\w\\w+\\b |        664.3 ns |        389.9 ns |      21.37 ns |   0.1135 |        - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1,287.8 ns |      1,898.7 ns |     104.08 ns |   0.2346 |        - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **21,542,300.6 ns** |  **2,521,996.3 ns** | **138,239.13 ns** |  **62.5000** |  **31.2500** |       **-** | **1554.45 KB** |
| Construct                  | \\S+          |      1,334.2 ns |        783.0 ns |      42.92 ns |   0.3223 |   0.0019 |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      2,125.2 ns |        233.1 ns |      12.77 ns |   0.4425 |   0.0038 |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|----------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **11.05 ms** |   **5.546 ms** |  **0.304 ms** |  **1.00** |    **0.03** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  32.09 ms |   1.070 ms |  0.059 ms |  2.91 |    0.07 |  218.7500 |   3.55 MB |        0.41 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **27.37 ms** |   **9.369 ms** |  **0.514 ms** |  **1.00** |    **0.02** |  **312.5000** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  34.78 ms |   3.464 ms |  0.190 ms |  1.27 |    0.02 |  133.3333 |   3.09 MB |        0.57 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **56.70 ms** |  **28.469 ms** |  **1.560 ms** |  **1.00** |    **0.03** | **1777.7778** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 202.03 ms | 270.874 ms | 14.848 ms |  3.56 |    0.24 | 3666.6667 |  59.08 MB |        2.08 |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean     | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |----- |---------:|----------:|---------:|------:|--------:|----------:|------------:|
| **Dot**    | **384**  | **33.35 ns** |  **4.495 ns** | **0.246 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| L2Norm | 384  | 23.65 ns |  3.356 ns | 0.184 ns |  0.71 |    0.01 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **768**  | **68.65 ns** | **32.450 ns** | **1.779 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| L2Norm | 768  | 47.30 ns |  2.579 ns | 0.141 ns |  0.69 |    0.02 |         - |          NA |
|        |      |          |           |          |       |         |           |             |
| **Dot**    | **1024** | **89.79 ns** | **21.259 ns** | **1.165 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| L2Norm | 1024 | 63.96 ns | 13.601 ns | 0.745 ns |  0.71 |    0.01 |         - |          NA |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |-----------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |   **112.4 μs** |  **42.46 μs** |  **2.33 μs** |  **1.00** |    **0.03** |   **12.4512** |    **2.1973** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |   118.9 μs |  12.19 μs |  0.67 μs |  1.06 |    0.02 |   14.2822 |    3.5400 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |   405.2 μs |  24.48 μs |  1.34 μs |  3.60 |    0.07 |   35.6445 |   13.1836 |         - |   586.98 KB |        2.86 |
| CountCharWordBoundary | 200       | 1,342.9 μs | 778.14 μs | 42.65 μs | 11.95 |    0.39 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |   103.3 μs |  16.98 μs |  0.93 μs |  0.92 |    0.02 |    8.3008 |    1.0986 |         - |   136.13 KB |        0.66 |
|                       |           |            |           |          |       |         |           |           |           |             |             |
| **Count**                 | **1000**      | **1,041.2 μs** | **173.84 μs** |  **9.53 μs** |  **1.00** |    **0.01** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      | 1,160.2 μs |  61.07 μs |  3.35 μs |  1.11 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      | 2,609.0 μs | 974.22 μs | 53.40 μs |  2.51 |    0.05 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 9,352.0 μs | 293.45 μs | 16.09 μs |  8.98 |    0.07 | 1046.8750 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      | 1,089.4 μs |  46.77 μs |  2.56 μs |  1.05 |    0.01 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **2.602 ms** |   **2.535 ms** | **0.1390 ms** |  **1.00** |    **0.06** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  25.638 ms |  15.622 ms | 0.8563 ms |  9.87 |    0.53 |  3968.7500 |  3875.0000 |  3687.5000 |  28.44 MB |       15.94 |
|          |           |            |            |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **12.378 ms** |   **9.919 ms** | **0.5437 ms** |  **1.00** |    **0.05** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 230.377 ms | 162.679 ms | 8.9170 ms | 18.64 |    0.94 | 75000.0000 | 75000.0000 | 75000.0000 | 324.33 MB |       40.00 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-10-26, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.166 | 0.036 | 0.21x | 0.166 | 0.036 | 0.21x |
| cdist_wratio_n50 | 2.221 | 2.484 | 1.12x | 2.221 | 2.483 | 1.12x |
| cdist_ratio_varied_n50 | 0.137 | 0.031 | 0.23x | 0.137 | 0.031 | 0.23x |
| cdist_ratio_varied_cutoff90_n50 | 0.058 | 0.031 | 0.54x | 0.058 | 0.031 | 0.54x |
| cdist_ratio_n200 | 2.782 | 0.439 | 0.16x | 2.784 | 0.439 | 0.16x |
| cdist_wratio_n200 | 33.941 | 37.618 | 1.11x | 33.940 | 37.616 | 1.11x |
| cdist_ratio_varied_n200 | 2.320 | 0.386 | 0.17x | 2.323 | 0.386 | 0.17x |
| cdist_ratio_varied_cutoff90_n200 | 0.990 | 0.386 | 0.39x | 0.992 | 0.386 | 0.39x |
| cdist_ratio_n500 | 16.634 | 2.651 | 0.16x | 16.648 | 2.651 | 0.16x |
| cdist_wratio_n500 | 204.162 | 228.844 | 1.12x | 204.317 | 228.837 | 1.12x |
| cdist_ratio_varied_n500 | 13.997 | 2.297 | 0.16x | 14.090 | 2.297 | 0.16x |
| cdist_ratio_varied_cutoff90_n500 | 5.452 | 2.297 | 0.42x | 5.539 | 2.297 | 0.41x |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 71.6 | 12.3 | 5.82x C# faster |
| latin | 32 | 101.5 | 37.7 | 2.69x C# faster |
| latin | 128 | 288.4 | 256.9 | 1.12x C# faster |
| latin | 512 | 3022.3 | 3310.2 | 1.10x Py faster |
| cjk | 8 | 84.6 | 11.7 | 7.24x C# faster |
| cjk | 32 | 153.7 | 81.6 | 1.88x C# faster |
| cjk | 128 | 1350.9 | 845.5 | 1.60x C# faster |
| cjk | 512 | 11088.0 | 5566.1 | 1.99x C# faster |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.14 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 85.2 | 9.1 | 9.41x C# faster |
| latin | 32 | 170.5 | 79.8 | 2.14x C# faster |
| latin | 128 | 1153.9 | 518.9 | 2.22x C# faster |
| latin | 512 | 9202.7 | 7349.4 | 1.25x C# faster |
| cjk | 8 | 87.1 | 9.7 | 8.99x C# faster |
| cjk | 32 | 195.4 | 116.9 | 1.67x C# faster |
| cjk | 128 | 1993.6 | 1390.8 | 1.43x C# faster |
| cjk | 512 | 17355.7 | 11383.8 | 1.52x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.002 | 0.445 | 188.54x | 0.002 | 0.445 | 188.54x |
| accuracy_n1000_k2 | 0.000 | 0.239 | 1996.23x | 0.000 | 0.238 | 1996.38x |
| precision_recall_f1_macro_n1000_k2 | 0.003 | 0.807 | 317.73x | 0.003 | 0.807 | 317.72x |
| classification_report_n1000_k2 | 0.004 | 3.156 | 825.83x | 0.004 | 3.156 | 825.86x |
| roc_auc_binary_n1000_k2 | 0.009 | 0.891 | 103.77x | 0.009 | 0.891 | 103.77x |
| balanced_accuracy_n1000_k2 | 0.003 | 0.493 | 196.97x | 0.003 | 0.493 | 196.98x |
| matthews_n1000_k2 | 0.002 | 0.937 | 375.77x | 0.002 | 0.937 | 375.77x |
| cohen_kappa_n1000_k2 | 0.002 | 0.512 | 206.66x | 0.002 | 0.512 | 206.66x |
| mse_n1000_k2 | 0.000 | 0.132 | 401.89x | 0.000 | 0.132 | 401.89x |
| mae_n1000_k2 | 0.000 | 0.131 | 419.73x | 0.000 | 0.131 | 419.68x |
| median_ae_n1000_k2 | 0.003 | 0.135 | 46.78x | 0.003 | 0.135 | 46.78x |
| r2_n1000_k2 | 0.001 | 0.153 | 184.74x | 0.001 | 0.153 | 184.74x |
| confusion_matrix_n1000_k10 | 0.002 | 0.447 | 223.52x | 0.002 | 0.447 | 223.52x |
| accuracy_n1000_k10 | 0.000 | 0.238 | 1979.62x | 0.000 | 0.238 | 1979.59x |
| precision_recall_f1_macro_n1000_k10 | 0.002 | 0.799 | 351.11x | 0.002 | 0.799 | 351.10x |
| classification_report_n1000_k10 | 0.006 | 2.998 | 534.55x | 0.006 | 2.998 | 534.57x |
| roc_auc_ovr_macro_n1000_k10 | 0.333 | 4.536 | 13.63x | 0.333 | 4.536 | 13.63x |
| balanced_accuracy_n1000_k10 | 0.002 | 0.477 | 200.71x | 0.002 | 0.476 | 200.71x |
| matthews_n1000_k10 | 0.002 | 0.895 | 383.90x | 0.002 | 0.894 | 383.91x |
| cohen_kappa_n1000_k10 | 0.003 | 0.489 | 188.68x | 0.003 | 0.489 | 188.68x |
| mse_n1000_k10 | 0.000 | 0.129 | 405.67x | 0.000 | 0.129 | 405.65x |
| mae_n1000_k10 | 0.000 | 0.124 | 397.97x | 0.000 | 0.124 | 397.96x |
| median_ae_n1000_k10 | 0.003 | 0.131 | 46.72x | 0.003 | 0.131 | 46.72x |
| r2_n1000_k10 | 0.001 | 0.153 | 185.56x | 0.001 | 0.153 | 185.56x |
| confusion_matrix_n100000_k2 | 0.229 | 6.569 | 28.66x | 0.229 | 6.569 | 28.66x |
| accuracy_n100000_k2 | 0.012 | 2.263 | 196.37x | 0.012 | 2.263 | 196.38x |
| precision_recall_f1_macro_n100000_k2 | 0.230 | 7.876 | 34.22x | 0.230 | 7.875 | 34.22x |
| classification_report_n100000_k2 | 0.232 | 16.973 | 73.27x | 0.232 | 16.973 | 73.27x |
| roc_auc_binary_n100000_k2 | 2.268 | 16.995 | 7.49x | 2.267 | 16.993 | 7.49x |
| balanced_accuracy_n100000_k2 | 0.229 | 6.777 | 29.55x | 0.229 | 6.776 | 29.55x |
| matthews_n100000_k2 | 0.232 | 13.452 | 57.90x | 0.232 | 13.451 | 57.89x |
| cohen_kappa_n100000_k2 | 0.232 | 6.606 | 28.45x | 0.232 | 6.604 | 28.45x |
| mse_n100000_k2 | 0.031 | 0.226 | 7.24x | 0.031 | 0.226 | 7.24x |
| mae_n100000_k2 | 0.031 | 0.226 | 7.39x | 0.031 | 0.226 | 7.39x |
| median_ae_n100000_k2 | 0.323 | 1.155 | 3.58x | 0.335 | 1.155 | 3.45x |
| r2_n100000_k2 | 0.078 | 0.355 | 4.58x | 0.078 | 0.355 | 4.58x |
| confusion_matrix_n100000_k10 | 0.191 | 6.722 | 35.24x | 0.191 | 6.719 | 35.23x |
| accuracy_n100000_k10 | 0.012 | 2.316 | 195.29x | 0.012 | 2.315 | 195.27x |
| precision_recall_f1_macro_n100000_k10 | 0.189 | 8.274 | 43.82x | 0.189 | 8.273 | 43.82x |
| classification_report_n100000_k10 | 0.189 | 18.387 | 97.04x | 0.189 | 18.385 | 97.03x |
| roc_auc_ovr_macro_n100000_k10 | 14.801 | 138.136 | 9.33x | 14.800 | 138.112 | 9.33x |
| balanced_accuracy_n100000_k10 | 0.195 | 6.540 | 33.48x | 0.195 | 6.539 | 33.47x |
| matthews_n100000_k10 | 0.190 | 13.687 | 72.03x | 0.190 | 13.685 | 72.02x |
| cohen_kappa_n100000_k10 | 0.191 | 6.582 | 34.55x | 0.190 | 6.581 | 34.55x |
| mse_n100000_k10 | 0.031 | 0.227 | 7.30x | 0.031 | 0.227 | 7.30x |
| mae_n100000_k10 | 0.030 | 0.222 | 7.42x | 0.030 | 0.222 | 7.42x |
| median_ae_n100000_k10 | 0.346 | 1.111 | 3.21x | 0.375 | 1.111 | 2.97x |
| r2_n100000_k10 | 0.077 | 0.348 | 4.55x | 0.077 | 0.348 | 4.55x |
| confusion_matrix_n1000000_k2 | 2.314 | 63.068 | 27.26x | 2.314 | 63.062 | 27.26x |
| accuracy_n1000000_k2 | 0.121 | 20.853 | 171.93x | 0.121 | 20.851 | 171.92x |
| precision_recall_f1_macro_n1000000_k2 | 2.285 | 69.635 | 30.47x | 2.285 | 69.624 | 30.47x |
| classification_report_n1000000_k2 | 2.277 | 140.998 | 61.91x | 2.277 | 140.970 | 61.91x |
| roc_auc_binary_n1000000_k2 | 35.921 | 192.620 | 5.36x | 35.917 | 192.597 | 5.36x |
| balanced_accuracy_n1000000_k2 | 2.306 | 62.856 | 27.25x | 2.306 | 62.831 | 27.25x |
| matthews_n1000000_k2 | 2.326 | 127.207 | 54.69x | 2.326 | 127.196 | 54.68x |
| cohen_kappa_n1000000_k2 | 2.243 | 63.054 | 28.11x | 2.243 | 63.046 | 28.11x |
| mse_n1000000_k2 | 0.309 | 1.568 | 5.07x | 0.309 | 1.567 | 5.07x |
| mae_n1000000_k2 | 0.301 | 1.562 | 5.19x | 0.301 | 1.562 | 5.19x |
| median_ae_n1000000_k2 | 3.285 | 9.757 | 2.97x | 3.370 | 9.755 | 2.89x |
| r2_n1000000_k2 | 0.772 | 2.550 | 3.30x | 0.772 | 2.550 | 3.30x |
| confusion_matrix_n1000000_k10 | 1.809 | 60.111 | 33.22x | 1.809 | 60.086 | 33.21x |
| accuracy_n1000000_k10 | 0.122 | 20.691 | 169.83x | 0.122 | 20.688 | 169.82x |
| precision_recall_f1_macro_n1000000_k10 | 1.813 | 72.442 | 39.95x | 1.813 | 72.433 | 39.95x |
| classification_report_n1000000_k10 | 1.803 | 152.271 | 84.44x | 1.803 | 152.260 | 84.44x |
| balanced_accuracy_n1000000_k10 | 1.809 | 62.563 | 34.59x | 1.809 | 62.556 | 34.59x |
| matthews_n1000000_k10 | 1.801 | 128.996 | 71.62x | 1.801 | 128.972 | 71.61x |
| cohen_kappa_n1000000_k10 | 1.810 | 62.688 | 34.63x | 1.810 | 62.686 | 34.63x |
| mse_n1000000_k10 | 0.320 | 1.625 | 5.07x | 0.320 | 1.624 | 5.07x |
| mae_n1000000_k10 | 0.311 | 1.619 | 5.21x | 0.311 | 1.619 | 5.21x |
| median_ae_n1000000_k10 | 3.644 | 9.788 | 2.69x | 3.883 | 9.786 | 2.52x |
| r2_n1000000_k10 | 0.770 | 2.533 | 3.29x | 0.770 | 2.532 | 3.29x |

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

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 3.309 | 5.875 | 1.78x | 3.838 | 5.875 | 1.53x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 7.629 | 9.448 | 1.24x | 7.978 | 9.447 | 1.18x | 706,526 | 706,526 |
| tokenizer_json_unigram | 7.604 | 28.637 | 3.77x | 7.886 | 28.633 | 3.63x | 1,990,038 | 1,990,038 |
| spiece_model | 4.941 | 19.105 | 3.87x | 5.194 | 19.105 | 3.68x | 533,084 | 533,084 |
| tfidf_save | 1.205 | 1.541 | 1.28x | 1.234 | 1.541 | 1.25x | 581,787 | 591,922 |
| tfidf_load | 3.434 | 2.677 | 0.78x | 3.620 | 2.677 | 0.74x | 581,787 | 591,922 |
| embedding_index_save | 1.964 | 0.892 | 0.45x | 2.228 | 0.892 | 0.40x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 74.879 | 54.838 | 0.73x | 7.964 | 4.267 | 0.54x | 20,589,007 | 15,360,128 |
| embedding_index_load | 3.149 | 0.575 | 0.18x | 3.403 | 0.575 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 3.129 | 0.439 | 0.14x | 3.393 | 0.439 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 2.508 | 0.572 | 0.23x | 2.667 | 0.572 | 0.21x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 0.984 | 0.574 | 0.58x | 1.179 | 0.574 | 0.49x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.000 | 63.29x | 0.000 | 0.000 | 63.29x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 296.863 | 417.457 | 1.41x | 299.061 | 417.426 | 1.40x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 48.424 | 48.765 | 1.01x | 48.722 | 48.761 | 1.00x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.042 | 0.061 | 1.46x | 0.042 | 0.061 | 1.46x |
| stratified_n10000 | 0.092 | 0.420 | 4.55x | 0.092 | 0.420 | 4.55x |
| traintest_n10000 | 0.003 | 0.100 | 33.84x | 0.003 | 0.100 | 33.79x |
| kfold_seeded_n10000 | 0.174 | 0.409 | 2.34x | 0.174 | 0.409 | 2.34x |
| stratified_seeded_n10000 | 0.173 | 0.835 | 4.84x | 0.173 | 0.835 | 4.84x |
| traintest_seeded_n10000 | 0.038 | 0.226 | 5.90x | 0.038 | 0.226 | 5.90x |
| stratified_traintest_n10000 | 0.500 | 1.522 | 3.04x | 0.500 | 1.522 | 3.04x |
| group_kfold_n10000 | 0.062 | 0.306 | 4.93x | 0.062 | 0.306 | 4.93x |
| stratified_group_kfold_n10000 | 0.130 | 14.471 | 111.04x | 0.130 | 14.470 | 111.02x |
| timeseries_n10000 | 0.000 | 0.007 | 90.84x | 0.000 | 0.007 | 90.84x |
| repeated_kfold_n10000 | 0.690 | 1.237 | 1.79x | 0.690 | 1.237 | 1.79x |
| kfold_n100000 | 0.684 | 1.879 | 2.75x | 0.777 | 1.879 | 2.42x |
| stratified_n100000 | 1.171 | 4.566 | 3.90x | 1.282 | 4.565 | 3.56x |
| traintest_n100000 | 0.154 | 0.206 | 1.34x | 0.154 | 0.206 | 1.33x |
| kfold_seeded_n100000 | 2.663 | 5.135 | 1.93x | 2.783 | 5.135 | 1.85x |
| stratified_seeded_n100000 | 3.122 | 7.848 | 2.51x | 3.239 | 7.848 | 2.42x |
| traintest_seeded_n100000 | 1.559 | 1.127 | 0.72x | 1.586 | 1.127 | 0.71x |
| stratified_traintest_n100000 | 6.260 | 13.758 | 2.20x | 6.326 | 13.757 | 2.17x |
| group_kfold_n100000 | 0.843 | 4.276 | 5.07x | 0.932 | 4.276 | 4.59x |
| stratified_group_kfold_n100000 | 1.555 | 142.592 | 91.69x | 1.649 | 142.573 | 86.45x |
| timeseries_n100000 | 0.000 | 0.029 | 372.46x | 0.000 | 0.029 | 372.47x |
| repeated_kfold_n100000 | 7.300 | 14.015 | 1.92x | 7.412 | 14.014 | 1.89x |
| kfold_n1000000 | 5.262 | 11.409 | 2.17x | 5.666 | 11.407 | 2.01x |
| stratified_n1000000 | 11.085 | 44.144 | 3.98x | 11.540 | 44.141 | 3.82x |
| traintest_n1000000 | 0.313 | 1.172 | 3.75x | 0.353 | 1.172 | 3.32x |
| kfold_seeded_n1000000 | 27.516 | 44.021 | 1.60x | 28.363 | 44.016 | 1.55x |
| stratified_seeded_n1000000 | 35.202 | 74.213 | 2.11x | 36.510 | 74.206 | 2.03x |
| traintest_seeded_n1000000 | 15.437 | 10.082 | 0.65x | 15.577 | 10.080 | 0.65x |
| stratified_traintest_n1000000 | 74.531 | 147.843 | 1.98x | 74.747 | 147.834 | 1.98x |
| group_kfold_n1000000 | 7.787 | 34.980 | 4.49x | 8.257 | 34.973 | 4.24x |
| stratified_group_kfold_n1000000 | 16.695 | 1454.660 | 87.13x | 17.740 | 1454.542 | 81.99x |
| timeseries_n1000000 | 0.000 | 0.245 | 3897.22x | 0.000 | 0.245 | 3896.96x |
| repeated_kfold_n1000000 | 78.297 | 135.254 | 1.73x | 80.046 | 135.226 | 1.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-10-27, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.003 | 0.334 | 122.14x | 0.003 | 0.334 | 122.11x |
| mann_whitney_n1000 | 0.019 | 0.286 | 14.74x | 0.019 | 0.285 | 14.72x |
| chi_square_n1000 | 0.000 | 0.139 | 1538.34x | 0.000 | 0.139 | 1538.25x |
| fligner_n1000 | 0.039 | 0.312 | 8.07x | 0.039 | 0.311 | 8.07x |
| anderson_ksamp_n1000 | 0.037 | 0.175 | 4.72x | 0.037 | 0.175 | 4.72x |
| spearman_matrix_n1000 | 0.050 | 0.309 | 6.16x | 0.050 | 0.308 | 6.16x |
| pointbiserial_n1000 | 0.006 | 0.241 | 38.62x | 0.006 | 0.241 | 38.62x |
| welch_t_n10000 | 0.025 | 0.355 | 14.12x | 0.025 | 0.355 | 14.12x |
| mann_whitney_n10000 | 0.288 | 1.549 | 5.37x | 0.288 | 1.549 | 5.37x |
| chi_square_n10000 | 0.001 | 0.135 | 198.34x | 0.001 | 0.135 | 198.32x |
| fligner_n10000 | 0.644 | 1.878 | 2.92x | 0.684 | 1.878 | 2.75x |
| anderson_ksamp_n10000 | 0.959 | 2.364 | 2.47x | 1.017 | 2.364 | 2.33x |
| spearman_matrix_n10000 | 0.896 | 2.748 | 3.07x | 0.923 | 2.748 | 2.98x |
| pointbiserial_n10000 | 0.057 | 0.265 | 4.61x | 0.057 | 0.265 | 4.61x |
| welch_t_n100000 | 0.253 | 0.580 | 2.29x | 0.253 | 0.580 | 2.29x |
| mann_whitney_n100000 | 3.272 | 16.297 | 4.98x | 3.272 | 16.295 | 4.98x |
| chi_square_n100000 | 0.008 | 0.144 | 18.27x | 0.008 | 0.144 | 18.27x |
| fligner_n100000 | 7.911 | 20.249 | 2.56x | 8.385 | 20.247 | 2.41x |
| anderson_ksamp_n100000 | 8.076 | 26.955 | 3.34x | 8.417 | 26.951 | 3.20x |
| spearman_matrix_n100000 | 8.038 | 32.977 | 4.10x | 8.501 | 32.974 | 3.88x |
| pointbiserial_n100000 | 0.881 | 0.680 | 0.77x | 0.889 | 2.713 | 3.05x |

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

_As of 2026-10-26, measured at commit `6dd69a3f07ee838a1a1aa90df3d1115b7a27fb90`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.14
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.267 | 0.461 | 1.72x | 0.270 | 0.461 | 1.71x |
| polynomial_n10000 | 0.732 | 1.017 | 1.39x | 0.737 | 1.017 | 1.38x |
| kbins_fit_n10000 | 1.455 | 2.622 | 1.80x | 1.455 | 2.621 | 1.80x |
| kbins_n10000 | 0.727 | 4.452 | 6.12x | 0.830 | 4.452 | 5.37x |
| quantile_fit_n10000 | 1.543 | 3.779 | 2.45x | 1.543 | 3.778 | 2.45x |
| quantile_n10000 | 1.806 | 1.693 | 0.94x | 1.809 | 1.693 | 0.94x |
| power_fit_n10000 | 6.155 | 135.810 | 22.06x | 6.160 | 135.810 | 22.05x |
| power_n10000 | 1.538 | 1.516 | 0.99x | 1.540 | 1.516 | 0.98x |
| knn_impute_n10000 | 7.360 | 22.407 | 3.04x | 7.359 | 89.358 | 12.14x |
| label_n10000 | 1.161 | 0.765 | 0.66x | 1.161 | 0.765 | 0.66x |
| normalize_n100000 | 1.398 | 1.834 | 1.31x | 1.770 | 1.834 | 1.04x |
| polynomial_n100000 | 2.713 | 7.050 | 2.60x | 3.321 | 7.049 | 2.12x |
| kbins_fit_n100000 | 14.699 | 8.168 | 0.56x | 15.509 | 8.167 | 0.53x |
| kbins_n100000 | 6.579 | 12.315 | 1.87x | 7.334 | 12.313 | 1.68x |
| quantile_fit_n100000 | 15.046 | 18.144 | 1.21x | 15.791 | 18.142 | 1.15x |
| quantile_n100000 | 16.740 | 14.658 | 0.88x | 17.160 | 14.657 | 0.85x |
| power_fit_n100000 | 65.505 | 916.508 | 13.99x | 68.343 | 916.373 | 13.41x |
| power_n100000 | 14.129 | 10.075 | 0.71x | 14.565 | 10.074 | 0.69x |
| knn_impute_n100000 | 7.349 | 19.307 | 2.63x | 7.347 | 77.056 | 10.49x |
| label_n100000 | 15.429 | 7.159 | 0.46x | 15.728 | 7.158 | 0.46x |

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
