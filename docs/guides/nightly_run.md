# Nightly benchmark run

<!-- nightly-baseline: afc1909d1a582511899e1e1cb204d26c573d6fef -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks ChainedProductBenchmarks DistributionTailBenchmarks GlmBenchmarks OlsBenchmarks QuantileBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks StatsBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks VectorMathBenchmarks VectorizerBenchmarks VectorizerIncumbentBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `afc1909d1a582511899e1e1cb204d26c573d6fef`
- Previous run: `afc1909d1a582511899e1e1cb204d26c573d6fef`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `BatchEmbeddingBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `BkTreeBenchmarks`
- `BlockedTableBenchmarks`
- `Bm25Benchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `BucketRouteDiagnostics`
- `ChainedProductBenchmarks`
- `DecompositionBenchmarks`
- `DistributionTailBenchmarks`
- `FuzzBenchmarks`
- `FuzzIncumbentBenchmarks`
- `GlmBenchmarks`
- `IndelBenchmarks`
- `LcsGateBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `LevenshteinIncumbentBenchmarks`
- `MetricsBenchmarks`
- `MetricsIncumbentBenchmarks`
- `MyersGateBenchmarks`
- `OlsBenchmarks`
- `PersistenceBenchmarks`
- `PrincipalComponentVarianceBenchmarks`
- `QuantileBenchmarks`
- `RegressionMetricsBenchmarks`
- `RobustCovarianceBenchmarks`
- `SerialCorrelationBenchmarks`
- `SimilaritySketchBenchmarks`
- `StatsBenchmarks`
- `StopWordBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `VectorizerIncumbentBenchmarks`

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean         | Error      | StdDev    | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |     **5.366 μs** |  **0.1421 μs** | **0.0078 μs** |  **1.00** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |     5.863 μs |  0.4703 μs | 0.0258 μs |  1.09 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |     5.761 μs |  0.1804 μs | 0.0099 μs |  1.07 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |              |            |           |       |         |        |           |             |
| **UnitLoop**           | **8**          |    **71.060 μs** |  **0.3842 μs** | **0.0211 μs** |  **1.00** |  **1.7090** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |    36.118 μs |  0.8636 μs | 0.0473 μs |  0.51 |  1.3428 |      - |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |    37.704 μs |  5.7970 μs | 0.3178 μs |  0.53 |  1.3428 |      - |  22.19 KB |        0.76 |
|                    |            |              |            |           |       |         |        |           |             |
| **UnitLoop**           | **32**         |   **261.095 μs** |  **4.1355 μs** | **0.2267 μs** |  **1.00** |  **6.3477** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |   128.097 μs |  2.5907 μs | 0.1420 μs |  0.49 |  4.8828 |      - |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |   116.944 μs |  6.1944 μs | 0.3395 μs |  0.45 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |              |            |           |       |         |        |           |             |
| **UnitLoop**           | **128**        | **1,061.427 μs** | **81.4755 μs** | **4.4659 μs** |  **1.00** | **25.3906** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        |   511.083 μs | 15.1474 μs | 0.8303 μs |  0.48 | 19.5313 | 1.9531 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        |   456.134 μs | 12.5023 μs | 0.6853 μs |  0.43 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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
| **LengthFilteredScan** | **1**      | **clustered** | **146.15 ms** |  **9.843 ms** | **0.540 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  72.86 ms |  4.021 ms | 0.220 ms |  0.50 |    0.00 |  103.71 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **153.51 ms** | **62.309 ms** | **3.415 ms** |  **1.00** |    **0.03** |   **23.79 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  68.53 ms |  2.800 ms | 0.153 ms |  0.45 |    0.01 |  116.49 KB |        4.90 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **219.12 ms** |  **3.412 ms** | **0.187 ms** |  **1.00** |    **0.00** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 283.10 ms | 19.504 ms | 1.069 ms |  1.29 |    0.00 |  259.28 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **224.45 ms** |  **1.522 ms** | **0.083 ms** |  **1.00** |    **0.00** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 240.53 ms |  5.291 ms | 0.290 ms |  1.07 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **275.80 ms** | **14.509 ms** | **0.795 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 383.83 ms | 25.756 ms | 1.412 ms |  1.39 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **280.17 ms** | **12.406 ms** | **0.680 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 359.88 ms | 31.999 ms | 1.754 ms |  1.28 |    0.01 |  1153.8 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **321.86 ms** | **21.333 ms** | **1.169 ms** |  **1.00** |    **0.00** | **5113.56 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 448.47 ms | 26.283 ms | 1.441 ms |  1.39 |    0.01 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **327.21 ms** | **33.350 ms** | **1.828 ms** |  **1.00** |    **0.01** | **5513.97 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 445.61 ms |  7.884 ms | 0.432 ms |  1.36 |    0.01 |  7964.5 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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
| **Latin**  | **1000**   |      **55.70 μs** |      **0.644 μs** |     **0.035 μs** |         **-** |
| Cjk    | 1000   |      64.19 μs |      0.227 μs |     0.012 μs |         - |
| **Latin**  | **10000**  |   **5,489.98 μs** |    **209.968 μs** |    **11.509 μs** |         **-** |
| Cjk    | 10000  |   7,005.09 μs |    154.066 μs |     8.445 μs |         - |
| **Latin**  | **65536**  | **229,132.19 μs** | **19,950.727 μs** | **1,093.567 μs** |         **-** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.Bm25Benchmarks-report-github

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

| Method           | Documents | Mean           | Error          | StdDev        | Ratio  | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|----------------- |---------- |---------------:|---------------:|--------------:|-------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| **LodestarQuery**    | **1000**      |      **28.661 μs** |      **0.6563 μs** |     **0.0360 μs** |   **1.00** |    **0.00** |     **0.7324** |         **-** |         **-** |     **12.27 KB** |        **1.00** |
| LuceneQuery      | 1000      |       3.310 μs |      0.3582 μs |     0.0196 μs |   0.12 |    0.00 |     0.3128 |         - |         - |      5.14 KB |        0.42 |
| LodestarFromText | 1000      |  14,477.590 μs |  1,249.1029 μs |    68.4675 μs | 505.13 |    2.14 |  1671.8750 | 1312.5000 |  500.0000 |  21362.14 KB |    1,741.63 |
| LuceneFromText   | 1000      |   7,367.723 μs |    566.6423 μs |    31.0596 μs | 257.06 |    0.98 |    78.1250 |   62.5000 |         - |   1342.82 KB |      109.48 |
|                  |           |                |                |               |        |         |            |           |           |              |             |
| **LodestarQuery**    | **20000**     |   **1,383.638 μs** |     **57.4039 μs** |     **3.1465 μs** |   **1.00** |    **0.00** |     **5.8594** |    **1.9531** |    **1.9531** |    **234.94 KB** |        **1.00** |
| LuceneQuery      | 20000     |      18.114 μs |      1.5352 μs |     0.0841 μs |   0.01 |    0.00 |     0.4883 |         - |         - |      8.45 KB |        0.04 |
| LodestarFromText | 20000     | 287,242.858 μs | 46,465.8421 μs | 2,546.9496 μs | 207.60 |    1.65 | 24000.0000 | 6000.0000 | 1000.0000 | 418590.77 KB |    1,781.66 |
| LuceneFromText   | 20000     | 145,185.525 μs | 67,486.1296 μs | 3,699.1425 μs | 104.93 |    2.32 |  1250.0000 | 1000.0000 |         - |  21891.82 KB |       93.18 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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

