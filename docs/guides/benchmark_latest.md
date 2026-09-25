# Latest known benchmark result, per method

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml` from the
> wiki's own history, alongside [nightly_run](nightly_run).

**Not a comparison across methods.** Each section below is the last night that method was
actually re-run -- whichever night touched the source near it, not necessarily last night,
and not the same night as its neighbours here. Every run measures on a GitHub hosted
runner whose hardware differs night to night, so a number here says "this is the last
known reading", never "faster than the section above it".

## Per method

### Lodestar.Stats.Benchmarks.CorrelationBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                         | PairCount | Mean             | Error           | StdDev        | Ratio    | RatioSD | Gen0    | Allocated | Alloc Ratio |
|------------------------------- |---------- |-----------------:|----------------:|--------------:|---------:|--------:|--------:|----------:|------------:|
| **Lodestar_Pearson**               | **100**       |         **974.7 ns** |        **74.22 ns** |       **4.07 ns** |     **1.00** |    **0.01** |       **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 100       |         797.3 ns |        15.69 ns |       0.86 ns |     0.82 |    0.00 |  0.0095 |     168 B |          NA |
| Lodestar_Spearman              | 100       |       2,991.3 ns |        23.26 ns |       1.27 ns |     3.07 |    0.01 |  0.2441 |    4144 B |          NA |
| MetaNumerics_Spearman          | 100       |       3,663.8 ns |        40.91 ns |       2.24 ns |     3.76 |    0.01 |  0.1183 |    2024 B |          NA |
| Lodestar_KendallTau            | 100       |       5,385.7 ns |       198.87 ns |      10.90 ns |     5.53 |    0.02 |       - |         - |          NA |
| MetaNumerics_KendallTau        | 100       |      10,799.1 ns |     4,899.35 ns |     268.55 ns |    11.08 |    0.24 |       - |     152 B |          NA |
| Lodestar_KendallTau_Asymptotic | 100       |       5,335.9 ns |        37.65 ns |       2.06 ns |     5.47 |    0.02 |       - |         - |          NA |
|                                |           |                  |                 |               |          |         |         |           |             |
| **Lodestar_Pearson**               | **10000**     |      **86,130.6 ns** |     **1,375.53 ns** |      **75.40 ns** |     **1.00** |    **0.00** |       **-** |         **-** |          **NA** |
| MetaNumerics_Pearson           | 10000     |      56,972.0 ns |     1,414.53 ns |      77.53 ns |     0.66 |    0.00 |       - |     168 B |          NA |
| Lodestar_Spearman              | 10000     |   1,418,199.7 ns |    68,434.31 ns |   3,751.12 ns |    16.47 |    0.04 | 23.4375 |  400147 B |          NA |
| MetaNumerics_Spearman          | 10000     |   1,957,132.0 ns |    98,845.67 ns |   5,418.06 ns |    22.72 |    0.06 |  7.8125 |  160430 B |          NA |
| Lodestar_KendallTau            | 10000     |   2,120,454.6 ns |    62,259.72 ns |   3,412.67 ns |    24.62 |    0.04 |       - |         - |          NA |
| MetaNumerics_KendallTau        | 10000     | 287,271,997.5 ns | 3,684,933.83 ns | 201,983.66 ns | 3,335.31 |    3.24 |       - |         - |          NA |
| Lodestar_KendallTau_Asymptotic | 10000     |   2,138,461.0 ns |    57,604.76 ns |   3,157.51 ns |    24.83 |    0.04 |       - |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **32.18 μs** |  **1.706 μs** | **0.094 μs** |  **1.00** | **0.9155** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    33.81 μs |  4.305 μs | 0.236 μs |  1.05 | 0.9155 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **2,003.30 μs** | **95.603 μs** | **5.240 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,454.56 μs | 15.631 μs | 0.857 μs |  0.73 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsExactTableBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                         | SampleSize | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |---------------:|--------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **775.8 ns** |      **10.53 ns** |      **0.58 ns** |   **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     2,191.6 ns |     206.55 ns |     11.32 ns |   2.82 |    0.01 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,515.5 ns |     365.43 ns |     20.03 ns |   3.24 |    0.02 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,996.7 ns |     381.56 ns |     20.91 ns |   5.15 |    0.02 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     7,376.1 ns |   1,299.29 ns |     71.22 ns |   9.51 |    0.08 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     9,862.7 ns |   3,818.69 ns |    209.32 ns |  12.71 |    0.23 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,494.2 ns |      36.99 ns |      2.03 ns |   3.21 |    0.00 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     4,061.1 ns |     233.24 ns |     12.78 ns |   5.23 |    0.01 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       969.4 ns |     124.19 ns |      6.81 ns |   1.25 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     3,168.7 ns |      99.33 ns |      5.44 ns |   4.08 |    0.01 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |     1,037.3 ns |      69.40 ns |      3.80 ns |   1.34 |    0.00 | 0.0477 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,069.3 ns |     129.01 ns |      7.07 ns |   1.38 |    0.01 | 0.0572 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       461.8 ns |      43.47 ns |      2.38 ns |   0.60 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,542.5 ns |     107.10 ns |      5.87 ns |   1.99 |    0.01 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |       121.8 ns |      14.86 ns |      0.81 ns |   0.16 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       670.6 ns |      47.64 ns |      2.61 ns |   0.86 |    0.00 | 0.0572 |     968 B |          NA |
|                                |            |                |               |              |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **37,579.8 ns** |     **335.23 ns** |     **18.38 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   178,738.0 ns |   1,609.95 ns |     88.25 ns |  4.756 |    0.00 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   716,969.9 ns |  16,288.12 ns |    892.81 ns | 19.079 |    0.02 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 2,096,231.3 ns | 960,866.07 ns | 52,668.31 ns | 55.781 |    1.21 | 3.9063 |   80382 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,473,575.7 ns |  21,833.86 ns |  1,196.79 ns | 39.212 |    0.03 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,645,930.0 ns | 173,764.40 ns |  9,524.61 ns | 97.018 |    0.22 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,458,716.6 ns |  50,476.44 ns |  2,766.78 ns | 38.816 |    0.07 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 2,151,924.7 ns |  90,704.37 ns |  4,971.81 ns | 57.263 |    0.12 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    84,464.5 ns |     402.93 ns |     22.09 ns |  2.248 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   267,793.0 ns |   8,415.91 ns |    461.30 ns |  7.126 |    0.01 |      - |     745 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   189,536.2 ns |   6,348.23 ns |    347.97 ns |  5.044 |    0.01 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   381,039.6 ns |  22,614.79 ns |  1,239.59 ns | 10.139 |    0.03 | 4.3945 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       460.6 ns |       0.99 ns |      0.05 ns |  0.012 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,541.5 ns |     114.09 ns |      6.25 ns |  0.041 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |       119.0 ns |       6.62 ns |      0.36 ns |  0.003 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       677.5 ns |      57.63 ns |      3.16 ns |  0.018 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MultinomialLogitBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                   | GroupSize | Mean           | Error        | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------------:|-------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_Levene**          | **100**       |     **2,035.5 ns** |    **127.30 ns** |     **6.98 ns** |  **1.00** |    **0.00** |  **0.3014** |      **-** |    **5072 B** |       **1.000** |
| Accord_Levene            | 100       |     4,022.5 ns |     37.17 ns |     2.04 ns |  1.98 |    0.01 |  0.3204 |      - |    5480 B |       1.080 |
| Lodestar_Bartlett        | 100       |       597.2 ns |     43.79 ns |     2.40 ns |  0.29 |    0.00 |  0.0048 |      - |      80 B |       0.016 |
| Accord_Bartlett          | 100       |     1,183.5 ns |     45.07 ns |     2.47 ns |  0.58 |    0.00 |  0.0095 |      - |     168 B |       0.033 |
| Lodestar_Binomial        | 100       |     1,288.7 ns |    116.84 ns |     6.40 ns |  0.63 |    0.00 |  0.0019 |      - |      48 B |       0.009 |
| Accord_Binomial          | 100       |     4,622.4 ns |    209.01 ns |    11.46 ns |  2.27 |    0.01 |  0.1297 |      - |    2216 B |       0.437 |
| Lodestar_AndersonDarling | 100       |     8,639.6 ns |    477.30 ns |    26.16 ns |  4.24 |    0.02 |  0.0458 |      - |    1000 B |       0.197 |
| Accord_AndersonDarling   | 100       |     6,274.2 ns |    244.56 ns |    13.41 ns |  3.08 |    0.01 |  0.0610 |      - |    1096 B |       0.216 |
| Lodestar_ClopperPearson  | 100       |     4,939.7 ns |    216.14 ns |    11.85 ns |  2.43 |    0.01 |       - |      - |      48 B |       0.009 |
|                          |           |                |              |             |       |         |         |        |           |             |
| **Lodestar_Levene**          | **10000**     |   **373,264.2 ns** | **11,947.39 ns** |   **654.88 ns** | **1.000** |    **0.00** | **28.3203** |      **-** |  **480272 B** |       **1.000** |
| Accord_Levene            | 10000     | 1,322,007.2 ns | 67,026.78 ns | 3,673.96 ns | 3.542 |    0.01 | 27.3438 | 5.8594 |  480681 B |       1.001 |
| Lodestar_Bartlett        | 10000     |    56,311.1 ns |    191.72 ns |    10.51 ns | 0.151 |    0.00 |       - |      - |      80 B |       0.000 |
| Accord_Bartlett          | 10000     |   112,352.6 ns |  2,559.10 ns |   140.27 ns | 0.301 |    0.00 |       - |      - |     168 B |       0.000 |
| Lodestar_Binomial        | 10000     |     1,989.9 ns |    143.65 ns |     7.87 ns | 0.005 |    0.00 |       - |      - |      48 B |       0.000 |
| Accord_Binomial          | 10000     |   892,306.2 ns | 18,211.93 ns |   998.26 ns | 2.391 |    0.00 | 11.7188 | 2.9297 |  200217 B |       0.417 |
| Lodestar_AndersonDarling | 10000     | 1,395,263.9 ns | 48,857.95 ns | 2,678.07 ns | 3.738 |    0.01 |  3.9063 |      - |   80201 B |       0.167 |
| Accord_AndersonDarling   | 10000     |             NA |           NA |          NA |     ? |       ? |      NA |     NA |        NA |           ? |
| Lodestar_ClopperPearson  | 10000     |    35,154.1 ns |  4,681.52 ns |   256.61 ns | 0.094 |    0.00 |       - |      - |      48 B |       0.000 |

Benchmarks with issues:
  VarianceAndFitBenchmarks.Accord_AndersonDarling: ShortRun(IterationCount=3, LaunchCount=1, WarmupCount=3) [GroupSize=10000]

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.VectorAutoregressionBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                 | Rows | Method   | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,877.2 μs** |     **20.57 μs** |     **1.13 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    88,472.2 μs |  6,512.37 μs |   356.97 μs |  47.13 |    0.17 |  3666.6667 |  1500.0000 |  833.3333 |  59880.82 KB |       58.56 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,808.5 μs** |     **42.25 μs** |     **2.32 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    89,058.4 μs | 21,827.01 μs | 1,196.41 μs |  49.25 |    0.58 |  5000.0000 |  1800.0000 |  800.0000 |  83111.68 KB |       81.29 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **779.0 μs** |     **59.88 μs** |     **3.28 μs** |   **1.00** |    **0.01** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   135,972.6 μs | 14,088.07 μs |   772.21 μs | 174.55 |    1.07 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,167.5 μs** |     **64.57 μs** |     **3.54 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    66,796.7 μs |  5,778.71 μs |   316.75 μs |  30.82 |    0.13 |  3875.0000 |  1750.0000 |  875.0000 |  64191.22 KB |       62.78 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **13,826.6 μs** |  **2,533.25 μs** |   **138.86 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,860,498.1 μs | 53,803.13 μs | 2,949.13 μs | 134.57 |    1.19 | 35000.0000 | 10000.0000 | 4000.0000 | 537149.55 KB |       60.18 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **12,521.5 μs** |    **389.53 μs** |    **21.35 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,823,599.6 μs | 11,831.29 μs |   648.51 μs | 145.64 |    0.22 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **6,644.4 μs** |    **563.21 μs** |    **30.87 μs** |   **1.00** |    **0.01** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 2,979,997.6 μs | 56,568.38 μs | 3,100.70 μs | 448.50 |    1.85 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.31 KB |    8,147.90 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,195.8 μs** |  **1,493.56 μs** |    **81.87 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,712,285.9 μs | 85,325.79 μs | 4,676.99 μs | 105.73 |    0.52 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.738 μs** |  **1.6001 μs** | **0.0877 μs** |  **1.00** |    **0.02** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |   6.026 μs |  1.0167 μs | 0.0557 μs |  1.05 |    0.02 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |   5.966 μs |  0.3922 μs | 0.0215 μs |  1.04 |    0.01 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **56.293 μs** | **32.0973 μs** | **1.7594 μs** |  **1.00** |    **0.04** |  **1.7700** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |  22.045 μs |  5.6844 μs | 0.3116 μs |  0.39 |    0.01 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |  22.138 μs |  2.0331 μs | 0.1114 μs |  0.39 |    0.01 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **221.117 μs** | **22.1009 μs** | **1.2114 μs** |  **1.00** |    **0.01** |  **6.5918** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |  78.440 μs |  4.0662 μs | 0.2229 μs |  0.35 |    0.00 |  5.0049 | 0.1221 |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |  72.059 μs | 17.4740 μs | 0.9578 μs |  0.33 |    0.00 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |            |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **900.455 μs** | **51.5727 μs** | **2.8269 μs** |  **1.00** |    **0.00** | **26.3672** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 315.260 μs | 16.6924 μs | 0.9150 μs |  0.35 |    0.00 | 20.0195 | 2.4414 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        | 268.010 μs | 26.1407 μs | 1.4329 μs |  0.30 |    0.00 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **147.81 ms** |   **4.100 ms** | **0.225 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  73.24 ms |   4.493 ms | 0.246 ms |  0.50 |    0.00 |  103.48 KB |        3.80 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **156.34 ms** |  **70.996 ms** | **3.892 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  69.30 ms |   5.391 ms | 0.295 ms |  0.44 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **221.41 ms** |  **17.720 ms** | **0.971 ms** |  **1.00** |    **0.01** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 277.13 ms |  12.762 ms | 0.700 ms |  1.25 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **231.60 ms** |  **11.692 ms** | **0.641 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 238.97 ms |  14.854 ms | 0.814 ms |  1.03 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **280.29 ms** |  **16.428 ms** | **0.900 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 386.32 ms |  36.043 ms | 1.976 ms |  1.38 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **284.31 ms** |  **11.476 ms** | **0.629 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 359.69 ms | 106.885 ms | 5.859 ms |  1.27 |    0.02 |  1153.8 KB |        1.56 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **324.83 ms** |   **2.904 ms** | **0.159 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 448.13 ms |  36.351 ms | 1.993 ms |  1.38 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **332.13 ms** |  **52.911 ms** | **2.900 ms** |  **1.00** |    **0.01** |  **5514.7 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 439.35 ms |  11.451 ms | 0.628 ms |  1.32 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **49.81 μs** |     **1.146 μs** |   **0.063 μs** |         **-** |
| Cjk    | 1000   |      68.63 μs |    21.366 μs |   1.171 μs |         - |
| **Latin**  | **10000**  |   **4,586.17 μs** |   **339.816 μs** |  **18.626 μs** |         **-** |
| Cjk    | 10000  |   6,984.71 μs |   380.710 μs |  20.868 μs |         - |
| **Latin**  | **65536**  | **207,512.92 μs** | **4,925.031 μs** | **269.958 μs** |         **-** |

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **2.015 μs** |      **0.3711 μs** |     **0.0203 μs** |     **1.00** |    **0.01** |    **0.0229** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.438 μs |      0.1346 μs |     0.0074 μs |     1.21 |    0.01 |    0.0229 |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.443 μs |      0.3060 μs |     0.0168 μs |     1.71 |    0.02 |    0.3128 |         - |        - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   8,006.823 μs |    646.7880 μs |    35.4526 μs | 3,973.42 |   37.75 |  984.3750 |  906.2500 | 890.6250 |  4838072 B |   11,410.55 |
| LuceneFromText         | 1000      |   7,193.061 μs |    216.9693 μs |    11.8928 μs | 3,569.58 |   31.44 |   78.1250 |   62.5000 |        - |  1375078 B |    3,243.11 |
|                        |           |                |                |               |          |         |           |           |          |            |             |
| **LodestarQuery**          | **20000**     |      **24.773 μs** |      **1.5571 μs** |     **0.0853 μs** |     **1.00** |    **0.00** |         **-** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      39.864 μs |      3.9599 μs |     0.2171 μs |     1.61 |    0.01 |         - |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      19.231 μs |      0.6679 μs |     0.0366 μs |     0.78 |    0.00 |    0.4883 |         - |        - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 121,061.031 μs | 56,119.8084 μs | 3,076.1161 μs | 4,886.85 |  108.52 | 2200.0000 | 1200.0000 | 800.0000 | 83750491 B |  197,524.74 |
| LuceneFromText         | 20000     | 143,419.661 μs | 46,687.1664 μs | 2,559.0811 μs | 5,789.40 |   91.11 | 1250.0000 | 1000.0000 |        - | 22417220 B |   52,870.80 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method  | Mean     | Error    | StdDev   | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|----------:|----------:|------------:|
| Unigram | 30.24 ms | 1.014 ms | 0.056 ms |  1.00 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 81.46 ms | 1.220 ms | 0.067 ms |  2.69 | 1714.2857 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **17.39 μs** |  **2.244 μs** | **0.123 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **65.35 μs** | **12.531 μs** | **0.687 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **178.15 μs** |  **9.686 μs** | **0.531 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **376.16 μs** | **44.752 μs** | **2.453 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 8.227 ms | 2.171 ms | 0.1190 ms |   1.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean      | Error    | StdDev   | Allocated |
|----------- |--------- |----------:|---------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **19.25 μs** | **2.132 μs** | **0.117 μs** |         **-** |
| MyersGroup | cjk      | 167.81 μs | 5.921 μs | 0.325 μs |         - |
| **DpGroup**    | **latin**    |  **10.55 μs** | **1.621 μs** | **0.089 μs** |         **-** |
| MyersGroup | latin    | 112.09 μs | 1.489 μs | 0.082 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistCutoffBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                   | Cutoff | Widths | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **295.88 μs** | **76.752 μs** | **4.207 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 287.82 μs | 28.966 μs | 1.588 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **327.42 μs** | **25.213 μs** | **1.382 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 303.70 μs | 17.885 μs | 0.980 μs |  0.93 |    0.00 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **317.78 μs** | **24.685 μs** | **1.353 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 308.53 μs | 61.274 μs | 3.359 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **276.09 μs** | **28.866 μs** | **1.582 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 309.05 μs |  8.707 μs | 0.477 μs |  1.12 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **304.11 μs** | **43.371 μs** | **2.377 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 294.83 μs | 23.563 μs | 1.292 μs |  0.97 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **187.71 μs** | **10.154 μs** | **0.557 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 300.25 μs | 38.122 μs | 2.090 μs |  1.60 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **245.74 μs** | **14.962 μs** | **0.820 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 302.02 μs | 42.336 μs | 2.321 μs |  1.23 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |           |          |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **86.08 μs** | **27.527 μs** | **1.509 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 302.08 μs |  9.393 μs | 0.515 μs |  3.51 |    0.05 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|--------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **178.7 μs** |  **37.99 μs** |  **2.08 μs** |  **1.00** |    **0.01** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   183.8 μs |   6.23 μs |  0.34 μs |  1.03 |    0.01 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   489.4 μs | 383.00 μs | 20.99 μs |  2.74 |    0.11 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |         |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,210.9 μs** | **367.18 μs** | **20.13 μs** |  **1.00** |    **0.01** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,119.6 μs | 330.39 μs | 18.11 μs |  0.97 |    0.01 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,375.4 μs | 284.29 μs | 15.58 μs |  2.30 |    0.01 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method       | Shape     | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |-----------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **332.3 ms** |  **10.76 ms** |  **0.59 ms** |  **1.00** |    **0.00** |  **1500.0000** | **1500.0000** | **1500.0000** |  **28.69 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,338.0 ms | 600.02 ms | 32.89 ms |  7.04 |    0.09 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.45 |
|              |           |            |           |          |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |   **115.8 ms** |  **20.69 ms** |  **1.13 ms** |  **1.00** |    **0.01** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.29 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   807.9 ms |   8.26 ms |  0.45 ms |  6.98 |    0.06 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                   | Shape      | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **873.96 ms** |  **15.80 ms** |  **0.866 ms** |  **1.00** |    **0.00** |  **2000.0000** |  **2000.0000** |  **2000.0000** | **112.13 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 6,722.72 ms | 175.80 ms |  9.636 ms |  7.69 |    0.01 | 38000.0000 | 13000.0000 | 11000.0000 | 531.87 MB |        4.74 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,955.04 ms | 384.82 ms | 21.093 ms |  4.53 |    0.02 | 22000.0000 | 12000.0000 | 10000.0000 | 372.11 MB |        3.32 |
|                          |            |             |           |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **68.62 ms** | **197.97 ms** | **10.851 ms** |  **1.02** |    **0.21** |  **1428.5714** |  **1428.5714** |  **1428.5714** |  **14.02 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   472.12 ms |  58.50 ms |  3.207 ms |  7.01 |    1.05 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.75 |
| Dbscan_CalculateClusters | 5000x2x5   |   264.16 ms |  33.25 ms |  1.823 ms |  3.92 |    0.59 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.88 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                                    | Mean      | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       | 14.889 ms | 7.1188 ms | 0.3902 ms |  1.00 |    0.03 |
| Nmf_Rank20                                | 95.881 ms | 8.7500 ms | 0.4796 ms |  6.44 |    0.15 |
| Nmf_Transform                             |  3.707 ms | 0.3486 ms | 0.0191 ms |  0.25 |    0.01 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 23.938 ms | 0.7052 ms | 0.0387 ms |  1.61 |    0.04 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method         | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |    97.01 ns |   0.838 ns |  0.046 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   760.66 ns | 394.651 ns | 21.632 ns |  7.84 |    0.19 |      - |         - |          NA |
| TokenSortRatio |   814.01 ns | 127.898 ns |  7.010 ns |  8.39 |    0.06 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   969.33 ns |  34.619 ns |  1.898 ns |  9.99 |    0.02 | 0.0534 |     896 B |          NA |
| WRatio         | 1,192.02 ns | 442.059 ns | 24.231 ns | 12.29 |    0.22 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  1,980,648.7624 ns |   104,239.6803 ns |   5,713.7286 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    645,709.7184 ns |    17,013.9814 ns |     932.5937 ns |  0.326 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,078,116.8324 ns |    86,061.6873 ns |   4,717.3315 ns |  0.544 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 69,700,128.4583 ns | 8,420,578.6250 ns | 461,560.3303 ns | 35.191 |    0.22 |       - |       - |  510798 B |        3.18 |
| WRatio_EmptyOperand       |          0.0532 ns |         0.0128 ns |       0.0007 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **97.31 ns** |   **4.357 ns** |  **0.239 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    230.88 ns |   9.065 ns |  0.497 ns |  2.37 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **755.87 ns** |   **8.177 ns** |  **0.448 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,045.40 ns | 460.976 ns | 25.268 ns | 13.29 |    0.03 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,002.23 ns** | **168.628 ns** |  **9.243 ns** |  **1.00** |    **0.01** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,047.68 ns |  64.584 ns |  3.540 ns |  2.04 |    0.02 | 0.1144 |    1944 B |        2.17 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,264.62 ns** |  **58.267 ns** |  **3.194 ns** |  **1.00** |    **0.00** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,972.10 ns | 124.779 ns |  6.840 ns |  3.93 |    0.01 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                     | Length | Mean        | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **29.68 ns** |   **2.677 ns** |  **0.147 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    41.23 ns |   1.508 ns |  0.083 ns |  1.39 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    30.44 ns |   0.622 ns |  0.034 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    30.03 ns |  15.391 ns |  0.844 ns |  1.01 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **32.38 ns** |   **1.012 ns** |  **0.055 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    42.07 ns |  17.694 ns |  0.970 ns |  1.30 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    32.93 ns |   0.556 ns |  0.030 ns |  1.02 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    32.01 ns |   1.228 ns |  0.067 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **35.81 ns** |   **0.538 ns** |  **0.029 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    48.38 ns |   0.147 ns |  0.008 ns |  1.35 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    34.36 ns |   0.592 ns |  0.032 ns |  0.96 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    35.28 ns |   0.255 ns |  0.014 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **42.00 ns** |  **10.927 ns** |  **0.599 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    52.40 ns |   0.219 ns |  0.012 ns |  1.25 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    38.16 ns |   1.060 ns |  0.058 ns |  0.91 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    37.49 ns |   1.502 ns |  0.082 ns |  0.89 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **61.16 ns** |   **1.498 ns** |  **0.082 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    68.68 ns |   4.603 ns |  0.252 ns |  1.12 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    62.78 ns |   0.598 ns |  0.033 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    57.61 ns |   1.725 ns |  0.095 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **68.88 ns** |   **0.801 ns** |  **0.044 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    75.09 ns |   0.933 ns |  0.051 ns |  1.09 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    75.42 ns |   1.931 ns |  0.106 ns |  1.09 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    65.96 ns |   0.427 ns |  0.023 ns |  0.96 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **425.96 ns** | **235.312 ns** | **12.898 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   425.13 ns |   6.946 ns |  0.381 ns |  1.00 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   408.45 ns |   4.551 ns |  0.249 ns |  0.96 |    0.03 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   406.68 ns |   0.964 ns |  0.053 ns |  0.96 |    0.03 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **4,985.69 ns** |  **61.322 ns** |  **3.361 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 5,056.60 ns | 256.632 ns | 14.067 ns |  1.01 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,997.61 ns | 453.052 ns | 24.833 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,981.69 ns |  76.036 ns |  4.168 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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

| Method             | Length | Mean        | Error       | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|------------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **406.1 ns** |    **19.77 ns** |  **1.08 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    201.5 ns |    38.80 ns |  2.13 ns |  0.50 |         - |          NA |
|                    |        |             |             |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,419.0 ns** |    **22.64 ns** |  **1.24 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  5,116.8 ns |    90.31 ns |  4.95 ns |  2.12 |         - |          NA |
|                    |        |             |             |          |       |           |             |
| **Distance_CodePoint** | **512**    | **14,541.4 ns** |   **588.06 ns** | **32.23 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 38,100.4 ns | 1,144.09 ns | 62.71 ns |  2.62 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelPatternBenchmarks-report-github

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

| Method               | Corpus       | Length | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------- |------- |------------:|------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Pairwise**    | **beyondTable**  | **4**      |  **7,961.1 ns** |   **793.04 ns** |  **43.47 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 4      |  7,950.3 ns |   779.64 ns |  42.73 ns |  1.00 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 4      | 11,812.5 ns | 1,346.53 ns |  73.81 ns |  1.48 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **16**     |  **9,797.6 ns** | **1,850.75 ns** | **101.45 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 16     |  9,739.5 ns | 1,255.22 ns |  68.80 ns |  0.99 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 16     | 14,821.6 ns | 3,471.10 ns | 190.26 ns |  1.51 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **beyondTable**  | **32**     | **12,547.9 ns** | **6,058.44 ns** | **332.08 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | beyondTable  | 32     | 12,194.1 ns |   685.76 ns |  37.59 ns |  0.97 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | beyondTable  | 32     | 19,074.2 ns | 1,430.80 ns |  78.43 ns |  1.52 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **4**      |  **5,596.7 ns** |   **568.23 ns** |  **31.15 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 4      |  6,620.8 ns | 1,708.25 ns |  93.64 ns |  1.18 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 4      |  9,199.9 ns |   383.70 ns |  21.03 ns |  1.64 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **16**     |  **7,304.2 ns** |   **191.82 ns** |  **10.51 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 16     |  7,625.3 ns | 3,637.70 ns | 199.39 ns |  1.04 |    0.02 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 16     | 12,334.5 ns |   225.87 ns |  12.38 ns |  1.69 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **longAffix**    | **32**     | **10,098.1 ns** |   **313.44 ns** |  **17.18 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | longAffix    | 32     |  9,603.6 ns |   539.23 ns |  29.56 ns |  0.95 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | longAffix    | 32     | 16,659.1 ns | 1,550.99 ns |  85.02 ns |  1.65 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **4**      |  **4,016.2 ns** |   **322.27 ns** |  **17.66 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 4      |  4,355.3 ns |   284.76 ns |  15.61 ns |  1.08 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 4      |  8,015.1 ns | 3,173.19 ns | 173.93 ns |  2.00 |    0.04 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **16**     |  **5,708.1 ns** |   **113.44 ns** |   **6.22 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 16     |  6,226.9 ns |   359.64 ns |  19.71 ns |  1.09 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 16     | 10,926.0 ns |   689.47 ns |  37.79 ns |  1.91 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **oneWordAffix** | **32**     |  **8,296.6 ns** |   **692.22 ns** |  **37.94 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | oneWordAffix | 32     |  8,087.6 ns |   432.25 ns |  23.69 ns |  0.97 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | oneWordAffix | 32     | 15,313.3 ns |   602.76 ns |  33.04 ns |  1.85 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **4**      |  **2,531.2 ns** |    **79.77 ns** |   **4.37 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 4      |  2,196.5 ns |   154.29 ns |   8.46 ns |  0.87 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 4      |  6,139.4 ns |    79.20 ns |   4.34 ns |  2.43 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **16**     |  **4,206.1 ns** |    **58.29 ns** |   **3.20 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 16     |  2,991.7 ns |   162.92 ns |   8.93 ns |  0.71 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 16     | 10,270.6 ns |   213.74 ns |  11.72 ns |  2.44 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **sharing24**    | **32**     |  **6,846.2 ns** |   **189.17 ns** |  **10.37 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | sharing24    | 32     |  4,089.3 ns |   173.99 ns |   9.54 ns |  0.60 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | sharing24    | 32     | 13,886.0 ns | 3,635.13 ns | 199.25 ns |  2.03 |    0.03 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **4**      |  **3,925.9 ns** |   **513.63 ns** |  **28.15 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 4      |  3,592.5 ns |   132.68 ns |   7.27 ns |  0.92 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 4      |  7,544.4 ns |   389.98 ns |  21.38 ns |  1.92 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **16**     |  **5,760.4 ns** |   **123.23 ns** |   **6.75 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 16     |  4,439.2 ns |   862.95 ns |  47.30 ns |  0.77 |    0.01 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 16     | 10,918.1 ns |   875.49 ns |  47.99 ns |  1.90 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **suffixOnly**   | **32**     |  **8,249.5 ns** |    **90.90 ns** |   **4.98 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | suffixOnly   | 32     |  8,577.7 ns |   417.92 ns |  22.91 ns |  1.04 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | suffixOnly   | 32     | 15,041.9 ns |   551.99 ns |  30.26 ns |  1.82 |    0.00 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **4**      |  **1,332.9 ns** |    **43.12 ns** |   **2.36 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 4      |    596.8 ns |    36.37 ns |   1.99 ns |  0.45 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 4      |  5,037.9 ns |   419.64 ns |  23.00 ns |  3.78 |    0.02 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **16**     |  **3,118.6 ns** |     **5.35 ns** |   **0.29 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 16     |  1,433.4 ns |   107.27 ns |   5.88 ns |  0.46 |    0.00 | 0.0019 |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 16     |  8,154.2 ns |   415.20 ns |  22.76 ns |  2.61 |    0.01 | 0.3052 |    5120 B |          NA |
|                      |              |        |             |             |           |       |         |        |           |             |
| **Lodestar_Pairwise**    | **unrelated**    | **32**     |  **5,717.0 ns** |    **46.66 ns** |   **2.56 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Lodestar_HeldPattern | unrelated    | 32     |  2,455.7 ns |   143.52 ns |   7.87 ns |  0.43 |    0.00 |      - |      40 B |          NA |
| FuzzySharp_PerText   | unrelated    | 32     | 12,525.6 ns |   901.40 ns |  49.41 ns |  2.19 |    0.01 | 0.3052 |    5120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **6,694.2 μs** |    **310.11 μs** |    **17.00 μs** |  **1.00** |    **0.00** | **7.8125** |      **-** | **162.63 KB** |        **1.00** |
| NumFlat_Fit      | 10000x16x16 |  38,927.1 μs |    327.73 μs |    17.96 μs |  5.82 |    0.01 |      - |      - |  10.39 KB |        0.06 |
| MetaNumerics_Fit | 10000x16x16 |  83,365.9 μs |  1,512.87 μs |    82.93 μs | 12.45 |    0.03 |      - |      - |  41.95 KB |        0.26 |
|                  |             |              |              |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **681.5 μs** |     **47.62 μs** |     **2.61 μs** |  **1.00** |    **0.00** | **8.7891** | **0.9766** | **156.73 KB** |        **1.00** |
| NumFlat_Fit      | 10000x2x8   |   4,776.0 μs |    411.42 μs |    22.55 μs |  7.01 |    0.04 |      - |      - |      3 KB |        0.02 |
| MetaNumerics_Fit | 10000x2x8   |   2,975.0 μs |  1,187.44 μs |    65.09 μs |  4.37 |    0.08 |      - |      - |  39.57 KB |        0.25 |
|                  |             |              |              |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **34,638.5 μs** |    **882.28 μs** |    **48.36 μs** |  **1.00** |    **0.00** |      **-** |      **-** | **787.83 KB** |        **1.00** |
| NumFlat_Fit      | 50000x8x32  | 442,877.9 μs |  9,114.29 μs |   499.58 μs | 12.79 |    0.02 |      - |      - |  16.29 KB |        0.02 |
| MetaNumerics_Fit | 50000x8x32  | 744,138.5 μs | 64,945.83 μs | 3,559.90 μs | 21.48 |    0.09 |      - |      - | 199.91 KB |        0.25 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **8.700 ms** | **0.4738 ms** | **0.0260 ms** |  **1.00** |   **88.7 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x16x16 |  16.654 ms | 0.5902 ms | 0.0323 ms |  1.91 |   13.6 KB |        0.15 |
|                |             |            |           |           |       |           |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **33.714 ms** | **2.6896 ms** | **0.1474 ms** |  **1.00** |  **98.96 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 113.216 ms | 3.7628 ms | 0.2062 ms |  3.36 |  96.48 KB |        0.97 |
|                |             |            |           |           |       |           |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **84.362 ms** | **0.9045 ms** | **0.0496 ms** |  **1.00** |  **410.3 KB** |        **1.00** |
| NumFlat_Lloyd  | 50000x8x32  | 210.771 ms | 2.1841 ms | 0.1197 ms |  2.50 |  36.47 KB |        0.09 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **130.12 ns** |     **4.306 ns** |   **0.236 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.99 ns |     1.340 ns |   0.073 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    131.43 ns |    42.903 ns |   2.352 ns |  1.01 |    0.02 |         - |          NA |
| Kernel_Cjk | 8    |    109.17 ns |     3.471 ns |   0.190 ns |  0.84 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **220.85 ns** |     **2.817 ns** |   **0.154 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     68.51 ns |     2.670 ns |   0.146 ns |  0.31 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    220.75 ns |     9.154 ns |   0.502 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    118.55 ns |    11.428 ns |   0.626 ns |  0.54 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **281.01 ns** |    **27.068 ns** |   **1.484 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 14   |     74.07 ns |     1.742 ns |   0.095 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    337.44 ns |     9.298 ns |   0.510 ns |  1.20 |    0.01 |         - |          NA |
| Kernel_Cjk | 14   |    125.97 ns |     3.240 ns |   0.178 ns |  0.45 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **361.07 ns** |   **413.456 ns** |  **22.663 ns** |  **1.00** |    **0.08** |         **-** |          **NA** |
| Kernel     | 16   |     78.23 ns |     2.237 ns |   0.123 ns |  0.22 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    459.58 ns |   292.649 ns |  16.041 ns |  1.28 |    0.08 |         - |          NA |
| Kernel_Cjk | 16   |    130.60 ns |     0.702 ns |   0.038 ns |  0.36 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **719.49 ns** |   **376.045 ns** |  **20.612 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 18   |     85.51 ns |    18.773 ns |   1.029 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    479.61 ns |   439.894 ns |  24.112 ns |  0.67 |    0.03 |         - |          NA |
| Kernel_Cjk | 18   |    134.86 ns |    15.427 ns |   0.846 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **823.29 ns** |    **14.922 ns** |   **0.818 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     85.99 ns |     1.425 ns |   0.078 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    811.54 ns |   138.959 ns |   7.617 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    140.77 ns |     4.749 ns |   0.260 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |  **1,003.97 ns** |   **261.221 ns** |  **14.318 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |     97.02 ns |     1.374 ns |   0.075 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |  1,051.78 ns |   268.792 ns |  14.733 ns |  1.05 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    153.57 ns |     1.301 ns |   0.071 ns |  0.15 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,612.31 ns** |   **710.975 ns** |  **38.971 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 32   |    117.10 ns |     3.774 ns |   0.207 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,703.41 ns |   133.454 ns |   7.315 ns |  1.06 |    0.02 |         - |          NA |
| Kernel_Cjk | 32   |    182.44 ns |     7.757 ns |   0.425 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,199.07 ns** | **1,352.023 ns** |  **74.109 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 48   |    175.73 ns |    47.954 ns |   2.629 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,301.12 ns |   520.177 ns |  28.513 ns |  1.03 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    259.57 ns |    22.144 ns |   1.214 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,484.41 ns** |   **518.848 ns** |  **28.440 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    171.08 ns |     1.871 ns |   0.103 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,419.78 ns |   668.115 ns |  36.622 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    315.30 ns |    16.004 ns |   0.877 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,191.39 ns** | **7,302.735 ns** | **400.288 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 96   |    365.62 ns |     3.423 ns |   0.188 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,549.09 ns | 1,652.734 ns |  90.592 ns |  1.03 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |    953.78 ns |   100.226 ns |   5.494 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **26.94 ns** |     **0.122 ns** |  **0.007 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     26.95 ns |     0.126 ns |  0.007 ns |  1.00 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    127.26 ns |    69.773 ns |  3.824 ns |  4.72 |    0.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     28.17 ns |     0.075 ns |  0.004 ns |  1.05 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **312.37 ns** |    **12.637 ns** |  **0.693 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    401.09 ns |     3.677 ns |  0.202 ns |  1.28 |    0.00 |         - |          NA |
| Distance_CodePoint         | 64     |    739.87 ns |    51.957 ns |  2.848 ns |  2.37 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    313.96 ns |     1.946 ns |  0.107 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **997.31 ns** |     **6.103 ns** |  **0.335 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,926.33 ns |     8.276 ns |  0.454 ns |  1.93 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,699.46 ns |    30.165 ns |  1.653 ns |  1.70 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |  1,007.68 ns |   216.148 ns | 11.848 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **12,403.23 ns** |    **44.074 ns** |  **2.416 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 18,020.17 ns | 1,001.024 ns | 54.870 ns |  1.45 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 14,905.64 ns |   647.984 ns | 35.518 ns |  1.20 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 12,475.56 ns |   681.454 ns | 37.353 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **355.0 ns** |      **6.42 ns** |     **0.35 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     258.4 ns |     13.66 ns |     0.75 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **356.3 ns** |      **4.12 ns** |     **0.23 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     260.4 ns |     32.31 ns |     1.77 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **446.7 ns** |      **5.72 ns** |     **0.31 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     349.2 ns |      7.14 ns |     0.39 ns |  0.78 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **450.0 ns** |     **19.92 ns** |     **1.09 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     348.0 ns |      1.88 ns |     0.10 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **535.7 ns** |      **4.65 ns** |     **0.26 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     446.2 ns |      7.50 ns |     0.41 ns |  0.83 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **547.6 ns** |    **100.18 ns** |     **5.49 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     444.3 ns |      4.07 ns |     0.22 ns |  0.81 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **629.8 ns** |    **107.61 ns** |     **5.90 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,318.0 ns |      6.10 ns |     0.33 ns |  2.09 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **639.6 ns** |     **40.23 ns** |     **2.21 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,304.1 ns |     34.52 ns |     1.89 ns |  2.04 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,075.2 ns** |     **61.42 ns** |     **3.37 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,725.3 ns |    295.96 ns |    16.22 ns |  2.76 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,050.9 ns** |     **32.46 ns** |     **1.78 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,733.5 ns |    284.17 ns |    15.58 ns |  2.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **16,493.4 ns** |    **105.35 ns** |     **5.77 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  68,931.5 ns |  1,447.85 ns |    79.36 ns |  4.18 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **447,775.2 ns** | **18,741.18 ns** | **1,027.27 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  65,665.2 ns |  2,194.08 ns |   120.26 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **27.15 ns** |      **5.343 ns** |     **0.293 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      87.58 ns |     10.626 ns |     0.582 ns |  3.23 |    0.04 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.82 ns |      1.088 ns |     0.060 ns |  3.01 |    0.03 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     162.07 ns |      6.002 ns |     0.329 ns |  5.97 |    0.06 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **313.53 ns** |      **3.053 ns** |     **0.167 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,889.55 ns |    835.385 ns |    45.790 ns | 18.78 |    0.13 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,279.45 ns |     86.429 ns |     4.737 ns |  4.08 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   9,223.51 ns |    620.600 ns |    34.017 ns | 29.42 |    0.09 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **12,540.78 ns** |    **208.871 ns** |    **11.449 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 498,676.02 ns | 77,121.735 ns | 4,227.303 ns | 39.76 |    0.29 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  36,343.79 ns |  1,624.409 ns |    89.039 ns |  2.90 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 745,326.55 ns | 38,022.663 ns | 2,084.150 ns | 59.43 |    0.15 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

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

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **146.84 μs** |     **10.574 μs** |     **0.580 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 101,248.78 μs | 14,500.682 μs |   794.831 μs | 689.51 |    5.25 | 833.3333 | 833.3333 | 833.3333 |  31413.4 KB |   13,226.69 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **3,184.06 μs** |    **104.383 μs** |     **5.722 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.07 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 481,452.52 μs | 88,816.623 μs | 4,868.339 μs | 151.21 |    1.34 |        - |        - |        - | 32054.48 KB |      762.01 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **24.26 μs** |      **0.667 μs** |     **0.037 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     936.95 μs |    395.493 μs |    21.678 μs |  38.63 |    0.78 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      138.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method         | Samples | Classes | Mean           | Error           | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,329.6 ns** |       **255.41 ns** |      **14.00 ns** | **0.0153** |     **376 B** |
| MatrixWeighted | 1000    | 2       |     7,373.3 ns |       146.25 ns |       8.02 ns | 0.0153 |     376 B |
| AccuracyScore  | 1000    | 2       |       124.1 ns |        15.01 ns |       0.82 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,800.5 ns |       204.82 ns |      11.23 ns | 0.0305 |     536 B |
| Report         | 1000    | 2       |     7,321.7 ns |       166.30 ns |       9.12 ns | 0.3128 |    5344 B |
| **Matrix**         | **1000**    | **10**      |     **4,548.7 ns** |       **121.13 ns** |       **6.64 ns** | **0.0763** |    **1312 B** |
| MatrixWeighted | 1000    | 10      |     7,539.7 ns |        94.42 ns |       5.18 ns | 0.0763 |    1312 B |
| AccuracyScore  | 1000    | 10      |       123.1 ns |         0.70 ns |       0.04 ns |      - |         - |
| F1Macro        | 1000    | 10      |     5,077.5 ns |       246.81 ns |      13.53 ns | 0.0992 |    1728 B |
| Report         | 1000    | 10      |    11,280.8 ns |       799.07 ns |      43.80 ns | 0.7324 |   12336 B |
| **Matrix**         | **100000**  | **2**       |   **455,916.5 ns** |    **36,788.57 ns** |   **2,016.51 ns** |      **-** |     **376 B** |
| MatrixWeighted | 100000  | 2       |   829,761.5 ns |    66,490.67 ns |   3,644.58 ns |      - |     377 B |
| AccuracyScore  | 100000  | 2       |    12,581.0 ns |       213.91 ns |      11.73 ns |      - |         - |
| F1Macro        | 100000  | 2       |   476,629.8 ns |    10,264.54 ns |     562.63 ns |      - |     536 B |
| Report         | 100000  | 2       |   788,692.6 ns |   215,962.11 ns |  11,837.61 ns |      - |    5369 B |
| **Matrix**         | **100000**  | **10**      |   **494,706.0 ns** |    **27,855.33 ns** |   **1,526.84 ns** |      **-** |    **1313 B** |
| MatrixWeighted | 100000  | 10      |   964,801.1 ns |    41,453.34 ns |   2,272.20 ns |      - |    1313 B |
| AccuracyScore  | 100000  | 10      |    12,476.2 ns |        33.87 ns |       1.86 ns |      - |         - |
| F1Macro        | 100000  | 10      |   494,029.2 ns |    20,615.12 ns |   1,129.98 ns |      - |    1729 B |
| Report         | 100000  | 10      |   668,103.8 ns |    60,598.85 ns |   3,321.63 ns |      - |   12681 B |
| **Matrix**         | **1000000** | **2**       | **4,605,706.8 ns** |    **60,200.37 ns** |   **3,299.79 ns** |      **-** |     **382 B** |
| MatrixWeighted | 1000000 | 2       | 8,742,675.0 ns | 2,567,259.80 ns | 140,720.17 ns |      - |     388 B |
| AccuracyScore  | 1000000 | 2       |   129,316.6 ns |   130,996.40 ns |   7,180.35 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,622,313.0 ns |   375,346.98 ns |  20,574.03 ns |      - |     542 B |
| Report         | 1000000 | 2       | 4,758,348.2 ns |    80,078.39 ns |   4,389.37 ns |      - |    5390 B |
| **Matrix**         | **1000000** | **10**      | **5,049,928.6 ns** |   **109,322.84 ns** |   **5,992.35 ns** |      **-** |    **1318 B** |
| MatrixWeighted | 1000000 | 10      | 9,344,970.4 ns | 1,827,078.59 ns | 100,148.34 ns |      - |    1324 B |
| AccuracyScore  | 1000000 | 10      |   134,127.7 ns |     9,505.08 ns |     521.01 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 4,907,366.7 ns |    77,970.16 ns |   4,273.81 ns |      - |    1734 B |
| Report         | 1000000 | 10      | 4,906,796.9 ns |   199,902.39 ns |  10,957.32 ns |      - |   12726 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **7,693.74 μs** |    **213.862 μs** |    **11.723 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1080 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  33,996.24 μs |  2,498.682 μs |   136.961 μs |     4.42 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5089266 B |    4,712.28 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.63 μs** |      **1.404 μs** |     **0.077 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  33,665.59 μs |    527.956 μs |    28.939 μs | 2,895.04 |   16.69 | 600.0000 | 600.0000 | 600.0000 |  5089801 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        |  **97,177.55 μs** | **52,520.084 μs** | **2,878.803 μs** |     **1.00** |    **0.04** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 242,539.10 μs |  7,104.990 μs |   389.448 μs |     2.50 |    0.06 |        - |        - |        - | 23228104 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **108.94 μs** |      **4.830 μs** |     **0.265 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 221,202.93 μs | 13,708.464 μs |   751.407 μs | 2,030.45 |    7.34 |        - |        - |        - | 23232104 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method     | Band | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **82.14 ns** |     **1.681 ns** |  **0.092 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     81.32 ns |     1.850 ns |  0.101 ns |  0.99 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     81.63 ns |     1.858 ns |  0.102 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     80.78 ns |     4.943 ns |  0.271 ns |  0.98 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **6**    |    **117.50 ns** |    **29.234 ns** |  **1.602 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 6    |     92.62 ns |    46.090 ns |  2.526 ns |  0.79 |    0.02 |         - |          NA |
| Dp_Cjk     | 6    |    120.37 ns |     3.654 ns |  0.200 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    146.46 ns |     3.380 ns |  0.185 ns |  1.25 |    0.01 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **8**    |    **157.01 ns** |     **6.704 ns** |  **0.367 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    101.53 ns |    28.788 ns |  1.578 ns |  0.65 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    156.37 ns |    18.379 ns |  1.007 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    180.87 ns |    28.164 ns |  1.544 ns |  1.15 |    0.01 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **10**   |    **203.16 ns** |     **5.123 ns** |  **0.281 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    110.69 ns |     1.730 ns |  0.095 ns |  0.54 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    207.19 ns |     3.635 ns |  0.199 ns |  1.02 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    170.09 ns |     2.140 ns |  0.117 ns |  0.84 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **12**   |    **271.43 ns** |    **22.460 ns** |  **1.231 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    117.21 ns |     2.529 ns |  0.139 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    275.74 ns |   168.083 ns |  9.213 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |    179.89 ns |     4.945 ns |  0.271 ns |  0.66 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **16**   |    **429.92 ns** |    **14.618 ns** |  **0.801 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    141.74 ns |    10.730 ns |  0.588 ns |  0.33 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    446.10 ns |    91.251 ns |  5.002 ns |  1.04 |    0.01 |         - |          NA |
| Kernel_Cjk | 16   |    200.18 ns |     7.551 ns |  0.414 ns |  0.47 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **24**   |    **863.27 ns** |    **12.835 ns** |  **0.704 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    180.68 ns |    10.525 ns |  0.577 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    871.19 ns |   263.277 ns | 14.431 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    249.84 ns |    63.675 ns |  3.490 ns |  0.29 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **32**   |  **1,482.33 ns** |    **32.586 ns** |  **1.786 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    219.26 ns |     2.268 ns |  0.124 ns |  0.15 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,489.01 ns |    30.772 ns |  1.687 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    295.71 ns |     8.490 ns |  0.465 ns |  0.20 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **48**   |  **3,244.14 ns** |    **64.385 ns** |  **3.529 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    301.75 ns |    26.235 ns |  1.438 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,246.11 ns |    68.203 ns |  3.738 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    391.04 ns |    18.515 ns |  1.015 ns |  0.12 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **64**   |  **5,718.42 ns** | **1,120.558 ns** | **61.422 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    382.99 ns |    41.104 ns |  2.253 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,673.73 ns |   162.783 ns |  8.923 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    495.85 ns |    13.104 ns |  0.718 ns |  0.09 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **96**   | **12,551.27 ns** |   **557.082 ns** | **30.536 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    821.97 ns |   284.080 ns | 15.571 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,524.26 ns |   275.322 ns | 15.091 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   |  1,577.99 ns |    21.305 ns |  1.168 ns |  0.13 |    0.00 |         - |          NA |

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **388.98 μs** |  **46.179 μs** |  **2.531 μs** |  **1.00** |    **0.01** |      **-** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   455.54 μs |  34.241 μs |  1.877 μs |  1.17 |    0.01 |      - |    3680 B |        4.55 |
| SparseFit             | 10000    | 10         |    73.90 μs |   6.650 μs |  0.364 μs |  0.19 |    0.00 |      - |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 10         |   540.33 μs |  76.255 μs |  4.180 μs |  1.39 |    0.01 |      - |     785 B |        0.97 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **385.32 μs** |  **11.349 μs** |  **0.622 μs** |  **1.00** |    **0.00** |      **-** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   630.37 μs | 658.184 μs | 36.077 μs |  1.64 |    0.08 | 1.9531 |   36801 B |       45.55 |
| SparseFit             | 10000    | 100        |    73.00 μs |  11.385 μs |  0.624 μs |  0.19 |    0.00 |      - |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 100        |   535.20 μs |  18.079 μs |  0.991 μs |  1.39 |    0.00 |      - |     784 B |        0.97 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **3,848.33 μs** |  **90.184 μs** |  **4.943 μs** |  **1.00** |    **0.00** |      **-** |     **811 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 4,701.67 μs | 505.986 μs | 27.735 μs |  1.22 |    0.01 |      - |    3686 B |        4.55 |
| SparseFit             | 100000   | 10         | 1,873.07 μs | 120.028 μs |  6.579 μs |  0.49 |    0.00 |      - |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 10         | 5,362.20 μs | 461.484 μs | 25.296 μs |  1.39 |    0.01 |      - |     790 B |        0.97 |
|                       |          |            |             |            |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **3,878.30 μs** | **338.733 μs** | **18.567 μs** |  **1.00** |    **0.01** |      **-** |     **811 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 5,719.96 μs | 445.952 μs | 24.444 μs |  1.47 |    0.01 |      - |   36806 B |       45.38 |
| SparseFit             | 100000   | 100        | 1,874.33 μs |  30.194 μs |  1.655 μs |  0.48 |    0.00 |      - |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 100        | 5,343.02 μs | 308.777 μs | 16.925 μs |  1.38 |    0.01 |      - |     790 B |        0.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |    **10.841 μs** |  **3.6131 μs** | **0.1980 μs** |         **-** |
| EqualLength | 65           |     2.929 μs |  0.1979 μs | 0.0108 μs |         - |
| **Embedded**    | **128**          |    **39.520 μs** |  **0.4789 μs** | **0.0263 μs** |         **-** |
| EqualLength | 128          |     5.364 μs |  0.3784 μs | 0.0207 μs |         - |
| **Embedded**    | **512**          | **1,320.029 μs** | **73.4407 μs** | **4.0255 μs** |         **-** |
| EqualLength | 512          |    43.714 μs |  0.2630 μs | 0.0144 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  4.220 ms |  3.5739 ms | 0.1959 ms | 109.3750 | 101.5625 |  31.2500 |  3711.63 KB |
| TokenizerJsonWordPiece | 11.349 ms |  2.7838 ms | 0.1526 ms | 187.5000 | 171.8750 |  46.8750 |   5852.4 KB |
| TokenizerJsonUnigram   | 12.636 ms |  0.6209 ms | 0.0340 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |  3.924 ms |  2.3960 ms | 0.1313 ms | 109.3750 | 101.5625 |  31.2500 |  3440.05 KB |
| TfidfSave              |  1.647 ms |  0.1075 ms | 0.0059 ms |  21.4844 |  15.6250 |  15.6250 |  2137.04 KB |
| TfidfLoad              |  4.284 ms |  0.2308 ms | 0.0126 ms |  85.9375 |  78.1250 |  23.4375 |  2930.51 KB |
| EmbeddingIndexSave     |  4.188 ms |  0.6383 ms | 0.0350 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  5.320 ms |  0.4612 ms | 0.0253 ms | 179.6875 | 148.4375 | 117.1875 | 16094.37 KB |
| EmbeddingIndexSaveFile | 49.814 ms | 10.8134 ms | 0.5927 ms |        - |        - |        - |   323.49 KB |
| EmbeddingIndexLoadGzip | 73.928 ms | 51.1662 ms | 2.8046 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **9,433.49 μs** | **156.477 μs** |  **8.577 μs** |  **1.00** |    **0.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.28 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 15,308.67 μs | 227.555 μs | 12.473 μs |  1.62 |    0.00 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |            |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **130.51 μs** |  **16.065 μs** |  **0.881 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    182.06 μs |  10.040 μs |  0.550 μs |  1.40 |    0.01 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,788.57 μs** | **128.525 μs** |  **7.045 μs** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,586.14 μs |  84.009 μs |  4.605 μs |  0.93 |    0.00 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |         |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **21.93 μs** |   **5.153 μs** |  **0.282 μs** |  **1.00** |    **0.02** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     25.62 μs |   2.230 μs |  0.122 μs |  1.17 |    0.01 |   0.0916 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **42.28 μs** |  **15.929 μs** |  **0.873 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    42.39 μs |   5.462 μs |  0.299 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   110.65 μs |   4.118 μs |  0.226 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   538.63 μs | 227.934 μs | 12.494 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **420.34 μs** | **229.351 μs** | **12.572 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   420.52 μs |  56.057 μs |  3.073 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,100.27 μs |  34.056 μs |  1.867 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,908.81 μs | 482.879 μs | 26.468 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **56.191 ms** |  **1.9906 ms** | **0.1091 ms** |  **1.00** |  **444.4444** |         **-** |        **-** |   **8152.39 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |  11.405 ms |  1.5929 ms | 0.0873 ms |  0.20 |  171.8750 |  156.2500 |  78.1250 |   2492.48 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   8.684 ms |  0.5543 ms | 0.0304 ms |  0.15 |   15.6250 |         - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   8.537 ms |  4.7593 ms | 0.2609 ms |  0.15 |   15.6250 |         - |        - |    305.29 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  10.585 ms |  1.0054 ms | 0.0551 ms |  0.19 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **53.663 ms** |  **1.1057 ms** | **0.0606 ms** |  **1.00** |  **400.0000** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  15.875 ms |  1.5687 ms | 0.0860 ms |  0.30 |  312.5000 |  312.5000 | 218.7500 |   4538.24 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  10.824 ms |  1.1396 ms | 0.0625 ms |  0.20 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   8.507 ms |  0.9743 ms | 0.0534 ms |  0.16 |   15.6250 |         - |        - |    430.79 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  10.679 ms |  1.3093 ms | 0.0718 ms |  0.20 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **968.799 ms** | **37.5988 ms** | **2.0609 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  47.212 ms |  1.3953 ms | 0.0765 ms |  0.05 |  818.1818 |  818.1818 | 454.5455 |  10165.68 KB |       0.080 |
| SignaturesOnly       | 2000      | 64           |  41.236 ms |  2.7745 ms | 0.1521 ms |  0.04 |         - |         - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  27.189 ms |  0.9320 ms | 0.0511 ms |  0.03 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  42.427 ms |  0.4358 ms | 0.0239 ms |  0.04 |   83.3333 |         - |        - |   1718.81 KB |       0.014 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **971.936 ms** | **55.3088 ms** | **3.0317 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126359.81 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  66.607 ms | 30.4336 ms | 1.6682 ms |  0.07 | 1500.0000 | 1500.0000 | 750.0000 |  18495.56 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  44.358 ms |  4.4022 ms | 0.2413 ms |  0.05 |   83.3333 |         - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  33.928 ms |  0.9070 ms | 0.0497 ms |  0.03 |   66.6667 |         - |        - |   1719.88 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  42.513 ms |  1.3479 ms | 0.0739 ms |  0.04 |   83.3333 |         - |        - |   1718.81 KB |        0.01 |

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **4.768 ms** | **0.2163 ms** | **0.0119 ms** |  **1.00** | **109.3750** | **109.3750** | **109.3750** | **1366.79 KB** |        **1.00** |
| CountWithStopWords   | 200       |  4.300 ms | 0.2091 ms | 0.0115 ms |  0.90 |  39.0625 |  15.6250 |        - |  725.83 KB |        0.53 |
| Hashing              | 200       |  4.924 ms | 0.6309 ms | 0.0346 ms |  1.03 |  70.3125 |  70.3125 |  70.3125 | 1051.16 KB |        0.77 |
| HashingWithStopWords | 200       |  4.534 ms | 0.4949 ms | 0.0271 ms |  0.95 |  31.2500 |  15.6250 |        - |   591.9 KB |        0.43 |
|                      |           |           |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **15.742 ms** | **0.5885 ms** | **0.0323 ms** |  **1.00** | **843.7500** | **843.7500** | **843.7500** | **5880.52 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 14.059 ms | 2.0602 ms | 0.1129 ms |  0.89 | 375.0000 | 375.0000 | 375.0000 | 3155.02 KB |        0.54 |
| Hashing              | 1000      | 16.548 ms | 1.2923 ms | 0.0708 ms |  1.05 | 656.2500 | 562.5000 | 562.5000 | 4793.67 KB |        0.82 |
| HashingWithStopWords | 1000      | 15.363 ms | 0.4196 ms | 0.0230 ms |  0.98 | 265.6250 | 265.6250 | 265.6250 | 2633.78 KB |        0.45 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **20.69 ms** |  **0.878 ms** | **0.048 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.82 ms |  3.554 ms | 0.195 ms |  2.60 |    0.01 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **44.32 ms** |  **4.541 ms** | **0.249 ms** |  **1.00** |    **0.01** |  **333.3333** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  54.54 ms |  0.953 ms | 0.052 ms |  1.23 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **81.71 ms** | **27.770 ms** | **1.522 ms** |  **1.00** |    **0.02** | **1714.2857** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 237.16 ms |  1.392 ms | 0.076 ms |  2.90 |    0.05 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  | **131.70 ns** | **3.664 ns** | **0.201 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  47.67 ns | 0.361 ns | 0.020 ns |  0.36 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **95.17 ns** | **4.734 ns** | **0.259 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  92.48 ns | 0.652 ns | 0.036 ns |  0.97 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **124.20 ns** | **5.900 ns** | **0.323 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 122.67 ns | 1.125 ns | 0.062 ns |  0.99 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method                | Documents | Mean      | Error     | StdDev    | Ratio | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|---------------------- |---------- |----------:|----------:|----------:|------:|----------:|----------:|----------:|-----------:|------------:|
| **Count**                 | **200**       |  **2.312 ms** | **0.3034 ms** | **0.0166 ms** |  **1.00** |   **15.6250** |   **11.7188** |         **-** |     **303 KB** |        **1.00** |
| Tfidf                 | 200       |  2.342 ms | 0.1644 ms | 0.0090 ms |  1.01 |   19.5313 |   15.6250 |         - |  332.14 KB |        1.10 |
| CountBigrams          | 200       |  2.760 ms | 0.2236 ms | 0.0123 ms |  1.19 |   31.2500 |   15.6250 |         - |  590.34 KB |        1.95 |
| CountCharWordBoundary | 200       |  2.122 ms | 0.0498 ms | 0.0027 ms |  0.92 |  382.8125 |  382.8125 |  382.8125 |  1685.7 KB |        5.56 |
| Hashing               | 200       |  2.323 ms | 0.1848 ms | 0.0101 ms |  1.01 |   11.7188 |    7.8125 |         - |  233.65 KB |        0.77 |
|                       |           |           |           |           |       |           |           |           |            |             |
| **Count**                 | **1000**      |  **4.444 ms** | **0.1809 ms** | **0.0099 ms** |  **1.00** |  **109.3750** |  **109.3750** |  **109.3750** | **1280.03 KB** |        **1.00** |
| Tfidf                 | 1000      |  4.575 ms | 0.1523 ms | 0.0084 ms |  1.03 |  140.6250 |  140.6250 |  140.6250 | 1424.43 KB |        1.11 |
| CountBigrams          | 1000      |  6.402 ms | 0.3072 ms | 0.0168 ms |  1.44 |  390.6250 |  390.6250 |  390.6250 |  2291.9 KB |        1.79 |
| CountCharWordBoundary | 1000      | 12.907 ms | 0.6044 ms | 0.0331 ms |  2.90 | 1046.8750 | 1015.6250 | 1015.6250 | 12022.2 KB |        9.39 |
| Hashing               | 1000      |  4.475 ms | 0.3772 ms | 0.0207 ms |  1.01 |   70.3125 |   70.3125 |   70.3125 | 1015.48 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **5.124 ms** |   **0.5411 ms** |  **0.0297 ms** |  **1.00** |    **0.01** |   **148.4375** |   **148.4375** |   **148.4375** |   **2.01 MB** |        **1.00** |
| MlNet    | 200       |  51.242 ms | 108.1049 ms |  5.9256 ms | 10.00 |    1.00 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       14.09 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **18.880 ms** |   **3.0978 ms** |  **0.1698 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **9.17 MB** |        **1.00** |
| MlNet    | 1000      | 405.115 ms | 771.1246 ms | 42.2679 ms | 21.46 |    1.95 | 76000.0000 | 76000.0000 | 76000.0000 | 324.35 MB |       35.36 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.157 | 0.036 | 0.23x | 0.157 | 0.036 | 0.23x |
| cdist_wratio_n50 | 2.227 | 2.504 | 1.12x | 2.226 | 2.504 | 1.12x |
| cdist_ratio_varied_n50 | 0.130 | 0.031 | 0.24x | 0.130 | 0.031 | 0.24x |
| cdist_ratio_varied_cutoff90_n50 | 0.054 | 0.031 | 0.58x | 0.054 | 0.031 | 0.58x |
| cdist_ratio_n200 | 2.706 | 0.440 | 0.16x | 2.709 | 0.440 | 0.16x |
| cdist_wratio_n200 | 33.918 | 37.935 | 1.12x | 33.919 | 37.933 | 1.12x |
| cdist_ratio_varied_n200 | 2.297 | 0.385 | 0.17x | 2.300 | 0.385 | 0.17x |
| cdist_ratio_varied_cutoff90_n200 | 0.981 | 0.385 | 0.39x | 0.984 | 0.385 | 0.39x |
| cdist_ratio_n500 | 16.390 | 2.655 | 0.16x | 16.407 | 2.655 | 0.16x |
| cdist_wratio_n500 | 207.518 | 229.402 | 1.11x | 207.648 | 229.388 | 1.10x |
| cdist_ratio_varied_n500 | 13.597 | 2.294 | 0.17x | 13.707 | 2.294 | 0.17x |
| cdist_ratio_varied_cutoff90_n500 | 5.501 | 2.294 | 0.42x | 5.600 | 2.294 | 0.41x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.398 | 3.348 | 8.41x | 0.398 | 3.348 | 8.41x |
| glm_gamma_n1000 | 0.489 | 4.158 | 8.51x | 0.488 | 4.158 | 8.51x |
| glm_poisson_exposure_n1000 | 0.397 | 3.926 | 9.88x | 0.397 | 3.926 | 9.88x |
| mnlogit_n1000 | 0.842 | 18.061 | 21.45x | 0.842 | 18.059 | 21.45x |
| glm_negative_binomial_n10000 | 3.604 | 13.770 | 3.82x | 3.611 | 13.769 | 3.81x |
| glm_gamma_n10000 | 4.144 | 12.954 | 3.13x | 4.153 | 12.953 | 3.12x |
| glm_poisson_exposure_n10000 | 4.333 | 12.971 | 2.99x | 4.340 | 12.969 | 2.99x |
| mnlogit_n10000 | 8.648 | 96.620 | 11.17x | 8.656 | 96.613 | 11.16x |
| glm_negative_binomial_n100000 | 37.412 | 119.356 | 3.19x | 37.705 | 473.135 | 12.55x |
| glm_gamma_n100000 | 43.372 | 134.259 | 3.10x | 43.629 | 535.959 | 12.28x |
| glm_poisson_exposure_n100000 | 55.859 | 131.451 | 2.35x | 56.263 | 524.135 | 9.32x |
| mnlogit_n100000 | 86.685 | 922.316 | 10.64x | 87.131 | 1654.580 | 18.99x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 110.6 | 23.8 | 4.65x C# faster |
| latin | 32 | 161.3 | 68.5 | 2.36x C# faster |
| latin | 128 | 480.5 | 400.7 | 1.20x C# faster |
| latin | 512 | 4858.5 | 5388.7 | 1.11x Py faster |
| cjk | 8 | 128.0 | 23.7 | 5.40x C# faster |
| cjk | 32 | 236.3 | 145.5 | 1.62x C# faster |
| cjk | 128 | 2036.9 | 1371.9 | 1.48x C# faster |
| cjk | 512 | 16840.6 | 9166.2 | 1.84x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 133.2 | 18.1 | 7.35x C# faster |
| latin | 32 | 249.6 | 126.7 | 1.97x C# faster |
| latin | 128 | 1816.7 | 923.3 | 1.97x C# faster |
| latin | 512 | 15550.2 | 13035.1 | 1.19x C# faster |
| cjk | 8 | 144.3 | 18.2 | 7.93x C# faster |
| cjk | 32 | 294.5 | 190.1 | 1.55x C# faster |
| cjk | 128 | 3029.4 | 2145.4 | 1.41x C# faster |
| cjk | 512 | 26220.3 | 18956.4 | 1.38x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.006 | 0.976 | 153.35x | 0.006 | 0.976 | 153.35x |
| accuracy_n1000_k2 | 0.000 | 0.514 | 4174.53x | 0.000 | 0.514 | 4173.52x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.771 | 367.95x | 0.005 | 1.771 | 367.91x |
| classification_report_n1000_k2 | 0.007 | 6.829 | 951.15x | 0.007 | 6.828 | 951.14x |
| roc_auc_binary_n1000_k2 | 0.016 | 1.894 | 122.05x | 0.016 | 1.894 | 122.05x |
| balanced_accuracy_n1000_k2 | 0.005 | 1.036 | 211.88x | 0.005 | 1.036 | 211.87x |
| matthews_n1000_k2 | 0.005 | 1.985 | 412.84x | 0.005 | 1.984 | 412.83x |
| cohen_kappa_n1000_k2 | 0.005 | 1.081 | 234.26x | 0.005 | 1.081 | 234.26x |
| mse_n1000_k2 | 0.000 | 0.300 | 615.51x | 0.000 | 0.300 | 615.53x |
| mae_n1000_k2 | 0.000 | 0.298 | 615.30x | 0.000 | 0.298 | 615.24x |
| median_ae_n1000_k2 | 0.005 | 0.312 | 64.76x | 0.005 | 0.311 | 64.75x |
| r2_n1000_k2 | 0.001 | 0.362 | 304.05x | 0.001 | 0.362 | 304.02x |
| confusion_matrix_n1000_k10 | 0.007 | 0.976 | 145.10x | 0.007 | 0.976 | 145.10x |
| accuracy_n1000_k10 | 0.000 | 0.516 | 4278.85x | 0.000 | 0.516 | 4278.93x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.793 | 319.68x | 0.006 | 1.793 | 319.68x |
| classification_report_n1000_k10 | 0.011 | 6.943 | 617.50x | 0.011 | 6.943 | 617.45x |
| roc_auc_ovr_macro_n1000_k10 | 0.523 | 9.824 | 18.80x | 0.523 | 9.824 | 18.80x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.046 | 180.91x | 0.006 | 1.046 | 180.89x |
| matthews_n1000_k10 | 0.006 | 2.016 | 357.24x | 0.006 | 2.016 | 357.26x |
| cohen_kappa_n1000_k10 | 0.006 | 1.093 | 178.89x | 0.006 | 1.093 | 178.86x |
| mse_n1000_k10 | 0.000 | 0.300 | 611.84x | 0.000 | 0.300 | 611.79x |
| mae_n1000_k10 | 0.000 | 0.301 | 617.73x | 0.000 | 0.301 | 617.72x |
| median_ae_n1000_k10 | 0.005 | 0.312 | 64.79x | 0.005 | 0.312 | 64.79x |
| r2_n1000_k10 | 0.001 | 0.363 | 304.37x | 0.001 | 0.363 | 304.36x |
| confusion_matrix_n100000_k2 | 0.626 | 10.678 | 17.06x | 0.626 | 10.677 | 17.06x |
| accuracy_n100000_k2 | 0.011 | 3.733 | 334.76x | 0.011 | 3.733 | 334.75x |
| precision_recall_f1_macro_n100000_k2 | 0.475 | 12.259 | 25.80x | 0.475 | 12.258 | 25.81x |
| classification_report_n100000_k2 | 0.478 | 26.935 | 56.34x | 0.478 | 26.933 | 56.34x |
| roc_auc_binary_n100000_k2 | 3.473 | 26.341 | 7.58x | 3.473 | 26.336 | 7.58x |
| balanced_accuracy_n100000_k2 | 0.475 | 10.771 | 22.69x | 0.475 | 10.770 | 22.69x |
| matthews_n100000_k2 | 0.475 | 21.466 | 45.20x | 0.475 | 21.465 | 45.20x |
| cohen_kappa_n100000_k2 | 0.476 | 10.808 | 22.73x | 0.475 | 10.807 | 22.73x |
| mse_n100000_k2 | 0.045 | 0.461 | 10.19x | 0.045 | 0.461 | 10.19x |
| mae_n100000_k2 | 0.045 | 0.453 | 10.11x | 0.045 | 0.453 | 10.11x |
| median_ae_n100000_k2 | 0.571 | 1.788 | 3.13x | 0.588 | 1.788 | 3.04x |
| r2_n100000_k2 | 0.110 | 0.658 | 5.97x | 0.110 | 0.658 | 5.97x |
| confusion_matrix_n100000_k10 | 0.508 | 10.675 | 21.03x | 0.508 | 10.673 | 21.03x |
| accuracy_n100000_k10 | 0.012 | 3.744 | 315.79x | 0.012 | 3.744 | 315.74x |
| precision_recall_f1_macro_n100000_k10 | 0.508 | 12.899 | 25.39x | 0.508 | 12.898 | 25.39x |
| classification_report_n100000_k10 | 0.514 | 29.469 | 57.30x | 0.514 | 29.467 | 57.30x |
| roc_auc_ovr_macro_n100000_k10 | 34.820 | 211.441 | 6.07x | 34.824 | 211.424 | 6.07x |
| balanced_accuracy_n100000_k10 | 0.508 | 10.749 | 21.17x | 0.508 | 10.749 | 21.17x |
| matthews_n100000_k10 | 0.508 | 22.127 | 43.60x | 0.507 | 22.125 | 43.60x |
| cohen_kappa_n100000_k10 | 0.708 | 10.784 | 15.23x | 0.708 | 10.783 | 15.23x |
| mse_n100000_k10 | 0.045 | 0.438 | 9.69x | 0.045 | 0.438 | 9.69x |
| mae_n100000_k10 | 0.045 | 0.429 | 9.55x | 0.045 | 0.429 | 9.55x |
| median_ae_n100000_k10 | 0.644 | 1.778 | 2.76x | 0.699 | 1.778 | 2.54x |
| r2_n100000_k10 | 0.111 | 0.646 | 5.81x | 0.111 | 0.646 | 5.81x |
| confusion_matrix_n1000000_k2 | 4.847 | 99.046 | 20.43x | 4.845 | 99.040 | 20.44x |
| accuracy_n1000000_k2 | 0.118 | 32.679 | 277.09x | 0.118 | 32.677 | 277.11x |
| precision_recall_f1_macro_n1000000_k2 | 4.773 | 107.469 | 22.52x | 4.773 | 107.455 | 22.51x |
| classification_report_n1000000_k2 | 4.926 | 209.219 | 42.47x | 4.925 | 209.185 | 42.47x |
| roc_auc_binary_n1000000_k2 | 67.988 | 288.337 | 4.24x | 67.988 | 288.312 | 4.24x |
| balanced_accuracy_n1000000_k2 | 4.876 | 99.072 | 20.32x | 4.875 | 99.047 | 20.32x |
| matthews_n1000000_k2 | 4.818 | 200.454 | 41.60x | 4.818 | 200.420 | 41.60x |
| cohen_kappa_n1000000_k2 | 4.922 | 99.206 | 20.16x | 4.921 | 99.196 | 20.16x |
| mse_n1000000_k2 | 0.496 | 2.110 | 4.25x | 0.496 | 2.109 | 4.26x |
| mae_n1000000_k2 | 0.524 | 2.083 | 3.98x | 0.524 | 2.083 | 3.98x |
| median_ae_n1000000_k2 | 5.611 | 14.060 | 2.51x | 5.668 | 14.058 | 2.48x |
| r2_n1000000_k2 | 1.102 | 3.508 | 3.18x | 1.102 | 3.507 | 3.18x |
| confusion_matrix_n1000000_k10 | 5.055 | 99.231 | 19.63x | 5.055 | 99.179 | 19.62x |
| accuracy_n1000000_k10 | 0.123 | 32.815 | 266.47x | 0.123 | 32.799 | 266.39x |
| precision_recall_f1_macro_n1000000_k10 | 5.153 | 113.468 | 22.02x | 5.153 | 113.455 | 22.02x |
| classification_report_n1000000_k10 | 5.130 | 233.791 | 45.57x | 5.129 | 233.764 | 45.58x |
| balanced_accuracy_n1000000_k10 | 5.058 | 99.363 | 19.64x | 5.057 | 99.355 | 19.65x |
| matthews_n1000000_k10 | 5.136 | 207.280 | 40.35x | 5.136 | 207.258 | 40.36x |
| cohen_kappa_n1000000_k10 | 5.151 | 99.451 | 19.31x | 5.150 | 99.436 | 19.31x |
| mse_n1000000_k10 | 0.490 | 2.222 | 4.53x | 0.490 | 2.221 | 4.53x |
| mae_n1000000_k10 | 0.493 | 2.199 | 4.46x | 0.493 | 2.199 | 4.46x |
| median_ae_n1000000_k10 | 5.408 | 14.131 | 2.61x | 5.449 | 14.129 | 2.59x |
| r2_n1000000_k10 | 1.268 | 3.922 | 3.09x | 1.268 | 3.921 | 3.09x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.243 | 0.851 | 0.38x | 2.250 | 0.851 | 0.38x |
| transform_frobenius_n500 | 6.799 | 1.636 | 0.24x | 6.799 | 1.635 | 0.24x |
| transform_frobenius_n2000 | 24.095 | 5.824 | 0.24x | 24.131 | 23.241 | 0.96x |
| transform_kl_n100 | 1.254 | 17.613 | 14.05x | 1.253 | 17.611 | 14.05x |
| transform_kl_n500 | 5.701 | 34.224 | 6.00x | 5.701 | 34.222 | 6.00x |
| transform_kl_n2000 | 22.597 | 122.088 | 5.40x | 22.647 | 450.930 | 19.91x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

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

### compare-persistence

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.269 | 10.564 | 2.00x | 5.583 | 10.563 | 1.89x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 13.430 | 29.467 | 2.19x | 13.818 | 29.465 | 2.13x | 706,526 | 706,526 |
| tokenizer_json_unigram | 15.691 | 66.829 | 4.26x | 16.039 | 66.819 | 4.17x | 1,990,038 | 1,990,038 |
| spiece_model | 5.344 | 36.761 | 6.88x | 5.590 | 36.758 | 6.58x | 533,084 | 533,084 |
| tfidf_save | 1.964 | 3.203 | 1.63x | 1.992 | 3.202 | 1.61x | 581,787 | 591,922 |
| tfidf_load | 4.977 | 5.114 | 1.03x | 5.296 | 5.113 | 0.97x | 581,787 | 591,922 |
| embedding_index_save | 4.431 | 3.865 | 0.87x | 4.669 | 3.865 | 0.83x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.465 | 36.712 | 0.74x | 10.157 | 6.334 | 0.62x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.968 | 1.542 | 0.26x | 6.305 | 1.542 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.436 | 1.100 | 0.17x | 6.796 | 1.096 | 0.16x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.799 | 1.590 | 0.33x | 5.201 | 1.589 | 0.31x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.565 | 1.487 | 0.95x | 1.869 | 1.484 | 0.79x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 104.15x | 0.000 | 0.001 | 104.14x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 414.304 | 557.609 | 1.35x | 414.287 | 557.514 | 1.35x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 71.110 | 66.386 | 0.93x | 71.492 | 66.383 | 0.93x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.082 | 0.107 | 1.31x | 0.082 | 0.107 | 1.31x |
| stratified_n10000 | 0.215 | 0.784 | 3.64x | 0.215 | 0.784 | 3.64x |
| traintest_n10000 | 0.005 | 0.174 | 37.79x | 0.005 | 0.174 | 37.80x |
| kfold_n100000 | 1.112 | 2.991 | 2.69x | 1.249 | 2.991 | 2.40x |
| stratified_n100000 | 2.180 | 8.243 | 3.78x | 2.317 | 8.242 | 3.56x |
| traintest_n100000 | 0.207 | 0.381 | 1.84x | 0.208 | 0.381 | 1.83x |
| kfold_n1000000 | 9.925 | 19.010 | 1.92x | 10.495 | 19.008 | 1.81x |
| stratified_n1000000 | 23.112 | 63.712 | 2.76x | 23.859 | 63.706 | 2.67x |
| traintest_n1000000 | 0.740 | 2.197 | 2.97x | 0.863 | 2.197 | 2.55x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.794 | 197.62x | 0.004 | 0.793 | 197.61x |
| mann_whitney_n1000 | 0.029 | 0.735 | 25.36x | 0.029 | 0.735 | 25.35x |
| chi_square_n1000 | 0.000 | 0.335 | 1599.72x | 0.000 | 0.335 | 1599.68x |
| welch_t_n10000 | 0.038 | 0.850 | 22.61x | 0.038 | 0.850 | 22.61x |
| mann_whitney_n10000 | 0.756 | 2.805 | 3.71x | 0.756 | 2.805 | 3.71x |
| chi_square_n10000 | 0.001 | 0.335 | 256.69x | 0.001 | 0.335 | 256.71x |
| welch_t_n100000 | 0.375 | 1.224 | 3.26x | 0.375 | 1.224 | 3.26x |
| mann_whitney_n100000 | 8.045 | 27.818 | 3.46x | 8.045 | 27.815 | 3.46x |
| chi_square_n100000 | 0.014 | 0.358 | 26.05x | 0.014 | 0.358 | 26.05x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-transformers

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| normalize_n10000 | 0.281 | 0.394 | 1.40x | 0.283 | 0.394 | 1.39x |
| polynomial_n10000 | 0.819 | 0.903 | 1.10x | 0.822 | 0.902 | 1.10x |
| kbins_fit_n10000 | 1.645 | 2.291 | 1.39x | 1.645 | 2.290 | 1.39x |
| kbins_n10000 | 0.410 | 4.410 | 10.75x | 0.516 | 4.410 | 8.55x |
| quantile_fit_n10000 | 1.768 | 3.952 | 2.24x | 1.767 | 3.952 | 2.24x |
| quantile_n10000 | 1.865 | 1.745 | 0.94x | 1.867 | 1.745 | 0.93x |
| power_fit_n10000 | 24.985 | 125.939 | 5.04x | 24.989 | 125.930 | 5.04x |
| power_n10000 | 0.972 | 1.254 | 1.29x | 0.975 | 1.254 | 1.29x |
| knn_impute_n10000 | 48.511 | 23.130 | 0.48x | 48.579 | 92.233 | 1.90x |
| label_n10000 | 1.177 | 0.789 | 0.67x | 1.177 | 0.789 | 0.67x |
| normalize_n100000 | 1.523 | 1.847 | 1.21x | 1.936 | 1.847 | 0.95x |
| polynomial_n100000 | 3.915 | 6.477 | 1.65x | 4.635 | 6.477 | 1.40x |
| kbins_fit_n100000 | 16.504 | 8.304 | 0.50x | 17.348 | 8.304 | 0.48x |
| kbins_n100000 | 3.241 | 11.819 | 3.65x | 3.961 | 11.815 | 2.98x |
| quantile_fit_n100000 | 16.658 | 19.610 | 1.18x | 17.407 | 19.609 | 1.13x |
| quantile_n100000 | 17.088 | 15.708 | 0.92x | 17.502 | 15.706 | 0.90x |
| power_fit_n100000 | 260.571 | 910.107 | 3.49x | 277.526 | 909.996 | 3.28x |
| power_n100000 | 8.357 | 8.718 | 1.04x | 8.781 | 8.717 | 0.99x |
| knn_impute_n100000 | 51.279 | 18.768 | 0.37x | 51.271 | 74.962 | 1.46x |
| label_n100000 | 15.539 | 7.959 | 0.51x | 15.879 | 7.959 | 0.50x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-09-25, measured at commit `eba6ece0df93976b598aa7f203ae077e6acda030`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.039 | 2.677 | 69.51x | 0.039 | 2.677 | 69.51x |
| var_n10000 | 0.470 | 16.511 | 35.11x | 0.473 | 16.510 | 34.88x |
| var_n100000 | 3.825 | 196.483 | 51.37x | 3.991 | 420.500 | 105.36x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
