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

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Mean      | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          | 18.454 ns |  3.6493 ns | 0.2000 ns |  1.00 |    0.01 |         - |          NA |
| ChiSquaredSfFourDf         |  6.992 ns |  0.2986 ns | 0.0164 ns |  0.38 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail | 24.646 ns |  8.5655 ns | 0.4695 ns |  1.34 |    0.03 |         - |          NA |
| ChiSquaredSfHundredDf      | 53.795 ns | 15.3009 ns | 0.8387 ns |  2.92 |    0.05 |         - |          NA |
| ChiSquaredSfFractionalDf   | 62.592 ns | 30.8356 ns | 1.6902 ns |  3.39 |    0.09 |         - |          NA |
| NormalQuantile             | 94.382 ns |  3.8566 ns | 0.2114 ns |  5.11 |    0.05 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlsBenchmarks-report-github

_As of 2026-09-18, measured at commit `a891e078a2dc2531149eba169d905639dfedd3af`._

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
| **Lodestar_Gls** | **50**         |      **23.43 μs** |      **1.018 μs** |     **0.056 μs** |  **1.00** |    **0.00** |   **1.6479** |   **0.0916** |        **-** |   **27.04 KB** |        **1.00** |
| MathNet_Gls  | 50         |      46.77 μs |     10.242 μs |     0.561 μs |  2.00 |    0.02 |   2.0142 |   0.1221 |        - |   33.28 KB |        1.23 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **200**        |     **490.46 μs** |     **73.623 μs** |     **4.036 μs** |  **1.00** |    **0.01** |  **99.6094** |  **99.6094** |  **99.6094** |  **336.51 KB** |        **1.00** |
| MathNet_Gls  | 200        |   1,430.01 μs |    191.813 μs |    10.514 μs |  2.92 |    0.03 |  99.6094 |  99.6094 |  99.6094 |  370.28 KB |        1.10 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **500**        |   **4,662.04 μs** |    **478.215 μs** |    **26.213 μs** |  **1.00** |    **0.01** | **320.3125** | **320.3125** | **320.3125** | **2010.15 KB** |        **1.00** |
| MathNet_Gls  | 500        |  17,614.83 μs |  3,003.655 μs |   164.640 μs |  3.78 |    0.04 | 281.2500 | 281.2500 | 281.2500 | 2229.28 KB |        1.11 |
|              |            |               |               |              |       |         |          |          |          |            |             |
| **Lodestar_Gls** | **1000**       |  **39,279.21 μs** |  **3,035.624 μs** |   **166.393 μs** |  **1.00** |    **0.01** | **153.8462** | **153.8462** | **153.8462** | **7924.13 KB** |        **1.00** |
| MathNet_Gls  | 1000       | 118,592.97 μs | 75,289.780 μs | 4,126.887 μs |  3.02 |    0.09 |        - |        - |        - | 8856.15 KB |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KsAutoBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | SampleSize | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **55.39 μs** |   **5.156 μs** |  **0.283 μs** |  **1.00** | **0.1831** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    52.08 μs |  11.331 μs |  0.621 μs |  0.94 | 0.1831 |  15.67 KB |        1.00 |
|                   |            |             |            |           |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,517.53 μs** |  **39.269 μs** |  **2.152 μs** |  **1.00** |      **-** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,264.22 μs | 244.318 μs | 13.392 μs |  0.83 |      - |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                         | SampleSize | Mean            | Error            | StdDev        | Ratio   | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|-----------------:|--------------:|--------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **539.99 ns** |         **4.945 ns** |      **0.271 ns** |    **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,702.48 ns |        36.437 ns |      1.997 ns |    3.15 |    0.00 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,193.56 ns |        68.850 ns |      3.774 ns |    4.06 |    0.01 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,251.33 ns |       871.926 ns |     47.793 ns |    6.02 |    0.08 | 0.0114 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     5,437.69 ns |       558.770 ns |     30.628 ns |   10.07 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     7,032.80 ns |     2,617.391 ns |    143.468 ns |   13.02 |    0.23 | 0.0153 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,163.05 ns |       159.473 ns |      8.741 ns |    4.01 |    0.01 | 0.0191 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,243.11 ns |     1,222.371 ns |     67.002 ns |    6.01 |    0.11 | 0.0114 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       571.19 ns |        15.745 ns |      0.863 ns |    1.06 |    0.00 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,345.64 ns |       120.530 ns |      6.607 ns |    4.34 |    0.01 | 0.0076 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       988.28 ns |        15.936 ns |      0.874 ns |    1.83 |    0.00 | 0.0095 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |     1,024.87 ns |       555.239 ns |     30.434 ns |    1.90 |    0.05 | 0.0114 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       264.57 ns |       161.935 ns |      8.876 ns |    0.49 |    0.01 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,115.75 ns |       244.696 ns |     13.413 ns |    2.07 |    0.02 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        88.82 ns |        13.823 ns |      0.758 ns |    0.16 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       556.44 ns |       102.908 ns |      5.641 ns |    1.03 |    0.01 | 0.0114 |     968 B |          NA |
|                                |            |                 |                  |               |         |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **23,074.69 ns** |     **9,763.196 ns** |    **535.154 ns** |   **1.000** |    **0.03** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   140,915.39 ns |    13,737.395 ns |    752.993 ns |   6.109 |    0.12 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   441,043.40 ns |     3,017.042 ns |    165.374 ns |  19.120 |    0.38 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,772,657.53 ns |   104,293.541 ns |  5,716.681 ns |  76.850 |    1.54 |      - |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      | 1,001,039.09 ns |   118,234.095 ns |  6,480.810 ns |  43.398 |    0.89 |      - |      35 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,112,775.39 ns | 1,366,615.458 ns | 74,908.805 ns | 134.948 |    3.88 |      - |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,240,560.07 ns |    74,694.849 ns |  4,094.277 ns |  53.782 |    1.08 |      - |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,787,746.94 ns |    91,732.793 ns |  5,028.184 ns |  77.504 |    1.55 |      - |   80371 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    51,232.48 ns |    15,921.842 ns |    872.730 ns |   2.221 |    0.05 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   212,166.12 ns |    30,835.509 ns |  1,690.198 ns |   9.198 |    0.19 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   215,935.62 ns |    31,532.313 ns |  1,728.392 ns |   9.361 |    0.20 | 0.7324 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   369,656.25 ns |    19,765.774 ns |  1,083.429 ns |  16.026 |    0.32 | 0.4883 |   80177 B |          NA |
| Lodestar_FisherExact           | 10000      |       256.10 ns |         6.269 ns |      0.344 ns |   0.011 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,099.53 ns |         1.897 ns |      0.104 ns |   0.048 |    0.00 | 0.0095 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        88.71 ns |         9.226 ns |      0.506 ns |   0.004 |    0.00 | 0.0019 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       552.20 ns |       163.898 ns |      8.984 ns |   0.024 |    0.00 | 0.0114 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-09-21, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | SampleSize | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|------------- |----------- |------------:|-----------:|----------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **Lodestar_Ols** | **100**        |    **83.58 μs** |   **8.407 μs** |  **0.461 μs** |  **1.00** |    **0.01** |    **4.1504** |    **0.1221** |         **-** |   **68.84 KB** |        **1.00** |
| Accord_Ols   | 100        |   265.58 μs |  32.832 μs |  1.800 μs |  3.18 |    0.02 |    2.4414 |         - |         - |      43 KB |        0.62 |
|              |            |             |            |           |       |         |           |           |           |            |             |
| **Lodestar_Ols** | **10000**      | **6,285.52 μs** | **497.437 μs** | **27.266 μs** |  **1.00** |    **0.01** | **1250.0000** | **1242.1875** | **1242.1875** | **6489.23 KB** |        **1.00** |
| Accord_Ols   | 10000      | 4,549.84 μs | 415.128 μs | 22.755 μs |  0.72 |    0.00 |  210.9375 |  187.5000 |         - | 3523.47 KB |        0.54 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.QuantileBenchmarks-report-github

_As of 2026-09-18, measured at commit `a891e078a2dc2531149eba169d905639dfedd3af`._

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
| NormalQuantile               |  69.60 ns |  0.270 ns | 0.015 ns |  1.00 |         - |          NA |
| NormalQuantileFarTail        |  69.89 ns |  4.062 ns | 0.223 ns |  1.00 |         - |          NA |
| StudentQuantile              | 525.21 ns |  4.081 ns | 0.224 ns |  7.55 |         - |          NA |
| StudentQuantileCauchyFarTail | 162.78 ns | 10.839 ns | 0.594 ns |  2.34 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |  **1,053.1 μs** |    **50.38 μs** |   **2.76 μs** |        **-** |        **-** |        **-** |       **82 B** |
| WilcoxonPaired            | 10000      | False |    271.3 μs |    25.37 μs |   1.39 μs |   0.4883 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | False |    585.0 μs |    20.97 μs |   1.15 μs |   2.9297 |        - |        - |   279681 B |
| WilcoxonPooledDifferences | 10000      | False |    885.4 μs |   194.55 μs |  10.66 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    472.1 μs |    25.10 μs |   1.38 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **647.4 μs** |    **47.38 μs** |   **2.60 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    246.2 μs |     7.32 μs |   0.40 μs |   0.4883 |        - |        - |    80057 B |
| KruskalWallisManyGroups   | 10000      | True  |    406.4 μs |     6.41 μs |   0.35 μs |   2.9297 |   0.4883 |        - |   279680 B |
| WilcoxonPooledDifferences | 10000      | True  |    796.7 μs |   358.34 μs |  19.64 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    440.9 μs |   118.62 μs |   6.50 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** | **11,085.7 μs** | **4,716.03 μs** | **258.50 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  3,249.2 μs |   141.91 μs |   7.78 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  8,077.0 μs | 3,123.19 μs | 171.19 μs | 328.1250 | 328.1250 | 328.1250 |  2800349 B |
| WilcoxonPooledDifferences | 100000     | False | 28,339.3 μs | 1,659.90 μs |  90.98 μs | 968.7500 | 968.7500 | 968.7500 | 13200772 B |
| MannWhitneyTest           | 100000     | False |  4,923.1 μs |   116.42 μs |   6.38 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **6,471.1 μs** |   **141.59 μs** |   **7.76 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,847.9 μs |   105.20 μs |   5.77 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  4,722.4 μs |   982.16 μs |  53.84 μs | 390.6250 | 390.6250 | 390.6250 |  2800386 B |
| WilcoxonPooledDifferences | 100000     | True  | 18,215.2 μs | 1,637.67 μs |  89.77 μs | 968.7500 | 968.7500 | 968.7500 | 13076300 B |
| MannWhitneyTest           | 100000     | True  |  4,349.9 μs |   594.41 μs |  32.58 μs |        - |        - |        - |          - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RobustCovarianceBenchmarks-report-github