| Method  | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|-------- |----------:|----------:|---------:|------:|--------:|----------:|----------:|------------:|
| Unigram |  32.11 ms |  4.083 ms | 0.224 ms |  1.00 |    0.01 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 562.77 ms | 33.381 ms | 1.830 ms | 17.53 |    0.12 | 7000.0000 | 112.18 MB |       20.64 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean       | Error    | StdDev  | Gen0   | Allocated |
|-------------------------- |------- |-----------:|---------:|--------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **103.5 μs** |  **5.01 μs** | **0.27 μs** | **1.2207** |  **20.38 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |   **209.1 μs** | **11.17 μs** | **0.61 μs** | **2.4414** |  **39.93 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |   **481.2 μs** | **29.36 μs** | **1.61 μs** | **4.3945** |  **78.98 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **1,030.0 μs** | **86.60 μs** | **4.75 μs** | **7.8125** | **157.03 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean      | Error    | StdDev   | Allocated |
|----------- |--------- |----------:|---------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **18.86 μs** | **1.086 μs** | **0.060 μs** |         **-** |
| MyersGroup | cjk      | 169.60 μs | 5.860 μs | 0.321 μs |         - |
| **DpGroup**    | **latin**    |  **10.57 μs** | **0.192 μs** | **0.011 μs** |         **-** |
| MyersGroup | latin    | 113.09 μs | 0.404 μs | 0.022 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean      | Error     | StdDev   | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       |  29.38 ms |  4.594 ms | 0.252 ms |  1.00 |    0.01 |
| Nmf_Rank20                                | 205.85 ms | 32.172 ms | 1.763 ms |  7.01 |    0.07 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  26.52 ms |  6.837 ms | 0.375 ms |  0.90 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |-----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |   109.8 ns |   3.10 ns |  0.17 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   677.8 ns |  55.47 ns |  3.04 ns |  6.17 |    0.03 |      - |         - |          NA |
| TokenSortRatio | 1,012.9 ns | 312.75 ns | 17.14 ns |  9.23 |    0.14 | 0.0782 |    1312 B |          NA |
| TokenSetRatio  | 1,111.8 ns | 137.62 ns |  7.54 ns | 10.13 |    0.06 | 0.0858 |    1448 B |          NA |
| WRatio         | 2,328.3 ns | 468.86 ns | 25.70 ns | 21.21 |    0.20 | 0.1640 |    2760 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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

| Method     | Operation     | Mean        | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |-------------- |------------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**   | **Ratio**         |    **111.5 ns** |   **4.63 ns** |  **0.25 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    230.8 ns |   7.80 ns |  0.43 ns |  2.07 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **675.6 ns** |  **44.61 ns** |  **2.45 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,125.8 ns | 513.25 ns | 28.13 ns | 14.99 |    0.06 |      - |     160 B |          NA |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,101.5 ns** | **470.10 ns** | **25.77 ns** |  **1.00** |    **0.03** | **0.0858** |    **1448 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  2,013.4 ns |  64.33 ns |  3.53 ns |  1.83 |    0.04 | 0.1144 |    1944 B |        1.34 |
|            |               |             |           |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **2,281.2 ns** | **393.31 ns** | **21.56 ns** |  **1.00** |    **0.01** | **0.1640** |    **2760 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,889.7 ns | 361.74 ns | 19.83 ns |  2.14 |    0.02 | 0.1831 |    3096 B |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

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

