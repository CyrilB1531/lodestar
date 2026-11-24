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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.529 ms** |   **0.2771 ms** |  **0.0152 ms** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   5.296 ms |  10.3488 ms |  0.5673 ms |  2.09 |    0.19 |        - |        - |        - |    42832 B |          NA |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.701 ms |   6.7377 ms |  0.3693 ms |  2.25 |    0.13 |  93.7500 |  93.7500 |  93.7500 |   470832 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **11.863 ms** |   **0.2998 ms** |  **0.0164 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |  10.621 ms |   8.1655 ms |  0.4476 ms |  0.90 |    0.03 |        - |        - |        - |    42840 B |          NA |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  13.392 ms |  16.7007 ms |  0.9154 ms |  1.13 |    0.07 | 312.5000 | 312.5000 | 312.5000 |  2711060 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **51.147 ms** |   **3.3914 ms** |  **0.1859 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  51.749 ms |  24.6987 ms |  1.3538 ms |  1.01 |    0.02 |        - |        - |        - |   802898 B |          NA |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  67.928 ms | 186.0545 ms | 10.1983 ms |  1.33 |    0.17 | 375.0000 | 375.0000 | 375.0000 |  8072038 B |          NA |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **236.248 ms** |   **8.4129 ms** |  **0.4611 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 176.982 ms | 114.9586 ms |  6.3013 ms |  0.75 |    0.02 |        - |        - |        - |   803165 B |          NA |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 265.765 ms |  61.8055 ms |  3.3878 ms |  1.12 |    0.01 | 500.0000 | 500.0000 | 500.0000 | 52871716 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,592.988 ms** |   **155.6830 ms** |   **8.5335 ms** | **1.000** |    **0.01** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,687.188 ms |   442.4952 ms |  24.2547 ms | 1.059 |    0.01 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.514 ms |     0.3264 ms |   0.0179 ms | 0.001 |    0.00 |   330.0781 |   330.0781 |   330.0781 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **6,319.729 ms** | **1,401.0987 ms** |  **76.7989 ms** | **1.000** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **37.83 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  6,478.407 ms | 2,584.1785 ms | 141.6475 ms | 1.025 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.94 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      9.156 ms |     1.9763 ms |   0.1083 ms | 0.001 |    0.00 |   484.3750 |   484.3750 |   484.3750 |   5.86 MB |        0.15 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **6,452.793 ms** | **1,781.4627 ms** |  **97.6480 ms** | **1.000** |    **0.02** |  **2000.0000** |  **2000.0000** |  **2000.0000** |  **44.12 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  6,462.409 ms |   386.5934 ms |  21.1905 ms | 1.002 |    0.01 |  1000.0000 |  1000.0000 |  1000.0000 |  42.15 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      8.013 ms |     1.5923 ms |   0.0873 ms | 0.001 |    0.00 |   375.0000 |   375.0000 |   375.0000 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **26,866.125 ms** | **7,077.6237 ms** | **387.9484 ms** | **1.000** |    **0.02** | **15000.0000** | **14000.0000** | **14000.0000** | **388.84 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 26,831.569 ms | 1,388.8500 ms |  76.1276 ms | 0.999 |    0.01 | 17000.0000 | 16000.0000 | 16000.0000 | 385.31 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     23.795 ms |     1.8830 ms |   0.1032 ms | 0.001 |    0.00 |   312.5000 |   312.5000 |   312.5000 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.CosineTopKSelectionBenchmarks-report-github

_As of 2026-11-20, measured at commit `d294b0dd903e83f6f31a701c2d1a31ff6843cd14`._

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

