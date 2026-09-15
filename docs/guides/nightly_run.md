# Nightly benchmark run

<!-- nightly-baseline: 1d0a36307dabc3141c3a5339c8bef057c359d454 -->
<!-- nightly-owed: BitParallelEditDistanceBenchmarks ChainedProductBenchmarks CoxBenchmarks GlmBenchmarks GlmPoissonBenchmarks OlsBenchmarks QuantileBenchmarks RobustCovarianceBenchmarks SerialCorrelationBenchmarks StationarityBenchmarks SurvivalBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `1d0a36307dabc3141c3a5339c8bef057c359d454`
- Previous run: `1d0a36307dabc3141c3a5339c8bef057c359d454`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `BatchEmbeddingBenchmarks`
- `BkTreeBenchmarks`
- `BlockedTableBenchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `BpeWordCacheBenchmarks`
- `BucketRouteDiagnostics`
- `FuzzBenchmarks`
- `FuzzIncumbentBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `VectorizerIncumbentBenchmarks`
- `IndelBenchmarks`
- `IndelCodePointBenchmarks`
- `LcsGateBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinIncumbentBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `MyersGateBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `PersistenceBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `Bm25Benchmarks`
- `CoxBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `StatsBenchmarks`
- `RankTestBenchmarks`
- `DistributionTailBenchmarks`
- `StationarityBenchmarks`
- `SerialCorrelationBenchmarks`
- `QuantileBenchmarks`
- `OlsBenchmarks`
- `RobustCovarianceBenchmarks`
- `GlmBenchmarks`
- `GlmPoissonBenchmarks`

### Lodestar.Stats.Benchmarks.DistributionTailBenchmarks-report-github

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

| Method                     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ChiSquaredSfOneDf          |  28.95 ns |  0.295 ns | 0.016 ns |  1.00 |    0.00 |         - |          NA |
| ChiSquaredSfFourDf         |  11.82 ns |  0.153 ns | 0.008 ns |  0.41 |    0.00 |         - |          NA |
| ChiSquaredSfThreeDfFarTail |  36.42 ns |  8.616 ns | 0.472 ns |  1.26 |    0.01 |         - |          NA |
| ChiSquaredSfHundredDf      |  77.59 ns | 14.667 ns | 0.804 ns |  2.68 |    0.02 |         - |          NA |
| ChiSquaredSfFractionalDf   | 119.76 ns |  1.359 ns | 0.075 ns |  4.14 |    0.00 |         - |          NA |
| NormalQuantile             | 117.18 ns |  7.153 ns | 0.392 ns |  4.05 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.RankTestBenchmarks-report-github

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

| Method            | SampleSize | Ties  | Mean        | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------ |----------- |------ |------------:|----------:|---------:|---------:|---------:|---------:|----------:|
| **KruskalWallisTest** | **10000**      | **False** |  **1,215.3 μs** |  **61.20 μs** |  **3.35 μs** |        **-** |        **-** |        **-** |      **82 B** |
| WilcoxonPaired    | 10000      | False |    251.8 μs |  36.91 μs |  2.02 μs |   4.3945 |        - |        - |   80057 B |
| MannWhitneyTest   | 10000      | False |    462.8 μs |  42.24 μs |  2.32 μs |        - |        - |        - |         - |
| **KruskalWallisTest** | **10000**      | **True**  |    **565.0 μs** | **138.20 μs** |  **7.58 μs** |        **-** |        **-** |        **-** |      **81 B** |
| WilcoxonPaired    | 10000      | True  |    210.8 μs |   5.32 μs |  0.29 μs |   4.6387 |        - |        - |   80056 B |
| MannWhitneyTest   | 10000      | True  |    361.6 μs |   5.89 μs |  0.32 μs |        - |        - |        - |         - |
| **KruskalWallisTest** | **100000**     | **False** | **12,193.5 μs** | **806.58 μs** | **44.21 μs** |        **-** |        **-** |        **-** |         **-** |
| WilcoxonPaired    | 100000     | False |  3,574.3 μs | 145.91 μs |  8.00 μs | 171.8750 | 171.8750 | 171.8750 |  800229 B |
| MannWhitneyTest   | 100000     | False |  5,304.5 μs |  79.21 μs |  4.34 μs |        - |        - |        - |         - |
| **KruskalWallisTest** | **100000**     | **True**  |  **5,439.5 μs** | **105.39 μs** |  **5.78 μs** |        **-** |        **-** |        **-** |      **88 B** |
| WilcoxonPaired    | 100000     | True  |  2,737.3 μs |  72.00 μs |  3.95 μs | 171.8750 | 171.8750 | 171.8750 |  800229 B |
| MannWhitneyTest   | 100000     | True  |  3,596.3 μs | 125.09 μs |  6.86 μs |        - |        - |        - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.StatsBenchmarks-report-github

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