_As of 2026-09-18, measured at commit `a891e078a2dc2531149eba169d905639dfedd3af`._

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

| Method | SampleSize | Covariance | Mean       | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------- |----------- |----------- |-----------:|------------:|----------:|---------:|---------:|---------:|----------:|
| **Fit**    | **100**        | **Nonrobust**  |   **5.512 μs** |   **0.1857 μs** | **0.0102 μs** |   **0.1526** |        **-** |        **-** |   **2.54 KB** |
| **Fit**    | **100**        | **Hc0**        |   **7.932 μs** |   **2.4633 μs** | **0.1350 μs** |   **0.5188** |        **-** |        **-** |   **8.53 KB** |
| **Fit**    | **100**        | **Hc1**        |   **7.906 μs** |   **0.8708 μs** | **0.0477 μs** |   **0.5188** |        **-** |        **-** |   **8.53 KB** |
| **Fit**    | **100**        | **Hc2**        |   **9.143 μs** |   **0.6870 μs** | **0.0377 μs** |   **0.5646** |        **-** |        **-** |    **9.4 KB** |
| **Fit**    | **100**        | **Hc3**        |   **9.384 μs** |   **5.8605 μs** | **0.3212 μs** |   **0.5646** |        **-** |        **-** |    **9.4 KB** |
| **Fit**    | **10000**      | **Nonrobust**  | **365.581 μs** | **131.9300 μs** | **7.2315 μs** |   **4.3945** |   **0.9766** |        **-** |  **79.88 KB** |
| **Fit**    | **10000**      | **Hc0**        | **757.395 μs** |  **83.8257 μs** | **4.5948 μs** | **124.0234** | **124.0234** | **124.0234** | **472.68 KB** |
| **Fit**    | **10000**      | **Hc1**        | **749.200 μs** |  **15.1043 μs** | **0.8279 μs** | **124.0234** | **124.0234** | **124.0234** | **472.68 KB** |
| **Fit**    | **10000**      | **Hc2**        | **868.884 μs** | **145.9053 μs** | **7.9976 μs** | **124.0234** | **124.0234** | **124.0234** | **550.89 KB** |
| **Fit**    | **10000**      | **Hc3**        | **877.018 μs** | **140.1184 μs** | **7.6804 μs** | **124.0234** | **124.0234** | **124.0234** | **550.89 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.SerialCorrelationBenchmarks-report-github

_As of 2026-09-18, measured at commit `a891e078a2dc2531149eba169d905639dfedd3af`._

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
| **LodestarAutocorrelation**        | **200**        |   **3.883 μs** | **1.4986 μs** | **0.0821 μs** |  **1.00** |    **0.03** | **0.1564** |    **2624 B** |        **1.00** |
| CortexAutocorrelation          | 200        |  10.085 μs | 0.4133 μs | 0.0227 μs |  2.60 |    0.05 |      - |     192 B |        0.07 |
| LodestarPartialAutocorrelation | 200        |   4.369 μs | 0.1123 μs | 0.0062 μs |  1.13 |    0.02 | 0.1678 |    2816 B |        1.07 |
| CortexPartialAutocorrelation   | 200        |  10.600 μs | 0.5309 μs | 0.0291 μs |  2.73 |    0.05 | 0.0458 |     768 B |        0.29 |
| LodestarLjungBox               | 200        |   4.165 μs | 0.4197 μs | 0.0230 μs |  1.07 |    0.02 | 0.1373 |    2344 B |        0.89 |
| CortexLjungBox                 | 200        |  10.186 μs | 0.4627 μs | 0.0254 μs |  2.62 |    0.05 |      - |     224 B |        0.09 |
|                                |            |            |           |           |       |         |        |           |             |
| **LodestarAutocorrelation**        | **2000**       |  **42.512 μs** | **0.8142 μs** | **0.0446 μs** |  **1.00** |    **0.00** | **0.9766** |   **17024 B** |        **1.00** |
| CortexAutocorrelation          | 2000       | 105.937 μs | 2.1619 μs | 0.1185 μs |  2.49 |    0.00 |      - |     192 B |        0.01 |
| LodestarPartialAutocorrelation | 2000       |  43.176 μs | 1.2682 μs | 0.0695 μs |  1.02 |    0.00 | 0.9766 |   17216 B |        1.01 |
| CortexPartialAutocorrelation   | 2000       | 106.889 μs | 8.5026 μs | 0.4661 μs |  2.51 |    0.01 |      - |     768 B |        0.05 |
| LodestarLjungBox               | 2000       |  43.022 μs | 1.7491 μs | 0.0959 μs |  1.01 |    0.00 | 0.9766 |   16744 B |        0.98 |
| CortexLjungBox                 | 2000       | 106.084 μs | 2.9819 μs | 0.1634 μs |  2.50 |    0.00 |      - |     224 B |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StationarityBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                        | SampleSize | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------------------------ |----------- |-----------:|-----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **6.733 μs** |  **6.7601 μs** | **0.3705 μs** |  **1.00** |    **0.07** |   **0.2899** |        **-** |        **-** |   **23.99 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  24.819 μs |  5.5951 μs | 0.3067 μs |  3.69 |    0.18 |   0.7935 |   0.0610 |        - |   66.56 KB |        2.77 |
| CortexAugmentedDickeyFuller   | 200        |  22.372 μs |  0.4249 μs | 0.0233 μs |  3.33 |    0.15 |   0.1831 |        - |        - |   16.18 KB |        0.67 |
| LodestarKpss                  | 200        |   2.791 μs |  0.9966 μs | 0.0546 μs |  0.42 |    0.02 |   0.0191 |        - |        - |    1.69 KB |        0.07 |
| CortexKpss                    | 200        |   2.867 μs |  1.0379 μs | 0.0569 μs |  0.43 |    0.02 |   0.0420 |        - |        - |    3.43 KB |        0.14 |
| LodestarDecompose             | 200        |   1.312 μs |  0.3312 μs | 0.0182 μs |  0.20 |    0.01 |   0.0782 |        - |        - |     6.5 KB |        0.27 |
| CortexDecompose               | 200        |   2.679 μs |  0.3479 μs | 0.0191 μs |  0.40 |    0.02 |   0.0992 |        - |        - |    8.16 KB |        0.34 |
|                               |            |            |            |           |       |         |          |          |          |            |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |  **86.184 μs** |  **6.4010 μs** | **0.3509 μs** |  **1.00** |    **0.00** |  **30.2734** |  **30.2734** |  **30.2734** |  **234.95 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 712.069 μs | 45.5692 μs | 2.4978 μs |  8.26 |    0.04 | 249.0234 | 249.0234 | 249.0234 | 1001.27 KB |        4.26 |
| CortexAugmentedDickeyFuller   | 2000       | 229.283 μs |  5.1119 μs | 0.2802 μs |  2.66 |    0.01 |  30.2734 |  30.2734 |  30.2734 |  142.76 KB |        0.61 |
| LodestarKpss                  | 2000       |  61.617 μs |  3.7231 μs | 0.2041 μs |  0.71 |    0.00 |   0.1221 |        - |        - |   15.75 KB |        0.07 |
| CortexKpss                    | 2000       |  62.911 μs |  1.8635 μs | 0.1021 μs |  0.73 |    0.00 |   0.3662 |        - |        - |   31.55 KB |        0.13 |
| LodestarDecompose             | 2000       |  13.185 μs |  4.1048 μs | 0.2250 μs |  0.15 |    0.00 |   0.7629 |   0.0610 |        - |   62.75 KB |        0.27 |
| CortexDecompose               | 2000       |  26.997 μs |  4.2490 μs | 0.2329 μs |  0.31 |    0.00 |   0.9460 |   0.0305 |        - |   78.48 KB |        0.33 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | Mean             | Error            | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |-----------------:|-----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |        **669.87 ns** |        **19.511 ns** |      **1.069 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     23,629.59 ns |       603.202 ns |     33.064 ns |   35.27 |    0.06 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      1,986.22 ns |        96.090 ns |      5.267 ns |    2.97 |    0.01 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 100        |     20,667.85 ns |       999.370 ns |     54.779 ns |   30.85 |    0.08 |   0.2747 |        - |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |         87.26 ns |        19.402 ns |      1.063 ns |    0.13 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 100        |        153.46 ns |        19.933 ns |      1.093 ns |    0.23 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
|                     |            |                  |                  |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **22,932.79 ns** |     **4,125.021 ns** |    **226.106 ns** |   **1.000** |    **0.01** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |     83,731.61 ns |    10,688.338 ns |    585.864 ns |   3.651 |    0.04 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    446,946.17 ns |    10,482.870 ns |    574.601 ns |  19.491 |    0.17 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 11,363,433.51 ns | 1,068,112.602 ns | 58,546.856 ns | 495.542 |    4.78 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |         87.54 ns |         5.840 ns |      0.320 ns |   0.004 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        153.72 ns |        12.448 ns |      0.682 ns |   0.007 |    0.00 |   0.0019 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.WeightedLeastSquaresBenchmarks-report-github

_As of 2026-09-18, measured at commit `a891e078a2dc2531149eba169d905639dfedd3af`._

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