| Method       | Documents | K    | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Allocated   | Alloc Ratio |
|------------- |---------- |----- |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|------------:|------------:|
| **SimdBaseline** | **10000**     | **10**   | **CPUAc(...) GPU) [26]** |       **9.871 ms** |      **1.6159 ms** |     **0.0886 ms** |     **1.00** |    **0.01** |    **26.02 KB** |        **1.00** |
| GpuResident  | 10000     | 10   | CPUAc(...) GPU) [26] |  27,407.531 ms |  7,134.8206 ms |   391.0836 ms | 2,776.82 |   40.53 |  1519.13 KB |       58.39 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **100**  | **CPUAc(...) GPU) [26]** |      **10.183 ms** |      **9.7291 ms** |     **0.5333 ms** |     **1.00** |    **0.06** |    **37.27 KB** |        **1.00** |
| GpuResident  | 10000     | 100  | CPUAc(...) GPU) [26] |  28,312.787 ms |  7,955.9133 ms |   436.0905 ms | 2,785.47 |  129.09 |  1541.63 KB |       41.37 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **10000**     | **1000** | **CPUAc(...) GPU) [26]** |      **14.496 ms** |      **0.9244 ms** |     **0.0507 ms** |     **1.00** |    **0.00** |   **149.77 KB** |        **1.00** |
| GpuResident  | 10000     | 1000 | CPUAc(...) GPU) [26] |  32,521.166 ms | 10,589.3107 ms |   580.4359 ms | 2,243.45 |   35.34 |   1765.7 KB |       11.79 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **10**   | **CPUAc(...) GPU) [26]** |     **138.959 ms** |      **6.1147 ms** |     **0.3352 ms** |     **1.00** |    **0.00** |    **26.25 KB** |        **1.00** |
| GpuResident  | 100000    | 10   | CPUAc(...) GPU) [26] | 269,907.774 ms | 56,422.9601 ms | 3,092.7328 ms | 1,942.36 |   19.70 | 39277.98 KB |    1,496.30 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **100**  | **CPUAc(...) GPU) [26]** |     **142.469 ms** |     **53.7920 ms** |     **2.9485 ms** |     **1.00** |    **0.03** |     **37.5 KB** |        **1.00** |
| GpuResident  | 100000    | 100  | CPUAc(...) GPU) [26] | 273,266.665 ms | 15,571.3204 ms |   853.5166 ms | 1,918.62 |   34.39 | 39299.82 KB |    1,048.00 |
|              |           |      |                      |                |                |               |          |         |             |             |
| **SimdBaseline** | **100000**    | **1000** | **CPUAc(...) GPU) [26]** |     **148.846 ms** |     **25.9228 ms** |     **1.4209 ms** |     **1.00** |    **0.01** |      **150 KB** |        **1.00** |
| GpuResident  | 100000    | 1000 | CPUAc(...) GPU) [26] | 278,611.111 ms | 26,581.9880 ms | 1,457.0485 ms | 1,871.93 |   17.71 | 39525.48 KB |      263.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledMinHashSignaturesBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method         | Documents | Permutations | Device               | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|--------------- |---------- |------------- |--------------------- |-------------:|--------------:|-------------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| **CpuBaseline**    | **5000**      | **64**           | **CPUAc(...) GPU) [26]** |     **88.08 ms** |      **5.474 ms** |     **0.300 ms** |  **1.00** |    **0.00** |          **-** |          **-** |         **-** |   **2.98 MB** |        **1.00** |
| GpuResident    | 5000      | 64           | CPUAc(...) GPU) [26] |  4,572.75 ms |  2,829.875 ms |   155.115 ms | 51.91 |    1.53 |          - |          - |         - |   5.81 MB |        1.95 |
| GpuWithHashing | 5000      | 64           | CPUAc(...) GPU) [26] |  4,584.78 ms |  2,337.456 ms |   128.124 ms | 52.05 |    1.27 |          - |          - |         - |  10.94 MB |        3.68 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **5000**      | **128**          | **CPUAc(...) GPU) [26]** |    **106.29 ms** |     **16.010 ms** |     **0.878 ms** |  **1.00** |    **0.01** |          **-** |          **-** |         **-** |    **4.2 MB** |        **1.00** |
| GpuResident    | 5000      | 128          | CPUAc(...) GPU) [26] |  8,530.43 ms |  5,095.588 ms |   279.306 ms | 80.26 |    2.35 |          - |          - |         - |  13.78 MB |        3.28 |
| GpuWithHashing | 5000      | 128          | CPUAc(...) GPU) [26] |  9,006.34 ms |  1,701.221 ms |    93.250 ms | 84.74 |    0.97 |          - |          - |         - |  18.91 MB |        4.51 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **64**           | **CPUAc(...) GPU) [26]** |    **877.98 ms** |    **146.065 ms** |     **8.006 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **29.76 MB** |        **1.00** |
| GpuResident    | 50000     | 64           | CPUAc(...) GPU) [26] | 43,189.71 ms |  1,724.803 ms |    94.542 ms | 49.19 |    0.40 |  3000.0000 |  2000.0000 | 2000.0000 | 160.07 MB |        5.38 |
| GpuWithHashing | 50000     | 64           | CPUAc(...) GPU) [26] | 44,100.17 ms |  2,678.312 ms |   146.807 ms | 50.23 |    0.42 |  5000.0000 |  3000.0000 | 2000.0000 | 211.63 MB |        7.11 |
|                |           |              |                      |              |               |              |       |         |            |            |           |           |             |
| **CpuBaseline**    | **50000**     | **128**          | **CPUAc(...) GPU) [26]** |  **1,062.07 ms** |    **177.276 ms** |     **9.717 ms** |  **1.00** |    **0.01** |  **1000.0000** |          **-** |         **-** |  **41.96 MB** |        **1.00** |
| GpuResident    | 50000     | 128          | CPUAc(...) GPU) [26] | 86,186.90 ms | 11,179.375 ms |   612.779 ms | 81.15 |    0.82 | 10000.0000 |  9000.0000 | 8000.0000 | 536.25 MB |       12.78 |
| GpuWithHashing | 50000     | 128          | CPUAc(...) GPU) [26] | 88,871.79 ms | 42,422.425 ms | 2,325.316 ms | 83.68 |    2.01 | 12000.0000 | 10000.0000 | 8000.0000 | 587.68 MB |       14.01 |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                          | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| Lodestar_StudentCdf             | 2,491.70 ns | 204.184 ns | 11.192 ns | 1.000 |      - |         - |          NA |
| MathNet_StudentCdf              |   294.67 ns |  12.207 ns |  0.669 ns | 0.118 |      - |         - |          NA |
| MetaNumerics_StudentCdf         |   345.98 ns |  17.039 ns |  0.934 ns | 0.139 |      - |         - |          NA |
| Lodestar_StudentQuantile        | 7,666.44 ns |   8.689 ns |  0.476 ns | 3.077 |      - |         - |          NA |
| MathNet_StudentQuantile         | 5,190.41 ns | 407.575 ns | 22.341 ns | 2.083 |      - |     112 B |          NA |
| MetaNumerics_StudentQuantile    | 1,230.46 ns |  38.617 ns |  2.117 ns | 0.494 | 0.0114 |     288 B |          NA |
| Lodestar_FisherCdf              |   926.03 ns |  39.985 ns |  2.192 ns | 0.372 |      - |         - |          NA |
| MathNet_FisherCdf               |   117.42 ns |   6.356 ns |  0.348 ns | 0.047 |      - |         - |          NA |
| MetaNumerics_FisherCdf          |   121.44 ns |   0.867 ns |  0.048 ns | 0.049 |      - |         - |          NA |
| Lodestar_FisherQuantile         | 3,234.34 ns | 492.025 ns | 26.970 ns | 1.298 |      - |         - |          NA |
| MathNet_FisherQuantile          | 2,252.24 ns | 118.215 ns |  6.480 ns | 0.904 | 0.0038 |     104 B |          NA |
| MetaNumerics_FisherQuantile     |   308.65 ns |  38.918 ns |  2.133 ns | 0.124 |      - |         - |          NA |
| Lodestar_ChiSquaredCdf          |    37.53 ns |   5.417 ns |  0.297 ns | 0.015 |      - |         - |          NA |
| MathNet_ChiSquaredCdf           |    51.34 ns |   6.389 ns |  0.350 ns | 0.021 |      - |         - |          NA |
| MetaNumerics_ChiSquaredCdf      |    69.20 ns |   1.017 ns |  0.056 ns | 0.028 |      - |         - |          NA |
| Lodestar_ChiSquaredQuantile     |   337.71 ns |  11.690 ns |  0.641 ns | 0.136 |      - |         - |          NA |
| MathNet_ChiSquaredQuantile      |   428.60 ns |  36.049 ns |  1.976 ns | 0.172 |      - |         - |          NA |
| MetaNumerics_ChiSquaredQuantile |   381.58 ns | 147.769 ns |  8.100 ns | 0.153 |      - |         - |          NA |
| Lodestar_NormalCdf              |    22.77 ns |   1.404 ns |  0.077 ns | 0.009 |      - |         - |          NA |
| MathNet_NormalCdf               |    19.30 ns |   2.034 ns |  0.111 ns | 0.008 |      - |         - |          NA |
| MetaNumerics_NormalCdf          |    56.30 ns |   0.682 ns |  0.037 ns | 0.023 |      - |         - |          NA |
| Lodestar_NormalQuantile         |    27.00 ns |   1.212 ns |  0.066 ns | 0.011 |      - |         - |          NA |
| MathNet_NormalQuantile          |    28.82 ns |   4.508 ns |  0.247 ns | 0.012 |      - |         - |          NA |
| MetaNumerics_NormalQuantile     |   159.08 ns |   0.856 ns |  0.047 ns | 0.064 |      - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                     | Mean      | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  30.62 ns | 3.203 ns | 0.176 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  13.18 ns | 2.840 ns | 0.156 ns |  0.43 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  37.67 ns | 5.686 ns | 0.312 ns |  1.23 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  65.79 ns | 0.871 ns | 0.048 ns |  2.15 |    0.01 |         - |          NA |
| ChiSquaredSfFractionalDf   | 270.43 ns | 1.321 ns | 0.072 ns |  8.83 |    0.04 |         - |          NA |
| NormalQuantile             |  26.87 ns | 0.542 ns | 0.030 ns |  0.88 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|----------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **32.92 μs** |   **5.847 μs** |  **0.320 μs** |  **1.00** |    **0.01** |  **0.4883** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  49.19 μs |   3.891 μs |  0.213 μs |  1.49 |    0.01 |  4.7607 |  0.1831 |  117.67 KB |        9.70 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **41.00 μs** |   **2.293 μs** |  **0.126 μs** |  **1.00** |    **0.00** |  **0.7324** |       **-** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 101.06 μs |   1.906 μs |  0.104 μs |  2.46 |    0.01 |  6.8359 |  0.3662 |  168.44 KB |        8.69 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **328.78 μs** |  **42.494 μs** |  **2.329 μs** |  **1.00** |    **0.01** |  **4.3945** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 367.05 μs |  62.704 μs |  3.437 μs |  1.12 |    0.01 | 35.6445 | 10.7422 |  881.77 KB |        7.98 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **396.99 μs** |  **15.441 μs** |  **0.846 μs** |  **1.00** |    **0.00** |  **6.8359** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 745.12 μs | 213.756 μs | 11.717 μs |  1.88 |    0.03 | 50.7813 | 20.5078 | 1265.51 KB |        7.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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
| **Fit**    | **1000**       |    **265.9 μs** |    **34.48 μs** |   **1.89 μs** |   **1.9531** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,801.6 μs** |   **352.83 μs** |  **19.34 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **28,649.8 μs** | **2,111.92 μs** | **115.76 μs** | **437.5000** | **437.5000** | **437.5000** | **5474.04 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmOffsetBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method              | SampleSize | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **29.07 μs** |     **0.351 μs** |   **0.019 μs** |  **1.00** |    **0.00** |   **0.4883** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    56.92 μs |     5.879 μs |   0.322 μs |  1.96 |    0.01 |   0.7935 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |            |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **3,794.86 μs** | **1,030.666 μs** |  **56.494 μs** |  **1.00** |    **0.02** | **328.1250** | **328.1250** | **328.1250** | **1097.72 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 6,914.06 μs | 1,862.791 μs | 102.106 μs |  1.82 |    0.03 | 500.0000 | 500.0000 | 500.0000 | 1877.48 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method | MeanCount | Mean     | Error     | StdDev   | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|----------:|---------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **303.2 μs** | **360.90 μs** | **19.78 μs** | **4.3945** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **286.6 μs** |   **7.07 μs** |  **0.39 μs** | **4.3945** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **303.4 μs** |   **5.07 μs** |  **0.28 μs** | **4.3945** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method       | SampleSize | Mean          | Error          | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **51.25 μs** |       **2.081 μs** |     **0.114 μs** |  **1.00** |    **0.00** |   **1.0986** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |      80.40 μs |       1.140 μs |     0.062 μs |  1.57 |    0.00 |   1.3428 |        - |        - |   32.97 KB |        1.22 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **699.55 μs** |     **285.969 μs** |    **15.675 μs** |  **1.00** |    **0.03** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,842.06 μs |      94.999 μs |     5.207 μs |  2.63 |    0.05 |  99.6094 |  99.6094 |  99.6094 |  369.43 KB |        1.10 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **7,007.87 μs** |     **922.459 μs** |    **50.563 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.21 KB** |        **1.00** |
| MathNet_Gls  | 500        |  25,900.46 μs |   9,805.733 μs |   537.485 μs |  3.70 |    0.07 | 281.2500 | 281.2500 | 281.2500 |  2228.6 KB |        1.11 |
|              |            |               |                |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **98,212.28 μs** |   **2,503.767 μs** |   **137.240 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** | **7924.15 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 245,881.61 μs | 111,197.640 μs | 6,095.118 μs |  2.50 |    0.05 |        - |        - |        - |  8898.8 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method    | SampleSize | Lags | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |    **37.57 μs** |   **3.409 μs** |  **0.187 μs** |  **1.00** |    **0.01** |   **0.0610** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    11.62 μs |   1.172 μs |  0.064 μs |  0.31 |    0.00 |   0.3357 |        - |        - |   8.45 KB |        3.19 |
| Hac       | 100        | 4    |    22.64 μs |   3.083 μs |  0.169 μs |  0.60 |    0.00 |   0.4883 |        - |        - |  12.63 KB |        4.77 |
| Cluster   | 100        | 4    |    10.40 μs |   0.193 μs |  0.011 μs |  0.28 |    0.00 |   0.3815 |        - |        - |   9.51 KB |        3.59 |
|           |            |      |             |            |           |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **546.27 μs** |  **17.529 μs** |  **0.961 μs** |  **1.00** |    **0.00** |   **2.9297** |        **-** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    |   961.38 μs |  87.826 μs |  4.814 μs |  1.76 |    0.01 | 123.0469 | 123.0469 | 123.0469 | 472.59 KB |        5.91 |
| Hac       | 10000      | 4    | 2,198.07 μs | 322.328 μs | 17.668 μs |  4.02 |    0.03 | 246.0938 | 246.0938 | 246.0938 | 863.57 KB |       10.80 |
| Cluster   | 10000      | 4    |   795.01 μs | 149.745 μs |  8.208 μs |  1.46 |    0.01 | 124.0234 | 124.0234 | 124.0234 | 564.97 KB |        7.06 |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **TwoSidedExact** | **999x1001** | **3,452.34 μs** | **413.997 μs** | **22.693 μs** |      **-** |  **39.28 KB** |
| **TwoSidedExact** | **99x101**   |    **48.67 μs** |   **2.297 μs** |  **0.126 μs** | **0.2441** |   **4.12 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **130.2 μs** |      **32.24 μs** |     **1.77 μs** |  **1.00** |    **0.02** |  **49.8047** |  **49.8047** |  **49.8047** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     231.6 μs |      18.36 μs |     1.01 μs |  1.78 |    0.02 |   1.2207 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     679.1 μs |     144.96 μs |     7.95 μs |  5.22 |    0.08 | 190.4297 | 190.4297 | 190.4297 |   705.49 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **86,041.6 μs** |   **8,310.36 μs** |   **455.52 μs** |  **1.00** |    **0.01** | **333.3333** | **333.3333** | **333.3333** |  **8865.82 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 356,982.6 μs |   4,424.47 μs |   242.52 μs |  4.15 |    0.02 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 521,270.7 μs | 171,078.38 μs | 9,377.38 μs |  6.06 |    0.10 |        - |        - |        - | 32666.45 KB |        3.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MannWhitneyExactBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method | Shape   | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **553.2 ms** | **125.1 ms** |  **6.86 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **457.4 ms** | **227.1 ms** | **12.45 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MathNetInteropBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method           | Size | Mean          | Error        | StdDev     | Gen0   | Gen1   | Gen2   | Allocated  |
|----------------- |----- |--------------:|-------------:|-----------:|-------:|-------:|-------:|-----------:|
| **Diagonal**         | **1000** |      **4.140 μs** |     **2.053 μs** |  **0.1126 μs** | **0.9613** | **0.0381** |      **-** |   **15.75 KB** |
| SparseDense      | 1000 |  1,577.648 μs |   242.911 μs | 13.3148 μs | 5.8594 |      - |      - |  126.59 KB |
| ToSparseSorted   | 1000 |     24.549 μs |     7.120 μs |  0.3903 μs | 7.4768 | 0.9155 |      - |  122.69 KB |
| ToSparseReversed | 1000 |     89.322 μs |    10.653 μs |  0.5839 μs | 7.6904 | 2.4414 |      - |  126.63 KB |
| **Diagonal**         | **4000** |     **16.468 μs** |     **3.384 μs** |  **0.1855 μs** | **3.8147** | **0.2136** |      **-** |   **62.63 KB** |
| SparseDense      | 4000 | 26,578.037 μs | 1,338.125 μs | 73.3471 μs |      - |      - |      - | 1909.55 KB |
| ToSparseSorted   | 4000 |    527.413 μs |   331.582 μs | 18.1751 μs | 7.8125 | 7.8125 | 7.8125 | 1893.94 KB |
| ToSparseReversed | 4000 |  1,356.136 μs |   343.140 μs | 18.8086 μs | 7.8125 | 7.8125 | 7.8125 | 1909.57 KB |

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

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method   | SampleSize | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|------------:|---------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **138.5 μs** |    **35.70 μs** |  **1.96 μs** |  **1.00** |    **0.02** |   **0.7324** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   882.0 μs |   103.01 μs |  5.65 μs |  6.37 |    0.09 |  59.5703 |   5.8594 |  1466.83 KB |       76.23 |
|          |            |            |             |          |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,352.0 μs** |   **166.87 μs** |  **9.15 μs** |  **1.00** |    **0.01** |   **3.9063** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 6,765.9 μs | 1,014.21 μs | 55.59 μs |  5.00 |    0.05 | 484.3750 | 234.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method       | SampleSize | Mean        | Error        | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **37.49 μs** |     **2.306 μs** |   **0.126 μs** |  **1.00** |    **0.00** |   **0.0610** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   224.12 μs |    10.812 μs |   0.593 μs |  5.98 |    0.02 |   1.7090 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    35.12 μs |     8.912 μs |   0.488 μs |  0.94 |    0.01 |   1.4038 |        - |        - |   35.26 KB |       13.31 |
|              |            |             |              |            |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **512.83 μs** |    **44.320 μs** |   **2.429 μs** |  **1.00** |    **0.01** |   **2.9297** |        **-** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 5,210.23 μs | 1,895.308 μs | 103.888 μs | 10.16 |    0.18 | 140.6250 | 132.8125 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 1,737.86 μs |   259.827 μs |  14.242 μs |  3.39 |    0.03 | 441.4063 | 441.4063 | 441.4063 | 1765.35 KB |       22.07 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method                       | Mean           | Error         | StdDev     | Ratio      | RatioSD   | Allocated | Alloc Ratio |
|----------------------------- |---------------:|--------------:|-----------:|-----------:|----------:|----------:|------------:|
| NormalQuantile               |      0.0416 ns |     0.1921 ns |  0.0105 ns |       1.05 |      0.33 |         - |          NA |
| NormalQuantileFarTail        |      0.0441 ns |     0.2280 ns |  0.0125 ns |       1.11 |      0.37 |         - |          NA |
| StudentQuantile              | 14,184.9290 ns | 1,436.1284 ns | 78.7190 ns | 356,738.00 | 79,773.70 |         - |          NA |
| StudentQuantileCauchyFarTail |  2,312.2612 ns |    69.7963 ns |  3.8258 ns |  58,151.26 | 13,000.92 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,189.4 μs** |   **206.69 μs** |  **11.33 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    308.6 μs |    10.12 μs |   0.55 μs |   2.9297 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    534.5 μs |    56.44 μs |   3.09 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | False |    935.9 μs |    71.59 μs |   3.92 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    494.0 μs |     6.67 μs |   0.37 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **680.1 μs** |    **16.02 μs** |   **0.88 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    281.6 μs |    15.81 μs |   0.87 μs |   2.9297 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    511.3 μs |   266.21 μs |  14.59 μs |  83.0078 |  83.0078 |  83.0078 |   741656 B |
| WilcoxonPooledDifferences | 10000      | True  |    800.8 μs |     4.39 μs |   0.24 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    436.7 μs |     4.21 μs |   0.23 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **12,805.9 μs** |   **413.08 μs** |  **22.64 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  3,568.7 μs |   257.11 μs |  14.09 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  4,807.1 μs | 2,671.02 μs | 146.41 μs | 656.2500 | 656.2500 | 656.2500 |  5062783 B |
| WilcoxonPooledDifferences | 100000     | False | 21,433.5 μs | 4,577.42 μs | 250.90 μs | 468.7500 | 468.7500 | 468.7500 | 19462711 B |
| MannWhitneyTest           | 100000     | False |  5,528.3 μs |   527.17 μs |  28.90 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **7,011.0 μs** |   **638.01 μs** |  **34.97 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  3,063.7 μs |   289.31 μs |  15.86 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  3,800.4 μs |   284.49 μs |  15.59 μs | 660.1563 | 660.1563 | 660.1563 |  5062782 B |
| WilcoxonPooledDifferences | 100000     | True  | 16,303.1 μs | 2,577.20 μs | 141.27 μs | 562.5000 | 562.5000 | 562.5000 | 19281722 B |
| MannWhitneyTest           | 100000     | True  |  4,454.6 μs |    95.55 μs |   5.24 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method | SampleSize | Covariance | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |    **37.55 μs** |   **2.468 μs** |  **0.135 μs** |   **0.0610** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |    **11.64 μs** |   **0.096 μs** |  **0.005 μs** |   **0.3357** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc1**        |    **11.67 μs** |   **0.308 μs** |  **0.017 μs** |   **0.3357** |        **-** |        **-** |    **8.4 KB** |
| **Fit**    | **100**        | **Hc2**        |    **13.61 μs** |   **0.891 μs** |  **0.049 μs** |   **0.3662** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **100**        | **Hc3**        |    **13.45 μs** |   **0.999 μs** |  **0.055 μs** |   **0.3662** |        **-** |        **-** |    **9.2 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **531.86 μs** |  **57.387 μs** |  **3.146 μs** |   **2.9297** |        **-** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        |   **974.96 μs** | **138.392 μs** |  **7.586 μs** | **124.0234** | **124.0234** | **124.0234** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc1**        |   **999.76 μs** | **324.708 μs** | **17.798 μs** | **123.0469** | **123.0469** | **123.0469** | **472.54 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,172.96 μs** | **104.643 μs** |  **5.736 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,144.59 μs** | **118.260 μs** |  **6.482 μs** | **123.0469** | **123.0469** | **123.0469** | **550.69 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method                         | SampleSize | Mean      | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |  **5.204 μs** | **0.6454 μs** | **0.0354 μs** |  **1.00** | **0.0992** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  6.240 μs | 0.1465 μs | 0.0080 μs |  1.20 | 0.0076 |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |  5.679 μs | 0.2571 μs | 0.0141 μs |  1.09 | 0.1068 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  7.084 μs | 0.5124 μs | 0.0281 μs |  1.36 | 0.0305 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |  5.749 μs | 0.4557 μs | 0.0250 μs |  1.10 | 0.0916 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  6.413 μs | 0.4575 μs | 0.0251 μs |  1.23 | 0.0076 |     224 B |        0.09 |
|                                |            |           |           |           |       |        |           |             |
| **LodestarAutocorrelation**        | **2000**       | **57.990 μs** | **2.0744 μs** | **0.1137 μs** |  **1.00** | **0.6714** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 65.002 μs | 0.7047 μs | 0.0386 μs |  1.12 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       | 58.573 μs | 0.7144 μs | 0.0392 μs |  1.01 | 0.6714 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 67.604 μs | 1.6412 μs | 0.0900 μs |  1.17 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       | 58.629 μs | 1.3651 μs | 0.0748 μs |  1.01 | 0.6104 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 65.441 μs | 6.7721 μs | 0.3712 μs |  1.13 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method                        | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |     **9.650 μs** |   **1.0262 μs** |  **0.0562 μs** |  **1.00** |    **0.01** |   **0.9766** |   **0.0153** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |    35.948 μs |   3.3427 μs |  0.1832 μs |  3.73 |    0.02 |   2.6855 |   0.2441 |        - |   66.67 KB |        2.78 |
| CortexAugmentedDickeyFuller   | 200        |    26.466 μs |   3.5720 μs |  0.1958 μs |  2.74 |    0.02 |   0.6409 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |     4.336 μs |   0.3598 μs |  0.0197 μs |  0.45 |    0.00 |   0.0687 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |     4.529 μs |   0.2009 μs |  0.0110 μs |  0.47 |    0.00 |   0.1373 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |     1.905 μs |   0.1046 μs |  0.0057 μs |  0.20 |    0.00 |   0.2632 |        - |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |     3.095 μs |   0.3096 μs |  0.0170 μs |  0.32 |    0.00 |   0.3319 |        - |        - |    8.16 KB |        0.34 |
|                               |            |              |             |            |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |   **128.047 μs** |  **21.2556 μs** |  **1.1651 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 1,085.687 μs | 383.3373 μs | 21.0120 μs |  8.48 |    0.16 | 248.0469 | 248.0469 | 248.0469 | 1001.38 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       |   293.701 μs |   4.3719 μs |  0.2396 μs |  2.29 |    0.02 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |   116.949 μs |   2.8074 μs |  0.1539 μs |  0.91 |    0.01 |   0.6104 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |   119.080 μs |  17.9361 μs |  0.9831 μs |  0.93 |    0.01 |   1.2207 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |    17.578 μs |   4.4142 μs |  0.2420 μs |  0.14 |    0.00 |   2.5330 |   0.2747 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |    30.160 μs |   2.7522 μs |  0.1509 μs |  0.24 |    0.00 |   3.1738 |   0.1831 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **6,051.1 ns** |     **206.76 ns** |     **11.33 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 100        |     30,577.1 ns |   6,402.94 ns |    350.97 ns |    5.05 |    0.05 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 100        |      2,354.0 ns |     360.17 ns |     19.74 ns |    0.39 |    0.00 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 100        |     23,560.6 ns |   3,711.16 ns |    203.42 ns |    3.89 |    0.03 |   0.9155 |   0.0305 |        - |   23336 B |      364.62 |
| LodestarChiSquare   | 100        |        111.2 ns |      10.47 ns |      0.57 ns |    0.02 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 100        |        210.2 ns |       4.80 ns |      0.26 ns |    0.03 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **66,193.9 ns** |     **825.78 ns** |     **45.26 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |      **64 B** |        **1.00** |
| AccordWelchT        | 10000      |    132,744.7 ns |   1,039.81 ns |     57.00 ns |   2.005 |    0.00 |        - |        - |        - |     392 B |        6.12 |
| LodestarMannWhitney | 10000      |    488,557.2 ns |   4,451.91 ns |    244.02 ns |   7.381 |    0.01 |        - |        - |        - |         - |        0.00 |
| AccordMannWhitney   | 10000      | 12,571,045.0 ns | 380,070.07 ns | 20,832.92 ns | 189.912 |    0.29 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |   35,019.02 |
| LodestarChiSquare   | 10000      |        111.3 ns |      11.18 ns |      0.61 ns |   0.002 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |
| AccordChiSquare     | 10000      |        211.3 ns |      14.88 ns |      0.82 ns |   0.003 |    0.00 |   0.0067 |        - |        - |     168 B |        2.62 |

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

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **27.16 μs** |   **5.242 μs** |  **0.287 μs** |   **1.9531** |   **0.1526** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **53.45 μs** |   **3.523 μs** |  **0.193 μs** |   **3.9063** |   **0.9155** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **82.52 μs** |   **0.976 μs** |  **0.053 μs** |   **4.3945** |   **1.3428** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **215.37 μs** |  **13.368 μs** |  **0.733 μs** |   **9.5215** |   **4.6387** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **303.54 μs** |  **73.611 μs** |  **4.035 μs** |  **73.7305** |  **73.7305** |  **73.7305** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **649.21 μs** |  **34.098 μs** |  **1.869 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **976.32 μs** | **119.363 μs** |  **6.543 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,906.76 μs** | **192.551 μs** | **10.554 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

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