| Method              | SampleSize | Mean            | Error           | StdDev        | Ratio   | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|-------------------- |----------- |----------------:|----------------:|--------------:|--------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **LodestarWelchT**      | **100**        |      **1,012.8 ns** |        **57.35 ns** |       **3.14 ns** |    **1.00** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 100        |     37,505.0 ns |     5,093.21 ns |     279.18 ns |   37.03 |    0.26 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 100        |      2,626.0 ns |       341.31 ns |      18.71 ns |    2.59 |    0.02 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 100        |     22,083.0 ns |       186.81 ns |      10.24 ns |   21.80 |    0.06 |   1.3733 |   0.0305 |        - |   23336 B |          NA |
| LodestarChiSquare   | 100        |        113.5 ns |         6.82 ns |       0.37 ns |    0.11 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 100        |        214.9 ns |        14.83 ns |       0.81 ns |    0.21 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
|                     |            |                 |                 |               |         |         |          |          |          |           |             |
| **LodestarWelchT**      | **10000**      |     **42,459.6 ns** |     **1,990.62 ns** |     **109.11 ns** |   **1.000** |    **0.00** |        **-** |        **-** |        **-** |         **-** |          **NA** |
| AccordWelchT        | 10000      |    146,774.6 ns |    18,018.28 ns |     987.64 ns |   3.457 |    0.02 |        - |        - |        - |     392 B |          NA |
| LodestarMannWhitney | 10000      |    464,444.2 ns |   198,586.22 ns |  10,885.18 ns |  10.939 |    0.22 |        - |        - |        - |         - |          NA |
| AccordMannWhitney   | 10000      | 14,026,111.4 ns | 3,390,136.80 ns | 185,824.84 ns | 330.342 |    3.86 | 234.3750 | 234.3750 | 234.3750 | 2241217 B |          NA |
| LodestarChiSquare   | 10000      |        111.1 ns |         7.56 ns |       0.41 ns |   0.003 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |
| AccordChiSquare     | 10000      |        214.7 ns |         5.36 ns |       0.29 ns |   0.005 |    0.00 |   0.0100 |        - |        - |     168 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean         | Error      | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------- |----------- |-------------:|-----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **UnitLoop**           | **1**          |     **5.507 μs** |  **0.8654 μs** | **0.0474 μs** |  **1.00** |    **0.01** |  **0.1297** |      **-** |   **2.23 KB** |        **1.00** |
| EmbedBatch         | 1          |     5.862 μs |  1.0935 μs | 0.0599 μs |  1.06 |    0.01 |  0.1602 |      - |   2.63 KB |        1.18 |
| EmbedBatchBucketed | 1          |     5.806 μs |  1.6576 μs | 0.0909 μs |  1.05 |    0.02 |  0.1602 |      - |   2.63 KB |        1.18 |
|                    |            |              |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **8**          |    **71.101 μs** | **11.5989 μs** | **0.6358 μs** |  **1.00** |    **0.01** |  **1.7090** |      **-** |  **29.16 KB** |        **1.00** |
| EmbedBatch         | 8          |    36.927 μs |  6.8321 μs | 0.3745 μs |  0.52 |    0.01 |  1.3428 |      - |  22.19 KB |        0.76 |
| EmbedBatchBucketed | 8          |    37.018 μs |  4.1949 μs | 0.2299 μs |  0.52 |    0.00 |  1.3428 |      - |  22.19 KB |        0.76 |
|                    |            |              |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **32**         |   **268.689 μs** | **26.6807 μs** | **1.4625 μs** |  **1.00** |    **0.01** |  **6.3477** |      **-** | **109.74 KB** |        **1.00** |
| EmbedBatch         | 32         |   132.484 μs | 13.8094 μs | 0.7569 μs |  0.49 |    0.00 |  4.8828 |      - |  82.35 KB |        0.75 |
| EmbedBatchBucketed | 32         |   117.704 μs |  3.2640 μs | 0.1789 μs |  0.44 |    0.00 |  4.1504 | 0.1221 |  68.84 KB |        0.63 |
|                    |            |              |            |           |       |         |         |        |           |             |
| **UnitLoop**           | **128**        | **1,064.421 μs** | **54.5801 μs** | **2.9917 μs** |  **1.00** |    **0.00** | **25.3906** |      **-** |  **438.9 KB** |        **1.00** |
| EmbedBatch         | 128        |   523.278 μs | 59.0556 μs | 3.2370 μs |  0.49 |    0.00 | 19.5313 | 1.9531 | 328.54 KB |        0.75 |
| EmbedBatchBucketed | 128        |   466.088 μs | 42.2280 μs | 2.3147 μs |  0.44 |    0.00 | 15.6250 | 1.9531 | 261.02 KB |        0.59 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeBenchmarks-report-github

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

| Method             | Radius | Shape     | Mean      | Error     | StdDev   | Ratio | RatioSD | Allocated  | Alloc Ratio |
|------------------- |------- |---------- |----------:|----------:|---------:|------:|--------:|-----------:|------------:|
| **LengthFilteredScan** | **1**      | **clustered** | **149.77 ms** |  **2.724 ms** | **0.149 ms** |  **1.00** |    **0.00** |   **27.25 KB** |        **1.00** |
| TreeWithinDistance | 1      | clustered |  75.27 ms | 22.154 ms | 1.214 ms |  0.50 |    0.01 |  103.71 KB |        3.81 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **1**      | **uniform**   | **155.88 ms** | **58.917 ms** | **3.229 ms** |  **1.00** |    **0.03** |   **23.86 KB** |        **1.00** |
| TreeWithinDistance | 1      | uniform   |  69.83 ms | 22.935 ms | 1.257 ms |  0.45 |    0.01 |  116.49 KB |        4.88 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **clustered** | **236.88 ms** | **62.238 ms** | **3.411 ms** |  **1.00** |    **0.02** |  **103.44 KB** |        **1.00** |
| TreeWithinDistance | 2      | clustered | 275.36 ms | 15.362 ms | 0.842 ms |  1.16 |    0.01 |  259.28 KB |        2.51 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **2**      | **uniform**   | **228.89 ms** | **21.846 ms** | **1.197 ms** |  **1.00** |    **0.01** |   **54.65 KB** |        **1.00** |
| TreeWithinDistance | 2      | uniform   | 238.86 ms |  7.849 ms | 0.430 ms |  1.04 |    0.00 |   192.8 KB |        3.53 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **clustered** | **278.41 ms** |  **2.864 ms** | **0.157 ms** |  **1.00** |    **0.00** |   **949.9 KB** |        **1.00** |
| TreeWithinDistance | 3      | clustered | 391.13 ms | 73.053 ms | 4.004 ms |  1.40 |    0.01 | 1366.63 KB |        1.44 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **3**      | **uniform**   | **283.60 ms** | **11.027 ms** | **0.604 ms** |  **1.00** |    **0.00** |  **741.56 KB** |        **1.00** |
| TreeWithinDistance | 3      | uniform   | 358.21 ms | 25.659 ms | 1.406 ms |  1.26 |    0.00 |  1153.8 KB |        1.56 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **clustered** | **324.86 ms** | **10.634 ms** | **0.583 ms** |  **1.00** |    **0.00** | **5113.42 KB** |        **1.00** |
| TreeWithinDistance | 4      | clustered | 449.58 ms | 17.097 ms | 0.937 ms |  1.38 |    0.00 |  7216.2 KB |        1.41 |
|                    |        |           |           |           |          |       |         |            |             |
| **LengthFilteredScan** | **4**      | **uniform**   | **330.79 ms** |  **1.942 ms** | **0.106 ms** |  **1.00** |    **0.00** | **5514.98 KB** |        **1.00** |
| TreeWithinDistance | 4      | uniform   | 457.32 ms | 49.545 ms | 2.716 ms |  1.38 |    0.01 | 7964.22 KB |        1.44 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BlockedTableBenchmarks-report-github

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
| **Latin**  | **1000**   |      **49.71 μs** |     **0.200 μs** |   **0.011 μs** |         **-** |
| Cjk    | 1000   |      64.21 μs |     1.162 μs |   0.064 μs |         - |
| **Latin**  | **10000**  |   **4,580.49 μs** |   **714.627 μs** |  **39.171 μs** |         **-** |
| Cjk    | 10000  |   7,021.53 μs |   172.053 μs |   9.431 μs |         - |
| **Latin**  | **65536**  | **207,352.14 μs** | **9,524.639 μs** | **522.078 μs** |         **-** |

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

