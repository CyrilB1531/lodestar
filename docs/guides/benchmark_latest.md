# Latest known benchmark result, per method

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml` from the
> wiki's own history, alongside [nightly_run](nightly_run).

**Not a comparison across methods.** Each section below is the last night that method was
actually re-run -- whichever night touched the source near it, not necessarily last night,
and not the same night as its neighbours here. Every run measures on a GitHub hosted
runner whose hardware differs night to night, so a number here says "this is the last
known reading", never "faster than the section above it".

## Per method

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

_As of 2026-09-10, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

_As of 2026-09-10, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method             | CorpusSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0    | Gen1   | Allocated  | Alloc Ratio |
|------------------- |----------- |-------------:|------------:|-----------:|------:|--------:|--------:|-------:|-----------:|------------:|
| **UnitLoop**           | **1**          |     **6.211 μs** |   **1.5080 μs** |  **0.0827 μs** |  **1.00** |    **0.02** |  **0.1526** |      **-** |     **2.6 KB** |        **1.00** |
| EmbedBatch         | 1          |     6.476 μs |   0.2829 μs |  0.0155 μs |  1.04 |    0.01 |  0.1831 |      - |       3 KB |        1.15 |
| EmbedBatchBucketed | 1          |     6.442 μs |   0.5240 μs |  0.0287 μs |  1.04 |    0.01 |  0.1831 |      - |       3 KB |        1.15 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **8**          |    **95.614 μs** |  **11.3246 μs** |  **0.6207 μs** |  **1.00** |    **0.01** |  **5.7373** | **0.1221** |   **94.76 KB** |        **1.00** |
| EmbedBatch         | 8          |    59.688 μs |   3.2092 μs |  0.1759 μs |  0.62 |    0.00 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
| EmbedBatchBucketed | 8          |    55.791 μs |  13.5135 μs |  0.7407 μs |  0.58 |    0.01 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **32**         |   **348.810 μs** |  **27.7637 μs** |  **1.5218 μs** |  **1.00** |    **0.01** | **20.0195** | **0.4883** |  **334.02 KB** |        **1.00** |
| EmbedBatch         | 32         |   199.389 μs |  47.2748 μs |  2.5913 μs |  0.57 |    0.01 | 18.5547 | 1.2207 |  306.63 KB |        0.92 |
| EmbedBatchBucketed | 32         |   185.304 μs |  28.9588 μs |  1.5873 μs |  0.53 |    0.00 | 17.8223 | 0.9766 |  293.12 KB |        0.88 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **128**        | **1,421.660 μs** | **524.6611 μs** | **28.7584 μs** |  **1.00** |    **0.02** | **80.0781** | **3.9063** | **1336.03 KB** |        **1.00** |
| EmbedBatch         | 128        |   764.779 μs |  69.8039 μs |  3.8262 μs |  0.54 |    0.01 | 74.2188 | 9.7656 | 1225.67 KB |        0.92 |
| EmbedBatchBucketed | 128        |   719.765 μs | 394.9960 μs | 21.6511 μs |  0.51 |    0.02 | 70.3125 | 9.7656 | 1158.15 KB |        0.87 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **132.04 ms** | **15.240 ms** | **0.835 ms** |  **1.00** |    **0.01** |   **27.18 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  64.78 ms |  1.723 ms | 0.094 ms |  0.49 |    0.00 |  103.68 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **138.85 ms** | **63.549 ms** | **3.483 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  60.40 ms |  2.304 ms | 0.126 ms |  0.44 |    0.01 |  116.47 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **199.87 ms** |  **1.956 ms** | **0.107 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 245.89 ms | 27.049 ms | 1.483 ms |  1.23 |    0.01 |     259 KB |        2.50 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **204.03 ms** |  **0.578 ms** | **0.032 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 209.30 ms | 11.421 ms | 0.626 ms |  1.03 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **253.02 ms** | **37.648 ms** | **2.064 ms** |  **1.00** |    **0.01** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 337.17 ms | 10.832 ms | 0.594 ms |  1.33 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **256.98 ms** | **32.218 ms** | **1.766 ms** |  **1.00** |    **0.01** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 309.13 ms |  9.613 ms | 0.527 ms |  1.20 |    0.01 | 1152.95 KB |        1.55 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **294.13 ms** | **59.508 ms** | **3.262 ms** |  **1.00** |    **0.01** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 393.75 ms | 20.095 ms | 1.101 ms |  1.34 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **297.41 ms** | **17.798 ms** | **0.976 ms** |  **1.00** |    **0.00** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 394.92 ms |  4.091 ms | 0.224 ms |  1.33 |    0.00 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method | length | Mean          | Error         | StdDev     | Allocated |
|------- |------- |--------------:|--------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **50.03 μs** |      **0.957 μs** |   **0.052 μs** |         **-** |
| Cjk    | 1000   |      57.95 μs |      2.345 μs |   0.129 μs |         - |
| **Latin**  | **10000**  |   **5,512.76 μs** |    **870.803 μs** |  **47.732 μs** |         **-** |
| Cjk    | 10000  |   6,736.70 μs |    319.942 μs |  17.537 μs |         - |
| **Latin**  | **65536**  | **202,111.11 μs** | **17,642.589 μs** | **967.050 μs** |         **-** |

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

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method           | Documents | Mean           | Error           | StdDev        | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------- |---------- |---------------:|----------------:|--------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **LodestarQuery**    | **1000**      |      **27.778 μs** |       **0.3712 μs** |     **0.0203 μs** |   **1.00** |    **0.00** |     **0.7324** |         **-** |         **-** |     **12.27 KB** |        **1.00** |
| LuceneQuery      | 1000      |       3.269 μs |       0.1563 μs |     0.0086 μs |   0.12 |    0.00 |     0.3128 |         - |         - |      5.14 KB |        0.42 |
| LodestarFromText | 1000      |  13,877.790 μs |   1,952.0021 μs |   106.9958 μs | 499.60 |    3.35 |  1671.8750 | 1312.5000 |  500.0000 |  21361.96 KB |    1,741.61 |
| LuceneFromText   | 1000      |   7,121.991 μs |   4,221.3244 μs |   231.3850 μs | 256.39 |    7.22 |    78.1250 |   62.5000 |         - |   1342.85 KB |      109.48 |
|                  |           |                |                 |               |        |         |            |           |           |              |             |
| **LodestarQuery**    | **20000**     |   **1,361.072 μs** |      **42.4639 μs** |     **2.3276 μs** |   **1.00** |    **0.00** |     **5.8594** |    **1.9531** |    **1.9531** |    **234.94 KB** |        **1.00** |
| LuceneQuery      | 20000     |      18.765 μs |       0.6131 μs |     0.0336 μs |   0.01 |    0.00 |     0.4883 |         - |         - |      8.45 KB |        0.04 |
| LodestarFromText | 20000     | 281,709.841 μs |  26,478.2382 μs | 1,451.3616 μs | 206.98 |    0.97 | 24000.0000 | 6000.0000 | 1000.0000 | 418590.77 KB |    1,781.66 |
| LuceneFromText   | 20000     | 145,222.090 μs | 108,494.6597 μs | 5,946.9584 μs | 106.70 |    3.79 |  1250.0000 | 1000.0000 |         - |  21891.74 KB |       93.18 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method  | Mean     | Error    | StdDev  | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|--------:|------:|----------:|----------:|------------:|
| Unigram | 308.1 ms | 10.86 ms | 0.60 ms |  1.00 | 1500.0000 |  30.32 MB |        1.00 |
| Bpe     | 534.7 ms | 56.62 ms | 3.10 ms |  1.74 | 7000.0000 | 112.18 MB |        3.70 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method                    | Length | Mean     | Error    | StdDev  | Gen0   | Allocated |
|-------------------------- |------- |---------:|---------:|--------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    | **103.6 μs** |  **5.70 μs** | **0.31 μs** | **1.2207** |  **20.38 KB** |
| **BpeOnOnePathologicalToken** | **1024**   | **222.9 μs** | **21.00 μs** | **1.15 μs** | **2.4414** |  **39.93 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **475.5 μs** | **20.17 μs** | **1.11 μs** | **4.3945** |  **78.98 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **988.7 μs** | **56.63 μs** | **3.10 μs** | **7.8125** | **157.03 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method     | Alphabet | Mean      | Error     | StdDev   | Allocated |
|----------- |--------- |----------:|----------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **20.07 μs** |  **1.132 μs** | **0.062 μs** |         **-** |
| MyersGroup | cjk      | 241.53 μs | 70.833 μs | 3.883 μs |         - |
| **DpGroup**    | **latin**    |  **10.02 μs** |  **0.819 μs** | **0.045 μs** |         **-** |
| MyersGroup | latin    | 130.56 μs |  2.502 μs | 0.137 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method                                    | Mean      | Error    | StdDev   | Ratio | RatioSD |
|------------------------------------------ |----------:|---------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       |  28.88 ms | 2.715 ms | 0.149 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 204.26 ms | 3.481 ms | 0.191 ms |  7.07 |    0.03 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  23.98 ms | 1.492 ms | 0.082 ms |  0.83 |    0.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method         | Mean         | Error      | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |-------------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
| Ratio          |     99.31 ns |   9.331 ns |  0.511 ns |   1.00 |    0.01 |      - |         - |          NA |
| PartialRatio   | 11,447.22 ns | 374.940 ns | 20.552 ns | 115.27 |    0.55 |      - |         - |          NA |
| TokenSortRatio |  1,125.93 ns | 513.144 ns | 28.127 ns |  11.34 |    0.25 | 0.0782 |    1312 B |          NA |
| TokenSetRatio  |  1,150.58 ns | 154.576 ns |  8.473 ns |  11.59 |    0.09 | 0.0858 |    1448 B |          NA |
| WRatio         |  2,297.82 ns | 451.480 ns | 24.747 ns |  23.14 |    0.24 | 0.1640 |    2760 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method     | Operation     | Mean        | Error       | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|------------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **100.5 ns** |     **1.90 ns** |  **0.10 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    206.2 ns |    24.47 ns |  1.34 ns |  2.05 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  | **11,420.4 ns** |   **227.54 ns** | **12.47 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,124.5 ns | 1,748.90 ns | 95.86 ns |  0.89 |    0.01 |      - |     160 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,145.5 ns** |    **99.08 ns** |  **5.43 ns** |  **1.00** |    **0.01** | **0.0858** |    **1448 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,174.6 ns |   293.11 ns | 16.07 ns |  1.90 |    0.01 | 0.1144 |    1944 B |        1.34 |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **2,308.2 ns** |   **663.22 ns** | **36.35 ns** |  **1.00** |    **0.02** | **0.1640** |    **2760 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,921.0 ns |   342.59 ns | 18.78 ns |  2.13 |    0.03 | 0.1831 |    3128 B |        1.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method                     | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |      **26.71 ns** |      **0.934 ns** |     **0.051 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |     130.29 ns |     10.720 ns |     0.588 ns |  4.88 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |      26.96 ns |      0.167 ns |     0.009 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |      26.31 ns |      0.446 ns |     0.024 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **12**     |      **29.39 ns** |      **1.044 ns** |     **0.057 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |     142.95 ns |      5.865 ns |     0.322 ns |  4.86 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |      28.51 ns |      1.285 ns |     0.070 ns |  0.97 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |      27.22 ns |      0.430 ns |     0.024 ns |  0.93 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **16**     |      **30.65 ns** |      **6.723 ns** |     **0.369 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |     165.45 ns |      5.723 ns |     0.314 ns |  5.40 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |      32.39 ns |     25.448 ns |     1.395 ns |  1.06 |    0.04 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |      30.24 ns |      0.230 ns |     0.013 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **20**     |      **35.32 ns** |      **1.068 ns** |     **0.059 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |     188.99 ns |      1.821 ns |     0.100 ns |  5.35 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |      34.75 ns |      0.463 ns |     0.025 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |      33.19 ns |      2.805 ns |     0.154 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **24**     |      **54.76 ns** |      **3.382 ns** |     **0.185 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |     666.02 ns |     31.948 ns |     1.751 ns | 12.16 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |      55.54 ns |      1.742 ns |     0.096 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |      55.08 ns |      8.309 ns |     0.455 ns |  1.01 |    0.01 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **32**     |      **61.73 ns** |      **0.761 ns** |     **0.042 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |   1,086.54 ns |      6.968 ns |     0.382 ns | 17.60 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |      65.12 ns |      2.348 ns |     0.129 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |      59.07 ns |      5.871 ns |     0.322 ns |  0.96 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **128**    |     **938.95 ns** |     **18.994 ns** |     **1.041 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |  21,894.10 ns |  7,326.318 ns |   401.580 ns | 23.32 |    0.37 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |     917.19 ns |     11.234 ns |     0.616 ns |  0.98 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |     937.87 ns |    110.564 ns |     6.060 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **512**    |   **7,791.97 ns** |    **206.803 ns** |    **11.336 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 339,764.72 ns | 22,750.982 ns | 1,247.058 ns | 43.60 |    0.15 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |   8,391.39 ns |    130.584 ns |     7.158 ns |  1.08 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    |   7,619.94 ns |    199.233 ns |    10.921 ns |  0.98 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **131.91 ns** |     **3.472 ns** |   **0.190 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     56.24 ns |     2.535 ns |   0.139 ns |  0.43 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    132.05 ns |     1.199 ns |   0.066 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     97.55 ns |     3.808 ns |   0.209 ns |  0.74 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **230.52 ns** |   **129.975 ns** |   **7.124 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 12   |     63.54 ns |     0.042 ns |   0.002 ns |  0.28 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    217.28 ns |    35.643 ns |   1.954 ns |  0.94 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |    108.69 ns |     0.435 ns |   0.024 ns |  0.47 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **282.16 ns** |   **200.242 ns** |  **10.976 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 14   |     66.90 ns |     0.218 ns |   0.012 ns |  0.24 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    277.93 ns |   322.168 ns |  17.659 ns |  0.99 |    0.06 |         - |          NA |
| Kernel_Cjk | 14   |    112.62 ns |     0.576 ns |   0.032 ns |  0.40 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **363.88 ns** |   **256.457 ns** |  **14.057 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 16   |     72.22 ns |     2.984 ns |   0.164 ns |  0.20 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    351.05 ns |     2.582 ns |   0.142 ns |  0.97 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    118.59 ns |     1.690 ns |   0.093 ns |  0.33 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **438.19 ns** |    **39.432 ns** |   **2.161 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 18   |     73.91 ns |     1.762 ns |   0.097 ns |  0.17 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    438.33 ns |    51.494 ns |   2.823 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 18   |    124.18 ns |     0.579 ns |   0.032 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **772.96 ns** |    **19.971 ns** |   **1.095 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     81.30 ns |    53.354 ns |   2.924 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    769.53 ns |   182.808 ns |  10.020 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    127.92 ns |     2.511 ns |   0.138 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **996.21 ns** |   **159.960 ns** |   **8.768 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     86.06 ns |     2.555 ns |   0.140 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |  1,006.75 ns |   138.905 ns |   7.614 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    141.58 ns |     1.901 ns |   0.104 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,549.45 ns** |   **621.372 ns** |  **34.060 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 32   |    105.34 ns |     7.343 ns |   0.402 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,555.84 ns |   543.463 ns |  29.789 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 32   |    166.56 ns |     5.039 ns |   0.276 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,212.60 ns** | **2,957.851 ns** | **162.130 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 48   |    130.52 ns |     5.981 ns |   0.328 ns |  0.04 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,126.92 ns |   580.352 ns |  31.811 ns |  0.97 |    0.04 |         - |          NA |
| Kernel_Cjk | 48   |    241.19 ns |     9.764 ns |   0.535 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,097.31 ns** | **4,618.527 ns** | **253.157 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Kernel     | 64   |    160.41 ns |     5.232 ns |   0.287 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,476.38 ns |   128.271 ns |   7.031 ns |  0.90 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    291.15 ns |     1.504 ns |   0.082 ns |  0.05 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,775.72 ns** |   **827.345 ns** |  **45.350 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    781.02 ns |     3.316 ns |   0.182 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,507.65 ns | 3,595.261 ns | 197.068 ns |  1.06 |    0.01 |         - |          NA |
| Kernel_Cjk | 96   |  1,060.71 ns |   228.560 ns |  12.528 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **25.60 ns** |     **1.424 ns** |  **0.078 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    123.10 ns |     3.721 ns |  0.204 ns |  4.81 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.22 ns |     0.071 ns |  0.004 ns |  0.98 |         - |          NA |
|                            |        |              |              |           |       |           |             |
| **Distance_Utf16**             | **64**     |    **269.81 ns** |    **14.327 ns** |  **0.785 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 64     |    680.33 ns |    59.626 ns |  3.268 ns |  2.52 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    271.31 ns |     1.334 ns |  0.073 ns |  1.01 |         - |          NA |
|                            |        |              |              |           |       |           |             |
| **Distance_Utf16**             | **512**    | **14,355.78 ns** |   **109.249 ns** |  **5.988 ns** |  **1.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 16,890.84 ns | 1,450.878 ns | 79.528 ns |  1.18 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 14,834.08 ns | 1,630.522 ns | 89.374 ns |  1.03 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **331.7 ns** |     **24.29 ns** |     **1.33 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     243.1 ns |      1.63 ns |     0.09 ns |  0.73 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **340.0 ns** |      **9.30 ns** |     **0.51 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     244.1 ns |     71.78 ns |     3.93 ns |  0.72 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **414.0 ns** |      **2.33 ns** |     **0.13 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     321.2 ns |      3.14 ns |     0.17 ns |  0.78 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **432.5 ns** |     **11.77 ns** |     **0.65 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     330.9 ns |      4.62 ns |     0.25 ns |  0.76 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **499.7 ns** |     **95.78 ns** |     **5.25 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     408.4 ns |     11.69 ns |     0.64 ns |  0.82 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **515.7 ns** |     **28.82 ns** |     **1.58 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     421.8 ns |      3.44 ns |     0.19 ns |  0.82 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **591.9 ns** |     **23.97 ns** |     **1.31 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,288.2 ns |     57.65 ns |     3.16 ns |  2.18 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **616.7 ns** |     **11.20 ns** |     **0.61 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,319.3 ns |     52.28 ns |     2.87 ns |  2.14 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,508.9 ns** |     **68.80 ns** |     **3.77 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,398.8 ns |     75.68 ns |     4.15 ns |  2.15 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,585.4 ns** |    **122.66 ns** |     **6.72 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,320.2 ns |     21.91 ns |     1.20 ns |  2.06 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **18,050.5 ns** |    **263.61 ns** |    **14.45 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  61,848.8 ns |    192.17 ns |    10.53 ns |  3.43 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **483,753.9 ns** | **69,680.55 ns** | **3,819.43 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  59,144.2 ns | 15,423.80 ns |   845.43 ns |  0.12 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **23.95 ns** |      **0.363 ns** |     **0.020 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      72.62 ns |      0.553 ns |     0.030 ns |  3.03 |    0.00 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      82.09 ns |     11.486 ns |     0.630 ns |  3.43 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     162.55 ns |      4.675 ns |     0.256 ns |  6.79 |    0.01 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **276.23 ns** |      **5.625 ns** |     **0.308 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   6,776.45 ns |    114.579 ns |     6.280 ns | 24.53 |    0.03 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,346.80 ns |     38.752 ns |     2.124 ns |  4.88 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |  11,231.31 ns |    324.840 ns |    17.806 ns | 40.66 |    0.07 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **14,436.78 ns** |    **146.206 ns** |     **8.014 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 393,106.19 ns |  6,024.412 ns |   330.218 ns | 27.23 |    0.02 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,695.41 ns |  7,188.624 ns |   394.033 ns |  2.61 |    0.02 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 715,633.26 ns | 91,670.973 ns | 5,024.795 ns | 49.57 |    0.30 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method         | Samples | Classes | Mean            | Error           | StdDev       | Gen0   | Allocated |
|--------------- |-------- |-------- |----------------:|----------------:|-------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |      **7,572.5 ns** |       **441.41 ns** |     **24.20 ns** | **0.0153** |     **312 B** |
| MatrixWeighted | 1000    | 2       |      7,441.5 ns |       667.71 ns |     36.60 ns | 0.0153 |     312 B |
| AccuracyScore  | 1000    | 2       |        917.8 ns |         5.12 ns |      0.28 ns |      - |         - |
| F1Macro        | 1000    | 2       |      7,674.8 ns |       550.01 ns |     30.15 ns | 0.0153 |     472 B |
| Report         | 1000    | 2       |     10,442.8 ns |     1,928.60 ns |    105.71 ns | 0.3815 |    6520 B |
| **Matrix**         | **1000**    | **10**      |      **7,806.5 ns** |       **345.26 ns** |     **18.92 ns** | **0.0610** |    **1248 B** |
| MatrixWeighted | 1000    | 10      |      7,613.4 ns |        24.99 ns |      1.37 ns | 0.0687 |    1248 B |
| AccuracyScore  | 1000    | 10      |        920.6 ns |        28.04 ns |      1.54 ns |      - |         - |
| F1Macro        | 1000    | 10      |      8,150.3 ns |       350.52 ns |     19.21 ns | 0.0916 |    1664 B |
| Report         | 1000    | 10      |     14,854.3 ns |     1,299.37 ns |     71.22 ns | 0.9155 |   15496 B |
| **Matrix**         | **100000**  | **2**       |    **833,057.5 ns** |   **149,066.33 ns** |  **8,170.83 ns** |      **-** |     **313 B** |
| MatrixWeighted | 100000  | 2       |    810,565.0 ns |    88,628.58 ns |  4,858.03 ns |      - |     313 B |
| AccuracyScore  | 100000  | 2       |    163,847.2 ns |    11,387.52 ns |    624.19 ns |      - |         - |
| F1Macro        | 100000  | 2       |    860,566.7 ns |    11,308.17 ns |    619.84 ns |      - |     473 B |
| Report         | 100000  | 2       |    879,207.5 ns |   198,778.72 ns | 10,895.73 ns |      - |    6545 B |
| **Matrix**         | **100000**  | **10**      |    **940,136.9 ns** |    **21,849.41 ns** |  **1,197.64 ns** |      **-** |    **1249 B** |
| MatrixWeighted | 100000  | 10      |    934,982.2 ns |    71,887.35 ns |  3,940.39 ns |      - |    1249 B |
| AccuracyScore  | 100000  | 10      |    264,424.7 ns |     4,195.12 ns |    229.95 ns |      - |         - |
| F1Macro        | 100000  | 10      |    984,188.5 ns |    31,495.97 ns |  1,726.40 ns |      - |    1665 B |
| Report         | 100000  | 10      |    992,799.7 ns |    88,080.34 ns |  4,827.98 ns |      - |   15841 B |
| **Matrix**         | **1000000** | **2**       |  **8,679,499.3 ns** |   **239,902.88 ns** | **13,149.89 ns** |      **-** |     **324 B** |
| MatrixWeighted | 1000000 | 2       |  8,511,322.0 ns | 1,167,700.75 ns | 64,005.62 ns |      - |     324 B |
| AccuracyScore  | 1000000 | 2       |  1,743,453.9 ns |     4,240.08 ns |    232.41 ns |      - |         - |
| F1Macro        | 1000000 | 2       |  8,279,294.7 ns |   284,252.54 ns | 15,580.84 ns |      - |     484 B |
| Report         | 1000000 | 2       |  8,773,416.6 ns |   654,491.86 ns | 35,874.91 ns |      - |    6572 B |
| **Matrix**         | **1000000** | **10**      | **10,023,514.2 ns** |   **105,905.48 ns** |  **5,805.04 ns** |      **-** |    **1260 B** |
| MatrixWeighted | 1000000 | 10      |  9,343,244.3 ns |   551,563.16 ns | 30,233.04 ns |      - |    1260 B |
| AccuracyScore  | 1000000 | 10      |  2,689,505.3 ns |    44,127.56 ns |  2,418.78 ns |      - |         - |
| F1Macro        | 1000000 | 10      |  9,963,208.9 ns |   412,583.72 ns | 22,615.11 ns |      - |    1676 B |
| Report         | 1000000 | 10      | 10,311,437.4 ns |   229,765.61 ns | 12,594.23 ns |      - |   15892 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method   | Samples | Request       | Mean         | Error        | StdDev      | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |-------------:|-------------:|------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **8,581.7 μs** |    **150.79 μs** |     **8.27 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1016 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  34,246.6 μs |  3,046.01 μs |   166.96 μs |   3.99 |    0.02 | 600.0000 | 600.0000 | 600.0000 |  5090157 B |    5,010.00 |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |     **259.9 μs** |     **66.23 μs** |     **3.63 μs** |   **1.00** |    **0.02** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  34,314.5 μs |  1,879.47 μs |   103.02 μs | 132.03 |    1.62 | 600.0000 | 600.0000 | 600.0000 |  5089622 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **104,004.2 μs** | **50,983.38 μs** | **2,794.57 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 219,346.7 μs |  4,346.07 μs |   238.22 μs |   2.11 |    0.05 |        - |        - |        - | 23231768 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |   **2,601.0 μs** |    **162.94 μs** |     **8.93 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 224,945.3 μs |  2,256.97 μs |   123.71 μs |  86.49 |    0.26 |        - |        - |        - | 23228104 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **74.09 ns** |    **12.927 ns** |   **0.709 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 4    |     73.63 ns |     0.785 ns |   0.043 ns |  0.99 |    0.01 |         - |          NA |
| Dp_Cjk     | 4    |     73.56 ns |     5.254 ns |   0.288 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 4    |     73.37 ns |     0.899 ns |   0.049 ns |  0.99 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **6**    |    **105.81 ns** |     **2.506 ns** |   **0.137 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     77.27 ns |     0.303 ns |   0.017 ns |  0.73 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    103.84 ns |     6.717 ns |   0.368 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 6    |    132.20 ns |     4.058 ns |   0.222 ns |  1.25 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **8**    |    **156.52 ns** |     **1.684 ns** |   **0.092 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     87.24 ns |     0.250 ns |   0.014 ns |  0.56 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    156.52 ns |     7.925 ns |   0.434 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    178.82 ns |    27.305 ns |   1.497 ns |  1.14 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **10**   |    **202.62 ns** |    **22.227 ns** |   **1.218 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 10   |     95.63 ns |     0.272 ns |   0.015 ns |  0.47 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    197.32 ns |    10.069 ns |   0.552 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    153.56 ns |     0.683 ns |   0.037 ns |  0.76 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **244.18 ns** |     **4.132 ns** |   **0.226 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    103.33 ns |     1.913 ns |   0.105 ns |  0.42 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    250.15 ns |   170.663 ns |   9.355 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 12   |    164.00 ns |     5.935 ns |   0.325 ns |  0.67 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **415.67 ns** |   **156.357 ns** |   **8.570 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 16   |    121.75 ns |     1.661 ns |   0.091 ns |  0.29 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    417.23 ns |     6.861 ns |   0.376 ns |  1.00 |    0.02 |         - |          NA |
| Kernel_Cjk | 16   |    179.66 ns |     3.340 ns |   0.183 ns |  0.43 |    0.01 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **853.20 ns** |   **138.592 ns** |   **7.597 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |    158.72 ns |     4.833 ns |   0.265 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    848.37 ns |   130.913 ns |   7.176 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 24   |    227.23 ns |     0.563 ns |   0.031 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,447.62 ns** |    **78.605 ns** |   **4.309 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    195.12 ns |    27.742 ns |   1.521 ns |  0.13 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,422.72 ns |    69.258 ns |   3.796 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    273.38 ns |    64.629 ns |   3.543 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,300.35 ns** |   **116.777 ns** |   **6.401 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    265.22 ns |     1.122 ns |   0.062 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,388.00 ns | 1,378.990 ns |  75.587 ns |  1.03 |    0.02 |         - |          NA |
| Kernel_Cjk | 48   |    359.97 ns |     1.849 ns |   0.101 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **6,285.78 ns** | **2,152.092 ns** | **117.963 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 64   |    338.92 ns |    12.390 ns |   0.679 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  6,198.21 ns |   341.308 ns |  18.708 ns |  0.99 |    0.02 |         - |          NA |
| Kernel_Cjk | 64   |    454.07 ns |     8.183 ns |   0.449 ns |  0.07 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **13,948.43 ns** |    **68.855 ns** |   **3.774 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |  1,243.00 ns |   199.098 ns |  10.913 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 13,958.86 ns |   360.764 ns |  19.775 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   |  1,619.82 ns |   124.866 ns |   6.844 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method                 | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| VocabTxt               |  4.209 ms | 3.0879 ms | 0.1693 ms | 117.1875 | 109.3750 |  39.0625 |   3.62 MB |
| TokenizerJsonWordPiece | 11.102 ms | 2.0538 ms | 0.1126 ms | 187.5000 | 171.8750 |  46.8750 |   5.72 MB |
| TokenizerJsonUnigram   | 12.470 ms | 0.4933 ms | 0.0270 ms |  93.7500 |  78.1250 |  31.2500 |   4.64 MB |
| SpieceModel            |  3.815 ms | 3.2581 ms | 0.1786 ms | 109.3750 | 101.5625 |  31.2500 |   3.36 MB |
| TfidfSave              |  1.841 ms | 0.2019 ms | 0.0111 ms |  29.2969 |  23.4375 |  23.4375 |   2.09 MB |
| TfidfLoad              |  4.362 ms | 0.8277 ms | 0.0454 ms |  85.9375 |  78.1250 |  23.4375 |   2.86 MB |
| EmbeddingIndexSave     |  4.011 ms | 0.6908 ms | 0.0379 ms | 273.4375 | 273.4375 | 273.4375 |  19.87 MB |
| EmbeddingIndexLoad     |  5.455 ms | 0.6383 ms | 0.0350 ms | 203.1250 | 171.8750 | 140.6250 |  15.72 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method           | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|----------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**    | **500**       | **64**           |  **51.466 ms** |  **1.5642 ms** | **0.0857 ms** |  **1.00** |  **400.0000** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify | 500       | 64           |  13.271 ms |  1.4259 ms | 0.0782 ms |  0.26 |  171.8750 |  156.2500 |  78.1250 |   2699.49 KB |        0.33 |
| SignaturesOnly   | 500       | 64           |   9.702 ms |  0.2841 ms | 0.0156 ms |  0.19 |   31.2500 |         - |        - |    511.75 KB |        0.06 |
| FingerprintsOnly | 500       | 64           |  10.602 ms |  0.4465 ms | 0.0245 ms |  0.21 |   31.2500 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **500**       | **128**          |  **50.033 ms** |  **1.6135 ms** | **0.0884 ms** |  **1.00** |  **400.0000** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify | 500       | 128          |  16.282 ms |  0.5148 ms | 0.0282 ms |  0.33 |  343.7500 |  312.5000 | 218.7500 |   4745.26 KB |        0.58 |
| SignaturesOnly   | 500       | 128          |  12.268 ms |  0.6625 ms | 0.0363 ms |  0.25 |   31.2500 |         - |        - |    636.75 KB |        0.08 |
| FingerprintsOnly | 500       | 128          |  10.385 ms |  1.0090 ms | 0.0553 ms |  0.21 |   31.2500 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **64**           | **883.841 ms** | **37.3258 ms** | **2.0460 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify | 2000      | 64           |  54.928 ms |  2.1869 ms | 0.1199 ms |  0.06 |  888.8889 |  888.8889 | 444.4444 |  10993.99 KB |        0.09 |
| SignaturesOnly   | 2000      | 64           |  38.912 ms |  5.8214 ms | 0.3191 ms |  0.04 |   76.9231 |         - |        - |   2046.95 KB |        0.02 |
| FingerprintsOnly | 2000      | 64           |  47.596 ms |  5.1545 ms | 0.2825 ms |  0.05 |   90.9091 |         - |        - |   2609.44 KB |        0.02 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **128**          | **884.947 ms** | **27.8285 ms** | **1.5254 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify | 2000      | 128          |  76.662 ms | 24.8068 ms | 1.3597 ms |  0.09 | 1571.4286 | 1571.4286 | 714.2857 |  19322.61 KB |        0.15 |
| SignaturesOnly   | 2000      | 128          |  49.303 ms |  1.0783 ms | 0.0591 ms |  0.06 |   90.9091 |         - |        - |   2546.97 KB |        0.02 |
| FingerprintsOnly | 2000      | 128          |  41.403 ms |  0.9949 ms | 0.0545 ms |  0.05 |  153.8462 |         - |        - |   2609.43 KB |        0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | Gen0      | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|----------:|---------:|---------:|----------:|------------:|
| **Count**                | **200**       |  **7.052 ms** | **0.8266 ms** | **0.0453 ms** |  **1.00** |  **500.0000** | **234.3750** |  **62.5000** |   **7.92 MB** |        **1.00** |
| CountWithStopWords   | 200       |  5.960 ms | 0.0397 ms | 0.0022 ms |  0.85 |  390.6250 | 187.5000 |        - |   6.34 MB |        0.80 |
| Hashing              | 200       |  7.016 ms | 0.3941 ms | 0.0216 ms |  0.99 |  507.8125 | 171.8750 |  70.3125 |   8.08 MB |        1.02 |
| HashingWithStopWords | 200       |  6.270 ms | 0.2109 ms | 0.0116 ms |  0.89 |  406.2500 | 156.2500 |        - |   6.52 MB |        0.82 |
|                      |           |           |           |           |       |           |          |          |           |             |
| **Count**                | **1000**      | **29.784 ms** | **0.9001 ms** | **0.0493 ms** |  **1.00** | **2666.6667** | **933.3333** | **533.3333** |  **38.64 MB** |        **1.00** |
| CountWithStopWords   | 1000      | 22.962 ms | 1.6824 ms | 0.0922 ms |  0.77 | 1968.7500 | 781.2500 | 250.0000 |  30.99 MB |        0.80 |
| Hashing              | 1000      | 27.387 ms | 3.2341 ms | 0.1773 ms |  0.92 | 2562.5000 | 750.0000 | 593.7500 |  39.55 MB |        1.02 |
| HashingWithStopWords | 1000      | 23.491 ms | 0.8428 ms | 0.0462 ms |  0.79 | 2031.2500 | 625.0000 | 250.0000 |  31.83 MB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method       | Model         | Mean      | Error    | StdDev   | Ratio | Gen0      | Gen1     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|---------:|---------:|------:|----------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **61.16 ms** | **4.287 ms** | **0.235 ms** |  **1.00** | **4222.2222** | **111.1111** |  **68.25 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.01 ms | 0.265 ms | 0.015 ms |  0.87 |  200.0000 |        - |   3.55 MB |        0.05 |
|              |               |           |          |          |       |           |          |           |             |
| **Lodestar**     | **SentencePiece** | **327.76 ms** | **6.401 ms** | **0.351 ms** |  **1.00** | **1500.0000** |        **-** |  **30.33 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  50.78 ms | 5.707 ms | 0.313 ms |  0.15 |  100.0000 |        - |   3.09 MB |        0.10 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method | Dim  | Mean      | Error     | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|----------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **53.55 ns** |  **1.188 ns** | **0.065 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  50.71 ns |  0.604 ns | 0.033 ns |  0.95 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **768**  |  **94.25 ns** |  **3.880 ns** | **0.213 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  92.58 ns |  4.871 ns | 0.267 ns |  0.98 |         - |          NA |
|        |      |           |           |          |       |           |             |
| **Dot**    | **1024** | **123.14 ns** |  **2.972 ns** | **0.163 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 122.71 ns | 10.916 ns | 0.598 ns |  1.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method       | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Count**        | **200**       |  **3.197 ms** | **5.6595 ms** | **0.3102 ms** |  **1.01** |    **0.12** |  **93.7500** |  **39.0625** |        **-** |    **1.6 MB** |        **1.00** |
| Tfidf        | 200       |  3.053 ms | 6.2689 ms | 0.3436 ms |  0.96 |    0.13 | 101.5625 |  39.0625 |        - |   1.63 MB |        1.02 |
| CountBigrams | 200       |  3.622 ms | 0.6018 ms | 0.0330 ms |  1.14 |    0.10 | 171.8750 | 109.3750 |        - |   2.78 MB |        1.74 |
| Hashing      | 200       |  2.799 ms | 0.2513 ms | 0.0138 ms |  0.88 |    0.08 |  97.6563 |  31.2500 |        - |    1.6 MB |        1.00 |
|              |           |           |           |           |       |         |          |          |          |           |             |
| **Count**        | **1000**      |  **6.738 ms** | **0.9640 ms** | **0.0528 ms** |  **1.00** |    **0.01** | **484.3750** | **343.7500** |  **62.5000** |   **7.83 MB** |        **1.00** |
| Tfidf        | 1000      |  6.932 ms | 0.6214 ms | 0.0341 ms |  1.03 |    0.01 | 484.3750 | 312.5000 |  93.7500 |   7.97 MB |        1.02 |
| CountBigrams | 1000      | 10.832 ms | 0.0568 ms | 0.0031 ms |  1.61 |    0.01 | 906.2500 | 375.0000 | 265.6250 |  13.42 MB |        1.71 |
| Hashing      | 1000      |  6.714 ms | 0.4062 ms | 0.0223 ms |  1.00 |    0.01 | 492.1875 | 156.2500 |  70.3125 |   7.85 MB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

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

| Method   | Documents | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **7.009 ms** |   **2.226 ms** |  **0.1220 ms** |  **1.00** |    **0.02** |   **296.8750** |   **218.7500** |   **140.6250** |   **5.13 MB** |        **1.00** |
| MlNet    | 200       |  50.878 ms |  68.432 ms |  3.7510 ms |  7.26 |    0.48 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |        5.52 |
|          |           |            |            |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **28.559 ms** |   **2.829 ms** |  **0.1550 ms** |  **1.00** |    **0.01** |  **2687.5000** |  **2468.7500** |  **1500.0000** |  **24.92 MB** |        **1.00** |
| MlNet    | 1000      | 376.817 ms | 332.790 ms | 18.2414 ms | 13.19 |    0.56 | 79000.0000 | 79000.0000 | 79000.0000 | 324.37 MB |       13.02 |

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 115.5 | 26.7 | 4.33x C# faster |
| latin | 32 | 177.9 | 85.9 | 2.07x C# faster |
| latin | 128 | 468.7 | 884.0 | 1.89x Py faster |
| latin | 512 | 4453.3 | 7533.9 | 1.69x Py faster |
| cjk | 8 | 146.9 | 27.4 | 5.37x C# faster |
| cjk | 32 | 336.1 | 222.0 | 1.51x C# faster |
| cjk | 128 | 1907.4 | 1602.8 | 1.19x C# faster |
| cjk | 512 | 14805.3 | 10672.2 | 1.39x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 147.5 | 17.5 | 8.42x C# faster |
| latin | 32 | 286.3 | 157.2 | 1.82x C# faster |
| latin | 128 | 1669.4 | 1482.2 | 1.13x C# faster |
| latin | 512 | 14203.0 | 14841.4 | 1.04x Py faster |
| cjk | 8 | 164.5 | 17.5 | 9.41x C# faster |
| cjk | 32 | 390.7 | 289.9 | 1.35x C# faster |
| cjk | 128 | 2852.2 | 2343.2 | 1.22x C# faster |
| cjk | 512 | 23866.8 | 18329.4 | 1.30x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.009 | 0.960 | 103.63x | 0.009 | 0.959 | 103.64x |
| accuracy_n1000_k2 | 0.001 | 0.508 | 493.28x | 0.001 | 0.508 | 493.22x |
| precision_recall_f1_macro_n1000_k2 | 0.008 | 1.727 | 223.92x | 0.008 | 1.727 | 223.92x |
| classification_report_n1000_k2 | 0.010 | 6.527 | 633.46x | 0.010 | 6.527 | 633.44x |
| roc_auc_binary_n1000_k2 | 0.016 | 1.867 | 118.15x | 0.016 | 1.867 | 118.15x |
| balanced_accuracy_n1000_k2 | 0.008 | 1.030 | 135.38x | 0.008 | 1.030 | 135.36x |
| matthews_n1000_k2 | 0.008 | 1.923 | 251.74x | 0.008 | 1.923 | 251.73x |
| cohen_kappa_n1000_k2 | 0.008 | 1.082 | 141.56x | 0.008 | 1.082 | 141.56x |
| mse_n1000_k2 | 0.002 | 0.305 | 126.09x | 0.002 | 0.305 | 126.08x |
| mae_n1000_k2 | 0.002 | 0.304 | 125.61x | 0.002 | 0.304 | 125.61x |
| median_ae_n1000_k2 | 0.006 | 0.316 | 50.09x | 0.006 | 0.316 | 50.09x |
| r2_n1000_k2 | 0.003 | 0.367 | 142.88x | 0.003 | 0.367 | 142.88x |
| confusion_matrix_n1000_k10 | 0.009 | 0.973 | 103.09x | 0.009 | 0.973 | 103.09x |
| accuracy_n1000_k10 | 0.001 | 0.516 | 457.84x | 0.001 | 0.516 | 457.84x |
| precision_recall_f1_macro_n1000_k10 | 0.008 | 1.767 | 213.05x | 0.008 | 1.766 | 213.04x |
| classification_report_n1000_k10 | 0.015 | 6.790 | 456.68x | 0.015 | 6.790 | 456.72x |
| roc_auc_ovr_macro_n1000_k10 | 0.549 | 9.676 | 17.64x | 0.549 | 9.675 | 17.64x |
| balanced_accuracy_n1000_k10 | 0.008 | 1.028 | 124.53x | 0.008 | 1.028 | 124.53x |
| matthews_n1000_k10 | 0.008 | 1.978 | 238.81x | 0.008 | 1.978 | 238.83x |
| cohen_kappa_n1000_k10 | 0.009 | 1.076 | 125.03x | 0.009 | 1.076 | 125.03x |
| mse_n1000_k10 | 0.002 | 0.305 | 126.34x | 0.002 | 0.305 | 126.35x |
| mae_n1000_k10 | 0.002 | 0.305 | 125.74x | 0.002 | 0.305 | 125.75x |
| median_ae_n1000_k10 | 0.006 | 0.315 | 50.07x | 0.006 | 0.315 | 50.07x |
| r2_n1000_k10 | 0.003 | 0.369 | 143.32x | 0.003 | 0.369 | 143.32x |
| confusion_matrix_n100000_k2 | 0.987 | 10.667 | 10.80x | 0.987 | 10.667 | 10.80x |
| accuracy_n100000_k2 | 0.185 | 3.742 | 20.21x | 0.185 | 3.742 | 20.21x |
| precision_recall_f1_macro_n100000_k2 | 0.862 | 12.230 | 14.19x | 0.862 | 12.230 | 14.19x |
| classification_report_n100000_k2 | 0.866 | 26.593 | 30.70x | 0.866 | 26.592 | 30.70x |
| roc_auc_binary_n100000_k2 | 3.519 | 26.284 | 7.47x | 3.518 | 26.283 | 7.47x |
| balanced_accuracy_n100000_k2 | 0.855 | 10.743 | 12.56x | 0.855 | 10.742 | 12.56x |
| matthews_n100000_k2 | 0.865 | 21.411 | 24.76x | 0.865 | 21.409 | 24.76x |
| cohen_kappa_n100000_k2 | 0.865 | 10.780 | 12.47x | 0.865 | 10.780 | 12.47x |
| mse_n100000_k2 | 0.238 | 0.458 | 1.92x | 0.238 | 0.458 | 1.92x |
| mae_n100000_k2 | 0.239 | 0.451 | 1.89x | 0.239 | 0.451 | 1.89x |
| median_ae_n100000_k2 | 0.750 | 1.790 | 2.39x | 0.771 | 1.790 | 2.32x |
| r2_n100000_k2 | 0.235 | 0.705 | 3.00x | 0.235 | 0.705 | 3.00x |
| confusion_matrix_n100000_k10 | 0.977 | 10.677 | 10.93x | 0.977 | 10.673 | 10.92x |
| accuracy_n100000_k10 | 0.271 | 3.744 | 13.81x | 0.271 | 3.744 | 13.81x |
| precision_recall_f1_macro_n100000_k10 | 0.991 | 12.908 | 13.02x | 0.991 | 12.906 | 13.02x |
| classification_report_n100000_k10 | 0.988 | 29.321 | 29.69x | 0.988 | 29.319 | 29.69x |
| roc_auc_ovr_macro_n100000_k10 | 36.555 | 211.279 | 5.78x | 36.555 | 211.254 | 5.78x |
| balanced_accuracy_n100000_k10 | 0.978 | 10.745 | 10.99x | 0.978 | 10.744 | 10.99x |
| matthews_n100000_k10 | 0.979 | 22.100 | 22.58x | 0.979 | 22.099 | 22.58x |
| cohen_kappa_n100000_k10 | 0.997 | 10.792 | 10.82x | 0.997 | 10.791 | 10.83x |
| mse_n100000_k10 | 0.238 | 0.461 | 1.93x | 0.238 | 0.461 | 1.93x |
| mae_n100000_k10 | 0.238 | 0.451 | 1.89x | 0.238 | 0.451 | 1.89x |
| median_ae_n100000_k10 | 0.807 | 1.787 | 2.21x | 0.862 | 1.787 | 2.07x |
| r2_n100000_k10 | 0.235 | 0.705 | 3.00x | 0.235 | 0.705 | 3.00x |
| confusion_matrix_n1000000_k2 | 8.621 | 98.480 | 11.42x | 8.621 | 98.478 | 11.42x |
| accuracy_n1000000_k2 | 1.956 | 32.670 | 16.70x | 1.956 | 32.668 | 16.70x |
| precision_recall_f1_macro_n1000000_k2 | 8.621 | 107.269 | 12.44x | 8.620 | 107.258 | 12.44x |
| classification_report_n1000000_k2 | 8.716 | 208.281 | 23.90x | 8.716 | 208.277 | 23.90x |
| roc_auc_binary_n1000000_k2 | 44.491 | 285.595 | 6.42x | 44.490 | 285.583 | 6.42x |
| balanced_accuracy_n1000000_k2 | 8.624 | 98.511 | 11.42x | 8.623 | 98.505 | 11.42x |
| matthews_n1000000_k2 | 8.694 | 198.938 | 22.88x | 8.694 | 198.926 | 22.88x |
| cohen_kappa_n1000000_k2 | 8.670 | 98.344 | 11.34x | 8.670 | 98.339 | 11.34x |
| mse_n1000000_k2 | 2.373 | 2.021 | 0.85x | 2.373 | 2.020 | 0.85x |
| mae_n1000000_k2 | 2.374 | 2.005 | 0.84x | 2.373 | 2.005 | 0.84x |
| median_ae_n1000000_k2 | 7.068 | 13.987 | 1.98x | 7.148 | 13.987 | 1.96x |
| r2_n1000000_k2 | 2.342 | 3.399 | 1.45x | 2.342 | 3.399 | 1.45x |
| confusion_matrix_n1000000_k10 | 9.769 | 98.185 | 10.05x | 9.769 | 98.180 | 10.05x |
| accuracy_n1000000_k10 | 2.789 | 32.615 | 11.69x | 2.789 | 32.615 | 11.70x |
| precision_recall_f1_macro_n1000000_k10 | 9.865 | 113.011 | 11.46x | 9.864 | 113.007 | 11.46x |
| classification_report_n1000000_k10 | 9.901 | 232.237 | 23.46x | 9.900 | 232.208 | 23.46x |
| balanced_accuracy_n1000000_k10 | 9.752 | 98.310 | 10.08x | 9.751 | 98.300 | 10.08x |
| matthews_n1000000_k10 | 9.851 | 204.982 | 20.81x | 9.850 | 204.951 | 20.81x |
| cohen_kappa_n1000000_k10 | 9.848 | 98.288 | 9.98x | 9.848 | 98.285 | 9.98x |
| mse_n1000000_k10 | 2.376 | 1.936 | 0.81x | 2.376 | 1.936 | 0.81x |
| mae_n1000000_k10 | 2.375 | 1.920 | 0.81x | 2.375 | 1.920 | 0.81x |
| median_ae_n1000000_k10 | 6.859 | 13.983 | 2.04x | 6.913 | 13.983 | 2.02x |
| r2_n1000000_k10 | 2.645 | 3.252 | 1.23x | 2.645 | 3.252 | 1.23x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

BELOW GATE on processor time:
  mse_n1000000_k2                  0.85x
  mae_n1000000_k2                  0.84x
  mse_n1000000_k10                 0.81x
  mae_n1000000_k10                 0.81x

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.249 | 3.022 | 12.13x | 0.249 | 3.022 | 12.13x |
| ols_summary_n10000 | 2.519 | 11.397 | 4.52x | 2.642 | 11.397 | 4.31x |
| ols_summary_n100000 | 23.454 | 108.920 | 4.64x | 23.974 | 434.387 | 18.12x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.634 | 9.675 | 2.09x | 4.981 | 9.675 | 1.94x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.606 | 15.831 | 1.26x | 12.970 | 15.831 | 1.22x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.981 | 34.916 | 2.50x | 14.328 | 34.910 | 2.44x | 1,990,038 | 1,990,038 |
| spiece_model | 4.672 | 27.906 | 5.97x | 4.894 | 27.904 | 5.70x | 533,084 | 533,084 |
| tfidf_save | 1.661 | 2.542 | 1.53x | 1.680 | 2.542 | 1.51x | 581,787 | 591,922 |
| tfidf_load | 4.705 | 3.893 | 0.83x | 5.927 | 3.893 | 0.66x | 581,787 | 591,922 |
| embedding_index_save | 4.175 | 1.404 | 0.34x | 4.388 | 1.404 | 0.32x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.341 | 37.180 | 0.75x | 10.314 | 4.529 | 0.44x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.132 | 1.221 | 0.24x | 5.392 | 1.221 | 0.23x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 6.004 | 0.776 | 0.13x | 6.319 | 0.776 | 0.12x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.307 | 1.199 | 0.28x | 4.610 | 1.199 | 0.26x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.297 | 1.193 | 0.92x | 1.554 | 1.193 | 0.77x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 102.31x | 0.000 | 0.001 | 102.31x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 410.324 | 556.856 | 1.36x | 410.384 | 556.850 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 78.179 | 66.262 | 0.85x | 79.673 | 66.258 | 0.83x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-09-12, measured at commit `f38a0c896419241561c30c0821b9ca02c0fb17f9`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.810 | 201.28x | 0.004 | 0.809 | 201.25x |
| mann_whitney_n1000 | 0.085 | 0.743 | 8.72x | 0.085 | 0.743 | 8.72x |
| chi_square_n1000 | 0.000 | 0.337 | 1248.06x | 0.000 | 0.337 | 1248.02x |
| welch_t_n10000 | 0.038 | 0.858 | 22.84x | 0.038 | 0.858 | 22.84x |
| mann_whitney_n10000 | 4.224 | 2.800 | 0.66x | 4.232 | 2.800 | 0.66x |
| chi_square_n10000 | 0.002 | 0.343 | 214.31x | 0.002 | 0.343 | 214.32x |
| welch_t_n100000 | 0.376 | 1.253 | 3.34x | 0.376 | 1.253 | 3.34x |
| mann_whitney_n100000 | 50.590 | 27.789 | 0.55x | 50.832 | 27.788 | 0.55x |
| chi_square_n100000 | 0.016 | 0.361 | 23.01x | 0.016 | 0.361 | 23.01x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