| Method       | SampleSize | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **46.19 μs** |     **1.858 μs** |   **0.102 μs** |  **1.00** |    **0.00** |    **0.1221** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     20.46 μs |     3.799 μs |   0.208 μs |  0.44 |    0.00 |    2.4719 |    0.1221 |         - |    58.35 KB |       17.01 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |    **149.21 μs** |    **22.415 μs** |   **1.229 μs** |  **1.00** |    **0.01** |    **0.4883** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    136.54 μs |    28.515 μs |   1.563 μs |  0.92 |    0.01 |   21.7285 |    8.7891 |         - |   507.87 KB |       29.03 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |  **1,185.97 μs** |   **302.931 μs** |  **16.605 μs** |  **1.00** |    **0.02** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.49 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,711.55 μs |   645.256 μs |  35.369 μs |  1.44 |    0.03 | 1066.4063 | 1066.4063 | 1066.4063 |  5024.09 KB |       31.70 |
|              |            |              |              |            |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     | **10,125.20 μs** | **1,457.191 μs** |  **79.874 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.47 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 20,102.85 μs | 4,436.156 μs | 243.161 μs |  1.99 |    0.02 |  843.7500 |  843.7500 |  843.7500 | 50023.17 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method | SampleSize | Covariates | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **460.8 μs** |   **211.19 μs** | **11.58 μs** |   **5.3711** |   **0.4883** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **1,828.3 μs** |    **76.29 μs** |  **4.18 μs** |   **7.8125** |   **1.9531** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,261.4 μs** |   **164.79 μs** |  **9.03 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **16,333.8 μs** | **1,546.84 μs** | **84.79 μs** | **250.0000** | **250.0000** | **250.0000** | **1737.34 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.78 μs** |     **2.317 μs** |  **0.127 μs** |  **1.00** |    **0.6409** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     14.67 μs |     0.788 μs |  0.043 μs |  0.87 |    0.6104 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     20.60 μs |     2.372 μs |  0.130 μs |  1.23 |    1.0681 |    0.0305 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **63.93 μs** |    **12.199 μs** |  **0.669 μs** |  **1.00** |    **2.9297** |    **0.3662** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.93 μs |     2.862 μs |  0.157 μs |  0.37 |    2.5330 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     45.32 μs |     1.307 μs |  0.072 μs |  0.71 |    2.0142 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **7,655.18 μs** |   **166.951 μs** |  **9.151 μs** |  **1.00** |  **328.1250** |  **320.3125** | **320.3125** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  7,566.64 μs |   366.431 μs | 20.085 μs |  0.99 |  328.1250 |  320.3125 | 320.3125 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               |  8,399.18 μs |   252.646 μs | 13.848 μs |  1.10 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **14,295.32 μs** | **1,343.338 μs** | **73.633 μs** |  **1.00** | **1078.1250** | **1062.5000** | **984.3750** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 11,301.06 μs |   796.977 μs | 43.685 μs |  0.79 |  750.0000 |  734.3750 | 656.2500 | 5660.21 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 14,243.18 μs | 1,586.919 μs | 86.984 μs |  1.00 |  546.8750 |  546.8750 | 546.8750 | 4916.41 KB |        0.74 |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1     | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|---------:|---------:|----------:|
| CountLoadAsync2M | 24.13 ms |  4.321 ms | 0.237 ms |  656.2500 | 625.0000 | 375.0000 |  21.53 MB |
| CountLoadAsync4M | 51.07 ms | 10.282 ms | 0.564 ms | 1000.0000 | 909.0909 | 454.5455 |  43.93 MB |
| CountLoad4M      | 49.34 ms | 43.200 ms | 2.368 ms | 1000.0000 | 909.0909 | 454.5455 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                | Mean          | Error         | StdDev       | Gen0     | Gen1     | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|---------:|---------:|---------:|------------:|
| CountFitLongDocuments | 192,642.31 μs | 34,284.847 μs | 1,879.268 μs |        - |        - |        - | 54334.51 KB |
| CountSave240K         |     401.93 μs |     74.010 μs |     4.057 μs |   7.3242 |   2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     471.77 μs |     35.127 μs |     1.925 μs |   7.8125 |   3.9063 |   3.9063 |   739.43 KB |
| TfidfSave45K          |      55.88 μs |     24.415 μs |     1.338 μs |   5.5542 |   0.9155 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      56.27 μs |      2.564 μs |     0.141 μs |   7.3853 |        - |        - |   181.88 KB |
| IndexSave942K         |     279.51 μs |     37.371 μs |     2.048 μs |   5.3711 |   0.9766 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     295.28 μs |     24.211 μs |     1.327 μs |   5.3711 |   0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  28,722.54 μs | 33,441.569 μs | 1,833.045 μs | 437.5000 | 406.2500 | 156.2500 | 22049.27 KB |
| CountLoadAsync4M      |  59,417.75 μs | 32,540.325 μs | 1,783.645 μs | 777.7778 | 666.6667 | 222.2222 | 44983.41 KB |
| CountLoad4M           |  56,457.16 μs | 35,869.661 μs | 1,966.137 μs | 700.0000 | 600.0000 | 200.0000 |  44976.7 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                 | Mean          | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|---------:|---------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     16.677 μs |     4.784 μs |   0.2622 μs |   1.0986 |   0.0610 |        - |    27.43 KB |
| CountSaveAsyncWide     |    517.251 μs |   103.771 μs |   5.6881 μs |  33.2031 |  29.2969 |  29.2969 |   740.17 KB |
| HashingLoadUndeclared  |      6.766 μs |     2.922 μs |   0.1602 μs |   3.4103 |        - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     28.637 μs |     8.744 μs |   0.4793 μs |   4.9438 |   0.7935 |        - |   121.83 KB |
| CountLoadUndeclared24K |    137.746 μs |    20.205 μs |   1.1075 μs |  10.7422 |   1.7090 |        - |   264.67 KB |
| CountLoadUndeclared78K |    906.426 μs |   493.733 μs |  27.0632 μs |  25.3906 |  23.4375 |   5.8594 |   656.32 KB |
| CountLoadUndeclared1M  | 10,126.691 μs | 1,883.210 μs | 103.2251 μs | 437.5000 | 406.2500 | 312.5000 | 10793.88 KB |
| CountSaveAsync1M       |  2,199.012 μs | 1,047.392 μs |  57.4111 μs | 117.1875 | 113.2813 | 113.2813 |     3162 KB |
| IndexSaveAsyncLongHead |  3,462.120 μs |   521.500 μs |  28.5852 μs |  78.1250 |  74.2188 |  74.2188 |   2124.2 KB |
| CountLoadUndeclared2M  | 20,871.457 μs | 1,747.732 μs |  95.7991 μs | 875.0000 | 843.7500 | 593.7500 | 22045.13 KB |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.733 μs** |      **0.2009 μs** |     **0.0110 μs** |     **1.00** |    **0.01** |    **0.0153** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.328 μs |      0.1318 μs |     0.0072 μs |     1.34 |    0.01 |    0.0153 |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.360 μs |      0.2416 μs |     0.0132 μs |     1.94 |    0.01 |    0.2060 |         - |         - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   3,546.187 μs |    441.4643 μs |    24.1981 μs | 2,046.22 |   16.49 |  898.4375 |  882.8125 |  882.8125 |  3723236 B |    8,781.22 |
| LuceneFromText         | 1000      |   7,412.038 μs |    408.4636 μs |    22.3893 μs | 4,276.89 |   25.98 |   46.8750 |   31.2500 |         - |  1375073 B |    3,243.10 |
|                        |           |                |                |               |          |         |           |           |           |            |             |
| **LodestarQuery**          | **20000**     |      **24.647 μs** |      **4.5962 μs** |     **0.2519 μs** |     **1.00** |    **0.01** |         **-** |         **-** |         **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      37.515 μs |      5.1664 μs |     0.2832 μs |     1.52 |    0.02 |         - |         - |         - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      17.936 μs |      0.9837 μs |     0.0539 μs |     0.73 |    0.01 |    0.3357 |         - |         - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  74,196.263 μs | 32,222.2065 μs | 1,766.2079 μs | 3,010.61 |   67.49 | 1571.4286 | 1571.4286 | 1571.4286 | 61663530 B |  145,432.85 |
| LuceneFromText         | 20000     | 149,525.658 μs | 31,882.0256 μs | 1,747.5614 μs | 6,067.20 |   81.39 |  750.0000 |  500.0000 |         - | 22423360 B |   52,885.28 |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method                         | Samples | Clusters | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.151 ms** |  **0.0718 ms** | **0.0039 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.174 ms |  0.4616 ms | 0.0253 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  20.536 ms |  0.7474 ms | 0.0410 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.009 ms** |  **1.1113 ms** | **0.0609 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.244 ms |  0.6584 ms | 0.0361 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 192.981 ms | 31.1314 ms | 1.7064 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **Lodestar**            | **12**     |        **871.6 ns** |       **215.99 ns** |      **11.84 ns** |  **1.00** |    **0.02** |   **0.0496** |        **-** |        **-** |     **840 B** |        **1.00** |
| F23StringSimilarity | 12     |      1,788.2 ns |        35.93 ns |       1.97 ns |  2.05 |    0.02 |   0.0896 |        - |        - |    1520 B |        1.81 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **120**    |     **68,075.3 ns** |     **6,336.81 ns** |     **347.34 ns** |  **1.00** |    **0.01** |   **0.1221** |        **-** |        **-** |    **2128 B** |        **1.00** |
| F23StringSimilarity | 120    |    153,059.4 ns |    16,123.66 ns |     883.79 ns |  2.25 |    0.01 |   3.4180 |        - |        - |   61064 B |       28.70 |
|                     |        |                 |                 |               |       |         |          |          |          |           |             |
| **Lodestar**            | **1000**   |  **4,322,355.7 ns** | **2,128,783.87 ns** | **116,685.83 ns** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** |    **5656 B** |        **1.00** |
| F23StringSimilarity | 1000   | 12,247,365.2 ns |   630,251.87 ns |  34,546.23 ns |  2.83 |    0.07 | 984.3750 | 984.3750 | 984.3750 | 4018553 B |      710.49 |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method      | Count  | Mean       | Error    | StdDev  | Allocated |
|------------ |------- |-----------:|---------:|--------:|----------:|
| **SearchTop10** | **10000**  |   **522.3 μs** | **31.89 μs** | **1.75 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **8,663.0 μs** | **87.87 μs** | **4.82 μs** |   **1.64 KB** |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-WUJWRU : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-WUJWRU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **345.13 μs** |    **55.936 μs** |    **62.172 μs** |  **1.03** |    **0.25** |        **-** |        **-** |        **-** |   **166.8 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    263.12 μs |    19.087 μs |    19.601 μs |  0.79 |    0.14 |        - |        - |        - |   17.84 KB |        0.11 |
| Lodestar_Impute                 | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |     74.63 μs |     6.632 μs |     7.096 μs |  0.22 |    0.04 |        - |        - |        - |   32.18 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    357.81 μs |    23.264 μs |    26.791 μs |  1.07 |    0.20 |        - |        - |        - |   34.19 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,583.94 μs |    92.747 μs |   103.088 μs |  4.73 |    0.85 |        - |        - |        - |  289.64 KB |        1.74 |
| MlNet_ReplaceMissingValues_Read | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,186.03 μs |    71.637 μs |    79.624 μs |  3.54 |    0.64 |        - |        - |        - |  309.91 KB |        1.86 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    170.33 μs |    10.573 μs |     0.580 μs |  1.00 |    0.00 |  49.8047 |  49.8047 |  49.8047 |  165.86 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     85.92 μs |     5.619 μs |     0.308 μs |  0.50 |    0.00 |   0.6104 |        - |        - |   16.91 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     16.43 μs |     2.299 μs |     0.126 μs |  0.10 |    0.00 |   1.2512 |        - |        - |   31.42 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,137.44 μs | 3,317.032 μs |   181.818 μs |  6.68 |    0.92 |  40.0391 |   6.8359 |        - |  986.36 KB |        5.95 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,014.02 μs | 1,308.978 μs |    71.749 μs |  5.95 |    0.37 |  31.2500 |   5.8594 |        - |   792.4 KB |        4.78 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    246.79 μs |    63.458 μs |     3.478 μs |  1.45 |    0.02 |  12.2070 |  11.7188 |        - |  291.22 KB |        1.76 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-WUJWRU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **5,620.41 μs** |   **322.909 μs** |   **302.049 μs** |  **1.00** |    **0.08** |        **-** |        **-** |        **-** | **3283.98 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,766.77 μs |   144.852 μs |   142.264 μs |  0.85 |    0.06 |        - |        - |        - |  314.71 KB |        0.10 |
| Lodestar_Impute                 | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |    600.50 μs |    14.254 μs |    16.415 μs |  0.11 |    0.01 |        - |        - |        - |  625.65 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,579.04 μs |    25.307 μs |    29.143 μs |  0.64 |    0.04 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,922.72 μs | 3,241.637 μs | 3,733.074 μs |  2.31 |    0.67 |        - |        - |        - |  496.45 KB |        0.15 |
| MlNet_ReplaceMissingValues_Read | Job-WUJWRU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,325.13 μs | 3,834.239 μs | 4,261.746 μs |  1.66 |    0.75 |        - |        - |        - |  445.48 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,223.81 μs |   522.981 μs |    28.666 μs |  1.00 |    0.01 | 984.3750 | 984.3750 | 984.3750 | 3291.54 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,703.20 μs |   200.414 μs |    10.985 μs |  0.88 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.42 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    395.99 μs |   107.622 μs |     5.899 μs |  0.09 |    0.00 | 115.2344 | 115.2344 | 115.2344 |  626.12 KB |        0.19 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,590.77 μs | 1,412.537 μs |    77.426 μs |  0.38 |    0.02 |  19.5313 |   3.9063 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,211.39 μs |   961.341 μs |    52.694 μs |  1.00 |    0.01 |  23.4375 |        - |        - |  568.95 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,426.21 μs | 1,579.508 μs |    86.578 μs |  0.57 |    0.02 |  19.5313 |   7.8125 |        - |  472.64 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method          | Records | Mean        | Error       | StdDev    | Gen0   | Allocated  |
|---------------- |-------- |------------:|------------:|----------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **462.8 μs** |    **74.48 μs** |   **4.08 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    602.1 μs |    53.62 μs |   2.94 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    612.0 μs |    74.05 μs |   4.06 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,376.1 μs |   632.50 μs |  34.67 μs | 9.7656 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **9,152.8 μs** | **1,440.10 μs** |  **78.94 μs** |      **-** |    **7.19 KB** |
| Unfiltered      | 100000  |  8,977.5 μs |   837.91 μs |  45.93 μs |      - |    2.94 KB |
| HybridSelective | 100000  | 10,012.5 μs | 2,243.93 μs | 123.00 μs |      - |   15.15 KB |
| HybridBroad     | 100000  | 17,400.6 μs | 1,840.68 μs | 100.89 μs |      - | 3155.14 KB |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method             | Length | Distinct | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **374.0 ns** |     **58.26 ns** |   **3.19 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     220.5 ns |      1.69 ns |   0.09 ns |  0.59 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **374.5 ns** |     **86.97 ns** |   **4.77 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     228.2 ns |      4.75 ns |   0.26 ns |  0.61 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **457.6 ns** |     **17.84 ns** |   **0.98 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     307.9 ns |     18.96 ns |   1.04 ns |  0.67 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **459.7 ns** |     **23.70 ns** |   **1.30 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     306.3 ns |     28.98 ns |   1.59 ns |  0.67 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **549.2 ns** |     **53.77 ns** |   **2.95 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     406.5 ns |     37.73 ns |   2.07 ns |  0.74 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **552.9 ns** |     **30.63 ns** |   **1.68 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     394.5 ns |      6.44 ns |   0.35 ns |  0.71 |    0.00 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **649.5 ns** |     **74.61 ns** |   **4.09 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,261.9 ns |    135.41 ns |   7.42 ns |  1.94 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **628.8 ns** |      **2.47 ns** |   **0.14 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,276.8 ns |     77.33 ns |   4.24 ns |  2.03 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,050.2 ns** |     **56.49 ns** |   **3.10 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,689.4 ns |  2,129.35 ns | 116.72 ns |  2.78 |    0.05 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,048.7 ns** |    **426.40 ns** |  **23.37 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,384.9 ns |    613.67 ns |  33.64 ns |  2.63 |    0.03 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **16,718.7 ns** |  **1,250.34 ns** |  **68.54 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  65,239.8 ns |  1,288.21 ns |  70.61 ns |  3.90 |    0.01 |         - |          NA |
|                    |        |          |              |              |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **482,347.2 ns** | **18,004.24 ns** | **986.87 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  65,858.7 ns | 11,158.54 ns | 611.64 ns |  0.14 |    0.00 |         - |          NA |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method            | Labels | Mean     | Error     | StdDev   | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|----------:|---------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **531.7 μs** | **958.18 μs** | **52.52 μs** |  **1.9531** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 505.5 μs |  20.08 μs |  1.10 μs |  1.9531 |       - |  67.71 KB |
| ReportRows        | 1000   | 538.8 μs |  65.19 μs |  3.57 μs |  5.8594 |  0.9766 | 153.99 KB |
| **F1Macro**           | **5000**   | **642.6 μs** |  **36.20 μs** |  **1.98 μs** | **13.6719** |  **3.9063** | **337.24 KB** |
| PrecisionWeighted | 5000   | 654.7 μs |  55.69 μs |  3.05 μs | 13.6719 |  3.9063 | 337.24 KB |
| ReportRows        | 5000   | 797.0 μs |  84.78 μs |  4.65 μs | 31.2500 | 30.2734 | 767.27 KB |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,466.0 ns** |       **104.28 ns** |       **5.72 ns** | **0.0153** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     6,715.4 ns |       632.12 ns |      34.65 ns | 0.0153 |     400 B |
| AccuracyScore  | 1000    | 2       |       156.8 ns |        11.07 ns |       0.61 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,740.4 ns |     1,018.46 ns |      55.83 ns | 0.0229 |     656 B |
| Report         | 1000    | 2       |     6,343.6 ns |     1,004.71 ns |      55.07 ns | 0.2060 |    5336 B |
| **Matrix**         | **1000**    | **10**      |     **3,862.7 ns** |       **707.04 ns** |      **38.76 ns** | **0.0458** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     6,926.9 ns |       166.63 ns |       9.13 ns | 0.0458 |    1336 B |
| AccuracyScore  | 1000    | 10      |       155.9 ns |         1.19 ns |       0.07 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,952.8 ns |     1,056.46 ns |      57.91 ns | 0.0687 |    1880 B |
| Report         | 1000    | 10      |    10,761.6 ns |     1,941.14 ns |     106.40 ns | 0.4883 |   12360 B |
| **Matrix**         | **100000**  | **2**       |   **340,848.1 ns** |     **4,602.11 ns** |     **252.26 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   774,591.4 ns |    83,403.65 ns |   4,571.64 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |    16,610.8 ns |       181.16 ns |       9.93 ns |      - |         - |
| F1Macro        | 100000  | 2       |   337,097.0 ns |    55,747.35 ns |   3,055.70 ns |      - |     656 B |
| Report         | 100000  | 2       |   354,250.5 ns |     7,263.46 ns |     398.13 ns |      - |    5360 B |
| **Matrix**         | **100000**  | **10**      |   **359,011.5 ns** |    **45,177.87 ns** |   **2,476.35 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   913,320.8 ns |   127,660.84 ns |   6,997.52 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |    17,192.6 ns |     2,100.02 ns |     115.11 ns |      - |         - |
| F1Macro        | 100000  | 10      |   330,970.0 ns |    34,096.94 ns |   1,868.97 ns |      - |    1880 B |
| Report         | 100000  | 10      |   339,835.2 ns |    32,229.25 ns |   1,766.59 ns | 0.4883 |   12704 B |
| **Matrix**         | **1000000** | **2**       | **3,394,906.8 ns** |   **768,389.59 ns** |  **42,118.03 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,951,105.9 ns |   875,275.12 ns |  47,976.78 ns |      - |     412 B |
| AccuracyScore  | 1000000 | 2       |   274,458.0 ns |   136,475.54 ns |   7,480.68 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,517,982.6 ns |   443,987.03 ns |  24,336.43 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,473,116.6 ns |   223,148.46 ns |  12,231.52 ns |      - |    5379 B |
| **Matrix**         | **1000000** | **10**      | **3,460,124.5 ns** |    **46,398.27 ns** |   **2,543.25 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 9,055,219.0 ns | 3,066,021.61 ns | 168,058.99 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |   278,159.3 ns |   296,620.63 ns |  16,258.78 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,261,984.5 ns |   583,189.36 ns |  31,966.58 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,409,024.5 ns |   362,565.72 ns |  19,873.45 ns |      - |   12747 B |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **77.50 ns** |      **3.825 ns** |   **0.210 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     78.56 ns |     28.201 ns |   1.546 ns |  1.01 |    0.02 |         - |          NA |
| Dp_Cjk     | 4    |     77.23 ns |      9.630 ns |   0.528 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |     76.40 ns |      1.327 ns |   0.073 ns |  0.99 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **6**    |    **107.36 ns** |      **7.868 ns** |   **0.431 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     77.34 ns |     16.091 ns |   0.882 ns |  0.72 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    106.21 ns |      8.911 ns |   0.488 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    130.84 ns |      3.362 ns |   0.184 ns |  1.22 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **8**    |    **150.26 ns** |      **3.678 ns** |   **0.202 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     89.52 ns |     89.954 ns |   4.931 ns |  0.60 |    0.03 |         - |          NA |
| Dp_Cjk     | 8    |    150.26 ns |      1.580 ns |   0.087 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    180.55 ns |     22.823 ns |   1.251 ns |  1.20 |    0.01 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **10**   |    **196.97 ns** |      **7.521 ns** |   **0.412 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |     94.67 ns |      0.941 ns |   0.052 ns |  0.48 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    198.93 ns |     30.215 ns |   1.656 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    136.71 ns |      1.589 ns |   0.087 ns |  0.69 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **279.64 ns** |     **21.008 ns** |   **1.152 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    103.54 ns |      1.915 ns |   0.105 ns |  0.37 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    279.12 ns |     36.979 ns |   2.027 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    150.24 ns |      6.690 ns |   0.367 ns |  0.54 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **423.49 ns** |     **47.896 ns** |   **2.625 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    118.72 ns |      1.580 ns |   0.087 ns |  0.28 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    421.60 ns |     31.102 ns |   1.705 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    169.74 ns |      3.813 ns |   0.209 ns |  0.40 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **815.39 ns** |      **4.878 ns** |   **0.267 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    152.73 ns |      1.608 ns |   0.088 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    820.88 ns |     48.962 ns |   2.684 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    214.11 ns |     10.552 ns |   0.578 ns |  0.26 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,437.11 ns** |    **156.309 ns** |   **8.568 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 32   |    187.14 ns |     17.647 ns |   0.967 ns |  0.13 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,573.30 ns |  1,182.981 ns |  64.843 ns |  1.09 |    0.04 |         - |          NA |
| Kernel_Cjk | 32   |    255.88 ns |      1.903 ns |   0.104 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **3,153.71 ns** |    **103.958 ns** |   **5.698 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    254.51 ns |      2.160 ns |   0.118 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,174.12 ns |  1,235.550 ns |  67.725 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    341.49 ns |      1.974 ns |   0.108 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **5,495.77 ns** |    **254.486 ns** |  **13.949 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    323.57 ns |     30.515 ns |   1.673 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,490.43 ns |    344.785 ns |  18.899 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |    432.28 ns |      4.528 ns |   0.248 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **12,283.30 ns** |    **407.397 ns** |  **22.331 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    814.90 ns |    115.770 ns |   6.346 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 13,025.43 ns | 16,099.213 ns | 882.452 ns |  1.06 |    0.06 |         - |          NA |
| Kernel_Cjk | 96   |  1,545.03 ns |    187.172 ns |  10.260 ns |  0.13 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **Distance_Utf16**     | **8**      |     **38.54 ns** |      **0.488 ns** |   **0.027 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    129.45 ns |      7.306 ns |   0.400 ns |  3.36 |    0.01 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **158.14 ns** |    **115.448 ns** |   **6.328 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,385.39 ns |    476.885 ns |  26.140 ns |  8.77 |    0.33 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **324.30 ns** |     **20.775 ns** |   **1.139 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  8,708.20 ns |  2,109.838 ns | 115.647 ns | 26.85 |    0.32 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **50,206.33 ns** | **17,000.234 ns** | **931.840 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 49,647.82 ns |  4,185.563 ns | 229.425 ns |  0.99 |    0.02 |         - |          NA |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **8.698 μs** |  **0.5907 μs** | **0.0324 μs** |         **-** |
| EqualLength | 65           |     3.096 μs |  0.0357 μs | 0.0020 μs |         - |
| **Embedded**    | **128**          |    **31.264 μs** |  **4.4754 μs** | **0.2453 μs** |         **-** |
| EqualLength | 128          |     5.745 μs |  0.4972 μs | 0.0273 μs |         - |
| **Embedded**    | **512**          | **1,182.206 μs** | **46.7120 μs** | **2.5604 μs** |         **-** |
| EqualLength | 512          |    44.374 μs |  1.4707 μs | 0.0806 μs |         - |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  4.357 ms |  0.1418 ms | 0.0078 ms |  78.1250 |  70.3125 |  23.4375 |  3711.58 KB |
| TokenizerJsonWordPiece | 11.687 ms |  0.9886 ms | 0.0542 ms | 125.0000 | 109.3750 |  31.2500 |  5852.26 KB |
| TokenizerJsonUnigram   | 12.182 ms |  1.0729 ms | 0.0588 ms |  62.5000 |  46.8750 |  15.6250 |   4748.9 KB |
| SpieceModel            |  4.981 ms |  4.0049 ms | 0.2195 ms |  78.1250 |  70.3125 |  31.2500 |  4292.23 KB |
| TfidfSave              |  1.604 ms |  0.2307 ms | 0.0126 ms |  19.5313 |  15.6250 |  15.6250 |  2137.06 KB |
| TfidfLoad              |  4.511 ms |  0.1915 ms | 0.0105 ms |  46.8750 |  39.0625 |   7.8125 |  2921.36 KB |
| EmbeddingIndexSave     |  5.875 ms |  2.4848 ms | 0.1362 ms | 257.8125 | 257.8125 | 257.8125 | 20349.96 KB |
| EmbeddingIndexLoad     |  8.607 ms |  1.5290 ms | 0.0838 ms | 125.0000 |  93.7500 |  93.7500 | 16094.43 KB |
| EmbeddingIndexSaveFile | 49.577 ms | 11.6901 ms | 0.6408 ms |        - |        - |        - |   323.43 KB |
| EmbeddingIndexLoadGzip | 73.622 ms |  5.6850 ms | 0.3116 ms |        - |        - |        - | 16095.25 KB |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **JaccardSimilarity** | **1** | **Utf16Unit** | **4.768 μs** | **0.3152 μs** | **0.0173 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **1** | **CodePoint** | **4.009 μs** | **1.5858 μs** | **0.0869 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **Utf16Unit** | **5.854 μs** | **0.3659 μs** | **0.0201 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **CodePoint** | **4.874 μs** | **2.1363 μs** | **0.1171 μs** | **0.0076** |     **192 B** |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method                   | Length | Mean         | Error         | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|--------------:|------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **375.1 ns** |      **31.39 ns** |     **1.72 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   6,059.2 ns |     553.83 ns |    30.36 ns | 16.15 |    0.09 |         - |          NA |
|                          |        |              |               |             |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **22,779.6 ns** |     **995.95 ns** |    **54.59 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 736,186.8 ns | 159,391.85 ns | 8,736.81 ns | 32.32 |    0.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **59.31 μs** |     **3.610 μs** |  **0.198 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    57.21 μs |     3.454 μs |  0.189 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   141.67 μs |     6.649 μs |  0.364 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   547.82 μs |    74.467 μs |  4.082 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **718.23 μs** |   **929.748 μs** | **50.963 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   606.64 μs |    26.575 μs |  1.457 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,525.19 μs |   241.739 μs | 13.251 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 7,464.44 μs | 1,343.387 μs | 73.636 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **Fit**    | **2000x2000x20** | **23.94 ms** | **0.583 ms** | **0.032 ms** | **93.7500** | **93.7500** | **93.7500** | **367.51 KB** |
| **Fit**    | **500x20000x5**  | **43.30 ms** | **0.438 ms** | **0.024 ms** |       **-** |       **-** |       **-** | **414.36 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-YEIKII : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean          | Error         | StdDev        | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |--------------:|--------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_MinMax**                   | **Job-YEIKII** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **99.045 μs** |     **4.1561 μs** |     **4.4470 μs** |  **1.00** |    **0.06** |        **-** |        **-** |        **-** |   **81632 B** |        **1.00** |
| Lodestar_MaxAbs                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     94.099 μs |     7.0902 μs |     7.8807 μs |  0.95 |    0.09 |        - |        - |        - |   81096 B |        0.99 |
| Lodestar_Robust                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,045.478 μs |    31.0013 μs |    35.7012 μs | 20.69 |    0.92 |        - |        - |        - |  161352 B |        1.98 |
| Lodestar_Standard                 | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |    102.181 μs |     6.7362 μs |     7.4873 μs |  1.03 |    0.09 |        - |        - |        - |   81560 B |        1.00 |
| Lodestar_Standard_SparseFit       | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     70.576 μs |    11.2517 μs |    12.0392 μs |  0.71 |    0.12 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseFit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     62.455 μs |     1.8690 μs |     1.8356 μs |  0.63 |    0.03 |        - |        - |        - |         - |        0.00 |
| Lodestar_MaxAbs_SparseClip        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |     91.043 μs |     7.1932 μs |     7.0647 μs |  0.92 |    0.08 |        - |        - |        - |   27856 B |        0.34 |
| MlNet_NormalizeMinMax_Fit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |    352.243 μs |    15.1314 μs |    17.4253 μs |  3.56 |    0.23 |        - |        - |        - |    9528 B |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,242.920 μs |   139.1484 μs |   154.6631 μs | 22.69 |    1.79 |        - |        - |        - |  331160 B |        4.06 |
| MlNet_NormalizeRobustScaling_Read | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 1000     |  5,444.374 μs |    90.4801 μs |   100.5684 μs | 55.07 |    2.46 |        - |        - |        - |  461776 B |        5.66 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     47.087 μs |    10.1848 μs |     0.5583 μs |  1.00 |    0.01 |   3.1738 |        - |        - |   80896 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     40.940 μs |     7.9238 μs |     0.4343 μs |  0.87 |    0.01 |   3.1738 |        - |        - |   80336 B |       0.993 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    451.769 μs |    17.2377 μs |     0.9449 μs |  9.60 |    0.10 |   6.3477 |        - |        - |  160616 B |       1.985 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     55.609 μs |     0.8095 μs |     0.0444 μs |  1.18 |    0.01 |   3.1738 |        - |        - |   80824 B |       0.999 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      9.808 μs |     0.3594 μs |     0.0197 μs |  0.21 |    0.00 |   0.0305 |        - |        - |     824 B |       0.010 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |      6.926 μs |     0.8193 μs |     0.0449 μs |  0.15 |    0.00 |   0.0076 |        - |        - |     376 B |       0.005 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     11.975 μs |     1.4421 μs |     0.0790 μs |  0.25 |    0.00 |   1.0681 |   0.0153 |        - |   27120 B |       0.335 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,856.174 μs | 5,034.6519 μs |   275.9663 μs | 39.42 |    5.09 |  55.6641 |   7.3242 |        - | 1398987 B |      17.294 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,150.716 μs |   505.7121 μs |    27.7198 μs | 24.44 |    0.57 |  25.3906 |        - |        - |  655969 B |       8.109 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,737.889 μs | 3,038.8128 μs |   166.5676 μs | 36.91 |    3.09 |  23.4375 |        - |        - |  623401 B |       7.706 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| **Lodestar_MinMax**                   | **Job-YEIKII** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,364.599 μs** |    **13.4171 μs** |    **14.3561 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1601632 B** |       **1.000** |
| Lodestar_MaxAbs                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,251.493 μs |    12.4791 μs |    13.3525 μs |  0.92 |    0.01 |        - |        - |        - | 1601096 B |       1.000 |
| Lodestar_Robust                   | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 14,338.034 μs |    61.0404 μs |    65.3125 μs | 10.51 |    0.12 |        - |        - |        - | 3201352 B |       1.999 |
| Lodestar_Standard                 | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,486.996 μs |    18.6393 μs |    21.4650 μs |  1.09 |    0.02 |        - |        - |        - | 1601272 B |       1.000 |
| Lodestar_Standard_SparseFit       | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    448.651 μs |    11.8778 μs |    13.2021 μs |  0.33 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseFit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    398.089 μs |    16.7585 μs |    19.2992 μs |  0.29 |    0.01 |        - |        - |        - |         - |       0.000 |
| Lodestar_MaxAbs_SparseClip        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |    489.546 μs |    17.9825 μs |    19.2410 μs |  0.36 |    0.01 |        - |        - |        - |  559752 B |       0.349 |
| MlNet_NormalizeMinMax_Fit         | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    |  5,376.579 μs |   424.4134 μs |   488.7551 μs |  3.94 |    0.35 |        - |        - |        - |    9528 B |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 22,105.078 μs | 3,375.4475 μs | 3,887.1703 μs | 16.20 |    2.79 |        - |        - |        - |  435712 B |       0.272 |
| MlNet_NormalizeRobustScaling_Read | Job-YEIKII | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,263.635 μs |   323.0295 μs |   359.0464 μs | 14.12 |    0.29 |        - |        - |        - | 4277384 B |       2.671 |
|                                   |            |                 |                |             |              |             |          |               |               |               |       |         |          |          |          |           |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,186.825 μs |   572.3945 μs |    31.3749 μs |  1.00 |    0.03 | 330.0781 | 330.0781 | 330.0781 | 1601382 B |       1.000 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,083.402 μs | 1,012.2115 μs |    55.4827 μs |  0.91 |    0.05 | 330.0781 | 330.0781 | 330.0781 | 1600551 B |       0.999 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,779.375 μs | 1,079.3633 μs |    59.1635 μs | 11.62 |    0.27 | 875.0000 | 875.0000 | 875.0000 | 3201194 B |       1.999 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,449.311 μs | 1,714.8171 μs |    93.9949 μs |  1.22 |    0.07 | 330.0781 | 330.0781 | 330.0781 | 1601054 B |       1.000 |
| Lodestar_Standard_SparseFit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    340.474 μs |    53.6593 μs |     2.9412 μs |  0.29 |    0.01 |        - |        - |        - |     824 B |       0.001 |
| Lodestar_MaxAbs_SparseFit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    283.694 μs |    56.4080 μs |     3.0919 μs |  0.24 |    0.01 |        - |        - |        - |     376 B |       0.000 |
| Lodestar_MaxAbs_SparseClip        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    428.826 μs |   268.7840 μs |    14.7330 μs |  0.36 |    0.01 | 126.9531 | 124.0234 | 124.0234 |  560100 B |       0.350 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,373.617 μs |   811.7776 μs |    44.4963 μs |  1.16 |    0.04 |  13.6719 |   3.9063 |        - |  354153 B |       0.221 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,605.213 μs | 1,793.4379 μs |    98.3044 μs |  3.88 |    0.11 |  23.4375 |        - |        - |  591640 B |       0.369 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 18,690.535 μs |   867.0907 μs |    47.5282 μs | 15.76 |    0.36 | 156.2500 |  62.5000 |        - | 4298542 B |       2.684 |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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
| **PerSample** | **2000**    |  **27.27 ms** |  **0.070 ms** | **0.004 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **196.83 ms** | **56.096 ms** | **3.075 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method               | Documents | Permutations | Mean         | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-------------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |    **60.079 ms** |  **9.2072 ms** | **0.5047 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |    10.284 ms |  1.5174 ms | 0.0832 ms |  0.17 |   78.1250 |   78.1250 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |     8.766 ms |  0.5161 ms | 0.0283 ms |  0.15 |         - |         - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |     7.022 ms |  0.1873 ms | 0.0103 ms |  0.12 |    7.8125 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |    10.563 ms |  0.4431 ms | 0.0243 ms |  0.18 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |    **60.391 ms** |  **3.1359 ms** | **0.1719 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |    13.297 ms |  0.8599 ms | 0.0471 ms |  0.22 |  218.7500 |  218.7500 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |    10.487 ms |  0.8665 ms | 0.0475 ms |  0.17 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |     7.232 ms |  0.5014 ms | 0.0275 ms |  0.12 |   15.6250 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |    10.781 ms |  0.7021 ms | 0.0385 ms |  0.18 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **1,026.100 ms** | **51.5777 ms** | **2.8271 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126359.81 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |    44.421 ms |  5.4270 ms | 0.2975 ms |  0.04 |  750.0000 |  750.0000 | 500.0000 |  10334.84 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |    34.884 ms |  1.3465 ms | 0.0738 ms |  0.03 |         - |         - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |    28.091 ms |  2.1022 ms | 0.1152 ms |  0.03 |   31.2500 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |    43.114 ms |  4.6561 ms | 0.2552 ms |  0.04 |         - |         - |        - |   1718.81 KB |       0.014 |
|                      |           |              |              |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **1,025.474 ms** | **20.9645 ms** | **1.1491 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |    63.193 ms | 25.2958 ms | 1.3865 ms |  0.06 | 1250.0000 | 1250.0000 | 750.0000 |  18666.59 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |    41.947 ms |  5.1433 ms | 0.2819 ms |  0.04 |         - |         - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |    28.796 ms |  0.5276 ms | 0.0289 ms |  0.03 |   62.5000 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |    42.768 ms |  1.0193 ms | 0.0559 ms |  0.04 |         - |         - |        - |   1718.81 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                 | Mean            | Error        | StdDev      | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |----------------:|-------------:|------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |      1,207.2 ns |    276.14 ns |    15.14 ns |   0.2174 |   0.0038 |        - |     5464 B |
| TfidfSaveAsync         |      1,470.2 ns |    177.75 ns |     9.74 ns |   0.2213 |        - |        - |     5592 B |
| HashingSaveAsync       |      1,245.5 ns |    209.37 ns |    11.48 ns |   0.2174 |   0.0038 |        - |     5496 B |
| IndexSaveAsync         |        459.9 ns |     22.01 ns |     1.21 ns |   0.0257 |        - |        - |      648 B |
| CountLoadSeekable1400K | 11,337,956.8 ns | 52,510.87 ns | 2,878.30 ns | 765.6250 | 750.0000 | 593.7500 | 12227249 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                          | SampleCount | Mean          | Error      | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-----------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **96.128 μs** |   **4.780 μs** |  **0.2620 μs** |  **1.00** |    **0.00** |  **11.1084** |   **4.6387** |        **-** |  **274.17 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    177.315 μs |  23.551 μs |  1.2909 μs |  1.84 |    0.01 |  12.6953 |   4.3945 |        - |  313.77 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      5.644 μs |   3.409 μs |  0.1869 μs |  0.06 |    0.00 |   1.5945 |   0.1755 |        - |   39.19 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    389.083 μs | 222.888 μs | 12.2173 μs |  4.05 |    0.11 |        - |        - |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,598.473 μs | 192.379 μs | 10.5450 μs | 58.24 |    0.17 |        - |        - |        - |  148.65 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     89.368 μs |  25.125 μs |  1.3772 μs |  0.93 |    0.01 |   0.4883 |        - |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,050.963 μs |  48.554 μs |  2.6614 μs | 10.93 |    0.04 |        - |        - |        - |   34.17 KB |        0.12 |
|                                 |             |               |            |            |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,312.387 μs** | **737.690 μs** | **40.4353 μs** |  **1.00** |    **0.04** | **439.4531** | **429.6875** | **423.8281** | **2738.36 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,037.914 μs |  63.652 μs |  3.4890 μs |  1.55 |    0.04 | 519.5313 | 507.8125 | 503.9063 | 3130.31 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     92.383 μs |  26.134 μs |  1.4325 μs |  0.07 |    0.00 |  47.4854 |  47.4854 |  47.4854 |   391.1 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    454.771 μs |  75.973 μs |  4.1643 μs |  0.35 |    0.01 |   1.4648 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 41,167.258 μs | 323.221 μs | 17.7168 μs | 31.39 |    0.85 |        - |        - |        - |  148.78 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     90.572 μs |  12.386 μs |  0.6789 μs |  0.07 |    0.00 |   0.4883 |   0.1221 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,293.571 μs | 534.326 μs | 29.2882 μs |  5.56 |    0.15 |        - |        - |        - |   34.17 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **2.049 ms** | **0.1541 ms** | **0.0084 ms** |  **1.00** |    **0.01** | **113.2813** | **113.2813** | **113.2813** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       |  1.505 ms | 0.0195 ms | 0.0011 ms |  0.73 |    0.00 |  15.6250 |   3.9063 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       |  2.122 ms | 0.1457 ms | 0.0080 ms |  1.04 |    0.01 |  70.3125 |  70.3125 |  70.3125 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       |  1.735 ms | 0.3307 ms | 0.0181 ms |  0.85 |    0.01 |   9.7656 |   1.9531 |        - |  273.65 KB |        0.31 |
|                      |           |           |           |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      |  **9.630 ms** | **9.5186 ms** | **0.5217 ms** |  **1.00** |    **0.07** | **859.3750** | **859.3750** | **859.3750** | **3436.53 KB** |        **1.00** |
| CountWithStopWords   | 1000      |  7.502 ms | 0.1640 ms | 0.0090 ms |  0.78 |    0.04 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 10.150 ms | 1.5418 ms | 0.0845 ms |  1.06 |    0.05 | 500.0000 | 500.0000 | 500.0000 | 2348.46 KB |        0.68 |
| HashingWithStopWords | 1000      |  8.617 ms | 0.4579 ms | 0.0251 ms |  0.90 |    0.04 | 265.6250 | 265.6250 | 265.6250 | 1139.46 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method  | Words | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Extract** | **2000**  |    **645.2 μs** |    **57.03 μs** |   **3.13 μs** |  **30.2734** |  **10.7422** |        **-** |  **499.33 KB** |
| **Extract** | **8000**  |  **2,810.9 μs** |   **294.36 μs** |  **16.14 μs** | **121.0938** |  **78.1250** |  **39.0625** | **1965.54 KB** |
| **Extract** | **32000** | **11,644.1 μs** | **1,843.01 μs** | **101.02 μs** | **859.3750** | **859.3750** | **734.3750** | **7946.75 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                     | Pattern       | Mean          | Error         | StdDev      | Gen0    | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |--------------:|--------------:|------------:|--------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **42,309.349 μs** | **7,973.8889 μs** | **437.0758 μs** |       **-** |       **-** |       **-** | **1554.62 KB** |
| Construct                  | [^ ]+         |      2.167 μs |     0.1405 μs |   0.0077 μs |  0.2213 |       - |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      3.352 μs |     0.8153 μs |   0.0447 μs |  0.3014 |       - |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **35,810.216 μs** |   **342.3008 μs** |  **18.7627 μs** |       **-** |       **-** |       **-** |    **7.23 KB** |
| Construct                  | [a-z]+        |      2.004 μs |     0.4825 μs |   0.0264 μs |  0.2174 |       - |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2.849 μs |     0.6366 μs |   0.0349 μs |  0.2899 |       - |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **47,652.564 μs** | **1,831.1686 μs** | **100.3725 μs** |       **-** |       **-** |       **-** | **1556.01 KB** |
| Construct                  | [A-Za-z]\\w+  |      3.525 μs |     0.9291 μs |   0.0509 μs |  0.2747 |       - |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      4.772 μs |     1.1457 μs |   0.0628 μs |  0.3586 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **24,433.611 μs** | **1,918.3313 μs** | **105.1502 μs** | **93.7500** | **62.5000** | **62.5000** | **9743.62 KB** |
| Construct                  | \\b\\w\\w+\\b |      1.137 μs |     0.1509 μs |   0.0083 μs |  0.0744 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      2.014 μs |     0.5082 μs |   0.0279 μs |  0.1564 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **42,725.933 μs** | **1,539.4778 μs** |  **84.3840 μs** |       **-** |       **-** |       **-** | **1554.51 KB** |
| Construct                  | \\S+          |      2.345 μs |     0.5773 μs |   0.0316 μs |  0.2136 |       - |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      3.664 μs |     0.9998 μs |   0.0548 μs |  0.2937 |       - |       - |    7.29 KB |

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

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

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