| Method           | Documents | Mean           | Error          | StdDev        | Ratio     | RatioSD | Gen0       | Gen1      | Gen2     | Allocated   | Alloc Ratio  |
|----------------- |---------- |---------------:|---------------:|--------------:|----------:|--------:|-----------:|----------:|---------:|------------:|-------------:|
| **LodestarQuery**    | **1000**      |       **2.193 μs** |      **0.2903 μs** |     **0.0159 μs** |      **1.00** |    **0.01** |     **0.0229** |         **-** |        **-** |       **424 B** |         **1.00** |
| LuceneQuery      | 1000      |       3.238 μs |      0.2137 μs |     0.0117 μs |      1.48 |    0.01 |     0.3128 |         - |        - |      5264 B |        12.42 |
| LodestarFromText | 1000      |  14,225.620 μs |    554.1253 μs |    30.3735 μs |  6,488.40 |   42.66 |  1671.8750 | 1312.5000 | 500.0000 |  21863377 B |    51,564.57 |
| LuceneFromText   | 1000      |   7,471.224 μs |    434.5101 μs |    23.8170 μs |  3,407.67 |   23.47 |    85.9375 |   78.1250 |   7.8125 |   1375080 B |     3,243.11 |
|                  |           |                |                |               |           |         |            |           |          |             |              |
| **LodestarQuery**    | **20000**     |      **26.277 μs** |      **1.7303 μs** |     **0.0948 μs** |      **1.00** |    **0.00** |          **-** |         **-** |        **-** |       **424 B** |         **1.00** |
| LuceneQuery      | 20000     |      18.125 μs |      0.9618 μs |     0.0527 μs |      0.69 |    0.00 |     0.4883 |         - |        - |      8648 B |        20.40 |
| LodestarFromText | 20000     | 300,456.972 μs | 79,965.4305 μs | 4,383.1751 μs | 11,434.38 |  148.83 | 23000.0000 | 6000.0000 | 500.0000 | 428392216 B | 1,010,359.00 |
| LuceneFromText   | 20000     | 148,322.955 μs | 60,398.2417 μs | 3,310.6314 μs |  5,644.67 |  110.53 |  1250.0000 | 1000.0000 |        - |  22417220 B |    52,870.80 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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
| Unigram | 31.81 ms | 3.448 ms | 0.189 ms |  1.00 |    0.01 |  312.5000 |   5.43 MB |        1.00 |
| Bpe     | 85.27 ms | 7.635 ms | 0.419 ms |  2.68 |    0.02 | 1666.6667 |  28.47 MB |        5.24 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean      | Error      | StdDev    | Gen0   | Allocated |
|-------------------------- |------- |----------:|-----------:|----------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **19.81 μs** |   **0.834 μs** |  **0.046 μs** | **0.4578** |   **7.48 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |  **38.89 μs** |   **6.564 μs** |  **0.360 μs** | **0.8545** |  **14.53 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |  **81.01 μs** |   **9.914 μs** |  **0.543 μs** | **1.7090** |  **28.58 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **323.74 μs** | **711.746 μs** | **39.013 μs** | **3.4180** |  **56.63 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeWordCacheBenchmarks-report-github

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
| EncodeUnseenProse | 19.95 ms | 61.85 ms | 3.390 ms |    7.5 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BucketRouteDiagnostics-report-github

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

| Method     | Alphabet | Mean      | Error     | StdDev   | Allocated |
|----------- |--------- |----------:|----------:|---------:|----------:|
| **DpGroup**    | **cjk**      |  **19.35 μs** |  **0.690 μs** | **0.038 μs** |         **-** |
| MyersGroup | cjk      | 168.56 μs | 13.216 μs | 0.724 μs |         - |
| **DpGroup**    | **latin**    |  **10.71 μs** |  **3.085 μs** | **0.169 μs** |         **-** |
| MyersGroup | latin    | 111.98 μs |  3.838 μs | 0.210 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzBenchmarks-report-github

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

