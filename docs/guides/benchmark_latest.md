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

_As of 2026-09-12, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

_As of 2026-09-12, measured at commit `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`._

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

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean         | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated  | Alloc Ratio |
|------------------- |----------- |-------------:|------------:|----------:|------:|--------:|-------:|-----------:|------------:|
| **UnitLoop**           | **1**          |     **6.338 μs** |   **0.6187 μs** | **0.0339 μs** |  **1.00** |  **0.0992** |      **-** |     **2.6 KB** |        **1.00** |
| EmbedBatch         | 1          |     6.654 μs |   0.4965 μs | 0.0272 μs |  1.05 |  0.1221 |      - |       3 KB |        1.15 |
| EmbedBatchBucketed | 1          |     6.590 μs |   0.1442 μs | 0.0079 μs |  1.04 |  0.1221 |      - |       3 KB |        1.15 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **8**          |    **97.537 μs** |   **5.9846 μs** | **0.3280 μs** |  **1.00** |  **3.7842** | **0.1221** |   **94.76 KB** |        **1.00** |
| EmbedBatch         | 8          |    55.427 μs |   3.4862 μs | 0.1911 μs |  0.57 |  3.5400 | 0.1221 |   87.78 KB |        0.93 |
| EmbedBatchBucketed | 8          |    55.316 μs |   4.7393 μs | 0.2598 μs |  0.57 |  3.5400 | 0.1221 |   87.78 KB |        0.93 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **32**         |   **360.456 μs** |  **30.3934 μs** | **1.6660 μs** |  **1.00** | **13.1836** | **0.4883** |  **334.02 KB** |        **1.00** |
| EmbedBatch         | 32         |   197.431 μs |  64.2147 μs | 3.5198 μs |  0.55 | 12.4512 | 0.7324 |  306.63 KB |        0.92 |
| EmbedBatchBucketed | 32         |   187.176 μs |   9.1493 μs | 0.5015 μs |  0.52 | 11.7188 | 0.7324 |  293.12 KB |        0.88 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **128**        | **1,463.731 μs** | **142.8269 μs** | **7.8288 μs** |  **1.00** | **52.7344** | **1.9531** | **1336.03 KB** |        **1.00** |
| EmbedBatch         | 128        |   778.161 μs | 124.8116 μs | 6.8413 μs |  0.53 | 49.8047 | 6.8359 | 1225.67 KB |        0.92 |
| EmbedBatchBucketed | 128        |   738.580 μs | 127.1169 μs | 6.9677 μs |  0.50 | 46.8750 | 6.8359 | 1158.15 KB |        0.87 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **134.07 ms** | **11.185 ms** | **0.613 ms** |  **1.00** |    **0.01** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  77.40 ms | 12.572 ms | 0.689 ms |  0.58 |    0.01 |  103.71 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **142.95 ms** | **81.033 ms** | **4.442 ms** |  **1.00** |    **0.04** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  70.61 ms |  2.408 ms | 0.132 ms |  0.49 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **200.10 ms** |  **4.095 ms** | **0.224 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 306.24 ms | 24.796 ms | 1.359 ms |  1.53 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **210.83 ms** | **12.306 ms** | **0.675 ms** |  **1.00** |    **0.00** |   **54.56 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 269.53 ms | 12.879 ms | 0.706 ms |  1.28 |    0.00 |  193.09 KB |        3.54 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **252.58 ms** |  **5.211 ms** | **0.286 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 392.08 ms | 48.330 ms | 2.649 ms |  1.55 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **257.68 ms** |  **6.233 ms** | **0.342 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 398.18 ms | 48.292 ms | 2.647 ms |  1.55 |    0.01 |  1153.8 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **296.97 ms** | **12.846 ms** | **0.704 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 471.77 ms | 23.351 ms | 1.280 ms |  1.59 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **299.97 ms** | **51.508 ms** | **2.823 ms** |  **1.00** |    **0.01** | **5514.13 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 459.94 ms | 15.839 ms | 0.868 ms |  1.53 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | length | Mean          | Error        | StdDev     | Allocated |
|------- |------- |--------------:|-------------:|-----------:|----------:|
| **Latin**  | **1000**   |      **55.69 μs** |     **1.648 μs** |   **0.090 μs** |         **-** |
| Cjk    | 1000   |      61.07 μs |     1.983 μs |   0.109 μs |         - |
| **Latin**  | **10000**  |   **5,580.92 μs** |    **93.741 μs** |   **5.138 μs** |         **-** |
| Cjk    | 10000  |   7,582.63 μs | 4,323.408 μs | 236.981 μs |         - |
| **Latin**  | **65536**  | **198,866.23 μs** | **3,694.068 μs** | **202.484 μs** |         **-** |

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

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev  | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|--------:|------:|----------:|----------:|------------:|
| Unigram | 303.7 ms | 51.88 ms | 2.84 ms |  1.00 | 1000.0000 |  30.32 MB |        1.00 |
| Bpe     | 528.1 ms | 22.44 ms | 1.23 ms |  1.74 | 4000.0000 | 112.18 MB |        3.70 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean       | Error     | StdDev  | Gen0   | Allocated |
|-------------------------- |------- |-----------:|----------:|--------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **104.4 μs** |   **2.08 μs** | **0.11 μs** | **0.7324** |  **20.38 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |   **248.6 μs** |   **5.65 μs** | **0.31 μs** | **1.4648** |  **39.93 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |   **525.0 μs** |  **20.28 μs** | **1.11 μs** | **2.9297** |  **78.98 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **1,081.2 μs** | **130.13 μs** | **7.13 μs** | **5.8594** | **157.03 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Alphabet | Mean       | Error     | StdDev    | Allocated |
|----------- |--------- |-----------:|----------:|----------:|----------:|
| **DpGroup**    | **cjk**      |  **17.203 μs** |  **3.004 μs** | **0.1646 μs** |         **-** |
| MyersGroup | cjk      | 242.538 μs | 63.959 μs | 3.5058 μs |         - |
| **DpGroup**    | **latin**    |   **9.605 μs** |  **1.275 μs** | **0.0699 μs** |         **-** |
| MyersGroup | latin    | 134.497 μs |  1.492 μs | 0.0818 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                                    | Mean      | Error     | StdDev   | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       |  31.34 ms |  3.536 ms | 0.194 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 206.45 ms | 28.276 ms | 1.550 ms |  6.59 |    0.06 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  27.09 ms |  4.603 ms | 0.252 ms |  0.86 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Mean         | Error      | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |-------------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
| Ratio          |     90.85 ns |   1.157 ns |  0.063 ns |   1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   | 10,573.49 ns | 152.498 ns |  8.359 ns | 116.39 |    0.11 |      - |         - |          NA |
| TokenSortRatio |  1,026.93 ns | 242.202 ns | 13.276 ns |  11.30 |    0.13 | 0.0515 |    1312 B |          NA |
| TokenSetRatio  |  1,184.75 ns |  69.023 ns |  3.783 ns |  13.04 |    0.04 | 0.0572 |    1448 B |          NA |
| WRatio         |  2,524.09 ns | 295.173 ns | 16.179 ns |  27.78 |    0.16 | 0.1068 |    2760 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Operation     | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |     **90.77 ns** |   **7.704 ns** |  **0.422 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    250.94 ns |  33.692 ns |  1.847 ns |  2.76 |    0.02 | 0.0029 |      80 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  | **10,412.18 ns** |  **28.120 ns** |  **1.541 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,357.23 ns | 762.839 ns | 41.814 ns |  0.99 |    0.00 |      - |     160 B |          NA |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,218.62 ns** | **218.336 ns** | **11.968 ns** |  **1.00** |    **0.01** | **0.0572** |    **1448 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,102.59 ns | 209.841 ns | 11.502 ns |  1.73 |    0.02 | 0.0763 |    1944 B |        1.34 |
|            |               |              |            |           |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **2,483.44 ns** | **245.892 ns** | **13.478 ns** |  **1.00** |    **0.01** | **0.1068** |    **2760 B** |        **1.00** |
| FuzzySharp | WRatio        |  5,203.34 ns | 252.542 ns | 13.843 ns |  2.10 |    0.01 | 0.1221 |    3128 B |        1.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean          | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |--------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |      **27.92 ns** |     **0.137 ns** |   **0.008 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |     140.82 ns |    18.057 ns |   0.990 ns |  5.04 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |      27.78 ns |     0.778 ns |   0.043 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |      26.33 ns |     1.531 ns |   0.084 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **12**     |      **27.31 ns** |     **2.843 ns** |   **0.156 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |     154.77 ns |    46.413 ns |   2.544 ns |  5.67 |    0.09 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |      28.42 ns |     5.351 ns |   0.293 ns |  1.04 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |      27.17 ns |     0.744 ns |   0.041 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **16**     |      **30.11 ns** |     **0.365 ns** |   **0.020 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |     159.49 ns |    43.360 ns |   2.377 ns |  5.30 |    0.07 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |      31.85 ns |     3.724 ns |   0.204 ns |  1.06 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |      29.62 ns |     0.772 ns |   0.042 ns |  0.98 |    0.00 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **20**     |      **33.72 ns** |     **1.879 ns** |   **0.103 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |     221.67 ns |    10.834 ns |   0.594 ns |  6.57 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |      36.54 ns |     1.254 ns |   0.069 ns |  1.08 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |      31.63 ns |     0.501 ns |   0.027 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **24**     |      **50.59 ns** |     **0.771 ns** |   **0.042 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |     666.10 ns |     1.473 ns |   0.081 ns | 13.17 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |      63.98 ns |     2.275 ns |   0.125 ns |  1.26 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |      53.55 ns |     7.869 ns |   0.431 ns |  1.06 |    0.01 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **32**     |      **62.14 ns** |     **3.144 ns** |   **0.172 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |     802.93 ns |   352.690 ns |  19.332 ns | 12.92 |    0.27 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |      62.87 ns |     2.720 ns |   0.149 ns |  1.01 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |      57.40 ns |     2.195 ns |   0.120 ns |  0.92 |    0.00 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |     **888.85 ns** |     **4.491 ns** |   **0.246 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |  18,888.55 ns | 3,229.342 ns | 177.011 ns | 21.25 |    0.17 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |     880.81 ns |    36.337 ns |   1.992 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |     892.00 ns |    46.829 ns |   2.567 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |               |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    |   **7,411.35 ns** |   **135.297 ns** |   **7.416 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 329,193.68 ns | 2,471.974 ns | 135.497 ns | 44.42 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |   7,363.86 ns |   131.676 ns |   7.218 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    |   7,426.96 ns |    82.078 ns |   4.499 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error         | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **123.35 ns** |      **7.507 ns** |   **0.411 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     55.99 ns |      1.483 ns |   0.081 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    124.40 ns |      1.417 ns |   0.078 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |     94.34 ns |      1.683 ns |   0.092 ns |  0.76 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **12**   |    **210.04 ns** |      **1.705 ns** |   **0.093 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     63.70 ns |      1.843 ns |   0.101 ns |  0.30 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    210.59 ns |      2.282 ns |   0.125 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    106.90 ns |      4.433 ns |   0.243 ns |  0.51 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **14**   |    **297.50 ns** |      **4.116 ns** |   **0.226 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 14   |     66.98 ns |      0.036 ns |   0.002 ns |  0.23 |    0.00 |         - |          NA |
| Dp_Cjk     | 14   |    267.07 ns |      5.084 ns |   0.279 ns |  0.90 |    0.00 |         - |          NA |
| Kernel_Cjk | 14   |    114.73 ns |      1.841 ns |   0.101 ns |  0.39 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **16**   |    **379.16 ns** |      **3.063 ns** |   **0.168 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |     71.23 ns |      2.911 ns |   0.160 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    378.57 ns |      1.005 ns |   0.055 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    122.04 ns |      4.928 ns |   0.270 ns |  0.32 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **18**   |    **462.62 ns** |      **2.614 ns** |   **0.143 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 18   |     75.55 ns |      2.263 ns |   0.124 ns |  0.16 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    462.57 ns |      5.099 ns |   0.279 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 18   |    166.42 ns |      1.727 ns |   0.095 ns |  0.36 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **20**   |    **560.00 ns** |      **4.395 ns** |   **0.241 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 20   |     78.73 ns |      0.575 ns |   0.031 ns |  0.14 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    559.32 ns |      7.837 ns |   0.430 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    129.86 ns |      2.037 ns |   0.112 ns |  0.23 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **24**   |    **736.06 ns** |     **61.544 ns** |   **3.373 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 24   |     88.05 ns |      3.815 ns |   0.209 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    768.71 ns |  1,025.464 ns |  56.209 ns |  1.04 |    0.07 |         - |          NA |
| Kernel_Cjk | 24   |    143.42 ns |     22.600 ns |   1.239 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **32**   |  **1,184.71 ns** |     **87.034 ns** |   **4.771 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    103.49 ns |      0.946 ns |   0.052 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,258.52 ns |  2,931.766 ns | 160.700 ns |  1.06 |    0.12 |         - |          NA |
| Kernel_Cjk | 32   |    168.40 ns |      5.535 ns |   0.303 ns |  0.14 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **48**   |  **2,689.91 ns** |  **3,145.449 ns** | **172.413 ns** |  **1.00** |    **0.08** |         **-** |          **NA** |
| Kernel     | 48   |    125.19 ns |      6.256 ns |   0.343 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  2,624.29 ns |    150.995 ns |   8.277 ns |  0.98 |    0.05 |         - |          NA |
| Kernel_Cjk | 48   |    238.15 ns |     55.703 ns |   3.053 ns |  0.09 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **64**   |  **4,628.71 ns** |    **968.101 ns** |  **53.065 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    158.11 ns |      5.078 ns |   0.278 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  4,562.98 ns |     91.276 ns |   5.003 ns |  0.99 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    288.97 ns |      7.207 ns |   0.395 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |               |            |       |         |           |             |
| **Dp**         | **96**   | **10,615.19 ns** | **13,778.420 ns** | **755.242 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Kernel     | 96   |    728.99 ns |      8.978 ns |   0.492 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 10,961.50 ns |    181.886 ns |   9.970 ns |  1.04 |    0.06 |         - |          NA |
| Kernel_Cjk | 96   |    998.67 ns |      9.889 ns |   0.542 ns |  0.09 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Length | Mean         | Error        | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **26.15 ns** |     **0.419 ns** |  **0.023 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    137.11 ns |     3.514 ns |  0.193 ns |  5.24 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     25.92 ns |     0.051 ns |  0.003 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **268.18 ns** |    **71.966 ns** |  **3.945 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 64     |    707.38 ns |   163.662 ns |  8.971 ns |  2.64 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    268.40 ns |     1.450 ns |  0.079 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |              |              |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **15,319.52 ns** |   **367.692 ns** | **20.154 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 18,582.68 ns | 1,582.247 ns | 86.728 ns |  1.21 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 15,254.32 ns |   633.027 ns | 34.698 ns |  1.00 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Length | Distinct | Mean         | Error       | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|------------:|----------:|------:|--------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **371.5 ns** |    **29.61 ns** |   **1.62 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     222.4 ns |    17.36 ns |   0.95 ns |  0.60 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **373.4 ns** |    **78.03 ns** |   **4.28 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     224.4 ns |     7.48 ns |   0.41 ns |  0.60 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **459.6 ns** |    **38.95 ns** |   **2.14 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     309.8 ns |    12.59 ns |   0.69 ns |  0.67 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **455.6 ns** |    **34.94 ns** |   **1.92 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     320.8 ns |    31.32 ns |   1.72 ns |  0.70 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **571.3 ns** |    **35.55 ns** |   **1.95 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     402.4 ns |     1.80 ns |   0.10 ns |  0.70 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **549.6 ns** |    **67.20 ns** |   **3.68 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     396.1 ns |     9.35 ns |   0.51 ns |  0.72 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **637.5 ns** |    **24.79 ns** |   **1.36 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,309.3 ns |   294.87 ns |  16.16 ns |  2.05 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **638.7 ns** |     **4.97 ns** |   **0.27 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,345.4 ns |   145.78 ns |   7.99 ns |  2.11 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,774.0 ns** |    **36.43 ns** |   **2.00 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,626.8 ns |    63.60 ns |   3.49 ns |  2.03 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,801.9 ns** |   **311.60 ns** |  **17.08 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,617.8 ns |   149.34 ns |   8.19 ns |  2.01 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **19,819.1 ns** |   **491.45 ns** |  **26.94 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  63,396.4 ns |   787.42 ns |  43.16 ns |  3.20 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **514,855.6 ns** | **8,951.20 ns** | **490.65 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  63,003.5 ns | 1,907.45 ns | 104.55 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **26.23 ns** |      **0.640 ns** |     **0.035 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      88.46 ns |      3.545 ns |     0.194 ns |  3.37 |    0.01 | 0.0021 |      56 B |          NA |
| Quickenshtein        | 8      |      82.87 ns |      2.037 ns |     0.112 ns |  3.16 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     158.65 ns |     39.113 ns |     2.144 ns |  6.05 |    0.07 | 0.0050 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **262.74 ns** |     **16.638 ns** |     **0.912 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,157.77 ns |  1,970.559 ns |   108.013 ns | 19.63 |    0.36 | 0.0076 |     280 B |          NA |
| Quickenshtein        | 64     |   1,202.43 ns |     27.317 ns |     1.497 ns |  4.58 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   9,223.73 ns |  1,034.099 ns |    56.682 ns | 35.11 |    0.21 | 0.0153 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **15,798.60 ns** |  **1,021.708 ns** |    **56.003 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 441,808.23 ns | 13,530.815 ns |   741.670 ns | 27.97 |    0.10 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  41,774.82 ns |  4,077.699 ns |   223.512 ns |  2.64 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 764,713.80 ns | 64,601.023 ns | 3,541.000 ns | 48.40 |    0.24 |      - |    4160 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method         | Samples | Classes | Mean         | Error       | StdDev     | Gen0   | Allocated |
|--------------- |-------- |-------- |-------------:|------------:|-----------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |     **6.546 μs** |   **0.3343 μs** |  **0.0183 μs** | **0.0076** |     **312 B** |
| MatrixWeighted | 1000    | 2       |     6.499 μs |   0.1998 μs |  0.0109 μs | 0.0076 |     312 B |
| AccuracyScore  | 1000    | 2       |     1.133 μs |   0.0086 μs |  0.0005 μs |      - |         - |
| F1Macro        | 1000    | 2       |     6.722 μs |   0.3689 μs |  0.0202 μs | 0.0153 |     472 B |
| Report         | 1000    | 2       |     9.829 μs |   1.0751 μs |  0.0589 μs | 0.2594 |    6520 B |
| **Matrix**         | **1000**    | **10**      |     **6.778 μs** |   **0.1109 μs** |  **0.0061 μs** | **0.0458** |    **1248 B** |
| MatrixWeighted | 1000    | 10      |     6.768 μs |   0.1579 μs |  0.0087 μs | 0.0458 |    1248 B |
| AccuracyScore  | 1000    | 10      |     1.131 μs |   0.0047 μs |  0.0003 μs |      - |         - |
| F1Macro        | 1000    | 10      |     6.832 μs |   0.1114 μs |  0.0061 μs | 0.0610 |    1664 B |
| Report         | 1000    | 10      |    14.326 μs |   0.9582 μs |  0.0525 μs | 0.6104 |   15496 B |
| **Matrix**         | **100000**  | **2**       |   **773.000 μs** |  **11.1790 μs** |  **0.6128 μs** |      **-** |     **313 B** |
| MatrixWeighted | 100000  | 2       |   752.370 μs | 106.5040 μs |  5.8378 μs |      - |     313 B |
| AccuracyScore  | 100000  | 2       |   180.910 μs |   4.7248 μs |  0.2590 μs |      - |         - |
| F1Macro        | 100000  | 2       |   758.003 μs |  51.7440 μs |  2.8363 μs |      - |     473 B |
| Report         | 100000  | 2       |   760.337 μs |  75.0527 μs |  4.1139 μs |      - |    6545 B |
| **Matrix**         | **100000**  | **10**      |   **891.445 μs** |  **66.9671 μs** |  **3.6707 μs** |      **-** |    **1249 B** |
| MatrixWeighted | 100000  | 10      |   894.282 μs |  16.4581 μs |  0.9021 μs |      - |    1249 B |
| AccuracyScore  | 100000  | 10      |   288.486 μs |  35.7854 μs |  1.9615 μs |      - |         - |
| F1Macro        | 100000  | 10      |   903.516 μs |  17.6735 μs |  0.9687 μs |      - |    1665 B |
| Report         | 100000  | 10      |   905.352 μs |  92.6800 μs |  5.0801 μs |      - |   15841 B |
| **Matrix**         | **1000000** | **2**       | **7,341.356 μs** | **231.7456 μs** | **12.7028 μs** |      **-** |     **318 B** |
| MatrixWeighted | 1000000 | 2       | 7,597.713 μs | 203.1340 μs | 11.1345 μs |      - |     318 B |
| AccuracyScore  | 1000000 | 2       | 1,971.025 μs |  72.5720 μs |  3.9779 μs |      - |         - |
| F1Macro        | 1000000 | 2       | 7,496.454 μs | 213.5727 μs | 11.7066 μs |      - |     478 B |
| Report         | 1000000 | 2       | 7,517.989 μs | 422.9851 μs | 23.1852 μs |      - |    6572 B |
| **Matrix**         | **1000000** | **10**      | **8,588.062 μs** | **818.9821 μs** | **44.8912 μs** |      **-** |    **1260 B** |
| MatrixWeighted | 1000000 | 10      | 8,680.844 μs |  95.9621 μs |  5.2600 μs |      - |    1260 B |
| AccuracyScore  | 1000000 | 10      | 2,999.878 μs |  19.3120 μs |  1.0586 μs |      - |         - |
| F1Macro        | 1000000 | 10      | 9,039.210 μs | 494.6547 μs | 27.1137 μs |      - |    1676 B |
| Report         | 1000000 | 10      | 8,580.519 μs | 333.4195 μs | 18.2759 μs |      - |   15892 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Samples | Request       | Mean         | Error        | StdDev      | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |-------------:|-------------:|------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **9,051.7 μs** |    **554.95 μs** |    **30.42 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |     **1016 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  32,937.6 μs |  4,202.54 μs |   230.36 μs |   3.64 |    0.02 | 625.0000 | 625.0000 | 625.0000 |  5088943 B |    5,008.80 |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |     **253.7 μs** |      **2.09 μs** |     **0.11 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  33,161.5 μs |  9,630.47 μs |   527.88 μs | 130.73 |    1.80 | 687.5000 | 687.5000 | 687.5000 |  5089710 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **155,181.4 μs** | **75,180.60 μs** | **4,120.90 μs** |   **1.00** |    **0.03** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 209,840.7 μs |  5,650.99 μs |   309.75 μs |   1.35 |    0.03 |        - |        - |        - | 23229116 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |   **2,674.0 μs** |     **95.13 μs** |     **5.21 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 207,543.0 μs | 19,604.85 μs | 1,074.61 μs |  77.62 |    0.37 |        - |        - |        - | 23232104 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | Band | Mean         | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **78.66 ns** |      **1.203 ns** |     **0.066 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     76.77 ns |      5.993 ns |     0.329 ns |  0.98 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     78.67 ns |      2.291 ns |     0.126 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     77.15 ns |      0.206 ns |     0.011 ns |  0.98 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **6**    |    **105.67 ns** |      **3.195 ns** |     **0.175 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     75.96 ns |      2.381 ns |     0.130 ns |  0.72 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    106.97 ns |      1.031 ns |     0.057 ns |  1.01 |    0.00 |         - |          NA |
| Kernel_Cjk | 6    |    135.23 ns |     24.084 ns |     1.320 ns |  1.28 |    0.01 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **8**    |    **151.86 ns** |      **3.137 ns** |     **0.172 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     85.95 ns |      2.146 ns |     0.118 ns |  0.57 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    150.40 ns |      4.719 ns |     0.259 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    178.23 ns |     13.426 ns |     0.736 ns |  1.17 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **10**   |    **196.85 ns** |      **4.486 ns** |     **0.246 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |     97.82 ns |      1.342 ns |     0.074 ns |  0.50 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    196.78 ns |      7.539 ns |     0.413 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    137.65 ns |      1.375 ns |     0.075 ns |  0.70 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **12**   |    **278.78 ns** |     **47.134 ns** |     **2.584 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    102.30 ns |      0.797 ns |     0.044 ns |  0.37 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    277.66 ns |      3.890 ns |     0.213 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    150.02 ns |      1.073 ns |     0.059 ns |  0.54 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **16**   |    **419.63 ns** |      **4.234 ns** |     **0.232 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    121.36 ns |      5.471 ns |     0.300 ns |  0.29 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    420.30 ns |     36.801 ns |     2.017 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    168.37 ns |      3.632 ns |     0.199 ns |  0.40 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **24**   |    **818.03 ns** |     **12.445 ns** |     **0.682 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    154.70 ns |      1.514 ns |     0.083 ns |  0.19 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    814.67 ns |     15.180 ns |     0.832 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    216.89 ns |      1.743 ns |     0.096 ns |  0.27 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **32**   |  **1,426.61 ns** |     **94.836 ns** |     **5.198 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    191.54 ns |      6.149 ns |     0.337 ns |  0.13 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,424.14 ns |     15.749 ns |     0.863 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    257.47 ns |      0.518 ns |     0.028 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **48**   |  **3,118.45 ns** |     **94.822 ns** |     **5.198 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    262.00 ns |      4.623 ns |     0.253 ns |  0.08 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,115.97 ns |    130.049 ns |     7.128 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    342.84 ns |     16.759 ns |     0.919 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **64**   |  **5,476.83 ns** |    **116.026 ns** |     **6.360 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    331.68 ns |      9.236 ns |     0.506 ns |  0.06 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,507.96 ns |    746.424 ns |    40.914 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    431.04 ns |      9.173 ns |     0.503 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |               |              |       |         |           |             |
| **Dp**         | **96**   | **13,109.98 ns** | **26,203.902 ns** | **1,436.324 ns** |  **1.01** |    **0.13** |         **-** |          **NA** |
| Kernel     | 96   |  1,247.44 ns |    150.346 ns |     8.241 ns |  0.10 |    0.01 |         - |          NA |
| Dp_Cjk     | 96   | 12,242.44 ns |    618.941 ns |    33.926 ns |  0.94 |    0.08 |         - |          NA |
| Kernel_Cjk | 96   |  1,574.22 ns |      8.258 ns |     0.453 ns |  0.12 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| VocabTxt               |  4.015 ms | 0.3370 ms | 0.0185 ms |  85.9375 |  78.1250 |  31.2500 |   3.62 MB |
| TokenizerJsonWordPiece | 11.465 ms | 2.4560 ms | 0.1346 ms | 125.0000 | 109.3750 |  31.2500 |   5.72 MB |
| TokenizerJsonUnigram   | 11.888 ms | 1.0983 ms | 0.0602 ms |  78.1250 |  62.5000 |  31.2500 |   4.64 MB |
| SpieceModel            |  3.717 ms | 1.1440 ms | 0.0627 ms |  89.8438 |  85.9375 |  35.1563 |   3.36 MB |
| TfidfSave              |  1.875 ms | 0.2953 ms | 0.0162 ms |  33.2031 |  29.2969 |  29.2969 |   2.09 MB |
| TfidfLoad              |  4.295 ms | 0.2567 ms | 0.0141 ms |  62.5000 |  54.6875 |  23.4375 |   2.86 MB |
| EmbeddingIndexSave     |  5.260 ms | 1.3291 ms | 0.0729 ms | 273.4375 | 273.4375 | 273.4375 |  19.87 MB |
| EmbeddingIndexLoad     |  8.043 ms | 3.9356 ms | 0.2157 ms | 156.2500 | 125.0000 | 125.0000 |  15.72 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Shape   | Mean         | Error      | StdDev    | Ratio | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------------- |-------- |-------------:|-----------:|----------:|------:|---------:|---------:|---------:|----------:|------------:|
| **Lodestar_ExplainedVariance** | **100x200** | **15,095.60 μs** |  **48.669 μs** |  **2.668 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.28 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 18,132.11 μs | 922.056 μs | 50.541 μs |  1.20 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **153.66 μs** |   **3.704 μs** |  **0.203 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    209.50 μs |  11.199 μs |  0.614 μs |  1.36 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,545.36 μs** | **188.579 μs** | **10.337 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  3,181.30 μs | 338.609 μs | 18.560 μs |  0.90 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **26.28 μs** |   **0.388 μs** |  **0.021 μs** |  **1.00** |   **0.0916** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     28.25 μs |   1.072 μs |  0.059 μs |  1.08 |   0.0610 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Documents | Permutations | Mean        | Error     | StdDev   | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|----------------- |---------- |------------- |------------:|----------:|---------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**    | **500**       | **64**           |    **59.76 ms** |  **4.146 ms** | **0.227 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify | 500       | 64           |    13.05 ms |  1.285 ms | 0.070 ms |  0.22 |  156.2500 |  140.6250 |  78.1250 |   2699.49 KB |        0.33 |
| SignaturesOnly   | 500       | 64           |    10.34 ms |  0.707 ms | 0.039 ms |  0.17 |   15.6250 |         - |        - |    511.75 KB |        0.06 |
| FingerprintsOnly | 500       | 64           |    11.21 ms |  1.317 ms | 0.072 ms |  0.19 |   15.6250 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |             |           |          |       |           |           |          |              |             |
| **ExactPairwise**    | **500**       | **128**          |    **59.96 ms** |  **3.536 ms** | **0.194 ms** |  **1.00** |  **250.0000** |         **-** |        **-** |   **8152.43 KB** |        **1.00** |
| SketchThenVerify | 500       | 128          |    16.72 ms |  1.543 ms | 0.085 ms |  0.28 |  218.7500 |  218.7500 | 218.7500 |   4745.26 KB |        0.58 |
| SignaturesOnly   | 500       | 128          |    13.01 ms |  0.331 ms | 0.018 ms |  0.22 |   15.6250 |         - |        - |    636.75 KB |        0.08 |
| FingerprintsOnly | 500       | 128          |    11.29 ms |  0.831 ms | 0.046 ms |  0.19 |   15.6250 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |             |           |          |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **64**           | **1,019.74 ms** | **29.146 ms** | **1.598 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify | 2000      | 64           |    51.46 ms |  4.156 ms | 0.228 ms |  0.05 |  800.0000 |  800.0000 | 500.0000 |  10993.92 KB |        0.09 |
| SignaturesOnly   | 2000      | 64           |    41.78 ms |  1.271 ms | 0.070 ms |  0.04 |   83.3333 |         - |        - |   2046.96 KB |        0.02 |
| FingerprintsOnly | 2000      | 64           |    44.50 ms |  1.418 ms | 0.078 ms |  0.04 |   83.3333 |         - |        - |   2609.43 KB |        0.02 |
|                  |           |              |             |           |          |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **128**          | **1,022.17 ms** | **32.851 ms** | **1.801 ms** |  **1.00** | **5000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify | 2000      | 128          |    73.79 ms | 24.631 ms | 1.350 ms |  0.07 | 1285.7143 | 1285.7143 | 714.2857 |  19323.92 KB |        0.15 |
| SignaturesOnly   | 2000      | 128          |    52.87 ms |  1.636 ms | 0.090 ms |  0.05 |  100.0000 |         - |        - |   2546.97 KB |        0.02 |
| FingerprintsOnly | 2000      | 128          |    44.64 ms |  2.104 ms | 0.115 ms |  0.04 |   83.3333 |         - |        - |   2609.43 KB |        0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method               | Documents | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0      | Gen1     | Gen2     | Allocated | Alloc Ratio |
|--------------------- |---------- |----------:|-----------:|----------:|------:|--------:|----------:|---------:|---------:|----------:|------------:|
| **Count**                | **200**       |  **7.642 ms** |  **1.7313 ms** | **0.0949 ms** |  **1.00** |    **0.02** |  **343.7500** | **156.2500** |  **62.5000** |   **7.92 MB** |        **1.00** |
| CountWithStopWords   | 200       |  6.271 ms |  0.8622 ms | 0.0473 ms |  0.82 |    0.01 |  257.8125 | 125.0000 |        - |   6.34 MB |        0.80 |
| Hashing              | 200       |  7.518 ms |  1.5745 ms | 0.0863 ms |  0.98 |    0.01 |  359.3750 | 140.6250 |  70.3125 |   8.08 MB |        1.02 |
| HashingWithStopWords | 200       |  6.377 ms |  0.4883 ms | 0.0268 ms |  0.83 |    0.01 |  265.6250 |  93.7500 |        - |   6.52 MB |        0.82 |
|                      |           |           |            |           |       |         |           |          |          |           |             |
| **Count**                | **1000**      | **31.107 ms** | **10.8206 ms** | **0.5931 ms** |  **1.00** |    **0.02** | **1687.5000** | **812.5000** | **562.5000** |  **38.64 MB** |        **1.00** |
| CountWithStopWords   | 1000      | 24.431 ms | 10.5877 ms | 0.5803 ms |  0.79 |    0.02 | 1406.2500 | 562.5000 | 250.0000 |  30.99 MB |        0.80 |
| Hashing              | 1000      | 29.677 ms |  4.6470 ms | 0.2547 ms |  0.95 |    0.02 | 1968.7500 | 593.7500 | 593.7500 |  39.55 MB |        1.02 |
| HashingWithStopWords | 1000      | 25.142 ms |  8.7121 ms | 0.4775 ms |  0.81 |    0.02 | 1343.7500 | 343.7500 | 250.0000 |  31.83 MB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **65.09 ms** | **16.617 ms** | **0.911 ms** |  **1.00** |    **0.02** | **2750.0000** |  **68.25 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  52.25 ms |  2.498 ms | 0.137 ms |  0.80 |    0.01 |  100.0000 |   3.55 MB |        0.05 |
|              |               |           |           |          |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** | **319.24 ms** | **23.137 ms** | **1.268 ms** |  **1.00** |    **0.00** | **1000.0000** |  **30.33 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  49.57 ms |  2.775 ms | 0.152 ms |  0.16 |    0.00 |   90.9091 |   3.09 MB |        0.10 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Dim  | Mean      | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|----------:|------------:|
| **Dot**    | **384**  |  **51.45 ns** | **0.520 ns** | **0.028 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  52.00 ns | 0.885 ns | 0.049 ns |  1.01 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  | **103.87 ns** | **3.217 ns** | **0.176 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  | 100.26 ns | 0.317 ns | 0.017 ns |  0.97 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **140.11 ns** | **3.710 ns** | **0.203 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 136.43 ns | 2.380 ns | 0.130 ns |  0.97 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Count**        | **200**       |  **2.950 ms** | **1.8716 ms** | **0.1026 ms** |  **1.00** |    **0.04** |  **62.5000** |  **23.4375** |        **-** |    **1.6 MB** |        **1.00** |
| Tfidf        | 200       |  3.153 ms | 9.2335 ms | 0.5061 ms |  1.07 |    0.15 |  62.5000 |  23.4375 |        - |   1.63 MB |        1.02 |
| CountBigrams | 200       |  3.784 ms | 0.7788 ms | 0.0427 ms |  1.28 |    0.04 | 109.3750 |  62.5000 |        - |   2.78 MB |        1.74 |
| Hashing      | 200       |  2.833 ms | 0.3295 ms | 0.0181 ms |  0.96 |    0.03 |  66.4063 |  31.2500 |        - |    1.6 MB |        1.00 |
|              |           |           |           |           |       |         |          |          |          |           |             |
| **Count**        | **1000**      |  **7.088 ms** | **1.2877 ms** | **0.0706 ms** |  **1.00** |    **0.01** | **343.7500** | **203.1250** |  **62.5000** |   **7.83 MB** |        **1.00** |
| Tfidf        | 1000      |  7.348 ms | 2.2694 ms | 0.1244 ms |  1.04 |    0.02 | 343.7500 | 234.3750 |  93.7500 |   7.97 MB |        1.02 |
| CountBigrams | 1000      | 11.924 ms | 3.4504 ms | 0.1891 ms |  1.68 |    0.03 | 640.6250 | 265.6250 | 265.6250 |  13.42 MB |        1.71 |
| Hashing      | 1000      |  7.102 ms | 0.9145 ms | 0.0501 ms |  1.00 |    0.01 | 351.5625 | 140.6250 |  70.3125 |   7.85 MB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method   | Documents | Mean       | Error      | StdDev     | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |---------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **Lodestar** | **200**       |   **8.100 ms** |   **2.401 ms** |  **0.1316 ms** |  **1.00** |    **0.02** |   **296.8750** |   **218.7500** |   **140.6250** |   **5.13 MB** |        **1.00** |
| MlNet    | 200       |  67.099 ms | 352.845 ms | 19.3406 ms |  8.29 |    2.07 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |        5.52 |
|          |           |            |            |            |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **30.609 ms** |  **13.315 ms** |  **0.7298 ms** |  **1.00** |    **0.03** |  **2281.2500** |  **2250.0000** |  **1500.0000** |  **24.92 MB** |        **1.00** |
| MlNet    | 1000      | 341.517 ms | 329.883 ms | 18.0820 ms | 11.16 |    0.56 | 72000.0000 | 72000.0000 | 72000.0000 | 324.34 MB |       13.01 |