| Method                     | Length | Mean          | Error         | StdDev       | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |--------------:|--------------:|-------------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |      **28.73 ns** |      **0.926 ns** |     **0.051 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |     133.23 ns |      0.867 ns |     0.048 ns |  4.64 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |      29.60 ns |      0.719 ns |     0.039 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |      28.95 ns |      1.643 ns |     0.090 ns |  1.01 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **12**     |      **32.48 ns** |      **0.374 ns** |     **0.021 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |     141.71 ns |     36.949 ns |     2.025 ns |  4.36 |    0.05 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |      33.48 ns |      0.437 ns |     0.024 ns |  1.03 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |      32.00 ns |      1.286 ns |     0.071 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **16**     |      **33.52 ns** |      **0.807 ns** |     **0.044 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |     150.54 ns |      3.457 ns |     0.190 ns |  4.49 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |      34.08 ns |      5.989 ns |     0.328 ns |  1.02 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |      33.44 ns |      0.427 ns |     0.023 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **20**     |      **42.54 ns** |      **2.111 ns** |     **0.116 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |     186.30 ns |     25.532 ns |     1.399 ns |  4.38 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |      39.74 ns |      0.360 ns |     0.020 ns |  0.93 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |      34.97 ns |      4.033 ns |     0.221 ns |  0.82 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **24**     |      **63.96 ns** |      **4.741 ns** |     **0.260 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |     851.50 ns |    154.593 ns |     8.474 ns | 13.31 |    0.12 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |      63.22 ns |      1.634 ns |     0.090 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |      56.90 ns |      2.011 ns |     0.110 ns |  0.89 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **32**     |      **69.38 ns** |      **2.274 ns** |     **0.125 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |   1,057.03 ns |    363.095 ns |    19.902 ns | 15.24 |    0.25 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |      73.29 ns |      0.457 ns |     0.025 ns |  1.06 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |      65.27 ns |      4.943 ns |     0.271 ns |  0.94 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **128**    |     **404.92 ns** |     **12.735 ns** |     **0.698 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |  18,107.42 ns |  5,714.336 ns |   313.222 ns | 44.72 |    0.67 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |     406.16 ns |      3.073 ns |     0.168 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |     403.74 ns |     20.012 ns |     1.097 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |               |               |              |       |         |           |             |
| **Distance_Utf16**             | **512**    |   **4,980.71 ns** |     **28.021 ns** |     **1.536 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 306,139.36 ns | 30,373.936 ns | 1,664.898 ns | 61.47 |    0.29 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    |   5,215.92 ns |    321.550 ns |    17.625 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    |   4,997.20 ns |    621.287 ns |    34.055 ns |  1.00 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Dp**         | **8**    |    **132.07 ns** |    **57.226 ns** |   **3.137 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 8    |     61.40 ns |     0.724 ns |   0.040 ns |  0.47 |    0.01 |         - |          NA |
| Dp_Cjk     | 8    |    129.95 ns |     0.755 ns |   0.041 ns |  0.98 |    0.02 |         - |          NA |
| Kernel_Cjk | 8    |    110.54 ns |     1.390 ns |   0.076 ns |  0.84 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **220.59 ns** |     **3.326 ns** |   **0.182 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |     68.49 ns |     0.985 ns |   0.054 ns |  0.31 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    220.05 ns |     2.220 ns |   0.122 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    120.77 ns |     2.815 ns |   0.154 ns |  0.55 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **293.76 ns** |   **458.861 ns** |  **25.152 ns** |  **1.00** |    **0.10** |         **-** |          **NA** |
| Kernel     | 14   |     73.28 ns |     1.866 ns |   0.102 ns |  0.25 |    0.02 |         - |          NA |
| Dp_Cjk     | 14   |    276.93 ns |    44.016 ns |   2.413 ns |  0.95 |    0.07 |         - |          NA |
| Kernel_Cjk | 14   |    125.57 ns |    11.070 ns |   0.607 ns |  0.43 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **360.77 ns** |   **463.431 ns** |  **25.402 ns** |  **1.00** |    **0.08** |         **-** |          **NA** |
| Kernel     | 16   |     77.46 ns |     1.000 ns |   0.055 ns |  0.22 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    371.14 ns |   428.086 ns |  23.465 ns |  1.03 |    0.08 |         - |          NA |
| Kernel_Cjk | 16   |    130.77 ns |    10.359 ns |   0.568 ns |  0.36 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **723.91 ns** |   **178.725 ns** |   **9.797 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 18   |     84.68 ns |     4.224 ns |   0.232 ns |  0.12 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    718.52 ns |   625.332 ns |  34.277 ns |  0.99 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |    138.55 ns |     2.746 ns |   0.151 ns |  0.19 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **821.93 ns** |    **70.500 ns** |   **3.864 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |     88.22 ns |     2.742 ns |   0.150 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    819.81 ns |    14.427 ns |   0.791 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 20   |    140.08 ns |     5.567 ns |   0.305 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |    **992.53 ns** |    **14.857 ns** |   **0.814 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |     97.01 ns |     6.701 ns |   0.367 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |  1,035.13 ns |   659.864 ns |  36.169 ns |  1.04 |    0.03 |         - |          NA |
| Kernel_Cjk | 24   |    154.34 ns |     2.419 ns |   0.133 ns |  0.16 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,634.29 ns** |   **888.270 ns** |  **48.689 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 32   |    118.97 ns |    33.985 ns |   1.863 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,613.91 ns |   933.071 ns |  51.145 ns |  0.99 |    0.04 |         - |          NA |
| Kernel_Cjk | 32   |    181.40 ns |    11.131 ns |   0.610 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,265.95 ns** |   **546.560 ns** |  **29.959 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 48   |    147.48 ns |    10.320 ns |   0.566 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,254.27 ns |   630.186 ns |  34.543 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 48   |    259.31 ns |     3.092 ns |   0.169 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,546.82 ns** | **2,470.901 ns** | **135.438 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 64   |    175.80 ns |     4.024 ns |   0.221 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,329.68 ns | 3,137.277 ns | 171.965 ns |  0.96 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    309.97 ns |     1.343 ns |   0.074 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,023.88 ns** | **5,638.160 ns** | **309.047 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 96   |    362.35 ns |     2.263 ns |   0.124 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,263.44 ns |   530.269 ns |  29.066 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 96   |    967.32 ns |    12.004 ns |   0.658 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **28.27 ns** |     **0.296 ns** |   **0.016 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    125.94 ns |    16.132 ns |   0.884 ns |  4.45 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     28.19 ns |     0.716 ns |   0.039 ns |  1.00 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **312.57 ns** |     **6.571 ns** |   **0.360 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 64     |    706.40 ns |     8.318 ns |   0.456 ns |  2.26 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    313.32 ns |    39.759 ns |   2.179 ns |  1.00 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **17,367.83 ns** |   **162.349 ns** |   **8.899 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 17,994.25 ns | 1,286.364 ns |  70.510 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 15,652.65 ns | 1,847.602 ns | 101.273 ns |  0.90 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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
| **Distance_CodePoint** | **16**     | **32**       |     **357.1 ns** |    **15.71 ns** |   **0.86 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     260.4 ns |    27.34 ns |   1.50 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **352.3 ns** |     **3.18 ns** |   **0.17 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     257.2 ns |     8.55 ns |   0.47 ns |  0.73 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **450.5 ns** |    **60.09 ns** |   **3.29 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     349.7 ns |     2.95 ns |   0.16 ns |  0.78 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **445.1 ns** |    **13.82 ns** |   **0.76 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     351.9 ns |    65.10 ns |   3.57 ns |  0.79 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **554.4 ns** |    **33.01 ns** |   **1.81 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     448.1 ns |     5.27 ns |   0.29 ns |  0.81 |    0.00 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **542.8 ns** |    **79.79 ns** |   **4.37 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     442.1 ns |     2.24 ns |   0.12 ns |  0.81 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **629.2 ns** |    **66.84 ns** |   **3.66 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,353.3 ns |    22.70 ns |   1.24 ns |  2.15 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **642.4 ns** |    **30.58 ns** |   **1.68 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,318.5 ns |   113.71 ns |   6.23 ns |  2.05 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,633.6 ns** |   **401.58 ns** |  **22.01 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,776.9 ns |    34.25 ns |   1.88 ns |  2.19 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,654.7 ns** |   **370.70 ns** |  **20.32 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,686.0 ns |   181.36 ns |   9.94 ns |  2.14 |    0.01 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **20,112.0 ns** |   **160.66 ns** |   **8.81 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  70,142.1 ns | 9,292.94 ns | 509.38 ns |  3.49 |    0.02 |         - |          NA |
|                    |        |          |              |             |           |       |         |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **432,724.6 ns** | **8,079.16 ns** | **442.85 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  66,574.3 ns |   177.87 ns |   9.75 ns |  0.15 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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
| **Lodestar**             | **8**      |      **27.24 ns** |      **0.275 ns** |     **0.015 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      82.68 ns |      3.366 ns |     0.185 ns |  3.04 |    0.01 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      81.01 ns |      2.205 ns |     0.121 ns |  2.97 |    0.00 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     175.46 ns |     16.017 ns |     0.878 ns |  6.44 |    0.03 | 0.0076 |     128 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **64**     |     **311.82 ns** |      **3.352 ns** |     **0.184 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,355.23 ns |    524.595 ns |    28.755 ns | 17.17 |    0.08 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,395.87 ns |    531.501 ns |    29.133 ns |  4.48 |    0.08 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   9,978.73 ns |  4,071.795 ns |   223.189 ns | 32.00 |    0.62 | 0.0305 |     576 B |          NA |
|                      |        |               |               |              |       |         |        |           |             |
| **Lodestar**             | **512**    |  **15,668.21 ns** |  **7,238.687 ns** |   **396.777 ns** |  **1.00** |    **0.03** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 434,647.11 ns | 50,933.209 ns | 2,791.821 ns | 27.75 |    0.62 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  37,073.14 ns |    515.630 ns |    28.263 ns |  2.37 |    0.05 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 741,170.62 ns | 19,976.659 ns | 1,094.988 ns | 47.32 |    1.02 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsBenchmarks-report-github

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

