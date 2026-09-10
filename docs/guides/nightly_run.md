# Nightly benchmark run

<!-- nightly-baseline: 5ffb8e811e09fb49b19db58c1c94ed7637b15c16 -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`
- Previous run: `5ffb8e811e09fb49b19db58c1c94ed7637b15c16`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `DecompositionBenchmarks`
- `OlsBenchmarks`
- `StatsBenchmarks`

### Lodestar.Stats.Benchmarks.OlsBenchmarks-report-github

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

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

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

| Method                                    | Mean      | Error     | StdDev   | Ratio | RatioSD |
|------------------------------------------ |----------:|----------:|---------:|------:|--------:|
| TruncatedSvd_Rank20                       |  28.20 ms |  0.682 ms | 0.037 ms |  1.00 |    0.00 |
| Nmf_Rank20                                | 206.07 ms | 45.689 ms | 2.504 ms |  7.31 |    0.08 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  23.92 ms |  0.646 ms | 0.035 ms |  0.85 |    0.00 |

<!-- markdownlint-enable MD060 -->
