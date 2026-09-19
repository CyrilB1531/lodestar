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

| Method                     | Mean      | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          | 23.589 ns |  0.1918 ns | 0.0105 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  9.427 ns |  0.0644 ns | 0.0035 ns |  0.40 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail | 28.050 ns |  1.0756 ns | 0.0590 ns |  1.19 |    0.00 |         - |          NA |
| ChiSquaredSfHundredDf      | 59.668 ns |  0.6424 ns | 0.0352 ns |  2.53 |    0.00 |         - |          NA |
| ChiSquaredSfFractionalDf   | 94.525 ns | 14.7553 ns | 0.8088 ns |  4.01 |    0.03 |         - |          NA |
| NormalQuantile             | 90.758 ns |  0.1317 ns | 0.0072 ns |  3.85 |    0.00 |         - |          NA |

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

| Method            | SampleSize | Mean        | Error     | StdDev   | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------- |------------:|----------:|---------:|------:|-------:|----------:|------------:|
| **Before_Asymptotic** | **1000**       |    **25.03 μs** |  **1.159 μs** | **0.064 μs** |  **1.00** | **0.9460** |  **15.73 KB** |        **1.00** |
| After_Auto        | 1000       |    28.35 μs |  5.377 μs | 0.295 μs |  1.13 | 0.9460 |  15.67 KB |        1.00 |
|                   |            |             |           |          |       |        |           |             |
| **Before_Asymptotic** | **10000**      | **1,682.55 μs** | **62.668 μs** | **3.435 μs** |  **1.00** | **7.8125** |  **156.3 KB** |        **1.00** |
| After_Auto        | 10000      | 1,273.83 μs | 41.181 μs | 2.257 μs |  0.76 | 7.8125 |  156.3 KB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.MetaNumericsStatsBenchmarks-report-github

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

| Method                         | SampleSize | Mean            | Error          | StdDev        | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |----------- |----------------:|---------------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| **Lodestar_StudentT**              | **100**        |       **664.98 ns** |     **112.501 ns** |      **6.167 ns** |   **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 100        |     1,924.28 ns |     180.462 ns |      9.892 ns |   2.89 |    0.03 | 0.0038 |     104 B |          NA |
| Lodestar_MannWhitney           | 100        |     2,013.16 ns |     521.479 ns |     28.584 ns |   3.03 |    0.04 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 100        |     3,008.45 ns |     172.558 ns |      9.458 ns |   4.52 |    0.04 | 0.0687 |    1176 B |          NA |
| Lodestar_KruskalWallis         | 100        |     5,565.45 ns |   2,200.825 ns |    120.635 ns |   8.37 |    0.17 |      - |      32 B |          NA |
| MetaNumerics_KruskalWallis     | 100        |     7,312.55 ns |     521.022 ns |     28.559 ns |  11.00 |    0.10 | 0.1068 |    1896 B |          NA |
| Lodestar_KolmogorovSmirnov     | 100        |     2,035.43 ns |   1,077.108 ns |     59.040 ns |   3.06 |    0.08 | 0.0954 |    1648 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 100        |     3,075.45 ns |     714.947 ns |     39.189 ns |   4.63 |    0.06 | 0.0687 |    1168 B |          NA |
| Lodestar_OneWayAnova           | 100        |       793.93 ns |      58.173 ns |      3.189 ns |   1.19 |    0.01 | 0.0019 |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 100        |     2,735.63 ns |      97.763 ns |      5.359 ns |   4.11 |    0.03 | 0.0420 |     744 B |          NA |
| Lodestar_Wilcoxon              | 100        |       820.85 ns |       1.743 ns |      0.096 ns |   1.23 |    0.01 | 0.0486 |     824 B |          NA |
| MetaNumerics_Wilcoxon          | 100        |       811.39 ns |      18.960 ns |      1.039 ns |   1.22 |    0.01 | 0.0582 |     976 B |          NA |
| Lodestar_FisherExact           | 100        |       341.12 ns |      14.264 ns |      0.782 ns |   0.51 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 100        |     1,189.01 ns |     104.577 ns |      5.732 ns |   1.79 |    0.02 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 100        |        87.43 ns |       5.989 ns |      0.328 ns |   0.13 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 100        |       539.07 ns |      61.067 ns |      3.347 ns |   0.81 |    0.01 | 0.0572 |     968 B |          NA |
|                                |            |                 |                |               |        |         |        |           |             |
| **Lodestar_StudentT**              | **10000**      |    **32,865.50 ns** |     **112.851 ns** |      **6.186 ns** |  **1.000** |    **0.00** |      **-** |         **-** |          **NA** |
| MetaNumerics_StudentT          | 10000      |   158,360.84 ns |   2,665.406 ns |    146.100 ns |  4.818 |    0.00 |      - |     104 B |          NA |
| Lodestar_MannWhitney           | 10000      |   344,376.72 ns |  47,148.514 ns |  2,584.369 ns | 10.478 |    0.07 |      - |         - |          NA |
| MetaNumerics_MannWhitney       | 10000      | 1,849,465.63 ns | 232,095.204 ns | 12,721.921 ns | 56.274 |    0.34 | 3.9063 |   80379 B |          NA |
| Lodestar_KruskalWallis         | 10000      |   918,470.71 ns |  26,619.805 ns |  1,459.121 ns | 27.946 |    0.04 |      - |      33 B |          NA |
| MetaNumerics_KruskalWallis     | 10000      | 3,159,162.89 ns | 106,453.684 ns |  5,835.086 ns | 96.124 |    0.15 | 3.9063 |  120702 B |          NA |
| Lodestar_KolmogorovSmirnov     | 10000      | 1,287,152.76 ns |  82,128.019 ns |  4,501.714 ns | 39.164 |    0.12 | 7.8125 |  160051 B |          NA |
| MetaNumerics_KolmogorovSmirnov | 10000      | 1,893,530.97 ns | 133,192.908 ns |  7,300.753 ns | 57.615 |    0.19 | 3.9063 |   80371 B |          NA |
| Lodestar_OneWayAnova           | 10000      |    74,143.61 ns |  11,343.829 ns |    621.794 ns |  2.256 |    0.02 |      - |      32 B |          NA |
| MetaNumerics_OneWayAnova       | 10000      |   238,077.11 ns |   6,929.102 ns |    379.807 ns |  7.244 |    0.01 |      - |     744 B |          NA |
| Lodestar_Wilcoxon              | 10000      |   140,394.85 ns |   6,527.072 ns |    357.771 ns |  4.272 |    0.01 | 4.6387 |   80056 B |          NA |
| MetaNumerics_Wilcoxon          | 10000      |   159,834.55 ns |  11,049.649 ns |    605.669 ns |  4.863 |    0.02 | 4.6387 |   80176 B |          NA |
| Lodestar_FisherExact           | 10000      |       338.18 ns |       3.750 ns |      0.206 ns |  0.010 |    0.00 |      - |         - |          NA |
| MetaNumerics_FisherExact       | 10000      |     1,185.53 ns |      11.663 ns |      0.639 ns |  0.036 |    0.00 | 0.0553 |     944 B |          NA |
| Lodestar_ChiSquare             | 10000      |        87.29 ns |       6.680 ns |      0.366 ns |  0.003 |    0.00 | 0.0100 |     168 B |          NA |
| MetaNumerics_ChiSquare         | 10000      |       534.63 ns |      22.862 ns |      1.253 ns |  0.016 |    0.00 | 0.0572 |     968 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-09-18, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

| Method                    | SampleSize | Ties  | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------- |----------- |------ |------------:|------------:|---------:|---------:|---------:|---------:|-----------:|
| **KruskalWallisTest**         | **10000**      | **False** |    **943.6 μs** |   **159.70 μs** |  **8.75 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | False |    196.7 μs |    88.98 μs |  4.88 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | False |    628.0 μs |   159.24 μs |  8.73 μs |  16.6016 |        - |        - |   279681 B |
| WilcoxonPooledDifferences | 10000      | False |    728.7 μs |   169.03 μs |  9.27 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | False |    358.4 μs |    99.83 μs |  5.47 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **10000**      | **True**  |    **430.9 μs** |    **11.27 μs** |  **0.62 μs** |        **-** |        **-** |        **-** |       **81 B** |
| WilcoxonPaired            | 10000      | True  |    163.1 μs |     7.61 μs |  0.42 μs |   4.6387 |        - |        - |    80056 B |
| KruskalWallisManyGroups   | 10000      | True  |    142.9 μs |    31.29 μs |  1.72 μs |  16.6016 |        - |        - |   279680 B |
| WilcoxonPooledDifferences | 10000      | True  |    491.4 μs |   194.78 μs | 10.68 μs |        - |        - |        - |          - |
| MannWhitneyTest           | 10000      | True  |    277.9 μs |     6.49 μs |  0.36 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **False** |  **9,546.0 μs** |   **346.68 μs** | **19.00 μs** |        **-** |        **-** |        **-** |          **-** |
| WilcoxonPaired            | 100000     | False |  2,774.1 μs |   297.14 μs | 16.29 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | False |  8,562.8 μs |   113.83 μs |  6.24 μs | 343.7500 | 343.7500 | 343.7500 |  2800358 B |
| WilcoxonPooledDifferences | 100000     | False | 30,169.7 μs | 1,775.99 μs | 97.35 μs | 968.7500 | 968.7500 | 968.7500 | 13200772 B |
| MannWhitneyTest           | 100000     | False |  4,090.6 μs |   246.92 μs | 13.53 μs |        - |        - |        - |          - |
| **KruskalWallisTest**         | **100000**     | **True**  |  **4,247.2 μs** |   **423.63 μs** | **23.22 μs** |        **-** |        **-** |        **-** |       **88 B** |
| WilcoxonPaired            | 100000     | True  |  2,081.1 μs |   102.23 μs |  5.60 μs | 113.2813 | 113.2813 | 113.2813 |   800172 B |
| KruskalWallisManyGroups   | 100000     | True  |  5,020.7 μs |   126.29 μs |  6.92 μs | 367.1875 | 367.1875 | 367.1875 |  2800370 B |
| WilcoxonPooledDifferences | 100000     | True  | 18,566.9 μs |   486.20 μs | 26.65 μs | 968.7500 | 968.7500 | 968.7500 | 13076300 B |
| MannWhitneyTest           | 100000     | True  |  2,761.1 μs |    62.91 μs |  3.45 μs |        - |        - |        - |          - |

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

| Method                        | SampleSize | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------------------------ |----------- |-----------:|------------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarAugmentedDickeyFuller** | **200**        |   **4.453 μs** |   **1.2362 μs** | **0.0678 μs** |  **1.00** |    **0.02** |   **1.2741** |   **0.0305** |        **-** |  **20.82 KB** |        **1.00** |
| LodestarAdfAutolag            | 200        |  20.378 μs |  11.0377 μs | 0.6050 μs |  4.58 |    0.13 |   3.5706 |        - |        - |  58.88 KB |        2.83 |
| CortexAugmentedDickeyFuller   | 200        |  22.156 μs |   1.9361 μs | 0.1061 μs |  4.98 |    0.07 |   0.9766 |   0.0305 |        - |  16.18 KB |        0.78 |
| LodestarKpss                  | 200        |   3.098 μs |   0.1035 μs | 0.0057 μs |  0.70 |    0.01 |   0.1030 |        - |        - |   1.69 KB |        0.08 |
| CortexKpss                    | 200        |   3.269 μs |   0.9850 μs | 0.0540 μs |  0.73 |    0.01 |   0.2098 |        - |        - |   3.43 KB |        0.16 |
| LodestarDecompose             | 200        |   1.162 μs |   0.3661 μs | 0.0201 μs |  0.26 |    0.01 |   0.3967 |   0.0038 |        - |    6.5 KB |        0.31 |
| CortexDecompose               | 200        |   2.075 μs |   0.1199 μs | 0.0066 μs |  0.47 |    0.01 |   0.4997 |   0.0076 |        - |   8.16 KB |        0.39 |
|                               |            |            |             |           |       |         |          |          |          |           |             |
| **LodestarAugmentedDickeyFuller** | **2000**       |  **77.182 μs** |   **8.7094 μs** | **0.4774 μs** |  **1.00** |    **0.01** |  **30.2734** |  **30.2734** |  **30.2734** | **203.65 KB** |        **1.00** |
| LodestarAdfAutolag            | 2000       | 759.813 μs | 170.8887 μs | 9.3670 μs |  9.84 |    0.12 | 249.0234 | 249.0234 | 249.0234 | 957.73 KB |        4.70 |
| CortexAugmentedDickeyFuller   | 2000       | 255.561 μs |  14.2193 μs | 0.7794 μs |  3.31 |    0.02 |  30.2734 |  30.2734 |  30.2734 | 142.76 KB |        0.70 |
| LodestarKpss                  | 2000       |  82.913 μs |   2.4593 μs | 0.1348 μs |  1.07 |    0.01 |   0.8545 |        - |        - |  15.75 KB |        0.08 |
| CortexKpss                    | 2000       |  85.810 μs |  36.3098 μs | 1.9903 μs |  1.11 |    0.02 |   1.8311 |        - |        - |  31.55 KB |        0.15 |
| LodestarDecompose             | 2000       |  11.581 μs |   5.3684 μs | 0.2943 μs |  0.15 |    0.00 |   3.8300 |   0.4120 |        - |  62.75 KB |        0.31 |
| CortexDecompose               | 2000       |  21.992 μs |   1.5438 μs | 0.0846 μs |  0.28 |    0.00 |   4.7913 |   0.4578 |        - |  78.48 KB |        0.39 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-09-18, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