| Method         | Samples | Classes | Mean          | Error         | StdDev      | Gen0   | Allocated |
|--------------- |-------- |-------- |--------------:|--------------:|------------:|-------:|----------:|
| **Matrix**         | **1000**    | **2**       |      **7.020 μs** |     **0.1894 μs** |   **0.0104 μs** | **0.0153** |     **312 B** |
| MatrixWeighted | 1000    | 2       |      6.986 μs |     1.2890 μs |   0.0707 μs | 0.0153 |     312 B |
| AccuracyScore  | 1000    | 2       |      1.030 μs |     0.0056 μs |   0.0003 μs |      - |         - |
| F1Macro        | 1000    | 2       |      7.362 μs |     0.3086 μs |   0.0169 μs | 0.0229 |     472 B |
| Report         | 1000    | 2       |      9.728 μs |     0.2966 μs |   0.0163 μs | 0.3815 |    6520 B |
| **Matrix**         | **1000**    | **10**      |      **7.236 μs** |     **0.4481 μs** |   **0.0246 μs** | **0.0687** |    **1248 B** |
| MatrixWeighted | 1000    | 10      |      7.472 μs |     0.2498 μs |   0.0137 μs | 0.0687 |    1248 B |
| AccuracyScore  | 1000    | 10      |      1.032 μs |     0.0254 μs |   0.0014 μs |      - |         - |
| F1Macro        | 1000    | 10      |      7.872 μs |     0.3697 μs |   0.0203 μs | 0.0916 |    1664 B |
| Report         | 1000    | 10      |     14.486 μs |     0.3262 μs |   0.0179 μs | 0.9155 |   15496 B |
| **Matrix**         | **100000**  | **2**       |    **830.846 μs** |   **281.4429 μs** |  **15.4268 μs** |      **-** |     **313 B** |
| MatrixWeighted | 100000  | 2       |    822.229 μs |     1.1579 μs |   0.0635 μs |      - |     313 B |
| AccuracyScore  | 100000  | 2       |    134.727 μs |    24.6803 μs |   1.3528 μs |      - |         - |
| F1Macro        | 100000  | 2       |    829.386 μs |     7.6181 μs |   0.4176 μs |      - |     473 B |
| Report         | 100000  | 2       |    839.840 μs |    79.1157 μs |   4.3366 μs |      - |    6545 B |
| **Matrix**         | **100000**  | **10**      |    **944.612 μs** |    **41.2583 μs** |   **2.2615 μs** |      **-** |    **1249 B** |
| MatrixWeighted | 100000  | 10      |    956.566 μs |    67.9113 μs |   3.7224 μs |      - |    1249 B |
| AccuracyScore  | 100000  | 10      |    246.152 μs |    18.2558 μs |   1.0007 μs |      - |         - |
| F1Macro        | 100000  | 10      |    969.645 μs |    44.1344 μs |   2.4192 μs |      - |    1665 B |
| Report         | 100000  | 10      |    975.229 μs |   120.8332 μs |   6.6233 μs |      - |   15841 B |
| **Matrix**         | **1000000** | **2**       |  **8,808.674 μs** |   **411.1492 μs** |  **22.5365 μs** |      **-** |     **319 B** |
| MatrixWeighted | 1000000 | 2       |  8,567.203 μs |   445.7470 μs |  24.4329 μs |      - |     324 B |
| AccuracyScore  | 1000000 | 2       |  1,914.885 μs |    30.2947 μs |   1.6606 μs |      - |         - |
| F1Macro        | 1000000 | 2       |  9,015.740 μs | 1,449.7268 μs |  79.4644 μs |      - |     484 B |
| Report         | 1000000 | 2       |  8,455.346 μs |    87.4640 μs |   4.7942 μs |      - |    6572 B |
| **Matrix**         | **1000000** | **10**      | **10,076.568 μs** |   **590.1541 μs** |  **32.3483 μs** |      **-** |    **1260 B** |
| MatrixWeighted | 1000000 | 10      | 10,271.207 μs | 2,161.9838 μs | 118.5056 μs |      - |    1260 B |
| AccuracyScore  | 1000000 | 10      |  3,083.578 μs |    75.4428 μs |   4.1353 μs |      - |         - |
| F1Macro        | 1000000 | 10      | 10,324.449 μs | 1,188.8689 μs |  65.1659 μs |      - |    1676 B |
| Report         | 1000000 | 10      | 10,179.076 μs |   297.0237 μs |  16.2809 μs |      - |   15892 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MetricsIncumbentBenchmarks-report-github

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

| Method   | Samples | Request       | Mean         | Error        | StdDev      | Ratio  | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|--------- |-------- |-------------- |-------------:|-------------:|------------:|-------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **Lodestar** | **100000**  | **Bundle**        |   **7,597.0 μs** |    **869.09 μs** |    **47.64 μs** |   **1.00** |    **0.01** |        **-** |        **-** |        **-** |     **1016 B** |        **1.00** |
| MlNet    | 100000  | Bundle        |  39,895.0 μs |  1,899.05 μs |   104.09 μs |   5.25 |    0.03 | 538.4615 | 538.4615 | 538.4615 |  5089463 B |    5,009.31 |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **100000**  | **AccuracyAlone** |     **191.0 μs** |     **12.82 μs** |     **0.70 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 100000  | AccuracyAlone |  38,433.9 μs |  1,696.18 μs |    92.97 μs | 201.27 |    0.77 | 642.8571 | 642.8571 | 642.8571 |  5090419 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **Bundle**        | **104,868.8 μs** | **67,714.10 μs** | **3,711.64 μs** |   **1.00** |    **0.04** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | Bundle        | 272,449.2 μs | 20,327.42 μs | 1,114.21 μs |   2.60 |    0.08 |        - |        - |        - | 23231816 B |          NA |
|          |         |               |              |              |             |        |         |          |          |          |            |             |
| **Lodestar** | **1000000** | **AccuracyAlone** |   **2,924.3 μs** |     **14.00 μs** |     **0.77 μs** |   **1.00** |    **0.00** |        **-** |        **-** |        **-** |          **-** |          **NA** |
| MlNet    | 1000000 | AccuracyAlone | 248,836.1 μs |  6,665.13 μs |   365.34 μs |  85.09 |    0.11 |        - |        - |        - | 23231816 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

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