| Method       | SampleSize | Mean          | Error        | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------- |----------- |--------------:|-------------:|------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Wls** | **200**        |      **9.533 μs** |     **4.498 μs** |   **0.2465 μs** |  **1.00** |    **0.03** |   **0.1984** |        **-** |        **-** |     **3.37 KB** |        **1.00** |
| MathNet_Wls  | 200        |     15.120 μs |     1.796 μs |   0.0984 μs |  1.59 |    0.04 |   3.7079 |   0.1984 |        - |    58.21 KB |       17.29 |
|              |            |               |              |             |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **2000**       |     **77.318 μs** |    **15.522 μs** |   **0.8508 μs** |  **1.00** |    **0.01** |   **0.9766** |        **-** |        **-** |    **17.43 KB** |        **1.00** |
| MathNet_Wls  | 2000       |     89.966 μs |    24.123 μs |   1.3222 μs |  1.16 |    0.02 |  33.0811 |  13.9160 |        - |   508.36 KB |       29.17 |
|              |            |               |              |             |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **20000**      |    **747.819 μs** |    **82.817 μs** |   **4.5395 μs** |  **1.00** |    **0.01** |  **46.8750** |  **46.8750** |  **46.8750** |   **158.44 KB** |        **1.00** |
| MathNet_Wls  | 20000      |  1,183.552 μs |   966.972 μs |  53.0030 μs |  1.58 |    0.06 | 994.1406 | 992.1875 | 992.1875 |  5029.69 KB |       31.74 |
|              |            |               |              |             |       |         |          |          |          |             |             |
| **Lodestar_Wls** | **200000**     |  **7,604.937 μs** |   **210.303 μs** |  **11.5274 μs** |  **1.00** |    **0.00** |  **93.7500** |  **93.7500** |  **93.7500** |  **1564.65 KB** |        **1.00** |
| MathNet_Wls  | 200000     | 12,915.615 μs | 6,167.047 μs | 338.0367 μs |  1.70 |    0.04 | 859.3750 | 859.3750 | 859.3750 | 50023.37 KB |       31.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------------ |---------:|---------:|---------:|---------:|----------:|
| ChatTemplate            | 84.48 ms | 2.469 ms | 0.135 ms | 333.3333 |   33.3 MB |
| ProseWithoutAddedTokens | 71.52 ms | 1.393 ms | 0.076 ms | 285.7143 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

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

| Method                 | Rows | Method   | Mean           | Error        | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|-------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,893.6 μs** |     **16.23 μs** |     **0.89 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |    90,786.6 μs |  5,256.63 μs |   288.13 μs |  47.94 |    0.13 |  3666.6667 |  1500.0000 |  833.3333 |  59880.37 KB |       58.56 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,812.7 μs** |    **407.18 μs** |    **22.32 μs** |   **1.00** |    **0.02** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |    94,676.8 μs | 27,519.32 μs | 1,508.43 μs |  52.23 |    0.91 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **716.0 μs** |     **45.34 μs** |     **2.49 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   136,367.1 μs |  4,484.01 μs |   245.78 μs | 190.46 |    0.64 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,245.6 μs** |     **72.50 μs** |     **3.97 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    75,364.2 μs | 67,750.51 μs | 3,713.63 μs |  33.56 |    1.43 |  3857.1429 |  1714.2857 |  857.1429 |  64191.22 KB |       62.78 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **14,527.7 μs** |  **1,136.63 μs** |    **62.30 μs** |   **1.00** |    **0.01** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,643,429.6 μs | 97,181.45 μs | 5,326.84 μs | 113.13 |    0.53 | 35000.0000 | 10000.0000 | 4000.0000 | 537151.59 KB |       60.18 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **12,819.6 μs** |    **684.80 μs** |    **37.54 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,975,439.3 μs | 87,870.47 μs | 4,816.48 μs | 154.10 |    0.51 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **5,763.7 μs** |    **278.01 μs** |    **15.24 μs** |   **1.00** |    **0.00** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,259,161.0 μs | 29,684.67 μs | 1,627.12 μs | 565.47 |    1.32 | 47000.0000 | 12000.0000 | 4000.0000 | 732674.32 KB |    8,147.90 |
|                        |      |          |                |              |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,337.8 μs** |    **752.60 μs** |    **41.25 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,853,440.4 μs | 52,781.83 μs | 2,893.15 μs | 113.45 |    0.29 | 38000.0000 | 11000.0000 | 4000.0000 | 573621.47 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|------------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.476 μs** |   **0.8169 μs** | **0.0448 μs** |  **1.00** |    **0.01** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.756 μs |   0.9039 μs | 0.0495 μs |  1.05 |    0.01 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |   5.841 μs |   1.7044 μs | 0.0934 μs |  1.07 |    0.02 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |            |             |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |  **55.355 μs** |   **6.0957 μs** | **0.3341 μs** |  **1.00** |    **0.01** |  **1.7700** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.564 μs |   2.4124 μs | 0.1322 μs |  0.39 |    0.00 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.581 μs |   4.3229 μs | 0.2370 μs |  0.39 |    0.00 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
|                    |            |            |             |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         | **216.835 μs** |  **10.3479 μs** | **0.5672 μs** |  **1.00** |    **0.00** |  **6.5918** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |  82.281 μs |   4.9955 μs | 0.2738 μs |  0.38 |    0.00 |  5.0049 | 0.1221 |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |  70.813 μs |  14.5367 μs | 0.7968 μs |  0.33 |    0.00 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |            |             |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **846.998 μs** | **104.0969 μs** | **5.7059 μs** |  **1.00** |    **0.01** | **26.3672** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 332.039 μs |  22.5538 μs | 1.2362 μs |  0.39 |    0.00 | 20.0195 | 2.4414 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        | 278.036 μs |  23.7565 μs | 1.3022 μs |  0.33 |    0.00 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|---------:|----------:|
| **Encode** | **Ascii**    | **False**     | **28.14 ms** | **12.156 ms** | **0.666 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **30.11 ms** |  **0.993 ms** | **0.054 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **27.10 ms** |  **5.908 ms** | **0.324 ms** | **125.0000** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **38.36 ms** |  **0.901 ms** | **0.049 ms** | **214.2857** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **32.11 ms** |  **1.659 ms** | **0.091 ms** | **250.0000** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **44.79 ms** | **21.665 ms** | **1.188 ms** | **250.0000** |  **24.48 MB** |

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

| Method                 | Documents | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|----------------------- |---------- |---------------:|---------------:|--------------:|---------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **LodestarQuery**          | **1000**      |       **1.780 μs** |      **0.2640 μs** |     **0.0145 μs** |     **1.00** |    **0.01** |    **0.0248** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.538 μs |      0.7865 μs |     0.0431 μs |     1.43 |    0.02 |    0.0229 |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.162 μs |      0.3515 μs |     0.0193 μs |     1.78 |    0.02 |    0.3128 |         - |        - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   7,906.949 μs |  1,327.7667 μs |    72.7794 μs | 4,441.69 |   47.23 |  984.3750 |  906.2500 | 890.6250 |  4837822 B |   11,409.96 |
| LuceneFromText         | 1000      |   7,332.227 μs |    353.8914 μs |    19.3980 μs | 4,118.84 |   30.48 |   85.9375 |   78.1250 |   7.8125 |  1375080 B |    3,243.11 |
|                        |           |                |                |               |          |         |           |           |          |            |             |
| **LodestarQuery**          | **20000**     |      **24.289 μs** |      **1.1944 μs** |     **0.0655 μs** |     **1.00** |    **0.00** |         **-** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      41.389 μs |      3.8018 μs |     0.2084 μs |     1.70 |    0.01 |         - |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      18.037 μs |      2.2939 μs |     0.1257 μs |     0.74 |    0.00 |    0.4883 |         - |        - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 116,075.150 μs | 59,797.8036 μs | 3,277.7194 μs | 4,778.97 |  117.40 | 2200.0000 | 1200.0000 | 800.0000 | 83752573 B |  197,529.65 |
| LuceneFromText         | 20000     | 148,146.338 μs | 50,470.7016 μs | 2,766.4695 μs | 6,099.39 |   99.66 | 1250.0000 | 1000.0000 |        - | 22417140 B |   52,870.61 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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

| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram | 31.26 ms | 3.852 ms | 0.211 ms |  1.00 |    0.01 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 86.60 ms | 4.939 ms | 0.271 ms |  2.77 |    0.02 | 1666.6667 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error      | StdDev   | Gen0   | Allocated |
|-------------------------- |------- |----------:|-----------:|---------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **19.84 μs** |   **1.482 μs** | **0.081 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **38.71 μs** |  **12.767 μs** | **0.700 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **81.18 μs** |   **3.057 μs** | **0.168 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **329.27 μs** | **142.399 μs** | **7.805 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Mean     | Error    | StdDev    | Allocated |
|------------------ |---------:|---------:|----------:|----------:|
| EncodeUnseenProse | 7.754 ms | 3.204 ms | 0.1756 ms |   1.45 MB |

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

| Method                   | Cutoff | Widths | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------- |------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar_Cdist_Bounded**   | **0**      | **False**  | **256.35 μs** |  **47.002 μs** |  **2.576 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | False  | 251.04 μs |  32.961 μs |  1.807 μs |  0.98 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **0**      | **True**   | **288.79 μs** |  **93.263 μs** |  **5.112 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 0      | True   | 305.78 μs | 145.696 μs |  7.986 μs |  1.06 |    0.03 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **False**  | **260.39 μs** |  **64.834 μs** |  **3.554 μs** |  **1.00** |    **0.02** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | False  | 272.86 μs | 233.505 μs | 12.799 μs |  1.05 |    0.04 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **60**     | **True**   | **227.10 μs** |  **11.900 μs** |  **0.652 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 60     | True   | 312.73 μs |  71.939 μs |  3.943 μs |  1.38 |    0.02 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **False**  | **252.52 μs** |  **24.631 μs** |  **1.350 μs** |  **1.00** |    **0.01** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | False  | 265.81 μs |   9.541 μs |  0.523 μs |  1.05 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **80**     | **True**   | **138.51 μs** |   **2.207 μs** |  **0.121 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 80     | True   | 299.54 μs |  35.510 μs |  1.946 μs |  2.16 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **False**  | **189.27 μs** |  **11.894 μs** |  **0.652 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | False  | 259.15 μs |  24.600 μs |  1.348 μs |  1.37 |    0.01 | 1.9531 |  32.02 KB |        1.00 |
|                          |        |        |           |            |           |       |         |        |           |             |
| **Lodestar_Cdist_Bounded**   | **90**     | **True**   |  **78.85 μs** |   **2.732 μs** |  **0.150 μs** |  **1.00** |    **0.00** | **1.9531** |  **32.02 KB** |        **1.00** |
| Lodestar_Cdist_EveryPair | 90     | True   | 299.50 μs |  17.015 μs |  0.933 μs |  3.80 |    0.01 | 1.9531 |  32.02 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.CdistIncumbentBenchmarks-report-github

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