| Method | Classes | Mean     | Error     | StdDev   | Allocated |
|------- |-------- |---------:|----------:|---------:|----------:|
| **TopTwo** | **10**      | **11.52 ms** |  **0.194 ms** | **0.011 ms** |         **-** |
| **TopTwo** | **100**     | **89.91 ms** | **24.380 ms** | **1.336 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-OUFSZU : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error         | StdDev        | Median       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Normalize**          | **Job-OUFSZU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **53.73 μs** |     **11.029 μs** |     **12.701 μs** |     **49.03 μs** |   **1.05** |    **0.33** |        **-** |        **-** |        **-** |   **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |     67.84 μs |      1.368 μs |      1.576 μs |     68.25 μs |   1.32 |    0.27 |        - |        - |        - |  118.16 KB |        3.69 |
| Lodestar_Discretize         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    164.99 μs |     10.212 μs |     11.760 μs |    165.02 μs |   3.22 |    0.69 |        - |        - |        - |  156.99 KB |        4.91 |
| Lodestar_Discretize_64Bins  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    252.32 μs |     11.636 μs |     12.933 μs |    250.24 μs |   4.92 |    1.03 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    866.35 μs |     26.745 μs |     28.617 μs |    858.29 μs |  16.90 |    3.47 |        - |        - |        - |   33.98 KB |        1.06 |
| Lodestar_Quantile           | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,445.99 μs |     11.406 μs |     12.678 μs |  1,443.05 μs |  28.21 |    5.73 |        - |        - |        - |   31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    947.16 μs |     20.806 μs |     22.262 μs |    938.63 μs |  18.48 |    3.77 |        - |        - |        - |  110.84 KB |        3.46 |
| Lodestar_Power              | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |    197.55 μs |      7.332 μs |      7.845 μs |    196.60 μs |   3.85 |    0.80 |        - |        - |        - |   31.71 KB |        0.99 |
| Lodestar_Power_Fit          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,793.11 μs |     17.216 μs |     19.136 μs |  1,788.14 μs |  34.98 |    7.11 |        - |        - |        - |  135.77 KB |        4.24 |
| Lodestar_KnnImpute          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     | 12,861.47 μs |  4,258.638 μs |  4,904.254 μs | 16,703.09 μs | 250.91 |  108.01 |        - |        - |        - |  103.87 KB |        3.25 |
| MlNet_NormalizeLpNorm_Read  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,176.26 μs |     92.434 μs |     98.903 μs |  1,181.56 μs |  22.95 |    5.03 |        - |        - |        - |  339.98 KB |       10.63 |
| MlNet_NormalizeBinning_Read | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,933.92 μs |    161.459 μs |    172.759 μs |  2,874.77 μs |  57.24 |   12.09 |        - |        - |        - |  399.35 KB |       12.48 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     13.09 μs |      2.733 μs |      0.150 μs |     13.03 μs |   1.00 |    0.01 |   1.2665 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     62.53 μs |     12.384 μs |      0.679 μs |     62.81 μs |   4.78 |    0.07 |  36.9873 |  36.9873 |  36.9873 |  117.47 KB |        3.76 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.24 μs |     14.567 μs |      0.798 μs |     83.09 μs |   6.36 |    0.08 |  49.9268 |  49.9268 |  49.9268 |  156.31 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     64.16 μs |      8.135 μs |      0.446 μs |     63.92 μs |   4.90 |    0.06 |   1.2207 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    136.06 μs |      1.872 μs |      0.103 μs |    136.04 μs |  10.40 |    0.10 |   1.2207 |        - |        - |   33.26 KB |        1.06 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    587.38 μs |    147.581 μs |      8.089 μs |    584.08 μs |  44.89 |    0.69 |   0.9766 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    251.69 μs |     39.606 μs |      2.171 μs |    250.94 μs |  19.24 |    0.24 |   4.3945 |   0.9766 |        - |   110.1 KB |        3.52 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    142.03 μs |      5.499 μs |      0.301 μs |    141.90 μs |  10.86 |    0.11 |   1.2207 |        - |        - |   31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    694.75 μs |     61.612 μs |      3.377 μs |    694.02 μs |  53.10 |    0.57 |   4.8828 |        - |        - |  134.99 KB |        4.32 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  6,733.37 μs |    385.079 μs |     21.107 μs |  6,724.29 μs | 514.63 |    5.26 |        - |        - |        - |  103.15 KB |        3.30 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    266.07 μs |     98.123 μs |      5.378 μs |    265.93 μs |  20.34 |    0.41 |  12.2070 |  11.7188 |        - |  291.59 KB |        9.32 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,099.47 μs |  1,212.096 μs |     66.439 μs |  1,093.50 μs |  84.03 |    4.48 |  29.2969 |   5.8594 |        - |   705.1 KB |       22.55 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| **Lodestar_Normalize**          | **Job-OUFSZU** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **421.48 μs** |     **13.177 μs** |     **15.174 μs** |    **419.09 μs** |   **1.00** |    **0.05** |        **-** |        **-** |        **-** |  **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,199.29 μs |     20.298 μs |     23.376 μs |  1,187.06 μs |   2.85 |    0.11 |        - |        - |        - | 2344.73 KB |        3.75 |
| Lodestar_Discretize         | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,952.85 μs |     27.743 μs |     31.949 μs |  2,947.51 μs |   7.01 |    0.25 |        - |        - |        - | 3125.74 KB |        5.00 |
| Lodestar_Discretize_64Bins  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,872.48 μs |    908.895 μs |  1,010.235 μs |  4,285.35 μs |   9.20 |    2.36 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Discretize_Fit     | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,972.40 μs |  4,691.660 μs |  5,402.923 μs |  6,168.82 μs |  23.69 |   12.56 |        - |        - |        - |  627.73 KB |        1.00 |
| Lodestar_Quantile           | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 11,971.62 μs |     32.905 μs |     33.791 μs | 11,973.86 μs |  28.44 |    0.98 |        - |        - |        - |  625.46 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,019.61 μs |  4,620.992 μs |  5,321.541 μs |  6,190.44 μs |  23.80 |   12.37 |        - |        - |        - |  704.59 KB |        1.13 |
| Lodestar_Power              | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,913.97 μs |     11.980 μs |     12.303 μs |  2,915.48 μs |   6.92 |    0.24 |        - |        - |        - |  625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,256.96 μs |  3,591.556 μs |  4,136.041 μs | 13,641.24 μs |  38.62 |    9.69 |        - |        - |        - |  2659.2 KB |        4.25 |
| Lodestar_KnnImpute          | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,832.73 μs |  4,420.705 μs |  5,090.891 μs |  7,163.82 μs |  25.73 |   11.84 |        - |        - |        - |  105.37 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,947.07 μs |  1,902.046 μs |  2,035.168 μs | 11,103.56 μs |  23.63 |    4.78 |        - |        - |        - |   548.3 KB |        0.88 |
| MlNet_NormalizeBinning_Read | Job-OUFSZU | 1               | 20             | Throughput  | 1            | 5           | 20000    | 25,746.35 μs | 12,088.985 μs | 13,921.693 μs | 15,374.31 μs |  61.16 |   32.36 |        - |        - |        - | 2154.53 KB |        3.44 |
|                             |            |                 |                |             |              |             |          |              |               |               |              |        |         |          |          |          |            |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    550.09 μs |     74.859 μs |      4.103 μs |    548.27 μs |   1.00 |    0.01 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    680.08 μs |    501.352 μs |     27.481 μs |    667.31 μs |   1.24 |    0.04 | 296.8750 | 296.8750 | 296.8750 | 2346.46 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,158.65 μs |    320.293 μs |     17.556 μs |  2,152.37 μs |   3.92 |    0.04 | 996.0938 | 996.0938 | 996.0938 | 3125.68 KB |        5.00 |
| Lodestar_Discretize_64Bins  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,608.88 μs |    162.696 μs |      8.918 μs |  1,613.89 μs |   2.92 |    0.02 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,226.92 μs |     57.113 μs |      3.131 μs |  5,228.48 μs |   9.50 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  627.14 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 12,429.56 μs |    284.626 μs |     15.601 μs | 12,423.64 μs |  22.60 |    0.15 | 187.5000 | 187.5000 | 187.5000 |  625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  5,416.86 μs |    111.190 μs |      6.095 μs |  5,414.49 μs |   9.85 |    0.06 | 195.3125 | 195.3125 | 195.3125 |  703.99 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,137.83 μs |    132.243 μs |      7.249 μs |  3,140.06 μs |   5.70 |    0.04 | 199.2188 | 199.2188 | 199.2188 |  625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 13,417.20 μs |  1,565.199 μs |     85.794 μs | 13,425.99 μs |  24.39 |    0.21 | 750.0000 | 750.0000 | 750.0000 | 2660.69 KB |        4.26 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  6,919.35 μs |    131.924 μs |      7.231 μs |  6,920.08 μs |  12.58 |    0.08 |        - |        - |        - |  104.65 KB |        0.17 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,295.90 μs |    504.426 μs |     27.649 μs |  2,280.74 μs |   4.17 |    0.05 |  19.5313 |   7.8125 |        - |   477.9 KB |        0.76 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 10,409.71 μs |  1,332.915 μs |     73.062 μs | 10,417.55 μs |  18.92 |    0.17 | 234.3750 | 203.1250 | 187.5000 | 2138.32 KB |        3.42 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **51.82 ns** |  **1.820 ns** | **0.100 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  51.31 ns |  4.026 ns | 0.221 ns |  0.99 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **768**  | **114.92 ns** |  **3.046 ns** | **0.167 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  | 101.41 ns |  6.460 ns | 0.354 ns |  0.88 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **1024** | **144.81 ns** | **14.502 ns** | **0.795 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 141.64 ns |  2.363 ns | 0.130 ns |  0.98 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