<!-- markdownlint-enable MD060 -->

### compare-indel

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 103.4 | 21.9 | 4.72x C# faster |
| latin | 32 | 146.5 | 87.4 | 1.68x C# faster |
| latin | 128 | 473.1 | 806.4 | 1.70x Py faster |
| latin | 512 | 4788.8 | 7487.5 | 1.56x Py faster |
| cjk | 8 | 119.8 | 21.8 | 5.49x C# faster |
| cjk | 32 | 272.0 | 216.7 | 1.26x C# faster |
| cjk | 128 | 1791.3 | 1577.1 | 1.14x C# faster |
| cjk | 512 | 16252.3 | 10638.7 | 1.53x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 128.3 | 19.3 | 6.65x C# faster |
| latin | 32 | 224.8 | 152.7 | 1.47x C# faster |
| latin | 128 | 1838.2 | 1665.9 | 1.10x C# faster |
| latin | 512 | 15602.7 | 16735.6 | 1.07x Py faster |
| cjk | 8 | 136.5 | 19.1 | 7.16x C# faster |
| cjk | 32 | 351.0 | 285.8 | 1.23x C# faster |
| cjk | 128 | 3017.6 | 2485.7 | 1.21x C# faster |
| cjk | 512 | 26349.1 | 21060.4 | 1.25x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.007 | 0.818 | 113.22x | 0.007 | 0.817 | 113.22x |
| accuracy_n1000_k2 | 0.001 | 0.427 | 374.39x | 0.001 | 0.427 | 374.38x |
| precision_recall_f1_macro_n1000_k2 | 0.007 | 1.485 | 227.13x | 0.007 | 1.485 | 227.14x |
| classification_report_n1000_k2 | 0.009 | 5.660 | 596.66x | 0.009 | 5.660 | 596.64x |
| roc_auc_binary_n1000_k2 | 0.015 | 1.637 | 108.01x | 0.015 | 1.637 | 107.72x |
| balanced_accuracy_n1000_k2 | 0.006 | 0.867 | 134.44x | 0.006 | 0.867 | 134.44x |
| matthews_n1000_k2 | 0.006 | 1.642 | 255.07x | 0.006 | 1.642 | 255.07x |
| cohen_kappa_n1000_k2 | 0.006 | 0.913 | 141.18x | 0.006 | 0.913 | 141.18x |
| mse_n1000_k2 | 0.003 | 0.231 | 87.33x | 0.003 | 0.231 | 87.33x |
| mae_n1000_k2 | 0.003 | 0.231 | 86.52x | 0.003 | 0.231 | 86.52x |
| median_ae_n1000_k2 | 0.007 | 0.244 | 36.67x | 0.007 | 0.244 | 36.67x |
| r2_n1000_k2 | 0.003 | 0.280 | 82.26x | 0.003 | 0.280 | 82.26x |
| confusion_matrix_n1000_k10 | 0.007 | 0.814 | 111.80x | 0.007 | 0.814 | 111.80x |
| accuracy_n1000_k10 | 0.001 | 0.427 | 374.33x | 0.001 | 0.427 | 374.34x |
| precision_recall_f1_macro_n1000_k10 | 0.007 | 1.501 | 212.51x | 0.007 | 1.501 | 212.51x |
| classification_report_n1000_k10 | 0.014 | 5.817 | 412.01x | 0.014 | 5.817 | 412.03x |
| roc_auc_ovr_macro_n1000_k10 | 0.522 | 8.322 | 15.94x | 0.522 | 8.322 | 15.94x |
| balanced_accuracy_n1000_k10 | 0.007 | 0.881 | 128.34x | 0.007 | 0.881 | 128.34x |
| matthews_n1000_k10 | 0.007 | 1.665 | 243.78x | 0.007 | 1.665 | 243.78x |
| cohen_kappa_n1000_k10 | 0.007 | 0.917 | 126.69x | 0.007 | 0.917 | 126.69x |
| mse_n1000_k10 | 0.003 | 0.231 | 86.50x | 0.003 | 0.231 | 86.51x |
| mae_n1000_k10 | 0.003 | 0.230 | 85.14x | 0.003 | 0.230 | 85.14x |
| median_ae_n1000_k10 | 0.007 | 0.245 | 36.83x | 0.007 | 0.245 | 36.83x |
| r2_n1000_k10 | 0.003 | 0.284 | 82.82x | 0.003 | 0.284 | 82.82x |
| confusion_matrix_n100000_k2 | 0.867 | 10.900 | 12.57x | 0.867 | 10.899 | 12.57x |
| accuracy_n100000_k2 | 0.176 | 3.769 | 21.40x | 0.176 | 3.769 | 21.40x |
| precision_recall_f1_macro_n100000_k2 | 0.733 | 12.394 | 16.92x | 0.733 | 12.393 | 16.92x |
| classification_report_n100000_k2 | 0.738 | 26.780 | 36.27x | 0.738 | 26.778 | 36.27x |
| roc_auc_binary_n100000_k2 | 4.120 | 25.363 | 6.16x | 4.119 | 25.361 | 6.16x |
| balanced_accuracy_n100000_k2 | 0.731 | 10.977 | 15.02x | 0.731 | 10.977 | 15.02x |
| matthews_n100000_k2 | 0.737 | 21.858 | 29.67x | 0.737 | 21.858 | 29.68x |
| cohen_kappa_n100000_k2 | 0.731 | 11.002 | 15.04x | 0.731 | 11.001 | 15.04x |
| mse_n100000_k2 | 0.287 | 0.533 | 1.86x | 0.287 | 0.533 | 1.86x |
| mae_n100000_k2 | 0.284 | 0.526 | 1.85x | 0.284 | 0.526 | 1.85x |
| median_ae_n100000_k2 | 0.767 | 1.698 | 2.21x | 0.780 | 1.698 | 2.18x |
| r2_n100000_k2 | 0.335 | 0.882 | 2.63x | 0.335 | 0.881 | 2.63x |
| confusion_matrix_n100000_k10 | 0.842 | 10.914 | 12.96x | 0.842 | 10.914 | 12.96x |
| accuracy_n100000_k10 | 0.277 | 3.780 | 13.64x | 0.277 | 3.780 | 13.64x |
| precision_recall_f1_macro_n100000_k10 | 0.845 | 12.990 | 15.37x | 0.845 | 12.989 | 15.37x |
| classification_report_n100000_k10 | 0.850 | 29.247 | 34.42x | 0.850 | 29.244 | 34.42x |
| roc_auc_ovr_macro_n100000_k10 | 42.820 | 201.648 | 4.71x | 42.816 | 201.630 | 4.71x |
| balanced_accuracy_n100000_k10 | 0.838 | 11.012 | 13.14x | 0.838 | 11.011 | 13.13x |
| matthews_n100000_k10 | 0.836 | 22.492 | 26.91x | 0.836 | 22.491 | 26.91x |
| cohen_kappa_n100000_k10 | 1.003 | 11.007 | 10.98x | 1.002 | 11.006 | 10.98x |
| mse_n100000_k10 | 0.286 | 0.536 | 1.88x | 0.286 | 0.536 | 1.88x |
| mae_n100000_k10 | 0.283 | 0.525 | 1.85x | 0.283 | 0.524 | 1.85x |
| median_ae_n100000_k10 | 0.841 | 1.697 | 2.02x | 0.906 | 1.697 | 1.87x |
| r2_n100000_k10 | 0.337 | 0.818 | 2.42x | 0.337 | 0.818 | 2.42x |
| confusion_matrix_n1000000_k2 | 7.374 | 102.408 | 13.89x | 7.373 | 102.378 | 13.88x |
| accuracy_n1000000_k2 | 1.931 | 33.569 | 17.39x | 1.931 | 33.568 | 17.39x |
| precision_recall_f1_macro_n1000000_k2 | 7.408 | 111.009 | 14.98x | 7.408 | 111.005 | 14.98x |
| classification_report_n1000000_k2 | 7.305 | 218.106 | 29.86x | 7.304 | 218.096 | 29.86x |
| roc_auc_binary_n1000000_k2 | 62.896 | 280.093 | 4.45x | 62.889 | 280.092 | 4.45x |
| balanced_accuracy_n1000000_k2 | 7.304 | 101.502 | 13.90x | 7.304 | 101.492 | 13.90x |
| matthews_n1000000_k2 | 7.348 | 206.646 | 28.12x | 7.348 | 206.635 | 28.12x |
| cohen_kappa_n1000000_k2 | 7.358 | 101.830 | 13.84x | 7.358 | 101.820 | 13.84x |
| mse_n1000000_k2 | 2.867 | 2.516 | 0.88x | 2.867 | 2.516 | 0.88x |
| mae_n1000000_k2 | 2.828 | 2.555 | 0.90x | 2.828 | 2.555 | 0.90x |
| median_ae_n1000000_k2 | 7.896 | 13.584 | 1.72x | 7.948 | 13.584 | 1.71x |
| r2_n1000000_k2 | 3.315 | 5.189 | 1.57x | 3.315 | 5.188 | 1.57x |
| confusion_matrix_n1000000_k10 | 8.476 | 101.947 | 12.03x | 8.475 | 101.935 | 12.03x |
| accuracy_n1000000_k10 | 2.952 | 33.611 | 11.38x | 2.952 | 33.607 | 11.38x |
| precision_recall_f1_macro_n1000000_k10 | 8.554 | 115.865 | 13.55x | 8.553 | 115.806 | 13.54x |
| classification_report_n1000000_k10 | 8.418 | 237.712 | 28.24x | 8.418 | 237.684 | 28.24x |
| balanced_accuracy_n1000000_k10 | 8.451 | 102.040 | 12.07x | 8.451 | 102.030 | 12.07x |
| matthews_n1000000_k10 | 8.468 | 210.763 | 24.89x | 8.467 | 210.738 | 24.89x |
| cohen_kappa_n1000000_k10 | 8.421 | 101.751 | 12.08x | 8.420 | 101.746 | 12.08x |
| mse_n1000000_k10 | 2.669 | 2.415 | 0.90x | 2.669 | 2.415 | 0.90x |
| mae_n1000000_k10 | 2.696 | 2.432 | 0.90x | 2.696 | 2.432 | 0.90x |
| median_ae_n1000000_k10 | 7.492 | 13.740 | 1.83x | 7.547 | 13.734 | 1.82x |
| r2_n1000000_k10 | 3.986 | 5.753 | 1.44x | 3.985 | 5.752 | 1.44x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