| Method     | Band | Mean         | Error      | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----- |-------------:|-----------:|----------:|------:|--------:|----------:|------------:|
| **Dp**         | **4**    |     **82.14 ns** |   **5.319 ns** |  **0.292 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 4    |     80.45 ns |   4.694 ns |  0.257 ns |  0.98 |    0.00 |         - |          NA |
| Dp_Cjk     | 4    |     82.36 ns |   1.035 ns |  0.057 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     80.01 ns |   0.672 ns |  0.037 ns |  0.97 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **6**    |    **117.32 ns** |  **15.675 ns** |  **0.859 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 6    |     96.75 ns |   0.577 ns |  0.032 ns |  0.82 |    0.01 |         - |          NA |
| Dp_Cjk     | 6    |    121.16 ns |   6.981 ns |  0.383 ns |  1.03 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    148.93 ns |   8.953 ns |  0.491 ns |  1.27 |    0.01 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **8**    |    **156.92 ns** |   **3.444 ns** |  **0.189 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |    100.44 ns |   0.333 ns |  0.018 ns |  0.64 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    179.82 ns |   3.713 ns |  0.204 ns |  1.15 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    185.91 ns |  17.252 ns |  0.946 ns |  1.18 |    0.01 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **10**   |    **209.18 ns** |  **56.263 ns** |  **3.084 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Kernel     | 10   |    107.18 ns |   2.620 ns |  0.144 ns |  0.51 |    0.01 |         - |          NA |
| Dp_Cjk     | 10   |    203.06 ns |   7.496 ns |  0.411 ns |  0.97 |    0.01 |         - |          NA |
| Kernel_Cjk | 10   |    168.93 ns |  11.165 ns |  0.612 ns |  0.81 |    0.01 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **12**   |    **268.64 ns** |  **12.753 ns** |  **0.699 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 12   |    117.87 ns |   3.719 ns |  0.204 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    268.63 ns |  10.680 ns |  0.585 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 12   |    178.24 ns |  18.669 ns |  1.023 ns |  0.66 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **16**   |    **429.57 ns** |   **7.300 ns** |  **0.400 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 16   |    138.24 ns |   5.020 ns |  0.275 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    426.62 ns |  19.276 ns |  1.057 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 16   |    200.30 ns |   5.226 ns |  0.286 ns |  0.47 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **24**   |    **863.71 ns** |  **24.016 ns** |  **1.316 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    180.03 ns |   8.983 ns |  0.492 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    861.72 ns |  22.442 ns |  1.230 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    245.49 ns |  13.702 ns |  0.751 ns |  0.28 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **32**   |  **1,482.41 ns** |   **9.714 ns** |  **0.532 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 32   |    219.34 ns |   4.614 ns |  0.253 ns |  0.15 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,481.91 ns |  35.818 ns |  1.963 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 32   |    297.85 ns |  10.971 ns |  0.601 ns |  0.20 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **48**   |  **3,243.12 ns** |  **60.622 ns** |  **3.323 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    300.99 ns |   8.335 ns |  0.457 ns |  0.09 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,245.15 ns |  41.719 ns |  2.287 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    390.83 ns |   8.259 ns |  0.453 ns |  0.12 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **64**   |  **5,642.23 ns** |  **92.923 ns** |  **5.093 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 64   |    381.34 ns |  20.242 ns |  1.110 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,702.54 ns | 932.022 ns | 51.087 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    484.09 ns |   5.953 ns |  0.326 ns |  0.09 |    0.00 |         - |          NA |
|            |      |              |            |           |       |         |           |             |
| **Dp**         | **96**   | **12,579.55 ns** |  **93.324 ns** |  **5.115 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |  1,341.46 ns |  13.521 ns |  0.741 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,538.74 ns | 111.989 ns |  6.138 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   |  1,555.12 ns | 198.567 ns | 10.884 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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

| Method                 | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| VocabTxt               |  4.106 ms |  3.6643 ms | 0.2009 ms | 109.3750 | 101.5625 |  31.2500 |  3711.63 KB |
| TokenizerJsonWordPiece | 11.192 ms |  2.2500 ms | 0.1233 ms | 187.5000 | 171.8750 |  46.8750 |   5852.4 KB |
| TokenizerJsonUnigram   | 11.009 ms |  0.3376 ms | 0.0185 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |  4.071 ms |  1.3306 ms | 0.0729 ms | 109.3750 | 101.5625 |  31.2500 |  3440.04 KB |
| TfidfSave              |  1.723 ms |  0.3893 ms | 0.0213 ms |  21.4844 |  15.6250 |  15.6250 |  2137.04 KB |
| TfidfLoad              |  4.383 ms |  0.3919 ms | 0.0215 ms |  85.9375 |  78.1250 |  23.4375 |  2930.47 KB |
| EmbeddingIndexSave     |  3.745 ms |  0.2468 ms | 0.0135 ms | 199.2188 | 195.3125 | 195.3125 | 20349.83 KB |
| EmbeddingIndexLoad     |  4.443 ms |  0.3623 ms | 0.0199 ms | 179.6875 | 140.6250 | 117.1875 | 16094.61 KB |
| EmbeddingIndexSaveFile | 49.676 ms | 10.3848 ms | 0.5692 ms |        - |        - |        - |   323.49 KB |
| EmbeddingIndexLoadGzip | 74.749 ms |  1.2276 ms | 0.0673 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrincipalComponentVarianceBenchmarks-report-github

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
| **Lodestar_ExplainedVariance** | **100x200** | **13,890.82 μs** | **398.795 μs** | **21.859 μs** |  **1.00** |  **46.8750** |  **46.8750** |  **46.8750** | **318.28 KB** |        **1.00** |
| NumFlat_Pca                | 100x200 | 16,332.49 μs | 391.131 μs | 21.439 μs |  1.18 | 187.5000 | 187.5000 | 187.5000 | 628.56 KB |        1.97 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x10** |    **146.86 μs** |  **12.037 μs** |  **0.660 μs** |  **1.00** |        **-** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 2000x10 |    201.00 μs |   9.445 μs |  0.518 μs |  1.37 |        - |        - |        - |   1.94 KB |        0.82 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **2000x50** |  **3,471.05 μs** |   **4.943 μs** |  **0.271 μs** |  **1.00** |        **-** |        **-** |        **-** |  **42.07 KB** |        **1.00** |
| NumFlat_Pca                | 2000x50 |  2,836.43 μs |  98.070 μs |  5.376 μs |  0.82 |        - |        - |        - |  40.07 KB |        0.95 |
|                            |         |              |            |           |       |          |          |          |           |             |
| **Lodestar_ExplainedVariance** | **200x10**  |     **26.37 μs** |   **1.011 μs** |  **0.055 μs** |  **1.00** |   **0.1221** |        **-** |        **-** |   **2.38 KB** |        **1.00** |
| NumFlat_Pca                | 200x10  |     27.29 μs |   0.918 μs |  0.050 μs |  1.03 |   0.0916 |        - |        - |   1.94 KB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RegressionMetricsBenchmarks-report-github

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