| Method         | Mean       | Error       | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |-----------:|------------:|---------:|------:|--------:|-------:|----------:|------------:|
| Ratio          |   109.6 ns |     6.15 ns |  0.34 ns |  1.00 |    0.00 |      - |         - |          NA |
| PartialRatio   |   764.6 ns |    38.36 ns |  2.10 ns |  6.98 |    0.02 |      - |         - |          NA |
| TokenSortRatio |   939.3 ns |   308.61 ns | 16.92 ns |  8.57 |    0.14 | 0.0782 |    1312 B |          NA |
| TokenSetRatio  | 1,167.1 ns |   435.64 ns | 23.88 ns | 10.65 |    0.19 | 0.0858 |    1448 B |          NA |
| WRatio         | 2,239.5 ns | 1,059.49 ns | 58.07 ns | 20.44 |    0.46 | 0.1640 |    2760 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FuzzIncumbentBenchmarks-report-github

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
| **Lodestar**   | **Ratio**         |    **108.5 ns** |     **2.08 ns** |  **0.11 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| FuzzySharp | Ratio         |    228.1 ns |    33.70 ns |  1.85 ns |  2.10 |    0.01 | 0.0048 |      80 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **PartialRatio**  |    **767.9 ns** |    **58.26 ns** |  **3.19 ns** |  **1.00** |    **0.01** |      **-** |         **-** |          **NA** |
| FuzzySharp | PartialRatio  | 10,114.6 ns |   502.26 ns | 27.53 ns | 13.17 |    0.06 |      - |     160 B |          NA |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **TokenSetRatio** |  **1,126.6 ns** |    **39.48 ns** |  **2.16 ns** |  **1.00** |    **0.00** | **0.0858** |    **1448 B** |        **1.00** |
| FuzzySharp | TokenSetRatio |  1,992.3 ns |   205.50 ns | 11.26 ns |  1.77 |    0.01 | 0.1144 |    1944 B |        1.34 |
|            |               |             |             |          |       |         |        |           |             |
| **Lodestar**   | **WRatio**        |  **2,293.1 ns** |   **771.76 ns** | **42.30 ns** |  **1.00** |    **0.02** | **0.1640** |    **2760 B** |        **1.00** |
| FuzzySharp | WRatio        |  4,924.6 ns | 1,343.02 ns | 73.62 ns |  2.15 |    0.04 | 0.1831 |    3096 B |        1.12 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

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
| **Distance_Utf16**             | **8**      |    **29.51 ns** |   **0.348 ns** |  **0.019 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 8      |    40.62 ns |   1.331 ns |  0.073 ns |  1.38 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |    31.08 ns |   1.380 ns |  0.076 ns |  1.05 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 8      |    28.68 ns |   0.181 ns |  0.010 ns |  0.97 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **12**     |    **31.52 ns** |   **4.774 ns** |  **0.262 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 12     |    42.20 ns |   0.221 ns |  0.012 ns |  1.34 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 12     |    33.16 ns |   0.122 ns |  0.007 ns |  1.05 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 12     |    32.82 ns |   2.306 ns |  0.126 ns |  1.04 |    0.01 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **16**     |    **35.25 ns** |   **1.925 ns** |  **0.105 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 16     |    45.24 ns |   1.160 ns |  0.064 ns |  1.28 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 16     |    34.05 ns |   0.575 ns |  0.032 ns |  0.97 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 16     |    32.87 ns |   0.369 ns |  0.020 ns |  0.93 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **20**     |    **39.02 ns** |   **4.359 ns** |  **0.239 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Distance_CodePoint         | 20     |    48.44 ns |   0.441 ns |  0.024 ns |  1.24 |    0.01 |         - |          NA |
| NormalizedSimilarity_Utf16 | 20     |    38.62 ns |   3.504 ns |  0.192 ns |  0.99 |    0.01 |         - |          NA |
| SubsequenceLength_Utf16    | 20     |    35.44 ns |  26.131 ns |  1.432 ns |  0.91 |    0.03 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **24**     |    **65.06 ns** |  **50.886 ns** |  **2.789 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_CodePoint         | 24     |    68.10 ns |   5.561 ns |  0.305 ns |  1.05 |    0.04 |         - |          NA |
| NormalizedSimilarity_Utf16 | 24     |    61.52 ns |   0.489 ns |  0.027 ns |  0.95 |    0.04 |         - |          NA |
| SubsequenceLength_Utf16    | 24     |    56.44 ns |   3.429 ns |  0.188 ns |  0.87 |    0.03 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **32**     |    **69.82 ns** |  **24.810 ns** |  **1.360 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_CodePoint         | 32     |    75.20 ns |   0.924 ns |  0.051 ns |  1.08 |    0.02 |         - |          NA |
| NormalizedSimilarity_Utf16 | 32     |    75.54 ns |   2.474 ns |  0.136 ns |  1.08 |    0.02 |         - |          NA |
| SubsequenceLength_Utf16    | 32     |    65.91 ns |   0.762 ns |  0.042 ns |  0.94 |    0.02 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **128**    |   **408.18 ns** |  **19.945 ns** |  **1.093 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 128    |   422.67 ns |  25.006 ns |  1.371 ns |  1.04 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |   407.28 ns |  33.250 ns |  1.823 ns |  1.00 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 128    |   403.86 ns |  14.307 ns |  0.784 ns |  0.99 |    0.00 |         - |          NA |
|                            |        |             |            |           |       |         |           |             |
| **Distance_Utf16**             | **512**    | **5,064.45 ns** | **124.220 ns** |  **6.809 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_CodePoint         | 512    | 5,040.09 ns |  41.752 ns |  2.289 ns |  1.00 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 5,017.49 ns |  50.517 ns |  2.769 ns |  0.99 |    0.00 |         - |          NA |
| SubsequenceLength_Utf16    | 512    | 4,991.88 ns | 185.826 ns | 10.186 ns |  0.99 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.IndelCodePointBenchmarks-report-github

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
| **Distance_CodePoint** | **20**     |    **409.6 ns** |  **19.41 ns** |  **1.06 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 20     |    216.2 ns |   2.92 ns |  0.16 ns |  0.53 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **128**    |  **2,444.8 ns** |  **16.42 ns** |  **0.90 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    |  5,327.2 ns | 121.95 ns |  6.68 ns |  2.18 |         - |          NA |
|                    |        |             |           |          |       |           |             |
| **Distance_CodePoint** | **512**    | **14,478.1 ns** | **581.95 ns** | **31.90 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 38,245.6 ns | 199.12 ns | 10.91 ns |  2.64 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LcsGateBenchmarks-report-github

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
| **Dp**         | **8**    |    **130.01 ns** |     **2.097 ns** |   **0.115 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     58.98 ns |     5.741 ns |   0.315 ns |  0.45 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    130.25 ns |     9.949 ns |   0.545 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    107.09 ns |     2.608 ns |   0.143 ns |  0.82 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **12**   |    **228.62 ns** |   **234.804 ns** |  **12.870 ns** |  **1.00** |    **0.07** |         **-** |          **NA** |
| Kernel     | 12   |     69.18 ns |     1.281 ns |   0.070 ns |  0.30 |    0.01 |         - |          NA |
| Dp_Cjk     | 12   |    226.16 ns |   172.355 ns |   9.447 ns |  0.99 |    0.06 |         - |          NA |
| Kernel_Cjk | 12   |    119.75 ns |    22.755 ns |   1.247 ns |  0.52 |    0.03 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **14**   |    **289.23 ns** |   **269.986 ns** |  **14.799 ns** |  **1.00** |    **0.06** |         **-** |          **NA** |
| Kernel     | 14   |     73.45 ns |     2.507 ns |   0.137 ns |  0.25 |    0.01 |         - |          NA |
| Dp_Cjk     | 14   |    290.13 ns |   323.634 ns |  17.739 ns |  1.00 |    0.07 |         - |          NA |
| Kernel_Cjk | 14   |    125.46 ns |     1.807 ns |   0.099 ns |  0.43 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **16**   |    **378.39 ns** |   **508.745 ns** |  **27.886 ns** |  **1.00** |    **0.09** |         **-** |          **NA** |
| Kernel     | 16   |     78.35 ns |     0.827 ns |   0.045 ns |  0.21 |    0.01 |         - |          NA |
| Dp_Cjk     | 16   |    348.59 ns |    19.244 ns |   1.055 ns |  0.92 |    0.06 |         - |          NA |
| Kernel_Cjk | 16   |    130.39 ns |    13.197 ns |   0.723 ns |  0.35 |    0.02 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **18**   |    **749.94 ns** |   **412.964 ns** |  **22.636 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 18   |     85.86 ns |    14.659 ns |   0.803 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 18   |    716.68 ns |   543.768 ns |  29.806 ns |  0.96 |    0.04 |         - |          NA |
| Kernel_Cjk | 18   |    134.82 ns |     1.130 ns |   0.062 ns |  0.18 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **20**   |    **817.56 ns** |    **74.372 ns** |   **4.077 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 20   |     89.77 ns |     1.146 ns |   0.063 ns |  0.11 |    0.00 |         - |          NA |
| Dp_Cjk     | 20   |    825.84 ns |   117.406 ns |   6.435 ns |  1.01 |    0.01 |         - |          NA |
| Kernel_Cjk | 20   |    142.30 ns |     1.120 ns |   0.061 ns |  0.17 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **24**   |  **1,012.47 ns** |   **615.589 ns** |  **33.743 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 24   |     97.55 ns |     0.255 ns |   0.014 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |  1,062.52 ns |   892.435 ns |  48.917 ns |  1.05 |    0.05 |         - |          NA |
| Kernel_Cjk | 24   |    155.04 ns |     3.504 ns |   0.192 ns |  0.15 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **32**   |  **1,629.33 ns** |   **863.585 ns** |  **47.336 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 32   |    118.03 ns |    29.284 ns |   1.605 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,637.79 ns |   651.850 ns |  35.730 ns |  1.01 |    0.03 |         - |          NA |
| Kernel_Cjk | 32   |    180.84 ns |     5.628 ns |   0.308 ns |  0.11 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **48**   |  **3,180.62 ns** | **1,634.501 ns** |  **89.593 ns** |  **1.00** |    **0.03** |         **-** |          **NA** |
| Kernel     | 48   |    145.27 ns |    26.019 ns |   1.426 ns |  0.05 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,185.73 ns | 1,159.730 ns |  63.569 ns |  1.00 |    0.03 |         - |          NA |
| Kernel_Cjk | 48   |    259.22 ns |     2.223 ns |   0.122 ns |  0.08 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **64**   |  **5,232.71 ns** | **2,977.975 ns** | **163.233 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 64   |    171.59 ns |     3.090 ns |   0.169 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,351.22 ns |   292.688 ns |  16.043 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 64   |    312.50 ns |     7.008 ns |   0.384 ns |  0.06 |    0.00 |         - |          NA |
|            |      |              |              |            |       |         |           |             |
| **Dp**         | **96**   | **11,035.90 ns** |   **931.705 ns** |  **51.070 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 96   |    362.86 ns |     3.482 ns |   0.191 ns |  0.03 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 11,312.56 ns | 3,400.719 ns | 186.405 ns |  1.03 |    0.02 |         - |          NA |
| Kernel_Cjk | 96   |    964.80 ns |     2.737 ns |   0.150 ns |  0.09 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinBenchmarks-report-github

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