| Method              | SampleSize | Mean            | Error           | StdDev       | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|-------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |        **969.9 ns** |        **69.54 ns** |      **3.81 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     38,463.3 ns |     4,883.60 ns |    267.69 ns |   39.66 |    0.27 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      7,421.8 ns |       609.71 ns |     33.42 ns |    7.65 |    0.04 |   0.5341 |        - |        - |    8944 B |          NA |
| AccordMannWhitney   | 100        |     22,471.7 ns |     1,030.38 ns |     56.48 ns |   23.17 |    0.09 |   1.3733 |   0.0305 |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |        302.5 ns |         4.74 ns |      0.26 ns |    0.31 |    0.00 |   0.0119 |        - |        - |     200 B |          NA |
| AccordChiSquare     | 100        |        213.0 ns |         3.94 ns |      0.22 ns |    0.22 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
|                     |            |                 |                 |              |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **37,656.8 ns** |     **2,692.75 ns** |    **147.60 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    141,090.4 ns |    15,149.84 ns |    830.41 ns |   3.747 |    0.02 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |  4,639,907.9 ns |   101,312.90 ns |  5,553.30 ns | 123.217 |    0.44 | 242.1875 | 242.1875 | 242.1875 |  880312 B |          NA |
| AccordMannWhitney   | 10000      | 12,481,054.0 ns | 1,082,005.79 ns | 59,308.39 ns | 331.446 |    1.77 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        306.8 ns |         3.93 ns |      0.22 ns |   0.008 |    0.00 |   0.0119 |        - |        - |     200 B |          NA |
| AccordChiSquare     | 10000      |        211.4 ns |        14.42 ns |      0.79 ns |   0.006 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

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

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                  | Mean      | Error    | StdDev   | Gen0      | Allocated |
|------------------------ |----------:|---------:|---------:|----------:|----------:|
| ChatTemplate            | 104.32 ms | 4.222 ms | 0.231 ms | 2000.0000 |   33.3 MB |
| ProseWithoutAddedTokens |  85.05 ms | 3.384 ms | 0.186 ms | 1666.6667 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AgglomerativeIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                 | Rows | Method   | Mean           | Error         | StdDev      | Ratio  | RatioSD | Gen0       | Gen1       | Gen2      | Allocated    | Alloc Ratio |
|----------------------- |----- |--------- |---------------:|--------------:|------------:|-------:|--------:|-----------:|-----------:|----------:|-------------:|------------:|
| **Lodestar_Fit**           | **500**  | **average**  |     **1,886.8 μs** |      **36.01 μs** |     **1.97 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |   **1022.55 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | average  |   102,926.0 μs |   2,915.39 μs |   159.80 μs |  54.55 |    0.09 |  3666.6667 |  1500.0000 |  833.3333 |  59880.38 KB |       58.56 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **complete** |     **1,833.7 μs** |     **134.56 μs** |     **7.38 μs** |   **1.00** |    **0.00** |   **248.0469** |   **248.0469** |  **248.0469** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | complete |   102,297.6 μs |  26,392.46 μs | 1,446.66 μs |  55.79 |    0.71 |  5000.0000 |  1800.0000 |  800.0000 |  83111.74 KB |       81.29 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **single**   |       **722.7 μs** |      **42.82 μs** |     **2.35 μs** |   **1.00** |    **0.00** |     **0.9766** |          **-** |         **-** |     **30.34 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | single   |   134,759.6 μs |  10,797.06 μs |   591.82 μs | 186.46 |    0.88 |  4750.0000 |  1750.0000 |  750.0000 |  81289.96 KB |    2,678.88 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **500**  | **ward**     |     **2,181.0 μs** |      **77.03 μs** |     **4.22 μs** |   **1.00** |    **0.00** |   **246.0938** |   **246.0938** |  **246.0938** |    **1022.4 KB** |        **1.00** |
| Aglomera_GetClustering | 500  | ward     |    84,617.3 μs |   5,439.37 μs |   298.15 μs |  38.80 |    0.14 |  3857.1429 |  1714.2857 |  857.1429 |  64191.18 KB |       62.78 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **average**  |    **13,703.9 μs** |     **836.17 μs** |    **45.83 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.15 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | average  | 1,844,665.9 μs |  38,347.24 μs | 2,101.94 μs | 134.61 |    0.41 | 35000.0000 | 10000.0000 | 4000.0000 | 537152.37 KB |       60.18 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **complete** |    **13,071.3 μs** |     **379.73 μs** |    **20.81 μs** |   **1.00** |    **0.00** |   **500.0000** |   **500.0000** |  **500.0000** |      **8925 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | complete | 1,990,562.9 μs |  51,734.18 μs | 2,835.73 μs | 152.28 |    0.28 | 48000.0000 | 12000.0000 | 4000.0000 | 747352.87 KB |       83.74 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **single**   |     **5,881.4 μs** |     **541.40 μs** |    **29.68 μs** |   **1.00** |    **0.01** |          **-** |          **-** |         **-** |     **89.92 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | single   | 3,562,679.8 μs | 104,318.26 μs | 5,718.04 μs | 605.76 |    2.77 | 47000.0000 | 12000.0000 | 4000.0000 |  732674.2 KB |    8,147.90 |
|                        |      |          |                |               |             |        |         |            |            |           |              |             |
| **Lodestar_Fit**           | **1500** | **ward**     |    **16,961.7 μs** |  **13,520.84 μs** |   **741.12 μs** |   **1.00** |    **0.05** |   **500.0000** |   **500.0000** |  **500.0000** |   **8925.01 KB** |        **1.00** |
| Aglomera_GetClustering | 1500 | ward     | 1,717,306.6 μs |  97,540.68 μs | 5,346.53 μs | 101.37 |    3.77 | 38000.0000 | 11000.0000 | 4000.0000 | 573618.81 KB |       64.27 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method             | CorpusSize | Mean       | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |   **5.396 μs** |  **0.0897 μs** | **0.0049 μs** |  **1.00** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |   5.586 μs |  0.3751 μs | 0.0206 μs |  1.04 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |   5.629 μs |  0.1053 μs | 0.0058 μs |  1.04 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |  **54.060 μs** |  **1.3819 μs** | **0.0757 μs** |  **1.00** |  **1.7700** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |  21.479 μs |  0.8123 μs | 0.0445 μs |  0.40 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |  21.282 μs |  0.5870 μs | 0.0322 μs |  0.39 |  1.3428 | 0.0305 |  22.19 KB |        0.76 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         | **215.412 μs** |  **4.1784 μs** | **0.2290 μs** |  **1.00** |  **6.5918** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |  83.119 μs |  4.8822 μs | 0.2676 μs |  0.39 |  5.0049 | 0.1221 |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |  71.437 μs | 23.4291 μs | 1.2842 μs |  0.33 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |            |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **846.483 μs** | **31.1392 μs** | **1.7068 μs** |  **1.00** | **26.3672** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        | 319.670 μs | 25.0135 μs | 1.3711 μs |  0.38 | 20.0195 | 2.4414 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        | 265.102 μs | 14.9694 μs | 0.8205 μs |  0.31 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method | Text     | Lowercase | Mean     | Error    | StdDev   | Gen0      | Allocated |
|------- |--------- |---------- |---------:|---------:|---------:|----------:|----------:|
| **Encode** | **Ascii**    | **False**     | **37.21 ms** | **0.787 ms** | **0.043 ms** |  **714.2857** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **41.44 ms** | **3.927 ms** | **0.215 ms** |  **692.3077** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **36.11 ms** | **0.195 ms** | **0.011 ms** |  **714.2857** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **50.84 ms** | **1.038 ms** | **0.057 ms** | **1272.7273** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **40.79 ms** | **1.610 ms** | **0.088 ms** | **1307.6923** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **55.18 ms** | **2.384 ms** | **0.131 ms** | **1500.0000** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method             | Radius | Shape     | Mean      | Error      | StdDev    | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|-----------:|----------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **149.90 ms** |   **9.927 ms** |  **0.544 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  74.61 ms |   1.747 ms |  0.096 ms |  0.50 |    0.00 |  103.71 KB |        3.81 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **155.94 ms** |  **64.430 ms** |  **3.532 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  69.72 ms |   3.457 ms |  0.189 ms |  0.45 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **220.81 ms** |   **1.645 ms** |  **0.090 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 279.39 ms |   1.244 ms |  0.068 ms |  1.27 |    0.00 |  259.28 KB |        2.51 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **226.68 ms** |   **4.784 ms** |  **0.262 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 239.64 ms |   3.620 ms |  0.198 ms |  1.06 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **281.38 ms** |   **8.917 ms** |  **0.489 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 387.72 ms |  25.963 ms |  1.423 ms |  1.38 |    0.00 | 1366.63 KB |        1.44 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **283.62 ms** |  **10.017 ms** |  **0.549 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 354.80 ms |  15.689 ms |  0.860 ms |  1.25 |    0.00 | 1153.52 KB |        1.56 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **324.79 ms** |  **16.042 ms** |  **0.879 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 458.83 ms | 232.723 ms | 12.756 ms |  1.41 |    0.03 |  7216.2 KB |        1.41 |
|                    |        |           |           |            |           |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **333.04 ms** |  **36.849 ms** |  **2.020 ms** |  **1.00** |    **0.01** | **5514.98 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 451.86 ms |  17.106 ms |  0.938 ms |  1.36 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Latin**  | **1000**   |      **48.94 μs** |     **0.661 μs** |   **0.036 μs** |         **-** |
| Cjk    | 1000   |      63.99 μs |     6.310 μs |   0.346 μs |         - |
| **Latin**  | **10000**  |   **4,576.06 μs** |   **127.890 μs** |   **7.010 μs** |         **-** |
| Cjk    | 10000  |   7,001.94 μs |    71.610 μs |   3.925 μs |         - |
| **Latin**  | **65536**  | **207,740.73 μs** | **4,583.463 μs** | **251.235 μs** |         **-** |

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

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **LodestarQuery**          | **1000**      |       **2.007 μs** |      **0.4606 μs** |     **0.0252 μs** |     **1.00** |    **0.02** |    **0.0229** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 1000      |       2.518 μs |      0.4939 μs |     0.0271 μs |     1.25 |    0.02 |    0.0229 |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 1000      |       3.153 μs |      0.3315 μs |     0.0182 μs |     1.57 |    0.02 |    0.3128 |         - |        - |     5264 B |       12.42 |
| LodestarFromText       | 1000      |   7,877.092 μs |    266.5384 μs |    14.6099 μs | 3,924.57 |   42.95 |  968.7500 |  890.6250 | 875.0000 |  4838186 B |   11,410.82 |
| LuceneFromText         | 1000      |   7,315.260 μs |    509.3008 μs |    27.9165 μs | 3,644.65 |   41.25 |   85.9375 |   78.1250 |   7.8125 |  1374823 B |    3,242.51 |
|                        |           |                |                |               |          |         |           |           |          |            |             |
| **LodestarQuery**          | **20000**     |      **24.478 μs** |      **1.6182 μs** |     **0.0887 μs** |     **1.00** |    **0.00** |         **-** |         **-** |        **-** |      **424 B** |        **1.00** |
| LodestarQueryMultiTerm | 20000     |      41.696 μs |      2.4812 μs |     0.1360 μs |     1.70 |    0.01 |         - |         - |        - |      424 B |        1.00 |
| LuceneQuery            | 20000     |      17.926 μs |      0.1346 μs |     0.0074 μs |     0.73 |    0.00 |    0.4883 |         - |        - |     8648 B |       20.40 |
| LodestarFromText       | 20000     | 116,092.092 μs | 48,901.0373 μs | 2,680.4309 μs | 4,742.68 |   95.99 | 2200.0000 | 1200.0000 | 800.0000 | 83749493 B |  197,522.39 |
| LuceneFromText         | 20000     | 143,934.075 μs | 52,583.3012 μs | 2,882.2682 μs | 5,880.10 |  103.62 | 1250.0000 | 1000.0000 |        - | 22417220 B |   52,870.80 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| Unigram | 31.87 ms | 4.418 ms | 0.242 ms |  1.00 |    0.01 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 84.62 ms | 1.356 ms | 0.074 ms |  2.66 |    0.02 | 1666.6667 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **BpeOnOnePathologicalToken** | **512**    |  **19.70 μs** |   **1.743 μs** | **0.096 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **38.18 μs** |   **2.121 μs** | **0.116 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **87.15 μs** |   **1.177 μs** | **0.064 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **314.84 μs** | **180.780 μs** | **9.909 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method            | Mean     | Error    | StdDev   | Allocated |
|------------------ |---------:|---------:|---------:|----------:|
| EncodeUnseenProse | 18.91 ms | 3.224 ms | 0.177 ms |    7.5 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **DpGroup**    | **cjk**      |  **19.25 μs** | **0.360 μs** | **0.020 μs** |         **-** |
| MyersGroup | cjk      | 168.60 μs | 0.566 μs | 0.031 μs |         - |
| **DpGroup**    | **latin**    |  **10.55 μs** | **0.057 μs** | **0.003 μs** |         **-** |
| MyersGroup | latin    | 112.07 μs | 0.729 μs | 0.040 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method | Classes | Mean         | Error       | StdDev    | Gen0   | Gen1   | Allocated |
|------- |-------- |-------------:|------------:|----------:|-------:|-------:|----------:|
| **Report** | **10**      |     **523.0 ns** |    **171.7 ns** |   **9.41 ns** | **0.0935** |      **-** |   **1.54 KB** |
| **Report** | **1000**    | **601,144.9 ns** | **11,918.6 ns** | **653.30 ns** | **6.8359** | **0.9766** | **117.56 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method          | Samples | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| Roc             | 1000000 | 151.4 ms | 14.01 ms | 0.77 ms |  750.0000 |  750.0000 |  750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 136.4 ms | 13.94 ms | 0.76 ms | 1000.0000 | 1000.0000 | 1000.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClusteringAgreementBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                         | Samples | Clusters | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------------------------- |-------- |--------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **AdjustedRandScore**              | **100000**  | **10**       |   **2.385 ms** | **0.1349 ms** | **0.0074 ms** |        **-** |        **-** |        **-** |   **12.07 KB** |
| MutualInformationScore         | 100000  | 10       |   2.378 ms | 0.1043 ms | 0.0057 ms |        - |        - |        - |   12.07 KB |
| AdjustedMutualInformationScore | 100000  | 10       |  22.159 ms | 0.0265 ms | 0.0015 ms | 218.7500 | 218.7500 | 218.7500 | 1031.09 KB |
| **AdjustedRandScore**              | **100000**  | **100**      |   **3.159 ms** | **0.1865 ms** | **0.0102 ms** | **210.9375** | **199.2188** | **199.2188** |  **937.61 KB** |
| MutualInformationScore         | 100000  | 100      |   3.329 ms | 0.0938 ms | 0.0051 ms | 210.9375 | 199.2188 | 199.2188 |  937.61 KB |
| AdjustedMutualInformationScore | 100000  | 100      | 193.245 ms | 1.1400 ms | 0.0625 ms | 333.3333 | 333.3333 | 333.3333 | 1745.05 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DamerauLevenshteinBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method   | Length | Mean        | Error       | StdDev    | Gen0   | Allocated |
|--------- |------- |------------:|------------:|----------:|-------:|----------:|
| **Distance** | **12**     |    **935.0 ns** |    **16.33 ns** |   **0.90 ns** | **0.0496** |     **840 B** |
| **Distance** | **120**    | **64,481.5 ns** | **4,055.17 ns** | **222.28 ns** | **0.1221** |    **2128 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanDimensionBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method       | Shape     | Mean       | Error     | StdDev  | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|------------- |---------- |-----------:|----------:|--------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| **Lodestar_Fit** | **10000x8x8** |   **385.7 ms** |   **7.55 ms** | **0.41 ms** |  **1.00** |    **0.00** |  **2000.0000** | **2000.0000** | **2000.0000** |  **28.69 MB** |        **1.00** |
| NumFlat_Fit  | 10000x8x8 | 2,613.3 ms | 114.77 ms | 6.29 ms |  6.78 |    0.02 | 14000.0000 | 8000.0000 | 7000.0000 | 156.32 MB |        5.45 |
|              |           |            |           |         |       |         |            |           |           |           |             |
| **Lodestar_Fit** | **5000x16x8** |   **132.1 ms** |  **12.94 ms** | **0.71 ms** |  **1.00** |    **0.01** |  **1250.0000** | **1250.0000** | **1250.0000** |  **13.29 MB** |        **1.00** |
| NumFlat_Fit  | 5000x16x8 |   900.5 ms |  14.99 ms | 0.82 ms |  6.82 |    0.03 |  5000.0000 | 3000.0000 | 3000.0000 |  59.77 MB |        4.50 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DbscanIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Lodestar_Fit**             | **20000x2x10** | **1,012.28 ms** |  **24.082 ms** |  **1.320 ms** |  **1.00** |    **0.00** |  **2000.0000** |  **2000.0000** |  **2000.0000** | **112.13 MB** |        **1.00** |
| NumFlat_Fit              | 20000x2x10 | 7,188.75 ms | 619.840 ms | 33.976 ms |  7.10 |    0.03 | 38000.0000 | 14000.0000 | 11000.0000 | 531.86 MB |        4.74 |
| Dbscan_CalculateClusters | 20000x2x10 | 4,007.21 ms | 291.125 ms | 15.958 ms |  3.96 |    0.01 | 22000.0000 | 12000.0000 | 10000.0000 | 372.14 MB |        3.32 |
|                          |            |             |            |           |       |         |            |            |            |           |             |
| **Lodestar_Fit**             | **5000x2x5**   |    **84.52 ms** |   **1.115 ms** |  **0.061 ms** |  **1.00** |    **0.00** |  **1166.6667** |  **1166.6667** |  **1166.6667** |  **14.02 MB** |        **1.00** |
| NumFlat_Fit              | 5000x2x5   |   535.40 ms |  16.696 ms |  0.915 ms |  6.33 |    0.01 |  4000.0000 |  3000.0000 |  2000.0000 |  66.59 MB |        4.75 |
| Dbscan_CalculateClusters | 5000x2x5   |   267.21 ms |  14.202 ms |  0.778 ms |  3.16 |    0.01 |  3500.0000 |  3500.0000 |  3500.0000 |   40.4 MB |        2.88 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                                    | Mean      | Error     | StdDev   | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       |  20.33 ms |  3.229 ms | 0.177 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 109.11 ms | 16.548 ms | 0.907 ms |  5.37 |    0.06 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  26.45 ms |  1.489 ms | 0.082 ms |  1.30 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                     | Columns | Mean         | Error        | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|-------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **39,242.9 μs** |  **9,096.95 μs** |   **498.63 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **12151.28 KB** |
| TruncatedSvd_Transform     | 500     |     576.8 μs |     19.28 μs |     1.06 μs |   94.7266 |   90.8203 |   90.8203 |    390.73 KB |
| Nmf_Frobenius_Rank20       | 500     | 213,183.9 μs | 38,712.75 μs | 2,121.98 μs | 4000.0000 | 4000.0000 | 4000.0000 |   18141.1 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 270,352.9 μs | 37,870.26 μs | 2,075.80 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.11 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **221,915.8 μs** | **12,421.14 μs** |   **680.84 μs** | **2666.6667** | **2666.6667** | **2666.6667** | **105766.49 KB** |
| TruncatedSvd_Transform     | 20000   |   1,555.0 μs |    444.60 μs |    24.37 μs |  496.0938 |  496.0938 |  496.0938 |   3437.86 KB |
| Nmf_Frobenius_Rank20       | 20000   | 787,174.7 μs | 38,520.97 μs | 2,111.46 μs | 5000.0000 | 5000.0000 | 5000.0000 | 156850.65 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 686,807.6 μs | 21,504.76 μs | 1,178.75 μs | 5000.0000 | 5000.0000 | 5000.0000 | 157305.66 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DoubleMetaphoneBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method | Mean     | Error   | StdDev  | Gen0    | Allocated |
|------- |---------:|--------:|--------:|--------:|----------:|
| Encode | 246.8 μs | 5.67 μs | 0.31 μs | 29.7852 |  488.6 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **SearchTop10** | **10000**  |   **565.1 μs** |  **19.92 μs** |  **1.09 μs** |   **1.63 KB** |
| **SearchTop10** | **100000** | **5,968.2 μs** | **301.50 μs** | **16.53 μs** |   **1.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method              | Rows   | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated    |
|-------------------- |------- |------------:|------------:|----------:|---------:|---------:|---------:|-------------:|
| **FromBlockNormalized** | **1000**   |    **741.6 μs** |    **79.29 μs** |   **4.35 μs** | **249.0234** | **249.0234** | **249.0234** |   **1500.18 KB** |
| MmrSelect100        | 1000   |  7,300.3 μs |   231.25 μs |  12.68 μs |        - |        - |        - |     21.02 KB |
| **FromBlockNormalized** | **100000** | **65,498.4 μs** | **5,363.27 μs** | **293.98 μs** | **250.0000** | **250.0000** | **250.0000** | **150000.27 KB** |
| MmrSelect100        | 100000 |  7,232.3 μs |   131.75 μs |   7.22 μs |        - |        - |        - |     21.02 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EncoderIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  Job-KHTMZC : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                          | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Median       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_OneHot**                 | **Job-KHTMZC** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **195.63 μs** |    **13.925 μs** |    **13.676 μs** |    **192.63 μs** |  **1.00** |    **0.09** |        **-** |        **-** |        **-** |  **166.08 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 1000     |    193.83 μs |    13.361 μs |    13.122 μs |    195.55 μs |  1.00 |    0.09 |        - |        - |        - |   17.53 KB |        0.11 |
| Lodestar_Impute                 | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 1000     |    196.77 μs |    30.400 μs |    35.009 μs |    204.27 μs |  1.01 |    0.19 |        - |        - |        - |   92.97 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 1000     |    343.59 μs |    13.958 μs |    14.935 μs |    344.53 μs |  1.76 |    0.13 |        - |        - |        - |   33.86 KB |        0.20 |
| MlNet_OneHotEncoding_Read       | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,462.49 μs |    72.139 μs |    80.182 μs |  1,473.04 μs |  7.51 |    0.62 |        - |        - |        - |  315.34 KB |        1.90 |
| MlNet_ReplaceMissingValues_Read | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 1000     |  1,143.67 μs |    70.307 μs |    78.146 μs |  1,156.85 μs |  5.87 |    0.54 |        - |        - |        - |  286.84 KB |        1.73 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    154.90 μs |     7.635 μs |     0.419 μs |    154.67 μs |  1.00 |    0.00 |  49.8047 |  49.8047 |  49.8047 |  165.37 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     66.54 μs |     7.575 μs |     0.415 μs |     66.38 μs |  0.43 |    0.00 |   0.9766 |        - |        - |   16.81 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     23.19 μs |     6.184 μs |     0.339 μs |     23.37 μs |  0.15 |    0.00 |   5.6458 |   0.1526 |        - |   92.53 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    891.08 μs | 2,217.303 μs |   121.538 μs |    882.17 μs |  5.75 |    0.68 |  59.5703 |   7.8125 |        - |  986.36 KB |        5.96 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    930.84 μs | 1,053.255 μs |    57.732 μs |    912.38 μs |  6.01 |    0.32 |  31.2500 |   7.8125 |        - |  560.73 KB |        3.39 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    261.51 μs |    27.748 μs |     1.521 μs |    261.07 μs |  1.69 |    0.01 |  18.5547 |  18.0664 |   0.4883 |  292.71 KB |        1.77 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_OneHot**                 | **Job-KHTMZC** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **4,132.91 μs** |   **258.151 μs** |   **265.102 μs** |  **4,267.82 μs** |  **1.00** |    **0.09** |        **-** |        **-** |        **-** | **3283.27 KB** |        **1.00** |
| Lodestar_Ordinal                | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,732.43 μs |   193.667 μs |   190.207 μs |  3,801.27 μs |  0.91 |    0.07 |        - |        - |        - |  314.41 KB |        0.10 |
| Lodestar_Impute                 | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,406.37 μs |    18.707 μs |    20.793 μs |  1,404.68 μs |  0.34 |    0.02 |        - |        - |        - | 1844.05 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 20000    |  3,550.83 μs |    41.135 μs |    45.722 μs |  3,549.09 μs |  0.86 |    0.06 |        - |        - |        - |   33.91 KB |        0.01 |
| MlNet_OneHotEncoding_Read       | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 20000    | 15,529.58 μs | 1,238.525 μs | 1,216.397 μs | 15,702.71 μs |  3.77 |    0.38 |        - |        - |        - |  521.79 KB |        0.16 |
| MlNet_ReplaceMissingValues_Read | Job-KHTMZC | 1               | 20             | Throughput  | 1            | 5           | 20000    |  9,460.59 μs | 3,944.474 μs | 4,220.543 μs | 12,041.65 μs |  2.30 |    1.01 |        - |        - |        - |  453.31 KB |        0.14 |
|                                 |            |                 |                |             |              |             |          |              |              |              |              |       |         |          |          |          |            |             |
| Lodestar_OneHot                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,505.10 μs |    92.043 μs |     5.045 μs |  3,504.46 μs |  1.00 |    0.00 | 992.1875 | 992.1875 | 992.1875 | 3290.87 KB |        1.00 |
| Lodestar_Ordinal                | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  3,212.50 μs |   267.229 μs |    14.648 μs |  3,209.27 μs |  0.92 |    0.00 |  89.8438 |  89.8438 |  89.8438 |  314.36 KB |        0.10 |
| Lodestar_Impute                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |    729.67 μs |   142.021 μs |     7.785 μs |    729.25 μs |  0.21 |    0.00 | 324.2188 | 324.2188 | 324.2188 | 1846.01 KB |        0.56 |
| MlNet_OneHotEncoding_Fit        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,462.73 μs | 1,229.635 μs |    67.400 μs |  1,465.30 μs |  0.42 |    0.02 |  29.2969 |   5.8594 |        - |  510.02 KB |        0.15 |
| MlNet_OneHotEncoding_Read       | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,211.77 μs | 1,509.166 μs |    82.722 μs |  4,186.80 μs |  1.20 |    0.02 |  31.2500 |  15.6250 |        - |  563.94 KB |        0.17 |
| MlNet_ReplaceMissingValues_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  2,555.03 μs | 1,465.262 μs |    80.316 μs |  2,519.00 μs |  0.73 |    0.02 |  27.3438 |  11.7188 |        - |  451.03 KB |        0.14 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method          | Records | Mean        | Error       | StdDev   | Gen0     | Gen1     | Gen2    | Allocated   |
|---------------- |-------- |------------:|------------:|---------:|---------:|---------:|--------:|------------:|
| **FilteredTop10**   | **10000**   |    **503.7 μs** |    **69.57 μs** |  **3.81 μs** |        **-** |        **-** |       **-** |      **7.2 KB** |
| Unfiltered      | 10000   |    567.8 μs |    34.77 μs |  1.91 μs |        - |        - |       - |      2.8 KB |
| HybridSelective | 10000   |  2,778.2 μs |   253.57 μs | 13.90 μs | 140.6250 | 117.1875 | 54.6875 |  2577.94 KB |
| HybridBroad     | 10000   |  4,360.9 μs |   249.16 μs | 13.66 μs | 164.0625 | 117.1875 | 62.5000 |  3221.11 KB |
| **FilteredTop10**   | **100000**  | **10,871.4 μs** |   **906.32 μs** | **49.68 μs** |        **-** |        **-** |       **-** |     **7.21 KB** |
| Unfiltered      | 100000  |  6,026.5 μs |   367.74 μs | 20.16 μs |        - |        - |       - |      2.8 KB |
| HybridSelective | 100000  | 51,068.3 μs |   750.59 μs | 41.14 μs | 230.7692 | 153.8462 |       - |    23471 KB |
| HybridBroad     | 100000  | 53,141.3 μs | 1,358.30 μs | 74.45 μs | 222.2222 | 111.1111 |       - | 29359.82 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| Ratio          |   108.1 ns |   2.07 ns |  0.11 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   764.5 ns |   5.43 ns |  0.30 ns |  7.07 |    0.01 |      - |         - |          NA |
| TokenSortRatio |   851.1 ns | 112.80 ns |  6.18 ns |  7.87 |    0.05 | 0.0668 |    1120 B |          NA |
| TokenSetRatio  |   920.3 ns | 573.10 ns | 31.41 ns |  8.51 |    0.25 | 0.0534 |     896 B |          NA |
| WRatio         | 1,170.2 ns | 642.90 ns | 35.24 ns | 10.82 |    0.28 | 0.0668 |    1120 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzCodePointBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                    | Mean               | Error             | StdDev          | Ratio  | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|-------------------------- |-------------------:|------------------:|----------------:|-------:|--------:|--------:|--------:|----------:|------------:|
| Ratio_RepeatedEmoji       |  2,207,853.2559 ns |   117,355.6381 ns |   6,432.6586 ns |  1.000 |    0.00 |  7.8125 |       - |  160508 B |        1.00 |
| TokenSortRatio_EmojiWords |    654,449.5762 ns |    16,491.1379 ns |     903.9349 ns |  0.296 |    0.00 | 35.1563 | 12.6953 |  602665 B |        3.75 |
| WRatio_ProseAndOneEmoji   |  1,173,894.9010 ns |    94,723.6539 ns |   5,192.1231 ns |  0.532 |    0.00 | 42.9688 | 15.6250 |  720569 B |        4.49 |
| Ratio_DistinctAstral      | 78,059,489.5238 ns | 7,580,875.3714 ns | 415,533.3613 ns | 35.356 |    0.19 |       - |       - |  510776 B |        3.18 |
| WRatio_EmptyOperand       |          0.0242 ns |         0.0554 ns |       0.0030 ns |  0.000 |    0.00 |       - |       - |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method     | Operation     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **108.5 ns** |   **1.88 ns** |  **0.10 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    226.4 ns |   3.09 ns |  0.17 ns |  2.09 |    0.00 | 0.0048 |      80 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **778.6 ns** |  **11.65 ns** |  **0.64 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,154.1 ns | 352.35 ns | 19.31 ns | 13.04 |    0.02 |      - |     160 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |    **975.4 ns** | **417.34 ns** | **22.88 ns** |  **1.00** |    **0.03** | **0.0534** |     **896 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,029.1 ns |  91.46 ns |  5.01 ns |  2.08 |    0.04 | 0.1144 |    1944 B |        2.17 |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **1,149.2 ns** | **137.13 ns** |  **7.52 ns** |  **1.00** |    **0.01** | **0.0668** |    **1120 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,905.0 ns | 147.89 ns |  8.11 ns |  4.27 |    0.03 | 0.1831 |    3096 B |        2.76 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.557 ms** | **0.4008 ms** | **0.0220 ms** | **277.3438** | **267.5781** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **17.100 ms** | **0.4438 ms** | **0.0243 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                     | Length | Mean        | Error        | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |------------:|-------------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |    **29.18 ns** |     **1.026 ns** |  **0.056 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    39.72 ns |     1.513 ns |  0.083 ns |  1.36 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    31.10 ns |     0.406 ns |  0.022 ns |  1.07 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    29.70 ns |     0.416 ns |  0.023 ns |  1.02 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **12**     |    **32.57 ns** |     **0.620 ns** |  **0.034 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    41.66 ns |     0.317 ns |  0.017 ns |  1.28 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    32.91 ns |     0.145 ns |  0.008 ns |  1.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    31.61 ns |     0.344 ns |  0.019 ns |  0.97 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **16**     |    **33.48 ns** |     **0.342 ns** |  **0.019 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.39 ns |     1.004 ns |  0.055 ns |  1.36 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    34.44 ns |     6.223 ns |  0.341 ns |  1.03 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    37.30 ns |     0.448 ns |  0.025 ns |  1.11 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **20**     |    **41.96 ns** |     **0.973 ns** |  **0.053 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    48.40 ns |     0.950 ns |  0.052 ns |  1.15 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    39.17 ns |     1.157 ns |  0.063 ns |  0.93 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    34.87 ns |     2.598 ns |  0.142 ns |  0.83 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **24**     |    **61.41 ns** |     **0.652 ns** |  **0.036 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    67.85 ns |     2.434 ns |  0.133 ns |  1.10 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    62.15 ns |     2.357 ns |  0.129 ns |  1.01 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    57.39 ns |     4.177 ns |  0.229 ns |  0.93 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **32**     |    **68.87 ns** |     **1.823 ns** |  **0.100 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    76.44 ns |     0.639 ns |  0.035 ns |  1.11 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    75.48 ns |     0.743 ns |  0.041 ns |  1.10 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    66.07 ns |     1.557 ns |  0.085 ns |  0.96 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **128**    |   **405.31 ns** |     **3.701 ns** |  **0.203 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   421.53 ns |     3.532 ns |  0.194 ns |  1.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   406.19 ns |    33.688 ns |  1.847 ns |  1.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   589.57 ns |    24.901 ns |  1.365 ns |  1.45 |         - |          NA |
|                            |        |             |              |           |       |           |             |
| **Distance_Utf16**             | **512**    | **4,994.01 ns** |   **188.966 ns** | **10.358 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 5,069.14 ns |   776.485 ns | 42.562 ns |  1.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 5,111.99 ns | 1,550.734 ns | 85.001 ns |  1.02 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,983.56 ns |     5.579 ns |  0.306 ns |  1.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method             | Length | Mean        | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |------------:|----------:|---------:|------:|----------:|------------:|
| **Distance_CodePoint** | **20**     |    **408.3 ns** |   **5.21 ns** |  **0.29 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    200.1 ns |   4.66 ns |  0.26 ns |  0.49 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,443.3 ns** |  **31.09 ns** |  **1.70 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  5,112.9 ns |  54.25 ns |  2.97 ns |  2.09 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **14,462.4 ns** | **227.48 ns** | **12.47 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 38,296.4 ns | 620.31 ns | 34.00 ns |  2.65 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansFitIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method           | Shape       | Mean         | Error         | StdDev       | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------------- |------------ |-------------:|--------------:|-------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Lodestar_Fit**     | **10000x16x16** |   **5,832.6 μs** |     **659.91 μs** |     **36.17 μs** |  **1.00** |    **0.01** | **7.8125** |      **-** | **162.63 KB** |        **1.00** |
| NumFlat_Fit      | 10000x16x16 |  36,861.7 μs |   6,476.71 μs |    355.01 μs |  6.32 |    0.06 |      - |      - |  10.39 KB |        0.06 |
| MetaNumerics_Fit | 10000x16x16 |  85,696.4 μs |   6,122.67 μs |    335.60 μs | 14.69 |    0.09 |      - |      - |  41.95 KB |        0.26 |
|                  |             |              |               |              |       |         |        |        |           |             |
| **Lodestar_Fit**     | **10000x2x8**   |     **765.8 μs** |      **45.70 μs** |      **2.50 μs** |  **1.00** |    **0.00** | **8.7891** | **0.9766** | **156.73 KB** |        **1.00** |
| NumFlat_Fit      | 10000x2x8   |   6,140.6 μs |     132.84 μs |      7.28 μs |  8.02 |    0.02 |      - |      - |      3 KB |        0.02 |
| MetaNumerics_Fit | 10000x2x8   |   2,867.7 μs |     114.95 μs |      6.30 μs |  3.74 |    0.01 |      - |      - |  39.57 KB |        0.25 |
|                  |             |              |               |              |       |         |        |        |           |             |
| **Lodestar_Fit**     | **50000x8x32**  |  **29,927.7 μs** |  **16,547.98 μs** |    **907.05 μs** |  **1.00** |    **0.04** |      **-** |      **-** | **787.79 KB** |        **1.00** |
| NumFlat_Fit      | 50000x8x32  | 452,888.9 μs | 216,512.76 μs | 11,867.79 μs | 15.14 |    0.52 |      - |      - |  16.29 KB |        0.02 |
| MetaNumerics_Fit | 50000x8x32  | 708,652.4 μs | 155,836.29 μs |  8,541.91 μs | 23.69 |    0.67 |      - |      - | 199.91 KB |        0.25 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.KMeansLloydIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method         | Shape       | Mean       | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |------------ |-----------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Lodestar_Lloyd** | **10000x16x16** |   **7.656 ms** |  **0.4388 ms** | **0.0241 ms** |  **1.00** |    **0.00** |   **88.7 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x16x16 |  16.791 ms |  1.1380 ms | 0.0624 ms |  2.19 |    0.01 |   13.6 KB |        0.15 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **10000x2x8**   |  **28.103 ms** |  **6.3517 ms** | **0.3482 ms** |  **1.00** |    **0.02** |  **98.92 KB** |        **1.00** |
| NumFlat_Lloyd  | 10000x2x8   | 126.261 ms | 43.4245 ms | 2.3802 ms |  4.49 |    0.09 |  96.48 KB |        0.98 |
|                |             |            |            |           |       |         |           |             |
| **Lodestar_Lloyd** | **50000x8x32**  |  **75.561 ms** |  **2.9626 ms** | **0.1624 ms** |  **1.00** |    **0.00** | **410.27 KB** |        **1.00** |
| NumFlat_Lloyd  | 50000x8x32  | 228.719 ms | 60.7141 ms | 3.3279 ms |  3.03 |    0.04 |  36.47 KB |        0.09 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Dp**         | **8**    |    **130.74 ns** |    **15.481 ns** |   **0.849 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 8    |     59.69 ns |     0.667 ns |   0.037 ns |  0.46 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.44 ns |     1.371 ns |   0.075 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 8    |    108.61 ns |     2.911 ns |   0.160 ns |  0.83 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **220.34 ns** |     **4.501 ns** |   **0.247 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     68.48 ns |     3.660 ns |   0.201 ns |  0.31 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    228.35 ns |   252.165 ns |  13.822 ns |  1.04 |    0.05 |         - |          NA |
| Kernel_Cjk | 12   |    121.95 ns |    19.477 ns |   1.068 ns |  0.55 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **279.94 ns** |    **11.153 ns** |   **0.611 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 14   |     73.63 ns |     1.294 ns |   0.071 ns |  0.26 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    314.63 ns |    83.670 ns |   4.586 ns |  1.12 |    0.01 |         - |          NA |
| Kernel_Cjk | 14   |    124.98 ns |     7.675 ns |   0.421 ns |  0.45 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **383.12 ns** |   **435.908 ns** |  **23.894 ns** |  **1.00** |    **0.08** |         **-** |          **NA** |
| Kernel     | 16   |     77.49 ns |     0.553 ns |   0.030 ns |  0.20 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    381.20 ns |   417.804 ns |  22.901 ns |  1.00 |    0.08 |         - |          NA |
| Kernel_Cjk | 16   |    130.33 ns |    21.269 ns |   1.166 ns |  0.34 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **717.19 ns** |   **293.132 ns** |  **16.068 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 18   |     85.37 ns |     4.083 ns |   0.224 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    729.05 ns |   605.760 ns |  33.204 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |    134.87 ns |     0.667 ns |   0.037 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **819.97 ns** |    **47.617 ns** |   **2.610 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     87.43 ns |     0.529 ns |   0.029 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    817.14 ns |    63.265 ns |   3.468 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    141.34 ns |     0.989 ns |   0.054 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |  **1,003.02 ns** |   **354.314 ns** |  **19.421 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 24   |     97.89 ns |     3.293 ns |   0.181 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    998.23 ns |    74.936 ns |   4.108 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 24   |    154.00 ns |     2.263 ns |   0.124 ns |  0.15 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,600.14 ns** |   **887.147 ns** |  **48.628 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 32   |    119.77 ns |     2.770 ns |   0.152 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,633.75 ns |   831.488 ns |  45.577 ns |  1.02 |    0.04 |         - |          NA |
| Kernel_Cjk | 32   |    181.51 ns |    11.164 ns |   0.612 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,216.05 ns** | **2,213.360 ns** | **121.322 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 48   |    144.42 ns |    12.362 ns |   0.678 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,274.91 ns |   430.154 ns |  23.578 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    259.48 ns |     0.271 ns |   0.015 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,415.48 ns** |   **317.140 ns** |  **17.384 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    172.36 ns |     3.087 ns |   0.169 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,208.29 ns | 2,945.726 ns | 161.465 ns |  0.96 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    325.91 ns |    18.199 ns |   0.998 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,384.87 ns** | **4,244.328 ns** | **232.646 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 96   |    363.26 ns |     7.208 ns |   0.395 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,397.37 ns | 3,946.829 ns | 216.339 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |    955.77 ns |    21.954 ns |   1.203 ns |  0.08 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **26.95 ns** |     **0.269 ns** |   **0.015 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     28.05 ns |     1.239 ns |   0.068 ns |  1.04 |         - |          NA |
| Distance_CodePoint         | 8      |    126.17 ns |     2.536 ns |   0.139 ns |  4.68 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     28.11 ns |     0.469 ns |   0.026 ns |  1.04 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **64**     |    **313.56 ns** |     **5.122 ns** |   **0.281 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    402.73 ns |     6.930 ns |   0.380 ns |  1.28 |         - |          NA |
| Distance_CodePoint         | 64     |    719.20 ns |    58.597 ns |   3.212 ns |  2.29 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    313.20 ns |     0.889 ns |   0.049 ns |  1.00 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **128**    |  **1,047.68 ns** |    **10.269 ns** |   **0.563 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  1,949.13 ns |    56.248 ns |   3.083 ns |  1.86 |         - |          NA |
| Distance_CodePoint         | 128    |  1,834.16 ns |    13.859 ns |   0.760 ns |  1.75 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |  1,001.68 ns |   109.938 ns |   6.026 ns |  0.96 |         - |          NA |
|                            |        |              |              |            |       |           |             |
| **Distance_Utf16**             | **512**    | **12,389.88 ns** |    **76.863 ns** |   **4.213 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 18,184.21 ns |   684.378 ns |  37.513 ns |  1.47 |         - |          NA |
| Distance_CodePoint         | 512    | 14,887.95 ns |   226.357 ns |  12.407 ns |  1.20 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 12,524.53 ns | 2,086.534 ns | 114.370 ns |  1.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Distance_CodePoint** | **16**     | **32**       |     **353.4 ns** |      **5.45 ns** |     **0.30 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     255.9 ns |      5.86 ns |     0.32 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **356.1 ns** |     **17.26 ns** |     **0.95 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     256.0 ns |      1.11 ns |     0.06 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **447.8 ns** |      **2.62 ns** |     **0.14 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     349.6 ns |      6.16 ns |     0.34 ns |  0.78 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **449.5 ns** |      **5.04 ns** |     **0.28 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     347.7 ns |      7.33 ns |     0.40 ns |  0.77 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **552.6 ns** |    **452.98 ns** |    **24.83 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     446.1 ns |      6.25 ns |     0.34 ns |  0.81 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **546.9 ns** |    **108.32 ns** |     **5.94 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     443.8 ns |      8.99 ns |     0.49 ns |  0.81 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **651.1 ns** |      **6.50 ns** |     **0.36 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,365.0 ns |    959.35 ns |    52.59 ns |  2.10 |    0.07 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **647.7 ns** |    **206.80 ns** |    **11.34 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,314.1 ns |    137.91 ns |     7.56 ns |  2.03 |    0.03 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,033.8 ns** |    **247.96 ns** |    **13.59 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,698.2 ns |    121.29 ns |     6.65 ns |  2.80 |    0.02 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,040.4 ns** |    **113.92 ns** |     **6.24 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,711.7 ns |    333.94 ns |    18.30 ns |  2.80 |    0.01 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **16,428.2 ns** |    **186.06 ns** |    **10.20 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  65,637.8 ns |  1,386.09 ns |    75.98 ns |  4.00 |    0.00 |         - |          NA |
|                    |        |          |              |              |             |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **434,428.8 ns** | **37,658.68 ns** | **2,064.20 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  66,661.6 ns |  2,503.06 ns |   137.20 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Lodestar**             | **8**      |      **27.87 ns** |      **0.163 ns** |     **0.009 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      87.26 ns |      5.842 ns |     0.320 ns |  3.13 |    0.01 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.25 ns |      0.495 ns |     0.027 ns |  2.92 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     174.06 ns |     70.206 ns |     3.848 ns |  6.25 |    0.12 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **311.64 ns** |      **3.387 ns** |     **0.186 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,831.04 ns |     99.940 ns |     5.478 ns | 18.71 |    0.02 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,292.09 ns |    181.288 ns |     9.937 ns |  4.15 |    0.03 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  10,000.31 ns |  2,691.663 ns |   147.539 ns | 32.09 |    0.41 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **12,382.00 ns** |    **213.424 ns** |    **11.698 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 456,836.75 ns | 65,155.268 ns | 3,571.380 ns | 36.90 |    0.25 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  35,904.52 ns |    418.240 ns |    22.925 ns |  2.90 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 741,474.27 ns | 35,331.576 ns | 1,936.643 ns | 59.88 |    0.14 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetaNumericsPcaBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                          | Shape   | Mean          | Error          | StdDev       | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|-------------------------------- |-------- |--------------:|---------------:|-------------:|-------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_ExplainedVarianceRatio** | **2000x10** |     **148.37 μs** |      **10.524 μs** |     **0.577 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x10 | 103,986.04 μs |  21,669.396 μs | 1,187.773 μs | 700.85 |    7.32 | 800.0000 | 800.0000 | 800.0000 | 31414.56 KB |   13,227.18 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **2000x50** |   **3,185.22 μs** |      **18.758 μs** |     **1.028 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |    **42.07 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 2000x50 | 497,237.91 μs | 113,039.576 μs | 6,196.081 μs | 156.11 |    1.69 |        - |        - |        - | 32054.48 KB |      762.01 |
|                                 |         |               |                |              |        |         |          |          |          |             |             |
| **Lodestar_ExplainedVarianceRatio** | **200x10**  |      **24.62 μs** |       **1.776 μs** |     **0.097 μs** |   **1.00** |    **0.00** |   **0.1221** |        **-** |        **-** |     **2.38 KB** |        **1.00** |
| MetaNumerics_VarianceFraction   | 200x10  |     928.54 μs |      16.917 μs |     0.927 μs |  37.71 |    0.13 |  99.6094 |  99.6094 |  99.6094 |   329.71 KB |      138.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method         | Samples | Classes | Mean           | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |---------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **4,158.0 ns** |       **495.81 ns** |     **27.18 ns** | **0.0153** |     **376 B** |
| MatrixWeighted | 1000    | 2       |     7,016.8 ns |       980.90 ns |     53.77 ns | 0.0153 |     376 B |
| AccuracyScore  | 1000    | 2       |       112.7 ns |        19.75 ns |      1.08 ns |      - |         - |
| F1Macro        | 1000    | 2       |     4,198.9 ns |       255.94 ns |     14.03 ns | 0.0305 |     536 B |
| Report         | 1000    | 2       |     6,517.5 ns |       221.27 ns |     12.13 ns | 0.3128 |    5344 B |
| **Matrix**         | **1000**    | **10**      |     **4,141.3 ns** |     **1,251.61 ns** |     **68.60 ns** | **0.0763** |    **1312 B** |
| MatrixWeighted | 1000    | 10      |     7,257.7 ns |       326.47 ns |     17.89 ns | 0.0763 |    1312 B |
| AccuracyScore  | 1000    | 10      |       115.6 ns |        18.02 ns |      0.99 ns |      - |         - |
| F1Macro        | 1000    | 10      |     4,598.2 ns |       322.78 ns |     17.69 ns | 0.0992 |    1728 B |
| Report         | 1000    | 10      |    10,107.2 ns |       148.12 ns |      8.12 ns | 0.7324 |   12336 B |
| **Matrix**         | **100000**  | **2**       |   **445,199.0 ns** |     **1,926.31 ns** |    **105.59 ns** |      **-** |     **376 B** |
| MatrixWeighted | 100000  | 2       |   845,479.2 ns |   402,390.06 ns | 22,056.36 ns |      - |     377 B |
| AccuracyScore  | 100000  | 2       |    12,287.1 ns |     1,529.64 ns |     83.84 ns |      - |         - |
| F1Macro        | 100000  | 2       |   405,441.6 ns |     4,071.54 ns |    223.17 ns |      - |     536 B |
| Report         | 100000  | 2       |   408,541.2 ns |    17,329.55 ns |    949.89 ns |      - |    5368 B |
| **Matrix**         | **100000**  | **10**      |   **398,994.5 ns** |    **16,236.84 ns** |    **890.00 ns** |      **-** |    **1312 B** |
| MatrixWeighted | 100000  | 10      |   969,750.1 ns |    45,316.26 ns |  2,483.94 ns |      - |    1313 B |
| AccuracyScore  | 100000  | 10      |    11,969.9 ns |        88.37 ns |      4.84 ns |      - |         - |
| F1Macro        | 100000  | 10      |   397,852.2 ns |    19,914.64 ns |  1,091.59 ns |      - |    1728 B |
| Report         | 100000  | 10      |   405,318.1 ns |    25,212.20 ns |  1,381.97 ns | 0.4883 |   12680 B |
| **Matrix**         | **1000000** | **2**       | **4,220,882.9 ns** |    **35,419.53 ns** |  **1,941.46 ns** |      **-** |     **382 B** |
| MatrixWeighted | 1000000 | 2       | 8,589,498.9 ns |   227,780.50 ns | 12,485.42 ns |      - |     383 B |
| AccuracyScore  | 1000000 | 2       |   121,461.7 ns |    12,573.52 ns |    689.20 ns |      - |         - |
| F1Macro        | 1000000 | 2       | 4,049,694.2 ns |    35,101.36 ns |  1,924.02 ns |      - |     542 B |
| Report         | 1000000 | 2       | 4,068,654.5 ns |    48,155.00 ns |  2,639.54 ns |      - |    5390 B |
| **Matrix**         | **1000000** | **10**      | **3,894,235.1 ns** |   **367,197.76 ns** | **20,127.35 ns** |      **-** |    **1318 B** |
| MatrixWeighted | 1000000 | 10      | 9,818,031.1 ns | 1,423,664.88 ns | 78,035.88 ns |      - |    1324 B |
| AccuracyScore  | 1000000 | 10      |   121,629.1 ns |     7,449.00 ns |    408.30 ns |      - |         - |
| F1Macro        | 1000000 | 10      | 3,797,429.1 ns |   380,333.02 ns | 20,847.34 ns |      - |    1731 B |
| Report         | 1000000 | 10      | 3,767,488.4 ns |   120,564.48 ns |  6,608.55 ns |      - |   12723 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Lodestar** | **100000**  | **Bundle**        |   **7,004.92 μs** |    **253.259 μs** |    **13.882 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1064 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  38,924.82 μs |  1,535.897 μs |    84.188 μs |     5.56 |    0.01 | 538.4615 | 538.4615 | 538.4615 |  5089463 B |    4,783.33 |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |      **11.14 μs** |      **2.089 μs** |     **0.115 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  39,081.05 μs |  2,613.169 μs |   143.237 μs | 3,507.48 |   33.32 | 538.4615 | 538.4615 | 538.4615 |  5088846 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **111,336.73 μs** | **21,362.405 μs** | **1,170.945 μs** |     **1.00** |    **0.01** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 254,282.95 μs | 21,228.375 μs | 1,163.599 μs |     2.28 |    0.02 |        - |        - |        - | 23231816 B |          NA |
|          |         |               |               |               |              |          |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |     **122.48 μs** |      **2.305 μs** |     **0.126 μs** |     **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 255,460.24 μs | 10,612.951 μs |   581.732 μs | 2,085.65 |    4.51 |        - |        - |        - | 23231816 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method    | Samples | Mean        | Error      | StdDev   | Allocated |
|---------- |-------- |------------:|-----------:|---------:|----------:|
| **OneVsRest** | **100000**  |    **32.45 ms** |   **0.624 ms** | **0.034 ms** |     **538 B** |
| OneVsOne  | 100000  |    82.80 ms |   2.146 ms | 0.118 ms |  801865 B |
| **OneVsRest** | **1000000** |   **482.30 ms** | **169.645 ms** | **9.299 ms** |         **-** |
| OneVsOne  | 1000000 | 1,082.33 ms |  70.443 ms | 3.861 ms | 8003648 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|----------:|----------:|---------:|------------:|
| PerLabel         | 100000 |  7.786 ms |  0.2304 ms | 0.0126 ms |         - |         - |        - |     6.29 KB |
| PerLabelWeighted | 100000 | 12.308 ms |  1.0603 ms | 0.0581 ms |         - |         - |        - |     6.29 KB |
| PerSample        | 100000 | 58.172 ms | 25.6872 ms | 1.4080 ms | 2555.5556 | 2444.4444 | 777.7778 | 31250.62 KB |
| PerClass         | 100000 |  5.093 ms |  1.7552 ms | 0.0962 ms |         - |         - |        - |     6.43 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method     | Band | Mean         | Error      | StdDev    | Ratio | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-----------:|----------:|------:|----------:|------------:|
| **Dp**         | **4**    |     **83.36 ns** |   **0.942 ns** |  **0.052 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 4    |     80.39 ns |   1.106 ns |  0.061 ns |  0.96 |         - |          NA |
| Dp_Cjk     | 4    |     82.60 ns |   1.327 ns |  0.073 ns |  0.99 |         - |          NA |
| Kernel_Cjk | 4    |     79.89 ns |   1.251 ns |  0.069 ns |  0.96 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **6**    |    **116.76 ns** |   **2.391 ns** |  **0.131 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 6    |     86.28 ns |   0.509 ns |  0.028 ns |  0.74 |         - |          NA |
| Dp_Cjk     | 6    |    117.09 ns |  14.593 ns |  0.800 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 6    |    150.31 ns |  14.027 ns |  0.769 ns |  1.29 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **8**    |    **156.69 ns** |   **1.777 ns** |  **0.097 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 8    |    100.11 ns |   0.666 ns |  0.037 ns |  0.64 |         - |          NA |
| Dp_Cjk     | 8    |    156.06 ns |   0.862 ns |  0.047 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 8    |    186.03 ns |   8.490 ns |  0.465 ns |  1.19 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **10**   |    **207.28 ns** |   **6.652 ns** |  **0.365 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 10   |    109.77 ns |   0.154 ns |  0.008 ns |  0.53 |         - |          NA |
| Dp_Cjk     | 10   |    207.94 ns |   7.552 ns |  0.414 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 10   |    168.09 ns |   1.373 ns |  0.075 ns |  0.81 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **12**   |    **268.56 ns** |   **3.910 ns** |  **0.214 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 12   |    117.53 ns |   7.918 ns |  0.434 ns |  0.44 |         - |          NA |
| Dp_Cjk     | 12   |    268.61 ns |  15.230 ns |  0.835 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 12   |    182.91 ns |  12.195 ns |  0.668 ns |  0.68 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **16**   |    **430.37 ns** |  **17.812 ns** |  **0.976 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 16   |    137.51 ns |   1.942 ns |  0.106 ns |  0.32 |         - |          NA |
| Dp_Cjk     | 16   |    438.33 ns |  11.167 ns |  0.612 ns |  1.02 |         - |          NA |
| Kernel_Cjk | 16   |    200.02 ns |  17.985 ns |  0.986 ns |  0.46 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **24**   |    **863.77 ns** |  **21.795 ns** |  **1.195 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 24   |    194.29 ns | 109.631 ns |  6.009 ns |  0.22 |         - |          NA |
| Dp_Cjk     | 24   |    862.02 ns |   2.998 ns |  0.164 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 24   |    244.72 ns |   3.915 ns |  0.215 ns |  0.28 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **32**   |  **1,485.76 ns** |  **59.090 ns** |  **3.239 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 32   |    219.95 ns |   0.318 ns |  0.017 ns |  0.15 |         - |          NA |
| Dp_Cjk     | 32   |  1,485.30 ns |  21.647 ns |  1.187 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 32   |    299.09 ns |  11.353 ns |  0.622 ns |  0.20 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **48**   |  **3,249.88 ns** | **124.517 ns** |  **6.825 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 48   |    310.96 ns |  13.203 ns |  0.724 ns |  0.10 |         - |          NA |
| Dp_Cjk     | 48   |  3,244.74 ns |  67.981 ns |  3.726 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 48   |    392.25 ns |   5.208 ns |  0.285 ns |  0.12 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **64**   |  **5,648.72 ns** | **126.354 ns** |  **6.926 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 64   |    384.69 ns |   1.721 ns |  0.094 ns |  0.07 |         - |          NA |
| Dp_Cjk     | 64   |  5,655.63 ns |  95.027 ns |  5.209 ns |  1.00 |         - |          NA |
| Kernel_Cjk | 64   |    485.83 ns |  35.374 ns |  1.939 ns |  0.09 |         - |          NA |
|            |      |              |            |           |       |           |             |
| **Dp**         | **96**   | **12,553.40 ns** |  **40.809 ns** |  **2.237 ns** |  **1.00** |         **-** |          **NA** |
| Kernel     | 96   |    807.51 ns |   3.067 ns |  0.168 ns |  0.06 |         - |          NA |
| Dp_Cjk     | 96   | 12,877.84 ns | 271.934 ns | 14.906 ns |  1.03 |         - |          NA |
| Kernel_Cjk | 96   |  1,549.75 ns |  10.197 ns |  0.559 ns |  0.12 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.OsaBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method             | Length | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**     | **8**      |     **43.17 ns** |      **3.068 ns** |   **0.168 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 8      |    129.76 ns |      5.881 ns |   0.322 ns |  3.01 |    0.01 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **32**     |    **159.98 ns** |      **1.293 ns** |   **0.071 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 32     |  1,506.09 ns |    188.343 ns |  10.324 ns |  9.41 |    0.06 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **64**     |    **355.71 ns** |      **0.880 ns** |   **0.048 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint | 64     |  7,326.62 ns |    226.411 ns |  12.410 ns | 20.60 |    0.03 |         - |          NA |
|                    |        |              |               |            |       |         |           |             |
| **Distance_Utf16**     | **128**    | **39,645.13 ns** | **10,307.365 ns** | **564.981 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint | 128    | 37,139.54 ns |  3,177.625 ns | 174.176 ns |  0.94 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialFitBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                | RowCount | BatchCount | Mean        | Error      | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------------- |--------- |----------- |------------:|-----------:|----------:|------:|-------:|----------:|------------:|
| **WholeFit**              | **10000**    | **10**         |   **386.65 μs** |  **24.129 μs** |  **1.323 μs** |  **1.00** |      **-** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 10         |   456.68 μs |  30.123 μs |  1.651 μs |  1.18 |      - |    3680 B |        4.55 |
| SparseFit             | 10000    | 10         |    68.50 μs |  32.298 μs |  1.770 μs |  0.18 |      - |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 10         |   537.67 μs |  17.424 μs |  0.955 μs |  1.39 |      - |     785 B |        0.97 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **10000**    | **100**        |   **385.20 μs** |   **8.497 μs** |  **0.466 μs** |  **1.00** |      **-** |     **808 B** |        **1.00** |
| BatchedFit            | 10000    | 100        |   668.57 μs |  33.952 μs |  1.861 μs |  1.74 | 1.9531 |   36801 B |       45.55 |
| SparseFit             | 10000    | 100        |    72.91 μs |  13.408 μs |  0.735 μs |  0.19 |      - |     704 B |        0.87 |
| DenseFitOfTheSameData | 10000    | 100        |   546.16 μs |  42.602 μs |  2.335 μs |  1.42 |      - |     785 B |        0.97 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **10**         | **3,846.70 μs** | **693.486 μs** | **38.012 μs** |  **1.00** |      **-** |     **811 B** |        **1.00** |
| BatchedFit            | 100000   | 10         | 4,694.72 μs | 107.589 μs |  5.897 μs |  1.22 |      - |    3686 B |        4.55 |
| SparseFit             | 100000   | 10         | 1,882.05 μs | 116.937 μs |  6.410 μs |  0.49 |      - |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 10         | 5,311.21 μs |  86.090 μs |  4.719 μs |  1.38 |      - |     790 B |        0.97 |
|                       |          |            |             |            |           |       |        |           |             |
| **WholeFit**              | **100000**   | **100**        | **3,819.98 μs** | **109.295 μs** |  **5.991 μs** |  **1.00** |      **-** |     **814 B** |        **1.00** |
| BatchedFit            | 100000   | 100        | 5,729.46 μs | 459.265 μs | 25.174 μs |  1.50 |      - |   36806 B |       45.22 |
| SparseFit             | 100000   | 100        | 1,890.61 μs |  87.756 μs |  4.810 μs |  0.49 |      - |     705 B |        0.87 |
| DenseFitOfTheSameData | 100000   | 100        | 5,332.55 μs | 617.726 μs | 33.860 μs |  1.40 |      - |     790 B |        0.97 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method      | NeedleLength | Mean         | Error      | StdDev    | Allocated |
|------------ |------------- |-------------:|-----------:|----------:|----------:|
| **Embedded**    | **65**           |     **9.914 μs** |  **0.1401 μs** | **0.0077 μs** |         **-** |
| EqualLength | 65           |     2.917 μs |  0.0470 μs | 0.0026 μs |         - |
| **Embedded**    | **128**          |    **36.091 μs** |  **0.6866 μs** | **0.0376 μs** |         **-** |
| EqualLength | 128          |     5.462 μs |  0.2320 μs | 0.0127 μs |         - |
| **Embedded**    | **512**          | **1,432.561 μs** | **11.1051 μs** | **0.6087 μs** |         **-** |
| EqualLength | 512          |    48.098 μs |  6.9588 μs | 0.3814 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                | Samples | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 | 10.57 ms | 0.817 ms | 0.045 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 16.99 ms | 2.689 ms | 0.147 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| VocabTxt               |  4.090 ms |  3.3283 ms | 0.1824 ms | 109.3750 | 101.5625 |  31.2500 |  3711.57 KB |
| TokenizerJsonWordPiece | 11.126 ms |  2.3143 ms | 0.1269 ms | 187.5000 | 171.8750 |  46.8750 |  5852.34 KB |
| TokenizerJsonUnigram   | 11.092 ms |  0.2028 ms | 0.0111 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |  3.984 ms |  2.1314 ms | 0.1168 ms | 109.3750 | 101.5625 |  31.2500 |  3440.09 KB |
| TfidfSave              |  1.687 ms |  0.2284 ms | 0.0125 ms |  21.4844 |  15.6250 |  15.6250 |  2137.05 KB |
| TfidfLoad              |  4.438 ms |  0.6034 ms | 0.0331 ms |  78.1250 |  62.5000 |  15.6250 |  2930.67 KB |
| EmbeddingIndexSave     |  3.771 ms |  1.0222 ms | 0.0560 ms | 199.2188 | 195.3125 | 195.3125 | 20349.83 KB |
| EmbeddingIndexLoad     |  4.420 ms |  0.3368 ms | 0.0185 ms | 179.6875 | 148.4375 | 117.1875 |  16094.4 KB |
| EmbeddingIndexSaveFile | 49.499 ms | 10.3816 ms | 0.5691 ms |        - |        - |        - |   323.49 KB |
| EmbeddingIndexLoadGzip | 74.647 ms |  0.8640 ms | 0.0474 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 14.66 ms | 0.376 ms | 0.021 ms | 171.8750 |   2.75 MB |
| EncodeDocuments    | 63.54 ms | 5.448 ms | 0.299 ms | 500.0000 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Lodestar_ExplainedVariance** | **100x200** |  **9,957.61 μs** | **442.687 μs** | **24.265 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.28 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,275.28 μs |  96.057 μs |  5.265 μs |  1.63 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **142.49 μs** |   **2.905 μs** |  **0.159 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    197.22 μs |   8.687 μs |  0.476 μs |  1.38 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,068.29 μs** | **211.551 μs** | **11.596 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,873.14 μs | 297.449 μs | 16.304 μs |  0.94 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **24.26 μs** |   **0.670 μs** |  **0.037 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.26 μs |   0.645 μs |  0.035 μs |  1.12 |   0.0916 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ProcessExtractBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Extract**    | **1**     | **7.325 ms** | **0.2555 ms** | **0.0140 ms** |  **1.00** |     **118 B** |        **1.00** |
| ExtractOne | 1     | 7.180 ms | 0.1922 ms | 0.0105 ms |  0.98 |         - |        0.00 |
|            |       |          |           |           |       |           |             |
| **Extract**    | **5**     | **7.334 ms** | **0.0512 ms** | **0.0028 ms** |  **1.00** |     **214 B** |        **1.00** |
| ExtractOne | 5     | 7.173 ms | 0.0772 ms | 0.0042 ms |  0.98 |         - |        0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.QgramBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **JaccardSimilarity** | **1** | **4.985 μs** | **0.6778 μs** | **0.0372 μs** | **0.0076** |     **192 B** |
| **JaccardSimilarity** | **3** | **6.210 μs** | **0.8658 μs** | **0.0475 μs** | **0.0076** |     **192 B** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| NdcgTieAveraged                   | 100000 | 40.727 ms | 1.4222 ms | 0.0780 ms |       - |       - |       - |  800353 B |
| NdcgIgnoringTies                  | 100000 | 38.785 ms | 0.9171 ms | 0.0503 ms |       - |       - |       - |  800353 B |
| DcgTieAveraged                    | 100000 | 24.022 ms | 1.3133 ms | 0.0720 ms |       - |       - |       - |  800215 B |
| ReciprocalRankScore               | 100000 | 20.058 ms | 0.1719 ms | 0.0094 ms |       - |       - |       - |         - |
| CoverageErrorScore                | 100000 |  6.597 ms | 0.0571 ms | 0.0031 ms | 15.6250 | 15.6250 | 15.6250 |  800271 B |
| LabelRankingAveragePrecisionScore | 100000 | 59.501 ms | 1.2216 ms | 0.0670 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RatcliffObershelpBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Similarity_Containment**   | **64**     |     **414.5 ns** |        **66.02 ns** |      **3.62 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Similarity_NearDuplicate | 64     |   5,478.1 ns |       146.49 ns |      8.03 ns | 13.22 |    0.10 |         - |          NA |
|                          |        |              |                 |              |       |         |           |             |
| **Similarity_Containment**   | **512**    |  **20,452.0 ns** |       **134.88 ns** |      **7.39 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Similarity_NearDuplicate | 512    | 664,288.6 ns | 1,199,531.93 ns | 65,750.39 ns | 32.48 |    2.78 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method   | Samples | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-----------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **48.50 μs** |   **0.214 μs** |  **0.012 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    47.48 μs |   3.537 μs |  0.194 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   122.22 μs |   1.175 μs |  0.064 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   514.93 μs | 338.067 μs | 18.531 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **484.76 μs** |  **26.755 μs** |  **1.467 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   473.97 μs |  40.465 μs |  2.218 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,222.25 μs |  25.625 μs |  1.405 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,713.28 μs | 894.373 μs | 49.024 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RobustScalerSparseBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method | Shape        | Mean     | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|------- |------------- |---------:|---------:|---------:|--------:|--------:|--------:|----------:|
| **Fit**    | **2000x2000x20** | **26.01 ms** | **2.549 ms** | **0.140 ms** | **93.7500** | **93.7500** | **93.7500** |  **375.3 KB** |
| **Fit**    | **500x20000x5**  | **44.66 ms** | **4.028 ms** | **0.221 ms** |       **-** |       **-** |       **-** | **492.47 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ScalerIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  Job-DYYGIJ : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2
  ShortRun   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX2

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method                            | Job        | InvocationCount | IterationCount | RunStrategy | UnrollFactor | WarmupCount | RowCount | Mean         | Error        | StdDev       | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|---------------------------------- |----------- |---------------- |--------------- |------------ |------------- |------------ |--------- |-------------:|-------------:|-------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_MinMax**                   | **Job-DYYGIJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **1000**     |    **120.36 μs** |    **13.287 μs** |    **14.768 μs** |  **1.01** |    **0.17** |        **-** |        **-** |        **-** |   **79.59 KB** |        **1.00** |
| Lodestar_MaxAbs                   | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    104.51 μs |     5.586 μs |     5.736 μs |  0.88 |    0.11 |        - |        - |        - |   79.14 KB |        0.99 |
| Lodestar_Robust                   | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,047.36 μs |    36.257 μs |    37.233 μs | 17.24 |    1.97 |        - |        - |        - |  157.51 KB |        1.98 |
| Lodestar_Standard                 | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    181.92 μs |    39.099 μs |    45.026 μs |  1.53 |    0.41 |        - |        - |        - |   79.66 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |    395.39 μs |    11.359 μs |    12.625 μs |  3.33 |    0.39 |        - |        - |        - |    8.65 KB |        0.11 |
| MlNet_NormalizeMinMax_Read        | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  2,080.32 μs |    92.305 μs |   102.596 μs | 17.52 |    2.15 |        - |        - |        - |  324.81 KB |        4.08 |
| MlNet_NormalizeRobustScaling_Read | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 1000     |  5,459.32 μs |    86.778 μs |    96.453 μs | 45.97 |    5.25 |        - |        - |        - |   467.2 KB |        5.87 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     48.68 μs |     1.150 μs |     0.063 μs |  1.00 |    0.00 |   4.7607 |        - |        - |   78.87 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     46.80 μs |     1.784 μs |     0.098 μs |  0.96 |    0.00 |   4.7607 |        - |        - |    78.4 KB |        0.99 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    408.56 μs |    53.315 μs |     2.922 μs |  8.39 |    0.05 |   9.2773 |        - |        - |  156.79 KB |        1.99 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |     66.27 μs |     2.319 μs |     0.127 μs |  1.36 |    0.00 |   4.7607 |        - |        - |   78.71 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,451.67 μs | 4,214.858 μs |   231.031 μs | 29.82 |    4.11 |  83.4961 |   7.3242 |        - |  1366.2 KB |       17.32 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |    993.24 μs |   667.595 μs |    36.593 μs | 20.40 |    0.65 |  27.3438 |        - |        - |   479.4 KB |        6.08 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 1000     |  1,730.27 μs | 2,227.685 μs |   122.107 μs | 35.54 |    2.17 |  35.1563 |        - |        - |  610.06 KB |        7.74 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| **Lodestar_MinMax**                   | **Job-DYYGIJ** | **1**               | **20**             | **Throughput**  | **1**            | **5**           | **20000**    |  **1,508.14 μs** |    **13.955 μs** |    **16.071 μs** |  **1.00** |    **0.01** |        **-** |        **-** |        **-** | **1563.96 KB** |       **1.000** |
| Lodestar_MaxAbs                   | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,416.38 μs |     7.875 μs |     8.426 μs |  0.94 |    0.01 |        - |        - |        - | 1563.23 KB |       1.000 |
| Lodestar_Robust                   | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 17,391.34 μs |    90.733 μs |    93.176 μs | 11.53 |    0.13 |        - |        - |        - | 3126.26 KB |       1.999 |
| Lodestar_Standard                 | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  1,985.00 μs |    28.236 μs |    31.385 μs |  1.32 |    0.02 |        - |        - |        - | 1564.03 KB |       1.000 |
| MlNet_NormalizeMinMax_Fit         | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    |  6,198.25 μs |    61.494 μs |    63.150 μs |  4.11 |    0.06 |        - |        - |        - |    9.02 KB |       0.006 |
| MlNet_NormalizeMinMax_Read        | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 19,075.36 μs | 6,258.130 μs | 7,206.872 μs | 12.65 |    4.67 |        - |        - |        - |   470.3 KB |       0.301 |
| MlNet_NormalizeRobustScaling_Read | Job-DYYGIJ | 1               | 20             | Throughput  | 1            | 5           | 20000    | 21,057.13 μs |   231.256 μs |   237.482 μs | 13.96 |    0.21 |        - |        - |        - | 4146.02 KB |       2.651 |
|                                   |            |                 |                |             |              |             |          |              |              |              |       |         |          |          |          |            |             |
| Lodestar_MinMax                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,335.85 μs |   229.334 μs |    12.571 μs |  1.00 |    0.01 | 332.0313 | 332.0313 | 332.0313 | 1564.31 KB |        1.00 |
| Lodestar_MaxAbs                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,061.11 μs |   157.412 μs |     8.628 μs |  0.79 |    0.01 | 332.0313 | 332.0313 | 332.0313 | 1562.99 KB |        1.00 |
| Lodestar_Robust                   | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 16,362.88 μs |    50.016 μs |     2.742 μs | 12.25 |    0.10 | 875.0000 | 875.0000 | 875.0000 | 3126.12 KB |        2.00 |
| Lodestar_Standard                 | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,520.35 μs | 1,223.593 μs |    67.069 μs |  1.14 |    0.04 | 332.0313 | 332.0313 | 332.0313 |  1563.3 KB |        1.00 |
| MlNet_NormalizeMinMax_Fit         | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  1,256.85 μs |   943.775 μs |    51.732 μs |  0.94 |    0.03 |  19.5313 |   3.9063 |        - |  345.85 KB |        0.22 |
| MlNet_NormalizeMinMax_Read        | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    |  4,683.01 μs | 3,365.054 μs |   184.450 μs |  3.51 |    0.12 |  31.2500 |        - |        - |  533.45 KB |        0.34 |
| MlNet_NormalizeRobustScaling_Read | ShortRun   | Default         | 3              | Default     | 16           | 3           | 20000    | 21,272.56 μs |   871.304 μs |    47.759 μs | 15.93 |    0.13 | 250.0000 | 125.0000 |        - | 4197.07 KB |        2.68 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method          | Model   | Mean     | Error   | StdDev  | Gen0      | Allocated |
|---------------- |-------- |---------:|--------:|--------:|----------:|----------:|
| **EncodeDocuments** | **Llama2**  | **150.7 ms** | **2.87 ms** | **0.16 ms** | **2250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **144.0 ms** | **2.99 ms** | **0.16 ms** | **2250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SilhouetteBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method    | Samples | Mean      | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------- |-------- |----------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **PerSample** | **2000**    |  **25.82 ms** |  **0.437 ms** | **0.024 ms** | **31.2500** | **31.2500** | **31.2500** | **148.75 KB** |
| **PerSample** | **5000**    | **173.79 ms** | **28.319 ms** | **1.552 ms** |       **-** |       **-** |       **-** |  **371.6 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **ExactPairwise**        | **500**       | **64**           |  **56.038 ms** |  **1.6110 ms** | **0.0883 ms** |  **1.00** |  **444.4444** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 64           |  12.219 ms |  0.2269 ms | 0.0124 ms |  0.22 |  171.8750 |  156.2500 |  78.1250 |   2492.47 KB |        0.31 |
| SignaturesOnly       | 500       | 64           |  10.319 ms |  0.7484 ms | 0.0410 ms |  0.18 |   15.6250 |         - |        - |    304.74 KB |        0.04 |
| AffineSignaturesOnly | 500       | 64           |   8.382 ms |  0.6421 ms | 0.0352 ms |  0.15 |   15.6250 |         - |        - |    305.29 KB |        0.04 |
| FingerprintsOnly     | 500       | 64           |  10.560 ms |  1.8963 ms | 0.1039 ms |  0.19 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **500**       | **128**          |  **55.538 ms** |  **1.2023 ms** | **0.0659 ms** |  **1.00** |  **444.4444** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify     | 500       | 128          |  14.203 ms |  1.0809 ms | 0.0593 ms |  0.26 |  312.5000 |  312.5000 | 218.7500 |   4538.23 KB |        0.56 |
| SignaturesOnly       | 500       | 128          |  10.788 ms |  0.4625 ms | 0.0254 ms |  0.19 |   15.6250 |         - |        - |    429.74 KB |        0.05 |
| AffineSignaturesOnly | 500       | 128          |   7.094 ms |  0.2491 ms | 0.0137 ms |  0.13 |   23.4375 |         - |        - |    430.78 KB |        0.05 |
| FingerprintsOnly     | 500       | 128          |  12.031 ms |  1.1061 ms | 0.0606 ms |  0.22 |   15.6250 |         - |        - |     429.7 KB |        0.05 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **64**           | **971.899 ms** | **23.9813 ms** | **1.3145 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |       **1.000** |
| SketchThenVerify     | 2000      | 64           |  46.427 ms |  1.7311 ms | 0.0949 ms |  0.05 |  818.1818 |  818.1818 | 454.5455 |  10165.72 KB |       0.080 |
| SignaturesOnly       | 2000      | 64           |  40.827 ms |  1.9434 ms | 0.1065 ms |  0.04 |         - |         - |        - |   1218.84 KB |       0.010 |
| AffineSignaturesOnly | 2000      | 64           |  27.117 ms |  0.7428 ms | 0.0407 ms |  0.03 |   62.5000 |         - |        - |   1219.36 KB |       0.010 |
| FingerprintsOnly     | 2000      | 64           |  48.168 ms |  0.6632 ms | 0.0364 ms |  0.05 |   90.9091 |         - |        - |   1718.82 KB |       0.014 |
|                      |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**        | **2000**      | **128**          | **972.138 ms** | **28.4136 ms** | **1.5574 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify     | 2000      | 128          |  64.925 ms | 29.4677 ms | 1.6152 ms |  0.07 | 1500.0000 | 1500.0000 | 750.0000 |  18494.97 KB |        0.15 |
| SignaturesOnly       | 2000      | 128          |  43.296 ms |  0.8704 ms | 0.0477 ms |  0.04 |   83.3333 |         - |        - |   1718.85 KB |        0.01 |
| AffineSignaturesOnly | 2000      | 128          |  27.909 ms |  0.3452 ms | 0.0189 ms |  0.03 |   93.7500 |         - |        - |   1719.86 KB |        0.01 |
| FingerprintsOnly     | 2000      | 128          |  42.140 ms |  1.6027 ms | 0.0879 ms |  0.04 |   83.3333 |         - |        - |   1718.81 KB |        0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SplitterIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                          | SampleCount | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|-------------------------------- |------------ |--------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar_KFold**                  | **10000**       |     **86.084 μs** |    **23.3329 μs** |   **1.2790 μs** |  **1.00** |    **0.02** |  **16.6016** |   **6.5918** |        **-** |  **273.94 KB** |        **1.00** |
| Lodestar_StratifiedKFold        | 10000       |    205.643 μs |    39.6698 μs |   2.1744 μs |  2.39 |    0.04 |  19.0430 |   4.6387 |        - |  313.53 KB |        1.14 |
| Lodestar_TrainTest              | 10000       |      4.673 μs |     0.1296 μs |   0.0071 μs |  0.05 |    0.00 |   2.3880 |   0.2594 |        - |   39.14 KB |        0.14 |
| MlNet_CrossValidationSplit      | 10000       |    217.618 μs |    56.7254 μs |   3.1093 μs |  2.53 |    0.05 |   1.9531 |        - |        - |   40.55 KB |        0.15 |
| MlNet_CrossValidationSplit_Read | 10000       |  5,437.783 μs |   410.8889 μs |  22.5222 μs | 63.18 |    0.85 |   7.8125 |        - |        - |  149.06 KB |        0.54 |
| MlNet_TrainTestSplit            | 10000       |     41.800 μs |    20.2427 μs |   1.1096 μs |  0.49 |    0.01 |   0.4883 |        - |        - |   12.55 KB |        0.05 |
| MlNet_TrainTestSplit_Read       | 10000       |  1,044.217 μs |   118.5549 μs |   6.4984 μs | 12.13 |    0.17 |   1.9531 |        - |        - |   34.26 KB |        0.13 |
|                                 |             |               |               |             |       |         |          |          |          |            |             |
| **Lodestar_KFold**                  | **100000**      |  **1,121.455 μs** |   **137.9781 μs** |   **7.5630 μs** |  **1.00** |    **0.01** | **416.0156** | **402.3438** | **392.5781** |  **2738.1 KB** |       **1.000** |
| Lodestar_StratifiedKFold        | 100000      |  2,356.774 μs |   279.8195 μs |  15.3379 μs |  2.10 |    0.02 | 597.6563 | 582.0313 | 574.2188 | 3130.57 KB |       1.143 |
| Lodestar_TrainTest              | 100000      |     82.813 μs |    55.1965 μs |   3.0255 μs |  0.07 |    0.00 |  49.5605 |  49.5605 |  49.5605 |  391.11 KB |       0.143 |
| MlNet_CrossValidationSplit      | 100000      |    252.887 μs |    28.9538 μs |   1.5871 μs |  0.23 |    0.00 |   2.4414 |   0.4883 |        - |   40.55 KB |       0.015 |
| MlNet_CrossValidationSplit_Read | 100000      | 41,832.030 μs | 2,792.2445 μs | 153.0523 μs | 37.30 |    0.25 |        - |        - |        - |  148.79 KB |       0.054 |
| MlNet_TrainTestSplit            | 100000      |     47.121 μs |     0.6471 μs |   0.0355 μs |  0.04 |    0.00 |   0.7324 |   0.2441 |        - |   12.55 KB |       0.005 |
| MlNet_TrainTestSplit_Read       | 100000      |  7,612.007 μs |   305.9698 μs |  16.7712 μs |  6.79 |    0.04 |        - |        - |        - |   34.19 KB |       0.012 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|---------:|---------:|---------:|-----------:|------------:|
| **Count**                | **200**       |  **4.956 ms** | **0.4158 ms** | **0.0228 ms** |  **1.00** | **109.3750** | **109.3750** | **109.3750** | **1366.79 KB** |        **1.00** |
| CountWithStopWords   | 200       |  4.362 ms | 0.1201 ms | 0.0066 ms |  0.88 |  39.0625 |  15.6250 |        - |  725.83 KB |        0.53 |
| Hashing              | 200       |  5.049 ms | 0.6244 ms | 0.0342 ms |  1.02 |  70.3125 |  70.3125 |  70.3125 | 1051.16 KB |        0.77 |
| HashingWithStopWords | 200       |  4.616 ms | 0.5760 ms | 0.0316 ms |  0.93 |  31.2500 |        - |        - |   591.9 KB |        0.43 |
|                      |           |           |           |           |       |          |          |          |            |             |
| **Count**                | **1000**      | **15.951 ms** | **0.1465 ms** | **0.0080 ms** |  **1.00** | **812.5000** | **812.5000** | **812.5000** | **5880.38 KB** |        **1.00** |
| CountWithStopWords   | 1000      | 14.105 ms | 1.1588 ms | 0.0635 ms |  0.88 | 375.0000 | 375.0000 | 375.0000 | 3155.02 KB |        0.54 |
| Hashing              | 1000      | 16.658 ms | 0.5408 ms | 0.0296 ms |  1.04 | 656.2500 | 562.5000 | 562.5000 | 4793.65 KB |        0.82 |
| HashingWithStopWords | 1000      | 15.270 ms | 0.8269 ms | 0.0453 ms |  0.96 | 265.6250 | 265.6250 | 265.6250 | 2633.78 KB |        0.45 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TextRankBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method  | Words | Mean      | Error     | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|-------- |------ |----------:|----------:|---------:|----------:|----------:|---------:|----------:|
| **Extract** | **2000**  |  **15.97 ms** |  **0.852 ms** | **0.047 ms** |  **187.5000** |   **93.7500** |        **-** |   **3.13 MB** |
| **Extract** | **8000**  | **301.20 ms** | **10.842 ms** | **0.594 ms** | **2000.0000** | **1500.0000** | **500.0000** |  **35.34 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **22.02 ms** |  **0.901 ms** | **0.049 ms** |  **1.00** |    **0.00** |  **531.2500** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  56.67 ms | 11.612 ms | 0.637 ms |  2.57 |    0.03 |  222.2222 |   3.55 MB |        0.41 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **48.28 ms** |  **2.258 ms** | **0.124 ms** |  **1.00** |    **0.00** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  57.45 ms |  0.261 ms | 0.014 ms |  1.19 |    0.00 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **89.53 ms** |  **3.196 ms** | **0.175 ms** |  **1.00** |    **0.00** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 281.01 ms | 36.312 ms | 1.990 ms |  3.14 |    0.02 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TopKAccuracyBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method | Classes | Mean      | Error    | StdDev   | Allocated |
|------- |-------- |----------:|---------:|---------:|----------:|
| **TopTwo** | **10**      |  **13.59 ms** | **0.226 ms** | **0.012 ms** |         **-** |
| **TopTwo** | **100**     | **106.03 ms** | **4.051 ms** | **0.222 ms** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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
| **Dot**    | **384**  |  **51.29 ns** | **3.856 ns** | **0.211 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  48.73 ns | 1.116 ns | 0.061 ns |  0.95 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **99.57 ns** | **3.931 ns** | **0.215 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  91.12 ns | 1.294 ns | 0.071 ns |  0.92 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **133.02 ns** | **2.241 ns** | **0.123 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 124.44 ns | 0.945 ns | 0.052 ns |  0.94 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method                | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|---------------------- |---------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| **Count**                 | **200**       |  **2.421 ms** | **0.1853 ms** | **0.0102 ms** |  **1.00** |    **0.01** |   **15.6250** |   **11.7188** |         **-** |      **303 KB** |        **1.00** |
| Tfidf                 | 200       |  2.481 ms | 1.1485 ms | 0.0630 ms |  1.03 |    0.02 |   15.6250 |    7.8125 |         - |   332.13 KB |        1.10 |
| CountBigrams          | 200       |  2.894 ms | 0.3362 ms | 0.0184 ms |  1.20 |    0.01 |   31.2500 |   15.6250 |         - |   590.34 KB |        1.95 |
| CountCharWordBoundary | 200       |  2.209 ms | 0.0093 ms | 0.0005 ms |  0.91 |    0.00 |  382.8125 |  382.8125 |  382.8125 |   1685.7 KB |        5.56 |
| Hashing               | 200       |  2.433 ms | 0.2117 ms | 0.0116 ms |  1.01 |    0.01 |   11.7188 |    7.8125 |         - |   233.65 KB |        0.77 |
|                       |           |           |           |           |       |         |           |           |           |             |             |
| **Count**                 | **1000**      |  **4.583 ms** | **0.3236 ms** | **0.0177 ms** |  **1.00** |    **0.00** |  **109.3750** |  **109.3750** |  **109.3750** |  **1280.03 KB** |        **1.00** |
| Tfidf                 | 1000      |  4.715 ms | 0.2043 ms | 0.0112 ms |  1.03 |    0.00 |  140.6250 |  140.6250 |  140.6250 |  1424.43 KB |        1.11 |
| CountBigrams          | 1000      |  6.645 ms | 0.3382 ms | 0.0185 ms |  1.45 |    0.01 |  398.4375 |  398.4375 |  398.4375 |  2291.91 KB |        1.79 |
| CountCharWordBoundary | 1000      | 13.494 ms | 0.5827 ms | 0.0319 ms |  2.94 |    0.01 | 1046.8750 | 1015.6250 | 1015.6250 | 12022.21 KB |        9.39 |
| Hashing               | 1000      |  4.568 ms | 0.2734 ms | 0.0150 ms |  1.00 |    0.00 |   70.3125 |   70.3125 |   70.3125 |  1015.48 KB |        0.79 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

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