BELOW GATE on processor time:
  mse_n1000000_k2                  0.88x
  mae_n1000000_k2                  0.90x
  mse_n1000000_k10                 0.90x
  mae_n1000000_k10                 0.90x

<!-- markdownlint-enable MD060 -->

### compare-ols

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.300 | 2.508 | 8.37x | 0.300 | 2.507 | 8.37x |
| ols_summary_n10000 | 2.911 | 10.318 | 3.54x | 3.018 | 10.317 | 3.42x |
| ols_summary_n100000 | 32.686 | 129.930 | 3.98x | 33.068 | 518.735 | 15.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.365 | 10.099 | 2.31x | 4.563 | 10.098 | 2.21x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 11.529 | 15.548 | 1.35x | 11.708 | 15.547 | 1.33x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.686 | 41.537 | 3.27x | 13.035 | 41.535 | 3.19x | 1,990,038 | 1,990,038 |
| spiece_model | 4.446 | 28.916 | 6.50x | 4.604 | 28.915 | 6.28x | 533,084 | 533,084 |
| tfidf_save | 1.814 | 2.476 | 1.36x | 1.849 | 2.475 | 1.34x | 581,787 | 591,922 |
| tfidf_load | 4.662 | 3.881 | 0.83x | 4.842 | 3.881 | 0.80x | 581,787 | 591,922 |
| embedding_index_save | 5.224 | 2.394 | 0.46x | 5.464 | 2.394 | 0.44x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.292 | 36.444 | 0.72x | 10.804 | 5.165 | 0.48x | 20,589,007 | 15,360,128 |
| embedding_index_load | 7.791 | 1.477 | 0.19x | 8.313 | 1.477 | 0.18x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 8.803 | 1.177 | 0.13x | 9.138 | 1.176 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 5.434 | 1.571 | 0.29x | 5.848 | 1.571 | 0.27x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 2.143 | 1.554 | 0.72x | 2.524 | 1.554 | 0.62x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 105.86x | 0.000 | 0.001 | 105.86x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 425.575 | 567.426 | 1.33x | 425.525 | 567.372 | 1.33x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 81.077 | 71.849 | 0.89x | 82.517 | 71.842 | 0.87x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-stats

_As of 2026-09-13, measured at commit `cfe9f048c42ed96c9098969f70db7f8f5e86b396`._

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.005 | 0.654 | 135.31x | 0.005 | 0.654 | 135.29x |
| mann_whitney_n1000 | 0.245 | 0.641 | 2.62x | 0.245 | 0.641 | 2.62x |
| chi_square_n1000 | 0.000 | 0.259 | 1109.17x | 0.000 | 0.259 | 1109.09x |
| welch_t_n10000 | 0.046 | 0.716 | 15.47x | 0.046 | 0.715 | 15.47x |
| mann_whitney_n10000 | 3.826 | 2.458 | 0.64x | 3.831 | 2.458 | 0.64x |
| chi_square_n10000 | 0.001 | 0.262 | 248.10x | 0.001 | 0.262 | 248.11x |
| welch_t_n100000 | 0.463 | 1.415 | 3.06x | 0.463 | 1.415 | 3.06x |
| mann_whitney_n100000 | 45.822 | 23.810 | 0.52x | 46.060 | 23.808 | 0.52x |
| chi_square_n100000 | 0.011 | 0.280 | 24.78x | 0.011 | 0.280 | 24.77x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->