| Method                        | Size | Mean       | Error     | StdDev   | Ratio | Gen0     | Gen1    | Gen2    | Allocated  | Alloc Ratio |
|------------------------------ |----- |-----------:|----------:|---------:|------:|---------:|--------:|--------:|-----------:|------------:|
| **Lodestar_Cdist**                | **50**   |   **155.9 μs** |   **1.35 μs** |  **0.07 μs** |  **1.00** |   **0.9766** |       **-** |       **-** |   **19.55 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 50   |   146.4 μs |   8.73 μs |  0.48 μs |  0.94 |   0.9766 |       - |       - |   19.55 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 50   |   433.9 μs |  32.24 μs |  1.77 μs |  2.78 |  18.0664 |       - |       - |  301.95 KB |       15.44 |
|                               |      |            |           |          |       |          |         |         |            |             |
| **Lodestar_Cdist**                | **200**  | **3,276.5 μs** |  **25.60 μs** |  **1.40 μs** |  **1.00** |  **97.6563** | **97.6563** | **97.6563** |  **312.62 KB** |        **1.00** |
| Lodestar_HandWrittenLoop      | 200  | 3,251.2 μs |  79.48 μs |  4.36 μs |  0.99 |  97.6563 | 97.6563 | 97.6563 |  312.62 KB |        1.00 |
| FuzzySharp_ExtractAllPerQuery | 200  | 7,394.9 μs | 995.23 μs | 54.55 μs |  2.26 | 289.0625 |       - |       - | 4723.45 KB |       15.11 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error       | StdDev      | Gen0   | Allocated |
|------- |-------- |-------------:|------------:|------------:|-------:|----------:|
| **Report** | **10**      |     **373.5 ns** |    **339.9 ns** |    **18.63 ns** | **0.0186** |   **1.54 KB** |
| **Report** | **1000**    | **621,786.8 ns** | **61,086.9 ns** | **3,348.38 ns** | **0.9766** | **117.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|---------------- |-------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| Roc             | 1000000 | 113.3 ms | 13.36 ms | 0.73 ms | 800.0000 | 800.0000 | 800.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 103.0 ms | 14.85 ms | 0.81 ms | 800.0000 | 800.0000 | 800.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

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

| Method                         | Samples | Clusters | Mean       | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.400 ms** |  **0.1950 ms** | **0.0107 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.399 ms |  0.2597 ms | 0.0142 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  22.181 ms |  0.3160 ms | 0.0173 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.243 ms** |  **0.2195 ms** | **0.0120 ms** | **187.5000** | **187.5000** | **187.5000** |  **937.68 KB** |
| MutualInformationScore         | 100000  | 100      |   3.325 ms |  0.0545 ms | 0.0030 ms | 210.9375 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 192.777 ms | 11.0228 ms | 0.6042 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

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

| Method   | Length | Mean        | Error      | StdDev    | Gen0   | Allocated |
|--------- |------- |------------:|-----------:|----------:|-------:|----------:|
| **Distance** | **12**     |    **948.3 ns** |   **257.0 ns** |  **14.09 ns** | **0.0496** |     **840 B** |
| **Distance** | **120**    | **63,387.4 ns** | **8,342.6 ns** | **457.29 ns** | **0.1221** |    **2128 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

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

| Method       | Shape     | Mean       | Error    | StdDev  | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |-----------:|---------:|--------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **389.1 ms** | **18.18 ms** | **1.00 ms** |  **1.00** |    **0.00** |  **2000.0000** | **2000.0000** | **2000.0000** |  **28.69 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,644.6 ms | 78.76 ms | 4.32 ms |  6.80 |    0.02 | 14000.0000 | 8000.0000 | 7000.0000 | 156.31 MB |        5.45 |
|              |           |            |          |         |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |   **132.7 ms** | **12.84 ms** | **0.70 ms** |  **1.00** |    **0.01** |  **1250.0000** | **1250.0000** | **1250.0000** |  **13.29 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   903.2 ms | 49.90 ms | 2.74 ms |  6.81 |    0.04 |  5000.0000 | 3000.0000 | 3000.0000 |  59.78 MB |        4.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

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

| Method                   | Shape      | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------------------- |----------- |------------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar_Fit**             | **20000x2x10** | **1,010.81 ms** |  **21.578 ms** |  **1.183 ms** |  **1.00** |    **0.00** |  **2000.0000** |  **2000.0000** |  **2000.0000** | **112.13 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 7,149.23 ms | 402.332 ms | 22.053 ms |  7.07 |    0.02 | 38000.0000 | 13000.0000 | 11000.0000 | 531.86 MB |        4.74 |
| Dbscan_CalculateClusters | 20000x2x10 | 4,005.47 ms | 361.295 ms | 19.804 ms |  3.96 |    0.02 | 22000.0000 | 12000.0000 | 10000.0000 | 372.13 MB |        3.32 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **84.84 ms** |   **1.798 ms** |  **0.099 ms** |  **1.00** |    **0.00** |  **1166.6667** |  **1166.6667** |  **1166.6667** |  **14.02 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   536.21 ms |  20.205 ms |  1.108 ms |  6.32 |    0.01 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.75 |
| Dbscan_CalculateClusters | 5000x2x5   |   274.41 ms |  85.391 ms |  4.681 ms |  3.23 |    0.05 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.88 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean       | Error     | StdDev    | Ratio | RatioSD |
|------------------------------------------ |-----------:|----------:|----------:|------:|--------:|
| TruncatedSvd_Rank20                       |  19.769 ms | 2.3751 ms | 0.1302 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 122.740 ms | 9.8457 ms | 0.5397 ms |  6.21 |    0.04 |
| Nmf_Transform                             |   3.578 ms | 0.2645 ms | 0.0145 ms |  0.18 |    0.00 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  26.590 ms | 1.3138 ms | 0.0720 ms |  1.35 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error       | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **32,990.2 μs** |  **1,333.0 μs** |    **73.07 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **12151.27 KB** |
| TruncatedSvd_Transform     | 500     |     436.2 μs |    104.5 μs |     5.73 μs |   91.3086 |   90.8203 |   90.8203 |    390.73 KB |
| Nmf_Frobenius_Rank20       | 500     | 192,641.1 μs | 87,800.1 μs | 4,812.62 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18143.39 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 228,977.9 μs | 30,919.5 μs | 1,694.80 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.11 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **194,674.0 μs** | **25,927.3 μs** | **1,421.16 μs** | **2666.6667** | **2666.6667** | **2666.6667** | **105766.49 KB** |
| TruncatedSvd_Transform     | 20000   |   1,917.4 μs |    243.2 μs |    13.33 μs |  496.0938 |  496.0938 |  496.0938 |   3437.86 KB |
| Nmf_Frobenius_Rank20       | 20000   | 732,028.8 μs | 48,777.2 μs | 2,673.65 μs | 5000.0000 | 5000.0000 | 5000.0000 |  156850.7 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 590,860.4 μs | 37,207.7 μs | 2,039.48 μs | 5000.0000 | 5000.0000 | 5000.0000 |  157305.7 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

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

| Method | Mean     | Error    | StdDev  | Gen0    | Allocated |
|------- |---------:|---------:|--------:|--------:|----------:|
| Encode | 246.8 μs | 26.56 μs | 1.46 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

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

| Method      | Count  | Mean       | Error     | StdDev   | Allocated |
|------------ |------- |-----------:|----------:|---------:|----------:|
| **SearchTop10** | **10000**  |   **555.7 μs** |  **18.81 μs** |  **1.03 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **6,090.9 μs** | **272.11 μs** | **14.92 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Rows   | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|-----------:|---------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **627.0 μs** |   **181.5 μs** |  **9.95 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  5,230.6 μs |   279.6 μs | 15.33 μs |        - |        - |        - |     21.02 KB |
| **FromBlockNormalized** | **100000** | **60,250.5 μs** | **1,271.1 μs** | **69.67 μs** | **222.2222** | **222.2222** | **222.2222** | **150000.24 KB** |
| MmrSelect100        | 100000 |  5,640.8 μs |   423.7 μs | 23.23 μs |        - |        - |        - |     21.02 KB |

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

| Method          | Records | Mean        | Error        | StdDev      | Gen0     | Gen1     | Gen2    | Allocated   |
|---------------- |-------- |------------:|-------------:|------------:|---------:|---------:|--------:|------------:|
| **FilteredTop10**   | **10000**   |    **512.9 μs** |    **286.75 μs** |    **15.72 μs** |        **-** |        **-** |       **-** |      **7.2 KB** |
| Unfiltered      | 10000   |    560.0 μs |     46.32 μs |     2.54 μs |        - |        - |       - |      2.8 KB |
| HybridSelective | 10000   |  2,952.8 μs |     39.44 μs |     2.16 μs | 140.6250 | 113.2813 | 54.6875 |  2577.92 KB |
| HybridBroad     | 10000   |  4,455.1 μs |    124.89 μs |     6.85 μs | 164.0625 | 117.1875 | 62.5000 |  3221.06 KB |
| **FilteredTop10**   | **100000**  | **10,855.8 μs** |    **490.40 μs** |    **26.88 μs** |        **-** |        **-** |       **-** |     **7.21 KB** |
| Unfiltered      | 100000  |  6,134.0 μs |    307.41 μs |    16.85 μs |        - |        - |       - |      2.8 KB |
| HybridSelective | 100000  | 37,893.1 μs |  3,272.77 μs |   179.39 μs | 230.7692 | 153.8462 |       - | 23470.98 KB |
| HybridBroad     | 100000  | 55,433.3 μs | 34,446.87 μs | 1,888.15 μs | 222.2222 | 111.1111 |       - | 29359.82 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |-----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |   109.4 ns |   1.60 ns |  0.09 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   766.6 ns |  33.44 ns |  1.83 ns |  7.01 |    0.02 |      - |         - |          NA |
| TokenSortRatio |   894.9 ns | 680.48 ns | 37.30 ns |  8.18 |    0.30 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   933.0 ns | 126.83 ns |  6.95 ns |  8.53 |    0.06 | 0.0534 |     896 B |          NA |
| WRatio         | 1,179.4 ns | 187.24 ns | 10.26 ns | 10.79 |    0.08 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

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