| Method                     | Length | Mean         | Error        | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |-------------:|-------------:|-----------:|------:|--------:|----------:|------------:|
| **Distance_Utf16**             | **8**      |     **26.99 ns** |     **0.255 ns** |   **0.014 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 8      |     26.97 ns |     0.914 ns |   0.050 ns |  1.00 |    0.00 |         - |          NA |
| Distance_CodePoint         | 8      |    128.04 ns |    32.382 ns |   1.775 ns |  4.74 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 8      |     28.23 ns |     0.951 ns |   0.052 ns |  1.05 |    0.00 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **64**     |    **315.96 ns** |    **90.425 ns** |   **4.956 ns** |  **1.00** |    **0.02** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 64     |    402.72 ns |     5.738 ns |   0.315 ns |  1.27 |    0.02 |         - |          NA |
| Distance_CodePoint         | 64     |    707.92 ns |    29.111 ns |   1.596 ns |  2.24 |    0.03 |         - |          NA |
| NormalizedSimilarity_Utf16 | 64     |    312.80 ns |    14.039 ns |   0.770 ns |  0.99 |    0.01 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **128**    |  **1,035.64 ns** |   **752.123 ns** |  **41.226 ns** |  **1.00** |    **0.05** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 128    |  2,006.27 ns |    24.528 ns |   1.344 ns |  1.94 |    0.07 |         - |          NA |
| Distance_CodePoint         | 128    |  1,698.55 ns |    11.011 ns |   0.604 ns |  1.64 |    0.06 |         - |          NA |
| NormalizedSimilarity_Utf16 | 128    |    997.28 ns |     5.163 ns |   0.283 ns |  0.96 |    0.03 |         - |          NA |
|                            |        |              |              |            |       |         |           |             |
| **Distance_Utf16**             | **512**    | **12,642.79 ns** |   **652.050 ns** |  **35.741 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Distance_Utf16_Cjk         | 512    | 18,206.98 ns | 2,904.489 ns | 159.205 ns |  1.44 |    0.01 |         - |          NA |
| Distance_CodePoint         | 512    | 14,910.31 ns |   508.937 ns |  27.897 ns |  1.18 |    0.00 |         - |          NA |
| NormalizedSimilarity_Utf16 | 512    | 12,647.97 ns | 2,118.756 ns | 116.136 ns |  1.00 |    0.01 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinCodePointBenchmarks-report-github

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

| Method             | Length | Distinct | Mean         | Error        | StdDev      | Ratio | Allocated | Alloc Ratio |
|------------------- |------- |--------- |-------------:|-------------:|------------:|------:|----------:|------------:|
| **Distance_CodePoint** | **16**     | **32**       |     **354.0 ns** |     **12.96 ns** |     **0.71 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 32       |     255.0 ns |      9.90 ns |     0.54 ns |  0.72 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **16**     | **512**      |     **351.5 ns** |     **11.70 ns** |     **0.64 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 16     | 512      |     257.6 ns |      7.54 ns |     0.41 ns |  0.73 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **24**     | **32**       |     **455.5 ns** |     **38.53 ns** |     **2.11 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 32       |     350.9 ns |      4.06 ns |     0.22 ns |  0.77 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **24**     | **512**      |     **452.5 ns** |     **11.88 ns** |     **0.65 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 24     | 512      |     346.9 ns |      2.74 ns |     0.15 ns |  0.77 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **32**     | **32**       |     **549.0 ns** |     **19.61 ns** |     **1.08 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 32       |     446.3 ns |     28.81 ns |     1.58 ns |  0.81 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **32**     | **512**      |     **537.4 ns** |      **9.51 ns** |     **0.52 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 32     | 512      |     446.6 ns |      3.52 ns |     0.19 ns |  0.83 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **40**     | **32**       |     **633.7 ns** |     **29.89 ns** |     **1.64 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 32       |   1,394.3 ns |      6.63 ns |     0.36 ns |  2.20 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **40**     | **512**      |     **637.2 ns** |     **54.99 ns** |     **3.01 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 40     | 512      |   1,289.5 ns |      2.89 ns |     0.16 ns |  2.02 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **128**    | **32**       |   **2,002.3 ns** |     **31.30 ns** |     **1.72 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 32       |   5,727.6 ns |    110.83 ns |     6.08 ns |  2.86 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **128**    | **512**      |   **2,045.6 ns** |    **168.82 ns** |     **9.25 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 128    | 512      |   5,694.1 ns |    184.66 ns |    10.12 ns |  2.78 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **512**    | **32**       |  **16,645.4 ns** |    **211.82 ns** |    **11.61 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 32       |  69,654.1 ns |    816.44 ns |    44.75 ns |  4.18 |         - |          NA |
|                    |        |          |              |              |             |       |           |             |
| **Distance_CodePoint** | **512**    | **512**      | **433,973.2 ns** | **33,142.67 ns** | **1,816.66 ns** |  **1.00** |         **-** |          **NA** |
| Distance_Utf16     | 512    | 512      |  70,351.2 ns | 12,011.47 ns |   658.39 ns |  0.16 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.LevenshteinIncumbentBenchmarks-report-github

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