_As of 2026-11-19, measured at commit `9336c7aaf27dfc658cb7d852f6b6e5d7d7b89264`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  Job-OLJJDR : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error        | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-------------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **1000**    |     **59.62 μs** |     **0.155 μs** |   **0.159 μs** | **0.1831** |   **3.12 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 1000    |     77.44 μs |     0.178 μs |   0.190 μs | 1.2207 |  20.55 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 1000    |     70.56 μs |     0.182 μs |   0.187 μs | 0.8545 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     69.25 μs |     5.761 μs |   0.316 μs | 0.1221 |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     78.31 μs |    42.750 μs |   2.343 μs | 1.2207 |  20.55 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     72.84 μs |    11.359 μs |   0.623 μs | 0.8545 |  15.52 KB |
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **10000**   |    **602.19 μs** |     **1.362 μs** |   **1.338 μs** |      **-** |   **3.12 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 10000   |    875.76 μs |     2.830 μs |   2.907 μs | 0.9766 |  26.13 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 10000   |    911.16 μs |     3.277 μs |   3.506 μs | 0.9766 |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    564.46 μs |    84.152 μs |   4.613 μs |      - |   3.12 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |    854.32 μs |    38.536 μs |   2.112 μs | 0.9766 |  26.13 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    854.25 μs |    64.772 μs |   3.550 μs | 0.9766 |  21.14 KB |
| **UpsertThenSearch** | **Job-OLJJDR** | **20**             | **Throughput**  | **5**           | **100000**  |  **8,952.54 μs** |    **66.134 μs** |  **73.508 μs** |      **-** |   **3.16 KB** |
| UpsertThenHybrid | Job-OLJJDR | 20             | Throughput  | 5           | 100000  | 13,282.48 μs |    42.446 μs |  48.881 μs |      - |  45.96 KB |
| Hybrid           | Job-OLJJDR | 20             | Throughput  | 5           | 100000  | 12,744.23 μs |    82.744 μs |  91.970 μs |      - |  41.02 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  |  9,512.02 μs | 1,053.014 μs |  57.719 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 13,600.15 μs | 3,816.049 μs | 209.171 μs |      - |  45.96 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 12,950.00 μs | 1,727.122 μs |  94.669 μs |      - |  41.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

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

