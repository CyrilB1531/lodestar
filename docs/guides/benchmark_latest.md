# Latest known benchmark result, per method

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml` from the
> wiki's own history, alongside [nightly_run](nightly_run.md).

**Not a comparison across methods.** Each section below is the last night that method was
actually re-run -- whichever night touched the source near it, not necessarily last night,
and not the same night as its neighbours here. Every run measures on a GitHub hosted
runner whose hardware differs night to night, so a number here says "this is the last
known reading", never "faster than the section above it".

## Per method

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

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

| Method                         | PairCount | Mean             | Error           | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|----------------:|--------------:|---------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **865.4 ns** |        **44.70 ns** |       **2.45 ns** |     **1.00** |    **0.00** |   **0.0019** |        **-** |        **-** |      **40 B** |        **1.00** |
| MetaNumerics_Pearson           | 100       |         690.7 ns |        95.79 ns |       5.25 ns |     0.80 |    0.01 |   0.0095 |        - |        - |     168 B |        4.20 |
| Lodestar_Spearman              | 100       |       2,374.8 ns |       244.61 ns |      13.41 ns |     2.74 |    0.02 |   0.2441 |        - |        - |    4144 B |      103.60 |
| MetaNumerics_Spearman          | 100       |       2,865.5 ns |        80.45 ns |       4.41 ns |     3.31 |    0.01 |   0.1183 |        - |        - |    2024 B |       50.60 |
| Lodestar_KendallTau            | 100       |       4,257.4 ns |     1,827.06 ns |     100.15 ns |     4.92 |    0.10 |        - |        - |        - |         - |        0.00 |
| MetaNumerics_KendallTau        | 100       |       8,136.1 ns |     3,508.06 ns |     192.29 ns |     9.40 |    0.19 |        - |        - |        - |     152 B |        3.80 |
| Lodestar_KendallTau_Asymptotic | 100       |       4,183.8 ns |       159.97 ns |       8.77 ns |     4.83 |    0.01 |        - |        - |        - |         - |        0.00 |
|                                |           |                  |                 |               |          |         |          |          |          |           |             |
| **Lodestar_Pearson**               | **10000**     |      **78,900.7 ns** |     **2,569.41 ns** |     **140.84 ns** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **40 B** |        **1.00** |
| MetaNumerics_Pearson           | 10000     |      49,563.0 ns |     1,999.01 ns |     109.57 ns |     0.63 |    0.00 |        - |        - |        - |     168 B |        4.20 |
| Lodestar_Spearman              | 10000     |     885,906.1 ns |    70,737.66 ns |   3,877.37 ns |    11.23 |    0.05 | 166.0156 | 166.0156 | 166.0156 | 1324848 B |   33,121.20 |
| MetaNumerics_Spearman          | 10000     |   1,705,313.0 ns |    55,649.32 ns |   3,050.33 ns |    21.61 |    0.05 |   7.8125 |        - |        - |  160427 B |    4,010.68 |
| Lodestar_KendallTau            | 10000     |   1,830,438.4 ns |    50,622.78 ns |   2,774.81 ns |    23.20 |    0.05 |        - |        - |        - |         - |        0.00 |
| MetaNumerics_KendallTau        | 10000     | 240,599,211.1 ns | 4,234,450.55 ns | 232,104.52 ns | 3,049.40 |    5.36 |        - |        - |        - |         - |        0.00 |
| Lodestar_KendallTau_Asymptotic | 10000     |   1,834,119.4 ns |    35,464.77 ns |   1,943.94 ns |    23.25 |    0.04 |        - |        - |        - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

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

| Method                     | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------:|---------:|---------:|------:|----------:|------------:|
| ChiSquaredSfOneDf          |  28.39 ns | 0.763 ns | 0.042 ns |  1.00 |         - |          NA |
| ChiSquaredSfFourDf         |  11.80 ns | 0.757 ns | 0.041 ns |  0.42 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  41.67 ns | 3.989 ns | 0.219 ns |  1.47 |         - |          NA |
| ChiSquaredSfHundredDf      |  71.77 ns | 1.997 ns | 0.109 ns |  2.53 |         - |          NA |
| ChiSquaredSfFractionalDf   | 112.67 ns | 0.379 ns | 0.021 ns |  3.97 |         - |          NA |
| NormalQuantile             | 115.50 ns | 1.335 ns | 0.073 ns |  4.07 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmBenchmarks-report-github

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

| Method       | SampleSize | Regressors | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1    | Allocated  | Alloc Ratio |
|------------- |----------- |----------- |----------:|-----------:|----------:|------:|--------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Glm** | **200**        | **1**          |  **38.22 μs** |  **16.114 μs** |  **0.883 μs** |  **1.00** |    **0.03** |  **0.7324** |       **-** |   **12.13 KB** |        **1.00** |
| Accord_Glm   | 200        | 1          |  68.00 μs |  13.274 μs |  0.728 μs |  1.78 |    0.04 |  7.2021 |  0.2441 |  117.67 KB |        9.70 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **200**        | **3**          |  **45.63 μs** |   **0.349 μs** |  **0.019 μs** |  **1.00** |    **0.00** |  **1.1597** |  **0.0610** |   **19.38 KB** |        **1.00** |
| Accord_Glm   | 200        | 3          | 123.47 μs |  36.967 μs |  2.026 μs |  2.71 |    0.04 | 10.2539 |  0.4883 |  168.44 KB |        8.69 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **1**          | **362.36 μs** |  **21.105 μs** |  **1.157 μs** |  **1.00** |    **0.00** |  **6.3477** |  **0.4883** |  **110.56 KB** |        **1.00** |
| Accord_Glm   | 2000       | 1          | 504.76 μs | 133.255 μs |  7.304 μs |  1.39 |    0.02 | 53.7109 | 15.6250 |  881.77 KB |        7.98 |
|              |            |            |           |            |           |       |         |         |         |            |             |
| **Lodestar_Glm** | **2000**       | **3**          | **429.42 μs** | **110.927 μs** |  **6.080 μs** |  **1.00** |    **0.02** | **10.2539** |       **-** |  **174.06 KB** |        **1.00** |
| Accord_Glm   | 2000       | 3          | 901.26 μs | 270.750 μs | 14.841 μs |  2.10 |    0.04 | 77.1484 | 33.2031 | 1265.51 KB |        7.27 |

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

| Method              | SampleSize | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |------------:|-------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Poisson**             | **200**        |    **31.36 μs** |     **7.293 μs** |  **0.400 μs** |  **1.00** |    **0.02** |   **0.7324** |        **-** |        **-** |   **12.13 KB** |        **1.00** |
| PoissonWithExposure | 200        |    61.37 μs |     2.087 μs |  0.114 μs |  1.96 |    0.02 |   1.2207 |        - |        - |   20.52 KB |        1.69 |
|                     |            |             |              |           |       |         |          |          |          |            |             |
| **Poisson**             | **20000**      | **4,310.44 μs** | **1,570.863 μs** | **86.104 μs** |  **1.00** |    **0.02** | **328.1250** | **328.1250** | **328.1250** | **1097.77 KB** |        **1.00** |
| PoissonWithExposure | 20000      | 7,910.40 μs |   148.367 μs |  8.133 μs |  1.84 |    0.03 | 500.0000 | 500.0000 | 500.0000 | 1878.97 KB |        1.71 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmPoissonBenchmarks-report-github

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

| Method | MeanCount | Mean     | Error    | StdDev  | Gen0   | Gen1   | Allocated |
|------- |---------- |---------:|---------:|--------:|-------:|-------:|----------:|
| **Fit**    | **5**         | **305.3 μs** | **23.71 μs** | **1.30 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **50000**     | **321.8 μs** | **88.03 μs** | **4.83 μs** | **6.3477** | **0.4883** | **110.56 KB** |
| **Fit**    | **5000000**   | **316.2 μs** |  **8.30 μs** | **0.45 μs** | **6.3477** | **0.4883** | **110.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

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

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **31.34 μs** |      **4.240 μs** |     **0.232 μs** |  **1.00** |    **0.01** |   **1.6479** |   **0.0610** |        **-** |    **27.1 KB** |        **1.00** |
| MathNet_Gls  | 50         |     112.93 μs |     26.409 μs |     1.448 μs |  3.60 |    0.05 |   1.9531 |   0.1221 |        - |   32.94 KB |        1.22 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **712.31 μs** |     **94.587 μs** |     **5.185 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.57 KB** |        **1.00** |
| MathNet_Gls  | 200        |   2,433.67 μs |  1,314.230 μs |    72.037 μs |  3.42 |    0.09 |  97.6563 |  97.6563 |  97.6563 |  369.89 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **7,787.91 μs** |  **1,391.392 μs** |    **76.267 μs** |  **1.00** |    **0.01** | **296.8750** | **296.8750** | **296.8750** |  **2010.2 KB** |        **1.00** |
| MathNet_Gls  | 500        |  30,174.90 μs | 23,409.210 μs | 1,283.138 μs |  3.87 |    0.15 | 281.2500 | 281.2500 | 281.2500 | 2239.78 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **50,264.66 μs** |  **2,169.296 μs** |   **118.906 μs** |  **1.00** |    **0.00** | **100.0000** | **100.0000** | **100.0000** | **7924.07 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 192,868.40 μs | 41,182.003 μs | 2,257.325 μs |  3.84 |    0.04 |        - |        - |        - | 8885.34 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.HacClusterBenchmarks-report-github

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

| Method    | SampleSize | Lags | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|---------- |----------- |----- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Nonrobust** | **100**        | **4**    |     **7.057 μs** |     **1.3520 μs** |   **0.0741 μs** |  **1.00** |    **0.01** |   **0.1602** |        **-** |        **-** |   **2.65 KB** |        **1.00** |
| Hc0       | 100        | 4    |    11.255 μs |     0.8372 μs |   0.0459 μs |  1.60 |    0.02 |   0.5341 |        - |        - |   8.75 KB |        3.30 |
| Hac       | 100        | 4    |    21.160 μs |     1.7258 μs |   0.0946 μs |  3.00 |    0.03 |   0.7629 |        - |        - |   12.9 KB |        4.87 |
| Cluster   | 100        | 4    |    10.247 μs |     0.7830 μs |   0.0429 μs |  1.45 |    0.01 |   0.5798 |        - |        - |   9.71 KB |        3.67 |
|           |            |      |              |               |             |       |         |          |          |          |           |             |
| **Nonrobust** | **10000**      | **4**    |   **460.129 μs** |    **20.0378 μs** |   **1.0983 μs** |  **1.00** |    **0.00** |   **4.3945** |   **0.9766** |        **-** |  **79.99 KB** |        **1.00** |
| Hc0       | 10000      | 4    | 1,151.257 μs |   319.1584 μs |  17.4942 μs |  2.50 |    0.03 | 123.0469 | 123.0469 | 123.0469 | 472.89 KB |        5.91 |
| Hac       | 10000      | 4    | 2,206.617 μs | 3,086.4207 μs | 169.1771 μs |  4.80 |    0.32 | 246.0938 | 246.0938 | 246.0938 | 863.84 KB |       10.80 |
| Cluster   | 10000      | 4    | 1,011.596 μs |   524.7550 μs |  28.7636 μs |  2.20 |    0.05 | 123.0469 | 123.0469 | 123.0469 | 565.18 KB |        7.07 |

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **25.15 μs** |  **0.523 μs** | **0.029 μs** |  **1.00** | **0.9460** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    28.09 μs |  1.168 μs | 0.064 μs |  1.12 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,689.33 μs** | **91.548 μs** | **5.018 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,281.24 μs | 69.645 μs | 3.817 μs |  0.76 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

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

| Method        | Shape    | Mean        | Error      | StdDev    | Gen0   | Allocated |
|-------------- |--------- |------------:|-----------:|----------:|-------:|----------:|
| **TwoSidedExact** | **999x1001** | **3,427.82 μs** | **232.939 μs** | **12.768 μs** |      **-** |  **39.23 KB** |
| **TwoSidedExact** | **99x101**   |    **47.31 μs** |   **4.093 μs** |  **0.224 μs** | **0.2441** |   **4.07 KB** |

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

| Method | Shape   | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| **Exact**  | **141x141** | **993.5 ms** | **357.4 ms** | **19.59 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **43.23 MB** |
| **Exact**  | **8x2500**  | **469.9 ms** | **113.1 ms** |  **6.20 ms** |         **-** |         **-** |         **-** |    **2.9 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

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

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **840.86 ns** |      **23.908 ns** |      **1.310 ns** |   **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,926.15 ns |     247.483 ns |     13.565 ns |   2.29 |    0.01 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,090.74 ns |     199.447 ns |     10.932 ns |   2.49 |    0.01 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     2,977.60 ns |     105.571 ns |      5.787 ns |   3.54 |    0.01 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     5,393.97 ns |   2,364.494 ns |    129.606 ns |   6.41 |    0.13 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     7,256.51 ns |      55.162 ns |      3.024 ns |   8.63 |    0.01 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,048.20 ns |     205.662 ns |     11.273 ns |   2.44 |    0.01 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,023.67 ns |     219.576 ns |     12.036 ns |   3.60 |    0.01 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       793.64 ns |      36.974 ns |      2.027 ns |   0.94 |    0.00 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,728.52 ns |      62.132 ns |      3.406 ns |   3.24 |    0.01 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       821.30 ns |     100.786 ns |      5.524 ns |   0.98 |    0.01 | 0.0486 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |       804.15 ns |     100.147 ns |      5.489 ns |   0.96 |    0.01 | 0.0582 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       340.59 ns |      37.988 ns |      2.082 ns |   0.41 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,182.24 ns |      35.839 ns |      1.964 ns |   1.41 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        88.47 ns |      11.205 ns |      0.614 ns |   0.11 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       520.11 ns |      12.948 ns |      0.710 ns |   0.62 |    0.00 | 0.0572 |     968 B |          NA |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **52,772.52 ns** |   **2,749.291 ns** |    **150.698 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   158,361.29 ns |   1,753.753 ns |     96.129 ns |  3.001 |    0.01 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   368,141.70 ns |  16,222.706 ns |    889.221 ns |  6.976 |    0.02 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,829,580.85 ns |  33,868.339 ns |  1,856.438 ns | 34.669 |    0.09 | 3.9063 |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      |   926,944.33 ns |  46,338.703 ns |  2,539.981 ns | 17.565 |    0.06 |      - |      33 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,145,263.77 ns | 197,714.268 ns | 10,837.386 ns | 59.601 |    0.23 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,283,587.80 ns |  42,254.990 ns |  2,316.139 ns | 24.323 |    0.07 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,046,571.63 ns | 117,857.475 ns |  6,460.166 ns | 38.781 |    0.14 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    73,794.15 ns |     661.702 ns |     36.270 ns |  1.398 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   237,981.23 ns |   7,232.323 ns |    396.428 ns |  4.510 |    0.01 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   141,805.31 ns |   4,044.775 ns |    221.708 ns |  2.687 |    0.01 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   167,858.71 ns |   6,790.996 ns |    372.237 ns |  3.181 |    0.01 | 4.6387 |   80176 B |          NA |
| Lodestar_FisherExact           | 10000      |       341.46 ns |      16.864 ns |      0.924 ns |  0.006 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,206.58 ns |     542.746 ns |     29.750 ns |  0.023 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        91.29 ns |       5.583 ns |      0.306 ns |  0.002 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       530.01 ns |      10.829 ns |      0.594 ns |  0.010 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

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

| Method   | SampleSize | Mean       | Error        | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Allocated   | Alloc Ratio |
|--------- |----------- |-----------:|-------------:|----------:|------:|--------:|---------:|---------:|------------:|------------:|
| **Lodestar** | **200**        |   **137.8 μs** |      **4.75 μs** |   **0.26 μs** |  **1.00** |    **0.00** |   **0.9766** |        **-** |    **19.24 KB** |        **1.00** |
| Accord   | 200        |   993.8 μs |     41.60 μs |   2.28 μs |  7.21 |    0.02 |  87.8906 |   7.8125 |  1466.83 KB |       76.23 |
|          |            |            |              |           |       |         |          |          |             |             |
| **Lodestar** | **2000**       | **1,348.2 μs** |    **194.42 μs** |  **10.66 μs** |  **1.00** |    **0.01** |   **7.8125** |        **-** |   **131.74 KB** |        **1.00** |
| Accord   | 2000       | 8,596.6 μs | 10,109.03 μs | 554.11 μs |  6.38 |    0.36 | 734.3750 | 359.3750 | 12015.21 KB |       91.20 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

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

| Method       | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |     **6.901 μs** |   **0.1702 μs** |  **0.0093 μs** |  **1.00** |    **0.00** |   **0.1602** |        **-** |        **-** |    **2.65 KB** |        **1.00** |
| Accord_Ols   | 100        |   269.367 μs |  39.1291 μs |  2.1448 μs | 39.03 |    0.27 |   2.4414 |        - |        - |      43 KB |       16.24 |
| MathNet_Ols  | 100        |    41.485 μs |  16.5253 μs |  0.9058 μs |  6.01 |    0.11 |   2.0752 |        - |        - |    34.1 KB |       12.88 |
|              |            |              |             |            |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **452.365 μs** |  **62.8765 μs** |  **3.4465 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |   **79.99 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,707.606 μs | 255.2461 μs | 13.9909 μs | 10.41 |    0.07 | 210.9375 | 187.5000 |        - | 3523.47 KB |       44.05 |
| MathNet_Ols  | 10000      | 2,246.838 μs | 718.1713 μs | 39.3654 μs |  4.97 |    0.08 | 441.4063 | 441.4063 | 441.4063 | 1763.75 KB |       22.05 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

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

| Method                       | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|----------------------------- |----------:|---------:|---------:|------:|----------:|------------:|
| NormalQuantile               |  87.36 ns | 2.457 ns | 0.135 ns |  1.00 |         - |          NA |
| NormalQuantileFarTail        |  86.95 ns | 0.585 ns | 0.032 ns |  1.00 |         - |          NA |
| StudentQuantile              | 623.79 ns | 9.361 ns | 0.513 ns |  7.14 |         - |          NA |
| StudentQuantileCauchyFarTail | 205.38 ns | 4.411 ns | 0.242 ns |  2.35 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

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

| Method                    | SampleSize | Ties  | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|-------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,487.2 μs** |     **16.99 μs** |   **0.93 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    428.3 μs |     25.60 μs |   1.40 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    712.7 μs |     35.89 μs |   1.97 μs |  16.6016 |        - |        - |   279681 B |
| WilcoxonPooledDifferences | 10000      | False |  1,269.0 μs |     43.63 μs |   2.39 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    700.7 μs |     27.18 μs |   1.49 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **623.9 μs** |     **18.80 μs** |   **1.03 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    246.7 μs |     18.03 μs |   0.99 μs |   4.3945 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    489.9 μs |     67.02 μs |   3.67 μs |  16.6016 |        - |        - |   279680 B |
| WilcoxonPooledDifferences | 10000      | True  |    787.4 μs |     10.24 μs |   0.56 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    410.5 μs |      3.63 μs |   0.20 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **15,037.3 μs** |  **1,113.18 μs** |  **61.02 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  4,593.2 μs |    314.59 μs |  17.24 μs | 101.5625 | 101.5625 | 101.5625 |   800164 B |
| KruskalWallisManyGroups   | 100000     | False |  9,901.1 μs |  1,399.87 μs |  76.73 μs | 312.5000 | 312.5000 | 312.5000 |  2800334 B |
| WilcoxonPooledDifferences | 100000     | False | 35,536.6 μs |  9,956.88 μs | 545.77 μs | 933.3333 | 933.3333 | 933.3333 | 13200775 B |
| MannWhitneyTest           | 100000     | False |  7,451.4 μs |    957.25 μs |  52.47 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,053.1 μs** |    **118.41 μs** |   **6.49 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  3,070.0 μs |    295.97 μs |  16.22 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  6,071.1 μs |  2,891.90 μs | 158.51 μs | 343.7500 | 343.7500 | 343.7500 |  2800352 B |
| WilcoxonPooledDifferences | 100000     | True  | 22,653.6 μs | 10,764.62 μs | 590.05 μs | 968.7500 | 968.7500 | 968.7500 | 13076301 B |
| MannWhitneyTest           | 100000     | True  |  4,202.7 μs |    144.96 μs |   7.95 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

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

| Method | SampleSize | Covariance | Mean         | Error         | StdDev      | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-------------:|--------------:|------------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |     **6.795 μs** |     **0.4616 μs** |   **0.0253 μs** |   **0.1526** |        **-** |        **-** |    **2.6 KB** |
| **Fit**    | **100**        | **Hc0**        |     **9.972 μs** |     **0.5322 μs** |   **0.0292 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc1**        |    **10.196 μs** |     **4.5774 μs** |   **0.2509 μs** |   **0.5188** |        **-** |        **-** |    **8.7 KB** |
| **Fit**    | **100**        | **Hc2**        |    **11.669 μs** |     **0.1418 μs** |   **0.0078 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **100**        | **Hc3**        |    **11.756 μs** |     **0.6233 μs** |   **0.0342 μs** |   **0.5798** |        **-** |        **-** |   **9.57 KB** |
| **Fit**    | **10000**      | **Nonrobust**  |   **445.324 μs** |   **192.8094 μs** |  **10.5685 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.95 KB** |
| **Fit**    | **10000**      | **Hc0**        | **1,038.137 μs** | **1,328.2986 μs** |  **72.8085 μs** | **124.0234** | **124.0234** | **124.0234** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc1**        | **1,021.523 μs** |    **45.9931 μs** |   **2.5210 μs** | **123.0469** | **123.0469** | **123.0469** | **472.85 KB** |
| **Fit**    | **10000**      | **Hc2**        | **1,182.426 μs** | **1,888.6945 μs** | **103.5257 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |
| **Fit**    | **10000**      | **Hc3**        | **1,141.795 μs** |   **255.8537 μs** |  **14.0242 μs** | **123.0469** | **123.0469** | **123.0469** | **551.06 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

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

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **4.577 μs** | **1.2278 μs** | **0.0673 μs** |  **1.00** |    **0.02** | **0.1526** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  11.542 μs | 0.4216 μs | 0.0231 μs |  2.52 |    0.03 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   5.053 μs | 1.9752 μs | 0.1083 μs |  1.10 |    0.02 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  12.188 μs | 0.3332 μs | 0.0183 μs |  2.66 |    0.03 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   5.078 μs | 1.3920 μs | 0.0763 μs |  1.11 |    0.02 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  11.671 μs | 0.5389 μs | 0.0295 μs |  2.55 |    0.03 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **49.953 μs** | **4.5716 μs** | **0.2506 μs** |  **1.00** |    **0.01** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 126.218 μs | 3.4765 μs | 0.1906 μs |  2.53 |    0.01 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  50.401 μs | 3.7867 μs | 0.2076 μs |  1.01 |    0.01 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 121.748 μs | 7.0557 μs | 0.3867 μs |  2.44 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  51.122 μs | 1.1126 μs | 0.0610 μs |  1.02 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 121.201 μs | 4.1223 μs | 0.2260 μs |  2.43 |    0.01 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

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

| Method                        | SampleSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-------------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |     **7.546 μs** |   **3.8939 μs** |  **0.2134 μs** |  **1.00** |    **0.03** |   **1.4648** |   **0.0458** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |    36.159 μs |  11.2739 μs |  0.6180 μs |  4.79 |    0.14 |   4.0283 |   0.2441 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |    25.346 μs |   0.7897 μs |  0.0433 μs |  3.36 |    0.08 |   0.9766 |   0.0305 |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |     3.943 μs |   0.2981 μs |  0.0163 μs |  0.52 |    0.01 |   0.0992 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |     4.291 μs |   0.4593 μs |  0.0252 μs |  0.57 |    0.01 |   0.2060 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |     1.941 μs |   4.0817 μs |  0.2237 μs |  0.26 |    0.03 |   0.3967 |   0.0038 |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |     3.112 μs |   1.3039 μs |  0.0715 μs |  0.41 |    0.01 |   0.4997 |   0.0076 |        - |    8.16 KB |        0.34 |
|                               |            |              |             |            |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |   **119.039 μs** |  **74.6437 μs** |  **4.0915 μs** |  **1.00** |    **0.04** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 1,156.058 μs | 420.3142 μs | 23.0388 μs |  9.72 |    0.33 | 249.0234 | 249.0234 | 249.0234 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       |   295.524 μs |  56.2976 μs |  3.0859 μs |  2.48 |    0.08 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |    96.303 μs |   7.5558 μs |  0.4142 μs |  0.81 |    0.02 |   0.8545 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |    99.732 μs |   2.6283 μs |  0.1441 μs |  0.84 |    0.02 |   1.8311 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |    15.597 μs |   2.2933 μs |  0.1257 μs |  0.13 |    0.00 |   3.8147 |   0.3967 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |    34.879 μs |   1.8780 μs |  0.1029 μs |  0.29 |    0.01 |   4.7607 |   0.4272 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

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

| Method              | SampleSize | Mean            | Error         | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|--------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |        **967.9 ns** |      **27.43 ns** |      **1.50 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     38,410.5 ns |   4,678.30 ns |    256.43 ns |   39.69 |    0.24 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      2,457.0 ns |      49.14 ns |      2.69 ns |    2.54 |    0.00 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 100        |     23,449.6 ns |   6,489.18 ns |    355.69 ns |   24.23 |    0.32 |   1.3733 |   0.0305 |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |        124.7 ns |       7.45 ns |      0.41 ns |    0.13 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 100        |        222.2 ns |      34.26 ns |      1.88 ns |    0.23 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
|                     |            |                 |               |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **37,641.0 ns** |     **508.89 ns** |     **27.89 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    141,027.1 ns |   8,598.62 ns |    471.32 ns |   3.747 |    0.01 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    707,065.6 ns |  20,934.71 ns |  1,147.50 ns |  18.784 |    0.03 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 12,592,977.7 ns | 561,252.68 ns | 30,764.15 ns | 334.555 |    0.74 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        123.9 ns |       3.10 ns |      0.17 ns |   0.003 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        219.3 ns |       9.25 ns |      0.51 ns |   0.006 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VarianceAndFitBenchmarks-report-github

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

| Method                   | GroupSize | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|-------------:|------------:|-------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **1,626.1 ns** |    **460.98 ns** |    **25.27 ns** |   **1.00** |    **0.02** |  **0.3014** | **0.0019** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     3,109.0 ns |    281.51 ns |    15.43 ns |   1.91 |    0.03 |  0.3242 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       502.2 ns |     40.13 ns |     2.20 ns |   0.31 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |       970.1 ns |     65.74 ns |     3.60 ns |   0.60 |    0.01 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |       605.7 ns |     65.43 ns |     3.59 ns |   0.37 |    0.01 |  0.0029 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     3,434.5 ns |     20.75 ns |     1.14 ns |   2.11 |    0.03 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     6,384.8 ns |    238.58 ns |    13.08 ns |   3.93 |    0.05 |  0.0534 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     5,030.6 ns |     95.21 ns |     5.22 ns |   3.09 |    0.04 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     3,632.7 ns |    161.72 ns |     8.86 ns |   2.23 |    0.03 |       - |      - |      48 B |       0.009 |
|                          |           |                |              |             |        |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **112,334.4 ns** | **47,314.75 ns** | **2,593.48 ns** |  **1.000** |    **0.03** | **28.5645** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,222,935.1 ns | 22,733.06 ns | 1,246.08 ns | 10.890 |    0.22 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    49,245.1 ns |    645.42 ns |    35.38 ns |  0.439 |    0.01 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |    98,362.2 ns |  1,696.95 ns |    93.02 ns |  0.876 |    0.02 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |       767.7 ns |    128.06 ns |     7.02 ns |  0.007 |    0.00 |  0.0029 |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   690,155.1 ns | 66,644.31 ns | 3,653.00 ns |  6.146 |    0.12 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,140,838.6 ns | 35,987.66 ns | 1,972.60 ns | 10.159 |    0.20 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |      ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    28,630.5 ns |    557.65 ns |    30.57 ns |  0.255 |    0.01 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

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

| Method | SampleSize | Variables | Lags | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |---------- |----- |------------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **500**        | **2**         | **1**    |    **22.25 μs** |   **0.415 μs** |  **0.023 μs** |   **2.9602** |   **0.3357** |        **-** |   **48.37 KB** |
| **Fit**    | **500**        | **2**         | **4**    |    **44.40 μs** |   **5.673 μs** |  **0.311 μs** |   **5.8594** |   **1.4038** |        **-** |   **96.13 KB** |
| **Fit**    | **500**        | **5**         | **1**    |    **69.98 μs** |   **3.683 μs** |  **0.202 μs** |   **6.5918** |   **1.5869** |        **-** |  **109.52 KB** |
| **Fit**    | **500**        | **5**         | **4**    |   **176.29 μs** |  **82.308 μs** |  **4.512 μs** |  **14.1602** |   **6.8359** |        **-** |  **233.92 KB** |
| **Fit**    | **5000**       | **2**         | **1**    |   **288.88 μs** | **118.873 μs** |  **6.516 μs** |  **73.7305** |  **73.7305** |  **73.7305** |  **470.29 KB** |
| **Fit**    | **5000**       | **2**         | **4**    |   **674.01 μs** | **219.780 μs** | **12.047 μs** | **221.6797** | **221.6797** | **221.6797** |  **940.03 KB** |
| **Fit**    | **5000**       | **5**         | **1**    |   **992.11 μs** | **573.553 μs** | **31.438 μs** | **142.5781** | **142.5781** | **142.5781** | **1058.83 KB** |
| **Fit**    | **5000**       | **5**         | **4**    | **2,829.85 μs** | **888.039 μs** | **48.676 μs** | **496.0938** | **496.0938** | **496.0938** | **2238.16 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

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

| Method       | SampleSize | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |-------------:|--------------:|-------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |     **11.85 μs** |      **0.966 μs** |     **0.053 μs** |  **1.00** |    **0.01** |    **0.1984** |         **-** |         **-** |     **3.43 KB** |        **1.00** |
| MathNet_Wls  | 200        |     36.41 μs |     16.087 μs |     0.882 μs |  3.07 |    0.07 |    3.6621 |    0.1831 |         - |    58.19 KB |       16.97 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **98.46 μs** |      **8.267 μs** |     **0.453 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |         **-** |    **17.49 KB** |        **1.00** |
| MathNet_Wls  | 2000       |    197.75 μs |    161.909 μs |     8.875 μs |  2.01 |    0.08 |   32.7148 |   12.6953 |         - |   508.04 KB |       29.04 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |  **1,068.15 μs** |    **525.197 μs** |    **28.788 μs** |  **1.00** |    **0.03** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.44 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  2,881.88 μs |  4,134.016 μs |   226.599 μs |  2.70 |    0.19 | 1070.3125 | 1070.3125 | 1070.3125 |  5024.18 KB |       31.71 |
|              |            |              |               |              |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **9,707.20 μs** |  **1,402.533 μs** |    **76.878 μs** |  **1.00** |    **0.01** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.95 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 22,216.98 μs | 47,264.883 μs | 2,590.748 μs |  2.29 |    0.23 |  812.5000 |  812.5000 |  812.5000 | 50022.21 KB |       31.96 |

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

| Method                  | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------------------------ |---------:|---------:|---------:|----------:|----------:|
| ChatTemplate            | 97.59 ms | 6.339 ms | 0.347 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens | 82.70 ms | 0.013 ms | 0.001 ms | 1714.2857 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,478.7 μs** |      **44.16 μs** |      **2.42 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    71,324.7 μs |  11,077.28 μs |    607.18 μs |  48.24 |    0.36 |  3857.1429 |  1571.4286 |  857.1429 |  59881.15 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,401.0 μs** |      **21.60 μs** |      **1.18 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    76,195.8 μs |  45,735.20 μs |  2,506.90 μs |  54.39 |    1.55 |  5000.0000 |  2000.0000 | 1000.0000 |  83111.89 KB |       81.29 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **554.2 μs** |      **41.93 μs** |      **2.30 μs** |   **1.00** |    **0.01** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   110,421.8 μs |  25,148.16 μs |  1,378.46 μs | 199.25 |    2.27 |  4800.0000 |  1800.0000 |  800.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,703.9 μs** |     **130.43 μs** |      **7.15 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    67,056.4 μs |  10,580.62 μs |    579.96 μs |  39.35 |    0.33 |  3888.8889 |  1777.7778 |  888.8889 |  64191.22 KB |       62.78 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **11,017.2 μs** |   **2,936.54 μs** |    **160.96 μs** |   **1.00** |    **0.02** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,448,052.8 μs | 114,000.67 μs |  6,248.76 μs | 131.45 |    1.72 | 35000.0000 | 10000.0000 | 4000.0000 | 537148.57 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **10,296.9 μs** |     **375.70 μs** |     **20.59 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,387,006.5 μs |  31,724.06 μs |  1,738.90 μs | 134.70 |    0.28 | 48000.0000 | 12000.0000 | 4000.0000 |  747353.3 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **4,557.0 μs** |     **330.03 μs** |     **18.09 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 2,802,338.7 μs |  98,113.61 μs |  5,377.94 μs | 614.95 |    2.34 | 47000.0000 | 12000.0000 | 4000.0000 |  732674.2 KB |    8,148.07 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **13,238.2 μs** |   **1,577.10 μs** |     **86.45 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,347,601.4 μs | 343,275.29 μs | 18,816.08 μs | 101.80 |    1.36 | 38000.0000 | 11000.0000 | 4000.0000 | 573618.81 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactLoadOnlyBenchmarks-report-github

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

| Method           | Mean     | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|----------------- |---------:|----------:|---------:|----------:|----------:|---------:|----------:|
| CountLoadAsync2M | 19.57 ms |  5.017 ms | 0.275 ms |  750.0000 |  687.5000 | 343.7500 |  21.53 MB |
| CountLoadAsync4M | 37.96 ms | 19.455 ms | 1.066 ms | 1285.7143 | 1214.2857 | 500.0000 |  43.93 MB |
| CountLoad4M      | 35.72 ms | 14.162 ms | 0.776 ms | 1428.5714 | 1357.1429 | 642.8571 |  43.92 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ArtifactTimeBenchmarks-report-github

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

| Method                | Mean          | Error         | StdDev       | Gen0      | Gen1      | Gen2     | Allocated   |
|---------------------- |--------------:|--------------:|-------------:|----------:|----------:|---------:|------------:|
| CountFitLongDocuments | 139,682.06 μs | 10,163.796 μs |   557.112 μs |         - |         - |        - | 54334.46 KB |
| CountSave240K         |     316.95 μs |     52.541 μs |     2.880 μs |    9.2773 |    2.9297 |   2.9297 |   504.66 KB |
| CountSaveAsync240K    |     353.85 μs |     40.336 μs |     2.211 μs |   10.2539 |    3.9063 |   3.9063 |   739.44 KB |
| TfidfSave45K          |      39.25 μs |      3.234 μs |     0.177 μs |    8.3618 |    1.6479 |        - |   137.13 KB |
| TfidfSaveAsync45K     |      41.17 μs |     12.670 μs |     0.695 μs |   11.1084 |         - |        - |   181.88 KB |
| IndexSave942K         |     209.15 μs |     44.750 μs |     2.453 μs |    7.5684 |    1.4648 |   0.9766 |    243.5 KB |
| IndexSaveAsync942K    |     205.57 μs |     46.372 μs |     2.542 μs |    7.8125 |    0.9766 |   0.9766 |   243.62 KB |
| CountLoadAsync2M      |  21,119.70 μs | 25,607.998 μs | 1,403.661 μs |  593.7500 |  562.5000 | 187.5000 | 22048.66 KB |
| CountLoadAsync4M      |  45,133.53 μs | 15,852.200 μs |   868.913 μs | 1181.8182 | 1090.9091 | 363.6364 | 44983.59 KB |
| CountLoad4M           |  42,115.02 μs | 10,782.921 μs |   591.048 μs | 1166.6667 | 1083.3333 | 333.3333 | 44976.82 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AsyncArtifactBenchmarks-report-github

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

| Method                 | Mean          | Error        | StdDev      | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |--------------:|-------------:|------------:|----------:|----------:|---------:|------------:|
| TfidfSaveAsyncSmall    |     15.227 μs |     8.861 μs |   0.4857 μs |    1.6785 |    0.0916 |        - |    27.43 KB |
| CountSaveAsyncWide     |    389.228 μs |   118.235 μs |   6.4809 μs |   37.1094 |   30.7617 |  30.7617 |   740.15 KB |
| HashingLoadUndeclared  |      3.725 μs |     1.068 μs |   0.0586 μs |    5.0964 |         - |        - |    83.87 KB |
| TfidfLoadUndeclared    |     22.038 μs |     6.648 μs |   0.3644 μs |    7.4158 |    0.8240 |        - |   121.83 KB |
| CountLoadUndeclared24K |     96.670 μs |    19.860 μs |   1.0886 μs |   16.1133 |    3.1738 |        - |   264.67 KB |
| CountLoadUndeclared78K |    679.762 μs |   379.431 μs |  20.7979 μs |   37.1094 |   35.1563 |   7.8125 |   656.34 KB |
| CountLoadUndeclared1M  | 10,129.932 μs | 4,486.079 μs | 245.8971 μs |  406.2500 |  375.0000 | 187.5000 | 10794.01 KB |
| CountSaveAsync1M       |  1,599.414 μs |   286.169 μs |  15.6859 μs |  128.9063 |  121.0938 | 121.0938 |  3162.18 KB |
| IndexSaveAsyncLongHead |  2,667.785 μs |   298.433 μs |  16.3581 μs |   82.0313 |   78.1250 |  78.1250 |  2124.21 KB |
| CountLoadUndeclared2M  | 14,275.111 μs |   583.463 μs |  31.9815 μs | 1046.8750 | 1031.2500 | 625.0000 | 22045.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **4.332 μs** |  **0.2525 μs** | **0.0138 μs** |  **1.00** |  **0.1373** |      **-** |   **2.27 KB** |        **1.00** |
| EmbedBatch         | 1          |   4.563 μs |  0.3611 μs | 0.0198 μs |  1.05 |  0.1602 |      - |    2.7 KB |        1.19 |
| EmbedBatchBucketed | 1          |   4.518 μs |  0.4162 μs | 0.0228 μs |  1.04 |  0.1602 |      - |    2.7 KB |        1.19 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **42.885 μs** |  **1.8213 μs** | **0.0998 μs** |  **1.00** |  **1.7700** |      **-** |  **29.48 KB** |        **1.00** |
| EmbedBatch         | 8          |  16.816 μs |  3.4964 μs | 0.1917 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
| EmbedBatchBucketed | 8          |  16.841 μs |  4.2838 μs | 0.2348 μs |  0.39 |  1.3428 |      - |  22.27 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **170.336 μs** |  **1.5079 μs** | **0.0827 μs** |  **1.00** |  **6.5918** |      **-** | **110.99 KB** |        **1.00** |
| EmbedBatch         | 32         |  65.526 μs |  8.7254 μs | 0.4783 μs |  0.38 |  5.0049 | 0.2441 |  82.64 KB |        0.74 |
| EmbedBatchBucketed | 32         |  56.774 μs |  2.2540 μs | 0.1236 μs |  0.33 |  4.2114 | 0.1221 |  69.13 KB |        0.62 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **677.752 μs** | **79.2728 μs** | **4.3452 μs** |  **1.00** | **26.3672** |      **-** |  **443.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 254.309 μs | 42.9315 μs | 2.3532 μs |  0.38 | 20.0195 | 2.4414 | 329.67 KB |        0.74 |
| EmbedBatchBucketed | 128        | 212.217 μs | 23.9052 μs | 1.3103 μs |  0.31 | 15.8691 | 1.9531 | 262.16 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

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

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **39.04 ms** | **56.935 ms** | **3.121 ms** |  **714.2857** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **39.70 ms** |  **5.592 ms** | **0.307 ms** |  **692.3077** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **35.17 ms** |  **2.069 ms** | **0.113 ms** |  **666.6667** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **48.11 ms** |  **0.424 ms** | **0.023 ms** | **1272.7273** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **39.73 ms** | **24.277 ms** | **1.331 ms** | **1307.6923** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **52.43 ms** |  **0.860 ms** | **0.047 ms** | **1500.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **112.93 ms** | **11.335 ms** | **0.621 ms** |  **1.00** |   **27.17 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  57.92 ms |  5.619 ms | 0.308 ms |  0.51 |  103.66 KB |        3.82 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **123.05 ms** | **16.270 ms** | **0.892 ms** |  **1.00** |   **23.72 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  52.89 ms |  3.873 ms | 0.212 ms |  0.43 |  116.45 KB |        4.91 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **169.13 ms** |  **2.072 ms** | **0.114 ms** |  **1.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 213.50 ms |  5.441 ms | 0.298 ms |  1.26 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **174.22 ms** |  **8.881 ms** | **0.487 ms** |  **1.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 185.75 ms | 14.168 ms | 0.777 ms |  1.07 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **214.12 ms** |  **2.171 ms** | **0.119 ms** |  **1.00** |  **949.52 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 296.10 ms |  6.017 ms | 0.330 ms |  1.38 | 1365.78 KB |        1.44 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **217.45 ms** | **18.229 ms** | **0.999 ms** |  **1.00** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 278.02 ms | 19.829 ms | 1.087 ms |  1.28 | 1152.95 KB |        1.56 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **249.73 ms** | **13.774 ms** | **0.755 ms** |  **1.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 341.87 ms | 13.537 ms | 0.742 ms |  1.37 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **253.84 ms** |  **0.905 ms** | **0.050 ms** |  **1.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 342.68 ms | 21.707 ms | 1.190 ms |  1.35 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **38.20 μs** |      **0.215 μs** |     **0.012 μs** |         **-** |
| Cjk    | 1000   |      49.72 μs |      1.795 μs |     0.098 μs |         - |
| **Latin**  | **10000**  |   **3,546.35 μs** |     **23.073 μs** |     **1.265 μs** |         **-** |
| Cjk    | 10000  |   5,480.22 μs |    193.016 μs |    10.580 μs |         - |
| **Latin**  | **65536**  | **162,205.38 μs** | **24,351.752 μs** | **1,334.802 μs** |         **-** |

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

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 23.22 ms | 2.451 ms | 0.134 ms |  1.00 |    0.01 |  500.0000 |   8.19 MB |        1.00 |
| Bpe     | 72.56 ms | 2.995 ms | 0.164 ms |  3.12 |    0.02 | 1714.2857 |  28.47 MB |        3.48 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **14.21 μs** |  **1.116 μs** | **0.061 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **30.09 μs** |  **1.831 μs** | **0.100 μs** | **0.8850** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **81.85 μs** |  **6.580 μs** | **0.361 μs** | **1.7090** |      **-** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **246.94 μs** | **47.990 μs** | **2.630 μs** | **3.4180** | **0.2441** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 6.940 ms | 2.540 ms | 0.1392 ms |   1.83 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **15.333 μs** | **0.6720 μs** | **0.0368 μs** |         **-** |
| MyersGroup | cjk      | 130.715 μs | 0.4187 μs | 0.0230 μs |         - |
| **DpGroup**    | **latin**    |   **8.037 μs** | **0.2537 μs** | **0.0139 μs** |         **-** |
| MyersGroup | latin    |  87.696 μs | 1.3052 μs | 0.0715 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **197.83 μs** | **17.469 μs** | **0.958 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 204.05 μs | 22.095 μs | 1.211 μs |  1.03 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **222.71 μs** | **68.638 μs** | **3.762 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 236.48 μs | 47.627 μs | 2.611 μs |  1.06 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **201.83 μs** | **12.021 μs** | **0.659 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 219.85 μs |  9.542 μs | 0.523 μs |  1.09 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **168.41 μs** |  **4.607 μs** | **0.253 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 238.68 μs | 20.602 μs | 1.129 μs |  1.42 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **194.73 μs** |  **5.087 μs** | **0.279 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 213.20 μs | 18.991 μs | 1.041 μs |  1.09 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **107.33 μs** |  **2.975 μs** | **0.163 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 230.08 μs | 17.235 μs | 0.945 μs |  2.14 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **147.08 μs** |  **6.624 μs** | **0.363 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 211.62 μs | 96.503 μs | 5.290 μs |  1.44 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **61.35 μs** |  **4.723 μs** | **0.259 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 228.93 μs |  9.951 μs | 0.545 μs |  3.73 |    0.02 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **119.8 μs** |   **2.39 μs** |  **0.13 μs** |  **1.00** |   **1.0986** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   112.2 μs |   4.55 μs |  0.25 μs |  0.94 |   1.0986 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   336.1 μs |  27.04 μs |  1.48 μs |  2.80 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **2,551.7 μs** | **108.38 μs** |  **5.94 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 2,480.8 μs | 176.77 μs |  9.69 μs |  0.97 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 5,878.0 μs | 414.49 μs | 22.72 μs |  2.30 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

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

| Method                         | Samples | Clusters | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.167 ms** | **0.2574 ms** | **0.0141 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.130 ms | 0.2315 ms | 0.0127 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  21.033 ms | 0.1634 ms | 0.0090 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.187 ms** | **0.0820 ms** | **0.0045 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.145 ms | 0.0221 ms | 0.0012 ms | 187.5000 | 187.5000 | 187.5000 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 183.833 ms | 9.0898 ms | 0.4982 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

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

| Method   | Length | Mean        | Error    | StdDev   | Gen0   | Allocated |
|--------- |------- |------------:|---------:|---------:|-------:|----------:|
| **Distance** | **12**     |    **988.6 ns** | **576.6 ns** | **31.61 ns** | **0.0496** |     **840 B** |
| **Distance** | **120**    | **64,266.7 ns** | **763.8 ns** | **41.86 ns** | **0.1221** |    **2128 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

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

| Method       | Shape     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **139.17 ms** | **33.318 ms** | **1.826 ms** |  **1.00** |    **0.02** |  **1750.0000** | **1750.0000** | **1750.0000** |  **29.42 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,047.08 ms | 67.137 ms | 3.680 ms | 14.71 |    0.17 | 14000.0000 | 8000.0000 | 7000.0000 | 156.32 MB |        5.31 |
|              |           |             |           |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |    **46.65 ms** |  **0.574 ms** | **0.031 ms** |  **1.00** |    **0.00** |  **1454.5455** | **1454.5455** | **1454.5455** |  **13.96 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   701.38 ms | 35.810 ms | 1.963 ms | 15.03 |    0.04 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.28 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                   | Shape      | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|----------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **125.83 ms** | **34.658 ms** | **1.900 ms** |  **1.00** |    **0.02** |  **1200.0000** |  **1200.0000** |  **1200.0000** | **112.66 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 5,561.69 ms | 51.187 ms | 2.806 ms | 44.21 |    0.57 | 38000.0000 | 12000.0000 | 11000.0000 | 531.86 MB |        4.72 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,146.75 ms | 78.875 ms | 4.323 ms | 25.01 |    0.33 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.30 |
|                          |            |             |           |          |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **14.70 ms** |  **1.841 ms** | **0.101 ms** |  **1.00** |    **0.01** |  **1343.7500** |  **1343.7500** |  **1343.7500** |  **14.16 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   416.79 ms | 30.150 ms | 1.653 ms | 28.35 |    0.19 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.70 |
| Dbscan_CalculateClusters | 5000x2x5   |   209.53 ms | 13.066 ms | 0.716 ms | 14.25 |    0.09 |  3666.6667 |  3666.6667 |  3666.6667 |   40.4 MB |        2.85 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean      | Error      | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|-----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       | 10.288 ms |  0.8760 ms | 0.0480 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 78.848 ms | 31.7419 ms | 1.7399 ms |  7.66 |    0.15 |
| Nmf_Transform                             |  2.674 ms |  0.0984 ms | 0.0054 ms |  0.26 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 19.251 ms |  4.8384 ms | 0.2652 ms |  1.87 |    0.02 |

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 284.1 μs | 179.6 μs | 9.84 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

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

| Method      | Count  | Mean       | Error    | StdDev   | Allocated |
|------------ |------- |-----------:|---------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **530.8 μs** | **160.0 μs** |  **8.77 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **7,865.7 μs** | **933.7 μs** | **51.18 μs** |   **1.64 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

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

| Method              | Rows   | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|----------:|---------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **706.3 μs** | **119.91 μs** |  **6.57 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  7,501.1 μs | 765.55 μs | 41.96 μs |        - |        - |        - |     21.02 KB |
| **FromBlockNormalized** | **100000** | **57,806.5 μs** | **288.10 μs** | **15.79 μs** | **222.2222** | **222.2222** | **222.2222** | **150000.24 KB** |
| MmrSelect100        | 100000 |  6,919.1 μs |  68.96 μs |  3.78 μs |        - |        - |        - |     21.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  Job-RZVRTL : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-RZVRTL** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **213.34 μs** |    **16.287 μs** |    **16.725 μs** |  **1.01** |    **0.11** |        **-** |        **-** |        **-** |  **166.08 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 1000     |    230.44 μs |    44.967 μs |    49.981 μs |  1.09 |    0.24 |        - |        - |        - |   17.53 KB |        0.11 |
| Lodestar_Impute                 | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 1000     |    172.36 μs |    12.214 μs |    13.575 μs |  0.81 |    0.09 |        - |        - |        - |   93.25 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 1000     |    352.47 μs |    21.569 μs |    24.839 μs |  1.66 |    0.17 |        - |        - |        - |   34.19 KB |        0.21 |
| MlNet_OneHotEncoding_Read       | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,427.55 μs |    71.742 μs |    76.764 μs |  6.73 |    0.61 |        - |        - |        - |  312.95 KB |        1.88 |
| MlNet_ReplaceMissingValues_Read | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,168.29 μs |    56.213 μs |    62.481 μs |  5.51 |    0.50 |        - |        - |        - |  286.89 KB |        1.73 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    155.83 μs |    67.464 μs |     3.698 μs |  1.00 |    0.03 |  49.8047 |  49.8047 |  49.8047 |  165.37 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     65.57 μs |     6.175 μs |     0.338 μs |  0.42 |    0.01 |   0.9766 |        - |        - |   16.81 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     22.15 μs |     1.905 μs |     0.104 μs |  0.14 |    0.00 |   5.6458 |   0.1526 |        - |   92.53 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    888.77 μs | 2,368.414 μs |   129.821 μs |  5.71 |    0.73 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.96 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    946.53 μs | 1,034.189 μs |    56.687 μs |  6.08 |    0.34 |  31.2500 |   7.8125 |        - |  563.66 KB |        3.41 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    256.86 μs |    38.419 μs |     2.106 μs |  1.65 |    0.04 |  18.5547 |  18.0664 |        - |  293.38 KB |        1.77 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-RZVRTL** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **4,092.57 μs** |   **275.635 μs** |   **283.057 μs** |  **1.00** |    **0.10** |        **-** |        **-** |        **-** | **3283.27 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,757.05 μs |   126.616 μs |   124.354 μs |  0.92 |    0.07 |        - |        - |        - |  314.41 KB |        0.10 |
| Lodestar_Impute                 | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,414.29 μs |    22.329 μs |    25.714 μs |  0.35 |    0.03 |        - |        - |        - | 1843.77 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,560.34 μs |    51.730 μs |    55.351 μs |  0.87 |    0.06 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,105.94 μs |   485.457 μs |   454.097 μs |  3.71 |    0.28 |        - |        - |        - |  440.26 KB |        0.13 |
| MlNet_ReplaceMissingValues_Read | Job-RZVRTL | 1               | 20             | Throughput  | 1            | 5           | 20000    |  8,754.11 μs | 4,180.491 μs | 4,814.260 μs |  2.15 |    1.17 |        - |        - |        - |  465.27 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,445.27 μs |   113.152 μs |     6.202 μs |  1.00 |    0.00 | 992.1875 | 992.1875 | 992.1875 | 3290.98 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,165.93 μs |   603.118 μs |    33.059 μs |  0.92 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.36 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    763.81 μs |   902.180 μs |    49.452 μs |  0.22 |    0.01 | 317.3828 | 317.3828 | 317.3828 | 1845.81 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,466.78 μs | 1,225.892 μs |    67.195 μs |  0.43 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,245.91 μs | 1,540.435 μs |    84.436 μs |  1.23 |    0.02 |  31.2500 |  15.6250 |        - |  571.27 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,534.12 μs |   273.874 μs |    15.012 μs |  0.74 |    0.00 |  27.3438 |  11.7188 |        - |  451.87 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

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

| Method          | Records | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2    | Allocated   |
|---------------- |-------- |------------:|------------:|----------:|---------:|---------:|--------:|------------:|
| **FilteredTop10**   | **10000**   |    **473.3 μs** |    **56.20 μs** |   **3.08 μs** |        **-** |        **-** |       **-** |      **7.2 KB** |
| Unfiltered      | 10000   |    525.4 μs |    24.27 μs |   1.33 μs |        - |        - |       - |      2.8 KB |
| HybridSelective | 10000   |  2,736.9 μs |   161.16 μs |   8.83 μs | 144.5313 | 121.0938 | 58.5938 |  2577.95 KB |
| HybridBroad     | 10000   |  4,372.1 μs |   565.41 μs |  30.99 μs | 164.0625 | 125.0000 | 62.5000 |  3221.11 KB |
| **FilteredTop10**   | **100000**  |  **9,799.2 μs** |   **812.64 μs** |  **44.54 μs** |        **-** |        **-** |       **-** |     **7.21 KB** |
| Unfiltered      | 100000  |  8,154.0 μs |   435.08 μs |  23.85 μs |        - |        - |       - |     2.81 KB |
| HybridSelective | 100000  | 40,355.2 μs |   994.57 μs |  54.52 μs | 230.7692 | 153.8462 |       - | 23470.98 KB |
| HybridBroad     | 100000  | 55,260.7 μs | 6,528.46 μs | 357.85 μs | 222.2222 | 111.1111 |       - | 29359.82 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  84.29 ns |   0.832 ns |  0.046 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   | 624.56 ns |  41.977 ns |  2.301 ns |  7.41 |    0.02 |      - |         - |          NA |
| TokenSortRatio | 733.68 ns | 181.461 ns |  9.946 ns |  8.70 |    0.10 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 731.24 ns |  91.316 ns |  5.005 ns |  8.67 |    0.05 | 0.0534 |     896 B |          NA |
| WRatio         | 960.87 ns | 325.953 ns | 17.867 ns | 11.40 |    0.18 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,654,749.2367 ns |    35,745.5053 ns |   1,959.3318 ns |  1.000 |    0.00 |  7.8125 |       - |  160506 B |        1.00 |
| TokenSortRatio_EmojiWords |    503,377.5332 ns |   144,922.2010 ns |   7,943.6749 ns |  0.304 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |    935,147.4440 ns |    27,144.3611 ns |   1,487.8740 ns |  0.565 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 60,259,063.7407 ns | 2,483,193.4644 ns | 136,112.2135 ns | 36.416 |    0.08 |       - |       - |  510783 B |        3.18 |
| WRatio_EmptyOperand       |          0.0000 ns |         0.0000 ns |       0.0000 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **84.74 ns** |   **2.154 ns** |  **0.118 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   178.40 ns |   7.863 ns |  0.431 ns |  2.11 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **616.08 ns** |   **0.642 ns** |  **0.035 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 7,834.41 ns | 490.021 ns | 26.860 ns | 12.72 |    0.04 |      - |     160 B |          NA |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **748.16 ns** |  **79.388 ns** |  **4.352 ns** |  **1.00** |    **0.01** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,577.77 ns | 592.197 ns | 32.460 ns |  2.11 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |             |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **924.35 ns** | **303.747 ns** | **16.649 ns** |  **1.00** |    **0.02** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 3,793.92 ns |  90.256 ns |  4.947 ns |  4.11 |    0.06 | 0.1831 |    3096 B |        2.76 |

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **22.92 ns** |   **0.954 ns** |  **0.052 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    28.30 ns |   0.921 ns |  0.050 ns |  1.23 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    24.27 ns |   3.625 ns |  0.199 ns |  1.06 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    22.00 ns |   0.268 ns |  0.015 ns |  0.96 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **25.03 ns** |   **0.462 ns** |  **0.025 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    28.26 ns |   0.655 ns |  0.036 ns |  1.13 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    24.16 ns |   1.088 ns |  0.060 ns |  0.97 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    24.98 ns |   0.316 ns |  0.017 ns |  1.00 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **25.94 ns** |   **1.469 ns** |  **0.080 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    30.89 ns |   1.053 ns |  0.058 ns |  1.19 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    27.65 ns |   0.878 ns |  0.048 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    24.96 ns |   0.487 ns |  0.027 ns |  0.96 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **30.32 ns** |   **1.165 ns** |  **0.064 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    35.77 ns |   2.543 ns |  0.139 ns |  1.18 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    30.34 ns |   0.465 ns |  0.025 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    27.18 ns |   5.395 ns |  0.296 ns |  0.90 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **49.05 ns** |   **0.737 ns** |  **0.040 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    57.79 ns |   6.952 ns |  0.381 ns |  1.18 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    48.57 ns |   3.923 ns |  0.215 ns |  0.99 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    48.36 ns |   1.661 ns |  0.091 ns |  0.99 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **54.53 ns** |   **2.280 ns** |  **0.125 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    57.91 ns |   1.417 ns |  0.078 ns |  1.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    58.66 ns |   0.748 ns |  0.041 ns |  1.08 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    52.33 ns |   1.747 ns |  0.096 ns |  0.96 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **315.24 ns** |   **2.602 ns** |  **0.143 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   329.08 ns |   4.294 ns |  0.235 ns |  1.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   315.69 ns |   1.826 ns |  0.100 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   315.24 ns |   9.998 ns |  0.548 ns |  1.00 |         - |          NA |
|                            |        |             |            |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,027.47 ns** |  **46.075 ns** |  **2.526 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 3,976.38 ns | 893.368 ns | 48.969 ns |  0.99 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 3,886.01 ns |  72.135 ns |  3.954 ns |  0.96 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,896.38 ns | 128.941 ns |  7.068 ns |  0.97 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **326.9 ns** |   **8.34 ns** |  **0.46 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    157.7 ns |   0.68 ns |  0.04 ns |  0.48 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **1,939.4 ns** | **100.79 ns** |  **5.52 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  2,586.5 ns | 122.17 ns |  6.70 ns |  1.33 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **11,187.0 ns** | **467.65 ns** | **25.63 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 25,727.7 ns | 995.99 ns | 54.59 ns |  2.30 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **6,077.7 ns** | **1,274.13 ns** |  **69.84 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  6,133.0 ns | 1,298.24 ns |  71.16 ns |  1.01 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      |  9,158.8 ns | 1,369.89 ns |  75.09 ns |  1.51 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **7,596.3 ns** |   **128.52 ns** |   **7.04 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  7,510.1 ns |   149.75 ns |   8.21 ns |  0.99 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 11,763.9 ns |   914.57 ns |  50.13 ns |  1.55 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     |  **9,609.5 ns** | **1,284.16 ns** |  **70.39 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     |  9,466.2 ns |   501.79 ns |  27.50 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 14,916.3 ns |    63.20 ns |   3.46 ns |  1.55 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **4,334.1 ns** |   **200.16 ns** |  **10.97 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  4,979.0 ns | 1,483.96 ns |  81.34 ns |  1.15 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  7,343.9 ns |   272.33 ns |  14.93 ns |  1.69 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **5,685.0 ns** |   **152.14 ns** |   **8.34 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  6,234.6 ns | 2,382.86 ns | 130.61 ns |  1.10 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     |  9,789.7 ns |   979.68 ns |  53.70 ns |  1.72 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     |  **7,570.3 ns** |   **121.80 ns** |   **6.68 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  8,111.5 ns |   128.82 ns |   7.06 ns |  1.07 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 13,523.6 ns |   921.44 ns |  50.51 ns |  1.79 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **3,144.1 ns** |    **54.91 ns** |   **3.01 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  3,382.3 ns |   426.12 ns |  23.36 ns |  1.08 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  6,123.5 ns |    54.61 ns |   2.99 ns |  1.95 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **4,549.2 ns** |    **37.88 ns** |   **2.08 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  4,910.1 ns |   340.27 ns |  18.65 ns |  1.08 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     |  8,704.2 ns | 1,878.00 ns | 102.94 ns |  1.91 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **6,537.1 ns** |   **274.33 ns** |  **15.04 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  6,690.1 ns |   250.66 ns |  13.74 ns |  1.02 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 12,425.3 ns |   316.95 ns |  17.37 ns |  1.90 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **1,980.6 ns** |     **5.32 ns** |   **0.29 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  1,703.7 ns |   124.54 ns |   6.83 ns |  0.86 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  4,810.2 ns |   190.76 ns |  10.46 ns |  2.43 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **3,259.7 ns** |    **19.38 ns** |   **1.06 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,331.2 ns |   113.02 ns |   6.20 ns |  0.72 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     |  7,872.2 ns |   231.79 ns |  12.70 ns |  2.42 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **5,379.1 ns** |   **557.91 ns** |  **30.58 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  3,193.9 ns |   422.46 ns |  23.16 ns |  0.59 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 11,367.7 ns |   392.42 ns |  21.51 ns |  2.11 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,058.5 ns** |    **66.37 ns** |   **3.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  2,781.8 ns |   108.57 ns |   5.95 ns |  0.91 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  6,253.2 ns | 2,256.60 ns | 123.69 ns |  2.04 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **4,439.5 ns** |    **25.40 ns** |   **1.39 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  3,455.5 ns |   503.93 ns |  27.62 ns |  0.78 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     |  8,448.1 ns |   218.25 ns |  11.96 ns |  1.90 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **6,533.4 ns** | **1,245.37 ns** |  **68.26 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  6,228.3 ns |   335.28 ns |  18.38 ns |  0.95 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 11,801.6 ns |   584.32 ns |  32.03 ns |  1.81 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,018.3 ns** |   **145.82 ns** |   **7.99 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    467.9 ns |    29.70 ns |   1.63 ns |  0.46 |    0.00 | 0.0024 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  4,024.8 ns |   372.46 ns |  20.42 ns |  3.95 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **2,407.3 ns** |    **48.73 ns** |   **2.67 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,112.6 ns |    94.19 ns |   5.16 ns |  0.46 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  6,556.0 ns |   601.34 ns |  32.96 ns |  2.72 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **4,378.3 ns** |    **90.33 ns** |   **4.95 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  1,920.8 ns |    98.42 ns |   5.39 ns |  0.44 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 10,291.9 ns |    95.08 ns |   5.21 ns |  2.35 |    0.00 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

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

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **4,101.7 μs** |    **333.02 μs** |    **18.25 μs** |  **1.00** |    **0.01** | **250.0000** | **242.1875** | **242.1875** | **1495.92 KB** |       **1.000** |
| NumFlat_Fit      | 10000x16x16 |  28,968.6 μs | 14,432.40 μs |   791.09 μs |  7.06 |    0.17 |        - |        - |        - |   10.33 KB |       0.007 |
| MetaNumerics_Fit | 10000x16x16 |  73,231.5 μs | 51,279.14 μs | 2,810.78 μs | 17.85 |    0.60 |        - |        - |        - |   41.89 KB |       0.028 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **666.0 μs** |     **62.16 μs** |     **3.41 μs** |  **1.00** |    **0.01** |  **49.8047** |  **49.8047** |  **49.8047** |  **391.94 KB** |       **1.000** |
| NumFlat_Fit      | 10000x2x8   |   4,217.6 μs |     84.91 μs |     4.65 μs |  6.33 |    0.03 |        - |        - |        - |       3 KB |       0.008 |
| MetaNumerics_Fit | 10000x2x8   |   2,220.7 μs |     91.94 μs |     5.04 μs |  3.33 |    0.02 |        - |        - |        - |   39.57 KB |       0.101 |
|                  |             |              |              |             |       |         |          |          |          |            |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **19,861.1 μs** |  **1,106.58 μs** |    **60.66 μs** |  **1.00** |    **0.00** | **343.7500** | **343.7500** | **343.7500** | **4309.02 KB** |       **1.000** |
| NumFlat_Fit      | 50000x8x32  | 352,209.7 μs | 86,387.06 μs | 4,735.17 μs | 17.73 |    0.21 |        - |        - |        - |   16.01 KB |       0.004 |
| MetaNumerics_Fit | 50000x8x32  | 561,418.2 μs | 73,383.45 μs | 4,022.39 μs | 28.27 |    0.19 |        - |        - |        - |  199.91 KB |       0.046 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **3.881 ms** |  **0.2363 ms** | **0.0130 ms** |  **1.00** |    **0.00** | **250.0000** | **242.1875** | **242.1875** | **1426.34 KB** |       **1.000** |
| NumFlat_Lloyd  | 10000x16x16 |  13.335 ms |  4.1531 ms | 0.2276 ms |  3.44 |    0.05 |        - |        - |        - |   13.58 KB |       0.010 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **19.422 ms** | **23.7075 ms** | **1.2995 ms** |  **1.00** |    **0.08** |  **31.2500** |  **31.2500** |  **31.2500** |   **360.4 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  95.711 ms | 25.2461 ms | 1.3838 ms |  4.94 |    0.28 |        - |        - |        - |   96.44 KB |        0.27 |
|                |             |            |            |           |       |         |          |          |          |            |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **36.406 ms** |  **0.8440 ms** | **0.0463 ms** |  **1.00** |    **0.00** | **285.7143** | **285.7143** | **285.7143** | **3947.39 KB** |       **1.000** |
| NumFlat_Lloyd  | 50000x8x32  | 172.867 ms | 21.1949 ms | 1.1618 ms |  4.75 |    0.03 |        - |        - |        - |   36.47 KB |       0.009 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |   **101.74 ns** |     **1.142 ns** |   **0.063 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    48.14 ns |     0.530 ns |   0.029 ns |  0.47 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   101.88 ns |     0.187 ns |   0.010 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    87.86 ns |     2.163 ns |   0.119 ns |  0.86 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **171.10 ns** |     **1.859 ns** |   **0.102 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    54.95 ns |     1.080 ns |   0.059 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   170.88 ns |     0.551 ns |   0.030 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    92.30 ns |     0.803 ns |   0.044 ns |  0.54 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **14**   |   **244.75 ns** |    **74.560 ns** |   **4.087 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 14   |    58.27 ns |     1.062 ns |   0.058 ns |  0.24 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |   226.81 ns |   260.001 ns |  14.252 ns |  0.93 |    0.05 |         - |          NA |
| Kernel_Cjk | 14   |    97.21 ns |     1.389 ns |   0.076 ns |  0.40 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **267.34 ns** |     **0.415 ns** |   **0.023 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    61.12 ns |     1.063 ns |   0.058 ns |  0.23 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   267.31 ns |     3.058 ns |   0.168 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |   102.56 ns |     4.327 ns |   0.237 ns |  0.38 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **18**   |   **500.25 ns** |   **440.791 ns** |  **24.161 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 18   |    64.45 ns |     0.235 ns |   0.013 ns |  0.13 |    0.01 |         - |          NA |
| Dp_Cjk     | 18   |   501.67 ns |    94.842 ns |   5.199 ns |  1.00 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |   117.61 ns |     4.751 ns |   0.260 ns |  0.24 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **20**   |   **621.98 ns** |    **18.370 ns** |   **1.007 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |    67.97 ns |     6.061 ns |   0.332 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   622.25 ns |    32.141 ns |   1.762 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |   109.28 ns |     2.914 ns |   0.160 ns |  0.18 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **779.63 ns** |   **351.943 ns** |  **19.291 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 24   |    75.14 ns |     1.423 ns |   0.078 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   767.21 ns |     8.676 ns |   0.476 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |   120.22 ns |     2.413 ns |   0.132 ns |  0.15 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,232.56 ns** |   **478.089 ns** |  **26.206 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 32   |    91.10 ns |    23.498 ns |   1.288 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,279.01 ns |     8.759 ns |   0.480 ns |  1.04 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |   140.88 ns |     5.769 ns |   0.316 ns |  0.11 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **2,628.31 ns** | **3,800.326 ns** | **208.309 ns** |  **1.00** |    **0.10** |         **-** |          **NA** |
| Kernel     | 48   |   114.45 ns |     3.774 ns |   0.207 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 3,204.96 ns |   143.856 ns |   7.885 ns |  1.22 |    0.08 |         - |          NA |
| Kernel_Cjk | 48   |   202.22 ns |     4.755 ns |   0.261 ns |  0.08 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **4,048.14 ns** | **1,312.943 ns** |  **71.967 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 64   |   134.67 ns |    15.238 ns |   0.835 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,010.19 ns | 1,698.342 ns |  93.092 ns |  0.99 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |   240.60 ns |     5.160 ns |   0.283 ns |  0.06 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **8,592.82 ns** | **8,753.419 ns** | **479.804 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 96   |   282.60 ns |    26.387 ns |   1.446 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 8,943.00 ns | 1,925.779 ns | 105.558 ns |  1.04 |    0.05 |         - |          NA |
| Kernel_Cjk | 96   |   737.58 ns |     8.142 ns |   0.446 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **20.93 ns** |     **0.305 ns** |   **0.017 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     21.99 ns |     0.064 ns |   0.004 ns |  1.05 |         - |          NA |
| Distance_CodePoint         | 8      |     96.93 ns |     0.715 ns |   0.039 ns |  4.63 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     21.87 ns |     0.227 ns |   0.012 ns |  1.04 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **64**     |    **242.98 ns** |     **2.924 ns** |   **0.160 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    312.92 ns |     1.615 ns |   0.089 ns |  1.29 |         - |          NA |
| Distance_CodePoint         | 64     |    546.75 ns |    15.625 ns |   0.856 ns |  2.25 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    241.73 ns |     2.014 ns |   0.110 ns |  0.99 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **128**    |    **775.30 ns** |     **7.171 ns** |   **0.393 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,571.49 ns |    67.958 ns |   3.725 ns |  2.03 |         - |          NA |
| Distance_CodePoint         | 128    |  1,313.34 ns |    22.116 ns |   1.212 ns |  1.69 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    797.18 ns |   115.979 ns |   6.357 ns |  1.03 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **512**    |  **9,648.92 ns** |    **35.283 ns** |   **1.934 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 14,351.57 ns | 1,952.996 ns | 107.050 ns |  1.49 |         - |          NA |
| Distance_CodePoint         | 512    | 11,481.37 ns |   100.588 ns |   5.514 ns |  1.19 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  9,670.16 ns |   149.251 ns |   8.181 ns |  1.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **275.2 ns** |     **84.02 ns** |     **4.61 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     197.5 ns |      1.51 ns |     0.08 ns |  0.72 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **271.8 ns** |      **7.41 ns** |     **0.41 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     198.8 ns |      1.81 ns |     0.10 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **353.3 ns** |     **13.74 ns** |     **0.75 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     271.0 ns |      8.61 ns |     0.47 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **340.9 ns** |      **0.63 ns** |     **0.03 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     269.6 ns |      3.20 ns |     0.18 ns |  0.79 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **412.2 ns** |      **5.73 ns** |     **0.31 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     342.9 ns |      3.83 ns |     0.21 ns |  0.83 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **415.7 ns** |     **19.33 ns** |     **1.06 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     344.8 ns |      3.08 ns |     0.17 ns |  0.83 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **483.1 ns** |     **16.47 ns** |     **0.90 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,055.6 ns |    145.49 ns |     7.97 ns |  2.19 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **491.1 ns** |     **56.77 ns** |     **3.11 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,022.8 ns |     29.52 ns |     1.62 ns |  2.08 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,562.7 ns** |     **45.04 ns** |     **2.47 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   4,525.3 ns |    666.36 ns |    36.53 ns |  2.90 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,564.0 ns** |     **16.96 ns** |     **0.93 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,434.9 ns |    235.47 ns |    12.91 ns |  2.84 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **12,722.5 ns** |     **61.93 ns** |     **3.39 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  50,803.1 ns |    601.28 ns |    32.96 ns |  3.99 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **345,103.4 ns** | **24,501.15 ns** | **1,342.99 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  55,028.1 ns |  2,295.54 ns |   125.83 ns |  0.16 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **21.42 ns** |       **0.428 ns** |      **0.023 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      68.13 ns |      10.418 ns |      0.571 ns |  3.18 |    0.02 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      63.87 ns |      10.974 ns |      0.602 ns |  2.98 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     128.31 ns |      12.520 ns |      0.686 ns |  5.99 |    0.03 | 0.0076 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **244.34 ns** |       **7.176 ns** |      **0.393 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   4,521.36 ns |     249.002 ns |     13.649 ns | 18.50 |    0.05 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |     994.38 ns |      31.776 ns |      1.742 ns |  4.07 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   7,743.23 ns |     846.849 ns |     46.419 ns | 31.69 |    0.17 | 0.0305 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |   **9,632.56 ns** |     **125.559 ns** |      **6.882 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 371,691.78 ns |  16,687.978 ns |    914.724 ns | 38.59 |    0.09 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  26,918.57 ns |     157.586 ns |      8.638 ns |  2.79 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 578,779.99 ns | 200,141.223 ns | 10,970.416 ns | 60.09 |    0.99 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ManyLabelsMetricsBenchmarks-report-github

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

| Method            | Labels | Mean     | Error    | StdDev  | Gen0    | Gen1    | Allocated |
|------------------ |------- |---------:|---------:|--------:|--------:|--------:|----------:|
| **F1Macro**           | **1000**   | **443.0 μs** | **11.41 μs** | **0.63 μs** |  **3.9063** |       **-** |  **67.71 KB** |
| PrecisionWeighted | 1000   | 433.5 μs | 20.23 μs | 1.11 μs |  3.9063 |       - |  67.71 KB |
| ReportRows        | 1000   | 464.2 μs | 51.53 μs | 2.82 μs |  9.2773 |  2.9297 | 153.98 KB |
| **F1Macro**           | **5000**   | **532.4 μs** |  **2.12 μs** | **0.12 μs** | **20.5078** |  **5.8594** | **337.24 KB** |
| PrecisionWeighted | 5000   | 545.4 μs | 30.00 μs | 1.64 μs | 20.5078 |  5.8594 | 337.24 KB |
| ReportRows        | 5000   | 727.4 μs | 11.86 μs | 0.65 μs | 46.8750 | 18.5547 | 767.27 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

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

| Method                          | Shape   | Mean          | Error          | StdDev        | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|--------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **114.83 μs** |       **3.991 μs** |      **0.219 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  85,974.01 μs |  23,272.462 μs |  1,275.642 μs | 748.71 |    9.70 | 857.1429 | 857.1429 | 857.1429 |  31415.1 KB |   12,806.15 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,432.81 μs** |     **202.215 μs** |     **11.084 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |    **42.14 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 406,012.45 μs | 340,454.883 μs | 18,661.481 μs | 166.89 |    6.68 |        - |        - |        - | 32054.48 KB |      760.60 |
|                                 |         |               |                |               |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **19.16 μs** |       **0.654 μs** |      **0.036 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.45 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     723.78 μs |      18.951 μs |      1.039 μs |  37.78 |    0.08 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      134.40 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

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

| Method         | Samples | Classes | Mean            | Error          | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |----------------:|---------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,482.41 ns** |   **1,309.300 ns** |     **71.767 ns** | **0.0229** |     **400 B** |
| MatrixWeighted | 1000    | 2       |     5,686.31 ns |     775.751 ns |     42.522 ns | 0.0229 |     400 B |
| AccuracyScore  | 1000    | 2       |        91.20 ns |       7.572 ns |      0.415 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,463.99 ns |      33.681 ns |      1.846 ns | 0.0381 |     656 B |
| Report         | 1000    | 2       |     5,367.04 ns |     233.031 ns |     12.773 ns | 0.3204 |    5464 B |
| **Matrix**         | **1000**    | **10**      |     **3,176.27 ns** |     **109.578 ns** |      **6.006 ns** | **0.0763** |    **1336 B** |
| MatrixWeighted | 1000    | 10      |     5,822.71 ns |     115.128 ns |      6.311 ns | 0.0763 |    1336 B |
| AccuracyScore  | 1000    | 10      |        91.04 ns |      14.464 ns |      0.793 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,512.89 ns |      20.457 ns |      1.121 ns | 0.1106 |    1880 B |
| Report         | 1000    | 10      |     8,228.41 ns |   3,166.997 ns |    173.594 ns | 0.7324 |   12488 B |
| **Matrix**         | **100000**  | **2**       |   **314,140.08 ns** |   **6,441.503 ns** |    **353.080 ns** |      **-** |     **400 B** |
| MatrixWeighted | 100000  | 2       |   638,255.97 ns | 208,377.558 ns | 11,421.877 ns |      - |     401 B |
| AccuracyScore  | 100000  | 2       |     9,541.10 ns |     303.185 ns |     16.619 ns |      - |         - |
| F1Macro        | 100000  | 2       |   332,280.99 ns |   7,356.956 ns |    403.260 ns |      - |     656 B |
| Report         | 100000  | 2       |   317,497.53 ns |  23,997.619 ns |  1,315.390 ns |      - |    5488 B |
| **Matrix**         | **100000**  | **10**      |   **292,545.16 ns** |   **4,784.833 ns** |    **262.273 ns** |      **-** |    **1336 B** |
| MatrixWeighted | 100000  | 10      |   772,485.31 ns |  24,096.000 ns |  1,320.783 ns |      - |    1337 B |
| AccuracyScore  | 100000  | 10      |     9,499.48 ns |   2,165.631 ns |    118.706 ns |      - |         - |
| F1Macro        | 100000  | 10      |   312,898.55 ns |  11,589.411 ns |    635.255 ns |      - |    1880 B |
| Report         | 100000  | 10      |   316,013.17 ns |  24,519.076 ns |  1,343.973 ns | 0.4883 |   12832 B |
| **Matrix**         | **1000000** | **2**       | **3,326,858.48 ns** | **184,393.042 ns** | **10,107.205 ns** |      **-** |     **403 B** |
| MatrixWeighted | 1000000 | 2       | 7,126,143.37 ns | 129,592.054 ns |  7,103.378 ns |      - |     406 B |
| AccuracyScore  | 1000000 | 2       |    95,366.85 ns |   7,554.180 ns |    414.070 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,277,514.97 ns |  34,041.529 ns |  1,865.931 ns |      - |     659 B |
| Report         | 1000000 | 2       | 3,285,035.80 ns |  84,282.076 ns |  4,619.785 ns |      - |    5507 B |
| **Matrix**         | **1000000** | **10**      | **3,065,584.58 ns** | **192,331.756 ns** | **10,542.353 ns** |      **-** |    **1339 B** |
| MatrixWeighted | 1000000 | 10      | 7,843,586.81 ns | 351,688.586 ns | 19,277.238 ns |      - |    1348 B |
| AccuracyScore  | 1000000 | 10      |    95,765.80 ns |   6,089.844 ns |    333.805 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 2,903,008.27 ns |  53,980.968 ns |  2,958.879 ns |      - |    1883 B |
| Report         | 1000000 | 10      | 3,102,431.53 ns | 291,176.903 ns | 15,960.389 ns |      - |   12875 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |---------------:|---------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **7,099.218 μs** |  **1,629.7609 μs** |    **89.3327 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |     **1280 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  30,295.656 μs |  4,576.3882 μs |   250.8473 μs |     4.27 |    0.06 | 593.7500 | 593.7500 | 593.7500 |  5089918 B |    3,976.50 |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |       **9.505 μs** |      **0.6356 μs** |     **0.0348 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  30,053.502 μs |    999.3177 μs |    54.7760 μs | 3,161.99 |   11.23 | 593.7500 | 593.7500 | 593.7500 |  5090163 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **115,286.463 μs** | **17,514.6290 μs** |   **960.0359 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 220,240.624 μs | 45,755.7581 μs | 2,508.0275 μs |     1.91 |    0.02 |        - |        - |        - | 23228948 B |          NA |
|          |         |               |                |                |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |      **95.246 μs** |      **1.2438 μs** |     **0.0682 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 197,234.391 μs |  7,309.7534 μs |   400.6723 μs | 2,070.79 |    3.86 |        - |        - |        - | 23228008 B |          NA |

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

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **64.49 ns** |     **3.909 ns** |   **0.214 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |    62.08 ns |     0.661 ns |   0.036 ns |  0.96 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |    63.47 ns |     0.760 ns |   0.042 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |    62.96 ns |     1.445 ns |   0.079 ns |  0.98 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **6**    |    **94.45 ns** |    **10.265 ns** |   **0.563 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 6    |    67.63 ns |     3.678 ns |   0.202 ns |  0.72 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    94.06 ns |     4.271 ns |   0.234 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |   114.55 ns |     4.297 ns |   0.236 ns |  1.21 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **8**    |   **121.58 ns** |     **8.989 ns** |   **0.493 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    77.67 ns |     1.372 ns |   0.075 ns |  0.64 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   121.68 ns |     2.850 ns |   0.156 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |   146.64 ns |    10.048 ns |   0.551 ns |  1.21 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **10**   |   **156.64 ns** |     **5.872 ns** |   **0.322 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    83.72 ns |     5.677 ns |   0.311 ns |  0.53 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |   157.06 ns |     4.720 ns |   0.259 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |   132.95 ns |     1.159 ns |   0.064 ns |  0.85 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **208.13 ns** |     **4.834 ns** |   **0.265 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    91.92 ns |    12.771 ns |   0.700 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   208.87 ns |    11.271 ns |   0.618 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |   140.08 ns |    23.983 ns |   1.315 ns |  0.67 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **332.60 ns** |    **19.486 ns** |   **1.068 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |   109.08 ns |     5.240 ns |   0.287 ns |  0.33 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   329.50 ns |     7.529 ns |   0.413 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |   159.25 ns |     1.011 ns |   0.055 ns |  0.48 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **668.78 ns** |    **18.475 ns** |   **1.013 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |   139.09 ns |     7.318 ns |   0.401 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   667.98 ns |     9.821 ns |   0.538 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |   190.55 ns |     4.364 ns |   0.239 ns |  0.28 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,151.89 ns** |    **10.161 ns** |   **0.557 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |   170.75 ns |     7.544 ns |   0.414 ns |  0.15 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,236.93 ns |     4.236 ns |   0.232 ns |  1.07 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |   229.89 ns |    19.159 ns |   1.050 ns |  0.20 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **2,526.15 ns** |   **227.998 ns** |  **12.497 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |   233.74 ns |     3.262 ns |   0.179 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,522.29 ns |    29.171 ns |   1.599 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |   301.74 ns |     4.037 ns |   0.221 ns |  0.12 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **4,625.05 ns** | **4,961.092 ns** | **271.934 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 64   |   296.58 ns |     9.316 ns |   0.511 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,373.20 ns |   102.573 ns |   5.622 ns |  0.95 |    0.05 |         - |          NA |
| Kernel_Cjk | 64   |   375.38 ns |     4.681 ns |   0.257 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **9,722.08 ns** |   **359.638 ns** |  **19.713 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |   627.57 ns |     1.826 ns |   0.100 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 9,766.31 ns |   756.397 ns |  41.461 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   | 1,234.65 ns |    10.984 ns |   0.602 ns |  0.13 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **45.18 ns** |     **0.929 ns** |   **0.051 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    130.46 ns |     1.464 ns |   0.080 ns |  2.89 |    0.00 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **159.35 ns** |     **6.378 ns** |   **0.350 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,456.07 ns |    42.800 ns |   2.346 ns |  9.14 |    0.02 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **355.67 ns** |    **13.503 ns** |   **0.740 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  7,378.76 ns | 1,516.249 ns |  83.111 ns | 20.75 |    0.21 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **39,601.27 ns** | **4,054.706 ns** | **222.252 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 37,176.50 ns | 4,358.900 ns | 238.926 ns |  0.94 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|---------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **180.24 μs** |   **8.852 μs** | **0.485 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   178.21 μs |   9.136 μs | 0.501 μs |  0.99 | 0.2441 |    7784 B |        9.73 |
| SparseFit             | 10000    | 10         |    45.73 μs |   2.364 μs | 0.130 μs |  0.25 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   178.09 μs |   4.002 μs | 0.219 μs |  0.99 |      - |     800 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **180.16 μs** |  **11.057 μs** | **0.606 μs** |  **1.00** |      **-** |     **800 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   197.22 μs |  11.189 μs | 0.613 μs |  1.09 | 4.6387 |   77624 B |       97.03 |
| SparseFit             | 10000    | 100        |    45.22 μs |   2.278 μs | 0.125 μs |  0.25 |      - |     800 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   179.26 μs |   2.026 μs | 0.111 μs |  0.99 |      - |     800 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **1,766.85 μs** |  **12.278 μs** | **0.673 μs** |  **1.00** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 1,763.33 μs | 126.425 μs | 6.930 μs |  1.00 |      - |    7785 B |        9.72 |
| SparseFit             | 100000   | 10         |   921.47 μs |   7.011 μs | 0.384 μs |  0.52 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 10         | 1,758.26 μs |  31.398 μs | 1.721 μs |  1.00 |      - |     801 B |        1.00 |
|                       |          |            |             |            |          |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **1,766.79 μs** |  **24.457 μs** | **1.341 μs** |  **1.00** |      **-** |     **801 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 1,780.84 μs |  67.067 μs | 3.676 μs |  1.01 | 3.9063 |   77625 B |       96.91 |
| SparseFit             | 100000   | 100        |   919.04 μs |  30.435 μs | 1.668 μs |  0.52 |      - |     801 B |        1.00 |
| DenseFitOfTheSameData | 100000   | 100        | 1,758.39 μs |  35.857 μs | 1.965 μs |  1.00 |      - |     801 B |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **7.663 μs** |  **0.2532 μs** | **0.0139 μs** |         **-** |
| EqualLength | 65           |     2.356 μs |  0.0134 μs | 0.0007 μs |         - |
| **Embedded**    | **128**          |    **27.972 μs** |  **0.2674 μs** | **0.0147 μs** |         **-** |
| EqualLength | 128          |     4.258 μs |  0.0868 μs | 0.0048 μs |         - |
| **Embedded**    | **512**          | **2,297.040 μs** | **14.0105 μs** | **0.7680 μs** |         **-** |
| EqualLength | 512          |    36.938 μs |  1.0797 μs | 0.0592 μs |         - |

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  3.234 ms |  0.4548 ms | 0.0249 ms | 117.1875 | 113.2813 |  35.1563 |  3711.61 KB |
| TokenizerJsonWordPiece |  8.726 ms |  4.2410 ms | 0.2325 ms | 187.5000 | 171.8750 |  46.8750 |   5852.3 KB |
| TokenizerJsonUnigram   |  8.700 ms |  0.8478 ms | 0.0465 ms |  78.1250 |  62.5000 |  15.6250 |  4748.83 KB |
| SpieceModel            |  4.787 ms |  1.6752 ms | 0.0918 ms | 132.8125 | 125.0000 |  39.0625 |  4807.75 KB |
| TfidfSave              |  1.238 ms |  0.9173 ms | 0.0503 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  3.510 ms |  0.2950 ms | 0.0162 ms |  85.9375 |  78.1250 |  23.4375 |  2921.38 KB |
| EmbeddingIndexSave     |  3.109 ms |  1.3161 ms | 0.0721 ms | 199.2188 | 195.3125 | 195.3125 | 20349.83 KB |
| EmbeddingIndexLoad     |  3.641 ms |  0.5592 ms | 0.0307 ms | 199.2188 | 164.0625 | 132.8125 | 16094.48 KB |
| EmbeddingIndexSaveFile | 82.999 ms | 99.9467 ms | 5.4784 ms |        - |        - |        - |   323.53 KB |
| EmbeddingIndexLoadGzip | 57.659 ms |  2.6989 ms | 0.1479 ms |        - |        - |        - | 16095.22 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

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

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 13.50 ms | 0.434 ms | 0.024 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 58.44 ms | 1.662 ms | 0.091 ms | 444.4444 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **7,767.99 μs** | **464.466 μs** | **25.459 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.35 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 12,699.93 μs | 246.640 μs | 13.519 μs |  1.63 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **114.86 μs** |   **2.401 μs** |  **0.132 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    155.48 μs |   7.279 μs |  0.399 μs |  1.35 |        - |        - |        - |   1.94 KB |        0.79 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,391.18 μs** | **137.589 μs** |  **7.542 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.14 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,219.64 μs |  70.843 μs |  3.883 μs |  0.93 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **19.07 μs** |   **0.711 μs** |  **0.039 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.45 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     21.11 μs |   0.658 μs |  0.036 μs |  1.11 |   0.0916 |        - |        - |   1.94 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **6.874 ms** | **0.1145 ms** | **0.0063 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 6.718 ms | 0.0985 ms | 0.0054 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **6.903 ms** | **0.3371 ms** | **0.0185 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 6.782 ms | 0.6415 ms | 0.0352 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

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

| Method            | Q | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **4.775 μs** | **0.9961 μs** | **0.0546 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **5.847 μs** | **0.8667 μs** | **0.0475 μs** | **0.0076** |     **192 B** |

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

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Length | Mean         | Error           | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|----------------:|-------------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **412.3 ns** |        **33.57 ns** |      **1.84 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,373.5 ns |       191.19 ns |     10.48 ns | 13.03 |    0.05 |         - |          NA |
|                          |        |              |                 |              |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **20,461.6 ns** |       **232.76 ns** |     **12.76 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 664,314.2 ns | 1,229,417.73 ns | 67,388.53 ns | 32.47 |    2.85 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **37.23 μs** |   **2.552 μs** |  **0.140 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    36.45 μs |   0.887 μs |  0.049 μs |        - |        - |        - |         - |
| R2Score  | 100000  |    95.28 μs |   2.178 μs |  0.119 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   405.23 μs | 308.218 μs | 16.894 μs | 199.7070 | 199.7070 | 199.7070 |  800186 B |
| **Mse**      | **1000000** |   **371.10 μs** |   **5.435 μs** |  **0.298 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   365.74 μs |  23.283 μs |  1.276 μs |        - |        - |        - |         - |
| R2Score  | 1000000 |   950.11 μs |   5.341 μs |  0.293 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 4,756.79 μs | 378.581 μs | 20.751 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

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

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **23.82 ms** | **0.248 ms** | **0.014 ms** | **93.7500** | **93.7500** | **93.7500** |  **375.3 KB** |
| **Fit**    | **500x20000x5**  | **43.90 ms** | **3.578 ms** | **0.196 ms** |       **-** |       **-** |       **-** | **492.47 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  Job-TAXBOT : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_MinMax**                   | **Job-TAXBOT** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **128.38 μs** |    **13.193 μs** |    **14.664 μs** |  **1.01** |    **0.16** |        **-** |        **-** |        **-** |   **79.59 KB** |        **1.00** |
| Lodestar_MaxAbs                   | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    112.72 μs |    23.564 μs |    26.192 μs |  0.89 |    0.23 |        - |        - |        - |   79.14 KB |        0.99 |
| Lodestar_Robust                   | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,049.65 μs |    26.908 μs |    29.908 μs | 16.17 |    1.88 |        - |        - |        - |  157.51 KB |        1.98 |
| Lodestar_Standard                 | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    161.32 μs |    10.418 μs |    11.580 μs |  1.27 |    0.17 |        - |        - |        - |   79.66 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |    427.12 μs |     7.941 μs |     8.826 μs |  3.37 |    0.39 |        - |        - |        - |    9.02 KB |        0.11 |
| MlNet_NormalizeMinMax_Read        | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,092.61 μs |    72.295 μs |    80.356 μs | 16.51 |    2.00 |        - |        - |        - |  308.01 KB |        3.87 |
| MlNet_NormalizeRobustScaling_Read | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 1000     |  5,614.56 μs |    88.421 μs |    94.610 μs | 44.29 |    5.15 |        - |        - |        - |  451.11 KB |        5.67 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     48.20 μs |     3.699 μs |     0.203 μs |  1.00 |    0.01 |   4.7607 |        - |        - |   78.87 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.56 μs |     1.511 μs |     0.083 μs |  0.97 |    0.00 |   4.7607 |        - |        - |    78.4 KB |        0.99 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    372.17 μs |   333.161 μs |    18.262 μs |  7.72 |    0.33 |   9.2773 |        - |        - |  156.79 KB |        1.99 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     66.31 μs |     3.108 μs |     0.170 μs |  1.38 |    0.01 |   4.7607 |        - |        - |   78.71 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,459.35 μs | 4,245.942 μs |   232.734 μs | 30.28 |    4.18 |  83.4961 |   7.3242 |        - |  1366.2 KB |       17.32 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    965.42 μs |   804.577 μs |    44.102 μs | 20.03 |    0.80 |  39.0625 |   1.9531 |        - |  634.77 KB |        8.05 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,788.83 μs | 5,527.914 μs |   303.004 μs | 37.11 |    5.45 |  31.2500 |        - |        - |   541.1 KB |        6.86 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_MinMax**                   | **Job-TAXBOT** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,523.70 μs** |    **25.613 μs** |    **29.496 μs** |  **1.00** |    **0.03** |        **-** |        **-** |        **-** | **1563.68 KB** |       **1.000** |
| Lodestar_MaxAbs                   | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,441.91 μs |    20.269 μs |    21.688 μs |  0.95 |    0.02 |        - |        - |        - | 1563.23 KB |       1.000 |
| Lodestar_Robust                   | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,425.57 μs |    81.570 μs |    83.766 μs | 11.44 |    0.22 |        - |        - |        - | 3126.26 KB |       1.999 |
| Lodestar_Standard                 | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,990.46 μs |    25.630 μs |    29.516 μs |  1.31 |    0.03 |        - |        - |        - | 1564.03 KB |       1.000 |
| MlNet_NormalizeMinMax_Fit         | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,080.17 μs | 1,380.771 μs | 1,534.723 μs |  3.99 |    0.99 |        - |        - |        - |    9.02 KB |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    | 20,150.24 μs | 5,415.347 μs | 6,019.144 μs | 13.23 |    3.86 |        - |        - |        - |  499.44 KB |       0.319 |
| MlNet_NormalizeRobustScaling_Read | Job-TAXBOT | 1               | 20             | Throughput  | 1            | 5           | 20000    | 21,517.42 μs |   161.431 μs |   158.547 μs | 14.13 |    0.29 |        - |        - |        - | 4175.95 KB |       2.671 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,309.61 μs |   119.821 μs |     6.568 μs |  1.00 |    0.01 | 332.0313 | 332.0313 | 332.0313 |  1564.3 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,160.61 μs |   551.636 μs |    30.237 μs |  0.89 |    0.02 | 332.0313 | 332.0313 | 332.0313 | 1562.99 KB |        1.00 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 16,390.01 μs |   213.838 μs |    11.721 μs | 12.52 |    0.05 | 875.0000 | 875.0000 | 875.0000 | 3126.12 KB |        2.00 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,562.44 μs | 2,762.844 μs |   151.441 μs |  1.19 |    0.10 | 332.0313 | 332.0313 | 332.0313 |  1563.3 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,269.15 μs |   891.568 μs |    48.870 μs |  0.97 |    0.03 |  19.5313 |   3.9063 |        - |  345.85 KB |        0.22 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,727.14 μs | 1,356.105 μs |    74.333 μs |  3.61 |    0.05 |  31.2500 |        - |        - |  533.76 KB |        0.34 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 21,261.43 μs |   763.126 μs |    41.830 μs | 16.24 |    0.08 | 250.0000 | 125.0000 |        - | 4200.05 KB |        2.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

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

| Method          | Model   | Mean     | Error    | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **138.6 ms** | **15.44 ms** | **0.85 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **133.2 ms** |  **0.78 ms** | **0.04 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

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

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **27.19 ms** | **0.298 ms** | **0.016 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **182.69 ms** | **0.769 ms** | **0.042 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

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

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **43.612 ms** |  **1.8331 ms** | **0.1005 ms** |  **1.00** |  **416.6667** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   9.835 ms |  1.1514 ms | 0.0631 ms |  0.23 |  171.8750 |  156.2500 |  78.1250 |   2533.15 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   7.950 ms |  2.1644 ms | 0.1186 ms |  0.18 |   15.6250 |         - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   5.266 ms |  0.3486 ms | 0.0191 ms |  0.12 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   9.420 ms |  0.6528 ms | 0.0358 ms |  0.22 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **43.871 ms** |  **1.0182 ms** | **0.0558 ms** |  **1.00** |  **416.6667** |         **-** |        **-** |    **8152.4 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  11.460 ms |  0.9379 ms | 0.0514 ms |  0.26 |  328.1250 |  312.5000 | 218.7500 |   4578.87 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   9.590 ms |  0.8639 ms | 0.0474 ms |  0.22 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   6.577 ms |  0.2345 ms | 0.0129 ms |  0.15 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   8.173 ms |  1.0358 ms | 0.0568 ms |  0.19 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **762.166 ms** | **12.7411 ms** | **0.6984 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  36.775 ms |  4.2857 ms | 0.2349 ms |  0.05 |  928.5714 |  928.5714 | 500.0000 |   10335.6 KB |       0.082 |
| SignaturesOnly       | 2000      | 64           |  26.970 ms |  1.3000 ms | 0.0713 ms |  0.04 |   62.5000 |         - |        - |    1218.8 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  21.351 ms |  1.3806 ms | 0.0757 ms |  0.03 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  32.447 ms |  1.9312 ms | 0.1059 ms |  0.04 |   62.5000 |         - |        - |   1718.79 KB |       0.014 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **761.879 ms** | **25.8625 ms** | **1.4176 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126359.81 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  54.512 ms | 26.8051 ms | 1.4693 ms |  0.07 | 1600.0000 | 1600.0000 | 900.0000 |  18665.36 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  37.368 ms |  1.1088 ms | 0.0608 ms |  0.05 |   71.4286 |         - |        - |   1718.84 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  26.514 ms |  0.2548 ms | 0.0140 ms |  0.03 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  32.613 ms |  0.3385 ms | 0.0186 ms |  0.04 |   62.5000 |         - |        - |   1718.79 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SmallArtifactBenchmarks-report-github

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

| Method                 | Mean           | Error           | StdDev        | Gen0     | Gen1     | Gen2     | Allocated  |
|----------------------- |---------------:|----------------:|--------------:|---------:|---------:|---------:|-----------:|
| CountSaveAsync         |       759.5 ns |       227.68 ns |      12.48 ns |   0.3262 |   0.0057 |        - |     5464 B |
| TfidfSaveAsync         |       943.3 ns |       228.30 ns |      12.51 ns |   0.3338 |        - |        - |     5592 B |
| HashingSaveAsync       |       785.2 ns |       478.01 ns |      26.20 ns |   0.3281 |   0.0057 |        - |     5496 B |
| IndexSaveAsync         |       304.9 ns |        47.40 ns |       2.60 ns |   0.0386 |        - |        - |      648 B |
| CountLoadSeekable1400K | 8,741,087.8 ns | 2,955,052.48 ns | 161,976.40 ns | 875.0000 | 843.7500 | 609.3750 | 12227072 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | SampleCount | Mean          | Error      | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-----------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **90.104 μs** |  **65.791 μs** |  **3.6062 μs** |  **1.00** |    **0.05** |  **16.6016** |   **6.5918** |        **-** |  **273.94 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    211.449 μs |  17.969 μs |  0.9849 μs |  2.35 |    0.08 |  19.0430 |   4.6387 |        - |  313.53 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.774 μs |   1.824 μs |  0.1000 μs |  0.05 |    0.00 |   2.3880 |   0.2594 |        - |   39.14 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    259.934 μs |  10.268 μs |  0.5628 μs |  2.89 |    0.10 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,730.908 μs | 420.183 μs | 23.0316 μs | 63.67 |    2.21 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     44.720 μs |  29.229 μs |  1.6022 μs |  0.50 |    0.02 |   0.4883 |        - |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,056.489 μs | 142.644 μs |  7.8188 μs | 11.74 |    0.41 |   1.9531 |        - |        - |   34.26 KB |        0.13 |
|                                 |             |               |            |            |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,194.910 μs** | **550.223 μs** | **30.1596 μs** |  **1.00** |    **0.03** | **437.5000** | **421.8750** | **414.0625** | **2738.22 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,307.911 μs | 566.126 μs | 31.0313 μs |  1.93 |    0.05 | 593.7500 | 578.1250 | 570.3125 | 3130.42 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     84.075 μs |  25.440 μs |  1.3944 μs |  0.07 |    0.00 |  49.9268 |  49.9268 |  49.9268 |  391.11 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    254.163 μs |  12.112 μs |  0.6639 μs |  0.21 |    0.00 |   2.4414 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 41,753.490 μs | 517.125 μs | 28.3454 μs | 34.96 |    0.78 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     48.618 μs |  20.422 μs |  1.1194 μs |  0.04 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,658.520 μs |  99.266 μs |  5.4411 μs |  6.41 |    0.14 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

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

| Method               | Documents | Mean     | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |---------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       | **1.628 ms** | **0.0680 ms** | **0.0037 ms** |  **1.00** | **115.2344** | **115.2344** | **115.2344** |  **869.91 KB** |        **1.00** |
| CountWithStopWords   | 200       | 1.199 ms | 0.0849 ms | 0.0047 ms |  0.74 |  23.4375 |   7.8125 |        - |   407.6 KB |        0.47 |
| Hashing              | 200       | 1.714 ms | 0.0624 ms | 0.0034 ms |  1.05 |  72.2656 |  72.2656 |  72.2656 |  554.26 KB |        0.64 |
| HashingWithStopWords | 200       | 1.415 ms | 0.0398 ms | 0.0022 ms |  0.87 |  15.6250 |   3.9063 |        - |  273.65 KB |        0.31 |
|                      |           |          |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **7.503 ms** | **0.1821 ms** | **0.0100 ms** |  **1.00** | **867.1875** | **867.1875** | **867.1875** | **3436.57 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 6.091 ms | 0.2373 ms | 0.0130 ms |  0.81 | 382.8125 | 382.8125 | 382.8125 | 1660.72 KB |        0.48 |
| Hashing              | 1000      | 8.311 ms | 0.4350 ms | 0.0238 ms |  1.11 | 515.6250 | 500.0000 | 500.0000 | 2349.58 KB |        0.68 |
| HashingWithStopWords | 1000      | 7.340 ms | 0.2882 ms | 0.0158 ms |  0.98 | 265.6250 | 265.6250 | 265.6250 | 1139.45 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

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

| Method  | Words | Mean      | Error     | StdDev   | Gen0      | Gen1      | Allocated |
|-------- |------ |----------:|----------:|---------:|----------:|----------:|----------:|
| **Extract** | **2000**  |  **14.20 ms** |  **1.600 ms** | **0.088 ms** |  **187.5000** |  **125.0000** |   **3.13 MB** |
| **Extract** | **8000**  | **324.28 ms** | **22.851 ms** | **1.253 ms** | **2000.0000** | **1000.0000** |  **35.34 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenPatternBenchmarks-report-github

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

| Method                     | Pattern       | Mean            | Error           | StdDev        | Gen0     | Gen1    | Gen2    | Allocated  |
|--------------------------- |-------------- |----------------:|----------------:|--------------:|---------:|--------:|--------:|-----------:|
| **Fit**                        | **[^ ]+**         | **28,265,728.1 ns** |   **278,648.16 ns** |  **15,273.65 ns** |  **62.5000** | **31.2500** |       **-** | **1554.58 KB** |
| Construct                  | [^ ]+         |      1,559.0 ns |       416.50 ns |      22.83 ns |   0.3319 |  0.0019 |       - |    5.43 KB |
| ConstructAndFitOneDocument | [^ ]+         |      2,551.3 ns |       947.15 ns |      51.92 ns |   0.4539 |  0.0038 |       - |    7.43 KB |
| **Fit**                        | **[a-z]+**        | **23,902,419.5 ns** |   **697,323.34 ns** |  **38,222.65 ns** |        **-** |       **-** |       **-** |     **7.2 KB** |
| Construct                  | [a-z]+        |      1,519.9 ns |       333.75 ns |      18.29 ns |   0.3300 |  0.0019 |       - |     5.4 KB |
| ConstructAndFitOneDocument | [a-z]+        |      2,305.7 ns |       431.40 ns |      23.65 ns |   0.4387 |  0.0038 |       - |    7.18 KB |
| **Fit**                        | **[A-Za-z]\\w+**  | **32,074,208.5 ns** |   **853,072.01 ns** |  **46,759.76 ns** |  **62.5000** |       **-** |       **-** | **1555.98 KB** |
| Construct                  | [A-Za-z]\\w+  |      2,676.3 ns |       602.06 ns |      33.00 ns |   0.4158 |  0.0038 |       - |    6.81 KB |
| ConstructAndFitOneDocument | [A-Za-z]\\w+  |      3,813.6 ns |       852.28 ns |      46.72 ns |   0.5188 |       - |       - |    8.81 KB |
| **Fit**                        | **\\b\\w\\w+\\b** | **16,934,307.9 ns** | **3,221,641.98 ns** | **176,589.07 ns** | **125.0000** | **93.7500** | **62.5000** | **9743.62 KB** |
| Construct                  | \\b\\w\\w+\\b |        840.3 ns |       233.72 ns |      12.81 ns |   0.1135 |       - |       - |    1.87 KB |
| ConstructAndFitOneDocument | \\b\\w\\w+\\b |      1,608.0 ns |       573.65 ns |      31.44 ns |   0.2346 |       - |       - |    3.84 KB |
| **Fit**                        | **\\S+**          | **29,225,646.7 ns** |   **567,509.22 ns** |  **31,107.10 ns** |  **62.5000** | **31.2500** |       **-** | **1554.45 KB** |
| Construct                  | \\S+          |      1,615.8 ns |        23.51 ns |       1.29 ns |   0.3223 |  0.0019 |       - |    5.29 KB |
| ConstructAndFitOneDocument | \\S+          |      2,654.8 ns |       453.73 ns |      24.87 ns |   0.4425 |  0.0038 |       - |    7.29 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **16.57 ms** |  **2.944 ms** | **0.161 ms** |  **1.00** |    **0.01** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  42.10 ms |  5.084 ms | 0.279 ms |  2.54 |    0.03 |  166.6667 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **38.91 ms** |  **1.193 ms** | **0.065 ms** |  **1.00** |    **0.00** |  **307.6923** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  44.95 ms |  1.444 ms | 0.079 ms |  1.16 |    0.00 |  166.6667 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **69.13 ms** |  **5.951 ms** | **0.326 ms** |  **1.00** |    **0.01** | **1714.2857** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 208.97 ms | 15.723 ms | 0.862 ms |  3.02 |    0.02 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

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

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **12.24 ms** | **0.344 ms** | **0.019 ms** |         **-** |
| **TopTwo** | **100**     | **83.98 ms** | **6.346 ms** | **0.348 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TransformerIncumbentBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  Job-ZADUWR : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                      | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio    | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|---------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Normalize**          | **Job-ZADUWR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **56.70 μs** |     **9.063 μs** |    **10.436 μs** |     **62.10 μs** |     **1.03** |    **0.27** |         **-** |         **-** |         **-** |    **31.99 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |     87.16 μs |    12.355 μs |    13.220 μs |     93.15 μs |     1.59 |    0.37 |         - |         - |         - |   118.91 KB |        3.72 |
| Lodestar_Discretize         | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    136.84 μs |    19.918 μs |    22.937 μs |    133.75 μs |     2.49 |    0.61 |         - |         - |         - |   156.71 KB |        4.90 |
| Lodestar_Discretize_Fit     | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    852.42 μs |    19.830 μs |    22.041 μs |    843.21 μs |    15.53 |    2.83 |         - |         - |         - |    33.48 KB |        1.05 |
| Lodestar_Quantile           | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,501.54 μs |    21.909 μs |    21.518 μs |  1,499.55 μs |    27.36 |    4.95 |         - |         - |         - |    31.99 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    915.06 μs |    18.590 μs |    17.389 μs |    918.35 μs |    16.67 |    3.02 |         - |         - |         - |   110.63 KB |        3.46 |
| Lodestar_Power              | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |    121.87 μs |     3.534 μs |     3.629 μs |    122.32 μs |     2.22 |    0.41 |         - |         - |         - |    31.99 KB |        1.00 |
| Lodestar_Power_Fit          | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  3,285.29 μs |    10.577 μs |    11.317 μs |  3,281.19 μs |    59.87 |   10.80 |         - |         - |         - |   628.16 KB |       19.63 |
| Lodestar_KnnImpute          | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     | 62,006.25 μs |    86.559 μs |    88.889 μs | 62,005.66 μs | 1,129.89 |  203.74 | 1000.0000 |         - |         - | 25631.73 KB |      801.19 |
| MlNet_NormalizeLpNorm_Read  | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,130.58 μs |    72.129 μs |    74.072 μs |  1,155.94 μs |    20.60 |    3.95 |         - |         - |         - |   315.72 KB |        9.87 |
| MlNet_NormalizeBinning_Read | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,866.54 μs |   188.276 μs |   209.269 μs |  2,900.11 μs |    52.23 |   10.15 |         - |         - |         - |   375.48 KB |       11.74 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |          |         |           |           |           |             |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     14.22 μs |     0.393 μs |     0.022 μs |     14.22 μs |     1.00 |    0.00 |    1.9073 |         - |         - |    31.27 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     85.55 μs |     7.591 μs |     0.416 μs |     85.73 μs |     6.02 |    0.03 |   36.9873 |   36.9873 |   36.9873 |    118.2 KB |        3.78 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    106.25 μs |     6.860 μs |     0.376 μs |    106.39 μs |     7.47 |    0.02 |   49.9268 |   49.9268 |   49.9268 |   156.31 KB |        5.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     54.64 μs |    12.062 μs |     0.661 μs |     54.58 μs |     3.84 |    0.04 |    1.9531 |         - |         - |    32.77 KB |        1.05 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    420.51 μs |   270.183 μs |    14.810 μs |    411.99 μs |    29.57 |    0.90 |    1.4648 |         - |         - |    31.27 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    174.62 μs |     4.022 μs |     0.220 μs |    174.69 μs |    12.28 |    0.02 |    6.5918 |    0.9766 |         - |   109.89 KB |        3.51 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     83.35 μs |     3.618 μs |     0.198 μs |     83.25 μs |     5.86 |    0.01 |    1.8311 |         - |         - |    31.27 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  2,224.72 μs |     2.257 μs |     0.124 μs |  2,224.76 μs |   156.46 |    0.21 |   35.1563 |         - |         - |   627.42 KB |       20.06 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     | 62,472.17 μs | 3,507.812 μs |   192.275 μs | 62,383.65 μs | 4,393.53 |   13.05 | 1500.0000 |  250.0000 |         - | 25631.11 KB |      819.58 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    260.02 μs |    12.502 μs |     0.685 μs |    260.28 μs |    18.29 |    0.05 |   18.5547 |   18.0664 |         - |    294.5 KB |        9.42 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    937.21 μs | 1,181.383 μs |    64.756 μs |    933.85 μs |    65.91 |    3.94 |   42.9688 |    5.8594 |         - |    702.4 KB |       22.46 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |          |         |           |           |           |             |             |
| **Lodestar_Normalize**          | **Job-ZADUWR** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |    **497.83 μs** |     **7.082 μs** |     **8.155 μs** |    **498.86 μs** |     **1.00** |    **0.02** |         **-** |         **-** |         **-** |   **625.74 KB** |        **1.00** |
| Lodestar_Polynomial         | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,383.94 μs |    20.163 μs |    22.412 μs |  1,373.56 μs |     2.78 |    0.06 |         - |         - |         - |  2345.48 KB |        3.75 |
| Lodestar_Discretize         | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,364.59 μs |    24.265 μs |    26.970 μs |  2,356.79 μs |     4.75 |    0.09 |         - |         - |         - |  3125.74 KB |        5.00 |
| Lodestar_Discretize_Fit     | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,613.29 μs | 4,423.171 μs | 5,093.731 μs |  7,047.38 μs |    21.32 |    9.99 |         - |         - |         - |   627.23 KB |        1.00 |
| Lodestar_Quantile           | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 12,372.04 μs |    55.382 μs |    63.778 μs | 12,355.98 μs |    24.86 |    0.42 |         - |         - |         - |   625.74 KB |        1.00 |
| Lodestar_Quantile_Fit       | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 10,540.15 μs | 4,897.251 μs | 5,639.681 μs |  6,986.81 μs |    21.18 |   11.06 |         - |         - |         - |   704.38 KB |        1.13 |
| Lodestar_Power              | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,898.05 μs |    22.979 μs |    26.462 μs |  1,903.15 μs |     3.81 |    0.08 |         - |         - |         - |   625.74 KB |        1.00 |
| Lodestar_Power_Fit          | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 54,969.71 μs |   341.188 μs |   379.230 μs | 54,916.59 μs |   110.45 |    1.91 | 4000.0000 | 4000.0000 | 4000.0000 | 13287.16 KB |       21.23 |
| Lodestar_KnnImpute          | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 65,752.10 μs |   138.251 μs |   141.974 μs | 65,737.67 μs |   132.11 |    2.13 | 1000.0000 |         - |         - | 27172.61 KB |       43.42 |
| MlNet_NormalizeLpNorm_Read  | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,809.10 μs | 4,075.114 μs | 4,360.326 μs | 11,950.93 μs |    19.71 |    8.53 |         - |         - |         - |    463.1 KB |        0.74 |
| MlNet_NormalizeBinning_Read | Job-ZADUWR | 1               | 20             | Throughput  | 1            | 5           | 20000    | 21,645.53 μs | 5,336.384 μs | 6,145.388 μs | 21,841.46 μs |    43.49 |   12.07 |         - |         - |         - |  2073.77 KB |        3.31 |
|                             |            |                 |                |             |              |             |          |              |              |              |              |          |         |           |           |           |             |             |
| Lodestar_Normalize          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    683.00 μs |    23.889 μs |     1.309 μs |    682.57 μs |     1.00 |    0.00 |  199.2188 |  199.2188 |  199.2188 |   625.16 KB |        1.00 |
| Lodestar_Polynomial         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    829.27 μs | 1,243.371 μs |    68.153 μs |    804.48 μs |     1.21 |    0.09 |  333.0078 |  333.0078 |  333.0078 |  2344.95 KB |        3.75 |
| Lodestar_Discretize         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,406.46 μs |   234.210 μs |    12.838 μs |  3,401.86 μs |     4.99 |    0.02 |  996.0938 |  996.0938 |  996.0938 |  3125.68 KB |        5.00 |
| Lodestar_Discretize_Fit     | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  6,578.68 μs | 1,452.604 μs |    79.622 μs |  6,618.01 μs |     9.63 |    0.10 |  195.3125 |  195.3125 |  195.3125 |   626.65 KB |        1.00 |
| Lodestar_Quantile           | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 11,320.66 μs |   333.788 μs |    18.296 μs | 11,327.73 μs |    16.57 |    0.04 |  187.5000 |  187.5000 |  187.5000 |   625.16 KB |        1.00 |
| Lodestar_Quantile_Fit       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  6,726.43 μs |    69.974 μs |     3.836 μs |  6,727.68 μs |     9.85 |    0.02 |  195.3125 |  195.3125 |  195.3125 |   703.77 KB |        1.13 |
| Lodestar_Power              | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,038.90 μs |    58.612 μs |     3.213 μs |  2,038.44 μs |     2.99 |    0.01 |  199.2188 |  199.2188 |  199.2188 |   625.16 KB |        1.00 |
| Lodestar_Power_Fit          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 53,689.25 μs |   317.161 μs |    17.385 μs | 53,697.59 μs |    78.61 |    0.13 | 4200.0000 | 4200.0000 | 4200.0000 | 13286.62 KB |       21.25 |
| Lodestar_KnnImpute          | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 65,536.51 μs | 3,401.145 μs |   186.428 μs | 65,633.75 μs |    95.95 |    0.28 | 1625.0000 |  250.0000 |         - | 27171.98 KB |       43.46 |
| MlNet_NormalizeLpNorm_Read  | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,481.71 μs |   295.249 μs |    16.184 μs |  2,490.16 μs |     3.63 |    0.02 |   27.3438 |   11.7188 |         - |   455.88 KB |        0.73 |
| MlNet_NormalizeBinning_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 11,475.23 μs |   758.637 μs |    41.583 μs | 11,463.82 μs |    16.80 |    0.06 |  265.6250 |  218.7500 |  187.5000 |  2122.36 KB |        3.39 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

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

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| **Dot**    | **384**  |  **38.77 ns** |  **0.550 ns** | **0.030 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| L2Norm | 384  |  37.79 ns |  0.322 ns | 0.018 ns |  0.97 |    0.00 |         - |          NA |
|        |      |           |           |          |       |         |           |             |
| **Dot**    | **768**  |  **77.56 ns** | **20.759 ns** | **1.138 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| L2Norm | 768  |  71.27 ns |  2.186 ns | 0.120 ns |  0.92 |    0.01 |         - |          NA |
|        |      |           |           |          |       |         |           |             |
| **Dot**    | **1024** | **103.68 ns** | **14.298 ns** | **0.784 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| L2Norm | 1024 |  97.17 ns |  2.699 ns | 0.148 ns |  0.94 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

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

| Method                | Documents | Mean        | Error       | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |------------:|------------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |    **150.7 μs** |     **5.62 μs** |  **0.31 μs** |  **1.00** |    **0.00** |   **12.4512** |    **2.1973** |         **-** |    **205.5 KB** |        **1.00** |
| Tfidf                 | 200       |    167.2 μs |    19.42 μs |  1.06 μs |  1.11 |    0.01 |   14.1602 |    3.4180 |         - |   234.64 KB |        1.14 |
| CountBigrams          | 200       |    536.0 μs |    62.14 μs |  3.41 μs |  3.56 |    0.02 |   35.1563 |   12.6953 |         - |   586.99 KB |        2.86 |
| CountCharWordBoundary | 200       |  1,698.5 μs |   205.66 μs | 11.27 μs | 11.27 |    0.07 |  382.8125 |  382.8125 |  382.8125 |  1676.53 KB |        8.16 |
| Hashing               | 200       |    148.7 μs |    22.99 μs |  1.26 μs |  0.99 |    0.01 |    8.3008 |    0.9766 |         - |   136.13 KB |        0.66 |
|                       |           |             |             |          |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **1,262.8 μs** |   **446.35 μs** | **24.47 μs** |  **1.00** |    **0.02** |  **109.3750** |  **109.3750** |  **109.3750** |   **835.34 KB** |        **1.00** |
| Tfidf                 | 1000      |  1,393.0 μs |   207.65 μs | 11.38 μs |  1.10 |    0.02 |  142.5781 |  142.5781 |  142.5781 |   979.75 KB |        1.17 |
| CountBigrams          | 1000      |  3,071.2 μs |    18.40 μs |  1.01 μs |  2.43 |    0.04 |  398.4375 |  398.4375 |  398.4375 |   2319.8 KB |        2.78 |
| CountCharWordBoundary | 1000      | 10,793.5 μs | 1,415.41 μs | 77.58 μs |  8.55 |    0.15 | 1046.8750 | 1015.6250 | 1015.6250 | 12013.04 KB |       14.38 |
| Hashing               | 1000      |  1,326.3 μs |    50.50 μs |  2.77 μs |  1.05 |    0.02 |   70.3125 |   70.3125 |   70.3125 |   570.77 KB |        0.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

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

| Method   | Documents | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **2.933 ms** |  **0.1080 ms** | **0.0059 ms** |  **1.00** |    **0.00** |   **152.3438** |   **152.3438** |   **152.3438** |   **1.78 MB** |        **1.00** |
| MlNet    | 200       |  43.830 ms | 42.1410 ms | 2.3099 ms | 14.95 |    0.68 |  7400.0000 |  7400.0000 |  7400.0000 |  28.27 MB |       15.85 |
|          |           |            |            |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **14.730 ms** |  **3.1452 ms** | **0.1724 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **8.11 MB** |        **1.00** |
| MlNet    | 1000      | 337.226 ms | 89.9127 ms | 4.9284 ms | 22.90 |    0.37 | 78000.0000 | 78000.0000 | 78000.0000 | 324.48 MB |       40.02 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.117 | 0.025 | 0.22x | 0.117 | 0.025 | 0.22x |
| cdist_wratio_n50 | 1.510 | 1.848 | 1.22x | 1.510 | 1.848 | 1.22x |
| cdist_ratio_varied_n50 | 0.101 | 0.021 | 0.21x | 0.101 | 0.021 | 0.21x |
| cdist_ratio_varied_cutoff90_n50 | 0.041 | 0.021 | 0.52x | 0.041 | 0.021 | 0.52x |
| cdist_ratio_n200 | 2.089 | 0.305 | 0.15x | 2.092 | 0.305 | 0.15x |
| cdist_wratio_n200 | 23.168 | 28.263 | 1.22x | 23.172 | 28.259 | 1.22x |
| cdist_ratio_varied_n200 | 1.807 | 0.247 | 0.14x | 1.809 | 0.247 | 0.14x |
| cdist_ratio_varied_cutoff90_n200 | 0.762 | 0.247 | 0.32x | 0.765 | 0.247 | 0.32x |
| cdist_ratio_n500 | 13.219 | 1.840 | 0.14x | 13.232 | 1.840 | 0.14x |
| cdist_wratio_n500 | 139.213 | 170.791 | 1.23x | 139.326 | 170.784 | 1.23x |
| cdist_ratio_varied_n500 | 10.795 | 1.460 | 0.14x | 10.856 | 1.460 | 0.13x |
| cdist_ratio_varied_cutoff90_n500 | 4.275 | 1.460 | 0.34x | 4.338 | 1.460 | 0.34x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cluster-weighted

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kmeans_weighted_10000 | 3.914 | 3.032 | 0.77x | 3.920 | 6.059 | 1.55x |
| kmeans_restarts_10000 | 15.707 | 7.991 | 0.51x | 15.712 | 15.958 | 1.02x |
| kmeans_weighted_100000 | 37.728 | 22.814 | 0.60x | 37.942 | 40.168 | 1.06x |
| kmeans_restarts_100000 | 153.346 | 59.599 | 0.39x | 153.635 | 112.144 | 0.73x |
| dbscan_weighted_5000 | 6.125 | 25.656 | 4.19x | 6.454 | 25.652 | 3.97x |
| dbscan_weighted_20000 | 75.673 | 165.605 | 2.19x | 76.402 | 165.587 | 2.17x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cox-extended

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| stratified_weighted_1000 | 0.636 | 84.327 | 132.65x | 0.636 | 84.320 | 132.64x |
| ridge_robust_1000 | 0.696 | 79.942 | 114.83x | 0.696 | 79.938 | 114.84x |
| lasso_1000 | 5.231 | 736.769 | 140.85x | 5.230 | 736.705 | 140.85x |
| ph_test_1000 | 0.097 | 14.879 | 154.01x | 0.097 | 14.879 | 154.01x |
| predict_survival_1000 | 0.026 | 1.230 | 47.01x | 0.026 | 1.230 | 47.01x |
| time_varying_1000 | 56.840 | 229.288 | 4.03x | 56.835 | 229.246 | 4.03x |
| stratified_weighted_10000 | 7.358 | 476.755 | 64.79x | 7.395 | 476.723 | 64.47x |
| ridge_robust_10000 | 8.228 | 818.751 | 99.51x | 8.393 | 818.650 | 97.53x |
| lasso_10000 | 48.519 | 4875.504 | 100.49x | 48.544 | 4874.930 | 100.42x |
| ph_test_10000 | 1.671 | 150.826 | 90.28x | 1.722 | 150.812 | 87.58x |
| predict_survival_10000 | 0.028 | 1.232 | 44.72x | 0.028 | 1.232 | 44.72x |
| time_varying_10000 | 2185.635 | 3745.166 | 1.71x | 2185.466 | 3744.802 | 1.71x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-cross-conformal

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'mapie': '1.5.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cv_plus_n1000 | 3.315 | 28.000 | 8.45x | 3.315 | 111.639 | 33.68x |
| cv_minmax_n1000 | 2.451 | 3.027 | 1.23x | 2.451 | 12.068 | 4.92x |
| cv_plus_gamma_n1000 | 3.859 | 28.809 | 7.47x | 3.859 | 114.960 | 29.79x |
| bootstrap_plus_n1000 | 12.762 | 28.742 | 2.25x | 12.761 | 114.720 | 8.99x |
| cv_plus_n10000 | 32.171 | 95.582 | 2.97x | 32.176 | 381.010 | 11.84x |
| cv_minmax_n10000 | 26.248 | 20.975 | 0.80x | 26.244 | 83.552 | 3.18x |
| cv_plus_gamma_n10000 | 47.824 | 103.403 | 2.16x | 47.816 | 412.200 | 8.62x |
| bootstrap_plus_n10000 | 115.975 | 97.961 | 0.84x | 115.967 | 390.639 | 3.37x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.300 | 2.254 | 7.51x | 0.300 | 2.254 | 7.51x |
| glm_gamma_n1000 | 0.360 | 3.022 | 8.40x | 0.360 | 3.021 | 8.40x |
| glm_poisson_exposure_n1000 | 0.306 | 2.601 | 8.49x | 0.306 | 2.600 | 8.49x |
| mnlogit_n1000 | 0.643 | 10.398 | 16.16x | 0.643 | 10.396 | 16.16x |
| glm_negative_binomial_n10000 | 2.700 | 8.071 | 2.99x | 2.706 | 8.070 | 2.98x |
| glm_gamma_n10000 | 3.071 | 9.530 | 3.10x | 3.076 | 9.529 | 3.10x |
| glm_poisson_exposure_n10000 | 3.250 | 9.214 | 2.83x | 3.256 | 9.212 | 2.83x |
| mnlogit_n10000 | 6.441 | 55.894 | 8.68x | 6.447 | 55.883 | 8.67x |
| glm_negative_binomial_n100000 | 27.701 | 75.371 | 2.72x | 27.970 | 300.831 | 10.76x |
| glm_gamma_n100000 | 32.152 | 88.821 | 2.76x | 32.343 | 354.644 | 10.97x |
| glm_poisson_exposure_n100000 | 41.255 | 82.410 | 2.00x | 41.730 | 328.927 | 7.88x |
| mnlogit_n100000 | 64.673 | 513.731 | 7.94x | 64.709 | 1097.867 | 16.97x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 118.8 | 27.9 | 4.25x C# faster |
| latin | 32 | 181.5 | 88.2 | 2.06x C# faster |
| latin | 128 | 481.7 | 425.2 | 1.13x C# faster |
| latin | 512 | 4494.1 | 4987.9 | 1.11x Py faster |
| cjk | 8 | 144.4 | 26.1 | 5.53x C# faster |
| cjk | 32 | 333.2 | 220.4 | 1.51x C# faster |
| cjk | 128 | 1913.8 | 1379.2 | 1.39x C# faster |
| cjk | 512 | 14860.6 | 9003.1 | 1.65x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-iv

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| 2sls_robust_n1000 | 1.333 | 39.006 | 29.26x | 1.333 | 39.001 | 29.27x |
| liml_robust_n1000 | 1.458 | 39.566 | 27.14x | 1.458 | 39.563 | 27.14x |
| gmm_robust_n1000 | 1.348 | 39.225 | 29.10x | 1.348 | 39.219 | 29.10x |
| 2sls_kernel_n1000 | 7.280 | 41.847 | 5.75x | 7.280 | 41.845 | 5.75x |
| 2sls_clustered_n1000 | 1.294 | 40.753 | 31.51x | 1.293 | 40.747 | 31.51x |
| 2sls_robust_n10000 | 13.624 | 192.213 | 14.11x | 13.957 | 384.454 | 27.55x |
| liml_robust_n10000 | 14.936 | 181.922 | 12.18x | 15.293 | 363.665 | 23.78x |
| gmm_robust_n10000 | 13.705 | 182.294 | 13.30x | 14.072 | 364.538 | 25.90x |
| 2sls_kernel_n10000 | 77.955 | 194.603 | 2.50x | 78.282 | 389.089 | 4.97x |
| 2sls_clustered_n10000 | 12.829 | 201.255 | 15.69x | 13.071 | 402.466 | 30.79x |
| 2sls_robust_n100000 | 141.236 | 1950.298 | 13.81x | 143.174 | 6388.352 | 44.62x |
| liml_robust_n100000 | 155.480 | 1949.922 | 12.54x | 158.373 | 6376.960 | 40.27x |
| gmm_robust_n100000 | 143.797 | 1911.607 | 13.29x | 146.562 | 6231.410 | 42.52x |
| 2sls_kernel_n100000 | 2891.968 | 2713.866 | 0.94x | 2893.264 | 8726.009 | 3.02x |
| 2sls_clustered_n100000 | 135.420 | 2117.852 | 15.64x | 137.977 | 7018.533 | 50.87x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 152.4 | 18.5 | 8.24x C# faster |
| latin | 32 | 284.3 | 158.0 | 1.80x C# faster |
| latin | 128 | 1694.3 | 887.6 | 1.91x C# faster |
| latin | 512 | 14191.7 | 11541.8 | 1.23x C# faster |
| cjk | 8 | 161.9 | 17.9 | 9.06x C# faster |
| cjk | 32 | 386.9 | 289.6 | 1.34x C# faster |
| cjk | 128 | 2977.3 | 2015.1 | 1.48x C# faster |
| cjk | 512 | 23604.6 | 17791.9 | 1.33x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 0.988 | 226.12x | 0.004 | 0.988 | 226.09x |
| accuracy_n1000_k2 | 0.000 | 0.527 | 4352.58x | 0.000 | 0.527 | 4352.18x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.819 | 354.38x | 0.005 | 1.819 | 354.35x |
| classification_report_n1000_k2 | 0.007 | 7.018 | 954.51x | 0.007 | 7.017 | 954.50x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.961 | 109.07x | 0.018 | 1.961 | 108.99x |
| balanced_accuracy_n1000_k2 | 0.004 | 1.047 | 235.42x | 0.004 | 1.047 | 235.45x |
| matthews_n1000_k2 | 0.005 | 2.041 | 439.24x | 0.005 | 2.041 | 439.30x |
| cohen_kappa_n1000_k2 | 0.004 | 1.099 | 245.92x | 0.004 | 1.099 | 245.91x |
| mse_n1000_k2 | 0.000 | 0.313 | 647.28x | 0.000 | 0.313 | 647.22x |
| mae_n1000_k2 | 0.000 | 0.312 | 642.89x | 0.000 | 0.312 | 642.69x |
| median_ae_n1000_k2 | 0.006 | 0.322 | 55.61x | 0.006 | 0.322 | 55.61x |
| r2_n1000_k2 | 0.001 | 0.379 | 318.96x | 0.001 | 0.379 | 318.96x |
| confusion_matrix_n1000_k10 | 0.005 | 0.991 | 190.52x | 0.005 | 0.991 | 190.52x |
| accuracy_n1000_k10 | 0.000 | 0.534 | 4414.59x | 0.000 | 0.534 | 4415.18x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.831 | 310.99x | 0.006 | 1.831 | 310.98x |
| classification_report_n1000_k10 | 0.012 | 7.072 | 595.77x | 0.012 | 7.072 | 595.77x |
| roc_auc_ovr_macro_n1000_k10 | 0.524 | 9.865 | 18.83x | 0.524 | 9.863 | 18.82x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.053 | 183.70x | 0.006 | 1.053 | 183.68x |
| matthews_n1000_k10 | 0.006 | 2.048 | 362.52x | 0.006 | 2.048 | 362.55x |
| cohen_kappa_n1000_k10 | 0.006 | 1.097 | 180.89x | 0.006 | 1.096 | 180.85x |
| mse_n1000_k10 | 0.000 | 0.309 | 634.08x | 0.000 | 0.309 | 634.07x |
| mae_n1000_k10 | 0.000 | 0.309 | 633.10x | 0.000 | 0.309 | 633.09x |
| median_ae_n1000_k10 | 0.006 | 0.319 | 56.22x | 0.006 | 0.319 | 56.21x |
| r2_n1000_k10 | 0.001 | 0.378 | 317.01x | 0.001 | 0.378 | 317.03x |
| confusion_matrix_n100000_k2 | 0.460 | 10.919 | 23.71x | 0.460 | 10.917 | 23.71x |
| accuracy_n100000_k2 | 0.012 | 3.804 | 313.26x | 0.012 | 3.803 | 313.40x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.543 | 27.25x | 0.460 | 12.541 | 27.25x |
| classification_report_n100000_k2 | 0.464 | 27.655 | 59.66x | 0.463 | 27.651 | 59.66x |
| roc_auc_binary_n100000_k2 | 4.264 | 27.665 | 6.49x | 4.264 | 27.661 | 6.49x |
| balanced_accuracy_n100000_k2 | 0.457 | 10.924 | 23.89x | 0.457 | 10.922 | 23.89x |
| matthews_n100000_k2 | 0.449 | 22.062 | 49.09x | 0.450 | 22.062 | 49.01x |
| cohen_kappa_n100000_k2 | 0.453 | 11.058 | 24.39x | 0.453 | 11.056 | 24.39x |
| mse_n100000_k2 | 0.045 | 0.469 | 10.37x | 0.045 | 0.469 | 10.37x |
| mae_n100000_k2 | 0.045 | 0.458 | 10.20x | 0.045 | 0.458 | 10.20x |
| median_ae_n100000_k2 | 0.685 | 1.822 | 2.66x | 0.704 | 1.822 | 2.59x |
| r2_n100000_k2 | 0.110 | 0.706 | 6.44x | 0.110 | 0.706 | 6.44x |
| confusion_matrix_n100000_k10 | 0.492 | 11.041 | 22.44x | 0.492 | 11.036 | 22.43x |
| accuracy_n100000_k10 | 0.011 | 3.836 | 344.51x | 0.011 | 3.835 | 344.49x |
| precision_recall_f1_macro_n100000_k10 | 0.493 | 13.141 | 26.65x | 0.493 | 13.139 | 26.65x |
| classification_report_n100000_k10 | 0.500 | 30.233 | 60.47x | 0.500 | 30.229 | 60.47x |
| roc_auc_ovr_macro_n100000_k10 | 33.352 | 224.751 | 6.74x | 33.350 | 224.726 | 6.74x |
| balanced_accuracy_n100000_k10 | 0.493 | 11.106 | 22.52x | 0.493 | 11.104 | 22.52x |
| matthews_n100000_k10 | 0.493 | 22.808 | 46.26x | 0.493 | 22.806 | 46.26x |
| cohen_kappa_n100000_k10 | 0.493 | 11.208 | 22.71x | 0.493 | 11.206 | 22.71x |
| mse_n100000_k10 | 0.045 | 0.463 | 10.24x | 0.045 | 0.463 | 10.23x |
| mae_n100000_k10 | 0.045 | 0.456 | 10.13x | 0.045 | 0.455 | 10.13x |
| median_ae_n100000_k10 | 0.686 | 1.804 | 2.63x | 0.734 | 1.804 | 2.46x |
| r2_n100000_k10 | 0.110 | 0.709 | 6.46x | 0.110 | 0.709 | 6.46x |
| confusion_matrix_n1000000_k2 | 4.605 | 102.232 | 22.20x | 4.605 | 102.196 | 22.19x |
| accuracy_n1000000_k2 | 0.135 | 33.913 | 251.69x | 0.135 | 33.910 | 251.68x |
| precision_recall_f1_macro_n1000000_k2 | 4.602 | 109.377 | 23.77x | 4.602 | 109.345 | 23.76x |
| classification_report_n1000000_k2 | 4.609 | 214.255 | 46.48x | 4.609 | 214.219 | 46.48x |
| roc_auc_binary_n1000000_k2 | 56.208 | 296.551 | 5.28x | 56.202 | 296.506 | 5.28x |
| balanced_accuracy_n1000000_k2 | 4.602 | 101.397 | 22.03x | 4.602 | 101.385 | 22.03x |
| matthews_n1000000_k2 | 4.602 | 203.869 | 44.30x | 4.602 | 203.843 | 44.30x |
| cohen_kappa_n1000000_k2 | 4.602 | 101.901 | 22.14x | 4.602 | 101.888 | 22.14x |
| mse_n1000000_k2 | 0.458 | 2.661 | 5.81x | 0.458 | 2.660 | 5.81x |
| mae_n1000000_k2 | 0.452 | 2.495 | 5.52x | 0.452 | 2.495 | 5.52x |
| median_ae_n1000000_k2 | 6.307 | 14.643 | 2.32x | 6.385 | 14.641 | 2.29x |
| r2_n1000000_k2 | 1.100 | 4.678 | 4.25x | 1.100 | 4.678 | 4.25x |
| confusion_matrix_n1000000_k10 | 4.922 | 101.054 | 20.53x | 4.921 | 101.016 | 20.53x |
| accuracy_n1000000_k10 | 0.112 | 33.425 | 297.57x | 0.112 | 33.419 | 297.55x |
| precision_recall_f1_macro_n1000000_k10 | 4.919 | 114.713 | 23.32x | 4.918 | 114.706 | 23.32x |
| classification_report_n1000000_k10 | 4.927 | 237.304 | 48.16x | 4.927 | 237.264 | 48.16x |
| balanced_accuracy_n1000000_k10 | 4.921 | 100.707 | 20.47x | 4.920 | 100.698 | 20.47x |
| matthews_n1000000_k10 | 4.924 | 209.816 | 42.61x | 4.924 | 209.784 | 42.61x |
| cohen_kappa_n1000000_k10 | 4.925 | 100.903 | 20.49x | 4.924 | 100.884 | 20.49x |
| mse_n1000000_k10 | 0.456 | 2.487 | 5.46x | 0.456 | 2.486 | 5.45x |
| mae_n1000000_k10 | 0.452 | 2.488 | 5.50x | 0.452 | 2.488 | 5.50x |
| median_ae_n1000000_k10 | 6.858 | 14.616 | 2.13x | 7.027 | 14.614 | 2.08x |
| r2_n1000000_k10 | 1.119 | 4.674 | 4.18x | 1.119 | 4.673 | 4.18x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 1.697 | 0.543 | 0.32x | 1.696 | 0.543 | 0.32x |
| transform_frobenius_n500 | 5.421 | 1.030 | 0.19x | 5.420 | 1.030 | 0.19x |
| transform_frobenius_n2000 | 19.574 | 3.308 | 0.17x | 19.597 | 13.199 | 0.67x |
| transform_kl_n100 | 1.003 | 11.134 | 11.10x | 1.003 | 11.133 | 11.10x |
| transform_kl_n500 | 4.545 | 23.565 | 5.18x | 4.545 | 23.565 | 5.18x |
| transform_kl_n2000 | 17.968 | 90.613 | 5.04x | 18.009 | 361.857 | 20.09x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-10-14, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.045 | 2.982 | 65.80x | 0.045 | 2.981 | 65.81x |
| ols_hac_n1000 | 0.167 | 3.468 | 20.82x | 0.167 | 3.468 | 20.82x |
| ols_cluster_n1000 | 0.060 | 3.514 | 59.03x | 0.060 | 3.513 | 59.03x |
| ols_summary_n10000 | 0.436 | 11.433 | 26.22x | 0.436 | 11.432 | 26.22x |
| ols_hac_n10000 | 1.680 | 12.677 | 7.55x | 1.688 | 12.676 | 7.51x |
| ols_cluster_n10000 | 0.599 | 12.972 | 21.67x | 0.602 | 12.971 | 21.54x |
| ols_summary_n100000 | 4.264 | 108.973 | 25.56x | 4.285 | 434.785 | 101.48x |
| ols_hac_n100000 | 16.699 | 119.690 | 7.17x | 17.017 | 477.666 | 28.07x |
| ols_cluster_n100000 | 6.521 | 124.379 | 19.07x | 6.724 | 496.598 | 73.86x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-onehot

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| sparse_10000 | 1.435 | 3.335 | 2.32x | 1.439 | 3.335 | 2.32x |
| sparse_infrequent_10000 | 1.787 | 4.087 | 2.29x | 1.791 | 4.086 | 2.28x |
| sparse_100000 | 14.372 | 24.299 | 1.69x | 14.676 | 24.297 | 1.66x |
| sparse_infrequent_100000 | 15.964 | 29.141 | 1.83x | 16.384 | 29.138 | 1.78x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-panel

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'linearmodels': '7.0', 'numpy': '2.5.3', 'pandas': '2.3.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fe_entity_n1000 | 0.170 | 13.912 | 81.82x | 0.170 | 13.910 | 81.81x |
| fe_twoway_clustered_n1000 | 0.304 | 21.803 | 71.62x | 0.304 | 21.801 | 71.62x |
| fe_kernel_n1000 | 0.184 | 14.461 | 78.62x | 0.184 | 14.458 | 78.61x |
| between_robust_n1000 | 0.096 | 13.521 | 141.24x | 0.096 | 13.520 | 141.24x |
| first_difference_robust_n1000 | 0.199 | 15.840 | 79.57x | 0.199 | 15.839 | 79.56x |
| random_effects_n1000 | 0.181 | 18.317 | 101.42x | 0.181 | 18.314 | 101.41x |
| fe_entity_n10000 | 2.109 | 21.415 | 10.16x | 2.180 | 21.412 | 9.82x |
| fe_twoway_clustered_n10000 | 3.800 | 41.324 | 10.87x | 4.004 | 82.636 | 20.64x |
| fe_kernel_n10000 | 2.294 | 21.921 | 9.55x | 2.330 | 21.919 | 9.41x |
| between_robust_n10000 | 1.167 | 20.696 | 17.74x | 1.170 | 20.693 | 17.68x |
| first_difference_robust_n10000 | 2.342 | 24.293 | 10.37x | 2.353 | 24.290 | 10.32x |
| random_effects_n10000 | 2.209 | 28.268 | 12.80x | 2.235 | 28.266 | 12.65x |
| fe_entity_n100000 | 26.201 | 134.701 | 5.14x | 27.782 | 538.015 | 19.37x |
| fe_twoway_clustered_n100000 | 37.507 | 251.242 | 6.70x | 38.444 | 981.977 | 25.54x |
| fe_kernel_n100000 | 25.278 | 125.601 | 4.97x | 26.182 | 500.882 | 19.13x |
| between_robust_n100000 | 14.877 | 126.149 | 8.48x | 15.361 | 461.997 | 30.08x |
| first_difference_robust_n100000 | 26.428 | 144.347 | 5.46x | 27.696 | 575.308 | 20.77x |
| random_effects_n100000 | 24.865 | 165.928 | 6.67x | 25.728 | 662.216 | 25.74x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.334 | 9.550 | 1.79x | 5.705 | 9.549 | 1.67x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.169 | 16.680 | 1.37x | 12.490 | 16.677 | 1.34x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.928 | 37.182 | 2.67x | 14.315 | 37.175 | 2.60x | 1,990,038 | 1,990,038 |
| spiece_model | 7.838 | 28.988 | 3.70x | 8.514 | 28.986 | 3.40x | 533,084 | 533,084 |
| tfidf_save | 1.547 | 2.358 | 1.52x | 1.589 | 2.358 | 1.48x | 581,787 | 591,922 |
| tfidf_load | 4.661 | 4.105 | 0.88x | 4.853 | 4.105 | 0.85x | 581,787 | 591,922 |
| embedding_index_save | 4.416 | 1.546 | 0.35x | 4.592 | 1.546 | 0.34x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.181 | 37.365 | 0.76x | 10.668 | 5.147 | 0.48x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.699 | 1.469 | 0.26x | 6.042 | 1.469 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 7.531 | 1.009 | 0.13x | 7.861 | 1.006 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.607 | 1.486 | 0.32x | 4.929 | 1.486 | 0.30x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.370 | 1.513 | 1.10x | 1.520 | 1.513 | 1.00x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 92.16x | 0.000 | 0.001 | 92.17x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 409.006 | 554.567 | 1.36x | 408.983 | 554.543 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 71.870 | 66.776 | 0.93x | 72.253 | 66.771 | 0.92x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.086 | 0.116 | 1.36x | 0.086 | 0.116 | 1.36x |
| stratified_n10000 | 0.178 | 0.870 | 4.88x | 0.178 | 0.870 | 4.88x |
| traintest_n10000 | 0.005 | 0.234 | 50.89x | 0.005 | 0.234 | 50.89x |
| kfold_seeded_n10000 | 0.306 | 0.673 | 2.20x | 0.306 | 0.673 | 2.20x |
| stratified_seeded_n10000 | 0.421 | 1.479 | 3.51x | 0.421 | 1.479 | 3.51x |
| traintest_seeded_n10000 | 0.160 | 0.523 | 3.27x | 0.160 | 0.523 | 3.27x |
| stratified_traintest_n10000 | 0.665 | 2.761 | 4.15x | 0.665 | 2.761 | 4.15x |
| group_kfold_n10000 | 0.123 | 0.600 | 4.89x | 0.123 | 0.600 | 4.89x |
| stratified_group_kfold_n10000 | 0.240 | 32.162 | 133.75x | 0.240 | 32.158 | 133.74x |
| timeseries_n10000 | 0.000 | 0.012 | 97.09x | 0.000 | 0.012 | 97.09x |
| repeated_kfold_n10000 | 0.959 | 1.790 | 1.87x | 0.959 | 1.790 | 1.87x |
| kfold_n100000 | 1.154 | 2.537 | 2.20x | 1.322 | 2.537 | 1.92x |
| stratified_n100000 | 2.154 | 7.131 | 3.31x | 2.297 | 7.129 | 3.10x |
| traintest_n100000 | 0.213 | 0.405 | 1.90x | 0.215 | 0.405 | 1.88x |
| kfold_seeded_n100000 | 3.574 | 6.876 | 1.92x | 3.720 | 6.876 | 1.85x |
| stratified_seeded_n100000 | 4.736 | 11.383 | 2.40x | 4.934 | 11.382 | 2.31x |
| traintest_seeded_n100000 | 2.047 | 2.001 | 0.98x | 2.089 | 2.001 | 0.96x |
| stratified_traintest_n100000 | 8.099 | 23.574 | 2.91x | 8.197 | 23.572 | 2.88x |
| group_kfold_n100000 | 1.579 | 6.634 | 4.20x | 1.719 | 6.634 | 3.86x |
| stratified_group_kfold_n100000 | 2.740 | 326.937 | 119.32x | 2.890 | 326.897 | 113.10x |
| timeseries_n100000 | 0.000 | 0.042 | 334.93x | 0.000 | 0.042 | 334.92x |
| repeated_kfold_n100000 | 10.077 | 19.007 | 1.89x | 10.266 | 19.004 | 1.85x |
| kfold_n1000000 | 9.475 | 17.625 | 1.86x | 10.117 | 17.619 | 1.74x |
| stratified_n1000000 | 20.117 | 66.740 | 3.32x | 20.764 | 66.730 | 3.21x |
| traintest_n1000000 | 0.481 | 2.075 | 4.32x | 0.553 | 2.075 | 3.76x |
| kfold_seeded_n1000000 | 36.678 | 58.600 | 1.60x | 37.190 | 58.597 | 1.58x |
| stratified_seeded_n1000000 | 50.389 | 106.692 | 2.12x | 51.870 | 106.687 | 2.06x |
| traintest_seeded_n1000000 | 20.836 | 19.222 | 0.92x | 21.213 | 19.220 | 0.91x |
| stratified_traintest_n1000000 | 93.527 | 238.394 | 2.55x | 94.047 | 238.373 | 2.53x |
| group_kfold_n1000000 | 14.488 | 52.317 | 3.61x | 15.084 | 52.315 | 3.47x |
| stratified_group_kfold_n1000000 | 26.823 | 3247.987 | 121.09x | 27.573 | 3247.655 | 117.78x |
| timeseries_n1000000 | 0.000 | 0.328 | 3154.31x | 0.000 | 0.328 | 3154.15x |
| repeated_kfold_n1000000 | 106.660 | 177.725 | 1.67x | 108.281 | 177.718 | 1.64x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-10-14, measured at commit `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.006 | 0.815 | 147.76x | 0.006 | 0.815 | 147.74x |
| mann_whitney_n1000 | 0.029 | 0.743 | 25.61x | 0.029 | 0.743 | 25.60x |
| chi_square_n1000 | 0.000 | 0.345 | 1522.27x | 0.000 | 0.345 | 1522.27x |
| fligner_n1000 | 0.078 | 0.820 | 10.56x | 0.078 | 0.820 | 10.56x |
| anderson_ksamp_n1000 | 0.165 | 0.399 | 2.42x | 0.165 | 0.399 | 2.42x |
| spearman_matrix_n1000 | 0.114 | 0.931 | 8.19x | 0.114 | 0.931 | 8.19x |
| pointbiserial_n1000 | 0.011 | 0.579 | 53.02x | 0.011 | 0.579 | 53.02x |
| welch_t_n10000 | 0.052 | 0.867 | 16.57x | 0.052 | 0.867 | 16.57x |
| mann_whitney_n10000 | 0.766 | 2.853 | 3.72x | 0.766 | 2.852 | 3.72x |
| chi_square_n10000 | 0.001 | 0.349 | 257.23x | 0.001 | 0.349 | 257.22x |
| fligner_n10000 | 1.424 | 3.262 | 2.29x | 1.494 | 3.261 | 2.18x |
| anderson_ksamp_n10000 | 2.057 | 4.002 | 1.95x | 2.185 | 4.002 | 1.83x |
| spearman_matrix_n10000 | 2.355 | 4.838 | 2.05x | 2.415 | 4.838 | 2.00x |
| pointbiserial_n10000 | 0.103 | 0.646 | 6.30x | 0.103 | 0.645 | 6.30x |
| welch_t_n100000 | 0.519 | 1.269 | 2.45x | 0.518 | 1.269 | 2.45x |
| mann_whitney_n100000 | 8.130 | 28.815 | 3.54x | 8.128 | 28.813 | 3.54x |
| chi_square_n100000 | 0.014 | 0.369 | 25.44x | 0.014 | 0.369 | 25.43x |
| fligner_n100000 | 13.576 | 32.767 | 2.41x | 13.840 | 32.762 | 2.37x |
| anderson_ksamp_n100000 | 15.154 | 44.050 | 2.91x | 15.529 | 44.045 | 2.84x |
| spearman_matrix_n100000 | 19.047 | 52.191 | 2.74x | 19.464 | 52.190 | 2.68x |
| pointbiserial_n100000 | 1.326 | 1.432 | 1.08x | 1.343 | 5.714 | 4.26x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-family

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| logrank_wilcoxon_weighted_1000 | 0.019 | 9.364 | 485.89x | 0.019 | 9.363 | 485.88x |
| logrank_fleming_harrington_1000 | 0.027 | 14.150 | 527.88x | 0.027 | 14.148 | 527.87x |
| multigroup_1000 | 0.091 | 19.132 | 210.81x | 0.091 | 19.132 | 210.82x |
| pairwise_1000 | 0.222 | 97.732 | 439.47x | 0.222 | 97.723 | 439.45x |
| rmst_1000 | 0.072 | 799.371 | 11114.66x | 0.072 | 799.254 | 11113.32x |
| fixed_point_1000 | 0.029 | 10.129 | 347.58x | 0.029 | 10.128 | 347.55x |
| concordance_1000 | 0.041 | 6.019 | 146.79x | 0.041 | 6.019 | 146.82x |
| logrank_wilcoxon_weighted_10000 | 0.202 | 10.819 | 53.46x | 0.202 | 10.819 | 53.46x |
| logrank_fleming_harrington_10000 | 0.275 | 17.434 | 63.34x | 0.275 | 17.433 | 63.34x |
| multigroup_10000 | 2.004 | 30.017 | 14.98x | 2.009 | 30.013 | 14.94x |
| pairwise_10000 | 3.835 | 113.221 | 29.52x | 3.835 | 113.213 | 29.52x |
| rmst_10000 | 1.535 | 869.516 | 566.58x | 1.563 | 869.461 | 556.14x |
| fixed_point_10000 | 0.296 | 12.143 | 41.01x | 0.296 | 12.141 | 41.00x |
| concordance_10000 | 2.038 | 72.350 | 35.50x | 2.038 | 72.344 | 35.50x |
| logrank_wilcoxon_weighted_100000 | 3.719 | 18.397 | 4.95x | 3.809 | 18.395 | 4.83x |
| logrank_fleming_harrington_100000 | 3.646 | 28.896 | 7.93x | 3.692 | 28.894 | 7.83x |
| multigroup_100000 | 9.244 | 41.632 | 4.50x | 9.348 | 41.626 | 4.45x |
| pairwise_100000 | 38.017 | 186.261 | 4.90x | 39.073 | 186.234 | 4.77x |
| rmst_100000 | 8.021 | 320.135 | 39.91x | 8.136 | 320.132 | 39.35x |
| fixed_point_100000 | 4.010 | 22.780 | 5.68x | 4.133 | 22.778 | 5.51x |
| concordance_100000 | 25.403 | 612.215 | 24.10x | 25.657 | 612.211 | 23.86x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-parametric

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| weibull_1000 | 2.449 | 45.771 | 18.69x | 2.449 | 45.767 | 18.69x |
| loglogistic_left_1000 | 4.685 | 65.462 | 13.97x | 4.685 | 65.455 | 13.97x |
| lognormal_interval_1000 | 4.118 | 117.080 | 28.43x | 4.117 | 117.074 | 28.43x |
| generalized_gamma_1000 | 36.072 | 640.309 | 17.75x | 36.066 | 640.212 | 17.75x |
| weibull_aft_1000 | 5.417 | 85.246 | 15.74x | 5.419 | 85.245 | 15.73x |
| lognormal_aft_ridge_robust_1000 | 8.530 | 3143.930 | 368.57x | 8.534 | 3143.566 | 368.34x |
| loglogistic_aft_interval_1000 | 8.380 | 134.564 | 16.06x | 8.465 | 134.541 | 15.89x |
| breslow_fleming_harrington_1000 | 0.114 | 4.663 | 40.77x | 0.114 | 4.663 | 40.77x |
| kaplan_meier_left_1000 | 0.070 | 5.530 | 78.51x | 0.070 | 5.529 | 78.50x |
| weibull_10000 | 24.758 | 51.991 | 2.10x | 24.755 | 51.991 | 2.10x |
| loglogistic_left_10000 | 51.631 | 108.221 | 2.10x | 51.627 | 108.215 | 2.10x |
| lognormal_interval_10000 | 41.782 | 220.521 | 5.28x | 41.779 | 220.516 | 5.28x |
| generalized_gamma_10000 | 335.233 | 1604.253 | 4.79x | 335.220 | 1604.133 | 4.79x |
| weibull_aft_10000 | 54.736 | 96.376 | 1.76x | 54.737 | 96.372 | 1.76x |
| lognormal_aft_ridge_robust_10000 | 110.676 | 30220.725 | 273.06x | 110.676 | 30218.173 | 273.03x |
| loglogistic_aft_interval_10000 | 78.560 | 179.419 | 2.28x | 78.559 | 179.399 | 2.28x |
| breslow_fleming_harrington_10000 | 2.119 | 6.130 | 2.89x | 2.119 | 6.129 | 2.89x |
| kaplan_meier_left_10000 | 0.843 | 7.181 | 8.52x | 0.843 | 7.180 | 8.52x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.219 | 0.308 | 1.41x | 0.219 | 0.308 | 1.40x |
| polynomial_n10000 | 0.249 | 0.703 | 2.82x | 0.304 | 0.703 | 2.31x |
| kbins_fit_n10000 | 1.288 | 1.764 | 1.37x | 1.288 | 1.764 | 1.37x |
| kbins_n10000 | 0.458 | 3.840 | 8.39x | 0.533 | 3.840 | 7.20x |
| quantile_fit_n10000 | 1.366 | 3.134 | 2.29x | 1.366 | 3.134 | 2.29x |
| quantile_n10000 | 1.433 | 1.346 | 0.94x | 1.435 | 1.345 | 0.94x |
| power_fit_n10000 | 5.001 | 56.042 | 11.21x | 5.005 | 56.035 | 11.20x |
| power_n10000 | 1.203 | 0.682 | 0.57x | 1.205 | 0.682 | 0.57x |
| knn_impute_n10000 | 5.501 | 24.590 | 4.47x | 5.501 | 98.154 | 17.84x |
| label_n10000 | 0.986 | 0.613 | 0.62x | 0.986 | 0.613 | 0.62x |
| normalize_n100000 | 1.279 | 1.446 | 1.13x | 1.616 | 1.446 | 0.89x |
| polynomial_n100000 | 2.182 | 4.994 | 2.29x | 2.714 | 4.994 | 1.84x |
| kbins_fit_n100000 | 12.626 | 6.439 | 0.51x | 13.247 | 6.438 | 0.49x |
| kbins_n100000 | 4.079 | 13.136 | 3.22x | 4.623 | 13.134 | 2.84x |
| quantile_fit_n100000 | 12.725 | 15.349 | 1.21x | 13.184 | 15.348 | 1.16x |
| quantile_n100000 | 13.354 | 12.196 | 0.91x | 13.712 | 12.196 | 0.89x |
| power_fit_n100000 | 52.345 | 329.003 | 6.29x | 54.221 | 329.005 | 6.07x |
| power_n100000 | 10.881 | 4.039 | 0.37x | 11.263 | 4.038 | 0.36x |
| knn_impute_n100000 | 5.500 | 21.128 | 3.84x | 5.500 | 84.313 | 15.33x |
| label_n100000 | 12.483 | 6.222 | 0.50x | 12.722 | 6.221 | 0.49x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-10-15, measured at commit `fd2e93693d2ef2d4f90ea338ac7f08615c02f6c5`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.032 | 1.937 | 59.77x | 0.032 | 1.937 | 59.77x |
| var_n10000 | 0.385 | 12.522 | 32.52x | 0.387 | 12.521 | 32.36x |
| var_n100000 | 3.305 | 119.632 | 36.19x | 3.451 | 119.623 | 34.66x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