| Method               | Length | Mean          | Error          | StdDev        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------- |--------------:|---------------:|--------------:|------:|--------:|-------:|----------:|------------:|
| **Lodestar**             | **8**      |      **26.94 ns** |       **0.119 ns** |      **0.007 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 8      |      86.70 ns |      12.092 ns |      0.663 ns |  3.22 |    0.02 | 0.0033 |      56 B |          NA |
| Quickenshtein        | 8      |      82.42 ns |       4.566 ns |      0.250 ns |  3.06 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 8      |     171.13 ns |       5.719 ns |      0.313 ns |  6.35 |    0.01 | 0.0076 |     128 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **64**     |     **312.26 ns** |      **11.060 ns** |      **0.606 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 64     |   5,945.32 ns |   3,475.021 ns |    190.478 ns | 19.04 |    0.53 | 0.0153 |     280 B |          NA |
| Quickenshtein        | 64     |   1,356.55 ns |      51.538 ns |      2.825 ns |  4.34 |    0.01 |      - |         - |          NA |
| F23_StringSimilarity | 64     |   9,191.39 ns |     632.085 ns |     34.647 ns | 29.44 |    0.11 | 0.0305 |     576 B |          NA |
|                      |        |               |                |               |       |         |        |           |             |
| **Lodestar**             | **512**    |  **12,400.03 ns** |     **620.575 ns** |     **34.016 ns** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| Fastenshtein         | 512    | 487,347.64 ns | 560,322.415 ns | 30,713.162 ns | 39.30 |    2.15 |      - |    2072 B |          NA |
| Quickenshtein        | 512    |  35,254.04 ns |  12,045.629 ns |    660.262 ns |  2.84 |    0.05 |      - |         - |          NA |
| F23_StringSimilarity | 512    | 803,283.96 ns | 286,226.772 ns | 15,689.055 ns | 64.78 |    1.11 |      - |    4161 B |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MyersGateBenchmarks-report-github

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
| **Dp**         | **4**    |     **82.41 ns** |     **6.627 ns** |  **0.363 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 4    |     81.76 ns |     5.998 ns |  0.329 ns |  0.99 |    0.01 |         - |          NA |
| Dp_Cjk     | 4    |     84.47 ns |     1.141 ns |  0.063 ns |  1.03 |    0.00 |         - |          NA |
| Kernel_Cjk | 4    |     80.40 ns |     0.756 ns |  0.041 ns |  0.98 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **6**    |    **116.55 ns** |     **5.045 ns** |  **0.277 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 6    |     92.55 ns |     1.726 ns |  0.095 ns |  0.79 |    0.00 |         - |          NA |
| Dp_Cjk     | 6    |    118.69 ns |    34.761 ns |  1.905 ns |  1.02 |    0.01 |         - |          NA |
| Kernel_Cjk | 6    |    147.71 ns |     7.168 ns |  0.393 ns |  1.27 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **8**    |    **157.39 ns** |     **8.865 ns** |  **0.486 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 8    |     99.48 ns |     0.124 ns |  0.007 ns |  0.63 |    0.00 |         - |          NA |
| Dp_Cjk     | 8    |    153.54 ns |    12.304 ns |  0.674 ns |  0.98 |    0.00 |         - |          NA |
| Kernel_Cjk | 8    |    185.34 ns |    55.127 ns |  3.022 ns |  1.18 |    0.02 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **10**   |    **206.69 ns** |    **13.957 ns** |  **0.765 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 10   |    109.05 ns |     0.886 ns |  0.049 ns |  0.53 |    0.00 |         - |          NA |
| Dp_Cjk     | 10   |    207.16 ns |     2.063 ns |  0.113 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 10   |    168.76 ns |    17.823 ns |  0.977 ns |  0.82 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **12**   |    **269.37 ns** |    **23.609 ns** |  **1.294 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 12   |    118.93 ns |     2.906 ns |  0.159 ns |  0.44 |    0.00 |         - |          NA |
| Dp_Cjk     | 12   |    269.84 ns |    40.244 ns |  2.206 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 12   |    215.64 ns |     4.102 ns |  0.225 ns |  0.80 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **16**   |    **431.02 ns** |    **38.184 ns** |  **2.093 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 16   |    136.79 ns |     1.015 ns |  0.056 ns |  0.32 |    0.00 |         - |          NA |
| Dp_Cjk     | 16   |    438.08 ns |   239.096 ns | 13.106 ns |  1.02 |    0.03 |         - |          NA |
| Kernel_Cjk | 16   |    202.51 ns |     0.322 ns |  0.018 ns |  0.47 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **24**   |    **863.68 ns** |    **21.604 ns** |  **1.184 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 24   |    180.32 ns |    11.096 ns |  0.608 ns |  0.21 |    0.00 |         - |          NA |
| Dp_Cjk     | 24   |    863.56 ns |    13.021 ns |  0.714 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 24   |    246.62 ns |     4.559 ns |  0.250 ns |  0.29 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **32**   |  **1,509.91 ns** |   **849.392 ns** | **46.558 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| Kernel     | 32   |    219.44 ns |     3.735 ns |  0.205 ns |  0.15 |    0.00 |         - |          NA |
| Dp_Cjk     | 32   |  1,486.33 ns |    37.016 ns |  2.029 ns |  0.98 |    0.03 |         - |          NA |
| Kernel_Cjk | 32   |    298.40 ns |    54.156 ns |  2.968 ns |  0.20 |    0.01 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **48**   |  **3,236.15 ns** |   **140.225 ns** |  **7.686 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 48   |    322.26 ns |   184.919 ns | 10.136 ns |  0.10 |    0.00 |         - |          NA |
| Dp_Cjk     | 48   |  3,246.45 ns |   180.702 ns |  9.905 ns |  1.00 |    0.00 |         - |          NA |
| Kernel_Cjk | 48   |    393.64 ns |     1.971 ns |  0.108 ns |  0.12 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **64**   |  **5,686.13 ns** | **1,085.499 ns** | **59.500 ns** |  **1.00** |    **0.01** |         **-** |          **NA** |
| Kernel     | 64   |    382.54 ns |    37.560 ns |  2.059 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 64   |  5,672.25 ns |   168.302 ns |  9.225 ns |  1.00 |    0.01 |         - |          NA |
| Kernel_Cjk | 64   |    486.41 ns |    26.594 ns |  1.458 ns |  0.09 |    0.00 |         - |          NA |
|            |      |              |              |           |       |         |           |             |
| **Dp**         | **96**   | **12,663.16 ns** |   **791.018 ns** | **43.358 ns** |  **1.00** |    **0.00** |         **-** |          **NA** |
| Kernel     | 96   |    853.35 ns |    68.296 ns |  3.744 ns |  0.07 |    0.00 |         - |          NA |
| Dp_Cjk     | 96   | 12,580.26 ns |   951.903 ns | 52.177 ns |  0.99 |    0.00 |         - |          NA |
| Kernel_Cjk | 96   |  1,550.24 ns |   124.609 ns |  6.830 ns |  0.12 |    0.00 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartialRatioLongNeedleBenchmarks-report-github

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
| **Embedded**    | **65**           |    **10.143 μs** |  **1.1481 μs** | **0.0629 μs** |         **-** |
| EqualLength | 65           |     2.924 μs |  0.0965 μs | 0.0053 μs |         - |
| **Embedded**    | **128**          |    **36.348 μs** |  **8.3368 μs** | **0.4570 μs** |         **-** |
| EqualLength | 128          |     5.474 μs |  0.1575 μs | 0.0086 μs |         - |
| **Embedded**    | **512**          | **1,430.955 μs** | **21.6744 μs** | **1.1880 μs** |         **-** |
| EqualLength | 512          |    47.506 μs |  1.1868 μs | 0.0651 μs |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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
| VocabTxt               |  4.217 ms |  3.9065 ms | 0.2141 ms | 109.3750 | 101.5625 |  31.2500 |  3711.57 KB |
| TokenizerJsonWordPiece | 11.369 ms |  3.8686 ms | 0.2121 ms | 187.5000 | 171.8750 |  46.8750 |  5852.29 KB |
| TokenizerJsonUnigram   | 11.172 ms |  0.8819 ms | 0.0483 ms |  78.1250 |  62.5000 |  15.6250 |  4748.81 KB |
| SpieceModel            |  4.202 ms |  1.8040 ms | 0.0989 ms | 109.3750 | 101.5625 |  31.2500 |  3440.04 KB |
| TfidfSave              |  1.734 ms |  0.0524 ms | 0.0029 ms |  21.4844 |  15.6250 |  15.6250 |  2137.04 KB |
| TfidfLoad              |  4.501 ms |  0.4148 ms | 0.0227 ms |  85.9375 |  78.1250 |  23.4375 |  2930.47 KB |
| EmbeddingIndexSave     |  4.179 ms |  0.5761 ms | 0.0316 ms | 187.5000 | 187.5000 | 187.5000 | 20349.83 KB |
| EmbeddingIndexLoad     |  4.887 ms |  1.2393 ms | 0.0679 ms | 179.6875 | 148.4375 | 117.1875 | 16094.41 KB |
| EmbeddingIndexSaveFile | 50.781 ms | 26.0034 ms | 1.4253 ms |        - |        - |        - |   323.49 KB |
| EmbeddingIndexLoadGzip | 75.405 ms |  3.5722 ms | 0.1958 ms |        - |        - |        - | 16095.21 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error      | StdDev    | Ratio | RatioSD | Gen0      | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|-----------:|----------:|------:|--------:|----------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **35.59 ms** |   **0.449 ms** |  **0.025 ms** |  **1.00** |    **0.00** |  **533.3333** |   **8.71 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.88 ms |   6.629 ms |  0.363 ms |  1.51 |    0.01 |  200.0000 |   3.55 MB |        0.41 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **SentencePiece** |  **48.01 ms** |  **11.200 ms** |  **0.614 ms** |  **1.00** |    **0.02** |  **272.7273** |   **5.44 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  57.66 ms |   2.200 ms |  0.121 ms |  1.20 |    0.01 |  100.0000 |   3.09 MB |        0.57 |
|              |               |           |            |           |       |         |           |           |             |
| **Lodestar**     | **ByteLevelBpe**  |  **89.21 ms** |  **80.183 ms** |  **4.395 ms** |  **1.00** |    **0.06** | **1666.6667** |  **28.47 MB** |        **1.00** |
| MlTokenizers | ByteLevelBpe  | 262.65 ms | 263.549 ms | 14.446 ms |  2.95 |    0.19 | 3500.0000 |  59.08 MB |        2.08 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorMathBenchmarks-report-github

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
| **Dot**    | **384**  |  **51.38 ns** | **5.484 ns** | **0.301 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 384  |  48.39 ns | 0.387 ns | 0.021 ns |  0.94 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **768**  |  **99.50 ns** | **1.809 ns** | **0.099 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 768  |  90.73 ns | 0.915 ns | 0.050 ns |  0.91 |         - |          NA |
|        |      |           |          |          |       |           |             |
| **Dot**    | **1024** | **133.63 ns** | **2.293 ns** | **0.126 ns** |  **1.00** |         **-** |          **NA** |
| L2Norm | 1024 | 124.45 ns | 1.499 ns | 0.082 ns |  0.93 |         - |          NA |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerBenchmarks-report-github

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