| Method                    | Mean               | Error             | StdDev          | Median             | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,208,801.0430 ns |   349,344.3616 ns |  19,148.7433 ns |  2,199,929.7539 ns |  1.000 |    0.01 |  7.8125 |       - |  160507 B |        1.00 |
| TokenSortRatio_EmojiWords |    658,638.7598 ns |    36,067.7165 ns |   1,976.9933 ns |    658,007.2656 ns |  0.298 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,199,610.1465 ns |   764,137.2025 ns |  41,884.9387 ns |  1,179,754.5312 ns |  0.543 |    0.02 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 77,596,702.4286 ns | 3,161,671.2923 ns | 173,301.8728 ns | 77,507,797.2857 ns | 35.132 |    0.27 |       - |       - |  510817 B |        3.18 |
| WRatio_EmptyOperand       |          0.0296 ns |         0.2456 ns |       0.0135 ns |          0.0221 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean        | Error       | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|------------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **109.3 ns** |     **0.96 ns** |  **0.05 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    227.9 ns |    47.06 ns |  2.58 ns |  2.09 |    0.02 | 0.0048 |      80 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **765.5 ns** |    **35.19 ns** |  **1.93 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,105.2 ns |   532.36 ns | 29.18 ns | 13.20 |    0.04 |      - |     160 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **943.4 ns** |   **206.03 ns** | **11.29 ns** |  **1.00** |    **0.01** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  1,972.9 ns |   117.10 ns |  6.42 ns |  2.09 |    0.02 | 0.1144 |    1944 B |        2.17 |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,201.1 ns** | **1,320.82 ns** | **72.40 ns** |  **1.00** |    **0.07** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,941.4 ns |   629.51 ns | 34.51 ns |  4.12 |    0.21 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.351 ms** | **0.0625 ms** | **0.0034 ms** | **253.9063** | **253.9063** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **20.482 ms** | **0.3915 ms** | **0.0215 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

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

| Method           | Shape       | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,876.6 μs** |     **331.63 μs** |    **18.18 μs** |  **1.00** |    **0.00** | **7.8125** |      **-** | **162.63 KB** |        **1.00** |
| NumFlat_Fit      | 10000x16x16 |  37,105.2 μs |  13,263.38 μs |   727.01 μs |  6.31 |    0.11 |      - |      - |  10.39 KB |        0.06 |
| MetaNumerics_Fit | 10000x16x16 |  85,518.8 μs |   6,157.07 μs |   337.49 μs | 14.55 |    0.06 |      - |      - |  41.95 KB |        0.26 |
|                  |             |              |               |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **766.1 μs** |      **38.63 μs** |     **2.12 μs** |  **1.00** |    **0.00** | **8.7891** | **0.9766** | **156.73 KB** |        **1.00** |
| NumFlat_Fit      | 10000x2x8   |   5,425.9 μs |     322.22 μs |    17.66 μs |  7.08 |    0.03 |      - |      - |      3 KB |        0.02 |
| MetaNumerics_Fit | 10000x2x8   |   2,865.7 μs |      84.95 μs |     4.66 μs |  3.74 |    0.01 |      - |      - |  39.57 KB |        0.25 |
|                  |             |              |               |             |       |         |        |        |           |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **28,994.9 μs** |     **224.64 μs** |    **12.31 μs** |  **1.00** |    **0.00** |      **-** |      **-** | **787.79 KB** |        **1.00** |
| NumFlat_Fit      | 50000x8x32  | 452,376.4 μs | 116,297.83 μs | 6,374.68 μs | 15.60 |    0.19 |      - |      - |  16.29 KB |        0.02 |
| MetaNumerics_Fit | 50000x8x32  | 713,800.5 μs | 100,779.11 μs | 5,524.04 μs | 24.62 |    0.17 |      - |      - | 199.91 KB |        0.25 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

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

| Method         | Shape       | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |------------ |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **7.653 ms** | **0.3383 ms** | **0.0185 ms** |  **1.00** |    **0.00** |   **88.7 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x16x16 |  17.063 ms | 5.5113 ms | 0.3021 ms |  2.23 |    0.03 |   13.6 KB |        0.15 |
|                |             |            |           |           |       |         |           |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **30.886 ms** | **6.0830 ms** | **0.3334 ms** |  **1.00** |    **0.01** |  **98.92 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 123.535 ms | 2.3822 ms | 0.1306 ms |  4.00 |    0.04 |  96.54 KB |        0.98 |
|                |             |            |           |           |       |         |           |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **76.352 ms** | **3.2494 ms** | **0.1781 ms** |  **1.00** |    **0.00** | **410.27 KB** |        **1.00** |
| NumFlat_Lloyd  | 50000x8x32  | 221.764 ms | 4.4040 ms | 0.2414 ms |  2.90 |    0.01 |  36.47 KB |        0.09 |

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

| Method         | Samples | Classes | Mean           | Error         | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|--------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,506.2 ns** |   **2,513.21 ns** |    **137.76 ns** | **0.0153** |     **376 B** |
| MatrixWeighted | 1000    | 2       |     7,181.2 ns |     449.22 ns |     24.62 ns | 0.0153 |     376 B |
| AccuracyScore  | 1000    | 2       |       113.0 ns |       1.40 ns |      0.08 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,630.4 ns |     929.89 ns |     50.97 ns | 0.0305 |     536 B |
| Report         | 1000    | 2       |     6,540.5 ns |     734.58 ns |     40.26 ns | 0.3128 |    5344 B |
| **Matrix**         | **1000**    | **10**      |     **4,303.7 ns** |     **131.28 ns** |      **7.20 ns** | **0.0763** |    **1312 B** |
| MatrixWeighted | 1000    | 10      |     7,494.6 ns |     158.67 ns |      8.70 ns | 0.0763 |    1312 B |
| AccuracyScore  | 1000    | 10      |       111.5 ns |       2.88 ns |      0.16 ns |      - |         - |
| F1Macro        | 1000    | 10      |     4,457.6 ns |     293.73 ns |     16.10 ns | 0.0992 |    1728 B |
| Report         | 1000    | 10      |     9,915.9 ns |     327.69 ns |     17.96 ns | 0.7324 |   12336 B |
| **Matrix**         | **100000**  | **2**       |   **427,630.7 ns** |  **11,196.27 ns** |    **613.71 ns** |      **-** |     **376 B** |
| MatrixWeighted | 100000  | 2       |   868,786.5 ns |  20,972.53 ns |  1,149.58 ns |      - |     377 B |
| AccuracyScore  | 100000  | 2       |    12,117.2 ns |     737.11 ns |     40.40 ns |      - |         - |
| F1Macro        | 100000  | 2       |   423,010.9 ns |  28,544.59 ns |  1,564.63 ns |      - |     536 B |
| Report         | 100000  | 2       |   408,761.9 ns |  23,540.33 ns |  1,290.32 ns |      - |    5368 B |
| **Matrix**         | **100000**  | **10**      |   **398,655.0 ns** |  **12,836.37 ns** |    **703.60 ns** |      **-** |    **1312 B** |
| MatrixWeighted | 100000  | 10      |   990,320.4 ns | 141,308.45 ns |  7,745.59 ns |      - |    1313 B |
| AccuracyScore  | 100000  | 10      |    11,806.0 ns |   1,465.64 ns |     80.34 ns |      - |         - |
| F1Macro        | 100000  | 10      |   394,504.3 ns |  16,864.94 ns |    924.42 ns |      - |    1728 B |
| Report         | 100000  | 10      |   405,054.7 ns |  79,996.72 ns |  4,384.89 ns | 0.4883 |   12680 B |
| **Matrix**         | **1000000** | **2**       | **4,464,704.4 ns** | **104,182.65 ns** |  **5,710.60 ns** |      **-** |     **382 B** |
| MatrixWeighted | 1000000 | 2       | 8,634,413.7 ns | 215,003.42 ns | 11,785.06 ns |      - |     388 B |
| AccuracyScore  | 1000000 | 2       |   122,368.7 ns |   4,684.79 ns |    256.79 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,053,678.3 ns |  88,258.72 ns |  4,837.76 ns |      - |     542 B |
| Report         | 1000000 | 2       | 4,225,637.5 ns |  52,233.55 ns |  2,863.10 ns |      - |    5390 B |
| **Matrix**         | **1000000** | **10**      | **3,717,688.2 ns** |  **98,489.31 ns** |  **5,398.53 ns** |      **-** |    **1315 B** |
| MatrixWeighted | 1000000 | 10      | 9,846,371.1 ns | 449,781.52 ns | 24,654.04 ns |      - |    1324 B |
| AccuracyScore  | 1000000 | 10      |   122,508.5 ns |  12,780.35 ns |    700.53 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,755,325.9 ns |  60,509.71 ns |  3,316.74 ns |      - |    1731 B |
| Report         | 1000000 | 10      | 3,767,444.0 ns | 107,489.09 ns |  5,891.84 ns |      - |   12726 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean          | Error         | StdDev       | Ratio    | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |--------------:|--------------:|-------------:|---------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **7,030.88 μs** |  **1,031.156 μs** |    **56.521 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1064 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  38,200.54 μs |  2,178.578 μs |   119.415 μs |     5.43 |    0.04 | 642.8571 | 642.8571 | 642.8571 |  5089464 B |    4,783.33 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.30 μs** |      **2.892 μs** |     **0.159 μs** |     **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  38,876.82 μs |    492.408 μs |    26.991 μs | 3,440.52 |   41.51 | 538.4615 | 538.4615 | 538.4615 |  5089463 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **106,125.51 μs** | **71,678.669 μs** | **3,928.950 μs** |     **1.00** |    **0.04** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 250,352.45 μs |  9,928.151 μs |   544.195 μs |     2.36 |    0.07 |        - |        - |        - | 23232104 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **123.33 μs** |      **5.323 μs** |     **0.292 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 250,429.96 μs |  8,896.373 μs |   487.640 μs | 2,030.61 |    5.39 |        - |        - |        - | 23231816 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean        | Error      | StdDev    | Allocated |
|---------- |-------- |------------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **36.02 ms** |   **1.731 ms** |  **0.095 ms** |         **-** |
| OneVsOne  | 100000  |    75.23 ms |  15.062 ms |  0.826 ms |  801865 B |
| **OneVsRest** | **1000000** |   **678.58 ms** |  **65.703 ms** |  **3.601 ms** |         **-** |
| OneVsOne  | 1000000 | 1,127.26 ms | 314.592 ms | 17.244 ms | 8003648 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| PerLabel         | 100000 |  6.887 ms |  2.2742 ms | 0.1247 ms |        - |        - |        - |     6.28 KB |
| PerLabelWeighted | 100000 |  9.484 ms |  1.4048 ms | 0.0770 ms |        - |        - |        - |     6.29 KB |
| PerSample        | 100000 | 44.840 ms | 44.5859 ms | 2.4439 ms | 437.5000 | 375.0000 | 125.0000 | 31250.15 KB |
| PerClass         | 100000 |  3.813 ms |  0.0259 ms | 0.0014 ms |        - |        - |        - |     6.42 KB |

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