| Method                | Documents | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **264.5 μs** |    **10.03 μs** |   **0.55 μs** |  **1.00** |    **0.00** |    **8.3008** |    **0.9766** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    274.6 μs |    38.67 μs |   2.12 μs |  1.04 |    0.01 |    9.2773 |    1.4648 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    743.8 μs |    29.35 μs |   1.61 μs |  2.81 |    0.01 |   23.4375 |    8.7891 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  2,118.6 μs |   115.60 μs |   6.34 μs |  8.01 |    0.03 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    280.4 μs |    29.18 μs |   1.60 μs |  1.06 |    0.01 |    5.3711 |    0.4883 |         - |   136.13 KB |        0.66 |
|                       |           |             |             |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,549.1 μs** |    **82.08 μs** |   **4.50 μs** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,696.3 μs |   169.84 μs |   9.31 μs |  1.10 |    0.01 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,702.5 μs |   623.20 μs |  34.16 μs |  2.39 |    0.02 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 13,070.9 μs | 7,776.69 μs | 426.27 μs |  8.44 |    0.24 | 1031.2500 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,647.9 μs |    90.85 μs |   4.98 μs |  1.06 |    0.00 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 5.743 | 4.197 | 0.73x | 5.749 | 8.371 | 1.46x |
| kmeans_restarts_10000 | 22.804 | 11.908 | 0.52x | 22.811 | 23.740 | 1.04x |
| kmeans_weighted_100000 | 57.618 | 33.903 | 0.59x | 58.023 | 57.020 | 0.98x |
| kmeans_restarts_100000 | 226.499 | 92.241 | 0.41x | 226.901 | 171.514 | 0.76x |
| dbscan_weighted_5000 | 8.565 | 31.521 | 3.68x | 9.001 | 31.519 | 3.50x |
| dbscan_weighted_20000 | 128.387 | 196.788 | 1.53x | 130.418 | 196.780 | 1.51x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.809 | 108.678 | 134.28x | 0.809 | 108.671 | 134.28x |
| ridge_robust_1000 | 0.889 | 100.124 | 112.61x | 0.889 | 100.121 | 112.61x |
| lasso_1000 | 5.933 | 916.777 | 154.53x | 5.932 | 916.733 | 154.54x |
| ph_test_1000 | 0.155 | 18.550 | 119.63x | 0.155 | 18.548 | 119.62x |
| predict_survival_1000 | 0.032 | 1.640 | 51.36x | 0.032 | 1.640 | 51.37x |
| time_varying_1000 | 54.676 | 330.889 | 6.05x | 54.676 | 330.883 | 6.05x |
| stratified_weighted_10000 | 8.512 | 617.802 | 72.58x | 8.561 | 617.783 | 72.17x |
| ridge_robust_10000 | 9.738 | 1037.864 | 106.58x | 9.945 | 1037.857 | 104.36x |
| lasso_10000 | 53.664 | 6194.494 | 115.43x | 53.687 | 6194.057 | 115.37x |
| ph_test_10000 | 2.222 | 175.012 | 78.75x | 2.285 | 174.996 | 76.59x |
| predict_survival_10000 | 0.034 | 1.618 | 47.23x | 0.034 | 1.618 | 47.24x |
| time_varying_10000 | 2005.624 | 5145.334 | 2.57x | 2006.814 | 5144.983 | 2.56x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 4.941 | 47.898 | 9.69x | 4.940 | 189.365 | 38.33x |
| cv_minmax_n1000 | 3.536 | 6.008 | 1.70x | 3.536 | 23.979 | 6.78x |
| cv_plus_gamma_n1000 | 5.736 | 48.855 | 8.52x | 5.736 | 195.235 | 34.04x |
| bootstrap_plus_n1000 | 16.221 | 48.854 | 3.01x | 16.223 | 193.563 | 11.93x |
| cv_plus_n10000 | 57.100 | 160.230 | 2.81x | 57.095 | 462.820 | 8.11x |
| cv_minmax_n10000 | 33.904 | 39.880 | 1.18x | 33.904 | 156.967 | 4.63x |
| cv_plus_gamma_n10000 | 69.017 | 170.219 | 2.47x | 69.013 | 479.639 | 6.95x |
| bootstrap_plus_n10000 | 172.616 | 161.417 | 0.94x | 172.592 | 473.933 | 2.75x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.401 | 3.316 | 8.28x | 0.401 | 3.316 | 8.28x |
| glm_gamma_n1000 | 0.489 | 4.297 | 8.79x | 0.489 | 4.297 | 8.79x |
| glm_poisson_exposure_n1000 | 0.396 | 3.939 | 9.94x | 0.396 | 3.939 | 9.94x |
| mnlogit_n1000 | 0.826 | 15.510 | 18.77x | 0.826 | 15.509 | 18.77x |
| glm_negative_binomial_n10000 | 3.741 | 13.414 | 3.59x | 3.750 | 13.414 | 3.58x |
| glm_gamma_n10000 | 4.298 | 15.620 | 3.63x | 4.310 | 15.619 | 3.62x |
| glm_poisson_exposure_n10000 | 4.357 | 15.163 | 3.48x | 4.369 | 15.162 | 3.47x |
| mnlogit_n10000 | 8.428 | 80.191 | 9.52x | 8.436 | 80.188 | 9.51x |
| glm_negative_binomial_n100000 | 39.891 | 140.564 | 3.52x | 40.127 | 548.575 | 13.67x |
| glm_gamma_n100000 | 47.709 | 203.993 | 4.28x | 47.951 | 805.933 | 16.81x |
| glm_poisson_exposure_n100000 | 56.113 | 145.030 | 2.58x | 57.035 | 578.406 | 10.14x |
| mnlogit_n100000 | 82.346 | 793.276 | 9.63x | 82.648 | 1552.082 | 18.78x |

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

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.842 | 51.657 | 28.04x | 1.842 | 51.481 | 27.95x |
| liml_robust_n1000 | 1.998 | 51.559 | 25.80x | 1.999 | 51.558 | 25.79x |
| gmm_robust_n1000 | 1.923 | 49.406 | 25.70x | 1.923 | 49.404 | 25.70x |
| 2sls_kernel_n1000 | 10.044 | 53.196 | 5.30x | 10.044 | 53.195 | 5.30x |
| 2sls_clustered_n1000 | 1.789 | 52.185 | 29.18x | 1.789 | 52.185 | 29.18x |
| 2sls_robust_n10000 | 17.648 | 227.033 | 12.86x | 17.863 | 454.017 | 25.42x |
| liml_robust_n10000 | 19.569 | 226.361 | 11.57x | 19.934 | 452.841 | 22.72x |
| gmm_robust_n10000 | 18.272 | 217.940 | 11.93x | 18.638 | 435.682 | 23.38x |
| 2sls_kernel_n10000 | 107.016 | 234.374 | 2.19x | 107.283 | 468.616 | 4.37x |
| 2sls_clustered_n10000 | 17.278 | 237.195 | 13.73x | 17.637 | 474.117 | 26.88x |
| 2sls_robust_n100000 | 198.697 | 2787.036 | 14.03x | 202.305 | 8496.162 | 42.00x |
| liml_robust_n100000 | 209.265 | 2774.219 | 13.26x | 211.909 | 8432.846 | 39.79x |
| gmm_robust_n100000 | 201.395 | 2714.971 | 13.48x | 204.807 | 8118.453 | 39.64x |
| 2sls_kernel_n100000 | 3929.261 | 3878.684 | 0.99x | 3930.802 | 11013.008 | 2.80x |
| 2sls_clustered_n100000 | 177.697 | 2996.341 | 16.86x | 179.151 | 9332.982 | 52.10x |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.598 | 0.878 | 0.34x | 2.597 | 0.878 | 0.34x |
| transform_frobenius_n500 | 8.110 | 1.835 | 0.23x | 8.110 | 1.835 | 0.23x |
| transform_frobenius_n2000 | 29.040 | 6.516 | 0.22x | 29.086 | 25.997 | 0.89x |
| transform_kl_n100 | 1.293 | 14.689 | 11.36x | 1.293 | 14.689 | 11.36x |
| transform_kl_n500 | 5.899 | 31.127 | 5.28x | 5.899 | 31.126 | 5.28x |
| transform_kl_n2000 | 23.440 | 124.031 | 5.29x | 23.492 | 408.673 | 17.40x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.093 | 2.641 | 28.46x | 0.093 | 2.641 | 28.46x |
| ols_hac_n1000 | 0.193 | 3.235 | 16.80x | 0.193 | 3.235 | 16.80x |
| ols_cluster_n1000 | 0.072 | 3.382 | 47.22x | 0.072 | 3.382 | 47.23x |
| ols_summary_n10000 | 0.529 | 11.428 | 21.62x | 0.529 | 11.428 | 21.62x |
| ols_hac_n10000 | 2.013 | 13.198 | 6.56x | 2.018 | 13.198 | 6.54x |
| ols_cluster_n10000 | 0.716 | 13.611 | 19.02x | 0.718 | 13.610 | 18.97x |
| ols_summary_n100000 | 4.959 | 147.855 | 29.82x | 4.981 | 587.725 | 117.98x |
| ols_hac_n100000 | 20.984 | 164.790 | 7.85x | 21.489 | 654.357 | 30.45x |
| ols_cluster_n100000 | 8.035 | 169.370 | 21.08x | 8.226 | 676.868 | 82.29x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.773 | 4.108 | 2.32x | 1.776 | 4.108 | 2.31x |
| sparse_infrequent_10000 | 2.195 | 4.361 | 1.99x | 2.198 | 4.361 | 1.98x |
| sparse_100000 | 17.653 | 29.790 | 1.69x | 17.948 | 29.788 | 1.66x |
| sparse_infrequent_100000 | 19.129 | 25.237 | 1.32x | 19.464 | 25.237 | 1.30x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.4', 'pandas': '2.3.3'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.277 | 18.079 | 65.35x | 0.277 | 18.078 | 65.35x |
| fe_twoway_clustered_n1000 | 0.465 | 28.008 | 60.22x | 0.465 | 28.006 | 60.22x |
| fe_kernel_n1000 | 0.295 | 18.400 | 62.37x | 0.295 | 18.399 | 62.37x |
| between_robust_n1000 | 0.165 | 17.254 | 104.45x | 0.165 | 17.254 | 104.45x |
| first_difference_robust_n1000 | 0.321 | 20.486 | 63.74x | 0.321 | 20.486 | 63.74x |
| random_effects_n1000 | 0.291 | 23.725 | 81.49x | 0.291 | 23.723 | 81.48x |
| fe_entity_n10000 | 3.048 | 27.569 | 9.05x | 3.124 | 27.568 | 8.82x |
| fe_twoway_clustered_n10000 | 5.474 | 51.316 | 9.38x | 5.667 | 102.588 | 18.10x |
| fe_kernel_n10000 | 3.310 | 27.798 | 8.40x | 3.355 | 27.797 | 8.28x |
| between_robust_n10000 | 1.779 | 26.408 | 14.85x | 1.783 | 26.406 | 14.81x |
| first_difference_robust_n10000 | 3.658 | 31.280 | 8.55x | 3.671 | 31.280 | 8.52x |
| random_effects_n10000 | 3.219 | 37.125 | 11.53x | 3.255 | 37.124 | 11.41x |
| fe_entity_n100000 | 35.150 | 215.935 | 6.14x | 36.650 | 857.505 | 23.40x |
| fe_twoway_clustered_n100000 | 56.334 | 391.329 | 6.95x | 57.742 | 1390.953 | 24.09x |
| fe_kernel_n100000 | 38.530 | 202.272 | 5.25x | 39.287 | 802.485 | 20.43x |
| between_robust_n100000 | 20.978 | 166.429 | 7.93x | 21.347 | 497.305 | 23.30x |
| first_difference_robust_n100000 | 39.017 | 217.784 | 5.58x | 40.424 | 820.584 | 20.30x |
| random_effects_n100000 | 35.961 | 264.705 | 7.36x | 37.336 | 990.788 | 26.54x |

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

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 0.728 | 76.296 | 104.83x | 0.728 | 304.913 | 418.97x |
| fit_penalised_weighted_1000 | 0.726 | 82.569 | 113.74x | 0.726 | 327.531 | 451.11x |
| predict_survival_1000 | 0.797 | 2.129 | 2.67x | 0.800 | 2.128 | 2.66x |
| fit_10000 | 7.879 | 692.339 | 87.87x | 8.433 | 1034.236 | 122.64x |
| fit_penalised_weighted_10000 | 7.999 | 714.382 | 89.31x | 8.559 | 1036.081 | 121.05x |
| predict_survival_10000 | 3.359 | 3.293 | 0.98x | 3.509 | 13.149 | 3.75x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.025 | 12.012 | 474.62x | 0.025 | 12.012 | 474.64x |
| logrank_fleming_harrington_1000 | 0.036 | 18.479 | 518.33x | 0.036 | 18.478 | 518.24x |
| multigroup_1000 | 0.118 | 24.687 | 209.73x | 0.118 | 24.683 | 209.71x |
| pairwise_1000 | 0.345 | 125.728 | 364.86x | 0.345 | 125.719 | 364.85x |
| rmst_1000 | 0.103 | 1080.450 | 10464.06x | 0.103 | 1080.425 | 10463.88x |
| fixed_point_1000 | 0.043 | 13.765 | 317.72x | 0.043 | 13.764 | 317.71x |
| concordance_1000 | 0.130 | 7.569 | 58.33x | 0.130 | 7.569 | 58.33x |
| logrank_wilcoxon_weighted_10000 | 0.365 | 13.919 | 38.10x | 0.365 | 13.919 | 38.10x |
| logrank_fleming_harrington_10000 | 0.465 | 22.718 | 48.81x | 0.465 | 22.716 | 48.81x |
| multigroup_10000 | 2.270 | 37.229 | 16.40x | 2.273 | 37.228 | 16.38x |
| pairwise_10000 | 4.292 | 145.243 | 33.84x | 4.292 | 145.234 | 33.84x |
| rmst_10000 | 1.827 | 1215.183 | 665.13x | 1.829 | 1215.137 | 664.47x |
| fixed_point_10000 | 0.524 | 16.369 | 31.27x | 0.523 | 16.368 | 31.27x |
| concordance_10000 | 2.241 | 88.960 | 39.70x | 2.241 | 88.952 | 39.70x |
| logrank_wilcoxon_weighted_100000 | 3.941 | 21.988 | 5.58x | 4.062 | 21.986 | 5.41x |
| logrank_fleming_harrington_100000 | 3.820 | 35.830 | 9.38x | 3.877 | 35.822 | 9.24x |
| multigroup_100000 | 9.880 | 50.863 | 5.15x | 10.004 | 50.861 | 5.08x |
| pairwise_100000 | 39.703 | 225.202 | 5.67x | 40.681 | 225.121 | 5.53x |
| rmst_100000 | 8.327 | 445.781 | 53.53x | 8.440 | 445.763 | 52.82x |
| fixed_point_100000 | 4.734 | 29.560 | 6.24x | 4.922 | 29.535 | 6.00x |
| concordance_100000 | 27.550 | 712.368 | 25.86x | 27.729 | 712.320 | 25.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