| Method       | Documents | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------- |---------- |----------:|----------:|----------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| **Count**        | **200**       |  **3.201 ms** | **6.2042 ms** | **0.3401 ms** |  **1.01** |    **0.13** |  **93.7500** |  **39.0625** |        **-** |    **1.6 MB** |        **1.00** |
| Tfidf        | 200       |  3.174 ms | 4.4341 ms | 0.2430 ms |  1.00 |    0.11 | 101.5625 |  39.0625 |        - |   1.63 MB |        1.02 |
| CountBigrams | 200       |  3.876 ms | 0.3352 ms | 0.0184 ms |  1.22 |    0.11 | 171.8750 | 109.3750 |        - |   2.78 MB |        1.74 |
| Hashing      | 200       |  3.054 ms | 1.7984 ms | 0.0986 ms |  0.96 |    0.09 |  93.7500 |  23.4375 |        - |    1.6 MB |        1.00 |
|              |           |           |           |           |       |         |          |          |          |           |             |
| **Count**        | **1000**      |  **7.119 ms** | **2.0032 ms** | **0.1098 ms** |  **1.00** |    **0.02** | **492.1875** | **351.5625** |  **70.3125** |   **7.83 MB** |        **1.00** |
| Tfidf        | 1000      |  7.324 ms | 0.8271 ms | 0.0453 ms |  1.03 |    0.01 | 484.3750 | 312.5000 |  93.7500 |   7.97 MB |        1.02 |
| CountBigrams | 1000      | 11.882 ms | 0.0168 ms | 0.0009 ms |  1.67 |    0.02 | 906.2500 | 375.0000 | 265.6250 |  13.42 MB |        1.71 |
| Hashing      | 1000      |  7.207 ms | 1.1576 ms | 0.0635 ms |  1.01 |    0.02 | 492.1875 | 156.2500 |  70.3125 |   7.85 MB |        1.00 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorizerIncumbentBenchmarks-report-github

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
| **Lodestar** | **200**       |   **7.595 ms** |   **4.285 ms** | **0.2349 ms** |  **1.00** |    **0.04** |   **296.8750** |   **218.7500** |   **140.6250** |   **5.13 MB** |        **1.00** |
| MlNet    | 200       |  51.215 ms |  59.384 ms | 3.2550 ms |  6.75 |    0.41 |  7500.0000 |  7500.0000 |  7500.0000 |  28.27 MB |        5.52 |
|          |           |            |            |           |       |         |            |            |            |           |             |
| **Lodestar** | **1000**      |  **31.126 ms** |   **4.238 ms** | **0.2323 ms** |  **1.00** |    **0.01** |  **2625.0000** |  **2437.5000** |  **1500.0000** |  **24.92 MB** |        **1.00** |
| MlNet    | 1000      | 440.866 ms | 137.301 ms | 7.5260 ms | 14.16 |    0.23 | 77000.0000 | 77000.0000 | 77000.0000 | 324.68 MB |       13.03 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `levenshtein`
- `indel`
- `stats`
- `ols`

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.14)
C#:     Lodestar.Text on .NET 10.0.12 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 110.7 | 23.5 | 4.71x C# faster |
| latin | 32 | 159.1 | 68.5 | 2.32x C# faster |
| latin | 128 | 481.9 | 407.4 | 1.18x C# faster |
| latin | 512 | 4854.9 | 5332.5 | 1.10x Py faster |
| cjk | 8 | 143.0 | 23.4 | 6.11x C# faster |
| cjk | 32 | 232.5 | 139.6 | 1.67x C# faster |
| cjk | 128 | 2039.2 | 1557.9 | 1.31x C# faster |
| cjk | 512 | 16784.1 | 9376.1 | 1.79x C# faster |

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
| latin | 8 | 135.8 | 18.3 | 7.41x C# faster |
| latin | 32 | 255.0 | 127.9 | 1.99x C# faster |
| latin | 128 | 1810.4 | 964.2 | 1.88x C# faster |
| latin | 512 | 15621.4 | 13378.6 | 1.17x C# faster |
| cjk | 8 | 138.7 | 18.4 | 7.52x C# faster |
| cjk | 32 | 296.4 | 190.5 | 1.56x C# faster |
| cjk | 128 | 3036.2 | 2173.2 | 1.40x C# faster |
| cjk | 512 | 26227.6 | 18961.0 | 1.38x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.241 | 2.541 | 10.53x | 0.241 | 2.541 | 10.53x |
| ols_summary_n10000 | 2.709 | 11.324 | 4.18x | 2.836 | 11.323 | 3.99x |
| ols_summary_n100000 | 24.823 | 105.436 | 4.25x | 25.293 | 420.872 | 16.64x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.004 | 0.635 | 141.39x | 0.004 | 0.635 | 141.37x |
| mann_whitney_n1000 | 0.031 | 0.601 | 19.34x | 0.031 | 0.600 | 19.34x |
| chi_square_n1000 | 0.000 | 0.255 | 1381.19x | 0.000 | 0.255 | 1381.10x |
| welch_t_n10000 | 0.042 | 0.672 | 15.87x | 0.042 | 0.672 | 15.87x |
| mann_whitney_n10000 | 0.493 | 3.006 | 6.10x | 0.493 | 3.006 | 6.10x |
| chi_square_n10000 | 0.001 | 0.259 | 196.71x | 0.001 | 0.259 | 196.71x |
| welch_t_n100000 | 0.423 | 1.107 | 2.62x | 0.423 | 1.107 | 2.62x |
| mann_whitney_n100000 | 6.112 | 30.708 | 5.02x | 6.112 | 30.704 | 5.02x |
| chi_square_n100000 | 0.014 | 0.277 | 19.28x | 0.014 | 0.277 | 19.28x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Selected, and not reached tonight