| Method      | NeedleLength | Mean         | Error     | StdDev    | Allocated |
|------------ |------------- |-------------:|----------:|----------:|----------:|
| **Embedded**    | **65**           |     **9.779 μs** | **0.1410 μs** | **0.0077 μs** |         **-** |
| EqualLength | 65           |     2.918 μs | 0.0347 μs | 0.0019 μs |         - |
| **Embedded**    | **128**          |    **36.779 μs** | **0.8005 μs** | **0.0439 μs** |         **-** |
| EqualLength | 128          |     5.478 μs | 0.1462 μs | 0.0080 μs |         - |
| **Embedded**    | **512**          | **1,428.629 μs** | **0.3747 μs** | **0.0205 μs** |         **-** |
| EqualLength | 512          |    47.958 μs | 2.8991 μs | 0.1589 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 |  7.769 ms | 0.5825 ms | 0.0319 ms | 156.2500 | 156.2500 | 156.2500 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 11.010 ms | 4.7985 ms | 0.2630 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  4.130 ms |  4.9446 ms | 0.2710 ms | 109.3750 | 101.5625 |  31.2500 |  3711.57 KB |
| TokenizerJsonWordPiece | 11.205 ms |  3.3573 ms | 0.1840 ms | 187.5000 | 171.8750 |  46.8750 |   5852.4 KB |
| TokenizerJsonUnigram   | 11.064 ms |  0.3248 ms | 0.0178 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |  4.179 ms |  1.5174 ms | 0.0832 ms | 109.3750 | 101.5625 |  31.2500 |  3440.06 KB |
| TfidfSave              |  1.716 ms |  0.4242 ms | 0.0233 ms |  21.4844 |  15.6250 |  15.6250 |  2137.04 KB |
| TfidfLoad              |  4.418 ms |  0.4796 ms | 0.0263 ms |  85.9375 |  78.1250 |  23.4375 |  2930.51 KB |
| EmbeddingIndexSave     |  3.880 ms |  3.0461 ms | 0.1670 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  4.647 ms |  0.8686 ms | 0.0476 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 49.869 ms | 12.5376 ms | 0.6872 ms |        - |        - |        - |   323.49 KB |
| EmbeddingIndexLoadGzip | 74.792 ms |  1.6344 ms | 0.0896 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-09-21, measured at commit `fac280c117f7e86c65fd70bf1cf12e07b683e38a`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 11.42 ms | 2.303 ms | 0.126 ms |  31.2500 |   2.75 MB |
| EncodeDocuments    | 49.62 ms | 2.625 ms | 0.144 ms | 100.0000 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **10,287.33 μs** | **343.248 μs** | **18.815 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.27 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,373.51 μs | 283.200 μs | 15.523 μs |  1.59 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **146.17 μs** |  **14.464 μs** |  **0.793 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    194.28 μs |  19.270 μs |  1.056 μs |  1.33 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,081.05 μs** | **178.178 μs** |  **9.767 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,832.29 μs |  47.898 μs |  2.625 μs |  0.92 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **24.16 μs** |   **4.314 μs** |  **0.236 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.33 μs |   1.514 μs |  0.083 μs |  1.13 |   0.0916 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

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

| Method     | Limit | Mean     | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |------ |---------:|----------:|----------:|------:|----------:|------------:|
| **Extract**    | **1**     | **7.323 ms** | **0.1123 ms** | **0.0062 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 7.225 ms | 0.1253 ms | 0.0069 ms |  0.99 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **7.309 ms** | **0.0278 ms** | **0.0015 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 7.159 ms | 0.0437 ms | 0.0024 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

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

| Method            | Q | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------ |-- |---------:|----------:|----------:|-------:|----------:|
| **JaccardSimilarity** | **1** | **5.052 μs** | **1.4937 μs** | **0.0819 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **6.185 μs** | **0.5866 μs** | **0.0322 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

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

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 40.697 ms | 1.0972 ms | 0.0601 ms |       - |       - |       - |  800353 B |
| NdcgIgnoringTies                  | 100000 | 38.511 ms | 0.9872 ms | 0.0541 ms |       - |       - |       - |  800353 B |
| DcgTieAveraged                    | 100000 | 23.779 ms | 0.6069 ms | 0.0333 ms |       - |       - |       - |  800215 B |
| ReciprocalRankScore               | 100000 | 19.945 ms | 0.4449 ms | 0.0244 ms |       - |       - |       - |         - |
| CoverageErrorScore                | 100000 |  6.589 ms | 0.3265 ms | 0.0179 ms | 15.6250 | 15.6250 | 15.6250 |  800271 B |
| LabelRankingAveragePrecisionScore | 100000 | 58.939 ms | 0.3888 ms | 0.0213 ms |       - |       - |       - |         - |

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

| Method   | Samples | Mean        | Error      | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|---------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **48.24 μs** |   **3.217 μs** | **0.176 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    47.45 μs |   2.798 μs | 0.153 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   122.56 μs |   2.681 μs | 0.147 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   507.05 μs |  35.206 μs | 1.930 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **480.50 μs** |  **21.803 μs** | **1.195 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   472.04 μs |   6.823 μs | 0.374 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,223.99 μs |  44.734 μs | 2.452 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,678.62 μs | 119.323 μs | 6.540 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

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

| Method | Shape        | Mean     | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **25.82 ms** |  **2.346 ms** | **0.129 ms** | **93.7500** | **93.7500** | **93.7500** |  **375.3 KB** |
| **Fit**    | **500x20000x5**  | **44.80 ms** | **10.186 ms** | **0.558 ms** |       **-** |       **-** |       **-** | **492.47 KB** |

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

| Method          | Model   | Mean     | Error    | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **149.1 ms** | **20.11 ms** | **1.10 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **145.5 ms** |  **2.24 ms** | **0.12 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

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

| Method    | Samples | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **25.79 ms** | **0.235 ms** | **0.013 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **172.72 ms** | **7.489 ms** | **0.411 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **4.983 ms** | **0.4360 ms** | **0.0239 ms** |  **1.00** |    **0.01** | **109.3750** | **109.3750** | **109.3750** | **1366.79 KB** |        **1.00** |
| CountWithStopWords   | 200       |  4.411 ms | 0.2988 ms | 0.0164 ms |  0.89 |    0.00 |  39.0625 |  15.6250 |        - |  725.83 KB |        0.53 |
| Hashing              | 200       |  5.067 ms | 0.2494 ms | 0.0137 ms |  1.02 |    0.00 |  70.3125 |  70.3125 |  70.3125 | 1051.16 KB |        0.77 |
| HashingWithStopWords | 200       |  4.590 ms | 0.3534 ms | 0.0194 ms |  0.92 |    0.01 |  31.2500 |  15.6250 |        - |   591.9 KB |        0.43 |
|                      |           |           |           |           |       |         |          |          |          |            |             |
| **Count**                | **1000**      | **16.375 ms** | **6.1289 ms** | **0.3359 ms** |  **1.00** |    **0.02** | **812.5000** | **812.5000** | **812.5000** | **5880.57 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 14.085 ms | 1.4832 ms | 0.0813 ms |  0.86 |    0.02 | 375.0000 | 375.0000 | 375.0000 | 3155.02 KB |        0.54 |
| Hashing              | 1000      | 16.817 ms | 2.1322 ms | 0.1169 ms |  1.03 |    0.02 | 656.2500 | 562.5000 | 562.5000 | 4793.55 KB |        0.82 |
| HashingWithStopWords | 1000      | 15.234 ms | 0.5353 ms | 0.0293 ms |  0.93 |    0.02 | 265.6250 | 265.6250 | 265.6250 | 2633.78 KB |        0.45 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

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

| Method  | Words | Mean      | Error     | StdDev   | Gen0      | Gen1      | Allocated |
|-------- |------ |----------:|----------:|---------:|----------:|----------:|----------:|
| **Extract** | **2000**  |  **15.99 ms** |  **0.609 ms** | **0.033 ms** |  **187.5000** |   **93.7500** |   **3.13 MB** |
| **Extract** | **8000**  | **309.73 ms** | **40.556 ms** | **2.223 ms** | **2000.0000** | **1000.0000** |  **35.34 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error      | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **22.03 ms** |   **0.531 ms** | **0.029 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  54.61 ms |   4.698 ms | 0.258 ms |  2.48 |    0.01 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **47.95 ms** |   **5.578 ms** | **0.306 ms** |  **1.00** |    **0.01** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  57.19 ms |   2.661 ms | 0.146 ms |  1.19 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **87.27 ms** |   **5.346 ms** | **0.293 ms** |  **1.00** |    **0.00** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 270.49 ms | 164.602 ms | 9.022 ms |  3.10 |    0.09 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

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

| Method | Classes | Mean      | Error     | StdDev   | Allocated |
|------- |-------- |----------:|----------:|---------:|----------:|
| **TopTwo** | **10**      |  **13.60 ms** |  **0.169 ms** | **0.009 ms** |         **-** |
| **TopTwo** | **100**     | **100.78 ms** | **11.157 ms** | **0.612 ms** |         **-** |

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

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **56.04 ns** | **8.192 ns** | **0.449 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  48.56 ns | 2.218 ns | 0.122 ns |  0.87 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **99.82 ns** | **2.725 ns** | **0.149 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  96.39 ns | 1.017 ns | 0.056 ns |  0.97 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **132.73 ns** | **2.113 ns** | **0.116 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 124.57 ns | 2.612 ns | 0.143 ns |  0.94 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

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