| Method   | Samples | Mean        | Error        | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|--------- |-------- |------------:|-------------:|----------:|---------:|---------:|---------:|----------:|
| **Mse**      | **100000**  |    **48.40 μs** |     **2.185 μs** |  **0.120 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 100000  |    47.62 μs |     2.513 μs |  0.138 μs |        - |        - |        - |         - |
| R2Score  | 100000  |   122.55 μs |     3.250 μs |  0.178 μs |        - |        - |        - |      64 B |
| MedianAe | 100000  |   509.59 μs |    12.837 μs |  0.704 μs | 199.2188 | 199.2188 | 199.2188 |  800186 B |
| **Mse**      | **1000000** |   **482.05 μs** |    **24.063 μs** |  **1.319 μs** |        **-** |        **-** |        **-** |         **-** |
| Mae      | 1000000 |   472.24 μs |    16.615 μs |  0.911 μs |        - |        - |        - |         - |
| R2Score  | 1000000 | 1,224.48 μs |    12.462 μs |  0.683 μs |        - |        - |        - |      65 B |
| MedianAe | 1000000 | 5,742.40 μs | 1,104.945 μs | 60.566 μs | 320.3125 | 320.3125 | 320.3125 | 8000269 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SimilaritySketchBenchmarks-report-github

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

| Method           | Documents | Permutations | Mean       | Error      | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated    | Alloc Ratio |
|----------------- |---------- |------------- |-----------:|-----------:|----------:|------:|----------:|----------:|---------:|-------------:|------------:|
| **ExactPairwise**    | **500**       | **64**           |  **56.217 ms** |  **1.2053 ms** | **0.0661 ms** |  **1.00** |  **444.4444** |         **-** |        **-** |   **8152.39 KB** |        **1.00** |
| SketchThenVerify | 500       | 64           |  13.625 ms |  0.8163 ms | 0.0447 ms |  0.24 |  171.8750 |  156.2500 |  78.1250 |   2699.49 KB |        0.33 |
| SignaturesOnly   | 500       | 64           |   9.810 ms |  0.6405 ms | 0.0351 ms |  0.17 |   31.2500 |         - |        - |    511.75 KB |        0.06 |
| FingerprintsOnly | 500       | 64           |  10.623 ms |  1.1376 ms | 0.0624 ms |  0.19 |   31.2500 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **500**       | **128**          |  **55.314 ms** |  **0.5259 ms** | **0.0288 ms** |  **1.00** |  **444.4444** |         **-** |        **-** |   **8152.42 KB** |        **1.00** |
| SketchThenVerify | 500       | 128          |  16.887 ms |  0.3788 ms | 0.0208 ms |  0.31 |  343.7500 |  312.5000 | 218.7500 |   4745.26 KB |        0.58 |
| SignaturesOnly   | 500       | 128          |  12.765 ms |  0.5498 ms | 0.0301 ms |  0.23 |   31.2500 |         - |        - |    636.75 KB |        0.08 |
| FingerprintsOnly | 500       | 128          |  12.092 ms |  0.5725 ms | 0.0314 ms |  0.22 |   31.2500 |         - |        - |    652.36 KB |        0.08 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **64**           | **970.372 ms** | **52.2933 ms** | **2.8664 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126359.81 KB** |        **1.00** |
| SketchThenVerify | 2000      | 64           |  56.737 ms |  2.6506 ms | 0.1453 ms |  0.06 |  888.8889 |  888.8889 | 444.4444 |  10993.82 KB |        0.09 |
| SignaturesOnly   | 2000      | 64           |  39.266 ms |  0.9445 ms | 0.0518 ms |  0.04 |   76.9231 |         - |        - |   2046.95 KB |        0.02 |
| FingerprintsOnly | 2000      | 64           |  42.500 ms |  1.0418 ms | 0.0571 ms |  0.04 |   83.3333 |         - |        - |   2609.43 KB |        0.02 |
|                  |           |              |            |            |           |       |           |           |          |              |             |
| **ExactPairwise**    | **2000**      | **128**          | **981.215 ms** | **29.4565 ms** | **1.6146 ms** |  **1.00** | **7000.0000** |         **-** |        **-** | **126360.09 KB** |        **1.00** |
| SketchThenVerify | 2000      | 128          |  75.757 ms | 23.9047 ms | 1.3103 ms |  0.08 | 1571.4286 | 1571.4286 | 714.2857 |  19322.83 KB |        0.15 |
| SignaturesOnly   | 2000      | 128          |  51.205 ms |  3.0530 ms | 0.1673 ms |  0.05 |  100.0000 |         - |        - |   2546.97 KB |        0.02 |
| FingerprintsOnly | 2000      | 128          |  41.565 ms |  0.8426 ms | 0.0462 ms |  0.04 |   83.3333 |         - |        - |   2609.43 KB |        0.02 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.StopWordBenchmarks-report-github

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