| Method   | Documents | Mean       | Error      | StdDev    | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **5.590 ms** |   **5.408 ms** | **0.2964 ms** |  **1.00** |    **0.06** |   **140.6250** |   **140.6250** |   **140.6250** |   **2.01 MB** |        **1.00** |
| MlNet    | 200       |  58.456 ms |  51.501 ms | 2.8230 ms | 10.48 |    0.64 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |       14.09 |
|          |           |            |            |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **20.699 ms** |   **2.570 ms** | **0.1409 ms** |  **1.00** |    **0.01** |  **1500.0000** |  **1500.0000** |  **1500.0000** |   **9.17 MB** |        **1.00** |
| MlNet    | 1000      | 415.828 ms | 144.819 ms | 7.9380 ms | 20.09 |    0.35 | 78000.0000 | 78000.0000 | 78000.0000 |  324.3 MB |       35.35 |

<!-- markdownlint-enable MD060 -->

### compare-glm

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| glm_negative_binomial_n1000 | 0.376 | 3.047 | 8.10x | 0.376 | 3.046 | 8.10x |
| glm_gamma_n1000 | 0.457 | 3.909 | 8.56x | 0.457 | 3.909 | 8.56x |
| glm_poisson_exposure_n1000 | 0.384 | 3.514 | 9.14x | 0.384 | 3.513 | 9.14x |
| mnlogit_n1000 | 0.833 | 16.371 | 19.65x | 0.833 | 16.370 | 19.65x |
| glm_negative_binomial_n10000 | 3.414 | 11.543 | 3.38x | 3.420 | 11.541 | 3.37x |
| glm_gamma_n10000 | 3.962 | 13.287 | 3.35x | 3.968 | 13.285 | 3.35x |
| glm_poisson_exposure_n10000 | 4.136 | 13.179 | 3.19x | 4.143 | 13.178 | 3.18x |
| mnlogit_n10000 | 9.133 | 97.944 | 10.72x | 9.139 | 97.934 | 10.72x |
| glm_negative_binomial_n100000 | 34.580 | 112.666 | 3.26x | 34.795 | 448.932 | 12.90x |
| glm_gamma_n100000 | 40.786 | 127.894 | 3.14x | 40.978 | 509.582 | 12.44x |
| glm_poisson_exposure_n100000 | 52.931 | 123.803 | 2.34x | 53.435 | 494.396 | 9.25x |
| mnlogit_n100000 | 83.143 | 890.448 | 10.71x | 83.485 | 1568.357 | 18.79x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 108.7 | 24.9 | 4.37x C# faster |
| latin | 32 | 158.6 | 69.4 | 2.29x C# faster |
| latin | 128 | 471.9 | 406.7 | 1.16x C# faster |
| latin | 512 | 4854.3 | 5522.3 | 1.14x Py faster |
| cjk | 8 | 127.5 | 24.9 | 5.12x C# faster |
| cjk | 32 | 233.3 | 143.2 | 1.63x C# faster |
| cjk | 128 | 2045.9 | 1546.1 | 1.32x C# faster |
| cjk | 512 | 16763.0 | 9406.6 | 1.78x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 143.7 | 18.1 | 7.93x C# faster |
| latin | 32 | 267.4 | 129.0 | 2.07x C# faster |
| latin | 128 | 1815.0 | 953.9 | 1.90x C# faster |
| latin | 512 | 15561.5 | 13364.3 | 1.16x C# faster |
| cjk | 8 | 136.5 | 18.1 | 7.55x C# faster |
| cjk | 32 | 287.9 | 192.0 | 1.50x C# faster |
| cjk | 128 | 3030.3 | 2269.2 | 1.34x C# faster |
| cjk | 512 | 26156.3 | 20237.7 | 1.29x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.005 | 0.780 | 172.19x | 0.005 | 0.780 | 172.18x |
| accuracy_n1000_k2 | 0.000 | 0.405 | 3386.20x | 0.000 | 0.405 | 3385.29x |
| precision_recall_f1_macro_n1000_k2 | 0.004 | 1.411 | 324.22x | 0.004 | 1.411 | 324.19x |
| classification_report_n1000_k2 | 0.007 | 5.328 | 816.78x | 0.007 | 5.328 | 816.77x |
| roc_auc_binary_n1000_k2 | 0.016 | 1.568 | 97.08x | 0.016 | 1.568 | 97.08x |
| balanced_accuracy_n1000_k2 | 0.004 | 0.831 | 195.75x | 0.004 | 0.831 | 195.74x |
| matthews_n1000_k2 | 0.004 | 1.580 | 370.47x | 0.004 | 1.580 | 369.88x |
| cohen_kappa_n1000_k2 | 0.004 | 0.885 | 207.57x | 0.004 | 0.885 | 207.58x |
| mse_n1000_k2 | 0.001 | 0.221 | 425.78x | 0.001 | 0.221 | 425.74x |
| mae_n1000_k2 | 0.001 | 0.219 | 425.90x | 0.001 | 0.219 | 425.92x |
| median_ae_n1000_k2 | 0.005 | 0.236 | 50.85x | 0.005 | 0.236 | 50.85x |
| r2_n1000_k2 | 0.001 | 0.276 | 208.83x | 0.001 | 0.276 | 208.81x |
| confusion_matrix_n1000_k10 | 0.005 | 0.781 | 168.16x | 0.005 | 0.781 | 168.13x |
| accuracy_n1000_k10 | 0.000 | 0.408 | 3405.80x | 0.000 | 0.408 | 3405.88x |
| precision_recall_f1_macro_n1000_k10 | 0.005 | 1.442 | 318.49x | 0.005 | 1.442 | 318.50x |
| classification_report_n1000_k10 | 0.010 | 5.587 | 578.21x | 0.010 | 5.586 | 578.20x |
| roc_auc_ovr_macro_n1000_k10 | 0.188 | 8.266 | 44.01x | 0.188 | 8.266 | 44.01x |
| balanced_accuracy_n1000_k10 | 0.005 | 0.839 | 184.73x | 0.005 | 0.839 | 184.74x |
| matthews_n1000_k10 | 0.005 | 1.623 | 359.49x | 0.005 | 1.623 | 359.48x |
| cohen_kappa_n1000_k10 | 0.005 | 0.895 | 181.06x | 0.005 | 0.895 | 181.09x |
| mse_n1000_k10 | 0.001 | 0.219 | 416.77x | 0.001 | 0.219 | 416.77x |
| mae_n1000_k10 | 0.001 | 0.218 | 420.29x | 0.001 | 0.218 | 420.32x |
| median_ae_n1000_k10 | 0.005 | 0.234 | 50.19x | 0.005 | 0.234 | 50.19x |
| r2_n1000_k10 | 0.001 | 0.273 | 206.16x | 0.001 | 0.273 | 206.17x |
| confusion_matrix_n100000_k2 | 0.435 | 10.961 | 25.21x | 0.435 | 10.960 | 25.22x |
| accuracy_n100000_k2 | 0.012 | 3.786 | 319.04x | 0.012 | 3.785 | 319.06x |
| precision_recall_f1_macro_n100000_k2 | 0.404 | 12.552 | 31.06x | 0.404 | 12.551 | 31.06x |
| classification_report_n100000_k2 | 0.407 | 26.805 | 65.78x | 0.407 | 26.804 | 65.78x |
| roc_auc_binary_n100000_k2 | 2.933 | 28.430 | 9.69x | 2.933 | 28.425 | 9.69x |
| balanced_accuracy_n100000_k2 | 0.404 | 11.022 | 27.28x | 0.404 | 11.022 | 27.28x |
| matthews_n100000_k2 | 0.404 | 22.049 | 54.56x | 0.404 | 22.048 | 54.56x |
| cohen_kappa_n100000_k2 | 0.404 | 11.071 | 27.38x | 0.404 | 11.071 | 27.38x |
| mse_n100000_k2 | 0.048 | 0.370 | 7.72x | 0.048 | 0.370 | 7.72x |
| mae_n100000_k2 | 0.047 | 0.371 | 7.88x | 0.047 | 0.371 | 7.88x |
| median_ae_n100000_k2 | 0.573 | 1.891 | 3.30x | 0.593 | 1.891 | 3.19x |
| r2_n100000_k2 | 0.122 | 0.584 | 4.77x | 0.122 | 0.584 | 4.77x |
| confusion_matrix_n100000_k10 | 0.375 | 10.928 | 29.14x | 0.375 | 10.927 | 29.14x |
| accuracy_n100000_k10 | 0.012 | 3.785 | 319.25x | 0.012 | 3.785 | 319.24x |
| precision_recall_f1_macro_n100000_k10 | 0.376 | 13.261 | 35.32x | 0.375 | 13.260 | 35.32x |
| classification_report_n100000_k10 | 0.383 | 29.754 | 77.73x | 0.383 | 29.749 | 77.73x |
| roc_auc_ovr_macro_n100000_k10 | 29.297 | 231.288 | 7.89x | 29.293 | 231.274 | 7.90x |
| balanced_accuracy_n100000_k10 | 0.375 | 11.036 | 29.42x | 0.375 | 11.035 | 29.42x |
| matthews_n100000_k10 | 0.375 | 22.853 | 60.87x | 0.375 | 22.851 | 60.88x |
| cohen_kappa_n100000_k10 | 0.435 | 11.076 | 25.47x | 0.435 | 11.075 | 25.47x |
| mse_n100000_k10 | 0.048 | 0.373 | 7.75x | 0.048 | 0.373 | 7.75x |
| mae_n100000_k10 | 0.047 | 0.372 | 7.88x | 0.047 | 0.372 | 7.88x |
| median_ae_n100000_k10 | 0.608 | 1.891 | 3.11x | 0.656 | 1.891 | 2.88x |
| r2_n100000_k10 | 0.122 | 0.591 | 4.83x | 0.122 | 0.591 | 4.83x |
| confusion_matrix_n1000000_k2 | 4.044 | 102.679 | 25.39x | 4.045 | 102.665 | 25.38x |
| accuracy_n1000000_k2 | 0.116 | 34.147 | 293.60x | 0.116 | 34.141 | 293.64x |
| precision_recall_f1_macro_n1000000_k2 | 4.045 | 112.528 | 27.82x | 4.044 | 112.511 | 27.82x |
| classification_report_n1000000_k2 | 4.058 | 219.204 | 54.02x | 4.057 | 219.185 | 54.02x |
| roc_auc_binary_n1000000_k2 | 41.965 | 311.840 | 7.43x | 41.956 | 311.805 | 7.43x |
| balanced_accuracy_n1000000_k2 | 4.047 | 102.743 | 25.39x | 4.047 | 102.739 | 25.39x |
| matthews_n1000000_k2 | 4.047 | 207.097 | 51.18x | 4.046 | 207.084 | 51.18x |
| cohen_kappa_n1000000_k2 | 4.045 | 102.753 | 25.41x | 4.044 | 102.752 | 25.41x |
| mse_n1000000_k2 | 0.477 | 1.801 | 3.78x | 0.477 | 1.801 | 3.78x |
| mae_n1000000_k2 | 0.468 | 1.785 | 3.81x | 0.468 | 1.784 | 3.81x |
| median_ae_n1000000_k2 | 5.161 | 15.457 | 3.00x | 5.224 | 15.455 | 2.96x |
| r2_n1000000_k2 | 1.220 | 3.478 | 2.85x | 1.220 | 3.478 | 2.85x |
| confusion_matrix_n1000000_k10 | 3.686 | 102.359 | 27.77x | 3.686 | 102.333 | 27.76x |
| accuracy_n1000000_k10 | 0.112 | 34.178 | 303.82x | 0.112 | 34.175 | 303.82x |
| precision_recall_f1_macro_n1000000_k10 | 3.709 | 119.361 | 32.18x | 3.709 | 119.341 | 32.18x |
| classification_report_n1000000_k10 | 3.762 | 247.127 | 65.69x | 3.762 | 247.121 | 65.69x |
| balanced_accuracy_n1000000_k10 | 3.693 | 102.506 | 27.76x | 3.693 | 102.493 | 27.76x |
| matthews_n1000000_k10 | 3.735 | 214.451 | 57.42x | 3.734 | 214.435 | 57.42x |
| cohen_kappa_n1000000_k10 | 3.705 | 102.540 | 27.68x | 3.705 | 102.530 | 27.68x |
| mse_n1000000_k10 | 0.476 | 1.804 | 3.79x | 0.476 | 1.804 | 3.79x |
| mae_n1000000_k10 | 0.468 | 1.786 | 3.82x | 0.468 | 1.785 | 3.82x |
| median_ae_n1000000_k10 | 5.631 | 15.429 | 2.74x | 5.797 | 15.427 | 2.66x |
| r2_n1000000_k10 | 1.221 | 3.419 | 2.80x | 1.221 | 3.418 | 2.80x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.050 | 2.694 | 53.96x | 0.050 | 2.693 | 53.97x |
| ols_hac_n1000 | 0.174 | 3.195 | 18.41x | 0.174 | 3.194 | 18.41x |
| ols_cluster_n1000 | 0.065 | 3.237 | 49.77x | 0.065 | 3.237 | 49.77x |
| ols_summary_n10000 | 0.473 | 11.332 | 23.97x | 0.473 | 11.331 | 23.97x |
| ols_hac_n10000 | 1.781 | 12.691 | 7.13x | 1.788 | 12.690 | 7.10x |
| ols_cluster_n10000 | 0.638 | 12.994 | 20.37x | 0.642 | 12.992 | 20.25x |
| ols_summary_n100000 | 4.658 | 105.467 | 22.64x | 4.682 | 420.119 | 89.73x |
| ols_hac_n100000 | 17.735 | 115.879 | 6.53x | 18.009 | 462.617 | 25.69x |
| ols_cluster_n100000 | 6.863 | 119.573 | 17.42x | 7.042 | 476.868 | 67.72x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.364 | 9.999 | 1.86x | 5.608 | 9.998 | 1.78x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.395 | 17.218 | 1.39x | 12.773 | 17.216 | 1.35x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.663 | 38.187 | 3.02x | 12.852 | 38.187 | 2.97x | 1,990,038 | 1,990,038 |
| spiece_model | 4.970 | 30.707 | 6.18x | 5.270 | 30.706 | 5.83x | 533,084 | 533,084 |
| tfidf_save | 2.090 | 2.427 | 1.16x | 2.120 | 2.427 | 1.14x | 581,787 | 591,922 |
| tfidf_load | 5.316 | 4.321 | 0.81x | 5.622 | 4.321 | 0.77x | 581,787 | 591,922 |
| embedding_index_save | 4.058 | 1.311 | 0.32x | 4.311 | 1.311 | 0.30x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.303 | 37.382 | 0.74x | 9.619 | 4.362 | 0.45x | 20,589,007 | 15,360,128 |
| embedding_index_load | 4.723 | 1.417 | 0.30x | 5.081 | 1.417 | 0.28x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 5.748 | 0.869 | 0.15x | 6.239 | 0.866 | 0.14x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 3.625 | 1.409 | 0.39x | 3.958 | 1.409 | 0.36x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.173 | 1.409 | 1.20x | 1.384 | 1.409 | 1.02x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 70.63x | 0.000 | 0.001 | 70.62x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 460.264 | 637.021 | 1.38x | 460.253 | 636.986 | 1.38x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 74.392 | 72.649 | 0.98x | 74.568 | 72.644 | 0.97x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.085 | 0.110 | 1.29x | 0.085 | 0.110 | 1.29x |
| stratified_n10000 | 0.189 | 0.782 | 4.15x | 0.189 | 0.782 | 4.14x |
| traintest_n10000 | 0.005 | 0.178 | 38.56x | 0.005 | 0.178 | 38.56x |
| kfold_n100000 | 1.178 | 2.990 | 2.54x | 1.300 | 2.990 | 2.30x |
| stratified_n100000 | 2.203 | 8.212 | 3.73x | 2.334 | 8.211 | 3.52x |
| traintest_n100000 | 0.209 | 0.380 | 1.82x | 0.210 | 0.380 | 1.81x |
| kfold_n1000000 | 10.819 | 18.862 | 1.74x | 11.658 | 18.858 | 1.62x |
| stratified_n1000000 | 23.734 | 63.321 | 2.67x | 24.609 | 63.311 | 2.57x |
| traintest_n1000000 | 0.734 | 2.193 | 2.99x | 0.856 | 2.192 | 2.56x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.632 | 140.75x | 0.004 | 0.632 | 140.73x |
| mann_whitney_n1000 | 0.032 | 0.606 | 18.96x | 0.032 | 0.606 | 18.96x |
| chi_square_n1000 | 0.000 | 0.254 | 1419.96x | 0.000 | 0.254 | 1419.72x |
| welch_t_n10000 | 0.042 | 0.685 | 16.17x | 0.042 | 0.685 | 16.17x |
| mann_whitney_n10000 | 0.465 | 3.015 | 6.49x | 0.464 | 3.015 | 6.49x |
| chi_square_n10000 | 0.001 | 0.257 | 198.71x | 0.001 | 0.257 | 198.72x |
| welch_t_n100000 | 0.424 | 1.103 | 2.60x | 0.424 | 1.103 | 2.60x |
| mann_whitney_n100000 | 6.137 | 31.029 | 5.06x | 6.136 | 31.026 | 5.06x |
| chi_square_n100000 | 0.014 | 0.273 | 19.11x | 0.014 | 0.273 | 19.11x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-var

_As of 2026-09-19, measured at commit `4f685cd7fe136f1f05c542322e3fa2cee8a50d29`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| var_n1000 | 0.041 | 2.554 | 62.23x | 0.041 | 2.554 | 62.23x |
| var_n10000 | 0.499 | 16.576 | 33.21x | 0.502 | 16.573 | 33.00x |
| var_n100000 | 4.180 | 189.080 | 45.23x | 4.351 | 399.988 | 91.93x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