| Method                | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated  | Alloc Ratio |
|---------------------- |---------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| **Count**                 | **200**       |  **2.441 ms** | **0.0604 ms** | **0.0033 ms** |  **1.00** |    **0.00** |   **15.6250** |   **11.7188** |         **-** |     **303 KB** |        **1.00** |
| Tfidf                 | 200       |  2.445 ms | 0.1179 ms | 0.0065 ms |  1.00 |    0.00 |   19.5313 |   15.6250 |         - |  332.14 KB |        1.10 |
| CountBigrams          | 200       |  2.894 ms | 0.1143 ms | 0.0063 ms |  1.19 |    0.00 |   35.1563 |   23.4375 |         - |  590.34 KB |        1.95 |
| CountCharWordBoundary | 200       |  2.225 ms | 0.0972 ms | 0.0053 ms |  0.91 |    0.00 |  382.8125 |  382.8125 |  382.8125 |  1685.7 KB |        5.56 |
| Hashing               | 200       |  2.432 ms | 0.0904 ms | 0.0050 ms |  1.00 |    0.00 |   11.7188 |    7.8125 |         - |  233.65 KB |        0.77 |
|                       |           |           |           |           |       |         |           |           |           |            |             |
| **Count**                 | **1000**      |  **4.580 ms** | **0.3684 ms** | **0.0202 ms** |  **1.00** |    **0.01** |  **109.3750** |  **109.3750** |  **109.3750** | **1280.03 KB** |        **1.00** |
| Tfidf                 | 1000      |  4.728 ms | 0.3145 ms | 0.0172 ms |  1.03 |    0.01 |  140.6250 |  140.6250 |  140.6250 | 1424.43 KB |        1.11 |
| CountBigrams          | 1000      |  6.853 ms | 0.3304 ms | 0.0181 ms |  1.50 |    0.01 |  398.4375 |  398.4375 |  398.4375 | 2291.91 KB |        1.79 |
| CountCharWordBoundary | 1000      | 13.629 ms | 4.7182 ms | 0.2586 ms |  2.98 |    0.05 | 1046.8750 | 1015.6250 | 1015.6250 | 12022.2 KB |        9.39 |
| Hashing               | 1000      |  4.575 ms | 0.2863 ms | 0.0157 ms |  1.00 |    0.00 |   70.3125 |   70.3125 |   70.3125 | 1015.48 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

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

| Method   | Documents | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|------------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **5.409 ms** |   **0.6442 ms** |  **0.0353 ms** |  **1.00** |    **0.01** |   **140.6250** |   **140.6250** |   **140.6250** |   **2.01 MB** |        **1.00** |
| MlNet    | 200       |  54.175 ms |  30.9522 ms |  1.6966 ms | 10.02 |    0.28 |  7400.0000 |  7400.0000 |  7400.0000 |  28.27 MB |       14.10 |
|          |           |            |             |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **20.309 ms** |   **1.9326 ms** |  **0.1059 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **9.17 MB** |        **1.00** |
| MlNet    | 1000      | 443.582 ms | 351.3403 ms | 19.2581 ms | 21.84 |    0.83 | 79000.0000 | 79000.0000 | 79000.0000 |  324.3 MB |       35.35 |

<!-- markdownlint-enable MD060 -->