The run stops starting classes when the budget left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are selected again tomorrow whether or not anything else changes.

- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`
- `CoxBenchmarks`
- `GlmBenchmarks`
- `GlmPoissonBenchmarks`
- `OlsBenchmarks`
- `QuantileBenchmarks`
- `RobustCovarianceBenchmarks`
- `SerialCorrelationBenchmarks`
- `StationarityBenchmarks`
- `SurvivalBenchmarks`
- `TiledCosineTopKBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds of [decision 0126](../decisions/0126-the-nightly-reports-a-ratio-that-steps-past-its-noise-or-drifts-over-ten-days.md). A ratio moves when either side of it does: read its baseline before calling a movement a regression.

**New tonight: 14.** Each of these had not moved on its previous reading.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `IndelBenchmarks` | Length=512 | `Distance_CodePoint` | step | 1 | 44.42 | -98% | 56% |
| `IndelBenchmarks` | Length=32 | `Distance_CodePoint` | step | 1.08 | 16.61 | -93% | 41% |
| `IndelBenchmarks` | Length=24 | `Distance_CodePoint` | step | 1.05 | 12.94 | -92% | 61% |
| `IndelBenchmarks` | Length=20 | `Distance_CodePoint` | step | 1.24 | 5.22 | -76% | 30% |
| `IndelBenchmarks` | Length=16 | `Distance_CodePoint` | step | 1.28 | 5.3 | -76% | 43% |
| `IndelBenchmarks` | Length=12 | `Distance_CodePoint` | step | 1.34 | 5.26 | -75% | 39% |
| `IndelBenchmarks` | Length=8 | `Distance_CodePoint` | step | 1.38 | 4.82 | -71% | 31% |
| `LevenshteinIncumbentBenchmarks` | Length=512 | `Fastenshtein` | step | 39.3 | 27.97 | +41% | 30% |
| `LevenshteinCodePointBenchmarks` | Length=128, Distinct=32 | `Distance_Utf16` | step | 2.86 | 2.14 | +34% | 30% |
| `LcsGateBenchmarks` | Band=18 | `Kernel_Cjk` | drift | 0.18 | 0.28 | -32% | 20% |
| `LevenshteinCodePointBenchmarks` | Length=128, Distinct=512 | `Distance_Utf16` | step | 2.78 | 2.11 | +32% | 30% |
| `LevenshteinIncumbentBenchmarks` | Length=512 | `F23_StringSimilarity` | step | 64.78 | 49.57 | +31% | 30% |
| `LcsGateBenchmarks` | Band=18 | `Kernel` | drift | 0.11 | 0.17 | -29% | 20% |
| `MyersGateBenchmarks` | Band=64 | `Kernel_Cjk` | drift | 0.09 | 0.07 | +29% | 20% |

**Still away from their median: 8.** These moved on an earlier run, and their median has not caught up yet.

| class | parameters | method | kind | tonight | against | change | threshold |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| `FuzzIncumbentBenchmarks` | Operation=PartialRatio | `FuzzySharp` | step | 13.17 | 0.89 | +1380% | 55% |
| `TokenizerIncumbentBenchmarks` | Model=SentencePiece | `MlTokenizers` | step | 1.2 | 0.15 | +700% | 30% |
| `IndelBenchmarks` | Length=128 | `Distance_CodePoint` | step | 1.04 | 23.2 | -96% | 30% |
| `FuzzBenchmarks` | — | `PartialRatio` | step | 6.98 | 115.57 | -94% | 74% |
| `TokenizerIncumbentBenchmarks` | Model=WordPiece | `MlTokenizers` | step | 1.51 | 0.85 | +78% | 30% |
| `BpeBenchmarks` | — | `Bpe` | step | 2.68 | 1.74 | +54% | 30% |
| `LcsGateBenchmarks` | Band=96 | `Kernel` | step | 0.03 | 0.06 | -50% | 50% |
| `LcsGateBenchmarks` | Band=48 | `Kernel` | drift | 0.05 | 0.04 | +25% | 20% |