| Method               | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated | Alloc Ratio |
|--------------------- |---------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|---------:|----------:|------------:|
| **Count**                | **200**       |  **7.483 ms** | **2.2245 ms** | **0.1219 ms** |  **1.00** |    **0.02** |  **500.0000** |  **234.3750** |  **62.5000** |   **7.92 MB** |        **1.00** |
| CountWithStopWords   | 200       |  6.191 ms | 0.1012 ms | 0.0055 ms |  0.83 |    0.01 |  390.6250 |  187.5000 |        - |   6.34 MB |        0.80 |
| Hashing              | 200       |  7.386 ms | 0.1718 ms | 0.0094 ms |  0.99 |    0.01 |  507.8125 |  171.8750 |  70.3125 |   8.08 MB |        1.02 |
| HashingWithStopWords | 200       |  6.459 ms | 0.4264 ms | 0.0234 ms |  0.86 |    0.01 |  406.2500 |  156.2500 |        - |   6.52 MB |        0.82 |
|                      |           |           |           |           |       |         |           |           |          |           |             |
| **Count**                | **1000**      | **30.094 ms** | **1.5226 ms** | **0.0835 ms** |  **1.00** |    **0.00** | **2718.7500** | **1031.2500** | **531.2500** |  **38.64 MB** |        **1.00** |
| CountWithStopWords   | 1000      | 23.246 ms | 3.9938 ms | 0.2189 ms |  0.77 |    0.01 | 1968.7500 |  781.2500 | 250.0000 |  30.99 MB |        0.80 |
| Hashing              | 1000      | 28.798 ms | 3.1126 ms | 0.1706 ms |  0.96 |    0.01 | 2562.5000 |  750.0000 | 593.7500 |  39.55 MB |        1.02 |
| HashingWithStopWords | 1000      | 24.297 ms | 1.2688 ms | 0.0695 ms |  0.81 |    0.00 | 2031.2500 |  625.0000 | 250.0000 |  31.83 MB |        0.82 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean     | Error    | StdDev   | Ratio | Gen0     | Allocated | Alloc Ratio |
|------------- |-------------- |---------:|---------:|---------:|------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     | **35.16 ms** | **1.431 ms** | **0.078 ms** |  **1.00** | **533.3333** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     | 53.62 ms | 3.854 ms | 0.211 ms |  1.53 | 200.0000 |   3.55 MB |        0.41 |
|              |               |          |          |          |       |          |           |             |
| **Lodestar**     | **SentencePiece** | **47.41 ms** | **7.681 ms** | **0.421 ms** |  **1.00** | **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece | 52.63 ms | 3.200 ms | 0.175 ms |  1.11 | 100.0000 |   3.09 MB |        0.57 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `indel`
- `levenshtein`
- `metrics`
- `ols`
- `persistence`
- `stats`

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 107.9 | 23.7 | 4.54x C# faster |
| latin | 32 | 156.5 | 69.7 | 2.25x C# faster |
| latin | 128 | 475.7 | 416.5 | 1.14x C# faster |
| latin | 512 | 4862.4 | 5505.4 | 1.13x Py faster |
| cjk | 8 | 125.8 | 23.7 | 5.30x C# faster |
| cjk | 32 | 234.1 | 142.6 | 1.64x C# faster |
| cjk | 128 | 2037.1 | 1419.8 | 1.43x C# faster |
| cjk | 512 | 16828.1 | 9275.6 | 1.81x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 135.5 | 18.4 | 7.36x C# faster |
| latin | 32 | 252.6 | 126.1 | 2.00x C# faster |
| latin | 128 | 1837.2 | 1583.6 | 1.16x C# faster |
| latin | 512 | 15710.5 | 16994.9 | 1.08x Py faster |
| cjk | 8 | 137.0 | 18.4 | 7.43x C# faster |
| cjk | 32 | 287.0 | 190.9 | 1.50x C# faster |
| cjk | 128 | 3040.1 | 2512.0 | 1.21x C# faster |
| cjk | 512 | 26171.3 | 20456.3 | 1.28x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.009 | 0.767 | 88.51x | 0.009 | 0.767 | 88.51x |
| accuracy_n1000_k2 | 0.001 | 0.400 | 380.95x | 0.001 | 0.400 | 380.84x |
| precision_recall_f1_macro_n1000_k2 | 0.007 | 1.391 | 196.53x | 0.007 | 1.391 | 196.55x |
| classification_report_n1000_k2 | 0.010 | 5.248 | 539.90x | 0.010 | 5.247 | 539.91x |
| roc_auc_binary_n1000_k2 | 0.016 | 1.534 | 94.73x | 0.016 | 1.533 | 94.72x |
| balanced_accuracy_n1000_k2 | 0.007 | 0.820 | 116.50x | 0.007 | 0.820 | 116.48x |
| matthews_n1000_k2 | 0.007 | 1.552 | 220.55x | 0.007 | 1.551 | 220.55x |
| cohen_kappa_n1000_k2 | 0.007 | 0.869 | 123.29x | 0.007 | 0.869 | 123.29x |
| mse_n1000_k2 | 0.001 | 0.216 | 416.68x | 0.001 | 0.216 | 416.62x |
| mae_n1000_k2 | 0.001 | 0.215 | 419.70x | 0.001 | 0.215 | 419.64x |
| median_ae_n1000_k2 | 0.005 | 0.228 | 47.83x | 0.005 | 0.228 | 47.83x |
| r2_n1000_k2 | 0.001 | 0.269 | 203.18x | 0.001 | 0.269 | 203.14x |
| confusion_matrix_n1000_k10 | 0.009 | 0.772 | 84.42x | 0.009 | 0.772 | 84.41x |
| accuracy_n1000_k10 | 0.001 | 0.404 | 385.63x | 0.001 | 0.404 | 385.65x |
| precision_recall_f1_macro_n1000_k10 | 0.008 | 1.427 | 184.44x | 0.008 | 1.427 | 184.45x |
| classification_report_n1000_k10 | 0.014 | 5.526 | 386.02x | 0.014 | 5.525 | 386.01x |
| roc_auc_ovr_macro_n1000_k10 | 0.499 | 8.099 | 16.22x | 0.499 | 8.098 | 16.22x |
| balanced_accuracy_n1000_k10 | 0.008 | 0.827 | 105.90x | 0.008 | 0.827 | 105.89x |
| matthews_n1000_k10 | 0.008 | 1.586 | 203.93x | 0.008 | 1.586 | 203.93x |
| cohen_kappa_n1000_k10 | 0.008 | 0.878 | 107.00x | 0.008 | 0.878 | 107.01x |
| mse_n1000_k10 | 0.001 | 0.214 | 411.78x | 0.001 | 0.213 | 411.79x |
| mae_n1000_k10 | 0.001 | 0.213 | 416.76x | 0.001 | 0.213 | 416.72x |
| median_ae_n1000_k10 | 0.005 | 0.230 | 48.21x | 0.005 | 0.230 | 48.21x |
| r2_n1000_k10 | 0.001 | 0.270 | 204.45x | 0.001 | 0.270 | 204.43x |
| confusion_matrix_n100000_k2 | 0.983 | 10.957 | 11.15x | 0.982 | 10.955 | 11.15x |
| accuracy_n100000_k2 | 0.106 | 3.786 | 35.81x | 0.106 | 3.785 | 35.81x |
| precision_recall_f1_macro_n100000_k2 | 0.783 | 12.555 | 16.03x | 0.783 | 12.553 | 16.02x |
| classification_report_n100000_k2 | 0.816 | 26.776 | 32.81x | 0.816 | 26.774 | 32.81x |
| roc_auc_binary_n100000_k2 | 2.980 | 28.424 | 9.54x | 2.979 | 28.421 | 9.54x |
| balanced_accuracy_n100000_k2 | 0.798 | 11.022 | 13.82x | 0.798 | 11.021 | 13.82x |
| matthews_n100000_k2 | 0.810 | 22.032 | 27.19x | 0.810 | 22.032 | 27.19x |
| cohen_kappa_n100000_k2 | 0.821 | 11.056 | 13.47x | 0.821 | 11.055 | 13.47x |
| mse_n100000_k2 | 0.048 | 0.369 | 7.72x | 0.048 | 0.369 | 7.72x |
| mae_n100000_k2 | 0.047 | 0.369 | 7.83x | 0.047 | 0.369 | 7.83x |
| median_ae_n100000_k2 | 0.579 | 1.890 | 3.26x | 0.622 | 1.889 | 3.04x |
| r2_n100000_k2 | 0.122 | 0.582 | 4.77x | 0.122 | 0.582 | 4.77x |
| confusion_matrix_n100000_k10 | 0.956 | 10.955 | 11.46x | 0.956 | 10.952 | 11.46x |
| accuracy_n100000_k10 | 0.106 | 3.779 | 35.77x | 0.106 | 3.778 | 35.77x |
| precision_recall_f1_macro_n100000_k10 | 0.952 | 13.242 | 13.90x | 0.952 | 13.240 | 13.90x |
| classification_report_n100000_k10 | 0.957 | 29.687 | 31.01x | 0.957 | 29.683 | 31.01x |
| roc_auc_ovr_macro_n100000_k10 | 31.669 | 231.455 | 7.31x | 31.667 | 231.419 | 7.31x |
| balanced_accuracy_n100000_k10 | 0.911 | 11.000 | 12.08x | 0.911 | 11.000 | 12.08x |
| matthews_n100000_k10 | 0.946 | 22.817 | 24.13x | 0.946 | 22.814 | 24.12x |
| cohen_kappa_n100000_k10 | 1.120 | 11.080 | 9.89x | 1.120 | 11.080 | 9.89x |
| mse_n100000_k10 | 0.048 | 0.371 | 7.75x | 0.048 | 0.371 | 7.75x |
| mae_n100000_k10 | 0.047 | 0.373 | 7.93x | 0.047 | 0.373 | 7.93x |
| median_ae_n100000_k10 | 0.586 | 1.890 | 3.23x | 0.627 | 1.890 | 3.01x |
| r2_n100000_k10 | 0.122 | 0.585 | 4.78x | 0.122 | 0.585 | 4.78x |
| confusion_matrix_n1000000_k2 | 8.523 | 103.049 | 12.09x | 8.522 | 103.046 | 12.09x |
| accuracy_n1000000_k2 | 2.138 | 34.293 | 16.04x | 2.137 | 34.290 | 16.04x |
| precision_recall_f1_macro_n1000000_k2 | 8.554 | 112.492 | 13.15x | 8.554 | 112.480 | 13.15x |
| classification_report_n1000000_k2 | 8.813 | 220.336 | 25.00x | 8.814 | 220.282 | 24.99x |
| roc_auc_binary_n1000000_k2 | 44.713 | 312.892 | 7.00x | 44.715 | 312.822 | 7.00x |
| balanced_accuracy_n1000000_k2 | 8.522 | 102.773 | 12.06x | 8.521 | 102.761 | 12.06x |
| matthews_n1000000_k2 | 8.825 | 207.143 | 23.47x | 8.824 | 207.130 | 23.47x |
| cohen_kappa_n1000000_k2 | 8.557 | 102.860 | 12.02x | 8.557 | 102.846 | 12.02x |
| mse_n1000000_k2 | 0.480 | 1.884 | 3.93x | 0.480 | 1.884 | 3.93x |
| mae_n1000000_k2 | 0.471 | 1.858 | 3.94x | 0.471 | 1.858 | 3.94x |
| median_ae_n1000000_k2 | 5.319 | 15.638 | 2.94x | 5.413 | 15.636 | 2.89x |
| r2_n1000000_k2 | 1.220 | 3.541 | 2.90x | 1.220 | 3.540 | 2.90x |
| confusion_matrix_n1000000_k10 | 10.070 | 102.545 | 10.18x | 10.069 | 102.534 | 10.18x |
| accuracy_n1000000_k10 | 3.112 | 34.679 | 11.14x | 3.112 | 34.667 | 11.14x |
| precision_recall_f1_macro_n1000000_k10 | 10.116 | 119.660 | 11.83x | 10.116 | 119.636 | 11.83x |
| classification_report_n1000000_k10 | 10.750 | 247.216 | 23.00x | 10.748 | 247.173 | 23.00x |
| balanced_accuracy_n1000000_k10 | 10.149 | 103.267 | 10.18x | 10.148 | 103.256 | 10.17x |
| matthews_n1000000_k10 | 10.713 | 215.091 | 20.08x | 10.713 | 215.070 | 20.08x |
| cohen_kappa_n1000000_k10 | 10.188 | 102.818 | 10.09x | 10.188 | 102.816 | 10.09x |
| mse_n1000000_k10 | 0.478 | 1.867 | 3.91x | 0.478 | 1.866 | 3.91x |
| mae_n1000000_k10 | 0.469 | 1.865 | 3.98x | 0.469 | 1.864 | 3.97x |
| median_ae_n1000000_k10 | 5.511 | 15.645 | 2.84x | 5.646 | 15.644 | 2.77x |
| r2_n1000000_k10 | 1.221 | 3.619 | 2.96x | 1.221 | 3.619 | 2.96x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.242 | 2.523 | 10.40x | 0.242 | 2.522 | 10.40x |
| ols_summary_n10000 | 2.733 | 11.302 | 4.14x | 2.864 | 11.301 | 3.95x |
| ols_summary_n100000 | 24.524 | 105.723 | 4.31x | 24.979 | 422.245 | 16.90x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.026 | 9.915 | 1.97x | 5.269 | 9.915 | 1.88x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.263 | 16.552 | 1.35x | 12.723 | 16.550 | 1.30x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.043 | 39.550 | 3.28x | 12.218 | 39.543 | 3.24x | 1,990,038 | 1,990,038 |
| spiece_model | 4.768 | 30.752 | 6.45x | 4.965 | 30.749 | 6.19x | 533,084 | 533,084 |
| tfidf_save | 1.666 | 2.440 | 1.46x | 1.702 | 2.439 | 1.43x | 581,787 | 591,922 |
| tfidf_load | 4.916 | 4.274 | 0.87x | 5.113 | 4.273 | 0.84x | 581,787 | 591,922 |
| embedding_index_save | 3.724 | 1.320 | 0.35x | 3.945 | 1.320 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.025 | 37.486 | 0.75x | 9.902 | 4.312 | 0.44x | 20,589,007 | 15,360,128 |
| embedding_index_load | 4.622 | 1.460 | 0.32x | 5.141 | 1.460 | 0.28x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 5.461 | 0.932 | 0.17x | 5.887 | 0.931 | 0.16x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 3.453 | 1.488 | 0.43x | 3.710 | 1.488 | 0.40x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.197 | 1.428 | 1.19x | 1.424 | 1.428 | 1.00x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 68.52x | 0.000 | 0.001 | 68.52x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 452.759 | 637.183 | 1.41x | 452.786 | 637.092 | 1.41x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 74.425 | 72.608 | 0.98x | 74.422 | 72.602 | 0.98x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.639 | 142.22x | 0.004 | 0.639 | 142.19x |
| mann_whitney_n1000 | 0.033 | 0.616 | 18.60x | 0.033 | 0.616 | 18.60x |
| chi_square_n1000 | 0.000 | 0.253 | 1408.79x | 0.000 | 0.253 | 1408.67x |
| welch_t_n10000 | 0.042 | 0.676 | 15.96x | 0.042 | 0.676 | 15.96x |
| mann_whitney_n10000 | 0.528 | 3.011 | 5.70x | 0.528 | 3.011 | 5.71x |
| chi_square_n10000 | 0.001 | 0.255 | 196.77x | 0.001 | 0.255 | 196.78x |
| welch_t_n100000 | 0.423 | 1.097 | 2.59x | 0.423 | 1.097 | 2.59x |
| mann_whitney_n100000 | 6.392 | 31.015 | 4.85x | 6.392 | 31.009 | 4.85x |
| chi_square_n100000 | 0.014 | 0.275 | 19.13x | 0.014 | 0.275 | 19.13x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Selected, and not reached tonight

The run stops starting classes when the budget left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are selected again tomorrow whether or not anything else changes.

- `BitParallelEditDistanceBenchmarks`
- `Bm25Benchmarks`
- `ChainedProductBenchmarks`
- `DistributionTailBenchmarks`
- `GlmBenchmarks`
- `OlsBenchmarks`
- `QuantileBenchmarks`
- `RobustCovarianceBenchmarks`
- `SerialCorrelationBenchmarks`
- `StatsBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `VectorizerIncumbentBenchmarks`
