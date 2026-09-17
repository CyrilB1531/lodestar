# Latest known benchmark result, per method

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml` from the
> wiki's own history, alongside [nightly_run](nightly_run).

**Not a comparison across methods.** Each section below is the last night that method was
actually re-run -- whichever night touched the source near it, not necessarily last night,
and not the same night as its neighbours here. Every run measures on a GitHub hosted
runner whose hardware differs night to night, so a number here says "this is the last
known reading", never "faster than the section above it".

## Per method

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|----------:|------:|----------:|------------:|
| ChiSquaredSfOneDf          | 23.313 ns | 0.5249 ns | 0.0288 ns |  1.00 |         - |          NA |
| ChiSquaredSfFourDf         |  9.591 ns | 5.2803 ns | 0.2894 ns |  0.41 |         - |          NA |
| ChiSquaredSfThreeDfFarTail | 27.748 ns | 0.4451 ns | 0.0244 ns |  1.19 |         - |          NA |
| ChiSquaredSfHundredDf      | 59.706 ns | 0.6958 ns | 0.0381 ns |  2.56 |         - |          NA |
| ChiSquaredSfFractionalDf   | 93.899 ns | 2.6506 ns | 0.1453 ns |  4.03 |         - |          NA |
| NormalQuantile             | 90.780 ns | 0.9677 ns | 0.0530 ns |  3.89 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |--------------:|--------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Gls** | **50**         |      **23.34 μs** |      **0.350 μs** |     **0.019 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |   **27.04 KB** |        **1.00** |
| MathNet_Gls  | 50         |      45.87 μs |     15.108 μs |     0.828 μs |  1.97 |    0.03 |   2.0142 |   0.1221 |        - |   33.28 KB |        1.23 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **483.80 μs** |     **22.891 μs** |     **1.255 μs** |  **1.00** |    **0.00** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.51 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,408.29 μs |    110.672 μs |     6.066 μs |  2.91 |    0.01 |  99.6094 |  99.6094 |  99.6094 |  370.31 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **4,596.94 μs** |    **450.726 μs** |    **24.706 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.15 KB** |        **1.00** |
| MathNet_Gls  | 500        |  17,631.82 μs |    342.376 μs |    18.767 μs |  3.84 |    0.02 | 281.2500 | 281.2500 | 281.2500 | 2229.12 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **38,751.36 μs** |  **4,492.231 μs** |   **246.234 μs** |  **1.00** |    **0.01** | **214.2857** | **214.2857** | **214.2857** | **7924.18 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 114,292.17 μs | 68,704.286 μs | 3,765.914 μs |  2.95 |    0.09 |        - |        - |        - | 8852.23 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **25.46 μs** |  **0.822 μs** | **0.045 μs** |  **1.00** | **0.9460** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    27.39 μs |  4.010 μs | 0.220 μs |  1.08 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,685.97 μs** | **59.526 μs** | **3.263 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,280.04 μs | 35.673 μs | 1.955 μs |  0.76 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **664.73 ns** |     **153.408 ns** |      **8.409 ns** |   **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,921.35 ns |     177.514 ns |      9.730 ns |   2.89 |    0.03 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,030.60 ns |     115.261 ns |      6.318 ns |   3.06 |    0.03 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,011.70 ns |     708.820 ns |     38.853 ns |   4.53 |    0.07 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     5,478.24 ns |     169.636 ns |      9.298 ns |   8.24 |    0.09 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     7,292.68 ns |     130.862 ns |      7.173 ns |  10.97 |    0.12 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     1,954.74 ns |      74.853 ns |      4.103 ns |   2.94 |    0.03 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,031.77 ns |     312.223 ns |     17.114 ns |   4.56 |    0.05 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       791.43 ns |       7.892 ns |      0.433 ns |   1.19 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,732.84 ns |      75.085 ns |      4.116 ns |   4.11 |    0.05 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       810.20 ns |       4.928 ns |      0.270 ns |   1.22 |    0.01 | 0.0486 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |       811.15 ns |      23.686 ns |      1.298 ns |   1.22 |    0.01 | 0.0582 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       343.61 ns |      81.592 ns |      4.472 ns |   0.52 |    0.01 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,159.77 ns |      35.316 ns |      1.936 ns |   1.74 |    0.02 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        85.26 ns |       6.123 ns |      0.336 ns |   0.13 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       515.40 ns |      12.492 ns |      0.685 ns |   0.78 |    0.01 | 0.0572 |     968 B |          NA |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **32,903.67 ns** |   **1,189.289 ns** |     **65.189 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   159,129.47 ns |  25,629.550 ns |  1,404.842 ns |  4.836 |    0.04 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   351,724.58 ns |   6,792.004 ns |    372.293 ns | 10.690 |    0.02 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,836,256.16 ns |   2,624.805 ns |    143.874 ns | 55.807 |    0.10 | 3.9063 |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      |   919,716.19 ns |  46,157.038 ns |  2,530.023 ns | 27.952 |    0.08 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,170,404.05 ns | 300,496.478 ns | 16,471.226 ns | 96.354 |    0.46 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,288,772.11 ns |  52,486.157 ns |  2,876.943 ns | 39.168 |    0.10 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,945,408.99 ns |  88,271.690 ns |  4,838.469 ns | 59.125 |    0.16 | 3.9063 |   80374 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    73,793.54 ns |     426.519 ns |     23.379 ns |  2.243 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   238,234.97 ns |   7,035.530 ns |    385.641 ns |  7.240 |    0.02 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   138,595.09 ns |   7,094.744 ns |    388.887 ns |  4.212 |    0.01 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   165,385.85 ns |  27,751.576 ns |  1,521.158 ns |  5.026 |    0.04 | 4.6387 |   80176 B |          NA |
| Lodestar_FisherExact           | 10000      |       341.09 ns |       2.159 ns |      0.118 ns |  0.010 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,165.57 ns |      28.821 ns |      1.580 ns |  0.035 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        85.02 ns |       1.369 ns |      0.075 ns |  0.003 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       518.94 ns |      10.920 ns |      0.599 ns |  0.016 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean         | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------- |----------- |-------------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |     **5.395 μs** |   **0.1936 μs** | **0.0106 μs** |  **1.00** |    **0.00** |   **0.1526** |        **-** |        **-** |    **2.59 KB** |        **1.00** |
| Accord_Ols   | 100        |   190.165 μs |  27.3211 μs | 1.4976 μs | 35.25 |    0.25 |   2.4414 |        - |        - |      43 KB |       16.63 |
| MathNet_Ols  | 100        |    24.043 μs |   3.9672 μs | 0.2175 μs |  4.46 |    0.04 |   2.1973 |   0.0610 |        - |   35.65 KB |       13.79 |
|              |            |              |             |           |       |         |          |          |          |            |             |
| **Lodestar_Ols** | **10000**      |   **366.681 μs** |  **63.5662 μs** | **3.4843 μs** |  **1.00** |    **0.01** |   **4.3945** |   **0.9766** |        **-** |   **79.93 KB** |        **1.00** |
| Accord_Ols   | 10000      | 3,551.872 μs | 113.4177 μs | 6.2168 μs |  9.69 |    0.08 | 214.8438 | 195.3125 |        - | 3523.47 KB |       44.08 |
| MathNet_Ols  | 10000      | 1,362.138 μs |  55.9955 μs | 3.0693 μs |  3.71 |    0.03 | 443.3594 | 443.3594 | 443.3594 | 1766.36 KB |       22.10 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                       | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|---------:|------:|----------:|------------:|
| NormalQuantile               |  69.87 ns |  2.338 ns | 0.128 ns |  1.00 |         - |          NA |
| NormalQuantileFarTail        |  69.75 ns |  0.954 ns | 0.052 ns |  1.00 |         - |          NA |
| StudentQuantile              | 525.20 ns |  4.141 ns | 0.227 ns |  7.52 |         - |          NA |
| StudentQuantileCauchyFarTail | 163.57 ns | 18.614 ns | 1.020 ns |  2.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |    **936.9 μs** |    **13.89 μs** |   **0.76 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | False |    191.2 μs |    11.11 μs |   0.61 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | False |    613.6 μs |    24.23 μs |   1.33 μs |  16.6016 |        - |        - |   279681 B |
| WilcoxonPooledDifferences | 10000      | False |    728.3 μs |   141.03 μs |   7.73 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    356.8 μs |    29.16 μs |   1.60 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **432.2 μs** |     **2.21 μs** |   **0.12 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    162.9 μs |     8.15 μs |   0.45 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    130.6 μs |    43.87 μs |   2.40 μs |  16.6016 |        - |        - |   279680 B |
| WilcoxonPooledDifferences | 10000      | True  |    496.5 μs |   176.34 μs |   9.67 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    284.8 μs |   126.68 μs |   6.94 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** |  **9,494.9 μs** |   **443.35 μs** |  **24.30 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  2,753.2 μs | 1,237.39 μs |  67.83 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  8,392.6 μs | 2,334.80 μs | 127.98 μs | 312.5000 | 312.5000 | 312.5000 |  2800338 B |
| WilcoxonPooledDifferences | 100000     | False | 30,470.5 μs |   818.94 μs |  44.89 μs | 968.7500 | 968.7500 | 968.7500 | 13200772 B |
| MannWhitneyTest           | 100000     | False |  4,074.6 μs |   117.16 μs |   6.42 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **4,273.4 μs** | **1,493.67 μs** |  **81.87 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,067.5 μs |   137.14 μs |   7.52 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  4,971.9 μs |    51.98 μs |   2.85 μs | 328.1250 | 328.1250 | 328.1250 |  2800343 B |
| WilcoxonPooledDifferences | 100000     | True  | 18,437.7 μs |   706.48 μs |  38.72 μs | 968.7500 | 968.7500 | 968.7500 | 13076300 B |
| MannWhitneyTest           | 100000     | True  |  2,779.7 μs |    23.15 μs |   1.27 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **LodestarAutocorrelation**        | **200**        |   **3.734 μs** | **0.5641 μs** | **0.0309 μs** |  **1.00** |    **0.01** | **0.1564** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  10.091 μs | 0.5755 μs | 0.0315 μs |  2.70 |    0.02 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.174 μs | 0.5685 μs | 0.0312 μs |  1.12 |    0.01 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  10.600 μs | 0.4683 μs | 0.0257 μs |  2.84 |    0.02 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.131 μs | 0.4839 μs | 0.0265 μs |  1.11 |    0.01 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  10.192 μs | 0.4006 μs | 0.0220 μs |  2.73 |    0.02 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **42.466 μs** | **1.7115 μs** | **0.0938 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 105.874 μs | 2.0539 μs | 0.1126 μs |  2.49 |    0.01 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  43.083 μs | 0.8922 μs | 0.0489 μs |  1.01 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 106.501 μs | 4.2941 μs | 0.2354 μs |  2.51 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  43.226 μs | 5.5908 μs | 0.3064 μs |  1.02 |    0.01 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 105.997 μs | 2.5849 μs | 0.1417 μs |  2.50 |    0.01 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------ |----------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **4.365 μs** |   **0.2115 μs** | **0.0116 μs** |  **1.00** |    **0.00** |   **1.2665** |   **0.0458** |        **-** |  **20.77 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  19.516 μs |   0.3954 μs | 0.0217 μs |  4.47 |    0.01 |   3.5706 |        - |        - |  58.88 KB |        2.84 |
| CortexAugmentedDickeyFuller   | 200        |  21.987 μs |   8.7468 μs | 0.4794 μs |  5.04 |    0.10 |   0.9766 |   0.0305 |        - |  16.18 KB |        0.78 |
| LodestarKpss                  | 200        |   3.079 μs |   0.0527 μs | 0.0029 μs |  0.71 |    0.00 |   0.1030 |        - |        - |   1.69 KB |        0.08 |
| CortexKpss                    | 200        |   3.198 μs |   0.0289 μs | 0.0016 μs |  0.73 |    0.00 |   0.2098 |        - |        - |   3.43 KB |        0.17 |
| LodestarDecompose             | 200        |   1.099 μs |   0.0700 μs | 0.0038 μs |  0.25 |    0.00 |   0.3967 |   0.0038 |        - |    6.5 KB |        0.31 |
| CortexDecompose               | 200        |   2.058 μs |   0.0948 μs | 0.0052 μs |  0.47 |    0.00 |   0.4997 |   0.0076 |        - |   8.16 KB |        0.39 |
|                               |            |            |             |           |       |         |          |          |          |           |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |  **76.344 μs** |   **6.0204 μs** | **0.3300 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** |  **203.6 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 753.892 μs |  75.1974 μs | 4.1218 μs |  9.88 |    0.06 | 249.0234 | 249.0234 | 249.0234 | 957.73 KB |        4.70 |
| CortexAugmentedDickeyFuller   | 2000       | 248.398 μs | 157.2757 μs | 8.6208 μs |  3.25 |    0.10 |  30.2734 |  30.2734 |  30.2734 | 142.76 KB |        0.70 |
| LodestarKpss                  | 2000       |  82.787 μs |   2.4774 μs | 0.1358 μs |  1.08 |    0.00 |   0.8545 |        - |        - |  15.75 KB |        0.08 |
| CortexKpss                    | 2000       |  84.014 μs |   4.7133 μs | 0.2584 μs |  1.10 |    0.01 |   1.8311 |        - |        - |  31.55 KB |        0.15 |
| LodestarDecompose             | 2000       |  10.868 μs |   0.4096 μs | 0.0225 μs |  0.14 |    0.00 |   3.8300 |   0.4120 |        - |  62.75 KB |        0.31 |
| CortexDecompose               | 2000       |  20.581 μs |   0.4622 μs | 0.0253 μs |  0.27 |    0.00 |   4.7913 |   0.4578 |        - |  78.48 KB |        0.39 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean             | Error          | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |-----------------:|---------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |        **786.15 ns** |      **72.417 ns** |      **3.969 ns** |    **1.00** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     26,749.95 ns |   3,005.500 ns |    164.742 ns |   34.03 |    0.23 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      1,966.65 ns |      93.901 ns |      5.147 ns |    2.50 |    0.01 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 100        |     17,149.98 ns |   1,836.700 ns |    100.676 ns |   21.82 |    0.15 |   1.3733 |   0.0305 |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |         85.36 ns |       3.949 ns |      0.216 ns |    0.11 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 100        |        170.99 ns |       6.165 ns |      0.338 ns |    0.22 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
|                     |            |                  |                |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **32,877.27 ns** |      **81.057 ns** |      **4.443 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    106,297.58 ns |  14,059.020 ns |    770.622 ns |   3.233 |    0.02 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    355,584.80 ns | 145,328.863 ns |  7,965.965 ns |  10.816 |    0.21 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 10,702,775.43 ns | 661,697.014 ns | 36,269.846 ns | 325.537 |    0.96 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |         95.08 ns |       1.819 ns |      0.100 ns |   0.003 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        169.00 ns |       0.261 ns |      0.014 ns |   0.005 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean          | Error          | StdDev      | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------- |----------- |--------------:|---------------:|------------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |      **9.318 μs** |      **0.1466 μs** |   **0.0080 μs** |  **1.00** |    **0.00** |    **0.1984** |         **-** |         **-** |     **3.37 KB** |        **1.00** |
| MathNet_Wls  | 200        |     14.068 μs |      0.9449 μs |   0.0518 μs |  1.51 |    0.00 |    3.7079 |    0.1831 |         - |    58.27 KB |       17.30 |
|              |            |               |                |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **2000**       |     **76.052 μs** |      **1.9119 μs** |   **0.1048 μs** |  **1.00** |    **0.00** |    **0.9766** |         **-** |         **-** |    **17.43 KB** |        **1.00** |
| MathNet_Wls  | 2000       |     87.462 μs |      5.4040 μs |   0.2962 μs |  1.15 |    0.00 |   33.2031 |   13.1836 |         - |   508.39 KB |       29.17 |
|              |            |               |                |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **20000**      |    **747.548 μs** |     **53.5014 μs** |   **2.9326 μs** |  **1.00** |    **0.00** |   **46.8750** |   **46.8750** |   **46.8750** |   **158.43 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,144.451 μs |    950.6967 μs |  52.1109 μs |  1.53 |    0.06 | 1039.0625 | 1039.0625 | 1039.0625 |  5025.06 KB |       31.72 |
|              |            |               |                |             |       |         |           |           |           |             |             |
| **Lodestar_Wls** | **200000**     |  **7,702.112 μs** |    **113.0792 μs** |   **6.1983 μs** |  **1.00** |    **0.00** |   **93.7500** |   **93.7500** |   **93.7500** |  **1564.65 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 12,798.874 μs | 12,261.1144 μs | 672.0731 μs |  1.66 |    0.08 |  734.3750 |  734.3750 |  734.3750 | 50023.64 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------------------------ |---------:|---------:|---------:|----------:|----------:|
| ChatTemplate            | 85.51 ms | 53.06 ms | 2.908 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens | 68.11 ms | 15.15 ms | 0.831 ms | 1750.0000 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Rows | Method   | Mean           | Error         | StdDev       | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|-------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,480.3 μs** |      **62.51 μs** |      **3.43 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.49 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    76,780.9 μs | 105,241.78 μs |  5,768.66 μs |  51.87 |    3.38 |  3857.1429 |  1571.4286 |  857.1429 |  59880.77 KB |       58.56 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,387.6 μs** |     **147.19 μs** |      **8.07 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    81,467.0 μs |  12,014.69 μs |    658.57 μs |  58.71 |    0.51 |  5000.0000 |  1857.1429 |  857.1429 |  83111.72 KB |       81.30 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **551.9 μs** |      **38.62 μs** |      **2.12 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.31 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   109,482.4 μs |  13,120.22 μs |    719.16 μs | 198.36 |    1.31 |  4800.0000 |  1800.0000 |  800.0000 |  81289.96 KB |    2,681.64 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **1,691.4 μs** |     **143.87 μs** |      **7.89 μs** |   **1.00** |    **0.01** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    56,109.8 μs |   1,699.49 μs |     93.15 μs |  33.17 |    0.14 |  3888.8889 |  1777.7778 |  888.8889 |  64191.22 KB |       62.79 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **10,743.2 μs** |   **1,309.65 μs** |     **71.79 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.08 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,291,308.1 μs | 136,850.06 μs |  7,501.21 μs | 120.20 |    0.92 | 35000.0000 | 10000.0000 | 4000.0000 | 537151.59 KB |       60.18 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |     **9,965.8 μs** |     **713.43 μs** |     **39.11 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8924.93 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,395,483.3 μs | 651,308.09 μs | 35,700.39 μs | 140.03 |    3.14 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.99 KB |       83.74 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **4,714.5 μs** |     **504.70 μs** |     **27.66 μs** |   **1.00** |    **0.01** |          **-** |          **-** |         **-** |     **89.89 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 2,529,109.4 μs | 185,271.83 μs | 10,155.37 μs | 536.47 |    3.30 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.18 KB |    8,150.73 |
|                        |      |          |                |               |              |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **13,065.6 μs** |     **448.65 μs** |     **24.59 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8924.93 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,318,047.7 μs |  74,616.03 μs |  4,089.96 μs | 100.88 |    0.32 | 38000.0000 | 11000.0000 | 4000.0000 | 573618.81 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **4.292 μs** |  **0.2848 μs** | **0.0156 μs** |  **1.00** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |   4.542 μs |  0.0124 μs | 0.0007 μs |  1.06 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |   4.584 μs |  0.3372 μs | 0.0185 μs |  1.07 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **55.204 μs** |  **1.4747 μs** | **0.0808 μs** |  **1.00** |  **1.7700** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |  28.637 μs |  2.1879 μs | 0.1199 μs |  0.52 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |  28.513 μs |  0.6810 μs | 0.0373 μs |  0.52 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **204.717 μs** | **19.5344 μs** | **1.0707 μs** |  **1.00** |  **6.5918** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         | 102.332 μs |  2.3652 μs | 0.1296 μs |  0.50 |  5.0049 | 0.1221 |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |  93.016 μs |  8.5183 μs | 0.4669 μs |  0.45 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **842.326 μs** | **57.2497 μs** | **3.1381 μs** |  **1.00** | **26.3672** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 406.242 μs | 52.0895 μs | 2.8552 μs |  0.48 | 20.0195 | 2.4414 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        | 352.950 μs |  6.3539 μs | 0.3483 μs |  0.42 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **113.46 ms** | **11.069 ms** | **0.607 ms** |  **1.00** |    **0.01** |   **27.17 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  57.59 ms |  1.695 ms | 0.093 ms |  0.51 |    0.00 |  103.48 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **122.05 ms** | **11.668 ms** | **0.640 ms** |  **1.00** |    **0.01** |   **23.78 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  54.19 ms |  1.293 ms | 0.071 ms |  0.44 |    0.00 |  116.45 KB |        4.90 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **188.36 ms** | **10.178 ms** | **0.558 ms** |  **1.00** |    **0.00** |  **103.35 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 213.77 ms |  6.046 ms | 0.331 ms |  1.13 |    0.00 |   258.9 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **177.16 ms** | **76.033 ms** | **4.168 ms** |  **1.00** |    **0.03** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 187.00 ms |  2.990 ms | 0.164 ms |  1.06 |    0.02 |  192.71 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **214.91 ms** |  **1.475 ms** | **0.081 ms** |  **1.00** |    **0.00** |  **949.61 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 295.67 ms |  1.205 ms | 0.066 ms |  1.38 |    0.00 | 1365.78 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **219.07 ms** |  **8.784 ms** | **0.481 ms** |  **1.00** |    **0.00** |  **741.28 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 276.76 ms | 25.045 ms | 1.373 ms |  1.26 |    0.01 | 1152.95 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **253.14 ms** |  **3.313 ms** | **0.182 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 349.56 ms |  6.109 ms | 0.335 ms |  1.38 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **256.02 ms** |  **4.505 ms** | **0.247 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 349.56 ms | 14.634 ms | 0.802 ms |  1.37 |    0.00 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error         | StdDev       | Allocated |
|------- |------- |--------------:|--------------:|-------------:|----------:|
| **Latin**  | **1000**   |      **38.75 μs** |      **0.103 μs** |     **0.006 μs** |         **-** |
| Cjk    | 1000   |      49.17 μs |      1.085 μs |     0.059 μs |         - |
| **Latin**  | **10000**  |   **3,541.18 μs** |    **144.680 μs** |     **7.930 μs** |         **-** |
| Cjk    | 10000  |   5,457.35 μs |    140.992 μs |     7.728 μs |         - |
| **Latin**  | **65536**  | **161,530.88 μs** | **22,266.450 μs** | **1,220.499 μs** |         **-** |

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

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.628 μs** |      **0.8575 μs** |     **0.0470 μs** |     **1.00** |    **0.04** |    **0.0248** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       1.963 μs |      0.4307 μs |     0.0236 μs |     1.21 |    0.03 |    0.0229 |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       2.658 μs |      1.8917 μs |     0.1037 μs |     1.63 |    0.07 |    0.3128 |         - |        - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   6,212.106 μs |    762.8482 μs |    41.8143 μs | 3,817.02 |   99.43 |  984.3750 |  906.2500 | 882.8125 |  4837583 B |   11,409.39 |
| LuceneFromText         | 1000      |   5,944.706 μs |  3,018.0221 μs |   165.4280 μs | 3,652.72 |  127.88 |   85.9375 |   78.1250 |   7.8125 |  1375127 B |    3,243.22 |
|                        |           |                |                |               |          |         |           |           |          |            |             |
| **LodestarQuery**          | **20000**     |      **18.891 μs** |      **0.9268 μs** |     **0.0508 μs** |     **1.00** |    **0.00** |         **-** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      32.153 μs |      1.3713 μs |     0.0752 μs |     1.70 |    0.01 |         - |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      14.382 μs |      1.8720 μs |     0.1026 μs |     0.76 |    0.01 |    0.5035 |         - |        - |     8648 B |       20.40 |
| LodestarFromText       | 20000     |  91,341.294 μs | 22,906.8668 μs | 1,255.6027 μs | 4,835.22 |   58.65 | 2166.6667 | 1166.6667 | 833.3333 | 83750619 B |  197,525.04 |
| LuceneFromText         | 20000     | 114,894.619 μs | 37,609.0313 μs | 2,061.4779 μs | 6,082.04 |   95.56 | 1200.0000 | 1000.0000 |        - | 22416571 B |   52,869.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 24.84 ms | 0.051 ms | 0.003 ms |  1.00 |    0.00 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 69.03 ms | 9.227 ms | 0.506 ms |  2.78 |    0.02 | 1750.0000 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |----------:|-----------:|----------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **15.53 μs** |   **0.410 μs** |  **0.022 μs** | **0.4578** |      **-** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **32.14 μs** |  **11.653 μs** |  **0.639 μs** | **0.8545** |      **-** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **62.47 μs** |   **2.440 μs** |  **0.134 μs** | **1.7090** |      **-** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **189.28 μs** | **215.302 μs** | **11.801 μs** | **3.4180** | **0.2441** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev   | Allocated |
|------------------ |---------:|---------:|---------:|----------:|
| EncodeUnseenProse | 33.84 ms | 10.71 ms | 0.587 ms |    7.5 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **15.127 μs** | **1.4078 μs** | **0.0772 μs** |         **-** |
| MyersGroup | cjk      | 129.977 μs | 0.8068 μs | 0.0442 μs |         - |
| **DpGroup**    | **latin**    |   **8.024 μs** | **0.0652 μs** | **0.0036 μs** |         **-** |
| MyersGroup | latin    |  87.898 μs | 0.3493 μs | 0.0191 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error        | StdDev    | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|-------------:|----------:|-------:|-------:|----------:|
| **Report** | **10**      |     **302.7 ns** |     **84.90 ns** |   **4.65 ns** | **0.0939** |      **-** |   **1.54 KB** |
| **Report** | **1000**    | **453,992.3 ns** | **10,159.91 ns** | **556.90 ns** | **6.8359** | **1.4648** | **117.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 117.9 ms | 10.71 ms | 0.59 ms | 600.0000 | 600.0000 | 600.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 106.5 ms | 13.75 ms | 0.75 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | Samples | Clusters | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **1.856 ms** |  **0.1044 ms** | **0.0057 ms** |        **-** |        **-** |        **-** |   **12.06 KB** |
| MutualInformationScore         | 100000  | 10       |   1.854 ms |  0.1054 ms | 0.0058 ms |        - |        - |        - |   12.06 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  17.314 ms |  0.5647 ms | 0.0310 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **2.502 ms** |  **0.2780 ms** | **0.0152 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   2.609 ms |  0.0503 ms | 0.0028 ms | 210.9375 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 151.062 ms | 22.4766 ms | 1.2320 ms | 250.0000 | 250.0000 | 250.0000 | 1744.81 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Length | Mean        | Error       | StdDev    | Gen0   | Allocated |
|--------- |------- |------------:|------------:|----------:|-------:|----------:|
| **Distance** | **12**     |    **738.8 ns** |    **21.68 ns** |   **1.19 ns** | **0.0496** |     **840 B** |
| **Distance** | **120**    | **47,961.3 ns** | **4,419.54 ns** | **242.25 ns** | **0.1221** |    **2128 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Shape     | Mean       | Error    | StdDev  | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |-----------:|---------:|--------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **323.8 ms** |  **9.90 ms** | **0.54 ms** |  **1.00** |    **0.00** |  **1500.0000** | **1500.0000** | **1500.0000** |  **28.69 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,022.8 ms | 76.64 ms | 4.20 ms |  6.25 |    0.01 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.45 |
|              |           |            |          |         |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |   **112.5 ms** | **12.83 ms** | **0.70 ms** |  **1.00** |    **0.01** |  **1400.0000** | **1400.0000** | **1400.0000** |  **13.29 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   700.2 ms | 36.48 ms | 2.00 ms |  6.22 |    0.04 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Shape      | Mean        | Error      | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** |   **856.93 ms** | **162.712 ms** | **8.919 ms** |  **1.00** |    **0.01** |  **2000.0000** |  **2000.0000** |  **2000.0000** | **112.13 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 5,587.39 ms |  85.067 ms | 4.663 ms |  6.52 |    0.06 | 38000.0000 | 12000.0000 | 11000.0000 | 531.86 MB |        4.74 |
| Dbscan_CalculateClusters | 20000x2x10 | 3,105.98 ms | 103.289 ms | 5.662 ms |  3.62 |    0.03 | 22000.0000 | 12000.0000 | 10000.0000 | 372.12 MB |        3.32 |
|                          |            |             |            |          |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **45.60 ms** |   **0.306 ms** | **0.017 ms** |  **1.00** |    **0.00** |  **1250.0000** |  **1250.0000** |  **1250.0000** |  **14.02 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   420.43 ms |  27.327 ms | 1.498 ms |  9.22 |    0.03 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.75 |
| Dbscan_CalculateClusters | 5000x2x5   |   209.25 ms |   4.259 ms | 0.233 ms |  4.59 |    0.00 |  3666.6667 |  3666.6667 |  3666.6667 |   40.4 MB |        2.88 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean     | Error    | StdDev   | Ratio | RatioSD |
|------------------------------------------ |---------:|---------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       | 15.59 ms | 3.564 ms | 0.195 ms |  1.00 |    0.02 |
| Nmf_Rank20                                | 86.80 ms | 5.095 ms | 0.279 ms |  5.57 |    0.06 |
| MlNet_ProjectToPrincipalComponents_Rank20 | 18.85 ms | 0.907 ms | 0.050 ms |  1.21 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **30,028.4 μs** |      **96.76 μs** |     **5.30 μs** | **3031.2500** | **3000.0000** | **3000.0000** |  **12151.25 KB** |
| TruncatedSvd_Transform     | 500     |     395.0 μs |       0.99 μs |     0.05 μs |   95.2148 |   90.8203 |   90.8203 |    390.73 KB |
| Nmf_Frobenius_Rank20       | 500     | 170,221.0 μs |  16,702.88 μs |   915.54 μs | 4333.3333 | 4333.3333 | 4333.3333 |  18142.39 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 219,789.5 μs |   7,749.27 μs |   424.76 μs | 4333.3333 | 4333.3333 | 4333.3333 |  18595.89 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **174,133.4 μs** | **115,477.46 μs** | **6,329.71 μs** | **2666.6667** | **2666.6667** | **2666.6667** | **105766.49 KB** |
| TruncatedSvd_Transform     | 20000   |   1,136.7 μs |     226.30 μs |    12.40 μs |  496.0938 |  496.0938 |  496.0938 |   3437.86 KB |
| Nmf_Frobenius_Rank20       | 20000   | 613,074.2 μs |  92,861.10 μs | 5,090.03 μs | 5000.0000 | 5000.0000 | 5000.0000 | 156850.65 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 520,052.5 μs |  52,386.64 μs | 2,871.49 μs | 5000.0000 | 5000.0000 | 5000.0000 | 157305.66 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 194.4 μs | 21.30 μs | 1.17 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count  | Mean       | Error     | StdDev   | Allocated |
|------------ |------- |-----------:|----------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **434.2 μs** |  **23.65 μs** |  **1.30 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **5,158.8 μs** | **320.65 μs** | **17.58 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Rows   | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|----------:|---------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **598.4 μs** | **330.72 μs** | **18.13 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  5,668.2 μs | 816.01 μs | 44.73 μs |        - |        - |        - |     21.02 KB |
| **FromBlockNormalized** | **100000** | **53,107.5 μs** | **541.23 μs** | **29.67 μs** | **300.0000** | **300.0000** | **300.0000** | **150000.29 KB** |
| MmrSelect100        | 100000 |  5,652.4 μs |  94.49 μs |  5.18 μs |        - |        - |        - |     21.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-FGTQMV : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-FGTQMV** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **153.70 μs** |    **15.130 μs** |    **16.189 μs** |  **1.01** |    **0.14** |        **-** |        **-** |        **-** |  **166.08 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 1000     |    156.13 μs |    21.185 μs |    23.547 μs |  1.03 |    0.18 |        - |        - |        - |   17.53 KB |        0.11 |
| Lodestar_Impute                 | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 1000     |    135.01 μs |    17.595 μs |    18.827 μs |  0.89 |    0.15 |        - |        - |        - |   93.25 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 1000     |    269.50 μs |    10.675 μs |    11.865 μs |  1.77 |    0.18 |        - |        - |        - |   34.19 KB |        0.21 |
| MlNet_OneHotEncoding_Read       | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,223.75 μs |    61.361 μs |    65.655 μs |  8.04 |    0.86 |        - |        - |        - |  325.99 KB |        1.96 |
| MlNet_ReplaceMissingValues_Read | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 1000     |    913.31 μs |    46.901 μs |    50.184 μs |  6.00 |    0.64 |        - |        - |        - |  313.55 KB |        1.89 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    119.71 μs |     9.094 μs |     0.498 μs |  1.00 |    0.01 |  49.9268 |  49.9268 |  49.9268 |  165.37 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     53.25 μs |    25.149 μs |     1.378 μs |  0.44 |    0.01 |   0.9766 |        - |        - |   16.81 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     18.30 μs |     1.999 μs |     0.110 μs |  0.15 |    0.00 |   5.6458 |   0.1526 |        - |   92.53 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    740.80 μs | 2,448.903 μs |   134.233 μs |  6.19 |    0.97 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.96 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    783.33 μs |   819.460 μs |    44.917 μs |  6.54 |    0.33 |  48.8281 |   7.8125 |        - |  793.85 KB |        4.80 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    209.11 μs |   144.914 μs |     7.943 μs |  1.75 |    0.06 |  18.5547 |  18.0664 |   0.4883 |  291.29 KB |        1.76 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-FGTQMV** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **3,480.60 μs** |    **52.907 μs** |    **58.806 μs** |  **1.00** |    **0.02** |        **-** |        **-** |        **-** | **3283.27 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,980.02 μs |    24.151 μs |    27.813 μs |  0.86 |    0.02 |        - |        - |        - |  314.41 KB |        0.10 |
| Lodestar_Impute                 | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,130.92 μs |    28.277 μs |    31.430 μs |  0.33 |    0.01 |        - |        - |        - | 1844.05 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 20000    |  2,782.65 μs |    28.166 μs |    32.436 μs |  0.80 |    0.02 |        - |        - |        - |   34.19 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,815.48 μs | 2,508.620 μs | 2,888.930 μs |  3.97 |    0.81 |        - |        - |        - |  448.89 KB |        0.14 |
| MlNet_ReplaceMissingValues_Read | Job-FGTQMV | 1               | 20             | Throughput  | 1            | 5           | 20000    |  8,933.72 μs | 2,733.809 μs | 2,925.145 μs |  2.57 |    0.82 |        - |        - |        - |  450.65 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,740.99 μs |   285.890 μs |    15.671 μs |  1.00 |    0.01 | 992.1875 | 992.1875 | 992.1875 | 3290.92 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,442.39 μs |   176.578 μs |     9.679 μs |  0.89 |    0.01 |  89.8438 |  89.8438 |  89.8438 |  314.34 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    573.61 μs |   213.815 μs |    11.720 μs |  0.21 |    0.00 | 323.2422 | 323.2422 | 323.2422 | 1845.99 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,151.16 μs | 1,012.717 μs |    55.510 μs |  0.42 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,477.36 μs | 1,272.495 μs |    69.750 μs |  1.27 |    0.02 |  39.0625 |   7.8125 |        - |  685.65 KB |        0.21 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,873.69 μs | 2,388.056 μs |   130.897 μs |  0.68 |    0.04 |  27.3438 |  11.7188 |        - |  448.54 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method        | Records | Mean        | Error       | StdDev    | Allocated |
|-------------- |-------- |------------:|------------:|----------:|----------:|
| **FilteredTop10** | **10000**   |    **392.7 μs** |    **63.98 μs** |   **3.51 μs** |    **7.2 KB** |
| Unfiltered    | 10000   |    433.8 μs |    26.85 μs |   1.47 μs |    2.8 KB |
| **FilteredTop10** | **100000**  | **10,021.9 μs** | **2,207.28 μs** | **120.99 μs** |   **7.21 KB** |
| Unfiltered    | 100000  |  5,167.5 μs | 1,317.71 μs |  72.23 μs |    2.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |  85.82 ns |   2.515 ns |  0.138 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   | 596.68 ns |  94.644 ns |  5.188 ns |  6.95 |    0.05 |      - |         - |          NA |
| TokenSortRatio | 675.73 ns | 381.291 ns | 20.900 ns |  7.87 |    0.21 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  | 691.14 ns | 267.824 ns | 14.680 ns |  8.05 |    0.15 | 0.0534 |     896 B |          NA |
| WRatio         | 979.99 ns | 313.985 ns | 17.211 ns | 11.42 |    0.17 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **86.09 ns** |  **20.18 ns** |  **1.106 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |   176.47 ns |  20.22 ns |  1.108 ns |  2.05 |    0.03 | 0.0048 |      80 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |   **600.62 ns** | **179.63 ns** |  **9.846 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 7,823.20 ns | 473.19 ns | 25.937 ns | 13.03 |    0.19 |      - |     160 B |          NA |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |   **697.83 ns** | **240.04 ns** | **13.158 ns** |  **1.00** |    **0.02** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio | 1,548.97 ns |  52.71 ns |  2.889 ns |  2.22 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |             |           |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |   **894.21 ns** | **115.02 ns** |  **6.305 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        | 3,767.84 ns | 177.61 ns |  9.735 ns |  4.21 |    0.03 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.175 ms** | **0.1954 ms** | **0.0107 ms** | **277.3438** | **267.5781** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **13.266 ms** | **0.2727 ms** | **0.0149 ms** | **984.3750** | **984.3750** | **984.3750** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **22.93 ns** |     **1.057 ns** |   **0.058 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    27.24 ns |     0.364 ns |   0.020 ns |  1.19 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    24.21 ns |     0.581 ns |   0.032 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    22.63 ns |     0.386 ns |   0.021 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **25.28 ns** |     **2.846 ns** |   **0.156 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    29.16 ns |     0.151 ns |   0.008 ns |  1.15 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    24.17 ns |     1.467 ns |   0.080 ns |  0.96 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    24.26 ns |     0.493 ns |   0.027 ns |  0.96 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **25.97 ns** |     **0.274 ns** |   **0.015 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    31.28 ns |     1.089 ns |   0.060 ns |  1.20 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    27.61 ns |     0.600 ns |   0.033 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    31.54 ns |     0.778 ns |   0.043 ns |  1.21 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **30.27 ns** |     **3.848 ns** |   **0.211 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    33.76 ns |     5.386 ns |   0.295 ns |  1.12 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    29.62 ns |     1.273 ns |   0.070 ns |  0.98 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    26.93 ns |     3.616 ns |   0.198 ns |  0.89 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **48.36 ns** |     **1.578 ns** |   **0.087 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    51.34 ns |     1.149 ns |   0.063 ns |  1.06 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    56.13 ns |     1.465 ns |   0.080 ns |  1.16 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    46.90 ns |     0.798 ns |   0.044 ns |  0.97 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **54.10 ns** |     **0.997 ns** |   **0.055 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    57.11 ns |     0.601 ns |   0.033 ns |  1.06 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    57.40 ns |     4.555 ns |   0.250 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    51.17 ns |     2.963 ns |   0.162 ns |  0.95 |    0.00 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **313.12 ns** |    **36.443 ns** |   **1.998 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   334.87 ns |     0.835 ns |   0.046 ns |  1.07 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   313.76 ns |    24.058 ns |   1.319 ns |  1.00 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   312.85 ns |    12.763 ns |   0.700 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |             |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **3,909.99 ns** |     **5.652 ns** |   **0.310 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 4,115.62 ns |    47.198 ns |   2.587 ns |  1.05 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 4,002.02 ns | 1,931.219 ns | 105.857 ns |  1.02 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 3,884.89 ns |   472.842 ns |  25.918 ns |  0.99 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **326.9 ns** |   **3.66 ns** |  **0.20 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    163.7 ns |   1.27 ns |  0.07 ns |  0.50 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **1,926.1 ns** |  **16.91 ns** |  **0.93 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  4,030.8 ns |  79.72 ns |  4.37 ns |  2.09 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **11,251.0 ns** | **137.55 ns** |  **7.54 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 29,640.0 ns | 798.04 ns | 43.74 ns |  2.63 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Shape       | Mean         | Error        | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------------- |------------ |-------------:|-------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **4,433.6 μs** |    **188.66 μs** |    **10.34 μs** |  **1.00** |    **0.00** | **7.8125** |      **-** | **162.63 KB** |        **1.00** |
| NumFlat_Fit      | 10000x16x16 |  29,134.0 μs | 10,920.91 μs |   598.61 μs |  6.57 |    0.12 |      - |      - |  10.33 KB |        0.06 |
| MetaNumerics_Fit | 10000x16x16 |  74,366.2 μs | 43,895.56 μs | 2,406.06 μs | 16.77 |    0.47 |      - |      - |   43.4 KB |        0.27 |
|                  |             |              |              |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **560.4 μs** |     **19.48 μs** |     **1.07 μs** |  **1.00** |    **0.00** | **8.7891** | **0.9766** | **156.73 KB** |        **1.00** |
| NumFlat_Fit      | 10000x2x8   |   4,299.7 μs |  1,045.54 μs |    57.31 μs |  7.67 |    0.09 |      - |      - |      3 KB |        0.02 |
| MetaNumerics_Fit | 10000x2x8   |   2,259.7 μs |    148.88 μs |     8.16 μs |  4.03 |    0.01 |      - |      - |  39.57 KB |        0.25 |
|                  |             |              |              |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **22,144.8 μs** |    **350.78 μs** |    **19.23 μs** |  **1.00** |    **0.00** |      **-** |      **-** | **787.79 KB** |        **1.00** |
| NumFlat_Fit      | 50000x8x32  | 350,884.8 μs | 87,148.80 μs | 4,776.92 μs | 15.85 |    0.19 |      - |      - |  16.01 KB |        0.02 |
| MetaNumerics_Fit | 50000x8x32  | 546,905.5 μs |  9,373.86 μs |   513.81 μs | 24.70 |    0.03 |      - |      - | 199.91 KB |        0.25 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **5.883 ms** |  **0.3779 ms** | **0.0207 ms** |  **1.00** |    **0.00** |   **88.7 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x16x16 |  13.180 ms |  3.4282 ms | 0.1879 ms |  2.24 |    0.03 |  13.58 KB |        0.15 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **24.001 ms** |  **1.9662 ms** | **0.1078 ms** |  **1.00** |    **0.01** |  **98.92 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   |  96.222 ms |  9.2693 ms | 0.5081 ms |  4.01 |    0.02 |  96.44 KB |        0.97 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **59.058 ms** |  **3.2138 ms** | **0.1762 ms** |  **1.00** |    **0.00** | **410.24 KB** |        **1.00** |
| NumFlat_Lloyd  | 50000x8x32  | 172.940 ms | 26.6705 ms | 1.4619 ms |  2.93 |    0.02 |  36.47 KB |        0.09 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |   **102.13 ns** |     **2.820 ns** |   **0.155 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    48.81 ns |     0.339 ns |   0.019 ns |  0.48 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   100.83 ns |     3.133 ns |   0.172 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |   115.36 ns |     0.617 ns |   0.034 ns |  1.13 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **178.51 ns** |   **105.641 ns** |   **5.791 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 12   |    53.40 ns |     1.121 ns |   0.061 ns |  0.30 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |   171.52 ns |     1.920 ns |   0.105 ns |  0.96 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |    97.56 ns |     1.529 ns |   0.084 ns |  0.55 |    0.02 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **14**   |   **238.55 ns** |   **248.800 ns** |  **13.638 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 14   |    56.65 ns |     2.904 ns |   0.159 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |   246.44 ns |   126.521 ns |   6.935 ns |  1.04 |    0.06 |         - |          NA |
| Kernel_Cjk | 14   |    97.97 ns |    29.493 ns |   1.617 ns |  0.41 |    0.02 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **267.25 ns** |     **2.768 ns** |   **0.152 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    60.54 ns |     6.206 ns |   0.340 ns |  0.23 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   267.43 ns |     0.951 ns |   0.052 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |   101.21 ns |     1.399 ns |   0.077 ns |  0.38 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **18**   |   **495.84 ns** |   **538.256 ns** |  **29.504 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 18   |    65.79 ns |     2.056 ns |   0.113 ns |  0.13 |    0.01 |         - |          NA |
| Dp_Cjk     | 18   |   516.79 ns |    29.669 ns |   1.626 ns |  1.04 |    0.05 |         - |          NA |
| Kernel_Cjk | 18   |   109.69 ns |     2.772 ns |   0.152 ns |  0.22 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **20**   |   **621.97 ns** |    **77.429 ns** |   **4.244 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |    68.68 ns |     1.258 ns |   0.069 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |   618.71 ns |    58.224 ns |   3.191 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |   108.64 ns |     0.619 ns |   0.034 ns |  0.17 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **769.23 ns** |    **23.611 ns** |   **1.294 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    75.25 ns |     1.251 ns |   0.069 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   778.82 ns |   415.393 ns |  22.769 ns |  1.01 |    0.03 |         - |          NA |
| Kernel_Cjk | 24   |   120.45 ns |    22.814 ns |   1.250 ns |  0.16 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,273.60 ns** |   **623.149 ns** |  **34.157 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 32   |    91.36 ns |     1.206 ns |   0.066 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,405.75 ns | 1,401.775 ns |  76.836 ns |  1.10 |    0.06 |         - |          NA |
| Kernel_Cjk | 32   |   140.10 ns |     5.619 ns |   0.308 ns |  0.11 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **2,503.18 ns** |    **48.583 ns** |   **2.663 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |   115.66 ns |     3.074 ns |   0.169 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,511.86 ns |    59.470 ns |   3.260 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |   202.46 ns |     0.877 ns |   0.048 ns |  0.08 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **4,246.15 ns** |   **861.763 ns** |  **47.236 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |   133.82 ns |     3.653 ns |   0.200 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,128.66 ns | 1,246.766 ns |  68.339 ns |  0.97 |    0.02 |         - |          NA |
| Kernel_Cjk | 64   |   240.26 ns |     4.453 ns |   0.244 ns |  0.06 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **8,358.13 ns** | **1,002.036 ns** |  **54.925 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |   322.85 ns |   532.196 ns |  29.171 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 8,699.90 ns | 6,459.265 ns | 354.054 ns |  1.04 |    0.04 |         - |          NA |
| Kernel_Cjk | 96   |   735.38 ns |     1.316 ns |   0.072 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **21.73 ns** |   **0.385 ns** |  **0.021 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     20.98 ns |   0.340 ns |  0.019 ns |  0.97 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |     98.40 ns |  38.805 ns |  2.127 ns |  4.53 |    0.08 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     21.91 ns |   0.188 ns |  0.010 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **243.41 ns** |  **21.743 ns** |  **1.192 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    312.73 ns |   1.779 ns |  0.098 ns |  1.28 |    0.01 |         - |          NA |
| Distance_CodePoint         | 64     |    547.33 ns |   3.903 ns |  0.214 ns |  2.25 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    243.37 ns |  10.401 ns |  0.570 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |    **782.33 ns** |  **27.306 ns** |  **1.497 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,510.23 ns |   5.054 ns |  0.277 ns |  1.93 |    0.00 |         - |          NA |
| Distance_CodePoint         | 128    |  1,325.36 ns |  16.186 ns |  0.887 ns |  1.69 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    774.08 ns |   4.811 ns |  0.264 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |              |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    |  **9,640.78 ns** | **116.033 ns** |  **6.360 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 14,034.97 ns | 272.363 ns | 14.929 ns |  1.46 |    0.00 |         - |          NA |
| Distance_CodePoint         | 512    | 11,589.20 ns | 101.898 ns |  5.585 ns |  1.20 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |  9,692.53 ns |  86.842 ns |  4.760 ns |  1.01 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **276.6 ns** |     **7.73 ns** |   **0.42 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     198.7 ns |     1.14 ns |   0.06 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **273.4 ns** |     **3.54 ns** |   **0.19 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     199.2 ns |     1.71 ns |   0.09 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **355.5 ns** |     **1.19 ns** |   **0.07 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     271.7 ns |     1.73 ns |   0.09 ns |  0.76 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **341.4 ns** |    **22.31 ns** |   **1.22 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     272.6 ns |   110.06 ns |   6.03 ns |  0.80 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **416.9 ns** |    **76.02 ns** |   **4.17 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     344.1 ns |     9.34 ns |   0.51 ns |  0.83 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **417.7 ns** |     **6.07 ns** |   **0.33 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     344.1 ns |     3.23 ns |   0.18 ns |  0.82 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **489.7 ns** |     **3.71 ns** |   **0.20 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,008.2 ns |     6.22 ns |   0.34 ns |  2.06 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **488.2 ns** |     **5.20 ns** |   **0.29 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,018.3 ns |     6.92 ns |   0.38 ns |  2.09 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **1,561.1 ns** |    **30.90 ns** |   **1.69 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   4,478.2 ns |   318.67 ns |  17.47 ns |  2.87 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **1,564.4 ns** |    **10.03 ns** |   **0.55 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   4,446.1 ns |   550.83 ns |  30.19 ns |  2.84 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **12,740.3 ns** |   **338.32 ns** |  **18.54 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  51,574.6 ns | 9,376.88 ns | 513.98 ns |  4.05 |    0.04 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **336,593.3 ns** | **8,094.15 ns** | **443.67 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  55,506.8 ns |   538.44 ns |  29.51 ns |  0.16 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **21.71 ns** |      **0.322 ns** |     **0.018 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      67.72 ns |      1.989 ns |     0.109 ns |  3.12 |    0.00 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      64.06 ns |      3.354 ns |     0.184 ns |  2.95 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     138.19 ns |      3.893 ns |     0.213 ns |  6.36 |    0.01 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **247.25 ns** |     **58.522 ns** |     **3.208 ns** |  **1.00** |    **0.02** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   3,717.30 ns |    492.978 ns |    27.022 ns | 15.04 |    0.19 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |     997.01 ns |     64.769 ns |     3.550 ns |  4.03 |    0.05 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   7,210.74 ns |  1,828.747 ns |   100.240 ns | 29.17 |    0.48 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |   **9,635.99 ns** |    **163.359 ns** |     **8.954 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 371,993.09 ns |  7,275.329 ns |   398.785 ns | 38.60 |    0.05 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  27,268.63 ns |  1,573.447 ns |    86.246 ns |  2.83 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 572,035.01 ns | 28,792.173 ns | 1,578.196 ns | 59.36 |    0.15 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | Shape   | Mean          | Error         | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|--------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **112.60 μs** |      **7.226 μs** |     **0.396 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 |  74,332.58 μs | 25,571.647 μs | 1,401.668 μs | 660.17 |   10.97 | 857.1429 | 857.1429 | 857.1429 | 31413.73 KB |   13,226.83 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **2,459.89 μs** |     **86.566 μs** |     **4.745 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.07 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 349,621.84 μs | 63,079.703 μs | 3,457.611 μs | 142.13 |    1.24 |        - |        - |        - | 32054.48 KB |      762.01 |
|                                 |         |               |               |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **18.76 μs** |      **0.127 μs** |     **0.007 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     721.65 μs |    234.164 μs |    12.835 μs |  38.48 |    0.59 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      138.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean            | Error            | StdDev        | Gen0   | Allocated |
|--------------- |-------- |-------- |----------------:|-----------------:|--------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **3,243.56 ns** |       **998.489 ns** |     **54.731 ns** | **0.0191** |     **376 B** |
| MatrixWeighted | 1000    | 2       |     5,792.25 ns |     2,204.922 ns |    120.859 ns | 0.0153 |     376 B |
| AccuracyScore  | 1000    | 2       |        90.60 ns |        22.258 ns |      1.220 ns |      - |         - |
| F1Macro        | 1000    | 2       |     3,292.08 ns |        82.989 ns |      4.549 ns | 0.0305 |     536 B |
| Report         | 1000    | 2       |     5,319.05 ns |       452.088 ns |     24.780 ns | 0.3128 |    5344 B |
| **Matrix**         | **1000**    | **10**      |     **3,219.11 ns** |       **889.670 ns** |     **48.766 ns** | **0.0763** |    **1312 B** |
| MatrixWeighted | 1000    | 10      |     5,801.27 ns |       261.905 ns |     14.356 ns | 0.0763 |    1312 B |
| AccuracyScore  | 1000    | 10      |        89.45 ns |         1.770 ns |      0.097 ns |      - |         - |
| F1Macro        | 1000    | 10      |     3,309.34 ns |        69.263 ns |      3.797 ns | 0.1030 |    1728 B |
| Report         | 1000    | 10      |     8,110.57 ns |       551.486 ns |     30.229 ns | 0.7324 |   12336 B |
| **Matrix**         | **100000**  | **2**       |   **313,932.82 ns** |     **4,377.758 ns** |    **239.960 ns** |      **-** |     **376 B** |
| MatrixWeighted | 100000  | 2       |   664,514.16 ns |    97,969.792 ns |  5,370.055 ns |      - |     377 B |
| AccuracyScore  | 100000  | 2       |     9,121.35 ns |     3,018.373 ns |    165.447 ns |      - |         - |
| F1Macro        | 100000  | 2       |   346,102.96 ns |    30,841.343 ns |  1,690.518 ns |      - |     536 B |
| Report         | 100000  | 2       |   317,757.24 ns |    18,482.933 ns |  1,013.112 ns |      - |    5368 B |
| **Matrix**         | **100000**  | **10**      |   **326,034.41 ns** |    **90,743.515 ns** |  **4,973.958 ns** |      **-** |    **1312 B** |
| MatrixWeighted | 100000  | 10      |   740,450.33 ns |    53,308.979 ns |  2,922.045 ns |      - |    1313 B |
| AccuracyScore  | 100000  | 10      |     9,476.69 ns |     1,022.886 ns |     56.068 ns |      - |         - |
| F1Macro        | 100000  | 10      |   305,467.61 ns |    21,344.679 ns |  1,169.974 ns |      - |    1728 B |
| Report         | 100000  | 10      |   311,329.11 ns |   120,581.648 ns |  6,609.487 ns | 0.4883 |   12680 B |
| **Matrix**         | **1000000** | **2**       | **3,312,756.53 ns** | **1,160,859.056 ns** | **63,630.602 ns** |      **-** |     **379 B** |
| MatrixWeighted | 1000000 | 2       | 6,588,911.23 ns |    85,485.285 ns |  4,685.737 ns |      - |     382 B |
| AccuracyScore  | 1000000 | 2       |    95,140.63 ns |     3,339.021 ns |    183.023 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 3,280,685.69 ns |    40,218.349 ns |  2,204.503 ns |      - |     539 B |
| Report         | 1000000 | 2       | 3,465,321.45 ns |   226,720.590 ns | 12,427.321 ns |      - |    5387 B |
| **Matrix**         | **1000000** | **10**      | **3,056,945.67 ns** |   **242,655.338 ns** | **13,300.758 ns** |      **-** |    **1315 B** |
| MatrixWeighted | 1000000 | 10      | 7,803,412.44 ns |   924,950.779 ns | 50,699.674 ns |      - |    1324 B |
| AccuracyScore  | 1000000 | 10      |    94,635.34 ns |     2,156.023 ns |    118.179 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,006,413.74 ns |    25,559.492 ns |  1,401.002 ns |      - |    1731 B |
| Report         | 1000000 | 10      | 2,924,445.49 ns |   243,046.587 ns | 13,322.204 ns |      - |   12723 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean           | Error         | StdDev        | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |---------------:|--------------:|--------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **5,414.643 μs** |    **346.664 μs** |    **19.0018 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1064 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  29,812.348 μs |  1,837.961 μs |   100.7448 μs |     5.51 |    0.02 | 593.7500 | 593.7500 | 593.7500 |  5089370 B |    4,783.24 |
|          |         |               |                |               |               |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |       **9.742 μs** |      **1.413 μs** |     **0.0774 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  29,871.914 μs |    778.899 μs |    42.6941 μs | 3,066.29 |   21.41 | 593.7500 | 593.7500 | 593.7500 |  5089242 B |          NA |
|          |         |               |                |               |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        |  **82,079.027 μs** | **99,480.409 μs** | **5,452.8569 μs** |     **1.00** |    **0.08** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 198,720.500 μs |  7,195.897 μs |   394.4314 μs |     2.43 |    0.13 |        - |        - |        - | 23229116 B |          NA |
|          |         |               |                |               |               |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |      **94.686 μs** |      **1.765 μs** |     **0.0968 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 197,216.434 μs |  7,623.657 μs |   417.8784 μs | 2,082.84 |    4.24 |        - |        - |        - | 23228948 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean      | Error      | StdDev    | Allocated |
|---------- |-------- |----------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |  **25.72 ms** |  **12.512 ms** |  **0.686 ms** |     **473 B** |
| OneVsOne  | 100000  |  63.42 ms |   0.398 ms |  0.022 ms |  801828 B |
| **OneVsRest** | **1000000** | **482.61 ms** | **308.761 ms** | **16.924 ms** |         **-** |
| OneVsOne  | 1000000 | 919.53 ms |  49.010 ms |  2.686 ms | 8003648 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|------------:|
| PerLabel         | 100000 |  6.110 ms |  0.9249 ms | 0.0507 ms |         - |         - |        - |     6.28 KB |
| PerLabelWeighted | 100000 |  9.337 ms |  0.6437 ms | 0.0353 ms |         - |         - |        - |     6.29 KB |
| PerSample        | 100000 | 45.569 ms | 20.4322 ms | 1.1200 ms | 2636.3636 | 2545.4545 | 818.1818 | 31250.65 KB |
| PerClass         | 100000 |  4.080 ms |  0.7910 ms | 0.0434 ms |         - |         - |        - |     6.43 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean        | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |    **63.49 ns** |     **2.437 ns** |   **0.134 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |    63.43 ns |     2.876 ns |   0.158 ns |  1.00 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |    63.50 ns |     2.161 ns |   0.118 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |    63.52 ns |     1.040 ns |   0.057 ns |  1.00 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **6**    |    **93.94 ns** |    **14.686 ns** |   **0.805 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 6    |    67.37 ns |     1.436 ns |   0.079 ns |  0.72 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    91.07 ns |     5.031 ns |   0.276 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |   114.25 ns |     4.256 ns |   0.233 ns |  1.22 |    0.01 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **8**    |   **121.38 ns** |     **3.863 ns** |   **0.212 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    77.81 ns |     0.140 ns |   0.008 ns |  0.64 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |   124.48 ns |    58.490 ns |   3.206 ns |  1.03 |    0.02 |         - |          NA |
| Kernel_Cjk | 8    |   148.02 ns |     1.873 ns |   0.103 ns |  1.22 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **10**   |   **160.91 ns** |     **1.324 ns** |   **0.073 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    84.98 ns |     0.386 ns |   0.021 ns |  0.53 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |   158.97 ns |     4.249 ns |   0.233 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |   130.95 ns |     3.823 ns |   0.210 ns |  0.81 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **12**   |   **208.93 ns** |     **2.405 ns** |   **0.132 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    93.29 ns |     1.425 ns |   0.078 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |   205.40 ns |     3.837 ns |   0.210 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |   139.26 ns |     1.237 ns |   0.068 ns |  0.67 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **16**   |   **331.63 ns** |    **28.758 ns** |   **1.576 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |   107.58 ns |     1.239 ns |   0.068 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |   330.18 ns |    10.016 ns |   0.549 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |   153.95 ns |     4.729 ns |   0.259 ns |  0.46 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **24**   |   **668.50 ns** |    **28.513 ns** |   **1.563 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |   138.78 ns |     0.839 ns |   0.046 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |   681.80 ns |   268.077 ns |  14.694 ns |  1.02 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |   191.24 ns |     4.167 ns |   0.228 ns |  0.29 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **32**   | **1,152.82 ns** |    **17.428 ns** |   **0.955 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |   178.11 ns |     1.111 ns |   0.061 ns |  0.15 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   | 1,248.75 ns |    15.011 ns |   0.823 ns |  1.08 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |   228.55 ns |     2.876 ns |   0.158 ns |  0.20 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **48**   | **2,518.26 ns** |   **131.131 ns** |   **7.188 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |   234.31 ns |     1.555 ns |   0.085 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   | 2,519.12 ns |    32.719 ns |   1.793 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |   303.93 ns |    22.937 ns |   1.257 ns |  0.12 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **64**   | **4,379.30 ns** |    **65.435 ns** |   **3.587 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |   298.11 ns |    13.057 ns |   0.716 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   | 4,396.23 ns |   220.544 ns |  12.089 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 64   |   376.05 ns |     3.150 ns |   0.173 ns |  0.09 |    0.00 |         - |          NA |
|            |      |             |              |            |       |         |           |             |
| **Dp**         | **96**   | **9,792.69 ns** |    **53.129 ns** |   **2.912 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |   665.84 ns |    16.420 ns |   0.900 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 9,802.12 ns | 2,167.236 ns | 118.793 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   | 1,221.30 ns |     2.218 ns |   0.122 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **33.11 ns** |     **3.397 ns** |   **0.186 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    101.27 ns |     3.277 ns |   0.180 ns |  3.06 |    0.02 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **121.92 ns** |     **8.181 ns** |   **0.448 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,134.28 ns |    25.710 ns |   1.409 ns |  9.30 |    0.03 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **268.12 ns** |     **1.946 ns** |   **0.107 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  5,734.15 ns |   386.715 ns |  21.197 ns | 21.39 |    0.07 |         - |          NA |
|                    |        |              |              |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **31,227.54 ns** |   **796.057 ns** |  **43.635 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 28,707.62 ns | 1,949.413 ns | 106.854 ns |  0.92 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | RowCount | BatchCount | Mean        | Error        | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-------------:|----------:|------:|--------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **355.67 μs** |   **154.846 μs** |  **8.488 μs** |  **1.00** |    **0.03** |      **-** |     **576 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   289.28 μs |    22.792 μs |  1.249 μs |  0.81 |    0.02 |      - |    3680 B |        6.39 |
| SparseFit             | 10000    | 10         |    20.78 μs |     0.834 μs |  0.046 μs |  0.06 |    0.00 | 0.0305 |     576 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 10         |   236.39 μs |     5.885 μs |  0.323 μs |  0.66 |    0.01 |      - |     368 B |        0.64 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **351.84 μs** |     **6.508 μs** |  **0.357 μs** |  **1.00** |    **0.00** |      **-** |     **576 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   476.03 μs |     8.012 μs |  0.439 μs |  1.35 |    0.00 | 1.9531 |   36800 B |       63.89 |
| SparseFit             | 10000    | 100        |    20.83 μs |     0.791 μs |  0.043 μs |  0.06 |    0.00 | 0.0305 |     576 B |        1.00 |
| DenseFitOfTheSameData | 10000    | 100        |   236.17 μs |     6.178 μs |  0.339 μs |  0.67 |    0.00 |      - |     368 B |        0.64 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **3,489.79 μs** |    **23.410 μs** |  **1.283 μs** |  **1.00** |    **0.00** |      **-** |     **579 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 2,976.16 μs |    63.952 μs |  3.505 μs |  0.85 |    0.00 |      - |    3683 B |        6.36 |
| SparseFit             | 100000   | 10         |   203.28 μs |     4.532 μs |  0.248 μs |  0.06 |    0.00 |      - |     576 B |        0.99 |
| DenseFitOfTheSameData | 100000   | 10         | 2,404.91 μs | 1,381.622 μs | 75.731 μs |  0.69 |    0.02 |      - |     371 B |        0.64 |
|                       |          |            |             |              |           |       |         |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **3,530.27 μs** |   **417.323 μs** | **22.875 μs** |  **1.00** |    **0.01** |      **-** |     **579 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 3,791.45 μs |   132.486 μs |  7.262 μs |  1.07 |    0.01 |      - |   36803 B |       63.56 |
| SparseFit             | 100000   | 100        |   204.25 μs |    50.623 μs |  2.775 μs |  0.06 |    0.00 |      - |     576 B |        0.99 |
| DenseFitOfTheSameData | 100000   | 100        | 2,369.32 μs |   255.349 μs | 13.997 μs |  0.67 |    0.01 |      - |     371 B |        0.64 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **7.587 μs** |  **0.1331 μs** | **0.0073 μs** |         **-** |
| EqualLength | 65           |     2.261 μs |  0.0419 μs | 0.0023 μs |         - |
| **Embedded**    | **128**          |    **28.086 μs** |  **0.3158 μs** | **0.0173 μs** |         **-** |
| EqualLength | 128          |     4.253 μs |  0.1824 μs | 0.0100 μs |         - |
| **Embedded**    | **512**          | **1,109.178 μs** | **26.5533 μs** | **1.4555 μs** |         **-** |
| EqualLength | 512          |    36.885 μs |  0.8468 μs | 0.0464 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 |  8.256 ms | 0.2276 ms | 0.0125 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 13.047 ms | 0.4812 ms | 0.0264 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean       | Error       | StdDev     | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |-----------:|------------:|-----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |   3.214 ms |   0.6439 ms |  0.0353 ms | 117.1875 | 113.2813 |  35.1563 |  3711.57 KB |
| TokenizerJsonWordPiece |   8.900 ms |   4.3348 ms |  0.2376 ms | 187.5000 | 171.8750 |  46.8750 |   5852.4 KB |
| TokenizerJsonUnigram   |   8.706 ms |   0.7796 ms |  0.0427 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |   3.142 ms |   1.0594 ms |  0.0581 ms | 117.1875 | 113.2813 |  35.1563 |  3440.06 KB |
| TfidfSave              |   1.376 ms |   0.4614 ms |  0.0253 ms |  21.4844 |  15.6250 |  15.6250 |  2137.04 KB |
| TfidfLoad              |   3.375 ms |   0.0910 ms |  0.0050 ms |  85.9375 |  78.1250 |  23.4375 |  2930.47 KB |
| EmbeddingIndexSave     |   3.096 ms |   0.4909 ms |  0.0269 ms | 199.2188 | 195.3125 | 195.3125 | 20349.83 KB |
| EmbeddingIndexLoad     |   3.743 ms |   0.5251 ms |  0.0288 ms | 195.3125 | 160.1563 | 128.9063 | 16094.44 KB |
| EmbeddingIndexSaveFile | 111.878 ms | 237.9923 ms | 13.0452 ms |        - |        - |        - |   323.67 KB |
| EmbeddingIndexLoadGzip |  59.279 ms |  45.4789 ms |  2.4929 ms |        - |        - |        - | 16095.18 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 11.56 ms | 2.225 ms | 0.122 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 49.63 ms | 3.489 ms | 0.191 ms | 454.5455 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** |  **7,622.31 μs** | **572.028 μs** | **31.355 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.28 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 12,633.04 μs | 216.908 μs | 11.889 μs |  1.66 | 187.5000 | 187.5000 | 187.5000 | 628.54 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **111.16 μs** |   **4.440 μs** |  **0.243 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    156.98 μs |  29.342 μs |  1.608 μs |  1.41 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **2,519.78 μs** |  **96.136 μs** |  **5.270 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,220.19 μs |  46.592 μs |  2.554 μs |  0.88 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **18.86 μs** |   **0.178 μs** |  **0.010 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     21.05 μs |   0.599 μs |  0.033 μs |  1.12 |   0.0916 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|--------:|----------:|------------:|
| **Extract**    | **1**     | **5.540 ms** | **0.0908 ms** | **0.0050 ms** |  **1.00** |    **0.00** |     **116 B** |        **1.00** |
| ExtractOne | 1     | 5.701 ms | 2.3877 ms | 0.1309 ms |  1.03 |    0.02 |         - |        0.00 |
|            |       |          |           |           |       |         |           |             |
| **Extract**    | **5**     | **5.503 ms** | **0.2368 ms** | **0.0130 ms** |  **1.00** |    **0.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 5.617 ms | 0.0364 ms | 0.0020 ms |  1.02 |    0.00 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Q | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **3.859 μs** | **0.4448 μs** | **0.0244 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **4.759 μs** | **0.4030 μs** | **0.0221 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 31.556 ms | 0.6079 ms | 0.0333 ms |       - |       - |       - |  800342 B |
| NdcgIgnoringTies                  | 100000 | 29.680 ms | 1.6160 ms | 0.0886 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 18.414 ms | 0.5069 ms | 0.0278 ms |       - |       - |       - |  800215 B |
| ReciprocalRankScore               | 100000 | 15.583 ms | 0.3645 ms | 0.0200 ms |       - |       - |       - |     175 B |
| CoverageErrorScore                | 100000 |  5.132 ms | 0.4309 ms | 0.0236 ms | 15.6250 | 15.6250 | 15.6250 |  800271 B |
| LabelRankingAveragePrecisionScore | 100000 | 45.692 ms | 1.1195 ms | 0.0614 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                   | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Similarity_Containment**   | **64**     |     **305.2 ns** |      **1.20 ns** |   **0.07 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   3,713.0 ns |     12.90 ns |   0.71 ns | 12.17 |    0.00 |         - |          NA |
|                          |        |              |              |           |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **28,199.0 ns** |    **383.85 ns** |  **21.04 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 453,807.9 ns | 14,322.71 ns | 785.08 ns | 16.09 |    0.03 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Mean        | Error        | StdDev     | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|-----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **36.92 μs** |     **0.081 μs** |   **0.004 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    36.46 μs |     2.298 μs |   0.126 μs |        - |        - |        - |         - |
| R2Score  | 100000  |    95.01 μs |     2.158 μs |   0.118 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   414.57 μs |   182.481 μs |  10.002 μs | 199.7070 | 199.7070 | 199.7070 |  800186 B |
| **Mse**      | **1000000** |   **369.11 μs** |    **16.923 μs** |   **0.928 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   364.71 μs |     4.853 μs |   0.266 μs |        - |        - |        - |         - |
| R2Score  | 1000000 |   953.12 μs |   107.046 μs |   5.868 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 4,789.80 μs | 5,268.832 μs | 288.802 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **20.38 ms** | **0.181 ms** | **0.010 ms** | **93.7500** | **93.7500** | **93.7500** | **359.63 KB** |
| **Fit**    | **500x20000x5**  | **34.55 ms** | **3.523 ms** | **0.193 ms** |       **-** |       **-** |       **-** | **336.14 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-EKQVZA : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_MinMax**                   | **Job-EKQVZA** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |     **82.77 μs** |    **10.529 μs** |    **11.703 μs** |     **74.40 μs** |  **1.02** |    **0.19** |        **-** |        **-** |        **-** |   **79.59 KB** |        **1.00** |
| Lodestar_MaxAbs                   | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |     81.47 μs |     6.316 μs |     6.203 μs |     78.35 μs |  1.00 |    0.14 |        - |        - |        - |   79.14 KB |        0.99 |
| Lodestar_Robust                   | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,904.10 μs |   319.014 μs |   367.377 μs |  1,628.14 μs | 23.39 |    5.27 |        - |        - |        - |  157.51 KB |        1.98 |
| Lodestar_Standard                 | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |    116.74 μs |     6.986 μs |     7.175 μs |    115.36 μs |  1.43 |    0.19 |        - |        - |        - |   79.38 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |    303.16 μs |     3.737 μs |     3.999 μs |    303.76 μs |  3.72 |    0.46 |        - |        - |        - |     9.3 KB |        0.12 |
| MlNet_NormalizeMinMax_Read        | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,547.01 μs |    45.873 μs |    50.987 μs |  1,533.93 μs | 19.01 |    2.39 |        - |        - |        - |  324.96 KB |        4.08 |
| MlNet_NormalizeRobustScaling_Read | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 1000     |  4,308.31 μs |   111.075 μs |   123.460 μs |  4,280.01 μs | 52.93 |    6.61 |        - |        - |        - |  451.34 KB |        5.67 |
|                                   |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     33.96 μs |    10.225 μs |     0.560 μs |     34.14 μs |  1.00 |    0.02 |   4.7607 |        - |        - |   78.87 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     34.26 μs |     1.350 μs |     0.074 μs |     34.25 μs |  1.01 |    0.01 |   4.7607 |        - |        - |    78.4 KB |        0.99 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    245.77 μs |   128.149 μs |     7.024 μs |    249.02 μs |  7.24 |    0.21 |   9.5215 |        - |        - |  156.79 KB |        1.99 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     39.63 μs |     1.022 μs |     0.056 μs |     39.60 μs |  1.17 |    0.02 |   4.7607 |        - |        - |   78.45 KB |        0.99 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,184.07 μs | 3,476.011 μs |   190.532 μs |  1,197.03 μs | 34.87 |    4.89 |  83.4961 |   7.3242 |        - |  1366.2 KB |       17.32 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    809.39 μs |   979.588 μs |    53.695 μs |    816.64 μs | 23.84 |    1.41 |  39.0625 |   1.9531 |        - |  637.71 KB |        8.09 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,302.35 μs |   185.274 μs |    10.155 μs |  1,301.73 μs | 38.35 |    0.61 |  46.8750 |   1.9531 |        - |   776.4 KB |        9.84 |
|                                   |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_MinMax**                   | **Job-EKQVZA** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,080.31 μs** |     **8.180 μs** |     **8.034 μs** |  **1,078.75 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1563.96 KB** |       **1.000** |
| Lodestar_MaxAbs                   | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,070.48 μs |    12.070 μs |    12.915 μs |  1,067.46 μs |  0.99 |    0.01 |        - |        - |        - | 1563.52 KB |       1.000 |
| Lodestar_Robust                   | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    | 13,615.79 μs |    51.484 μs |    57.225 μs | 13,601.08 μs | 12.60 |    0.10 |        - |        - |        - | 3126.26 KB |       1.999 |
| Lodestar_Standard                 | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,295.94 μs |    31.880 μs |    36.714 μs |  1,277.84 μs |  1.20 |    0.03 |        - |        - |        - | 1564.03 KB |       1.000 |
| MlNet_NormalizeMinMax_Fit         | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    |  4,816.20 μs |    34.188 μs |    36.581 μs |  4,810.10 μs |  4.46 |    0.05 |        - |        - |        - |     9.3 KB |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    | 16,934.03 μs | 3,012.898 μs | 3,348.829 μs | 18,916.55 μs | 15.68 |    3.02 |        - |        - |        - |  470.78 KB |       0.301 |
| MlNet_NormalizeRobustScaling_Read | Job-EKQVZA | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,571.51 μs | 1,387.950 μs | 1,485.090 μs | 16,858.70 μs | 16.27 |    1.34 |        - |        - |        - | 4179.85 KB |       2.673 |
|                                   |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    948.86 μs |   180.556 μs |     9.897 μs |    943.71 μs |  1.00 |    0.01 | 332.0313 | 332.0313 | 332.0313 | 1564.29 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    908.84 μs | 1,227.925 μs |    67.307 μs |    884.78 μs |  0.96 |    0.06 | 332.0313 | 332.0313 | 332.0313 | 1562.98 KB |        1.00 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 12,679.15 μs |   517.926 μs |    28.389 μs | 12,664.47 μs | 13.36 |    0.12 | 890.6250 | 890.6250 | 890.6250 | 3126.11 KB |        2.00 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,093.49 μs |   313.544 μs |    17.186 μs |  1,084.01 μs |  1.15 |    0.02 | 332.0313 | 332.0313 | 332.0313 | 1563.04 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,273.45 μs | 1,698.966 μs |    93.126 μs |  1,274.93 μs |  1.34 |    0.09 |  41.0156 |   6.8359 |        - |  686.19 KB |        0.44 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,475.23 μs | 3,050.763 μs |   167.223 μs |  3,400.66 μs |  3.66 |    0.16 |  31.2500 |        - |        - |  583.51 KB |        0.37 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 16,697.22 μs |    98.408 μs |     5.394 μs | 16,699.38 μs | 17.60 |    0.16 | 250.0000 | 125.0000 |        - |  4198.2 KB |        2.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Model   | Mean     | Error   | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|--------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **116.2 ms** | **7.34 ms** | **0.40 ms** | **2200.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **111.6 ms** | **0.50 ms** | **0.03 ms** | **2200.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **20.00 ms** | **0.254 ms** | **0.014 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **135.20 ms** | **3.656 ms** | **0.200 ms** |       **-** |       **-** |       **-** | **371.54 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|--------------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**        | **500**       | **64**           |  **43.518 ms** |  **0.5563 ms** | **0.0305 ms** |  **1.00** |  **416.6667** |         **-** |        **-** |   **8152.38 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |   9.937 ms |  2.3448 ms | 0.1285 ms |  0.23 |  171.8750 |  156.2500 |  78.1250 |   2492.48 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |   6.844 ms |  0.0586 ms | 0.0032 ms |  0.16 |   15.6250 |         - |        - |    304.73 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   5.349 ms |  2.8745 ms | 0.1576 ms |  0.12 |   15.6250 |         - |        - |    305.28 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |   8.205 ms |  0.5898 ms | 0.0323 ms |  0.19 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **43.781 ms** |  **0.9152 ms** | **0.0502 ms** |  **1.00** |  **416.6667** |         **-** |        **-** |   **8152.38 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  11.349 ms |  0.2115 ms | 0.0116 ms |  0.26 |  312.5000 |  312.5000 | 218.7500 |   4538.23 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |   8.127 ms |  1.0128 ms | 0.0555 ms |  0.19 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   5.391 ms |  0.2189 ms | 0.0120 ms |  0.12 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |   9.411 ms |  0.9313 ms | 0.0511 ms |  0.21 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **759.464 ms** | **42.7622 ms** | **2.3439 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  42.137 ms |  7.6715 ms | 0.4205 ms |  0.06 |  916.6667 |  916.6667 | 500.0000 |  10164.89 KB |       0.080 |
| SignaturesOnly       | 2000      | 64           |  31.833 ms |  1.5635 ms | 0.0857 ms |  0.04 |   62.5000 |         - |        - |   1218.83 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  26.030 ms |  0.9989 ms | 0.0548 ms |  0.03 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  32.500 ms |  0.2471 ms | 0.0135 ms |  0.04 |   62.5000 |         - |        - |   1718.79 KB |       0.014 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **756.081 ms** | **29.9111 ms** | **1.6395 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126359.81 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  52.502 ms | 46.7318 ms | 2.5615 ms |  0.07 | 1600.0000 | 1600.0000 | 800.0000 |  18495.35 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  32.948 ms |  0.6737 ms | 0.0369 ms |  0.04 |   62.5000 |         - |        - |   1718.83 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  21.873 ms |  1.0206 ms | 0.0559 ms |  0.03 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  32.471 ms |  0.6582 ms | 0.0361 ms |  0.04 |   62.5000 |         - |        - |   1718.79 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                          | SampleCount | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **70.407 μs** |    **10.436 μs** |   **0.5720 μs** |  **1.00** |    **0.01** |  **16.6016** |   **6.5918** |        **-** |  **273.94 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    126.278 μs |    79.379 μs |   4.3510 μs |  1.79 |    0.05 |  19.0430 |   4.6387 |        - |  313.41 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      3.926 μs |     2.835 μs |   0.1554 μs |  0.06 |    0.00 |   2.3918 |   0.2632 |        - |   39.14 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    205.362 μs |    19.315 μs |   1.0587 μs |  2.92 |    0.02 |   2.4414 |   0.9766 |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  4,458.909 μs |   398.869 μs |  21.8633 μs | 63.33 |    0.52 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     38.069 μs |     2.632 μs |   0.1443 μs |  0.54 |    0.00 |   0.7324 |   0.3052 |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |    803.671 μs |   107.405 μs |   5.8872 μs | 11.42 |    0.11 |   1.9531 |        - |        - |   34.26 KB |        0.13 |
|                                 |             |               |              |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |    **893.640 μs** |   **200.033 μs** |  **10.9645 μs** |  **1.00** |    **0.02** | **430.6641** | **416.0156** | **406.2500** | **2738.21 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  1,640.397 μs |   821.548 μs |  45.0318 μs |  1.84 |    0.05 | 542.9688 | 527.3438 | 519.5313 |  3130.1 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     65.525 μs |    41.942 μs |   2.2990 μs |  0.07 |    0.00 |  49.3164 |  49.3164 |  49.3164 |   391.1 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    201.009 μs |    14.531 μs |   0.7965 μs |  0.22 |    0.00 |   2.4414 |   0.7324 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 34,169.612 μs |   227.732 μs |  12.4827 μs | 38.24 |    0.41 |        - |        - |        - |  148.72 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     37.745 μs |     3.138 μs |   0.1720 μs |  0.04 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  6,031.143 μs | 2,491.754 μs | 136.5815 μs |  6.75 |    0.15 |        - |        - |        - |   34.17 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **3.969 ms** | **1.6106 ms** | **0.0883 ms** |  **1.00** |    **0.03** | **109.3750** | **109.3750** | **109.3750** | **1366.79 KB** |        **1.00** |
| CountWithStopWords   | 200       |  3.439 ms | 1.3337 ms | 0.0731 ms |  0.87 |    0.02 |  42.9688 |  19.5313 |        - |  725.83 KB |        0.53 |
| Hashing              | 200       |  4.004 ms | 0.7816 ms | 0.0428 ms |  1.01 |    0.02 |  70.3125 |  70.3125 |  70.3125 | 1051.16 KB |        0.77 |
| HashingWithStopWords | 200       |  3.608 ms | 0.6974 ms | 0.0382 ms |  0.91 |    0.02 |  31.2500 |  15.6250 |        - |   591.9 KB |        0.43 |
|                      |           |           |           |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **12.830 ms** | **0.8305 ms** | **0.0455 ms** |  **1.00** |    **0.00** | **828.1250** | **828.1250** | **828.1250** | **5880.78 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 11.057 ms | 1.7292 ms | 0.0948 ms |  0.86 |    0.01 | 375.0000 | 375.0000 | 375.0000 | 3155.02 KB |        0.54 |
| Hashing              | 1000      | 13.246 ms | 1.3686 ms | 0.0750 ms |  1.03 |    0.01 | 640.6250 | 546.8750 | 546.8750 | 4793.32 KB |        0.82 |
| HashingWithStopWords | 1000      | 12.101 ms | 2.3965 ms | 0.1314 ms |  0.94 |    0.01 | 265.6250 | 265.6250 | 265.6250 | 2633.78 KB |        0.45 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Words | Mean      | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|-------- |------ |----------:|---------:|---------:|----------:|----------:|---------:|----------:|
| **Extract** | **2000**  |  **12.41 ms** | **0.050 ms** | **0.003 ms** |  **187.5000** |  **125.0000** |        **-** |   **3.13 MB** |
| **Extract** | **8000**  | **245.24 ms** | **8.862 ms** | **0.486 ms** | **2000.0000** | **1500.0000** | **500.0000** |  **35.34 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **27.26 ms** |  **1.974 ms** | **0.108 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  42.58 ms | 10.898 ms | 0.597 ms |  1.56 |    0.02 |  166.6667 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **36.36 ms** |  **0.130 ms** | **0.007 ms** |  **1.00** |    **0.00** |  **285.7143** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  45.06 ms |  2.297 ms | 0.126 ms |  1.24 |    0.00 |  166.6667 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **66.05 ms** | **17.356 ms** | **0.951 ms** |  **1.00** |    **0.02** | **1750.0000** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 200.99 ms | 23.984 ms | 1.315 ms |  3.04 |    0.04 | 3666.6667 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean     | Error    | StdDev   | Allocated |
|------- |-------- |---------:|---------:|---------:|----------:|
| **TopTwo** | **10**      | **10.65 ms** | **1.219 ms** | **0.067 ms** |         **-** |
| **TopTwo** | **100**     | **81.58 ms** | **0.117 ms** | **0.006 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **39.54 ns** |  **1.294 ns** | **0.071 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  37.22 ns |  0.629 ns | 0.034 ns |  0.94 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **768**  |  **76.78 ns** |  **1.339 ns** | **0.073 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  70.60 ns |  1.016 ns | 0.056 ns |  0.92 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **1024** | **102.68 ns** |  **0.120 ns** | **0.007 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 |  97.73 ns | 15.396 ns | 0.844 ns |  0.95 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |  **1.889 ms** | **0.0959 ms** | **0.0053 ms** |  **1.00** |    **0.00** |   **15.6250** |   **11.7188** |         **-** |      **303 KB** |        **1.00** |
| Tfidf                 | 200       |  1.902 ms | 0.2528 ms | 0.0139 ms |  1.01 |    0.01 |   19.5313 |   15.6250 |         - |   332.14 KB |        1.10 |
| CountBigrams          | 200       |  2.244 ms | 0.1339 ms | 0.0073 ms |  1.19 |    0.00 |   35.1563 |   23.4375 |         - |   590.34 KB |        1.95 |
| CountCharWordBoundary | 200       |  1.707 ms | 0.0853 ms | 0.0047 ms |  0.90 |    0.00 |  382.8125 |  382.8125 |  382.8125 |   1685.7 KB |        5.56 |
| Hashing               | 200       |  1.887 ms | 0.0427 ms | 0.0023 ms |  1.00 |    0.00 |   11.7188 |    7.8125 |         - |   233.65 KB |        0.77 |
|                       |           |           |           |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **3.577 ms** | **0.1250 ms** | **0.0068 ms** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |  **1280.03 KB** |        **1.00** |
| Tfidf                 | 1000      |  3.681 ms | 0.1914 ms | 0.0105 ms |  1.03 |    0.00 |  140.6250 |  140.6250 |  140.6250 |  1424.43 KB |        1.11 |
| CountBigrams          | 1000      |  5.213 ms | 0.3188 ms | 0.0175 ms |  1.46 |    0.00 |  398.4375 |  398.4375 |  398.4375 |  2291.91 KB |        1.79 |
| CountCharWordBoundary | 1000      | 10.942 ms | 1.2774 ms | 0.0700 ms |  3.06 |    0.02 | 1046.8750 | 1015.6250 | 1015.6250 | 12022.21 KB |        9.39 |
| Hashing               | 1000      |  3.594 ms | 0.3802 ms | 0.0208 ms |  1.00 |    0.01 |   70.3125 |   70.3125 |   70.3125 |  1015.48 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **4.156 ms** |   **0.1867 ms** |  **0.0102 ms** |  **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |   **2.01 MB** |        **1.00** |
| MlNet    | 200       |  39.827 ms |  45.2211 ms |  2.4787 ms |  9.58 |    0.52 |  7333.3333 |  7333.3333 |  7333.3333 |  28.26 MB |       14.09 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **16.436 ms** |   **0.1903 ms** |  **0.0104 ms** |  **1.00** |    **0.00** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **9.17 MB** |        **1.00** |
| MlNet    | 1000      | 326.161 ms | 195.7055 ms | 10.7273 ms | 19.84 |    0.57 | 77000.0000 | 77000.0000 | 77000.0000 | 324.41 MB |       35.37 |

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.301 | 2.258 | 7.50x | 0.301 | 2.257 | 7.50x |
| glm_gamma_n1000 | 0.359 | 2.969 | 8.28x | 0.359 | 2.968 | 8.28x |
| glm_poisson_exposure_n1000 | 0.307 | 2.639 | 8.60x | 0.307 | 2.639 | 8.60x |
| mnlogit_n1000 | 0.639 | 10.412 | 16.29x | 0.639 | 10.411 | 16.29x |
| glm_negative_binomial_n10000 | 2.768 | 8.164 | 2.95x | 2.775 | 8.163 | 2.94x |
| glm_gamma_n10000 | 3.127 | 9.597 | 3.07x | 3.135 | 9.596 | 3.06x |
| glm_poisson_exposure_n10000 | 3.325 | 9.343 | 2.81x | 3.332 | 9.340 | 2.80x |
| mnlogit_n10000 | 6.423 | 56.471 | 8.79x | 6.429 | 56.468 | 8.78x |
| glm_negative_binomial_n100000 | 27.827 | 93.296 | 3.35x | 28.129 | 372.810 | 13.25x |
| glm_gamma_n100000 | 32.229 | 90.717 | 2.81x | 32.433 | 361.764 | 11.15x |
| glm_poisson_exposure_n100000 | 41.895 | 85.010 | 2.03x | 42.300 | 339.125 | 8.02x |
| mnlogit_n100000 | 64.478 | 530.997 | 8.24x | 64.507 | 1118.766 | 17.34x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 83.7 | 18.4 | 4.54x C# faster |
| latin | 32 | 121.5 | 53.4 | 2.28x C# faster |
| latin | 128 | 367.8 | 315.9 | 1.16x C# faster |
| latin | 512 | 3769.8 | 4271.0 | 1.13x Py faster |
| cjk | 8 | 96.9 | 18.5 | 5.25x C# faster |
| cjk | 32 | 178.4 | 109.0 | 1.64x C# faster |
| cjk | 128 | 1586.9 | 1086.0 | 1.46x C# faster |
| cjk | 512 | 13033.7 | 7323.9 | 1.78x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 110.1 | 14.0 | 7.86x C# faster |
| latin | 32 | 208.7 | 99.0 | 2.11x C# faster |
| latin | 128 | 1410.9 | 749.5 | 1.88x C# faster |
| latin | 512 | 12081.0 | 10407.5 | 1.16x C# faster |
| cjk | 8 | 110.3 | 14.0 | 7.88x C# faster |
| cjk | 32 | 228.7 | 148.2 | 1.54x C# faster |
| cjk | 128 | 2343.7 | 1662.0 | 1.41x C# faster |
| cjk | 512 | 20303.7 | 14601.6 | 1.39x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 0.602 | 158.17x | 0.004 | 0.602 | 158.17x |
| accuracy_n1000_k2 | 0.000 | 0.313 | 3321.88x | 0.000 | 0.313 | 3320.92x |
| precision_recall_f1_macro_n1000_k2 | 0.004 | 1.086 | 296.40x | 0.004 | 1.086 | 296.37x |
| classification_report_n1000_k2 | 0.005 | 4.111 | 760.47x | 0.005 | 4.110 | 760.50x |
| roc_auc_binary_n1000_k2 | 0.013 | 1.213 | 96.31x | 0.013 | 1.213 | 96.31x |
| balanced_accuracy_n1000_k2 | 0.004 | 0.642 | 178.16x | 0.004 | 0.641 | 178.16x |
| matthews_n1000_k2 | 0.004 | 1.212 | 337.34x | 0.004 | 1.212 | 337.33x |
| cohen_kappa_n1000_k2 | 0.004 | 0.678 | 187.83x | 0.004 | 0.678 | 187.83x |
| mse_n1000_k2 | 0.000 | 0.167 | 412.31x | 0.000 | 0.167 | 412.32x |
| mae_n1000_k2 | 0.000 | 0.166 | 419.73x | 0.000 | 0.166 | 419.70x |
| median_ae_n1000_k2 | 0.004 | 0.179 | 46.06x | 0.004 | 0.179 | 46.05x |
| r2_n1000_k2 | 0.001 | 0.211 | 204.36x | 0.001 | 0.211 | 204.35x |
| confusion_matrix_n1000_k10 | 0.004 | 0.602 | 152.94x | 0.004 | 0.601 | 152.91x |
| accuracy_n1000_k10 | 0.000 | 0.315 | 3261.01x | 0.000 | 0.315 | 3261.15x |
| precision_recall_f1_macro_n1000_k10 | 0.004 | 1.101 | 290.01x | 0.004 | 1.100 | 290.00x |
| classification_report_n1000_k10 | 0.008 | 4.243 | 513.84x | 0.008 | 4.243 | 513.87x |
| roc_auc_ovr_macro_n1000_k10 | 0.148 | 6.326 | 42.75x | 0.148 | 6.325 | 42.75x |
| balanced_accuracy_n1000_k10 | 0.004 | 0.641 | 165.75x | 0.004 | 0.641 | 165.75x |
| matthews_n1000_k10 | 0.004 | 1.242 | 323.18x | 0.004 | 1.242 | 323.16x |
| cohen_kappa_n1000_k10 | 0.004 | 0.682 | 163.68x | 0.004 | 0.682 | 163.67x |
| mse_n1000_k10 | 0.000 | 0.166 | 410.24x | 0.000 | 0.166 | 410.18x |
| mae_n1000_k10 | 0.000 | 0.166 | 418.49x | 0.000 | 0.166 | 418.47x |
| median_ae_n1000_k10 | 0.004 | 0.178 | 46.88x | 0.004 | 0.178 | 46.88x |
| r2_n1000_k10 | 0.001 | 0.210 | 203.26x | 0.001 | 0.210 | 203.26x |
| confusion_matrix_n100000_k2 | 0.369 | 8.534 | 23.15x | 0.369 | 8.532 | 23.15x |
| accuracy_n100000_k2 | 0.010 | 2.936 | 303.41x | 0.010 | 2.936 | 303.43x |
| precision_recall_f1_macro_n100000_k2 | 0.345 | 9.764 | 28.29x | 0.345 | 9.763 | 28.29x |
| classification_report_n100000_k2 | 0.348 | 20.846 | 59.90x | 0.348 | 20.846 | 59.90x |
| roc_auc_binary_n100000_k2 | 2.330 | 22.332 | 9.58x | 2.330 | 22.328 | 9.58x |
| balanced_accuracy_n100000_k2 | 0.345 | 8.591 | 24.91x | 0.345 | 8.590 | 24.91x |
| matthews_n100000_k2 | 0.345 | 17.227 | 49.95x | 0.345 | 17.226 | 49.95x |
| cohen_kappa_n100000_k2 | 0.345 | 8.629 | 25.00x | 0.345 | 8.629 | 25.00x |
| mse_n100000_k2 | 0.037 | 0.287 | 7.79x | 0.037 | 0.287 | 7.79x |
| mae_n100000_k2 | 0.036 | 0.287 | 7.90x | 0.036 | 0.287 | 7.90x |
| median_ae_n100000_k2 | 0.459 | 1.468 | 3.20x | 0.477 | 1.468 | 3.08x |
| r2_n100000_k2 | 0.095 | 0.451 | 4.75x | 0.095 | 0.451 | 4.75x |
| confusion_matrix_n100000_k10 | 0.323 | 8.507 | 26.36x | 0.323 | 8.505 | 26.36x |
| accuracy_n100000_k10 | 0.009 | 2.931 | 318.21x | 0.009 | 2.931 | 318.21x |
| precision_recall_f1_macro_n100000_k10 | 0.323 | 10.320 | 31.98x | 0.323 | 10.319 | 31.98x |
| classification_report_n100000_k10 | 0.329 | 23.103 | 70.30x | 0.329 | 23.101 | 70.30x |
| roc_auc_ovr_macro_n100000_k10 | 23.956 | 184.102 | 7.69x | 23.954 | 184.087 | 7.68x |
| balanced_accuracy_n100000_k10 | 0.323 | 8.567 | 26.54x | 0.323 | 8.566 | 26.54x |
| matthews_n100000_k10 | 0.323 | 17.752 | 55.01x | 0.323 | 17.752 | 55.01x |
| cohen_kappa_n100000_k10 | 0.368 | 8.616 | 23.39x | 0.368 | 8.615 | 23.39x |
| mse_n100000_k10 | 0.037 | 0.287 | 7.77x | 0.037 | 0.287 | 7.77x |
| mae_n100000_k10 | 0.036 | 0.286 | 7.87x | 0.036 | 0.286 | 7.87x |
| median_ae_n100000_k10 | 0.467 | 1.464 | 3.13x | 0.503 | 1.464 | 2.91x |
| r2_n100000_k10 | 0.095 | 0.449 | 4.72x | 0.095 | 0.448 | 4.72x |
| confusion_matrix_n1000000_k2 | 3.455 | 80.990 | 23.44x | 3.454 | 80.984 | 23.45x |
| accuracy_n1000000_k2 | 0.098 | 27.127 | 277.34x | 0.098 | 27.123 | 277.35x |
| precision_recall_f1_macro_n1000000_k2 | 3.458 | 88.675 | 25.64x | 3.458 | 88.663 | 25.64x |
| classification_report_n1000000_k2 | 3.459 | 173.515 | 50.17x | 3.459 | 173.499 | 50.16x |
| roc_auc_binary_n1000000_k2 | 40.580 | 249.476 | 6.15x | 40.574 | 249.454 | 6.15x |
| balanced_accuracy_n1000000_k2 | 3.456 | 81.300 | 23.53x | 3.456 | 81.288 | 23.52x |
| matthews_n1000000_k2 | 3.457 | 164.071 | 47.46x | 3.457 | 164.051 | 47.46x |
| cohen_kappa_n1000000_k2 | 3.458 | 81.231 | 23.49x | 3.458 | 81.223 | 23.49x |
| mse_n1000000_k2 | 0.366 | 1.736 | 4.74x | 0.366 | 1.736 | 4.74x |
| mae_n1000000_k2 | 0.361 | 1.711 | 4.73x | 0.361 | 1.711 | 4.73x |
| median_ae_n1000000_k2 | 4.174 | 12.541 | 3.00x | 4.223 | 12.538 | 2.97x |
| r2_n1000000_k2 | 0.946 | 3.423 | 3.62x | 0.946 | 3.422 | 3.62x |
| confusion_matrix_n1000000_k10 | 3.212 | 81.006 | 25.22x | 3.212 | 80.990 | 25.22x |
| accuracy_n1000000_k10 | 0.095 | 27.152 | 287.16x | 0.095 | 27.144 | 287.12x |
| precision_recall_f1_macro_n1000000_k10 | 3.195 | 94.057 | 29.44x | 3.194 | 94.045 | 29.44x |
| classification_report_n1000000_k10 | 3.230 | 195.170 | 60.42x | 3.230 | 195.154 | 60.42x |
| balanced_accuracy_n1000000_k10 | 3.198 | 81.097 | 25.36x | 3.197 | 81.092 | 25.36x |
| matthews_n1000000_k10 | 3.187 | 169.758 | 53.27x | 3.186 | 169.738 | 53.27x |
| cohen_kappa_n1000000_k10 | 3.202 | 81.095 | 25.33x | 3.202 | 81.081 | 25.33x |
| mse_n1000000_k10 | 0.370 | 1.692 | 4.57x | 0.370 | 1.691 | 4.57x |
| mae_n1000000_k10 | 0.364 | 1.598 | 4.39x | 0.364 | 1.598 | 4.39x |
| median_ae_n1000000_k10 | 4.631 | 12.204 | 2.64x | 4.787 | 12.203 | 2.55x |
| r2_n1000000_k10 | 0.954 | 2.931 | 3.07x | 0.954 | 2.931 | 3.07x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.038 | 1.941 | 51.53x | 0.038 | 1.941 | 51.53x |
| ols_hac_n1000 | 0.135 | 2.376 | 17.54x | 0.135 | 2.375 | 17.54x |
| ols_cluster_n1000 | 0.051 | 2.414 | 47.01x | 0.051 | 2.414 | 47.01x |
| ols_summary_n10000 | 0.363 | 8.747 | 24.09x | 0.363 | 8.746 | 24.09x |
| ols_hac_n10000 | 1.393 | 9.825 | 7.05x | 1.399 | 9.824 | 7.02x |
| ols_cluster_n10000 | 0.495 | 10.139 | 20.48x | 0.498 | 10.137 | 20.36x |
| ols_summary_n100000 | 3.670 | 85.319 | 23.25x | 3.685 | 339.871 | 92.24x |
| ols_hac_n100000 | 13.910 | 96.797 | 6.96x | 14.144 | 386.541 | 27.33x |
| ols_cluster_n100000 | 5.466 | 98.075 | 17.94x | 5.659 | 390.813 | 69.06x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.045 | 8.173 | 2.02x | 4.279 | 8.172 | 1.91x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 9.954 | 13.531 | 1.36x | 10.234 | 13.530 | 1.32x | 706,526 | 706,526 |
| tokenizer_json_unigram | 9.580 | 35.472 | 3.70x | 9.882 | 35.468 | 3.59x | 1,990,038 | 1,990,038 |
| spiece_model | 4.016 | 24.336 | 6.06x | 4.199 | 24.333 | 5.80x | 533,084 | 533,084 |
| tfidf_save | 1.424 | 1.910 | 1.34x | 1.440 | 1.909 | 1.33x | 581,787 | 591,922 |
| tfidf_load | 3.884 | 3.374 | 0.87x | 4.051 | 3.374 | 0.83x | 581,787 | 591,922 |
| embedding_index_save | 3.167 | 1.274 | 0.40x | 3.393 | 1.274 | 0.38x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 76.862 | 69.787 | 0.91x | 8.306 | 4.188 | 0.50x | 20,589,007 | 15,360,128 |
| embedding_index_load | 3.618 | 1.459 | 0.40x | 3.990 | 1.459 | 0.37x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 4.356 | 0.785 | 0.18x | 4.665 | 0.785 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 2.707 | 1.450 | 0.54x | 2.885 | 1.450 | 0.50x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 0.975 | 1.459 | 1.50x | 1.178 | 1.459 | 1.24x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.000 | 80.32x | 0.000 | 0.000 | 80.32x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 351.541 | 495.345 | 1.41x | 354.005 | 495.286 | 1.40x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 57.789 | 56.359 | 0.98x | 58.034 | 56.353 | 0.97x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.068 | 0.087 | 1.28x | 0.068 | 0.087 | 1.28x |
| stratified_n10000 | 0.124 | 0.654 | 5.28x | 0.124 | 0.654 | 5.28x |
| traintest_n10000 | 0.004 | 0.136 | 36.18x | 0.004 | 0.136 | 36.18x |
| kfold_n100000 | 0.945 | 2.345 | 2.48x | 1.059 | 2.345 | 2.22x |
| stratified_n100000 | 1.432 | 6.975 | 4.87x | 1.538 | 6.974 | 4.53x |
| traintest_n100000 | 0.162 | 0.296 | 1.83x | 0.163 | 0.296 | 1.82x |
| kfold_n1000000 | 8.325 | 15.654 | 1.88x | 8.744 | 15.653 | 1.79x |
| stratified_n1000000 | 14.218 | 57.327 | 4.03x | 14.773 | 57.319 | 3.88x |
| traintest_n1000000 | 0.537 | 1.744 | 3.25x | 0.632 | 1.743 | 2.76x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.487 | 138.94x | 0.004 | 0.487 | 138.93x |
| mann_whitney_n1000 | 0.024 | 0.435 | 17.99x | 0.024 | 0.435 | 17.99x |
| chi_square_n1000 | 0.000 | 0.194 | 1366.80x | 0.000 | 0.194 | 1366.74x |
| welch_t_n10000 | 0.033 | 0.519 | 15.80x | 0.033 | 0.519 | 15.80x |
| mann_whitney_n10000 | 0.374 | 2.266 | 6.05x | 0.374 | 2.265 | 6.05x |
| chi_square_n10000 | 0.001 | 0.197 | 191.25x | 0.001 | 0.197 | 191.24x |
| welch_t_n100000 | 0.328 | 0.849 | 2.59x | 0.328 | 0.849 | 2.59x |
| mann_whitney_n100000 | 4.737 | 23.815 | 5.03x | 4.737 | 23.812 | 5.03x |
| chi_square_n100000 | 0.011 | 0.210 | 18.38x | 0.011 | 0.210 | 18.38x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-09-17, measured at commit `a5f400484df38ca0ec04247446d4e85df7013528`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.034 | 1.914 | 56.87x | 0.034 | 1.914 | 56.87x |
| var_n10000 | 0.387 | 13.462 | 34.74x | 0.389 | 13.460 | 34.56x |
| var_n100000 | 3.457 | 120.346 | 34.81x | 3.556 | 120.328 | 33.84x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