_As of 2026-11-18, measured at commit `1a87da350fbdf0a6f569275fd6868c72f0bd8371`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 3.570 | 67.600 | 18.93x | 3.570 | 67.599 | 18.93x |
| loglogistic_left_1000 | 9.744 | 95.945 | 9.85x | 9.855 | 95.946 | 9.74x |
| lognormal_interval_1000 | 3.347 | 181.758 | 54.30x | 3.348 | 181.747 | 54.29x |
| generalized_gamma_1000 | 13.317 | 847.569 | 63.65x | 13.316 | 847.537 | 63.65x |
| weibull_aft_1000 | 7.500 | 128.000 | 17.07x | 7.499 | 127.990 | 17.07x |
| lognormal_aft_ridge_robust_1000 | 9.506 | 4024.046 | 423.30x | 9.602 | 4023.788 | 419.06x |
| loglogistic_aft_interval_1000 | 10.461 | 193.657 | 18.51x | 10.460 | 193.638 | 18.51x |
| breslow_fleming_harrington_1000 | 0.284 | 6.055 | 21.35x | 0.284 | 6.054 | 21.35x |
| kaplan_meier_left_1000 | 0.097 | 7.595 | 78.67x | 0.097 | 7.594 | 78.66x |
| weibull_10000 | 36.450 | 79.095 | 2.17x | 36.449 | 79.090 | 2.17x |
| loglogistic_left_10000 | 110.998 | 170.182 | 1.53x | 111.007 | 170.172 | 1.53x |
| lognormal_interval_10000 | 32.045 | 343.449 | 10.72x | 32.043 | 343.440 | 10.72x |
| generalized_gamma_10000 | 111.790 | 1965.315 | 17.58x | 111.783 | 1965.187 | 17.58x |
| weibull_aft_10000 | 76.632 | 139.961 | 1.83x | 77.012 | 139.954 | 1.82x |
| lognormal_aft_ridge_robust_10000 | 132.218 | 38384.164 | 290.31x | 132.210 | 38381.966 | 290.31x |
| loglogistic_aft_interval_10000 | 98.856 | 267.547 | 2.71x | 98.887 | 267.540 | 2.71x |
| breslow_fleming_harrington_10000 | 2.759 | 7.801 | 2.83x | 2.759 | 7.801 | 2.83x |
| kaplan_meier_left_10000 | 0.962 | 9.359 | 9.72x | 0.962 | 9.359 | 9.72x |

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

_As of 2026-11-24, measured at commit `64dcf7b37e167677244d26cb8bf82db1915bf6cf`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.4'} (py 3.12.16)
C#:     Lodestar on .NET 10.0.15
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.067 | 2.607 | 38.63x | 0.067 | 2.607 | 38.62x |
| var_n10000 | 0.742 | 15.855 | 21.38x | 0.744 | 15.854 | 21.30x |
| var_n100000 | 7.570 | 144.497 | 19.09x | 7.795 | 144.493 | 18.54x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