### compare-cdist

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'rapidfuzz': '3.14.6', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| cdist_ratio_n50 | 0.149 | 0.033 | 0.22x | 0.149 | 0.033 | 0.22x |
| cdist_wratio_n50 | 2.008 | 2.375 | 1.18x | 2.008 | 2.374 | 1.18x |
| cdist_ratio_varied_n50 | 0.130 | 0.027 | 0.21x | 0.130 | 0.027 | 0.21x |
| cdist_ratio_varied_cutoff90_n50 | 0.053 | 0.027 | 0.51x | 0.052 | 0.027 | 0.51x |
| cdist_ratio_n200 | 2.649 | 0.396 | 0.15x | 2.653 | 0.396 | 0.15x |
| cdist_wratio_n200 | 29.563 | 36.175 | 1.22x | 29.564 | 36.172 | 1.22x |
| cdist_ratio_varied_n200 | 2.328 | 0.319 | 0.14x | 2.331 | 0.319 | 0.14x |
| cdist_ratio_varied_cutoff90_n200 | 0.990 | 0.319 | 0.32x | 0.993 | 0.319 | 0.32x |
| cdist_ratio_n500 | 16.767 | 2.373 | 0.14x | 16.783 | 2.373 | 0.14x |
| cdist_wratio_n500 | 177.759 | 218.053 | 1.23x | 177.895 | 218.018 | 1.23x |
| cdist_ratio_varied_n500 | 13.899 | 1.870 | 0.13x | 14.014 | 1.870 | 0.13x |
| cdist_ratio_varied_cutoff90_n500 | 5.540 | 1.874 | 0.34x | 5.631 | 1.874 | 0.33x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.380 | 3.028 | 7.97x | 0.380 | 3.028 | 7.97x |
| glm_gamma_n1000 | 0.465 | 3.916 | 8.42x | 0.465 | 3.915 | 8.42x |
| glm_poisson_exposure_n1000 | 0.394 | 3.520 | 8.93x | 0.394 | 3.520 | 8.93x |
| mnlogit_n1000 | 0.830 | 16.558 | 19.96x | 0.830 | 16.557 | 19.96x |
| glm_negative_binomial_n10000 | 3.453 | 11.529 | 3.34x | 3.460 | 11.529 | 3.33x |
| glm_gamma_n10000 | 3.994 | 13.276 | 3.32x | 4.002 | 13.275 | 3.32x |
| glm_poisson_exposure_n10000 | 4.236 | 13.197 | 3.12x | 4.244 | 13.195 | 3.11x |
| mnlogit_n10000 | 8.269 | 98.812 | 11.95x | 8.275 | 98.797 | 11.94x |
| glm_negative_binomial_n100000 | 35.197 | 113.007 | 3.21x | 35.418 | 450.658 | 12.72x |
| glm_gamma_n100000 | 41.527 | 129.531 | 3.12x | 41.717 | 515.191 | 12.35x |
| glm_poisson_exposure_n100000 | 54.295 | 125.007 | 2.30x | 54.815 | 497.731 | 9.08x |
| mnlogit_n100000 | 83.164 | 921.842 | 11.08x | 83.195 | 1617.663 | 19.44x |

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

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.005 | 0.773 | 152.27x | 0.005 | 0.773 | 152.28x |
| accuracy_n1000_k2 | 0.000 | 0.402 | 3353.01x | 0.000 | 0.402 | 3353.00x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.432 | 301.75x | 0.005 | 1.431 | 301.74x |
| classification_report_n1000_k2 | 0.007 | 5.452 | 788.40x | 0.007 | 5.451 | 788.39x |
| roc_auc_binary_n1000_k2 | 0.016 | 1.547 | 94.68x | 0.016 | 1.546 | 94.68x |
| balanced_accuracy_n1000_k2 | 0.005 | 0.823 | 171.04x | 0.005 | 0.822 | 171.04x |
| matthews_n1000_k2 | 0.005 | 1.599 | 344.87x | 0.005 | 1.598 | 344.83x |
| cohen_kappa_n1000_k2 | 0.005 | 0.868 | 186.41x | 0.005 | 0.868 | 186.42x |
| mse_n1000_k2 | 0.001 | 0.213 | 409.95x | 0.001 | 0.213 | 409.97x |
| mae_n1000_k2 | 0.001 | 0.212 | 405.12x | 0.001 | 0.212 | 405.03x |
| median_ae_n1000_k2 | 0.005 | 0.232 | 48.76x | 0.005 | 0.232 | 48.76x |
| r2_n1000_k2 | 0.001 | 0.268 | 202.49x | 0.001 | 0.268 | 202.48x |
| confusion_matrix_n1000_k10 | 0.005 | 0.774 | 153.08x | 0.005 | 0.774 | 153.07x |
| accuracy_n1000_k10 | 0.000 | 0.405 | 3426.12x | 0.000 | 0.405 | 3426.24x |
| precision_recall_f1_macro_n1000_k10 | 0.005 | 1.437 | 289.60x | 0.005 | 1.437 | 289.55x |
| classification_report_n1000_k10 | 0.010 | 5.556 | 544.33x | 0.010 | 5.555 | 544.27x |
| roc_auc_ovr_macro_n1000_k10 | 0.189 | 8.075 | 42.81x | 0.189 | 8.073 | 42.79x |
| balanced_accuracy_n1000_k10 | 0.005 | 0.825 | 166.14x | 0.005 | 0.825 | 166.15x |
| matthews_n1000_k10 | 0.005 | 1.622 | 328.74x | 0.005 | 1.622 | 328.73x |
| cohen_kappa_n1000_k10 | 0.005 | 0.882 | 164.75x | 0.005 | 0.882 | 164.72x |
| mse_n1000_k10 | 0.001 | 0.213 | 412.60x | 0.001 | 0.213 | 412.57x |
| mae_n1000_k10 | 0.001 | 0.212 | 414.15x | 0.001 | 0.212 | 414.19x |
| median_ae_n1000_k10 | 0.005 | 0.228 | 48.17x | 0.005 | 0.228 | 48.17x |
| r2_n1000_k10 | 0.001 | 0.270 | 203.61x | 0.001 | 0.270 | 203.62x |
| confusion_matrix_n100000_k2 | 0.475 | 10.954 | 23.08x | 0.475 | 10.954 | 23.08x |
| accuracy_n100000_k2 | 0.012 | 3.779 | 327.66x | 0.012 | 3.779 | 327.67x |
| precision_recall_f1_macro_n100000_k2 | 0.445 | 12.591 | 28.33x | 0.444 | 12.591 | 28.33x |
| classification_report_n100000_k2 | 0.448 | 27.052 | 60.36x | 0.448 | 27.046 | 60.35x |
| roc_auc_binary_n100000_k2 | 2.967 | 28.397 | 9.57x | 2.967 | 28.396 | 9.57x |
| balanced_accuracy_n100000_k2 | 0.445 | 11.050 | 24.86x | 0.444 | 11.048 | 24.86x |
| matthews_n100000_k2 | 0.444 | 22.106 | 49.74x | 0.444 | 22.105 | 49.74x |
| cohen_kappa_n100000_k2 | 0.445 | 11.078 | 24.87x | 0.445 | 11.076 | 24.87x |
| mse_n100000_k2 | 0.048 | 0.369 | 7.73x | 0.048 | 0.369 | 7.72x |
| mae_n100000_k2 | 0.047 | 0.369 | 7.85x | 0.047 | 0.369 | 7.85x |
| median_ae_n100000_k2 | 0.583 | 1.894 | 3.25x | 0.625 | 1.894 | 3.03x |
| r2_n100000_k2 | 0.122 | 0.588 | 4.81x | 0.122 | 0.588 | 4.81x |
| confusion_matrix_n100000_k10 | 0.416 | 10.959 | 26.35x | 0.416 | 10.955 | 26.34x |
| accuracy_n100000_k10 | 0.012 | 3.785 | 311.13x | 0.012 | 3.784 | 311.10x |
| precision_recall_f1_macro_n100000_k10 | 0.417 | 13.300 | 31.92x | 0.417 | 13.297 | 31.91x |
| classification_report_n100000_k10 | 0.424 | 29.899 | 70.58x | 0.424 | 29.898 | 70.58x |
| roc_auc_ovr_macro_n100000_k10 | 29.526 | 231.736 | 7.85x | 29.520 | 231.705 | 7.85x |
| balanced_accuracy_n100000_k10 | 0.416 | 11.025 | 26.47x | 0.416 | 11.023 | 26.47x |
| matthews_n100000_k10 | 0.417 | 22.861 | 54.88x | 0.417 | 22.860 | 54.88x |
| cohen_kappa_n100000_k10 | 0.475 | 11.071 | 23.30x | 0.475 | 11.070 | 23.29x |
| mse_n100000_k10 | 0.048 | 0.370 | 7.73x | 0.048 | 0.370 | 7.73x |
| mae_n100000_k10 | 0.047 | 0.368 | 7.80x | 0.047 | 0.368 | 7.80x |
| median_ae_n100000_k10 | 0.594 | 1.891 | 3.19x | 0.636 | 1.891 | 2.97x |
| r2_n100000_k10 | 0.122 | 0.583 | 4.77x | 0.122 | 0.583 | 4.77x |
| confusion_matrix_n1000000_k2 | 4.454 | 102.596 | 23.03x | 4.453 | 102.564 | 23.03x |
| accuracy_n1000000_k2 | 0.116 | 34.225 | 296.31x | 0.115 | 34.206 | 296.18x |
| precision_recall_f1_macro_n1000000_k2 | 4.453 | 112.738 | 25.32x | 4.452 | 112.713 | 25.32x |
| classification_report_n1000000_k2 | 4.454 | 219.448 | 49.26x | 4.454 | 219.420 | 49.26x |
| roc_auc_binary_n1000000_k2 | 47.529 | 310.300 | 6.53x | 47.526 | 310.253 | 6.53x |
| balanced_accuracy_n1000000_k2 | 4.453 | 102.696 | 23.06x | 4.453 | 102.679 | 23.06x |
| matthews_n1000000_k2 | 4.451 | 206.943 | 46.50x | 4.450 | 206.930 | 46.50x |
| cohen_kappa_n1000000_k2 | 4.453 | 102.719 | 23.06x | 4.453 | 102.709 | 23.07x |
| mse_n1000000_k2 | 0.495 | 1.851 | 3.74x | 0.495 | 1.851 | 3.74x |
| mae_n1000000_k2 | 0.487 | 1.860 | 3.82x | 0.487 | 1.859 | 3.82x |
| median_ae_n1000000_k2 | 5.339 | 15.542 | 2.91x | 5.411 | 15.539 | 2.87x |
| r2_n1000000_k2 | 1.222 | 3.480 | 2.85x | 1.222 | 3.480 | 2.85x |
| confusion_matrix_n1000000_k10 | 4.112 | 102.569 | 24.95x | 4.112 | 102.551 | 24.94x |
| accuracy_n1000000_k10 | 0.113 | 34.194 | 302.80x | 0.113 | 34.193 | 302.82x |
| precision_recall_f1_macro_n1000000_k10 | 4.121 | 119.447 | 28.98x | 4.121 | 119.427 | 28.98x |
| classification_report_n1000000_k10 | 4.150 | 246.987 | 59.51x | 4.149 | 246.979 | 59.52x |
| balanced_accuracy_n1000000_k10 | 4.100 | 102.668 | 25.04x | 4.099 | 102.661 | 25.04x |
| matthews_n1000000_k10 | 4.103 | 214.285 | 52.23x | 4.102 | 214.277 | 52.23x |
| cohen_kappa_n1000000_k10 | 4.102 | 102.528 | 24.99x | 4.102 | 102.513 | 24.99x |
| mse_n1000000_k10 | 0.479 | 1.807 | 3.77x | 0.479 | 1.807 | 3.77x |
| mae_n1000000_k10 | 0.472 | 1.798 | 3.81x | 0.472 | 1.797 | 3.81x |
| median_ae_n1000000_k10 | 5.274 | 15.512 | 2.94x | 5.372 | 15.510 | 2.89x |
| r2_n1000000_k10 | 1.221 | 3.453 | 2.83x | 1.221 | 3.452 | 2.83x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-nmf-transform

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scikit-learn': '1.9.1', 'scipy': '1.18.1'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| transform_frobenius_n100 | 2.032 | 0.756 | 0.37x | 2.031 | 0.756 | 0.37x |
| transform_frobenius_n500 | 6.314 | 1.548 | 0.25x | 6.313 | 1.548 | 0.25x |
| transform_frobenius_n2000 | 22.576 | 5.603 | 0.25x | 22.610 | 22.388 | 0.99x |
| transform_kl_n100 | 1.149 | 14.550 | 12.67x | 1.148 | 14.550 | 12.67x |
| transform_kl_n500 | 5.126 | 30.833 | 6.02x | 5.126 | 30.832 | 6.02x |
| transform_kl_n2000 | 20.195 | 115.414 | 5.72x | 20.254 | 423.954 | 20.93x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.049 | 2.548 | 51.79x | 0.049 | 2.548 | 51.78x |
| ols_hac_n1000 | 0.172 | 3.119 | 18.09x | 0.172 | 3.119 | 18.09x |
| ols_cluster_n1000 | 0.064 | 3.159 | 49.20x | 0.064 | 3.158 | 49.20x |
| ols_summary_n10000 | 0.463 | 11.380 | 24.59x | 0.463 | 11.378 | 24.59x |
| ols_hac_n10000 | 1.773 | 12.712 | 7.17x | 1.780 | 12.712 | 7.14x |
| ols_cluster_n10000 | 0.632 | 13.028 | 20.62x | 0.635 | 13.002 | 20.46x |
| ols_summary_n100000 | 4.577 | 107.361 | 23.46x | 4.602 | 428.588 | 93.14x |
| ols_hac_n100000 | 17.633 | 116.107 | 6.58x | 17.908 | 463.200 | 25.87x |
| ols_cluster_n100000 | 6.772 | 120.381 | 17.78x | 6.953 | 480.388 | 69.09x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.900 | 9.883 | 2.02x | 5.222 | 9.882 | 1.89x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 11.936 | 17.403 | 1.46x | 12.450 | 17.401 | 1.40x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.044 | 38.085 | 3.16x | 12.394 | 38.077 | 3.07x | 1,990,038 | 1,990,038 |
| spiece_model | 4.906 | 30.701 | 6.26x | 5.097 | 30.696 | 6.02x | 533,084 | 533,084 |
| tfidf_save | 1.890 | 2.419 | 1.28x | 1.922 | 2.419 | 1.26x | 581,787 | 591,922 |
| tfidf_load | 5.019 | 4.303 | 0.86x | 5.292 | 4.302 | 0.81x | 581,787 | 591,922 |
| embedding_index_save | 3.956 | 1.342 | 0.34x | 4.151 | 1.341 | 0.32x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.233 | 37.222 | 0.76x | 9.666 | 4.514 | 0.47x | 20,589,007 | 15,360,128 |
| embedding_index_load | 4.515 | 1.375 | 0.30x | 4.827 | 1.375 | 0.28x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 5.340 | 0.949 | 0.18x | 5.691 | 0.949 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 3.390 | 1.400 | 0.41x | 3.698 | 1.400 | 0.38x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.072 | 1.399 | 1.31x | 1.298 | 1.399 | 1.08x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 76.88x | 0.000 | 0.001 | 76.87x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 453.512 | 638.422 | 1.41x | 453.600 | 638.308 | 1.41x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 74.284 | 72.550 | 0.98x | 74.272 | 72.542 | 0.98x | 15,250,490 | 14,022,374 |

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

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.635 | 141.24x | 0.004 | 0.635 | 141.23x |
| mann_whitney_n1000 | 0.031 | 0.594 | 18.99x | 0.031 | 0.594 | 18.99x |
| chi_square_n1000 | 0.000 | 0.251 | 1405.07x | 0.000 | 0.251 | 1405.18x |
| welch_t_n10000 | 0.042 | 0.680 | 16.06x | 0.042 | 0.680 | 16.07x |
| mann_whitney_n10000 | 0.484 | 3.000 | 6.20x | 0.484 | 2.999 | 6.20x |
| chi_square_n10000 | 0.001 | 0.255 | 197.41x | 0.001 | 0.255 | 197.42x |
| welch_t_n100000 | 0.423 | 1.102 | 2.60x | 0.423 | 1.101 | 2.60x |
| mann_whitney_n100000 | 6.168 | 30.969 | 5.02x | 6.168 | 30.965 | 5.02x |
| chi_square_n100000 | 0.014 | 0.276 | 19.34x | 0.014 | 0.276 | 19.34x |

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

_As of 2026-09-24, measured at commit `9f9406c5b886c83d62bb9e18ea4a73d891564409`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.041 | 2.514 | 61.40x | 0.041 | 2.514 | 61.40x |
| var_n10000 | 0.497 | 16.052 | 32.29x | 0.500 | 16.051 | 32.13x |
| var_n100000 | 4.178 | 154.030 | 36.87x | 4.349 | 364.651 | 83.84x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
